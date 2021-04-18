Imports MySql.Data.MySqlClient
Public Class jsControlArcCalendario

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable

    Private strSQL As String = ""

    Private nTabla As String = "tblCalendario"

    Private fechaTrabajo As Date
    Private FechaInicioMes As Date
    Private FechaFinMes As Date

    Private DiasMes As Integer

    Public Sub Cargar(MyCon As MySqlConnection)

        fechaTrabajo = jytsistema.sFechadeTrabajo
        FechaInicioMes = PrimerDiaMes(jytsistema.sFechadeTrabajo)
        FechaFinMes = UltimoDiaMes(jytsistema.sFechadeTrabajo)
        DiasMes = FechaFinMes.Day - FechaInicioMes.Day + 1
        myConn = MyCon
        mc.SetDate(fechaTrabajo)
        Me.StartPosition = FormStartPosition.CenterScreen
        IniciarDiasMes()

        Me.Show()
    End Sub
    Private Sub IniciarDiasMes()

        strSQL = "select * from jsconcatper " _
            & " where " _
            & " MES = " & fechaTrabajo.Month & " AND " _
            & " ANO = " & fechaTrabajo.Year & " AND " _
            & " ID_EMP = '" & jytsistema.WorkID & "' " _
            & " order by DIA "


        Dim aCampos() As String = {"dia", "descripcion"}
        Dim aNombres() As String = {"Día", "Descripción"}
        Dim aAnchos() As Long = {30, 150}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {"", ""}

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
                EjecutarSTRSQL(myConn, lblInfo, " delete from jsconcatper where " _
                        & " MES = " & e.Start.Month & " AND " _
                        & " ANO = " & e.Start.Year & " AND " _
                        & " DIA = " & e.Start.Day & " AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "'")
            Else
                EjecutarSTRSQL(myConn, lblInfo, "insert into jsconcatper values ( " _
                       & e.Start.Month & ", " _
                       & e.Start.Year & ", " _
                       & e.Start.Day & ", '" & aDias(e.Start.DayOfWeek) & "', " _
                       & "'" & jytsistema.WorkExercise & "', " _
                       & "'" & jytsistema.WorkID & "'" _
                       & ")")
            End If
        Catch ex As Exception
            MensajeCritico(lblInfo, ex.Message)
        End Try

        IniciarDiasMes()

    End Sub
    Private Function ExisteFechaEnCalendario(fecha As Date) As Boolean
        Return CBool(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(dia) from jsconcatper where ano = " & fecha.Year & " and " _
                              & " mes = " & fecha.Month & " and dia = " & fecha.Day & " and id_emp = '" & jytsistema.WorkID & "'  "))

    End Function

    Private Sub FechaSistema_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MensajeEtiqueta(lblInfo, "Escoja fecha deseada", TipoMensaje.iInfo)
    End Sub

    'Private Sub FechaSistema_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
    '    Dim s As New Size(MonthCalendar1.Width, MonthCalendar1.Height + lblInfo.Height)
    '    Me.ClientSize = s
    'End Sub

    Private Sub mc_DateChanged(sender As System.Object, e As System.Windows.Forms.DateRangeEventArgs) Handles mc.DateChanged
        If e.Start.Month <> FechaInicioMes.Month Then
            FechaInicioMes = PrimerDiaMes(e.Start)
            FechaFinMes = UltimoDiaMes(e.End)
            fechaTrabajo = PrimerDiaMes(e.Start)
            DiasMes = FechaFinMes.Day - FechaInicioMes.Day + 1
            IniciarDiasMes()
        End If
    End Sub

    Private Sub btnOK_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click, btnCancel.Click
        Me.Close()
    End Sub
End Class