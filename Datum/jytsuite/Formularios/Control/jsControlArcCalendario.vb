Imports MySql.Data.MySqlClient
Public Class jsControlArcCalendario

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private strSQL As String = ""

    Private nTabla As String = "tblCalendario"

    Private fechaTrabajo As Date
    Private FechaInicioMes As Date
    Private FechaFinMes As Date

    Private DiasMes As Integer

    Private aTipoCalendario() As String = {"NOMINA", "VENTAS Y CXC"}

    Public Sub Cargar(MyCon As MySqlConnection)



        fechaTrabajo = jytsistema.sFechadeTrabajo
        FechaInicioMes = PrimerDiaMes(jytsistema.sFechadeTrabajo)
        FechaFinMes = UltimoDiaMes(jytsistema.sFechadeTrabajo)
        DiasMes = FechaFinMes.Day - FechaInicioMes.Day + 1
        myConn = MyCon
        mc.SetDate(fechaTrabajo)

        ft.RellenaCombo(aTipoCalendario, cmbTipoCalendario)

        Me.StartPosition = FormStartPosition.CenterScreen
        IniciarDiasMes()

        Me.Show()
    End Sub
    Private Sub IniciarDiasMes()

        strSQL = " SELECT * FROM jsconcatper " _
            & " WHERE " _
            & " MODULO = " & cmbTipoCalendario.SelectedIndex & " and " _
            & " MES = " & fechaTrabajo.Month & " AND " _
            & " ANO = " & fechaTrabajo.Year & " AND " _
            & " ID_EMP = '" & jytsistema.WorkID & "' " _
            & " ORDER BY DIA "


        Dim aCampos() As String = {"dia", "descripcion", "Tipo"}
        Dim aNombres() As String = {"Día", "Descripción", "Feriado"}
        Dim aAnchos() As Integer = {30, 150, 30}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro}
        Dim aFormatos() As String = {"", "", ""}

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        ColocaDiasEnNegritas()

        lbl.Text = "Dias Hábiles = " & DiasMes - dt.Rows.Count & " -- Días no Hábiles = " & dt.Rows.Count

    End Sub

    Private Sub ColocaDiasEnNegritas()
        mc.RemoveAllBoldedDates()
        For Each dr As DataRow In dt.Rows
            mc.AddBoldedDate(CDate(dr.Item("ANO").ToString & "-" & Format(dr.Item("MES"), "00") & "-" & Format(dr.Item("DIA"), "00")))
        Next
        mc.UpdateBoldedDates()
    End Sub

    Private Sub MonthCalendar1_DateSelected(ByVal sender As Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles mc.DateSelected

        Try
            If ExisteFechaEnCalendario(e.Start) Then
                ft.Ejecutar_strSQL(myConn, " delete from jsconcatper where " _
                                   & " modulo =  " & cmbTipoCalendario.SelectedIndex & " and " _
                        & " MES = " & e.Start.Month & " AND " _
                        & " ANO = " & e.Start.Year & " AND " _
                        & " DIA = " & e.Start.Day & " AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "'")
            Else
                ft.Ejecutar_strSQL(myConn, "insert into jsconcatper values ( " _
                       & e.Start.Month & ", " _
                       & e.Start.Year & ", " _
                       & e.Start.Day & ", '" & aDias(e.Start.DayOfWeek) & "', 0, " _
                       & cmbTipoCalendario.SelectedIndex & ", " _
                       & "'" & jytsistema.WorkExercise & "', " _
                       & "'" & jytsistema.WorkID & "'" _
                       & ")")
            End If
        Catch ex As Exception
            ft.MensajeCritico(ex.Message)
        End Try

        IniciarDiasMes()

    End Sub
    Private Function ExisteFechaEnCalendario(fecha As Date) As Boolean
        Return ft.DevuelveScalarBooleano(myConn, " select count(dia) from jsconcatper where " _
                                         & " modulo =  " & cmbTipoCalendario.SelectedIndex & " AND " _
                                         & " ano = " & fecha.Year & " and " _
                                         & " mes = " & fecha.Month & " and " _
                                         & " dia = " & fecha.Day & " and " _
                                         & " id_emp = '" & jytsistema.WorkID & "'  ")

    End Function

    Private Sub jsControlArcCalendario_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub FechaSistema_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ft.mensajeEtiqueta(lblInfo, "Escoja fecha deseada", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub mc_DateChanged(sender As System.Object, e As System.Windows.Forms.DateRangeEventArgs) Handles mc.DateChanged
        If e.Start.Month <> FechaInicioMes.Month Then
            FechaInicioMes = PrimerDiaMes(e.Start)
            FechaFinMes = UltimoDiaMes(e.End)
            fechaTrabajo = PrimerDiaMes(e.Start)
            DiasMes = FechaFinMes.Day - FechaInicioMes.Day + 1
            IniciarDiasMes()
        End If
    End Sub

    Private Sub btnOK_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        Guardar()
        Me.Close()
    End Sub
    Private Sub Guardar()
        For Each nRow As DataRow In dt.Rows
            ft.Ejecutar_strSQL(myConn, " update jsconcatper " _
                               & " SET TIPO = " & nRow.Item("TIPO") & " " _
                               & " WHERE " _
                               & " MES = " & nRow.Item("MES") & " AND " _
                               & " ANO = " & nRow.Item("ANO") & " AND " _
                               & " DIA = " & nRow.Item("DIA") & " AND " _
                               & " MODULO = " & cmbTipoCalendario.SelectedIndex & " AND " _
                               & " ID_EMP = '" & jytsistema.WorkID & "' ")
        Next
    End Sub
    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub dg_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg.CellContentDoubleClick
        If e.ColumnIndex = 2 Then _
            dt.Rows(e.RowIndex).Item("TIPO") = Not CBool(dt.Rows(e.RowIndex).Item("TIPO").ToString)

    End Sub

    Private Sub dg_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dg.CellFormatting
        Dim aValor() As String = {"No", "Si"}
        If e.ColumnIndex = 2 Then e.Value = aValor(e.Value)
    End Sub

    Private Sub cmbTipoCalendario_MouseClick(sender As Object, e As MouseEventArgs) Handles cmbTipoCalendario.MouseClick
        Guardar()
    End Sub

    Private Sub cmbTipoCalendario_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTipoCalendario.SelectedIndexChanged
        IniciarDiasMes()
    End Sub
End Class