Imports MySql.Data.MySqlClient
Public Class jsControlArcContadores
    Private Const sModulo As String = "Contadores de sistema"
    Private Const lRegion As String = "RibbonButton172"
    Private Const nTabla As String = "contadores"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private strSQL As String
    Private Posicion As Long

    Private n_Seleccionado As String

    Private iiGestion As Integer

    Public Sub Cargar(ByVal Mycon As MySqlConnection)

        myConn = Mycon
        ft.RellenaCombo(aGestion, cmbGestion)
        IniciarModulo(cmbGestion.SelectedIndex)
        AsignarTooltips()
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)

        Me.Show()

    End Sub
    Private Sub IniciarModulo(ByVal numModulo As Integer)

        strSQL = "select * from jsconcontadores where gestion = " & numModulo + 1 & " and id_emp = '" & _
            jytsistema.WorkID & "' order by descripcion "
        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        IniciarGrilla()
        If dt.Rows.Count > 0 Then Posicion = 0
        AsignaTXT(Posicion, True)

    End Sub
    Private Sub jsControlContadores_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt = Nothing
        ds = Nothing
    End Sub

    Private Sub jsControlContadores_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
        C1SuperTooltip1.SetToolTip(btnReconstruir, "<B>Reconstruir</B> contadores ")

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean)

        Dim c As Integer = CInt(nRow)

        If Actualiza Then ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)

        If c >= 0 AndAlso ds.Tables(nTabla).Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = c
            With dt
                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)
            End With
            dg.CurrentCell = dg(0, c)
        Else
            MostrarItemsEnMenuBarra(MenuBarra, 0, 0)
        End If
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
        btnReconstruir.Enabled = True

    End Sub
    Private Sub IniciarGrilla()
        Dim aCampos() As String = {"descripcion", "contador"}
        Dim aNombres() As String = {"Descripción", "contador"}
        Dim aAnchos() As Integer = {600, 300}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha}
        Dim aFormatos() As String = {"", ""}
        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, , True)

        dg.ReadOnly = False
        dg.Columns("descripcion").ReadOnly = True


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

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dg.CellFormatting
        Select Case dg.Columns(e.ColumnIndex).Name
            Case "gestion"
                e.Value = aGestion(e.Value - 1)
            Case "descripcion"
                If e.Value IsNot Nothing Then
                    Dim stringValue As String = CType(e.Value, String)
                    If InStr("0. 1. 2. 3. 4. 5. 6. 7. 8. 9. 10. ", Mid(stringValue, 1, 3)) > 0 Then
                        e.CellStyle.Font = New Font("Consolas", e.CellStyle.Font.Size, FontStyle.Bold)
                    End If
                End If
        End Select
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.RegionChanged

        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        AsignaTXT(e.RowIndex, False)
    End Sub


    Private Sub btnReconstruir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReconstruir.Click
        IniciarContadores(myConn, lblInfo)
        IniciarModulo(cmbGestion.SelectedIndex)
    End Sub
    Private Sub dg_RowValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles _
        dg.RowValidated
        Dim codigoCampo As String = dt.Rows(dg.CurrentCell.RowIndex).Item("codigo").ToString
        If dg.CurrentCell.ColumnIndex = 1 Then

            ft.Ejecutar_strSQL(myconn, " update jsconcontadores set contador = '" & dg.CurrentCell.Value & "' " _
                            & " where codigo = '" & codigoCampo & "' and id_emp = '" & jytsistema.WorkID & "' ")
        End If
    End Sub

    Private Sub dg_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) Handles _
        dg.CellValidating


        Dim headerText As String = _
            dg.Columns(e.ColumnIndex).HeaderText

        If Not headerText.Equals("contador") Then Return

    End Sub

    Private Sub dg_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles _
        dg.CellEndEdit
        dg.Rows(e.RowIndex).ErrorText = String.Empty
    End Sub

    Private Sub cmbGestion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbGestion.SelectedIndexChanged
        IniciarModulo(cmbGestion.SelectedIndex)
    End Sub

    Private Sub btnEliminar_Click(sender As System.Object, e As System.EventArgs) Handles btnEliminar.Click

    End Sub
End Class