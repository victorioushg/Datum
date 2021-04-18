Imports MySql.Data.MySqlClient
Imports System.IO
Imports ReportesDeVentas
Public Class jsVenRepParametrosDos

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

    Private vCXCAgrupadoPor() As String = {"Ninguno", "Canal", "Tipo", "Zona", "Ruta", "Asesor Comercial", "Territorio", "Canal & Tipo Negocio", "Zona & Ruta", "Asesor & Canal & Tipo Negocio", "Asesor & Zona & Ruta"}
    Private aCXCAgrupadoPor() As String = {"", "canal", "tiponegocio", "zona", "ruta", "asesor", "territorio", "canal, tiponegocio", _
                                           "zona, ruta", "asesor, canal, tiponegocio", "asesor, zona, ruta"}

    Private vCXCAgrupadoPorDos() As String = {"Ninguno", "Asesor Comercial"}
    Private aCXCAgrupadoPorDos() As String = {"", "asesor"}

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

    Private aSiNoTodos() As String = {"Si", "No", "Todos"}

    Private aEstatusFacturas() As String = {"Por confirmar", "Confirmadas", "Anuladas", "Todas"}
    Private aCondicionPago() As String = {"Crédito", "Contado", "Todos"}
    Private aEstatus() As String = {"Activo", "Bloqueado", "Inactivo", "Desincorporado", "Todos"}
    Private aTipo() As String = {"Ordinario", "Especial", "Formal", "No contribuyente", "Todos"}
    Private aTipoSemaforo() As String = {"Rojo", "Amarillo", "Verde", "Rojo & Amarillo", "Rojo & Verde", "Rojo & Amarillo & Verde"}
    Private aTipoReporte() As String = {"Unidad Venta (UV) ", "Kilogramos (UMP)", "Ventas (BsF)", "Cajas (UMS)", "Costos (BsF)"}

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


            Case ReporteVentas.cRetencionesIVAClientes
                Dim vOrdenNombres() As String = {"Código Cliente"}
                Dim vOrdenCampos() As String = {"codcli"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)

            Case ReporteVentas.cSaldosPorDocumento
                Dim vOrdenNombres() As String = {"Código Cliente"}
                Dim vOrdenCampos() As String = {"codcli"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)

            Case ReporteVentas.cCobranzaAnterior, ReporteVentas.cCobranzaActual, ReporteVentas.cCierreDiarioCT, _
                    ReporteVentas.cCobranzaPlana

                Dim vOrdenNombres() As String = {"Código Cliente"}
                Dim vOrdenCampos() As String = {"codcli"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)

            Case ReporteVentas.cVentasClienteDivision
                Dim vOrdenNombres() As String = {"Mayor Peso"}
                Dim vOrdenCampos() As String = {"prov_cli"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteVentas.cPedidosSinFacturar
                Dim vOrdenNombres() As String = {"Código Cliente"}
                Dim vOrdenCampos() As String = {"codcli"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)

           Case ReporteVentas.cPedidosVsEstatus, ReporteVentas.cPedidosVsEstatusPrepedidos
                Dim vOrdenNombres() As String = {"N° Pedido"}
                Dim vOrdenCampos() As String = {"numped"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteVentas.cBackorders
                Dim vOrdenNombres() As String = {"N° Pedido"}
                Dim vOrdenCampos() As String = {"numped"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteVentas.cRanking
                Dim vOrdenNombres() As String = {"Peso"}
                Dim vOrdenCampos() As String = {"peso"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, True, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteVentas.cComisionesPorJerarquia, ReporteVentas.cComisionesPorFacturaYJerarquia
                Dim vOrdenNombres() As String = {"Código Mercancía"}
                Dim vOrdenCampos() As String = {"CODART"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteVentas.cComisionesPorDiasCobranza
                Dim vOrdenNombres() As String = {"Código Cliente"}
                Dim vOrdenCampos() As String = {"CODCLI"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteVentas.cComisionesPorDiasCobranzaJerarquia
                Dim vOrdenNombres() As String = {"Código Cliente"}
                Dim vOrdenCampos() As String = {"CODCLI"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReporteVentas.cRutasVisita
                Dim vOrdenNombres() As String = {"Código Ruta"}
                Dim vOrdenCampos() As String = {"CODRUT"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
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
            Case ReporteVentas.cRanking, _
                ReporteVentas.cBackorders
                ft.habilitarObjetos(True, False, tabPageMercas)
        End Select

    End Sub

    Private Sub IniciarCriterios()

        VerCriterio_Periodo(False, 0)
        VerCriterio_TipoDocumento(False)
        VerCriterio_Cliente(False)
        VerCriterio_Mercancia(False)
        VerCriterio_Asesor(False)
        VerCriterio_Almacen(False)


        Select Case ReporteNumero
            Case ReporteVentas.cPedidosSinFacturar
                VerCriterio_Periodo(True, 0)
                VerCriterio_Asesor(True)
            Case ReporteVentas.cChequesDevueltosMes, _
                ReporteVentas.cRetencionesIVAClientes

                VerCriterio_Periodo(True, 0)
            Case ReporteVentas.cChequesDevueltosMes
                VerCriterio_Periodo(True, 0, TipoPeriodo.iAnual)
            Case ReporteVentas.cVentasClienteDivision
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Cliente(True)
            Case ReporteVentas.cCobranzaAnterior, ReporteVentas.cCobranzaActual
                VerCriterio_Periodo(True, 2, TipoPeriodo.iDiario)
                VerCriterio_Asesor(True)
            Case ReporteVentas.cCierreDiarioCT
                VerCriterio_Periodo(True, 0, TipoPeriodo.iDiario)
                VerCriterio_Asesor(True)
            Case ReporteVentas.cCobranzaPlana
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Asesor(True)
            Case ReporteVentas.cPedidosVsEstatus, ReporteVentas.cPedidosVsEstatusPrepedidos
                VerCriterio_Periodo(True, 0, TipoPeriodo.iDiario)
                VerCriterio_Asesor(True)
            Case ReporteVentas.cRanking, ReporteVentas.cBackorders
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Mercancia(True)
                VerCriterio_Cliente(True)
            Case ReporteVentas.cComisionesPorJerarquia, ReporteVentas.cComisionesPorFacturaYJerarquia, _
                ReporteVentas.cComisionesPorDiasCobranza, ReporteVentas.cComisionesPorDiasCobranzaJerarquia
                VerCriterio_Periodo(True, 0, TipoPeriodo.iMensual)
                VerCriterio_Asesor(True)
            Case Else

        End Select

    End Sub

    Private Sub VerCriterio_Periodo(ByVal Ver As Boolean, ByVal CompletoDesdeHasta As Integer, Optional ByVal Periodo As TipoPeriodo = TipoPeriodo.iMensual)
        'CompletoDesdeHasta 0 = Complete , 1 = Desde , 2 = Hasta 
        ft.visualizarObjetos(False, lblPeriodoDesde, lblPeriodoHasta, lblPeriodo, txtPeriodoDesde, txtPeriodoHasta, btnPeriodoDesde, btnPeriodoHasta)
        ft.habilitarObjetos(False, True, txtPeriodoDesde, txtPeriodoHasta)
        periodoTipo = Periodo
        If Ver Then
            Select Case CompletoDesdeHasta
                Case 0
                    ft.visualizarObjetos(Ver, lblPeriodoDesde, lblPeriodoHasta, lblPeriodo, txtPeriodoDesde, txtPeriodoHasta, btnPeriodoDesde, btnPeriodoHasta)
                Case 1
                    ft.visualizarObjetos(Ver, lblPeriodoDesde, lblPeriodo, txtPeriodoDesde, btnPeriodoDesde)
                Case 2
                    ft.visualizarObjetos(Ver, lblPeriodoHasta, lblPeriodo, txtPeriodoHasta, btnPeriodoHasta)
            End Select
        End If
        ft.habilitarObjetos(False, True, txtPeriodoDesde, txtPeriodoHasta)
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
        ft.visualizarObjetos(ver, lblcliente, lblcliente, txtClienteDesde, btnClienteDesde, _
                                lblClienteHasta, txtClienteHasta, btnClienteHasta)

        ft.habilitarObjetos(True, True, txtClienteDesde, txtClienteHasta)
        txtClienteDesde.MaxLength = 15
        txtClienteHasta.MaxLength = 15

    End Sub
    Private Sub VerCriterio_Asesor(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblAsesorCriterios, txtAsesorDesdeCriterios, btnAsesorDesdeCriterios, _
                                txtAsesorHastaCriterios, btnAsesorHastaCriterios)

        ft.habilitarObjetos(True, True, txtAsesorDesdeCriterios, txtAsesorHastaCriterios)
        txtAsesorDesdeCriterios.MaxLength = 5
        txtAsesorHastaCriterios.MaxLength = 5

    End Sub
    Private Sub VerCriterio_Mercancia(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblMercanciaDesde, txtMercanciaDesde, btnMercanciaDesde, _
                                lblMercanciaHasta, txtMercanciaHasta, btnMercanciaHasta)

        ft.habilitarObjetos(True, True, txtMercanciaDesde, txtMercanciaHasta)
        txtMercanciaDesde.MaxLength = 15
        txtMercanciaHasta.MaxLength = 15

    End Sub
    Private Sub VerCriterio_Almacen(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblAlmacen, txtAlmacenDesde, btnAlmacenDesde, _
                                txtAlmacenHasta, btnAlmacenHasta)

        ft.habilitarObjetos(True, True, txtAlmacenDesde, txtAlmacenHasta)
        txtAlmacenDesde.MaxLength = 5
        txtAlmacenHasta.MaxLength = 5

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
        VerConstante_ConstanteVentas(False)
        VerConstante_Primeros(False)
        VerConstante_TipoReporte(False)

        Select Case ReporteNumero

            Case ReporteVentas.cChequesDevueltosMes
                VerConstante_TipoCliente(True)
                VerConstante_Estatus(True)
            Case ReporteVentas.cVentasClienteDivision
                VerConstante_Estatus(True)
            Case ReporteVentas.cCobranzaActual, ReporteVentas.cCobranzaAnterior, _
                ReporteVentas.cCierreDiarioCT, ReporteVentas.cBackorders
                verConstante_Resumen(True)
            Case ReporteVentas.cPedidosVsEstatus, ReporteVentas.cPedidosVsEstatusPrepedidos
                VerConstante_TipoCliente(True)
                ft.RellenaCombo(aTipoSemaforo, cmbTipo, 3)
            Case ReporteVentas.cRanking
                VerConstante_Primeros(True)
            Case ReporteVentas.cSaldosPorDocumento
                Dim afTipo() As String = {"Actuales", "Históricos"}
                lblTipo.Text = "Tipo Saldo"
                ft.RellenaCombo(afTipo, cmbTipo)
                VerConstante_TipoCliente(True)
            Case ReporteVentas.cComisionesPorJerarquia, ReporteVentas.cComisionesPorFacturaYJerarquia
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
        ft.RellenaCombo(aCondicionPago, cmbCondicionPago, 2)
        ft.RellenaCombo(aSiNoTodos, cmbRegulada, 2)
        ft.RellenaCombo(aTipo, cmbTipo, 4)
        ft.RellenaCombo(aEstatus, cmbEstatus, 4)
        ft.RellenaCombo(aEstatusFacturas, cmbEstatusFacturas, 3)
        txtConstanteVentas.Text = ft.FormatoNumero(500)
        txtPrimeros.Text = ft.FormatoEntero(30)
        ft.RellenaCombo(aTipoReporte, cmbTipoReporte)


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

    Private Sub VerConstante_ConstanteVentas(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblConstanteVentas, txtConstanteVentas)
    End Sub

    Private Sub VerConstante_Primeros(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblPrimeros, txtPrimeros)
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
    Private Sub VerConstante_TipoReporte(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblTipoReporte, cmbTipoReporte)
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

    Private Sub jsVenRepParametrosDos_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
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
        Dim dsVen As New DataSet 'dsVentasDos
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
            Case ReporteVentas.cRutasVisita
                nTabla = "dtRutas"
                oReporte = New rptVentasRutas
                str = SeleccionVENRuta(CodigoCliente, 0)

            Case ReporteVentas.cComisionesPorDiasCobranzaJerarquia
                nTabla = "dtComisionesCobranzaJerarquia"
                oReporte = New rptVentasComisionesCobranza_Orlyzam
                str = SeleccionComisionesCobranzaJerarquia(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), _
                                                           txtAsesorDesdeCriterios.Text, txtAsesorHastaCriterios.Text)
            Case ReporteVentas.cComisionesPorDiasCobranza
                nTabla = "dtComisionesCobranzaAlimica"
                oReporte = New rptVentasComisionesCobranza_Alimica
                str = SeleccionVENComisionesCobranzaVencimientos(txtAsesorDesdeCriterios.Text, txtAsesorHastaCriterios.Text, _
                                                         CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text))
            Case ReporteVentas.cComisionesPorJerarquia
                nTabla = "dtComisionesJerarquia"
                oReporte = New rptVentasComisionesJerarquia
                str = SeleccionVENComisionesJerarquia(txtAsesorDesdeCriterios.Text, txtAsesorHastaCriterios.Text, _
                                                         CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text))
            Case ReporteVentas.cComisionesPorFacturaYJerarquia
                nTabla = "dtComisionesJerarquia"
                oReporte = New rptVentasComisionesJerarquiaFactura
                str = SeleccionVENComisionesJerarquia(txtAsesorDesdeCriterios.Text, txtAsesorHastaCriterios.Text, _
                                                         CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), 1)

            Case ReporteVentas.cSaldosPorDocumento
                nTabla = "dtSaldosDoc"
                oReporte = New rptVentasSaldosPorDocumento
                str = SeleccionVENSaldosPorDocumento(CodigoCliente, cmbTipo.SelectedIndex)
            Case ReporteVentas.cRetencionesIVAClientes
                nTabla = "dtRetencionesIVAClientes"
                oReporte = New rptVentasRetencionesIVAClientes
                str = SeleccionVENRetencionesIVAClientes(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text))
            Case ReporteVentas.cBackorders
                nTabla = "dtBackorders"
                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbCXCAgrupadorPor.SelectedIndex.ToString)
                    Case 0 ' 0 grupos
                        oReporte = New rptVentasBackorders0G
                    Case 1 '1 Grupo"
                        oReporte = New rptVentasBackorders1G
                    Case 2 '2 grupos
                        oReporte = New rptVentasBackorders2G
                    Case 3 '3 grupos
                        oReporte = New rptVentasBackorders3G
                    Case 4 '4 grupos
                        oReporte = New rptVentasBackorders4G
                    Case Else
                        oReporte = New rptVentasBackorders0G
                End Select
                str = SeleccionVENBackorders(myConn, lblInfo, txtOrdenDesde.Text, txtOrdenHasta.Text, CDate(txtPeriodoDesde.Text), _
                                             CDate(txtPeriodoHasta.Text), txtClienteDesde.Text, txtClienteHasta.Text, _
                                             txtMercanciaDesde.Text, txtMercanciaHasta.Text, txtCanalDesde.Text, txtCanalHasta.Text, _
                                             txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text, txtZonaDesde.Text, txtZonaHasta.Text, _
                                             txtRutaDesde.Text, txtRutaHasta.Text, txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text, _
                                             txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, txtCategoriaDesde.Text, txtCategoriaHasta.Text, _
                                             txtMarcaDesde.Text, txtMarcaHasta.Text, txtDivisionDesde.Text, txtDivisionHasta.Text, _
                                             txtTipoJerarquia.Text, txtCodjer1.Text, txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, _
                                             txtCodjer5.Text, txtCodjer6.Text)


            Case ReporteVentas.cRanking
                nTabla = "dtRanking"
                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbCXCAgrupadorPor.SelectedIndex.ToString)
                    Case 0 ' 0 grupos
                        oReporte = New rptVentasRanking0G
                    Case 1 '1 Grupo"
                        oReporte = New rptVentasRanking1G
                    Case 2 '2 grupos
                        oReporte = New rptVentasRanking2G
                    Case 3 '3 grupos
                        oReporte = New rptVentasRanking3G
                    Case 4 '4 grupos
                        oReporte = New rptVentasRanking4G
                    Case Else
                        oReporte = New rptVentasRanking0G
                End Select
                str = SeleccionVENRankingDeVentas(myConn, lblInfo, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), _
                                                  txtClienteDesde.Text, txtClienteHasta.Text, txtMercanciaDesde.Text, txtMercanciaHasta.Text, _
                                                  txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text, _
                                                  txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text, txtAsesorDesde.Text, _
                                                  txtAsesorHasta.Text, txtPais.Text, txtEstado.Text, txtMunicipio.Text, txtParroquia.Text, _
                                                  txtCiudad.Text, txtBarrio.Text, txtCategoriaDesde.Text, txtCategoriaHasta.Text, txtMarcaDesde.Text, _
                                                  txtMarcaHasta.Text, txtDivisionDesde.Text, txtDivisionHasta.Text, txtTipoJerarquia.Text, txtCodjer1.Text, _
                                                  txtCodjer2.Text, txtCodjer3.Text, txtCodjer4.Text, txtCodjer5.Text, txtCodjer6.Text, " peso ", ValorEntero(txtPrimeros.Text))



            Case ReporteVentas.cPedidosVsEstatus
                nTabla = "dtPedidosVsEstatus"
                oReporte = New rptVentasPedidosVsEstatusCliente0G
                str = SeleccionVENSEMAFORO(txtAsesorDesdeCriterios.Text, txtAsesorHastaCriterios.Text, _
                                                            CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), cmbTipo.SelectedIndex)
            Case ReporteVentas.cPedidosVsEstatusPrepedidos
                nTabla = "dtPedidosVsEstatus"
                oReporte = New rptVentasPedidosVsEstatusCliente1G
                str = SeleccionVENSEMAFOROPREPEDIDOS(txtAsesorDesdeCriterios.Text, txtAsesorHastaCriterios.Text, _
                                                            CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), cmbTipo.SelectedIndex)

            Case ReporteVentas.cCierreDiarioCT
                nTabla = "dtCierreDiarioCT"
                oReporte = New rptVentasCierreDiarioCT
                str = SeleccionVENCierreCestaTicket(myConn, lblInfo, txtAsesorDesdeCriterios.Text, txtAsesorHastaCriterios.Text, CDate(txtPeriodoDesde.Text))
            Case ReporteVentas.cCobranzaAnterior
                nTabla = "dtCobranza"
                oReporte = New rptVentasCobranza
                str = SeleccionVENCuotasCobranza(myConn, lblInfo, txtAsesorDesdeCriterios.Text, txtAsesorHastaCriterios.Text, CDate(txtPeriodoHasta.Text), 0)
            Case ReporteVentas.cCobranzaActual
                nTabla = "dtCobranza"
                oReporte = New rptVentasCobranza
                str = SeleccionVENCuotasCobranza(myConn, lblInfo, txtAsesorDesdeCriterios.Text, txtAsesorHastaCriterios.Text, CDate(txtPeriodoHasta.Text), 1)
            Case ReporteVentas.cCobranzaPlana
                nTabla = "dtCobranzaPlana"
                oReporte = New rptVentasCobranzaPlana
                str = SeleccionVENCobranzaPlana(myConn, lblInfo, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), txtAsesorDesdeCriterios.Text, txtAsesorHastaCriterios.Text)
         
            Case ReporteVentas.cPedidosSinFacturar
                nTabla = "dtPedidosSinFacturar"
                oReporte = New rptVentasPedidosSinFacturar0G
                str = SeleccionVENPedidosSinFacturar(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), _
                                                      txtOrdenDesde.Text, txtOrdenHasta.Text, txtAsesorDesdeCriterios.Text, _
                                                      txtAsesorHastaCriterios.Text)

            Case ReporteVentas.cChequesDevueltosMes
                nTabla = "dtChequesDevueltosMes"
                Select Case GruposCantidad(cmbMERAgrupadoPor.SelectedIndex.ToString & cmbCXCAgrupadorPor.SelectedIndex.ToString)
                    Case 0 ' 0 grupos
                        oReporte = New rptVentasChequesDEvueltosMes0G
                    Case 1 '1 Grupo"
                        oReporte = New rptVentasChequesDEvueltosMes1G
                    Case 2 '2 grupos
                        oReporte = New rptVentasChequesDEvueltosMes2G
                    Case 3 '3 grupos
                        oReporte = New rptVentasChequesDEvueltosMes3G
                    Case Else
                        oReporte = New rptVentasChequesDEvueltosMes0G
                End Select

                str = SeleccionVENChequesDevueltosMES(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex, _
                                            txtCanalDesde.Text, txtCanalHasta.Text, txtTipoNegocioDesde.Text, txtTipoNegocioHasta.Text, txtZonaDesde.Text, txtZonaHasta.Text, txtRutaDesde.Text, txtRutaHasta.Text, txtAsesorDesde.Text, txtAsesorHasta.Text, txtPais.Text, txtEstado.Text, txtMunicipio.Text, txtParroquia.Text, txtCiudad.Text, txtBarrio.Text, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), cmbTipo.SelectedIndex, cmbEstatus.SelectedIndex, _
                                            IIf(cmbCXCAgrupadorPor.SelectedIndex = 0, 1, _
                                                IIf(cmbCXCAgrupadorPor.SelectedIndex = 1, 2, _
                                                    IIf(cmbCXCAgrupadorPor.SelectedIndex = 2, 3, _
                                                        IIf(cmbCXCAgrupadorPor.SelectedIndex = 3, 4, _
                                                            IIf(cmbCXCAgrupadorPor.SelectedIndex = 4, 5, 6))))))


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
                'If ReporteNumero = ReporteVentas.cPlanillaCliente Then
                '    oReporte = PresentaReporte(oReporte, dsVen, nTabla)
                '    r.CrystalReportViewer1.ReportSource = oReporte
                '    r.CrystalReportViewer1.ToolPanelView = IIf(PresentaArbol, CrystalDecisions.Windows.Forms.ToolPanelViewType.GroupTree, _
                '                                  CrystalDecisions.Windows.Forms.ToolPanelViewType.None)
                '    r.CrystalReportViewer1.ShowGroupTreeButton = PresentaArbol
                '    r.CrystalReportViewer1.Zoom(1)
                '    r.CrystalReportViewer1.Refresh()
                '    r.Cargar(ReporteNombre)
                '    DeshabilitarCursorEnEspera()
                'Else
                ft.mensajeCritico("No existe información que cumpla con estos criterios y/o constantes ")
                'End If

            End If
        End If


        r.Close()
        r = Nothing
        oReporte.Close()
        oReporte = Nothing

    End Sub
    Private Function PresentaReporte(ByVal oReporte As CrystalDecisions.CrystalReports.Engine.ReportClass, _
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

        'Select Case ReporteNumero
        '    Case ReporteVentas.cClientes
        '        oReporte.ReportDefinition.Sections("DetailSection3").SectionFormat.EnableSuppress = chkConsResumen.Checked
        '    Case ReporteVentas.cPresupuesto, ReporteVentas.cPrePedido, ReporteVentas.cPedido, ReporteVentas.cNotaDeEntrega, _
        '        ReporteVentas.cFactura, ReporteVentas.cNotaDebito, ReporteVentas.cNotaCredito
        '        oReporte.Subreports("rptGENIVA.rpt").SetDataSource(ds.Tables("dtIVA"))
        '        If ReporteNumero <> ReporteVentas.cNotaCredito And ReporteNumero <> ReporteVentas.cNotaDebito Then _
        '            oReporte.Subreports("rptGENDescuentos.rpt").SetDataSource(ds.Tables("dtDescuentos"))
        '        oReporte.Subreports("rptGENComentarios.rpt").SetDataSource(ds.Tables("dtComentarios"))
        '    Case ReporteVentas.cCxC
        '        oReporte.Subreports("rptGENFormaDePago.rpt").SetDataSource(ds.Tables("dtCXCFormaPago"))
        '        oReporte.Subreports("rptGENRelacionCT.rpt").SetDataSource(ds.Tables("dtCXCRelacionCT"))
        'End Select

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

            Case ReporteVentas.cPedidosSinFacturar, ReporteVentas.cChequesDevueltosMes, _
             ReporteVentas.cVentasClienteDivision, ReporteVentas.cCobranzaActual, _
                ReporteVentas.cCobranzaAnterior, ReporteVentas.cRanking, ReporteVentas.cBackorders, _
                ReporteVentas.cPedidosVsEstatus, ReporteVentas.cCobranzaPlana, ReporteVentas.cComisionesPorJerarquia, _
                ReporteVentas.cComisionesPorFacturaYJerarquia

                If ReporteNumero = ReporteVentas.cBackorders _
                    Or ReporteNumero = ReporteVentas.cComisionesPorJerarquia _
                    Or ReporteNumero = ReporteVentas.cComisionesPorFacturaYJerarquia _
                    Then oReporte.SetParameterValue("Resumido", CBool(chkConsResumen.Checked))

                If ReporteNumero = ReporteVentas.cVentasClienteDivision Then
                    oReporte.SetParameterValue("Division1", ft.DevuelveScalarCadena(myConn, "select descrip from jsmercatdiv where division = '00001' and id_emp = '" & jytsistema.WorkID & "' ").ToString)
                    oReporte.SetParameterValue("Division2", ft.DevuelveScalarCadena(myConn, "select descrip from jsmercatdiv where division = '00002' and id_emp = '" & jytsistema.WorkID & "' ").ToString)
                    oReporte.SetParameterValue("Division3", ft.DevuelveScalarCadena(myConn, "select descrip from jsmercatdiv where division = '00003' and id_emp = '" & jytsistema.WorkID & "' ").ToString)
                    oReporte.SetParameterValue("Division4", ft.DevuelveScalarCadena(myConn, "select descrip from jsmercatdiv where division = '00004' and id_emp = '" & jytsistema.WorkID & "' ").ToString)
                    oReporte.SetParameterValue("Division5", ft.DevuelveScalarCadena(myConn, "select descrip from jsmercatdiv where division = '00005' and id_emp = '" & jytsistema.WorkID & "' ").ToString)
                    oReporte.SetParameterValue("Division6", ft.DevuelveScalarCadena(myConn, "select descrip from jsmercatdiv where division = '00006' and id_emp = '" & jytsistema.WorkID & "' ").ToString)
                    oReporte.SetParameterValue("Division7", ft.DevuelveScalarCadena(myConn, "select descrip from jsmercatdiv where division = '00007' and id_emp = '" & jytsistema.WorkID & "' ").ToString)
                End If
                If ReporteNumero = ReporteVentas.cEstadoDeCuentas Or _
                    ReporteNumero = ReporteVentas.cMovimientosDocumento Then _
                    oReporte.SetParameterValue("SaldoAl", "Saldo al " & ft.FormatoFecha(DateAdd("d", -1, CDate(txtPeriodoDesde.Text))))

                If ReporteNumero = ReporteVentas.cCobranzaAnterior Then _
                        oReporte.SetParameterValue("Titulo", "Cobranza del mes ocurrida sobre meses anteriores")
                If ReporteNumero = ReporteVentas.cCobranzaActual Then _
                       oReporte.SetParameterValue("Titulo", "Cobranza del mes ocurrida sobre el mes actual")

                If ReporteNumero = ReporteVentas.cListadoPresupuestos Then _
                        oReporte.SetParameterValue("Titulo", "Presupuestos")

                If ReporteNumero = ReporteVentas.cListadoPrePedidos Then _
                        oReporte.SetParameterValue("Titulo", "Pre-Pedidos")

                If ReporteNumero = ReporteVentas.cListadoPedidos Then _
                        oReporte.SetParameterValue("Titulo", "Pedidos")

                If ReporteNumero = ReporteVentas.cListadoNotasDeEntrega Then _
                        oReporte.SetParameterValue("Titulo", "Notas de Entrega")

                If ReporteNumero = ReporteVentas.cListadoNotasDeCredito Then _
                        oReporte.SetParameterValue("Titulo", "Notas de Crédito")

                If ReporteNumero = ReporteVentas.cListadoNotasDebito Then _
                        oReporte.SetParameterValue("Titulo", "Notas Débito")

                Select Case cmbMERAgrupadoPor.SelectedIndex.ToString & cmbCXCAgrupadorPor.SelectedIndex.ToString
                    Case "10", "20", "30", "40", "50"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        Dim FieldDef1 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(aMERAgrupadoPor(cmbMERAgrupadoPor.SelectedIndex))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                    Case "01", "02", "03", "04", "05", "014"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        Dim FieldDef1 As CrystalDecisions.CrystalReports.Engine.FieldDefinition
                        FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex))
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
                        Dim aFld() As String = Split(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex), ",")
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
                        FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex))
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
                        Dim aFld() As String = Split(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex), ",")
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
                        FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                    Case "16", "17", "18", "19", "111", "112", "26", "27", "28", "29", "211", "212", _
                        "36", "37", "38", "39", "311", "312", "46", "47", "48", "49", "411", "412", _
                        "56", "57", "58", "59", "511", "512"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        oReporte.SetParameterValue("Grupo3", "tipjer")
                        Dim aFld() As String = Split(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex), ",")
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
                        Dim aFld2() As String = Split(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex), ",")
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
                        FieldDef4 = oReporte.Database.Tables.Item(0).Fields.Item(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
                        oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
                        oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
                        oReporte.DataDefinition.Groups.Item(3).ConditionField = FieldDef4
                    Case "110", "210", "310", "410", "510", "113", "213", "313", "413", "513"
                        oReporte.SetParameterValue("Grupo1", "categoria")
                        oReporte.SetParameterValue("Grupo2", "marca")
                        oReporte.SetParameterValue("Grupo3", "tipjer")
                        oReporte.SetParameterValue("Grupo4", "division")
                        Dim aFld1() As String = Split(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex), ",")
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
                        FieldDef5 = oReporte.Database.Tables.Item(0).Fields.Item(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex))
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
                        Dim aFld2() As String = Split(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex), ",")
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
                        Dim aFld2() As String = Split(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex), ",")
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
                        Dim aFld2() As String = Split(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex), ",")
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
                        Dim aFld2() As String = Split(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex), ",")
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
                        Dim aFld2() As String = Split(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex), ",")
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
                        Case "NoFacturadoG1"
                            oFormula.Text = " Sum ({dtBackorders.totalrenglon}, {dtBackorders." & oReporte.DataDefinition.Groups.Item(0).ConditionField.Name.ToString & "})/{#RTotal5} "
                        Case "NoFacturadoG2"
                            oFormula.Text = " Sum ({dtBackorders.totalrenglon}, {dtBackorders." & oReporte.DataDefinition.Groups.Item(1).ConditionField.Name.ToString & "})/{#RTotal10} "
                        Case "NoFacturadoG3"
                            oFormula.Text = " Sum ({dtBackorders.totalrenglon}, {dtBackorders." & oReporte.DataDefinition.Groups.Item(2).ConditionField.Name.ToString & "})/{#RTotal15} "
                        Case "NoFacturadoG4"
                            oFormula.Text = " Sum ({dtBackorders.totalrenglon}, {dtBackorders." & oReporte.DataDefinition.Groups.Item(3).ConditionField.Name.ToString & "})/{#RTotal20} "
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



        End Select

        PresentaReporte = oReporte


    End Function


    Private Function GruposCantidad(ByVal CadenaGrupos As String) As Integer
        Select Case CadenaGrupos
            Case "00" ' 0 grupos
                GruposCantidad = 0
            Case "01", "02", "03", "04", "05", "06", "10", "20", "30", "40"  ' 1 grupo
                GruposCantidad = 1
            Case "11", "12", "13", "14", "15", "16", "21", "22", "23", "24", "25", "26", "31", "32", "33", _
                "34", "35", "36", "41", "42", "43", "44", "45", "46", "51", "52", "53", "54", "55", "56", _
                "07", "08", "60", "80", "90", "110", "120" '2 grupos
                GruposCantidad = 2
            Case "70", "100", "140", "61", "62", "63", "64", "65", "66", "81", "82", "83", "84", "85", "86", _
                "91", "92", "93", "94", "95", "96", "111", "112", "113", "114", "115", "116", "121", "122", _
                "123", "124", "125", "126", "17", "18", "27", "28", "37", "38", "47", "48", "57", "58", "09", "010" '3 grupos
                GruposCantidad = 3
            Case "130", "71", "72", "73", "75", "76", "101", "102", "103", "104", "105", "106", _
                "141", "142", "143", "144", "145", "146", "67", "68", "87", "88", "97", "98", _
                "117", "118", "127", "128", "19", "110", "29", "210", "39", "310", "49", "410", _
                "59", "510" '4 grupos
                GruposCantidad = 4
            Case Else
                GruposCantidad = 0
        End Select
    End Function


    Private Sub btnPeriodoDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btnPeriodoDesde.Click
        txtPeriodoDesde.Text = SeleccionaFecha(CDate(txtPeriodoDesde.Text), Me, grpCriterios, sender)
    End Sub

    Private Sub btnPeriodoHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btnPeriodoHasta.Click
        txtPeriodoHasta.Text = SeleccionaFecha(CDate(txtPeriodoHasta.Text), Me, grpCriterios, sender)
    End Sub

    Private Sub btnLimpiar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLimpiar.Click
        LimpiarOrden()
        LimpiarGrupos()
    End Sub
    Private Sub LimpiarGrupos()
        LimpiarTextos(txtCanalDesde, txtCanalHasta, txtRutaDesde, txtRutaHasta, txtAsesorDesde, txtPais, txtAsesorHasta, _
            txtEstado, txtMunicipio, txtZonaDesde, txtZonaHasta, txtTipoNegocioDesde, txtTipoNegocioHasta)
    End Sub
    Private Sub LimpiarOrden()
        LimpiarTextos(txtOrdenDesde, txtOrdenHasta)
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

            If lblCategoria.Visible Then LineaGrupos += "Categorías : " & txtCategoriaDesde.Text & "/" & txtCategoriaHasta.Text
            If LineaGrupos <> "" Then LineaGrupos += " - "
            If lblMarcas.Visible Then LineaGrupos += "Marcas : " & txtMarcaDesde.Text & "/" & txtMarcaHasta.Text
            If LineaGrupos <> "" Then LineaGrupos += " - "
            If lblDivisiones.Visible Then LineaGrupos += "División : " & txtDivisionDesde.Text & "/" & txtDivisionHasta.Text
            If LineaGrupos <> "" Then LineaGrupos += " - "
            If lblJerarquias.Visible Then LineaGrupos += "Jerarquía : " & txtTipoJerarquia.Text

        End If
    End Function
    Private Function LineaCriterios() As String
        LineaCriterios = ""
        If ReporteNumero = ReporteVentas.cLibroIVA Then
            LineaCriterios = "MES : " & UCase(Format(CDate(txtPeriodoDesde.Text), "MMMM")) & _
                            " - AÑO : " & CDate(txtPeriodoHasta.Text).Year.ToString
        Else
            If lblPeriodo.Visible Then LineaCriterios += "Período: " & IIf(lblPeriodoDesde.Visible, txtPeriodoDesde.Text, "") & IIf(lblPeriodoDesde.Visible AndAlso lblPeriodoHasta.Visible, "/", "") & IIf(lblPeriodoHasta.Visible, txtPeriodoHasta.Text, "")
            If LineaCriterios <> "" Then LineaCriterios += " - "
            If lblTipodocumento.Visible Then LineaCriterios += " Tipo Documentos : " + txtTipDoc.Text
            If LineaCriterios <> "" Then LineaCriterios += " - "
            If lblcliente.Visible Then LineaCriterios += " Cliente : " + txtClienteDesde.Text + "/" + txtClienteHasta.Text
            If LineaCriterios <> "" Then LineaCriterios += " - "
            If lblMercanciaDesde.Visible Then LineaCriterios += " Mercancias : " + txtMercanciaDesde.Text + "/" + txtMercanciaHasta.Text
            If LineaCriterios <> "" Then LineaCriterios += " - "
            If lblAsesorCriterios.Visible Then LineaCriterios += " Asesor : " + txtAsesorDesdeCriterios.Text + "/" + txtAsesorHastaCriterios.Text
            If LineaCriterios <> "" Then LineaCriterios += " - "
            If lblAlmacen.Visible Then LineaCriterios += " Almacén : " + txtAlmacenDesde.Text + "/" + txtAlmacenHasta.Text

        End If
    End Function
    Private Function LineaConstantes() As String
        LineaConstantes = ""
        If lblConsResumen.Visible Then LineaConstantes += "Resumido : " + IIf(chkConsResumen.Checked, "Si", "No")
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblCondicionPago.Visible Then LineaConstantes += " Condición Pago : " + aCondicionPago(cmbCondicionPago.SelectedIndex)
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblEstatusFactura.Visible Then LineaConstantes += lblEstatusFactura.Text + cmbEstatusFacturas.SelectedItem.ToString
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblTipo.Visible Then
            Select Case ReporteNumero
                Case ReporteVentas.cPedidosVsEstatus
                    LineaConstantes += " Tipo Cliente : " + aTipoSemaforo(cmbTipo.SelectedIndex)
                Case ReporteVentas.cSaldosPorDocumento
                    LineaConstantes += " Tipo saldos : " + cmbTipo.Text
                Case Else
                    LineaConstantes += " Tipo Cliente : " + aTipo(cmbTipo.SelectedIndex)
            End Select
        End If
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblEstatus.Visible Then LineaConstantes += " Estatus Clientes : " + aEstatus(cmbEstatus.SelectedIndex)
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblLapso.Visible Then LineaConstantes += " Lapsos: 1. " + txtDesde1.Text + "-" + txtHasta1.Text + _
                                                    " 2. " + txtDesde2.Text + "-" + txtHasta2.Text + _
                                                    " 3. " + txtDesde3.Text + "-" + txtHasta3.Text + _
                                                    " 4. " + txtDesde4.Text
        If lblConstanteVentas.Visible Then LineaConstantes += " - Ventas mayores a " & txtConstanteVentas.Text & " " & cmbTipoReporte.SelectedText
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblTipoReporte.Visible Then LineaConstantes += " - Tipo Reporte : " & aTipoReporte(cmbTipoReporte.SelectedIndex)


    End Function
    Private Sub btnOrdenDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrdenDesde.Click
        txtDeOrden(txtOrdenDesde)
    End Sub

    Private Sub btnOrdenHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrdenHasta.Click
        txtDeOrden(txtOrdenHasta)
    End Sub
    Private Sub txtDeOrden(ByVal txt As TextBox)
        Select Case cmbOrdenadoPor.Text
            Case "Código Cliente"
                txt.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcli codigo, nombre descripcion from jsvencatcli where id_emp = '" & jytsistema.WorkID & "' order by codcli ", "Clientes", txt.Text)
            Case "Nombre Cliente"
            Case "Número Factura"
        End Select
    End Sub

    Private Sub txtOrdenDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOrdenDesde.TextChanged
        txtOrdenHasta.Text = txtOrdenDesde.Text
    End Sub

    Private Sub cmbCXCAgrupadorPor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCXCAgrupadorPor.SelectedIndexChanged

        ft.visualizarObjetos(False, lblGrupoDesde, lblGrupoHasta, lblCanal, lblTipoNegocio, lblZona, lblRuta, lblAsesor, lblTerritorio, _
                            txtCanalDesde, btnCanalDesde, txtCanalHasta, btnCanalHasta, _
                            txtTipoNegocioDesde, btnTipoNegocioDesde, txtTipoNegocioHasta, btnTipoNegocioHasta, _
                            txtZonaDesde, btnZonaDesde, txtZonaHasta, btnZonaHasta, _
                            txtRutaDesde, btnRutaDesde, txtRutaHasta, btnRutaHasta, _
                            txtAsesorDesde, btnAsesorDesde, txtAsesorHasta, btnAsesorHasta, _
                            txtPais, btnPais, txtEstado, btnEstado, txtMunicipio, btnMunicipio, _
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
        ft.visualizarObjetos(True, lblTerritorio, txtPais, btnPais, txtEstado, btnEstado, _
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
        ft.visualizarObjetos(True, lblJerarquias, txtTipoJerarquia, btnTipoJerarquia, txtCodjer1, btnCodjer1, _
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
        grpGrupos.Text = "Agrupago por    " & "Mercancías : " & cmbMERAgrupadoPor.Text & "   /   Ventas : " & cmbCXCAgrupadorPor.Text & " "
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
        txtCanalDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, "select codigo, descrip descripcion from jsvenliscan where id_emp = '" & jytsistema.WorkID & "' order by 1", "Canales", _
                                               txtCanalDesde.Text)
    End Sub

    Private Sub btnCanalHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCanalHasta.Click
        txtCanalHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, "select codigo, descrip descripcion from jsvenliscan where id_emp = '" & jytsistema.WorkID & "' order by 1", "Canales", _
                                               txtCanalHasta.Text)
    End Sub

    Private Sub btnTipoNegocioDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoNegocioDesde.Click
        txtTipoNegocioDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, "select codigo, descrip descripcion from jsvenlistip where id_emp = '" & jytsistema.WorkID & "' order by 1", "Tipos de Negocio", _
                                                     txtTipoNegocioDesde.Text)
    End Sub

    Private Sub btnTipoNegocioHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoNegocioHasta.Click
        txtTipoNegocioHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, "select codigo, descrip descripcion from jsvenlistip where id_emp = '" & jytsistema.WorkID & "' order by 1", "Tipos de Negocio", _
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

    Private Sub chkList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkList.SelectedIndexChanged, _
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
        ft.visualizarObjetos(False, lblGrupoDesde, lblGrupoHasta, lblCategoria, lblMarcas, lblDivisiones, lblJerarquias, _
                         txtCategoriaDesde, btnCategoriaDesde, txtCategoriaHasta, btnCategoriaHasta, _
                         txtMarcaDesde, btnMarcaDesde, txtMarcaHasta, btnMarcaHasta, _
                         txtDivisionDesde, btnDivisionDesde, txtDivisionHasta, btnDivisionHasta, _
                         txtTipoJerarquia, btnTipoJerarquia, txtCodjer1, btnCodjer1, txtCodjer2, btnCodjer2, _
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
            Case 6 '"Categorías & Marcas"
                VerCategorias()
                VerMarcas()
            Case 7 '"División & Categorías &  Marcas"
                VerDivisiones()
                VerCategorias()
                VerMarcas()
            Case 8 '"Categorías & Jerarquías"
                VerCategorias()
                VerJerarquias()
            Case 9 '"División & Jerarquía"
                VerDivisiones()
                VerJerarquias()
            Case 10 '"Mix & Categorías & Marcas"
                VerCategorias()
                VerMarcas()
            Case 11 '"Mix & Jerarquías"
                VerJerarquias()
            Case 12 '"Mix & División"
                VerDivisiones()
            Case 13 '"Mix & División & Categorías & Marcas"
                VerDivisiones()
                VerCategorias()
                VerMarcas()
            Case 14 '"Mix & División & Jerarquías"
                VerDivisiones()
                VerJerarquias()
            Case 15 '"División & Mix"
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

    Private Sub btnMercanciaDesde_Click(sender As System.Object, e As System.EventArgs) Handles btnMercanciaDesde.Click
        txtMercanciaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codart codigo, nomart descripcion from jsmerctainv where estatus = 1 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Mercancías", _
                                                  txtMercanciaDesde.Text)
    End Sub

    Private Sub btnMercanciaHasta_Click(sender As System.Object, e As System.EventArgs) Handles btnMercanciaHasta.Click
        txtMercanciaHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codart codigo, nomart descripcion from jsmerctainv where estatus = 1 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Mercancías", _
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

    Private Sub btnAlmacenDesde_Click(sender As System.Object, e As System.EventArgs) Handles btnAlmacenDesde.Click
        txtAlmacenDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codalm codigo, desalm descripcion from jsmercatalm where id_emp = '" & jytsistema.WorkID & "' order by 1", "Asesores Comerciales", txtAlmacenDesde.Text)
    End Sub

    Private Sub btnAlmacenHasta_Click(sender As System.Object, e As System.EventArgs) Handles btnAlmacenHasta.Click
        txtAlmacenHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codalm codigo, desalm descripcion from jsmercatalm where id_emp = '" & jytsistema.WorkID & "' order by 1", "Asesores Comerciales", txtAlmacenHasta.Text)
    End Sub

    Private Sub txtAlmacenDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtAlmacenDesde.TextChanged
        txtAlmacenHasta.Text = txtAlmacenDesde.Text
    End Sub

    Private Sub txtDesde1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDesde1.KeyPress, txtDesde2.KeyPress, _
        txtDesde3.KeyPress, txtDesde4.KeyPress, txtHasta1.KeyPress, txtHasta2.KeyPress, txtHasta3.KeyPress, txtPrimeros.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
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

    Private Sub txtCategoriaDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCategoriaDesde.TextChanged
        txtCategoriaHasta.Text = txtCategoriaDesde.Text
    End Sub

    Private Sub txtMarcaDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtMarcaDesde.TextChanged
        txtMarcaHasta.Text = txtMarcaDesde.Text
    End Sub

    Private Sub txtDivisionDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtDivisionDesde.TextChanged
        txtDivisionHasta.Text = txtDivisionDesde.Text
    End Sub
End Class