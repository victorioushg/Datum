Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsContabProProcesarCuentas
    Private Const sModulo As String = "Procesar saldos cuentas contables"
    Private Const nTabla As String = "tblprocesarcuentas"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private documentos_procesados As String = ""
    Private seleccionarTodo As Boolean = False


    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        myConn = MyCon
        Me.Tag = sModulo

        Dim dates As SfDateTimeEdit() = {txtFechaDesde, txtFechaHasta}
        SetSizeDateObjects(dates)

        lblLeyenda.Text = " Mediante este proceso se recalculan los saldos de las cuentas contables " + vbCr +
                " en un período de tiempo determinado.  "

        IniciarValores()

        ft.mensajeEtiqueta(lblInfo, "Seleccione periodo para proceso ... ", Transportables.tipoMensaje.iAyuda)
        Me.Show()

    End Sub
    Private Sub IniciarValores()

        lblEjercicio.Text = IIf(jytsistema.WorkExercise = "", "00000", jytsistema.WorkExercise) +
           " | " + ft.FormatoFecha(FechaInicioEjercicio(myConn, lblInfo)) + " | " + ft.FormatoFecha(FechaCierreEjercicio(myConn, lblInfo))

        txtFechaDesde.Value = FechaInicioEjercicio(myConn, lblInfo)
        txtFechaHasta.Value = FechaCierreEjercicio(myConn, lblInfo)

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub


    Private Sub jsContabProProcesaAsientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt.Dispose()
        ds.Dispose()
        dt = Nothing
        ds = Nothing
        ft = Nothing
    End Sub

    Private Sub jsContabProProcesaAsientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        ProcesarCuentas()
        InsertarAuditoria(myConn, MovAud.iProcesar, sModulo, documentos_procesados)

    End Sub
    Private Sub IniciarProgreso()
        ProgressBar1.Value = 0
        lblProgreso.Text = ""
    End Sub
    Private Sub ProcesarCuentas()

        Dim iCont As Integer = 0

        Dim FechaInicial As Date = CDate(txtFechaDesde.Text)

        While FechaInicial <= CDate(txtFechaHasta.Text)
            dt = ft.AbrirDataTable(ds, nTabla, myConn, " select * from jscotcatcon where RIGHT(codcon,1 ) <> '.' AND id_emp = '" & jytsistema.WorkID & "' order by codcon ")
            Dim kCont As Integer = 1

            For Each nRow As DataRow In dt.Rows
                With nRow

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, kCont / dt.Rows.Count * 100, ft.FormatoFecha(FechaInicial) & " | " &
                        .Item("codcon") & "   " & .Item("descripcion"))
                    ActualizaSaldosCuentasContables(myConn, lblInfo, .Item("codcon"), FechaInicial)

                    kCont += 1

                End With
            Next

            FechaInicial = DateAdd("d", 1, FechaInicial)

        End While

        ft.mensajeInformativo("PROCESO CULMINADO CON EXITO ...")

    End Sub

End Class