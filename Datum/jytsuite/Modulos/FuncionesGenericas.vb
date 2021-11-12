Imports System.Reflection
Imports MySql.Data.MySqlClient

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



End Module
