Imports MySql.Data.MySqlClient
Imports Microsoft.Win32
Imports fTransport
Public Class jsProArcFormulaciones
    Private Const sModulo As String = "Formulaciones"
    Private Const lRegion As String = "RibbonButton161"
    Private Const nTabla As String = "tblEncabFormulaciones"
    Private Const nTablaRenglones As String = "tblRenglones_Formulaciones"
    Private Const nTablaRenglonesResidual As String = "tblRenglones_FormulacionesResidual"
    Private Const nTablaCostosFijos As String = "tblCostosFijos"

    Private strSQL As String = " (select a.*, b.nomart from jsfabencfor a " _
            & " left join jsmerctainv b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
            & " where " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.codfor desc) order by codfor "

    Private strSQLMov As String = ""
    Private strSQLMovRes As String = ""
    Private strSQLCostosAdicionales As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtRenglones As New DataTable
    Private dtRenglonesResidual As New DataTable
    Private dtCostosFijos As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer

    Private nPosicionEncab As Long
    Private nPosicionRenglon As Long
    Private nPosicionRenglonResidual As Long
    Private nPosicionCostosAdicionales As Long

    Private aEstatus() As String = {"Inactiva", "Activa"}

    Private Eliminados As New ArrayList
    Private sDescripcion1 As String = ""

    Private Sub jsProArcFormulaciones_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsProArcFormulaciones_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()
            ds = DataSetRequeryPlus(ds, nTabla, myConn, strSQL)
            dt = ds.Tables(nTabla)

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionEncab = dt.Rows.Count - 1
                Me.BindingContext(ds, nTabla).Position = nPosicionEncab
                AsignaTXT(nPosicionEncab)
            Else
                IniciarDocumento(False)
            End If
            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva Fórmula")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> Fórmula")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> Fórmula")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> Fórmula")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primera</B> Fórmula")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a Fórmula <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a Fórmula <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>última Fórmula</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> Fórmula")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnDuplicar, "<B>Duplicar</B> este Fórmula")
        C1SuperTooltip1.SetToolTip(btnRecalcular, "<B>Recalculoar</B> esta Fórmula en base a nuevos costos")
        C1SuperTooltip1.SetToolTip(btnExplosion, "<B>Explotar</B> esta Fórmula en base a una cantidad deseada")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en Fórmula")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón en Fórmula")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón en Fórmula")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en Fórmula")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón en Fórmula")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón en Fórmula")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> en Fórmula")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la Fórmula")

        'Menu Barra Descuento 
        C1SuperTooltip1.SetToolTip(btnAgregaIndirecto, "<B>Agrega </B> Costo Indirecto a Fórmula")
        C1SuperTooltip1.SetToolTip(btnEliminaIndirecto, "<B>Elimina</B> Costo Indirecto de Fórmula")



    End Sub


    Private Sub AsignaTXT(ByVal nRow As Long)

        i_modo = movimiento.iConsultar

        If nRow >= 0 Then
            With dt

                nPosicionEncab = nRow
                Me.BindingContext(ds, nTabla).Position = nRow
                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

                With .Rows(nRow)
                    'Encabezado 
                    txtCodigo.Text = ft.muestraCampoTexto(.Item("CODFOR"))
                    txtEmision.Text = ft.muestraCampoFecha(.Item("FECHA").ToString)
                    txtMercancia.Text = ft.muestraCampoTexto(.Item("CODART"))
                    sDescripcion1 = .Item("DESCRIP_1")
                    txtNombreMercancia.Text = ft.muestraCampoTexto(.Item("DESCRIP_2"))
                    txtDescripProduccion.Text = ft.muestraCampoTexto(.Item("COMENTARIOS"))
                    txtCantidad.Text = ft.muestraCampoCantidad(.Item("CANTIDAD"))
                    lblUND.Text = ft.muestraCampoTexto(.Item("UNIDAD"))
                    txtAlmacen.Text = ft.muestraCampoTexto(.Item("ALMACEN_DESTINO"))

                    tslblPesoT.Text = ft.muestraCampoCantidad(.Item("PESO_TOTAL"))

                    ft.RellenaCombo(aEstatus, cmbEstatus, .Item("ESTATUS"))

                    txtCostosNetos.Text = ft.muestraCampoNumero(.Item("TOTAL_NETO"))
                    txtCostosIndirectos.Text = ft.muestraCampoNumero(.Item("TOTAL_INDIRECTOS"))
                    txtCostoTotal.Text = ft.muestraCampoNumero(.Item("TOTAL"))

                    'Renglones
                    AsignarMovimientos(.Item("codfor"))

                    'Costos Fijos
                    AsignarMovimientosFijos(.Item("codfor"))
                    'Totales
                    CalculaTotales()

                End With
            End With
        Else
            IniciarDocumento(False)
        End If

    End Sub
    Private Sub AsignarMovimientosFijos(ByVal CodigoFormula As String)

        strSQLCostosAdicionales = "select a.codfor, a.codcosto, b.titulo, b.porcentaje, b.montofijo " _
                            + " from jsfabfijfor a " _
                            + " left join jsfabcatfij b on (a.codcosto = b.codcosto and a.id_emp = b.id_emp )" _
                            & " where " _
                            & " a.codfor  = '" & CodigoFormula & "' and " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.codcosto "

        ds = DataSetRequeryPlus(ds, nTablaCostosFijos, myConn, strSQLCostosAdicionales)
        dtCostosFijos = ds.Tables(nTablaCostosFijos)

        Dim aCampos() As String = {"codcosto.Costo.50.I.", _
                                   "titulo.Descripción.150.I.", _
                                   "porcentaje.% Costo.70.D.Numero", _
                                   "montofijo.Importe fijo.100.D.Numero"}

        ft.IniciarTablaPlus(dgCostosAdicionales, dtCostosFijos, aCampos, , , New Font("Consolas", 7, FontStyle.Regular))
        If dtCostosFijos.Rows.Count > 0 Then
            nPosicionCostosAdicionales = 0
            MostrarFilaEnTabla(myConn, ds, nTablaCostosFijos, strSQLCostosAdicionales, Me.BindingContext, MenuDescuentos, dgCostosAdicionales, lRegion, jytsistema.sUsuario, nPosicionCostosAdicionales, True)
        End If

    End Sub

    Private Sub AsignarMovimientos(ByVal CodigoFormula As String)

        strSQLMov = "select * from jsfabrenfor " _
                            & " where " _
                            & " codfor  = '" & CodigoFormula & "' and " _
                            & " residual = 0 and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequeryPlus(ds, nTablaRenglones, myConn, strSQLMov)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"item.Item.90.I.", _
                                   "descrip.Descripción.350.I.", _
                                   "cantidad.Cantidad.100.D.Cantidad", _
                                   "unidad.UND.45.C.", _
                                   "Costo.Costo Unitario.70.D.Numero", _
                                   "totren.Costo Total.100.D.Numero", _
                                   "almacen_salida.Almacén Salida.70.C.", _
                                   "sada..100.C."}

        ft.IniciarTablaPlus(dg, dtRenglones, aCampos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, jytsistema.sUsuario, nPosicionRenglon, True)
        End If

    End Sub

    Private Sub AsignarMovimientosResidual(ByVal CodigoFormula As String)

        strSQLMovRes = "select * from jsfabrenfor " _
                            & " where " _
                            & " codfor  = '" & CodigoFormula & "' and " _
                            & " residual = 1 and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "


        ds = DataSetRequeryPlus(ds, nTablaRenglonesResidual, myConn, strSQLMovRes)
        dtRenglonesResidual = ds.Tables(nTablaRenglonesResidual)

        Dim aCampos() As String = {"item.Item.90.I.", _
                                   "descrip.Descripción.350.I.", _
                                   "cantidad.Cantidad.100.D.Cantidad", _
                                   "unidad.UND.45.C.", _
                                   "porcentaje.% Costo Total.70.D.Numero", _
                                   "almacen_salida.Almacén Entrada.70.C.", _
                                   "sada..100.C."}

        ft.IniciarTablaPlus(dgResidual, dtRenglonesResidual, aCampos)
        If dtRenglonesResidual.Rows.Count > 0 Then
            nPosicionRenglonResidual = 0
            MostrarFilaEnTabla(myConn, ds, nTablaRenglonesResidual, strSQLMovRes, Me.BindingContext, MenuBarraRenglon, dgResidual, _
                                  lRegion, jytsistema.sUsuario, nPosicionRenglonResidual, False)
        End If

    End Sub

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = ft.autoCodigo(myConn, "CODFOR", "jsfabencfor", "id_emp", jytsistema.WorkID, 10)
        Else
            txtCodigo.Text = ""
        End If

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtMercancia, txtNombreMercancia, txtDescripProduccion, _
                            txtAlmacen)

        Dim nAlmacen As String = ParametroPlus(myConn, Gestion.iProduccion, "PROPARAM01")
        If nAlmacen = "" Then nAlmacen = ft.DevuelveScalarCadena(myConn, "SELECT codalm FROM jsmercatalm WHERE id_emp = '" & jytsistema.WorkID & "' ORDER BY codalm LIMIT 1")
        If nAlmacen <> "" Then txtAlmacen.Text = ft.muestraCampoTexto(nAlmacen)

        ft.RellenaCombo(aEstatus, cmbEstatus, 1)
        txtEmision.Text = ft.muestraCampoFecha(sFechadeTrabajo)
        txtCantidad.Text = ft.muestraCampoCantidad(1)

        tslblPesoT.Text = ft.muestraCampoCantidad(0)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtCostosNetos, txtCostosIndirectos, txtCostoTotal)

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(txtCodigo.Text)
        AsignarMovimientosFijos(txtCodigo.Text)

    End Sub

    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon, MenuDescuentos)
        ft.habilitarObjetos(True, True, txtDescripProduccion, btnEmision, btnMercancia, txtMercancia, txtNombreMercancia, _
                         cmbEstatus, btnAlmacen, txtCantidad, btnAdjuntos)


        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtEmision, btnEmision, _
                txtMercancia, txtNombreMercancia, btnMercancia, txtDescripProduccion, _
                cmbEstatus, btnAlmacen, _
                txtNombreAlmacen, btnAdjuntos)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

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

        ft.Ejecutar_strSQL(myConn, " delete from jsfabrenfor where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsfabfijfor where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsfabadjfor where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub

    Private Function Validado() As Boolean


        If txtNombreMercancia.Text = "" Then
            ft.mensajeCritico("Debe indicar un producto válido...")
            Return False
        End If


        If dtRenglones.Rows.Count = 0 Then
            ft.mensajeCritico("Debe incluir al menos un ítem...")
            Return False
        End If


        Return True

    End Function
    Private Sub GuardarTXT()

        Dim Codigo As String = txtCodigo.Text
        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count
        End If

        InsertEditPRODUCCIONEncabezadoFormulas(myConn, lblInfo, Inserta, Codigo, txtMercancia.Text, sDescripcion1, txtNombreMercancia.Text, _
                                                ValorCantidad(txtCantidad.Text), lblUND.Text, ValorCantidad(tslblPesoT.Text), _
                                                txtAlmacen.Text, ValorNumero(txtCostosNetos.Text), ValorNumero(txtCostosIndirectos.Text), _
                                                ValorNumero(txtCostoTotal.Text), txtDescripProduccion.Text, Convert.ToDateTime(txtEmision.Text), _
                                                cmbEstatus.SelectedIndex)


        ds = DataSetRequeryPlus(ds, nTabla, myConn, strSQL)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" CODFOR = '" & Codigo & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub


    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescripProduccion.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique comentario ... ", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        If CBool(ParametroPlus(myConn, Gestion.iProduccion, "PROFOR0001")) Then
            
            i_modo = movimiento.iEditar
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position
            ActivarMarco0()
            ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtMercancia, btnMercancia)
            
        Else
            ft.mensajeCritico("Edición de Formulaciones NO está permitida...")
        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If CBool(ParametroPlus(myConn, Gestion.iProduccion, "PROFOR0002")) Then

            nPosicionEncab = Me.BindingContext(ds, nTabla).Position

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("codfor"))

                ft.Ejecutar_strSQL(myConn, " delete from jsfabfijfor where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myConn, " delete from jsfabadjfor where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myConn, " delete from jsfabrenfor where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myConn, " delete from jsfabencfor where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                dt = ds.Tables(nTabla)
                If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                AsignaTXT(nPosicionEncab)

            End If
        Else
            ft.mensajeCritico("Eliminación de Formulaciones NO está permitida...")
        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"codfor", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Nombre", "Emisión", "Comentario"}
        Dim Anchos() As Integer = {150, 400, 100, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Formulaciones...")
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

    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dg.CellFormatting
        'Dim aTipoRenglon() As String = {"Normal", "Sin Desc.", "Bonificación"}
        'Select Case e.ColumnIndex
        '    Case 10
        '        e.Value = aTipoRenglon(e.Value)
        'End Select
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)

    End Sub

    Private Sub CalculaTotales()

        txtCostosNetos.Text = ft.muestraCampoNumero(ft.DevuelveScalarDoble(myConn, " select sum(totren) from jsfabrenfor where codfor = '" + txtCodigo.Text + "' and residual = 0 and id_emp = '" + jytsistema.WorkID + "' group by codfor "))
        tslblPesoT.Text = ft.FormatoCantidad(ft.DevuelveScalarDoble(myConn, " select sum(peso_renglon) from jsfabrenfor where codfor = '" + txtCodigo.Text + "' and residual = 0 and id_emp = '" + jytsistema.WorkID + "' group by codfor "))

        Dim nCostosFijos As Double = 0.0
        For Each nnRow As DataRow In dtCostosFijos.Rows
            With nnRow
                If .Item("porcentaje") <> 0.0 Then
                    nCostosFijos += ValorNumero(txtCostosNetos.Text) * (.Item("porcentaje") / 100)
                Else
                    nCostosFijos += .Item("montofijo")
                End If
            End With
        Next
        txtCostosIndirectos.Text = ft.muestraCampoNumero(nCostosFijos)

        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtMercancia, btnMercancia)

    End Sub


    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        If txtNombreMercancia.Text <> "" Then

            Dim Residual As Boolean = IIf(tbcRenglones.SelectedTab.Text = "Residuales", True, False)
            Dim f As New jsProArcFormulacionesRenglones

            If Residual Then
                f.Apuntador = Me.BindingContext(ds, nTablaRenglonesResidual).Position
                f.Agregar(myConn, ds, dtRenglonesResidual, txtCodigo.Text, txtAlmacen.Text, , _
                          Residual)
                nPosicionRenglonResidual = f.Apuntador
                MostrarFilaEnTabla(myConn, ds, nTablaRenglonesResidual, strSQLMovRes, Me.BindingContext, MenuBarraRenglon, dgResidual, lRegion, _
                                      jytsistema.sUsuario, nPosicionRenglonResidual, True)
            Else
                f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
                f.Agregar(myConn, ds, dtRenglones, txtCodigo.Text, txtAlmacen.Text, , _
                          Residual)
                nPosicionRenglon = f.Apuntador
                MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, _
                                      jytsistema.sUsuario, nPosicionRenglon, True)
            End If

            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        If dtRenglones.Rows.Count > 0 Then
            'With dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position)
            '    Dim f As New jsProArcFormulacionesRenglones
            '    f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            '    f.Editar(myConn, ds, dtRenglones, "FAC", txtCodigo.Text, txtAlmacen.Text,
            '             IIf(.Item("item").ToString.Substring(0, 1) = "$", True, False))
            '    nPosicionRenglon = f.Apuntador
            '    MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, _
            '                          jytsistema.sUsuario, nPosicionRenglon, True)

            '    CalculaTotales()
            '    f = Nothing
            'End With
        End If

    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click

        EliminarMovimiento()

    End Sub
    Private Sub EliminarMovimiento()

        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                With dtRenglones.Rows(nPosicionRenglon)

                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("item"))

                    Eliminados.Add(.Item("item"))


                    'ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and " _
                    '       & " origen = 'FAC' and item = '" & .Item("item") & "' and renglon = '" & .Item("renglon") & "' and " _
                    '       & " id_emp = '" & jytsistema.WorkID & "' ")

                    Dim Residual As Boolean = IIf(tbcRenglones.SelectedTab.Text = "Residuales", True, False)

                    Dim aCamposDel() As String = {"codfor", "item", "renglon", "residual", "id_emp"}
                    Dim aStringsDel() As String = {txtCodigo.Text, .Item("item"), .Item("renglon"), IIf(Residual, 1, 0), jytsistema.WorkID}

                    If Residual Then
                        nPosicionRenglonResidual = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglonesResidual, "jsfabrenfor", _
                                                            strSQLMovRes, aCamposDel, aStringsDel, _
                                                            Me.BindingContext(ds, nTablaRenglonesResidual).Position, True)

                        If dtRenglonesResidual.Rows.Count - 1 < nPosicionRenglonResidual Then nPosicionRenglonResidual = dtRenglonesResidual.Rows.Count - 1

                        MostrarFilaEnTabla(myConn, ds, nTablaRenglonesResidual, strSQLMovRes, Me.BindingContext, MenuBarraRenglon, dgResidual, lRegion, _
                                             jytsistema.sUsuario, nPosicionRenglonResidual, True)
                    Else
                        nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsfabrenfor", _
                                                            strSQLMov, aCamposDel, aStringsDel, _
                                                            Me.BindingContext(ds, nTablaRenglones).Position, True)

                        If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1

                        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, _
                                              jytsistema.sUsuario, nPosicionRenglon, True)

                    End If

                    CalculaTotales()

                End With
            End If
        End If

    End Sub
    Private Sub btnBuscarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarMovimiento.Click

        Dim f As New frmBuscar
        Dim Campos() As String = {"item", "descripcion"}
        Dim Nombres() As String = {"Item", "Descripción"}
        Dim Anchos() As Integer = {140, 350}
        f.Text = "Movimientos "
        f.Buscar(dtRenglones, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaRenglones).Position, " " & Me.Tag & "...")
        nPosicionRenglon = f.Apuntador
        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, _
                              jytsistema.sUsuario, nPosicionRenglon, False)
        f = Nothing

    End Sub

    Private Sub btnPrimerMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimerMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position = 0
        nPosicionRenglon = 0
        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, _
                              jytsistema.sUsuario, nPosicionRenglon, False)
    End Sub

    Private Sub btnAnteriorMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnteriorMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position -= 1
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, _
                              jytsistema.sUsuario, nPosicionRenglon, False)
    End Sub

    Private Sub btnSiguienteMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguienteMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position += 1
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, _
                              jytsistema.sUsuario, nPosicionRenglon, False)
    End Sub

    Private Sub btnUltimoMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimoMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position = ds.Tables(nTablaRenglones).Rows.Count - 1
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, _
                              jytsistema.sUsuario, nPosicionRenglon, False)
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        'Imprimir()
    End Sub

    Private Sub btnEmision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmision.Click
        txtEmision.Text = SeleccionaFecha(CDate(txtEmision.Text), Me, sender)
    End Sub


    Private Sub txtMercancia_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMercancia.TextChanged
        If txtMercancia.Text <> "" Then
            If i_modo = movimiento.iAgregar Then
                txtNombreMercancia.Text = ft.DevuelveScalarCadena(myConn, " select nomart from jsmerctainv where codart = '" & txtMercancia.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                sDescripcion1 = txtNombreMercancia.Text
            End If

            lblUND.Text = ft.DevuelveScalarCadena(myConn, " select UNIDAD from jsmerctainv where codart = '" & txtMercancia.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Else
            ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNombreMercancia, lblUND)
        End If
    End Sub

    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs)
        If e.ColumnIndex = 1 Then e.Value = ft.FormatoNumero(e.Value) & "%"
    End Sub



    Private Sub btnMercancia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMercancia.Click

        txtMercancia.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codart codigo, nomart descripcion, " _
                                              & " elt(tipoart+1,  'Venta', 'Uso interno', 'POP', 'Alquiler', 'Préstamo', 'Materia prima', 'Venta & Envase', 'Otros') Tipo " _
                                              & " from jsmerctainv where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Mercancías", _
                                                txtMercancia.Text)

    End Sub


    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If txtNombreMercancia.Text <> "" Then
            'Dim f As New jsGenRenglonesMovimientos
            'f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            'f.Agregar(myConn, ds, dtRenglones, "FAC", txtCodigo.Text, CDate(txtEmision.Text), txtAlmacen.Text, cmbEstatus.Text, txtMercancia.Text, True)
            'ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            'nPosicionRenglon = f.Apuntador
            'MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, _
            '                      jytsistema.sUsuario, nPosicionRenglon, True)
            'CalculaTotales()
            'f = Nothing
        End If
    End Sub

    Private Sub txtAlmacen_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlmacen.TextChanged
        txtNombreAlmacen.Text = ft.DevuelveScalarCadena(myConn, " select desalm from jsmercatalm where codalm = '" & txtAlmacen.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub btnAlmacen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacen.Click
        txtAlmacen.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codalm codigo, desalm descripcion from jsmercatalm where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Almacenes", _
                                            txtAlmacen.Text)
    End Sub

    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(ds, nTablaRenglones).Position += 1
                nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
                MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, _
                                      jytsistema.sUsuario, nPosicionRenglon, False)
            Case Keys.Up
                Me.BindingContext(ds, nTablaRenglones).Position -= 1
                nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
                MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, _
                                      jytsistema.sUsuario, nPosicionRenglon, False)

        End Select
    End Sub

    Private Sub tbcRenglones_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles tbcRenglones.SelectedIndexChanged
        Select Case tbcRenglones.SelectedIndex
            Case 0 'FORMULA
                AsignarMovimientos(txtCodigo.Text)
            Case 1 'RESIDUALES
                AsignarMovimientosResidual(txtCodigo.Text)
        End Select
    End Sub

    Private Sub txtCostosNetos_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCostosNetos.TextChanged, _
        txtCostosIndirectos.TextChanged

        txtCostoTotal.Text = ft.muestraCampoNumero(ValorNumero(txtCostosNetos.Text) + ValorNumero(txtCostosIndirectos.Text))

    End Sub

    Private Sub btnAgregaIndirecto_Click(sender As System.Object, e As System.EventArgs) Handles btnAgregaIndirecto.Click
        Dim f As New jsProArcCostosFijos
        'f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        If f.Seleccionado <> "" Then
            ft.Ejecutar_strSQL(myConn, " replace into jsfabfijfor set codfor = '" + txtCodigo.Text + "', codcosto = '" + f.Seleccionado + "', id_emp = '" + jytsistema.WorkID + "'  ")
        End If
        ds = DataSetRequery(ds, strSQLCostosAdicionales, myConn, nTablaCostosFijos, lblInfo)
        MostrarFilaEnTabla(myConn, ds, nTablaCostosFijos, strSQLCostosAdicionales, Me.BindingContext, MenuDescuentos, dgCostosAdicionales, lRegion, _
                              jytsistema.sUsuario, nPosicionCostosAdicionales, True)
        CalculaTotales()
        f.Dispose()
        f = Nothing
    End Sub

    Private Sub btnDescripcion_Click(sender As System.Object, e As System.EventArgs) Handles btnDescripcion.Click
        If txtDescripProduccion.Text <> "" Then
            Dim g As New jsGenComentariosRenglones
            Dim aFld() As String = {"numdoc", "origen", "ITEM", "id_emp"}
            Dim aStr() As String = {txtCodigo.Text, "FOR", txtCodigo.Text, jytsistema.WorkID}

            If qFound(myConn, lblInfo, "jsvenrencom", aFld, aStr) Then
                g.Editar(myConn, txtCodigo.Text, "FOR", txtCodigo.Text, "00001")
            Else
                g.Agregar(myConn, txtCodigo.Text, "FOR", txtCodigo.Text, "00001")
            End If
            g = Nothing
        End If
    End Sub

    Private Sub btnAdjuntos_Click(sender As Object, e As EventArgs) Handles btnAdjuntos.Click
        Dim f As New jsGenTablaArchivos
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog, txtCodigo.Text, "PRO", "FOR")
        f.Dispose()
        f = Nothing
    End Sub

    
    Private Sub btnExplosion_Click(sender As Object, e As EventArgs) Handles btnExplosion.Click
        '/// EXPLOSIONAR EN UN REPORTE . TAL VEZ ?????
    End Sub
End Class