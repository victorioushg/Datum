Imports MySql.Data.MySqlClient
Public Class jsVenArcClientesCXCPlus
    Private Const sModulo As String = "Clientes y CxC - Movimientos CxC"

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
    Private dolarSel As Double = 0.00
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

        ft.RellenaCombo(aTipo, cmbTipo, numTipoMovimiento)
        Me.ShowDialog()

    End Sub
    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal CodigoCli As String)

        i_modo = movimiento.iEditar
        myConn = MyCon
        ds = dsMov
        dt = dtMov
        CodigoCliente = CodigoCli

        numTipoMovimiento = numTipoMovimientoOrigen()
        ft.RellenaCombo(aTipo, cmbTipo, numTipoMovimiento)
        cmbTipo.Enabled = False

        Me.ShowDialog()

    End Sub
    Private Function numTipoMovimientoOrigen() As Integer

        numTipoMovimientoOrigen = ft.InArray(aTipoNick, dt.Rows(Apuntador).Item("tipomov"))
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

    Private Sub jsVenArcClientesCXCPlus_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsVenArcClientesCXC_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ds = DataSetRequery(ds, strSQLCajas, myConn, nTablaCajas, lblInfo)
        dtCajas = ds.Tables(nTablaCajas)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ft.Ejecutar_strSQL(myconn, "DELETE FROM jsventabtic where NUMCAN = '" & txtDocumentoCR.Text & "' AND " _
                                    & " ID_EMP = '" & jytsistema.WorkID & "' ")
        Me.Close()
    End Sub
    Private Sub VerGrupoComoEs(grp As GroupBox)
        grp.Location = New Point(1, 53)
        grp.BringToFront()
    End Sub
    Private Sub cmbTipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipo.SelectedIndexChanged
        iSel = 0
        dSel = 0
        dolarSel = 0
        Select Case cmbTipo.SelectedIndex
            Case 0, 1, 2
                nTipoMovimientoCxC = 0
                ft.visualizarObjetos(True, grpDebitos)
                VerGrupoComoEs(grpDebitos)
                ft.visualizarObjetos(False, grpCreditos, grpRetencionIVA, grpRetencionISLR)
                If i_modo = movimiento.iAgregar Then
                    IniciarDebitos(cmbTipo.SelectedIndex)
                Else
                    AsignarDebitos(Apuntador)
                End If
            Case 3, 4, 5

                If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM25")) AndAlso cmbTipo.SelectedIndex = 5 Then
                    ft.MensajeCritico("NO ES POSIBLE REALIZAR NOTAS CREDITO EN ESTE MOMENTO. CONSULTE CON UN SUPERVISOR... ")
                    cmbTipo.SelectedIndex = 0
                    Return
                End If
                nTipoMovimientoCxC = 1
                ft.visualizarObjetos(True, grpCreditos)
                VerGrupoComoEs(grpCreditos)
                ft.visualizarObjetos(False, grpDebitos, grpRetencionISLR, grpRetencionIVA)
                IniciarCreditos(cmbTipo.SelectedIndex)

            Case 7

                nTipoMovimientoCxC = 2
                ft.visualizarObjetos(True, grpRetencionIVA)
                VerGrupoComoEs(grpRetencionIVA)
                ft.visualizarObjetos(False, grpDebitos, grpCreditos, grpRetencionISLR)
                IniciarRetencionIVA()
            Case 8
                nTipoMovimientoCxC = 3
                ft.visualizarObjetos(True, grpRetencionISLR)
                VerGrupoComoEs(grpRetencionISLR)
                ft.visualizarObjetos(False, grpDebitos, grpCreditos, grpRetencionIVA)
                IniciarRetencionISLR()
        End Select
    End Sub
    Private Sub IniciarCreditos(ByVal Tipo As Integer)

        ft.habilitarObjetos(False, True, txtDocumentoCR, txtDocSel, txtSaldoSel, txtNumPagCR, txtNomPagCR, txtEmisionCR, txtDocSel, txtSaldoSel,
                         cmbNombrePago, txtAsesorCR, txtImporteCRAjustado, txtSaldoEnDivisa)

        If Tipo = 3 Then
            strDocsIni = "ABONO "
            optCO.Checked = True
        ElseIf Tipo = 4 Then
            strDocsIni = "CANCELACION "
            optCO.Checked = True
        Else
            strDocsIni = "NOTA CREDITO "
        End If

        txtDocumentoCR.Text = "TMP" & ft.NumeroAleatorio(1000000)
        txtEmisionCR.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtReferenciaCR.Text = ""
        txtConceptoCR.Text = strDocs
        txtSaldoSel.Text = ft.FormatoNumero(0.0)
        txtImporteCR.Text = ft.FormatoNumero(0.0)
        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENCXCPA02")) Then
            txtAsesorCR.Text = ft.DevuelveScalarCadena(myConn, "select vendedor from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Else
            txtAsesorCR.Text = ""
        End If
        txtDocSel.Text = "0"


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

        If ChequesDevueltosPermitidos(myConn, lblInfo) > 0 AndAlso _
            ChequesDevueltosCliente(myConn, lblInfo, CodigoCliente) >= ChequesDevueltosPermitidos(myConn, lblInfo) Then
            ft.RellenaCombo(DeleteArrayValue(aFormaPago, "Cheque"), cmbFPCR)
        Else
            ft.RellenaCombo(aFormaPago, cmbFPCR)
        End If


    End Sub
    Private Sub IniciarRetencionIVA()

        ft.habilitarObjetos(False, True, txtTotalRetIVA, txtFechaComprobanteRetIVA, txtFechaRecepcionRetIVA, txtAsesorRetIVA)

        IniciarSaldosIVA(75)

        txtPorcentajeRetIVA.Text = ft.FormatoNumero(75)
        txtFechaComprobanteRetIVA.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtFechaRecepcionRetIVA.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtNumeroRetIVA.Text = ""
        txtAsesorRetIVA.Text = ft.DevuelveScalarCadena(myConn, " select vendedor from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")



        lblLeyendaIVA.Text = "Si el documento al cual desea hacer retención del IVA no aparece : " & vbLf _
            & " 1. El Documento no posee número de control de IVA " & vbLf _
            & " 2. El Documento no posee renglones con tasa de impuesto mayor a CERO (0) " & vbLf _
            & " 3. El Documento YA posee una Retención Asignada." & vbLf _
            & " 4. El documento tiene saldo cero (0) (YA fué cancelado) "

        dgRetIVA.Height = grpRetencionIVA.Height - GroupBox1.Height - dgRetIVA.Top - 10

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

            CalculaTotalRetIVA()

        End If

    End Sub
    Private Sub CalculaTotalRetIVA()
        Dim montoSeleccionado As Double = 0.0
        iSel = 0
        For Each selectedItem As DataGridViewRow In dgRetIVA.Rows
            If selectedItem.Cells(0).Value Then
                montoSeleccionado += selectedItem.Cells(8).Value
                iSel += 1
            End If
        Next
        txtTotalRetIVA.Text = ft.FormatoNumero(montoSeleccionado)
    End Sub

    Private Sub txtPorRetIVA_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPorcentajeRetIVA.TextChanged
        IniciarSaldosIVA(ValorNumero(txtPorcentajeRetIVA.Text))
        CalculaTotalRetIVA()
    End Sub
    Private Sub txtPorcentajeRetIVA_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPorcentajeRetIVA.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub ResetearVariablesDEUsoComun()
        iSel = 0
        dSel = 0
        MontoPositivo = 0
        MontoNegativo = 0
    End Sub


    Private Sub IniciarRetencionISLR()

        ResetearVariablesDEUsoComun()
        ft.habilitarObjetos(False, True, txtDocsSelRetISLR, txtSaldoSelRetISLR, txtImporteBaseRetISLR, _
                        txtFechaRetISLR, txtAsesorRetISLR)
        ft.habilitarObjetos(True, True, txtPorcentajeRetISLR, txtNumeroRetISLR, txtRetencionISLR)
        IniciarSaldosISLR(lvRetISLR)

        txtImporteBaseRetISLR.Text = ft.FormatoNumero(0.0)
        txtPorcentajeRetISLR.Text = ft.FormatoNumero(0.0)
        txtFechaRetISLR.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtNumeroRetISLR.Text = ""
        txtAsesorRetISLR.Text = ft.DevuelveScalarCadena(myConn, " select vendedor from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")



    End Sub

    Private Sub IniciarSaldos(ByVal lv As ListView)

        Dim list As List(Of KeyValuePair(Of String, Object)) =
            New List(Of KeyValuePair(Of String, Object))
        list.Add(New KeyValuePair(Of String, Object)("WorkId", jytsistema.WorkID))
        list.Add(New KeyValuePair(Of String, Object)("CodigoCliente", CodigoCliente))

        ds = DataSetRequery(myConn, "CXC_SaldosClientePorDocumento", list, ds, nTablaSaldos)
        dtSaldos = ds.Tables(nTablaSaldos)

        Dim symbolChange = ft.DevuelveScalarCadena(myConn, " select codigoISO from jsconcatmon where id = (select monedacambio from jsconctaemp where id_emp = '" + jytsistema.WorkID + "') ")

        Dim aNombres() As String = {"Nº Documento", "TP", "Referencia", "Emisión", "Vence", "Importe inicial", "Saldo", "Asesor", symbolChange + "/Emision "}
        Dim aCampos() As String = {"nummov", "tipomov", "refer", "emision", "vence", "importe", "saldo", "codven", "cambioEnEmision"}
        Dim aAnchos() As Integer = {100, 40, 120, 90, 90, 150, 150, 50, 100}
        Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Left, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center,
            HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, HorizontalAlignment.Center,
            HorizontalAlignment.Right}
        Dim aFormato() As FormatoItemListView = {FormatoItemListView.iCadena, FormatoItemListView.iCadena, FormatoItemListView.iCadena,
            FormatoItemListView.iFecha, FormatoItemListView.iFecha, FormatoItemListView.iNumero, FormatoItemListView.iNumero, FormatoItemListView.iCadena,
            FormatoItemListView.iNumero}

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
    Private Sub IniciarSaldosIVA(PorcentajeRetencionIVA As Double)

        'Dim tblSaldosRetIVA As String = "tblSaldosIVA" & ft.NumeroAleatorio(10000)
        'Dim aFields() As String = {"sel.entero.1.0", "codcli.cadena.15.0", "nombre.cadena.250.0", "nummov.cadena.20.0", "tipomov.cadena.2.0", _
        '                               "num_control.cadena.20.0", "emision.fecha.0.0", "vence.fecha.0.0", "importe.doble.19.2", "impiva.doble.19.2", _
        '                               "retimpiva.doble.19.2"}

        'CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblSaldosRetIVA, aFields)

        'ft.Ejecutar_strSQL(myConn, " INSERT INTO " & tblSaldosRetIVA _
        '               & " SELECT 0 sel,  a.codcli, b.nombre, a.nummov, a.tipomov,  d.num_control, a.emision, a.vence, a.importe, " _
        '               & " SUM(c.impiva) impiva, ROUND(SUM(c.impiva)*" & PorcentajeRetencionIVA & "/100,2) retimpiva " _
        '               & " FROM jsventracob a " _
        '               & " LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
        '               & " LEFT JOIN jsvenivafac c ON (a.nummov = c.numfac AND a.codcli = b.codcli AND a.id_emp = c.id_emp) " _
        '               & " LEFT JOIN jsconnumcon d ON (a.nummov = d.numdoc AND a.codcli = d.prov_cli AND a.emision = d.emision AND 'FAC' = d.org AND 'FAC' = d.origen AND a.id_emp = d.id_emp) " _
        '               & " LEFT JOIN (SELECT a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsventracob a WHERE a.codcli = '" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov)  " _
        '               & " WHERE " _
        '               & " a.nummov NOT IN (SELECT a.nummov FROM jsventracob a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codcli = '" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) AND  " _
        '               & " e.saldo <> 0.00 AND " _
        '               & " d.num_control <> '' AND " _
        '               & " a.tipomov = 'FC' AND " _
        '               & " a.origen = 'FAC' AND " _
        '               & " a.codcli = '" & CodigoCliente & "'  AND " _
        '               & " a.historico = '0' AND " _
        '               & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
        '& " GROUP BY nummov ")

        'ft.Ejecutar_strSQL(myConn, " INSERT INTO " & tblSaldosRetIVA _
        '               & " SELECT 0 sel,  a.codcli, b.nombre, a.nummov, a.tipomov,  d.num_control, a.emision, a.vence, a.importe, " _
        '               & " SUM(c.impiva) impiva, ROUND(SUM(c.impiva)*" & PorcentajeRetencionIVA & "/100,2) retimpiva " _
        '               & " FROM jsventracob a " _
        '               & " LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
        '               & " LEFT JOIN jsvenivapos c ON (a.nummov = c.numfac AND a.codcli = b.codcli AND a.id_emp = c.id_emp) " _
        '               & " LEFT JOIN jsconnumcon d ON (a.nummov = d.numdoc AND a.codcli = d.prov_cli AND a.emision = d.emision AND 'PVE' = d.org AND 'FAC' = d.origen AND a.id_emp = d.id_emp) " _
        '               & " LEFT JOIN (SELECT a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsventracob a WHERE a.codcli = '" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov)  " _
        '               & " WHERE " _
        '               & " a.nummov NOT IN (SELECT a.nummov FROM jsventracob a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codcli = '" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) AND  " _
        '               & " e.saldo <> 0.00 AND " _
        '               & " d.num_control <> '' AND " _
        '               & " a.tipomov = 'FC' AND " _
        '               & " a.origen = 'PVE' AND " _
        '               & " a.codcli = '" & CodigoCliente & "'  AND " _
        '               & " a.historico = '0' AND " _
        '               & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
        '               & " GROUP BY nummov ")

        'ft.Ejecutar_strSQL(myConn, " INSERT INTO " & tblSaldosRetIVA _
        '               & " SELECT 0 sel,  a.codcli, b.nombre, a.nummov, a.tipomov,  d.num_control, a.emision, a.vence, a.importe, " _
        '               & " SUM(c.impiva) impiva, ROUND(SUM(c.impiva)*" & PorcentajeRetencionIVA & "/100,2) retimpiva " _
        '               & " FROM jsventracob a " _
        '               & " LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
        '               & " LEFT JOIN jsvenivandb c ON (a.nummov = c.numndb AND a.codcli = b.codcli AND a.id_emp = c.id_emp) " _
        '               & " LEFT JOIN jsconnumcon d ON (a.nummov = d.numdoc AND a.codcli = d.prov_cli AND a.emision = d.emision AND 'NDB' = d.org AND 'FAC' = d.origen AND a.id_emp = d.id_emp) " _
        '               & " LEFT JOIN (SELECT a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsventracob a WHERE a.codcli = '" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov)  " _
        '               & " WHERE " _
        '               & " a.nummov NOT IN (SELECT a.nummov FROM jsventracob a WHERE a.tipomov = 'NC'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codcli = '" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) AND  " _
        '               & " e.saldo <> 0.00 AND " _
        '               & " d.num_control <> '' AND " _
        '               & " a.tipomov = 'ND' AND " _
        '               & " a.origen = 'NDB' AND " _
        '               & " a.codcli = '" & CodigoCliente & "'  AND " _
        '               & " a.historico = '0' AND " _
        '               & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
        '               & " GROUP BY nummov ")

        'ft.Ejecutar_strSQL(myConn, " INSERT INTO " & tblSaldosRetIVA _
        '               & " SELECT 0 sel,  a.codcli, b.nombre, a.nummov, a.tipomov,  d.num_control, a.emision, a.vence, a.importe, " _
        '               & " -1*SUM(c.impiva) impiva, -1*ROUND(SUM(c.impiva)*" & PorcentajeRetencionIVA & "/100,2) retimpiva " _
        '               & " FROM jsventracob a " _
        '               & " LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
        '               & " LEFT JOIN jsvenivancr c ON (a.nummov = c.numncr AND a.codcli = b.codcli AND a.id_emp = c.id_emp) " _
        '               & " LEFT JOIN jsconnumcon d ON (a.nummov = d.numdoc AND a.codcli = d.prov_cli AND a.emision = d.emision AND 'NCR' = d.org AND 'FAC' = d.origen AND a.id_emp = d.id_emp) " _
        '               & " LEFT JOIN (SELECT a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsventracob a WHERE a.codcli = '" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov HAVING saldo <> 0.00) e ON (a.nummov = e.nummov)  " _
        '               & " WHERE " _
        '               & " a.nummov NOT IN (SELECT a.nummov FROM jsventracob a WHERE a.tipomov = 'ND'  AND SUBSTRING(a.concepto,1,13) = 'RETENCION IVA' AND a.codcli = '" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) AND  " _
        '               & " e.saldo <> 0.00 AND " _
        '               & " d.num_control <> '' AND " _
        '               & " a.tipomov = 'NC' AND " _
        '               & " a.origen = 'NCR' AND " _
        '               & " a.codcli = '" & CodigoCliente & "'  AND " _
        '               & " a.historico = '0' AND " _
        '               & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
        '               & " GROUP BY nummov ")


        'Dim strSaldos As String = " select * from  " & tblSaldosRetIVA & " order by nummov, emision "

        '' ds = DataSetRequery(ds, strSaldos, myConn, nTablaSaldos, lblInfo)
        Dim list As List(Of KeyValuePair(Of String, Object)) = New List(Of KeyValuePair(Of String, Object))
        list.Add(New KeyValuePair(Of String, Object)("WorkId", jytsistema.WorkID))
        list.Add(New KeyValuePair(Of String, Object)("CodigoCliente", CodigoCliente))
        list.Add(New KeyValuePair(Of String, Object)("PorcentajeRetencionIVA", PorcentajeRetencionIVA))

        ds = DataSetRequery(myConn, "CXC_SaldosClienteRetencionesIVA", list, ds, nTablaSaldos)
        dtSaldos = ds.Tables(nTablaSaldos)

        CargarListaSeleccionRetencionesIVA(dgRetIVA, dtSaldos, 7, False)

        CalculaTotalRetIVA()

    End Sub

    Private Sub IniciarCajas()
        RellenaComboConDatatable(cmbCajaCR, dtCajas, "nomcaja", "caja")
    End Sub
    Private Sub IniciarDebitos(ByVal Tipo As Integer)

        ft.habilitarObjetos(False, True, txtDocumentoDB, txtEmisionDB, txtVenceDB, txtAsesor)

        txtDocumentoDB.Text = "TMP" & ft.NumeroAleatorio(1000000)
        txtEmisionDB.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtVenceDB.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtReferDB.Text = ""
        txtConceptoDB.Text = ""
        txtImporteDB.Text = ft.FormatoNumero(0.0)
        txtAsesor.Text = ""
        Dim Asesor As String = ft.DevuelveScalarCadena(myConn, " select vendedor from jsvencatcli where codcli = '" _
                                              & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "'")

        If Asesor = "" Then Asesor = CStr(IIf(IsDBNull(ft.DevuelveScalarCadena(myConn, " SELECT b.codven FROM jsvenrenrut a " _
            & " LEFT JOIN jsvenencrut b ON ( a.codrut = b.codrut AND a.tipo = b.tipo AND a.id_emp = b.id_emp ) " _
            & " WHERE " _
            & " a.tipo = 0 AND a.cliente ='" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' ")), "0", ft.DevuelveScalarCadena(myConn, " SELECT b.codven FROM jsvenrenrut a " _
            & " LEFT JOIN jsvenencrut b ON ( a.codrut = b.codrut AND a.tipo = b.tipo AND a.id_emp = b.id_emp ) " _
            & " WHERE " _
            & " a.tipo = 0 AND a.cliente ='" & CodigoCliente & "' AND a.id_emp = '" & jytsistema.WorkID & "' ")))

        If Asesor <> "" Then txtAsesor.Text = Asesor

    End Sub
    Private Sub AsignarDebitos(ByVal Puntero As Long)

        ft.habilitarObjetos(False, True, txtDocumentoDB, txtEmisionDB, btnEmisionDB, _
                         txtVenceDB, btnVenceDB, txtReferDB, txtConceptoDB, txtImporteDB, txtAsesor)

        With dt.Rows(Puntero)
            txtDocumentoDB.Text = .Item("nummov")
            txtEmisionDB.Text = ft.FormatoFecha(CDate(.Item("emision").ToString))
            txtVenceDB.Text = ft.FormatoFecha(CDate(.Item("vence").ToString))
            txtReferDB.Text = ft.MuestraCampoTexto(.Item("refer"))
            txtConceptoDB.Text = ft.MuestraCampoTexto(.Item("concepto"))
            txtImporteDB.Text = ft.FormatoNumero(.Item("importe"))
            txtAsesor.Text = ft.MuestraCampoTexto(.Item("codven"))
        End With


    End Sub

    Private Sub ValidarDebitos(ByVal Tipo As Integer)

        Dim aAdicionales() As String = {"tipomov IN ('FC','GR','ND') AND "}
        If FechaUltimoBloqueo(myConn, "jsventracob", aAdicionales) >= Convert.ToDateTime(txtEmisionDB.Text) Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Exit Sub
        End If

        If txtDocumentoDB.Text = "" Then
            ft.mensajeCritico("Debe indicar un Nº de documento válido")
            Exit Sub
        Else
            Dim aCampos() As String = {"codpro", "tipomov", "nummov", "id_emp"}
            Dim aStrings() As String = {CodigoCliente, aTipoNick(Tipo), txtDocumentoDB.Text, jytsistema.WorkID}
            If i_modo = movimiento.iAgregar AndAlso qFound(myConn, lblInfo, "jsprotrapag", aCampos, aStrings) Then
                ft.mensajeCritico("Documento YA se encuentra en movimientos de este proveedor ...")
                Exit Sub
            End If
        End If

        If CDate(txtEmisionDB.Text) > CDate(txtVenceDB.Text) Then
            ft.mensajeCritico("Fecha emisión mayor que fecha de vencimiento...")
            Exit Sub
        End If

        If Not ft.isNumeric(txtImporteDB.Text) Then
            ft.mensajeCritico("Debe indicar un número válido para el importe...")
            Exit Sub
        End If

        If txtAsesor.Text = "" Then
            ft.mensajeCritico("Debe indicar un asesor válido...")
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
                            ft.FormatoHora(Now()), CDate(txtVenceDB.Text), txtReferDB.Text, txtConceptoDB.Text, ValorNumero(txtImporteDB.Text), _
                            0.0, "", "", "", "", "", "CXC", Documento, "0", "", jytsistema.sFechadeTrabajo, "", "0", "", 0.0, 0.0, "", "", "", _
                            "", txtAsesor.Text, txtAsesor.Text, "0", FOTipo.Debito, jytsistema.WorkDivition)

        InsertarAuditoria(myConn, IIf(Inserta, MovAud.iIncluir, MovAud.imodificar), sModulo, Documento)

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Select Case cmbTipo.SelectedIndex
            Case TipoDocumento.Factura,
                 TipoDocumento.Giro,
                 TipoDocumento.NotaDebito
                ValidarDebitos(cmbTipo.SelectedIndex)

            Case TipoDocumento.Abono,
                 TipoDocumento.Cancelacion,
                 TipoDocumento.NotaCredito
                ValidarCreditos(cmbTipo.SelectedIndex)

            Case TipoDocumento.RetencionIVA
                ValidarRetencionIVA()

            Case TipoDocumento.RetencionISLR
                ValidarRetencionISLR()

        End Select

    End Sub
    Private Sub ValidarRetencionISLR()

        Dim aAdicionales() As String = {"tipomov = 'NC' AND ", "SUBSTRING(refer,1,5) = 'ISLR-' AND "}
        If FechaUltimoBloqueo(myConn, "jsventracob", aAdicionales) >= Convert.ToDateTime(txtFechaRetISLR.Text) Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Exit Sub
        End If

        If ValorNumero(txtSaldoSelRetISLR.Text) = 0 Then
            ft.MensajeCritico("Debe seleccionar al menos un documento...")
            Exit Sub
        End If

        If Not (ValorNumero(txtPorcentajeRetISLR.Text) > 0 Or ValorNumero(txtPorcentajeRetISLR.Text) <= 100) Then
            ft.MensajeCritico("Debe indicar un porcentaje de retención válido...")
            Exit Sub
        End If

        If txtNumeroRetISLR.Text.Trim() = "" Then
            ft.MensajeCritico("Debe indicar un número de comprobante de retención válido...")
            Exit Sub
        End If

        GuardarRetencionISLR()

        Me.Close()

    End Sub
    Private Sub ValidarRetencionIVA()

        Dim aAdicionales() As String = {"tipomov = 'NC' AND ", "SUBSTRING(concepto,1,13) = 'RETENCION IVA' AND "}
        If FechaUltimoBloqueo(myConn, "jsventracob", aAdicionales) >= Convert.ToDateTime(txtFechaRecepcionRetIVA.Text) Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Exit Sub
        End If

        If ValorNumero(txtTotalRetIVA.Text) = 0 Then
            ft.mensajeCritico("Debe seleccionar al menos un documento...")
            Exit Sub
        End If

        If Not (ValorNumero(txtPorcentajeRetIVA.Text) = 75 Or ValorNumero(txtPorcentajeRetIVA.Text) = 100) Then
            ft.mensajeCritico("Debe indicar un porcentaje de retención válido...")
            Exit Sub
        End If

        If txtNumeroRetIVA.Text.Trim() = "" Then
            ft.mensajeCritico("Debe indicar un número de comprobante de retención válido...")
            Exit Sub
        End If

        GuardarRetencionIVA()


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
                         IIf(.SubItems(1).Text = "NC", "ND", "NC"), .Text, CDate(txtFechaRetISLR.Text), ft.FormatoHora(Now()), CDate(txtFechaRetISLR.Text), _
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

        Dim montoTotalRetencion As Double = ValorNumero(txtTotalRetIVA.Text)
        For Each selectedItem As DataGridViewRow In dgRetIVA.Rows
            If selectedItem.Cells(0).Value Then
                With selectedItem
                    InsertEditVENTASCXC(myConn, lblInfo, Inserta, CodigoCliente, _
                          IIf(.Cells(2).Value = "NC", "ND", "NC"), .Cells(1).Value, CDate(txtFechaRecepcionRetIVA.Text), ft.FormatoHora(Now()), CDate(txtFechaRecepcionRetIVA.Text), _
                         Documento, "RETENCION IVA CLIENTE ESPECIAL", -1 * ValorNumero(.Cells(8).Value), 0.0, "", "", "", "", _
                         "", "CXC", .Cells(1).Value, "0", "", CDate(txtFechaComprobanteRetIVA.Text), "", "0", "", _
                         0.0, 0.0, "", "", "", "", txtAsesorRetIVA.Text, txtAsesorRetIVA.Text, "0", IIf(.Cells(2).Value = "NC", FOTipo.Debito, FOTipo.Credito), "")
                End With

            End If
        Next


        ComprobanteNumero = Documento
        InsertarAuditoria(myConn, IIf(Inserta, MovAud.iIncluir, MovAud.imodificar), sModulo, Documento)

    End Sub

    Private Sub ValidarCreditos(ByVal Tipo As Integer)


        Dim aAdicionales() As String = {"tipomov IN ('AB','CA','NC') AND ", _
                                        "SUBSTRING(concepto,1,13) <> 'RETENCION IVA' AND ", _
                                        "SUBSTRING(refer,1,5) <> 'ISLR-' AND "}

        If FechaUltimoBloqueo(myConn, "jsventracob", aAdicionales) >= Convert.ToDateTime(txtEmisionCR.Text) Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Exit Sub
        End If

        If ValorEntero(txtDocSel.Text) = 0 And Tipo < 5 Then
            ft.MensajeCritico("Debe seleccionar al menos un documento...")
            Exit Sub
        End If

        If Trim(txtDocumentoCR.Text) = "" Then
            ft.MensajeCritico("Debe indicar un número de documento (comprobante) válido...")
            Exit Sub
        End If

        If Tipo = 5 Then
            If txtCausaNotaCredito.Text.Trim = "" Then
                ft.MensajeCritico("Debe indicar una CAUSA DE CREDITO Válida...")
                Exit Sub
            Else
                If ft.DevuelveScalarBooleano(myConn, " select VALIDA_DOCUMENTOS from jsconcausas_notascredito where codigo = '" & txtCausaNotaCredito.Text & "' and origen = 'CXC' ") Then
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
            If ValorNumero(txtImporteCR.Text) < 0 Then
                ft.mensajeCritico("Debe indicar un importe MAYOR O IGUAL QUE CERO")
                Exit Sub
            Else
                Select Case cmbTipo.SelectedIndex
                    Case 3, 4 'ABONOS, CANCELACIONES
                        If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPARAM10")) Then
                            If ValorNumero(txtImporteCR.Text) > ValorNumero(txtSaldoSel.Text) Then
                                ft.mensajeCritico("EL MONTO A CANCELAR NO DEBE EXCEDER AL MONTO TOTAL DE DOCUMENTOS SELECCIONADOS ... ")
                                Exit Sub
                            End If
                        End If
                    Case 5 'NOTAS CREDITO
                        If ValorNumero(txtImporteCR.Text) = 0 Then
                            ft.mensajeCritico("Debe indicar un importe MAYOR O IGUAL QUE CERO")
                            Exit Sub
                        End If
                End Select
            End If
        Else
            ft.mensajeCritico("Debe indicar un importe válido...")
            Exit Sub
        End If

        If txtAsesorCR.Text = "" Then
            ft.MensajeCritico("Debe indicar un asesor comercial válido...")
            Exit Sub
        End If

        If txtNumPagCR.Text = "" AndAlso cmbFPCR.SelectedIndex >= 1 Then
            ft.MensajeCritico("Debe indicar un número de pago válido...")
            Exit Sub
        End If

        If txtNomPagCR.Text = "" AndAlso cmbFPCR.SelectedIndex >= 1 Then
            ft.MensajeCritico("Debe indicar un nombre de pago válido ...")
            Exit Sub
        End If

        If cmbFPCR.Text = "Cupón de Alimentación" AndAlso CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPARAM09")) Then
            If ft.DevuelveScalarEntero(myConn, " SELECT COUNT(*) FROM jsventabtic WHERE NUMCAN = '" _
                                          & txtDocumentoCR.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ") = 0 Then
                ft.mensajeCritico("Debe incluir los codigos de tickets pertenecientes a esta cancelación... ")
                Exit Sub
            End If
        End If

        If cmbFPCR.Text = "Cheque" Then
            If CDate(txtEmisionCR.Text) > jytsistema.sFechadeTrabajo Then
                Dim DiasPostDate As Integer = Math.Abs(DateDiff(DateInterval.Day, CDate(txtEmisionCR.Text), CDate(ft.FormatoFecha(jytsistema.sFechadeTrabajo))))
                If DiasPostDate > 0 AndAlso CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPARAM17")) Then
                    Dim MaxDiasPostDate As Integer = CInt(ParametroPlus(myConn, Gestion.iVentas, "VENPARAM18"))
                    If DiasPostDate > MaxDiasPostDate Then
                        ft.MensajeCritico("Se aceptan cheques posdatados pero con un máximo de " & CStr(MaxDiasPostDate) _
                                       & " días a partir de hoy... ")
                        Exit Sub
                    End If
                Else

                    ft.MensajeCritico("No se aceptan cancelaciones con cheques posdatados...")
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

        Dim FormaDePago As String = ""
        Dim NombreDePago As String = ""
        Dim NumeroDePago As String = ""

        If optCO.Checked Then

            FormaDePago = aFormaPagoAbreviada(ft.InArray(aFormaPago, cmbFPCR.Text))
            NombreDePago = txtNomPagCR.Text
            NumeroDePago = txtNumPagCR.Text

            Select Case cmbFPCR.Text
                Case "Efectivo", "Cheque", "Tarjeta", "Cupón de Alimentación"
                    If cmbFPCR.Text = "Cupón de Alimentación" Then
                        Dim dtCantidad As DataTable
                        Dim nTablaCantidad As String = "tblCantidad"
                        ds = DataSetRequery(ds, "select CORREDOR, IFNULL(COUNT(*),0) AS CANTIDAD, SUM(MONTO) AS IMPORTE from jsventabtic WHERE " _
                                                & " ID_EMP = '" & jytsistema.WorkID & "' AND" _
                                                & " NUMCAN = '" & txtDocumentoCR.Text & "' GROUP BY CORREDOR ORDER BY CORREDOR  ", myConn, nTablaCantidad, lblInfo)

                        dtCantidad = ds.Tables(nTablaCantidad)

                        If dtCantidad.Rows.Count > 0 Then
                            Dim fCont As Integer
                            For fCont = 0 To dtCantidad.Rows.Count - 1
                                With dtCantidad.Rows(fCont)
                                    InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, cmbCajaCR.SelectedValue, UltimoCajaMasUno(myConn, lblInfo, cmbCajaCR.SelectedValue), _
                                                                CDate(txtEmisionCR.Text), "CXC", "EN", Documento, _
                                                                FormaDePago, NumeroDePago, _
                                                                .Item("CORREDOR"), .Item("IMPORTE"), "", txtConceptoCR.Text, "", jytsistema.sFechadeTrabajo, _
                                                                .Item("CANTIDAD"), "", "", "", jytsistema.sFechadeTrabajo, CodigoCliente, txtAsesorCR.Text, "1")

                                End With
                            Next
                        End If
                        dtCantidad.Dispose()
                        dtCantidad = Nothing

                        ft.Ejecutar_strSQL(myconn, " update jsventabtic set numcan = '" & Documento & "' where numcan = '" & txtDocumentoCR.Text & "' ")

                    Else

                        If ValorNumero(txtImporteCR.Text) > 0 Then _
                            InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, cmbCajaCR.SelectedValue, UltimoCajaMasUno(myConn, lblInfo, cmbCajaCR.SelectedValue), _
                                                        CDate(txtEmisionCR.Text), "CXC", "EN", Documento, _
                                                        FormaDePago, NumeroDePago, IIf(cmbNombrePago.Items.Count > 0, cmbNombrePago.SelectedValue, ""), _
                                                        ValorNumero(txtImporteCR.Text), "", txtConceptoCR.Text, "", jytsistema.sFechadeTrabajo, 1, "", "", _
                                                        "", jytsistema.sFechadeTrabajo, CodigoCliente, txtAsesorCR.Text, "1")
                    End If



                Case "Depósito", "Transferencia"

                    InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, CDate(txtEmisionCR.Text), txtNumPagCR.Text, _
                                                    "NC", IIf(cmbNombrePago.Items.Count > 0, cmbNombrePago.SelectedValue.ToString, ""), "", txtConceptoCR.Text, ValorNumero(txtImporteCR.Text), _
                                                    "CXC", Documento, "", "", "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, _
                                                    aTipoNick(Tipo), "", jytsistema.sFechadeTrabajo, "0", CodigoCliente, txtAsesorCR.Text)
            End Select

        Else

            If cmbTipo.SelectedIndex = 5 Then

                FormaDePago = ft.DevuelveScalarCadena(myConn, " select FORMAPAGO from jsconcausas_notascredito WHERE origen = 'CXC' and codigo = '" & txtCausaNotaCredito.Text & "' ")
                NombreDePago = ft.DevuelveScalarCadena(myConn, " select NUMPAG from jsconcausas_notascredito WHERE origen = 'CXC' and codigo = '" & txtCausaNotaCredito.Text & "' ")
                'ACTUALIZAR O INCREMENTAR CONTADOR
                NumeroDePago = ft.DevuelveScalarCadena(myConn, " select NOMPAG from jsconcausas_notascredito WHERE origen = 'CXC' and codigo = '" & txtCausaNotaCredito.Text & "' ")


            End If

        End If

        If ValorNumero(txtImporteCR.Text) >= ValorNumero(txtSaldoSel.Text) Then
            Resto = ValorNumero(txtImporteCR.Text) - ValorNumero(txtSaldoSel.Text)
        ElseIf ValorNumero(txtImporteCR.Text) < ValorNumero(txtSaldoSel.Text) Then
            MontoPositivo = ValorNumero(txtImporteCR.Text) - MontoNegativo
        End If

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
                                    CDate(txtEmisionCR.Text), ft.FormatoHora(Now), CDate(txtEmisionCR.Text), _
                                    txtReferenciaCR.Text, txtConceptoCR.Text, -1 * (MontoPositivo), 0.0, _
                                    FormaDePago, cmbCajaCR.SelectedValue, NumeroDePago, NombreDePago, txtBeneficiario.Text, "CXC", _
                                    Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), causaNotaCredito, jytsistema.sFechadeTrabajo, _
                                    "", " ", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", "", txtAsesorCR.Text, _
                                    txtAsesorCR.Text, "0", FOTipo.Credito, "")


                            Else

                                InsertEditVENTASCXC(myConn, lblInfo, True, CodigoCliente, aTipoNick(Tipo), lv.Items(jCont).Text, _
                                    CDate(txtEmisionCR.Text), ft.FormatoHora(Now), CDate(txtEmisionCR.Text), _
                                    txtReferenciaCR.Text, txtConceptoCR.Text, -1 * (Math.Abs(MontoACancelar)), 0.0, _
                                    FormaDePago, cmbCajaCR.SelectedValue, NumeroDePago, NombreDePago, txtBeneficiario.Text, "CXC", _
                                    Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), "", jytsistema.sFechadeTrabajo, _
                                    "", "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", "", txtAsesorCR.Text, _
                                    txtAsesorCR.Text, "0", FOTipo.Credito, "")

                            End If

                        Else

                            If MontoACancelar > MontoPositivo Then

                                InsertEditVENTASCXC(myConn, lblInfo, True, CodigoCliente, "NC", lv.Items(jCont).Text, _
                                    CDate(txtEmisionCR.Text), ft.FormatoHora(Now), CDate(txtEmisionCR.Text), _
                                    txtReferenciaCR.Text, txtConceptoCR.Text, -1 * (MontoPositivo), 0.0, _
                                    FormaDePago, cmbCajaCR.SelectedValue, NumeroDePago, NombreDePago, txtBeneficiario.Text, "CXC", _
                                    Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), causaNotaCredito, jytsistema.sFechadeTrabajo, _
                                    "", "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", "", txtAsesorCR.Text, _
                                    txtAsesorCR.Text, "0", FOTipo.Credito, "")

                            Else

                                InsertEditVENTASCXC(myConn, lblInfo, True, CodigoCliente, "NC", lv.Items(jCont).Text, _
                                    CDate(txtEmisionCR.Text), ft.FormatoHora(Now), CDate(txtEmisionCR.Text), _
                                    txtReferenciaCR.Text, txtConceptoCR.Text, -1 * Math.Abs(MontoACancelar), 0.0, _
                                    FormaDePago, cmbCajaCR.SelectedValue, NumeroDePago, NombreDePago, txtBeneficiario.Text, "CXC", _
                                    Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), causaNotaCredito, jytsistema.sFechadeTrabajo, _
                                    "", "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", "", txtAsesorCR.Text, _
                                    txtAsesorCR.Text, "0", FOTipo.Credito, "")
                            End If



                        End If

                        MontoPositivo -= Math.Abs(MontoACancelar)

                    End If

                ElseIf MontoACancelar < 0 Then

                    If Math.Abs(MontoACancelar) > MontoNegativo Then

                        InsertEditVENTASCXC(myConn, lblInfo, True, CodigoCliente, "ND", lv.Items(jCont).Text, _
                                CDate(txtEmisionCR.Text), ft.FormatoHora(Now()), CDate(txtEmisionCR.Text), txtReferenciaCR.Text, txtConceptoCR.Text, _
                                 Math.Abs(MontoNegativo), 0.0, FormaDePago, cmbCajaCR.SelectedValue, NumeroDePago, _
                                 NombreDePago, txtBeneficiario.Text, "CXC", Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), "", _
                                 jytsistema.sFechadeTrabajo, "", "", lv.Items(jCont).SubItems(1).Text, 0.0, 0.0, Documento, "", "", "", _
                                 txtAsesorCR.Text, txtAsesorCR.Text, "0", FOTipo.Debito, "")
                    Else

                        InsertEditVENTASCXC(myConn, lblInfo, True, CodigoCliente, "ND", lv.Items(jCont).Text, _
                                CDate(txtEmisionCR.Text), ft.FormatoHora(Now()), CDate(txtEmisionCR.Text), txtReferenciaCR.Text, txtConceptoCR.Text, _
                                 Math.Abs(MontoACancelar), 0.0, FormaDePago, cmbCajaCR.SelectedValue, NumeroDePago, _
                                 NombreDePago, txtBeneficiario.Text, "CXC", Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), "", _
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

                ft.Ejecutar_strSQL(myconn, " update jsvencatcli set " _
                        & " ultcobro = " & ValorNumero(txtImporteCR.Text) & ", " _
                        & " fecultcobro = '" & ft.FormatoFechaMySQL(CDate(txtEmisionCR.Text)) & "', " _
                        & " forultcobro = '" & FormaDePago & "' " _
                        & " where " _
                        & " codcli = '" & CodigoCliente & "' and " _
                        & " id_emp = '" & jytsistema.WorkID & "' ")

                'MODIFICA FACTURA QUE ORIGINÓ LA CXC
                ft.Ejecutar_strSQL(myconn, " update jsvenencfac set " _
                                   & " formapag = '" & FormaDePago & "', " _
                                   & " numpag = '" & NumeroDePago & "', " _
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
                           CDate(txtEmisionCR.Text), ft.FormatoHora(Now()), CDate(txtEmisionCR.Text), txtReferenciaCR.Text, txtConceptoCR.Text, _
                            -1 * Resto, 0.0, FormaDePago, cmbCajaCR.SelectedValue, txtNumPagCR.Text, _
                            txtNomPagCR.Text, txtBeneficiario.Text, "CXC", Documento, IIf(CInt(txtDocSel.Text) <= 1, "0", "1"), causaNotaCredito, _
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
        ft.enfocarTexto(sender)
    End Sub
    Private Sub lv_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lv.ItemChecked
        Dim strCon As String = aTipo(cmbTipo.SelectedIndex)
        With e.Item
            If .Checked Then
                iSel += 1
                dSel += CDbl(.SubItems(6).Text)
                dolarSel += CDbl(.SubItems(8).Text)
                strDocs = strDocs & .Text & ", "
                MontoPositivo += IIf(CDbl(.SubItems(6).Text) > 0, Math.Round(CDbl(.SubItems(6).Text), 2), 0)
                MontoNegativo += IIf(CDbl(.SubItems(6).Text) < 0, Math.Round(CDbl(.SubItems(6).Text), 2), 0)
            Else
                iSel -= 1
                If .SubItems.Count > 1 Then
                    dSel -= CDbl(.SubItems(6).Text)
                    dolarSel -= CDbl(.SubItems(8).Text)
                    strDocs = Replace(strDocs, .Text & ", ", "")
                    MontoPositivo -= IIf(CDbl(.SubItems(6).Text) > 0, Math.Round(CDbl(.SubItems(6).Text), 2), 0)
                    MontoNegativo -= IIf(CDbl(.SubItems(6).Text) < 0, Math.Round(CDbl(.SubItems(6).Text), 2), 0)
                End If
            End If
        End With

        If iSel < 0 Then
            iSel = 0
            dSel = 0.0
            dolarSel = 0.00
            MontoNegativo = 0.0
            MontoPositivo = 0.0
        End If

        txtDocSel.Text = ft.FormatoEntero(iSel)
        txtSaldoSel.Text = ft.FormatoNumero(dSel)
        txtSaldoEnDivisa.Text = ft.FormatoNumero(dolarSel)

        txtConceptoCR.Text = strDocsIni & " " & IIf(iSel > 1, "DOCUMENTOS ", "DOCUMENTO ")
        If strDocs.Length >= 2 Then
            txtConceptoCR.Text = strDocsIni & " " & IIf(iSel > 1, "DOCUMENTOS ", "DOCUMENTO ") & strDocs.Substring(0, strDocs.Length - 2)
        End If

        txtImporteCR.Text = ft.FormatoNumero(dSel)
        txtImporteCRAjustado.Text = ft.FormatoNumero(dolarSel * CambioActual(myConn, jytsistema.sFechadeTrabajo))

    End Sub

    Private Sub cmbFPCR_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFPCR.SelectedIndexChanged

        btnNumPago.Enabled = False
        If cmbTipo.SelectedIndex <> 5 Then ft.habilitarObjetos(False, False, grpCRCO, optCO, optCR)

        txtNumPagCR.Text = ""
        txtNomPagCR.Text = ""

        'txtEmisionCR.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        'MensajeInformativoPlus("La Fecha de registro de este crédito ha sido modificada a fecha ACTUAL")

        'COLOCAR CAJA DE PAGO DESDE PARAMETRO
        '
        Dim aCajaCT As String = ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM11")
        Dim aCajaEF As String = ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM12")
        Dim aCajaCH As String = ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM13")
        Dim aCajaTA As String = ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM14")

        Select Case cmbFPCR.Text

            Case "Efectivo"
                cmbCajaCR.SelectedValue = aCajaEF
                ft.habilitarObjetos(False, True, txtNumPagCR, txtNomPagCR, btnNumPago, btnNomPago, cmbNombrePago)

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


                ft.habilitarObjetos(True, True, txtNumPagCR, cmbNombrePago)
                If cmbFPCR.Text = "Cupón de Alimentación" Then btnNumPago.Enabled = True

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

                txtNumPagCR.Text = "CT" & Format(jytsistema.sFechadeTrabajo, "ddMMyy") & CodigoCliente.Substring(CodigoCliente.Length - 5, 5) & ft.NumeroAleatorio(100)

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
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub btnAsesor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesor.Click
        txtAsesor.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, concat(nombres, ' ', apellidos) " _
                                            & " descripcion from jsvencatven  where id_emp = '" & jytsistema.WorkID & "' ", _
                                            "Asesores comerciales", txtAsesor.Text)
    End Sub

    Private Sub btnAsesorCR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesorCR.Click

        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENCXCPA01")) Then
            txtAsesorCR.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, concat(nombres, ' ', apellidos) " _
                                                & " descripcion from jsvencatven  where TIPO = " & TipoVendedor.iFuerzaventa & " AND id_emp = '" & jytsistema.WorkID & "' ", _
                                                "Asesores comerciales", txtAsesorCR.Text)
        Else
            ft.MensajeCritico("Escogencia de asesor comercial no permitida...")
        End If
    End Sub

    Private Sub cmbNombrePago_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbNombrePago.Click
        'txtNomPagCR.Text = cmbNombrePago.SelectedValue
    End Sub

    Private Sub btnNumPago_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNumPago.Click
        If ValorEntero(txtDocSel.Text) = 0 AndAlso optCR.Checked Then
            ft.MensajeCritico("Debe seleccionar al menos un documento...")
        Else
            'Declarar dias para comision 
            Dim DiasParaComision As Integer = 0
            Dim f As New jsVenArcCXCCestaTicket
            If optCO.Checked Then 'Produce Nota de Crédito
                ' Cargar Movimientos de CEsta Ticket con esos dias comisión
                '´ft.mensajeAdvertencia( " Adelanto de Cestaticket ")
                f.Cargar(myConn, txtDocumentoCR.Text, txtNumPagCR.Text, ValorNumero(txtSaldoSel.Text), cmbCajaCR.SelectedValue, DiasParaComision)
            ElseIf optCR.Checked Then

                DiasParaComision = DateDiff(DateInterval.Day, CDate(txtEmisionCR.Text), FechaEmisionDocumento)
                ' Cargar Movimientos de CEsta Ticket con esos dias comisión
                'ft.mensajeAdvertencia( " Cancelación con CestaTicket ")
                f.Cargar(myConn, txtDocumentoCR.Text, txtNumPagCR.Text, ValorNumero(txtSaldoSel.Text), cmbCajaCR.SelectedValue, DiasParaComision)
            End If

            txtImporteCR.Text = ft.FormatoNumero(f.montoPago)

            f.Close()

        End If
    End Sub

    Private Sub btnEmisionCR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmisionCR.Click
        'If cmbFPCR.SelectedItem = "Cheque" Or cmbFPCR.SelectedItem = "Depósito" Or cmbFPCR.SelectedItem = "Transferencia" Then _
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

        txtDocsSelRetISLR.Text = ft.FormatoEntero(iSel)
        txtSaldoSelRetISLR.Text = ft.FormatoNumero(dSel)

    End Sub
    Private Sub CalculaTotalesRetISLR()
        txtRetencionISLR.Text = ft.FormatoNumero(Math.Round(ValorNumero(txtImporteBaseRetISLR.Text) * ValorNumero(txtPorcentajeRetISLR.Text) / 100, 2))
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

    Private Sub btnCompras_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub txtConceptoCR_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtConceptoCR.TextChanged
        If txtConceptoCR.Text.Length > 250 Then
        End If
    End Sub

    Private Sub dgRetIVA_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles _
        dgRetIVA.CellContentClick
        If e.ColumnIndex = 0 Then
            dtSaldos.Rows(e.RowIndex).Item(0) = Not CBool(dtSaldos.Rows(e.RowIndex).Item(0).ToString)
            CalculaTotalRetIVA()
        End If
    End Sub

    Private Sub dgRetIVA_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
            Handles dgRetIVA.CellValidating

        Dim headerText As String = _
            dgRetIVA.Columns(e.ColumnIndex).HeaderText

        If Not (headerText.Equals("Retención Importe IVA")) Then Return

        Dim eSinComas As String = Replace(e.FormattedValue.ToString(), ",", "")
        If (String.IsNullOrEmpty(eSinComas)) Then
            ft.mensajeAdvertencia("Debe indicar dígito(s) válido...")
            e.Cancel = True
        End If

        If Not ft.isNumeric(eSinComas) Then
            ft.mensajeAdvertencia("Debe indicar un número válido...")
            e.Cancel = True
        End If

        ft.mensajeInformativo("...")

    End Sub
    Private Sub dgRetIVA_DataError(ByVal sender As Object, ByVal e As DataGridViewDataErrorEventArgs) _
        Handles dgRetIVA.DataError

        If StrComp(e.Exception.Message, "Input string was not in a correct format.") = 0 Or _
            StrComp(e.Exception.Message, "La cadena de entrada no tiene el formato correcto.") = 0 Then
            ft.mensajeAdvertencia("Por favor indique un número sin formato (sin comas)")

            dgRetIVA.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = " "
        End If

    End Sub


    Private Sub dgRetIVA_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dgRetIVA.CellEndEdit
        dgRetIVA.Rows(e.RowIndex).ErrorText = String.Empty
    End Sub

    Private Sub dgRetIVA_CellValidated(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgRetIVA.CellValidated
        CalculaTotalRetIVA()
    End Sub

    Private Sub btnCausaNotaCredito_Click(sender As System.Object, e As System.EventArgs) Handles btnCausaNotaCredito.Click
        txtCausaNotaCredito.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descripcion  " _
                                            & " from jsconcausas_notascredito  where origen  = 'CXC' order by codigo ", _
                                            "CAUSAS NOTAS DE CREDITO", txtCausaNotaCredito.Text)
    End Sub

    Private Sub txtCausaNotaCredito_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCausaNotaCredito.TextChanged
        lblDescripCausaNotaCredito.Text = ft.DevuelveScalarCadena(myConn, " select descripcion from jsconcausas_notascredito where codigo = '" & txtCausaNotaCredito.Text & "' and origen = 'CXC' ")
        txtConceptoCR.Text += " " + lblDescripCausaNotaCredito.Text
        txtReferenciaCR.Text = ContadorNC_Financiera(myConn, txtCausaNotaCredito.Text, "CXC")
        causaNotaCredito = txtCausaNotaCredito.Text

        If ft.DevuelveScalarBooleano(myConn, " select CR from jsconcausas_notascredito where codigo = '" & txtCausaNotaCredito.Text & "' and origen = 'CXC' ") Then
            optCR.Checked = True
            grpPago.Enabled = False
        Else
            optCO.Checked = True
            grpPago.Enabled = True
        End If

    End Sub

End Class