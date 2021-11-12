Imports MySql.Data.MySqlClient
Imports fTransport
Public Class jsGenFormasPago
    Private Const sModulo As String = "Condiciones de Pago"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As DataTable
    Private ft As New Transportables
    Private pagosRecibidos As New List(Of FormaDePagoYMoneda)

    Private nTabla As String = "tblpagos"
    Private strSQL As String = ""

    Private i_modo As Integer
    Private aCondicion() As String = {"CREDITO", "CONTADO"}
    Private nModulo As String = ""
    Private NumeroDocumento As String = ""
    Private TotalFactura As Double = 0.0
    Private FacturaEnApartado As Boolean = False
    Private personaJuridica As Integer
    Private NumeroSerialFiscal As String = ""
    Private nomTablaIVA As String = ""
    Private nomTablaRenglones As String = ""

    Public Property CondicionPago As Integer
    Public Property TipoCredito As Integer
    Public Property Vencimiento As Date
    Public Property Caja As String
    Public Property condicionIVAPeriodoEspecial As Boolean
    Public Property FechaDocumento As DateTime?



    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal ModuloOrigen As String,
                      ByVal Documento As String, ByVal PermiteSeleccionarCRCO As Boolean,
                      ByVal TotalAPagar As Double, ByVal Moneda As Integer,
                      Optional ByVal VerCajas As Boolean = False, Optional ByVal Apartado As Boolean = False,
                      Optional TipoPersonaJuridica As Integer = 0)

        '0 = Juridica ; 1 = Natural

        MyConn = MyCon
        nModulo = ModuloOrigen
        NumeroDocumento = Documento
        TotalFactura = TotalAPagar
        FacturaEnApartado = Apartado
        personaJuridica = TipoPersonaJuridica

        If jytsistema.WorkBox <> "" Then NumeroSerialFiscal = NumeroSERIALImpresoraFISCAL(MyConn, lblInfo, jytsistema.WorkBox)

        Select Case nModulo

            Case "PVE"
                nomTablaIVA = "jsvenivapos"
                nomTablaRenglones = "jsvenrenpos"
            Case Else
                nomTablaIVA = "jsvenivafac"
                nomTablaRenglones = "jsvenrenfac"

        End Select
        IniciarCredito()
        IniciarContado()

        ft.RellenaCombo(aCondicion, cmbCondicion, CondicionPago)
        ft.habilitarObjetos(PermiteSeleccionarCRCO, True, cmbCondicion)

        IniciarCajas()
        ft.visualizarObjetos(VerCajas, lblCaja, cmbCaja)

        Me.KeyPreview = True
        Me.ShowDialog()

    End Sub
    Private Sub IniciarContado()

        AsignarTooltips()
        ft.habilitarObjetos(False, True, txtSubtotal, txtIVA, txtAPagar, txtEfectivo, txtCambio)

        Dim aCampos() As String = {"FormaDePago.Pago.50.I.",
                                   "NumeroDePago.Nº Pago.120.I.",
                                   "NombreDePago.Nombre.120.I.",
                                   "Importe.Importe.110.D.Numero",
                                   "CodigoIso.moneda.60.C.",
                                   "ImporteReal.Importe Real.110.D.Numero",
                                   "vence.Vence.120.C.Fecha"}

        strSQL = " select a.formapag FormaDePago, a.numpag NumeroDePago, a.nompag NombreDePago, a.importe, a.vence, b.CodigoIso, b.UnidadMonetaria, b.Simbolo, b.Equivale,  " _
            & " a.numfac NumeroFactura, a.origen, a.Currency, a.Currency_Date, a.numserial SerialCaja " _
            & " from jsvenforpag a " _
            & " Left join (" & SQLSelectCambiosYMonedas(IIf(FechaDocumento Is Nothing, jytsistema.sFechadeTrabajo, FechaDocumento)) & ") b " _
            & " on ( a.currency = b.moneda ) " _
            & " where " _
            & " numfac = '" & NumeroDocumento & "' and " _
            & " origen = '" & nModulo & "' and " _
            & " id_emp = '" & jytsistema.WorkID & "' order by formapag "

        pagosRecibidos = Lista(Of FormaDePagoYMoneda)(MyConn, strSQL)
        dt = ft.AbrirDataTable(ds, nTabla, MyConn, strSQL)


        Dim impuestoIVA As Double = ft.DevuelveScalarDoble(MyConn, " select sum(impiva) from " & nomTablaIVA & " " _
                                                               & " where " _
                                                               & " numfac = '" & NumeroDocumento & "' and " _
                                                               & " id_emp = '" & jytsistema.WorkID & "' ")

        Dim netoFactura As Double = ft.DevuelveScalarDoble(MyConn, " select sum(baseiva) from " & nomTablaIVA & " " _
                                                               & " where " _
                                                               & " numfac = '" & NumeroDocumento & "' and " _
                                                               & " id_emp = '" & jytsistema.WorkID & "' ")

        txtIVA.Text = ft.FormatoNumero(impuestoIVA)
        txtAPagar.Text = ft.FormatoNumero(netoFactura + impuestoIVA)
        txtSubtotal.Text = ft.FormatoNumero(netoFactura + impuestoIVA)


        dg.DataSource = pagosRecibidos

        ft.IniciarTablaList(Of FormaDePagoYMoneda)(dg, pagosRecibidos, aCampos)
        CalculaTotales()

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnAgregar, btnEliminar, btnPrimero, btnSiguiente, btnAnterior, btnUltimo,
                          btnEfectivo)

        MenuBarra.ImageList = ImageList1

        btnAgregar.Image = ImageList1.Images(0)
        btnEliminar.Image = ImageList1.Images(1)

        btnPrimero.Image = ImageList1.Images(2)
        btnAnterior.Image = ImageList1.Images(3)
        btnSiguiente.Image = ImageList1.Images(4)
        btnUltimo.Image = ImageList1.Images(5)

        btnEfectivo.Image = ImageList1.Images(6)

    End Sub
    Private Sub IniciarCajas()

        Dim dtCajas As DataTable
        Dim nTablaCajas As String = "tblCajas"

        ds = DataSetRequery(ds, " select * from jsbanenccaj where id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaCajas, lblInfo)
        dtCajas = ds.Tables(nTablaCajas)

        RellenaComboConDatatable(cmbCaja, dtCajas, "nomcaja", "caja")

    End Sub
    Private Sub IniciarCredito()
        Dim aCreadito() As String = {"Vencimiento"}  ', "Giros"}
        txtVence.MinDateTime = jytsistema.sFechadeTrabajo
        txtVence.Value = Vencimiento
        ft.RellenaCombo(aCreadito, cmbCredito)

    End Sub

    Private Sub cmbCondicion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCondicion.SelectedIndexChanged

        grpCredito.Location = New Point(1, 100) : grpContado.Location = New Point(1, 100)

        ft.visualizarObjetos(Not CBool(cmbCondicion.SelectedIndex), grpCredito)
        ft.visualizarObjetos(CBool(cmbCondicion.SelectedIndex), grpContado)

    End Sub

    Private Sub jsGenFormasPago_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt = Nothing
        ds = Nothing
        ft = Nothing
    End Sub

    Private Sub jsGenFormasPago_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyData
            Case Keys.F4
                If ValorNumero(txtDiferencia.Text) < 0 Then
                    Dim f As New jsGenFormasPagoMovimientoPlus
                    f.Agregar(MyConn, ds, dt, NumeroDocumento, nModulo, Math.Abs(ValorNumero(txtDiferencia.Text)), personaJuridica,
                              nomTablaRenglones, nomTablaIVA)
                    ds = DataSetRequery(ds, strSQL, MyConn, nTabla, lblInfo)
                    If f.Apuntador >= 0 Then AsignaMov(f.Apuntador, True)
                    f = Nothing
                End If
            Case Keys.F6
                If dt.Rows.Count > 0 Then
                    Dim Apuntador As Long = Me.BindingContext(ds, nTabla).Position

                    Dim aCamposDel() As String = {"numfac", "origen", "formapag", "numpag", "id_emp"}
                    With dt.Rows(Apuntador)
                        Dim aStringsDel() As String = { .Item("numfac"), .Item("origen"), .Item("formapag"), .Item("numpag"), jytsistema.WorkID}

                        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                            AsignaMov(EliminarRegistros(MyConn, lblInfo, ds, nTabla, "jsvenforpag", strSQL, aCamposDel, aStringsDel,
                                                     Apuntador, True), True)
                        End If
                    End With
                End If
            Case Keys.Control + Keys.E
                IncluirEfectivo()
        End Select
        CalculaTotales()
    End Sub

    Private Sub jsGenFormasPago_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub cmbCredito_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCredito.SelectedIndexChanged
        grpVencimiento.Location = New Point(5, 50) : grpGiros.Location = New Point(5, 50)

        ft.visualizarObjetos(Not CBool(cmbCredito.SelectedIndex), grpVencimiento)
        ft.visualizarObjetos(CBool(cmbCredito.SelectedIndex), grpGiros)

    End Sub

    Private Sub CalculaTotales()

        If NumeroDocumento <> "" AndAlso nModulo <> "" Then

            Dim subTotalPagos As Decimal = pagosRecibidos.Sum(Function(p) p.ImporteReal)
            txtRegistrado.Text = ft.FormatoNumero(subTotalPagos)

            Dim pagadoEnEfectivo As Decimal = pagosRecibidos.Where(Function(e) e.FormaDePago = "EF").Sum(Function(p) p.ImporteReal)
            txtEfectivo.Text = ft.FormatoNumero(pagadoEnEfectivo)

            txtDiferencia.Text = ft.FormatoNumero(subTotalPagos - ValorNumero(txtAPagar.Text))
            txtDiferencia.ForeColor = IIf(ValorNumero(txtDiferencia.Text) >= 0, Color.Navy, Color.Red)

        End If
    End Sub
    Private Sub IncluirEfectivo()

        Dim diferencia As Double = MontoResidualFactura()
        If diferencia < 0 Then _
            InsertEditVentasFormaPago(MyConn, lblInfo, True, NumeroDocumento, NumeroSerialFiscal,
                nModulo, "EF", "", "", Math.Abs(diferencia),
                jytsistema.sFechadeTrabajo, jytsistema.WorkCurrency.Id, DateTime.Now())

            AsignaMov(dt.Rows.Count, True)
        'End If

    End Sub
    Private Function MontoResidualFactura() As Decimal
        Return pagosRecibidos.Sum(Function(p) p.ImporteReal) - ValorNumero(txtAPagar.Text)
    End Function
    Private Sub txtSuPago_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSuPago.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub
    Private Sub txtSuPago_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSuPago.TextChanged
        txtCambio.Text = ft.FormatoNumero(ValorNumero(txtEfectivo.Text) - ValorNumero(txtSuPago.Text))
        txtCambio.ForeColor = IIf(ValorNumero(txtCambio.Text) >= 0, Color.Green, Color.Red)
    End Sub
    Private Sub btnEfectivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEfectivo.Click
        IncluirEfectivo()
        CalculaTotales()
    End Sub


    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        If Actualiza Then
            pagosRecibidos = Lista(Of FormaDePagoYMoneda)(MyConn, strSQL)
            dg.DataSource = pagosRecibidos
        End If

        dg.Rows().Item(nRow).Selected = True
        MostrarItemsEnMenuBarra(MenuBarra, CInt(nRow), pagosRecibidos.Count)


    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Todo eliminar pagos
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            TipoCredito = cmbCredito.SelectedIndex
            CondicionPago = cmbCondicion.SelectedIndex
            scrMain.Focus()
        End If
    End Sub
    Private Function Validado() As Boolean
        If FacturaEnApartado Then
        Else
            If cmbCondicion.SelectedIndex = 1 Then 'CONTADO
                If pagosRecibidos.Count <= 0 Then
                    ft.mensajeAdvertencia("Debe indicar al menos una forma de pago válida...")
                    Return False
                End If
                If ValorNumero(txtDiferencia.Text) <> 0.0 Then
                    ft.mensajeAdvertencia("La suma de los pagos debe ser cero (0.00). Verifique por favor...")
                    Return False
                End If
            Else 'CREDITO
            End If

        End If
        Return True
    End Function

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

        Dim diferencia As Double = MontoResidualFactura()
        If diferencia < 0 Then
            Dim f As New jsGenFormasPagoMovimientoPlus
            f.Agregar(MyConn, ds, dt, NumeroDocumento, nModulo, Math.Abs(diferencia), personaJuridica,
                      nomTablaRenglones, nomTablaIVA)
            If f.Apuntador >= 0 Then AsignaMov(f.Apuntador, True)
            f = Nothing
        End If
        CalculaTotales()

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If pagosRecibidos.Count > 0 Then

            Dim Apuntador = dg.SelectedRows(0).Index
            Dim data As FormaDePagoYMoneda = dg.Rows(Apuntador).DataBoundItem

            Dim aCamposDel() As String = {"numfac", "origen", "formapag", "numpag", "Currency"}
            Dim aStringsDel() As String = {data.NumeroFactura, data.Origen, data.FormaDePago, data.NumeroDePago, data.Currency}
            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                AsignaMov(EliminarRegistros(MyConn, lblInfo, ds, nTabla, "jsvenforpag", strSQL, aCamposDel, aStringsDel,
                                         Apuntador, True), True)
            End If

            CalculaTotales()
        End If
    End Sub


    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        ' Dim selectedRowCount = dg.Rows.GetRowCount(DataGridViewElementStates.Selected)
        Dim nRow As Integer = dg.Rows.GetFirstRow(DataGridViewElementStates.None)
        AsignaMov(IIf(nRow < 0, 0, nRow), False)
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Dim nRow As Integer = dg.Rows.GetPreviousRow(dg.CurrentRow.Index, DataGridViewElementStates.None)
        AsignaMov(IIf(nRow < 0, 0, nRow), False)
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Dim nRow As Integer = dg.Rows.GetNextRow(nRow, DataGridViewElementStates.None)
        AsignaMov(IIf(nRow < 0, 0, nRow), False)
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Dim nRow As Integer = dg.Rows.GetLastRow(DataGridViewElementStates.None)
        AsignaMov(IIf(nRow < 0, 0, nRow), False)
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
       dg.RowHeaderMouseClick, dg.CellMouseClick
        AsignaMov(e.RowIndex, False)
    End Sub

End Class