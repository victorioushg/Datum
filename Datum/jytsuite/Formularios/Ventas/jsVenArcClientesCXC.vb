Imports MySql.Data.MySqlClient
Public Class jsVenArcClientesCXC
    Private Const sModulo As String = "Movimiento de cuentas por cobrar"

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

    Private strSQLCajas As String = "select caja, concat(caja,'-',nomcaja) nomcaja from jsbanenccaj where id_emp = '" & jytsistema.WorkID & "' "
  
    Private i_modo As Integer
    Private nPosicion As Long
    Private CodigoCliente As String

    Private aTipo() As String = {"Factura", "Giro", "Nota Débito", "Abono", "Cancelación", "Nota Crédito", "---", "Retención IVA", "Retención ISLR"}
    Private aTipoNick() As String = {"FC", "GR", "ND", "AB", "CA", "NC", "", "NC", "NC"}
    Private aContador() As String = {"vennummov", "vennummov", "vennumvdb", "vennumcan", "vennumcan", "vennumvcr", "", "", ""}
    Private aContadorModulo() As String = {"02", "02", "13", "11", "11", "12", "", "", ""}
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
    Private FechaEmisionDocumento As Date = jytsistema.sFechadeTrabajo
    Private strDocsRetIVA As String = " nummov = 'XX XX' OR "
    Private TotalRetencionIVA As Double = 0.0
    Private nTipoMovimientoCxC As Integer = 0 '0 = Debitos ; 1 = Creditos; 2 = Retenciones IVA; 3 = Retenciones ISLR
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
    Public Property TipoMovimientoCXC() As Integer
        Get
            Return nTipoMovimientoCxC
        End Get
        Set(ByVal value As Integer)
            nTipoMovimientoCxC = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal CodigoCli As String)

        i_modo = movimiento.iAgregar
        myConn = MyCon
        ds = dsMov
        dt = dtMov
        CodigoCliente = CodigoCli
        If dt.Rows.Count = 0 Then Apuntador = -1

        RellenaCombo(aTipo, cmbTipo, numTipoMovimiento)
        Me.ShowDialog()

    End Sub
    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal CodigoCli As String)

        i_modo = movimiento.iEditar
        myConn = MyCon
        ds = dsMov
        dt = dtMov
        CodigoCliente = CodigoCli

        numTipoMovimiento = numTipoMovimientoOrigen()
        RellenaCombo(aTipo, cmbTipo, numTipoMovimiento)
        cmbTipo.Enabled = False

        Me.ShowDialog()

    End Sub
    Private Function numTipoMovimientoOrigen() As Integer

        numTipoMovimientoOrigen = InArray(aTipoNick, dt.Rows(Apuntador).Item("tipomov")) - 1
        If numTipoMovimientoOrigen > 4 Then
            If Mid(dt.Rows(Apuntador).Item("REFER"), 1, 5) = "ISLR-" Then
                numTipoMovimientoOrigen = 8
            Else
                If Mid(dt.Rows(Apuntador).Item("CONCEPTO"), 1, 13) = "RETENCION IVA" Then
                    numTipoMovimientoOrigen = 7
                Else
                    numTipoMovimientoOrigen = 5
                End If
            End If
        End If

    End Function
    Private Sub jsVenArcClientesCXC_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ds = DataSetRequery(ds, strSQLCajas, myConn, nTablaCajas, lblInfo)
        dtCajas = ds.Tables(nTablaCajas)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        EjecutarSTRSQL(myConn, lblInfo, "DELETE FROM jsventabtic where NUMCAN = '" & txtDocumentoCR.Text & "' AND " _
                                    & " ID_EMP = '" & jytsistema.WorkID & "' ")
        Me.Close()
    End Sub

    Private Sub cmbTipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipo.SelectedIndexChanged
        iSel = 0
        dSel = 0
        Select Case cmbTipo.SelectedIndex
            Case 0, 1, 2
                nTipoMovimientoCxC = 0
                VisualizarObjetos(True, grpDebitos)
                VisualizarObjetos(False, grpCreditos, grpRetencionIVA, grpRetencionISLR)
                If i_modo = movimiento.iAgregar Then
                    IniciarDebitos(cmbTipo.SelectedIndex)
                Else
                    AsignarDebitos(Apuntador)
                End If
            Case 3, 4, 5

                If CBool(Parametro(myConn, lblInfo, Gestion.iVentas, "VENPARAM25")) AndAlso cmbTipo.SelectedIndex = 5 Then
                    MensajeCritico(lblInfo, "NO ES POSIBLE REALIZAR NOTAS CREDITO EN ESTE MOMENTO. CONSULTE CON UN SUPERVISOR... ")
                    cmbTipo.SelectedIndex = 0
                    Return
                End If
                nTipoMovimientoCxC = 1
                VisualizarObjetos(True, grpCreditos)
                VisualizarObjetos(False, grpDebitos, grpRetencionISLR, grpRetencionIVA)
                IniciarCreditos(cmbTipo.SelectedIndex)

            Case 7

                nTipoMovimientoCxC = 2
                VisualizarObjetos(True, grpRetencionIVA)
                VisualizarObjetos(False, grpDebitos, grpCreditos, grpRetencionISLR)
                IniciarRetencionIVA()
            Case 8
                nTipoMovimientoCxC = 3
                VisualizarObjetos(True, grpRetencionISLR)
                VisualizarObjetos(False, grpDebitos, grpCreditos, grpRetencionIVA)
                IniciarRetencionISLR()
        End Select
    End Sub
    Private Sub IniciarCreditos(ByVal Tipo As Integer)

        HabilitarObjetos(False, True, txtDocumentoCR, txtDocSel, txtSaldoSel, txtNumPagCR, txtNomPagCR, txtEmisionCR, txtDocSel, txtSaldoSel, _
                         cmbNombrePago, txtAsesorCR)

        If Tipo = 3 Then
            strDocsIni = "ABONO "
        ElseIf Tipo = 4 Then
            strDocsIni = "CANCELACION "
        Else
            strDocsIni = "NOTA CREDITO "
        End If

        txtDocumentoCR.Text = "TMP" & Format(NumeroAleatorio(1000000), 0)
        txtEmisionCR.Text = FormatoFechaMedia(jytsistema.sFechadeTrabajo)
        txtReferenciaCR.Text = ""
        txtConceptoCR.Text = strDocs
        txtSaldoSel.Text = FormatoNumero(0.0)
        txtImporteCR.Text = FormatoNumero(0.0)
        txtAsesorCR.Text = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, "select vendedor from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' "))
        txtDocSel.Text = "0"


        IniciarSaldos(lv)

        optCO.Checked = True

        If Tipo <> 5 Then
            HabilitarObjetos(False, False, grpCRCO)
        Else
            HabilitarObjetos(True, False, grpCRCO)
        End If
        IniciarCajas()

        If ChequesDevueltosPermitidos(myConn, lblInfo) > 0 AndAlso _
            ChequesDevueltosCliente(myConn, lblInfo, CodigoCliente) >= ChequesDevueltosPermitidos(myConn, lblInfo) Then
            RellenaCombo(DeleteArrayValue(aFormaPago, "Cheque"), cmbFPCR)
        Else
            RellenaCombo(aFormaPago, cmbFPCR)
        End If


    End Sub
    Private Sub IniciarRetencionIVA()

        HabilitarObjetos(False, True, txtSaldoRetIVA, txtTotalRetIVA, txtFechaComprobanteRetIVA, txtFechaRecepcionRetIVA, txtAsesorRetIVA)

        IniciarSaldosIVA(lvRetIVA)

        txtSaldoRetIVA.Text = FormatoNumero(0.0)
        txtPorcentajeRetIVA.Text = FormatoNumero(75)
        txtFechaComprobanteRetIVA.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        txtFechaRecepcionRetIVA.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        txtNumeroRetIVA.Text = ""
        txtAsesorRetIVA.Text = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select vendedor from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' "))

        lblLeyendaIVA.Text = "Si el documento al cual desea hacer retención del IVA no aparece : " & vbLf _
            & " 1. El Documento no posee número de control de IVA " & vbLf _
            & " 2. El Documento no posee renglones con tasa de impuesto mayor a CERO (0) " & vbLf _
            & " 3. El Documento YA posee una Retención Asignada." & vbLf _
            & " 4. El documento tiene saldo cero (0) (YA fué cancelado) "

    End Sub
    Private Function CalculaRetencionIVA() As Double
        Return Math.Round(ValorNumero(txtPorcentajeRetIVA.Text) / 100 * ValorNumero(txtSaldoRetIVA.Text), 2)
    End Function
    Private Sub lvRetIVA_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles _
        lvRetIVA.ItemChecked

        If e.Item.SubItems.Count >= 3 Then
            If e.Item.Checked Then
                strDocsRetIVA += " nummov = '" & e.Item.Text & "' OR "
                TotalRetencionIVA += CDbl(e.Item.SubItems(6).Text)
            Else
                strDocsRetIVA = Replace(strDocsRetIVA, " nummov = '" & e.Item.Text & "' OR ", "")
                TotalRetencionIVA -= CDbl(e.Item.SubItems(6).Text)
            End If

            Dim dtIVA As DataTable
            Dim tblIVA As String = "tblIVA"
            ds = DataSetRequery(ds, " SELECT a.tipoiva, a.poriva, SUM(a.baseiva) baseiva, SUM(a.impiva) impiva " _
                                        & " FROM (SELECT a.tipoiva, a.poriva, a.baseiva, a.impiva " _
                                        & "       	FROM jsvenivafac a " _
                                        & "         WHERE " _
                                        & Replace("(" & strDocsRetIVA.Substring(0, strDocsRetIVA.Length - 4) & ") and ", "nummov", "a.numfac") _
                                        & "     	id_emp = '" & jytsistema.WorkID & "' " _
                                        & " UNION " _
                                        & " 	  SELECT a.tipoiva, a.poriva, a.baseiva, a.impiva " _
                                        & "    	    FROM jsvenivapos a " _
                                        & "         WHERE " _
                                        & Replace("(" & strDocsRetIVA.Substring(0, strDocsRetIVA.Length - 4) & ") and ", "nummov", "a.numfac") _
                                        & "  	    id_emp = '" & jytsistema.WorkID & "' " _
                                        & " UNION " _
                                        & " 	  SELECT a.tipoiva, a.poriva, -a.baseiva, -a.impiva " _
                                        & "    	    FROM jsvenivancr a " _
                                        & "         WHERE " _
                                        & Replace("(" & strDocsRetIVA.Substring(0, strDocsRetIVA.Length - 4) & ") and ", "nummov", "a.numncr") _
                                        & "  	    id_emp = '" & jytsistema.WorkID & "'" _
                                        & " UNION " _
                                        & " 	  SELECT a.tipoiva, a.poriva, a.baseiva , a.impiva  " _
                                        & "    	    FROM jsvenivandb a " _
                                        & "         WHERE " _
                                        & Replace("(" & strDocsRetIVA.Substring(0, strDocsRetIVA.Length - 4) & ") and ", "nummov", "a.numndb") _
                                        & "  	    id_emp = '" & jytsistema.WorkID & "' ) a " _
                                        & " GROUP BY a.tipoiva ", myConn, tblIVA, lblInfo)

            dtIVA = ds.Tables(tblIVA)

            Dim aNombresIVA() As String = {"Tipo IVA", "Porcentaje IVA", "Base Imponible", "IVA a Retener", ""}
            Dim aCamposIVA() As String = {"tipoiva", "poriva", "baseiva", "impiva", ""}
            Dim aAnchosIVA() As Long = {70, 70, 100, 100, 100}
            Dim aAlineacionIVA() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                            AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                            AlineacionDataGrid.Izquierda}

            Dim aFormatosIVA() As String = {"", sFormatoNumero, sFormatoNumero, sFormatoNumero, ""}

            IniciarTabla(dgRetIVA, dtIVA, aCamposIVA, aNombresIVA, aAnchosIVA, aAlineacionIVA, aFormatosIVA)

            CalculaTotalRetIVA()

        End If

    End Sub
    Private Sub CalculaTotalRetIVA()
        txtSaldoRetIVA.Text = FormatoNumero(TotalRetencionIVA)
        txtTotalRetIVA.Text = FormatoNumero(Math.Round(ValorNumero(txtPorcentajeRetIVA.Text) / 100 * TotalRetencionIVA, 2))
    End Sub

    Private Sub txtPorRetIVA_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPorcentajeRetIVA.TextChanged
        CalculaTotalRetIVA()
    End Sub

    Private Sub ResetearVariablesDEUsoComun()
        iSel = 0
        dSel = 0
        MontoPositivo = 0
        MontoNegativo = 0
    End Sub


    Private Sub IniciarRetencionISLR()

        ResetearVariablesDEUsoComun()
        HabilitarObjetos(False, True, txtDocsSelRetISLR, txtSaldoSelRetISLR, txtImporteBaseRetISLR, _
                        txtFechaRetISLR, txtAsesorRetISLR)
        HabilitarObjetos(True, True, txtPorcentajeRetISLR, txtNumeroRetISLR, txtRetencionISLR)
        IniciarSaldosISLR(lvRetISLR)

        txtImporteBaseRetISLR.Text = FormatoNumero(0.0)
        txtPorcentajeRetISLR.Text = FormatoNumero(0.0)
        txtFechaRetISLR.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        txtNumeroRetISLR.Text = ""
        txtAsesorRetISLR.Text = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select vendedor from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' "))



    End Sub

    Private Sub IniciarSaldos(ByVal lv As ListView)

        Dim strSaldos As String = " SELECT a.codcli, b.nombre, a.nummov, a.tipomov, a.refer, a.emision, a.vence, a.importe, c.saldo, a.codven " _
                                  & " FROM jsventracob a " _
                                  & " LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
                                  & " LEFT JOIN (SELECT a.codcli, a.nummov, a.tipomov, SUM(a.importe) saldo, a.id_emp " _
                                  & "            FROM jsventracob a " _
                                  & "            WHERE " _
                                  & "            a.codcli = '" & CodigoCliente & "' AND " _
                                  & "            a.id_emp = '" & jytsistema.WorkID & "' " _
                                  & "            GROUP BY a.nummov) c ON (a.codcli = c.codcli AND a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                                  & " WHERE " _
                                  & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsventracob a WHERE a.codcli = '" & CodigoCliente & "' GROUP BY nummov) AND " _
                                  & " a.codcli = '" & CodigoCliente & "'  AND " _
                                  & " a.codcli <> '' AND " _
                                  & " (c.saldo > 0.001 OR c.saldo < -0.001) AND " _
                                  & " a.historico = '0' AND " _
                                  & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                  & " ORDER BY a.nummov, a.emision "

        ds = DataSetRequery(ds, strSaldos, myConn, nTablaSaldos, lblInfo)
        dtSaldos = ds.Tables(nTablaSaldos)

        Dim aNombres() As String = {"Nº Documento", "TP", "Referencia", "Emisión", "Vence", "Importe inicial", "Saldo", "Asesor Comercial"}
        Dim aCampos() As String = {"nummov", "tipomov", "refer", "emision", "vence", "importe", "saldo", "codven"}
        Dim aAnchos() As Integer = {150, 40, 120, 100, 100, 100, 100, 90}
        Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, HorizontalAlignment.Center}
        Dim aFormato() As FormatoItemListView = {FormatoItemListView.iCadena, FormatoItemListView.iCadena, FormatoItemListView.iCadena, FormatoItemListView.iFecha, FormatoItemListView.iFecha, FormatoItemListView.iNumero, FormatoItemListView.iNumero, FormatoItemListView.iCadena}

        CargaListViewDesdeTabla(lv, dtSaldos, aNombres, aCampos, aAnchos, aAlineacion, aFormato)


    End Sub
    Private Sub IniciarSaldosISLR(ByVal lv As ListView)

        Dim strSaldos As String = " SELECT a.codcli, b.nombre, a.nummov, a.tipomov, c.num_control, a.emision, a.vence, a.importe, c.saldo " _
                                  & " FROM jsventracob a " _
                                  & " LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
                                  & " LEFT JOIN (SELECT b.codcli, a.numfac nummov, c.num_control, SUM(a.totrendes) saldo, a.id_emp " _
                                                    & " FROM jsvenrenfac a " _
                                                    & " LEFT JOIN jsvenencfac b ON (a.numfac = b.numfac AND a.id_emp = b.id_emp) " _
                                                    & " LEFT JOIN jsconnumcon c ON (a.numfac = c.numdoc AND c.org = 'FAC' AND origen = 'FAC' AND a.id_emp = c.id_emp)" _
                                                    & " WHERE " _
                                                    & " b.codcli = '" & CodigoCliente & "' and " _
                                                    & " MID(a.item,1,1) = '$' AND " _
                                                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                                    & " GROUP BY b.codcli, a.numfac " _
                                                    & " UNION " _
                                                    & " SELECT b.codcli, a.numfac, c.num_control, SUM(a.totrendes) saldo, a.id_emp " _
                                                    & " FROM jsvenrenpos a " _
                                                    & " LEFT JOIN jsvenencpos b ON (a.numfac = b.numfac AND a.id_emp = b.id_emp) " _
                                                    & " LEFT JOIN jsconnumcon c ON (a.numfac = c.numdoc AND c.org = 'PVE' AND origen = 'FAC' AND a.id_emp = c.id_emp) " _
                                                    & " WHERE " _
                                                    & " b.codcli = '" & CodigoCliente & "' and " _
                                                    & " MID(a.item,1,1) = '$' AND " _
                                                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                                    & " GROUP BY b.codcli, a.numfac) c ON (a.codcli = c.codcli AND a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                                  & " WHERE " _
                                  & " a.tipomov = 'FC' and " _
                                  & " a.codcli = '" & CodigoCliente & "'  AND " _
                                  & " a.codcli <> '' AND " _
                                  & " (c.saldo > 0.001 OR c.saldo < -0.001) AND " _
                                  & " a.historico = '0' AND " _
                                  & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                  & " ORDER BY a.nummov, a.emision "

        '& " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsventracob a WHERE a.codcli = '" & CodigoCliente & "' GROUP BY nummov) AND " _
        ds = DataSetRequery(ds, strSaldos, myConn, nTablaSaldos, lblInfo)
        dtSaldos = ds.Tables(nTablaSaldos)

        Dim aNombres() As String = {"Nº Documento", "TP", "Nº Control", "Emisión", "Vence", "Importe inicial", "Por servicios"}
        Dim aCampos() As String = {"nummov", "tipomov", "num_control", "emision", "vence", "importe", "saldo"}
        Dim aAnchos() As Integer = {150, 40, 120, 100, 100, 120, 120}
        Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right}
        Dim aFormato() As FormatoItemListView = {FormatoItemListView.iCadena, FormatoItemListView.iCadena, FormatoItemListView.iCadena, FormatoItemListView.iFecha, FormatoItemListView.iFecha, FormatoItemListView.iNumero, FormatoItemListView.iNumero}

        CargaListViewDesdeTabla(lv, dtSaldos, aNombres, aCampos, aAnchos, aAlineacion, aFormato)


    End Sub
    Private Sub IniciarSaldosIVA(ByVal lv As ListView)

        Dim strSaldos As String = "SELECT a.codcli, b.nombre, a.nummov, a.tipomov, d.num_control, a.emision, a.vence, a.importe, a.importe/ABS(a.importe)*c.impiva impiva " _
                                  & " FROM jsventracob a " _
                                  & " LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
                                  & " LEFT JOIN (SELECT a.numFAC nummov, SUM(a.baseiva) baseiva, SUM(a.impiva) impiva, a.id_emp FROM jsvenivafac a WHERE a.tipoiva NOT IN ('', 'E') AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.numFAC) c ON (a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                                  & " LEFT JOIN jsconnumcon d ON (a.nummov = d.numdoc AND d.org = 'FAC' AND d.origen = 'FAC' AND a.id_emp = d.id_emp) " _
                                  & " LEFT JOIN (SELECT a.nummov, SUM(a.importe) saldo FROM jsventracob a WHERE a.codcli = '" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov) " _
                                  & " WHERE " _
                                  & " d.num_control <> '' AND " _
                                  & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsventracob a WHERE a.codcli = '" & CodigoCliente & "' GROUP BY nummov) AND " _
                                  & " a.nummov NOT IN (SELECT a.nummov FROM jsventracob a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codcli = '" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) AND " _
                                  & " e.saldo <> 0 AND " _
                                  & " a.codcli = '" & CodigoCliente & "'  AND " _
                                  & " a.codcli <> '' AND " _
                                  & " c.impiva <> 0.00 AND " _
                                  & " a.historico = '0' AND " _
                                  & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                  & " UNION " _
                                  & " SELECT a.codcli, b.nombre, a.nummov, a.tipomov, d.num_control, a.emision, a.vence, a.importe, a.importe/ABS(a.importe)*c.impiva impiva " _
                                  & " FROM jsventracob a " _
                                  & " LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
                                  & " LEFT JOIN (SELECT a.numfac nummov, SUM(a.baseiva) baseiva, SUM(a.impiva) impiva, a.id_emp FROM jsvenivapos a WHERE a.tipoiva NOT IN ('', 'E') AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.numfac) c ON (a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                                  & " LEFT JOIN jsconnumcon d ON (a.nummov = d.numdoc AND d.org = 'PVE' AND d.origen = 'FAC' AND a.id_emp = d.id_emp) " _
                                  & " LEFT JOIN (SELECT a.nummov, SUM(a.importe) saldo FROM jsventracob a WHERE a.codcli = '" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov) " _
                                  & " WHERE " _
                                  & " d.num_control <> '' AND " _
                                  & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsventracob a WHERE a.codcli = '" & CodigoCliente & "' GROUP BY nummov) AND " _
                                  & " a.nummov NOT IN (SELECT a.nummov FROM jsventracob a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codcli = '" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) AND " _
                                  & " e.saldo <> 0 AND " _
                                  & " a.codcli = '" & CodigoCliente & "'  AND " _
                                  & " a.codcli <> '' AND " _
                                  & " c.impiva <> 0.00 AND " _
                                  & " a.historico = '0' AND " _
                                  & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                  & " UNION " _
                                  & " SELECT a.codcli, b.nombre, a.nummov, a.tipomov, d.num_control, a.emision, a.vence, a.importe, a.importe/ABS(a.importe)*c.impiva impiva " _
                                  & " FROM jsventracob a " _
                                  & " LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
                                  & " LEFT JOIN (SELECT a.numndb nummov, SUM(a.baseiva) baseiva, SUM(a.impiva) impiva, a.id_emp FROM jsvenivandb a WHERE a.tipoiva NOT IN ('', 'E') AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.numndb) c ON (a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                                  & " LEFT JOIN jsconnumcon d ON (a.nummov = d.numdoc AND d.org = 'NDB' AND d.origen = 'FAC' AND a.id_emp = d.id_emp) " _
                                  & " LEFT JOIN (SELECT a.nummov, SUM(a.importe) saldo FROM jsventracob a WHERE a.codcli = '" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov) " _
                                  & " WHERE " _
                                  & " d.num_control <> '' AND " _
                                  & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsventracob a WHERE a.codcli = '" & CodigoCliente & "' GROUP BY nummov) AND " _
                                  & " a.nummov NOT IN (SELECT a.nummov FROM jsventracob a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codcli = '" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) AND " _
                                  & " e.saldo <> 0 AND " _
                                  & " a.codcli = '" & CodigoCliente & "'  AND " _
                                  & " a.codcli <> '' AND " _
                                  & " c.impiva <> 0.00 AND " _
                                  & " a.historico = '0' AND " _
                                  & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                  & " UNION " _
                                  & " SELECT a.codcli, b.nombre, a.nummov, a.tipomov, d.num_control, a.emision, a.vence, a.importe, a.importe/ABS(a.importe)*c.impiva impiva " _
                                  & " FROM jsventracob a " _
                                  & " LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
                                  & " LEFT JOIN (SELECT a.numncr nummov, SUM(a.baseiva) baseiva, SUM(a.impiva) impiva, a.id_emp FROM jsvenivancr a WHERE a.tipoiva NOT IN ('', 'E') AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.numncr) c ON (a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                                  & " LEFT JOIN jsconnumcon d ON (a.nummov = d.numdoc AND d.org = 'NCR' AND d.origen = 'FAC' AND a.id_emp = d.id_emp) " _
                                  & " LEFT JOIN (SELECT a.nummov, SUM(a.importe) saldo FROM jsventracob a WHERE a.codcli = '" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov) " _
                                  & " WHERE " _
                                  & " d.num_control <> '' AND " _
                                  & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) FROM jsventracob a WHERE a.codcli = '" & CodigoCliente & "' GROUP BY nummov) AND " _
                                  & " a.nummov NOT IN (SELECT a.nummov FROM jsventracob a WHERE a.tipomov = 'ND'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codcli = '" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) AND " _
                                  & " e.saldo <> 0 AND " _
                                  & " a.codcli = '" & CodigoCliente & "'  AND " _
                                  & " a.codcli <> '' AND " _
                                  & " c.impiva <> 0.00 AND " _
                                  & " a.historico = '0' AND " _
                                  & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                  & " ORDER BY nummov, emision "

        ds = DataSetRequery(ds, strSaldos, myConn, nTablaSaldos, lblInfo)
        dtSaldos = ds.Tables(nTablaSaldos)

        Dim aNombres() As String = {"Nº Documento", "TP", "Nº Control", "Emisión", "Vence", "Importe inicial", "Impuesto IVA"}
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

        HabilitarObjetos(False, True, txtDocumentoDB, txtEmisionDB, txtVenceDB, txtAsesor)

        txtDocumentoDB.Text = "TMP" & Format(NumeroAleatorio(1000000), 0)
        txtEmisionDB.Text = FormatoFechaMedia(jytsistema.sFechadeTrabajo)
        txtVenceDB.Text = FormatoFechaMedia(jytsistema.sFechadeTrabajo)
        txtReferDB.Text = ""
        txtConceptoDB.Text = ""
        txtImporteDB.Text = FormatoNumero(0.0)
        txtAsesor.Text = ""
        Dim Asesor As String = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select vendedor from jsvencatcli where codcli = '" _
                                              & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "'"))

        If Asesor = "0" Then Asesor = CStr(IIf(IsDBNull(EjecutarSTRSQL_Scalar(myConn, lblInfo, " SELECT b.codven FROM jsvenrenrut a " _
            & " LEFT JOIN jsvenencrut b ON ( a.codrut = b.codrut AND a.tipo = b.tipo AND a.id_emp = b.id_emp ) " _
            & " WHERE " _
            & " a.tipo = 0 AND a.cliente ='" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' ")), "0", EjecutarSTRSQL_Scalar(myConn, lblInfo, " SELECT b.codven FROM jsvenrenrut a " _
            & " LEFT JOIN jsvenencrut b ON ( a.codrut = b.codrut AND a.tipo = b.tipo AND a.id_emp = b.id_emp ) " _
            & " WHERE " _
            & " a.tipo = 0 AND a.cliente ='" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' ")))

        If Asesor <> "0" Then txtAsesor.Text = Asesor

    End Sub
    Private Sub AsignarDebitos(ByVal Puntero As Long)

        HabilitarObjetos(False, True, txtDocumentoDB, txtEmisionDB, btnEmisionDB, _
                         txtVenceDB, btnVenceDB, txtReferDB, txtConceptoDB, txtImporteDB, txtAsesor)

        With dt.Rows(Puntero)
            txtDocumentoDB.Text = .Item("nummov")
            txtEmisionDB.Text = FormatoFecha(CDate(.Item("emision").ToString))
            txtVenceDB.Text = FormatoFecha(CDate(.Item("vence").ToString))
            txtReferDB.Text = MuestraCampoTexto(.Item("refer"))
            txtConceptoDB.Text = MuestraCampoTexto(.Item("concepto"))
            txtImporteDB.Text = FormatoNumero(.Item("importe"))
            txtAsesor.Text = MuestraCampoTexto(.Item("codven"))
        End With


    End Sub

    Private Sub ValidarDebitos(ByVal Tipo As Integer)
        If txtDocumentoDB.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un Nº de documento válido")
            Exit Sub
        Else
            Dim aCampos() As String = {"codpro", "tipomov", "nummov", "id_emp"}
            Dim aStrings() As String = {Codigocliente, aTipoNick(Tipo), txtDocumentoDB.Text, jytsistema.WorkID}
            If i_modo = movimiento.iAgregar AndAlso qFound(myConn, lblInfo, "jsprotrapag", aCampos, aStrings) Then
                MensajeCritico(lblInfo, "Documento YA se encuentra en movimientos de este proveedor ...")
                Exit Sub
            End If
        End If

        If CDate(txtEmisionDB.Text) > CDate(txtVenceDB.Text) Then
            MensajeCritico(lblInfo, "Fecha emisión mayor que fecha de vencimiento...")
            Exit Sub
        End If

        If Not IsNumeric(txtImporteDB.Text) Then
            MensajeCritico(lblInfo, "Debe indicar un número válido para el importe...")
            Exit Sub
        End If

        If txtAsesor.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un asesor válido...")
            Exit Sub
        End If

        GuardarDebito(Tipo)

        SaldoCxC(myConn, lblInfo, CodigoCliente)

        Me.Close()

    End Sub
    Private Sub GuardarDebito(ByVal Tipo As Integer)

        Dim Inserta As Boolean = False
        Apuntador = Me.BindingContext(ds, dt.TableName).Position
        Dim Documento As String = txtDocumentoDB.Text

        If i_modo = movimiento.iAgregar Then
            Inserta = True
            Apuntador = dt.Rows.Count
            Documento = Contador(myConn, lblInfo, Gestion.iVentas, aContador(Tipo), aContadorModulo(Tipo))
        End If

        InsertEditVENTASCXC(myConn, lblInfo, Inserta, CodigoCliente, aTipoNick(Tipo), Documento, CDate(txtEmisionDB.Text), _
                            FormatoHora(Now()), CDate(txtVenceDB.Text), txtReferDB.Text, txtConceptoDB.Text, ValorNumero(txtImporteDB.Text), _
                            0.0, "", "", "", "", "", "CXC", Documento, "0", "", jytsistema.sFechadeTrabajo, "", "0", "", 0.0, 0.0, "", "", "", _
                            "", txtAsesor.Text, txtAsesor.Text, "0", FOTipo.Debito, jytsistema.WorkDivition)

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

        If ValorNumero(txtSaldoSelRetISLR.Text) = 0 Then
            MensajeCritico(lblInfo, "Debe seleccionar al menos un documento...")
            Exit Sub
        End If

        If Not (ValorNumero(txtPorcentajeRetISLR.Text) > 0 Or ValorNumero(txtPorcentajeRetISLR.Text) <= 100) Then
            MensajeCritico(lblInfo, "Debe indicar un porcentaje de retención válido...")
            Exit Sub
        End If

        If txtNumeroRetISLR.Text.Trim() = "" Then
            MensajeCritico(lblInfo, "Debe indicar un número de comprobante de retención válido...")
            Exit Sub
        End If

        GuardarRetencionISLR()


        '    Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        '   sRespuesta = MsgBox(" ¿ Desea imprimir Comprobante de Retención ?", MsgBoxStyle.YesNo, "Imprimir ... ")
        '  If sRespuesta = MsgBoxResult.Yes Then
        'Dim f As New jsComRepParametros
        'Select Case cmbFPCR.SelectedIndex
        '    Case 0, 2, 3, 4, 5 'EF
        'f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cComprobantePago, "COMPROBANTE DE EGRESOS", CodigoProveedor, ComprobanteNumero)
        '    Case 1 'CH
        '
        '           End Select
        '          f = Nothing
        'End If
        '     SaldoCxP(myConn, lblInfo, CodigoProveedor)
        Me.Close()

    End Sub
    Private Sub ValidarRetencionIVA()

        If ValorNumero(txtSaldoRetIVA.Text) = 0 Then
            MensajeCritico(lblInfo, "Debe seleccionar al menos un documento...")
            Exit Sub
        End If

        If Not (ValorNumero(txtPorcentajeRetIVA.Text) = 75 Or ValorNumero(txtPorcentajeRetIVA.Text) = 100) Then
            MensajeCritico(lblInfo, "Debe indicar un porcentaje de retención válido...")
            Exit Sub
        End If

        If txtNumeroRetIVA.Text.Trim() = "" Then
            MensajeCritico(lblInfo, "Debe indicar un número de comprobante de retención válido...")
            Exit Sub
        End If

        GuardarRetencionIVA()


        '    Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        '   sRespuesta = MsgBox(" ¿ Desea imprimir Comprobante de Retención ?", MsgBoxStyle.YesNo, "Imprimir ... ")
        '  If sRespuesta = MsgBoxResult.Yes Then
        'Dim f As New jsComRepParametros
        'Select Case cmbFPCR.SelectedIndex
        '    Case 0, 2, 3, 4, 5 'EF
        'f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cComprobantePago, "COMPROBANTE DE EGRESOS", CodigoProveedor, ComprobanteNumero)
        '    Case 1 'CH
        '
        '           End Select
        '          f = Nothing
        'End If
        '     SaldoCxP(myConn, lblInfo, CodigoProveedor)
        Me.Close()

    End Sub
    Private Sub GuardarRetencionISLR()

        Dim Inserta As Boolean = False
        Apuntador = Me.BindingContext(ds, dt.TableName).Position
        Dim Documento As String = ""

        If i_modo = movimiento.iAgregar Then
            Inserta = True
            Apuntador = dt.Rows.Count
            Documento = txtNumeroRetISLR.Text
        End If

        Dim gCont As Integer
        For gCont = 0 To lvRetISLR.Items.Count - 1
            With lvRetISLR.Items.Item(gCont)
                If .Checked Then

                    InsertEditVENTASCXC(myConn, lblInfo, Inserta, CodigoCliente, _
                         IIf(.SubItems(1).Text = "NC", "ND", "NC"), .Text, CDate(txtFechaRetISLR.Text), FormatoHora(Now()), CDate(txtFechaRetISLR.Text), _
                         Documento, "RETENCION I.S.L.R.", IIf(.SubItems(1).Text = "NC", 1, -1) * ValorNumero(txtRetencionISLR.Text), 0.0, "", "", "", "", _
                         "", "CXC", .Text, "0", "", jytsistema.sFechadeTrabajo, "", "0", "", _
                         ValorNumero(txtPorcentajeRetISLR.Text), 0.0, "", "", "", "", txtAsesorRetISLR.Text, txtAsesorRetISLR.Text, "0", IIf(.SubItems(1).Text = "NC", FOTipo.Debito, FOTipo.Credito), "")


                End If
            End With
        Next

        ComprobanteNumero = Documento
        InsertarAuditoria(myConn, IIf(Inserta, MovAud.iIncluir, MovAud.imodificar), sModulo, Documento)

    End Sub
    Private Sub GuardarRetencionIVA()

        Dim Inserta As Boolean = False
        Apuntador = Me.BindingContext(ds, dt.TableName).Position
        Dim Documento As String = ""

        If i_modo = movimiento.iAgregar Then
            Inserta = True
            Apuntador = dt.Rows.Count
            Documento = txtNumeroRetIVA.Text
        End If

        Dim gCont As Integer
        For gCont = 0 To lvRetIVA.Items.Count - 1
            With lvRetIVA.Items.Item(gCont)
                If .Checked Then

                    InsertEditVENTASCXC(myConn, lblInfo, Inserta, CodigoCliente, _
                         IIf(.SubItems(1).Text = "NC", "ND", "NC"), .Text, CDate(txtFechaRecepcionRetIVA.Text), FormatoHora(Now()), CDate(txtFechaRecepcionRetIVA.Text), _
                         Documento, "RETENCION IVA CLIENTE ESPECIAL", IIf(.SubItems(1).Text = "NC", 1, -1) * Math.Round(Math.Abs(ValorNumero(.SubItems(6).Text)) * ValorNumero(txtPorcentajeRetIVA.Text) / 100, 2), 0.0, "", "", "", "", _
                         "", "CXC", .Text, "0", "", CDate(txtFechaComprobanteRetIVA.Text), "", "0", "", _
                         0.0, 0.0, "", "", "", "", txtAsesorRetIVA.Text, txtAsesorRetIVA.Text, "0", IIf(.SubItems(1).Text = "NC", FOTipo.Debito, FOTipo.Credito), "")

  
                End If
            End With
        Next

        ComprobanteNumero = Documento
        InsertarAuditoria(myConn, IIf(Inserta, MovAud.iIncluir, MovAud.imodificar), sModulo, Documento)

    End Sub

    Private Sub ValidarCreditos(ByVal Tipo As Integer)

        If ValorEntero(txtDocSel.Text) = 0 And Tipo < 5 Then
            MensajeCritico(lblInfo, "Debe seleccionar al menos un documento...")
            Exit Sub
        End If

        If Trim(txtDocumentoCR.Text) = "" Then
            MensajeCritico(lblInfo, "Debe indicar un número de documento (comprobante) válido...")
            Exit Sub
        End If

        If IsNumeric(txtImporteCR.Text) Then
            If ValorNumero(txtImporteCR.Text) < 0 Then
                MensajeCritico(lblInfo, "Debe indicar un importe MAYOR O IGUAL QUE CERO")
                Exit Sub
            Else
                If CBool(Parametro(myConn, lblInfo, Gestion.iVentas, "VENPARAM10")) Then
                    If ValorNumero(txtImporteCR.Text) > ValorNumero(txtSaldoSel.Text) Then
                        If cmbTipo.SelectedIndex = 5 Then 'NOTA DE CREDITO
                        Else
                            If optCO.Checked Then
                            Else
                                MensajeCritico(lblInfo, "EL MONTO A CANCELAR NO DEBE EXCEDER AL MONTO TOTAL DE DOCUMENTOS SELECCIONADOS ... ")
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            End If
        Else
            MensajeCritico(lblInfo, "Debe indicar un importe válido...")
            Exit Sub
        End If

        If txtAsesorCR.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un asesor comercial válido...")
            Exit Sub
        End If

        If txtNumPagCR.Text = "" AndAlso cmbFPCR.SelectedIndex >= 1 Then
            MensajeCritico(lblInfo, "Debe indicar un número de pago válido...")
            Exit Sub
        End If

        If txtNomPagCR.Text = "" AndAlso cmbFPCR.SelectedIndex >= 1 Then
            MensajeCritico(lblInfo, "Debe indicar un nombre de pago válido ...")
            Exit Sub
        End If

        If cmbFPCR.Text = "Cupón de Alimentación" AndAlso CBool(Parametro(myConn, lblInfo, Gestion.iVentas, "VENPARAM09")) Then
            If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " SELECT COUNT(*) CUENTA FROM jsventabtic WHERE NUMCAN = '" _
                                          & txtDocumentoCR.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")) = 0 Then
                MensajeCritico(lblInfo, "Debe incluir los codigos de tickets pertenecientes a esta cancelación... ")
                Exit Sub
            End If
        End If

        If cmbFPCR.Text = "Cheque" Then
            If CDate(txtEmisionCR.Text) > jytsistema.sFechadeTrabajo Then
                Dim DiasPostDate As Integer = Math.Abs(DateDiff(DateInterval.Day, CDate(txtEmisionCR.Text), CDate(FormatoFecha(jytsistema.sFechadeTrabajo))))
                If DiasPostDate > 0 AndAlso CBool(Parametro(myConn, lblInfo, Gestion.iVentas, "VENPARAM17")) Then
                    Dim MaxDiasPostDate As Integer = CInt(Parametro(myConn, lblInfo, Gestion.iVentas, "VENPARAM18"))
                    If DiasPostDate > MaxDiasPostDate Then
                        MensajeCritico(lblInfo, "Se aceptan cheques posdatados pero con un máximo de " & CStr(MaxDiasPostDate) _
                                       & " días a partir de hoy... ")
                        Exit Sub
                    End If
                Else

                    MensajeCritico(lblInfo, "No se aceptan cancelaciones con cheques posdatados...")
                    Exit Sub
                End If
            End If
        End If

        GuardarCreditos(Tipo)

        DesbloqueoDeCliente(myConn, lblInfo, ds, CodigoCliente, jytsistema.sFechadeTrabajo, 8, "00002", "CANCELACION DE FACTURA PENDIENTE")

        'Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        'sRespuesta = MsgBox(" ¿ Desea imprimir Comprobante de Egreso ?", MsgBoxStyle.YesNo, "Imprimir ... ")
        'If sRespuesta = MsgBoxResult.Yes Then
        ' Dim f As New jsComRepParametros
        ' Select Case cmbFPCR.SelectedIndex
        '     Case 0, 2, 3, 4, 5 'EF
        ' f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cComprobantePago, "COMPROBANTE DE EGRESOS", CodigoCliente, ComprobanteNumero)
        '     Case 1 'CH
        '
        ' End Select
        ' f = Nothing
        ' End If
        SaldoCxC(myConn, lblInfo, CodigoCliente)
        Me.Close()

    End Sub

    Private Sub GuardarCreditos(ByVal Tipo As Integer)
        Dim Inserta As Boolean = False
        Dim jCont As Integer
        Dim Resto As Double = 0.0
        Dim Documento As String = txtDocumentoCR.Text


        If i_modo = movimiento.iAgregar Then
            Inserta = True
            Apuntador = dt.Rows.Count
            Documento = Contador(myConn, lblInfo, Gestion.iVentas, aContador(Tipo), aContadorModulo(Tipo))
        End If

        If optCO.Checked Then
            Select Case cmbFPCR.Text
                Case "Efectivo", "Cheque", "Tarjeta", "Cupón de Alimentación"
                    If cmbFPCR.Text = "Cupón de Alimentación" Then
                        Dim dtCantidad As DataTable
                        Dim nTablaCantidad As String = "tblCantidad"
                        ds = DataSetRequery(ds, "select CORREDOR, count(*) AS CANTIDAD, SUM(MONTO) AS IMPORTE from jsventabtic WHERE " _
                                                & " ID_EMP = '" & jytsistema.WorkID & "' AND" _
                                                & " NUMCAN = '" & txtDocumentoCR.Text & "' GROUP BY CORREDOR ORDER BY CORREDOR  ", myConn, nTablaCantidad, lblInfo)

                        dtCantidad = ds.Tables(nTablaCantidad)

                        If dtCantidad.Rows.Count > 0 Then
                            Dim fCont As Integer
                            For fCont = 0 To dtCantidad.Rows.Count - 1
                                With dtCantidad.Rows(fCont)
                                    InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, cmbCajaCR.SelectedValue, UltimoCajaMasUno(myConn, lblInfo, cmbCajaCR.SelectedValue), _
                                                                CDate(txtEmisionCR.Text), "CXC", "EN", Documento, _
                                                                aFormaPagoAbreviada(InArray(aFormaPago, cmbFPCR.Text) - 1), txtNumPagCR.Text, _
                                                                .Item("CORREDOR"), .Item("IMPORTE"), "", txtConceptoCR.Text, "", jytsistema.sFechadeTrabajo, _
                                                                .Item("CANTIDAD"), "", "", "", jytsistema.sFechadeTrabajo, CodigoCliente, txtAsesorCR.Text, "1")

                                End With
                            Next
                        End If
                        dtCantidad.Dispose()
                        dtCantidad = Nothing

                        EjecutarSTRSQL(myConn, lblInfo, " update jsventabtic set numcan = '" & Documento & "' where numcan = '" & txtDocumentoCR.Text & "' ")

                    Else
                        If ValorNumero(txtImporteCR.Text) > 0 Then _
                            InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, cmbCajaCR.SelectedValue, UltimoCajaMasUno(myConn, lblInfo, cmbCajaCR.SelectedValue), _
                                                        CDate(txtEmisionCR.Text), "CXC", "EN", Documento, _
                                                        aFormaPagoAbreviada(InArray(aFormaPago, cmbFPCR.Text) - 1), txtNumPagCR.Text, cmbNombrePago.SelectedValue, _
                                                        ValorNumero(txtImporteCR.Text), "", txtConceptoCR.Text, "", jytsistema.sFechadeTrabajo, 1, "", "", _
                                                        "", jytsistema.sFechadeTrabajo, CodigoCliente, txtAsesorCR.Text, "1")
                    End If



                Case "Depósito", "Transferencia"

                    InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, CDate(txtEmisionCR.Text), txtNumPagCR.Text, _
                                                    "NC", cmbNombrePago.SelectedValue.ToString, "", txtConceptoCR.Text, ValorNumero(txtImporteCR.Text), _
                                                    "CXC", Documento, "", "", "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, _
                                                    aTipoNick(Tipo), "", jytsistema.sFechadeTrabajo, "0", CodigoCliente, txtAsesorCR.Text)
            End Select
        End If

        If ValorNumero(txtImporteCR.Text) >= ValorNumero(txtSaldoSel.Text) Then
            Resto = ValorNumero(txtImporteCR.Text) - ValorNumero(txtSaldoSel.Text)
        ElseIf ValorNumero(txtImporteCR.Text) < ValorNumero(txtSaldoSel.Text) Then
            MontoPositivo = ValorNumero(txtImporteCR.Text) - MontoNegativo
        End If

        Dim FormaDePago As String = aFormaPagoAbreviada(InArray(aFormaPago, cmbFPCR.Text) - 1)

        MontoPositivo = Math.Abs(MontoPositivo)
        MontoNegativo = Math.Abs(MontoNegativo)

        For jCont = 0 To lv.Items.Count - 1
            If lv.Items(jCont).Checked Then
                Dim MontoACancelar As Double = CDbl(lv.Items(jCont).SubItems(6).Text)
                Dim TipoDocumento As String = lv.Items(jCont).SubItems(1).Text

                If MontoACancelar > 0 Then 'FC,GR,ND
                    If MontoPositivo > 0 Then

                        InsertEditVENTASCancelacion(myConn, lblInfo, True, CodigoCliente, lv.Items(jCont).SubItems(1).Text, _
                                lv.Items(jCont).Text, CDate(lv.Items(jCont).SubItems(3).Text), txtReferenciaCR.Text, _
                                txtConceptoCR.Text, MontoACancelar, Documento, txtAsesorCR.Text)

                        If TipoDocumento = "FC" Or TipoDocumento = "GR" Then

                            If MontoACancelar > MontoPositivo Then

                                InsertEditVENTASCXC(myConn, lblInfo, True, CodigoCliente, IIf(Tipo = 3 OrElse Tipo = 4, "AB", "NC"), lv.Items(jCont).Text, _
                                    CDate(txtEmisionCR.Text), FormatoHora(Now), CDate(txtEmisionCR.Text), _
                                    txtReferenciaCR.Text, txtConceptoCR.Text, -1 * (MontoPositivo), 0.0, _
                                    FormaDePago, cmbCajaCR.SelectedValue, txtNumPagCR.Text, txtNomPagCR.Text, txtBeneficiario.Text, "CXC", _
                                    Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), "", jytsistema.sFechadeTrabajo, _
                                    "", "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", "", txtAsesorCR.Text, _
                                    txtAsesorCR.Text, "0", FOTipo.Credito, "")

                            Else

                                InsertEditVENTASCXC(myConn, lblInfo, True, CodigoCliente, aTipoNick(Tipo), lv.Items(jCont).Text, _
                                    CDate(txtEmisionCR.Text), FormatoHora(Now), CDate(txtEmisionCR.Text), _
                                    txtReferenciaCR.Text, txtConceptoCR.Text, -1 * (Math.Abs(MontoACancelar)), 0.0, _
                                    FormaDePago, cmbCajaCR.SelectedValue, txtNumPagCR.Text, txtNomPagCR.Text, txtBeneficiario.Text, "CXC", _
                                    Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), "", jytsistema.sFechadeTrabajo, _
                                    "", "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", "", txtAsesorCR.Text, _
                                    txtAsesorCR.Text, "0", FOTipo.Credito, "")
                            End If

                        Else

                            If MontoACancelar > MontoPositivo Then

                                InsertEditVENTASCXC(myConn, lblInfo, True, CodigoCliente, "NC", lv.Items(jCont).Text, _
                                    CDate(txtEmisionCR.Text), FormatoHora(Now), CDate(txtEmisionCR.Text), _
                                    txtReferenciaCR.Text, txtConceptoCR.Text, -1 * (MontoPositivo), 0.0, _
                                    FormaDePago, cmbCajaCR.SelectedValue, txtNumPagCR.Text, txtNomPagCR.Text, txtBeneficiario.Text, "CXC", _
                                    Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), "", jytsistema.sFechadeTrabajo, _
                                    "", "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", "", txtAsesorCR.Text, _
                                    txtAsesorCR.Text, "0", FOTipo.Credito, "")

                            Else

                                InsertEditVENTASCXC(myConn, lblInfo, True, CodigoCliente, "NC", lv.Items(jCont).Text, _
                                    CDate(txtEmisionCR.Text), FormatoHora(Now), CDate(txtEmisionCR.Text), _
                                    txtReferenciaCR.Text, txtConceptoCR.Text, -1 * Math.Abs(MontoACancelar), 0.0, _
                                    FormaDePago, cmbCajaCR.SelectedValue, txtNumPagCR.Text, txtNomPagCR.Text, txtBeneficiario.Text, "CXC", _
                                    Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), "", jytsistema.sFechadeTrabajo, _
                                    "", "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", "", txtAsesorCR.Text, _
                                    txtAsesorCR.Text, "0", FOTipo.Credito, "")
                            End If



                        End If

                        MontoPositivo -= Math.Abs(MontoACancelar)

                    End If

                ElseIf MontoACancelar < 0 Then

                    If Math.Abs(MontoACancelar) > MontoNegativo Then

                        InsertEditVENTASCXC(myConn, lblInfo, True, CodigoCliente, "ND", lv.Items(jCont).Text, _
                                CDate(txtEmisionCR.Text), FormatoHora(Now()), CDate(txtEmisionCR.Text), txtReferenciaCR.Text, txtConceptoCR.Text, _
                                 Math.Abs(MontoNegativo), 0.0, FormaDePago, cmbCajaCR.SelectedValue, txtNumPagCR.Text, _
                                 txtNomPagCR.Text, txtBeneficiario.Text, "CXC", Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), "", _
                                 jytsistema.sFechadeTrabajo, "", "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", "", _
                                 txtAsesorCR.Text, txtAsesorCR.Text, "0", FOTipo.Debito, "")
                    Else

                        InsertEditVENTASCXC(myConn, lblInfo, True, CodigoCliente, "ND", lv.Items(jCont).Text, _
                                CDate(txtEmisionCR.Text), FormatoHora(Now()), CDate(txtEmisionCR.Text), txtReferenciaCR.Text, txtConceptoCR.Text, _
                                 Math.Abs(MontoACancelar), 0.0, FormaDePago, cmbCajaCR.SelectedValue, txtNumPagCR.Text, _
                                 txtNomPagCR.Text, txtBeneficiario.Text, "CXC", Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), "", _
                                 jytsistema.sFechadeTrabajo, "", "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", "", _
                                 txtAsesorCR.Text, txtAsesorCR.Text, "0", FOTipo.Debito, "")

                    End If

                    InsertEditVENTASCancelacion(myConn, lblInfo, True, CodigoCliente, lv.Items(jCont).SubItems(1).Text, _
                            lv.Items(jCont).Text, CDate(lv.Items(jCont).SubItems(3).Text), txtReferenciaCR.Text, _
                            txtConceptoCR.Text, MontoACancelar, Documento, txtAsesorCR.Text)


                    MontoNegativo -= Math.Abs(MontoACancelar)
                Else

                End If

                'Resto -= CDbl(MontoACancelar)

                'MODIFICA CLIENTE -> ULTIMO PAGO, FECHA ULTIMO PAGO , FORMA DE PAGO 

                EjecutarSTRSQL(myConn, lblInfo, " update jsvencatcli set " _
                        & " ultcobro = " & ValorNumero(txtImporteCR.Text) & ", " _
                        & " fecultcobro = '" & FormatoFechaMySQL(CDate(txtEmisionCR.Text)) & "', " _
                        & " forultcobro = '" & FormaDePago & "' " _
                        & " where " _
                        & " codcli = '" & CodigoCliente & "' and " _
                        & " id_emp = '" & jytsistema.WorkID & "' ")

                'MODIFICA FACTURA QUE ORIGINÓ LA CXC
                EjecutarSTRSQL(myConn, lblInfo, " update jsvenencfac set " _
                                   & " formapag = '" & FormaDePago & "', " _
                                   & " numpag = '" & txtNumPagCR.Text & "', " _
                                   & " nompag = '" & cmbNombrePago.Text & "', " _
                                   & " caja = '" & cmbCajaCR.SelectedValue & "' " _
                                   & " where " _
                                   & " numfac = '" & lv.Items(jCont).Text & "' and " _
                                   & " codcli = '" & CodigoCliente & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' ")

            End If

        Next




        If Resto > 0 Then

            InsertEditVENTASCXC(myConn, lblInfo, True, CodigoCliente, "NC", Contador(myConn, lblInfo, Gestion.iVentas, "VENNUMVDB", "13"), _
                           CDate(txtEmisionCR.Text), FormatoHora(Now()), CDate(txtEmisionCR.Text), txtReferenciaCR.Text, txtConceptoCR.Text, _
                            -1 * Resto, 0.0, FormaDePago, cmbCajaCR.SelectedValue, txtNumPagCR.Text, _
                            txtNomPagCR.Text, txtBeneficiario.Text, "CXC", Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), "", _
                            jytsistema.sFechadeTrabajo, "", "", "", 0.0, 0.0, Documento, "", "", "", _
                            txtAsesorCR.Text, txtAsesorCR.Text, "0", FOTipo.Credito, "")
        End If

        InsertarAuditoria(myConn, IIf(i_modo = movimiento.iAgregar, MovAud.iIncluir, MovAud.imodificar), sModulo, Documento)
        ComprobanteNumero = Documento

    End Sub

    Private Sub btnEmisionDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmisionDB.Click
        txtEmisionDB.Text = SeleccionaFecha(CDate(txtEmisionDB.Text), grpDebitos, Me)
    End Sub

    Private Sub btnVenceDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVenceDB.Click
        txtVenceDB.Text = SeleccionaFecha(CDate(txtVenceDB.Text), grpDebitos, Me)
    End Sub

    Private Sub txtImporteDB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtImporteDB.Click
        EnfocarTexto(sender)
    End Sub
    Private Sub lv_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lv.ItemChecked
        Dim strCon As String = aTipo(cmbTipo.SelectedIndex)
        With e.Item
            If .Checked Then
                iSel += 1
                dSel += CDbl(.SubItems(6).Text)
                strDocs = strDocs & .Text & ", "
                MontoPositivo += IIf(CDbl(.SubItems(6).Text) > 0, Math.Round(CDbl(.SubItems(6).Text), 2), 0)
                MontoNegativo += IIf(CDbl(.SubItems(6).Text) < 0, Math.Round(CDbl(.SubItems(6).Text), 2), 0)
                'If .SubItems(1).Text =  
            Else
                iSel -= 1
                If .SubItems.Count > 1 Then
                    dSel -= CDbl(.SubItems(6).Text)
                    strDocs = Replace(strDocs, .Text & ", ", "")
                    MontoPositivo -= IIf(CDbl(.SubItems(6).Text) > 0, Math.Round(CDbl(.SubItems(6).Text), 2), 0)
                    MontoNegativo -= IIf(CDbl(.SubItems(6).Text) < 0, Math.Round(CDbl(.SubItems(6).Text), 2), 0)
                End If
            End If
        End With

        If iSel < 0 Then
            iSel = 0
            dSel = 0.0
            MontoNegativo = 0.0
            MontoPositivo = 0.0
        End If

        txtDocSel.Text = FormatoEntero(iSel)
        txtSaldoSel.Text = FormatoNumero(dSel)
        txtConceptoCR.Text = strDocsIni & " " & IIf(iSel > 1, "DOCUMENTOS ", "DOCUMENTO ")
        If strDocs.Length >= 2 Then
            txtConceptoCR.Text = strDocsIni & " " & IIf(iSel > 1, "DOCUMENTOS ", "DOCUMENTO ") & strDocs.Substring(0, strDocs.Length - 2)
        End If

        txtImporteCR.Text = FormatoNumero(dSel)

    End Sub

    Private Sub cmbFPCR_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFPCR.SelectedIndexChanged

        btnNumPago.Enabled = False
        If cmbTipo.SelectedIndex <> 5 Then HabilitarObjetos(False, False, grpCRCO, optCO, optCR)

        txtNumPagCR.Text = ""
        txtNomPagCR.Text = ""

        'COLOCAR CAJA DE PAGO DESDE PARAMETRO
        '
        Dim aCajaCT As String = Parametro(myConn, lblInfo, Gestion.iVentas, "VENPARAM11")
        Dim aCajaEF As String = Parametro(myConn, lblInfo, Gestion.iVentas, "VENPARAM12")
        Dim aCajaCH As String = Parametro(myConn, lblInfo, Gestion.iVentas, "VENPARAM13")
        Dim aCajaTA As String = Parametro(myConn, lblInfo, Gestion.iVentas, "VENPARAM14")

        Select Case cmbFPCR.Text

            Case "Efectivo"
                cmbCajaCR.SelectedValue = aCajaEF
                HabilitarObjetos(False, True, txtNumPagCR, txtNomPagCR, btnNumPago, btnNomPago, cmbNombrePago)

            Case "Cheque", "Tarjeta", "Cupón de Alimentación", "Depósito", "Transferencia"

                If cmbFPCR.Text = "Cheque" Then
                    cmbCajaCR.SelectedValue = aCajaCH
                ElseIf cmbFPCR.Text = "Tarjeta" Then
                    cmbCajaCR.SelectedValue = aCajaTA
                ElseIf cmbFPCR.Text = "Cupón de Alimentación" Then
                    cmbCajaCR.SelectedValue = aCajaCT
                Else
                    cmbCajaCR.SelectedValue = "00"
                End If


                HabilitarObjetos(True, True, txtNumPagCR, cmbNombrePago)
                If cmbFPCR.Text = "Cupón de Alimentación" Then
                    btnNumPago.Enabled = True
                    HabilitarObjetos(True, False, grpCRCO, optCO, optCR)

                End If

        End Select

        IniciarNombrePago(cmbFPCR.Text)

    End Sub

    Private Sub IniciarNombrePago(ByVal FormaDePago As String)

        cmbNombrePago.DataSource = Nothing

        Select Case FormaDePago
            Case "Cheque"

                ds = DataSetRequery(ds, " Select codigo, descrip from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iBancos) _
                                    & "' and id_emp = '" & jytsistema.WorkID & "' order by descrip ", _
                                     myConn, nTablaBancosExt, lblInfo)

                dtBancosExt = ds.Tables(nTablaBancosExt)
                cmbNombrePago.DataSource = dtBancosExt
                cmbNombrePago.DisplayMember = dtBancosExt.Columns(1).ColumnName
                cmbNombrePago.ValueMember = dtBancosExt.Columns(0).ColumnName

            Case "Tarjeta"

                ds = DataSetRequery(ds, " Select codtar, nomtar from jsconctatar where id_emp = '" & jytsistema.WorkID & "' order by nomtar ", _
                                    myConn, nTablaTarjetas, lblInfo)

                dtTarjetas = ds.Tables(nTablaTarjetas)
                cmbNombrePago.DataSource = dtTarjetas
                cmbNombrePago.DisplayMember = dtTarjetas.Columns(1).ColumnName
                cmbNombrePago.ValueMember = dtTarjetas.Columns(0).ColumnName

            Case "Cupón de Alimentación"

                ds = DataSetRequery(ds, " Select 'CHEQUE ALIMENTACION' codban, 'CHEQUE ALIMENTACION' nomban ", _
                                   myConn, nTablaBancosInt, lblInfo)

                dtBancosInt = ds.Tables(nTablaBancosInt)
                cmbNombrePago.DataSource = dtBancosInt
                cmbNombrePago.DisplayMember = dtBancosInt.Columns(1).ColumnName
                cmbNombrePago.ValueMember = dtBancosInt.Columns(0).ColumnName

                ' Dim aStr() As String = {"CHEQUE ALIMENTACION"}
                'RellenaCombo(aStr, cmbNombrePago)
                txtNumPagCR.Text = "CT" & Format(jytsistema.sFechadeTrabajo, "ddMMyy") & CodigoCliente.Substring(CodigoCliente.Length - 5, 5) & Format(NumeroAleatorio(100), "00")

            Case "Depósito", "Transferencia"

                ds = DataSetRequery(ds, " Select codban, nomban from jsbancatban where id_emp = '" & jytsistema.WorkID & "' order by nomban ", _
                                   myConn, nTablaBancosInt, lblInfo)

                dtBancosInt = ds.Tables(nTablaBancosInt)
                cmbNombrePago.DataSource = dtBancosInt
                cmbNombrePago.DisplayMember = dtBancosInt.Columns(1).ColumnName
                cmbNombrePago.ValueMember = dtBancosInt.Columns(0).ColumnName


        End Select

    End Sub

    Private Sub txtImporteDB_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtImporteDB.KeyPress, _
        txtImporteCR.KeyPress
        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub

    Private Sub btnAsesor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesor.Click
        txtAsesor.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, concat(nombres, ' ', apellidos) " _
                                            & " descripcion from jsvencatven  where id_emp = '" & jytsistema.WorkID & "' ", _
                                            "Asesores comerciales", txtAsesor.Text)
    End Sub

    Private Sub btnAsesorCR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesorCR.Click

        If CBool(Parametro(myConn, lblInfo, Gestion.iVentas, "VENFACPA09")) Then
            txtAsesorCR.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, concat(nombres, ' ', apellidos) " _
                                                & " descripcion from jsvencatven  where TIPO = " & TipoVendedor.iFuerzaventa & " AND id_emp = '" & jytsistema.WorkID & "' ", _
                                                "Asesores comerciales", txtAsesorCR.Text)
        Else
            MensajeCritico(lblInfo, "Escogencia de asesor comercial no permitida...")
        End If
    End Sub

    Private Sub cmbNombrePago_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbNombrePago.Click
        'txtNomPagCR.Text = cmbNombrePago.SelectedValue
    End Sub

    Private Sub btnNumPago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNumPago.Click
        If ValorEntero(txtDocSel.Text) = 0 AndAlso optCR.Checked Then
            MensajeCritico(lblInfo, "Debe seleccionar al menos un documento...")
        Else
            'Declarar dias para comision 
            Dim DiasParaComision As Integer = 0
            Dim f As New jsVenArcCXCCestaTicket
            If optCO.Checked Then 'Produce Nota de Crédito
                ' Cargar Movimientos de CEsta Ticket con esos dias comisión
                '´MensajeAdvertencia(lblInfo, " Adelanto de Cestaticket ")
                f.Cargar(myConn, txtDocumentoCR.Text, txtNumPagCR.Text, ValorNumero(txtSaldoSel.Text), cmbCajaCR.SelectedValue, DiasParaComision)

            ElseIf optCR.Checked Then

                DiasParaComision = DateDiff(DateInterval.Day, CDate(txtEmisionCR.Text), FechaEmisionDocumento)
                ' Cargar Movimientos de CEsta Ticket con esos dias comisión
                'MensajeAdvertencia(lblInfo, " Cancelación con CestaTicket ")
                f.Cargar(myConn, txtDocumentoCR.Text, txtNumPagCR.Text, ValorNumero(txtSaldoSel.Text), cmbCajaCR.SelectedValue, DiasParaComision)

            End If

            txtImporteCR.Text = FormatoNumero(f.montoPago)

            f.Close()

        End If
    End Sub

    Private Sub btnEmisionCR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmisionCR.Click
        txtEmisionCR.Text = SeleccionaFecha(CDate(txtEmisionCR.Text), Me, grpCreditos, grptextos, btnEmisionCR)
    End Sub

    Private Sub cmbNombrePago_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbNombrePago.SelectedIndexChanged
        If cmbNombrePago.SelectedIndex <> -1 Then txtNomPagCR.Text = cmbNombrePago.SelectedValue.ToString()

    End Sub

    Private Sub cmbNombrePago_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbNombrePago.SelectedValueChanged
        If cmbNombrePago.SelectedIndex <> -1 Then txtNomPagCR.Text = cmbNombrePago.SelectedValue.ToString()
    End Sub

    Private Sub btnFechaComprobanteRetIVA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaComprobanteRetIVA.Click
        txtFechaComprobanteRetIVA.Text = SeleccionaFecha(CDate(txtFechaComprobanteRetIVA.Text), Me, btnFechaComprobanteRetIVA)
    End Sub

    Private Sub btnFechaRecepcionRetIVA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaRecepcionRetIVA.Click
        txtFechaRecepcionRetIVA.Text = SeleccionaFecha(CDate(txtFechaRecepcionRetIVA.Text), Me, sender)
    End Sub

    Private Sub btnAsesorRetIVA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesorRetIVA.Click
        txtAsesorRetIVA.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, concat(nombres, ' ', apellidos) " _
                                            & " descripcion from jsvencatven  where id_emp = '" & jytsistema.WorkID & "' ", _
                                            "Asesores comerciales", txtAsesorRetIVA.Text)
    End Sub

    Private Sub btnFechaRetISLR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaRetISLR.Click
        txtFechaRetISLR.Text = SeleccionaFecha(CDate(txtFechaRetISLR.Text), Me, btnFechaRetISLR)
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

        End If

        If iSel < 0 Then
            iSel = 0
            dSel = 0.0
            MontoNegativo = 0.0
            MontoPositivo = 0.0
        End If

        txtDocsSelRetISLR.Text = FormatoEntero(iSel)
        txtSaldoSelRetISLR.Text = FormatoNumero(dSel)

    End Sub
    Private Sub CalculaTotalesRetISLR()
        txtRetencionISLR.Text = FormatoNumero(Math.Round(ValorNumero(txtImporteBaseRetISLR.Text) * ValorNumero(txtPorcentajeRetISLR.Text) / 100, 2))
    End Sub

    Private Sub txtSaldoSelRetISLR_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSaldoSelRetISLR.TextChanged
        txtImporteBaseRetISLR.Text = txtSaldoSelRetISLR.Text
    End Sub

    Private Sub txtPorcentajeRetISLR_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPorcentajeRetISLR.TextChanged, _
        txtImporteBaseRetISLR.TextChanged
        CalculaTotalesRetISLR()
    End Sub

    Private Sub btnAsesorRetISLR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesorRetISLR.Click
        txtAsesorRetISLR.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, concat(nombres, ' ', apellidos) " _
                                            & " descripcion from jsvencatven  where id_emp = '" & jytsistema.WorkID & "' ", _
                                            "Asesores comerciales", txtAsesorRetISLR.Text)
    End Sub

    Private Sub btnCompras_Click(sender As System.Object, e As System.EventArgs) Handles btnCompras.Click

    End Sub

    Private Sub txtConceptoCR_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtConceptoCR.TextChanged
        If txtConceptoCR.Text.Length > 250 Then

        End If
    End Sub

    Private Sub txtRetencionISLR_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRetencionISLR.TextChanged

    End Sub
End Class