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
End Class