Imports MySql.Data.MySqlClient
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

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long
    Private aEstatus() As String = {"Transito", "Procesado"}
    Private CodigoProveedorAnterior As String = ""
    Private NumeroDocumentoAnterior As String = ""

    Private Impresa As Integer
    Private Sub jsComArcOrdenesDeCompra_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
            MensajeCritico(lblInfo, "Error en conexi�n de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva Orden de Compra")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> Orden de Compra")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> Orden de Compra")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> Orden de Compra")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primer</B> Orden de Compra")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a Orden de Compra <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a Orden de Compra <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>�ltima Orden de Compra</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> Orden de Compra")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnDuplicar, "<B>Duplicar</B> esta Orden de Compra")

        'Menu barra rengl�n
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> rengl�n en Orden de Compra")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> rengl�n en Orden de Compra")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> rengl�n en Orden de Compra")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un rengl�n en Orden de Compra")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> rengl�n en Orden de Compra")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> rengl�n en Orden de Compra")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al rengl�n <B>siguiente </B> en Orden de Compra")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>�ltimo</B> rengl�n de la Orden de Compra")

    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)

        If c >= 0 AndAlso dtRenglones.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaRenglones).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", c, dtRenglones.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        'ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtRenglones, MenuBarraRenglon)
        HabilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtProveedor, btnProveedor, txtCodigo, txtNumeroSerie)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtCodigo.Text = .Item("numord")
                txtNumeroSerie.Text = MuestraCampoTexto(.Item("serie_numord"))
                txtEmision.Text = FormatoFecha(CDate(.Item("emision").ToString))
                txtVence.Text = FormatoFecha(CDate(.Item("entrega").ToString))
                txtEstatus.Text = aEstatus(.Item("estatus"))
                txtProveedor.Text = .Item("codpro")
                CodigoProveedorAnterior = .Item("codpro")
                NumeroDocumentoAnterior = .Item("numord")

                txtComentario.Text = MuestraCampoTexto(.Item("comen"))

                tslblPesoT.Text = FormatoCantidad(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, "select if( sum(peso) is null, 0.000, sum(peso)) from jsprorenord where numord = '" & .Item("numord") & "' and id_emp = '" & jytsistema.WorkID & "' ")))

                txtSubTotal.Text = FormatoNumero(.Item("tot_net"))
                txtTotalIVA.Text = FormatoNumero(.Item("imp_iva"))
                txtTotal.Text = FormatoNumero(.Item("tot_ord"))


                Impresa = .Item("impresa")

                'Renglones
                AsignarMovimientos(.Item("numord"), CodigoProveedorAnterior)

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
        Dim aNombres() As String = {"Item", "Descripci�n", "IVA", "Cant.", "UND", "Costo Unitario", "Desc. Art.", "Desc. Pro.", "Costo Total", "Tipo Renglon", ""}
        Dim aAnchos() As Long = {90, 300, 30, 90, 45, 100, 50, 50, 120, 60, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", sFormatoCantidad, "", sFormatoNumero, sFormatoNumero, _
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
        Dim aAnchos() As Long = {15, 45, 90, 90}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {"", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero}
        IniciarTabla(dgIVA, dtIVA, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False, , 8, False)

        txtTotalIVA.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(impiva) from jsproivaord where numord = '" & txtCodigo.Text & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' group by numord ")))


    End Sub


    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "OCTemp" & NumeroAleatorio(100000)
        Else
            txtCodigo.Text = ""
        End If

        txtNumeroSerie.Text = ""
        txtProveedor.Text = ""

        CodigoProveedorAnterior = ""
        NumeroDocumentoAnterior = txtCodigo.Text
        txtComentario.Text = ""
        txtEmision.Text = FormatoFecha(sFechadeTrabajo)
        txtVence.Text = FormatoFecha(sFechadeTrabajo)
        txtEstatus.Text = aEstatus(0)
        tslblPesoT.Text = FormatoCantidad(0)

        txtSubTotal.Text = FormatoNumero(0.0)
        txtTotalIVA.Text = FormatoNumero(0.0)
        txtTotal.Text = FormatoNumero(0.0)

        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", 0, 0)
        AsignarMovimientos(txtCodigo.Text, CodigoProveedorAnterior)
        AbrirIVA(txtCodigo.Text, CodigoProveedorAnterior)

    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        HabilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon)
        HabilitarObjetos(True, True, txtNumeroSerie, txtComentario, btnEmision, btnVence, btnProveedor, txtProveedor)

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier bot�n de la barra menu...", TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        HabilitarObjetos(False, True, txtCodigo, txtNumeroSerie, txtEmision, btnEmision, txtVence, btnVence, txtEstatus, _
                txtProveedor, txtNombreProveedor, btnProveedor, txtComentario)

        HabilitarObjetos(False, True, txtSubTotal, txtTotalIVA, txtTotal)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier bot�n de la barra menu...", TipoMensaje.iAyuda)
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

        EjecutarSTRSQL(myConn, lblInfo, " delete from jsprorenord where numord = '" & txtCodigo.Text & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsproivaord where numord = '" & txtCodigo.Text & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'ORD' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado() As Boolean


        If txtNombreProveedor.Text.Trim() = "" OrElse txtProveedor.Text.Trim() = "" Then
            MensajeCritico(lblInfo, "Debe indicar un proveedor v�lido...")
            Return False
        End If

        If CDate(txtVence.Text) < CDate(txtEmision.Text) Then
            MensajeCritico(lblInfo, "Fecha de emision es mayor a fecha de entrega. Verifique ...")
            Return False
        End If

        If dtRenglones.Rows.Count = 0 Then
            MensajeCritico(lblInfo, "Debe incluir al menos un �tem...")
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

        EjecutarSTRSQL(myConn, lblInfo, " update jsprorenord set numord = '" & Codigo & "', codpro = '" & txtProveedor.Text & "' where codpro = '" & CodigoProveedorAnterior & "' and numord = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " update jsproivaord set numord = '" & Codigo & "', codpro = '" & txtProveedor.Text & "' where codpro = '" & CodigoProveedorAnterior & "' and numord = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & NumeroDocumentoAnterior & "' and origen = 'ORD' and id_emp = '" & jytsistema.WorkID & "' ")

        InsertEditCOMPRASEncabezadoOrdenes(myConn, lblInfo, Inserta, Codigo, txtNumeroSerie.Text, CDate(txtEmision.Text), CDate(txtVence.Text), _
                                               txtProveedor.Text, txtComentario.Text, _
                                               ValorNumero(txtSubTotal.Text), _
                                               ValorNumero(txtTotalIVA.Text), ValorNumero(txtTotal.Text), _
                                               InArray(aEstatus, txtEstatus.Text) - 1, dtRenglones.Rows.Count, _
                                               Impresa, CodigoProveedorAnterior)


        ActualizarMovimientos(Codigo)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub ActualizarMovimientos(ByVal NumeroPresupuesto As String)

        '1.- Actualiza cantidad de Ordenes de Compra en cat�logo de mercanc�as
        EjecutarSTRSQL_Scalar(myConn, lblInfo, _
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
        MensajeEtiqueta(lblInfo, " Indique comentario ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
        HabilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtProveedor, btnProveedor, txtCodigo, txtNumeroSerie)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        If dt.Rows(nPosicionEncab).Item("estatus") = "0" Then
            i_modo = movimiento.iEditar
            ActivarMarco0()
            HabilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtProveedor, btnProveedor, txtCodigo, txtNumeroSerie)
        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position

        If dt.Rows(nPosicionEncab).Item("estatus") = "0" Then
            sRespuesta = MsgBox(" � Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")

            If sRespuesta = MsgBoxResult.Yes Then

                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numord"))
                EjecutarSTRSQL(myConn, lblInfo, " delete from jsproencord where " _
                    & " numord = '" & txtCodigo.Text & "' AND " _
                    & " codpro = '" & CodigoProveedorAnterior & "' and " _
                    & " ID_EMP = '" & jytsistema.WorkID & "'")

                EjecutarSTRSQL(myConn, lblInfo, " delete from jsprorenord where numord = '" & txtCodigo.Text & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
                EjecutarSTRSQL(myConn, lblInfo, " delete from jsproivaord where numord = '" & txtCodigo.Text & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")

                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                dt = ds.Tables(nTabla)
                If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                AsignaTXT(nPosicionEncab)

            End If
        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numord", "codpro", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"N�mero ", "Proveedor", "nombre", "Emisi�n", "Comentario"}
        Dim Anchos() As Long = {150, 400, 100, 150, 250}
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
        Dim aTipoRenglon() As String = {"Normal", "Sin Desc.", "Bonificaci�n"}
        Select Case e.ColumnIndex
            Case 9
                e.Value = aTipoRenglon(IIf(e.Value = "", "0", e.Value))
        End Select
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "ItemsRenglon", "lblItemsRenglon", e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub CalculaTotales()

        txtSubTotal.Text = FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsprorenord", "numord", "costotot", txtCodigo.Text, 0, CodigoProveedorAnterior))

        CalculaTotalIVACompras(myConn, lblInfo, CodigoProveedorAnterior, "", "jsproivaord", "jsprorenord", "numord", txtCodigo.Text, "impiva", "costototdes", CDate(txtEmision.Text), "costotot")

        AbrirIVA(txtCodigo.Text, CodigoProveedorAnterior)

        txtTotal.Text = FormatoNumero(ValorNumero(txtSubTotal.Text) + ValorNumero(txtTotalIVA.Text))

        tslblPesoT.Text = FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsprorenord", "peso", "numord", txtCodigo.Text))

    End Sub

    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        If txtCodigo.Text.Trim() = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar un n�mero de Orden de Compra v�lido...")
        Else
            If txtNombreProveedor.Text.Trim() = "" Then
                MensajeAdvertencia(lblInfo, "Debe indicar un C�digo de proveedor v�lido...")
            Else
                Dim f As New jsGenRenglonesMovimientos
                f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
                f.Agregar(myConn, ds, dtRenglones, "ORD", txtCodigo.Text, CDate(txtEmision.Text), , , , _
                          , , , , , , , , txtProveedor.Text)
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
                MensajeAdvertencia(lblInfo, "RENGLON YA PROCESADO")
            Else
                If CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select estatus from jsmerctainv where codart = '" & dtRenglones.Rows(nPosicionRenglon).Item("ITEM") & "' and id_emp = '" & jytsistema.WorkID & "' ")) = "1" Then
                    MensajeAdvertencia(lblInfo, "MERCANCIA INACTIVA")
                Else
                    Dim f As New jsGenRenglonesMovimientos
                    f.Apuntador = nPosicionRenglon
                    f.Editar(myConn, ds, dtRenglones, "ORD", txtCodigo.Text, CDate(txtEmision.Text), , , , _
                             IIf(dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position).Item("item").ToString.Substring(1, 1) = "$", True, False), _
                             , , , , , , , , , txtProveedor.Text)
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

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then
            If dtRenglones.Rows(nPosicionRenglon).Item("CANTRAN") <= 0 Then
                MensajeAdvertencia(lblInfo, "RENGLON YA PROCESADO")
            Else
                If CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select estatus from jsmerctainv where codart = '" & dtRenglones.Rows(nPosicionRenglon).Item("ITEM") & "' and id_emp = '" & jytsistema.WorkID & "' ")) = "1" Then
                    MensajeAdvertencia(lblInfo, "MERCANCIA INACTIVA")
                Else
                    sRespuesta = MsgBox(" � Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                    If sRespuesta = MsgBoxResult.Yes Then
                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dtRenglones.Rows(nPosicionRenglon).Item("item"))
                        Dim aCamposDel() As String = {"numord", "codpro", "item", "renglon", "id_emp"}
                        Dim aStringsDel() As String = {txtCodigo.Text, CodigoProveedorAnterior, dtRenglones.Rows(nPosicionRenglon).Item("item"), dtRenglones.Rows(nPosicionRenglon).Item("renglon"), jytsistema.WorkID}

                        nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsprorenord", strSQLMov, aCamposDel, aStringsDel, _
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
        Dim Nombres() As String = {"Item", "Descripci�n"}
        Dim Anchos() As Long = {140, 350}
        f.Buscar(dtRenglones, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaRenglones).Position, "Renglones de �rdenes de compra")
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
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cOrdenDeCompra, "Orden de Compra", txtProveedor.Text, txtCodigo.Text, CDate(txtEmision.Text))
        f.Dispose()
        f = Nothing

        'EjecutarSTRSQL(myConn, lblInfo, "update jsproencord set impresa = 1 where numord = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub

    Private Sub btnEmision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmision.Click
        txtEmision.Text = SeleccionaFecha(CDate(txtEmision.Text), Me, sender)
    End Sub

    Private Sub btnVence_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVence.Click
        txtVence.Text = SeleccionaFecha(CDate(txtVence.Text), Me, sender)
    End Sub


    Private Sub txtCliente_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProveedor.TextChanged
        Dim aFld() As String = {"codpro", "id_emp"}
        Dim aStr() As String = {txtProveedor.Text, jytsistema.WorkID}
        txtNombreProveedor.Text = qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "nombre")

    End Sub

    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dgIVA.CellFormatting
        If e.ColumnIndex = 1 Then e.Value = FormatoNumero(e.Value) & "%"
    End Sub

    Private Sub btnCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProveedor.Click
        txtProveedor.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codpro codigo, nombre descripcion, disponible, elt(estatus+1, 'Activo', 'Inactivo') estatus from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Proveedores de Compras/Gastos", _
                                            txtProveedor.Text)
    End Sub

    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If txtNombreProveedor.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "ORD", txtCodigo.Text, CDate(txtEmision.Text), , , , True, , , , , , , , , , CodigoProveedorAnterior)
            nPosicionRenglon = f.Apuntador
            AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
            CalculaTotales()
            f = Nothing
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

End Class