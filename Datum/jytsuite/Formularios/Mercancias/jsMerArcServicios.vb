Imports MySql.Data.MySqlClient
Imports C1.Win.C1Chart
Public Class jsMerArcServicios
    Private Const sModulo As String = "Servicios"
    Private Const nTabla As String = "Servicios"
    Private Const lRegion As String = "RibbonButton143"
    Private Const nTablaMovimientos As String = "movimientos"
    Private Const nTablaCompras As String = "compras"
    Private Const nTablaVentas As String = "ventas"

    Private strSQL As String = "select * from jsmercatser where tipo = '0' AND id_emp = '" & jytsistema.WorkID & "' order by codser "
    Private strSQLMov As String
    Private strSQLCompras As String
    Private strSQLVentas As String

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtMovimientos As New DataTable
    Private ft As New Transportables

    Private aTipoMovimiento() As String = {"Todos", "Entradas", "Salidas"}

    Private i_modo As Integer
    Private nPosicionCat As Long, nPosicionMov As Long

    Private BindingSource1 As New BindingSource
    Private FindField As String

    Private Sub jsMerArcServicios_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsMerArcServicios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()


            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)
            BindingSource1.DataSource = dt

            IniciarGrilla()
            AsignarTooltips()
            If dt.Rows.Count > 0 Then nPosicionCat = 0
            AsignaTXT(nPosicionCat, False)

            ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)

            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub IniciarGrilla()

        Dim aCampos() As String = {"codser", "desser", "tipoiva", "precio", "tiposervicio", "codcon", ""}
        Dim aNombres() As String = {"Código", "Descripción", "IVA", "Precio", "Tipo servicios", "Código Contable", ""}
        Dim aAnchos() As Integer = {80, 300, 30, 100, 70, 150, 70}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {"", "", "", sFormatoNumero, "", "", ""}

        IniciarTabla(dgServicios, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

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
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        dtMovimientos = ds.Tables(nTablaMovimientos)
        If c >= 0 AndAlso dtMovimientos.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaMovimientos).Position = c
            MostrarItemsEnMenuBarra(MenuBarra, c, dtMovimientos.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        ft.ActivarMenuBarra(myConn, ds, dtMovimientos, lRegion, MenuBarra, jytsistema.sUsuario)
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        IniciarGrilla()
        If c >= 0 AndAlso ds.Tables(nTabla).Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = c
            With dt
                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)
                txtCodigoMovimientos.Text = .Rows(c).Item("codser")
                txtNombreMovimientos.Text = .Rows(c).Item("desser")
                txtCodigoCompras.Text = .Rows(c).Item("codser")
                txtNombreCompras.Text = .Rows(c).Item("desser")
                txtCodigoVentas.Text = .Rows(c).Item("codser")
                txtNombreVentas.Text = .Rows(c).Item("desser")
            End With

            dgServicios.CurrentCell = dgServicios(0, c)
        End If
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
    End Sub

    Private Sub AbrirMovimientos(ByVal CodigoArticulo As String)

        dg.DataSource = Nothing

        strSQLMov = " SELECT a.item, b.emision fechamov, 'EN' tipomov, a.numcom numdoc, a.unidad, b.almacen, " _
                & " a.cantidad, a.costotot costotal, a.costototdes costotaldes, '' lote, 0.00 peso, 'COM' origen, b.codpro prov_cli, " _
                & " c.nombre nomprov_cli, b.asiento, b.fechasi, '' vendedor , " _
                & " '' nomvendedor " _
                & " FROM jsprorencom a " _
                & " LEFT JOIN jsproenccom b ON (a.numcom = b.numcom AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN jsprocatpro c ON (b.codpro = c.codpro AND b.id_emp = c.id_emp) " _
                & " WHERE " _
                & " a.item = '$" & CodigoArticulo & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " UNION SELECT a.item, b.emision fechamov, 'EN' tipomov, a.numgas numdoc, a.unidad, b.almacen, " _
                & " a.cantidad, a.costotot costotal, a.costototdes costotaldes, '' lote, 0.00 peso, 'GAS' origen, b.codpro prov_cli, " _
                & " c.nombre nomprov_cli, b.asiento, b.fechasi, '' vendedor , " _
                & " '' nomvendedor " _
                & " FROM jsprorengas a " _
                & " LEFT JOIN jsproencgas b ON (a.numgas = b.numgas AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN jsprocatpro c ON (b.codpro = c.codpro AND b.id_emp = c.id_emp) " _
                & " WHERE " _
                & " a.item = '$" & CodigoArticulo & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " UNION SELECT a.item, b.emision fechamov, 'SA' tipomov, a.numfac numdoc, a.unidad, b.almacen, " _
                & " a.cantidad, a.totren costotal, a.totrendes costotaldes, '' lote, 0.00 peso, 'FAC' origen, b.codcli prov_cli, " _
                & " c.nombre nomprov_cli, b.asiento, b.fechasi, b.codven vendedor , " _
                & " '' nomvendedor " _
                & " FROM jsvenrenfac a " _
                & " LEFT JOIN jsvenencfac b ON (a.numfac = b.numfac AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN jsvencatcli c ON (b.codcli = c.codcli AND b.id_emp = c.id_emp) " _
                & " WHERE " _
                & " a.item = '$" & CodigoArticulo & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " ORDER BY fechamov DESC, numdoc, tipomov "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        dtMovimientos = ds.Tables(nTablaMovimientos)
        Dim aCampos() As String = {"fechamov.Emisión.80.C.fecha", _
                                   "tipomov.TP.35.C.", _
                                   "numdoc.Documento.100.I.", _
                                   "unidad.UND.35.C.", _
                                   "almacen.ALM.50.C.", _
                                   "cantidad.Cantidad.100.D.Cantidad", _
                                   "costotal.Costo Total.120.D.Numero", _
                                   "costotaldes.Costo Total y Desc.120.D.Numero", _
                                   "peso.Peso.100.D.Cantidad", _
                                   "origen.ORG.35.C.", _
                                   "prov_cli.Prov/Clie.80.C.", _
                                   "nomProv_cli.Nombre o razón social.230.I.", _
                                   "vendedor.Asesor.50.C.", _
                                   "nomvendedor.Nombre.90.I."}

        ft.IniciarTablaPlus(dg, dtMovimientos, aCampos)
        If dtMovimientos.Rows.Count > 0 Then nPosicionMov = 0

        ft.RellenaCombo(aTipoMovimiento, cmbTipoMovimiento)

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Select Case tbcServicios.SelectedTab.Text
            Case "Servicios"
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
            Case "Movimientos"
                Me.BindingContext(ds, nTablaMovimientos).Position = 0
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Compras"
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
                AsignarCompras(txtCodigoCompras.Text)
            Case "Ventas"
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
                AsignarVentas(txtCodigoVentas.Text)
        End Select



    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Select Case tbcServicios.SelectedTab.Text
            Case "Servicios"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
            Case "Movimientos"
                Me.BindingContext(ds, nTablaMovimientos).Position -= 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Compras"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
                AsignarCompras(txtCodigoCompras.Text)
            Case "Ventas"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
                AsignarVentas(txtCodigoVentas.Text)
        End Select

    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Select Case tbcServicios.SelectedTab.Text
            Case "Servicios"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
            Case "Movimientos"
                Me.BindingContext(ds, nTablaMovimientos).Position += 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Compras"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
                AsignarCompras(txtCodigoCompras.Text)
            Case "Ventas"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
                AsignarVentas(txtCodigoVentas.Text)
        End Select

    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Select Case tbcServicios.SelectedTab.Text
            Case "Servicios"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
            Case "Movimientos"
                Me.BindingContext(ds, nTablaMovimientos).Position = ds.Tables(nTablaMovimientos).Rows.Count - 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Compras"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
                AsignarCompras(txtCodigoCompras.Text)
            Case "Ventas"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
                AsignarVentas(txtCodigoVentas.Text)
        End Select

    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

        Select Case tbcServicios.SelectedTab.Text
            Case "Servicios"
                Dim f As New jsMerArcServiciosMovimientos
                f.Apuntador = nPosicionCat
                f.Agregar(myConn, ds, dt)
                AsignaTXT(f.Apuntador, True)
                f = Nothing
        End Select


    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Select Case tbcServicios.SelectedTab.Text
            Case "Servicios"
                If dt.Rows.Count > 0 Then
                    Dim f As New jsMerArcServiciosMovimientos
                    f.Apuntador = nPosicionCat
                    f.Editar(myConn, ds, dt)
                    AsignaTXT(f.Apuntador, True)
                    f = Nothing
                End If
        End Select




    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Select Case tbcServicios.SelectedTab.Text
            Case "Servicios"
                EliminaServicio()
        End Select
    End Sub
    Private Sub EliminaServicio()

        Dim strCod As String = dt.Rows(nPosicionCat).Item("codser")

        Dim cuenta As Long = ft.DevuelveScalarEntero(myConn, " SELECT count(*) FROM jsprorencom a " _
                & " LEFT JOIN jsproenccom b ON (a.numcom = b.numcom AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN jsprocatpro c ON (b.codpro = c.codpro AND b.id_emp = c.id_emp) " _
                & " WHERE " _
                & " a.item = '$" & strCod & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' group by a.item  ")

        cuenta += ft.DevuelveScalarEntero(myConn, " SELECT count(*) FROM jsprorengas a " _
                & " LEFT JOIN jsproencgas b ON (a.numgas = b.numgas AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN jsprocatpro c ON (b.codpro = c.codpro AND b.id_emp = c.id_emp) " _
                & " WHERE " _
                & " a.item = '$" & strCod & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' group by a.item ")

        cuenta += ft.DevuelveScalarEntero(myConn, " SELECT count(*) FROM jsvenrenfac a " _
                & " LEFT JOIN jsvenencfac b ON (a.numfac = b.numfac AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN jsvencatcli c ON (b.codcli = c.codcli AND b.id_emp = c.id_emp) " _
                & " WHERE " _
                & " a.item = '$" & strCod & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' ")


        If cuenta = 0 Then

            Dim aCamposDel() As String = {"codser", "id_emp"}
            Dim aStringsDel() As String = {dt.Rows(nPosicionCat).Item("codser"), jytsistema.WorkID}

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsmercatser", strSQL, aCamposDel, aStringsDel, _
                                             nPosicionCat, True), True)
            End If
        Else
            ft.mensajeCritico("Este SERVICIO posee movimientos. Verifique por favor ...")
        End If

    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar

        Select Case tbcServicios.SelectedTab.Text
            Case "Servicios"
                Dim Campos() As String = {"codser", "desser"}
                Dim Nombres() As String = {"Código", "Descripción"}
                Dim Anchos() As Integer = {120, 850}

                f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Servicios")
                nPosicionCat = f.Apuntador
                Me.BindingContext(ds, nTabla).Position = nPosicionCat
                AsignaTXT(nPosicionCat, False)

        End Select

        f = Nothing


    End Sub


    Private Sub btnColumnas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsMerRepParametros
        Select Case tbcServicios.SelectedTab.Text
            Case "Servicios"
                f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cServicios, "Servicios")
            Case "Movimientos"
                f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cMovimientosServicios, "MOVIMIENTO DE SDEVICIO", txtCodigoMovimientos.Text)
        End Select
        f.Dispose()
        f = Nothing
    End Sub


    Private Function Validado() As Boolean
        Validado = True
    End Function
    Private Sub dgServicios_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgServicios.CellFormatting
        Me.BindingContext(ds, nTabla).Position = e.RowIndex

        Dim aT() As String = {"ISLR", "NORMAL"}

        If e.ColumnIndex = 4 Then
            If e.Value Then
                e.Value = aT(1)
            Else
                e.Value = aT(0)
            End If
        End If


    End Sub
    Private Sub dgServicios_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgServicios.RowHeaderMouseClick, _
        dgServicios.CellMouseClick, dgServicios.RegionChanged

        If e.RowIndex >= 0 Then
            Me.BindingContext(ds, nTabla).Position = e.RowIndex
            nPosicionCat = e.RowIndex

            With dt.Rows(nPosicionCat)
                txtCodigoMovimientos.Text = .Item("codser")
                txtNombreMovimientos.Text = .Item("desser")
                txtCodigoCompras.Text = .Item("codser")
                txtNombreCompras.Text = .Item("desser")
                txtCodigoVentas.Text = .Item("codser")
                txtNombreVentas.Text = .Item("desser")

                AbrirMovimientos(.Item("codser"))

            End With
        End If

    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
        dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        nPosicionMov = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
    End Sub

    Private Sub tbcMercas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcServicios.SelectedIndexChanged

        Select Case tbcServicios.SelectedIndex
            Case 0 ' Mercancias
                AsignaTXT(nPosicionCat, False)
            Case 1 ' Movimientos
                AbrirMovimientos(dt.Rows(nPosicionCat).Item("codser"))
                AsignaMov(nPosicionMov, True)
                dg.Enabled = True
            Case 2 ' Compras
                AsignarCompras(dt.Rows(nPosicionCat).Item("codser"))
                MostrarItemsEnMenuBarra(MenuBarra, nPosicionCat, ds.Tables(nTabla).Rows.Count)
            Case 3 ' Ventas
                AsignarVentas(dt.Rows(nPosicionCat).Item("codser"))
                MostrarItemsEnMenuBarra(MenuBarra, nPosicionCat, ds.Tables(nTabla).Rows.Count)
        End Select
    End Sub
    Private Sub AsignarCompras(ByVal nArticulo As String)

        ft.habilitarObjetos(False, True, txtCodigoCompras, txtNombreCompras, txtOrdenes, txtRecepciones, txtBackorder, _
            txtFechaUltimaCompr, txtUltimoProveedor, txtEntradas, txtCostoAcum, txtCostoAcumDesc, txtUltimoCosto, _
            txtDevolucionesCompras, txtCostoAcumDev, txtCostoAcumDevDesc, txtCostoPromedio, txtNombreUltimoProveedor)

        With dt.Rows(nPosicionCat)

            ''            txtOrdenes.Text = ft.FormatoCantidad(.Item("ORDENES"))
            ''          txtRecepciones.Text = ft.FormatoCantidad(.Item("RECEPCIONES"))
            ''        txtBackorder.Text = ft.FormatoCantidad(.Item("BACKORDER"))
            ''      txtFechaUltimaCompr.Text = ft.FormatoFecha(CDate(.Item("FECULTCOSTO").ToString))
            ''    txtUltimoProveedor.Text = IIf(IsDBNull(.Item("ULTIMOPROVEEDOR")), "", .Item("ultimoproveedor"))
            ''  txtEntradas.Text = ft.FormatoCantidad(.Item("ENTRADAS"))
            ''         txtCostoAcum.Text = ft.FormatoNumero(.Item("ACU_COS"))
            ''       txtCostoAcumDesc.Text = ft.FormatoNumero(.Item("ACU_COS_DES"))
            ''     txtUltimoCosto.Text = ft.FormatoNumero(.Item("montoultimacompra"))
            ''   txtDevolucionesCompras.Text = ft.FormatoCantidad(.Item("CREDITOSCOMPRAS"))
            ''        txtCostoAcumDev.Text = ft.FormatoNumero(.Item("ACU_COD"))
            ''      txtCostoAcumDevDesc.Text = ft.FormatoNumero(.Item("ACU_COD_DES"))
            ''    txtCostoPromedio.Text = ft.FormatoNumero(.Item("COSTO_PROM_DES"))

        End With

        rBtn1.Checked = True
        VerHistograma(c1Chart1, 0, txtCodigoCompras.Text, 0)

        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)


    End Sub
    Private Function ValoresMensuales(ByVal dtValores As DataTable) As Double()
        Dim aMes As Double() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        If dtValores.Rows.Count > 0 Then
            With dtValores.Rows(0)
                aMes(0) = CDbl(.Item("ENE"))
                aMes(1) = CDbl(.Item("FEB"))
                aMes(2) = CDbl(.Item("MAR"))
                aMes(3) = CDbl(.Item("ABR"))
                aMes(4) = CDbl(.Item("MAY"))
                aMes(5) = CDbl(.Item("JUN"))
                aMes(6) = CDbl(.Item("JUL"))
                aMes(7) = CDbl(.Item("AGO"))
                aMes(8) = CDbl(.Item("SEP"))
                aMes(9) = CDbl(.Item("OCT"))
                aMes(10) = CDbl(.Item("NOV"))
                aMes(11) = CDbl(.Item("DIC"))
            End With
        End If
        ValoresMensuales = aMes

    End Function
    Private Sub VerHistograma(ByVal Histograma As C1Chart, ByVal Compras_o_Ventas As Integer, ByVal nArticulo As String, ByVal CantidadKilosMoney As Integer)

        Dim Area As Area = Histograma.ChartArea
        Dim ax As Axis = Area.AxisX
        Dim ay As Axis = Area.AxisY

        ax.ValueLabels.Clear()
        ax.ValueLabels.Add(1, "Ene")
        ax.ValueLabels.Add(2, "Feb")
        ax.ValueLabels.Add(3, "Mar")
        ax.ValueLabels.Add(4, "Abr")
        ax.ValueLabels.Add(5, "May")
        ax.ValueLabels.Add(6, "Jun")
        ax.ValueLabels.Add(7, "Jul")
        ax.ValueLabels.Add(8, "Ago")
        ax.ValueLabels.Add(9, "Sep")
        ax.ValueLabels.Add(10, "Oct")
        ax.ValueLabels.Add(11, "Nov")
        ax.ValueLabels.Add(12, "Dic")

        Dim aaY As Double() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim abY As Double() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Histograma.ChartGroups(0).ChartData.SeriesList(0).Y.CopyDataIn(aaY)
        Histograma.ChartGroups(0).ChartData.SeriesList(1).Y.CopyDataIn(abY)

        Dim dtt As DataTable = IIf(Compras_o_Ventas = 0, TablaCompras(nArticulo, CantidadKilosMoney), _
                                   TablaVentas(nArticulo, CantidadKilosMoney))


        Dim dttAnteriores As DataTable = IIf(Compras_o_Ventas = 0, TablaCompras(nArticulo, CantidadKilosMoney, 1), _
                                             TablaVentas(nArticulo, CantidadKilosMoney, 1))


        aaY = ValoresMensuales(dtt)
        abY = ValoresMensuales(dttAnteriores)

        Dim aFFld() As String = {"id_emp"}
        Dim aSStr() As String = {jytsistema.WorkID}

        ay.Text = IIf(CantidadKilosMoney = 0, "Unidad de Venta", IIf(CantidadKilosMoney = 1, "Kilogramos", qFoundAndSign(myConn, lblInfo, "jsconctaemp", aFFld, aSStr, "moneda")))
        ax.Text = IIf(Compras_o_Ventas = 0, "Compras mes a mes", "Ventas mes a mes")

        Histograma.ChartGroups(0).ChartData.SeriesList(0).Y.CopyDataIn(aaY)
        Histograma.ChartGroups(0).ChartData.SeriesList(1).Y.CopyDataIn(abY)

        Histograma.ChartGroups(0).ChartData.SeriesList(0).Label = Year(jytsistema.sFechadeTrabajo)
        Histograma.ChartGroups(0).ChartData.SeriesList(1).Label = Year(jytsistema.sFechadeTrabajo) - 1

        dtt = Nothing
        dttAnteriores = Nothing


    End Sub

    Private Function TablaCompras(ByVal nArticulo As String, ByVal CantidadKilosMoney As Integer, Optional ByVal AñosAtras As Integer = 0) As DataTable

        Dim nTablaCompras As String = "tablacompras" + AñosAtras.ToString
        Dim strSQLCompras As String = ""
        Dim tblTemp As String = "tblTemp" & ft.NumeroAleatorio(10000)
        Select Case CantidadKilosMoney
            Case 0 ' Cantidad

                ft.Ejecutar_strSQL(myConn, " DROP TABLE IF EXISTS " & tblTemp & " ")
                ft.Ejecutar_strSQL(myConn, " CREATE TABLE " & tblTemp & " " _
                               & " SELECT " & nArticulo & " codart, SUM(IF(MONTH(b.emision) = 1, a.cantidad, 0.00 )) ENE, " _
                               & " SUM(IF(MONTH(b.emision) = 1, a.cantidad, 0.00 )) FEB, " _
                               & " SUM(IF(MONTH(b.emision) = 2, a.cantidad, 0.00 )) MAR, " _
                               & " SUM(IF(MONTH(b.emision) = 3, a.cantidad, 0.00 )) ABR, " _
                               & " SUM(IF(MONTH(b.emision) = 4, a.cantidad, 0.00 )) MAY, " _
                               & " SUM(IF(MONTH(b.emision) = 5, a.cantidad, 0.00 )) JUN, " _
                               & " SUM(IF(MONTH(b.emision) = 6, a.cantidad, 0.00 )) JUL, " _
                               & " SUM(IF(MONTH(b.emision) = 7, a.cantidad, 0.00 )) AGO, " _
                               & " SUM(IF(MONTH(b.emision) = 8, a.cantidad, 0.00 )) SEP, " _
                               & " SUM(IF(MONTH(b.emision) = 9, a.cantidad, 0.00 )) OCT, " _
                               & " SUM(IF(MONTH(b.emision) = 10, a.cantidad, 0.00 )) NOV, " _
                               & " SUM(IF(MONTH(b.emision) = 11, a.cantidad, 0.00 )) DIC " _
                               & " FROM jsprorencom a " _
                               & " LEFT JOIN jsproenccom b ON (a.numcom = b.numcom AND a.id_emp = b.id_emp) " _
                               & " LEFT JOIN jsprocatpro c ON (b.codpro = c.codpro AND b.id_emp = c.id_emp) " _
                               & " WHERE " _
                               & " a.item = '$" & nArticulo & "' AND " _
                               & " YEAR(b.emision) = " & Year(jytsistema.sFechadeTrabajo) - AñosAtras & " AND " _
                               & " a.id_emp = '" & jytsistema.WorkID & "'  " _
                               & " UNION SELECT " & nArticulo & " codart, SUM( IF (  MONTH(b.emision) = 1, a.cantidad, 0.00 )    ) ENE, " _
                               & " SUM(IF(MONTH(b.emision) = 1, a.cantidad, 0.00 )) FEB, " _
                               & " SUM(IF(MONTH(b.emision) = 2, a.cantidad, 0.00 )) MAR, " _
                               & " SUM(IF(MONTH(b.emision) = 3, a.cantidad, 0.00 )) ABR, " _
                               & " SUM(IF(MONTH(b.emision) = 4, a.cantidad, 0.00 )) MAY, " _
                               & " SUM(IF(MONTH(b.emision) = 5, a.cantidad, 0.00 )) JUN, " _
                               & " SUM(IF(MONTH(b.emision) = 6, a.cantidad, 0.00 )) JUL, " _
                               & " SUM(IF(MONTH(b.emision) = 7, a.cantidad, 0.00 )) AGO, " _
                               & " SUM(IF(MONTH(b.emision) = 8, a.cantidad, 0.00 )) SEP, " _
                               & " SUM(IF(MONTH(b.emision) = 9, a.cantidad, 0.00 )) OCT, " _
                               & " SUM(IF(MONTH(b.emision) = 10, a.cantidad, 0.00 )) NOV, " _
                               & " SUM(IF(MONTH(b.emision) = 11, a.cantidad, 0.00 )) DIC " _
                               & " FROM jsprorengas a " _
                               & " LEFT JOIN jsproencgas b ON (a.numgas = b.numgas AND a.id_emp = b.id_emp) " _
                               & " LEFT JOIN jsprocatpro c ON (b.codpro = c.codpro AND b.id_emp = c.id_emp) " _
                               & " WHERE " _
                               & " a.item = '$" & nArticulo & "' AND " _
                               & " YEAR(b.emision) = " & Year(jytsistema.sFechadeTrabajo) - AñosAtras & " AND " _
                               & " a.id_emp = '" & jytsistema.WorkID & "'  ")


                strSQLCompras = " select " & nArticulo & " CODART, " _
                & " IF(SUM(ENE) IS NULL, 0.00, SUM(ENE)) ENE, " _
                & " IF(SUM(FEB) IS NULL, 0.00, SUM(FEB)) FEB, " _
                & " IF(SUM(MAR) IS NULL, 0.00, SUM(MAR)) MAR, " _
                & " IF(SUM(ABR) IS NULL, 0.00, SUM(ABR)) ABR, " _
                & " IF(SUM(MAY) IS NULL, 0.00, SUM(MAY)) MAY, " _
                & " IF(SUM(JUN) IS NULL, 0.00, SUM(JUN)) JUN, " _
                & " IF(SUM(JUL) IS NULL, 0.00, SUM(JUL)) JUL, " _
                & " IF(SUM(AGO) IS NULL, 0.00, SUM(AGO)) AGO, " _
                & " IF(SUM(SEP) IS NULL, 0.00, SUM(SEP)) SEP, " _
                & " IF(SUM(OCT) IS NULL, 0.00, SUM(OCT)) OCT, " _
                & " IF(SUM(NOV) IS NULL, 0.00, SUM(NOV)) NOV, " _
                & " IF(SUM(DIC) IS NULL, 0.00, SUM(DIC)) DIC " _
                & " from " _
                & " " & tblTemp & " " _
                & " group by 1 "


            Case 1 ' Kilos
            Case Else ' Money
                ft.Ejecutar_strSQL(myConn, " DROP TABLE IF EXISTS " & tblTemp & " ")
                ft.Ejecutar_strSQL(myConn, " CREATE TABLE " & tblTemp & " " _
                               & " SELECT " & nArticulo & " codart, SUM(IF(MONTH(b.emision) = 1, a.cantidad, 0.00 )) ENE, " _
                               & " SUM(IF(MONTH(b.emision) = 1, a.costototdes, 0.00 )) FEB, " _
                               & " SUM(IF(MONTH(b.emision) = 2, a.costototdes, 0.00 )) MAR, " _
                               & " SUM(IF(MONTH(b.emision) = 3, a.costototdes, 0.00 )) ABR, " _
                               & " SUM(IF(MONTH(b.emision) = 4, a.costototdes, 0.00 )) MAY, " _
                               & " SUM(IF(MONTH(b.emision) = 5, a.costototdes, 0.00 )) JUN, " _
                               & " SUM(IF(MONTH(b.emision) = 6, a.costototdes, 0.00 )) JUL, " _
                               & " SUM(IF(MONTH(b.emision) = 7, a.costototdes, 0.00 )) AGO, " _
                               & " SUM(IF(MONTH(b.emision) = 8, a.costototdes, 0.00 )) SEP, " _
                               & " SUM(IF(MONTH(b.emision) = 9, a.costototdes, 0.00 )) OCT, " _
                               & " SUM(IF(MONTH(b.emision) = 10, a.costototdes, 0.00 )) NOV, " _
                               & " SUM(IF(MONTH(b.emision) = 11, a.costototdes, 0.00 )) DIC " _
                               & " FROM jsprorencom a " _
                               & " LEFT JOIN jsproenccom b ON (a.numcom = b.numcom AND a.id_emp = b.id_emp) " _
                               & " LEFT JOIN jsprocatpro c ON (b.codpro = c.codpro AND b.id_emp = c.id_emp) " _
                               & " WHERE " _
                               & " a.item = '$" & nArticulo & "' AND " _
                               & " YEAR(b.emision) = " & Year(jytsistema.sFechadeTrabajo) - AñosAtras & " AND " _
                               & " a.id_emp = '" & jytsistema.WorkID & "'  " _
                               & " UNION SELECT " & nArticulo & " codart, SUM( IF (  MONTH(b.emision) = 1, a.costototdes, 0.00 )    ) ENE, " _
                               & " SUM(IF(MONTH(b.emision) = 1, a.costototdes, 0.00 )) FEB, " _
                               & " SUM(IF(MONTH(b.emision) = 2, a.costototdes, 0.00 )) MAR, " _
                               & " SUM(IF(MONTH(b.emision) = 3, a.costototdes, 0.00 )) ABR, " _
                               & " SUM(IF(MONTH(b.emision) = 4, a.costototdes, 0.00 )) MAY, " _
                               & " SUM(IF(MONTH(b.emision) = 5, a.costototdes, 0.00 )) JUN, " _
                               & " SUM(IF(MONTH(b.emision) = 6, a.costototdes, 0.00 )) JUL, " _
                               & " SUM(IF(MONTH(b.emision) = 7, a.costototdes, 0.00 )) AGO, " _
                               & " SUM(IF(MONTH(b.emision) = 8, a.costototdes, 0.00 )) SEP, " _
                               & " SUM(IF(MONTH(b.emision) = 9, a.costototdes, 0.00 )) OCT, " _
                               & " SUM(IF(MONTH(b.emision) = 10, a.costototdes, 0.00 )) NOV, " _
                               & " SUM(IF(MONTH(b.emision) = 11, a.costototdes, 0.00 )) DIC " _
                               & " FROM jsprorengas a " _
                               & " LEFT JOIN jsproencgas b ON (a.numgas = b.numgas AND a.id_emp = b.id_emp) " _
                               & " LEFT JOIN jsprocatpro c ON (b.codpro = c.codpro AND b.id_emp = c.id_emp) " _
                               & " WHERE " _
                               & " a.item = '$" & nArticulo & "' AND " _
                               & " YEAR(b.emision) = " & Year(jytsistema.sFechadeTrabajo) - AñosAtras & " AND " _
                               & " a.id_emp = '" & jytsistema.WorkID & "'  ")


                strSQLCompras = " select " & nArticulo & " CODART, " _
                & " IF(SUM(ENE) IS NULL, 0.00, SUM(ENE)) ENE, " _
                & " IF(SUM(FEB) IS NULL, 0.00, SUM(FEB)) FEB, " _
                & " IF(SUM(MAR) IS NULL, 0.00, SUM(MAR)) MAR, " _
                & " IF(SUM(ABR) IS NULL, 0.00, SUM(ABR)) ABR, " _
                & " IF(SUM(MAY) IS NULL, 0.00, SUM(MAY)) MAY, " _
                & " IF(SUM(JUN) IS NULL, 0.00, SUM(JUN)) JUN, " _
                & " IF(SUM(JUL) IS NULL, 0.00, SUM(JUL)) JUL, " _
                & " IF(SUM(AGO) IS NULL, 0.00, SUM(AGO)) AGO, " _
                & " IF(SUM(SEP) IS NULL, 0.00, SUM(SEP)) SEP, " _
                & " IF(SUM(OCT) IS NULL, 0.00, SUM(OCT)) OCT, " _
                & " IF(SUM(NOV) IS NULL, 0.00, SUM(NOV)) NOV, " _
                & " IF(SUM(DIC) IS NULL, 0.00, SUM(DIC)) DIC " _
                & " from " _
                & " " & tblTemp & " " _
                & " group by 1 "

        End Select

        ds = DataSetRequery(ds, strSQLCompras, myConn, nTablaCompras, lblInfo)
        TablaCompras = ds.Tables(nTablaCompras)
        ft.Ejecutar_strSQL(myConn, " DROP TABLE IF EXISTS " & tblTemp & " ")

    End Function
    Private Function TablaVentas(ByVal nArticulo As String, ByVal CantidadKilosMoney As Integer, Optional ByVal AñosAtras As Integer = 0) As DataTable

        Dim nTablaVentas As String = "tablaventas" + AñosAtras.ToString
        Dim strSQLVentas As String = ""
        Dim tblTemp As String = "tblTemp" & ft.NumeroAleatorio(10000)

        Select Case CantidadKilosMoney
            Case 0 ' Cantidad
                ft.Ejecutar_strSQL(myConn, " DROP TABLE IF EXISTS " & tblTemp & " ")
                ft.Ejecutar_strSQL(myConn, " CREATE TABLE " & tblTemp & " " _
                               & " SELECT " & nArticulo & " codart, SUM(IF(MONTH(b.emision) = 1, a.cantidad, 0.00 )) ENE, " _
                               & " SUM(IF(MONTH(b.emision) = 1, a.cantidad, 0.00 )) FEB, " _
                               & " SUM(IF(MONTH(b.emision) = 2, a.cantidad, 0.00 )) MAR, " _
                               & " SUM(IF(MONTH(b.emision) = 3, a.cantidad, 0.00 )) ABR, " _
                               & " SUM(IF(MONTH(b.emision) = 4, a.cantidad, 0.00 )) MAY, " _
                               & " SUM(IF(MONTH(b.emision) = 5, a.cantidad, 0.00 )) JUN, " _
                               & " SUM(IF(MONTH(b.emision) = 6, a.cantidad, 0.00 )) JUL, " _
                               & " SUM(IF(MONTH(b.emision) = 7, a.cantidad, 0.00 )) AGO, " _
                               & " SUM(IF(MONTH(b.emision) = 8, a.cantidad, 0.00 )) SEP, " _
                               & " SUM(IF(MONTH(b.emision) = 9, a.cantidad, 0.00 )) OCT, " _
                               & " SUM(IF(MONTH(b.emision) = 10, a.cantidad, 0.00 )) NOV, " _
                               & " SUM(IF(MONTH(b.emision) = 11, a.cantidad, 0.00 )) DIC " _
                               & " FROM jsvenrenfac a " _
                               & " LEFT JOIN jsvenencfac b ON (a.numfac = b.numfac AND a.id_emp = b.id_emp) " _
                               & " LEFT JOIN jsvencatcli c ON (b.codcli = c.codcli AND b.id_emp = c.id_emp) " _
                               & " WHERE " _
                               & " a.item = '$" & nArticulo & "' AND " _
                               & " YEAR(b.emision) = " & Year(jytsistema.sFechadeTrabajo) - AñosAtras & " AND " _
                               & " a.id_emp = '" & jytsistema.WorkID & "'  ")


                strSQLVentas = " select " & nArticulo & " CODART, " _
                & " IF(SUM(ENE) IS NULL, 0.00, SUM(ENE)) ENE, " _
                & " IF(SUM(FEB) IS NULL, 0.00, SUM(FEB)) FEB, " _
                & " IF(SUM(MAR) IS NULL, 0.00, SUM(MAR)) MAR, " _
                & " IF(SUM(ABR) IS NULL, 0.00, SUM(ABR)) ABR, " _
                & " IF(SUM(MAY) IS NULL, 0.00, SUM(MAY)) MAY, " _
                & " IF(SUM(JUN) IS NULL, 0.00, SUM(JUN)) JUN, " _
                & " IF(SUM(JUL) IS NULL, 0.00, SUM(JUL)) JUL, " _
                & " IF(SUM(AGO) IS NULL, 0.00, SUM(AGO)) AGO, " _
                & " IF(SUM(SEP) IS NULL, 0.00, SUM(SEP)) SEP, " _
                & " IF(SUM(OCT) IS NULL, 0.00, SUM(OCT)) OCT, " _
                & " IF(SUM(NOV) IS NULL, 0.00, SUM(NOV)) NOV, " _
                & " IF(SUM(DIC) IS NULL, 0.00, SUM(DIC)) DIC " _
                & " from " _
                & " " & tblTemp & " " _
                & " group by 1 "

            Case 2 ' 
                ft.Ejecutar_strSQL(myConn, " DROP TABLE IF EXISTS " & tblTemp & " ")
                ft.Ejecutar_strSQL(myConn, " CREATE TABLE " & tblTemp & " " _
                               & " SELECT " & nArticulo & " codart, SUM(IF(MONTH(b.emision) = 1, a.totrendes, 0.00 )) ENE, " _
                               & " SUM(IF(MONTH(b.emision) = 1, a.totrendes, 0.00 )) FEB, " _
                               & " SUM(IF(MONTH(b.emision) = 2, a.totrendes, 0.00 )) MAR, " _
                               & " SUM(IF(MONTH(b.emision) = 3, a.totrendes, 0.00 )) ABR, " _
                               & " SUM(IF(MONTH(b.emision) = 4, a.totrendes, 0.00 )) MAY, " _
                               & " SUM(IF(MONTH(b.emision) = 5, a.totrendes, 0.00 )) JUN, " _
                               & " SUM(IF(MONTH(b.emision) = 6, a.totrendes, 0.00 )) JUL, " _
                               & " SUM(IF(MONTH(b.emision) = 7, a.totrendes, 0.00 )) AGO, " _
                               & " SUM(IF(MONTH(b.emision) = 8, a.totrendes, 0.00 )) SEP, " _
                               & " SUM(IF(MONTH(b.emision) = 9, a.totrendes, 0.00 )) OCT, " _
                               & " SUM(IF(MONTH(b.emision) = 10, a.totrendes, 0.00 )) NOV, " _
                               & " SUM(IF(MONTH(b.emision) = 11, a.totrendes, 0.00 )) DIC " _
                               & " FROM jsvenrenfac a " _
                               & " LEFT JOIN jsvenencfac b ON (a.numfac = b.numfac AND a.id_emp = b.id_emp) " _
                               & " LEFT JOIN jsvencatcli c ON (b.codcli = c.codcli AND b.id_emp = c.id_emp) " _
                               & " WHERE " _
                               & " a.item = '$" & nArticulo & "' AND " _
                               & " YEAR(b.emision) = " & Year(jytsistema.sFechadeTrabajo) - AñosAtras & " AND " _
                               & " a.id_emp = '" & jytsistema.WorkID & "'  ")


                strSQLVentas = " select " & nArticulo & " CODART, " _
                & " IF(SUM(ENE) IS NULL, 0.00, SUM(ENE)) ENE, " _
                & " IF(SUM(FEB) IS NULL, 0.00, SUM(FEB)) FEB, " _
                & " IF(SUM(MAR) IS NULL, 0.00, SUM(MAR)) MAR, " _
                & " IF(SUM(ABR) IS NULL, 0.00, SUM(ABR)) ABR, " _
                & " IF(SUM(MAY) IS NULL, 0.00, SUM(MAY)) MAY, " _
                & " IF(SUM(JUN) IS NULL, 0.00, SUM(JUN)) JUN, " _
                & " IF(SUM(JUL) IS NULL, 0.00, SUM(JUL)) JUL, " _
                & " IF(SUM(AGO) IS NULL, 0.00, SUM(AGO)) AGO, " _
                & " IF(SUM(SEP) IS NULL, 0.00, SUM(SEP)) SEP, " _
                & " IF(SUM(OCT) IS NULL, 0.00, SUM(OCT)) OCT, " _
                & " IF(SUM(NOV) IS NULL, 0.00, SUM(NOV)) NOV, " _
                & " IF(SUM(DIC) IS NULL, 0.00, SUM(DIC)) DIC " _
                & " from " _
                & " " & tblTemp & " " _
                & " group by 1 "

        End Select

        ds = DataSetRequery(ds, strSQLVentas, myConn, nTablaVentas, lblInfo)
        TablaVentas = ds.Tables(nTablaVentas)
        ft.Ejecutar_strSQL(myConn, " DROP TABLE IF EXISTS " & tblTemp & " ")

    End Function


    Private Sub AsignarVentas(ByVal nArticulo As String)

        ft.habilitarObjetos(False, True, txtCodigoVentas, txtNombreVentas, txtCotizacion, txtPedidos, txtEntregas, _
            txtFechaUltimaVenta, txtUltimoCliente, txtSalidas, txtVentasAcum, txtVentasAcumDesc, txtPrecioUltimo, _
            txtDevolucionesVentas, txtVentasAcumDev, txtVentasAcumDevDesc, txtPrecioPromedio, txtNombreUltimoCliente)

        ''With dt.Rows(nPosicionCat)
        '' txtCotizacion.Text = ft.FormatoCantidad(.Item("cotizados"))
        '' txtPedidos.Text = ft.FormatoCantidad(.Item("pedidos"))
        ''txtEntregas.Text = ft.FormatoCantidad(.Item("entregas"))
        ''txtFechaUltimaVenta.Text = ft.FormatoFecha(CDate(.Item("FECULTventa").ToString))
        ''txtUltimoCliente.Text = IIf(IsDBNull(.Item("ultimocliente")), "", .Item("ultimocliente"))
        ''txtSalidas.Text = ft.FormatoCantidad(.Item("salidas"))
        ''txtVentasAcum.Text = ft.FormatoNumero(.Item("ACU_PRE"))
        ''txtVentasAcumDesc.Text = ft.FormatoNumero(.Item("ACU_PRE_DES"))
        ''txtPrecioUltimo.Text = ft.FormatoNumero(.Item("montoultimaventa"))
        ''txtDevolucionesVentas.Text = ft.FormatoCantidad(.Item("CREDITOSventas"))
        ''txtVentasAcumDev.Text = ft.FormatoNumero(.Item("ACU_PRD"))
        ''txtVentasAcumDevDesc.Text = ft.FormatoNumero(.Item("ACU_prd_DES"))
        ''txtPrecioPromedio.Text = ft.FormatoNumero(.Item("Venta_PROM_DES"))
        ''End With

        rBtnV1.Checked = True
        VerHistograma(C1Chart2, 1, txtCodigoVentas.Text, 0)

        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub txtUltimoProveedor_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUltimoProveedor.TextChanged
        Dim afld() As String = {"codpro", "id_emp"}
        Dim aCam() As String = {txtUltimoProveedor.Text, jytsistema.WorkID}
        txtNombreUltimoProveedor.Text = qFoundAndSign(myConn, lblInfo, "jsprocatpro", afld, aCam, "nombre")
    End Sub

    Private Sub rBtn1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rBtn1.CheckedChanged, _
         rBtn3.CheckedChanged

        If rBtn1.Checked Then VerHistograma(c1Chart1, 0, txtCodigoCompras.Text, 0)
        If rBtn3.Checked Then VerHistograma(c1Chart1, 0, txtCodigoCompras.Text, 2)

    End Sub
    Private Sub rBtnV1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rBtnV1.CheckedChanged, _
         rBtnV3.CheckedChanged

        If rBtnV1.Checked Then VerHistograma(C1Chart2, 1, txtCodigoVentas.Text, 0)
        If rBtnV3.Checked Then VerHistograma(C1Chart2, 1, txtCodigoVentas.Text, 2)

    End Sub
    Private Sub txtUltimoCliente_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUltimoCliente.TextChanged
        Dim afld() As String = {"codcli", "id_emp"}
        Dim aCam() As String = {txtUltimoCliente.Text, jytsistema.WorkID}
        txtNombreUltimoCliente.Text = qFoundAndSign(myConn, lblInfo, "jsvencatcli", afld, aCam, "nombre")
    End Sub

    Private Sub cmbTipoMovimiento_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipoMovimiento.SelectedIndexChanged
        Dim bs As New BindingSource
        Dim aTTipo() As String = {"", "EN", "SA", "AE", "AS", "AC"}
        bs.DataSource = dtMovimientos
        If dtMovimientos.Columns("tipomov").DataType Is GetType(String) Then _
            bs.Filter = " tipomov like '%" & aTTipo(cmbTipoMovimiento.SelectedIndex) & "%'"
        dg.DataSource = bs
    End Sub

    Private Function ColocaJerarquiaNivel(ByVal MyConn As MySqlConnection, ByVal TipoJerarquia As String, ByVal CodigoJerarquia As String, ByVal Nivel As Integer) As String
        Dim aCam() As String = {"tipjer", "codjer", "nivel", "id_emp"}
        Dim aStr() As String = {TipoJerarquia, CodigoJerarquia, Nivel, jytsistema.WorkID}
        ColocaJerarquiaNivel = qFoundAndSign(MyConn, lblInfo, "jsmerrenjer", aCam, aStr, "desjer")
    End Function

    Private Sub dgServicios_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgServicios.CellContentClick

    End Sub
End Class