Imports MySql.Data.MySqlClient
Public Class jsVenArcClientesSADA
    Private Const sModulo As String = "Consulta códigos SADA"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private CodigoCliente As String
    Private Expediente As String

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal iCodCliente As String)

        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoCliente = iCodCliente

        IniciarSADA()
        Me.ShowDialog()

    End Sub

    Private Sub IniciarSADA()


        Dim aCam() As String = {"codigosada", "razon", "tipo", "Estado", "Municipio", "ciudad", "Estatus"}
        Dim aNom() As String = {"Código SADA", "Nombre y/o Razón Social", "Tipo Ente", "Estado", "Municipio", "Ciudad", "Estatus"}
        Dim aAnc() As Integer = {90, 300, 90, 90, 90, 90, 90}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFor() As String = {"", "", "", "", "", "", ""}


        IniciarTabla(dgSADA, dtLocal, aCam, aNom, aAnc, aAli, aFor)


    End Sub

    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)
        End With
    End Sub

    Private Sub jsVenArcClientesCIs_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsVenArcClientesCIss_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnOK.Image = ImageList1.Images(0)
        btnCancel.Image = ImageList1.Images(1)
    End Sub

    
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Close()
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class