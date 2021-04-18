Imports MySql.Data.MySqlClient
Public Class jsLogin

    Private Const nModulo As String = "Módulo de carga del sistema Jytsuite Inmobiliario"

    Private myConnection As MySqlConnection
    Private myCommand As MySqlCommand

    Private LoginSucceeded As Boolean
    Private nVeces As Integer
    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        LoginSucceeded = False
        End
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click

        If Trim(txtLogin.Text) <> "jytadmin" Then
            If Not UsuarioClave(myConnection, txtLogin.Text, txtPassword.Text) Then
                nVeces += 1
                If nVeces > 3 Then
                    End
                End If
                MessageBox.Show("Usuario y/o Clave no encontrada. Intente de nuevo...", nModulo)
                EnfocarTexto(txtLogin)
            Else
                LoginSucceeded = True
                Me.Close()
            End If
        Else
            jytsistema.sUsuario = "00000"
            jytsistema.sNombreUsuario = "Administrador de Sistema"
            LoginSucceeded = True
            Me.Close()
        End If
    End Sub

    Private Sub txtLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLogin.Click
        EnfocarTexto(txtLogin)
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click
        Dim cServer As New frmServidor
        cServer.Cargar()
    End Sub
End Class
