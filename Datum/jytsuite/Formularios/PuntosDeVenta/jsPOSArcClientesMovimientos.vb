Imports MySql.Data.MySqlClient
Public Class jsPOSArcClientesMovimientos

    Private Const sModulo As String = "Movimiento clientes puntos de venta"
    Private Const nTabla As String = "clientespos"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer

    Private nPosicion As Integer
    Private n_Apuntador As Long

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property


    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)
            txtRif.Text = .Item("rif")
            txtNombre.Text = .Item("nombre")
            txtDir.Text = .Item("dirfiscal")
            txtTelefono.Text = .Item("telef1")
        End With
    End Sub
    Private Sub jsPOSArcCajerosMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
        ft = Nothing
    End Sub

    Private Sub jsPOSArcCajerosMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ft.habilitarObjetos(False, True, txtRif)
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtRif.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRif.GotFocus, _
        txtNombre.GotFocus
        Select Case sender.name
            Case "txtRif"
                ft.mensajeEtiqueta(lblInfo, "Indique el código de cliente ...", Transportables.TipoMensaje.iInfo)
            Case "txtNombre"
                ft.mensajeEtiqueta(lblInfo, "Indique el nombre del cliente  ...", Transportables.TipoMensaje.iInfo)
            Case "txtDir"
                ft.mensajeEtiqueta(lblInfo, "Indique la dirección fiscal del cliente... ", Transportables.TipoMensaje.iInfo)
            Case "txtTelefono"
                ft.mensajeEtiqueta(lblInfo, "Indique el teléfono del cliente... ", Transportables.TipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtNombre.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre válido...")
            ft.enfocarTexto(txtNombre)
            Exit Function
        End If

        Validado = True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dtLocal.Rows.Count
            End If
            InsertarModificarPOSClientePV(MyConn, Insertar, "00000000", txtNombre.Text, _
                                       "", "", txtRif.Text, "", "", "", txtDir.Text, _
                                       txtTelefono.Text, "", jytsistema.sFechadeTrabajo, 0)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtRif.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

End Class