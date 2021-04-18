Imports MySql.Data.MySqlClient
Module ConsultasContabilidad
    Private ft As New Transportables
    Public Function SeleccionCOTCuentasContables(ByVal CuentaDesde As String, ByVal CuentaHasta As String) As String

        Dim str As String = ""
        If CuentaDesde <> "" Then str = str & " a.codcon >= '" & CuentaDesde & "' and "
        If CuentaHasta <> "" Then str = str & " a.codcon <= '" & CuentaHasta & "' and "

        SeleccionCOTCuentasContables = " select a.codcon codigo, a.descripcion , LPAD(codcon, 2*LENGTH(codcon), ' ') codigoampliado, " _
            & " LPAD(a.descripcion, LENGTH(a.codcon) + LENGTH(a.descripcion), ' ') descripcionampliada, a.id_emp from jscotcatcon  a  " _
            & " where " _
            & str _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.codcon "

    End Function
    Public Function SeleccionCOTPolizasContables(ByVal TipoAsiento As Asiento, _
    Optional ByVal Asientodesde As String = "", Optional ByVal AsientoHasta As String = "", _
    Optional ByVal FechaDesde As Date = #1/1/2007#, Optional ByVal FechaHasta As Date = #1/1/2007#) As String

        Dim str As String = ""

        If Asientodesde <> "" Then str += " a.asiento >= '" & Asientodesde & "' and "
        If AsientoHasta <> "" Then str += " a.asiento <= '" & AsientoHasta & "' and "
        If FechaDesde <> #1/1/2007# Then str += " a.fechasi >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and "
        If FechaHasta <> #1/1/2007# Then str += " a.fechasi <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and "

        SeleccionCOTPolizasContables = " select a.asiento, a.fechasi, a.descripcion, a.debitos total_debitos, a.creditos total_creditos, " _
            & " b.codcon, c.descripcion nombre, b.referencia, b.concepto, if( debito_credito = 0,  b.importe, 0.00) debitos, " _
            & " if( debito_credito = 1,  abs(b.importe), 0.00) creditos, a.id_emp " _
            & " from jscotencasi a " _
            & " left join jscotrenasi b on (a.asiento = b.asiento and a.actual = b.actual and a.ejercicio = b.ejercicio and a.id_emp = b.id_emp) " _
            & " left join jscotcatcon c on (b.codcon = c.codcon and b.id_emp = c.id_emp ) " _
            & " Where " _
            & str _
            & " a.actual = " & TipoAsiento & " and " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' "

    End Function

    Public Function SeleccionCOTPlantillaAsiento(Optional ByVal Asientodesde As String = "", Optional ByVal AsientoHasta As String = "") As String

        Dim str As String = ""

        If Asientodesde <> "" Then str += " a.asiento >= '" & Asientodesde & "' and "
        If AsientoHasta <> "" Then str += " a.asiento <= '" & AsientoHasta & "' and "

        SeleccionCOTPlantillaAsiento = " select a.asiento, a.descripcion,  " _
            & " b.codcon, c.descripcion nombre, b.referencia, b.concepto, " _
            & " b.regla,  if( b.signo = 0, '+', '-') signo, a.id_emp " _
            & " from jscotencdef a " _
            & " left join jscotrendef b on (a.asiento = b.asiento and a.id_emp = b.id_emp) " _
            & " left join jscotcatcon c on (b.codcon = c.codcon and b.id_emp = c.id_emp ) " _
            & " Where " _
            & str _
            & " a.id_emp = '" & jytsistema.WorkID & "' "

    End Function

    Public Function SeleccionCOTMayorAnalitico(ByVal FechaDesde As Date, ByVal Fechahasta As Date, ByVal CuentaInicial As String, ByVal CuentaFinal As String) As String

        Dim str As String = ""

        If CuentaInicial <> "" Then str = str & " substring(a.codcon,1," & Len(CuentaInicial) & ")  >= '" & CuentaInicial & "' and "
        If CuentaFinal <> "" Then str = str & " substring(a.codcon,1," & Len(CuentaFinal) & ")  <= '" & CuentaFinal & "' and "

        SeleccionCOTMayorAnalitico = " select a.codcon, c.descripcion, b.fechasi, a.asiento, a.referencia, a.concepto, " _
            & " date_format( b.fechasi, '%b/%Y') mes, if( d.saldo is null, 0.00, d.saldo) saldo, " _
            & " if( a.debito_credito = 0 , a.importe, 0.00) debitos, " _
            & " if( a.debito_credito = 1 , abs(a.importe), 0.00) creditos " _
            & " from jscotrenasi a " _
            & " left join jscotencasi b on (a.asiento = b.asiento and a.actual = b.actual and a.ejercicio = b.ejercicio and a.id_emp = b.id_emp) " _
            & " left join jscotcatcon c on (a.codcon = c.codcon and a.id_emp = c.id_emp) " _
            & " Left join ( select a.codcon, IFNULL(SUM(a.IMPORTE),0) saldo, a.ejercicio, a.id_emp from jscotrenasi a " _
            & " left join jscotencasi b on ( a.asiento = b.asiento and a.actual = b.actual and a.ejercicio = b.ejercicio and a.id_emp = b.id_emp ) " _
            & " Where " _
            & " b.fechasi < '" & ft.FormatoFechaMySQL(FechaDesde) & "' and " _
            & " a.actual = 1 and " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by a.codcon ) d on (a.codcon = d.codcon  and a.ejercicio = d.ejercicio and a.id_emp = d.id_emp ) " _
            & " Where " _
            & str _
            & " b.fechasi >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and " _
            & " b.fechasi <= '" & ft.FormatoFechaMySQL(Fechahasta) & "' and " _
            & " a.actual = 1 and " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by a.codcon, b.fechasi, a.asiento "

    End Function

    Public Function SeleccionCOTBalanceDeComprobacionPlus(ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                                       ByVal CuentaDesde As String, ByVal CuentaHasta As String, ByVal Balance As Boolean, _
                                                       ByVal CuentasConMovimientos As Boolean, Optional ByVal CuentasResultadoSolamente As Boolean = False) As String
        ' SIRVE PARA EL LIBRO DIARIO

        Dim str As String = ""
        Dim strX As String = ""

        If CuentaDesde <> "" Then str += " a.codcon >= '" & CuentaDesde & "' and "
        If CuentaHasta <> "" Then str += " a.codcon <= '" & CuentaHasta & "' and "
        If CuentasResultadoSolamente Then str += " RIGHT(a.codcon,1) <> '.' and "

        If Balance Then
            If CuentasConMovimientos Then strX = " having  ( saldoanterior <> 0.00 or debitos <> 0.00 or creditos <> 0.00 )  "
        Else
            If CuentasConMovimientos Then strX = " having  ( debitos <> 0.00 or creditos <> 0.00 )  "
        End If

        SeleccionCOTBalanceDeComprobacionPlus = " SELECT a.codcon, a.descripcion, if( SUM(b.debitos) is null, 0.00, sum(b.debitos) ) + if ( SUM(b.creditos) is null, 0.00, sum(b.creditos) ) saldoanterior, " _
                & " if (SUM(b.debitosmes) is null, 0.00, sum(b.debitosmes)) debitos, if( SUM(b.creditosmes) is null, 0.00, sum(b.creditosmes)) creditos,  a.id_emp " _
                & " FROM jscotcatcon a " _
                & " LEFT JOIN (SELECT a.codcon, SUM(IF ( a.debito_credito = 0 AND b.fechasi < '" & ft.FormatoFechaMySQL(FechaDesde) & "' , a.importe,0 ))  debitos,  " _
                & "             SUM(IF ( a.debito_credito = 1 AND b.fechasi < '" & ft.FormatoFechaMySQL(FechaDesde) & "' , a.importe,0)) creditos,  " _
                & "             SUM(IF ( a.debito_credito = 0 AND b.fechasi >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND b.fechasi <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' , a.importe,0)) debitosmes,  " _
                & "             SUM(IF ( a.debito_credito = 1 AND b.fechasi >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND b.fechasi <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' , a.importe,0)) creditosmes, " _
                & "             a.ejercicio, a.id_emp " _
                & "             FROM jscotrenasi a  " _
                & "             LEFT JOIN jscotencasi b ON ( a.asiento = b.asiento AND a.actual = b.actual AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp )  " _
                & "             WHERE " _
                & "             b.fechasi <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND  " _
                & "             a.actual = 1 AND  " _
                & "             a.ejercicio = '" & jytsistema.WorkExercise & "' AND  " _
                & "             a.id_emp = '" & jytsistema.WorkID & "'  " _
                & "             GROUP BY a.codcon) b ON (a.codcon = SUBSTRING( b.codcon, 1, LENGTH(a.codcon))  AND a.id_emp = b.id_emp ) " _
                & " WHERE " _
                & str _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " GROUP BY a.codcon " _
                & strX _
                & " ORDER BY a.codcon "

    End Function

    Public Function SeleccionCOTBalanceGeneral(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal Nivel As Integer, ByVal Balance As Boolean, _
                                               Ejercicio As String) As String

        Dim CuentaResultado As String = ParametroPlus(MyConn, Gestion.iContabilidad, "CONPARAM03")

        Dim strResultado As String = IIf(Balance, " UNION (SELECT '" & CuentaResultado & "' codcon, IFNULL(SUM(a.IMPORTE),0) saldo, a.ejercicio, a.id_emp  " _
        & " FROM jscotrenasi a   " _
        & "     LEFT JOIN jscotencasi b ON (a.asiento = b.asiento AND a.actual = b.actual AND a.id_emp = b.id_emp)   " _
        & "     left join jscotcatcon c on ( a.codcon = c.codcon and a.id_emp = c.id_emp )" _
        & " WHERE " _
        & " b.fechasi >= '" & ft.FormatoFechaMySQL(DateAdd(DateInterval.Day, -1, FechaDesde)) & "' AND " _
        & " b.fechasi <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND   " _
        & " a.codcon >= '4' AND " _
        & " a.actual = 1 AND " _
        & " a.ejercicio = '" & Ejercicio & "' and " _
        & " a.id_emp = '" & jytsistema.WorkID & "' " _
        & " GROUP BY a.id_emp )", "")



        SeleccionCOTBalanceGeneral = "SELECT a.codcon, a.descripcion, a.nivel, if(" & Balance & ",1,-1)*if( SUM(b.saldo) is null, 0.00, sum(b.saldo) ) saldo, a.id_emp " _
                & " FROM jscotcatcon a " _
                & " LEFT JOIN (SELECT a.codcon, IFNULL(SUM(a.IMPORTE),0) saldo,  " _
                & "             a.ejercicio, a.id_emp " _
                & "             FROM jscotrenasi a  " _
                & "             LEFT JOIN jscotencasi b ON (a.asiento = b.asiento AND a.actual = b.actual and a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp )  " _
                & "             WHERE " _
                & "             NOT b.asiento IS NULL and " _
                & IIf(Balance, "", "b.fechasi >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND ") _
                & "             b.fechasi <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND  " _
                & "             a.actual = 1 AND  " _
                & "             a.ejercicio = '" & Ejercicio & "' and " _
                & "             a.id_emp = '" & jytsistema.WorkID & "'  " _
                & "             GROUP BY a.codcon " & strResultado & " ) b ON (a.codcon = SUBSTRING( b.codcon, 1, LENGTH(a.codcon))  AND a.id_emp = b.id_emp ) " _
                & " WHERE " _
                & " a.nivel <= " & Nivel & " and " _
                & IIf(Balance, " a.codcon < '4' and ", " a.codcon >= '4' and ") _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " GROUP BY a.codcon " _
                & " ORDER BY 1 "

        '              


    End Function

    Public Function SeleccionCOTReglas(ByVal ReglaDesde As String, ByVal ReglaHasta As String)
        Dim str As String = ""
        If ReglaDesde <> "" Then str += " plantilla >= '" & ReglaDesde & "' and "
        If ReglaHasta <> "" Then str += " plantilla <= '" & ReglaHasta & "' and "

        SeleccionCOTReglas = " select *  " _
            & " from jscotcatreg " _
            & " where " _
            & str _
            & " id_emp = '" & jytsistema.WorkID & "' order by plantilla "

    End Function
End Module
