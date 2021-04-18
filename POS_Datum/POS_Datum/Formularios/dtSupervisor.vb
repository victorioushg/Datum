Imports MySql.Data.MySqlClient
Public Class dtSupervisor
    Private Const sModulo As String = " Control de Supervisor "

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private tblSupervisor As String = "supervisor"

    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet)
        MyConn = MyCon
        dsLocal = ds
        txtCodigo.Text = ""
        Me.ShowDialog()
    End Sub

    Private Sub jsSupervisor_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsSupervisor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique clave supervisor ...", Transportables.tipoMensaje.iAyuda)
        ft.enfocarTexto(sender)
    End Sub
    Private Function Validado() As Boolean

        Dim aFld() As String = {"clave", "id_emp"}
        Dim aStr() As String = {txtCodigo.Text, jytsistema.WorkID}

        Validado = False

        If Not qFound(MyConn, lblInfo, "jsvencatsup", aFld, aStr) Then
            ft.mensajeAdvertencia("Clave supervisor no encontrada por favor verifique...")
            txtCodigo.Focus()
            Exit Function
        End If

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, "")
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub txtCodigo_TextChanged(sender As Object, e As EventArgs) Handles txtCodigo.TextChanged

    End Sub
End Class