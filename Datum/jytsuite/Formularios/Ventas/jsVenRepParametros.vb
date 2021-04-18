Imports MySql.Data.MySqlClient
Imports System.IO
Imports ReportesDeVentas
Imports CrystalDecisions.CrystalReports.Engine
Imports Syncfusion.WinForms.Input
Public Class jsVenRepParametros

    Private Const sModulo As String = "Reportes de ventas y cuentas por cobrar"

    Private ReporteNumero As Integer
    Private ReporteNombre As String
    Private CodigoCliente As String, Documento As String
    Private FechaParametro As Date

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet
    Private ft As New Transportables

    Private vOrdenNombres() As String
    Private vOrdenCampos() As String
    Private vOrdenTipo() As String
    Private vOrdenLongitud() As Integer
    Private IndiceReporte As Integer
    Private strIndiceReporte As String

    Private vGrupos As String = ""

    Private vCXCAgrupadoPor() As String = {"Ninguno", "Canal", "Tipo", "Zona", "Ruta", "Asesor Comercial", "Territorio", "Canal & Tipo Negocio", _
                                           "Zona & Ruta", "Asesor & Canal & Tipo Negocio", "Asesor & Zona & Ruta"}

    Private aCXCAgrupadoPor() As String = {"", "canal", "tiponegocio", "zona", "ruta", "asesor", "territorio", "canal, tiponegocio", _
                                           "zona, ruta", "asesor, canal, tiponegocio", "asesor, zona, ruta"}

    Private vMERAgrupadoPor() As String = {"Ninguno", "Categor�as", "Marcas", "Jerarqu�a", "Divisi�n", "Mix", _
        "Categor�as & Marcas", "Divisi�n & Categor�as &  Marcas", _
        "Categor�as & Jerarqu�as", _
        "Divisi�n & Jerarqu�a", "Mix & Categor�as & Marcas", "Mix & Jerarqu�as", _
        "Mix & Divisi�n", "Mix & Divisi�n & Categor�as & Marcas", "Mix & Divisi�n & Jerarqu�as"}

    Private aMERAgrupadoPor() As String = {"", "categoria", "marca", "tipjer", "division", "mix", _
                                        "categoria, marca", "division, categoria, marca", _
                                        "categoria, tipjer", "division, tipjer", _
                                        "mix, categoria, marca", "mix, tipjer", "mix, division", _
                                        "mix, division, categoria, marca", "mix, division, tipjer"}

    Private aSiNoTodos() As String = {"Si", "No", "Todos"}

    Private aEstatusFacturas() As String = {"Por confirmar", "Confirmadas", "Anuladas", "Todas"}
    Private aCondicionPago() As String = {"Cr�dito", "Contado", "Todos"}
    Private aEstatus() As String = {"Activo", "Bloqueado", "Inactivo", "Desincorporado", "Todos"}
    Private aTipo() As String = {"Ordinario", "Especial", "Formal", "No contribuyente", "Todos"}
    Private aVencimientos() As String = {"Emisi�n", "Vencimiento"}
    Private periodoTipo As TipoPeriodo
    Public Sub Cargar(ByVal TipoCarga As Integer, ByVal numReporte As Integer, ByVal nomReporte As String, _
                      Optional ByVal CodCliente As String = "", Optional ByVal numDocumento As String = "", _
                      Optional ByVal Fecha As Date = #1/1/2012#)

        Me.Dock = DockStyle.Fill
        myConn.Open()

        ReporteNumero = numReporte
        ReporteNombre = nomReporte
        CodigoCliente = CodCliente
        Documento = numDocumento
        FechaParametro = Fecha

        ft.RellenaCombo(aDesde, cmbOrdenDesde)
        ft.RellenaCombo(aHasta, cmbOrdenHasta)

        Dim dates As SfDateTimeEdit() = {txtPeriodoDesde, txtPeriodoHasta}
        SetSizeDateObjects(dates)

        PresentarReporte(numReporte, nomReporte, CodigoCliente)

        If TipoCarga = TipoCargaFormulario.iShow Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If

    End Sub
    Private Sub PresentarReporte(ByVal NumeroReporte As Integer, ByVal NombreDelReporte As String, Optional ByVal CodigoCliente As String = "")
        lblNombreReporte.Text += " - " + NombreDelReporte
        Select Case NumeroReporte
            Case ReporteVentas.cClientes
                Dim vOrdenNombres() As String = {"C�digo Cliente", "Nombre cliente"}
                Dim vOrdenCampos() As String = {"codcli", "nombre"}
                Dim vOrdenTipo() As String = {"S", "S"}
                Dim vOrdenLongitud() As Integer = {15, 250}
                Inicializar(ReporteNombre, True, True, False, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteVentas.cCliente, ReporteVentas.cPlanillaCliente
                Dim vOrdenNombres() As String = {"C�digo Cliente"}
                Dim vOrdenCampos() As String = {"codcli"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteVentas.cListadoFacturas, ReporteVentas.cListadoNotasDeEntrega
                Dim vOrdenNombres() As String = {"N�mero Documento"}
                Dim vOrdenCampos() As String = {"numfac"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteVentas.cLibroIVA
                Dim vOrdenNombres() As String = {"N�mero Operaci�n"}
                Dim vOrdenCampos() As String = {"numfac"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, True, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteVentas.cListadoPrePedidos, ReporteVentas.cListadoPedidos
                Dim vOrdenNombres() As String = {"N�mero Documento"}
                Dim vOrdenCampos() As String = {"numped"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteVentas.cListadoNotasDeCredito
                Dim vOrdenNombres() As String = {"N�mero Documento"}
                Dim vOrdenCampos() As String = {"numncr"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteVentas.cListadoNotasDebito
                Dim vOrdenNombres() As String = {"N�mero Documento"}
                Dim vOrdenCampos() As String = {"numndb"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteVentas.cListadoPresupuestos
                Dim vOrdenNombres() As String = {"N�mero Documento"}
                Dim vOrdenCampos() As String = {"numcot"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteVentas.cSaldos, ReporteVentas.cAuditoriaClientes, ReporteVentas.cVencimientos, ReporteVentas.cVencimientosResumen
                Dim vOrdenNombres() As String = {"C�digo Cliente", "Nombre cliente"}
                Dim vOrdenCampos() As String = {"codcli", "nombre"}
                Dim vOrdenTipo() As String = {"S", "S"}
                Dim vOrdenLongitud() As Integer = {15, 250}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)

            Case ReporteVentas.cFacturasMesAMes, ReporteVentas.cPresupuestosMesMes, ReporteVentas.cPrepedidosMesMes, _
                ReporteVentas.cPedidosMesMes, ReporteVentas.cNotasEntregaMesMes, ReporteVentas.cNotasCreditoMesMes, _
                ReporteVentas.cNotasDebitoMesMes

                Dim vOrdenNombres() As String = {"C�digo Cliente"}
                Dim vOrdenCampos() As String = {"codcli"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)

            Case ReporteVentas.cActivacionesClientesMercas, ReporteVentas.cActivacionesClientes
                Dim vOrdenNombres() As String = {"C�digo Cliente"}
                Dim vOrdenCampos() As String = {"codcli"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)

            Case ReporteVentas.cEstadoDeCuentas, ReporteVentas.cMovimientosClientes
                Dim vOrdenNombres() As String = {"C�digo Cliente", "Nombre cliente"}
                Dim vOrdenCampos() As String = {"codcli", "nombre"}
                Dim vOrdenTipo() As String = {"S", "S"}
                Dim vOrdenLongitud() As Integer = {15, 250}
                Inicializar(ReporteNombre, IIf(CodigoCliente.Trim() = "", True, False), False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteVentas.cMovimientosDocumento
                Dim vOrdenNombres() As String = {"N�mero Documento", "N� Comprobante"}
                Dim vOrdenCampos() As String = {"nummov", "comproba"}
                Dim vOrdenTipo() As String = {"S", "S"}
                Dim vOrdenLongitud() As Integer = {15, 15}
                Inicializar(ReporteNombre, True, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)

            Case ReporteVentas.cPesosPedidos
                Dim vOrdenNombres() As String = {"N�mero Prepedido"}
                Dim vOrdenCampos() As String = {"numped"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteVentas.cPresupuesto, ReporteVentas.cPrePedido, ReporteVentas.cPedido, ReporteVentas.cNotaDeEntrega, _
                ReporteVentas.cFactura, ReporteVentas.cNotaCredito, ReporteVentas.cNotaDebito, ReporteVentas.cFacturasGuiaDespacho, _
                ReporteVentas.cMercanciasGuiaDespacho, ReporteVentas.cRelacionFacturas, ReporteVentas.cRelacionNotasCredito, _
                ReporteVentas.cGuiaPedidosMercancias, ReporteVentas.cGuiaPedidos
                Dim vOrdenNombres() As String = {"N�mero Documento"}
                Dim vOrdenCampos() As String = {"numcot"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)

            Case ReporteVentas.cGuiaCargaPedidos
                Dim vOrdenNombres() As String = {"N�mero Documento"}
                Dim vOrdenCampos() As String = {"numcot"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteVentas.cCxC
                Dim vOrdenNombres() As String = {"N�mero Comprobante"}
                Dim vOrdenCampos() As String = {"comproba"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {25}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteVentas.cCierreDiario, ReporteVentas.cCierreDiarioContado
                Dim vOrdenNombres() As String = {"C�digo Cliente"}
                Dim vOrdenCampos() As String = {"codcli"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {25}
                Inicializar(ReporteNombre, False, False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteVentas.cVentasHEINZ
                Dim vOrdenNombres() As String = {"C�digo Cliente"}
                Dim vOrdenCampos() As String = {"codcli"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {25}
                Inicializar(ReporteNombre, False, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)

        End Select
    End Sub
    Private Sub Inicializar(ByVal nEtiqueta As String, ByVal TabOrden As Boolean, ByVal TabGrupo As Boolean, _
        ByVal TabCriterio As Boolean, ByVal TabConstantes As Boolean, ByVal aNombreOrden() As String, _
        ByVal aCampoOrden() As String, ByVal aTipoOrden() As String, ByVal aLongitudOrden() As Integer, _
        Optional ByVal Trabajador As String = "")


        HabilitarTabs(TabOrden, TabGrupo, TabCriterio, TabConstantes)
        txtOrdenDesde.Text = Trabajador
        txtOrdenHasta.Text = Trabajador
        IniciarOrden(aNombreOrden, aCampoOrden, aTipoOrden, aLongitudOrden, Trabajador)
        IniciarGrupos()
        IniciarCriterios()
        IniciarConstantes()

    End Sub
    Private Sub HabilitarTabs(ByVal Orden As Boolean, ByVal GRupo As Boolean, ByVal Criterio As Boolean, ByVal Constante As Boolean)
        grpOrden.Enabled = Orden
        ft.habilitarObjetos(Orden, True, cmbOrdenadoPor, cmbOrdenDesde, txtOrdenDesde, cmbOrdenHasta, txtOrdenHasta)
        grpGrupos.Enabled = GRupo
        grpCriterios.Enabled = Criterio
        grpConstantes.Enabled = Constante
    End Sub
    Private Sub IniciarOrden(ByVal vNombres As Object, ByVal vCampos As Object, ByVal vTipo As Object, ByVal vLongitud As Object, ByVal OrdenMandado As String)

        vOrdenNombres = vNombres
        vOrdenCampos = vCampos
        vOrdenTipo = vTipo
        vOrdenLongitud = vLongitud

        IndiceReporte = 0
        strIndiceReporte = vCampos(IndiceReporte)
        ft.RellenaCombo(vNombres, cmbOrdenadoPor)
        LongitudMaximaOrden(CInt(vLongitud(IndiceReporte)))
        TipoOrden(CStr(vTipo(IndiceReporte)), OrdenMandado)

        ft.habilitarObjetos(False, True, cmbOrdenDesde, cmbOrdenHasta)

    End Sub

    Private Sub IniciarGrupos()

        ft.RellenaCombo(vMERAgrupadoPor, cmbMERAgrupadoPor)
        ft.RellenaCombo(vCXCAgrupadoPor, cmbCXCAgrupadorPor)

        ft.habilitarObjetos(False, False, tabPageMercas)
        Select Case ReporteNumero
            Case ReporteVentas.cActivacionesClientesMercas
                ft.habilitarObjetos(True, False, tabPageMercas)
            Case ReporteVentas.cVentasHEINZ
                ft.habilitarObjetos(False, False, tabPageVentas)
                ft.habilitarObjetos(True, False, tabPageMercas)
                C1DockingTab1.SelectedIndex = 1
                cmbMERAgrupadoPor.SelectedIndex = 3
                txtTipoJerarquia.Text = "00016"
        End Select

    End Sub

    Private Sub IniciarCriterios()

        VerCriterio_Periodo(False, 0)
        VerCriterio_TipoDocumento(False)
        VerCriterio_Cliente(False)
        VerCriterio_Mercancia(False)
        VerCriterio_Asesor(False)
        VerCriterio_Almacen(False)
        VerCriterio_CausaNC(False)

        Select Case ReporteNumero
            Case ReporteVentas.cGuiaCargaPedidos
                VerCriterio_Periodo(True, 0, TipoPeriodo.iDiario)
                VerCriterio_Asesor(True)

            Case ReporteVentas.cListadoFacturas, ReporteVentas.cListadoPrePedidos, ReporteVentas.cListadoPedidos, _
                ReporteVentas.cListadoNotasDeEntrega, ReporteVentas.cListadoNotasDeCredito, ReporteVentas.cListadoNotasDebito, _
                ReporteVentas.cListadoPresupuestos
                VerCriterio_Periodo(True, 0)
                VerCriterio_Cliente(True)
            Case ReporteVentas.cActivacionesClientes
                VerCriterio_Periodo(True, 0)
            Case ReporteVentas.cSaldos, ReporteVentas.cAuditoriaClientes, ReporteVentas.cVencimientos, _
                ReporteVentas.cVencimientosResumen
                VerCriterio_Periodo(True, 2, TipoPeriodo.iDiario)
            Case ReporteVentas.cPesosPedidos
                VerCriterio_Periodo(True, 0, TipoPeriodo.iDiario)
            Case ReporteVentas.cEstadoDeCuentas, ReporteVentas.cMovimientosDocumento
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
            Case ReporteVentas.cMovimientosClientes
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_TipoDocumento(True, 3)
                VerCriterio_CausaNC(True)
            Case ReporteVentas.cLibroIVA
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
            Case ReporteVentas.cActivacionesClientesMercas
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Mercancia(True)
            Case ReporteVentas.cCierreDiario, ReporteVentas.cCierreDiarioContado
                VerCriterio_Periodo(True, 0, TipoPeriodo.iDiario)
                VerCriterio_Asesor(True)
            Case ReporteVentas.cVentasHEINZ
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Almacen(True)
            Case Else

        End Select

    End Sub
    Private Sub VerCriterio_CausaNC(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblCausaNotaCredito, txtCausaNC_Desde, txtCausaNC_Hasta, btnCausaNC_Desde, btnCausaNC_Hasta)
        ft.habilitarObjetos(False, True, txtCausaNC_Desde, txtCausaNC_Hasta)
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
        ft.habilitarObjetos(False, True, txtPeriodoDesde, txtPeriodoHasta)
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
                txtPeriodoDesde.Value = PrimerDiaA�o(jytsistema.sFechadeTrabajo)
                txtPeriodoHasta.Value = UltimoDiaA�o(jytsistema.sFechadeTrabajo)
            Case Else
                txtPeriodoDesde.Value = jytsistema.sFechadeTrabajo
                txtPeriodoHasta.Value = jytsistema.sFechadeTrabajo
        End Select

    End Sub
    Private Sub VerCriterio_TipoDocumento(ByVal ver As Boolean, Optional ByVal DocumentosTipo As Integer = 0)
        'DocumentosTipo : 0 = Bancos, 1 = caja, 2 = Forma de pago , 3 = CxC/CxP
        ft.visualizarObjetos(ver, lblTipodocumento, chkList)
        Select Case DocumentosTipo
            Case 0
                Dim aTipoDocumento() As String = {"CH", "DP", "NC", "ND"}
                Dim aSel() As Boolean = {True, True, True, True}
                RellenaListaSeleccionable(chkList, aTipoDocumento, aSel)
                txtTipDoc.Text = ".CH.DP.NC.ND"
            Case 1
                Dim aTipoDocumento() As String = {"EN", "SA"}
                Dim aSel() As Boolean = {True, True}
                RellenaListaSeleccionable(chkList, aTipoDocumento, aSel)
                txtTipDoc.Text = ".EN.SA"
            Case 2
                Dim aTipoDocumento() As String = {"EF", "CH", "TA", "CT", "DP", "TR"}
                Dim aSel() As Boolean = {True, True, True, True}
                RellenaListaSeleccionable(chkList, aTipoDocumento, aSel)
                txtTipDoc.Text = ".EF.CH.TA.CT.DP.TR"
            Case 3
                Dim aTipoDocumento() As String = {"FC", "GR", "ND", "AB", "CA", "NC"}
                Dim aSel() As Boolean = {True, True, True, True, True, True}
                RellenaListaSeleccionable(chkList, aTipoDocumento, aSel)
                txtTipDoc.Text = ".FC.GR.ND.AB.CA.NC"
        End Select
    End Sub
    Private Sub VerCriterio_Cliente(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblcliente, lblcliente, txtClienteDesde, btnClienteDesde,
                                lblClienteHasta, txtClienteHasta, btnClienteHasta)

        ft.habilitarObjetos(True, True, txtClienteDesde, txtClienteHasta)
        txtClienteDesde.MaxLength = 15
        txtClienteHasta.MaxLength = 15

    End Sub
    Private Sub VerCriterio_Asesor(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblAsesorCriterios, txtAsesorDesdeCriterios, btnAsesorDesdeCriterios,
                                txtAsesorHastaCriterios, btnAsesorHastaCriterios)

        ft.habilitarObjetos(True, True, txtAsesorDesdeCriterios, txtAsesorHastaCriterios)
        txtAsesorDesdeCriterios.MaxLength = 5
        txtAsesorHastaCriterios.MaxLength = 5

    End Sub
    Private Sub VerCriterio_Almacen(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblCriterioAlmacen, txtAlmacenDesde, btnAlmacenDesde,
                                txtAlmacenHasta, btnAlmacenHasta)

        ft.habilitarObjetos(True, True, txtAlmacenDesde, txtAlmacenHasta)
        txtAlmacenDesde.MaxLength = 5
        txtAlmacenHasta.MaxLength = 5

    End Sub
    Private Sub VerCriterio_Mercancia(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblMercanciaDesde, txtMercanciaDesde, btnMercanciaDesde,
                                lblMercanciaHasta, txtMercanciaHasta, btnMercanciaHasta)

        ft.habilitarObjetos(True, True, txtMercanciaDesde, txtMercanciaHasta)
        txtMercanciaDesde.MaxLength = 15
        txtMercanciaHasta.MaxLength = 15

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

        Select Case ReporteNumero
            Case ReporteVentas.cClientes
                verConstante_Resumen(True)
                VerConstante_TipoCliente(True)
                VerConstante_Estatus(True)
                verConstante_Descuentos(True)
            Case ReporteVentas.cPesosPedidos
                verConstante_Resumen(True)
                lblConsResumen.Text = "Resto cuota Asesor"
            Case ReporteVentas.cListadoFacturas, ReporteVentas.cListadoPrePedidos, ReporteVentas.cListadoPedidos,
                ReporteVentas.cListadoNotasDeEntrega
                If ReporteNumero = ReporteVentas.cListadoFacturas Then
                    verConstante_Resumen(True)
                    lblConsResumen.Text = "Facturas POS"
                End If


                VerConstante_CondicionPago(True)
                verConstante_EstatusFacturas(True)
                VerConstante_TipoCliente(True)
                VerConstante_Estatus(True)
            Case ReporteVentas.cListadoNotasDebito, ReporteVentas.cListadoNotasDeCredito, ReporteVentas.cListadoPresupuestos
                verConstante_Resumen(True)
                verConstante_EstatusFacturas(True)
                VerConstante_TipoCliente(True)
                VerConstante_Estatus(True)
            Case ReporteVentas.cSaldos, ReporteVentas.cEstadoDeCuentas, ReporteVentas.cMovimientosClientes,
                ReporteVentas.cAuditoriaClientes, ReporteVentas.cFacturasMesAMes, ReporteVentas.cActivacionesClientes,
                ReporteVentas.cPresupuestosMesMes, ReporteVentas.cPrepedidosMesMes,
                ReporteVentas.cPedidosMesMes, ReporteVentas.cNotasEntregaMesMes, ReporteVentas.cNotasCreditoMesMes,
                ReporteVentas.cNotasDebitoMesMes

                VerConstante_TipoCliente(True)
                VerConstante_Estatus(True)

                If ReporteNumero = ReporteVentas.cAuditoriaClientes Then verConstante_Resumen(True)

            Case ReporteVentas.cVencimientos, ReporteVentas.cVencimientosResumen

                VerConstante_CondicionPago(True)
                lblCondicionPago.Text = "Documentos desde : "
                ft.RellenaCombo(aVencimientos, cmbCondicionPago)

                VerConstante_TipoCliente(True)
                VerConstante_Estatus(True)
                VerConstante_Lapsos(True)
            Case ReporteVentas.cActivacionesClientesMercas
                VerConstante_Estatus(True)
                verConstante_EstatusFacturas(True)
                lblEstatusFactura.Text = " Tipo Activaci�n : "
                Dim aR() As String = {"Clientes sin activar", "Clientes activados", "Todos"}
                ft.RellenaCombo(aR, cmbEstatusFacturas, 1)
                verConstante_Resumen(True)
            Case ReporteVentas.cCierreDiario, ReporteVentas.cCierreDiarioContado
                verConstante_Resumen(True)
            Case ReporteVentas.cFacturasGuiaDespacho
                verConstante_Resumen(True)
                lblConsResumen.Text = "Direcci�n de despacho "
                VerConstante_peso(True)
                lblPeso.Text = "Totales monetarios en gu�a"
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
        ft.RellenaCombo(aEstatus, cmbEstatus, 4)
        ft.RellenaCombo(aEstatusFacturas, cmbEstatusFacturas, 3)
        ft.RellenaCombo(aSiNoTodos, cmbDescuentos, 2)

    End Sub
    Private Sub VerConstante_Lapsos(Ver As Boolean, Optional Desde1 As Integer = 1, Optional Hasta1 As Integer = 7,
                                     Optional Desde2 As Integer = 8, Optional Hasta2 As Integer = 15,
                                     Optional Desde3 As Integer = 16, Optional Hasta3 As Integer = 30,
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

    Private Sub LongitudMaximaOrden(ByVal iLongitud As Integer)
        txtOrdenDesde.MaxLength = iLongitud
        txtOrdenHasta.MaxLength = iLongitud
    End Sub
    Private Sub TipoOrden(ByVal cTipo As String, ByVal OrdenMandado As String)
        Select Case vOrdenTipo(IndiceReporte)
            Case "D"
                txtOrdenDesde.Text = ft.FormatoFecha(PrimerDiaMes(jytsistema.sFechadeTrabajo))
                txtOrdenHasta.Text = ft.FormatoFecha(UltimoDiaMes(jytsistema.sFechadeTrabajo))
                txtOrdenDesde.Enabled = False
                txtOrdenHasta.Enabled = False
            Case Else
                txtOrdenDesde.Text = OrdenMandado
                txtOrdenHasta.Text = OrdenMandado
        End Select

    End Sub

    Private Sub jsVenRepParametros_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
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

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

        HabilitarCursorEnEspera()

        Dim r As New frmViewer
        Dim dsVen As New DataSet ''dsVentas
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

        Select Case ReporteNumero
            Case ReporteVentas.cCxC
                nTabla = "dtCXC"
                nTablaFormaPago = "dtCXCFormaPago"
                nTablaRelacionCT = "dtCXCRelacionCT"
                oReporte = New rptVentasReciboCxC
                str = SeleccionVENCancelacionPlus(Documento)
                strFormaPago = SeleccionVENCancelacionFormaPago(Documento)
                strRelacionCT = SeleccionVENCancelacionRelacionCT(Documento)
            Case ReporteVentas.cVentasHEINZ
                nTabla = "dtVentasHEINZ"
                oReporte = New rptMercanciaVentaPlanaHEINZ
                str = SeleccionVENVentasHEINZ(txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtAlmacenDesde.Text, txtAlmacenHasta.Text,
                                              txtTipoJerarquia.Text)
            Case ReporteVentas.cCierreDiario
                nTabla = "dtCierre"
                oReporte = New rptVentasCierreDiario
                str = SeleccionVENCierreFinanciero(txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtAsesorDesdeCriterios.Text, txtAsesorHastaCriterios.Text)
            Case ReporteVentas.cCierreDiarioContado
                nTabla = "dtCierre"
                oReporte = New rptVentasCierreDiarioContado
                str = SeleccionVENCierreFinancieroContado(txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtAsesorDesdeCriterios.Text, txtAsesorHastaCriterios.Text)
            Case ReporteVentas.cFacturasGuiaDespacho
                nTabla = "dtGuiaFacturas"
                oReporte = New rptVentasGuiaDespachoFacturas
                str = SeleccionVENFacturasGuia(Documento)
            Case ReporteVentas.cMercanciasGuiaDespacho
                nTabla = "dtGuiaMercancias"
                oReporte = New rptVentasGuiaDespachoMercancias
                str = SeleccionVENMercanciasGuia(Documento)
            Case ReporteVentas.cRelacionFacturas
                nTabla = "dtGuiaFacturas"
                oReporte = New rptVentasRelacionDeFacturas
                str = SeleccionVENRelacionFacturas(Documento)
            Case ReporteVentas.cRelacionNotasCredito
                nTabla = "dtGuiaFacturas"
                oReporte = New rptVentasRelacionNotasDeCredito
                str = SeleccionVENRelacionNotasCredito(Documento)


            Case ReporteVentas.cGuiaPedidosMercancias
                nTabla = "dtGuiaMercancias"
                oReporte = New rptVentasGuiaPedidosMercancias
                str = SeleccionVENMercanciasGuiaPedidosXXX(Documento)
            Case ReporteVentas.cGuiaPedidos
                nTabla = "dtGuiaFacturas"
                oReporte = New rptVentasGuiaPedidos
                str = SeleccionVENPedidossGuia(Documento)


            Case ReporteVentas.cGuiaCargaPedidos
                nTabla = "dtGuiaMercancias"
                oReporte = New rptVentasGuiaCargaPedidosMercancias
                str = SeleccionVENMercanciasGuiaPedidos(txtOrdenDesde.Text, txtOrdenHasta.Text, txtAsesorDesdeCriterios.Text, txtAsesorHastaCriterios.Text,
                                                         txtPeriodoDesde.Value, txtPeriodoHasta.Value)
            Case ReporteVentas.cPresupuesto
                nTabla = "dtPresupuestos"
                nTablaIVA = "dtIVA"
                nTablaDescuentos = "dtDescuentos"
                nTablaComentarios = "dtComentarios"
                oReporte = New rptVentasPresupuesto
                str = SeleccionVENPresupuesto(Documento, FechaParametro)
                strIVA = SeleccionVENIVADocumento(Documento, "jsvenivacot", "numcot", "")
                strDescuentos = SeleccionVENDescuentosDocumento(Documento, "jsvendescot", "numcot", "")
                strComentarios = SeleccionGENComentarios("COT")
            Case ReporteVentas.cPrePedido
                nTabla = "dtPresupuestos"
                nTablaIVA = "dtIVA"
                nTablaDescuentos = "dtDescuentos"
                nTablaComentarios = "dtComentarios"
                oReporte = New rptVentasPrePedido
                str = SeleccionVENPrePedido(Documento, FechaParametro)
                strIVA = SeleccionVENIVADocumento(Documento, "jsvenivapedrgv", "numped", "")
                strDescuentos = SeleccionVENDescuentosDocumento(Documento, "jsvendespedrgv", "numped", "")
                strComentarios = SeleccionGENComentarios("PPE")
            Case ReporteVentas.cPedido
                nTabla = "dtPresupuestos"
                nTablaIVA = "dtIVA"
                nTablaDescuentos = "dtDescuentos"
                nTablaComentarios = "dtComentarios"
                oReporte = New rptVentasPedido
                str = SeleccionVENPedido(Documento, FechaParametro)
                strIVA = SeleccionVENIVADocumento(Documento, "jsvenivaped", "numped", "")
                strDescuentos = SeleccionVENDescuentosDocumento(Documento, "jsvendesped", "numped", "")
                strComentarios = SeleccionGENComentarios("PED")
            Case ReporteVentas.cNotaDeEntrega
                nTabla = "dtPresupuestos"
                nTablaIVA = "dtIVA"
                nTablaDescuentos = "dtDescuentos"
                nTablaComentarios = "dtComentarios"
                oReporte = New rptVentasNotaDeEntrega
                str = SeleccionVENNotaDeEntrega(Documento, FechaParametro)
                strIVA = SeleccionVENIVADocumento(Documento, "jsvenivanot", "numfac", "")
                strDescuentos = SeleccionVENDescuentosDocumento(Documento, "jsvendesnot", "numfac", "")
                strComentarios = SeleccionGENComentarios("PFC")
            Case ReporteVentas.cFactura
                nTabla = "dtPresupuestos"
                nTablaIVA = "dtIVA"
                nTablaDescuentos = "dtDescuentos"
                nTablaComentarios = "dtComentarios"
                oReporte = New rptVentasFactura
                str = SeleccionVENFactura(Documento, FechaParametro)
                strIVA = SeleccionVENIVADocumento(Documento, "jsvenivafac", "numfac", "")
                strDescuentos = SeleccionVENDescuentosDocumento(Documento, "jsvendesfac", "numfac", "")
                strComentarios = SeleccionGENComentarios("FAC")
            Case ReporteVentas.cNotaCredito
                nTabla = "dtPresupuestos"
                nTablaIVA = "dtIVA"
                nTablaDescuentos = "dtDescuentos"
                nTablaComentarios = "dtComentarios"
                oReporte = New rptVentasNotaCredito
                str = SeleccionVENNotaCredito(Documento, FechaParametro)
                strIVA = SeleccionVENIVADocumento(Documento, "jsvenivancr", "numncr", "")
                strComentarios = SeleccionGENComentarios("NCV")
            Case ReporteVentas.cNotaDebito
                nTabla = "dtPresupuestos"
                nTablaIVA = "dtIVA"
                nTablaDescuentos = "dtDescuentos"
                nTablaComentarios = "dtComentarios"
                oReporte = New rptVentasNotaDebito
                str = SeleccionVENNotaDebito(Documento, FechaParametro)
                strIVA = SeleccionVENIVADocumento(Documento, "jsvenivandb", "numndb", "")
                strComentarios = SeleccionGENComentarios("NDV")
            Case ReporteVentas.cClientes
                nTabla = "dtClientes"
                Select Case cmbCXCAgrupadorPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptVentasClientes0G
                    Case 1, 2, 3, 4, 5 '1 Grupo
                        oReporte = New rptVentasClientes1G
                    Case 7, 8 '2 grupos
                        oReporte = New rptVentasClientes2G
                    Case 9, 10 '3 grupos
                        oReporte = New rptVentasClientes3G
                    Case Else
                        oReporte = New rptVentasClientes0G
                End Select

                str = SeleccionVENClientes(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                             txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                             txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                             txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                             txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text,
                                             cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex, cmbDescuentos.SelectedIndex)

            Case ReporteVentas.cActivacionesClientes

                nTabla = "dtClientes"
                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbCXCAgrupadorPor.SelectedIndex.ToString)
                    Case 0 ' 0 grupos
                        oReporte = New rptVentasActivacionClientes0G
                    Case 1 '1 Grupo"
                        oReporte = New rptVentasActivacionClientes1G
                    Case 2 '2 grupos
                        oReporte = New rptVentasActivacionClientes2G
                    Case 3 '3 grupos
                        oReporte = New rptVentasActivacionClientes3G
                    Case Else
                        oReporte = New rptVentasActivacionClientes0G
                End Select

                str = SeleccionVENActivacionesClientes(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                             txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                             txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                             txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                             txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text,
                                             txtPeriodoDesde.Value, txtPeriodoHasta.Value, cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex)


            Case ReporteVentas.cFacturasMesAMes, ReporteVentas.cPresupuestosMesMes, ReporteVentas.cPedidosMesMes, ReporteVentas.cPrepedidosMesMes,
                ReporteVentas.cNotasEntregaMesMes, ReporteVentas.cNotasCreditoMesMes, ReporteVentas.cNotasDebitoMesMes
                nTabla = "dtFacturasMES"
                Select Case cmbCXCAgrupadorPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptVentasFacturacionMensual0G
                    Case 1, 2, 3, 4, 5 '1 Grupo
                        oReporte = New rptVentasFacturacionMensual1G
                    Case 7, 8 '2 grupos
                        oReporte = New rptVentasFacturacionMensual2G
                    Case 9, 10 '3 grupos
                        oReporte = New rptVentasFacturacionMensual3G
                    Case Else
                        oReporte = New rptVentasFacturacionMensual0G
                End Select

                If ReporteNumero = ReporteVentas.cFacturasMesAMes Then
                    str = SeleccionVENFacturacionMensual(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                                 txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                                 txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                                 txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                                 txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex)
                Else
                    Dim aTbls() As String = {"jsvenenccot", "jsvenencpedrgv", "jsvenencped", "jsvenencnot", "jsvenencncr", "jsvenencndb"}

                    str = SeleccionVENDocumentosMesMes(aTbls(ReporteNumero - 542), txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                                 txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                                 txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                                 txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                                 txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex)


                End If

            Case ReporteVentas.cCliente, ReporteVentas.cPlanillaCliente
                nTabla = "dtClientes"
                oReporte = New rptVentasFichaCliente
                str = SeleccionVENClientes(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex)

            Case ReporteVentas.cListadoFacturas
                nTabla = "dtFacturas"
                Select Case cmbCXCAgrupadorPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptVentasFacturas0G
                    Case 1, 2, 3, 4, 5 '1 Grupo
                        oReporte = New rptVentasFacturas1G
                    Case 7, 8 '2 grupos
                        oReporte = New rptVentasFacturas2G
                    Case 9, 10 '3 grupos
                        oReporte = New rptVentasFacturas3G
                    Case Else
                        oReporte = New rptVentasFacturas0G
                End Select

                str = SeleccionVENFacturas(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtClienteDesde.Text, txtClienteHasta.Text,
                                           txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                           txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                           txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                           txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, cmbTipo.SelectedIndex,
                                           cmbEstatus.SelectedIndex, cmbCondicionPago.SelectedIndex, cmbEstatusFacturas.SelectedIndex,
                                           chkConsResumen.Checked)
            Case ReporteVentas.cListadoPrePedidos
                nTabla = "dtFacturas"
                Select Case cmbCXCAgrupadorPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptVentasPrePedidos0G
                    Case 1, 2, 3, 4, 5 '1 Grupo
                        oReporte = New rptVentasPrePedidos1G
                    Case 7, 8 '2 grupos
                        oReporte = New rptVentasPrePedidos2G
                    Case 9, 10 '3 grupos
                        oReporte = New rptVentasPrePedidos3G
                    Case Else
                        oReporte = New rptVentasPrePedidos0G
                End Select

                str = SeleccionVENListadoPrePedidos(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtClienteDesde.Text, txtClienteHasta.Text,
                                           txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                           txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                           txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                           txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, cmbTipo.SelectedIndex,
                                           cmbEstatus.SelectedIndex, cmbCondicionPago.SelectedIndex, cmbEstatusFacturas.SelectedIndex)

            Case ReporteVentas.cListadoPedidos
                nTabla = "dtFacturas"
                Select Case cmbCXCAgrupadorPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptVentasPrePedidos0G
                    Case 1, 2, 3, 4, 5 '1 Grupo
                        oReporte = New rptVentasPrePedidos1G
                    Case 7, 8 '2 grupos
                        oReporte = New rptVentasPrePedidos2G
                    Case 9, 10 '3 grupos
                        oReporte = New rptVentasPrePedidos3G
                    Case Else
                        oReporte = New rptVentasPrePedidos0G
                End Select

                str = SeleccionVENListadoPedidos(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtClienteDesde.Text, txtClienteHasta.Text,
                                           txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                           txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                           txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                           txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, cmbTipo.SelectedIndex,
                                           cmbEstatus.SelectedIndex, cmbCondicionPago.SelectedIndex, cmbEstatusFacturas.SelectedIndex)

            Case ReporteVentas.cActivacionesClientesMercas
                nTabla = "dtClientesMercancias"
                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbCXCAgrupadorPor.SelectedIndex.ToString)
                    Case 0 ' 0 grupos
                        oReporte = New rptVentasActivacionesClientesMercancias0G
                    Case 1 '1 Grupo
                        oReporte = New rptVentasActivacionesClientesMercancias1G
                    Case 2 '2 grupos
                        oReporte = New rptVentasActivacionesClientesMercancias2G
                    Case 3 '3 grupos
                        oReporte = New rptVentasActivacionesClientesMercancias3G
                    Case Else
                        oReporte = New rptVentasActivacionesClientesMercancias0G
                End Select

                str = SeleccionVENActivacionesClientesYMercancias(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtPeriodoDesde.Value, txtPeriodoHasta.Value,
                                           txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                           txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                           txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                           txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, cmbEstatusFacturas.SelectedIndex, cmbEstatus.SelectedIndex,
                                           txtMercanciaDesde.Text, txtMercanciaHasta.Text, txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text,
                                           txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, txtCategoriaDesde.Text, txtCategoriaHasta.Text,
                                           txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text, txtDivisionHasta.Text)


            Case ReporteVentas.cListadoNotasDeEntrega
                nTabla = "dtFacturas"
                Select Case cmbCXCAgrupadorPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptVentasPrePedidos0G
                    Case 1, 2, 3, 4, 5 '1 Grupo
                        oReporte = New rptVentasPrePedidos1G
                    Case 7, 8 '2 grupos
                        oReporte = New rptVentasPrePedidos2G
                    Case 9, 10 '3 grupos
                        oReporte = New rptVentasPrePedidos3G
                    Case Else
                        oReporte = New rptVentasPrePedidos0G
                End Select

                str = SeleccionVENListadoNotasDeEntrega(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtClienteDesde.Text, txtClienteHasta.Text,
                                           txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                           txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                           txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                           txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, cmbTipo.SelectedIndex,
                                           cmbEstatus.SelectedIndex, cmbCondicionPago.SelectedIndex, cmbEstatusFacturas.SelectedIndex)

            Case ReporteVentas.cListadoNotasDeCredito

                nTabla = "dtFacturasPLUS"
                Select Case cmbCXCAgrupadorPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptVentasNotasCredito0G
                    Case 1, 2, 3, 4, 5 '1 Grupo
                        oReporte = New rptVentasNotasCredito1G
                    Case 7, 8 '2 grupos
                        oReporte = New rptVentasNotasCredito2G
                    Case 9, 10 '3 grupos
                        oReporte = New rptVentasNotasCredito3G
                    Case Else
                        oReporte = New rptVentasNotasCredito0G
                End Select

                str = SeleccionVENListadoNotasDeCredito(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtClienteDesde.Text, txtClienteHasta.Text,
                                           txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                           txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                           txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                           txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, cmbTipo.SelectedIndex,
                                           cmbEstatus.SelectedIndex, cmbEstatusFacturas.SelectedIndex)

            Case ReporteVentas.cListadoNotasDebito

                nTabla = "dtFacturas"
                Select Case cmbCXCAgrupadorPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptVentasPrePedidos0G
                    Case 1, 2, 3, 4, 5 '1 Grupo
                        oReporte = New rptVentasPrePedidos1G
                    Case 7, 8 '2 grupos
                        oReporte = New rptVentasPrePedidos2G
                    Case 9, 10 '3 grupos
                        oReporte = New rptVentasPrePedidos3G
                    Case Else
                        oReporte = New rptVentasPrePedidos0G
                End Select

                str = SeleccionVENListadoNotasDebito(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtClienteDesde.Text, txtClienteHasta.Text,
                                           txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                           txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                           txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                           txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, cmbTipo.SelectedIndex,
                                           cmbEstatus.SelectedIndex, cmbEstatusFacturas.SelectedIndex)

            Case ReporteVentas.cListadoPresupuestos

                nTabla = "dtFacturas"
                Select Case cmbCXCAgrupadorPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptVentasPrePedidos0G
                    Case 1, 2, 3, 4, 5 '1 Grupo
                        oReporte = New rptVentasPrePedidos1G
                    Case 7, 8 '2 grupos
                        oReporte = New rptVentasPrePedidos2G
                    Case 9, 10 '3 grupos
                        oReporte = New rptVentasPrePedidos3G
                    Case Else
                        oReporte = New rptVentasPrePedidos0G
                End Select

                str = SeleccionVENListadoPresupuestos(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtClienteDesde.Text, txtClienteHasta.Text,
                                           txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                           txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                           txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                           txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, cmbTipo.SelectedIndex,
                                           cmbEstatus.SelectedIndex, cmbEstatusFacturas.SelectedIndex)
            Case ReporteVentas.cSaldos
                nTabla = "dtClientes"
                Select Case cmbCXCAgrupadorPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptVentasSaldos0G
                    Case 1, 2, 3, 4, 5 '1 Grupo
                        oReporte = New rptVentasSaldos1G
                    Case 7, 8 '2 grupos
                        oReporte = New rptVentasSaldos2G
                    Case 9, 10 '3 grupos
                        oReporte = New rptVentasSaldos3G
                    Case Else
                        oReporte = New rptVentasSaldos0G
                End Select

                str = SeleccionVENSaldoClientes(myConn, lblInfo, txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                               txtPeriodoHasta.Value, txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                             txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                             txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                             txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex)

            Case ReporteVentas.cEstadoDeCuentas
                nTabla = "dtMovimientos"
                oReporte = New rptVentasEstadodeCuentaClientes0G

                str = SeleccionVENEstadoDeCuentaClientesPlus(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                             txtPeriodoDesde.Value, txtPeriodoHasta.Value,
                                             cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex)
            Case ReporteVentas.cMovimientosClientes
                nTabla = "dtMovimientos"
                oReporte = New rptVentasMovimientosClientes0G

                str = SeleccionVENMovimientosClientes(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                             txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtTipDoc.Text,
                                             cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex, txtCausaNC_Desde.Text, txtCausaNC_Hasta.Text)

            Case ReporteVentas.cAuditoriaClientes
                nTabla = "dtMovimientos"
                oReporte = New rptVentasAuditoriasClientes

                str = SeleccionVENAuditoriaClientes(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                             txtPeriodoHasta.Value, txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                             txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                             txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                             txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex)
            Case ReporteVentas.cVencimientos
                nTabla = "dtMovimientos"
                oReporte = New rptVentasVencimientos

                str = SeleccionVENVencimientosPlus(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                             txtPeriodoHasta.Value, txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                             txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                             txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                             txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex,
                                             CInt(txtDesde1.Text), CInt(txtHasta1.Text), CInt(txtDesde2.Text), CInt(txtHasta2.Text),
                                             CInt(txtDesde3.Text), CInt(txtHasta3.Text), CInt(txtDesde4.Text), IIf(cmbCondicionPago.SelectedIndex = 0, True, False))

            Case ReporteVentas.cVencimientosResumen
                nTabla = "dtVencimientosR"
                oReporte = New rptVentasVencimientosResumen

                str = SeleccionVENVencimientosResumen(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                             txtPeriodoHasta.Value, txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                             txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                             txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                             txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex,
                                             CInt(txtDesde1.Text), CInt(txtHasta1.Text), CInt(txtDesde2.Text), CInt(txtHasta2.Text),
                                             CInt(txtDesde3.Text), CInt(txtHasta3.Text), CInt(txtDesde4.Text), IIf(cmbCondicionPago.SelectedIndex = 0, True, False))

            Case ReporteVentas.cMovimientosDocumento
                nTabla = "dtMovimientos"
                oReporte = New rptVentasMovimientosDocumentosG0

                str = SeleccionVENMovimientosDocumentos(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                             txtPeriodoDesde.Value, txtPeriodoHasta.Value, cmbOrdenadoPor.SelectedIndex)

            Case ReporteVentas.cLibroIVA
                nTabla = "dtLibroIVA"
                oReporte = New rptVentasLibroIVAGeneral

                str = SeleccionVENLibroIVA(myConn, lblInfo,
                                              txtPeriodoDesde.Value, txtPeriodoHasta.Value, , , , , , , , , , ,
                                              txtPais.Text, txtEstado.Text, txtMunicipio.Text, txtParroquia.Text, txtBarrio.Text)

            Case ReporteVentas.cPesosPedidos
                nTabla = "dtPedidos"
                Select Case cmbCXCAgrupadorPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptVentasPrepedidosPesos0G
                    Case 1, 2, 3, 4, 5 '1 Grupo
                        oReporte = New rptVentasPrepedidosPesos1G
                    Case 7, 8 '2 grupos
                        oReporte = New rptVentasPrepedidosPesos2G
                    Case 9, 10 '3 grupos
                        oReporte = New rptVentasPrepedidosPesos3G
                    Case Else
                        oReporte = New rptVentasPrepedidosPesos0G
                End Select

                str = SeleccionVENPrePedidos(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                             txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtClienteDesde.Text, txtClienteHasta.Text,
                                             txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text,
                                             txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                             txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text,
                                             txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text,
                                             cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex, , , True)

            Case Else
                oReporte = Nothing
        End Select

        If nTabla <> "" Then
            dsVen = DataSetRequery(dsVen, str, myConn, nTabla, lblInfo)

            '/////////// SUBREPORTES
            Select Case ReporteNumero
                Case ReporteVentas.cPresupuesto, ReporteVentas.cPrePedido, ReporteVentas.cPedido, ReporteVentas.cNotaDeEntrega,
                    ReporteVentas.cFactura, ReporteVentas.cNotaDebito, ReporteVentas.cNotaCredito

                    dsVen = DataSetRequery(dsVen, strIVA, myConn, nTablaIVA, lblInfo)
                    If strDescuentos <> "" Then dsVen = DataSetRequery(dsVen, strDescuentos, myConn, nTablaDescuentos, lblInfo)
                    dsVen = DataSetRequery(dsVen, strComentarios, myConn, nTablaComentarios, lblInfo)
                Case ReporteVentas.cCxC
                    dsVen = DataSetRequery(dsVen, strFormaPago, myConn, nTablaFormaPago, lblInfo)
                    dsVen = DataSetRequery(dsVen, strRelacionCT, myConn, nTablaRelacionCT, lblInfo)
            End Select
            If dsVen.Tables(nTabla).Rows.Count > 0 Then

                oReporte = PresentaReporte(oReporte, dsVen, nTabla)
                r.CrystalReportViewer1.ReportSource = oReporte
                r.CrystalReportViewer1.ToolPanelView = IIf(PresentaArbol, CrystalDecisions.Windows.Forms.ToolPanelViewType.GroupTree,
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
                    r.CrystalReportViewer1.ToolPanelView = IIf(PresentaArbol, CrystalDecisions.Windows.Forms.ToolPanelViewType.GroupTree,
                                                  CrystalDecisions.Windows.Forms.ToolPanelViewType.None)
                    r.CrystalReportViewer1.ShowGroupTreeButton = PresentaArbol
                    r.CrystalReportViewer1.Zoom(1)
                    r.CrystalReportViewer1.Refresh()
                    r.Cargar(ReporteNombre)
                    DeshabilitarCursorEnEspera()
                Else
                    ft.mensajeCritico("No existe informaci�n que cumpla con estos criterios y/o constantes ")
                End If

            End If
        End If


        r.Close()
        r = Nothing
        oReporte.Close()
        oReporte = Nothing

    End Sub
    Private Function PresentaReporte(ByVal oReporte As CrystalDecisions.CrystalReports.Engine.ReportClass,
                                     ByVal ds As DataSet, ByVal nTabla As String) As CrystalDecisions.CrystalReports.Engine.ReportClass

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
            'CaminoImagen = My.Computer.FileSystem.CurrentDirectory & "\" & "logo" & .Item("id_emp") & ".jpg"
        End With
        dtEmpresa.Dispose()
        dtEmpresa = Nothing

        Select Case ReporteNumero
            Case ReporteVentas.cClientes
                oReporte.ReportDefinition.Sections("DetailSection3").SectionFormat.EnableSuppress = chkConsResumen.Checked
            Case ReporteVentas.cPresupuesto, ReporteVentas.cPrePedido, ReporteVentas.cPedido, ReporteVentas.cNotaDeEntrega,
                ReporteVentas.cFactura, ReporteVentas.cNotaDebito, ReporteVentas.cNotaCredito
                oReporte.Subreports("rptGENIVA.rpt").SetDataSource(ds.Tables("dtIVA"))
                If ReporteNumero <> ReporteVentas.cNotaCredito And ReporteNumero <> ReporteVentas.cNotaDebito Then _
                    oReporte.Subreports("rptGENDescuentos.rpt").SetDataSource(ds.Tables("dtDescuentos"))
                oReporte.Subreports("rptGENComentarios.rpt").SetDataSource(ds.Tables("dtComentarios"))
            Case ReporteVentas.cCxC
                oReporte.Subreports("rptGENFormaDePago.rpt").SetDataSource(ds.Tables("dtCXCFormaPago"))
                oReporte.Subreports("rptGENRelacionCT.rpt").SetDataSource(ds.Tables("dtCXCRelacionCT"))
        End Select

        oReporte.SetDataSource(ds)
        oReporte.Refresh()
        oReporte.SetParameterValue("strLogo", CaminoImagen)
        oReporte.SetParameterValue("Orden", IIf(ReporteNumero = ReporteVentas.cLibroIVA, "", "Ordenado por : " + cmbOrdenadoPor.Text + " " + txtOrdenDesde.Text + "/" + txtOrdenHasta.Text))
        oReporte.SetParameterValue("RIF", "RIF : " + rif)
        oReporte.SetParameterValue("Grupo", LineaGrupos)
        oReporte.SetParameterValue("Criterios", LineaCriterios)
        oReporte.SetParameterValue("Constantes", LineaConstantes)
        oReporte.SetParameterValue("Empresa", jytsistema.WorkName.TrimEnd(" "))

        Select Case ReporteNumero

            Case ReporteVentas.cCierreDiario, ReporteVentas.cCierreDiarioContado
                oReporte.SetParameterValue("Resumido", chkConsResumen.Checked)
            Case ReporteVentas.cFacturasGuiaDespacho
                oReporte.SetParameterValue("Resumido", chkConsResumen.Checked)
                oReporte.SetParameterValue("Totales", chkPeso.Checked)
            Case ReporteVentas.cAuditoriaClientes
                oReporte.SetParameterValue("pResumen", chkConsResumen.Checked)
            Case ReporteVentas.cCxC
                oReporte.SetParameterValue("MontoEnLetras", NumerosATexto(ft.DevuelveScalarDoble(myConn, "select abs(SUM(IMPORTE)) from jsventracob where comproba = '" & Documento & "' and id_emp = '" & jytsistema.WorkID & "' ")))
            Case ReporteVentas.cPresupuesto
                oReporte.SetParameterValue("MontoEnLetras", NumerosATexto(ft.DevuelveScalarDoble(myConn, "select tot_cot from jsvenenccot where numcot = '" & Documento & "' and id_emp = '" & jytsistema.WorkID & "' ")))
            Case ReporteVentas.cPrePedido
                oReporte.SetParameterValue("MontoEnLetras", NumerosATexto(ft.DevuelveScalarDoble(myConn, "select tot_ped from jsvenencpedrgv where numped = '" & Documento & "' and id_emp = '" & jytsistema.WorkID & "' ")))
            Case ReporteVentas.cPedido
                oReporte.SetParameterValue("MontoEnLetras", NumerosATexto(ft.DevuelveScalarDoble(myConn, "select tot_ped from jsvenencped where numped = '" & Documento & "' and id_emp = '" & jytsistema.WorkID & "' ")))
            Case ReporteVentas.cNotaDeEntrega
                oReporte.SetParameterValue("MontoEnLetras", NumerosATexto(ft.DevuelveScalarDoble(myConn, "select tot_fac from jsvenencnot where numfac = '" & Documento & "' and id_emp = '" & jytsistema.WorkID & "' ")))
            Case ReporteVentas.cFactura
                oReporte.SetParameterValue("MontoEnLetras", NumerosATexto(ft.DevuelveScalarDoble(myConn, "select tot_fac from jsvenencfac where numfac = '" & Documento & "' and id_emp = '" & jytsistema.WorkID & "' ")))
            Case ReporteVentas.cNotaCredito
                oReporte.SetParameterValue("MontoEnLetras", NumerosATexto(ft.DevuelveScalarDoble(myConn, "select tot_ncr from jsvenencncr where numncr = '" & Documento & "' and id_emp = '" & jytsistema.WorkID & "' ")))
            Case ReporteVentas.cNotaDebito
                oReporte.SetParameterValue("MontoEnLetras", NumerosATexto(ft.DevuelveScalarDoble(myConn, "select tot_ndb from jsvenencndb where numndb = '" & Documento & "' and id_emp = '" & jytsistema.WorkID & "' ")))
            Case ReporteVentas.cVencimientosResumen
                oReporte.SetParameterValue("Lapso0", txtDesde1.Text & " d�as � menos")
                oReporte.SetParameterValue("Lapso1", "de " & txtDesde1.Text & " a " & txtHasta1.Text & " d�as")
                oReporte.SetParameterValue("Lapso2", "de " & txtDesde2.Text & " a " & txtHasta2.Text & " d�as")
                oReporte.SetParameterValue("Lapso3", "de " & txtDesde3.Text & " a " & txtHasta3.Text & " d�as")
                oReporte.SetParameterValue("Lapso4", txtDesde4.Text & " d�as � m�s")
            Case ReporteVentas.cClientes, ReporteVentas.cListadoFacturas, ReporteVentas.cSaldos, ReporteVentas.cPesosPedidos,
                ReporteVentas.cListadoPrePedidos, ReporteVentas.cListadoPedidos, ReporteVentas.cListadoNotasDeEntrega,
                ReporteVentas.cListadoNotasDebito, ReporteVentas.cListadoNotasDeCredito, ReporteVentas.cListadoPresupuestos,
                ReporteVentas.cEstadoDeCuentas, ReporteVentas.cMovimientosClientes, ReporteVentas.cMovimientosDocumento,
                ReporteVentas.cFacturasMesAMes, ReporteVentas.cActivacionesClientesMercas, ReporteVentas.cActivacionesClientes,
                ReporteVentas.cPresupuestosMesMes, ReporteVentas.cPedidosMesMes, ReporteVentas.cPrepedidosMesMes,
                ReporteVentas.cNotasEntregaMesMes, ReporteVentas.cNotasCreditoMesMes, ReporteVentas.cNotasDebitoMesMes

                If ReporteNumero = ReporteVentas.cEstadoDeCuentas Or
                    ReporteNumero = ReporteVentas.cMovimientosDocumento Then _
                    oReporte.SetParameterValue("SaldoAl", "Saldo al " & ft.FormatoFecha(DateAdd("d", -1, txtPeriodoDesde.Value)))

                If ReporteNumero = ReporteVentas.cActivacionesClientesMercas Then _
                        oReporte.SetParameterValue("Resumen", chkConsResumen.Checked)

                If ReporteNumero = ReporteVentas.cPesosPedidos Then _
                        oReporte.SetParameterValue("Resumido", Not chkConsResumen.Checked)

                If ReporteNumero = ReporteVentas.cFacturasMesAMes Then _
                        oReporte.SetParameterValue("Titulo", "Facturaci�n Mes a Mes")
                If ReporteNumero = ReporteVentas.cPresupuestosMesMes Then _
                        oReporte.SetParameterValue("Titulo", "Presupuestos Mes a Mes")
                If ReporteNumero = ReporteVentas.cPrepedidosMesMes Then _
                        oReporte.SetParameterValue("Titulo", "Prepedidos Mes a Mes")
                If ReporteNumero = ReporteVentas.cPedidosMesMes Then _
                        oReporte.SetParameterValue("Titulo", "Pedidos Mes a Mes")
                If ReporteNumero = ReporteVentas.cNotasEntregaMesMes Then _
                        oReporte.SetParameterValue("Titulo", "Notas de Entrega Mes a Mes")
                If ReporteNumero = ReporteVentas.cNotasCreditoMesMes Then _
                        oReporte.SetParameterValue("Titulo", "Notas de Cr�dito Mes a Mes")
                If ReporteNumero = ReporteVentas.cNotasDebitoMesMes Then _
                        oReporte.SetParameterValue("Titulo", "Notas D�bito Mes a Mes")

                If ReporteNumero = ReporteVentas.cListadoPresupuestos Then _
                        oReporte.SetParameterValue("Titulo", "Presupuestos")

                If ReporteNumero = ReporteVentas.cListadoPrePedidos Then _
                        oReporte.SetParameterValue("Titulo", "Pre-Pedidos")

                If ReporteNumero = ReporteVentas.cListadoPedidos Then _
                        oReporte.SetParameterValue("Titulo", "Pedidos")

                If ReporteNumero = ReporteVentas.cListadoNotasDeEntrega Then _
                        oReporte.SetParameterValue("Titulo", "Notas de Entrega")

                If ReporteNumero = ReporteVentas.cListadoNotasDeCredito Then
                    oReporte.SetParameterValue("Titulo", "Notas de Cr�dito")
                    oReporte.SetParameterValue("Resumen", chkConsResumen.Checked)
                End If

                If ReporteNumero = ReporteVentas.cListadoNotasDebito Then _
                        oReporte.SetParameterValue("Titulo", "Notas D�bito")

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

        PresentaReporte = oReporte


    End Function

    Private Function numGrupo() As Integer

        vGrupos = ""
        numGrupo = 0
        '/////////////// ojojojojoj FALTA EL MIX (DEBE SER SELECCIONADO EN UN COMBOBOX COMO CRITERIO
        If lblCanal.Visible Then
            vGrupos += ", canal "
            numGrupo += 1
        End If

        If lblTipoNegocio.Visible Then
            vGrupos += ", tiponegocio "
            numGrupo += 1
        End If

        If lblZona.Visible Then
            vGrupos += ", zona "
            numGrupo += 1
        End If

        If lblRuta.Visible Then
            vGrupos += ", ruta "
            numGrupo += 1
        End If

        If lblAsesor.Visible Then
            vGrupos += ", asesor "
            numGrupo += 1
        End If

        If lblCategoria.Visible Then
            vGrupos += ", categoria "
            numGrupo += 1
        End If

        If lblMarcas.Visible Then
            vGrupos += ", marca "
            numGrupo += 1
        End If

        If lblDivisiones.Visible Then
            vGrupos += ", division "
            numGrupo += 1
        End If

        If lblJerarquias.Visible Then
            vGrupos += ", tipjer "
            numGrupo += 1
        End If


    End Function


    Private Function GruposCantidad(ByVal CadenaGrupos As String) As Integer
        Select Case CadenaGrupos
            Case "00" ' 0 grupos
                GruposCantidad = 0
            Case "01", "02", "03", "04", "05", "06", "10", "20", "30", "40"  ' 1 grupo
                GruposCantidad = 1
            Case "11", "12", "13", "14", "15", "16", "21", "22", "23", "24", "25", "26", "31", "32", "33",
                "34", "35", "36", "41", "42", "43", "44", "45", "46", "51", "52", "53", "54", "55", "56",
                "07", "08", "60", "80", "90", "110", "120" '2 grupos
                GruposCantidad = 2
            Case "70", "100", "140", "61", "62", "63", "64", "65", "66", "81", "82", "83", "84", "85", "86",
                "91", "92", "93", "94", "95", "96", "111", "112", "113", "114", "115", "116", "121", "122",
                "123", "124", "125", "126", "17", "18", "27", "28", "37", "38", "47", "48", "57", "58", "09", "010" '3 grupos
                GruposCantidad = 3
            Case "130", "71", "72", "73", "75", "76", "101", "102", "103", "104", "105", "106",
                "141", "142", "143", "144", "145", "146", "67", "68", "87", "88", "97", "98",
                "117", "118", "127", "128", "19", "110", "29", "210", "39", "310", "49", "410",
                "59", "510" '4 grupos
                GruposCantidad = 4
            Case Else
                GruposCantidad = 0
        End Select
    End Function

    Private Sub btnLimpiar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLimpiar.Click
        LimpiarOrden()
        LimpiarGrupos()
    End Sub
    Private Sub LimpiarGrupos()
        LimpiarTextos(txtCanalDesde, txtCanalHasta, txtRutaDesde, txtRutaHasta, txtAsesorDesde, txtPais, txtAsesorHasta,
            txtEstado, txtMunicipio, txtZonaDesde, txtZonaHasta, txtTipoNegocioDesde, txtTipoNegocioHasta)
    End Sub
    Private Sub LimpiarOrden()
        LimpiarTextos(txtOrdenDesde, txtOrdenHasta)
    End Sub

    Private Function LineaGrupos() As String
        LineaGrupos = ""
        If ReporteNumero = ReporteVentas.cLibroIVA Then
            LineaGrupos = "Direcci�n comercial y fiscal :" _
                & ft.DevuelveScalarCadena(myConn, " select dirfiscal from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ") _
                & ". Telefonos : " & ft.DevuelveScalarCadena(myConn, " select telef1 from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ") & ". " _
                & ft.DevuelveScalarCadena(myConn, " select telef2 from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ") & ". " _
                & ft.DevuelveScalarCadena(myConn, " select fax from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ") & ". e-mail : " _
                & ft.DevuelveScalarCadena(myConn, " select email from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ")
        Else

            If lblCanal.Visible Then LineaGrupos += "Canal: " & txtCanalDesde.Text & "/" & txtCanalHasta.Text
            If LineaGrupos <> "" Then LineaGrupos += " - "
            If lblTipoNegocio.Visible Then LineaGrupos += "Tipo Negocio: " & txtTipoNegocioDesde.Text & "/" & txtTipoNegocioHasta.Text
            If LineaGrupos <> "" Then LineaGrupos += " - "
            If lblZona.Visible Then LineaGrupos += "Zona: " & txtZonaDesde.Text & "/" & txtZonaHasta.Text
            If LineaGrupos <> "" Then LineaGrupos += " - "
            If lblRuta.Visible Then LineaGrupos += "Ruta: " & txtRutaDesde.Text & "/" & txtRutaHasta.Text
            If LineaGrupos <> "" Then LineaGrupos += " - "
            If lblAsesor.Visible Then LineaGrupos += "Asesor: " & txtAsesorDesde.Text & "/" & txtAsesorHasta.Text
            If LineaGrupos <> "" Then LineaGrupos += " - "
            If lblTerritorio.Visible Then LineaGrupos += "Territorio: " & txtPais.Text & "/" & txtEstado.Text _
                    & "/" & txtMunicipio.Text & "/" & txtParroquia.Text & "/" & txtCiudad.Text & "/" & txtBarrio.Text

            If lblCategoria.Visible Then LineaGrupos += "Categor�as : " & txtCategoriaDesde.Text & "/" & txtCategoriaHasta.Text
            If LineaGrupos <> "" Then LineaGrupos += " - "
            If lblMarcas.Visible Then LineaGrupos += "Marcas : " & txtMarcaDesde.Text & "/" & txtMarcaHasta.Text
            If LineaGrupos <> "" Then LineaGrupos += " - "
            If lblDivisiones.Visible Then LineaGrupos += "Divisi�n : " & txtDivisionDesde.Text & "/" & txtDivisionHasta.Text
            If LineaGrupos <> "" Then LineaGrupos += " - "
            If lblJerarquias.Visible Then LineaGrupos += "Jerarqu�a : " & txtTipoJerarquia.Text

        End If
    End Function
    Private Function LineaCriterios() As String
        LineaCriterios = ""
        If ReporteNumero = ReporteVentas.cLibroIVA Then
            LineaCriterios = "MES : " & UCase(Format(txtPeriodoDesde.Value, "MMMM")) &
                            " - A�O : " & txtPeriodoHasta.Value.GetValueOrDefault().Year.ToString()
        Else
            If lblPeriodo.Visible Then LineaCriterios += "Per�odo: " & IIf(lblPeriodoDesde.Visible, txtPeriodoDesde.Value, "") & IIf(lblPeriodoDesde.Visible AndAlso lblPeriodoHasta.Visible, "/", "") & IIf(lblPeriodoHasta.Visible, txtPeriodoHasta.Value, "")
            If LineaCriterios <> "" Then LineaCriterios += " - "
            If lblTipodocumento.Visible Then LineaCriterios += " Tipo Documentos : " + txtTipDoc.Text
            If LineaCriterios <> "" Then LineaCriterios += " - "
            If lblcliente.Visible Then LineaCriterios += " Cliente : " + txtClienteDesde.Text + "/" + txtClienteHasta.Text
            If LineaCriterios <> "" Then LineaCriterios += " - "
            If lblMercanciaDesde.Visible Then LineaCriterios += " Mercancias : " + txtMercanciaDesde.Text + "/" + txtMercanciaHasta.Text
            If LineaCriterios <> "" Then LineaCriterios += " - "
            If lblAsesorCriterios.Visible Then LineaCriterios += " Asesor Comercial : " + txtAsesorDesdeCriterios.Text + "/" + txtAsesorHastaCriterios.Text
        End If
    End Function
    Private Function LineaConstantes() As String
        LineaConstantes = ""
        If lblConsResumen.Visible Then LineaConstantes += "Resumido : " + IIf(chkConsResumen.Checked, "Si", "No")
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblCondicionPago.Visible Then LineaConstantes += lblCondicionPago.Text + cmbCondicionPago.Text '   aCondicionPago(cmbCondicionPago.SelectedIndex)
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblEstatusFactura.Visible Then LineaConstantes += lblEstatusFactura.Text + cmbEstatusFacturas.SelectedItem.ToString
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblTipo.Visible Then LineaConstantes += " Tipo Cliente : " + aTipo(cmbTipo.SelectedIndex)
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblEstatus.Visible Then LineaConstantes += " Estatus Clientes : " + aEstatus(cmbEstatus.SelectedIndex)
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblLapso.Visible Then LineaConstantes += " Lapsos: 1. " + txtDesde1.Text + "-" + txtHasta1.Text +
                                                    " 2. " + txtDesde2.Text + "-" + txtHasta2.Text +
                                                    " 3. " + txtDesde3.Text + "-" + txtHasta3.Text +
                                                    " 4. " + txtDesde4.Text

    End Function
    Private Sub btnOrdenDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrdenDesde.Click
        txtDeOrden(txtOrdenDesde)
    End Sub

    Private Sub btnOrdenHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrdenHasta.Click
        txtDeOrden(txtOrdenHasta)
    End Sub
    Private Sub txtDeOrden(ByVal txt As TextBox)
        Select Case cmbOrdenadoPor.Text
            Case "C�digo Cliente"
                txt.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcli codigo, nombre descripcion from jsvencatcli where id_emp = '" & jytsistema.WorkID & "' order by codcli ", "Clientes", txt.Text)
            Case "Nombre Cliente"
            Case "N�mero Factura"
            Case "N�mero Prepedido"
                txt.Text = CargarTablaSimple(myConn, lblInfo, ds, " select numped codigo, codcli descripcion from jsvenencpedrgv where emision >= '" & ft.FormatoFechaMySQL(DateAdd("d", -15, jytsistema.sFechadeTrabajo)) & "' AND  id_emp = '" & jytsistema.WorkID & "' order by numped ", "N�mero Pre-Pedido", txt.Text)
            Case "N�mero Documento"
                If ReporteNumero = ReporteVentas.cGuiaCargaPedidos Then
                    txt.Text = CargarTablaSimple(myConn, lblInfo, ds, " select numped codigo, codcli descripcion from jsvenencped where emision >= '" & ft.FormatoFechaMySQL(txtPeriodoDesde.Value) & "' AND emision <= '" & ft.FormatoFechaMySQL(txtPeriodoHasta.Value) & "' AND " _
                                                 & " CODVEN >= '" & txtAsesorDesdeCriterios.Text & "' AND " _
                                                 & " CODVEN <= '" & txtAsesorHastaCriterios.Text & "' AND " _
                                                 & " id_emp = '" & jytsistema.WorkID & "' order by numped ", "Pedidos ", txt.Text)
                End If
        End Select


    End Sub

    Private Sub txtOrdenDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOrdenDesde.TextChanged
        txtOrdenHasta.Text = txtOrdenDesde.Text
    End Sub

    Private Sub cmbCXCAgrupadorPor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCXCAgrupadorPor.SelectedIndexChanged

        ft.visualizarObjetos(False, lblGrupoDesde, lblGrupoHasta, lblCanal, lblTipoNegocio, lblZona, lblRuta, lblAsesor, lblTerritorio,
                            txtCanalDesde, btnCanalDesde, txtCanalHasta, btnCanalHasta,
                            txtTipoNegocioDesde, btnTipoNegocioDesde, txtTipoNegocioHasta, btnTipoNegocioHasta,
                            txtZonaDesde, btnZonaDesde, txtZonaHasta, btnZonaHasta,
                            txtRutaDesde, btnRutaDesde, txtRutaHasta, btnRutaHasta,
                            txtAsesorDesde, btnAsesorDesde, txtAsesorHasta, btnAsesorHasta,
                            txtPais, btnPais, txtEstado, btnEstado, txtMunicipio, btnMunicipio,
                            txtParroquia, btnParroquia, txtCiudad, btnCiudad, txtBarrio, btnBarrio)
        LimpiarGrupos()
        Select Case cmbCXCAgrupadorPor.SelectedIndex
            Case 0
            Case 1
                VerCanal()
            Case 2
                VerTipoNegocio()
            Case 3
                VerZona()
            Case 4
                VerRuta()
            Case 5
                VerAsesor()
            Case 6
                VerTerritorio()
            Case 7
                VerCanal()
                VerTipoNegocio()
            Case 8
                VerZona()
                VerRuta()
            Case 9
                VerAsesor()
                VerCanal()
                VerTipoNegocio()
            Case 10
                VerAsesor()
                VerZona()
                VerRuta()
            Case Else
        End Select
        ColocarGrupos()

    End Sub
    Private Sub VerCanal()
        ft.visualizarObjetos(True, lblGrupoDesde, lblGrupoHasta)
        ft.visualizarObjetos(True, lblCanal, txtCanalDesde, btnCanalDesde, txtCanalHasta, btnCanalHasta)
        ft.habilitarObjetos(False, True, txtCanalDesde, txtCanalHasta)
    End Sub
    Private Sub VerTipoNegocio()
        ft.visualizarObjetos(True, lblGrupoDesde, lblGrupoHasta)
        ft.visualizarObjetos(True, lblTipoNegocio, txtTipoNegocioDesde, btnTipoNegocioDesde, txtTipoNegocioHasta, btnTipoNegocioHasta)
        ft.habilitarObjetos(False, True, txtTipoNegocioDesde, txtTipoNegocioHasta)
    End Sub
    Private Sub VerZona()
        ft.visualizarObjetos(True, lblGrupoDesde, lblGrupoHasta)
        ft.visualizarObjetos(True, lblZona, txtZonaDesde, btnZonaDesde, txtZonaHasta, btnZonaHasta)
        ft.habilitarObjetos(False, True, txtZonaDesde, txtZonaHasta)
    End Sub
    Private Sub VerRuta()
        ft.visualizarObjetos(True, lblGrupoDesde, lblGrupoHasta)
        ft.visualizarObjetos(True, lblRuta, txtRutaDesde, btnRutaDesde, txtRutaHasta, btnRutaHasta)
        ft.habilitarObjetos(False, True, txtRutaDesde, txtRutaHasta)
    End Sub
    Private Sub VerAsesor()
        ft.visualizarObjetos(True, lblGrupoDesde, lblGrupoHasta)
        ft.visualizarObjetos(True, lblAsesor, txtAsesorDesde, btnAsesorDesde, txtAsesorHasta, btnAsesorHasta)
        ft.habilitarObjetos(False, True, txtAsesorDesde, txtAsesorHasta)
    End Sub

    Private Sub VerTerritorio()
        ft.visualizarObjetos(True, lblGrupoDesde, lblGrupoHasta)
        ft.visualizarObjetos(True, lblTerritorio, txtPais, btnPais, txtEstado, btnEstado,
                            txtMunicipio, btnMunicipio, txtParroquia, btnParroquia, txtCiudad, btnCiudad, txtBarrio, btnBarrio)
        ft.habilitarObjetos(False, True, txtPais, txtEstado, txtMunicipio, txtParroquia, txtCiudad, txtBarrio)
    End Sub

    Private Sub VerCategorias()
        ft.visualizarObjetos(True, lblGrupoDesde, lblGrupoHasta)
        ft.visualizarObjetos(True, lblCategoria, txtCategoriaDesde, btnCategoriaDesde, txtCategoriaHasta, btnCategoriaHasta)
        ft.habilitarObjetos(False, True, txtCategoriaDesde, txtCategoriaHasta)
    End Sub
    Private Sub VerMarcas()
        ft.visualizarObjetos(True, lblGrupoDesde, lblGrupoHasta)
        ft.visualizarObjetos(True, lblMarcas, txtMarcaDesde, btnMarcaDesde, txtMarcaHasta, btnMarcaHasta)
        ft.habilitarObjetos(False, True, txtMarcaDesde, txtMarcaHasta)
    End Sub

    Private Sub VerJerarquias()
        ft.visualizarObjetos(True, lblGrupoDesde, lblGrupoHasta)
        ft.visualizarObjetos(True, lblJerarquias, txtTipoJerarquia, btnTipoJerarquia, txtCodjer1, btnCodjer1,
                            txtCodjer2, btnCodjer2, txtCodjer3, btnCodjer3, txtCodjer4, btnCodjer4, txtCodjer5, btnCodjer5, txtCodjer6, btnCodjer6)
        ft.habilitarObjetos(False, True, txtTipoJerarquia, txtCodjer1, txtCodjer2, txtCodjer3, txtCodjer4, txtCodjer5, txtCodjer6)
    End Sub
    Private Sub VerDivisiones()
        ft.visualizarObjetos(True, lblGrupoDesde, lblGrupoHasta)
        ft.visualizarObjetos(True, lblDivisiones, txtDivisionDesde, btnDivisionDesde, txtDivisionHasta, btnDivisionHasta)
        ft.habilitarObjetos(False, True, txtDivisionDesde, txtDivisionHasta)
    End Sub

    Private Sub txtCanalDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCanalDesde.TextChanged
        txtCanalHasta.Text = txtCanalDesde.Text
    End Sub

    Private Sub ColocarGrupos()
        grpGrupos.Text = "Agrupago por    " & "Mercanc�as : " & cmbMERAgrupadoPor.Text & "   /   Ventas : " & cmbCXCAgrupadorPor.Text & " "
    End Sub

    Private Sub txtTipoNegocioDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTipoNegocioDesde.TextChanged
        txtTipoNegocioHasta.Text = txtTipoNegocioDesde.Text
    End Sub

    Private Sub txtZonaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtZonaDesde.TextChanged
        txtZonaHasta.Text = txtZonaDesde.Text
    End Sub
    Private Sub txtRutaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtRutaDesde.TextChanged
        txtRutaHasta.Text = txtRutaDesde.Text
    End Sub

    Private Sub txtAsesorDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAsesorDesde.TextChanged
        txtAsesorHasta.Text = txtAsesorDesde.Text
    End Sub

    Private Sub btnCanalDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCanalDesde.Click
        txtCanalDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, "select codigo, descrip descripcion from jsvenliscan where id_emp = '" & jytsistema.WorkID & "' order by 1", "Canales",
                                               txtCanalDesde.Text)
    End Sub

    Private Sub btnCanalHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCanalHasta.Click
        txtCanalHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, "select codigo, descrip descripcion from jsvenliscan where id_emp = '" & jytsistema.WorkID & "' order by 1", "Canales",
                                               txtCanalHasta.Text)
    End Sub

    Private Sub btnTipoNegocioDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoNegocioDesde.Click
        txtTipoNegocioDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, "select codigo, descrip descripcion from jsvenlistip where id_emp = '" & jytsistema.WorkID & "' order by 1", "Tipos de Negocio",
                                                     txtTipoNegocioDesde.Text)
    End Sub

    Private Sub btnTipoNegocioHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoNegocioHasta.Click
        txtTipoNegocioHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, "select codigo, descrip descripcion from jsvenlistip where id_emp = '" & jytsistema.WorkID & "' order by 1", "Tipos de Negocio",
                                                     txtTipoNegocioHasta.Text)
    End Sub

    Private Sub btnZonaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZonaDesde.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Zonas", FormatoTablaSimple(Modulo.iZonasClientes), False, TipoCargaFormulario.iShowDialog)
        txtZonaDesde.Text = f.Seleccion
        f = Nothing
    End Sub

    Private Sub btnZonaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZonaHasta.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Zonas", FormatoTablaSimple(Modulo.iZonasClientes), False, TipoCargaFormulario.iShowDialog)
        txtZonaHasta.Text = f.Seleccion
        f = Nothing
    End Sub

    Private Sub btnRutaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRutaDesde.Click
        txtRutaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codrut codigo, nomrut descripcion from jsvenencrut where tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1", "Rutas de visita", txtRutaDesde.Text)
    End Sub

    Private Sub btnRutaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRutaHasta.Click
        txtRutaHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codrut codigo, nomrut descripcion from jsvenencrut where tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1", "Rutas de visita", txtRutaHasta.Text)
    End Sub

    Private Sub btnAsesorDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesorDesde.Click
        txtAsesorDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, concat(apellidos,', ',nombres) descripcion from jsvencatven where tipo = " & TipoVendedor.iFuerzaventa & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Asesores Comerciales", txtAsesorDesde.Text)
    End Sub

    Private Sub btnAsesorHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesorHasta.Click
        txtAsesorHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, concat(apellidos,', ',nombres) descripcion from jsvencatven where tipo = " & TipoVendedor.iFuerzaventa & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Asesores Comerciales", txtAsesorHasta.Text)
    End Sub

    Private Sub btnPais_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPais.Click
        txtPais.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1", "Paises", txtPais.Text)
    End Sub

    Private Sub btnEstado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEstado.Click
        If txtPais.Text <> "" Then _
            txtEstado.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 1 and antecesor = " & txtPais.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Estados", txtEstado.Text)

    End Sub

    Private Sub btnMunicipio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMunicipio.Click
        If txtEstado.Text <> "" Then _
            txtMunicipio.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 2 and antecesor = " & txtEstado.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Municipios", txtMunicipio.Text)
    End Sub

    Private Sub btnParroquia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnParroquia.Click
        If txtMunicipio.Text <> "" Then _
            txtParroquia.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 2 and antecesor = " & txtMunicipio.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Parroquias", txtParroquia.Text)
    End Sub

    Private Sub btnCiudad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCiudad.Click
        If txtParroquia.Text <> "" Then _
                    txtCiudad.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 3 and antecesor = " & txtParroquia.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Ciudades", txtCiudad.Text)
    End Sub

    Private Sub btnBarrio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBarrio.Click
        If txtCiudad.Text <> "" Then _
                    txtBarrio.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 4 and antecesor = " & txtCiudad.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Barrios", txtBarrio.Text)
    End Sub

    Private Sub btnClienteDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClienteDesde.Click
        txtClienteDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select a.codcli codigo, a.nombre descripcion, a.ingreso from jsvencatcli a where a.id_emp = '" & jytsistema.WorkID & "' order by codcli ", "Clientes", txtClienteDesde.Text)
    End Sub

    Private Sub btnClienteHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClienteHasta.Click
        txtClienteHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select a.codcli codigo, a.nombre descripcion, a.ingreso from jsvencatcli a where a.id_emp = '" & jytsistema.WorkID & "' order by codcli ", "Clientes", txtClienteHasta.Text)
    End Sub

    Private Sub txtClienteDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtClienteDesde.TextChanged
        txtClienteHasta.Text = txtClienteDesde.Text
    End Sub

    Private Sub chkList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkList.SelectedIndexChanged,
         chkList.DoubleClick
        Dim iCont As Integer
        txtTipDoc.Text = ""
        For iCont = 0 To chkList.Items.Count - 1
            If chkList.GetItemCheckState(iCont) = CheckState.Checked Then
                txtTipDoc.Text += "." + chkList.Items(iCont).ToString
            End If
        Next
    End Sub


    Private Sub cmbMERAgrupadoPor_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbMERAgrupadoPor.SelectedIndexChanged
        ft.visualizarObjetos(False, lblGrupoDesde, lblGrupoHasta, lblCategoria, lblMarcas, lblDivisiones, lblJerarquias,
                         txtCategoriaDesde, btnCategoriaDesde, txtCategoriaHasta, btnCategoriaHasta,
                         txtMarcaDesde, btnMarcaDesde, txtMarcaHasta, btnMarcaHasta,
                         txtDivisionDesde, btnDivisionDesde, txtDivisionHasta, btnDivisionHasta,
                         txtTipoJerarquia, btnTipoJerarquia, txtCodjer1, btnCodjer1, txtCodjer2, btnCodjer2,
                         txtCodjer3, btnCodjer3, txtCodjer4, btnCodjer4, txtCodjer5, btnCodjer5, txtCodjer6, btnCodjer6)
        LimpiarGrupos()
        Select Case cmbMERAgrupadoPor.SelectedIndex
            Case 0
            Case 1
                VerCategorias()
            Case 2
                VerMarcas()
            Case 3
                VerJerarquias()
            Case 4
                VerDivisiones()
            Case 5
            Case 6 '"Categor�as & Marcas"
                VerCategorias()
                VerMarcas()
            Case 7 '"Divisi�n & Categor�as &  Marcas"
                VerDivisiones()
                VerCategorias()
                VerMarcas()
            Case 8 '"Categor�as & Jerarqu�as"
                VerCategorias()
                VerJerarquias()
            Case 9 '"Divisi�n & Jerarqu�a"
                VerDivisiones()
                VerJerarquias()
            Case 10 '"Mix & Categor�as & Marcas"
                VerCategorias()
                VerMarcas()
            Case 11 '"Mix & Jerarqu�as"
                VerJerarquias()
            Case 12 '"Mix & Divisi�n"
                VerDivisiones()
            Case 13 '"Mix & Divisi�n & Categor�as & Marcas"
                VerDivisiones()
                VerCategorias()
                VerMarcas()
            Case 14 '"Mix & Divisi�n & Jerarqu�as"
                VerDivisiones()
                VerJerarquias()
            Case 15 '"Divisi�n & Mix"
                VerDivisiones()
            Case Else
                VerCategorias()
                VerMarcas()
                VerJerarquias()
                VerDivisiones()
        End Select
        ColocarGrupos()
    End Sub

    Private Sub btnCategoriaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoriaDesde.Click
        Dim fg As New jsControlArcTablaSimple
        fg.Cargar("Categor�as", FormatoTablaSimple(Modulo.iCategoriaMerca), True, TipoCargaFormulario.iShowDialog)
        txtCategoriaDesde.Text = fg.Seleccion
        fg.Close()
        fg = Nothing
    End Sub

    Private Sub btnCategoriaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoriaHasta.Click
        Dim fg As New jsControlArcTablaSimple
        fg.Cargar("Categor�as", FormatoTablaSimple(Modulo.iCategoriaMerca), True, TipoCargaFormulario.iShowDialog)
        txtCategoriaHasta.Text = fg.Seleccion
        fg.Close()
        fg = Nothing
    End Sub

    Private Sub btnMarcaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarcaDesde.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Marcas", FormatoTablaSimple(Modulo.iMarcaMerca), True, TipoCargaFormulario.iShowDialog)
        txtMarcaDesde.Text = f.Seleccion
        f.Close()
        f = Nothing
    End Sub

    Private Sub btnMarcaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarcaHasta.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Marcas", FormatoTablaSimple(Modulo.iMarcaMerca), True, TipoCargaFormulario.iShowDialog)
        txtMarcaHasta.Text = f.Seleccion
        f.Close()
        f = Nothing
    End Sub

    Private Sub btnDivisionDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDivisionDesde.Click
        txtDivisionDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select division codigo, descrip descripcion from jsmercatdiv where  id_emp = '" & jytsistema.WorkID & "' order by 1", "Divisiones de mercanc�as", txtDivisionDesde.Text)
    End Sub

    Private Sub btnDivisionHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDivisionHasta.Click
        txtDivisionHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select division codigo, descrip descripcion from jsmercatdiv where  id_emp = '" & jytsistema.WorkID & "' order by 1", "Divisiones de mercanc�as", txtDivisionHasta.Text)
    End Sub

    Private Sub btnTipoJerarquia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoJerarquia.Click
        txtTipoJerarquia.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT tipjer codigo, descrip descripcion FROM jsmerencjer WHERE  id_emp  = '" & jytsistema.WorkID & "' order by 1 ", " Tipo de Jerarqu�a",
                                                  txtTipoJerarquia.Text)
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
                f.Cargar(" Codigo Jerarqu�a nivel " & Nivel & " ", ds, dtCJ, nTableCJ, TipoCargaFormulario.iShowDialog, False)
                txtCodjer.Text = f.Seleccion
                f = Nothing
            End If
            dtCJ = Nothing
        End If

    End Sub
    Private Sub btnCodjer1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodjer1.Click
        CargarJerarquia(myConn, ds, txtTipoJerarquia.Text, 1, txtCodjer1)
    End Sub

    Private Sub btnCodjer2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodjer2.Click
        CargarJerarquia(myConn, ds, txtTipoJerarquia.Text, 2, txtCodjer2)
    End Sub
    Private Sub btnCodjer3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodjer3.Click
        CargarJerarquia(myConn, ds, txtTipoJerarquia.Text, 3, txtCodjer3)
    End Sub

    Private Sub btnCodjer4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodjer4.Click
        CargarJerarquia(myConn, ds, txtTipoJerarquia.Text, 4, txtCodjer4)
    End Sub

    Private Sub btnCodjer5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodjer5.Click
        CargarJerarquia(myConn, ds, txtTipoJerarquia.Text, 5, txtCodjer5)
    End Sub

    Private Sub btnCodjer6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodjer6.Click
        CargarJerarquia(myConn, ds, txtTipoJerarquia.Text, 6, txtCodjer6)
    End Sub

    Private Sub btnMercanciaDesde_Click(sender As System.Object, e As System.EventArgs) Handles btnMercanciaDesde.Click
        txtMercanciaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codart codigo, nomart descripcion from jsmerctainv where estatus = 1 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Mercanc�as",
                                                  txtMercanciaDesde.Text)
    End Sub

    Private Sub btnMercanciaHasta_Click(sender As System.Object, e As System.EventArgs) Handles btnMercanciaHasta.Click
        txtMercanciaHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codart codigo, nomart descripcion from jsmerctainv where estatus = 1 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Mercanc�as",
                                                  txtMercanciaHasta.Text)
    End Sub

    Private Sub txtMercanciaDesde_TextChanged(sender As Object, e As System.EventArgs) Handles txtMercanciaDesde.TextChanged
        txtMercanciaHasta.Text = txtMercanciaDesde.Text
    End Sub

    Private Sub btnAsesorDesdeCriterios_Click(sender As System.Object, e As System.EventArgs) Handles btnAsesorDesdeCriterios.Click
        txtAsesorDesdeCriterios.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, concat(apellidos,', ',nombres) descripcion from jsvencatven where tipo = " & TipoVendedor.iFuerzaventa & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Asesores Comerciales", txtAsesorDesdeCriterios.Text)
    End Sub

    Private Sub btnAsesorHastaCriterios_Click(sender As System.Object, e As System.EventArgs) Handles btnAsesorHastaCriterios.Click
        txtAsesorHastaCriterios.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, concat(apellidos,', ',nombres) descripcion from jsvencatven where tipo = " & TipoVendedor.iFuerzaventa & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Asesores Comerciales", txtAsesorHastaCriterios.Text)
    End Sub

    Private Sub txtAsesorDesdeCriterios_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtAsesorDesdeCriterios.TextChanged
        txtAsesorHastaCriterios.Text = txtAsesorDesdeCriterios.Text
    End Sub

    Private Sub txtPeriodoDesde_TextChanged(sender As System.Object, e As System.EventArgs)
        Select Case periodoTipo
            Case TipoPeriodo.iDiario
                txtPeriodoHasta.Value = txtPeriodoDesde.Value
            Case TipoPeriodo.iSemanal
                txtPeriodoHasta.Value = DateAdd(DateInterval.Day, 7, txtPeriodoDesde.Value.GetValueOrDefault())
            Case TipoPeriodo.iMensual
                txtPeriodoHasta.Value = UltimoDiaMes(txtPeriodoDesde.Value)
            Case TipoPeriodo.iAnual
                txtPeriodoHasta.Value = UltimoDiaA�o(txtPeriodoDesde.Value)
        End Select
    End Sub

    Private Sub btnAlmacenDesde_Click(sender As System.Object, e As System.EventArgs) Handles btnAlmacenDesde.Click
        txtAlmacenDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codalm codigo, desalm descripcion from jsmercatalm where id_emp = '" & jytsistema.WorkID & "' order by 1", "Almacenes", txtAlmacenDesde.Text)
    End Sub

    Private Sub txtAlmacenDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtAlmacenDesde.TextChanged
        txtAlmacenHasta.Text = txtAlmacenDesde.Text
    End Sub

    Private Sub btnAlmacenHasta_Click(sender As System.Object, e As System.EventArgs) Handles btnAlmacenHasta.Click
        txtAlmacenHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codalm codigo, desalm descripcion from jsmercatalm where id_emp = '" & jytsistema.WorkID & "' order by 1", "Almacenes", txtAlmacenHasta.Text)
    End Sub

    Private Sub txtCategoriaDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCategoriaDesde.TextChanged
        txtCategoriaHasta.Text = txtCategoriaDesde.Text
    End Sub

    Private Sub txtMarcaDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtMarcaDesde.TextChanged
        txtMarcaHasta.Text = txtMarcaDesde.Text
    End Sub

    Private Sub txtDivisionDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtDivisionDesde.TextChanged
        txtDivisionHasta.Text = txtDivisionDesde.Text
    End Sub
    Private Sub btnCausaNC_Desde_Click(sender As Object, e As EventArgs) Handles btnCausaNC_Desde.Click
        txtCausaNC_Desde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descripcion from jsconcausas_notascredito where origen = 'CXC' order by codigo ", "CAUSAS NOTAS CREDITO NO FISCALES", txtCausaNC_Desde.Text)
    End Sub
    Private Sub btnCausaNC_hASTA_Click(sender As Object, e As EventArgs) Handles btnCausaNC_Hasta.Click
        txtCausaNC_Hasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descripcion from jsconcausas_notascredito where origen = 'CXC' order by codigo ", "CAUSAS NOTAS CREDITO NO FISCALES", txtCausaNC_Hasta.Text)
    End Sub
    Private Sub txtCausaNC_Desde_TextChanged(sender As Object, e As EventArgs) Handles txtCausaNC_Desde.TextChanged
        txtCausaNC_Hasta.Text = txtCausaNC_Desde.Text
    End Sub
End Class