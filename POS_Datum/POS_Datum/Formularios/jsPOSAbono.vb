Imports MySql.Data.MySqlClient
Public Class jsPOSAbono
    Private Const sModulo As String = "Abono documento"

    Private MyConn As New MySqlConnection
    Private ft As New Transportables

    Private m_MontoAbonado As Double
    Private m_ImprimeTicket As Boolean
    Private m_Procede As Boolean

    Public Property MontoAbonado() As Double
        Get
            Return m_MontoAbonado
        End Get
        Set(ByVal value As Double)
            m_MontoAbonado = value
        End Set
    End Property

    Public Property Procede() As Boolean
        Get
            Return m_Procede
        End Get
        Set(ByVal value As Boolean)
            m_Procede = value
        End Set
    End Property

    Public Sub Cargar(ByVal MyCon As MySqlConnection)
        MyConn = MyCon
        IniciarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()
        txtMontoAbonado.Text = ft.muestraCampoNumero(MontoAbonado)
    End Sub

   

    Private Sub jsPOSAbono_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
   
    End Sub

    Private Sub jsPOSAbono_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnOK.Image = ImageList1.Images(0)
        btnCancel.Image = ImageList1.Images(1)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMontoAbonado.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el código ...", Transportables.tipoMensaje.iInfo)
    End Sub

   
    Private Function Validado() As Boolean
        Validado = False

        If Not IsNumeric(txtMontoAbonado.Text) Then
            ft.mensajeCritico("DEBE INDICAR UN NUMERO VALIDO...")
        End If

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            MontoAbonado = ValorNumero(txtMontoAbonado.Text)
            Procede = True
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Procede = False
        Me.Close()
    End Sub
End Class