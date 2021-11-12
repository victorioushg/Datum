Public Class SimpleTable

	Public Property Codigo As String
	Public Property Descripcion As String
	Public Sub New()

	End Sub
	Public Sub New(ByVal codigo As String, ByVal descripcion As String)
		Me.Codigo = codigo
		Me.Descripcion = descripcion
	End Sub
End Class

