Imports MySql.Data.MySqlClient
Public Class frmServidorValidacion

    Private Const sModulo As String = "Validacion Servidor"

    Private MyConn As New MySqlConnection
    Private dsLocal As New DataSet
    Private dtLocal As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private strPort As String = "port = 3306; "
    Private licence_servers() As String = {"datum2online.ddns.net", _
                                           "alimica.systes.net", _
                                           "casiano.systes.net"}

    Private strRemoteConection As String = "server=datum2online.ddns.net " _
                + "; database=jytsuitejytsuite;" _
                + strPort _
                + jytsistema.strCon

    Private aTipoAplicacion() As String = {"Basica", _
                                           "Normal", _
                                           "Plus", _
                                           "Plus Producción", _
                                           "Plus Restaturant", _
                                           "Plus Prensa", _
                                           "Plus Farmacia", _
                                           "Plus Clínicas", _
                                           "Plus Hotel/Spa"}

    Private aLicencia() As String = {"Inactiva", "Activa"}
    Private nEncryptWord As String = ""
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        MyConn = MyCon

        nEncryptWord = ft.getHardDrivesSerials()(0)

        ft.habilitarObjetos(False, True, txtCodigoRemoto, txtNombreRemoto, cmbaplicacionRemoto, cmbLicenciaRemoto, _
                            txtNumLicenciaRemoto, txtMACRemoto, txtExpiracionRemoto)

        dtLocal = ft.AbrirDataTable(dsLocal, "tblServidorLocal", MyConn, " select * from datumreg where estacion = 0 ")
        If dtLocal.Rows.Count > 0 Then
            AsignarTXT()
        Else
            IniciarTxt()
        End If

        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT()

        ft.habilitarObjetos(False, True, txtCodigoLocal, txtNombreLocal, txtExpiracionLocal, txtMACLocal)
        Dim nEncriptado As String = ""

        '//// LOCAL
        With dtLocal.Rows(0)

            txtCodigoLocal.Text = ft.muestraCampoTexto(.Item("codigo"))
            txtNombreLocal.Text = ft.muestraCampoTexto(.Item("Empresa"))

            ft.RellenaCombo(aTipoAplicacion, cmbAplicacionLocal, Convert.ToInt16(.Item("aplicacion")))
            ft.RellenaCombo(aLicencia, cmbLicenciaLocal, Convert.ToInt16(.Item("licencia")))

            txtNumLicenciaLocal.Text = ft.muestraCampoTexto(.Item("numero_licencia"))
            txtMACLocal.Text = ft.muestraCampoTexto(.Item("mac_estacion"))

            txtExpiracionLocal.Text = ft.muestraCampoFecha(.Item("fecha_expiracion"))

            nEncriptado = ft.DevuelveScalarCadena(MyConn, " select cast( aes_decrypt( cadena_autenticacion,'" & nEncryptWord & "') AS CHAR) from datumreg where estacion = 0 ")

            '//// REMOTO
            txtCodigoRemoto.Text = nEncriptado.Split("|")(0)
            txtNombreRemoto.Text = nEncriptado.Split("|")(1)

            ft.RellenaCombo(aTipoAplicacion, cmbaplicacionRemoto, Convert.ToInt16(nEncriptado.Split("|")(2)))
            ft.RellenaCombo(aLicencia, cmbLicenciaRemoto, Convert.ToInt16(nEncriptado.Split("|")(3)))

            txtNumLicenciaRemoto.Text = ft.muestraCampoTexto(nEncriptado.Split("|")(4))
            txtMACRemoto.Text = ft.muestraCampoTexto(nEncriptado.Split("|")(6))

            txtExpiracionRemoto.Text = ft.muestraCampoFecha(nEncriptado.Split("|")(7))

        End With

    End Sub
    Private Sub IniciarTxt()

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtCodigoLocal, txtCodigoRemoto, txtNombreLocal, _
                               txtNombreRemoto, txtNumLicenciaLocal, txtNumLicenciaRemoto, txtMACRemoto)

        ft.RellenaCombo(aTipoAplicacion, cmbAplicacionLocal)
        ft.RellenaCombo(aLicencia, cmbLicenciaLocal)

        ft.RellenaCombo(aTipoAplicacion, cmbaplicacionRemoto)
        ft.RellenaCombo(aLicencia, cmbLicenciaRemoto)

        txtMACLocal.Text = ft.getMACAddresses()(0)
        txtExpiracionLocal.Text = ft.muestraCampoFecha(jytsistema.sFechadeTrabajo)

    End Sub

    Private Sub frmServidorValidacion_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
        ft = Nothing
    End Sub

    Private Sub frmServidorValidacion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigoLocal.Text)
    End Sub

    Private Function Validado() As Boolean
        Return True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim Insertar As Boolean = False
            If dtLocal.Rows.Count = 0 Then Insertar = True

            InsertEditCONTROLLicencia(MyConn, lblInfo, Insertar, txtCodigoLocal.Text, txtNombreLocal.Text, _
                                       cmbAplicacionLocal.SelectedIndex, cmbLicenciaLocal.SelectedIndex, txtNumLicenciaLocal.Text, _
                                       txtMACLocal.Text, Convert.ToDateTime(txtExpiracionLocal.Text), nEncryptWord)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnHaciaRemoto.Click
        txtCodigoRemoto.Text = txtCodigoLocal.Text
        txtNombreRemoto.Text = txtNombreLocal.Text
        cmbaplicacionRemoto.SelectedIndex = cmbAplicacionLocal.SelectedIndex
        cmbLicenciaRemoto.SelectedIndex = cmbLicenciaLocal.SelectedIndex
        txtNumLicenciaRemoto.Text = txtNumLicenciaLocal.Text
        txtMACRemoto.Text = txtMACLocal.Text
        txtExpiracionRemoto.Text = txtExpiracionLocal.Text
    End Sub
End Class