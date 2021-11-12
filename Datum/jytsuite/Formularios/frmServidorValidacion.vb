Imports MySql.Data.MySqlClient
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json.Converters
Imports Newtonsoft.Json.Serialization
Imports System.Linq

Public Class frmServidorValidacion

    Private Const sModulo As String = "Validacion Servidor"

    Private MyConn As New MySqlConnection
    Private dsLocal As New DataSet
    Private dtLocal As New DataTable
    Private dt As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private strPort As String = "port = 3306; "
    Private licence_servers() As String = {"datum2online.ddns.net",
                                           "alimica.systes.net",
                                           "casiano.systes.net"}

    Private strRemoteConection As String = "server=datum2online.ddns.net " _
                + "; database=jytsuitejytsuite;" _
                + strPort _
                + jytsistema.strCon

    Private aTipoAplicacion() As String = {"Basica",
                                           "Normal",
                                           "Plus",
                                           "Plus Producción",
                                           "Plus Restaturant",
                                           "Plus Prensa",
                                           "Plus Farmacia",
                                           "Plus Clínicas",
                                           "Plus Hotel/Spa"}

    Private aLicencia() As String = {"Inactiva", "Activa"}
    Private nEncryptWord As String = ""
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        MyConn = MyCon

        nEncryptWord = ft.getHardDrivesSerials()(0)

        ft.habilitarObjetos(False, True, txtCodigoRemoto, txtNombreRemoto, cmbaplicacionRemoto, cmbLicenciaRemoto,
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

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtCodigoLocal, txtCodigoRemoto, txtNombreLocal,
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

            InsertEditCONTROLLicencia(MyConn, lblInfo, Insertar, txtCodigoLocal.Text, txtNombreLocal.Text,
                                       cmbAplicacionLocal.SelectedIndex, cmbLicenciaLocal.SelectedIndex, txtNumLicenciaLocal.Text,
                                       txtMACLocal.Text, Convert.ToDateTime(txtExpiracionLocal.Text), nEncryptWord)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub



    Private Async Function btnHaciaRemoto_Click(sender As Object, e As EventArgs) As Threading.Tasks.Task Handles btnHaciaRemoto.Click

        'Using client As New HttpClient()


        '    Dim response As HttpResponseMessage = Await client.GetAsync("/WeatherForecast")


        '    If (response.IsSuccessStatusCode = True) Then
        '        Dim reports() = New WeatherForecast() {}
        '        Dim rep As String = Await response.Content.ReadAsStringAsync()
        '    End If
        'End Using

        'Using client As HttpClient = New HttpClient()
        '    client.BaseAddress = New Uri("https://localhost:44356/")
        '    client.DefaultRequestHeaders.Accept.Clear()
        '    client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
        '    Dim str As String = String.Format("api/customer/all/{0}", jytsistema.WorkID)
        '    Using response As HttpResponseMessage = Await client.GetAsync(str)
        '        Using content As HttpContent = response.Content
        '            ' Get contents of page as a String.
        '            Dim result As String = Await content.ReadAsStringAsync()
        '            ' If data exists, convert as a table
        '            If result IsNot Nothing Then
        '                'Dim res = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(result)("result"))
        '                ' DataTable
        '                dt = JsonConvert.DeserializeObject(Of DataTable)(result)
        '                ' IEnumarable 
        '                Dim wl = JsonConvert.DeserializeObject(Of IEnumerable(Of Customer))(result)

        '            End If
        '        End Using
        '    End Using
        'End Using

        Dim gnGet As GenericGet(Of Customer) = New GenericGet(Of Customer)()
        Dim customers As List(Of Customer) = Await gnGet.GetList(String.Format("api/customer/all/{0}", jytsistema.WorkID))

        Dim cust = customers.Where(Function(c) c.Codcli = "00000010")


    End Function
End Class

Public Class WeatherForecast
    <JsonProperty("date")>
    Public Property RealDate() As DateTime?
    Public Property TemperatureC() As Integer?
    Public Property TemperatureF() As Integer?
    Public Property Summary As String

End Class