Imports MySql.Data.MySqlClient
Public Class jsVenArcValidacionControlDebitoCredito

    Private Const sModulo As String = "Validación de Números de Control de Facturas En Notas Crédito/Débito"

    Private MyConn As New MySqlConnection
    Private ft As New Transportables


    Private CodigoNotaCredito As String
    Private NumeroDeControlFinal As String
    Private lenFactura As Integer = 0
    Public Property NumeroDeFactura() As String
   

    Public Sub Cargar(ByVal Mycon As MySqlConnection, ByVal iCodigoNotaCredito As String)

        MyConn = Mycon
        lenFactura = Convert.ToInt32(ParametroPlus(MyConn, Gestion.iVentas, "VENNCRPA24"))

        CodigoNotaCredito = iCodigoNotaCredito

        Habilitar()
        IniciarTXT()

        Me.ShowDialog()

    End Sub

    Private Sub Habilitar()
        ft.habilitarObjetos(False, True, txtFactura)
    End Sub
    Private Sub IniciarTXT()
        txtFactura.Text = NumeroDeFactura
    End Sub
    Private Sub jsMerArcLotesMercanciaMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing '
    End Sub

    Private Sub jsMerArcLotesMercanciaMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtFactura.Text)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        Validado = True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            NumeroDeControlFinal = txtNumeroControl.Text + NumeroDeControlFinal
            ft.Ejecutar_strSQL(MyConn, " UPDATE jsconnumcon set num_control = '" & NumeroDeControlFinal & "' " _
                               & " where " _
                               & " numdoc = '" & txtFactura.Text & "' and " _
                               & " origen = 'FAC' and org = 'FAC' and " _
                               & " id_emp = '" & jytsistema.WorkID & "' ")

            Me.Close()
        End If

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFactura.Click
        Dim txt As TextBox = sender
        ft.enfocarTexto(txt)
    End Sub

    Private Sub txtFactura_TextChanged(sender As Object, e As EventArgs) Handles txtFactura.TextChanged

        NumeroDeControlFinal = ft.DevuelveScalarCadena(MyConn, " select num_control " _
                                                        & " from jsconnumcon " _
                                                        & " where " _
                                                        & " numdoc = '" & txtFactura.Text & "' and " _
                                                        & " origen = 'FAC' AND " _
                                                        & " org = 'FAC' AND " _
                                                        & " id_emp = '" & jytsistema.WorkID & "' ")
        If NumeroDeControlFinal.Length > lenFactura Then
            txtNumeroControl.Text = NumeroDeControlFinal.Substring(0, lenFactura)
            NumeroDeControlFinal = NumeroDeControlFinal.Substring(lenFactura, NumeroDeControlFinal.Length - lenFactura)
        Else
            txtNumeroControl.Text = NumeroDeControlFinal
        End If

    End Sub
End Class