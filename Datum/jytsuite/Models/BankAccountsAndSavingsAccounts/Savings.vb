
Public Class Saving

    Public Property Codigo() As String
    Public Property Descripcion() As String
    Public Property CodigoContable() As String
    Public Property Saldo() As Decimal
    Public Property Currency() As Integer

    Public ReadOnly Property DisplayNamne As String
        Get
            Return Codigo & " | " & Descripcion
        End Get
    End Property

End Class

Public Class SavingWithLines
    Inherits Saving
    Public Property Lines() As List(Of SavingLines)
End Class

Public Class SavingLines

    Public Property Codigo() As String
    Public Property Id() As Long
    Public Property Fecha() As Date
    Public Property Origen() As String
    Public Property TipoMovimiento() As String
    Public Property NumeroMovimiento() As String
    Public Property FormaDePago() As String
    Public Property NumeroDePago() As String
    Public Property ReferenciaDePago() As String
    Public Property Importe() As Decimal
    Public Property Currency() As Integer
    Public Property Currency_Date() As DateTime
    Public Property CodigoContable() As String
    Public Property Concepto() As String
    Public Property Deposito() As String
    Public Property FechaDeposito() As Date
    Public Property CantidadDocumentos() As Integer
    Public Property CodigoBancario As String
    Public Property MultiCancelacion() As String
    Public Property Asiento() As String
    Public Property FechaAsiento() As Date
    Public Property ProveedorCliente() As String
    Public Property CodigoVendedor() As String
    Public Property FechaBloqueo() As Date

    Public Property ImporteReal() As Decimal
    Public Property CodigoIso() As String

End Class

