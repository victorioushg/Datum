Imports MySql.Data.MySqlClient
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports ReportesDeCompras
Imports Syncfusion.WinForms.Input

Public Class jsComRepParametros

    Private Const sModulo As String = "Reportes de compras y cuentas por pagar"

    Private ReporteNumero As Integer
    Private ReporteNombre As String
    Private CodigoProveedor As String, Documento As String
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

    Private vCOMAgrupadoPor() As String = {}
    Private aCOMAgrupadoPor() As String = {}

    Private aSiNoTodos() As String = {"Si", "No", "Todos"}
    Private aExistencias() As String = {"Con", "Sin", "Todas"}
    Private aEstatus() As String = {"Activo", "Inactivo", "Todos"}
    Private aEstatusDocumento() As String = {"Tránsito", "Procesado", "Todas"}
    Private aTipoProveedor() As String = {"Compras", "Gastos", "Todos"}
    Private aCuenta() As String = {"CxP", "ExP"}
    Private strGrupoSubgrupo As String = ""
    Private titleGrupo As String = ""
    Private titleSubGrupo As String = ""

    Private PeriodoTipo As TipoPeriodo
    Private CxP_ExP As Integer

    Public Sub Cargar(ByVal TipoCarga As Integer, ByVal numReporte As Integer, ByVal nomReporte As String,
                      Optional ByVal CodProveedor As String = "", Optional ByVal numDocumento As String = "",
                      Optional ByVal Fecha As Date = #1/1/2009#, Optional strSQLTablaDoble As String = "",
                      Optional TituloGrupo As String = "", Optional TituloSubGrupo As String = "",
                      Optional TipoCuenta As Integer = 0)



        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo
        myConn.Open()

        Dim dates As SfDateTimeEdit() = {txtPeriodoDesde, txtPeriodoHasta}

        ReporteNumero = numReporte
        ReporteNombre = nomReporte
        CodigoProveedor = CodProveedor
        Documento = numDocumento
        FechaParametro = Fecha
        strGrupoSubgrupo = strSQLTablaDoble
        titleGrupo = TituloGrupo
        titleSubGrupo = TituloSubGrupo
        CxP_ExP = TipoCuenta

        If ReporteNumero = ReporteCompras.cListadoGastos Then
            vCOMAgrupadoPor = {"Ninguno", "Categorías", "Unidades de Negocio", "Categorías & Unidades", "Grupo & SubGrupo"}
            aCOMAgrupadoPor = {"", "categoriaprov", "unidadnegocioprov", "categoriaprov, unidadnegocioprov", "grupo, subgrupo"}
        Else
            vCOMAgrupadoPor = {"Ninguno", "Categorías", "Unidades de Negocio", "Categorías & Unidades"}
            aCOMAgrupadoPor = {"", "categoriaprov", "unidadnegocioprov", "categoriaprov, unidadnegocioprov"}
        End If


        '////////////////
        ft.habilitarObjetos(False, True, cmbOrdenDesde, cmbOrdenHasta)

        PresentarReporte(numReporte, nomReporte, CodigoProveedor)

        If TipoCarga = TipoCargaFormulario.iShow Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If

    End Sub
    Private Sub PresentarReporte(ByVal NumeroReporte As Integer, ByVal NombreDelReporte As String, Optional ByVal CodigoProveedor As String = "")
        lblNombreReporte.Text += " - " + NombreDelReporte
        Select Case NumeroReporte

            Case ReporteCompras.cGrupoSubGrupo
                Dim vOrdenNombres() As String = {"Grupo/Sub-Grupo"}
                Dim vOrdenCampos() As String = {"grupo"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteCompras.cOrdenDeCompra, ReporteCompras.cRecepcion, ReporteCompras.cGasto, ReporteCompras.cCompra, ReporteCompras.cNotaCredito,
                ReporteCompras.cNotaDebito
                Dim vOrdenNombres() As String = {"N° Documento"}
                Dim vOrdenCampos() As String = {"numord"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteCompras.cRetencionIVA, ReporteCompras.cRetencionISLR
                Dim vOrdenNombres() As String = {"No. Comprobante"}
                Dim vOrdenCampos() As String = {"comproba"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteCompras.cComprobantePago
                Dim vOrdenNombres() As String = {"No. Comprobante"}
                Dim vOrdenCampos() As String = {"num_retencion"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteCompras.cListadoOrdenesDeCompra
                Dim vOrdenNombres() As String = {"Número Documento"}
                Dim vOrdenCampos() As String = {"numord"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteCompras.cListadoRecepciones
                Dim vOrdenNombres() As String = {"Número Documento"}
                Dim vOrdenCampos() As String = {"numrec"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteCompras.cListadoCompras
                Dim vOrdenNombres() As String = {"Número Documento"}
                Dim vOrdenCampos() As String = {"numcom"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteCompras.cListadoDocumentosSinRetencionIVA
                Dim vOrdenNombres() As String = {"Número Documento"}
                Dim vOrdenCampos() As String = {"numcom"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)

            Case ReporteCompras.cListadoNotasCredito
                Dim vOrdenNombres() As String = {"Número Documento"}
                Dim vOrdenCampos() As String = {"numncr"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteCompras.cListadoNotasDebito
                Dim vOrdenNombres() As String = {"Número Documento"}
                Dim vOrdenCampos() As String = {"numndb"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteCompras.cListadoGastos
                Dim vOrdenNombres() As String = {"Número Documento"}
                Dim vOrdenCampos() As String = {"numgas"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteCompras.cProveedores
                Dim vOrdenNombres() As String = {"Código proveedor", "Nombre proveedor"}
                Dim vOrdenCampos() As String = {"codpro", "nombre"}
                Dim vOrdenTipo() As String = {"S", "S"}
                Dim vOrdenLongitud() As Integer = {15, 50}
                Inicializar(ReporteNombre, True, True, False, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoProveedor)
            Case ReporteCompras.cFichaProveedor
                Dim vOrdenNombres() As String = {"Código proveedor"}
                Dim vOrdenCampos() As String = {"codpro"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoProveedor)

            Case ReporteCompras.cSaldosProveedores, ReporteCompras.cAuditoriasProveedores, ReporteCompras.cVencimientos, ReporteCompras.cVencimientosResumen
                Dim vOrdenNombres() As String = {"Código proveedor", "Nombre proveedor"}
                Dim vOrdenCampos() As String = {"codpro", "nombre"}
                Dim vOrdenTipo() As String = {"S", "S"}
                Dim vOrdenLongitud() As Integer = {15, 50}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoProveedor)
            Case ReporteCompras.cEstadodeCuentasProveedores, ReporteCompras.cMovimientosProveedores
                Dim vOrdenNombres() As String = {"Código proveedor", "Nombre proveedor"}
                Dim vOrdenCampos() As String = {"codpro", "nombre"}
                Dim vOrdenTipo() As String = {"S", "S"}
                Dim vOrdenLongitud() As Integer = {15, 50}
                Inicializar(ReporteNombre, IIf(CodigoProveedor.Trim() = "", True, False), False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoProveedor)
            Case ReporteCompras.cLibroIVA
                Dim vOrdenNombres() As String = {"Emisión/Documento"}
                Dim vOrdenCampos() As String = {"numcom"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteCompras.cListaRetencionesISLR, ReporteCompras.cListaRetencionesIVA
                Dim vOrdenNombres() As String = {"Emisión/Documento"}
                Dim vOrdenCampos() As String = {"numcom"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
        End Select
    End Sub
    Private Sub Inicializar(ByVal nEtiqueta As String, ByVal TabOrden As Boolean, ByVal TabGrupo As Boolean,
        ByVal TabCriterio As Boolean, ByVal TabConstantes As Boolean, ByVal aNombreOrden() As String,
        ByVal aCampoOrden() As String, ByVal aTipoOrden() As String, ByVal aLongitudOrden() As Integer,
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
        grpGrupos.Enabled = GRupo
        grpCriterios.Enabled = Criterio
        grpConstantes.Enabled = Constante
    End Sub
    Private Sub IniciarOrden(ByVal vNombres As Object, ByVal vCampos As Object, ByVal vTipo As Object, ByVal vLongitud As Object,
                             ByVal OrdenMandado As String)

        vOrdenNombres = vNombres
        vOrdenCampos = vCampos
        vOrdenTipo = vTipo
        vOrdenLongitud = vLongitud

        IndiceReporte = 0
        strIndiceReporte = vCampos(IndiceReporte)
        ft.RellenaCombo(vNombres, cmbOrdenadoPor)
        LongitudMaximaOrden(CInt(vLongitud(IndiceReporte)))
        TipoOrden(CStr(vTipo(IndiceReporte)), OrdenMandado)

    End Sub
    Private Sub IniciarGrupos()
        ft.RellenaCombo(vCOMAgrupadoPor, cmbCOMAgrupadoPor)
    End Sub

    Private Sub IniciarCriterios()

        VerCriterio_Periodo(False, 0)
        VerCriterio_Proveedor(False)
        VerCriterio_GrupoSubGrupo(False)
        VerCriterio_TipoDocumento(False)
        VerCriterio_Documento(False)
        VerCriterio_MesAño(False)
        VerCriterio_CausaNC(False)

        Select Case ReporteNumero
            Case ReporteCompras.cListadoOrdenesDeCompra, ReporteCompras.cListadoRecepciones, ReporteCompras.cListadoCompras,
                ReporteCompras.cListadoNotasCredito, ReporteCompras.cListadoNotasDebito, ReporteCompras.cListadoGastos,
                ReporteCompras.cListadoDocumentosSinRetencionIVA
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Proveedor(True)
                If ReporteNumero = ReporteCompras.cListadoGastos Then _
                    VerCriterio_GrupoSubGrupo(True)
            Case ReporteCompras.cSaldosProveedores, ReporteCompras.cAuditoriasProveedores, ReporteCompras.cVencimientos,
                ReporteCompras.cVencimientosResumen
                VerCriterio_Periodo(True, 2, TipoPeriodo.iDiario)
            Case ReporteCompras.cEstadodeCuentasProveedores
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
            Case ReporteCompras.cMovimientosProveedores
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_TipoDocumento(True, 3)
                VerCriterio_CausaNC(True)

            Case ReporteCompras.cLibroIVA
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Proveedor(True)
            Case ReporteCompras.cListaRetencionesISLR, ReporteCompras.cListaRetencionesIVA
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Proveedor(True)
            Case Else

        End Select

    End Sub
    Private Sub VerCriterio_GrupoSubGrupo(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblGrupoSubgrupo, txtGrupo, txtSubgrupo, btnSubgrupo)
        ft.habilitarObjetos(False, True, txtGrupo, txtSubgrupo)
    End Sub
    Private Sub VerCriterio_CausaNC(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblCausaNotaCredito, txtCausaNC_Desde, txtCausaNC_Hasta, btnCausaNC_Desde, btnCausaNC_Hasta)
        ft.habilitarObjetos(False, True, txtCausaNC_Desde, txtCausaNC_Hasta)
    End Sub
    Private Sub VerCriterio_Proveedor(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblProveedor, lblProveedorDesde, lblProveedorHasta, txtProveedorDesde, txtProveedorHasta, btnProveedorDesde, btnProveedorHasta)
        txtProveedorDesde.Enabled = True : txtProveedorDesde.MaxLength = 15
        txtProveedorHasta.Enabled = True : txtProveedorHasta.MaxLength = 15
    End Sub
    Private Sub VerCriterio_Documento(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblDocumento, lblDocumentoDesde, lblDocumentoHasta, txtDocumentoDesde, txtDocumentoHasta,
                          btnDocumentoDesde, btnDocumentoHasta)
        ft.habilitarObjetos(ver, ver, txtDocumentoDesde, txtDocumentoHasta)
    End Sub
    Private Sub VerCriterio_Periodo(ByVal Ver As Boolean, ByVal CompletoDesdeHasta As Integer, Optional ByVal Periodo As TipoPeriodo = TipoPeriodo.iMensual)
        'CompletoDesdeHasta 0 = Complete , 1 = Desde , 2 = Hasta 

        ft.visualizarObjetos(False, lblPeriodoDesde, lblPeriodoHasta, lblPeriodo, txtPeriodoDesde, txtPeriodoHasta)
        PeriodoTipo = Periodo
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
        'DocumentosTipo : 0 = Bancos, 1 = caja, 2 = Forma de pago, 3 = CxP/CxC
        ft.visualizarObjetos(ver, lblTipodocumento, chkList)
        'ft.habilitarObjetos(ver, False, lblTipodocumento, chkList)
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
                Dim aTipoDocumento() As String = {"EF", "CH", "TA", "CT", "TR", "DP"}
                Dim aSel() As Boolean = {True, True, True, True}
                RellenaListaSeleccionable(chkList, aTipoDocumento, aSel)
                txtTipDoc.Text = ".EF.CH.TA.CT.TR.DP"
            Case 3
                Dim aTipoDocumento() As String = {"FC", "GR", "ND", "AB", "CA", "NC"}
                Dim aSel() As Boolean = {True, True, True, True, True, True}
                RellenaListaSeleccionable(chkList, aTipoDocumento, aSel)
                txtTipDoc.Text = ".FC.GR.ND.AB.CA.NC"

        End Select
    End Sub
    Private Sub VerCriterio_MesAño(ByVal ver As Boolean)
        'Dim aOBJ() As Object = {lblMesAño, lblMes, lblAño, cmbMes, cmbAño}
        'ft.visualizarObjetos(aOBJ, ver)
        'IniciarCriterioMesAño()
    End Sub

    Private Sub IniciarConstantes()
        ValoresInicialesConstantes()

        verConstante_Resumen(False)
        VerConstante_Tarifas(False)
        VerConstante_peso(False)
        VerConstante_TipoProveedor(False)
        VerConstante_Cartera(False)
        VerConstante_Regulado(False)
        VerConstante_Estatus(False)
        verConstante_Existencias(False)
        VerConstante_Lapsos(False)
        verConstante_Cuenta(False)

        Select Case ReporteNumero
            Case ReporteCompras.cListadoCompras,
                ReporteCompras.cListadoNotasCredito, ReporteCompras.cListadoNotasDebito, ReporteCompras.cListadoGastos,
                ReporteCompras.cProveedores, ReporteCompras.cListadoDocumentosSinRetencionIVA
                VerConstante_TipoProveedor(True)
                If ReporteNumero = ReporteCompras.cListadoCompras Or ReporteNumero = ReporteCompras.cListadoGastos Then
                    VerConstante_Cartera(True)
                    lblCartera.Text = "Condición Crédito"
                    Dim aCR() = {"Crédito", "Contado", "Todos"}
                    ft.RellenaCombo(aCR, cmbCartera)

                End If
            Case ReporteCompras.cListadoOrdenesDeCompra, ReporteCompras.cListadoRecepciones
                VerConstante_TipoProveedor(True)
                VerConstante_Estatus(True)
                verConstante_Resumen(True)
            Case ReporteCompras.cSaldosProveedores, ReporteCompras.cEstadodeCuentasProveedores, ReporteCompras.cMovimientosProveedores,
                ReporteCompras.cAuditoriasProveedores
                VerConstante_TipoProveedor(True)
                VerConstante_Estatus(True)
                verConstante_Cuenta(True)

            Case ReporteCompras.cVencimientos, ReporteCompras.cVencimientosResumen
                VerConstante_TipoProveedor(True)
                VerConstante_Estatus(True)
                VerConstante_Lapsos(True)
                verConstante_Cuenta(True)
            Case ReporteCompras.cLibroIVA
                VerConstante_TipoProveedor(True)

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
        ft.RellenaCombo(aSiNoTodos, cmbCartera, 2)
        ft.RellenaCombo(aSiNoTodos, cmbRegulada, 2)
        ft.RellenaCombo(aTipoProveedor, cmbTipo, 2)
        If ReporteNumero = ReporteCompras.cListadoRecepciones Or
            ReporteNumero = ReporteCompras.cListadoOrdenesDeCompra Then
            ft.RellenaCombo(aEstatusDocumento, cmbEstatus)
        Else
            ft.RellenaCombo(aEstatus, cmbEstatus)
        End If

        ft.RellenaCombo(aExistencias, cmbExistencias, 2)
        ft.RellenaCombo(aCuenta, cmbCuenta, CxP_ExP)

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

    Private Sub verConstante_Cuenta(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblCuenta, cmbCuenta)
    End Sub

    Private Sub verConstante_Existencias(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblExistencias, cmbExistencias)
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
    Private Sub VerConstante_Cartera(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblCartera, cmbCartera)
    End Sub
    Private Sub VerConstante_TipoProveedor(ByVal Ver As Boolean)
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
    Private Sub IniciarCriterioMesAño()

        '        Dim aMeses() As String = {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"}
        '       ft.RellenaCombo(aMeses, cmbMes, Month(jytsistema.sFechadeTrabajo) - 1)
        '      Dim aAño() As String = {"2000", "2001", "2002", "2003", "2004", "2005", "2006", "2007", "2008", "2009", "2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019", "2020", "2021", "2022", "2023", "2024", "2025"}
        '     ft.RellenaCombo(aAño, cmbAño, Year(jytsistema.sFechadeTrabajo) - 2000)

    End Sub

    Private Sub jsComRepParametros_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsComRepParametros_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(myConn, MovAud.ientrar, sModulo, ReporteNumero)
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

        HabilitarCursorEnEspera()

        EsperaPorFavor()

        Dim r As New frmViewer
        Dim dsCOM As New dsCompras
        Dim str As String = ""
        Dim strIVA As String = ""
        Dim strDescuentos As String = ""
        Dim strComentarios As String = ""
        Dim nTabla As String = ""
        Dim nTablaIVA As String = ""
        Dim nTablaDescuentos As String = ""
        Dim nTablaComentarios As String = ""
        Dim oReporte As New CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim PresentaArbol As Boolean = False

        Select Case ReporteNumero
            Case ReporteCompras.cOrdenDeCompra
                nTabla = "dtOrdenesDeCompra"
                nTablaIVA = "dtIVA"
                nTablaComentarios = "dtComentarios"

                oReporte = New rptComprasOrdenDeCompra

                str = SeleccionCOMPRASOrdenDeCompra(Documento, CodigoProveedor, FechaParametro)
                strIVA = SeleccionVENIVADocumento(Documento, "jsproivaord", "numord", " codpro = '" & CodigoProveedor & "' and ")
                strComentarios = SeleccionGENComentarios("ORD")
            Case ReporteCompras.cRecepcion
                nTabla = "dtOrdenesDeCompra"
                nTablaIVA = "dtIVA"
                nTablaComentarios = "dtComentarios"

                oReporte = New rptComprasRecepcion

                str = SeleccionCOMPRASRecepcion(Documento, CodigoProveedor, FechaParametro)
                strIVA = SeleccionVENIVADocumento(Documento, "jsproivarec", "numrec", " codpro = '" & CodigoProveedor & "' and ")
                strComentarios = SeleccionGENComentarios("REC")

            Case ReporteCompras.cListaRetencionesISLR
                nTabla = "dtRetencionesISLR"
                oReporte = New rptComprasRetencionesISLR
                str = SeleccionCOMPRASListadoRetencionesISLR(txtPeriodoDesde.Value, txtPeriodoHasta.Value,
                                                             txtProveedorDesde.Text, txtProveedorHasta.Text)
            Case ReporteCompras.cListaRetencionesIVA
                nTabla = "dtRetencionesIVA"
                oReporte = New rptComprasRetencionesIVA
                str = SeleccionCOMPRASListadoRetencionesIVA(txtPeriodoDesde.Value, txtPeriodoHasta.Value,
                                                             txtProveedorDesde.Text, txtProveedorHasta.Text)
            Case ReporteCompras.cGrupoSubGrupo
                nTabla = "dtGrupoSubGrupo"
                oReporte = New rptGENTablaSimpleGrupoSubGrupo
                str = strGrupoSubgrupo

            Case ReporteCompras.cGasto
                nTabla = "dtOrdenesDeCompra"
                nTablaIVA = "dtIVA"
                nTablaComentarios = "dtComentarios"
                nTablaDescuentos = "dtDescuentos"
                oReporte = New rptComprasGasto

                str = SeleccionCOMPRASGasto(Documento, CodigoProveedor, FechaParametro)
                strIVA = SeleccionVENIVADocumento(Documento, "jsproivagas", "numgas", " codpro = '" & CodigoProveedor & "' and ")
                strDescuentos = SeleccionVENDescuentosDocumento(Documento, "jsprodesgas", "numgas", " codpro = '" & CodigoProveedor & "' and ")
                strComentarios = SeleccionGENComentarios("GAS")
            Case ReporteCompras.cCompra
                nTabla = "dtOrdenesDeCompra"
                nTablaIVA = "dtIVA"
                nTablaComentarios = "dtComentarios"
                nTablaDescuentos = "dtDescuentos"
                oReporte = New rptComprasCompra

                str = SeleccionCOMPRASCompra(Documento, CodigoProveedor, FechaParametro)
                strIVA = SeleccionVENIVADocumento(Documento, "jsproivacom", "numcom", " codpro = '" & CodigoProveedor & "' and ")
                strDescuentos = SeleccionVENDescuentosDocumento(Documento, "jsprodescom", "numcom", " codpro = '" & CodigoProveedor & "' and ")
                strComentarios = SeleccionGENComentarios("COM")

            Case ReporteCompras.cNotaCredito
                nTabla = "dtOrdenesDeCompra"
                nTablaIVA = "dtIVA"
                nTablaComentarios = "dtComentarios"
                nTablaDescuentos = "dtDescuentos"
                oReporte = New rptComprasNotaCredito

                str = SeleccionCOMPRASNotaCredito(Documento, CodigoProveedor, FechaParametro)
                strIVA = SeleccionVENIVADocumento(Documento, "jsproivancr", "numncr", " codpro = '" & CodigoProveedor & "' and ")
                strComentarios = SeleccionGENComentarios("NCC")
            Case ReporteCompras.cNotaDebito

                nTabla = "dtOrdenesDeCompra"
                nTablaIVA = "dtIVA"
                nTablaComentarios = "dtComentarios"
                nTablaDescuentos = "dtDescuentos"
                oReporte = New rptComprasNotaDebito

                str = SeleccionCOMPRASNotaDebito(Documento, CodigoProveedor, FechaParametro)
                strIVA = SeleccionVENIVADocumento(Documento, "jsproivandb", "numndb", " codpro = '" & CodigoProveedor & "' and ")
                strComentarios = SeleccionGENComentarios("NDC")
            Case ReporteCompras.cComprobantePago
                nTabla = "dtComprobantePago"
                oReporte = New rptComprasRecibo
                str = SeleccionCOMPRASComprobantePagoCH(Documento)
            Case ReporteCompras.cRetencionISLR
                nTabla = "dtRetencionesISLR"
                oReporte = New rptComprasRetencionISLR
                str = SeleccionCOMPRASRetencionISLR(Documento, CodigoProveedor)
            Case ReporteCompras.cRetencionIVA
                nTabla = "dtRetencionesIVA"
                oReporte = New rptComprasRetencionIVA
                str = SeleccionCOMPRASRetencionIVA(Documento)
            Case ReporteCompras.cListadoOrdenesDeCompra

                nTabla = "dtListadoComprasItems"
                Select Case cmbCOMAgrupadoPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptComprasListadoDoc_Items0G
                    Case 1, 2 '1 Grupo
                        oReporte = New rptComprasListadoDoc_Items1G
                        PresentaArbol = True
                    Case 3, 4 '2 grupos
                        oReporte = New rptComprasListadoDoc_Items2G
                        PresentaArbol = True
                    Case Else
                        oReporte = New rptComprasListadoDoc_Items0G
                End Select

                str = SeleccionCOMPRASListadoOrdenesDeCompra(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtProveedorDesde.Text, txtProveedorHasta.Text,
                                           txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtUnidadDesde.Text, txtUnidadHasta.Text,
                                           cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex)

            Case ReporteCompras.cListadoRecepciones
                nTabla = "dtListadoComprasItems"
                Select Case cmbCOMAgrupadoPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptComprasListadoDoc_Items0G
                    Case 1, 2 '1 Grupo
                        oReporte = New rptComprasListadoDoc_Items1G
                        PresentaArbol = True
                    Case 3, 4 '2 grupos
                        oReporte = New rptComprasListadoDoc_Items2G
                        PresentaArbol = True
                    Case Else
                        oReporte = New rptComprasListadoDoc_Items2G
                End Select

                str = SeleccionCOMPRASListadoRecepciones(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtProveedorDesde.Text, txtProveedorHasta.Text,
                                           txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtUnidadDesde.Text, txtUnidadHasta.Text,
                                           cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex)

            Case ReporteCompras.cListadoCompras

                nTabla = "dtListadoCompras"
                Select Case cmbCOMAgrupadoPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptComprasListadoDocumentos0G
                    Case 1, 2 '1 Grupo
                        oReporte = New rptComprasListadoDocumentos1G
                        PresentaArbol = True
                    Case 3, 4 '2 grupos
                        oReporte = New rptComprasListadoDocumentos2G
                        PresentaArbol = True
                    Case 5 '3 grupos
                        oReporte = New rptComprasListadoDocumentos3G
                        PresentaArbol = True
                    Case Else
                        oReporte = New rptComprasListadoDocumentos0G
                End Select

                str = SeleccionCOMPRASListadoCompras(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtProveedorDesde.Text, txtProveedorHasta.Text,
                                           txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtUnidadDesde.Text, txtUnidadHasta.Text,
                                           cmbTipo.SelectedIndex, cmbCartera.SelectedIndex)

            Case ReporteCompras.cListadoDocumentosSinRetencionIVA

                nTabla = "dtListadoCompras"
                Select Case cmbCOMAgrupadoPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptComprasListadoDocumentos0G
                    Case 1, 2 '1 Grupo
                        oReporte = New rptComprasListadoDocumentos1G
                        PresentaArbol = True
                    Case 3, 4 '2 grupos
                        oReporte = New rptComprasListadoDocumentos2G
                        PresentaArbol = True
                    Case 5 '3 grupos
                        oReporte = New rptComprasListadoDocumentos3G
                        PresentaArbol = True
                    Case Else
                        oReporte = New rptComprasListadoDocumentos0G
                End Select

                str = SeleccionCOMPRASListadoComprasGastosNCRSinRetencion(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtProveedorDesde.Text, txtProveedorHasta.Text,
                                           txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtUnidadDesde.Text, txtUnidadHasta.Text,
                                           cmbTipo.SelectedIndex)

            Case ReporteCompras.cListadoNotasCredito
                nTabla = "dtListadoCompras"
                Select Case cmbCOMAgrupadoPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptComprasListadoDocumentos0G
                    Case 1, 2 '1 Grupo
                        oReporte = New rptComprasListadoDocumentos1G
                        PresentaArbol = True
                    Case 3, 4 '2 grupos
                        oReporte = New rptComprasListadoDocumentos2G
                        PresentaArbol = True
                    Case 5 '3 grupos
                        oReporte = New rptComprasListadoDocumentos3G
                        PresentaArbol = True
                    Case Else
                        oReporte = New rptComprasListadoDocumentos0G
                End Select

                str = SeleccionCOMPRASListadoNotasCredito(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtProveedorDesde.Text, txtProveedorHasta.Text,
                                           txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtUnidadDesde.Text, txtUnidadHasta.Text,
                                           cmbTipo.SelectedIndex)
            Case ReporteCompras.cListadoNotasDebito

                nTabla = "dtListadoCompras"
                Select Case cmbCOMAgrupadoPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptComprasListadoDocumentos0G
                    Case 1, 2 '1 Grupo
                        oReporte = New rptComprasListadoDocumentos1G
                        PresentaArbol = True
                    Case 3, 4 '2 grupos
                        oReporte = New rptComprasListadoDocumentos2G
                        PresentaArbol = True
                    Case 5 '3 grupos
                        oReporte = New rptComprasListadoDocumentos3G
                        PresentaArbol = True
                    Case Else
                        oReporte = New rptComprasListadoDocumentos0G
                End Select

                str = SeleccionCOMPRASListadoNotasDebito(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtProveedorDesde.Text, txtProveedorHasta.Text,
                                           txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtUnidadDesde.Text, txtUnidadHasta.Text,
                                           cmbTipo.SelectedIndex)

            Case ReporteCompras.cListadoGastos

                nTabla = "dtListadoCompras"
                Select Case cmbCOMAgrupadoPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptComprasListadoDocumentos0G
                    Case 1, 2 '1 Grupo
                        oReporte = New rptComprasListadoDocumentos1G
                        PresentaArbol = True
                    Case 3, 4 '2 grupos
                        oReporte = New rptComprasListadoDocumentos2G
                        PresentaArbol = True
                    Case 5 '3 grupos
                        oReporte = New rptComprasListadoDocumentos3G
                        PresentaArbol = True
                    Case Else
                        oReporte = New rptComprasListadoDocumentos0G
                End Select

                str = SeleccionCOMPRASListadoGastos(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtProveedorDesde.Text, txtProveedorHasta.Text,
                                           txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtUnidadDesde.Text, txtUnidadHasta.Text,
                                           txtGrupo.Text, txtSubgrupo.Text,
                                           cmbTipo.SelectedIndex, cmbCartera.SelectedIndex)

            Case ReporteCompras.cFichaProveedor
                nTabla = "dtProveedor"
                oReporte = New rptComprasFichaProveedor
                str = SeleccionCOMPRASProveedores(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtUnidadDesde.Text, txtUnidadHasta.Text,
                                           cmbTipo.SelectedIndex)

            Case ReporteCompras.cProveedores

                nTabla = "dtProveedor"
                Select Case cmbCOMAgrupadoPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptComprasProveedores0G
                    Case 1, 2 '1 Grupo
                        oReporte = New rptComprasProveedores1G
                        PresentaArbol = True
                    Case 3
                        oReporte = New rptComprasProveedores2G
                        PresentaArbol = True
                    Case Else
                        oReporte = New rptComprasProveedores0G
                End Select

                str = SeleccionCOMPRASProveedores(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtUnidadDesde.Text, txtUnidadHasta.Text,
                                           cmbTipo.SelectedIndex)

            Case ReporteCompras.cSaldosProveedores

                nTabla = "dtProveedor"
                Select Case cmbCOMAgrupadoPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptComprasSaldosProveedores0G
                    Case 1, 2 '1 Grupo
                        oReporte = New rptComprasSaldosProveedores1G
                        PresentaArbol = True
                    Case 3
                        oReporte = New rptComprasSaldosProveedores2G
                        PresentaArbol = True
                    Case Else
                        oReporte = New rptComprasSaldosProveedores0G
                End Select

                str = SeleccionCOMPRASSaldoProveedores(myConn, lblInfo, txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                           txtPeriodoHasta.Value, txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtUnidadDesde.Text, txtUnidadHasta.Text,
                                           cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex, cmbCuenta.SelectedIndex)

            Case ReporteCompras.cEstadodeCuentasProveedores

                nTabla = "dtMovimientos"
                oReporte = New rptComprasEstadodeCuentaProveedores0G
                str = SeleccionCOMPRASEstadoDeCuentaProveedores(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                   txtPeriodoDesde.Value, txtPeriodoHasta.Value,
                                   cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex, cmbCuenta.SelectedIndex)

            Case ReporteCompras.cAuditoriasProveedores

                nTabla = "dtMovimientos"
                oReporte = New rptComprasAuditoriasProveedores

                str = SeleccionCOMPRASAuditoriaProveedores(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                             txtPeriodoHasta.Value, txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtUnidadDesde.Text, txtUnidadHasta.Text,
                                             cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex, cmbCuenta.SelectedIndex)

            Case ReporteCompras.cVencimientos
                nTabla = "dtMovimientos"
                oReporte = New rptComprasVencimientos

                str = SeleccionCOMPRASVencimientos(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                             txtPeriodoHasta.Value, txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtUnidadDesde.Text, txtUnidadHasta.Text,
                                             cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex,
                                             CInt(txtDesde1.Text), CInt(txtHasta1.Text), CInt(txtDesde2.Text), CInt(txtHasta2.Text),
                                             CInt(txtDesde3.Text), CInt(txtHasta3.Text), CInt(txtDesde4.Text), cmbCuenta.SelectedIndex)

            Case ReporteCompras.cVencimientosResumen
                nTabla = "dtVencimientosR"
                oReporte = New rptComprasVencimientosResumen

                str = SeleccionCOMPRASVencimientosResumen(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                             txtPeriodoHasta.Value, txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtUnidadDesde.Text, txtUnidadHasta.Text,
                                             cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex,
                                             CInt(txtDesde1.Text), CInt(txtHasta1.Text), CInt(txtDesde2.Text), CInt(txtHasta2.Text),
                                             CInt(txtDesde3.Text), CInt(txtHasta3.Text), CInt(txtDesde4.Text), cmbCuenta.SelectedIndex)

            Case ReporteCompras.cMovimientosProveedores

                nTabla = "dtMovimientos"
                oReporte = New rptComprasMovimientosProveedores0G

                str = SeleccionCOMPRASMovimientosProveedores(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                   txtPeriodoDesde.Value, txtPeriodoHasta.Value, txtTipDoc.Text,
                                   cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex, cmbCuenta.SelectedIndex, txtCausaNC_Desde.Text, txtCausaNC_Hasta.Text)

            Case ReporteCompras.cLibroIVA

                nTabla = "dtLibroIVA"
                oReporte = New rptComprasLibroIVAGeneral
                str = SeleccionCOMPRASLibroIVA(myConn, lblInfo,
                                   txtPeriodoDesde.Value, txtPeriodoHasta.Value, , , , , cmbTipo.SelectedIndex,
                                   txtProveedorDesde.Text, txtProveedorHasta.Text)

            Case Else
                oReporte = Nothing
        End Select

        If nTabla <> "" Then
            dsCOM = DataSetRequery(dsCOM, str, myConn, nTabla, lblInfo)
            If ReporteNumero = ReporteCompras.cOrdenDeCompra Or
                ReporteNumero = ReporteCompras.cRecepcion Or
                ReporteNumero = ReporteCompras.cGasto Or
                ReporteNumero = ReporteCompras.cCompra Or
                ReporteNumero = ReporteCompras.cNotaCredito Or
                ReporteNumero = ReporteCompras.cNotaDebito Then
                dsCOM = DataSetRequery(dsCOM, strIVA, myConn, nTablaIVA, lblInfo)
                If strDescuentos <> "" Then dsCOM = DataSetRequery(dsCOM, strDescuentos, myConn, nTablaDescuentos, lblInfo)
                dsCOM = DataSetRequery(dsCOM, strComentarios, myConn, nTablaComentarios, lblInfo)
            End If
            If dsCOM.Tables(nTabla).Rows.Count > 0 Then
                oReporte = PresentaReporte(oReporte, dsCOM, nTabla)
                r.CrystalReportViewer1.ReportSource = oReporte
                r.CrystalReportViewer1.ToolPanelView = IIf(PresentaArbol, CrystalDecisions.Windows.Forms.ToolPanelViewType.GroupTree,
                                              CrystalDecisions.Windows.Forms.ToolPanelViewType.None)
                r.CrystalReportViewer1.ShowGroupTreeButton = PresentaArbol
                r.CrystalReportViewer1.Zoom(1)
                r.CrystalReportViewer1.Refresh()
                r.Cargar(ReporteNombre)
                DeshabilitarCursorEnEspera()
            Else
                ft.mensajeCritico("No existe información que cumpla con estos criterios y/o constantes ")
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
            Case ReporteCompras.cOrdenDeCompra, ReporteCompras.cRecepcion, ReporteCompras.cNotaCredito,
                ReporteCompras.cNotaDebito
                oReporte.Subreports("rptGENIVA.rpt").SetDataSource(ds.Tables("dtIVA"))
                oReporte.Subreports("rptGENComentarios.rpt").SetDataSource(ds.Tables("dtComentarios"))
            Case ReporteCompras.cGasto, ReporteCompras.cCompra
                oReporte.Subreports("rptGENIVA.rpt").SetDataSource(ds.Tables("dtIVA"))
                oReporte.Subreports("rptGENDescuentos.rpt").SetDataSource(ds.Tables("dtDescuentos"))
                oReporte.Subreports("rptGENComentarios.rpt").SetDataSource(ds.Tables("dtComentarios"))
        End Select

        Dim dirFiscalWork As String = ft.DevuelveScalarCadena(myConn, " select dirfiscal from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ")

        oReporte.SetDataSource(ds)
        oReporte.Refresh()
        oReporte.SetParameterValue("strLogo", CaminoImagen)
        oReporte.SetParameterValue("Orden", IIf(ReporteCompras.cLibroIVA, "", "Ordenado por : " + cmbOrdenadoPor.Text + " " + txtOrdenDesde.Text + "/" + txtOrdenHasta.Text))
        oReporte.SetParameterValue("RIF", "RIF : " & rif)
        oReporte.SetParameterValue("Grupo", LineaGrupos)
        oReporte.SetParameterValue("Criterios", LineaCriterios)
        oReporte.SetParameterValue("Constantes", LineaConstantes)
        oReporte.SetParameterValue("Empresa", jytsistema.WorkName.TrimEnd(" "))

        Select Case ReporteNumero
            Case ReporteCompras.cGrupoSubGrupo
                oReporte.SetParameterValue("Titulo", ReporteNombre)
                oReporte.SetParameterValue("DescripGrupo", titleGrupo)
                oReporte.SetParameterValue("DescripSubGrupo", titleSubGrupo)
            Case ReporteCompras.cRetencionIVA
                Dim fFecha As Date = ft.DevuelveScalarFecha(myConn, " select emision from jsprotrapag where refer = '" & Documento & "' and codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "' ")
                oReporte.SetParameterValue("Criterios", "PERIODO FISCAL - AÑO : " & Year(fFecha) & " - MES : " & Format(Month(fFecha), "00"))
                oReporte.SetParameterValue("Grupo", "DOMICILIO FISCAL : " & ft.DevuelveScalarCadena(myConn, " select DIRFISCAL from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' "))
            Case ReporteCompras.cRetencionISLR
                oReporte.SetParameterValue("Criterios", "")
                oReporte.SetParameterValue("Grupo", "DOMICILIO FISCAL : " & ft.DevuelveScalarCadena(myConn, " select DIRFISCAL from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' "))
            Case ReporteCompras.cComprobantePago
                oReporte.SetParameterValue("MontoEnLetras", NumerosATexto(ft.DevuelveScalarDoble(myConn, "select SUM(IMPORTE) from jsprotrapag where comproba = '" & Documento & "' and id_emp = '" & jytsistema.WorkID & "' ")))
            Case ReporteCompras.cOrdenDeCompra
                oReporte.SetParameterValue("MontoEnLetras", NumerosATexto(ft.DevuelveScalarDoble(myConn, "select tot_ord from jsproencord where numord = '" & Documento & "' and codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "' ")))
            Case ReporteCompras.cRecepcion
                oReporte.SetParameterValue("MontoEnLetras", NumerosATexto(ft.DevuelveScalarDoble(myConn, "select tot_rec from jsproencrep where numrec = '" & Documento & "' and codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "' ")))
            Case ReporteCompras.cGasto
                oReporte.SetParameterValue("MontoEnLetras", NumerosATexto(ft.DevuelveScalarDoble(myConn, "select tot_gas from jsproencgas where numgas = '" & Documento & "' and codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "' ")))
            Case ReporteCompras.cCompra
                oReporte.SetParameterValue("MontoEnLetras", NumerosATexto(ft.DevuelveScalarDoble(myConn, "select tot_com from jsproenccom where numcom = '" & Documento & "' and codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "' ")))
            Case ReporteCompras.cNotaCredito
                oReporte.SetParameterValue("MontoEnLetras", NumerosATexto(ft.DevuelveScalarDoble(myConn, "select tot_ncr from jsproencncr where numncr = '" & Documento & "' and codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "' ")))
            Case ReporteCompras.cNotaDebito
                oReporte.SetParameterValue("MontoEnLetras", NumerosATexto(ft.DevuelveScalarDoble(myConn, "select tot_ndb from jsproencndb where numndb = '" & Documento & "' and codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "' ")))
            Case ReporteCompras.cVencimientosResumen
                oReporte.SetParameterValue("Lapso0", " -" & txtDesde1.Text & " día")
                oReporte.SetParameterValue("Lapso1", "de " & txtDesde1.Text & " a " & txtHasta1.Text & " días")
                oReporte.SetParameterValue("Lapso2", "de " & txtDesde2.Text & " a " & txtHasta2.Text & " días")
                oReporte.SetParameterValue("Lapso3", "de " & txtDesde3.Text & " a " & txtHasta3.Text & " días")
                oReporte.SetParameterValue("Lapso4", txtDesde4.Text & " días ó más")
            Case ReporteCompras.cListadoOrdenesDeCompra, ReporteCompras.cListadoRecepciones, ReporteCompras.cListadoCompras,
                ReporteCompras.cListadoNotasCredito, ReporteCompras.cListadoNotasDebito, ReporteCompras.cListadoGastos,
                ReporteCompras.cSaldosProveedores, ReporteCompras.cEstadodeCuentasProveedores, ReporteCompras.cMovimientosProveedores,
                ReporteCompras.cListadoDocumentosSinRetencionIVA

                If ReporteNumero = ReporteCompras.cEstadodeCuentasProveedores Then _
                    oReporte.SetParameterValue("SaldoAl", "Saldo al " & ft.FormatoFecha(DateAdd("d", -1, txtPeriodoDesde.Value)))

                If ReporteNumero = ReporteCompras.cListadoOrdenesDeCompra Then
                    oReporte.SetParameterValue("Titulo", "Ordenes de Compra")
                    oReporte.SetParameterValue("Resumen", chkConsResumen.Checked)
                End If

                If ReporteNumero = ReporteCompras.cListadoRecepciones Then
                    oReporte.SetParameterValue("Titulo", "Recepciones")
                    oReporte.SetParameterValue("Resumen", chkConsResumen.Checked)
                End If

                If ReporteNumero = ReporteCompras.cListadoCompras Then _
                        oReporte.SetParameterValue("Titulo", "Compras")
                If ReporteNumero = ReporteCompras.cListadoDocumentosSinRetencionIVA Then _
                       oReporte.SetParameterValue("Titulo", "COMPRAS/GASTOS/NOTAS CREDITO SIN RETENCION FISCAL IVA")

                If ReporteNumero = ReporteCompras.cListadoNotasCredito Then _
                        oReporte.SetParameterValue("Titulo", "Notas de Crédito")

                If ReporteNumero = ReporteCompras.cListadoNotasDebito Then _
                        oReporte.SetParameterValue("Titulo", "Notas Débito")

                If ReporteNumero = ReporteCompras.cListadoGastos Then _
                        oReporte.SetParameterValue("Titulo", "Gastos")


                Select Case cmbCOMAgrupadoPor.SelectedIndex
                    Case 0
                    Case 1, 2
                        oReporte.SetParameterValue("Grupo1", "categoriaprov")
                        Dim FieldDef1 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(aCOMAgrupadoPor(cmbCOMAgrupadoPor.SelectedIndex))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                    Case 3, 4
                        oReporte.SetParameterValue("Grupo1", "categoriaprov")
                        oReporte.SetParameterValue("Grupo2", "unidadnegocioprov")
                        Dim aFld() As String = Split(aCOMAgrupadoPor(cmbCOMAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(0)))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(1)))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                    Case Else
                        oReporte.SetParameterValue("Grupo1", "categoriaprov")
                        oReporte.SetParameterValue("Grupo2", "unidadnegocioprov")
                        oReporte.SetParameterValue("Grupo3", "grupo")
                        Dim aFld() As String = Split(aCOMAgrupadoPor(cmbCOMAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2, FieldDef3 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(0)))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(1)))
                        FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(2)))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                End Select

        End Select

        PresentaReporte = oReporte

    End Function

    Private Sub btnLimpiar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLimpiar.Click
        LimpiarOrden()
        LimpiarGrupos()
        LimpiarCriterios()
    End Sub
    Private Sub LimpiarGrupos()
        LimpiarTextos(txtCategoriaDesde, txtCategoriaHasta, txtUnidadDesde, txtUnidadHasta)
    End Sub
    Private Sub LimpiarOrden()
        LimpiarTextos(txtOrdenDesde, txtOrdenHasta)
    End Sub
    Private Sub LimpiarCriterios()
        LimpiarTextos(txtProveedorDesde, txtProveedorHasta, txtGrupo, txtSubgrupo)
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
    Private Function LineaGrupos() As String

        LineaGrupos = ""
        If ReporteNumero = ReporteCompras.cLibroIVA Or
            ReporteNumero = ReporteCompras.cListaRetencionesISLR Or
            ReporteNumero = ReporteCompras.cListaRetencionesIVA Then
            LineaGrupos = "Dirección comercial y fiscal :" _
                & ft.DevuelveScalarCadena(myConn, " select dirfiscal from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ") _
                & ". Telefonos : " & ft.DevuelveScalarCadena(myConn, " select telef1 from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ") & ". " _
                & ft.DevuelveScalarCadena(myConn, " select telef2 from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ") & ". " _
                & ft.DevuelveScalarCadena(myConn, " select fax from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ") & ". e-mail : " _
                & ft.DevuelveScalarCadena(myConn, " select email from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ")
        Else
            LineaGrupos = "Agrupado por : "
            If lblCategoria.Visible Then LineaGrupos += "Categoría: " & txtCategoriaDesde.Text & "/" & txtCategoriaHasta.Text
            If LineaGrupos <> "" Then LineaGrupos += " - "
            If lblUnidad.Visible Then LineaGrupos += "Unidad Negocio: " & txtUnidadDesde.Text & "/" & txtUnidadHasta.Text
        End If


    End Function
    Private Function LineaCriterios() As String

        If ReporteNumero = ReporteCompras.cLibroIVA Then

            LineaCriterios = "MES : " & UCase(Format(txtPeriodoDesde.Value, "MMMM")) &
                            " - AÑO : " & CDate(txtPeriodoHasta.Text).Year.ToString()
        Else
            LineaCriterios = "Criterios : "
            If lblPeriodo.Visible Then LineaCriterios += "Período: " & IIf(lblPeriodoDesde.Visible, txtPeriodoDesde.Text, "") & IIf(lblPeriodoDesde.Visible AndAlso lblPeriodoHasta.Visible, "/", "") & IIf(lblPeriodoHasta.Visible, txtPeriodoHasta.Text, "")
            If lblProveedor.Visible Then LineaCriterios += " - " + " Proveedor : " + txtProveedorDesde.Text + "/" + txtProveedorHasta.Text
            If lblGrupoSubgrupo.Visible Then LineaCriterios += " - " + "Grupo/Subgrupo: " & txtGrupo.Text & "/" & txtSubgrupo.Text

        End If
    End Function
    Private Function LineaConstantes() As String
        If ReporteCompras.cLibroIVA Then
            LineaConstantes = ""
        Else
            LineaConstantes = "Constantes : "
            If lblConsResumen.Visible Then LineaConstantes += "Resumido : " + IIf(chkConsResumen.Checked, "Si", "No")
            If LineaConstantes <> "" Then LineaConstantes += " - "
            If lblTipo.Visible Then LineaConstantes += "Tipo proveedor : " + aTipoProveedor(cmbTipo.SelectedIndex)
            If LineaConstantes <> "" Then LineaConstantes += " - "
            If lblLapso.Visible Then LineaConstantes += " Lapsos: 1. " + txtDesde1.Text + "-" + txtHasta1.Text +
                                                        " 2. " + txtDesde2.Text + "-" + txtHasta2.Text +
                                                        " 3. " + txtDesde3.Text + "-" + txtHasta3.Text +
                                                        " 4. " + txtDesde4.Text
            If lblCuenta.Visible Then LineaConstantes += " Cuenta : " + aCuenta(cmbCuenta.SelectedIndex)
        End If
    End Function
    Private Sub btnOrdenDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrdenDesde.Click
        txtDeOrden(txtOrdenDesde)
    End Sub

    Private Sub btnOrdenHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrdenHasta.Click
        txtDeOrden(txtOrdenHasta)
    End Sub
    Private Sub txtDeOrden(ByVal txt As TextBox)
        Select Case cmbOrdenadoPor.Text
            Case "Código proveedor"
                txt.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codpro codigo, nombre descripcion " _
                                             & " from jsprocatpro where id_emp = '" & jytsistema.WorkID _
                                             & " order by codpro ", "Proveedores", txt.Text)
                '& "'" & IIf(cmbTipo.SelectedIndex < 2, " and tipo = '" & cmbTipo.SelectedIndex & "' ", "") _

            Case "Nombre proveedor"
        End Select

    End Sub

    Private Sub txtOrdenDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOrdenDesde.TextChanged
        txtOrdenHasta.Text = txtOrdenDesde.Text
    End Sub

    Private Sub cmbCOMAgrupadorPor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCOMAgrupadoPor.SelectedIndexChanged

        ft.visualizarObjetos(False, lblGrupoDesde, lblGrupoHasta, lblCategoria, lblUnidad,
                            txtCategoriaDesde, btnCategoriaDesde, txtCategoriaHasta, btnCategoriaHasta,
                            txtUnidadDesde, btnUnidadDesde, txtUnidadHasta, btnUNidadHasta)
        LimpiarGrupos()
        Select Case cmbCOMAgrupadoPor.SelectedIndex
            Case 0
            Case 1
                VerCategorias()
            Case 2
                VerUnidad()
            Case 3 '"Categorías & Unidad"
                VerCategorias()
                VerUnidad()
            Case Else
                'VerCategorias()
                'VerUnidad()
        End Select
    End Sub
    Private Sub VerCategorias()
        ft.visualizarObjetos(True, lblGrupoDesde, lblGrupoHasta)
        ft.visualizarObjetos(True, lblCategoria, txtCategoriaDesde, btnCategoriaDesde, txtCategoriaHasta, btnCategoriaHasta)
        ft.habilitarObjetos(False, True, txtCategoriaDesde, txtCategoriaHasta)
    End Sub
    Private Sub VerUnidad()
        ft.visualizarObjetos(True, lblGrupoDesde)
        ft.visualizarObjetos(True, lblUnidad, txtUnidadDesde, btnUnidadDesde)
        ft.habilitarObjetos(False, True, txtUnidadDesde)
    End Sub
    Private Sub VerGrupoSubGrupo()
        ft.visualizarObjetos(True, lblGrupoSubgrupo, txtGrupo, txtSubgrupo, btnSubgrupo)
        ft.habilitarObjetos(False, True, txtGrupo, txtSubgrupo)
    End Sub
    Private Sub txtCategoriaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCategoriaDesde.TextChanged
        txtCategoriaHasta.Text = txtCategoriaDesde.Text
    End Sub

    Private Sub txtMarcaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUnidadDesde.TextChanged
        txtUnidadHasta.Text = txtUnidadDesde.Text
    End Sub

    Private Sub btnCategoriaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoriaDesde.Click
        If txtUnidadDesde.Text <> "" And txtUnidadHasta.Text <> "" Then
            txtCategoriaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion " _
                                                       & " from jsproliscat " _
                                                       & " where " _
                                                       & " ANTEC = '" & txtUnidadDesde.Text & "' AND " _
                                                       & " id_emp = '" & jytsistema.WorkID & "' " _
                                                       & " order by codigo  ", "Categorías", txtCategoriaDesde.Text)
        End If
    End Sub

    Private Sub btnCategoriaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoriaHasta.Click
        If txtUnidadDesde.Text <> "" And txtUnidadHasta.Text <> "" Then
            txtCategoriaHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion " _
                                                       & " from jsproliscat " _
                                                       & " where " _
                                                       & " ANTEC = '" & txtUnidadDesde.Text & "' AND " _
                                                       & " id_emp = '" & jytsistema.WorkID & "' " _
                                                       & " order by codigo  ", "Categorías", txtCategoriaHasta.Text)
        End If
    End Sub

    Private Sub btnUnidadDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnidadDesde.Click
        txtUnidadDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsprolisuni where id_emp = '" & jytsistema.WorkID & "' order by codigo  ", "Unidades de Negocio", txtUnidadDesde.Text)
    End Sub

    Private Sub btnUnidadHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUNidadHasta.Click
        txtUnidadHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsprolisuni where id_emp = '" & jytsistema.WorkID & "' order by codigo  ", "Unidades de Negocio", txtUnidadHasta.Text)
    End Sub

    Private Sub btnDivisionHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubgrupo.Click
        Dim f As New jsComArcGrupoSubgrupo
        f.Grupo0 = txtGrupo.Text
        f.Grupo1 = txtSubgrupo.Text
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        txtGrupo.Text = f.Grupo0
        txtSubgrupo.Text = f.Grupo1
        f = Nothing
    End Sub


    Private Sub btnProveedorDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProveedorDesde.Click
        txtProveedorDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codpro codigo, nombre descripcion from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by 1  ", "Proveedores", txtProveedorDesde.Text)
    End Sub

    Private Sub btnProveedorHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProveedorHasta.Click
        txtProveedorHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codpro codigo, nombre descripcion from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by 1  ", "Proveedores", txtProveedorHasta.Text)
    End Sub

    Private Sub txtProveedorDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProveedorDesde.TextChanged
        txtProveedorHasta.Text = txtProveedorDesde.Text
    End Sub

    Private Sub txtPeriodoDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtPeriodoDesde.ValueChanged
        Select Case PeriodoTipo
            Case TipoPeriodo.iDiario
                txtPeriodoHasta.Value = txtPeriodoDesde.Value
            Case TipoPeriodo.iSemanal
                txtPeriodoHasta.Value = DateAdd(DateInterval.Day, 7, CDate(txtPeriodoDesde.Text))
            Case TipoPeriodo.iMensual
                txtPeriodoHasta.Value = UltimoDiaMes(txtPeriodoDesde.Value)
            Case TipoPeriodo.iAnual
                txtPeriodoHasta.Value = UltimoDiaAño(txtPeriodoDesde.Value)
        End Select
    End Sub

    Private Sub btnCausaNC_Desde_Click(sender As Object, e As EventArgs) Handles btnCausaNC_Desde.Click
        txtCausaNC_Desde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descripcion from jsconcausas_notascredito where origen = 'CXP' order by codigo ", "CAUSAS NOTAS CREDITO NO FISCALES", txtCausaNC_Desde.Text)
    End Sub
    Private Sub btnCausaNC_hASTA_Click(sender As Object, e As EventArgs) Handles btnCausaNC_Hasta.Click
        txtCausaNC_Hasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descripcion from jsconcausas_notascredito where origen = 'CXP' order by codigo ", "CAUSAS NOTAS CREDITO NO FISCALES", txtCausaNC_Hasta.Text)
    End Sub


    Private Sub txtCausaNC_Desde_TextChanged(sender As Object, e As EventArgs) Handles txtCausaNC_Desde.TextChanged
        txtCausaNC_Hasta.Text = txtCausaNC_Desde.Text
    End Sub
End Class