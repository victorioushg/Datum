Imports MySql.Data.MySqlClient
Public Class jsVenArcCXCCestaTicketValor
    Private Const sModulo As String = "Movimiento Valor de Cesta Ticket "

    Private MyConn As New MySqlConnection
    Private ft As New Transportables

    Private n_Apuntador As Long
    Private CodigoCorredor As String
    Private CodigoValorEnBarra As String

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property

    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal Corredor As String, ByVal CodigoEnBarra As String, ByVal PosibleValor As Double)

        MyConn = MyCon

        CodigoCorredor = Corredor
        CodigoValorEnBarra = CodigoEnBarra
        txtCodigo.Text = CodigoEnBarra
        txtValor.Text = ft.FormatoNumero(PosibleValor)

        ft.habilitarObjetos(False, True, txtCodigo)

        Me.ShowDialog()

    End Sub

    Private Sub jsVenArcCXCCestaTicketValor_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsVenArcCXCCestaTicketValor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnOK.Image = ImageList1.Images(0)
    End Sub

    Private Sub txtValor_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtValor.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el Valor impreso en el ticket de alimentación ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Function Validado() As Boolean

        If ValorNumero(txtValor.Text) > 100000 Or ValorNumero(txtValor.Text) < 0 Then
            ft.MensajeCritico("INDIQUE VALOR IMPRESO EN ESTE TICKET... ")
            Return False
        End If

        If ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsvenvaltic where codigo = '" & CodigoCorredor & "' and enbarra = '" & CodigoValorEnBarra & "' ") > 0 Then
            ft.mensajeCritico("CODIGO VALOR EN BARRA YA SE ENCUENTRA EN BASE DE DATOS.")
            Return False
        End If

        Return True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            ft.Ejecutar_strSQL(myconn, "INSERT INTO jsvenvaltic VALUES('" & CodigoCorredor & "', '" & CodigoValorEnBarra & "', " _
                            & ValorNumero(txtValor.Text) & ", '0', '" & jytsistema.WorkID & "')")
            Me.Close()
        End If
    End Sub

    Private Sub txtValor_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtValor.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

End Class