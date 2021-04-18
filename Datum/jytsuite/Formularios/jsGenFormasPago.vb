Imports MySql.Data.MySqlClient
Public Class jsGenFormasPago
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
    Private n_CondicionIVA As Boolean = False
    Private FacturaEnApartado As Boolean = False
    Private personaJuridica As Integer
    Private NumeroSerialFiscal As String = ""
    Private nomTablaIVA As String = ""
    Private nomTablaRenglones As String = ""
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

    Public Property condicionIVAPeriodoEspecial() As Boolean
        Get
            Return n_CondicionIVA
        End Get
        Set(ByVal value As Boolean)
            n_CondicionIVA = value
        End Set
    End Property



    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal ModuloOrigen As String, _
                      ByVal Documento As String, ByVal PermiteSeleccionarCRCO As Boolean, ByVal TotalAPagar As Double, _
                      Optional ByVal VerCajas As Boolean = False, Optional ByVal Apartado As Boolean = False, _
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

        Dim aCampos() As String = {"formapag.Pago.80.I.", _
                                   "numpag.Nº Pago.120.I.", _
                                   "nompag.Nombre.120.I.", _
                                   "importe.Importe.120.D.Numero", _
                                   "vence.Vencimiento.120.C.Fecha"}

        strSQL = " select * from jsvenforpag " _
            & " where " _
            & " numfac = '" & NumeroDocumento & "' and " _
            & " origen = '" & nModulo & "' and " _
            & " id_emp = '" & jytsistema.WorkID & "' order by formapag "

        dt = ft.AbrirDataTable(ds, nTabla, MyConn, strSQL)

        ft.IniciarTablaPlus(dg, dt, aCampos)

        'txtAPagar.Text = ft.FormatoNumero(TotalFactura)

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

        txtVence.Text = ft.FormatoFecha(Vencimiento)
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
                    f.Agregar(MyConn, ds, dt, NumeroDocumento, nModulo, Math.Abs(ValorNumero(txtDiferencia.Text)), personaJuridica, _
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
                        Dim aStringsDel() As String = {.Item("numfac"), .Item("origen"), .Item("formapag"), .Item("numpag"), jytsistema.WorkID}

                        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                            AsignaMov(EliminarRegistros(MyConn, lblInfo, ds, nTabla, "jsvenforpag", strSQL, aCamposDel, aStringsDel, _
                                                     Apuntador, True), True)
                        End If
                    End With
                End If
            Case Keys.Control + Keys.E
                calculaIVAFacturasConCondicionEspecial(MyConn, nomTablaRenglones, nomTablaIVA, personaJuridica, NumeroDocumento, _
                                                    nModulo, NumeroSerialFiscal, ft.valorNumero(txtAPagar.Text), "EF")
                IncluirEfectivo()

        End Select
        CalculaTotales()
    End Sub

    Private Sub jsGenFormasPago_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnVence_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVence.Click

        Dim FechaNueva As Date = CDate(SeleccionaFecha(CDate(txtVence.Text), Me, btnVence))
        If ft.FormatoFechaMySQL(FechaNueva) > ft.FormatoFechaMySQL(Vencimiento) Then
            txtVence.Text = ft.FormatoFecha(Vencimiento)
        ElseIf ft.FormatoFechaMySQL(FechaNueva) < ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) Then
            txtVence.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        Else
            txtVence.Text = ft.FormatoFecha(FechaNueva)
        End If

    End Sub

    Private Sub cmbCredito_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCredito.SelectedIndexChanged
        grpVencimiento.Location = New Point(5, 50) : grpGiros.Location = New Point(5, 50)

        ft.visualizarObjetos(Not CBool(cmbCredito.SelectedIndex), grpVencimiento)
        ft.visualizarObjetos(CBool(cmbCredito.SelectedIndex), grpGiros)

    End Sub

    Private Sub txtSubtotal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        txtSubtotal.TextChanged, txtAPagar.TextChanged

        txtDiferencia.Text = ft.FormatoNumero(ValorNumero(txtSubtotal.Text) - ValorNumero(txtAPagar.Text))
        txtDiferencia.ForeColor = IIf(ValorNumero(txtDiferencia.Text) >= 0, Color.Navy, Color.Red)

        'Calcula total de pagos en efectivo
        If NumeroDocumento <> "" AndAlso nModulo <> "" Then
            Dim dtEF As DataTable
            Dim nTablaEF As String = "tblEF"
            ds = DataSetRequery(ds, " select formapag, IFNULL(SUM(IMPORTE),0) efectivo from jsvenforpag " _
                                & " where " _
                                & " numfac = '" & NumeroDocumento & "' and " _
                                & " origen = '" & nModulo & "' and " _
                                & " formapag = 'EF' and " _
                                & " id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaEF, lblInfo)
            dtEF = ds.Tables(nTablaEF)
            txtEfectivo.Text = ft.FormatoNumero(IIf(dtEF.Rows.Count > 0, IIf(IsDBNull(dtEF.Rows(0).Item("efectivo")), 0.0, dtEF.Rows(0).Item("efectivo")), 0.0))

            dtEF = Nothing

        End If
    End Sub
    Private Sub CalculaTotales()

        If NumeroDocumento <> "" AndAlso nModulo <> "" Then

            Dim subTotalPagos As Double = ft.DevuelveScalarDoble(MyConn, " select SUM(IMPORTE) from jsvenforpag " _
                                & " where " _
                                & " numfac = '" & NumeroDocumento & "' and " _
                                & " origen = '" & nModulo & "' and " _
                                & " id_emp = '" & jytsistema.WorkID & "' ")

            txtSubtotal.Text = ft.muestraCampoNumero(subTotalPagos)

            Dim impuestoIVA As Double = ft.DevuelveScalarDoble(MyConn, " select sum(impiva) from " & nomTablaIVA & " " _
                                                               & " where " _
                                                               & " numfac = '" & NumeroDocumento & "' and " _
                                                               & " id_emp = '" & jytsistema.WorkID & "' ")

            Dim netoFactura As Double = ft.DevuelveScalarDoble(MyConn, " select sum(baseiva) from " & nomTablaIVA & " " _
                                                               & " where " _
                                                               & " numfac = '" & NumeroDocumento & "' and " _
                                                               & " id_emp = '" & jytsistema.WorkID & "' ")

            Dim porIVA As Double = ft.DevuelveScalarDoble(MyConn, " select poriva from " & nomTablaIVA & " " _
                                                               & " where " _
                                                               & " numfac = '" & NumeroDocumento & "' and " _
                                                               & " tipoiva = 'A' and " _
                                                               & " id_emp = '" & jytsistema.WorkID & "' ")
            If porIVA = 0.0 Then
                lblIVA.Text = " IVA"
            Else
                lblIVA.Text = " IVA (" & ft.muestraCampoNumero(porIVA) & "%)"
            End If

            txtIVA.Text = ft.muestraCampoNumero(impuestoIVA)
            txtAPagar.Text = ft.muestraCampoNumero(netoFactura + impuestoIVA)

        End If
    End Sub
    'Private Sub CalculaIVA(formaDePago As String)

    '    If cumpleCondicionesIVAEspecial(MyConn, personaJuridica, NumeroDocumento, nModulo, txtAPagar.Text, formaDePago) Then
    '        ActualizarIVARenglonAlbaranPlus(MyConn, lblInfo, nomTablaIVA, nomTablaRenglones, "numfac", _
    '                               NumeroDocumento, jytsistema.sFechadeTrabajo, "totrendes", _
    '                               NumeroSerialFiscal)
    '    Else
    '        ActualizarIVARenglonAlbaran(MyConn, lblInfo, nomTablaIVA, nomTablaRenglones, "numfac", _
    '                            NumeroDocumento, jytsistema.sFechadeTrabajo, "totrendes", _
    '                            NumeroSerialFiscal)
    '    End If

    'End Sub

    Private Sub IncluirEfectivo()

        If ft.DevuelveScalarEntero(MyConn, " select count(*) from jsvenforpag " _
                                   & " where " _
                                   & " numfac = '" & NumeroDocumento & "' and " _
                                   & " origen = '" & nModulo & "' and  " _
                                   & " formapag = 'EF' and " _
                                   & " numpag = '' and  " _
                                   & " nompag = '' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then

            Dim diferencia As Double = montoResidualFactura(MyConn, nomTablaIVA, NumeroDocumento, nModulo)    ' ft.valorNumero(txtSubtotal.Text) - ft.valorNumero(txtAPagar.Text)
            If diferencia < 0 Then _
            InsertEditVentasFormaPago(MyConn, lblInfo, True, NumeroDocumento, NumeroSerialFiscal, _
                nModulo, "EF", "", "", Math.Abs(diferencia), _
                jytsistema.sFechadeTrabajo)

            AsignaMov(dt.Rows.Count, True)


        End If

    End Sub

    Private Sub txtSuPago_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSuPago.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub
    Private Sub txtSuPago_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSuPago.TextChanged
        txtCambio.Text = ft.FormatoNumero(ValorNumero(txtEfectivo.Text) - ValorNumero(txtSuPago.Text))
        txtCambio.ForeColor = IIf(ValorNumero(txtCambio.Text) >= 0, Color.Green, Color.Red)
    End Sub
    Private Sub btnEfectivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEfectivo.Click

        calculaIVAFacturasConCondicionEspecial(MyConn, nomTablaRenglones, nomTablaIVA, personaJuridica, NumeroDocumento, _
                                                    nModulo, NumeroSerialFiscal, ft.valorNumero(txtAPagar.Text), "EF")
        IncluirEfectivo()
        CalculaTotales()

    End Sub


    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        If Actualiza Then ds = DataSetRequery(ds, strSQL, MyConn, nTabla, lblInfo)
        Dim c As Integer = CInt(nRow)
        If c >= 0 AndAlso dt.Rows.Count > 0 AndAlso c <= dt.Rows.Count - 1 Then
            Me.BindingContext(ds, nTabla).Position = c
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If
        MostrarItemsEnMenuBarra(MenuBarra, c, dt.Rows.Count)

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

            Dim cantidadFP As Integer = ft.DevuelveScalarEntero(MyConn, " select count(*) from jsvenforpag " _
                                            & " where " _
                                            & " numfac = '" & NumeroDocumento & "' and " _
                                            & " origen = '" & nModulo & "' and " _
                                            & " formapag IN ('EF','CH','DP','CT') AND " _
                                            & " id_emp = '" & jytsistema.WorkID & "' group by formapag ")

            condicionIVAPeriodoEspecial = False

            '0 = Juridica ; 1 = Natural

            Dim porIVA As Double = ft.DevuelveScalarDoble(MyConn, "SELECT PORIVA " _
                                                            & " FROM " & nomTablaIVA & " " _
                                                            & " WHERE " _
                                                            & " numfac = '" & NumeroDocumento & "' AND " _
                                                            & " TIPOIVA = 'A' AND " _
                                                            & " ID_EMP='" & jytsistema.WorkID & "'")

            If TipoCredito = 0 And cantidadFP = 0 And personaJuridica = 1 And porIVA = 10.0 Then
                condicionIVAPeriodoEspecial = True
            End If

            scrMain.Focus()

        End If
    End Sub
    Private Function Validado() As Boolean
        If FacturaEnApartado Then

        Else
            If cmbCondicion.SelectedIndex = 1 Then 'CONTADO
                If dt.Rows.Count <= 0 Then
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

        Dim diferencia As Double = montoResidualFactura(MyConn, nomTablaIVA, NumeroDocumento, nModulo)
        If diferencia < 0 Then
            Dim f As New jsGenFormasPagoMovimientoPlus

            f.Agregar(MyConn, ds, dt, NumeroDocumento, nModulo, Math.Abs(diferencia), personaJuridica, _
                      nomTablaRenglones, nomTablaIVA)

            ds = DataSetRequery(ds, strSQL, MyConn, nTabla, lblInfo)
            If f.Apuntador >= 0 Then AsignaMov(f.Apuntador, True)
            f = Nothing
        End If
        CalculaTotales()
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If dt.Rows.Count > 0 Then
            Dim Apuntador As Long = Me.BindingContext(ds, nTabla).Position

            With dt.Rows(Apuntador)
                Dim aCamposDel() As String = {"numfac", "origen", "formapag", "numpag", "id_emp"}
                Dim aStringsDel() As String = {.Item("numfac"), .Item("origen"), .Item("formapag"), .Item("numpag"), jytsistema.WorkID}

                If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                    AsignaMov(EliminarRegistros(MyConn, lblInfo, ds, nTabla, "jsvenforpag", strSQL, aCamposDel, aStringsDel, _
                                             Apuntador, True), True)

                    Dim cantidadFP As Integer = ft.DevuelveScalarEntero(MyConn, " select count(*) from jsvenforpag " _
                                            & " where " _
                                            & " numfac = '" & NumeroDocumento & "' and " _
                                            & " origen = '" & nModulo & "' and " _
                                            & " formapag IN ('EF','CH','DP','CT') AND " _
                                            & " id_emp = '" & jytsistema.WorkID & "' group by formapag ")

                    Dim fp As String = IIf(cantidadFP > 0, "EF", "TA")

                    calculaIVAFacturasConCondicionEspecial(MyConn, nomTablaRenglones, nomTablaIVA, personaJuridica, NumeroDocumento, _
                                                     nModulo, NumeroSerialFiscal, ft.valorNumero(txtAPagar.Text), fp)
                End If

            End With

            CalculaTotales()
        End If
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Me.BindingContext(ds, nTabla).Position = 0
        AsignaMov(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Me.BindingContext(ds, nTabla).Position -= 1
        AsignaMov(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Me.BindingContext(ds, nTabla).Position += 1
        AsignaMov(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
        AsignaMov(Me.BindingContext(ds, nTabla).Position, False)
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
       dg.RowHeaderMouseClick, dg.CellMouseClick

        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        AsignaMov(e.RowIndex, False)

    End Sub

End Class