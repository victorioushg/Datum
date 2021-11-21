Public Class DataGridField
    Public Property Campo() As String
    Public Property Nombre() As String
    Public Property Ancho() As Integer
    Public Property Alineacion() As DataGridViewContentAlignment
    Public Property Formato As String
    Public Sub New()
    End Sub
    Public Sub New(ByVal campo As String, ByVal nombre As String, ancho As Integer,
                   alineacion As DataGridViewContentAlignment, formato As String)
        Me.Campo = ancho
        Me.Nombre = nombre
        Me.Ancho = ancho
        Me.Alineacion = alineacion
        Me.Formato = formato
    End Sub
End Class
