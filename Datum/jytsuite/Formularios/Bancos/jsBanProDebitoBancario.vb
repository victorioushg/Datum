Imports MySql.Data.MySqlClient
Public Class jsBanProDebitoBancario
    Private Const sModulo As String = "Impuesto al débito bancario"
    Private Const nTabla As String = "tbiidb"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private ProcesarIDB As Boolean
    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal Procesar As Boolean)

        myConn = MyCon
        Me.Tag = sModulo
        ProcesarIDB = Procesar

        IniciarCuentasEnCombo(myConn, lblInfo, cmbCuenta)
        IniciarMesesEnCombo(cmbMes)
        lblAño.Text = Year(jytsistema.sFechadeTrabajo)
        ft.mensajeEtiqueta(lblInfo, "Seleccione cuenta y mes del IDB ... ", Transportables.TipoMensaje.iAyuda)
        If ProcesarIDB Then
            lblLeyenda.Text = " Mediante este proceso se calculan y se generan las notas débito " + vbCr + _
                " correspondientes al I.D.B. (Impuesto al Débito Bancario).  " + vbCr + _
                " Para proceder deben inicialiarze el o los tipos de documentos a los cuales aplica, " + vbCr + _
                " el porcentaje del IDB, el monto mínimo de la transacción y la descripción del impuesto " + vbCr + _
                " como concepto en la sección de parámetros del sistema. "
        Else
            lblLeyenda.Text = " Este proceso reversa las notas débito generadas por un proceso de cálculo " + vbCr + _
                " del I.D.B. (Impuesto al Débito Bancario).  " + vbCr + _
                " Para proceder deben escojerse el banco/cuenta y el mes procesado. "
        End If
        Me.Show()

    End Sub
    
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub jsBanProDebitoBancario_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt.Dispose()
    End Sub

    Private Sub jsBanProDebitoBancario_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Dim lastMonthDay As Date = UltimoDiaMes(Convert.ToDateTime(lblAño.Text & "-" & cmbMes.SelectedIndex + 1 & "-01"))
        If FechaUltimoBloqueo(myConn, "jsbantraban") >= lastMonthDay Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return
        End If

        EliminarIDB()
        If ProcesarIDB Then
            GenerarIDB()
            ft.mensajeInformativo(" CALCULO DEL DEBITO BANCARIO REALIZADO CON EXITO ...")
        Else
            ft.mensajeInformativo(" REVERSO DEL DEBITO BANCARIO REALIZADO CON EXITO ...")
        End If

        ProgressBar1.Value = 0
        lblProgreso.Text = ""

    End Sub
    Private Sub EliminarIDB()

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " Eliminando I.D.B. ")
        ft.Ejecutar_strSQL(myconn, "DELETE from jsbantraban where " _
        & " NUMORG = 'DB" & Format(cmbMes.SelectedIndex + 1, "00") & CStr(Year(jytsistema.sFechadeTrabajo)) & "' and " _
        & " ID_EMP = '" & jytsistema.WorkID & "' and " _
        & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
        & " CODBAN = '" & Mid(cmbCuenta.Text, 1, 5) & "' ")

    End Sub
    Private Sub GenerarIDB()

        Dim iCont As Integer

        Dim strIDB As String = Convert.ToString(ParametroPlus(myConn, Gestion.iBancos, "BANPARAM01"))
        Dim MontoMinimo As Double = Convert.ToDouble(ParametroPlus(myConn, Gestion.iBancos, "BANPARAM03"))

        Dim strSQL As String = "select * from jsbantraban where " _
            & " MONTH(FECHAMOV) = " & cmbMes.SelectedIndex + 1 & " and " _
            & " YEAR(FECHAMOV) = " & lblAño.Text & " and " _
            & " LOCATE(TIPOMOV,'" & strIDB & "') <> 0 and " _
            & " SUBSTRING(NUMORG,1,2) <> 'DB' and " _
            & " CODBAN = '" & Mid(cmbCuenta.Text, 1, 5) & "' and " _
            & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
            & " ID_EMP = '" & jytsistema.WorkID & "' order by FECHAMOV "

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        With dt
            If .Rows.Count > 0 Then
                For iCont = 0 To .Rows.Count - 1
                  
                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (iCont + 1) / .Rows.Count * 100, _
                                                  .Rows(iCont).Item("NUMDOC").ToString & "  " & .Rows(iCont).Item("TIPOMOV").ToString & _
                                                    " " & ft.FormatoFecha(CDate(.Rows(iCont).Item("FECHAMOV").ToString)))

                    If Math.Abs(.Rows(iCont).Item("IMPORTE")) > MontoMinimo Then IncluirDebito(.Rows(iCont))
                Next
            Else
                ft.MensajeCritico("No existem movimientos para procesar ...")
            End If
        End With

    End Sub
    Private Sub IncluirDebito(ByVal nRow As DataRow)

        Dim MontoIDB As Double = Convert.ToDouble(ParametroPlus(myConn, Gestion.iBancos, "BANPARAM02"))
        Dim desIDB As String = Convert.ToString(ParametroPlus(myConn, Gestion.iBancos, "BANPARAM04"))

        With nRow
            InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, CDate(.Item("FECHAMOV").ToString), .Item("NUMDOC").ToString,
                        "ND", Mid(cmbCuenta.Text, 1, 5), "", desIDB + " MES : " + Format(cmbMes.SelectedIndex + 1, "00") + "/" + CStr(Year(jytsistema.sFechadeTrabajo)),
                        .Item("IMPORTE") * MontoIDB / 100, "BAN", "DB" & Format(cmbMes.SelectedIndex + 1, "00") & Year(jytsistema.sFechadeTrabajo),
                        "", "", "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, "ND", "", jytsistema.sFechadeTrabajo,
                        "0", "", "", jytsistema.WorkCurrency.Id, DateTime.Now())
        End With

    End Sub
End Class