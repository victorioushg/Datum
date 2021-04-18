Imports System.Windows.Forms
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Imports System.Globalization
Public Class frmServidor
    Private Const sModulo = "Servidor y Base de datos  "
    Private MyConn As New MySqlConnection
    Private ft As New Transportables

    Private CambiarServidor As Boolean = False
    Private n_Servidor As String
    Private n_BaseDeDatos As String
    Public Property Servidor() As String
        Get
            Return n_Servidor
        End Get
        Set(ByVal value As String)
            n_Servidor = value
        End Set
    End Property

    Public Property BaseDeDatos() As String
        Get
            Return n_BaseDeDatos
        End Get
        Set(ByVal value As String)
            n_BaseDeDatos = value
        End Set
    End Property

    Public Sub Cargar()
        CambiarServidor = True
        Me.ShowDialog()
    End Sub

    Private Function Validado() As Boolean

        If Trim(txtServidor.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un servidor válido...")
            txtServidor.Focus()
            Return False
        End If

        If Trim(txtBaseDatos.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar una Base de datos váilda...")
            txtBaseDatos.Focus()
            Return False
        End If

        jytsistema.strConn = CadenaConexion(txtServidor.Text, txtBaseDatos.Text)

        Try
            MyConn = New MySql.Data.MySqlClient.MySqlConnection(jytsistema.strConn)
            MyConn.Open()
        Catch ex As Exception
            MessageBox.Show("Error conectando con servidor MySQL : " + ex.Message)
            Exit Function
        End Try

        Validado = True

    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Guardar()
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub
    Private Sub Guardar()

        Servidor = txtServidor.Text
        BaseDeDatos = txtBaseDatos.Text
        Registry.SetValue(jytsistema.DirReg, jytsistema.ClaveServidor, txtServidor.Text)
        Registry.SetValue(jytsistema.DirReg, jytsistema.ClaveBaseDatos, txtBaseDatos.Text)

        jytsistema.strConn = CadenaConexion()

    End Sub
    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub frmServidor_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Hide()

        Servidor = Microsoft.Win32.Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveServidor, "")
        BaseDeDatos = Microsoft.Win32.Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveBaseDatos, "")
        Dim aServidor() As String = Servidor.Split(":")

        jytsistema.WorkDataBase = BaseDeDatos

        If jytsistema.strConn <> "" And Not CambiarServidor Then
            If Me.DialogResult = Windows.Forms.DialogResult.Cancel Then End
            jsLogin.ShowDialog()
        Else
            jytsistema.strConn = CadenaConexion()
        End If

        MyConn.Close()
        MyConn = Nothing
        ft = Nothing

    End Sub

    Private Sub frmServidor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        txtServidor.Text = Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveServidor, "")
        txtBaseDatos.Text = Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveBaseDatos, "")

        jytsistema.strConn = CadenaConexion()
        jytsistema.WorkLanguage = IdiomaDatum.iEspañol

        ft.colocaIdiomaEtiquetas(jytsistema.WorkLanguage, Me)

        If Not CambiarServidor Then
            Try
                If txtServidor.Text = "" Or txtBaseDatos.Text = "" Then Exit Sub
                MyConn = New MySqlConnection(jytsistema.strConn)
                MyConn.Open()
                Me.Close()
            Catch ex As Exception
                MessageBox.Show("Error conectando con servidor MySQL : " + ex.Message)
                Exit Sub
            End Try
        End If

    End Sub

End Class
