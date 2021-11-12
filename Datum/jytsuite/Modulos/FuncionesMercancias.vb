Imports MySql.Data.MySqlClient
Module FuncionesMercancias

    Private ft As New Transportables

    Public Function BonificacionVigenteOferta(MyConn As MySqlConnection, lblInfo As Label, dFechaActual As Date, _
        sCodigoArticulo As String, sRenglon As String, sTarifa As String, sNumDoc As String, iModulo As Integer, sVIA As Integer) As Bonificaciones


        Dim ds As New DataSet
        Dim dtBonificacionVigente As DataTable

        Dim nTableBonificacion As String = "tbl_Bonos"

        Dim TablaRenglon As String = ""
        Dim sDocumento As String = ""
        Dim sCantidadenRenglones As Double

        If sTarifa = "" Then sTarifa = "A"

        Dim nTabla() As String = {"jsvenrenfac", "jsvenrenped", "jsvenrenpedrgv", "jsvenrencot", "jsvenrennot"}
        Dim nCampo() As String = {"numfac", "numped", "numped", "numcot", "numfac"}

        BonificacionVigenteOferta.CantidadABonificar = 0
        BonificacionVigenteOferta.ItemABonificar = ""
        BonificacionVigenteOferta.UnidadDeBonificacion = ""

        sCantidadenRenglones = ft.DevuelveScalarDoble(MyConn, "  select " _
            & " SUM(IF( ISNULL(b.UVALENCIA), a.CANTIDAD, a.CANTIDAD/b.EQUIVALE)) from " _
            & nTabla(iModulo) & " a LEFT JOIN jsmerequmer b " _
            & " ON (a.ITEM = b.CODART AND a.UNIDAD = b.UVALENCIA and a.id_emp = b.id_emp) " _
            & " WHERE " _
            & " a." & nCampo(iModulo) & " = '" & sNumDoc & "' AND " _
            & " a.RENGLON <> '' AND " _
            & " a.ITEM = '" & sCodigoArticulo & "' AND " _
            & " a.ESTATUS < '2' AND " _
            & " a.ACEPTADO <= '1' AND " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
            & " GROUP BY ITEM ")

        ds = DataSetRequery(ds, " select jsmerencofe.*, jsmerrenbon.* from jsmerencofe, jsmerrenbon " _
                        & " WHERE " _
                        & " jsmerencofe.CODOFE = jsmerrenbon.CODOFE AND " _
                        & " jsmerencofe.DESDE <= '" & ft.FormatoFechaMySQL(dFechaActual) & "' AND " _
                        & " jsmerencofe.HASTA >= '" & ft.FormatoFechaMySQL(dFechaActual) & "' AND " _
                        & " jsmerrenbon.CODART = '" & sCodigoArticulo & "' AND " _
                        & " jsmerencofe.TARIFA_" & sTarifa & "  = '1' AND " _
                        & " jsmerrenbon.ID_EMP = '" & jytsistema.WorkID & "' AND " _
                        & " jsmerrenbon.CANTIDAD <= " & sCantidadenRenglones & " AND " _
                        & " jsmerrenbon.OTORGACAN = " & sVIA & " AND " _
                        & " jsmerencofe.ID_EMP = '" & jytsistema.WorkID & "' " _
                        & " ORDER BY DESDE DESC, jsmerrenbon.CANTIDAD DESC LIMIT 1 ", MyConn, nTableBonificacion, lblInfo)

        dtBonificacionVigente = ds.Tables(nTableBonificacion)

        If dtBonificacionVigente.Rows.Count > 0 Then
            With dtBonificacionVigente.Rows(0)
                If sCantidadenRenglones >= .Item("CANTIDADINICIO") Then
                    BonificacionVigenteOferta.CantidadABonificar = Int(sCantidadenRenglones / .Item("CANTIDAD")) * .Item("CANTIDADBON")
                    BonificacionVigenteOferta.ItemABonificar = .Item("ITEMBON")
                    BonificacionVigenteOferta.UnidadDeBonificacion = .Item("UNIDADBON")
                End If
            End With
        End If

        dtBonificacionVigente.Dispose()
        dtBonificacionVigente = Nothing

    End Function

    Public Function ExistenciasEnAlmacenes(ByVal MyConn As MySqlConnection, ByVal CodigoMercancia As String, _
                                                 CodigoAlmacen As String) As Double

        ExistenciasEnAlmacenes = 0.0
        Dim ds As New DataSet
        Dim dt As DataTable = ft.AbrirDataTable(ds, "tblexistealm", MyConn, " SELECT a.codart, a.almacen, c.unidad,  " _
                    & " ROUND( SUM(IF( a.tipomov IN ('EN', 'AE', 'DV') ,  a.cantidad, if ( a.tipomov IN ('SA','AS','DC'), -1*a.cantidad, 0 )) *(IF( ISNULL(b.uvalencia), 1, 1/b.equivale  ))), 3) Existencia " _
                    & " FROM jsmertramer a  " _
                    & " LEFT JOIN jsmerequmer b ON (a.codart = b.codart AND a.unidad = b.uvalencia AND a.id_emp = b.id_emp)  " _
                    & " LEFT JOIN jsmerctainv c ON (a.codart = c.codart AND a.id_emp = c.id_emp)  " _
                    & " WHERE " _
                    & IIf(CodigoAlmacen = "", "", " a.almacen = '" & CodigoAlmacen & "' AND ") _
                    & " a.codart = '" & CodigoMercancia & "' AND " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " GROUP BY a.codart, a.almacen  ")

        For Each nRow As DataRow In dt.Rows
            With nRow
                If ft.DevuelveScalarEntero(MyConn, " select count(*) from jsmerextalm where " _
                                                               & " codart = '" & .Item("codart") & "' and " _
                                                               & " almacen = '" & .Item("almacen") & "' and " _
                                                               & " id_emp = '" & jytsistema.WorkID & "'  ") > 0 Then

                    ft.Ejecutar_strSQL(MyConn, " update jsmerextalm set existencia = " & .Item("existencia") & " where codart = '" & .Item("codart") & "' and almacen = '" & .Item("almacen") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                Else
                    ft.Ejecutar_strSQL(MyConn, " insert into jsmerextalm set codart = '" & UCase(.Item("codart")) & "', almacen = '" & .Item("almacen") & "', existencia = " & .Item("existencia") & " , ubicacion = '', id_emp = '" & jytsistema.WorkID & "'  ")

                End If
                If .Item("almacen") = CodigoAlmacen Then ExistenciasEnAlmacenes = .Item("existencia")
            End With
        Next

        dt = Nothing
        ds = Nothing

    End Function

    'Function ExistenciaEnAlmacen(ByVal MyConn As MySqlConnection, ByVal CodigoMercancia As String, _
    '                              ByVal CodigoAlmacen As String) As Double

    '    ExistenciaEnAlmacen = ft.DevuelveScalarDoble(MyConn, " SELECT SUM(IF( a.tipomov IN ('AE','EN'), 1,  " _
    '                                                     & " IF( a.tipomov IN ('SA', 'AS') , -1, 0 ) ) * IF( b.uvalencia IS NULL,  a.cantidad, a.cantidad/b.equivale )  ) " _
    '                                                     & " FROM jsmertramer a " _
    '                                                     & " LEFT JOIN jsmerequmer b ON (a.codart = b.codart AND a.unidad = b.uvalencia AND a.id_emp = b.id_emp) " _
    '                                                     & " WHERE " _
    '                                                     & " a.codart  = '" & CodigoMercancia & "' AND " _
    '                                                     & " a.almacen = '" & CodigoAlmacen & "' AND " _
    '                                                     & " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(jytsistema.sFechadeTrabajo) & "'  AND " _
    '                                                     & " a.id_emp = '" & jytsistema.WorkID & "' " _
    '                                                     & " GROUP BY a.codart ")

    'End Function
    Function MercanciaPoseeMovimientos(ByVal myconn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoArticulo As String) As Boolean
        MercanciaPoseeMovimientos = ft.DevuelveScalarBooleano(myconn, "SELECT COUNT(DISTINCT codart) FROM jsmertramer WHERE codart = '" & CodigoArticulo & "' AND id_emp = '" & jytsistema.WorkID & "' ")
    End Function
    Function MercanciaRegulada(ByVal myconn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoArticulo As String) As Boolean
        MercanciaRegulada = ft.DevuelveScalarBooleano(myconn, "SELECT REGULADO FROM jsmerctainv WHERE codart = '" & CodigoArticulo & "' AND id_emp = '" & jytsistema.WorkID & "' ")
    End Function

    Function EsMercanciaCombo(MyConn As MySqlConnection, codigoArticulo As String) As Boolean
        Dim cantidadCombo As Integer = ft.DevuelveScalarEntero(MyConn, " select count(*) from jsmercatcom " _
                                                                      & " where " _
                                                                      & " codart = '" & codigoArticulo & "' and " _
                                                                      & " id_emp = '" & jytsistema.WorkID & "' ")

        If cantidadCombo > 0 Then Return True

        Return False


    End Function


    Function MovimientoXDocumentoRenglonAlmacen(ByVal MyConn As MySqlConnection, ByVal CodigoMercancia As String, _
                                                ByVal Documento As String, ByVal Origen As String, ByVal Renglon As String, _
                                                ByVal Almacen As String, ByVal lblInfo As Label) As Double

        Dim ds As New DataSet
        Dim dt As DataTable
        Dim nTabla As String = "tblmovxdoc"
        ds = DataSetRequery(ds, " SELECT a.codart, ROUND(IF( ISNULL(b.equivale), a.cantidad, a.cantidad / b.equivale),3) cantidad, a.unidad FROM jsmertramer a " _
                & " LEFT JOIN jsmerequmer b ON (a.codart = b.codart AND a.unidad = b.uvalencia AND a.id_emp = b.id_emp) " _
                & " WHERE " _
                & " a.codart = '" & CodigoMercancia & "' and " _
                & " a.numorg  = '" & Documento & "' AND " _
                & " a.origen = '" & Origen & "' AND  " _
                & " a.asiento = '" & Renglon & "' AND  " _
                & " a.almacen = '" & Almacen & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "'", MyConn, nTabla, lblInfo)

        dt = ds.Tables(nTabla)
        MovimientoXDocumentoRenglonAlmacen = 0.0
        If dt.Rows.Count > 0 Then MovimientoXDocumentoRenglonAlmacen = dt.Rows(0).Item("cantidad")

        dt = Nothing
        ds = Nothing

    End Function

    Function MultiploValido(ByVal MyConn As MySqlConnection, ByVal CodigoMercancia As String, ByVal UnidadDeVenta As String, _
                            ByVal Cantidad As Double, ByVal lblInfo As Label) As Boolean
        Dim ds As New DataSet
        Dim dt As DataTable
        Dim nTabla As String = "tblmultiplo"

        ds = DataSetRequery(ds, "SELECT a.codart, a.unidad unidad, 1 equivale, a.divideuv divide " _
                & " FROM jsmerctainv a " _
                & " WHERE " _
                & " a.codart = '" & CodigoMercancia & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " UNION " _
                & " SELECT a.codart, a.uvalencia unidad, a.equivale, FORMAT(a.divide , 0) divide " _
                & " FROM jsmerequmer a  " _
                & " WHERE " _
                & " a.codart = '" & CodigoMercancia & "' AND  " _
                & " a.id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTabla, lblInfo)

        dt = ds.Tables(nTabla)
        MultiploValido = False

        Dim siDivide As Boolean = False

        Dim fila() As DataRow
        fila = dt.Select("unidad = '" & UnidadDeVenta & "'")
        If fila.Length > 0 Then
            For Each dr As DataRow In fila
                If dr("DIVIDE") <> "" Then
                    siDivide = CBool(dr("divide").ToString)
                Else
                    siDivide = False
                End If
            Next
        End If

        Dim afld() As String = {"codart", "id_emp"}
        Dim aStr() As String = {CodigoMercancia, jytsistema.WorkID}
        Dim Unidad As String = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "Unidad")

        If siDivide Then

            If Unidad = "KGR" Then
                dt = Nothing
                ds = Nothing
                Return True
            Else
                Dim kCont As Integer
                For kCont = 0 To dt.Rows.Count - 1


                    With dt.Rows(kCont)
                        Dim UnidadAComparar As String = .Item("UNIDAD")

                        If UnidadAComparar = Unidad AndAlso dt.Rows.Count = 1 Then
                            dt = Nothing
                            ds = Nothing
                            Return True
                        Else
                            Dim Equivale As Double = 1 / .Item("equivale")
                            If Multiplo(Cantidad, Equivale) Then
                                dt = Nothing
                                ds = Nothing
                                Return True
                            End If
                        End If
                    End With
                Next
            End If
        Else
            MultiploValido = IIf(Cantidad - Microsoft.VisualBasic.Int(Cantidad) = 0, True, False)
        End If

        dt = Nothing
        ds = Nothing

    End Function
    Function PrecioOferta(ByVal MyConn As MySqlConnection, ByVal CodigoMercancia As String, _
                          ByVal Tarifa As String, ByVal Fecha As Date, ByVal lblInfo As Label) As Double
        Dim ds As New DataSet
        Dim dt As DataTable
        Dim nTabla As String = "tblOferta"

        ds = DataSetRequery(ds, "SELECT b.porcentaje FROM jsmerencofe a  " _
                                    & " LEFT JOIN  jsmerrenofe b ON (a.codofe = b.codofe AND a.id_emp = b.id_emp)  " _
                                    & " WHERE " _
                                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                    & " AND a.desde <= '" & ft.FormatoFechaMySQL(Fecha) & "' " _
                                    & " AND a.hasta >= '" & ft.FormatoFechaMySQL(Fecha) & "' " _
                                    & " AND codart = '" & CodigoMercancia & "' " _
                                    & " AND a.tarifa_" & Tarifa & "  = '1' ", MyConn, nTabla, lblInfo)

        dt = ds.Tables(nTabla)
        PrecioOferta = 0.0
        Dim aFld() As String = {"codart", "id_emp"}
        Dim aStr() As String = {CodigoMercancia, jytsistema.WorkID}
        If dt.Rows.Count > 0 Then PrecioOferta = Math.Round(CDbl(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "PRECIO_" & Tarifa)) * (1 - dt.Rows(0).Item("porcentaje") / 100), 2)

        dt = Nothing
        ds = Nothing

    End Function

    Function MensajeOferta(ByVal MyConn As MySqlConnection, ByVal CodigoArticulo As String, ByVal Fecha As Date, _
                           ByVal lblInfo As Label) As String

        Dim ds As New DataSet
        Dim dt As DataTable
        Dim nTabla As String = "tblmensajeoferta"

        ds = DataSetRequery(ds, "  select a.codofe, a.desde, a.hasta, b.codart, b.limitei As de, b.limites as a,  b.Unidad , b.porcentaje as porcentajeDescuento, elt(otorgapor+1,'JYTSUITE','El ASESOR') AS otorgadopor from jsmerencofe a " _
            & " left join  jsmerrenofe b on (a.codofe = b.codofe and a.id_emp = b.id_emp) " _
            & " Where " _
            & " a.id_emp = '" & jytsistema.WorkID & "'  " _
            & " and a.desde <= '" & ft.FormatoFechaMySQL(Fecha) & "' " _
            & " and a.hasta >= '" & ft.FormatoFechaMySQL(Fecha) & "' " _
            & " and codart = '" & CodigoArticulo & "' " _
            & " ", MyConn, nTabla, lblInfo)

        dt = ds.Tables(nTabla)
        MensajeOferta = ""
        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                MensajeOferta = " Oferta N°: " & .Item("codofe") & " Válida desde : " & ft.FormatoFecha(CDate(.Item("desde").ToString)) & " - Hasta : " & ft.FormatoFecha(CDate(.Item("hasta").ToString))
                If .Item("porcentajedescuento") > 0 Then _
                    MensajeOferta = MensajeOferta & vbCrLf & vbTab & "De " & ft.FormatoCantidad(.Item("de")) & " a " & ft.FormatoCantidad(.Item("a")) & " " & .Item("unidad") & "  " & .Item("otorgadopor") & " otorgará " & .Item("porcentajedescuento") & "% de descuento "
            End With
        End If

        ds = DataSetRequery(ds, " select a.codofe, a.desde, a.hasta, b.codart, b.cantidadinicio As a_partir, b.unidad, b.cantidadbon as Se_Bonificara, " _
            & " b.unidadbon, b.itembon as de, b.cantidad as Porcada, b.unidad, elt(otorgacan+1,'JYTSUITE','El ASESOR') as otorgadapor " _
            & " from jsmerencofe a " _
            & " left join  jsmerrenbon b on (a.codofe = b.codofe and a.id_emp = b.id_emp) " _
            & " Where " _
            & " a.id_emp = '" & jytsistema.WorkID & "'  " _
            & " and a.desde <= '" & ft.FormatoFechaMySQL(Fecha) & "' " _
            & " and a.hasta >= '" & ft.FormatoFechaMySQL(Fecha) & "' " _
            & " and codart = '" & CodigoArticulo & "' " _
            & " ", MyConn, nTabla, lblInfo)

        dt = ds.Tables(nTabla)
        If dt.Rows.Count > 0 Then
            With dt.Rows(0)
                MensajeOferta = MensajeOferta & vbCrLf & vbTab & "A partir de " & ft.FormatoCantidad(.Item("a_partir")) & " " & .Item("unidad") & " " & .Item("otorgadapor") & " bonificará " & ft.FormatoCantidad(.Item("se_bonificara")) & _
                    " " & .Item("unidadbon") & " de la mercancía " & .Item("de") & " cada " & ft.FormatoCantidad(.Item("porcada")) & " " & .Item("unidad") & " adicional "
            End With
        End If

        dt = Nothing
        ds = Nothing
    End Function
    Public Sub ActualizarExistenciasPlus(ByVal MyConn As MySqlConnection, ByVal CodigoMercancia As String, _
                                         Optional CodigoAlmacen As String = "", Optional Origen As String = "CXC")

        Dim lblInfo As New Label

        Dim existenciaActual As Double = ft.DevuelveScalarDoble(MyConn, " SELECT SUM(IF( a.tipomov IN ('AE','EN','DV'), 1,  IF( a.tipomov IN ('SA', 'AS','DC') , -1, 0 ) ) * IF( b.uvalencia IS NULL,  a.cantidad, a.cantidad/b.equivale )  )  " _
                                                                   & " FROM jsmertramer a  " _
                                                                   & " LEFT JOIN jsmerequmer b ON (a.codart = b.codart AND a.unidad = b.uvalencia AND a.id_emp = b.id_emp) " _
                                                                   & " WHERE " _
                                                                   & IIf(CodigoAlmacen = "", "", " a.almacen = '" & CodigoAlmacen & "' AND ") _
                                                                   & " a.codart  = '" & CodigoMercancia & "' AND  " _
                                                                   & " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(jytsistema.sFechadeTrabajo) & "' AND " _
                                                                   & " a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.codart ")

        ft.Ejecutar_strSQL(MyConn, " update jsmerctainv set existe_act = " & existenciaActual & " where codart = '" & CodigoMercancia & "' and id_emp = '" & jytsistema.WorkID & "' ")

        If Origen <> "PVE" Then ft.Ejecutar_strSQL(MyConn, " UPDATE jsmerctainv a, (SELECT a.codart, a.almacen, c.unidad,  " _
                        & " ROUND( SUM(IF( a.tipomov IN ('EN', 'AE', 'DV') ,  a.cantidad, 0) *(IF( ISNULL(b.uvalencia), 1, 1/b.equivale  ))), 3) Entradas, " _
                        & " ROUND( SUM(IF( a.tipomov IN ('SA', 'AS', 'DC') ,  a.cantidad, 0) *(IF( ISNULL(b.uvalencia), 1, 1/b.equivale  ))), 3) Salidas, " _
                        & " ROUND( SUM(IF( a.tipomov IN ('EN', 'AE', 'DV', 'AC'),  a.costotal , 0 )), 2 ) Costos, " _
                        & " ROUND( SUM(IF( a.tipomov IN ('EN', 'AE', 'DV', 'AC'),  a.costotaldes, 0)) , 2) CostosDescuentos,   " _
                        & " ROUND( SUM(IF( a.tipomov IN ('SA', 'AS', 'DC', 'AP'),  a.ventotal, 0 )), 2 ) Ventas,   " _
                        & " ROUND( SUM(IF( a.tipomov IN ('SA', 'AS', 'DC', 'AP'),  a.ventotaldes, 0 )) , 2) VentasDescuentos,  " _
                        & " if( d.fechamov is null, now(), d.fechamov) fechaultimocosto, IF( d.ultimocosto IS NULL, 0.00, d.ultimocosto) ultimocosto, if( d.ultimoproveedor is null, '', d.ultimoproveedor) ultimoproveedor, " _
                        & " if( e.fechamov is null, now(), e.fechamov) fechaultimaventa, if( e.ultimaventa is null, 0.00, e.ultimaventa) ultimaventa, if( e.ultimocliente is null, '', e.ultimocliente) ultimocliente, a.id_emp " _
                        & " FROM jsmertramer a  " _
                        & " LEFT JOIN jsmerequmer b ON (a.codart = b.codart AND a.unidad = b.uvalencia AND a.id_emp = b.id_emp)  " _
                        & " LEFT JOIN jsmerctainv c ON (a.codart = c.codart AND a.id_emp = c.id_emp)  " _
                        & " LEFT JOIN (SELECT a.codart, DATE_FORMAT(MAX(a.fechamov),'%Y-%m-%d') fechamov, ROUND(a.costotaldes / SUM(a.cantidad* IF( ISNULL(b.uvalencia), 1, 1/ b.equivale) ),2) ultimocosto, a.prov_cli ultimoproveedor, a.id_emp FROM jsmertramer a LEFT JOIN jsmerequmer b ON (a.codart = b.codart AND a.unidad = b.uvalencia AND a.id_emp = b.id_emp) WHERE a.codart = '" & CodigoMercancia & "' AND a.tipomov IN ('EN', 'AC', 'AE') AND a.origen IN ('COM','NCC','INV') AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.numorg ORDER BY a.FECHAMOV DESC LIMIT 1) d ON (a.codart = d.codart AND a.id_emp = d.id_emp) " _
                        & " LEFT JOIN (SELECT a.codart, DATE_FORMAT(MAX(a.fechamov),'%Y-%m-%d') fechamov, ROUND(a.ventotaldes / SUM(a.cantidad* IF( ISNULL(b.uvalencia), 1, 1/ b.equivale) ),2) ultimaventa, a.prov_cli ultimocliente, a.id_emp FROM jsmertramer a LEFT JOIN jsmerequmer b ON (a.codart = b.codart AND a.unidad = b.uvalencia AND a.id_emp = b.id_emp) WHERE a.codart = '" & CodigoMercancia & "' AND a.tipomov IN ('SA', 'AS') AND a.origen IN ('FAC','PFC', 'PVE', 'INV') AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.numorg ORDER BY a.FECHAMOV DESC LIMIT 1) e ON (a.codart = e.codart AND a.id_emp = e.id_emp)  " _
                        & " WHERE " _
                        & IIf(CodigoAlmacen = "", "", " a.almacen = '" & CodigoAlmacen & "' AND ") _
                        & " a.codart = '" & CodigoMercancia & "' AND " _
                        & " a.origen <> 'TRF' AND " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' " _
                        & " GROUP BY a.codart ) b " _
                        & " SET " _
                        & " a.entradas = b.entradas, a.fecultcosto = b.fechaultimocosto, a.ultimoproveedor = b.ultimoproveedor, a.montoultimacompra = b.ultimocosto, " _
                        & " a.acu_cos = b.costos, a.acu_cos_des = b.costosdescuentos, " _
                        & " a.costo_prom = b.costos / if( b.entradas > 0, b.entradas, 1  ), " _
                        & " a.costo_prom_des = b.costosdescuentos / if( b.entradas > 0, b.entradas, 1  ), " _
                        & " a.salidas = b.salidas, a.fecultventa = b.fechaultimaventa, a.ultimocliente = b.ultimocliente, a.montoultimaventa = b.ultimaventa, " _
                        & " a.acu_pre = b.ventas, a.acu_pre_des = b.ventasdescuentos, " _
                        & " a.venta_prom = b.ventas / if( b.salidas > 0, b.salidas, 1  ), " _
                        & " a.venta_prom_des = b.ventasdescuentos / if( b.salidas > 0, b.salidas, 1  ) " _
                        & " WHERE " _
                        & " a.codart = b.codart AND " _
                        & " a.id_emp = b.id_emp ")

        ft.Ejecutar_strSQL(MyConn, " update jsmerctainv set montoultimacompra = " & UltimoCostoAFecha(MyConn, CodigoMercancia, jytsistema.sFechadeTrabajo) & " where codart = '" & CodigoMercancia & "' and id_emp = '" & jytsistema.WorkID & "' ")

        ExistenciasEnAlmacenes(MyConn, CodigoMercancia, CodigoAlmacen)

        If Origen <> "PVE" Then ActualizaCostosEnCombo(MyConn, lblInfo, CodigoMercancia)

    End Sub
    'Public Sub ActualizarExistenciasXX(ByVal MyConn As MySqlConnection, ByVal CodigoMercancia As String, ByVal lblInfo As Label)

    '    If Mid(CodigoMercancia, 1, 1) <> "$" Then
    '        ft.Ejecutar_strSQL(myconn, " UPDATE jsmerctainv a, (SELECT a.codart, a.almacen, c.unidad,  " _
    '                    & " ROUND( SUM(IF( a.tipomov IN ('EN', 'AE', 'DV') ,  a.cantidad, IF( a.tipomov IN ('SA', 'AS', 'DC') ,  -1*a.cantidad, 0 ) ) *(IF( ISNULL(b.uvalencia), 1, 1/b.equivale  ))), 3) Existencia, " _
    '                    & " ROUND( SUM(IF( a.tipomov IN ('EN', 'AE', 'DV') ,  a.cantidad, 0) *(IF( ISNULL(b.uvalencia), 1, 1/b.equivale  ))), 3) Entradas, " _
    '                    & " ROUND( SUM(IF( a.tipomov IN ('SA', 'AS', 'DC') ,  a.cantidad, 0) *(IF( ISNULL(b.uvalencia), 1, 1/b.equivale  ))), 3) Salidas, " _
    '                    & " ROUND( SUM(IF( a.tipomov IN ('EN', 'AE', 'DV', 'AC'),  a.costotal , 0 )), 2 ) Costos, " _
    '                    & " ROUND( SUM(IF( a.tipomov IN ('EN', 'AE', 'DV', 'AC'),  a.costotaldes, 0)) , 2) CostosDescuentos,   " _
    '                    & " ROUND( SUM(IF( a.tipomov IN ('SA', 'AS', 'DC', 'AP'),  a.ventotal, 0 )), 2 ) Ventas,   " _
    '                    & " ROUND( SUM(IF( a.tipomov IN ('SA', 'AS', 'DC', 'AP'),  a.ventotaldes, 0 )) , 2) VentasDescuentos,  " _
    '                    & " if( d.fechamov is null, now(), d.fechamov) fechaultimocosto, IF( d.ultimocosto IS NULL, 0.00, d.ultimocosto) ultimocosto, if( d.ultimoproveedor is null, '', d.ultimoproveedor) ultimoproveedor, " _
    '                    & " if( e.fechamov is null, now(), e.fechamov) fechaultimaventa, if( e.ultimaventa is null, 0.00, e.ultimaventa) ultimaventa, if( e.ultimocliente is null, '', e.ultimocliente) ultimocliente, a.id_emp " _
    '                    & " FROM jsmertramer a  " _
    '                    & " LEFT JOIN jsmerequmer b ON (a.codart = b.codart AND a.unidad = b.uvalencia AND a.id_emp = b.id_emp)  " _
    '                    & " LEFT JOIN jsmerctainv c ON (a.codart = c.codart AND a.id_emp = c.id_emp)  " _
    '                    & " LEFT JOIN (SELECT a.codart, DATE_FORMAT(MAX(a.fechamov),'%Y-%m-%d') fechamov, ROUND(a.costotaldes / SUM(a.cantidad* IF( ISNULL(b.uvalencia), 1, 1/ b.equivale) ),2) ultimocosto, a.prov_cli ultimoproveedor, a.id_emp FROM jsmertramer a LEFT JOIN jsmerequmer b ON (a.codart = b.codart AND a.unidad = b.uvalencia AND a.id_emp = b.id_emp) WHERE a.codart = '" & CodigoMercancia & "' AND a.tipomov IN ('EN', 'AC', 'AE') AND a.origen IN ('COM','NCC','INV') AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.numorg ORDER BY a.FECHAMOV DESC LIMIT 1) d ON (a.codart = d.codart AND a.id_emp = d.id_emp) " _
    '                    & " LEFT JOIN (SELECT a.codart, DATE_FORMAT(MAX(a.fechamov),'%Y-%m-%d') fechamov, ROUND(a.ventotaldes / SUM(a.cantidad* IF( ISNULL(b.uvalencia), 1, 1/ b.equivale) ),2) ultimaventa, a.prov_cli ultimocliente, a.id_emp FROM jsmertramer a LEFT JOIN jsmerequmer b ON (a.codart = b.codart AND a.unidad = b.uvalencia AND a.id_emp = b.id_emp) WHERE a.codart = '" & CodigoMercancia & "' AND a.tipomov IN ('SA', 'AS') AND a.origen IN ('FAC','PFC', 'PVE', 'INV') AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.numorg ORDER BY a.FECHAMOV DESC LIMIT 1) e ON (a.codart = e.codart AND a.id_emp = e.id_emp)  " _
    '                    & " WHERE " _
    '                    & " a.codart = '" & CodigoMercancia & "' AND " _
    '                    & " a.origen <> 'TRF' AND " _
    '                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
    '                    & " GROUP BY a.codart ) b " _
    '                    & " SET a.existe_act = b.existencia, " _
    '                    & " a.entradas = b.entradas, a.fecultcosto = b.fechaultimocosto, a.ultimoproveedor = b.ultimoproveedor, a.montoultimacompra = b.ultimocosto, " _
    '                    & " a.acu_cos = b.costos, a.acu_cos_des = b.costosdescuentos, " _
    '                    & " a.costo_prom = b.costos / if( b.entradas > 0, b.entradas, 1  ), " _
    '                    & " a.costo_prom_des = b.costosdescuentos / if( b.entradas > 0, b.entradas, 1  ), " _
    '                    & " a.salidas = b.salidas, a.fecultventa = b.fechaultimaventa, a.ultimocliente = b.ultimocliente, a.montoultimaventa = b.ultimaventa, " _
    '                    & " a.acu_pre = b.ventas, a.acu_pre_des = b.ventasdescuentos, " _
    '                    & " a.venta_prom = b.ventas / if( b.salidas > 0, b.salidas, 1  ), " _
    '                    & " a.venta_prom_des = b.ventasdescuentos / if( b.salidas > 0, b.salidas, 1  ) " _
    '                    & " WHERE " _
    '                    & " a.codart = b.codart AND " _
    '                    & " a.id_emp = b.id_emp ")

    '        ft.Ejecutar_strSQL(MyConn, " update jsmerctainv set montoultimacompra = " & UltimoCostoAFecha(MyConn, CodigoMercancia, jytsistema.sFechadeTrabajo) & " where codart = '" & CodigoMercancia & "' and id_emp = '" & jytsistema.WorkID & "' ")

    '        ActualizaExistenciasEnAlmacen(MyConn, CodigoMercancia, lblInfo)

    '        ActualizaCostosEnCombo(MyConn, lblInfo, CodigoMercancia)

    '    End If

    'End Sub
    Public Sub ActualizaCostosEnCombo(MyConn As MySqlConnection, lblInfo As Label, CodigoMercancia As String)

        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim nTabla As String = "tblcostosencombos"

        ds = DataSetRequery(ds, " select * from jsmercatcom where codartcom = '" & CodigoMercancia & "' and id_emp = '' ", MyConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        For Each nRow As DataRow In dt.Rows
            ft.Ejecutar_strSQL(MyConn, " update jsmerctainv set montoultimacompra =  " & ft.DevuelveScalarDoble(MyConn, " SELECT SUM(b.montoultimacompra) " _
                                                            & " FROM jsmercatcom a " _
                                                            & " LEFT JOIN jsmerctainv b ON (a.codartcom = b.codart AND a.id_emp = b.id_emp) " _
                                                            & " WHERE " _
                                                            & " a.codart = '" & nRow.Item("codart") & "' AND " _
                                                            & " a.id_emp = '" & jytsistema.WorkID & "'group by a.id_emp ") _
                                                            & " where " _
                                                            & " codart = '" & nRow.Item("codart") & "' and " _
                                                            & " id_emp = '" & jytsistema.WorkID & "' ")
        Next


        dt.Dispose()
        dt = Nothing
        ds.Dispose()
        ds = Nothing


    End Sub

    Public Function OtorgaDescuentoOferta(ByVal MyConn As MySqlConnection, ByVal Fecha As Date, _
        ByVal CodigoMercancia As String, ByVal UnidadVenta As String, ByVal Cantidad As Double, ByVal Tarifa As String, _
        ByVal lblInfo As Label) As String

        Dim ds As New DataSet
        Dim dt As DataTable
        Dim nTabla As String = "tblOtorgaOferta"

        ds = DataSetRequery(ds, strDescuentoOferta(Fecha, CodigoMercancia, UnidadVenta, Cantidad, Tarifa), MyConn, nTabla, lblInfo)

        OtorgaDescuentoOferta = ""
        dt = ds.Tables(nTabla)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0).Item("otorgapor") = 0 Then
                OtorgaDescuentoOferta = "Dscto. otorgado x Datum"
            Else
                OtorgaDescuentoOferta = "Dscto. otorgado x A.C."
            End If
        End If

        dt = Nothing
        ds = Nothing

    End Function
    Public Function PorcentajeDescuentoOferta(ByVal MyConn As MySqlConnection, ByVal Fecha As Date, _
        ByVal CodigoMercancia As String, ByVal UnidadVenta As String, ByVal Cantidad As Double, _
        ByVal Tarifa As String, ByVal lblInfo As Label) As Double

        Dim ds As New DataSet
        Dim dt As DataTable
        Dim nTabla As String = "tblDescuentoOferta"

        ds = DataSetRequery(ds, strDescuentoOferta(Fecha, CodigoMercancia, UnidadVenta, Cantidad, Tarifa), MyConn, nTabla, lblInfo)

        PorcentajeDescuentoOferta = 0.0
        dt = ds.Tables(nTabla)
        If dt.Rows.Count > 0 Then PorcentajeDescuentoOferta = dt.Rows(0).Item("porcentaje")

        dt = Nothing
        ds = Nothing

    End Function
    Private Function strDescuentoOferta(ByVal Fecha As Date, _
        ByVal CodigoMercancia As String, ByVal UnidadVenta As String, ByVal Cantidad As Double, ByVal Tarifa As String) As String

        strDescuentoOferta = " select a.codofe, b.porcentaje, b.otorgapor from jsmerencofe a, jsmerrenofe b " _
            & " WHERE " _
            & " a.CODOFE = b.CODOFE AND " _
            & " a.id_emp = b.id_emp and " _
            & " a.DESDE <= '" & ft.FormatoFechaMySQL(Fecha) & "' AND " _
            & " a.HASTA >= '" & ft.FormatoFechaMySQL(Fecha) & "' AND " _
            & " b.CODART = '" & CodigoMercancia & "' AND " _
            & " b.UNIDAD = '" & UnidadVenta & "' AND " _
            & " b.LIMITEI <= " & Cantidad & " AND " _
            & " b.LIMITES >= " & Cantidad & " and " _
            & " a.TARIFA_" & Tarifa & "  = '1' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
            & " ORDER BY DESDE DESC LIMIT 1"

    End Function

    Public Function UltimoCostoAFecha(ByVal MyConn As MySqlConnection, ByVal CodigoMercancia As String, _
                                      ByVal Fecha As Date, _
                                      Optional Unidad As String = "") As Double

        If Mid(CodigoMercancia, 1, 1) <> "$" Then

            UltimoCostoAFecha = ft.DevuelveScalarDoble(MyConn, " select ROUND( SUM(a.costotaldes)/if( SUM(IF (ISNULL(UVALENCIA), a.CANTIDAD, a.cantidad/b.equivale)) > 0, SUM(IF (ISNULL(UVALENCIA), a.CANTIDAD, a.cantidad/b.equivale)) ,1),2) " _
                & " FROM jsmertramer a " _
                & " LEFT JOIN jsmerequmer b ON ( a.codart = b.CODART AND a.unidad = b.UVALENCIA AND a.ID_EMP = b.ID_EMP ) " _
                & " WHERE " _
                & " a.codart = '" & CodigoMercancia & "' AND " _
                & " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(Fecha) & "' AND " _
                & " a.tipomov IN ('EN', 'AC', 'AE') AND " _
                & " a.origen IN ('COM', 'INV') AND " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                & " GROUP BY a.codart, a.numdoc " _
                & " ORDER BY a.fechamov DESC, a.origen LIMIT 1")

            If Unidad <> "" Then UltimoCostoAFecha = UltimoCostoAFecha / Equivalencia(MyConn, CodigoMercancia, Unidad)

        End If

    End Function

    Public Function UltimoCostoMenorAFecha(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoMercancia As String, ByVal Fecha As Date, nCosto As Double) As Double

        UltimoCostoMenorAFecha = 0.0
        If Mid(CodigoMercancia, 1, 1) <> "$" Then
            Dim aArr As New ArrayList
            aArr = ft.Ejecutar_strSQL_DevuelveLista(MyConn, " select ROUND(SUM(a.costotaldes)/if( SUM(IF (ISNULL(UVALENCIA), a.CANTIDAD, a.cantidad/b.equivale)) > 0, SUM(IF (ISNULL(UVALENCIA), a.CANTIDAD, a.cantidad/b.equivale)) ,1) ,2) costotal " _
                & " FROM jsmertramer a LEFT JOIN jsmerequmer b ON ( a.codart = b.CODART AND a.unidad = b.UVALENCIA AND a.ID_EMP = b.ID_EMP ) WHERE " _
                & " a.codart = '" & CodigoMercancia & "' AND " _
                & " a.fechamov <= '" & ft.FormatoFechaHoraMySQL(Fecha) & "' AND " _
                & " a.tipomov IN ('EN', 'AC', 'AE') AND " _
                & " a.origen IN ('COM', 'INV') AND " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                & " GROUP BY a.codart, a.numdoc " _
                & " ORDER BY a.fechamov DESC, a.origen ")

            Dim iCont As Integer = 0
            Dim bVerdad As Boolean = True
            While bVerdad
                If iCont > aArr.Count - 1 Or nCosto > aArr(iCont) Then
                    If nCosto > aArr(iCont) Then UltimoCostoMenorAFecha = aArr(iCont)
                    bVerdad = False
                End If
                iCont += 1
            End While

        End If

    End Function



    Public Function UltimoProveedor(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoMercancia As String, ByVal Fecha As Date) As String

        Return ft.DevuelveScalarCadena(MyConn, " select prov_cli " _
               & " FROM jsmertramer a " _
               & " WHERE " _
               & " a.codart = '" & CodigoMercancia & "' AND " _
               & " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(Fecha) & "' AND " _
               & " a.tipomov IN ('EN') AND " _
               & " a.origen IN ('COM') AND " _
               & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
               & " GROUP BY a.codart, a.numdoc " _
               & " ORDER BY a.fechamov DESC, a.origen LIMIT 1")

    End Function

    Public Function UltimaFechaCompra(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoMercancia As String, ByVal Fecha As Date) As Date

        Dim ultimafechaDeCompra As Date = ft.DevuelveScalarFecha(MyConn, " select fechamov " _
               & " FROM jsmertramer a " _
               & " WHERE " _
               & " a.codart = '" & CodigoMercancia & "' AND " _
               & " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(Fecha) & "' AND " _
               & " a.tipomov IN ('EN') AND " _
               & " a.origen IN ('COM') AND " _
               & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
               & " GROUP BY a.codart, a.numdoc " _
               & " ORDER BY a.fechamov DESC, a.origen LIMIT 1")

        If ultimafechaDeCompra = Nothing Then
            Return ft.DevuelveScalarFecha(MyConn, " select creacion " _
               & " FROM jsmerctainv a " _
               & " WHERE " _
               & " a.codart = '" & CodigoMercancia & "' AND " _
               & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
               & " ")
        Else
            Return CDate(ultimafechaDeCompra)
        End If


    End Function
    '//// Datum3
    Public Function Equivalencia(ByVal MyConn As MySqlConnection, ByVal CodigoMercancia As String, ByVal Unidad As String) As Double
        Dim Equivale As Double = ft.DevuelveScalarDoble(MyConn, "SELECT equivale FROM jsmerequmer where CODART = '" & CodigoMercancia & "' AND UVALENCIA = '" & Unidad & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
        If Equivale = 0.0 Then Equivale = 1.0
        Equivalencia = Equivale
    End Function

    Public Sub EliminarMovimientosdeInventario(ByVal MyConn As MySqlConnection, ByVal Documento As String, ByVal Origen As String, _
                 ByVal lblinfo As Label, Optional ByVal DocumentoOrigen As String = "", Optional ByVal Tipo As String = "", Optional ByVal ProveedorCliente As String = "", _
                 Optional ByVal CodigoArticulo As String = "", Optional numReglon As String = "")

        Dim str As String = ""

        If Tipo <> "" Then str += " tipomov = '" & Tipo & "' and "
        If ProveedorCliente <> "" Then str += " prov_cli = '" & ProveedorCliente & "' and "
        If DocumentoOrigen <> "" Then str += " numorg = '" & DocumentoOrigen & "' and "
        If CodigoArticulo <> "" Then str += " codart = '" & CodigoArticulo & "' and "
        If numReglon <> "" Then str += " asiento = '" & numReglon & "' and "

        ft.Ejecutar_strSQL(myconn, "delete FROM jsmertramer " _
            & " where " _
            & str _
            & " numdoc = '" & Documento & "' AND " _
            & " origen = '" & Origen & "' AND " _
            & " ejercicio = '" & jytsistema.WorkExercise & "' AND " _
            & " ID_EMP = '" & jytsistema.WorkID & "' ")

    End Sub

    Public Sub CargaListViewCombosConExistencia(ByVal LV As ListView, ByVal dt As DataTable)

        Dim iCont As Integer

        LV.Clear()

        LV.BeginUpdate()

        LV.Columns.Add("Código Combo", 100, HorizontalAlignment.Left)
        LV.Columns.Add("Nombre y/o Descripción", 340, HorizontalAlignment.Left)
        LV.Columns.Add("Existencia", 90, HorizontalAlignment.Right)
        LV.Columns.Add("Unidad", 40, HorizontalAlignment.Center)


        For iCont = 0 To dt.Rows.Count - 1

            With dt.Rows(iCont)
                LV.Items.Add(.Item("codart").ToString)
                LV.Items(iCont).SubItems.Add(.Item("nomart").ToString)
                LV.Items(iCont).SubItems.Add(ft.FormatoCantidad(CDbl(.Item("existencia").ToString)))
                LV.Items(iCont).SubItems.Add(.Item("UNIDAD").ToString)
            End With
        Next

        LV.EndUpdate()

    End Sub

    Function ConversiondeUnidades(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, _
                                   ByVal CodigoArticulo As String, ByVal UnidadOrigen As String, ByVal CantidadOrigen As Double, ByVal UnidadDestino As String) As Double

        Dim dtLocal As DataTable
        Dim nTablaLocal As String = "tblLocal"

        ds = DataSetRequery(ds, " select UNIDAD from jsmerctainv where CODART = '" & CodigoArticulo _
                             & "' AND ID_EMP = '" & jytsistema.WorkID & "'", MyConn, nTablaLocal, lblInfo)

        dtLocal = ds.Tables(nTablaLocal)

        If dtLocal.Rows.Count > 0 Then
            With dtLocal.Rows(0)
                If UnidadOrigen = .Item("UNIDAD") Then '
                    ConversiondeUnidades = CantidadDesquivalente(MyConn, lblInfo, CodigoArticulo, UnidadDestino, CantidadOrigen)
                ElseIf UnidadDestino = .Item("UNIDAD") Then
                    ConversiondeUnidades = CantidadEquivalente(MyConn, lblInfo, CodigoArticulo, UnidadOrigen, CantidadOrigen)
                ElseIf UnidadOrigen = UnidadDestino Then
                    ConversiondeUnidades = CantidadOrigen
                Else
                    ConversiondeUnidades = CantidadDesquivalente(MyConn, lblInfo, CodigoArticulo, UnidadDestino, CantidadOrigen)
                    ConversiondeUnidades = CantidadEquivalente(MyConn, lblInfo, CodigoArticulo, UnidadOrigen, ConversiondeUnidades)
                End If
            End With

        End If

        dtLocal.Dispose()
        dtLocal = Nothing

    End Function
    Function CantidadEquivalente(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                  ByVal sArticulo As String, ByVal sUnidad As String, ByVal sCantidad As Double) As Double
        Dim lEquivalencia As Double
        lEquivalencia = Equivalencia(myConn, sArticulo, sUnidad)
        If lEquivalencia > 1 Then
            CantidadEquivalente = sCantidad / lEquivalencia
        ElseIf lEquivalencia > 0 And lEquivalencia <= 1 Then
            CantidadEquivalente = sCantidad * lEquivalencia
        End If
    End Function
    Function CantidadDesquivalente(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                    ByVal sArticulo As String, ByVal sUnidad As String, ByVal sCantidad As Double) As Double
        Dim lEquivalencia As Double
        lEquivalencia = Equivalencia(myConn, sArticulo, sUnidad)
        If lEquivalencia > 1 Then
            CantidadDesquivalente = sCantidad * lEquivalencia
        ElseIf lEquivalencia > 0 And lEquivalencia <= 1 Then
            CantidadDesquivalente = sCantidad / lEquivalencia
        End If
    End Function
    Function MercanciaPoseeEquivalencia(MyConn As MySqlConnection, lblInfo As Label, CodigoArticulo As String, UnidadAVerificar As String) As Boolean

        Dim nInt As Integer = ft.DevuelveScalarEntero(MyConn, " SELECT COUNT(*) FROM (SELECT unidad FROM jsmerctainv " _
                                                           & " WHERE " _
                                                           & " codart = '" & CodigoArticulo & "' and " _
                                                           & " id_emp = '" & jytsistema.WorkID & "' " _
                                                           & " UNION " _
                                                           & " Select a.uvalencia " _
                                                           & " FROM jsmerequmer a " _
                                                           & " LEFT JOIN jsmerctainv b ON (a.codart = b.codart AND a.unidad = b.unidad AND a.id_emp = b.id_emp) " _
                                                           & " WHERE " _
                                                           & " a.codart = '" & CodigoArticulo & "') a " _
                                                           & " WHERE a.unidad = '" & UnidadAVerificar & "' ")
        If nInt = 0 Then
            Return False
        Else
            Return True
        End If


    End Function

    Function CantidadVentasEnElMes_KGR(MyConn As MySqlConnection, CodigoAsesor As String, CodigoMercancia As String, _
                                                 FechaMovimiento As Date) As Double
        Return ft.DevuelveScalarDoble(MyConn, " SELECT SUM( IF( origen = 'NCV', -1,1) * peso) " _
                                                 & " FROM jsmertramer a " _
                                                 & " WHERE " _
                                                 & " YEAR(fechamov) = " & FechaMovimiento.Year & " AND " _
                                                 & " MONTH(fechamov) = " & FechaMovimiento.Month & " AND " _
                                                 & " origen IN ('FAC', 'PVE', 'PFC', 'NDV', 'NCV') AND " _
                                                 & " vendedor = '" & CodigoAsesor & "' AND " _
                                                 & " codart = '" & CodigoMercancia & "' AND " _
                                                 & " id_emp = '" & jytsistema.WorkID & "' group by vendedor ")
    End Function

    Public Sub ActualizarRenglonesEnPedidosAlmacen(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, ByVal dtRenglon As DataTable,
                                                ByVal nombreTablaOrigen As String)

        Dim NumeroOrdenDeCompra As String
        Dim sItem As String
        Dim RenglonOrdenDeCompra As String
        Dim gCont As Integer

        For gCont = 0 To dtRenglon.Rows.Count - 1
            With dtRenglon.Rows(gCont)
                NumeroOrdenDeCompra = ""
                RenglonOrdenDeCompra = ""
                'NumeroOrdenDeCompra = IIf(Not IsDBNull(.Item("numped")), .Item("numped"), "")
                'RenglonOrdenDeCompra = IIf(Not IsDBNull(.Item("renped")), .Item("renped"), "")
                sItem = .Item("item")
                If NumeroOrdenDeCompra <> "" Then
                    If .Item("ESTATUS") < 2 Then
                        ActualizaCantidadTransitoEnRenglon(MyConn, lblInfo, ds, "jsmerrenped", "numped", NumeroOrdenDeCompra,
                                                           RenglonOrdenDeCompra, sItem, .Item("CANTIDAD"),
                                                           .Item("UNIDAD"), .Item("ESTATUS"))

                        ActualizaEstatusDocumento(MyConn, lblInfo, ds, "jsmerencped", "jsmerrenped", "numped", NumeroOrdenDeCompra)
                    End If
                End If
            End With
        Next

    End Sub




End Module
