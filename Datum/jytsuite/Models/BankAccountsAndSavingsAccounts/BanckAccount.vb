Public Class BankAccount

    Public Property CodigoBanco As String
    Public Property NombreCuenta As String
    Public Property CodigoCuenta As String
    Public Property Currency As Integer
    Public Property SaldoActual As Decimal
    Public Property CodigoContable As String
    Public Property FormatoCheque As String
    Public Property Estatus As Integer

    Public ReadOnly Property DisplayName As String
        Get
            Return CodigoBanco & "|" & NombreCuenta & "|" & CodigoCuenta
        End Get
    End Property

End Class
