Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsComArcProveedoresCXP
    Private Const sModulo As String = "Proveedores y CxP - Movimientos CxP"

    Private Const nTablaSaldos As String = "saldos"
    Private Const nTablaCajas As String = "cajas"
    Private Const nTablaBancosExt As String = "bancosext"
    Private Const nTablaBancosInt As String = "bancos"
    Private Const nTablaTarjetas As String = "tarjetas"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As DataTable
    Private dtSaldos As DataTable
    Private dtSaldosISLR As DataTable
    Private dtSaldosIVA As DataTable
    Private dtCajas As DataTable
    Private dtBancosExt As DataTable
    Private dtBancosInt As DataTable
    Private dtTarjetas As DataTable
    Private ft As New Transportables

    Private strSQLCajas As String = "select caja, concat(caja,'-',nomcaja) nomcaja from jsbanenccaj where id_emp = '" & jytsistema.WorkID & "' "
    Private strSQLBancosExt As String = " select codigo, descrip, descrip descripcion from jsconctatab where modulo = '00010' and id_emp = '" & jytsistema.WorkID & "' "
    Private strSQLBancosInt As String = " select codban codigo, nomban descripcion from jsbancatban where id_emp = '" & jytsistema.WorkID & "' "
    Private strSQLTarjetas As String = " select codtar codigo, nomtar descripcion from jsconctatar where id_emp = '" & jytsistema.WorkID & "' "

    Private i_modo As Integer
    Private nPosicion As Long
    Private CodigoProveedor As String
    Private TipoProveedor As Integer

    Private aTipo() As String = {"Factura", "Giro", "Nota D�bito", "Abono", "Cancelaci�n", "Nota Cr�dito", "---", "Retenci�n IVA", "Retenci�n ISLR"}
    Private aTipoNick() As String = {"FC", "GR", "ND", "AB", "CA", "NC", "", "NC", "NC"}
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
    Private causaNotaCredito As String = ""
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Property Comprobante() As String
        Get
            Return ComprobanteNumero
        End Get
        Set(ByVal value As String)
            ComprobanteNumero = value
        End Set
    End Property
    Public Property TipoMovimientoCXP() As Integer
        Get
            Return nTipoMovimientoCXP
        End Get
        Set(ByVal value As Integer)
            nTipoMovimientoCXP = value
        End Set
    End Property

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

        ft.RellenaCombo(aTipo, cmbTipo, numTipoMovimiento)
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

        numTipoMovimiento = numTipoMovimientoOrigen()
        ft.RellenaCombo(aTipo, cmbTipo, numTipoMovimiento)
        cmbTipo.Enabled = False

        Me.ShowDialog()

    End Sub
    Private Function numTipoMovimientoOrigen() As Integer

        numTipoMovimientoOrigen = ft.InArray(aTipoNick, dt.Rows(Apuntador).Item("tipomov"))
        If numTipoMovimientoOrigen > 4 Then
            numTipoMovimientoOrigen = 5
            If Mid(dt.Rows(Apuntador).Item("REFER"), 1, 5) = "ISLR-" Then numTipoMovimientoOrigen = 8
            If Mid(dt.Rows(Apuntador).Item("CONCEPTO"), 1, 13) = "RETENCION IVA" Then numTipoMovimientoOrigen = 7
        End If

    End Function

    Private Sub jsComArcProveedoresCXP_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsComArcProveedoresCXP_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ds = DataSetRequery(ds, strSQLCajas, myConn, nTablaCajas, lblInfo)
        dtCajas = ds.Tables(nTablaCajas)
        Dim dates As SfDateTimeEdit() = {txtEmisionCR, txtEmisionDB, txtFechaRetISLR, txtFechaRetIVA, txtVenceDB}
        SetSizeDateObjects(dates)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub cmbTipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipo.SelectedIndexChanged
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
        ft.habilitarObjetos(False, True, txtDocumentoCR, txtDocSel, txtSaldoSel, txtNumPagCR, cmbNombrePago, txtNomPagCR, txtDocSel, txtSaldoSel,
                         txtCodigoContable)

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

        IniciarSaldos(lv)

        optCO.Checked = True
        If Tipo <> 5 Then
            ft.habilitarObjetos(False, False, grpCRCO)
            ft.visualizarObjetos(False, lblNotaCredito, txtCausaNotaCredito, btnCausaNotaCredito, lblDescripCausaNotaCredito)
        Else
            ft.habilitarObjetos(False, False, grpCRCO, optCO, optCR, txtCausaNotaCredito)
            ft.visualizarObjetos(True, lblNotaCredito, txtCausaNotaCredito, btnCausaNotaCredito, lblDescripCausaNotaCredito)
            txtCausaNotaCredito.Text = ""
        End If
        IniciarCajas()

        cmbFPCR.DataSource = formasDePago
        cmbFPCR.DisplayMember = "Method"
        cmbFPCR.ValueMember = "ShortName"
        cmbFPCR.SelectedIndex = 0

        txtCodigoContable.Text = ft.DevuelveScalarCadena(myConn, "SELECT codcon from jsprocatpro where codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "'  ")

    End Sub
    Private Sub ResetearVariablesDEUsoComun()
        iSel = 0
        dSel = 0
        MontoPositivo = 0
        MontoNegativo = 0
    End Sub

    Private Sub IniciarRetencionIVA()

        ft.habilitarObjetos(False, True, txtSaldoRetIVA, txtRetIVA, txtCodigoContableIVA)
        txtSaldoRetIVA.Text = ft.FormatoNumero(0.0)
        txtFechaRetIVA.Value = jytsistema.sFechadeTrabajo
        txtPorRetIVA.Text = ft.FormatoNumero(75.0)
        txtRetIVA.Text = ft.FormatoNumero(CalculaRetencionIVA())

        IniciarSaldosIVA(lvRetIVA)



        lblLeyendaIVA.Text = "Si el documento al cual desea hacer retenci�n del IVA no aparece : " & vbLf _
            & " 1. El Documento no posee n�mero de control de IVA " & vbLf _
            & " 2. El Documento no posee renglones con tasa de impuesto mayor a CERO (0) " & vbLf _
            & " 3. El Documento YA posee una Retenci�n Asignada." & vbLf _
            & " 4. El documento tiene saldo cero (0) (YA fu� cancelado) "

    End Sub
    Private Function CalculaRetencionIVA() As Double
        Return Math.Round(ValorNumero(txtPorRetIVA.Text) / 100 * ValorNumero(txtSaldoRetIVA.Text), 2)
    End Function
    Private Sub IniciarRetencionISLR()

        ResetearVariablesDEUsoComun()
        ft.habilitarObjetos(False, True, txtDocsSelRetISLR, txtSaldoSelRetISLR, txtPorcentajeMontoBaseRetISLR,
                        txtMontoBaseRetISLR, txtMinimoRetISLR,
                        txtAcumuladoSinRetISLR, txtSustraendoRetISLR, txtMontoRetencionISLR,
                        txtCodigoContableISLR)

        IniciarSaldosISLR(lvRetISLR)

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

    Private Sub IniciarSaldos(ByVal lv As ListView)

        Dim strSaldos As String = " SELECT a.codpro, b.nombre, a.nummov, a.tipomov, a.refer, a.emision, a.vence, a.importe, c.saldo " _
                                  & " FROM jsprotrapag a " _
                                  & " LEFT JOIN jsprocatpro b ON (a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
                                  & " LEFT JOIN (SELECT a.codpro, a.nummov, a.tipomov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp " _
                                  & "            FROM jsprotrapag a " _
                                  & "            WHERE " _
                                  & "            a.REMESA = '" & Remesa & "' AND " _
                                  & "            a.codpro = '" & CodigoProveedor & "' AND " _
                                  & "            a.id_emp = '" & jytsistema.WorkID & "' " _
                                  & "            GROUP BY a.nummov) c ON (a.codpro = c.codpro AND a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                                  & " WHERE " _
                                  & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' GROUP BY nummov) AND " _
                                  & " a.codpro = '" & CodigoProveedor & "'  AND " _
                                  & " a.codpro <> '' AND " _
                                  & " (c.saldo > 0.001 OR c.saldo < -0.001) AND " _
                                  & " a.historico = '0' AND " _
                                  & " a.REMESA = '" & Remesa & "' AND " _
                                  & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                  & " ORDER BY a.nummov, a.emision "

        ds = DataSetRequery(ds, strSaldos, myConn, nTablaSaldos, lblInfo)
        dtSaldos = ds.Tables(nTablaSaldos)

        Dim aNombres() As String = {"N� Documento", "TP", "Referencia", "Emisi�n", "Vence", "Importe inicial", "Saldo"}
        Dim aCampos() As String = {"nummov", "tipomov", "refer", "emision", "vence", "importe", "saldo"}
        Dim aAnchos() As Integer = {150, 40, 120, 100, 100, 120, 120}
        Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right}
        Dim aFormato() As FormatoItemListView = {FormatoItemListView.iCadena, FormatoItemListView.iCadena, FormatoItemListView.iCadena, FormatoItemListView.iFecha, FormatoItemListView.iFecha, FormatoItemListView.iNumero, FormatoItemListView.iNumero}

        CargaListViewDesdeTabla(lv, dtSaldos, aNombres, aCampos, aAnchos, aAlineacion, aFormato)


    End Sub
    Private Sub IniciarSaldosISLR(ByVal lv As ListView)

        Dim strSaldos As String = " SELECT a.codpro, b.nombre, a.nummov, a.tipomov, c.num_control, a.emision, a.vence, a.importe, c.saldo " _
                                  & " FROM jsprotrapag a " _
                                  & " LEFT JOIN jsprocatpro b ON (a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
                                  & " LEFT JOIN (SELECT a.codpro, a.numcom nummov, c.num_control, SUM(a.costototdes) saldo, a.id_emp " _
                                                    & " FROM jsprorencom a " _
                                                    & " LEFT JOIN jsproenccom b ON (a.numcom = b.numcom AND a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
                                                    & " LEFT JOIN jsconnumcon c ON (a.numcom = c.numdoc AND a.codpro = c.prov_cli AND b.emisioniva = c.emision AND c.org = 'COM' AND origen = 'COM' AND a.id_emp = c.id_emp)" _
                                                    & " LEFT JOIN jsmercatser d on (a.item = concat('$', d.codser) and d.tipo = '0' and a.id_emp = d.id_emp) " _
                                                    & " WHERE " _
                                                    & " d.tiposervicio = '0' and  d.tipo = '0' and " _
                                                    & " b.num_ret_islr = '' AND " _
                                                    & " a.codpro = '" & CodigoProveedor & "' and " _
                                                    & " MID(a.item,1,1) = '$' AND " _
                                                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                                    & " GROUP BY a.codpro, a.numcom " _
                                                    & " UNION " _
                                                    & " SELECT a.codpro, a.numgas, c.num_control, SUM(a.costototdes) saldo, a.id_emp " _
                                                    & " FROM jsprorengas a " _
                                                    & " LEFT JOIN jsproencgas b ON (a.numgas = b.numgas AND a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
                                                    & " LEFT JOIN jsconnumcon c ON (a.numgas = c.numdoc AND a.codpro = c.prov_cli AND b.emisioniva = c.emision AND c.org = 'GAS' AND origen = 'COM' AND a.id_emp = c.id_emp) " _
                                                    & " LEFT JOIN jsmercatser d on (a.item = concat('$', d.codser) and d.tipo = '0' and a.id_emp = d.id_emp) " _
                                                    & " WHERE " _
                                                    & " d.tiposervicio = '0' and d.tipo = '0' and " _
                                                    & " b.num_ret_islr = '' AND " _
                                                    & " a.codpro = '" & CodigoProveedor & "' and " _
                                                    & " MID(a.item,1,1) = '$' AND " _
                                                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                                    & " GROUP BY a.codpro, a.numgas) c ON (a.codpro = c.codpro AND a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                                  & " WHERE " _
                                  & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' GROUP BY nummov) AND " _
                                  & " a.codpro = '" & CodigoProveedor & "'  AND " _
                                  & " a.codpro <> '' AND " _
                                  & " (c.saldo > 0.001 OR c.saldo < -0.001) AND " _
                                  & " a.REMESA = '" & Remesa & "' AND " _
                                  & " a.historico = '0' AND " _
                                  & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                  & " ORDER BY a.nummov, a.emision "

        ds = DataSetRequery(ds, strSaldos, myConn, nTablaSaldos, lblInfo)
        dtSaldos = ds.Tables(nTablaSaldos)

        Dim aNombres() As String = {"N� Documento", "TP", "N� Control", "Emisi�n", "Vence", "Importe inicial", "Por servicios"}
        Dim aCampos() As String = {"nummov", "tipomov", "num_control", "emision", "vence", "importe", "saldo"}
        Dim aAnchos() As Integer = {150, 40, 120, 100, 100, 120, 120}
        Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right}
        Dim aFormato() As FormatoItemListView = {FormatoItemListView.iCadena, FormatoItemListView.iCadena, FormatoItemListView.iCadena, FormatoItemListView.iFecha, FormatoItemListView.iFecha, FormatoItemListView.iNumero, FormatoItemListView.iNumero}

        CargaListViewDesdeTabla(lv, dtSaldos, aNombres, aCampos, aAnchos, aAlineacion, aFormato)



    End Sub
    Private Sub IniciarSaldosIVA(ByVal lv As ListView)

        Dim strSaldos As String = "SELECT a.codpro, b.nombre, a.nummov, a.tipomov, d.num_control, a.emision, a.vence, a.importe, c.impiva impiva " _
                                  & " FROM jsprotrapag a " _
                                  & " LEFT JOIN jsprocatpro b ON (a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
                                  & " LEFT JOIN (SELECT a.codpro, a.numcom nummov, SUM(a.baseiva) baseiva, SUM(a.impiva) impiva, a.id_emp " _
                                  & "            FROM jsproivacom a " _
                                  & "            WHERE " _
                                  & "            a.tipoiva NOT IN ('', 'E') AND " _
                                  & "            a.codpro = '" & CodigoProveedor & "' AND " _
                                  & "            a.id_emp = '" & jytsistema.WorkID & "' " _
                                  & "            GROUP BY a.numcom) c ON (a.codpro = c.codpro AND a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                                  & " LEFT JOIN jsconnumcon d on (a.nummov = d.numdoc and a.codpro = d.prov_cli and d.org = 'COM' and d.origen = 'COM' and a.id_emp = d.id_emp) " _
                                  & " LEFT JOIN (SELECT a.codpro, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov AND a.codpro = e.codpro ) " _
                                  & " WHERE " _
                                  & " d.num_control <> '' and " _
                                  & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' GROUP BY nummov) AND " _
                                  & " a.nummov NOT IN (SELECT a.nummov FROM jsprotrapag a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codpro = '" & CodigoProveedor & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) AND " _
                                  & " a.codpro = '" & CodigoProveedor & "'  AND " _
                                  & " a.codpro <> '' AND " _
                                  & " c.impiva <> 0.00 AND " _
                                  & " e.saldo <> 0.00 AND " _
                                  & " a.historico = '0' AND " _
                                  & " a.REMESA = '" & Remesa & "' AND " _
                                  & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                  & " UNION " _
                                  & " SELECT a.codpro, b.nombre, a.nummov, a.tipomov, d.num_control, a.emision, a.vence, a.importe, c.impiva impiva " _
                                  & " FROM jsprotrapag a " _
                                  & " LEFT JOIN jsprocatpro b ON (a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
                                  & " LEFT JOIN (SELECT a.codpro, a.numgas nummov, SUM(a.baseiva) baseiva, SUM(a.impiva) impiva, a.id_emp " _
                                  & "            FROM jsproivagas a " _
                                  & "            WHERE " _
                                  & "            a.tipoiva NOT IN ('', 'E') AND " _
                                  & "            a.codpro = '" & CodigoProveedor & "' AND " _
                                  & "            a.id_emp = '" & jytsistema.WorkID & "' " _
                                  & "            GROUP BY a.numgas) c ON (a.codpro = c.codpro AND a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                                  & " LEFT JOIN jsconnumcon d on (a.nummov = d.numdoc and a.codpro = d.prov_cli and d.org = 'GAS' and d.origen = 'COM' and a.id_emp = d.id_emp) " _
                                  & " LEFT JOIN (SELECT a.codpro, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov AND a.codpro = e.codpro ) " _
                                  & " WHERE " _
                                  & " d.num_control <> '' and " _
                                  & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' GROUP BY nummov) AND " _
                                  & " a.nummov NOT IN (SELECT a.nummov FROM jsprotrapag a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codpro = '" & CodigoProveedor & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) AND " _
                                  & " a.codpro = '" & CodigoProveedor & "'  AND " _
                                  & " a.codpro <> '' AND " _
                                  & " c.impiva <> 0.00 AND " _
                                  & " e.saldo <> 0.00 AND " _
                                  & " a.historico = '0' AND " _
                                  & " a.REMESA = '" & Remesa & "' AND " _
                                  & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                  & " UNION " _
                                  & " SELECT a.codpro, b.nombre, a.nummov, a.tipomov, d.num_control, a.emision, a.vence, a.importe, c.impiva impiva " _
                                  & " FROM jsprotrapag a " _
                                  & " LEFT JOIN jsprocatpro b ON (a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
                                  & " LEFT JOIN (SELECT a.codpro, a.numncr nummov, SUM(a.baseiva) baseiva, SUM(a.impiva) impiva, a.id_emp " _
                                  & "            FROM jsproivancr a " _
                                  & "            WHERE " _
                                  & "            a.tipoiva NOT IN ('', 'E') AND " _
                                  & "            a.codpro = '" & CodigoProveedor & "' AND " _
                                  & "            a.id_emp = '" & jytsistema.WorkID & "' " _
                                  & "            GROUP BY a.numncr) c ON (a.codpro = c.codpro AND a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                                  & " LEFT JOIN jsconnumcon d on (a.nummov = d.numdoc and a.codpro = d.prov_cli  and d.org = 'NCR' and d.origen = 'COM' and a.id_emp = d.id_emp) " _
                                  & " LEFT JOIN (SELECT a.codpro, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov AND a.codpro = e.codpro ) " _
                                  & " WHERE " _
                                  & " d.num_control <> '' and " _
                                  & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' GROUP BY nummov) AND " _
                                  & " a.nummov NOT IN (SELECT a.nummov FROM jsprotrapag a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codpro = '" & CodigoProveedor & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) AND " _
                                  & " a.codpro = '" & CodigoProveedor & "'  AND " _
                                  & " a.codpro <> '' AND " _
                                  & " c.impiva <> 0.00 AND " _
                                  & " e.saldo <> 0.00 AND " _
                                  & " a.REMESA = '" & Remesa & "' AND " _
                                  & " a.historico = '0' AND " _
                                  & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                  & " UNION " _
                                  & " SELECT a.codpro, b.nombre, a.nummov, a.tipomov, d.num_control, a.emision, a.vence, a.importe, c.impiva impiva " _
                                  & " FROM jsprotrapag a " _
                                  & " LEFT JOIN jsprocatpro b ON (a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
                                  & " LEFT JOIN (SELECT a.codpro, a.numndb nummov, SUM(a.baseiva) baseiva, SUM(a.impiva) impiva, a.id_emp " _
                                  & "            FROM jsproivandb a " _
                                  & "            WHERE " _
                                  & "            a.tipoiva NOT IN ('', 'E') AND " _
                                  & "            a.codpro = '" & CodigoProveedor & "' AND " _
                                  & "            a.id_emp = '" & jytsistema.WorkID & "' " _
                                  & "            GROUP BY a.numndb) c ON (a.codpro = c.codpro AND a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                                  & " LEFT JOIN jsconnumcon d on (a.nummov = d.numdoc and a.codpro = d.prov_cli and d.org = 'NDB' and d.origen = 'COM' and a.id_emp = d.id_emp) " _
                                  & " LEFT JOIN (SELECT a.codpro, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov AND a.codpro = e.codpro ) " _
                                  & " WHERE " _
                                  & " d.num_control <> '' and " _
                                  & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsprotrapag a WHERE a.codpro = '" & CodigoProveedor & "' GROUP BY nummov) AND " _
                                  & " a.nummov NOT IN (SELECT a.nummov FROM jsprotrapag a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codpro = '" & CodigoProveedor & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) AND " _
                                  & " a.codpro = '" & CodigoProveedor & "'  AND " _
                                  & " a.codpro <> '' AND " _
                                  & " c.impiva <> 0.00 AND " _
                                  & " e.saldo <> 0.00 AND " _
                                  & " a.REMESA = '" & Remesa & "' AND " _
                                  & " a.historico = '0' AND " _
                                  & " a.ID_EMP = '" & jytsistema.WorkID & "'" _
                                  & " ORDER BY nummov, emision "

        ds = DataSetRequery(ds, strSaldos, myConn, nTablaSaldos, lblInfo)
        dtSaldos = ds.Tables(nTablaSaldos)

        Dim aNombres() As String = {"N� Documento", "TP", "N� Control", "Emisi�n", "Vence", "Importe inicial Documentos", "Impuesto IVA"}
        Dim aCampos() As String = {"nummov", "tipomov", "num_control", "emision", "vence", "importe", "impiva"}
        Dim aAnchos() As Integer = {150, 40, 120, 100, 100, 120, 120}
        Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right}
        Dim aFormato() As FormatoItemListView = {FormatoItemListView.iCadena, FormatoItemListView.iCadena, FormatoItemListView.iCadena, FormatoItemListView.iFecha, FormatoItemListView.iFecha, FormatoItemListView.iNumero, FormatoItemListView.iNumero}

        CargaListViewDesdeTabla(lv, dtSaldos, aNombres, aCampos, aAnchos, aAlineacion, aFormato)

    End Sub

    Private Sub IniciarCajas()
        RellenaComboConDatatable(cmbCajaCR, dtCajas, "nomcaja", "caja")
    End Sub
    Private Sub IniciarDebitos(ByVal Tipo As Integer)

        ft.habilitarObjetos(False, True, txtDocumentoDB, txtCodigoContableDebitos)

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
            ft.mensajeCritico("DEBE INDICAR UN N� DE DOCUMENTO VALIDO...")
            Exit Sub
        Else
            Dim aCampos() As String = {"codpro", "tipomov", "nummov", "remesa", "id_emp"}
            Dim aStrings() As String = {CodigoProveedor, aTipoNick(Tipo), txtDocumentoDB.Text, Remesa, jytsistema.WorkID}
            If i_modo = movimiento.iAgregar AndAlso qFound(myConn, lblInfo, "jsprotrapag", aCampos, aStrings) Then
                ft.mensajeCritico("Documento YA se encuentra en movimientos de este proveedor ...")
                Exit Sub
            End If
        End If

        If txtEmisionDB.Value > txtVenceDB.Value Then
            ft.mensajeCritico("Fecha emisi�n mayor que fecha de vencimiento...")
            Exit Sub
        End If

        If Not ft.isNumeric(txtImporteDB.Text) Then
            ft.mensajeCritico("Debe indicar un n�mero v�lido para el importe...")
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
             aTipoNick(Tipo), Documento, txtEmisionDB.Value, ft.FormatoHora(Now()), txtVenceDB.Value,
             txtReferDB.Text, txtConceptoDB.Text, -1 * Math.Abs(ValorNumero(txtImporteDB.Text)), 0.0, "", "", "", "",
             "CXP", "", "", "", "", Documento, "0", "", jytsistema.sFechadeTrabajo, txtCodigoContableDebitos.Text, "0",
             "", 0.0, 0.0, "", "", "", Remesa, "", "", TipoProveedor, FOTipo.Debito, "0")

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
            ft.mensajeCritico("Debe indicar un porcentaje de retenci�n v�lido...")
            Exit Sub
        End If

        GuardarRetencionISLR()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        sRespuesta = MsgBox(" � Desea imprimir Comprobante de Retenci�n ?", MsgBoxStyle.YesNo, "Imprimir ... ")
        If sRespuesta = MsgBoxResult.Yes Then
            Dim f As New jsComRepParametros
            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cRetencionISLR, "Comprobante de Retenci�n de ISLR", CodigoProveedor, ComprobanteNumero)
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
            ft.mensajeCritico("Debe indicar un porcentaje de retenci�n v�lido...")
            Exit Sub
        End If

        GuardarRetencionIVA()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        sRespuesta = MsgBox(" � Desea imprimir Comprobante de Retenci�n ?", MsgBoxStyle.YesNo, "Imprimir ... ")
        If sRespuesta = MsgBoxResult.Yes Then
            Dim f As New jsComRepParametros
            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cRetencionIVA, "Comprobante de Retenci�n de IVA", CodigoProveedor, ComprobanteNumero)
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
        For gCont = 0 To lvRetISLR.Items.Count - 1
            With lvRetISLR.Items.Item(gCont)

                If .Checked Then

                    InsertEditCOMPRASCXP(myConn, lblInfo, Inserta, CodigoProveedor,
                         "NC", .Text, txtFechaRetISLR.Value, ft.FormatoHora(Now()), txtFechaRetISLR.Value,
                          Documento, cmbConceptoRetISLR.Text, ValorNumero(txtMontoRetencionISLR.Text), 0.0, "", "", "", "",
                         "CXP", "", "", "", "", Documento, "0", "", jytsistema.sFechadeTrabajo, txtCodigoContableISLR.Text, "0",
                         "", 0.0, 0.0, "", "", "", Remesa, "", "", TipoProveedor, FOTipo.Credito, "0")

                    ft.Ejecutar_strSQL(myConn, " update jsproenccom set " _
                                    & " ret_islr = " & ValorNumero(txtMontoRetencionISLR.Text) & ", " _
                                    & " num_ret_islr = '" & Documento & "', " _
                                    & " fecha_ret_islr = '" & ft.FormatoFechaMySQL(txtFechaRetISLR.Value) & "',  " _
                                    & " base_ret_islr = " & ValorNumero(txtMontoBaseRetISLR.Text) & ", " _
                                    & " por_ret_islr = " & ValorNumero(txtPorcentajeRetISLR.Text) & " " _
                                    & " where " _
                                    & " numcom = '" & .Text & "' and " _
                                    & " codpro = '" & CodigoProveedor & "' and " _
                                    & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "' ")

                    ft.Ejecutar_strSQL(myConn, " update jsproencgas set " _
                                    & " ret_islr = " & ValorNumero(txtMontoRetencionISLR.Text) & ", " _
                                    & " num_ret_islr = '" & Documento & "', " _
                                    & " fecha_ret_islr = '" & ft.FormatoFechaMySQL(txtFechaRetISLR.Value) & "',  " _
                                    & " base_ret_islr = " & ValorNumero(txtMontoBaseRetISLR.Text) & ", " _
                                    & " por_ret_islr = " & ValorNumero(txtPorcentajeRetISLR.Text) & " " _
                                    & " where " _
                                    & " numgas = '" & .Text & "' and " _
                                    & " codpro = '" & CodigoProveedor & "' and " _
                                    & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "' ")
                End If

            End With
        Next

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
        For gCont = 0 To lvRetIVA.Items.Count - 1
            With lvRetIVA.Items.Item(gCont)
                If .Checked Then

                    Dim CodigoDocumento As String = .Text

                    InsertEditCOMPRASCXP(myConn, lblInfo, Inserta, CodigoProveedor,
                         IIf(.SubItems(1).Text = "NC", "ND", "NC"), .Text, txtFechaRetIVA.Value, ft.FormatoHora(Now()), txtFechaRetIVA.Value,
                         Documento, "RETENCION IVA S/COMPROBANTE N� " & Documento, IIf(.SubItems(1).Text = "NC", -1, 1) * Math.Round(ValorNumero(.SubItems(6).Text) * ValorNumero(txtPorRetIVA.Text) / 100, 2), 0.0, "", "", "", "",
                         "CXP", "", "", "", "", Documento, "0", "", jytsistema.sFechadeTrabajo, txtCodigoContableIVA.Text, "0",
                         "", 0.0, 0.0, "", "", "", Remesa, "", "", TipoProveedor, IIf(.SubItems(1).Text = "NC", FOTipo.Debito, FOTipo.Credito), "0")

                    ft.Ejecutar_strSQL(myConn, " update jsproenccom set " _
                                    & " ret_iva = " & Math.Round(ValorNumero(.SubItems(6).Text) * ValorNumero(txtPorRetIVA.Text) / 100, 2) & ", " _
                                    & " num_ret_iva = '" & Documento & "', fecha_ret_iva = '" & ft.FormatoFechaMySQL(txtFechaRetIVA.Value) & "' " _
                                    & " where " _
                                    & " numcom = '" & CodigoDocumento & "' and " _
                                    & " codpro = '" & CodigoProveedor & "' and " _
                                    & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "' ")

                    ft.Ejecutar_strSQL(myConn, " update jsproivacom set " _
                                    & " retencion = round(impiva*" & ValorNumero(txtPorRetIVA.Text) / 100 & ", 2), " _
                                    & " numretencion = '" & Documento & "' " _
                                    & " where " _
                                    & " numcom = '" & .Text & "' and " _
                                    & " codpro = '" & CodigoProveedor & "' and " _
                                    & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "' ")

                    ft.Ejecutar_strSQL(myConn, " update jsproencgas set " _
                                    & " ret_iva = " & Math.Round(ValorNumero(.SubItems(6).Text) * ValorNumero(txtPorRetIVA.Text) / 100, 2) & ", " _
                                    & " num_ret_iva = '" & Documento & "', fecha_ret_iva = '" & ft.FormatoFechaMySQL(txtFechaRetIVA.Value) & "' " _
                                    & " where " _
                                    & " numgas = '" & .Text & "' and " _
                                    & " codpro = '" & CodigoProveedor & "' and " _
                                    & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "' ")

                    ft.Ejecutar_strSQL(myConn, " update jsproivagas set " _
                                    & " retencion = round(impiva*" & ValorNumero(txtPorRetIVA.Text) / 100 & ", 2), " _
                                    & " numretencion = '" & Documento & "' " _
                                    & " where " _
                                    & " numgas = '" & .Text & "' and " _
                                    & " codpro = '" & CodigoProveedor & "' and " _
                                    & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "' ")

                    ft.Ejecutar_strSQL(myConn, " update jsproivancr set " _
                                   & " retencion = -1*round(impiva*" & ValorNumero(txtPorRetIVA.Text) / 100 & ", 2), " _
                                   & " numretencion = '" & Documento & "' " _
                                   & " where " _
                                   & " numncr = '" & .Text & "' and " _
                                   & " codpro = '" & CodigoProveedor & "' and " _
                                   & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' ")

                    ft.Ejecutar_strSQL(myConn, " update jsproivandb set " _
                                   & " retencion = round(impiva*" & ValorNumero(txtPorRetIVA.Text) / 100 & ", 2), " _
                                   & " numretencion = '" & Documento & "' " _
                                   & " where " _
                                   & " numndb = '" & .Text & "' and " _
                                   & " codpro = '" & CodigoProveedor & "' and " _
                                   & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' ")

                End If
            End With
        Next

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
            ft.mensajeCritico("Debe indicar un n�mero de documento (comprobante) v�lido...")
            Exit Sub
        End If

        If Tipo = 5 Then
            If txtCausaNotaCredito.Text.Trim = "" Then
                ft.mensajeCritico("Debe indicar una CAUSA DE CREDITO V�lida...")
                Exit Sub
            Else
                If ft.DevuelveScalarBooleano(myConn, " select VALIDA_DOCUMENTOS from jsconcausas_notascredito where codigo = '" & txtCausaNotaCredito.Text & "' and origen = 'CXP' ") Then
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
            ft.mensajeCritico("Debe indicar un importe v�lido...")
            Exit Sub
        End If

        If txtNumPagCR.Text.Trim = "" AndAlso cmbFPCR.SelectedIndex >= 1 Then
            ft.mensajeCritico("Debe indicar un n�mero de pago v�lido...")
            Exit Sub
        End If

        If cmbNombrePago.SelectedValue = "" AndAlso cmbFPCR.SelectedIndex >= 1 Then
            ft.mensajeCritico("Debe indicar un nombre de pago v�lido ...")
            Exit Sub
        End If


        If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPARAM05")) Then
            If txtCodigoContable.Text.Trim = "" Then
                ft.mensajeCritico("Debe indicar CODIGO CONTABLE ...")
                Exit Sub
            End If
        End If

        GuardarCreditos(Tipo)


        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        sRespuesta = MsgBox(" � Desea imprimir Comprobante de Egreso ?", MsgBoxStyle.YesNo, "Imprimir ... ")
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
                    InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, cmbCajaCR.SelectedValue, UltimoCajaMasUno(myConn, lblInfo, cmbCajaCR.SelectedValue),
                        txtEmisionCR.Value, "CXP", "SA", Documento, cmbFPCR.SelectedValue,
                        txtNumPagCR.Text, NombreDePago, ValorNumero(txtImporteCR.Text), txtCodigoContable.Text, txtConceptoCR.Text, "", jytsistema.sFechadeTrabajo, 1,
                        "", "", "", jytsistema.sFechadeTrabajo, CodigoProveedor, "", "1", jytsistema.WorkCurrency.Id, DateTime.Now())
                Case 1, 2, 4, 5 'CH, TA, DP, TR
                    NombreDePago = cmbNombrePago.SelectedValue

                    InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, txtEmisionCR.Value, txtNumPagCR.Text,
                        IIf(cmbFPCR.SelectedIndex <> 1, "ND", "CH"), NombreDePago, "", txtConceptoCR.Text, ValorNumero(txtImporteCR.Text),
                        "CXP", Documento, txtBeneficiario.Text, Documento, "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo,
                         aTipoNick(Tipo), "", jytsistema.sFechadeTrabajo, "0", CodigoProveedor, "", jytsistema.WorkCurrency.Id, DateTime.Now())

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
        For jCont = 0 To lv.Items.Count - 1
            If lv.Items(jCont).Checked Then

                Dim MontoACancelar As Double = CDbl(lv.Items(jCont).SubItems(6).Text)
                Dim TipoDocumento As String = lv.Items(jCont).SubItems(1).Text
                NumeroDocumento = lv.Items(jCont).Text

                If MontoACancelar < 0 Then 'FC,GR,ND
                    If MontoNegativo > 0 Then


                        InsertEditCOMPRASCancelacion(myConn, lblInfo, True, CodigoProveedor, lv.Items(jCont).SubItems(1).Text,
                                                    NumeroDocumento, CDate(lv.Items(jCont).SubItems(3).Text), txtReferenciaCR.Text,
                                                    txtConceptoCR.Text, CDbl(lv.Items(jCont).SubItems(6).Text), Documento, "")


                        If TipoDocumento = "FC" Or TipoDocumento = "GR" Then

                            If Math.Abs(MontoACancelar) > MontoNegativo Then
                                InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, IIf(Tipo = 3 OrElse Tipo = 4, "AB", "NC"), NumeroDocumento,
                                     txtEmisionCR.Value, ft.FormatoHora(Now()), txtEmisionCR.Value,
                                    txtReferenciaCR.Text, txtConceptoCR.Text, Math.Abs(MontoNegativo), 0.0, cmbFPCR.SelectedValue,
                                    txtNumPagCR.Text, NombreDePago, txtBeneficiario.Text, "CXP", txtNumeroDeposito.Text, txtCuentaDeposito.Text,
                                    txtBancoDeposito.Text, cmbCajaCR.SelectedValue, Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), causaNotaCredito, jytsistema.sFechadeTrabajo,
                                    txtCodigoContable.Text, "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", Remesa, "", "", TipoProveedor, FOTipo.Credito, "0")


                            Else

                                InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, aTipoNick(Tipo), NumeroDocumento,
                                    txtEmisionCR.Value, ft.FormatoHora(Now), txtEmisionCR.Value,
                                    txtReferenciaCR.Text, txtConceptoCR.Text,
                                    Math.Abs(MontoACancelar), 0.0, cmbFPCR.SelectedValue,
                                    txtNumPagCR.Text, NombreDePago, txtBeneficiario.Text, "CXP", txtNumeroDeposito.Text, txtCuentaDeposito.Text,
                                    txtBancoDeposito.Text, cmbCajaCR.SelectedValue, Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), "", jytsistema.sFechadeTrabajo,
                                    txtCodigoContable.Text, "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", Remesa, "", "", TipoProveedor, FOTipo.Credito, "0")

                            End If


                        Else

                            If Math.Abs(MontoACancelar) > MontoNegativo Then

                                InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, "NC", NumeroDocumento,
                                            txtEmisionCR.Value, ft.FormatoHora(Now()), txtEmisionCR.Value,
                                            txtReferenciaCR.Text, txtConceptoCR.Text, Math.Abs(MontoNegativo), 0.0, cmbFPCR.SelectedValue,
                                            txtNumPagCR.Text, NombreDePago, txtBeneficiario.Text, "CXP", txtNumeroDeposito.Text, txtCuentaDeposito.Text,
                                            txtBancoDeposito.Text, cmbCajaCR.SelectedValue, Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), causaNotaCredito, jytsistema.sFechadeTrabajo,
                                            txtCodigoContable.Text, "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", Remesa, "", "", TipoProveedor, FOTipo.Credito, "0")


                            Else

                                InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, "NC", NumeroDocumento,
                                            txtEmisionCR.Value, ft.FormatoHora(Now()), txtEmisionCR.Value,
                                            txtReferenciaCR.Text, txtConceptoCR.Text, Math.Abs(MontoACancelar), 0.0, cmbFPCR.SelectedValue,
                                            txtNumPagCR.Text, NombreDePago, txtBeneficiario.Text, "CXP", txtNumeroDeposito.Text, txtCuentaDeposito.Text,
                                            txtBancoDeposito.Text, cmbCajaCR.SelectedValue, Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), causaNotaCredito, jytsistema.sFechadeTrabajo,
                                            txtCodigoContable.Text, "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", Remesa, "", "", TipoProveedor, FOTipo.Credito, "0")

                            End If

                        End If

                        MontoNegativo -= Math.Abs(MontoACancelar)

                    End If

                ElseIf MontoACancelar > 0 Then


                    If Math.Abs(MontoACancelar) > MontoPositivo Then

                        InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, "ND", NumeroDocumento,
                            txtEmisionCR.Value, ft.FormatoHora(Now()), txtEmisionCR.Value,
                            txtReferenciaCR.Text, txtConceptoCR.Text, -1 * Math.Abs(MontoPositivo),
                            0.0, cmbFPCR.SelectedValue, txtNumPagCR.Text, NombreDePago,
                            txtBeneficiario.Text, "CXP", txtNumeroDeposito.Text, txtCuentaDeposito.Text, txtBancoDeposito.Text,
                            cmbCajaCR.SelectedValue, Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), "", jytsistema.sFechadeTrabajo,
                            txtCodigoContable.Text, "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", Remesa, "", "",
                            TipoProveedor, FOTipo.Debito, "0")
                    Else
                        InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, "ND", NumeroDocumento,
                            txtEmisionCR.Value, ft.FormatoHora(Now()), txtEmisionCR.Value,
                            txtReferenciaCR.Text, txtConceptoCR.Text, -1 * Math.Abs(MontoACancelar),
                            0.0, cmbFPCR.SelectedValue, txtNumPagCR.Text, NombreDePago,
                            txtBeneficiario.Text, "CXP", txtNumeroDeposito.Text, txtCuentaDeposito.Text, txtBancoDeposito.Text,
                            cmbCajaCR.SelectedValue, Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), "", jytsistema.sFechadeTrabajo,
                            txtCodigoContable.Text, "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", Remesa, "", "",
                            TipoProveedor, FOTipo.Debito, "0")

                    End If

                    InsertEditCOMPRASCancelacion(myConn, lblInfo, True, CodigoProveedor, lv.Items(jCont).SubItems(1).Text,
                            NumeroDocumento, CDate(lv.Items(jCont).SubItems(3).Text), txtReferenciaCR.Text,
                            txtConceptoCR.Text, CDbl(lv.Items(jCont).SubItems(6).Text), Documento, "")


                    MontoPositivo -= CDbl(MontoACancelar)

                Else

                End If

                'Resto -= CDbl(lv.Items(jCont).SubItems(6).Text)
                'MODIFICA PROPIETARIO -> ULTIMO PAGO, FECHA ULTIMO PAGO , FORMA DE PAGO 

                ft.Ejecutar_strSQL(myConn, " update jsprocatpro set " _
                    & " ultpago = " & ValorNumero(txtImporteCR.Text) & ", " _
                    & " fecultpago = '" & ft.FormatoFechaMySQL(txtEmisionCR.Value) & "', " _
                    & " forultpago = '" & cmbFPCR.SelectedValue & "' " _
                    & " where " _
                    & " codpro = '" & CodigoProveedor & "' and " _
                    & " id_emp = '" & jytsistema.WorkID & "' ")

                'MODIFICA FACTURA QUE ORIGIN� LA CXP
                ft.Ejecutar_strSQL(myConn, " update jsproencgas set " _
                               & " formapag = '" & cmbFPCR.SelectedValue & "', " _
                               & " numpag = '" & txtNumPagCR.Text & "', " _
                               & " nompag = '" & NombreDePago & "', " _
                               & " benefic = '" & txtBeneficiario.Text & "', " _
                               & " caja = '" & cmbCajaCR.SelectedValue & "' " _
                               & " where " _
                               & " numgas = '" & NumeroDocumento & "' and " _
                               & " codpro = '" & CodigoProveedor & "' and " _
                               & " id_emp = '" & jytsistema.WorkID & "' ")

                ft.Ejecutar_strSQL(myConn, " update jsproenccom set " _
                               & " formapag = '" & cmbFPCR.SelectedValue & "', " _
                               & " numpag = '" & txtNumPagCR.Text & "', " _
                               & " nompag = '" & NombreDePago & "', " _
                               & " benefic = '" & txtBeneficiario.Text & "', " _
                               & " caja = '" & cmbCajaCR.SelectedValue & "' " _
                               & " where " _
                               & " numcom = '" & NumeroDocumento & "' and " _
                               & " codpro = '" & CodigoProveedor & "' and " _
                               & " id_emp = '" & jytsistema.WorkID & "' ")

            End If
        Next

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
                           cmbCajaCR.SelectedValue, Documento, sDocSel, causaNotaCredito, jytsistema.sFechadeTrabajo,
                           txtCodigoContable.Text, "", sDocCan, 0.0, 0.0, Documento, "", "", Remesa, "", "",
                           TipoProveedor, FOTipo.Credito, "0")
        End If

        InsertarAuditoria(myConn, IIf(i_modo = movimiento.iAgregar, MovAud.iIncluir, MovAud.imodificar), sModulo, Documento)
        ComprobanteNumero = Documento

    End Sub

    Private Sub txtImporteDB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtImporteDB.Click
        ft.enfocarTexto(sender)
    End Sub
    Private Sub lv_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lv.ItemChecked
        Dim strCon As String = aTipo(cmbTipo.SelectedIndex)
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

        txtDocSel.Text = ft.FormatoEntero(iSel)
        txtSaldoSel.Text = ft.FormatoNumero(dSel)
        txtConceptoCR.Text = strDocsIni & " " & IIf(iSel > 1, "DOCUMENTOS ", "DOCUMENTO ")
        If strDocs.Length >= 2 Then
            txtConceptoCR.Text = strDocsIni & " " & IIf(iSel > 1, "DOCUMENTOS ", "DOCUMENTO ") & strDocs.Substring(0, strDocs.Length - 2)
        End If

        txtImporteCR.Text = ft.FormatoNumero(dSel)

    End Sub

    Private Sub cmbFPCR_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFPCR.SelectedIndexChanged
        cmbNombrePago.DataSource = Nothing
        cmbNombrePago.Items.Clear()
        Select Case cmbFPCR.SelectedIndex
            Case 0 'EF
                ft.habilitarObjetos(False, True, txtNumPagCR, txtNomPagCR, btnNumPago, btnNomPago, cmbNombrePago, txtBeneficiario)
                ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNumPagCR, txtNomPagCR, txtBeneficiario)
            Case 1, 2, 3, 4, 5 'CH, TA, CT, DP, TR
                ft.habilitarObjetos(True, True, txtNumPagCR, btnNumPago, btnNomPago, cmbNombrePago, txtBeneficiario)
                ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNumPagCR, txtNomPagCR)

                If cmbFPCR.SelectedIndex = 1 Or cmbFPCR.SelectedIndex = 4 Or cmbFPCR.SelectedIndex = 5 Then
                    txtBeneficiario.Text = ft.DevuelveScalarCadena(myConn, " select nombre from jsprocatpro where codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                    IniciarNombrePago()

                End If

                If cmbFPCR.SelectedIndex = 2 Or cmbFPCR.SelectedIndex = 3 Then
                    ft.mensajeCritico("Formas de pago no aceptadas por los momentos...")
                    cmbFPCR.SelectedIndex = 0
                End If
        End Select
    End Sub
    Private Sub IniciarNombrePago()
        ds = DataSetRequery(ds, " select codban, concat(codban , '-', nomban, '-', ctaban) banco from jsbancatban where " _
                            & " estatus = 1 and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by codban ",
                              myConn, nTablaBancosInt, lblInfo)
        dtBancosInt = ds.Tables(nTablaBancosInt)
        RellenaComboConDatatable(cmbNombrePago, dtBancosInt, "banco", "codban")

    End Sub

    Private Sub txtImporteDB_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtImporteDB.KeyPress,
        txtImporteCR.KeyPress, txtPorRetIVA.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub lvRetIVA_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvRetIVA.ItemChecked
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

    Private Sub lvRetISLR_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lvRetISLR.ItemCheck
        If (lvRetISLR.CheckedItems.Count > 0 And e.NewValue = CheckState.Checked) Then

            Dim item As ListViewItem = lvRetISLR.CheckedItems(0)
            item.Checked = False
        End If
    End Sub

    Private Sub lvRetISLR_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvRetISLR.ItemChecked

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

    Private Sub cmbNombrePago_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbNombrePago.SelectedIndexChanged
        If cmbNombrePago.SelectedValue IsNot Nothing Then
            If cmbNombrePago.SelectedValue.ToString <> "" Then txtNombrePago.Text = cmbNombrePago.SelectedValue.ToString
        End If

    End Sub

    Private Sub btnCodigoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigoContable.Click
        txtCodigoContable.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables",
                                                   txtCodigoContable.Text)
    End Sub

    Private Sub btnCodigoContableIVA_Click(sender As System.Object, e As System.EventArgs) Handles btnCodigoContableIVA.Click
        txtCodigoContableIVA.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables", _
                                                   txtCodigoContableIVA.Text)
    End Sub

    Private Sub btnCodigoContableISLR_Click(sender As System.Object, e As System.EventArgs) Handles btnCodigoContableISLR.Click
        txtCodigoContableISLR.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables", _
                                                   txtCodigoContableISLR.Text)
    End Sub
    Private Sub btnCodigoContableDebitos_Click(sender As System.Object, e As System.EventArgs) Handles btnCodigoContableDebitos.Click
        txtCodigoContableDebitos.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables", _
                                                   txtCodigoContableDebitos.Text)
    End Sub

    Private Sub btnCausaNotaCredito_Click(sender As System.Object, e As System.EventArgs) Handles btnCausaNotaCredito.Click
        txtCausaNotaCredito.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descripcion  " _
                                            & " from jsconcausas_notascredito  where origen  = 'CXP' order by codigo ", _
                                            "CAUSAS NOTAS DE CREDITO", txtCausaNotaCredito.Text)
    End Sub

    Private Sub txtCausaNotaCredito_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCausaNotaCredito.TextChanged
        lblDescripCausaNotaCredito.Text = ft.DevuelveScalarCadena(myConn, " select descripcion from jsconcausas_notascredito where codigo = '" & txtCausaNotaCredito.Text & "' and origen = 'CXP' ")
        txtConceptoCR.Text += " " + lblDescripCausaNotaCredito.Text
        txtReferenciaCR.Text = ContadorNC_Financiera(myConn, txtCausaNotaCredito.Text, "CXP")
        causaNotaCredito = txtCausaNotaCredito.Text

        If ft.DevuelveScalarBooleano(myConn, " select CR from jsconcausas_notascredito where codigo = '" & txtCausaNotaCredito.Text & "' and origen = 'CXP' ") Then
            optCR.Checked = True
            grpPago.Enabled = False
        Else
            optCO.Checked = True
            grpPago.Enabled = True
        End If
    End Sub
End Class