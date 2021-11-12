Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsGenProNumerosControl

    Private Const sModulo As String = "Asignación y/o modificación de números de control"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dtNumerosControl As New DataTable
    Private ft As New Transportables

    Private nTablaNumerosControl As String = "tblNumerosControl"

    Private strSQLNumerosControl As String = ""

    Private m_SortingColumn As ColumnHeader

    '///////////////////////////////////////////////
    Private numGestion As Gestion
    Private TablaAleatoria As String = ""
    Private Origen As String = ""
    Public Sub Cargar(ByVal MyCon As MySqlConnection, GestionNum As Integer)

        numGestion = GestionNum
        MyConn = MyCon

        Dim dates As SfDateTimeEdit() = {txtDesde, txtHasta}
        SetSizeDateObjects(dates)

        IniciarTXT()
        Me.Show()

    End Sub
    Private Sub IniciarTXT()

        Select Case numGestion
            Case Gestion.iCompras
                lbl.Text = "NUMEROS DE CONTROL COMPRAS, GASTOS, CREDITOS Y DEBITOS"
                Origen = "COM"
            Case Gestion.iVentas
                lbl.Text = "NUMEROS DE CONTROL FACTURAS VENTAS, CREDITOS Y DEBITOS"
                Origen = "FAC"
            Case Gestion.iPuntosdeVentas
                lbl.Text = "NUMEROS DE CONTROL FACTURAS PUNTOS DE VENTA"
                Origen = "PVE"
        End Select

        txtDesde.Value = PrimerDiaMes(jytsistema.sFechadeTrabajo)
        txtHasta.Value = jytsistema.sFechadeTrabajo

        CrearTablaAleatoria()
        RellenaTablaAleatoria()

        Dim aCampos() As String = {"numdoc", "num_control", "prov_cli", "emision", "org", ""}
        Dim aNombres() As String = {"Documento", "N° de control", IIf(numGestion = Gestion.iCompras, "Proveedor", "Cliente"), "Fecha emisión", "Origen", ""}
        Dim aAnchos() As Integer = {120, 150, 120, 90, 50, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda,
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {"", "", "", sFormatoFechaCorta, "", ""}

        IniciarTabla(dg, dtNumerosControl, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, , True)

        dg.ReadOnly = False
        dg.Columns("numdoc").ReadOnly = True
        dg.Columns("prov_cli").ReadOnly = True
        dg.Columns("emision").ReadOnly = True
        dg.Columns("org").ReadOnly = True
        dg.Columns("").ReadOnly = True


    End Sub

    Private Sub jsGenProNumerosControl_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsGenProNumerosControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo

    End Sub



    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        dtNumerosControl.Dispose()
        dtNumerosControl = Nothing
        ft.Ejecutar_strSQL(MyConn, " drop temporary table " & TablaAleatoria)
        Me.Close()
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then

            ActualizarNumerosDeControl()

            ft.mensajeInformativo("Actualización de Números de Control actualizados...")
            RellenaTablaAleatoria()
            'Me.Close()

        End If

    End Sub
    Private Sub CrearTablaAleatoria()

        TablaAleatoria = "tbl" & ft.NumeroAleatorio(10000)
        ft.Ejecutar_strSQL(MyConn, " drop table if exists  " & TablaAleatoria)
        Dim aCampos() As String = {"numdoc.cadena.15.0", "num_control.cadena.30.0", "prov_cli.cadena.20.0", "emision.fecha.0.0", "org.cadena.3.0",
                                   "origen.cadena.3.0", "id_emp.cadena.2.0"}

        CrearTabla(MyConn, lblInfo, jytsistema.WorkDataBase, True, TablaAleatoria, aCampos, " numdoc, num_control, prov_cli, emision, id_emp ")

    End Sub

    Private Sub RellenaTablaAleatoria()

        ft.Ejecutar_strSQL(MyConn, " delete from " & TablaAleatoria & " ")

        Select Case numGestion
            Case Gestion.iCompras
                SeleccionComprasControl(TablaAleatoria, CDate(txtDesde.Text), CDate(txtHasta.Text), 2)
            Case Gestion.iVentas
                SeleccionVentasControl(TablaAleatoria, CDate(txtDesde.Text), CDate(txtHasta.Text))
            Case Gestion.iPuntosdeVentas
                SeleccionVentasControlPVE(TablaAleatoria, CDate(txtDesde.Text), CDate(txtHasta.Text))
        End Select

        ds = DataSetRequery(ds, "select * from " & TablaAleatoria & " order by 4,2,1 ", MyConn, nTablaNumerosControl, lblInfo)
        dtNumerosControl = ds.Tables(nTablaNumerosControl)

    End Sub
    Private Sub SeleccionVentasControlPVE(NombreTabla As String, FechaInicio As Date, FechaHasta As Date)

        Dim strSQlFacturas As String

        strSQlFacturas = "select a.numfac, if( e.num_control is null, a.numfac, e.num_control) num_control , a.codcli prov_cli, a.emision,  'PVE' org, 'PVE' origen, a.id_emp  FROM jsvenencpos a " _
            & " left join jsconnumcon e on (a.numfac = e.numdoc and a.id_emp = e.id_emp and e.org = 'PVE' and e.origen = 'PVE') " _
            & " Where " _
            & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
            & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' "

        ft.Ejecutar_strSQL(MyConn, " insert into  " & NombreTabla & " " _
            & strSQlFacturas)

    End Sub

    Private Sub SeleccionVentasControl(NombreTabla As String, FechaInicio As Date, FechaHasta As Date)

        Dim strSQlFacturas As String
        Dim strSQlFacturasPVE As String
        Dim strSQLNotasCredito As String
        Dim strSQLNotasDebito As String
        Dim strSQLNulas As String

        strSQlFacturas = "select a.numfac, IF( ISNULL(e.num_control), '',  " _
            & " IF( LOCATE('-', e.num_control ) = 0, " _
            & " CONCAT( SUBSTRING( e.num_control,1,2), '-', SUBSTRING(e.num_control, 3, LENGTH( e.num_control ) ) ) , " _
            & " e.num_control ) ) numcontrol , a.codcli prov_cli, a.emision, 'FAC' org, 'FAC' origen, a.id_emp " _
            & " FROM jsvenencfac a " _
            & " left join jsconnumcon e on (a.numfac = e.numdoc and a.id_emp = e.id_emp and e.org = 'FAC' and e.origen = 'FAC') " _
            & " Where " _
            & " a.tipo = 1 and " _
            & " substring(a.numfac,1,3) <> 'TMP' and " _
            & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
            & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' "

        strSQLNotasCredito = " SELECT a.NUMNCR, IF( ISNULL(e.num_control), '',  " _
            & " IF( LOCATE('-', e.num_control ) = 0, " _
            & " CONCAT( SUBSTRING( e.num_control,1,2), '-', SUBSTRING(e.num_control, 3, LENGTH( e.num_control ) ) ) , " _
            & " e.num_control ) ) numcontrol , a.codcli prov_cli, a.emision, 'NCR' org, 'FAC' origen, a.id_emp  FROM jsvenencncr a " _
            & " left join jsconnumcon e on (a.numncr = e.numdoc and a.id_emp = e.id_emp and e.org = 'NCR' and e.origen = 'FAC') " _
            & " Where " _
            & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
            & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' "

        strSQLNotasDebito = " SELECT a.NUMNDB, IF( ISNULL(e.num_control), '',  " _
            & " IF( LOCATE('-', e.num_control ) = 0, " _
            & " CONCAT( SUBSTRING( e.num_control,1,2), '-', SUBSTRING(e.num_control, 3, LENGTH( e.num_control ) ) ) , " _
            & " e.num_control ) ) numcontrol , a.codcli prov_cli, a.emision, 'NDB' org, 'FAC' origen, a.id_emp  FROM jsvenencndb a " _
            & " left join jsconnumcon e on (a.numndb = e.numdoc and a.id_emp = e.id_emp and e.org = 'NDB' and e.origen = 'FAC') " _
            & " Where " _
            & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
            & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "'"

        strSQLNulas = " select numdoc, num_control, prov_cli, emision, org, origen, id_emp  " _
            & " from jsconnumcon a WHERE " _
            & " a.org = 'CON' and origen = 'FAC'  and " _
            & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
            & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "'"

        strSQlFacturasPVE = "select a.numfac, IF( ISNULL(e.num_control), a.numfac,  " _
           & " IF( LOCATE('-', e.num_control ) = 0, " _
           & " CONCAT( SUBSTRING( e.num_control,1,2), '-', SUBSTRING(e.num_control, 3, LENGTH( e.num_control ) ) ) , " _
           & " e.num_control ) ) numcontrol , a.codcli prov_cli, a.emision,  'PVE' org, 'PVE' origen, a.id_emp  FROM jsvenencpos a " _
           & " left join jsconnumcon e on (a.numfac = e.numdoc and a.id_emp = e.id_emp and e.org = 'PVE' and e.origen = 'PVE') " _
           & " Where " _
           & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
           & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
           & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
           & " a.ID_EMP = '" & jytsistema.WorkID & "' "

        ft.Ejecutar_strSQL(MyConn, " replace into " & NombreTabla & " " _
            & strSQlFacturas)
        ft.Ejecutar_strSQL(MyConn, " replace into " & NombreTabla & " " _
            & strSQLNotasCredito)
        ft.Ejecutar_strSQL(MyConn, " replace into " & NombreTabla & " " _
            & strSQLNotasDebito)
        ft.Ejecutar_strSQL(MyConn, " replace into " & NombreTabla & " " _
            & strSQLNulas)
        ft.Ejecutar_strSQL(MyConn, " replace into " & NombreTabla & " " _
            & strSQlFacturasPVE)

    End Sub


    Private Sub SeleccionComprasControl(NombreTabla As String, FechaInicio As Date, FechaHasta As Date, Tipo As Integer)

        Dim strSQlFacturas As String
        Dim strSQLGastos As String
        Dim strSQLNotasCredito As String
        Dim strSQLNotasDebito As String

        strSQlFacturas = "SELECT a.NUMCOM, if( isnull( e.num_control), '', e.num_control) num_control , a.codpro prov_cli, a.emision, 'COM' org, 'COM' origen, a.id_emp FROM jsproenccom a " _
                & " left join jsconnumcon e on (a.numcom = e.numdoc  AND a.codpro = e.prov_cli and a.id_emp = e.id_emp and e.org = 'COM' and e.origen = 'COM') " _
                & " Where " _
                & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
                & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
                & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "' "

        strSQLGastos = "select a.numgas, if( isnull( e.num_control), '', e.num_control) num_control, a.codpro prov_cli, a.emision, 'GAS' org, 'COM' origen, a.id_emp   FROM jsproencgas a " _
                & " left join jsconnumcon e on (a.numgas = e.numdoc  AND a.codpro = e.prov_cli and a.id_emp = e.id_emp and e.org = 'GAS' and e.origen = 'COM') " _
                & " Where " _
                & " FIND_IN_SET(a.otra_cxp,'COM,FAC,') > 0 and " _
                & " a.emision >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
                & " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' "

        'strSQLGastosi = "select a.numgas, if( isnull( e.num_control), '', e.num_control) num_control, a.codpro prov_cli, a.emision, 'GAS' org, 'FAC' origen, a.id_emp FROM jsproencgas a " _
        '        & " left join jsconnumcon e on (a.numgas = e.numdoc  AND a.codpro = e.prov_cli and a.id_emp = e.id_emp and e.org = 'GAS' and e.origen = 'FAC') " _
        '        & " Where " _
        '        & " FIND_IN_SET(a.otra_cxp, 'FAC,') > 0 and " _
        '        & " a.emision >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
        '        & " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
        '        & " a.id_emp = '" & jytsistema.WorkID & "' "

        strSQLNotasCredito = " SELECT a.NUMNCR, if( isnull( e.num_control), '', e.num_control) num_control, a.codpro prov_cli, a.emision, 'NCR' org, 'COM' origen, a.id_emp   FROM jsproencncr a " _
                & " left join jsconnumcon e on (a.numncr = e.numdoc  AND a.codpro = e.prov_cli and a.id_emp = e.id_emp and e.org = 'NCR' and e.origen = 'COM') " _
                & " Where " _
                & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
                & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
                & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "' "

        strSQLNotasDebito = " SELECT a.NUMNDB, IF( e.num_control IS NULL, '', e.num_control) num_control, a.codpro prov_cli, a.emision, 'NDB' org, 'COM' origen, a.id_emp   FROM jsproencndb a " _
                & " left join jsconnumcon e on (a.numndb = e.numdoc AND a.codpro = e.prov_cli and a.id_emp = e.id_emp and e.org = 'NDB' and e.origen = 'COM') " _
                & " Where " _
                & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
                & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
                & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "'"


        If Tipo = 0 Then
            ft.Ejecutar_strSQL(MyConn, " insert into  " & NombreTabla & " " _
                & strSQlFacturas)
            ft.Ejecutar_strSQL(MyConn, " insert into  " & NombreTabla & " " _
                & strSQLNotasCredito)

            ft.Ejecutar_strSQL(MyConn, " insert into  " & NombreTabla & " " _
                & strSQLNotasDebito)

        ElseIf Tipo = 1 Then
            ft.Ejecutar_strSQL(MyConn, " insert into  " & NombreTabla & " " _
                & strSQLGastos)
            'ft.Ejecutar_strSQL ( myconn, " insert into  " & NombreTabla & " " _
            '    & strSQLGastosi)
        Else
            ft.Ejecutar_strSQL(MyConn, " insert into  " & NombreTabla & " " _
                & strSQlFacturas)
            ft.Ejecutar_strSQL(MyConn, " insert ignore into  " & NombreTabla & " " _
                & strSQLGastos)
            'ft.Ejecutar_strSQL ( myconn, " insert into  " & NombreTabla & " " _
            '    & strSQLGastosi)
            ft.Ejecutar_strSQL(MyConn, " insert into  " & NombreTabla & " " _
                & strSQLNotasCredito)
            ft.Ejecutar_strSQL(MyConn, " insert into  " & NombreTabla & " " _
                & strSQLNotasDebito)
        End If

    End Sub

    Private Sub ActualizarNumerosDeControl()
        Dim iCont As Integer
        If dtNumerosControl.Rows.Count > 0 Then
            For iCont = 0 To dtNumerosControl.Rows.Count - 1
                With dtNumerosControl.Rows(iCont)

                    InsertEditCONTROLNumeroControl(MyConn, .Item("NUMDOC"), .Item("PROV_CLI"), .Item("NUM_CONTROL"), _
                                                   CDate(.Item("EMISION").ToString), .Item("ORIGEN"), .Item("ORG"))
                End With
            Next
        End If
    End Sub

    Private Sub dg_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
         Handles dg.CellEndEdit
        dg.Rows(e.RowIndex).ErrorText = String.Empty
    End Sub

    Private Sub dg_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellValidated
        If dg.CurrentCell.ColumnIndex = 1 Then
        End If
    End Sub

    Private Sub dg_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
           Handles dg.CellValidating

        Dim headerText As String =
            dg.Columns(e.ColumnIndex).HeaderText

        If Not headerText.Equals("N° de control") Then Return

        'If (String.IsNullOrEmpty(e.FormattedValue.ToString())) Then
        ' ft.MensajeCritico( "Debe indicar un número de control válido...")
        ' e.Cancel = True
        ' End If

        Dim afld() As String = {"num_control", "id_emp"}
        Dim aCam() As String = {Replace(e.FormattedValue.ToString(), "-", ""), jytsistema.WorkID}

        'If e.FormattedValue.ToString().Trim() <> "" And e.FormattedValue.ToString().Trim() <> "-" Then
        '    Dim Documento As String = dtNumerosControl.Rows(e.RowIndex).Item("numdoc")
        '    Dim DocBD As String = ft.Ejecutar_strSQL_DevuelveScalar(MyConn, lblInfo, " select numdoc from jsconnumcon where replace(num_control, '-','') = '" & Replace(e.FormattedValue.ToString(), "-", "") & "' and origen = '" & dtNumerosControl.Rows(e.RowIndex).Item("origen") & "' and id_emp = '" & jytsistema.WorkID & "' ")
        '    If Documento <> DocBD Then
        '        If ft.DevuelveScalarCadena(MyConn, " select replace(num_control,'-','') from jsconnumcon where  replace(num_control, '-','') = '" & Replace(e.FormattedValue.ToString(), "-", "") & "' and id_emp = '" & jytsistema.WorkID & "'  ") = Replace(e.FormattedValue.ToString(), "-", "") _
        '            And numGestion <> Gestion.iCompras Then
        '            ft.MensajeCritico( "Numero de control YA ha sido asignado a otro documento ...")
        '            e.Cancel = True
        '        End If
        '    End If
        'End If

    End Sub

    Private Sub btnGo_Click(sender As System.Object, e As System.EventArgs) Handles btnGo.Click
        If CDate(txtDesde.Text) <= CDate(txtHasta.Text) Then
            RellenaTablaAleatoria()
        Else
            ft.mensajeCritico("Fecha inicio período no válida...")
        End If
    End Sub
    Private Sub btnAgregaEquivale_Click(sender As System.Object, e As System.EventArgs) Handles btnAgregaEquivale.Click
        Dim f As New jsGenProNumerosControlMovimientos
        f.num_Control = ""
        f.Agregar(MyConn, ds, dtNumerosControl, Origen)
        If f.num_Control <> "" Then
            RellenaTablaAleatoria()
            Dim row As DataRow = dtNumerosControl.Select(" num_control = '" & f.num_Control & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
            Me.BindingContext(ds, nTablaNumerosControl).Position = dtNumerosControl.Rows.IndexOf(row)

            dg.CurrentCell = dg(0, dtNumerosControl.Rows.IndexOf(row))

        End If


    End Sub

    Private Sub btnEliminaEquivale_Click(sender As System.Object, e As System.EventArgs) Handles btnEliminaEquivale.Click

        If dtNumerosControl.Rows.Count > 0 Then
            Dim nPosicion As Long = Me.BindingContext(ds, nTablaNumerosControl).Position
            If dtNumerosControl.Rows(nPosicion).Item("org") = "CON" Then

                If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                    ft.Ejecutar_strSQL(MyConn, " delete from jsconnumcon where " _
                        & " numdoc = '" & dtNumerosControl.Rows(nPosicion).Item("numdoc") & "' and " _
                        & " org = 'CON' and " _
                        & " origen = '" & Origen & "' and " _
                        & " id_emp = '" & jytsistema.WorkID & "' ")

                    RellenaTablaAleatoria()

                    If dtNumerosControl.Rows.Count > 0 Then
                        If dtNumerosControl.Rows.Count >= nPosicion Then
                            Me.BindingContext(ds, nTablaNumerosControl).Position = nPosicion
                        Else
                            Me.BindingContext(ds, nTablaNumerosControl).Position = dtNumerosControl.Rows.Count - 1
                        End If
                    End If
                End If
            Else
                ft.mensajeCritico("Numero de control no ELIMINABLE ")
            End If
        End If
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, dg.CellMouseClick
        Me.BindingContext(ds, nTablaNumerosControl).Position = e.RowIndex
    End Sub

    Private Sub txtDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtDesde.ValueChanged
        txtHasta.Value = txtDesde.Value
    End Sub
End Class