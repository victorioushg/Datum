Imports MySql.Data.MySqlClient
Public Class jsVenProBloqueoDeClientes
    Private Const sModulo As String = "Cambio, verificación y bloqueo de estatus de clientes"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dtClientes As New DataTable
    Private ft As New Transportables

    Private strSQLClientes As String = ""

    Private nTablaClientes As String = "tblClientes"


    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        MyConn = MyCon
        IniciarTXT()
        Me.Show()

    End Sub
    Private Sub IniciarTXT()

        ft.habilitarObjetos(False, True, txtCausa)
        ft.habilitarObjetos(True, True, txtDiasVencimiento, btnCausa)

        txtCausa.Text = "00001"
        txtDiasVencimiento.Text = ft.FormatoEntero(8)

        chkFacturar.Checked = True


    End Sub

    Private Sub jsVenProBloqueoDeClientes_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsVenProReconstruccionDeSaldos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If lblCausa.Text = "" Then
            ft.MensajeCritico("Debe escoger una causa válida para este proceso ... ")
            Exit Function
        End If

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then

            BloqueoDeClientes()

            MsgBox("Proceso terminado...", MsgBoxStyle.Information, sModulo)
            InsertarAuditoria(MyConn, MovAud.iProcesar, sModulo, "")
            ProgressBar1.Value = 0
            lblProgreso.Text = ""

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtDiasVencimiento.Text)
            Me.Close()

        End If

    End Sub
    Private Sub BloqueoDeClientes()

        ds = DataSetRequery(ds, " select * from jsvencatcli where id_emp = '" & jytsistema.WorkID & "' order by codcli ", MyConn, nTablaClientes, lblInfo)
        dtClientes = ds.Tables(nTablaClientes)

        Dim iCont As Integer
        If dtClientes.Rows.Count > 0 Then
            For iCont = 0 To dtClientes.Rows.Count - 1
                With dtClientes.Rows(iCont)

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(iCont / dtClientes.Rows.Count * 100), _
                                                  " Progreso : " & .Item("CODCLI") & " " & .Item("NOMBRE"))
                    VerificaVencimientoCliente(MyConn, lblInfo, ds, .Item("CODCLI"), .Item("ESTATUS"), _
                                                ValorEntero(txtDiasVencimiento.Text), txtCausa.Text, chkFacturar.Checked)

                End With
            Next
            lblProgreso.Text = ""
            InsertarAuditoria(MyConn, MovAud.iProcesar, sModulo, "Días : " & txtDiasVencimiento.Text & " - Causa : " & txtCausa.Text & " - CHD : " & chkFacturar.Checked.ToString)
        End If
    End Sub

    'Private Sub VerificaVencimiento(CodigoCliente As String, Estatus As String)

    '    Dim dts As DataTable
    '    Dim nTable As String = "tblFActurasPendientes"
    '    ds = DataSetRequery(ds, "SELECT a.CODCLI, b.NOMBRE, a.NUMMOV, a.TIPOMOV, a.REFER, " _
    '        & " a.EMISION, a.VENCE, a.IMPORTE, SUM(a.importe) AS SALDO, " _
    '        & " to_days('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "') - to_Days(a.vence) as DV, " _
    '        & " if( to_days('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "') - to_Days(a.vence)>= " & ValorEntero(txtDiasVencimiento.Text) & ", 1, 0) as lapso " _
    '        & " from jsventracob a LEFT JOIN jsvencatcli b ON  b.CODCLI = a.CODCLI AND b.ID_EMP = a.ID_EMP and " _
    '        & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' Where a.ID_EMP = '" & jytsistema.WorkID & "' " _
    '        & " and a.tipomov <> 'ND' " _
    '        & " and a.codcli = '" & CodigoCliente & "' " _
    '        & " GROUP BY a.codcli, a.nummov Having Saldo > 1 AND a.vence < '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and lapso = 1 " _
    '        & " ORDER BY LAPSO, a.CODCLI, NUMMOV, EMISION ASC ", MyConn, nTable, lblInfo)


    '    dts = ds.Tables(nTable)

    '    If dts.Rows.Count > 0 Then
    '        If Estatus = "0" Then
    '            InsertEditVENTASExpedienteCliente(MyConn, lblInfo, True, CodigoCliente, CDate(ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & " " & ft.FormatoHora(Now())), _
    '                                              "FACTURAS PENDIENTES", 1, txtCausa.Text, 0)

    '            ft.Ejecutar_strSQL ( myconn, " update jsvencatcli set estatus = '1' where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
    '        End If

    '    End If

    '    If chkFacturar.Checked Then
    '        ft.Ejecutar_strSQL ( myconn, " update jsvencatcli set backorder = '0' where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
    '    End If

    '    dts.Dispose()
    '    dts = Nothing

    'End Sub

    Private Sub btnClienteHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCausa.Click
        txtCausa.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '00018' and id_emp = '" & jytsistema.WorkID & "' order by codigo  ", "Causa para bloqueo de clientes", txtCausa.Text)
    End Sub
    Private Sub txtDiasVencimiento_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDiasVencimiento.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub txtCausa_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCausa.TextChanged
        lblCausa.Text = ft.DevuelveScalarCadena(MyConn, " select descrip from jsconctatab where codigo = '" & txtCausa.Text & "' and modulo = '00018' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub
End Class