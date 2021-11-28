Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsBanArcDepositarCaja
    Private Const sModulo As String = "Depositar Caja"
    Private Const nTablaDepositos As String = "noDepositos"

    Private strSQLDepos As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dtDepos As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Long
    Private CodigoBanco As String, tDeposito As Integer
    Private fTipoOrigen As String
    Private iCont As Integer, jCont As Integer
    Private iSel As Integer = 0, tSel As Integer = 0
    Private dSel As Double = 0.0, dISLR As Double = 0.0, dComision As Double = 0.0
    Private dCar As Double = 0.0, dIVA As Double = 0.0

    Private aCorredor() As String = {"codigo", "id_emp"}
    Private sCorredor(2) As String


    Private CodigoCaja As String
    Private n_Apuntador As Long

    Private m_SortingColumn As ColumnHeader

    Private aSeleccion() As String = {"Ninguno", "Todos", "Por fecha", "Por forma/referencia pago", "Por Fecha & Forma de Pago"}
    Private aSel() As String = {"EF", "CH", "TA", "CT"}
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Depositar(ByVal MyCon As MySqlConnection, ByVal dsBan As DataSet, ByVal CodBanco As String,
                         ByVal CodCaja As String, ByVal TipoDeposito As Integer, Optional ByVal Corredor As String = "")
        myConn = MyCon
        ds = dsBan
        CodigoBanco = CodBanco
        CodigoCaja = CodCaja
        tDeposito = TipoDeposito

        sCorredor(0) = Mid(Corredor, 1, 5)
        sCorredor(1) = jytsistema.WorkID

        lblTituloCaja.Text = IIf(tDeposito = 2, "Depositar remesas o sobres del corredor ", lblTituloCaja.Text)
        lblCaja.Text = IIf(tDeposito = 2, Corredor, CodCaja)
        ft.mensajeEtiqueta(lblInfo, " Escoja el o los documentos  a depositar e indique emision, número y/o concepto de este depósito ...", Transportables.tipoMensaje.iAyuda)
        txtEmision.Value = jytsistema.sFechadeTrabajo
        txtAjustes.Text = ft.FormatoNumero(0.0)
        txtDeposito.Text = IIf(tDeposito = 0, Contador(myConn, lblInfo, Gestion.iBancos, "BANNUMDEP", "04"), "")

        ft.habilitarObjetos(False, True, txtDocSel, txtSaldoSel, txtTotalDeposito)

        ft.RellenaCombo(aSeleccion, cmbSeleccion)
        ft.RellenaCombo(aSel, cmbFP)
        txtFechaSeleccion.Value = jytsistema.sFechadeTrabajo

        Dim dates As SfDateTimeEdit() = {txtEmision, txtFechaSeleccion}
        SetSizeDateObjects(dates)

        MovimientosCaja(Mid(lblCaja.Text, 1, 2), tDeposito, Mid(lblCaja.Text, 1, 5))
        Me.ShowDialog()

    End Sub
    Private Sub MovimientosCaja(ByVal numCaja As String, ByVal fp As Integer, Optional ByVal Corredor As String = "")
        Dim strForma As String
        Select Case fp
            Case 0
                strForma = "'CH','EF'"
                fTipoOrigen = "CE"
                Verlinea(False, False, False, False, False, False)
                strSQLDepos = "select fecha, nummov, tipomov, formpag, numpag, refpag, importe, concepto, origen, cantidad, id_emp " _
                    & " from jsbantracaj where " _
                    & " id_emp = '" & jytsistema.WorkID & "' and " _
                    & " deposito = '' and " _
                    & " formpag IN (" & strForma & ") and " _
                    & " fecha <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
                    & " caja = '" & numCaja & "' and " _
                    & " ejercicio = '" & jytsistema.WorkExercise & "' " _
                    & " order by fecha desc, formpag "
            Case 1
                strForma = "'TA'"
                fTipoOrigen = "TA"
                Verlinea(False, False, True, True, False, False)
                strSQLDepos = "select fecha, nummov, tipomov, formpag, numpag, refpag, importe, concepto, origen, cantidad, id_emp " _
                            & " from jsbantracaj where " _
                            & " id_emp = '" & jytsistema.WorkID & "' and " _
                            & " deposito = '' and " _
                            & " formpag IN (" & strForma & ") and " _
                            & " fecha <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
                            & " caja = '" & numCaja & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' " _
                            & " order by fecha desc, formpag "
            Case 2
                strForma = "'CT'"
                fTipoOrigen = "CT"
                Verlinea(True, True, True, False, True, True)
                strSQLDepos = "select COUNT(TICKET) as TICKETS, SUM(MONTO) AS MONTO, NUMCAN, CORREDOR, NUMSOBRE, FECHASOBRE, NUMDEP " _
                    & " from jsventabtic " _
                    & " where ID_EMP = '" & jytsistema.WorkID & "' AND " _
                    & " NUMDEP = '' and " _
                    & " NUMSOBRE <> '' and " _
                    & " corredor = '" & Corredor & "' " _
                    & " GROUP BY corredor, NUMSOBRE " _
                    & " order by corredor, NUMSOBRE "
            Case Else
                strForma = ""
        End Select

        ds = DataSetRequery(ds, strSQLDepos, myConn, nTablaDepositos, lblInfo)
        dtDepos = ds.Tables(nTablaDepositos)

        CargaListViewDesdeCAJ(lv, dtDepos, IIf(fp = 2, 1, 0))


        txtDocSel.Text = ft.FormatoEntero(0)
        txtSaldoSel.Text = ft.FormatoNumero(0.0)
        txtAjustes.Text = ft.FormatoNumero(0.0)

    End Sub
    Private Sub Verlinea(ByVal Tickets As Boolean, ByVal Ajustes As Boolean, ByVal Comision As Boolean,
        ByVal ISLR As Boolean, ByVal Cargos As Boolean, ByVal IVA As Boolean)
        Label11.Visible = Tickets : txtTickets.Visible = Tickets : Label10.Visible = Tickets : txtNumControl.Visible = Tickets
        Label13.Visible = Ajustes : txtAjustes.Visible = Ajustes : txtAjustes.Enabled = True
        Label9.Visible = Comision : txtComision.Visible = Comision : ft.habilitarObjetos(False, True, txtComision)
        Label12.Visible = ISLR : txtISRL.Visible = ISLR : ft.habilitarObjetos(False, True, txtISRL)
        Label8.Visible = Cargos : txtCargos.Visible = Cargos
        Label7.Visible = IVA : txtIVA.Visible = IVA
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub lv_ColumnClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lv.ColumnClick
        Dim new_sorting_column As ColumnHeader = lv.Columns(e.Column)
        Dim sort_order As System.Windows.Forms.SortOrder
        If m_SortingColumn Is Nothing Then
            ' New column. Sort ascending.  
            sort_order = SortOrder.Ascending
        Else ' See if this is the same column.  
            If new_sorting_column.Equals(m_SortingColumn) Then
                ' Same column. Switch the sort order.  
                If m_SortingColumn.Text.StartsWith("> ") Then
                    sort_order = SortOrder.Descending
                Else
                    sort_order = SortOrder.Ascending
                End If
            Else
                ' New column. Sort ascending.  
                sort_order = SortOrder.Ascending
            End If
            ' Remove the old sort indicator.  
            m_SortingColumn.Text = m_SortingColumn.Text.Substring(2)
        End If
        ' Display the new sort order.  
        m_SortingColumn = new_sorting_column
        If sort_order = SortOrder.Ascending Then
            m_SortingColumn.Text = "> " & m_SortingColumn.Text
        Else
            m_SortingColumn.Text = "< " & m_SortingColumn.Text
        End If
        ' Create a comparer.  
        lv.ListViewItemSorter = New clsListviewSorter(e.Column, sort_order)
        ' Sort.  
        lv.Sort()

        txtBuscar.Focus()

    End Sub
    Private Sub lv_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lv.ItemChecked
        Dim aTarjetaBan() As String = {"codban", "codtar", "id_emp"}
        Dim aTarjeta() As String = {"codtar", "id_emp"}
        Dim TipoIVA As String = qFoundAndSign(myConn, lblInfo, "jsvencestic", aCorredor, sCorredor, "tipoiva")
        Dim Porcentajecomision As Double = 0.0
        Dim PorcentajeISLR As Double = 0.0
        Dim aString(2) As String
        Dim aStringBan(3) As String

        If tDeposito = 2 Then
            If e.Item.Checked Then
                iSel += 1
                dSel += CDbl(e.Item.SubItems(4).Text)
                tSel += CInt(e.Item.SubItems(3).Text)
                dComision -= ComisionCorredorSobre(myConn, ds, e.Item.SubItems(2).Text, e.Item.SubItems(1).Text)
                dCar -= CDbl(qFoundAndSign(myConn, lblInfo, "jsvencestic", aCorredor, sCorredor, "cargos"))
                dIVA = -1 * Math.Round((Math.Abs(dComision) + Math.Abs(dCar)) * PorcentajeIVA(myConn, lblInfo, txtEmision.Value, TipoIVA) / 100, 2)
            Else
                iSel -= 1
                dSel -= CDbl(e.Item.SubItems(4).Text)
                tSel -= CInt(e.Item.SubItems(3).Text)
                dComision += ComisionCorredorSobre(myConn, ds, e.Item.SubItems(2).Text, e.Item.SubItems(1).Text)
                dCar += CDbl(qFoundAndSign(myConn, lblInfo, "jsvencestic", aCorredor, sCorredor, "cargos"))
                dIVA = -1 * Math.Round((Math.Abs(dComision) + Math.Abs(dCar)) * PorcentajeIVA(myConn, lblInfo, txtEmision.Value, TipoIVA) / 100, 2)

            End If
        Else
            If e.Item.Checked Then
                iSel += 1
                dSel += CDbl(e.Item.SubItems(6).Text)
                Select Case e.Item.SubItems(3).Text
                    Case "TA"
                        aStringBan(0) = CodigoBanco : aStringBan(1) = IIf(IsDBNull(e.Item.SubItems(5).Text), "", e.Item.SubItems(5).Text) : aStringBan(2) = jytsistema.WorkID
                        If qFound(myConn, lblInfo, "jsbancatbantar", aTarjetaBan, aStringBan) Then
                            dComision -= CDbl(e.Item.SubItems(6).Text) * CDbl(qFoundAndSign(myConn, lblInfo, "jsbancatbantar", aTarjetaBan, aStringBan, "com1")) / 100
                            dISLR -= CDbl(e.Item.SubItems(6).Text) * CDbl(qFoundAndSign(myConn, lblInfo, "jsbancatbantar", aTarjetaBan, aStringBan, "com2")) / 100
                        Else
                            If IsDBNull(e.Item.SubItems(5).Text) Then
                                dComision -= 0.0
                                dISLR -= 0.0
                            Else
                                aString(0) = IIf(IsDBNull(e.Item.SubItems(5).Text), "", e.Item.SubItems(5).Text) : aString(1) = jytsistema.WorkID
                                dComision -= CDbl(e.Item.SubItems(6).Text) * CDbl(qFoundAndSign(myConn, lblInfo, "jsconctatar", aTarjeta, aString, "com1")) / 100
                                dISLR -= CDbl(e.Item.SubItems(6).Text) * CDbl(qFoundAndSign(myConn, lblInfo, "jsconctatar", aTarjeta, aString, "com2")) / 100
                            End If

                        End If
                End Select
            Else
                iSel -= 1
                If e.Item.SubItems.Count > 1 Then
                    dSel -= CDbl(e.Item.SubItems(6).Text)
                    Select Case e.Item.SubItems(3).Text
                        Case "TA"
                            aStringBan(0) = CodigoBanco : aStringBan(1) = IIf(IsDBNull(e.Item.SubItems(5).Text), "", e.Item.SubItems(5).Text) : aStringBan(2) = jytsistema.WorkID
                            If qFound(myConn, lblInfo, "jsbancatbantar", aTarjetaBan, aStringBan) Then
                                dComision += CDbl(e.Item.SubItems(6).Text) * CDbl(qFoundAndSign(myConn, lblInfo, "jsbancatbantar", aTarjetaBan, aStringBan, "com1")) / 100
                                dISLR += CDbl(e.Item.SubItems(6).Text) * CDbl(qFoundAndSign(myConn, lblInfo, "jsbancatbantar", aTarjetaBan, aStringBan, "com2")) / 100
                            Else
                                If IsDBNull(e.Item.SubItems(5).Text) Then
                                    dComision -= 0.0
                                    dISLR -= 0.0
                                Else
                                    aString(0) = IIf(IsDBNull(e.Item.SubItems(5).Text), "", e.Item.SubItems(5).Text) : aString(1) = jytsistema.WorkID
                                    dComision += CDbl(e.Item.SubItems(6).Text) * CDbl(qFoundAndSign(myConn, lblInfo, "jsconctatar", aTarjeta, aString, "com1")) / 100
                                    dISLR += CDbl(e.Item.SubItems(6).Text) * CDbl(qFoundAndSign(myConn, lblInfo, "jsconctatar", aTarjeta, aString, "com2")) / 100
                                End If

                            End If
                    End Select
                End If
            End If
        End If

        If iSel < 0 Then
            iSel = 0
            tSel = 0
            dSel = 0.0
            dCar = 0.0
            dComision = 0.0
            dISLR = 0.0
            dIVA = 0.0
        End If

        txtDocSel.Text = ft.FormatoEntero(iSel)
        txtTickets.Text = ft.FormatoEntero(tSel)
        txtComision.Text = ft.FormatoNumero(dComision)
        txtCargos.Text = ft.FormatoNumero(dCar)
        txtISRL.Text = ft.FormatoNumero(dISLR)
        txtIVA.Text = ft.FormatoNumero(dIVA)
        txtSaldoSel.Text = ft.FormatoNumero(dSel)
        txtConcepto.Text = "DEPOSITO CAJA DE " & iSel.ToString & " DOCUMENTOS "

    End Sub
    Private Function ComisionCorredorSobre(ByVal MyConn As MySqlConnection, ByVal dsC As DataSet, ByVal Corredor As String, ByVal numSobre As String) As Double
        Dim nTablaC As String = "comisioncorredorsobre"
        Dim strC As String
        Dim Inicio As Integer, Longitud As Integer
        Dim iCont As Integer
        ComisionCorredorSobre = 0.0
        If CorredorTipo(MyConn, dsC, Corredor) Then
            Inicio = CInt(qFoundAndSign(MyConn, lblInfo, "jsvencestic", aCorredor, sCorredor, "iniciotipo"))
            Longitud = CInt(qFoundAndSign(MyConn, lblInfo, "jsvencestic", aCorredor, sCorredor, "lentipo"))

            strC = "select substring(a.ticket," & Inicio & " , " & Longitud & " ) tipo, sum(a.monto)*b.com_corredor/100 monto " _
                & " from jsventabtic a " _
                & " left join jsvencestip b on (a.corredor = b.corredor and substring(a.ticket, " & Inicio & ", " & Longitud & " ) = b.tipo) " _
                & " Where " _
                & " b.id_emp = '" & jytsistema.WorkID & "' and " _
                & " a.corredor = '" & Corredor & "' and " _
                & " a.numsobre = '" & numSobre & "' " _
                & " group by 1 "
        Else
            strC = "select '00', sum(a.monto)*b.com_corredor/100 monto " _
                & " from jsventabtic a " _
                & " left join jsvencestip b on (a.corredor = b.corredor and '00' = b.tipo) " _
                & " Where " _
                & " b.id_emp = '" & jytsistema.WorkID & "' and " _
                & " a.corredor = '" & Corredor & "' and " _
                & " a.numsobre = '" & numSobre & "' " _
                & " group by 1 "
        End If
        dsC = DataSetRequery(dsC, strC, MyConn, nTablaC, lblInfo)
        If dsC.Tables(nTablaC).Rows.Count > 0 Then
            For iCont = 0 To dsC.Tables(nTablaC).Rows.Count - 1
                ComisionCorredorSobre += dsC.Tables(nTablaC).Rows(iCont).Item("monto")
            Next
        End If
        dsC.Tables(nTablaC).Dispose()

    End Function
    Private Function CorredorTipo(ByVal MyConn As MySqlConnection, ByVal dsCT As DataSet, ByVal Corredor As String) As Boolean
        If CInt(qFoundAndSign(MyConn, lblInfo, "jsvencestic", aCorredor, sCorredor, "iniciotipo")) > 0 Then Return True
    End Function

    Private Sub jsBanDepositarCaja_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        InsertarAuditoria(myConn, MovAud.iSalir, sModulo, CodigoBanco)
    End Sub

    Private Sub jsBanDepositarCaja_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        InsertarAuditoria(myConn, MovAud.ientrar, sModulo, CodigoBanco)
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarDeposito()
            Me.Close()
        End If
    End Sub
    Private Sub GuardarDeposito()
        InsertarAuditoria(myConn, MovAud.iIncluir, sModulo, txtDeposito.Text)

        Dim CodigoProveedor As String = ""
        If tDeposito = 2 Then CodigoProveedor = CStr(qFoundAndSign(myConn, lblInfo, "jsvencestic", aCorredor, sCorredor, "codpro"))

        InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, txtEmision.Value, txtDeposito.Text, "DP", CodigoBanco,
                 IIf(tDeposito = 2, "", Mid(lblCaja.Text, 1, 2)), txtConcepto.Text, ValorNumero(txtTotalDeposito.Text), "CAJ",
                 IIf(tDeposito = 2, txtNumControl.Text, ""), "", "", "0",
                 jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, fTipoOrigen, "", jytsistema.sFechadeTrabajo, "0",
                 CodigoProveedor, "", jytsistema.WorkCurrency.Id, DateTime.Now())

        If tDeposito <> 2 Then
            For iCont = 0 To lv.Items.Count - 1
                If lv.Items(iCont).Checked Then

                    ft.Ejecutar_strSQL(myConn, "UPDATE jsbantracaj SET DEPOSITO = '" & txtDeposito.Text & "',  " _
                        & " fecha_dep = '" & ft.FormatoFechaMySQL(txtEmision.Value) & "', " _
                        & " codban = '" & CodigoBanco & "' " _
                        & " where " _
                        & " caja = '" & Mid(CodigoCaja, 1, 2) & "' and " _
                        & " tipomov ='" & lv.Items(iCont).SubItems(1).Text & "' and " _
                        & " fecha = '" & ft.FormatoFechaMySQL(CDate(lv.Items(iCont).Text)) & "' and " _
                        & " nummov = '" & lv.Items(iCont).SubItems(2).Text & "' and " _
                        & " formpag = '" & lv.Items(iCont).SubItems(3).Text & "' and " _
                        & " numpag = '" & lv.Items(iCont).SubItems(4).Text & "' and " _
                        & " refpag = '" & lv.Items(iCont).SubItems(5).Text & "' and " _
                        & " importe = " & ValorNumero(lv.Items(iCont).SubItems(6).Text) & "  and " _
                        & " origen = '" & lv.Items(iCont).SubItems(7).Text & "' and " _
                        & " id_emp ='" & jytsistema.WorkID & "' and " _
                        & " ejercicio = '" & jytsistema.WorkExercise & "'")

                End If
            Next
        Else


            Dim aCam() As String = {"codpro", "id_emp"}
            Dim aStr() As String = {CodigoProveedor, jytsistema.WorkID}

            Dim NombreProveedor As String = CStr(qFoundAndSign(myConn, lblInfo, "jsprocatpro", aCam, aStr, "nombre"))
            Dim RIFProveedor As String = CStr(qFoundAndSign(myConn, lblInfo, "jsprocatpro", aCam, aStr, "rif"))
            Dim NITProveedor As String = CStr(qFoundAndSign(myConn, lblInfo, "jsprocatpro", aCam, aStr, "nit"))
            Dim CodigoContable As String = CStr(qFoundAndSign(myConn, lblInfo, "jsvencestic", aCorredor, sCorredor, "codcon"))
            Dim CodigoGrupo As String = CStr(qFoundAndSign(myConn, lblInfo, "jsvencestic", aCorredor, sCorredor, "grupo"))
            Dim CodigoSubgrupo As String = CStr(qFoundAndSign(myConn, lblInfo, "jsvencestic", aCorredor, sCorredor, "subgrupo"))
            Dim TipoIVA As String = CStr(qFoundAndSign(myConn, lblInfo, "jsvencestic", aCorredor, sCorredor, "tipoiva"))
            Dim BaseIVA As Double = ValorNumero(txtComision.Text) + ValorNumero(txtCargos.Text)
            Dim TotalGasto As Double = BaseIVA + ValorNumero(txtIVA.Text)


            InsertEditCOMPRASEncabezadoGasto(myConn, lblInfo, True, txtNumControl.Text, txtNumControl.Text, "", txtEmision.Value,
                txtEmision.Value, CodigoProveedor, CodigoProveedor, NombreProveedor, RIFProveedor,
                NITProveedor, "GASTOS CHEQUES ALIMENTACION", "", "", CodigoContable, CodigoGrupo,
                CodigoSubgrupo, BaseIVA, 0, 0, 0, TipoIVA, PorcentajeIVA(myConn, lblInfo, txtEmision.Value, TipoIVA), BaseIVA,
                ValorNumero(txtIVA.Text), TotalGasto, txtEmision.Value, 1, 0, "EF", txtDeposito.Text, "", "", "", 0.0#,
                "", 0, 0, 0, 0, "", jytsistema.sFechadeTrabajo, 0, "", "COM", "", "", "0",
                jytsistema.WorkCurrency.Id, jytsistema.sFechadeTrabajo)

            For iCont = 0 To lv.Items.Count - 1
                If lv.Items(iCont).Checked Then
                    ft.Ejecutar_strSQL(myConn, "UPDATE jsventabtic SET NUMDEP = '" & txtDeposito.Text & "', " _
                                    & " FECHADEP = '" & ft.FormatoFechaMySQL(txtEmision.Value) & "', " _
                                    & " BANCODEP = '" & CodigoBanco & "' " _
                                    & " where " _
                                    & " CORREDOR = '" & Mid(lblCaja.Text, 1, 5) & "' and " _
                                    & " NUMSOBRE = '" & lv.Items(iCont).SubItems(1).Text & "' and " _
                                    & " ID_EMP ='" & jytsistema.WorkID & "' ")
                End If
            Next

            Dim nTablaTickets As String = "tblticketsdeposito"

            ds = DataSetRequery(ds, "select * from jsventabtic WHERE NUMDEP = '" & txtDeposito.Text & "' AND " _
                & " BANCODEP = '" & CodigoBanco & "' AND " _
                & " ID_EMP = '" & jytsistema.WorkID & "' group by numcan, corredor  ", myConn, nTablaTickets, lblInfo)

            Dim tCont As Integer
            With ds.Tables(nTablaTickets)
                If .Rows.Count > 0 Then
                    For tCont = 0 To .Rows.Count - 1
                        If .Rows(tCont).Item("numdep") <> "" Then
                            ft.Ejecutar_strSQL(myConn, "update jsbantracaj set " _
                             & " DEPOSITO = '" & .Rows(tCont).Item("NUMDEP") & "', " _
                            & " FECHA_DEP = '" & ft.FormatoFechaMySQL(CDate(.Rows(tCont).Item("FECHADEP").ToString)) & "', " _
                            & " CODBAN = '" & .Rows(tCont).Item("BANCODEP") & "' WHERE " _
                            & " NUMMOV = '" & .Rows(tCont).Item("NUMCAN") & "' AND " _
                            & " TIPOMOV = 'EN' AND " _
                            & " FORMPAG = 'CT' AND " _
                            & " REFPAG = '" & .Rows(tCont).Item("CORREDOR") & "' AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "' ")
                        End If
                    Next
                End If
            End With


        End If

    End Sub
    Private Function Validado() As Boolean
        Validado = False
        If ValorEntero(txtDocSel.Text) = 0 Then
            ft.mensajeAdvertencia("Debe seleccionar al menos un documento ... ")
            Exit Function
        End If

        If txtDeposito.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar un numero de depósito válido ...")
            ft.enfocarTexto(txtDeposito)
            Exit Function
        Else
            If Not DocumentoValido() Then
                ft.mensajeAdvertencia("Documento ya Existe en Banco")
                ft.enfocarTexto(txtDeposito)
                Exit Function
            End If
        End If

        If ValorNumero(txtTotalDeposito.Text) < 0 Then
            ft.mensajeAdvertencia("El monto del depósito debe ser mayor o igual a cero")
            Exit Function
        End If

        If tDeposito = 2 Then

            If Not ft.isNumeric(txtAjustes.Text) Then
                ft.mensajeAdvertencia("Debe indicar un monto por ajuste válido...")
                Return False
            End If

            If txtNumControl.Text = "" Then
                ft.mensajeAdvertencia("Debe indicar un número de control valido...")
                Return False
            Else
                Dim aStrGasto() As String = {txtNumControl.Text, jytsistema.WorkExercise, jytsistema.WorkID}
                Dim aCamGasto() As String = {"numgas", "ejercicio", "id_emp"}
                If qFound(myConn, lblInfo, "jsproencgas", aCamGasto, aStrGasto) Then
                    ft.mensajeAdvertencia("Número de control YA existe...")
                    Return False
                End If
            End If

        End If

        Validado = True

    End Function
    Private Function DocumentoValido() As Boolean
        Dim strSQLExiste As String = "select IFNULL(count(*),0) AS cuenta from jsbantraban " _
            & " where CODBAN = '" & CodigoBanco & "' and NUMDOC = '" & txtDeposito.Text & "' " _
            & " and TIPOMOV = 'DP'  " _
            & " and EJERCICIO = '" & jytsistema.WorkExercise & "' " _
            & " and ID_EMP = '" & jytsistema.WorkID & "'"
        Dim dtExiste As DataTable
        Dim tblExiste As String = "tblExiste"
        ds = DataSetRequery(ds, strSQLExiste, myConn, tblExiste, lblInfo)
        dtExiste = ds.Tables(tblExiste)
        If dtExiste.Rows.Count > 0 Then
            If dtExiste.Rows(0).Item("cuenta") > 0 Then
                DocumentoValido = False
            Else
                DocumentoValido = True
            End If
        Else
            DocumentoValido = False
        End If
        dtExiste = Nothing
    End Function

    Private Sub txtDeposito_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDeposito.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el número deposito ... ", Transportables.tipoMensaje.iInfo)
        txtDeposito.MaxLength = 15
    End Sub

    Private Sub txtConcepto_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConcepto.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el concepto o comentario para este deposito ... ", Transportables.tipoMensaje.iInfo)
        txtDeposito.MaxLength = 250
    End Sub

    Private Sub btnEmision_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        ft.mensajeEtiqueta(lblInfo, "seleccione la fecha de emisión de este depósito...", Transportables.tipoMensaje.iInfo)
    End Sub
    Private Sub txtNumControl_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNumControl.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el Nº de Control ...", Transportables.tipoMensaje.iInfo)
    End Sub
    Private Sub txtAjustes_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAjustes.TextChanged,
        txtComision.TextChanged, txtCargos.TextChanged, txtISRL.TextChanged, txtIVA.TextChanged, txtSaldoSel.TextChanged

        txtTotalDeposito.Text = ft.FormatoNumero(ValorNumero(txtSaldoSel.Text) + ValorNumero(txtAjustes.Text) + ValorNumero(txtComision.Text) +
            ValorNumero(txtCargos.Text) + ValorNumero(txtISRL.Text) + ValorNumero(txtIVA.Text))

    End Sub
    Private Sub cmbSeleccion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSeleccion.SelectedIndexChanged
        txtRP.Text = ""
        Select Case cmbSeleccion.SelectedIndex
            Case 2
                ft.visualizarObjetos(True, txtFechaSeleccion)
                ft.visualizarObjetos(False, cmbFP, txtRP, btnBP)
            Case 3
                ft.visualizarObjetos(False, txtFechaSeleccion)
                ft.visualizarObjetos(True, cmbFP, txtRP, btnBP)
            Case 4
                ft.visualizarObjetos(True, txtFechaSeleccion)
                ft.visualizarObjetos(True, cmbFP, txtRP, btnBP)
            Case Else
                ft.visualizarObjetos(False, txtFechaSeleccion)
                ft.visualizarObjetos(False, cmbFP, txtRP, btnBP)
        End Select
    End Sub

    Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Dim kCont As Integer
        Select Case cmbSeleccion.SelectedIndex
            Case 0 'Ninguno 
                For kCont = 0 To lv.Items.Count - 1
                    lv.Items(kCont).Checked = False
                Next
            Case 1 'Todos
                For kCont = 0 To lv.Items.Count - 1
                    lv.Items(kCont).Checked = True
                Next
            Case 2 ' Por fecha
                For kCont = 0 To lv.Items.Count - 1
                    If ft.FormatoFechaMySQL(CDate(lv.Items(kCont).SubItems(0).Text)) = ft.FormatoFechaMySQL(txtFechaSeleccion.Value) Then lv.Items(kCont).Checked = True
                Next
            Case 3 ' forma de pago y referencia pago 
                For kCont = 0 To lv.Items.Count - 1
                    If txtRP.Text <> "" Then
                        If lv.Items(kCont).SubItems(3).Text = cmbFP.Text AndAlso
                            lv.Items(kCont).SubItems(5).Text = txtRP.Text Then lv.Items(kCont).Checked = True
                    Else
                        If lv.Items(kCont).SubItems(3).Text = cmbFP.SelectedText Then lv.Items(kCont).Checked = True

                    End If
                Next
            Case 4
                For kCont = 0 To lv.Items.Count - 1
                    If txtRP.Text <> "" Then
                        If lv.Items(kCont).SubItems(3).Text = cmbFP.Text AndAlso
                            lv.Items(kCont).SubItems(5).Text = txtRP.Text AndAlso
                             ft.FormatoFechaMySQL(CDate(lv.Items(kCont).SubItems(0).Text)) = ft.FormatoFechaMySQL(txtFechaSeleccion.Value) Then
                            lv.Items(kCont).Checked = True
                        End If

                    Else
                        If lv.Items(kCont).SubItems(3).Text = cmbFP.Text And
                            ft.FormatoFechaMySQL(CDate(lv.Items(kCont).SubItems(0).Text)) = ft.FormatoFechaMySQL(txtFechaSeleccion.Value) Then
                            lv.Items(kCont).Checked = True
                        End If


                    End If

                Next

        End Select
    End Sub

    Private Sub btnBP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBP.Click
        txtRP.Text = CargarTablaSimplePlusReal("Bancos", Modulo.iBancos)
    End Sub

    Private Sub txtBuscar_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        ' 
        Dim foundItem As ListViewItem =
            lv.FindItemWithText(txtBuscar.Text, True, 0)

        If (foundItem IsNot Nothing) Then
            lv.TopItem = foundItem
        End If

    End Sub

    Private Sub jsBanArcDepositarCaja_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        txtBuscar.Focus()
    End Sub
End Class