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

    Private aTipoPersona() As String = {"Natural", "Jurídica"}
    Private aTipoSociedad() As String = {"S.A.", "C.A.", "S.R.L.", "Sucursal del exterior", "Soc. de personas", "Comandita Simple", "Consorcios", "Otra"}

    Private i_modo As Integer
    Private nPosicionCat As Long
    Private CaminoImagen As String = ""

    Private Sub jsControlArcEmpresas_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

    End Sub
    Private Sub jsControlArcEmpresas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)



            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaTXT(nPosicionCat)
            Else
                IniciarEmpresa(False)
            End If
            ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo registro")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> registro deseado")
        C1SuperTooltip1.SetToolTip(btnSeleccionar, "<B>Seleccionar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir al <B>primer</B> registro")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir al <B>siguiente</B> registro")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir al registro <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir al <B>último registro</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B>")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")

        'Botones Adicionales


    End Sub



    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt

            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

            With .Rows(nRow)
                txtCodigo.Text = .Item("id_emp")
                txtNombre.Text = IIf(IsDBNull(.Item("nombre")), "", .Item("nombre"))
                txtDirFiscal.Text = IIf(IsDBNull(.Item("dirfiscal")), "", .Item("dirfiscal"))
                txtDirDepsacho.Text = IIf(IsDBNull(.Item("dirorigen")), "", .Item("dirorigen"))
                txtCiudad.Text = IIf(IsDBNull(.Item("ciudad")), "", .Item("ciudad"))
                txtEstado.Text = IIf(IsDBNull(.Item("estado")), "", .Item("estado"))
                txtCodGeo.Text = IIf(IsDBNull(.Item("codgeo")), "", .Item("codgeo"))
                txtZIP.Text = IIf(IsDBNull(.Item("zip")), "", .Item("zip"))
                txtTelefono1.Text = IIf(IsDBNull(.Item("telef1")), "", .Item("telef1"))
                txtTelefono2.Text = IIf(IsDBNull(.Item("telef2")), "", .Item("telef2"))
                txtFAX.Text = IIf(IsDBNull(.Item("fax")), "", .Item("fax"))
                txtCorreoElectronico.Text = IIf(IsDBNull(.Item("email")), "", .Item("email"))
                txtActividad.Text = IIf(IsDBNull(.Item("actividad")), "", .Item("actividad"))
                txtRIF.Text = IIf(IsDBNull(.Item("rif")), "", .Item("rif"))
                txtNIT.Text = IIf(IsDBNull(.Item("nit")), "", .Item("nit"))
                txtCIIU.Text = IIf(IsDBNull(.Item("ciiu")), "", .Item("ciiu"))
                RellenaCombo(aTipoPersona, cmbTipoPersona, .Item("tipopersona"))
                RellenaCombo(aTipoSociedad, cmbTipoSociedad, .Item("tiposoc"))
                txtCILetra.Text = IIf(IsDBNull(.Item("nacional")), "", .Item("nacional"))
                txtCINumero.Text = IIf(IsDBNull(.Item("ci")), "", .Item("ci"))
                txtPasaporte.Text = IIf(IsDBNull(.Item("pasaporte")), "", .Item("pasaporte"))
                chkLucro.Checked = CBool(.Item("lucro"))
                chkRentas.Checked = CBool(.Item("rentasexentas"))
                chkCasado.Checked = CBool(.Item("casado"))
                chkSeparado.Checked = CBool(.Item("separabienes"))
                chkEsposa.Checked = CBool(.Item("esposadeclara"))
                txtInicio.Text = IIf(IsDBNull(.Item("inicio")), FormatoFecha(jytsistema.sFechadeTrabajo), FormatoFecha(CDate(.Item("inicio").ToString)))
                txtCierre.Text = IIf(IsDBNull(.Item("cierre")), FormatoFecha(jytsistema.sFechadeTrabajo), FormatoFecha(CDate(.Item("cierre").ToString)))

                txtCILetraRep.Text = IIf(IsDBNull(.Item("rep_nacional")), "", .Item("rep_nacional"))
                txtCIRep.Text = IIf(IsDBNull(.Item("rep_ci")), "", .Item("rep_ci"))
                txtRIFRep.Text = IIf(IsDBNull(.Item("rep_rif")), "", .Item("rep_rif"))
                txtNITRep.Text = IIf(IsDBNull(.Item("rep_nit")), "", .Item("rep_nit"))
                txtNombreRep.Text = IIf(IsDBNull(.Item("rep_nombre")), "", .Item("rep_nombre"))
                txtDirFiscalRep.Text = IIf(IsDBNull(.Item("rep_direccion")), "", .Item("rep_direccion"))
                txtCiudadRep.Text = IIf(IsDBNull(.Item("rep_ciudad")), "", .Item("rep_ciudad"))
                txtEstadoRep.Text = IIf(IsDBNull(.Item("rep_estado")), "", .Item("rep_estado"))
                txtTelefonoRep.Text = IIf(IsDBNull(.Item("rep_telef")), "", .Item("rep_telef"))
                txtFaxRep.Text = IIf(IsDBNull(.Item("rep_fax")), "", .Item("rep_fax"))
                txtEmailRep.Text = IIf(IsDBNull(.Item("rep_email")), "", .Item("rep_email"))

                txtCILetraGer.Text = IIf(IsDBNull(.Item("ger_nacional")), "", .Item("ger_nacional"))
                txtCIGer.Text = IIf(IsDBNull(.Item("ger_ci")), "", .Item("ger_ci"))
                txtNombreGer.Text = IIf(IsDBNull(.Item("ger_nombre")), "", .Item("ger_nombre"))
                txtDirFiscalGer.Text = IIf(IsDBNull(.Item("ger_direccion")), "", .Item("ger_direccion"))
                txtCiudadGer.Text = IIf(IsDBNull(.Item("ger_ciudad")), "", .Item("ger_ciudad"))
                txtEdoGer.Text = IIf(IsDBNull(.Item("ger_estado")), "", .Item("ger_estado"))
                txtTelefonoGer.Text = IIf(IsDBNull(.Item("ger_telef")), "", .Item("ger_telef"))
                txtMovilGer.Text = IIf(IsDBNull(.Item("ger_cel")), "", .Item("ger_cel"))
                txtemailGer.Text = IIf(IsDBNull(.Item("ger_email")), "", .Item("ger_email"))



                CaminoImagen = BaseDatosAImagen(dt.Rows(nRow), "logo", .Item("id_emp"))
                'CaminoImagen = My.Computer.FileSystem.CurrentDirectory & "\" & "logo" & .Item("id_emp") & ".jpg"
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

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub IniciarEmpresa(ByVal Inicio As Boolean)

        Dim c As Control
        For Each c In grpEmpresa.Controls
            If c.GetType Is txtCodigo.GetType Then c.Text = ""
        Next
        If Inicio Then txtCodigo.Text = AutoCodigo(2, ds, nTabla, "id_emp")

        RellenaCombo(aTipoPersona, cmbTipoPersona)
        RellenaCombo(aTipoSociedad, cmbTipoSociedad)

        chkLucro.Checked = False
        chkRentas.Checked = False
        chkCasado.Checked = False
        chkSeparado.Checked = False
        chkEsposa.Checked = False



        txtInicio.Text = FormatoFecha(PrimerDiaAño(jytsistema.sFechadeTrabajo))
        txtCierre.Text = FormatoFecha(UltimoDiaAño(jytsistema.sFechadeTrabajo))

        For Each c In grpRepresentante.Controls
            If c.GetType Is txtCodigo.GetType Then c.Text = ""
        Next
        For Each c In grpGerente.Controls
            If c.GetType Is txtCodigo.GetType Then c.Text = ""
        Next
        pctLogo.Image = Nothing

    End Sub
    Private Sub ActivarMarco0()
        Dim c As Control
        grpAceptarSalir.Visible = True

        For Each c In grpEmpresa.Controls
            If c.Name = "txtCodigo" Or c.Name = "txtInicio" Or _
                c.Name = "txtCierre" Then
                c.Enabled = False
            Else
                c.Enabled = True
                If c.GetType Is txtCodigo.GetType Then
                    c.BackColor = Color.White
                Else
                    If c.GetType Is cmbTipoPersona.GetType Then
                        c.BackColor = Color.White
                    End If
                End If
            End If
        Next
        For Each c In grpRepresentante.Controls
            c.Enabled = True
            If c.GetType Is txtCodigo.GetType Then c.BackColor = Color.White
        Next
        For Each c In grpGerente.Controls
            c.Enabled = True
            If c.GetType Is txtCodigo.GetType Then c.BackColor = Color.White
        Next

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
    End Sub
    Private Sub DesactivarMarco0()
        Dim c As Control
        grpAceptarSalir.Visible = False

        For Each c In grpEmpresa.Controls
            c.Enabled = False
            If c.Name = "pctlogo" Then c.Enabled = True
            If c.GetType Is txtCodigo.GetType Then
                c.BackColor = Color.AliceBlue
            Else
                If c.GetType Is cmbTipoPersona.GetType Then
                    c.BackColor = Color.AliceBlue
                End If
            End If
        Next
        For Each c In grpRepresentante.Controls
            c.Enabled = False
            If c.GetType Is txtCodigo.GetType Then c.BackColor = Color.AliceBlue
        Next
        For Each c In grpGerente.Controls
            c.Enabled = False
            If c.GetType Is txtCodigo.GetType Then c.BackColor = Color.AliceBlue
        Next

        MenuBarra.Enabled = True
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
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
            Dim Respuesta As Microsoft.VisualBasic.MsgBoxResult
            Dim aCamposDel() As String = {"id_emp"}
            Dim aStringsDel() As String = {txtCodigo.Text}
            Respuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
            If Respuesta = MsgBoxResult.Yes Then
                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsconctaemp", strSQL, aCamposDel, aStringsDel, _
                                              Me.BindingContext(ds, nTabla).Position, True))
            End If
        End If

    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar

        Dim Campos() As String = {"id_emp", "nombre"}
        Dim Nombres() As String = {"Código empresa", "Nombre empresa"}
        Dim Anchos() As Long = {100, 2500}
        f.Text = "Empresas"
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Empresas del sistema...")
        AsignaTXT(f.Apuntador)
        f = Nothing

    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        '        Dim f As New jsBanRepParametros
        '        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cFichaBanco, "Ficha de Banco", txtCodigo.Text)
        '        f = Nothing
    End Sub
    Private Function Validado() As Boolean
        Validado = True
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

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionCat
        AsignaTXT(nPosicionCat)
        DesactivarMarco0()
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub ValoresInicialesEmpresa(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoEmpresa As String)

        'CLAVE USUARIO SOPORTE
        If EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select usuario from jsconctausu where usuario = 'jytsuite' ") = "0" Then
            InsertEditCONTROLUsuario(MyConn, lblInfo, True, "jytsuite", _
                "capidoncella", "VICTOR HUGO GUTIERREZ", "00000", "001", txtCodigo.Text, _
                0, 2, 1, "", "")
        End If

        If EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select usuario from jsconctausu where usuario = 'soporte' ") = "0" Then
            InsertEditCONTROLUsuario(MyConn, lblInfo, True, "soporte", _
                "Datum2016", "SOPORTE DATUM", "09090", "001", txtCodigo.Text, _
                0, 2, 1, "", "")
        End If

        'FORMATO INICIAL DE CHEQUES EN BANCOS
        If EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsbancatfor where id_emp = '" & jytsistema.WorkID & "' group by id_emp ").ToString = "0" Then
            InsertEditBANCOSPlantillaCheque(MyConn, lblInfo, True, "01", "FORMATO CHEQUE BANCO DE LA REPUBLICA", 440, 6350, _
                        1295, 1480, 1595, 1460, 2300, 500, 850, 3800)
        End If
        'ALMACENES 
        If EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsmercatalm where codalm = '00001' and id_emp = '" & jytsistema.WorkID & "' group by codalm ").ToString = "0" Then
            InsertEditMERCASAlmacen(MyConn, lblInfo, True, "00001", "PRINCIPAL", "", 0)
        End If
        If EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsmercatalm where codalm = '00002' and id_emp = '" & jytsistema.WorkID & "' group by codalm ").ToString = "0" Then
            InsertEditMERCASAlmacen(MyConn, lblInfo, True, "00002", "DEVOLUCIONES EN MAL ESTADO", "", 0)
        End If

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