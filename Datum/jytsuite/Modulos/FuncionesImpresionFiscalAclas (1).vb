Imports MySql.Data.MySqlClient
Imports System.IO
Module FuncionesImpresionFiscalAclas
    '//////////////////////////////////////////////////////////
    '// Fiscal Printer library definitions
    '//////////////////////////////////////////////////////////
    Public Declare Function OpenFpctrl Lib "TFHKAIF.DLL" (ByVal lpPortName As String) As Long
    Public Declare Function CloseFpctrl Lib "TFHKAIF.DLL" () As Long
    Public Declare Function CheckFprinter Lib "TFHKAIF.DLL" () As Long
    Public Declare Function ReadFpStatus Lib "TFHKAIF.DLL" (ByRef status As Long, ByRef errror As Long) As Long
    Public Declare Function SendCmd Lib "TFHKAIF.DLL" (ByRef status As Long, ByRef errror As Long, ByVal cmd As String) As Long
    Public Declare Function SendNCmd Lib "TFHKAIF.DLL" (ByVal status As Long, ByVal errror As Long, ByVal buffer As String) As Long
    Public Declare Function SendFileCmd Lib "TFHKAIF.DLL" (ByRef status As Long, ByRef errror As Long, ByVal file As String) As Long
    Public Declare Function UploadReportCmd Lib "TFHKAIF.DLL" (ByRef status As Long, ByRef errror As Long, ByVal cmd As String, ByVal file As String) As Long
    Public Declare Function UploadStatusCmd Lib "TFHKAIF.DLL" (ByRef status As Long, ByRef errror As Long, ByVal cmd As String, ByVal file As String) As Long

    Public iError As Long
    Public iStatus As Long
    Public bRet As Boolean
    Public cmd As String

    Private bCont As Integer = 0

    Private TipoImpresora As Integer = 2
    Private nNumeroColumnas As Integer = 40

    Private Enum ImpresoraFiscal
        iAclas = 2
        iBixolon350 = 5
        iTallyDascon1125 = 6
    End Enum

    Private Enum ComandoFiscalPP1F3
        iEncabezadoPiePaginaDatos = 0
        iRenglon = 1
        iPago = 2
        iNombrePago = 3
        iEncabezadoPiePaginaProgramacion = 4
        iIVA = 5
        iHora = 6
        iFecha = 7
        iSubtotal = 8
        iDescuento = 9
        iPagodirecto = 10
        iLineaNoFiscal = 11
        iLineaNoFiscalCierre = 12
        iMensajeFactura = 13
        iPagoNotaCredito = 14
        iResetImpresora = 15
        iDescuentoRenglon = 16
    End Enum
    Private Function EsperaParaImprimirEnPP1F3() As Boolean
        bRet = ReadFpStatus(iStatus, iError)
        'MsgBox(" estatus = " & iStatus & "; error = " & iError)
        Return bRet
    End Function
    Private Function AbrirPuertoPP1F3(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label) As Boolean

        Dim PuertoImpresoraFiscalPP1F3 As String = "COM" & PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox).ToString
        If PuertoImpresoraFiscalPP1F3 = "COM0" Then PuertoImpresoraFiscalPP1F3 = "COM1"

        
        bRet = OpenFpctrl(PuertoImpresoraFiscalPP1F3)
        While Not bRet
            bRet = ResetMaquina()
            bRet = OpenFpctrl(PuertoImpresoraFiscalPP1F3)
        End While
        AbrirPuertoPP1F3 = True

    End Function
    Private Function CerrarPuertoPP1F3() As Boolean
        bRet = CloseFpctrl()
        If bRet Then CerrarPuertoPP1F3 = True
    End Function
    Private Function ImpresoraEncendida() As Boolean
        bRet = CheckFprinter()
        While Not bRet
            bRet = CheckFprinter()
            If Not bRet Then MsgBox("Impresora fiscal apagada. Verifique por favor...")
        End While
        Return bRet
    End Function
    Private Function ResetMaquina() As Boolean
        ResetMaquina = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iResetImpresora, 0, "", bRet, iStatus, iError)
    End Function

    Public Function ImprimirLineaFiscalPP1F3(ByVal Tipo As Integer, ByVal Linea As Integer, ByVal Texto As String, _
    ByVal bRet As Boolean, ByVal iStatus As Long, ByVal ierror As Long) As Boolean

        Dim cmd As String = ""
        Select Case Tipo
            Case 0 ' ENCABEZADO
                cmd = "i" & Format(Linea, "00") & Texto
            Case 1 ' RENGLON
                cmd = Texto
            Case 2 ' PAGO
                cmd = "2" & FormaPagoFiscal(Linea) & Texto
            Case 3 ' NOMBRE PAGO
                cmd = "PE" & FormaPagoFiscal(Linea) & Texto
            Case 4 ' MENSAJE PIE DE PAGINA
                cmd = "PH9" & CStr(Linea) & Texto
            Case 5 ' PROGRAMACION TASAS IVA
                cmd = "PT" & Texto
            Case 6 'HORA
                cmd = "PF" & Texto
            Case 7 'FECHA
                cmd = "PG" & Texto
            Case 8 'Imprimir Subtotal Descuento
                cmd = "3"
            Case 9 'Descuento
                cmd = "p-" & Texto
            Case 10 'PAGO DIRECTO
                cmd = "1" & FormaPagoFiscal(Linea)
            Case 11 ' NO FISCAL
                cmd = "800" & Texto
            Case 12 'NO FISCAL CIERRE
                cmd = "810" & Texto
            Case 13
                cmd = "@" & Texto
            Case 14
                cmd = "f" & FormaPagoFiscal(Linea) & Texto
            Case 15
                cmd = "e"
            Case 16 're-impresion FACTURA
                cmd = "RF" & Texto
            Case 17 're-impresion nota credito
                cmd = "RC" & Texto
            Case 18 're-IMPRESION ULTIMO DOCUMENTO
                cmd = "RU00000000000000"
            Case ComandoFiscalPP1F3.iDescuentoRenglon
                cmd = "q-" & Texto
        End Select

        ImprimirLineaFiscalPP1F3 = False
        While Not ImprimirLineaFiscalPP1F3
            ImprimirLineaFiscalPP1F3 = SendCmd(iStatus, ierror, cmd)
        End While


    End Function
    Private Function IVAFiscal(ByVal TipoIVA As String) As String
        IVAFiscal = Chr(&H20)
        Select Case Trim(TipoIVA)
            Case ""
                IVAFiscal = Chr(&H20)
            Case "A"
                IVAFiscal = Chr(&H21)
            Case "B"
                IVAFiscal = Chr(&H22)
            Case "C"
                IVAFiscal = Chr(&H23)
        End Select
    End Function
    Public Function IVAFiscalNC(ByVal TipoIVA As String) As String
        IVAFiscalNC = "0"
        Select Case Trim(TipoIVA)
            Case ""
                IVAFiscalNC = "0"
            Case "A"
                IVAFiscalNC = "1"
            Case "B"
                IVAFiscalNC = "2"
            Case "C"
                IVAFiscalNC = "3"
        End Select
    End Function

    Private Function FormaPagoFiscal(ByVal FP As Integer) As String
        'FP 1-4 Efectivo, 5-8 Cheque, 9-12 Tarjeta 13-16 Cesta Ticket 
        FormaPagoFiscal = Format(FP, "00")
    End Function
    Private Function FormatoFechaFiscal(ByVal sFecha As Date) As String
        FormatoFechaFiscal = Format(sFecha, sFormatoFechaFiscal)
    End Function
    Private Function FormatoCantidadFiscal(ByVal sNumero As Double) As String
        FormatoCantidadFiscal = Format(sNumero * 1000, FORMATO_CANTIDAD)
    End Function
    Private Function FormatoPrecioFiscal(ByVal sNumero As Double) As String
        FormatoPrecioFiscal = Format(sNumero * 100, FORMATO_PRECIO)
    End Function
    Public Sub ImprimirFacturaFiscalPP1F3(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroFactura As String, _
                                            ByVal NumeroSerialFiscal As String, ByVal NombreCliente As String, _
                                            ByVal CodigoCliente As String, ByVal RIF As String, ByVal DireccionFiscal As String, _
                                            ByVal FechaEmision As Date, ByVal CondicionPago As String, _
                                            ByVal FechaVencimiento As Date, ByVal CodigoCajero As String, ByVal NombreCajero As String, _
                                            Optional ByVal TipoImpresoraAclas As Integer = 2)

        Dim ds As New DataSet

        '////////////// TIPO DE IMPRESORA
        Select Case TipoImpresoraAclas
            Case ImpresoraFiscal.iAclas, ImpresoraFiscal.iBixolon350
                nNumeroColumnas = 40
            Case ImpresoraFiscal.iTallyDascon1125
                nNumeroColumnas = 120
        End Select



        ' Verifica impresora
        bRet = AbrirPuertoPP1F3(MyConn, lblInfo)

        'Encendido/Apagado Impresora
        bRet = ImpresoraEncendida()

        'Imprimir Encabezado 
        ImprimeEncabezadoFacturaPP1F3(bRet, NombreCliente, NumeroFactura, FechaEmision, DireccionFiscal, RIF, CondicionPago, FechaVencimiento)

        'imprimir renglones 
        Dim bbCont As Integer = 0
        Dim dtRenglones As DataTable
        Dim nTablaRenglon As String = "tblrenglones"
        ds = DataSetRequery(ds, " select * from jsvenrenfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaRenglon, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglon)
        If dtRenglones.Rows.Count > 0 Then
            For bbCont = 0 To dtRenglones.Rows.Count - 1
                With dtRenglones.Rows(bbCont)

                    Dim nDescrip As String = .Item("UNIDAD") & " " & .Item("DESCRIP")

                    Dim nDescripcion As String = ""
                    Dim nDescripResto80 As String = ""
                    Dim nDescripResto120 As String = ""

                    If nNumeroColumnas = 40 Then
                        nDescripcion = IIf(Len(nDescrip) > 36, Mid(nDescrip, 1, 36), nDescrip)
                        nDescripResto80 = IIf(nDescrip.Length > 36, Mid(nDescrip, 37, IIf(nDescrip.Length > 74, 37, nDescrip.Length)), "")
                        nDescripResto120 = IIf(nDescrip.Length > 74, Mid(nDescrip, 74, IIf(nDescrip.Length > 111, 37, nDescrip.Length)), "")
                    Else
                        nDescripcion = nDescrip
                    End If

                    If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA23")) Then
                        Dim dPrecioSugerido As String = " PVJusto :" & FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, _
                            " select sugerido from jsmerctainv where codart = '" & .Item("item") & "' and id_emp = '" & jytsistema.WorkID & "' "))).PadLeft(10, " ")
                        If Trim(nDescripResto120) = "" Then
                            nDescripResto120 = dPrecioSugerido
                        Else
                            nDescripResto120 = IIf(Len(nDescripResto120) > 17, nDescripResto120.Substring(0, 16), nDescripResto120) & dPrecioSugerido
                        End If
                    End If

                    cmd = IVAFiscal(.Item("IVA")) & FormatoPrecioFiscal(.Item("PRECIO")) & FormatoCantidadFiscal(.Item("CANTIDAD")) & nDescripcion
                    bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iRenglon, 0, cmd, bRet, iStatus, iError)
                    Dim DescuentoRenglon As Double = (1 - (1 - .Item("des_art") / 100) * (1 - .Item("des_cli") / 100) * (1 - .Item("des_ofe") / 100)) * 100
                    Dim ss As String = Format(DescuentoRenglon * 100, "0000")
                    If DescuentoRenglon <> 0.0 Then bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iDescuento, 0, ss, bRet, iStatus, iError)

                    If nDescripResto80 <> "" Then _
                        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iMensajeFactura, 0, nDescripResto80, bRet, iStatus, iError)
                    If nDescripResto120 <> "" Then _
                        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iMensajeFactura, 0, nDescripResto120, bRet, iStatus, iError)


                End With
            Next
        End If
        dtRenglones.Dispose()
        dtRenglones = Nothing


        'imprimir pie de pagina
        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iEncabezadoPiePaginaDatos, 7, "ASESOR COMERCIAL : " & CodigoCajero & " " & NombreCajero, bRet, iStatus, iError)
        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iEncabezadoPiePaginaDatos, 8, "NUMERO DE FACTURA DATUM : " & NumeroFactura, bRet, iStatus, iError)

        'imprimir descuentos
        Dim dtDescuentos As DataTable
        Dim dCont As Integer = 0
        Dim nTablaDescuentos As String = "tblDescuentos"
        ds = DataSetRequery(ds, " select * from jsvendesfac where numfac = '" & NumeroFactura & "' and ID_EMP = '" & jytsistema.WorkID & "'", MyConn, nTablaDescuentos, lblInfo)
        dtDescuentos = ds.Tables(nTablaDescuentos)

        If dtDescuentos.Rows.Count > 0 Then
            For dCont = 0 To dtDescuentos.Rows.Count - 1
                With dtDescuentos.Rows(dCont)
                    bRet = ImprimirLineaFiscalPP1F3(8, 0, "", bRet, iStatus, iError)
                    bRet = ImprimirLineaFiscalPP1F3(9, 0, Format(.Item("pordes") * 100, "0000"), bRet, iStatus, iError)
                End With
            Next
        End If

        dtDescuentos.Dispose()
        dtDescuentos = Nothing

        'imprimir formas de pago 
        Dim mCont As Integer
        Dim dtFP As DataTable
        Dim nTablaFP As String = "tblfp"
        Dim ImportePago As Double = 0


        Dim fpCont As Integer = 1
        ds = DataSetRequery(ds, " select * from jsvenforpag where numfac = '" & NumeroFactura & "' and numserial = '" & NumeroSerialFiscal & "' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaFP, lblInfo)
        dtFP = ds.Tables(nTablaFP)
        If dtFP.Rows.Count > 0 Then
            For mCont = 0 To dtFP.Rows.Count - 1
                With dtFP.Rows(mCont)
                    fpCont = InArray(aFormaPagoAbreviada, .Item("FORMAPAG"))
                    bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iNombrePago, fpCont, aFormaPago(fpCont - 1) + " " & Right(.Item("numpag"), 5), bRet, iStatus, iError)
                    ImportePago = .Item("importe")
                    If mCont = dtFP.Rows.Count - 1 Then
                        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iPagodirecto, fpCont, "", bRet, iStatus, iError)
                    Else
                        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iPago, fpCont, Format(ImportePago * 100, FORMATO_ENTERO12), bRet, iStatus, iError)
                    End If
                End With

            Next
        Else
            'Pago CREDITO
            bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iPagodirecto, fpCont, "", bRet, iStatus, iError)
        End If
        dtFP.Dispose()
        dtFP = Nothing




        '//// ACTUALIZAR BIT IMPRESION EN FACTURA
        EjecutarSTRSQL(MyConn, lblInfo, " update jsvenencfac set impresa = '1' where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")
        '//// ACTUALIZAR NUMERO DE CONTROL"
        Dim FechaIvaFactura As Date = CDate(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select emision from jsvenencfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ").ToString)
        'If AbrirPuertoPP1F3(MyConn, lblInfo) Then _
        ActualizaNumeroControl(MyConn, lblInfo, NumeroFactura, FechaIvaFactura, "FAC", "FAC", CodigoCliente)

        CerrarPuertoPP1F3()

        ds = Nothing

    End Sub
    Public Sub ImprimirNotaCreditoPP1F3(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroNotaCredito As String, _
                                        ByVal NumeroFacturaAfectada As String, _
                                        ByVal NumeroSerialFiscal As String, ByVal Cliente As String, ByVal RIF As String, _
                                        ByVal DireccionFiscal As String, ByVal Telefono As String, ByVal FechaEmision As Date, ByVal CodigoCliente As String, _
                                        ByVal CondicionPago As String, ByVal FechaVencimiento As Date, ByVal CodigoVendedor As String, ByVal Vendedor As String, _
                                        ByVal TotalFactura As Double, Optional ByVal TipoImpresoraAclas As Integer = ImpresoraFiscal.iAclas)

        Dim ds As New DataSet


        '////////////// TIPO DE IMPRESORA
        Select Case TipoImpresoraAclas
            Case ImpresoraFiscal.iAclas, ImpresoraFiscal.iBixolon350
                nNumeroColumnas = 40
            Case ImpresoraFiscal.iTallyDascon1125
                nNumeroColumnas = 120
        End Select


        bRet = AbrirPuertoPP1F3(MyConn, lblInfo)
        bRet = ImpresoraEncendida()

        ImprimeEncabezadoFacturaPP1F3(bRet, Cliente, NumeroNotaCredito, FechaEmision, DireccionFiscal, RIF, CondicionPago, FechaVencimiento)

        Dim dtRenglones As DataTable
        Dim nTablaRenglon As String = "tblrenglones"
        ds = DataSetRequery(ds, " select * from jsvenrenncr where numncr = '" & NumeroNotaCredito & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaRenglon, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglon)
        If dtRenglones.Rows.Count > 0 Then
            For bCont = 0 To dtRenglones.Rows.Count - 1
                With dtRenglones.Rows(bCont)
                    Dim nDescrip As String = .Item("UNIDAD") & " " & .Item("DESCRIP")

                    Dim nDescripcion As String = ""
                    Dim nDescripResto80 As String = ""
                    Dim nDescripResto120 As String = ""

                    If nNumeroColumnas = 40 Then
                        nDescripcion = IIf(Len(nDescrip) > 36, Mid(nDescrip, 1, 36), nDescrip)
                        nDescripResto80 = IIf(nDescrip.Length > 36, Mid(nDescrip, 37, IIf(nDescrip.Length > 74, 37, nDescrip.Length)), "")
                        nDescripResto120 = IIf(nDescrip.Length > 74, Mid(nDescrip, 74, IIf(nDescrip.Length > 111, 37, nDescrip.Length)), "")
                    Else
                        nDescripcion = nDescrip
                    End If

                    cmd = "d" & IVAFiscalNC(.Item("IVA")) & FormatoPrecioFiscal(.Item("TOTRENDES") / .Item("cantidad")) & FormatoCantidadFiscal(.Item("cantidad")) & nDescripcion
                    bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iRenglon, 0, cmd, bRet, iStatus, iError)

                    If nDescripResto80 <> "" Then _
                        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iMensajeFactura, 0, nDescripResto80, bRet, iStatus, iError)
                    If nDescripResto120 <> "" Then _
                        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iMensajeFactura, 0, nDescripResto120, bRet, iStatus, iError)


                End With
            Next
        End If

        Dim numFacturaFiscalAfectada As String = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select num_control from jsconnumcon where numdoc = '" & NumeroFacturaAfectada & "' and id_emp = '" & jytsistema.WorkID & "' ").ToString
        If numFacturaFiscalAfectada.Length > 8 Then numFacturaFiscalAfectada = Mid(numFacturaFiscalAfectada, 1, 8)

        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iEncabezadoPiePaginaDatos, 5, "ASESOR COMERCIAL : " & CodigoVendedor & " " & Vendedor, bRet, iStatus, iError)
        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iEncabezadoPiePaginaDatos, 6, "NOTA CREDITO DATUM : " & NumeroNotaCredito, bRet, iStatus, iError)
        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iEncabezadoPiePaginaDatos, 7, "FACTURA INTERNA AFECTADA : " & NumeroFacturaAfectada, bRet, iStatus, iError)
        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iEncabezadoPiePaginaDatos, 8, "FACTURA FISCAL AFECTADA : " & numFacturaFiscalAfectada, bRet, iStatus, iError)

        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iNombrePago, 1, "EFECTIVO", bRet, iStatus, iError)
        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iPagoNotaCredito, 1, Format(TotalFactura * 100, FORMATO_ENTERO12), bRet, iStatus, iError)

        '//// ACTUALIZAR BIT IMPRESION EN FACTURA
        EjecutarSTRSQL(MyConn, lblInfo, " update jsvenencncr set impresa = '1' where numncr = '" & NumeroNotaCredito & "' and id_emp = '" & jytsistema.WorkID & "' ")
        '//// ACTUALIZAR NUMERO DE CONTROL"
        Dim FechaIvaFactura As Date = EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select emision from jsvenencncr where numncr = '" & NumeroNotaCredito & "' and id_emp = '" & jytsistema.WorkID & "' ").ToString
        ActualizaNumeroControl(MyConn, lblInfo, NumeroNotaCredito, FechaIvaFactura, "NCR", "FAC", CodigoCliente)

        CerrarPuertoPP1F3()

        ds.Dispose()
        ds = Nothing

    End Sub

    Public Sub ImprimirNotaDebitoFiscalPP1F3(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroNotaDebito As String, _
                                        ByVal NumeroSerialFiscal As String, ByVal NombreCliente As String, _
                                        ByVal CodigoCliente As String, ByVal RIF As String, ByVal DireccionFiscal As String, _
                                        ByVal FechaEmision As Date, ByVal CondicionPago As String, _
                                        ByVal FechaVencimiento As Date, ByVal CodigoCajero As String, ByVal NombreCajero As String, _
                                        Optional ByVal TipoImpresoraAclas As Integer = 2)

        Dim ds As New DataSet

        '////////////// TIPO DE IMPRESORA
        Select Case TipoImpresoraAclas
            Case ImpresoraFiscal.iAclas, ImpresoraFiscal.iBixolon350
                nNumeroColumnas = 40
            Case ImpresoraFiscal.iTallyDascon1125
                nNumeroColumnas = 120
        End Select

        ' Verifica impresora
        bRet = AbrirPuertoPP1F3(MyConn, lblInfo)

        'Encendido/Apagado Impresora
        bRet = ImpresoraEncendida()

        'Imprimir Encabezado 
        ImprimeEncabezadoFacturaPP1F3(bRet, NombreCliente, NumeroNotaDebito, FechaEmision, DireccionFiscal, RIF, CondicionPago, FechaVencimiento)

        'imprimir renglones 
        Dim bbCont As Integer = 0
        Dim dtRenglones As DataTable
        Dim nTablaRenglon As String = "tblrenglones"
        ds = DataSetRequery(ds, " select * from jsvenrenndb where numndb = '" & NumeroNotaDebito & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaRenglon, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglon)
        If dtRenglones.Rows.Count > 0 Then
            For bbCont = 0 To dtRenglones.Rows.Count - 1
                With dtRenglones.Rows(bbCont)

                    Dim nDescrip As String = .Item("UNIDAD") & " " & .Item("DESCRIP")

                    Dim nDescripcion As String = ""
                    Dim nDescripResto80 As String = ""
                    Dim nDescripResto120 As String = ""

                    If nNumeroColumnas = 40 Then
                        nDescripcion = IIf(Len(nDescrip) > 36, Mid(nDescrip, 1, 36), nDescrip)
                        nDescripResto80 = IIf(nDescrip.Length > 36, Mid(nDescrip, 37, IIf(nDescrip.Length > 74, 37, nDescrip.Length)), "")
                        nDescripResto120 = IIf(nDescrip.Length > 74, Mid(nDescrip, 74, IIf(nDescrip.Length > 111, 37, nDescrip.Length)), "")
                    Else
                        nDescripcion = nDescrip
                    End If


                    cmd = IVAFiscal(.Item("IVA")) & FormatoPrecioFiscal(.Item("PRECIO")) & FormatoCantidadFiscal(.Item("CANTIDAD")) & nDescripcion
                    bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iRenglon, 0, cmd, bRet, iStatus, iError)
                    Dim DescuentoRenglon As Double = (1 - (1 - .Item("des_art") / 100) * (1 - .Item("des_cli") / 100) * (1 - .Item("des_ofe") / 100)) * 100
                    Dim ss As String = Format(DescuentoRenglon * 100, "0000")
                    If DescuentoRenglon <> 0.0 Then bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iDescuento, 0, ss, bRet, iStatus, iError)

                    If nDescripResto80 <> "" Then _
                        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iMensajeFactura, 0, nDescripResto80, bRet, iStatus, iError)
                    If nDescripResto120 <> "" Then _
                        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iMensajeFactura, 0, nDescripResto120, bRet, iStatus, iError)

                End With
            Next
        End If
        dtRenglones.Dispose()
        dtRenglones = Nothing


        'imprimir pie de pagina
        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iEncabezadoPiePaginaDatos, 7, "ASESOR COMERCIAL : " & CodigoCajero & " " & NombreCajero, bRet, iStatus, iError)
        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iEncabezadoPiePaginaDatos, 8, "NOTA DEBITO DATUM : " & NumeroNotaDebito, bRet, iStatus, iError)


       

        Dim fpCont As Integer = 1
        'Pago CREDITO
        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iPagodirecto, fpCont, "", bRet, iStatus, iError)


        '//// ACTUALIZAR BIT IMPRESION EN FACTURA
        EjecutarSTRSQL(MyConn, lblInfo, " update jsvenencndb set impresa = '1' where numndb = '" & NumeroNotaDebito & "' and id_emp = '" & jytsistema.WorkID & "' ")
        '//// ACTUALIZAR NUMERO DE CONTROL"
        Dim FechaIvaFactura As Date = CDate(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select emision from jsvenencndb where numndb = '" & NumeroNotaDebito & "' and id_emp = '" & jytsistema.WorkID & "' ").ToString)
        'If AbrirPuertoPP1F3(MyConn, lblInfo) Then _
        ActualizaNumeroControl(MyConn, lblInfo, NumeroNotaDebito, FechaIvaFactura, "NDB", "FAC", CodigoCliente)

        CerrarPuertoPP1F3()

        ds = Nothing

    End Sub


    Private Sub ImprimeEncabezadoFacturaPP1F3(ByVal bRet As Boolean, ByVal Cliente As String, ByVal NumeroFactura As String, ByVal FechaEmision As Date, _
                                              ByVal DireccionFiscal As String, ByVal RIF As String, ByVal CondPago As Integer, _
                                              ByVal FechaVencimiento As Date)

        Dim LineaEncab As Integer
        Dim LineasCliente As Integer
        Dim CharRestocliente As Integer
        Dim kCont As Integer

        bRet = EsperaParaImprimirEnPP1F3()

        LineasCliente = Fix(Len("Cliente: " & Cliente) / 39) + 1
        CharRestocliente = (Len("Cliente: " & Cliente) Mod 39)
        LineaEncab = 1
        For kCont = 1 To LineasCliente
            If kCont = LineasCliente Then
                bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iEncabezadoPiePaginaDatos, LineaEncab, Mid("CLIENTE:" & Cliente, 1 + 39 * (kCont - 1), CharRestocliente), bRet, iStatus, iError)
            Else
                bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iEncabezadoPiePaginaDatos, LineaEncab, Mid("CLIENTE: " & Cliente, 1 + 39 * (kCont - 1), 39), bRet, iStatus, iError)
            End If
            LineaEncab += 1
        Next
        Dim strDireccion As String = "DIRECCION:" & DireccionFiscal
        LineasCliente = Fix(Len(strDireccion) / 39) + 1
        CharRestocliente = (Len(strDireccion) Mod 39)
        For kCont = 1 To LineasCliente
            If kCont = LineasCliente Then
                bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iEncabezadoPiePaginaDatos, LineaEncab, Mid(strDireccion, 1 + 39 * (kCont - 1), CharRestocliente), bRet, iStatus, iError)
            Else
                bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iEncabezadoPiePaginaDatos, LineaEncab, Mid(strDireccion, 1 + 39 * (kCont - 1), 39), bRet, iStatus, iError)
            End If
            LineaEncab += 1
        Next
        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iEncabezadoPiePaginaDatos, LineaEncab, "RIF/CI: " & RIF, bRet, iStatus, iError)
        LineaEncab += 1
        bRet = ImprimirLineaFiscalPP1F3(ComandoFiscalPP1F3.iEncabezadoPiePaginaDatos, LineaEncab, IIf(CondPago = CondicionPago.iCredito, "CREDITO, VENCE: " & FormatoFecha(FechaVencimiento), "CONTADO"), bRet, iStatus, iError)
        LineaEncab += 1

    End Sub


    Public Sub ReporteXFiscalPP1F3(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label)

        bRet = AbrirPuertoPP1F3(Myconn, lblInfo)
        bRet = SendCmd(iStatus, iError, "I0X")
        bRet = CerrarPuertoPP1F3()

    End Sub
    Public Sub ReporteZFiscalPP1F3(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label)

        bRet = AbrirPuertoPP1F3(Myconn, lblInfo)
        bRet = SendCmd(iStatus, iError, "I0Z")
        bRet = CerrarPuertoPP1F3()

    End Sub
    Public Function UltimoDocumentoFiscalAclasBixolon(MyConn As MySqlConnection, lblInfo As Label, TipoDocumento As String) As String

        'TipoDocumento FC = Factura ;  NC = Nota Crédito

        Dim bRet As Boolean
        Dim filename As String

        UltimoDocumentoFiscalAclasBixolon = ""

        Try
            bRet = AbrirPuertoPP1F3(MyConn, lblInfo)

            cmd = IIf(TipoDocumento = "FC", "S1", "U0X")
            filename = System.Environment.CurrentDirectory + "\" + "DOC" & NumeroAleatorio(100000) & ".txt"
            bRet = UploadStatusCmd(iStatus, iError, cmd, filename)

            Dim objReader As New StreamReader(filename, System.Text.Encoding.Default, True)
            Dim sLine As String = ""
            Dim str As String = ""
            Do
                sLine = objReader.ReadLine()
                If Not sLine Is Nothing Then
                    str += sLine.Trim
                End If
            Loop Until sLine Is Nothing
            objReader.Close()
            If str.Length > 0 Then UltimoDocumentoFiscalAclasBixolon = Mid(str, IIf(TipoDocumento = "FC", 22, 169), 8)
            My.Computer.FileSystem.DeleteFile(filename)

            CerrarPuertoPP1F3()

        Catch ex As Exception

        End Try
    End Function

    'Public Function UltimaFCFiscalPP1F3(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label) As String
    '    Dim bRet As Boolean
    '    Dim filename As String

    '    UltimaFCFiscalPP1F3 = ""

    '    cmd = "S1"

    '    filename = System.Environment.CurrentDirectory + "\" + "s" & NumeroAleatorio(10000) & ".txt"
    '    bRet = UploadStatusCmd(iStatus, iError, cmd, filename)

    '    Dim sr As New StreamReader(filename)
    '    While sr.Peek() <> -1
    '        Dim s As String = sr.ReadLine()
    '        If String.IsNullOrEmpty(s) Then
    '            Continue While
    '        End If
    '        If Trim(s) <> "" Then UltimaFCFiscalPP1F3 = Mid(s, 22, 8)
    '    End While
    '    sr.Close()
    '    My.Computer.FileSystem.DeleteFile(filename)

    '    CerrarPuertoPP1F3()


    'End Function

    'Public Function UltimaNCFiscalPP1F3(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label) As String
    '    Dim bRet As Boolean
    '    Dim filename As String

    '    UltimaNCFiscalPP1F3 = ""

    '    bRet = AbrirPuertoPP1F3(Myconn, lblInfo)
    '    bRet = ImpresoraEncendida()

    '    cmd = "U0X"
    '    filename = System.Environment.CurrentDirectory + "\" + "RX" & NumeroAleatorio(10000) & ".txt"
    '    bRet = UploadReportCmd(iStatus, iError, cmd, filename)

    '    Dim sr As New System.IO.StreamReader(filename, System.Text.Encoding.Default, True)
    '    While sr.Peek() <> -1
    '        Dim s As String = sr.ReadLine()
    '        If String.IsNullOrEmpty(s) Then
    '            Continue While
    '        End If
    '        If Trim(s) <> "" Then UltimaNCFiscalPP1F3 = Mid(s, 169, 8)
    '    End While
    '    sr.Close()

    '    My.Computer.FileSystem.DeleteFile(filename)
    '    CerrarPuertoPP1F3()


    'End Function

End Module

