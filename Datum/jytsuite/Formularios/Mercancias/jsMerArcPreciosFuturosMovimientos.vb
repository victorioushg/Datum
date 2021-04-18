Imports MySql.Data.MySqlClient
Public Class jsMerArcPreciosFuturosMovimientos

    Private Const sModulo As String = "Movimiento de lista de precios a futuro"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

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
        ft.habilitarObjetos(False, True, txtUnidad, txtDescripcion, txtFecha)
        If i_modo = movimiento.iEditar Then _
            ft.habilitarObjetos(False, True, btnCodigo, cmbTarifa, btnFecha)
    End Sub
    Private Sub IniciarTXT()

        txtCodigo.Text = ""
        txtDescripcion.Text = ""
        txtUnidad.Text = ""
        ft.RellenaCombo(aTarifa, cmbTarifa)
        txtPrecio.Text = 0.0
        txtDescuento.Text = 0.0
        txtFecha.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)

            txtCodigo.Text = .Item("codart")
            txtDescripcion.Text = ft.muestraCampoTexto(.Item("nomart"))
            txtUnidad.Text = ft.muestraCampoTexto(.Item("unidad"))
            ft.RellenaCombo(aTarifa, cmbTarifa, ft.InArray(aTarifa, .Item("tipoprecio")))
            txtPrecio.Text = ft.muestraCampoNumero(.Item("monto"))
            txtDescuento.Text = ft.muestraCampoNumero(.Item("des_art"))
            txtFecha.Text = ft.muestraCampoFecha(.Item("fecha").ToString)

        End With
    End Sub

    Private Sub jsMerArcPreciosFuturosMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing '
    End Sub

    Private Sub jsMerArcPreciosFuturosMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtDescripcion.GotFocus, txtUnidad.GotFocus, txtPrecio.GotFocus, txtFecha.GotFocus, _
        btnCodigo.GotFocus

        Select Case sender.name
            Case "txtCodigo"
                ft.mensajeEtiqueta(lblInfo, "Indique el codigo de la mercancía...", Transportables.TipoMensaje.iInfo)
            Case "cmbTarifa"
                ft.mensajeEtiqueta(lblInfo, "Seleccione la tarifa de precio de mercancía", Transportables.TipoMensaje.iInfo)
            Case "txtPrecio"
                ft.mensajeEtiqueta(lblInfo, "Indique el precio de la mercancía... ", Transportables.TipoMensaje.iInfo)
            Case "txtDescuento"
                ft.mensajeEtiqueta(lblInfo, "Indique el descuento de la mercancía...", Transportables.TipoMensaje.iInfo)
            Case "btnFecha"
                ft.mensajeEtiqueta(lblInfo, "Seleccione la fecha para la cual este precio sera efectivo... ", Transportables.TipoMensaje.iInfo)
            Case "btnCodigo"
                ft.mensajeEtiqueta(lblInfo, "Seleccione la mercancía a cambiar el precio ...", Transportables.TipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If ValorNumero(txtPrecio.Text) <= 0.0 Then
            ft.mensajeAdvertencia("Precio a futuro no puede ser CERO (0.00). Veriqfique por favor ")
            ft.enfocarTexto(txtPrecio)
            Exit Function
        End If

        If ValorNumero(txtDescuento.Text) < 0.0 Then
            ft.mensajeAdvertencia("Descuento de Precio a futuro no puede ser CERO (0.00). Veriqfique por favor ")
            ft.enfocarTexto(txtDescuento)
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
        ft.enfocarTexto(txt)
    End Sub


    Private Sub txtCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPrecio.KeyPress, _
        txtDescuento.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub txtCodigo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigo.TextChanged
        Dim afld() As String = {"codart", "id_emp"}
        Dim aStr() As String = {txtCodigo.Text, jytsistema.WorkID}
        txtDescripcion.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "nomart")
        txtUnidad.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "unidad")
    End Sub

    Private Sub btnCodigo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigo.Click
        Dim f As New jsMerArcListaCostosPreciosNormal
        f.Cargar(MyConn, 0)
        txtCodigo.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub btnFecha_Click(sender As System.Object, e As System.EventArgs) Handles btnFecha.Click
        txtFecha.Text = SeleccionaFecha(CDate(txtFecha.Text), Me, btnFecha)
    End Sub
End Class