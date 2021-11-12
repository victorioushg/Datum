Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsMERArcMercanciasExpediente

    Private Const sModulo As String = "Movimientos EN EXPEDIENTE DE MERCANCIA"

    Private MyConn As New MySqlConnection
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private CodMERCANCIA As String
    Private elEstatus As String
    Private Item As String
    Private Renglon As String

    Private aCondicion() As String = {"Activo", "Inactivo"}

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal CodigoProveedor As String, ByVal Estatus As Integer)

        i_modo = movimiento.iAgregar
        MyConn = MyCon

        CodMERCANCIA = CodigoProveedor
        elEstatus = Estatus

        txtFecha.Value = jytsistema.sFechadeTrabajo
        txtCausa.Text = "NOTA DE USUARIO"
        txtEstatus.Text = aCondicion(Estatus)
        txtComentario.Text = ""

        Me.ShowDialog()
    End Sub

    Private Sub jsMERArcMercanciasExpediente_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub


    Private Sub jsMERArcMERCANCIAExpediente_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dates As SfDateTimeEdit() = {txtFecha}
        SetSizeDateObjects(dates)
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtFecha.Text & txtEstatus.Text)
    End Sub

    Private Function Validado() As Boolean

        If FechaUltimoBloqueo(MyConn, "jsmerexpmer") >= Convert.ToDateTime(txtFecha.Text) Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If txtComentario.Text = "" Then
            ft.mensajeAdvertencia("Indique un comentario válido")
            Return False
        End If

        Return True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
            End If
            InsertEditMERCASExpedienteMercancias(MyConn, lblInfo, Insertar, CodMERCANCIA, CDate(txtFecha.Text & " " & ft.FormatoHora(Now())),
                                               txtComentario.Text, elEstatus, "", 1)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFecha.Text & CodMERCANCIA)
            Me.Close()

        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtComentario_GotFocus(sender As Object, e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique un comentario válido...", Transportables.tipoMensaje.iAyuda)
    End Sub

End Class