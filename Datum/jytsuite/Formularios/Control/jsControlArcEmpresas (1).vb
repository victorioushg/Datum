Imports MySql.Data.MySqlClient
Imports System.IO
Public Class jsControlArcEmpresas
    Private Const sModulo As String = "Empresas"
    Private Const lRegion As String = "RibbonButton165"
    Private Const nTabla As String = "empresas"

    Private strSQL As String = "select * from jsconctaemp order by id_emp "

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private ft As New Transportables

    Private aTipoPersona() As String = {"Natural", "Jurídica"}
    Private aTipoSociedad() As String = {"S.A.", "C.A.", "S.R.L.", "Sucursal del exterior", "Soc. de personas", "Comandita Simple", "Consorcios", "Otra"}

    Private i_modo As Integer
    Private nPosicionCat As Long
    Private CaminoImagen As String = ""

    Private Sub jsControlArcEmpresas_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsControlArcEmpresas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaTXT(nPosicionCat)
            Else
                IniciarEmpresa(False)
            End If

            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnSeleccionar, btnPrimero, _
                          btnSiguiente, btnAnterior, btnUltimo, btnImprimir, btnSalir)
    End Sub

    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt

            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)

                txtCodigo.Text = ft.muestraCampoTexto(.Item("id_emp"))
                txtNombre.Text = ft.muestraCampoTexto(.Item("nombre"))
                txtDirFiscal.Text = ft.muestraCampoTexto(.Item("dirfiscal"))
                txtDirDepsacho.Text = ft.muestraCampoTexto(.Item("dirorigen"))
                txtCiudad.Text = ft.muestraCampoTexto(.Item("ciudad"))
                txtEstado.Text = ft.muestraCampoTexto(.Item("estado"))
                txtCodGeo.Text = ft.muestraCampoTexto(.Item("codgeo"))
                txtZIP.Text = ft.muestraCampoTexto(.Item("zip"))
                txtTelefono1.Text = ft.muestraCampoTexto(.Item("telef1"))
                txtTelefono2.Text = ft.muestraCampoTexto(.Item("telef2"))
                txtFAX.Text = ft.muestraCampoTexto(.Item("fax"))
                txtCorreoElectronico.Text = ft.muestraCampoTexto(.Item("email"))
                txtActividad.Text = ft.muestraCampoTexto(.Item("actividad"))
                txtRIF.Text = ft.muestraCampoTexto(.Item("rif"))
                txtNIT.Text = ft.muestraCampoTexto(.Item("nit"))
                txtCIIU.Text = ft.muestraCampoTexto(.Item("ciiu"))

                ft.RellenaCombo(aTipoPersona, cmbTipoPersona, .Item("tipopersona"))
                ft.RellenaCombo(aTipoSociedad, cmbTipoSociedad, .Item("tiposoc"))

                txtCILetra.Text = ft.muestraCampoTexto(.Item("nacional"))
                txtCINumero.Text = ft.muestraCampoTexto(.Item("ci"))
                txtPasaporte.Text = ft.muestraCampoTexto(.Item("pasaporte"))

                chkLucro.Checked = CBool(.Item("lucro"))
                chkRentas.Checked = CBool(.Item("rentasexentas"))
                chkCasado.Checked = CBool(.Item("casado"))
                chkSeparado.Checked = CBool(.Item("separabienes"))
                chkEsposa.Checked = CBool(.Item("esposadeclara"))

                txtInicio.Text = ft.muestraCampoFecha(.Item("inicio"))
                txtCierre.Text = ft.muestraCampoFecha(.Item("cierre"))

                txtCILetraRep.Text = ft.muestraCampoTexto(.Item("rep_nacional"))
                txtCIRep.Text = ft.muestraCampoTexto(.Item("rep_ci"))
                txtRIFRep.Text = ft.muestraCampoTexto(.Item("rep_rif"))
                txtNITRep.Text = ft.muestraCampoTexto(.Item("rep_nit"))
                txtNombreRep.Text = ft.muestraCampoTexto(.Item("rep_nombre"))
                txtDirFiscalRep.Text = ft.muestraCampoTexto(.Item("rep_direccion"))
                txtCiudadRep.Text = ft.muestraCampoTexto(.Item("rep_ciudad"))
                txtEstadoRep.Text = ft.muestraCampoTexto(.Item("rep_estado"))
                txtTelefonoRep.Text = ft.muestraCampoTexto(.Item("rep_telef"))
                txtFaxRep.Text = ft.muestraCampoTexto(.Item("rep_fax"))
                txtEmailRep.Text = ft.muestraCampoTexto(.Item("rep_email"))

                txtCILetraGer.Text = ft.muestraCampoTexto(.Item("ger_nacional"))
                txtCIGer.Text = ft.muestraCampoTexto(.Item("ger_ci"))
                txtNombreGer.Text = ft.muestraCampoTexto(.Item("ger_nombre"))
                txtDirFiscalGer.Text = ft.muestraCampoTexto(.Item("ger_direccion"))
                txtCiudadGer.Text = ft.muestraCampoTexto(.Item("ger_ciudad"))
                txtEdoGer.Text = ft.muestraCampoTexto(.Item("ger_estado"))
                txtTelefonoGer.Text = ft.muestraCampoTexto(.Item("ger_telef"))
                txtMovilGer.Text = ft.muestraCampoTexto(.Item("ger_cel"))
                txtemailGer.Text = ft.muestraCampoTexto(.Item("ger_email"))

                CaminoImagen = BaseDatosAImagen(dt.Rows(nRow), "logo", .Item("id_emp"))
                If My.Computer.FileSystem.FileExists(CaminoImagen) Then
                    Dim fs As System.IO.FileStream
                    fs = New System.IO.FileStream(CaminoImagen, IO.FileMode.Open, IO.FileAccess.Read)
                    pctLogo.Image = System.Drawing.Image.FromStream(fs)
                    fs.Close()
                Else
                    pctLogo.Image = Nothing
                End If

            End With
        End With

        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub IniciarEmpresa(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = ft.autoCodigo(myConn, "id_emp", "jsconctaemp", "", "", 2)
        Else
            txtCodigo.Text = ""
        End If

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtCodigo, txtNombre, txtDirFiscal, txtDirDepsacho, txtCiudad, txtEstado, txtCodGeo, _
                            txtZIP, txtTelefono1, txtTelefono2, txtFAX, txtCorreoElectronico, txtActividad, txtRIF, _
                            txtNIT, txtCIIU, txtCILetra, txtCINumero, txtPasaporte, txtCILetraRep, txtCIRep, txtRIFRep, _
                            txtNITRep, txtNombreRep, txtDirFiscalRep, txtCiudadRep, txtEstadoRep, txtTelefonoRep, _
                            txtFaxRep, txtEmailRep, txtCILetraGer, txtCIGer, txtNombreGer, _
                            txtDirFiscalGer, txtCiudadGer, txtEdoGer, txtTelefonoGer, txtMovilGer, txtemailGer)

        ft.RellenaCombo(aTipoPersona, cmbTipoPersona)
        ft.RellenaCombo(aTipoSociedad, cmbTipoSociedad)

        chkLucro.Checked = False
        chkRentas.Checked = False
        chkCasado.Checked = False
        chkSeparado.Checked = False
        chkEsposa.Checked = False

        txtInicio.Text = ft.muestraCampoFecha(PrimerDiaAño(jytsistema.sFechadeTrabajo))
        txtCierre.Text = ft.muestraCampoFecha(UltimoDiaAño(jytsistema.sFechadeTrabajo))

        pctLogo.Image = Nothing


    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, True, txtNombre, txtDirFiscal, txtDirDepsacho, txtCiudad, txtEstado, txtCodGeo, _
                            txtZIP, txtTelefono1, txtTelefono2, txtFAX, txtCorreoElectronico, txtActividad, txtRIF, _
                            txtNIT, txtCIIU, cmbTipoPersona, cmbTipoSociedad, txtCILetra, txtCINumero, txtPasaporte, _
                            chkLucro, chkRentas, chkCasado, chkSeparado, chkEsposa, _
                            txtCILetraRep, txtCIRep, txtRIFRep, txtNITRep, txtNombreRep, txtDirFiscalRep, txtCiudadRep, _
                            txtEstadoRep, txtTelefonoRep, txtFaxRep, txtEmailRep, txtCILetraGer, txtCIGer, txtNombreGer, _
                            txtDirFiscalGer, txtCiudadGer, txtEdoGer, txtTelefonoGer, txtMovilGer, txtemailGer, btnLogo)

        If i_modo = movimiento.iAgregar Then ft.habilitarObjetos(True, False, btnInicio, btnCierre)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False

        ft.habilitarObjetos(False, True, txtCodigo, txtNombre, txtDirFiscal, txtDirDepsacho, txtCiudad, txtEstado, txtCodGeo, _
                            txtZIP, txtTelefono1, txtTelefono2, txtFAX, txtCorreoElectronico, txtActividad, txtRIF, _
                            txtNIT, txtCIIU, cmbTipoPersona, cmbTipoSociedad, txtCILetra, txtCINumero, txtPasaporte, _
                            chkLucro, chkRentas, chkCasado, chkSeparado, chkEsposa, txtInicio, btnInicio, txtCierre, btnCierre, _
                            txtCILetraRep, txtCIRep, txtRIFRep, txtNITRep, txtNombreRep, txtDirFiscalRep, txtCiudadRep, _
                            txtEstadoRep, txtTelefonoRep, txtFaxRep, txtEmailRep, txtCILetraGer, txtCIGer, txtNombreGer, _
                            txtDirFiscalGer, txtCiudadGer, txtEdoGer, txtTelefonoGer, txtMovilGer, txtemailGer, btnLogo)

        MenuBarra.Enabled = True
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Me.BindingContext(ds, nTabla).Position = 0
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Me.BindingContext(ds, nTabla).Position -= 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Me.BindingContext(ds, nTabla).Position += 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionCat = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarEmpresa(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        i_modo = movimiento.iEditar
        nPosicionCat = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        EliminaEmpresa()
    End Sub
    Private Sub EliminaEmpresa()
        If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) = 2 Then
            Dim aCamposDel() As String = {"id_emp"}
            Dim aStringsDel() As String = {txtCodigo.Text}

            If ft.PreguntaEliminarRegistro() = MsgBoxResult.Yes Then
                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsconctaemp", strSQL, aCamposDel, aStringsDel, _
                                              Me.BindingContext(ds, nTabla).Position, True))
            End If
        End If

    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar

        Dim Campos() As String = {"id_emp", "nombre"}
        Dim Nombres() As String = {"Código empresa", "Nombre empresa"}
        Dim Anchos() As Integer = {100, 2500}
        f.Text = "Empresas"
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Empresas del sistema...")
        AsignaTXT(f.Apuntador)
        f = Nothing

    End Sub
    Private Function Validado() As Boolean

        If txtNombre.Text.Trim = "" Then
            ft.mensajeCritico("Debe indicar un Nombre de Empresa válido...")
            ft.enfocarTexto(txtNombre)
            Return False
        End If

        If txtRIF.Text.Trim = "" Then
            ft.mensajeCritico("Debe indicar N° Registro de Información Tributaria (RIF) válido...")
            ft.enfocarTexto(txtRIF)
            Return False
        End If

        Return True

    End Function
    Private Sub GuardarTXT()
        Dim MyData() As Byte
        Dim Inserta As Boolean = False

        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionCat = ds.Tables(nTabla).Rows.Count
        End If

        InsertEditCONTROLEmpresa(myConn, lblInfo, Inserta, txtCodigo.Text, txtNombre.Text, txtDirFiscal.Text, txtDirDepsacho.Text, txtCiudad.Text, _
            txtEstado.Text, txtCodGeo.Text, txtZIP.Text, txtTelefono1.Text, txtTelefono2.Text, txtFAX.Text, _
            txtCorreoElectronico.Text, txtActividad.Text, txtRIF.Text, txtNIT.Text, txtCIIU.Text, _
            cmbTipoPersona.SelectedIndex, CDate(txtInicio.Text), CDate(txtCierre.Text), cmbTipoSociedad.SelectedIndex, _
            IIf(chkLucro.Checked, "1", "0"), txtCILetra.Text, txtCINumero.Text, txtPasaporte.Text, IIf(chkCasado.Checked, "1", "0"), _
            IIf(chkSeparado.Checked, "1", "0"), IIf(chkRentas.Checked, "1", "0"), IIf(chkEsposa.Checked, "1", "0"), _
            txtRIFRep.Text, txtNITRep.Text, txtCILetraRep.Text, txtCIRep.Text, txtNombreRep.Text, _
            txtDirFiscalRep.Text, txtCiudadRep.Text, txtEstadoRep.Text, txtTelefonoRep.Text, txtFaxRep.Text, _
            txtEmailRep.Text, txtCILetraGer.Text, txtCIGer.Text, txtNombreGer.Text, txtDirFiscalGer.Text, _
            txtTelefonoGer.Text, txtCiudadGer.Text, txtEdoGer.Text, txtMovilGer.Text, txtemailGer.Text)


        jytsistema.WorkID = txtCodigo.Text
        jytsistema.WorkName = txtNombre.Text

        ValoresInicialesEmpresa(myConn, lblInfo, txtCodigo.Text)

        If Not pctLogo.Image Is Nothing Then
            If CaminoImagen <> "" Then
                Dim fs As New FileStream(CaminoImagen, FileMode.OpenOrCreate, FileAccess.Read)
                MyData = New Byte(fs.Length) {}
                fs.Read(MyData, 0, fs.Length)
                fs.Close()
                GuardarLogo(myConn, lblInfo, txtCodigo.Text, MyData)
            End If
        End If

        dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)
        Me.BindingContext(ds, nTabla).Position = nPosicionCat
        AsignaTXT(nPosicionCat)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub ValoresInicialesEmpresa(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoEmpresa As String)

        'CLAVE USUARIO SOPORTE
        If ft.DevuelveScalarCadena(MyConn, " select usuario from jsconctausu where usuario = 'jytsuite' ") = "" Then
            InsertEditCONTROLUsuario(MyConn, lblInfo, True, "jytsuite", _
                "capidoncella", "VICTOR HUGO GUTIERREZ", "00000", "001", txtCodigo.Text, _
                0, 2, 1, "", "")
        End If
        If ft.DevuelveScalarCadena(MyConn, " select usuario from jsconctausu where usuario = 'soporte' ") = "" Then
            InsertEditCONTROLUsuario(MyConn, lblInfo, True, "soporte", _
                "Datum2017", "SOPORTE DATUM", "09090", "001", txtCodigo.Text, _
                0, 2, 1, "", "")
        End If

        'FORMATO INICIAL DE CHEQUES EN BANCOS
        If ft.DevuelveScalarEntero(MyConn, " select count(*) from jsbancatfor where id_emp = '" & CodigoEmpresa & "' group by id_emp ") = 0 Then
            InsertEditBANCOSPlantillaCheque(MyConn, lblInfo, True, "01", "FORMATO CHEQUE BANCO DE LA REPUBLICA", 440, 6350, _
                        1295, 1480, 1595, 1460, 2300, 500, 850, 3800)
        End If

        'ALMACENES 
        If ft.DevuelveScalarEntero(MyConn, " select count(*) from jsmercatalm where codalm = '00001' and id_emp = '" & CodigoEmpresa & "' group by codalm ") = 0 Then
            InsertEditMERCASAlmacen(MyConn, lblInfo, True, "00001", "PRINCIPAL", "", 0)
        End If
        If ft.DevuelveScalarEntero(MyConn, " select count(*) from jsmercatalm where codalm = '00002' and id_emp = '" & CodigoEmpresa & "' group by codalm ") = 0 Then
            InsertEditMERCASAlmacen(MyConn, lblInfo, True, "00002", "DEVOLUCIONES EN MAL ESTADO", "", 0)
        End If

        'PARAMETROS
        IniciarParametros(MyConn, lblInfo, CodigoEmpresa)
        'CONTADORES
        IniciarContadores(MyConn, lblInfo, CodigoEmpresa)

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If dt.Rows.Count = 0 Then
            IniciarEmpresa(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            If Me.BindingContext(ds, nTabla).Position > 0 Then _
                nPosicionCat = Me.BindingContext(ds, nTabla).Position

            AsignaTXT(nPosicionCat)
        End If
        DesactivarMarco0()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub


    Private Sub btnLogo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogo.Click
        Dim ofd As New OpenFileDialog()

        ofd.InitialDirectory = "c:\"
        ofd.Filter = "Archivos JPG |*.jpg"
        ofd.FilterIndex = 2
        ofd.RestoreDirectory = True

        If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            CaminoImagen = ofd.FileName
            pctLogo.ImageLocation = CaminoImagen
            pctLogo.Load()
        End If

        ofd = Nothing

    End Sub
End Class