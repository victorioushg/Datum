Public Class FormaDePago

	Public Property NumeroFactura As String
	Public Property SerialCaja As String
	Public Property Origen As String
	Public Property FormaDePago As String
	Public Property NumeroDePago As String
	Public Property NombreDePago As String
	Public Property Importe As Decimal
	Public Property Currency As Integer
	Public Property CurrencyDate As DateTime
	Public Property Vencimiento As Date

	Public Sub New()

	End Sub

End Class

Public Class FormaDePagoYMoneda
	Inherits FormaDePago

	Public Property UnidadMonetaria As String
	Public Property Simbolo As String
	Public Property CodigoIso As String
	Public Property Equivale As Double
	Public ReadOnly Property ImporteReal As Decimal
		Get
			Return Importe / Equivale
		End Get
	End Property

End Class
