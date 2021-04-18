Imports MySql.Data.MySqlClient
Public Class jsControlProCierreDiario
    Private Const sModulo As String = "Cierre/Reverso movimientos"

    Private myConn As New MySqlConnection
    Private ft As New Transportables

    Private tipoBloqueo As Integer
    Public Sub Cargar(ByVal MyCon As MySqlConnection, TipoCierre As Integer)

        '0 = Cierre, 1 = Reverso Cierre

        myConn = MyCon
        Me.Tag = sModulo
        tipoBloqueo = TipoCierre

        lblLeyenda.Text = IIf(Convert.ToBoolean(TipoCierre), " Mediante este proceso se produce el CIERRE/BLOQUEO de las diferentes gestiones para una fecha dada " + vbCr + _
                " No podrán ser modificado ó eliminado ningún movimiento cuyo bloqueo se haya producido en el cierre.  ", _
                " Mediante este proceso se produce el DESBLOQUEO de las diferentes gestiones para una fecha dada " + vbCr + _
                " No podrán ser modificado ó eliminado ningún movimiento cuyo bloqueo se haya producido en el cierre.  ") _
                + vbCr + _
                " - Si no esta seguro POR FAVOR CONSULTE con el administrador " + vbCr + _
                " - SI NO ESTA SEGURO por favor consulte CON EL ADMINISTRADOR "

        IniciarCHKs()

        lblUltimoCierre.Text = " FECHA ÚLTIMO CIERRE : " & ft.muestraCampoFecha(ft.DevuelveScalarFecha(myConn, " select fechatrabajo from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' "))

        txtFecha.Text = ft.muestraCampoFecha(jytsistema.sFechadeTrabajo)
        ft.mensajeEtiqueta(lblInfo, " ... ", Transportables.tipoMensaje.iAyuda)

        Me.Show()

    End Sub
    Private Sub IniciarCHKs()

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub jsControlProVerificarBD_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsControlProVerificarBD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Procesar()
    End Sub
    Private Sub Procesar()
        HabilitarCursorEnEspera()

        If chk1.Checked Then ActualizarContabilidad()
        If chk2.Checked Then ActualizarBancos()
        If chk3.Checked Then ActualizarNomina()
        If chk4.Checked Then ActualizarCompras()
        If chk5.Checked Then ActualizarVentas()
        If chk6.Checked Then ActualizarPuntosdeVenta()
        If chk7.Checked Then ActualizarMercancias()
        If chk8.Checked Then ActualizarProduccion()
        If chk9.Checked Then ActualizarFunciones()
        If chk10.Checked Then ActualizarControlDeGestiones()

        ProgressBar1.Value = 0
        lblProgreso.Text = ""
        IniciarCHKs()
        DeshabilitarCursorEnEspera()
        ft.mensajeInformativo(" Proceso culminado con éxito... ")

    End Sub
    Private Sub ActualizarFunciones()

       
    End Sub
    Private Sub BloquearTablas(aTablas() As String)

        For iCont As Integer = 0 To aTablas.Length - 1

            refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (iCont + 1) / aTablas.Length * 100, _
                                          aTablas(iCont).Split("|")(0))

            ft.Ejecutar_strSQL(myConn, " UPDATE " & aTablas(iCont).Split("|")(0) _
                               & IIf(tipoBloqueo = 0, " SET BLOCK_DATE = '" & ft.FormatoFechaMySQL(Convert.ToDateTime(txtFecha.Text)) & "' ", " SET BLOCK_DATE = '2009-01-01' ") _
                               & " WHERE " _
                               & IIf(tipoBloqueo = 0, " BLOCK_DATE = '2009-01-01' AND ", " BLOCK_DATE >= '" & ft.FormatoFechaMySQL(Convert.ToDateTime(txtFecha.Text)) & "' AND ") _
                               & IIf(tipoBloqueo = 0, aTablas(iCont).Split("|")(1), "") _
                               & " ID_EMP = '" & jytsistema.WorkID & "' ")
        Next

    End Sub
   
    Private Sub ActualizarContabilidad()

        Dim fechaBloqueo As String = ft.FormatoFechaMySQL(Convert.ToDateTime(txtFecha.Text)).ToString

        Dim aTablas() As String = {"jscotencasi|ACTUAL = '1' AND FECHASI <= '" & fechaBloqueo & "' AND ", _
                                   "jscottrafij|FIN_PERIODO_VALUACION <= '" & fechaBloqueo & "' AND "}
        BloquearTablas(aTablas)

    End Sub
    Private Sub ActualizarBancos()

        Dim fechaBloqueo As String = ft.FormatoFechaMySQL(Convert.ToDateTime(txtFecha.Text)).ToString

        Dim aTablas() As String = {"jsbantracaj|FECHA <= '" & fechaBloqueo & "' AND ", _
                                   "jsbantraban|FECHAMOV <= '" & fechaBloqueo & "' AND ", _
                                   "jsbantracon|FECCONCILIA <= '" & fechaBloqueo & "' AND", _
                                   "jsbanordpag|"}
        BloquearTablas(aTablas)

    End Sub
    Private Sub ActualizarNomina()

        Dim fechaBloqueo As String = ft.FormatoFechaMySQL(Convert.ToDateTime(txtFecha.Text)).ToString

        Dim aTablas() As String = {"jsnomfecnom|HASTA <= '" & fechaBloqueo & "' AND ", _
                                   "jsnomhisdes|HASTA <= '" & fechaBloqueo & "' AND ", _
                                   "jsnomtrades|"}
        BloquearTablas(aTablas)

    End Sub

    Private Sub ActualizarCompras()

        Dim fechaBloqueo As String = ft.FormatoFechaMySQL(Convert.ToDateTime(txtFecha.Text)).ToString

        Dim aTablas() As String = {"jsprotrapag|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsprohispag|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsproenccom|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsproencgas|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsproencncr|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsproencndb|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsproencord|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsproencprg|FECHAPROCESO <= '" & fechaBloqueo & "' AND ", _
                                   "jsproencrep|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsproexppro|FECHA <= '" & fechaBloqueo & "' AND "}
        BloquearTablas(aTablas)

    End Sub
    Private Sub ActualizarVentas()

        Dim fechaBloqueo As String = ft.FormatoFechaMySQL(Convert.ToDateTime(txtFecha.Text)).ToString

        Dim aTablas() As String = {"jsvencobrgv|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsvenenccot|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsvenencnot|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsvenencfac|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsvenencgui|FECHAGUIA <= '" & fechaBloqueo & "' AND ", _
                                   "jsvenencguipedidos|FECHAGUIA <= '" & fechaBloqueo & "' AND ", _
                                   "jsvenencncr|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsvenencncrrgv|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsvenencndb|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsvenencped|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsvenencpedrgv|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsvenencrel|FECHAGUIA <= '" & fechaBloqueo & "' AND ", _
                                   "jsvenencrgv|FECHA <= '" & fechaBloqueo & "' AND ", _
                                   "jsvenexpcli|DATE_FORMAT(FECHA, '%Y-%m-%d') <= '" & fechaBloqueo & "' AND ", _
                                   "jsventracob|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsvenhiscob|EMISION <= '" & fechaBloqueo & "' AND "}
        BloquearTablas(aTablas)

    End Sub
    Private Sub ActualizarPuntosdeVenta()

        Dim fechaBloqueo As String = ft.FormatoFechaMySQL(Convert.ToDateTime(txtFecha.Text)).ToString

        Dim aTablas() As String = {"jsvenencpos|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsventrapos|FECHACIERRE <= '" & fechaBloqueo & "' AND "}
        BloquearTablas(aTablas)

    End Sub

    Private Sub ActualizarMercancias()

        Dim fechaBloqueo As String = ft.FormatoFechaMySQL(Convert.ToDateTime(txtFecha.Text)).ToString

        Dim aTablas() As String = {"jsmerenccon|FECHAPRO <= '" & fechaBloqueo & "' AND ", _
                                   "jsmerenctra|EMISION <= '" & fechaBloqueo & "' AND ", _
                                   "jsmerexpmer|FECHA <= '" & fechaBloqueo & "' AND ", _
                                   "jsmerhismer|FECHAMOV <= '" & fechaBloqueo & "' AND ", _
                                   "jsmertramer|DATE_FORMAT(FECHAMOV, '%Y-%m-%d') <= '" & fechaBloqueo & "' AND ", _
                                   "jsmertraenv|FECHAMOV <= '" & fechaBloqueo & "' AND ", _
                                   "jsmerencped|EMISION <= '" & fechaBloqueo & "' AND "}
        BloquearTablas(aTablas)

    End Sub

    Private Sub ActualizarProduccion()

        Dim fechaBloqueo As String = ft.FormatoFechaMySQL(Convert.ToDateTime(txtFecha.Text)).ToString

        Dim aTablas() As String = {"jsfabencord|EMISION <= '" & fechaBloqueo & "' AND "}
        BloquearTablas(aTablas)

    End Sub

    Private Sub ActualizarControlDeGestiones()

    End Sub

    Private Sub chk_CheckedChanged(sender As Object, e As EventArgs) Handles chk.CheckedChanged
        For Each cb As Control In grpGestiones.Controls
            If cb.GetType.Equals(chk.GetType) Then ft.setProperty(cb, "Checked", chk.Checked)
        Next
    End Sub

    Private Sub btnFecha_Click(sender As Object, e As EventArgs) Handles btnFecha.Click
        txtFecha.Text = SeleccionaFecha(CDate(txtFecha.Text), Me, grpGestiones, btnFecha)
    End Sub
End Class