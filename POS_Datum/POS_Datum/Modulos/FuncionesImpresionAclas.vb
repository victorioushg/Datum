Imports MySql.Data.MySqlClient
Imports TfhkaNet.IF
Imports TfhkaNet.IF.VE
Imports System
Imports System.IO
Imports System.Collections
Imports FP_AclasBixolon

Module FuncionesImpresionAclas
    
    Private bRet As Boolean
    Private cmd As String

    Private ft As New Transportables
    Private IB As New AclasBixolon

    Private Function IVAFiscal(ByVal TipoIVA As String, condicionIVAEspecial As Boolean) As String
        IVAFiscal = Chr(&H20)
        Select Case Trim(TipoIVA)
            Case ""
                IVAFiscal = Chr(&H20)
            Case "A"
                If condicionIVAEspecial Then
                    IVAFiscal = Chr(&H23)
                Else
                    IVAFiscal = Chr(&H21)
                End If

            Case "B"
                IVAFiscal = Chr(&H22)
            Case "C"
                IVAFiscal = Chr(&H23)
        End Select
    End Function
    Public Function IVAFiscalNC(ByVal TipoIVA As String, condicionIVAEspecial As Boolean) As String
        IVAFiscalNC = "0"
        Select Case Trim(TipoIVA)
            Case ""
                IVAFiscalNC = "0"
            Case "A"
                If condicionIVAEspecial Then
                    IVAFiscalNC = "3"
                Else
                    IVAFiscalNC = "1"
                End If

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
    Public Sub ImprimirFacturaFiscalPP1F3(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroFactura As String, ByVal NombreCliente As String, _
       ByVal CodigoCliente As String, ByVal RIF As String, ByVal DireccionFiscal As String, ByVal FechaEmision As Date, _
       ByVal CondicionPago As String, ByVal FechaVencimiento As Date, ByVal CodigoCajero As String, ByVal NombreCajero As String, _
       ByVal NumeroDeFacturaReal As String, condicionIVAEspecial As Boolean)

        Dim ds As New DataSet

        ' Verifica impresora
        bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox))
        If bRet Then
            'Encendido/Apagado Impresora
            bRet = IB.estatusImpresora()

            'Imprimir Encabezado 
            ImprimeEncabezadoFacturaPP1F3(bRet, NombreCliente, NumeroFactura, FechaEmision, DireccionFiscal, RIF, CondicionPago, FechaVencimiento)

            'imprimir renglones 
            Dim bbCont As Integer = 0
            Dim dtRenglones As DataTable
            Dim nTablaRenglon As String = "tblrenglones"
            ds = DataSetRequery(ds, " select * from jsvenrenpos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaRenglon, lblInfo)
            dtRenglones = ds.Tables(nTablaRenglon)
            If dtRenglones.Rows.Count > 0 Then
                For bbCont = 0 To dtRenglones.Rows.Count - 1
                    With dtRenglones.Rows(bbCont)
                        Dim nDescrip As String = .Item("UNIDAD") & " " & .Item("DESCRIP")
                        Dim Descripcion As String = IIf(Len(nDescrip) > 36, Mid(nDescrip, 1, 36), nDescrip)
                        Dim nDescripResto80 As String = IIf(nDescrip.Length > 36, Mid(nDescrip, 37, IIf(nDescrip.Length > 74, 37, nDescrip.Length)), "")
                        Dim nDescripResto120 As String = IIf(nDescrip.Length > 74, Mid(nDescrip, 74, IIf(nDescrip.Length > 111, 37, nDescrip.Length)), "")

                        Dim DescuentoRenglon As Double = (1 - (1 - .Item("des_art") / 100) * (1 - .Item("des_cli") / 100) * (1 - .Item("des_ofe") / 100)) * 100

                        'Dim nPrecioYDescuento As Double = .Item("PRECIO") * (1 - DescuentoRenglon / 100)
                        cmd = IVAFiscal(.Item("IVA"), condicionIVAEspecial) & FormatoPrecioFiscal(.Item("PRECIO")) & FormatoCantidadFiscal(.Item("CANTIDAD")) & Descripcion

                        bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iRenglon, 0, cmd)

                        Dim ss As String = Format(DescuentoRenglon * 100, "0000")
                        If DescuentoRenglon <> 0.0 Then bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iDescuento, 0, ss)

                        If nDescripResto80 <> "" Then _
                            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iMensajeFactura, 0, nDescripResto80)
                        If nDescripResto120 <> "" Then _
                            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iMensajeFactura, 0, nDescripResto120)

                    End With
                Next
            End If
            dtRenglones.Dispose()
            dtRenglones = Nothing

            'imprimir pie de pagina
            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iEncabezadoPiePaginaDatos, 7, "ASESOR COMERCIAL : " & CodigoCajero & " " & NombreCajero)
            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iEncabezadoPiePaginaDatos, 8, "NUMERO DE FACTURA DATUM : " & NumeroDeFacturaReal)


            'imprimir descuentos
            Dim dtDescuentos As DataTable
            Dim dCont As Integer = 0
            Dim nTablaDescuentos As String = "tblDescuentos"
            ds = DataSetRequery(ds, " select * from jsvendespos where numfac = '" & NumeroFactura & "' and ID_EMP = '" & jytsistema.WorkID & "'", MyConn, nTablaDescuentos, lblInfo)
            dtDescuentos = ds.Tables(nTablaDescuentos)

            If dtDescuentos.Rows.Count > 0 Then
                For dCont = 0 To dtDescuentos.Rows.Count - 1
                    With dtDescuentos.Rows(dCont)
                        bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iSubtotal, 0, "")
                        bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iDescuento, 0, Format(.Item("pordes") * 100, "0000"))
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
            ds = DataSetRequery(ds, " select * from jsvenforpag where numfac = '" & NumeroFactura & "' and origen = 'PVE' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaFP, lblInfo)
            dtFP = ds.Tables(nTablaFP)
            If dtFP.Rows.Count > 0 Then
                For mCont = 0 To dtFP.Rows.Count - 1
                    With dtFP.Rows(mCont)
                        fpCont = ft.InArray(aFormaPagoAbreviada, .Item("FORMAPAG")) + 1
                        bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iNombrePago, fpCont, aFormaPago(fpCont) + " " & Right(.Item("numpag"), 5))
                        ImportePago = .Item("importe")
                        If mCont = dtFP.Rows.Count - 1 Then
                            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iPagodirecto, fpCont, "")
                        Else
                            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iPago, fpCont, Format(ImportePago * 100, FORMATO_ENTERO12))
                        End If

                    End With
                Next
            Else
                'Pago CREDITO
                bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iPagodirecto, fpCont, "")
            End If
            dtFP.Dispose()
            dtFP = Nothing

            '//// ACTUALIZAR NUMERO DE CONTROL"
            Dim FechaIvaFactura As Date = ft.DevuelveScalarFecha(MyConn, "select emision from jsvenencpos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ActualizaNumeroControl(bRet, MyConn, lblInfo, NumeroDeFacturaReal, FechaIvaFactura, "PVE", "FAC", CodigoCliente)

            IB.FinalizarFactura()
            IB.cerrarPuerto()

        End If

        ds = Nothing

    End Sub
    Public Sub ImprimirNotaCreditoPP1F3(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroNotaCredito As String, ByVal Cliente As String, ByVal RIF As String, _
                                      ByVal DireccionFiscal As String, ByVal Telefono As String, ByVal FechaEmision As Date, ByVal CodigoCliente As String, _
                                      ByVal CondicionPago As String, ByVal FechaVencimiento As Date, ByVal CodigoVendedor As String, ByVal Vendedor As String, _
                                      ByVal TotalFactura As Double, ByVal NumeroFacturaAfectada As String, _
                                      ByVal NumeroSerialFiscal As String, NumeroNotaCreditoReal As String, _
                                      condicionIVAEspecial As Boolean)

        Dim ds As New DataSet

        bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox))
        If bRet Then
            bRet = IB.estatusImpresora()
            ImprimeEncabezadoFacturaPP1F3(bRet, Cliente, NumeroNotaCredito, FechaEmision, DireccionFiscal, RIF, _
                                          CondicionPago, FechaVencimiento)

            Dim dtRenglones As DataTable
            Dim nTablaRenglon As String = "tblrenglones"
            ds = DataSetRequery(ds, " select * from jsvenrenpos where numfac = '" & NumeroNotaCredito & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaRenglon, lblInfo)
            dtRenglones = ds.Tables(nTablaRenglon)
            If dtRenglones.Rows.Count > 0 Then
                For bCont = 0 To dtRenglones.Rows.Count - 1
                    With dtRenglones.Rows(bCont)
                        Dim nDescrip As String = .Item("UNIDAD") & " " & .Item("DESCRIP")
                        Dim Descripcion As String = IIf(Len(nDescrip) > 36, Mid(nDescrip, 1, 36), nDescrip)
                        Dim nDescripResto80 As String = IIf(nDescrip.Length > 36, Mid(nDescrip, 37, IIf(nDescrip.Length > 74, 37, nDescrip.Length)), "")
                        Dim nDescripResto120 As String = IIf(nDescrip.Length > 74, Mid(nDescrip, 74, IIf(nDescrip.Length > 111, 37, nDescrip.Length)), "")

                        cmd = "d" & IVAFiscalNC(.Item("IVA"), condicionIVAEspecial) & FormatoPrecioFiscal(.Item("TOTREN") / .Item("cantidad")) & FormatoCantidadFiscal(.Item("cantidad")) & Descripcion
                        bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iRenglon, 0, cmd)

                        If nDescripResto80 <> "" Then _
                            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iMensajeFactura, 0, nDescripResto80)
                        If nDescripResto120 <> "" Then _
                            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iMensajeFactura, 0, nDescripResto120)

                        Dim DescuentoRenglon As Double = (1 - (1 - .Item("des_art") / 100) * (1 - .Item("des_cli") / 100) * (1 - .Item("des_ofe") / 100)) * 100
                        Dim ss As String = Format(DescuentoRenglon * 100, "0000")
                        If DescuentoRenglon <> 0.0 Then bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iDescuento, 0, ss)

                    End With
                Next
            End If

            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iEncabezadoPiePaginaDatos, 7, "NOTA CREDITO     : " & NumeroNotaCreditoReal)
            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iEncabezadoPiePaginaDatos, 8, "FACTURA AFECTADA : " & NumeroFacturaAfectada)

            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iNombrePago, 1, "EFECTIVO")
            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iPagoNotaCredito, 1, Format(TotalFactura * 100, FORMATO_ENTERO12))


            Dim FechaIvaFactura As Date = ft.DevuelveScalarFecha(MyConn, "select emision from jsvenencpos where numfac = '" & NumeroNotaCredito & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ActualizaNumeroControl(bRet, MyConn, lblInfo, NumeroNotaCreditoReal, FechaIvaFactura, "PVE", "FAC", CodigoCliente)

            'ft.mensajeInformativo("SE HA ENVIADO LA NOTA DE CREDITO A LA IMPRESORA FISCAL")

            IB.FinalizarFactura()
            IB.cerrarPuerto()

        End If

    End Sub


    Public Sub Imprimir_NC_SRP812(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroNotaCredito As String, ByVal Cliente As String, ByVal RIF As String, _
                                      ByVal DireccionFiscal As String, ByVal Telefono As String, ByVal FechaEmision As Date, ByVal CodigoCliente As String, _
                                      ByVal CondicionPago As String, ByVal FechaVencimiento As Date, ByVal CodigoVendedor As String, ByVal Vendedor As String, _
                                      ByVal TotalFactura As Double, ByVal NumeroFacturaAfectada As String, _
                                      ByVal NumeroSerialFiscal As String, NumeroNotaCreditoReal As String, _
                                      condicionIVAEspecial As Boolean)

        Dim ds As New DataSet

        bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox))
        If bRet Then

            ImprimeEncabezado_NC_SRP812(bRet, Cliente, RIF, DireccionFiscal, NumeroFacturaAfectada, _
                                       ft.muestraCampoFecha(FechaVencimiento), NumeroSerialFiscal, _
                                         NumeroNotaCreditoReal)

            Dim dtRenglones As DataTable
            Dim nTablaRenglon As String = "tblrenglones"
            ds = DataSetRequery(ds, " select * from jsvenrenpos where numfac = '" & NumeroNotaCredito & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaRenglon, lblInfo)
            dtRenglones = ds.Tables(nTablaRenglon)
            If dtRenglones.Rows.Count > 0 Then
                For bCont = 0 To dtRenglones.Rows.Count - 1
                    With dtRenglones.Rows(bCont)
                        Dim nDescrip As String = .Item("UNIDAD") & " " & .Item("DESCRIP")
                        Dim Descripcion As String = IIf(nDescrip.Length > 127, Mid(nDescrip, 1, 127), nDescrip)

                        cmd = "d" & IVAFiscalNC(.Item("IVA"), condicionIVAEspecial) & FormatoPrecioFiscal(.Item("TOTREN") / .Item("cantidad")) & FormatoCantidadFiscal(.Item("cantidad")) & Descripcion
                        bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iRenglon, 0, cmd)

                        Dim DescuentoRenglon As Double = (1 - (1 - .Item("des_art") / 100) * (1 - .Item("des_cli") / 100) * (1 - .Item("des_ofe") / 100)) * 100
                        Dim ss As String = Format(DescuentoRenglon * 100, "0000")
                        If DescuentoRenglon <> 0.0 Then bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iDescuento, 0, ss)

                    End With
                Next
            End If

            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iRenglon, 1, "101")

            Dim FechaIvaFactura As Date = ft.DevuelveScalarFecha(MyConn, "select emision from jsvenencpos where numfac = '" & NumeroNotaCredito & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ActualizaNumeroControl(bRet, MyConn, lblInfo, NumeroNotaCreditoReal, FechaIvaFactura, "PVE", "FAC", CodigoCliente)

            'ft.mensajeInformativo("SE HA ENVIADO LA NOTA DE CREDITO A LA IMPRESORA FISCAL")

            IB.FinalizarFactura()
            IB.cerrarPuerto()

        End If


    End Sub


    Private Sub ImprimeEncabezadoFacturaPP1F3(bRet As Boolean, ByVal Cliente As String, ByVal NumeroFactura As String, ByVal FechaEmision As Date, _
                                              ByVal DireccionFiscal As String, ByVal RIF As String, ByVal CondPago As Integer, _
                                              ByVal FechaVencimiento As Date)

        Dim LineaEncab As Integer
        Dim LineasCliente As Integer
        Dim CharRestocliente As Integer
        Dim kCont As Integer

        LineasCliente = Fix(Len("CLIENTE:" & Cliente) / 40) + 1
        CharRestocliente = (Len("CLIENTE:" & Cliente) Mod 40)
        LineaEncab = 1
        For kCont = 1 To LineasCliente
            If kCont = LineasCliente Then
                bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iEncabezadoPiePaginaDatos, _
                                                            LineaEncab, Mid("CLIENTE:" & Cliente, 1 + 40 * (kCont - 1), CharRestocliente))
            Else
                bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iEncabezadoPiePaginaDatos, _
                                                                  LineaEncab, Mid("CLIENTE:" & Cliente, 1 + 40 * (kCont - 1), 40))
            End If
            LineaEncab += 1
        Next

        Dim strDireccion As String = "DIRECCION:" & DireccionFiscal
        LineasCliente = Fix(Len(strDireccion) / 40) + 1
        CharRestocliente = (Len(strDireccion) Mod 40)
        For kCont = 1 To LineasCliente
            If kCont = LineasCliente Then
                bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iEncabezadoPiePaginaDatos, _
                                                                  LineaEncab, Mid(strDireccion, 1 + 40 * (kCont - 1), CharRestocliente))
            Else
                bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iEncabezadoPiePaginaDatos, _
                                                                  LineaEncab, Mid(strDireccion, 1 + 40 * (kCont - 1), 40))
            End If
            LineaEncab += 1
        Next
        bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iEncabezadoPiePaginaDatos, _
                                                          LineaEncab, "RIF/CI: " & RIF)
        LineaEncab += 1
        bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iEncabezadoPiePaginaDatos, _
                                                          LineaEncab, IIf(CondPago = CondicionPago.iCredito, "CREDITO, VENCE: " & ft.muestracampofecha(FechaVencimiento), "CONTADO"))
        LineaEncab += 1

    End Sub


    Private Sub ImprimeEncabezado_NC_SRP812(bRet As Boolean, ByVal Cliente As String, _
                                            ByVal RIF As String, _
                                            ByVal DireccionFiscal As String, _
                                            NumeroFacturaAfectada As String, _
                                            FechaFacturaAfectada As String, _
                                            SerialMaqFacturaAfectada As String, _
                                            NumeroNC_Real As String)
        Dim LineaEncab As Integer = 0
        Dim kCont As Integer

        If Cliente.Length > 40 Then Cliente = Cliente.Substring(0, 40)

        '' RAZON SOCIAL
        bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iRazonSocial, LineaEncab, Cliente)
        '' RIF
        bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iRif, LineaEncab, RIF)
        '' NUMERO FACTURA AFECTADA
        bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iFacturaAfectada, LineaEncab, NumeroFacturaAfectada)
        '' SERIAL MAQUINA FACTURA AFECTADA
        bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iSerialNDNC, LineaEncab, SerialMaqFacturaAfectada)
        '' FECHA FACTURA AFECTADA
        bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iFechaNDNC, LineaEncab, FechaFacturaAfectada)

        '' NUMERO NOTA CREDITO EN SISTEMA
        bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iEncabezadoPiePaginaDatos, LineaEncab, _
                                "NC Sist N° : " & NumeroNC_Real)
        LineaEncab += 1

        '' DIRECCION
        Dim strDireccion As String = "DIRECCION:" & DireccionFiscal
        Dim LineasCliente As Integer = Fix(Len(strDireccion) / 40) + 1
        Dim CharRestocliente As Integer = (Len(strDireccion) Mod 40)
        For kCont = 1 To LineasCliente
            If kCont = LineasCliente Then
                bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iEncabezadoPiePaginaDatos, _
                                                                  LineaEncab, Mid(strDireccion, 1 + 40 * (kCont - 1), CharRestocliente))
            Else
                bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iEncabezadoPiePaginaDatos, _
                                                                  LineaEncab, Mid(strDireccion, 1 + 40 * (kCont - 1), 40))
            End If
            LineaEncab += 1
        Next

    End Sub


    Public Function ReimprimirUltimoDocumentoFiscal(Myconn As MySqlConnection, lblInfo As Label, Documento As AclasBixolon.tipoDocumentoFiscal) As Boolean

        Dim puerto As String = PuertoImpresoraFiscal(Myconn, lblInfo, jytsistema.WorkBox)
        bRet = IB.abrirPuerto(puerto)

        Dim ultimoDocumentoImpreso As String = IB.UltimoDocumentoFiscal(Documento)
        bRet = IB.estatusImpresora()
        bRet = IB.ReimprimirUltimoDocumentoFiscal(Documento, ultimoDocumentoImpreso)
        IB.cerrarPuerto()

    End Function

End Module
