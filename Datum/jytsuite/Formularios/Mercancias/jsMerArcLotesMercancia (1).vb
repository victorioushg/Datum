Imports MySql.Data.MySqlClient

Public Class jsMerArcLotesMercancia

    Private Const sModulo As String = "Lotes de mercancía"
    Private Const lRegion As String = ""
    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private nTabla As String = "tblLotesMercancia"
    Private strSQL As String


    Dim aCampos() As String = {"lote", "expiracion", "existencia", ""}
    Dim aNombres() As String = {"N° de Lote", "Fecha Expiración", "Existencia", ""}
    Dim aAnchos() As Integer = {150, 100, 100, 100}
    Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda}
    Dim aFormatos() As String = {"", sFormatoFecha, sFormatoCantidad, ""}

    Private n_Apuntador As Long
    Private n_Seleccion As String
    Private BindingSource1 As New BindingSource
    Private FindField As String

    Private CodigoMercancia As String
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Property Seleccion() As String
        Get
            Return n_Seleccion
        End Get
        Set(ByVal value As String)
            n_Seleccion = value
        End Set
    End Property
    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal iCodigoMercancia As String)
        Try
            strSQL = "select lote, expiracion, (entradas - salidas) existencia from jsmerlotmer where codart = '" & _
                iCodigoMercancia & "' and id_emp = '" & jytsistema.WorkID & "' order by expiracion desc "

            Me.Text = sModulo
            Me.Tag = sModulo
            CodigoMercancia = iCodigoMercancia
            myConn = MyCon

            dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)
            IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

            If dt.Rows.Count > 0 Then n_Apuntador = 0

            AsignaTXT(n_Apuntador, False)
            AsignarTooltips()
            Me.ShowDialog()

            ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)

        Catch ex As MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        Catch ex1 As Exception
            'ft.mensajeCritico("Error " & ex1.Message)
        End Try
    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra()
        ft.colocaToolTip(C1SuperTooltip1, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnSeleccionar, _
                          btnPrimero, btnSiguiente, btnAnterior, btnUltimo, btnImprimir, btnSalir)

        MenuBarra.ImageList = ImageList1

        btnAgregar.Image = ImageList1.Images(0)
        btnEditar.Image = ImageList1.Images(1)
        btnEliminar.Image = ImageList1.Images(2)
        btnBuscar.Image = ImageList1.Images(3)
        btnSeleccionar.Image = ImageList1.Images(4)
        btnPrimero.Image = ImageList1.Images(6)
        btnAnterior.Image = ImageList1.Images(7)
        btnSiguiente.Image = ImageList1.Images(8)
        btnUltimo.Image = ImageList1.Images(9)
        btnImprimir.Image = ImageList1.Images(10)
        btnSalir.Image = ImageList1.Images(11)


    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean)
        dt = ft.MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, _
                                nRow, Actualiza)
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        If dt.Rows.Count > 0 AndAlso dg.RowCount > 0 Then
            Apuntador = Me.BindingContext(ds, nTabla).Position
            Seleccion = dg.SelectedRows(0).Cells(0).Value.ToString
            Me.Close()
        End If
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        If dg.RowCount = dt.Rows.Count Then
            Me.BindingContext(ds, nTabla).Position = 0
            AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
        End If
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        If dg.RowCount = dt.Rows.Count Then
            Me.BindingContext(ds, nTabla).Position -= 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
        End If
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        If dg.RowCount = dt.Rows.Count Then
            Me.BindingContext(ds, nTabla).Position += 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
        End If
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        If dg.RowCount = dt.Rows.Count Then
            Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)

        End If
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        n_Apuntador = -1
        Seleccion = ""
        Me.Dispose()
    End Sub

    Private Sub jsConTablaSimple_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt.Dispose()
        dt = Nothing
        ft = Nothing
    End Sub

    Private Sub jsConTablaSimple_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)
        FindField = aCampos(0)
        lblBuscar.Text = aNombres(0)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim f As New jsMerArcLotesMercanciaMovimientos
        f.Agregar(myConn, ds, dt, CodigoMercancia)
        If f.Apuntador >= 0 Then AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim f As New jsMerArcLotesMercanciaMovimientos
        f.Apuntador = Me.BindingContext(ds, nTabla).Position
        f.Editar(myConn, ds, dt, CodigoMercancia)
        AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Apuntador = Me.BindingContext(ds, nTabla).Position
        AsignaTXT(Apuntador, False)
        EliminaFilaTS()
    End Sub
    Private Sub EliminaFilaTS()

        Dim aCamposDel() As String = {"codart", "lote", "id_emp"}
        Dim aStringsDel() As String = {CodigoMercancia, dt.Rows(Apuntador).Item("lote"), jytsistema.WorkID}

        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
            AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsmerlotemer", strSQL, aCamposDel, aStringsDel, _
                                     Apuntador, True), True)
        End If
    End Sub
    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        Apuntador = Me.BindingContext(ds, nTabla).Position
        If dt.Rows.Count > 0 AndAlso dg.RowCount > 0 Then
            Seleccion = dg.SelectedRows(0).Cells(0).Value.ToString
            Me.Close()
        End If
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, dg.CellMouseClick

        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        n_Apuntador = e.RowIndex
        AsignaTXT(e.RowIndex, False)
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        f.Buscar(dt, aCampos, aNombres, aAnchos, n_Apuntador, "Lotes de mercancías...")
        AsignaTXT(f.Apuntador, False)
        f = Nothing
    End Sub

    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.ColumnHeaderMouseClick
        FindField = dt.Columns(aCampos(e.ColumnIndex)).ColumnName
        lblBuscar.Text = aNombres(e.ColumnIndex)
    End Sub

    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        BindingSource1.DataSource = dt
        If dt.Columns(FindField).DataType Is GetType(String) Then _
            BindingSource1.Filter = FindField & " like '%" & txtBuscar.Text & "%'"
        dg.DataSource = BindingSource1
    End Sub

End Class