Imports MySql.Data.MySqlClient
Imports System.IO
Imports ReportesDeMedicionGerencial
Public Class jsSIGMERepParametros

    Private Const sModulo As String = "Reportes de Medición gerencial"

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

    Private vMERAgrupadoPor() As String = {"Ninguno", "Categorías", "Marcas", "Jerarquía", "División", "Mix", _
        "Categorías & Marcas", "División & Categorías &  Marcas", _
        "Categorías & Jerarquías", _
        "División & Jerarquía", "Mix & Categorías & Marcas", "Mix & Jerarquías", _
        "Mix & División", "Mix & División & Categorías & Marcas", "Mix & División & Jerarquías"}

    Private aMERAgrupadoPor() As String = {"", "categoria", "marca", "tipjer", "division", "mix", _
                                        "categoria, marca", "division, categoria, marca", _
                                        "categoria, tipjer", "division, tipjer", _
                                        "mix, categoria, marca", "mix, tipjer", "mix, division", _
                                        "mix, division, categoria, marca", "mix, division, tipjer"}
    Private vMERAgrupadoPorR() As String = {"Ninguno", "Categorías", "Marcas", "Jerarquía", "División", "Mix"}
    Private aMERAgrupadoPorR() As String = {"", "categoria", "marca", "tipjer", "division", "mix"}

    Private vMERAgrupadoPorJ() As String = {"Jerarquía"}
    Private aMERAgrupadoPorJ() As String = {"tipjer"}

    Private aSiNoTodos() As String = {"Si", "No", "Todos"}
    Private aExistencias() As String = {"Con", "Sin", "Todas"}
    Private aEstatus() As String = {"Activo", "Inactivo", "Todos"}
    Private aTipo() As String = {"Venta", "Uso interno", "POP", "Alquiler", "Préstamo", "Materia prima", "Otros", "Todos"}
    Private aTipoReporte() As String = {"Unidad Venta (UV) ", "Kilogramos (UMP)", "Ventas (BsF)", "Cajas (UMS)", "Costos (BsF)"}
    Private aTarifa() As String = {"A", "B", "C", "D", "E", "F"}
    Private aTerritorio() As String = {"Barrio/Urbanización/Sector", "Ciudad/Pueblo/Aldea", "Parroquia/Comunidad", "Municipio/Región", "Estado/Provincia/Departamento", "País"}
    Private vVENAgrupadoPor() As String = {"Ninguno", "Canal de Distribución", "Tipo de Negocio", "Zona", "Ruta", _
                                           "Asesor Comercial", "Canal & Tipo Negocio", "Zona & Ruta", _
                                           "Asesor & Canal", "Asesor & Tipo de Negocio", "Asesor & Canal & Tipo de negocio", _
                                           "Asesor & Zona", "Asesor & ruta", "Asesor & Zona & Ruta", "Territorio"}
    Private aVENAgrupadoPor() As String = {"", "canal", "tiponegocio", "zona", "ruta", "asesor", "canal, tiponegocio", _
                                           "zona, ruta", "asesor, canal", "asesor, tiponegocio", "asesor, canal, tiponegocio", _
                                           "asesor, zona", "asesor, ruta", "asesor, zona, ruta", "territorio"}
    Private vCOMAgrupadoPor() As String = {"Ninguno", "Categorías", "Unidades de Negocio", "Categorías & Unidades"}
    Private aCOMAgrupadoPor() As String = {"", "categoriaprov", "unidadnegocioprov", "categoriaprov, unidadnegocioprov"}
    Private aAño() As Object = {2000, 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, _
                                  2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028, 2029, 2030}

    Private periodoTipo As TipoPeriodo
    Public Sub Cargar(ByVal TipoCarga As Integer, ByVal numReporte As Integer, ByVal nomReporte As String, _
                      Optional ByVal CodMercancia As String = "", Optional ByVal numDocumento As String = "", _
                      Optional ByVal Fecha As Date = #1/1/2009#)


        Me.Tag = sModulo
        Me.Dock = DockStyle.Fill
        myConn = New MySqlConnection(jytsistema.strConn)
        myConn.Open()

        ReporteNumero = numReporte
        ReporteNombre = nomReporte
        CodigoMercancia = CodMercancia
        Documento = numDocumento
        FechaParametro = Fecha

        ft.RellenaCombo(aDesde, cmbOrdenDesde)
        ft.RellenaCombo(aHasta, cmbOrdenHasta)
        ft.RellenaCombo(aTarifa, cmbTarifa)
        ft.RellenaCombo(aAño, cmbAño, Year(jytsistema.sFechadeTrabajo) - 2000)

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
            Case ReporteMedicionGerencial.cGananciasPorFactura
                Dim vOrdenNombres() As String = {"N° Factura"}
                Dim vOrdenCampos() As String = {"numdoc"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)
            Case ReporteMedicionGerencial.cGananciasPorItem
                Dim vOrdenNombres() As String = {"Código Mercancía"}
                Dim vOrdenCampos() As String = {"codart"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)
            Case ReporteMedicionGerencial.cGananciasItemMesMes, ReporteMedicionGerencial.cActivacionClientesMesMes
                Dim vOrdenNombres() As String = {"Código Mercancía"}
                Dim vOrdenCampos() As String = {"codart"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, True, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)

            Case ReporteMedicionGerencial.cComprasComparativasMesMes, ReporteMedicionGerencial.cVentasComparativasMesMes
                Dim vOrdenNombres() As String = {"Código Mercancía"}
                Dim vOrdenCampos() As String = {"codart"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)

            Case ReporteMedicionGerencial.cIngresosPorAsesor
                Dim vOrdenNombres() As String = {"Código Asesor"}
                Dim vOrdenCampos() As String = {"codven"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {5}
                Inicializar(ReporteNombre, False, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)
            Case ReporteMedicionGerencial.cVentasMesMesAsesor, ReporteMedicionGerencial.cDevolucionesAsesorMesMes
                Dim vOrdenNombres() As String = {"Código Asesor"}
                Dim vOrdenCampos() As String = {"codven"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {5}
                Inicializar(ReporteNombre, False, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)
            Case ReporteMedicionGerencial.cDropSizeMesMes
                Dim vOrdenNombres() As String = {"Código Mercancía"}
                Dim vOrdenCampos() As String = {"codart"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)
            Case ReporteMedicionGerencial.cGananciasCestaTicket
                Dim vOrdenNombres() As String = {"Corredor"}
                Dim vOrdenCampos() As String = {"codigo"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)
            Case ReporteMedicionGerencial.cGananciasCestaTicketMesMes
                Dim vOrdenNombres() As String = {"Corredor"}
                Dim vOrdenCampos() As String = {"codigo"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)
            Case ReporteMedicionGerencial.cDescuentosAsesor
                Dim vOrdenNombres() As String = {"Código Mercancía"}
                Dim vOrdenCampos() As String = {"codart"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)
            Case ReporteMedicionGerencial.cDescuentosAsesorMesMes
                Dim vOrdenNombres() As String = {"Código Mercancía"}
                Dim vOrdenCampos() As String = {"codart"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)

            Case ReporteMedicionGerencial.cVentasComprasExistenciasMesMes
                Dim vOrdenNombres() As String = {"Jerarquía"}
                Dim vOrdenCampos() As String = {"tipjer"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, True, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoMercancia)

        End Select
    End Sub
    Private Sub Inicializar(ByVal nEtiqueta As String, ByVal TabOrden As Boolean, ByVal TabGrupo As Boolean, _
        ByVal TabCriterio As Boolean, ByVal TabConstantes As Boolean, ByVal aNombreOrden() As String, _
        ByVal aCampoOrden() As String, ByVal aTipoOrden() As String, ByVal aLongitudOrden() As Integer, _
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

        Select Case ReporteNumero
            Case ReporteMedicionGerencial.cVentasMesMesAsesor, ReporteMedicionGerencial.cDevolucionesAsesorMesMes
                ft.RellenaCombo(vMERAgrupadoPorR, cmbMERAgrupadoPor)
            Case ReporteMedicionGerencial.cVentasComprasExistenciasMesMes
                ft.RellenaCombo(vMERAgrupadoPorJ, cmbMERAgrupadoPor)
            Case Else
                ft.RellenaCombo(vMERAgrupadoPor, cmbMERAgrupadoPor)
        End Select
        
        ft.RellenaCombo(vVENAgrupadoPor, cmbVenAgrupadoPor)
        ft.RellenaCombo(vCOMAgrupadoPor, cmbCOMAgrupadoPor)

        ft.habilitarObjetos(False, False, TabPageVentas, TabPageCompras)

        Select Case ReporteNumero
            Case ReporteMedicionGerencial.cGananciasPorFactura, ReporteMedicionGerencial.cGananciasPorItem, _
                ReporteMedicionGerencial.cGananciasItemMesMes, ReporteMedicionGerencial.cVentasComparativasMesMes, _
                ReporteMedicionGerencial.cActivacionClientesMesMes, ReporteMedicionGerencial.cDescuentosAsesor, _
                ReporteMedicionGerencial.cDescuentosAsesorMesMes
                ft.habilitarObjetos(True, False, TabPageVentas)
            Case ReporteMedicionGerencial.cComprasComparativasMesMes
                ft.habilitarObjetos(True, False, TabPageCompras)
        End Select

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

        Select Case ReporteNumero
            Case ReporteMedicionGerencial.cGananciasPorFactura, ReporteMedicionGerencial.cGananciasPorItem, ReporteMedicionGerencial.cGananciasCestaTicket
                VerCriterio_Periodo(True, 0)
            Case ReporteMedicionGerencial.cIngresosPorAsesor
                VerCriterio_Asesor(True)
                VerCriterio_Periodo(True, 0)
            Case ReporteMedicionGerencial.cComprasComparativasMesMes, ReporteMedicionGerencial.cVentasComparativasMesMes
                VerCriterio_ProveedorCliente(True)
            Case ReporteMedicionGerencial.cVentasMesMesAsesor, ReporteMedicionGerencial.cDevolucionesAsesorMesMes
                VerCriterio_Año(True)
                VerCriterio_Asesor(True)
            Case ReporteMedicionGerencial.cDropSizeMesMes, ReporteMedicionGerencial.cDescuentosAsesorMesMes
                VerCriterio_Año(True)
                VerCriterio_ProveedorCliente(True)
            Case ReporteMedicionGerencial.cGananciasCestaTicketMesMes
                VerCriterio_Año(True)
            Case ReporteMedicionGerencial.cDescuentosAsesor
                VerCriterio_Periodo(True, 0)
                VerCriterio_ProveedorCliente(True)
            Case ReporteMedicionGerencial.cVentasComprasExistenciasMesMes
                VerCriterio_Año(True)
                VerCriterio_Almacen(True)
            Case Else
        End Select

    End Sub

    Private Sub VerCriterio_Periodo(ByVal Ver As Boolean, ByVal CompletoDesdeHasta As Integer, Optional ByVal Periodo As TipoPeriodo = TipoPeriodo.iMensual)
        'CompletoDesdeHasta 0 = Complete , 1 = Desde , 2 = Hasta 
        periodoTipo = Periodo
        ft.visualizarObjetos(False, lblPeriodoHasta, lblPeriodo, txtPeriodoDesde, txtPeriodoHasta, btnPeriodoDesde, btnPeriodoHasta)
        ft.habilitarObjetos(False, True, txtPeriodoDesde, txtPeriodoHasta)
        If Ver Then

            Select Case CompletoDesdeHasta
                Case 0
                    ft.visualizarObjetos(Ver, lblPeriodoHasta, lblPeriodo, txtPeriodoDesde, txtPeriodoHasta, btnPeriodoDesde, btnPeriodoHasta)
                Case 1
                    ft.visualizarObjetos(Ver, lblPeriodo, txtPeriodoDesde, btnPeriodoDesde)
                Case 2
                    ft.visualizarObjetos(Ver, lblPeriodoHasta, lblPeriodo, txtPeriodoHasta, btnPeriodoHasta)
            End Select
        End If


        Select Case Periodo
            Case TipoPeriodo.iDiario
                txtPeriodoDesde.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
                txtPeriodoHasta.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
            Case TipoPeriodo.iSemanal
                txtPeriodoDesde.Text = ft.FormatoFecha(PrimerDiaSemana(jytsistema.sFechadeTrabajo))
                txtPeriodoHasta.Text = ft.FormatoFecha(UltimoDiaSemana(jytsistema.sFechadeTrabajo))
            Case TipoPeriodo.iMensual
                txtPeriodoDesde.Text = ft.FormatoFecha(PrimerDiaMes(jytsistema.sFechadeTrabajo))
                txtPeriodoHasta.Text = ft.FormatoFecha(UltimoDiaMes(jytsistema.sFechadeTrabajo))
            Case TipoPeriodo.iAnual
                txtPeriodoDesde.Text = ft.FormatoFecha(PrimerDiaAño(jytsistema.sFechadeTrabajo))
                txtPeriodoHasta.Text = ft.FormatoFecha(UltimoDiaAño(jytsistema.sFechadeTrabajo))
            Case Else
                txtPeriodoDesde.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
                txtPeriodoHasta.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        End Select

    End Sub
    Private Sub VerCriterio_Año(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblAño, cmbAño)
        'ft.habilitarObjetos(False, True, cmbAño)
    End Sub
    Private Sub VerCriterio_Asesor(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblAsesor, lblAsesorHasta, txtAsesorDesde, txtAsesorHasta, btnAsesorDesde, btnAsesorHasta)
        ft.habilitarObjetos(False, True, txtAsesorDesde, txtAsesorHasta)
    End Sub
    Private Sub VerCriterio_TipoDocumento(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblTipodocumento, chkList)
        Dim aTipoDocumento() As String = {"EN", "SA", "AE", "AS", "AC"}
        Dim aSel() As Boolean = {True, True, True, True, True}
        RellenaListaSeleccionable(chkList, aTipoDocumento, aSel)
        txtTipDoc.Text = ".EN.SA.AE.AS.AC."
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
        VerConstante_ResumenMercas(False)
        VerConstante_Tarifas(False)
        VerConstante_Tarifa(False)
        VerConstante_peso(False)
        VerConstante_TipoMercancia(False)
        VerConstante_Cartera(False)
        VerConstante_Regulado(False)
        VerConstante_Estatus(False)
        verConstante_Existencias(False)
        verConstante_TipoReporte(False)

        Select Case ReporteNumero
            Case ReporteMedicionGerencial.cGananciasPorFactura
                verConstante_Resumen(True)
            Case ReporteMedicionGerencial.cComprasComparativasMesMes, ReporteMedicionGerencial.cVentasComparativasMesMes
                verConstante_Resumen(True)
                verConstante_TipoReporte(True)
            Case ReporteMedicionGerencial.cVentasMesMesAsesor, ReporteMedicionGerencial.cDevolucionesAsesorMesMes
                verConstante_TipoReporte(True)
            Case ReporteMedicionGerencial.cDescuentosAsesor, ReporteMedicionGerencial.cDescuentosAsesorMesMes
                verConstante_Resumen(True)
                lblConsResumen.Text = "Resumen clientes"
                verConstante_ResumenMercas(True)
                Dim atipoR() As String = {"Generales", "Bonificaciones", "Clientes", "Mercancías", "Ofertas", "Devoluciones", "Todos"}
                ft.RellenaCombo(atipoR, cmbTipoReporte, 6)
                verConstante_TipoReporte(True)
                lblTipoReporte.Text = "Tipo descuento "
        End Select

    End Sub
    Private Sub ValoresInicialesConstantes()

        chkConsResumen.Checked = False
        chkResumenMercas.Checked = False
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
        ft.RellenaCombo(aTipoReporte, cmbTipoReporte)
        ft.RellenaCombo(aEstatus, cmbEstatus)
        ft.RellenaCombo(aExistencias, cmbExistencias, 2)

    End Sub
    Private Sub verConstante_TipoReporte(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblTipoReporte, cmbTipoReporte)
    End Sub
    Private Sub verConstante_Existencias(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblExistencias, cmbExistencias)
    End Sub
    Private Sub verConstante_Resumen(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblConsResumen, chkConsResumen)
    End Sub
    Private Sub verConstante_ResumenMercas(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblChkMercas, chkResumenMercas)
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

    Private Sub jsSIGMERepParametros_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsSIGMERepParametros_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        Dim dsSIGME As New dsSIGME
        Dim str As String = ""
        Dim nTabla As String = ""
        Dim oReporte As New CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim PresentaArbol As Boolean = False

        Select Case ReporteNumero
            Case ReporteMedicionGerencial.cDescuentosAsesorMesMes
                nTabla = "dtDescuentosAsesorMesMes"

                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbVenAgrupadoPor.SelectedIndex.ToString)
                    Case 0  ' 0 grupos
                        oReporte = New rptSIGMEDescuentosAsesorMes0G
                    Case 1
                        oReporte = New rptSIGMEDescuentosAsesorMes1G
                    Case 2
                        oReporte = New rptSIGMEDescuentosAsesorMes2G
                    Case 3
                        oReporte = New rptSIGMEDescuentosAsesorMes3G
                    Case 4
                        oReporte = New rptSIGMEDescuentosAsesorMes4G
                    Case Else
                        oReporte = New rptSIGMEDescuentosAsesorMes0G
                End Select
                str = SeleccionSIGMEDescuentos(myConn, lblInfo, CDate("01/01/" & (cmbAño.SelectedIndex + 2000).ToString), CDate("31/12/" & (cmbAño.SelectedIndex + 2000).ToString), txtOrdenDesde.Text, txtOrdenHasta.Text, _
                                            vOrdenCampos(cmbOrdenadoPor.SelectedIndex), txtProvCliDesde.Text, txtProvCliHasta.Text, txtCategoriaDesde.Text, txtCategoriaHasta.Text, _
                                            txtMarcaDesde.Text, txtMarcaHasta.Text, txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, _
                                            txtCodjer5.Text, txtCodjer6.Text, txtDivisionDesde.Text, txtDivisionHasta.Text, txtCanalDesde.Text, txtCanalHasta.Text, _
                                            txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text, txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text, _
                                            txtAsesorRDesde.Text, txtAsesorRHasta.Text, txtPais.Text, txtEstado.Text, txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, _
                                            False, cmbTipoReporte.SelectedIndex)

            Case ReporteMedicionGerencial.cDescuentosAsesor
                nTabla = "dtDescuentosAsesor"

                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbVenAgrupadoPor.SelectedIndex.ToString)
                    Case 0  ' 0 grupos
                        oReporte = New rptSIGMEDescuentosAsesor0G
                    Case 1
                        oReporte = New rptSIGMEDescuentosAsesor1G
                    Case 2
                        oReporte = New rptSIGMEDescuentosAsesor2G
                    Case 3
                        oReporte = New rptSIGMEDescuentosAsesor3G
                    Case 4
                        oReporte = New rptSIGMEDescuentosAsesor4G
                    Case Else
                        oReporte = New rptSIGMEDescuentosAsesor0G
                End Select
                str = SeleccionSIGMEDescuentos(myConn, lblInfo, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), txtOrdenDesde.Text, txtOrdenHasta.Text, _
                                            vOrdenCampos(cmbOrdenadoPor.SelectedIndex), txtProvCliDesde.Text, txtProvCliHasta.Text, txtCategoriaDesde.Text, txtCategoriaHasta.Text, _
                                            txtMarcaDesde.Text, txtMarcaHasta.Text, txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, _
                                            txtCodjer5.Text, txtCodjer6.Text, txtDivisionDesde.Text, txtDivisionHasta.Text, txtCanalDesde.Text, txtCanalHasta.Text, _
                                            txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text, txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text, _
                                            txtAsesorRDesde.Text, txtAsesorRHasta.Text, txtPais.Text, txtEstado.Text, txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, _
                                            False, cmbTipoReporte.SelectedIndex)

            Case ReporteMedicionGerencial.cVentasComprasExistenciasMesMes
                nTabla = "dtVentasComprasExistencias"
                oReporte = New rptSIGMEVentasComprasPorJerarquia
                str = SeleccionSIGMEVentasyComprasMesaMes(myConn, lblInfo, txtTipoJerarquia.Text, txtTipoJerarquia.Text, txtAlmacenDesde.Text, txtAlmacenHasta.Text, cmbAño.SelectedIndex + 2000)

            Case ReporteMedicionGerencial.cGananciasCestaTicketMesMes
                nTabla = "dtGananciaCestaticketMesMes"
                oReporte = New rptSIGMEGananciasCestaTicketMesMes
                str = SeleccionSIGMEGananciasCestaTicketMesMes(myConn, lblInfo, cmbAño.SelectedIndex + 2000)
            Case ReporteMedicionGerencial.cGananciasCestaTicket
                nTabla = "dtGananciaCestaticket"
                oReporte = New rptSIGMEGananciasCestaTicket
                str = SeleccionSIGMEGananciaCestaTicketEnPeriodo(myConn, lblInfo, txtOrdenDesde.Text, txtOrdenHasta.Text, _
                                                                 CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text))
            Case ReporteMedicionGerencial.cDropSizeMesMes
                nTabla = "dtDropSize"
                oReporte = New rptSIGMEDropSizeMesMes
                str = SeleccionSIGMEDropSize(myConn, lblInfo, txtOrdenDesde.Text, txtOrdenHasta.Text, txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, _
                                             txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text, txtDivisionHasta.Text, _
                                             txtProvCliDesde.Text, txtProvCliHasta.Text, txtCanalDesde.Text, txtCanalHasta.Text, _
                                             txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text, txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text, _
                                             txtAsesorRDesde.Text, txtAsesorRHasta.Text, txtPais.Text, txtEstado.Text, txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, _
                                             cmbAño.SelectedIndex + 2000)

            Case ReporteMedicionGerencial.cIngresosPorAsesor
                nTabla = "dtIngresosAsesor"
                oReporte = New rptSIGMEIngresosAsesor0G
                str = SeleccionSIGMEIngresosporAsesoryForma(myConn, lblInfo, txtAsesorDesde.Text, txtAsesorHasta.Text, _
                                                             CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text))
            Case ReporteMedicionGerencial.cComprasComparativasMesMes

                nTabla = "dtComprasVentasComparativas"
                oReporte = New rptSIGMEComprasVentasComparativas
                str = SeleccionSIGMEComprasComparativasMesMes(myConn, lblInfo, txtOrdenDesde.Text, txtOrdenHasta.Text, _
                                                              txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, _
                                                              txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, txtCategoriaDesde.Text, _
                                                              txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text, _
                                                              txtDivisionHasta.Text, txtProvCliDesde.Text, txtProvCliHasta.Text, _
                                                              txtCategoriaProveedorDesde.Text, txtCategoriaProveedorHasta.Text, txtUnidadProveedorDesde.Text, _
                                                              txtUnidadProveedorHasta.Text, cmbTipoReporte.SelectedIndex)

            Case ReporteMedicionGerencial.cVentasComparativasMesMes
                nTabla = "dtComprasVentasComparativas"
                oReporte = New rptSIGMEComprasVentasComparativas
                str = SeleccionSIGMEVentasComparativasMesMes(myConn, lblInfo, txtOrdenDesde.Text, txtOrdenHasta.Text, txtTipoJerarquia.Text, _
                                                             txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, _
                                                             txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text, txtDivisionHasta.Text, _
                                                             txtProvCliDesde.Text, txtProvCliHasta.Text, txtCanalDesde.Text, txtCanalHasta.Text, _
                                                             txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text, txtZonaDesde.Text, txtZonaHasta.Text, _
                                                             txtRutaDesde.Text, txtRutaHasta.Text, txtAsesorRDesde.Text, txtAsesorRHasta.Text, _
                                                             txtPais.Text, txtEstado.Text, txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, _
                                                             cmbTipoReporte.SelectedIndex)
            Case ReporteMedicionGerencial.cVentasMesMesAsesor
                nTabla = "dtVentasMesMes"
                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbVenAgrupadoPor.SelectedIndex.ToString)
                    Case 0  ' 0 grupos
                        oReporte = New rptSIGMEVentasMesMes0G
                    Case 1  ' 1 grupo
                        oReporte = New rptSIGMEVentasMesMes1G
                    Case Else
                        oReporte = New rptSIGMEVentasMesMes0G
                End Select
                str = SeleccionSIGMEVentasMesMes(myConn, lblInfo, txtOrdenDesde.Text, txtOrdenHasta.Text, txtTipoJerarquia.Text, _
                                                             txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text, txtDivisionHasta.Text, _
                                                             cmbAño.SelectedIndex + 2000,
                                                             cmbMERAgrupadoPor.Text, cmbTipoReporte.SelectedIndex)

            Case ReporteMedicionGerencial.cDevolucionesAsesorMesMes
                nTabla = "dtVentasMesMes"
                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbVenAgrupadoPor.SelectedIndex.ToString)
                    Case 0  ' 0 grupos
                        oReporte = New rptSIGMEDevolucionesAsesorMesMes0G
                    Case 1  ' 1 grupo
                        oReporte = New rptSIGMEDevolucionesAsesorMesMes1G
                    Case Else
                        oReporte = New rptSIGMEDevolucionesAsesorMesMes0G
                End Select
                str = SeleccionSIGMEDevolucionesMesMes(myConn, lblInfo, txtOrdenDesde.Text, txtOrdenHasta.Text, txtTipoJerarquia.Text, _
                                                             txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text, txtDivisionHasta.Text, _
                                                             cmbAño.SelectedIndex + 2000,
                                                             cmbMERAgrupadoPor.Text, cmbTipoReporte.SelectedIndex)


            Case ReporteMedicionGerencial.cGananciasPorFactura
                PresentaArbol = True
                nTabla = "dtGananciasFacturas"
                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbVenAgrupadoPor.SelectedIndex.ToString)
                    Case 0  ' 0 grupos
                        oReporte = New rptSIGMEGananciasFacturas0G
                    Case 1  ' 1 grupo
                        oReporte = New rptSIGMEGananciasFacturas1G
                    Case 2  '2 grupos
                        oReporte = New rptSIGMEGananciasFacturas2G
                    Case 3 '3 grupos
                        oReporte = New rptSIGMEGananciasFacturas3G
                    Case 4 '4 grupos
                        oReporte = New rptSIGMEGananciasFacturas4G
                    Case Else
                        oReporte = New rptSIGMEGananciasFacturas0G
                End Select

                str = SeleccionSIGMEGananciasBrutasFacturas(myConn, lblInfo, txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex),
                                             txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, _
                                            txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, txtCategoriaDesde.Text, _
                                            txtCategoriaHasta.Text, txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text, _
                                            txtDivisionHasta.Text, cmbEstatus.SelectedIndex, cmbCartera.SelectedIndex, chkPeso.Checked, cmbTipo.SelectedIndex, _
                                            cmbRegulada.SelectedIndex, txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, _
                                            txtTipoNegocioHasta.Text, txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text, _
                                            txtAsesorRDesde.Text, txtAsesorRHasta.Text, txtPais.Text, txtEstado.Text, txtMunicipio.Text, txtParroquia.Text, _
                                            txtCiudad.Text, txtBarrio.Text, 0, 0, CDate(txtPeriodoDesde.Text), _
                                            CDate(txtPeriodoHasta.Text), txtAlmacenDesde.Text, txtAlmacenHasta.Text, _
                                            txtProvCliDesde.Text, txtProvCliHasta.Text, chkConsResumen.Checked, False)


            Case ReporteMedicionGerencial.cGananciasPorItem
                PresentaArbol = True
                nTabla = "dtGananciasItem"
                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbVenAgrupadoPor.SelectedIndex.ToString)
                    Case 0  ' 0 grupos
                        oReporte = New rptSIGMEGananciasItem0G
                    Case 1  ' 1 grupo
                        oReporte = New rptSIGMEGananciasItem1G
                    Case 2  '2 grupos
                        oReporte = New rptSIGMEGananciasItem2G
                    Case 3 '3 grupos
                        oReporte = New rptSIGMEGananciasItem3G
                    Case 4 '4 grupos
                        oReporte = New rptSIGMEGananciasItem4G
                    Case Else
                        oReporte = New rptSIGMEGananciasItem0G
                End Select

                str = SeleccionSIGMEGananciasBrutasItem(myConn, lblInfo, txtOrdenDesde.Text, txtOrdenHasta.Text, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), _
                                                        txtAsesorRDesde.Text, txtAsesorRHasta.Text, txtCanalDesde.Text, txtCanalHasta.Text, _
                                                        txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text, txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text, _
                                                        txtPais.Text, txtEstado.Text, txtMunicipio.Text, txtParroquia.Text, _
                                                        txtCiudad.Text, txtBarrio.Text, txtCategoriaDesde.Text, txtCategoriaHasta.Text, _
                                                        txtMarcaDesde.Text, txtMarcaHasta.Text, txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, _
                                                        txtDivisionDesde.Text, txtDivisionHasta.Text, False)

            Case ReporteMedicionGerencial.cGananciasItemMesMes
                PresentaArbol = True
                nTabla = "dtGananciasItemMesMes"
                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbVenAgrupadoPor.SelectedIndex.ToString)
                    Case 0  ' 0 grupos
                        oReporte = New rptSIGMEGananciasItemMesMes0G
                    Case 1  ' 1 grupo
                        oReporte = New rptSIGMEGananciasItemMesMes1G
                    Case 2  '2 grupos
                        'oReporte = New rptSIGMEGananciasItem2G
                    Case 3 '3 grupos
                        'oReporte = New rptSIGMEGananciasItem3G
                    Case 4 '4 grupos
                        'oReporte = New rptSIGMEGananciasItem4G
                    Case Else
                        oReporte = New rptSIGMEGananciasItemMesMes0G
                End Select

                str = SeleccionSIGMEGananciasBrutasMesMes(myConn, lblInfo, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), _
                                                            txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, _
                                                            txtAsesorRDesde.Text, txtAsesorRHasta.Text, txtCanalDesde.Text, txtCanalHasta.Text, _
                                                            txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text, txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text, _
                                                            txtPais.Text, txtEstado.Text, txtMunicipio.Text, txtParroquia.Text, _
                                                            txtCiudad.Text, txtBarrio.Text, txtCategoriaDesde.Text, txtCategoriaHasta.Text, _
                                                            txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text, txtDivisionHasta.Text, _
                                                            CamposEnGrupos())

            Case ReporteMedicionGerencial.cActivacionClientesMesMes
                PresentaArbol = True
                nTabla = "dtActivacionMesMes"
                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbVenAgrupadoPor.SelectedIndex.ToString)
                    Case 0  ' 0 grupos
                        oReporte = New rptSIGMEActivacionMesMes0G
                    Case 1  ' 1 grupo
                        oReporte = New rptSIGMEActivacionMesMes1G
                    Case 2  '2 grupos
                        oReporte = New rptSIGMEActivacionMesMes2G
                    Case 3 '3 grupos
                        oReporte = New rptSIGMEActivacionMesMes3G
                    Case 4 '4 grupos
                        oReporte = New rptSIGMEActivacionMesMes4G
                    Case Else
                        oReporte = New rptSIGMEActivacionMesMes0G
                End Select

                str = SeleccionSIGMEActivacion(myConn, lblInfo, txtOrdenDesde.Text, txtOrdenHasta.Text, "codart", txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, txtCategoriaDesde.Text, txtCategoriaHasta.Text, _
                                                            txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text, txtDivisionHasta.Text, txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text, txtZonaDesde.Text, txtZonaHasta.Text, _
                                                            txtRutaDesde.Text, txtRutaHasta.Text, txtAsesorRDesde.Text, txtAsesorRHasta.Text, txtPais.Text, txtEstado.Text, txtCiudad.Text, txtMunicipio.Text, txtParroquia.Text, txtBarrio.Text, _
                                                             , , , , CamposEnGrupos())


            Case Else
                oReporte = Nothing
        End Select

        If nTabla <> "" Then
            dsSIGME = DataSetRequery(dsSIGME, str, myConn, nTabla, lblInfo)
            If dsSIGME.Tables(nTabla).Rows.Count > 0 Then
                oReporte = PresentaReporte(oReporte, dsSIGME, nTabla)
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

        r.Close()
        r.Dispose()
        r = Nothing
        oReporte.Close()
        oReporte.Dispose()
        oReporte = Nothing

    End Sub
    Private Function CamposEnGrupos() As String()

        Dim CamposEn As String() = {}


        Select Case cmbMERAgrupadoPor.SelectedIndex.ToString & cmbVenAgrupadoPor.SelectedIndex.ToString
            Case "10", "20", "30", "40", "50"
                ReDim CamposEn(1)
                CamposEn(0) = aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex)
            Case "01", "02", "03", "04", "05", "014"
                ReDim CamposEn(1)
                CamposEn(0) = aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex)
            Case "60", "80", "90", "110", "120", "150"
                ReDim CamposEn(2)
                Dim aFld() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                CamposEn(0) = Trim(aFld(0))
                CamposEn(1) = Trim(aFld(1))
            Case "06", "07", "08", "09", "011", "012"
                ReDim CamposEn(2)
                Dim aFld() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                CamposEn(0) = Trim(aFld(0))
                CamposEn(1) = Trim(aFld(1))
            Case "11", "12", "13", "14", "15", "114", "21", "22", "23", "24", "25", "214", _
                "31", "32", "33", "34", "35", "314", "41", "42", "43", "44", "45", "414", _
                "51", "52", "53", "54", "55", "514"
                ReDim CamposEn(2)
                CamposEn(0) = (aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex))
                CamposEn(1) = (aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex))

            Case "70", "100", "140"
                ReDim CamposEn(3)
                Dim aFld() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                CamposEn(0) = Trim(aFld(0))
                CamposEn(1) = Trim(aFld(1))
                CamposEn(2) = Trim(aFld(2))
            Case "010", "013"
                ReDim CamposEn(3)
                Dim aFld() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                CamposEn(0) = Trim(aFld(0))
                CamposEn(1) = Trim(aFld(1))
                CamposEn(2) = Trim(aFld(2))
            Case "61", "81", "91", "111", "121", "62", "82", "92", "112", "122", _
                 "63", "83", "93", "113", "123", "64", "84", "94", "114", "124", _
                 "65", "85", "95", "115", "125", "614", "814", "914", "1114", "1214"
                ReDim CamposEn(3)
                Dim aFld() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                CamposEn(0) = Trim(aFld(0))
                CamposEn(1) = Trim(aFld(1))
                CamposEn(2) = aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex)
            Case "16", "17", "18", "19", "111", "112", "26", "27", "28", "29", "211", "212", _
                "36", "37", "38", "39", "311", "312", "46", "47", "48", "49", "411", "412", _
                "56", "57", "58", "59", "511", "512"
                ReDim CamposEn(3)
                Dim aFld() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                CamposEn(0) = aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex)
                CamposEn(1) = Trim(aFld(0))
                CamposEn(2) = Trim(aFld(1))
            Case "130"
                ReDim CamposEn(4)
                Dim aFld() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                CamposEn(0) = Trim(aFld(0))
                CamposEn(1) = Trim(aFld(1))
                CamposEn(2) = Trim(aFld(2))
                CamposEn(3) = Trim(aFld(3))
            Case "66", "67", "68", "69", "611", "612", "86", "87", "88", "89", "811", "812", _
                    "96", "97", "98", "99", "911", "912", "116", "117", "118", "119", "1111", "1112", _
                    "126", "127", "128", "129", "1211", "1212"
                ReDim CamposEn(4)
                Dim aFld1() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                Dim aFld2() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                CamposEn(0) = Trim(aFld1(0))
                CamposEn(1) = Trim(aFld1(1))
                CamposEn(2) = Trim(aFld2(0))
                CamposEn(3) = Trim(aFld2(1))

            Case "71", "72", "73", "74", "75", "714", "101", "102", "103", "104", "105", "1014", "141", "142", "143", "144", "145", "1414"
                ReDim CamposEn(4)
                Dim aFld1() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                CamposEn(0) = Trim(aFld1(0))
                CamposEn(1) = Trim(aFld1(1))
                CamposEn(2) = Trim(aFld1(2))
                CamposEn(3) = aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex)
            Case "110", "210", "310", "410", "510", "113", "213", "313", "413", "513"
                ReDim CamposEn(4)
                Dim aFld1() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                CamposEn(0) = aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex)
                CamposEn(1) = Trim(aFld1(0))
                CamposEn(2) = Trim(aFld1(1))
                CamposEn(3) = Trim(aFld1(2))
            Case "131", "132", "133", "134", "135", "1314" '5grupos 4X1
                ReDim CamposEn(5)
                Dim aFld1() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                CamposEn(0) = Trim(aFld1(0))
                CamposEn(1) = Trim(aFld1(1))
                CamposEn(2) = Trim(aFld1(2))
                CamposEn(3) = Trim(aFld1(3))
                CamposEn(4) = aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex)
            Case "106", "107", "108", "109", "1011", "1012", "146", "147", "148", "149", "1411", "1412" '5Grupos 3X2
                ReDim CamposEn(5)
                Dim aFld1() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                Dim aFld2() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                CamposEn(0) = Trim(aFld1(0))
                CamposEn(1) = Trim(aFld1(1))
                CamposEn(2) = Trim(aFld1(2))
                CamposEn(3) = Trim(aFld2(0))
                CamposEn(4) = Trim(aFld2(1))
            Case "610", "810", "910", "1110", "1210", "613", "813", "913", "1113", "1213" '5grupos 2X3
                ReDim CamposEn(5)
                Dim aFld1() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                Dim aFld2() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                CamposEn(0) = Trim(aFld1(0))
                CamposEn(1) = Trim(aFld1(1))
                CamposEn(2) = Trim(aFld2(0))
                CamposEn(3) = Trim(aFld2(1))
                CamposEn(4) = Trim(aFld2(2))

            Case "136", "137", "138", "139", "1311", "1312" '6Grupos 4X2
                ReDim CamposEn(6)
                Dim aFld1() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                Dim aFld2() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                CamposEn(0) = Trim(aFld1(0))
                CamposEn(1) = Trim(aFld1(1))
                CamposEn(2) = Trim(aFld1(2))
                CamposEn(3) = Trim(aFld1(3))
                CamposEn(4) = Trim(aFld2(0))
                CamposEn(5) = Trim(aFld2(1))
            Case "710", "713", "1010", "1013", "1410", "1413" '6Grupos 3X3
                ReDim CamposEn(6)
                Dim aFld1() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                Dim aFld2() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                CamposEn(0) = Trim(aFld1(0))
                CamposEn(1) = Trim(aFld1(1))
                CamposEn(2) = Trim(aFld1(2))
                CamposEn(3) = Trim(aFld2(0))
                CamposEn(4) = Trim(aFld2(1))
                CamposEn(5) = Trim(aFld2(2))
            Case "1310", "1313" '7Grupos 4X3
                ReDim CamposEn(7)
                Dim aFld1() As String = Split(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex), ",")
                Dim aFld2() As String = Split(aVENAgrupadoPor(cmbVenAgrupadoPor.SelectedIndex), ",")
                CamposEn(0) = Trim(aFld1(0))
                CamposEn(1) = Trim(aFld1(1))
                CamposEn(2) = Trim(aFld1(2))
                CamposEn(3) = Trim(aFld1(3))
                CamposEn(4) = Trim(aFld2(0))
                CamposEn(5) = Trim(aFld2(1))
                CamposEn(6) = Trim(aFld2(2))
            Case Else
                CamposEn = {}
        End Select
        Return CamposEn
    End Function

    Private Function PresentaReporte(ByVal oReporte As CrystalDecisions.CrystalReports.Engine.ReportClass, _
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
            'CaminoImagen = My.Computer.FileSystem.CurrentDirectory & "\" & "logo" & .Item("id_emp") & ".jpg"
        End With
        dtEmpresa.Dispose()
        dtEmpresa = Nothing

        oReporte.SetDataSource(dsReport)
        oReporte.Refresh()
        oReporte.SetParameterValue("strLogo", CaminoImagen)
        oReporte.SetParameterValue("Orden", "Ordenado por : " + cmbOrdenadoPor.Text + " " + txtOrdenDesde.Text + "/" + txtOrdenHasta.Text)
        oReporte.SetParameterValue("RIF", rif)
        oReporte.SetParameterValue("Grupo", LineaGrupos)
        oReporte.SetParameterValue("Criterios", LineaCriterios)
        oReporte.SetParameterValue("Constantes", LineaConstantes)
        oReporte.SetParameterValue("Empresa", jytsistema.WorkName.TrimEnd(" "))  '+ "      R.I.F. : " & rif)
        If ReporteNumero = ReporteMedicionGerencial.cComprasComparativasMesMes Then
            oReporte.SetParameterValue("Titulo", "Compras comparativas mes a mes")
            oReporte.SetParameterValue("AñoAnterior", CStr(Year(jytsistema.sFechadeTrabajo) - 1))
            oReporte.SetParameterValue("AñoActual", CStr(Year(jytsistema.sFechadeTrabajo)))
            oReporte.SetParameterValue("Resumido", chkConsResumen.Checked)
        End If
        If ReporteNumero = ReporteMedicionGerencial.cVentasComparativasMesMes Then
            oReporte.SetParameterValue("Titulo", "Ventas comparativas mes a mes")
            oReporte.SetParameterValue("AñoAnterior", CStr(Year(jytsistema.sFechadeTrabajo) - 1))
            oReporte.SetParameterValue("AñoActual", CStr(Year(jytsistema.sFechadeTrabajo)))
            oReporte.SetParameterValue("Resumido", chkConsResumen.Checked)
        End If
        If ReporteNumero = ReporteMedicionGerencial.cActivacionClientesMesMes Then
            oReporte.SetParameterValue("Titulo", "Activacion de clientes mes a mes")
            oReporte.SetParameterValue("AñoAnterior", CStr(Year(jytsistema.sFechadeTrabajo) - 1))
            oReporte.SetParameterValue("AñoActual", CStr(Year(jytsistema.sFechadeTrabajo)))
        End If
        If ReporteNumero = ReporteMedicionGerencial.cDescuentosAsesor Or _
            ReporteNumero = ReporteMedicionGerencial.cDescuentosAsesor Then
            oReporte.SetParameterValue("ResumidoClientes", chkConsResumen.Checked)
            oReporte.SetParameterValue("ResumidoMercancias", chkResumenMercas.Checked)
        End If
        'If ReporteNumero = ReporteMercancias.cMarcas Then _
        '        oReporte.SetParameterValue("Titulo", "Marcas de mercancías")
        'If ReporteNumero = ReporteMercancias.cPreciosYEquivalencias Then _
        '       oReporte.SetParameterValue("Titulo", "Precios y equivalencias de mercancías")

        If ReporteNumero = ReporteMedicionGerencial.cGananciasPorFactura Then _
               oReporte.SetParameterValue("Resumen", CBool(chkConsResumen.Checked))

        Select Case ReporteNumero
            Case ReporteMedicionGerencial.cGananciasPorFactura, ReporteMedicionGerencial.cGananciasPorItem, _
                ReporteMedicionGerencial.cGananciasItemMesMes, ReporteMedicionGerencial.cActivacionClientesMesMes, _
                ReporteMedicionGerencial.cDescuentosAsesor, ReporteMedicionGerencial.cDescuentosAsesorMesMes

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
                    Case "11", "12", "13", "14", "15", "114", "21", "22", "23", "24", "25", "214", _
                        "31", "32", "33", "34", "35", "314", "41", "42", "43", "44", "45", "414", _
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
                    Case "61", "81", "91", "111", "121", "62", "82", "92", "112", "122", _
                         "63", "83", "93", "113", "123", "64", "84", "94", "114", "124", _
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
                    Case "16", "17", "18", "19", "111", "112", "26", "27", "28", "29", "211", "212", _
                        "36", "37", "38", "39", "311", "312", "46", "47", "48", "49", "411", "412", _
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
                    Case "66", "67", "68", "69", "611", "612", "86", "87", "88", "89", "811", "812", _
                            "96", "97", "98", "99", "911", "912", "116", "117", "118", "119", "1111", "1112", _
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

                Dim oFormula As CrystalDecisions.CrystalReports.Engine.FormulaFieldDefinition
                For Each oFormula In oReporte.DataDefinition.FormulaFields
                    Select Case oFormula.Name
                        Case "porcentajeGrupo1"
                            oFormula.Text = " iif( Sum ({dtGananciasFacturas.GANANCIA}, {dtGananciasFacturas." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "}) > 0,  100-(Sum ({dtGananciasFacturas.CosTotalDes}, {dtGananciasFacturas." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "})/Sum ({dtGananciasFacturas.VenTotalDes}, {dtGananciasFacturas." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "})*100), 0.00) "
                        Case "porcentajeGrupo2"
                            oFormula.Text = " iif( Sum ({dtGananciasFacturas.GANANCIA}, {dtGananciasFacturas." & oReporte.DataDefinition.Groups.Item(1).ConditionField.Name.ToString & "}) > 0,  100-(Sum ({dtGananciasFacturas.CosTotalDes}, {dtGananciasFacturas." & oReporte.DataDefinition.Groups.Item(1).ConditionField.Name.ToString & "})/Sum ({dtGananciasFacturas.VenTotalDes}, {dtGananciasFacturas." & oReporte.DataDefinition.Groups.Item(1).ConditionField.Name.ToString & "})*100), 0.00) "
                        Case "porcentajeGrupo3"
                            oFormula.Text = " iif( Sum ({dtGananciasFacturas.ganancia}, {dtGananciasFacturas." & oReporte.DataDefinition.Groups.Item(2).ConditionField.Name.ToString & "}) > 0,  100-(Sum ({dtGananciasFacturas.costotaldes}, {dtGananciasFacturas." & oReporte.DataDefinition.Groups.Item(2).ConditionField.Name.ToString & "})/Sum ({dtGananciasFacturas.ventotaldes}, {dtGananciasFacturas." & oReporte.DataDefinition.Groups.Item(2).ConditionField.Name.ToString & "})*100), 0.00) "
                        Case "porcentajeGrupo4"
                            oFormula.Text = " iif( Sum ({dtGananciasFacturas.ganancia}, {dtGananciasFacturas." & oReporte.DataDefinition.Groups.Item(3).ConditionField.Name.ToString & "}) > 0,  100-(Sum ({dtGananciasFacturas.costotaldes}, {dtGananciasFacturas." & oReporte.DataDefinition.Groups.Item(3).ConditionField.Name.ToString & "})/Sum ({dtGananciasFacturas.ventotaldes}, {dtGananciasFacturas." & oReporte.DataDefinition.Groups.Item(3).ConditionField.Name.ToString & "})*100), 0.00) "

                        Case "PorcentajeVentasG1"
                            oFormula.Text = " IF Sum ({dtVentasMer.VenTotal}) <> 0 then Sum ({dtVentasMer.VenTotal}, {dtVentasMer." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "})/Sum ({dtVentasMer.VenTotal})*100 else 0 "
                        Case "PorcentajeVentasG2"
                            oFormula.Text = "if Sum ({dtVentasMer.VenTotal}, {dtVentasMer." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "}) <> 0 then Sum ({dtVentasMer.VenTotal}, {dtVentasMer." & oReporte.DataDefinition.Groups.Item(1).ConditionField.Name.ToString & "})/Sum ({dtVentasMer.VenTotal}, {dtVentasMer." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "}) *100 else 0"
                    End Select
                Next
            Case Else

        End Select

        PresentaReporte = oReporte

    End Function
    Private Function GruposCantidad(ByVal CadenaGrupos As String) As Integer
        Select Case CadenaGrupos
            Case "00" ' 0 grupos
                GruposCantidad = 0
            Case "10", "20", "30", "40", "50", "01", "02", "03", "04", "05", "014"  ' 1 grupo
                GruposCantidad = 1
            Case "60", "80", "90", "110", "120", "150", "06", "07", "08", "09", "011", "012", _
                "11", "12", "13", "14", "15", "114", "21", "22", "23", "24", "25", "214", _
                "31", "32", "33", "34", "35", "314", "41", "42", "43", "44", "45", "414", _
                "51", "52", "53", "54", "55", "514" '2 grupos
                GruposCantidad = 2
            Case "70", "100", "140", "61", "81", "91", "111", "121", "62", "82", "92", "112", "122", _
                "63", "83", "93", "113", "123", "64", "84", "94", "114", "124", _
                "65", "85", "95", "115", "125", "614", "814", "914", "1114", "1214", _
                "16", "17", "18", "19", "111", "112", "26", "27", "28", "29", "211", "212", _
                "36", "37", "38", "39", "311", "312", "46", "47", "48", "49", "411", "412", _
                "56", "57", "58", "59", "511", "512", "010", "013" '3 grupos
                GruposCantidad = 3
            Case "130", "66", "67", "68", "69", "611", "612", "86", "87", "88", "89", "811", "812", _
                        "96", "97", "98", "99", "911", "912", "116", "117", "118", "119", "1111", "1112", _
                        "126", "127", "128", "129", "1211", "1212", _
                        "71", "72", "73", "74", "75", "714", _
                        "101", "102", "103", "104", "105", "1014", "141", "142", "143", "144", "145", "1414", _
                        "110", "210", "310", "410", "510", "113", "213", "313", "413", "513" '4 grupos
                GruposCantidad = 4
            Case "131", "132", "133", "134", "135", "1314", "76", "77", "78", "79", "711", "712", _
                "106", "107", "108", "109", "1011", "1012", "146", "147", "148", "149", "1411", "1412", _
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
    Private Sub btnPeriodoDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btnPeriodoDesde.Click
        txtPeriodoDesde.Text = SeleccionaFecha(CDate(txtPeriodoDesde.Text), Me, sender)
    End Sub

    Private Sub btnPeriodoHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btnPeriodoHasta.Click
        txtPeriodoHasta.Text = SeleccionaFecha(CDate(txtPeriodoHasta.Text), Me, sender)
    End Sub

    Private Sub btnLimpiar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLimpiar.Click
        LimpiarOrden()
        LimpiarGrupos()
        LimpiarGruposVentas()
        LimpiarGruposCompras()
        LimpiarCriterios()
    End Sub
    Private Sub LimpiarGrupos()
        LimpiarTextos(txtCategoriaDesde, txtCategoriaHasta, txtCodjer1, txtCodjer2, txtCodjer3, txtCodjer4, _
            txtCodjer5, txtCodjer6, txtDivisionDesde, txtDivisionHasta, txtMarcaDesde, txtMarcaHasta)
    End Sub
    Private Sub LimpiarGruposVentas()
        LimpiarTextos(txtCanalDesde, txtCanalHasta, txtTipoNegocioDesde, txtTipoNegocioHasta, txtZonaDesde, txtZonaHasta, _
            txtRutaDesde, txtRutaHasta, txtPais, txtEstado, txtMunicipio, txtParroquia, txtCiudad, txtBarrio, _
            txtAsesorRDesde, txtAsesorRHasta)
    End Sub
    Private Sub LimpiarGruposCompras()
        LimpiarTextos(txtCategoriaProveedorDesde, txtCategoriaProveedorHasta, txtUnidadProveedorDesde, txtUnidadProveedorHasta)
    End Sub
    Private Sub LimpiarOrden()
        LimpiarTextos(txtOrdenDesde, txtOrdenHasta)
    End Sub
    Private Sub LimpiarCriterios()
        LimpiarTextos(txtAsesorDesde, txtAsesorHasta, txtAlmacenDesde, txtAlmacenHasta, txtProvCliDesde, _
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
        If lblPeriodo.Visible Then LineaCriterios += "Período: " & IIf(lblPeriodo.Visible, txtPeriodoDesde.Text, "") & IIf(lblPeriodo.Visible AndAlso lblPeriodoHasta.Visible, "/", "") & IIf(lblPeriodoHasta.Visible, txtPeriodoHasta.Text, "")
        If LineaCriterios <> "" Then LineaCriterios += " - "
        If lblAsesor.Visible Then LineaCriterios += " Asesor : " + txtAsesorDesde.Text + "/" + txtAsesorHasta.Text
    End Function
    Private Function LineaConstantes() As String
        LineaConstantes = ""
        If lblConsResumen.Visible Then LineaConstantes += "Resumido : " + IIf(chkConsResumen.Checked, "Si", "No")
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblTarifa.Visible Then LineaConstantes += "Tarifas : " + IIf(chkPrecioA.Checked, " A", "") + IIf(chkPrecioB.Checked, " B", "") + IIf(chkPrecioC.Checked, " C", "") + IIf(chkPrecioD.Checked, " D", "") + IIf(chkPrecioE.Checked, " E", "") + IIf(chkPrecioF.Checked, " F", "")
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblPeso.Visible Then LineaConstantes += "Mercancias de peso solamente : " + IIf(chkPeso.Checked, "Si", "No")
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblCartera.Visible Then LineaConstantes += "Cartera : " & cmbCartera.Text
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblExistencias.Visible Then LineaConstantes += "Existencias : " & cmbExistencias.Text
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblTipo.Visible Then LineaConstantes += "Tipo de mercancias : " & cmbTipo.Text
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblRegulada.Visible Then LineaConstantes += "Regulada : " & cmbRegulada.Text
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblEstatus.Visible Then LineaConstantes += "Estatus : " & cmbEstatus.Text
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblTipoReporte.Visible Then LineaConstantes += "Tipo Reporte : " & cmbTipoReporte.Text

    End Function
    Private Sub btnOrdenDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrdenDesde.Click
        txtDeOrden(txtOrdenDesde)
    End Sub

    Private Sub btnOrdenHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrdenHasta.Click
        txtDeOrden(txtOrdenHasta)
    End Sub
    Private Sub txtDeOrden(ByVal txt As TextBox)
        Select Case vOrdenCampos(cmbOrdenadoPor.SelectedIndex)
            Case "CODART", "codart", "item"
                txt.Text = CargarTablaSimple(myConn, lblInfo, ds, "select codart codigo, nomart descripcion from jsmerctainv where id_emp = '" & jytsistema.WorkID & "' order by codart ", "MERCANCIAS", txt.Text)
            Case "NUMDOC"
                If ReporteMedicionGerencial.cGananciasPorFactura Then

                End If
        End Select

    End Sub

    Private Sub txtOrdenDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOrdenDesde.TextChanged
        txtOrdenHasta.Text = txtOrdenDesde.Text
    End Sub
    Private Sub ColocarGrupos()
        grpGrupos.Text = "Agrupago por    " & "Mercancías : " & cmbMERAgrupadoPor.Text & "   /   Ventas : " & cmbVenAgrupadoPor.Text & " "
    End Sub

    Private Sub cmbMERAgrupadorPor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMERAgrupadoPor.SelectedIndexChanged

        ft.visualizarObjetos(False, lblGrupoDesde, lblGrupoHasta, lblCategoria, lblMarcas, lblDivisiones, lblJerarquias, _
                            txtCategoriaDesde, btnCategoriaDesde, txtCategoriaHasta, btnCategoriaHasta, _
                            txtMarcaDesde, btnMarcaDesde, txtMarcaHasta, btnMarcaHasta, _
                            txtDivisionDesde, btnDivisionDesde, txtDivisionHasta, btnDivisionHasta, _
                            txtTipoJerarquia, btnTipoJerarquia, txtCodjer1, btnCodjer1, txtCodjer2, btnCodjer2, _
                            txtCodjer3, btnCodjer3, txtCodjer4, btnCodjer4, txtCodjer5, btnCodjer5, txtCodjer6, btnCodjer6)
        LimpiarGrupos()
        Select Case cmbMERAgrupadoPor.Text
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
            Case "División & Categorías &  Marcas"
                VerDivisiones()
                VerCategorias()
                VerMarcas()
            Case "Categorías & Jerarquías"
                VerCategorias()
                VerJerarquias()
            Case "División & Jerarquía"
                VerDivisiones()
                VerJerarquias()
            Case "Mix & Categorías & Marcas"
                VerCategorias()
                VerMarcas()
            Case "Mix & Jerarquías"
                VerJerarquias()
            Case "Mix & División"
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

        ft.visualizarObjetos(False, Label7, Label8, Label9, Label11, Label1, Label2, Label3, Label4, Label5, Label6, _
                          txtCanalDesde, btnCanalDesde, txtCanalHasta, btnCanalHasta, _
                          txtTipoNegocioDesde, btnTipoNegocioDesde, txtTipoNegocioHasta, btnTipoNegocioHasta, _
                          txtZonaDesde, btnZonaDesde, txtZonaHasta, btnZonaHasta, _
                          txtRutaDesde, btnRutaDesde, txtRutaHasta, btnRutaHasta, _
                          txtAsesorRDesde, btnAsesorRDesde, txtAsesorRHasta, btnAsesorRHasta, _
                          txtPais, txtEstado, txtMunicipio, txtParroquia, txtCiudad, txtBarrio, _
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
        ft.visualizarObjetos(True, lblJerarquias, txtTipoJerarquia, btnTipoJerarquia, txtCodjer1, btnCodjer1, _
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
        Dim f As New jsControlArcDivisiones
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        txtDivisionDesde.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub btnDivisionHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDivisionHasta.Click
        Dim f As New jsControlArcDivisiones
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        txtDivisionHasta.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub btnTipoJerarquia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoJerarquia.Click
        txtTipoJerarquia.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT tipjer codigo, descrip descripcion FROM jsmerencjer WHERE  id_emp  = '" & jytsistema.WorkID & "' order by 1 ", " Tipo de Jerarquía", _
                                                  txtTipoJerarquia.Text)
    End Sub
    Private Sub CargarJerarquia(ByVal MyConn As MySqlConnection, ByVal ds As DataSet, ByVal TipoJerarquia As String, ByVal Nivel As Integer, _
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
        txtAsesorDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT codven codigo, concat(apellidos, ', ', nombres) descripcion FROM jsvencatven WHERE tipo = '" & TipoVendedor.iFuerzaventa & "'  and estatus = 1 and id_emp  = '" & jytsistema.WorkID & "' order by 1 ", "Asesores Comerciales", _
                                                txtAsesorDesde.Text)
    End Sub
    Private Sub btnAsesorHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesorHasta.Click
        txtAsesorHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT codven codigo, concat(apellidos, ', ', nombres) descripcion FROM jsvencatven WHERE tipo = '" & TipoVendedor.iFuerzaventa & "'  and estatus = 1 and id_emp  = '" & jytsistema.WorkID & "' order by 1 ", "Asesores Comerciales", _
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
        If ReporteNumero = ReporteMedicionGerencial.cComprasComparativasMesMes Then
            txtProvCliDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codpro codigo, nombre descripcion from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Proveedores", _
                                                     txtProvCliDesde.Text)
        Else
            txtProvCliDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcli codigo, nombre descripcion from jsvencatcli where estatus < 3 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Clientes", _
                                                     txtProvCliDesde.Text)
        End If
    End Sub
    Private Sub btnProvCliHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProvCliHasta.Click
        If ReporteNumero = ReporteMedicionGerencial.cComprasComparativasMesMes Then
            txtProvCliHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codpro codigo, nombre descripcion from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Proveedores", _
                                                             txtProvCliHasta.Text)
        Else
            txtProvCliHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcli codigo, nombre descripcion from jsvencatcli where estatus < 3 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Clientes", _
                                                txtProvCliHasta.Text)
        End If
    End Sub

    Private Sub txtProvCliDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProvCliDesde.TextChanged
        txtProvCliHasta.Text = txtProvCliDesde.Text
    End Sub

    Private Sub btnCanalDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCanalDesde.Click
        txtCanalDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsvenliscan where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Canal de Distribución", _
                                               txtCanalDesde.Text)
    End Sub

    Private Sub btnCanalHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCanalHasta.Click
        txtCanalHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsvenliscan where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Canal de Distribución", _
                                               txtCanalHasta.Text)
    End Sub

    Private Sub btnTipoNegocioDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoNegocioDesde.Click
        txtTipoNegocioDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsvenlistip where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Tipo de Negocio", _
                                                     txtTipoNegocioDesde.Text)
    End Sub

    Private Sub btnTipoNegocioHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoNegocioHasta.Click
        txtTipoNegocioHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsvenlistip where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Tipo de Negocio", _
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
        txtRutaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT codrut codigo, nomrut descripcion FROM jsvenencrut WHERE tipo = 0 and id_emp  = '" & jytsistema.WorkID & "' order by 1 ", "Rutas de Visita", _
                                              txtRutaDesde.Text)
    End Sub

    Private Sub btnRutaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRutaHasta.Click
        txtRutaHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT codrut codigo, nomrut descripcion FROM jsvenencrut WHERE tipo = 0 and id_emp  = '" & jytsistema.WorkID & "' order by 1 ", "Rutas de Visita", _
                                              txtRutaHasta.Text)
    End Sub

    Private Sub btnAsesorRDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesorRDesde.Click
        txtAsesorRDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT codven codigo, concat(apellidos, ', ', nombres) descripcion FROM jsvencatven WHERE tipo = '" & TipoVendedor.iFuerzaventa & "' and estatus = 1 and clase = 0 and id_emp  = '" & jytsistema.WorkID & "' order by 1 ", "Asesores Comerciales", _
                                                 txtAsesorRDesde.Text)
    End Sub

    Private Sub btnAsesorRHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesorRHasta.Click
        txtAsesorRHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT codven codigo, concat(apellidos, ', ', nombres) descripcion FROM jsvencatven WHERE tipo = '" & TipoVendedor.iFuerzaventa & "' and estatus = 1 and clase = 0 and id_emp  = '" & jytsistema.WorkID & "' order by 1 ", "Asesores Comerciales", _
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
        txtPais.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1", "Paises", _
                                         txtPais.Text)
    End Sub

    Private Sub btnEstado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEstado.Click
        If txtPais.Text <> "" Then _
            txtEstado.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 1 and antecesor = " & txtPais.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Estados", _
                txtEstado.Text)

    End Sub

    Private Sub btnMunicipio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMunicipio.Click
        If txtEstado.Text <> "" Then _
            txtMunicipio.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 2 and antecesor = " & txtEstado.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Municipios", _
                txtMunicipio.Text)

    End Sub

    Private Sub btnParroquia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnParroquia.Click
        If txtMunicipio.Text <> "" Then _
            txtParroquia.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 2 and antecesor = " & txtMunicipio.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Parroquias", _
                txtParroquia.Text)
    End Sub

    Private Sub btnCiudad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCiudad.Click
        If txtParroquia.Text <> "" Then _
                    txtCiudad.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 3 and antecesor = " & txtParroquia.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Ciudades", _
                        txtCiudad.Text)
    End Sub

    Private Sub btnBarrio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBarrio.Click
        If txtCiudad.Text <> "" Then _
                    txtBarrio.Text = CargarTablaSimple(myConn, lblInfo, ds, " select concat(codigo) codigo, nombre descripcion from jsconcatter where tipo = 4 and antecesor = " & txtCiudad.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Barrios", _
                        txtBarrio.Text)
    End Sub

    Private Sub btnCategoriaProveedorDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoriaProveedorDesde.Click
        txtCategoriaProveedorDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion rom jsproliscat where id_emp = '" & jytsistema.WorkID & "' order by codigo  ", "Categorías", txtCategoriaProveedorDesde.Text)
    End Sub

    Private Sub btnCategoriaProveedorHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoriaProveedorHasta.Click
        txtCategoriaProveedorHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion rom jsproliscat where id_emp = '" & jytsistema.WorkID & "' order by codigo  ", "Categorías", txtCategoriaProveedorHasta.Text)
    End Sub

    Private Sub btnUnidadProveedorDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnidadProveedorDesde.Click
        txtUnidadProveedorDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion rom jsprolisuni where id_emp = '" & jytsistema.WorkID & "' order by codigo  ", "Unidades de Negocio", txtUnidadProveedorDesde.Text)
    End Sub

    Private Sub btnUNidadProveedorHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUNidadProveedorHasta.Click
        txtUnidadProveedorHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion rom jsprolisuni where id_emp = '" & jytsistema.WorkID & "' order by codigo  ", "Unidades de Negocio", txtUnidadProveedorHasta.Text)
    End Sub

    Private Sub cmbCOMAgrupadoPor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCOMAgrupadoPor.SelectedIndexChanged
        ft.visualizarObjetos(False, lblGrupoProveedorDesde, lblGrupoProveedorHasta, lblCategoriaProveedor, lblUnidadProveedor, _
                           txtCategoriaProveedorDesde, btnCategoriaProveedorDesde, txtCategoriaProveedorHasta, btnCategoriaProveedorHasta, _
                           txtUnidadProveedorDesde, btnUnidadProveedorDesde, txtUnidadProveedorHasta, btnUNidadProveedorHasta)
        LimpiarGrupos()
        Select Case cmbCOMAgrupadoPor.SelectedIndex
            Case 0
            Case 1
                VerCategoriasProveedor()
            Case 2
                VerUnidadProveedor()
            Case 3 '"Categorías & Unidad"
                VerCategoriasProveedor()
                VerUnidadProveedor()
            Case Else
                VerCategoriasProveedor()
                VerUnidadProveedor()
        End Select
    End Sub
    Private Sub VerCategoriasProveedor()
        ft.visualizarObjetos(True, lblGrupoProveedorDesde, lblGrupoProveedorHasta)
        ft.visualizarObjetos(True, lblCategoriaProveedor, txtCategoriaProveedorDesde, btnCategoriaProveedorDesde, txtCategoriaProveedorHasta, btnCategoriaProveedorHasta)
        ft.habilitarObjetos(False, True, txtCategoriaProveedorDesde, txtCategoriaProveedorHasta)
    End Sub
    Private Sub VerUnidadProveedor()
        ft.visualizarObjetos(True, lblGrupoProveedorDesde, lblGrupoProveedorHasta)
        ft.visualizarObjetos(True, lblUnidadProveedor, txtUnidadProveedorDesde, btnUnidadProveedorDesde, txtUnidadProveedorHasta, btnUNidadProveedorHasta)
        ft.habilitarObjetos(False, True, txtUnidadProveedorDesde, txtUnidadProveedorHasta)
    End Sub

    Private Sub txtPeriodoDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtPeriodoDesde.TextChanged
        Select Case PeriodoTipo
            Case TipoPeriodo.iDiario
                txtPeriodoHasta.Text = txtPeriodoDesde.Text
            Case TipoPeriodo.iSemanal
                txtPeriodoHasta.Text = ft.FormatoFecha(DateAdd(DateInterval.Day, 7, CDate(txtPeriodoDesde.Text)))
            Case TipoPeriodo.iMensual
                txtPeriodoHasta.Text = ft.FormatoFecha(UltimoDiaMes(CDate(txtPeriodoDesde.Text)))
            Case TipoPeriodo.iAnual
                txtPeriodoHasta.Text = ft.FormatoFecha(UltimoDiaAño(CDate(txtPeriodoDesde.Text)))
        End Select
    End Sub
End Class