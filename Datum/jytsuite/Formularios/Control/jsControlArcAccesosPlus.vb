Imports MySql.Data.MySqlClient

Public Class jsControlArcAccesosPlus
    Private Const sModulo As String = "Registro de auditoría de accesos"
    

    Private strSQL As String
    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private ft As New Transportables

    Private nTabla As String = "tblmovaud"

    Private i_modo As Integer
    Private Posicion As Long

    Private n_Seleccionado As String
    Private BindingSource1 As New BindingSource
    Private FindField As String

    Public Property Seleccionado() As String
        Get
            Return n_Seleccionado
        End Get
        Set(ByVal value As String)
            n_Seleccionado = value
        End Set
    End Property

    Public Property nModulo As String

    Private Sub jsConUsuarios_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        ds = Nothing
        dt = Nothing
        myConn.Close()
        myConn = Nothing
    End Sub
    Public Sub Cargar()
        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo
        Me.Text = sModulo & " : " & nModulo
        Try
            myConn.Open()

            AsignarTooltips()
            AbrirAuditoria()
            If dt.Rows.Count > 0 Then AsignaTXT(Posicion = 0)


            ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)

            Me.ShowDialog()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try
    End Sub
    
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnSeleccionar, _
                          btnPrimero, btnSiguiente, btnAnterior, btnUltimo, btnImprimir, btnSalir)

    End Sub
   
    Private Sub AbrirAuditoria()

        strSQL = " SELECT a.fecha, a.maquina, a.id_user, b.nombre, a.numdoc , ELT(a.tipomov,  " _
            & " 'Entrar', 'Salir', 'Incluir', 'Editar','Eliminar','Seleccionar', 'Imprimir', 'Procesar') TipoMovimiento  " _
            & " FROM jsconregaud a " _
            & " LEFT JOIN jsconctausu b ON (a.id_user = b.id_user ) " _
            & " WHERE " _
            & " a.tipomov > 2 AND " _
            & " a.fecha >=  '" & ft.FormatoFechaMySQL(DateAdd(DateInterval.Year, -1, jytsistema.sFechadeTrabajo)) & "' AND " _
            & " a.modulo = '" & nModulo & "' AND  " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY a.fecha DESC "
        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        IniciarGrilla()

    End Sub
    Private Sub IniciarGrilla()

        Dim aCampos() As String = {"fecha", "maquina", "id_user", "nombre", "numdoc", "TipoMovimiento"}
        Dim aNombres() As String = {"Fecha", "Máquina", "Usuario", "Nombre Usuario", "Nº Documento/Item/etc.", "Tipo Movimiento"}
        Dim aAnchos() As Integer = {180, 150, 70, 300, 200, 150}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {sFormatoFechaHoraMySQL, "", "", "", "", ""}

        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

        FindField = "numdoc"
        BindingSource1.DataSource = dt
        BindingSource1.Filter = " numdoc like '" & txtBuscar.Text & "%'"
        dg.DataSource = BindingSource1

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)
        Dim c As Integer = CInt(nRow)
        If c >= 0 AndAlso ds.Tables(nTabla).Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = c
            With dt
                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)
            End With
            dg.CurrentCell = dg(0, c)
        End If
        Posicion = nRow
        ft.ActivarMenuBarra(myConn, ds, dt, "", MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        '
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

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        '
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click

        Me.Close()
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.CellMouseClick
        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        AsignaTXT(e.RowIndex)
    End Sub
    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.ColumnHeaderMouseClick
        FindField = dt.Columns(e.ColumnIndex).ColumnName
        lblBuscar.Text = dg.Columns(e.ColumnIndex).HeaderText

        txtBuscar.Focus()
    End Sub

    Private Sub btnBuscar_Click(sender As System.Object, e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"fecha", "maquina", "id_user", "nombre", "numdoc", "TipoMovimiento"}
        Dim Nombres() As String = {"Fecha", "Máquina", "Usuario", "Nombre Usuario", "Nº Documento/Item/etc.", "Tipo Movimiento"}
        Dim Anchos() As Integer = {180, 150, 70, 300, 200, 150}
        f.Text = Me.Text
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, Me.Text)
        Posicion = f.Apuntador
        Me.BindingContext(ds, nTabla).Position = Posicion
        AsignaTXT(Posicion)
        f.Dispose()
        f = Nothing
    End Sub

    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        BindingSource1.DataSource = dt
        If dt.Columns(FindField).DataType Is GetType(String) Then _
            BindingSource1.Filter = FindField & " like '%" & txtBuscar.Text & "%'"
        dg.DataSource = BindingSource1
    End Sub

End Class