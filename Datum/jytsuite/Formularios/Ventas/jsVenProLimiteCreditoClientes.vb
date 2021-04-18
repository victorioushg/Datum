Imports MySql.Data.MySqlClient
Public Class jsVenProLimiteCreditoClientes

    Private Const sModulo As String = "Proceso reconstrucción de límites de crédito"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private strSQL As String = ""

    Private nTabla As String = "tbl"


    Private aFactor() As String = {"Constante"}   ' {"Constante", "Predicción de venta"}
    Private aBase() As String = {"Venta promedio mensual", "Venta último mes"}

    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        MyConn = MyCon
        IniciarTXT()
        Me.Show()

    End Sub
    Private Sub IniciarTXT()

        ft.habilitarObjetos(False, True, txtClienteDesde, txtClienteHasta, txtCanalDesde, txtCanalHasta, txtTipoNegocioDesde, _
                         txtTipoNegocioHasta, txtZonaDesde, txtZonaHasta, txtRutaDesde, txtRutaHasta, txtAsesorDesde, _
                         txtAsesorHasta)

        ft.habilitarObjetos(True, True, txtConstante)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtCanalDesde, txtClienteHasta, txtCanalDesde, txtCanalHasta, txtTipoNegocioDesde, _
            txtTipoNegocioHasta, txtZonaDesde, txtZonaHasta, txtRutaDesde, txtRutaHasta, txtAsesorDesde, txtAsesorHasta)

        ft.RellenaCombo(aEstatusCliente, cmbEstatus)
        ft.RellenaCombo(aBase, cmbBase)
        ft.RellenaCombo(aFactor, cmbFactor)

        txtConstante.Text = ft.FormatoNumero(1.0)

        strSQL = " select codven, concat(nombres,' ',apellidos) nombre from jsvencatven where estatus = 1 and tipo = " & TipoVendedor.iFuerzaventa & " and ID_EMP = '" & jytsistema.WorkID & "' order by codven "
        ds = DataSetRequery(ds, strSQL, MyConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        strSQL = ""

    End Sub

    Private Sub jsVenProLimiteCreditoClientes_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsVenProLimiteCreditoClientes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtClienteHasta.Text)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtClienteHasta.Text)
        Me.Close()
    End Sub

    Private Function Validado() As Boolean

        Validado = False

        If cmbFactor.SelectedIndex = 0 Then
            If ft.isNumeric(txtConstante.Text) Then
                If ValorNumero(txtConstante.Text) < 0 Then
                    ft.mensajeCritico("Valor de constante debe ser mayor a 0 ....")
                    Exit Function
                End If
            Else
                ft.mensajeCritico("Valor de constante no válido. Verifique...")
                Exit Function
            End If
        Else
            ft.MensajeCritico("La opción 'Predición de ventas' no está habilitada en esta versión... ")
        End If

        Validado = True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Dim iCont As Integer


        If Validado() Then
            DeshabilitarCursorEnEspera()
            ds = DataSetRequery(ds, ConsultaLimiteBase(), MyConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)
            If dt.Rows.Count > 0 Then
                For iCont = 0 To dt.Rows.Count - 1

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(iCont / (dt.Rows.Count - 1) * 100), _
                                                  "")
                    With dt.Rows(iCont)
                        Dim LimiteCreditoAnterior As Double = ft.DevuelveScalarDoble(MyConn, " select limitecredito from jsvencatcli where codcli = '" & .Item("codcli") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        ft.Ejecutar_strSQL(MyConn, " update jsvencatcli set limitecredito = " & .Item("venta") * ValorNumero(txtConstante.Text) & ", " _
                            & " disponible  = disponible + " & .Item("venta") * ValorNumero(txtConstante.Text) - LimiteCreditoAnterior & " " _
                            & " where " _
                            & " codcli = '" & .Item("codcli") & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")
                    End With
                Next
            End If

            HabilitarCursorEnEspera()
            MsgBox("Proceso terminado...", MsgBoxStyle.Information, sModulo)

            refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 0, "")

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtClienteHasta.Text)

            Me.Close()

        End If

    End Sub

    Private Function ConsultaLimiteBase() As String
        Dim strSQL As String = ""
        Dim numMeses As Integer

        If txtClienteDesde.Text <> "" Then strSQL = strSQL & " a.codcli >= '" & txtClienteDesde.Text & "' and "
        If txtClienteHasta.Text <> "" Then strSQL = strSQL & " a.codcli <= '" & txtClienteHasta.Text & "' and "
        If txtCanalDesde.Text <> "" Then strSQL = strSQL & " a.categoria >= '" & txtCanalDesde.Text & "' and "
        If txtCanalHasta.Text <> "" Then strSQL = strSQL & " a.categoria <= '" & txtCanalHasta.Text & "' and "
        If txtTipoNegocioDesde.Text <> "" Then strSQL = strSQL & " a.unidad >= '" & txtTipoNegocioDesde.Text & "' and "
        If txtTipoNegocioHasta.Text <> "" Then strSQL = strSQL & " a.unidad <= '" & txtTipoNegocioHasta.Text & "' and "
        If txtZonaDesde.Text <> "" Then strSQL = strSQL & " a.zona >= '" & txtZonaDesde.Text & "' and "
        If txtZonaHasta.Text <> "" Then strSQL = strSQL & " a.zona <= '" & txtZonaHasta.Text & "' and "
        If txtRutaDesde.Text <> "" Then strSQL = strSQL & " a.ruta_visita >= '" & txtRutaDesde.Text & "' and "
        If txtRutaHasta.Text <> "" Then strSQL = strSQL & " a.ruta_visita <= '" & txtRutaHasta.Text & "' and "
        If txtAsesorDesde.Text <> "" Then strSQL = strSQL & " a.codven >= '" & txtAsesorDesde.Text & "' and "
        If txtAsesorHasta.Text <> "" Then strSQL = strSQL & " a.codven <= '" & txtAsesorHasta.Text & "' and "

        If cmbEstatus.SelectedIndex < 4 Then strSQL = strSQL & " a.estatus = " & cmbEstatus.SelectedIndex & " and "
        Dim strNumMeses As String

        If cmbBase.SelectedIndex = 0 Then
            numMeses = 12
            strNumMeses = " if( Date_sub('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', interval 12 month)  >= a.ingreso, " & numMeses & " , round(  (to_days('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "') - to_days(a.ingreso))/30,0  ) )"
        Else
            numMeses = 1
            strNumMeses = CStr(numMeses)
        End If

        ConsultaLimiteBase = "select a.codcli, a.limitecredito, " _
            & " sum(b.tot_fac)/" & 12 & " as venta " _
            & " from jsvencatcli a left join " _
            & " jsvenencfac b on (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
            & " Where " _
            & " b.emision >= Date_sub('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', interval " & numMeses & " month) and " _
            & " b.emision < '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
            & strSQL _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by a.codcli "


    End Function

    Private Sub btnClienteDesde_Click(sender As System.Object, e As System.EventArgs) Handles btnClienteDesde.Click
        txtClienteDesde.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codcli codigo, nombre descripcion, disponible, elt(estatus+1, 'Activo', 'Bloqueado', 'Inactivo', 'Desincorporado', 'Todos') estatus from jsvencatcli where estatus < 3 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Clientes", _
                                           txtClienteDesde.Text)
    End Sub

    Private Sub txtClienteDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtClienteDesde.TextChanged
        txtClienteHasta.Text = txtClienteDesde.Text
    End Sub

    Private Sub btnClienteHasta_Click(sender As System.Object, e As System.EventArgs) Handles btnClienteHasta.Click
        txtClienteHasta.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codcli codigo, nombre descripcion, disponible, elt(estatus+1, 'Activo', 'Bloqueado', 'Inactivo', 'Desincorporado', 'Todos') estatus from jsvencatcli where estatus < 3 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Clientes", _
                                           txtClienteHasta.Text)
    End Sub

    Private Sub btnCanalDesde_Click(sender As System.Object, e As System.EventArgs) Handles btnCanalDesde.Click
        txtCanalDesde.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iCategoriaclientes) & "' and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Canal Distribución", _
                                           txtCanalDesde.Text)
    End Sub

    Private Sub btnCanalHasta_Click(sender As System.Object, e As System.EventArgs) Handles btnCanalHasta.Click
        txtCanalHasta.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iCategoriaclientes) & "' and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Canal Distribución", _
                                          txtCanalHasta.Text)
    End Sub

    Private Sub txtCanalDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCanalDesde.TextChanged
        txtCanalHasta.Text = txtCanalDesde.Text
    End Sub


    Private Sub txtTipoNegocioDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtTipoNegocioDesde.TextChanged
        txtTipoNegocioHasta.Text = txtTipoNegocioDesde.Text
    End Sub

    Private Sub btnTipoDesde_Click(sender As System.Object, e As System.EventArgs) Handles btnTipoDesde.Click
        txtTipoNegocioDesde.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iUnidadClientes) & "' and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Tipo de negocio", _
                                          txtTipoNegocioHasta.Text)
    End Sub

    Private Sub btnTipoNegocioHasta_Click(sender As System.Object, e As System.EventArgs) Handles btnTipoNegocioHasta.Click
        txtTipoNegocioHasta.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iUnidadClientes) & "' and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Tipo de negocio", _
                                          txtTipoNegocioHasta.Text)
    End Sub

    Private Sub btnZonaDesde_Click(sender As System.Object, e As System.EventArgs) Handles btnZonaDesde.Click
        txtZonaDesde.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iZonasClientes) & "' and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Zonas Clientes", _
                                          txtZonaDesde.Text)
    End Sub

    Private Sub btnZonaHasta_Click(sender As System.Object, e As System.EventArgs) Handles btnZonaHasta.Click
        txtZonaHasta.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iZonasClientes) & "' and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Zona Clientes", _
                                          txtZonaHasta.Text)
    End Sub

    Private Sub txtZonaDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtZonaDesde.TextChanged
        txtZonaHasta.Text = txtZonaDesde.Text
    End Sub

    Private Sub btnRutaDesde_Click(sender As System.Object, e As System.EventArgs) Handles btnRutaDesde.Click
        txtRutaDesde.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codrut codigo, nomrut descripcion from jsvenencrut where tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Rutas Visita", _
                                          txtRutaDesde.Text)
    End Sub

    Private Sub btnRutaHasta_Click(sender As System.Object, e As System.EventArgs) Handles btnRutaHasta.Click
        txtRutaHasta.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codrut codigo, nomrut descripcion from jsvenencrut where tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Rutas Visita", _
                                          txtRutaHasta.Text)
    End Sub

    Private Sub txtRutaDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtRutaDesde.TextChanged
        txtRutaHasta.Text = txtRutaDesde.Text
    End Sub

    Private Sub btnAsesorDesde_Click(sender As System.Object, e As System.EventArgs) Handles btnAsesorDesde.Click
        txtAsesorDesde.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codven codigo, concat(apellidos, ', ',nombres) descripcion from jsvencatven where tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", " Comerciales", _
                                          txtAsesorDesde.Text)
    End Sub

    Private Sub btnAsesorHasta_Click(sender As System.Object, e As System.EventArgs) Handles btnAsesorHasta.Click
        txtAsesorHasta.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codven codigo, concat(apellidos, ', ',nombres) descripcion from jsvencatven where tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", " Comerciales", _
                                          txtAsesorHasta.Text)
    End Sub

    Private Sub txtAsesorDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtAsesorDesde.TextChanged
        txtAsesorHasta.Text = txtAsesorDesde.Text
    End Sub

    Private Sub txtConstante_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtConstante.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

   
End Class