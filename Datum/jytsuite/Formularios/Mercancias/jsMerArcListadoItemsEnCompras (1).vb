Imports MySql.Data.MySqlClient

Public Class jsMerArcListadoItemsEnCompras

    Private Const sModulo As String = "LISTADO DE ITEM EN COMPRAS"
    Private Const lRegion As String = ""
    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private nTabla As String = "tblItemsEnFacturas"
    Private strSQL As String


    Dim aCampos() As String = {"numCOM.N° de Factura.150.I.", _
                               "emision.Fecha Emisión.100.C.Fecha", _
                               "cantidad.Cantidad.100.D.Cantidad", _
                               "unidad.UND.50.C.", _
                               "COSTOU.COSTO Unitario.100.D.Numero", _
                               "COSTOTOTDES.COSTO Total.100.D.Numero", _
                               "codPRO.Código PROVEEDOR.100.I."}

    Private n_Apuntador As Long
    Private n_Seleccion As String
    Private n_Precio As Double
    Private n_Unidad As String

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
    Public Property Unidad() As String
        Get
            Return n_Unidad
        End Get
        Set(ByVal value As String)
            n_Unidad = value
        End Set
    End Property
    Public Property Precio() As Double
        Get
            Return n_Precio
        End Get
        Set(ByVal value As Double)
            n_Precio = value
        End Set
    End Property

    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal iCodigoProveedor As String, ByVal iCodigoMercancia As String)
        Try
            strSQL = " select b.NUMCOM, b.emision, a.cantidad, a.COSTOU, a.COSTOTOTDES, b.CODPRO, a.item, a.unidad, a.descrip from " _
                        & " jsprorencom a, jsproenccom b " _
                        & " where " _
                        & " a.NUMCOM = b.NUMCOM AND " _
                        & " a.codpro = b.codpro and " _
                        & " b.CODPRO = '" & iCodigoProveedor & "' AND " _
                        & " a.item = '" & iCodigoMercancia & "' AND " _
                        & " a.id_emp = b.id_emp AND " _
                        & " a.id_emp = '" & jytsistema.WorkID & "'  " _
                        & " union " _
                        & " select nummov NUMCOM, emision, 1 cantidad, 1 COSTOU, abs(importe) COSTOTOTDES , CODPRO,'xx' item, 'xxx' unidad, 'xxx' descrip " _
                        & " from jsprotrapag " _
                        & " where " _
                        & " CODPRO = '" & iCodigoProveedor & "' and " _
                        & " tipomov = 'FC' and origen = 'CXP' and " _
                        & " id_emp = '" & jytsistema.WorkID & "' " _
                        & " order by 2 desc, 1 desc, 7 "

            Me.Text = sModulo
            Me.Tag = sModulo
            CodigoMercancia = iCodigoMercancia
            myConn = MyCon

            dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)
            ft.IniciarTablaPlus(dg, dt, aCampos)

            AsignarTooltips()

            If dt.Rows.Count > 0 Then n_Apuntador = 0

            AsignaTXT(n_Apuntador, False)


            Me.ShowDialog()


        Catch ex As MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        Catch ex As Exception
            '  ft.mensajeCritico("Error " & ex.Message)
        End Try
    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnSeleccionar, btnPrimero, _
                          btnSiguiente, btnAnterior, btnUltimo, btnImprimir, btnSalir)
        ft.colocaImagenesEnBarra(MenuBarra)
        ft.ajustarAnchoForma(Me, dg, MenuBarra)

        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.tipoMensaje.iAyuda)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean)

        dt = ft.MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, _
                                    jytsistema.sUsuario, nRow, Actualiza)

    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        If dt.Rows.Count > 0 AndAlso dg.RowCount > 0 Then
            Apuntador = Me.BindingContext(ds, nTabla).Position
            Seleccion = dg.SelectedRows(0).Cells(0).Value.ToString
            Unidad = dg.SelectedRows(0).Cells(3).Value.ToString
            Precio = CDbl(dg.SelectedRows(0).Cells(4).Value.ToString)
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
        ft = Nothing
        dt.Dispose()
        dt = Nothing
    End Sub
    Private Sub jsMerArcListadoItemsEnCompras_Shown(sender As Object, e As EventArgs) Handles Me.Shown

    End Sub
    Private Sub jsConTablaSimple_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        FindField = aCampos(0)
        lblBuscar.Text = dg.Columns(0).HeaderText

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
       
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        
    End Sub

    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        Apuntador = Me.BindingContext(ds, nTabla).Position
        If dt.Rows.Count > 0 AndAlso dg.RowCount > 0 Then
            Seleccion = dg.SelectedRows(0).Cells(0).Value.ToString
            Unidad = dg.SelectedRows(0).Cells(3).Value.ToString
            Precio = CDbl(dg.SelectedRows(0).Cells(4).Value.ToString)
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
        f.BuscarPlus(dt, aCampos, n_Apuntador, "Listado de ítems en factura...")
        AsignaTXT(f.Apuntador, False)
        f = Nothing
    End Sub

    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.ColumnHeaderMouseClick
        FindField = aCampos(e.ColumnIndex).Split(".")(0)
        lblBuscar.Text = dg.Columns(e.ColumnIndex).HeaderText
    End Sub

    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        BindingSource1.DataSource = dt
        If dt.Columns(FindField).DataType Is GetType(String) Then _
            BindingSource1.Filter = FindField & " like '%" & txtBuscar.Text & "%'"
        dg.DataSource = BindingSource1
    End Sub


   

End Class