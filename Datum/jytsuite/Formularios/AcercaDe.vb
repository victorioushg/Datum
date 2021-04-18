Imports System
Imports System.Diagnostics
Imports System.ComponentModel

Public NotInheritable Class AcercaDe

    Private ft As New Transportables

    Private Sub AcercaDe_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub AcercaDe_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set the title of the form.
        Dim ApplicationTitle As String


        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format("Acerca de {0}", ApplicationTitle)
        ' Initialize all of the text displayed on the About Box.
        Me.LabelProductName.Text = My.Application.Info.ProductName
        Me.LabelVersion.Text = String.Format("Versión {0}", jytsistema.nVersion)
        Me.LabelCopyright.Text = My.Application.Info.Copyright
        Me.LabelCompanyName.Text = My.Application.Info.CompanyName
        Me.txtVersiones.LoadFile(System.Environment.CurrentDirectory + "\" + "HistoricoDatumVersionesV2.rtf", RichTextBoxStreamType.RichText)
    End Sub

   


    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

    Private Sub btnSoporte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSoporte.Click
        Dim Proceso As New Process()
        Try
            Proceso.StartInfo.FileName = Application.StartupPath & "\TeamViewerQS_es.EXE"
            Proceso.Start()
        Catch ex As Win32Exception
            ft.mensajeInformativo(ex.Message)
        End Try

    End Sub

    Private Sub TableLayoutPanel_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles TableLayoutPanel.Paint

    End Sub
End Class
