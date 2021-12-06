Public Class RetencionesISLR
    Public Property CodigoRetencion As String
    Public Property Concepto As String
    Public Property BasseImponible As Decimal
    Public Property Tarifa As Double
    Public Property PagoMinimo As Decimal
    Public Property Menos As Decimal
    Public Property Persona As String
    Public Property Acumula As String
    Public Property Tipo As Integer
    Public Property Comentario As String
    Public Property CodigoContable As String
    Public ReadOnly Property DisplayName As String
        Get
            Dim Tipos() As String = {"PNR", "PNNR", "PJD", "PJND", "PJNCD"}
            Return CodigoRetencion & "-" & Tipos(Tipo) & "-" & Concepto
        End Get
    End Property

End Class
