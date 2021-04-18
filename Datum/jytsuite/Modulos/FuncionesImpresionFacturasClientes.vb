Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Drawing.Printing
Imports System.Text
Imports RawPrinterHeper
Module FuncionesImpresionFacturasClientes

    Private ft As New Transportables
    Public Sub ImprimirFacturaGraficaBUHITO(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                              ByVal ds As DataSet, ByVal NumeroFactura As String, Optional PideParametros As Boolean = True)

        Dim nTablaEncabezado As String = "tblEncabezadoR"
        Dim nTablaRenglones As String = "tblRenglonesR"
        Dim nTablaIVA As String = "tblIVAR"
        Dim nTablaDescuentos As String = "tblDescuentosR"

        Dim dtRenglones As DataTable
        Dim dtEncab As DataTable
        Dim dtIVA As DataTable
        Dim dtDescuentos As DataTable

        Dim nLineasFactura As Integer = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA16"))
        Dim nLinea As Integer = 0
        Dim nRenglones As Integer

        Dim Imprime As Boolean = False

        ds = DataSetRequery(ds, " select a.numfac, a.emision, a.vence, a.codcli, b.nombre, b.rif, b.dirfiscal, b.telef1, b.ruta_visita, a.comen, a.codven, " _
                             & " concat(c.nombres, ' ', c.apellidos) vendedor, " _
                             & " a.condpag, a.transporte, a.tot_fac, a.imp_iva, a.descuen, a.tot_net " _
                             & " from jsvenencfac a " _
                             & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp ) " _
                             & " left join jsvencatven c on (a.codven = c.codven and a.id_emp = c.id_emp ) " _
                             & " where " _
                             & " a.numfac = '" & NumeroFactura & "' and " _
                             & " a.id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaEncabezado, lblInfo)

        ds = DataSetRequery(ds, " select a.numfac, a.item, b.alterno, a.renglon, a.descrip, a.cantidad, a.unidad, a.peso, a.iva, a.precio, " _
                             & " a.des_art, a.des_cli, a.des_ofe, a.totren, elt(a.estatus+1,'', 'SD', 'BN') estatus " _
                             & " from jsvenrenfac a " _
                             & " LEFT JOIN jsmerctainv b on (a.item = b.codart and a.id_emp = b.id_emp) " _
                             & " where " _
                             & " a.numfac = '" & NumeroFactura & "' and " _
                             & " a.id_emp = '" & jytsistema.WorkID & "' " _
                             & " order by a.estatus, a.renglon ", MyConn, nTablaRenglones, lblInfo)

        ds = DataSetRequery(ds, " select * from jsvendesfac where " _
                            & " numfac = '" & NumeroFactura & "' and " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'", MyConn, nTablaDescuentos, lblInfo)

        ds = DataSetRequery(ds, " SELECT numfac, '' tipoiva, poriva, SUM(baseiva) baseiva, SUM(IMPIVA) impiva, ejercicio, id_emp " _
                                & " From jsvenivafac " _
                                & " Where " _
                                & " numfac = '" & NumeroFactura & "' AND " _
                                & " tipoiva IN ('','E') AND " _
                                & " ID_EMP = '" & jytsistema.WorkID & "' " _
                                & " gROUP BY tipoiva " _
                                & " Union " _
                                & " SELECT numfac, tipoiva, poriva, baseiva, impiva, ejercicio, id_emp " _
                                & " From jsvenivafac " _
                                & " Where " _
                                & " numfac = '" & NumeroFactura & "' AND " _
                                & " tipoiva NOT IN ('','E') AND " _
                                & " ID_EMP = '" & jytsistema.WorkID & "' " _
                                & " ORDER BY tipoiva ", MyConn, nTablaIVA, lblInfo)

        dtEncab = ds.Tables(nTablaEncabezado)
        dtRenglones = ds.Tables(nTablaRenglones)
        dtDescuentos = ds.Tables(nTablaDescuentos)
        dtIVA = ds.Tables(nTablaIVA)

        Dim pd As New PrintDialog()
        pd.PrinterSettings = New PrinterSettings()
        pd.PrinterSettings.Copies = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA20"))

        If PideParametros Then
            If (pd.ShowDialog() = DialogResult.OK) Then
                Imprime = True
            Else
                Imprime = False
            End If
        Else
            Imprime = True
        End If

        If Imprime Then
            Dim sb As New StringBuilder()
            '///// ENCABEZADO DE FACTURA 
            Dim FechaFactura As Date = CDate(dtEncab.Rows(0).Item("emision").ToString)
            sb = ImprimeEncabezadoFacturaBUHITO(MyConn, lblInfo, sb, dtEncab)

            '///// RENGLONES DE FACTURA
            For nRenglones = 0 To dtRenglones.Rows.Count - 1
                With dtRenglones.Rows(nRenglones)

                    If ft.DevuelveScalarEntero(MyConn, " SELECT COUNT(*) FROM jsmercatcom where codart = '" & .Item("item") & "' and id_emp = '" & jytsistema.WorkID & "' ") > 0 Then ' COMBO
                        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA28")) Then 'IMPRIME COMBO SEPARADO
                            '///// MERCANCIAS DE COMBO
                            Dim nTablaCombo As String = "tblCombo"
                            Dim dtCombo As DataTable
                            ds = DataSetRequery(ds, "select * from jsmercatcom where codart = '" & .Item("ITEM") & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaCombo, lblInfo)
                            dtCombo = ds.Tables(nTablaCombo)
                            If dtCombo.Rows.Count > 0 Then

                                For nCombo As Integer = 0 To dtCombo.Rows.Count - 1

                                    Dim cItem As String = dtCombo.Rows(nCombo).Item("CODARTCOM")
                                    Dim cDescrip As String = dtCombo.Rows(nCombo).Item("descrip")
                                    Dim cCantidad As Double = .Item("cantidad") * dtCombo.Rows(nCombo).Item("cantidad")
                                    Dim cUnidad As String = dtCombo.Rows(nCombo).Item("unidad")
                                    Dim cPeso As Double = cCantidad * dtCombo.Rows(nCombo).Item("peso") / dtCombo.Rows(nCombo).Item("cantidad")
                                    Dim cIVA As String = ft.DevuelveScalarCadena(MyConn, " select IVA from jsmerctainv where codart = '" + cItem + "' and id_emp = '" + jytsistema.WorkID + "'  ")
                                    Dim Tarifa As String = ft.DevuelveScalarCadena(MyConn, " select TARIFA from jsvenencfac where numfac = '" + NumeroFactura + "' and id_emp = '" + jytsistema.WorkID + "'  ")
                                    Dim cPrecio As Double = ft.DevuelveScalarCadena(MyConn, " select PRECIO_" + Tarifa + " from jsmerctainv where codart = '" + cItem + "' and id_emp = '" + jytsistema.WorkID + "'  ")
                                    Dim cTotalRenglon As Double = cPrecio * cCantidad

                                    cTotalRenglon = CalculaDescuento(cTotalRenglon, .Item("des_art"))
                                    cTotalRenglon = CalculaDescuento(cTotalRenglon, .Item("des_cli"))
                                    cTotalRenglon = CalculaDescuento(cTotalRenglon, .Item("des_ofe"))

                                    Dim aCad As String = cItem & "|" & cDescrip & "|" & ft.FormatoEntero(cCantidad) & "|" & _
                                        ft.FormatoNumero(PorcentajeIVA(MyConn, lblInfo, FechaFactura, cIVA)) & "|" & _
                                        ft.FormatoNumero(cPrecio) & "|" & ft.FormatoNumero(.Item("des_art")) & "|" & _
                                        ft.FormatoNumero(.Item("des_cli")) & "|" & ft.FormatoNumero(.Item("des_ofe")) & "|" & _
                                        ft.FormatoNumero(cTotalRenglon) & "|" & .Item("estatus")

                                    Dim aAnc As String = "12|12|60|10|7|11|7|7|7|12|2"
                                    Dim aAli As String = "I|I|I|D|D|D|D|D|D|D|C"
                                    sb = ImprimirLineaGrafica(sb, aCad, aAnc, aAli)

                                Next

                            End If

                            dtCombo.Dispose()
                            dtCombo = Nothing
                            '///// FIN MERCANCIAS DE COMBO
                        Else 'IMPRIME COMBO EN UN SOLO RENGLON CON DESCRIPCION DEL COMBO
                            Dim aCad As String = .Item("item") & "|" & .Item("descrip") & "|" & ft.FormatoCantidad(.Item("cantidad")) & "|" & .Item("unidad") & "|" & _
                                                                                                                   ft.FormatoCantidad(.Item("peso")) & "|" & ft.FormatoNumero(PorcentajeIVA(MyConn, lblInfo, FechaFactura, .Item("IVA"))) & "|" & _
                                                                                                                   ft.FormatoNumero(.Item("precio")) & "|" & ft.FormatoNumero(.Item("des_art")) & "|" & ft.FormatoNumero(.Item("des_cli")) & "|" & ft.FormatoNumero(.Item("des_ofe")) & "|" & _
                                                                                                                   ft.FormatoNumero(.Item("totren")) & "|" & .Item("estatus")

                            Dim aAnc As String = "12|60|10|5|10|7|11|7|7|7|12|2"
                            Dim aAli As String = "I|I|D|D|D|D|D|D|D|D|D|C"
                            sb = ImprimirLineaGrafica(sb, aCad, aAnc, aAli)
                            '///// MERCANCIAS DE COMBO
                            Dim nTablaCombo As String = "tblCombo"
                            Dim dtCombo As DataTable
                            ds = DataSetRequery(ds, "select * from jsmercatcom where codart = '" & .Item("ITEM") & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaCombo, lblInfo)
                            dtCombo = ds.Tables(nTablaCombo)
                            If dtCombo.Rows.Count > 0 Then
                                Dim nCombo As Integer
                                sb = ImprimirLineaGrafica(sb, "MERCANCIAS DEL COMBO...", "150", "D")
                                For nCombo = 0 To dtCombo.Rows.Count - 1
                                    With dtCombo.Rows(nCombo)
                                        sb = ImprimirLineaGrafica(sb, .Item("item") & "|" & .Item("descrip") & "|" & ft.FormatoCantidad(.Item("cantidad")) & "|" & .Item("unidad") & "|" & _
                                                    ft.FormatoCantidad(.Item("peso")) & "|", "12|60|10|5|10|53", "I|I|D|D|D|D")
                                    End With
                                Next
                            End If
                            dtCombo.Dispose()
                            dtCombo = Nothing
                            '///// FIN MERCANCIAS DE COMBO
                        End If
                    Else

                        Dim aCad As String = .Item("item") & "|" & .Item("alterno") & "|" & .Item("descrip") & "|" & _
                            ft.FormatoEntero(.Item("cantidad")) & "|" & ft.FormatoNumero(PorcentajeIVA(MyConn, lblInfo, FechaFactura, .Item("IVA"))) & "|" & _
                            ft.FormatoNumero(.Item("precio")) & "|" & ft.FormatoNumero(.Item("des_art")) & "|" & _
                            ft.FormatoNumero(.Item("des_cli")) & "|" & _
                            ft.FormatoNumero(.Item("totren")) & "|" & .Item("estatus")

                        Dim aAnc As String = "12|12|63|10|7|13|7|7|17|2"
                        Dim aAli As String = "I|I|I|C|D|D|D|D|D|C"
                        sb = ImprimirLineaGrafica(sb, aCad, aAnc, aAli)


                    End If



                    '///// COMENTARIOS DE RENGLON DE FACTURA
                    Dim Comentario_Renglon As String = ft.DevuelveScalarCadena(MyConn, "select comentario from jsvenrencom where " _
                                                                             & " numdoc = '" & .Item("numfac") & "' and " _
                                                                             & " origen = 'FAC' and " _
                                                                             & " item = '" & .Item("item") & "' and " _
                                                                             & " renglon = '" & .Item("renglon") & "' and " _
                                                                             & " id_emp = '" & jytsistema.WorkID & "' ")

                    If Comentario_Renglon = "" Then Comentario_Renglon = ""

                    If Len(Comentario_Renglon) > 0 Then
                        Dim comen As Boolean = True
                        Dim inicioLinea As Integer = 0
                        Dim LongitudLinea As Integer = 60
                        While comen
                            Dim Comentario As String = Mid(Comentario_Renglon, inicioLinea + 1, LongitudLinea)
                            sb = ImprimirLineaGrafica(sb, "|" & Comentario & "|", "20|60|70", "I|I|I")
                            inicioLinea = inicioLinea + LongitudLinea
                            If inicioLinea >= Len(Comentario_Renglon) Then comen = False
                        End While
                    End If
                    '///// FIN COMENTARIOS RENGLON DE FACTURA



                End With
            Next
            '///// SUBTOTAL FACTURA
            sb = ImprimirLineaGrafica(sb, "|----------------------------------------------------------------", "65|85", "D|D")
            Dim nItems As String = ft.FormatoEntero(ft.DevuelveScalarEntero(MyConn, "select SUM(CANTIDAD) from jsvenrenfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac "))
            sb = ImprimirLineaGrafica(sb, "SUBTOTAL :|" & nItems & "||" & ft.FormatoNumero(ft.DevuelveScalarDoble(MyConn, "Select SUM(totren) FROM jsvenrenfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac")) & "|", _
                                      "87|10|39|12|2", _
                                      "D|C|D|D|C")


            '///// LIneas Adicionales para total
            Dim gCont As Integer
            Dim TotalLineas As Integer = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA16")) 'CANTIDAD DE LINEAS ESPECIFICAS POR LARGO DE PAPEL Y POR MODO DE IMPRESORA (DRAFT/GRAFICA)
            Dim TotalLineasFactura As Integer = NumeroLineasEnFactura(MyConn, lblInfo, NumeroFactura)
            For gCont = 1 To TotalLineas - TotalLineasFactura
                sb = ImprimirLineaGrafica(sb, "", "150", "D")
            Next

            '///// DESCUENTOS FACTURA
            If dtDescuentos.Rows.Count > 0 Then
                Dim nDescuentos As Integer
                For nDescuentos = 0 To dtDescuentos.Rows.Count - 1
                    With dtDescuentos.Rows(nDescuentos)
                        sb = ImprimirLineaGrafica(sb, .Item("descrip") & "|" & ft.FormatoNumero(.Item("pordes")) & "%|" & ft.FormatoNumero(.Item("descuento")) & "|", "114|10|12|14", "D|D|D|C")
                    End With
                Next
            End If
            sb = ImprimirLineaGrafica(sb, "|----------------------------------------------------------------", "70|80", "D|D")
            sb = ImprimirLineaGrafica(sb, "|TOTAL DESCUENTOS :|" & ft.FormatoNumero(ft.DevuelveScalarDoble(MyConn, " select SUM(DESCUENTO) from jsvendesfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac ")) & "|", "96|40|12|2", "D|D|D|C")
            '///// FIN DESCUENTOS FACTURA

            '///// IMPUESTO AL VALOR AGREGADO
            If dtIVA.Rows.Count > 0 Then
                Dim nIVA As Integer
                For nIVA = 0 To dtIVA.Rows.Count - 1
                    With dtIVA.Rows(nIVA)
                        If .Item("tipoiva") = "" Then
                            sb = ImprimirLineaGrafica(sb, "|Monto exento o exonerado de IVA :|" & ft.FormatoNumero(.Item("BASEIVA")) & "|", "40|42|12|56", "D|D|D|I")
                        Else
                            '///sb = ImprimirLineaGrafica(sb, "|Base imponible IVA según alícuota " & ft.FormatoNumero(.Item("PORIVA")) & "% :|" & ft.FormatoNumero(.Item("BASEIVA")) & "|Total IVA según alícuota|" & ft.FormatoNumero(.Item("PORIVA")) & "% :|" & ft.FormatoNumero(.Item("IMPIVA")) & "|", "40|42|12|32|10|12|2", "I|D|D|D|D|D|I")

                            '//// DECRETO 3085
                            Dim BaseImponible As Double = .Item("BASEIVA")
                            Select Case .Item("TIPOIVA")
                                Case "A"
                                    sb = ImprimirLineaGrafica(sb, "|Base imponible IVA según alícuota " & ft.FormatoNumero(.Item("PORIVA")) & "% :|" & ft.FormatoNumero(.Item("BASEIVA")) & "|Total IVA según alícuota|" & ft.FormatoNumero(.Item("PORIVA")) & "% :|" & ft.FormatoNumero(.Item("IMPIVA")) & "|", _
                                                              "20|62|12|32|10|12|2", _
                                                              "I|D|D|D|D|D|I")
                                Case "B"
                                    sb = ImprimirLineaGrafica(sb, "|Base imponible IVA según alícuota 12.00% :|" & ft.FormatoNumero(BaseImponible) & "|Total IVA según alícuota|12.00% :|" & ft.FormatoNumero(Math.Round(BaseImponible * 0.12, 2)) & "|", _
                                                              "20|62|12|32|10|12|2", _
                                                              "I|D|D|D|D|D|I")
                                    sb = ImprimirLineaGrafica(sb, "|REBAJA de la tasa de IVA según Decreto N° 3085 (" & ft.FormatoNumero(5) & "%) :| " & ft.FormatoNumero(-1 * Math.Round(BaseImponible * 0.05, 2)) & "|Total IVA (12%-5%):|" & ft.FormatoNumero(.Item("IMPIVA")) & "|", _
                                                              "20|62|12|42|12|2", _
                                                              "I|D|D|D|D|I")
                                Case "C"
                                    sb = ImprimirLineaGrafica(sb, "|Base imponible IVA según alícuota 12.00% :|" & ft.FormatoNumero(BaseImponible) & "|Total IVA según alícuota|12.00% :|" & ft.FormatoNumero(Math.Round(BaseImponible * 0.12, 2)) & "|", _
                                                              "20|62|12|32|10|12|2", _
                                                              "I|D|D|D|D|D|I")
                                    sb = ImprimirLineaGrafica(sb, "|REBAJA de la tasa de IVA según Decreto N° 3085 (" & ft.FormatoNumero(3) & "%) :|" & ft.FormatoNumero(-1 * Math.Round(BaseImponible * 0.03, 2)) & "|Total IVA (12%-3%):|" & ft.FormatoNumero(.Item("IMPIVA")) & "|", _
                                                              "20|62|12|42|12|2", _
                                                              "I|D|D|D|D|I")
                            End Select

                        End If
                    End With
                Next
            End If
            '///// FIN IMPUESTO AL VALOR AGREGADO

            '///// PIE DE FACTURA
            Dim Son As String = NumerosATexto(dtEncab.Rows(0).Item("TOT_FAC"))
            sb = ImprimirLineaGrafica(sb, "________________________________________", "150", "I")
            sb = ImprimirLineaGrafica(sb, "        FIRMA Y SELLO DEL CLIENTE ", "150", "I")

            sb = ImprimirLineaGrafica(sb, "ACEPTADO PARA SER PAGADO A SU VENCIMIENTO|TOTAL FACTURA >>> Referencias : " & ft.FormatoEntero(dtRenglones.Rows.Count) & _
                                      ",  Items : " & nItems & "     " & "|MONTO TOTAL DE LA VENTA :|" & ft.FormatoNumero(dtEncab.Rows(0).Item("TOT_FAC")) & "|", _
                                      "60|51|25|12|2", _
                                      "I|D|D|D|C")
            sb = ImprimirLineaGrafica(sb, "|----------------------------------------------------------------", "80|70", "I|D")
            sb = ImprimirLineaGrafica(sb, "SON : " & Son, "150", "D")

            '/////////// COMENTARIOS DE FACTURA
            Dim nTablaComentarios As String = "tblComentarios"
            Dim dtComentarios As DataTable
            ds = DataSetRequery(ds, "select * from jsconctacom where origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "'", MyConn, nTablaComentarios, lblInfo)
            dtComentarios = ds.Tables(nTablaComentarios)
            If dtComentarios.Rows.Count > 0 Then
                Dim nComen As Integer
                For nComen = 0 To dtComentarios.Rows.Count - 1
                    With dtComentarios.Rows(nComen)
                        sb = ImprimirLineaGrafica(sb, .Item("comentario"), "150", "I")
                    End With
                Next
            End If
            dtComentarios.Dispose()
            dtComentarios = Nothing
            '///// FIN PIE DE FACTURA

            '///// CIERRE E IMPRESION DE FACTURA
            ImprimeDocumentoRAW(MyConn, lblInfo, sb, pd, CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA22")), _
                                CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA14")))

            '//// ACTUALIZAR BIT IMPRESION EN FACTURA
            ft.Ejecutar_strSQL(MyConn, " update jsvenencfac set impresa = '1' where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")
            '//// ACTUALIZAR NUMERO DE CONTROL"
            Dim FechaIvaFactura As Date = ft.DevuelveScalarFecha(MyConn, "select emision from jsvenencfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ").ToString
            ActualizaNumeroControl(False, MyConn, lblInfo, NumeroFactura, FechaIvaFactura, "FAC", "FAC", dtEncab.Rows(0).Item("codcli").ToString)


            sb = Nothing

        End If

        dtRenglones.Dispose()
        dtEncab.Dispose()
        dtIVA.Dispose()
        dtDescuentos.Dispose()
        dtEncab = Nothing
        dtRenglones = Nothing
        dtIVA = Nothing
        dtDescuentos = Nothing


    End Sub

    Private Function ImprimeEncabezadoFacturaBUHITO(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal sb As StringBuilder, _
                                             ByVal dtEncab As DataTable) As StringBuilder
        Dim gCont As Integer
        Dim BeginingLine As Integer = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA18"))
        For gCont = 1 To BeginingLine
            sb = ImprimirLineaGrafica(sb, "", "150", "D")
        Next

        With dtEncab.Rows(0)
            Dim Cliente As String = .Item("nombre")
            Dim CondicionPago As String = IIf(CInt(.Item("condpag")) = 0, "CREDITO CON VENCIMIENTO AL " & ft.FormatoFecha(CDate(.Item("vence").ToString)), "CONTADO")

            sb = ImprimirLineaGrafica(sb, "F A C T U R A", "150", "D")
            sb = ImprimirLineaGrafica(sb, "NOMBRE CLIENTE O RAZON SOCIAL : " & Cliente & "|No " & .Item("numfac"), "130|20", "I|D")
            sb = ImprimirLineaGrafica(sb, "RIF O CI O PASAPORTE No : " & .Item("RIF") & "|FECHA DE EMISION : " & Format(CDate(.Item("emision").ToString), "dd-MM-yyyy"), "90|60", "I|D")
            sb = ImprimirLineaGrafica(sb, "DOMICILIO FISCAL : " & .Item("dirfiscal") & "|Teléf.: " & .Item("telef1"), "130|20", "I|D")
            sb = ImprimirLineaGrafica(sb, "------------------------------------------------------------------------------------------------------------------------------------------------------", "150", "C")
            sb = ImprimirLineaGrafica(sb, "Cod. Cliente: " & .Item("codcli") & "   Condición pago: " & CondicionPago & "   Asesor: " & .Item("codven") & " " & Left(.Item("vendedor"), 25) & "|Ruta: " & .Item("ruta_visita"), "138|12", "I|D")
            sb = ImprimirLineaGrafica(sb, "------------------------------------------------------------------------------------------------------------------------------------------------------", "150", "C")
            sb = ImprimirLineaGrafica(sb, "ITEM|REFERENCIA|DESCRIPCION|CANTIDAD     IVA      PRECIO      DESCUENTOS           TOTAL   TP", "12|12|60|66", "I|I|I|D")
            sb = ImprimirLineaGrafica(sb, "------------------------------------------------------------------------------------------------------------------------------------------------------", "150", "C")

        End With

        Return sb

    End Function

End Module
