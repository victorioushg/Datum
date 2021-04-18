Public Class CambioMoneda

	Private _fecha As Date
	Private _moneda As String
	Private _equivale As Double
	'Private _id_emp As String

	Public Property Fecha() As Date
		Get
			Return _fecha
		End Get
		Set(ByVal value As Date)
			_fecha = value
		End Set
	End Property

	Public Property Moneda() As Integer
		Get
			Return _moneda
		End Get
		Set(ByVal value As Integer)
			_moneda = value
		End Set
	End Property

	Public Property Equivale() As Double
		Get
			Return _equivale
		End Get
		Set(ByVal value As Double)
			_equivale = value
		End Set
	End Property

	'Public Property Id_Emp() As String
	'	Get
	'		Return _id_emp
	'	End Get
	'	Set(ByVal value As String)
	'		_id_emp = value
	'	End Set
	'End Property

	'Public Sub New(ByVal fecha As Date, ByVal moneda As String, ByVal equivale As Double, ByVal id_emp As String)
	'	Me.Fecha = fecha
	'	Me.Moneda = moneda
	'	Me.Equivale = equivale
	'	Me.Id_Emp = id_emp
	'End Sub
	Public Sub New()

	End Sub

End Class
