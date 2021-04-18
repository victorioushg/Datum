Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.DataGrid.Enums
Imports Syncfusion.WinForms.DataGrid
Imports Syncfusion.WinForms.ListView.Enums
Imports Syncfusion.WinForms.Input.Enums

Public Class jsControlArcCambioMonedas

    Private Const sModulo As String = "Monedas"
    Private Const lRegion As String = "RibbonButton185"
    Private Const nTabla As String = "monedas"
    Private Const nTablaCurrencies As String = "Currencies"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private listOfCambios As New List(Of CambioMoneda)()
    Private dtCurrencies As New DataTable
    Private ft As New Transportables

    'Private strSQL As String = "SELECT a.fecha, a.moneda, b.equivale FROM (Select MAX(fecha) fecha, moneda " +
    '        " FROM jsconctacam " +
    '        " WHERE fecha <=  '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' " +
    '        " GROUP BY moneda) a " +
    '        " LEFT JOIN jsconctacam b ON (a.fecha = b.fecha AND a.moneda = b.moneda) " +
    '        " ORDER BY a.moneda "

    Private strSQL As String = "SELECT fecha, moneda, equivale " +
            " FROM jsconctacam " +
            " WHERE fecha <  '" & ft.FormatoFechaMySQL(DateAdd("d", 1, jytsistema.sFechadeTrabajo)) & "' " +
            " ORDER BY fecha desc, moneda "

    Private strSqlCurrency As String = "select concat(a.UnidadMonetaria, ' | ', a.simbolo) PaisMoneda,  a.id, " +
        " a.unidadmonetaria, a.simbolo, a.codigoiso, GROUP_CONCAT(a.pais SEPARATOR '| ') paises " +
        " FROM jsconcatmon a GROUP BY a.unidadmonetaria "


    Private Posicion As Long
    Private defaultChange As Integer

    Private n_Seleccionado As String
    Public Property Seleccionado() As String
        Get
            Return n_Seleccionado
        End Get
        Set(ByVal value As String)
            n_Seleccionado = value
        End Set
    End Property
    Public Sub Cargar(ByVal Mycon As MySqlConnection, ByVal TipoCarga As Integer)

        ' 0 = show() ; 1 = showdialog()
        myConn = Mycon

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        listOfCambios = GetCambios(dt)

        ds = DataSetRequery(ds, strSqlCurrency, myConn, nTablaCurrencies, lblInfo)
        dtCurrencies = ds.Tables(nTablaCurrencies)

        defaultChange = Convert.ToInt32(ft.Ejecutar_strSQL_DevuelveScalar(myConn, " select monedacambio from jsconctaemp where id_emp = '" + jytsistema.WorkID + "' "))

        IniciarGrilla()
        AsignarTooltips()
        If dt.Rows.Count > 0 Then Posicion = 0
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.tipoMensaje.iAyuda)

        If TipoCarga = 0 Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If

    End Sub
    Private Sub jsControlMonedas_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt = Nothing
        ds = Nothing
    End Sub

    Private Sub jsControlMonedas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
    End Sub

    Private Sub IniciarGrilla()

        SfDataGrid1.Style.AddNewRowStyle.BackColor = Color.DarkCyan
        SfDataGrid1.Style.AddNewRowStyle.TextColor = Color.White

        SfDataGrid1.ThemeName = "Office2016DarkGray"
        SfDataGrid1.AutoGenerateColumns = False
        SfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill
        SfDataGrid1.Columns.Add(New GridComboBoxColumn() With {.MappingName = "Moneda", .HeaderText = "Pais/Moneda",
                                .ValueMember = "ID",
                                .DisplayMember = "PaisMoneda",
                                .DataSource = dtCurrencies,
                                .AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                                .DropDownStyle = DropDownStyle.DropDown})

        SfDataGrid1.Columns.Add(New GridNumericColumn() With {
                                .MappingName = "Equivale",
                                .HeaderText = "Equivale",
                                .Width = 250,
                                .Format = cFormatoNumero})
        SfDataGrid1.Columns.Add(New GridDateTimeColumn() With {
                                .MappingName = "Fecha",
                                .HeaderText = "Fecha",
                                .AllowNull = True,
                                .MinDateTime = DateTime.Now,
                                .Width = 150,
                                .Pattern = DateTimePattern.Custom,
                                .Format = "dd-MM-yyyy HH:mm:ss"})

        'SfDataGrid1.Columns.Add(New GridButtonColumn() With {.MappingName = "Eliminar", .HeaderText = "Eliminar",
        '                        .Image = My.Resources.Eliminar, .ImageSize = New Size(16, 16),
        '                        .Width = 70})

        'SfDataGrid1.Columns("Eliminar").CellStyle.HorizontalAlignment = HorizontalAlignment.Center
        SfDataGrid1.Columns("Fecha").CellStyle.HorizontalAlignment = HorizontalAlignment.Center

        SfDataGrid1.Columns("Equivale").CellStyle.HorizontalAlignment = HorizontalAlignment.Right

        SfDataGrid1.AddNewRowPosition = RowPosition.Top
        SfDataGrid1.AddNewRowText = "doble click para agregar una nueva fila"
        SfDataGrid1.NewItemPlaceholderPosition = Syncfusion.Data.NewItemPlaceholderPosition.AtBeginning

        SfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill
        SfDataGrid1.AllowEditing = True
        SfDataGrid1.Columns(1).AllowEditing = True
        SfDataGrid1.DataSource = listOfCambios

    End Sub
    Private Sub SfDataGrid1_RowValidated(sender As Object, e As Events.RowValidatedEventArgs) Handles SfDataGrid1.RowValidated

        Dim data As CambioMoneda = DirectCast(e.DataRow.RowData, CambioMoneda)
        Dim newRow As Boolean = SfDataGrid1.IsAddNewRowIndex(e.DataRow.RowIndex)
        InsertEditCONTROLCambioMoneda(myConn, lblInfo, newRow, data)

        dt.Rows.Clear()
        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        listOfCambios = GetCambios(dt)

        'SfDataGrid1.DataSource = listOfCambios

    End Sub
    Private Sub SfDataGrid1_RowValidating(sender As Object, e As Events.RowValidatingEventArgs) Handles SfDataGrid1.RowValidating
        If SfDataGrid1.IsAddNewRowIndex(e.DataRow.RowIndex) Then
            Dim data = DirectCast(e.DataRow.RowData, CambioMoneda)
            If (String.IsNullOrEmpty(data.Fecha) Or String.IsNullOrEmpty(data.Moneda) Or String.IsNullOrEmpty(data.Equivale)) Then
                e.ErrorMessage = "Campo(s) vacios, por favor verifique... "
                e.IsValid = False
            End If
        End If
    End Sub

    Private Sub SfDataGrid1_CurrentCellBeginEdit(sender As Object, e As Events.CurrentCellBeginEditEventArgs) Handles SfDataGrid1.CurrentCellBeginEdit
        If Not SfDataGrid1.IsAddNewRowIndex(e.DataRow.RowIndex) And (
            e.DataColumn.GridColumn.MappingName = "Moneda" Or e.DataColumn.GridColumn.MappingName = "Fecha") Then
            e.Cancel = True
        End If
    End Sub

    Private Sub SfDataGrid1_AddNewRowInitiating(sender As Object, e As Events.AddNewRowInitiatingEventArgs) Handles SfDataGrid1.AddNewRowInitiating
        Dim data = TryCast(e.NewObject, CambioMoneda)
        data.Moneda = defaultChange
        data.Equivale = 0.00
    End Sub

    Private Function GetCambios(dt As DataTable) As List(Of CambioMoneda)
        Dim cambios As New List(Of CambioMoneda)()
        For i As Integer = 0 To dt.Rows.Count - 1
            Dim cambio As CambioMoneda = New CambioMoneda()
            cambio.Fecha = dt.Rows(i).Item(0)
            cambio.Moneda = dt.Rows(i).Item(1)
            cambio.Equivale = dt.Rows(i).Item(2)
            cambios.Add(cambio)
        Next i
        Return cambios
    End Function

End Class

