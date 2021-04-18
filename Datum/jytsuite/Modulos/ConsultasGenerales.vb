Module ConsultasGenerales
    Private ft As New Transportables
    Public Function SeleccionGENTablaSimple(ByVal modulo As String) As String
        SeleccionGENTablaSimple = " select * from jsconctatab where modulo = '" & modulo & "' and id_emp = '" & jytsistema.WorkID & "' order by codigo "
    End Function
    Public Function SeleccionGENComentarios(ByVal Origen As String) As String
        Return " select codigo, comentario from jsconctacom " _
            & " where " _
            & " origen = '" & Origen & "' and " _
            & IIf(Origen = "COT", " substring(comentario,1,1) = '*' AND ", "") _
            & " id_emp = '" & jytsistema.WorkID & "' order by codigo "
    End Function
    Public Function SeleccionGENTablaEquivalencias() As String
        Return " SELECT a.codart, a.unidad, a.equivale, a.uvalencia, a.id_emp  " _
                & " 			FROM jsmerequmer a  " _
                & "             WHERE " _
                & " 			a.id_emp = '" & jytsistema.WorkID & "' " _
                & "             UNION " _
                & " 			SELECT a.codart, a.unidad, 1 equivale, a.unidad uvalencia, a.id_emp " _
                & " 			FROM jsmerctainv a " _
                & " 			WHERE  " _
                & " 			a.id_emp = '" & jytsistema.WorkID & "' "

    End Function

    ''' <summary>
    ''' Devuelve las existencias de todas las mercancías a una fecha deseada
    ''' </summary>
    ''' <param name="FechaHasta">Fecha para la cual deseas ver existencia</param>
    ''' <param name="FiltroAlmacen">Cadena ej. "b.Almacen in ('00001') and " (letra debe ser b.) "</param>
    ''' <param name="CampoUnidadDeseado">Campo de la UNIDAD en BD que se desea ver la existencia (letra debe ser b.)"</param>
    ''' <remarks></remarks>
    Public Function SeleccionGENExistenciasAFecha(FechaHasta As Date, FiltroAlmacen As String, _
                                                  Optional CampoUnidadDeseado As String = "b.unidad", _
                                                  Optional CampoCantidadDeseado As String = "b.cantidad", _
                                                  Optional CampoPesoUnidad As String = "") As String

        Return " SELECT b.codart, SUM(cantidad)" & CampoPesoUnidad & " Existencia, b.id_emp  " _
            & "  FROM (SELECT b.codart, b.unidad, SUM(IF( b.TIPOMOV IN ('EN','AE'), 1, IF( b.tipomov IN ('SA', 'AS'), -1, 0) )*" & CampoCantidadDeseado & ") cantidad, " _
            & "         b.id_emp " _
            & "         FROM jsmertramer b " _
            & "         LEFT JOIN ( " & SeleccionGENTablaEquivalencias() & ") n ON (b.codart = n.codart AND " & CampoUnidadDeseado & " = n.uvalencia AND b.id_emp = n.id_emp) " _
            & "         WHERE " _
            & FiltroAlmacen _
            & "         b.tipomov <> 'AC'  AND " _
            & "         b.id_emp = '" & jytsistema.WorkID & "' AND " _
            & "         DATE_FORMAT(b.fechamov, '%Y-%m-%d') <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' " _
            & "         GROUP BY b.codart, b.unidad) b " _
            & " LEFT JOIN jsmerctainv a ON ( b.codart = a.codart AND b.id_emp = b.id_emp )" _
            & " GROUP BY b.codart "

    End Function

    Public Function SeleccionGENClientes(Optional TipoRuta As Integer = 0) As String
        Return " SELECT a.*, IFNULL(b.descrip,'') NOMBRECANAL, IFNULL(c.descrip,'') NOMBRETIPONEGOCIO, " _
            & " IFNULL(e.codrut,'') CODRUT, IFNULL(e.nomrut,'') NOMRUT, IFNULL(e.numero,0) NUMERO_EN_RUTA, " _
            & " IFNULL(e.tipo,0) TIPORUTA, IFNULL(e.dia,0) DIA, IFNULL(e.codzon, '') CODZON, IFNULL(e.NOMZONA,'') NOMZONA, " _
            & " IFNULL(e.codven,'') CODVEN, IFNULL(e.NOMVENDEDOR,'') NOMBRE_VENDEDOR,  " _
            & " IFNULL(m.NOMBRE, '') BARRIOFISCAL, IFNULL(n.NOMBRE, '') CIUDADFISCAL, " _
            & " IFNULL(o.NOMBRE, '') PARROQUIAFISCAL, IFNULL(p.NOMBRE, '') MUNICIPIOFISCAL, " _
            & " IFNULL(q.NOMBRE, '') ESTADOFISCAL, IFNULL(r.NOMBRE, '') PAISFISCAL, " _
            & " IFNULL(s.NOMBRE, '') BARRIODESPACHO, IFNULL(t.NOMBRE, '') CIUDADDESPACHO, " _
            & " IFNULL(u.NOMBRE, '') PARROQUIADESPACHO, " _
            & " IFNULL(v.NOMBRE, '') MUNICIPIODESPACHO, IFNULL(w.NOMBRE, '') ESTADODESPACHO, " _
            & " IFNULL(x.NOMBRE, '') PAISDESPACHO " _
            & " from jsvencatcli a " _
            & " left join jsvenliscan b on (a.categoria = b.codigo and a.id_emp = b.id_emp ) " _
            & " left join jsvenlistip c on (a.unidad = c.codigo and a.id_emp = c.id_emp ) " _
            & " left join (SELECT a.codrut, b.nomrut, a.numero, a.cliente, a.tipo, b.codzon, c.descrip nomZona, " _
            & "             b.codven, CONCAT(d.nombres, ' ', d.apellidos) nomVendedor, b.dia,  a.id_emp " _
            & "             FROM jsvenrenrut a " _
            & "             LEFT JOIN jsvenencrut b ON (a.codrut = b.codrut AND a.tipo = b.tipo AND a.id_emp = b.id_emp) " _
            & "             LEFT JOIN jsconctatab c ON (b.codzon = c.codigo AND b.id_emp = c.id_emp AND c.modulo = '00005') " _
            & "             LEFT JOIN jsvencatven d ON (b.codven = d.codven AND b.id_emp = d.id_emp AND d.tipo = '0') " _
            & "             WHERE " _
            & "             a.id_emp = '" & jytsistema.WorkID & "' ) e on (a.codcli = e.cliente and a.id_emp = e.id_emp) " _
            & " left join jsconcatter m on (a.fbarrio = m.codigo and a.id_emp = m.id_emp ) " _
            & " left join jsconcatter n on (a.fciudad = n.codigo and a.id_emp = n.id_emp ) " _
            & " left join jsconcatter o on (a.fparroquia = o.codigo and a.id_emp = o.id_emp ) " _
            & " left join jsconcatter p on (a.fmunicipio = p.codigo and a.id_emp = p.id_emp ) " _
            & " left join jsconcatter q on (a.festado = q.codigo and a.id_emp = q.id_emp ) " _
            & " left join jsconcatter r on (a.fpais = r.codigo and a.id_emp = r.id_emp ) " _
            & " left join jsconcatter s on (a.dbarrio = s.codigo and a.id_emp = s.id_emp ) " _
            & " left join jsconcatter t on (a.dciudad = t.codigo and a.id_emp = t.id_emp ) " _
            & " left join jsconcatter u on (a.dparroquia = u.codigo and a.id_emp = u.id_emp ) " _
            & " left join jsconcatter v on (a.dmunicipio = v.codigo and a.id_emp = v.id_emp ) " _
            & " left join jsconcatter w on (a.destado = w.codigo and a.id_emp = w.id_emp ) " _
            & " left join jsconcatter x on (a.dpais = x.codigo and a.id_emp = x.id_emp ) " _
            & " where " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
            & " GROUP BY a.codcli "
    End Function

    Public Function SeleccionGENMercancias()

        Return " SELECT a.*, " _
            & " IFNULL(b.descrip,'') NOMBRECATEGORIA, IFNULL(c.descrip,'') NOMBREMARCA, IFNULL(d.descrip,'') NOMBREJERARQUIA," _
            & " IFNULL(e.descrip,'') NOMBREDIVISION " _
            & " FROM jsmerctainv a " _
            & " left join jsconctatab b on (a.grupo = b.codigo and a.id_emp = b.id_emp and b.modulo = '00002') " _
            & " left join jsconctatab c on (a.marca = c.codigo and a.id_emp = c.id_emp and c.modulo = '00003') " _
            & " left join jsmerencjer d on (a.tipjer = d.tipjer and a.id_emp = d.id_emp) " _
            & " left join jsmercatdiv e on (a.division = e.division and a.id_emp = e.id_emp) " _
            & " WHERE " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " GROUP BY a.CODART "

    End Function
    
    Public Function SeleccionGENProveedores() As String

        Return " SELECT  a.*, f.categoriaprov, f.unidadnegocioprov, h.descrip zonanombre " _
                & " FROM jsprocatpro a " _
                & " LEFT JOIN ( SELECT a.codigo codigocategoria, a.antec codigounidad, a.descrip categoriaprov, g.descrip unidadnegocioprov " _
                & "				FROM jsproliscat a " _
                & "      		LEFT JOIN jsprolisuni g ON (a.antec = g.codigo  AND a.id_emp = g.id_emp) " _
                & "             WHERE " _
                & "     		a.id_emp = '" & jytsistema.WorkID & "' )  f ON (a.unidad = f.codigounidad AND a.categoria = f.codigocategoria) " _
                & " LEFT JOIN jsconctatab h on (a.zona = h.codigo and a.id_emp = h.id_emp  and h.modulo = '00008') " _
                & " WHERE " _
                & " a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.codpro "

    End Function

    Public Function SeleccionGENTablaIVA(FechaIVA As Date) As String
        Return " SELECT a.fecha, a.tipo, b.monto FROM (SELECT MAX(fecha) fecha, tipo " _
            & " FROM jsconctaiva " _
            & " WHERE fecha <= '" & ft.FormatoFechaMySQL(FechaIVA) & "' " _
            & " GROUP BY tipo) a " _
            & " LEFT JOIN jsconctaiva b ON (a.fecha = b.fecha AND a.tipo = b.tipo) " _
            & " ORDER BY a.tipo "

        'Return "SELECT fecha, tipo, monto " _
        '    & " FROM jsconctaiva " _
        '    & " WHERE " _
        '    & " fecha IN (SELECT MAX(fecha) FROM jsconctaiva WHERE fecha <= '" & ft.FormatoFechaMySQL(FechaIVA) & "' GROUP BY tipo) AND " _
        '    & " fecha <= '" & ft.FormatoFechaMySQL(FechaIVA) & "' "

    End Function

    Public Function SeleccionGENResumenIVADocumento(TablaEnBD As String, CampoPrincipal As String, CampoClienteProveedor As String) As String
        Return " SELECT a." & CampoPrincipal & ", a." & CampoClienteProveedor & ", " _
            & " SUM(  IF ( tipoiva IN ('','E'), a.baseiva, 0) ) EXENTO,  " _
            & " SUM(IF(tipoiva = 'A', a.baseiva, 0) ) BASEIVA_A, SUM(IF(tipoiva = 'A', a.IMPIVA, 0) ) IMPIVA_A, " _
            & " SUM(IF(tipoiva = 'B', a.baseiva, 0) ) BASEIVA_B, SUM(IF(tipoiva = 'B', a.IMPIVA, 0) ) IMPIVA_B, " _
            & " SUM(IF(tipoiva = 'C', a.baseiva, 0) ) BASEIVA_C, SUM(IF(tipoiva = 'C', a.IMPIVA, 0) ) IMPIVA_C, " _
            & " a.id_emp " _
            & " FROM " & TablaEnBD & " a " _
            & " WHERE " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " GROUP BY a." & CampoPrincipal & ", a." & CampoClienteProveedor & " "
    End Function

End Module
