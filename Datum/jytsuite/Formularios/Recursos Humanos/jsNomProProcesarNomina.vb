Imports MySql.Data.MySqlClient
Public Class jsNomProProcesarNomina
    Private Const sModulo As String = "Procesar nómina"
    Private Const nTabla As String = "pronomina"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Public Sub Cargar(ByVal MyCon As MySqlConnection)
        myConn = MyCon
        Me.Tag = sModulo

        lblLeyenda.Text = " Mediante este proceso se crea la nómina la cual pasa a histórico los conceptos antes definidos " + vbCr + _
                " de la siguiente forma.  " + vbCr + _
                " 1. Procesa conceptos de nómina colocándolos en histórico. " + vbCr + _
                " 2. Calcula o coloca la siguiente cuota/préstamo. " + vbCr + _
                " 3. Calcula saldo cuota/préstamo." + vbCr + _
                " 4. Pasa a cuentas por pagar el saldo o los saldos de proveedor de gastos asociado al concepto. " + vbCr + _
                " 5. Actualiza tabla de fechas de Nómina procesadas."

        ft.mensajeEtiqueta(lblInfo, " ... ", Transportables.TipoMensaje.iAyuda)

        ft.habilitarObjetos(False, True, txtNomina, cmbTipo)

        txtFechaDesde.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtFechaHasta.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)

        Me.Show()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub jsNomProProcesarNomina_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt.Dispose()
    End Sub

    Private Sub jsNomProProcesarNomina_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ft.habilitarObjetos(False, False, txtFechaDesde, txtFechaHasta)
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If lblNomina.Text.Trim = "" Then
            ft.MensajeCritico("Debe indicar/seleccionar una NOMINA VALIDA...")
            Exit Sub
        End If

        If CDate(txtFechaDesde.Text) > CDate(txtFechaHasta.Text) Then
            ft.MensajeCritico("Debe indicar una rango de fechas válido...")
            Exit Sub
        End If
        ProcesarNomina()
        Me.Close()
    End Sub
    Private Sub ProcesarNomina()
        Dim iCont As Integer = 0
        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        For iCont = 0 To dt.Rows.Count - 1
            With dt.Rows(iCont)

                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (iCont + 1) / dt.Rows.Count * 100, _
                                              " Trabajador : " + .Item("codtra") + " " + .Item("nomtra") + " - Concepto : " + .Item("codcon"))
                '1.
                If .Item("importe") > 0 Then
                    ProcesarConcepto(.Item("codtra"), .Item("codcon"), CDate(txtFechaDesde.Text), CDate(txtFechaHasta.Text), _
                                     .Item("importe"), .Item("cuota"), .Item("num_cuota"), .Item("porcentaje_asig"), _
                                    ft.DevuelveScalarCadena(myConn, " select codigocon from jsnomcatcon where codcon = '" & .Item("codcon") & "' and id_emp = '" & jytsistema.WorkID & "' "), _
                                      txtNomina.Text)

                    '2.- Calcula siguiente cuota/prestamo
                    If .Item("cuota") <> "" Then
                        ' 2.1.- Coloca la cuota/prestamo como procesada
                        ft.Ejecutar_strSQL(myConn, " update jsnomrenpre set procesada = 1, fechainicio = '" & ft.FormatoFechaMySQL(CDate(txtFechaDesde.Text)) & "', fechafin = '" & ft.FormatoFechaMySQL(CDate(txtFechaHasta.Text)) & "'  " _
                                       & " where " _
                                       & " codtra = '" & .Item("codtra") & "' and " _
                                       & " codnom = '" & txtNomina.Text & "' and " _
                                       & " codpre = '" & .Item("cuota") & "' and " _
                                       & " num_cuota = " & .Item("num_cuota") & " and " _
                                       & " id_emp = '" & jytsistema.WorkID & "' ")
                        '2.2.- Crea la nueva cuota si es cuota universal
                        Dim NumeroCuotas As Integer = ft.DevuelveScalarEntero(myConn, " select numcuotas from jsnomencpre where codtra = '" & .Item("codtra") & "' AND CODNOM = '" & txtNomina.Text & "' and codpre = '" & .Item("cuota") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        If NumeroCuotas = 9999 Then
                            InsertEditNOMINARenglonCuota(myConn, lblInfo, True, .Item("codtra"), txtNomina.Text, .Item("cuota"), 1, .Item("importe"), _
                                     0.0, 0.0, 0, CDate(txtFechaDesde.Text), CDate(txtFechaHasta.Text))
                        End If
                    End If
                End If
            End With

        Next
        '3.- Calcula saldo de cuotas/prestasmo
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, "Actualizando saldos de cuotas/préstamos ... ")
        ft.Ejecutar_strSQL(myConn, " UPDATE jsnomencpre a, (SELECT codtra, codnom, codpre, SUM(monto) saldo, id_emp " _
                                & "                     FROM jsnomrenpre " _
                                & "                     WHERE " _
                                & "                     procesada = 0 AND " _
                                & "                     CODNOM = '" & txtNomina.Text & "' AND " _
                                & "                     id_emp = '" & jytsistema.WorkID & "' " _
                                & "                     GROUP BY " _
                                & "                     codnom, codtra, codpre) b " _
                                & " SET a.saldo = b.saldo " _
                                & " WHERE " _
                                & " a.codnom = '" & txtNomina.Text & "' AND " _
                                & " a.ID_EMP = '" & jytsistema.WorkID & "' AND " _
                                & " a.codnom = b.codnom AND " _
                                & " a.codtra =  b.codtra AND  " _
                                & " a.codpre = b.codpre AND " _
                                & " a.id_emp = b.id_emp ")

        '4.- Pasa a cuentas por pagar el saldo o los saldos de proveedor de gastos asociado al concepto. 
        'lblProgreso.Text = " Pasando acumulados por concepto a CXP de proveedor de gastos... "
        'refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso)
        'ActualizarCXP()

        '5.- Actualiza histórico de nómina
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " Actualizando fechas nómina ...")
        ActualizaFechaNomina()

        ft.mensajeInformativo("PROCESO CULMINADO ...")
        lblProgreso.Text = ""
        ProgressBar1.Value = 0

    End Sub
    Private Sub ActualizarCXP()
        Dim dtCXP As DataTable
        Dim tblCXP As String = "tblcxp"
        ds = DataSetRequery(ds, " SELECT a.codcon, b.codpro, b.nomcon, b.tipo, IFNULL(SUM(a.IMPORTE),0) total FROM jsnomhisdes a " _
                                    & " LEFT JOIN jsnomcatcon b ON (a.codcon = b.codcon AND a.id_emp = b.id_emp) " _
                                    & " WHERE " _
                                    & " b.tipo <> 2 and " _
                                    & " b.codpro <> '' and " _
                                    & " a.desde >= '" & ft.FormatoFechaMySQL(CDate(txtFechaDesde.Text)) & "' AND " _
                                    & " a.hasta <= '" & ft.FormatoFechaMySQL(CDate(txtFechaHasta.Text)) & "' AND " _
                                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                    & " GROUP BY a.codcon, b.codpro ", myConn, tblCXP, lblInfo)

        dtCXP = ds.Tables(tblCXP)

        ft.Ejecutar_strSQL(myconn, "DELETE FROM jsprotrapag where mid(refer,1,16) = '" & Format(CDate(txtFechaDesde.Text), "yyyyMMdd") _
                            & Format(CDate(txtFechaHasta.Text), "yyyyMMdd") & "' and id_emp = '" & jytsistema.WorkID & "' ")

        If dtCXP.Rows.Count > 0 Then
            Dim iiCont As Integer
            For iiCont = 0 To dtCXP.Rows.Count - 1
                With dtCXP.Rows(iiCont)
                    Dim Signo As Integer = 1
                    Dim TipoMovimiento As String = ""
                    Dim nContador As String = ""
                    If .Item("tipo") = 1 Then
                        Signo = 1
                        TipoMovimiento = "NC"
                        nContador = Contador(myConn, lblInfo, Gestion.iCompras, "COMNUMCCR", "11")
                    Else
                        Signo = -1
                        TipoMovimiento = "ND"
                        nContador = Contador(myConn, lblInfo, Gestion.iCompras, "COMNUMCDB", "12")

                    End If
                    Dim nReferencia As String = Format(CDate(txtFechaDesde.Text), "yyyyMMdd") _
                                                    & Format(CDate(txtFechaHasta.Text), "yyyyMMdd") _
                                                    & .Item("codcon")

                    InsertEditCOMPRASCXP(myConn, lblInfo, True, .Item("codpro"), TipoMovimiento, nContador, jytsistema.sFechadeTrabajo, ft.FormatoHora(Now), _
                                         jytsistema.sFechadeTrabajo, nReferencia, .Item("nomcon") & " " & ft.FormatoFechaMySQL(CDate(txtFechaDesde.Text)) & "/" & _
                                         ft.FormatoFechaMySQL(CDate(txtFechaHasta.Text)), Signo * .Item("total"), 0.0, "", "", "", "", "NOM", _
                                         "", "", "", "", nReferencia, "0", "", jytsistema.sFechadeTrabajo, "", "0", "", 0.0, 0.0, "", "", "", "", _
                                         "", "", TipoProveedor.Gastos, IIf(Signo = 1, FOTipo.Credito, FOTipo.Debito), "0")

                End With
            Next
        End If

        dtCXP.Dispose()
        dtCXP = Nothing

    End Sub
    Private Sub ActualizaFechaNomina()

        Dim aFld() As String = {"desde", "hasta", "tiponom", "id_emp", "CODNOM"}
        Dim aFldS() As String = {ft.FormatoFechaMySQL(CDate(txtFechaDesde.Text)), ft.FormatoFechaMySQL(CDate(txtFechaHasta.Text)), cmbTipo.SelectedIndex, jytsistema.WorkID, txtNomina.Text}
        If Not qFound(myConn, lblInfo, "jsnomfecnom", aFld, aFldS) Then _
            ft.Ejecutar_strSQL(myConn, "insert into jsnomfecnom set " _
                & " desde = '" & ft.FormatoFechaMySQL(CDate(txtFechaDesde.Text)) & "', " _
                & " hasta = '" & ft.FormatoFechaMySQL(CDate(txtFechaHasta.Text)) & "', " _
                & " tiponom = " & cmbTipo.SelectedIndex & ", " _
                & " id_emp = '" & jytsistema.WorkID & "', " _
                & " CODNOM = '" & txtNomina.Text & "' ")

        ft.Ejecutar_strSQL(myconn, " update jsnomencnom set ult_desde = '" & ft.FormatoFechaMySQL(CDate(txtFechaDesde.Text)) & "', ult_hasta = '" & ft.FormatoFechaMySQL(CDate(txtFechaHasta.Text)) & "' where codnom = '" & txtNomina.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub ProcesarConcepto(ByVal CodigoTrabajador As String, ByVal CodigoConcepto As String, ByVal Desde As Date, _
                                 ByVal Hasta As Date, ByVal Importe As Double, ByVal CodigoPrestamo As String, ByVal num_cuota As Integer, _
                                 PorcentajeSobreAsignacion As Double, CodigoContable As String, CodigoNomina As String)

        Dim aFld() As String = {"codtra", "codcon", "desde", "hasta", "id_emp", "codnom"}
        Dim aFldS() As String = {CodigoTrabajador, CodigoConcepto, ft.FormatoFechaMySQL(Desde), ft.FormatoFechaMySQL(Hasta), jytsistema.WorkID, CodigoNomina}
        Dim Insertar As Boolean = True
        If qFound(myConn, lblInfo, "jsnomhisdes", aFld, aFldS) Then Insertar = False
        InsertEditNOMINAHistoricoConcepto(myConn, lblInfo, Insertar, CodigoTrabajador, CodigoConcepto, Desde, Hasta, _
                        Importe, CodigoPrestamo, num_cuota, "", jytsistema.sFechadeTrabajo, PorcentajeSobreAsignacion, _
                         CodigoContable, txtNomina.Text)

    End Sub
    Private Sub btnFechaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaDesde.Click
        txtFechaDesde.Text = SeleccionaFecha(CDate(txtFechaDesde.Text), Me, btnFechaDesde)
    End Sub

    Private Sub btnFechaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaHasta.Click
        txtFechaHasta.Text = SeleccionaFecha(CDate(txtFechaHasta.Text), Me, btnFechaHasta)
    End Sub

    Private Sub txtFechaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFechaDesde.TextChanged
        txtFechaHasta.Text = txtFechaDesde.Text
    End Sub

    Private Sub cmbTipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipo.SelectedIndexChanged

        ColocaRangoFechasNomina(cmbTipo.SelectedIndex)

        strSQL = " (SELECT a.codtra, CONCAT(b.apellidos, ', ', b.nombres) nomtra, " _
                        & " a.codcon, a.importe, c.cuota, 0 num_cuota,  c.codpro, a.porcentaje_asig " _
                        & " FROM jsnomtrades a " _
                        & " LEFT JOIN jsnomcattra b ON (a.codtra = b.codtra AND a.id_emp = b.id_emp) " _
                        & " LEFT JOIN jsnomcatcon c ON (a.codnom = c.codnom AND a.codcon = c.codcon AND a.id_emp = c.id_emp) " _
                        & " LEFT JOIN jsnomrennom d on (a.codnom = d.codnom AND a.codtra = d.codtra and a.id_emp = d.id_emp) " _
                        & " WHERE " _
                        & " c.cuota = '' AND " _
                        & " a.codnom = '" & txtNomina.Text & "' AND " _
                        & " a.id_emp = '" & jytsistema.WorkID & "') " _
                        & " UNION " _
                        & " (SELECT a.codtra, CONCAT(b.apellidos, ', ', b.nombres) nomtra, " _
                        & " a.codcon, a.importe, c.cuota, d.num_cuota, c.codpro, a.porcentaje_asig " _
                        & " FROM jsnomtrades a " _
                        & " LEFT JOIN jsnomcattra b ON (a.codtra = b.codtra AND a.id_emp = b.id_emp) " _
                        & " LEFT JOIN jsnomcatcon c ON (a.codnom = c.codnom AND a.codcon = c.codcon AND a.id_emp = c.id_emp) " _
                        & " LEFT JOIN jsnomrenpre d ON (a.codnom = d.codnom AND a.codtra = d.codtra AND c.cuota = d.codpre AND c.id_emp = d.id_emp) " _
                        & " LEFT JOIN jsnomrennom e ON (a.codnom = e.codnom AND a.codtra = e.codtra AND a.ID_EMP = e.ID_EMP) " _
                        & " WHERE " _
                        & " d.procesada = 0 AND " _
                        & " a.codnom = '" & txtNomina.Text & "' AND " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' " _
                        & " GROUP BY a.codtra HAVING importe > 0 ) " _
                        & " ORDER BY 2, 3 "



    End Sub
    Private Sub ColocaRangoFechasNomina(ByVal TipoNomina As Integer)
        Dim nTablaNom As String = "tblnom"
        Dim dtNom As DataTable
        Dim Fecha As Date = jytsistema.sFechadeTrabajo
        ds = DataSetRequery(ds, " select * from jsnomfecnom where tiponom = " & TipoNomina & " and id_emp = '" & jytsistema.WorkID & "' AND CODNOM = '" & txtNomina.Text & "' order by desde desc limit 1 ", myConn, nTablaNom, lblInfo)
        dtNom = ds.Tables(nTablaNom)
        If dtNom.Rows.Count > 0 Then Fecha = DateAdd(DateInterval.Day, 1, CDate(dtNom.Rows(0).Item("hasta").ToString))
        Select Case TipoNomina
            Case 0 'Diaria
                txtFechaDesde.Text = ft.FormatoFecha(Fecha)
                txtFechaHasta.Text = ft.FormatoFecha(Fecha)
            Case 1 'Semanal
                Dim DiaInicialSemana As Integer = CInt(ParametroPlus(MyConn, Gestion.iRecursosHumanos, "nomparam01")) + 1
                txtFechaDesde.Text = ft.FormatoFecha(PrimerDiaSemana(Fecha, DiaInicialSemana))
                txtFechaHasta.Text = ft.FormatoFecha(UltimoDiaSemana(Fecha, DiaInicialSemana))
            Case 2 'Quincenal
                txtFechaDesde.Text = ft.FormatoFecha(PrimerDiaQuincena(Fecha))
                txtFechaHasta.Text = ft.FormatoFecha(UltimoDiaQuincena(Fecha))
            Case 3 'Mensual
                txtFechaDesde.Text = ft.FormatoFecha(PrimerDiaMes(Fecha))
                txtFechaHasta.Text = ft.FormatoFecha(UltimoDiaMes(Fecha))
            Case 4 'Anual
                txtFechaDesde.Text = ft.FormatoFecha(PrimerDiaAño(Fecha))
                txtFechaHasta.Text = ft.FormatoFecha(UltimoDiaAño(Fecha))
            Case 5 'Eventual
                txtFechaDesde.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
                txtFechaHasta.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        End Select

        dtNom.Dispose()
        dtNom = Nothing

    End Sub

    Private Sub txtNomina_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtNomina.TextChanged
        lblNomina.Text = ft.DevuelveScalarCadena(myConn, " SELECT DESCRIPCION FROM jsnomencnom where CODNOM = '" & txtNomina.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ").ToString
    End Sub

    Private Sub btnNomina_Click(sender As System.Object, e As System.EventArgs) Handles btnNomina.Click
        txtNomina.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codnom codigo, descripcion from jsnomencnom where id_emp = '" & jytsistema.WorkID & "' order by codnom ", "NOMINAS", txtNomina.Text)

        ft.RellenaCombo(aTipoNomina, cmbTipo, ft.DevuelveScalarEntero(myConn, " SELECT TIPONOM FROM jsnomencnom WHERE CODNOM = '" & txtNomina.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "'  "))

    End Sub

End Class