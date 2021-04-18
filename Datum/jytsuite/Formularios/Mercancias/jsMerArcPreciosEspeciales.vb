Imports MySql.Data.MySqlClient
Public Class jsMerArcPreciosEspeciales
    Private Const sModulo As String = "Listas de precios especiales"
    Private Const lRegion As String = "RibbonButton141"
    Private Const nTabla As String = "tblLisprecios"
    Private Const nTablaRenglones As String = "tblRenglones_lisprecios"

    Private strSQL As String = "select * from jsmerenclispre where ID_EMP = '" & JytSistema.WorkID & "'  order by codlis "

    Private strSQLMov As String

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private myCom As New MySqlCommand(strSQL, myConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtRenglones As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long

    Private Sub jsMerArcPreciosEspeciales_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsMerArcPreciosEspeciales_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionEncab = dt.Rows.Count - 1
                Me.BindingContext(ds, nTabla).Position = nPosicionEncab
                AsignaTXT(nPosicionEncab)
            Else
                IniciarLista(False)
            End If

            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnPrimero, btnSiguiente, _
                          btnAnterior, btnUltimo, btnImprimir, btnSalir, btnAgregarMovimiento, btnEditarMovimiento, _
                          btnEliminarMovimiento, btnBuscarMovimiento, btnPrimerMovimiento, btnAnteriorMovimiento, btnSiguienteMovimiento, _
                          btnUltimoMovimiento)

    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)

        dtRenglones = ft.MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, _
                                             dg, lRegion, jytsistema.sUsuario, nRow, Actualiza)
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt
            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            If nRow >= 0 Then
                With .Rows(nRow)
                    'Encabezado 

                    txtLista.Text = ft.muestraCampoTexto(.Item("codlis"))
                    txtComentario.Text = ft.muestraCampoTexto(.Item("descrip"))
                    txtFechaDesde.Text = ft.muestraCampoFecha(.Item("emision").ToString)
                    txtFechaHasta.Text = ft.muestraCampoFecha(.Item("vence").ToString)

                    'Renglones
                    AsignarMovimientos(.Item("codlis"))

                End With
            End If

        End With
    End Sub
    Private Sub AsignarMovimientos(ByVal CodigoLista As String)
        strSQLMov = "select a.codart, b.nomart, b.unidad, a.precio " _
                                    & " from jsmerrenlispre a " _
                                    & " left join jsmerctainv b on (a.codart = b.codart and a.id_emp = b.id_emp ) " _
                                    & " where " _
                                    & " a.codlis = '" & CodigoLista & "' and " _
                                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                    & " order by a.codart "

        dtRenglones = ft.AbrirDataTable(ds, nTablaRenglones, myConn, strSQLMov)
        txtItems.Text = ft.FormatoEntero(dtRenglones.Rows.Count)

        Dim aCampos() As String = {"codart.Código.70.I.", "nomart.Descripción.300.I.", "unidad.Unidad.45.C.", "precio.Precio especial.120.D.Numero"}
        ft.IniciarTablaPlus(dg, dtRenglones, aCampos)

        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub
    Private Sub IniciarLista(ByVal Inicio As Boolean)

        If Inicio Then
            txtLista.Text = "LP" & ft.NumeroAleatorio(1000)
        Else
            txtLista.Text = ""
        End If

        txtComentario.Text = ""
        txtFechaDesde.Text = ft.FormatoFecha(sFechadeTrabajo)
        txtFechaHasta.Text = ft.FormatoFecha(sFechadeTrabajo)
        txtItems.Text = ft.FormatoEntero(0)

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(txtLista.Text)


    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, MenuBarraRenglon)
        ft.habilitarObjetos(True, True, txtComentario, btnFechaDEsde, btnFechaHasta)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()
        Dim c As Control
        grpAceptarSalir.Visible = False

        For Each c In grpEncab.Controls
            ft.habilitarObjetos(False, True, c)
        Next

        ft.habilitarObjetos(False, True, grpEncab)
        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then AgregaYCancela()

        If dt.Rows.Count = 0 Then
            IniciarLista(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            AsignaTXT(nPosicionEncab)
        End If

        DesactivarMarco0()
    End Sub
    Private Sub AgregaYCancela()
        ft.Ejecutar_strSQL(myconn, " delete from jsmerrenlispre where codlis = '" & txtLista.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado() As Boolean
        Validado = True
    End Function
    Private Sub GuardarTXT()

        Dim CodigoLista As String = txtLista.Text
        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count

            CodigoLista = ft.autoCodigo(myConn, "codlis", "jsmerenclispre", "id_emp", jytsistema.WorkID, 5)
            ft.Ejecutar_strSQL(myconn, " update jsmerrenlispre set codlis = '" & CodigoLista & "' where codlis = '" & txtLista.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        InsertEditMERCASEncabezadoListaPrecios(myConn, lblInfo, Inserta, CodigoLista, _
                                                CDate(txtFechaDesde.Text), txtComentario.Text, CDate(txtFechaHasta.Text))

        dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub txtComentario_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique un comentario o descripción del proceso ... ", Transportables.TipoMensaje.iAyuda)
        txtComentario.MaxLength = 50
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarLista(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        If dt.Rows.Count > 0 Then
            i_modo = movimiento.iEditar
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position
            ActivarMarco0()
        End If
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click


        If dt.Rows.Count > 0 Then
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("codlis"))
                ft.Ejecutar_strSQL(myConn, " delete from jsmerenclispre where " _
                    & " codlis = '" & txtLista.Text & "' AND " _
                    & " ID_EMP = '" & jytsistema.WorkID & "'")

                ft.Ejecutar_strSQL(myConn, " delete from jsmerrenlispre where codlis = '" & txtLista.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                dt = ds.Tables(nTabla)
                If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                If nPosicionEncab >= 0 Then
                    AsignaTXT(nPosicionEncab)
                Else
                    IniciarLista(False)
                End If

            End If
        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"codlis", "descrip", "emision"}
        Dim Nombres() As String = {"Código Lista", "Descripción", "Emision"}
        Dim Anchos() As Integer = {150, 350, 100}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Lista de precios especiales...")
        AsignaTXT(f.Apuntador)
        f = Nothing
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click

        If dt.Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = 0
            AsignaTXT(Me.BindingContext(ds, nTabla).Position)
        End If
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click

        If dt.Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position -= 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position)
        End If

    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click


        If dt.Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position += 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position)
        End If

    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click

        If dt.Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position)
        End If

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick

        If dt.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
            MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
        End If

    End Sub


    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click

        Dim f As New jsMerArcPreciosEspecialesMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
        f.Agregar(myConn, ds, dtRenglones, txtLista.Text)
        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        AsignaMov(f.Apuntador, True)
        f = Nothing

    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click
        Dim f As New jsMerArcPreciosEspecialesMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
        f.Editar(myConn, ds, dtRenglones, txtLista.Text)
        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        AsignaMov(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click
        EliminarMovimiento()
    End Sub
    Private Sub EliminarMovimiento()

        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                With dtRenglones.Rows(nPosicionRenglon)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("codart"))
                    Dim aCamposDel() As String = {"codlis", "codart", "id_emp"}
                    Dim aStringsDel() As String = {.Item("codlis"), .Item("codart"), jytsistema.WorkID}
                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsmerrenlispre", strSQLMov, aCamposDel, aStringsDel, _
                                                  Me.BindingContext(ds, nTablaRenglones).Position, True)

                    If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1
                    AsignaMov(nPosicionRenglon, True)
                End With

            End If
        End If

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
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cPreciosEspeciales, "Precios Especiales", txtLista.Text)
        f = Nothing
    End Sub

    Private Sub btnEmision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaDEsde.Click
        txtFechaDesde.Text = SeleccionaFecha(CDate(txtFechaDesde.Text), Me, sender)
    End Sub

    Private Sub btnFechaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaHasta.Click
        txtFechaHasta.Text = SeleccionaFecha(CDate(txtFechaHasta.Text), Me, sender)
    End Sub

End Class