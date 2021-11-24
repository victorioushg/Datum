Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsBanArcCajasMovimientos
    Private Const sModulo As String = "Movimiento de caja"
    Private Const nTabla As String = "tarjetas"
    Private strSQL As String = "select * from jsconctatar where id_emp = '" & jytsistema.WorkID & "' order by codtar"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dtTar As New DataTable
    Private ft As New Transportables
    Private transaction As SavingLines

    Private i_modo As Integer

    Private CodigoCaja As String
    Private Renglon As String
    Public Property Apuntador() As Long

    Public Sub Agregar(ByVal MyCon As MySqlConnection, CodCaja As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        CodigoCaja = CodCaja
        IniciarControles()
        IniciarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()
        txtFecha.Value = jytsistema.sFechadeTrabajo
        txtDocumento.Text = ""
        txtDocPago.Text = ""
        txtRefPago.Text = ""
        txtImporte.Text = ft.FormatoNumero(0.0)
    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, transact As SavingLines, ByVal codCaja As String)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        transaction = transact
        CodigoCaja = codCaja
        IniciarControles()
        AsignarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarControles()
        InitiateDropDown(Of TextoValor)(MyConn, cmbFormaPago, Tipo.FormaDePago, 0)
        InitiateDropDown(Of TextoValor)(MyConn, cmbTipo, Tipo.TipoMovimientoCaja, 0)

    End Sub
    Private Sub AsignarTXT()
        txtFecha.Value = transaction.Fecha
        cmbTipo.SelectedValue = transaction.TipoMovimiento
        cmbFormaPago.SelectedValue = transaction.FormaDePago
        txtDocumento.Text = transaction.NumeroMovimiento
        txtDocPago.Text = transaction.NumeroDePago
        txtRefPago.Text = transaction.ReferenciaDePago
        txtImporte.Text = ft.FormatoNumero(Math.Abs(transaction.Importe))
        Renglon = transaction.Id.ToString
    End Sub

    Private Sub jsBanCajasMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dtTar = Nothing
        ft = Nothing
    End Sub

    Private Sub jsBanCajasMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo

        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, CodigoCaja)
        ds = DataSetRequery(ds, strSQL, MyConn, nTabla, lblInfo)
        dtTar = ds.Tables(nTabla)

        Dim dates As SfDateTimeEdit() = {txtFecha}
        SetSizeDateObjects(dates)

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, CodigoCaja)
        Me.Close()
    End Sub
    Private Function Validado() As Boolean

        If Trim(txtDocumento.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un número de documento válido ...")
            ft.enfocarTexto(txtDocumento)
            Return False
        Else
            Dim aCampos() As String = {"tipomov", "nummov", "id_emp"}
            Dim aValores() As String = {cmbTipo.SelectedValue, txtDocumento.Text, jytsistema.WorkID}

            If qFound(MyConn, lblInfo, "jsbantracaj", aCampos, aValores) AndAlso i_modo = movimiento.iAgregar Then
                ft.mensajeAdvertencia("Esté documento ya fué incluido, verifique ...")
                ft.enfocarTexto(txtDocumento)
                Return False
            End If
        End If

        If cmbFormaPago.SelectedIndex >= 1 AndAlso txtDocPago.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar un número de pago válido ...")
            ft.enfocarTexto(txtDocPago)
            Return False
        End If

        If cmbFormaPago.SelectedIndex >= 1 AndAlso txtRefPago.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar una referencia de pago válida ...")
            ft.enfocarTexto(txtRefPago)
            Return False
        End If

        If cmbTipo.SelectedIndex = 1 AndAlso cmbFormaPago.SelectedIndex >= 1 Then
            ft.mensajeAdvertencia("Una salida de caja sólo puede ser en efectivo ...")
            cmbFormaPago.Focus()
            Return False
        End If

        If Not ft.isNumeric(txtImporte.Text) Then
            ft.mensajeAdvertencia("El importe  debe ser numérico ...")
            ft.enfocarTexto(txtImporte)
            Return False
        End If

        Dim aAdicionales() As String = {" caja = '" & CodigoCaja & "' AND "}
        If FechaUltimoBloqueo(MyConn, "jsbantracaj", aAdicionales) >= txtFecha.Value Then
            ft.mensajeEtiqueta(lblInfo, "FECHA MENOR QUE ULTIMA FECHA DE CIERRE...", Transportables.tipoMensaje.iAdvertencia)
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
            InsertEditBANCOSRenglonCaja(MyConn, lblInfo, Insertar, CodigoCaja, Renglon, txtFecha.Value, "CAJ",
                 cmbTipo.SelectedValue, txtDocumento.Text, cmbFormaPago.SelectedValue,
                 txtDocPago.Text, txtRefPago.Text, IIf(cmbTipo.SelectedValue = "EN", 1, -1) * ValorNumero(txtImporte.Text), "", "", "", jytsistema.MyDate, 1,
                 "", "", "", MyDate, "", "", "0", jytsistema.WorkCurrency.Id, DateTime.Now())

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, CodigoCaja & " " & txtDocumento.Text)
            Me.Close()
        End If
    End Sub

    Private Sub txtDocumento_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDocumento.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el número de documento para este movimiento ...", Transportables.TipoMensaje.iInfo)
    End Sub
    Private Sub txtDocPago_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDocPago.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el número de documento de pago para este movimiento ...", Transportables.tipoMensaje.iInfo)
    End Sub
    Private Sub txtRefPago_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRefPago.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique la referencia de pago ...", Transportables.tipoMensaje.iInfo)
    End Sub
    Private Sub txtImporte_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtImporte.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el monto ó importe para este movimiento ...", Transportables.tipoMensaje.iInfo)
    End Sub
    Private Sub cmbTipo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        ft.mensajeEtiqueta(lblInfo, "Seleccione el tipo de movimiento ...", Transportables.tipoMensaje.iInfo)
    End Sub
    Private Sub cmbFormaPago_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        ft.mensajeEtiqueta(lblInfo, "Seleccione la forma de pago para este movimiento ...", Transportables.tipoMensaje.iInfo)
    End Sub
    Private Sub cmbFormaPago_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If cmbTipo.SelectedIndex = 0 AndAlso cmbFormaPago.SelectedIndex = 2 Then
            ft.visualizarObjetos(True, btnTarjeta)
            ft.habilitarObjetos(False, True, txtRefPago)

        Else
            txtRefPago.Enabled = True
            btnTarjeta.Visible = False
            ft.habilitarObjetos(True, True, txtRefPago)
            ft.visualizarObjetos(False, btnTarjeta)
        End If
    End Sub
    Private Sub btnTarjeta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTarjeta.Click
        Dim f As New jsControlArcTarjetas
        f.Cargar(MyConn, TipoCargaFormulario.iShowDialog)
        If dtTar.Rows.Count > 0 Then
            txtRefPago.Text = f.Seleccionado
        Else
            txtRefPago.Text = ""
        End If
        f = Nothing
    End Sub
    Private Sub txtImporte_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtImporte.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub
End Class