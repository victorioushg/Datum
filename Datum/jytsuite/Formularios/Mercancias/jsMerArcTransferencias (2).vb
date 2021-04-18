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

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long
    Private aTipo() As String = {"Transferencia", "Consumo Interno", "Desincorporaciones"}

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
            ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva tranaferencia ó consumo")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> transferencia ó consumo deseada")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primera</B> transferencia ó consumo")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a transferencia ó consumo <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a transrencia ó consumo <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>última  transferencia ó consumo</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en la transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón de la transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón de la transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en la transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón de la transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón de la transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> de la transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la transferencia ó consumo actual")


    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)

        If c >= 0 Then
            Me.BindingContext(ds, nTablaRenglones).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", c, dtRenglones.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtRenglones, MenuBarraRenglon)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtCodigo.Text = .Item("numtra")
                RellenaCombo(aTipo, cmbTipo, .Item("tipo"))
                txtComentario.Text = IIf(IsDBNull(.Item("comen")), "", .Item("comen"))
                txtEmision.Text = FormatoFecha(CDate(.Item("emision").ToString))
                txtAlmacenSalida.Text = .Item("alm_sale")
                txtAlmacenEntrada.Text = .Item("alm_entra")
                txtItems.Text = FormatoEntero(.Item("items"))

                txtTotalCantidad.Text = FormatoCantidad(.Item("totalcan"))
                txtTotalPeso.Text = FormatoCantidad(.Item("pesototal"))
                txtTotalCosto.Text = FormatoNumero(.Item("totaltra"))

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

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"item", "descrip", "unidad", "cantidad", "costou", "totren", ""}
        Dim aNombres() As String = {"Item", "Descripción", "Unidad", "Cantidad", "Costo Unitario", "Costo Total", ""}
        Dim aAnchos() As Long = {70, 380, 45, 80, 80, 80, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}
        Dim aFormatos() As String = {"", "", "", sFormatoCantidad, sFormatoNumero, sFormatoNumero, ""}
        IniciarTabla(dg, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub
    Private Sub IniciarTransferencia(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "TRTemp" & NumeroAleatorio(100000)
        Else
            txtCodigo.Text = ""
        End If

        RellenaCombo(aTipo, cmbTipo)
        txtComentario.Text = ""
        txtEmision.Text = FormatoFecha(sFechadeTrabajo)
        txtAlmacenSalida.Text = ""
        txtAlmacenEntrada.Text = ""
        txtItems.Text = FormatoEntero(0)

        txtTotalCantidad.Text = FormatoCantidad(0.0)
        txtTotalPeso.Text = FormatoCantidad(0.0)
        txtTotalCosto.Text = FormatoNumero(0.0)

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", 0, 0)
        AsignarMovimientos(txtCodigo.Text)


    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        HabilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon)
        HabilitarObjetos(True, True, txtComentario, btnEmision, btnAlmacenSalida, btnAlmacenEntrada, cmbTipo)
        HabilitarObjetos(False, False, txtTotalCantidad, txtTotalCosto, txtTotalPeso)

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()
        Dim c As Control
        grpAceptarSalir.Visible = False
        For Each c In grpEncab.Controls
            HabilitarObjetos(False, True, c)
        Next
        For Each c In grpTotales.Controls
            HabilitarObjetos(False, True, c)
        Next

        HabilitarObjetos(False, True, grpEncab, grpTotales)
        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then AgregaYCancela()

        If dt.Rows.Count = 0 Then
            IniciarTransferencia(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            AsignaTXT(nPosicionEncab)
        End If

        DesactivarMarco0()
    End Sub
    Private Sub AgregaYCancela()
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsmerrentra where numtra = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado() As Boolean

        If txtAlmacenEntrada.Text.Trim = txtAlmacenSalida.Text.Trim Then
            MensajeCritico(lblInfo, "ALMACEN DE SALIDA Y DE ENTRADA SON IGUALES...")
            Return False
        End If

        If cmbTipo.SelectedIndex = 0 AndAlso txtAlmacenSalida.Text.Trim = "" Then
            MensajeCritico(lblInfo, "DEBE INDICAR ALMACEN DE SALIDA VALIDO...")
            Return False
        End If

        If cmbTipo.SelectedIndex = 0 AndAlso txtAlmacenEntrada.Text.Trim = "" Then
            MensajeCritico(lblInfo, "DEBE INDICAR ALMACEN DE ENTRADA VALIDO...")
            Return False
        End If

        If cmbTipo.SelectedIndex = 1 AndAlso txtAlmacenSalida.Text.Trim = "" Then
            MensajeCritico(lblInfo, "DEBE INDICAR ALMACEN DE SALIDA VALIDO...")
            Return False
        End If

        Validado = True
    End Function
    Private Sub GuardarTXT()

        Dim CodigoTransferencia As String = txtCodigo.Text

        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count

            CodigoTransferencia = Contador(myConn, lblInfo, Gestion.iMercancías, "INVNUMTRA", "01")
            EjecutarSTRSQL(myConn, lblInfo, " update jsmerrentra set numtra = '" & CodigoTransferencia & "' where numtra = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        InsertEditMERCASEncabezadoTransferencia(myConn, lblInfo, Inserta, CodigoTransferencia, CDate(txtEmision.Text), txtAlmacenSalida.Text, txtAlmacenEntrada.Text, _
                                                 txtComentario.Text, ValorNumero(txtTotalCosto.Text), ValorCantidad(txtTotalCantidad.Text), _
                                                 ValorEntero(txtItems.Text), ValorCantidad(txtTotalPeso.Text), cmbTipo.SelectedIndex)

        ActualizarMovimientos(CodigoTransferencia)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        CalculaTotales()
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub CalculaTotales()

        txtTotalCantidad.Text = FormatoCantidad(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsmerrentra", "numtra", "cantidad", txtCodigo.Text))

        txtTotalPeso.Text = FormatoCantidad(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsmerrentra", "numtra", "peso", txtCodigo.Text))

        txtTotalCosto.Text = FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsmerrentra", "numtra", "totren", txtCodigo.Text))

        txtItems.Text = FormatoEntero(dtRenglones.Rows.Count)


    End Sub

    Private Sub ActualizarMovimientos(ByVal CodigoTransferencia As String)

        Dim rCont As Integer

        EjecutarSTRSQL(myConn, lblInfo, " delete from jsmertramer where " _
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
        MensajeEtiqueta(lblInfo, " Indique el nombre de la caja ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarTransferencia(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        i_modo = movimiento.iEditar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position

        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")

        If sRespuesta = MsgBoxResult.Yes Then

            InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numtra"))
            EjecutarSTRSQL(myConn, lblInfo, " delete from jsmerenctra where " _
                & " numtra = '" & txtCodigo.Text & "' AND " _
                & " ID_EMP = '" & jytsistema.WorkID & "'")

            EjecutarSTRSQL(myConn, lblInfo, " delete from jsmerrentra where numtra = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            EjecutarSTRSQL(myConn, lblInfo, " delete from jsmertramer where numdoc = '" & txtCodigo.Text & "' and origen = 'TRF' and id_emp = '" & jytsistema.WorkID & "' ")

            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)
            If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
            AsignaTXT(nPosicionEncab)

        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numtra", "emision", "comen"}
        Dim Nombres() As String = {"Número de transferencia", "Emisión", "Comentario"}
        Dim Anchos() As Long = {150, 100, 650}
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
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "ItemsRenglon", "lblItemsRenglon", e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
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
            MensajeAdvertencia(lblInfo, "Debe indicar un almacén de salida válido...")
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
            MensajeAdvertencia(lblInfo, "Debe indicar un almacén de salida válido...")
        End If
        CalculaTotales()
    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click

        EliminarMovimiento()

    End Sub
    Private Sub EliminarMovimiento()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then
            sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
            If sRespuesta = MsgBoxResult.Yes Then
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
        Dim Anchos() As Long = {140, 350}
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
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cTransferencia, "Transferencia/Nota de Consumo de mercancías", txtCodigo.Text)
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
        VisualizarObjetos(True, lblAlmacenEntrada, btnAlmacenEntrada, txtAlmacenEntrada, lblAlmacenSalida, btnAlmacenSalida, txtAlmacenSalida)
        Select Case cmbTipo.SelectedIndex
            Case 1, 2
                VisualizarObjetos(False, txtAlmacenEntrada, btnAlmacenEntrada, lblAlmacenEntrada)
            Case Else

        End Select
    End Sub
End Class