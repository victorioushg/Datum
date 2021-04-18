Imports MySql.Data.MySqlClient
Module ConsultasVentasPlus

    Private ft As New Transportables
    Public Structure TypeRerport
        Dim strCan As String
        Dim strUnidad As String
        Dim strUND As String
    End Structure

    Public Function CadenaPLUS_CLIENTES(letra As String, strClientes As String, _
                                strCanal As String, strTipoNegocio As String, _
                                strZona As String, strRuta As String, strAsesor As String, _
                                strPais As String, strEstado As String, strMunicipio As String, _
                                strParroquia As String, strCiudad As String, strBarrio As String, _
                                Optional ByVal TipoContribuyente As Integer = 9, _
                                Optional ByVal EstatusCliente As Integer = 9) As String

        Dim strCli As String = ""
        If strClientes.Trim() <> "" Then strCli += " " & letra & ".CODCLI " & strClientes & " AND "
        If strCanal.Trim() <> "" Then strCli += " " & letra & ".CATEGORIA " & strCanal & " AND "
        If strTipoNegocio.Trim() <> "" Then strCli += " " & letra & ".UNIDAD " & strTipoNegocio & " AND "
        If strZona.Trim() <> "" Then strCli += " " & letra & ".ZONA " & strZona & " AND "
        If strRuta.Trim() <> "" Then strCli += " " & letra & ".RUTA_VISITA " & strRuta & " AND "
        If strAsesor.Trim() <> "" Then strCli += " " & letra & ".VENDEDOR " & strAsesor & " AND "
        If strPais.Trim() <> "" Then strCli += " " & letra & ".FPAIS " & strPais & " AND "
        If strEstado.Trim() <> "" Then strCli += " " & letra & ".FESTADO " & strEstado & " AND "
        If strMunicipio.Trim() <> "" Then strCli += " " & letra & ".FMUNICIPIO " & strMunicipio & " AND "
        If strParroquia.Trim() <> "" Then strCli += " " & letra & ".FPARROQUIA " & strParroquia & " AND "
        If strCiudad.Trim() <> "" Then strCli += " " & letra & ".FCIUDAD " & strCiudad & " AND "
        If strBarrio.Trim() <> "" Then strCli += " " & letra & ".FBARRIO " & strBarrio & " AND "
        If EstatusCliente < 4 Then strCli += " " & letra & ".estatus = " & EstatusCliente & " and "
        If TipoContribuyente < 4 Then strCli += " " & letra & ".especial = " & TipoContribuyente & " and "

        Return strCli

    End Function

    Public Function CadenaPLUS_PROVEEDORES(letra As String, strProveedor As String, _
                            strCategoria As String, strUnidadNegocio As String, _
                            Optional ByVal EstatusCliente As Integer = 9) As String

        Dim strCli As String = ""
        If strProveedor.Trim() <> "" Then strCli += " " & letra & ".CODPRO " & strProveedor & " AND "
        If strCategoria.Trim() <> "" Then strCli += " " & letra & ".CATEGORIA " & strCategoria & " AND "
        If strUnidadNegocio.Trim() <> "" Then strCli += " " & letra & ".UNIDAD " & strUnidadNegocio & " AND "

        If EstatusCliente < 4 Then strCli += " " & letra & ".estatus = " & EstatusCliente & " and "

        Return strCli

    End Function

    Public Function CadenaPLUS_MERCANCIAS(letra As String, strMercas As String, strCategorias As String, strMarcas As String, strDivisiones As String, _
                                        strJerarquias As String, strNivel1 As String, strNivel2 As String, _
                                        strNivel3 As String, strNivel4 As String, strNivel5 As String, strNivel6 As String, _
                                        Optional Estatus As Integer = 2, Optional Cartera As Integer = 2, _
                                        Optional SoloPeso As Boolean = False, Optional TipoMercancia As Integer = 9, _
                                        Optional Regulada As Integer = 9) As String

        Dim strMER As String = ""
        If strMercas.Trim <> "" Then strMER += " " & letra & ".CODART " & strMercas & " AND "
        If strCategorias.Trim() <> "" Then strMER += " " & letra & ".GRUPO " & strCategorias & " AND "
        If strMarcas.Trim() <> "" Then strMER += " " & letra & ".MARCA " & strMarcas & " AND "
        If strDivisiones.Trim() <> "" Then strMER += " " & letra & ".DIVISION " & strDivisiones & " AND "
        If strJerarquias.Trim() <> "" Then strMER += " " & letra & ".TIPJER " & strJerarquias & " AND "
        If strNivel1.Trim() <> "" Then strMER += " " & letra & ".CODJER1 " & strNivel1 & " AND "
        If strNivel2.Trim() <> "" Then strMER += " " & letra & ".CODJER2 " & strNivel2 & " AND "
        If strNivel3.Trim() <> "" Then strMER += " " & letra & ".CODJER3 " & strNivel3 & " AND "
        If strNivel4.Trim() <> "" Then strMER += " " & letra & ".CODJER4 " & strNivel4 & " AND "
        If strNivel5.Trim() <> "" Then strMER += " " & letra & ".CODJER5 " & strNivel5 & " AND "
        If strNivel6.Trim() <> "" Then strMER += " " & letra & ".CODJER6 " & strNivel6 & " AND "
        If Estatus < 2 Then strMER += " " & letra & ".estatus = " & Estatus & " and "
        If Cartera < 2 Then strMER += " " & letra & ".cuota = " & Cartera & " and "
        If SoloPeso Then strMER += " " & letra & ".unidad = 'KGR' and "
        If TipoMercancia < 7 Then strMER += " " & letra & ".tipoart = " & TipoMercancia & " and "
        If Regulada = 0 Or Regulada = 1 Then
            strMER += " " & letra & ".regulado = " & IIf(Regulada = 0, 1, 0) & " and "
        End If

        Return strMER

    End Function

    Public Function CadenaPLUS_MERCANCIAS_MOVIMIENTOS(letra As String, strAlmacen As String) As String

        Dim strMER As String = ""
        If strAlmacen.Trim() <> "" Then strMER += " " & letra & ".ALMACEN " & strAlmacen & " AND "
        
        Return strMER

    End Function

    Function ReporteTipo(TipoReporte As Integer, letraMovimiento As String, letraCatalogo As String, _
                         letraEquivalencia As String) As TypeRerport

        ReporteTipo.strCan = ""
        ReporteTipo.strUnidad = ""
        ReporteTipo.strUND = ""

        Select Case TipoReporte
            Case 0 'UNIDAD DE VENTA
                ReporteTipo.strCan = " " & letraMovimiento & ".cantidad/" & letraEquivalencia & ".equivale "
                ReporteTipo.strUnidad = " " & letraMovimiento & ".unidad "
                ReporteTipo.strUND = " " & letraCatalogo & ".unidad "
            Case 1 'UNIDAD DE MEDIDA PRINCIPAL UMP (KGR)
                ReporteTipo.strCan = " " & letraMovimiento & ".peso "
                ReporteTipo.strUnidad = " " & letraMovimiento & ".unidad "
                ReporteTipo.strUND = " 'KGR' "
            Case 2 'MONETARIOS VENTAS
                ReporteTipo.strCan = " " & letraMovimiento & ".ventotaldes "
                ReporteTipo.strUnidad = " " & letraMovimiento & ".unidad "
                ReporteTipo.strUND = " 'BsF' "
            Case 3 'UNIDAD DE MEDIDA SECUNDARIA UMS (CAJ)
                ReporteTipo.strCan = " " & letraMovimiento & ".peso/" & letraCatalogo & ".pesounidad*" & letraEquivalencia & ".equivale "
                ReporteTipo.strUnidad = " 'CAJ' "
                ReporteTipo.strUND = " 'CAJ' "
            Case 4 'MONETARIOS COSTOS
                ReporteTipo.strCan = " " & letraMovimiento & ".COSTOTALDES "
                ReporteTipo.strUnidad = " " & letraMovimiento & ".unidad "
                ReporteTipo.strUND = " 'BsF' "
        End Select

    End Function

    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////77
    '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Function SeleccionVENPLUS_ActivacionClientesMercanciasPorAsesor(FechaDesde As Date, FechaHasta As Date, _
                                                                    strCliente As String, _
                                                                    strCanal As String, strTipoNegocio As String, _
                                                                    strZona As String, strRuta As String, strAsesor As String, _
                                                                    strPais As String, strEstado As String, strMunicipio As String, _
                                                                    strParroquia As String, strCiudad As String, strBarrio As String, _
                                                                    strMercas As String, _
                                                                    strCategorias As String, strMarcas As String, strDivisiones As String, _
                                                                    strJerarquias As String, strNivel1 As String, strNivel2 As String, _
                                                                    strNivel3 As String, strNivel4 As String, strNivel5 As String, strNivel6 As String)


        Dim str As String = " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaDesde) & "' and a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' AND "

        str += CadenaPLUS_MERCANCIAS("b", strMercas, strCategorias, strMarcas, strDivisiones, strJerarquias, strNivel1, strNivel2, strNivel3, strNivel4, strNivel5, strNivel6)

        If strAsesor.Trim() <> "" Then str += " a.VENDEDOR " & strAsesor & " AND "

        Return " SELECT a.vendedor, CONCAT(c.apellidos, ', ', c.nombres) NombreAsesor, COUNT(DISTINCT a.prov_cli) actClientes, d.actmercas " _
            & " FROM jsmertramer a " _
            & " LEFT JOIN jsmerctainv b ON (a.codart = b.codart AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvencatven c ON (a.vendedor = c.codven AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT a.vendedor, COUNT(DISTINCT a.codart) actMercas, a.id_emp " _
            & "             FROM jsmertramer a  " _
            & "             LEFT JOIN jsmerctainv b ON (a.codart = b.codart AND a.id_emp = b.id_emp) " _
            & "             LEFT JOIN jsvencatven c ON (a.vendedor = c.codven AND a.id_emp = c.id_emp) " _
            & "             WHERE " _
            & str _
            & "             a.origen IN ('FAC','PFC', 'PVE', 'NDV') AND " _
            & "             a.id_emp = '" & jytsistema.WorkID & "' " _
            & "             GROUP BY 1) d ON (a.vendedor = d.vendedor  AND a.id_emp = d.id_emp) " _
            & " WHERE " _
            & str _
            & " a.origen IN ('FAC','PFC', 'PVE', 'NDV') AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " GROUP BY 1 "

    End Function

    Function SeleccionVENPLUS_VentasAsesor(FechaDesde As Date, FechaHasta As Date, _
                                            strCliente As String, _
                                            strCanal As String, strTipoNegocio As String, _
                                            strZona As String, strRuta As String, strAsesor As String, _
                                            strPais As String, strEstado As String, strMunicipio As String, _
                                            strParroquia As String, strCiudad As String, strBarrio As String, _
                                            strMercancias As String, _
                                            strCategorias As String, strMarcas As String, strDivisiones As String, _
                                            strJerarquias As String, strNivel1 As String, strNivel2 As String, _
                                            strNivel3 As String, strNivel4 As String, strNivel5 As String, strNivel6 As String, _
                                            TipoReporte As Integer) As String

        Dim str As String = " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaDesde) & "' and a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' AND "

        str += CadenaPLUS_MERCANCIAS("b", strMercancias, strCategorias, strMarcas, strDivisiones, strJerarquias, strNivel1, strNivel2, strNivel3, strNivel4, strNivel5, strNivel6)

        If strAsesor.Trim() <> "" Then str += " a.VENDEDOR " & strAsesor & " AND "

        Dim strCan As String = ReporteTipo(TipoReporte, "a", "b", "m").strCan
        Dim strUnidad As String = ReporteTipo(TipoReporte, "a", "b", "m").strUnidad
        Dim strUND As String = ReporteTipo(TipoReporte, "a", "b", "m").strUND

        Return " SELECT a.vendedor, CONCAT(c.apellidos, ', ', c.nombres) NombreAsesor, ROUND(SUM(IF( a.origen = 'NCV', -1, 1)* " & strCan & " ),3) VOLUMEN " _
            & " FROM jsmertramer a " _
            & " LEFT JOIN jsmerctainv b ON (a.codart = b.codart AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvencatven c ON (a.vendedor = c.codven AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT a.codart, a.unidad, a.equivale, a.uvalencia, a.id_emp  " _
            & "      		FROM jsmerequmer a  " _
            & "             WHERE " _
            & "     		a.id_emp = '" & jytsistema.WorkID & "' " _
            & "             UNION " _
            & "           SELECT a.codart, a.unidad, 1 equivale, a.unidad uvalencia, a.id_emp  " _
            & "     		FROM jsmerctainv a " _
            & "     		WHERE  " _
            & "     		a.id_emp = '" & jytsistema.WorkID & "') m ON (a.codart = m.codart AND " & strUnidad & " = m.uvalencia AND a.id_emp = m.id_emp) " _
            & " WHERE " _
            & str _
            & " a.origen IN ('FAC','PFC', 'PVE', 'NDV','NCV') AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " GROUP BY 1 "


    End Function

    Function SeleccionVENPLUS_Clientes(ordenadoPor As String, strCanal As String, strTipoNegocio As String, _
                                       strZona As String, strRuta As String, strAsesor As String, _
                                       strPais As String, strEstado As String, strMunicipio As String, _
                                       strParroquia As String, strCiudad As String, strBarrio As String, _
                                       strCliente As String, _
                                       Optional ByVal TipoCliente As Integer = 4, Optional ByVal EstatusCliente As Integer = 4, _
                                       Optional Descuentos As Integer = 2) As String


        Dim str As String = CadenaPLUS_CLIENTES("a", strCliente, strCanal, strTipoNegocio, strZona, strRuta, strAsesor, strPais, _
                                                      strEstado, strMunicipio, strParroquia, strCiudad, strBarrio, _
                                                      TipoCliente, EstatusCliente)



        If Descuentos < 2 Then str += " a.codcre = 0 AND " & IIf(Descuentos = 0, " a.des_cli > 0 AND ", " a.des_cli = 0 AND ")

        SeleccionVENPLUS_Clientes = " select a.CODCLI, a.NOMBRE, a.ALTERNO, " _
                        & " a.RIF, a.NIT, a.DIRFISCAL, if(b.descrip is null, '', b.descrip) CANAL, if( c.descrip is null, '', c.descrip) TIPONEGOCIO, " _
                        & " if(e.nomrut is null, '', e.nomrut) RUTA, if( e.nomZona is null, '', e.nomzona) ZONA, if( e.codven is null, '', e.codven) VENDEDOR, a.EMAIL1, a.EMAIL2, a.EMAIL3, a.EMAIL4, " _
                        & " a.TELEF1, a.TELEF2, a.CONTACTO, a.TELCON, if( e.nomVendedor is null, '', e.nomVendedor) ASESOR, " _
                        & " a.REQ_RIF, a.REQ_NIT, a.REQ_CIS, a.REQ_REC, a.REQ_REG, a.REQ_REA, a.REQ_BAN, a.REQ_COM, " _
                        & " if( m.nombre is null, '', m.nombre) FBARRIO, if( n.nombre is null, '', n.nombre) FCIUDAD, if( o.nombre is null, '', o.nombre) FPARROQUIA, " _
                        & " if( p.nombre is null, '', p.nombre) FMUNICIPIO, if(q.nombre is null, '', q.nombre) FESTADO, if(r.nombre is null, '', r.nombre) FPAIS, " _
                        & " a.FZIP, a.DIRDESPA, " _
                        & " if( s.nombre is null, '', s.nombre) DBARRIO, if( t.nombre is null, '', t.nombre) DCIUDAD, if( u.nombre is null, '', u.nombre) DPARROQUIA, " _
                        & " if( v.nombre is null, '', v.nombre) DMUNICIPIO, if(w.nombre is null, '', w.nombre) DESTADO, if(x.nombre is null, '', x.nombre) DPAIS, " _
                        & " a.DZIP, " _
                        & " a.CODGEO, a.TELEF3, a.FAX, a.GERENTE, a.TELGER, a.NUM_VISITA, a.INGRESO, a.COMENTARIO, a.LIMITECREDITO, a.SALDO, " _
                        & " a.DISPONIBLE, a.ESPECIAL, a.ESTATUS, a.TARIFA, a.LISPRE, a.FORMAPAGO, a.BANCO, a.CTABANCO, a.TELEF1, a.TELEF2, a.ID_EMP " _
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
                        & "             a.id_emp = '" & jytsistema.WorkID & "' ) e on (a.vendedor = e.codven and a.codcli = e.cliente and a.id_emp = e.id_emp) " _
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
                        & " Where " _
                        & str _
                        & " a.id_emp = '" & jytsistema.WorkID & "'" _
                        & " group by a.codcli " _
                        & " order by a." & ordenadoPor _
                        & " "


    End Function

    Function SeleccionVENPLUS_VentasComparativasMesMes(strCliente As String, _
                                            strCanal As String, strTipoNegocio As String, _
                                            strZona As String, strRuta As String, strAsesor As String, _
                                            strPais As String, strEstado As String, strMunicipio As String, _
                                            strParroquia As String, strCiudad As String, strBarrio As String, _
                                            strMercancias As String, _
                                            strCategorias As String, strMarcas As String, strDivisiones As String, _
                                            strJerarquias As String, strNivel1 As String, strNivel2 As String, _
                                            strNivel3 As String, strNivel4 As String, strNivel5 As String, strNivel6 As String, _
                                            Optional ByVal CantidadPesoMoneda As Integer = 0) As String

        Dim str As String = ""
        Dim AñoActual As Integer, AñoAnterior As Integer

        AñoActual = Year(jytsistema.sFechadeTrabajo)
        AñoAnterior = Year(jytsistema.sFechadeTrabajo) - 1

        Dim strCan As String = ReporteTipo(CantidadPesoMoneda, "a", "c", "n").strCan
        Dim strUnidad As String = ReporteTipo(CantidadPesoMoneda, "a", "c", "n").strUnidad
        Dim strUND As String = ReporteTipo(CantidadPesoMoneda, "a", "c", "n").strUND

        str += CadenaPLUS_MERCANCIAS("c", strMercancias, strCategorias, strMarcas, strDivisiones, strJerarquias, strNivel1, strNivel2, strNivel3, strNivel4, strNivel5, strNivel6)
        str += CadenaPLUS_CLIENTES("d", "", strCanal, strTipoNegocio, strZona, strRuta, "", strPais, strEstado, strMunicipio, strParroquia, strCiudad, strBarrio)
        If strCliente.Trim() <> "" Then str += " a.PROV_CLI " & strCliente & " AND "
        If strAsesor.Trim() <> "" Then str += " a.VENDEDOR " & strAsesor & " AND "


        SeleccionVENPLUS_VentasComparativasMesMes = " select a.codart, c.nomart, " & strUND & " unidad, " _
            & " c.grupo categoria, c.marca, elt(mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') mix, c.division, c.tipjer, c.codjer, a.vendedor, a.prov_cli, " _
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
            & str _
            & " year(a.fechamov) >= " & AñoAnterior & " and year(a.fechamov) <= " & AñoActual & " and " _
            & " a.origen in ('FAC','NDV','PFC','PVE', 'NCV') and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by a.codart "

    End Function

    Function SeleccionVENPLUS_ComprasComparativasMesMes(strProveedor As String, _
                                            strCategpriaProveedor As String, strUnidadNegocio As String, _
                                            strMercancias As String, _
                                            strCategorias As String, strMarcas As String, strDivisiones As String, _
                                            strJerarquias As String, strNivel1 As String, strNivel2 As String, _
                                            strNivel3 As String, strNivel4 As String, strNivel5 As String, strNivel6 As String, _
                                            Optional ByVal CantidadPesoMoneda As Integer = 0) As String

        Dim str As String = ""

        Dim AñoActual As Integer, AñoAnterior As Integer

        AñoActual = Year(jytsistema.sFechadeTrabajo)
        AñoAnterior = Year(jytsistema.sFechadeTrabajo) - 1

        Dim strCan As String = ReporteTipo(CantidadPesoMoneda, "a", "c", "n").strCan
        Dim strUnidad As String = ReporteTipo(CantidadPesoMoneda, "a", "c", "n").strUnidad
        Dim strUND As String = ReporteTipo(CantidadPesoMoneda, "a", "c", "n").strUND

        str += CadenaPLUS_MERCANCIAS("c", strMercancias, strCategorias, strMarcas, strDivisiones, strJerarquias, strNivel1, strNivel2, strNivel3, strNivel4, strNivel5, strNivel6)
        str += CadenaPLUS_PROVEEDORES("d", strProveedor, strCategpriaProveedor, strUnidadNegocio)
        If strProveedor.Trim() <> "" Then str += " a.PROV_CLI " & strProveedor & " AND "

        SeleccionVENPLUS_ComprasComparativasMesMes = " select a.codart, c.nomart, " & strUND & " unidad, " _
            & " c.grupo categoria, c.marca, elt(mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') mix, c.division, c.tipjer, c.codjer, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 1, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) eneAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 1, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) eneActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 2, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) febAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 2, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) febActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 3, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) marAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 3, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) marActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 4, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) abrAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 4, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) abrActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 5, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) mayAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 5, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) mayActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 6, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) junAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 6, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) junActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 7, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) julAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 7, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) julActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 8, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) agoAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 8, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) agoActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 9, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) sepAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 9, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) sepActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 10, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) octAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 10, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) octActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 11, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) novAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 11, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) novActual, " _
            & " ABS(sum(if(year(a.fechamov) = " & AñoAnterior & " and month(a.fechamov) = 12, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) dicAnterior, ABS(sum(if(year(a.fechamov) = " & AñoActual & " and month(a.fechamov) = 12, IF(a.origen = 'NCC', -1,1)*" & strCan & ", 0.00 ))) dicActual " _
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
            & " left join jsprocatpro d on (a.prov_cli = d.codpro and a.id_emp = d.id_emp )" _
            & " Where " _
            & str _
            & " year(a.fechamov) >= " & AñoAnterior & " and year(a.fechamov) <= " & AñoActual & " and " _
            & " SUBSTRING(a.CODART,1,1) <> '$' AND " _
            & " a.origen in ('COM','NDC','REC','NCC') and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by a.codart "

    End Function


    Function SeleccionVENPLUS_VentasAsesorMesMes(strCliente As String, _
                                            strCanal As String, strTipoNegocio As String, _
                                            strZona As String, strRuta As String, strAsesor As String, _
                                            strPais As String, strEstado As String, strMunicipio As String, _
                                            strParroquia As String, strCiudad As String, strBarrio As String, _
                                            strMercancias As String, _
                                            strCategorias As String, strMarcas As String, strDivisiones As String, _
                                            strJerarquias As String, strNivel1 As String, strNivel2 As String, _
                                            strNivel3 As String, strNivel4 As String, strNivel5 As String, strNivel6 As String, _
                                            Año As Integer, Optional CantidadPesoMoneda As Integer = 0) As String

        Dim str As String = ""

        Dim strCan As String = ReporteTipo(CantidadPesoMoneda, "a", "b", "n").strCan
        Dim strUnidad As String = ReporteTipo(CantidadPesoMoneda, "a", "b", "n").strUnidad
        Dim strUND As String = ReporteTipo(CantidadPesoMoneda, "a", "b", "n").strUND

        str += CadenaPLUS_MERCANCIAS("b", strMercancias, strCategorias, strMarcas, strDivisiones, strJerarquias, strNivel1, strNivel2, strNivel3, strNivel4, strNivel5, strNivel6)
        str += CadenaPLUS_CLIENTES("a", "", "", "", "", "", strAsesor, "", "", "", "", "", "")

        SeleccionVENPLUS_VentasAsesorMesMes = " select a.vendedor,  concat(c.apellidos,', ',c.nombres) as nombre_vendedor, b.tipjer, d.descrip,  " _
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
                & " where a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(PrimerDiaAño(CDate("01/01/" & Año.ToString))) & "' AND " _
                & " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(UltimoDiaAño(CDate("01/01/" & Año.ToString))) & "' AND " _
                & " a.origen in ('FAC','PFC','PVE','NDV','NCV') AND " _
                & str _
                & " c.clase = 0 and " _
                & " a.id_emp='" & jytsistema.WorkID & "' " _
                & " group by a.vendedor "

    End Function

    Function SeleccionVENPLUS_DevolucionesCausa(FechaDesde As Date, FechaHasta As Date, _
                                           strCliente As String, _
                                           strCanal As String, strTipoNegocio As String, _
                                           strZona As String, strRuta As String, strAsesor As String, _
                                           strPais As String, strEstado As String, strMunicipio As String, _
                                           strParroquia As String, strCiudad As String, strBarrio As String, _
                                           strMercancias As String, _
                                           strCategorias As String, strMarcas As String, strDivisiones As String, _
                                           strJerarquias As String, strNivel1 As String, strNivel2 As String, _
                                           strNivel3 As String, strNivel4 As String, strNivel5 As String, strNivel6 As String, _
                                           TipoReporte As Integer) As String

        Dim str As String = " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaDesde) & "' and a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' AND "

        str += CadenaPLUS_MERCANCIAS("b", "", strCategorias, strMarcas, strDivisiones, strJerarquias, strNivel1, strNivel2, strNivel3, strNivel4, strNivel5, strNivel6)
        If strAsesor.Trim() <> "" Then str += " a.VENDEDOR " & strAsesor & " AND "
        If strMercancias.Trim() <> "" Then str += " a.CODART " & strMercancias & " AND "

        Dim strCan As String = ReporteTipo(TipoReporte, "a", "d", "m").strCan
        Dim strUnidad As String = ReporteTipo(TipoReporte, "a", "d", "m").strUnidad
        Dim strUND As String = ReporteTipo(TipoReporte, "a", "d", "m").strUND

        Return " SELECT b.causa vendedor , c.descrip NombreAsesor, ROUND(SUM(" & strCan & " ),3) VOLUMEN " _
            & " FROM jsmertramer a " _
            & " LEFT JOIN jsvenrenncr b ON (a.codart = b.item AND a.numdoc = b.numncr AND a.asiento = b.renglon AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvencaudcr c ON (b.causa = c.codigo AND b.id_emp = c.id_emp AND c.credito_debito = 0 ) " _
            & " LEFT JOIN (SELECT a.codart, a.unidad, a.equivale, a.uvalencia, a.id_emp  " _
            & "      		FROM jsmerequmer a  " _
            & "             WHERE " _
            & "     		a.id_emp = '" & jytsistema.WorkID & "' " _
            & "             UNION " _
            & "           SELECT a.codart, a.unidad, 1 equivale, a.unidad uvalencia, a.id_emp  " _
            & "     		FROM jsmerctainv a " _
            & "     		WHERE  " _
            & "     		a.id_emp = '" & jytsistema.WorkID & "') m ON (a.codart = m.codart AND " & strUnidad & " = m.uvalencia AND a.id_emp = m.id_emp) " _
            & " LEFT JOIN jsmerctainv d ON (a.codart = d.codart AND a.ID_EMP  = d.ID_EMP ) " _
            & " WHERE " _
            & str _
            & " a.origen IN ('NCV') AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " GROUP BY 1 " _
            & " ORDER BY 3 DESC"


    End Function

    Function SeleccionVENPLUS_VentasClientesAct(FechaDesde As Date, FechaHasta As Date, _
                                           strCliente As String, _
                                           strCanal As String, strTipoNegocio As String, _
                                           strZona As String, strRuta As String, strAsesor As String, _
                                           strPais As String, strEstado As String, strMunicipio As String, _
                                           strParroquia As String, strCiudad As String, strBarrio As String, _
                                           strMercancias As String, _
                                           strCategorias As String, strMarcas As String, strDivisiones As String, _
                                           strJerarquias As String, strNivel1 As String, strNivel2 As String, _
                                           strNivel3 As String, strNivel4 As String, strNivel5 As String, strNivel6 As String, _
                                           strAlmacen As String,
                                           TipoReporte As Integer, IncluyeCXC As Boolean) As String

        Dim str As String = " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaDesde) & "' and a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' AND "
        Dim strX As String = " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND "

        str += CadenaPLUS_MERCANCIAS("b", "", strCategorias, strMarcas, strDivisiones, strJerarquias, strNivel1, strNivel2, strNivel3, strNivel4, strNivel5, strNivel6)
        If strAsesor.Trim() <> "" Then str += " a.VENDEDOR " & strAsesor & " AND "
        If strAsesor.Trim() <> "" Then strX += " a.codven " & strAsesor & " AND "
        If strMercancias.Trim() <> "" Then str += " a.CODART " & strMercancias & " AND "
        If strCliente.Trim() <> "" Then str += " a.PROV_CLI " & strCliente & " AND "
        If strCliente.Trim() <> "" Then strX += " a.codcli " & strCliente & " AND "
        str += CadenaPLUS_CLIENTES("f", "", strCanal, strTipoNegocio, strZona, strRuta, "", strPais, strEstado, strMunicipio, strParroquia, strCiudad, strBarrio)
        strX += CadenaPLUS_CLIENTES("f", "", strCanal, strTipoNegocio, strZona, strRuta, "", strPais, strEstado, strMunicipio, strParroquia, strCiudad, strBarrio)

        If strAlmacen.Trim() <> "" Then str += " a.ALMACEN " & strAlmacen & " AND "

        Dim strCan As String = ReporteTipo(TipoReporte, "a", "b", "m").strCan
        Dim strUnidad As String = ReporteTipo(TipoReporte, "a", "b", "m").strUnidad
        Dim strUND As String = ReporteTipo(TipoReporte, "a", "b", "m").strUND

        Dim strAD As String = " SELECT '' mix, '.' codart, '' nomart, 'BsF' unidad, '' categoria, '' marca, '' division, '' tipjer, " _
        & " a.codcli prov_cli, f.nombre nombre, " _
        & " SUM(IF( a.tipomov = 'FC' AND a.origen = 'CXC' , 1, 0)* ABS(a.importe) ) ventasBrutas, " _
        & " SUM(IF( a.tipomov = 'NC' AND a.origen = 'CXC' , 1, 0)* ABS(a.importe) ) devolucionBuenEstado, " _
        & " 0.00 devolucionMalEstado, " _
        & " SUM(IF( a.tipomov = 'NC', -1, 1)*ABS(a.importe)) ventasNetas, " _
        & " 'A' peso, COUNT(a.codcli) activacion, a.codcli, a.codven vendedor, " _
        & " g.descrip AS canal, IF(h.descrip IS NULL, '', h.descrip) tiponegocio, IF( i.descrip IS NULL, '', i.descrip) zona, IF( j.nomrut IS NULL, '', j.nomrut) ruta,  " _
        & " CONCAT(k.codven,' ', k.apellidos, ', ', k.nombres) AS asesor, " _
        & " IF( n.nombre IS NULL, '', n.nombre) barrio, IF( o.nombre IS NULL, '', o.nombre) ciudad, " _
        & " IF( p.nombre IS NULL, '', p.nombre) parroquia, IF( q.nombre IS NULL, '', q.nombre) municipio, " _
        & " IF( r.nombre IS NULL, '', r.nombre) estado, IF( s.nombre IS NULL, '', s.nombre) pais " _
        & " FROM jsventracob a " _
        & " LEFT JOIN jsvencatcli f ON (a.codcli = f.codcli AND a.id_emp = f.id_emp) " _
        & " LEFT JOIN jsvenliscan g ON (f.categoria = g.codigo AND f.id_emp = g.id_emp) " _
        & " LEFT JOIN jsvenlistip h ON (f.unidad = h.codigo AND f.categoria = h.antec AND f.id_emp = h.id_emp)  " _
        & " LEFT JOIN jsconctatab i ON (f.zona = i.codigo AND f.id_emp = i.id_emp AND i.modulo = '00005') " _
        & " LEFT JOIN jsvenencrut j ON (f.ruta_visita = j.codrut AND f.id_emp = j.id_emp AND j.tipo = '0') " _
        & " LEFT JOIN jsvencatven k ON (a.codven = k.codven AND a.id_emp = k.id_emp AND k.tipo = '0' AND k.estatus = 1)  " _
        & " LEFT JOIN jsconcatter n ON (f.fbarrio = n.codigo AND a.id_emp = n.id_emp ) " _
        & " LEFT JOIN jsconcatter o ON (f.fciudad = o.codigo AND a.id_emp = o.id_emp ) " _
        & " LEFT JOIN jsconcatter p ON (f.fparroquia = p.codigo AND a.id_emp = p.id_emp ) " _
        & " LEFT JOIN jsconcatter q ON (f.fmunicipio = q.codigo AND a.id_emp = q.id_emp )  " _
        & " LEFT JOIN jsconcatter r ON (f.festado = r.codigo AND a.id_emp = r.id_emp ) " _
        & " LEFT JOIN jsconcatter s ON (f.fpais = s.codigo AND a.id_emp = s.id_emp ) " _
        & " WHERE " _
        & " a.id_emp = '" & jytsistema.WorkID & "' AND a.origen ='CXC' AND a.tipomov IN ('FC','NC') AND " _
        & strX _
        & " a.ejercicio = '" & jytsistema.WorkExercise & "'  GROUP BY a.codcli "

        Return " select elt(b.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') AS mix, " _
            & " a.codart, b.nomart, " & strUND & " unidad, b.NOMBRECATEGORIA categoria, b.NOMBREMARCA marca, " _
            & " b.NOMBREDIVISION division, b.NOMBREJERARQUIA tipjer, " _
            & " a.prov_cli, IF(a.prov_cli = '00000000', 'PUNTOS DE VENTA (POS)', f.nombre) nombre, " _
            & " sum(if( a.origen = 'NCV' , 0, 1)*" & strCan & " ) ventasBrutas, " _
            & " sum(if( a.origen = 'NCV' AND a.almacen <> '00002', 1, 0)*" & strCan & " ) devolucionBuenEstado, " _
            & " sum(if( a.origen = 'NCV' AND a.almacen = '00002',  1, 0)*" & strCan & " ) devolucionMalEstado, " _
            & " sum(if( a.origen = 'NCV', -1, 1)*" & strCan & ") ventasNetas, " _
            & " 'A' peso, " _
            & " count(a.codart)  activacion, a.prov_cli codcli, a.vendedor, " _
            & " f.NOMBRECANAL canal, f.NOMBRETIPONEGOCIO tiponegocio, " _
            & " f.NOMZONA zona, f.NOMRUT ruta, concat(f.CODVEN,' ', f.NOMBRE_VENDEDOR) ASESOR, " _
            & " f.BARRIOFISCAL barrio, f.CIUDADFISCAL ciudad, f.PARROQUIAFISCAL parroquia, " _
            & " f.MUNICIPIOFISCAL municipio, f.ESTADOFISCAL estado, f.PAISFISCAL pais " _
            & " from jsmertramer a " _
            & " left join (" & SeleccionGENMercancias() & ") b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
            & " left join (" & SeleccionGENClientes() & ") f on (a.prov_cli = f.codcli and a.id_emp = f.id_emp) " _
            & " left join (" & SeleccionGENTablaEquivalencias() & ") m on (a.codart = m.codart and " & strUnidad & " = m.uvalencia and a.id_emp = m.id_emp) " _
            & " Where   " _
            & " a.id_emp = '" & jytsistema.WorkID & "' and a.origen in('FAC','PFC','PVE','NDV','NCV') and " _
            & str _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "'  GROUP BY a.prov_cli, a.codart " _
            & IIf(TipoReporte = 2 And IncluyeCXC, " UNION " & strAD, "")




    End Function

    Function SeleccionVENPLUS_ActivacionesClientesYMercancias(FechaDesde As Date, FechaHasta As Date, _
                                           strCliente As String, _
                                           strCanal As String, strTipoNegocio As String, _
                                           strZona As String, strRuta As String, strAsesor As String, _
                                           strPais As String, strEstado As String, strMunicipio As String, _
                                           strParroquia As String, strCiudad As String, strBarrio As String, _
                                           strMercancias As String, _
                                           strCategorias As String, strMarcas As String, strDivisiones As String, _
                                           strJerarquias As String, strNivel1 As String, strNivel2 As String, _
                                           strNivel3 As String, strNivel4 As String, strNivel5 As String, strNivel6 As String, _
                                           strAlmacen As String, TipoReporte As Integer, _
                                           Optional ByVal Activados As Integer = 4, Optional ByVal EstatusCliente As Integer = 4, _
                                           Optional ByVal CondicionMercancia As Integer = 4) As String


        Dim str As String = ""
        str += IIf(Activados <= 1, " a.activo = " & Activados & " and ", " a.activo >= 0 and ")
        str += IIf(EstatusCliente = 0, " a.estatus = 0 and ", _
                   IIf(EstatusCliente = 1, " a.estatus = 1 and ", _
                       IIf(EstatusCliente = 2, " a.estatus = 2 and ", _
                           IIf(EstatusCliente = 3, " a.estatus = 3 and ", IIf(EstatusCliente = 4, " a.estatus <= 4 and ", "  ")))))

        Dim strX As String = CadenaPLUS_MERCANCIAS("c", strMercancias, strCategorias, strMarcas, strDivisiones, strJerarquias, strNivel1, strNivel2, strNivel3, strNivel4, strNivel5, strNivel6)
        strX += " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaDesde) & "' and a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' AND "

        Dim strY As String = " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND "


        str += CadenaPLUS_CLIENTES("c", strCliente, strCanal, strTipoNegocio, strZona, strRuta, "", strPais, strEstado, strMunicipio, strParroquia, strCiudad, strBarrio)

        If strAsesor.Trim() <> "" Then strY += " a.codven " & strAsesor & " AND "
        If strAsesor.Trim() <> "" Then strX += " a.vendedor " & strAsesor & " AND "

        Dim strCan As String = ReporteTipo(TipoReporte, "a", "c", "m").strCan
        Dim strUnidad As String = ReporteTipo(TipoReporte, "a", "c", "m").strUnidad
        Dim strUND As String = ReporteTipo(TipoReporte, "a", "c", "m").strUND


        Dim strC As String = CadenaPLUS_CLIENTES("a", strCliente, "", "", "", "", IIf(Activados = 0, strAsesor, ""), "", "", "", "", "", "")

        Dim strAd As String = " SELECT a.codcli cliente, '.' item , 'BsF' UNIDAD, SUM(IF( a.tipomov = 'NC', -1, 1) *ABS(a.importe) ) CANTIDAD, " _
                              & " 0 pesoactivacion, 1 vecesactivacion " _
                              & " FROM jsventracob a " _
                              & " WHERE " _
                              & strY _
                              & " a.origen = 'CXC' AND a.tipomov IN ('FC','NC') AND  " _
                              & " a.id_emp = '" & jytsistema.WorkID & "' " _
                              & " GROUP BY a.codcli "

        SeleccionVENPLUS_ActivacionesClientesYMercancias = " select a.codcli,  " _
                & " a.nombre, a.ingreso, elt( a.estatus+1, 'Activo', 'Bloqueado', 'Inactivo', 'Desincorporado') estatus, d.descrip canal, e.descrip tiponegocio, " _
                & " f.descrip zona, g.nomrut ruta, concat(c.vendedor,' ', h.apellidos,' ', h.nombres) asesor, m.nombre territorio, " _
                & " a.item, b.nomart, i.descrip categoria, j.descrip marca, Elt(b.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') mix, " _
                & " k.descrip tipjer , codjer1, codjer2, codjer3, codjer4, codjer5, codjer6, r.descrip division, " _
                & " a.unidad, a.cantidad, a.peso, a.num_activados, a.activo, a.id_emp " _
                & " from (select a.codcli, a.nombre, a.ingreso, a.estatus, b.item, b.unidad, b.cantidad, " _
                & "             b.pesoactivacion peso, b.vecesactivacion num_activados, " _
                & "             if(isnull(sum(b.vecesactivacion)),0,1) Activo, a.id_emp " _
                & "             from jsvencatcli a " _
                & "             left join (select a.prov_cli cliente, a.codart item ," & strUND & " UNIDAD, " _
                & "                         sum(   if ( a.origen = 'NCV', -1, 1) *" & strCan & "  ) CANTIDAD, " _
                & "                         0 pesoactivacion, count(a.codart) vecesactivacion " _
                & "                         from jsmertramer a " _
                & "                         left join jsmerctainv c on (a.codart = c.codart and a.id_emp = c.id_emp) " _
                & "                         left join (" & SeleccionGENTablaEquivalencias() & ") m on (a.codart = m.codart and " & strUnidad & " = m.uvalencia and a.id_emp = m.id_emp) " _
                & "                         Where " _
                & strX _
                & "                         a.origen in('FAC','PFC','PVE','NCV','NDV') and " _
                & "                         a.id_emp = '" & jytsistema.WorkID & "' " _
                & "                         group by a.prov_cli, a.codart " & IIf(TipoReporte = 2, " UNION " & strAd, "") & ") b on (a.codcli = b.cliente) " _
                & "      Where " _
                & strC _
                & "      a.id_emp ='" & jytsistema.WorkID & "' " _
                & "      Group By a.codcli, b.item " & ") a " _
                & "      left join jsmerctainv b on (a.item = b.codart and b.id_emp = '" & jytsistema.WorkID & "') " _
                & "      left join jsvencatcli c on (a.codcli = c.codcli and c.id_emp = '" & jytsistema.WorkID & "')" _
                & "      left join jsvenliscan d on (c.categoria = d.codigo and d.id_emp = '" & jytsistema.WorkID & "')" _
                & "      left join jsvenlistip e on (c.unidad = e.codigo and e.id_emp = '" & jytsistema.WorkID & "')" _
                & "      left join jsconctatab f on (c.zona = f.codigo and f.id_emp = '" & jytsistema.WorkID & "' and f.modulo = '00005') " _
                & "      left join jsvenencrut g on (c.ruta_visita = g.codrut and g.id_emp = '" & jytsistema.WorkID & "' and g.tipo = '0') " _
                & "      left join jsvencatven h on (c.vendedor = h.codven and h.id_emp = '" & jytsistema.WorkID & "' and h.tipo = '0' and h.estatus = 1) " _
                & " left join jsconctatab i on (b.grupo = i.codigo and i.id_emp = '" & jytsistema.WorkID & "' and i.modulo = '00002') " _
                & " left join jsconctatab j on (b.marca = j.codigo and j.id_emp = '" & jytsistema.WorkID & "' and j.modulo = '00003') " _
                & " left join jsmerencjer k on (k.tipjer = b.tipjer and k.id_emp = '" & jytsistema.WorkID & "') " _
                & " left join jsconcatter l on (c.fpais = l.codigo and c.id_emp = l.id_emp) " _
                & " left join jsconcatter m on (c.festado = m.codigo and c.id_emp = m.id_emp) " _
                & " left join jsconcatter n on (c.fmunicipio = n.codigo and c.id_emp = n.id_emp) " _
                & " left join jsconcatter o on (c.fparroquia = o.codigo and c.id_emp = o.id_emp) " _
                & " left join jsconcatter p on (c.fciudad = p.codigo and c.id_emp = p.id_emp) " _
                & " left join jsconcatter q on (c.fbarrio = q.codigo and c.id_emp = q.id_emp) " _
                & " left join jsmercatdiv r on (b.division = r.division and b.id_emp = r.id_emp) " _
                & " where " _
                & str _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " order by  " _
                & " codcli, item "


    End Function

    Function SeleccionVENPLUS_VEN_VentasPorDivision(FechaDesde As Date, FechaHasta As Date, _
                                           strCliente As String, _
                                           strCanal As String, strTipoNegocio As String, _
                                           strZona As String, strRuta As String, strAsesor As String, _
                                           strPais As String, strEstado As String, strMunicipio As String, _
                                           strParroquia As String, strCiudad As String, strBarrio As String, _
                                           TipoReporte As Integer, _
                                           EstatusCliente As Integer) As String

        Dim str As String = CadenaPLUS_CLIENTES("c", strCliente, strCanal, strTipoNegocio, strZona, strRuta, "", strPais, strEstado, strMunicipio, strParroquia, strCiudad, strBarrio, , EstatusCliente)
        str += CadenaPLUS_CLIENTES("a", "", "", "", "", "", strAsesor, "", "", "", "", "", "")

        Dim strCan As String = ReporteTipo(TipoReporte, "a", "b", "m").strCan
        Dim strUnidad As String = ReporteTipo(TipoReporte, "a", "b", "m").strUnidad
        Dim strUND As String = ReporteTipo(TipoReporte, "a", "b", "m").strUND



        SeleccionVENPLUS_VEN_VentasPorDivision = " select a.prov_cli, c.nombre, " _
            & " a.vendedor, " _
            & " g.descrip as canal, if(h.descrip is null, '', h.descrip) tiponegocio, " _
            & " if( i.descrip is null, '', i.descrip) zona, if( j.nomrut is null, '', j.nomrut) ruta, " _
            & " concat(k.codven,' ', k.apellidos, ', ', k.nombres) as asesor, " _
            & " if( n.nombre is null, '', n.nombre) barrio, " _
            & " if( o.nombre is null, '', o.nombre) ciudad, " _
            & " if( p.nombre is null, '', p.nombre) parroquia, " _
            & " if( q.nombre is null, '', q.nombre) municipio, " _
            & " if( r.nombre is null, '', r.nombre) estado, " _
            & " if( s.nombre is null, '', s.nombre) pais, " _
            & " sum(if(b.division = '00001', if ( a.origen = 'NCV', -1, 1) *" & strCan & ", 0)) div01, " _
            & " sum(if(b.division = '00002', if ( a.origen = 'NCV', -1, 1) *" & strCan & ", 0)) div02, " _
            & " sum(if(b.division = '00003', if ( a.origen = 'NCV', -1, 1) *" & strCan & ", 0)) div03, " _
            & " sum(if(b.division = '00004', if ( a.origen = 'NCV', -1, 1) *" & strCan & ", 0)) div04, " _
            & " sum(if(b.division = '00005', if ( a.origen = 'NCV', -1, 1) *" & strCan & ", 0)) div05, " _
            & " sum(if(b.division = '00006', if ( a.origen = 'NCV', -1, 1) *" & strCan & ", 0)) div06, " _
            & " sum(if(b.division = '00007', if ( a.origen = 'NCV', -1, 1) *" & strCan & ", 0)) div07, " _
            & " sum(if(b.division = '00001', if ( a.origen = 'NCV', -1, 1) *" & strCan & ", 0)) + sum(if(b.division = '00002', if ( a.origen = 'NCV', -1, 1) *" & strCan & ", 0)) + sum(if(b.division = '00003', if ( a.origen = 'NCV', -1, 1) *" & strCan & ", 0)) + sum(if(b.division = '00004', if ( a.origen = 'NCV', -1, 1) *" & strCan & ", 0)) + sum(if(b.division = '00005', if ( a.origen = 'NCV', -1, 1) *" & strCan & ", 0)) +  sum(if(b.division = '00006', if ( a.origen = 'NCV', -1, 1) *" & strCan & ", 0)) + sum(if(b.division = '00007', if ( a.origen = 'NCV', -1, 1) *" & strCan & ", 0)) divtot " _
            & " from jsmertramer a " _
            & " left join (" & SeleccionGENTablaEquivalencias() & ") m on (a.codart = m.codart and " & strUnidad & " = m.uvalencia and a.id_emp = m.id_emp) " _
            & " left join jsmerctainv b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
            & " left join jsvencatcli c on (a.prov_cli = c.codcli and a.id_emp = c.id_emp) " _
            & " left join jsvenliscan g on (c.categoria = g.codigo and c.id_emp = g.id_emp) " _
            & " left join jsvenlistip h on (c.unidad = h.codigo and c.categoria = h.antec and c.id_emp = h.id_emp) " _
            & " left join jsconctatab i on (c.zona = i.codigo and c.id_emp = i.id_emp and i.modulo = '00005') " _
            & " left join jsvenencrut j on (c.ruta_visita = j.codrut and c.id_emp = j.id_emp and j.tipo = '0')" _
            & " left join jsvencatven k on (a.vendedor = k.codven and a.id_emp = k.id_emp and k.tipo = '0' and k.estatus = 1) " _
            & " left join jsconcatter n on (c.fbarrio = n.codigo and a.id_emp = n.id_emp ) " _
            & " left join jsconcatter o on (c.fciudad = o.codigo and a.id_emp = o.id_emp ) " _
            & " left join jsconcatter p on (c.fparroquia = p.codigo and a.id_emp = p.id_emp ) " _
            & " left join jsconcatter q on (c.fmunicipio = q.codigo and a.id_emp = q.id_emp ) " _
            & " left join jsconcatter r on (c.festado = r.codigo and a.id_emp = r.id_emp ) " _
            & " left join jsconcatter s on (c.fpais = s.codigo and a.id_emp = s.id_emp ) " _
            & " Where " _
            & " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaDesde) & "' and " _
            & " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' and " _
            & " a.origen in ('FAC','PFC','PVE','NDV', 'NCV') and " _
            & str _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by a.prov_cli " _
            & " order by divtot desc "

    End Function







    '////////// MERCANCIAS

    Function SeleccionVENPLUS_MER_InventarioLegalPlus(ByVal FechaInicial As Date, ByVal FechaFinal As Date, _
                                                    strMercancias As String, _
                                                    strCategorias As String, strMarcas As String, strDivisiones As String, _
                                                    strJerarquias As String, strNivel1 As String, strNivel2 As String, _
                                                    strNivel3 As String, strNivel4 As String, strNivel5 As String, strNivel6 As String, _
                                                    strAlmacen As String, TipoReporte As Integer, OrdenadoPor As String, _
                                                    Optional ByVal Estatus As Integer = 9, Optional ByVal Cartera As Integer = 9, _
                                                    Optional ByVal SoloPeso As Boolean = False, Optional ByVal TipoMercancia As Integer = 99, _
                                                    Optional ByVal Regulada As Integer = 9, Optional ByVal TarifaA As Boolean = True, _
                                                    Optional ByVal Existencias As Integer = 2) As String

        Dim str As String = CadenaPLUS_MERCANCIAS("c", strMercancias, strCategorias, strMarcas, strDivisiones, strJerarquias, strNivel1, strNivel2, strNivel3, strNivel4, strNivel5, strNivel6)

        If strAlmacen.Trim() <> "" Then str += " b.ALMACEN " & strAlmacen & " AND "


        Dim strCan As String = ReporteTipo(TipoReporte, "b", "c", "n").strCan
        Dim strUnidad As String = ReporteTipo(TipoReporte, "b", "c", "n").strUnidad
        Dim strUND As String = ReporteTipo(TipoReporte, "b", "c", "n").strUND

        Dim strFechaInicial As String = ft.FormatoFechaHoraMySQLInicial(FechaInicial)
        Dim strFechaFinal As String = ft.FormatoFechaHoraMySQLFinal(FechaFinal)

        SeleccionVENPLUS_MER_InventarioLegalPlus = " SELECT a.codart, a.nomart, a.unidad, a.costoinicial, " _
                & " IF(a.InventarioInicial < 0, 0.00,  IF ( a.inventarioinicial <= 0.01, 0.00, a.inventarioinicial) ) InventarioInicial, IF( a.InventarioInicial < 0, 0.00, IF ( a.inventarioinicial <= 0.01, 0.00, a.inventarioinicial) )*a.costoInicial CostoInicialTotal,  " _
                & " a.Entradas, a.Entradas*a.costoinicial CostoEntradas, " _
                & " a.Salidas, a.Salidas*a.costoinicial CostoSalidas, " _
                & " a.autoconsumo, a.autoconsumo*a.costoinicial CostoAutoconsumo, " _
                & " a.retiros, a.retiros*a.costoinicial CostoRetiros, " _
                & " ( IF(a.InventarioInicial < 0, 0.00, IF ( a.inventarioinicial <= 0.01, 0.00, a.inventarioinicial)) + a.entradas - a.salidas - a.autoconsumo - a.retiros) InventarioFinal, (IF(a.InventarioInicial < 0 , 0.00, IF ( a.inventarioinicial <= 0.01, 0.00, a.inventarioinicial)) + a.entradas - a.salidas - a.autoconsumo - a.retiros)*a.costoinicial CostoFinalTotal " _
                & " FROM ( SELECT b.codart, c.nomart, " & strUND & " unidad, " _
                & "                     SUM(IF( b.tipomov IN ('EN','AE','DV','AC') , b.costotaldes, 0.00)) / SUM( IF( b.tipomov IN ('EN','AE','DV','AC') , " & strCan & ", 0.00)  ) CostoInicial, " _
                & "                 	SUM(IF( b.fechamov < '" & strFechaInicial & "', IF(b.TIPOMOV IN ('EN','AE','DV'), 1, IF( b.tipomov IN ('SA','AS','DC'), -1, 0))*" & strCan & " , 0 )) InventarioInicial, " _
                & "                 	SUM(IF( b.fechamov >= '" & strFechaInicial & "', IF(b.TIPOMOV IN ('EN','AE','DV'), 1, 0)*" & strCan & " , 0 )) Entradas, " _
                & "                 	SUM(IF( b.fechamov >= '" & strFechaInicial & "', IF(b.TIPOMOV IN ('SA','AS','DC') AND b.prov_cli NOT IN ('CONSUMO','DESINCORP.'), 1, 0)*" & strCan & " , 0 )) Salidas,  " _
                & "                 	SUM(IF( b.fechamov >= '" & strFechaInicial & "', IF(b.TIPOMOV IN ('SA','AS','DC') AND b.prov_cli = 'CONSUMO', 1, 0)*" & strCan & " , 0 )) AutoConsumo,  " _
                & "                 	SUM(IF( b.fechamov >= '" & strFechaInicial & "', IF(b.TIPOMOV IN ('SA','AS','DC') AND b.prov_cli = 'DESINCORP.', 1, 0)*" & strCan & " , 0 )) retiros " _
                & "                     FROM jsmertramer b " _
                & "                     LEFT JOIN jsmerctainv c on (b.codart = c.codart and b.id_emp = c.id_emp ) " _
                & "                     LEFT JOIN (SELECT a.codart, a.unidad, a.equivale, a.uvalencia, a.id_emp  " _
                & "                         		FROM jsmerequmer a  " _
                & "                                 WHERE " _
                & "                         		a.id_emp = '" & jytsistema.WorkID & "' " _
                & "                                 UNION " _
                & "                         		SELECT a.codart, a.unidad, 1 equivale, a.unidad uvalencia, a.id_emp " _
                & "                         		FROM jsmerctainv a " _
                & "                         		WHERE  " _
                & "                         		a.id_emp = '" & jytsistema.WorkID & "') n ON (b.codart = n.codart AND " & strUnidad & " = n.uvalencia AND b.id_emp = n.id_emp ) " _
                & "                     WHERE " _
                & str _
                & "                 	SUBSTRING(b.CODART,1,1) <> '$' AND " _
                & "                     b.id_emp = '" & jytsistema.WorkID & "' AND " _
                & "                 	b.ejercicio = '' AND " _
                & "                 	b.fechamov <= '" & strFechaFinal & "' " _
                & "                     GROUP BY b.codart ) a "


    End Function



End Module
