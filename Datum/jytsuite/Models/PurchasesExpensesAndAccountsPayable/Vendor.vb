
Public Class VendorBase

    Public Property Codigo() As String
    Public Property Nombre() As String

End Class

Public Class Vendor

    Inherits VendorBase
    Public Property Categoria() As String
    Public Property Unidad() As String
    Public Property Rif() As String
    Public Property Nit() As String
    Public Property Asignado() As String
    Public Property Direccionfiscal() As String
    Public Property DireccionProveedor() As String
    Public Property Email1() As String
    Public Property Email2() As String
    Public Property Email3() As String
    Public Property Email4() As String
    Public Property Email5() As String
    Public Property SitioWeb() As String
    Public Property Telef1() As String
    Public Property Telef2() As String
    Public Property Telef3() As String
    Public Property Fax() As String
    Public Property Gerente() As String
    Public Property TelefonoGerente() As String
    Public Property Contacto() As String
    Public Property TelefonoContacto() As String
    Public Property LimiteCredito() As Double
    Public Property Disponible() As Double
    Public Property Descuento1() As Double
    Public Property Descuento2() As Double
    Public Property Descuento3() As Double
    Public Property Descuento4() As Double
    Public Property Dias2() As Integer
    Public Property Dias3() As Integer
    Public Property Observacion() As String
    Public Property Zona() As String
    Public Property Cobrador() As String
    Public Property Vendedor() As String
    Public Property Saldo() As Double
    Public Property UltimoPago() As Double
    Public Property FechaUltimoPago() As Date
    Public Property FormaUltimoPago() As String
    Public Property RegimenIVA() As String
    Public Property FormaDePago() As String
    Public Property Banco() As String
    Public Property CuentaBanco() As String
    Public Property CodigoBancoDeposito1() As String
    Public Property CodigoBancoDeposito2() As String
    Public Property CuentaBancoDeposito1() As String
    Public Property CuentaBancoDeposito2() As String
    Public Property FechaIngreso() As Date
    Public Property CodigoContable() As String
    Public Property Estatus() As String
    Public Property TipoProveedor() As Integer

End Class

