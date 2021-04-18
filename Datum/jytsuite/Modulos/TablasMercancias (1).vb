Imports MySql.Data.MySqlClient
Module TablasMercancias
    Public Sub InsertEditMERCASDivision(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal Codigo As String, ByVal Nombre As String, _
      ByVal Color As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmercatdiv SET "
        Else
            strSQLInicio = " UPDATE jsmercatdiv SET "
            strSQLFin = " WHERE " _
                & " division = '" & Codigo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(Codigo, "division")
        strSQL += ModificarCadena(Nombre, "descrip")
        strSQL += ModificarCadena(Color, "color")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditMERCASComponenteCombo(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                               ByVal CodigoArticulo As String, ByVal CodigoComponente As String, _
                                               ByVal Nombre As String, ByVal Cantidad As Double, ByVal Unidad As String, _
                                               ByVal Costo As Double, ByVal Peso As Double)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmercatcom SET "
        Else
            strSQLInicio = " UPDATE jsmercatcom SET "
            strSQLFin = " WHERE " _
                & " codart = '" & CodigoArticulo & "' and " _
                & " codartcom = '" & CodigoComponente & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoArticulo, "codart")
        strSQL += ModificarCadena(CodigoComponente, "codartcom")
        strSQL += ModificarCadena(Nombre, "descrip")
        strSQL += ModificarDoble(Cantidad, "cantidad")
        strSQL += ModificarCadena(Unidad, "unidad")
        strSQL += ModificarDoble(Costo, "costo")
        strSQL += ModificarDoble(Peso, "peso")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditMERCASMercancia(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal Codigo As String, _
                                          ByVal Alterno As String, ByVal Barras As String, ByVal Nombre As String, _
                                          ByVal categoria As String, ByVal Marca As String, ByVal Division As String, ByVal TipoJerarquia As String, _
                                          ByVal codigoJerarquia1 As String, ByVal codigoJerarquia2 As String, ByVal codigoJerarquia3 As String, _
                                          ByVal codigoJerarquia4 As String, ByVal codigoJerarquia5 As String, ByVal codigoJerarquia6 As String, _
                                          ByVal Presentacion As String, ByVal PrecioSugerido As Double, ByVal Regulada As Integer, ByVal Devoluciones As Integer, _
                                          ByVal PorAceptacionDevoluciones As Double, ByVal Unidad As String, ByVal PesoUnidad As Double, ByVal DivideUnidadVenta As String, ByVal UnidadDetal As String, _
                                          ByVal ExistenciaMinima As Double, ByVal ExistenciaMaxima As Double, ByVal Ubicacion1 As String, _
                                          ByVal Ubicacion2 As String, ByVal Ubicacion3 As String, ByVal Altura As Double, ByVal Ancho As Double, _
                                          ByVal Profundidad As Double, ByVal TipoIVA As String, ByVal Cartera As Integer, ByVal CuotaFija As Integer, ByVal Descuentos As Integer, _
                                          ByVal PrecioA As Double, ByVal PrecioB As Double, ByVal PrecioC As Double, ByVal PrecioD As Double, _
                                          ByVal PrecioE As Double, ByVal PrecioF As Double, ByVal DescuentoA As Double, ByVal DescuentoB As Double, _
                                          ByVal DescuentoC As Double, ByVal DescuentoD As Double, ByVal DescuentoE As Double, ByVal DescuentoF As Double, _
                                          ByVal GananciaA As Double, ByVal GananciaB As Double, ByVal GananciaC As Double, ByVal GananciaD As Double, _
                                          ByVal GananciaE As Double, ByVal GananciaF As Double, ByVal BarraA As String, ByVal BarraB As String, ByVal BarraC As String, ByVal BarraD As String, ByVal BarraE As String, ByVal BarraF As String, _
                                          ByVal TipoArticulo As Integer, ByVal MIX As Integer, _
                                          ByVal Ingreso As Date, ByVal Estatus As Integer, _
                                          ByVal CodigoSICA As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmerctainv SET "
        Else
            strSQLInicio = " UPDATE jsmerctainv SET "
            strSQLFin = " WHERE " _
                & " codart = '" & Codigo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(Codigo, "codart")
        strSQL += ModificarCadena(Alterno, "alterno")
        strSQL += ModificarCadena(Barras, "barras")
        strSQL += ModificarCadena(Nombre, "nomart")
        strSQL += ModificarCadena(categoria, "grupo")
        strSQL += ModificarCadena(Marca, "marca")
        strSQL += ModificarCadena(Division, "division")
        strSQL += ModificarCadena(TipoJerarquia, "tipjer")
        strSQL += ModificarCadena(codigoJerarquia1, "codjer1")
        strSQL += ModificarCadena(codigoJerarquia2, "codjer2")
        strSQL += ModificarCadena(codigoJerarquia3, "codjer3")
        strSQL += ModificarCadena(codigoJerarquia4, "codjer4")
        strSQL += ModificarCadena(codigoJerarquia5, "codjer5")
        strSQL += ModificarCadena(codigoJerarquia6, "codjer6")
        strSQL += ModificarCadena(CodigoSICA, "codjer")
        strSQL += ModificarCadena(Presentacion, "presentacion")
        strSQL += ModificarDoble(PrecioSugerido, "sugerido")
        strSQL += ModificarEntero(Regulada, "regulado")
        strSQL += ModificarEntero(Devoluciones, "devolucion")
        strSQL += ModificarDoble(PorAceptacionDevoluciones, "por_acepta_dev")
        strSQL += ModificarCadena(Unidad, "unidad")
        strSQL += ModificarDoble(PesoUnidad, "pesounidad")
        strSQL += ModificarCadena(DivideUnidadVenta, "divideuv")
        strSQL += ModificarCadena(UnidadDetal, "unidaddetal")
        strSQL += ModificarCadena(ExistenciaMinima, "exmin")
        strSQL += ModificarCadena(ExistenciaMaxima, "exmax")
        strSQL += ModificarCadena(Ubicacion1 + "." + Ubicacion2 + "." + Ubicacion3, "ubicacion")
        strSQL += ModificarDoble(Altura, "altura")
        strSQL += ModificarDoble(Ancho, "ancho")
        strSQL += ModificarDoble(Profundidad, "profun")
        strSQL += ModificarCadena(TipoIVA, "IVA")
        strSQL += ModificarEntero(Cartera, "cuota")
        strSQL += ModificarEntero(Cartera, "cuotafija")
        strSQL += ModificarEntero(Descuentos, "descuento")
        strSQL += ModificarDoble(PrecioA, "precio_A")
        strSQL += ModificarDoble(PrecioB, "precio_B")
        strSQL += ModificarDoble(PrecioC, "precio_C")
        strSQL += ModificarDoble(PrecioD, "precio_D")
        strSQL += ModificarDoble(PrecioE, "precio_E")
        strSQL += ModificarDoble(PrecioF, "precio_F")
        strSQL += ModificarDoble(DescuentoA, "desc_A")
        strSQL += ModificarDoble(DescuentoB, "desc_B")
        strSQL += ModificarDoble(DescuentoC, "desc_C")
        strSQL += ModificarDoble(DescuentoD, "desc_D")
        strSQL += ModificarDoble(DescuentoE, "desc_E")
        strSQL += ModificarDoble(DescuentoF, "desc_F")
        strSQL += ModificarDoble(GananciaA, "ganan_A")
        strSQL += ModificarDoble(GananciaB, "ganan_B")
        strSQL += ModificarDoble(GananciaC, "ganan_C")
        strSQL += ModificarDoble(GananciaD, "ganan_D")
        strSQL += ModificarDoble(GananciaE, "ganan_E")
        strSQL += ModificarDoble(GananciaF, "ganan_F")
        strSQL += ModificarCadena(BarraA, "barra_a")
        strSQL += ModificarCadena(BarraB, "barra_b")
        strSQL += ModificarCadena(BarraC, "barra_c")
        strSQL += ModificarCadena(BarraD, "barra_d")
        strSQL += ModificarCadena(BarraE, "barra_e")
        strSQL += ModificarCadena(BarraF, "barra_f")
        strSQL += ModificarEnteroLargo(TipoArticulo, "tipoart")
        strSQL += ModificarEntero(MIX, "mix")
        strSQL += ModificarEntero(Estatus, "estatus")
        strSQL += ModificarFecha(Ingreso, "creacion")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

        Dim nTableAlm As String = "tblAlmacen"
        Dim dsAlm As New DataSet
        Dim dtAlm As DataTable
        dsAlm = DataSetRequery(dsAlm, " select * from jsmercatalm where id_emp = '" & jytsistema.WorkID & "'  ", MyConn, nTableAlm, lblInfo)
        dtAlm = dsAlm.Tables(nTableAlm)

        For bCont As Integer = 0 To dtAlm.Rows.Count - 1
            With dtAlm.Rows(bCont)
                EjecutarSTRSQL(MyConn, lblInfo, " REPLACE INTO jsmerextalm SET CODART = '" & Codigo & "', ALMACEN = '" _
                               & .Item("codalm") & "', EXISTENCIA = 0.00,  UBICACION = '', ID_EMP = '" & jytsistema.WorkID & "'  ")
            End With
        Next

        dtAlm.Dispose()
        dsAlm.Dispose()
        dtAlm = Nothing
        dsAlm = Nothing

        ActualizarExistenciasPlus(MyConn, Codigo)


    End Sub
    Public Sub InsertEditMERCASEncabezadoTransferencia(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal NumeroTransferencia As String, _
                                                        ByVal Emision As Date, ByVal AlmacenSalida As String, ByVal AlmacenEntrada As String, ByVal Comentario As String, _
                                                        ByVal TotalTransferencia As Double, ByVal TotalCantidad As Double, ByVal Items As Integer, ByVal PesoTotal As Double, _
                                                        ByVal Tipo As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmerenctra SET "
        Else
            strSQLInicio = " UPDATE jsmerenctra SET "
            strSQLFin = " WHERE " _
                & " numtra = '" & NumeroTransferencia & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroTransferencia, "numtra")
        strSQL += ModificarFecha(Emision, "emision")
        strSQL += ModificarCadena(AlmacenSalida, "alm_sale")
        strSQL += ModificarCadena(AlmacenEntrada, "alm_entra")
        strSQL += ModificarCadena(Comentario, "comen")
        strSQL += ModificarDoble(TotalTransferencia, "totaltra")
        strSQL += ModificarDoble(TotalCantidad, "totalcan")
        strSQL += ModificarEnteroLargo(Items, "items")
        strSQL += ModificarDoble(PesoTotal, "pesototal")
        strSQL += ModificarEnteroLargo(Tipo, "tipo")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditMERCASRenglonTransferencia(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal NumeroTransferencia As String, _
                                                        ByVal Renglon As String, ByVal item As String, ByVal Descripcion As String, ByVal Unidad As String, _
                                                        ByVal Cantidad As Double, ByVal Peso As Double, ByVal Lote As String, ByVal CostoUnitario As Double, _
                                                        ByVal TotalRenglon As Double, ByVal Aceptado As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmerrentra SET "
        Else
            strSQLInicio = " UPDATE jsmerrentra SET "
            strSQLFin = " WHERE " _
                & " numtra = '" & NumeroTransferencia & "' and " _
                & " renglon = '" & Renglon & "' and " _
                & " item = '" & item & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroTransferencia, "numtra")
        strSQL += ModificarCadena(Renglon, "renglon")
        strSQL += ModificarCadena(item, "item")
        strSQL += ModificarCadena(Descripcion, "descrip")
        strSQL += ModificarCadena(Unidad, "unidad")
        strSQL += ModificarDoble(Cantidad, "cantidad")
        strSQL += ModificarDoble(Peso, "peso")
        strSQL += ModificarCadena(Lote, "lote")
        strSQL += ModificarDoble(CostoUnitario, "costou")
        strSQL += ModificarDoble(TotalRenglon, "totren")
        strSQL += ModificarCadena(Aceptado, "aceptado")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub


    Public Sub InsertEditMERCASMovimientoInventario(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal CodigoArticulo As String, _
                            ByVal FechaMovimiento As Date, ByVal TipoMovimiento As String, ByVal Documento As String, _
                            ByVal Unidad As String, ByVal Cantidad As Double, ByVal Peso As Double, ByVal CostoTotal As Double, _
                            ByVal CostoTotalDescuento As Double, ByVal Origen As String, ByVal NumeroOrigen As String, ByVal Lote As String, _
                            ByVal ProveedorCliente As String, ByVal VentaTotal As Double, ByVal VentaTotalDescuento As Double, _
                            ByVal ImporteIVA As Double, ByVal Descuento As Double, ByVal Vendedor As String, ByVal Almacen As String, _
                            ByVal Asiento As String, ByVal FechaAsiento As Date, Optional ActualizaInventario As Boolean = True)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmertramer SET "
        Else
            strSQLInicio = " UPDATE jsmertramer SET "
            strSQLFin = " WHERE " _
                & " codart = '" & CodigoArticulo & "' and " _
                & " date_format(fechamov, '%Y-%m-%d') = '" & FormatoFechaMySQL(FechaMovimiento) & "' and " _
                & " tipomov = '" & TipoMovimiento & "' and " _
                & " numdoc = '" & Documento & "' and " _
                & " origen = '" & Origen & "' and " _
                & " numorg = '" & NumeroOrigen & "' and " _
                & " asiento = '" & Asiento & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoArticulo, "codart")
        strSQL += ModificarFechaTiempo(FechaMovimiento, "fechamov")
        strSQL += ModificarCadena(TipoMovimiento, "tipomov")
        strSQL += ModificarCadena(Documento, "numdoc")
        strSQL += ModificarCadena(Unidad, "unidad")
        strSQL += ModificarDoble(Cantidad, "cantidad")
        strSQL += ModificarDoble(Peso, "peso")
        strSQL += ModificarDoble(CostoTotal, "costotal")
        strSQL += ModificarDoble(CostoTotalDescuento, "costotaldes")
        strSQL += ModificarCadena(Origen, "origen")
        strSQL += ModificarCadena(NumeroOrigen, "numorg")
        strSQL += ModificarCadena(Lote, "lote")
        strSQL += ModificarCadena(ProveedorCliente, "prov_cli")
        strSQL += ModificarDoble(VentaTotal, "ventotal")
        strSQL += ModificarDoble(VentaTotalDescuento, "ventotaldes")
        strSQL += ModificarDoble(ImporteIVA, "impiva")
        strSQL += ModificarDoble(Descuento, "descuento")
        strSQL += ModificarCadena(Vendedor, "vendedor")
        strSQL += ModificarCadena(Almacen, "almacen")
        strSQL += ModificarCadena(Asiento, "asiento")
        strSQL += ModificarFecha(FechaAsiento, "fechasi")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))


        Dim nTableAlm As String = "tblAlmacen"
        Dim dsAlm As New DataSet
        Dim dtAlm As DataTable
        dsAlm = DataSetRequery(dsAlm, " select * from jsmercatalm where id_emp = '" & jytsistema.WorkID & "'  ", MyConn, nTableAlm, lblInfo)
        dtAlm = dsAlm.Tables(nTableAlm)

        For bCont As Integer = 0 To dtAlm.Rows.Count - 1
            With dtAlm.Rows(bCont)
                EjecutarSTRSQL(MyConn, lblInfo, " REPLACE INTO jsmerextalm SET CODART = '" & CodigoArticulo & "', ALMACEN = '" _
                               & .Item("codalm") & "', EXISTENCIA = 0.00,  UBICACION = '', ID_EMP = '" & jytsistema.WorkID & "'  ")
            End With
        Next

        dtAlm.Dispose()
        dsAlm.Dispose()
        dtAlm = Nothing
        dsAlm = Nothing

        If ActualizaInventario Then ActualizarExistenciasPlus(MyConn, CodigoArticulo)


    End Sub

    Public Sub InsertEditMERCASAlmacen(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal CodigoAlmacen As String, _
                           ByVal Descripcíon As String, ByVal Responsable As String, ByVal Tipoalmacen As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmercatalm SET "
        Else
            strSQLInicio = " UPDATE jsmercatalm SET "
            strSQLFin = " WHERE " _
                & " codalm = '" & CodigoAlmacen & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoAlmacen, "codalm")
        strSQL += ModificarCadena(Descripcíon, "desalm")
        strSQL += ModificarCadena(Responsable, "responsable")
        strSQL += ModificarEnteroLargo(Tipoalmacen, "tipoalm")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditMERCASEquivalencia(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                             ByVal CodigoMercancia As String, ByVal Unidad As String, _
                                             ByVal Equivale As Double, ByVal UnidadEquivalente As String, _
                                             ByVal Divide As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmerequmer SET "
        Else
            strSQLInicio = " UPDATE jsmerequmer SET "
            strSQLFin = " WHERE " _
                & " codart = '" & CodigoMercancia & "' and " _
                & " unidad = '" & Unidad & "' and " _
                & " uvalencia = '" & UnidadEquivalente & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoMercancia, "codart")
        strSQL += ModificarCadena(Unidad, "unidad")
        strSQL += ModificarDoble(Equivale, "equivale")
        strSQL += ModificarCadena(UnidadEquivalente, "uvalencia")
        strSQL += ModificarEntero(Divide, "divide")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditMERCASRenglonesConteo(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal NumeroConteo As String, _
                                                        ByVal item As String, ByVal Descripcion As String, ByVal Unidad As String, _
                                                        ByVal Conteo As Double, ByVal Existencia As Double, ByVal Existencia1 As Double, ByVal Conteo1 As Double, _
                                                        ByVal Existencia2 As Double, ByVal Conteo2 As Double, ByVal Existencia3 As Double, ByVal Conteo3 As Double, _
                                                        ByVal Existencia4 As Double, ByVal Conteo4 As Double, ByVal Existencia5 As Double, ByVal Conteo5 As Double, _
                                                        ByVal CostoUnitario As Double, ByVal CostoTotal As Double, ByVal Aceptado As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmerconmer SET "
        Else
            strSQLInicio = " UPDATE jsmerconmer SET "
            strSQLFin = " WHERE " _
                & " conmer = '" & NumeroConteo & "' and " _
                & " codart = '" & item & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroConteo, "conmer")
        strSQL += ModificarCadena(item, "codart")
        strSQL += ModificarCadena(Descripcion, "nomart")
        strSQL += ModificarCadena(Unidad, "unidad")
        strSQL += ModificarDoble(Existencia, "existencia")
        strSQL += ModificarDoble(Conteo, "conteo")
        strSQL += ModificarDoble(Existencia1, "exist1")
        strSQL += ModificarDoble(Conteo1, "cont1")
        strSQL += ModificarDoble(Existencia2, "exist2")
        strSQL += ModificarDoble(Conteo2, "cont2")
        strSQL += ModificarDoble(Existencia3, "exist3")
        strSQL += ModificarDoble(Conteo3, "cont3")
        strSQL += ModificarDoble(Existencia4, "exist4")
        strSQL += ModificarDoble(Conteo4, "cont4")
        strSQL += ModificarDoble(Existencia5, "exist5")
        strSQL += ModificarDoble(Conteo5, "cont5")
        strSQL += ModificarDoble(CostoUnitario, "costou")
        strSQL += ModificarDoble(CostoTotal, "costo_tot")
        strSQL += ModificarCadena(Aceptado, "aceptado")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditMERCASEncabezadoConteo(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal NumeroConteo As String, _
                                                        ByVal FechaConteo As Date, ByVal Almacen As String, ByVal Comentario As String, ByVal Procesado As Integer, _
                                                        ByVal FechaProceso As Date)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmerenccon SET "
        Else
            strSQLInicio = " UPDATE jsmerenccon SET "
            strSQLFin = " WHERE " _
                & " conmer = '" & NumeroConteo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroConteo, "conmer")
        strSQL += ModificarFecha(FechaConteo, "fechacon")
        strSQL += ModificarCadena(Almacen, "almacen")
        strSQL += ModificarCadena(Comentario, "comentario")
        strSQL += ModificarEntero(Procesado, "procesado")
        strSQL += ModificarFecha(FechaProceso, "fechapro")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditMERCASEncabezadoListaPrecios(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                                       ByVal NumeroLista As String, ByVal FechaInicio As Date, ByVal Comentario As String, _
                                                       ByVal FechaCierre As Date)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmerenclispre SET "
        Else
            strSQLInicio = " UPDATE jsmerenclispre SET "
            strSQLFin = " WHERE " _
                & " codlis = '" & NumeroLista & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroLista, "codlis")
        strSQL += ModificarFecha(FechaInicio, "emision")
        strSQL += ModificarCadena(Comentario, "descrip")
        strSQL += ModificarFecha(FechaCierre, "vence")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditMERCASEncabezadoJerarquia(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                                       ByVal TipoJerarquia As String, ByVal NombreJerarquia As String, _
                                                       ByVal Mascara1 As String, ByVal Mascara2 As String, ByVal Mascara3 As String, ByVal Mascara4 As String, _
                                                       ByVal Mascara5 As String, ByVal Mascara6 As String, ByVal Descripcion1 As String, _
                                                       ByVal Descripcion2 As String, ByVal Descripcion3 As String, ByVal Descripcion4 As String, _
                                                       ByVal Descripcion5 As String, ByVal Descripcion6 As String, ByVal Proveedor As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmerencjer SET "
        Else
            strSQLInicio = " UPDATE jsmerencjer SET "
            strSQLFin = " WHERE " _
                & " tipjer = '" & TipoJerarquia & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(TipoJerarquia, "tipjer")
        strSQL += ModificarCadena(NombreJerarquia, "descrip")
        strSQL += ModificarCadena(Mascara1, "mascara1")
        strSQL += ModificarCadena(Mascara2, "mascara2")
        strSQL += ModificarCadena(Mascara3, "mascara3")
        strSQL += ModificarCadena(Mascara4, "mascara4")
        strSQL += ModificarCadena(Mascara5, "mascara5")
        strSQL += ModificarCadena(Mascara6, "mascara6")
        strSQL += ModificarCadena(Descripcion1, "descrip1")
        strSQL += ModificarCadena(Descripcion2, "Descrip2")
        strSQL += ModificarCadena(Descripcion3, "Descrip3")
        strSQL += ModificarCadena(Descripcion4, "Descrip4")
        strSQL += ModificarCadena(Descripcion5, "Descrip5")
        strSQL += ModificarCadena(Descripcion6, "Descrip6")
        strSQL += ModificarCadena(Proveedor, "PROVEEDOR")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditMERCASRenglonListaPrecios(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                                       ByVal NumeroLista As String, ByVal CodigoMercancia As String, _
                                                       ByVal Precio As Double)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmerrenlispre SET "
        Else
            strSQLInicio = " UPDATE jsmerrenlispre SET "
            strSQLFin = " WHERE " _
                & " codlis = '" & NumeroLista & "' and " _
                & " codart = '" & CodigoMercancia & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroLista, "codlis")
        strSQL += ModificarCadena(CodigoMercancia, "codart")
        strSQL += ModificarDoble(Precio, "precio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditMERCASRenglonJerarquia(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                                       ByVal TipoJerarquia As String, ByVal CodigoJerarquia As String, _
                                                       ByVal DescripcionJerarquia As String, ByVal Nivel As Integer, _
                                                       ByVal Aceptado As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmerrenjer SET "
        Else
            strSQLInicio = " UPDATE jsmerrenjer SET "
            strSQLFin = " WHERE " _
                & " tipjer = '" & TipoJerarquia & "' and " _
                & " codjer = '" & CodigoJerarquia & "' and " _
                & " nivel = " & Nivel & " and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(TipoJerarquia, "tipjer")
        strSQL += ModificarCadena(CodigoJerarquia, "codjer")
        strSQL += ModificarCadena(DescripcionJerarquia, "desjer")
        strSQL += ModificarEntero(Nivel, "nivel")
        strSQL += ModificarCadena(Aceptado, "aceptado")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditMERCASPrecioFuturo(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                                       ByVal FechaPrecio As Date, ByVal CodigoMercancia As String, _
                                                       ByVal TipoPrecio As String, ByVal Precio As Double, ByVal Descuento As Double, _
                                                       ByVal Procesado As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmerlispre SET "
        Else
            strSQLInicio = " UPDATE jsmerlispre SET "
            strSQLFin = " WHERE " _
                & " fecha = '" & FormatoFechaMySQL(FechaPrecio) & "' and " _
                & " codart = '" & CodigoMercancia & "' and " _
                & " tipoprecio = '" & TipoPrecio & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarFecha(FechaPrecio, "fecha")
        strSQL += ModificarCadena(CodigoMercancia, "codart")
        strSQL += ModificarCadena(TipoPrecio, "tipoprecio")
        strSQL += ModificarDoble(Precio, "monto")
        strSQL += ModificarDoble(Descuento, "des_art")
        strSQL += ModificarEntero(Procesado, "procesado")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditMERCASEncabezadoOferta(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal NumeroOferta As String, _
                                                       ByVal Desde As Date, ByVal Hasta As Date, ByVal Comentario As String, _
                                                       ByVal TarifaA As Integer, ByVal TarifaB As Integer, ByVal TarifaC As Integer, _
                                                       ByVal TarifaD As Integer, ByVal TarifaE As Integer, ByVal TarifaF As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmerencofe SET "
        Else
            strSQLInicio = " UPDATE jsmerencofe SET "
            strSQLFin = " WHERE " _
                & " codofe = '" & NumeroOferta & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroOferta, "codofe")
        strSQL += ModificarFecha(Desde, "desde")
        strSQL += ModificarFecha(Hasta, "hasta")
        strSQL += ModificarCadena(Comentario, "descrip")

        strSQL += ModificarCadena(TarifaA, "tarifa_a")
        strSQL += ModificarCadena(TarifaB, "tarifa_b")
        strSQL += ModificarCadena(TarifaC, "tarifa_c")
        strSQL += ModificarCadena(TarifaD, "tarifa_d")
        strSQL += ModificarCadena(TarifaE, "tarifa_e")
        strSQL += ModificarCadena(TarifaF, "tarifa_f")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditMERCASRenglonOferta(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal NumeroOferta As String, _
                                                       ByVal renglon As String, ByVal CodigoArticulo As String, ByVal Descripcion As String, _
                                                       ByVal unidad As String, ByVal limiteInferior As Double, ByVal limiteSuperior As Double, _
                                                       ByVal Porcentaje As Double, ByVal Otorgante As Integer, ByVal Aceptado As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmerrenofe SET "
        Else
            strSQLInicio = " UPDATE jsmerrenofe SET "
            strSQLFin = " WHERE " _
                & " codofe = '" & NumeroOferta & "' and " _
                & " renglon = '" & renglon & "' and  " _
                & " codart = '" & CodigoArticulo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroOferta, "codofe")
        strSQL += ModificarCadena(renglon, "renglon")
        strSQL += ModificarCadena(CodigoArticulo, "codart")
        strSQL += ModificarCadena(Descripcion, "descrip")
        strSQL += ModificarCadena(unidad, "unidad")
        strSQL += ModificarDoble(limiteInferior, "limitei")
        strSQL += ModificarDoble(limiteSuperior, "limites")
        strSQL += ModificarDoble(Porcentaje, "porcentaje")
        strSQL += ModificarEntero(Otorgante, "otorgapor")
        strSQL += ModificarCadena(Aceptado, "aceptado")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditMERCASRenglonBonificacion(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal NumeroOferta As String, _
                                                    ByVal CodigoArticulo As String, ByVal RENGLON As String, ByVal UNIDAD As String, ByVal CANTIDAD As Double, ByVal CantidadBonificable As Double, _
                                                    ByVal CantidadInicial As Double, ByVal UnidadBonificable As String, ByVal ItemBonificable As String, ByVal NombreItemBonificable As String, _
                                                    ByVal Otorgante As Integer, ByVal Aceptado As String)


        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmerrenbon SET "
        Else
            strSQLInicio = " UPDATE jsmerrenbon SET "
            strSQLFin = " WHERE " _
                & " codofe = '" & NumeroOferta & "' and " _
                & " codart = '" & CodigoArticulo & "' and " _
                & " renglon = '" & RENGLON & "' and  " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroOferta, "codofe")
        strSQL += ModificarCadena(CodigoArticulo, "codart")
        strSQL += ModificarCadena(RENGLON, "renglon")
        strSQL += ModificarCadena(UNIDAD, "unidad")
        strSQL += ModificarDoble(CANTIDAD, "cantidad")
        strSQL += ModificarDoble(CantidadInicial, "cantidadinicio")
        strSQL += ModificarDoble(CantidadBonificable, "cantidadbon")
        strSQL += ModificarCadena(UnidadBonificable, "unidadbon")
        strSQL += ModificarCadena(ItemBonificable, "itembon")
        strSQL += ModificarCadena(NombreItemBonificable, "nombreitembon")
        strSQL += ModificarEntero(Otorgante, "otorgacan")
        strSQL += ModificarCadena(Aceptado, "aceptado")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditMERCASServicio(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal CodigoServicio As String, _
                                                     ByVal Descripcion As String, ByVal Porcentaje As Double, ByVal Precio As Double, _
                                                     ByVal PrecioA As Double, ByVal PrecioB As Double, ByVal PrecioC As Double, ByVal horas As Double, ByVal HorasA As Double, _
                                                     ByVal HorasB As Double, ByVal HorasC As Double, ByVal Columnas As Integer, ByVal Cemtimetros As Integer, _
                                                     ByVal TipoIVa As String, ByVal TipoServicio As Integer, ByVal MercanciaServicio As String, ByVal Tipo As Integer, _
                                                     ByVal Clase As Integer, ByVal CodigoContable As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmercatser SET "
        Else
            strSQLInicio = " UPDATE jsmercatser SET "
            strSQLFin = " WHERE " _
                & " codser = '" & CodigoServicio & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoServicio, "codser")
        strSQL += ModificarCadena(Descripcion, "desser")
        strSQL += ModificarDoble(Porcentaje, "porcentaje")
        strSQL += ModificarDoble(Precio, "precio")
        strSQL += ModificarDoble(PrecioA, "precio_a")
        strSQL += ModificarDoble(PrecioB, "precio_b")
        strSQL += ModificarDoble(PrecioC, "precio_c")
        strSQL += ModificarDoble(horas, "horas")
        strSQL += ModificarDoble(HorasA, "horas_a")
        strSQL += ModificarDoble(HorasB, "horas_b")
        strSQL += ModificarDoble(HorasC, "horas_c")
        strSQL += ModificarEnteroLargo(Cemtimetros, "cms")
        strSQL += ModificarEnteroLargo(Columnas, "col")
        strSQL += ModificarCadena(TipoIVa, "tipoiva")
        strSQL += ModificarEntero(TipoServicio, "tiposervicio")
        strSQL += ModificarCadena(MercanciaServicio, "codart_codser")
        strSQL += ModificarEntero(Tipo, "tipo")
        strSQL += ModificarEntero(Clase, "clase")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditMERCASLoteMercancia(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal CodigoMercancia As String, _
                                                    ByVal NumeroDeLote As String, ByVal FechaVencimiento As Date, ByVal Entradas As Double, _
                                                    ByVal Salidas As Double)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmerlotmer SET "
        Else
            strSQLInicio = " UPDATE jsmerlotmer SET "
            strSQLFin = " WHERE " _
                & " codart = '" & CodigoMercancia & "' and " _
                & " lote = '" & NumeroDeLote & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoMercancia, "codart")
        strSQL += ModificarCadena(NumeroDeLote, "lote")
        strSQL += ModificarFecha(FechaVencimiento, "expiracion")
        strSQL += ModificarDoble(Entradas, "entradas")
        strSQL += ModificarDoble(Salidas, "salidas")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditMERCASCuotasMercancia(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal CodigoMercancia As String, _
                                                    ByVal Ene As Double, ByVal Feb As Double, ByVal Mar As Double, ByVal Abr As Double, _
                                                    ByVal May As Double, ByVal Jun As Double, ByVal Jul As Double, ByVal Ago As Double, _
                                                    ByVal Sep As Double, ByVal Oct As Double, ByVal Nov As Double, ByVal Dic As Double)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsmerctacuo SET "
        Else
            strSQLInicio = " UPDATE jsmerctacuo SET "
            strSQLFin = " WHERE " _
                & " codart = '" & CodigoMercancia & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoMercancia, "codart")
        strSQL += ModificarDoble(Ene, "mes01")
        strSQL += ModificarDoble(Feb, "mes02")
        strSQL += ModificarDoble(Mar, "mes03")
        strSQL += ModificarDoble(Abr, "mes04")
        strSQL += ModificarDoble(May, "mes05")
        strSQL += ModificarDoble(Jun, "mes06")
        strSQL += ModificarDoble(Jul, "mes07")
        strSQL += ModificarDoble(Ago, "mes08")
        strSQL += ModificarDoble(Sep, "mes09")
        strSQL += ModificarDoble(Oct, "mes10")
        strSQL += ModificarDoble(Nov, "mes11")
        strSQL += ModificarDoble(Dic, "mes12")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditMERCASExpedienteMercancias(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                       ByVal CODIGOMERCANCIA As String, ByVal FechaMovimiento As Date, _
                       ByVal Comentario As String, ByVal Condicion As Integer, ByVal Causa As String, _
                       ByVal TipoCondicion As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsmerexpmer SET "
        Else
            strSQLInicio = " UPDATE jsmerexpmer SET "
            strSQLFin = " WHERE " _
                & " codart = '" & CODIGOMERCANCIA & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CODIGOMERCANCIA, "codART")
        strSQL += ModificarFechaTiempo(FechaMovimiento, "fecha")
        strSQL += ModificarCadena(Comentario, "comentario")
        strSQL += ModificarEntero(Condicion, "condicion")
        strSQL += ModificarCadena(Causa, "causa")
        strSQL += ModificarEntero(TipoCondicion, "tipocondicion")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        EjecutarSTRSQL(MyConn, lblInfo, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

End Module
