Imports MySql.Data.MySqlClient
Public Class jsNomProReversarNomina
    Private Const sModulo As String = "Reversar nómina"
    Private Const nTabla As String = "revnomina"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Public Sub Cargar(ByVal MyCon As MySqlConnection)
        myConn = MyCon
        Me.Tag = sModulo

        lblLeyenda.Text = " Mediante este proceso se reversa la última nómina en histórico... " + vbCr + _
                " 1. Elimina conceptos de nómina colocados en histórico. " + vbCr + _
                " 2. Elimina la o las cuotas/préstamo. " + vbCr + _
                " 3. Re-Calcula saldo cuota/préstamo." + vbCr + _
                " 4. Elimina de cuentas por pagar el saldo o los saldos de proveedor de gastos asociado al concepto. " + vbCr + _
                " 5. Actualiza tabla de fechas de Nómina procesadas."

        ft.mensajeEtiqueta(lblInfo, " ... ", Transportables.TipoMensaje.iAyuda)

        ft.RellenaCombo(aTipoNomina, cmbTipo)

        Me.Show()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub jsNomProReversarNomina_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt.Dispose()
    End Sub

    Private Sub jsNomProReversarNomina_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ft.habilitarObjetos(False, True, txtNomina)
    End Sub
    Private Function Validado() As Boolean

        If txtNomina.Text.Trim = "" Then
            ft.mensajeCritico("No existen nóminas para reversar...")
            Return False
        End If

        Dim aItem() As String = {" CODNOM = '" & txtCodNom.Text & "' AND "}
        If FechaUltimoBloqueo(myConn, "jsnomhisdes", aItem) >= CDate(Split(txtNomina.Text, "|")(1).ToString) Then
            ft.mensajeEtiqueta(lblInfo, "FECHA MENOR QUE ULTIMA FECHA DE CIERRE...", Transportables.tipoMensaje.iAdvertencia)
            Return False
        End If

        If lblNomina.Text.Trim = "" Then
            ft.mensajeCritico("Debe indicar/seleccionar una NOMINA VALIDA...")
            Return False
        End If

        If txtNomina.Text = "" Then
            ft.mensajeCritico("No existe nómina para reversar...")
            Return False
        End If

        'Se debe validar si una nómina YA ha sido depositada o pagada no puede reversarse

        Return True

    End Function
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If Validado() Then
            ReversarNomina()
            Me.Close()
        End If

    End Sub
    Private Sub ReversarNomina()
        Dim iCont As Integer = 0
        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        For iCont = 0 To dt.Rows.Count - 1
            With dt.Rows(iCont)
              
                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (iCont + 1) / dt.Rows.Count * 100, " Trabajador : " + .Item("codtra") + " " + .Item("nomtra"))
                '1. Elimina conceptos de nómina colocandos en histórico. 
                ft.Ejecutar_strSQL(myconn, " delete from jsnomhisdes " _
                               & " where " _
                               & " codtra = '" & .Item("codtra") & "' and  " _
                               & " desde = '" & ft.FormatoFechaMySQL(CDate(Split(txtNomina.Text, "|")(0).ToString)) & "' and " _
                               & " hasta = '" & ft.FormatoFechaMySQL(CDate(Split(txtNomina.Text, "|")(1).ToString)) & "' and " _
                               & " codnom = '" & txtCodNom.Text & "' and " _
                               & " id_emp = '" & jytsistema.WorkID & "' ")

                '2. Elimina la o las cuotas/préstamo.
                ft.Ejecutar_strSQL(myconn, " update jsnomrenpre " _
                               & " set procesada = 0, fechainicio = '2009-01-01', fechafin = '2009-01-01'  " _
                               & " where " _
                               & " codtra = '" & .Item("codtra") & "' and " _
                               & " procesada = 1 and " _
                               & " fechainicio = '" & ft.FormatoFechaMySQL(CDate(Split(txtNomina.Text, "|")(0).ToString)) & "' and " _
                               & " fechafin = '" & ft.FormatoFechaMySQL(CDate(Split(txtNomina.Text, "|")(1).ToString)) & "' and " _
                               & " id_emp = '" & jytsistema.WorkID & "' ")

                ft.Ejecutar_strSQL(myconn, " DELETE a " _
                               & " FROM jsnomrenpre a " _
                               & " LEFT JOIN jsnomencpre b ON (a.codtra = b.codtra AND a.codpre = b.codpre AND a.id_emp = b.id_emp) " _
                               & " WHERE " _
                               & " b.numcuotas = 9999 AND " _
                               & " a.procesada = 1 AND  " _
                               & " a.fechainicio = '" & ft.FormatoFechaMySQL(CDate(Split(txtNomina.Text, "|")(0).ToString)) & "' AND " _
                               & " a.fechafin = '" & ft.FormatoFechaMySQL(CDate(Split(txtNomina.Text, "|")(1).ToString)) & "' AND " _
                               & " a.id_emp = '" & jytsistema.WorkID & "'")

            End With

        Next

        '3.- Calcula saldo de cuotas/prestasmo
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, "Actualizando saldos de cuotas/préstamos ... ")
        ft.Ejecutar_strSQL(myconn, " UPDATE jsnomencpre a, (SELECT codtra, codpre, SUM(monto) saldo, id_emp " _
                                & "                     FROM jsnomrenpre " _
                                & "                     WHERE " _
                                & "                     procesada = 0 AND " _
                                & "                     id_emp = '" & jytsistema.WorkID & "' " _
                                & "                     GROUP BY " _
                                & "                     codtra, codpre) b " _
                                & " SET a.saldo = b.saldo " _
                                & " WHERE " _
                                & " a.codtra =  b.codtra AND  " _
                                & " a.codpre = b.codpre AND " _
                                & " a.id_emp = b.id_emp ")

        '4. Elimina de cuentas por pagar el saldo o los saldos de proveedor de gastos asociado al concepto.  
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " Eliminando acumulados por concepto a CXP de proveedor de gastos... ")
        ActualizarCXP()

        '5.- Actualiza histórico de nómina
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " Actualizando fechas nómina ...")
        ActualizaFechaNomina()

        ft.mensajeInformativo("PROCESO CULMINADO ...")
        lblProgreso.Text = ""
        ProgressBar1.Value = 0

    End Sub
    Private Sub ActualizarCXP()

        ft.Ejecutar_strSQL(myconn, "DELETE FROM jsprotrapag " _
                        & " where mid(refer,1,16) = '" & Format(CDate(Split(txtNomina.Text, "|")(0).ToString), "yyyyMMdd") _
                        & Format(CDate(Split(txtNomina.Text, "|")(1).ToString), "yyyyMMdd") & "' and " _
                        & " id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub ActualizaFechaNomina()

        ft.Ejecutar_strSQL(myconn, " delete from jsnomfecnom " _
                        & " where " _
                        & " desde = '" & ft.FormatoFechaMySQL(CDate(Split(txtNomina.Text, "|")(0).ToString)) & "' and " _
                        & " hasta = '" & ft.FormatoFechaMySQL(CDate(Split(txtNomina.Text, "|")(1).ToString)) & "' and " _
                        & " codnom = '" & txtCodNom.Text & "' and " _
                        & " id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub ProcesarConcepto(ByVal CodigoTrabajador As String, ByVal CodigoConcepto As String, ByVal Desde As Date, _
                                 ByVal Hasta As Date, ByVal Importe As Double, ByVal CodigoPrestamo As String, ByVal num_cuota As Integer, _
                                 PorcentajeAsignacion As Double, CodigoContable As String)

        Dim aFld() As String = {"codtra", "codcon", "desde", "hasta", "id_emp", "CODNOM"}
        Dim aFldS() As String = {CodigoTrabajador, CodigoConcepto, ft.FormatoFechaMySQL(Desde), ft.FormatoFechaMySQL(Hasta), jytsistema.WorkID, txtCodNom.Text}
        Dim Insertar As Boolean = True
        If qFound(myConn, lblInfo, "jsnomhisdes", aFld, aFldS) Then Insertar = False
        InsertEditNOMINAHistoricoConcepto(myConn, lblInfo, Insertar, CodigoTrabajador, CodigoConcepto, Desde, Hasta, _
                        Importe, CodigoPrestamo, num_cuota, "", jytsistema.sFechadeTrabajo, PorcentajeAsignacion, CodigoContable, _
                         txtCodNom.Text)

    End Sub

    Private Sub cmbTipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipo.SelectedIndexChanged

        ColocaUltimaNomina(cmbTipo.SelectedIndex)

        strSQL = " SELECT a.codtra, CONCAT(a.apellidos, ', ', a.nombres) nomtra " _
                 & " FROM jsnomcattra a " _
                 & " LEFT JOIN jsnomrennom b on (a.codtra = b.codtra and a.id_emp = b.id_emp) " _
                 & " where " _
                 & " b.codnom = '" & txtCodNom.Text & "' AND  " _
                 & " a.id_emp = '" & jytsistema.WorkID & "'  " _
                 & " order by a.codtra "

    End Sub
    Private Sub ColocaUltimaNomina(ByVal TipoNomina As Integer)

        Dim nTablaNom As String = "tblnom"
        Dim dtNom As DataTable
        ds = DataSetRequery(ds, " select * from jsnomfecnom where tiponom = " & TipoNomina & " and codnom = '" & txtCodNom.Text & "' AND id_emp = '" & jytsistema.WorkID & "' order by desde desc limit 1 ", myConn, nTablaNom, lblInfo)
        dtNom = ds.Tables(nTablaNom)
        If dtNom.Rows.Count > 0 Then
            With dtNom.Rows(0)
                txtNomina.Text = ft.FormatoFecha(CDate(.Item("desde").ToString)) & "|" & ft.FormatoFecha(CDate(.Item("hasta").ToString))
            End With
        Else
            txtNomina.Text = ""
        End If
    End Sub

    Private Sub btnNomina_Click(sender As System.Object, e As System.EventArgs) Handles btnNomina.Click
        txtCodNom.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codnom codigo, descripcion from jsnomencnom where id_emp = '" & jytsistema.WorkID & "' order by codnom ", "NOMINAS", txtCodNom.Text)
        ft.RellenaCombo(aTipoNomina, cmbTipo, ft.DevuelveScalarEntero(myConn, " SELECT TIPONOM FROM jsnomencnom WHERE CODNOM = '" & txtCodNom.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "'  "))
    End Sub

    Private Sub txtCodNom_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCodNom.TextChanged
        lblNomina.Text = ft.DevuelveScalarCadena(myConn, " SELECT DESCRIPCION FROM jsnomencnom where CODNOM = '" & txtCodNom.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
    End Sub
End Class