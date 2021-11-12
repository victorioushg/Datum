Imports MySql.Data.MySqlClient
Module TablasPuntoDeVenta
    Private ft As New Transportables

    Public Sub ActualizarRenglonPV(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroFactura As String, _
                                    ByVal NumeroSerialFiscal As String, ByVal Estatus As String, ByVal TipoDocumento As Integer, _
                                    ByVal Renglon As String, ByVal Item As String, ByVal Barras As String, ByVal Descripcion As String, ByVal TipoIVA As String, _
                                    ByVal Unidad As String, ByVal Cantidad As Double, ByVal Precio As Double, ByVal Peso As Double, ByVal Lote As String, _
                                    ByVal DescuentoCliente As Double, ByVal DescuentoArticulo As Double, ByVal DescuentoOferta As Double, _
                                    ByVal TotalRenglon As Double, ByVal TotalRenglonDescuento As Double, ByVal CodigoContable As String, ByVal Editable As Integer)
       
        Dim Talla As String = ""
        Dim Color As String = ""

        Dim dsP As New DataSet
        Dim dt As DataTable
        Dim nTabla As String = "tblRenPOS"

        dsP = DataSetRequery(dsP, " select * from jsvenrenpos where " _
                                & " NUMFAC = '" & NumeroFactura & "' AND " _
                                & " ITEM = '" & Item & "' AND " _
                                & " ID_EMP = '" & jytsistema.WorkID & "' ", MyConn, nTabla, lblInfo)

        '& " tipo = " & TipoDocumento & " and " _
        '                      & " ITEM = '" & Item & "' AND " _
        '                      & " BARRAS = '" & Barras & "' AND " _
        '                      & " ESTATUS = '" & Estatus & "' AND " _
        '                      & " ACEPTADO = '1' AND " _
        '                      & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
        '                      & " ID_EMP = '" & jytsistema.WorkID & "' ", MyConn, nTabla, lblInfo)

        dt = dsP.Tables(nTabla)
        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                If .Item("UNIDAD") = Unidad Then

                    InsertarModificarPOSRenglonPuntoDeVenta(MyConn, lblInfo, False, NumeroFactura, NumeroSerialFiscal, TipoDocumento, .Item("renglon"), Item, Barras, _
                                Descripcion, TipoIVA, Unidad, Cantidad + .Item("CANTIDAD"), Precio, Peso + .Item("PESO"), _
                                Lote, Estatus, DescuentoCliente, DescuentoArticulo, DescuentoOferta, TotalRenglon + .Item("TOTREN"), _
                                TotalRenglonDescuento + .Item("TOTRENDES"), CodigoContable, "1", "0")

                    If ExisteTabla(MyConn, jytsistema.WorkDataBase, "jsmermovcatcol") Then
                        Dim aFldMov() As String = {"codart", "nummov", "codtal", "codcol", "tipomov", "origen", "id_emp"}
                        Dim aStrMov() As String = {Item, NumeroFactura, Talla, Color, IIf(TipoDocumento = 0, "SA", "EN"), "PVE", jytsistema.WorkID}

                        If qFound(MyConn, lblInfo, "jsmermovcatcol", aFldMov, aStrMov) Then
                            Dim dtCan As DataTable
                            Dim CantidadAnterior As Double = 0.0
                            Dim nTablaCan As String = "tblcancodtal"
                            dsP = DataSetRequery(dsP, " select IFNULL(SUM(CANTIDAD),0) cantidad " _
                                                    & " from jsmermovcatcol where codart = '" & Item & "' and " _
                                                    & " nummov = '" & NumeroFactura & "' and " _
                                                    & " codtal = '" & Talla & "' and " _
                                                    & " codcol = '" & Color & "' and " _
                                                    & " tipomov = '" & IIf(TipoDocumento = 0, "SA", "EN") & "' and " _
                                                    & " origen = 'PVE' and " _
                                                    & " id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaCan, lblInfo)
                            dtCan = dsP.Tables(nTablaCan)
                            If dtCan.Rows.Count > 0 Then CantidadAnterior = IIf(IsDBNull(dtCan.Rows(0).Item("cantidad")), 0.0#, dtCan.Rows(0).Item("cantidad"))
                            If Talla <> "" Then _
                                IngresarMovimientoTallaColor(MyConn, lblInfo, False, Item, NumeroFactura, Talla, Color, IIf(TipoDocumento = 0, "SA", "EN"), "PVE", 0, Cantidad + CantidadAnterior)
                            dtCan = Nothing

                        Else
                            If Talla <> "" Then _
                                IngresarMovimientoTallaColor(MyConn, lblInfo, True, Item, NumeroFactura, Talla, Color, IIf(TipoDocumento = 0, "SA", "EN"), "PVE", 0, Cantidad)
                        End If
                    End If

                Else
                    InsertarModificarPOSRenglonPuntoDeVenta(MyConn, lblInfo, True, NumeroFactura, NumeroSerialFiscal, TipoDocumento, Renglon, Item, Barras, _
                        Descripcion, TipoIVA, Unidad, Cantidad, Precio, Peso, Lote, Estatus, DescuentoCliente, DescuentoArticulo, DescuentoOferta, TotalRenglon, _
                        TotalRenglonDescuento, CodigoContable, "1", Editable)

                    If Talla <> "" Then _
                        IngresarMovimientoTallaColor(MyConn, lblInfo, True, Item, NumeroFactura, Talla, Color, IIf(TipoDocumento = 0, "SA", "EN"), "PVE", 0, Cantidad)

                End If
            End With
        Else
            InsertarModificarPOSRenglonPuntoDeVenta(MyConn, lblInfo, True, NumeroFactura, NumeroSerialFiscal, TipoDocumento, Renglon, Item, Barras, _
                Descripcion, TipoIVA, Unidad, Cantidad, Precio, Peso, Lote, Estatus, DescuentoCliente, DescuentoArticulo, DescuentoOferta, TotalRenglon, _
                TotalRenglonDescuento, CodigoContable, "1", Editable)
            If Talla <> "" Then _
                IngresarMovimientoTallaColor(MyConn, lblInfo, True, Item, NumeroFactura, Talla, Color, IIf(TipoDocumento = 0, "SA", "EN"), "PVE", 0, Cantidad)

        End If

        dt = Nothing
        dsP = Nothing


    End Sub

    Public Sub InsertarModificarPOSRenglonPuntoDeVenta(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                                    ByVal NumeroFactura As String, ByVal NumeroSerialFiscal As String, ByVal TipoDocumento As Integer, ByVal NumRenglon As String, ByVal Item As String, _
                                                    ByVal CodBarras As String, ByVal Descripcion As String, ByVal IVA As String, _
                                                    ByVal Unidad As String, ByVal Cantidad As Double, ByVal Precio As Double, ByVal Peso As Double, _
                                                    ByVal Lote As String, ByVal Estatus As String, ByVal DescuentoCliente As Double, _
                                                    ByVal DescuentoArticulo As Double, ByVal DescuentoOferta As Double, ByVal TotalRenglon As Double, _
                                                    ByVal TotalRenglonDescuento As Double, ByVal CodigoContable As String, ByVal Aceptado As String, _
                                                    ByVal Editable As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsvenrenpos SET "
            strSQL = ""
            strSQLFin = " "

        Else
            strSQLInicio = " UPDATE jsvenrenpos SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " NUMFAC = '" & NumeroFactura & "' AND " _
                & " RENGLON = '" & NumRenglon & "' AND " _
                & " ITEM = '" & Item & "' AND " _
                & " ID_EMP = '" & jytsistema.WorkID & "' "

            '& " NUMFAC = '" & NumeroFactura & "' AND " _
            '               & " numserial = '" & NumeroSerialFiscal & "' and  " _
            '               & " tipo = " & TipoDocumento & " and " _
            '               & " RENGLON = '" & NumRenglon & "' AND " _
            '               & " ITEM = '" & Item & "' AND " _
            '               & " BARRAS = '" & CodBarras & "' AND " _
            '               & " ESTATUS = '" & Estatus & "' AND " _
            '               & " ACEPTADO = '" & Aceptado & "' AND " _
            '               & " ID_EMP = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroFactura, "numfac")
        strSQL += ModificarCadena(NumeroSerialFiscal, "numserial")
        strSQL += ModificarEntero(TipoDocumento, "tipo")
        strSQL += ModificarCadena(NumRenglon, "renglon")
        strSQL += ModificarCadena(Item, "item")
        strSQL += ModificarCadena(CodBarras, "barras")
        strSQL += ModificarCadena(Descripcion, "descrip")
        strSQL += ModificarCadena(IVA, "iva")
        strSQL += ModificarCadena(Unidad, "unidad")
        strSQL += ModificarDoble(Cantidad, "cantidad")
        strSQL += ModificarDoble(Precio, "precio")
        strSQL += ModificarDoble(Peso, "peso")
        strSQL += ModificarCadena(Lote, "lote")
        strSQL += ModificarCadena(Estatus, "estatus")
        strSQL += ModificarDoble(DescuentoCliente, "des_cli")
        strSQL += ModificarDoble(DescuentoArticulo, "des_art")
        strSQL += ModificarDoble(DescuentoOferta, "des_ofe")
        strSQL += ModificarDoble(TotalRenglon, "totren")
        strSQL += ModificarDoble(TotalRenglonDescuento, "totrendes")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarCadena(Aceptado, "aceptado")
        strSQL += ModificarCadena(Editable, "editable")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertarModificarPOSEncabezadoPuntoDeVenta(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                                    ByVal NumeroFactura As String, ByVal NumeroSerialFiscal As String, ByVal TipoDocumento As Integer, ByVal Emision As Date,
                                                    ByVal CodigoCliente As String, ByVal NombreCliente As String, ByVal RIF As String, ByVal NIT As String, ByVal Comentario As String,
                                                    ByVal Almacen As String, ByVal Vencimiento As Date, ByVal Referencia As String,
                                                    ByVal NumeroControlFiscal As String, ByVal VendedorDePiso As String, ByVal Items As Integer,
                                                    ByVal Cajas As Double, ByVal Kilos As Double, ByVal TotalNeto As Double, ByVal PorcentajeDescuento As Double, ByVal Descuento As Double,
                                                    ByVal Cargos As Double, ByVal ImporteIVA As Double, ByVal TotalFactura As Double, ByVal CondicionPago As Integer, ByVal TipoCredito As String,
                                                    ByVal ImporteEfectivo As Double, ByVal NumeroCheque As String, ByVal BancoCheque As String, ByVal ImporteCheque As Double,
                                                    ByVal NumeroTarjeta As String, ByVal CodigoTarjeta As String, ByVal ImporteTarjeta As Double, ByVal NumeroCestaTicket As String,
                                                    ByVal NombreCestaTicket As String, ByVal ImporteCestaTicket As Double, ByVal NumeroDeposito As String, ByVal BancoDeposito As String,
                                                    ByVal ImporteDeposito As Double, ByVal Asiento As String, ByVal FechaAsiento As Date, ByVal Estatus As Integer, ByVal Tarifa As String,
                                                    ByVal CodigoCaja As String, ByVal CodigoVendedor As String, ByVal Impresa As Integer,
                                                          ByVal Currency As Integer, ByVal CurrencyDate As DateTime)
        Dim strSQL As String
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsvenencpos SET "
            strSQL += ModificarFechaTiempoPlus(CurrencyDate, "currency_date")
            strSQL += ModificarEntero(Currency, "currency")


        Else
            strSQLInicio = " UPDATE jsvenencpos SET "
            strSQL = ""
            strSQLFin = " where " _
                & " NUMFAC = '" & NumeroFactura & "' AND " _
                & " NUMSERIAL = '" & NumeroSerialFiscal & "' AND " _
                & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                & " ID_EMP = '" & jytsistema.WorkID & "'"
        End If

        strSQL += ModificarCadena(NumeroFactura, "numfac")
        strSQL += ModificarCadena(NumeroSerialFiscal, "numSERIAL")
        strSQL += ModificarFecha(Emision, "EMISION")
        strSQL += ModificarCadena(CodigoCliente, "CODCLI")
        strSQL += ModificarCadena(NombreCliente, "NOMCLI")
        strSQL += ModificarCadena(RIF, "RIF")
        strSQL += ModificarCadena(NIT, "NIT")
        strSQL += ModificarCadena(Comentario, "COMEN")
        strSQL += ModificarCadena(Almacen, "ALMACEN")
        strSQL += ModificarFecha(Vencimiento, "VENCE")
        strSQL += ModificarCadena(Referencia, "REFER")
        strSQL += ModificarCadena(VendedorDePiso, "CODCON")
        strSQL += ModificarEnteroLargo(Items, "ITEMS")
        strSQL += ModificarDoble(Cajas, "CAJAS")
        strSQL += ModificarDoble(Kilos, "KILOS")
        strSQL += ModificarDoble(TotalNeto, "TOT_NET")
        strSQL += ModificarDoble(PorcentajeDescuento, "PORDES")
        strSQL += ModificarDoble(Descuento, "DESCUEN")
        strSQL += ModificarDoble(Cargos, "CARGOS")
        strSQL += ModificarDoble(ImporteIVA, "IMP_IVA")
        strSQL += ModificarDoble(TotalFactura, "TOT_FAC")
        strSQL += ModificarEntero(CondicionPago, "CONDPAG")
        strSQL += ModificarCadena(TipoCredito, "TIPOCRE")
        strSQL += ModificarDoble(ImporteEfectivo, "IMPORTEFECTIVO")
        strSQL += ModificarCadena(NumeroCheque, "NUMEROCHEQUE")
        strSQL += ModificarCadena(BancoCheque, "BANCOCHEQUE")
        strSQL += ModificarDoble(ImporteCheque, "IMPORTECHEQUE")
        strSQL += ModificarCadena(NumeroTarjeta, "NUMEROTARJETA")
        strSQL += ModificarCadena(CodigoTarjeta, "CODIGOTARJETA")
        strSQL += ModificarDoble(ImporteTarjeta, "IMPORTETARJETA")
        strSQL += ModificarCadena(NumeroCestaTicket, "NUMEROCESTATICKET")
        strSQL += ModificarCadena(NombreCestaTicket, "NOMBRECESTATICKET")
        strSQL += ModificarDoble(ImporteCestaTicket, "IMPORTECESTATICKET")
        strSQL += ModificarCadena(NumeroDeposito, "NUMERODEPOSITO")
        strSQL += ModificarCadena(BancoDeposito, "BANCODEPOSITO")
        strSQL += ModificarDoble(ImporteDeposito, "IMPORTEDEPOSITO")
        strSQL += ModificarCadena(Asiento, "ASIENTO")
        strSQL += ModificarFecha(FechaAsiento, "FECHASI")
        strSQL += ModificarEntero(Estatus, "ESTATUS")
        strSQL += ModificarCadena(Tarifa, "TARIFA")
        strSQL += ModificarCadena(CodigoCaja, "CODCAJ")
        strSQL += ModificarCadena(CodigoVendedor, "CODVEN")
        strSQL += ModificarEntero(Impresa, "IMPRESA")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub


    Public Sub IngresarMovimientoTallaColor(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
    ByVal CodigoArticulo As String, ByVal Documento As String, ByVal CodigoTalla As String, ByVal CodigoColor As String, _
    ByVal TipoMovimiento As String, ByVal Origen As String, ByVal Estatus As Integer, _
    ByVal Cantidad As Double)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsmermovcatcol SET "
            strSQL = ""
            strSQLFin = " "

        Else
            strSQLInicio = " UPDATE jsmermovcatcol SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " codart = '" & CodigoArticulo & "' and " _
                & " nummov = '" & Documento & "' and " _
                & " codtal = '" & CodigoTalla & "' and " _
                & " codcol = '" & CodigoColor & "' and " _
                & " tipomov = '" & TipoMovimiento & "' and " _
                & " origen = '" & Origen & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoArticulo, "codart")
        strSQL += ModificarCadena(Documento, "nummov")
        strSQL += ModificarCadena(CodigoTalla, "codtal")
        strSQL += ModificarCadena(CodigoColor, "codcol")
        strSQL += ModificarCadena(TipoMovimiento, "tipomov")
        strSQL += ModificarCadena(Origen, "origen")
        strSQL += ModificarEntero(Estatus, "estatus")
        strSQL += ModificarDoble(Cantidad, "cantidad")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditVentasFormaPago(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
    ByVal NumeroFactura As String, ByVal NumeroSerialFiscal As String, ByVal Origen As String, ByVal FormaPago As String, ByVal NumeroPago As String,
    ByVal NombrePago As String, ByVal Importe As Double, ByVal Vencimiento As Date, ByVal Currency As Integer, ByVal CurrencyDate As DateTime)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsvenforpag SET "
            strSQL = ""
            strSQLFin = " "
            strSQL += ModificarFechaTiempoPlus(CurrencyDate, "currency_date")
            strSQL += ModificarEntero(Currency, "currency")

        Else
            strSQLInicio = " UPDATE jsvenforpag SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " numfac = '" & NumeroFactura & "' and " _
                & " origen = '" & Origen & "' and " _
                & " formapag = '" & FormaPago & "' and " _
                & " numpag = '" & NumeroPago & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroFactura, "numfac")
        strSQL += ModificarCadena(NumeroSerialFiscal, "numserial")
        strSQL += ModificarCadena(Origen, "origen")
        strSQL += ModificarCadena(FormaPago, "formapag")
        strSQL += ModificarCadena(NumeroPago, "numpag")
        strSQL += ModificarCadena(NombrePago, "nompag")
        strSQL += ModificarDoble(Importe, "importe")
        strSQL += ModificarFecha(Vencimiento, "vence")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")
        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertarModificarPOSClientePV(ByVal MyConn As MySqlConnection, ByVal Insertar As Boolean, _
        ByVal CodigoCliente As String, ByVal NombreCliente As String, ByVal Categoria As String, _
        ByVal Unidad As String, ByVal RIF As String, ByVal NIT As String, ByVal CI As String, _
        ByVal Alterno As String, ByVal DireccionFiscal As String, ByVal Telefono As String, ByVal Fax As String, _
        ByVal Ingreso As Date, ByVal Estatus As Integer)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsvencatclipv SET "
            strSQL = ""
            strSQLFin = " "

        Else
            strSQLInicio = " UPDATE jsvencatclipv SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " rif = '" & RIF & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoCliente, "codcli")
        strSQL += ModificarCadena(NombreCliente, "nombre")
        strSQL += ModificarCadena(Categoria, "categoria")
        strSQL += ModificarCadena(Unidad, "unidad")
        strSQL += ModificarCadena(RIF, "rif")
        strSQL += ModificarCadena(NIT, "nit")
        strSQL += ModificarCadena(CI, "ci")
        strSQL += ModificarCadena(Alterno, "alterno")
        strSQL += ModificarCadena(DireccionFiscal, "dirfiscal")
        strSQL += ModificarCadena(Telefono, "telef1")
        strSQL += ModificarCadena(Fax, "fax")
        strSQL += ModificarFecha(Ingreso, "ingreso")
        strSQL += ModificarEntero(Estatus, "estatus")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertarModificarPOSCaja(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal CodigoCaja As String, ByVal NombreCaja As String, ByVal Almacen As String, ByVal POS_FAC As Integer, _
        ByVal ImpreFiscal As String)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsvencatcaj SET "
            strSQL = ""
            strSQLFin = " "

        Else
            strSQLInicio = " UPDATE jsvencatcaj SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " codcaj = '" & CodigoCaja & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoCaja, "codcaj")
        strSQL += ModificarCadena(NombreCaja, "descrip")
        strSQL += ModificarCadena(Almacen, "almacen")
        strSQL += ModificarCadena(POS_FAC, "pos_fac")
        strSQL += ModificarCadena(ImpreFiscal, "impre_fiscal")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertarInicioCierrePV(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal FechaCaja As Date, ByVal CodigoVendedor As String, _
            ByVal CodigoSupervisor As String, ByVal MontoInicio As Double, ByVal HoraInicio As String, _
            ByVal HoraCierre As String, ByVal CodigoCaja As String, ByVal Estatus As Integer)

        Dim aFld() As String = {"fecha", "codcaj", "codven", "ejercicio", "id_emp"}
        Dim aStr() As String = {ft.FormatoFechaMySQL(FechaCaja), CodigoCaja, CodigoVendedor, jytsistema.WorkExercise, jytsistema.WorkID}

        If qFound(MyConn, lblInfo, "jsveninipos", aFld, aStr) Then
        Else
            ft.Ejecutar_strSQL(myconn, " insert into jsveninipos values (" _
                & "'" & ft.FormatoFechaMySQL(FechaCaja) & "', " _
                & "'" & CodigoVendedor & "', " _
                & "'" & CodigoSupervisor & "', " _
                & MontoInicio & ", " _
                & "'" & HoraInicio & "', " _
                & "'" & HoraCierre & "', " _
                & "'" & CodigoCaja & "', " _
                & Estatus & ", " _
                & "'" & jytsistema.WorkExercise & "', " _
                & "'" & jytsistema.WorkID & "' " _
                & ")")

        End If

    End Sub

    Public Sub InsertarModificarPOSSupervisor(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
       ByVal CodigoSupervisor As String, ByVal DescripSupervisor As String, ByVal NombreSupervisor As String, _
       ByVal Clave As String, ByVal Nivel As Integer)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsvencatsup SET "
            strSQL = ""
            strSQLFin = " "

        Else
            strSQLInicio = " UPDATE jsvencatsup SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " codigo = '" & CodigoSupervisor & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoSupervisor, "codigo")
        strSQL += ModificarCadena(DescripSupervisor, "descrip")
        strSQL += ModificarCadena(NombreSupervisor, "nombre")
        strSQL += ModificarEnteroLargo(Nivel, "nivel")
        strSQL += ModificarCadena(Clave, "clave")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertarModificarPOSWork(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                            ByVal CodigoCaja As String, ByVal FechaEmision As Date, ByVal Origen As String, ByVal TipoMovimiento As String,
                            ByVal NumeroFactura As String, ByVal NumeroSerialFiscal As String, ByVal FormaPago As String, ByVal NumeroPago As String,
                            ByVal NombrePago As String, ByVal Importe As Double, ByVal FechaVencimiento As Date,
                                        ByVal Cantidad As Integer, ByVal CodigoCajero As String,
                                        ByVal Currency As Integer, ByVal CurrencyDate As DateTime)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsventrapos SET "
            strSQL = ""
            strSQLFin = " "
            strSQL += ModificarFechaTiempoPlus(CurrencyDate, "currency_date")
            strSQL += ModificarEntero(Currency, "currency")

        Else
            strSQLInicio = " UPDATE jsventrapos SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " caja = '" & CodigoCaja & "' and " _
                & " fecha = '" & ft.FormatoFechaMySQL(FechaEmision) & "' and " _
                & " origen = '" & Origen & "' and " _
                & " tipomov = '" & TipoMovimiento & "' and " _
                & " nummov = '" & NumeroFactura & "' and " _
                & " formapag = '" & FormaPago & "' and " _
                & " numpag = '" & NumeroPago & "' and " _
                & " nompag = '" & NombrePago & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoCaja, "caja")
        strSQL += ModificarFecha(FechaEmision, "fecha")
        strSQL += ModificarCadena(Origen, "origen")
        strSQL += ModificarCadena(TipoMovimiento, "tipomov")
        strSQL += ModificarCadena(NumeroFactura & NumeroSerialFiscal, "nummov")
        strSQL += ModificarCadena(FormaPago, "formpag")
        strSQL += ModificarCadena(NumeroPago, "numpag")
        strSQL += ModificarCadena(NombrePago, "nompag")
        strSQL += ModificarDoble(Importe, "importe")
        strSQL += ModificarFecha(FechaVencimiento, "fechacierre")
        strSQL += ModificarEnteroLargo(Cantidad, "cantidad")
        strSQL += ModificarCadena(CodigoCajero, "cajero")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertarModificarPOSPERFIL(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                           ByVal CodigoPerfil As String, ByVal NombrePerfil As String, _
                           ByVal Credito As Integer, ByVal Contado As Integer, _
                           ByVal TarifaA As Integer, ByVal TarifaB As Integer, ByVal TarifaC As Integer, _
                           ByVal TarifaD As Integer, ByVal TarifaE As Integer, ByVal TarifaF As Integer, _
                           ByVal Almacen As String, ByVal Descuento As Integer)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsvenperven SET "
            strSQL = ""
            strSQLFin = " "

        Else
            strSQLInicio = " UPDATE jsvenperven SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " codper = '" & CodigoPerfil & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoPerfil, "codper")
        strSQL += ModificarCadena(NombrePerfil, "descrip")
        strSQL += ModificarEntero(Credito, "cr")
        strSQL += ModificarEntero(Contado, "co")
        strSQL += ModificarEntero(TarifaA, "tarifa_a")
        strSQL += ModificarEntero(TarifaB, "tarifa_b")
        strSQL += ModificarEntero(TarifaC, "tarifa_c")
        strSQL += ModificarEntero(TarifaD, "tarifa_d")
        strSQL += ModificarEntero(TarifaE, "tarifa_e")
        strSQL += ModificarEntero(TarifaF, "tarifa_f")
        strSQL += ModificarCadena(Almacen, "almacen")
        strSQL += ModificarEntero(Descuento, "descuento")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub


    Public Sub InsertarModificarCADIP(ByVal MyConn As MySqlConnection, ByVal Insertar As Boolean, _
                           ByVal TipoRazon As String, ByVal Documento As String, Identificador As String, _
                           ByVal Cantidad As Integer, ByVal Codigo As Integer, FechaMovimiento As Date, _
                           ByVal Hora As String, ByVal Factura As Integer, ByVal Procesado As Integer, _
                           ByVal FechaProceso As Date)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsvenCADIPpos SET "
            strSQL = ""
            strSQLFin = " "

        Else
            strSQLInicio = " UPDATE jsvenCADIPpos SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " tiporazon = '" & TipoRazon & "' and " _
                & " documento = '" & Documento & "' and " _
                & " identificador = '" & Identificador & "' and " _
                & " codigo = " & Codigo & " and " _
                & " factura = '" & Factura & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(TipoRazon, "tiporazon")
        strSQL += ModificarCadena(Documento, "documento")
        strSQL += ModificarCadena(Identificador, "identificador")
        strSQL += ModificarEntero(Cantidad, "cantidad")
        strSQL += ModificarEntero(Codigo, "codigo")
        strSQL += ModificarFecha(FechaMovimiento, "fechamov")
        strSQL += ModificarCadena(Hora, "hora")
        strSQL += ModificarCadena(Factura, "factura")
        strSQL += ModificarEntero(Procesado, "procesado")
        strSQL += ModificarFechaTiempoPlus(FechaProceso, "fechaproceso")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertarModificarCADIPXXX(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                           ByVal TipoRazon As String, ByVal TipoComercio As String, Documento As String, _
                           ByVal CantidadAComprar As Integer, ByVal CodigoProducto As Integer, CantidadComprada As Integer, _
                           ByVal FechaInicioCompra As Date, ByVal FechaUltimaCompra As Date, ByVal FechaProximaCompra As Date, _
                           ByVal Estatus As Integer)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsvenCADIPposX SET "
            strSQL = ""
            strSQLFin = " "

        Else
            strSQLInicio = " UPDATE jsvenCADIPposX SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " tiporazon = '" & TipoRazon & "' and " _
                & " documento = '" & Documento & "' and " _
                & " codigo = " & CodigoProducto & " and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(TipoRazon, "tiporazon")
        strSQL += ModificarCadena(TipoComercio, "tipocomercio")
        strSQL += ModificarCadena(Documento, "documento")
        strSQL += ModificarEntero(CodigoProducto, "codigo_producto")
        strSQL += ModificarEntero(CantidadAComprar, "cantidad_a_comprar")
        strSQL += ModificarEntero(CantidadComprada, "cantidad_comprada")
        strSQL += ModificarFecha(FechaInicioCompra, "fechainiciocompra")
        strSQL += ModificarFecha(FechaUltimaCompra, "fechaultimacompra")
        strSQL += ModificarFecha(FechaProximaCompra, "fechaproximacompra")
        strSQL += ModificarEntero(Estatus, "estatus")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

End Module
