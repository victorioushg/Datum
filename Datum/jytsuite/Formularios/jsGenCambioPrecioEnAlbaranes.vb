Imports MySql.Data.MySqlClient
Public Class jsGenCambioPrecioEnAlbaranes

    Private Const sModulo As String = "Cambio de precio en Documento"
    Private Const nTabla As String = "tbldes"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private CodigoCliente As String = ""
    Private NumeroRenglon As String = ""
    Private NumeroDocumento As String = ""
    Private CodigoArticulo As String = ""
    Private UnidadArticulo As String = ""
    Private CantidadArticulo As Double = 0.0
    Private PrecioArticulo As Double = 0.0

    Private nPosicion As Integer
    Private n_NuevoPrecio As Double

    Public Property NuevoPrecio() As Double
        Get
            Return n_NuevoPrecio
        End Get
        Set(ByVal value As Double)
            n_NuevoPrecio = value
        End Set
    End Property
    Public Sub Cambiar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                       ByVal numDocumento As String, ByVal CodCliente As String, ByVal iRenglon As String, _
                       ByVal Item As String, ByVal Unidad As String, ByVal Cantidad As Double, ByVal PrecioAnterior As Double)

        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt

        CodigoCliente = CodCliente
        NumeroDocumento = numDocumento
        NumeroRenglon = iRenglon
        CodigoArticulo = Item
        UnidadArticulo = Unidad
        CantidadArticulo = Cantidad
        PrecioArticulo = PrecioAnterior

        Me.Text = sModulo
        IniciarTXT()

        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()

        txtCliente.Text = CodigoCliente
        txtNombreCliente.Text = ft.DevuelveScalarCadena(MyConn, " select nombre from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
        If txtNombreCliente.Text = "0" Then txtNombreCliente.Text = "CLIENTE PUNTO DE VENTA"
        txtCodigo.Text = CodigoArticulo
        txtDescripcion.Text = ft.DevuelveScalarCadena(MyConn, " select nomart from jsmerctainv where codart = '" & CodigoArticulo & "' and id_emp = '" & jytsistema.WorkID & "' ")
        txtCantidad.Text = ft.FormatoCantidad(CantidadArticulo)
        txtUnidad.Text = UnidadArticulo
        txtPrecio.Text = ft.FormatoNumero(PrecioArticulo)

        Dim lEquivalencia As Double = Equivalencia(myConn,  CodigoArticulo, UnidadArticulo)

        Dim UltimoCosto As Double = UltimoCostoAFecha(MyConn, CodigoArticulo, jytsistema.sFechadeTrabajo)
        Dim PrecioPromedio As Double = 0.0

        lblUltimoCosto.Text = ft.FormatoNumero(UltimoCosto / IIf(lEquivalencia = 0, 1, lEquivalencia))

        txtNuevoPrecio.Text = txtPrecio.Text
        ft.habilitarObjetos(False, True, txtCliente, txtNombreCliente, txtCodigo, txtDescripcion, txtCantidad, txtUnidad, txtPrecio)
        ft.enfocarTexto(txtNuevoPrecio)

    End Sub
    Private Sub jsGenArcDescuentosVentas_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsGenArcDescuentosVentas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, CodigoCliente)
    End Sub

    Private Function Validado() As Boolean

        Dim lNuevoPrecio As Double = ValorNumero(txtNuevoPrecio.Text)
        If lNuevoPrecio <= 0 Then
            ft.MensajeCritico("DEBE INDICAR UN PRECIO VALIDO. VERIFIQUE POR FAVOR...")
            Return False
        Else
            If lNuevoPrecio < ValorNumero(lblUltimoCosto.Text) Then
                ft.MensajeCritico("NUEVO PRECIO MENOR QUE ULTIMO COSTO. VERIFIQUE POR FAVOR...")
                Return False
            End If
        End If

        If NivelUsuario(MyConn, lblInfo, UsuarioClaveAESPlus(MyConn, lblInfo, txtPassword.Text)) = 0 Then
            ft.MensajeCritico("NO POSEE NIVEL SUFICIENTE O CONTRASEÑA NO ES VALIDA...")
            Return False
        End If

        Return True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            NuevoPrecio = ValorNumero(txtNuevoPrecio.Text)
            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, NumeroDocumento & " " & CodigoArticulo & " " & NuevoPrecio.ToString)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, NumeroDocumento & " " & CodigoArticulo)
        Me.Close()
    End Sub

    Private Sub txtMontoDescuento_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNuevoPrecio.GotFocus, _
        txtNuevoPrecio.Click
        ft.enfocarTexto(txtNuevoPrecio)
        ft.mensajeEtiqueta(lblInfo, "Indique un monto nuevo precio ...", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub txtPorcentaje_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNuevoPrecio.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

End Class