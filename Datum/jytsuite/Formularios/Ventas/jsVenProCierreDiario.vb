Imports MySql.Data.MySqlClient
Public Class jsVenProCierreDiario
    Private Const sModulo As String = "Cierre diario ventas"

    Private Const nTabla As String = "tblAsesores"

    Private strSQLAsesores As String = " select * from jsvencatven where tipo = " & TipoVendedor.iFuerzaventa & " and " _
                                       & " estatus = 1 and " _
                                       & " id_emp = '" & jytsistema.WorkID & "' order by clase, codven "

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private tipoCargaForm As TipoCargaFormulario
    Public Sub Cargar(ByVal MyCon As MySqlConnection, TipoCarga As Integer, FechaProceso As Date)

        myConn = MyCon
        Me.Tag = sModulo
        tipoCargaForm = TipoCarga
        lblLeyenda.Text = " Mediante este proceso se hace el cierre diario, el cual produce el resumen diario de las ventas " + vbCr + _
                " en la fuerza de venta. " + vbCr + _
                " - Las estadísticas son un resumen mensual de ventas, por lo tanto se generará un registro diario " + vbCr + _
                "   por cada ítem : Mercancías, Marcas, Categorías, Divisiones, Jerarquías, Cobranza Anterior y " + vbCr + _
                "   Cobranza Actual "

        txtFechaProceso.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        ft.habilitarObjetos(False, True, txtFechaProceso)
        ft.mensajeEtiqueta(lblInfo, " ... ", Transportables.TipoMensaje.iAyuda)

        chkSeleccionar.Checked = True

        If tipoCargaForm = TipoCargaFormulario.iShow Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub jsControlProVerificarBD_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsControlProVerificarBD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'If tipoCargaForm = TipoCargaFormulario.iShowDialog Then
        '    'For Each cont As Control In Me.Controls
        '    '    cont.Enabled = False
        '    'Next
        '    Procesar()
        '    Me.Close()
        'End If

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Procesar()
    End Sub
    Private Sub Procesar()

        DeshabilitarCursorEnEspera()


        ds = DataSetRequery(ds, strSQLAsesores, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        For Each nRow As DataRow In dt.Rows
            With nRow
                If chk1.Checked Then ProcesarMercancias(.Item("codven"), .Item("clase"))
                'If chk2.Checked Then ProcesarCategorias(.Item("codven"), .Item("clase"))
                'If chk3.Checked Then ProcesarMarcas(.Item("codven"), .Item("clase"))
                'If chk5.Checked Then ProcesarDivisiones(.Item("codven"), .Item("clase"))
                'If chk4.Checked Then ProcesarJerarquías(.Item("codven"), .Item("clase"))
                If chk6.Checked Then ProcesarCobranzaAnteriorPLUS(.Item("codven"), .Item("clase"))
                If chk7.Checked Then ProcesarCobranzaActual(.Item("codven"), .Item("clase"))
                If chkCierre.Checked Then
                    Select Case .Item("clase")
                        Case 0 'Vendedores
                            ProcesarCierre(.Item("codven"))
                        Case 1, 2 'Supervisores
                            ProcesarCierreSupervisor(.Item("codven"), .Item("clase"))
                    End Select
                    ProcesarVentasCliente(.Item("CODVEN"), .Item("CLASE"))
                End If
            End With

        Next

        HabilitarCursorEnEspera()
        ft.mensajeInformativo(" Proceso culminado con éxito... ")
        ProgressBar1.Value = 0
        lblProgreso.Text = ""


    End Sub
    Private Sub ProcesarCierreSupervisor(CodigoSupervisor As String, Clase As Integer)


        Dim strAsesores As String = AsesoresDeSupervisor(myConn, lblInfo, CodigoSupervisor, Clase)

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " Supervisor : " & CodigoSupervisor & " - CIERRE / COSTOS DE LAS VENTAS ")

        Dim nCostos As Double = ft.DevuelveScalarDoble(myConn, " SELECT SUM( IF(a.origen = 'NCV', -1, 1) * a.costotaldes ) " _
                                                      & " FROM jsmertramer a " _
                                                      & " WHERE " _
                                                      & " a.origen IN ('FAC', 'PFC', 'PVE', 'NDV', 'NCV') AND " _
                                                      & " a.vendedor IN " & strAsesores & " AND " _
                                                      & " MONTH(a.fechamov) =  " & CDate(txtFechaProceso.Text).Month & " AND  " _
                                                      & " YEAR(a.fechamov) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                      & " a.id_emp = '" & jytsistema.WorkID & "' ")

        Dim nPesos As Double = ft.DevuelveScalarDoble(myConn, " SELECT SUM( IF(a.origen = 'NCV', -1, 1) * a.peso ) " _
                                                      & " FROM jsmertramer a " _
                                                      & " WHERE " _
                                                      & " a.origen IN ('FAC', 'PFC', 'PVE', 'NDV', 'NCV') AND " _
                                                      & " a.vendedor IN " & strAsesores & " AND " _
                                                      & " MONTH(a.fechamov) =  " & CDate(txtFechaProceso.Text).Month & " AND  " _
                                                      & " YEAR(a.fechamov) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                      & " a.id_emp = '" & jytsistema.WorkID & "' ")

        Dim nCantidad As Double = ft.DevuelveScalarDoble(myConn, " SELECT SUM( IF(a.origen = 'NCV', -1, 1) * if( b.uvalencia is null, a.cantidad,  a.cantidad/b.equivale )  ) " _
                                                      & " FROM jsmertramer a " _
                                                      & " left join jsmerequmer b on (a.codart = b.codart and a.unidad = b.uvalencia and a.id_emp = b.id_emp ) " _
                                                      & " WHERE " _
                                                      & " a.origen IN ('FAC', 'PFC', 'PVE', 'NDV', 'NCV') AND " _
                                                      & " a.vendedor IN " & strAsesores & " AND " _
                                                      & " MONTH(a.fechamov) =  " & CDate(txtFechaProceso.Text).Month & " AND  " _
                                                      & " YEAR(a.fechamov) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                      & " a.id_emp = '" & jytsistema.WorkID & "' group by a.vendedor ")


        Dim aVisitar As Integer = ft.DevuelveScalarEntero(myConn, " SELECT SUM(AVISITAR) FROM jsvenenccie where  " _
                                                      & " CODVEN IN " & strAsesores & " AND " _
                                                      & " MONTH(FECHA) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                                                      & " YEAR(FECHA) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                      & " ID_EMP = '" & jytsistema.WorkID & "' ")

        Dim nVisitados As Integer = ft.DevuelveScalarEntero(myConn, " SELECT SUM(VISITADOS) FROM jsvenenccie where  " _
                                                      & " CODVEN IN " & strAsesores & " AND " _
                                                      & " MONTH(FECHA) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                                                      & " YEAR(FECHA) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                      & " ID_EMP = '" & jytsistema.WorkID & "' ")


        Dim nEfectividad As Double = 0
        If aVisitar > 0 Then nEfectividad = nVisitados / aVisitar * 100


        Dim dMontoCD As Double = ft.DevuelveScalarDoble(myConn, " SELECT SUM( IF( SUBSTRING(nummov,1,2) = 'CD' ,importe, 0 ) ) " _
                                                           & " FROM jsventracob " _
                                                           & " WHERE " _
                                                           & " tipomov = 'ND' AND " _
                                                           & " MONTH(emision) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                                                           & " YEAR(emision) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                           & " origen = 'BAN' AND " _
                                                           & " codven IN " & strAsesores & " AND " _
                                                           & " id_emp = '" & jytsistema.WorkID & "' ")

        Dim dMontoCimisionCD As Double = ft.DevuelveScalarDoble(myConn, " SELECT SUM( IF( SUBSTRING(nummov,1,2) = 'ND' ,importe, 0 ) ) " _
                                                           & " FROM jsventracob " _
                                                           & " WHERE " _
                                                           & " tipomov = 'ND' AND " _
                                                           & " MONTH(emision) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                                                           & " YEAR(emision) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                           & " origen = 'BAN' AND " _
                                                           & " codven IN " & strAsesores & " AND " _
                                                           & " id_emp = '" & jytsistema.WorkID & "' ")

        Dim dCantidadCD As Integer = ft.DevuelveScalarEntero(myConn, " SELECT SUM( IF( SUBSTRING(nummov,1,2) = 'CD' , 1 , 0 ) ) " _
                                                           & " FROM jsventracob " _
                                                           & " WHERE " _
                                                           & " tipomov = 'ND' AND " _
                                                           & " MONTH(emision) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                                                           & " YEAR(emision) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                           & " origen = 'BAN' AND " _
                                                           & " codven IN " & strAsesores & " AND " _
                                                           & " id_emp = '" & jytsistema.WorkID & "'  ")



        If ft.DevuelveScalarEntero(myConn, " select COUNT(*) from jsvenenccie where " _
                                      & " CODVEN = '" & CodigoSupervisor & "' AND " _
                                      & " month(fecha) = " & CDate(txtFechaProceso.Text).Month & " and " _
                                      & " YEAR(FECHA) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                      & " id_emp = '" & jytsistema.WorkID & "' ") > 0 Then
            'EXISTE
            ft.Ejecutar_strSQL(myConn, " UPDATE jsvenenccie set FECHA = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "',  " _
                                    & " AVISITAR = " & aVisitar & ", " _
                                    & " VISITADOS = " & nVisitados & ", " _
                                    & " EFECTIVIDAD = " & nEfectividad & ", " _
                                    & " ITEMS_CAJ = " & nCantidad & ", " _
                                    & " ITEMS_KGS = " & nPesos & ", " _
                                    & " COSTOSVENTAS = " & nCostos & ", " _
                                    & " MONTO_CD = " & dMontoCD & ", " _
                                    & " MONTO_COM_CD = " & dMontoCimisionCD & ", " _
                                    & " CANTIDAD_CD = " & dCantidadCD & " " _
                                    & " WHERE " _
                                    & " CODVEN = '" & CodigoSupervisor & "' AND " _
                                    & " MONTH(FECHA) = " & CDate(txtFechaProceso.Text).Month & " and " _
                                    & " YEAR(FECHA) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                    & " ID_EMP = '" & jytsistema.WorkID & "' ")
        Else

            ft.Ejecutar_strSQL(myConn, " INSERT INTO jsvenenccie " _
                        & " set FECHA = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                        & " AVISITAR = " & aVisitar & ", " _
                        & " VISITADOS = " & nVisitados & ", " _
                        & " EFECTIVIDAD = " & nEfectividad & ", " _
                        & " ITEMS_CAJ = " & nCantidad & ", " _
                        & " ITEMS_KGS = " & nPesos & ", " _
                        & " COSTOSVENTAS = " & nCostos & ", " _
                        & " CODVEN = '" & CodigoSupervisor & "', " _
                        & " MONTO_CD = " & dMontoCD & ", " _
                        & " MONTO_COM_CD = " & dMontoCimisionCD & ", " _
                        & " CANTIDAD_CD = " & dCantidadCD & ", " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' ")

        End If

    End Sub

    Private Sub ProcesarCierre(CodigoAsesor As String)

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " Asesor : " & CodigoAsesor & " - COSTOS DE LAS VENTAS ")

        Dim nCostos As Double = ft.DevuelveScalarDoble(myConn, " SELECT SUM( IF(a.origen = 'NCV', -1, 1) * a.costotaldes ) " _
                                                      & " FROM jsmertramer a " _
                                                      & " WHERE " _
                                                      & " a.origen IN ('FAC', 'PFC', 'PVE', 'NDV', 'NCV') AND " _
                                                      & " a.vendedor = '" & CodigoAsesor & "' AND " _
                                                      & " MONTH(a.fechamov) =  " & CDate(txtFechaProceso.Text).Month & " AND  " _
                                                      & " YEAR(a.fechamov) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                      & " a.id_emp = '" & jytsistema.WorkID & "' group by a.vendedor ")

        Dim nPesos As Double = ft.DevuelveScalarDoble(myConn, " SELECT SUM( IF(a.origen = 'NCV', -1, 1) * a.peso ) " _
                                                      & " FROM jsmertramer a " _
                                                      & " WHERE " _
                                                      & " a.origen IN ('FAC', 'PFC', 'PVE', 'NDV', 'NCV') AND " _
                                                      & " a.vendedor = '" & CodigoAsesor & "' AND " _
                                                      & " MONTH(a.fechamov) =  " & CDate(txtFechaProceso.Text).Month & " AND  " _
                                                      & " YEAR(a.fechamov) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                      & " a.id_emp = '" & jytsistema.WorkID & "' group by a.vendedor ")

        Dim nCantidad As Double = ft.DevuelveScalarDoble(myConn, " SELECT SUM( IF(a.origen = 'NCV', -1, 1) * if( b.uvalencia is null, a.cantidad,  a.cantidad/b.equivale )  ) " _
                                                      & " FROM jsmertramer a " _
                                                      & " left join jsmerequmer b on (a.codart = b.codart and a.unidad = b.uvalencia and a.id_emp = b.id_emp ) " _
                                                      & " WHERE " _
                                                      & " a.origen IN ('FAC', 'PFC', 'PVE', 'NDV', 'NCV') AND " _
                                                      & " a.vendedor = '" & CodigoAsesor & "' AND " _
                                                      & " MONTH(a.fechamov) =  " & CDate(txtFechaProceso.Text).Month & " AND  " _
                                                      & " YEAR(a.fechamov) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                      & " a.id_emp = '" & jytsistema.WorkID & "' group by a.vendedor ")


        Dim aVisitar As Integer = NumeroDeClientesAVisitarMes(myConn, lblInfo, ds, CodigoAsesor, PrimerDiaMes(CDate(txtFechaProceso.Text)), CDate(txtFechaProceso.Text))
        Dim nVisitados As Integer = ft.DevuelveScalarEntero(myConn, " select count(*) from jsvenencpedrgv where codven = '" & CodigoAsesor & "' and emision >= '" _
                                                          & ft.FormatoFechaMySQL(PrimerDiaMes(CDate(txtFechaProceso.Text))) & "' and emision <= '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "' and id_emp = '" & jytsistema.WorkID & "'  ")

        Dim nEfectividad As Double = 0
        If aVisitar > 0 Then nEfectividad = nVisitados / aVisitar * 100


        Dim dMontoCD As Double = ft.DevuelveScalarDoble(myConn, " SELECT SUM( IF( SUBSTRING(nummov,1,2) = 'CD' ,importe, 0 ) ) " _
                                                           & " FROM jsventracob " _
                                                           & " WHERE " _
                                                           & " tipomov = 'ND' AND " _
                                                           & " MONTH(emision) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                                                           & " YEAR(emision) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                           & " origen = 'BAN' AND " _
                                                           & " codven = '" & CodigoAsesor & "' AND " _
                                                           & " id_emp = '" & jytsistema.WorkID & "' GROUP BY ID_EMP ")

        Dim dMontoCimisionCD As Double = ft.DevuelveScalarDoble(myConn, " SELECT SUM( IF( SUBSTRING(nummov,1,2) = 'ND' ,importe, 0 ) ) " _
                                                           & " FROM jsventracob " _
                                                           & " WHERE " _
                                                           & " tipomov = 'ND' AND " _
                                                           & " MONTH(emision) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                                                           & " YEAR(emision) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                           & " origen = 'BAN' AND " _
                                                           & " codven = '" & CodigoAsesor & "' AND " _
                                                           & " id_emp = '" & jytsistema.WorkID & "' GROUP BY ID_EMP ")

        Dim dCantidadCD As Integer = ft.DevuelveScalarEntero(myConn, " SELECT SUM( IF( SUBSTRING(nummov,1,2) = 'CD' , 1 , 0 ) ) " _
                                                           & " FROM jsventracob " _
                                                           & " WHERE " _
                                                           & " tipomov = 'ND' AND " _
                                                           & " MONTH(emision) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                                                           & " YEAR(emision) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                           & " origen = 'BAN' AND " _
                                                           & " codven = '" & CodigoAsesor & "' AND " _
                                                           & " id_emp = '" & jytsistema.WorkID & "' GROUP BY ID_EMP ")


        If ft.DevuelveScalarEntero(myConn, " select count(*) from jsvenenccie where " _
                                      & " CODVEN = '" & CodigoAsesor & "' AND " _
                                      & " month(fecha) = " & CDate(txtFechaProceso.Text).Month & " and " _
                                      & " YEAR(FECHA) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                      & " id_emp = '" & jytsistema.WorkID & "' ") > 0 Then
            'EXISTE
            ft.Ejecutar_strSQL(myConn, " UPDATE jsvenenccie set FECHA = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "',  " _
                                       & " AVISITAR = " & aVisitar & ", " _
                                       & " VISITADOS = " & nVisitados & ", " _
                                       & " EFECTIVIDAD = " & nEfectividad & ", " _
                                       & " ITEMS_CAJ = " & nCantidad & ", " _
                                       & " ITEMS_KGS = " & nPesos & ", " _
                                       & " COSTOSVENTAS = " & nCostos & ", " _
                                       & " MONTO_CD = " & dMontoCD & ", " _
                                       & " MONTO_COM_CD = " & dMontoCimisionCD & ", " _
                                       & " CANTIDAD_CD = " & dCantidadCD & " " _
                                       & " WHERE " _
                                       & " CODVEN = '" & CodigoAsesor & "' AND " _
                                       & " MONTH(FECHA) = " & CDate(txtFechaProceso.Text).Month & " and " _
                                       & " YEAR(FECHA) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                       & " ID_EMP = '" & jytsistema.WorkID & "' ")
        Else

            ft.Ejecutar_strSQL(myConn, " INSERT INTO jsvenenccie " _
                        & " set FECHA = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                        & " AVISITAR = " & aVisitar & ", " _
                        & " VISITADOS = " & nVisitados & ", " _
                        & " EFECTIVIDAD = " & nEfectividad & ", " _
                        & " COSTOSVENTAS = " & nCostos & ", " _
                        & " ITEMS_CAJ = " & nCantidad & ", " _
                        & " ITEMS_KGS = " & nPesos & ", " _
                        & " CODVEN = '" & CodigoAsesor & "', " _
                        & " MONTO_CD = " & dMontoCD & ", " _
                        & " MONTO_COM_CD = " & dMontoCimisionCD & ", " _
                        & " CANTIDAD_CD = " & dCantidadCD & ", " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' ")

        End If

    End Sub
    Private Sub ProcesarVentasCliente(CodigoAsesor As String, Clase As Integer)

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, "PROCESANDO VENTAS CLIENTES ...")

        Dim dtClientes As DataTable
        Dim nTablacLIENTES As String = "tblClientes"

        Dim strAsesores As String = IIf(Clase = 0, "('" & CodigoAsesor & "')", AsesoresDeSupervisor(myConn, lblInfo, CodigoAsesor, Clase))


        Dim strSQLClientes As String = " SELECT a.vendedor, a.fechamov, a.prov_cli cliente, b.nombre, " _
                                       & " SUM( IF( a.origen = 'NCV', -1, 1) * IF( c.uvalencia IS NULL, a.cantidad, a.cantidad / c.equivale ) ) CAJAS, " _
                                       & " SUM( IF( a.origen = 'NCV', -1, 1) * a.peso ) KILOS, " _
                                       & " SUM( IF( a.origen = 'NCV', -1, 1) * a.costotaldes  ) COSTOS, " _
                                       & " SUM( IF( a.origen = 'NCV', -1, 1) * a.ventotaldes ) VENTAS " _
                                       & " FROM jsmertramer a " _
                                       & " LEFT JOIN jsvencatcli b ON (a.prov_cli = b.codcli AND a.id_emp = b.id_emp) " _
                                       & " LEFT JOIN jsmerequmer c ON (a.codart = c.codart AND a.unidad = c.uvalencia AND a.id_emp = c.id_emp ) " _
                                       & " WHERE " _
                                       & " a.origen IN ('FAC', 'PVE', 'PFC', 'NCV', 'NDV') AND " _
                                       & " a.vendedor IN " & strAsesores & " AND " _
                                       & " MONTH(a.fechamov) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                                       & " YEAR(a.fechamov) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                       & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                       & " GROUP BY a.prov_cli "

        ds = DataSetRequery(ds, strSQLClientes, myConn, nTablaCLIENTES, lblInfo)
        dtClientes = ds.Tables(nTablacLIENTES)

        If dtClientes.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtClientes.Rows.Count - 1
                With dtClientes.Rows(iCont)

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (iCont + 1) / dtClientes.Rows.Count * 100, " Asesor : " & CodigoAsesor & " - Cliente : " & .Item("cliente") & " " & .Item("nombre"))

                    Dim pesoPedidosCliente As Double = ft.DevuelveScalarDoble(myConn, " SELECT SUM(a.kilos) " _
                                                                             & " FROM jsvenencped a " _
                                                                             & " WHERE " _
                                                                             & " MONTH(a.emision ) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                                                                             & " YEAR(a.emision) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                                             & " a.codven IN " & strAsesores & " AND " _
                                                                             & " a.codcli = '" & .Item("CLIENTE") & "' and " _
                                                                             & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                                                             & " GROUP BY a.codcli")

                    If ft.DevuelveScalarEntero(myConn, " select count(*) from jsvenrencie " _
                                           & " where " _
                                           & " CODVEN = '" & CodigoAsesor & "' and " _
                                           & " MONTH(FECHA) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                                           & " YEAR(FECHA) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                           & " CLIENTE = '" & .Item("CLIENTE") & "' AND " _
                                           & " ID_EMP = '" & jytsistema.WorkID & "' ") > 0 Then

                        ft.Ejecutar_strSQL(myConn, " UPDATE jsvenrencie set " _
                                        & " FECHA = '" & ft.FormatoFechaMySQL(CDate(.Item("FECHAMOV").ToString)) & "' , " _
                                        & " CAJAS = " & .Item("CAJAS") & ", " _
                                        & " KILOS = " & .Item("KILOS") & ", " _
                                        & " COSTOS = " & .Item("COSTOS") & ", " _
                                        & " VENTAS = " & .Item("VENTAS") & ", " _
                                        & " TOTALPEDIDO = " & pesoPedidosCliente & ", " _
                                        & " TOTALDEV = " & IIf(pesoPedidosCliente > 0, Math.Round(.Item("KILOS") / pesoPedidosCliente * 100, 2), 100) & " " _
                                        & " WHERE " _
                                        & " CODVEN = '" & CodigoAsesor & "' AND " _
                                        & " MONTH(FECHA) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                                        & " YEAR(FECHA) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                        & " CLIENTE = '" & .Item("CLIENTE") & "' AND " _
                                        & " ID_EMP = '" & jytsistema.WorkID & "' ")

                    Else

                        ft.Ejecutar_strSQL(myConn, " INSERT INTO jsvenrencie set " _
                                        & " CODVEN = '" & CodigoAsesor & "', " _
                                        & " FECHA = '" & ft.FormatoFechaMySQL(CDate(.Item("FECHAMOV").ToString)) & "' , " _
                                        & " CLIENTE = '" & .Item("CLIENTE") & "', " _
                                        & " NOMBRE = '" & Replace(.Item("NOMBRE"), "'", "''") & "',  " _
                                        & " CAJAS = " & .Item("CAJAS") & ", " _
                                        & " KILOS = " & .Item("KILOS") & ", " _
                                        & " COSTOS = " & .Item("COSTOS") & ", " _
                                        & " VENTAS = " & .Item("VENTAS") & ", " _
                                        & " TOTALPEDIDO = " & pesoPedidosCliente & ", " _
                                        & " TOTALDEV = " & IIf(pesoPedidosCliente > 0, Math.Round(.Item("KILOS") / pesoPedidosCliente * 100, 2), 100) & ", " _
                                        & " ID_EMP = '" & jytsistema.WorkID & "' ")

                    End If



                End With
            Next
        End If



        dtClientes.Dispose()
        dtClientes = Nothing

    End Sub
    Private Sub ProcesarMercanciasPLUSXX(ByVal CodigoAsesor As String, Clase As Integer)

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, "PROCESANDO MERCANCÍAS...")

        Dim dtMercas As DataTable
        Dim nTablaMercas As String = "tblMercas"
        Dim nMes As String = String.Format("{0:00}", CDate(txtFechaProceso.Text).Month)
        Dim strAsesores As String = IIf(Clase = 0, "('" & CodigoAsesor & "')", AsesoresDeSupervisor(myConn, lblInfo, CodigoAsesor, Clase))

        Dim strSQLMercancias As String = " SELECT a.codart, a.nomart, " _
            & " c.esmes" & nMes & " cuota,  " _
            & " IF( b.acumulado IS NULL, 0.00, b.acumulado) acumulado, " _
            & " IF ( c.esmes" & nMes & " = 0, 100, IF( b.acumulado IS NULL, 0.00, b.acumulado)/c.esmes" & nMes & " *100)  logro, " _
            & " IF( DiasHabilesRestantesMes('" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "','" & jytsistema.WorkID & "') = 0, 0, " _
            & " (c.esmes" & nMes & " - IF( b.acumulado IS NULL, 0.00, b.acumulado))/DiasHabilesRestantesMes('" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "','" & jytsistema.WorkID & "') ) meta, " _
            & " (c.esmes" & nMes & " - IF( b.acumulado IS NULL, 0.00, b.acumulado)) cierre, if(b.activados is null, 0, b.activados) activados " _
            & " FROM jsmerctainv a " _
            & " LEFT JOIN (SELECT a.codart, a.vendedor, ROUND(SUM( IF( a.tipomov = 'SA', 1, -1) * a.peso ), 3) acumulado, COUNT( DISTINCT a.prov_cli ) activados  " _
            & "        		    FROM jsmertramer a " _
            & "                 WHERE " _
            & "            		a.origen IN ('FAC','PFC','PVE','NCV','NDV') AND " _
            & "            		MONTH(a.fechamov) = " & CDate(txtFechaProceso.Text).Month & " AND " _
            & "            		YEAR(a.fechamov) = " & CDate(txtFechaProceso.Text).Year & " AND " _
            & "            		a.vendedor IN " & strAsesores & " AND " _
            & "            		a.id_emp = '" & jytsistema.WorkID & "' " _
            & "            		GROUP BY a.codart) b ON (a.codart = b.codart) " _
            & " LEFT JOIN (SELECT * FROM jsvencuoart WHERE codven IN " & strAsesores & " and id_emp = '" & jytsistema.WorkID & "') c ON (a.codart = c.codart AND a.id_emp = c.id_emp ) " _
            & "         WHERE " _
            & " (NOT c.esmes" & nMes & "  IS NULL) AND " _
            & "  a.id_emp = '" & jytsistema.WorkID & "' "


        ds = DataSetRequery(ds, strSQLMercancias, myConn, nTablaMercas, lblInfo)
        dtMercas = ds.Tables(nTablaMercas)

        If dtMercas.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtMercas.Rows.Count - 1
                With dtMercas.Rows(iCont)

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (iCont + 1) / dtMercas.Rows.Count * 100, _
                                                  " Asesor : " & CodigoAsesor & " - Mercancía : " & .Item("codart") & " " & .Item("nomart"))

                    Dim aFld() As String = {"codven", "item", " month(fecha)", "year(fecha)", "tipo", "id_emp"}
                    Dim aStr() As String = {CodigoAsesor, .Item("codart"), CDate(txtFechaProceso.Text).Month, CDate(txtFechaProceso.Text).Year, "0", jytsistema.WorkID}

                    If qFound(myConn, lblInfo, "jsvenstaven", aFld, aStr) Then

                        ft.Ejecutar_strSQL(myconn, " update jsvenstaven set " _
                            & " codven = '" & CodigoAsesor & "',  " _
                            & " item = '" & .Item("codart") & "', fecha = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " cuota = " & .Item("cuota") & " , acumulado = " & .Item("acumulado") & ", " _
                            & " logro = " & .Item("logro") & ", meta = " & .Item("meta") & ", " _
                            & " activacion = " & .Item("activados") & ", cierre = " & .Item("cierre") & ", tipo = 0 , id_emp = '" & jytsistema.WorkID & "'" _
                            & " where " _
                            & " codven = '" & CodigoAsesor & "' and " _
                            & " item = '" & .Item("codart") & "' and " _
                            & " month(fecha) = '" & CDate(txtFechaProceso.Text).Month & "' and " _
                            & " year(fecha) = '" & CDate(txtFechaProceso.Text).Year & "' and " _
                            & " tipo = '0' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

                    Else

                        ft.Ejecutar_strSQL(myconn, " insert into  jsvenstaven set " _
                            & " codven = '" & CodigoAsesor & "',  " _
                            & " item = '" & .Item("codart") & "', fecha = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " cuota = " & .Item("cuota") & " , acumulado = " & .Item("acumulado") & ", " _
                            & " logro = " & .Item("logro") & ", meta = " & .Item("meta") & ", " _
                            & " activacion = " & .Item("activados") & ", cierre = " & .Item("cierre") & ", tipo = 0 , id_emp = '" & jytsistema.WorkID & "'")

                    End If

                End With
            Next
        End If

    End Sub


    Private Sub ProcesarMercancias(ByVal CodigoAsesor As String, Clase As Integer)

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, "PROCESANDO MERCANCÍAS...")

        Dim dtMercas As DataTable
        Dim nTablaMercas As String = "tblMercas"
        Dim nMes As String = String.Format("{0:00}", CDate(txtFechaProceso.Text).Month)
        Dim strAsesores As String = IIf(Clase = 0, "('" & CodigoAsesor & "')", AsesoresDeSupervisor(myConn, lblInfo, CodigoAsesor, Clase))


        Dim strSQLMercancias As String = " SELECT a.codart, a.nomart " _
                                        & " FROM jsmerctainv a " _
                                        & " WHERE " _
                                        & " a.id_emp = '" & jytsistema.WorkID & "' "

        Dim iDiasRestantes As Integer = DiasHabilesRestantesDelMes(myConn, CDate(txtFechaProceso.Text), 1)

        ds = DataSetRequery(ds, strSQLMercancias, myConn, nTablaMercas, lblInfo)
        dtMercas = ds.Tables(nTablaMercas)

        If dtMercas.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtMercas.Rows.Count - 1
                With dtMercas.Rows(iCont)
                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (iCont + 1) / dtMercas.Rows.Count * 100, _
                                                  " Asesor : " & CodigoAsesor & " - Mercancía : " & .Item("codart") & " " & .Item("nomart"))

                    Dim aFld() As String = {"codven", "item", " month(fecha)", "year(fecha)", "tipo", "id_emp"}
                    Dim aStr() As String = {CodigoAsesor, .Item("codart"), CDate(txtFechaProceso.Text).Month, CDate(txtFechaProceso.Text).Year, "0", jytsistema.WorkID}

                    Dim dCuota As Double = ft.DevuelveScalarDoble(myConn, " select esmes" & nMes & " from jsvencuoart where CODART = '" & .Item("CODART") & "' AND codven in " & strAsesores & " and id_emp = '" & jytsistema.WorkID & "' ")

                    Dim dAcumulado As Double = ft.DevuelveScalarDoble(myConn, " SELECT SUM( IF( a.tipomov = 'SA', 1, -1) * a.peso ) " _
                                                                                & " FROM jsmertramer a " _
                                                                                & " WHERE " _
                                                                                & " a.CODART = '" & .Item("CODART") & "' AND " _
                                                                                & " a.origen IN ('FAC','PFC','PVE','NCV','NDV') AND " _
                                                                                & " MONTH(a.fechamov) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                                                                                & " YEAR(a.fechamov) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                                                & " a.vendedor IN " & strAsesores & " AND " _
                                                                                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                                                                & " GROUP BY a.codart ")

                    Dim dActivados As Integer = 0  'ft.DevuelveScalarEntero(MyConn, " SELECT  COUNT( DISTINCT a.prov_cli ) activados " _
                    '                                                            & " FROM jsmertramer a " _
                    '                                                            & " WHERE " _
                    '                                                            & " a.CODART = '" & .Item("CODART") & "' AND " _
                    '                                                            & " a.origen IN ('FAC','PFC','PVE','NCV','NDV') AND " _
                    '                                                            & " MONTH(a.fechamov) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                    '                                                            & " YEAR(a.fechamov) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                    '                                                            & " a.vendedor IN " & strAsesores & " AND " _
                    '                                                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    '                                                            & " GROUP BY a.codart ") )

                    Dim dLogro As Double = IIf(dCuota = 0, 100, dAcumulado / dCuota * 100)
                    Dim dCierre As Double = dCuota - dAcumulado
                    Dim dMeta As Double = IIf(iDiasRestantes = 0, 0, dCierre / iDiasRestantes)

                    If qFound(myConn, lblInfo, "jsvenstaven", aFld, aStr) Then

                        ft.Ejecutar_strSQL(myconn, " update jsvenstaven set " _
                            & " codven = '" & CodigoAsesor & "',  " _
                            & " item = '" & .Item("codart") & "', fecha = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " cuota = " & dCuota & " , acumulado = " & dAcumulado & ", " _
                            & " logro = " & dLogro & ", meta = " & dMeta & ", " _
                            & " activacion = " & dActivados & ", cierre = " & dCierre & ", tipo = 0 , id_emp = '" & jytsistema.WorkID & "'" _
                            & " where " _
                            & " codven = '" & CodigoAsesor & "' and " _
                            & " item = '" & .Item("codart") & "' and " _
                            & " month(fecha) = '" & CDate(txtFechaProceso.Text).Month & "' and " _
                            & " year(fecha) = '" & CDate(txtFechaProceso.Text).Year & "' and " _
                            & " tipo = '0' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

                    Else

                        ft.Ejecutar_strSQL(myconn, " insert into  jsvenstaven set " _
                            & " codven = '" & CodigoAsesor & "',  " _
                            & " item = '" & .Item("codart") & "', fecha = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " cuota = " & dCuota & " , acumulado = " & dAcumulado & ", " _
                            & " logro = " & dLogro & ", meta = " & dMeta & ", " _
                            & " activacion = " & dActivados & ", cierre = " & dCierre & ", tipo = 0 , id_emp = '" & jytsistema.WorkID & "'")

                    End If

                End With
            Next
        End If

    End Sub



    Private Sub ProcesarCategorias(ByVal CodigoAsesor As String, Clase As Integer)
        Dim dtCategorias As DataTable
        Dim nTablaCategorias As String = "tblCategorias"
        Dim strAsesores As String = IIf(Clase = 0, "('" & CodigoAsesor & "')", AsesoresDeSupervisor(myConn, lblInfo, CodigoAsesor, Clase))

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, "PROCESANDO CATEGORÍAS...")

        Dim strSQLCategorias As String = " SELECT a.codigo item, a.descrip, if( SUM(b.cuota) is null, 0.00, sum(b.cuota)) cuota, " _
                            & " if( SUM(b.acumulado) is null, 0.00, sum(b.acumulado)) acumulado, " _
                            & " if( sum(b.cuota) > 0, SUM(b.acumulado)/SUM(b.cuota)*100, 0.00) logro, " _
                            & " ((if( SUM(b.cuota) is null, 0.00, sum(b.cuota)) - if(SUM(b.acumulado) is null, 0.00, sum(b.acumulado) ) ) / DiasHabilesRestantesMes('" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "','" & jytsistema.WorkID & "')) meta, 0 activacion, " _
                            & " (if( SUM(b.acumulado) is null, 0.00, sum(b.acumulado)) - if( SUM(b.cuota) is null, 0.00, sum(b.cuota))) cierre " _
                            & " FROM jsconctatab a " _
                            & " LEFT JOIN (SELECT a.item, b.nomart descrip, b.pesounidad, b.grupo, b.marca, b.tipjer, b.division, a.cuota, a.acumulado, a.logro, " _
                            & "                   a.meta, a.activacion, a.cierre " _
                            & "            FROM jsvenstaven a " _
                            & "            LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                            & "            WHERE " _
                            & "            a.codven IN " & strAsesores & " AND " _
                            & "            MONTH(a.fecha) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                            & "            YEAR(a.fecha) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                            & "            a.tipo = 0 AND " _
                            & "            a.id_emp = '" & jytsistema.WorkID & "') b ON (a.codigo = b.grupo) " _
                            & " WHERE " _
                            & " a.modulo = '00002' AND " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " GROUP BY a.codigo "

        ds = DataSetRequery(ds, strSQLCategorias, myConn, nTablaCategorias, lblInfo)
        dtCategorias = ds.Tables(nTablaCategorias)

        If dtCategorias.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtCategorias.Rows.Count - 1
                With dtCategorias.Rows(iCont)

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (iCont + 1) / dtCategorias.Rows.Count * 100, _
                                                  " Asesor : " & CodigoAsesor & " - Categoría : " & .Item("item") & " " & .Item("descrip"))

                    Dim aFld() As String = {"codven", "item", " month(fecha)", "year(fecha)", "tipo", "id_emp"}
                    Dim aStr() As String = {CodigoAsesor, .Item("item"), CDate(txtFechaProceso.Text).Month, CDate(txtFechaProceso.Text).Year, "1", jytsistema.WorkID}

                    If qFound(myConn, lblInfo, "jsvenstaven", aFld, aStr) Then

                        ft.Ejecutar_strSQL(myconn, " update jsvenstaven set " _
                            & " codven = '" & CodigoAsesor & "',  " _
                            & " item = '" & .Item("item") & "', fecha = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " cuota = " & .Item("cuota") & " , acumulado = " & .Item("acumulado") & ", " _
                            & " logro = " & .Item("logro") & ", meta = " & .Item("meta") & ", " _
                            & " activacion = 0, cierre = " & .Item("cierre") & ", tipo = 1 , id_emp = '" & jytsistema.WorkID & "'" _
                            & " where " _
                            & " codven = '" & CodigoAsesor & "' and " _
                            & " item = '" & .Item("item") & "' and " _
                            & " month(fecha) = '" & CDate(txtFechaProceso.Text).Month & "' and " _
                            & " year(fecha) = '" & CDate(txtFechaProceso.Text).Year & "' and " _
                            & " tipo = '1' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

                    Else

                        ft.Ejecutar_strSQL(myconn, " insert into  jsvenstaven set " _
                            & " codven = '" & CodigoAsesor & "',  " _
                            & " item = '" & .Item("item") & "', fecha = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " cuota = " & .Item("cuota") & " , acumulado = " & .Item("acumulado") & ", " _
                            & " logro = " & .Item("logro") & ", meta = " & .Item("meta") & ", " _
                            & " activacion = 0, cierre = " & .Item("cierre") & ", tipo = 1 , id_emp = '" & jytsistema.WorkID & "'")

                    End If

                End With
            Next
        End If


    End Sub
    Private Sub ProcesarMarcas(ByVal CodigoAsesor As String, Clase As Integer)
        Dim dtMarcas As DataTable
        Dim nTablaMarcas As String = "tblMarcas"
        Dim strAsesores As String = IIf(Clase = 0, "('" & CodigoAsesor & "')", AsesoresDeSupervisor(myConn, lblInfo, CodigoAsesor, Clase))

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, "PROCESANDO MARCAS...")

        Dim strSQLMarcas As String = " SELECT a.codigo item, a.descrip, IF( SUM(b.cuota) IS NULL, 0, SUM(b.cuota)) cuota, " _
                            & " if( SUM(b.acumulado) is null, 0.00, sum(b.acumulado)) acumulado, " _
                            & " if( sum(b.cuota) > 0, SUM(b.acumulado)/SUM(b.cuota)*100, 0.00) logro, " _
                            & " ((IF( SUM(b.cuota) IS NULL, 0, SUM(b.cuota)) - if( SUM(b.acumulado) is null, 0.00, sum(b.acumulado)))/DiasHabilesRestantesMes('" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "','" & jytsistema.WorkID & "')) meta, 0 activacion, " _
                            & " (if( SUM(b.acumulado) is null, 0.00, sum(b.acumulado)) - IF( SUM(b.cuota) IS NULL, 0, SUM(b.cuota))) cierre " _
                            & " FROM jsconctatab a " _
                            & " LEFT JOIN (SELECT a.item, b.nomart descrip, b.pesounidad, b.grupo, b.marca, b.tipjer, b.division, a.cuota, a.acumulado, a.logro, " _
                            & "                   a.meta, a.activacion, a.cierre " _
                            & "            FROM jsvenstaven a " _
                            & "            LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                            & "            WHERE " _
                            & "            a.codven IN " & strAsesores & " AND " _
                            & "            MONTH(a.fecha) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                            & "            YEAR(a.fecha) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                            & "            a.tipo = 0 AND " _
                            & "            a.id_emp = '" & jytsistema.WorkID & "') b ON (a.codigo = b.marca) " _
                            & " WHERE " _
                            & " a.modulo = '00003' AND " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " GROUP BY a.codigo "

        ds = DataSetRequery(ds, strSQLMarcas, myConn, nTablaMarcas, lblInfo)
        dtMarcas = ds.Tables(nTablaMarcas)

        If dtMarcas.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtMarcas.Rows.Count - 1
                With dtMarcas.Rows(iCont)

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (iCont + 1) / dtMarcas.Rows.Count * 100, _
                                                  " Asesor : " & CodigoAsesor & " - Marca : " & .Item("item") & " " & .Item("descrip"))

                    Dim aFld() As String = {"codven", "item", " month(fecha)", "year(fecha)", "tipo", "id_emp"}
                    Dim aStr() As String = {CodigoAsesor, .Item("item"), CDate(txtFechaProceso.Text).Month, CDate(txtFechaProceso.Text).Year, "2", jytsistema.WorkID}

                    If qFound(myConn, lblInfo, "jsvenstaven", aFld, aStr) Then

                        ft.Ejecutar_strSQL(myConn, " update jsvenstaven set " _
                            & " codven = '" & CodigoAsesor & "',  " _
                            & " item = '" & .Item("item") & "', fecha = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " cuota = " & .Item("cuota") & " , acumulado = " & .Item("acumulado") & ", " _
                            & " logro = " & .Item("logro") & ", meta = " & .Item("meta") & ", " _
                            & " activacion = 0, cierre = " & .Item("cierre") & ", tipo = 2 , id_emp = '" & jytsistema.WorkID & "'" _
                            & " where " _
                            & " codven = '" & CodigoAsesor & "' and " _
                            & " item = '" & .Item("item") & "' and " _
                            & " month(fecha) = '" & CDate(txtFechaProceso.Text).Month & "' and " _
                            & " year(fecha) = '" & CDate(txtFechaProceso.Text).Year & "' and " _
                            & " tipo = '2' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

                    Else

                        ft.Ejecutar_strSQL(myConn, " insert into  jsvenstaven set " _
                            & " codven = '" & CodigoAsesor & "',  " _
                            & " item = '" & .Item("item") & "', fecha = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " cuota = " & .Item("cuota") & " , acumulado = " & .Item("acumulado") & ", " _
                            & " logro = " & .Item("logro") & ", meta = " & .Item("meta") & ", " _
                            & " activacion = 0, cierre = " & .Item("cierre") & ", tipo = 2 , id_emp = '" & jytsistema.WorkID & "'")

                    End If

                End With
            Next
        End If


    End Sub

    Private Sub ProcesarDivisiones(ByVal CodigoAsesor As String, Clase As Integer)
        Dim dtDivisiones As DataTable
        Dim nTablaDivisiones As String = "tblDivisiones"
        Dim strAsesores As String = IIf(Clase = 0, "('" & CodigoAsesor & "')", AsesoresDeSupervisor(myConn, lblInfo, CodigoAsesor, Clase))

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, "PROCESANDO DIVISIONES...")

        Dim strSQLDivisiones As String = " SELECT a.division item, a.descrip, IF( SUM(b.cuota) IS NULL, 0, SUM(b.cuota)) cuota, " _
                            & " IF(SUM(b.acumulado) IS NULL, 0.00, SUM(b.acumulado)) acumulado, " _
                            & " if( sum(b.cuota) > 0, SUM(b.acumulado)/SUM(b.cuota)*100, 0.00) logro, " _
                            & " ((IF( SUM(b.cuota) IS NULL, 0, SUM(b.cuota)) - IF(SUM(b.acumulado) IS NULL, 0.00, SUM(b.acumulado)))/DiasHabilesRestantesMes('" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "','" & jytsistema.WorkID & "')) meta, 0 activacion, " _
                            & " (IF(SUM(b.acumulado) IS NULL, 0.00, SUM(b.acumulado)) - IF( SUM(b.cuota) IS NULL, 0, SUM(b.cuota))) cierre " _
                            & " FROM jsmercatdiv a " _
                            & " LEFT JOIN (SELECT a.item, b.nomart descrip, b.pesounidad, b.grupo, b.marca, b.tipjer, b.division, a.cuota, a.acumulado, a.logro, " _
                            & "                   a.meta, a.activacion, a.cierre " _
                            & "            FROM jsvenstaven a " _
                            & "            LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                            & "            WHERE " _
                            & "            a.codven IN " & strAsesores & " AND " _
                            & "            MONTH(a.fecha) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                            & "            YEAR(a.fecha) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                            & "            a.tipo = 0 AND " _
                            & "            a.id_emp = '" & jytsistema.WorkID & "') b ON (a.division = b.division ) " _
                            & " WHERE " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " GROUP BY a.division "

        ds = DataSetRequery(ds, strSQLDivisiones, myConn, nTablaDivisiones, lblInfo)
        dtDivisiones = ds.Tables(nTablaDivisiones)

        If dtDivisiones.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtDivisiones.Rows.Count - 1
                With dtDivisiones.Rows(iCont)

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (iCont + 1) / dtDivisiones.Rows.Count * 100, _
                                                  " Asesor : " & CodigoAsesor & " - División : " & .Item("item") & " " & .Item("descrip"))

                    Dim aFld() As String = {"codven", "item", " month(fecha)", "year(fecha)", "tipo", "id_emp"}
                    Dim aStr() As String = {CodigoAsesor, .Item("item"), CDate(txtFechaProceso.Text).Month, CDate(txtFechaProceso.Text).Year, "3", jytsistema.WorkID}

                    If qFound(myConn, lblInfo, "jsvenstaven", aFld, aStr) Then

                        ft.Ejecutar_strSQL(myConn, " update jsvenstaven set " _
                            & " codven = '" & CodigoAsesor & "',  " _
                            & " item = '" & .Item("item") & "', fecha = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " cuota = " & .Item("cuota") & " , acumulado = " & .Item("acumulado") & ", " _
                            & " logro = " & .Item("logro") & ", meta = " & .Item("meta") & ", " _
                            & " activacion = 0, cierre = " & .Item("cierre") & ", tipo = 3 , id_emp = '" & jytsistema.WorkID & "'" _
                            & " where " _
                            & " codven = '" & CodigoAsesor & "' and " _
                            & " item = '" & .Item("item") & "' and " _
                            & " month(fecha) = '" & CDate(txtFechaProceso.Text).Month & "' and " _
                            & " year(fecha) = '" & CDate(txtFechaProceso.Text).Year & "' and " _
                            & " tipo = '3' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

                    Else

                        ft.Ejecutar_strSQL(myConn, " insert into  jsvenstaven set " _
                            & " codven = '" & CodigoAsesor & "',  " _
                            & " item = '" & .Item("item") & "', fecha = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " cuota = " & .Item("cuota") & " , acumulado = " & .Item("acumulado") & ", " _
                            & " logro = " & .Item("logro") & ", meta = " & .Item("meta") & ", " _
                            & " activacion = 0, cierre = " & .Item("cierre") & ", tipo = 3 , id_emp = '" & jytsistema.WorkID & "'")

                    End If

                End With
            Next
        End If


    End Sub
    Private Sub ProcesarJerarquías(ByVal CodigoAsesor As String, Clase As Integer)
        Dim dtJerarquias As DataTable
        Dim nTablaJerarquias As String = "tblJerarquias"
        Dim strAsesores As String = IIf(Clase = 0, "('" & CodigoAsesor & "')", AsesoresDeSupervisor(myConn, lblInfo, CodigoAsesor))

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, "PROCESANDO JERARQUÍAS...")

        Dim strSQLJerarquias As String = " SELECT a.tipjer item, a.descrip, IF( SUM(b.cuota) IS NULL, 0, SUM(b.cuota)) cuota, " _
                            & " IF(SUM(b.acumulado) IS NULL, 0.00, SUM(b.acumulado)) acumulado, " _
                            & " if( sum(b.cuota) > 0, SUM(b.acumulado)/SUM(b.cuota)*100, 0.00) logro, " _
                            & " ((IF( SUM(b.cuota) IS NULL, 0, SUM(b.cuota)) - IF(SUM(b.acumulado) IS NULL, 0.00, SUM(b.acumulado)))/DiasHabilesRestantesMes('" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "','" & jytsistema.WorkID & "')) meta, 0 activacion, " _
                            & " (IF(SUM(b.acumulado) IS NULL, 0.00, SUM(b.acumulado)) - IF( SUM(b.cuota) IS NULL, 0, SUM(b.cuota))) cierre " _
                            & " FROM jsmerencjer a " _
                            & " LEFT JOIN (SELECT a.item, b.nomart descrip, b.pesounidad, b.grupo, b.marca, b.tipjer, b.division, a.cuota, a.acumulado, a.logro, " _
                            & "                   a.meta, a.activacion, a.cierre " _
                            & "            FROM jsvenstaven a " _
                            & "            LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                            & "            WHERE " _
                            & "            a.codven IN " & strAsesores & " AND " _
                            & "            MONTH(a.fecha) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                            & "            YEAR(a.fecha) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                            & "            a.tipo = 0 AND " _
                            & "            a.id_emp = '" & jytsistema.WorkID & "') b ON (a.tipjer = b.tipjer) " _
                            & " WHERE " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " GROUP BY a.tipjer "

        ds = DataSetRequery(ds, strSQLJerarquias, myConn, nTablaJerarquias, lblInfo)
        dtJerarquias = ds.Tables(nTablaJerarquias)

        If dtJerarquias.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtJerarquias.Rows.Count - 1
                With dtJerarquias.Rows(iCont)

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (iCont + 1) / dtJerarquias.Rows.Count * 100, _
                                                  " Asesor : " & CodigoAsesor & " - Jerarquía : " & .Item("item") & " " & .Item("descrip"))

                    Dim aFld() As String = {"codven", "item", " month(fecha)", "year(fecha)", "tipo", "id_emp"}
                    Dim aStr() As String = {CodigoAsesor, .Item("item"), CDate(txtFechaProceso.Text).Month, CDate(txtFechaProceso.Text).Year, "4", jytsistema.WorkID}

                    If qFound(myConn, lblInfo, "jsvenstaven", aFld, aStr) Then

                        ft.Ejecutar_strSQL(myConn, " update jsvenstaven set " _
                            & " codven = '" & CodigoAsesor & "',  " _
                            & " item = '" & .Item("item") & "', fecha = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " cuota = " & .Item("cuota") & " , acumulado = " & .Item("acumulado") & ", " _
                            & " logro = " & .Item("logro") & ", meta = " & .Item("meta") & ", " _
                            & " activacion = 0, cierre = " & .Item("cierre") & ", tipo = 4 , id_emp = '" & jytsistema.WorkID & "'" _
                            & " where " _
                            & " codven = '" & CodigoAsesor & "' and " _
                            & " item = '" & .Item("item") & "' and " _
                            & " month(fecha) = '" & CDate(txtFechaProceso.Text).Month & "' and " _
                            & " year(fecha) = '" & CDate(txtFechaProceso.Text).Year & "' and " _
                            & " tipo = '4' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

                    Else

                        ft.Ejecutar_strSQL(myConn, " insert into  jsvenstaven set " _
                            & " codven = '" & CodigoAsesor & "',  " _
                            & " item = '" & .Item("item") & "', fecha = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " cuota = " & .Item("cuota") & " , acumulado = " & .Item("acumulado") & ", " _
                            & " logro = " & .Item("logro") & ", meta = " & .Item("meta") & ", " _
                            & " activacion = 0, cierre = " & .Item("cierre") & ", tipo = 4 , id_emp = '" & jytsistema.WorkID & "'")

                    End If

                End With
            Next
        End If


    End Sub


    Private Sub ProcesarCobranzaAnteriorPLUS(ByVal CodigoAsesor As String, Clase As Integer)
        Dim dtCobranzaAnt As DataTable
        Dim nTablaCobranzaAnt As String = "tblCobranzaAnt"
        Dim strAsesores As String = IIf(Clase = 0, "('" & CodigoAsesor & "')", AsesoresDeSupervisor(myConn, lblInfo, CodigoAsesor, Clase))

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, "PROCESANDO COBRANZA ANTERIOR ASESOR : " & CodigoAsesor)

        Dim strSQLCobranzaAnt As String = " SELECT codcli item, nombre descrip, IF(ISNULL(saldo), 0.00, saldo) cuota, " _
                & " IF(ISNULL(pagos), 0.00, ABS(pagos)) acumulado, IF(ISNULL(logro), 0.00, logro) logro,  " _
                & " IF(ISNULL(metadiaria), 0.00, metadiaria) meta, IF(ISNULL(cierre), 0.00, cierre) cierre " _
                & " FROM (SELECT CONCAT(b.codven,' ',b.nombres,' ',b.apellidos) AS vendedor, codcli, nombre, SUM(saldo) AS saldo, SUM(pagos) pagos,  IF( SUM(saldo) <> 0 ,-1*SUM(pagos)/SUM(saldo)*100, 0.00) logro, " _
                & "        IF(DiasHabilesRestantesMes('" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "','" & jytsistema.WorkID & "') <> 0, (SUM(saldo)+ SUM(pagos))/DiasHabilesRestantesMes('" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "','" & jytsistema.WorkID & "'), 0.00)  AS metadiaria, SUM(saldo) + SUM(pagos) AS cierre  " _
                & "         FROM (SELECT a.codcli, a.nombre, a.vendedor, b.saldo, SUM(c.pagos) pagos " _
                & " 		FROM jsvencatcli a " _
                & " 		LEFT JOIN (SELECT a.codcli, ROUND(SUM(b.importe),2) AS saldo " _
                & " 				FROM jsvencatcli a " _
                & " 				LEFT JOIN jsventracob b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
                & "                 WHERE " _
                & " 				b.emision < PrimerDiaMes('" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "') AND " _
                & " 				a.id_emp = '" & jytsistema.WorkID & "' " _
                & " 				GROUP BY a.codcli " _
                & " 				HAVING  ABS(saldo) > 0.0001) b ON (a.codcli = b.codcli) " _
                & " 		LEFT JOIN (SELECT a.codcli, SUM(b.importe) pagos " _
                & " 				FROM (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) importe FROM jsventracob a WHERE a.codcli <>  '' AND a.emision < PrimerDiaMes('" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "') AND a.id_emp = '" & jytsistema.WorkID & "' 	GROUP BY a.codcli, a.nummov HAVING ABS(importe) > 0.0001 ) a " _
                & " 				LEFT JOIN jsventracob  b ON (a.nummov = b.nummov AND a.codcli = b.codcli) " _
                & "                 WHERE " _
                & " 				b.tipomov IN ('NC','ND','CA','AB') AND " _
                & " 				b.emision >= PrimerDiaMes('" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "') AND " _
                & " 				b.emision <= '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "' AND " _
                & " 				b.id_emp = '" & jytsistema.WorkID & "' " _
                & "				GROUP BY a.codcli) c ON (a.codcli = c.codcli) " _
                & " 		WHERE a.id_emp = '" & jytsistema.WorkID & "'  " _
                & " 		GROUP BY a.codcli) a " _
                & "         LEFT JOIN jsvencatven b ON (a.vendedor = b.codven) " _
                & "         WHERE " _
                & "         a.vendedor IN " & strAsesores & " AND " _
                & "         b.id_emp = '" & jytsistema.WorkID & "' " _
                & "         GROUP BY codcli) a " _
                & " ORDER BY 1,2 "

        ds = DataSetRequery(ds, strSQLCobranzaAnt, myConn, nTablaCobranzaAnt, lblInfo)
        dtCobranzaAnt = ds.Tables(nTablaCobranzaAnt)

        If dtCobranzaAnt.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtCobranzaAnt.Rows.Count - 1
                With dtCobranzaAnt.Rows(iCont)

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (iCont + 1) / dtCobranzaAnt.Rows.Count * 100, _
                                                  " Asesor : " & CodigoAsesor & " - Cobranza Anterior : " & .Item("item") & " " & .Item("descrip"))

                    Dim aFld() As String = {"codven", "item", " month(fecha)", "year(fecha)", "tipo", "id_emp"}
                    Dim aStr() As String = {CodigoAsesor, .Item("item"), CDate(txtFechaProceso.Text).Month, CDate(txtFechaProceso.Text).Year, "5", jytsistema.WorkID}

                    If qFound(myConn, lblInfo, "jsvenstaven", aFld, aStr) Then

                        ft.Ejecutar_strSQL(myConn, " update jsvenstaven set " _
                            & " codven = '" & CodigoAsesor & "',  " _
                            & " item = '" & .Item("item") & "', fecha = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " cuota = " & .Item("cuota") & " , acumulado = " & .Item("acumulado") & ", " _
                            & " logro = " & .Item("logro") & ", meta = " & .Item("meta") & ", " _
                            & " activacion = 0, cierre = " & .Item("cierre") & ", tipo = 5 , id_emp = '" & jytsistema.WorkID & "'" _
                            & " where " _
                            & " codven = '" & CodigoAsesor & "' and " _
                            & " item = '" & .Item("item") & "' and " _
                            & " month(fecha) = '" & CDate(txtFechaProceso.Text).Month & "' and " _
                            & " year(fecha) = '" & CDate(txtFechaProceso.Text).Year & "' and " _
                            & " tipo = '5' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

                    Else

                        ft.Ejecutar_strSQL(myConn, " insert into  jsvenstaven set " _
                            & " codven = '" & CodigoAsesor & "',  " _
                            & " item = '" & .Item("item") & "', fecha = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " cuota = " & .Item("cuota") & " , acumulado = " & .Item("acumulado") & ", " _
                            & " logro = " & .Item("logro") & ", meta = " & .Item("meta") & ", " _
                            & " activacion = 0, cierre = " & .Item("cierre") & ", tipo = 5 , id_emp = '" & jytsistema.WorkID & "'")

                    End If

                End With
            Next
        End If


    End Sub



    Private Sub ProcesarCobranzaActual(ByVal CodigoAsesor As String, Clase As Integer)
        Dim dtCobranzaAnt As DataTable
        Dim nTablaCobranzaAnt As String = "tblCobranzaAnt"
        Dim strAsesores As String = IIf(Clase = 0, "('" & CodigoAsesor & "')", AsesoresDeSupervisor(myConn, lblInfo, CodigoAsesor, Clase))

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, "PROCESANDO COBRANZA ACTUAL ASESOR : " & CodigoAsesor)

        ft.Ejecutar_strSQL(myconn, " DROP TABLE IF EXISTS saldos")
        ft.Ejecutar_strSQL(myconn, " DROP TABLE IF EXISTS pagos ")

        ft.Ejecutar_strSQL(myConn, " CREATE TEMPORARY TABLE saldos (codcli VARCHAR(15) NOT NULL, nummov VARCHAR(30) NOT NULL, saldos DOUBLE(19,2) NOT NULL,  PRIMARY KEY (codcli,nummov)) " _
                        & " SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldos " _
                        & "                         		FROM jsventracob a " _
                        & "                                 WHERE " _
                        & "                                 a.tipomov in ('FC', 'GR', 'ND') AND " _
                        & "                                 a.emision >= primerdiames('" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "' ) AND " _
                        & "               	                a.emision <= '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "'  AND " _
                        & "                         		a.id_emp = '" & jytsistema.WorkID & "' " _
                        & "                         		GROUP BY a.codcli, a.nummov ")

        ft.Ejecutar_strSQL(myconn, " CREATE TEMPORARY TABLE pagos (codcli VARCHAR(15) NOT NULL, nummov VARCHAR(30) NOT NULL, pagos DOUBLE(19,2) NOT NULL,  PRIMARY KEY (codcli,nummov)) " _
                        & " SELECT b.codcli, b.nummov, SUM(b.importe) pagos " _
                        & " FROM jsventracob b  " _
                        & " WHERE " _
                        & " b.tipomov IN ('NC','CA','AB') AND " _
                        & " b.emision >= primerdiames('" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "' ) AND " _
                        & " b.emision <= '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "'  AND " _
                        & " b.id_emp = '" & jytsistema.WorkID & "' " _
                        & " GROUP BY b.codcli, b.nummov ")


        Dim strSQLCobranzaAnt As String = " SELECT a.codcli item, a.nombre descrip, " _
                    & " IF(b.saldos IS NULL,0.00, b.saldos) cuota, " _
                    & " IF(b.pagos IS NULL,0.00, ABS(b.pagos)) acumulado, " _
                    & " IF (  IF(b.pagos IS NULL, 0.00, ABS( b.pagos))/IF(b.saldos IS NULL, 0.00,  b.saldos) IS NULL, 0.00, IF(b.pagos IS NULL, 0.00, ABS(b.pagos))/IF(b.saldos IS NULL, 0.00,  b.saldos) )  *100 logro, " _
                    & " IF( DiasHabilesRestantesMes('" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "','" & jytsistema.WorkID & "') = 0 , 0, " _
                    & " (IF(b.saldos IS NULL, 0.00,  b.saldos)  + IF(b.pagos IS NULL, 0.00, b.pagos))/DiasHabilesRestantesMes('" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "','" & jytsistema.WorkID & "')) meta ,  " _
                    & " 0 activacion, (IF(b.pagos IS NULL, 0.00, b.pagos) + IF(b.saldos IS NULL, 0.00,  b.saldos)) cierre " _
                    & " FROM jsvencatcli a " _
                    & " LEFT JOIN (SELECT a.codcli, SUM(saldos) saldos, SUM(pagos) pagos " _
                    & "             FROM (SELECT a.codcli, a.nummov, a.saldos, IF ( b.pagos IS NULL, 0.00, b.pagos) pagos " _
                    & "  				    FROM saldos a " _
                    & " 					LEFT JOIN pagos b ON (a.codcli = b.codcli AND a.nummov = b.nummov)) a " _
                    & "             GROUP BY a.codcli) b ON (a.codcli = b.codcli) " _
                    & " WHERE " _
                    & " a.vendedor IN " & strAsesores & " AND " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' "


        ds = DataSetRequery(ds, strSQLCobranzaAnt, myConn, nTablaCobranzaAnt, lblInfo)
        dtCobranzaAnt = ds.Tables(nTablaCobranzaAnt)

        If dtCobranzaAnt.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtCobranzaAnt.Rows.Count - 1
                With dtCobranzaAnt.Rows(iCont)

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (iCont + 1) / dtCobranzaAnt.Rows.Count * 100, _
                                                  " Asesor : " & CodigoAsesor & " - Cobranza Actual : " & .Item("item") & " " & .Item("descrip"))

                    Dim aFld() As String = {"codven", "item", " month(fecha)", "year(fecha)", "tipo", "id_emp"}
                    Dim aStr() As String = {CodigoAsesor, .Item("item"), CDate(txtFechaProceso.Text).Month, CDate(txtFechaProceso.Text).Year, "6", jytsistema.WorkID}

                    If qFound(myConn, lblInfo, "jsvenstaven", aFld, aStr) Then

                        ft.Ejecutar_strSQL(myconn, " update jsvenstaven set " _
                            & " codven = '" & CodigoAsesor & "',  " _
                            & " item = '" & .Item("item") & "', fecha = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " cuota = " & .Item("cuota") & " , acumulado = " & .Item("acumulado") & ", " _
                            & " logro = " & .Item("logro") & ", meta = " & .Item("meta") & ", " _
                            & " activacion = 0, cierre = " & .Item("cierre") & ", tipo = 6 , id_emp = '" & jytsistema.WorkID & "'" _
                            & " where " _
                            & " codven = '" & CodigoAsesor & "' and " _
                            & " item = '" & .Item("item") & "' and " _
                            & " month(fecha) = '" & CDate(txtFechaProceso.Text).Month & "' and " _
                            & " year(fecha) = '" & CDate(txtFechaProceso.Text).Year & "' and " _
                            & " tipo = '6' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

                    Else

                        ft.Ejecutar_strSQL(myconn, " insert into  jsvenstaven set " _
                            & " codven = '" & CodigoAsesor & "',  " _
                            & " item = '" & .Item("item") & "', fecha = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " cuota = " & .Item("cuota") & " , acumulado = " & .Item("acumulado") & ", " _
                            & " logro = " & .Item("logro") & ", meta = " & .Item("meta") & ", " _
                            & " activacion = 0, cierre = " & .Item("cierre") & ", tipo = 6 , id_emp = '" & jytsistema.WorkID & "'")

                    End If

                End With
            Next
        End If


    End Sub

    Private Sub btnFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFecha.Click
        txtFechaProceso.Text = SeleccionaFecha(CDate(txtFechaProceso.Text), btnFecha)
    End Sub

    Private Sub chkSeleccionar_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkSeleccionar.CheckedChanged
        chk1.Checked = chkSeleccionar.Checked
        chk2.Checked = chkSeleccionar.Checked
        chk3.Checked = chkSeleccionar.Checked
        chk4.Checked = chkSeleccionar.Checked
        chk5.Checked = chkSeleccionar.Checked
        chk6.Checked = chkSeleccionar.Checked
        chk7.Checked = chkSeleccionar.Checked
        chkCierre.Checked = chkSeleccionar.Checked
    End Sub
End Class