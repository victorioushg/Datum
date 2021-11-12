Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.DataGrid
Imports Syncfusion.WinForms.DataGrid.Enums


Public Class jsControlArcMonedas

    Private Const sModulo As String = "Monedas"
    Private Const lRegion As String = "RibbonButton185"
    Private Const nTabla As String = "Currencies"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private listOfMonedas As New List(Of Moneda)()

    Private ft As New Transportables

    Private strSQL As String = "select * FROM jsconcatmon order by pais "
    Private Posicion As Long

    Public Sub Cargar(ByVal Mycon As MySqlConnection, ByVal TipoCarga As Integer)

        ' 0 = show() ; 1 = showdialog()
        myConn = Mycon

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        listOfMonedas = GetMonedas(dt)
        IniciarGrilla()
        If dt.Rows.Count > 0 Then Posicion = 0
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.tipoMensaje.iAyuda)

        Me.Show()

    End Sub

    Private Sub jsControlMonedas_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt = Nothing
        ds = Nothing
    End Sub

    Private Sub IniciarGrilla()

        dataGrid.Style.AddNewRowStyle.BackColor = Color.DarkCyan
        dataGrid.Style.AddNewRowStyle.TextColor = Color.White

        dataGrid.ThemeName = "Office2016DarkGray"
        dataGrid.AutoGenerateColumns = False
        dataGrid.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill
        dataGrid.Columns.Add(New GridNumericColumn() With {.MappingName = "Id", .HeaderText = "Id", .Visible = False})
        dataGrid.Columns.Add(New GridTextColumn() With {.MappingName = "Pais", .HeaderText = "Pais", .Width = 250})
        dataGrid.Columns.Add(New GridTextColumn() With {.MappingName = "UnidadMonetaria", .HeaderText = "Moneda", .Width = 250})
        dataGrid.Columns.Add(New GridTextColumn() With {.MappingName = "Simbolo", .HeaderText = "Simbolo", .Width = 100})
        dataGrid.Columns.Add(New GridTextColumn() With {.MappingName = "CodigoISO", .HeaderText = "Código ISO", .Width = 100})
        dataGrid.Columns.Add(New GridTextColumn() With {.MappingName = "UnidadFraccionaria", .HeaderText = "Unidad Fraccionaria", .Width = 150})
        dataGrid.Columns.Add(New GridNumericColumn() With {.MappingName = "Division", .HeaderText = "Fraccion", .Width = 100, .Format = cFormatoEntero})
        dataGrid.Columns.Add(New GridTextColumn() With {.MappingName = "Notas", .HeaderText = "Notas", .Width = 250})

        dataGrid.Columns("Division").CellStyle.HorizontalAlignment = HorizontalAlignment.Right
        dataGrid.AddNewRowPosition = RowPosition.Top
        dataGrid.AddNewRowText = "doble click para agregar una nueva fila"
        dataGrid.NewItemPlaceholderPosition = Syncfusion.Data.NewItemPlaceholderPosition.AtBeginning

        dataGrid.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill
        dataGrid.AllowEditing = True
        dataGrid.DataSource = listOfMonedas

    End Sub

    Private Sub dataGrid_RowValidated(sender As Object, e As Events.RowValidatedEventArgs) Handles dataGrid.RowValidated

        Dim data As Moneda = DirectCast(e.DataRow.RowData, Moneda)
        Dim newRow As Boolean = dataGrid.IsAddNewRowIndex(e.DataRow.RowIndex)
        InsertEditCONTROLMoneda(myConn, lblInfo, newRow, data)
        dt.Rows.Clear()
        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        listOfMonedas = GetMonedas(dt)

    End Sub
    Private Sub dataGrid_RowValidating(sender As Object, e As Events.RowValidatingEventArgs) Handles dataGrid.RowValidating
        If dataGrid.IsAddNewRowIndex(e.DataRow.RowIndex) Then
            Dim data = DirectCast(e.DataRow.RowData, Moneda)
            If (String.IsNullOrEmpty(data.Pais) _
                Or String.IsNullOrEmpty(data.UnidadMonetaria) _
                Or String.IsNullOrEmpty(data.Simbolo) _
                Or String.IsNullOrEmpty(data.CodigoISO)
                ) Then
                e.ErrorMessage = "Campo(s) vacios, por favor verifique... "
                e.IsValid = False
            End If
        End If
    End Sub

    Private Sub dataGrid_CurrentCellBeginEdit(sender As Object, e As Events.CurrentCellBeginEditEventArgs) Handles dataGrid.CurrentCellBeginEdit
        '-' Not empty columns on adding / editting??
        'If Not dataGrid.IsAddNewRowIndex(e.DataRow.RowIndex) And (
        '    e.DataColumn.GridColumn.MappingName = "Moneda" Or e.DataColumn.GridColumn.MappingName = "Fecha") Then
        '    e.Cancel = True
        'End If
    End Sub

    Private Sub dataGrid_AddNewRowInitiating(sender As Object, e As Events.AddNewRowInitiatingEventArgs) Handles dataGrid.AddNewRowInitiating
        '-' Setting default values 
        Dim data = TryCast(e.NewObject, Moneda)
        data.Division = 0
    End Sub

    Private Function GetMonedas(dt As DataTable) As List(Of Moneda)
        Dim monedas As New List(Of Moneda)()
        monedas = ConvertDataTable(Of Moneda)(dt)
        Return monedas
    End Function

End Class