Imports MySql.Data.MySqlClient
Public Class jsPOSArcClientes

    Private Const sModulo As String = "Clientes Puntos de Venta"
    Private Const lRegion As String = "RibbonButton266"
    Private Const nTabla As String = "tblClientes"

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private strSQL As String
    Private Posicion As Long
    Private BindingSource1 As New BindingSource
    Private FindField As String
  
    Private Sub jsPOSArcClientes_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt = Nothing
        ds = Nothing
        ft = Nothing
    End Sub

    Private Sub jsPOSArcClientes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        myConn.Open()

        strSQL = " select a.CODCLI, a.nombre, a.rif, a.dirfiscal, a.telef1, a.ingreso " _
            & " from jsvencatclipv a " _
            & " where " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' order by a.nombre "

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        IniciarGrilla()

        AsignarTooltips()
        If dt.Rows.Count > 0 Then Posicion = 0
        AsignaTXT(Posicion, True)
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)

        FindField = "nombre"
        lblBuscar.Text = "Nombre"


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
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean, Optional ByVal NumeroCelda As Integer = 0)

        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        If c >= 0 AndAlso ds.Tables(nTabla).Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = c
            With dt
                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)
            End With
            dg.CurrentCell = dg(NumeroCelda, c)
        End If
        ft.ActivarMenuBarra(myConn, ds, dt, "", MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub IniciarGrilla()
        Dim aCampos() As String = {"rif", "nombre", "dirfiscal", "telef1", ""}
        Dim aNombres() As String = {"RIF", "Nombre y/o Razón Social", "Dirección Fiscal", "Teléfono", ""}
        Dim aAnchos() As Integer = {140, 300, 600, 140, 70}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", "", ""}
        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, , True)

        FindField = "nombre"
        BindingSource1.DataSource = dt
        dg.DataSource = BindingSource1

        dg.ReadOnly = False
        dg.Columns("rif").ReadOnly = True
        dg.Columns("").ReadOnly = True

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        Posicion = Me.BindingContext(ds, nTabla).Position

        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
            Dim aCampos() As String = {"nombre", "rif", "id_emp"}
            Dim aString() As String = {dg.CurrentRow.Cells(1).Value, dg.CurrentRow.Cells(0).Value, jytsistema.WorkID}
            Posicion = EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsvencatclipv", strSQL, aCampos, aString, dg.CurrentRow.Index)
        End If

        AsignaTXT(Posicion, True)
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"nombre", "rif", ""}
        Dim Nombres() As String = {"Nombre", "RIF", ""}
        Dim Anchos() As Integer = {450, 50, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Clientes de Puntos de Venta...")
        AsignaTXT(f.Apuntador, False)
        f = Nothing
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        Posicion = Me.BindingContext(ds, nTabla).Position
        
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

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub



    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        Posicion = Me.BindingContext(ds, nTabla).Position
        
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        Posicion = Me.BindingContext(ds, nTabla).Position
    End Sub

    Private Sub dg_RowValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.RowValidated
        Posicion = Me.BindingContext(ds, nTabla).Position
        Select Case dg.CurrentCell.ColumnIndex
            
            Case 1
                ft.Ejecutar_strSQL(myconn, " update jsvencatclipv set nombre = '" & dg.CurrentCell.Value & "' " _
                                       & " where rif = '" & dg.CurrentRow.Cells(0).Value & "' and id_emp = '" & jytsistema.WorkID & "' ")
                AsignaTXT(Posicion, True, dg.CurrentCell.ColumnIndex)
            Case 2
                ft.Ejecutar_strSQL(myconn, " update jsvencatclipv set dirfiscal = '" & dg.CurrentCell.Value & "' " _
                                       & " where rif = '" & dg.CurrentRow.Cells(0).Value & "' and id_emp = '" & jytsistema.WorkID & "' ")
                AsignaTXT(Posicion, True, dg.CurrentCell.ColumnIndex)
            Case 3
                ft.Ejecutar_strSQL(myconn, " update jsvencatclipv set telef1 = '" & dg.CurrentCell.Value & "' " _
                                       & " where rif = '" & dg.CurrentRow.Cells(0).Value & "' and id_emp = '" & jytsistema.WorkID & "' ")
                AsignaTXT(Posicion, True, dg.CurrentCell.ColumnIndex)
        End Select


    End Sub
    Private Sub dg_CancelRowEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.QuestionEventArgs) Handles dg.CancelRowEdit
        ft.mensajeAdvertencia("Cancelando")
    End Sub
    Private Sub dg_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
            Handles dg.CellValidating

        Dim headerText As String = _
            dg.Columns(e.ColumnIndex).HeaderText

        If Mid(headerText, 1, 3) = "MES" Then Return

    End Sub

    Private Sub dg_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dg.CellEndEdit
        dg.Rows(e.RowIndex).ErrorText = String.Empty
    End Sub
    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.ColumnHeaderMouseClick
        If e.ColumnIndex < 2 Then
            Dim aCam() As String = {"rif", "nombre"}
            Dim aStr() As String = {"RIF", "Nombre"}
            FindField = dt.Columns(aCam(e.ColumnIndex)).ColumnName
            lblBuscar.Text = aStr(e.ColumnIndex)
        End If
    End Sub

    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        BindingSource1.DataSource = dt
        If dt.Columns(FindField).DataType Is GetType(String) Then _
            BindingSource1.Filter = FindField & " like '%" & txtBuscar.Text & "%'"
        dg.DataSource = BindingSource1
    End Sub

    Private Sub dg_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellValidated
        Select Case dg.CurrentCell.ColumnIndex

            Case 1
                ft.Ejecutar_strSQL(myconn, " update jsvencatclipv set nombre = '" & dg.CurrentCell.Value & "' " _
                                       & " where rif = '" & dg.CurrentRow.Cells(0).Value & "' and id_emp = '" & jytsistema.WorkID & "' ")
            Case 2
                ft.Ejecutar_strSQL(myconn, " update jsvencatclipv set dirfiscal = '" & dg.CurrentCell.Value & "' " _
                                       & " where rif = '" & dg.CurrentRow.Cells(0).Value & "' and id_emp = '" & jytsistema.WorkID & "' ")
            Case 3
                ft.Ejecutar_strSQL(myconn, " update jsvencatclipv set telef1 = '" & dg.CurrentCell.Value & "' " _
                                       & " where rif = '" & dg.CurrentRow.Cells(0).Value & "' and id_emp = '" & jytsistema.WorkID & "' ")

        End Select
    End Sub
End Class