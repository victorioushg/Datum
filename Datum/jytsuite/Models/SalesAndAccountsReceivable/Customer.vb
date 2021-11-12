
Public Class CustomerBase

    Public Property Codcli() As String
    Public Property Nombre() As String
    Public Property Categoria() As String
    Public Property Unidad() As String
    Public Property Rif() As String
    Public Property Nit() As String
    Public Property Ci() As String
    Public Property Alterno() As String

End Class

Public Class Customer
    Inherits CustomerBase

    Public Property Dirfiscal() As String
    Public Property Fpais() As Integer
    Public Property Festado() As Integer
    Public Property Fmunicipio() As Integer
    Public Property Fparroquia() As Integer
    Public Property FCiudad() As Integer
    Public Property FBarrio() As Integer
    Public Property Fzip() As String
    Public Property Dpais() As Integer
    Public Property Destado() As Integer
    Public Property Dmunicipio() As Integer
    Public Property Dparroquia() As Integer
    Public Property Dciudad() As Integer
    Public Property Dbarrio() As Integer
    Public Property Dzip() As String
    Public Property Codgeo() As Integer
    Public Property Dirdespa() As String
    Public Property EMAIL1() As String
    Public Property EMAIL2() As String
    Public Property EMAIL3() As String
    Public Property EMAIL4() As String
    Public Property Telef1() As String
    Public Property TELEF2() As String
    Public Property TELEF3() As String
    Public Property FAX() As String
    Public Property GERENTE() As String
    Public Property TELGER() As String
    Public Property CONTACTO() As String
    Public Property TELCON() As String
    Public Property LimiteCredito() As Double
    Public Property Disponible() As Double
    Public Property CHEQDEV() As Integer
    Public Property CHEQMES() As Integer
    Public Property CHEQACU() As Integer
    Public Property DES_CLI() As Double
    Public Property DESC_CLI_1() As Double
    Public Property DESC_CLI_2() As Double
    Public Property DESC_CLI_3() As Double
    Public Property DESC_CLI_4() As Double
    Public Property DESDE_1() As Integer
    Public Property HASTA_1() As Integer
    Public Property DESDE_2() As Integer
    Public Property HASTA_2() As Integer
    Public Property DESDE_3() As Integer
    Public Property HASTA_3() As Integer
    Public Property DESDE_4() As Integer
    Public Property HASTA_4() As Integer
    Public Property POR_CAR_DEV() As Double
    Public Property ZONA() As String
    Public Property RUTA_VISITA() As String
    Public Property RUTA_DESPACHO() As String
    Public Property NUM_DESPACHO() As Integer
    Public Property NUM_VISITA() As Integer
    Public Property COBRADOR() As String
    Public Property VENDEDOR() As String
    Public Property Transporte() As String
    Public Property SALDO() As Double
    Public Property ULTCOBRO() As Double
    Public Property FECULTCOBRO() As Date
    Public Property FORULTCOBRO() As String
    Public Property REGIMENIVA() As String
    Public Property Tarifa() As String
    Public Property LISPRE() As String
    Public Property FormaPago() As String
    Public Property BANCO() As String
    Public Property CTABANCO() As String
    Public Property INGRESO() As Date
    Public Property CODCON() As String
    Public Property CODCRE() As Integer
    Public Property Estatus() As String
    Public Property REQ_RIF() As Integer
    Public Property REQ_NIT() As Integer
    Public Property REQ_REC() As Integer
    Public Property REQ_CIS() As Integer
    Public Property REQ_REG() As Integer
    Public Property REQ_REA() As Integer
    Public Property REQ_BAN() As Integer
    Public Property REQ_COM() As Integer
    Public Property FECVISITA() As Integer
    Public Property INIVISITA() As Date
    Public Property FECULTVISITA() As Date
    Public Property MERCHANDISING() As Integer
    Public Property BACKORDER() As Integer
    Public Property DIAPAGO() As Integer
    Public Property DEPAGO() As String
    Public Property APAGO() As String
    Public Property RANKING() As Integer
    Public Property FECHARANK() As Date
    Public Property COMENTARIO() As String
    Public Property ESPECIAL() As Integer
    Public Property SHARE() As Integer
    Public Property EJERCICIO() As String
    Public Property ID_EMP() As String

    '' 
    Public Property Asesor() As String
    Public Property CodigoEstatus() As Integer


End Class

