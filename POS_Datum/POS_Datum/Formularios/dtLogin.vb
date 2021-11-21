Imports MySql.Data.MySqlClient
Imports System.Threading
Public Class dtLogin

    Private Const nModulo As String = "Carga sistema POS Datum"

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
            If txtCajero.Text = "jytsuite" Then VerificaBaseDatos(myConnection, jytsistema.WorkDataBase, lbl)
            jytsistema.sFechadeTrabajo = CDate(txtFecha.Text)

            procMenu.SetApartmentState(Threading.ApartmentState.STA)
            procMenu.Start()
            Me.Close() 'Cerrara la ventana de Login
        End If
        myConnection.Close()
        myCommand = Nothing
        myConnection = Nothing


    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not myConnection Is Nothing Then myConnection.Close()

        Dim m_xmld = New Xml.XmlDocument()

        m_xmld.Load(Application.ExecutablePath & ".manifest")
        jytsistema.nVersion = "v" & m_xmld.ChildNodes.Item(1).ChildNodes.Item(0).Attributes.GetNamedItem("version").Value

        lblVersion.Text = "Versión : " + jytsistema.nVersion
        txtFecha.Text = ft.muestraCampoFecha(jytsistema.sFechadeTrabajo)

        Me.Text = "POS Datum " & jytsistema.nVersion
        Try
            myConnection = New MySql.Data.MySqlClient.MySqlConnection(jytsistema.strConn)
            myConnection.Open()
        Catch ex As Exception
            MessageBox.Show("Error conectando con servidor MySQL : " + ex.Message)
        End Try


    End Sub
    Private Sub showMenu()
        Application.Run(New scrMainPlus)
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btnCancelar.Click
        LoginSucceeded = False
        End
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btnAceptar.Click
        Dim lblInfo As New Label
        lblInfo.Visible = False

        If Trim(txtCajero.Text) <> "jytadmin" Then
            Dim aCampoCajero() As String = {"clave", "tipo", "id_emp"}
            Dim aStringCajero() As String = {txtCajero.Text, TipoVendedor.iCajeros, jytsistema.WorkID}
            If Not qFound(myConnection, lblInfo, "jsvencatven", aCampoCajero, aStringCajero) Then
                MessageBox.Show("clave Cajero no encontrada. Intente de nuevo...", nModulo)
                txtCajero.Focus()
            Else
                jytsistema.sUsuario = qFoundAndSign(myConnection, lblInfo, "jsvencatven", aCampoCajero, aStringCajero, "codven") 'txtCajero.Text
                jytsistema.sNombreUsuario = qFoundAndSign(myConnection, lblInfo, "jsvencatven", aCampoCajero, aStringCajero, "apellidos")
                Dim aCamposSup() As String = {"clave", "id_emp"}
                Dim aStringsSup() As String = {txtSupervisor.Text, jytsistema.WorkID}
                If Not qFound(myConnection, lblInfo, "jsvencatsup", aCamposSup, aStringsSup) Then
                    MessageBox.Show("clave Supervisor no encontrada. Intente de nuevo...", nModulo)
                    txtCajero.Focus()
                Else
                    jytsistema.sFechadeTrabajo = CDate(txtFecha.Text)
                    LoginSucceeded = True
                    Me.Close()
                End If
                If LoginSucceeded Then

                    InsertarInicioCierrePV(myConnection, lblInfo, jytsistema.sFechadeTrabajo, jytsistema.sUsuario, _
                    qFoundAndSign(myConnection, lblInfo, "jsvencatsup", aCamposSup, aStringsSup, "codigo"), _
                    0.0, Format(Now, "HH:mm:ss"), "00:00:00", jytsistema.WorkBox, 0)

                End If
            End If
        Else
            jytsistema.sUsuario = "00000"
            jytsistema.sNombreUsuario = "Administrador de Sistema"
            LoginSucceeded = True
            Me.Close()
        End If

    End Sub

    Private Sub txtLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCajero.Click
        txtCajero.Focus()
    End Sub

    Private Sub lblLeyenda_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblLeyenda.Click
        Dim cServer As New frmServidor
        cServer.Cargar()
    End Sub

    Private Sub btnFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFecha.Click
        ' txtFecha.Text = ft.muestraCampoFecha(SeleccionaFecha(CDate(txtFecha.Text), Me, btnFecha))
        'txtFecha.Text = ft.NumeroAleatorio(10000000)
    End Sub

    Private Sub txtCajero_GotFocus(sender As Object, e As EventArgs) Handles txtCajero.GotFocus, _
        txtSupervisor.GotFocus
        ft.enfocarTexto(sender)
    End Sub

    Private Sub txtCajero_TextChanged(sender As Object, e As EventArgs) Handles txtCajero.TextChanged

    End Sub
End Class
