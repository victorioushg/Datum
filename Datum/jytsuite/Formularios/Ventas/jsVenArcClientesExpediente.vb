Imports MySql.Data.MySqlClient
Public Class jsVenArcClientesExpediente

    Private Const sModulo As String = "Clientes y CxC - Expediente"

    Private MyConn As New MySqlConnection
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private CodCliente As String
    Private elEstatus As String
    Private Item As String
    Private Renglon As String

    Private aCondicion() As String = {"Activo", "Bloqueado", "Inactivo", "Desincorporado"}

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Property registroExpediente As String

    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal CodigoCliente As String, Estatus As Integer)

        i_modo = movimiento.iAgregar
        MyConn = MyCon

        CodCliente = CodigoCliente
        elEstatus = Estatus

        txtFecha.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtCausa.Text = "NOTA DE USUARIO"
        txtEstatus.Text = aCondicion(Estatus)
        txtComentario.Text = ""

        ft.habilitarObjetos(False, True, txtFecha, txtCausa, txtEstatus)


        Me.ShowDialog()
    End Sub

    Private Sub jsVenArcClientesExpediente_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub


    Private Sub jsVenArcClientesExpediente_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Private Function Validado() As Boolean

        Validado = False

        Dim aAdic() As String = {" codcli = '" & CodCliente & "' AND "}
        If FechaUltimoBloqueo(MyConn, "jsvenexpcli", aAdic) >= Convert.ToDateTime(txtFecha.Text) Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If txtComentario.Text = "" Then
            ft.mensajeAdvertencia("Indique un comentario válido")
            Exit Function
        End If

        Validado = True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
            End If
            InsertEditVENTASExpedienteCliente(MyConn, lblInfo, Insertar, CodCliente, CDate(ft.FormatoFecha(txtFecha.Text) & " " & ft.FormatoHora(Now())), _
                                               txtComentario.Text, elEstatus, "", 1)

            registroExpediente = txtFecha.Text & CodCliente

            Me.Close()

        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtComentario_GotFocus(sender As Object, e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique un comentario válido...", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub btnFecha_Click(sender As System.Object, e As System.EventArgs) Handles btnFecha.Click
        txtFecha.Text = ft.FormatoFecha(SeleccionaFecha(CDate(txtFecha.Text), Me, btnFecha))
    End Sub

End Class