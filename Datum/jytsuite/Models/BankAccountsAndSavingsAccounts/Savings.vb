
Public Class Saving

    Public Property Codigo() As String
    Public Property Descripcion() As String
    Public Property CodigoContable() As String
    Public Property Saldo() As Decimal
    Public Property Currency() As Integer

    Public ReadOnly Property DisplayName As String
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


Public Class BankLine

    Public Property Id() As Long
    Public Property FechaMovimiento() As DateTime
    Public Property NumeroMovimiento() As String
    Public Property TipoMovimiento() As String
    Public Property Concepto() As String
    Public Property Importe() As Decimal
    Public Property Currency As Integer
    Public Property CurrencyDate As DateTime
    Public Property Origen As String
    Public Property NumeroOrigen As String
    Public Property TipoOrigen As String
    Public Property Beneficiario As String
    Public Property Comprobante As String
    Public Property Conciliado As String
    Public Property MesConciliacion As Date
    Public Property FechaConciliacion As Date
    Public Property Asiento As String
    Public Property FechaAsiento As Date
    Public Property Multicancelacion As String
    Public Property CodigoBanco As String
    Public Property CodigoCaja As String
    Public Property ProveedorCliente As String
    Public Property CodigoVendedor As String
    Public Property FechaBloqueo As Date

    Public Property ImporteReal() As Decimal
    Public Property CodigoIso() As String

End Class