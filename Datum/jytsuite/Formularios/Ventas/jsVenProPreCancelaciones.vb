Imports MySql.Data.MySqlClient
Public Class jsVenProPreCancelaciones

    Private Const sModulo As String = "Procesar precancelaciones a cancelaciones"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dtCxC As New DataTable
    Private ft As New Transportables

    Private n_Apuntador As Long

    Private strSQLCxC As String = ""

    Private nTablaCxC As String = "tblCxC"

    Private m_SortingColumn As ColumnHeader
    Private mEfectivo, mCheque, mTarjeta, mCupones, mDeposito, mTransfer, mIVA, mISLR As Double
    Private cEfectivo, cCheque, cTarjeta, cCupones, cDeposito, cTransfer, cIVA, cISLR As Double

    Private strAsesor As String = " select concat( codven, '-', apellidos, ', ', nombres ) nombre, codven, descar  from jsvencatven where estatus = '1' and  tipo = '0' and id_emp = '" & jytsistema.WorkID & "' order by codven "
    Private nTablaAsesor As String = "tblAsesor"

    Private strCajas As String = " SELECT a.caja codigo, CONCAT('C-',a.nomcaja) descrip " _
                                 & " FROM jsbanenccaj a " _
                                 & " WHERE " _
                                 & " a.id_emp = '" & jytsistema.WorkID & "' order by a.caja "

    Private strBancos As String = " Select a.codban codigo, CONCAT('B-', a.nomban, '-', a.ctaban) descrip " _
                                  & " FROM jsbancatban a " _
                                  & " WHERE " _
                                  & " a.id_emp = '" & jytsistema.WorkID & "' order by codban "

    Private nTablaCajas As String = "tblCajas"
    Private nTablaBancos As String = "tblBancos"

    Private dtCajas As DataTable
    Private dtBancos As DataTable
    Private dtAsesor As DataTable

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        MyConn = MyCon
        IniciarTXT()


        ds = DataSetRequery(ds, strAsesor, MyConn, nTablaAsesor, lblInfo)
        dtAsesor = ds.Tables(nTablaAsesor)
        RellenaComboConDatatable(cmbAsesores, dtAsesor, "nombre", "codven")

        ds = DataSetRequery(ds, strCajas, MyConn, nTablaCajas, lblInfo)
        dtCajas = ds.Tables(nTablaCajas)

        ds = DataSetRequery(ds, strBancos, MyConn, nTablaBancos, lblInfo)
        dtBancos = ds.Tables(nTablaBancos)

        RellenaComboConDatatable(cmbCaja, dtCajas, "DESCRIP", "CODIGO")
        RellenaComboConDatatable(cmbBancos, dtBancos, "DESCRIP", "CODIGO")

        Me.Show()

    End Sub
    Private Sub IniciarTXT()

        ft.visualizarObjetos(False, chkCT, chkTA, chkDP, chkTR)

        ft.habilitarObjetos(False, True, txtFecha, txtMEfectivo, txtMCheques, txtMTarjetas, txtMCupones, txtMDepositos, txtMTransfer, _
                         txtMRetISLR, txtMRetIVA, txtCEfectivo, txtCCheques, txtCTarjetas, txtCCupones, txtCDepositos, txtCTransfer, _
                         txtCRetISLR, txtCRetIVA, txtMTotal, txtCTotal)

        ft.habilitarObjetos(True, True, txtNumeroDeposito)

        txtNumeroDeposito.Text = ""

        txtFecha.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)

        chkEF.Checked = False
        chkCH.Checked = False
        chkTA.Checked = False
        chkCT.Checked = False
        chkDP.Checked = False
        chkTR.Checked = False
        chkISLR.Checked = False
        chkIVA.Checked = False


        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtMEfectivo, txtMCheques, txtMTarjetas, txtMCupones, txtMDepositos, txtMTransfer, _
                         txtMRetISLR, txtMRetIVA, txtMTotal)

        ft.iniciarTextoObjetos(FormatoItemListView.iEntero, txtCEfectivo, txtCCheques, txtCTarjetas, txtCCupones, txtCDepositos, txtCTransfer, _
                         txtCRetISLR, txtCRetIVA, txtCTotal)


    End Sub



    Private Sub CargarCancelaciones()

        If cmbAsesores.Items.Count > 0 And txtFecha.Text <> "" Then

            Dim strFP As String = ""

            strFP += IIf(chkEF.Checked, ",'EF'", "")
            strFP += IIf(chkCH.Checked, ",'CH'", "")
            strFP += IIf(chkTA.Checked, ",'TA'", "")
            strFP += IIf(chkCT.Checked, ",'CT'", "")
            strFP += IIf(chkDP.Checked, ",'DP'", "")
            strFP += IIf(chkTR.Checked, ",'TR'", "")

            Dim strNC As String = IIf(chkIVA.Checked, " UNION SELECT * FROM jsvencobrgv " _
                                      & " where " _
                                      & " emision = '" & ft.FormatoFechaMySQL(CDate(txtFecha.Text)) & "' and " _
                                      & " substring(concepto,1,13)  = 'RETENCION IVA' " _
                                      & " historico = '0' AND " _
                                      & " codven = '" & cmbAsesores.SelectedValue.ToString & "' AND " _
                                      & " tipomov = 'NC' AND " _
                                      & " id_emp = '" & jytsistema.WorkID & "' ", "")

            Dim strCXC As String = " SELECT * FROM jsvencobrgv WHERE " _
                                   & " emision = '" & ft.FormatoFechaMySQL(CDate(txtFecha.Text)) & "' and " _
                                   & " historico = '0' AND " _
                                   & " tipomov in ('AB', 'CA') and " _
                                   & " formapag IN ('XX'" & strFP & ") AND " _
                                   & " codven = '" & cmbAsesores.SelectedValue.ToString & "' AND " _
                                   & " id_emp = '" & jytsistema.WorkID & "' " & strNC _
                                   & " ORDER BY formapag, codcli "

            ds = DataSetRequery(ds, strCXC, MyConn, nTablaCxC, lblInfo)
            dtCxC = ds.Tables(nTablaCxC)

            CargaListViewPreCobranza(lvCobranza, dtCxC)

        End If

    End Sub

    Private Sub jsVenProPreCancelaciones_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsVenProPreCancelaciones_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtFecha.Text)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFecha.Text)
        Me.Close()
    End Sub

    Private Function Validado() As Boolean

        If FechaUltimoBloqueo(MyConn, "jsventracob") >= Convert.ToDateTime(txtFecha.Text) Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If txtNumeroDeposito.Text = "" AndAlso CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM26")) Then
            ft.MensajeCritico("Debe indicar un Número de depósito válido...")
            Return False
        End If

        If ValorEntero(txtCTotal.Text) = 0 Then
            ft.MensajeCritico("Debe seleccionar al menos un ítem...")
            Return False
        End If

        Return True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then

            Dim lvCont As Integer
            For lvCont = 0 To lvCobranza.Items.Count - 1

                ProgressBar1.Value = (lvCont + 1) / lvCobranza.Items.Count * 100
                With lvCobranza.Items(lvCont)
                    If .Checked Then

                        lblProgreso.Text = " Cobranza No. " & .Text

                        '1. Insertar en CXC
                        InsertEditVENTASCXC(MyConn, lblInfo, True, .SubItems(1).Text, .SubItems(3).Text, .SubItems(2).Text, CDate(txtFecha.Text), _
                                ft.FormatoHora(Now()), CDate(txtFecha.Text), .SubItems(6).Text, .SubItems(4).Text, ValorNumero(.SubItems(5).Text), _
                                0.0, .SubItems(9).Text, cmbCaja.SelectedValue, .SubItems(7).Text, .SubItems(8).Text, "", "CXC", .SubItems(2).Text, _
                                "0", "", jytsistema.sFechadeTrabajo, "", "0", "", 0.0, 0.0, .Text, "", "", _
                                "", cmbAsesores.SelectedValue, cmbAsesores.SelectedValue, "0", _
                                IIf(InStr("AB.CA.NC", .SubItems(3).Text) > 0, FOTipo.Credito, FOTipo.Debito), _
                                jytsistema.WorkDivition)

                        '2. Insertar en CAJA y/o Banco
                        If .SubItems(9).Text = "CT" Then

                            Dim dtCantidad As DataTable
                            Dim nTablaCantidad As String = "tblCantidad"
                            ds = DataSetRequery(ds, "select CORREDOR, IFNULL(COUNT(*),0) AS CANTIDAD, SUM(MONTO) AS IMPORTE from jsventabtic WHERE " _
                                                    & " ID_EMP = '" & jytsistema.WorkID & "' AND" _
                                                    & " NUMCAN = '" & .Text & "' GROUP BY CORREDOR ORDER BY CORREDOR  ", MyConn, nTablaCantidad, lblInfo)

                            dtCantidad = ds.Tables(nTablaCantidad)

                            If dtCantidad.Rows.Count > 0 Then
                                Dim fCont As Integer
                                For fCont = 0 To dtCantidad.Rows.Count - 1
                                    With dtCantidad.Rows(fCont)
                                        InsertEditBANCOSRenglonCaja(MyConn, lblInfo, True, cmbCaja.SelectedValue, UltimoCajaMasUno(MyConn, lblInfo, cmbCaja.SelectedValue), _
                                                                    CDate(txtFecha.Text), "CXC", "EN", lvCobranza.Items(lvCont).SubItems(2).Text, _
                                                                    "CT", lvCobranza.Items(lvCont).SubItems(7).Text, _
                                                                    .Item("CORREDOR"), .Item("IMPORTE"), "", lvCobranza.Items(lvCont).SubItems(4).Text, "", jytsistema.sFechadeTrabajo, _
                                                                    .Item("CANTIDAD"), "", "", "", jytsistema.sFechadeTrabajo, lvCobranza.Items(lvCont).SubItems(1).Text, cmbAsesores.SelectedValue, "1")

                                    End With
                                Next
                            End If
                            dtCantidad.Dispose()
                            dtCantidad = Nothing

                        Else
                            InsertEditBANCOSRenglonCaja(MyConn, lblInfo, True, cmbCaja.SelectedValue, UltimoCajaMasUno(MyConn, lblInfo, cmbCaja.SelectedValue), _
                                                        CDate(txtFecha.Text), "CXC", "EN", .Text, _
                                                        .SubItems(9).Text, .SubItems(7).Text, .SubItems(8).Text, _
                                                        Math.Abs(ValorNumero(.SubItems(5).Text)), "", .SubItems(4).Text, txtNumeroDeposito.Text, jytsistema.sFechadeTrabajo, 1, "", "", _
                                                        "", jytsistema.sFechadeTrabajo, .SubItems(1).Text, cmbAsesores.SelectedValue, "1")

                            '3. Insertar Deposito en bancos
                            If (.SubItems(9).Text = "EF" Or .SubItems(9).Text = "CH") AndAlso txtNumeroDeposito.Text.Trim() <> "" Then
                                InsertEditBANCOSMovimientoBanco(MyConn, lblInfo, True, CDate(txtFecha.Text), txtNumeroDeposito.Text, "DP", cmbBancos.SelectedValue, _
                                                                cmbCaja.SelectedValue, "DEPOSITO CAJA DE " & txtCTotal.Text & " DOCUMENTOS ", ValorNumero(txtMTotal.Text), _
                                                                "CXC", txtNumeroDeposito.Text, "", "", "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, "CE", "", _
                                                                jytsistema.sFechadeTrabajo, "0", "", cmbAsesores.SelectedValue)
                            End If


                        End If


                        '4. Actualiza Historico a '1' = procesado en jsvencobrgv
                        ft.Ejecutar_strSQL(myconn, " update jsvencobrgv set historico = '1' where codcli = '" & .SubItems(1).Text & "' and nummov = '" & .SubItems(2).Text & "' and comproba = '" & .Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                    End If

                End With
            Next
            MsgBox("Proceso terminado...", MsgBoxStyle.Information, sModulo)

            ProgressBar1.Value = 0
            lblProgreso.Text = ""
            IniciarTXT()

            InsertarAuditoria(MyConn, MovAud.iProcesar, sModulo, txtFecha.Text)


        End If

    End Sub

    Private Sub btnFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFecha.Click
        txtFecha.Text = ft.FormatoFecha(SeleccionaFecha(CDate(txtFecha.Text), Me, btnFecha))
    End Sub


    Private Sub lvPedidos_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvCobranza.ItemChecked

        If e.Item.SubItems.Count > 4 Then
            If e.Item.Checked Then
                Select Case e.Item.SubItems(9).Text
                    Case "EF"
                        mEfectivo += ValorNumero(e.Item.SubItems(5).Text)
                        cEfectivo += 1
                    Case "CH"
                        mCheque += ValorNumero(e.Item.SubItems(5).Text)
                        cCheque += 1
                    Case "TA"
                        mTarjeta += ValorNumero(e.Item.SubItems(5).Text)
                        cTarjeta += 1
                    Case "CT"
                        mCupones += ValorNumero(e.Item.SubItems(5).Text)
                        cCupones += 1
                    Case "DP"
                        mDeposito += ValorNumero(e.Item.SubItems(5).Text)
                        cDeposito += 1
                    Case "TR"
                        mTransfer += ValorNumero(e.Item.SubItems(5).Text)
                        cTransfer += 1
                    Case Else
                        If e.Item.SubItems(3).Text = "NC" Then
                            If Mid(e.Item.SubItems(4).Text, 1, 13) = "RETENCION IVA" Then
                                mIVA += ValorNumero(e.Item.SubItems(5).Text)
                                cIVA += 1
                            Else
                                mISLR += ValorNumero(e.Item.SubItems(5).Text)
                                cISLR += 1
                            End If
                        End If
                End Select
            Else
                Select Case e.Item.SubItems(9).Text
                    Case "EF"
                        mEfectivo -= ValorNumero(e.Item.SubItems(5).Text)
                        cEfectivo -= 1
                    Case "CH"
                        mCheque -= ValorNumero(e.Item.SubItems(5).Text)
                        cCheque -= 1
                    Case "TA"
                        mTarjeta -= ValorNumero(e.Item.SubItems(5).Text)
                        cTarjeta -= 1
                    Case "CT"
                        mCupones -= ValorNumero(e.Item.SubItems(5).Text)
                        cCupones -= 1
                    Case "DP"
                        mDeposito -= ValorNumero(e.Item.SubItems(5).Text)
                        cDeposito -= 1
                    Case "TR"
                        mTransfer -= ValorNumero(e.Item.SubItems(5).Text)
                        cTransfer -= 1
                    Case Else
                        If e.Item.SubItems(3).Text = "NC" Then
                            If Mid(e.Item.SubItems(4).Text, 1, 13) = "RETENCION IVA" Then
                                mIVA -= ValorNumero(e.Item.SubItems(5).Text)
                                cIVA -= 1
                            Else
                                mISLR -= ValorNumero(e.Item.SubItems(5).Text)
                                cISLR -= 1
                            End If
                        End If
                End Select
            End If

            txtMEfectivo.Text = ft.FormatoNumero(mEfectivo)
            txtMCheques.Text = ft.FormatoNumero(mCheque)
            txtMTarjetas.Text = ft.FormatoNumero(mTarjeta)
            txtMCupones.Text = ft.FormatoNumero(mCupones)
            txtMDepositos.Text = ft.FormatoNumero(mDeposito)
            txtMTarjetas.Text = ft.FormatoNumero(mTransfer)
            txtMRetISLR.Text = ft.FormatoNumero(mISLR)
            txtMRetIVA.Text = ft.FormatoNumero(mIVA)

            txtCEfectivo.Text = ft.FormatoEntero(cEfectivo)
            txtCCheques.Text = ft.FormatoEntero(cCheque)
            txtCTarjetas.Text = ft.FormatoEntero(cTarjeta)
            txtCCupones.Text = ft.FormatoEntero(cCupones)
            txtCDepositos.Text = ft.FormatoEntero(cDeposito)
            txtCTransfer.Text = ft.FormatoEntero(cTransfer)
            txtCRetISLR.Text = ft.FormatoEntero(cISLR)
            txtCRetIVA.Text = ft.FormatoEntero(cIVA)

            txtMTotal.Text = ft.FormatoNumero(mEfectivo + mCheque + mTarjeta + mCupones + mDeposito + mTransfer)
            txtCTotal.Text = ft.FormatoEntero(cEfectivo + cCheque + cTarjeta + cCupones + cDeposito + cTransfer)

        End If

    End Sub

    Private Sub chkEF_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkEF.CheckedChanged, chkCH.CheckedChanged, _
        chkTA.CheckedChanged, chkCT.CheckedChanged, chkDP.CheckedChanged, chkTR.CheckedChanged, chkIVA.CheckedChanged, chkISLR.CheckedChanged, _
        txtFecha.TextChanged, cmbAsesores.SelectedIndexChanged

        CargarCancelaciones()

    End Sub

    Private Sub btnCestaTicket_Click(sender As System.Object, e As System.EventArgs) Handles btnCestaTicket.Click

        If lvCobranza.SelectedItems.Count > 0 Then
            With lvCobranza.SelectedItems.Item(0)
                If .SubItems(9).Text = "CT" Then
                    'Declarar dias para comision 
                    Dim DiasParaComision As Integer = 0
                    Dim FechaEmisionDocumento As Date = ft.DevuelveScalarFecha(MyConn, " SELECT a.emision " _
                                                                              & " FROM jsventracob a " _
                                                                              & " LEFT JOIN (SELECT a.codcli, a.nummov, a.tipomov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp " _
                                                                              & "            FROM jsventracob a " _
                                                                              & "            WHERE " _
                                                                              & "            a.codcli = '" & .SubItems(1).Text & "' AND " _
                                                                              & "            a.id_emp = '" & jytsistema.WorkID & "' " _
                                                                              & "            GROUP BY a.nummov) c ON (a.codcli = c.codcli AND a.nummov = c.nummov AND a.id_emp = c.id_emp) " _
                                                                              & " WHERE " _
                                                                              & " CONCAT(a.nummov, a.emision, a.hora) IN (SELECT MIN(CONCAT(nummov, emision, hora)) " _
                                                                              & "                                         FROM jsventracob a " _
                                                                              & "                                         WHERE " _
                                                                              & "                                         a.codcli = '" & .SubItems(1).Text & "' AND " _
                                                                              & "                                         a.nummov = '" & .SubItems(2).Text & "'  " _
                                                                              & "                                         GROUP BY nummov) AND  " _
                                                                              & " a.codcli = '" & .SubItems(1).Text & "'  AND " _
                                                                              & " a.nummov = '" & .SubItems(2).Text & "' AND " _
                                                                              & " a.codcli <> '' AND " _
                                                                              & " a.historico = '0' AND " _
                                                                              & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                                                              & " ORDER BY a.nummov, a.emision ")

                    Dim f As New jsVenArcCXCCestaTicket
                    DiasParaComision = DateDiff(DateInterval.Day, CDate(txtFecha.Text), FechaEmisionDocumento)

                    f.Cargar(MyConn, .Text, .SubItems(7).Text, Math.Abs(ValorNumero(.SubItems(5).Text)), cmbCaja.SelectedValue, DiasParaComision)

                    'Modifica monto de cancelación Cesta Ticket
                    Dim row As DataRow = dtCxC.Select(" comproba = '" & .Text & "' and codcli = '" & .SubItems(1).Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
                    row("importe") = -1 * Math.Abs(f.montoPago)
                    ft.Ejecutar_strSQL(myconn, " update jsvencobrgv set importe = " & -1 * Math.Abs(f.montoPago) & " where comproba = '" & .Text & "' and  codcli = '" & .SubItems(1).Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                    CargarCancelaciones()

                    f.Close()

                End If

            End With

        End If


    End Sub

End Class