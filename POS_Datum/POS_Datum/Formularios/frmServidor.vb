Imports System.Windows.Forms
Imports Microsoft.Win32
Imports MySql.Data.MySqlClient
Public Class frmServidor
    Private Const sModulo = "Servidor y Base de datos  "
    Private MyConn As New MySqlConnection
    Private ft As New Transportables

    Private CambiarServidor As Boolean = False
    Private n_Servidor As String
    Private n_User As String
    Private n_Password As String
    Private n_Port As String
    Private n_BaseDeDatos As String
    Private n_Caja As String
    Private n_Empresa As String
    Public Property Servidor() As String
        Get
            Return n_Servidor
        End Get
        Set(ByVal value As String)
            n_Servidor = value
        End Set
    End Property
    Public Property User() As String
        Get
            Return n_User
        End Get
        Set(ByVal value As String)
            n_User = value
        End Set
    End Property
    Public Property Password() As String
        Get
            Return n_Password
        End Get
        Set(ByVal value As String)
            n_Password = value
        End Set
    End Property
    Public Property Port() As String
        Get
            Return n_Port
        End Get
        Set(ByVal value As String)
            n_Port = value
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
    Public Property Caja() As String
        Get
            Return n_Caja
        End Get
        Set(ByVal value As String)
            n_Caja = value
        End Set
    End Property
    Public Property Empresa() As String
        Get
            Return n_Empresa
        End Get
        Set(ByVal value As String)
            n_Empresa = value
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
        If Trim(txtCaja.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar una caja válida...")
            txtCaja.Focus()
            Return False
        End If
        If Trim(txtEmpresa.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar una empresa de trabajo válida...")
            txtEmpresa.Focus()
            Return False
        End If


        jytsistema.strConn = CadenaConexion(txtServidor.Text, txtUser.Text, txtPassword.Text, txtPort.Text, txtBaseDatos.Text)

        Try
            MyConn = New MySqlConnection(jytsistema.strConn)
            MyConn.Open()

            If ft.DevuelveScalarEntero(MyConn, " select count(*) from jsconctaemp where id_emp = '" & txtEmpresa.Text & "' ") = 0 Then
                ft.mensajeCritico("CODIGO DE EMPRESA NO EXISTE")
                ft.enfocarTexto(txtEmpresa)
                Return False
            End If

            If ft.DevuelveScalarEntero(MyConn, " select count(*) from jsvencatcaj where codcaj = '" & txtCaja.Text & "' and id_emp = '" & txtEmpresa.Text & "' ") = 0 Then
                ft.mensajeCritico("CODIGO DE CAJA (POS) NO EXISTE")
                ft.enfocarTexto(txtCaja)
                Return False
            End If

        Catch ex As Exception
            MessageBox.Show("Error conectando con servidor MySQL : " + ex.Message)
            Return False
        End Try
        Return True

    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            CerrarFormulario()
        End If
    End Sub
    Private Sub CerrarFormulario()
        Dim signIn = New dtLogin

        If Me.DialogResult = System.Windows.Forms.DialogResult.OK Then
            Guardar()
            signIn.ShowDialog()
        End If
        Me.Close()
    End Sub
    Private Sub Guardar()

        Servidor = txtServidor.Text
        BaseDeDatos = txtBaseDatos.Text
        Caja = txtCaja.Text
        Empresa = txtEmpresa.Text
        Registry.SetValue(jytsistema.DirReg, jytsistema.ClaveServidor, txtServidor.Text)
        Registry.SetValue(jytsistema.DirReg, jytsistema.ClaveUsuario, txtUser.Text)
        Registry.SetValue(jytsistema.DirReg, jytsistema.ClavePassword, txtPassword.Text)
        Registry.SetValue(jytsistema.DirReg, jytsistema.ClavePort, txtPort.Text)
        Registry.SetValue(jytsistema.DirReg, jytsistema.ClaveBaseDatos, txtBaseDatos.Text)
        Registry.SetValue(jytsistema.DirReg, jytsistema.ClaveCaja, txtCaja.Text)
        Registry.SetValue(jytsistema.DirReg, jytsistema.ClaveEmpresa, txtEmpresa.Text)

        jytsistema.WorkDataBase = BaseDeDatos
        jytsistema.WorkBox = Caja
        jytsistema.WorkID = Empresa

    End Sub
    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        CerrarFormulario()
    End Sub
    Private Sub frmServidor_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Hide()

        ft = Nothing
        MyConn.Close()
        MyConn = Nothing
    End Sub

    Private Sub frmServidor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        txtServidor.Text = Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveServidor, "")
        txtUser.Text = Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveUsuario, "")
        txtPassword.Text = Registry.GetValue(jytsistema.DirReg, jytsistema.ClavePassword, "")
        txtPort.Text = Registry.GetValue(jytsistema.DirReg, jytsistema.ClavePort, "")
        txtBaseDatos.Text = Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveBaseDatos, "")
        txtCaja.Text = Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveCaja, "")
        txtEmpresa.Text = Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveEmpresa, "")


        If Not CambiarServidor Then
            If Validado() Then
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                CerrarFormulario()
            End If
        End If

    End Sub

End Class
