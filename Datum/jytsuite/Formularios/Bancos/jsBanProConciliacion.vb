Imports MySql.Data.MySqlClient
Public Class jsBanProConciliacion
    Private Const sModulo As String = "Conciliación Bancaria"
    Private Const nTabla As String = "concilia"
    Private Const nTablaSaldos As String = "tblsaldos"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private dtSaldos As New DataTable
    Private ft As New Transportables

    Private InicioEjercicio As Date, CierreEjercicio As Date
    Private EjercicioActual As String
    Private FechaCon As Date
    Private Mes As Integer, Año As Integer
    Private b_Control As Boolean = False

    Public Sub Cargar(ByVal MyCon As MySqlConnection)
        myConn = MyCon
        Me.Tag = sModulo
        IniciarEjercicios(myConn)
        IniciarCuentasEnCombo(myConn, lblInfo, cmbCuenta)
        IniciarMesesEnComboPlus(myConn, cmbMes, cmbEjercicio.Text.Substring(0, 5))

        'If CargaEjercicio() Then CargaConciliacion()

        ft.mensajeEtiqueta(lblInfo, "Seleccione ejercicio, cuenta y mes de conciliación... ", Transportables.TipoMensaje.iAyuda)
        Me.Show()

    End Sub
    Private Sub IniciarEjercicios(ByVal MyCon As MySqlConnection)
        Dim eCont As Integer
        Dim nTablaE As String = "tEjercicio"
        ds = DataSetRequery(ds, " select * from jsconctaeje where id_emp = '" & jytsistema.WorkID & "' order by ejercicio desc ", myConn, nTablaE, lblInfo)
        Dim aEjercicio(ds.Tables(nTablaE).Rows.Count) As String
        aEjercicio(0) = "00000 | Ejercicio Actual"
        With ds.Tables(nTablaE)
            For eCont = 0 To .Rows.Count - 1
                aEjercicio(eCont + 1) = .Rows(eCont).Item("ejercicio") & " | " & _
                    ft.FormatoFecha(CDate(.Rows(eCont).Item("inicio").ToString)) & " | " & ft.FormatoFecha(CDate(.Rows(eCont).Item("cierre").ToString))
            Next
        End With

        ds.Tables(nTablaE).Dispose()

        RellenaCombo(aEjercicio, cmbEjercicio)

    End Sub

    Private Function CargaEjercicio() As Boolean
        CargaEjercicio = False
        If Mid(cmbEjercicio.Text, 1, 5) <> "00000" Then
            InicioEjercicio = ft.DevuelveScalarFecha(myConn, " select inicio from jsconctaeje where ejercicio = '" & cmbEjercicio.Text.Substring(0, 5) & "' and id_emp = '" & jytsistema.WorkID & "'   ")
            CierreEjercicio = ft.DevuelveScalarFecha(myConn, " select cierre from jsconctaeje where ejercicio = '" & cmbEjercicio.Text.Substring(0, 5) & "' and id_emp = '" & jytsistema.WorkID & "'   ")
            EjercicioActual = Mid(cmbEjercicio.Text, 1, 5)
        Else
            InicioEjercicio = ft.DevuelveScalarFecha(myConn, " select inicio from jsconctaemp where id_emp = '" & jytsistema.WorkID & "'   ")
            CierreEjercicio = ft.DevuelveScalarFecha(myConn, " select cierre from jsconctaemp where id_emp = '" & jytsistema.WorkID & "'   ")
            EjercicioActual = ""
        End If
        CargaAño()
        CargaEjercicio = True
    End Function
    Private Sub CargaAño()
        'lblAño.Text = cmbMes.SelectedItem.Split("-")(1)
    End Sub
    Private Sub CargaConciliacion()
        Dim sfecha As String
        If cmbCuenta.Text <> "" And cmbMes.Text <> "" Then
            Mes = ft.InArray(aMesesReal, cmbMes.SelectedItem.Split("-")(0)) + 1
            If Mes >= 1 And Mes <= 12 Then

                Año = cmbMes.SelectedItem.Split("-")(1)

                sfecha = CDate("01/" & Format(Mes, "00") & "/" & Year(CierreEjercicio)).ToString

                If cmbCuenta.Text <> "" AndAlso IsDate(sfecha) Then

                    FechaCon = CDate("01/" & Format(Mes, "00") & "/" & Format(Año, "0000"))
                    ds = DataSetRequery(ds, " select fechamov, tipomov, numdoc, concepto, importe, origen, conciliado from jsbantraban where conciliado = 0 and " _
                        & " fechamov <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(FechaCon)) & "'  and " _
                        & " codban = '" & Mid(cmbCuenta.Text, 1, 5) & "' and id_emp = '" & jytsistema.WorkID & "' " _
                        & " Union " _
                        & " select fechamov, tipomov, numdoc, concepto, importe, origen, if( mesconcilia > '" & ft.FormatoFechaMySQL(UltimoDiaMes(FechaCon)) & "', 0, conciliado) conciliado from jsbantraban where " _
                        & " conciliado = 1 and " _
                        & " fechamov >= '" & ft.FormatoFechaMySQL(PrimerDiaMes(FechaCon)) & "' and " _
                        & " fechamov <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(FechaCon)) & "' and " _
                        & " codban = '" & Mid(cmbCuenta.Text, 1, 5) & "' and " _
                        & " id_emp = '" & jytsistema.WorkID & "' " _
                        & " union " _
                        & " select fechamov, tipomov, numdoc, concepto, importe, origen, if( mesconcilia > '" & ft.FormatoFechaMySQL(UltimoDiaMes(FechaCon)) & "', 0, conciliado) conciliado from jsbantraban where " _
                        & " conciliado = 1 and " _
                        & " mesconcilia >= '" & ft.FormatoFechaMySQL(PrimerDiaMes(FechaCon)) & "' and " _
                        & " mesconcilia <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(FechaCon)) & "' and " _
                        & " fechamov < '" & ft.FormatoFechaMySQL(PrimerDiaMes(FechaCon)) & "' and " _
                        & " codban = '" & Mid(cmbCuenta.Text, 1, 5) & "' and " _
                        & " id_emp = '" & jytsistema.WorkID & "' " _
                        & " order  by fechamov, numdoc ", myConn, nTabla, lblInfo)

                    dt = ds.Tables(nTabla)

                    CargaListViewDesdeTraBan(lv, dt)
                    SaldoMesAnterior(FechaCon)
                    ' ConsolidadoXConsolidar(lv)

                End If
            End If
            ConsolidadoXConsolidar()
        Else
            lv.Items.Clear()
        End If

    End Sub
    Private Sub SaldoMesAnterior(ByVal MesConciliacion As Date)

        txtCRMesAnt.Text = ft.FormatoNumero(CalculaSaldoBanco(myConn, lblInfo, Mid(cmbCuenta.Text, 1, 5), True, DateAdd(DateInterval.Day, -1, MesConciliacion), 1))
        txtDBMesAnt.Text = ft.FormatoNumero(CalculaSaldoBanco(myConn, lblInfo, Mid(cmbCuenta.Text, 1, 5), True, DateAdd(DateInterval.Day, -1, MesConciliacion), 0))
        txtSalMesAnt.Text = ft.FormatoNumero(CalculaSaldoBanco(myConn, lblInfo, Mid(cmbCuenta.Text, 1, 5), True, DateAdd(DateInterval.Day, -1, MesConciliacion), 2))

        txtCRMesAct.Text = ft.FormatoNumero(CalculaSaldoBanco(myConn, lblInfo, Mid(cmbCuenta.Text, 1, 5), True, UltimoDiaMes(MesConciliacion), 1) - ValorNumero(txtCRMesAnt.Text))
        txtDBMesAct.Text = ft.FormatoNumero(CalculaSaldoBanco(myConn, lblInfo, Mid(cmbCuenta.Text, 1, 5), True, UltimoDiaMes(MesConciliacion), 0) - ValorNumero(txtDBMesAnt.Text))
        txtSalMesAct.Text = ft.FormatoNumero(CalculaSaldoBanco(myConn, lblInfo, Mid(cmbCuenta.Text, 1, 5), True, UltimoDiaMes(MesConciliacion), 2) - ValorNumero(txtSalMesAnt.Text))

    End Sub

    Private Sub ConsolidadoXConsolidar()
        Dim kCont As Integer

        Dim sPositivoCon As Double = 0.0
        Dim sNegativoCon As Double = 0.0
        Dim sPositivoXCon As Double = 0.0
        Dim sNegativoXCon As Double = 0.0
        Dim sValor As Double


        Try
            For kCont = 0 To lv.Items.Count - 1
                sValor = ValorNumero(lv.Items(kCont).SubItems(4).Text)
                If lv.Items(kCont).Checked Then 'CONCILIADO
                    If sValor > 0 Then
                        sPositivoCon += sValor
                    Else
                        sNegativoCon += sValor
                    End If
                Else ' NO CONCILIADO
                    If sValor > 0 Then
                        sPositivoXCon += sValor
                    Else
                        sNegativoXCon += sValor
                    End If
                End If
            Next

            txtCRC.Text = ft.FormatoNumero(sPositivoCon)
            txtDBC.Text = ft.FormatoNumero(Math.Abs(sNegativoCon))
            txtSalC.Text = ft.FormatoNumero(sPositivoCon - Math.Abs(sNegativoCon))

            txtCRNC.Text = ft.FormatoNumero(sPositivoXCon)
            txtDBNC.Text = ft.FormatoNumero(Math.Abs(sNegativoXCon))

            txtSalNC.Text = ft.FormatoNumero(sPositivoXCon - Math.Abs(sNegativoXCon))


        Catch ex As Exception

        End Try

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub lv_ChangeUICues(ByVal sender As Object, ByVal e As System.Windows.Forms.UICuesEventArgs) Handles lv.ChangeUICues

    End Sub

    Private Sub lv_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lv.ItemCheck

    End Sub

    Private Sub lv_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lv.ItemChecked
        ConsolidadoXConsolidar()

    End Sub
    Private Sub jsBanProConciliacion_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt.Dispose()
        dtSaldos.Dispose()
    End Sub

    Private Sub jsBanProConciliacion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        GuardarRemesa()
        ImprimirConciliacion()

    End Sub
    Private Sub GuardarRemesa()
        InsertarAuditoria(myConn, MovAud.iIncluir, sModulo, txtSalC.Text)
        ActualizaConsolidados()
    End Sub
    Private Sub ImprimirConciliacion()
        '
        Dim resp As Integer
        resp = MsgBox(" ¿Desea imprimir la conciliación? ", MsgBoxStyle.YesNo, sModulo)
        If resp = MsgBoxResult.Yes Then
            Dim f As New jsBanRepParametros
            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cConciliacion, "Conciliación Bancaria", Mid(cmbCuenta.Text, 1, 5))
            f = Nothing
        End If

    End Sub
    Private Sub ActualizaConsolidados()

        Dim sFec As Date
        Dim sTip As String
        Dim sNum As String
        Dim sCon As String
        Dim sImp As Double
        Dim sOrg As String
        Dim iCont As Integer


        ft.Ejecutar_strSQL(myconn, "DELETE FROM jsbantracon " _
            & " where " _
            & " MESCONCILIA = '" & ft.FormatoFechaMySQL(CDate("01/" & CStr(Mes) & "/" & CStr(Año))) & "' AND " _
            & " CODBAN = '" & Mid(cmbCuenta.Text, 1, 5) & "' AND " _
            & " ID_EMP = '" & jytsistema.WorkID & "' ")

        For iCont = 0 To lv.Items.Count - 1

            sFec = CDate(lv.Items(iCont).Text)
            sTip = lv.Items(iCont).SubItems(1).Text
            sNum = lv.Items(iCont).SubItems(2).Text
            sCon = lv.Items(iCont).SubItems(3).Text
            sImp = ValorNumero(lv.Items(iCont).SubItems(4).Text)
            sOrg = lv.Items(iCont).SubItems(5).Text

            If lv.Items(iCont).Checked Then
                ' ACTUALIZA MOVIMIENTOS CONCILIADOS
                ft.Ejecutar_strSQL(myconn, "UPDATE jsbantraban SET " _
                    & " CONCILIADO = '1' ," _
                    & " MESCONCILIA = '" & ft.FormatoFechaMySQL(CDate("01/" & CStr(Mes) & "/" & CStr(Año))) & "', " _
                    & " FECCONCILIA = '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' WHERE " _
                    & " FECHAMOV = '" & ft.FormatoFechaMySQL(sFec) & "' AND " _
                    & " TIPOMOV = '" & sTip & "' AND " _
                    & " NUMDOC = '" & sNum & "' AND " _
                    & " CONCEPTO = '" & sCon & "' AND " _
                    & " ORIGEN = '" & sOrg & "' and " _
                    & " CODBAN = '" & Mid(cmbCuenta.Text, 1, 5) & "' AND " _
                    & " " _
                    & " ID_EMP = '" & jytsistema.WorkID & "' ")

                ft.Ejecutar_strSQL(myconn, "INSERT jsbantracon VALUES (" _
                    & " '" & ft.FormatoFechaMySQL(sFec) & "', " _
                    & " '" & sNum & "', " _
                    & " '" & sTip & "', " _
                    & " '" & sCon & "', " _
                    & " " & sImp & ", " _
                    & " '1' ," _
                    & " '" & sOrg & "', " _
                    & " '" & Mid(cmbCuenta.Text, 1, 5) & "', " _
                    & " '" & ft.FormatoFechaMySQL(CDate("01/" & CStr(Mes) & "/" & CStr(Año))) & "', " _
                    & " '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', " _
                    & " '', " _
                    & " '" & jytsistema.WorkID & "')")

            Else
                ft.Ejecutar_strSQL(myconn, "UPDATE jsbantraban SET " _
                    & " CONCILIADO = '0' ," _
                    & " MESCONCILIA = '1963-05-07', " _
                    & " FECCONCILIA = '1963-05-07' WHERE " _
                    & " FECHAMOV = '" & ft.FormatoFechaMySQL(sFec) & "' AND " _
                    & " TIPOMOV = '" & sTip & "' AND " _
                    & " NUMDOC = '" & sNum & "' AND " _
                    & " CONCEPTO = '" & sCon & "' AND " _
                    & " ORIGEN = '" & sOrg & "' and " _
                    & " CODBAN = '" & Mid(cmbCuenta.Text, 1, 5) & "' AND " _
                    & " ID_EMP = '" & jytsistema.WorkID & "' ")
            End If
        Next

    End Sub


    Private Sub cmbEjercicio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbEjercicio.SelectedIndexChanged, _
        cmbCuenta.SelectedIndexChanged, cmbMes.SelectedIndexChanged
        'If cmbMes.Items.Count > 1 Then lblAño.Text = cmbMes.SelectedText.Split("-")(1)
        If CargaEjercicio() Then CargaConciliacion()
    End Sub

    Private Sub txtSalMesAnt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSalMesAnt.TextChanged, _
        txtSalMesAct.TextChanged
        txtSaldoEnLibros.Text = ft.FormatoNumero(ValorNumero(txtSalMesAnt.Text) + ValorNumero(txtSalMesAct.Text))
    End Sub

    Private Sub txtSalNC_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        txtSaldoEnLibros.TextChanged, txtSalNC.TextChanged
        txtSaldoEnBancos.Text = ft.FormatoNumero(ft.FormatoNumero(ValorNumero(txtSaldoEnLibros.Text) - ValorNumero(txtSalNC.Text)))

    End Sub
End Class