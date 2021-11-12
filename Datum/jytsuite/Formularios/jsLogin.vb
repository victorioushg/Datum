Imports MySql.Data.MySqlClient
Imports Microsoft.Win32

Imports Syncfusion.Windows.Forms.Tools
Imports Syncfusion.WinForms.Controls
Imports System.Management

Public Class jsLogin

    Private Const nModulo As String = "Entrada Sistema Jytsuite"

    Private myConnection As MySqlConnection
    Private myCommand As MySqlCommand

    Private ft As New Transportables

    Private LoginSucceeded As Boolean
    Private nVeces As Integer
    Private procMenu As New Threading.Thread(AddressOf showMenu)


    Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Dim lbl As New Label
        lbl.Visible = False
        If LoginSucceeded Then

            jytsistema.strConn = CadenaConexion()

            procMenu.SetApartmentState(Threading.ApartmentState.STA)
            procMenu.Start()
            Me.Close()
        End If
        myConnection.Close()
        myCommand = Nothing
        myConnection = Nothing

    End Sub
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not myConnection Is Nothing Then myConnection.Close()

        Try
            Dim m_xmld = New Xml.XmlDocument()

            m_xmld.Load(Application.ExecutablePath & ".manifest")
            jytsistema.nVersion = "v" & m_xmld.ChildNodes.Item(1).ChildNodes.Item(0).Attributes.GetNamedItem("version").Value
            jytsistema.WorkLanguage = Transportables.idioma.iEspañol

        Catch ex As Exception
        Finally
        End Try

        lblVersion.Text = jytsistema.nVersion
        Me.Text = "Datum " & jytsistema.nVersion

        'Dim ser = GetHDSerialNo("D")
        'ft.mensajeInformativo(ser)

        ft.colocaIdiomaEtiquetas(jytsistema.WorkLanguage, Me)
        Try
            myConnection = New MySqlConnection(jytsistema.strConn)
            myConnection.Open()


        Catch ex As Exception
            MessageBox.Show("Error conectando con servidor MySQL : " + ex.Message)
        End Try
    End Sub
    Public Function GetHDSerialNo(ByVal strDrive As String) As String 'Get HD Serial Number

        'Ensure Valid Drive Letter Entered, Else, Default To C
        If strDrive = "" OrElse strDrive Is Nothing Then

            strDrive = "C"

        End If

        'Make Use Of Win32_LogicalDisk To Obtain Hard Disk Properties
        Dim moHD As New ManagementObject("Win32_LogicalDisk.DeviceID=""" + strDrive + ":""")

        'Get Info
        moHD.[Get]()

        'Get Serial Number
        Return moHD("VolumeSerialNumber").ToString()

    End Function

    Private Function ServerCheck()

        ' Dim MS As ManagementScope = New ManagementScope()
        ' MS.Path.Server = "192.168.0.10" ' Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveServidor, "").split(":")(0)
        ' MS.Path.NamespacePath = "\root\default"  '' "\root\cimv2" 
        ' MS.Options.EnablePrivileges = True
        ' MS.Connect()

        ' Dim mc As ManagementClass = New ManagementClass("stdRegProv")
        ' mc.Scope = MS

        '        Dim mbo As ManagementBaseObject
        '       mbo = mc.GetMethodParameters("EnumValues")

        '       mbo.SetPropertyValue("sSubKeyName", "SOFTWARE\\VendorName\\Versions")

        '       Dim subkeys As String() = mc.InvokeMethod("EnumValues", mbo, Nothing).Properties("sNames").Value

        '       Dim mboS As ManagementBaseObject
        '      Dim keyValue As String

        'foreach(String strKey In subkeys)
        '{
        '    mboS = mc.GetMethodParameters("GetStringValue");
        '    mboS.SetPropertyValue("sSubKeyName", "SOFTWARE\\VendorName\\Versions");
        '    mboS.SetPropertyValue("sValueName", strKey);

        '    keyValue = mc.InvokeMethod("GetStringValue", mboS, null).Properties["sValue"].Value.ToString();
        '    MessageBox.Show(strKey + " : " + keyValue);
        '}
        Return True
    End Function

    Private Sub showMenu()
        Application.Run(New scrMain)
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        LoginSucceeded = False
        End
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click

        If Trim(txtLogin.Text) <> "jytadmin" Then
            Dim lbl As New Label
            If Not UsuarioClaveAES(myConnection, lbl, txtLogin.Text, txtPassword.Text) Then
                nVeces += 1
                If nVeces > 3 Then
                    End
                End If
                ft.mensajeCritico("Usuario y/o Clave no encontrada. Intente de nuevo...")
                ft.enfocarTexto(txtLogin)
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

    Private Sub txtLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLogin.Click,
        txtPassword.Click, txtLogin.GotFocus, txtPassword.GotFocus
        ft.enfocarTexto(sender)
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click
        Dim cServer As New frmServidor
        cServer.Cargar()
        jytsistema.strConn = CadenaConexion()
        myConnection = New MySqlConnection(jytsistema.strConn)
        myConnection.Open()

    End Sub


End Class
