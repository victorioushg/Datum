Public Class FechaSistemaPlus

    Private ft As New Transportables

    Private n_Fecha As Date
    Private SeleccionaFechaAnterior As Boolean
    Private UltimaFecha As Date
    Public Property Fecha() As Date
        Get
            Return n_Fecha
        End Get
        Set(ByVal value As Date)
            n_Fecha = value
        End Set
    End Property
    Public Sub Cargar(FechaFinal As Date, PermiteSeleccionarFechaAnterior As Boolean, ByVal nLeft As Integer, ByVal nTop As Integer)

        MonthCalendar1.SetDate(Fecha)

        SeleccionaFechaAnterior = PermiteSeleccionarFechaAnterior
        UltimaFecha = FechaFinal

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

        If Not SeleccionaFechaAnterior Then
            If e.Start.Date < Fecha Then
                ft.mensajeCritico("NO SE PERMITE SELECCIONAR UNA FECHA MENOR A LA INICIAL...")
                Return
            End If
        End If

        If ft.FormatoFechaMySQL(UltimaFecha) <> ft.FormatoFechaMySQL(Fecha) Then
            If e.Start.Date > UltimaFecha Then
                ft.mensajeCritico("NO SE PUEDE SELECCIONAR UNA FECHA MAYOR A " + ft.FormatoFecha(UltimaFecha))
                Return
            End If
        End If

        Fecha = e.Start.Date

        Me.Close()

    End Sub

    Private Sub FechaSistemaPlus_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub FechaSistema_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ft.mensajeEtiqueta(lblInfo, "Escoja fecha deseada", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub FechaSistema_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Dim s As New Size(MonthCalendar1.Width, MonthCalendar1.Height + lblInfo.Height)
        Me.ClientSize = s
    End Sub
End Class