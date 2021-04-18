Public Class FechaSistema

    Private n_Fecha As Date
    Private SeleccionaFechaAnterior As Boolean
    Private UltimaFecha As Date

    Private ft As New Transportables
    Public Property Fecha() As Date
        Get
            Return n_Fecha
        End Get
        Set(ByVal value As Date)
            n_Fecha = value
        End Set
    End Property
    Public Sub Cargar(ByVal nLeft As Integer, ByVal nTop As Integer)

        MonthCalendar1.SetDate(Fecha)

        Dim rPoint As New Point(nLeft, nTop)
        If nLeft + nTop = 0 Then
            Me.StartPosition = FormStartPosition.CenterScreen
        Else
            Me.StartPosition = FormStartPosition.Manual
            Me.Location = PointToClient(rPoint)
        End If

        Me.ShowDialog()

    End Sub

    Private Sub MonthCalendar1_DateSelected(ByVal sender As Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles MonthCalendar1.DateSelected

        Fecha = e.Start.Date
        Me.Close()

    End Sub

    Private Sub FechaSistema_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ft.mensajeEtiqueta(lblInfo, "Escoja fecha deseada", Transportables.TipoMensaje.iInfo)

    End Sub

    Private Sub FechaSistema_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Dim s As New Size(MonthCalendar1.Width, MonthCalendar1.Height + lblInfo.Height)
        Me.ClientSize = s
    End Sub
End Class