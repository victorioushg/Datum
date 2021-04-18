Imports MySql.Data.MySqlClient
Imports RawPrinterHeper
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Drawing.Printing

Module FuncionesReportesRAW
    Private ft As New Transportables

    Public Sub ImprimirFactura(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                               ByVal ds As DataSet, ByVal NumeroFactura As String)

        Dim nTablaEncabezado As String = "tblEncabezadoR"
        Dim nTablaRenglones As String = "tblRenglonesR"
        Dim nTablaIVA As String = "tblIVAR"
        Dim nTablaDescuentos As String = "tblDescuentosR"

        Dim dtRenglones As DataTable
        Dim dtEncab As DataTable
        Dim dtIVA As DataTable
        Dim dtDescuentos As DataTable

        Dim nLineasFactura As Integer = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA16"))
        Dim nLinea As Integer
        Dim nRenglones As Integer

        ds = DataSetRequery(ds, " select a.numfac, a.emision, a.vence, a.codcli, b.nombre, b.rif, b.dirfiscal, b.telef1, b.ruta_visita, a.comen, a.codven, " _
                             & " concat(c.nombres, ' ', c.apellidos) vendedor, " _
                             & " a.condpag, a.transporte, a.tot_fac, a.imp_iva, a.descuen, a.tot_net " _
                             & " from jsvenencfac a " _
                             & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp ) " _
                             & " left join jsvencatven c on (a.codven = c.codven and a.id_emp = b.id_emp ) " _
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

        ds = DataSetRequery(ds, " SELECT numfac, '' tipoiva, poriva, SUM(baseiva) baseiva, IFNULL(SUM(IMPIVA),0) impiva, ejercicio, id_emp " _
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

        If (pd.ShowDialog() = DialogResult.OK) Then
            RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, Chr(27) & "@")
            RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, Chr(27) & Chr(77))

            '///// ENCABEZADO DE FACTURA 
            Dim FechaFactura As Date = CDate(dtEncab.Rows(0).Item("emision").ToString)
            nLinea = ImprimeEncabezadoFactura(MyConn, lblInfo, nLinea, pd.PrinterSettings.PrinterName, dtEncab)

            '///// RENGLONES DE FACTURA
            For nRenglones = 0 To dtRenglones.Rows.Count - 1
                With dtRenglones.Rows(nLinea)
                    Dim aCad As String = .Item("item") & "|" & .Item("descrip") & "|" & ft.FormatoCantidad(.Item("cantidad")) & "|" & .Item("unidad") & "|" & _
                                                ft.FormatoCantidad(.Item("peso")) & "|" & ft.FormatoNumero(PorcentajeIVA(MyConn, lblInfo, FechaFactura, .Item("IVA"))) & "|" & _
                                                ft.FormatoNumero(.Item("precio")) & "|" & ft.FormatoNumero(.Item("des_art")) & "|" & ft.FormatoNumero(.Item("des_cli")) & "|" & ft.FormatoNumero(.Item("des_ofe")) & "|" & _
                                                ft.FormatoNumero(.Item("totren")) & "|" & .Item("estatus")

                    Dim aAnc As String = "15|67|10|5|10|7|11|7|7|7|12|2"
                    Dim aAli As String = "I|I|D|C|D|D|D|D|D|D|D|C"
                    nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, aCad, aAnc, aAli)

                    '///// COMENTARIOS DE RENGLON DE FACTURA
                    Dim Comentario_Renglon As String = ft.DevuelveScalarCadena(MyConn, "select comentario from jsvenrencom where " _
                                                                             & " numdoc = '" & .Item("numfac") & "' and " _
                                                                             & " origen = 'FAC' and " _
                                                                             & " item = '" & .Item("item") & "' and " _
                                                                             & " renglon = '" & .Item("renglon") & "' and " _
                                                                             & " id_emp = '" & jytsistema.WorkID & "' ")

                    If Len(Comentario_Renglon) > 0 Then
                        Dim comen As Boolean = True
                        Dim inicioLinea As Integer = 0
                        Dim LongitudLinea As Integer = 60
                        While comen
                            Dim Comentario As String = Mid(Comentario_Renglon, inicioLinea + 1, LongitudLinea)
                            nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, "|" & Comentario & "|", "20|60|80", "I|I|I")
                            inicioLinea = inicioLinea + LongitudLinea
                            If inicioLinea >= Len(Comentario_Renglon) Then comen = False
                        End While
                    End If
                    '///// FIN COMENTARIOS RENGLON DE FACTURA

                    '///// MERCANCIAS DE COMBO
                    Dim MercanciaCombo As Integer = ft.DevuelveScalarBooleano(MyConn, "select count(*) from jsmercatcom where codart = '" & .Item("item") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If MercanciaCombo > 0 Then
                        Dim nTablaCombo As String = "tblCombo"
                        Dim dtCombo As DataTable
                        ds = DataSetRequery(ds, "select * from jsmercatcom where codart = '" & .Item("ITEM") & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaCombo, lblInfo)
                        dtCombo = ds.Tables(nTablaCombo)
                        If dtCombo.Rows.Count > 0 Then
                            Dim nCombo As Integer
                            nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, "MERCANCIAS DEL COMBO...", "160", "D")
                            For nCombo = 0 To dtCombo.Rows.Count - 1
                                With dtCombo.Rows(nCombo)
                                    nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, _
                                                           .Item("item") & "|" & .Item("descrip") & "|" & ft.FormatoCantidad(.Item("cantidad")) & "|" & .Item("unidad") & "|" & _
                                                ft.FormatoCantidad(.Item("peso")) & "|", "15|67|10|5|10|53", "I|I|D|D|D|D")
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
            nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, "|----------------------------------------------------------------", "80|80", "D|D")
            nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, "SUBTOTAL :|" & ft.FormatoCantidad(ft.DevuelveScalarDoble(MyConn, "select SUM(PESO) from jsvenrenfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac ")) & "||" & ft.FormatoNumero(ft.DevuelveScalarDoble(MyConn, "Select SUM(totren) FROM jsvenrenfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac")), "97|10|39|12|2", "D|D|D|D|C")

            '///// LIneas Adicionales para total
            Dim gCont As Integer
            Dim TotalLineas As Integer = 39 'CANTIDAD DE LINEAS ESPECIFICAS POR LARGO DE PAPEL Y POR MODO DE IMPRESORA (DRAFT/GRAFICA)
            Dim TotalLineasFactura As Integer = NumeroLineasEnFactura(MyConn, lblInfo, NumeroFactura)
            For gCont = 1 To TotalLineas - TotalLineasFactura
                nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, "", "160", "D")
            Next

            '///// DESCUENTOS FACTURA
            If dtDescuentos.Rows.Count > 0 Then
                Dim nDescuentos As Integer
                For nDescuentos = 0 To dtDescuentos.Rows.Count - 1
                    With dtDescuentos.Rows(nDescuentos)
                        nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, .Item("descrip") & "|" & ft.FormatoNumero(.Item("pordes")) & "%|" & ft.FormatoNumero(.Item("descuento")) & "|", "124|10|12|14", "D|D|D|C")
                    End With
                Next
            End If
            nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, "|----------------------------------------------------------------", "80|80", "D|D")
            nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, "|TOTAL DESCUENTOS :|" & ft.FormatoNumero(ft.DevuelveScalarDoble(MyConn, " select SUM(DESCUENTO) from jsvendesfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac ")) & "|", "106|40|12|2", "D|D|D|C")
            '///// FIN DESCUENTOS FACTURA

            '///// IMPUESTO AL VALOR AGREGADO
            If dtIVA.Rows.Count > 0 Then
                Dim nIVA As Integer
                For nIVA = 0 To dtIVA.Rows.Count - 1
                    With dtIVA.Rows(nIVA)
                        If .Item("tipoiva") = "" Then
                            nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, "|MONTO TOTAL EXENTO O EXONERADO :|" & ft.FormatoNumero(.Item("BASEIVA")) & "|", "50|42|12|56", "D|D|D|I")
                        Else
                            nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, "|BASE IMPONIBLE SEGUN ALICUOTA|" & ft.FormatoNumero(.Item("PORIVA")) & "% :|" & ft.FormatoNumero(.Item("BASEIVA")) & "|TOTAL IMPUESTO SEGUN ALICUOTA|" & ft.FormatoNumero(.Item("PORIVA")) & "% :|" & ft.FormatoNumero(.Item("IMPIVA")) & "|", "50|32|10|12|32|10|12|2", "I|D|D|D|D|D|D|I")
                        End If
                    End With
                Next
            End If
            '///// FIN IMPUESTO AL VALOR AGREGADO

            '///// PIE DE FACTURA
            Dim Son As String = NumerosATexto(dtEncab.Rows(0).Item("TOT_FAC"))
            nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, "________________________________________", "160", "I")
            nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, "        FIRMA Y SELLO DEL CLIENTE ", "160", "I")
            nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, "ACEPTADO PARA SER PAGADO A SU VENCIMIENTO|MONTO TOTAL DE LA VENTA :|" & ft.FormatoNumero(dtEncab.Rows(0).Item("TOT_FAC")) & "|", "121|25|12|2", "I|D|D|C")
            nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, "FORMA DE PAGO :___EFECTIVO ___ CHEQUE ___TARJETA DEBITO ___TARJETA CREDITO|----------------------------------------------------------------", "80|80", "I|D")
            nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, "SON : " & Son, "160", "D")
            '/////////// COMENTARIOS DE FACTURA
            Dim nTablaComentarios As String = "tblComentarios"
            Dim dtComentarios As DataTable
            ds = DataSetRequery(ds, "select * from jsconctacom where origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "'", MyConn, nTablaComentarios, lblInfo)
            dtComentarios = ds.Tables(nTablaComentarios)
            If dtComentarios.Rows.Count > 0 Then
                Dim nComen As Integer
                For nComen = 0 To dtComentarios.Rows.Count - 1
                    With dtComentarios.Rows(nComen)
                        nLinea = ImprimirLinea(nLinea, pd.PrinterSettings.PrinterName, .Item("comentario"), "160", "I")
                    End With
                Next
            End If
            dtComentarios.Dispose()
            dtComentarios = Nothing
            '///// FIN PIE DE FACTURA

            '///// CIERRE DE FACTURA
            RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, Chr(12))
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
    Private Function ImprimeEncabezadoFactura(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal nLinea As Integer, ByVal szPrinterName As String, _
                                              ByVal dtEncab As DataTable) As Integer
        Dim gCont As Integer
        nLinea = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA18"))
        For gCont = 1 To nLinea
            RawPrinterHelper.SendStringToPrinter(szPrinterName, Chr(10))
        Next
        With dtEncab.Rows(0)
            Dim Cliente As String = .Item("nombre")
            Dim CondicionPago As String = IIf(CInt(.Item("condpag")) = 0, "CREDITO CON VENCIMIENTO AL " & ft.FormatoFecha(CDate(.Item("vence").ToString)), "CONTADO")

            nLinea = ImprimirLinea(nLinea, szPrinterName, "F A C T U R A", "160", "D")
            nLinea = ImprimirLinea(nLinea, szPrinterName, "NOMBRE CLIENTE O RAZON SOCIAL : " & Cliente & "|No " & .Item("numfac"), "140|20", "I|D")
            nLinea = ImprimirLinea(nLinea, szPrinterName, "RIF O CI O PASAPORTE No : " & .Item("RIF") & "|FECHA DE EMISION : " & Format(CDate(.Item("emision").ToString), "dd-MM-yyyy"), "100|60", "I|D")
            nLinea = ImprimirLinea(nLinea, szPrinterName, "DOMICILIO FISCAL : " & .Item("dirfiscal") & "|TELEFONO : " & .Item("telef1"), "135|25", "I|D")
            nLinea = ImprimirLinea(nLinea, szPrinterName, "----------------------------------------------------------------------------------------------------------------------------------------------------------------", "160", "C")
            nLinea = ImprimirLinea(nLinea, szPrinterName, "CODIGO CLIENTE: " & .Item("codcli") & "|CONDICION PAGO: " & CondicionPago & "|VENDEDOR: " & .Item("codven") & " " & Left(.Item("vendedor"), 25) & "|RUTA: " & .Item("ruta_visita"), "22|60|42|12", "I|C|C|D")
            nLinea = ImprimirLinea(nLinea, szPrinterName, "----------------------------------------------------------------------------------------------------------------------------------------------------------------", "160", "C")
            nLinea = ImprimirLinea(nLinea, szPrinterName, "ITEM|DESCRIPCION|CANTIDAD|UND|KGS|IVA|PRECIO|DESCUENTOS|TOTAL|TP", "15|67|10|5|10|7|11|21|12|5", "I|I|D|C|D|D|D|C|D|D")
            nLinea = ImprimirLinea(nLinea, szPrinterName, "----------------------------------------------------------------------------------------------------------------------------------------------------------------", "160", "C")

        End With

    End Function
    Public Function ImprimirLinea(ByVal nLinea As Integer, ByVal szPrinterName As String, ByVal Cadenas As String, ByVal Anchos As String, ByVal Alineaciones As String) As Integer

        Dim aCadenas() As String = Split(Cadenas, "|")
        Dim aAnchos() As String = Split(Anchos, "|")
        Dim aAlineaciones() As String = Split(Alineaciones, "|")

        Dim i As Integer, Columnas As Integer
        Dim CadenaAImprimir As String = ""
        ImprimirLinea = nLinea
        If Not IsNothing(aCadenas) Then
            Columnas = UBound(aCadenas)
            i = UBound(aAlineaciones)
            If i < Columnas Then Columnas = i
            i = UBound(aAnchos)
            If i < Columnas Then Columnas = i
            ImprimirLinea += 1 'Devuelve el nº de columnas impresas
            For i = 0 To Columnas
                CadenaAImprimir += RellenaCadenaConCaracter(aCadenas(i), aAlineaciones(i), CInt(aAnchos(i)))
            Next
            RawPrinterHelper.SendStringToPrinter(szPrinterName, CadenaAImprimir & vbCrLf)
        End If

    End Function

    Public Sub EnviarReporteImpresoraEpsonESCP(prtName As String, doc As String, Optional nLineasPie As Integer = 0)


        'INICIAR IMPRESORA
        RawPrinterHelper.SendStringToPrinter(prtName, ChrW(27) & ChrW(64))

        'DRAFT
        RawPrinterHelper.SendStringToPrinter(prtName, ChrW(27) & ChrW(120) & "0")

        '12CPI
        RawPrinterHelper.SendStringToPrinter(prtName, ChrW(27) & ChrW(77))

        'CONDENSADA
        RawPrinterHelper.SendStringToPrinter(prtName, ChrW(27) & ChrW(15))

        'INTERLINEADO = 1/8 INCH
        RawPrinterHelper.SendStringToPrinter(prtName, ChrW(27) & ChrW(48))

        'INTERnacional character set
        'RawPrinterHelper.SendStringToPrinter(prtName, ChrW(27) & ChrW(116) & ChrW(16))

        'DOCUMENTO
        RawPrinterHelper.SendStringToPrinter(prtName, doc)

        'RETORNO DE CARRO
        'RawPrinterHelper.SendStringToPrinter(prtName, ChrW(13))

        'FORM FEED
        RawPrinterHelper.SendStringToPrinter(prtName, ChrW(12))

        For pCont As Integer = 1 To nLineasPie
            RawPrinterHelper.SendStringToPrinter(prtName, Chr(10))
        Next

    End Sub

End Module
