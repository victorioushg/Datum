Public Class frmViewer

    Private Sub frmViewer_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Dock = DockStyle.None
        Me.WindowState = FormWindowState.Normal
    End Sub

    Private Sub frmViewer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, Me.Text)
        End Try

    End Sub
    Public Sub Cargar(ByVal Nombre As String)
        With Me
            .Text = Nombre
            .Dock = DockStyle.Fill
            .WindowState = FormWindowState.Maximized
            .CrystalReportViewer1.ShowRefreshButton = False
            .ShowDialog()
        End With


    End Sub

End Class