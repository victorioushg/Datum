Public Class CambioMoneda

    Public Property Fecha() As DateTime
    Public Property Moneda() As Integer
    Public Property Equivale() As Double

    Public Sub New()
    End Sub
    Public Sub New(ByVal fecha As DateTime, ByVal moneda As Integer, equivale As Double)

        Me.Fecha = fecha
        Me.Moneda = moneda
        Me.Equivale = equivale

    End Sub

End Class


Public Class CambioMonedaPlus
    Inherits CambioMoneda

    Public Property UnidadMonetaria() As String
    Public Property Simbolo() As String
    Public Property CodigoIso As String

End Class

'' 
'Public Class Foo ' consider this as your clsTest
'    Public Sub New()
'    End Sub
'    Public Property Name As String
'    Public Property Number As Int32
'    Public Sub New(name As String)
'        Me.Name = name
'    End Sub
'End Class

'Public Class Bar
'    Inherits Foo

'    Public Sub New(name As String, i As Integer)
'        MyBase.Name = name
'        MyBase.Number = i
'    End Sub
'End Class