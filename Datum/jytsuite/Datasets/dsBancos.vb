Partial Class dsBancos
    Partial Class dtBancosMovimientosDataTable

        Private Sub dtBancosMovimientosDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.fechamovColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

    Partial Class dtConciliacionDataTable

        Private Sub dtConciliacionDataTable_ColumnChanging(sender As Object, e As DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.prov_cliColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

End Class

Namespace dsBancosTableAdapters
    
    Partial Public Class dtCajaMovimientosTableAdapter
    End Class
End Namespace

Namespace dsBancosTableAdapters
    
    Partial Public Class dtDepositosTableAdapter
    End Class
End Namespace

Namespace dsBancosTableAdapters
    
    Partial Public Class dtEstadoCuentaTableAdapter
    End Class
End Namespace
