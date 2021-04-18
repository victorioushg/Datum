Imports MySql.Data.MySqlClient
Public Class jsContabProApertura

    Private Const sModulo As String = "Proceso generar asiento de apertura"
    Private Const nTabla As String = "proasiaper"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private tbl As String = ""

    Public Sub Cargar(ByVal MyCon As MySqlConnection)
        myConn = MyCon
        Me.Tag = sModulo


        tbl = "tbl" & ft.NumeroAleatorio(100000)

        lblLeyenda.Text = " Mediante este proceso se re-construye el asiento de apertura del ejercicio " + vbCr + _
                " a partir del balance general al 31 de diciembre del ejercicio anterior.  " + vbCr + _
                " Para proceder debe , " + vbCr + _
                "  ... "

        ft.mensajeEtiqueta(lblInfo, " ... ", Transportables.TipoMensaje.iAyuda)

        EjercicioActual()
        EjecicioAnterior(MyCon)

        Me.Show()

    End Sub
    Private Sub EjercicioActual()
        Dim nEjercicio As String = IIf(jytsistema.WorkExercise = "", "00000", jytsistema.WorkExercise)
        If nEjercicio = "00000" Then
            Dim aCampos() As String = {"id_emp"}
            Dim aStrings() As String = {jytsistema.WorkID}
            txtEjercicioActual.Text = "00000 | " & ft.FormatoFecha(CDate(qFoundAndSign(myConn, lblInfo, "jsconctaemp", aCampos, aStrings, "inicio").ToString)) & _
            " | " & ft.FormatoFecha(CDate(qFoundAndSign(myConn, lblInfo, "jsconctaemp", aCampos, aStrings, "cierre").ToString))
        Else
            Dim aFld() As String = {"ejercicio", "id_emp"}
            Dim aStr() As String = {nEjercicio, jytsistema.WorkID}
            txtEjercicioActual.Text = nEjercicio & " | " & ft.FormatoFecha(CDate(qFoundAndSign(myConn, lblInfo, "jsconctaeje", aFld, aStr, "inicio").ToString)) & _
                        " | " & ft.FormatoFecha(CDate(qFoundAndSign(myConn, lblInfo, "jsconctaeje", aFld, aStr, "cierre").ToString))
        End If

    End Sub

    Private Sub EjecicioAnterior(ByVal MyCon As MySqlConnection)
        Dim eCont As Integer = 0
        Dim nTablaE As String = "tEjercicio"

        Dim dtAnterior As DataTable = ft.AbrirDataTable(ds, nTablaE, myConn, _
                            " SELECT a.ejercicio, a.inicio, a.cierre, a.id_emp " _
                            & " FROM jsconctaeje a " _
                            & " WHERE id_emp = '" & jytsistema.WorkID & "' " _
                            & " UNION " _
                            & " SELECT '00000', a.inicio, a.cierre, a.id_emp " _
                            & " FROM jsconctaemp a " _
                            & " WHERE " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " ORDER BY inicio ")

        Dim aEjercicioAnterior As String = ""
        Dim nTrue As Boolean = True

        For eCont = 0 To dtAnterior.Rows.Count - 1
            With dtAnterior

                If .Rows(eCont).Item("ejercicio") = txtEjercicioActual.Text.Substring(0, 5) Then
                    If eCont - 1 >= 0 Then
                        aEjercicioAnterior = .Rows(eCont - 1).Item("ejercicio") & " | " & _
                                                        ft.FormatoFecha(CDate(.Rows(eCont - 1).Item("inicio").ToString)) & " | " & _
                                                        ft.FormatoFecha(CDate(.Rows(eCont - 1).Item("cierre").ToString))
                    End If
                End If

            End With
        Next


        txtEjercicioAnterior.Text = aEjercicioAnterior


    End Sub


    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub


    Private Sub jsContabProContabilizar_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt.Dispose()
    End Sub

    Private Sub jsContabProProcesaAsientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ft.habilitarObjetos(False, False, txtEjercicioActual, txtEjercicioAnterior)
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        ProcesarAsientoApertura()

    End Sub
    Private Sub ProcesarAsientoApertura()

        Dim FechaInicioAnterior As Date = CDate(txtEjercicioAnterior.Text.Split("|")(1).ToString)
        Dim FechaFinAnterior As Date = CDate(txtEjercicioAnterior.Text.Split("|")(2).ToString)
        Dim strSQL As String = SeleccionCOTBalanceGeneral(myConn, lblInfo, FechaInicioAnterior, _
                                                          FechaFinAnterior, 6, True, IIf(txtEjercicioAnterior.Text.Split("|")(0).Trim = "00000", "", txtEjercicioAnterior.Text.Split("|")(0).Trim))


        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim numAsiento As String = Year(CDate(txtEjercicioActual.Text.Split("|")(1))).ToString & "00-00000"
        Dim dCreditos As Double = 0.0
        Dim dDebitos As Double = 0.0

        If dt.Rows.Count > 0 Then
            ft.Ejecutar_strSQL(myconn, " DELETE FROM jscotrenasi where asiento = '" & numAsiento & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myconn, " DELETE FROM jscotencasi where asiento = '" & numAsiento & "' and id_emp = '" & jytsistema.WorkID & "' ")
            Dim iCont As Integer = 1
            For Each dr As DataRow In dt.Rows
                With dr

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, iCont / dt.Rows.Count * 100, .Item("codcon") + " " + .Item("descripcion"))

                    If .Item("CODCON").ToString.Substring(.Item("CODCON").ToString.Length - 1, 1) <> "." And .Item("saldo") <> 0 Then

                        InsertEditCONTABRenglonAsiento(myConn, lblInfo, True, numAsiento, RellenaCadenaConCaracter(iCont.ToString, "D", 5, "0"), _
                                                       .Item("codcon"), "APERTURA", "ASIENTO APERTURA", .Item("SALDO"), "0", _
                                                       IIf(.Item("SALDO") > 0, 0, 1), 0, "")

                        If .Item("SALDO") > 0 Then
                            dCreditos += .Item("SALDO")
                        Else
                            dDebitos += .Item("SALDO")
                        End If

                    End If

                    iCont += 1
                End With
            Next

            InsertEditCONTABEncabezadoAsiento(myConn, lblInfo, True, numAsiento, "", CDate(txtEjercicioAnterior.Text.Split("|")(2)), _
                                               "ASIENTO APERTURA", dCreditos, dDebitos, 0, "")

        End If

        ft.mensajeInformativo("PROCESO CULMINADO ...")
        lblProgreso.Text = ""
        ProgressBar1.Value = 0

    End Sub
    



End Class