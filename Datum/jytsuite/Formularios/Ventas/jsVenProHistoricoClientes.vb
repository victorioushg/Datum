Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsVenProHistoricoClientes
    Private Const sModulo As String = "Subir/Bajar movimientos a/de histórico"

    Private Const nTabla As String = "tblClientes"

    Private strSQL As String = ""

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private ProcesoTipo As iProceso


    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal TipoProceso As Integer, Optional ByVal CodCliente As String = "")

        myConn = MyCon
        Me.Tag = sModulo
        ProcesoTipo = TipoProceso

        Dim dates As SfDateTimeEdit() = {txtFechaProceso}
        SetSizeDateObjects(dates)

        If TipoProceso = iProceso.Procesar Then
            lblLeyenda.Text = " Mediante este proceso se pasan los movimientos YA cancelados a histórico, con lo cual " + vbCr +
                    " desaparecen del movimiento actual. " + vbCr +
                    " "
            ft.visualizarObjetos(False, lblFecha, txtFechaProceso)

        Else
            lblLeyenda.Text = " Mediante este proceso se reversan los movimientos YA cancelados desde histórico. " + vbCr +
                    " Si prefiere puede indicar la fecha desde la cual desea que aparezcan los movimientos. " + vbCr +
                    " "
            ft.visualizarObjetos(True, lblFecha, txtFechaProceso)
        End If

        If CodCliente <> "" Then ft.habilitarObjetos(False, True, txtClienteDesde, txtClienteHasta, btnClienteDesde, btnClienteHasta)

        txtClienteDesde.Text = CodCliente
        txtFechaProceso.Value = DateAdd(DateInterval.Month, -6, jytsistema.sFechadeTrabajo)

        ft.mensajeEtiqueta(lblInfo, " ... ", Transportables.tipoMensaje.iAyuda)

        Me.ShowDialog()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub jsVenProHistoricoClientes_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsVenProHistoricoClientes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Procesar()
    End Sub

    Private Sub Procesar()

        DeshabilitarCursorEnEspera()
        pb.Value = 0
        lblProgreso.Text = ""

        ds = DataSetRequery(ds, " select codcli, nombre from jsvencatcli where " _
                            & IIf(txtClienteDesde.Text <> "", " codcli >= '" & txtClienteDesde.Text & "' and ", "") _
                            & IIf(txtClienteHasta.Text <> "", " codcli <= '" & txtClienteHasta.Text & "' and ", "") _
                            & " id_emp = '" & jytsistema.WorkID & "' order by codcli ", myConn, nTabla, lblInfo)

        dt = ds.Tables(nTabla)
        If dt.Rows.Count > 0 Then
            Dim jCont As Integer
            For jCont = 0 To dt.Rows.Count - 1
                With dt.Rows(jCont)
                    lblProgreso.Text = "Cliente : " & .Item("codcli") & " " & .Item("nombre")
                    pb.Value = (jCont + 1) / dt.Rows.Count * 100
                    If ProcesoTipo = iProceso.Procesar Then
                        PasarAHistoricoCXC(.Item("codcli"))
                    Else
                        ReversarDeHistoricoCXC(.Item("codcli"), CDate(txtFechaProceso.Text))
                    End If
                End With

            Next
        End If



        HabilitarCursorEnEspera()
        ft.mensajeInformativo(" Proceso culminado con éxito... ")

        Me.Close()

    End Sub

    Private Sub PasarAHistoricoCXC(ByVal CodigoCliente As String)

        Dim dtProcesar As DataTable
        Dim nTablaProcesar As String = "tblProcesar"

        ds = DataSetRequery(ds, " select a.NUMMOV, IFNULL(SUM(a.IMPORTE),0) SALDO from jsventracob a " _
                                & " where " _
                                & " a.CODCLI = '" & CodigoCliente & "' and " _
                                & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                & " GROUP BY NUMMOV ", myConn, nTablaProcesar, lblInfo)

        dtProcesar = ds.Tables(nTablaProcesar)

        If dtProcesar.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtProcesar.Rows.Count - 1
                With dtProcesar.Rows(iCont)
                    If CDbl(.Item("saldo")) = 0.0# Then
                        ft.Ejecutar_strSQL(myConn, "UPDATE jsventracob SET HISTORICO = '1' " _
                          & " where CODCLI = '" & CodigoCliente & "' " _
                          & " and NUMMOV = '" & .Item("nummov") & "' " _
                          & " and EJERCICIO = '" & jytsistema.WorkExercise & "' " _
                          & " and ID_EMP = '" & jytsistema.WorkID & "'")
                    End If
                End With
            Next
        End If

        InsertarAuditoria(myConn, MovAud.iProcesar, sModulo, CodigoCliente)

        dtProcesar.Dispose()
        dtProcesar = Nothing

    End Sub

    Private Sub ReversarDeHistoricoCXC(ByVal CodigoCliente As String, ByVal FechaDesde As Date)

        ft.Ejecutar_strSQL(myConn, "UPDATE jsventracob SET HISTORICO = '0' " _
        & " where CODCLI = '" & CodigoCliente & "' " _
        & " and emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' " _
        & " and HISTORICO = '1' " _
        & " and ID_EMP = '" & jytsistema.WorkID & "'")


        ft.Ejecutar_strSQL(myConn, " update jsventracob a, (select nummov from jsventracob " _
                                        & "                      where historico = '0' and " _
                                        & "                      codcli = '" & CodigoCliente & "' and " _
                                        & "                      id_emp = '" & jytsistema.WorkID & "' group by nummov) b " _
                                        & " SET a.historico = '0' " _
                                        & " where a.nummov = b.nummov and " _
                                        & " a.codcli = '" & CodigoCliente & "' and " _
                                        & " a.id_emp = '" & jytsistema.WorkID & "'")

        InsertarAuditoria(myConn, MovAud.iProcesar, sModulo, CodigoCliente & "REV")


    End Sub

    Private Sub txtClienteDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtClienteDesde.TextChanged
        txtClienteHasta.Text = txtClienteDesde.Text
        lblClienteDesde.Text = ft.DevuelveScalarCadena(myConn, " select nombre from jsvencatcli where codcli = '" & txtClienteDesde.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub


    Private Sub btnClienteDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClienteDesde.Click
        txtClienteDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcli codigo, nombre descripcion from jsvencatcli where id_emp = '" & jytsistema.WorkID & "' order by codcli ", "Clientes", txtClienteDesde.Text)
    End Sub

    Private Sub btnClienteHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClienteHasta.Click
        txtClienteHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcli codigo, nombre descripcion from jsvencatcli where id_emp = '" & jytsistema.WorkID & "' order by codcli ", "Clientes", txtClienteHasta.Text)
    End Sub

    Private Sub txtClienteHasta_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtClienteHasta.TextChanged
        lblClienteHasta.Text = ft.DevuelveScalarCadena(myConn, " select nombre from jsvencatcli where codcli = '" & txtClienteHasta.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub
End Class