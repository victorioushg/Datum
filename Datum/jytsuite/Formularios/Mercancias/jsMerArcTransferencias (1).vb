Imports MySql.Data.MySqlClient

Public Class jsMerArcTransferencias
    Private Const sModulo As String = "Transferencias y/o Consumos internos"
    Private Const lRegion As String = "RibbonButton137"
    Private Const nTabla As String = "tbltransfers"
    Private Const nTablaRenglones As String = "tblRenglones_transfers"

    Private strSQL As String = "select * from jsmerenctra where id_emp = '" & jytsistema.WorkID & "' order by numtra "
    Private strSQLMov As String

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private myCom As New MySqlCommand(strSQL, myConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtRenglones As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long
    Private aTipo() As String = {"Transferencia", "Consumo Interno", "Desincorporaciones"}

    Private Sub jsMerArcTransferencias_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub


    Private Sub jsMerArcTransferencia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)



            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionEncab = dt.Rows.Count - 1
                Me.BindingContext(ds, nTabla).Position = nPosicionEncab
                AsignaTXT(nPosicionEncab)
            Else
                IniciarTransferencia(False)
            End If
            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnPrimero, btnSiguiente, _
                          btnAnterior, btnUltimo, btnImprimir, btnSalir, btnAgregarMovimiento, btnEditarMovimiento, _
                          btnEliminarMovimiento, btnBuscarMovimiento, btnPrimerMovimiento, btnAnteriorMovimiento, _
                          btnSiguienteMovimiento, btnUltimoMovimiento)
    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        dtRenglones = ft.MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, _
                                              lRegion, jytsistema.sUsuario, nRow, Actualiza)
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtCodigo.Text = ft.muestraCampoTexto(.Item("numtra"))
                ft.RellenaCombo(aTipo, cmbTipo, .Item("tipo"))
                txtComentario.Text = ft.muestraCampoTexto(.Item("comen"))
                txtCausa.Text = ft.muestraCampoTexto(.Item("causa"))
                txtEmision.Text = ft.muestraCampoFecha(.Item("emision"))
                txtAlmacenSalida.Text = ft.muestraCampoTexto(.Item("alm_sale"))
                txtAlmacenEntrada.Text = ft.muestraCampoTexto(.Item("alm_entra"))
                txtItems.Text = ft.muestraCampoEntero(.Item("items"))

                txtTotalCantidad.Text = ft.muestraCampoCantidad(.Item("totalcan"))
                txtTotalPeso.Text = ft.muestraCampoCantidad(.Item("pesototal"))
                txtTotalCosto.Text = ft.muestraCampoNumero(.Item("totaltra"))

                'Renglones
                AsignarMovimientos(.Item("numtra"))

            End With
        End With
    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroTransferencia As String)
        strSQLMov = "select * from jsmerrentra " _
                            & " where " _
                            & " numtra  = '" & NumeroTransferencia & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "


        dtRenglones = ft.AbrirDataTable(ds, nTablaRenglones, myConn, strSQLMov)

        Dim aCampos() As String = {"item.Item.70.I.", _
                                   "descrip.Descripción.300.I.", _
                                   "unidad.Unidad.45.C.", _
                                   "cantidad.Cantidad.110.D.Cantidad", _
                                   "costou.Costo Unitario.110.D.Numero", _
                                   "totren.Costo Total.110.D.Numero", _
                                   "sada..100.I."}
        ft.IniciarTablaPlus(dg, dtRenglones, aCampos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub
    Private Sub IniciarTransferencia(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "TRTemp" & ft.NumeroAleatorio(100000)
        Else
            txtCodigo.Text = ""
        End If

        ft.RellenaCombo(aTipo, cmbTipo)
        txtComentario.Text = ""
        txtEmision.Text = ft.FormatoFecha(sFechadeTrabajo)
        txtAlmacenSalida.Text = ""
        txtAlmacenEntrada.Text = ""
        txtCausa.Text = ""
        txtItems.Text = ft.FormatoEntero(0)

        txtTotalCantidad.Text = ft.FormatoCantidad(0.0)
        txtTotalPeso.Text = ft.FormatoCantidad(0.0)
        txtTotalCosto.Text = ft.FormatoNumero(0.0)

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(txtCodigo.Text)


    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon)
        ft.habilitarObjetos(True, True, txtComentario, btnEmision, btnAlmacenSalida, btnAlmacenEntrada, cmbTipo, _
                         btnCausa)
        ft.habilitarObjetos(False, False, txtTotalCantidad, txtTotalCosto, txtTotalPeso)

        MenuBarra.Enabled = False

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()
        Dim c As Control
        grpAceptarSalir.Visible = False
        For Each c In grpEncab.Controls
            ft.habilitarObjetos(False, True, c)
        Next
        For Each c In grpTotales.Controls
            ft.habilitarObjetos(False, True, c)
        Next

        ft.habilitarObjetos(False, True, grpEncab, grpTotales)
        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then
            AgregaYCancela()
        Else
            If dtRenglones.Rows.Count = 0 Then
                ft.mensajeCritico("DEBE INCLUIR AL MENOS UN ITEM...")
                Return
            End If
        End If

        If dt.Rows.Count = 0 Then
            IniciarTransferencia(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            AsignaTXT(nPosicionEncab)
        End If

        DesactivarMarco0()
    End Sub
    Private Sub AgregaYCancela()
        ft.Ejecutar_strSQL(myConn, " delete from jsmerrentra where numtra = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado() As Boolean

        If FechaUltimoBloqueo(myConn, "jsmerenctra") >= Convert.ToDateTime(txtEmision.Text) Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If txtAlmacenEntrada.Text.Trim = txtAlmacenSalida.Text.Trim Then
            ft.mensajeCritico("ALMACEN DE SALIDA Y DE ENTRADA SON IGUALES...")
            Return False
        End If

        If cmbTipo.SelectedIndex = 0 AndAlso txtAlmacenSalida.Text.Trim = "" Then
            ft.mensajeCritico("DEBE INDICAR ALMACEN DE SALIDA VALIDO...")
            Return False
        End If

        If cmbTipo.SelectedIndex = 0 AndAlso txtAlmacenEntrada.Text.Trim = "" Then
            ft.mensajeCritico("DEBE INDICAR ALMACEN DE ENTRADA VALIDO...")
            Return False
        End If

        If cmbTipo.SelectedIndex = 1 AndAlso txtAlmacenSalida.Text.Trim = "" Then
            ft.mensajeCritico("DEBE INDICAR ALMACEN DE SALIDA VALIDO...")
            Return False
        End If

        If CBool(ParametroPlus(myConn, Gestion.iMercancías, "MERTRAPA03")) And _
            cmbTipo.SelectedIndex > 0 Then
            If txtCausa.Text.Trim = "" Then
                ft.mensajeCritico("Debe indicar una causa para " & cmbTipo.Text)
                Return False
            End If
        End If

        Return True

    End Function
    Private Sub GuardarTXT()

        Dim CodigoTransferencia As String = txtCodigo.Text

        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count

            CodigoTransferencia = Contador(myConn, lblInfo, Gestion.iMercancías, "INVNUMTRA", "01")
            ft.Ejecutar_strSQL(myconn, " update jsmerrentra set numtra = '" & CodigoTransferencia & "' where numtra = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        InsertEditMERCASEncabezadoTransferencia(myConn, lblInfo, Inserta, CodigoTransferencia, CDate(txtEmision.Text), txtAlmacenSalida.Text, txtAlmacenEntrada.Text, _
                                                 txtComentario.Text, txtCausa.Text, ValorNumero(txtTotalCosto.Text), ValorCantidad(txtTotalCantidad.Text), _
                                                 ValorEntero(txtItems.Text), ValorCantidad(txtTotalPeso.Text), cmbTipo.SelectedIndex)

        ActualizarMovimientos(CodigoTransferencia)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        CalculaTotales()
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

        ActualizarRenglonesEnPedidosAlmacen(myConn, lblInfo, ds, dtRenglones, "jsmerrenped")

    End Sub
    Private Sub CalculaTotales()

        txtTotalCantidad.Text = ft.FormatoCantidad(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsmerrentra", "numtra", "cantidad", txtCodigo.Text))

        txtTotalPeso.Text = ft.FormatoCantidad(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsmerrentra", "numtra", "peso", txtCodigo.Text))

        txtTotalCosto.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsmerrentra", "numtra", "totren", txtCodigo.Text))

        txtItems.Text = ft.FormatoEntero(dtRenglones.Rows.Count)


    End Sub

    Private Sub ActualizarMovimientos(ByVal CodigoTransferencia As String)

        Dim rCont As Integer

        ft.Ejecutar_strSQL(myConn, " delete from jsmertramer where " _
           & " numorg = '" & CodigoTransferencia & "' and " _
           & " origen = 'TRF' and " _
           & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
           & " id_emp = '" & jytsistema.WorkID & "'")


        For rCont = 0 To dtRenglones.Rows.Count - 1

            Dim strCausa As String = IIf(cmbTipo.SelectedIndex = 0, "TRANSFER.", IIf(cmbTipo.SelectedIndex = 1, "CONSUMO", "DESINCORP."))
            With dtRenglones.Rows(rCont)

                InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(txtEmision.Text), _
                    "SA", CodigoTransferencia, .Item("unidad"), .Item("cantidad"), .Item("peso"), _
                    .Item("totren"), .Item("totren"), "TRF", CodigoTransferencia, .Item("lote"), _
                   strCausa, 0.0, 0.0, 0.0, 0.0, "", txtAlmacenSalida.Text, .Item("renglon"), jytsistema.sFechadeTrabajo)

                If cmbTipo.SelectedIndex = 0 Then _
                    InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(txtEmision.Text), _
                        "EN", CodigoTransferencia, .Item("unidad"), .Item("cantidad"), .Item("peso"), _
                        .Item("totren"), .Item("totren"), "TRF", CodigoTransferencia, .Item("lote"), _
                         strCausa, 0.0, 0.0, 0.0, 0.0, "", txtAlmacenEntrada.Text, .Item("renglon"), jytsistema.sFechadeTrabajo)

            End With
        Next

    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique el nombre de la caja ... ", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarTransferencia(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        With dt.Rows(nPosicionEncab)
            Dim aCamposAdicionales() As String = {"numtra|'" & txtCodigo.Text & "'"}
            If DocumentoBloqueado(myConn, "jsmerenctra", aCamposAdicionales) Then
                ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
            Else
                If CBool(ParametroPlus(myConn, Gestion.iMercancías, "MERTRAPA02")) Then
                    i_modo = movimiento.iEditar
                    nPosicionEncab = Me.BindingContext(ds, nTabla).Position
                    ActivarMarco0()
                Else
                    ft.mensajeCritico("NO es posible MODIFICAR Transferencias de mercancías ")
                End If
            End If
        End With

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click


        If CBool(ParametroPlus(myConn, Gestion.iMercancías, "MERTRAPA02")) Then

            nPosicionEncab = Me.BindingContext(ds, nTabla).Position
            With dt.Rows(nPosicionEncab)
                Dim aCamposAdicionales() As String = {"numtra|'" & txtCodigo.Text & "'"}
                If DocumentoBloqueado(myConn, "jsmerenctra", aCamposAdicionales) Then
                    ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                Else
                    If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numtra"))
                        ft.Ejecutar_strSQL(myConn, " delete from jsmerenctra where " _
                            & " numtra = '" & txtCodigo.Text & "' AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'")

                        ft.Ejecutar_strSQL(myConn, " delete from jsmerrentra where numtra = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        ft.Ejecutar_strSQL(myConn, " delete from jsmertramer where numdoc = '" & txtCodigo.Text & "' and origen = 'TRF' and id_emp = '" & jytsistema.WorkID & "' ")

                        ds = DataSetRequeryPlus(ds, nTabla, myConn, strSQL)
                        dt = ds.Tables(nTabla)
                        If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                        AsignaTXT(nPosicionEncab)
                    End If
                End If
            End With
        Else
            ft.mensajeCritico("NO es posible ELIMINAR Transferencias de mercancías ")
        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numtra", "emision", "comen"}
        Dim Nombres() As String = {"Número de transferencia", "Emisión", "Comentario"}
        Dim Anchos() As Integer = {150, 100, 650}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Transferencias...")
        AsignaTXT(f.Apuntador)
        f = Nothing
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

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        If txtAlmacenSalida.Text <> "" Then

            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "TRF", txtCodigo.Text, CDate(txtEmision.Text), txtAlmacenSalida.Text)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            f = Nothing
        Else
            ft.mensajeAdvertencia("Debe indicar un almacén de salida válido...")
        End If
        CalculaTotales()
    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        If txtAlmacenSalida.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Editar(myConn, ds, dtRenglones, "TRF", txtCodigo.Text, CDate(txtEmision.Text), txtAlmacenSalida.Text)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            f = Nothing
        Else
            ft.mensajeAdvertencia("Debe indicar un almacén de salida válido...")
        End If
        CalculaTotales()
    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click

        EliminarMovimiento()

    End Sub
    Private Sub EliminarMovimiento()

        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dtRenglones.Rows(nPosicionRenglon).Item("item"))
                Dim aCamposDel() As String = {"numtra", "item", "renglon", "id_emp"}
                Dim aStringsDel() As String = {txtCodigo.Text, dtRenglones.Rows(nPosicionRenglon).Item("item"), dtRenglones.Rows(nPosicionRenglon).Item("renglon"), jytsistema.WorkID}

                nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsmerrentra", strSQLMov, aCamposDel, aStringsDel, _
                                              Me.BindingContext(ds, nTablaRenglones).Position, True)

                If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1
                AsignaMov(nPosicionRenglon, True)

            End If
        End If
        CalculaTotales()

    End Sub
    Private Sub btnBuscarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarMovimiento.Click

        Dim f As New frmBuscar
        Dim Campos() As String = {"item", "descripcion"}
        Dim Nombres() As String = {"Item", "Descripción"}
        Dim Anchos() As Integer = {140, 350}
        f.Text = "Movimientos de transferencia"
        f.Buscar(dtRenglones, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaRenglones).Position, " " & Me.Tag & "...")
        AsignaMov(f.Apuntador, False)
        f = Nothing

    End Sub

    Private Sub btnPrimerMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimerMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position = 0
        AsignaMov(Me.BindingContext(ds, nTablaRenglones).Position, False)
    End Sub

    Private Sub btnAnteriorMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnteriorMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position -= 1
        AsignaMov(Me.BindingContext(ds, nTablaRenglones).Position, False)
    End Sub

    Private Sub btnSiguienteMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguienteMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position += 1
        AsignaMov(Me.BindingContext(ds, nTablaRenglones).Position, False)
    End Sub

    Private Sub btnUltimoMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimoMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position = ds.Tables(nTablaRenglones).Rows.Count - 1
        AsignaMov(Me.BindingContext(ds, nTablaRenglones).Position, False)
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cTransferencia, cmbTipo.Text.ToUpper(), txtCodigo.Text)
        '        f = Nothing
    End Sub

    Private Sub btnEmision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmision.Click
        txtEmision.Text = SeleccionaFecha(CDate(txtEmision.Text), Me, sender)
    End Sub

    Private Sub btnAlmacenSalida_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacenSalida.Click
        Dim f As New jsControlArcAlmacenes
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        txtAlmacenSalida.Text = f.Seleccionado
    End Sub

    Private Sub btnAlmacenEntrada_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacenEntrada.Click

        If txtAlmacenSalida.Text <> "" Then
        Else
            Dim f As New jsControlArcAlmacenes
            f.Cargar(myConn, TipoCargaFormulario.iShowDialog, "'" & txtAlmacenSalida.Text & "'")
            txtAlmacenEntrada.Text = f.Seleccionado
            f.Dispose()
            f = Nothing
        End If

    End Sub

    Private Sub cmbTipo_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbTipo.SelectedIndexChanged
        ft.visualizarObjetos(True, lblAlmacenEntrada, btnAlmacenEntrada, txtAlmacenEntrada, lblAlmacenSalida, btnAlmacenSalida, _
                          txtAlmacenSalida, lblCausa, txtCausa, btnCausa)
        Select Case cmbTipo.SelectedIndex
            Case 0
                ft.visualizarObjetos(False, lblCausa, txtCausa, btnCausa)
            Case 1, 2
                ft.visualizarObjetos(False, txtAlmacenEntrada, btnAlmacenEntrada, lblAlmacenEntrada)
            Case Else

        End Select
    End Sub

    Private Sub ToolStripButton1_Click(sender As System.Object, e As System.EventArgs) Handles btnTraerPedidos.Click
        If txtCodigo.Text <> "" Then

            Dim f As New jsGenListadoSeleccion
            Dim aNombres() As String = {"", "Pedido N°", "Emisión", "Monto"}
            Dim aCampos() As String = {"sel", "codigo", "emision", "tot_ped"}
            Dim aAnchos() As Integer = {20, 120, 120, 200}
            Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Right}
            Dim aFormato() As String = {"", "", sFormatoFecha, sFormatoNumero}
            Dim aFields() As String = {"sel.entero.1.0", "CODIGO.cadena.20.0", "emision.fecha.0.0", "tot_ord.doble.19.2"}

            Dim str As String = "  select 0 sel, numped CODIGO, emision, tot_ped from jsmerencped where " _
                    & " alm_origen = '" & txtAlmacenSalida.Text & "' AND " _
                    & " alm_destino = '" & txtAlmacenEntrada.Text & "' AND " _
                    & " ESTATUS < '1' AND " _
                    & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND" _
                    & " ID_EMP = '" & jytsistema.WorkID & "' order by numped desc "

            f.Cargar(myConn, ds, "Pedidos entre almacenes en tránsito", str, _
                aFields, aNombres, aCampos, aAnchos, aAlineacion, aFormato)

            If f.Seleccion.Length > 0 Then
                Dim cod As String
                For Each cod In f.Seleccion
                    Dim nTablaRenglonesOrden As String = "tblRenglonesOrden" & ft.NumeroAleatorio(100000)
                    Dim dtRenglonesOrden As New DataTable
                    ds = DataSetRequery(ds, " select * from jsmerrenped where numped = '" & cod & "' and id_emp = '" & jytsistema.WorkID & "' order by renglon ", myConn, nTablaRenglonesOrden, lblInfo)
                    dtRenglonesOrden = ds.Tables(nTablaRenglonesOrden)

                    For Each rRow As DataRow In dtRenglonesOrden.Rows
                        With rRow

                            Dim pesoTransito As Double = .Item("peso") / .Item("cantidad") * .Item("cantran")

                            Dim numRenglon As String = ft.autoCodigo(myConn, "renglon", "jsmerrentra", "numtra.id_emp", txtCodigo.Text + "." + jytsistema.WorkID, 5)
                            InsertEditMERCASRenglonTransferencia(myConn, lblInfo, True, txtCodigo.Text, numRenglon, _
                                .Item("item"), .Item("descrip"), .Item("unidad"), .Item("cantran"), pesoTransito, .Item("lote"), _
                                 .Item("costou"), .Item("costou") * .Item("cantran"), "1")

                        End With

                        AsignarMovimientos(txtCodigo.Text)
                        CalculaTotales()
                    Next
                    dtRenglonesOrden.Dispose()
                    dtRenglonesOrden = Nothing
                Next

            End If
            f = Nothing
        End If

    End Sub

    Private Sub btnCausa_Click(sender As Object, e As EventArgs) Handles btnCausa.Click
        txtCausa.Text = CargarTablaSimplePlusReal("Causas AutoConsumo/Desincorporación", Modulo.iCausaDesincorporaciones)
    End Sub
End Class