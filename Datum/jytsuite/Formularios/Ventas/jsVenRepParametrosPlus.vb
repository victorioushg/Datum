Imports MySql.Data.MySqlClient
Imports System.IO
Imports ReportesDeVentas
Imports CrystalDecisions.CrystalReports.Engine
Imports Syncfusion.WinForms.Input
Public Class jsVenRepParametrosPlus

    Private Const sModulo As String = "REPORTES"

    Private ReporteNumero As Integer
    Private ReporteNombre As String
    Private CodigoCliente As String, Documento As String
    Private FechaParametro As Date

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private vOrdenNombres() As String
    Private vOrdenCampos() As String
    Private vOrdenTipo() As String
    Private vOrdenLongitud() As Integer
    Private IndiceReporte As Integer
    Private strIndiceReporte As String

    Private vGrupos As String = ""

    Private aSiNoTodos() As String = {"Si", "No", "Todos"}

    Private aEstatusFacturas() As String = {"Por confirmar", "Confirmadas", "Anuladas", "Todas"}
    Private aCondicionPago() As String = {"Crédito", "Contado", "Todos"}
    Private aEstatusClientes() As String = {"Activo", "Bloqueado", "Inactivo", "Desincorporado", "Todos"}
    Private aEstatusMercancias() As String = {"Activo", "Inactivo", "Todos"}

    Private aExistencias() As String = {"Con", "Sin", "Todas"}

    Private aTipo() As String = {"Ordinario", "Especial", "Formal", "No contribuyente", "Todos"}
    Private aVencimientos() As String = {"Emisión", "Vencimiento"}
    Private aTipoMercancia() As String = {"Venta", "Uso interno", "POP", "Alquiler", "Préstamo", "Materia prima", "Otros", "Todos"}
    Private periodoTipo As TipoPeriodo
    Private aAño() As Object = {2000, 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, _
                                  2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028, 2029, 2030}

    Private aInNOTIn() As String = {"EN", "NO EN"}
    Private aTipoReporte() As String = {"Unidad Venta (UV) ", "Kilogramos (UMP)", "Ventas (BsF)", "Cajas (UMS)", "Costos (BsF)"}
    Private strSQLConsultas As String = ""
    Private nTabla As String = "tbl" & ft.NumeroAleatorio(1000000)

    Private cadClientes As String = ""
    Private cadCanal As String = ""
    Private cadTipoNegocio As String = ""
    Private cadZona As String = ""
    Private cadRuta As String = ""
    Private cadAsesor As String = ""
    Private cadPais As String = ""
    Private cadEstado As String = ""
    Private cadMunicipio As String = ""
    Private cadParroquia As String = ""
    Private cadCiudad As String = ""
    Private cadBarrio As String = ""
    Private cadMercancias As String = ""
    Private cadCategoria As String = ""
    Private cadMarca As String = ""
    Private cadDivision As String = ""
    Private cadJerarquia As String = ""
    Private cadNivel1 As String = ""
    Private cadNivel2 As String = ""
    Private cadNivel3 As String = ""
    Private cadNivel4 As String = ""
    Private cadNivel5 As String = ""
    Private cadNivel6 As String = ""
    Private cadAlmacen As String = ""
    Private cadCategoriaProveedor As String = ""
    Private cadUnidadProveedor As String = ""


    Public Sub Cargar(ByVal TipoCarga As Integer, ByVal numReporte As Integer, ByVal nomReporte As String, _
                      Optional ByVal CodCliente As String = "", Optional ByVal numDocumento As String = "", _
                      Optional ByVal Fecha As Date = #1/1/2012#)

        Me.Dock = DockStyle.Fill
        myConn.Open()

        Dim dates As SfDateTimeEdit() = {txtPeriodoDesde, txtPeriodoHasta}
        SetSizeDateObjects(dates)

        ReporteNumero = numReporte
        ReporteNombre = nomReporte
        CodigoCliente = CodCliente
        Documento = numDocumento
        FechaParametro = Fecha

        For Each cmb As Control In grpVentas.Controls
            If TypeOf cmb Is ComboBox Then
                ft.RellenaCombo(aInNOTIn, cmb)
            End If
        Next
        For Each cmb As Control In grpMercas.Controls
            If TypeOf cmb Is ComboBox Then
                ft.RellenaCombo(aInNOTIn, cmb)
            End If
        Next
        For Each cmb As Control In grpCompras.Controls
            If TypeOf cmb Is ComboBox Then
                ft.RellenaCombo(aInNOTIn, cmb)
            End If
        Next
        For Each cmb As Control In grpCriterios.Controls
            If TypeOf cmb Is ComboBox And cmb.Name <> "Año" Then
                ft.RellenaCombo(aInNOTIn, cmb)
            End If
        Next

        ft.RellenaCombo(aTipoReporte, cmbTipoReporte)
        ft.RellenaCombo(aAño, cmbAño, Year(jytsistema.sFechadeTrabajo) - 2000)
        RellenarConsultas(myConn)
        cargarConsultas()
        PresentarReporte(numReporte, nomReporte, CodigoCliente)

        If TipoCarga = TipoCargaFormulario.iShow Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If

    End Sub


    Private Sub RellenarConsultas(MyConn As MySqlConnection, Optional numConsulta As Integer = 0)

        strSQLConsultas = "select a.consulta_nombre, a.consulta_id from jsconencconsulta a " _
                & " where " _
                & " a.usuario_id = '" & jytsistema.sUsuario & "' AND " _
                & " a.reporte_id = " & ReporteNumero & " AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' order by a.consulta_nombre "

        ds = DataSetRequery(ds, strSQLConsultas, MyConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        RellenaComboConDatatable(cmbMisConsultas, dt, "CONSULTA_NOMBRE", "CONSULTA_ID")

    End Sub

    Private Sub cargarConsultas()

        If cmbMisConsultas.Text <> "" Then

            Dim strCON As String = " select a.* from jsconrenconsulta a " _
                & " where " _
                & " a.consulta_id = '" & cmbMisConsultas.SelectedValue.ToString & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' order by a.consulta_id "
            Dim nTablecon As String = "tblCON"
            Dim dtCON As DataTable
            ds = DataSetRequery(ds, strCON, myConn, nTablecon, lblAsesor)
            dtCON = ds.Tables(nTablecon)

            lIMPIAR()

            If dtCON.Rows.Count > 0 Then
                For Each nRow As DataRow In dtCON.Rows

                    Dim ss As Integer = Me.Controls.Count

                    For Each mControl As Control In grpVentas.Controls
                        If mControl.Name = nRow.Item("OBJETO_ID") Then
                            mControl.Text = nRow.Item("OBJETO_TEXTO").ToString.Replace("°", "'")
                        End If
                    Next
                    For Each mControl As Control In grpMercas.Controls
                        If mControl.Name = nRow.Item("OBJETO_ID") Then
                            mControl.Text = nRow.Item("OBJETO_TEXTO").ToString.Replace("°", "'")
                        End If
                    Next
                    For Each mControl As Control In grpCompras.Controls
                        If mControl.Name = nRow.Item("OBJETO_ID") Then
                            mControl.Text = nRow.Item("OBJETO_TEXTO").ToString.Replace("°", "'")
                        End If
                    Next
                    For Each mControl As Control In grpCriterios.Controls
                        If mControl.Name = nRow.Item("OBJETO_ID") Then
                            mControl.Text = nRow.Item("OBJETO_TEXTO").ToString.Replace("°", "'")
                        End If
                    Next

                Next

            End If

            dtCON.Dispose()
            dtCON = Nothing

        End If

    End Sub
    Private Function OrigenReporte(numRep As Integer) As String

        Select Case Int(numRep / 100)
            Case 1
                Return " COT "
            Case 2
                Return " BAN "
            Case 3
                Return " NOM "
            Case 4
                Return " COM "
            Case 5
                Return " VEN "
            Case 6
                Return " PVE "
            Case 7
                Return " MER "
            Case 8
                Return " SIG "
            Case Else
                Return ""
        End Select

    End Function

    Private Sub PresentarReporte(ByVal NumeroReporte As Integer, ByVal NombreDelReporte As String, Optional ByVal CodigoCliente As String = "")

        lblNombreReporte.Text += OrigenReporte(NumeroReporte) + " - " + NombreDelReporte
        Select Case NumeroReporte

            Case ReporteVentas.cVentasClienteDivision
                Dim vOrdenNombres() As String = {"Mayor Peso"}
                Dim vOrdenCampos() As String = {"prov_cli"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)

            Case ReporteVentas.cActivacionAsesor
                Dim vOrdenNombres() As String = {"Código Asesor"}
                Dim vOrdenCampos() As String = {"vendedor"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)

            Case ReporteVentas.cVentasAsesor, ReporteMedicionGerencial.cVentasMesMesAsesor
                Dim vOrdenNombres() As String = {"Código Asesor"}
                Dim vOrdenCampos() As String = {"vendedor"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)

            Case ReporteMedicionGerencial.cDevolucionesPorCausa
                Dim vOrdenNombres() As String = {"Código Causa"}
                Dim vOrdenCampos() As String = {"causa"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)

            Case ReporteVentas.cClientes
                Dim vOrdenNombres() As String = {"Código Cliente", "Nombre cliente"}
                Dim vOrdenCampos() As String = {"codcli", "nombre"}
                Dim vOrdenTipo() As String = {"S", "S"}
                Dim vOrdenLongitud() As Integer = {15, 250}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)

            Case ReporteMedicionGerencial.cActivacionClientesMesMes

                Dim vOrdenNombres() As String = {"Año"}
                Dim vOrdenCampos() As String = {"anio"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, True, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud)

            Case ReporteMedicionGerencial.cVentasComparativasMesMes, ReporteMedicionGerencial.cComprasComparativasMesMes

                Dim vOrdenNombres() As String = {"Código Mercancía"}
                Dim vOrdenCampos() As String = {"codart"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud)

            Case ReporteVentas.cVentasPorClientesPlus, ReporteVentas.cVentasPorClienteMesMes
                Dim vOrdenNombres() As String = {"Código Cliente"}
                Dim vOrdenCampos() As String = {"prov_cli"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)

            Case ReporteVentas.cActivacionesClientesMercas, ReporteVentas.cChequesDevueltos
                Dim vOrdenNombres() As String = {"Código Cliente"}
                Dim vOrdenCampos() As String = {"codcli"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)


                '///////////////// MERCANCIAS
            Case ReporteMercancias.cCatalogo, ReporteMercancias.cPrecios, ReporteMercancias.cPreciosIVA, _
            ReporteMercancias.cEquivalencias
                Dim vOrdenNombres() As String = {"Código mercancía", "Nombre mercancía"}
                Dim vOrdenCampos() As String = {"codart", "nomart"}
                Dim vOrdenTipo() As String = {"S", "S"}
                Dim vOrdenLongitud() As Integer = {15, 150}
                Inicializar(ReporteNombre, True, True, False, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteMercancias.cInventarioLegal, ReporteMercancias.cExistenciasAUnaFecha
                Dim vOrdenNombres() As String = {"Código Mercancía", "Nombre Mercancía"}
                Dim vOrdenCampos() As String = {"codart", "nomart"}
                Dim vOrdenTipo() As String = {"S", "S"}
                Dim vOrdenLongitud() As Integer = {15, 150}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)


        End Select
    End Sub
    Private Sub Inicializar(ByVal nEtiqueta As String, ByVal TabOrden As Boolean, ByVal TabGrupo As Boolean, _
        ByVal TabCriterio As Boolean, ByVal TabConstantes As Boolean, ByVal aNombreOrden() As String, _
        ByVal aCampoOrden() As String, ByVal aTipoOrden() As String, ByVal aLongitudOrden() As Integer, _
        Optional ByVal Trabajador As String = "")


        HabilitarTabs(TabOrden, TabGrupo, TabCriterio, TabConstantes)
        IniciarOrden(aNombreOrden, aCampoOrden)
        IniciarGrupos()
        IniciarCriterios()
        IniciarConstantes()

    End Sub
    Private Sub HabilitarTabs(ByVal Orden As Boolean, ByVal GRupo As Boolean, ByVal Criterio As Boolean, ByVal Constante As Boolean)

        grpOrden.Enabled = Orden
        grpGrupos.Enabled = GRupo
        grpCriterios.Enabled = Criterio
        grpConstantes.Enabled = Constante

    End Sub
    Private Sub IniciarOrden(ByVal vNombres As Object, ByVal vCampos As Object)

        vOrdenNombres = vNombres
        vOrdenCampos = vCampos

        IndiceReporte = 0
        strIndiceReporte = vCampos(IndiceReporte)
        ft.RellenaCombo(vNombres, cmbOrdenadoPor)

    End Sub

    Private Sub IniciarGrupos()

        ft.habilitarObjetos(False, False, tabPageVentas, tabPageMercas, tabPageCompras)

        Select ReporteNumero
            Case ReporteVentas.cActivacionAsesor, ReporteVentas.cVentasAsesor, _
                ReporteMedicionGerencial.cVentasMesMesAsesor

                ft.habilitarObjetos(True, False, tabPageVentas, tabPageMercas)
                C1DockingTab1.SelectedTab = tabPageVentas

                ft.visualizarObjetos(False, lblCanal, btnCanal, cmbCanal, lblTipoNegocio, btnTipoNegocio, cmbTipoNegocio, _
                                  lblZona, btnZona, cmbZona, lblRuta, btnRuta, cmbRuta)

            Case ReporteMedicionGerencial.cVentasComparativasMesMes, ReporteVentas.cVentasPorClientesPlus, _
                ReporteVentas.cActivacionesClientesMercas, ReporteMedicionGerencial.cActivacionClientesMesMes

                ft.habilitarObjetos(True, False, tabPageVentas, tabPageMercas)
                C1DockingTab1.SelectedTab = tabPageVentas

            Case ReporteMedicionGerencial.cComprasComparativasMesMes

                ft.habilitarObjetos(True, False, tabPageMercas, tabPageCompras)
                C1DockingTab1.SelectedTab = tabPageCompras

            Case ReporteMercancias.cInventarioLegal, ReporteMercancias.cCatalogo, ReporteMercancias.cExistenciasAUnaFecha, _
                ReporteMercancias.cEquivalencias

                ft.habilitarObjetos(True, False, tabPageMercas)
                C1DockingTab1.SelectedTab = tabPageMercas

            Case Else

                ft.habilitarObjetos(True, False, tabPageVentas)
                C1DockingTab1.SelectedTab = tabPageVentas

        End Select

    End Sub

    Private Sub IniciarCriterios()

        VerCriterio_Periodo(False, 0)
        VerCriterio_TipoDocumento(False)
        VerCriterio_Cliente(False)
        VerCriterio_Mercancia(False)
        VerCriterio_Almacen(False)
        VerCriterio_Año(False)

        Select Case ReporteNumero

            Case ReporteVentas.cVentasClienteDivision
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Cliente(True)
            Case ReporteVentas.cClientes, ReporteVentas.cVentasPorClienteMesMes
                VerCriterio_Cliente(True)
            Case ReporteVentas.cActivacionAsesor, ReporteVentas.cVentasAsesor, _
                ReporteMedicionGerencial.cDevolucionesPorCausa, ReporteVentas.cChequesDevueltos

                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)

            Case ReporteVentas.cVentasPorClientesPlus
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Cliente(True)
                VerCriterio_Almacen(True)
                VerCriterio_Mercancia(True)
            Case ReporteMedicionGerencial.cVentasComparativasMesMes
                VerCriterio_Cliente(True)
            Case ReporteMedicionGerencial.cVentasMesMesAsesor, ReporteMedicionGerencial.cDevolucionesAsesorMesMes
                VerCriterio_Año(True)
            Case ReporteMedicionGerencial.cComprasComparativasMesMes
                VerCriterio_Cliente(True)
                lblcliente.Text = "PROVEEDORES "
            Case ReporteVentas.cActivacionesClientesMercas
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Cliente(True)
                VerCriterio_Mercancia(True)
            Case ReporteMercancias.cExistenciasAUnaFecha
                VerCriterio_Periodo(True, 2, TipoPeriodo.iDiario)
                VerCriterio_Almacen(True)
                VerCriterio_Mercancia(True)
            Case ReporteMercancias.cInventarioLegal
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Almacen(True)
            Case ReporteMedicionGerencial.cActivacionClientesMesMes
                VerCriterio_Año(True)
            Case Else

        End Select

    End Sub
    Private Sub VerCriterio_Año(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblAño, cmbAño)
    End Sub
    Private Sub VerCriterio_Periodo(ByVal Ver As Boolean, ByVal CompletoDesdeHasta As Integer, Optional ByVal Periodo As TipoPeriodo = TipoPeriodo.iMensual)
        'CompletoDesdeHasta 0 = Complete , 1 = Desde , 2 = Hasta 
        ft.visualizarObjetos(False, lblPeriodoDesde, lblPeriodoHasta, lblPeriodo, txtPeriodoDesde, txtPeriodoHasta)
        periodoTipo = Periodo
        If Ver Then
            Select Case CompletoDesdeHasta
                Case 0
                    ft.visualizarObjetos(Ver, lblPeriodoDesde, lblPeriodoHasta, lblPeriodo, txtPeriodoDesde, txtPeriodoHasta)
                Case 1
                    ft.visualizarObjetos(Ver, lblPeriodoDesde, lblPeriodo, txtPeriodoDesde)
                Case 2
                    ft.visualizarObjetos(Ver, lblPeriodoHasta, lblPeriodo, txtPeriodoHasta)
            End Select
        End If

        Select Case Periodo
            Case TipoPeriodo.iDiario
                txtPeriodoDesde.Value = jytsistema.sFechadeTrabajo
                txtPeriodoHasta.Value = jytsistema.sFechadeTrabajo
            Case TipoPeriodo.iSemanal
                txtPeriodoDesde.Value = PrimerDiaSemana(jytsistema.sFechadeTrabajo)
                txtPeriodoHasta.Value = UltimoDiaSemana(jytsistema.sFechadeTrabajo)
            Case TipoPeriodo.iMensual
                txtPeriodoDesde.Value = PrimerDiaMes(jytsistema.sFechadeTrabajo)
                txtPeriodoHasta.Value = UltimoDiaMes(jytsistema.sFechadeTrabajo)
            Case TipoPeriodo.iAnual
                txtPeriodoDesde.Value = PrimerDiaAño(jytsistema.sFechadeTrabajo)
                txtPeriodoHasta.Value = UltimoDiaAño(jytsistema.sFechadeTrabajo)
            Case Else
                txtPeriodoDesde.Value = jytsistema.sFechadeTrabajo
                txtPeriodoHasta.Value = jytsistema.sFechadeTrabajo
        End Select

    End Sub
    Private Sub VerCriterio_TipoDocumento(ByVal ver As Boolean, Optional ByVal DocumentosTipo As Integer = 0)
        'DocumentosTipo : 0 = Bancos, 1 = caja, 2 = Forma de pago , 3 = CxC/CxP, 4 = Mercancías
        'ft.visualizarObjetos(ver, lblTipodocumento, btnTipoDocumento, cmbTipoDocumento, lblTipoDocumentoSeleccion)

        'Select Case DocumentosTipo
        '    Case 0
        '        Dim aTipoDocumento() As String = {"CH", "DP", "NC", "ND"}
        '        Dim aSel() As Boolean = {True, True, True, True}
        '        RellenaListaSeleccionable(chkList, aTipoDocumento, aSel)
        '        txtTipDoc.Text = ".CH.DP.NC.ND"
        '    Case 1
        '        Dim aTipoDocumento() As String = {"EN", "SA"}
        '        Dim aSel() As Boolean = {True, True}
        '        RellenaListaSeleccionable(chkList, aTipoDocumento, aSel)
        '        txtTipDoc.Text = ".EN.SA"
        '    Case 2
        '        Dim aTipoDocumento() As String = {"EF", "CH", "TA", "CT", "DP", "TR"}
        '        Dim aSel() As Boolean = {True, True, True, True}
        '        RellenaListaSeleccionable(chkList, aTipoDocumento, aSel)
        '        txtTipDoc.Text = ".EF.CH.TA.CT.DP.TR"
        '    Case 3
        '        Dim aTipoDocumento() As String = {"FC", "GR", "ND", "AB", "CA", "NC"}
        '        Dim aSel() As Boolean = {True, True, True, True, True, True}
        '        RellenaListaSeleccionable(chkList, aTipoDocumento, aSel)
        '        txtTipDoc.Text = ".FC.GR.ND.AB.CA.NC"
        'End Select
    End Sub
    Private Sub VerCriterio_Cliente(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblcliente, btnCliente, cmbCliente, lblClienteSeleccion)
    End Sub

    Private Sub VerCriterio_Almacen(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblCriterioAlmacen, btnAlmacen, cmbAlmacen, lblAlmacenSeleccion)

    End Sub
    Private Sub VerCriterio_Mercancia(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblMercancia, btnMercancia, cmbMercancias, lblMercanciaSeleccion)
    End Sub


    Private Sub IniciarConstantes()

        ValoresInicialesConstantes()

        verConstante_Resumen(False)
        VerConstante_Tarifas(False)
        VerConstante_peso(False)
        VerConstante_TipoCliente(False)
        VerConstante_CondicionPago(False)
        VerConstante_Regulado(False)
        VerConstante_Estatus(False)
        verConstante_EstatusFacturas(False)
        VerConstante_Lapsos(False)
        verConstante_Descuentos(False)
        VerConstante_VerPrimeros(False)
        VerConstante_VentasConstante(False)
        VerConstante_TipoReporte(False)
        VerConstante_TipoMercancia(False)
        VerConstante_Cartera(False)

        Select Case ReporteNumero
            Case ReporteVentas.cVentasAsesor, ReporteMedicionGerencial.cVentasMesMesAsesor, _
                ReporteMedicionGerencial.cDevolucionesAsesorMesMes, ReporteMedicionGerencial.cDevolucionesPorCausa, _
                ReporteMercancias.cInventarioLegal

                VerConstante_TipoReporte(True)
                If ReporteNumero = ReporteMercancias.cInventarioLegal Then cmbTipoReporte.Enabled = False

            Case ReporteVentas.cClientes

                verConstante_Resumen(True)
                VerConstante_TipoCliente(True)
                VerConstante_Estatus(True)
                verConstante_Descuentos(True)

            Case ReporteVentas.cChequesDevueltos
                verConstante_Resumen(True)
                VerConstante_TipoCliente(True)
                VerConstante_Estatus(True)

            Case ReporteMedicionGerencial.cComprasComparativasMesMes, ReporteMedicionGerencial.cVentasComparativasMesMes, _
                ReporteVentas.cVentasPorClientesPlus

                verConstante_Resumen(True)
                VerConstante_TipoReporte(True)
                VerConstante_peso(True)
                lblPeso.Text = "Incluye movimientos de CXC"
                chkPeso.Checked = False

            Case ReporteVentas.cVentasClienteDivision
                VerConstante_Estatus(True)
                VerConstante_TipoReporte(True)
            Case ReporteVentas.cVentasPorClienteMesMes

                VerConstante_TipoCliente(True)
                VerConstante_Estatus(True)
                VerConstante_TipoReporte(True)


            Case ReporteVentas.cActivacionesClientesMercas
                VerConstante_Estatus(True)
                verConstante_EstatusFacturas(True)
                lblEstatusFactura.Text = " Tipo Activación : "
                Dim aR() As String = {"Clientes sin activar", "Clientes activados", "Todos"}
                ft.RellenaCombo(aR, cmbEstatusFacturas, 1)
                verConstante_Resumen(True)
                VerConstante_TipoReporte(True)

                '////// MERCANCIAS
            Case ReporteMercancias.cCatalogo, ReporteMercancias.cEquivalencias
                VerConstante_peso(True)
                VerConstante_TipoMercancia(True)
                VerConstante_Regulado(True)
                VerConstante_Cartera(True)
                VerConstante_Estatus(True)

            Case ReporteMercancias.cExistenciasAUnaFecha
                VerConstante_peso(True)
                VerConstante_Cartera(True)
                verConstante_Descuentos(True)
                VerConstante_TipoMercancia(True)
                VerConstante_Estatus(True)
                verConstante_Resumen(True)
                lblConsResumen.Text = " Costes en reporte : "
                VerConstante_VentasConstante(True)
                lblConstanteVentas.Text = "Días cálculo promedio ventas : "
                txtConstanteVentas.Text = ft.FormatoEntero(90)
                VerConstante_TipoReporte(True)


        End Select

    End Sub
    Private Sub ValoresInicialesConstantes()

        chkConsResumen.Checked = False
        chkPrecioA.Checked = True
        chkPrecioB.Checked = True
        chkPrecioC.Checked = True
        chkPrecioD.Checked = True
        chkPrecioE.Checked = True
        chkPrecioF.Checked = True
        chkPeso.Checked = False
        ft.RellenaCombo(aCondicionPago, cmbCondicionPago, 2)
        ft.RellenaCombo(aSiNoTodos, cmbRegulada, 2)
        ft.RellenaCombo(aTipo, cmbTipo, 4)
        If OrigenReporte(ReporteNumero) = " MER " Then
            ft.RellenaCombo(aEstatusMercancias, cmbEstatus)
            ft.RellenaCombo(aExistencias, cmbDescuentos, 2)
            lblDescuentos.Text = "Existencias"
        ElseIf OrigenReporte(ReporteNumero) = " VEN " Then
            ft.RellenaCombo(aEstatusClientes, cmbEstatus)
            ft.RellenaCombo(aSiNoTodos, cmbDescuentos, 2)
        Else

        End If

        ft.RellenaCombo(aEstatusFacturas, cmbEstatusFacturas, 3)
        ft.RellenaCombo(aSiNoTodos, cmbCartera, 2)
        ft.RellenaCombo(aTipoMercancia, cmbTipoMercancias)
        txtConstanteVentas.Text = "0"


    End Sub
    Private Sub VerConstante_Lapsos(Ver As Boolean, Optional Desde1 As Integer = 1, Optional Hasta1 As Integer = 7, _
                                     Optional Desde2 As Integer = 8, Optional Hasta2 As Integer = 15, _
                                     Optional Desde3 As Integer = 16, Optional Hasta3 As Integer = 30, _
                                     Optional Desde4 As Integer = 31)

        ft.visualizarObjetos(Ver, lblLapso, txtDesde1, txtDesde2, txtDesde3, txtDesde4, txtHasta1, txtHasta2, txtHasta3)
        txtDesde1.Text = ft.FormatoEntero(Desde1) : txtHasta1.Text = ft.FormatoEntero(Hasta1)
        txtDesde2.Text = ft.FormatoEntero(Desde2) : txtHasta2.Text = ft.FormatoEntero(Hasta2)
        txtDesde3.Text = ft.FormatoEntero(Desde3) : txtHasta3.Text = ft.FormatoEntero(Hasta3)
        txtDesde4.Text = ft.FormatoEntero(Desde4)

    End Sub
    Private Sub verConstante_Descuentos(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblDescuentos, cmbDescuentos)
    End Sub
    Private Sub verConstante_EstatusFacturas(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblEstatusFactura, cmbEstatusFacturas)
    End Sub
    Private Sub verConstante_Resumen(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblConsResumen, chkConsResumen)
    End Sub
    Private Sub VerConstante_Tarifas(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblTarifa, chkPrecioA, chkPrecioB, chkPrecioC, chkPrecioD, chkPrecioE, chkPrecioF)
    End Sub
    Private Sub VerConstante_peso(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblPeso, chkPeso)
    End Sub
    Private Sub VerConstante_VerPrimeros(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblPrimeros, txtPrimeros)
    End Sub
    Private Sub VerConstante_VentasConstante(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblConstanteVentas, txtConstanteVentas)
    End Sub
    Private Sub VerConstante_TipoReporte(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblTipoReporte, cmbTipoReporte)
    End Sub
    Private Sub VerConstante_TipoMercancia(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblTipoMercancia, cmbTipoMercancias)
    End Sub
    Private Sub VerConstante_Cartera(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblCartera, cmbCartera)
    End Sub

    Private Sub VerConstante_CondicionPago(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblCondicionPago, cmbCondicionPago)
    End Sub
    Private Sub VerConstante_TipoCliente(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblTipo, cmbTipo)
    End Sub
    Private Sub VerConstante_Regulado(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblRegulada, cmbRegulada)
    End Sub
    Private Sub VerConstante_Estatus(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblEstatus, cmbEstatus)
    End Sub

    Private Sub jsVenRepParametrosPlus_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsVenRepParametros_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(myConn, MovAud.ientrar, sModulo, ReporteNumero)
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        myConn.Close()
        myConn = Nothing
        Me.Close()
    End Sub

    Private Function numGrupo() As Integer

        vGrupos = ""
        numGrupo = 0
        '/////////////// ojojojojoj FALTA EL MIX (DEBE SER SELECCIONADO EN UN COMBOBOX COMO CRITERIO
        If lblCanalSeleccion.Text.Trim() <> "" Then
            vGrupos += ", canal "
            numGrupo += 1
        End If

        If lblTipoNegocioSeleccion.Text.Trim() <> "" Then
            vGrupos += ", tiponegocio "
            numGrupo += 1
        End If

        If lblzonaSeleccion.Text.Trim() <> "" Then
            vGrupos += ", zona "
            numGrupo += 1
        End If

        If lblRutaSeleccion.Text.Trim() <> "" Then
            vGrupos += ", ruta "
            numGrupo += 1
        End If

        If lblAsesorSeleccion.Text.Trim() <> "" Then
            vGrupos += ", asesor "
            numGrupo += 1
        End If

        If lblCategoriaSeleccion.Text.Trim() <> "" Then
            vGrupos += ", categoria "
            numGrupo += 1
        End If

        If lblMarcasSeleccion.Text.Trim() <> "" Then
            vGrupos += ", marca "
            numGrupo += 1
        End If

        If lblDivisionesSeleccion.Text.Trim() <> "" Then
            vGrupos += ", division "
            numGrupo += 1
        End If

        If lblJerarquiasSeleccion.Text.Trim() <> "" Then
            vGrupos += ", tipjer "
            numGrupo += 1
        End If


    End Function

    Private Function prepararCadena(str As String, cmb As ComboBox) As String
        Return IIf(str.Trim() = "", "", IIf(cmb.SelectedIndex = 0, " IN ( ", " NOT IN ( ") & str & ") ")
    End Function
    Private Sub IniciarCadenas()

        cadClientes = prepararCadena(lblClienteSeleccion.Text, cmbCliente)
        cadCanal = prepararCadena(lblCanalSeleccion.Text, cmbCanal)
        cadTipoNegocio = prepararCadena(lblTipoNegocioSeleccion.Text, cmbTipoNegocio)
        cadZona = prepararCadena(lblzonaSeleccion.Text, cmbZona)
        cadRuta = prepararCadena(lblRutaSeleccion.Text, cmbRuta)
        cadAsesor = prepararCadena(lblAsesorSeleccion.Text, cmbAsesor)
        cadPais = prepararCadena(lblPaisSeleccion.Text, cmbPais)
        cadEstado = prepararCadena(lblEstadoSeleccion.Text, cmbEstado)
        cadMunicipio = prepararCadena(lblMunicipioSeleccion.Text, cmbMunicipio)
        cadParroquia = prepararCadena(lblParroquiaSeleccion.Text, cmbParroquia)
        cadCiudad = prepararCadena(lblCiudadSeleccion.Text, cmbCiudad)
        cadBarrio = prepararCadena(lblBarrioSeleccion.Text, cmbBarrio)
        cadMercancias = prepararCadena(lblMercanciaSeleccion.Text, cmbMercancias)
        cadCategoria = prepararCadena(lblCategoriaSeleccion.Text, cmbCategoria)
        cadMarca = prepararCadena(lblMarcasSeleccion.Text, cmbMarca)
        cadDivision = prepararCadena(lblDivisionesSeleccion.Text, cmbDivision)
        cadJerarquia = prepararCadena(lblJerarquiasSeleccion.Text, cmbJerarquia)
        cadNivel1 = prepararCadena(lblNivel1Seleccion.Text, cmbNivel1)
        cadNivel2 = prepararCadena(lblNivel2Seleccion.Text, cmbNivel2)
        cadNivel3 = prepararCadena(lblNivel3Seleccion.Text, cmbNivel3)
        cadNivel4 = prepararCadena(lblNivel4Seleccion.Text, cmbNivel4)
        cadNivel5 = prepararCadena(lblNivel5Seleccion.Text, cmbNivel5)
        cadNivel6 = prepararCadena(lblNivel6Seleccion.Text, cmbNivel6)
        cadAlmacen = prepararCadena(lblAlmacenSeleccion.Text, cmbAlmacen)
        cadCategoriaProveedor = prepararCadena(lblCategoriaProveedorSeleccion.Text, cmbCategoriaProveedor)
        cadUnidadProveedor = prepararCadena(lblUnidadProveedorSeleccion.Text, cmbUnidadProveedor)

    End Sub
    Private Function Validado() As Boolean
        If ReporteNumero = ReporteMercancias.cExistenciasAUnaFecha Then
            If cmbTipoReporte.SelectedIndex = 2 Or cmbTipoReporte.SelectedIndex = 4 Then
                ft.mensajeCritico("Tipo de Reporte NO permitido. Seleccione UV, UMP ó UMS...")
                Return False
            End If

        End If
        Return True
    End Function
    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

        If Not Validado() Then Return
        HabilitarCursorEnEspera()

        Dim r As New frmViewer
        Dim dsVen As New DataSet ''dsVentasPlus
        Dim str As String = ""
        Dim strIVA As String = ""
        Dim strDescuentos As String = ""
        Dim strComentarios As String = ""
        Dim strFormaPago As String = ""
        Dim strRelacionCT As String = ""
        Dim nTabla As String = ""
        Dim nTablaIVA As String = ""
        Dim nTablaDescuentos As String = ""
        Dim nTablaComentarios As String = ""
        Dim nTablaFormaPago As String = ""
        Dim nTablaRelacionCT As String = ""
        Dim oReporte As New CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim PresentaArbol As Boolean = False

        IniciarCadenas()


        Select Case ReporteNumero

            Case ReporteVentas.cClientes
                nTabla = "dtClientes"
                Select Case numGrupo()
                    Case 0
                        oReporte = New rptVentasClientes0G
                    Case 1
                        oReporte = New rptVentasClientes1G
                    Case 2
                        oReporte = New rptVentasClientes2G
                    Case 3
                        oReporte = New rptVentasClientes3G
                    Case Else
                        oReporte = New rptVentasClientes0G
                End Select
                str = SeleccionVENPLUS_Clientes(vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cadCanal, _
                                                cadTipoNegocio, cadZona, cadRuta, cadAsesor, cadPais, _
                                                cadEstado, cadMunicipio, cadParroquia, cadCiudad, cadBarrio, _
                                                cadClientes, _
                                                cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex, cmbDescuentos.SelectedIndex)


            Case ReporteVentas.cChequesDevueltos
                nTabla = "dtChequesDevueltos"
                oReporte = New rptVentasChequesDevueltos0G
                str = SeleccionVENPLUS_VEN_ChequesDevueltos(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), _
                                            cadClientes, _
                                            cadCanal, cadTipoNegocio, cadZona, cadRuta, cadAsesor, _
                                            cadPais, cadEstado, cadMunicipio, cadParroquia, cadCiudad, cadBarrio, _
                                            cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex, chkConsResumen.Checked)

            Case ReporteVentas.cActivacionAsesor
                nTabla = "dtActivacionVendedor"
                oReporte = New rptVentasActivacionClientesMercancias0G
                str = SeleccionVENPLUS_ActivacionClientesMercanciasPorAsesor(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), _
                                                cadClientes, _
                                                cadCanal, cadTipoNegocio, cadZona, cadRuta, cadAsesor, _
                                                cadPais, cadEstado, cadMunicipio, cadParroquia, cadCiudad, cadBarrio, _
                                                cadMercancias, _
                                                cadCategoria, cadMarca, cadDivision, cadJerarquia, _
                                                cadNivel1, cadNivel2, cadNivel3, cadNivel4, cadNivel5, cadNivel6)


            Case ReporteMedicionGerencial.cComprasComparativasMesMes

                nTabla = "dtComprasVentasComparativas"
                oReporte = New rptSIGMEComprasVentasComparativas
                str = SeleccionVENPLUS_ComprasComparativasMesMes(cadClientes, cadCategoriaProveedor, cadUnidadProveedor, _
                                                                    cadMercancias, _
                                                                    cadCategoria, cadMarca, cadDivision, cadJerarquia, _
                                                                    cadNivel1, cadNivel2, cadNivel3, cadNivel4, cadNivel5, cadNivel6, _
                                                                    cmbTipoReporte.SelectedIndex)


            Case ReporteVentas.cVentasAsesor
                nTabla = "dtActivacionVendedor"
                oReporte = New rptVentasAsesor0G

                str = SeleccionVENPLUS_VentasAsesor(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), _
                                                cadClientes, _
                                                cadCanal, cadTipoNegocio, cadZona, cadRuta, cadAsesor, _
                                                cadPais, cadEstado, cadMunicipio, cadParroquia, cadCiudad, cadBarrio, _
                                                cadMercancias, _
                                                cadCategoria, cadMarca, cadDivision, cadJerarquia, _
                                                cadNivel1, cadNivel2, cadNivel3, cadNivel4, cadNivel5, _
                                                cadNivel6, cmbTipoReporte.SelectedIndex)

            Case ReporteMedicionGerencial.cDevolucionesPorCausa
                nTabla = "dtActivacionVendedor"
                oReporte = New rptSIGMEDevolucionCausa0G
                str = SeleccionVENPLUS_DevolucionesCausa(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), _
                                                cadClientes, _
                                                cadCanal, cadTipoNegocio, cadZona, cadRuta, cadAsesor, _
                                                cadPais, cadEstado, cadMunicipio, cadParroquia, cadCiudad, cadBarrio, _
                                                cadMercancias, _
                                                cadCategoria, cadMarca, cadDivision, cadJerarquia, _
                                                cadNivel1, cadNivel2, cadNivel3, cadNivel4, cadNivel5, cadNivel6, _
                                                cmbTipoReporte.SelectedIndex)
            Case ReporteMedicionGerencial.cVentasComparativasMesMes
                nTabla = "dtComprasVentasComparativas"
                oReporte = New rptSIGMEComprasVentasComparativas
                str = SeleccionVENPLUS_VentasComparativasMesMes(cadClientes, _
                                           cadCanal, cadTipoNegocio, cadZona, cadRuta, cadAsesor, _
                                           cadPais, cadEstado, cadMunicipio, cadParroquia, cadCiudad, cadBarrio, _
                                           cadMercancias, _
                                           cadCategoria, cadMarca, cadDivision, cadJerarquia, _
                                           cadNivel1, cadNivel2, cadNivel3, cadNivel4, cadNivel5, cadNivel6, _
                                           cmbTipoReporte.SelectedIndex)

            Case ReporteMedicionGerencial.cVentasMesMesAsesor
                nTabla = "dtVentasMesMes"
                oReporte = New rptSIGMEVentasMesMes0G
                str = SeleccionVENPLUS_VentasAsesorMesMes(cadClientes, _
                                           cadCanal, cadTipoNegocio, cadZona, cadRuta, cadAsesor, _
                                           cadPais, cadEstado, cadMunicipio, cadParroquia, cadCiudad, cadBarrio, _
                                           cadMercancias, _
                                           cadCategoria, cadMarca, cadDivision, cadJerarquia, _
                                           cadNivel1, cadNivel2, cadNivel3, cadNivel4, cadNivel5, cadNivel6, _
                                            cmbAño.SelectedIndex + 2000, cmbTipoReporte.SelectedIndex)


            Case ReporteVentas.cVentasPorClientesPlus
                nTabla = "dtVentasVentasYActivacionPlus"
                oReporte = New rptVentasVentasYActivacionPlus0G
                str = SeleccionVENPLUS_VentasClientesAct(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), _
                                           cadClientes, _
                                           cadCanal, cadTipoNegocio, cadZona, cadRuta, cadAsesor, _
                                           cadPais, cadEstado, cadMunicipio, cadParroquia, cadCiudad, cadBarrio, _
                                           cadMercancias, _
                                           cadCategoria, cadMarca, cadDivision, cadJerarquia, _
                                           cadNivel1, cadNivel2, cadNivel3, cadNivel4, cadNivel5, cadNivel6, _
                                           cadAlmacen,
                                           cmbTipoReporte.SelectedIndex, chkPeso.Checked)

            Case ReporteVentas.cActivacionesClientesMercas
                nTabla = "dtClientesMercancias"
                oReporte = New rptVentasActivacionesClientesMercanciasPLUS
                str = SeleccionVENPLUS_ActivacionesClientesYMercancias(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), _
                                           cadClientes, _
                                           cadCanal, cadTipoNegocio, cadZona, cadRuta, cadAsesor, _
                                           cadPais, cadEstado, cadMunicipio, cadParroquia, cadCiudad, cadBarrio, _
                                           cadMercancias, _
                                           cadCategoria, cadMarca, cadDivision, cadJerarquia, _
                                           cadNivel1, cadNivel2, cadNivel3, cadNivel4, cadNivel5, cadNivel6, _
                                           cadAlmacen,
                                           cmbTipoReporte.SelectedIndex, _
                                           cmbEstatusFacturas.SelectedIndex, cmbEstatus.SelectedIndex)

            Case ReporteVentas.cVentasPorClienteMesMes
                nTabla = "dtVentasClientesMesMes"
                oReporte = New rptVentasClientesMesMes0G
                str = SeleccionVENPLUS_VEN_VentasClientesMes(cadClientes, _
                                           cadCanal, cadTipoNegocio, cadZona, cadRuta, cadAsesor, _
                                           cadPais, cadEstado, cadMunicipio, cadParroquia, cadCiudad, cadBarrio, _
                                           cmbTipoReporte.SelectedIndex, _
                                           cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex)

            Case ReporteMercancias.cInventarioLegal
                nTabla = "dtInventarioLegal"
                oReporte = New rptMercanciaIventarioLegalSG
                str = SeleccionVENPLUS_MER_InventarioLegalPlus(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), _
                                                    cadMercancias, _
                                                    cadCategoria, cadMarca, cadDivision, cadJerarquia, _
                                                    cadNivel1, cadNivel2, cadNivel3, cadNivel4, cadNivel5, cadNivel6, _
                                                    cadAlmacen, cmbTipoReporte.SelectedIndex, vOrdenCampos(cmbOrdenadoPor.SelectedIndex))

            Case ReporteVentas.cVentasClienteDivision
                nTabla = "dtVentasClientesDivision"
                oReporte = New rptVentasClientesDivision0G
                str = SeleccionVENPLUS_VEN_VentasPorDivision(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), _
                                                             cadClientes, _
                                                             cadCanal, cadTipoNegocio, cadZona, cadRuta, cadAsesor, _
                                                             cadPais, cadEstado, cadMunicipio, cadParroquia, cadCiudad, cadBarrio, _
                                                             cmbTipoReporte.SelectedIndex, _
                                                             cmbEstatus.SelectedIndex)

            Case ReporteMedicionGerencial.cActivacionClientesMesMes

                nTabla = "dtActivacionMesMes"
                oReporte = New rptSIGMEActivacionMesMes0GPlus
                str = SeleccionVENPLUS_VEN_ActivacionMESMES(cmbAño.SelectedIndex + 2000, _
                                                             cadClientes, _
                                                             cadCanal, cadTipoNegocio, cadZona, cadRuta, cadAsesor, _
                                                             cadPais, cadEstado, cadMunicipio, cadParroquia, cadCiudad, cadBarrio, _
                                                             cadMercancias, _
                                                             cadCategoria, cadMarca, cadDivision, cadJerarquia, _
                                                             cadNivel1, cadNivel2, cadNivel3, cadNivel4, cadNivel5, cadNivel6, _
                                                             cadAlmacen, _
                                                             False)
                '////////////////////MERCANCIAS 
            Case ReporteMercancias.cCatalogo

                nTabla = "dtMercancias"
                oReporte = New rptMercanciaCatalogoSG
                str = SeleccionMERCAS_Mercancias(cadMercancias, _
                                                cadCategoria, cadMarca, cadDivision, cadJerarquia, _
                                                cadNivel1, cadNivel2, cadNivel3, cadNivel4, cadNivel5, cadNivel6, _
                                                cadAlmacen, cmbEstatus.SelectedIndex, cmbCartera.SelectedIndex, chkPeso.Checked, _
                                                cmbTipoMercancias.SelectedIndex, cmbRegulada.SelectedIndex)
            Case ReporteMercancias.cExistenciasAUnaFecha
                nTabla = "dtMercancias"
                Select Case numGrupo()
                    Case 0
                        oReporte = New rptMercanciaExistenciasSG
                    Case 1
                        oReporte = New rptMercanciaExistencias1G
                    Case 2
                        oReporte = New rptMercanciaExistencias2G
                    Case 3
                        oReporte = New rptMercanciaExistencias3G
                    Case Else
                        oReporte = New rptMercanciaExistenciasSG
                End Select

                str = SeleccionMERCAS_ExistenciasAFecha(myConn, lblInfo, _
                                                        cadMercancias, cadCategoria, cadMarca, cadDivision, cadJerarquia, _
                                                        cadNivel1, cadNivel2, cadNivel3, cadNivel4, cadNivel5, cadNivel6, _
                                                        cadAlmacen, _
                                                        CDate(txtPeriodoHasta.Text), cmbTipoReporte.SelectedIndex, ValorNumero(txtConstanteVentas.Text), _
                                                        vOrdenCampos(cmbOrdenadoPor.SelectedIndex), _
                                                        cmbEstatus.SelectedIndex, cmbCartera.SelectedIndex, _
                                                        chkPeso.Checked, cmbTipoMercancias.SelectedIndex, cmbRegulada.SelectedIndex, _
                                                        cmbDescuentos.SelectedIndex)
            Case ReporteMercancias.cEquivalencias
                nTabla = "dtEquivalencia"
                oReporte = New rptMercanciaEquivalenciasSG
                str = SeleccionMERCAS_MercanciasYEquivalencias(cadMercancias, cadCategoria, cadMarca, cadDivision, cadJerarquia, _
                                                                cadNivel1, cadNivel2, cadNivel3, cadNivel4, cadNivel5, cadNivel6, _
                                                                vOrdenCampos(cmbOrdenadoPor.SelectedIndex), _
                                                                cmbEstatus.SelectedIndex, cmbCartera.SelectedIndex, _
                                                                chkPeso.Checked, cmbTipoMercancias.SelectedIndex, cmbRegulada.SelectedIndex)

            Case Else

                oReporte = Nothing

        End Select

        If nTabla <> "" Then
            dsVen = DataSetRequery(dsVen, str, myConn, nTabla, lblInfo)

            '/////////// SUBREPORTES
            'Select Case ReporteNumero
            '    Case ReporteVentas.cPresupuesto, ReporteVentas.cPrePedido, ReporteVentas.cPedido, ReporteVentas.cNotaDeEntrega, _
            '        ReporteVentas.cFactura, ReporteVentas.cNotaDebito, ReporteVentas.cNotaCredito

            '        dsVen = DataSetRequery(dsVen, strIVA, myConn, nTablaIVA, lblInfo)
            '        If strDescuentos <> "" Then dsVen = DataSetRequery(dsVen, strDescuentos, myConn, nTablaDescuentos, lblInfo)
            '        dsVen = DataSetRequery(dsVen, strComentarios, myConn, nTablaComentarios, lblInfo)
            '    Case ReporteVentas.cCxC
            '        dsVen = DataSetRequery(dsVen, strFormaPago, myConn, nTablaFormaPago, lblInfo)
            '        dsVen = DataSetRequery(dsVen, strRelacionCT, myConn, nTablaRelacionCT, lblInfo)
            'End Select
            If dsVen.Tables(nTabla).Rows.Count > 0 Then

                oReporte = PresentaReporte(oReporte, dsVen, nTabla)
                r.CrystalReportViewer1.ReportSource = oReporte
                r.CrystalReportViewer1.ToolPanelView = IIf(PresentaArbol, CrystalDecisions.Windows.Forms.ToolPanelViewType.GroupTree, _
                                              CrystalDecisions.Windows.Forms.ToolPanelViewType.None)
                r.CrystalReportViewer1.ShowGroupTreeButton = PresentaArbol
                r.CrystalReportViewer1.Zoom(1)
                r.CrystalReportViewer1.Refresh()
                r.Cargar(ReporteNombre)
                DeshabilitarCursorEnEspera()
            Else
                If ReporteNumero = ReporteVentas.cPlanillaCliente Then
                    oReporte = PresentaReporte(oReporte, dsVen, nTabla)
                    r.CrystalReportViewer1.ReportSource = oReporte
                    r.CrystalReportViewer1.ToolPanelView = IIf(PresentaArbol, CrystalDecisions.Windows.Forms.ToolPanelViewType.GroupTree, _
                                                  CrystalDecisions.Windows.Forms.ToolPanelViewType.None)
                    r.CrystalReportViewer1.ShowGroupTreeButton = PresentaArbol
                    r.CrystalReportViewer1.Zoom(1)
                    r.CrystalReportViewer1.Refresh()
                    r.Cargar(ReporteNombre)
                    DeshabilitarCursorEnEspera()
                Else
                    ft.MensajeCritico("No existe información que cumpla con estos criterios y/o constantes ")
                End If

            End If
        End If

        r.Close()
        r = Nothing
        oReporte.Close()
        oReporte = Nothing

    End Sub
    Private Function PresentaReporte(ByVal oReporte As CrystalDecisions.CrystalReports.Engine.ReportClass, _
                                     ByVal ds As DataSet, ByVal nTabla As String) As CrystalDecisions.CrystalReports.Engine.ReportClass

        Try


            Dim rif As String
            Dim nCampos() As String = {"id_emp"}
            Dim nString() As String = {jytsistema.WorkID}
            Dim CaminoImagen As String = ""
            Dim dtEmpresa As DataTable
            Dim nTablaEmpresa As String = "tblEmpresa"
            Dim strSQLEmpresa As String = " select id_emp, rif, logo from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' "

            ds = DataSetRequery(ds, strSQLEmpresa, myConn, nTablaEmpresa, lblInfo)
            dtEmpresa = ds.Tables(nTablaEmpresa)

            With dtEmpresa.Rows(0)
                rif = .Item("RIF").ToString
                CaminoImagen = BaseDatosAImagen(dtEmpresa.Rows(0), "logo", .Item("id_emp").ToString)
            End With
            dtEmpresa.Dispose()
            dtEmpresa = Nothing

            Select Case ReporteNumero
                Case ReporteVentas.cClientes
                    oReporte.ReportDefinition.Sections("DetailSection3").SectionFormat.EnableSuppress = chkConsResumen.Checked

                    '    Case ReporteVentas.cPresupuesto, ReporteVentas.cPrePedido, ReporteVentas.cPedido, ReporteVentas.cNotaDeEntrega, _
                    '        ReporteVentas.cFactura, ReporteVentas.cNotaDebito, ReporteVentas.cNotaCredito
                    '        oReporte.Subreports("rptGENIVA.rpt").SetDataSource(ds.Tables("dtIVA"))
                    '        If ReporteNumero <> ReporteVentas.cNotaCredito And ReporteNumero <> ReporteVentas.cNotaDebito Then _
                    '            oReporte.Subreports("rptGENDescuentos.rpt").SetDataSource(ds.Tables("dtDescuentos"))
                    '        oReporte.Subreports("rptGENComentarios.rpt").SetDataSource(ds.Tables("dtComentarios"))
                    '    Case ReporteVentas.cCxC
                    '        oReporte.Subreports("rptGENFormaDePago.rpt").SetDataSource(ds.Tables("dtCXCFormaPago"))
                    '        oReporte.Subreports("rptGENRelacionCT.rpt").SetDataSource(ds.Tables("dtCXCRelacionCT"))
            End Select

            oReporte.SetDataSource(ds)
            oReporte.Refresh()
            oReporte.SetParameterValue("strLogo", CaminoImagen)
            oReporte.SetParameterValue("Orden", IIf(ReporteNumero = ReporteVentas.cLibroIVA, "", "Ordenado por : " + cmbOrdenadoPor.Text)) '' + " " + txtOrdenDesde.Text + "/" + txtOrdenHasta.Text))
            oReporte.SetParameterValue("RIF", "RIF : " + rif)
            oReporte.SetParameterValue("Grupo", LineaGrupos)
            oReporte.SetParameterValue("Criterios", LineaCriterios)
            oReporte.SetParameterValue("Constantes", LineaConstantes)
            oReporte.SetParameterValue("Empresa", jytsistema.WorkName.TrimEnd(" "))

            Select Case ReporteNumero
                '///////////////////////// MEDICION GERENCIAL  
                Case ReporteMedicionGerencial.cActivacionClientesMesMes
                    oReporte.SetParameterValue("Titulo", "ACTIVACION CLIENTES MES A MES")
                Case ReporteMedicionGerencial.cVentasComparativasMesMes
                    oReporte.SetParameterValue("Titulo", "VENTAS COMPARATIVAS MES A MES")
                    oReporte.SetParameterValue("AñoAnterior", CStr(Year(jytsistema.sFechadeTrabajo) - 1))
                    oReporte.SetParameterValue("AñoActual", CStr(Year(jytsistema.sFechadeTrabajo)))
                    oReporte.SetParameterValue("Resumido", chkConsResumen.Checked)
                Case ReporteMedicionGerencial.cComprasComparativasMesMes
                    oReporte.SetParameterValue("Titulo", "COMPRAS COMPARATIVAS MES A MES")
                    oReporte.SetParameterValue("AñoAnterior", CStr(Year(jytsistema.sFechadeTrabajo) - 1))
                    oReporte.SetParameterValue("AñoActual", CStr(Year(jytsistema.sFechadeTrabajo)))
                    oReporte.SetParameterValue("Resumido", chkConsResumen.Checked)


                    '///////////////////////// VENTAS Y CUENTAS POR COBRAR 
                Case ReporteVentas.cVentasPorClientesPlus
                    oReporte.SetParameterValue("Resumido", chkConsResumen.Checked)
                Case ReporteVentas.cActivacionesClientesMercas
                    oReporte.SetParameterValue("Resumen", chkConsResumen.Checked)
                Case ReporteVentas.cVentasClienteDivision
                    oReporte.SetParameterValue("Division1", ft.DevuelveScalarCadena(myConn, "select descrip from jsmercatdiv where division = '00001' and id_emp = '" & jytsistema.WorkID & "' ").ToString)
                    oReporte.SetParameterValue("Division2", ft.DevuelveScalarCadena(myConn, "select descrip from jsmercatdiv where division = '00002' and id_emp = '" & jytsistema.WorkID & "' ").ToString)
                    oReporte.SetParameterValue("Division3", ft.DevuelveScalarCadena(myConn, "select descrip from jsmercatdiv where division = '00003' and id_emp = '" & jytsistema.WorkID & "' ").ToString)
                    oReporte.SetParameterValue("Division4", ft.DevuelveScalarCadena(myConn, "select descrip from jsmercatdiv where division = '00004' and id_emp = '" & jytsistema.WorkID & "' ").ToString)
                    oReporte.SetParameterValue("Division5", ft.DevuelveScalarCadena(myConn, "select descrip from jsmercatdiv where division = '00005' and id_emp = '" & jytsistema.WorkID & "' ").ToString)
                    oReporte.SetParameterValue("Division6", ft.DevuelveScalarCadena(myConn, "select descrip from jsmercatdiv where division = '00006' and id_emp = '" & jytsistema.WorkID & "' ").ToString)
                    oReporte.SetParameterValue("Division7", ft.DevuelveScalarCadena(myConn, "select descrip from jsmercatdiv where division = '00007' and id_emp = '" & jytsistema.WorkID & "' ").ToString)

                Case ReporteVentas.cClientes, ReporteMercancias.cExistenciasAUnaFecha

                    If ReporteNumero = ReporteMercancias.cExistenciasAUnaFecha Then
                        oReporte.SetParameterValue("Totales", chkConsResumen.Checked)
                        oReporte.SetParameterValue("Unidad", cmbTipoReporte.SelectedIndex)
                    End If


                    Dim FieldDef1, FieldDef2, FieldDef3, FieldDef4, FieldDef5 As FieldDefinition
                    Select Case numGrupo()
                        Case 1
                            oReporte.SetParameterValue("Grupo1", "categoria")
                            Dim aFld() As String = vGrupos.Split(",")
                            FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(aFld(1).Trim())
                            oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        Case 2
                            oReporte.SetParameterValue("Grupo1", "categoria")
                            oReporte.SetParameterValue("Grupo2", "marca")
                            Dim aFld() As String = vGrupos.Split(",")
                            FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(1)))
                            FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(2)))
                            oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                            oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        Case 3
                            oReporte.SetParameterValue("Grupo1", "categoria")
                            oReporte.SetParameterValue("Grupo2", "marca")
                            oReporte.SetParameterValue("Grupo3", "tipjer")
                            Dim aFld() As String = vGrupos.Split(",")

                            FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(1)))
                            FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(2)))
                            FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(3)))
                            oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                            oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                            oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3

                        Case 4

                            oReporte.SetParameterValue("Grupo1", "categoria")
                            oReporte.SetParameterValue("Grupo2", "marca")
                            oReporte.SetParameterValue("Grupo3", "tipjer")
                            oReporte.SetParameterValue("Grupo4", "division")
                            Dim aFld() As String = vGrupos.Split(",")
                            FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(1)))
                            FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(2)))
                            FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(3)))
                            FieldDef4 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(4)))
                            oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                            oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                            oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                            oReporte.DataDefinition.Groups.Item(3).ConditionField = FieldDef4


                        Case 5

                            oReporte.SetParameterValue("Grupo1", "categoria")
                            oReporte.SetParameterValue("Grupo2", "marca")
                            oReporte.SetParameterValue("Grupo3", "tipjer")
                            oReporte.SetParameterValue("Grupo4", "division")
                            oReporte.SetParameterValue("Grupo5", "mix")
                            Dim aFld1() As String = vGrupos.Split(",")
                            FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(1)))
                            FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(2)))
                            FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(3)))
                            FieldDef4 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(4)))
                            FieldDef5 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(5)))
                            oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                            oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                            oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                            oReporte.DataDefinition.Groups.Item(3).ConditionField = FieldDef4
                            oReporte.DataDefinition.Groups.Item(4).ConditionField = FieldDef5


                    End Select


            End Select
        Catch ex As Exception

        End Try

        Return oReporte


    End Function
    Private Sub btnLimpiar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLimpiar.Click
        Limpiar()
    End Sub
    Private Sub Limpiar()

        LimpiarTextos(lblCanalSeleccion, lblTipoNegocioSeleccion, lblzonaSeleccion, lblRutaSeleccion, lblAsesorSeleccion,
                    lblPaisSeleccion, lblEstadoSeleccion, lblMunicipioSeleccion, lblParroquiaSeleccion, lblCiudadSeleccion, lblBarrioSeleccion,
                    lblCategoriaSeleccion, lblMarcasSeleccion, lblDivisionesSeleccion, lblJerarquiasSeleccion, lblClienteSeleccion, lblMercanciaSeleccion,
                    lblNivel1Seleccion, lblNivel2Seleccion, lblNivel3Seleccion, lblNivel4Seleccion, lblNivel5Seleccion, lblNivel6Seleccion)
        LimpiarTextos(lblAlmacenSeleccion, lblClienteSeleccion, lblMercanciaSeleccion, lblTipoDocumentoSeleccion)

    End Sub

    Private Function LineaGrupos() As String
        LineaGrupos = ""
        If ReporteNumero = ReporteVentas.cLibroIVA Then
            LineaGrupos = "Dirección comercial y fiscal :" _
                & ft.DevuelveScalarCadena(myConn, " select dirfiscal from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ") _
                & ". Telefonos : " & ft.DevuelveScalarCadena(myConn, " select telef1 from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ") & ". " _
                & ft.DevuelveScalarCadena(myConn, " select telef2 from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ") & ". " _
                & ft.DevuelveScalarCadena(myConn, " select fax from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ") & ". e-mail : " _
                & ft.DevuelveScalarCadena(myConn, " select email from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ")
        Else

            If lblCanalSeleccion.Text <> "" Then LineaGrupos += " Canal: " & lblCanalSeleccion.Text
            If lblTipoNegocioSeleccion.Text <> "" Then LineaGrupos += "   Tipo Negocio: " & lblTipoNegocioSeleccion.Text
            If lblzonaSeleccion.Text <> "" Then LineaGrupos += "  Zona: " & lblzonaSeleccion.Text
            If lblRutaSeleccion.Text <> "" Then LineaGrupos += "  Ruta: " & lblRutaSeleccion.Text
            If lblAsesorSeleccion.Text <> "" Then LineaGrupos += "  Asesor: " & lblAsesorSeleccion.Text



            If lblCategoriaSeleccion.Text <> "" Then LineaGrupos += "  Categorías : " & lblCategoriaSeleccion.Text
            If lblMarcasSeleccion.Text <> "" Then LineaGrupos += "  Marcas : " & lblMarcasSeleccion.Text
            If lblDivisionesSeleccion.Text <> "" Then LineaGrupos += "  División : " & lblDivisionesSeleccion.Text
            If lblJerarquiasSeleccion.Text <> "" Then LineaGrupos += "  Jerarquía : " & lblJerarquiasSeleccion.Text

            If lblNivel1Seleccion.Text <> "" Then LineaGrupos += "  Nivel 1 : " & lblNivel1Seleccion.Text
            If lblNivel2Seleccion.Text <> "" Then LineaGrupos += "  Nivel 2 : " & lblNivel2Seleccion.Text
            If lblNivel3Seleccion.Text <> "" Then LineaGrupos += "  Nive3 3 : " & lblNivel3Seleccion.Text
            If lblNivel4Seleccion.Text <> "" Then LineaGrupos += "  Nivel 4 : " & lblNivel4Seleccion.Text
            If lblNivel5Seleccion.Text <> "" Then LineaGrupos += "  Nivel 5 : " & lblNivel5Seleccion.Text
            If lblNivel6Seleccion.Text <> "" Then LineaGrupos += "  Nivel 6 : " & lblNivel6Seleccion.Text

        End If

        If LineaGrupos.Length > 120 Then
            Return LineaGrupos.Substring(0, 119)
        Else
            Return LineaGrupos
        End If

    End Function
    Private Function LineaCriterios() As String
        LineaCriterios = ""
        If ReporteNumero = ReporteVentas.cLibroIVA Then
            LineaCriterios = " - MES : " & UCase(Format(CDate(txtPeriodoDesde.Text), "MMMM")) &
                            " - AÑO : " & CDate(txtPeriodoHasta.Text).Year.ToString
        Else
            If lblPeriodo.Visible Then LineaCriterios += " - Período: " & IIf(lblPeriodoDesde.Visible, txtPeriodoDesde.Text, "") & IIf(lblPeriodoDesde.Visible AndAlso lblPeriodoHasta.Visible, "/", "") & IIf(lblPeriodoHasta.Visible, txtPeriodoHasta.Text, "")
            If lblTipodocumento.Visible Then LineaCriterios += " - Tipo Documentos : " + lblTipoDocumentoSeleccion.Text
            If lblcliente.Visible Then LineaCriterios += " - Clientes : " + lblClienteSeleccion.Text
            If lblMercancia.Visible Then LineaCriterios += " - Mercancias : " + lblMercanciaSeleccion.Text
        End If

        If LineaCriterios.Length >= 3 Then LineaCriterios = LineaCriterios.Substring(3)
        Return LineaCriterios

    End Function
    Private Function LineaConstantes() As String

        LineaConstantes = ""
        If lblConsResumen.Visible Then LineaConstantes += " - " + lblConsResumen.Text + " : " + IIf(chkConsResumen.Checked, "Si", "No")
        If lblCondicionPago.Visible Then LineaConstantes += " - Condición Pago : " + aCondicionPago(cmbCondicionPago.SelectedIndex)
        If lblEstatusFactura.Visible Then LineaConstantes += " - " + lblEstatusFactura.Text + cmbEstatusFacturas.SelectedItem.ToString
        If lblTipo.Visible Then LineaConstantes += " - Tipo Cliente : " + aTipo(cmbTipo.SelectedIndex)
        If lblTipoMercancia.Visible Then LineaConstantes += " - Tipo Mercancía : " + aTipoMercancia(cmbTipoMercancias.SelectedIndex)
        If lblEstatus.Visible Then LineaConstantes += " - Estatus : " + IIf(OrigenReporte(ReporteNumero) = " VEN ", aEstatusClientes(cmbEstatus.SelectedIndex), aEstatusMercancias(cmbEstatus.SelectedIndex))
        If lblLapso.Visible Then LineaConstantes += " - Lapsos: 1. " + txtDesde1.Text + "-" + txtHasta1.Text +
                                                    " 2. " + txtDesde2.Text + "-" + txtHasta2.Text +
                                                    " 3. " + txtDesde3.Text + "-" + txtHasta3.Text +
                                                    " 4. " + txtDesde4.Text
        If lblConstanteVentas.Visible Then LineaConstantes += " - " + lblConstanteVentas.Text + txtConstanteVentas.Text
        If lblTipoReporte.Visible Then LineaConstantes += " - Tipo Reporte : " & aTipoReporte(cmbTipoReporte.SelectedIndex)

        If LineaConstantes.Length >= 3 Then LineaConstantes = LineaConstantes.Substring(3)
        Return LineaConstantes

    End Function
    Private Function SubeSeleccion(textoSeleccion As String) As String()
        SubeSeleccion = {}
        If textoSeleccion.Trim <> "" Then
            SubeSeleccion = Replace(textoSeleccion, "'", "").Split(",")
        End If
    End Function
    Private Function bajaSeleccion(arraySeleccion() As String) As String
        bajaSeleccion = ""
        If arraySeleccion.Length > 0 Then bajaSeleccion = "'" & String.Join("','", arraySeleccion) & "'"
    End Function
    Private Sub btnCanal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCanal.Click
        AbrirTabla("CANALES DE DISTRIBUCIÓN", "  select 0 sel,  codigo, descrip from jsvenliscan where " _
                   & " ID_EMP = '" & jytsistema.WorkID & "' order by descrip ", lblCanalSeleccion)
    End Sub

    Private Sub btnTipoNegocio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoNegocio.Click
        If lblCanalSeleccion.Text.Trim = "" Then
            ft.mensajeCritico("Debe indicar uno ó varios canales de distribución ")
        Else
            AbrirTabla("TIPOS DE NEGOCIO", "  select 0 sel,  codigo, descrip from jsvenlistip where " _
                                & " ANTEC IN (" & lblCanalSeleccion.Text & ") AND " _
                                & " ID_EMP = '" & jytsistema.WorkID & "' order by descrip ", lblTipoNegocioSeleccion)
        End If
    End Sub

    Private Sub btnZona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZona.Click
        AbrirTabla("ZONAS CLIENTES", "  select 0 sel,  codigo, descrip from jsconctatab where " _
                            & " MODULO = '" & FormatoTablaSimple(Modulo.iZonasClientes) & "' AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "' order by descrip ", lblzonaSeleccion)
    End Sub
    Private Sub btnRuta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRuta.Click
        AbrirTabla("RUTAS CLIENTES", " select 0 SEL, codrut CODIGO, nomrut DESCRIP from jsvenencrut " _
                            & " where " _
                            & " tipo = 0 and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by nomrut ", lblRutaSeleccion)
    End Sub
    Private Sub btnAsesor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesor.Click
        AbrirTabla("ASESORES COMERCIALES", " select 0 SEL, codven codigo, concat(apellidos,', ',nombres) descrip from jsvencatven where " _
                            & " CLASE = 0 AND id_emp = '" & jytsistema.WorkID & "' ORDER BY 2 ", lblAsesorSeleccion)
    End Sub
    Private Sub btnCategoria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoria.Click
        AbrirTabla("CATEGORIAS DE MERCANCÍAS", "  select 0 sel,  codigo, descrip from jsconctatab where " _
                            & " MODULO = '" & FormatoTablaSimple(Modulo.iCategoriaMerca) & "' AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "' order by descrip ", lblCategoriaSeleccion)
    End Sub

    Private Sub btnMarca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarca.Click
        AbrirTabla("MARCAS DE MERCANCÍAS", "  select 0 sel,  codigo, descrip from jsconctatab where " _
                            & " MODULO = '" & FormatoTablaSimple(Modulo.iMarcaMerca) & "' AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "' order by descrip ", lblMarcasSeleccion)
    End Sub


    Private Sub btnDivisionDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDivision.Click
        AbrirTabla("DIVISIONES DE MERCANCÍAS", " select 0 SEL, division codigo, descrip from jsmercatdiv " _
                            & " where  id_emp = '" & jytsistema.WorkID & "' order by descrip", lblDivisionesSeleccion)
    End Sub

    Private Sub btnTipoJerarquia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoJerarquia.Click
        AbrirTabla("TIPO JERARQUIA DE MERCANCÍAS", " SELECT 0 sel, tipjer codigo, descrip " _
                    & " FROM jsmerencjer WHERE  id_emp  = '" & jytsistema.WorkID & "' order by descrip ", lblJerarquiasSeleccion)
    End Sub
    Private Sub btnCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCliente.Click

        Select Case ReporteNumero
            Case ReporteMedicionGerencial.cComprasComparativasMesMes
                AbrirTabla("PROVEEDORES", " SELECT 0 sel, CODPRO codigo, nombre descrip " _
                           & " FROM jsprocatpro " _
                           & " WHERE  " _
                           & IIf(lblCategoriaProveedorSeleccion.Text.Trim <> "", " CATEGORIA IN (" & lblCategoriaProveedorSeleccion.Text & ") AND ", "") _
                           & IIf(lblUnidadProveedorSeleccion.Text.Trim <> "", " UNIDAD IN (" & lblTipoNegocioSeleccion.Text & ") AND ", "") _
                           & " id_emp  = '" & jytsistema.WorkID & "' order by NOMBRE ", lblClienteSeleccion)
            Case Else
                AbrirTabla("CLIENTES", " SELECT 0 sel, codcli codigo, nombre descrip " _
                           & " FROM jsvencatcli " _
                           & " WHERE  " _
                           & IIf(lblCanalSeleccion.Text.Trim <> "", " CATEGORIA IN (" & lblCanalSeleccion.Text & ") AND ", "") _
                           & IIf(lblTipoNegocioSeleccion.Text.Trim <> "", " UNIDAD IN (" & lblTipoNegocioSeleccion.Text & ") AND ", "") _
                           & IIf(lblzonaSeleccion.Text.Trim <> "", " ZONA IN (" & lblzonaSeleccion.Text & ") AND ", "") _
                           & IIf(lblRutaSeleccion.Text.Trim <> "", " RUTA_VISITA IN (" & lblRutaSeleccion.Text & ") AND ", "") _
                           & IIf(lblAsesorSeleccion.Text.Trim <> "", " VENDEDOR IN (" & lblAsesorSeleccion.Text & ") AND ", "") _
                           & " id_emp  = '" & jytsistema.WorkID & "' order by nombre ", lblClienteSeleccion)

        End Select

    End Sub


    Private Sub chkList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        'Dim iCont As Integer
        'txtTipDoc.Text = ""
        'For iCont = 0 To chkList.Items.Count - 1
        '    If chkList.GetItemCheckState(iCont) = CheckState.Checked Then
        '        txtTipDoc.Text += "." + chkList.Items(iCont).ToString
        '    End If
        'Next

    End Sub


    Private Sub CargarJerarquia(ByVal MyConn As MySqlConnection, ByVal ds As DataSet, ByVal TipoJerarquia As String, ByVal Nivel As Integer,
                                    ByVal txtCodjer As TextBox)

        If TipoJerarquia <> "" Then

            Dim strSQLCodjer As String = " SELECT codjer codigo, desjer descripcion FROM jsmerrenjer WHERE tipjer = '" & TipoJerarquia & "' and nivel = " & Nivel & " and id_emp  = '" & jytsistema.WorkID & "' order by 1 "
            Dim dtCJ As DataTable
            Dim nTableCJ As String = "tblCodjer"
            ds = DataSetRequery(ds, strSQLCodjer, MyConn, nTableCJ, lblInfo)
            dtCJ = ds.Tables(nTableCJ)
            If dtCJ.Rows.Count > 0 Then
                Dim f As New jsControlArcTablaSimple

                f.Cargar(" Codigo Jerarquía nivel " & Nivel & " ", ds, dtCJ, nTableCJ, TipoCargaFormulario.iShowDialog, False)
                txtCodjer.Text = f.Seleccion
                f = Nothing
            End If
            dtCJ = Nothing

        End If

    End Sub

    Private Sub AbrirTabla(Titulo As String, strSQL As String, lblSeleccion As Label)
        Dim f As New jsGenListadoSeleccion
        Dim aNombres() As String = {"", "CÓDIGO", "DESCRIPCION"}
        Dim aCampos() As String = {"sel", "codigo", "descrip"}
        Dim aAnchos() As Integer = {20, 80, 150}
        Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Left}
        Dim aFormato() As String = {"", "", ""}

        f.Seleccion = SubeSeleccion(lblSeleccion.Text)
        f.Cargar(myConn, ds, Titulo, strSQL,
            {"sel.entero.1.0", "codigo.cadena.15.0", "descrip.cadena.150.0"}, aNombres, aCampos, aAnchos, aAlineacion, aFormato)
        lblSeleccion.Text = bajaSeleccion(f.Seleccion)

        f.Dispose()
        f = Nothing
    End Sub
    Private Sub btnAlmacen_Click(sender As System.Object, e As System.EventArgs) Handles btnAlmacen.Click
        AbrirTabla("ALMACENES", "  select 0 sel,  codalm codigo, desalm descrip from jsmercatalm  where " _
                            & " ID_EMP = '" & jytsistema.WorkID & "' order by codalm ", lblAlmacenSeleccion)

    End Sub
    Private Sub btnMercancia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMercancia.Click
        AbrirTabla("MERCANCÍAS", "  select 0 sel,  codart codigo, nomart descrip from jsmerctainv  where " _
                            & " ID_EMP = '" & jytsistema.WorkID & "' order by nomart ", lblMercanciaSeleccion)

    End Sub

    Private Sub txtPeriodoDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtPeriodoDesde.ValueChanged
        Select Case periodoTipo
            Case TipoPeriodo.iDiario
                txtPeriodoHasta.Value = txtPeriodoDesde.Value
            Case TipoPeriodo.iSemanal
                txtPeriodoHasta.Value = DateAdd(DateInterval.Day, 7, CDate(txtPeriodoDesde.Text))
            Case TipoPeriodo.iMensual
                txtPeriodoHasta.Value = UltimoDiaMes(CDate(txtPeriodoDesde.Text))
            Case TipoPeriodo.iAnual
                txtPeriodoHasta.Value = UltimoDiaAño(CDate(txtPeriodoDesde.Text))
        End Select
    End Sub


    Private Sub btnAgregaConsulta_Click(sender As System.Object, e As System.EventArgs) Handles btnAgregaConsulta.Click
        If cmbMisConsultas.Text.Trim <> "" Then
            'VALIDAR CONSULTA A INCLUIR
            If ft.DevuelveScalarEntero(myConn, " select COUNT(*) from " _
                                         & " jsconencconsulta " _
                                         & " where " _
                                         & " consulta_nombre = '" & cmbMisConsultas.Text & "' AND " _
                                         & " USUARIO_ID = '" & jytsistema.sUsuario & "' AND " _
                                         & " REPORTE_ID = " & ReporteNumero & " AND " _
                                         & " ID_EMP = '" & jytsistema.WorkID & "' ") = 0 Then

                InsertarConsulta(myConn, cmbMisConsultas.Text, ReporteNumero)
                RellenarConsultas(myConn)

            Else

                'MODIFICAR
                InsertarConsulta(myConn, cmbMisConsultas.Text, ReporteNumero, False)
                RellenarConsultas(myConn, cmbMisConsultas.SelectedIndex)



            End If

        End If

    End Sub

    Private Sub InsertarConsulta(MyConn As MySqlConnection, ConsultaNombre As String, ReporteNumero As Integer, Optional INSERTAR As Boolean = True)

        Dim aID_Objeto() As Object = {lblCanalSeleccion, lblTipoNegocioSeleccion, lblzonaSeleccion, lblRutaSeleccion, lblAsesorSeleccion, _
                                      lblCategoriaSeleccion, lblMarcasSeleccion, lblDivisionesSeleccion, lblJerarquiasSeleccion, _
                                      lblAlmacenSeleccion, lblMercanciaSeleccion, lblClienteSeleccion, lblTipoDocumentoSeleccion}

        Dim CodigoConsulta As String = ""

        If INSERTAR Then

            CodigoConsulta = ft.autoCodigo(MyConn, "CONSULTA_ID", "jsconencconsulta", "", "", 10)
            ft.Ejecutar_strSQL(myconn, " INSERT INTO jsconencconsulta SET " _
               & " CONSULTA_ID = '" & CodigoConsulta & "', " _
               & " CONSULTA_NOMBRE = '" & ConsultaNombre & "',  " _
               & " USUARIO_ID = '" & jytsistema.sUsuario & "' , " _
               & " REPORTE_ID = " & ReporteNumero & ", " _
               & " ID_EMP = '" & jytsistema.WorkID & "' ")

        Else

            CodigoConsulta = cmbMisConsultas.SelectedValue.ToString

        End If

        ft.Ejecutar_strSQL(MyConn, " DELETE FROM jsconrenconsulta " _
                              & " where " _
                              & " CONSULTA_ID = '" & CodigoConsulta & "' AND " _
                              & " ID_EMP = '" & jytsistema.WorkID & "' ")

        For Each aElemen As Object In aID_Objeto
            If aElemen.text.trim() <> "" Then
                ft.Ejecutar_strSQL(myconn, " INSERT INTO jsconrenconsulta SET " _
                    & " CONSULTA_ID = '" & CodigoConsulta & "', " _
                    & " OBJETO_ID = '" & aElemen.Name & "', " _
                    & " OBJETO_TEXTO = '" & aElemen.text.Replace("'", "°") & "',  " _
                    & " ID_EMP = '" & jytsistema.WorkID & "' ")
            End If
        Next



    End Sub


    Private Sub btnEliminaConsulta_Click(sender As System.Object, e As System.EventArgs) Handles btnEliminaConsulta.Click
        ft.Ejecutar_strSQL(myconn, " DELETE FROM jsconencconsulta where " _
                       & " CONSULTA_ID = '" & cmbMisConsultas.SelectedValue & "' AND " _
                       & " ID_EMP = '" & jytsistema.WorkID & "' ")

        ft.Ejecutar_strSQL(myconn, " DELETE FROM jsconrenconsulta where " _
                       & " CONSULTA_ID = '" & cmbMisConsultas.SelectedValue & "' AND " _
                       & " ID_EMP = '" & jytsistema.WorkID & "' ")

        RellenarConsultas(myConn)

    End Sub

    Private Sub cmbMisConsultas_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbMisConsultas.SelectedIndexChanged
        cargarConsultas()
    End Sub

    Private Sub txtConstanteVentas_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtConstanteVentas.KeyPress
        e.Handled = ValidaNumeroEnteroEnTextbox(e)
    End Sub

    Private Sub btnCodjer1_Click(sender As System.Object, e As System.EventArgs) Handles btnCodjer1.Click
        If lblJerarquiasSeleccion.Text <> "" Then _
        AbrirTabla("NIVEL JERARQUICO 1", " SELECT 0 sel, codjer codigo, desjer descrip " _
                    & " FROM jsmerrenjer WHERE tipjer in (" & lblJerarquiasSeleccion.Text & ") AND nivel = 1 AND " _
                    & " id_emp  = '" & jytsistema.WorkID & "' order by descrip ", lblNivel1Seleccion)
    End Sub

    Private Sub btnCodjer2_Click(sender As System.Object, e As System.EventArgs) Handles btnCodjer2.Click
        If lblJerarquiasSeleccion.Text <> "" Then _
        AbrirTabla("NIVEL JERARQUICO 2", " SELECT 0 sel, codjer codigo, desjer descrip " _
                    & " FROM jsmerrenjer WHERE tipjer in (" & lblJerarquiasSeleccion.Text & ") AND nivel = 2 AND " _
                    & " id_emp  = '" & jytsistema.WorkID & "' order by descrip ", lblNivel2Seleccion)
    End Sub

    Private Sub btnCodjer3_Click(sender As System.Object, e As System.EventArgs) Handles btnCodjer3.Click
        If lblJerarquiasSeleccion.Text <> "" Then _
        AbrirTabla("NIVEL JERARQUICO 3", " SELECT 0 sel, codjer codigo, desjer descrip " _
                    & " FROM jsmerrenjer WHERE tipjer in (" & lblJerarquiasSeleccion.Text & ") AND nivel = 3 AND " _
                    & " id_emp  = '" & jytsistema.WorkID & "' order by descrip ", lblNivel3Seleccion)

    End Sub

    Private Sub btnCodjer4_Click(sender As System.Object, e As System.EventArgs) Handles btnCodjer4.Click
        If lblJerarquiasSeleccion.Text <> "" Then _
        AbrirTabla("NIVEL JERARQUICO 4", " SELECT 0 sel, codjer codigo, desjer descrip " _
                    & " FROM jsmerrenjer WHERE tipjer in (" & lblJerarquiasSeleccion.Text & ") AND nivel = 4 AND " _
                    & " id_emp  = '" & jytsistema.WorkID & "' order by descrip ", lblNivel4Seleccion)

    End Sub

    Private Sub btnCodjer5_Click(sender As System.Object, e As System.EventArgs) Handles btnCodjer5.Click
        If lblJerarquiasSeleccion.Text <> "" Then _
        AbrirTabla("NIVEL JERARQUICO 5", " SELECT 0 sel, codjer codigo, desjer descrip " _
                    & " FROM jsmerrenjer WHERE tipjer in (" & lblJerarquiasSeleccion.Text & ") AND nivel = 5 AND " _
                    & " id_emp  = '" & jytsistema.WorkID & "' order by descrip ", lblNivel5Seleccion)
    End Sub

    Private Sub btnCodjer6_Click(sender As System.Object, e As System.EventArgs) Handles btnCodjer6.Click
        If lblJerarquiasSeleccion.Text <> "" Then _
        AbrirTabla("NIVEL JERARQUICO 6", " SELECT 0 sel, codjer codigo, desjer descrip " _
                    & " FROM jsmerrenjer WHERE tipjer in (" & lblJerarquiasSeleccion.Text & ") AND nivel = 6 AND " _
                    & " id_emp  = '" & jytsistema.WorkID & "' order by descrip ", lblNivel6Seleccion)

    End Sub

    Private Sub btnPais_Click(sender As System.Object, e As System.EventArgs) Handles btnPais.Click
        AbrirTabla("PAIS", " SELECT 0 sel, codigo, NOMBRE descrip " _
                     & " FROM jsconcatter WHERE antecesor in (0) AND " _
                     & " id_emp  = '" & jytsistema.WorkID & "' order by 2 ", lblPaisSeleccion)
    End Sub

    Private Sub btnEstado_Click(sender As System.Object, e As System.EventArgs) Handles btnEstado.Click
        If lblPaisSeleccion.Text.Trim() <> "" Then _
        AbrirTabla("ESTADO/DEPARTAMENTO/CANTON", " SELECT 0 sel, codigo, NOMBRE descrip " _
                    & " FROM jsconcatter WHERE antecesor in (" & lblPaisSeleccion.Text & ") AND " _
                    & " id_emp  = '" & jytsistema.WorkID & "' order by 2 ", lblEstadoSeleccion)
    End Sub

    Private Sub btnMunicipio_Click(sender As System.Object, e As System.EventArgs) Handles btnMunicipio.Click
        If lblEstadoSeleccion.Text.Trim() <> "" Then _
         AbrirTabla("MUNICIPIO", " SELECT 0 sel, codigo, NOMBRE descrip " _
                     & " FROM jsconcatter WHERE antecesor in (" & lblEstadoSeleccion.Text & ") AND " _
                     & " id_emp  = '" & jytsistema.WorkID & "' order by 2 ", lblMunicipioSeleccion)
    End Sub

    Private Sub btnParroquia_Click(sender As System.Object, e As System.EventArgs) Handles btnParroquia.Click
        If lblMunicipioSeleccion.Text.Trim() <> "" Then _
        AbrirTabla("PARROQUIA", " SELECT 0 sel, codigo, NOMBRE descrip " _
                    & " FROM jsconcatter WHERE antecesor in (" & lblMunicipioSeleccion.Text & ") AND " _
                    & " id_emp  = '" & jytsistema.WorkID & "' order by 2 ", lblParroquiaSeleccion)

    End Sub

    Private Sub btnCiudad_Click(sender As System.Object, e As System.EventArgs) Handles btnCiudad.Click
        If lblParroquiaSeleccion.Text.Trim() <> "" Then _
         AbrirTabla("CIUDAD", " SELECT 0 sel, codigo, NOMBRE descrip " _
                     & " FROM jsconcatter WHERE antecesor in (" & lblParroquiaSeleccion.Text & ") AND " _
                     & " id_emp  = '" & jytsistema.WorkID & "' order by 2 ", lblCiudadSeleccion)
    End Sub

    Private Sub btnBarrio_Click(sender As System.Object, e As System.EventArgs) Handles btnBarrio.Click
        If lblCiudadSeleccion.Text.Trim() <> "" Then _
        AbrirTabla("BARRIO", " SELECT 0 sel, codigo, NOMBRE descrip " _
                    & " FROM jsconcatter WHERE antecesor in (" & lblCiudadSeleccion.Text & ") AND " _
                    & " id_emp  = '" & jytsistema.WorkID & "' order by 2 ", lblBarrioSeleccion)

    End Sub

    Private Sub btnTipoDocumento_Click(sender As System.Object, e As System.EventArgs) Handles btnTipoDocumento.Click

    End Sub
End Class