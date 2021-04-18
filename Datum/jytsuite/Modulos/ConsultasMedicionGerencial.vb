Imports MySql.Data.MySqlClient
Module ConsultasMedicionGerencial
    Private ft As New Transportables
    Function SeleccionSIGMEGananciasBrutasFacturas(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                            ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, ByVal OrdenadoPor As String, _
                                            Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                            Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                            Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                            Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                            Optional ByVal Estatus As Integer = 9, Optional ByVal Cartera As Integer = 9, _
                                            Optional ByVal SoloPeso As Boolean = False, Optional ByVal TipoMercancia As Integer = 99, _
                                            Optional ByVal Regulada As Integer = 9, _
                                            Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                            Optional ByVal TiponegocioDesde As String = "", Optional ByVal TipoNegocioHasta As String = "", _
                                            Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                            Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                            Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                            Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                            Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                            Optional ByVal TipoContribuyente As Integer = 0, Optional ByVal EstatusCliente As Integer = 0, _
                                            Optional ByVal EmisionDesde As Date = MyDate, Optional ByVal EmisionHasta As Date = MyDate, _
                                            Optional ByVal AlmacenDesde As String = "", Optional ByVal AlmacenHasta As String = "", _
                                            Optional ByVal ClienteDesde As String = "", Optional ByVal ClienteHasta As String = "", _
                                            Optional ByVal Resumido As Boolean = False, Optional ByVal siMeses As Boolean = False) As String

        Dim strSQLPie As String

        Dim tblGanancias As String

        Dim strSQLWhere As String = CadenaComplementoClientes("b", ClienteDesde, ClienteHasta, 0, OrdenadoPor, _
                                                      CanalDesde, CanalHasta, TiponegocioDesde, TipoNegocioHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, Parroquia, Ciudad, Barrio)


        strSQLPie = " group by a.numdoc, a.codart " _
                & " ORDER BY a.numdoc, a.codart "

        tblGanancias = "tbl" & ft.NumeroAleatorio(10000)

        Dim aFld() As String = {"prov_cli.cadena.15.0", "vendedor.cadena.5.0", "NUMDOC.cadena.30.0", "fechamov.fechahora.0.0", "codart.cadena.15.0", _
                                "unidad.cadena.3.0", "signo.entero.4.0", "cantidad.doble.10.3", "equivale.doble.10.3", "costotal.doble.19.2", _
                                "costotaldes.doble.19.2", "ventotal.doble.19.2", "ventotaldes.doble.19.2", "id_emp.cadena.2.0"}

        CrearTabla(MyConn, lblInfo, jytsistema.WorkDataBase, True, tblGanancias, aFld)

        If CategoriaDesde <> "" Then strSQLWhere += " c.grupo >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then strSQLWhere += " c.grupo <= '" & CategoriaHasta & "' and "
        If MarcaDesde <> "" Then strSQLWhere = " c.MARCA  >= '" & MarcaDesde & "' and  "
        If MarcaHasta <> "" Then strSQLWhere = " c.MARCA <= '" & MarcaHasta & "' and "
        If DivisionDesde <> "" Then strSQLWhere = " c.division >= '" & DivisionDesde & "' and "
        If DivisionHasta <> "" Then strSQLWhere = " c.division <= '" & DivisionHasta & "' and "
        If TipoJerarquia <> "" Then strSQLWhere = " c.tipjer = '" & TipoJerarquia & "' and "

        If AsesorDesde <> "" Then strSQLWhere = " a.VENDEDOR >= '" & AsesorDesde & "' AND "
        If AsesorHasta <> "" Then strSQLWhere = " a.VENDEDOR <= '" & AsesorHasta & "' AND "


        ft.Ejecutar_strSQL(myconn, " insert into " & tblGanancias & " select a.prov_cli, a.vendedor, a.numdoc, a.fechamov, a.codart, a.unidad, if( tipomov in ('EN','AE'), -1, 1) signo,  sum(a.cantidad) as cantidad, " _
            & " if( isnull(b.equivale),1, b.equivale) as equivale,  sum(a.costotal) as costotal, " _
            & " sum(a.costotaldes) as costotaldes, sum(a.ventotal) as ventotal, sum(ventotaldes) as ventotaldes, a.id_emp " _
            & " from jsmertramer a " _
            & " left join jsmerequmer b on (a.codart = b.codart and a.unidad = b.uvalencia and a.id_emp = b.id_emp) " _
            & " Where " _
            & " a.origen in ('FAC','PFC','PVE','NDV','NCV','INV') and " _
            & " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(EmisionDesde) & "' AND " _
            & " a.FECHAMOV <= '" & ft.FormatoFechaHoraMySQLFinal(EmisionHasta) & "' and " _
            & IIf(MercanciaDesde <> "", " a.numdoc >= '" & MercanciaDesde & "' AND ", "") _
            & IIf(MercanciaHasta <> "", " a.numdoc <= '" & MercanciaHasta & "' AND ", "") _
            & " a.id_emp  = '" & jytsistema.WorkID & "'  " _
            & " group By " _
            & " a.prov_cli, a.vendedor, a.codart, a.unidad, signo ")

        SeleccionSIGMEGananciasBrutasFacturas = " SELECT a.numdoc, a.fechamov, a.codart, c.nomart, c.unidad,  " _
            & " sum(a.signo*a.cantidad/a.equivale) cantidadreal, " _
            & " Sum(a.signo*a.costotaldes) costotaldes, " _
            & " Sum(a.signo*a.ventotaldes) ventotaldes, " _
            & " (sum(a.signo*a.ventotaldes) - sum(a.signo*a.costotaldes)) ganancia, " _
            & " (1 - sum(a.signo*a.costotaldes) / sum(a.signo*a.ventotaldes))*100 porgan, " _
            & " a.vendedor, concat(e.codven, ' ', e.apellidos,' ',e.nombres) asesor, " _
            & " b.codcli, b.nombre nomcli, b.rif, f.descrip canal, g.descrip tiponegocio, h.descrip zona, i.nomrut ruta, " _
            & " if( q.nombre is null, '', q.nombre) FBARRIO, if( r.nombre is null, '', r.nombre) FCIUDAD, if( s.nombre is null, '', s.nombre) FPARROQUIA, " _
            & " if( t.nombre is null, '', t.nombre) FMUNICIPIO, if(u.nombre is null, '', u.nombre) FESTADO, if(v.nombre is null, '', v.nombre) FPAIS, " _
            & " j.descrip categoria, k.descrip marca, l.descrip tipjer, p.descrip division, elt(c.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') as mix " _
            & " from " & tblGanancias & " a " _
            & " left join jsvencatcli b on (a.prov_cli = b.codcli and b.id_emp = '" & jytsistema.WorkID & "' ) " _
            & " left join jsmerctainv c on (a.CODART = c.CODART AND c.id_emp = '" & jytsistema.WorkID & "') " _
            & " left join jsvencatven e on (a.vendedor=e.codven and e.id_emp = '" & jytsistema.WorkID & "' ) " _
            & " left join jsvenliscan f on (b.categoria = f.codigo and b.id_emp = f.id_emp ) " _
            & " left join jsvenlistip g on (b.unidad = g.codigo and b.id_emp = g.id_emp ) " _
            & " left join jsconctatab h on (b.zona = h.codigo and b.id_emp = h.id_emp  and h.modulo = '00005') " _
            & " left join jsvenencrut i on (b.ruta_visita = i.codrut and b.id_emp = i.id_emp and i.tipo = '0' ) " _
            & " left join jsconctatab j on (c.grupo = j.codigo and c.id_emp = j.id_emp and j.modulo = '00002') " _
            & " left join jsconctatab k on (c.marca = k.codigo and c.id_emp = k.id_emp and k.modulo = '00003') " _
            & " left join jsmerencjer l on (c.tipjer = l.tipjer and c.id_emp = l.id_emp) " _
            & " left join jsconcatter q on (b.fbarrio = q.codigo and b.id_emp = q.id_emp ) " _
            & " left join jsconcatter r on (b.fciudad = r.codigo and b.id_emp = r.id_emp ) " _
            & " left join jsconcatter s on (b.fparroquia = s.codigo and b.id_emp = s.id_emp ) " _
            & " left join jsconcatter t on (b.fmunicipio = t.codigo and b.id_emp = t.id_emp ) " _
            & " left join jsconcatter u on (b.festado = u.codigo and b.id_emp = u.id_emp ) " _
            & " left join jsconcatter v on (b.fpais = v.codigo and b.id_emp = v.id_emp ) " _
            & " left join jsmercatdiv p on (c.division = p.division and c.id_emp = p.id_emp) " _
             & " where  " _
            & strSQLWhere & " a.id_emp = '" & jytsistema.WorkID & "' " & strSQLPie

    End Function


    Function SeleccionSIGMEGananciasBrutasItem(ByVal myConn As MySqlConnection, ByVal lblInfo As Label, _
                                               ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, _
                                               ByVal FechaDesde As Date, ByVal FechaHasta As Date, Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                               Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                               Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                               Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                               Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                               Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                               Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                               Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                               Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                               Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                               Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                               Optional ByVal MercanciasPVE As Boolean = False) As String

        Dim strSQLFiltro As String = ""

        Dim tblGanancias As String

        Dim strSQLWhere As String = CadenaComplementoClientes("b", "", "", 0, "", _
                                                     CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                     ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, Parroquia, Ciudad, Barrio)


        If MercanciaDesde <> "" Then strSQLWhere += " a.codart >= '" & MercanciaDesde & "' AND "
        If MercanciaHasta <> "" Then strSQLWhere += " a.codart <= '" & MercanciaHasta & "' AND "

        tblGanancias = "tbl" & ft.NumeroAleatorio(10000)
        Dim aFld() As String = {"prov_cli.cadena.15.0", "vendedor.cadena.5.0", "codart.cadena.15.0", _
                               "unidad.cadena.3.0", "signo.entero.4.0", "cantidad.doble.10.3", "equivale.doble.10.3", "costotal.doble.19.2", _
                               "costotaldes.doble.19.2", "ventotal.doble.19.2", "ventotaldes.doble.19.2", "id_emp.cadena.2.0"}

        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblGanancias, aFld)


        If CategoriaDesde <> "" Then strSQLWhere += " c.grupo >= '" & CategoriaDesde & "' AND "
        If CategoriaHasta <> "" Then strSQLWhere += " c.grupo <= '" & CategoriaHasta & "' AND "
        If MarcaDesde <> "" Then strSQLWhere += " c.MARCA >= '" & MarcaDesde & "' AND "
        If MarcaHasta <> "" Then strSQLWhere += " c.MARCA <= '" & MarcaHasta & "' AND "
        If DivisionDesde <> "" Then strSQLWhere += " c.division >= '" & DivisionDesde & "' AND "
        If DivisionHasta <> "" Then strSQLWhere += " c.division <= '" & DivisionHasta & "' AND "

        If TipoJerarquia <> "" Then strSQLWhere = " c.tipjer = '" & TipoJerarquia & "' AND "

        Dim strOrigen As String = ""
        If AsesorDesde <> "" Then strOrigen = " a.vendedor >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then strOrigen = strOrigen & " a.vendedor <= '" & AsesorHasta & "' and "
        If MercanciasPVE Then
            strOrigen = " a.origen = 'PVE' and "
        Else
            strOrigen = " a.origen in ('FAC','PFC','PVE','NDV','NCV') and "
        End If

        ft.Ejecutar_strSQL(myconn, " insert into " & tblGanancias & " select a.prov_cli, a.vendedor, a.codart, a.unidad, if( tipomov in ('SA', 'AS') , 1,-1) as signo,  sum(a.cantidad) as cantidad, " _
            & " if( isnull(b.equivale),1, b.equivale) as equivale,  sum(a.costotal) as costotal, " _
            & " sum(a.costotaldes) as costotaldes, sum(a.ventotal) as ventotal, sum(ventotaldes) as ventotaldes, a.id_emp " _
            & " from jsmertramer a " _
            & " left join jsmerequmer b on (a.codart = b.codart and a.unidad = b.uvalencia and a.id_emp = b.id_emp) " _
            & " Where " _
            & strOrigen _
            & " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaDesde) & "' AND " _
            & " a.FECHAMOV <= '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' and " _
            & " a.id_emp  = '" & jytsistema.WorkID & "'  " _
            & " group By " _
            & " a.prov_cli, a.vendedor, a.codart, a.unidad, signo ")

        SeleccionSIGMEGananciasBrutasItem = " SELECT a.codart, c.nomart, c.unidad, " _
            & " sum(a.signo*a.cantidad/a.equivale) as cantidadreal, " _
            & " Sum(a.signo*a.costotaldes) costotaldes, " _
            & " Sum(a.signo*a.ventotaldes) ventotaldes, " _
            & " (sum(a.signo*a.ventotaldes) - sum(a.signo*a.costotaldes)) ganancia, " _
            & " if( sum(a.signo*a.ventotaldes) > 0 and (sum(a.signo*a.ventotaldes) - sum(a.signo*a.costotaldes)) > 0, (1 - sum(a.signo*a.costotaldes) / sum(a.signo*a.ventotaldes))*100, 0.00)  porgan, " _
            & " a.vendedor, concat(e.codven,' ',e.apellidos,', ',e.nombres) asesor, " _
            & " b.codcli, b.NOMBRE nomcli, b.rif, f.descrip as canal, g.descrip tiponegocio, h.descrip zona, i.nomrut ruta, " _
            & " if( q.nombre is null, '', q.nombre) FBARRIO, if( r.nombre is null, '', r.nombre) FCIUDAD, if( s.nombre is null, '', s.nombre) FPARROQUIA, " _
            & " if( t.nombre is null, '', t.nombre) FMUNICIPIO, if(u.nombre is null, '', u.nombre) FESTADO, if(v.nombre is null, '', v.nombre) FPAIS, " _
            & " j.descrip categoria, k.descrip marca, l.descrip tipjer, p.descrip division, elt(c.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') as mix " _
            & " from " & tblGanancias & " a " _
            & " left join jsvencatcli b on (a.prov_cli = b.codcli and b.id_emp = '" & jytsistema.WorkID & "' ) " _
            & " left join jsmerctainv c on (a.CODART = c.CODART AND c.id_emp = '" & jytsistema.WorkID & "') " _
            & " left join jsvencatven e on (a.vendedor=e.codven and e.id_emp = '" & jytsistema.WorkID & "' ) " _
            & " left join jsvenliscan f on (b.categoria = f.codigo and b.id_emp = f.id_emp ) " _
            & " left join jsvenlistip g on (b.unidad = g.codigo and b.id_emp = g.id_emp ) " _
            & " left join jsconctatab h on (b.zona = h.codigo and b.id_emp = h.id_emp  and h.modulo = '00005') " _
            & " left join jsvenencrut i on (b.ruta_visita = i.codrut and b.id_emp = i.id_emp and i.tipo = '0' ) " _
            & " left join jsconctatab j on (c.grupo = j.codigo and c.id_emp = j.id_emp and j.modulo = '00002') " _
            & " left join jsconctatab k on (c.marca = k.codigo and c.id_emp = k.id_emp and k.modulo = '00003') " _
            & " left join jsmerencjer l on (c.tipjer = l.tipjer and c.id_emp = l.id_emp) " _
            & " left join jsconcatter q on (b.fbarrio = q.codigo and b.id_emp = q.id_emp ) " _
            & " left join jsconcatter r on (b.fciudad = r.codigo and b.id_emp = r.id_emp ) " _
            & " left join jsconcatter s on (b.fparroquia = s.codigo and b.id_emp = s.id_emp ) " _
            & " left join jsconcatter t on (b.fmunicipio = t.codigo and b.id_emp = t.id_emp ) " _
            & " left join jsconcatter u on (b.festado = u.codigo and b.id_emp = u.id_emp ) " _
            & " left join jsconcatter v on (b.fpais = v.codigo and b.id_emp = v.id_emp ) " _
            & " left join jsmercatdiv p on (c.division = p.division and c.id_emp = p.id_emp) " _
            & " WHERE " _
            & strSQLWhere _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
            & " group by a.codart "

        'ft.Ejecutar_strSQL ( myconn, " drop table " & tblGanancias)

    End Function



    Function SeleccionSIGMEGananciasBrutasMesMes(ByVal myConn As MySqlConnection, ByVal lblInfo As Label, _
                                    ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                    Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                    Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "",
                                    Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "",
                                    Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                    Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                    Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                    Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                    Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                    Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                    Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                    Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                    Optional ByVal AgrupadoPor As String() = Nothing) As String

        Dim st01 As String, st02 As String, st03 As String, st04 As String, st05 As String, st06 As String
        Dim st07 As String, st08 As String, st09 As String, st10 As String, st11 As String, st12 As String
        Dim strSQL As String = ""
        Dim strGrupo As String
        Dim i As Integer

        strGrupo = " a.id_emp "
        If AgrupadoPor IsNot Nothing Then
            If UBound(AgrupadoPor) <= 0 Then
            Else
                For i = 0 To UBound(AgrupadoPor) - 1
                    Select Case AgrupadoPor(i)
                        Case "categoria"
                            strGrupo = strGrupo & ", " & " c.grupo "
                        Case "marca"
                            strGrupo = strGrupo & ", " & " c.marca "
                        Case "tipjer"
                            strGrupo = strGrupo & ", " & " c.tipjer "
                        Case "codjer"

                        Case "division"
                            strGrupo = strGrupo & ", " & " c.division "
                        Case "mix"
                            strGrupo = strGrupo & ", " & " c.mix "
                        Case "canal"
                            strGrupo = strGrupo & ", " & " b.categoria "
                        Case "tiponegocio"
                            strGrupo = strGrupo & ", " & " b.unidad "
                        Case "zona"
                            strGrupo = strGrupo & ", " & " i.codzon "
                        Case "ruta"
                            strGrupo = strGrupo & ", " & " i.codrut "
                        Case "asesor"
                            strGrupo = strGrupo & ", " & " a.vendedor "
                        Case "territorio"
                            strGrupo = strGrupo & ", " & " b.fpais, b.festado, b.fmunicipio, b.fparroquia, b.fciudad, b.fbarrio "
                    End Select
                Next
            End If
        End If

        If CategoriaDesde <> "" Then strSQL += " and c.grupo >= '" & CategoriaDesde & "' "
        If CategoriaHasta <> "" Then strSQL += " and c.grupo <= '" & CategoriaHasta & "' "
        If MarcaDesde <> "" Then strSQL += " and c.MARCA  >= '" & MarcaDesde & "'"
        If MarcaHasta <> "" Then strSQL += " and  c.MARCA <= '" & MarcaHasta & "'"
        If DivisionDesde <> "" Then strSQL += " and c.division >= '" & DivisionDesde & "' "
        If DivisionHasta <> "" Then strSQL += " and c.division <= '" & DivisionHasta & "' "
        If TipoJerarquia <> "" Then strSQL += " and c.tipjer = '" & TipoJerarquia & "' "

        If CanalDesde <> "" Then strSQL += " AND b.CATEGORIA >= '" & CanalDesde & "' "
        If CanalHasta <> "" Then strSQL += " AND b.CATEGORIA <= '" & CanalHasta & "' "
        If TipoDesde <> "" Then strSQL += " AND b.UNIDAD >= '" & TipoDesde & "' "
        If TipoHasta <> "" Then strSQL += " AND b.UNIDAD <= '" & TipoHasta & "' "
        If ZonaDesde <> "" Then strSQL += " AND e.codzon >= '" & ZonaDesde & "' "
        If ZonaHasta <> "" Then strSQL += " AND e.codzon <= '" & ZonaHasta & "' "
        If RutaDesde <> "" Then strSQL += " AND e.codrut >= '" & RutaDesde & "' "
        If RutaHasta <> "" Then strSQL += " AND e.codrut <= '" & RutaHasta & "' "
        If AsesorDesde <> "" Then strSQL += " AND a.vendedor >= '" & AsesorDesde & "' "
        If AsesorHasta <> "" Then strSQL += " AND a.vendedor <= '" & AsesorHasta & "' "

        st01 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'),if( a.tipomov <> 'AP', if( month(a.fechamov) = 1, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 1, a.cantidad,0),0))), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 1, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 1, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad01, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 1, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 1, a.peso,0),0))) As PesoNeto01, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 1, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 1, a.costotaldes,0),0))) CosTotal01, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( month(a.fechamov) = 1, a.ventotaldes,0), -1*if( month(a.fechamov) = 1, a.ventotaldes,0))) VenTotal01, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 1, a.ventotaldes,0) - if( month(a.fechamov) = 1, a.costotaldes,0), if( month(a.fechamov) = 1, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 1, a.costotaldes,0)-if( month(a.fechamov) = 1, a.ventotaldes,0), -1*if( month(a.fechamov) = 1, a.ventotaldes,0)))) GANANCIA01, "

        st02 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'),if( a.tipomov <> 'AP', if( month(a.fechamov) = 2, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 2, a.cantidad,0),0))), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 2, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 2, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad02, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 2, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 2, a.peso,0),0))) As PesoNeto02, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 2, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 2, a.costotaldes,0),0))) CosTotal02, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( month(a.fechamov) = 2, a.ventotaldes,0), -1*if( month(a.fechamov) = 2, a.ventotaldes,0))) VenTotal02, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'),  if( a.tipomov <> 'AP', if( month(a.fechamov) = 2, a.ventotaldes,0) - if( month(a.fechamov) = 2, a.costotaldes,0), if( month(a.fechamov) = 2, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 2, a.costotaldes,0)-if( month(a.fechamov) = 2, a.ventotaldes,0), -1*if( month(a.fechamov) = 2, a.ventotaldes,0)))) GANANCIA02, "

        st03 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'),if( a.tipomov <> 'AP', if( month(a.fechamov) = 3, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 3, a.cantidad,0),0))), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 3, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 3, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad03, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 3, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 3, a.peso,0),0))) As PesoNeto03, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 3, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 3, a.costotaldes,0),0))) CosTotal03, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( month(a.fechamov) = 3, a.ventotaldes,0), -1*if( month(a.fechamov) = 3, a.ventotaldes,0))) VenTotal03, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'),  if( a.tipomov <> 'AP', if( month(a.fechamov) = 3, a.ventotaldes,0) - if( month(a.fechamov) = 3, a.costotaldes,0), if( month(a.fechamov) = 3, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 3, a.costotaldes,0)-if( month(a.fechamov) = 3, a.ventotaldes,0), -1*if( month(a.fechamov) = 3, a.ventotaldes,0)))) GANANCIA03, "


        st04 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'),if( a.tipomov <> 'AP', if( month(a.fechamov) = 4, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 4, a.cantidad,0),0))), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 4, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 4, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad04, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 4, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 4, a.peso,0),0))) As PesoNeto04, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 4, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 4, a.costotaldes,0),0))) CosTotal04, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( month(a.fechamov) = 4, a.ventotaldes,0), -1*if( month(a.fechamov) = 4, a.ventotaldes,0))) VenTotal04, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'),  if( a.tipomov <> 'AP', if( month(a.fechamov) = 4, a.ventotaldes,0) - if( month(a.fechamov) = 4, a.costotaldes,0), if( month(a.fechamov) = 4, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 4, a.costotaldes,0)-if( month(a.fechamov) = 4, a.ventotaldes,0), -1*if( month(a.fechamov) = 4, a.ventotaldes,0)))) GANANCIA04, "

        st05 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'),if( a.tipomov <> 'AP', if( month(a.fechamov) = 5, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 5, a.cantidad,0),0))), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 5, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 5, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad05, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 5, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 5, a.peso,0),0))) As PesoNeto05, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 5, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 5, a.costotaldes,0),0))) CosTotal05, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( month(a.fechamov) = 5, a.ventotaldes,0), -1*if( month(a.fechamov) = 5, a.ventotaldes,0))) VenTotal05, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'),  if( a.tipomov <> 'AP', if( month(a.fechamov) = 5, a.ventotaldes,0) - if( month(a.fechamov) = 5, a.costotaldes,0), if( month(a.fechamov) = 5, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 5, a.costotaldes,0)-if( month(a.fechamov) = 5, a.ventotaldes,0), -1*if( month(a.fechamov) = 5, a.ventotaldes,0)))) GANANCIA05, "

        st06 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'),if( a.tipomov <> 'AP', if( month(a.fechamov) = 6, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 6, a.cantidad,0),0))), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 6, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 6, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad06, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 6, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 6, a.peso,0),0))) As PesoNeto06," _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 6, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 6, a.costotaldes,0),0))) CosTotal06, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( month(a.fechamov) = 6, a.ventotaldes,0), -1*if( month(a.fechamov) = 6, a.ventotaldes,0))) VenTotal06, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'),  if( a.tipomov <> 'AP', if( month(a.fechamov) = 6, a.ventotaldes,0) - if( month(a.fechamov) = 6, a.costotaldes,0), if( month(a.fechamov) = 6, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 6, a.costotaldes,0)-if( month(a.fechamov) = 6, a.ventotaldes,0), -1*if( month(a.fechamov) = 6, a.ventotaldes,0)))) GANANCIA06, "


        st07 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'),if( a.tipomov <> 'AP', if( month(a.fechamov) = 7, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 7, a.cantidad,0),0))), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 7, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 7, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad07, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 7, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 7, a.peso,0),0))) As PesoNeto07, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 7, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 7, a.costotaldes,0),0))) CosTotal07, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( month(a.fechamov) = 7, a.ventotaldes,0), -1*if( month(a.fechamov) = 7, a.ventotaldes,0))) VenTotal07, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'),  if( a.tipomov <> 'AP', if( month(a.fechamov) = 7, a.ventotaldes,0) - if( month(a.fechamov) = 7, a.costotaldes,0), if( month(a.fechamov) = 7, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 7, a.costotaldes,0)-if( month(a.fechamov) = 7, a.ventotaldes,0), -1*if( month(a.fechamov) = 7, a.ventotaldes,0)))) GANANCIA07, "

        st08 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'),if( a.tipomov <> 'AP', if( month(a.fechamov) = 8, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 8, a.cantidad,0),0))), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 8, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 8, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad08, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 8, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 8, a.peso,0),0))) As PesoNeto08, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 8, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 8, a.costotaldes,0),0))) CosTotal08, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( month(a.fechamov) = 8, a.ventotaldes,0), -1*if( month(a.fechamov) = 8, a.ventotaldes,0))) VenTotal08, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'),  if( a.tipomov <> 'AP', if( month(a.fechamov) = 8, a.ventotaldes,0) - if( month(a.fechamov) = 8, a.costotaldes,0), if( month(a.fechamov) = 8, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 8, a.costotaldes,0)-if( month(a.fechamov) = 8, a.ventotaldes,0), -1*if( month(a.fechamov) = 8, a.ventotaldes,0)))) GANANCIA08, "

        st09 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'),if( a.tipomov <> 'AP', if( month(a.fechamov) = 9, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 9, a.cantidad,0),0))), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 9, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 9, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad09, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 9, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 9, a.peso,0),0))) As PesoNeto09, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 9, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 9, a.costotaldes,0),0))) CosTotal09, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( month(a.fechamov) = 9, a.ventotaldes,0), -1*if( month(a.fechamov) = 9, a.ventotaldes,0))) VenTotal09, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'),  if( a.tipomov <> 'AP', if( month(a.fechamov) = 9, a.ventotaldes,0) - if( month(a.fechamov) = 9, a.costotaldes,0), if( month(a.fechamov) = 9, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 9, a.costotaldes,0)-if( month(a.fechamov) = 9, a.ventotaldes,0), -1*if( month(a.fechamov) = 9, a.ventotaldes,0)))) GANANCIA09, "

        st10 = "IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'),if( a.tipomov <> 'AP', if( month(a.fechamov) = 10, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 10, a.cantidad,0),0))), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 10, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 10, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad10, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 10, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 10, a.peso,0),0))) As PesoNeto10, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 10, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 10, a.costotaldes,0),0))) CosTotal10, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( month(a.fechamov) = 10, a.ventotaldes,0), -1*if( month(a.fechamov) = 10, a.ventotaldes,0))) VenTotal10, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'),  if( a.tipomov <> 'AP', if( month(a.fechamov) = 10, a.ventotaldes,0) - if( month(a.fechamov) = 10, a.costotaldes,0), if( month(a.fechamov) = 10, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 10, a.costotaldes,0)-if( month(a.fechamov) = 10, a.ventotaldes,0), -1*if( month(a.fechamov) = 10, a.ventotaldes,0)))) GANANCIA10, "

        st11 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'),if( a.tipomov <> 'AP', if( month(a.fechamov) = 11, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 11, a.cantidad,0),0))), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 11, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 11, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad11, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 11, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 11, a.peso,0),0))) As PesoNeto11, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 11, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 11, a.costotaldes,0),0))) CosTotal11, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( month(a.fechamov) = 11, a.ventotaldes,0), -1*if( month(a.fechamov) = 11, a.ventotaldes,0))) VenTotal11, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'),  if( a.tipomov <> 'AP', if( month(a.fechamov) = 11, a.ventotaldes,0) - if( month(a.fechamov) = 11, a.costotaldes,0), if( month(a.fechamov) = 11, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 11, a.costotaldes,0)-if( month(a.fechamov) = 11, a.ventotaldes,0), -1*if( month(a.fechamov) = 11, a.ventotaldes,0)))) GANANCIA11, "

        st12 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'),if( a.tipomov <> 'AP', if( month(a.fechamov) = 12, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 12, a.cantidad,0),0))), SUM(IF( a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 12, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 12, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad12, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 12, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 12, a.peso,0),0))) As PesoNeto12, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( a.tipomov <> 'AP', if( month(a.fechamov) = 12, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 12, a.costotaldes,0),0))) CosTotal12, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'), if( month(a.fechamov) = 12, a.ventotaldes,0), -1*if( month(a.fechamov) = 12, a.ventotaldes,0))) VenTotal12, " _
            & " Sum( if (a.origen in ('FAC','PFC','PVE','NDV'),  if( a.tipomov <> 'AP', if( month(a.fechamov) = 12, a.ventotaldes,0) - if( month(a.fechamov) = 12, a.costotaldes,0), if( month(a.fechamov) = 12, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 12, a.costotaldes,0)-if( month(a.fechamov) = 12, a.ventotaldes,0), -1*if( month(a.fechamov) = 12, a.ventotaldes,0)))) GANANCIA12, "


        SeleccionSIGMEGananciasBrutasMesMes = "SELECT a.numdoc, a.codart, c.nomart, c.unidad, a.fechamov, " & st01 & st02 & st03 & st04 & st05 & st06 & st07 & st08 & st09 & st10 & st11 & st12 _
            & " a.vendedor, concat(e.codven, ' ', e.apellidos,' ',e.nombres) asesor, " _
            & " b.codcli, b.NOMBRE AS NOMCLI, b.RIF, f.descrip canal, g.descrip tiponegocio, h.descrip zona, i.nomrut ruta, " _
            & " if( q.nombre is null, '', q.nombre) FBARRIO, if( r.nombre is null, '', r.nombre) FCIUDAD, if( s.nombre is null, '', s.nombre) FPARROQUIA, " _
            & " if( t.nombre is null, '', t.nombre) FMUNICIPIO, if(u.nombre is null, '', u.nombre) FESTADO, if(v.nombre is null, '', v.nombre) FPAIS, " _
            & " j.descrip categoria, k.descrip marca, l.descrip tipjer, p.descrip division, elt(c.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') mix " _
            & " from jsmertramer a " _
            & " left join jsvencatcli b on (a.prov_cli = b.codcli and a.id_emp = b.id_emp) " _
            & " left join jsmerctainv c on (a.CODART = c.CODART AND a.ID_EMP = c.ID_EMP) " _
            & " left join jsmerequmer d on (a.codart = d.codart and a.unidad = d.uvalencia and a.id_emp = d.id_emp) " _
            & " left join (SELECT a.codrut, a.nomrut, a.comen, a.codzon, a.codven,  b.cliente, b.nomcli, a.tipo, a.id_emp " _
            & "             FROM jsvenencrut a " _
            & "             LEFT JOIN jsvenrenrut b ON (a.codrut = b.codrut AND a.tipo = b.tipo AND a.id_emp = b.id_emp) " _
            & "             WHERE " _
            & "             a.id_emp = '" & jytsistema.WorkID & "') i on (a.prov_cli = i.cliente and a.vendedor = i.codven and a.id_emp = i.id_emp and i.tipo = '0' ) " _
            & " left join jsvencatven e on (i.codven=e.codven and a.id_emp=e.id_emp) " _
            & " left join jsvenliscan f on (b.categoria = f.codigo and b.id_emp = f.id_emp ) " _
            & " left join jsvenlistip g on (b.unidad = g.codigo and b.id_emp = g.id_emp ) " _
            & " left join jsconctatab h on (i.codzon = h.codigo and e.id_emp = h.id_emp  and h.modulo = '00005') " _
            & " left join jsconctatab j on (c.grupo = j.codigo and c.id_emp = j.id_emp and j.modulo = '00002') " _
            & " left join jsconctatab k on (c.marca = k.codigo and c.id_emp = k.id_emp and k.modulo = '00003') " _
            & " left join jsmerencjer l on (c.tipjer = l.tipjer and c.id_emp = l.id_emp) " _
            & " left join jsconcatter q on (b.fbarrio = q.codigo and b.id_emp = q.id_emp ) " _
            & " left join jsconcatter r on (b.fciudad = r.codigo and b.id_emp = r.id_emp ) " _
            & " left join jsconcatter s on (b.fparroquia = s.codigo and b.id_emp = s.id_emp ) " _
            & " left join jsconcatter t on (b.fmunicipio = t.codigo and b.id_emp = t.id_emp ) " _
            & " left join jsconcatter u on (b.festado = u.codigo and b.id_emp = u.id_emp ) " _
            & " left join jsconcatter v on (b.fpais = v.codigo and b.id_emp = v.id_emp ) " _
            & " left join jsmercatdiv p on (c.division = p.division and c.id_emp = p.id_emp) " _
            & " Where a.origen in ('FAC','PFC','PVE','NDV','NCV') AND " _
            & " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(PrimerDiaAño(FechaDesde)) & "' AND " _
            & " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(UltimoDiaAño(FechaHasta)) & "' " _
            & " AND a.ID_EMP = '" & jytsistema.WorkID & "' " & strSQL _
            & " group by " & strGrupo _
            & " "

    End Function

    Function SeleccionSIGMEGananciasBrutasMesMesPVE(ByVal myConn As MySqlConnection, ByVal lblInfo As Label, _
                                    ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                    Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                    Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "",
                                    Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "",
                                    Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                    Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                    Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                    Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                    Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                    Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                    Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                    Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                    Optional ByVal AgrupadoPor As String() = Nothing) As String

        Dim st01 As String, st02 As String, st03 As String, st04 As String, st05 As String, st06 As String
        Dim st07 As String, st08 As String, st09 As String, st10 As String, st11 As String, st12 As String
        Dim strSQL As String = ""
        Dim strGrupo As String = ""
        Dim i As Integer


        If AgrupadoPor IsNot Nothing Then
            strGrupo = " a.id_emp "
            If UBound(AgrupadoPor) <= 0 Then
            Else
                For i = 0 To UBound(AgrupadoPor) - 1
                    Select Case AgrupadoPor(i)
                        Case "categoria"
                            strGrupo = strGrupo & ", " & " c.grupo "
                        Case "marca"
                            strGrupo = strGrupo & ", " & " c.marca "
                        Case "tipjer"
                            strGrupo = strGrupo & ", " & " c.tipjer "
                        Case "codjer"
                        Case "division"
                            strGrupo = strGrupo & ", " & " c.division "
                        Case "mix"
                            strGrupo = strGrupo & ", " & " c.mix "
                        Case "asesor"
                            strGrupo = strGrupo & ", " & " b.vendedor "
                    End Select
                Next
            End If
        End If

        If CategoriaDesde <> "" Then strSQL += " and c.grupo >= '" & CategoriaDesde & "' "
        If CategoriaHasta <> "" Then strSQL += " and c.grupo <= '" & CategoriaHasta & "' "
        If MarcaDesde <> "" Then strSQL += " and c.MARCA  >= '" & MarcaDesde & "'"
        If MarcaHasta <> "" Then strSQL += " and  c.MARCA <= '" & MarcaHasta & "'"
        If DivisionDesde <> "" Then strSQL += " and c.division >= '" & DivisionDesde & "' "
        If DivisionHasta <> "" Then strSQL += " and c.division <= '" & DivisionHasta & "' "
        If TipoJerarquia <> "" Then strSQL += " and c.tipjer = '" & TipoJerarquia & "' "
        If AsesorDesde <> "" Then strSQL += " AND b.vendedor >= '" & AsesorDesde & "' "
        If AsesorHasta <> "" Then strSQL += " AND b.vendedor <= '" & AsesorHasta & "' "


        st01 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen = 'PVE',if( a.tipomov <> 'AP', if( month(a.fechamov) = 1, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 1, a.cantidad,0),0))), SUM(IF( a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 1, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 1, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad01, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 1, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 1, a.peso,0),0))) As PesoNeto01, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 1, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 1, a.costotaldes,0),0))) CosTotal01, " _
            & " Sum( if (a.origen = 'PVE', if( month(a.fechamov) = 1, a.ventotaldes,0), -1*if( month(a.fechamov) = 1, a.ventotaldes,0))) VenTotal01, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 1, a.ventotaldes,0) - if( month(a.fechamov) = 1, a.costotaldes,0), if( month(a.fechamov) = 1, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 1, a.costotaldes,0)-if( month(a.fechamov) = 1, a.ventotaldes,0), -1*if( month(a.fechamov) = 1, a.ventotaldes,0)))) GANANCIA01, "

        st02 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen = 'PVE',if( a.tipomov <> 'AP', if( month(a.fechamov) = 2, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 2, a.cantidad,0),0))), SUM(IF( a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 2, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 2, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad02, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 2, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 2, a.peso,0),0))) As PesoNeto02, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 2, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 2, a.costotaldes,0),0))) CosTotal02, " _
            & " Sum( if (a.origen = 'PVE', if( month(a.fechamov) = 2, a.ventotaldes,0), -1*if( month(a.fechamov) = 2, a.ventotaldes,0))) VenTotal02, " _
            & " Sum( if (a.origen = 'PVE',  if( a.tipomov <> 'AP', if( month(a.fechamov) = 2, a.ventotaldes,0) - if( month(a.fechamov) = 2, a.costotaldes,0), if( month(a.fechamov) = 2, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 2, a.costotaldes,0)-if( month(a.fechamov) = 2, a.ventotaldes,0), -1*if( month(a.fechamov) = 2, a.ventotaldes,0)))) GANANCIA02, "

        st03 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen = 'PVE',if( a.tipomov <> 'AP', if( month(a.fechamov) = 3, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 3, a.cantidad,0),0))), SUM(IF( a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 3, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 3, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad03, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 3, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 3, a.peso,0),0))) As PesoNeto03, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 3, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 3, a.costotaldes,0),0))) CosTotal03, " _
            & " Sum( if (a.origen = 'PVE', if( month(a.fechamov) = 3, a.ventotaldes,0), -1*if( month(a.fechamov) = 3, a.ventotaldes,0))) VenTotal03, " _
            & " Sum( if (a.origen = 'PVE',  if( a.tipomov <> 'AP', if( month(a.fechamov) = 3, a.ventotaldes,0) - if( month(a.fechamov) = 3, a.costotaldes,0), if( month(a.fechamov) = 3, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 3, a.costotaldes,0)-if( month(a.fechamov) = 3, a.ventotaldes,0), -1*if( month(a.fechamov) = 3, a.ventotaldes,0)))) GANANCIA03, "


        st04 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen = 'PVE',if( a.tipomov <> 'AP', if( month(a.fechamov) = 4, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 4, a.cantidad,0),0))), SUM(IF( a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 4, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 4, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad04, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 4, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 4, a.peso,0),0))) As PesoNeto04, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 4, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 4, a.costotaldes,0),0))) As CosTotal04, " _
            & " Sum( if (a.origen = 'PVE', if( month(a.fechamov) = 4, a.ventotaldes,0), -1*if( month(a.fechamov) = 4, a.ventotaldes,0))) As VenTotal04, " _
            & " Sum( if (a.origen = 'PVE',  if( a.tipomov <> 'AP', if( month(a.fechamov) = 4, a.ventotaldes,0) - if( month(a.fechamov) = 4, a.costotaldes,0), if( month(a.fechamov) = 4, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 4, a.costotaldes,0)-if( month(a.fechamov) = 4, a.ventotaldes,0), -1*if( month(a.fechamov) = 4, a.ventotaldes,0)))) AS GANANCIA04, "

        st05 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen = 'PVE',if( a.tipomov <> 'AP', if( month(a.fechamov) = 5, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 5, a.cantidad,0),0))), SUM(IF( a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 5, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 5, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad05, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 5, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 5, a.peso,0),0))) As PesoNeto05, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 5, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 5, a.costotaldes,0),0))) CosTotal05, " _
            & " Sum( if (a.origen = 'PVE', if( month(a.fechamov) = 5, a.ventotaldes,0), -1*if( month(a.fechamov) = 5, a.ventotaldes,0))) VenTotal05, " _
            & " Sum( if (a.origen = 'PVE',  if( a.tipomov <> 'AP', if( month(a.fechamov) = 5, a.ventotaldes,0) - if( month(a.fechamov) = 5, a.costotaldes,0), if( month(a.fechamov) = 5, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 5, a.costotaldes,0)-if( month(a.fechamov) = 5, a.ventotaldes,0), -1*if( month(a.fechamov) = 5, a.ventotaldes,0)))) GANANCIA05, "

        st06 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen = 'PVE',if( a.tipomov <> 'AP', if( month(a.fechamov) = 6, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 6, a.cantidad,0),0))), SUM(IF( a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 6, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 6, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad06, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 6, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 6, a.peso,0),0))) As PesoNeto06," _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 6, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 6, a.costotaldes,0),0))) CosTotal06, " _
            & " Sum( if (a.origen = 'PVE', if( month(a.fechamov) = 6, a.ventotaldes,0), -1*if( month(a.fechamov) = 6, a.ventotaldes,0))) VenTotal06, " _
            & " Sum( if (a.origen = 'PVE',  if( a.tipomov <> 'AP', if( month(a.fechamov) = 6, a.ventotaldes,0) - if( month(a.fechamov) = 6, a.costotaldes,0), if( month(a.fechamov) = 6, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 6, a.costotaldes,0)-if( month(a.fechamov) = 6, a.ventotaldes,0), -1*if( month(a.fechamov) = 6, a.ventotaldes,0)))) GANANCIA06, "


        st07 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen = 'PVE',if( a.tipomov <> 'AP', if( month(a.fechamov) = 7, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 7, a.cantidad,0),0))), SUM(IF( a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 7, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 7, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad07, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 7, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 7, a.peso,0),0))) As PesoNeto07, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 7, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 7, a.costotaldes,0),0))) CosTotal07, " _
            & " Sum( if (a.origen = 'PVE', if( month(a.fechamov) = 7, a.ventotaldes,0), -1*if( month(a.fechamov) = 7, a.ventotaldes,0))) VenTotal07, " _
            & " Sum( if (a.origen = 'PVE',  if( a.tipomov <> 'AP', if( month(a.fechamov) = 7, a.ventotaldes,0) - if( month(a.fechamov) = 7, a.costotaldes,0), if( month(a.fechamov) = 7, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 7, a.costotaldes,0)-if( month(a.fechamov) = 7, a.ventotaldes,0), -1*if( month(a.fechamov) = 7, a.ventotaldes,0)))) GANANCIA07, "

        st08 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen = 'PVE',if( a.tipomov <> 'AP', if( month(a.fechamov) = 8, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 8, a.cantidad,0),0))), SUM(IF( a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 8, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 8, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad08, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 8, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 8, a.peso,0),0))) As PesoNeto08, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 8, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 8, a.costotaldes,0),0))) CosTotal08, " _
            & " Sum( if (a.origen = 'PVE', if( month(a.fechamov) = 8, a.ventotaldes,0), -1*if( month(a.fechamov) = 8, a.ventotaldes,0))) VenTotal08, " _
            & " Sum( if (a.origen = 'PVE',  if( a.tipomov <> 'AP', if( month(a.fechamov) = 8, a.ventotaldes,0) - if( month(a.fechamov) = 8, a.costotaldes,0), if( month(a.fechamov) = 8, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 8, a.costotaldes,0)-if( month(a.fechamov) = 8, a.ventotaldes,0), -1*if( month(a.fechamov) = 8, a.ventotaldes,0)))) GANANCIA08, "

        st09 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen = 'PVE',if( a.tipomov <> 'AP', if( month(a.fechamov) = 9, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 9, a.cantidad,0),0))), SUM(IF( a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 9, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 9, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad09, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 9, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 9, a.peso,0),0))) As PesoNeto09, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 9, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 9, a.costotaldes,0),0))) CosTotal09, " _
            & " Sum( if (a.origen = 'PVE', if( month(a.fechamov) = 9, a.ventotaldes,0), -1*if( month(a.fechamov) = 9, a.ventotaldes,0))) VenTotal09, " _
            & " Sum( if (a.origen = 'PVE',  if( a.tipomov <> 'AP', if( month(a.fechamov) = 9, a.ventotaldes,0) - if( month(a.fechamov) = 9, a.costotaldes,0), if( month(a.fechamov) = 9, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 9, a.costotaldes,0)-if( month(a.fechamov) = 9, a.ventotaldes,0), -1*if( month(a.fechamov) = 9, a.ventotaldes,0)))) GANANCIA09, "

        st10 = "IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen = 'PVE',if( a.tipomov <> 'AP', if( month(a.fechamov) = 10, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 10, a.cantidad,0),0))), SUM(IF( a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 10, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 10, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad10, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 10, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 10, a.peso,0),0))) As PesoNeto10, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 10, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 10, a.costotaldes,0),0))) CosTotal10, " _
            & " Sum( if (a.origen = 'PVE', if( month(a.fechamov) = 10, a.ventotaldes,0), -1*if( month(a.fechamov) = 10, a.ventotaldes,0))) VenTotal10, " _
            & " Sum( if (a.origen = 'PVE',  if( a.tipomov <> 'AP', if( month(a.fechamov) = 10, a.ventotaldes,0) - if( month(a.fechamov) = 10, a.costotaldes,0), if( month(a.fechamov) = 10, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 10, a.costotaldes,0)-if( month(a.fechamov) = 10, a.ventotaldes,0), -1*if( month(a.fechamov) = 10, a.ventotaldes,0)))) GANANCIA10, "

        st11 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen = 'PVE',if( a.tipomov <> 'AP', if( month(a.fechamov) = 11, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 11, a.cantidad,0),0))), SUM(IF( a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 11, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 11, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad11, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 11, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 11, a.peso,0),0))) As PesoNeto11, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 11, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 11, a.costotaldes,0),0))) CosTotal11, " _
            & " Sum( if (a.origen = 'PVE', if( month(a.fechamov) = 11, a.ventotaldes,0), -1*if( month(a.fechamov) = 11, a.ventotaldes,0))) VenTotal11, " _
            & " Sum( if (a.origen = 'PVE',  if( a.tipomov <> 'AP', if( month(a.fechamov) = 11, a.ventotaldes,0) - if( month(a.fechamov) = 11, a.costotaldes,0), if( month(a.fechamov) = 11, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 11, a.costotaldes,0)-if( month(a.fechamov) = 11, a.ventotaldes,0), -1*if( month(a.fechamov) = 11, a.ventotaldes,0)))) GANANCIA11, "

        st12 = " IF( ISNULL(d.UVALENCIA), SUM(IF( a.origen = 'PVE',if( a.tipomov <> 'AP', if( month(a.fechamov) = 12, a.cantidad,0) ,0), -1*if( a.tipomov <> 'AP', if( month(a.fechamov) = 12, a.cantidad,0),0))), SUM(IF( a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 12, a.cantidad,0),0), -1*if( a.tipomov <> 'AP',if( month(a.fechamov) = 12, a.cantidad,0),0)))/d.EQUIVALE ) AS Cantidad12, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 12, a.peso,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 12, a.peso,0),0))) As PesoNeto12, " _
            & " Sum( if (a.origen = 'PVE', if( a.tipomov <> 'AP', if( month(a.fechamov) = 12, a.costotaldes,0),0) , if( a.tipomov <> 'AP', -1*if( month(a.fechamov) = 12, a.costotaldes,0),0))) CosTotal12, " _
            & " Sum( if (a.origen = 'PVE', if( month(a.fechamov) = 12, a.ventotaldes,0), -1*if( month(a.fechamov) = 12, a.ventotaldes,0))) VenTotal12, " _
            & " Sum( if (a.origen = 'PVE',  if( a.tipomov <> 'AP', if( month(a.fechamov) = 12, a.ventotaldes,0) - if( month(a.fechamov) = 12, a.costotaldes,0), if( month(a.fechamov) = 12, a.ventotaldes,0)), if (a.tipomov <> 'AP', if( month(a.fechamov) = 12, a.costotaldes,0)-if( month(a.fechamov) = 12, a.ventotaldes,0), -1*if( month(a.fechamov) = 12, a.ventotaldes,0)))) GANANCIA12, "

        SeleccionSIGMEGananciasBrutasMesMesPVE = "SELECT a.numdoc, a.codart, c.nomart, c.unidad, a.fechamov, " & st01 & st02 & st03 & st04 & st05 & st06 & st07 & st08 & st09 & st10 & st11 & st12 _
            & " a.vendedor, concat(e.codven, ' ', e.apellidos,' ',e.nombres) asesor, " _
            & " b.codcli, b.NOMBRE AS NOMCLI, b.RIF, '' canal, '' tiponegocio, '' zona, '' ruta,  " _
            & " if( q.nombre is null, '', q.nombre) FBARRIO, if( r.nombre is null, '', r.nombre) FCIUDAD, if( s.nombre is null, '', s.nombre) FPARROQUIA, " _
            & " if( t.nombre is null, '', t.nombre) FMUNICIPIO, if(u.nombre is null, '', u.nombre) FESTADO, if(v.nombre is null, '', v.nombre) FPAIS, " _
            & " j.descrip categoria, k.descrip marca, l.descrip tipjer, p.descrip division, elt(c.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') mix " _
            & " from jsmertramer a " _
            & " left join jsvencatcli b on (a.prov_cli = b.codcli and a.id_emp = b.id_emp) " _
            & " left join jsmerctainv c on (a.CODART = c.CODART AND a.ID_EMP = c.ID_EMP) " _
            & " left join jsmerequmer d on (a.codart = d.codart and a.unidad = d.uvalencia and a.id_emp = d.id_emp) " _
            & " left join jsvencatven e on (a.vendedor=e.codven and a.id_emp=e.id_emp) " _
            & " left join jsconctatab j on (c.grupo = j.codigo and c.id_emp = j.id_emp and j.modulo = '00002') " _
            & " left join jsconctatab k on (c.marca = k.codigo and c.id_emp = k.id_emp and k.modulo = '00003') " _
            & " left join jsmerencjer l on (c.tipjer = l.tipjer and c.id_emp = l.id_emp) " _
            & " left join jsconcatter q on (b.fbarrio = q.codigo and b.id_emp = q.id_emp ) " _
            & " left join jsconcatter r on (b.fciudad = r.codigo and b.id_emp = r.id_emp ) " _
            & " left join jsconcatter s on (b.fparroquia = s.codigo and b.id_emp = s.id_emp ) " _
            & " left join jsconcatter t on (b.fmunicipio = t.codigo and b.id_emp = t.id_emp ) " _
            & " left join jsconcatter u on (b.festado = u.codigo and b.id_emp = u.id_emp ) " _
            & " left join jsconcatter v on (b.fpais = v.codigo and b.id_emp = v.id_emp ) " _
            & " left join jsmercatdiv p on (c.division = p.division and c.id_emp = p.id_emp) " _
            & " Where a.origen ='PVE' AND " _
            & " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(PrimerDiaAño(FechaDesde)) & "' AND " _
            & " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(UltimoDiaAño(FechaHasta)) & "' " _
            & " AND a.ID_EMP = '" & jytsistema.WorkID & "' " & strSQL _
            & " group by " & strGrupo _
            & " "

    End Function


    Function SeleccionSIGMEgananciasBrutasFacturasPVE(ByVal myConn As MySqlConnection, ByVal lblInfo As Label, _
    ByVal FechaDesde As Date, ByVal FechaHasta As Date, Optional ByVal AsesorDesde As String = "", _
    Optional ByVal AsesorHasta As String = "", Optional ByVal CanalDesde As String = "", _
    Optional ByVal CanalHasta As String = "", Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
    Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
    Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
    Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
    Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
    Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
    Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
    Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
    Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "") As String

        Dim strSQLPie As String

        Dim tblGanancias As String
        Dim strSQLWhere As String

        Dim strSQL As String = ""

        strSQLPie = " group by a.numdoc, a.codart " _
                & " ORDER BY a.numdoc, a.codart "


        tblGanancias = "tbl" & ft.NumeroAleatorio(10000)

        ft.Ejecutar_strSQL(myconn, "create temporary table " & tblGanancias & "( " _
            & " prov_cli        VARCHAR(15)                     NOT NULL, " _
            & " vendedor        VARCHAR(05)                     NOT NULL, " _
            & " numdoc          VARCHAR(15)                     NOT NULL, " _
            & " fechamov        DATETIME    default '000-00-00' not null,  " _
            & " codart          VARCHAR(15)                     NOT NULL, " _
            & " unidad          VARCHAR(03)                     NOT NULL, " _
            & " signo           int(03)                         NOT NULL, " _
            & " cantidad        double(10,3)  default '0.000'   NOT NULL, " _
            & " equivale        double(10,3)  default '0.000'   NOT NULL, " _
            & " costotal        double(19,3)  default '0.00'    not null, " _
            & " costotaldes     double(19,3)  default '0.00'    not null, " _
            & " ventotal        double(19,3)  default '0.00'    not null, " _
            & " ventotaldes     double(19,3)  default '0.00'    not null) ")

        If CategoriaDesde <> "" Then strSQL += " and c.grupo >= '" & CategoriaDesde & "' "
        If CategoriaHasta <> "" Then strSQL += " and c.grupo <= '" & CategoriaHasta & "' "
        If MarcaDesde <> "" Then strSQL += " and c.MARCA >= '" & MarcaDesde & "' "
        If MarcaHasta <> "" Then strSQL += " and c.MARCA <= '" & MarcaHasta & "' "
        If DivisionDesde <> "" Then strSQL += " and c.division >= '" & DivisionDesde & "' "
        If DivisionHasta <> "" Then strSQL += " and c.division <= '" & DivisionHasta & "' "
        If TipoJerarquia <> "" Then strSQL += " and c.tipjer = '" & TipoJerarquia & "' "

        If AsesorDesde <> "" Then strSQL += " AND b.VENDEDOR >= '" & AsesorDesde & "' "
        If AsesorHasta <> "" Then strSQL += " AND b.VENDEDOR <= '" & AsesorHasta & "' "

        ft.Ejecutar_strSQL(myconn, " insert into " & tblGanancias & " select a.prov_cli, a.vendedor, a.numdoc, a.fechamov, a.codart, a.unidad, if(tipomov in ('EN', 'AE'), -1, 1) as signo,  sum(a.cantidad) as cantidad, " _
            & " if( isnull(b.equivale),1, b.equivale) as equivale,  sum(a.costotal) as costotal, " _
            & " sum(a.costotaldes) as costotaldes, sum(a.ventotal) as ventotal, sum(ventotaldes) as ventotaldes " _
            & " from jsmertramer a " _
            & " left join jsmerequmer b on (a.codart = b.codart and a.unidad = b.uvalencia and a.id_emp = b.id_emp) " _
            & " Where " _
            & " a.origen ='PVE' and " _
            & " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaDesde) & "' AND " _
            & " a.FECHAMOV <=  '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' and " _
            & " a.id_emp  = '" & jytsistema.WorkID & "'  " _
            & " group By " _
            & " a.prov_cli, a.vendedor, a.codart, a.unidad, signo ")

        strSQLWhere = strSQL
        If UCase(Left(LTrim(strSQLWhere), 4)) = "AND " Then
            strSQLWhere = Mid(LTrim(strSQLWhere), 4, Len(strSQLWhere))
        End If

        If Trim(strSQLWhere) <> "" Then strSQLWhere = " where " & strSQLWhere

        SeleccionSIGMEgananciasBrutasFacturasPVE = " SELECT a.numdoc, a.fechamov, a.codart, c.nomart, c.unidad,  " _
            & " sum(a.signo*a.cantidad/a.equivale) cantidadreal, " _
            & " Sum(a.signo*a.costotaldes) costotaldes, " _
            & " Sum(a.signo*a.ventotaldes) ventotaldes, " _
            & " (sum(a.signo*a.ventotaldes) - sum(a.signo*a.costotaldes)) ganancia, " _
            & " (1 - sum(a.signo*a.costotaldes) / sum(a.signo*a.ventotaldes))*100 porgan, " _
            & " a.vendedor, concat(e.codven, ' ', e.apellidos,' ',e.nombres) asesor, " _
            & " b.codcli, b.nombre nomcli, b.rif, '' canal, '' tiponegocio, '' zona, '' ruta, " _
            & " if( q.nombre is null, '', q.nombre) FBARRIO, if( r.nombre is null, '', r.nombre) FCIUDAD, if( s.nombre is null, '', s.nombre) FPARROQUIA, " _
            & " if( t.nombre is null, '', t.nombre) FMUNICIPIO, if(u.nombre is null, '', u.nombre) FESTADO, if(v.nombre is null, '', v.nombre) FPAIS, " _
            & " j.descrip categoria, k.descrip marca, l.descrip tipjer, p.descrip division, elt(c.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') as mix " _
            & " from " & tblGanancias & " a " _
            & " left join jsvencatcli b on (a.prov_cli = b.codcli and b.id_emp = '" & jytsistema.WorkID & "' ) " _
            & " left join jsmerctainv c on (a.CODART = c.CODART AND c.id_emp = '" & jytsistema.WorkID & "') " _
            & " left join jsvencatven e on (a.vendedor=e.codven and e.id_emp = '" & jytsistema.WorkID & "' ) " _
            & " left join jsconctatab j on (c.grupo = j.codigo and c.id_emp = j.id_emp and j.modulo = '00002') " _
            & " left join jsconctatab k on (c.marca = k.codigo and c.id_emp = k.id_emp and k.modulo = '00003') " _
            & " left join jsmerencjer l on (c.tipjer = l.tipjer and c.id_emp = l.id_emp) " _
            & " left join jsconcatter q on (b.fbarrio = q.codigo and b.id_emp = q.id_emp ) " _
            & " left join jsconcatter r on (b.fciudad = r.codigo and b.id_emp = r.id_emp ) " _
            & " left join jsconcatter s on (b.fparroquia = s.codigo and b.id_emp = s.id_emp ) " _
            & " left join jsconcatter t on (b.fmunicipio = t.codigo and b.id_emp = t.id_emp ) " _
            & " left join jsconcatter u on (b.festado = u.codigo and b.id_emp = u.id_emp ) " _
            & " left join jsconcatter v on (b.fpais = v.codigo and b.id_emp = v.id_emp ) " _
            & " left join jsmercatdiv p on (c.division = p.division and c.id_emp = p.id_emp) " _
            & " " _
            & strSQLWhere & strSQLPie


    End Function

    Function SeleccionSIGMEIngresosporAsesoryForma(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal AsesorDesde As String, ByVal AsesorHasta As String, _
                                                    ByVal FechaDesde As Date, ByVal FechaHasta As Date) As String

        Dim strSQLAsesor1 As String = ""
        Dim strSQLAsesor2 As String = ""

        If AsesorDesde <> "" Then strSQLAsesor1 = " And b.vendedor >= '" & AsesorDesde & "' "
        If AsesorHasta <> "" Then strSQLAsesor1 = strSQLAsesor1 & " And b.vendedor <= '" & AsesorHasta & "' "
        If AsesorDesde <> "" Then strSQLAsesor2 = " And a.codven >= '" & AsesorDesde & "' "
        If AsesorHasta <> "" Then strSQLAsesor2 = strSQLAsesor2 & " And a.codven <= '" & AsesorHasta & "' "

        SeleccionSIGMEIngresosporAsesoryForma = " select IFNULL(SUM(a.IMPORTE),0)*(-1) monto, concat(a.formapag,'     ') as forma,if (a.formapag='EF','EFECTIVO                  ',if (a.formapag='CH','CHEQUE     ','DEPOSITO    ')) as formadescrip, " _
            & " '        ' as codigo_corredor,'            ' as corredor, b.vendedor as codigo_vendedor, concat(c.apellidos,' ',c.nombres) as vendedor " _
            & " from jsventracob a " _
            & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
            & " left join jsvencatven c  on (b.vendedor = c.codven and b.id_emp= c.id_emp) " _
            & " where a.id_emp='" & jytsistema.WorkID & "' " _
            & " and a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' " _
            & " and a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and a.formapag in ('CH','DP','EF') " & strSQLAsesor1 _
            & " group by 2,6 " _
            & " Union " _
            & " select IFNULL(SUM(a.IMPORTE),0) as monto,CONCAT(a.formpag,a.refpag) as forma , concat('CHEQUE ALIMENTACION ', a.refpag) as formadescrip , " _
            & " a.refpag as codigo_corredor, b.descrip as corredor, a.codven as codigo_vendedor,concat(c.apellidos,' ',c.nombres) as vendedor " _
            & " from jsbantracaj a left join jsvencestic b on (a.refpag=b.codigo and a.id_emp=b.id_emp) left join jsvencatven c on (a.codven=c.codven and a.id_emp=c.id_emp) " _
            & " where a.id_emp='" & jytsistema.WorkID & "' and a.tipomov='EN' " _
            & " and a.formpag='CT' " _
            & " and a.fecha >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' " _
            & " and a.fecha <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' " & strSQLAsesor2 _
            & " group by 2,4,6 " _
            & " order by forma, codigo_vendedor "

    End Function

    Function SeleccionSIGMEVentasyComprasMesaMes(ByVal myConn As MySqlConnection, ByVal lblInfo As Label, _
                                    ByVal JerarquiaDesde As String, ByVal JerarquiaHasta As String, _
                                    ByVal AlmacenDesde As String, ByVal AlmacenHasta As String, ByVal Año As Integer) As String

        Dim strSQLTipJer As String = ""

        Dim tblexistencias_jer As String
        Dim tblVentas_jer As String
        Dim tblcompras_jer As String

        Dim AñoTrabajo As String = CStr(Año)
        Dim FechaTrabajo As Date = CDate("31/12/" & Año)

        If JerarquiaDesde <> "" Then strSQLTipJer = " and a.tipjer >= '" & JerarquiaDesde & "' "
        If JerarquiaHasta <> "" Then strSQLTipJer = strSQLTipJer & " and a.tipjer <= '" & JerarquiaHasta & "' "

        Dim strAlm As String = ""
        If AlmacenDesde <> "" Then strAlm += " and b.almacen >= '" & AlmacenDesde & "' "
        If AlmacenHasta <> "" Then strAlm += " and b.almacen <= '" & AlmacenHasta & "' "

        tblexistencias_jer = " select a.tipjer jerarquia," _
            & " sum( if( 1 <= " & Month(FechaTrabajo) & " , if( date_format(b.fechamov,'%Y%m') <='" & AñoTrabajo & "01' ,if(b.codart is null,0,if(c.codart is null,if(b.tipomov='EN',b.cantidad*a.pesounidad,b.cantidad*a.pesounidad*(-1)),if(b.tipomov='EN',(b.cantidad*a.pesounidad)/c.equivale, ((b.cantidad*a.pesounidad)/c.equivale)*(-1)))),0),0)) mes1, " _
            & " sum( if( 2 <= " & Month(FechaTrabajo) & " , if( date_format(b.fechamov,'%Y%m') <='" & AñoTrabajo & "02' ,if(b.codart is null,0,if(c.codart is null,if(b.tipomov='EN',b.cantidad*a.pesounidad,b.cantidad*a.pesounidad*(-1)),if(b.tipomov='EN',(b.cantidad*a.pesounidad)/c.equivale, ((b.cantidad*a.pesounidad)/c.equivale)*(-1)))),0),0)) mes2, " _
            & " sum( if( 3 <= " & Month(FechaTrabajo) & " , if( date_format(b.fechamov,'%Y%m') <='" & AñoTrabajo & "03' ,if(b.codart is null,0,if(c.codart is null,if(b.tipomov='EN',b.cantidad*a.pesounidad,b.cantidad*a.pesounidad*(-1)),if(b.tipomov='EN',(b.cantidad*a.pesounidad)/c.equivale, ((b.cantidad*a.pesounidad)/c.equivale)*(-1)))),0),0)) mes3, " _
            & " sum( if( 4 <= " & Month(FechaTrabajo) & " , if( date_format(b.fechamov,'%Y%m') <='" & AñoTrabajo & "04' ,if(b.codart is null,0,if(c.codart is null,if(b.tipomov='EN',b.cantidad*a.pesounidad,b.cantidad*a.pesounidad*(-1)),if(b.tipomov='EN',(b.cantidad*a.pesounidad)/c.equivale, ((b.cantidad*a.pesounidad)/c.equivale)*(-1)))),0),0)) mes4, " _
            & " sum( if( 5 <= " & Month(FechaTrabajo) & " , if( date_format(b.fechamov,'%Y%m') <='" & AñoTrabajo & "05' ,if(b.codart is null,0,if(c.codart is null,if(b.tipomov='EN',b.cantidad*a.pesounidad,b.cantidad*a.pesounidad*(-1)),if(b.tipomov='EN',(b.cantidad*a.pesounidad)/c.equivale, ((b.cantidad*a.pesounidad)/c.equivale)*(-1)))),0),0)) mes5, " _
            & " sum( if( 6 <= " & Month(FechaTrabajo) & " , if( date_format(b.fechamov,'%Y%m') <='" & AñoTrabajo & "06' ,if(b.codart is null,0,if(c.codart is null,if(b.tipomov='EN',b.cantidad*a.pesounidad,b.cantidad*a.pesounidad*(-1)),if(b.tipomov='EN',(b.cantidad*a.pesounidad)/c.equivale, ((b.cantidad*a.pesounidad)/c.equivale)*(-1)))),0),0)) mes6, " _
            & " sum( if( 7 <= " & Month(FechaTrabajo) & " , if( date_format(b.fechamov,'%Y%m') <='" & AñoTrabajo & "07' ,if(b.codart is null,0,if(c.codart is null,if(b.tipomov='EN',b.cantidad*a.pesounidad,b.cantidad*a.pesounidad*(-1)),if(b.tipomov='EN',(b.cantidad*a.pesounidad)/c.equivale, ((b.cantidad*a.pesounidad)/c.equivale)*(-1)))),0),0)) mes7, " _
            & " sum( if( 8 <= " & Month(FechaTrabajo) & " , if( date_format(b.fechamov,'%Y%m') <='" & AñoTrabajo & "08' ,if(b.codart is null,0,if(c.codart is null,if(b.tipomov='EN',b.cantidad*a.pesounidad,b.cantidad*a.pesounidad*(-1)),if(b.tipomov='EN',(b.cantidad*a.pesounidad)/c.equivale, ((b.cantidad*a.pesounidad)/c.equivale)*(-1)))),0),0)) mes8, " _
            & " sum( if( 9 <= " & Month(FechaTrabajo) & " , if( date_format(b.fechamov,'%Y%m') <='" & AñoTrabajo & "09' ,if(b.codart is null,0,if(c.codart is null,if(b.tipomov='EN',b.cantidad*a.pesounidad,b.cantidad*a.pesounidad*(-1)),if(b.tipomov='EN',(b.cantidad*a.pesounidad)/c.equivale, ((b.cantidad*a.pesounidad)/c.equivale)*(-1)))),0),0)) mes9, " _
            & " sum( if( 10 <= " & Month(FechaTrabajo) & " , if( date_format(b.fechamov,'%Y%m') <='" & AñoTrabajo & "10' ,if(b.codart is null,0,if(c.codart is null,if(b.tipomov='EN',b.cantidad*a.pesounidad,b.cantidad*a.pesounidad*(-1)),if(b.tipomov='EN',(b.cantidad*a.pesounidad)/c.equivale, ((b.cantidad*a.pesounidad)/c.equivale)*(-1)))),0),0)) mes10, " _
            & " sum( if( 11 <= " & Month(FechaTrabajo) & " , if( date_format(b.fechamov,'%Y%m') <='" & AñoTrabajo & "11' ,if(b.codart is null,0,if(c.codart is null,if(b.tipomov='EN',b.cantidad*a.pesounidad,b.cantidad*a.pesounidad*(-1)),if(b.tipomov='EN',(b.cantidad*a.pesounidad)/c.equivale, ((b.cantidad*a.pesounidad)/c.equivale)*(-1)))),0),0)) mes11, " _
            & " sum( if( 12 <= " & Month(FechaTrabajo) & " , if( date_format(b.fechamov,'%Y%m') <='" & AñoTrabajo & "12' ,if(b.codart is null,0,if(c.codart is null,if(b.tipomov='EN',b.cantidad*a.pesounidad,b.cantidad*a.pesounidad*(-1)),if(b.tipomov='EN',(b.cantidad*a.pesounidad)/c.equivale, ((b.cantidad*a.pesounidad)/c.equivale)*(-1)))),0),0)) mes12 " _
            & " from jsmerctainv a " _
            & " left join jsmertramer b on (a.codart=b.codart and a.id_emp=b.id_emp " & strAlm & " ) " _
            & " left join jsmerequmer c on (b.codart=c.codart and b.id_emp=c.id_emp and " _
            & " b.unidad=c.uvalencia) where a.id_emp='" & jytsistema.WorkID _
            & "' group by 1 "

        'ft.Ejecutar_strSQL ( myconn, " create temporary table " & tblVentas_jer & " (jerarquia  char(2),mes1 double (19,3), mes2 double (19,3),mes3 double (19,3),mes4 double (19,3),mes5 double (19,3),  mes6 double (19,3),mes7 double (19,3),mes8 double (19,3),mes9 double (19,3),mes10 double (19,3),  mes11 double (19,3),mes12 double (19,3))")
        tblVentas_jer = " select a.tipjer jerarquia," _
            & " sum(if(month(b.fechamov)=1,if( b.tipomov = 'SA',  b.peso,-1*if (b.tipomov <> 'AP', b.peso, 0)),0)) mes1, " _
            & " sum(if(month(b.fechamov)=2,if( b.tipomov = 'SA',  b.peso,-1*if (b.tipomov <> 'AP', b.peso, 0)),0)) mes2, " _
            & " sum(if(month(b.fechamov)=3,if( b.tipomov = 'SA',  b.peso,-1*if (b.tipomov <> 'AP', b.peso, 0)),0)) mes3, " _
            & " sum(if(month(b.fechamov)=4,if( b.tipomov = 'SA',  b.peso,-1*if (b.tipomov <> 'AP', b.peso, 0)),0)) mes4, " _
            & " sum(if(month(b.fechamov)=5,if( b.tipomov = 'SA',  b.peso,-1*if (b.tipomov <> 'AP', b.peso, 0)),0)) mes5, " _
            & " sum(if(month(b.fechamov)=6,if( b.tipomov = 'SA',  b.peso,-1*if (b.tipomov <> 'AP', b.peso, 0)),0)) mes6, " _
            & " sum(if(month(b.fechamov)=7,if( b.tipomov = 'SA',  b.peso,-1*if (b.tipomov <> 'AP', b.peso, 0)),0)) mes7, " _
            & " sum(if(month(b.fechamov)=8,if( b.tipomov = 'SA',  b.peso,-1*if (b.tipomov <> 'AP', b.peso, 0)),0)) mes8, " _
            & " sum(if(month(b.fechamov)=9,if( b.tipomov = 'SA',  b.peso,-1*if (b.tipomov <> 'AP', b.peso, 0)),0)) mes9, " _
            & " sum(if(month(b.fechamov)=10,if( b.tipomov = 'SA',  b.peso,-1*if (b.tipomov <> 'AP', b.peso, 0)),0)) mes10, " _
            & " sum(if(month(b.fechamov)=11,if( b.tipomov = 'SA',  b.peso,-1*if (b.tipomov <> 'AP', b.peso, 0)),0)) mes11, " _
            & " sum(if(month(b.fechamov)=12,if( b.tipomov = 'SA',  b.peso,-1*if (b.tipomov <> 'AP', b.peso, 0)),0)) mes12 " _
            & " from jsmerctainv a " _
            & " left join jsmertramer b  on (a.codart=b.codart and a.id_emp=b.id_emp and b.origen in ('FAC','NCV','NDV','PVE','PFC') and " _
            & " year(b.fechamov) = " & Año & ") " _
            & " left join jsmerencjer c on (a.tipjer=c.tipjer and a.id_emp=c.id_emp) " _
            & " where a.id_emp='" & jytsistema.WorkID & "' " _
            & " group by 1 "

        '        ft.Ejecutar_strSQL ( myconn, " create temporary table " & tblcompras_jer & " (jerarquia  char(2),mes1 double (19,3), mes2 double (19,3),mes3 double (19,3),mes4 double (19,3),mes5 double (19,3), mes6 double (19,3),mes7 double (19,3),mes8 double (19,3),mes9 double (19,3),mes10 double (19,3), mes11 double (19,3),mes12 double (19,3)) ")
        tblcompras_jer = " select a.tipjer jerarquia," _
            & " sum(if(month(b.fechamov)=1,if( b.tipomov = 'EN',  b.peso,-1*if (b.tipomov <> 'AC', b.peso, 0) ),0)) mes1," _
            & " sum(if(month(b.fechamov)=2,if( b.tipomov = 'EN',  b.peso,-1*if (b.tipomov <> 'AC', b.peso, 0)),0)) mes2," _
            & " sum(if(month(b.fechamov)=3,if( b.tipomov = 'EN',  b.peso,-1*if (b.tipomov <> 'AC', b.peso, 0)),0)) mes3, " _
            & " sum(if(month(b.fechamov)=4,if( b.tipomov = 'EN',  b.peso,-1*if (b.tipomov <> 'AC', b.peso, 0)),0)) mes4," _
            & " sum(if(month(b.fechamov)=5,if( b.tipomov = 'EN',  b.peso,-1*if (b.tipomov <> 'AC', b.peso, 0)),0)) mes5," _
            & " sum(if(month(b.fechamov)=6,if( b.tipomov = 'EN',  b.peso,-1*if (b.tipomov <> 'AC', b.peso, 0)),0)) mes6," _
            & " sum(if(month(b.fechamov)=7,if( b.tipomov = 'EN',  b.peso,-1*if (b.tipomov <> 'AC', b.peso, 0)),0)) mes7," _
            & " sum(if(month(b.fechamov)=8,if( b.tipomov = 'EN',  b.peso,-1*if (b.tipomov <> 'AC', b.peso, 0)),0)) mes8," _
            & " sum(if(month(b.fechamov)=9,if( b.tipomov = 'EN',  b.peso,-1*if (b.tipomov <> 'AC', b.peso, 0)),0)) mes9," _
            & " sum(if(month(b.fechamov)=10,if( b.tipomov = 'EN',  b.peso,-1*if (b.tipomov <> 'AC', b.peso, 0)),0)) mes10," _
            & " sum(if(month(b.fechamov)=11,if( b.tipomov = 'EN',  b.peso,-1*if (b.tipomov <> 'AC', b.peso, 0)),0)) mes11," _
            & " sum(if(month(b.fechamov)=12,if( b.tipomov = 'EN',  b.peso,-1*if (b.tipomov <> 'AC', b.peso, 0)),0)) mes12 " _
            & " from jsmerctainv a " _
            & " left join jsmertramer b  on (a.codart=b.codart and a.id_emp=b.id_emp and b.origen in ('COM','NCC','NDC') and " _
            & " year(b.fechamov) = " & Año & ") " _
            & " left join jsmerencjer c on (a.tipjer=c.tipjer and a.id_emp=c.id_emp) " _
            & " where a.id_emp='" & jytsistema.WorkID & "' " _
            & " group by 1 "

        SeleccionSIGMEVentasyComprasMesaMes = "select a.tipjer, a.descrip, " _
            & " b.mes1 ventas1, c.mes1 compras1, if(d.jerarquia is null,0,d.mes1) as exist1, (if(d.jerarquia is null,0,d.mes1))/(b.mes1/e.mes1) as diasinv1, " _
            & " b.mes2 ventas2, c.mes2 compras2, if(d.jerarquia is null,0,d.mes2) as exist2, (if(d.jerarquia is null,0,d.mes2))/(b.mes2/e.mes2) as diasinv2, " _
            & " b.mes3 ventas3, c.mes3 compras3, if(d.jerarquia is null,0,d.mes3) as exist3, (if(d.jerarquia is null,0,d.mes3))/(b.mes3/e.mes3) as diasinv3, " _
            & " b.mes4 ventas4, c.mes4 compras4, if(d.jerarquia is null,0,d.mes4) as exist4, (if(d.jerarquia is null,0,d.mes4))/(b.mes4/e.mes4) as diasinv4, " _
            & " b.mes5 ventas5, c.mes5 compras5, if(d.jerarquia is null,0,d.mes5) as exist5, (if(d.jerarquia  is null,0,d.mes5))/(b.mes5/e.mes5) as diasinv5, " _
            & " b.mes6 ventas6, c.mes6 compras6, if(d.jerarquia is null,0,d.mes6) as exist6, (if(d.jerarquia  is null,0,d.mes6))/(b.mes6/e.mes6) as diasinv6, " _
            & " b.mes7 ventas7, c.mes7 compras7, if(d.jerarquia is null,0,d.mes7) as exist7, (if(d.jerarquia  is null,0,d.mes7))/(b.mes7/e.mes7) as diasinv7, " _
            & " b.mes8 ventas8, c.mes8 compras8, if(d.jerarquia is null,0,d.mes8) as exist8, (if(d.jerarquia  is null,0,d.mes8))/(b.mes8/e.mes8) as diasinv8, " _
            & " b.mes9 ventas9, c.mes9 compras9, if(d.jerarquia is null,0,d.mes9) as exist9, (if(d.jerarquia  is null,0,d.mes9))/(b.mes9/e.mes9) as diasinv9, " _
            & " b.mes10 ventas10, c.mes10 compras10, if(d.jerarquia is null,0,d.mes10) as exist10, (if(d.jerarquia  is null,0,d.mes10))/(b.mes10/e.mes10) as diasinv10, " _
            & " b.mes11 ventas11, c.mes11 compras11, if(d.jerarquia is null,0,d.mes11) as exist11, (if(d.jerarquia  is null,0,d.mes11))/(b.mes11/e.mes11) as diasinv11, " _
            & " b.mes12 ventas12, c.mes12 compras12, if(d.jerarquia is null,0,d.mes12) as exist12, (if(d.jerarquia  is null,0,d.mes12))/(b.mes12/e.mes12) as diasinv12 " _
            & " from jsmerencjer a " _
            & " left join (" & tblVentas_jer & ") b on (a.tipjer=b.jerarquia) " _
            & " left join (" & tblcompras_jer & ") c on (a.tipjer=c.jerarquia) " _
            & " left join (" & tblexistencias_jer & ") d on (a.tipjer=d.jerarquia) " _
            & " left join (select '1' marca, a.ano,a.mes1-b.mes1 mes1,a.mes2-b.mes2 mes2,a.mes3-b.mes3 mes3,a.mes4-b.mes4 mes4,a.mes5-b.mes5 mes5, a.mes6-b.mes6 mes6, a.mes7-b.mes7 mes7, a.mes8-b.mes8 mes8, a.mes9-b.mes9 mes9, a.mes10-b.mes10 mes10, a.mes11-b.mes11 mes11, a.mes12-b.mes12 mes12 from ( select year(inicio) ano ,31 mes1," & Microsoft.VisualBasic.DateAndTime.Day(UltimoDiaMes(CDate("01/02/" & Año))) & " mes2,31 mes3,30 mes4,31 mes5,30 mes6,31 mes7,31 mes8,30 mes9,31 mes10,30 mes11,31 mes12 from jsconctaemp where id_emp='" & jytsistema.WorkID & "' ) a, (select ano , count(if(mes=1,dia,null)) mes1,count(if(mes=2,dia,null)) mes2, count(if(mes=3,dia,null)) mes3, count(if(mes=4,dia,null)) mes4,count(if(mes=5,dia,null)) mes5, count(if(mes=6,dia,null)) mes6, count(if(mes=7,dia,null)) mes7, count(if(mes=8,dia,null)) mes8, count(if(mes=9,dia,null)) mes9, count(if(mes=10,dia,null)) mes10, count(if(mes=11,dia,null)) mes11, count(if(mes=12,dia,null)) mes12 from jsconcatper a,jsconctaemp b where a.MODULO = 1 AND a.ano=year(b.inicio) and a.id_emp='" & jytsistema.WorkID & "' and a.id_emp=b.id_emp group by 1 ) b where a.ano=b.ano ) e on (e.marca = '1') " _
            & " Where " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & strSQLTipJer _
            & "order by a.tipjer "

    End Function

    Function SeleccionSIGMEDescuentos(ByVal myConn As MySqlConnection, ByVal lblInfo As Label, ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                    Optional ByVal MercanciaDesde As String = "", Optional ByVal MercanciaHasta As String = "", Optional ByVal OrdenadoPor As String = "", _
                                    Optional ByVal ClienteDesde As String = "", Optional ByVal ClienteHasta As String = "", _
                                    Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                    Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                    Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                    Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                    Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                    Optional ByVal TiponegocioDesde As String = "", Optional ByVal TipoNegocioHasta As String = "", _
                                    Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                    Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                    Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                    Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                    Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                    Optional ByVal siMeses As Boolean = True, Optional ByVal TipoDescuento As Integer = 6) As String

        Dim strSQL As String = ""
        Dim strDescuentos As String = ""

        strSQL += CadenaComplementoMercancias("i", MercanciaDesde, MercanciaHasta, 0, "codart", TipoJerarquia, Nivel1, Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, _
                                              CategoriaDesde, CategoriaHasta, MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta)

        strSQL += CadenaComplementoClientes("b", ClienteDesde, ClienteHasta, 0, "codcli", CanalDesde, CanalHasta, TiponegocioDesde, TipoNegocioHasta, _
            ZonaDesde, ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, Parroquia, Ciudad, Barrio)

        Dim strSQLVendedor As String = ""
        If AsesorDesde <> "" Then strSQLVendedor += " a.vendedor >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then strSQLVendedor += " a.vendedor <= '" & AsesorHasta & "' and "

        Dim strSQLCodven As String = ""
        If AsesorDesde <> "" Then strSQLCodven += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then strSQLCodven += " a.codven <= '" & AsesorHasta & "' and "

        If TipoDescuento < 6 Then strSQL += " a.tipo = " & TipoDescuento & " AND "

        'tblDescuentos = "tbl" & ft.numeroaleatorio(10000), "00000")


        'ft.Ejecutar_strSQL ( myconn, " create temporary table " & tblDescuentos & " (tipo integer(1) default '0' not null, codcli varchar(15) not null, numdoc varchar(15) not null, codart varchar(15) not null, unidad char(3) not null, " _
        '    & " cantidad double(10,3) default '0.000' not null, peso double(10,3) default '0.000' not null, descuento double(19,2) default '0.00' not null, " _
        '    & " desc01 double(19,2) default '0.00' not null, desc02 double(19,2) default '0.00' not null, desc03 double(19,2) default '0.00' not null, " _
        '    & " desc04 double(19,2) default '0.00' not null, desc05 double(19,2) default '0.00' not null, desc06 double(19,2) default '0.00' not null, " _
        '    & " desc07 double(19,2) default '0.00' not null, desc08 double(19,2) default '0.00' not null, desc09 double(19,2) default '0.00' not null, " _
        '    & " desc10 double(19,2) default '0.00' not null, desc11 double(19,2) default '0.00' not null, desc12 double(19,2) default '0.00' not null) ")

        'ft.Ejecutar_strSQL ( myconn, " create index " & tblDescuentos & " on " & tblDescuentos & " (tipo, codcli, codart ) ")

        ' Descuentos Generales
        'ft.Ejecutar_strSQL ( myconn, " insert into " & tblDescuentos _
        strDescuentos = " (select 0 tipo, a.prov_cli codcli, a.numdoc, a.codart, b.unidad, " _
        & " sum(if(f.uvalencia is null,a.cantidad*((ventotal-ventotaldes)/ventotal),(a.cantidad/f.equivale)*((ventotal-ventotaldes)/ventotal))) as cantidad, " _
        & " sum(peso*((ventotal-ventotaldes)/ventotal)) as peso," _
        & " sum(a.ventotal-a.ventotaldes) as descuento, " _
        & " sum(if(month(a.fechamov)='01',a.ventotal-a.ventotaldes,0)) desc01, " _
        & " sum(if(month(a.fechamov)='02',a.ventotal-a.ventotaldes,0)) desc02, " _
        & " sum(if(month(a.fechamov)='03',a.ventotal-a.ventotaldes,0)) desc03, " _
        & " sum(if(month(a.fechamov)='04',a.ventotal-a.ventotaldes,0)) desc04, " _
        & " sum(if(month(a.fechamov)='05',a.ventotal-a.ventotaldes,0)) desc05, " _
        & " sum(if(month(a.fechamov)='06',a.ventotal-a.ventotaldes,0)) desc06, " _
        & " sum(if(month(a.fechamov)='07',a.ventotal-a.ventotaldes,0)) desc07, " _
        & " sum(if(month(a.fechamov)='08',a.ventotal-a.ventotaldes,0)) desc08, " _
        & " sum(if(month(a.fechamov)='09',a.ventotal-a.ventotaldes,0)) desc09, " _
        & " sum(if(month(a.fechamov)='10',a.ventotal-a.ventotaldes,0)) desc10, " _
        & " sum(if(month(a.fechamov)='11',a.ventotal-a.ventotaldes,0)) desc11, " _
        & " sum(if(month(a.fechamov)='12',a.ventotal-a.ventotaldes,0)) desc12 " _
        & " from jsmertramer a " _
        & " left join jsmerctainv b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
        & " left join jsmerequmer f on (a.codart=f.codart and a.unidad=f.uvalencia and a.id_emp= f.id_emp) " _
        & " Where " _
        & strSQLVendedor _
        & " a.ventotal <> 0.00 and " _
        & " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaDesde) & "' and " _
        & " a.fechamov <=  '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' and " _
        & " a.origen in ('FAC','PFC') and " _
        & " a.id_emp = '" & jytsistema.WorkID & "' " _
        & " GROUP by 2, 3, 4) "

        ' Bonificaciones
        strDescuentos += " union  (select 1 tipo, a.codcli, a.numfac numdoc, b.item codart , d.unidad, " _
        & " sum(if(c.codart is null,b.cantidad,b.cantidad/c.equivale)) as cantidad, sum(b.peso) as peso, sum(b.precio*b.cantidad) as descuento, " _
        & " sum(if(month(a.emision)='01',b.precio*b.cantidad,0)) as desc01, " _
        & " sum(if(month(a.emision)='02',b.precio*b.cantidad,0)) as desc02, " _
        & " sum(if(month(a.emision)='03',b.precio*b.cantidad,0)) as desc03, " _
        & " sum(if(month(a.emision)='04',b.precio*b.cantidad,0)) as desc04, " _
        & " sum(if(month(a.emision)='05',b.precio*b.cantidad,0)) as desc05, " _
        & " sum(if(month(a.emision)='06',b.precio*b.cantidad,0)) as desc06, " _
        & " sum(if(month(a.emision)='07',b.precio*b.cantidad,0)) as desc07, " _
        & " sum(if(month(a.emision)='08',b.precio*b.cantidad,0)) as desc08, " _
        & " sum(if(month(a.emision)='09',b.precio*b.cantidad,0)) as desc09, " _
        & " sum(if(month(a.emision)='10',b.precio*b.cantidad,0)) as desc10, " _
        & " sum(if(month(a.emision)='11',b.precio*b.cantidad,0)) as desc11, " _
        & " sum(if(month(a.emision)='12',b.precio*b.cantidad,0)) as desc12 " _
        & " from jsvenencfac a " _
        & " left join jsvenrenfac b on (a.numfac=b.numfac and a.ejercicio=b.ejercicio and a.id_emp=b.id_emp) " _
        & " left join jsmerctainv d on (b.item=d.codart and b.id_emp=d.id_emp) " _
        & " left join jsmerequmer c on (b.item=c.codart and b.unidad=c.uvalencia and b.id_emp=c.id_emp) " _
        & " Where " _
        & strSQLCodven _
        & " b.estatus = '2' AND a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
        & " a.id_emp = '" & jytsistema.WorkID & "' group by 2,3,4 )"

        strDescuentos += " union (select 2 tipo, a.codcli, a.numfac numdoc, b.item codart, d.unidad, " _
            & " sum(if(c.codart is null,(b.cantidad-b.cantidad*(b.des_art/100))*(b.des_cli/100),((b.cantidad/c.equivale)-(b.cantidad/c.equivale)*(b.des_art/100))*(b.des_cli/100))) as cantidad, sum((b.peso-b.peso*(b.des_art/100))*(b.des_cli/100)) as peso, sum((b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100)) as descuento, " _
            & " sum(if (month(a.emision)='01',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100),0)) as desc01, " _
            & " sum(if (month(a.emision)='02',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100),0)) as desc02, " _
            & " sum(if (month(a.emision)='03',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100),0)) as desc03, " _
            & " sum(if (month(a.emision)='04',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100),0)) as desc04, " _
            & " sum(if (month(a.emision)='05',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100),0)) as desc05, " _
            & " sum(if (month(a.emision)='06',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100),0)) as desc06, " _
            & " sum(if (month(a.emision)='07',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100),0)) as desc07, " _
            & " sum(if (month(a.emision)='08',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100),0)) as desc08, " _
            & " sum(if (month(a.emision)='09',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100),0)) as desc09, " _
            & " sum(if (month(a.emision)='10',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100),0)) as desc10, " _
            & " sum(if (month(a.emision)='11',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100),0)) as desc11, " _
            & " sum(if (month(a.emision)='12',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100),0)) as desc12 " _
            & " from jsvenencfac a " _
            & " left join jsvenrenfac b on (a.numfac=b.numfac and a.ejercicio=b.ejercicio and a.id_emp=b.id_emp) " _
            & " left join jsmerctainv d on (b.item=d.codart and b.id_emp=d.id_emp) " _
            & " left join jsmerequmer c on (b.item=c.codart and b.unidad=c.uvalencia and b.id_emp=c.id_emp) " _
            & " Where " _
            & strSQLCodven _
            & " b.des_cli <> 0.00 and b.estatus <> '2' and " _
            & " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and " _
            & " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
            & " a.id_emp='" & jytsistema.WorkID & "' group by 2,3,4  )  "

        strDescuentos += " union (select 3 tipo, a.codcli, a.numfac numdoc, b.item codart, d.unidad, sum(if(c.codart is null,b.cantidad*(b.des_art/100),b.cantidad/c.equivale*(b.des_art/100))) as cantidad, sum(b.peso*(b.des_art/100)) as peso, sum(b.precio*b.cantidad*(b.des_art/100)) as descuento, " _
            & " sum(if(month(a.emision)='01',(b.precio*b.cantidad*(b.des_art/100)),0)) desc01, " _
            & " sum(if(month(a.emision)='02',(b.precio*b.cantidad*(b.des_art/100)),0)) desc02, " _
            & " sum(if(month(a.emision)='03',(b.precio*b.cantidad*(b.des_art/100)),0)) desc03, " _
            & " sum(if(month(a.emision)='04',(b.precio*b.cantidad*(b.des_art/100)),0)) desc04, " _
            & " sum(if(month(a.emision)='05',(b.precio*b.cantidad*(b.des_art/100)),0)) desc05, " _
            & " sum(if(month(a.emision)='06',(b.precio*b.cantidad*(b.des_art/100)),0)) desc06, " _
            & " sum(if(month(a.emision)='07',(b.precio*b.cantidad*(b.des_art/100)),0)) desc07, " _
            & " sum(if(month(a.emision)='08',(b.precio*b.cantidad*(b.des_art/100)),0)) desc08, " _
            & " sum(if(month(a.emision)='09',(b.precio*b.cantidad*(b.des_art/100)),0)) desc09, " _
            & " sum(if(month(a.emision)='10',(b.precio*b.cantidad*(b.des_art/100)),0)) desc10, " _
            & " sum(if(month(a.emision)='11',(b.precio*b.cantidad*(b.des_art/100)),0)) desc11, " _
            & " sum(if(month(a.emision)='12',(b.precio*b.cantidad*(b.des_art/100)),0)) desc12 " _
            & " from jsvenencfac a " _
            & " left join jsvenrenfac b on (a.numfac=b.numfac and a.ejercicio=b.ejercicio and a.id_emp=b.id_emp) " _
            & " left join jsmerctainv d on (b.item=d.codart and b.id_emp=d.id_emp) " _
            & " left join jsmerequmer c on (b.item=c.codart and b.unidad=c.uvalencia and b.id_emp=c.id_emp) " _
            & " Where " _
            & strSQLCodven _
            & " b.des_art <> 0.00 and b.estatus <> '2' and " _
            & " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and " _
            & " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' group by 2,3,4 )"


        strDescuentos += " union  (select 4 tipo, a.codcli, a.numfac numdoc, b.item codart, d.unidad, " _
            & " sum(if(c.codart is null, b.cantidad*b.des_ofe/100- b.cantidad*des_art*b.des_ofe/10000 - b.cantidad*b.des_cli*b.des_ofe/10000 + b.cantidad*des_art*b.des_cli*b.des_ofe/1000000 , (b.cantidad/c.equivale)*b.des_ofe/100- (b.cantidad/c.equivale)*des_art*b.des_ofe/10000 - (b.cantidad/c.equivale)*b.des_cli*b.des_ofe/10000 + (b.cantidad/c.equivale)*des_art*b.des_cli*b.des_ofe/1000000)) as cantidad, " _
            & " sum( b.peso*b.des_ofe/100 - b.peso*b.des_art*b.des_ofe/10000 - b.peso*b.des_cli*b.des_ofe/10000 + b.peso*b.des_art*b.des_cli*b.des_ofe/1000000 ) as peso, " _
            & " sum( b.precio*b.cantidad - b.precio*b.cantidad*b.des_art/100 - b.precio*b.cantidad*b.des_cli/100 + b.precio*b.cantidad*b.des_art*b.des_cli/10000 - b.totren ) descuento, " _
            & " sum(if (month(a.emision)='01',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100)-(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100))*(b.des_ofe/100),0)) desc01, " _
            & " sum(if( month(a.emision)='02',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100)-(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100))*(b.des_ofe/100),0)) desc02, " _
            & " sum(if (month(a.emision)='03',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100)-(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100))*(b.des_ofe/100),0)) desc03, " _
            & " sum(if (month(a.emision)='04',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100)-(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100))*(b.des_ofe/100),0)) desc04, " _
            & " sum(if (month(a.emision)='05',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100)-(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100))*(b.des_ofe/100),0)) desc05, " _
            & " sum(if (month(a.emision)='06',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100)-(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100))*(b.des_ofe/100),0)) desc06, " _
            & " sum(if (month(a.emision)='07',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100)-(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100))*(b.des_ofe/100),0)) desc07, " _
            & " sum(if (month(a.emision)='08',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100)-(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100))*(b.des_ofe/100),0)) desc08, " _
            & " sum(if (month(a.emision)='09',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100)-(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100))*(b.des_ofe/100),0)) desc09, " _
            & " sum(if (month(a.emision)='10',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100)-(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100))*(b.des_ofe/100),0)) desc10, " _
            & " sum(if (month(a.emision)='11',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100)-(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100))*(b.des_ofe/100),0)) desc11, " _
            & " sum(if (month(a.emision)='12',(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100)-(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_art/100))*(b.des_cli/100))*(b.des_ofe/100),0)) desc12 " _
            & " from jsvenencfac a " _
            & " left join jsvenrenfac b on (a.numfac=b.numfac and a.ejercicio=b.ejercicio and a.id_emp=b.id_emp) " _
            & " left join jsmerctainv d on (b.item=d.codart and b.id_emp=d.id_emp) " _
            & " left join jsmerequmer c on (b.item=c.codart and b.unidad=c.uvalencia and b.id_emp=c.id_emp) " _
            & " Where " _
            & strSQLCodven _
            & " b.des_ofe <> 0.00 and b.estatus <> '2' and a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' group by 2,3,4 ) "


        strDescuentos += " union (select  5 tipo, a.codcli , a.numncr numdoc, b.item codart , d.unidad, " _
                & " sum(if(c.codart is null,(b.cantidad*(b.des_cli/100)+(b.cantidad-(b.cantidad*(b.des_cli/100)))*(b.des_art/100)+(b.cantidad-b.cantidad*(b.des_cli/100)-(b.cantidad-(b.cantidad*(b.des_cli/100)))*(b.des_art/100))*(b.des_ofe/100))*(-1)*(b.por_acepta_dev/100),((b.cantidad/c.equivale)*(b.des_cli/100)+((b.cantidad/c.equivale)-((b.cantidad/c.equivale)*(b.des_cli/100)))*(b.des_art/100)+((b.cantidad/c.equivale)-(b.cantidad/c.equivale)*(b.des_cli/100)-((b.cantidad/c.equivale)-((b.cantidad/c.equivale)*(b.des_cli/100)))*(b.des_art/100))*(b.des_ofe/100))*(-1)*(b.por_acepta_dev/100))) as cantidad, sum((b.peso*(b.des_cli/100)+(b.peso-(b.peso*(b.des_cli/100)))*(b.des_art/100)+(b.peso-b.peso*(b.des_cli/100)-(b.peso-(b.peso*(b.des_cli/100)))*(b.des_art/100))*(b.des_ofe/100))*(-1)*(b.por_acepta_dev/100)) as peso, " _
                & " sum((b.precio*b.cantidad*(b.des_cli/100)+(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100)+(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_cli/100)-(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100))*(b.des_ofe/100))*(-1)*(b.por_acepta_dev/100)) as descuento, " _
                & " sum(if (month(a.emision)='01',(b.precio*b.cantidad*(b.des_cli/100)+(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100)+(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_cli/100)-(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100))*(b.des_ofe/100))*(-1)*(b.por_acepta_dev/100),0)) desc01, " _
                & " sum(if (month(a.emision)='02',(b.precio*b.cantidad*(b.des_cli/100)+(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100)+(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_cli/100)-(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100))*(b.des_ofe/100))*(-1)*(b.por_acepta_dev/100),0)) desc02, " _
                & " sum(if (month(a.emision)='03',(b.precio*b.cantidad*(b.des_cli/100)+(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100)+(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_cli/100)-(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100))*(b.des_ofe/100))*(-1)*(b.por_acepta_dev/100),0)) desc03, " _
                & " sum(if (month(a.emision)='04',(b.precio*b.cantidad*(b.des_cli/100)+(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100)+(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_cli/100)-(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100))*(b.des_ofe/100))*(-1)*(b.por_acepta_dev/100),0)) desc04, " _
                & " sum(if (month(a.emision)='05',(b.precio*b.cantidad*(b.des_cli/100)+(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100)+(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_cli/100)-(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100))*(b.des_ofe/100))*(-1)*(b.por_acepta_dev/100),0)) desc05, " _
                & " sum(if (month(a.emision)='06',(b.precio*b.cantidad*(b.des_cli/100)+(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100)+(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_cli/100)-(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100))*(b.des_ofe/100))*(-1)*(b.por_acepta_dev/100),0)) desc06, " _
                & " sum(if (month(a.emision)='07',(b.precio*b.cantidad*(b.des_cli/100)+(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100)+(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_cli/100)-(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100))*(b.des_ofe/100))*(-1)*(b.por_acepta_dev/100),0)) desc07, " _
                & " sum(if (month(a.emision)='08',(b.precio*b.cantidad*(b.des_cli/100)+(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100)+(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_cli/100)-(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100))*(b.des_ofe/100))*(-1)*(b.por_acepta_dev/100),0)) desc08, " _
                & " sum(if (month(a.emision)='09',(b.precio*b.cantidad*(b.des_cli/100)+(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100)+(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_cli/100)-(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100))*(b.des_ofe/100))*(-1)*(b.por_acepta_dev/100),0)) desc09, " _
                & " sum(if (month(a.emision)='10',(b.precio*b.cantidad*(b.des_cli/100)+(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100)+(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_cli/100)-(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100))*(b.des_ofe/100))*(-1)*(b.por_acepta_dev/100),0)) desc10, " _
                & " sum(if (month(a.emision)='11',(b.precio*b.cantidad*(b.des_cli/100)+(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100)+(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_cli/100)-(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100))*(b.des_ofe/100))*(-1)*(b.por_acepta_dev/100),0)) desc11, " _
                & " sum(if (month(a.emision)='12',(b.precio*b.cantidad*(b.des_cli/100)+(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100)+(b.precio*b.cantidad-b.precio*b.cantidad*(b.des_cli/100)-(b.precio*b.cantidad-(b.precio*b.cantidad*(b.des_cli/100)))*(b.des_art/100))*(b.des_ofe/100))*(-1)*(b.por_acepta_dev/100),0)) desc12 " _
                & " from jsvenencncr a " _
                & " left join jsvenrenncr b on (a.numncr=b.numncr and a.ejercicio=b.ejercicio and a.id_emp=b.id_emp) " _
                & " left join jsmerctainv d on (b.item=d.codart and b.id_emp=d.id_emp) " _
                & " left join jsmerequmer c on (b.item=c.codart and b.unidad=c.uvalencia and b.id_emp=c.id_emp) " _
                & " Where " _
                & strSQLCodven _
                & " (b.des_ofe<>0 or  b.des_cli<> 0 or b.des_art<>0) and " _
                & " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and  a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
                & " a.id_emp='" & jytsistema.WorkID & "' " _
                & " Group By 2,3,4 ) "

        SeleccionSIGMEDescuentos = "select elt(a.tipo+1, 'Descuentos Generales','Bonificaciones', " _
                & " 'Descuentos Clientes','Descuentos Mercancías','Descuentos Ofertas', 'Descuentos Devoluciones') tipo, " _
                & " a.codcli, b.nombre, c.descrip canal, d.descrip tiponegocio, e.descrip zona, f.nomrut ruta, concat(g.apellidos, ' ', g.nombres) asesor, " _
                & " if( q.nombre is null, '', q.nombre) FBARRIO, if( r.nombre is null, '', r.nombre) FCIUDAD, if( s.nombre is null, '', s.nombre) FPARROQUIA, " _
                & " if( t.nombre is null, '', t.nombre) FMUNICIPIO, if(u.nombre is null, '', u.nombre) FESTADO, if(v.nombre is null, '', v.nombre) FPAIS, a.numdoc, " _
                & " a.codart, i.nomart, a.unidad, elt(mix+1,'Económico','Estandar','Superior') mix, j.descrip categoria, k.descrip marca, l.descrip tipjer,  " _
                & " l.descrip tipjer, n.descrip division, " _
                & " a.cantidad,  a.peso, a.descuento descuento, a.desc01 desc01, a.desc02 desc02, a.desc03 desc03, a.desc04 desc04, a.desc05 desc05, a.desc06 desc06, " _
                & " a.desc07 desc07, a.desc08 desc08, a.desc09 desc09, a.desc10 desc10, a.desc11 desc11, a.desc12 desc12 " _
                & " from (" & strDescuentos & ") a " _
                & " left join jsvencatcli b on (a.codcli = b.codcli) " _
                & " left join jsvenliscan c on (b.categoria = c.codigo and b.id_emp = c.id_emp)" _
                & " left join jsvenlistip d on (b.unidad = d.codigo and b.id_emp = d.id_emp)" _
                & " left join jsconctatab e on (b.zona = e.codigo and b.id_emp = e.id_emp and e.modulo = '00005') " _
                & " left join jsvenencrut f on (b.ruta_visita = f.codrut and b.id_emp = f.id_emp and f.tipo = '0') " _
                & " left join jsvencatven g on (b.vendedor = g.codven and b.id_emp = g.id_emp and g.tipo = '0') " _
                & " left join jsconcatter q on (b.fbarrio = q.codigo and b.id_emp = q.id_emp ) " _
                & " left join jsconcatter r on (b.fciudad = r.codigo and b.id_emp = r.id_emp ) " _
                & " left join jsconcatter s on (b.fparroquia = s.codigo and b.id_emp = s.id_emp ) " _
                & " left join jsconcatter t on (b.fmunicipio = t.codigo and b.id_emp = t.id_emp ) " _
                & " left join jsconcatter u on (b.festado = u.codigo and b.id_emp = u.id_emp ) " _
                & " left join jsconcatter v on (b.fpais = v.codigo and b.id_emp = v.id_emp ) " _
                & " left join jsmerctainv i on (a.codart = i.codart)  " _
                & " left join jsconctatab j on (i.grupo = j.codigo and i.id_emp = j.id_emp and j.modulo = '00002') " _
                & " left join jsconctatab k on (i.marca = k.codigo and i.id_emp = k.id_emp and k.modulo = '00003') " _
                & " left join jsmerencjer l on (i.tipjer = l.tipjer and i.id_emp = l.id_emp) " _
                & " left join jsmercatdiv n on (i.division = n.division and i.id_emp = n.id_emp) " _
                & " where " _
                & strSQL _
                & " b.id_emp = '" & jytsistema.WorkID & "' and  " _
                & " i.id_emp = '" & jytsistema.WorkID & "' and a.descuento <> 0.00 " _
                & " order by a.tipo, a.codcli, a.numdoc, a.codart "

    End Function
    Function SeleccionSIGMEDevolucionesMesMes(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label,
                        ByVal AsesorDesde As String, ByVal AsesorHasta As String, ByVal TipoJerarquia As String, _
                        ByVal CategoriaDesde As String, ByVal CategoriaHasta As String, ByVal MarcaDesde As String, ByVal MarcaHasta As String, _
                        ByVal DivisionDesde As String, ByVal DivisionHasta As String, ByVal Año As Integer, _
                        ByVal AgrupadoPor As String, Optional CantidadPesoMoneda As Integer = 0) As String

        Dim strSQLAsesor As String = ""
        Dim strSQLTipJer As String = ""
        Dim strSQLGrupo As String = ""

        Select Case AgrupadoPor
            Case "Ninguno"
                strSQLGrupo = " a.vendedor "
            Case "Categorías"
                strSQLGrupo = " b.grupo, a.vendedor "
            Case "Marcas"
                strSQLGrupo = " b.marca, a.vendedor "
            Case "Jerarquía"
                strSQLGrupo = " b.tipjer, a.vendedor "
            Case "División"
                strSQLGrupo = " b.division, a.vendedor "
            Case "Mix"
                strSQLGrupo = " b.mix, a.vendedor "
        End Select

        Dim strCan As String = ""
        Dim strUnidad As String = ""
        Dim strUND As String = ""

        Select Case CantidadPesoMoneda
            Case 0 'UNIDAD DE VENTA
                strCan = " a.cantidad/n.equivale "
                strUnidad = " a.unidad "
                strUND = " c.unidad "
            Case 1 'UNIDAD DE MEDIDA PRINCIPAL UMP (KGR)
                strCan = " a.peso "
                strUnidad = " a.unidad "
                strUND = " 'KGR' "
            Case 2 'MONETARIOS
                strCan = " a.ventotaldes "
                strUnidad = " a.unidad "
                strUND = " 'BsF' "
            Case 3 'UNIDAD DE MEDIDA SECUNDARIA UMS (CAJ)
                strCan = " a.peso/b.pesounidad*n.equivale "
                strUnidad = " 'CAJ' "
                strUND = " 'CAJ' "
        End Select


        If AsesorDesde <> "" Then strSQLAsesor += " and a.vendedor >= '" & AsesorDesde & "' "
        If AsesorHasta <> "" Then strSQLAsesor += " and a.vendedor <= '" & AsesorHasta & "' "

        If TipoJerarquia <> "" Then strSQLTipJer += " and b.tipjer = '" & TipoJerarquia & "' "
        If CategoriaDesde <> "" Then strSQLTipJer += " and b.grupo >= '" & CategoriaDesde & "' "
        If CategoriaHasta <> "" Then strSQLTipJer += " and b.grupo <= '" & CategoriaHasta & "' "
        If MarcaDesde <> "" Then strSQLTipJer += " and b.marca >= '" & MarcaDesde & "' "
        If MarcaHasta <> "" Then strSQLTipJer += " and b.marca <= '" & MarcaHasta & "' "
        If DivisionDesde <> "" Then strSQLTipJer += " and b.division >= '" & DivisionDesde & "' "
        If DivisionHasta <> "" Then strSQLTipJer += " and b.division <= '" & DivisionHasta & "' "

        SeleccionSIGMEDevolucionesMesMes = " select a.vendedor,  concat(c.apellidos,', ',c.nombres) as nombre_vendedor, b.tipjer, d.descrip,  " _
                & " sum(if( MONTH(a.fechamov)= 1, " & strCan & ", 0)) bs1, " _
                & " sum(if( MONTH(a.fechamov)= 2, " & strCan & ", 0)) bs2, " _
                & " sum(if( MONTH(a.fechamov)= 3, " & strCan & ", 0)) bs3, " _
                & " sum(if( MONTH(a.fechamov)= 4, " & strCan & ", 0)) bs4, " _
                & " sum(if( MONTH(a.fechamov)= 5, " & strCan & ", 0)) bs5, " _
                & " sum(if( MONTH(a.fechamov)= 6, " & strCan & ", 0)) bs6, " _
                & " sum(if( MONTH(a.fechamov)= 7, " & strCan & ", 0)) bs7, " _
                & " sum(if( MONTH(a.fechamov)= 8, " & strCan & ", 0)) bs8, " _
                & " sum(if( MONTH(a.fechamov)= 9, " & strCan & ", 0)) bs9, " _
                & " sum(if( MONTH(a.fechamov)= 10, " & strCan & ", 0)) bs10, " _
                & " sum(if( MONTH(a.fechamov)= 11, " & strCan & ", 0)) bs11, " _
                & " sum(if( MONTH(a.fechamov)= 12, " & strCan & ", 0)) bs12 " _
                & " from jsmertramer a " _
                & " left join (SELECT a.codart, a.unidad, a.equivale, a.uvalencia, a.id_emp  " _
                & " 			FROM jsmerequmer a  " _
                & "             WHERE " _
                & " 			a.id_emp = '" & jytsistema.WorkID & "' " _
                & "             UNION " _
                & " 			SELECT a.codart, a.unidad, 1 equivale, a.unidad uvalencia, a.id_emp " _
                & " 			FROM jsmerctainv a " _
                & " 			WHERE  " _
                & " 			a.id_emp = '" & jytsistema.WorkID & "') n on (a.codart = n.codart and " & strUnidad & " = n.uvalencia and a.id_emp = n.id_emp) " _
                & " left join jsmerctainv b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
                & " left join jsvencatven c on (a.vendedor = c.codven and a.id_emp = c.id_emp) " _
                & " left join jsmerencjer d on (b.tipjer = d.tipjer and b.id_emp = d.id_emp) " _
                & " where a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(PrimerDiaAño(CDate("01/01/" & Año.ToString))) & "' " _
                & " and a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(UltimoDiaAño(CDate("01/01/" & Año.ToString))) & "' " _
                & " and a.id_emp='" & jytsistema.WorkID & "' " _
                & " and a.origen in ('NCV') " _
                & strSQLAsesor & strSQLTipJer _
                & " group by " & strSQLGrupo

    End Function


    Function SeleccionSIGMEVentasMesMes(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label,
                            ByVal AsesorDesde As String, ByVal AsesorHasta As String, ByVal TipoJerarquia As String, _
                            ByVal CategoriaDesde As String, ByVal CategoriaHasta As String, ByVal MarcaDesde As String, ByVal MarcaHasta As String, _
                            ByVal DivisionDesde As String, ByVal DivisionHasta As String, ByVal Año As Integer, _
                            ByVal AgrupadoPor As String, Optional CantidadPesoMoneda As Integer = 0) As String

        Dim strSQLAsesor As String = ""
        Dim strSQLTipJer As String = ""
        Dim strSQLGrupo As String = ""

        Select Case AgrupadoPor
            Case "Ninguno"
                strSQLGrupo = " a.vendedor "
            Case "Categorías"
                strSQLGrupo = " b.grupo, a.vendedor "
            Case "Marcas"
                strSQLGrupo = " b.marca, a.vendedor "
            Case "Jerarquía"
                strSQLGrupo = " b.tipjer, a.vendedor "
            Case "División"
                strSQLGrupo = " b.division, a.vendedor "
            Case "Mix"
                strSQLGrupo = " b.mix, a.vendedor "
        End Select

        Dim strCan As String = ""
        Dim strUnidad As String = ""
        Dim strUND As String = ""

        Select Case CantidadPesoMoneda
            Case 0 'UNIDAD DE VENTA
                strCan = " a.cantidad/n.equivale "
                strUnidad = " a.unidad "
                strUND = " c.unidad "
            Case 1 'UNIDAD DE MEDIDA PRINCIPAL UMP (KGR)
                strCan = " a.peso "
                strUnidad = " a.unidad "
                strUND = " 'KGR' "
            Case 2 'MONETARIOS VENTAS
                strCan = " a.ventotaldes "
                strUnidad = " a.unidad "
                strUND = " 'BsF' "
            Case 3 'UNIDAD DE MEDIDA SECUNDARIA UMS (CAJ)
                strCan = " a.peso/b.pesounidad*n.equivale "
                strUnidad = " 'CAJ' "
                strUND = " 'CAJ' "
            Case 4 'MONETARIOS COSTOS
                strCan = " a.COSTOTALDES "
                strUnidad = " a.unidad "
                strUND = " 'BsF' "
        End Select



        If AsesorDesde <> "" Then strSQLAsesor += " and a.vendedor >= '" & AsesorDesde & "' "
        If AsesorHasta <> "" Then strSQLAsesor += " and a.vendedor <= '" & AsesorHasta & "' "

        If TipoJerarquia <> "" Then strSQLTipJer += " and b.tipjer = '" & TipoJerarquia & "' "
        If CategoriaDesde <> "" Then strSQLTipJer += " and b.grupo >= '" & CategoriaDesde & "' "
        If CategoriaHasta <> "" Then strSQLTipJer += " and b.grupo <= '" & CategoriaHasta & "' "
        If MarcaDesde <> "" Then strSQLTipJer += " and b.marca >= '" & MarcaDesde & "' "
        If MarcaHasta <> "" Then strSQLTipJer += " and b.marca <= '" & MarcaHasta & "' "
        If DivisionDesde <> "" Then strSQLTipJer += " and b.division >= '" & DivisionDesde & "' "
        If DivisionHasta <> "" Then strSQLTipJer += " and b.division <= '" & DivisionHasta & "' "

        SeleccionSIGMEVentasMesMes = " select a.vendedor,  concat(c.apellidos,', ',c.nombres) as nombre_vendedor, b.tipjer, d.descrip,  " _
                & " sum(if( MONTH(a.fechamov)= 1, IF(a.origen = 'NCV', 0, 1)*" & strCan & ", 0)) bs1, " _
                & " sum(if( MONTH(a.fechamov)= 2, IF(a.origen = 'NCV', 0, 1)*" & strCan & ", 0)) bs2, " _
                & " sum(if( MONTH(a.fechamov)= 3, IF(a.origen = 'NCV', 0, 1)*" & strCan & ", 0)) bs3, " _
                & " sum(if( MONTH(a.fechamov)= 4, IF(a.origen = 'NCV', 0, 1)*" & strCan & ", 0)) bs4, " _
                & " sum(if( MONTH(a.fechamov)= 5, IF(a.origen = 'NCV', 0, 1)*" & strCan & ", 0)) bs5, " _
                & " sum(if( MONTH(a.fechamov)= 6, IF(a.origen = 'NCV', 0, 1)*" & strCan & ", 0)) bs6, " _
                & " sum(if( MONTH(a.fechamov)= 7, IF(a.origen = 'NCV', 0, 1)*" & strCan & ", 0)) bs7, " _
                & " sum(if( MONTH(a.fechamov)= 8, IF(a.origen = 'NCV', 0, 1)*" & strCan & ", 0)) bs8, " _
                & " sum(if( MONTH(a.fechamov)= 9, IF(a.origen = 'NCV', 0, 1)*" & strCan & ", 0)) bs9, " _
                & " sum(if( MONTH(a.fechamov)= 10, IF(a.origen = 'NCV', 0, 1)*" & strCan & ", 0)) bs10, " _
                & " sum(if( MONTH(a.fechamov)= 11, IF(a.origen = 'NCV', 0, 1)*" & strCan & ", 0)) bs11, " _
                & " sum(if( MONTH(a.fechamov)= 12, IF(a.origen = 'NCV', 0, 1)*" & strCan & ", 0)) bs12, " _
                & " sum(if( MONTH(a.fechamov)= 1, IF(a.origen = 'NCV', 1, 0)*" & strCan & ", 0)) dev1, " _
                & " sum(if( MONTH(a.fechamov)= 2, IF(a.origen = 'NCV', 1, 0)*" & strCan & ", 0)) dev2, " _
                & " sum(if( MONTH(a.fechamov)= 3, IF(a.origen = 'NCV', 1, 0)*" & strCan & ", 0)) dev3, " _
                & " sum(if( MONTH(a.fechamov)= 4, IF(a.origen = 'NCV', 1, 0)*" & strCan & ", 0)) dev4, " _
                & " sum(if( MONTH(a.fechamov)= 5, IF(a.origen = 'NCV', 1, 0)*" & strCan & ", 0)) dev5, " _
                & " sum(if( MONTH(a.fechamov)= 6, IF(a.origen = 'NCV', 1, 0)*" & strCan & ", 0)) dev6, " _
                & " sum(if( MONTH(a.fechamov)= 7, IF(a.origen = 'NCV', 1, 0)*" & strCan & ", 0)) dev7, " _
                & " sum(if( MONTH(a.fechamov)= 8, IF(a.origen = 'NCV', 1, 0)*" & strCan & ", 0)) dev8, " _
                & " sum(if( MONTH(a.fechamov)= 9, IF(a.origen = 'NCV', 1, 0)*" & strCan & ", 0)) dev9, " _
                & " sum(if( MONTH(a.fechamov)= 10, IF(a.origen = 'NCV', 1, 0)*" & strCan & ", 0)) dev10, " _
                & " sum(if( MONTH(a.fechamov)= 11, IF(a.origen = 'NCV', 1, 0)*" & strCan & ", 0)) dev11, " _
                & " sum(if( MONTH(a.fechamov)= 12, IF(a.origen = 'NCV', 1, 0)*" & strCan & ", 0)) dev12 " _
                & " from jsmertramer a " _
                & " left join (" & SeleccionGENTablaEquivalencias() & ") n on (a.codart = n.codart and " & strUnidad & " = n.uvalencia and a.id_emp = n.id_emp) " _
                & " left join jsmerctainv b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
                & " left join jsvencatven c on (a.vendedor = c.codven and a.id_emp = c.id_emp) " _
                & " left join jsmerencjer d on (b.tipjer = d.tipjer and b.id_emp = d.id_emp) " _
                & " where a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(PrimerDiaAño(CDate("01/01/" & Año.ToString))) & "' " _
                & " and a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(UltimoDiaAño(CDate("01/01/" & Año.ToString))) & "' " _
                & " and a.id_emp='" & jytsistema.WorkID & "' " _
                & " and a.origen in ('FAC','PFC','PVE','NDV','NCV') " _
                & strSQLAsesor & strSQLTipJer _
                & " group by " & strSQLGrupo

    End Function

    Function SeleccionSIGMEGananciasCestaTicketMesMes(ByVal myConn As MySqlConnection, ByVal lblInfo As Label, ByVal Año As Integer) As String


        ft.Ejecutar_strSQL(myconn, " create temporary table sobres (corredor char(5), fechamov date,numsobre varchar(15))")
        ft.Ejecutar_strSQL(myconn, " insert into sobres " _
        & " select b.corredor,a.fechamov,a.numdoc " _
         & " from jsbantraban a, jsventabtic b " _
         & " where year(a.fechamov)='" & Año & "' " _
         & " and a.id_emp='" & jytsistema.WorkID & "' " _
         & " and a.tiporg='CT' " _
         & " and a.id_emp=b.id_emp " _
         & " and a.numdoc=b.numdep " _
         & " group by 3 ")

        ft.Ejecutar_strSQL(myconn, " create temporary table iva (corredor char(5), mes1 double(16,2),mes2 double(16,2), " _
         & " mes3 double(16,2),mes4 double(16,2),mes5 double(16,2),mes6 double(16,2),mes7 double(16,2)," _
         & " mes8 double(16,2),mes9 double(16,2),mes10 double(16,2),mes11 double(16,2),mes12 double(16,2)) ")

        ft.Ejecutar_strSQL(myconn, " insert into iva " _
         & " select a.corredor,sum(if(month(a.fechamov)='01',b.imp_iva,0)), " _
         & " sum(if(month(a.fechamov)='02',b.imp_iva,0)),sum(if(month(a.fechamov)='03',b.imp_iva,0)), " _
         & " sum(if(month(a.fechamov)='04',b.imp_iva,0)),sum(if(month(a.fechamov)='05',b.imp_iva,0)), " _
         & " sum(if(month(a.fechamov)='06',b.imp_iva,0)),sum(if(month(a.fechamov)='07',b.imp_iva,0)), " _
         & " sum(if(month(a.fechamov)='08',b.imp_iva,0)),sum(if(month(a.fechamov)='09',b.imp_iva,0)), " _
         & " sum(if(month(a.fechamov)='10',b.imp_iva,0)),sum(if(month(a.fechamov)='11',b.imp_iva,0)), " _
         & " sum(if(month(a.fechamov)='12',b.imp_iva,0)) " _
         & " from sobres a,jsproencgas b where b.id_emp='" & jytsistema.WorkID & "' " _
         & " and a.numsobre=b.numpag and a.fechamov=b.emision group by 1 ")


        ft.Ejecutar_strSQL(myconn, " create temporary table comisiones (corredor char(5), mes1 integer, " _
        & " mes2 integer,mes3 integer,mes4 integer,mes5 integer,mes6 integer, " _
        & " mes7 integer,mes8 integer,mes9 integer,mes10 integer,mes11 integer,mes12 integer)")

        ft.Ejecutar_strSQL(myconn, " insert into comisiones " _
         & " select a.corredor,count(if(month(a.fechamov)='01',a.numsobre,null))*b.cargos, " _
        & " count(if(month(a.fechamov)='02',a.numsobre,null))*b.cargos, " _
         & " count(if(month(a.fechamov)='03',a.numsobre,null))*b.cargos,count(if(month(a.fechamov)='04',a.numsobre,null))*b.cargos, " _
         & " count(if(month(a.fechamov)='05',a.numsobre,null))*b.cargos,count(if(month(a.fechamov)='06',a.numsobre,null))*b.cargos, " _
         & " count(if(month(a.fechamov)='07',a.numsobre,null))*b.cargos,count(if(month(a.fechamov)='08',a.numsobre,null))*b.cargos, " _
         & " count(if(month(a.fechamov)='09',a.numsobre,null))*b.cargos,count(if(month(a.fechamov)='10',a.numsobre,null))*b.cargos, " _
         & " count(if(month(a.fechamov)='11',a.numsobre,null))*b.cargos,count(if(month(a.fechamov)='12',a.numsobre,null))*b.cargos " _
         & " from sobres a, jsvencestic b " _
         & " where b.id_emp='" & jytsistema.WorkID & "' " _
         & " and a.corredor=b.codigo " _
         & " group by 1 ")



        ft.Ejecutar_strSQL(myconn, " create temporary table cobrado_clientes (corredor char(5),mes1 double(16,2), " _
        & " mes2 double(16,2),mes3 double(16,2),mes4 double(16,2),mes5 double(16,2),mes6 double(16,2), " _
        & " mes7 double(16,2),mes8 double(16,2),mes9 double(16,2),mes10 double(16,2),mes11 double(16,2),mes12 double(16,2)) ")

        ft.Ejecutar_strSQL(myconn, " insert into cobrado_clientes " _
        & " select a.corredor,sum(if(month(a.fechamov)='01',b.monto-b.comision,0)), " _
        & "  sum(if(month(a.fechamov)='02',b.monto-b.comision,0)),sum(if(month(a.fechamov)='03',b.monto-b.comision,0)), " _
        & "  sum(if(month(a.fechamov)='04',b.monto-b.comision,0)),sum(if(month(a.fechamov)='05',b.monto-b.comision,0)), " _
        & "  sum(if(month(a.fechamov)='06',b.monto-b.comision,0)),sum(if(month(a.fechamov)='07',b.monto-b.comision,0)), " _
         & " sum(if(month(a.fechamov)='08',b.monto-b.comision,0)),sum(if(month(a.fechamov)='09',b.monto-b.comision,0)), " _
         & " sum(if(month(a.fechamov)='10',b.monto-b.comision,0)),sum(if(month(a.fechamov)='11',b.monto-b.comision,0)), " _
         & " sum(if(month(a.fechamov)='12',b.monto-b.comision,0)) " _
         & " from sobres a, jsventabtic b " _
         & " Where " _
          & " b.id_emp='" & jytsistema.WorkID & "' " _
         & " and a.numsobre=b.numdep " _
         & " group by 1 ")

        ft.Ejecutar_strSQL(myconn, " create temporary table entradas_banco (corredor char(5),mes1 double(16,2), " _
        & " mes2 double(16,2),mes3 double(16,2),mes4 double(16,2),mes5 double(16,2),mes6 double(16,2), " _
        & " mes7 double(16,2),mes8 double(16,2),mes9 double(16,2),mes10 double(16,2),mes11 double(16,2),mes12 double(16,2)) ")

        ft.Ejecutar_strSQL(myconn, " insert into entradas_banco select b.corredor,sum(if(month(b.fechamov)='01',a.importe,0)),sum(if(month(b.fechamov)='02',a.importe,0)), " _
         & " sum(if(month(b.fechamov)='03',a.importe,0)),sum(if(month(b.fechamov)='04',a.importe,0)),  " _
         & " sum(if(month(b.fechamov)='05',a.importe,0)),sum(if(month(b.fechamov)='06',a.importe,0)), " _
         & " sum(if(month(b.fechamov)='07',a.importe,0)),sum(if(month(b.fechamov)='08',a.importe,0)), " _
         & " sum(if(month(b.fechamov)='09',a.importe,0)),sum(if(month(b.fechamov)='10',a.importe,0)), " _
         & " sum(if(month(b.fechamov)='11',a.importe,0)),sum(if(month(b.fechamov)='12',a.importe,0)) " _
         & " from jsbantraban a,sobres b where a.id_emp='" & jytsistema.WorkID & "' and " _
         & " a.tiporg='CT' and a.numdoc=b.numsobre group by 1 ")

        SeleccionSIGMEGananciasCestaTicketMesMes = " select a.corredor,c.descrip as nombre_corredor,(a.mes1-b.mes1) ganancia1, " _
        & " (a.mes2-b.mes2) ganancia2, (a.mes3-b.mes3) ganancia3, (a.mes4-b.mes4) ganancia4, " _
        & " (a.mes5-b.mes5) ganancia5, (a.mes6-b.mes6) ganancia6, (a.mes7-b.mes7) ganancia7, " _
        & " (a.mes8-b.mes8) ganancia8, (a.mes9-b.mes9) ganancia9, (a.mes10-b.mes10) ganancia10, " _
        & " (a.mes11-b.mes11) ganancia11, (a.mes12-b.mes12) ganancia12, " _
        & " a.mes1 banco1, a.mes2 banco2, a.mes3 banco3, a.mes4 banco4, a.mes5 banco5, a.mes6 banco6, " _
        & " a.mes7 banco7, a.mes8 banco8, a.mes9 banco9, a.mes10 banco10, a.mes11 banco11, a.mes12 banco12, " _
        & " b.mes1 cliente1, b.mes2 cliente2, b.mes3 cliente3, b.mes4 cliente4, b.mes5 cliente5, b.mes6 cliente6, " _
        & " b.mes7 cliente7, b.mes8 cliente8, b.mes9 cliente9, b.mes10 cliente10, b.mes11 cliente11, b.mes12 cliente12, " _
        & " d.mes1 iva1, d.mes2 iva2, d.mes3 iva3, d.mes4 iva4, d.mes5 iva5, d.mes6 iva6, " _
        & " d.mes7 iva7, d.mes8 iva8, d.mes9 iva9, d.mes10 iva10, d.mes11 iva11, d.mes12 iva12, " _
        & " e.mes1 com1, e.mes2 com2, e.mes3 com3, e.mes4 com4, e.mes5 com5, e.mes6 com6, " _
        & " e.mes7 com7, e.mes8 com8, e.mes9 com9, e.mes10 com10, e.mes11 com11, e.mes12 com12 " _
        & " from entradas_banco a,cobrado_clientes b, jsvencestic c,iva d,comisiones e " _
        & " Where a.corredor = b.corredor And a.corredor = c.codigo And a.corredor = d.corredor " _
        & " and a.corredor=e.corredor " _
        & " and c.id_emp='" & jytsistema.WorkID & "' "

    End Function

    Function SeleccionSIGMEGananciaCestaTicketEnPeriodo(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                                        ByVal CorredorDesde As String, ByVal CorredorHasta As String, _
                                                        ByVal FechaDesde As Date, ByVal FechaHasta As Date)

        Dim strCorredor As String = ""
        If CorredorDesde <> "" Then strCorredor += " and a.corredor >= '" & CorredorDesde & "' "
        If CorredorHasta <> "" Then strCorredor += " and a.corredor <= '" & CorredorHasta & "' "

        Dim tblsobres As String, tblIVA As String
        Dim tblComisiones As String, tblentradas_banco As String

        tblsobres = "tbl" & ft.NumeroAleatorio(10000)
        tblIVA = "tbl" & ft.NumeroAleatorio(10000)
        tblComisiones = "tbl" & ft.NumeroAleatorio(10000)
        tblentradas_banco = "tbl" & ft.NumeroAleatorio(10000)

        ft.Ejecutar_strSQL(myconn, " create temporary table " & tblsobres & " (corredor char(5), fechamov date, numsobre varchar(15), montoideal double(19,2) default '0.00' not null, comisionideal double(19,2) default '0.00' not null) ")
        ft.Ejecutar_strSQL(myconn, " insert into " & tblsobres & " select b.corredor, a.fechamov, a.numdoc , sum(b.monto), sum(b.comision) " _
            & " from jsbantraban a, jsventabtic b " _
            & " Where " _
            & " a.fechamov >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and " _
            & " a.fechamov <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
            & " a.tiporg = 'CT' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' and " _
            & " a.id_emp = b.id_emp and " _
            & " a.NumDoc = b.numdep " _
            & " group by 3 ")

        ft.Ejecutar_strSQL(myconn, " create temporary table " & tblIVA & " (corredor char(5), iva double(19,2)) ")
        ft.Ejecutar_strSQL(myconn, " insert into " & tblIVA _
            & " select a.corredor, sum(b.imp_iva) " _
            & " from " & tblsobres & " a, jsproencgas b " _
            & " Where " _
            & " b.id_emp = '" & jytsistema.WorkID & "' and " _
            & " a.numsobre = b.numpag and " _
            & " a.fechamov = b.emision group by 1 ")

        ft.Ejecutar_strSQL(myconn, " create temporary table " & tblComisiones & " (corredor char(5), comisiones integer) ")
        ft.Ejecutar_strSQL(myconn, " insert into " & tblComisiones _
            & " select a.corredor, count(a.numsobre)*b.cargos " _
            & " from " & tblsobres & " a, jsvencestic b " _
            & " Where " _
            & " b.id_emp = '" & jytsistema.WorkID & "' " _
            & " and a.corredor = b.codigo " _
            & " group by 1 ")

        ft.Ejecutar_strSQL(myconn, " create temporary table " & tblentradas_banco & "(corredor char(5), entrada double(19,2) ) ")
        ft.Ejecutar_strSQL(myconn, " insert into " & tblentradas_banco _
            & " select b.corredor, sum( a.importe) " _
            & " from jsbantraban a, " & tblsobres & " b " _
            & " where a.id_emp = '" & jytsistema.WorkID & "' and " _
            & " a.tiporg = 'CT' and " _
            & " a.numdoc = b.numsobre " _
            & " group by 1 ")

        SeleccionSIGMEGananciaCestaTicketEnPeriodo = " select a.corredor, e.descrip, sum(a.montoideal) montoideal, sum(a.comisionideal) comisionideal, " _
             & " b.iva, c.comisiones gastos, (sum(montoideal) - sum(a.comisionideal)) cobrado, d.entrada, " _
             & " sum(a.comisionideal) - b.iva - c.comisiones + (d.entrada - (sum(a.montoideal) - sum(a.comisionideal))) ganancia, " _
             & " (sum(a.comisionideal) - b.iva - c.comisiones + (d.entrada - (sum(a.montoideal) - sum(a.comisionideal))))/sum(a.montoideal)*100 por_ganancia " _
             & " from " & tblsobres & " a, " & tblIVA & " b, " & tblComisiones & " c, " & tblentradas_banco & " d, jsvencestic e " _
             & " where " _
             & " e.id_emp = '" & jytsistema.WorkID & "' and " _
             & " a.corredor = b.corredor and " _
             & " a.corredor = c.corredor and " _
             & " a.corredor = d.corredor and " _
             & " a.corredor = e.codigo " & strCorredor _
             & " group by a.corredor "

    End Function
    Function SeleccionSIGMEActivacion(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, ByVal OrdenadoPor As String, _
                                Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", _
                                Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", _
                                Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                Optional ByVal TiponegocioDesde As String = "", Optional ByVal TipoNegocioHasta As String = "", _
                                Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                Optional ByVal EmisionDesde As Date = MyDate, Optional ByVal EmisionHasta As Date = MyDate, _
                                Optional ByVal AlmacenDesde As String = "", Optional ByVal AlmacenHasta As String = "", _
                                Optional ByVal AgrupadoPor As String() = Nothing, Optional ByVal ClienteDesde As String = "", Optional ByVal ClienteHasta As String = "", _
                                Optional ByVal Resumido As Boolean = False, Optional ByVal siMeses As Boolean = False) As String

        Dim strSQL As String = ""
        Dim strsqlEmision As String = ""
        Dim strsqlAlmacen As String = ""
        Dim strSQLREsumen As String = ""
        Dim StrMeses As String
        Dim AñoActual As Integer, AñoAnterior As Integer

        Dim strSQLVendedor As String = ""
        Dim strCuenta As String
        Dim i As Integer, strGrupo As String = ""

        AñoActual = Year(jytsistema.sFechadeTrabajo)
        AñoAnterior = AñoActual - 1

        If AgrupadoPor IsNot Nothing Then
            If UBound(AgrupadoPor) <= 0 Then
            Else
                For i = 0 To UBound(AgrupadoPor) - 1
                    Select Case AgrupadoPor(i)
                        Case "categoria"
                            strGrupo = strGrupo & ", " & " a.grupo "
                        Case "marca"
                            strGrupo = strGrupo & ", " & " a.marca "
                        Case "tipjer"
                            strGrupo = strGrupo & ", " & " a.tipjer "
                        Case "codjer"

                        Case "division"
                            strGrupo = strGrupo & ", " & " a.division "
                        Case "mix"
                            strGrupo = strGrupo & ", " & " a.mix "
                        Case "canal"
                            strGrupo = strGrupo & ", " & " f.categoria "
                        Case "tiponegocio"
                            strGrupo = strGrupo & ", " & " f.unidad "
                        Case "zona"
                            strGrupo = strGrupo & ", " & " f.codzon "
                        Case "ruta"
                            strGrupo = strGrupo & ", " & " f.codrut "
                        Case "asesor"
                            strGrupo = strGrupo & ", " & " b.vendedor "
                        Case "territorio"
                            strGrupo = strGrupo & ", " & " f.fpais, f.festado, f.fmunicipio, f.fparroquia, f.fciudad, f.fbarrio "
                    End Select
                Next
            End If
        End If

        If Resumido Then
            strCuenta = "b.codart"
        Else
            strCuenta = "b.prov_cli"
        End If


        If MercanciaDesde <> "" Then strSQL += " a." & OrdenadoPor & " >= '" & MercanciaDesde & "' and "
        If MercanciaHasta <> "" Then strSQL += " a." & OrdenadoPor & " <= '" & MercanciaHasta & "' and "
        If CategoriaDesde <> "" Then strSQL += " a.grupo >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then strSQL += " a.grupo <= '" & CategoriaHasta & "' and "
        If MarcaDesde <> "" Then strSQL += " a.marca >= '" & MarcaDesde & "' and "
        If MarcaHasta <> "" Then strSQL += " a.marca <= '" & MarcaHasta & "' and "
        If DivisionDesde <> "" Then strSQL += " a.division >= '" & DivisionDesde & "' and "
        If DivisionHasta <> "" Then strSQL += " a.division <= '" & DivisionHasta & "' and "
        If TipoJerarquia <> "" Then strSQL += " a.tipjer = '" & TipoJerarquia & "' and "

        If siMeses Then
            strsqlEmision = " and year(b.fechamov) = " & Year(EmisionDesde) & " "
        Else
            strsqlEmision = " and b.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(EmisionDesde) & "' "
            strsqlEmision += " and b.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(EmisionHasta) & "' "
        End If
        If AlmacenDesde <> "" Then strsqlAlmacen += " and b.almacen >= '" & AlmacenDesde & "'  "
        If AlmacenHasta <> "" Then strsqlAlmacen += " and b.almacen <= '" & AlmacenHasta & "' "

        If CanalDesde <> "" Then strSQL += " f.categoria >= '" & CanalDesde & "' and "
        If CanalHasta <> "" Then strSQL += " f.categoria <= '" & CanalHasta & "' and "
        If TiponegocioDesde <> "" Then strSQL += " f.unidad >= '" & TiponegocioDesde & "' and "
        If TipoNegocioHasta <> "" Then strSQL += " f.unidad <= '" & TipoNegocioHasta & "' and "
        If ZonaDesde <> "" Then strSQL += " f.zona >= '" & ZonaDesde & "' and "
        If ZonaHasta <> "" Then strSQL += " f.zona <= '" & ZonaHasta & "' and "
        If RutaDesde <> "" Then strSQL += " f.Ruta_visita >= '" & RutaDesde & "' and "
        If RutaHasta <> "" Then strSQL += " f.Ruta_visita <= '" & RutaHasta & "' and "
        If Pais <> "" Then strSQL += " f.fpais = '" & Pais & "' and "
        If Estado <> "" Then strSQL += " f.festado = '" & Estado & "' and "
        If Municipio <> "" Then strSQL += " f.fmunicipio = '" & Municipio & "' and "
        If Parroquia <> "" Then strSQL += " f.fparroquia = '" & Parroquia & "' and "
        If Ciudad <> "" Then strSQL += " f.fciudad = '" & Ciudad & "' and "
        If Barrio <> "" Then strSQL += " f.fbarrio = '" & Barrio & "' and "

        If AsesorDesde <> "" Then strSQLVendedor += " and b.vendedor >= '" & AsesorDesde & "' "
        If AsesorHasta <> "" Then strSQLVendedor += " and b.vendedor <= '" & AsesorHasta & "' "


        StrMeses = ", count(distinct( if ( month(b.fechamov) = 1 and year(b.fechamov) = " & AñoActual & " , " & strCuenta & ", null))) as act01,  " _
                      & " count(distinct( if ( month(b.fechamov) = 2 and year(b.fechamov) = " & AñoActual & " , " & strCuenta & ", null))) as act02,  " _
                      & " count(distinct( if ( month(b.fechamov) = 3 and year(b.fechamov) = " & AñoActual & " , " & strCuenta & ", null))) as act03,  " _
                      & " count(distinct( if ( month(b.fechamov) = 4 and year(b.fechamov) = " & AñoActual & " , " & strCuenta & ", null))) as act04,  " _
                      & " count(distinct( if ( month(b.fechamov) = 5 and year(b.fechamov) = " & AñoActual & " , " & strCuenta & ", null))) as act05,  " _
                      & " count(distinct( if ( month(b.fechamov) = 6 and year(b.fechamov) = " & AñoActual & " , " & strCuenta & ", null))) as act06,  " _
                      & " count(distinct( if ( month(b.fechamov) = 7 and year(b.fechamov) = " & AñoActual & " , " & strCuenta & ", null))) as act07,  " _
                      & " count(distinct( if ( month(b.fechamov) = 8 and year(b.fechamov) = " & AñoActual & " , " & strCuenta & ", null))) as act08,  " _
                      & " count(distinct( if ( month(b.fechamov) = 9 and year(b.fechamov) = " & AñoActual & " , " & strCuenta & ", null))) as act09,  " _
                      & " count(distinct( if ( month(b.fechamov) = 10 and year(b.fechamov) = " & AñoActual & " , " & strCuenta & ", null))) as act10,  " _
                      & " count(distinct( if ( month(b.fechamov) = 11 and year(b.fechamov) = " & AñoActual & " , " & strCuenta & ", null))) as act11,  " _
                      & " count(distinct( if ( month(b.fechamov) = 12 and year(b.fechamov) = " & AñoActual & " , " & strCuenta & ", null))) as act12, " _
                      & " count(distinct( if ( month(b.fechamov) = 1 and year(b.fechamov) = " & AñoAnterior & " , " & strCuenta & ", null))) as ant01,  " _
                      & " count(distinct( if ( month(b.fechamov) = 2 and year(b.fechamov) = " & AñoAnterior & " , " & strCuenta & ", null))) as ant02,  " _
                      & " count(distinct( if ( month(b.fechamov) = 3 and year(b.fechamov) = " & AñoAnterior & " , " & strCuenta & ", null))) as ant03,  " _
                      & " count(distinct( if ( month(b.fechamov) = 4 and year(b.fechamov) = " & AñoAnterior & " , " & strCuenta & ", null))) as ant04,  " _
                      & " count(distinct( if ( month(b.fechamov) = 5 and year(b.fechamov) = " & AñoAnterior & " , " & strCuenta & ", null))) as ant05,  " _
                      & " count(distinct( if ( month(b.fechamov) = 6 and year(b.fechamov) = " & AñoAnterior & " , " & strCuenta & ", null))) as ant06,  " _
                      & " count(distinct( if ( month(b.fechamov) = 7 and year(b.fechamov) = " & AñoAnterior & " , " & strCuenta & ", null))) as ant07,  " _
                      & " count(distinct( if ( month(b.fechamov) = 8 and year(b.fechamov) = " & AñoAnterior & " , " & strCuenta & ", null))) as ant08,  " _
                      & " count(distinct( if ( month(b.fechamov) = 9 and year(b.fechamov) = " & AñoAnterior & " , " & strCuenta & ", null))) as ant09,  " _
                      & " count(distinct( if ( month(b.fechamov) = 10 and year(b.fechamov) = " & AñoAnterior & " , " & strCuenta & ", null))) as ant10,  " _
                      & " count(distinct( if ( month(b.fechamov) = 11 and year(b.fechamov) = " & AñoAnterior & " , " & strCuenta & ", null))) as ant11,  " _
                      & " count(distinct( if ( month(b.fechamov) = 12 and year(b.fechamov) = " & AñoAnterior & " , " & strCuenta & ", null))) as ant12  "

        SeleccionSIGMEActivacion = " select elt(a.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') AS mix, a.codart, a.nomart, a.unidad, c.descrip categoria, d.descrip as marca, p.descrip division, e.descrip as tipjer, f.nombre , " _
             & " b.prov_cli, f.vendedor, g.descrip as canal, h.descrip As tiponegocio, i.descrip as zona, j.nomrut As ruta, concat(f.vendedor,' ', k.apellidos, ', ', k.nombres) as asesor,  " _
             & " if( q.nombre is null, '', q.nombre) FBARRIO, if( r.nombre is null, '', r.nombre) FCIUDAD, if( s.nombre is null, '', s.nombre) FPARROQUIA, " _
             & " if( t.nombre is null, '', t.nombre) FMUNICIPIO, if(u.nombre is null, '', u.nombre) FESTADO, if(v.nombre is null, '', v.nombre) FPAIS " _
             & StrMeses _
             & " from jsmerctainv a " _
             & " left join jsmertramer b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
             & " left join jsconctatab c on (a.grupo = c.codigo and a.id_emp = c.id_emp and c.modulo = '00002') " _
             & " left join jsconctatab d on (a.marca = d.codigo and a.id_emp = d.id_emp and d.modulo = '00003') " _
             & " left join jsmercatdiv p on (a.division = p.division and a.id_emp = p.id_emp) " _
             & " left join jsmerencjer e on (a.tipjer = e.tipjer and a.id_emp = e.id_emp) " _
             & " left join jsvencatcli f on (b.prov_cli = f.codcli and a.id_emp = f.id_emp) " _
             & " left join jsvenliscan g on (f.categoria = g.codigo and f.id_emp = g.id_emp) " _
             & " left join jsvenlistip h on (f.unidad = h.codigo and f.id_emp = h.id_emp) " _
             & " left join jsconctatab i on (f.zona = i.codigo and f.id_emp = i.id_emp and i.modulo = '00005') " _
             & " left join jsvenencrut j on (f.ruta_visita = j.codrut and f.id_emp = j.id_emp and j.tipo = '0')" _
             & " left join jsvencatven k on (b.vendedor = k.codven and b.id_emp = k.id_emp and k.tipo = '0') " _
             & " left join jsconcatter q on (f.fbarrio = q.codigo and f.id_emp = q.id_emp ) " _
             & " left join jsconcatter r on (f.fciudad = r.codigo and f.id_emp = r.id_emp ) " _
             & " left join jsconcatter s on (f.fparroquia = s.codigo and f.id_emp = s.id_emp ) " _
             & " left join jsconcatter t on (f.fmunicipio = t.codigo and f.id_emp = t.id_emp ) " _
             & " left join jsconcatter u on (f.festado = u.codigo and f.id_emp = u.id_emp ) " _
             & " left join jsconcatter v on (f.fpais = v.codigo and f.id_emp = v.id_emp ) " _
             & " Where " & strSQL _
             & " a.id_emp = '" & jytsistema.WorkID & "' and " _
             & " (year(b.fechamov) = " & AñoAnterior & " or year(b.fechamov) = " & AñoActual & " ) and " _
             & " b.origen in('FAC','PFC','PVE','NCV','NDV') " _
             & strsqlAlmacen _
             & strSQLVendedor _
             & " group by a.id_emp " & strGrupo _
             & " "



    End Function


    Function SeleccionSIGMEVentasComparativasMesMes(ByVal MyConn As MySqlConnection, ByVal lbInfo As Label, _
                                             ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, _
                                            Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", _
                                            Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                            Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                            Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                            Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                            Optional ByVal ClienteDesde As String = "", Optional ByVal ClienteHasta As String = "", _
                                            Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                            Optional ByVal TiponegocioDesde As String = "", Optional ByVal TipoNegocioHasta As String = "", _
                                            Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                            Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                            Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                            Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                            Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                            Optional ByVal CantidadPesoMoneda As Integer = 0) As String

        Dim strMercas As String, strClientes As String
        Dim strCan As String = ""
        Dim strUnidad As String = ""
        Dim strUND As String = ""
        Dim tblComparativa As String
        Dim AñoActual As Integer, AñoAnterior As Integer

        AñoActual = Year(jytsistema.sFechadeTrabajo)
        AñoAnterior = Year(jytsistema.sFechadeTrabajo) - 1

        tblComparativa = "tbl" & ft.NumeroAleatorio(10000)

        Select Case CantidadPesoMoneda
            Case 0 'UNIDAD DE VENTA
                strCan = " a.cantidad/n.equivale "
                strUnidad = " a.unidad "
                strUND = " c.unidad "
            Case 1 'UNIDAD DE MEDIDA PRINCIPAL UMP (KGR)
                strCan = " a.peso "
                strUnidad = " a.unidad "
                strUND = " 'KGR' "
            Case 2 'MONETARIOS VENTAS
                strCan = " a.ventotaldes "
                strUnidad = " a.unidad "
                strUND = " 'BsF' "
            Case 3 'UNIDAD DE MEDIDA SECUNDARIA UMS (CAJ)
                strCan = " a.peso/c.pesounidad*n.equivale "
                strUnidad = " 'CAJ' "
                strUND = " 'CAJ' "
            Case 4 'MONETARIOS COSTOS
                strCan = " a.COSTOTALDES "
                strUnidad = " a.unidad "
                strUND = " 'BsF' "
        End Select


        strMercas = CadenaComplementoMercancias("c", MercanciaDesde, MercanciaHasta, 0, "codart", TipoJerarquia, Nivel1, _
                                                Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, CategoriaDesde, CategoriaHasta, _
                                                MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta)

        strClientes = CadenaComplementoClientes("d", ClienteDesde, ClienteHasta, 0, "codcli", CanalDesde, CanalHasta, TiponegocioDesde, TipoNegocioHasta, _
                        ZonaDesde, ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, Parroquia, Ciudad, Barrio)

        If AsesorDesde <> "" Then strClientes += " a.vendedor >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then strClientes += " a.vendedor <= '" & AsesorHasta & "' and "

        SeleccionSIGMEVentasComparativasMesMes = " select a.codart, c.nomart, " & strUND & " unidad, " _
            & " c.grupo categoria, c.marca, elt(mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') mix, c.division, c.tipjer, c.codjer, " _
            & " sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 1, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) eneAnterior, sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 1, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) eneActual, " _
            & " sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 2, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) febAnterior, sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 2, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) febActual, " _
            & " sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 3, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) marAnterior, sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 3, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) marActual, " _
            & " sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 4, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) abrAnterior, sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 4, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) abrActual, " _
            & " sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 5, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) mayAnterior, sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 5, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) mayActual, " _
            & " sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 6, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) junAnterior, sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 6, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) junActual, " _
            & " sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 7, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) julAnterior, sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 7, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) julActual, " _
            & " sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 8, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) agoAnterior, sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 8, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) agoActual, " _
            & " sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 9, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) sepAnterior, sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 9, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) sepActual, " _
            & " sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 10, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) octAnterior, sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 10, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) octActual, " _
            & " sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 11, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) novAnterior, sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 11, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) novActual, " _
            & " sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 12, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) dicAnterior, sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 12, IF(a.origen = 'NCV', -1,1)*" & strCan & ", 0.00 )) dicActual " _
            & " from jsmertramer a " _
            & " left join (SELECT a.codart, a.unidad, a.equivale, a.uvalencia, a.id_emp  " _
            & " 			FROM jsmerequmer a  " _
            & "             WHERE " _
            & " 			a.id_emp = '" & jytsistema.WorkID & "' " _
            & "             UNION " _
            & " 			SELECT a.codart, a.unidad, 1 equivale, a.unidad uvalencia, a.id_emp " _
            & " 			FROM jsmerctainv a " _
            & " 			WHERE  " _
            & " 			a.id_emp = '" & jytsistema.WorkID & "') n on (a.codart = n.codart and " & strUnidad & " = n.uvalencia and a.id_emp = n.id_emp) " _
            & " left join jsmerctainv c on (a.codart = c.codart and a.id_emp = c.id_emp) " _
            & " left join jsvencatcli d on (a.prov_cli = d.codcli and a.id_emp = d.id_emp )" _
            & " Where " _
            & strMercas & strClientes _
            & " year(a.fechamov) >= " & AñoAnterior & " and year(a.fechamov) <= " & AñoActual & " and " _
            & " a.origen in ('FAC','NDV','PFC','PVE', 'NCV') and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by a.codart "

    End Function
    Function SeleccionSIGMEComprasComparativasMesMes(ByVal NyConn As MySqlConnection, ByVal lblInfo As Label, ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, _
                               Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", _
                               Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                               Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                               Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                               Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                               Optional ByVal ProveedorDesde As String = "", Optional ByVal ProveedorHasta As String = "", _
                               Optional ByVal CatProDesde As String = "", Optional ByVal CatProHasta As String = "", _
                               Optional ByVal UnidadNegocioDesde As String = "", Optional ByVal UnidadNegocioHasta As String = "", _
                               Optional ByVal CantidadPesoMoneda As Integer = 0) As String

        Dim strMercas As String = ""
        Dim strCan As String = ""
        Dim AñoActual As Integer, AñoAnterior As Integer

        AñoActual = Year(jytsistema.sFechadeTrabajo)
        AñoAnterior = Year(jytsistema.sFechadeTrabajo) - 1

        Select Case CantidadPesoMoneda
            Case 0
                strCan = " if( isnull(n.uvalencia), a.cantidad, a.cantidad / n.equivale) "
            Case 1
                strCan = " a.peso "
            Case 2
                strCan = " a.costotaldes "
        End Select


        strMercas = CadenaComplementoMercancias("c", MercanciaDesde, MercanciaHasta, 0, "codart", TipoJerarquia, Nivel1, Nivel2, Nivel3, Nivel4, _
                                                Nivel5, Nivel6, CategoriaDesde, CategoriaHasta, MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta)

        If ProveedorDesde <> "" Then strMercas += " d.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then strMercas += " d.codpro <= '" & ProveedorHasta & "' and "
        If CatProDesde <> "" Then strMercas += " d.categoria >= '" & CatProDesde & "' and "
        If CatProHasta <> "" Then strMercas += " d.categoria <= '" & CatProHasta & "' and "
        If UnidadNegocioDesde <> "" Then strMercas += " d.unidad >= '" & UnidadNegocioDesde & "' and "
        If UnidadNegocioHasta <> "" Then strMercas += " d.unidad <= '" & UnidadNegocioHasta & "' and "

        SeleccionSIGMEComprasComparativasMesMes = " select a.codart, c.nomart, c.unidad, " _
            & " c.grupo categoria, c.marca, elt(mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') mix, c.division, c.tipjer, c.codjer, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 1, " & strCan & ", 0.00 ))) eneAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 1, " & strCan & ", 0.00 ))) eneActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 2, " & strCan & ", 0.00 ))) febAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 2, " & strCan & ", 0.00 ))) febActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 3, " & strCan & ", 0.00 ))) marAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 3, " & strCan & ", 0.00 ))) marActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 4, " & strCan & ", 0.00 ))) abrAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 4, " & strCan & ", 0.00 ))) abrActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 5, " & strCan & ", 0.00 ))) mayAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 5, " & strCan & ", 0.00 ))) mayActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 6, " & strCan & ", 0.00 ))) junAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 6, " & strCan & ", 0.00 ))) junActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 7, " & strCan & ", 0.00 ))) julAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 7, " & strCan & ", 0.00 ))) julActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 8, " & strCan & ", 0.00 ))) agoAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 8, " & strCan & ", 0.00 ))) agoActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 9, " & strCan & ", 0.00 ))) sepAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 9, " & strCan & ", 0.00 ))) sepActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 10, " & strCan & ", 0.00 ))) octAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 10, " & strCan & ", 0.00 ))) octActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 11, " & strCan & ", 0.00 ))) novAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 11, " & strCan & ", 0.00 ))) novActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 12, " & strCan & ", 0.00 ))) dicAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 12, " & strCan & ", 0.00 ))) dicActual " _
            & " from jsmertramer a " _
            & " left join jsmerequmer n on (a.codart = n.codart and a.unidad = n.uvalencia and a.id_emp = n.id_emp) " _
            & " left join jsmerctainv c on (a.codart = c.codart and a.id_emp = c.id_emp) " _
            & " left join jsprocatpro d on (a.prov_cli = d.codpro and a.id_emp = d.id_emp )" _
            & " Where " _
            & strMercas _
            & " year(a.fechamov) >= " & AñoAnterior & " and year(a.fechamov) <= " & AñoActual & " and " _
            & " a.origen in ('COM','NDC','REC') and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by a.codart "

    End Function

    Function SeleccionSIGMEDropSize(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                               ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, _
                               Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", _
                               Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                               Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                               Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                               Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                               Optional ByVal ClienteDesde As String = "", Optional ByVal ClienteHasta As String = "", _
                               Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                               Optional ByVal TiponegocioDesde As String = "", Optional ByVal TipoNegocioHasta As String = "", _
                               Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                               Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                               Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                               Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                               Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                               Optional ByVal Año As Integer = 2014) As String

        Dim strMercas As String = ""
        Dim strClientes As String = ""
        Dim tbldropsize As String, tbldropsize_b As String, tbldropsize_c As String
        Dim tblcantidad As String, tblkilos As String, tblMoney As String

        tbldropsize = "tbl" & ft.NumeroAleatorio(10000)
        tbldropsize_b = "tbl" & ft.NumeroAleatorio(10000)
        tbldropsize_c = "tbl" & ft.NumeroAleatorio(10000)
        tblcantidad = "tbl" & ft.NumeroAleatorio(10000)
        tblkilos = "tbl" & ft.NumeroAleatorio(10000)
        tblMoney = "tbl" & ft.NumeroAleatorio(10000)

        strMercas = CadenaComplementoMercancias("c", MercanciaDesde, MercanciaHasta, 0, "codart",
                                                  TipoJerarquia, Nivel1, Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, CategoriaDesde, CategoriaHasta, _
                                                  MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta)

        strClientes = CadenaComplementoClientes("d", ClienteDesde, ClienteHasta, 0, "codcli", CanalDesde, CanalHasta, TiponegocioDesde, TipoNegocioHasta, _
               ZonaDesde, ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, Parroquia, Ciudad, Barrio)

        ft.Ejecutar_strSQL(myconn, " create temporary TABLE " & tbldropsize & " (tipo int(1) default '0' not null,  codigo varchar(55) not null, mEne Double(19,2) default '0.00' not null, mFeb Double(19,2) default '0.00' not null, mMar Double(19,2) default '0.00' not null, mAbr Double(19,2) default '0.00' not null, mMay Double(19,2) default '0.00' not null, mJun Double(19,2) default '0.00' not null, mJul Double(19,2) default '0.00' not null, mAgo Double(19,2) default '0.00' not null, mSep Double(19,2) default '0.00' not null, mOct Double(19,2) default '0.00' not null, mNov Double(19,2) default '0.00' not null, mDic Double(19,2) default '0.00' not null)")
        ft.Ejecutar_strSQL(myconn, " create temporary TABLE " & tbldropsize_b & " (tipo int(1) default '0' not null, codigo varchar(55) not null, mEne Double(19,2) default '0.00' not null, mFeb Double(19,2) default '0.00' not null, mMar Double(19,2) default '0.00' not null, mAbr Double(19,2) default '0.00' not null, mMay Double(19,2) default '0.00' not null, mJun Double(19,2) default '0.00' not null, mJul Double(19,2) default '0.00' not null, mAgo Double(19,2) default '0.00' not null, mSep Double(19,2) default '0.00' not null, mOct Double(19,2) default '0.00' not null, mNov Double(19,2) default '0.00' not null, mDic Double(19,2) default '0.00' not null)")
        ft.Ejecutar_strSQL(myconn, " create temporary TABLE " & tbldropsize_c & " (tipo int(1) default '0' not null, codigo varchar(55) not null, mEne Double(19,2) default '0.00' not null, mFeb Double(19,2) default '0.00' not null, mMar Double(19,2) default '0.00' not null, mAbr Double(19,2) default '0.00' not null, mMay Double(19,2) default '0.00' not null, mJun Double(19,2) default '0.00' not null, mJul Double(19,2) default '0.00' not null, mAgo Double(19,2) default '0.00' not null, mSep Double(19,2) default '0.00' not null, mOct Double(19,2) default '0.00' not null, mNov Double(19,2) default '0.00' not null, mDic Double(19,2) default '0.00' not null)")
        ft.Ejecutar_strSQL(myconn, " create temporary TABLE " & tblcantidad & " (codart varchar(15) not null, mEne Double(19,2) default '0.00' not null, mFeb Double(19,2) default '0.00' not null, mMar Double(19,2) default '0.00' not null, mAbr Double(19,2) default '0.00' not null, mMay Double(19,2) default '0.00' not null, mJun Double(19,2) default '0.00' not null, mJul Double(19,2) default '0.00' not null, mAgo Double(19,2) default '0.00' not null, mSep Double(19,2) default '0.00' not null, mOct Double(19,2) default '0.00' not null, mNov Double(19,2) default '0.00' not null, mDic Double(19,2) default '0.00' not null) ")
        ft.Ejecutar_strSQL(myconn, " create temporary TABLE " & tblkilos & " (codart varchar(15) not null, mEne Double(19,2) default '0.00' not null, mFeb Double(19,2) default '0.00' not null, mMar Double(19,2) default '0.00' not null, mAbr Double(19,2) default '0.00' not null, mMay Double(19,2) default '0.00' not null, mJun Double(19,2) default '0.00' not null, mJul Double(19,2) default '0.00' not null, mAgo Double(19,2) default '0.00' not null, mSep Double(19,2) default '0.00' not null, mOct Double(19,2) default '0.00' not null, mNov Double(19,2) default '0.00' not null, mDic Double(19,2) default '0.00' not null) ")
        ft.Ejecutar_strSQL(myconn, " create temporary TABLE " & tblMoney & " (codart varchar(15) not null, mEne Double(19,2) default '0.00' not null, mFeb Double(19,2) default '0.00' not null, mMar Double(19,2) default '0.00' not null, mAbr Double(19,2) default '0.00' not null, mMay Double(19,2) default '0.00' not null, mJun Double(19,2) default '0.00' not null, mJul Double(19,2) default '0.00' not null, mAgo Double(19,2) default '0.00' not null, mSep Double(19,2) default '0.00' not null, mOct Double(19,2) default '0.00' not null, mNov Double(19,2) default '0.00' not null, mDic Double(19,2) default '0.00' not null) ")

        ft.Ejecutar_strSQL(myconn, " insert into " & tblcantidad _
            & " select a.item, " _
            & " sum(if( month(b.emision) = 1,  if ( isnull(n.uvalencia), a.cantidad, a.cantidad/n.equivale), 0.00)) mEne, " _
            & " sum(if( month(b.emision) = 2,  if ( isnull(n.uvalencia), a.cantidad, a.cantidad/n.equivale), 0.00)) mFeb, " _
            & " sum(if( month(b.emision) = 3,  if ( isnull(n.uvalencia), a.cantidad, a.cantidad/n.equivale), 0.00)) mMar, " _
            & " sum(if( month(b.emision) = 4,  if ( isnull(n.uvalencia), a.cantidad, a.cantidad/n.equivale), 0.00)) mAbr, " _
            & " sum(if( month(b.emision) = 5,  if ( isnull(n.uvalencia), a.cantidad, a.cantidad/n.equivale), 0.00)) mMay, " _
            & " sum(if( month(b.emision) = 6,  if ( isnull(n.uvalencia), a.cantidad, a.cantidad/n.equivale), 0.00)) mJun, " _
            & " sum(if( month(b.emision) = 7,  if ( isnull(n.uvalencia), a.cantidad, a.cantidad/n.equivale), 0.00)) mJul, " _
            & " sum(if( month(b.emision) = 8,  if ( isnull(n.uvalencia), a.cantidad, a.cantidad/n.equivale), 0.00)) mAgo, " _
            & " sum(if( month(b.emision) = 9,  if ( isnull(n.uvalencia), a.cantidad, a.cantidad/n.equivale), 0.00)) mSep, " _
            & " sum(if( month(b.emision) = 10,  if ( isnull(n.uvalencia), a.cantidad, a.cantidad/n.equivale), 0.00)) mOct, " _
            & " sum(if( month(b.emision) = 11,  if ( isnull(n.uvalencia), a.cantidad, a.cantidad/n.equivale), 0.00)) mNov, " _
            & " sum(if( month(b.emision) = 12,  if ( isnull(n.uvalencia), a.cantidad, a.cantidad/n.equivale), 0.00)) mDic " _
            & " from jsvenrenfac a " _
            & " left join jsvenencfac b on (a.numfac = b.numfac and a.id_emp = b.id_emp) " _
            & " left join jsmerequmer n on (a.item = n.codart and a.unidad = n.uvalencia and a.id_emp = n.id_emp) " _
            & " inner join jsmerctainv c on (a.item = c.codart and a.id_emp = c.id_emp) " _
            & " inner join jsvencatcli d on (b.codcli = d.codcli and b.id_emp = d.id_emp )" _
            & " Where " _
            & strMercas & strClientes _
            & " year(b.emision) = " & Año & " and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " GROUP by a.item")

        ft.Ejecutar_strSQL(myconn, " insert into " & tblkilos _
            & " Select a.item, " _
            & " sum(if( month(b.emision) = 1,  a.peso, 0.00)) mEne, " _
            & " sum(if( month(b.emision) = 2,  a.peso, 0.00)) mFeb, " _
            & " sum(if( month(b.emision) = 3,  a.peso, 0.00)) mMar, " _
            & " sum(if( month(b.emision) = 4,  a.peso, 0.00)) mAbr, " _
            & " sum(if( month(b.emision) = 5,  a.peso, 0.00)) mMay, " _
            & " sum(if( month(b.emision) = 6,  a.peso, 0.00)) mJun, " _
            & " sum(if( month(b.emision) = 7,  a.peso, 0.00)) mJul, " _
            & " sum(if( month(b.emision) = 8,  a.peso, 0.00)) mAgo, " _
            & " sum(if( month(b.emision) = 9,  a.peso, 0.00)) mSep, " _
            & " sum(if( month(b.emision) = 10,  a.peso, 0.00)) mOct, " _
            & " sum(if( month(b.emision) = 11,  a.peso, 0.00)) mNov, " _
            & " sum(if( month(b.emision) = 12,  a.peso, 0.00)) mDic " _
            & " from jsvenrenfac a " _
            & " left join jsvenencfac b on (a.numfac = b.numfac and a.id_emp = b.id_emp) " _
            & " left join jsmerequmer n on (a.item = n.codart and a.unidad = n.uvalencia and a.id_emp = n.id_emp) " _
            & " inner join jsmerctainv c on (a.item = c.codart and a.id_emp = c.id_emp) " _
            & " inner join jsvencatcli d on (b.codcli = d.codcli and b.id_emp = d.id_emp )" _
            & " Where " _
            & strMercas & strClientes _
            & " year(b.emision) = " & Año & " and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " GROUP by a.item ")

        ft.Ejecutar_strSQL(myconn, " insert into " & tblMoney _
        & " Select a.item, " _
        & " sum(if( month(b.emision) = 1,  a.totrendes, 0.00)) mEne, " _
        & " sum(if( month(b.emision) = 2,  a.totrendes, 0.00)) mFeb, " _
        & " sum(if( month(b.emision) = 3,  a.totrendes, 0.00)) mMar, " _
        & " sum(if( month(b.emision) = 4,  a.totrendes, 0.00)) mAbr, " _
        & " sum(if( month(b.emision) = 5,  a.totrendes, 0.00)) mMay, " _
        & " sum(if( month(b.emision) = 6,  a.totrendes, 0.00)) mJun, " _
        & " sum(if( month(b.emision) = 7,  a.totrendes, 0.00)) mJul, " _
        & " sum(if( month(b.emision) = 8,  a.totrendes, 0.00)) mAgo, " _
        & " sum(if( month(b.emision) = 9,  a.totrendes, 0.00)) mSep, " _
        & " sum(if( month(b.emision) = 10, a.totrendes, 0.00)) mOct, " _
        & " sum(if( month(b.emision) = 11, a.totrendes, 0.00)) mNov, " _
        & " sum(if( month(b.emision) = 12, a.totrendes, 0.00)) mDic " _
        & " from jsvenrenfac a " _
        & " left join jsvenencfac b on (a.numfac = b.numfac and a.id_emp = b.id_emp) " _
        & " left join jsmerequmer n on (a.item = n.codart and a.unidad = n.uvalencia and a.id_emp = n.id_emp) " _
        & " inner join jsmerctainv c on (a.item = c.codart and a.id_emp = c.id_emp) " _
        & " inner join jsvencatcli d on (b.codcli = d.codcli and b.id_emp = d.id_emp )" _
        & " Where " _
        & strMercas & strClientes _
        & " year(b.emision) = " & Año & " and " _
        & " a.id_emp = '" & jytsistema.WorkID & "' " _
        & " GROUP by a.item ")

        ft.Ejecutar_strSQL(myconn, " insert into " & tbldropsize _
        & " select 0, 'Cantidades (UV)', if(isnull(sum(mEne)),0.00, sum(mEne)) , if( isnull(sum(mFeb)), 0.00, sum(mFeb)) , " _
        & " if( isnull(sum(mMar)), 0.00, sum(mMar)), if(isnull(sum(mAbr)), 0.00, sum(mAbr)), if( isnull(sum(mMay)), 0.00, sum(mMay)), " _
        & " if( isnull(sum(mJun)), 0.00, sum(mJun)), if(isnull(sum(mJul)), 0.00, sum(mJul)), if( isnull(sum(mAgo)), 0.00, sum(mAgo)), " _
        & " if( isnull(sum(mSep)), 0.00, sum(mSep)), if(isnull(sum(mOct)), 0.00, sum(mOct)), if( isnull(sum(mNov)), 0.00, sum(mNov)), " _
        & " if( isnull(sum(mDic)), 0.00, sum(mDic)) from " & tblcantidad)

        ft.Ejecutar_strSQL(myconn, " insert into " & tbldropsize _
        & " select 0, 'Peso (KGS)', if(isnull(sum(mEne)),0.00, sum(mEne)) , if( isnull(sum(mFeb)), 0.00, sum(mFeb)) , " _
        & " if( isnull(sum(mMar)), 0.00, sum(mMar)), if(isnull(sum(mAbr)), 0.00, sum(mAbr)), if( isnull(sum(mMay)), 0.00, sum(mMay)), " _
        & " if( isnull(sum(mJun)), 0.00, sum(mJun)), if(isnull(sum(mJul)), 0.00, sum(mJul)), if( isnull(sum(mAgo)), 0.00, sum(mAgo)), " _
        & " if( isnull(sum(mSep)), 0.00, sum(mSep)), if(isnull(sum(mOct)), 0.00, sum(mOct)), if( isnull(sum(mNov)), 0.00, sum(mNov)), " _
        & " if( isnull(sum(mDic)), 0.00, sum(mDic)) from " & tblkilos)

        ft.Ejecutar_strSQL(myconn, " insert into " & tbldropsize _
        & " select 0, 'Financieros (Bs.)', if(isnull(sum(mEne)),0.00, sum(mEne)) , if( isnull(sum(mFeb)), 0.00, sum(mFeb)) , " _
        & " if( isnull(sum(mMar)), 0.00, sum(mMar)), if(isnull(sum(mAbr)), 0.00, sum(mAbr)), if( isnull(sum(mMay)), 0.00, sum(mMay)), " _
        & " if( isnull(sum(mJun)), 0.00, sum(mJun)), if(isnull(sum(mJul)), 0.00, sum(mJul)), if( isnull(sum(mAgo)), 0.00, sum(mAgo)), " _
        & " if( isnull(sum(mSep)), 0.00, sum(mSep)), if(isnull(sum(mOct)), 0.00, sum(mOct)), if( isnull(sum(mNov)), 0.00, sum(mNov)), " _
        & " if( isnull(sum(mDic)), 0.00, sum(mDic)) from " & tblMoney)

        ft.Ejecutar_strSQL(myconn, " insert into " & tbldropsize _
        & " select 0, 'Facturas', " _
        & " sum(if(month(a.emision) = 1, 1,0 )) mEne, " _
        & " sum(if(month(a.emision) = 2, 1,0 )) mfeb, " _
        & " sum(if(month(a.emision) = 3, 1,0 )) mMar, " _
        & " sum(if(month(a.emision) = 4, 1,0 )) mAbr, " _
        & " sum(if(month(a.emision) = 5, 1,0 )) mMay, " _
        & " sum(if(month(a.emision) = 6, 1,0 )) mJun, " _
        & " sum(if(month(a.emision) = 7, 1,0 )) mJul, " _
        & " sum(if(month(a.emision) = 8, 1,0 )) mAgo, " _
        & " sum(if(month(a.emision) = 9, 1,0 )) mSep, " _
        & " sum(if(month(a.emision) = 10, 1,0 )) mOct, " _
        & " sum(if(month(a.emision) = 11, 1,0 )) mNov, " _
        & " sum(if(month(a.emision) = 12, 1,0 )) mDic " _
        & " from jsvenencfac a " _
        & " left join jsvencatcli d on (a.codcli = d.codcli and a.id_emp = d.id_emp ) " _
        & " Where " _
        & strClientes _
        & " year(a.emision) = " & Año & " and " _
        & " a.id_emp = '" & jytsistema.WorkID & "' ")

        ft.Ejecutar_strSQL(myconn, " insert into " & tbldropsize _
        & " select 0, 'Pedidos', " _
        & " sum(if(month(a.emision) = 1, 1,0 )) mEne, " _
        & " sum(if(month(a.emision) = 2, 1,0 )) mfeb, " _
        & " sum(if(month(a.emision) = 3, 1,0 )) mMar, " _
        & " sum(if(month(a.emision) = 4, 1,0 )) mAbr, " _
        & " sum(if(month(a.emision) = 5, 1,0 )) mMay, " _
        & " sum(if(month(a.emision) = 6, 1,0 )) mJun, " _
        & " sum(if(month(a.emision) = 7, 1,0 )) mJul, " _
        & " sum(if(month(a.emision) = 8, 1,0 )) mAgo, " _
        & " sum(if(month(a.emision) = 9, 1,0 )) mSep, " _
        & " sum(if(month(a.emision) = 10, 1,0 )) mOct, " _
        & " sum(if(month(a.emision) = 11, 1,0 )) mNov, " _
        & " sum(if(month(a.emision) = 12, 1,0 )) mDic " _
        & " from jsvenencped a " _
        & " left join jsvenrenped b on (a.numped = b.numped and a.id_emp = b.id_emp) " _
        & " inner join jsmerctainv c on (b.item = c.codart and b.id_emp = c.id_emp )" _
        & " left join jsvencatcli d on (a.codcli = d.codcli and a.id_emp = d.id_emp ) " _
        & " Where " _
        & strMercas & strClientes _
        & " year(a.emision) = " & Año & " and " _
        & " a.id_emp = '" & jytsistema.WorkID & "'")

        ft.Ejecutar_strSQL(myconn, " insert into " & tbldropsize _
            & " select 0, 'Visitas', " _
            & " count(IF( d.INGRESO <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(CDate("01/01/" & CStr(Año)))) & "', d.codcli, null)) mEne, " _
            & " count(IF( d.INGRESO <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(CDate("01/02/" & CStr(Año)))) & "', d.codcli, null)) mfeb, " _
            & " count(IF( d.INGRESO <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(CDate("01/03/" & CStr(Año)))) & "', d.codcli, null)) mMar, " _
            & " count(IF( d.INGRESO <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(CDate("01/04/" & CStr(Año)))) & "', d.codcli, null)) mAbr, " _
            & " count(IF( d.INGRESO <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(CDate("01/05/" & CStr(Año)))) & "', d.codcli, null)) mMay, " _
            & " count(IF( d.INGRESO <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(CDate("01/06/" & CStr(Año)))) & "', d.codcli, null)) mJun, " _
            & " count(IF( d.INGRESO <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(CDate("01/07/" & CStr(Año)))) & "', d.codcli, null)) mJul, " _
            & " count(IF( d.INGRESO <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(CDate("01/08/" & CStr(Año)))) & "', d.codcli, null)) mAgo, " _
            & " count(IF( d.INGRESO <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(CDate("01/09/" & CStr(Año)))) & "', d.codcli, null)) mSep, " _
            & " count(IF( d.INGRESO <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(CDate("01/10/" & CStr(Año)))) & "', d.codcli, null)) mOct, " _
            & " count(IF( d.INGRESO <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(CDate("01/11/" & CStr(Año)))) & "', d.codcli, null)) mNov, " _
            & " count(IF( d.INGRESO <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(CDate("01/12/" & CStr(Año)))) & "', d.codcli, null)) mDic " _
            & " from jsvencatcli d " _
            & " Where " _
            & strClientes _
            & " d.ruta_visita <> '' and " _
            & " d.estatus <= 1 and " _
            & " d.id_emp = '" & jytsistema.WorkID & "'")


        ft.Ejecutar_strSQL(myconn, " insert into " & tbldropsize _
            & " select 0, 'Clientes activos', " _
            & " count(distinct if(month(a.emision) = 1, a.codcli, null )) mEne, " _
            & " count(distinct if(month(a.emision) = 2, a.codcli, null )) mfeb, " _
            & " count(distinct if(month(a.emision) = 3, a.codcli, null )) mMar, " _
            & " count(distinct if(month(a.emision) = 4, a.codcli, null )) mAbr, " _
            & " count(distinct if(month(a.emision) = 5, a.codcli, null )) mMay, " _
            & " count(distinct if(month(a.emision) = 6, a.codcli, null )) mJun, " _
            & " count(distinct if(month(a.emision) = 7, a.codcli, null )) mJul, " _
            & " count(distinct if(month(a.emision) = 8, a.codcli, null )) mAgo, " _
            & " count(distinct if(month(a.emision) = 9, a.codcli, null )) mSep, " _
            & " count(distinct if(month(a.emision) = 10, a.codcli, null )) mOct, " _
            & " count(distinct if(month(a.emision) = 11, a.codcli, null )) mNov, " _
            & " count(distinct if(month(a.emision) = 12, a.codcli, null )) mDic " _
            & " from jsvenencfac a " _
            & " left join jsvencatcli d on (a.codcli = d.codcli and a.id_emp = d.id_emp ) " _
            & " Where " _
            & strClientes _
            & " year(a.emision) = " & Año & " and " _
            & " a.id_emp = '" & jytsistema.WorkID & "'")

        ft.Ejecutar_strSQL(myconn, " insert into " & tbldropsize_b & " select * FROM " & tbldropsize)
        ft.Ejecutar_strSQL(myconn, " insert into " & tbldropsize_c & " select * FROM " & tbldropsize)

        ft.Ejecutar_strSQL(myconn, " insert into " & tbldropsize _
            & " select 1, 'Cantidad (UV) / Número Facturas ', " _
            & " if( b.mEne > 0 , sum(a.mEne)/b.mEne, 0 ) mEne, " _
            & " if( b.mFeb > 0 , sum(a.mFeb)/b.mFeb, 0 ) mFeb, " _
            & " if( b.mMar > 0 , sum(a.mMar)/b.mMar, 0 ) mMar, " _
            & " if( b.mAbr > 0 , sum(a.mAbr)/b.mAbr, 0 ) mAbr, " _
            & " if( b.mMay > 0 , sum(a.mMay)/b.mMay, 0 ) mMay, " _
            & " if( b.mJun > 0 , sum(a.mJun)/b.mJun, 0 ) mJun, " _
            & " if( b.mJul > 0 , sum(a.mJul)/b.mJul, 0 ) mJul, " _
            & " if( b.mAgo > 0 , sum(a.mAgo)/b.mAgo, 0 ) mAgo, " _
            & " if( b.mSep > 0 , sum(a.mSep)/b.mSep, 0 ) mSep, " _
            & " if( b.mOct > 0 , Sum(a.mOct)/b.mOct, 0 ) mOct, " _
            & " if( b.mNov > 0 , Sum(a.mNov)/b.mNov, 0 ) mNov, " _
            & " if( b.mDic > 0 , Sum(a.mDic)/b.mDic, 0 ) mDic " _
            & " from " & tblcantidad & " a " _
            & " left join " & tbldropsize_b & " b ON (b.codigo = 'Facturas') " _
            & " Having " _
            & " Not isnull(mEne)")

        ft.Ejecutar_strSQL(myconn, " insert into " & tbldropsize _
            & " select 1, 'Peso (Kgs) / Número Facturas', " _
            & " if( b.mEne > 0 , sum(a.mEne)/b.mEne, 0 ) mEne, " _
            & " if( b.mFeb > 0 , sum(a.mFeb)/b.mFeb, 0 ) mFeb, " _
            & " if( b.mMar > 0 , sum(a.mMar)/b.mMar, 0 ) mMar, " _
            & " if( b.mAbr > 0 , sum(a.mAbr)/b.mAbr, 0 ) mAbr, " _
            & " if( b.mMay > 0 , sum(a.mMay)/b.mMay, 0 ) mMay, " _
            & " if( b.mJun > 0 , sum(a.mJun)/b.mJun, 0 ) mJun, " _
            & " if( b.mJul > 0 , sum(a.mJul)/b.mJul, 0 ) mJul, " _
            & " if( b.mAgo > 0 , sum(a.mAgo)/b.mAgo, 0 ) mAgo, " _
            & " if( b.mSep > 0 , sum(a.mSep)/b.mSep, 0 ) mSep, " _
            & " if( b.mOct > 0 , Sum(a.mOct)/b.mOct, 0 ) mOct, " _
            & " if( b.mNov > 0 , Sum(a.mNov)/b.mNov, 0 ) mNov, " _
            & " if( b.mDic > 0 , Sum(a.mDic)/b.mDic, 0 ) mDic " _
            & " from " & tblkilos & " a " _
            & " left join " & tbldropsize_b & " b ON (b.codigo = 'Facturas') " _
            & " Having " _
            & " Not isnull(mEne) ")


        ft.Ejecutar_strSQL(myconn, " insert into " & tbldropsize _
            & " select 1, 'Financieros (Bs.) / Número Facturas', " _
            & " if( b.mEne > 0 , a.mEne/b.mEne, 0 ) mEne, " _
            & " if( b.mFeb > 0 , a.mFeb/b.mFeb, 0 ) mFeb, " _
            & " if( b.mMar > 0 , a.mMar/b.mMar, 0 ) mMar, " _
            & " if( b.mAbr > 0 , a.mAbr/b.mAbr, 0 ) mAbr, " _
            & " if( b.mMay > 0 , a.mMay/b.mMay, 0 ) mMay, " _
            & " if( b.mJun > 0 , a.mJun/b.mJun, 0 ) mJun, " _
            & " if( b.mJul > 0 , a.mJul/b.mJul, 0 ) mJul, " _
            & " if( b.mAgo > 0 , a.mAgo/b.mAgo, 0 ) mAgo, " _
            & " if( b.mSep > 0 , a.mSep/b.mSep, 0 ) mSep, " _
            & " if( b.mOct > 0 , a.mOct/b.mOct, 0 ) mOct, " _
            & " if( b.mNov > 0 , a.mNov/b.mNov, 0 ) mNov, " _
            & " if( b.mDic > 0 , a.mDic/b.mDic, 0 ) mDic " _
            & " from " & tbldropsize_b & " a " _
            & " left join " & tbldropsize_c & " b ON (a.codigo = 'Financieros (Bs.)' and b.codigo = 'Facturas')" _
            & " Where " _
            & " Not isnull(b.mEne) ")

        ft.Ejecutar_strSQL(myconn, " insert into " & tbldropsize _
            & " select 2, 'Número Pedidos / Número visitas', " _
            & " if( b.mEne > 0 , a.mEne/b.mEne, 0 ) mEne, " _
            & " if( b.mFeb > 0 , a.mFeb/b.mFeb, 0 ) mFeb, " _
            & " if( b.mMar > 0 , a.mMar/b.mMar, 0 ) mMar, " _
            & " if( b.mAbr > 0 , a.mAbr/b.mAbr, 0 ) mAbr, " _
            & " if( b.mMay > 0 , a.mMay/b.mMay, 0 ) mMay, " _
            & " if( b.mJun > 0 , a.mJun/b.mJun, 0 ) mJun, " _
            & " if( b.mJul > 0 , a.mJul/b.mJul, 0 ) mJul, " _
            & " if( b.mAgo > 0 , a.mAgo/b.mAgo, 0 ) mAgo, " _
            & " if( b.mSep > 0 , a.mSep/b.mSep, 0 ) mSep, " _
            & " if( b.mOct > 0 , a.mOct/b.mOct, 0 ) mOct, " _
            & " if( b.mNov > 0 , a.mNov/b.mNov, 0 ) mNov, " _
            & " if( b.mDic > 0 , a.mDic/b.mDic, 0 ) mDic " _
            & " from " & tbldropsize_b & " a " _
            & " left join " & tbldropsize_c & "  b ON (a.codigo = 'Pedidos' and b.codigo = 'Visitas') " _
            & " Where " _
            & " Not isnull(b.mEne) ")

        ft.Ejecutar_strSQL(myconn, " insert into " & tbldropsize _
           & " select 3, 'Cantidades (UV) / Clientes activos', " _
           & " if( b.mEne > 0 , a.mEne/b.mEne, 0 ) mEne, " _
           & " if( b.mFeb > 0 , a.mFeb/b.mFeb, 0 ) mFeb, " _
           & " if( b.mMar > 0 , a.mMar/b.mMar, 0 ) mMar, " _
           & " if( b.mAbr > 0 , a.mAbr/b.mAbr, 0 ) mAbr, " _
           & " if( b.mMay > 0 , a.mMay/b.mMay, 0 ) mMay, " _
           & " if( b.mJun > 0 , a.mJun/b.mJun, 0 ) mJun, " _
           & " if( b.mJul > 0 , a.mJul/b.mJul, 0 ) mJul, " _
           & " if( b.mAgo > 0 , a.mAgo/b.mAgo, 0 ) mAgo, " _
           & " if( b.mSep > 0 , a.mSep/b.mSep, 0 ) mSep, " _
           & " if( b.mOct > 0 , a.mOct/b.mOct, 0 ) mOct, " _
           & " if( b.mNov > 0 , a.mNov/b.mNov, 0 ) mNov, " _
           & " if( b.mDic > 0 , a.mDic/b.mDic, 0 ) mDic " _
           & " from " & tbldropsize_b & " a " _
           & " left join " & tbldropsize_c & " b ON (a.codigo = 'Cantidades (UV)' and b.codigo = 'Clientes activos') " _
           & " Where " _
           & " Not isnull(b.mEne) ")

        ft.Ejecutar_strSQL(myconn, " insert into " & tbldropsize _
        & " select 3, 'Peso (KGS) / Clientes activos', " _
        & " if( b.mEne > 0 , a.mEne/b.mEne, 0 ) mEne, " _
        & " if( b.mFeb > 0 , a.mFeb/b.mFeb, 0 ) mFeb, " _
        & " if( b.mMar > 0 , a.mMar/b.mMar, 0 ) mMar, " _
        & " if( b.mAbr > 0 , a.mAbr/b.mAbr, 0 ) mAbr, " _
        & " if( b.mMay > 0 , a.mMay/b.mMay, 0 ) mMay, " _
        & " if( b.mJun > 0 , a.mJun/b.mJun, 0 ) mJun, " _
        & " if( b.mJul > 0 , a.mJul/b.mJul, 0 ) mJul, " _
        & " if( b.mAgo > 0 , a.mAgo/b.mAgo, 0 ) mAgo, " _
        & " if( b.mSep > 0 , a.mSep/b.mSep, 0 ) mSep, " _
        & " if( b.mOct > 0 , a.mOct/b.mOct, 0 ) mOct, " _
        & " if( b.mNov > 0 , a.mNov/b.mNov, 0 ) mNov, " _
        & " if( b.mDic > 0 , a.mDic/b.mDic, 0 ) mDic " _
        & " from " & tbldropsize_b & " a " _
        & " left join " & tbldropsize_c & " b ON (a.codigo = 'Peso (KGS)' and b.codigo = 'Clientes activos') " _
        & " Where " _
        & " Not isnull(b.mEne) ")

        ft.Ejecutar_strSQL(myconn, " insert into " & tbldropsize _
        & " select 3,'Financieros (Bs.) / Clientes Activos', " _
        & " if( b.mEne > 0 , a.mEne/b.mEne, 0 ) mEne, " _
        & " if( b.mFeb > 0 , a.mFeb/b.mFeb, 0 ) mFeb, " _
        & " if( b.mMar > 0 , a.mMar/b.mMar, 0 ) mMar, " _
        & " if( b.mAbr > 0 , a.mAbr/b.mAbr, 0 ) mAbr, " _
        & " if( b.mMay > 0 , a.mMay/b.mMay, 0 ) mMay, " _
        & " if( b.mJun > 0 , a.mJun/b.mJun, 0 ) mJun, " _
        & " if( b.mJul > 0 , a.mJul/b.mJul, 0 ) mJul, " _
        & " if( b.mAgo > 0 , a.mAgo/b.mAgo, 0 ) mAgo, " _
        & " if( b.mSep > 0 , a.mSep/b.mSep, 0 ) mSep, " _
        & " if( b.mOct > 0 , a.mOct/b.mOct, 0 ) mOct, " _
        & " if( b.mNov > 0 , a.mNov/b.mNov, 0 ) mNov, " _
        & " if( b.mDic > 0 , a.mDic/b.mDic, 0 ) mDic " _
        & " from " & tbldropsize_b & " a " _
        & " left join " & tbldropsize_c & " b ON (a.codigo = 'Financieros (Bs.)' and b.codigo = 'Clientes activos') " _
        & " Where " _
        & " Not isnull(b.mEne) ")


        SeleccionSIGMEDropSize = " select elt(tipo+1,'Indicadores','Drop Size - I','Drop size - II','Drop size - III') tipo, codigo, " _
            & " if(isnull(mEne), 0.00, mEne) mEne , if(isnull(mFeb), 0.00, mFeb) mFeb, if(isnull(mMar), 0.00, mMar) mMar, " _
            & " if(isnull(mAbr), 0.00, mAbr) mAbr , if(isnull(mMay), 0.00, mMay) mMay, if(isnull(mJun), 0.00, mJun) mJun, " _
            & " if(isnull(mJul), 0.00, mJul) mJul , if(isnull(mAgo), 0.00, mAgo) mAgo, if(isnull(mSep), 0.00, mSep) mSep, " _
            & " if(isnull(mOct), 0.00, mOct) mOct , if(isnull(mNov), 0.00, mNov) mNov, if(isnull(mDic), 0.00, mDic) mDic from " & tbldropsize & " order by tipo, codigo "

    End Function



End Module
