Imports MySql.Data.MySqlClient
Imports fTransport
Public Class jsGenFormasPagoMovimientoPlus

    Private Const sModulo As String = "Movimiento de Formas de pago "

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables
    Private interchangeList As New List(Of CambioMonedaPlus)
    Private moneda As New CambioMonedaPlus

    Private i_modo As Integer

    Private NumeroFactura As String
    Private OrigenFactura As String
    Private MontoRestante As Double
    Private aFormaPagoPVE() As String
    Private aFormaPagoPVEAbreviada() As String
    Private personaJuridica As Integer
    Private NumeroSerialFiscal As String = ""
    Private nomTablaRenglones As String = ""
    Private nomTablaIVA As String = ""

    Public Property Apuntador() As Long
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable,
                       ByVal nNumeroFactura As String, ByVal Origen As String, ByVal nMontoRestante As Double,
                       tipoPersonaJuridica As Integer, tablaRenglones As String, tablaIVA As String)

        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        NumeroFactura = nNumeroFactura
        OrigenFactura = Origen
        MontoRestante = nMontoRestante
        personaJuridica = tipoPersonaJuridica
        nomTablaRenglones = tablaRenglones
        nomTablaIVA = tablaIVA
        interchangeList = GetListaDeMonedasyCambios(MyConn, jytsistema.sFechadeTrabajo)

        If jytsistema.WorkBox <> "" Then NumeroSerialFiscal = NumeroSERIALImpresoraFISCAL(MyConn, lblInfo, jytsistema.WorkBox)

        IniciarTXT()

        Me.ShowDialog()
    End Sub

    Private Sub IniciarTXT()

        mostrarGrilla()
        txtRestoFactura.Text = ft.muestraCampoNumero(MontoRestante)

        InitiateDropDownInterchangeCurrency(MyConn, cmbMonedas, jytsistema.sFechadeTrabajo, True)

        txtNumeroPago.Text = ""
        txtNombrePago.Text = ""
        txtImporte.Text = ft.FormatoNumero(MontoRestante)
        txtVence.Value = jytsistema.sFechadeTrabajo

    End Sub

    Private Sub mostrarGrilla()

        Dim nTablaIVA As String = "tblIVAFP"
        Dim dtIva As DataTable = ft.AbrirDataTable(dsLocal, nTablaIVA, MyConn, " select * from " & nomTablaIVA _
                                                   & " WHERE " _
                                                   & " NUMFAC = '" & NumeroFactura & "' AND " _
                                                   & " ID_EMP = '" & jytsistema.WorkID & "' ORDER BY TIPOIVA ")

        Dim aCampos() As String = {"tipoiva.TP.25.C.",
                                   "poriva.%.60.D.Numero",
                                   "baseiva.Base.120.D.Numero",
                                   "impiva.Impuesto.120.D.Numero"}

        ft.IniciarTablaPlus(dgIVA, dtIva, aCampos, , , New Font("Consolas", 11, FontStyle.Regular), False, 20)

        dtIva.Dispose()
        dtIva = Nothing

    End Sub

    Private Sub jsGenFormasPagoMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        ft = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsGenFormasPagoMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ft.habilitarObjetos(False, True, txtNombrePago, txtVence, txtRestoFactura)
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, cmbFP.SelectedValue)

        '//// ELIMINA FORMAS DE PAGO NO PERMITIDAS
        Dim dataFP = formasDePago
        InitiateDropDown(Of TextoValor)(MyConn, cmbFP, Tipo.FormaDePago)
        If OrigenFactura = "PVE" Then

            If Not CBool(ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM06")) Then
                cmbFP.DataSource = dataFP.Where(Function(item) item.Value <> "CH").ToList()
                cmbFP.SelectedValue = "TA"
            End If

            If Not CBool(ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM07")) Then
                cmbFP.DataSource = dataFP.Where(Function(item) item.Value <> "TA").ToList()
                cmbFP.SelectedValue = "EF"
            End If

            If Not CBool(ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM08")) Then
                cmbFP.DataSource = dataFP.Where(Function(item) item.Value <> "CT").ToList()
                cmbFP.SelectedValue = "TA"
            End If

            If Not CBool(ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM08")) Then
                cmbFP.DataSource = dataFP.Where(Function(item) item.Value <> "DP" And item.Value <> "TR").ToList()
                cmbFP.SelectedValue = "TA"
            End If

        Else
            cmbFP.DataSource = formasDePago.Where(Function(item) item.Value <> "CT").ToList()
            cmbFP.SelectedValue = "TA"
        End If

    End Sub

    Private Sub txtcmbFP_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNumeroPago.GotFocus, txtNombrePago.GotFocus, txtImporte.GotFocus,
        btnNombrePago.GotFocus
        Select Case sender.text
            Case "cmbFP"
                lblInfo.Text = "Seleccione forma de pago ..."
            Case "txtNumeroPago"
                lblInfo.Text = "Indique el número de documento de pago ..." : sender.maxlength = 20
            Case "txtNombrePago"
                lblInfo.Text = "Indique el nombre de documento de pago (banco, etc..) ..." : sender.maxlength = 20
            Case "txtImporte"
                lblInfo.Text = "Indique el monto a cancelar en este pago ..." : sender.maxlength = 15
            Case "txtVence"
                lblInfo.Text = "Indique la fecha de pago ó fecha a futuro ..."
            Case "btnFecha"
                lblInfo.Text = "Seleccione la fecha para este pago ..."
            Case "btnNombrePago"
                lblInfo.Text = "Seleccione el nombre o institución de pago ..."
        End Select

    End Sub
    Private Sub txtImporte_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtImporte.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub
    Private Function Validado() As Boolean
        Validado = False

        If cmbFP.SelectedValue <> "EF" Then
            If Trim(txtNumeroPago.Text) = "" Then
                ft.mensajeAdvertencia("Debe indicar un número de pago válido...")
                Exit Function
            End If

            If Trim(txtNombrePago.Text) = "" Then
                ft.mensajeAdvertencia("Debe indicar ó escoger un nombre de pago válido...")
                Exit Function
            End If
        End If

        If ValorNumero(txtImporte.Text) > Math.Round(MontoRestante * moneda.Equivale, 2) Then
            ft.mensajeAdvertencia("Debe indicar un monto menor al monto restante por pagar...")
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

            Dim NumeroSerialFiscal As String = ""
            If jytsistema.WorkBox <> "" Then NumeroSerialFiscal = NumeroSERIALImpresoraFISCAL(MyConn, lblInfo, jytsistema.WorkBox)

            InsertEditVentasFormaPago(MyConn, lblInfo, Insertar, NumeroFactura, NumeroSerialFiscal, OrigenFactura, cmbFP.SelectedValue,
                                        txtNumeroPago.Text, txtNombrePagoX.Text, ValorNumero(txtImporte.Text), txtVence.Value,
                                       cmbMonedas.SelectedValue, DateTime.Now)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, NumeroFactura + cmbFP.SelectedValue)

            Me.Close()
        End If

    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, NumeroFactura)
        Me.Close()
    End Sub

    Private Sub cmbFP_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFP.SelectedIndexChanged

        Dim aEtiquetaNumero() As String = {"", "Número cheque :", "Número tarjeta :", "Número Factura :", "Número depósito :", "Nº Tranferencia :"}
        Dim aEtiquetaNombre() As String = {"", "Banco :", "Tarjeta :", "", "Banco :", "Banco :"}

        If i_modo = movimiento.iAgregar Then
            ft.habilitarObjetos(True, True, txtNumeroPago, txtNombrePago, txtImporte, txtVence, btnNombrePago, txtVence)
            txtNumeroPago.Text = ""
            txtNombrePago.Text = ""
            Select Case cmbFP.SelectedValue
                Case "EF" 'EFECTIVO
                    ft.habilitarObjetos(False, True, txtNumeroPago, txtNombrePago, txtVence, btnNombrePago, txtVence)
                Case "CH", "CT", "DP"
                    ft.habilitarObjetos(False, True, txtNombrePago, txtVence)
                    If cmbFP.SelectedIndex = 3 Then
                        ft.habilitarObjetos(False, True, txtNumeroPago)
                        txtNumeroPago.Text = NumeroFactura
                        txtNombrePago.Text = "CUPON ALIMENTACION"
                    End If
                Case "TA", "TR"
                    ft.habilitarObjetos(False, True, txtNombrePago, txtVence)
            End Select

            mostrarGrilla()

            MontoRestante = Math.Abs(montoResidualFactura(MyConn, nomTablaIVA, NumeroFactura, OrigenFactura))
            txtRestoFactura.Text = ft.muestraCampoNumero(MontoRestante)
            txtImporte.Text = ft.FormatoNumero(MontoRestante * moneda.Equivale)

        End If

        lblNombre.Text = aEtiquetaNombre(cmbFP.SelectedIndex)
        lblNumero.Text = aEtiquetaNumero(cmbFP.SelectedIndex)

    End Sub

    Private Sub txtNombrePagoX_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNombrePagoX.TextChanged
        Select Case cmbFP.SelectedValue
            '{"EF", "CH", "TA", "CT", "DP", "TR"}
            Case "CH" 'CHEQUE
                Dim aFld() As String = {"codigo", "modulo", "id_emp"}
                Dim aStr() As String = {sender.Text, FormatoTablaSimple(Modulo.iBancos), jytsistema.WorkID}
                txtNombrePago.Text = qFoundAndSign(MyConn, lblInfo, "jsconctatab", aFld, aStr, "descrip")
            Case "TA" 'TARJETA
                Dim aFld() As String = {"codtar", "id_emp"}
                Dim aStr() As String = {sender.text, jytsistema.WorkID}
                txtNombrePago.Text = qFoundAndSign(MyConn, lblInfo, "jsconctatar", aFld, aStr, "nomtar")
            Case "DP", "TR" 'DEPOSITO, TRANSFERENCIAS 
                Dim aFld() As String = {"codban", "id_emp"}
                Dim aStr() As String = {sender.Text, jytsistema.WorkID}
                txtNombrePago.Text = qFoundAndSign(MyConn, lblInfo, "jsbancatban", aFld, aStr, "nomban")
        End Select
    End Sub

    Private Sub btnNombrePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNombrePago.Click
        Select Case cmbFP.SelectedValue
            '{"EF", "CH", "TA", "CT", "DP", "TR"}
            Case "CH" ' CHEQUE
                Dim f As New jsControlArcTablaSimple
                f.Cargar("Bancos", FormatoTablaSimple(Modulo.iBancos), False, TipoCargaFormulario.iShowDialog)
                txtNombrePagoX.Text = f.Seleccion
                f = Nothing
            Case "TA" 'TARJETA DEBITO/CREDITO/CT
                Dim f As New jsControlArcTarjetas
                f.Cargar(MyConn, TipoCargaFormulario.iShowDialog)
                txtNombrePagoX.Text = f.Seleccionado
                f = Nothing
            Case "CT" 'CHEQUES DE ALIMENTACION
            Case "DP", "TR" 'DEPOSITO / TRANSFERENCIAS
                Dim dtBancos As DataTable
                Dim nTablaBancos As String = "tblBancos"
                dsLocal = DataSetRequery(dsLocal, " select codban codigo, nomban descripcion from jsbancatban where estatus = 1 and id_emp = '" & jytsistema.WorkID & "' order by codban ", MyConn, nTablaBancos, lblInfo)
                dtBancos = dsLocal.Tables(nTablaBancos)
                Dim f As New jsControlArcTablaSimple
                f.Cargar("Bancos", dsLocal, dtBancos, nTablaBancos, TipoCargaFormulario.iShowDialog, False)
                txtNombrePagoX.Text = f.Seleccion
                f = Nothing
        End Select
    End Sub

    Private Sub cmbMonedas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMonedas.SelectedIndexChanged
        moneda = cmbMonedas.SelectedItem
        txtImporte.Text = ft.FormatoNumero(MontoRestante * moneda.Equivale)
    End Sub
End Class