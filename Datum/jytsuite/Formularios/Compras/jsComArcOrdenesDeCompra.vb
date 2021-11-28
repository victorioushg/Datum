Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsComArcOrdenesDeCompra
    Private Const sModulo As String = "Ordenes de Compra"
    Private Const lRegion As String = "RibbonButton59"
    Private Const nTabla As String = "tblEncab"
    Private Const nTablaRenglones As String = "tblRenglones"
    Private Const nTablaIVA As String = "tblIVA"

    Private strSQL As String = " select a.*, b.nombre from jsproencord a " _
            & " left join jsprocatpro b on (a.codpro = b.codpro and a.id_emp = b.id_emp) " _
            & " where a.id_emp = '" & jytsistema.WorkID & "' order by a.numord "

    Private strSQLMov As String = ""
    Private strSQLIVA As String = ""
    Private strSQLDescuentos As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtRenglones As New DataTable
    Private dtIVA As New DataTable
    Private dtDescuentos As New DataTable
    Private ft As New Transportables

    Private proveedor As New Vendor()
    Private interchangeList As New List(Of CambioMonedaPlus)


    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long
    Private aEstatus() As String = {"Tránsito", "Procesado"}
    Private CodigoProveedorAnterior As String = ""
    Private NumeroDocumentoAnterior As String = ""

    Private Impresa As Integer

    Private Sub jsComArcOrdenesDeCompra_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsComArcOrdenesDeCompra_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)
            IniciarControles()
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
    Private Sub IniciarControles()
        Dim dates As SfDateTimeEdit() = {txtEmision, txtVence}
        SetSizeDateObjects(dates)
        '' Proveedores
        InitiateDropDown(Of Vendor)(myConn, cmbProveedor)
        '' Monedas
        interchangeList = GetListaDeMonedasyCambios(myConn, jytsistema.sFechadeTrabajo)
        InitiateDropDownInterchangeCurrency(myConn, cmbMonedas, jytsistema.sFechadeTrabajo)
    End Sub

    Private Sub AsignarTooltips()
        Dim menus As New List(Of ToolStrip) From {MenuBarra, MenuBarraRenglon}
        AsignarToolTipsMenuBarraToolStrip(menus, "Orden de Compra")
    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        If c >= 0 AndAlso dtRenglones.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaRenglones).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, c, dtRenglones.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, cmbProveedor, txtCodigo, txtNumeroSerie)
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt
            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow
            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)
            With .Rows(nRow)
                'Encabezado 
                txtCodigo.Text = .Item("numord")
                txtNumeroSerie.Text = ft.muestraCampoTexto(.Item("serie_numord"))
                txtEmision.Value = ft.FormatoFecha(CDate(.Item("emision").ToString))
                txtVence.Value = ft.FormatoFecha(CDate(.Item("entrega").ToString))
                txtEstatus.Text = aEstatus(.Item("estatus"))
                cmbProveedor.SelectedValue = .Item("codpro")
                cmbMonedas.SelectedValue = .Item("Currency")
                CodigoProveedorAnterior = .Item("codpro")
                NumeroDocumentoAnterior = .Item("numord")
                txtComentario.Text = ft.muestraCampoTexto(.Item("comen"))
                tslblPesoT.Text = ft.FormatoCantidad(ft.DevuelveScalarDoble(myConn, "select SUM(PESO) from jsprorenord where numord = '" & .Item("numord") & "' and id_emp = '" & jytsistema.WorkID & "' "))
                txtSubTotal.Text = ft.FormatoNumero(.Item("tot_net"))
                txtTotalIVA.Text = ft.FormatoNumero(.Item("imp_iva"))
                txtTotal.Text = ft.FormatoNumero(.Item("tot_ord"))
                Impresa = .Item("impresa")
                'Renglones
                AsignarMovimientos(.Item("numord"), IIf(CodigoProveedorAnterior = "", proveedor.Codigo, CodigoProveedorAnterior))
                'Totales
                CalculaTotales()
            End With
        End With
    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroDocumento As String, ByVal CodigoProveedor As String)
        strSQLMov = "select * from jsprorenord " _
                            & " where " _
                            & " codpro = '" & CodigoProveedor & "' and " _
                            & " numord  = '" & NumeroDocumento & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"item", "descrip", "iva", "cantidad", "unidad", "costou", "des_art", "des_pro", "costotot", "estatus", ""}
        Dim aNombres() As String = {"Item", "Descripción", "IVA", "Cant.", "UND", "Costo Unitario", "Desc. Art.", "Desc. Pro.", "Costo Total", "Tipo Renglon", ""}
        Dim aAnchos() As Integer = {90, 300, 30, 90, 45, 100, 50, 50, 120, 60, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda,
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", sFormatoCantidad, "", sFormatoNumero, sFormatoNumero,
                                     sFormatoNumero, sFormatoNumero, "", ""}
        IniciarTabla(dg, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtRenglones.Rows.Count > 0 Then nPosicionRenglon = 0
        AsignaMov(nPosicionRenglon, True)

    End Sub
    Private Sub AbrirIVA(ByVal NumeroDocumento As String, ByVal CodigoProveedor As String)

        strSQLIVA = "select * from jsproivaord " _
                            & " where " _
                            & " numord  = '" & NumeroDocumento & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by tipoiva "

        ds = DataSetRequery(ds, strSQLIVA, myConn, nTablaIVA, lblInfo)
        dtIVA = ds.Tables(nTablaIVA)

        Dim aCampos() As String = {"tipoiva", "poriva", "baseiva", "impiva"}
        Dim aNombres() As String = {"", "", "", ""}
        Dim aAnchos() As Integer = {15, 45, 90, 90}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {"", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero}
        IniciarTabla(dgIVA, dtIVA, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False, , 8, False)

        txtTotalIVA.Text = ft.FormatoNumero(ft.DevuelveScalarDoble(myConn, " select SUM(IMPIVA) from jsproivaord where numord = '" & txtCodigo.Text & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' group by numord "))


    End Sub


    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        txtNumeroSerie.Text = ""
        CodigoProveedorAnterior = ""

        txtComentario.Text = ""
        txtEmision.Value = jytsistema.sFechadeTrabajo
        txtVence.Value = jytsistema.sFechadeTrabajo
        txtEstatus.Text = aEstatus(0)
        tslblPesoT.Text = ft.FormatoCantidad(0)
        cmbProveedor.SelectedIndex = -1
        cmbMonedas.SelectedIndex = 0

        txtSubTotal.Text = ft.FormatoNumero(0.0)
        txtTotalIVA.Text = ft.FormatoNumero(0.0)
        txtTotal.Text = ft.FormatoNumero(0.0)

        Impresa = 0

        If Inicio Then
            txtCodigo.Text = "OCTemp" & ft.NumeroAleatorio(100000)
            NumeroDocumentoAnterior = txtCodigo.Text
            'Movimientos
            MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
            AsignarMovimientos(txtCodigo.Text, CodigoProveedorAnterior)
            AbrirIVA(txtCodigo.Text, CodigoProveedorAnterior)

        Else
            DesactivarMarco0()
            txtCodigo.Text = ""
        End If

    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon)
        ft.habilitarObjetos(True, True, txtNumeroSerie, txtComentario, txtEmision, txtVence, cmbProveedor, cmbMonedas)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtNumeroSerie, txtEmision, txtVence, txtEstatus, cmbProveedor, cmbMonedas, txtComentario)

        ft.habilitarObjetos(False, True, txtSubTotal, txtTotalIVA, txtTotal)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then
            If ft.DevuelveScalarEntero(myConn, " select count(*) from jsproencord " _
                                      & " where " _
                                      & " numord = '" & txtCodigo.Text & "' and " _
                                      & " codpro = '" & proveedor.Codigo & "' and " _
                                      & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                AgregaYCancela()
            End If
        Else
            If dtRenglones.Rows.Count = 0 Then
                ft.mensajeCritico("DEBE INCLUIR AL MENOS UN ITEM...")
                Return
            End If
        End If

        If dt.Rows.Count = 0 Then
            IniciarDocumento(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            AsignaTXT(nPosicionEncab)
        End If

        DesactivarMarco0()
    End Sub
    Private Sub AgregaYCancela()

        ft.Ejecutar_strSQL(myConn, " delete from jsprorenord where numord = '" & txtCodigo.Text & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsproivaord where numord = '" & txtCodigo.Text & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'ORD' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado() As Boolean

        If FechaUltimoBloqueo(myConn, "jsproencord") >= txtEmision.Value Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If cmbProveedor.SelectedValue = "" Then
            ft.mensajeCritico("Debe indicar un proveedor válido...")
            Return False
        End If

        If txtVence.Value < txtEmision.Value Then
            ft.mensajeCritico("Fecha de emision es mayor a fecha de entrega. Verifique ...")
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
            Codigo = Contador(myConn, lblInfo, Gestion.iCompras, "comnumord", "05")
        End If

        ft.Ejecutar_strSQL(myConn, " update jsprorenord set numord = '" & Codigo & "', codpro = '" & proveedor.Codigo & "' where codpro = '" & CodigoProveedorAnterior & "' and numord = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " update jsproivaord set numord = '" & Codigo & "', codpro = '" & proveedor.Codigo & "' where codpro = '" & CodigoProveedorAnterior & "' and numord = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & NumeroDocumentoAnterior & "' and origen = 'ORD' and id_emp = '" & jytsistema.WorkID & "' ")

        InsertEditCOMPRASEncabezadoOrdenes(myConn, lblInfo, Inserta, Codigo, txtNumeroSerie.Text, txtEmision.Value, txtVence.Value,
                                               proveedor.Codigo, txtComentario.Text,
                                               ValorNumero(txtSubTotal.Text),
                                               ValorNumero(txtTotalIVA.Text), ValorNumero(txtTotal.Text),
                                               ft.InArray(aEstatus, txtEstatus.Text), dtRenglones.Rows.Count,
                                               Impresa, CodigoProveedorAnterior, cmbMonedas.SelectedValue, jytsistema.sFechadeTrabajo)


        ActualizarMovimientos(Codigo)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub ActualizarMovimientos(ByVal NumeroPresupuesto As String)

        '1.- Actualiza cantidad de Ordenes de Compra en catálogo de mercancías
        ft.Ejecutar_strSQL(myConn,
                               " UPDATE jsmerctainv a, (SELECT a.item, SUM(IF( ISNULL(b.UVALENCIA), ABS(a.CANTIDAD), ABS(a.CANTIDAD/b.EQUIVALE))) cantidad, a.id_emp FROM " _
                                & "                          jsprorenord a LEFT JOIN jsmerequmer b " _
                                & "                          ON (a.ITEM = b.CODART AND a.UNIDAD = b.UVALENCIA AND a.id_emp = b.id_emp) " _
                                & "                          WHERE " _
                                & "                          substring(a.item,1,1) <> '$' and " _
                                & "                          a.numord = '" & NumeroPresupuesto & "' and " _
                                & "                          a.estatus <> 2 AND " _
                                & "                          a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                                & "                          a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                & "                          GROUP BY a.ITEM) b " _
                                & " set a.ordenes = b.cantidad " _
                                & " WHERE " _
                                & " a.codart =  b.item AND " _
                                & " a.id_emp = b.id_emp and  " _
                                & " a.id_emp = '" & jytsistema.WorkID & "' ")

    End Sub

    Private Sub txtComentario_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique comentario ... ", Transportables.tipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, cmbProveedor, txtCodigo, txtNumeroSerie)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        With dt.Rows(nPosicionEncab)
            Dim aCamposAdicionales() As String = {"numord|'" & txtCodigo.Text & "'",
                                                  "codpro|'" & proveedor.Codigo & "'"}
            If DocumentoBloqueado(myConn, "jsproencord", aCamposAdicionales) Then
                ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
            Else
                If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPAORD01")) Then
                    nPosicionEncab = Me.BindingContext(ds, nTabla).Position
                    If dt.Rows(nPosicionEncab).Item("estatus") = "0" Then
                        i_modo = movimiento.iEditar
                        ActivarMarco0()
                        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, cmbProveedor, txtCodigo, txtNumeroSerie)
                    End If
                Else
                    ft.mensajeCritico(" Esta Oden de Compra NO puede ser modificada...")
                End If
            End If
        End With
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        With dt.Rows(nPosicionEncab)
            Dim aCamposAdicionales() As String = {"numord|'" & txtCodigo.Text & "'",
                                                  "codpro|'" & proveedor.Codigo & "'"}
            If DocumentoBloqueado(myConn, "jsproenccom", aCamposAdicionales) Then
                ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
            Else
                If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPAORD01")) Then
                    If dt.Rows(nPosicionEncab).Item("estatus") = "0" Then
                        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

                            InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numord"))
                            ft.Ejecutar_strSQL(myConn, " delete from jsproencord where " _
                                & " numord = '" & txtCodigo.Text & "' AND " _
                                & " codpro = '" & CodigoProveedorAnterior & "' and " _
                                & " ID_EMP = '" & jytsistema.WorkID & "'")

                            ft.Ejecutar_strSQL(myConn, " delete from jsprorenord where numord = '" & txtCodigo.Text & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
                            ft.Ejecutar_strSQL(myConn, " delete from jsproivaord where numord = '" & txtCodigo.Text & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")

                            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                            dt = ds.Tables(nTabla)
                            If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                            If dt.Rows.Count > 0 Then
                                AsignaTXT(nPosicionEncab)
                            Else
                                IniciarDocumento(False)

                            End If

                        End If
                    End If
                Else
                    ft.mensajeCritico("Esta orden de compra NO puede ser eliminada...")
                End If
            End If
        End With

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numord", "codpro", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Proveedor", "nombre", "Emisión", "Comentario"}
        Dim Anchos() As Integer = {150, 400, 100, 150, 250}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Ordenes de Compra...")
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
        Dim aTipoRenglon() As String = {"Normal", "Sin Desc.", "Bonificación"}
        Select Case e.ColumnIndex
            Case 9
                e.Value = aTipoRenglon(IIf(e.Value = "", "0", e.Value))
        End Select
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick,
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub CalculaTotales()

        txtSubTotal.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsprorenord", "numord", "costotot", txtCodigo.Text, 0, CodigoProveedorAnterior))

        CalculaTotalIVACompras(myConn, lblInfo, CodigoProveedorAnterior, "", "jsproivaord", "jsprorenord", "numord", txtCodigo.Text, "impiva", "costototdes", txtEmision.Value, "costotot")

        AbrirIVA(txtCodigo.Text, CodigoProveedorAnterior)

        txtTotal.Text = ft.FormatoNumero(ValorNumero(txtSubTotal.Text) + ValorNumero(txtTotalIVA.Text))

        tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsprorenord", "peso", "numord", txtCodigo.Text))

    End Sub

    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        If txtCodigo.Text.Trim() = "" Then
            ft.mensajeAdvertencia("Debe indicar un número de Orden de Compra válido...")
        Else
            If cmbProveedor.SelectedValue = "" Then
                ft.mensajeAdvertencia("Debe indicar un Código de proveedor válido...")
            Else
                Dim f As New jsGenRenglonesMovimientos
                f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
                f.Agregar(myConn, ds, dtRenglones, "ORD", txtCodigo.Text, txtEmision.Value, , , ,
                          , , , , , , , , proveedor.Codigo, , CodigoProveedorAnterior)
                nPosicionRenglon = f.Apuntador
                AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
                CalculaTotales()
                f = Nothing
            End If
        End If
    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
            If dtRenglones.Rows(nPosicionRenglon).Item("CANTIDAD") <= 0 Then
                ft.mensajeAdvertencia("RENGLON YA PROCESADO")
            Else
                If ft.DevuelveScalarCadena(myConn, " select estatus from jsmerctainv where codart = '" & dtRenglones.Rows(nPosicionRenglon).Item("ITEM") & "' and id_emp = '" & jytsistema.WorkID & "' ") = "1" Then
                    ft.mensajeAdvertencia("MERCANCIA INACTIVA")
                Else
                    Dim f As New jsGenRenglonesMovimientos
                    f.Apuntador = nPosicionRenglon
                    f.Editar(myConn, ds, dtRenglones, "ORD", txtCodigo.Text, txtEmision.Value, , , ,
                             IIf(dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position).Item("item").ToString.Substring(0, 1) = "$", True, False),
                             , , , , , , , , , proveedor.Codigo)
                    ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
                    AsignaMov(f.Apuntador, True)
                    CalculaTotales()
                    f = Nothing
                End If
            End If
        End If

    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click

        EliminarMovimiento()

    End Sub
    Private Sub EliminarMovimiento()


        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then
            If dtRenglones.Rows(nPosicionRenglon).Item("CANTRAN") <= 0 Then
                ft.mensajeAdvertencia("RENGLON YA PROCESADO")
            Else
                If ft.DevuelveScalarCadena(myConn, " select estatus from jsmerctainv where codart = '" & dtRenglones.Rows(nPosicionRenglon).Item("ITEM") & "' and id_emp = '" & jytsistema.WorkID & "' ") = "1" Then
                    ft.mensajeAdvertencia("MERCANCIA INACTIVA")
                Else

                    If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dtRenglones.Rows(nPosicionRenglon).Item("item"))
                        Dim aCamposDel() As String = {"numord", "codpro", "item", "renglon", "id_emp"}
                        Dim aStringsDel() As String = {txtCodigo.Text, CodigoProveedorAnterior, dtRenglones.Rows(nPosicionRenglon).Item("item"), dtRenglones.Rows(nPosicionRenglon).Item("renglon"), jytsistema.WorkID}

                        nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsprorenord", strSQLMov, aCamposDel, aStringsDel,
                                                      Me.BindingContext(ds, nTablaRenglones).Position, True)

                        If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1
                        AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
                        CalculaTotales()
                    End If
                End If
            End If
        End If

    End Sub
    Private Sub btnBuscarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarMovimiento.Click

        Dim f As New frmBuscar
        Dim Campos() As String = {"item", "descripcion"}
        Dim Nombres() As String = {"Item", "Descripción"}
        Dim Anchos() As Integer = {140, 350}
        f.Buscar(dtRenglones, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaRenglones).Position, "Renglones de órdenes de compra")
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
        Imprimir()
    End Sub
    Private Sub Imprimir()
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cOrdenDeCompra, "Orden de Compra", proveedor.Codigo, txtCodigo.Text, txtEmision.Value)
        f.Dispose()
        f = Nothing
    End Sub
    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dgIVA.CellFormatting
        If e.ColumnIndex = 1 Then e.Value = ft.FormatoNumero(e.Value) & "%"
    End Sub
    Private Sub btnCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        proveedor.Codigo = CargarTablaSimple(myConn, lblInfo, ds, " select codpro codigo, nombre descripcion, disponible, elt(estatus+1, 'Activo', 'Inactivo') estatus from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Proveedores de Compras/Gastos",
                                            proveedor.Codigo)
    End Sub
    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If txtCodigo.Text.Trim() = "" Then
            ft.mensajeAdvertencia("Debe indicar un número de Orden de Compra válido...")
        Else
            If cmbProveedor.SelectedValue = "" Then
                ft.mensajeAdvertencia("Debe indicar un Código de proveedor válido...")
            Else
                Dim f As New jsMerArcListaCostosPreciosSelecionable
                f.Cargar(myConn, TipoListaPrecios.CostosSugeridoPlus, , , IIf(CodigoProveedorAnterior = "", proveedor.Codigo, CodigoProveedorAnterior))
                If Not f.Seleccionado Is Nothing Then
                    For Each nRow As Object In f.Seleccionado
                        If ft.DevuelveScalarEntero(myConn, " select count(*) from jsprorenord where numord = '" & txtCodigo.Text & "' and codpro = '" & CodigoProveedorAnterior & "' and item = '" + nRow(1).ToString + "' and id_emp = '" + jytsistema.WorkID + "' group by item ") = 0 Then

                            Dim peso As Double = ft.DevuelveScalarDoble(myConn, " select pesounidad from jsmerctainv where codart = '" + nRow(1).ToString + "' and id_emp = '" + jytsistema.WorkID + "' ")
                            Dim costo As Double = ft.DevuelveScalarDoble(myConn, " select montoultimacompra from jsmerctainv where codart = '" + nRow(1).ToString + "' and id_emp = '" + jytsistema.WorkID + "' ")

                            InsertEditCOMPRASRenglonORDENES(myConn, lblInfo, True, txtCodigo.Text,
                                 ft.autoCodigo(myConn, "RENGLON", "jsprorenord", "numord.id_emp", txtCodigo.Text + "." + jytsistema.WorkID, 5), IIf(CodigoProveedorAnterior = "", proveedor.Codigo, CodigoProveedorAnterior),
                                 nRow(1).ToString, nRow(2).ToString, nRow(4).ToString, 0.0, nRow(9).ToString,
                                 ValorCantidad(nRow(8).ToString), peso * ValorCantidad(nRow(8).ToString), ValorCantidad(nRow(8).ToString), 0,
                                 costo, 0, 0, costo * ValorCantidad(nRow(8).ToString), costo * ValorCantidad(nRow(8).ToString), "", "1")

                        End If
                    Next
                    AsignarMovimientos(txtCodigo.Text, IIf(CodigoProveedorAnterior = "", proveedor.Codigo, CodigoProveedorAnterior))
                    CalculaTotales()
                End If
                f.Dispose()
                f = Nothing
            End If
        End If
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Private Sub btnDuplicar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDuplicar.Click

    End Sub
    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(ds, nTablaRenglones).Position += 1
                nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
                AsignaMov(nPosicionRenglon, False)
            Case Keys.Up
                Me.BindingContext(ds, nTablaRenglones).Position -= 1
                nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
                AsignaMov(nPosicionRenglon, False)
        End Select
    End Sub
    Private Sub cmbProveedor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbProveedor.SelectedIndexChanged
        proveedor = cmbProveedor.SelectedItem
        CodigoProveedorAnterior = proveedor.Codigo
    End Sub
    Private Sub btnAgregarServico_Click(sender As System.Object, e As System.EventArgs) Handles btnAgregarServico.Click
        If cmbProveedor.SelectedValue <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "ORD", NumeroDocumentoAnterior, txtEmision.Value, , , , True, , , , , , , , , , CodigoProveedorAnterior)
            nPosicionRenglon = f.Apuntador
            AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
            CalculaTotales()
            f = Nothing
        Else
            ft.mensajeEtiqueta(lblInfo, "Debe indicar un proveedor válido para asi incluir ítems en la orden", Transportables.tipoMensaje.iAyuda)

        End If
    End Sub
End Class