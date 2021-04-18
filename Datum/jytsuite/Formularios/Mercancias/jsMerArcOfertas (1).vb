Imports MySql.Data.MySqlClient
Public Class jsMerArcOfertas
    Private Const sModulo As String = "Ofertas de mercancia"
    Private Const lRegion As String = "RibbonButton142"
    Private Const nTabla As String = "tblOfertas"
    Private Const nTablaRenglones As String = "tblRenglonesOfertas"
    Private Const nTablaBonos As String = "tblBonos"

    Private strSQL As String = "select * from jsmerencofe where ID_EMP = '" & jytsistema.WorkID & "'  order by codofe "
    Private strSQLMov As String
    Private strSQLBono As String

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private myCom As New MySqlCommand(strSQL, myConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtRenglones As New DataTable
    Private dtBonos As New DataTable

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long, nPosicionBono As Long

    Private Sub jsMerArcOfertas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
                IniciarDocumento(False)
            End If
            ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva Oferta")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> Oferta")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> Oferta")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> Oferta")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primera</B> Oferta ")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a Oferta <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a Oferta <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>última</B> Oferta")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> Oferta")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en la Oferta")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón de la Oferta")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón de la Oferta")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en la Oferta")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón de la Oferta")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón de la Oferta")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> de la Oferta")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la Oferta")


    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)

        If c >= 0 AndAlso dtRenglones.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaRenglones).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", c, dtRenglones.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
            AsignarBonificaciones(txtOferta.Text, dtRenglones.Rows(c).Item("codart"))
        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtRenglones, MenuBarraRenglon)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt
            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

            If nRow >= 0 Then
                With .Rows(nRow)
                    'Encabezado 
                    txtOferta.Text = .Item("codofe")
                    txtComentario.Text = MuestraCampoTexto(.Item("descrip"), "")
                    txtFechaDesde.Text = FormatoFecha(CDate(.Item("desde").ToString))
                    txtFechaHasta.Text = FormatoFecha(CDate(.Item("hasta").ToString))
                    chkA.Checked = CBool(.Item("tarifa_a"))
                    chkB.Checked = CBool(.Item("tarifa_b"))
                    chkC.Checked = CBool(.Item("tarifa_c"))
                    chkD.Checked = CBool(.Item("tarifa_d"))
                    chkE.Checked = CBool(.Item("tarifa_e"))
                    chkF.Checked = CBool(.Item("tarifa_f"))

                    'Renglones
                    AsignarMovimientos(.Item("codofe"))

                End With
            End If

        End With
    End Sub
    Private Sub AsignarMovimientos(ByVal Codigo As String)

        strSQLMov = "select a.renglon, a.codart, a.descrip, a.unidad, a.limitei, a.limites, a.porcentaje, elt(a.otorgapor + 1,'Datum','Asesor') otorgapor  " _
                                    & " from jsmerrenofe a " _
                                    & " where " _
                                    & " a.codofe = '" & Codigo & "' and " _
                                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                    & " order by a.renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        txtItems.Text = FormatoEntero(dtRenglones.Rows.Count)

        Dim aCampos() As String = {"codart", "descrip", "limitei", "limites", "unidad", "porcentaje", "otorgapor", ""}
        Dim aNombres() As String = {"Código", "Descripción", "De...", "A...", "Unidad", "Porcentaje Descuento", "Otorgado Por...", ""}
        Dim aAnchos() As Long = {70, 300, 45, 90, 90, 70, 100, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", sFormatoCantidad, sFormatoCantidad, "", sFormatoNumero, "", ""}
        IniciarTabla(dg, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub
    Private Sub AsignarBonificaciones(ByVal CodigoOferta As String, ByVal CodigoArticulo As String)

        strSQLBono = "select a.codart, a.renglon, a.unidad, a.cantidad, a.cantidadbon, a.cantidadinicio, a.unidadbon, a.itembon,  " _
                                & " a.nombreitembon, a.codart codigoart, a.unidad unidadart, elt( a.otorgacan + 1, 'Datum','Asesor') otorgacan  " _
                                & " from jsmerrenbon a " _
                                & " where " _
                                & " a.codofe = '" & CodigoOferta & "' and " _
                                & " a.codart = '" & CodigoArticulo & "' and " _
                                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                & " order by a.renglon "

        ds = DataSetRequery(ds, strSQLBono, myConn, nTablaBonos, lblInfo)
        dtBonos = ds.Tables(nTablaBonos)

        Dim aCampos() As String = {"cantidad", "unidad", "codart", "cantidadbon", "unidadbon", "itembon", "nombreitembon", "cantidadinicio", "unidadart", "codigoart", "otorgacan", ""}
        Dim aNombres() As String = {"A partir de...", " ", "de...", "Se bonificara...", " ", " ", " ", "Por cada...", "", "de...", "Otorgado por...", ""}
        Dim aAnchos() As Long = {70, 40, 90, 70, 40, 90, 200, 70, 40, 90, 90, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {sFormatoCantidad, "", "", sFormatoCantidad, "", "", "", sFormatoCantidad, "", "", "", ""}
        IniciarTabla(dgBonos, dtBonos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtBonos.Rows.Count > 0 Then nPosicionBono = 0

    End Sub

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtOferta.Text = "OF" & NumeroAleatorio(10000000)
        Else
            txtOferta.Text = ""
        End If

        txtComentario.Text = ""
        txtFechaDesde.Text = FormatoFecha(sFechadeTrabajo)
        txtFechaHasta.Text = FormatoFecha(sFechadeTrabajo)
        txtItems.Text = FormatoEntero(0)
        chkA.Checked = True
        chkB.Checked = True
        chkC.Checked = True
        chkD.Checked = True
        chkE.Checked = True
        chkF.Checked = True

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", 0, 0)
        AsignarMovimientos(txtOferta.Text)


    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        HabilitarObjetos(True, False, grpEncab, MenuBarraRenglon)
        HabilitarObjetos(True, True, txtComentario, btnFechaDEsde, btnFechaHasta, chkA, chkB, chkC, chkD, chkE, chkF)

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()
        Dim c As Control
        grpAceptarSalir.Visible = False

        For Each c In grpEncab.Controls
            HabilitarObjetos(False, True, c)
        Next

        HabilitarObjetos(False, True, grpEncab)
        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then AgregaYCancela()

        If dt.Rows.Count = 0 Then
            IniciarDocumento(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            AsignaTXT(nPosicionEncab)
        End If

        DesactivarMarco0()
    End Sub
    Private Sub AgregaYCancela()
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsmerrenofe where codofe = '" & txtOferta.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsmerrenbon where codofe = '" & txtOferta.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
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

        Dim Codigo As String = txtOferta.Text
        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count

            Codigo = AutoCodigo(8, ds, nTabla, "codofe")
            EjecutarSTRSQL(myConn, lblInfo, " update jsmerrenofe set codofe = '" & Codigo & "' where codofe = '" & txtOferta.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            EjecutarSTRSQL(myConn, lblInfo, " update jsmerrenbon set codofe = '" & Codigo & "' where codofe = '" & txtOferta.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        InsertEditMERCASEncabezadoOferta(myConn, lblInfo, Inserta, Codigo, CDate(txtFechaDesde.Text), CDate(txtFechaHasta.Text), _
                                                txtComentario.Text, IIf(chkA.Checked, 1, 0), IIf(chkB.Checked, 1, 0), IIf(chkC.Checked, 1, 0), _
                                                IIf(chkD.Checked, 1, 0), IIf(chkE.Checked, 1, 0), IIf(chkF.Checked, 1, 0))


        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub txtComentario_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        MensajeEtiqueta(lblInfo, " Indique un comentario o descripción del proceso ... ", TipoMensaje.iAyuda)
        txtComentario.MaxLength = 150
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
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
            InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("codofe"))
            EjecutarSTRSQL(myConn, lblInfo, " delete from jsmerencofe where " _
                & " codofe = '" & txtOferta.Text & "' AND " _
                & " ID_EMP = '" & jytsistema.WorkID & "'")

            EjecutarSTRSQL(myConn, lblInfo, " delete from jsmerrenofe where codofe = '" & txtOferta.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            EjecutarSTRSQL(myConn, lblInfo, " delete from jsmerrenbon where codofe = '" & txtOferta.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)
            If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
            If nPosicionEncab >= 0 Then
                AsignaTXT(nPosicionEncab)
            Else
                IniciarDocumento(False)
            End If

        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"codofe", "descrip", "desde"}
        Dim Nombres() As String = {"Código Lista", "Descripción", "Fecha Desde"}
        Dim Anchos() As Long = {150, 350, 100}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Ofertas de mercancías...")
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
        nPosicionRenglon = e.RowIndex
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        'AsignarBonificaciones(txtOferta.Text, dtRenglones.Rows(nPosicionRenglon).Item("codart"))
        'MostrarItemsEnMenuBarra(MenuBarraRenglon, "ItemsRenglon", "lblItemsRenglon", e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
        AsignaMov(nPosicionRenglon, False)
    End Sub


    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        Dim f As New jsMerArcOfertasMovimiento
        f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
        f.Agregar(myConn, ds, dtRenglones, txtOferta.Text)
        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        AsignaMov(f.Apuntador, True)
        f = Nothing

    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click
        Dim f As New jsMerArcOfertasMovimiento
        f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
        f.Editar(myConn, ds, dtRenglones, txtOferta.Text)
        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        AsignaMov(f.Apuntador, True)
        f = Nothing
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
                With dtRenglones.Rows(nPosicionRenglon)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("codofe") & .Item("codart"))
                    Dim aCamposDel() As String = {"codofe", "codart", "id_emp"}
                    Dim aStringsDel() As String = {.Item("codofe"), .Item("codart"), jytsistema.WorkID}
                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsmerrenofe", strSQLMov, aCamposDel, aStringsDel, _
                                                  Me.BindingContext(ds, nTablaRenglones).Position, True)
                    nPosicionBono = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsmerrenbon", strSQLMov, aCamposDel, aStringsDel, _
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
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cOfertas, "Boletín promocional de ofertas", txtOferta.Text)
        f = Nothing
    End Sub

    Private Sub btnFechaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaDEsde.Click
        txtFechaDesde.Text = SeleccionaFecha(CDate(txtFechaDesde.Text), Me, sender)
    End Sub

    Private Sub btnFechaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaHasta.Click
        txtFechaHasta.Text = SeleccionaFecha(CDate(txtFechaHasta.Text), Me, sender)
    End Sub

    Private Sub dg_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub
End Class