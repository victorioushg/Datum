Imports MySql.Data.MySqlClient
Module FuncionesImpresionPnP
    '//////////////////////////////////////////////////////////
    '// Fiscal Printer library definitions
    '//////////////////////////////////////////////////////////
    Public Declare Function PFabrefiscal Lib "PNPDLL.DLL" (ByVal Razon As String, ByVal RIF As String) As String
    Public Declare Function PFtotal Lib "PNPDLL.DLL" () As String
    Public Declare Function PFrepz Lib "PNPDLL.DLL" () As String
    Public Declare Function PFrepx Lib "PNPDLL.DLL" () As String
    Public Declare Function PFrenglon Lib "PNPDLL.DLL" (ByVal Descripcion As String, ByVal Cantidad As String, ByVal Monto As String, ByVal IVA As String) As String
    Public Declare Function PFabrepuerto Lib "PNPDLL.DLL" (ByVal Numero As String) As String
    Public Declare Function PFcierrapuerto Lib "PNPDLL.DLL" () As String
    Public Declare Function PFDisplay950 Lib "PNPDLL.DLL" (ByVal edlinea As String) As String
    Public Declare Function PFAbreNF Lib "PNPDLL.DLL" () As String
    Public Declare Function PFLineaNF Lib "PNPDLL.DLL" (ByVal edlinea As String) As String
    Public Declare Function PFCierraNF Lib "PNPDLL.DLL" () As String
    Public Declare Function PFCortar Lib "PNPDLL.DLL" () As String
    Public Declare Function PFTfiscal Lib "PNPDLL.DLL" (ByVal edlinea As String) As String
    Public Declare Function PFparcial Lib "PNPDLL.DLL" () As String
    Public Declare Function PFSerial Lib "PNPDLL.DLL" () As String
    Public Declare Function PFtoteconomico Lib "PNPDLL.DLL" () As String
    Public Declare Function PFCancelaDoc Lib "PNPDLL.DLL" (ByVal edlinea As String, ByVal monto As String) As String
    Public Declare Function PFGaveta Lib "PNPDLL.DLL" () As String
    Public Declare Function PFDevolucion Lib "PNPDLL.DLL" (ByVal razon As String, ByVal rif As String, ByVal comp As String, ByVal maqui As String, ByVal fecha As String, ByVal hora As String) As String
    Public Declare Function PFSlipON Lib "PNPDLL.DLL" () As String
    Public Declare Function PFSlipOFF Lib "PNPDLL.DLL" () As String
    Public Declare Function PFestatus Lib "PNPDLL.DLL" (ByVal edlinea As String) As String
    Public Declare Function PFreset Lib "PNPDLL.DLL" () As String
    Public Declare Function PFendoso Lib "PNPDLL.DLL" (ByVal campo1 As String, ByVal campo2 As String, ByVal campo3 As String, ByVal tipoendoso As String) As String
    Public Declare Function PFvalida675total Lib "PNPDLL.DLL" (ByVal campo1 As String, ByVal campo2 As String, ByVal campo3 As String, ByVal campo4 As String) As String
    Public Declare Function PFCheque2 Lib "PNPDLL.DLL" (ByVal mon As String, ByVal ben As String, ByVal fec As String, ByVal c1 As String, ByVal c2 As String, ByVal c3 As String, ByVal c4 As String, ByVal campo1 As String, ByVal campo2 As String) As String
    Public Declare Function PFcambiofecha Lib "PNPDLL.DLL" (ByVal edfecha As String, ByVal edhora As String) As String
    Public Declare Function PFcambiatasa Lib "PNPDLL.DLL" (ByVal t1 As String, ByVal t2 As String, ByVal t3 As String) As String
    Public Declare Function PFBarra Lib "PNPDLL.DLL" (ByVal edbarra As String) As String
    Public Declare Function PFvoltea Lib "PNPDLL.DLL" () As String
    Public Declare Function PFLeereloj Lib "PNPDLL.DLL" () As String
    Public Declare Function PFrepMemNF Lib "PNPDLL.DLL" (ByVal desf As String, ByVal hasf As String, ByVal modmem As String) As String
    Public Declare Function PFRepMemoriaNumero Lib "PNPDLL.DLL" (ByVal desn As String, ByVal hasn As String, ByVal modmem As String) As String
    Public Declare Function PFCambtipoContrib Lib "PNPDLL.DLL" (ByVal tip As String) As String
    Public Declare Function PFultimo Lib "PNPDLL.DLL" () As String
    Public Declare Function PFTipoImp Lib "PNPDLL.DLL" (ByVal edtexto As String) As String

    Public iError As Long
    Public iStatus As Long
    Private bRet As String
    Public cmd As String

    Private ft As New Transportables

    Private nLinea As String = "----------------------------------------"
    Private Function AbrirPuertoPnP(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label) As Boolean

        Dim PuertoImpresoraFiscalPnP As String = PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox)
        If PuertoImpresoraFiscalPnP = "0" Then PuertoImpresoraFiscalPnP = "COM1"
        Dim nPort As String = Right(PuertoImpresoraFiscalPnP, 1)

        bRet = ""
        Try
            While bRet <> "OK"
                bRet = PFabrepuerto(nPort)
                If bRet <> "OK" Then MsgBox("Impresora fiscal presenta problemas. Verifique por favor...")
            End While
            Return True

        Catch ex As Exception
            ft.mensajeCritico("VERIFIQUE PUERTO O IMPRESORA FISCAL... (" + ex.Message.ToString + ")")
            Return False
        End Try


    End Function

    Private Function CerrarPuertoPnP() As Boolean
        bRet = PFcierrapuerto()
        If bRet = "OK" Then CerrarPuertoPnP = True
    End Function

    Private Function ResetMaquina() As Boolean
        If PFreset() = "OK" Then Return True
    End Function
    Private Function IVAFiscalEpson(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                    ByVal FechaIVA As Date, ByVal TipoIVA As String) As String
        IVAFiscalEpson = Format(PorcentajeIVA(MyConn, lblInfo, FechaIVA, TipoIVA) * 100, "0000")
    End Function


    Private Function FormatoFechaFiscalEpson(ByVal sFecha As Date) As String
        FormatoFechaFiscalEpson = Format(sFecha, "ddMMYY")
    End Function
    Private Function FormatoHoraFiscalEpson(ByVal sHora As Date) As String
        FormatoHoraFiscalEpson = Format(sHora, "HHmm")
    End Function

    Private Function FormatoCantidadFiscalEpson(ByVal sNumero As Double) As String
        FormatoCantidadFiscalEpson = Format(sNumero, sFormatoCantidadEpson)
    End Function
    Private Function FormatoPrecioFiscalEpson(ByVal sNumero As Double) As String
        FormatoPrecioFiscalEpson = Format(sNumero, sFormatoNumeroEpson)
    End Function
    Public Sub ImprimirFacturaFiscalPnP(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroFactura As String, ByVal NombreCliente As String, _
       ByVal CodigoCliente As String, ByVal RIF As String, ByVal DireccionFiscal As String, ByVal FechaEmision As Date, _
       ByVal CondicionPago As String, ByVal FechaVencimiento As Date, ByVal CodigoCajero As String, ByVal NombreCajero As String)

        Dim ds As New DataSet

        ' Verifica impresora
        AbrirPuertoPnP(MyConn, lblInfo)

        'Imprimir Encabezado 
        ImprimeEncabezadoFacturaPnP(NombreCliente, NumeroFactura, FechaEmision, DireccionFiscal, RIF, CondicionPago, FechaVencimiento)

        'Imprimir renglones 
        Dim bbCont As Integer = 0
        Dim dtRenglones As DataTable
        Dim nTablaRenglon As String = "tblrenglones"
        ds = DataSetRequery(ds, " select * from jsvenrenpos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaRenglon, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglon)
        If dtRenglones.Rows.Count > 0 Then
            For bbCont = 0 To dtRenglones.Rows.Count - 1
                With dtRenglones.Rows(bbCont)
                    Dim Descripcion As String = .Item("UNIDAD") & " " & .Item("DESCRIP") '    IIf(Len(.Item("descrip")) > 35, Mid(.Item("descrip"), 1, 35), .Item("descrip"))
                    Dim PrecioJusto As Double = .Item("totrendes") / .Item("cantidad")

                    bRet = ""
                    While bRet <> "OK"
                        bRet = PFrenglon(Descripcion, FormatoCantidadFiscalEpson(.Item("CANTIDAD")), FormatoPrecioFiscalEpson(.Item("TOTRENDES") / .Item("CANTIDAD")), IVAFiscalEpson(MyConn, lblInfo, FechaEmision, .Item("IVA")))
                    End While

                End With
            Next
        End If
        dtRenglones.Dispose()
        dtRenglones = Nothing

        'IMPRIMIR SUBTOTAL
        bRet = ""
        While bRet <> "OK"
            bRet = PFparcial()
        End While


        'imprimir formas de pago 
        Dim mCont As Integer
        Dim dtFP As DataTable
        Dim nTablaFP As String = "tblfp"
        Dim ImportePago As Double = 0

        Dim fpCont As Integer = 1
        ds = DataSetRequery(ds, " select * from jsvenforpag where numfac = '" & NumeroFactura & "' and origen = 'PVE' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaFP, lblInfo)
        dtFP = ds.Tables(nTablaFP)
        If dtFP.Rows.Count > 0 Then
            For mCont = 0 To dtFP.Rows.Count - 1
                With dtFP.Rows(mCont)
                    fpCont = ft.InArray(aFormaPagoAbreviada, .Item("FORMAPAG"))
                    bRet = ""
                    While bRet <> "OK"
                        bRet = PFTfiscal(aFormaPago(fpCont - 1) & RellenaCadenaConCaracter(Format(.Item("IMPORTE"), sFormatoNumeroEpson), "D", 40 - aFormaPago(fpCont - 1).Length))
                    End While

                End With
            Next
        End If
        dtFP.Dispose()
        dtFP = Nothing

        'IMPRIMIR TOTAL Y CIERRE DE FASCTURA
        bRet = ""
        While bRet <> "OK"
            bRet = PFtotal()
        End While

        CerrarPuertoPnP()

        ds = Nothing

    End Sub
    Public Sub ImprimirNotaCreditoPnP(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroNotaCredito As String, _
                                      ByVal NumeroFacturaAfectada As String, ByVal SerialMaquinaFacturaAfectada As String, _
                                      ByVal FechaFacturaAfectada As String, ByVal HoraFacturaAfectada As String, _
                                      ByVal Cliente As String, ByVal RIF As String, _
                                      ByVal DireccionFiscal As String, ByVal FechaEmision As Date, _
                                      ByVal FechaVencimiento As Date)

        Dim ds As New DataSet

        AbrirPuertoPnP(MyConn, lblInfo)
        ImprimeEncabezadoDevolucionPnP(Cliente, NumeroFacturaAfectada, FechaEmision, SerialMaquinaFacturaAfectada, FechaFacturaAfectada, _
                                       HoraFacturaAfectada, DireccionFiscal, RIF)


        Dim dtRenglones As DataTable
        Dim nTablaRenglon As String = "tblrenglones"
        ds = DataSetRequery(ds, " select * from jsvenrenpos where numfac = '" & NumeroNotaCredito & "' and tipo = 0 and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaRenglon, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglon)
        If dtRenglones.Rows.Count > 0 Then
            For bCont = 0 To dtRenglones.Rows.Count - 1
                With dtRenglones.Rows(bCont)
                    Dim Descripcion As String = IIf(Len(.Item("descrip")) > 39, Mid(.Item("descrip"), 1, 39), .Item("descrip"))
                    bRet = ""
                    While bRet <> "OK"
                        bRet = PFrenglon(Descripcion, FormatoCantidadFiscalEpson(.Item("cantidad")), FormatoPrecioFiscalEpson(.Item("TOTREN") / .Item("cantidad")), IVAFiscalEpson(MyConn, lblInfo, FechaEmision, .Item("IVA")))
                    End While
                End With
            Next
        End If

        bRet = ""
        While bRet <> "OK"
            bRet = PFtotal()
        End While

        ft.mensajeInformativo("Se ha enviado la nota crédito a la impresora fiscal ")
        CerrarPuertoPnP()

    End Sub

    Private Sub ImprimeEncabezadoFacturaPnP(ByVal Cliente As String, ByVal NumeroFactura As String, ByVal FechaEmision As Date, _
                                              ByVal DireccionFiscal As String, ByVal RIF As String, ByVal CondPago As Integer, _
                                              ByVal FechaVencimiento As Date)

        bRet = ""
        While bRet <> "OK"
            bRet = PFabrefiscal(Cliente, RIF)
        End While
        Dim strDireccion As String = "DIRECCION:" & DireccionFiscal

        bRet = ""
        While bRet <> "OK"
            bRet = PFTfiscal(IIf(CondPago = CondicionPago.iCredito, "CREDITO, VENCE: " & ft.muestracampofecha(FechaVencimiento), "CONTADO"))
        End While
        bRet = ""
        While bRet <> "OK"
            bRet = PFTfiscal(nLinea)
        End While

    End Sub
    Private Sub ImprimeEncabezadoDevolucionPnP(ByVal Cliente As String, ByVal NumeroFacturaAfectada As String, ByVal FechaEmision As Date, _
                                               ByVal SerialMáquinaFacturaAfectada As String, ByVal FechaFacturaAfectada As String, ByVal HoraFacturaAfectada As String, _
                                               ByVal DireccionFiscal As String, ByVal RIF As String)

        bRet = ""
        While bRet <> "OK"
            bRet = PFDevolucion(Cliente, RIF, NumeroFacturaAfectada, SerialMáquinaFacturaAfectada, FechaFacturaAfectada, HoraFacturaAfectada)
        End While
        Dim strDireccion As String = "DIRECCION:" & DireccionFiscal

    End Sub


    Public Sub ReporteXFiscalPnP(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label)

        bRet = AbrirPuertoPnP(Myconn, lblInfo)
        bRet = PFrepx()
        bRet = CerrarPuertoPnP()

    End Sub
    Public Sub ReporteZFiscalPnP(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label)

        bRet = AbrirPuertoPnP(Myconn, lblInfo)
        bRet = PFrepz()
        bRet = CerrarPuertoPnP()

    End Sub

    Public Function EstatusImpresoraPnP(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal TipoEstatus As String) As String

        Dim bbRet As String = ""
        Dim repN As String = ""
        PFabrepuerto(CStr(PuertoImpresoraFiscal(myConn, lblInfo, jytsistema.WorkBox)))
        While bbRet <> "OK"
            bbRet = PFestatus(TipoEstatus)
        End While
        repN = PFultimo()
        PFcierrapuerto()

        Return repN

    End Function

    Public Function UltimaFCFiscalPnP(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label) As String
        Return Split(EstatusImpresoraPnP(MyConn, lblInfo, "N"), ",")(9)
    End Function

    Public Function UltimaNCFiscalPnP(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label) As String
        Return Split(EstatusImpresoraPnP(Myconn, lblInfo, "T"), ",")(7)
    End Function

End Module
