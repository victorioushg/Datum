Module ConsultasBancos
    Private ft As New Transportables
    Public Function SeleccionBANBancos(ByVal CodigoBancoDesde As String, ByVal CodigoBancoHasta As String, _
                                    ByVal FechaHasta As Date, Optional ActivosSolamente As Boolean = False) As String

        Dim str As String = ""
        If CodigoBancoDesde <> "" Then str += " a.codban >= '" & CodigoBancoDesde & "' and "
        If CodigoBancoHasta <> "" Then str += " a.codban <= '" & CodigoBancoHasta & "' and "

        SeleccionBANBancos = " select a.*, if(c.saldo is null, 0.00, c.saldo) saldo from jsbancatban a " _
            & " left join (select a.codban, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp " _
            & "             from jsbantraban a " _
            & "             where " _
            & "             a.fechamov <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
            & str _
            & "             a.id_emp = '" & jytsistema.WorkID & "' " _
            & "             group by a.codban ) c on (a.codban = c.codban and a.id_emp = c.id_emp) " _
            & " where " _
            & IIf(ActivosSolamente, " a.estatus = 1 AND ", "") _
            & str _
            & " a.id_emp = '" & jytsistema.WorkID & "' ORDER BY a.CODBAN "

    End Function
    Public Function SeleccionBANMovimientosBancos(ByVal CodigoBancoDesde As String, ByVal CodigoBancoHasta As String, _
                                                ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal Tipodocumentos As String, _
                                                cadOrigen As String) As String
        Dim str As String = ""
        If CodigoBancoDesde <> "" Then str += " and a.codban >= '" & CodigoBancoDesde & "' "
        If CodigoBancoHasta <> "" Then str += " and a.codban <= '" & CodigoBancoHasta & "' "
        str += " and b.fechamov >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' "
        str += " and b.fechamov <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' "
        str += " and locate(b.tipomov, '" & Tipodocumentos & "') > 0 "
        If cadOrigen <> "" Then str += " AND b.origen " & cadOrigen & " "

        SeleccionBANMovimientosBancos = " select a.codban, a.nomban, a.ctaban, a.saldoact, b.numdoc , b.tipomov, b.fechamov, " _
            & " b.benefic, b.concepto, b.importe, b.origen, b.numorg " _
            & " from jsbancatban a " _
            & " left join jsbantraban b on (a.codban = b.codban and a.id_emp = b.id_emp) " _
            & " Where " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " & str _
            & " order By  a.codban, b.fechamov "

    End Function

    Public Function SeleccionBANMovimientosCaja(ByVal CodigoCajaDesde As String, ByVal CodigoCajaHasta As String, _
                                                ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                                cadOrigen As String, Optional ByVal TipoMovimiento As String = "") As String
        Dim str As String = ""

        If CodigoCajaDesde <> "" Then str += " a.caja >= '" & CodigoCajaDesde & "' and "
        If CodigoCajaHasta <> "" Then str += " a.caja <= '" & CodigoCajaHasta & "' and "
        If cadOrigen <> "" Then str += " origen " & cadOrigen & " AND "

        SeleccionBANMovimientosCaja = " select a.caja, a.nomcaja, b.saldo saldo, b.saldoefectivo, b.saldocheque, b.saldotarjeta, b.saldocestaticket, b.saldootro, " _
            & " c.fecha , c.tipomov, c.nummov, c.concepto, c.origen, c.formpag, c.numpag, c.refpag, c.importe importe, a.id_emp " _
            & " from jsbanenccaj a " _
            & " left join ( select a.caja, IFNULL(SUM(a.IMPORTE),0) saldo, sum(if( a.formpag = 'EF', a.importe, 0.00) ) saldoefectivo, " _
                            & " sum(if( a.formpag = 'CH', a.importe, 0.00) ) saldocheque, sum(if( a.formpag = 'TA', a.importe, 0.00) ) saldotarjeta, " _
                            & " sum(if( a.formpag = 'CT', a.importe, 0.00) ) saldocestaticket, sum(if( a.formpag in('EF','CH','TA','CT'), 0.00, a.importe) ) saldootro, a.id_emp " _
                            & " from jsbantracaj a " _
                            & " where " _
                            & str _
                            & " a.deposito = '' and " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' group by a.caja)  b on (a.caja = b.caja and a.id_emp = b.id_emp) " _
            & " left join jsbantracaj c on (a.caja = c.caja and a.id_emp = c.id_emp) " _
            & " where " _
            & " c.fecha >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and " _
            & " c.fecha <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
            & str & IIf(TipoMovimiento <> "", " locate(c.tipomov, '" & TipoMovimiento & "') > 0 and ", "") _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by a.caja, c.fecha "

    End Function

    Public Function SeleccionBANConciliacion(ByVal CodBancoInicio As String, ByVal CodBancoFinal As String, ByVal MesAConciliar As Date) As String

        Dim strSQL As String = ""

        Dim Primerdia As Date
        Dim UltimoDia As Date

        If CodBancoInicio <> "" Then strSQL = strSQL & " a.codban >= '" & CodBancoInicio & "' and "
        If CodBancoFinal <> "" Then strSQL = strSQL & " a.codban <= '" & CodBancoFinal & "' and "

        Primerdia = PrimerDiaMes(MesAConciliar)
        UltimoDia = UltimoDiaMes(MesAConciliar)

        Dim grupoNoConciliado As String = "MOVIMIENTOS NO CONCILIADOS EN EL PERIODO"
        Dim grupoConciliado As String = "MOVIMIENTOS CONCILIADOS EN EL PERIODO"
        SeleccionBANConciliacion = " select a.codban, a.nomban, a.ctaban, 0 creditos, 0 debitos, c.saldomesanterior, d.saldomesactual, e.saldoconsolidado, " _
            & " b.numdoc, b.tipomov, b.fechamov, b.concepto, concat(b.prov_cli, ' ', f.nombre) prov_cli, b.Importe, b.mesconcilia, b.fecconcilia, " _
            & " if(b.conciliado = '0', '0', if(b.mesconcilia > '" & ft.FormatoFechaMySQL(UltimoDia) & "', '0', '1' ) ) conciliado, " _
            & " if(b.conciliado = '0', '" & grupoNoConciliado & "' , if( b.mesconcilia > '" & ft.FormatoFechaMySQL(UltimoDia) & "', '" & grupoNoConciliado & "','" & grupoConciliado & "' ) ) nombreconciliado, a.id_emp " _
            & " from jsbancatban a " _
            & " left join ( select a.* from jsbantraban a " _
            & "             Where " _
            & "             a.fechamov <= '" & ft.FormatoFechaMySQL(UltimoDia) & "' and " _
            & "             (a.mesconcilia > '" & ft.FormatoFechaMySQL(UltimoDia) & "'  or a.mesconcilia = '1963-05-07' ) and " _
            & "             a.id_emp = '" & jytsistema.WorkID & "'" _
            & "             UNION " _
            & "             SELECT a.* FROM jsbantraban a " _
            & "             WHERE " _
            & "             a.fechamov <= '" & ft.FormatoFechaMySQL(UltimoDia) & "' AND " _
            & "             (a.mesconcilia >= '" & ft.FormatoFechaMySQL(Primerdia) & "'  AND a.mesconcilia <= '" & ft.FormatoFechaMySQL(UltimoDia) & "' ) AND " _
            & "             a.id_emp = '" & jytsistema.WorkID & "' ) b on (a.codban = b.codban and a.id_emp = b.id_emp) " _
            & " left join ( select a.codban, IFNULL(SUM(a.IMPORTE),0) saldomesanterior, a.id_emp from jsbantraban a where a.fechamov < '" & ft.FormatoFechaMySQL(Primerdia) & "' and a.id_emp = '" & jytsistema.WorkID & "' group by a.codban ) c " _
            & "     on (a.codban = c.codban and a.id_emp = c.id_emp) " _
            & " left join ( select a.codban, IFNULL(SUM(a.IMPORTE),0) saldomesactual, a.id_emp from jsbantraban a where a.fechamov <= '" & ft.FormatoFechaMySQL(UltimoDia) & "' and a.id_emp = '" & jytsistema.WorkID & "' group by a.codban ) d " _
            & "     on (a.codban = d.codban and a.id_emp = d.id_emp) " _
            & " left join ( select a.codban, IFNULL(SUM(a.IMPORTE),0) saldoconsolidado, a.id_emp from jsbantraban a where a.conciliado = 1 and a.mesconcilia <= '" & ft.FormatoFechaMySQL(UltimoDia) & "' and a.id_emp = '" & jytsistema.WorkID & "' group by a.codban ) e " _
            & "     on (a.codban = e.codban and a.id_emp = e.id_emp) " _
            & " left join jsprocatpro f on (b.prov_cli = f.codpro and b.id_emp = f.id_emp and b.ORIGEN = 'CXP')" _
            & " where " _
            & strSQL _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.codban, b.conciliado, b.fechamov "
    End Function
    Public Function SeleccionBANSaldosXMes(ByVal BancoInicio As String, ByVal BancoFin As String) As String

        Dim strSQL As String = ""

        If BancoInicio <> "" Then strSQL += " and a.codban >= '" & BancoInicio & "' "
        If BancoFin <> "" Then strSQL += " and a.codban <= '" & BancoFin & "' "

        SeleccionBANSaldosXMes = " select a.codban, a.nomban, a.ctaban, a.saldoact, a.fechacrea, " _
                    & " ifnull(b.deb00,0) deb00, ifnull(b.cre00,0) cre00, ifnull(b.deb01,0) deb01, ifnull(b.cre01,0) cre01, ifnull(b.deb02,0) deb02, ifnull(b.cre02,0) cre02, ifnull(b.deb03,0) deb03, ifnull(b.cre03,0) cre03, " _
                    & " ifnull(b.deb04,0) deb04, ifnull(b.cre04,0) cre04, ifnull(b.deb05,0) deb05, ifnull(b.cre05,0) cre05, ifnull(b.deb06,0) deb06, ifnull(b.cre06,0) cre06, ifnull(b.deb07,0) deb07, ifnull(b.cre07,0) cre07, " _
                    & " ifnull(b.deb08,0) deb08, ifnull(b.cre08,0) cre08, ifnull(b.deb09,0) deb09, ifnull(b.cre09,0) cre09, ifnull(b.deb10,0) deb10, ifnull(b.cre10,0) cre10, ifnull(b.deb11,0) deb11, ifnull(b.cre11,0) cre11, " _
                    & " ifnull(b.deb12,0) deb12, ifnull(b.cre12,0) cre12, a.id_emp " _
                    & " from jsbancatban a " _
                    & " left join (" & SeleccionBANSaldosBancosPorMES() & ") b on (a.codban = b.codban And a.id_emp = b.id_emp and b.ejercicio = '" & jytsistema.WorkExercise & "') " _
                    & " Where " _
                    & " a.id_emp = '" & jytsistema.WorkID & "'  " _
                    & strSQL _
                    & " ORDER BY a.codban "

    End Function
    Public Function SeleccionBANSaldosBancosPorMES() As String

        Dim FechaTrabajo As String = ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo)
        Return " SELECT codban, " _
            & " ifnull( SUM( IF (importe > 0 AND YEAR(FECHAMOV) < " & jytsistema.sFechadeTrabajo.Year & ", importe, 0 ) ), 0 ) CRE00, " _
            & " ifnull( SUM( IF (importe < 0 AND YEAR(FECHAMOV) < " & jytsistema.sFechadeTrabajo.Year & ", importe, 0 ) ), 0 ) DEB00, " _
            & " ifnull( SUM( IF (importe > 0 AND MONTH(fechamov) = 1, importe, 0 ) ), 0 ) CRE01,  " _
            & " ifnull( SUM( IF (importe < 0 AND MONTH(fechamov) = 1, importe, 0 ) ), 0 ) DEB01, " _
            & " ifnull( SUM( IF (importe > 0 AND MONTH(fechamov) = 2, importe, 0 ) ), 0 ) CRE02, " _
            & " ifnull( SUM( IF (importe < 0 AND MONTH(fechamov) = 2, importe, 0 ) ), 0 ) DEB02, " _
            & " ifnull( SUM( IF (importe > 0 AND MONTH(fechamov) = 3, importe, 0 ) ), 0 ) CRE03, " _
            & " ifnull( SUM( IF (importe < 0 AND MONTH(fechamov) = 3, importe, 0 ) ), 0 ) DEB03, " _
            & " ifnull( SUM( IF (importe > 0 AND MONTH(fechamov) = 4, importe, 0 ) ), 0 ) CRE04, " _
            & " ifnull( SUM( IF (importe < 0 AND MONTH(fechamov) = 4, importe, 0 ) ), 0 ) DEB04, " _
            & " ifnull( SUM( IF (importe > 0 AND MONTH(fechamov) = 5, importe, 0 ) ), 0 ) CRE05, " _
            & " ifnull( SUM( IF (importe < 0 AND MONTH(fechamov) = 5, importe, 0 ) ), 0 ) DEB05, " _
            & " ifnull( SUM( IF (importe > 0 AND MONTH(fechamov) = 6, importe, 0 ) ), 0 ) CRE06, " _
            & " ifnull( SUM( IF (importe < 0 AND MONTH(fechamov) = 6, importe, 0 ) ), 0 ) DEB06, " _
            & " ifnull( SUM( IF (importe > 0 AND MONTH(fechamov) = 7, importe, 0 ) ), 0 ) CRE07, " _
            & " ifnull( SUM( IF (importe < 0 AND MONTH(fechamov) = 7, importe, 0 ) ), 0 ) DEB07, " _
            & " ifnull( SUM( IF (importe > 0 AND MONTH(fechamov) = 8, importe, 0 ) ), 0 ) CRE08, " _
            & " ifnull( SUM( IF (importe < 0 AND MONTH(fechamov) = 8, importe, 0 ) ), 0 ) DEB08, " _
            & " ifnull( SUM( IF (importe > 0 AND MONTH(fechamov) = 9, importe, 0 ) ), 0 ) CRE09, " _
            & " ifnull( SUM( IF (importe < 0 AND MONTH(fechamov) = 9, importe, 0 ) ), 0 ) DEB09, " _
            & " ifnull( SUM( IF (importe > 0 AND MONTH(fechamov) = 10, importe, 0 ) ), 0 ) CRE10, " _
            & " ifnull( SUM( IF (importe < 0 AND MONTH(fechamov) = 10, importe, 0 ) ), 0 ) DEB10, " _
            & " ifnull( SUM( IF (importe > 0 AND MONTH(fechamov) = 11, importe, 0 ) ), 0 ) CRE11, " _
            & " ifnull( SUM( IF (importe < 0 AND MONTH(fechamov) = 11, importe, 0 ) ), 0 ) DEB11, " _
            & " ifnull( SUM( IF (importe > 0 AND MONTH(fechamov) = 12, importe, 0 ) ), 0 ) CRE12, " _
            & " ifnull( SUM( IF (importe < 0 AND MONTH(fechamov) = 12, importe, 0 ) ), 0 ) DEB12, " _
            & " a.ejercicio, a.id_emp " _
            & " FROM jsbantraban a " _
            & " WHERE " _
            & " Year(a.fechamov) = " & jytsistema.sFechadeTrabajo.Year & " and " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' "

    End Function
    Function SeleccionBANEstadoCuenta(ByVal FechaInicio As Date, ByVal FechaFin As Date, _
    ByVal BancoInicio As String, ByVal BancoFin As String) As String

        Dim strSQL As String = ""

        If BancoInicio <> "" Then strSQL += " a.codban >= '" & BancoInicio & "' and "
        If BancoFin <> "" Then strSQL += " a.codban <= '" & BancoFin & "' and "

        SeleccionBANEstadoCuenta = " SELECT a.codban, a.nomban, a.ctaban, IF( ISNULL(b.saldoanterior),0.00 ,b.saldoanterior ) saldoanterior, " _
                & " c.numdoc, c.tipomov, c.fechamov, c.concepto, IF(c.importe >=0 , ABS(c.importe), 0) creditos, " _
                & " IF( c.importe < 0, ABS(c.importe), 0.00 ) debitos, (IF(c.importe >=0 , ABS(c.importe), 0) - IF( c.importe < 0, ABS(c.importe), 0.00 ))  saldo, " _
                & " c.prov_cli, c.benefic, a.id_emp  " _
                & " FROM jsbancatban a " _
                & " LEFT JOIN (SELECT a.codban, IFNULL(SUM(a.IMPORTE),0) saldoanterior FROM jsbantraban a WHERE " _
                & "         a.fechamov < '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
                & "         a.id_emp = '" & jytsistema.WorkID & "' " _
                & "         GROUP BY a.codban ) b ON (a.codban = b.codban) " _
                & " LEFT JOIN jsbantraban c ON (a.codban = c.codban AND a.id_emp = c.id_emp ) " _
                & " Where " _
                & " c.fechamov >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
                & " c.fechamov <= '" & ft.FormatoFechaMySQL(FechaFin) & "' AND " _
                & strSQL _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " ORDER BY " _
                & " a.codban, c.fechamov, c.numdoc, c.tipomov "

    End Function
    Function SeleccionBANReposicionSaldoCaja(ByVal DocumentoDesde As String, ByVal DocumentoHasta As String, _
                                             CajaDesde As String, CajaHasta As String, _
                                                Optional ByVal FechaDesde As Date = #1/1/2010#, Optional ByVal FechaHasta As Date = #1/1/2010#) As String

        Dim str As String = ""
        If DocumentoDesde <> "" Then str += " b.comproba >= '" & DocumentoDesde & "' AND "
        If DocumentoHasta <> "" Then str += " b.comproba <= '" & DocumentoHasta & "' and "
        If CajaDesde <> "" Then str += " c.caja >= '" & CajaDesde & "' AND "
        If CajaHasta <> "" Then str += " c.caja <= '" & CajaHasta & "' and "
        If FechaDesde <> #1/1/2010# Then str += " b.fechamov >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and "
        If FechaHasta <> #1/1/2010# Then str += " b.fechamov <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and "

        SeleccionBANReposicionSaldoCaja = " SELECT a.codban, a.nomban, a.ctaban, b.comproba numdoc, b.fechamov, " _
            & " c.fecha, c.tipomov, c.nummov, c.codven, c.concepto, c.origen, c.formpag, c.numpag, c.refpag, c.importe, " _
            & " c.prov_cli, d.nombre " _
            & " FROM jsbantraban b " _
            & " LEFT JOIN jsbancatban a ON (b.codban=a.codban AND b.id_emp = a.id_emp) " _
            & " LEFT JOIN jsbantracaj c ON (b.comproba=c.deposito AND b.id_emp = c.id_emp) " _
            & " left join jsprocatpro d on (c.prov_cli = d.codpro and c.id_emp = d.id_emp) " _
            & " WHERE " _
            & " b.comproba <> '' and " _
            & str _
            & " b.origen = 'BAN' AND b.tipomov IN ('CH','ND') AND  " _
            & " a.id_emp = '" & jytsistema.WorkID & "' ORDER BY c.fecha "

    End Function
    Function SeleccionBANDepositos(ByVal BancoInicio As String, ByVal BancoFin As String, _
        ByVal FechaInicio As Date, ByVal FechaFin As Date, ByVal DocumentoInicio As String, ByVal DocumentoFin As String, _
        DepositoInicio As String, DepositoFin As String, Optional ByVal ParecidoA As Integer = 0) As String

        Dim strSQL As String = ""

        If BancoInicio <> "" Then strSQL += " b.codban >= '" & BancoInicio & "' and "
        If BancoFin <> "" Then strSQL += " b.codban <= '" & BancoFin & "' and "
        If DocumentoInicio <> "" Then strSQL += " c.nummov >= '" & DocumentoInicio & "' and  "
        If DocumentoFin <> "" Then strSQL += " c.nummov <= '" & DocumentoFin & "' and  "
        If DepositoInicio <> "" Then strSQL += " b.numdoc >= '" & DepositoInicio & "' and  "
        If DepositoFin <> "" Then strSQL += " b.numdoc <= '" & DepositoFin & "' and  "

        SeleccionBANDepositos = " select a.*, c.fecha , c.tipomov, c.nummov, c.codven, c.concepto, c.origen, c.formpag, c.numpag, " _
            & " c.refpag, c.importe " _
            & " from (select a.codban, a.nomban, a.ctaban, b.numdoc, b.fechamov, a.id_emp " _
            & "         from jsbantraban b " _
            & "         left join jsbancatban a on (b.codban=a.codban and b.id_emp = a.id_emp) " _
            & "         left join jsbantracaj c on (b.numdoc=c.deposito AND b.codban = c.codban AND b.id_emp = c.id_emp) " _
            & "         Where " _
            & strSQL _
            & "         c.formpag = 'EF' and " _
            & "         b.origen = 'CAJ' and b.tipomov = 'DP' and " _
            & "         b.fechamov >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' and " _
            & "         b.fechamov <= '" & ft.FormatoFechaMySQL(FechaFin) & "' and " _
            & "         a.id_emp = '" & jytsistema.WorkID & "' " _
            & " " _
            & "         UNION select a.codban, a.nomban, a.ctaban, b.numdoc, b.fechamov, a.id_emp " _
            & "         from jsbantraban b " _
            & "         left join jsbancatban a on (b.codban=a.codban and b.id_emp = a.id_emp) " _
            & "         left join jsbantracaj c on (b.numdoc=c.deposito AND b.codban = c.codban and b.id_emp = c.id_emp) " _
            & "         left join jsconctatab d on (c.refpag = d.codigo and '" & FormatoTablaSimple(Modulo.iBancos) & "' = d.modulo and c.id_emp = d.id_emp ) " _
            & "         Where " _
            & strSQL _
            & "         c.formpag = 'CH' and " _
            & "         b.origen = 'CAJ' and b.tipomov = 'DP' and " _
            & "         b.fechamov >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' and " _
            & "         b.fechamov <= '" & ft.FormatoFechaMySQL(FechaFin) & "' and " _
            & "         a.id_emp = '" & jytsistema.WorkID & "' " _
            & " " _
            & "         UNION select a.codban, a.nomban, a.ctaban, b.numdoc, b.fechamov, a.id_emp " _
            & "         from jsbantraban b " _
            & "         left join jsbancatban a on (b.codban=a.codban and b.id_emp = a.id_emp) " _
            & "         left join jsbantracaj c on (b.numdoc=c.deposito AND b.codban = c.codban and b.id_emp = c.id_emp) " _
            & "         left join jsvencestic d on (c.refpag = d.codigo and c.id_emp = d.id_emp ) " _
            & "         Where " _
            & strSQL _
            & "         c.formpag = 'CT' and " _
            & "         b.origen = 'CAJ' and b.tipomov = 'DP' and " _
            & "         b.fechamov >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' and " _
            & "         b.fechamov <= '" & ft.FormatoFechaMySQL(FechaFin) & "' and " _
            & "         a.id_emp = '" & jytsistema.WorkID & "' " _
            & " " _
            & "         UNION select a.codban, a.nomban, a.ctaban, b.numdoc, b.fechamov, a.id_emp " _
            & "         from jsbantraban b " _
            & "         left join jsbancatban a on (b.codban=a.codban and b.id_emp = a.id_emp) " _
            & "         left join jsbantracaj c on (b.numdoc=c.deposito AND b.codban = c.codban and b.id_emp = c.id_emp) " _
            & "         left join jsconctatar d on (c.refpag = d.codtar and c.id_emp = d.id_emp ) " _
            & "         Where " _
            & strSQL _
            & "         c.formpag = 'TA' and " _
            & "         b.origen = 'CAJ' and b.tipomov = 'DP' and " _
            & "         b.fechamov >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' and " _
            & "         b.fechamov <= '" & ft.FormatoFechaMySQL(FechaFin) & "' and " _
            & "         a.id_emp = '" & jytsistema.WorkID & "' ) a " _
            & " LEFT JOIN jsbantracaj c on (a.numdoc = c.deposito AND a.codban = c.codban  AND a.ID_EMP = c.ID_EMP ) " _
            & " order by 1,5,6 "

    End Function
    Function SeleccionBANDebitoBancario(ByVal BancoInicio As String, ByVal BancoFin As String, ByVal FechaInicio As Date, ByVal FechaFin As Date)
        Dim strSQL As String = ""
        If BancoInicio <> "" Then strSQL += " and a.codban >= '" & BancoInicio & "' "
        If BancoFin <> "" Then strSQL += " and a.codban <= '" & BancoFin & "' "

        SeleccionBANDebitoBancario = "select a.codban, a.nomban, a.ctaban, " _
            & " a.saldoact, b.numdoc, b.tipomov, b.fechamov, b.benefic, " _
            & " b.concepto, b.importe, b.origen, b.numorg from jsbancatban a " _
            & " left join jsbantraban b on (a.codban=b.codban and a.id_emp=b.id_emp) " _
            & " Where " _
            & " b.tipomov = 'ND' " _
            & " and MID(b.concepto,1,21) = 'DEBITO BANCARIO MES :' " _
            & " and b.fechamov >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' " _
            & " and b.fechamov <= '" & ft.FormatoFechaMySQL(FechaFin) & "' " _
            & strSQL _
            & " and a.id_emp = '" & jytsistema.WorkID & "' " _
            & " Order By " _
            & " b.fechamov, b.numdoc"

    End Function
    Function SeleccionBANDebitoBancarioMes(ByVal BancoInicio As String, ByVal BancoFin As String, ByVal Año As Integer) As String
        Dim strSQL As String = ""

        If BancoInicio <> "" Then strSQL += " and a.codban >= '" & BancoInicio & "' "
        If BancoFin <> "" Then strSQL += " and a.codban <= '" & BancoFin & "' "

        SeleccionBANDebitoBancarioMes = "select a.codban, b.nomban, b.ctaban, b.saldoact, " _
            & " sum(if( month(a.fechamov) = 1, a.importe, 0.00 )) ene, " _
            & " sum(if( month(a.fechamov) = 2, a.importe, 0.00 )) feb, " _
            & " sum(if( month(a.fechamov) = 3, a.importe, 0.00 )) mar, " _
            & " sum(if( month(a.fechamov) = 4, a.importe, 0.00 )) abr, " _
            & " sum(if( month(a.fechamov) = 5, a.importe, 0.00 )) may, " _
            & " sum(if( month(a.fechamov) = 6, a.importe, 0.00 )) jun, " _
            & " sum(if( month(a.fechamov) = 7, a.importe, 0.00 )) jul, " _
            & " sum(if( month(a.fechamov) = 8, a.importe, 0.00 )) ago, " _
            & " sum(if( month(a.fechamov) = 9, a.importe, 0.00 )) sep, " _
            & " sum(if( month(a.fechamov) = 10, a.importe, 0.00 )) oct, " _
            & " sum(if( month(a.fechamov) = 11, a.importe, 0.00 )) nov, " _
            & " sum(if( month(a.fechamov) = 12, a.importe, 0.00 )) dic " _
            & " from jsbantraban a " _
            & " left join jsbancatban b on (a.codban = b.codban and a.id_emp = b.id_emp) " _
            & " Where " _
            & " a.tipomov = 'ND' and " _
            & " MID(a.concepto,1,21) = 'DEBITO BANCARIO MES :' and " _
            & " year(a.fechamov) = " & Año & " and " _
            & " a.id_emp ='" & jytsistema.WorkID & "' " _
            & strSQL _
            & " group by a.codban "

    End Function
    Function SeleccionBANChequeDevuelto(ByVal CodigoCliente As String, ByVal Fecha As Date, ByVal NumeroCheque As String) As String

        SeleccionBANChequeDevuelto = " select  a.codcli, b.nombre, b.dirfiscal, b.rif, b.nit, b.telef1, b.telef2, b.telef3, b.ingreso, b.ruta_visita, d.nomrut, " _
            & " b.vendedor, concat(c.codven,' ', c.nombres, ' ', c.apellidos) asesor, e.numcheque, e.fechadev, x.fecha, x.refpag codban, f.descrip nom_banco, " _
            & " elt(e.causa+1, '1. FECHA DEFECTUOSA ', '2. FECHA ADELANTADA', '3. FALTA FIRMA', '4. DEFECTO FIRMA Y/O SELLO', " _
            & " '6. DEFECTO DE ENDOSO', '8. CUENTA CERRADA','9. GIRA SOBRE FONDOS NO DISPONIBLES', '10. PAGO SUSPENDIDO', '11. NO ES A NUESTRO CARGO', " _
            & " '13. GIRADOR FALLECIDO', '14. PRESENTAR POR TAQUILLA', '15. DIRIGIRSE AL GIRADOR','16. CANTIDAD DEFECTUOSA', '17. FALTA SELLO COMPENSACION', " _
            & " '18. PASO 2 VECES COMPENSACION','19. CAUSA EXTERNA', '20. OTRA') causa, g.chequesmes, h.chequesano, i.chequestotal, " _
            & " a.nummov , a.emision, a.hora, a.tipomov, a.concepto, a.importe " _
            & " FROM jsventracob a " _
            & " inner join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
            & " left join jsvencatven c on (a.codven = c.codven and a.id_emp = c.id_emp) " _
            & " left join jsvenencrut d on (b.ruta_visita = d.codrut and b.id_emp = d.id_emp and d.tipo = '0') " _
            & " left join jsbanchedev e on (a.codcli = e.prov_cli and a.numorg = e.numcheque and a.id_emp = e.id_emp) " _
            & " left join jsbantracaj x on (e.numcheque = x.numpag and e.id_emp = x.id_emp) " _
            & " left join jsconctatab f on (x.refpag = f.codigo and x.id_emp = f.id_emp )" _
            & " left join ( select prov_cli, IFNULL(COUNT(*),0) chequesmes  " _
                            & " from jsbanchedev " _
                            & " Where " _
                            & " prov_cli = '" & CodigoCliente & "' and " _
                            & " month(fechadev) = " & Month(Fecha) & " and " _
                            & " year(fechadev) = " & Year(Fecha) & " and " _
                            & " id_emp ='" & jytsistema.WorkID & "' " _
                            & " group by prov_cli ) g on (a.codcli = g.prov_cli) " _
            & " left join ( select prov_cli, IFNULL(COUNT(*),0) chequesano " _
                            & " From jsbanchedev " _
                            & " Where " _
                            & " prov_cli = '" & CodigoCliente & "' and " _
                            & " year(fechadev) = " & Year(Fecha) & " and " _
                            & " id_emp ='" & jytsistema.WorkID & "' " _
                            & " GROUP by prov_cli ) h on (a.codcli = h.prov_cli) " _
            & " left join ( select prov_cli, IFNULL(COUNT(*),0) chequestotal " _
                            & " from jsbanchedev " _
                            & " where " _
                            & " prov_cli = '" & CodigoCliente & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' " _
                            & " group by prov_cli ) i on (a.codcli = i.prov_cli) " _
            & " Where " _
            & " f.modulo = '" & FormatoTablaSimple(Modulo.iBancos) & "'  and " _
            & " a.codcli = '" & CodigoCliente & "' and " _
            & " a.origen = 'BAN' and " _
            & " a.numorg = '" & NumeroCheque & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by a.nummov " _
            & " order by a.nummov "


    End Function
    Function SeleccionBANChequesDevueltosBancos(ByVal BancoInicio As String, ByVal BancoFin As String, ByVal FechaInicio As Date, ByVal FechaFin As Date) As String

        Dim strSQL As String = ""

        If BancoInicio <> "" Then strSQL += " a.codban >= '" & BancoInicio & "' and "
        If BancoFin <> "" Then strSQL += " a.codban <= '" & BancoFin & "' and "

        SeleccionBANChequesDevueltosBancos = " select a.codban, a.nomban, a.ctaban, " _
            & " b.numcheque, b.deposito, b.prov_cli, c.nombre, b.numcan, b.causa, b.monto, " _
            & " b.fechadev, if( f.importe is null, 0.00, f.importe) saldo " _
            & " from jsbancatban a " _
            & " left join jsbanchedev b on (a.codban = b.codban and a.id_emp = b.id_emp) " _
            & " left join jsvencatcli c on (b.prov_cli = c.codcli and b.id_emp = c.id_emp) " _
            & " left join (SELECT  a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) importe, a.numorg " _
            & "             FROM jsventracob a " _
            & "             Where " _
            & "             SUBSTRING(a.nummov,1,2) = 'CD' AND " _
            & "             a.id_emp = '" & jytsistema.WorkID & "' " _
            & "             GROUP BY a.codcli, a.nummov " _
            & "             HAVING ROUND(importe,0) > 0) f ON (b.numcheque = f.numorg) " _
            & " Where " _
            & " b.fechadev >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' and " _
            & " b.fechadev <= '" & ft.FormatoFechaMySQL(FechaFin) & "' and " _
            & strSQL _
            & " a.id_emp = '" & jytsistema.WorkID & "' "

    End Function
    Function SeleccionBANTicketDevuelto(ByVal NumeroTicket As String) As String

        SeleccionBANTicketDevuelto = "select a.ticket, a.corredor, a.monto, " _
            & " a.numcan, b.codcli, c.nombre, c.dirfiscal, c.rif, c.nit, c.telef1, c.telef2, " _
            & " c.ruta_visita, b.codven, concat(d.nombres, ' ', d.apellidos) asesor, " _
            & " b.nummov , b.emision, b.tipomov, b.concepto, b.importe " _
            & " from jsventabtic a " _
            & " left join jsventracob b on (a.ticket = b.numorg and a.id_emp = b.id_emp) " _
            & " left join jsvencatcli c on (b.codcli = c.codcli and b.id_emp = c.id_emp) " _
            & " left join jsvencatven d on (b.codven = d.codven and b.id_emp = d.id_emp) " _
            & " Where " _
            & " a.ticket = '" & NumeroTicket & "' and " _
            & " a.id_emp ='" & jytsistema.WorkID & "'"

    End Function

    Function SeleccionBANRemesaCestaTicket(ByVal RemesaDesde As String, ByVal RemesaHasta As String, _
                                        ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal RemesasDepositadas As Boolean) As String

        Dim str As String = ""
        Dim strSQL As String = ""

        If RemesasDepositadas Then
            str = " and a.numdep <> '' "
        Else
            str = " and a.numdep = '' "
        End If

        If RemesaDesde <> "" Then strSQL += " and a.numsobre >= '" & RemesaDesde & "' "
        If RemesaHasta <> "" Then strSQL += " and a.numsobre <= '" & RemesaHasta & "' "

        SeleccionBANRemesaCestaTicket = "select count(a.ticket) tickets, a.ticket,  " _
            & " a.monto, sum(a.monto) totalmonto, a.corredor, b.descrip, " _
            & " a.numsobre , a.fechasobre, a.numdep " _
            & " from jsventabtic a " _
            & " left join jsvencestic b on (a.corredor = b.codigo and a.id_emp = b.id_emp) " _
            & " Where " _
            & IIf(RemesaDesde <> "" AndAlso RemesaHasta <> "", "", " a.fechasobre >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and ") _
            & IIf(RemesaDesde <> "" AndAlso RemesaHasta <> "", "", " a.fechasobre <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and ") _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & str & " and a.numsobre <> '' " _
            & strSQL _
            & " group by a.corredor, a.numsobre, a.monto, a.ticket " _
            & " order by a.corredor, a.numsobre, a.monto, a.ticket "

    End Function

    Function SeleccionBANTransferencia(ByVal NumTransferencia As String) As String

        SeleccionBANTransferencia = " select a.codban, b.nomban, b.ctaban, a.fechamov, a.numdoc, " _
            & " a.tipomov, if(a.tipomov = 'ND', 'Banco_emisor_de_transferencia :','Banco_receptor_de_transferencia :') tipodesban, a.origen, a.numorg, a.importe " _
            & " from jsbantraban a " _
            & " inner join jsbancatban b on (a.codban = b.codban and a.id_emp = b.id_emp) " _
            & " Where " _
            & " a.numdoc = '" & NumTransferencia & "' and " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
            & " order by a.tipomov desc "

    End Function
    Function SeleccionBANCajaNoDepositado(ByVal CajaDesde As String, ByVal CajaHasta As String, _
    ByVal FechaDesde As Date, ByVal TipoDocumentosPago As String) As String

        Dim str As String = ""

        str += " locate(c.formpag, '" & TipoDocumentosPago & "') > 0  and "
        If CajaDesde <> "" Then str += " a.caja >= '" & CajaDesde & "' and "
        If CajaHasta <> "" Then str += " a.caja <= '" & CajaHasta & "' and "

        SeleccionBANCajaNoDepositado = "select a.caja, a.nomcaja, " _
            & " c.fecha , c.tipomov, c.nummov, c.concepto, c.origen, c.formpag, c.numpag, c.refpag, c.importe, c.prov_cli, c.codven " _
            & " from jsbanenccaj a " _
            & " left join jsbantracaj c on (a.caja = c.caja and a.id_emp = c.id_emp) " _
            & " Where " _
            & " c.deposito = '' and " _
            & " c.fecha >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and " _
            & str _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order By " _
            & " a.caja, c.fecha "

    End Function

    Function SeleccionBANComprobanteCheque(ByVal CodigoBanco As String, ByVal Comprobante As String) As String

        SeleccionBANComprobanteCheque = "select a.prov_cli codpro, a.tipomov, a.numdoc nummov,  " _
            & " a.fechamov emision, '' refer, a.concepto, a.importe, " _
            & " a.numdoc numpag, a.codban nompag, a.benefic, a.benefic nombre, a.comproba, '' zona, b.nomban, b.ctaban, b.formato, a.fechamov emisiondoc " _
            & " from jsbantraban a " _
            & " left join jsbancatban b on (a.codban = b.codban and a.id_emp = b.id_emp) " _
            & " Where " _
            & " a.codban = '" & CodigoBanco & "' and " _
            & " a.comproba = '" & Comprobante & "' and " _
            & " a.tipomov = 'CH' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "'"

    End Function
    Function SeleccionBANComprobanteChequeContable(ByVal CodigoBanco As String, ByVal Comprobante As String) As String

        SeleccionBANComprobanteChequeContable = "select a.prov_cli codpro, a.tipomov, a.numdoc nummov,  " _
            & " a.fechamov emision, '' refer, a.concepto, a.importe, " _
            & " a.numdoc numpag, a.codban nompag, a.benefic, a.benefic nombre, a.comproba, '' zona, b.nomban, b.ctaban, b.formato,  " _
            & " c.renglon, c.codcon, c.refer refercont, c.concepto conceptocont, c.importe importecont, a.fechamov emisiondoc  " _
            & " from jsbantraban a " _
            & " left join jsbancatban b on (a.codban = b.codban and a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsbanordpag c ON (a.comproba = c.comproba AND a.id_emp = c.id_emp) " _
            & " Where " _
            & " a.codban = '" & CodigoBanco & "' and " _
            & " a.comproba = '" & Comprobante & "' and " _
            & " a.tipomov = 'CH' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by c.renglon "

    End Function

    Function SeleccionBANFormatoCheque() As String

        SeleccionBANFormatoCheque = "select 'xx' codpro, 'xx' tipomov, 'xx' nummov,  " _
            & " now() emision, '' refer,  'xxx' concepto, 1234567.89 importe, " _
            & " '' numpag, '' nompag,  'YOSI JIRO CHEKE CITO TOKA GAU' benefic, 'YOSI JIRO CHEKE CITO TOKA GAU' nombre, 'XX' comproba, '' zona, '' nomban, '' ctaban, '' formato " _
            & " from jsconctaemp a " _
            & " Where " _
            & " id_emp = '01' "

    End Function

End Module
