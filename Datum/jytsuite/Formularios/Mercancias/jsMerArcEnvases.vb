Imports MySql.Data.MySqlClient
Imports C1.Win.C1Chart
Imports System.IO

Public Class jsMerArcEnvases

    Private Const sModulo As String = "Envases y/o cestas"
    Private Const lRegion As String = "RibbonButton308"
    Private Const nTabla As String = "tblEnvases"
    Private Const nTablaMovimientos As String = "tblmovimientosEnvases"
   
    Private strSQL As String = "select * from jsmercatenv where id_emp = '" & jytsistema.WorkID & "' order by codenv "
    Private strSQLMov As String
   

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtMovimientos As New DataTable
    Private dtEquivalencias As New DataTable
    Private dtCuotas As New DataTable
    Private ft As New Transportables

    Private aTipo() As String = {"Cesta"}

    Private aEstatus() As String = {"Inactivo", "Activo"}
    '/// { "En Tránsito", "En Cliente", "En Proveedor", "Vacío/Depósito"
    '/// "Lleno/Depósito", "Por Reparación", "Desincorporado", "Indeterminado"}

    Private aTipoMovimiento() As String = {"Todos", "Entradas", "Salidas"}

    Private strUnidad As String = ""

    Private i_modo As Integer

    Private nPosicionCat As Long
    Private nPosicionMov As Long

    Private Sub jsMerArcMercancias_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsMerArcMercancias_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try

            myConn.Open()

            dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)
            DesactivarMarco0()

            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaTXT(nPosicionCat)
            Else
                IniciarEnvase(False)
            End If

            tbcEnvases.SelectedTab = C1DockingTabPage1
            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

            AsignarTooltips()


        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub

    Private Sub AsignarTooltips()

        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnSeleccionar, _
                          btnPrimero, btnSiguiente, btnAnterior, btnUltimo, btnImprimir, btnSalir, btnRecalcular)

        'Botones Adicionales
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnFabricante, btnMaterial, btnFechaAdquisicion, btnFechaFabricacion, btnFechaRevision)
    End Sub
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        If txtCodigoMovimientos.Text.Trim() <> "" Then
            dtMovimientos = ft.MostrarFilaEnTabla(myConn, ds, nTablaMovimientos, strSQLMov, Me.BindingContext, MenuBarra,
               dg, lRegion, jytsistema.sUsuario, nRow, Actualiza)
            Me.BindingContext(ds, nTablaMovimientos).Position = nPosicionMov
        End If
    End Sub

    Private Sub AsignaTXT(ByVal nRow As Long)

        i_modo = movimiento.iConsultar

        If dt.Rows.Count > 0 Then
            With dt
                ft.MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

                With .Rows(nRow)
                    'Envases
                    nPosicionCat = nRow

                    txtCodigo.Text = ft.muestraCampoTexto(.Item("CODENV"))
                    txtSerialInterno.Text = ft.muestraCampoTexto(.Item("SERIAL_1"))
                    txtSerialExterno.Text = ft.muestraCampoTexto(.Item("SERIAL_2"))
                    txtDescripcion.Text = ft.muestraCampoTexto(.Item("DESCRIPCION"))
                    txtCodigoArticulo.Text = ft.muestraCampoTexto(.Item("CODIGO_CONTENIDO"))
                    txtCapacidad.Text = ft.muestraCampoCantidad(.Item("CAPACIDAD"))
                    cmbUnidad.Text = ft.muestraCampoTexto(.Item("UNIDAD"))

                    txtTaraClientes.Text = ft.muestraCampoTexto(.Item("PESO"))
                    txtMaterial.Text = ft.muestraCampoTexto(.Item("MATERIAL"))

                    txtFechaAdquisicion.Text = ft.muestraCampoFecha(.Item("FECHA_ADQUISICION"))
                    txtFechaFabricacion.Text = ft.muestraCampoFecha(.Item("FECHA_FABRICACION"))
                    txtFechaRevision.Text = ft.muestraCampoFecha(.Item("FECHA_REVISION"))

                    txtNumeroRevision.Text = ft.muestraCampoEntero(.Item("REVISIONES"))

                    txtFabricante.Text = ft.muestraCampoTexto(.Item("FABRICANTE"))
                    'txtRevisor.Text = ft.muestraCampoTexto(.Item("REVISOR"))

                    lblProveedor.Text = ft.muestraCampoTexto(.Item("COMPRADOR"))
                    txtNumeroLote.Text = ft.muestraCampoTexto(.Item("NUM_LOTE"))

                    ft.RellenaCombo(aTipo, cmbTipoEnvase, .Item("TIPO"))
                    ft.RellenaCombo(aEstatus, cmbEstatus, .Item("ESTATUS"))

                    'Movimientos
                    txtCodigoMovimientos.Text = ft.muestraCampoTexto(.Item("CODENV"))
                    txtNombreMovimientos.Text = ft.muestraCampoTexto(.Item("DESCRIPCION"))

                End With
            End With
        End If

        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub AbrirMovimientos(ByVal CodigoEnvase As String)

        dg.DataSource = Nothing

        strSQLMov = " SELECT a.*, IF(b.nombre IS NULL, IF( c.nombre IS NULL, '', c.nombre) , b.nombre) nomProv_Cli, " _
            & " CONCAT(d.nombres, ' ' , d.apellidos) nomVendedor, elt(a.estatus + 1, " _
            & " 'Tránsito', 'Cliente', 'Proveedor', 'Vacío/Depósito', 'Lleno/Depósito', 'Reparación', 'Desincorporado', 'Indeterminado') nomEstatus " _
            & " from jsmertraenv a " _
            & " LEFT JOIN jsvencatcli b ON (a.prov_cli = b.codcli AND a.id_emp = b.id_emp AND a.origen IN ('FAC', 'PFC', 'NCV', 'NDV') ) " _
            & " LEFT JOIN jsprocatpro c ON (a.prov_cli = c.codpro AND a.id_emp = c.id_emp AND a.origen IN ('COM', 'REP', 'NCC' 'NDC') ) " _
            & " LEFT JOIN jsvencatven d ON (a.vendedor = d.codven AND a.id_emp = d.id_emp ) " _
            & " WHERE " _
            & " a.codenv = '" & CodigoEnvase & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY fechamov DESC "

        dtMovimientos = ft.AbrirDataTable(ds, nTablaMovimientos, myConn, strSQLMov)

        Dim aCampos() As String = {"fechamov.Emisión.80.C.fecha", _
                                   "tipomov.TP.35.C.", _
                                   "numdoc.Documento.100.I.", _
                                   "almacen.ALM.50.C.", _
                                   "cantidad.Cantidad.70.D.Entero", _
                                   "origen.ORG.35.C.", _
                                   "prov_cli.Prov/Clie.80.C.", _
                                   "nomProv_cli.Nombre o razón social.300.I.", _
                                   "vendedor.Asesor.50.C.", _
                                   "nomvendedor.Nombre.300.I.", _
                                   "nomEstatus.Estatus.200.I.", _
                                   "sada..10.I."}

        ft.IniciarTablaPlus(dg, dtMovimientos, aCampos)
        If dtMovimientos.Rows.Count > 0 Then nPosicionMov = 0

        ft.RellenaCombo(aTipoMovimiento, cmbTipoMovimiento)

    End Sub
    Private Sub IniciarEnvase(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = ft.autoCodigo(myConn, "CODENV", "jsmercatenv", "id_emp", jytsistema.WorkID, 8, True, "EV")
        Else
            txtCodigo.Text = ""
        End If

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtSerialInterno, txtSerialExterno, txtDescripcion, _
                          txtCodigoArticulo, txtMaterial, txtNombreMaterial, txtFabricante, txtRevisor, _
                          txtNumeroLote)

        'Try
        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, lblNombreFabricante, lblNombreProveedor, _
                                           lblProveedor, lblRevisor)
        'Catch ex As Exception
        '    If (Not ex.InnerException Is Nothing) Then
        '        ft.mensajeCritico(ex.InnerException.Message)
        '    End If
        'End Try

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cantidad, txtCapacidad, txtTaraClientes)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Fecha, txtFechaAdquisicion, txtFechaFabricacion, txtFechaRevision)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Entero, txtNumeroRevision)

        ft.RellenaCombo(aTipo, cmbTipoEnvase)
        ft.RellenaCombo(aEstatus, cmbEstatus, 1)
        ft.RellenaCombo(aUnidadAbreviada, cmbUnidad)

        dg.Columns.Clear()

    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True
        ft.habilitarObjetos(False, False, C1DockingTabPage2)
        ft.habilitarObjetos(True, True, txtDescripcion, btnCodigoArticulo, cmbEstatus)
        If i_modo = movimiento.iAgregar Then ft.habilitarObjetos(True, True, txtSerialInterno, txtSerialExterno, _
            txtCapacidad, cmbUnidad, txtTaraClientes, btnMaterial, btnFechaAdquisicion, btnFechaFabricacion, _
                            btnFechaRevision, btnFabricante, btnRevisor, txtNumeroLote, cmbTipoEnvase)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub

    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(True, False, C1DockingTabPage2)
        ft.habilitarObjetos(False, True, txtCodigo, txtCodigoMovimientos, txtSerialInterno, txtSerialExterno, _
                         txtDescripcion, txtNombreMovimientos, txtCodigoArticulo, btnCodigoArticulo, _
                         txtCapacidad, cmbUnidad, txtTaraClientes, txtMaterial, txtNombreMaterial, btnMaterial, _
                         txtFechaAdquisicion, btnFechaAdquisicion, txtFechaFabricacion, btnFechaFabricacion, txtFabricante, _
                         txtFechaRevision, btnFechaRevision, txtRevisor, txtNumeroRevision, txtNumeroLote, _
                         cmbTipoEnvase, cmbEstatus)

        MenuBarra.Enabled = True
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Select Case tbcEnvases.SelectedIndex
            Case 0 'Envases
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case 1 '"Movimientos"
                Me.BindingContext(ds, nTablaMovimientos).Position = 0
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End Select

    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Select Case tbcEnvases.SelectedIndex
            Case 0 'Envases
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case 1 'Movimientos"
                Me.BindingContext(ds, nTablaMovimientos).Position -= 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End Select

    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Select Case tbcEnvases.SelectedIndex
            Case 0 'Envases
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case 1 'Movimientos
                Me.BindingContext(ds, nTablaMovimientos).Position += 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End Select

    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click

        Select Case tbcEnvases.SelectedIndex
            Case 0 'Mercancías"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case 1 'Movimientos"
                Me.BindingContext(ds, nTablaMovimientos).Position = ds.Tables(nTablaMovimientos).Rows.Count - 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End Select

    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

        Select Case tbcEnvases.SelectedIndex
            Case 0 'Envases
                i_modo = movimiento.iAgregar
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
                IniciarEnvase(True)
            Case 1 'Movimientos"
                Dim f As New jsMerArcEnvasesMovimientos
                nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
                f.Agregar(myConn, ds, dtMovimientos, txtCodigoMovimientos.Text, txtNombreMovimientos.Text)
                dtMovimientos = ft.AbrirDataTable(ds, nTablaMovimientos, myConn, strSQLMov)
                If f.Documento <> "" Then
                    Dim row As DataRow = dtMovimientos.Select(" CODENV = '" & txtCodigo.Text & "' AND " _
                                                              & " NUMDOC = '" & f.Documento & "' AND " _
                                                              & " ORIGEN = 'INV' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
                    nPosicionMov = dtMovimientos.Rows.IndexOf(row)
                End If
                AsignaMov(nPosicionMov, True)
                AsignaTXT(nPosicionCat)
                f = Nothing
        End Select


    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Select Case tbcEnvases.SelectedIndex
            Case 0 'Envases
                i_modo = movimiento.iEditar
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
                cmbUnidad.Enabled = Not MercanciaPoseeMovimientos(myConn, lblInfo, txtCodigo.Text)
            Case 1 'Movimientos"
                If dtMovimientos.Rows(nPosicionMov).Item("ORIGEN") = "INV" Then
                    Dim f As New jsMerArcMercanciasMovimientos
                    f.Apuntador = nPosicionMov
                    f.Editar(myConn, ds, dtMovimientos, txtCodigoMovimientos.Text, txtNombreMovimientos.Text)
                    dtMovimientos = ft.AbrirDataTable(ds, nTablaMovimientos, myConn, strSQLMov)
                    dtMovimientos = ds.Tables(nTablaMovimientos)
                    If f.Documento <> "" Then
                        Dim row As DataRow = dtMovimientos.Select(" CODENV = '" & txtCodigo.Text & "' AND " _
                                                                  & " NUMDOC = '" & f.Documento & "' AND " _
                                                                  & " ORIGEN = 'INV'  AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
                        nPosicionMov = dtMovimientos.Rows.IndexOf(row)
                    End If
                    AsignaMov(nPosicionMov, True)
                    f = Nothing
                End If
        End Select


    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Select Case tbcEnvases.SelectedIndex
            Case 0 'Envases
                EliminaEnvase()
            Case 1 'Movimientos"
                EliminarMovimiento()
        End Select
    End Sub
    Private Sub EliminaEnvase()

        Dim aCamposDel() As String = {"CODENV", "ID_EMP"}
        Dim aStringsDel() As String = {txtCodigo.Text, jytsistema.WorkID}

        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
            If dtMovimientos.Rows.Count = 0 Then
                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsmercatenv", strSQL, aCamposDel, aStringsDel, _
                                              Me.BindingContext(ds, nTabla).Position, True))
            Else
                ft.mensajeCritico("Este ENVASE/CESTA posee movimientos. Verifique por favor ...")
            End If
        End If

    End Sub
    Private Sub EliminarMovimiento()

        If nPosicionMov >= 0 Then
            With dtMovimientos.Rows(nPosicionMov)
                If .Item("origen").ToString = "INV" Then

                    If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("numdoc"))

                        Dim aCamposDel() As String = {"CODENV", "NUMDOC", "ORIGEN", "EJERCICIO", "ID_EMP"}
                        Dim aFieldsDel() As String = {.Item("CODENV"), .Item("NUMDOC"), "INV", jytsistema.WorkExercise, jytsistema.WorkID}

                        nPosicionMov = EliminarRegistros(myConn, lblInfo, ds, nTablaMovimientos, "jsmertraenv", strSQLMov, aCamposDel, aFieldsDel, nPosicionMov)

                        AsignaTXT(nPosicionCat)
                        AsignaMov(nPosicionMov, False)

                    End If

                Else
                    ft.mensajeCritico("Movimiento proveniente de " & dtMovimientos.Rows(nPosicionMov).Item("origen") & ". ")
                End If
            End With

        End If

    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar

        Select Case tbcEnvases.SelectedIndex
            Case 0 'Envases
                Dim Campos() As String = {"CODENV", "DESCRIPCION", "SERIAL_1", "SERIAL_2"}
                Dim Nombres() As String = {"Código", "Descripción", "Serial Interno", "Serial Externo"}
                Dim Anchos() As Integer = {120, 650, 100, 120}
                f.Text = "Envases y/o Cestas"
                f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Envases...")
                nPosicionCat = f.Apuntador
                Me.BindingContext(ds, nTabla).Position = nPosicionCat
                AsignaTXT(nPosicionCat)
            Case "Movimientos"
                'Dim Campos() As String = {"fechamov", "numdoc", "prov_cli", "nomProv_Cli"}
                'Dim Nombres() As String = {"Emisión", "Nº Movimiento", "Código Prov./Cli.", "Cliente ó Proveedor"}
                'Dim Anchos() As Integer = {100, 120, 120, 500}
                'f.Text = "Movimientos de mercancía"
                'f.Buscar(dtMovimientos, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaMovimientos).Position, "Movimientos de mercancías...")
                'nPosicionMov = f.Apuntador
                'Me.BindingContext(ds, nTablaMovimientos).Position = nPosicionMov
                'AsignaMov(nPosicionMov, False)
        End Select

        f = Nothing

    End Sub


    Private Function Validado() As Boolean

        '/////// VALIDAR OMISION DE CAMPOS POR PETICION PARAMETRO 
        If CBool(ParametroPlus(myConn, Gestion.iMercancías, "MERENVPA01").ToString) Then
            If i_modo = movimiento.iAgregar Then
                If txtSerialInterno.Text = "" Then
                    ft.mensajeCritico("Debe indicar un Serial Interno válido...")
                    Return False
                Else
                    If ft.DevuelveScalarEntero(myConn, " select count(*) from jsmercatenv where serial_1 = '" & txtSerialInterno.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") > 0 Then
                        ft.mensajeCritico("Este Serial YA se encuentra en envases. Debe indicar un Serial Interno válido...")
                        Return False
                    End If
                End If
            End If
        End If

        If CBool(ParametroPlus(myConn, Gestion.iMercancías, "MERENVPA02")) Then
            If i_modo = movimiento.iAgregar Then
                If txtSerialExterno.Text = "" Then
                    ft.mensajeCritico("Debe indicar un Serial Externo válido...")
                    Return False
                Else
                    If ft.DevuelveScalarEntero(myConn, " select count(*) from jsmercatenv where serial_2 = '" & txtSerialExterno.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") > 0 Then
                        ft.mensajeCritico("Este Serial YA se encuentra en envases. Debe indicar un Serial Externo válido...")
                        Return False
                    End If
                End If
            End If
        End If

        If CBool(ParametroPlus(myConn, Gestion.iMercancías, "MERENVPA03")) Then
            If txtNombreMaterial.Text = "" Then
                ft.mensajeCritico("Debe el material de fabricación del envase...")
                Return False
            End If
        End If

        If ft.valorNumero(txtCapacidad.Text) <= 0 Then
            ft.mensajeCritico("Debe indicar capacidad del envase...")
            Return False
        End If

        If ft.valorNumero(txtTaraClientes.Text) <= 0 Then
            ft.mensajeCritico("Debe indicar un PESO Válido...")
            Return False
        End If

        Return True

    End Function
    Private Sub GuardarTXT()

        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then Inserta = True

        InsertEditMERCASEnvase(myConn, lblInfo, Inserta, txtCodigo.Text, txtSerialInterno.Text, txtSerialExterno.Text, _
                                txtCodigoArticulo.Text, txtDescripcion.Text, ft.valorCantidad(txtCapacidad.Text), cmbUnidad.Text, _
                                 Convert.ToDateTime(txtFechaRevision.Text), Convert.ToInt16(txtNumeroRevision.Text), _
                                 txtFabricante.Text, lblProveedor.Text, Convert.ToDateTime(txtFechaAdquisicion.Text), _
                                 txtNumeroLote.Text, Convert.ToDateTime(txtFechaFabricacion.Text), 0.0, 0, _
                                 ft.valorCantidad(txtTaraClientes.Text), txtMaterial.Text, cmbTipoEnvase.SelectedIndex, _
                                 cmbEstatus.SelectedIndex, 1)

        InsertarAuditoria(myConn, IIf(Inserta, MovAud.iIncluir, MovAud.imodificar), sModulo, txtCodigo.Text)

        dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

        Dim row As DataRow = dt.Select(" CODENV = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionCat = dt.Rows.IndexOf(row)
        Me.BindingContext(ds, nTabla).Position = nPosicionCat

        AsignaTXT(nPosicionCat)
        DesactivarMarco0()
        tbcEnvases.SelectedTab = C1DockingTabPage1
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtSerialInterno.GotFocus, txtSerialInterno.GotFocus, txtSerialExterno.GotFocus, txtDescripcion.GotFocus, _
           txtCapacidad.GotFocus, txtTaraClientes.GotFocus, txtNumeroLote.GotFocus

        Select Case sender.name
            Case "txtCodigo"
                ft.mensajeEtiqueta(lblInfo, " Indique el código del envase ... ", Transportables.tipoMensaje.iInfo)
            Case "txtSerialInterno"
                ft.mensajeEtiqueta(lblInfo, " Indique el Serial interno del envase  ... ", Transportables.tipoMensaje.iInfo)
            Case "txtSerialExterno"
                ft.mensajeEtiqueta(lblInfo, " Indique el Serial Externo del envase ...", Transportables.tipoMensaje.iInfo)
            Case "txtDescripcion"
                ft.mensajeEtiqueta(lblInfo, " Indique el nombre o descripción del envase ... ", Transportables.tipoMensaje.iInfo)
            Case "txtCapacidad"
                ft.mensajeEtiqueta(lblInfo, " Indique la capacidad del envase de acuerdo a la unidad  ... ", Transportables.tipoMensaje.iInfo)
            Case "txtPeso"
                ft.mensajeEtiqueta(lblInfo, " Indique el peso del envase ... ", Transportables.tipoMensaje.iInfo)
            Case "txtNumeroLote"
                ft.mensajeEtiqueta(lblInfo, " Indique el número de lote del envase ... ", Transportables.tipoMensaje.iInfo)
        End Select

    End Sub

    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dg.CellFormatting
        e.Value = ft.dataGridViewCellFormating(dg, e)
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
        dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        nPosicionMov = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
    End Sub

    Private Sub tbcMercas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcEnvases.SelectedIndexChanged
        C1SuperTooltip1.SetToolTip(btnRecalcular, "<B></B>")
        Select Case tbcEnvases.SelectedIndex
            Case 0 ' Mercancias
                AsignaTXT(nPosicionCat)
            Case 1 ' Movimientos
                C1SuperTooltip1.SetToolTip(btnRecalcular, "<B>Recalcular costos de mercancías</B>")
                AbrirMovimientos(txtCodigo.Text)
                nPosicionMov = 0

                AsignaMov(nPosicionMov, True)

                dg.Enabled = True


        End Select
    End Sub


    Private Sub AgregaYCancela()

        ft.Ejecutar_strSQL(myConn, " delete from jsmercatcom where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsmerequmer where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then AgregaYCancela()
        If dt.Rows.Count = 0 Then
            IniciarEnvase(False)
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



    Private Sub txtSugerido_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles _
         txtCapacidad.KeyPress

        e.Handled = ft.validaNumeroEnTextbox(e)

    End Sub



    Private Sub cmbTipoMovimiento_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipoMovimiento.SelectedIndexChanged

        Dim bs As New BindingSource
        Dim aTTipo() As String = {"", "EN", "SA", "AE", "AS", "AC"}
        bs.DataSource = dtMovimientos
        If dtMovimientos.Columns("tipomov").DataType Is GetType(String) Then _
          bs.Filter = " tipomov like '%" & aTTipo(cmbTipoMovimiento.SelectedIndex) & "%'"
        dg.DataSource = bs
        dg.Refresh()

    End Sub


    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(ds, nTablaMovimientos).Position += 1
                nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(nPosicionMov, False)
            Case Keys.Up
                Me.BindingContext(ds, nTablaMovimientos).Position -= 1
                nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(nPosicionMov, False)
        End Select
    End Sub

    Private Sub txtCapacidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles _
        txtCapacidad.KeyPress, txtTaraClientes.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub btnMaterial_Click(sender As Object, e As EventArgs) Handles btnMaterial.Click
        txtMaterial.Text = CargarTablaSimplePlusReal("MATERIAL DE ENVASES", Modulo.iMER_Materiales)
    End Sub

    Private Sub txtMaterial_TextChanged(sender As Object, e As EventArgs) Handles txtMaterial.TextChanged
        txtNombreMaterial.Text = ft.DevuelveScalarCadena(myConn, " select descrip from jsconctatab where codigo = '" & txtMaterial.Text & "' and modulo = '" & ft.FormatoTablaSimple(Modulo.iMER_Materiales) & "'  and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub
End Class