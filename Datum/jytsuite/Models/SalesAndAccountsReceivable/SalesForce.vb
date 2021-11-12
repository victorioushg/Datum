Public Class SalesForce
    Public Property Codigo() As String
    Public Property DescripcionCargo() As String
    Public Property Apellidos() As String
    Public Property Nombres() As String
    Public Property Direccion() As String
    Public Property Telefono() As String
    Public Property Celular() As String
    Public Property Email() As String
    Public Property Fianza As Decimal
    Public Property Tipo As String
    Public Property Clase As Integer   ' VENDEDOR Ó SUPERVISOR Ó GERENTE',
    Public Property Zona As String
    Public Property Estructura As String
    Public Property Clave As String
    Public Property Ingreso As Date
    Public Property CarteraClientes As Integer
    Public Property CarteraMercancias As Integer
    Public Property CarteraMarcas As Integer
    Public Property Lista_A As Integer
    Public Property Lista_B As Integer
    Public Property Lista_C As Integer
    Public Property Lista_D As Integer
    Public Property Lista_E As Integer
    Public Property Lista_F As Integer
    Public Property FactorCuota As Double
    Public Property Estatus As Integer
    Public Property Division As String
    Public Property Supervisor As String
    Public Property Comision_Ventas As Double
    Public Property Comision_Cobranza As Double

    Public ReadOnly Property NombreAsesor As String
        Get
            Return Apellidos & ", " & Nombres
        End Get
    End Property

End Class
