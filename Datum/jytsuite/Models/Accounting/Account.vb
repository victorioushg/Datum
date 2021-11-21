
Public Class AccountBase

    Public Property CodigoContable() As String
    Public Property Descripcion() As String
    Public Property Nivel() As Integer
    Public Property Marca() As Integer

    Public ReadOnly Property DisplayName As String
        Get
            Return CodigoContable & " | " & Descripcion
        End Get
    End Property

End Class

Public Class Account
    Inherits AccountBase

    Public Property Deb00 As Double
    Public Property Deb01 As Double
    Public Property Deb02 As Double
    Public Property Deb03 As Double
    Public Property Deb04 As Double
    Public Property Deb05 As Double
    Public Property Deb06 As Double
    Public Property Deb07 As Double
    Public Property Deb08 As Double
    Public Property Deb09 As Double
    Public Property Deb10 As Double
    Public Property Deb11 As Double
    Public Property Deb12 As Double
    Public Property Cre00 As Double
    Public Property Cre01 As Double
    Public Property Cre02 As Double
    Public Property Cre03 As Double
    Public Property Cre04 As Double
    Public Property Cre05 As Double
    Public Property Cre06 As Double
    Public Property Cre07 As Double
    Public Property Cre08 As Double
    Public Property Cre09 As Double
    Public Property Cre10 As Double
    Public Property Cre11 As Double
    Public Property Cre12 As Double

End Class

