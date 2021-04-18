Imports MySql.Data.MySqlClient
Public Class jsComFormasPago
    Private Const sModulo As String = "Condiciones de Pago"
    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As DataTable
    Private ft As New Transportables

    Private nTabla As String = "tblpagos"
    Private strSQL As String = ""

    Private i_modo As Integer
    Private aCondicion() As String = {"CREDITO", "CONTADO"}
    Private nModulo As String = ""
    Private NumeroDocumento As String = ""
    Private TotalFactura As Double = 0.0

    Private n_CondicionPago As Integer
    Private n_TipoCredito As Integer
    Private n_Vencimiento As Date = MyDate
    Private n_Caja As String = ""
    Private n_FormaPago As String = ""
    Private n_NombrePago As String = ""
    Private n_NumeroPago As String = ""
    Private n_Beneficiario As String = ""
    Private n_FechaDocumentoPago As Date = MyDate
    Public Property CondicionPago() As Integer
        Get
            Return n_CondicionPago
        End Get
        Set(ByVal value As Integer)
            n_CondicionPago = value
        End Set
    End Property
    Public Property TipoCredito() As Integer
        Get
            Return n_TipoCredito
        End Get
        Set(ByVal value As Integer)
            n_TipoCredito = value
        End Set
    End Property

    Public Property Vencimiento() As Date
        Get
            Return n_Vencimiento
        End Get
        Set(ByVal value As Date)
            n_Vencimiento = value
        End Set
    End Property

    Public Property Caja() As String
        Get
            Return n_Caja
        End Get
        Set(ByVal value As String)
            n_Caja = value
        End Set
    End Property
    Public Property FormaPago() As String
        Get
            Return n_FormaPago
        End Get
        Set(ByVal value As String)
            n_FormaPago = value
        End Set
    End Property
    Public Property NumeroPago() As String
        Get
            Return n_NumeroPago
        End Get
        Set(ByVal value As String)
            n_NumeroPago = value
        End Set
    End Property
    Public Property NombrePago() As String
        Get
            Return n_NombrePago
        End Get
        Set(ByVal value As String)
            n_NombrePago = value
        End Set
    End Property
    Public Property Beneficiario() As String
        Get
            Return n_Beneficiario
        End Get
        Set(ByVal value As String)
            n_Beneficiario = value
        End Set
    End Property
    Public Property FechaDocumentoPago() As Date
        Get
            Return n_FechaDocumentoPago
        End Get
        Set(ByVal value As Date)
            n_FechaDocumentoPago = value
        End Set
    End Property

    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal ModuloOrigen As String, _
                      ByVal Documento As String, ByVal PermiteSeleccionarCRCO As Boolean, ByVal TotalAPagar As Double, _
                      Optional ByVal VerCajas As Boolean = False)

        MyConn = MyCon
        nModulo = ModuloOrigen
        NumeroDocumento = Documento
        TotalFactura = TotalAPagar

        IniciarCredito()
        IniciarContado()

        ft.RellenaCombo(aCondicion, cmbCondicion, CondicionPago)
        ft.habilitarObjetos(PermiteSeleccionarCRCO, True, cmbCondicion)

        IniciarCajas()

        Me.ShowDialog()

    End Sub
    Private Sub IniciarContado()

        ft.habilitarObjetos(False, True, txtImporte, txtVencimientoPago)

        ft.RellenaCombo(aFormaPagoCompras, cmbFP)
        txtNombrePago.Text = ""
        txtNumeroPago.Text = ""
        txtBeneficiario.Text = ""
        txtImporte.Text = ft.FormatoNumero(TotalFactura)
        txtVencimientoPago.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)


    End Sub

    Private Sub IniciarCajas()

        Dim dtCajas As DataTable
        Dim nTablaCajas As String = "tblCajas"

        ds = DataSetRequery(ds, " select * from jsbanenccaj where id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaCajas, lblInfo)
        dtCajas = ds.Tables(nTablaCajas)

        RellenaComboConDatatable(cmbCaja, dtCajas, "nomcaja", "caja")

    End Sub
    Private Sub IniciarCredito()
        Dim aCreadito() As String = {"VENCIMIENTO"}  ', "Giros"}

        txtVence.Text = ft.FormatoFecha(Vencimiento)
        ft.RellenaCombo(aCreadito, cmbCredito)

        ft.habilitarObjetos(False, True, txtVence)

    End Sub

    Private Sub cmbCondicion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCondicion.SelectedIndexChanged
        grpCredito.Location = New Point(1, 100) : grpContado.Location = New Point(1, 100)

        ft.visualizarObjetos(Not CBool(cmbCondicion.SelectedIndex), grpCredito, lblCaja, cmbCaja)
        ft.visualizarObjetos(CBool(cmbCondicion.SelectedIndex), grpContado, lblCaja, cmbCaja)

    End Sub



    Private Sub jsCOMFormasPago_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt = Nothing
        ds = Nothing
    End Sub



    Private Sub btnVence_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVence.Click
        txtVence.Text = SeleccionaFecha(CDate(txtVence.Text), Me, btnVence)
    End Sub

    Private Sub cmbCredito_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCredito.SelectedIndexChanged
        grpVencimiento.Location = New Point(5, 50) : grpGiros.Location = New Point(5, 50)

        ft.visualizarObjetos(Not CBool(cmbCredito.SelectedIndex), grpVencimiento)
        ft.visualizarObjetos(CBool(cmbCredito.SelectedIndex), grpGiros)

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Eliminar pagos
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            TipoCredito = cmbCredito.SelectedIndex
            CondicionPago = cmbCondicion.SelectedIndex
            If cmbCondicion.SelectedIndex = 0 Then    'credito 
                Vencimiento = CDate(txtVence.Text)
            Else
                Vencimiento = CDate(txtVencimientoPago.Text)
            End If
            Caja = cmbCaja.SelectedValue
            FormaPago = aFormaPagoAbreviadaCompras(cmbFP.SelectedIndex)
            NombrePago = txtNombrePagoX.Text
            NumeroPago = txtNumeroPago.Text
            Beneficiario = txtBeneficiario.Text
            FechaDocumentoPago = CDate(txtVencimientoPago.Text)
            scrMain.Focus()
        End If
    End Sub
    Private Function Validado() As Boolean
        If cmbCondicion.SelectedIndex = 1 Then 'CONTADO

            Select Case aFormaPagoAbreviadaCompras(cmbFP.SelectedIndex)
                Case "EF"
                Case "CH", "DP", "TR"
                    If txtNombrePagoX.Text.Trim() = "" Then
                        ft.mensajeCritico("Debe Seleccionar un NOMBRE DE PAGO válida")
                        btnNombrePago.Focus()
                        Return False
                    End If
                Case "TA"
                Case "CT"
            End Select

        Else 'CREDITO
            
        End If

        Return True
    End Function


    Private Sub btnNombrePago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNombrePago.Click
        Select Case aFormaPagoAbreviadaCompras(cmbFP.SelectedIndex)

            Case "EF"
            Case "CH", "DP", "TR"
                txtNombrePagoX.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codban codigo, nomban descripcion, ctaban from jsbancatban where id_emp = '" & jytsistema.WorkID & "' ", "Bancos", txtNombrePagoX.Text)
            Case "TA"
            Case "CT"

        End Select
    End Sub

    Private Sub txtNombrePagoX_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNombrePagoX.TextChanged
        Select Case aFormaPagoAbreviadaCompras(cmbFP.SelectedIndex)
            Case "CH", "DP", "TR"
                txtNombrePago.Text = ft.DevuelveScalarCadena(MyConn, " select nomban from jsbancatban where codban = '" & txtNombrePagoX.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            Case Else

        End Select

    End Sub

    Private Sub cmbFP_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFP.SelectedIndexChanged
        txtNombrePago.Text = ""
        txtNumeroPago.Text = ""
        txtBeneficiario.Text = ""
        txtImporte.Text = ft.FormatoNumero(TotalFactura)
        txtVencimientoPago.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)

        Select Case cmbFP.SelectedIndex
            Case 2, 3

                'Dim FP As String = aFormaPagoAbreviadaCompras(cmbFP.SelectedIndex)
                'calculaIVAFacturasConCondicionEspecial(MyConn, nomTablaRenglones, nomTablaIVA, 0, NumeroDocumento, _
                '                                        OrigenFactura, NumeroSerialFiscal, MontoRestante, FP)
        End Select

    End Sub

    Private Sub btnFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFecha.Click
        Select Case aFormaPagoAbreviadaCompras(cmbFP.SelectedIndex)
            Case "CH", "DP", "TR"
                txtVencimientoPago.Text = SeleccionaFechaPlus(CDate(txtVencimientoPago.Text), CDate(txtVencimientoPago.Text), False, Me, btnFecha)
        End Select

    End Sub
End Class