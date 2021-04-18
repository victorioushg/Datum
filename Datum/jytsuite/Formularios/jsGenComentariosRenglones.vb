Imports MySql.Data.MySqlClient
Public Class jsGenComentariosRenglones

    Private Const sModulo As String = "Comentario adicional PARA renglones"

    Private MyConn As New MySqlConnection
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private Numerodocumento As String
    Private Origen As String
    Private Item As String
    Private Renglon As String

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal nDocumento As String, ByVal nOrigen As String, _
                ByVal nItem As String, ByVal nRenglon As String)

        i_modo = movimiento.iAgregar
        MyConn = MyCon

        Numerodocumento = nDocumento
        Origen = nOrigen
        Item = nItem
        Renglon = nRenglon

        IniciarTXT()

        Me.ShowDialog()
    End Sub
    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal nDocumento As String, ByVal nOrigen As String, _
                ByVal nItem As String, ByVal nRenglon As String)

        i_modo = movimiento.iEditar
        MyConn = MyCon

        Numerodocumento = nDocumento
        Origen = nOrigen
        Item = nItem
        Renglon = nRenglon

        AsignarTXT()

        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()

        txtDocumento.Text = Numerodocumento
        txtOrigen.Text = Origen
        txtItem.Text = Item
        txtRenglon.Text = Renglon
        txtComentario.Text = ""

    End Sub
    Private Sub AsignarTXT()

        Dim aFld() As String = {"numdoc", "origen", "item", "renglon", "id_emp"}
        Dim aStr() As String = {Numerodocumento, Origen, Item, Renglon, jytsistema.WorkID}

        txtDocumento.Text = Numerodocumento
        txtOrigen.Text = Origen
        txtItem.Text = Item
        txtRenglon.Text = Renglon
        txtComentario.Text = qFoundAndSign(MyConn, lblInfo, "jsvenrencom", aFld, aStr, "comentario")

    End Sub

    Private Sub jsGenComentariosRenglones_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsGenArcComentariosRenglones_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtDocumento.Text & txtRenglon.Text)
        ft.habilitarObjetos(False, True, txtDocumento, txtItem, txtOrigen, txtRenglon)
    End Sub

    Private Function Validado() As Boolean

        Validado = False

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
            InsertEditVENTASComentarioRenglon(MyConn, lblInfo, Insertar, txtDocumento.Text, txtOrigen.Text, txtItem.Text, _
                                      txtRenglon.Text, txtComentario.Text)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtDocumento.Text & txtRenglon.Text)
            Me.Close()

        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtComentario_GotFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique un comentario válido...", Transportables.tipoMensaje.iAyuda)
    End Sub
End Class