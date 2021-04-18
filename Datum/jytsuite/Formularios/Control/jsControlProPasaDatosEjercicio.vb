Imports MySql.Data.MySqlClient
Public Class jsControlProPasaDatosEjercicio
    Private Const sModulo As String = "Transfiere datos a ejercicio"
    Private Const nTabla As String = "tblDatosfsfsfs"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private ft As New Transportables

    Private CierreAnterior As Date
    Private UltimoEjercicio As String = ""
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        myConn = MyCon
        Me.Tag = sModulo

        lblLeyenda.Text = " Mediante este proceso se realiza el pase de datos a históricos del ejercicio deseado " + vbCr + _
                "  " + vbCr + _
                " - Si no esta seguro POR FAVOR CONSULTE con el administrador " + vbCr + _
                " - Se transferiran a históricos los datos obsoletos (cancelados, con saldo cero (0), etc.  ) " + vbCr + _
                " - Este proceso puede ser realizado para todos los ejercicios anteriores " + vbCr + _
                " - " + vbCr + _
                " - SI NO ESTA SEGURO por favor consulte CON EL ADMINISTRADOR "

        ft.mensajeEtiqueta(lblInfo, " ... ", Transportables.tipoMensaje.iAyuda)
        ft.habilitarObjetos(False, False, chkContabilidad, chkBancos, chkCXC, chkCXP, chkNomina, chkProduccion)
        IniciarEjercicios(myConn)

        Me.Show()

    End Sub
    
    Private Sub IniciarEjercicios(ByVal MyCon As MySqlConnection)
        Dim eCont As Integer
        Dim nTablaE As String = "tEjercicio"
        ds = DataSetRequery(ds, " select * from jsconctaeje where id_emp = '" & jytsistema.WorkID & "' order by ejercicio ", myConn, nTablaE, lblInfo)
        Dim aEjercicio(ds.Tables(nTablaE).Rows.Count - 1) As String
        aEjercicio(0) = "00000 | Ejercicio Actual"
        With ds.Tables(nTablaE)
            For eCont = 0 To .Rows.Count - 1
                aEjercicio(eCont) = .Rows(eCont).Item("ejercicio") & " | " & _
                    ft.FormatoFecha(CDate(.Rows(eCont).Item("inicio").ToString)) & " | " & ft.FormatoFecha(CDate(.Rows(eCont).Item("cierre").ToString))
            Next
        End With

        ds.Tables(nTablaE).Dispose()
        ft.RellenaCombo(aEjercicio, cmbEjercicio)

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub jsControlProCierreEjercicio_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing

    End Sub
    Private Sub jsControlProCierreEjercicio_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Procesar()
    End Sub
    Private Sub Procesar()

        'HabilitarCursorEnEspera()

        'If chkCXC.Checked Then ProcesarCXC()
        'If chkCXP.Checked Then ProcesarCXP()
        If chkMercancias.Checked Then procesarMercancias()
        If chkPOS.Checked Then procesarPOS()

        'DeshabilitarCursorEnEspera()
        ft.mensajeInformativo(" Proceso culminado con éxito... ")

    End Sub
    Private Sub procesarPOS()

        Dim pb As New Progress_Bar
        pb.WindowTitle = "PROCESANDO PUNTOS DE VENTAS..."
        pb.TimeOut = 60
        pb.CallerThreadSet = Threading.Thread.CurrentThread

        Dim FechaInicial As Date = CDate(cmbEjercicio.Text.Split("|")(1).ToString)
        Dim FechaFinal As Date = CDate(cmbEjercicio.Text.Split("|")(2).ToString)

        Dim dtFacturas As New DataTable
        dtFacturas = ft.AbrirDataTable(ds, "tblFacturas", myConn, " select * from jsvenencpos " _
                                        & " where " _
                                        & " emision >= '" & ft.FormatoFechaHoraMySQLInicial(FechaInicial) & "' and " _
                                        & " emision <= '" & ft.FormatoFechaHoraMySQLFinal(FechaFinal) & "' and " _
                                        & " id_emp = '" & jytsistema.WorkID & "' order by numfac ")

        For iCont As Integer = 0 To dtFacturas.Rows.Count - 1
            With dtFacturas.Rows(iCont)

                pb.OverallProgressValue = (iCont + 1) / dtFacturas.Rows.Count * 100
                pb.OverallProgressText = ">> " & .Item("NUMFAC") & " - " & ft.muestraCampoFecha(CDate(.Item("EMISION").ToString)) & " ..."

                'ENCABEZADO
                ft.Ejecutar_strSQL(myConn, " INSERT INTO jsvenencposhis " _
                                    & " select * from jsvenencpos " _
                                    & " where " _
                                    & " numfac = '" & .Item("NUMFAC") & "' and  " _
                                    & " id_emp = '" & .Item("ID_EMP") & "' ")

                ft.Ejecutar_strSQL(myConn, " DELETE from jsvenencpos " _
                                    & " where " _
                                    & " numfac = '" & .Item("NUMFAC") & "' and  " _
                                    & " id_emp = '" & .Item("ID_EMP") & "' ")

                'RENGLONES
                ft.Ejecutar_strSQL(myConn, " INSERT INTO jsvenrenposhis " _
                                   & " select * from jsvenrenpos " _
                                   & " where " _
                                   & " numfac = '" & .Item("NUMFAC") & "' and  " _
                                   & " id_emp = '" & .Item("ID_EMP") & "' ")

                ft.Ejecutar_strSQL(myConn, " DELETE from jsvenrenpos " _
                                   & " where " _
                                   & " numfac = '" & .Item("NUMFAC") & "' and  " _
                                   & " id_emp = '" & .Item("ID_EMP") & "' ")
                '(IVA)
                ft.Ejecutar_strSQL(myConn, " INSERT INTO jsvenivaposhis " _
                                   & " select * from jsvenivapos " _
                                   & " where " _
                                   & " numfac = '" & .Item("NUMFAC") & "' and  " _
                                   & " id_emp = '" & .Item("ID_EMP") & "' ")
                ft.Ejecutar_strSQL(myConn, " DELETE from jsvenivapos " _
                                   & " where " _
                                   & " numfac = '" & .Item("NUMFAC") & "' and  " _
                                   & " id_emp = '" & .Item("ID_EMP") & "' ")
                'DESCUENTOS
                ft.Ejecutar_strSQL(myConn, " INSERT INTO jsvendesposhis " _
                                  & " select * from jsvendespos " _
                                  & " where " _
                                  & " numfac = '" & .Item("NUMFAC") & "' and  " _
                                  & " id_emp = '" & .Item("ID_EMP") & "' ")
                ft.Ejecutar_strSQL(myConn, " DELETE from jsvendespos " _
                                  & " where " _
                                  & " numfac = '" & .Item("NUMFAC") & "' and  " _
                                  & " id_emp = '" & .Item("ID_EMP") & "' ")

                'MOVIMIENTOS
                ft.Ejecutar_strSQL(myConn, " INSERT INTO jsventraposhis " _
                                  & " select * from jsventrapos " _
                                  & " where " _
                                  & " NUMMOV = '" & .Item("NUMFAC") & .Item("NUMSERIAL") & "' and  " _
                                  & " id_emp = '" & .Item("ID_EMP") & "' ")
                ft.Ejecutar_strSQL(myConn, " DELETE from jsventrapos " _
                                  & " where " _
                                  & " NUMMOV = '" & .Item("NUMFAC") & .Item("NUMSERIAL") & "' and  " _
                                  & " id_emp = '" & .Item("ID_EMP") & "' ")

                'FORMA DE PAGO
                ft.Ejecutar_strSQL(myConn, " INSERT INTO jsvenforpaghis " _
                                  & " select * from jsvenforpag " _
                                  & " where " _
                                  & " numfac = '" & .Item("NUMFAC") & "' and  " _
                                  & " ORIGEN = 'PVE' AND " _
                                  & " id_emp = '" & .Item("ID_EMP") & "' ")
                ft.Ejecutar_strSQL(myConn, " DELETE FROM jsvenforpag " _
                                  & " where " _
                                  & " numfac = '" & .Item("NUMFAC") & "' and  " _
                                  & " ORIGEN = 'PVE' AND " _
                                  & " id_emp = '" & .Item("ID_EMP") & "' ")

            End With

        Next

        pb.OverallProgressValue = 100


    End Sub
    Private Sub procesarMercancias()

        Dim pb As New Progress_Bar
        pb.WindowTitle = "PROCESANDO MERCANCIAS..."
        pb.TimeOut = 60
        pb.CallerThreadSet = Threading.Thread.CurrentThread


        Dim dtMercancias As New DataTable
        dtMercancias = ft.AbrirDataTable(ds, "tblMercancias", myConn, " select * from jsmerctainv where id_emp = '" _
                                          & jytsistema.WorkID & "' order by codart ")

        Dim FechaInicial As Date = CDate(cmbEjercicio.Text.Split("|")(1).ToString)
        Dim FechaFinal As Date = CDate(cmbEjercicio.Text.Split("|")(2).ToString)

        For iCont As Integer = 0 To dtMercancias.Rows.Count - 1
            With dtMercancias.Rows(iCont)

                pb.OverallProgressText = ">> " & .Item("CODART") & " - " & .Item("NOMART") & " ..."
                pb.OverallProgressValue = (iCont + 1) / dtMercancias.Rows.Count * 100


                '1. Incluir movimientos en historico
                ft.Ejecutar_strSQL(myConn, " INSERT INTO jsmerhismer " _
                                    & " select * from jsmertramer " _
                                    & " where " _
                                    & " codart = '" & .Item("codart") & "' and " _
                                    & " fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaInicial) & "' and " _
                                    & " fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaFinal) & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "' ")

                'Dim dtMovimientos As New DataTable
                'dtMovimientos = ft.AbrirDataTable(ds, "tblmovimientos", myConn, " select * from jsmertramer " _
                '                                  & " where " _
                '                                  & " codart = '" & .Item("codart") & "' and " _
                '                                  & " fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaInicial) & "' and " _
                '                                  & " fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaFinal) & "' and " _
                '                                  & " id_emp = '" & jytsistema.WorkID & "' order by fechamov ")

                ''1. Incluir movimientos en historico
                'For jCont As Integer = 0 To dtMovimientos.Rows.Count - 1
                '    With dtMovimientos.Rows(jCont)

                '        pb.PartialProgressText = ft.FormatoFechaHoraMySQL(CDate(.Item("FECHAMOV").ToString)) & " | " & .Item("NUMDOC") & "..."
                '        pb.PartialProgressValue = (jCont + 1) / dtMovimientos.Rows.Count * 100

                '        Dim insertar As Boolean = True
                '        If ft.DevuelveScalarEntero(myConn, " select count(*) from jsmerhismer where " _
                '                & " codart = '" & .Item("CODART") & "' and " _
                '                & " date_format(fechamov, '%Y-%m-%d') = '" & ft.FormatoFechaMySQL(CDate(.Item("FECHAMOV").ToString)) & "' and " _
                '                & " tipomov = '" & .Item("TIPOMOV") & "' and " _
                '                & " numdoc = '" & .Item("NUMDOC") & "' and " _
                '                & " origen = '" & .Item("ORIGEN") & "' and " _
                '                & " numorg = '" & .Item("NUMORG") & "' and " _
                '                & " asiento = '" & .Item("ASIENTO") & "' and " _
                '                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                '                & " id_emp = '" & jytsistema.WorkID & "' ") > 0 Then insertar = False

                '        InsertEditMERCASMovimientoInventario(myConn, lblInfo, insertar, .Item("codart"), CDate(.Item("FECHAMOV").ToString), .Item("TIPOMOV"), _
                '             .Item("NUMDOC"), .Item("UNIDAD"), CDbl(.Item("CANTIDAD")), CDbl(.Item("PESO")), CDbl(.Item("COSTOTAL")), CDbl(.Item("COSTOTALDES")), _
                '             .Item("ORIGEN"), .Item("NUMORG"), .Item("LOTE"), .Item("PROV_CLI"), CDbl(.Item("VENTOTAL")), CDbl(.Item("VENTOTALDES")), CDbl(.Item("IMPIVA")), _
                '             CDbl(.Item("DESCUENTO")), .Item("VENDEDOR"), .Item("ALMACEN"), .Item("ASIENTO"), CDate(.Item("FECHASI").ToString), , False)

                '    End With
                'Next

                '2. Eliminar movimientos de la tabla de trabajo
                pb.PartialProgressText = " ELIMINANDO REGISTROS EN MOVIMIENTOS ..."
                pb.PartialProgressValue = 66
                ft.Ejecutar_strSQL(myConn, " DELETE FROM jsmertramer " _
                                   & " where " _
                                   & " codart = '" & .Item("codart") & "' and " _
                                   & " fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaInicial) & "' and " _
                                   & " fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaFinal) & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' ")

                '3. Resumir movimientos recien pasados a historico en uno solo
                pb.PartialProgressText = " RESUMIENDO E INCLUYENDO EN MOVIMIENTOS ..."
                pb.PartialProgressValue = 100
                ft.Ejecutar_strSQL(myConn, " insert into jsmertramer " _
                                   & " SELECT CODART, '" & ft.FormatoFechaHoraMySQLFinal(FechaFinal) & "' FECHAMOV, " _
                                   & " TIPOMOV, '" & ft.FormatoFechaMySQL(FechaFinal).Replace("-", "") & "', UNIDAD, " _
                                   & " SUM(CANTIDAD) CANTIDAD, SUM(PESO) PESO, SUM( IF( ORIGEN = 'TRF', 0, COSTOTAL) ) COSTOTAL,  " _
                                   & " SUM(IF( ORIGEN = 'TRF', 0, COSTOTALDES)) COSTOTALDES, 'INV' ORIGEN, " _
                                   & " '" & ft.FormatoFechaMySQL(FechaFinal).Replace("-", "") & "' NUMORG, '' LOTE, '' PROV_CLI, VENTOTAL, VENTOTALDES, " _
                                   & " SUM(IMPIVA), DESCUENTO, '' VENDEDOR, ALMACEN, '' ASIENTO, '" & ft.FormatoFechaMySQL(FechaFinal) & "' FECHASI, " _
                                   & " '2009-01-01' BLOCK_DATE, EJERCICIO, ID_EMP " _
                                   & " FROM jsmerhismer " _
                                   & " WHERE " _
                                   & " TIPOMOV IN ('EN', 'AE') AND " _
                                   & " codart = '" & .Item("codart") & "' AND " _
                                   & " fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaInicial) & "' and " _
                                   & " fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaFinal) & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' " _
                                   & " GROUP BY tipomov, almacen, unidad " _
                                   & " UNION " _
                                   & " SELECT CODART, '" & ft.FormatoFechaHoraMySQLFinal(FechaFinal) & "' FECHAMOV, " _
                                   & " TIPOMOV, '" & ft.FormatoFechaMySQL(FechaFinal).Replace("-", "") & "', UNIDAD, " _
                                   & " SUM(CANTIDAD), SUM(PESO), SUM(IF( ORIGEN = 'TRF', 0, COSTOTAL)), SUM(IF( ORIGEN = 'TRF', 0, COSTOTALDES)), 'INV' ORIGEN, " _
                                   & " '" & ft.FormatoFechaMySQL(FechaFinal).Replace("-", "") & "' NUMORG, '' LOTE, '' PROV_CLI, " _
                                   & " SUM(VENTOTAL), SUM(VENTOTALDES), SUM(IMPIVA), DESCUENTO, '' VENDEDOR, ALMACEN, '' ASIENTO, '" & ft.FormatoFechaMySQL(FechaFinal) & "' FECHASI,  " _
                                   & " '2009-01-01' BLOCK_DATE, EJERCICIO, ID_EMP " _
                                   & " FROM jsmerhismer " _
                                   & " WHERE  " _
                                   & " TIPOMOV IN ('SA', 'AS') AND " _
                                   & " codart = '" & .Item("codart") & "' AND " _
                                   & " fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaInicial) & "' and " _
                                   & " fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaFinal) & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' " _
                                   & " GROUP BY tipomov, almacen, unidad " _
                                   & " UNION  " _
                                   & " SELECT CODART, '" & ft.FormatoFechaHoraMySQLFinal(FechaFinal) & "' FECHAMOV, " _
                                   & " TIPOMOV, '" & ft.FormatoFechaMySQL(FechaFinal).Replace("-", "") & "', UNIDAD, " _
                                   & " SUM(CANTIDAD), SUM(PESO), SUM(COSTOTAL), SUM(COSTOTALDES), 'INV' ORIGEN,  " _
                                   & " '" & ft.FormatoFechaMySQL(FechaFinal).Replace("-", "") & "' NUMORG, '' LOTE, '' PROV_CLI, " _
                                   & " VENTOTAL, VENTOTALDES, IMPIVA, " _
                                   & " DESCUENTO, '' VENDEDOR, ALMACEN, '' ASIENTO, '" & ft.FormatoFechaMySQL(FechaFinal) & "' FECHASI,  " _
                                   & " '2009-01-01' BLOCK_DATE, EJERCICIO, ID_EMP " _
                                   & " FROM jsmerhismer " _
                                   & " WHERE " _
                                   & " TIPOMOV IN ('AC') AND " _
                                   & " codart = '" & .Item("codart") & "' AND " _
                                   & " fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaInicial) & "' and " _
                                   & " fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaFinal) & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' " _
                                   & " GROUP BY tipomov, almacen, unidad ")

            End With
        Next

        pb.PartialProgressValue = 100
        pb.OverallProgressValue = 100

    End Sub
    Private Sub ProcesarCXC()

        '    Dim dFecha As Date = CDate(cmbEjercicio.Text.Split("|")(2).ToString)

        '    ds = DataSetRequery(ds, " SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) importe " _
        '                        & " FROM jsventracob a " _
        '                        & " WHERE " _
        '                        & " a.codcli <>  '' AND " _
        '                        & " a.emision <= '" & ft.FormatoFechaMySQL(dFecha) & "' AND " _
        '                        & " a.id_emp = '" & jytsistema.WorkID & "' " _
        '                        & " GROUP BY a.codcli, a.nummov " _
        '                        & " HAVING ROUND(ABS(IMPORTE), 2)  <= 0.02 ", _
        '                         myConn, nTabla, lblInfo)

        '    dt = ds.Tables(nTabla)

        '    Dim eCont As Integer = 0
        '    If dt.Rows.Count > 0 Then
        '        For eCont = 0 To dt.Rows.Count - 1
        '            With dt.Rows(eCont)

        '                'refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(eCont / dt.Rows.Count * 100), _
        '                '                              " PROCESANDO CxC -> CLIENTE : " & .Item("CODCLI") & " - N° DOC.: " & .Item("NUMMOV"))

        '                ft.Ejecutar_strSQL(myconn, " INSERT INTO jsvenhiscob SELECT * FROM jsventracob " _
        '                               & " WHERE " _
        '                               & " emision <= '" & ft.FormatoFechaMySQL(dFecha) & "' AND " _
        '                               & " CODCLI = '" & .Item("CODCLI") & "' and " _
        '                               & " NUMMOV = '" & .Item("NUMMOV") & "' and " _
        '                               & " ID_EMP = '" & jytsistema.WorkID & "' ")

        '                ft.Ejecutar_strSQL(myconn, " DELETE FROM jsventracob " _
        '                               & " WHERE " _
        '                               & " emision <= '" & ft.FormatoFechaMySQL(dFecha) & "' AND " _
        '                               & " CODCLI = '" & .Item("CODCLI") & "' and " _
        '                               & " NUMMOV = '" & .Item("NUMMOV") & "' and " _
        '                               & " ID_EMP = '" & jytsistema.WorkID & "' ")

        '            End With
        '        Next
        '    End If

        'End Sub

        'Private Sub ProcesarCXP()

        '    Dim dFecha As Date = CDate(cmbEjercicio.Text.Split("|")(2).ToString)

        '    ds = DataSetRequery(ds, " SELECT a.codpro, a.nummov, IFNULL(SUM(a.IMPORTE),0) importe " _
        '                        & " FROM jsprotrapag a " _
        '                        & " WHERE " _
        '                        & " a.codpro <>  '' AND " _
        '                        & " a.emision <= '" & ft.FormatoFechaMySQL(dFecha) & "' AND " _
        '                        & " a.id_emp = '" & jytsistema.WorkID & "' " _
        '                        & " GROUP BY a.codpro, a.nummov " _
        '                        & " HAVING ROUND(ABS(IMPORTE), 2)  <= 0.02 ", _
        '                         myConn, nTabla, lblInfo)

        '    dt = ds.Tables(nTabla)

        '    Dim eCont As Integer = 0
        '    If dt.Rows.Count > 0 Then
        '        For eCont = 0 To dt.Rows.Count - 1
        '            With dt.Rows(eCont)

        '                'refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(eCont / dt.Rows.Count * 100), _
        '                '                              " PROCESANDO CxP -> CODPRO : " & .Item("CODPRO") & " - N° DOC.: " & .Item("NUMMOV"))

        '                ft.Ejecutar_strSQL(myconn, " INSERT INTO jsprohispag SELECT * FROM jsprotrapag " _
        '                               & " WHERE " _
        '                               & " emision <= '" & ft.FormatoFechaMySQL(dFecha) & "' AND " _
        '                               & " CODPRO = '" & .Item("CODPRO") & "' and " _
        '                               & " NUMMOV = '" & .Item("NUMMOV") & "' and " _
        '                               & " ID_EMP = '" & jytsistema.WorkID & "' ")

        '                ft.Ejecutar_strSQL(myconn, " DELETE FROM jsprotrapag " _
        '                               & " WHERE " _
        '                               & " emision <= '" & ft.FormatoFechaMySQL(dFecha) & "' AND " _
        '                               & " CODPRO = '" & .Item("CODPRO") & "' and " _
        '                               & " NUMMOV = '" & .Item("NUMMOV") & "' and " _
        '                               & " ID_EMP = '" & jytsistema.WorkID & "' ")

        '            End With
        '        Next
        '    End If


    End Sub






End Class