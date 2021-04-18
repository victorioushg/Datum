Imports MySql.Data.MySqlClient
Imports FP_AclasBixolon
Module FuncionesPuntosDeVenta

    Private IB As New AclasBixolon
    Private ft As New Transportables

    Private bRet As Boolean
    Private iStatus As Long
    Private iError As Long

    Private puerto As String = "COM1"

    Public Function NumeroUltimaFactura(ByVal MyConn As MySqlConnection, lblInfo As Label, ByVal TipoImpresora As Integer) As String
        NumeroUltimaFactura = ""

        Select Case TipoImpresora
            Case 2, 5, 6 'PP1F3
                bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox))
                If bRet Then
                    NumeroUltimaFactura = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.Factura)
                    IB.cerrarPuerto()
                End If
            Case 7 'BIXOLON SRP812
                bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox))
                If bRet Then
                    NumeroUltimaFactura = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.FC_SRP812)
                    IB.cerrarPuerto()
                End If
            Case 3 'Bematech
                Dim sr As New System.IO.StreamReader("C:\retorno.txt", System.Text.Encoding.Default, True)
                While sr.Peek() <> -1
                    Dim s As String = sr.ReadLine()
                    If String.IsNullOrEmpty(s) Then
                        Continue While
                    End If
                    If Mid(s, 1, 20) = "Contador de Factura:" Then NumeroUltimaFactura = Right(RTrim(s), 6)
                End While
                sr.Close()
            Case Else
                NumeroUltimaFactura = ""
        End Select

        Return NumeroUltimaFactura
    End Function



    Public Function NumeroUltimaNC(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal TipoImpresora As Integer) As String

        NumeroUltimaNC = ""

        Select Case TipoImpresora
            Case 2, 5, 6 'PP1F3
                bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox))
                If bRet Then
                    NumeroUltimaNC = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.NotaCredito)
                    IB.cerrarPuerto()
                End If
            Case 7 'BIXOLON SRP812
                bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox))
                If bRet Then
                    NumeroUltimaNC = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.NC_SRP812)
                    IB.cerrarPuerto()
                End If
            Case 3 'Bematech
                Dim sr As New System.IO.StreamReader("C:\retorno.txt", System.Text.Encoding.Default, True)
                While sr.Peek() <> -1
                    Dim s As String = sr.ReadLine()
                    If String.IsNullOrEmpty(s) Then
                        Continue While
                    End If
                    If Mid(s, 1, 28) = "Contador de Nota de Crédito:" Then NumeroUltimaNC = Right(RTrim(s), 6)
                End While
                sr.Close()
            Case Else
                NumeroUltimaNC = ""
        End Select

        Return NumeroUltimaNC

    End Function

    Function CajaCerrada(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Cajero As String, ByVal Caja As String) As Boolean

        Return ft.DevuelveScalarBooleano(MyConn, " select estatus from jsveninipos where " _
              & " FECHA = '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' AND " _
              & " CODCAJ = '" & Caja & "' AND " _
              & " CODVEN = '" & Cajero & "' AND " _
              & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
              & " ID_EMP = '" & jytsistema.WorkID & "'")

    End Function

    Function FacturasPendientes(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Caja As String, ByVal Cajero As String) As Boolean

        Dim iCont As Integer = ft.DevuelveScalarEntero(MyConn, " select ifnull(count(*),0) from jsvenencpos where " _
              & " EMISION = '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' AND " _
              & " CODCAJ = '" & Caja & "' AND " _
              & " CODVEN = '" & Cajero & "' AND " _
              & " ESTATUS = " & EstatusFactura.ePorConfirmar & " AND " _
              & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
              & " ID_EMP = '" & jytsistema.WorkID & "'")

        If iCont > 0 Then FacturasPendientes = True

    End Function

    Public Sub GuardarFacturaNotaCredito_PuntoDeVenta(MyConn As MySqlConnection, NumeroDocumento As String, ByVal NumeroSerialFiscal As String, _
                                 ByVal DocumentoAnterior As String, ByVal NumeroSerialFiscalAnterior As String, _
                                 ByVal Tipo As Integer, _
                                 Optional NumeroFacturaFiscal As String = "")

        Dim pb As New Progress_Bar
        pb.WindowTitle = "GUARDANDO FACTURA..."
        pb.TimeOut = 60
        Dim nTotal As Int16 = 13

        '0.- ENCABEZADO fACTURA
        pb.OverallProgressText = "Encabezado Factura..."
        pb.OverallProgressValue = 1 / nTotal * 100
        ft.Ejecutar_strSQL(MyConn, " update jsvenencpos set numfac = '" & NumeroDocumento & "', " _
                       & " numserial = '" & NumeroSerialFiscal & "', tipo = " & Tipo & ", " _
                       & " estatus = " & EstatusFactura.eProcesada & ", refer = '" & NumeroFacturaFiscal & "' " _
                       & " where " _
                       & " numfac = '" & DocumentoAnterior & "' and " _
                       & " id_emp = '" & jytsistema.WorkID & "'")

        '1.- RENGLONES FACTURA
        pb.OverallProgressText = "Renglones Factura..."
        pb.OverallProgressValue = 2 / nTotal * 100
        ft.Ejecutar_strSQL(MyConn, " update jsvenrenpos set numfac = '" & NumeroDocumento & "', numserial = '" & NumeroSerialFiscal & "', tipo = " & Tipo & " where " _
                       & " numfac = '" & DocumentoAnterior & "' and " _
                       & " id_emp = '" & jytsistema.WorkID & "'")

        '2.- IVA FACTURA
        pb.OverallProgressText = "IVA Factura..."
        pb.OverallProgressValue = 3 / nTotal * 100
        ft.Ejecutar_strSQL(MyConn, " update jsvenivapos set numfac = '" & NumeroDocumento & "', numserial = '" & NumeroSerialFiscal & "', tipo = " & Tipo & " where " _
                           & " numfac = '" & DocumentoAnterior & "' and " _
                           & " id_emp = '" & jytsistema.WorkID & "'")

        '3.- Decuentos FACTURA
        pb.OverallProgressText = "Descuentos Factura..."
        pb.OverallProgressValue = 4 / nTotal * 100
        ft.Ejecutar_strSQL(MyConn, " update jsvendespos set numfac = '" & NumeroDocumento & "', numserial = '" & NumeroSerialFiscal & "', tipo = " & Tipo & " where " _
                           & " numfac = '" & DocumentoAnterior & "' and " _
                           & " id_emp = '" & jytsistema.WorkID & "'")

        '4.- Forma de pago Cesta Ticket
        pb.OverallProgressText = "Forma de Pago Cesta Ticket..."
        pb.OverallProgressValue = 5 / nTotal * 100
        ft.Ejecutar_strSQL(MyConn, " update jsvenforpag set numpag = '" & NumeroDocumento & "' where " _
                           & " numfac = '" & DocumentoAnterior & "' and " _
                           & " origen = 'PVE' and formapag = 'CT' and id_emp = '" & jytsistema.WorkID & "'")

        '5.- Formas de pago
        pb.OverallProgressText = "Formas de pagoa..."
        pb.OverallProgressValue = 6 / nTotal * 100
        ft.Ejecutar_strSQL(MyConn, " update jsvenforpag set numfac = '" & NumeroDocumento & "', numserial = '" & NumeroSerialFiscal & "' where " _
                           & " numfac = '" & DocumentoAnterior & "' and " _
                           & " origen = 'PVE' and id_emp = '" & jytsistema.WorkID & "'")

        '6.- Movimientos puntos de venta
        pb.OverallProgressText = "Movimientos puntos de venta..."
        pb.OverallProgressValue = 7 / nTotal * 100
        ft.Ejecutar_strSQL(MyConn, " update jsventrapos set nummov = '" & NumeroDocumento & "' where " _
                           & " nummov = '" & DocumentoAnterior & "' and " _
                           & " caja = '" & jytsistema.WorkBox & "' and " _
                           & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                           & " id_emp = '" & jytsistema.WorkID & "'")

        '7.- Movimientos inventarios
        pb.OverallProgressText = "Movimientos de inventario..."
        pb.OverallProgressValue = 8 / nTotal * 100
        ft.Ejecutar_strSQL(MyConn, " update jsmertramer set numdoc = '" & NumeroDocumento & "' where " _
                           & " numorg = '" & DocumentoAnterior & "' and " _
                           & " tipomov = '" & IIf(Tipo = 0, "SA", "EN") & "' and " _
                           & " origen = 'PVE' and " _
                           & " id_emp = '" & jytsistema.WorkID & "'")

        '8.- Movimientos de Tallas/Colores
        pb.OverallProgressText = "Movimientos Tallas y Colores..."
        pb.OverallProgressValue = 9 / nTotal * 100
        If ExisteTabla(MyConn, jytsistema.WorkDataBase, "jsmermovcatcol") Then _
            ft.Ejecutar_strSQL(MyConn, " update jsmermovcatcol set nummov = '" & NumeroDocumento & "' where " _
                & " nummov = '" & DocumentoAnterior & "' and " _
                & " tipomov = 'SA' and origen = 'PVE' and " _
                & " id_emp = '" & jytsistema.WorkID & "'")

        '9.- Movimientos de cilindros
        pb.OverallProgressText = "Movimientos de Cilindros..."
        pb.OverallProgressValue = 10 / nTotal * 100
        If ExisteTabla(MyConn, jytsistema.WorkDataBase, "jsmertracil") Then _
                ft.Ejecutar_strSQL(MyConn, " update jsmertracil set numdoc = '" & NumeroDocumento & "' where " _
                    & " numdoc = '" & DocumentoAnterior & "' and " _
                    & " origen = 'PVE' and " _
                    & " id_emp = '" & jytsistema.WorkID & "'  ")

        '10.- Renglones de ordenes de pedidos
        pb.OverallProgressText = "Renglones ordenes de pedidos..."
        pb.OverallProgressValue = 11 / nTotal * 100
        If ExisteTabla(MyConn, jytsistema.WorkDataBase, "jsmerencord") Then _
                ft.Ejecutar_strSQL(MyConn, " update jsmerencord set numfac = '" & NumeroDocumento & "' where " _
                    & " numfac = '" & DocumentoAnterior & "' and " _
                    & " id_emp = '" & jytsistema.WorkID & "' ")

        '11.- Catalogo y movimientos de cilindros
        pb.OverallProgressText = "Catálogo y movimientos de cilindros..."
        pb.OverallProgressValue = 12 / nTotal * 100
        If ExisteTabla(MyConn, jytsistema.WorkDataBase, "jsmertracil") And ExisteTabla(MyConn, jytsistema.WorkDataBase, "jsmercatcil") Then _
                ft.Ejecutar_strSQL(MyConn, " UPDATE jsmercatcil a, jsmertracil b  " _
                                    & " set a.estatus = 1 " _
                                    & " Where " _
                                    & " b.numdoc = '" & NumeroDocumento & "' and " _
                                    & " a.codart = b.item and " _
                                    & " a.serial_1 = b.serial_1 and " _
                                    & " a.id_emp = b.id_emp and " _
                                    & " a.id_emp = '" & jytsistema.WorkID & "'")

        '12.- Movimientos de Cajas e Impresora Fiscal
        pb.OverallProgressText = "Movimientos de Cajas e impresora fiscal..."
        pb.OverallProgressValue = 100
        If Tipo = 0 Then
            ft.Ejecutar_strSQL(MyConn, " update jsvencatcaj a, jsconcatimpfis b set b.ultima_factura = '" & NumeroDocumento & "' " _
                                           & " where " _
                                           & " a.impre_fiscal = b.codigo and a.id_emp = b.id_emp AND " _
                                           & " a.codcaj = '" & jytsistema.WorkBox & "' and " _
                                           & " a.id_emp = '" & jytsistema.WorkID & "'")

        Else
            ft.Ejecutar_strSQL(MyConn, " update jsvencatcaj a, jsconcatimpfis b set b.ultima_notacredito = '" & NumeroDocumento & "' " _
                                           & " where " _
                                           & " a.impre_fiscal = b.codigo and a.id_emp = b.id_emp AND " _
                                           & " a.codcaj = '" & jytsistema.WorkBox & "' and " _
                                           & " a.id_emp = '" & jytsistema.WorkID & "'")
        End If

    End Sub


    Public Sub IncluirMovimientosPuntoDeVentaEnCaja(ByVal MyConn As MySqlConnection, ds As DataSet, ByVal NumeroFactura As String, _
                                                     ByVal NumeroSerialFiscal As String, _
                                                     Factura_NotaCredito As Integer, MontoFacturaNotaCredito As Double, _
                                                     FechaVencimiento As Date,
                                                     Optional CondicionDePago As CondicionPago = CondicionPago.iContado)


        Dim lblInfo As New Label
        'Factura_NotaCredito = 0 Factura 1 NotaCredito 
        If Factura_NotaCredito = 0 Then
            If CondicionDePago = CondicionPago.iContado Then
                Dim dtFP As DataTable = ft.AbrirDataTable(ds, "tblFP", MyConn, " select * from jsvenforpag where " _
                                                          & " numfac = '" & NumeroFactura & "' AND " _
                                                          & " id_emp = '" & jytsistema.WorkID & "' ")
                If dtFP.Rows.Count > 0 Then
                    Dim iCont As Integer
                    For iCont = 0 To dtFP.Rows.Count - 1
                        With dtFP.Rows(iCont)
                            InsertarModificarPOSWork(MyConn, lblInfo, True, jytsistema.WorkBox, jytsistema.sFechadeTrabajo, _
                                                     "PVE", "EN", NumeroFactura, NumeroSerialFiscal, .Item("formapag"), _
                                                     .Item("numpag"), .Item("nompag"), .Item("importe"), _
                                                     CDate(.Item("vence").ToString), 1, jytsistema.sUsuario)
                        End With
                    Next
                End If
                dtFP.Dispose()
                dtFP = Nothing

            Else
                InsertarModificarPOSWork(MyConn, lblInfo, True, jytsistema.WorkBox, jytsistema.sFechadeTrabajo, "PVE", "EN", _
                                         NumeroFactura, NumeroSerialFiscal, "CR", "", _
                                         "", MontoFacturaNotaCredito, FechaVencimiento, 1, jytsistema.sUsuario)
            End If
        Else
            InsertarModificarPOSWork(MyConn, lblInfo, True, jytsistema.WorkBox, jytsistema.sFechadeTrabajo, "PVE", "SA", _
                                     NumeroFactura, _
                                NumeroSerialFiscal, "EF", "", _
                                "", MontoFacturaNotaCredito, FechaVencimiento, 1, jytsistema.sUsuario)
        End If

    End Sub

    Public Sub IncluirClientePV(ByVal MyConn As MySqlConnection, ByVal CodigoCliente As String, RIF_CI_Cliente As String, _
                                 NombreCliente As String, DireccionCliente As String, TelefonoCliente As String)

        Dim sCIF As String = ""
        Dim TipoRazon As String = RIF_CI_Cliente.Replace("_", "").Replace(" ", "").Split("-")(0)
        Dim Documento As String = RIF_CI_Cliente.Replace("_", "").Replace(" ", "").Split("-")(1)
        Dim Identificador As String = RIF_CI_Cliente.Replace("_", "").Replace(" ", "").Split("-")(2)

        If EsRIF(RIF_CI_Cliente) Then
            sCIF = TipoRazon + "-" + Documento + "-" + Identificador
        Else
            sCIF = TipoRazon + "-" + Documento
        End If

        If CodigoCliente = "00000000" AndAlso ft.DevuelveScalarEntero(MyConn, " select count(*) from jsvencatclipv " _
                                                                      & " WHERE RIF = '" & sCIF & "' and " _
                                                                      & " ID_EMP = '" & jytsistema.WorkID & "' ") = 0 Then

            InsertarModificarPOSClientePV(MyConn, True, "00000000", NombreCliente, "", "", sCIF, "", _
                    "", "", DireccionCliente, TelefonoCliente, "", jytsistema.sFechadeTrabajo, 1)

        End If

    End Sub

    Public Function FacturaValida(MyConn As MySqlConnection, RIF_CI_Cliente As String, NombreCliente As String, _
                                  TelefonoCliente As String, num_items_Factura As String) As Boolean

        If Trim(RIF_CI_Cliente) = "" Then
            ft.mensajeCritico(" CI o RIF no válido. Verifique por favor...")
            Return False
        Else
            If Not IIf(EsRIF(RIF_CI_Cliente.Trim), validarRif(RIF_CI_Cliente.Trim), validarCI(RIF_CI_Cliente.Trim.Split("-")(0) + "-" + RIF_CI_Cliente.Trim.Split("-")(1).Trim)) Then
                ft.mensajeCritico(" CI o RIF no válido. Debe indicarlo de la forma V-11111111 ...")
                Return False
            End If
        End If
        '
        If Trim(NombreCliente) = "" Then
            ft.mensajeCritico(" DEBE INDICAR UN NOMBRE DE CLIENTE VALIDO ...")
            Return False
        End If

        If ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM04") = 1 AndAlso TelefonoCliente = "" Then
            ft.mensajeAdvertencia(" DEBE INDICAR UN NUMERO DE TELEFONO VALIDO ...")
            Return False
        End If

        If ft.valorNumero(num_items_Factura) = 0 Then
            ft.mensajeAdvertencia(" DEBE INCLUIR POR LO MENOS UN ITEM ...")
            Return False
        End If

        Return True

    End Function




    Public Sub ActualizarMovimientosInventarioCADIP(ByVal MyConn As MySqlConnection, ByVal NumeroFactura As String, _
                                                      RIF_CI As String, _
                                                      NumeroSerialFiscal As String, FechaFactura As Date)
        Dim dsCadip As New DataSet
        Dim dtCadip As New DataTable

        Dim TipoRazon As String = RIF_CI.Replace("_", "").Replace(" ", "").Split("-")(0)
        Dim Documento As String = RIF_CI.Replace("_", "").Replace(" ", "").Split("-")(1)
        Dim Identificador As String = RIF_CI.Replace("_", "").Replace(" ", "").Split("-")(2)
        If Identificador = "" Then Identificador = "0"

        Dim numeritoFactura As Long = ValorEnteroLargo(NumeroFactura)

        ft.Ejecutar_strSQL(MyConn, " delete from jsvenCADIPpos " _
                       & " where " _
                       & " FACTURA = '" & numeritoFactura & "' AND " _
                       & " TIPORAZON = '" & TipoRazon & "' AND " _
                       & " DOCUMENTO = '" & Documento & "' AND " _
                       & " IDENTIFICADOR = '" & Identificador & "' AND " _
                       & " id_emp = '" & jytsistema.WorkID & "' ")

        dtCadip = ft.AbrirDataTable(dsCadip, "tblRenglonesCadip", MyConn, "select * from jsvenrenpos " _
            & " where " _
            & " numfac  = '" & NumeroFactura & "' and " _
            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " id_emp = '" & jytsistema.WorkID & "' order by RENGLON ")

        If dtCadip.Rows.Count > 0 Then

            For Each nRow As DataRow In dtCadip.Rows
                With nRow

                    If Mid(.Item("ITEM"), 1, 1) <> "$" Then
                        Dim MercanciaRegulada As Boolean = ft.DevuelveScalarBooleano(MyConn, " select REGULADO from jsmerctainv where codart = '" & .Item("item") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        If MercanciaRegulada Then
                            Dim CodigoProducto As String = ft.DevuelveScalarCadena(MyConn, " select CODJER from jsmerctainv where codart = '" & .Item("item") & "' and id_emp = '" & jytsistema.WorkID & "' ").ToString.Replace(".", "")
                            If CodigoProducto.Trim() = "" Then CodigoProducto = "0"
                            If CInt(CodigoProducto) <> 0 Then
                                InsertarModificarCADIP(MyConn, True, TipoRazon, Documento, Identificador, _
                                    .Item("cantidad"), CodigoProducto, FechaFactura, ft.FormatoHora(Now()), _
                                    numeritoFactura, 0, jytsistema.sFechadeTrabajo)
                            End If
                        End If

                    End If

                End With

            Next

        End If

        dsCadip.Dispose()
        dtCadip.Dispose()

        dsCadip = Nothing
        dtCadip = Nothing

    End Sub

    Public Sub ActualizarMovimientosInventario(ByVal MyConn As MySqlConnection, ByVal NumeroFactura As String, _
                                                ByVal NumeroSerialFiscal As String, _
                                                ByVal Factura_Devolucion As Boolean)

        Dim lblInfo As New Label
        Dim ds As New DataSet
        Dim dtRenglones As New DataTable
        Dim nTablaRenglones As String = "tblRenglones"
        Dim AlmacenSalida As String = ft.DevuelveScalarCadena(MyConn, "select ALMACEN from jsvenencpos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "'")
        Dim CodigoCliente As String = ft.DevuelveScalarCadena(MyConn, "select CODCLI from jsvenencpos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "'")
        Dim CodigoVendedor As String = ft.DevuelveScalarCadena(MyConn, "select CODVEN from jsvenencpos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "'")
        Dim Descuentos As Double = ft.DevuelveScalarDoble(MyConn, "select DESCUEN from jsvenencpos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "'")


        CalculaDescuentosEnRenglones(MyConn, NumeroFactura, NumeroSerialFiscal, Descuentos)

        ft.Ejecutar_strSQL(MyConn, " delete from jsmertramer where " _
                           & " numorg = '" & NumeroFactura & "' and " _
                           & " origen = 'PVE' and " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")

        dtRenglones = ft.AbrirDataTable(ds, nTablaRenglones, MyConn, "select * from jsvenrenpos " _
            & " where " _
            & " numfac  = '" & NumeroFactura & "' and " _
            & " id_emp = '" & jytsistema.WorkID & "' order by RENGLON ")

        If dtRenglones.Rows.Count > 0 Then
            Dim rCont As Integer
            For rCont = 0 To dtRenglones.Rows.Count - 1
                With dtRenglones.Rows(rCont)
                    If Mid(.Item("ITEM"), 1, 1) <> "$" Then
                        If Factura_Devolucion Then ft.Ejecutar_strSQL(MyConn, " update jsmerctainv SET " _
                            & " fecultventa = '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', ultimocliente = '" & CodigoCliente & "', montoultimaventa = " & .Item("precio") & "  " _
                            & " where " _
                            & " codart = '" & .Item("item") & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

                        Dim nEquivale As Double = Equivalencia(MyConn, .Item("item"), .Item("unidad"))
                        Dim nCosto As Double = UltimoCostoAFecha(MyConn, .Item("item"), jytsistema.sFechadeTrabajo)
                        Dim Costototal As Double = nCosto * .Item("cantidad") / IIf(nEquivale = 0, 1, nEquivale)

                        InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, .Item("item"), jytsistema.sFechadeTrabajo, IIf(Factura_Devolucion, "SA", "EN"),
                            NumeroFactura, .Item("unidad"), .Item("cantidad"), .Item("peso"), Costototal, Costototal, "PVE", NumeroFactura,
                            IIf(IsDBNull(.Item("lote")), "", .Item("LOTE")), CodigoCliente, .Item("totren"), .Item("totrendes"), 0.0, .Item("totren") - .Item("totrendes"),
                            CodigoVendedor, AlmacenSalida, .Item("renglon") + .Item("estatus"), jytsistema.sFechadeTrabajo)

                    End If
                End With

            Next
        End If

    End Sub

    Private Sub CalculaDescuentosEnRenglones(ByVal MyConn As MySqlConnection, ByVal NumeroFactura As String, ByVal NumeroSerialFiscal As String, ByVal TotalDescuento As Double)

        Dim TotalMercanciasConDescuento As Double = ft.DevuelveScalarDoble(MyConn, " select sum(totren) from " _
                                                & " jsvenrenpos " _
                                                & " where " _
                                                & " numfac = '" & NumeroFactura & "' and " _
                                                & " tipo = 0 and " _
                                                & " estatus = 0 and " _
                                                & " id_emp = '" & jytsistema.WorkID & "' ")

        ft.Ejecutar_strSQL(MyConn, " update jsvenrenpos set totrendes = totren - if( " & TotalMercanciasConDescuento * TotalDescuento & " <= 0, 0, round(totren/" & TotalMercanciasConDescuento * TotalDescuento & ",2) ) " _
                    & " where " _
                    & " numfac = '" & NumeroFactura & "' and " _
                    & " tipo = 0 and " _
                    & " estatus = 0 and " _
                    & " id_emp = '" & jytsistema.WorkID & "' ")

    End Sub

    Public Function getCodigoMercancia(MyConn As MySqlConnection, CodigoBarras As String) As String

        Dim aTarifaPrecios() As String = {"A", "B", "C", "D", "E", "F"}
        Dim codMER As String = ""
        For Each item As String In aTarifaPrecios
            If codMER.Equals("") Then _
                codMER = ft.DevuelveScalarCadena(MyConn, " select CODART " _
                               & " FROM jsmerctainv " _
                               & " where " _
                               & " BARRA_" & item & " = '" & CodigoBarras & "' AND " _
                               & " ID_EMP = '" & jytsistema.WorkID & "' ")
        Next

        If codMER.Equals("") Then
            codMER = ft.DevuelveScalarCadena(MyConn, " select CODART " _
                                            & " FROM jsmerctainv " _
                                            & " where " _
                                            & " BARRAS = '" & CodigoBarras & "' AND " _
                                            & " ID_EMP = '" & jytsistema.WorkID & "' ")
        End If
        If codMER.Equals("") Then
            codMER = ft.DevuelveScalarCadena(MyConn, " select CODART " _
                           & " FROM jsmerctainv " _
                           & " where " _
                           & " ALTERNO = '" & CodigoBarras & "' AND " _
                           & " ID_EMP = '" & jytsistema.WorkID & "' ")
        End If

        Return codMER

    End Function
    Public Function getTarifaPrecioMercancia(MyConn As MySqlConnection, myPerfil As Perfil, CodigoBarraOriginal As String) As String

        Dim TarifaPrecios As String = ""

        Dim aTarifaPrecios() As String = {"A", "B", "C", "D", "E", "F"}
        For Each item As String In aTarifaPrecios
            If ft.DevuelveScalarEntero(MyConn, " select count(*) " _
                               & " FROM jsmerctainv " _
                               & " where " _
                               & " BARRA_" & item & " = '" & CodigoBarraOriginal & "' AND " _
                               & " ID_EMP = '" & jytsistema.WorkID & "' ") > 0 Then
                TarifaPrecios = item
            End If
        Next

        If myPerfil.TarifaA And TarifaPrecios = "A" Then
            TarifaPrecios = "A"
        ElseIf myPerfil.TarifaB And TarifaPrecios = "B" Then
            TarifaPrecios = "B"
        ElseIf myPerfil.TarifaC And TarifaPrecios = "C" Then
            TarifaPrecios = "C"
        ElseIf myPerfil.TarifaD And TarifaPrecios = "D" Then
            TarifaPrecios = "D"
        ElseIf myPerfil.TarifaE And TarifaPrecios = "E" Then
            TarifaPrecios = "E"
        ElseIf myPerfil.TarifaF And TarifaPrecios = "F" Then
            TarifaPrecios = "F"
        End If

        If TarifaPrecios = "" Then
            TarifaPrecios = ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM11")
            If myPerfil.TarifaA And TarifaPrecios = "A" Then
                TarifaPrecios = "A"
            ElseIf myPerfil.TarifaB And TarifaPrecios = "B" Then
                TarifaPrecios = "B"
            ElseIf myPerfil.TarifaC And TarifaPrecios = "C" Then
                TarifaPrecios = "C"
            ElseIf myPerfil.TarifaD And TarifaPrecios = "D" Then
                TarifaPrecios = "D"
            ElseIf myPerfil.TarifaE And TarifaPrecios = "E" Then
                TarifaPrecios = "E"
            ElseIf myPerfil.TarifaF And TarifaPrecios = "F" Then
                TarifaPrecios = "F"
            End If
        End If


        Return TarifaPrecios

    End Function

End Module
