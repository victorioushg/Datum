Imports MySql.Data.MySqlClient
Imports fTransport
Public Class jsBanArcDepositarCajaPlus
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

    Private tblCaja As String = ""

    Private CodigoCaja As String
    Private n_Apuntador As Long

    Private aCampos() As String
    Private FindField As String
    Private aNombres() As String

    Private BindingSource1 As New BindingSource


    Private aSeleccion() As String = {"Ninguno", "Todos", "fecha", "FormaPago/Refer.Pago", "Fecha/FormaPago", _
                                      "Vendedor/Fecha/FormaPago", "Vendedor/Fecha/FormaPago/Refer.Pago"}

    Private aSel() As String = {"EF", "CH", "TA", "CT"}
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Depositar(ByVal MyCon As MySqlConnection, ByVal dsBan As DataSet, ByVal CodBanco As String, _
                         ByVal CodCaja As String, ByVal TipoDeposito As Integer, Optional ByVal Corredor As String = "")

        'TipoDeposito 0 = Efectivo, Cheques ; 1 = Tarjetas; 2 = Cheques de Alimentación 

        myConn = MyCon
        ds = dsBan
        CodigoBanco = CodBanco
        CodigoCaja = CodCaja
        tDeposito = TipoDeposito
        tblCaja = "tblcaja" & ft.NumeroAleatorio(100000)
        sCorredor(0) = Mid(Corredor, 1, 5)
        sCorredor(1) = jytsistema.WorkID

        lblTituloCaja.Text = IIf(tDeposito = 2, "Depositar remesas o sobres del corredor ", lblTituloCaja.Text)
        lblCaja.Text = IIf(tDeposito = 2, Corredor, CodCaja)
        ft.mensajeEtiqueta(lblInfo, " Escoja el o los documentos  a depositar e indique emision, número y/o concepto de este depósito ...", Transportables.tipoMensaje.iAyuda)
        txtEmision.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtAjustes.Text = ft.FormatoNumero(0.0)
        txtDeposito.Text = IIf(tDeposito = 0, Contador(myConn, lblInfo, Gestion.iBancos, "BANNUMDEP", "04"), "")

        ft.habilitarObjetos(False, True, txtFechaSeleccion, txtDocSel, txtSaldoSel, txtEmision, txtTotalDeposito)

        ft.RellenaCombo(aSeleccion, cmbSeleccion)

        txtFechaSeleccion.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)

        MovimientosCaja(Mid(lblCaja.Text, 1, 2), tDeposito, Mid(lblCaja.Text, 1, 5))



        Dim iRow As DataGridViewRow
        For Each iRow In dg.Rows
            iRow.Selected = False
        Next

        Me.ShowDialog()

    End Sub
    Private Sub MovimientosCaja(ByVal numCaja As String, ByVal fp As Integer, Optional ByVal Corredor As String = "")
        Dim strForma As String


        Select Case fp
            Case 0
                aSel = DeleteArrayValuePlus(aSel, "TA")
                aSel = DeleteArrayValuePlus(aSel, "CT")

                strForma = "'CH','EF'"
                fTipoOrigen = "CE"
                Verlinea(False, False, False, False, False, False)

                Dim aFields() As String = {"sel.entero.1.0", "fecha.fecha.0.0", "nummov.cadena.15.0", "tipomov.cadena.2.0", "formpag.cadena.2.0", _
                                           "numpag.cadena.30.0", "refpag.cadena.50.0", "importe.doble.19.2", "concepto.cadena.250.0", _
                                           "origen.cadena.3.0", "cantidad.entero.4.0", "codven.cadena.5.0", "id_emp.cadena.2.0"}

                CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblCaja, aFields)

                ft.Ejecutar_strSQL(myConn, " insert into " & tblCaja _
                    & " select 0 sel, fecha, nummov, tipomov, formpag, numpag, refpag, importe, concepto, origen, cantidad, codven, id_emp " _
                    & " from jsbantracaj where " _
                    & " id_emp = '" & jytsistema.WorkID & "' and " _
                    & " deposito = '' and " _
                    & " formpag IN (" & strForma & ") and " _
                    & " fecha <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
                    & " caja = '" & numCaja & "' and " _
                    & " ejercicio = '" & jytsistema.WorkExercise & "' " _
                    & " order by fecha desc, formpag ")


                aCampos = {"sel", "fecha", "tipomov", "nummov", "formpag", "numpag", "refpag", "importe", "origen", "Cantidad", "codven"}
                aNombres = {"", "Emisión", "TP", "Número", "FP", "Número Pago", "Ref. Pago", "Importe", "ORG", "Cant.", "Vendedor"}

                lblBuscar.Text = "Ordenado por : " & "Número Pago"
                FindField = "numpag"


            Case 1
                aSel = DeleteArrayValuePlus(aSel, "EF")
                aSel = DeleteArrayValuePlus(aSel, "CH")
                aSel = DeleteArrayValuePlus(aSel, "CT")
                strForma = "'TA'"
                fTipoOrigen = "TA"
                Verlinea(False, False, True, True, False, False)

                Dim aFields() As String = {"sel.entero.1.0", "fecha.fecha.0.0", "nummov.cadena.15.0", "tipomov.cadena.2.0", "formpag.cadena.2.0", "numpag.cadena.20.0", _
                                           "refpag.cadena.50.0", "importe.doble.19.2", "concepto.cadena.250.0", "origen.cadena.3.0", "cantidad.entero.4.0", _
                                           "codven.cadena.5.0", "id_emp.cadena.2.0"}
                CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblCaja, aFields)

                ft.Ejecutar_strSQL(myConn, " insert into " & tblCaja _
                            & " select 0 sel, fecha, nummov, tipomov, formpag, numpag, refpag, importe, concepto, origen, cantidad, codven, id_emp " _
                            & " from jsbantracaj where " _
                            & " id_emp = '" & jytsistema.WorkID & "' and " _
                            & " deposito = '' and " _
                            & " formpag IN (" & strForma & ") and " _
                            & " fecha <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
                            & " caja = '" & numCaja & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' " _
                            & " order by fecha desc, formpag ")

                aCampos = {"sel", "fecha", "tipomov", "nummov", "formpag", "numpag", "refpag", "importe", "origen", "Cantidad", "codven"}
                aNombres = {"", "Emisión", "TP", "Número", "FP", "Número Pago", "Ref. Pago", "Importe", "ORG", "Tickets", "Vendedor"}

                lblBuscar.Text = "Ordenado por : " & "Número Pago"
                FindField = "numpag"

            Case 2
                DeleteArrayValuePlus(aSel, "TA")
                DeleteArrayValuePlus(aSel, "EF")
                DeleteArrayValuePlus(aSel, "CH")
                strForma = "'CT'"
                fTipoOrigen = "CT"
                Verlinea(True, True, True, False, True, True)

                Dim aFields() As String = {"sel.entero.1.0", "TICKETS.entero.6.0", "MONTO.doble.19.2", "NUMCAN.cadena.20.0", "CORREDOR.cadena.15.0", "NUMSOBRE.cadena.20.0", _
                                           "FECHASOBRE.fecha.0.0", "NUMDEP.cadena.20.0", "id_emp.cadena.2.0"}
                CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblCaja, aFields)

                ft.Ejecutar_strSQL(myConn, " insert into " & tblCaja _
                   & " select 0 sel, COUNT(TICKET) as TICKETS, SUM(MONTO) AS MONTO, NUMCAN, CORREDOR, NUMSOBRE, FECHASOBRE, NUMDEP, ID_EMP " _
                   & " from jsventabtic " _
                   & " where ID_EMP = '" & jytsistema.WorkID & "' AND " _
                   & " NUMDEP = '' and " _
                   & " NUMSOBRE <> '' and " _
                   & " corredor = '" & Corredor & "' " _
                   & " GROUP BY corredor, NUMSOBRE " _
                   & " order by corredor, NUMSOBRE ")

                aCampos = {"sel", "FECHASOBRE", "NUMSOBRE", "CORREDOR", "TICKETS", "MONTO"}
                aNombres = {"", "Emisión", "Nº Sobre/Remesa", "Corredor", "Cantidad Tickets", "Importe"}

                lblBuscar.Text = "Ordenado por : " & "Nº Sobre/Remesa"
                FindField = "numsobre"

            Case Else
                strForma = ""
        End Select

        ft.RellenaCombo(aSel, cmbFP)

        ds = DataSetRequery(ds, " select * from " & tblCaja, myConn, nTablaDepositos, lblInfo)
        dtDepos = ds.Tables(nTablaDepositos)

        CargarListaDesdeCaja(dg, dtDepos, IIf(fp = 2, 1, 0))
        dg.ReadOnly = False
        For Each col As DataGridViewColumn In dg.Columns
            If col.Index > 0 Then col.ReadOnly = True
        Next

        CalculaTotales()

    End Sub

    Private Sub Verlinea(ByVal Tickets As Boolean, ByVal Ajustes As Boolean, ByVal Comision As Boolean, _
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
    Private Sub btnEmision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmision.Click
        txtEmision.Text = SeleccionaFecha(CDate(txtEmision.Text), Me, sender)
    End Sub

    Private Sub jsBanDepositarCaja_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        InsertarAuditoria(myConn, MovAud.iSalir, sModulo, CodigoBanco)
        ft = Nothing
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

        InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, CDate(txtEmision.Text), txtDeposito.Text, "DP", CodigoBanco, _
                 IIf(tDeposito = 2, "", Mid(lblCaja.Text, 1, 2)), txtConcepto.Text, ValorNumero(txtTotalDeposito.Text), "CAJ", _
                 IIf(tDeposito = 2, txtNumControl.Text, ""), "", "", "0", _
                 jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, fTipoOrigen, "", jytsistema.sFechadeTrabajo, "0", _
                 CodigoProveedor, "")

        If tDeposito <> 2 Then

            For Each selectedItem As DataGridViewRow In dg.Rows

                If CBool(selectedItem.Cells(0).Value) Then
                    ft.Ejecutar_strSQL(myconn, "UPDATE jsbantracaj SET DEPOSITO = '" & txtDeposito.Text & "',  " _
                        & " fecha_dep = '" & ft.FormatoFechaMySQL(CDate(txtEmision.Text)) & "', " _
                        & " codban = '" & CodigoBanco & "' " _
                        & " where " _
                        & " caja = '" & Mid(CodigoCaja, 1, 2) & "' and " _
                        & " tipomov ='" & selectedItem.Cells(2).Value & "' and " _
                        & " fecha = '" & ft.FormatoFechaMySQL(CDate(selectedItem.Cells(1).Value.ToString)) & "' and " _
                        & " nummov = '" & selectedItem.Cells(3).Value & "' and " _
                        & " formpag = '" & selectedItem.Cells(4).Value & "' and " _
                        & " numpag = '" & selectedItem.Cells(5).Value & "' and " _
                        & " refpag = '" & selectedItem.Cells(6).Value & "' and " _
                        & " importe = " & ValorNumero(selectedItem.Cells(7).Value) & "  and " _
                        & " origen = '" & selectedItem.Cells(8).Value & "' and " _
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


            InsertEditCOMPRASEncabezadoGasto(myConn, lblInfo, True, txtNumControl.Text, txtNumControl.Text, "", CDate(txtEmision.Text), _
                CDate(txtEmision.Text), CodigoProveedor, CodigoProveedor, NombreProveedor, RIFProveedor, _
                NITProveedor, "GASTOS CHEQUES ALIMENTACION", "", "", CodigoContable, CodigoGrupo, _
                CodigoSubgrupo, BaseIVA, 0, 0, 0, TipoIVA, PorcentajeIVA(myConn, lblInfo, CDate(txtEmision.Text), TipoIVA), BaseIVA, _
                ValorNumero(txtIVA.Text), TotalGasto, CDate(txtEmision.Text), 1, 0, "EF", txtDeposito.Text, "", "", "", 0.0#, _
                "", 0, 0, 0, 0, "", jytsistema.sFechadeTrabajo, 0, "", "COM", "", "", "0")

            For Each selectedItem As DataGridViewRow In dg.Rows
                If CBool(selectedItem.Cells(0).Value) Then
                    ft.Ejecutar_strSQL(myconn, "UPDATE jsventabtic SET NUMDEP = '" & txtDeposito.Text & "', " _
                                    & " FECHADEP = '" & ft.FormatoFechaMySQL(CDate(txtEmision.Text)) & "', " _
                                    & " BANCODEP = '" & CodigoBanco & "' " _
                                    & " where " _
                                    & " CORREDOR = '" & Mid(lblCaja.Text, 1, 5) & "' and " _
                                    & " NUMSOBRE = '" & selectedItem.Cells(2).Value & "' and " _
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
                            ft.Ejecutar_strSQL(myconn, "update jsbantracaj set " _
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

        If FechaUltimoBloqueo(myConn, "jsbantraban") >= Convert.ToDateTime(txtEmision.Text) Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If ValorEntero(txtDocSel.Text) = 0 Then
            ft.mensajeAdvertencia("Debe seleccionar al menos un documento ... ")
            Return False
        End If

        If txtDeposito.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar un numero de depósito válido ...")
            ft.enfocarTexto(txtDeposito)
            Return False
        Else
            If Not DocumentoValido() Then
                ft.mensajeAdvertencia("Documento ya Existe en Banco")
                ft.enfocarTexto(txtDeposito)
                Return False
            End If
        End If

        If ValorNumero(txtTotalDeposito.Text) < 0 Then
            ft.mensajeAdvertencia("El monto del depósito debe ser mayor o igual a cero")
            Return False
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

        Return True

    End Function

    Private Function ValidaSeleccion() As Boolean

        ValidaSeleccion = False

        If txtRP.Text.Trim = "" AndAlso (cmbSeleccion.SelectedIndex = 6 Or _
                                          cmbSeleccion.SelectedIndex = 3) Then

            ft.MensajeCritico("Debe indicar N° Referencia pago ")
            Exit Function
        End If

        If txtVendedor.Text.Trim = "" AndAlso (cmbSeleccion.SelectedIndex = 5 Or _
                                           cmbSeleccion.SelectedIndex = 6) Then
            ft.MensajeCritico("Debe indicar Vendedor ")
            Exit Function
        End If

        ValidaSeleccion = True

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
        ft.mensajeEtiqueta(lblInfo, "Indique el número deposito ... ", Transportables.TipoMensaje.iInfo)
        txtDeposito.MaxLength = 15
    End Sub

    Private Sub txtConcepto_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConcepto.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el concepto o comentario para este deposito ... ", Transportables.TipoMensaje.iInfo)
        txtDeposito.MaxLength = 250
    End Sub

    Private Sub btnEmision_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEmision.GotFocus
        ft.mensajeEtiqueta(lblInfo, "seleccione la fecha de emisión de este depósito...", Transportables.TipoMensaje.iInfo)
    End Sub
    Private Sub txtNumControl_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNumControl.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el Nº de Control ...", Transportables.TipoMensaje.iInfo)
    End Sub
    Private Sub txtAjustes_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAjustes.TextChanged, _
        txtComision.TextChanged, txtCargos.TextChanged, txtISRL.TextChanged, txtIVA.TextChanged, txtSaldoSel.TextChanged

        txtTotalDeposito.Text = ft.FormatoNumero(ValorNumero(txtSaldoSel.Text) + ValorNumero(txtAjustes.Text) + ValorNumero(txtComision.Text) + _
            ValorNumero(txtCargos.Text) + ValorNumero(txtISRL.Text) + ValorNumero(txtIVA.Text))

    End Sub
    Private Sub cmbSeleccion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSeleccion.SelectedIndexChanged, _
        cmbFP.SelectedIndexChanged

        '0= "Ninguno",                                   {"EF", "CH", "TA", "CT"}
        '1= "Todos", 
        '2= "fecha"
        '3= "FormaPago/Refer.Pago", 
        '4= "Fecha/FormaPago"
        '5= "Vendedor/Fecha/FormaPago"
        '6= "Vendedor/Fecha/FormaPago/Refer.Pago"

        txtRP.Text = ""
        Select Case cmbSeleccion.SelectedIndex.ToString + cmbFP.Text
            Case "2EF", "2CH", "2TA", "2CT"
                ft.visualizarObjetos(True, txtFechaSeleccion, btnFechaSeleccion)
                ft.visualizarObjetos(False, cmbFP, txtRP, btnRP, txtVendedor, btnVendedor)
            Case "3EF", "3CH", "3TA", "3CT"
                ft.visualizarObjetos(False, txtFechaSeleccion, btnFechaSeleccion, txtVendedor, btnVendedor)
                ft.visualizarObjetos(True, cmbFP, txtRP, btnRP)
            Case "4EF", "4CH", "4TA", "4CT"
                ft.visualizarObjetos(True, txtFechaSeleccion, btnFechaSeleccion, cmbFP)
                ft.visualizarObjetos(False, txtRP, btnRP, txtVendedor, btnVendedor)
            Case "5EF", "5CH", "5TA", "5CT"
                ft.visualizarObjetos(True, txtVendedor, btnVendedor, txtFechaSeleccion, btnFechaSeleccion, cmbFP)
                ft.visualizarObjetos(False, txtRP, btnRP)
            Case "6EF", "6CH", "6TA", "6CT"
                ft.visualizarObjetos(True, txtVendedor, btnVendedor, txtFechaSeleccion, btnFechaSeleccion, cmbFP, txtRP, btnRP)
            Case Else
                ft.visualizarObjetos(False, txtVendedor, btnVendedor, txtFechaSeleccion, btnFechaSeleccion, cmbFP, txtRP, btnRP)
        End Select
    End Sub

    Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGo.Click

        Dim nRow As DataGridViewRow
        Dim nFecha As Date = MyDate
        Dim nFormaPago As String = ""
        Dim nReferPago As String = ""
        Dim nVendedor As String = ""
        If ValidaSeleccion() Then
            For Each nRow In dg.Rows

                nFecha = CDate(nRow.Cells(1).Value.ToString)
                nFormaPago = nRow.Cells(4).Value.ToString
                nReferPago = nRow.Cells(6).Value.ToString
                nVendedor = nRow.Cells(10).Value.ToString

                Select Case cmbSeleccion.SelectedIndex
                    Case 0 'Ninguno 
                        nRow.Cells(0).Value = False
                    Case 1 'Todos
                        nRow.Cells(0).Value = True
                    Case 2 ' Por fecha
                        If ft.FormatoFechaMySQL(nFecha) = ft.FormatoFechaMySQL(CDate(txtFechaSeleccion.Text)) Then _
                            nRow.Cells(0).Value = True
                    Case 3 'FormaPago/Refer.Pago 
                        If nFormaPago = cmbFP.Text AndAlso _
                            nReferPago = txtRP.Text Then nRow.Cells(0).Value = True
                    Case 4 'Fecha/FormaPago
                        If nFormaPago = cmbFP.Text AndAlso _
                                 ft.FormatoFechaMySQL(nFecha) = ft.FormatoFechaMySQL(CDate(txtFechaSeleccion.Text)) Then

                            nRow.Cells(0).Value = True

                        End If
                    Case 5 '"Vendedor/Fecha/FormaPago"
                        If nVendedor = txtVendedor.Text AndAlso _
                            nFormaPago = cmbFP.Text AndAlso _
                                 ft.FormatoFechaMySQL(nFecha) = ft.FormatoFechaMySQL(CDate(txtFechaSeleccion.Text)) Then

                            nRow.Cells(0).Value = True

                        End If
                    Case 6 'Vendedor/Fecha/FormaPago/ReferPago
                        If nVendedor = txtVendedor.Text AndAlso _
                            nFormaPago = cmbFP.Text AndAlso _
                                nReferPago = txtRP.Text AndAlso _
                                 ft.FormatoFechaMySQL(nFecha) = ft.FormatoFechaMySQL(CDate(txtFechaSeleccion.Text)) Then
                            nRow.Cells(0).Value = True

                        End If
                End Select
            Next
            CalculaTotales()
        End If

    End Sub
    Private Sub btnFechaSeleccion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaSeleccion.Click
        txtFechaSeleccion.Text = ft.FormatoFecha(SeleccionaFecha(CDate(txtFechaSeleccion.Text), Me, btnFechaSeleccion))
    End Sub

    Private Sub btnBP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRP.Click
        Select Case cmbFP.Text
            Case "CH"
                txtRP.Text = CargarTablaSimplePlusReal("Bancos", Modulo.iBancos)
            Case "TA"
                Dim f As New jsControlArcTarjetas
                f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
                txtRP.Text = f.Seleccionado
                f = Nothing
            Case "CT"
                txtRP.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsvencestic where id_emp = '" & jytsistema.WorkID & "' order by codigo", "Corredores de Cesta Ticket", txtRP.Text)
        End Select

    End Sub

    Private Sub txtBuscar_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged

        txtBuscar.Text = Replace(txtBuscar.Text, "'", "")
        'BindingSource1.DataSource = dtDepos
        'If dtDepos.Columns(FindField).DataType Is GetType(String) Then
        '    nPosicion = BindingSource1.Find(FindField, txtBuscar.Text)
        '    BindingSource1.Filter = FindField & " like '%" & txtBuscar.Text & "%'"
        'End If

        'dg.DataSource = BindingSource1
        If dtDepos.Columns(FindField).DataType Is GetType(String) Then
            Dim foundRow As DataRow
            If dtDepos.Select(FindField & " like '%" & txtBuscar.Text & "%'").Count > 0 Then
                foundRow = dtDepos.Select(FindField & " like '%" & txtBuscar.Text & "%'")(0)
                nPosicion = dtDepos.Rows.IndexOf(foundRow)
                dg.Rows(nPosicion).Cells(FindField).Selected = True
            Else
                MsgBox("Esta secuencia no se encuentra en la tabla...", MsgBoxStyle.Information)
            End If

        End If




    End Sub

    Private Sub jsBanArcDepositarCaja_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        txtBuscar.Focus()
    End Sub



    Private Sub CalculaTotales()
        Dim aTarjetaBan() As String = {"codban", "codtar", "id_emp"}
        Dim aTarjeta() As String = {"codtar", "id_emp"}
        Dim TipoIVA As String = qFoundAndSign(myConn, lblInfo, "jsvencestic", aCorredor, sCorredor, "tipoiva")
        Dim Porcentajecomision As Double = 0.0
        Dim PorcentajeISLR As Double = 0.0
        Dim aString(2) As String
        Dim aStringBan(3) As String
        iSel = 0
        dSel = 0.0
        tSel = 0
        dComision = 0.0
        dCar = 0.0
        dIVA = 0.0
        If tDeposito = 2 Then

            For Each selectedItem As DataGridViewRow In dg.Rows
                If selectedItem.Cells(0).Value Then
                    iSel += 1
                    dSel += CDbl(selectedItem.Cells(5).Value)
                    tSel += CInt(selectedItem.Cells(4).Value)
                    dComision -= ComisionCorredorSobre(myConn, ds, selectedItem.Cells(3).Value, selectedItem.Cells(2).Value)
                    dCar -= CDbl(qFoundAndSign(myConn, lblInfo, "jsvencestic", aCorredor, sCorredor, "cargos"))
                    dIVA = -1 * Math.Round((Math.Abs(dComision) + Math.Abs(dCar)) * PorcentajeIVA(myConn, lblInfo, CDate(txtEmision.Text), TipoIVA) / 100, 2)
                End If
            Next

        Else
            For Each selectedItem As DataGridViewRow In dg.Rows
                If selectedItem.Cells(0).Value Then
                    iSel += 1
                    dSel += CDbl(selectedItem.Cells(7).Value)
                    Select Case selectedItem.Cells(4).Value
                        Case "TA"
                            aStringBan(0) = CodigoBanco : aStringBan(1) = IIf(IsDBNull(selectedItem.Cells(6).Value), "", selectedItem.Cells(6).Value) : aStringBan(2) = jytsistema.WorkID
                            If qFound(myConn, lblInfo, "jsbancatbantar", aTarjetaBan, aStringBan) Then
                                dComision -= CDbl(selectedItem.Cells(7).Value) * CDbl(qFoundAndSign(myConn, lblInfo, "jsbancatbantar", aTarjetaBan, aStringBan, "com1")) / 100
                                dISLR -= CDbl(selectedItem.Cells(7).Value) * CDbl(qFoundAndSign(myConn, lblInfo, "jsbancatbantar", aTarjetaBan, aStringBan, "com2")) / 100
                            Else
                                If IsDBNull(selectedItem.Cells(6).Value) Then
                                    dComision -= 0.0
                                    dISLR -= 0.0
                                Else
                                    aString(0) = IIf(IsDBNull(selectedItem.Cells(6).Value), "", selectedItem.Cells(6).Value) : aString(1) = jytsistema.WorkID
                                    dComision -= CDbl(selectedItem.Cells(7).Value) * CDbl(qFoundAndSign(myConn, lblInfo, "jsconctatar", aTarjeta, aString, "com1")) / 100
                                    dISLR -= CDbl(selectedItem.Cells(7).Value) * CDbl(qFoundAndSign(myConn, lblInfo, "jsconctatar", aTarjeta, aString, "com2")) / 100
                                End If

                            End If
                    End Select
                End If
            Next
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

    Private Sub dg_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick
        If e.ColumnIndex = 0 Then
            dtDepos.Rows(e.RowIndex).Item(0) = Not CBool(dtDepos.Rows(e.RowIndex).Item(0).ToString)
        End If

    End Sub

    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
       dg.ColumnHeaderMouseClick


        If dtDepos.Columns(e.ColumnIndex).DataType Is GetType(String) Then
            FindField = dtDepos.Columns(dg.Columns(e.ColumnIndex).Name).ColumnName
            lblBuscar.Text = "Buscando por : " & aNombres(e.ColumnIndex)
        Else
            MsgBox("NO PUEDE BUSCAR POR ESTE TIPO DE COLUMNA", MsgBoxStyle.Information)
        End If

        txtBuscar.Focus()
        ft.enfocarTexto(txtBuscar)

    End Sub

    Private Sub dg_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellClick

    End Sub

    Private Sub dg_CellMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.CellMouseUp
        'BindingSource1.Filter = FindField & " like '%%'"
        'dg.DataSource = BindingSource1
        CalculaTotales()
    End Sub

    Private Sub dg_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellValidated
        If dg.CurrentCell.ColumnIndex = 0 Then

            If tDeposito <> 2 Then
                ft.Ejecutar_strSQL(myconn, " update  " & tblCaja & " set sel  = " & CInt(dg.CurrentCell.Value) & " " _
                                & " where " _
                                & " fecha = '" & ft.FormatoFechaMySQL(dg.CurrentRow.Cells(1).Value.ToString) & "' and " _
                                & " tipomov = '" & CStr(dg.CurrentRow.Cells(2).Value) & "' and " _
                                & " nummov = '" & CStr(dg.CurrentRow.Cells(3).Value) & "' and " _
                                & " formpag = '" & CStr(dg.CurrentRow.Cells(4).Value) & "' and " _
                                & " numpag = '" & CStr(dg.CurrentRow.Cells(5).Value) & "' and " _
                                & " refpag = '" & CStr(dg.CurrentRow.Cells(6).Value) & "' and " _
                                & " importe = " & CStr(dg.CurrentRow.Cells(7).Value) & " and " _
                                & " origen = '" & CStr(dg.CurrentRow.Cells(8).Value) & "' and " _
                                & " id_emp = '" & jytsistema.WorkID & "' ")
            Else

                ft.Ejecutar_strSQL(myconn, " update  " & tblCaja & " set sel  = " & CInt(dg.CurrentCell.Value) & " " _
                                & " where " _
                                & " fechasobre = '" & ft.FormatoFechaMySQL(dg.CurrentRow.Cells(1).Value.ToString) & "' and " _
                                & " numsobre = '" & CStr(dg.CurrentRow.Cells(2).Value) & "' and " _
                                & " corredor = '" & CStr(dg.CurrentRow.Cells(3).Value) & "' and " _
                                & " id_emp = '" & jytsistema.WorkID & "' ")


            End If


        End If
    End Sub

    Private Sub btnVendedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVendedor.Click
        txtVendedor.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, concat(apellidos, ' ', nombres ) descripcion from jsvencatven order by codven ", "Vendedores", txtVendedor.Text)
    End Sub
End Class