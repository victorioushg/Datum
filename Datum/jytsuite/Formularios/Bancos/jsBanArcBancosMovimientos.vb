Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Imports fTransport
Public Class jsBanArcBancosMovimientos
    Private Const sModulo As String = "Movimiento bancario"

    Private MyConn As New MySql.Data.MySqlClient.MySqlConnection
    Private ds As DataSet
    Private dt As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private Codigobanco As String
    Private aTipo() As String = {"Deposito", "Nota Crédito", "Cheque", "Nota Débito"}
    Private aTipoDeposito() As String = {"Normal", "Diferido"}
    Private aaTipo() As String = {"DP", "NC", "CH", "ND"}
    Private n_Apuntador As Long
    Private numComprobante As String
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySql.Data.MySqlClient.MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal Codban As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        ds = dsMov
        dt = dtMov
        Codigobanco = Codban
        If dt.Rows.Count = 0 Then Apuntador = -1
        IniciarTXT()
        ft.habilitarObjetos(False, True, cmbTipoDeposito)


        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()
        txtFecha.Value = jytsistema.sFechadeTrabajo
        txtDocumento.Text = ""
        txtConcepto.Text = ""
        txtImporte.Text = ft.FormatoNumero(0.0)
        txtBeneficiario.Text = ""
        ft.RellenaCombo(aTipo, cmbTipo)
        ft.RellenaCombo(aTipoDeposito, cmbTipoDeposito)

    End Sub

    Public Sub Editar(ByVal MyCon As MySql.Data.MySqlClient.MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal Codban As String)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        ds = dsMov
        dt = dtMov
        Codigobanco = Codban
        AsignarTXT(Apuntador)
        ft.habilitarObjetos(False, True, cmbTipo, cmbTipoDeposito)

        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dt.Rows(nPosicion)
            txtFecha.Value = .Item("fechamov")

            ft.RellenaCombo(aTipo, cmbTipo, ft.InArray(aaTipo, .Item("tipomov")))
            ft.RellenaCombo(aTipoDeposito, cmbTipoDeposito, .Item("multican"))

            txtDocumento.Text = ft.muestraCampoTexto(.Item("numdoc"))
            txtConcepto.Text = ft.muestraCampoTexto(.Item("concepto"))
            txtImporte.Text = ft.muestraCampoNumero(Math.Abs(.Item("importe")))
            txtBeneficiario.Text = ft.muestraCampoTexto(.Item("benefic"))
            numComprobante = ft.muestraCampoTexto(.Item("comproba"))

        End With
    End Sub

    Private Sub jsBanArcBancosMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFecha.Value)
        ft = Nothing
    End Sub

    Private Sub jsBanArcBancosMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dates As SfDateTimeEdit() = {txtFecha}
        SetSizeDateObjects(dates)

        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtFecha.Value)
        Me.Tag = sModulo
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub
    Private Function Validado() As Boolean
        Validado = False
        If Trim(txtDocumento.Text) = "" Then
            ft.mensajeEtiqueta(lblInfo, "Debe indicar un número de documento válido ...", Transportables.TipoMensaje.iAdvertencia)
            ft.enfocarTexto(txtDocumento)
            Exit Function
        End If

        Dim aCampos() As String = {"tipomov", "numdoc", "id_emp"}
        Dim aValores() As String = {aaTipo(cmbTipo.SelectedIndex), txtDocumento.Text, jytsistema.WorkID}
        If qFound(MyConn, lblInfo, "jsbantraban", aCampos, aValores) AndAlso i_modo = movimiento.iAgregar Then
            ft.mensajeEtiqueta(lblInfo, "Esté documento ya fué incluido, verifique ...", Transportables.TipoMensaje.iAdvertencia)
            ft.enfocarTexto(txtDocumento)
            Exit Function
        End If

        If Trim(txtConcepto.Text) = "" Then
            ft.mensajeEtiqueta(lblInfo, "Debe indicar un concepto ó comentario válido ...", Transportables.TipoMensaje.iAdvertencia)
            ft.enfocarTexto(txtConcepto)
            Exit Function
        End If

        If cmbTipo.SelectedIndex = 2 AndAlso Trim(txtBeneficiario.Text) = "" Then
            ft.mensajeEtiqueta(lblInfo, "Debe indicar un nombre de beneficiario ...", Transportables.TipoMensaje.iAdvertencia)
            ft.enfocarTexto(txtBeneficiario)
            Exit Function
        End If

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim Insertar As Boolean = False
        If Validado() Then
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = 0
                numComprobante = IIf(cmbTipo.SelectedIndex = 2, Contador(MyConn, lblInfo, Gestion.iBancos, "BANNUMRCC", "03"), "")
            End If
            InsertEditBANCOSMovimientoBanco(MyConn, lblInfo, Insertar, txtFecha.Value, txtDocumento.Text, aaTipo(cmbTipo.SelectedIndex),
                Codigobanco, "", txtConcepto.Text, IIf(cmbTipo.SelectedIndex < 2, ValorNumero(txtImporte.Text), -1 * ValorNumero(txtImporte.Text)),
                "BAN", txtDocumento.Text, IIf(Trim(txtBeneficiario.Text) = "", ".", txtBeneficiario.Text),
                numComprobante, "0", MyDate, MyDate, cmbTipo.SelectedIndex, "", MyDate,
                cmbTipoDeposito.SelectedIndex, "", "", jytsistema.WorkCurrency.Id, DateTime.Now())

            ImprimirComprobante()

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, Codigobanco & " " & txtDocumento.Text)
            Me.Close()
        End If

    End Sub
    Private Sub ImprimirComprobante()
        If cmbTipo.SelectedIndex = 2 Then
            Dim resp As Integer
            resp = MsgBox(" ¿Desea imprimir comprobante de egreso? ", MsgBoxStyle.YesNo, sModulo)
            If resp = MsgBoxResult.Yes Then
                Dim f As New jsBanRepParametros
                f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cComprobanteDeEgreso, "Comprobante de pago", Codigobanco, numComprobante)
                f = Nothing
            End If
        End If
    End Sub


    Private Sub cmbTipo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTipo.GotFocus
        ft.mensajeEtiqueta(lblInfo, "seleccione el tipo de movimiento a realizar", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtDocumento_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDocumento.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el número de documento para este movimiento ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtConcepto_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConcepto.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el concepto por el cual se realiza este movimiento ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtImporte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtImporte.Click, _
        txtDocumento.Click, txtBeneficiario.Click, txtConcepto.Click
        Dim objTXT As TextBox = sender
        ft.enfocarTexto(objTXT)
    End Sub

    Private Sub txtImporte_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtImporte.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el importe monetario de este movimiento ...", Transportables.TipoMensaje.iInfo)
        ft.enfocarTexto(txtImporte)
    End Sub

    Private Sub txtBeneficiario_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBeneficiario.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el nombre y apellido del beneficiario de este movimiento ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtImporte_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtImporte.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub txtImporte_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtImporte.TextChanged

    End Sub

    Private Sub cmbTipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipo.SelectedIndexChanged
        If cmbTipo.SelectedIndex = 0 Then
            ft.habilitarObjetos(True, True, cmbTipoDeposito)
        Else
            ft.habilitarObjetos(False, True, cmbTipoDeposito)
        End If

        If CBool(ParametroPlus(MyConn, Gestion.iBancos, "BANPARAM08")) Then 'si exige movimientos contables obligatorios


        End If

    End Sub
End Class