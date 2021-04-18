Imports MySql.Data.MySqlClient
Public Class jsGenArcDescuentosCompras

    Private Const sModulo As String = "Descuentos Compras"
    Private Const nTabla As String = "tbldes"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private nomModulo As String = ""
    Private codidoProveedor As String = ""
    Private tipoProveedor As Integer = 0
    Private FechaDescuento As Date = jytsistema.sFechadeTrabajo
    Private MontoNetoDocumento As Double = 0.0
    Private NombreTablaEnBD As String = ""
    Private NombreCampoEnBD As String = ""
    Private numDocumento As String = ""
    Private numRenglon As String
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                       ByVal NombreTablaEnBaseDatos As String, ByVal NombreCampoClaveEnBD As String, ByVal NumeroDocumento As String, _
                       ByVal NombreModulo As String, ByVal CodProveedor As String, ByVal TipProveedor As Integer, _
                       ByVal FechDescuento As Date, ByVal NetoDocumento As Double)

        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        nomModulo = NombreModulo
        codidoProveedor = CodProveedor
        tipoProveedor = TipProveedor
        FechaDescuento = FechDescuento
        MontoNetoDocumento = NetoDocumento
        NombreTablaEnBD = NombreTablaEnBaseDatos
        NombreCampoEnBD = NombreCampoClaveEnBD
        numDocumento = NumeroDocumento

        Me.Text += " " & NombreModulo

        IniciarTXT()

        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()
        numRenglon = ft.autoCodigo(MyConn, "renglon", NombreTablaEnBD, NombreCampoEnBD + ".id_emp", numDocumento + "." + jytsistema.WorkID, 5)
        txtCodigo.Text = numRenglon
        txtNombre.Text = "DESCUENTO"
        txtPorcentaje.Text = ft.FormatoPorcentajeLargo(0.0)
        ft.habilitarObjetos(False, True, txtMontoDescuento, txtDescuento, txtCodigo)

    End Sub
    Private Sub jsGenArcDescuentosCompras_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
        ft = Nothing
    End Sub

    Private Sub jsGenArcDescuentosCompras_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Function Validado() As Boolean

        Validado = False


        If Trim(txtNombre.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre válido...")
            txtNombre.Focus()
            Exit Function
        End If

        If ValorNumero(txtPorcentaje.Text) <= 0 Or ValorNumero(txtPorcentaje.Text) > 100 Then
            ft.mensajeAdvertencia("Debe indicar un valor de porcentaje válido...")
            ft.enfocarTexto(txtPorcentaje)
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
            InsertEditCOMPRASDescuento(MyConn, lblInfo, Insertar, NombreTablaEnBD, NombreCampoEnBD, numDocumento, _
                                      numRenglon, txtNombre.Text, ValorNumero(txtPorcentaje.Text), ValorNumero(txtDescuento.Text), codidoProveedor)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()

        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtPorcentaje_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPorcentaje.Click, _
        txtPorcentaje.GotFocus
        Dim txt As TextBox = sender
        ft.enfocarTexto(txt)
        ft.mensajeEtiqueta(lblInfo, "Indique un porcentaje válido para este descuento...", Transportables.tipoMensaje.iAyuda)
    End Sub
    Private Sub txtPorcentaje_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPorcentaje.TextChanged
        txtMontoDescuento.Text = ft.FormatoNumero((1 - ValorNumero(txtPorcentaje.Text) / 100) * MontoNetoDocumento)
        txtDescuento.Text = ft.FormatoNumero(MontoNetoDocumento * ValorNumero(txtPorcentaje.Text) / 100)
    End Sub

    Private Sub txtMontoDescuento_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMontoDescuento.GotFocus, _
        txtMontoDescuento.Click
        ft.enfocarTexto(txtMontoDescuento)
        ft.mensajeEtiqueta(lblInfo, "Indique un monto válido de descuento ...", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub txtPorcentaje_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPorcentaje.KeyPress, _
        txtMontoDescuento.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    
End Class