Imports MySql.Data.MySqlClient
Public Class jsControlProCierreEjercicio
    Private Const sModulo As String = "Procesar Cierre Ejercicio"
    Private Const nTabla As String = "tblEjercicio"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private InicioAnterior As Date
    Private CierreAnterior As Date
    Private UltimoEjercicio As String = ""
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        myConn = MyCon
        Me.Tag = sModulo

        lblLeyenda.Text = " Mediante este proceso se realiza el cierre del ejercicio de la siguiente forma : " + vbCr + _
                "  " + vbCr + _
                " - Si no esta seguro POR FAVOR CONSULTE con el administrador " + vbCr + _
                " - Crea el nuevo ejercicio, prepara las tablas de la base de datos " + vbCr + _
                " - Traspasa los saldos al nuevo ejercicio y " + vbCr + _
                " - recalcula los saldos del nuevo ejercicio. " + vbCr + _
                " - SI NO ESTA SEGURO por favor consulte CON EL ADMINISTRADOR "

        ft.mensajeEtiqueta(lblInfo, " ... ", Transportables.TipoMensaje.iAyuda)

        Dim afld() As String = {"id_emp"}
        Dim aStr() As String = {jytsistema.WorkID}

        InicioAnterior = FechaInicioEjercicio(myConn, lblInfo)
        CierreAnterior = FechaCierreEjercicio(myConn, lblInfo)
        CargarEjercicio()

        ft.habilitarObjetos(False, False, chk1, chk2, cmbEjercicio)


        Me.Show()

    End Sub
    Private Sub CargarEjercicio()

        Dim aEjercicio() As String = {"00000 | " & ft.FormatoFecha(InicioAnterior) & " | " & ft.FormatoFecha(CierreAnterior)}
        ft.RellenaCombo(aEjercicio, cmbEjercicio)

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub jsControlProCierreEjercicio_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt.Dispose()
    End Sub
    Private Sub jsControlProCierreEjercicio_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Procesar()
    End Sub
    Private Sub Procesar()

        HabilitarCursorEnEspera()

        CrearNuevoEjercicio()
        ProcesarTablas()
        ProgressBar1.Value = 0
        lblProgreso.Text = ""
        DeshabilitarCursorEnEspera()
        ft.mensajeInformativo(" Proceso culminado con éxito... ")
        CargarEjercicio()

    End Sub

    Private Sub CrearNuevoEjercicio()

        Dim NuevoAño As String = ""

        If Mid(cmbEjercicio.Text, 1, 5) = "00000" Then

            ds = DataSetRequery(ds, " select * from jsconctaeje where id_emp = '" & jytsistema.WorkID & "' order by ejercicio ", myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)

            UltimoEjercicio = ft.autoCodigo(myConn, "ejercicio", "jsconctaeje", "id_emp", jytsistema.WorkID, 5)

            lblProgreso.Text = " CREANDO NUEVO EJERCICIO "

            NuevoAño = CStr(Year(InicioAnterior) + 1)
            ProgressBar1.Value = 50
            lblProgreso.Text += " NUEVO EJERCICIO  "

            ft.Ejecutar_strSQL(myconn, " insert into jsconctaeje Values(" _
                & "'" & UltimoEjercicio & "', " _
                & "'" & ft.FormatoFechaMySQL(InicioAnterior) & "', " _
                & "'" & ft.FormatoFechaMySQL(CierreAnterior) & "', " _
                & "'" & jytsistema.WorkID & "'" _
                & ")")

            ProgressBar1.Value = 100
            lblProgreso.Text = " ACTUALIZANDO...  "

            ft.Ejecutar_strSQL(myConn, " update jsconctaemp set " _
                & " INICIO = DATE_ADD(INICIO, INTERVAL 1 YEAR), " _
                & " CIERRE = DATE_ADD(CIERRE, INTERVAL 1 YEAR) " _
                & " WHERE " _
                & " ID_EMP = '" & jytsistema.WorkID & "' " _
                & "")

            InicioAnterior = ft.DevuelveScalarFecha(myConn, " select INICIO from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ")
            CierreAnterior = ft.DevuelveScalarFecha(myConn, " select CIERRE from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ")

        Else
            Dim aDD As Object
            aDD = Split(cmbEjercicio.Text, " | ")
            UltimoEjercicio = aDD(0)
            InicioAnterior = CDate(aDD(1))
            CierreAnterior = CDate(aDD(2))
        End If

        ProgressBar1.Value = 0
        lblProgreso.Text = ""

    End Sub

    Private Sub ProcesarTablas()

        Dim iCont As Integer
        Dim jCont As Integer

        lblProgreso.Text = " PROCESANDO TABLAS "


        ' CATALOGO DE BANCOS
        lblProgreso.Text += " CATALOGO DE BANCOS "

        If Mid(cmbEjercicio.Text, 1, 5) = "00000" Then _
            ft.Ejecutar_strSQL(myconn, " UPDATE jsbandaaban set " _
                & " EJERCICIO = '" & UltimoEjercicio & "' where " _
                & " EJERCICIO = '' AND " _
                & " ID_EMP = '" & jytsistema.WorkID & "' ")

        Dim dtBan As DataTable
        Dim nTableBan As String = "tblBancos"
        ds = DataSetRequery(ds, " select * from jsbancatban where " _
            & " ID_EMP = '" & jytsistema.WorkID & "' ", myConn, nTableBan, lblInfo)

        dtBan = ds.Tables(nTableBan)
        Dim sSaldo As Double = 0.0

        If dtBan.Rows.Count > 0 Then
            For iCont = 0 To dtBan.Rows.Count - 1
                With dtBan.Rows(iCont)
                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(iCont / (dtBan.Rows.Count) * 100), _
                                                  .Item("CODBAN") & " " & .Item("NOMBAN"))
                    If Mid(cmbEjercicio.Text, 1, 5) = "00000" Then ft.Ejecutar_strSQL(myconn, " insert into jsbandaaban " _
                                        & " Values('" & .Item("codban") & "',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,'', '" & jytsistema.WorkID & "' ) ")

                    CalculaSaldoBanco(myConn, lblInfo, .Item("codban"))

                End With
            Next
        End If

        dtBan.Dispose()
        dtBan = Nothing

        ' REALIZAR MISMO PROCESO DE CATALOGO DE BANCOS CON CATALOGO DE CUENTAS
        lblProgreso.Text = " CATALOGO DE CUENTAS CONTABLES "

        If Mid(cmbEjercicio.Text, 1, 5) = "00000" Then ft.Ejecutar_strSQL(myconn, " UPDATE jscotdaacon set " _
            & " EJERCICIO = '" & UltimoEjercicio & "' where " _
            & " EJERCICIO = '' AND " _
            & " ID_EMP = '" & jytsistema.WorkID & "' ")

        Dim dtCon As DataTable
        Dim nTableCon As String = "tblContab"
        ds = DataSetRequery(ds, " select * from jscotcatcon where " _
            & " ID_EMP = '" & jytsistema.WorkID & "' ", myConn, nTableCon, lblInfo)

        dtCon = ds.Tables(nTableCon)

        If dtCon.Rows.Count > 0 Then
            For jCont = 0 To dtCon.Rows.Count - 1
                With dtCon.Rows(jCont)

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(jCont / (dtCon.Rows.Count - 1) * 100), _
                                                  .Item("CODCON") & " " & .Item("DESCRIPCION"))
                    If Mid(cmbEjercicio.Text, 1, 5) = "00000" Then ft.Ejecutar_strSQL(myConn, " insert into jscotdaacon values(" _
                        & "'" & .Item("CODCON") & "', " _
                        & " 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0, " _
                        & "'', '" & jytsistema.WorkID & "'" _
                        & ")")
                End With
            Next
        End If

        dtCon.Dispose()
        dtCon = Nothing

        lblProgreso.Text = " TABLAS DE CONTABILIDAD "
        Dim aTablas() As String = {"jscotencasi", "jscotrenasi"}
        If Mid(cmbEjercicio.Text, 1, 5) = "00000" Then _
            ActualizarTablas(myConn, aTablas, UltimoEjercicio)

        lblProgreso.Text = ""

    End Sub

    Private Sub ActualizarTablas(ByVal MyConn As MySqlConnection, ByVal aArreglo() As String, ByVal Ejercicio As String)

        Dim iiCont As Integer

        For iiCont = 0 To UBound(aArreglo)
            ft.Ejecutar_strSQL(MyConn, " update " & aArreglo(iiCont) & " set " _
                & " EJERCICIO = '" & Ejercicio & "' " _
                & " WHERE " _
                & " EJERCICIO = '' AND " _
                & " ID_EMP = '" & jytsistema.WorkID & "' ")
            refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(iiCont / UBound(aArreglo) * 100), _
                                          "Tabla : " & aArreglo(iiCont))
        Next
        ProgressBar1.Value = 0
        lblProgreso.Text = ""

    End Sub

    Private Sub AsientoCierreContable()

    End Sub

End Class