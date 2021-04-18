Imports MySql.Data.MySqlClient
Public Class jsVenArcCXCCestaTicket
    Private Const sModulo As String = "Movimiento de CxC Adelanto/Cancelación con cheques de alimentación"
    Private Const nTablaTickets As String = "tblTickets"
    Private Const nTablaCorredores As String = "tblCorredores"
    Private Const nTablaResumen As String = "tblResumen"

    Private MyConn As New MySqlConnection
    Private dsLocal As New DataSet
    Private dtCorredores As DataTable
    Private dtTickets As DataTable
    Private dtResumen As DataTable


    Private numCancelacion As String
    Private numPago As String
    Private monto_a_Cancelar As Double
    Private cajaDePago As String
    Private diasComision As Integer

    Private CodigoBarraTicket As String = ""
    Private strTickets As String = ""
    Private strSQLResumen As String = ""
    Private nPosicionR As Long = 0
    Private nPosicionT As Long = 0
    Private CodigoCorredor As String = ""

    Private lenCodigoBarraCorredor As Integer = 0
    Private iniPrecioEnBarraCorredor As Integer = 0
    Private lenPrecioEnBarraCorredor As Integer = 0
    Private iniTipoCorredor As Integer = 0
    Private lenTipoCorredor As Integer = 0

    Private n_montoPago As Double
    Private n_montoComision As Double

    Public Property montoPago() As Double
        Get
            Return n_montoPago
        End Get
        Set(ByVal value As Double)
            n_montoPago = value
        End Set
    End Property

    Public Property montoComision() As Double
        Get
            Return n_montoComision
        End Get
        Set(ByVal value As Double)
            n_montoComision = value
        End Set
    End Property

    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal NumeroCancelacion As String, _
        ByVal NumeroDePago As String, ByVal MontoACancelar As Double, ByVal CajaPago As String, ByVal DiasParaComision As Integer)

        MyConn = MyCon
        numCancelacion = NumeroCancelacion
        numPago = NumeroDePago
        monto_a_Cancelar = MontoACancelar
        cajaDePago = CajaPago
        diasComision = DiasParaComision



        dsLocal = DataSetRequery(dsLocal, "select * from jsvencestic where ID_EMP = '" & jytsistema.WorkID & "' ", MyConn, nTablaCorredores, lblInfo)
        dtCorredores = dsLocal.Tables(nTablaCorredores)

        RellenaComboConDatatable(cmbCorredor, dtCorredores, "descrip", "codigo")

        txt0.Text = FormatoNumero(0.0)
        txt1.Text = FormatoNumero(0.0)
        txt2.Text = FormatoNumero(0.0)
        txt3.Text = FormatoNumero(MontoACancelar)

        If dtCorredores.Rows.Count <= 0 Then HabilitarObjetos(False, True, txtBarraticket)

        IniciaTickets()
        IniciaResumen()
        CalculaTotales()

        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, numCancelacion)

        Me.ShowDialog()

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        montoPago = monto_a_Cancelar
        If ValorNumero(txt2.Text) = 0.0# Then montoComision = 0.0
        EjecutarSTRSQL(MyConn, lblInfo, "DELETE FROM jsventabtic where NUMCAN = '" & numCancelacion & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
        Me.Close()

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If Validado Then
            montoPago = ValorNumero(txt2.Text)
            If ValorNumero(txt2.Text) > 0 Then
                montoComision = ValorNumero(txt0.Text)
            Else
                montoComision = 0.0
                'txtNumeroPago = ""
            End If
            InsertarAuditoria(MyConn, MovAud.iIncluir, sModulo, aGestion(Gestion.iVentas))
        Else
            Exit Sub
        End If

        Me.Close()

    End Sub

    Private Function Validado() As Boolean

        'If ValorNumero(txt2.Text) > ValorNumero(txt3.Text) Then
        '    MensajeCritico(lblInfo, "LA SUMA DE LOS TICKETES DEBE SER MENOR O IGUAL AL MONTO DE FACTURAS A CANCELAR...")
        '    Exit Function
        'End If
        Return True

    End Function

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Dim Respuesta As Integer
        If dtTickets.Rows.Count > 0 Then
            Respuesta = MsgBox(" ¿ Esta Seguro que desea eliminar el ticket n° >>>>  " & dtTickets.Rows(Me.BindingContext(dsLocal, nTablaTickets).Position).Item("ticket") & " <<<< ?", vbYesNo, "Eliminar registro ... ")
            If Respuesta = vbYes Then
                EjecutarSTRSQL(MyConn, lblInfo, " delete from jsventabtic where TICKET = '" & dtTickets.Rows(Me.BindingContext(dsLocal, nTablaTickets).Position).Item("ticket") & "'")
                CalculaTotales()
            End If
        End If
        txtBarraticket.Enabled = True
        EnfocarTexto(txtBarraticket)
    End Sub

    Private Sub CalculaTotales()

        txt0.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select sum(monto) from jsventabtic WHERE " _
                                                                        & " ID_EMP = '" & jytsistema.WorkID & "' AND" _
                                                                        & " NUMCAN = '" & numCancelacion & "' GROUP BY NUMCAN")))

        txt1.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select sum(comision) from jsventabtic WHERE " _
                                                                        & " ID_EMP = '" & jytsistema.WorkID & "' AND" _
                                                                        & " NUMCAN = '" & numCancelacion & "' GROUP BY NUMCAN")))

        txt2.Text = FormatoNumero(ValorNumero(txt0.Text) - ValorNumero(txt1.Text))

        dsLocal = DataSetRequery(dsLocal, strTickets, MyConn, nTablaTickets, lblInfo)
        dtTickets = dsLocal.Tables(nTablaTickets)
        dg.Refresh()

        dsLocal = DataSetRequery(dsLocal, strSQLResumen, MyConn, nTablaResumen, lblInfo)
        dtResumen = dsLocal.Tables(nTablaResumen)
        dgResumen.Refresh()


    End Sub

    Private Sub jsVenArcCXCCestaTicket_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        dtCorredores.Dispose()
        dtTickets.Dispose()
        dtResumen.Dispose()

        dtCorredores = Nothing
        dtTickets = Nothing
        dtResumen = Nothing

        dsLocal.Dispose()
        dsLocal = Nothing

    End Sub

    Private Sub jsVenArcCXCCestaTicket_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
    End Sub

    Private Sub IniciaResumen()

        strSQLResumen = "select CORREDOR, count(*) AS CANTIDAD, SUM(MONTO) AS IMPORTE from jsventabtic WHERE " _
                                    & " ID_EMP = '" & jytsistema.WorkID & "' AND " _
                                    & " NUMCAN = '" & numCancelacion & "' GROUP BY CORREDOR ORDER BY CORREDOR  "


        dsLocal = DataSetRequery(dsLocal, strSQLResumen, MyConn, nTablaResumen, lblInfo)
        dtResumen = dsLocal.Tables(nTablaResumen)

        Dim aCamR() As String = {"corredor", "cantidad", "importe"}
        Dim aNomR() As String = {"Corredor", "Cantidad", "Total"}
        Dim aAncR() As Long = {70, 100, 100}
        Dim aAliR() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}
        Dim aForR() As String = {"", sFormatoEntero, sFormatoNumero}


        IniciarTabla(dgResumen, dtResumen, aCamR, aNomR, aAncR, aAliR, aForR, True, False, 8, True)
        If dtResumen.Rows.Count > 0 Then nPosicionR = 0

    End Sub
    Private Sub IniciaTickets()

        strTickets = "select * from jsventabtic where NUMCAN = '" & numCancelacion & "' "
        dsLocal = DataSetRequery(dsLocal, strTickets, MyConn, nTablaTickets, lblInfo)

        dtTickets = dsLocal.Tables(nTablaTickets)

        Dim aCam() As String = {"ticket", "corredor", "monto", "comision"}
        Dim aNom() As String = {"N° de Ticket", "Corredor", "Importe Ticket", "Comisión"}
        Dim aAnc() As Long = {350, 70, 100, 100}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}
        Dim aFor() As String = {"", "", sFormatoNumero, sFormatoNumero}

        IniciarTabla(dg, dtTickets, aCam, aNom, aAnc, aAli, aFor, True, False, 8, True)
        If dtTickets.Rows.Count > 0 Then nPosicionT = 0

    End Sub

    Private Sub cmbCorredor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCorredor.SelectedIndexChanged
        txtBarraticket.Text = ""
        EnfocarTexto(txtBarraticket)

    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(dsLocal, nTablaTickets).Position = e.RowIndex
        nPosicionT = e.RowIndex
    End Sub

    Private Sub txtBarraticket_GotFocus(sender As Object, e As System.EventArgs) Handles txtBarraticket.GotFocus
        EnfocarTexto(sender)
    End Sub
    'Private Sub txtBarraticket_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBarraticket.KeyDown
    '    If e.KeyCode = Keys.Enter Then
    '        txtBarraticket.Enabled = True
    '        txtBarraticket.Text = ""
    '        EnfocarTexto(txtBarraticket)
    '    End If
    'End Sub
    Private Sub txtBarraticket_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBarraticket.KeyDown

        If e.KeyCode = Keys.Enter Then

            CodigoCorredor = cmbCorredor.SelectedValue
            lenCodigoBarraCorredor = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lencodbar from jsvencestic where codigo = '" & CodigoCorredor & "' and id_emp = '" & jytsistema.WorkID & "' ")
            iniPrecioEnBarraCorredor = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select inicioprecio from jsvencestic where codigo = '" & CodigoCorredor & "' and id_emp = '" & jytsistema.WorkID & "' ")
            lenPrecioEnBarraCorredor = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lenprecio from jsvencestic where codigo = '" & CodigoCorredor & "' and id_emp = '" & jytsistema.WorkID & "' ")
            iniTipoCorredor = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select iniciotipo from jsvencestic where codigo = '" & CodigoCorredor & "' and id_emp = '" & jytsistema.WorkID & "' ")
            lenTipoCorredor = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select lentipo from jsvencestic where codigo = '" & CodigoCorredor & "' and id_emp = '" & jytsistema.WorkID & "' ")

            Dim lenBarra As Integer = txtBarraticket.Text.Length
            txtBarraticket.Enabled = False

            If lenBarra <> lenCodigoBarraCorredor Then
                MensajeCritico(lblInfo, " La longitud del Código de barra de corredor escaneado ES DIFERENTE al INDICADO PARA ESTE CORREDOR ...")
                txtBarraticket.Enabled = True
                EnfocarTexto(txtBarraticket)
                Exit Sub
            End If

            Dim TipoTicket As String
            If iniTipoCorredor > 0 Then
                TipoTicket = Mid(txtBarraticket.Text, iniTipoCorredor, lenTipoCorredor)
            Else
                TipoTicket = "00"
            End If

            Dim perComisionCliente As Double = PorcentajeComisionCliente(MyConn, CodigoCorredor, TipoTicket)
            Dim perComisionProveedor As Double = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select com_corredor from jsvencestip where corredor = '" & CodigoCorredor & "' and tipo = '" & TipoTicket & "' and id_emp = '" & jytsistema.WorkID & "'  ")

            If CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "SELECT COUNT(*) FROM jsventabtic WHERE TICKET = '" & txtBarraticket.Text & "'AND CORREDOR = '" & CodigoCorredor & "' ")) = 0 Then
                Dim InsertaTicket As Boolean = False
                If CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsvenvaltic where " _
                                                                & " CODIGO = '" & CodigoCorredor & "' AND " _
                                                                & " ENBARRA = '" & txtBarraticket.Text.Substring(iniPrecioEnBarraCorredor - 1, lenPrecioEnBarraCorredor) & "' ")) = 0 Then
                    'Valor del ticket NO EXISTE en BD

                    Dim g As New jsVenArcCXCCestaTicketValor
                    g.Agregar(MyConn, CodigoCorredor, txtBarraticket.Text.Substring(iniPrecioEnBarraCorredor - 1, lenPrecioEnBarraCorredor), _
                            ValorNumero(txtBarraticket.Text.Substring(iniPrecioEnBarraCorredor - 1, lenPrecioEnBarraCorredor)))
                    g.Dispose()
                    g = Nothing

                    MensajeCritico(lblInfo, ":-)) DEBE LEER ESTE TICKET DE NUEVO ooooooooo")

                Else 'Valor Existe en BD
                    InsertaTicket = True
                End If
                If InsertaTicket Then
                    Dim ValorTicket As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select valor from jsvenvaltic where " _
                                                                           & " codigo = '" & CodigoCorredor & "' and " _
                                                                           & " enbarra = '" & txtBarraticket.Text.Substring(iniPrecioEnBarraCorredor - 1, lenPrecioEnBarraCorredor) & "' "))

                    Dim nTicket As String = txtBarraticket.Text

                    EjecutarSTRSQL(MyConn, lblInfo, " lock tables jsventabtic write ")
                    EjecutarSTRSQL(MyConn, lblInfo, "INSERT INTO jsventabtic " _
                        & "(TICKET, CORREDOR, MONTO, PORCOM, COMISION, NUMCAN,  " _
                        & " CAJA, NUMSOBRE, FECHASOBRE, NUMDEP, FECHADEP, BANCODEP, " _
                        & " CONDICION, FALSO, ID_EMP) VALUES (" _
                        & "'" & txtBarraticket.Text & "', " _
                        & "'" & CodigoCorredor & "', " _
                        & "" & ValorTicket & ", " _
                        & "" & perComisionCliente & ", " _
                        & "" & perComisionCliente * ValorTicket / 100 & ", " _
                        & "'" & numCancelacion & "', " _
                        & "'" & cajaDePago & "', " _
                        & " '','1963-05-07', '', '1963-05-07', '', 0,0, " _
                        & "'" & jytsistema.WorkID & "')")

                    EjecutarSTRSQL(MyConn, lblInfo, " unlock tables ")

                    CalculaTotales()

                    Dim row As DataRow = dtTickets.Select(" TICKET = '" & nTicket & "' ")(0)
                    nPosicionT = dtTickets.Rows.IndexOf(row)

                    If nPosicionT >= 0 Then
                        dg.Rows(nPosicionT).Selected = True
                        dg.FirstDisplayedScrollingRowIndex = dg.SelectedRows(0).Index
                    End If




                End If
            Else
                MensajeCritico(lblInfo, "CODIGO DE BARRA YA EXISTE. VERIFIQUE POR FAVOR...")
            End If
            txtBarraticket.Text = ""
            txtBarraticket.Enabled = True
            EnfocarTexto(txtBarraticket)

        End If


    End Sub

    
    Private Function PorcentajeComisionCliente(ByVal MyConn As MySqlConnection, ByVal Corredor As String, ByVal Tipo As String) As Double

        Dim nTablaTipos As String = "tblTipoComisiones"
        Dim nTablaComisiones As String = "tblComisiones"
        Dim dtTipos As DataTable
        Dim dtComisiones As DataTable
        Dim jCont As Integer

        PorcentajeComisionCliente = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select porcomcli from jsvencestic where codigo = '" & Corredor & "' and id_emp = '" & jytsistema.WorkID & "' "))

        dsLocal = DataSetRequery(dsLocal, " select * from jsvencestip where corredor = '" & Corredor & "' and tipo = '" & Tipo & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaTipos, lblInfo)
        dtTipos = dsLocal.Tables(nTablaTipos)

        If dtTipos.Rows.Count > 0 Then
            PorcentajeComisionCliente = dtTipos.Rows(0).Item("com_cliente")

            dsLocal = DataSetRequery(dsLocal, "select * from jsvencescom where corredor = '" & Corredor & "' and tipo = '" & Tipo & "' and id_emp = '" & jytsistema.WorkID & "'", MyConn, nTablaComisiones, lblInfo)
            dtComisiones = dsLocal.Tables(nTablaComisiones)
            If dtComisiones.Rows.Count > 0 Then
                With dtComisiones
                    For jCont = 0 To .Rows.Count - 1
                        If CInt(.Rows(jCont).Item("desde")) <= diasComision And _
                            CInt(.Rows(jCont).Item("hasta")) >= diasComision Then _
                            PorcentajeComisionCliente = .Rows(jCont).Item("comision")
                    Next
                End With
            End If
            dtComisiones.Dispose()
            dtComisiones = Nothing

        End If

        dtTipos.Dispose()
        dtTipos = Nothing

    End Function
   
    
    Private Sub txtBarraticket_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtBarraticket.TextChanged

    End Sub
End Class