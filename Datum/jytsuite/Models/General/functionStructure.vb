Public Class functionStructure

    Public Property FunctionName As String
    Public Property FunctionParameters As String
    Public Property FunctionReturnType As String
    Public Property FunctionFormula As String
    Public Property FunctionCommentary As String

    Public Sub New()

    End Sub
    Public Sub New(ByVal name As String, ByVal parameters As String, ByVal returnType As String,
                   ByVal formula As String, ByVal commentary As String)
        FunctionName = name
        FunctionParameters = parameters
        FunctionReturnType = returnType
        FunctionFormula = formula
        FunctionCommentary = commentary
    End Sub

End Class
