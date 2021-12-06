Imports System.Reflection
Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.ListView.Enums
Imports Syncfusion.WinForms.ListView '


Module FuncionesGenericas


    Public Function Lista(Of T)(MyConn As MySqlConnection, strSQL As String) As List(Of T)

        Dim data As List(Of T) = New List(Of T)()
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim nTabla = "tbl"
        Dim lblInfo As New Label
        ds = DataSetRequery(ds, strSQL, MyConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        data = ConvertDataTable(Of T)(dt)
        Return data

    End Function

    Public Function ConvertDataTable(Of T)(dt As DataTable) As List(Of T)
        Dim data As List(Of T) = New List(Of T)()

        For Each row As DataRow In dt.Rows
            Dim item As T = GetItem(Of T)(row)
            data.Add(item)
        Next

        Return data

    End Function

    Private Function GetItem(Of T)(dr As DataRow) As T

        Dim temp As Type = GetType(T)
        Dim obj As T = Activator.CreateInstance(Of T)()

        For Each column As DataColumn In dr.Table.Columns
            For Each pro As PropertyInfo In temp.GetProperties()
                If pro.Name.ToLower() = column.ColumnName.ToLower() Then
                    Dim convertedValue =
                        IIf(IsDBNull(dr(column.ColumnName)),
                            Nothing,
                            Convert.ChangeType(dr(column.ColumnName), pro.PropertyType))

                    pro.SetValue(obj, convertedValue, Nothing)

                End If
            Next
        Next
        Return obj
    End Function

    Public Function ToDataTable(Of T)(items As List(Of T)) As DataTable

        Dim datTable As DataTable = New DataTable(GetType(T).Name)
        '' Get all the properties by using reflection   
        Dim Props As PropertyInfo() = GetType(T).GetProperties(BindingFlags.Public Or BindingFlags.Instance)
        For Each prop As PropertyInfo In Props
            '' Setting column names as Property names  
            datTable.Columns.Add(prop.Name)
        Next
        For Each item As T In items
            Dim values(Props.Length - 1) As Object
            For i As Integer = 0 To i < (Props.Length - 1)
                values(i) = Props(i).GetValue(item, Nothing)
            Next
            datTable.Rows.Add(values)
        Next

        Return datTable
    End Function


    Public Enum Tipo
        Defecto = 0
        FormaDePago = 1
        Transportes = 2
        Almacenes = 3
        CondicionDePago = 4
        TipoMovimientoCaja = 5
        ZonaProveedor = 6
        TipoMovimientoBanco = 7
        TipoDeposito = 8
        TipoMovimientoCxP = 9
    End Enum
    Public Function InitiateDropDown(Of T)(MyConn As MySqlConnection, cmb As SfComboBox,
                                           Optional tipo As Tipo = Tipo.Defecto,
                                           Optional defaultIndex As Integer = -1,
                                           Optional campoOptional As String = "") As List(Of T)
        Dim list As New List(Of T)
        cmb.DropDownStyle = DropDownStyle.DropDown
        Select Case GetType(T)
            '' Compras 
            Case GetType(Vendor)
                cmb.DisplayMember = "Nombre"
                cmb.ValueMember = "Codigo"
                cmb.Watermark = "Escriba o seleccione un Proveedor"
                list = GetVendorList(MyConn).Cast(Of T)
            Case GetType(CreditCause)
                cmb.DisplayMember = "Descripcion"
                cmb.ValueMember = "Codigo"
                cmb.Watermark = "Escriba o seleccione una Causa para el Crédito"
                list = GetCreditCauses(MyConn, campoOptional).Cast(Of T)
                cmb.DropDownStyle = DropDownStyle.DropDownList
            Case GetType(RetencionesISLR)
                cmb.DisplayMember = "DisplayName"
                cmb.ValueMember = "CodigoRetencion"
                list = GetRetencionesISLRList(MyConn).Cast(Of T)
            '' Ventas
            Case GetType(Customer)
                cmb.DisplayMember = "Nombre"
                cmb.ValueMember = "Codcli"
                cmb.Watermark = "Escriba o seleccione un Cliente"
                list = GetCustomersList(MyConn).Cast(Of T)
            Case GetType(SalesForce)
                cmb.DisplayMember = "NombreAsesor"
                cmb.ValueMember = "Codigo"
                cmb.Watermark = "Escriba o seleccione un Asesor Comerciaal"
                list = GetSalesForce(MyConn).Cast(Of T)
            '' Generales
            Case GetType(TextoValor)
                cmb.DisplayMember = "Text"
                cmb.ValueMember = "Value"
                If (tipo = Tipo.FormaDePago) Then
                    list = formasDePago.Cast(Of T)
                ElseIf (tipo = Tipo.CondicionDePago) Then
                    list = condicionesDePago.Cast(Of T)
                ElseIf (tipo = Tipo.TipoMovimientoCaja) Then
                    list = tipoMovimientoCaja.Cast(Of T)
                ElseIf (tipo = Tipo.TipoMovimientoBanco) Then
                    list = tipoMovimientoBanco.Cast(Of T)
                ElseIf (tipo = Tipo.TipoDeposito) Then
                    list = tipoDepositoBanco.Cast(Of T)
                ElseIf (tipo = Tipo.TipoMovimientoCxP) Then
                    list = tipoMovimientoCXP.Cast(Of T)
                End If
                cmb.DropDownStyle = DropDownStyle.DropDownList
            Case GetType(SimpleTable)
                cmb.DisplayMember = "Descripcion"
                cmb.ValueMember = "Codigo"
                If (tipo = Tipo.Almacenes) Then
                    list = GetWarehouseList(MyConn).Cast(Of T)
                ElseIf (tipo - Tipo.Transportes) Then
                    list = GetTransportList(MyConn).Cast(Of T)
                ElseIf (tipo = Tipo.ZonaProveedor) Then
                    list = GetVendorZonesList(MyConn).Cast(Of T)
                End If
                cmb.DropDownStyle = DropDownStyle.DropDownList
            '' Bancos y Cajas
            Case GetType(Saving)
                cmb.DisplayMember = "DisplayName"
                cmb.ValueMember = "Codigo"
                list = GetCashBoxes(MyConn).Cast(Of T)
                cmb.DropDownStyle = DropDownStyle.DropDownList
            Case GetType(BankAccount)
                cmb.DisplayMember = "DisplayName"
                cmb.ValueMember = "CodigoBanco"
                list = GetBanksAccounts(MyConn).Cast(Of T)
                cmb.DropDownStyle = DropDownStyle.DropDownList
            '' Coontabilidad
            Case GetType(AccountBase)
                cmb.DisplayMember = "DisplayName"
                cmb.ValueMember = "CodigoContable"
                list = GetResultAccounts(MyConn).Cast(Of T)
        End Select

        cmb.DataSource = list
        cmb.SelectedIndex = defaultIndex
        cmb.AutoCompleteMode = AutoCompleteMode.Suggest
        cmb.AutoCompleteSuggestMode = AutoCompleteSuggestMode.Contains
        cmb.MaxDropDownItems = 10
        cmb.Style.EditorStyle.WatermarkForeColor = Color.LightGray

        Return list
    End Function

End Module
