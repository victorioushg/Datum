Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Public Class jsBanProTransferencias
    Private Const sModulo As String = "Transferencias bancarias"
    Private Const nTabla As String = "tbltran"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private aTipoTransferencia() As String = {"Electrónica", " Cheque"}
    Private numComprobante As String
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        myConn = MyCon
        Me.Tag = sModulo

        Dim dates As SfDateTimeEdit() = {txtFecha}
        SetSizeDateObjects(dates)

        IniciarCuentasEnCombo(myConn, lblInfo, cmbCuentaOrigen, , 1)
        IniciarCuentasEnCombo(myConn, lblInfo, cmbCuentaDestino, , 1)
        txtFecha.Value = jytsistema.sFechadeTrabajo
        txtCambio.Text = ft.FormatoNumero(1.0)
        txtMontoATransferir.Text = ft.FormatoNumero(0.0)
        ft.RellenaCombo(aTipoTransferencia, cmbTipoTransferencia)

        ft.visualizarObjetos(False, txtBeneficiario, txtNumCheque, lblBeneficiario, lblNumCheque)


        ft.mensajeEtiqueta(lblInfo, "Seleccione las cuentas origen y destino, indique la fecha y el monto de la transferencia  ... ", Transportables.TipoMensaje.iAyuda)
        lblLeyenda.Text = " Mediante este proceso se transfiere un monto determinado desde una cuenta origen " + vbCr + _
                " a una cuenta destino. Se producen la correspondiente Nota Débito en la cuenta origen y  " + vbCr + _
                " Nota Crédito en la cuenta destino. "
        Me.Show()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub jsBanProTransferencias_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt.Dispose()
        ft = Nothing
    End Sub

    Private Sub jsBanProTransferencias_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Function Validado() As Boolean

        If FechaUltimoBloqueo(myConn, "jsbantraban") >= txtFecha.Value Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If


        If Mid(cmbCuentaOrigen.Text, 1, 5) = Mid(cmbCuentaDestino.Text, 1, 5) Then
            ft.MensajeCritico("Cuenta origen igual a cuenta destino...")
            Return False
        End If

        If cmbTipoTransferencia.SelectedIndex = 1 Then
            If txtNumCheque.Text.Trim = "" Then
                ft.MensajeCritico("DEBE INDICAR UN NUMERO DE CHEQUE VALIDO...")
                Return False
            End If
            If txtBeneficiario.Text.Trim = "" Then
                ft.MensajeCritico("DEBE INDICAR UN NOMBRE DE BENEFICIARIO VALIDO...")
                Return False
            End If
        End If

        If ft.isNumeric(txtMontoATransferir.Text) Then
            If ValorNumero(txtMontoATransferir.Text) <= 0.0 Then
                ft.mensajeCritico("DEBE INDICAR UN MONTO A TRANSFERIR VALIDO...")
            End If
        Else
            ft.mensajeCritico("DEBE INDICAR UN MONTO A TRANSFERIR VALIDO...")
            Return False
        End If

        Return True
    End Function
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GenerarTransferencia()
            ft.mensajeInformativo(" TRANSFERENCIA REALIZADA CON EXITO ...")

            'ImprimirComprobante() 

            ProgressBar1.Value = 0
            lblProgreso.Text = ""
        End If
    End Sub
    Private Sub GenerarTransferencia()

        Dim codContableSalida As String = ft.DevuelveScalarCadena(myConn, " select codcon from jsbancatban where codban = '" & Mid(cmbCuentaOrigen.Text, 1, 5) & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim codContableEntrada As String = ft.DevuelveScalarCadena(myConn, " select codcon from jsbancatban where codban = '" & Mid(cmbCuentaDestino.Text, 1, 5) & "' and id_emp = '" & jytsistema.WorkID & "' ")

        Dim codContableCaja As String = ft.DevuelveScalarCadena(myConn, " select codcon from jsbanenccaj where caja = '00' and id_emp = '" & jytsistema.WorkID & "' ")
        If codContableCaja = "0" Then codContableCaja = ""


        Dim numTrans As String = Contador(myConn, lblInfo, Gestion.iBancos, "BANNUMTRA", "01")
        numComprobante = Contador(myConn, lblInfo, Gestion.iBancos, "BANNUMRCC", "03")


        If cmbTipoTransferencia.SelectedIndex = 0 Then 'ELECTRONICA  

            'BANCO ORIGEN
            InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, txtFecha.Value, numTrans, "ND", Mid(cmbCuentaOrigen.Text, 1, 5), "",
                "TRANSFERENCIA BANCARIA", -1 * ValorNumero(txtMontoReal.Text), "BAN", numTrans, "", numComprobante, "0", jytsistema.sFechadeTrabajo,
                jytsistema.sFechadeTrabajo, "ND", "", jytsistema.sFechadeTrabajo, "0", "", "", jytsistema.WorkCurrency.Id, DateTime.Now())

            'BANCO DESTINO
            InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, txtFecha.Value, numTrans, "NC", Mid(cmbCuentaDestino.Text, 1, 5), "",
                "TRANSFERENCIA BANCARIA", ValorNumero(txtMontoReal.Text), "BAN", numTrans, "", numComprobante, "0", jytsistema.sFechadeTrabajo,
                jytsistema.sFechadeTrabajo, "NC", "", jytsistema.sFechadeTrabajo, "0", "", "", jytsistema.WorkCurrency.Id, DateTime.Now())

        Else ' CHEQUE

            'BANCO ORIGEN
            InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, txtFecha.Value, numTrans, "CH",
                 Mid(cmbCuentaOrigen.Text, 1, 5), "", "TRANSFERENCIA BANCARIA", -1 * ValorNumero(txtMontoReal.Text),
                "BAN", numTrans, txtBeneficiario.Text, numComprobante, "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, "CH", "", jytsistema.sFechadeTrabajo,
                "0", "", "", jytsistema.WorkCurrency.Id, DateTime.Now())


            InsertEditBANCOSRenglonORDENPAGO(myConn, lblInfo, True, numComprobante, "00002", codContableCaja,
                                           numTrans, "TRANSFERENCIA BANCARIA", ValorNumero(txtMontoReal.Text), 0, 0)
            InsertEditBANCOSRenglonORDENPAGO(myConn, lblInfo, True, numComprobante, "00003", codContableCaja,
                                            numTrans, "TRANSFERENCIA BANCARIA", -1 * ValorNumero(txtMontoReal.Text), 0, 1)

            'BANCO DESTINO
            InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, txtFecha.Value, numTrans, "DP",
                 Mid(cmbCuentaDestino.Text, 1, 5), "", "TRANSFERENCIA BANCARIA", ValorNumero(txtMontoReal.Text),
                "BAN", numTrans, txtBeneficiario.Text, numComprobante, "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, "CH", "", jytsistema.sFechadeTrabajo,
                "0", "", "", jytsistema.WorkCurrency.Id, DateTime.Now())

        End If

        InsertEditBANCOSRenglonORDENPAGO(myConn, lblInfo, True, numComprobante, "00001", codContableSalida,
                                            numTrans, "TRANSFERENCIA BANCARIA", -1 * ValorNumero(txtMontoReal.Text), 0, 1)
        InsertEditBANCOSRenglonORDENPAGO(myConn, lblInfo, True, numComprobante, "00004", codContableEntrada,
                                           numTrans, "TRANSFERENCIA BANCARIA", ValorNumero(txtMontoReal.Text), 0, 0)



        ProgressBar1.Value = 100
        lblProgreso.Text = ""

        IniciarCuentasEnCombo(myConn, lblInfo, cmbCuentaOrigen, , 1)
        IniciarCuentasEnCombo(myConn, lblInfo, cmbCuentaDestino, , 1)

        ImprimirComprobante(numTrans)

    End Sub
    Private Sub ImprimirComprobante(ByVal NumeroTransferencia As String)

        If cmbTipoTransferencia.SelectedIndex = 1 Then
            'Dim resp As Integer
            'resp = MsgBox(" ¿Desea imprimir comprobante de egreso? ", MsgBoxStyle.YesNo, sModulo)
            'If resp = MsgBoxResult.Yes Then
            Dim f As New jsBanRepParametros
            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cComprobanteDeEgreso, "Comprobante de pago", Mid(cmbCuentaOrigen.Text, 1, 5), numComprobante)
            f = Nothing
            'End If
        Else
            Dim f As New jsBanRepParametros
            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cTransferencia, "Transferencia entre cuentas bancarias", NumeroTransferencia)
            f = Nothing
        End If

    End Sub

    Private Sub txtCambio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCambio.KeyPress,
        txtMontoATransferir.KeyPress, txtMontoReal.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub
    Private Sub txtMontoATransferir_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMontoATransferir.TextChanged,
        txtCambio.TextChanged
        txtMontoReal.Text = ft.FormatoNumero(ValorNumero(txtCambio.Text) * ValorNumero(txtMontoATransferir.Text))
    End Sub
    Private Sub cmbTipoTransferencia_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbTipoTransferencia.SelectedIndexChanged
        Select Case cmbTipoTransferencia.SelectedIndex
            Case 0 ' ELECTRONICA
                ft.visualizarObjetos(False, txtBeneficiario, txtNumCheque, lblBeneficiario, lblNumCheque)
            Case 1 'CHEQUE
                ft.visualizarObjetos(True, txtBeneficiario, txtNumCheque, lblBeneficiario, lblNumCheque)
        End Select
    End Sub

End Class