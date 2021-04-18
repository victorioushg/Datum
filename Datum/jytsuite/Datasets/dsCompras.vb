Partial Class dsCompras
    Partial Class dtRetencionesIVADataTable

        Private Sub dtRetencionesIVADataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.emision1Column.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

    Partial Class dtMovimientosDataTable

        Private Sub dtMovimientosDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.asignadoColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

    Partial Class dtLibroIVADataTable

        Private Sub dtLibroIVADataTable_ColumnChanging(sender As System.Object, e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.emision1Column.ColumnName) Then
                'Add user code here
            End If

        End Sub

        Private Sub dtLibroIVADataTable_dtLibroIVARowChanging(sender As System.Object, e As dtLibroIVARowChangeEvent) Handles Me.dtLibroIVARowChanging

        End Sub

    End Class

End Class
