Imports MySql.Data.MySqlClient
Public Class jsMerArcPreciosFuturosMovimientos

    Private Const sModulo As String = "Movimiento de lista de precios a futuro"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable

    Private i_modo As Integer

    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private aTarifa() As String = {"A", "B", "C", "D", "E", "F"}

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property

    Public Sub Agregar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable)

        i_modo = movimiento.iAgregar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt

        AsignarTooltips()
        Habilitar()
        IniciarTXT()

        Me.ShowDialog()

    End Sub
    Public Sub Editar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable)

        i_modo = movimiento.iEditar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt

        AsignarTooltips()
        Habilitar()
        AsignarTXT(Apuntador)

        Me.ShowDialog()

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnFecha, "<B>Seleccionar fecha</B> movimiento de precio a futuro ...")
        C1SuperTooltip1.SetToolTip(btnCodigo, "<B>Selecciona mercancia</B> para este movimiento ...")

    End Sub
    Private Sub Habilitar()
        HabilitarObjetos(False, True, txtUnidad, txtDescripcion, txtFecha)
        If i_modo = movimiento.iEditar Then _
            HabilitarObjetos(False, True, btnCodigo, cmbTarifa, btnFecha)
    End Sub
    Private Sub IniciarTXT()

        txtCodigo.Text = ""
        txtDescripcion.Text = ""
        txtUnidad.Text = ""
        RellenaCombo(aTarifa, cmbTarifa)
        txtPrecio.Text = 0.0
        txtDescuento.Text = 0.0
        txtFecha.Text = FormatoFecha(jytsistema.sFechadeTrabajo)

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)

            txtCodigo.Text = .Item("codart")
            txtDescripcion.Text = MuestraCampoTexto(.Item("nomart"), "")
            txtUnidad.Text = MuestraCampoTexto(.Item("unidad"), "")
            RellenaCombo(aTarifa, cmbTarifa, InArray(aTarifa, .Item("tipoprecio")) - 1)
            txtPrecio.Text = FormatoNumero(.Item("monto"))
            txtDescuento.Text = FormatoNumero(.Item("des_art"))
            txtFecha.Text = FormatoFecha(CDate(.Item("fecha").ToString))

        End With
    End Sub

    Private Sub jsMerArcPreciosFuturosMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        '
    End Sub

    Private Sub jsMerArcPreciosFuturosMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtDescripcion.GotFocus, txtUnidad.GotFocus, txtPrecio.GotFocus, txtFecha.GotFocus, _
        btnCodigo.GotFocus

        Select Case sender.name
            Case "txtCodigo"
                MensajeEtiqueta(lblInfo, "Indique el codigo de la mercancía...", TipoMensaje.iInfo)
            Case "cmbTarifa"
                MensajeEtiqueta(lblInfo, "Seleccione la tarifa de precio de mercancía", TipoMensaje.iInfo)
            Case "txtPrecio"
                MensajeEtiqueta(lblInfo, "Indique el precio de la mercancía... ", TipoMensaje.iInfo)
            Case "txtDescuento"
                MensajeEtiqueta(lblInfo, "Indique el descuento de la mercancía...", TipoMensaje.iInfo)
            Case "btnFecha"
                MensajeEtiqueta(lblInfo, "Seleccione la fecha para la cual este precio sera efectivo... ", TipoMensaje.iInfo)
            Case "btnCodigo"
                MensajeEtiqueta(lblInfo, "Seleccione la mercancía a cambiar el precio ...", TipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If ValorNumero(txtPrecio.Text) <= 0.0 Then
            MensajeAdvertencia(lblInfo, "Precio a futuro no puede ser CERO (0.00). Veriqfique por favor ")
            EnfocarTexto(txtPrecio)
            Exit Function
        End If

        If ValorNumero(txtDescuento.Text) < 0.0 Then
            MensajeAdvertencia(lblInfo, "Descuento de Precio a futuro no puede ser CERO (0.00). Veriqfique por favor ")
            EnfocarTexto(txtDescuento)
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

            InsertEditMERCASPrecioFuturo(MyConn, lblInfo, Insertar, CDate(txtFecha.Text), txtCodigo.Text, _
                        cmbTarifa.Text, CDbl(txtPrecio.Text), CDbl(txtDescuento.Text), 0)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.Click, _
        txtFecha.Click
        Dim txt As TextBox = sender
        EnfocarTexto(txt)
    End Sub


    Private Sub txtCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPrecio.KeyPress, _
        txtDescuento.KeyPress
        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub

    Private Sub txtCodigo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigo.TextChanged
        Dim afld() As String = {"codart", "id_emp"}
        Dim aStr() As String = {txtCodigo.Text, jytsistema.WorkID}
        txtDescripcion.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "nomart")
        txtUnidad.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "unidad")
    End Sub

    Private Sub btnCodigo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigo.Click
        Dim f As New jsMerArcListaCostosPreciosPlus
        f.Cargar(MyConn, 0)
        txtCodigo.Text = f.Seleccionado
        f = Nothing
    End Sub
End Class