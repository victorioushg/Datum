Imports MySql.Data.MySqlClient
Public Class jsBanProConciliacionPlus
    Private Const sModulo As String = "Conciliación Bancaria"
    Private Const nTabla As String = "concilia"
    Private Const nTablaSaldos As String = "tblsaldos"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private dtSaldos As New DataTable
    Private ft As New Transportables

    Private InicioEjercicio As Date, CierreEjercicio As Date
    Private EjercicioActual As String
    Private FechaCon As Date
    Private Mes As Integer, Año As Integer
    Private b_Control As Boolean = False

    Public Sub Cargar(ByVal MyCon As MySqlConnection)
        myConn = MyCon
        Me.Tag = sModulo
        IniciarEjercicios(myConn)
        IniciarCuentasEnCombo(myConn, lblInfo, cmbCuenta)
        IniciarMesesEnComboPlus(myConn, cmbMes, cmbEjercicio.Text.Substring(0, 5))

        'If CargaEjercicio() Then CargaConciliacion()

        ft.mensajeEtiqueta(lblInfo, "Seleccione ejercicio, cuenta y mes de conciliación... ", Transportables.tipoMensaje.iAyuda)
        Me.ShowDialog()

    End Sub
    Private Sub IniciarEjercicios(ByVal MyCon As MySqlConnection)
        Dim eCont As Integer
        Dim nTablaE As String = "tEjercicio"
        ds = DataSetRequery(ds, " select * from jsconctaeje where id_emp = '" & jytsistema.WorkID & "' order by ejercicio desc ", myConn, nTablaE, lblInfo)
        Dim aEjercicio(ds.Tables(nTablaE).Rows.Count) As String
        aEjercicio(0) = "00000 | Ejercicio Actual"
        With ds.Tables(nTablaE)
            For eCont = 0 To .Rows.Count - 1
                aEjercicio(eCont + 1) = .Rows(eCont).Item("ejercicio") & " | " & _
                    ft.FormatoFecha(CDate(.Rows(eCont).Item("inicio").ToString)) & " | " & ft.FormatoFecha(CDate(.Rows(eCont).Item("cierre").ToString))
            Next
        End With

        ds.Tables(nTablaE).Dispose()

        ft.RellenaCombo(aEjercicio, cmbEjercicio)

    End Sub

    Private Function CargaEjercicio() As Boolean

        CargaEjercicio = False
        If Mid(cmbEjercicio.Text, 1, 5) <> "00000" Then
            InicioEjercicio = ft.DevuelveScalarFecha(myConn, " select inicio from jsconctaeje where ejercicio = '" & cmbEjercicio.Text.Substring(0, 5) & "' and id_emp = '" & jytsistema.WorkID & "'   ")
            CierreEjercicio = ft.DevuelveScalarFecha(myConn, " select cierre from jsconctaeje where ejercicio = '" & cmbEjercicio.Text.Substring(0, 5) & "' and id_emp = '" & jytsistema.WorkID & "'   ")
            EjercicioActual = cmbEjercicio.Text.Substring(0, 5)
        Else
            InicioEjercicio = ft.DevuelveScalarFecha(myConn, " select inicio from jsconctaemp where id_emp = '" & jytsistema.WorkID & "'   ")
            CierreEjercicio = ft.DevuelveScalarFecha(myConn, " select cierre from jsconctaemp where id_emp = '" & jytsistema.WorkID & "'   ")
            EjercicioActual = ""
        End If

        CargaEjercicio = True
    End Function
    Private Sub CargaConciliacion()
        Dim sfecha As String
        Dim tblCaja As String = "tblcaja" & ft.NumeroAleatorio(100000)



        If cmbCuenta.Text <> "" And cmbMes.Text <> "" Then

            Mes = ft.InArray(aMesesReal, cmbMes.SelectedItem.Split("-")(0)) + 1
            If Mes >= 1 And Mes <= 12 Then

                Año = cmbMes.SelectedItem.Split("-")(1)

                sfecha = CDate("01/" & Format(Mes, "00") & "/" & Year(CierreEjercicio)).ToString

                If cmbCuenta.Text <> "" AndAlso IsDate(sfecha) Then

                    Dim aFields() As String = {"sel.entero.1.0", "fechamov.fecha.0.0", "tipomov.cadena.2.0", "numdoc.cadena.15.0", "concepto.cadena.250.0", _
                                               "importe.doble.19.2", "origen.cadena.3.0", "benefic.cadena.250.0", "conciliado.cadena.2.0"} ', "block_date.fecha.0.0"

                    ft.Ejecutar_strSQL(myConn, " drop temporary table if exists " + tblCaja)
                    CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblCaja, aFields)


                    FechaCon = CDate("01/" & Format(Mes, "00") & "/" & Format(Año, "0000"))

                    ft.Ejecutar_strSQL(myConn, " insert into " & tblCaja & " " _
                        & " select CAST(if(conciliado = '0', '0', if(mesconcilia > '" & ft.FormatoFechaMySQL(UltimoDiaMes(FechaCon)) & "', '0', '1' ) ) AS SIGNED INTEGER) sel, " _
                        & " fechamov, tipomov, numdoc, concepto, importe, origen, benefic,  " _
                        & " if(conciliado = '0', 'No', if(mesconcilia > '" & ft.FormatoFechaMySQL(UltimoDiaMes(FechaCon)) & "', 'No', 'Si' ) ) conciliado " _
                        & " from jsbantraban a " _
                        & " Where " _
                        & " a.fechamov <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(FechaCon)) & "' and " _
                        & " (a.mesconcilia > '" & ft.FormatoFechaMySQL(UltimoDiaMes(FechaCon)) & "'  or a.mesconcilia in ('1963-05-07','2010-01-01') ) and " _
                        & " codban = '" & Mid(cmbCuenta.Text, 1, 5) & "' and " _
                        & " id_emp = '" & jytsistema.WorkID & "' " _
                        & " UNION " _
                        & " SELECT CAST(if(conciliado = '0', '0', if(mesconcilia > '" & ft.FormatoFechaMySQL(UltimoDiaMes(FechaCon)) & "', '0', '1' ) ) AS SIGNED INTEGER) sel, " _
                        & " fechamov, tipomov, numdoc, concepto, importe, origen, benefic,  " _
                        & " if(conciliado = '0', 'No', if(mesconcilia > '" & ft.FormatoFechaMySQL(UltimoDiaMes(FechaCon)) & "', 'No', 'Si' ) ) conciliado " _
                        & " FROM jsbantraban a " _
                        & " WHERE " _
                        & " fechamov <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(FechaCon)) & "' AND " _
                        & " (a.mesconcilia >= '" & ft.FormatoFechaMySQL(PrimerDiaMes(FechaCon)) & "'  AND a.mesconcilia <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(FechaCon)) & "' ) AND " _
                        & " codban = '" & Mid(cmbCuenta.Text, 1, 5) & "' and " _
                        & " id_emp = '" & jytsistema.WorkID & "' ")

                    dt = ft.AbrirDataTable(ds, nTabla, myConn, " select * from " & tblCaja & " order by fechamov, numdoc ")

                    IniciarMovimientosConciliacion()

                    SaldoMesAnterior(FechaCon)

                End If
            End If
            ConsolidadoXConsolidar()

        Else
            dg.Columns.Clear()
        End If

    End Sub
    Private Sub IniciarMovimientosConciliacion()

        Dim aCampos() As String = {"fechamov.FECHA.80.C.fecha", _
                                   "tipomov.TP.30.C.", _
                                   "numdoc.Documento.100.I.", _
                                   "concepto.Concepto.250.I.", _
                                   "Benefic.BENEFICIARIO.250.I.", _
                                   "origen.ORG.30.C.", _
                                   "Importe.Importe.120.D.Numero", _
                                   "conciliado.Conc.50.C."}

        ft.IniciarTablaPlus(dg, dt, aCampos, , True, , , , , False)
        Dim aCamposEdicion() As String = {"sel"}
        ft.EditarColumnasEnDataGridView(dg, aCamposEdicion)
    
    End Sub
    Private Sub SaldoMesAnterior(ByVal MesConciliacion As Date)

        txtCRMesAnt.Text = ft.FormatoNumero(CalculaSaldoBanco(myConn, lblInfo, Mid(cmbCuenta.Text, 1, 5), True, DateAdd(DateInterval.Day, -1, MesConciliacion), 1))
        txtDBMesAnt.Text = ft.FormatoNumero(CalculaSaldoBanco(myConn, lblInfo, Mid(cmbCuenta.Text, 1, 5), True, DateAdd(DateInterval.Day, -1, MesConciliacion), 0))
        txtSalMesAnt.Text = ft.FormatoNumero(CalculaSaldoBanco(myConn, lblInfo, Mid(cmbCuenta.Text, 1, 5), True, DateAdd(DateInterval.Day, -1, MesConciliacion), 2))

        txtCRMesAct.Text = ft.FormatoNumero(CalculaSaldoBanco(myConn, lblInfo, Mid(cmbCuenta.Text, 1, 5), True, UltimoDiaMes(MesConciliacion), 1) - ValorNumero(txtCRMesAnt.Text))
        txtDBMesAct.Text = ft.FormatoNumero(CalculaSaldoBanco(myConn, lblInfo, Mid(cmbCuenta.Text, 1, 5), True, UltimoDiaMes(MesConciliacion), 0) - ValorNumero(txtDBMesAnt.Text))
        txtSalMesAct.Text = ft.FormatoNumero(CalculaSaldoBanco(myConn, lblInfo, Mid(cmbCuenta.Text, 1, 5), True, UltimoDiaMes(MesConciliacion), 2) - ValorNumero(txtSalMesAnt.Text))

    End Sub

    Private Sub ConsolidadoXConsolidar()
        'Dim kCont As Integer

        Dim sPositivoCon As Double = 0.0
        Dim sNegativoCon As Double = 0.0
        Dim sPositivoXCon As Double = 0.0
        Dim sNegativoXCon As Double = 0.0
        Dim sValor As Double


        Try
            For Each selectedItem As DataRow In dt.Rows
                sValor = selectedItem.Item("importe")
                If CBool(selectedItem.Item("sel")) Then
                    If sValor > 0 Then
                        sPositivoCon += sValor
                    Else
                        sNegativoCon += sValor
                    End If
                Else
                    If sValor > 0 Then
                        sPositivoXCon += sValor
                    Else
                        sNegativoXCon += sValor
                    End If
                End If
            Next


            txtCRC.Text = ft.FormatoNumero(sPositivoCon)
            txtDBC.Text = ft.FormatoNumero(Math.Abs(sNegativoCon))
            txtSalC.Text = ft.FormatoNumero(sPositivoCon - Math.Abs(sNegativoCon))

            txtCRNC.Text = ft.FormatoNumero(sPositivoXCon)
            txtDBNC.Text = ft.FormatoNumero(Math.Abs(sNegativoXCon))

            txtSalNC.Text = ft.FormatoNumero(sPositivoXCon - Math.Abs(sNegativoXCon))


        Catch ex As Exception

        End Try

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

   
    Private Sub jsBanProConciliacion_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt.Dispose()
        dtSaldos.Dispose()
    End Sub

    Private Sub jsBanProConciliacion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        GuardarRemesa()
        ImprimirConciliacion()
        CargaConciliacion()

    End Sub
    Private Sub GuardarRemesa()
        InsertarAuditoria(myConn, MovAud.iIncluir, sModulo, txtSalC.Text)
        ActualizaConsolidados()
    End Sub
    Private Sub ImprimirConciliacion()
        '
        If ft.Pregunta("Desea imprimir la conciliación", sModulo) = Windows.Forms.DialogResult.Yes Then
                Dim f As New jsBanRepParametros
                f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cConciliacion, "Conciliación Bancaria", Mid(cmbCuenta.Text, 1, 5), , FechaCon)
                f = Nothing
        End If

    End Sub
    Private Sub ActualizaConsolidados()

        Dim sFec As Date
        Dim sTip As String
        Dim sNum As String
        Dim sCon As String
        Dim sImp As Double
        Dim sOrg As String


        ft.Ejecutar_strSQL(myconn, "DELETE FROM jsbantracon " _
            & " where " _
            & " MESCONCILIA = '" & ft.FormatoFechaMySQL(CDate("01/" & CStr(Mes) & "/" & CStr(Año))) & "' AND " _
            & " CODBAN = '" & Mid(cmbCuenta.Text, 1, 5) & "' AND " _
            & " ID_EMP = '" & jytsistema.WorkID & "' ")

        'For iCont = 0 To lv.Items.Count - 1
        For Each dtRow As DataRow In dt.Rows

            sFec = Convert.ToDateTime(dtRow.Item("fechamov"))
            sTip = dtRow.Item("tipomov")
            sNum = dtRow.Item("numdoc")
            sCon = dtRow.Item("concepto")
            sImp = ValorNumero(dtRow.Item("Importe"))
            sOrg = dtRow.Item("origen")

            If CBool(dtRow.Item("sel")) Then

                'If lv.Items(iCont).Checked Then
                ' ACTUALIZA MOVIMIENTOS CONCILIADOS
                ft.Ejecutar_strSQL(myConn, "UPDATE jsbantraban SET " _
                    & " CONCILIADO = '1' ," _
                    & " MESCONCILIA = '" & ft.FormatoFechaMySQL(CDate("01/" & CStr(Mes) & "/" & CStr(Año))) & "', " _
                    & " FECCONCILIA = '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' WHERE " _
                    & " FECHAMOV = '" & ft.FormatoFechaMySQL(sFec) & "' AND " _
                    & " TIPOMOV = '" & sTip & "' AND " _
                    & " NUMDOC = '" & sNum & "' AND " _
                    & " CONCEPTO = '" & sCon & "' AND " _
                    & " ORIGEN = '" & sOrg & "' and " _
                    & " CODBAN = '" & Mid(cmbCuenta.Text, 1, 5) & "' AND " _
                    & " " _
                    & " ID_EMP = '" & jytsistema.WorkID & "' ")

                InsertEditBANCOSMovimientoConciliacion(myConn, lblInfo, True, sFec, sNum, sTip, Mid(cmbCuenta.Text, 1, 5), sCon, _
                                                       sImp, "1", sOrg, CDate("01/" & CStr(Mes) & "/" & CStr(Año)), jytsistema.sFechadeTrabajo)

            Else
                ft.Ejecutar_strSQL(myConn, "UPDATE jsbantraban SET " _
                    & " CONCILIADO = '0' ," _
                    & " MESCONCILIA = '1963-05-07', " _
                    & " FECCONCILIA = '1963-05-07' WHERE " _
                    & " FECHAMOV = '" & ft.FormatoFechaMySQL(sFec) & "' AND " _
                    & " TIPOMOV = '" & sTip & "' AND " _
                    & " NUMDOC = '" & sNum & "' AND " _
                    & " CONCEPTO = '" & sCon & "' AND " _
                    & " ORIGEN = '" & sOrg & "' and " _
                    & " CODBAN = '" & Mid(cmbCuenta.Text, 1, 5) & "' AND " _
                    & " ID_EMP = '" & jytsistema.WorkID & "' ")
            End If
        Next

    End Sub


    Private Sub cmbEjercicio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        cmbEjercicio.SelectedIndexChanged, cmbCuenta.SelectedIndexChanged, cmbMes.SelectedIndexChanged

        If sender.name <> "cmbMes" Then IniciarMesesEnComboPlus(myConn, cmbMes, cmbEjercicio.Text.Substring(0, 5))
        If CargaEjercicio() Then CargaConciliacion()

    End Sub

    Private Sub txtSalMesAnt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        txtSalMesAnt.TextChanged, txtSalMesAct.TextChanged

        txtSaldoEnLibros.Text = ft.FormatoNumero(ValorNumero(txtSalMesAnt.Text) + ValorNumero(txtSalMesAct.Text))

    End Sub

    Private Sub txtSalNC_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        txtSaldoEnLibros.TextChanged, txtSalNC.TextChanged

        txtSaldoEnBancos.Text = ft.FormatoNumero(ft.FormatoNumero(ValorNumero(txtSaldoEnLibros.Text) - ValorNumero(txtSalNC.Text)))

    End Sub

   
    Private Sub dg_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles _
        dg.CellContentClick

        If e.ColumnIndex = 0 Then dt.Rows(e.RowIndex).Item(0) = Not CBool(dt.Rows(e.RowIndex).Item(0).ToString)
        ConsolidadoXConsolidar()

    End Sub
  
    Private Sub dg_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dg.CellValidating

        'If e.ColumnIndex = 0 And ConciliacionCargada Then
        '    If FechaUltimoBloqueo(myConn, "jsbantraban") >= CDate(dg.Rows(e.RowIndex).Cells(1).Value.ToString) Then
        '        ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE ó BLOQUEO...")
        '        Return
        '    End If
        'End If

    End Sub


End Class