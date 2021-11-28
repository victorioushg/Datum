Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Imports fTransport.Models
Imports dgFieldSF = fTransport.Models.SfDataGridField

Public Class jsComArcProveedoresCXP
    Private Const sModulo As String = "Proveedores y CxP - Movimientos CxP"

    Private Const nTablaBancosExt As String = "bancosext"
    Private Const nTablaTarjetas As String = "tarjetas"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As DataTable

    Private dtBancosExt As DataTable
    Private dtTarjetas As DataTable
    Private ft As New Transportables

    Private listaTipoCxP As New List(Of TextoValor)
    Private listaCajas As New List(Of Saving)
    Private listaMonedas As New List(Of CambioMonedaPlus)
    Private tipoCxP As TextoValor
    Private causaCredito As CreditCause

    Private strSQLBancosExt As String = " select codigo, descrip, descrip descripcion from jsconctatab where modulo = '00010' and id_emp = '" & jytsistema.WorkID & "' "
    Private strSQLTarjetas As String = " select codtar codigo, nomtar descripcion from jsconctatar where id_emp = '" & jytsistema.WorkID & "' "

    Private i_modo As Integer
    Private nPosicion As Long
    Private CodigoProveedor As String
    Private TipoProveedor As Integer

    Private aContador() As String = {"comnummov", "comnummov", "comnumcdb", "comnumcan", "comnumcan", "comnumccr", "", "comnumretiva", "comnumretislr"}
    Private aContadorModulo() As String = {"03", "03", "12", "10", "10", "11", "", "14", "15"}
    Private aCajas() As String
    Private n_Apuntador As Long

    Private iSel As Integer = 0
    Private dSel As Double = 0.0
    Private numTipoMovimiento As Integer = 0
    Private strDocs As String = ""
    Private strDocsIni As String = ""
    Private MontoPositivo As Double = 0.0
    Private MontoNegativo As Double = 0.0
    Private ComprobanteNumero As String = ""
    Private nTipoMovimientoCXP As Integer = 0 '0 = Debitos ; 1 = Creditos; 2 = Retenciones IVA; 3 = Retenciones ISLR
    Private strDocsRetIVA As String = " nummov = 'XX XX' OR "
    Private TotalRetencionIVA As Double = 0.0
    Private Remesa As String = ""

    Public Property Apuntador() As Long
    Public Property Comprobante() As String
    Public Property TipoMovimientoCXP() As Integer

    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal Codigo As String,
                       ByVal ProveedorTipo As Integer, Optional CxP_ExP As Integer = 0)
        i_modo = movimiento.iAgregar
        myConn = MyCon
        ds = dsMov
        dt = dtMov
        CodigoProveedor = Codigo
        TipoProveedor = ProveedorTipo
        If CxP_ExP = 1 Then Remesa = "1"
        If dt.Rows.Count = 0 Then Apuntador = -1
        listaTipoCxP = InitiateDropDown(Of TextoValor)(myConn, cmbTipo, Tipo.TipoMovimientoCxP)
        cmbTipo.SelectedIndex = 0
        Me.ShowDialog()

    End Sub
    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal Codigo As String,
                       ByVal ProveedorTipo As Integer)
        i_modo = movimiento.iEditar
        myConn = MyCon
        ds = dsMov
        dt = dtMov
        CodigoProveedor = Codigo
        TipoProveedor = ProveedorTipo
        listaTipoCxP = InitiateDropDown(Of TextoValor)(myConn, cmbTipo, Tipo.TipoMovimientoCxP)
        cmbTipo.SelectedIndex = MovimientoCxP()
        cmbTipo.Enabled = False
        Me.ShowDialog()

    End Sub
    Private Function MovimientoCxP() As Integer
        Dim tipomov As String = dt.Rows(Apuntador).Item("tipomov")
        If tipomov = "NC" Then
            If Mid(dt.Rows(Apuntador).Item("REFER"), 1, 5) = "ISLR-" Then
                Return listaTipoCxP.FirstOrDefault(Function(e) e.Text = "Retención ISLR").Index
            ElseIf Mid(dt.Rows(Apuntador).Item("CONCEPTO"), 1, 13) = "RETENCION IVA" Then
                Return listaTipoCxP.FirstOrDefault(Function(e) e.Text = "Retención IVA").Index
            Else
                Return listaTipoCxP.FirstOrDefault(Function(e) e.Text = "Nota Crédito").Index
            End If
        Else
            Return listaTipoCxP.FirstOrDefault(Function(e) e.Value = tipomov).Index
        End If
    End Function

    Private Sub jsComArcProveedoresCXP_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsComArcProveedoresCXP_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        IniciarControles()
    End Sub
    Private Sub IniciarControles()

        Dim dates As SfDateTimeEdit() = {txtEmisionCR, txtEmisionDB, txtFechaRetISLR, txtFechaRetIVA, txtVenceDB}
        SetSizeDateObjects(dates)

        InitiateDropDown(Of AccountBase)(myConn, cmbCCDebitos)
        InitiateDropDown(Of AccountBase)(myConn, cmbCCISLR)
        InitiateDropDown(Of AccountBase)(myConn, cmbCCIVA)
        InitiateDropDown(Of AccountBase)(myConn, cmbCC)

        InitiateDropDown(Of CreditCause)(myConn, cmbCausaNC,,, "CXP")
        InitiateDropDown(Of Saving)(myConn, cmbCaja, , 0)
        InitiateDropDown(Of TextoValor)(myConn, cmbFPCR, Tipo.FormaDePago, 0)
        listaMonedas = InitiateDropDownInterchangeCurrency(myConn, cmbMonedas, jytsistema.sFechadeTrabajo, True, "CodigoIso")

        InitiateDropDown(Of BankAccount)(myConn, cmbNombrePago)

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub cmbTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTipo.SelectedIndexChanged

        tipoCxP = cmbTipo.SelectedItem

        Select Case cmbTipo.SelectedIndex
            Case 0, 1, 2
                ft.visualizarObjetos(True, grpDebitos)
                ft.visualizarObjetos(False, grpCreditos, grpRetencionIVA, grpRetencionISLR)
                If i_modo = movimiento.iAgregar Then
                    IniciarDebitos(cmbTipo.SelectedIndex)
                Else
                    AsignarDebitos(Apuntador)
                End If
                nTipoMovimientoCXP = 0
            Case 3, 4, 5
                ft.visualizarObjetos(True, grpCreditos)
                ft.visualizarObjetos(False, grpDebitos, grpRetencionISLR, grpRetencionIVA)
                IniciarCreditos(cmbTipo.SelectedIndex)
                nTipoMovimientoCXP = 1
            Case 7
                ft.visualizarObjetos(True, grpRetencionIVA)
                ft.visualizarObjetos(False, grpDebitos, grpCreditos, grpRetencionISLR)
                IniciarRetencionIVA()
                nTipoMovimientoCXP = 2
                strDocsRetIVA = " nummov = 'XX XX' OR "
                TotalRetencionIVA = 0.0
            Case 8
                ft.visualizarObjetos(True, grpRetencionISLR)
                ft.visualizarObjetos(False, grpDebitos, grpCreditos, grpRetencionIVA)
                IniciarRetencionISLR()
                nTipoMovimientoCXP = 3
        End Select
    End Sub
    Private Sub IniciarCreditos(ByVal Tipo As Integer)

        ResetearVariablesDEUsoComun()
        ft.habilitarObjetos(False, True, txtDocumentoCR, txtDocSel, txtSaldoSel, txtNumPagCR, cmbNombrePago,
                            txtDocSel, txtSaldoSel)

        If Tipo = 3 Then
            strDocsIni = "ABONO "
        ElseIf Tipo = 4 Then
            strDocsIni = "CANCELACION "
        Else
            strDocsIni = "NOTA CREDITO "
        End If

        txtDocumentoCR.Text = "TMP" & ft.NumeroAleatorio(1000000)
        txtEmisionCR.Value = jytsistema.sFechadeTrabajo
        txtReferenciaCR.Text = ""
        txtConceptoCR.Text = strDocs
        txtSaldoSel.Text = ft.FormatoNumero(0.0)
        txtImporteCR.Text = ft.FormatoNumero(0.0)

        IniciarSaldos()

        optCO.Checked = True
        If Tipo <> 5 Then
            ft.habilitarObjetos(False, False, grpCRCO)
            ft.visualizarObjetos(False, lblNotaCredito, cmbCausaNC)
        Else
            ft.habilitarObjetos(False, False, grpCRCO, optCO, optCR)
            ft.visualizarObjetos(True, lblNotaCredito, cmbCausaNC)
        End If

    End Sub
    Private Sub ResetearVariablesDEUsoComun()
        iSel = 0
        dSel = 0
        MontoPositivo = 0
        MontoNegativo = 0
    End Sub

    Private Sub IniciarRetencionIVA()

        ft.habilitarObjetos(False, True, txtSaldoRetIVA, txtRetIVA)
        txtSaldoRetIVA.Text = ft.FormatoNumero(0.0)
        txtFechaRetIVA.Value = jytsistema.sFechadeTrabajo
        txtPorRetIVA.Text = ft.FormatoNumero(75.0)
        txtRetIVA.Text = ft.FormatoNumero(CalculaRetencionIVA())

        IniciarSaldosIVA()

        lblLeyendaIVA.Text = "Si el documento al cual desea hacer retención del IVA no aparece : " & vbLf _
            & " 1. El Documento no posee número de control de IVA " & vbLf _
            & " 2. El Documento no posee renglones con tasa de impuesto mayor a CERO (0) " & vbLf _
            & " 3. El Documento YA posee una Retención Asignada." & vbLf _
            & " 4. El documento tiene saldo cero (0) (YA fué cancelado) "

    End Sub
    Private Function CalculaRetencionIVA() As Double
        Return Math.Round(ValorNumero(txtPorRetIVA.Text) / 100 * ValorNumero(txtSaldoRetIVA.Text), 2)
    End Function
    Private Sub IniciarRetencionISLR()

        ResetearVariablesDEUsoComun()
        ft.habilitarObjetos(False, True, txtDocsSelRetISLR, txtSaldoSelRetISLR, txtPorcentajeMontoBaseRetISLR,
                        txtMontoBaseRetISLR, txtMinimoRetISLR,
                        txtAcumuladoSinRetISLR, txtSustraendoRetISLR, txtMontoRetencionISLR)

        IniciarSaldosISLR()

        txtDocsSelRetISLR.Text = 0
        txtSaldoSelRetISLR.Text = ft.FormatoNumero(0.0)

        txtFechaRetISLR.Value = jytsistema.sFechadeTrabajo

        Dim dtRetISLR As DataTable
        Dim nTablaRetISLR As String = "tblRetIVA"
        ds = DataSetRequery(ds, " select codret, concat(codret,'-', elt( tipo + 1, 'PNR  ', 'PNNR ', 'PJD  ', 'PJND ', 'PJNCD'), '-' ,concepto) retencion from jscontabret where id_emp = '" & jytsistema.WorkID & "' order by codret ", myConn, nTablaRetISLR, lblInfo)
        dtRetISLR = ds.Tables(nTablaRetISLR)
        RellenaComboConDatatable(cmbConceptoRetISLR, dtRetISLR, "retencion", "codret")

        txtPorcentajeMontoBaseRetISLR.Text = ft.FormatoNumero(0.0)
        txtMontoBaseRetISLR.Text = ft.FormatoNumero(0.0)
        txtMinimoRetISLR.Text = ft.FormatoNumero(0.0)
        txtAcumuladoSinRetISLR.Text = ft.FormatoNumero(0.0)
        txtPorcentajeRetISLR.Text = ft.FormatoNumero(0.0)
        txtSustraendoRetISLR.Text = ft.FormatoNumero(0.0)
        txtMontoRetencionISLR.Text = ft.FormatoNumero(0.0)


    End Sub

    Private Sub IniciarSaldos()

        Dim listaSaldos As New List(Of VendorTransaction)
        listaSaldos = GetVendorBalance(myConn, CodigoProveedor, Remesa)

        For Each item As VendorTransaction In listaSaldos
            Dim mon = listaMonedas.FirstOrDefault(Function(elem) elem.Moneda = item.Currency)
            Dim equivale As Integer = 1
            If Not mon Is Nothing Then equivale = mon.Equivale
            item.SaldoReal = item.Saldo / equivale
        Next

        Dim aCampos As List(Of dgFieldSF) = New List(Of dgFieldSF)() From {
                New dgFieldSF(TypeColumn.TextColumn, "NumeroMovimiento", "Nº Documento", 150, HorizontalAlignment.Left, ""),
                New dgFieldSF(TypeColumn.TextColumn, "TipoMovimiento", "TP", 120, HorizontalAlignment.Center, ""),
                New dgFieldSF(TypeColumn.TextColumn, "Referencia", "Referencia", 180, HorizontalAlignment.Left, ""),
                New dgFieldSF(TypeColumn.DateTimeColumn, "Emision", "Emision", 180, HorizontalAlignment.Center, FormatoFecha.Corta),
                New dgFieldSF(TypeColumn.DateTimeColumn, "Vencimiento", "Vencimiento", 180, HorizontalAlignment.Center, FormatoFecha.Corta),
                New dgFieldSF(TypeColumn.NumericColumn, "Importe", "Importe Inicial", 120, HorizontalAlignment.Right, FormatoNumero.FormatoNumero),
                New dgFieldSF(TypeColumn.NumericColumn, "Saldo", "Saldo Documento", 180, HorizontalAlignment.Right, FormatoNumero.FormatoNumero),
                New dgFieldSF(TypeColumn.NumericColumn, "SaldoReal", "Saldo(" + jytsistema.WorkCurrency.CodigoISO + ")", 180, HorizontalAlignment.Right, FormatoNumero.FormatoNumero)
            }

        ft.IniciarDataGridWithList(Of VendorTransaction)(dg, listaSaldos, aCampos,,,,,,, False, , False)

    End Sub
    Private Sub IniciarSaldosISLR()

        Dim listaSaldos As New List(Of VendorTransaction)
        listaSaldos = GetVendorBalanceISLR(myConn, CodigoProveedor, Remesa)
        Dim aCampos As List(Of dgFieldSF) = New List(Of dgFieldSF)() From {
                New dgFieldSF(TypeColumn.TextColumn, "NumeroMovimiento", "Nº Documento", 150, HorizontalAlignment.Left, ""),
                New dgFieldSF(TypeColumn.TextColumn, "TipoMovimiento", "TP", 120, HorizontalAlignment.Center, ""),
                New dgFieldSF(TypeColumn.TextColumn, "NumeroControl", "Numero Control", 180, HorizontalAlignment.Left, ""),
                New dgFieldSF(TypeColumn.DateTimeColumn, "Emision", "Emision", 180, HorizontalAlignment.Center, FormatoFecha.Corta),
                New dgFieldSF(TypeColumn.DateTimeColumn, "Vencimiento", "Vencimiento", 180, HorizontalAlignment.Center, FormatoFecha.Corta),
                New dgFieldSF(TypeColumn.NumericColumn, "Importe", "Importe Inicial", 120, HorizontalAlignment.Right, FormatoNumero.FormatoNumero),
                New dgFieldSF(TypeColumn.NumericColumn, "Saldo", "Saldo Documento", 180, HorizontalAlignment.Right, FormatoNumero.FormatoNumero)
            }

        ft.IniciarDataGridWithList(Of VendorTransaction)(dgISLR, listaSaldos, aCampos,,,,,,, False, , False)

    End Sub
    Private Sub IniciarSaldosIVA()

        Dim listaSaldos As New List(Of VendorTransaction)
        listaSaldos = GetVendorBalanceIVA(myConn, CodigoProveedor, Remesa)

        Dim aCampos As List(Of dgFieldSF) = New List(Of dgFieldSF)() From {
                New dgFieldSF(TypeColumn.TextColumn, "NumeroMovimiento", "Nº Documento", 150, HorizontalAlignment.Left, ""),
                New dgFieldSF(TypeColumn.TextColumn, "TipoMovimiento", "TP", 120, HorizontalAlignment.Center, ""),
                New dgFieldSF(TypeColumn.TextColumn, "NumeroControl", "Numero Control", 180, HorizontalAlignment.Left, ""),
                New dgFieldSF(TypeColumn.DateTimeColumn, "Emision", "Emision", 180, HorizontalAlignment.Center, FormatoFecha.Corta),
                New dgFieldSF(TypeColumn.DateTimeColumn, "Vencimiento", "Vencimiento", 180, HorizontalAlignment.Center, FormatoFecha.Corta),
                New dgFieldSF(TypeColumn.NumericColumn, "Importe", "Importe Inicial", 120, HorizontalAlignment.Right, FormatoNumero.FormatoNumero),
                New dgFieldSF(TypeColumn.NumericColumn, "ImporteIVA", "Importe IVA", 180, HorizontalAlignment.Right, FormatoNumero.FormatoNumero)
            }

        ft.IniciarDataGridWithList(Of VendorTransaction)(dgISLR, listaSaldos, aCampos,,,,,,, False, , False)

    End Sub
    Private Sub IniciarDebitos(ByVal Tipo As Integer)

        ft.habilitarObjetos(False, True, txtDocumentoDB)

        txtDocumentoDB.Text = "TMP" & ft.NumeroAleatorio(1000000)
        txtEmisionDB.Value = jytsistema.sFechadeTrabajo
        txtVenceDB.Value = jytsistema.sFechadeTrabajo
        txtReferDB.Text = ""
        txtConceptoDB.Text = ""
        txtImporteDB.Text = ft.FormatoNumero(0.0)

    End Sub
    Private Sub AsignarDebitos(ByVal Puntero As Long)

        ft.habilitarObjetos(False, True, txtDocumentoDB, txtReferDB, txtConceptoDB, txtImporteDB)

        With dt.Rows(Puntero)
            txtDocumentoDB.Text = .Item("nummov")
            txtEmisionDB.Value = .Item("emision")
            txtVenceDB.Value = .Item("vence")
            txtReferDB.Text = ft.muestraCampoTexto(.Item("refer"))
            txtConceptoDB.Text = ft.muestraCampoTexto(.Item("concepto"))
            txtImporteDB.Text = ft.FormatoNumero(.Item("importe"))
        End With


    End Sub

    Private Sub ValidarDebitos(ByVal Tipo As Integer)

        Dim aAdicionales() As String = {"tipomov IN ('FC','GR','ND') AND "}
        If FechaUltimoBloqueo(myConn, "jsprotrapag", aAdicionales) >= txtEmisionDB.Value Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Exit Sub
        End If

        If txtDocumentoDB.Text = "" Then
            ft.mensajeCritico("DEBE INDICAR UN Nº DE DOCUMENTO VALIDO...")
            Exit Sub
        Else
            Dim aCampos() As String = {"codpro", "tipomov", "nummov", "remesa", "id_emp"}
            Dim aStrings() As String = {CodigoProveedor, cmbTipo.SelectedValue, txtDocumentoDB.Text, Remesa, jytsistema.WorkID}
            If i_modo = movimiento.iAgregar AndAlso qFound(myConn, lblInfo, "jsprotrapag", aCampos, aStrings) Then
                ft.mensajeCritico("Documento YA se encuentra en movimientos de este proveedor ...")
                Exit Sub
            End If
        End If

        If txtEmisionDB.Value > txtVenceDB.Value Then
            ft.mensajeCritico("Fecha emisión mayor que fecha de vencimiento...")
            Exit Sub
        End If

        If Not ft.isNumeric(txtImporteDB.Text) Then
            ft.mensajeCritico("Debe indicar un número válido para el importe...")
            Exit Sub
        End If

        GuardarDebito(Tipo)

        If Remesa = "" Then
            SaldoCxP(myConn, lblInfo, CodigoProveedor)
        Else
            SaldoExP(myConn, lblInfo, CodigoProveedor)
        End If

        Me.Close()

    End Sub
    Private Sub GuardarDebito(ByVal Tipo As Integer)

        Dim Inserta As Boolean = False
        Apuntador = Me.BindingContext(ds, dt.TableName).Position
        Dim Documento As String = txtDocumentoDB.Text

        If i_modo = movimiento.iAgregar Then
            Inserta = True
            Apuntador = dt.Rows.Count
            Documento = Contador(myConn, lblInfo, Gestion.iCompras, aContador(Tipo), aContadorModulo(Tipo))
        End If

        InsertEditCOMPRASCXP(myConn, lblInfo, Inserta, CodigoProveedor,
             cmbTipo.SelectedValue, Documento, txtEmisionDB.Value, ft.FormatoHora(Now()), txtVenceDB.Value,
             txtReferDB.Text, txtConceptoDB.Text, -1 * Math.Abs(ValorNumero(txtImporteDB.Text)), 0.0, "", "", "", "",
             "CXP", "", "", "", "", Documento, "0", "", jytsistema.sFechadeTrabajo, cmbCCDebitos.SelectedValue, "0",
             "", 0.0, 0.0, "", "", "", Remesa, "", "", TipoProveedor, FOTipo.Debito, "0",
             jytsistema.WorkCurrency.Id, jytsistema.sFechadeTrabajo)

        ComprobanteNumero = Documento

        InsertarAuditoria(myConn, IIf(Inserta, MovAud.iIncluir, MovAud.imodificar), sModulo, Documento)

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Select Case cmbTipo.SelectedIndex
            Case 0, 1, 2
                ValidarDebitos(cmbTipo.SelectedIndex)
            Case 3, 4, 5
                ValidarCreditos(cmbTipo.SelectedIndex)
            Case 7
                ValidarRetencionIVA()
            Case 8
                ValidarRetencionISLR()
        End Select
    End Sub
    Private Sub ValidarRetencionISLR()

        Dim aAdicionales() As String = {"tipomov = 'NC' AND ", "SUBSTRING(refer,1,5) = 'ISLR-' AND "}
        If FechaUltimoBloqueo(myConn, "jsprotrapag", aAdicionales) >= txtFechaRetISLR.Value Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Exit Sub
        End If

        If ValorNumero(txtDocsSelRetISLR.Text) = 0 Then
            ft.mensajeCritico("Debe seleccionar al menos un documento...")
            Exit Sub
        End If

        If ValorNumero(txtMontoRetencionISLR.Text) = 0.0 Then
            ft.mensajeCritico("Debe indicar un porcentaje de retención válido...")
            Exit Sub
        End If

        GuardarRetencionISLR()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        sRespuesta = MsgBox(" ¿ Desea imprimir Comprobante de Retención ?", MsgBoxStyle.YesNo, "Imprimir ... ")
        If sRespuesta = MsgBoxResult.Yes Then
            Dim f As New jsComRepParametros
            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cRetencionISLR, "Comprobante de Retención de ISLR", CodigoProveedor, ComprobanteNumero)
            f = Nothing
        End If
        SaldoCxP(myConn, lblInfo, CodigoProveedor)
        Me.Close()

    End Sub
    Private Sub ValidarRetencionIVA()

        Dim aAdicionales() As String = {"tipomov = 'NC' AND ", "SUBSTRING(concepto,1,13) = 'RETENCION IVA' AND "}
        If FechaUltimoBloqueo(myConn, "jsprotrapag", aAdicionales) >= txtFechaRetIVA.Value Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Exit Sub
        End If

        If ValorNumero(txtSaldoRetIVA.Text) = 0 Then
            ft.mensajeCritico("Debe seleccionar al menos un documento...")
            Exit Sub
        End If

        If Not (ValorNumero(txtPorRetIVA.Text) = 75 Or ValorNumero(txtPorRetIVA.Text) = 100) Then
            ft.mensajeCritico("Debe indicar un porcentaje de retención válido...")
            Exit Sub
        End If

        GuardarRetencionIVA()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        sRespuesta = MsgBox(" ¿ Desea imprimir Comprobante de Retención ?", MsgBoxStyle.YesNo, "Imprimir ... ")
        If sRespuesta = MsgBoxResult.Yes Then
            Dim f As New jsComRepParametros
            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cRetencionIVA, "Comprobante de Retención de IVA", CodigoProveedor, ComprobanteNumero)
            f = Nothing
        End If

        If Remesa = "" Then
            SaldoCxP(myConn, lblInfo, CodigoProveedor)
        Else
            SaldoExP(myConn, lblInfo, CodigoProveedor)
        End If
        Me.Close()

    End Sub
    Private Sub GuardarRetencionISLR()

        Dim Inserta As Boolean = False
        Apuntador = Me.BindingContext(ds, dt.TableName).Position
        Dim Documento As String = txtDocumentoDB.Text

        If i_modo = movimiento.iAgregar Then
            Inserta = True
            Apuntador = dt.Rows.Count
            Documento = "ISLR-" & Contador(myConn, lblInfo, Gestion.iCompras, aContador(8), aContadorModulo(8))
        End If

        Dim gCont As Integer
        'For gCont = 0 To lvRetISLR.Items.Count - 1
        '    With lvRetISLR.Items.Item(gCont)

        '        If .Checked Then

        '            InsertEditCOMPRASCXP(myConn, lblInfo, Inserta, CodigoProveedor,
        '                 "NC", .Text, txtFechaRetISLR.Value, ft.FormatoHora(Now()), txtFechaRetISLR.Value,
        '                  Documento, cmbConceptoRetISLR.Text, ValorNumero(txtMontoRetencionISLR.Text), 0.0, "", "", "", "",
        '                 "CXP", "", "", "", "", Documento, "0", "", jytsistema.sFechadeTrabajo, txtCodigoContableISLR.Text, "0",
        '                 "", 0.0, 0.0, "", "", "", Remesa, "", "", TipoProveedor, FOTipo.Credito, "0",
        '                  jytsistema.WorkCurrency.Id, jytsistema.sFechadeTrabajo)

        '            ft.Ejecutar_strSQL(myConn, " update jsproenccom set " _
        '                            & " ret_islr = " & ValorNumero(txtMontoRetencionISLR.Text) & ", " _
        '                            & " num_ret_islr = '" & Documento & "', " _
        '                            & " fecha_ret_islr = '" & ft.FormatoFechaMySQL(txtFechaRetISLR.Value) & "',  " _
        '                            & " base_ret_islr = " & ValorNumero(txtMontoBaseRetISLR.Text) & ", " _
        '                            & " por_ret_islr = " & ValorNumero(txtPorcentajeRetISLR.Text) & " " _
        '                            & " where " _
        '                            & " numcom = '" & .Text & "' and " _
        '                            & " codpro = '" & CodigoProveedor & "' and " _
        '                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
        '                            & " id_emp = '" & jytsistema.WorkID & "' ")

        '            ft.Ejecutar_strSQL(myConn, " update jsproencgas set " _
        '                            & " ret_islr = " & ValorNumero(txtMontoRetencionISLR.Text) & ", " _
        '                            & " num_ret_islr = '" & Documento & "', " _
        '                            & " fecha_ret_islr = '" & ft.FormatoFechaMySQL(txtFechaRetISLR.Value) & "',  " _
        '                            & " base_ret_islr = " & ValorNumero(txtMontoBaseRetISLR.Text) & ", " _
        '                            & " por_ret_islr = " & ValorNumero(txtPorcentajeRetISLR.Text) & " " _
        '                            & " where " _
        '                            & " numgas = '" & .Text & "' and " _
        '                            & " codpro = '" & CodigoProveedor & "' and " _
        '                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
        '                            & " id_emp = '" & jytsistema.WorkID & "' ")
        '        End If

        '    End With
        'Next

        ComprobanteNumero = Documento

        InsertarAuditoria(myConn, IIf(Inserta, MovAud.iIncluir, MovAud.imodificar), sModulo, Documento)

    End Sub
    Private Sub GuardarRetencionIVA()

        Dim Inserta As Boolean = False
        Apuntador = Me.BindingContext(ds, dt.TableName).Position
        Dim Documento As String = txtDocumentoDB.Text

        If i_modo = movimiento.iAgregar Then
            Inserta = True
            Apuntador = dt.Rows.Count
            Documento = Format(txtFechaRetIVA.Value, "yyyyMM") & Contador(myConn, lblInfo, Gestion.iCompras, aContador(7), aContadorModulo(7))
        End If

        Dim gCont As Integer
        'For gCont = 0 To lvRetIVA.Items.Count - 1
        '    With lvRetIVA.Items.Item(gCont)
        '        If .Checked Then

        '            Dim CodigoDocumento As String = .Text

        '            InsertEditCOMPRASCXP(myConn, lblInfo, Inserta, CodigoProveedor,
        '                 IIf(.SubItems(1).Text = "NC", "ND", "NC"), .Text, txtFechaRetIVA.Value, ft.FormatoHora(Now()), txtFechaRetIVA.Value,
        '                 Documento, "RETENCION IVA S/COMPROBANTE Nº " & Documento, IIf(.SubItems(1).Text = "NC", -1, 1) * Math.Round(ValorNumero(.SubItems(6).Text) * ValorNumero(txtPorRetIVA.Text) / 100, 2), 0.0, "", "", "", "",
        '                 "CXP", "", "", "", "", Documento, "0", "", jytsistema.sFechadeTrabajo, txtCodigoContableIVA.Text, "0",
        '                 "", 0.0, 0.0, "", "", "", Remesa, "", "", TipoProveedor, IIf(.SubItems(1).Text = "NC", FOTipo.Debito, FOTipo.Credito), "0",
        '                  jytsistema.WorkCurrency.Id, jytsistema.sFechadeTrabajo)

        '            ft.Ejecutar_strSQL(myConn, " update jsproenccom set " _
        '                            & " ret_iva = " & Math.Round(ValorNumero(.SubItems(6).Text) * ValorNumero(txtPorRetIVA.Text) / 100, 2) & ", " _
        '                            & " num_ret_iva = '" & Documento & "', fecha_ret_iva = '" & ft.FormatoFechaMySQL(txtFechaRetIVA.Value) & "' " _
        '                            & " where " _
        '                            & " numcom = '" & CodigoDocumento & "' and " _
        '                            & " codpro = '" & CodigoProveedor & "' and " _
        '                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
        '                            & " id_emp = '" & jytsistema.WorkID & "' ")

        '            ft.Ejecutar_strSQL(myConn, " update jsproivacom set " _
        '                            & " retencion = round(impiva*" & ValorNumero(txtPorRetIVA.Text) / 100 & ", 2), " _
        '                            & " numretencion = '" & Documento & "' " _
        '                            & " where " _
        '                            & " numcom = '" & .Text & "' and " _
        '                            & " codpro = '" & CodigoProveedor & "' and " _
        '                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
        '                            & " id_emp = '" & jytsistema.WorkID & "' ")

        '            ft.Ejecutar_strSQL(myConn, " update jsproencgas set " _
        '                            & " ret_iva = " & Math.Round(ValorNumero(.SubItems(6).Text) * ValorNumero(txtPorRetIVA.Text) / 100, 2) & ", " _
        '                            & " num_ret_iva = '" & Documento & "', fecha_ret_iva = '" & ft.FormatoFechaMySQL(txtFechaRetIVA.Value) & "' " _
        '                            & " where " _
        '                            & " numgas = '" & .Text & "' and " _
        '                            & " codpro = '" & CodigoProveedor & "' and " _
        '                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
        '                            & " id_emp = '" & jytsistema.WorkID & "' ")

        '            ft.Ejecutar_strSQL(myConn, " update jsproivagas set " _
        '                            & " retencion = round(impiva*" & ValorNumero(txtPorRetIVA.Text) / 100 & ", 2), " _
        '                            & " numretencion = '" & Documento & "' " _
        '                            & " where " _
        '                            & " numgas = '" & .Text & "' and " _
        '                            & " codpro = '" & CodigoProveedor & "' and " _
        '                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
        '                            & " id_emp = '" & jytsistema.WorkID & "' ")

        '            ft.Ejecutar_strSQL(myConn, " update jsproivancr set " _
        '                           & " retencion = -1*round(impiva*" & ValorNumero(txtPorRetIVA.Text) / 100 & ", 2), " _
        '                           & " numretencion = '" & Documento & "' " _
        '                           & " where " _
        '                           & " numncr = '" & .Text & "' and " _
        '                           & " codpro = '" & CodigoProveedor & "' and " _
        '                           & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
        '                           & " id_emp = '" & jytsistema.WorkID & "' ")

        '            ft.Ejecutar_strSQL(myConn, " update jsproivandb set " _
        '                           & " retencion = round(impiva*" & ValorNumero(txtPorRetIVA.Text) / 100 & ", 2), " _
        '                           & " numretencion = '" & Documento & "' " _
        '                           & " where " _
        '                           & " numndb = '" & .Text & "' and " _
        '                           & " codpro = '" & CodigoProveedor & "' and " _
        '                           & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
        '                           & " id_emp = '" & jytsistema.WorkID & "' ")

        '        End If
        '    End With
        'Next

        ComprobanteNumero = Documento
        InsertarAuditoria(myConn, IIf(Inserta, MovAud.iIncluir, MovAud.imodificar), sModulo, Documento)

    End Sub

    Private Sub ValidarCreditos(ByVal Tipo As Integer)

        Dim aAdicionales() As String = {"tipomov IN ('AB','CA','NC') AND ",
                                        "SUBSTRING(concepto,1,13) <> 'RETENCION IVA' AND ",
                                        "SUBSTRING(refer,1,5) <> 'ISLR-' AND "}

        If FechaUltimoBloqueo(myConn, "jsprotrapag", aAdicionales) >= txtEmisionCR.Value Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Exit Sub
        End If

        If ValorEntero(txtDocSel.Text) = 0 And Tipo < 5 Then
            ft.mensajeCritico("Debe seleccionar al menos un documento...")
            Exit Sub
        End If

        If Trim(txtDocumentoCR.Text) = "" Then
            ft.mensajeCritico("Debe indicar un número de documento (comprobante) válido...")
            Exit Sub
        End If

        If Tipo = 5 Then
            If cmbCausaNC.SelectedValue = "" Then
                ft.mensajeCritico("Debe indicar una CAUSA DE CREDITO Válida...")
                Exit Sub
            Else
                If CBool(causaCredito.ValidaDocumentos) Then
                    If ValorEntero(txtDocSel.Text) = 0 Then
                        ft.mensajeCritico("Debe seleccionar al menos un documento...")
                        Exit Sub
                    End If
                Else
                    If ValorEntero(txtDocSel.Text) <> 0 Then
                        ft.mensajeCritico("NO DEBE seleccionar documentos para esta causa de NOTA CREDITO...")
                        Exit Sub
                    End If
                End If
            End If
        End If


        If ft.isNumeric(txtImporteCR.Text) Then
            If ValorNumero(txtImporteCR.Text) > 0 Then
                ft.mensajeCritico("Debe indicar un importe MENOR O IGUAL a cero (0)")
                Exit Sub
            Else

                Select Case cmbTipo.SelectedIndex
                    Case 3, 4 'ABONOS, CANCELACIONES
                        'If ValorNumero(txtImporteCR.Text) < ValorNumero(txtSaldoSel.Text) Then
                        '    ft.MensajeCritico( "EL MONTO A CANCELAR NO DEBE EXCEDER AL MONTO TOTAL DE DOCUMENTOS SELECCIONADOS ... ")
                        '    Exit Sub
                        'End If
                    Case 5 'NOTAS CREDITO
                        If ValorNumero(txtImporteCR.Text) = 0 Then
                            ft.mensajeCritico("Debe indicar un importe MENOR QUE CERO")
                            Exit Sub
                        End If
                End Select

            End If
        Else
            ft.mensajeCritico("Debe indicar un importe válido...")
            Exit Sub
        End If

        If txtNumPagCR.Text.Trim = "" AndAlso cmbFPCR.SelectedIndex >= 1 Then
            ft.mensajeCritico("Debe indicar un número de pago válido...")
            Exit Sub
        End If

        If cmbNombrePago.SelectedValue = "" AndAlso cmbFPCR.SelectedIndex >= 1 Then
            ft.mensajeCritico("Debe indicar un nombre de pago válido ...")
            Exit Sub
        End If


        If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPARAM05")) Then
            If cmbCC.SelectedValue = "" Then
                ft.mensajeCritico("Debe indicar CODIGO CONTABLE ...")
                Exit Sub
            End If
        End If

        GuardarCreditos(Tipo)


        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        sRespuesta = MsgBox(" ¿ Desea imprimir Comprobante de Egreso ?", MsgBoxStyle.YesNo, "Imprimir ... ")
        If sRespuesta = MsgBoxResult.Yes Then

            Select Case cmbFPCR.SelectedIndex
                Case 0, 2, 3, 4, 5 'EF
                    Dim f As New jsComRepParametros
                    f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cComprobantePago, "COMPROBANTE DE EGRESOS", CodigoProveedor, ComprobanteNumero)
                    f.Dispose()
                    f = Nothing
                Case 1 'CH
                    Dim fr As New jsBanRepParametros
                    fr.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cComprobanteDeEgreso, "COMPROBANTE DE PAGO", cmbNombrePago.SelectedValue, ComprobanteNumero, , "CXP")
                    fr.Dispose()
                    fr = Nothing
            End Select

        End If
        SaldoCxP(myConn, lblInfo, CodigoProveedor)
        Me.Close()

    End Sub

    Private Sub GuardarCreditos(ByVal Tipo As Integer)
        Dim Inserta As Boolean = False
        Dim jCont As Integer
        Dim Resto As Double = ValorNumero(txtImporteCR.Text)
        Dim Documento As String = txtDocumentoCR.Text
        Dim NombreDePago As String = ""


        If i_modo = movimiento.iAgregar Then
            Inserta = True
            Apuntador = dt.Rows.Count
            Documento = Contador(myConn, lblInfo, Gestion.iCompras, aContador(Tipo), aContadorModulo(Tipo))
        End If

        If optCO.Checked Then
            Select Case cmbFPCR.SelectedIndex
                Case 0, 3 'EF, CT
                    NombreDePago = txtNombrePago.Text
                    InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, cmbCaja.SelectedValue, UltimoCajaMasUno(myConn, lblInfo, cmbCaja.SelectedValue),
                        txtEmisionCR.Value, "CXP", "SA", Documento, cmbFPCR.SelectedValue,
                        txtNumPagCR.Text, NombreDePago, ValorNumero(txtImporteCR.Text), cmbCC.SelectedValue, txtConceptoCR.Text, "", jytsistema.sFechadeTrabajo, 1,
                        "", "", "", jytsistema.sFechadeTrabajo, CodigoProveedor, "", "1", jytsistema.WorkCurrency.Id, DateTime.Now())
                Case 1, 2, 4, 5 'CH, TA, DP, TR
                    NombreDePago = cmbNombrePago.SelectedValue

                    InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, txtEmisionCR.Value, txtNumPagCR.Text,
                        IIf(cmbFPCR.SelectedIndex <> 1, "ND", "CH"), NombreDePago, "", txtConceptoCR.Text, ValorNumero(txtImporteCR.Text),
                        "CXP", Documento, txtBeneficiario.Text, Documento, "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo,
                         cmbTipo.SelectedValue, "", jytsistema.sFechadeTrabajo, "0", CodigoProveedor, "", jytsistema.WorkCurrency.Id, DateTime.Now())

                    If cmbFPCR.SelectedIndex = 1 Or cmbFPCR.SelectedIndex = 5 Then _
                        IncluirImpuestoDebitoBancario(myConn, lblInfo, IIf(cmbFPCR.SelectedIndex <> 1, "ND", "CH"), NombreDePago, txtNumPagCR.Text, txtEmisionCR.Value, ValorNumero(txtImporteCR.Text))

            End Select
        End If

        If ValorNumero(txtImporteCR.Text) < ValorNumero(txtSaldoSel.Text) Then
            Resto = ValorNumero(txtSaldoSel.Text) - ValorNumero(txtImporteCR.Text)
        Else
            MontoNegativo = ValorNumero(txtImporteCR.Text) - MontoPositivo
        End If

        MontoPositivo = Math.Abs(MontoPositivo)
        MontoNegativo = Math.Abs(MontoNegativo)

        Dim NumeroDocumento As String = ""
        'For jCont = 0 To lv.Items.Count - 1
        '    If lv.Items(jCont).Checked Then

        '        Dim MontoACancelar As Double = CDbl(lv.Items(jCont).SubItems(6).Text)
        '        Dim TipoDocumento As String = lv.Items(jCont).SubItems(1).Text
        '        NumeroDocumento = lv.Items(jCont).Text

        '        If MontoACancelar < 0 Then 'FC,GR,ND
        '            If MontoNegativo > 0 Then


        '                InsertEditCOMPRASCancelacion(myConn, lblInfo, True, CodigoProveedor, lv.Items(jCont).SubItems(1).Text,
        '                                            NumeroDocumento, CDate(lv.Items(jCont).SubItems(3).Text), txtReferenciaCR.Text,
        '                                            txtConceptoCR.Text, CDbl(lv.Items(jCont).SubItems(6).Text), Documento, "",
        '                                             jytsistema.WorkCurrency.Id, jytsistema.sFechadeTrabajo)


        '                If TipoDocumento = "FC" Or TipoDocumento = "GR" Then

        '                    If Math.Abs(MontoACancelar) > MontoNegativo Then
        '                        InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, IIf(Tipo = 3 OrElse Tipo = 4, "AB", "NC"), NumeroDocumento,
        '                             txtEmisionCR.Value, ft.FormatoHora(Now()), txtEmisionCR.Value,
        '                            txtReferenciaCR.Text, txtConceptoCR.Text, Math.Abs(MontoNegativo), 0.0, cmbFPCR.SelectedValue,
        '                            txtNumPagCR.Text, NombreDePago, txtBeneficiario.Text, "CXP", txtNumeroDeposito.Text, txtCuentaDeposito.Text,
        '                            txtBancoDeposito.Text, cmbCajaCR.SelectedValue, Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), causaNotaCredito, jytsistema.sFechadeTrabajo,
        '                            txtCodigoContable.Text, "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", Remesa, "", "", TipoProveedor, FOTipo.Credito, "0",
        '                             jytsistema.WorkCurrency.Id, jytsistema.sFechadeTrabajo)


        '                    Else

        '                        InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, cmbTipo.SelectedValue, NumeroDocumento,
        '                            txtEmisionCR.Value, ft.FormatoHora(Now), txtEmisionCR.Value,
        '                            txtReferenciaCR.Text, txtConceptoCR.Text,
        '                            Math.Abs(MontoACancelar), 0.0, cmbFPCR.SelectedValue,
        '                            txtNumPagCR.Text, NombreDePago, txtBeneficiario.Text, "CXP", txtNumeroDeposito.Text, txtCuentaDeposito.Text,
        '                            txtBancoDeposito.Text, cmbCajaCR.SelectedValue, Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), "", jytsistema.sFechadeTrabajo,
        '                            txtCodigoContable.Text, "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", Remesa, "", "", TipoProveedor, FOTipo.Credito, "0",
        '                             jytsistema.WorkCurrency.Id, jytsistema.sFechadeTrabajo)

        '                    End If


        '                Else

        '                    If Math.Abs(MontoACancelar) > MontoNegativo Then

        '                        InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, "NC", NumeroDocumento,
        '                                    txtEmisionCR.Value, ft.FormatoHora(Now()), txtEmisionCR.Value,
        '                                    txtReferenciaCR.Text, txtConceptoCR.Text, Math.Abs(MontoNegativo), 0.0, cmbFPCR.SelectedValue,
        '                                    txtNumPagCR.Text, NombreDePago, txtBeneficiario.Text, "CXP", txtNumeroDeposito.Text, txtCuentaDeposito.Text,
        '                                    txtBancoDeposito.Text, cmbCajaCR.SelectedValue, Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), causaNotaCredito, jytsistema.sFechadeTrabajo,
        '                                    txtCodigoContable.Text, "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", Remesa, "", "", TipoProveedor, FOTipo.Credito, "0",
        '                                     jytsistema.WorkCurrency.Id, jytsistema.sFechadeTrabajo)


        '                    Else

        '                        InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, "NC", NumeroDocumento,
        '                                    txtEmisionCR.Value, ft.FormatoHora(Now()), txtEmisionCR.Value,
        '                                    txtReferenciaCR.Text, txtConceptoCR.Text, Math.Abs(MontoACancelar), 0.0, cmbFPCR.SelectedValue,
        '                                    txtNumPagCR.Text, NombreDePago, txtBeneficiario.Text, "CXP", txtNumeroDeposito.Text, txtCuentaDeposito.Text,
        '                                    txtBancoDeposito.Text, cmbCajaCR.SelectedValue, Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), causaNotaCredito, jytsistema.sFechadeTrabajo,
        '                                    txtCodigoContable.Text, "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", Remesa, "", "", TipoProveedor, FOTipo.Credito, "0",
        '                                     jytsistema.WorkCurrency.Id, jytsistema.sFechadeTrabajo)

        '                    End If

        '                End If

        '                MontoNegativo -= Math.Abs(MontoACancelar)

        '            End If

        '        ElseIf MontoACancelar > 0 Then


        '            If Math.Abs(MontoACancelar) > MontoPositivo Then

        '                InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, "ND", NumeroDocumento,
        '                    txtEmisionCR.Value, ft.FormatoHora(Now()), txtEmisionCR.Value,
        '                    txtReferenciaCR.Text, txtConceptoCR.Text, -1 * Math.Abs(MontoPositivo),
        '                    0.0, cmbFPCR.SelectedValue, txtNumPagCR.Text, NombreDePago,
        '                    txtBeneficiario.Text, "CXP", txtNumeroDeposito.Text, txtCuentaDeposito.Text, txtBancoDeposito.Text,
        '                    cmbCajaCR.SelectedValue, Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), "", jytsistema.sFechadeTrabajo,
        '                    txtCodigoContable.Text, "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", Remesa, "", "",
        '                    TipoProveedor, FOTipo.Debito, "0",
        '                     jytsistema.WorkCurrency.Id, jytsistema.sFechadeTrabajo)
        '            Else
        '                InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, "ND", NumeroDocumento,
        '                    txtEmisionCR.Value, ft.FormatoHora(Now()), txtEmisionCR.Value,
        '                    txtReferenciaCR.Text, txtConceptoCR.Text, -1 * Math.Abs(MontoACancelar),
        '                    0.0, cmbFPCR.SelectedValue, txtNumPagCR.Text, NombreDePago,
        '                    txtBeneficiario.Text, "CXP", txtNumeroDeposito.Text, txtCuentaDeposito.Text, txtBancoDeposito.Text,
        '                    cmbCajaCR.SelectedValue, Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), "", jytsistema.sFechadeTrabajo,
        '                    txtCodigoContable.Text, "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", Remesa, "", "",
        '                    TipoProveedor, FOTipo.Debito, "0",
        '                     jytsistema.WorkCurrency.Id, jytsistema.sFechadeTrabajo)

        '            End If

        '            InsertEditCOMPRASCancelacion(myConn, lblInfo, True, CodigoProveedor, lv.Items(jCont).SubItems(1).Text,
        '                    NumeroDocumento, CDate(lv.Items(jCont).SubItems(3).Text), txtReferenciaCR.Text,
        '                    txtConceptoCR.Text, CDbl(lv.Items(jCont).SubItems(6).Text), Documento, "",
        '                     jytsistema.WorkCurrency.Id, jytsistema.sFechadeTrabajo)


        '            MontoPositivo -= CDbl(MontoACancelar)

        '        Else

        '        End If

        '        'Resto -= CDbl(lv.Items(jCont).SubItems(6).Text)
        '        'MODIFICA PROPIETARIO -> ULTIMO PAGO, FECHA ULTIMO PAGO , FORMA DE PAGO 

        '        ft.Ejecutar_strSQL(myConn, " update jsprocatpro set " _
        '            & " ultpago = " & ValorNumero(txtImporteCR.Text) & ", " _
        '            & " fecultpago = '" & ft.FormatoFechaMySQL(txtEmisionCR.Value) & "', " _
        '            & " forultpago = '" & cmbFPCR.SelectedValue & "' " _
        '            & " where " _
        '            & " codpro = '" & CodigoProveedor & "' and " _
        '            & " id_emp = '" & jytsistema.WorkID & "' ")

        '        'MODIFICA FACTURA QUE ORIGINÓ LA CXP
        '        ft.Ejecutar_strSQL(myConn, " update jsproencgas set " _
        '                       & " formapag = '" & cmbFPCR.SelectedValue & "', " _
        '                       & " numpag = '" & txtNumPagCR.Text & "', " _
        '                       & " nompag = '" & NombreDePago & "', " _
        '                       & " benefic = '" & txtBeneficiario.Text & "', " _
        '                       & " caja = '" & cmbCajaCR.SelectedValue & "' " _
        '                       & " where " _
        '                       & " numgas = '" & NumeroDocumento & "' and " _
        '                       & " codpro = '" & CodigoProveedor & "' and " _
        '                       & " id_emp = '" & jytsistema.WorkID & "' ")

        '        ft.Ejecutar_strSQL(myConn, " update jsproenccom set " _
        '                       & " formapag = '" & cmbFPCR.SelectedValue & "', " _
        '                       & " numpag = '" & txtNumPagCR.Text & "', " _
        '                       & " nompag = '" & NombreDePago & "', " _
        '                       & " benefic = '" & txtBeneficiario.Text & "', " _
        '                       & " caja = '" & cmbCajaCR.SelectedValue & "' " _
        '                       & " where " _
        '                       & " numcom = '" & NumeroDocumento & "' and " _
        '                       & " codpro = '" & CodigoProveedor & "' and " _
        '                       & " id_emp = '" & jytsistema.WorkID & "' ")

        '    End If
        'Next

        If Resto > 0 Then
            Dim sDocSel As String = ""
            Dim sDocCan As String = ""
            If txtDocSel.Text <> "" And txtDocSel.Text <> "0" Then
                sDocSel = IIf(CInt(txtDocSel.Text) <= 1, "0", "1")
                'sDocCan = lv.Items(jCont).SubItems(1).Text
            End If

            If NumeroDocumento.Trim = "" Then NumeroDocumento = Contador(myConn, lblInfo, Gestion.iCompras, "COMNUMNCC", "08")

            InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, "NC", NumeroDocumento,
                           txtEmisionCR.Value, ft.FormatoHora(Now()), txtEmisionCR.Value, txtReferenciaCR.Text, txtConceptoCR.Text,
                           Resto, 0.0, cmbFPCR.SelectedValue, txtNumPagCR.Text, NombreDePago, txtBeneficiario.Text, "CXP", txtNumeroDeposito.Text,
                           txtCuentaDeposito.Text, txtBancoDeposito.Text,
                           cmbCaja.SelectedValue, Documento, sDocSel, causaCredito.Codigo, jytsistema.sFechadeTrabajo,
                           cmbCC.SelectedValue, "", sDocCan, 0.0, 0.0, Documento, "", "", Remesa, "", "",
                           TipoProveedor, FOTipo.Credito, "0",
                            jytsistema.WorkCurrency.Id, jytsistema.sFechadeTrabajo)
        End If

        InsertarAuditoria(myConn, IIf(i_modo = movimiento.iAgregar, MovAud.iIncluir, MovAud.imodificar), sModulo, Documento)
        ComprobanteNumero = Documento

    End Sub

    Private Sub txtImporteDB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtImporteDB.Click
        ft.enfocarTexto(sender)
    End Sub


    Private Sub cmbFPCR_SelectedIndexChanged_(sender As Object, e As EventArgs) Handles cmbFPCR.SelectedIndexChanged

        Select Case cmbFPCR.SelectedIndex
            Case 0 'EF
                ft.habilitarObjetos(False, True, txtNumPagCR, btnNumPago, cmbNombrePago, txtBeneficiario)
                ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNumPagCR, txtBeneficiario)
            Case 1, 2, 3, 4, 5 'CH, TA, CT, DP, TR
                ft.habilitarObjetos(True, True, txtNumPagCR, btnNumPago, cmbNombrePago, txtBeneficiario)
                ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNumPagCR)

                If cmbFPCR.SelectedIndex = 1 Or cmbFPCR.SelectedIndex = 4 Or cmbFPCR.SelectedIndex = 5 Then
                    txtBeneficiario.Text = ft.DevuelveScalarCadena(myConn, " select nombre from jsprocatpro where codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                End If

                If cmbFPCR.SelectedIndex = 2 Or cmbFPCR.SelectedIndex = 3 Then
                    ft.mensajeCritico("Formas de pago no aceptadas por los momentos...")
                    cmbFPCR.SelectedIndex = 0
                End If
        End Select
    End Sub

    Private Sub txtImporteDB_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtImporteDB.KeyPress,
        txtImporteCR.KeyPress, txtPorRetIVA.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub lvRetIVA_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs)
        If e.Item.SubItems.Count >= 3 Then
            If e.Item.Checked Then
                strDocsRetIVA += " nummov = '" & e.Item.Text & "' OR "
                TotalRetencionIVA += CDbl(e.Item.SubItems(6).Text)
            Else
                strDocsRetIVA = Replace(strDocsRetIVA, " nummov = '" & e.Item.Text & "' OR ", "")
                TotalRetencionIVA -= CDbl(e.Item.SubItems(6).Text)
            End If

            IniciarTablaRetencionIVA(ValorNumero(txtPorRetIVA.Text))
            CalculaTotalRetIVA()

        End If

    End Sub
    Private Sub IniciarTablaRetencionIVA(ByVal PorcentajeRetencion As Double)

        Dim dtIVA As DataTable
        Dim tblIVA As String = "tblIVA"
        ds = DataSetRequery(ds, " SELECT a.tipoiva, a.poriva, SUM(a.baseiva) baseiva, SUM(a.impiva) impiva, SUM(a.impiva)*" & PorcentajeRetencion / 100 & " retiva " _
                                    & " FROM (SELECT a.tipoiva, a.poriva, -1*a.baseiva baseiva, -1*a.impiva impiva " _
                                    & "       	FROM jsproivacom a " _
                                    & "         WHERE " _
                                    & Replace("(" & strDocsRetIVA.Substring(0, strDocsRetIVA.Length - 4) & ") and ", "nummov", "a.numcom") _
                                    & "         codpro = '" & CodigoProveedor & "' AND " _
                                    & "     	id_emp = '" & jytsistema.WorkID & "' " _
                                    & "         UNION " _
                                    & " 	  SELECT a.tipoiva, a.poriva, -1*a.baseiva baseiva, -1*a.impiva impiva " _
                                    & "    	    FROM jsproivagas a " _
                                    & "         WHERE " _
                                    & Replace("(" & strDocsRetIVA.Substring(0, strDocsRetIVA.Length - 4) & ") and ", "nummov", "a.numgas") _
                                    & "     	CODPRO = '" & CodigoProveedor & "' AND  " _
                                    & "  	    id_emp = '" & jytsistema.WorkID & "' " _
                                    & " UNION " _
                                    & " 	  SELECT a.tipoiva, a.poriva, a.baseiva, a.impiva " _
                                    & "    	    FROM jsproivancr a " _
                                    & "         WHERE " _
                                    & Replace("(" & strDocsRetIVA.Substring(0, strDocsRetIVA.Length - 4) & ") and ", "nummov", "a.numncr") _
                                    & "     	CODPRO = '" & CodigoProveedor & "' AND  " _
                                    & "  	    id_emp = '" & jytsistema.WorkID & "'" _
                                    & " UNION " _
                                    & " 	  SELECT a.tipoiva, a.poriva, -1*a.baseiva baseiva, -1*a.impiva impiva " _
                                    & "    	    FROM jsproivandb a " _
                                    & "         WHERE " _
                                    & Replace("(" & strDocsRetIVA.Substring(0, strDocsRetIVA.Length - 4) & ") and ", "nummov", "a.numndb") _
                                    & "     	CODPRO = '" & CodigoProveedor & "' AND  " _
                                    & "  	    id_emp = '" & jytsistema.WorkID & "' ) a " _
                                    & " GROUP BY a.tipoiva ", myConn, tblIVA, lblInfo)

        dtIVA = ds.Tables(tblIVA)

        Dim aNombresIVA() As String = {"Tipo IVA", "Porcentaje IVA", "Base Imponible", "IVA", "IVA a Retener", ""}
        Dim aCamposIVA() As String = {"tipoiva", "poriva", "baseiva", "impiva", "retiva", ""}
        Dim aAnchosIVA() As Integer = {70, 70, 100, 100, 100, 100}
        Dim aAlineacionIVA() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Izquierda}

        Dim aFormatosIVA() As String = {"", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, ""}

        IniciarTabla(dgRetIVA, dtIVA, aCamposIVA, aNombresIVA, aAnchosIVA, aAlineacionIVA, aFormatosIVA)

    End Sub
    Private Sub CalculaTotalRetIVA()
        txtSaldoRetIVA.Text = ft.FormatoNumero(TotalRetencionIVA)
        txtRetIVA.Text = ft.FormatoNumero(Math.Round(ValorNumero(txtPorRetIVA.Text) / 100 * TotalRetencionIVA, 2))
    End Sub

    Private Sub txtPorRetIVA_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPorRetIVA.TextChanged
        If ft.isNumeric(txtPorRetIVA.Text) Then _
            IniciarTablaRetencionIVA(ValorNumero(txtPorRetIVA.Text))
        CalculaTotalRetIVA()
    End Sub

    Private Sub cmbConceptoRetIVA_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbConceptoRetISLR.SelectedIndexChanged,
        txtSaldoSelRetISLR.TextChanged, txtPorcentajeRetISLR.TextChanged
        CalculaTotalesRetISLR()
    End Sub

    Private Sub lvRetISLR_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs)

        If e.Item.Checked Then
            iSel += 1
            dSel += CDbl(e.Item.SubItems(6).Text)
            MontoPositivo += IIf(CDbl(e.Item.SubItems(6).Text) > 0, Math.Round(CDbl(e.Item.SubItems(6).Text), 2), 0)
            MontoNegativo += IIf(CDbl(e.Item.SubItems(6).Text) < 0, Math.Round(CDbl(e.Item.SubItems(6).Text), 2), 0)
        Else
            iSel -= 1
            If e.Item.SubItems.Count > 1 Then

                dSel -= CDbl(e.Item.SubItems(6).Text)
                MontoPositivo -= IIf(CDbl(e.Item.SubItems(6).Text) > 0, Math.Round(CDbl(e.Item.SubItems(6).Text), 2), 0)
                MontoNegativo -= IIf(CDbl(e.Item.SubItems(6).Text) < 0, Math.Round(CDbl(e.Item.SubItems(6).Text), 2), 0)
            End If
            If iSel < 0 Then iSel = 0
        End If


        txtDocsSelRetISLR.Text = ft.FormatoEntero(iSel)
        txtSaldoSelRetISLR.Text = ft.FormatoNumero(dSel)

    End Sub

    Private Sub CalculaTotalesRetISLR()
        If cmbConceptoRetISLR.SelectedValue IsNot Nothing Then
            txtPorcentajeMontoBaseRetISLR.Text = ft.FormatoNumero(ft.DevuelveScalarDoble(myConn, " select baseimp from jscontabret where codret = '" & cmbConceptoRetISLR.SelectedValue.ToString & "' and id_emp = '" & jytsistema.WorkID & "' "))
            txtMontoBaseRetISLR.Text = ft.FormatoNumero(Math.Round(ValorNumero(txtPorcentajeMontoBaseRetISLR.Text) / 100 * ValorNumero(txtSaldoSelRetISLR.Text), 2))
            txtMinimoRetISLR.Text = ft.FormatoNumero(ft.DevuelveScalarDoble(myConn, " select pagomin from jscontabret where codret = '" & cmbConceptoRetISLR.SelectedValue.ToString & "' and id_emp = '" & jytsistema.WorkID & "' "))
            txtAcumuladoSinRetISLR.Text = ft.FormatoNumero(0.0)

            If ValorNumero(txtMontoBaseRetISLR.Text) > 0 Then
                If ValorNumero(txtMontoBaseRetISLR.Text) > ValorNumero(txtMinimoRetISLR.Text) Then
                    txtPorcentajeRetISLR.Text = ft.FormatoNumero(ft.DevuelveScalarDoble(myConn, " select tarifa from jscontabret where codret = '" & cmbConceptoRetISLR.SelectedValue.ToString & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    txtSustraendoRetISLR.Text = ft.FormatoNumero(ft.DevuelveScalarDoble(myConn, " select menos from jscontabret where codret = '" & cmbConceptoRetISLR.SelectedValue.ToString & "' and id_emp = '" & jytsistema.WorkID & "' "))
                Else
                    txtPorcentajeRetISLR.Text = ft.FormatoNumero(0.0)
                    txtSustraendoRetISLR.Text = ft.FormatoNumero(0.0)
                End If
            End If

            txtMontoRetencionISLR.Text = ft.FormatoNumero(Math.Round(ValorNumero(txtMontoBaseRetISLR.Text) * ValorNumero(txtPorcentajeRetISLR.Text) / 100, 2) - ValorNumero(txtSustraendoRetISLR.Text))

        End If
    End Sub
    Private Sub cmbCausaNC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCausaNC.SelectedIndexChanged

        causaCredito = cmbCausaNC.SelectedItem
        txtConceptoCR.Text += " " + causaCredito.Descripcion
        txtReferenciaCR.Text = ContadorNC_Financiera(myConn, causaCredito.Codigo, "CXP")

        If CBool(causaCredito.Credit) Then
            optCR.Checked = True
            grpPago.Enabled = False
        Else
            optCO.Checked = True
            grpPago.Enabled = True
        End If

    End Sub

    Private Sub dg_SelectionChanged(sender As Object, e As Syncfusion.WinForms.DataGrid.Events.SelectionChangedEventArgs) Handles dg.SelectionChanged
        txtDocSel.Text = ft.FormatoEntero(dg.SelectedItems.Count)
        txtSaldoSel.Text = ft.FormatoNumero(dg.SelectedItems.Sum(Function(s) s.Saldo))
    End Sub

    Private Sub lv_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs)
        If e.Item.Checked Then
            iSel += 1
            dSel += CDbl(e.Item.SubItems(6).Text)
            strDocs = strDocs & e.Item.Text & ", "
            MontoPositivo += IIf(CDbl(e.Item.SubItems(6).Text) > 0, Math.Round(CDbl(e.Item.SubItems(6).Text), 2), 0)
            MontoNegativo += IIf(CDbl(e.Item.SubItems(6).Text) < 0, Math.Round(CDbl(e.Item.SubItems(6).Text), 2), 0)
        Else
            iSel -= 1
            If e.Item.SubItems.Count > 1 Then
                dSel -= CDbl(e.Item.SubItems(6).Text)
                strDocs = Replace(strDocs, e.Item.Text & ", ", "")
                MontoPositivo -= IIf(CDbl(e.Item.SubItems(6).Text) > 0, Math.Round(CDbl(e.Item.SubItems(6).Text), 2), 0)
                MontoNegativo -= IIf(CDbl(e.Item.SubItems(6).Text) < 0, Math.Round(CDbl(e.Item.SubItems(6).Text), 2), 0)
            End If

        End If

        If iSel < 0 Then
            iSel = 0
            dSel = 0.0
            MontoNegativo = 0.0
            MontoPositivo = 0.0
        End If



        txtConceptoCR.Text = strDocsIni & " " & IIf(iSel > 1, "DOCUMENTOS ", "DOCUMENTO ")
        If strDocs.Length >= 2 Then
            txtConceptoCR.Text = strDocsIni & " " & IIf(iSel > 1, "DOCUMENTOS ", "DOCUMENTO ") & strDocs.Substring(0, strDocs.Length - 2)
        End If

        txtImporteCR.Text = ft.FormatoNumero(dSel)

    End Sub
End Class