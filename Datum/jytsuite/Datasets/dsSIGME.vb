Partial Class dsSIGME
    Partial Class dtDescuentosAsesorDataTable

        Private Sub dtDescuentosAsesorDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.zonaColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

    Partial Class dtGananciasItemDataTable

        Private Sub dtGananciasItemDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.tipjerColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

End Class
