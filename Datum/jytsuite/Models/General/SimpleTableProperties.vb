Public Class SimpleTableProperties

	Private _nombreTabla As String
	Private _documentKey As String
	Private _documentValue As String

	Public Property NombreTabla() As String
		Get
			Return _nombreTabla
		End Get
		Set(ByVal value As String)
			_nombreTabla = value
		End Set
	End Property

	Public Property DocumentKey() As String
		Get
			Return _documentKey
		End Get
		Set(ByVal value As String)
			_documentKey = value
		End Set
	End Property

	Public Property DocumentValue() As String
		Get
			Return _documentValue
		End Get
		Set(ByVal value As String)
			_documentValue = value
		End Set
	End Property
	Public Sub New(ByVal tabla As String, ByVal key As String, ByVal value As String)
		Me.NombreTabla = tabla
		Me.DocumentKey = key
		Me.DocumentValue = value
	End Sub

End Class
