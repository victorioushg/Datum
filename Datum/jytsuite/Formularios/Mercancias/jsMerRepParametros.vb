Imports MySql.Data.MySqlClient
Imports System.IO
Imports ReportesDeMercancias
Imports Syncfusion.WinForms.Input

Public Class jsMerRepParametros

    Private Const sModulo As String = "Reportes de Mercancías"

    Private ReporteNumero As Integer
    Private ReporteNombre As String
    Private CodigoMercancia As String, Documento As String
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

    Private vMERAgrupadoPor() As String = {"Ninguno", "Categorías", "Marcas", "Jerarquía", "División", "Mix",
        "Categorías & Marcas", "División & Categorías &  Marcas",
        "Categorías & Jerarquías",
        "División & Jerarquía", "Mix & Categorías & Marcas", "Mix & Jerarquías",
        "Mix & División", "Mix & División & Categorías & Marcas", "Mix & División & Jerarquías"}

    Private aMERAgrupadoPor() As String = {"", "categoria", "marca", "tipjer", "division", "mix",
                                        "categoria, marca", "division, categoria, marca",
                                        "categoria, tipjer", "division, tipjer",
                                        "mix, categoria, marca", "mix, tipjer", "mix, division",
                                        "mix, division, categoria, marca", "mix, division, tipjer"}
    Private aSiNoTodos() As String = {"Si", "No", "Todos"}
    Private aExistencias() As String = {"Con", "Sin", "Todas"}
    Private aEstatus() As String = {"Activo", "Inactivo", "Todos"}
    Private aTipo() As String = {"Venta", "Uso interno", "POP", "Alquiler", "Préstamo", "Materia prima", "Otros", "Todos"}
    Private aTarifa() As String = {"A", "B", "C", "D", "E", "F"}
    Private aTerritorio() As String = {"Barrio/Urbanización/Sector", "Ciudad/Pueblo/Aldea", "Parroquia/Comunidad", "Municipio/Región", "Estado/Provincia/Departamento", "País"}
    Private vVENAgrupadoPor() As String = {"Ninguno", "Canal de Distribución", "Tipo de Negocio", "Zona", "Ruta",
                                           "Asesor Comercial", "Canal & Tipo Negocio", "Zona & Ruta",
                                           "Asesor & Canal", "Asesor & Tipo de Negocio", "Asesor & Canal & Tipo de negocio",
                                           "Asesor & Zona", "Asesor & ruta", "Asesor & Zona & Ruta", "Territorio"}
    Private aVENAgrupadoPor() As String = {"", "canal", "tiponegocio", "zona", "ruta", "asesor", "canal, tiponegocio",
                                           "zona, ruta", "asesor, canal", "asesor, tiponegocio", "asesor, canal, tiponegocio",
                                           "asesor, zona", "asesor, ruta", "asesor, zona, ruta", "territorio"}
    Private aAño() As Object = {2000, 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020,
                                  2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028, 2029, 2030}

    Private aTipoEtiqueta() As String = {"Grande (58mm X 40mm) ", "Hablador (85mm X 37mm)", "Pequeña (30mm X 13mm)", "Hablador Nevera(85mm X 37mm)"}
    Private aTipoConteo() As String = {"CONTEO 1", "CONTEO 2", "DIFERENCIAS (1-2)"}

    Private periodoTipo As TipoPeriodo
    Public Sub Cargar(ByVal TipoCarga As Integer, ByVal numReporte As Integer, ByVal nomReporte As String,
                      Optional ByVal CodMercancia As String = "", Optional ByVal numDocumento As String = "",
                      Optional ByVal Fecha As Date = #1/1/2009#)


        Me.Tag = sModulo
        Me.Dock = DockStyle.Fill
        myConn = New MySqlConnection(jytsistema.strConn)
        myConn.Open()

        Dim dates As SfDateTimeEdit() = {txtPeriodoDesde, txtPeriodoHasta}
        SetSizeDateObjects(dates)

        ReporteNumero = numReporte
        ReporteNombre = nomReporte
        CodigoMercancia = CodMercancia
        Documento = numDocumento
        FechaParametro = Fecha

        ft.RellenaCombo(aDesde, cmbOrdenDesde)
        ft.RellenaCombo(aHasta, cmbOrdenHasta)
        ft.RellenaCombo(aTarifa, cmbTarifa)
        ft.RellenaCombo(aAño, cmbAño, Year(jytsistema.sFechadeTrabajo) - 2000)
        ft.RellenaCombo(aTipoEtiqueta, cmbTipoEtiqueta)

        PresentarReporte(numReporte, nomReporte, CodigoMercancia)


        If TipoCarga = TipoCargaFormulario.iShow Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If

    End Sub
    Private Sub PresentarReporte(ByVal NumeroReporte As Integer, ByVal NombreDelReporte As String, Optional ByVal CodigoMercancia As String = "")
        lblNombreReporte.Text += " - " + NombreDelReporte
        Select Case NumeroReporte
            Case ReporteMercancias.cFichaMercancia
                Dim vOrdenNombres() As String = {"Código mercancía"}
                Dim vOrdenCampos() As String = {"codart"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)
            Case ReporteMercancias.cCodigoBarras
                Dim vOrdenNombres() As String = {"Código mercancía"}
                Dim vOrdenCampos() As String = {"codart"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)

            Case ReporteMercancias.cCodigoBarraUbicacion
                Dim vOrdenNombres() As String = {"Código"}
                Dim vOrdenCampos() As String = {"CODUBI"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)

            Case ReporteMercancias.cCatalogo, ReporteMercancias.cPrecios, ReporteMercancias.cPreciosIVA
                Dim vOrdenNombres() As String = {"Código mercancía", "Nombre mercancía"}
                Dim vOrdenCampos() As String = {"codart", "nomart"}
                Dim vOrdenTipo() As String = {"S", "S"}
                Dim vOrdenLongitud() As Integer = {15, 150}
                Inicializar(ReporteNombre, True, True, False, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)
            Case ReporteMercancias.cObsolescencia
                Dim vOrdenNombres() As String = {"Días Obsolescencia"}
                Dim vOrdenCampos() As String = {"obsolescencia"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)
            Case ReporteMercancias.cMovimientosMercancia
                Dim vOrdenNombres() As String = {"Código Mercancía"}
                Dim vOrdenCampos() As String = {"codart"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)
            Case ReporteMercancias.cMovimientosServicios
                Dim vOrdenNombres() As String = {"Código Servicio"}
                Dim vOrdenCampos() As String = {"item"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)

            Case ReporteMercancias.cMovimientosResumen
                Dim vOrdenNombres() As String = {"Código Mercancía"}
                Dim vOrdenCampos() As String = {"codart"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, True, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)

            Case ReporteMercancias.cCategorias, ReporteMercancias.cMarcas, ReporteMercancias.cTransferencia,
                    ReporteMercancias.cConteos, ReporteMercancias.cPreciosEspeciales, ReporteMercancias.cOfertas,
                    ReporteMercancias.cJerarquias, ReporteMercancias.cPedidosAlmacen
                Dim vOrdenNombres() As String = {"Código"}
                Dim vOrdenCampos() As String = {"codigo"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)

            Case ReporteMercancias.cMovimientosMercancias,
                ReporteMercancias.cVentasDeMercancias, ReporteMercancias.cVentasDeMercanciasActivacion,
                ReporteMercancias.cVentasDeMercanciasActivacionXMes, ReporteMercancias.cComprasdeMercanciasActivacion,
                ReporteMercancias.cComprasdeMercanciasActivacionXMes, ReporteMercancias.cPreciosYEquivalencias,
                ReporteMercancias.cPreciosYEquivalenciasSinIVA,
                ReporteMercancias.cLeyDeCostos, ReporteMercancias.cVentasNetasMercancias

                Dim vOrdenNombres() As String = {"Código Mercancía", "Nombre Mercancía"}
                Dim vOrdenCampos() As String = {"codart", "nomart"}
                Dim vOrdenTipo() As String = {"S", "S"}
                Dim vOrdenLongitud() As Integer = {15, 150}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)
            Case ReporteMercancias.cMovimientosServiciosS

                Dim vOrdenNombres() As String = {"Código Servicio"}
                Dim vOrdenCampos() As String = {"item"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)


            Case ReporteMercancias.cInventarioLegal
                Dim vOrdenNombres() As String = {"Código Mercancía", "Nombre Mercancía"}
                Dim vOrdenCampos() As String = {"codart", "nomart"}
                Dim vOrdenTipo() As String = {"S", "S"}
                Dim vOrdenLongitud() As Integer = {15, 150}
                Inicializar(ReporteNombre, True, False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)

            Case ReporteMercancias.cPreciosFuturos
                Dim vOrdenNombres() As String = {"Código Mercancía"}
                Dim vOrdenCampos() As String = {"codart"}
                Dim vOrdenTipo() As String = {"S", "S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)

            Case ReporteMercancias.cVentasPlana

                Dim vOrdenNombres() As String = {"Código Mercancía"}
                Dim vOrdenCampos() As String = {"codart"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, True, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)

            Case ReporteMercancias.cServicios
                Dim vOrdenNombres() As String = {"Código Servicio"}
                Dim vOrdenCampos() As String = {"codser"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)

            Case ReporteMercancias.cListadoTransferencias
                Dim vOrdenNombres() As String = {"N° Transferencia"}
                Dim vOrdenCampos() As String = {"numtra"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)

            Case ReporteMercancias.cComprasDeMercancias
                Dim vOrdenNombres() As String = {"Código Mercancía"}
                Dim vOrdenCampos() As String = {"codart"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)
        End Select
    End Sub
    Private Sub Inicializar(ByVal nEtiqueta As String, ByVal TabOrden As Boolean, ByVal TabGrupo As Boolean,
        ByVal TabCriterio As Boolean, ByVal TabConstantes As Boolean, ByVal aNombreOrden() As String,
        ByVal aCampoOrden() As String, ByVal aTipoOrden() As String, ByVal aLongitudOrden() As Integer,
        Optional ByVal Trabajador As String = "")

        HabilitarTabs(TabOrden, TabGrupo, TabCriterio, TabConstantes)
        txtOrdenDesde.Text = Trabajador
        txtOrdenHasta.Text = Trabajador
        IniciarOrden(aNombreOrden, aCampoOrden, aTipoOrden, aLongitudOrden)
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
    Private Sub IniciarOrden(ByVal vNombres As Object, ByVal vCampos As Object, ByVal vTipo As Object, ByVal vLongitud As Object)

        vOrdenNombres = vNombres
        vOrdenCampos = vCampos
        vOrdenTipo = vTipo
        vOrdenLongitud = vLongitud

        IndiceReporte = 0
        strIndiceReporte = vCampos(IndiceReporte)
        ft.RellenaCombo(vNombres, cmbOrdenadoPor)
        LongitudMaximaOrden(CInt(vLongitud(IndiceReporte)))
        TipoOrden(CStr(vTipo(IndiceReporte)))

    End Sub
    Private Sub IniciarGrupos()

        ft.habilitarObjetos(False, False, TabPageVentas)
        Select Case ReporteNumero
            Case ReporteMercancias.cVentasDeMercancias, ReporteMercancias.cVentasDeMercanciasActivacion,
                ReporteMercancias.cVentasDeMercanciasActivacionXMes

                ft.habilitarObjetos(True, False, TabPageVentas)
            Case ReporteMercancias.cVentasPlana
                vMERAgrupadoPor = {"Jerarquía"}
                aMERAgrupadoPor = {"tipjer"}

        End Select

        ft.RellenaCombo(vMERAgrupadoPor, cmbMERAgrupadoPor)
        ft.RellenaCombo(vVENAgrupadoPor, cmbVenAgrupadoPor)

    End Sub

    Private Sub IniciarCriterios()

        VerCriterio_Periodo(False, 0)
        VerCriterio_Año(False)
        VerCriterio_TipoDocumento(False)
        VerCriterio_Almacen(False)
        VerCriterio_Documento(False)
        VerCriterio_Asesor(False)
        VerCriterio_Lote(False)
        VerCriterio_ProveedorCliente(False)
        VerCriterio_Origen(False)
        VerCriterio_Causa(False)

        Select Case ReporteNumero
            Case ReporteMercancias.cObsolescencia
                VerCriterio_Almacen(True)
                VerCriterio_Asesor(True)
            Case ReporteMercancias.cLeyDeCostos
                VerCriterio_Periodo(True, 2, TipoPeriodo.iDiario)
                VerCriterio_Almacen(True)
            Case ReporteMercancias.cPreciosYEquivalencias, ReporteMercancias.cPreciosYEquivalenciasSinIVA
                VerCriterio_Almacen(True)
            Case ReporteMercancias.cMovimientosMercancia, ReporteMercancias.cMovimientosMercancias
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_TipoDocumento(True)
                VerCriterio_Almacen(True)
                VerCriterio_Documento(True)
                VerCriterio_Asesor(True)
                VerCriterio_Lote(True)
                VerCriterio_ProveedorCliente(True)
                VerCriterio_Origen(True)
            Case ReporteMercancias.cMovimientosServicios, ReporteMercancias.cMovimientosServiciosS
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_TipoDocumento(True)
                VerCriterio_Documento(True)
                VerCriterio_Asesor(True)
                VerCriterio_ProveedorCliente(True)
                VerCriterio_Origen(True)

            Case ReporteMercancias.cVentasDeMercancias, ReporteMercancias.cVentasDeMercanciasActivacion,
                ReporteMercancias.cVentasNetasMercancias

                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Almacen(True)
                'VerCriterio_Asesor(True)
                VerCriterio_ProveedorCliente(True)
            Case ReporteMercancias.cComprasdeMercanciasActivacion
                VerCriterio_Periodo(True, 0)
                VerCriterio_Almacen(True)
                VerCriterio_ProveedorCliente(True)
            Case ReporteMercancias.cVentasDeMercanciasActivacionXMes
                VerCriterio_Año(True)
                VerCriterio_Almacen(True)
                VerCriterio_ProveedorCliente(True)
            Case ReporteMercancias.cInventarioLegal, ReporteMercancias.cMovimientosResumen
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Almacen(True)
            Case ReporteMercancias.cListadoTransferencias
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Causa(True)
            Case ReporteMercancias.cComprasDeMercancias
                VerCriterio_ProveedorCliente(True)
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Almacen(True)
            Case ReporteMercancias.cVentasPlana
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
            Case Else
        End Select

    End Sub

    Private Sub VerCriterio_Periodo(ByVal Ver As Boolean, ByVal CompletoDesdeHasta As Integer, Optional ByVal Periodo As TipoPeriodo = TipoPeriodo.iMensual)
        'CompletoDesdeHasta 0 = Complete , 1 = Desde , 2 = Hasta 
        periodoTipo = Periodo
        ft.visualizarObjetos(False, lblPeriodoHasta, lblPeriodo, txtPeriodoDesde, txtPeriodoHasta)
        If Ver Then

            Select Case CompletoDesdeHasta
                Case 0
                    ft.visualizarObjetos(Ver, lblPeriodoHasta, lblPeriodo, txtPeriodoDesde, txtPeriodoHasta)
                Case 1
                    ft.visualizarObjetos(Ver, lblPeriodo, txtPeriodoDesde)
                Case 2
                    ft.visualizarObjetos(Ver, lblPeriodoHasta, txtPeriodoHasta)
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
    Private Sub VerCriterio_Asesor(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblAsesor, lblAsesorHasta, txtAsesorDesde, txtAsesorHasta, btnAsesorDesde, btnAsesorHasta)
        ft.habilitarObjetos(False, True, txtAsesorDesde, txtAsesorHasta)
    End Sub
    Private Sub VerCriterio_Causa(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblCausaDesde, lblCausaHasta, txtCausaDesde, txtCausaHasta, btnCausaDesde, btnCausaHasta)
        ft.habilitarObjetos(False, True, txtCausaDesde, txtCausaHasta)
    End Sub
    Private Sub VerCriterio_Origen(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblOrigenDesde, lblOrigenHasta, txtOrigenDesde, txtOrigenHasta)
        ft.habilitarObjetos(True, True, txtOrigenDesde, txtOrigenHasta)
    End Sub
    Private Sub VerCriterio_TipoDocumento(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblTipodocumento, chkList)
        Dim aTipoDocumento() As String = {"EN", "SA", "AE", "AS", "AC"}
        Dim aSel() As Boolean = {True, True, True, True, True}
        RellenaListaSeleccionable(chkList, aTipoDocumento, aSel)
        txtTipDoc.Text = ".EN.SA.AE.AS.AC."
    End Sub
    Private Sub VerCriterio_Año(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblAño, cmbAño)
    End Sub
    Private Sub VerCriterio_Almacen(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblAlmacenDesde, txtAlmacenDesde, btnAlmacenDesde, lblalmacenHasta, txtAlmacenHasta, btnAlmacenHasta)
        ft.habilitarObjetos(False, True, txtAlmacenDesde, txtAlmacenHasta)
    End Sub
    Private Sub VerCriterio_ProveedorCliente(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblProveedorDesde, txtProvCliDesde, btnProvCliDesde, lblProveedorHasta, txtProvCliHasta, btnProvCliHasta)
        ft.habilitarObjetos(False, True, txtProvCliDesde, txtProvCliHasta)
    End Sub
    Private Sub VerCriterio_Documento(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblDocumentoDesde, lblDocumentoHasta, txtDocumentoDesde, txtDocumentoHasta)
        ft.habilitarObjetos(True, True, txtDocumentoDesde, txtDocumentoHasta)
        txtDocumentoDesde.MaxLength = 15
        txtDocumentoHasta.MaxLength = 15
    End Sub
    Private Sub VerCriterio_Lote(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblLoteDesde, lblLoteHasta, txtLoteDesde, txtLoteHasta, btnLoteDesde, btnLoteHasta)
        ft.habilitarObjetos(False, True, txtLoteDesde, txtLoteHasta)
    End Sub

    '/////////////////////////// COSTANTES
    '//////////////////////////
    Private Sub IniciarConstantes()
        ValoresInicialesConstantes()

        verConstante_Resumen(False)
        VerConstante_Tarifas(False)
        VerConstante_Tarifa(False)
        VerConstante_peso(False)
        VerConstante_TipoMercancia(False)
        VerConstante_TipoEtiqueta(False)
        VerConstante_Cartera(False)
        VerConstante_Regulado(False)
        VerConstante_Estatus(False)
        verConstante_Existencias(False)
        verConstante_porGastos(False)

        Select Case ReporteNumero

            Case ReporteMercancias.cPreciosYEquivalencias, ReporteMercancias.cPreciosYEquivalenciasSinIVA
                VerConstante_peso(True)
                VerConstante_Tarifa(True)
                VerConstante_TipoMercancia(True)
                VerConstante_Regulado(True)
                VerConstante_Cartera(True)
                VerConstante_Estatus(True)
                verConstante_Existencias(True)
            Case ReporteMercancias.cPrecios, ReporteMercancias.cPreciosIVA
                VerConstante_peso(True)
                VerConstante_TipoMercancia(True)
                VerConstante_Regulado(True)
                VerConstante_Cartera(True)
                VerConstante_Estatus(True)
                VerConstante_Tarifas(True)
                verConstante_Existencias(True)
            Case ReporteMercancias.cObsolescencia, ReporteMercancias.cMovimientosMercancias
                VerConstante_TipoMercancia(True)
                VerConstante_Regulado(True)
                VerConstante_Estatus(True)
                VerConstante_Cartera(True)
            Case ReporteMercancias.cMovimientosServiciosS
                VerConstante_Estatus(True)
            Case ReporteMercancias.cVentasDeMercancias, ReporteMercancias.cVentasDeMercanciasActivacion,
                ReporteMercancias.cVentasNetasMercancias

                VerConstante_TipoMercancia(True)
                VerConstante_Regulado(True)
                VerConstante_Estatus(True)
                VerConstante_Cartera(True)
                verConstante_Resumen(True)

            Case ReporteMercancias.cComprasdeMercanciasActivacion
                verConstante_Resumen(True)
            Case ReporteMercancias.cVentasDeMercanciasActivacionXMes
                VerConstante_TipoMercancia(True)
                VerConstante_Regulado(True)
                VerConstante_Estatus(True)
                VerConstante_Cartera(True)

                lblConsResumen.Text = "Resumir mercancías"
                VerConstante_peso(True)
                lblPeso.Text = "Resumir clientes"

            Case ReporteMercancias.cLeyDeCostos
                verConstante_Existencias(True)
                VerConstante_Tarifa(True)
                VerConstante_Estatus(True)
                verConstante_porGastos(True)
            Case ReporteMercancias.cInventarioLegal
                verConstante_Existencias(True)
            Case ReporteMercancias.cCodigoBarras
                VerConstante_TipoEtiqueta(True)
                verConstante_Resumen(True)
            Case ReporteMercancias.cConteos
                VerConstante_Estatus(True)
                lblEstatus.Text = "Tipo Conteo"
                ft.RellenaCombo(aTipoConteo, cmbEstatus)
            Case ReporteMercancias.cListadoTransferencias
                verConstante_Resumen(True)
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
        ft.RellenaCombo(aTipo, cmbTipo, 7)
        ft.RellenaCombo(aEstatus, cmbEstatus)
        ft.RellenaCombo(aExistencias, cmbExistencias, 2)
        txtporGastos.Text = ft.FormatoNumero(12.5)

    End Sub
    Private Sub verConstante_porGastos(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblporGastos, txtporGastos)
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
    Private Sub VerConstante_Tarifa(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblTarifa, cmbTarifa)
    End Sub
    Private Sub VerConstante_peso(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblPeso, chkPeso)
    End Sub
    Private Sub VerConstante_Cartera(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblCartera, cmbCartera)
    End Sub
    Private Sub VerConstante_TipoMercancia(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblTipo, cmbTipo)
    End Sub
    Private Sub VerConstante_TipoEtiqueta(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblBarra, cmbTipoEtiqueta)
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
    Private Sub TipoOrden(ByVal cTipo As String)
        Select Case vOrdenTipo(IndiceReporte)
            Case "D"
                txtOrdenDesde.Text = ft.FormatoFecha(PrimerDiaMes(jytsistema.sFechadeTrabajo))
                txtOrdenHasta.Text = ft.FormatoFecha(UltimoDiaMes(jytsistema.sFechadeTrabajo))
                txtOrdenDesde.Enabled = False
                txtOrdenHasta.Enabled = False
            Case Else
                txtOrdenDesde.Text = CodigoMercancia
                txtOrdenHasta.Text = CodigoMercancia
        End Select

    End Sub

    Private Sub jsMerRepParametros_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsMerRepParametros_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        Dim dsMerca As New dsMercas
        Dim str As String = ""
        Dim nTabla As String = ""
        Dim oReporte As New CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim PresentaArbol As Boolean = False

        Select Case ReporteNumero
            Case ReporteMercancias.cCodigoBarraUbicacion
                nTabla = "dtBarras"
                oReporte = New rptMercanciaBarraEstante
                str = seleccionMERCAS_UbicacionAlmacen(myConn, CodigoMercancia.Substring(0, 5), CodigoMercancia.Substring(5))
            Case ReporteMercancias.cCodigoBarras
                nTabla = "dtBarras"
                Select Case cmbTipoEtiqueta.SelectedIndex
                    Case 0
                        oReporte = New rptMercanciaBarra
                    Case 1
                        oReporte = New rptMercanciaBarraPrecio
                    Case 2
                        oReporte = New rptMercanciaBarraPequeña
                    Case 3
                        oReporte = New rptMercanciaBarraHabladorNevera
                End Select
                str = SeleccionMERCodigoBarras(myConn, lblInfo, CodigoMercancia)

            Case ReporteMercancias.cFichaMercancia
                nTabla = "dtMercancias"
                oReporte = New rptMercanciaFicha
                str = SeleccionMERMercancias(CodigoMercancia, CodigoMercancia, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex)
            Case ReporteMercancias.cCategorias
                nTabla = "dtCategorias"
                oReporte = New rptGENTablaSimple
                str = SeleccionGENTablaSimple(FormatoTablaSimple(Modulo.iCategoriaMerca))
            Case ReporteMercancias.cMarcas
                nTabla = "dtCategorias"
                oReporte = New rptGENTablaSimple
                str = SeleccionGENTablaSimple(FormatoTablaSimple(Modulo.iMarcaMerca))
            Case ReporteMercancias.cTransferencia
                nTabla = "dtTransferencia"
                oReporte = New rptMercanciaTransferencia
                str = SeleccionMERTransferencia(CodigoMercancia)
            Case ReporteMercancias.cPedidosAlmacen
                nTabla = "dtTransferencia"
                oReporte = New rptMercanciaPedidosAlmacen
                str = SeleccionMERPedidosAlmacen(CodigoMercancia)
            Case ReporteMercancias.cConteos
                nTabla = "dtConteo"
                oReporte = New rptMercanciaConteo
                str = SeleccionMERConteo(CodigoMercancia, cmbEstatus.SelectedIndex)
            Case ReporteMercancias.cPreciosFuturos
                nTabla = "dtPreciosFuturos"
                oReporte = New rptMercanciaPreciosFuturos
                str = SeleccionMERPreciosFuturos()
            Case ReporteMercancias.cPreciosEspeciales
                nTabla = "dtPreciosEspeciales"
                oReporte = New rptMercanciaPreciosEspeciales
                str = SeleccionMERPreciosEspeciales(CodigoMercancia)
            Case ReporteMercancias.cJerarquias
                nTabla = "dtJerarquias"
                oReporte = New rptMercanciaJerarquias
                str = SeleccionMERJerarquia(CodigoMercancia)
            Case ReporteMercancias.cOfertas
                nTabla = "dtOfertas"
                oReporte = New rptMercanciaOferta
                str = SeleccionMEROfertas(CodigoMercancia)
            Case ReporteMercancias.cVentasPlana
                nTabla = "dtVentasPlana"
                oReporte = New rptMercanciaVentasPlana
                str = SeleccionMERVentasMercanciasPLANA(txtTipoJerarquia.Text, txtTipoJerarquia.Text, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text))

            Case ReporteMercancias.cMovimientosResumen
                nTabla = "dtMercanciasMovimientosR"
                oReporte = New rptMercanciaMovimientosResumen
                str = SeleccionMERMovimientosMercanciasResumen(txtOrdenDesde.Text, txtOrdenHasta.Text, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), txtAlmacenDesde.Text, txtAlmacenHasta.Text,
                                                               txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text, txtDivisionHasta.Text,
                                                               txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text)
            Case ReporteMercancias.cServicios
                PresentaArbol = True
                nTabla = "dtServicios"
                oReporte = New rptMercanciaServicios
                str = SeleccionMERServicios()


            Case ReporteMercancias.cPreciosYEquivalencias, ReporteMercancias.cPreciosYEquivalenciasSinIVA
                nTabla = "dtPreciosEquivalentes"
                Select Case cmbMERAgrupadoPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptMercanciaPreciosEquivalentes0G
                    Case 1, 2, 3, 4, 5 '1 Grupo
                        oReporte = New rptMercanciaPreciosEquivalentes1G
                    Case 6, 8, 9, 11, 12, 15 '2 grupos
                        oReporte = New rptMercanciaPreciosEquivalentes2G
                    Case 7, 10, 14 '3 grupos
                        oReporte = New rptMercanciaPreciosEquivalentes3G
                    Case 13 '4 grupos
                        oReporte = New rptMercanciaPreciosEquivalentes4G
                End Select

                str = IIf(ReporteNumero = ReporteMercancias.cPreciosYEquivalencias,
                        SeleccionMERMercanciasPreciosYEquivalencias(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                                                  txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text,
                                                                  txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text,
                                                                  txtDivisionHasta.Text, txtAlmacenDesde.Text, txtAlmacenHasta.Text, cmbEstatus.SelectedIndex, cmbCartera.SelectedIndex, chkPeso.Checked, cmbTipo.SelectedIndex, cmbRegulada.SelectedIndex, cmbTarifa.SelectedIndex,
                                                                  True, cmbExistencias.SelectedIndex),
                        SeleccionMERMercanciasPreciosYEquivalenciasSinIVA(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                                                  txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text,
                                                                  txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text,
                                                                  txtDivisionHasta.Text, txtAlmacenDesde.Text, txtAlmacenHasta.Text, cmbEstatus.SelectedIndex, cmbCartera.SelectedIndex, chkPeso.Checked, cmbTipo.SelectedIndex, cmbRegulada.SelectedIndex, cmbTarifa.SelectedIndex,
                                                                  True, cmbExistencias.SelectedIndex))



            Case ReporteMercancias.cPrecios, ReporteMercancias.cPreciosIVA
                nTabla = "dtMercancias"
                Select Case cmbMERAgrupadoPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptMercanciaPreciosSG
                    Case 1, 2, 3, 4, 5 '1 Grupo
                        oReporte = New rptMercanciaPrecios1G
                    Case 6, 8, 9, 11, 12, 15 '2 grupos
                        oReporte = New rptMercanciaPrecios2G
                    Case 7, 10, 14 '3 grupos
                        oReporte = New rptMercanciaPrecios3G
                    Case 13 '4 grupos
                        oReporte = New rptMercanciaPrecios4G
                End Select

                str = SeleccionMERMercancias(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                             txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text,
                                             txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, txtCategoriaDesde.Text,
                                             txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text,
                                             txtDivisionHasta.Text, , , cmbEstatus.SelectedIndex, cmbCartera.SelectedIndex,
                                             chkPeso.Checked, cmbTipo.SelectedIndex, cmbRegulada.SelectedIndex,
                                             chkPrecioA.Checked, chkPrecioB.Checked, chkPrecioC.Checked, chkPrecioD.Checked, chkPrecioE.Checked, chkPrecioF.Checked,
                                             IIf(ReporteNumero = ReporteMercancias.cPreciosIVA, True, False), cmbExistencias.SelectedIndex)
            Case ReporteMercancias.cObsolescencia
                nTabla = "dtMercancias"
                Select Case cmbMERAgrupadoPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptMercanciaObsoletaSG
                    Case 1, 2, 3, 4, 5 '1 Grupo
                        oReporte = New rptMercanciaObsoleta1G
                    Case 6, 8, 9, 11, 12, 15 '2 grupos
                        oReporte = New rptMercanciaObsoleta2G
                    Case 7, 10, 14 '3 grupos
                        oReporte = New rptMercanciaObsoleta3G
                    Case 13 '4 grupos
                        oReporte = New rptMercanciaObsoleta4G
                End Select

                str = SeleccionMERObsolescencias(myConn, lblInfo, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                             txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text,
                                             txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, txtCategoriaDesde.Text,
                                             txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text,
                                             txtDivisionHasta.Text, cmbEstatus.SelectedIndex, cmbCartera.SelectedIndex,
                                             cmbTipo.SelectedIndex, cmbRegulada.SelectedIndex, txtAsesorDesde.Text, txtAsesorHasta.Text)

            Case ReporteMercancias.cMovimientosMercancias
                nTabla = "dtMercanciasMovimientos"
                Select Case cmbMERAgrupadoPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptMercanciaMovimiento0G
                    Case 1, 2, 3, 4, 5 '1 Grupo
                        oReporte = New rptMercanciaMovimiento1G
                    Case 6, 8, 9, 11, 12, 15 '2 grupos
                        oReporte = New rptMercanciaMovimiento2G
                    Case 7, 10, 14 '3 grupos
                        oReporte = New rptMercanciaMovimiento3G
                    Case 13 '4 grupos
                        oReporte = New rptMercanciaMovimiento4G
                End Select

                str = SeleccionMERMovimientosMercancias(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                                           CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), txtTipDoc.Text, txtAlmacenDesde.Text,
                                                           txtAlmacenHasta.Text, txtProvCliDesde.Text, txtProvCliHasta.Text, txtAsesorDesde.Text, txtAsesorHasta.Text,
                                                           txtLoteDesde.Text, txtLoteHasta.Text, txtDocumentoDesde.Text, txtDocumentoHasta.Text,
                                                           txtOrigenDesde.Text, txtOrigenHasta.Text, txtCategoriaDesde.Text,
                                                           txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtTipoJerarquia.Text,
                                                           txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text,
                                                           txtDivisionDesde.Text, txtDivisionHasta.Text, cmbEstatus.SelectedIndex, cmbCartera.SelectedIndex, ,
                                                           cmbTipo.SelectedIndex, cmbRegulada.SelectedIndex)

            Case ReporteMercancias.cMovimientosMercancia
                nTabla = "dtMercanciasMovimientos"
                oReporte = New rptMercanciaMovimientoSG

                str = SeleccionMERMovimientosMercancias(txtOrdenDesde.Text, txtOrdenHasta.Text, "codart", cmbOrdenDesde.SelectedIndex,
                                                           CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), txtTipDoc.Text,
                                                           txtAlmacenDesde.Text, txtAlmacenHasta.Text, txtProvCliDesde.Text, txtProvCliHasta.Text,
                                                           txtAsesorDesde.Text, txtAsesorHasta.Text, txtLoteDesde.Text, txtLoteHasta.Text,
                                                           txtDocumentoDesde.Text, txtDocumentoHasta.Text, txtOrigenDesde.Text, txtOrigenHasta.Text)

            Case ReporteMercancias.cMovimientosServicios, ReporteMercancias.cMovimientosServiciosS

                nTabla = "dtMercanciasMovimientos"
                oReporte = New rptMercanciaServiciosMovimientos0G
                str = SeleccionMERMovimientosServicios(txtOrdenDesde.Text, txtOrdenHasta.Text, "codart", cmbOrdenDesde.SelectedIndex,
                                                       CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), txtTipDoc.Text,
                                                         txtAlmacenDesde.Text, txtAlmacenHasta.Text, txtProvCliDesde.Text, txtProvCliHasta.Text,
                                                           txtAsesorDesde.Text, txtAsesorHasta.Text,
                                                           txtDocumentoDesde.Text, txtDocumentoHasta.Text, txtOrigenDesde.Text, txtOrigenHasta.Text)
            Case ReporteMercancias.cLeyDeCostos
                nTabla = "dtLeyDeCostos"
                Select Case cmbMERAgrupadoPor.SelectedIndex
                    Case 0 ' 0 grupos
                        oReporte = New rptMercanciaLeyDeCostos

                    Case Else
                        oReporte = New rptMercanciaLeyDeCostos
                End Select

                str = SeleccionMERExistenciasAFecha(myConn, lblInfo, txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                            txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text,
                                            txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, txtCategoriaDesde.Text,
                                            txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text,
                                            txtDivisionHasta.Text, txtAlmacenDesde.Text, txtAlmacenHasta.Text, CDate(txtPeriodoHasta.Text),
                                            cmbEstatus.SelectedIndex, cmbCartera.SelectedIndex, chkPeso.Checked, cmbTipo.SelectedIndex,
                                            cmbRegulada.SelectedIndex, cmbExistencias.SelectedIndex, ValorNumero(txtporGastos.Text), cmbTarifa.Text)

            Case ReporteMercancias.cMovimientosMercancia
                nTabla = "dtMercanciasMovimientos"
                oReporte = New rptMercanciaMovimientoSG

                str = SeleccionMERMovimientosMercancias(txtOrdenDesde.Text, txtOrdenHasta.Text, "codart", cmbOrdenDesde.SelectedIndex,
                                                           CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), txtTipDoc.Text,
                                                           txtAlmacenDesde.Text, txtAlmacenHasta.Text, txtProvCliDesde.Text, txtProvCliHasta.Text,
                                                           txtAsesorDesde.Text, txtAsesorHasta.Text, txtLoteDesde.Text, txtLoteHasta.Text,
                                                           txtDocumentoDesde.Text, txtDocumentoHasta.Text, txtOrigenDesde.Text, txtOrigenHasta.Text)


            Case ReporteMercancias.cVentasNetasMercancias

                PresentaArbol = True
                nTabla = "dtVentasMer"
                oReporte = New rptMercanciaVentasNetasMerG0

                str = SeleccionMERVentasMercancias(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                             txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text,
                                            txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, txtCategoriaDesde.Text,
                                            txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text,
                                            txtDivisionHasta.Text, txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text,
                                            txtTipoNegocioHasta.Text, txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                            txtAsesorRDesde.Text, txtAsesorRHasta.Text, txtPais.Text, txtEstado.Text, txtMunicipio.Text, txtParroquia.Text,
                                            txtCiudad.Text, txtBarrio.Text, txtAlmacenDesde.Text, txtAlmacenHasta.Text, CDate(txtPeriodoDesde.Text),
                                            CDate(txtPeriodoHasta.Text), txtProvCliDesde.Text, txtProvCliHasta.Text,
                                            cmbEstatus.SelectedIndex, cmbCartera.SelectedIndex, chkPeso.Checked, cmbTipo.SelectedIndex,
                                            cmbRegulada.SelectedIndex)

            Case ReporteMercancias.cVentasDeMercancias

                PresentaArbol = True
                nTabla = "dtVentasMer"
                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbVenAgrupadoPor.SelectedIndex.ToString)
                    Case 0  ' 0 grupos
                        oReporte = New rptMercanciaVentasMerG0
                    Case 1  ' 1 grupo
                        oReporte = New rptMercanciaVentasMerG1
                    Case 2  '2 grupos
                        oReporte = New rptMercanciaVentasMerG2
                    Case 3 '3 grupos
                        oReporte = New rptMercanciaVentasMerG3
                    Case Else
                        oReporte = New rptMercanciaVentasMerG0
                End Select

                str = SeleccionMERVentasMercancias(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                             txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text,
                                            txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, txtCategoriaDesde.Text,
                                            txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text,
                                            txtDivisionHasta.Text, txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text,
                                            txtTipoNegocioHasta.Text, txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text,
                                            txtAsesorRDesde.Text, txtAsesorRHasta.Text, txtPais.Text, txtEstado.Text, txtMunicipio.Text, txtParroquia.Text,
                                            txtCiudad.Text, txtBarrio.Text, txtAlmacenDesde.Text, txtAlmacenHasta.Text, CDate(txtPeriodoDesde.Text),
                                            CDate(txtPeriodoHasta.Text), txtProvCliDesde.Text, txtProvCliHasta.Text,
                                            cmbEstatus.SelectedIndex, cmbCartera.SelectedIndex, chkPeso.Checked, cmbTipo.SelectedIndex,
                                            cmbRegulada.SelectedIndex)

            Case ReporteMercancias.cVentasDeMercanciasActivacion
                PresentaArbol = True
                nTabla = "dtVentasMerAct"
                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbVenAgrupadoPor.SelectedIndex.ToString)
                    Case 0  ' 0 grupos
                        oReporte = New rptMercanciaVentasMerActG0
                    Case 1  ' 1 grupo
                        oReporte = New rptMercanciaVentasMerActG1
                    Case 2  '2 grupos
                        oReporte = New rptMercanciaVentasMerActG2
                    Case 3 '3 grupos
                        oReporte = New rptMercanciaVentasMerActG3
                    Case 4 '4 grupos
                        oReporte = New rptMercanciaVentasMerActG4
                    Case Else
                        oReporte = New rptMercanciaVentasMerActG0
                End Select

                str = SeleccionMERVentasMercanciasAct(myConn, lblInfo, txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex),
                                            txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, txtCategoriaDesde.Text,
                                            txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text, txtDivisionHasta.Text,
                                            cmbEstatus.SelectedIndex, cmbCartera.SelectedIndex, chkPeso.Checked, cmbTipo.SelectedIndex, cmbRegulada.SelectedIndex,
                                            txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text, txtZonaDesde.Text,
                                            txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text, txtAsesorRDesde.Text, txtAsesorRHasta.Text,
                                            txtPais.Text, txtEstado.Text, txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text,
                                            CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), txtAlmacenDesde.Text, txtAlmacenHasta.Text,
                                            txtProvCliDesde.Text, txtProvCliHasta.Text, chkConsResumen.Checked, False)

            Case ReporteMercancias.cComprasdeMercanciasActivacion
                PresentaArbol = True
                nTabla = "dtVentasMerAct"
                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbVenAgrupadoPor.SelectedIndex.ToString)
                    Case 0  ' 0 grupos
                        oReporte = New rptMercanciaVentasMerActG0
                    Case 1  ' 1 grupo
                        oReporte = New rptMercanciaVentasMerActG1
                    Case 2  '2 grupos
                        oReporte = New rptMercanciaVentasMerActG2
                    Case 3 '3 grupos
                        oReporte = New rptMercanciaVentasMerActG3
                    Case 4 '4 grupos
                        oReporte = New rptMercanciaVentasMerActG4
                    Case Else
                        oReporte = New rptMercanciaVentasMerActG0
                End Select

                str = SeleccionMERComprasMercanciasAct(myConn, lblInfo, txtOrdenDesde.Text, txtOrdenHasta.Text,
                                            txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text, txtDivisionHasta.Text,
                                            "", "", "", "", CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), txtAlmacenDesde.Text, txtAlmacenHasta.Text,
                                            txtProvCliDesde.Text, txtProvCliHasta.Text, txtTipoJerarquia.Text, txtCodjer1.Text,
                                            txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, False)

            Case ReporteMercancias.cVentasDeMercanciasActivacionXMes
                PresentaArbol = True
                nTabla = "dtVentasMerAct"
                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbVenAgrupadoPor.SelectedIndex.ToString)
                    Case 0  ' 0 grupos
                        oReporte = New rptMercanciaVentasPorItemActMesMes0G
                    Case 1  ' 1 grupos
                        oReporte = New rptMercanciaVentasPorItemActMesMes1G
                    Case 2  ' 2 grupos
                        oReporte = New rptMercanciaVentasPorItemActMesMes2G
                    Case Else  ' 0 grupos
                        oReporte = New rptMercanciaVentasPorItemActMesMes0G
                End Select

                str = SeleccionMERVentasMercanciasAct(myConn, lblInfo, txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex),
                                            txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, txtCategoriaDesde.Text,
                                            txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text, txtDivisionHasta.Text,
                                            cmbEstatus.SelectedIndex, cmbCartera.SelectedIndex, chkPeso.Checked, cmbTipo.SelectedIndex, cmbRegulada.SelectedIndex,
                                            txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text, txtZonaDesde.Text,
                                            txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text, txtAsesorRDesde.Text, txtAsesorRHasta.Text,
                                            txtPais.Text, txtEstado.Text, txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text,
                                            CDate("01/01/" & CStr(cmbAño.SelectedIndex + 2000)), CDate("31/12/" & CStr(cmbAño.SelectedIndex + 2000)), txtAlmacenDesde.Text, txtAlmacenHasta.Text,
                                            txtProvCliDesde.Text, txtProvCliHasta.Text, chkConsResumen.Checked, True)

            Case ReporteMercancias.cComprasDeMercancias
                PresentaArbol = True
                nTabla = "dtComprasMer"
                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbVenAgrupadoPor.SelectedIndex.ToString)
                    Case 0  ' 0 grupos
                        oReporte = New rptMercanciaComprasMer0G
                    Case 1  ' 1 grupos
                        oReporte = New rptMercanciaComprasMer1G
                    Case 2  ' 2 grupos
                        oReporte = New rptMercanciaComprasMer2G
                    Case 3
                        oReporte = New rptMercanciaComprasMer3G
                    Case Else  ' 0 grupos
                        oReporte = New rptMercanciaComprasMer0G
                End Select

                str = SeleccionMERComprasMercancias(txtOrdenDesde.Text, txtOrdenHasta.Text, txtCategoriaDesde.Text, txtCategoriaHasta.Text,
                                            txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text, txtDivisionHasta.Text, "", "", "", "", CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), txtAlmacenDesde.Text, txtAlmacenHasta.Text, txtProvCliDesde.Text, txtProvCliHasta.Text, txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text)

            Case ReporteMercancias.cInventarioLegal
                nTabla = "dtInventarioLegal"
                oReporte = New rptMercanciaIventarioLegalSG
                str = SeleccionMERInventarioLegalPlus(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex,
                                                  CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text),
                                                  txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text,
                                                  txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, txtCategoriaDesde.Text,
                                                    txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text,
                                                    txtDivisionHasta.Text, txtAlmacenDesde.Text, txtAlmacenHasta.Text,
                                                    cmbEstatus.SelectedIndex, cmbCartera.SelectedIndex, chkPeso.Checked, cmbTipo.SelectedIndex,
                                                    cmbRegulada.SelectedIndex)
            Case ReporteMercancias.cListadoTransferencias
                nTabla = "dtTransferencias"
                oReporte = New rptMercanciaTransferencias0G
                str = SeleccionMERListadoTransferencias(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text),
                                                        txtCausaDesde.Text, txtCausaHasta.Text)
            Case Else
                oReporte = Nothing
        End Select

        If nTabla <> "" Then
            dsMerca = DataSetRequery(dsMerca, str, myConn, nTabla, lblInfo)
            If dsMerca.Tables(nTabla).Rows.Count > 0 Then
                oReporte = PresentaReporte(oReporte, dsMerca, nTabla)
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
        r.Dispose()
        r = Nothing
        oReporte.Close()
        oReporte.Dispose()
        oReporte = Nothing

    End Sub
    Private Sub colocarParametrosEnReportes(ByVal oReporte As CrystalDecisions.CrystalReports.Engine.ReportClass,
                                          nomIndex As String, valor As Object, reportActual As Integer,
                                          ParamArray aReportes() As Integer)

        If Array.IndexOf(aReportes, reportActual) >= 0 Then
            oReporte.SetParameterValue(nomIndex, valor)
        End If


    End Sub
    Private Function PresentaReporte(ByVal oReporte As CrystalDecisions.CrystalReports.Engine.ReportClass,
        ByVal dsReport As DataSet, ByVal nTabla As String) As CrystalDecisions.CrystalReports.Engine.ReportClass

        Dim rif As String
        Dim nCampos() As String = {"id_emp"}
        Dim nString() As String = {jytsistema.WorkID}
        Dim CaminoImagen As String = ""
        Dim dtEmpresa As DataTable
        Dim nTablaEmpresa As String = "tblEmpresa"
        Dim strSQLEmpresa As String = " select id_emp, rif, logo from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' "

        dsReport = DataSetRequery(dsReport, strSQLEmpresa, myConn, nTablaEmpresa, lblInfo)
        dtEmpresa = dsReport.Tables(nTablaEmpresa)

        With dtEmpresa.Rows(0)
            rif = .Item("RIF").ToString
            CaminoImagen = BaseDatosAImagen(dtEmpresa.Rows(0), "logo", .Item("id_emp").ToString)
        End With
        dtEmpresa.Dispose()
        dtEmpresa = Nothing

        oReporte.SetDataSource(dsReport)
        oReporte.Refresh()
        oReporte.SetParameterValue("strLogo", CaminoImagen)
        oReporte.SetParameterValue("Orden", "Ordenado por : " + cmbOrdenadoPor.Text + " " + txtOrdenDesde.Text + "/" + txtOrdenHasta.Text)
        oReporte.SetParameterValue("RIF", "")
        oReporte.SetParameterValue("Grupo", LineaGrupos)
        oReporte.SetParameterValue("Criterios", LineaCriterios)
        oReporte.SetParameterValue("Constantes", LineaConstantes)
        oReporte.SetParameterValue("Empresa", jytsistema.WorkName.TrimEnd(" ") + "      R.I.F. : " & rif)

        colocarParametrosEnReportes(oReporte, "Titulo", ReporteNombre, ReporteNumero,
                                    ReporteMercancias.cTransferencia, ReporteMercancias.cPrecios,
                                    ReporteMercancias.cPreciosIVA, ReporteMercancias.cCategorias,
                                    ReporteMercancias.cMarcas, ReporteMercancias.cPreciosYEquivalencias,
                                    ReporteMercancias.cPreciosYEquivalenciasSinIVA,
                                    ReporteMercancias.cVentasDeMercanciasActivacion, ReporteMercancias.cComprasdeMercanciasActivacion)

        colocarParametrosEnReportes(oReporte, "numConteo", cmbEstatus.Text, ReporteNumero,
                                    ReporteMercancias.cConteos)
        colocarParametrosEnReportes(oReporte, "RIF", rif, ReporteNumero,
                                    ReporteMercancias.cCodigoBarras, ReporteMercancias.cCodigoBarraUbicacion)
        colocarParametrosEnReportes(oReporte, "Empresa", jytsistema.WorkName.TrimEnd(" "), ReporteNumero,
                                    ReporteMercancias.cCodigoBarras, ReporteMercancias.cCodigoBarraUbicacion)
        colocarParametrosEnReportes(oReporte, "resumido", chkConsResumen.Checked, ReporteNumero,
                                    ReporteMercancias.cCodigoBarras, ReporteMercancias.cCodigoBarraUbicacion)
        colocarParametrosEnReportes(oReporte, "Resumen", chkConsResumen.Checked, ReporteNumero,
                                    ReporteMercancias.cVentasDeMercancias, ReporteMercancias.cVentasDeMercanciasActivacion,
                                    ReporteMercancias.cVentasNetasMercancias, ReporteMercancias.cListadoTransferencias,
                                    ReporteMercancias.cVentasDeMercanciasActivacionXMes, ReporteMercancias.cComprasdeMercanciasActivacion)
        colocarParametrosEnReportes(oReporte, "ResumenR", chkConsResumen.Checked, ReporteNumero,
                                    ReporteMercancias.cVentasDeMercanciasActivacionXMes)

        If ReporteNumero = ReporteMercancias.cVentasDeMercanciasActivacion Then
            oReporte.SetParameterValue("TituloVentasBrutas", "Ventas Brutas")
            oReporte.SetParameterValue("TituloVentasNetas", "Ventas Netas")
            oReporte.SetParameterValue("TituloVentas", "Ventas")
        End If

        If ReporteNumero = ReporteMercancias.cComprasdeMercanciasActivacion Then
            oReporte.SetParameterValue("TituloVentasBrutas", "Compras Brutas")
            oReporte.SetParameterValue("TituloVentasNetas", "Compras Netas")
            oReporte.SetParameterValue("TituloVentas", "")
        End If

        Select Case ReporteNumero
            Case ReporteMercancias.cCatalogo, ReporteMercancias.cPrecios, ReporteMercancias.cPreciosIVA,
                ReporteMercancias.cObsolescencia,
                ReporteMercancias.cPreciosYEquivalencias, ReporteMercancias.cPreciosYEquivalenciasSinIVA,
                ReporteMercancias.cVentasDeMercancias, ReporteMercancias.cVentasDeMercanciasActivacion,
                ReporteMercancias.cComprasDeMercancias, ReporteMercancias.cVentasNetasMercancias, ReporteMercancias.cVentasDeMercanciasActivacionXMes

                Select Case cmbMERAgrupadoPor.SelectedIndex.ToString & cmbVenAgrupadoPor.SelectedIndex.ToString
                    Case "10", "20", "30", "40", "50"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        Dim FieldDef1 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                    Case "01", "02", "03", "04", "05", "014"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        Dim FieldDef1 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                    Case "60", "80", "90", "110", "120", "150"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        Dim aFld() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(0)))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(1)))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                    Case "06", "07", "08", "09", "011", "012"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        Dim aFld() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(0)))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(1)))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                    Case "11", "12", "13", "14", "15", "114", "21", "22", "23", "24", "25", "214",
                        "31", "32", "33", "34", "35", "314", "41", "42", "43", "44", "45", "414",
                        "51", "52", "53", "54", "55", "514"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        Dim FieldDef1, FieldDef2 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2

                    Case "70", "100", "140"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        oReporte.SetParameterValue("Grupo3", "tipjer")
                        Dim aFld() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2, FieldDef3 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(0)))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(1)))
                        FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(2)))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                    Case "010", "013"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        oReporte.SetParameterValue("Grupo3", "tipjer")
                        Dim aFld() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2, FieldDef3 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(0)))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(1)))
                        FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(2)))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                    Case "61", "81", "91", "111", "121", "62", "82", "92", "112", "122",
                         "63", "83", "93", "113", "123", "64", "84", "94", "114", "124",
                         "65", "85", "95", "115", "125", "614", "814", "914", "1114", "1214"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        oReporte.SetParameterValue("Grupo3", "tipjer")
                        Dim aFld() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2, FieldDef3 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(0)))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(1)))
                        FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                    Case "16", "17", "18", "19", "111", "112", "26", "27", "28", "29", "211", "212",
                        "36", "37", "38", "39", "311", "312", "46", "47", "48", "49", "411", "412",
                        "56", "57", "58", "59", "511", "512"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        oReporte.SetParameterValue("Grupo3", "tipjer")
                        Dim aFld() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2, FieldDef3 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(0)))
                        FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(1)))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                    Case "130"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        oReporte.SetParameterValue("Grupo3", "tipjer")
                        oReporte.SetParameterValue("Grupo4", "division")
                        Dim aFld() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2, FieldDef3, FieldDef4 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(0)))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(1)))
                        FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(2)))
                        FieldDef4 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(3)))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                        oReporte.DataDefinition.Groups.Item(3).ConditionField = FieldDef4
                    Case "66", "67", "68", "69", "611", "612", "86", "87", "88", "89", "811", "812",
                            "96", "97", "98", "99", "911", "912", "116", "117", "118", "119", "1111", "1112",
                            "126", "127", "128", "129", "1211", "1212"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        oReporte.SetParameterValue("Grupo3", "tipjer")
                        oReporte.SetParameterValue("Grupo4", "division")
                        Dim aFld1() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                        Dim aFld2() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2, FieldDef3, FieldDef4 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(0)))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(1)))
                        FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld2(0)))
                        FieldDef4 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld2(1)))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                        oReporte.DataDefinition.Groups.Item(3).ConditionField = FieldDef4
                    Case "71", "72", "73", "74", "75", "714", "101", "102", "103", "104", "105", "1014", "141", "142", "143", "144", "145", "1414"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        oReporte.SetParameterValue("Grupo3", "tipjer")
                        oReporte.SetParameterValue("Grupo4", "division")
                        Dim aFld1() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2, FieldDef3, FieldDef4 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(0)))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(1)))
                        FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(2)))
                        FieldDef4 = oReporte.Database.Tables.Item(0).Fields.Item(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                        oReporte.DataDefinition.Groups.Item(3).ConditionField = FieldDef4
                    Case "110", "210", "310", "410", "510", "113", "213", "313", "413", "513"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        oReporte.SetParameterValue("Grupo3", "tipjer")
                        oReporte.SetParameterValue("Grupo4", "division")
                        Dim aFld1() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2, FieldDef3, FieldDef4 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(0)))
                        FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(1)))
                        FieldDef4 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(2)))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                        oReporte.DataDefinition.Groups.Item(3).ConditionField = FieldDef4
                    Case "131", "132", "133", "134", "135", "1314" '5grupos 4X1
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        oReporte.SetParameterValue("Grupo3", "tipjer")
                        oReporte.SetParameterValue("Grupo4", "division")
                        oReporte.SetParameterValue("Grupo5", "mix")
                        Dim aFld1() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2, FieldDef3, FieldDef4, FieldDef5 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(0)))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(1)))
                        FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(2)))
                        FieldDef4 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(3)))
                        FieldDef5 = oReporte.Database.Tables.Item(0).Fields.Item(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                        oReporte.DataDefinition.Groups.Item(3).ConditionField = FieldDef4
                        oReporte.DataDefinition.Groups.Item(4).ConditionField = FieldDef5
                    Case "106", "107", "108", "109", "1011", "1012", "146", "147", "148", "149", "1411", "1412" '5Grupos 3X2
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        oReporte.SetParameterValue("Grupo3", "tipjer")
                        oReporte.SetParameterValue("Grupo4", "division")
                        oReporte.SetParameterValue("Grupo5", "mix")
                        Dim aFld1() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                        Dim aFld2() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2, FieldDef3, FieldDef4, FieldDef5 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(0)))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(1)))
                        FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(2)))
                        FieldDef4 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld2(0)))
                        FieldDef5 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld2(1)))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                        oReporte.DataDefinition.Groups.Item(3).ConditionField = FieldDef4
                        oReporte.DataDefinition.Groups.Item(4).ConditionField = FieldDef5
                    Case "610", "810", "910", "1110", "1210", "613", "813", "913", "1113", "1213" '5grupos 2X3
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        oReporte.SetParameterValue("Grupo3", "tipjer")
                        oReporte.SetParameterValue("Grupo4", "division")
                        oReporte.SetParameterValue("Grupo5", "mix")
                        Dim aFld1() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                        Dim aFld2() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2, FieldDef3, FieldDef4, FieldDef5 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(0)))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(1)))
                        FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld2(0)))
                        FieldDef4 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld2(1)))
                        FieldDef5 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld2(2)))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                        oReporte.DataDefinition.Groups.Item(3).ConditionField = FieldDef4
                        oReporte.DataDefinition.Groups.Item(4).ConditionField = FieldDef5
                    Case "136", "137", "138", "139", "1311", "1312" '6Grupos 4X2
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        oReporte.SetParameterValue("Grupo3", "tipjer")
                        oReporte.SetParameterValue("Grupo4", "division")
                        oReporte.SetParameterValue("Grupo5", "mix")
                        oReporte.SetParameterValue("Grupo6", "canal")
                        Dim aFld1() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                        Dim aFld2() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2, FieldDef3, FieldDef4, FieldDef5, FieldDef6 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(0)))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(1)))
                        FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(2)))
                        FieldDef4 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(3)))
                        FieldDef5 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld2(0)))
                        FieldDef6 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld2(1)))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                        oReporte.DataDefinition.Groups.Item(3).ConditionField = FieldDef4
                        oReporte.DataDefinition.Groups.Item(4).ConditionField = FieldDef5
                        oReporte.DataDefinition.Groups.Item(5).ConditionField = FieldDef6
                    Case "710", "713", "1010", "1013", "1410", "1413" '6Grupos 3X3
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        oReporte.SetParameterValue("Grupo3", "tipjer")
                        oReporte.SetParameterValue("Grupo4", "division")
                        oReporte.SetParameterValue("Grupo5", "mix")
                        oReporte.SetParameterValue("Grupo6", "canal")
                        Dim aFld1() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                        Dim aFld2() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2, FieldDef3, FieldDef4, FieldDef5, FieldDef6 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(0)))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(1)))
                        FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(2)))
                        FieldDef4 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld2(0)))
                        FieldDef5 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld2(1)))
                        FieldDef6 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld2(2)))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                        oReporte.DataDefinition.Groups.Item(3).ConditionField = FieldDef4
                        oReporte.DataDefinition.Groups.Item(4).ConditionField = FieldDef5
                        oReporte.DataDefinition.Groups.Item(5).ConditionField = FieldDef6
                    Case "1310", "1313" '7Grupos 4X3
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        oReporte.SetParameterValue("Grupo3", "tipjer")
                        oReporte.SetParameterValue("Grupo4", "division")
                        oReporte.SetParameterValue("Grupo5", "mix")
                        oReporte.SetParameterValue("Grupo6", "canal")
                        oReporte.SetParameterValue("Grupo7", "tiponegocio")
                        Dim aFld1() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                        Dim aFld2() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                        Dim FieldDef1, FieldDef2, FieldDef3, FieldDef4, FieldDef5, FieldDef6, FieldDef7 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(0)))
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(1)))
                        FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(2)))
                        FieldDef4 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld1(3)))
                        FieldDef5 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld2(0)))
                        FieldDef6 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld2(1)))
                        FieldDef7 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld2(2)))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                        oReporte.DataDefinition.Groups.Item(3).ConditionField = FieldDef4
                        oReporte.DataDefinition.Groups.Item(4).ConditionField = FieldDef5
                        oReporte.DataDefinition.Groups.Item(5).ConditionField = FieldDef6
                        oReporte.DataDefinition.Groups.Item(6).ConditionField = FieldDef7
                    Case Else
                End Select

                If ReporteNumero = ReporteMercancias.cComprasDeMercancias Or
                    ReporteNumero = ReporteMercancias.cComprasdeMercanciasActivacion Then
                    Dim oFormula As CrystalDecisions.CrystalReports.Engine.FormulaFieldDefinition
                    For Each oFormula In oReporte.DataDefinition.FormulaFields
                        Select Case oFormula.Name
                            Case "CostoPromG1"
                                oFormula.Text = " if Sum ({dtComprasMer.PesoTotal}, {dtComprasMer." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "}) <> 0 then " _
                                    & " Sum ({dtComprasMer.CosTotal}, {dtComprasMer." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "})/Sum ({dtComprasMer.PesoTotal}, {dtComprasMer." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "}) else  0 "
                            Case "CostoPromG2"
                                oFormula.Text = " if Sum ({dtComprasMer.PesoTotal}, {dtComprasMer." & oReporte.DataDefinition.Groups.Item(1).ConditionField.Name.ToString & "}) <> 0 then " _
                                    & " Sum ({dtComprasMer.CosTotal}, {dtComprasMer." & oReporte.DataDefinition.Groups.Item(1).ConditionField.Name.ToString & "})/Sum ({dtComprasMer.PesoTotal}, {dtComprasMer." & oReporte.DataDefinition.Groups.Item(1).ConditionField.Name.ToString & "}) else  0 "
                            Case "PorcentajeVentasG1"
                                oFormula.Text = " IF Sum ({dtComprasMer.costotal}) <> 0 then Sum ({dtComprasMer.costotal}, {dtComprasMer." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "})/Sum ({dtComprasMer.costotal})*100 else 0 "
                            Case "PorcentajeVentasG2"
                                oFormula.Text = "if Sum ({dtComprasMer.costotal}, {dtComprasMer." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "}) <> 0 then Sum ({dtComprasMer.costotal}, {dtComprasMer." & oReporte.DataDefinition.Groups.Item(1).ConditionField.Name.ToString & "})/Sum ({dtComprasMer.costotal}, {dtComprasMer." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "}) *100 else 0"
                        End Select
                    Next
                Else
                    Dim oFormula As CrystalDecisions.CrystalReports.Engine.FormulaFieldDefinition
                    For Each oFormula In oReporte.DataDefinition.FormulaFields
                        Select Case oFormula.Name
                            Case "CostoPromG1"
                                oFormula.Text = " if Sum ({dtVentasMer.PesoTotal}, {dtVentasMer." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "}) <> 0 then " _
                                    & " Sum ({dtVentasMer.CosTotal}, {dtVentasMer." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "})/Sum ({dtVentasMer.PesoTotal}, {dtVentasMer." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "}) else  0 "
                            Case "CostoPromG2"
                                oFormula.Text = " if Sum ({dtVentasMer.PesoTotal}, {dtVentasMer." & oReporte.DataDefinition.Groups.Item(1).ConditionField.Name.ToString & "}) <> 0 then " _
                                    & " Sum ({dtVentasMer.CosTotal}, {dtVentasMer." & oReporte.DataDefinition.Groups.Item(1).ConditionField.Name.ToString & "})/Sum ({dtVentasMer.PesoTotal}, {dtVentasMer." & oReporte.DataDefinition.Groups.Item(1).ConditionField.Name.ToString & "}) else  0 "
                            Case "PorcentajeVentasG1"
                                oFormula.Text = " IF Sum ({dtVentasMer.VenTotal}) <> 0 then Sum ({dtVentasMer.VenTotal}, {dtVentasMer." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "})/Sum ({dtVentasMer.VenTotal})*100 else 0 "
                            Case "PorcentajeVentasG2"
                                oFormula.Text = "if Sum ({dtVentasMer.VenTotal}, {dtVentasMer." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "}) <> 0 then Sum ({dtVentasMer.VenTotal}, {dtVentasMer." & oReporte.DataDefinition.Groups.Item(1).ConditionField.Name.ToString & "})/Sum ({dtVentasMer.VenTotal}, {dtVentasMer." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "}) *100 else 0"
                        End Select
                    Next
                End If

        End Select

        PresentaReporte = oReporte

    End Function
    Private Function GruposCantidad(ByVal CadenaGrupos As String) As Integer
        Select Case CadenaGrupos
            Case "00" ' 0 grupos
                GruposCantidad = 0
            Case "10", "20", "30", "40", "50", "01", "02", "03", "04", "05", "014"  ' 1 grupo
                GruposCantidad = 1
            Case "60", "80", "90", "110", "120", "150", "06", "07", "08", "09", "011", "012",
                "11", "12", "13", "14", "15", "114", "21", "22", "23", "24", "25", "214",
                "31", "32", "33", "34", "35", "314", "41", "42", "43", "44", "45", "414",
                "51", "52", "53", "54", "55", "514" '2 grupos
                GruposCantidad = 2
            Case "70", "100", "140", "61", "81", "91", "111", "121", "62", "82", "92", "112", "122",
                "63", "83", "93", "113", "123", "64", "84", "94", "114", "124",
                "65", "85", "95", "115", "125", "614", "814", "914", "1114", "1214",
                "16", "17", "18", "19", "111", "112", "26", "27", "28", "29", "211", "212",
                "36", "37", "38", "39", "311", "312", "46", "47", "48", "49", "411", "412",
                "56", "57", "58", "59", "511", "512", "010", "013" '3 grupos
                GruposCantidad = 3
            Case "130", "66", "67", "68", "69", "611", "612", "86", "87", "88", "89", "811", "812",
                        "96", "97", "98", "99", "911", "912", "116", "117", "118", "119", "1111", "1112",
                        "126", "127", "128", "129", "1211", "1212",
                        "71", "72", "73", "74", "75", "714",
                        "101", "102", "103", "104", "105", "1014", "141", "142", "143", "144", "145", "1414",
                        "110", "210", "310", "410", "510", "113", "213", "313", "413", "513" '4 grupos
                GruposCantidad = 4
            Case "131", "132", "133", "134", "135", "1314", "76", "77", "78", "79", "711", "712",
                "106", "107", "108", "109", "1011", "1012", "146", "147", "148", "149", "1411", "1412",
                "610", "810", "910", "1110", "1210", "613", "813", "913", "1113", "1213" ' 5 Grupos
                GruposCantidad = 5
            Case "136", "137", "138", "139", "1311", "1312", "710", "713", "1010", "1013", "1410", "1413" '6Grupos
                GruposCantidad = 6
            Case "1310", "1313" '7Grupos
                GruposCantidad = 7
            Case Else
                GruposCantidad = 0
        End Select
    End Function

    Private Sub btnLimpiar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLimpiar.Click
        LimpiarOrden()
        LimpiarGrupos()
        LimpiarGruposVentas()
        LimpiarCriterios()
    End Sub
    Private Sub LimpiarGrupos()
        LimpiarTextos(txtCategoriaDesde, txtCategoriaHasta, txtCodjer1, txtCodjer2, txtCodjer3, txtCodjer4,
            txtCodjer5, txtCodjer6, txtDivisionDesde, txtDivisionHasta, txtMarcaDesde, txtMarcaHasta)
    End Sub
    Private Sub LimpiarGruposVentas()
        LimpiarTextos(txtCanalDesde, txtCanalHasta, txtTipoNegocioDesde, txtTipoNegocioHasta, txtZonaDesde, txtZonaHasta,
            txtRutaDesde, txtRutaHasta, txtPais, txtEstado, txtMunicipio, txtParroquia, txtCiudad, txtBarrio)
    End Sub
    Private Sub LimpiarOrden()
        LimpiarTextos(txtOrdenDesde, txtOrdenHasta)
    End Sub
    Private Sub LimpiarCriterios()
        LimpiarTextos(txtAsesorDesde, txtAsesorHasta, txtAlmacenDesde, txtAlmacenHasta, txtProvCliDesde,
                      txtProvCliHasta, txtDocumentoDesde, txtDocumentoHasta, txtLoteDesde, txtLoteHasta)
    End Sub

    Private Sub chkList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkList.SelectedIndexChanged
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
        If lblCategoria.Visible Then LineaGrupos += "Categorías : " & txtCategoriaDesde.Text & "/" & txtCategoriaHasta.Text
        If LineaGrupos <> "" Then LineaGrupos += " - "
        If lblMarcas.Visible Then LineaGrupos += "Marcas : " & txtMarcaDesde.Text & "/" & txtMarcaHasta.Text
        If LineaGrupos <> "" Then LineaGrupos += " - "
        If lblDivisiones.Visible Then LineaGrupos += "División : " & txtDivisionDesde.Text & "/" & txtDivisionHasta.Text
        If LineaGrupos <> "" Then LineaGrupos += " - "
        If lblJerarquias.Visible Then LineaGrupos += "Jerarquía : " & txtTipoJerarquia.Text
    End Function
    Private Function LineaCriterios() As String
        LineaCriterios = ""
        LineaCriterios += IIf(lblPeriodo.Visible, "Período: ", IIf(lblPeriodoHasta.Visible, "Hasta : ", "")) & IIf(lblPeriodo.Visible, txtPeriodoDesde.Text, "") & IIf(lblPeriodo.Visible AndAlso lblPeriodoHasta.Visible, "/", "") & IIf(lblPeriodoHasta.Visible, txtPeriodoHasta.Text, "")
        If LineaCriterios <> "" Then LineaCriterios += " - "
        If lblAsesor.Visible Then LineaCriterios += " Asesor : " + txtAsesorDesde.Text + "/" + txtAsesorHasta.Text
    End Function
    Private Function LineaConstantes() As String
        LineaConstantes = ""
        If lblConsResumen.Visible Then LineaConstantes += " - Resumido : " + IIf(chkConsResumen.Checked, "Si", "No")
        If lblTarifa.Visible Then LineaConstantes += " - Tarifas : " + IIf(chkPrecioA.Checked, " A", "") + IIf(chkPrecioB.Checked, " B", "") + IIf(chkPrecioC.Checked, " C", "") + IIf(chkPrecioD.Checked, " D", "") + IIf(chkPrecioE.Checked, " E", "") + IIf(chkPrecioF.Checked, " F", "")
        If lblPeso.Visible Then LineaConstantes += " - Mercancias de peso solamente : " + IIf(chkPeso.Checked, "Si", "No")
        If lblCartera.Visible Then LineaConstantes += " - Cartera : " & cmbCartera.Text
        If lblExistencias.Visible Then LineaConstantes += " - Existencias : " & cmbExistencias.Text
        If lblTipo.Visible Then LineaConstantes += " - Tipo de mercancias : " & cmbTipo.Text
        If lblRegulada.Visible Then LineaConstantes += " - Regulada : " & cmbRegulada.Text
        If lblEstatus.Visible Then LineaConstantes += " - Estatus : " & cmbEstatus.Text
        If lblTarifa.Visible Then LineaConstantes += " - " & lblTarifa.Text & " " & cmbTarifa.Text
        If lblporGastos.Visible Then LineaConstantes += " - " & lblporGastos.Text & " " & txtporGastos.Text

        If LineaConstantes.Length > 2 And Mid(LineaConstantes, 1, 3) = " - " Then LineaConstantes = Mid(LineaConstantes, 4, LineaConstantes.Length)

    End Function
    Private Sub btnOrdenDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrdenDesde.Click
        txtDeOrden(txtOrdenDesde)
    End Sub

    Private Sub btnOrdenHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrdenHasta.Click
        txtDeOrden(txtOrdenHasta)
    End Sub
    Private Sub txtDeOrden(ByVal txt As TextBox)
        Dim f As New jsControlArcTablaSimple
        Dim dtDeOrden As New DataTable
        Dim nTAbleOrden As String = "tblorden"
        Select Case vOrdenCampos(cmbOrdenadoPor.SelectedIndex)
            Case "CODART", "codart", "item", "nomart", "NOMART"
                ds = DataSetRequery(ds, "select codart codigo, nomart descripcion from jsmerctainv where id_emp = '" & jytsistema.WorkID & "' order by codart ", myConn, nTAbleOrden, lblInfo)
                dtDeOrden = ds.Tables(nTAbleOrden)
                f.Cargar("Mercancías", ds, dtDeOrden, nTAbleOrden, TipoCargaFormulario.iShowDialog, False)
                txt.Text = f.Seleccion
            Case "Nombre de mercancías"
        End Select
        f.Close()
        f = Nothing
        dtDeOrden.Dispose()
        dtDeOrden = Nothing

    End Sub

    Private Sub txtOrdenDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOrdenDesde.TextChanged
        txtOrdenHasta.Text = txtOrdenDesde.Text
    End Sub
    Private Sub ColocarGrupos()
        grpGrupos.Text = "Agrupago por    " & "Mercancías : " & cmbMERAgrupadoPor.Text & "   /   Ventas : " & cmbVenAgrupadoPor.Text & " "
    End Sub

    Private Sub cmbMERAgrupadorPor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMERAgrupadoPor.SelectedIndexChanged

        ft.visualizarObjetos(False, lblGrupoDesde, lblGrupoHasta, lblCategoria, lblMarcas, lblDivisiones, lblJerarquias,
                            txtCategoriaDesde, btnCategoriaDesde, txtCategoriaHasta, btnCategoriaHasta,
                            txtMarcaDesde, btnMarcaDesde, txtMarcaHasta, btnMarcaHasta,
                            txtDivisionDesde, btnDivisionDesde, txtDivisionHasta, btnDivisionHasta,
                            txtTipoJerarquia, btnTipoJerarquia, txtCodjer1, btnCodjer1, txtCodjer2, btnCodjer2,
                            txtCodjer3, btnCodjer3, txtCodjer4, btnCodjer4, txtCodjer5, btnCodjer5, txtCodjer6, btnCodjer6)
        LimpiarGrupos()
        Dim ss As String = vMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex)
        Select Case ss
            Case "Ninguno"
            Case "Categorías"
                VerCategorias()
            Case "Marcas"
                VerMarcas()
            Case "Jerarquía"
                VerJerarquias()
            Case "División"
                VerDivisiones()
            Case "Mix"
            Case "Categorías & Marcas"
                VerCategorias()
                VerMarcas()
            Case "División & Categorías &  Marcas" '"División & Categorías &  Marcas"
                VerDivisiones()
                VerCategorias()
                VerMarcas()
            Case "Categorías & Jerarquías" '"Categorías & Jerarquías"
                VerCategorias()
                VerJerarquias()
            Case "División & Jerarquía" '"División & Jerarquía"
                VerDivisiones()
                VerJerarquias()
            Case "Mix & Categorías & Marcas" '"Mix & Categorías & Marcas"
                VerCategorias()
                VerMarcas()
            Case "Mix & Jerarquías" '"Mix & Jerarquías"
                VerJerarquias()
            Case "Mix & División" '"Mix & División"
                VerDivisiones()
            Case "Mix & División & Categorías & Marcas"
                VerDivisiones()
                VerCategorias()
                VerMarcas()
            Case "Mix & División & Jerarquías"
                VerDivisiones()
                VerJerarquias()
            Case "División & Mix"
                VerDivisiones()
            Case Else
                VerCategorias()
                VerMarcas()
                VerJerarquias()
                VerDivisiones()
        End Select

        ColocarGrupos()
    End Sub
    Private Sub cmbVENAgrupadorPor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbVenAgrupadoPor.SelectedIndexChanged

        ft.visualizarObjetos(False, Label7, Label8, Label9, Label11, Label1, Label2, Label3, Label4, Label5, Label6,
                          txtCanalDesde, btnCanalDesde, txtCanalHasta, btnCanalHasta,
                          txtTipoNegocioDesde, btnTipoNegocioDesde, txtTipoNegocioHasta, btnTipoNegocioHasta,
                          txtZonaDesde, btnZonaDesde, txtZonaHasta, btnZonaHasta,
                          txtRutaDesde, btnRutaDesde, txtRutaHasta, btnRutaHasta,
                          txtAsesorRDesde, btnAsesorRDesde, txtAsesorRHasta, btnAsesorRHasta,
                          txtPais, txtEstado, txtMunicipio, txtParroquia, txtCiudad, txtBarrio,
                          btnPais, btnEstado, btnMunicipio, btnParroquia, btnCiudad, btnBarrio)


        LimpiarGruposVentas()
        Select Case cmbVenAgrupadoPor.SelectedIndex
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
                VerCanal()
                VerTipoNegocio()
            Case 7
                VerZona()
                VerRuta()
            Case 8
                VerAsesor()
                VerCanal()
            Case 9
                VerAsesor()
                VerTipoNegocio()
            Case 10
                VerAsesor()
                VerCanal()
                VerTipoNegocio()
            Case 11
                VerAsesor() : VerZona()
            Case 12
                VerAsesor() : VerRuta()
            Case 13
                VerAsesor() : VerZona() : VerRuta()
            Case 14
                VerTerritorio()
        End Select
        ColocarGrupos()

    End Sub
    Private Sub VerCanal()
        ft.visualizarObjetos(True, Label7, Label9)
        ft.visualizarObjetos(True, Label1, txtCanalDesde, btnCanalDesde, txtCanalHasta, btnCanalHasta)
        ft.habilitarObjetos(False, True, txtCanalDesde, txtCanalHasta)
    End Sub
    Private Sub VerTipoNegocio()
        ft.visualizarObjetos(True, Label7, Label9)
        ft.visualizarObjetos(True, Label2, txtTipoNegocioDesde, btnTipoNegocioDesde, txtTipoNegocioHasta, btnTipoNegocioHasta)
        ft.habilitarObjetos(False, True, txtTipoNegocioDesde, txtTipoNegocioHasta)

    End Sub
    Private Sub VerZona()
        ft.visualizarObjetos(True, Label7, Label9)
        ft.visualizarObjetos(True, Label3, txtZonaDesde, btnZonaDesde, txtZonaHasta, btnZonaHasta)
        ft.habilitarObjetos(False, True, txtZonaDesde, txtZonaHasta)
    End Sub
    Private Sub VerRuta()
        ft.visualizarObjetos(True, Label7, Label9)
        ft.visualizarObjetos(True, Label4, txtRutaDesde, btnRutaDesde, txtRutaHasta, btnRutaHasta)
        ft.habilitarObjetos(False, True, txtRutaDesde, txtRutaHasta)
    End Sub

    Private Sub VerAsesor()
        ft.visualizarObjetos(True, Label8, Label11)
        ft.visualizarObjetos(True, Label5, txtAsesorRDesde, btnAsesorRDesde, txtAsesorRHasta, btnAsesorRHasta)
        ft.habilitarObjetos(False, True, txtAsesorRDesde, txtAsesorRHasta)
    End Sub

    Private Sub VerTerritorio()
        ft.visualizarObjetos(True, Label8, Label11)
        ft.visualizarObjetos(True, Label6, txtPais, btnPais, txtEstado, btnEstado, txtMunicipio, btnMunicipio, txtParroquia, btnParroquia, txtCiudad, btnCiudad, txtBarrio, btnBarrio)
        ft.habilitarObjetos(False, True, txtPais, txtEstado, txtParroquia, txtMunicipio, txtCiudad, txtBarrio)
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
    Private Sub VerDivisiones()
        ft.visualizarObjetos(True, lblGrupoDesde, lblGrupoHasta)
        ft.visualizarObjetos(True, lblDivisiones, txtDivisionDesde, btnDivisionDesde, txtDivisionHasta, btnDivisionHasta)
        ft.habilitarObjetos(False, True, txtDivisionDesde, txtDivisionHasta)
    End Sub
    Private Sub VerJerarquias()
        ft.visualizarObjetos(True, lblGrupoDesde, lblGrupoHasta)
        ft.visualizarObjetos(True, lblJerarquias, txtTipoJerarquia, btnTipoJerarquia, txtCodjer1, btnCodjer1,
                            txtCodjer2, btnCodjer2, txtCodjer3, btnCodjer3, txtCodjer4, btnCodjer4, txtCodjer5, btnCodjer5, txtCodjer6, btnCodjer6)
        ft.habilitarObjetos(False, True, txtTipoJerarquia, txtCodjer1, txtCodjer2, txtCodjer3, txtCodjer4, txtCodjer5, txtCodjer6)
    End Sub

    Private Sub txtCategoriaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCategoriaDesde.TextChanged
        txtCategoriaHasta.Text = txtCategoriaDesde.Text
    End Sub

    Private Sub txtMarcaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMarcaDesde.TextChanged
        txtMarcaHasta.Text = txtMarcaDesde.Text
    End Sub

    Private Sub txtDivisionDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDivisionDesde.TextChanged
        txtDivisionHasta.Text = txtDivisionDesde.Text
    End Sub

    Private Sub btnCategoriaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoriaDesde.Click
        Dim fg As New jsControlArcTablaSimple
        fg.Cargar("Categorías", FormatoTablaSimple(Modulo.iCategoriaMerca), True, TipoCargaFormulario.iShowDialog)
        txtCategoriaDesde.Text = fg.Seleccion
        fg.Close()
        fg = Nothing
    End Sub

    Private Sub btnCategoriaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoriaHasta.Click
        Dim fg As New jsControlArcTablaSimple
        fg.Cargar("Categorías", FormatoTablaSimple(Modulo.iCategoriaMerca), True, TipoCargaFormulario.iShowDialog)
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
        txtDivisionDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select division codigo, descrip descripcion from jsmercatdiv where  id_emp = '" & jytsistema.WorkID & "' order by 1", "Divisiones de mercancías", txtDivisionDesde.Text)
    End Sub

    Private Sub btnDivisionHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDivisionHasta.Click
        txtDivisionHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select division codigo, descrip descripcion from jsmercatdiv where  id_emp = '" & jytsistema.WorkID & "' order by 1", "Divisiones de mercancías", txtDivisionHasta.Text)
    End Sub

    Private Sub btnTipoJerarquia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoJerarquia.Click
        txtTipoJerarquia.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT tipjer codigo, descrip descripcion FROM jsmerencjer WHERE  id_emp  = '" & jytsistema.WorkID & "' order by 1 ", " Tipo de Jerarquía",
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
                f.Cargar(" Codigo Jerarquía nivel " & Nivel & " ", ds, dtCJ, nTableCJ, TipoCargaFormulario.iShowDialog, False)
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

    Private Sub cmbOrdenDesde_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOrdenDesde.SelectedIndexChanged
        If cmbOrdenHasta.Items.Count > 0 Then cmbOrdenHasta.SelectedIndex = cmbOrdenDesde.SelectedIndex
    End Sub

    Private Sub cmbOrdenHasta_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOrdenHasta.SelectedIndexChanged
        If cmbOrdenDesde.Items.Count > 0 Then cmbOrdenDesde.SelectedIndex = cmbOrdenHasta.SelectedIndex
    End Sub

    Private Sub btnAsesorDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesorDesde.Click
        txtAsesorDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT codven codigo, concat(apellidos, ', ', nombres) descripcion FROM jsvencatven WHERE estatus = 1 and id_emp  = '" & jytsistema.WorkID & "' order by 1 ", "Asesores Comerciales",
                                                txtAsesorDesde.Text)
    End Sub
    Private Sub btnAsesorHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesorHasta.Click
        txtAsesorHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT codven codigo, concat(apellidos, ', ', nombres) descripcion FROM jsvencatven WHERE estatus = 1 and id_emp  = '" & jytsistema.WorkID & "' order by 1 ", "Asesores Comerciales",
                                                txtAsesorHasta.Text)
    End Sub

    Private Sub txtAsesorDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAsesorDesde.TextChanged
        txtAsesorHasta.Text = txtAsesorDesde.Text
    End Sub

    Private Sub btnAlmacenDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacenDesde.Click
        Dim f As New jsControlArcAlmacenes
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        txtAlmacenDesde.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub btnAlmacenHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacenHasta.Click
        Dim f As New jsControlArcAlmacenes
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        txtAlmacenHasta.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub btnProvCliDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProvCliDesde.Click
        If ReporteNumero = ReporteMercancias.cComprasDeMercancias Or ReporteNumero = ReporteMercancias.cComprasdeMercanciasActivacion _
            Or ReporteNumero = ReporteMercancias.cComprasdeMercanciasActivacionXMes Then
            txtProvCliDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codpro codigo, nombre descripcion from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Proveedores",
                                                             txtProvCliDesde.Text)
        Else
            txtProvCliDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcli codigo, nombre descripcion from jsvencatcli where estatus < 3 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Clientes",
                                                             txtProvCliDesde.Text)
        End If

    End Sub
    Private Sub btnProvCliHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProvCliHasta.Click
        If ReporteNumero = ReporteMercancias.cComprasDeMercancias Or ReporteNumero = ReporteMercancias.cComprasdeMercanciasActivacion _
            Or ReporteNumero = ReporteMercancias.cComprasdeMercanciasActivacionXMes Then
            txtProvCliHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codpro codigo, nombre descripcion from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Proveedores",
                                                 txtProvCliHasta.Text)
        Else
            txtProvCliHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcli codigo, nombre descripcion from jsvencatcli where estatus < 3 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Clientes",
                                                             txtProvCliHasta.Text)
        End If

    End Sub

    Private Sub txtProvCliDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProvCliDesde.TextChanged
        txtProvCliHasta.Text = txtProvCliDesde.Text
    End Sub

    Private Sub btnCanalDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCanalDesde.Click
        txtCanalDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsvenliscan where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Canal de Distribución",
                                               txtCanalDesde.Text)
    End Sub

    Private Sub btnCanalHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCanalHasta.Click
        txtCanalHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsvenliscan where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Canal de Distribución",
                                               txtCanalHasta.Text)
    End Sub

    Private Sub btnTipoNegocioDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoNegocioDesde.Click
        txtTipoNegocioDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsvenlistip where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Tipo de Negocio",
                                                     txtTipoNegocioDesde.Text)
    End Sub

    Private Sub btnTipoNegocioHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoNegocioHasta.Click
        txtTipoNegocioHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsvenlistip where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Tipo de Negocio",
                                                     txtTipoNegocioHasta.Text)
    End Sub

    Private Sub btnZonaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZonaDesde.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Zonas de Venta", FormatoTablaSimple(Modulo.iZonasClientes), True, TipoCargaFormulario.iShowDialog)
        txtZonaDesde.Text = f.Seleccion
        f.Close()
        f = Nothing
    End Sub

    Private Sub btnZonaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZonaHasta.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Zonas de venta", FormatoTablaSimple(Modulo.iZonasClientes), True, TipoCargaFormulario.iShowDialog)
        txtZonaHasta.Text = f.Seleccion
        f.Close()
        f = Nothing
    End Sub

    Private Sub btnRutaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRutaDesde.Click
        txtRutaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT codrut codigo, nomrut descripcion FROM jsvenencrut WHERE tipo = 0 and id_emp  = '" & jytsistema.WorkID & "' order by 1 ", "Rutas de Visita",
                                              txtRutaDesde.Text)
    End Sub

    Private Sub btnRutaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRutaHasta.Click
        txtRutaHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT codrut codigo, nomrut descripcion FROM jsvenencrut WHERE tipo = 0 and id_emp  = '" & jytsistema.WorkID & "' order by 1 ", "Rutas de Visita",
                                              txtRutaHasta.Text)
    End Sub

    Private Sub btnAsesorRDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesorRDesde.Click
        txtAsesorRDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT codven codigo, concat(apellidos, ', ', nombres) descripcion FROM jsvencatven WHERE estatus = 1 and id_emp  = '" & jytsistema.WorkID & "' order by 1 ", "Asesores Comerciales",
                                                 txtAsesorRDesde.Text)
    End Sub

    Private Sub btnAsesorRHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesorRHasta.Click
        txtAsesorRHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT codven codigo, concat(apellidos, ', ', nombres) descripcion FROM jsvencatven WHERE estatus = 1 and id_emp  = '" & jytsistema.WorkID & "' order by 1 ", "Asesores Comerciales",
                                                 txtAsesorRHasta.Text)
    End Sub

    Private Sub txtCanalDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCanalDesde.TextChanged
        txtCanalHasta.Text = txtCanalDesde.Text
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

    Private Sub txtAsesorRDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAsesorRDesde.TextChanged
        txtAsesorRHasta.Text = txtAsesorRDesde.Text
    End Sub


    Private Sub txtAlmacenDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlmacenDesde.TextChanged
        txtAlmacenHasta.Text = txtAlmacenDesde.Text
    End Sub

    Private Sub btnPais_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPais.Click
        txtPais.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1", "Paises",
                                         txtPais.Text)
    End Sub

    Private Sub btnEstado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEstado.Click
        If txtPais.Text <> "" Then _
            txtEstado.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 1 and antecesor = " & txtPais.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Estados",
                txtEstado.Text)

    End Sub

    Private Sub btnMunicipio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMunicipio.Click
        If txtEstado.Text <> "" Then _
            txtMunicipio.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 2 and antecesor = " & txtEstado.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Municipios",
                txtMunicipio.Text)

    End Sub

    Private Sub btnParroquia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnParroquia.Click
        If txtMunicipio.Text <> "" Then _
            txtParroquia.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 2 and antecesor = " & txtMunicipio.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Parroquias",
                txtParroquia.Text)
    End Sub

    Private Sub btnCiudad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCiudad.Click
        If txtParroquia.Text <> "" Then _
                    txtCiudad.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 3 and antecesor = " & txtParroquia.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Ciudades",
                        txtCiudad.Text)
    End Sub

    Private Sub btnBarrio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBarrio.Click
        If txtCiudad.Text <> "" Then _
                    txtBarrio.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 4 and antecesor = " & txtCiudad.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Barrios",
                        txtBarrio.Text)
    End Sub


    Private Sub chkConsResumen_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkConsResumen.CheckedChanged
        If ReporteNumero = ReporteMercancias.cVentasDeMercanciasActivacionXMes Then
            If chkConsResumen.Checked Then
                chkPeso.Checked = True
                chkPeso.Enabled = False
            Else
                chkPeso.Enabled = True
            End If
        End If
    End Sub

    Private Sub txtporGastos_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtporGastos.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
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

    Private Sub cmbTipoEtiqueta_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbTipoEtiqueta.SelectedIndexChanged

        If ReporteNumero = ReporteMercancias.cCodigoBarras Then
            Select Case cmbTipoEtiqueta.SelectedIndex
                Case 0, 2
                    ft.visualizarObjetos(True, chkConsResumen, lblConsResumen)
                    lblConsResumen.Text = "Precio Etiqueta"
                Case Else
                    ft.visualizarObjetos(False, chkConsResumen, lblConsResumen)
            End Select
        End If

    End Sub

    Private Sub btnCausaDesde_Click(sender As Object, e As EventArgs) Handles btnCausaDesde.Click
        txtCausaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iCausaDesincorporaciones) & "' and id_emp = '" & jytsistema.WorkID & "' ", "Causas de Desincorporaciones/Autoconsumos", txtCausaDesde.Text)
    End Sub

    Private Sub txtCausaDesde_TextChanged(sender As Object, e As EventArgs) Handles txtCausaDesde.TextChanged
        txtCausaHasta.Text = txtCausaDesde.Text
    End Sub

    Private Sub btnCausaHasta_Click(sender As Object, e As EventArgs) Handles btnCausaHasta.Click
        txtCausaHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iCausaDesincorporaciones) & "' and id_emp = '" & jytsistema.WorkID & "' ", "Causas de Desincorporaciones/Autoconsumos", txtCausaHasta.Text)
    End Sub
End Class