Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Drawing.Printing
Imports System.Text
Imports RawPrinterHeper
Module FuncionesReportesGraficos
    Private ft As New Transportables

    Public Sub ImprimirFacturaGrafica(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
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

        ds = DataSetRequery(ds, " select numfac, item, renglon, descrip, cantidad, unidad, peso, iva, precio, " _
                             & " des_art, des_cli, des_ofe, totren, elt(estatus+1,'', 'SD', 'BN') estatus " _
                             & " from jsvenrenfac " _
                             & " where " _
                             & " numfac = '" & NumeroFactura & "' and " _
                             & " id_emp = '" & jytsistema.WorkID & "' " _
                             & " order by estatus, renglon ", MyConn, nTablaRenglones, lblInfo)

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
            sb = ImprimeEncabezadoFactura(MyConn, lblInfo, sb, dtEncab)

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
                                    Dim cDescrip As String = ft.NormalizeString(dtCombo.Rows(nCombo).Item("descrip").ToString, 56)
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

                                    Dim aCad As String = cItem & "|" & cDescrip & "|" & ft.FormatoCantidad(cCantidad) & "|" & _
                                        .Item("unidad") & "|" & ft.FormatoCantidad(cPeso) & "|" & _
                                        ft.FormatoNumero(PorcentajeIVA(MyConn, lblInfo, FechaFactura, cIVA)) & "|" & _
                                        ft.FormatoNumero(cPrecio) & "|" & ft.FormatoNumero(.Item("des_art")) & "|" & _
                                        ft.FormatoNumero(.Item("des_cli")) & "|" & ft.FormatoNumero(.Item("des_ofe")) & "|" & _
                                        ft.FormatoNumero(cTotalRenglon) & "|" & .Item("estatus")

                                    Dim aAnc As String = "12|56|10|5|10|7|12|7|7|7|15|2"
                                    Dim aAli As String = "I|I|D|D|D|D|D|D|D|D|D|C"
                                    sb = ImprimirLineaGrafica(sb, aCad, aAnc, aAli)

                                Next

                            End If

                            dtCombo.Dispose()
                            dtCombo = Nothing
                            '///// FIN MERCANCIAS DE COMBO
                        Else 'IMPRIME COMBO EN UN SOLO RENGLON CON DESCRIPCION DEL COMBO
                            Dim aCad As String = .Item("item") & "|" & ft.NormalizeString(.Item("descrip"), 56) & "|" & ft.FormatoCantidad(.Item("cantidad")) & "|" & .Item("unidad") & "|" & _
                                                                                                                   ft.FormatoCantidad(.Item("peso")) & "|" & ft.FormatoNumero(PorcentajeIVA(MyConn, lblInfo, FechaFactura, .Item("IVA"))) & "|" & _
                                                                                                                   ft.FormatoNumero(.Item("precio")) & "|" & ft.FormatoNumero(.Item("des_art")) & "|" & ft.FormatoNumero(.Item("des_cli")) & "|" & ft.FormatoNumero(.Item("des_ofe")) & "|" & _
                                                                                                                   ft.FormatoNumero(.Item("totren")) & "|" & .Item("estatus")

                            Dim aAnc As String = "12|56|10|5|10|7|12|7|7|7|15|2"
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
                                        sb = ImprimirLineaGrafica(sb, .Item("item") & "|" & ft.NormalizeString(.Item("descrip"), 60) & "|" & ft.FormatoCantidad(.Item("cantidad")) & "|" & .Item("unidad") & "|" & _
                                                    ft.FormatoCantidad(.Item("peso")) & "|", "12|60|10|5|10|53", "I|I|D|D|D|D")
                                    End With
                                Next
                            End If
                            dtCombo.Dispose()
                            dtCombo = Nothing
                            '///// FIN MERCANCIAS DE COMBO
                        End If
                    Else
                        Dim aCad As String = .Item("item") & "|" & ft.NormalizeString(.Item("descrip"), 56) & "|" & ft.FormatoCantidad(.Item("cantidad")) & "|" & .Item("unidad") & "|" & _
                                                                                                                   ft.FormatoCantidad(.Item("peso")) & "|" & ft.FormatoNumero(PorcentajeIVA(MyConn, lblInfo, FechaFactura, .Item("IVA"))) & "|" & _
                                                                                                                   ft.FormatoNumero(.Item("precio")) & "|" & ft.FormatoNumero(.Item("des_ofe")) & "|" & _
                                                                                                                   ft.FormatoNumero(.Item("totren")) & "|" & .Item("estatus")

                        Dim aAnc As String = "12|56|10|5|10|7|19|7|22|2"
                        Dim aAli As String = "I|I|D|D|D|D|D|D|D|C"
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
            sb = ImprimirLineaGrafica(sb, "|----------------------------------------------------------------", "70|80", "D|D")
            sb = ImprimirLineaGrafica(sb, "SUBTOTAL :|" & ft.FormatoCantidad(ft.DevuelveScalarDoble(MyConn, "select SUM(PESO) from jsvenrenfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac ")) & "||" & ft.FormatoNumero(ft.DevuelveScalarDoble(MyConn, "Select SUM(totren) FROM jsvenrenfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac")) & "|", "87|10|39|12|2", "D|D|D|D|C")


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
                            '//// ANTERIOR DECRETO 3085
                            'sb = ImprimirLineaGrafica(sb, "|Base imponible IVA según alícuota " & ft.FormatoNumero(.Item("PORIVA")) & "% :|" & ft.FormatoNumero(.Item("BASEIVA")) & "|Total IVA según alícuota|" & ft.FormatoNumero(.Item("PORIVA")) & "% :|" & ft.FormatoNumero(.Item("IMPIVA")) & "|", "40|42|12|32|10|12|2", "I|D|D|D|D|D|I")

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
            sb = ImprimirLineaGrafica(sb, "ACEPTADO PARA SER PAGADO A SU VENCIMIENTO|MONTO TOTAL DE LA VENTA :|" & ft.FormatoNumero(dtEncab.Rows(0).Item("TOT_FAC")) & "|", "111|25|12|2", "I|D|D|C")
            sb = ImprimirLineaGrafica(sb, "FORMA DE PAGO :___EFECTIVO ___ CHEQUE ___TARJETA DEBITO ___TARJETA CREDITO|----------------------------------------------------------------", "80|70", "I|D")
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
            ft.Ejecutar_strSQL(myconn, " update jsvenencfac set impresa = '1' where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")
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

    Public Sub ImprimirFacturaGraficaPOS(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                              ByVal ds As DataSet, ByVal NumeroFactura As String, Optional ByVal PideParametros As Boolean = True)

        Dim nTablaEncabezado As String = "tblEncabezadoR"
        Dim nTablaRenglones As String = "tblRenglonesR"
        Dim nTablaIVA As String = "tblIVAR"
        Dim nTablaDescuentos As String = "tblDescuentosR"

        Dim dtRenglones As DataTable
        Dim dtEncab As DataTable
        Dim dtIVA As DataTable
        Dim dtDescuentos As DataTable

        Dim nLineasFactura As Integer = CInt(ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM27"))
        Dim nLinea As Integer = 0
        Dim nRenglones As Integer

        Dim Imprime As Boolean = False

        ds = DataSetRequery(ds, " select a.numfac, a.emision, a.vence, a.codcli, a.nomcli nombre, a.rif, IF(a.comen = '', b.dirfiscal, a.comen) dirfiscal, " _
                             & " b.telef1, b.ruta_visita, a.comen, a.codven, " _
                             & " concat(c.nombres, ' ', c.apellidos) vendedor, " _
                             & " a.condpag, '' transporte, a.tot_fac, a.imp_iva, a.descuen, a.tot_net " _
                             & " from jsvenencpos a " _
                             & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp ) " _
                             & " left join jsvencatven c on (a.codven = c.codven and a.id_emp = c.id_emp ) " _
                             & " where " _
                             & " a.numfac = '" & NumeroFactura & "' and " _
                             & " a.id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaEncabezado, lblInfo)

        ds = DataSetRequery(ds, " select numfac, item, renglon, descrip, cantidad, unidad, peso, iva, precio, " _
                             & " des_art, des_cli, des_ofe, totren, elt(estatus+1,'', 'SD', 'BN') estatus " _
                             & " from jsvenrenpos " _
                             & " where " _
                             & " numfac = '" & NumeroFactura & "' and " _
                             & " id_emp = '" & jytsistema.WorkID & "' " _
                             & " order by estatus, renglon ", MyConn, nTablaRenglones, lblInfo)

        ds = DataSetRequery(ds, " select * from jsvendespos where " _
                            & " numfac = '" & NumeroFactura & "' and " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'", MyConn, nTablaDescuentos, lblInfo)

        ds = DataSetRequery(ds, " SELECT numfac, '' tipoiva, poriva, SUM(baseiva) baseiva, IFNULL(SUM(IMPIVA),0) impiva, ejercicio, id_emp " _
                                & " From jsvenivapos " _
                                & " Where " _
                                & " numfac = '" & NumeroFactura & "' AND " _
                                & " tipoiva IN ('','E') AND " _
                                & " ID_EMP = '" & jytsistema.WorkID & "' " _
                                & " gROUP BY tipoiva " _
                                & " Union " _
                                & " SELECT numfac, tipoiva, poriva, baseiva, impiva, ejercicio, id_emp " _
                                & " From jsvenivapos " _
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
        pd.PrinterSettings.Copies = CInt(ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM30"))

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
            sb = ImprimeEncabezadoFacturaPOS(MyConn, lblInfo, sb, dtEncab)

            '///// RENGLONES DE FACTURA
            For nRenglones = 0 To dtRenglones.Rows.Count - 1
                With dtRenglones.Rows(nRenglones)

                    Dim porIVA As Double = ft.DevuelveScalarDoble(MyConn, " SELECT PORIVA " _
                                                    & " FROM jsvenivapos " _
                                                    & " WHERE " _
                                                    & " NUMFAC = '" & .Item("numfac") & "' and " _
                                                    & " TIPOIVA = '" & .Item("IVA") & "' and " _
                                                    & " ID_EMP = '" & jytsistema.WorkID & "' ")

                    Dim aCad As String = .Item("item") & "|" & .Item("descrip") & "|" & ft.FormatoCantidad(.Item("cantidad")) & "|" & .Item("unidad") & "|" & _
                                                ft.FormatoCantidad(.Item("peso")) & "|" & ft.FormatoNumero(porIVA) & "|" & _
                                                ft.FormatoNumero(.Item("precio")) & "|" & ft.FormatoNumero(.Item("des_art")) & "|" & ft.FormatoNumero(.Item("des_cli")) & "|" & ft.FormatoNumero(.Item("des_ofe")) & "|" & _
                                                ft.FormatoNumero(.Item("totren")) & "|" & .Item("estatus")

                    Dim aAnc As String = "12|60|10|5|10|7|11|7|7|7|12|2"
                    Dim aAli As String = "I|I|D|D|D|D|D|D|D|D|D|C"
                    sb = ImprimirLineaGrafica(sb, aCad, aAnc, aAli)


                    '///// MERCANCIAS DE COMBO
                    Dim MercanciaCombo As Boolean = ft.DevuelveScalarBooleano(MyConn, "select tipoart from jsmerctainv where codart = '" & .Item("item") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If MercanciaCombo Then
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
                    End If
                    '///// FIN MERCANCIAS DE COMBO

                End With
            Next
            '///// SUBTOTAL FACTURA
            sb = ImprimirLineaGrafica(sb, "|----------------------------------------------------------------", "70|80", "D|D")
            sb = ImprimirLineaGrafica(sb, "SUBTOTAL :|" & ft.FormatoCantidad(ft.DevuelveScalarDoble(MyConn, "select SUM(PESO) from jsvenrenpos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac ")) & "||" & ft.FormatoNumero(ft.DevuelveScalarDoble(MyConn, "Select SUM(totren) FROM jsvenrenpos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac")) & "|", "87|10|39|12|2", "D|D|D|D|C")


            '///// LIneas Adicionales para total
            Dim gCont As Integer
            Dim TotalLineas As Integer = CInt(ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM27")) 'CANTIDAD DE LINEAS ESPECIFICAS POR LARGO DE PAPEL Y POR MODO DE IMPRESORA (DRAFT/GRAFICA)
            Dim TotalLineasFactura As Integer = NumeroLineasEnFacturaPOS(MyConn, lblInfo, NumeroFactura)
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
            sb = ImprimirLineaGrafica(sb, "|TOTAL DESCUENTOS :|" & ft.FormatoNumero(ft.DevuelveScalarDoble(MyConn, " select SUM(DESCUENTO) from jsvendespos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac ")) & "|", "96|40|12|2", "D|D|D|C")
            '///// FIN DESCUENTOS FACTURA

            '///// IMPUESTO AL VALOR AGREGADO
            If dtIVA.Rows.Count > 0 Then
                Dim nIVA As Integer
                For nIVA = 0 To dtIVA.Rows.Count - 1
                    With dtIVA.Rows(nIVA)
                        If .Item("tipoiva") = "" Then
                            sb = ImprimirLineaGrafica(sb, "|Monto exento o exonerado de IVA :|" & ft.FormatoNumero(.Item("BASEIVA")) & "|", "40|42|12|56", "D|D|D|I")
                        Else
                            sb = ImprimirLineaGrafica(sb, "|Base imponible IVA según alícuota " & ft.FormatoNumero(.Item("PORIVA")) & "% :|" & ft.FormatoNumero(.Item("BASEIVA")) & "|Total IVA según alícuota|" & ft.FormatoNumero(.Item("PORIVA")) & "% :|" & ft.FormatoNumero(.Item("IMPIVA")) & "|", "40|42|12|32|10|12|2", "I|D|D|D|D|D|I")
                        End If
                    End With
                Next
            End If
            '///// FIN IMPUESTO AL VALOR AGREGADO

            '///// PIE DE FACTURA
            Dim Son As String = NumerosATexto(dtEncab.Rows(0).Item("TOT_FAC"))
            sb = ImprimirLineaGrafica(sb, "________________________________________", "150", "I")
            sb = ImprimirLineaGrafica(sb, "        FIRMA Y SELLO DEL CLIENTE ", "150", "I")
            sb = ImprimirLineaGrafica(sb, "ACEPTADO PARA SER PAGADO A SU VENCIMIENTO|MONTO TOTAL DE LA VENTA :|" & ft.FormatoNumero(dtEncab.Rows(0).Item("TOT_FAC")) & "|", "111|25|12|2", "I|D|D|C")
            sb = ImprimirLineaGrafica(sb, "FORMA DE PAGO :___EFECTIVO ___ CHEQUE ___TARJETA DEBITO ___TARJETA CREDITO|----------------------------------------------------------------", "80|70", "I|D")
            sb = ImprimirLineaGrafica(sb, "SON : " & Son, "150", "D")

            '/////////// COMENTARIOS DE FACTURA
            'Dim nTablaComentarios As String = "tblComentarios"
            'Dim dtComentarios As DataTable
            'ds = DataSetRequery(ds, "select * from jsconctacom where origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "'", MyConn, nTablaComentarios, lblInfo)
            'dtComentarios = ds.Tables(nTablaComentarios)
            'If dtComentarios.Rows.Count > 0 Then
            '    Dim nComen As Integer
            '    For nComen = 0 To dtComentarios.Rows.Count - 1
            '        With dtComentarios.Rows(nComen)
            '            sb = ImprimirLineaGrafica(sb, .Item("comentario"), "150", "I")
            '        End With
            '    Next
            'End If
            'dtComentarios.Dispose()
            'dtComentarios = Nothing
            '///// FIN PIE DE FACTURA

            '///// CIERRE E IMPRESION DE FACTURA

            ImprimeDocumentoRAW(MyConn, lblInfo, sb, pd, CInt(ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM31")), _
                                 CInt(ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM26")))

            '//// ACTUALIZAR BIT IMPRESION EN FACTURA
            ft.Ejecutar_strSQL(myconn, " update jsvenencpos set impresa = '1' where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")
            '//// ACTUALIZAR NUMERO DE CONTROL"
            Dim FechaIvaFactura As Date = ft.DevuelveScalarFecha(MyConn, "select emision from jsvenencpos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ").ToString
            ActualizaNumeroControl(False, MyConn, lblInfo, NumeroFactura, FechaIvaFactura, "PVE", "PVE", dtEncab.Rows(0).Item("codcli").ToString)

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
    Public Sub ImprimeDocumentoRAW(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal sb As StringBuilder, _
                                ByVal pd As PrintDialog, ByVal nLineasPieDocumento As Integer, ByVal ModoImpresora As ModoImpresoraRAW)

        Dim mydocpath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Using outfile As New StreamWriter(mydocpath & "\DocumentoAImprimir.txt")
            outfile.Write(sb.ToString())
        End Using
        Dim s As String = ""
        Dim nLineas As Integer = 1
        Using sr As New StreamReader(mydocpath & "\DocumentoAImprimir.txt")
            Dim line As String
            Do
                line = sr.ReadLine()
                If Not (line Is Nothing) Then
                    s += line + vbCrLf
                End If
            Loop Until line Is Nothing
        End Using

        Dim nCopias As Integer

        For nCopias = 1 To pd.PrinterSettings.Copies
            If ModoImpresora = ModoImpresoraRAW.Draft Then

                'Dim ps As New PaperSize("Custom", 850, 733)
                'ps.PaperName = PaperKind.Custom
                'pd.PrinterSettings.DefaultPageSettings.PaperSize = ps

                Dim ss As String = pd.PrinterSettings.DefaultPageSettings.PaperSize.PaperName.ToString()
                Dim s1 As Integer = pd.PrinterSettings.DefaultPageSettings.PaperSize.Height


                EnviarReporteImpresoraEpsonESCP(pd.PrinterSettings.PrinterName, s, nLineasPieDocumento)
            Else
                Dim pr As New MyPrinter
                pd.PrinterSettings.DefaultPageSettings.Margins.Left = 10
                pr.prt(pd.PrinterSettings.PrinterName, s)
            End If
        Next


    End Sub


    Public Sub ImprimirNotaDebitoGrafica(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                             ByVal ds As DataSet, ByVal NumeroNotaDebito As String, Optional PideParametros As Boolean = True)

        Dim nTablaEncabezado As String = "tblEncabezadoR"
        Dim nTablaRenglones As String = "tblRenglonesR"
        Dim nTablaIVA As String = "tblIVAR"

        Dim dtRenglones As DataTable
        Dim dtEncab As DataTable
        Dim dtIVA As DataTable

        Dim nLineasFactura As Integer = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENNDBPA18"))

        Dim nLinea As Integer = 0
        Dim nRenglones As Integer

        Dim Imprime As Boolean = False

        ds = DataSetRequery(ds, "select a.numndb, a.emision, a.vence, a.codcli, b.nombre, b.rif, b.dirfiscal, b.telef1, b.ruta_visita, a.comen, a.codven, " _
                             & " concat(c.nombres, ' ', c.apellidos) vendedor, " _
                             & " a.transporte, a.tot_ndb, a.imp_iva, a.tot_net " _
                             & " from jsvenencndb a " _
                             & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp ) " _
                             & " left join jsvencatven c on (a.codven = c.codven and a.id_emp = c.id_emp ) " _
                             & " where " _
                             & " a.numndb = '" & NumeroNotaDebito & "' and " _
                             & " a.id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaEncabezado, lblInfo)

        ds = DataSetRequery(ds, " select numndb, item, renglon, descrip, cantidad, unidad, peso, iva, precio, " _
                             & " des_art, des_cli, des_ofe, totren, elt(estatus+1,'', 'SD', 'BN') estatus " _
                             & " from jsvenrenndb " _
                             & " where " _
                             & " numndb = '" & NumeroNotaDebito & "' and " _
                             & " id_emp = '" & jytsistema.WorkID & "' " _
                             & " order by estatus, renglon ", MyConn, nTablaRenglones, lblInfo)

        ds = DataSetRequery(ds, " SELECT numndb, '' tipoiva, poriva, SUM(baseiva) baseiva, IFNULL(SUM(IMPIVA),0) impiva, ejercicio, id_emp " _
                                & " From jsvenivandb " _
                                & " Where " _
                                & " numndb = '" & NumeroNotaDebito & "' AND " _
                                & " tipoiva IN ('','E') AND " _
                                & " ID_EMP = '" & jytsistema.WorkID & "' " _
                                & " gROUP BY tipoiva " _
                                & " Union " _
                                & " SELECT numndb, tipoiva, poriva, baseiva, impiva, ejercicio, id_emp " _
                                & " From jsvenivandb " _
                                & " Where " _
                                & " numndb = '" & NumeroNotaDebito & "' AND " _
                                & " tipoiva NOT IN ('','E') AND " _
                                & " ID_EMP = '" & jytsistema.WorkID & "' " _
                                & " ORDER BY tipoiva ", MyConn, nTablaIVA, lblInfo)

        dtEncab = ds.Tables(nTablaEncabezado)
        dtRenglones = ds.Tables(nTablaRenglones)
        dtIVA = ds.Tables(nTablaIVA)

        Dim pd As New PrintDialog()
        pd.PrinterSettings = New PrinterSettings()
        pd.PrinterSettings.Copies = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENNDBPA22"))

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
            '///// ENCABEZADO DE NOTA DEBITO
            Dim FechaNotaDebito As Date = CDate(dtEncab.Rows(0).Item("emision").ToString)
            sb = ImprimeEncabezadoNotaDebito(MyConn, lblInfo, sb, dtEncab)

            '///// RENGLONES DE NOTA DEBITO
            For nRenglones = 0 To dtRenglones.Rows.Count - 1
                With dtRenglones.Rows(nRenglones)
                    Dim aCad As String = .Item("item") & "|" & .Item("descrip") & "|" & ft.FormatoCantidad(.Item("cantidad")) & "|" & .Item("unidad") & "|" & _
                                                ft.FormatoNumero(PorcentajeIVA(MyConn, lblInfo, FechaNotaDebito, .Item("IVA"))) & "|" & _
                                                ft.FormatoNumero(.Item("precio")) & "|" & ft.FormatoNumero(.Item("des_art")) & "|" & ft.FormatoNumero(.Item("des_cli")) & "|" & ft.FormatoNumero(.Item("des_ofe")) & "|" & _
                                                ft.FormatoNumero(.Item("totren")) & "|" & .Item("estatus")

                    Dim aAnc As String = "12|60|10|15|7|11|7|7|7|12|2"
                    Dim aAli As String = "I|I|D|D|D|D|D|D|D|D|C"
                    sb = ImprimirLineaGrafica(sb, aCad, aAnc, aAli)

                    '///// COMENTARIOS DE RENGLON DE NOTA DEBITO
                    Dim Comentario_Renglon As String = ft.DevuelveScalarCadena(MyConn, "select comentario from jsvenrencom where " _
                                                                             & " numdoc = '" & .Item("numndb") & "' and " _
                                                                             & " origen = 'NDB' and " _
                                                                             & " item = '" & .Item("item") & "' and " _
                                                                             & " renglon = '" & .Item("renglon") & "' and " _
                                                                             & " id_emp = '" & jytsistema.WorkID & "' ")

                    If Comentario_Renglon = "0" Then Comentario_Renglon = ""

                    If Len(Comentario_Renglon) > 0 Then
                        Dim comen As Boolean = True
                        Dim inicioLinea As Integer = 0
                        Dim LongitudLinea As Integer = 80
                        While comen
                            Dim Comentario As String = Mid(Comentario_Renglon, inicioLinea + 1, LongitudLinea)
                            sb = ImprimirLineaGrafica(sb, "|" & Comentario & "|", "20|80|50", "D|D|I")
                            inicioLinea += LongitudLinea
                            If inicioLinea >= Len(Comentario_Renglon) Then comen = False
                        End While
                    End If
                    '///// FIN COMENTARIOS RENGLON DE NOTA DEBITO
                End With
            Next
            '///// SUBTOTAL NOTA DEBITO
            sb = ImprimirLineaGrafica(sb, "|----------------------------------------------------------------", "70|80", "D|D")
            sb = ImprimirLineaGrafica(sb, "SUBTOTAL :|" & ft.FormatoNumero(ft.DevuelveScalarDoble(MyConn, "Select SUM(totren) FROM jsvenrenndb where numndb = '" & NumeroNotaDebito & "' and id_emp = '" & jytsistema.WorkID & "' group by numndb")) & "|", "136|12|2", "D|D|C")


            '///// LIneas Adicionales para total
            Dim gCont As Integer
            Dim TotalLineas As Integer = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENNDBPA18")) 'CANTIDAD DE LINEAS ESPECIFICAS POR LARGO DE PAPEL Y POR MODO DE IMPRESORA (DRAFT/GRAFICA)
            Dim TotalLineasFactura As Integer = NumeroLineasEnNotasDebito(MyConn, lblInfo, NumeroNotaDebito)
            For gCont = 1 To TotalLineas - TotalLineasFactura
                sb = ImprimirLineaGrafica(sb, "", "150", "D")
            Next


            '///// IMPUESTO AL VALOR AGREGADO
            If dtIVA.Rows.Count > 0 Then
                Dim nIVA As Integer
                For nIVA = 0 To dtIVA.Rows.Count - 1
                    With dtIVA.Rows(nIVA)
                        If .Item("tipoiva") = "" Then
                            sb = ImprimirLineaGrafica(sb, "|Monto exento o exonerado de IVA :|" & ft.FormatoNumero(.Item("BASEIVA")) & "|", "40|42|12|56", "D|D|D|I")
                        Else
                            sb = ImprimirLineaGrafica(sb, "|Base imponible IVA según alícuota " & ft.FormatoNumero(.Item("PORIVA")) & "% :|" & ft.FormatoNumero(.Item("BASEIVA")) & "|Total IVA según alícuota|" & ft.FormatoNumero(.Item("PORIVA")) & "% :|" & ft.FormatoNumero(.Item("IMPIVA")) & "|", "40|42|12|32|10|12|2", "I|D|D|D|D|D|I")
                        End If
                    End With
                Next
            End If
            '///// FIN IMPUESTO AL VALOR AGREGADO

            '///// PIE DE NOTA  DEBITO
            Dim Son As String = NumerosATexto(dtEncab.Rows(0).Item("TOT_NDB"))
            sb = ImprimirLineaGrafica(sb, "________________________________________", "150", "I")
            sb = ImprimirLineaGrafica(sb, "        FIRMA Y SELLO DEL CLIENTE ", "150", "I")
            sb = ImprimirLineaGrafica(sb, "ACEPTADO PARA SER PAGADO A SU VENCIMIENTO|MONTO TOTAL NOTA DEBITO :|" & ft.FormatoNumero(dtEncab.Rows(0).Item("TOT_NDB")) & "|", "111|25|12|2", "I|D|D|C")
            sb = ImprimirLineaGrafica(sb, "FORMA DE PAGO :___EFECTIVO ___ CHEQUE ___TARJETA DEBITO ___TARJETA CREDITO|----------------------------------------------------------------", "80|70", "I|D")
            sb = ImprimirLineaGrafica(sb, "SON : " & Son, "150", "D")

            '/////////// COMENTARIOS NOTA DEBITO
            Dim nTablaComentarios As String = "tblComentarios"
            Dim dtComentarios As DataTable
            ds = DataSetRequery(ds, "select * from jsconctacom where origen = 'NDB' and id_emp = '" & jytsistema.WorkID & "'", MyConn, nTablaComentarios, lblInfo)
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
            '///// FIN PIE NOTA DEBITO

            '///// CIERRE E IMPRESION NOTA DEBITO
            ImprimeDocumentoRAW(MyConn, lblInfo, sb, pd, CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENNDBPA23")), _
                                CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENNDBPA16")))

            '//// ACTUALIZAR BIT IMPRESION EN NOTA DEBITO
            ft.Ejecutar_strSQL(myconn, " update jsvenencndb set impresa = '1' where numndb = '" & NumeroNotaDebito & "' and id_emp = '" & jytsistema.WorkID & "' ")
            '//// ACTUALIZAR NUMERO DE CONTROL"
            Dim FechaIvaNotaDebito As Date = ft.DevuelveScalarFecha(MyConn, "select emision from jsvenencndb where numndb = '" & NumeroNotaDebito & "' and id_emp = '" & jytsistema.WorkID & "' ").ToString
            ActualizaNumeroControl(False, MyConn, lblInfo, NumeroNotaDebito, FechaIvaNotaDebito, "NDB", "FAC", dtEncab.Rows(0).Item("codcli").ToString)


            sb = Nothing

        End If

        dtRenglones.Dispose()
        dtEncab.Dispose()
        dtIVA.Dispose()
        dtEncab = Nothing
        dtRenglones = Nothing
        dtIVA = Nothing


    End Sub


    Public Sub ImprimirNotaCreditoGrafica(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                              ByVal ds As DataSet, ByVal NumeroNotaCredito As String, Optional PideParametros As Boolean = False)

        Dim nTablaEncabezado As String = "tblEncabezadoR"
        Dim nTablaRenglones As String = "tblRenglonesR"
        Dim nTablaIVA As String = "tblIVAR"
        Dim nTablaDescuentos As String = "tblDescuentosR"
        Dim nTableComen As String = "tblComen"

        Dim dtRenglones As DataTable
        Dim dtEncab As DataTable
        Dim dtIVA As DataTable

        Dim nLineasNotaCredito As Integer = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENNCRPA16"))
        Dim nLinea As Integer = 0
        Dim nRenglones As Integer

        Dim Imprime As Boolean = False

        ds = DataSetRequery(ds, " select a.numncr, a.emision, a.vence, a.codcli, b.nombre, b.rif, b.dirfiscal, b.telef1, b.ruta_visita, a.comen, a.codven, " _
                             & " concat(c.nombres, ' ', c.apellidos) vendedor, " _
                             & " a.condpag, a.transporte, a.tot_ncr, a.imp_iva, a.tot_net, a.almacen " _
                             & " from jsvenencncr a " _
                             & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp ) " _
                             & " left join jsvencatven c on (a.codven = c.codven and a.id_emp = c.id_emp ) " _
                             & " where " _
                             & " a.numncr = '" & NumeroNotaCredito & "' and " _
                             & " a.id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaEncabezado, lblInfo)

        ds = DataSetRequery(ds, " select numncr, item, renglon, descrip, cantidad, unidad, peso, iva, precio, " _
                             & " POR_ACEPTA_DEV, CAUSA, totren, elt(estatus+1,'', 'SD', 'BN') estatus, NUMFAC " _
                             & " from jsvenrenncr " _
                             & " where " _
                             & " numncr = '" & NumeroNotaCredito & "' and " _
                             & " id_emp = '" & jytsistema.WorkID & "' " _
                             & " order by estatus, renglon ", MyConn, nTablaRenglones, lblInfo)

        ds = DataSetRequery(ds, " SELECT numncr, '' tipoiva, poriva, SUM(baseiva) baseiva, IFNULL(SUM(IMPIVA),0) impiva, ejercicio, id_emp " _
                                & " From jsvenivancr " _
                                & " Where " _
                                & " numncr = '" & NumeroNotaCredito & "' AND " _
                                & " tipoiva IN ('','E') AND " _
                                & " ID_EMP = '" & jytsistema.WorkID & "' " _
                                & " gROUP BY tipoiva " _
                                & " Union " _
                                & " SELECT numncr, tipoiva, poriva, baseiva, impiva, ejercicio, id_emp " _
                                & " From jsvenivancr " _
                                & " Where " _
                                & " numncr = '" & NumeroNotaCredito & "' AND " _
                                & " tipoiva NOT IN ('','E') AND " _
                                & " ID_EMP = '" & jytsistema.WorkID & "' " _
                                & " ORDER BY tipoiva ", MyConn, nTablaIVA, lblInfo)

        dtEncab = ds.Tables(nTablaEncabezado)
        dtRenglones = ds.Tables(nTablaRenglones)
        dtIVA = ds.Tables(nTablaIVA)

        Dim pd As New PrintDialog()
        pd.PrinterSettings = New PrinterSettings()
        pd.PrinterSettings.Copies = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENNCRPA20").ToString)

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
            '///// ENCABEZADO DE NOTA CREDITO
            Dim FechaNotaCredito As Date = CDate(dtEncab.Rows(0).Item("emision").ToString)
            sb = ImprimeEncabezadoNotaCredito(MyConn, lblInfo, sb, dtEncab)

            '///// RENGLONES DE NOTA CREDITO
            For nRenglones = 0 To dtRenglones.Rows.Count - 1
                With dtRenglones.Rows(nRenglones)
                    Dim aCad As String = .Item("item") & "|" & .Item("descrip") & "|" & ft.FormatoCantidad(.Item("cantidad")) & "|" & .Item("unidad") & "|" & _
                                                ft.FormatoCantidad(.Item("peso")) & "|" & ft.FormatoNumero(PorcentajeIVA(MyConn, lblInfo, FechaNotaCredito, .Item("IVA"))) & "|" & _
                                                ft.FormatoNumero(.Item("precio")) & "|" & ft.FormatoNumero(.Item("por_acepta_dev")) & "|" & .Item("causa") & "|" & _
                                                ft.FormatoNumero(.Item("totren")) & "|" & .Item("estatus")

                    Dim aAnc As String = "12|60|10|5|10|7|11|7|7|12|2"
                    Dim aAli As String = "I|I|D|D|D|D|D|D|D|D|C"
                    sb = ImprimirLineaGrafica(sb, aCad, aAnc, aAli)

                    '///// CAUSA DE RENGLON DE NOTA CREDITO
                    Dim Comentario_Renglon As String = "CAUSA : " & .Item("CAUSA") & " "
                    Comentario_Renglon += ft.DevuelveScalarCadena(MyConn, " select DESCRIP from jsvencaudcr where CODIGO = '" & .Item("causa") & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "'")
                    If .Item("numfac").ToString.Trim <> "" Then
                        If .Item("numfac").ToString.Substring(0, 2) <> "ND" Then
                            Comentario_Renglon += " N° FACTURA : " & .Item("NUMFAC") & " EMISION : " & ft.muestraCampoFecha(ft.DevuelveScalarFecha(MyConn, " SELECT EMISION from jsvenencfac where numfac = '" & .Item("numfac") & "' and id_emp = '" & jytsistema.WorkID & "' "))
                            Comentario_Renglon += " TOTAL : " & ft.FormatoNumero(ft.DevuelveScalarDoble(MyConn, " SELECT TOT_FAC from jsvenencfac where numfac = '" & .Item("numfac") & "' and id_emp = '" & jytsistema.WorkID & "' "))
                        Else
                            Comentario_Renglon += " N° NOTA DEBITO : " & .Item("NUMFAC") & " EMISION : " & ft.muestraCampoFecha(ft.DevuelveScalarFecha(MyConn, " SELECT EMISION from jsvenencndb where numndb = '" & .Item("numfac") & "' and id_emp = '" & jytsistema.WorkID & "' GROUP BY NUMNDB "))
                            Comentario_Renglon += " TOTAL : " & ft.FormatoNumero(ft.DevuelveScalarDoble(MyConn, " SELECT TOT_NDB from jsvenencndb where numndb = '" & .Item("numfac") & "' and id_emp = '" & jytsistema.WorkID & "' "))
                        End If
                    End If


                    If Comentario_Renglon = "0" Then Comentario_Renglon = ""

                    If Len(Comentario_Renglon) > 0 Then
                        Dim comen As Boolean = True
                        Dim inicioLinea As Integer = 0
                        Dim LongitudLinea As Integer = 110
                        While comen
                            Dim Comentario As String = Mid(Comentario_Renglon, inicioLinea + 1, LongitudLinea)
                            sb = ImprimirLineaGrafica(sb, "|" & Comentario & "|", "20|110|20", "I|I|I")
                            inicioLinea = inicioLinea + LongitudLinea
                            If inicioLinea >= Len(Comentario_Renglon) Then comen = False
                        End While
                    End If
                    '///// FIN CAUSA RENGLON DE CREDITO

                End With
            Next
            '///// SUBTOTAL NOTA CREDITO
            sb = ImprimirLineaGrafica(sb, "|----------------------------------------------------------------", "70|80", "D|D")
            sb = ImprimirLineaGrafica(sb, "SUBTOTAL :|" & ft.FormatoCantidad(ft.DevuelveScalarDoble(MyConn, "select SUM(PESO) from jsvenrenncr where numncr = '" & NumeroNotaCredito & "' and id_emp = '" & jytsistema.WorkID & "' group by numncr ")) & "||" & ft.FormatoNumero(ft.DevuelveScalarDoble(MyConn, "Select SUM(totren) FROM jsvenrenncr where numncr = '" & NumeroNotaCredito & "' and id_emp = '" & jytsistema.WorkID & "' group by numncr")) & "|", "87|10|39|12|2", "D|D|D|D|C")


            '///// LIneas Adicionales para total
            Dim gCont As Integer
            Dim TotalLineas As Integer = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENNCRPA16")) 'CANTIDAD DE LINEAS ESPECIFICAS POR LARGO DE PAPEL Y POR MODO DE IMPRESORA (DRAFT/GRAFICA)
            Dim TotalLineasNCR As Integer = NumeroLineasEnNotaCredito(MyConn, lblInfo, NumeroNotaCredito)
            For gCont = 1 To TotalLineas - TotalLineasNCR
                sb = ImprimirLineaGrafica(sb, "", "150", "D")
            Next


            '///// IMPUESTO AL VALOR AGREGADO
            If dtIVA.Rows.Count > 0 Then
                Dim nIVA As Integer
                For nIVA = 0 To dtIVA.Rows.Count - 1
                    With dtIVA.Rows(nIVA)
                        If .Item("tipoiva") = "" Then
                            sb = ImprimirLineaGrafica(sb, "|Monto exento o exonerado de IVA :|" & ft.FormatoNumero(.Item("BASEIVA")) & "|", "40|42|12|56", "D|D|D|I")
                        Else
                            sb = ImprimirLineaGrafica(sb, "|Base imponible IVA según alícuota " & ft.FormatoNumero(.Item("PORIVA")) & "% :|" & ft.FormatoNumero(.Item("BASEIVA")) & "|Total IVA según alícuota|" & ft.FormatoNumero(.Item("PORIVA")) & "% :|" & ft.FormatoNumero(.Item("IMPIVA")) & "|", "40|42|12|32|10|12|2", "I|D|D|D|D|D|I")
                        End If
                    End With
                Next
            End If
            '///// FIN IMPUESTO AL VALOR AGREGADO

            '///// PIE DE NOTA CREDITO
            Dim Son As String = NumerosATexto(dtEncab.Rows(0).Item("tot_ncr"))
            sb = ImprimirLineaGrafica(sb, "________________________________________|________________________________________", "75|75", "I|I")
            sb = ImprimirLineaGrafica(sb, " FIRMA Y/O SELLO CLIENTE OBLIGATORIO |        A.P. EMITIR N/C ", "75|75", "I|I")
            sb = ImprimirLineaGrafica(sb, ".|MONTO TOTAL NOTA DE CREDITO :|" & ft.FormatoNumero(dtEncab.Rows(0).Item("tot_ncr")) & "|", "100|26|12|2", "I|D|D|C")
            sb = ImprimirLineaGrafica(sb, "|----------------------------------------------------------------", "80|70", "I|D")
            sb = ImprimirLineaGrafica(sb, "SON : " & Son, "150", "D")

            '/////////// COMENTARIOS DE FACTURA
            Dim nTablaComentarios As String = "tblComentarios"
            Dim dtComentarios As DataTable
            ds = DataSetRequery(ds, "select * from jsconctacom where origen = 'NCR' and id_emp = '" & jytsistema.WorkID & "'", MyConn, nTablaComentarios, lblInfo)
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
            '///// FIN PIE DE NOTA CREDITO

            '///// CIERRE E IMPRESION DE FACTURA
            ImprimeDocumentoRAW(MyConn, lblInfo, sb, pd, CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENNCRPA22")), _
                               CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENNCRPA14")))

            '//// ACTUALIZAR BIT IMPRESION EN FACTURA
            ft.Ejecutar_strSQL(myconn, " update jsvenencncr set impresa = '1' where numncr = '" & NumeroNotaCredito & "' and id_emp = '" & jytsistema.WorkID & "' ")
            '//// ACTUALIZAR NUMERO DE CONTROL"
            Dim FechaIvaFactura As Date = ft.DevuelveScalarFecha(MyConn, " select emision from jsvenencncr where numncr = '" & NumeroNotaCredito & "' and id_emp = '" & jytsistema.WorkID & "' ").ToString
            ActualizaNumeroControl(False, MyConn, lblInfo, NumeroNotaCredito, FechaIvaFactura, "NCR", "FAC", dtEncab.Rows(0).Item("codcli").ToString)


            sb = Nothing

        End If

        dtRenglones.Dispose()
        dtEncab.Dispose()
        dtIVA.Dispose()
        ' dtDescuentos.Dispose()
        dtEncab = Nothing
        dtRenglones = Nothing
        dtIVA = Nothing
        'dtDescuentos = Nothing


    End Sub
    Private Function ImprimeEncabezadoFacturaPOS(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal sb As StringBuilder, _
                                          ByVal dtEncab As DataTable) As StringBuilder
        Dim gCont As Integer
        Dim BeginingLine As Integer = CInt(ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM29"))
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
            sb = ImprimirLineaGrafica(sb, "Cod. Cliente: " & .Item("codcli") & "   Condición pago: " & CondicionPago & "   Asesor: " & .Item("codven") & " " & Left(.Item("vendedor"), 25) & "|", "138|12", "I|D")
            sb = ImprimirLineaGrafica(sb, "------------------------------------------------------------------------------------------------------------------------------------------------------", "150", "C")
            sb = ImprimirLineaGrafica(sb, "ITEM|DESCRIPCION|CANTIDAD UND    KGS     IVA      PRECIO      DESCUENTOS           TOTAL   TP", "12|60|78", "I|I|D")
            sb = ImprimirLineaGrafica(sb, "------------------------------------------------------------------------------------------------------------------------------------------------------", "150", "C")

        End With
        ImprimeEncabezadoFacturaPOS = sb
    End Function


    Private Function ImprimeEncabezadoFactura(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal sb As StringBuilder, _
                                              ByVal dtEncab As DataTable) As StringBuilder
        Dim gCont As Integer
        Dim BeginingLine As Integer = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA18"))
        For gCont = 1 To BeginingLine
            sb = ImprimirLineaGrafica(sb, "", "150", "D")
        Next
        With dtEncab.Rows(0)
            Dim Cliente As String = .Item("nombre")
            Dim ImprimeContado As Boolean = CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA29"))
            Dim CondicionPago As String = IIf(CInt(.Item("condpag")) = 0, _
                                               IIf(ImprimeContado, "CONTADO", "CREDITO CON VENCIMIENTO AL " & ft.FormatoFecha(CDate(.Item("vence").ToString))), _
                                              "CONTADO")

            sb = ImprimirLineaGrafica(sb, "F A C T U R A", "150", "D")
            sb = ImprimirLineaGrafica(sb, "NOMBRE CLIENTE O RAZON SOCIAL : " & Cliente & "|No " & .Item("numfac"), "130|20", "I|D")
            sb = ImprimirLineaGrafica(sb, "RIF O CI O PASAPORTE No : " & .Item("RIF") & "|FECHA DE EMISION : " & Format(CDate(.Item("emision").ToString), "dd-MM-yyyy"), "90|60", "I|D")
            sb = ImprimirLineaGrafica(sb, "DOMICILIO FISCAL : " & .Item("dirfiscal") & "|Teléf.: " & .Item("telef1"), "130|20", "I|D")
            sb = ImprimirLineaGrafica(sb, "------------------------------------------------------------------------------------------------------------------------------------------------------", "150", "C")
            sb = ImprimirLineaGrafica(sb, "Cod. Cliente: " & ft.muestraCampoTexto(.Item("codcli")) & "   Condición pago: " & CondicionPago & "   Asesor: " & ft.muestraCampoTexto(.Item("codven")) & " " & ft.muestraCampoTexto(Left(.Item("vendedor"), 25)) & "|Ruta: " & ft.muestraCampoTexto(.Item("ruta_visita")), "138|12", "I|D")
            sb = ImprimirLineaGrafica(sb, "------------------------------------------------------------------------------------------------------------------------------------------------------", "150", "C")
            sb = ImprimirLineaGrafica(sb, "ITEM|DESCRIPCION|CANTIDAD UND    KGS     IVA      PRECIO      DESCUENTOS           TOTAL   TP", "12|56|82", "I|I|D")
            sb = ImprimirLineaGrafica(sb, "------------------------------------------------------------------------------------------------------------------------------------------------------", "150", "C")

        End With
        ImprimeEncabezadoFactura = sb
    End Function

    Private Function ImprimeEncabezadoNotaDebito(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal sb As StringBuilder, _
                                             ByVal dtEncab As DataTable) As StringBuilder
        Dim gCont As Integer
        Dim BeginingLine As Integer = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENNDBPA20"))
        For gCont = 1 To BeginingLine
            sb = ImprimirLineaGrafica(sb, "", "150", "D")
        Next
        With dtEncab.Rows(0)
            Dim Cliente As String = .Item("nombre")

            sb = ImprimirLineaGrafica(sb, "N O T A   D E B I T O", "150", "D")
            sb = ImprimirLineaGrafica(sb, "NOMBRE CLIENTE O RAZON SOCIAL : " & Cliente & "|No " & .Item("NUMNDB"), "130|20", "I|D")
            sb = ImprimirLineaGrafica(sb, "RIF O CI O PASAPORTE No : " & .Item("RIF") & "|FECHA DE EMISION : " & Format(CDate(.Item("emision").ToString), "dd-MM-yyyy"), "90|60", "I|D")
            sb = ImprimirLineaGrafica(sb, "DOMICILIO FISCAL : " & .Item("dirfiscal") & "|Teléf.: " & .Item("telef1"), "130|20", "I|D")
            sb = ImprimirLineaGrafica(sb, "------------------------------------------------------------------------------------------------------------------------------------------------------", "150", "C")
            sb = ImprimirLineaGrafica(sb, "Cod. Cliente: " & .Item("codcli") & "   Asesor: " & .Item("codven") & " " & Left(.Item("vendedor"), 25) & "|Ruta: " & .Item("ruta_visita"), "138|12", "I|D")
            sb = ImprimirLineaGrafica(sb, "------------------------------------------------------------------------------------------------------------------------------------------------------", "150", "C")
            sb = ImprimirLineaGrafica(sb, "ITEM|DESCRIPCION|CANTIDAD      UNIDAD    IVA      PRECIO      DESCUENTOS           TOTAL   TP", "12|60|78", "I|I|D")
            sb = ImprimirLineaGrafica(sb, "------------------------------------------------------------------------------------------------------------------------------------------------------", "150", "C")

        End With
        ImprimeEncabezadoNotaDebito = sb

    End Function

    Private Function ImprimeEncabezadoNotaCredito(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal sb As StringBuilder, _
                                             ByVal dtEncab As DataTable) As StringBuilder
        Dim gCont As Integer
        Dim BeginingLine As Integer = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENNCRPA18"))
        For gCont = 1 To BeginingLine
            sb = ImprimirLineaGrafica(sb, "", "150", "D")
        Next
        With dtEncab.Rows(0)
            Dim Cliente As String = .Item("nombre")

            sb = ImprimirLineaGrafica(sb, "N O T A   C R E D I T O", "150", "D")
            sb = ImprimirLineaGrafica(sb, "NOMBRE CLIENTE O RAZON SOCIAL : " & Cliente & "|No " & .Item("NUMNCR"), "130|20", "I|D")
            sb = ImprimirLineaGrafica(sb, "RIF O CI O PASAPORTE No : " & .Item("RIF") & "|FECHA DE EMISION : " & Format(CDate(.Item("emision").ToString), "dd-MM-yyyy"), "90|60", "I|D")
            sb = ImprimirLineaGrafica(sb, "DOMICILIO FISCAL : " & .Item("dirfiscal") & "|Teléf.: " & .Item("telef1"), "130|20", "I|D")
            sb = ImprimirLineaGrafica(sb, "------------------------------------------------------------------------------------------------------------------------------------------------------", "150", "C")
            sb = ImprimirLineaGrafica(sb, "Cod. Cliente: " & .Item("codcli") & "   Asesor: " & .Item("codven") & " " & Left(.Item("vendedor"), 25) & "|Almacen : " & .Item("almacen") & "|Ruta: " & .Item("ruta_visita"), "120|18|12", "I|I|D")
            sb = ImprimirLineaGrafica(sb, "------------------------------------------------------------------------------------------------------------------------------------------------------", "150", "C")
            sb = ImprimirLineaGrafica(sb, "ITEM|DESCRIPCION|CANTIDAD UND    KGS     IVA      PRECIO   %DEV   CAUSA           TOTAL   TP", "12|50|88", "I|I|D")
            sb = ImprimirLineaGrafica(sb, "------------------------------------------------------------------------------------------------------------------------------------------------------", "150", "C")

        End With
        ImprimeEncabezadoNotaCredito = sb

    End Function
    Public Function ImprimirLineaGrafica(ByVal sb As StringBuilder, ByVal Cadenas As String, ByVal Anchos As String, ByVal Alineaciones As String) As StringBuilder

        Dim aCadenas() As String = Split(Cadenas, "|")
        Dim aAnchos() As String = Split(Anchos, "|")
        Dim aAlineaciones() As String = Split(Alineaciones, "|")

        Dim i As Integer
        Dim Columnas As Integer = UBound(aAnchos)
        Dim CadenaAImprimir As String = ""

        If Not IsNothing(aCadenas) Then
            For i = 0 To Columnas
                Select Case aAlineaciones(i)
                    Case "D"
                        CadenaAImprimir += ChangeSpanishCharacters(aCadenas(i)).PadLeft(CInt(aAnchos(i)), " ")
                    Case "C"
                        CadenaAImprimir += ChangeSpanishCharacters(aCadenas(i)).PadLeft(Int(CInt(aAnchos(i)) / 2), " ")
                    Case "I"
                        CadenaAImprimir += ChangeSpanishCharacters(aCadenas(i)).PadRight(CInt(aAnchos(i)), " ")
                End Select
            Next
            sb.AppendLine(CadenaAImprimir)
        End If
        ImprimirLineaGrafica = sb
    End Function
    Public Function ImprimirLineaGraficaPlus(ByVal sb As StringBuilder, ByVal Cadenas As String) As StringBuilder

        Dim aCadenas() As String = Split(Cadenas, "|")
        Dim CadenaAImprimir As String = ""

        If Not IsNothing(aCadenas) Then
            For Each nStr As String In aCadenas
                Select Case nStr.Split("_")(2)
                    Case "D"
                        CadenaAImprimir += ChangeSpanishCharacters(nStr.Split("_")(0)).PadLeft(CInt(nStr.Split("_")(1)), " ")
                    Case "C"
                        CadenaAImprimir += ChangeSpanishCharacters(nStr.Split("_")(0)).PadLeft(Int(CInt(nStr.Split("_")(1)) / 2), " ")
                    Case "I"
                        CadenaAImprimir += ChangeSpanishCharacters(nStr.Split("_")(0)).PadRight(CInt(nStr.Split("_")(1)), " ")
                End Select
            Next
            sb.AppendLine(CadenaAImprimir)
        End If

        Return sb

    End Function

    Private Function ChangeSpanishCharacters(ByVal SpanishString As String) As String
        Dim sEnglishString As String = ""
        If Not SpanishString Is Nothing Then
            For i As Short = 0 To SpanishString.Length - 1
                Select Case SpanishString.Chars(i)
                    Case "á" : sEnglishString &= "a"
                    Case "é" : sEnglishString &= "e"
                    Case "í" : sEnglishString &= "i"
                    Case "ó" : sEnglishString &= "o"
                    Case "ú" : sEnglishString &= "u"
                    Case "ñ" : sEnglishString &= "#"
                    Case "Ñ" : sEnglishString &= "#"
                    Case "½" : sEnglishString &= "0.500"
                    Case "¼" : sEnglishString &= "0.250"
                    Case "¾" : sEnglishString &= "0.750"
                    Case Else : sEnglishString &= SpanishString.Chars(i)
                End Select
            Next
        End If
        Return sEnglishString
    End Function

    Public Sub ImprimirConsecutivosFaltantes(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                              ByVal dt As DataTable)

        Dim pd As New PrintDialog()
        pd.PrinterSettings = New PrinterSettings()

        If (pd.ShowDialog() = DialogResult.OK) Then

            Dim sb As New StringBuilder()
            Dim nConsecutivo As Integer
            For nConsecutivo = 0 To dt.Rows.Count - 1
                With dt.Rows(nConsecutivo)

                    Dim aCad As String = .Item("noconsecutivo")

                    Dim aAnc As String = "150"
                    Dim aAli As String = "I"
                    sb = ImprimirLineaGrafica(sb, aCad, aAnc, aAli)

                End With
            Next


            '///// CIERRE E IMPRESION DE FACTURA

            Dim mydocpath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            Using outfile As New StreamWriter(mydocpath & "\ConsecutivosFaltantes.txt")
                outfile.Write(sb.ToString())
            End Using
            Dim s As String = ""
            Using sr As New StreamReader(mydocpath & "\ConsecutivosFaltantes.txt")
                Dim line As String
                ' Read and display lines from the file until the end of
                ' the file is reached.
                Do
                    line = sr.ReadLine()
                    If Not (line Is Nothing) Then
                        s += line + vbCrLf
                    End If
                Loop Until line Is Nothing
            End Using


            Dim pr As New MyPrinter
            pd.PrinterSettings.DefaultPageSettings.Margins.Left = 0
            pr.prt(pd.PrinterSettings.PrinterName, s)

            sb = Nothing

        End If



    End Sub
End Module
