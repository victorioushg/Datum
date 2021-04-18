Imports MySql.Data.MySqlClient
Imports fTransport
Public Class jsControlArcAccesos
    Private Const sModulo As String = "Registro de auditoría de accesos"
    Private Const lRegion As String = "RibbonButton169"
    Private Const strSQLUsuarios As String = " select * from jsconctausu order by id_user "

    Private Const nTabla As String = "Usuarios"

    Private strSQL As String
    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private tblMov As String = "tblmovaud"
    Private dtMov As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private i As Integer
    Private Posicion As Long

    Private Sub jsConUsuarios_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ds = Nothing
        dt = Nothing
        ft = Nothing
        myConn.Close()
        myConn = Nothing
    End Sub

    Private Sub jsConUsuarios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo

        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQLUsuarios, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)


            AsignarTooltips()

            If dt.Rows.Count > 0 Then Posicion = 0
            AsignaTXT(Posicion, True)
            ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try
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

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean)

        If Actualiza Then ds = DataSetRequery(ds, strSQLUsuarios, myConn, nTabla, lblInfo)
        With dt
            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)
            With .Rows(nRow)
                txtID_User.Text = .Item("id_user")
                txtNombre.Text = IIf(IsDBNull(.Item("nombre")), "", .Item("nombre"))
                AbrirRenglonUsuario(.Item("id_user"))
            End With
        End With
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub AbrirRenglonUsuario(ByVal nUser As String)

        strSQL = "select * " _
                    & " from jsconregaud " _
                    & " where " _
                    & " id_user = '" & nUser & "' and " _
                    & " fecha >= '" & ft.FormatoFechaMySQL(DateAdd(DateInterval.Day, -1, jytsistema.sFechadeTrabajo)) & "' " _
                    & " order by fecha desc "

        ds = DataSetRequery(ds, strSQL, myConn, tblMov, lblInfo)
        dtMov = ds.Tables(tblMov)
        IniciarGrilla()

    End Sub
    Private Sub IniciarGrilla()

        Dim aCampos() As String = {"fecha", "maquina", "gestion", "modulo", "tipomov", "numdoc", "ID_EMP", "control"}
        Dim aNombres() As String = {"Fecha", "Máquina", "Gestión", "Módulo", "Tipo Movimiento", "Nº Documento", "Empresa", ""}
        Dim aAnchos() As Integer = {150, 130, 150, 300, 100, 100, 100, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {"", "", "", "", "", "", "", ""}

        IniciarTabla(dg, dtMov, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        '  Dim f As New jsControlArcUsuariosMovimientos
        '  f.Agregar(myConn, ds, dt)
        '  If f.Apuntador >= 0 Then AsignaTXT(f.Apuntador, True)
        '  f = Nothing
    End Sub
    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
     
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        '
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Me.BindingContext(ds, nTabla).Position = 0
        AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Me.BindingContext(ds, nTabla).Position -= 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Me.BindingContext(ds, nTabla).Position += 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        '
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click

        Me.Close()
    End Sub

    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dg.CellFormatting

        Select Case dg.Columns(e.ColumnIndex).Name
            Case "gestion"
                e.Value = aGestion(e.Value)
            Case "tipomov"
                Dim aTipo() As String = {"Entrar", "Salir", "Incluir", "Modificar", "Eliminar", "Seleccionar", "Imprimir", "Procesar"}
                e.Value = aTipo(e.Value - 1)
        End Select
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.CellMouseClick
        ' Me.BindingContext(ds, nTabla).Position = e.RowIndex
        ' AsignaTXT(e.RowIndex, False)
    End Sub


    Private Sub dg_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub
End Class