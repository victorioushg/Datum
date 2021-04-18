Imports MySql.Data.MySqlClient

Public Class jsGenRenglonesMovimientosPlus

    Private Const sModulo As String = "Movimiento renglones"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer

    Private nPosicion As Integer
    Private nModulo As String
    Private Documento As String
    Private Fechadocumento As Date
    Private Almacen As String
    Private numRenglon As String
    Private n_Apuntador As Long
    Private CodigoCliente As String
    Private CodigoProveedor As String = ""

    Private AceptaDescuento As Boolean = True
    Private CostoActual As Double
    Private TarifaActual As String
    Private PesoActual As Double
    Private Equivalencia As Double = 1
    Private aUnidades() As String = {}
    Private aPrecios() As String = {}

    Private aPreciosMercancias(5, 2) As Object

    Private aDescuentosMercancias() As Double = {}
    Private CodigoBarras As String = ""
    Private aTipoRenglon() As String = {"Normal", "Sin Descuento", "Bonificación"}
    Private EsServicio As Boolean = False
    Private numPresupuesto As String = ""
    Private numPresupuestoRenglon As String = ""
    Private numPedido As String = ""
    Private numPedidoRenglon As String = ""
    Private numNotaEntgrega As String = ""
    Private numNotaEntregaRenglon As String = ""
    Private FacturaAfectada As String = ""
    Private ProveedorLista As String = ""
    Private CodigoAsesor As String = ""
    Private CodigoProveedorAnterior As String = ""
    Private DescuentoOfertaMaximo As Double = 0.0

    Private ValidaMultiplosDeUnidadDeVenta As Boolean = True

    Private aTablasRenglones() As String = {"TRF.jsmerrentra.numtra", _
                                           "PAL.jsmerrenped.numped", _
                                           "PVE.jsvenrenpos.numfac", _
                                           "COT.jsvenrencot.numcot", _
                                           "PPE.jsvenrenpedrgv.numped", _
                                           "PED.jsvenrenped.numped", _
                                           "PFC.jsvenrennot.numfac", _
                                           "FAC.jsvenrenfac.numfac", _
                                           "NDV.jsvenrenndb.numndv", _
                                           "NCV.jsvenrenncr.numncr", _
                                           "NCC.jsprorenncr.numncr", _
                                           "ORD.jsprorenord.numord", _
                                           "REC.jsprorenrep.numrec", _
                                           "GAS.jsprorengas.numgas", _
                                           "COM.jsprorencom.numcom", _
                                           "NDC.jsprorenndb.numndb"}
    Private Enum FacturaAPartirDe
        iPrecio = 0
        iCostos = 1
    End Enum
    Private TipoFacturacionCliente As Integer = 0


    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property

    Public Sub Agregar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                        ByVal Modulo As String, ByVal NumeroDocumento As String, ByVal FechaDoc As Date, _
                        Optional ByVal AlmacenDoc As String = "", Optional ByVal Tarifa As String = "A", _
                        Optional ByVal CodCliente As String = "", Optional ByVal Servicio As Boolean = False, _
                        Optional ByVal PresupuestoNumero As String = "", Optional ByVal PresupuestoRenglon As String = "", _
                        Optional ByVal PedidoNumero As String = "", Optional ByVal PedidoRenglon As String = "", _
                        Optional ByVal NotaEntregaNumero As String = "", Optional ByVal NotaEntregaRenglon As String = "", _
                        Optional ByVal NumeroFacturaAfectada As String = "", _
                        Optional ByVal ProveedorParaLista As String = "", Optional ByVal CodigoVendedor As String = "", _
                        Optional ByVal CodigoAnteriorProveedor As String = "")

        i_modo = movimiento.iAgregar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt
        nModulo = Modulo
        Documento = NumeroDocumento
        Fechadocumento = FechaDoc
        Almacen = AlmacenDoc
        TarifaActual = Tarifa
        CodigoCliente = CodCliente
        EsServicio = Servicio
        numPresupuesto = PresupuestoNumero
        numPresupuestoRenglon = PresupuestoRenglon
        numPedido = PedidoNumero
        numPedidoRenglon = PedidoRenglon
        numNotaEntgrega = NotaEntregaNumero
        numNotaEntregaRenglon = NotaEntregaRenglon
        FacturaAfectada = NumeroFacturaAfectada
        ProveedorLista = ProveedorParaLista
        CodigoAsesor = CodigoVendedor
        CodigoProveedorAnterior = CodigoAnteriorProveedor
        TipoFacturacionCliente = ft.DevuelveScalarEntero(MyConn, " select codcre from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")

        AsignarTooltips()
        Iniciar(nModulo)
        Habilitar(nModulo)
        VerificarParametros(nModulo)
        IniciarTXT()

        Me.ShowDialog()

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                      ByVal Modulo As String, ByVal NumeroDocumento As String, ByVal FechaDoc As Date, _
                      Optional ByVal AlmacenDoc As String = "", Optional ByVal Tarifa As String = "A", _
                      Optional ByVal CodCliente As String = "", Optional ByVal Servicio As Boolean = False, _
                      Optional ByVal PresupuestoNumero As String = "", Optional ByVal PresupuestoRenglon As String = "", _
                      Optional ByVal PedidoNumero As String = "", Optional ByVal PedidoRenglon As String = "", _
                      Optional ByVal NotaEntregaNumero As String = "", Optional ByVal NotaEntregaRenglon As String = "", _
                      Optional ByVal NumeroFacturaAfectada As String = "", _
                      Optional ByVal ProveedorParaLista As String = "", Optional ByVal CodigoVendedor As String = "", _
                      Optional ByVal CodigoAnteriorProveedor As String = "")


        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        nModulo = Modulo
        Documento = NumeroDocumento
        Fechadocumento = FechaDoc
        Almacen = AlmacenDoc
        TarifaActual = Tarifa
        CodigoCliente = CodCliente
        EsServicio = Servicio
        numPresupuesto = PresupuestoNumero
        numPresupuestoRenglon = PresupuestoRenglon
        numPedido = PedidoNumero
        numPedidoRenglon = PedidoRenglon
        numNotaEntgrega = NotaEntregaNumero
        numNotaEntregaRenglon = NotaEntregaRenglon
        RellenaCombo(aTipoRenglon, cmbTipoRenglon)
        FacturaAfectada = NumeroFacturaAfectada
        ProveedorLista = ProveedorParaLista
        CodigoAsesor = CodigoVendedor
        CodigoProveedorAnterior = CodigoAnteriorProveedor
        TipoFacturacionCliente = ft.DevuelveScalarEntero(MyConn, " select codcre from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")

        AsignarTooltips()
        Iniciar(nModulo)
        Habilitar(nModulo)
        AsignarTXT(Apuntador)
        VerificarParametros(nModulo)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnCodigo, "<B>Seleccionar</B> mercancía")
        C1SuperTooltip1.SetToolTip(btnDescripcion, "<B>Indicar o agregar comentario </B>adicional a la descripción...")
        C1SuperTooltip1.SetToolTip(btnFactura, "<B>Seleccionar factura</B> de la cual proviene la devolución...")
        C1SuperTooltip1.SetToolTip(btnLote, "seleccionar <B>lote</B> de inventario para este movimiento...")
        C1SuperTooltip1.SetToolTip(btnCantidadTC, "permite <B>Seleccionar</B> las cantidades en tallas y colores de la mercancía ...")
        C1SuperTooltip1.SetToolTip(btnPesoCaptura, "<B>Captura o toma</B> el peso de la mercancía desde una balanza conectada...")
        C1SuperTooltip1.SetToolTip(btnCausa, "selecciona la<B> causa</B> de la devolución...")

    End Sub
    Private Sub Habilitar(ByVal nomModulo As String)
        Select Case nomModulo
            Case "TRF"
                HabilitarObjetos(False, True, txtCostoPrecioTotal, txtPesoTotal, txtPrecioIVA, _
                                 txtComentarioOferta, txtTotalIVA)
            Case "PVE", "COT", "PPE", "PED", "PFC", "FAC", "NDV"
                HabilitarObjetos(False, True, txtCostoPrecioTotal, txtPesoTotal, txtPrecioIVA, _
                                 txtComentarioOferta, txtTotalIVA, txtIVA, txtPorIVA, _
                                 txtDesc_ofe, txtDesc_cli, txtDesc_art)
                If nModulo = "NDV" Then HabilitarObjetos(False, True, txtCausa)
            Case "NCV", "NCC"
                HabilitarObjetos(False, True, txtCostoPrecioTotal, txtPesoTotal, txtPrecioIVA, _
                                 txtComentarioOferta, txtTotalIVA, txtIVA, txtPorIVA, txtCausa)
            Case "ORD"
                HabilitarObjetos(False, True, txtCostoPrecioTotal, txtPesoTotal, txtPrecioIVA, _
                                 txtComentarioOferta, txtTotalIVA, txtIVA, txtPorIVA)
            Case "REC", "GAS", "COM", "NDC"
                HabilitarObjetos(False, True, txtCostoPrecioTotal, txtPesoTotal, txtPrecioIVA, _
                    txtComentarioOferta, txtTotalIVA, txtIVA, txtPorIVA)
                If nModulo = "NDC" Then HabilitarObjetos(False, True, txtCausa)
        End Select

    End Sub
    Private Sub VerificarParametros(ByVal nomModulo As String)
        Select Case nomModulo
            Case "TRF", "ORD", "REC", "COM", "NCC", "NDC"
            Case "PVE"
                PermiteEditarPrecio(Gestion.iPuntosdeVentas, "POSPARAM12")
                VisualizarObjetos(False, cmbCostoPrecio) : VisualizarObjetos(True, txtCostoPrecio)
            Case "COT"
                PermiteEditarPrecio(Gestion.iVentas, "VENCOTPA04")
                PermiteEditarDescuentoArticulo(Gestion.iVentas, "VENCOTPA05")
                PermiteEditarDescuentoCliente(Gestion.iVentas, "VENCOTPA06")
            Case "PPE"
                PermiteEditarPrecio(Gestion.iVentas, "VENPPEPA04")
                PermiteEditarDescuentoArticulo(Gestion.iVentas, "VENPPEPA05")
                PermiteEditarDescuentoCliente(Gestion.iVentas, "VENPPEPA06")
            Case "PED"
                PermiteEditarPrecio(Gestion.iVentas, "VENPEDPA04")
                PermiteEditarDescuentoArticulo(Gestion.iVentas, "VENPEDPA05")
                PermiteEditarDescuentoCliente(Gestion.iVentas, "VENPEDPA06")
            Case "PFC"
                PermiteEditarPrecio(Gestion.iVentas, "VENNOTPA04")
                PermiteEditarDescuentoArticulo(Gestion.iVentas, "VENNOTPA05")
                PermiteEditarDescuentoCliente(Gestion.iVentas, "VENNOTPA06")
            Case "FAC"
                PermiteEditarPrecio(Gestion.iVentas, "VENFACPA04")
                PermiteEditarDescuentoArticulo(Gestion.iVentas, "VENFACPA05")
                PermiteEditarDescuentoCliente(Gestion.iVentas, "VENFACPA06")
            Case "NCV"
                PermiteEditarPrecio(Gestion.iVentas, "VENNCRPA04")
                'HabilitarObjetos(True, True, txtCostoPrecio)
                'HabilitarObjetos(False, True, cmbCostoPrecio)
                PermiteEditarPorcentajeAceptacion(Gestion.iVentas, "VENNCRPA05")
            Case "NDV"
                PermiteEditarPrecio(Gestion.iVentas, "VENNDBPA04")
                PermiteEditarDescuentoArticulo(Gestion.iVentas, "VENNDBPA05")
                PermiteEditarDescuentoCliente(Gestion.iVentas, "VENNDBPA06")
        End Select

        If EsServicio Then
            VisualizarObjetos(False, cmbCostoPrecio) : VisualizarObjetos(True, txtCostoPrecio)
            HabilitarObjetos(True, True, txtCostoPrecio)
            HabilitarObjetos(False, True, cmbCostoPrecio)
        End If

    End Sub
    Private Sub PermiteEditarPrecio(ByVal Gestion As Gestion, ByVal NombreCampoParametro As String)
        Dim bParametro As Boolean = CBool(ParametroPlus(MyConn, Gestion, NombreCampoParametro))
        VisualizarObjetos(Not bParametro, cmbCostoPrecio) : VisualizarObjetos(bParametro, txtCostoPrecio)
        HabilitarObjetos(bParametro, True, txtCostoPrecio)
        HabilitarObjetos(Not bParametro, True, cmbCostoPrecio)
    End Sub
    Private Sub PermiteEditarDescuentoArticulo(ByVal Gestion As Gestion, ByVal NombreCampoParametro As String)
        Dim bParametro As Boolean = CBool(ParametroPlus(MyConn, Gestion, NombreCampoParametro))
        HabilitarObjetos(bParametro, True, txtDesc_art)
    End Sub
    Private Sub PermiteEditarDescuentoCliente(ByVal Gestion As Gestion, ByVal NombreCampoParametro As String)
        Dim bParametro As Boolean = CBool(ParametroPlus(MyConn, Gestion, NombreCampoParametro))
        HabilitarObjetos(bParametro, True, txtDesc_cli)
    End Sub
    Private Sub PermiteEditarPorcentajeAceptacion(ByVal Gestion As Gestion, ByVal NombreCampoParametro As String)
        Dim bParametro As Boolean = CBool(ParametroPlus(MyConn, Gestion, NombreCampoParametro))
        HabilitarObjetos(bParametro, True, txtPorAceptaDev)
    End Sub

    Private Sub Iniciar(ByVal nomModulo As String)
        Select Case nomModulo
            Case "TRF"
                VisualizarObjetos(False, lblFactura, txtFactura, btnFactura)
                VisualizarObjetos(False, lblIVA, txtIVA, txtPorIVA)
                VisualizarObjetos(False, btnCantidadTC)
                VisualizarObjetos(False, cmbCostoPrecio)
                VisualizarObjetos(False, lblDsctoMercancia, txtDesc_art)
                VisualizarObjetos(False, lblDsctoCliente, txtDesc_cli, btnDescProveedor)
                VisualizarObjetos(False, lblDsctoOferta, txtDesc_ofe)
                VisualizarObjetos(False, lblPorAcepta, txtPorAceptaDev)
                VisualizarObjetos(False, lblCausa, txtCausa, btnCausa, lblCausaDEs)
                VisualizarObjetos(False, txtPrecioIVA, txtComentarioOferta, txtTotalIVA, lblPrecioIVA, lbltotalIVA)
                VisualizarObjetos(False, cmbTipoRenglon, lblTipoRenglon)
            Case "ORD"
                VisualizarObjetos(False, lblFactura, txtFactura, btnFactura)
                VisualizarObjetos(False, lblLote, txtLote, btnLote)
                VisualizarObjetos(False, btnCantidadTC, btnPesoCaptura)
                VisualizarObjetos(False, cmbCostoPrecio)
                VisualizarObjetos(False, lblDsctoOferta, txtDesc_ofe, txtComentarioOferta)
                VisualizarObjetos(False, lblPorAcepta, txtPorAceptaDev)
                VisualizarObjetos(False, lblCausa, txtCausa, btnCausa, lblCausaDEs)
                VisualizarObjetos(True, cmbTipoRenglon, lblTipoRenglon)
                lblDsctoCliente.Text = "Dscto. Prov."
            Case "REC", "GAS", "COM"
                VisualizarObjetos(False, lblFactura, txtFactura, btnFactura)
                VisualizarObjetos(False, btnCantidadTC)
                VisualizarObjetos(False, cmbCostoPrecio)
                VisualizarObjetos(False, lblDsctoOferta, txtDesc_ofe, txtComentarioOferta)
                VisualizarObjetos(False, lblPorAcepta, txtPorAceptaDev)
                VisualizarObjetos(False, lblCausa, txtCausa, btnCausa, lblCausaDEs)
                VisualizarObjetos(True, cmbTipoRenglon, lblTipoRenglon)
                lblDsctoCliente.Text = "Dscto. Prov."
            Case "NCC"
                VisualizarObjetos(False, btnCantidadTC)
                VisualizarObjetos(False, cmbCostoPrecio)
                VisualizarObjetos(False, lblDsctoCliente, txtDesc_cli, lblDsctoMercancia, txtDesc_art, btnDescProveedor)
                VisualizarObjetos(False, lblDsctoOferta, txtDesc_ofe, txtComentarioOferta)
                VisualizarObjetos(True, cmbTipoRenglon, lblTipoRenglon)
                lblDsctoCliente.Text = "Dscto. Prov."
            Case "NDC"
                VisualizarObjetos(False, btnCantidadTC)
                VisualizarObjetos(False, cmbCostoPrecio)
                VisualizarObjetos(False, lblDsctoOferta, txtDesc_ofe, txtComentarioOferta)
                VisualizarObjetos(False, lblPorAcepta, txtPorAceptaDev)
                VisualizarObjetos(True, cmbTipoRenglon, lblTipoRenglon)
                lblDsctoCliente.Text = "Dscto. Prov."
            Case "PVE", "COT", "PPE", "PED", "PFC", "FAC"
                VisualizarObjetos(False, lblFactura, txtFactura, btnFactura)
                VisualizarObjetos(False, lblPorAcepta, txtPorAceptaDev)
                VisualizarObjetos(False, lblCausa, txtCausa, btnCausa, lblCausaDEs)
                VisualizarObjetos(False, btnDescProveedor)
                VisualizarObjetos(False, cmbCostoPrecio)
                lblCostoPrecio.Text = "Precio Unitario"
                lblCostoTotal.Text = "Precio Total"
            Case "NCV"
                VisualizarObjetos(False, btnDescProveedor)
                VisualizarObjetos(False, lblDsctoCliente, txtDesc_cli, lblDsctoMercancia, txtDesc_art, btnDescProveedor, lblDsctoOferta, txtDesc_ofe, _
                                  txtComentarioOferta)
                lblCostoPrecio.Text = "Precio Unitario"
                lblCostoTotal.Text = "Precio Total"
                lblCausaDEs.Text = ""
            Case "NDV"
                VisualizarObjetos(False, cmbCostoPrecio)
                VisualizarObjetos(False, btnDescProveedor)
                VisualizarObjetos(False, lblPorAcepta, txtPorAceptaDev)
        End Select
    End Sub
    Private Sub IniciarTXT()
        Dim nomTablaEnBD As String = ""
        Dim fieldTablaEnBD As String = ""
        For Each elem As String In aTablasRenglones
            If elem.Split(".")(0) = nModulo Then
                nomTablaEnBD = elem.Split(".")(1)
                fieldTablaEnBD = elem.Split(".")(2)
            End If
        Next

        numRenglon = ft.autoCodigo(MyConn, "renglon", nomTablaEnBD, fieldTablaEnBD + ".id_emp", Documento + "." + jytsistema.WorkID, 5)

        txtCodigo.Text = ""
        txtDescripcion.Text = ""
        cmbUnidad.Items.Clear()
        txtFactura.Text = FacturaAfectada
        txtCostoPrecio.Text = ft.FormatoNumero(0.0)
        txtIVA.Text = ""
        txtDesc_art.Text = ft.FormatoNumero(0.0)
        txtDesc_cli.Text = ft.FormatoNumero(ft.DevuelveScalarDoble(MyConn, " SELECT DES_CLI FROM jsvencatcli WHERE CODCLI = '" & CodigoCliente & "' AND ID_EMP = '" & jytsistema.WorkID & "' "))
        txtDesc_ofe.Text = ft.FormatoNumero(0.0)
        txtPorAceptaDev.Text = ft.FormatoNumero(0.0)
        txtCostoPrecioTotal.Text = ft.FormatoNumero(0.0)
        txtCausa.Text = ""
        lblCausaDEs.Text = ""
        txtPesoTotal.Text = ft.FormatoCantidad(0.0)
        txtLote.Text = ""
        txtCantidad.Text = ft.FormatoCantidad(1.0)
        RellenaCombo(aTipoRenglon, cmbTipoRenglon)

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)
            If nModulo <> "TRF" Then RellenaCombo(aTipoRenglon, cmbTipoRenglon, .Item("estatus"))
            numRenglon = .Item("renglon")
            If .Item("item").ToString.Substring(0, 1) = "$" Then EsServicio = True
            txtCodigo.Text = .Item("item")
            txtDescripcion.Text = .Item("descrip")

            RellenaCombo(aUnidades, cmbUnidad, ft.InArray(aUnidades, .Item("unidad")))

            Select Case nModulo
                Case "TRF"
                    txtFactura.Text = ""
                    txtCostoPrecio.Text = ft.FormatoNumero(.Item("costou"))
                    txtIVA.Text = ""
                    txtDesc_art.Text = ft.FormatoNumero(0.0)
                    txtDesc_cli.Text = ft.FormatoNumero(0.0)
                    txtDesc_ofe.Text = ft.FormatoNumero(0.0)
                    txtPorAceptaDev.Text = ft.FormatoNumero(0.0)
                    txtCostoPrecioTotal.Text = ft.FormatoNumero(.Item("totren"))
                    txtCausa.Text = "" '  .Item("causa")
                    txtLote.Text = .Item("lote")
                Case "PVE", "COT", "PPE", "PED", "PFC", "FAC", "NDV"
                    txtCostoPrecio.Text = ft.FormatoNumero(.Item("precio"))
                    txtIVA.Text = .Item("IVA")
                    txtDesc_art.Text = ft.FormatoNumero(.Item("des_art"))
                    txtDesc_cli.Text = ft.FormatoNumero(.Item("des_cli"))
                    txtDesc_ofe.Text = ft.FormatoNumero(.Item("des_ofe"))
                    RellenaCombo(aTipoRenglon, cmbTipoRenglon, .Item("estatus"))
                    If InStr("PVE.PFC.FAC", nModulo) > 0 Then txtLote.Text = .Item("lote")
                    If nModulo = "NDV" Then
                        txtFactura.Text = .Item("numfac")
                        'txtCausa.Text = ft.MuestraCampoTexto(.Item("CAUSA").ToString)
                    End If

                Case "NCV"
                    txtCostoPrecio.Text = ft.FormatoNumero(.Item("precio"))
                    txtIVA.Text = .Item("IVA")
                    txtPorAceptaDev.Text = ft.FormatoNumero(.Item("por_acepta_dev"))
                    RellenaCombo(aTipoRenglon, cmbTipoRenglon, .Item("estatus"))
                    txtLote.Text = .Item("lote")
                    txtFactura.Text = .Item("numfac")
                    txtCausa.Text = .Item("CAUSA")
                Case "ORD"
                    txtFactura.Text = ""
                    txtCostoPrecio.Text = ft.FormatoNumero(.Item("costou"))
                    txtIVA.Text = .Item("IVA")
                    txtDesc_art.Text = ft.FormatoNumero(.Item("des_art"))
                    txtDesc_cli.Text = ft.FormatoNumero(.Item("des_pro"))
                    RellenaCombo(aTipoRenglon, cmbTipoRenglon, .Item("estatus"))
                Case "REC", "GAS", "COM"
                    txtFactura.Text = ""
                    txtLote.Text = .Item("lote")
                    txtCostoPrecio.Text = ft.FormatoNumero(.Item("costou"))
                    txtIVA.Text = .Item("IVA")
                    txtDesc_art.Text = ft.FormatoNumero(.Item("des_art"))
                    txtDesc_cli.Text = ft.FormatoNumero(.Item("des_pro"))
                    RellenaCombo(aTipoRenglon, cmbTipoRenglon, .Item("estatus"))
                Case "NCC"
                    txtFactura.Text = .Item("NUMCOM")
                    txtLote.Text = .Item("lote")
                    txtCostoPrecio.Text = ft.FormatoNumero(.Item("PRECIO"))
                    txtIVA.Text = .Item("IVA")
                    txtPorAceptaDev.Text = ft.FormatoNumero(.Item("POR_ACEPTA_DEV"))
                    RellenaCombo(aTipoRenglon, cmbTipoRenglon, .Item("estatus"))
                    txtLote.Text = .Item("LOTE")
                    txtCausa.Text = .Item("CAUSA")
                Case "NDC"
                    txtFactura.Text = .Item("NUMCOM")
                    txtLote.Text = .Item("lote")
                    txtCostoPrecio.Text = ft.FormatoNumero(.Item("COSTO"))
                    txtIVA.Text = .Item("IVA")
                    txtDesc_art.Text = ft.FormatoNumero(.Item("des_art"))
                    txtDesc_cli.Text = ft.FormatoNumero(.Item("des_pro"))
                    RellenaCombo(aTipoRenglon, cmbTipoRenglon, .Item("estatus"))
                    txtLote.Text = .Item("LOTE")
                    txtCausa.Text = .Item("CAUSA")
                Case Else

                    txtFactura.Text = ""
                    txtCostoPrecio.Text = ft.FormatoNumero(.Item("costou"))
                    txtIVA.Text = ""
                    txtDesc_art.Text = ft.FormatoNumero(0.0)
                    txtDesc_cli.Text = ft.FormatoNumero(0.0)
                    txtDesc_ofe.Text = ft.FormatoNumero(0.0)
                    txtPorAceptaDev.Text = ft.FormatoNumero(0.0)
                    txtCostoPrecioTotal.Text = ft.FormatoNumero(.Item("totren"))
                    txtCausa.Text = ""
                    txtLote.Text = ""
            End Select

            txtPesoTotal.Text = ft.FormatoCantidad(.Item("peso"))
            txtCantidad.Text = ft.FormatoCantidad(.Item("cantidad"))


        End With
    End Sub
    Private Sub jsGenRenglonesMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsGenRenglonesMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo & nModulo, txtCodigo.Text)
        txtCodigo.Focus()
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtDescripcion.GotFocus, cmbUnidad.GotFocus, txtFactura.GotFocus, txtLote.GotFocus, txtCantidad.GotFocus, _
        txtCostoPrecio.GotFocus, cmbCostoPrecio.GotFocus, txtDesc_art.GotFocus, txtDesc_cli.GotFocus, txtDesc_ofe.GotFocus, _
        txtPorAceptaDev.GotFocus, btnCantidadTC.GotFocus, btnCausa.GotFocus, btnCodigo.GotFocus, _
        btnDescripcion.GotFocus, btnFactura.GotFocus, btnLote.GotFocus, btnPesoCaptura.GotFocus

        Select Case sender.name
            Case "txtCodigo"
                ft.mensajeEtiqueta(lblInfo, "Indique el código de la mercancía ...", Transportables.TipoMensaje.iInfo)
            Case "txtDescripcion"
                ft.mensajeEtiqueta(lblInfo, "Indique la descripción o nombre de la mercancía ...", Transportables.TipoMensaje.iInfo)
            Case "cmbUnidad"
                ft.mensajeEtiqueta(lblInfo, "Seleccione la unidad para movimiento de mercancía ...", Transportables.TipoMensaje.iInfo)
            Case "txtFactura"
                ft.mensajeEtiqueta(lblInfo, "Indique el Número de factura de la cual se desea hacer la devolución de mercancía ...", Transportables.TipoMensaje.iInfo)
            Case "txtLote"
                ft.mensajeEtiqueta(lblInfo, "Indique el número de lote al cual pertenece la mercancía...", Transportables.TipoMensaje.iInfo)
            Case "txtCantidad"
                ft.mensajeEtiqueta(lblInfo, "Indique la cantidad del movimiento de mercancía", Transportables.TipoMensaje.iInfo)
            Case "txtCostoPrecio"
                If InStr("TRF.ORD.REC.GAS.COM.NCC.NDC", nModulo) = 0 Then
                    ft.mensajeEtiqueta(lblInfo, "Indique el precio de la mercancía... ", Transportables.TipoMensaje.iInfo)
                Else
                    ft.mensajeEtiqueta(lblInfo, "Indique el costo de la mercancía... ", Transportables.TipoMensaje.iInfo)
                End If

            Case "cmbCostoPrecio"
                If InStr("TRF.ORD.REC.GAS.COM.NCC.NDC", nModulo) = 0 Then
                    ft.mensajeEtiqueta(lblInfo, "Seleccione el precio de la mercancía... ", Transportables.TipoMensaje.iInfo)
                Else
                    ft.mensajeEtiqueta(lblInfo, "Seleccione el costo de la mercancía... ", Transportables.TipoMensaje.iInfo)
                End If
            Case "txtDesc_art"
                ft.mensajeEtiqueta(lblInfo, "Indique descuento por mercancía...", Transportables.TipoMensaje.iInfo)
            Case "txtDesc_cli"
                If InStr("TRF.ORD.REC.GAS.COM.NCC.NDC", nModulo) = 0 Then
                    ft.mensajeEtiqueta(lblInfo, "Indique descuento por cliente...", Transportables.TipoMensaje.iInfo)
                Else
                    ft.mensajeEtiqueta(lblInfo, "Indique descuento por Proveedor ...", Transportables.TipoMensaje.iInfo)
                End If
            Case "txtDesc_ofe"
                ft.mensajeEtiqueta(lblInfo, "Indique descuento por oferta...", Transportables.TipoMensaje.iInfo)
            Case "txtPorAceptaDev"
                ft.mensajeEtiqueta(lblInfo, "Indique porcentaje devolución...", Transportables.TipoMensaje.iInfo)
            Case "btnCausa"
                ft.mensajeEtiqueta(lblInfo, "Seleccione la causa de devolución", Transportables.TipoMensaje.iInfo)
            Case "btnCantidadTC"
                ft.mensajeEtiqueta(lblInfo, "Seleccione las cantidades por talla y color de esta mercancía...", Transportables.TipoMensaje.iInfo)
            Case "btnPesoCaptura"
                ft.mensajeEtiqueta(lblInfo, "Seleccione el peso de la mercancía desde la balanza... ", Transportables.TipoMensaje.iInfo)
            Case "btnLote"
                ft.mensajeEtiqueta(lblInfo, "Seleccione el Número de lote de inventario...", Transportables.TipoMensaje.iInfo)
            Case "btnCodigo"
                ft.mensajeEtiqueta(lblInfo, "Seleccione el código de mercancía para el movimiento...", Transportables.TipoMensaje.iInfo)
            Case "btnFactura"
                ft.mensajeEtiqueta(lblInfo, "Seleccione el número de factura a la cual pertenece la mercancía", Transportables.TipoMensaje.iInfo)
            Case "btnDescripcion"
                ft.mensajeEtiqueta(lblInfo, "Agregar comentario más extenso a la descripción presentada", Transportables.TipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False

        Dim CantidadReal As Double = ValorCantidad(txtCantidad.Text) / IIf(Equivalencia = 0, 1, Equivalencia)
        Dim CantidadMovimiento As Double = MovimientoXDocumentoRenglonAlmacen(MyConn, txtCodigo.Text, Documento, nModulo, numRenglon, Almacen, lblInfo)

        If Trim(txtDescripcion.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar una descripción válida...")
            txtDescripcion.Focus()
            Exit Function
        End If

        If txtCodigo.Text.Substring(0, 1) <> "$" Then

            'CODIGO VALIDO
            If ft.DevuelveScalarCadena(MyConn, "select codart from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") = "" Then
                ft.mensajeAdvertencia("Mercancía NO VALIDA...")
                Exit Function
            End If

            If InStr(nModulo, "PVE.COT.PPE.PED.PFC.FAC.NDV") > 0 Then

                Dim MercanciaInactiva As Boolean = ft.DevuelveScalarBooleano(MyConn, "Select estatus from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                If MercanciaInactiva Then
                    ft.mensajeCritico("ESTA MERCANCIA POSEE ESTATUS DE INACTIVA POR FAVOR DIRIGASE AL SUPERVISOR ...")
                    Return False
                End If

            End If


            'EXISTENCIAS
            If nModulo = "TRF" AndAlso CBool(ParametroPlus(MyConn, Gestion.iMercancías, "MERTRAPA01")) Then
                If CantidadReal > (ExistenciaEnAlmacen(MyConn, txtCodigo.Text, Almacen, lblInfo) + CantidadMovimiento) Then
                    ft.mensajeAdvertencia("Cantidad es mayor a la existente en el almacén " & Almacen & " ...")
                    EnfocarTexto(txtCantidad)
                    Return False
                End If
            End If

            If nModulo = "PVE" AndAlso CBool(ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM21")) Then
                If CantidadReal > (ExistenciaEnAlmacen(MyConn, txtCodigo.Text, Almacen, lblInfo) + CantidadMovimiento) Then
                    ft.mensajeCritico("Cantidad es mayor a la existente en el almacén " & Almacen & " ...")
                    EnfocarTexto(txtCantidad)
                    Return False
                End If
            End If

            If nModulo = "PFC" AndAlso CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNOTPA13").ToString) Then
                If CantidadReal > (ExistenciaEnAlmacen(MyConn, txtCodigo.Text, Almacen, lblInfo) + CantidadMovimiento) Then
                    ft.mensajeCritico("Cantidad es mayor a la existente en el almacén " & Almacen & " ...")
                    EnfocarTexto(txtCantidad)
                    Return False
                End If
            End If

            If nModulo = "FAC" AndAlso CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA12").ToString) Then
                If CantidadReal > (ExistenciaEnAlmacen(MyConn, txtCodigo.Text, Almacen, lblInfo) + CantidadMovimiento) Then
                    ft.mensajeAdvertencia("Cantidad es mayor a la existente en el almacén " & Almacen & " ...")
                    EnfocarTexto(txtCantidad)
                    Return False
                End If
            End If


            If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNDBPA14").ToString) AndAlso nModulo = "NDB" Then
                If CantidadReal > (ExistenciaEnAlmacen(MyConn, txtCodigo.Text, Almacen, lblInfo) + CantidadMovimiento) Then
                    ft.mensajeAdvertencia("Cantidad es mayor a la existente en el almacén " & Almacen & " ...")
                    EnfocarTexto(txtCantidad)
                    Return False
                End If
            End If

            'PROVEEDOR EXCLUSIVO
            If (CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENCOTPA10")) Or _
                 CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPEDPA10")) Or _
                 CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPPEPA10")) Or _
                 CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNOTPA10")) Or _
                 CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA10"))) AndAlso ProveedorClienteEnItem(MyConn, lblInfo, CodigoProveedor, CodigoCliente) _
                AndAlso InStr(nModulo, "COT.PED.PPE.PFC.FAC.NDV") > 0 Then

                ft.mensajeAdvertencia(" Esta Mercancía no puede ser incluida pues El CLIENTE posee PROVEEDOR EXCLUSIVO...")
                Exit Function

            End If

            If ValorNumero(txtCantidad.Text) <= 0 Then
                ft.mensajeCritico("DEBE INDICAR UNA CANTIDAD VALIDA")
                EnfocarTexto(txtCantidad)
                Exit Function
            End If

            If ValorNumero(txtCostoPrecioTotal.Text) <= 0 Then
                ft.mensajeCritico("MERCANCIA SIN CANTIDAD O PRECIO POR FAVOR VERIFIQUE...")
                EnfocarTexto(txtCantidad)
                Exit Function
            End If

            ' CANTIDAD MULTIPLO VALIDO 
            If ValidaMultiplosDeUnidadDeVenta Then

                If cmbUnidad.Text <> "KGR" Then
                    If MultiploValido(MyConn, txtCodigo.Text, cmbUnidad.Text, ValorCantidad(txtCantidad.Text), lblInfo) Then
                    Else
                        ft.mensajeCritico("La cantidad NO es múltiplo válido para este movimiento. Verifique por favor...")
                        EnfocarTexto(txtCantidad)
                        Exit Function
                    End If
                End If
            End If

            If ValorNumero(txtCostoPrecio.Text) = 0.0 AndAlso InStr(nModulo, "ORD.TRF.REC.GAS.COM.NCC.NDC") > 0 Then
                ft.mensajeAdvertencia("Movimiento no puede tener costo CERO (0.00). Veriqfique por favor ")
                EnfocarTexto(txtCostoPrecio)
                Exit Function
            End If

            'PRECIO VALIDO EN RENGLON
            If InStr(nModulo, "PVE.COT.PPE.PED.PFC.FAC.NDV") > 0 Then
                Dim PrecioMinimo As Double = ValorNumero(MinValueInArray(DeleteArrayValue(aPrecios, "0.00")))
                Dim PrecioMaximo As Double = ValorNumero(MaxValueInArray(DeleteArrayValue(aPrecios, "0.00")))
                If txtCostoPrecio.Enabled And (ValorNumero(txtCostoPrecio.Text) > PrecioMaximo _
                        Or ValorNumero(txtCostoPrecio.Text) < PrecioMinimo) Then
                    ft.mensajeAdvertencia("Movimiento con PRECIO FUERA DE RANGO. Veriqfique por favor ")
                    EnfocarTexto(txtCostoPrecio)
                    Exit Function
                End If

                'DESCUENTOS 
                If MercanciaAceptaDescuento(MyConn, lblInfo, txtCodigo.Text) Then

                    'DESCUENTO ARTICULO
                    If ValorNumero(txtDesc_art.Text) <> 0 And ValorNumero(txtDesc_art.Text) > aDescuentosMercancias(cmbCostoPrecio.SelectedIndex) Then
                        ft.mensajeAdvertencia("Descuento debe ser menor o igual a " & ft.FormatoPorcentaje(aDescuentosMercancias(cmbCostoPrecio.SelectedIndex)))
                        EnfocarTexto(txtDesc_art)
                        Exit Function
                    End If

                    'DESCUENTO CLIENTE
                    Dim DescuentoCliente As Double = ft.DevuelveScalarDoble(MyConn, " select des_cli from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                    If ValorNumero(txtDesc_cli.Text) <> 0 And ValorNumero(txtDesc_cli.Text) > DescuentoCliente Then
                        ft.mensajeAdvertencia("Descuento debe ser menor o igual a " & ft.FormatoPorcentaje(DescuentoCliente))
                        EnfocarTexto(txtDesc_cli)
                        Exit Function
                    End If

                    'DESCUENTO OFERTA


                Else
                    If (ValorNumero(txtDesc_art.Text) + ValorNumero(txtDesc_cli.Text) + ValorNumero(txtDesc_ofe.Text)) > 0 Then
                        ft.mensajeAdvertencia("Esta mercancía NO ACEPTA descuentos...")
                        Exit Function
                    End If
                End If
            End If
            'CAUSA CREDITO DEBITO

            If InStr("NCV.NCC", nModulo) > 0 Then
                If txtCausa.Text.Trim() = "" Then
                    ft.mensajeAdvertencia("Debe indicar una causa válida...")
                    Exit Function
                End If
            End If


            If InStr("NCV", nModulo) > 0 Then
                If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNCRPA21")) Then
                    If txtFactura.Text.Trim = "" Then
                        ft.mensajeCritico("DEBE INDICAR EL NUMERO DE FACTURA QUE AFECTA ESTA DEVOLUCION. VERIFIQUE POR FAVOR...")
                        Return False
                    Else
                        If ft.DevuelveScalarCadena(MyConn, " SELECT NUMFAC FROM jsvenencfac where numfac = '" & txtFactura.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") = "" Then
                            ft.mensajeCritico("EL N° DE FACTURA INDICADO NO EXISTE. VERIFIQUE POR FAVOR ...")
                            Return False
                        End If
                End If
            End If
        End If



        Else
        If ft.DevuelveScalarCadena(MyConn, "select codser from jsmercatser where codser = '" & txtCodigo.Text.Substring(1, txtCodigo.Text.Length - 1) & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
            ft.mensajeAdvertencia("Servicio NO VALIDO...")
            Exit Function
        End If

        If ValorNumero(txtCostoPrecio.Text) = 0.0 Then
            ft.mensajeAdvertencia("Movimiento no puede tener costo/precio CERO (0.00). Veriqfique por favor ")
            EnfocarTexto(txtCostoPrecio)
            Exit Function
        End If
        End If

        Validado = True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dtLocal.Rows.Count
            End If

            Dim NumeroSerialFiscal As String = ""
            If jytsistema.WorkBox <> "" Then NumeroSerialFiscal = NumeroSERIALImpresoraFISCAL(MyConn, lblInfo, jytsistema.WorkBox)

            Select Case nModulo
                Case "TRF"
                    InsertEditMERCASRenglonTransferencia(MyConn, lblInfo, Insertar, Documento, numRenglon, txtCodigo.Text, txtDescripcion.Text, cmbUnidad.Text, ValorCantidad(txtCantidad.Text), ValorCantidad(txtPesoTotal.Text), txtLote.Text, ValorNumero(txtCostoPrecio.Text), ValorNumero(txtCostoPrecioTotal.Text), "1")
                Case "COT"
                    InsertEditVENTASRenglonPresupuesto(MyConn, lblInfo, Insertar, Documento, numRenglon, txtCodigo.Text, txtDescripcion.Text, _
                                        txtIVA.Text, "", cmbUnidad.Text, 0.0, ValorCantidad(txtCantidad.Text), ValorCantidad(txtCantidad.Text), _
                                        ValorCantidad(txtPesoTotal.Text), cmbTipoRenglon.SelectedIndex, ValorNumero(txtCostoPrecio.Text), _
                                        ValorNumero(txtDesc_cli.Text), ValorNumero(txtDesc_art.Text), ValorNumero(txtDesc_ofe.Text), _
                                        ValorNumero(txtCostoPrecioTotal.Text), ValorNumero(txtCostoPrecioTotal.Text), "1", IIf(cmbTipoRenglon.SelectedIndex < 2, 0, 1))
                Case "PPE"
                    InsertEditVENTASRenglonPrePedidos(MyConn, lblInfo, Insertar, Documento, numRenglon, txtCodigo.Text, txtDescripcion.Text, _
                                        txtIVA.Text, "", cmbUnidad.Text, 0.0, ValorCantidad(txtCantidad.Text), ValorCantidad(txtCantidad.Text), _
                                        0.0, 0.0, 0, ValorCantidad(txtPesoTotal.Text), txtLote.Text, cmbTipoRenglon.SelectedIndex, ValorNumero(txtCostoPrecio.Text), _
                                        ValorNumero(txtDesc_cli.Text), ValorNumero(txtDesc_art.Text), ValorNumero(txtDesc_ofe.Text), _
                                        ValorNumero(txtCostoPrecioTotal.Text), ValorNumero(txtCostoPrecioTotal.Text), "", "", "", "1", IIf(cmbTipoRenglon.SelectedIndex < 2, 0, 1))
                Case "PED"
                    InsertEditVENTASRenglonPedidos(MyConn, lblInfo, Insertar, Documento, numRenglon, txtCodigo.Text, txtDescripcion.Text, _
                                                txtIVA.Text, "", cmbUnidad.Text, 0.0, ValorCantidad(txtCantidad.Text), ValorCantidad(txtCantidad.Text), _
                                                0.0, 0.0, 0, ValorCantidad(txtPesoTotal.Text), txtLote.Text, cmbTipoRenglon.SelectedIndex, _
                                                ValorNumero(txtCostoPrecio.Text), ValorNumero(txtDesc_cli.Text), ValorNumero(txtDesc_art.Text), _
                                                ValorNumero(txtDesc_ofe.Text), ValorNumero(txtCostoPrecioTotal.Text), ValorNumero(txtCostoPrecioTotal.Text), _
                                                numPresupuesto, numPresupuestoRenglon, "", "1", IIf(cmbTipoRenglon.SelectedIndex < 2, 0, 1))

                Case "PFC"
                    InsertEditVENTASRenglonNotasEntrega(MyConn, lblInfo, Insertar, Documento, numRenglon, txtCodigo.Text, txtDescripcion.Text, _
                                                txtIVA.Text, "", cmbUnidad.Text, 0.0, ValorCantidad(txtCantidad.Text), _
                                                0.0, 0.0, 0, "", "", ValorCantidad(txtPesoTotal.Text), txtLote.Text, cmbTipoRenglon.SelectedIndex, _
                                                ValorNumero(txtCostoPrecio.Text), ValorNumero(txtDesc_cli.Text), ValorNumero(txtDesc_art.Text), _
                                                ValorNumero(txtDesc_ofe.Text), ValorNumero(txtCostoPrecioTotal.Text), ValorNumero(txtCostoPrecioTotal.Text), _
                                                numPresupuesto, numPresupuestoRenglon, numPedido, numPedidoRenglon, "", "", "", "1", IIf(cmbTipoRenglon.SelectedIndex < 2, 0, 1))
                Case "FAC"
                    InsertEditVENTASRenglonFactura(MyConn, lblInfo, Insertar, Documento, numRenglon, txtCodigo.Text, txtDescripcion.Text, _
                                                txtIVA.Text, "", cmbUnidad.Text, 0.0, ValorCantidad(txtCantidad.Text), _
                                                0.0, 0.0, 0, "", "", ValorCantidad(txtPesoTotal.Text), txtLote.Text, cmbTipoRenglon.SelectedIndex, _
                                                ValorNumero(txtCostoPrecio.Text), ValorNumero(txtDesc_cli.Text), ValorNumero(txtDesc_art.Text), _
                                                ValorNumero(txtDesc_ofe.Text), ValorNumero(txtCostoPrecioTotal.Text), ValorNumero(txtCostoPrecioTotal.Text), _
                                                numPresupuesto, numPresupuestoRenglon, numPedido, numPedidoRenglon, numNotaEntgrega, numNotaEntregaRenglon, _
                                                "", "", "", "1", IIf(cmbTipoRenglon.SelectedIndex < 2, 0, 1))
                Case "PVE"
                    InsertarModificarPOSRenglonPuntoDeVenta(MyConn, lblInfo, Insertar, Documento, NumeroSerialFiscal, 0, numRenglon, txtCodigo.Text, _
                                        CodigoBarras, txtDescripcion.Text, txtIVA.Text, cmbUnidad.SelectedItem, _
                                        ValorCantidad(txtCantidad.Text), ValorNumero(txtCostoPrecio.Text), ValorCantidad(txtPesoTotal.Text), _
                                        txtLote.Text, 0, ValorNumero(txtDesc_cli.Text), ValorNumero(txtDesc_art.Text), _
                                        ValorNumero(txtDesc_ofe.Text), ValorNumero(txtCostoPrecioTotal.Text), _
                                        ValorNumero(txtCostoPrecioTotal.Text), "", "1", "0")
                Case "NCV"
                    InsertEditVENTASRenglonNotaDeCredito(MyConn, lblInfo, Insertar, Documento, numRenglon, txtCodigo.Text, _
                                                         txtDescripcion.Text, txtIVA.Text, "", cmbUnidad.Text, 0.0, ValorCantidad(txtCantidad.Text), _
                                                         ValorCantidad(txtPesoTotal.Text), txtLote.Text, cmbTipoRenglon.SelectedIndex, _
                                                         ValorNumero(txtCostoPrecio.Text), 0.0, 0.0, 0.0, ValorNumero(txtPorAceptaDev.Text), _
                                                         ValorNumero(txtCostoPrecioTotal.Text), ValorNumero(txtCostoPrecioTotal.Text), _
                                                         txtFactura.Text, "", IIf(cmbTipoRenglon.SelectedIndex < 2, 0, 1), txtCausa.Text, "1")
                Case "NDV"
                    InsertEditVENTASRenglonNOTASDEBITO(MyConn, lblInfo, Insertar, Documento, numRenglon, txtCodigo.Text, _
                                                       txtDescripcion.Text, txtIVA.Text, "", cmbUnidad.Text, 0.0, ValorCantidad(txtCantidad.Text), _
                                                       ValorCantidad(txtPesoTotal.Text), txtLote.Text, cmbTipoRenglon.SelectedIndex, ValorNumero(txtCostoPrecio.Text), _
                                                       ValorNumero(txtDesc_cli.Text), ValorNumero(txtDesc_art.Text), ValorNumero(txtDesc_ofe.Text), _
                                                       ValorNumero(txtPorAceptaDev.Text), ValorNumero(txtCostoPrecioTotal.Text), ValorNumero(txtCostoPrecioTotal.Text), _
                                                       txtFactura.Text, "", IIf(cmbTipoRenglon.SelectedIndex < 2, 0, 1), txtCausa.Text, "1")

                Case "ORD"
                    InsertEditCOMPRASRenglonOrdenes(MyConn, lblInfo, Insertar, Documento, numRenglon, CodigoProveedorAnterior, txtCodigo.Text, _
                                        txtDescripcion.Text, txtIVA.Text, "", cmbUnidad.Text, ValorCantidad(txtCantidad.Text), ValorCantidad(txtPesoTotal.Text), _
                                        ValorCantidad(txtCantidad.Text), "0", ValorNumero(txtCostoPrecio.Text), ValorNumero(txtDesc_art.Text), _
                                        ValorNumero(txtDesc_cli.Text), ValorNumero(txtCostoPrecioTotal.Text), ValorNumero(txtCostoPrecioTotal.Text), _
                                        "", "1")
                Case "REC"
                    InsertEditCOMPRASRenglonRECEPCIONES(MyConn, lblInfo, Insertar, Documento, numRenglon, CodigoProveedorAnterior, txtCodigo.Text, _
                                                        txtDescripcion.Text, txtIVA.Text, "", cmbUnidad.Text, ValorCantidad(txtCantidad.Text), _
                                                        ValorCantidad(txtPesoTotal.Text), ValorCantidad(txtCantidad.Text), cmbTipoRenglon.SelectedIndex, _
                                                        ValorNumero(txtCostoPrecio.Text), ValorNumero(txtDesc_art.Text), ValorNumero(txtDesc_cli.Text), _
                                                        ValorNumero(txtCostoPrecioTotal.Text), ValorNumero(txtCostoPrecioTotal.Text), txtLote.Text, "", "", "", "1")
                Case "GAS"
                    InsertEditCOMPRASRenglonGASTOS(MyConn, lblInfo, Insertar, Documento, numRenglon, CodigoProveedorAnterior, txtCodigo.Text, _
                                                   txtDescripcion.Text, txtIVA.Text, "", cmbUnidad.Text, 0.0, ValorCantidad(txtCantidad.Text), _
                                                   ValorCantidad(txtPesoTotal.Text), txtLote.Text, "0", ValorNumero(txtCostoPrecio.Text), _
                                                   ValorNumero(txtDesc_art.Text), ValorNumero(txtDesc_cli.Text), ValorNumero(txtCostoPrecioTotal.Text), _
                                                   ValorNumero(txtCostoPrecioTotal.Text), txtCausa.Text, "", "1")

                Case "COM"
                    InsertEditCOMPRASRenglonCOMPRAS(MyConn, lblInfo, Insertar, Documento, numRenglon, CodigoProveedorAnterior, txtCodigo.Text, _
                                                    txtDescripcion.Text, txtIVA.Text, "", cmbUnidad.Text, 0.0, ValorCantidad(txtCantidad.Text), _
                                                    ValorCantidad(txtPesoTotal.Text), txtLote.Text, "", "", cmbTipoRenglon.SelectedIndex, _
                                                    ValorNumero(txtCostoPrecio.Text), ValorNumero(txtDesc_art.Text), ValorNumero(txtDesc_cli.Text), _
                                                    ValorNumero(txtCostoPrecioTotal.Text), ValorNumero(txtCostoPrecioTotal.Text), "", "", "", "", "", "1")
                Case "NCC"
                    InsertEditCOMPRASRenglonNOTACREDITO(MyConn, lblInfo, Insertar, Documento, numRenglon, CodigoProveedorAnterior, txtCodigo.Text, _
                                                        txtDescripcion.Text, txtIVA.Text, "", cmbUnidad.Text, ValorCantidad(txtCantidad.Text), ValorCantidad(txtPesoTotal.Text), _
                                                        txtLote.Text, cmbTipoRenglon.SelectedIndex, ValorNumero(txtCostoPrecio.Text), ValorNumero(txtDesc_art.Text), _
                                                        ValorNumero(txtDesc_cli.Text), ValorNumero(txtPorAceptaDev.Text), ValorNumero(txtCostoPrecioTotal.Text), _
                                                        ValorNumero(txtCostoPrecioTotal.Text), txtFactura.Text, txtCausa.Text, "", 0, "1")
                Case "NDC"
                    InsertEditCOMPRASRenglonNOTADEBITO(MyConn, lblInfo, Insertar, Documento, numRenglon, CodigoProveedorAnterior, txtCodigo.Text, _
                                                        txtDescripcion.Text, txtIVA.Text, "", cmbUnidad.Text, ValorCantidad(txtCantidad.Text), ValorCantidad(txtPesoTotal.Text), _
                                                        txtLote.Text, cmbTipoRenglon.SelectedIndex, ValorNumero(txtCostoPrecio.Text), ValorNumero(txtDesc_art.Text), _
                                                        ValorNumero(txtDesc_cli.Text), ValorNumero(txtCostoPrecio.Text), ValorNumero(txtCostoPrecioTotal.Text), _
                                                        txtFactura.Text, txtCausa.Text, "", 0, "1")

            End Select
            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo & nModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.Click, _
        txtCantidad.Click, txtCostoPrecio.Click
        Dim txt As TextBox = sender
        EnfocarTexto(txt)
    End Sub

    Private Sub cmbUnidad_SelectedItemChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        cmbUnidad.SelectedIndexChanged

        If txtCodigo.Text.Substring(0, 1) = "$" Then
            Dim afld() As String = {"codser", "id_emp"}
            Dim aStr() As String = {txtCodigo.Text.Substring(1, txtCodigo.Text.Length - 1), jytsistema.WorkID}

            Equivalencia = 1

            txtIVA.Text = qFoundAndSign(MyConn, lblInfo, "jsmercatser", afld, aStr, "TIPOIVA")
            PesoActual = 0
            CostoActual = ValorNumero(qFoundAndSign(MyConn, lblInfo, "jsmercatser", afld, aStr, "precio"))

            txtCostoPrecio.Text = ft.FormatoNumero(CostoActual)
            txtDescripcion.Text = qFoundAndSign(MyConn, lblInfo, "jsmercatser", afld, aStr, "desser")

        Else
            Dim afld() As String = {"codart", "id_emp"}
            Dim aStr() As String = {txtCodigo.Text, jytsistema.WorkID}

            Dim afldX() As String = {"codart", "uvalencia", "id_emp"}
            Dim aStrX() As String = {txtCodigo.Text, CType(cmbUnidad.SelectedItem, String), jytsistema.WorkID}

            Equivalencia = FuncionesMercancias.Equivalencia(MyConn, lblInfo, txtCodigo.Text, aUnidades(cmbUnidad.SelectedIndex))
            aPrecios = ArregloPreciosPlus(MyConn, lblInfo, txtCodigo.Text, CodigoCliente, Equivalencia, CodigoAsesor, TipoVendedor.iFuerzaventa, Fechadocumento)
            aDescuentosMercancias = ArregloDescuentos(MyConn, lblInfo, txtCodigo.Text)
            RellenaCombo(DeleteArrayValue(aPrecios, "0.00"), cmbCostoPrecio)

            If cmbCostoPrecio.SelectedIndex >= 0 Then
                txtCostoPrecio.Text = cmbCostoPrecio.SelectedText
            Else
                txtCostoPrecio.Text = ft.FormatoNumero(0.0)
            End If

            txtIVA.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "IVA")
            PesoActual = ValorNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "pesounidad")) / IIf(Equivalencia = 0, 1, Equivalencia)

            Select Case nModulo
                Case "TRF", "ORD", "REC", "GAS", "COM", "NCC", "NDC"
                    CostoActual = ValorNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "montoultimacompra"))
                Case "PVE", "COT", "APT", "PPE", "PED", "PFC", "FAC", "NCV", "NDV"
                    CostoActual = ValorNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "precio_" & TarifaActual))
            End Select

            AceptaDescuento = CBool(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "descuento"))

            If Not AceptaDescuento Then
                If InStr("PVE.COT.PPE.PED.PFC.FAC.NDV", nModulo) > 0 Then
                    cmbTipoRenglon.SelectedIndex = 1
                    cmbTipoRenglon.Enabled = False
                Else
                    cmbTipoRenglon.SelectedIndex = 0
                    cmbTipoRenglon.Enabled = True
                    cmbTipoRenglon.Visible = True
                End If
            Else
                If InStr("PVE.COT.PPE.PED.PFC.FAC.NDV", nModulo) > 0 Then ColocaDescuentoOferta()
                If cmbCostoPrecio.SelectedIndex >= 0 Then
                    txtDesc_art.Text = ft.FormatoNumero(aDescuentosMercancias(cmbCostoPrecio.SelectedIndex))
                Else
                    txtDesc_art.Text = ft.FormatoNumero(0.0)
                End If
            End If


            txtCostoPrecio.Text = ft.FormatoNumero(CostoActual / IIf(Equivalencia = 0, 1, Equivalencia))
            txtDescripcion.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "nomart")

        End If


    End Sub

    Private Sub txtIVA_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIVA.TextChanged
        txtPorIVA.Text = ft.FormatoNumero(PorcentajeIVA(MyConn, lblInfo, Fechadocumento, txtIVA.Text)) & "%"
    End Sub

    Private Sub txtCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCantidad.KeyPress, _
        txtCostoPrecio.KeyPress, txtDesc_art.KeyPress, txtDesc_cli.KeyPress, txtDesc_ofe.KeyPress, txtPorAceptaDev.KeyPress
        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub

    Private Sub txtCantidad_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCantidad.TextChanged, _
        txtCostoPrecio.TextChanged, txtDesc_art.TextChanged, txtDesc_cli.TextChanged, txtDesc_ofe.TextChanged, txtPorAceptaDev.TextChanged

        txtCostoPrecioTotal.Text = ft.FormatoNumero(PrecioTotal(ValorCantidad(txtCantidad.Text), ValorNumero(txtCostoPrecio.Text), _
                                                             ValorNumero(txtPorAceptaDev.Text), ValorNumero(txtDesc_art.Text), _
                                                             ValorNumero(txtDesc_cli.Text), ValorNumero(txtDesc_ofe.Text)))

        txtPesoTotal.Text = ft.FormatoCantidad(PesoActual * ValorCantidad(txtCantidad.Text))

        txtPrecioIVA.Text = ft.FormatoNumero(ValorNumero(txtCostoPrecio.Text) * (1 + PorcentajeIVA(MyConn, lblInfo, Fechadocumento, txtIVA.Text) / 100))
        txtTotalIVA.Text = ft.FormatoNumero(ValorNumero(txtCostoPrecioTotal.Text) * (1 + PorcentajeIVA(MyConn, lblInfo, Fechadocumento, txtIVA.Text) / 100))

    End Sub

    Private Function PrecioTotal(ByVal Cantidad As Double, ByVal PrecioCosto As Double, ByVal PorcentajeAceptacion As Double, _
                                 ByVal PorcentajeDescuentoArticulo As Double, ByVal PorcentajeDescuentoCliente As Double, _
                                 ByVal PorcentajeDescuentoOferta As Double) As Double
        Dim PrecioNeto As Double = Cantidad * PrecioCosto * IIf(nModulo = "NCV" Or nModulo = "NCC", PorcentajeAceptacion / 100, 1)

        Dim DescuentoArticulo As Double = 0.0
        Dim DescuentoCliente As Double = 0.0
        Dim DescuentoOferta As Double = 0.0

        Select Case nModulo
            Case "NCV", "NCC"
            Case Else
                DescuentoArticulo = PrecioNeto * PorcentajeDescuentoArticulo / 100
                DescuentoCliente = (PrecioNeto - DescuentoArticulo) * PorcentajeDescuentoCliente / 100
                DescuentoOferta = (PrecioNeto - DescuentoArticulo - DescuentoCliente) * PorcentajeDescuentoOferta / 100
        End Select

        PrecioTotal = PrecioNeto - DescuentoArticulo - DescuentoCliente - DescuentoOferta

    End Function

    Private Sub txtCodigo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigo.TextChanged
        'txtFactura.Text = ""
        'txtLote.Text = ""
        If EsServicio Then
            If txtCodigo.Text <> "" Then
                If txtCodigo.Text.Substring(0, 1) = "$" Then
                    Dim afld() As String = {"codser", "id_emp"}
                    Dim aStr() As String = {txtCodigo.Text.Substring(1, txtCodigo.Text.Length - 1), jytsistema.WorkID}
                    If qFound(MyConn, lblInfo, "jsmercatser", afld, aStr) Then
                        Dim aUND() As String = {"UND"}
                        Dim aPRE() As String = {qFoundAndSign(MyConn, lblInfo, "jsmercatser", afld, aStr, "PRECIO")}
                        aUnidades = aUND
                        aPrecios = aPRE
                        RellenaCombo(aUnidades, cmbUnidad)
                        RellenaCombo(aPrecios, cmbCostoPrecio)
                        CodigoBarras = ""
                        If InStr("NCV.NCC", nModulo) > 0 Then txtPorAceptaDev.Text = ft.FormatoNumero(100)
                    Else
                        ItemNoEncontrado()
                    End If
                Else
                    ItemNoEncontrado()
                End If
            End If
        Else

            Dim afld() As String = {"codart", "ESTATUS", "id_emp"}
            Dim aStr() As String = {txtCodigo.Text, "0", jytsistema.WorkID}
            Dim aP() As String = {"A", "B", "C", "D", "E", "F"}
            If qFound(MyConn, lblInfo, "jsmerctainv", afld, aStr) Then

                aUnidades = ArregloUnidades(MyConn, lblInfo, txtCodigo.Text, ft.DevuelveScalarCadena(MyConn, " select UNIDAD from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                'RellenaCombo(aUnidades, cmbUnidad)
                Dim equiv As Double = 0.0 ' FuncionesMercancias.Equivalencia(MyConn, lblInfo, txtCodigo.Text, aUnidades(cmbUnidad.SelectedIndex))
                Dim UnidadDetalX As String = ft.DevuelveScalarCadena(MyConn, " select UNIDADDETAL from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                If nModulo = "PVE" Then
                    Dim ItemDefecto As Integer = ft.InArray(aUnidades, UnidadDetalX)
                    RellenaCombo(aUnidades, cmbUnidad, IIf(ItemDefecto >= 0, ItemDefecto, 0))
                    equiv = FuncionesMercancias.Equivalencia(MyConn, lblInfo, txtCodigo.Text, aUnidades(cmbUnidad.SelectedIndex))
                    aPrecios = ArregloPreciosPlus(MyConn, lblInfo, txtCodigo.Text, CodigoCliente, equiv, CodigoAsesor, TipoVendedor.iCajeros, Fechadocumento)
                    aDescuentosMercancias = ArregloDescuentosPlus(MyConn, lblInfo, txtCodigo.Text, CodigoCliente, equiv, CodigoAsesor, TipoVendedor.iCajeros)
                Else
                    RellenaCombo(aUnidades, cmbUnidad)
                    equiv = FuncionesMercancias.Equivalencia(MyConn, lblInfo, txtCodigo.Text, aUnidades(cmbUnidad.SelectedIndex))
                    aPrecios = ArregloPreciosPlus(MyConn, lblInfo, txtCodigo.Text, CodigoCliente, equiv, CodigoAsesor, TipoVendedor.iFuerzaventa, Fechadocumento)
                    aDescuentosMercancias = ArregloDescuentosPlus(MyConn, lblInfo, txtCodigo.Text, CodigoCliente, equiv, CodigoAsesor, TipoVendedor.iFuerzaventa)
                End If

                RellenaCombo(DeleteArrayValue(aPrecios, "0.00"), cmbCostoPrecio)

                CodigoBarras = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "barras")
                CodigoProveedor = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "ultimoproveedor")
                AceptaDescuento = CBool(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "descuento"))

                If TipoFacturacionCliente = FacturaAPartirDe.iCostos Then AceptaDescuento = False
                If MercanciaRegulada(MyConn, lblInfo, txtCodigo.Text) Then AceptaDescuento = False

                If Not AceptaDescuento Then
                    If InStr("PVE.COT.PPE.PED.PFC.FAC.NDV", nModulo) > 0 Then
                        txtDesc_art.Text = ft.FormatoNumero(0.0)
                        txtDesc_cli.Text = ft.FormatoNumero(0.0)
                        txtDesc_ofe.Text = ft.FormatoNumero(0.0)
                        HabilitarObjetos(False, True, txtDesc_art, txtDesc_cli, txtDesc_ofe)
                        cmbTipoRenglon.SelectedIndex = 1
                        cmbTipoRenglon.Enabled = False
                    Else
                        cmbTipoRenglon.SelectedIndex = 0
                        cmbTipoRenglon.Enabled = True
                        cmbTipoRenglon.Visible = True
                    End If
                Else
                    If InStr("PVE.COT.PPE.PED.PFC.FAC.NDV", nModulo) > 0 Then ColocaDescuentoOferta()
                    If UBound(aDescuentosMercancias) > 0 Then
                        txtDesc_art.Text = ft.FormatoNumero(aDescuentosMercancias(cmbCostoPrecio.SelectedIndex))
                    Else
                        txtDesc_art.Text = ft.FormatoNumero(0.0)
                    End If
                End If

                If InStr("NCV.NCC", nModulo) > 0 Then txtPorAceptaDev.Text = ft.FormatoNumero(CDbl(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "por_acepta_dev")))

            Else
                ItemNoEncontrado()
            End If

        End If

    End Sub

    Private Sub ColocaDescuentoOferta()

        Dim OtorgaOferta As Double
        If txtCodigo.Text <> "" Then
            If txtCodigo.Text.Substring(0, 1) <> "$" Then
                If nModulo <> "NCR" Then
                    If ft.DevuelveScalarCadena(MyConn, " select codart from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") = txtCodigo.Text Then
                        Dim TarifaVenta As String = Tarifa()
                        Dim DescuentoOferta As Double = 0.0#
                        DescuentoOferta = PorcentajeDescuentoOferta(MyConn, Fechadocumento, _
                            txtCodigo.Text, cmbUnidad.Text, ValorCantidad(txtCantidad.Text) / FuncionesMercancias.Equivalencia(MyConn, lblInfo, txtCodigo.Text, cmbUnidad.Text), TarifaVenta, lblInfo)
                        txtDesc_ofe.Text = ft.FormatoNumero(DescuentoOferta)
                        DescuentoOfertaMaximo = DescuentoOferta
                        If DescuentoOferta <> 0 Then
                            OtorgaOferta = OtorgaPorcentajeOferta(MyConn, lblInfo, Fechadocumento, txtCodigo.Text, _
                               cmbUnidad.Text, ValorCantidad(txtCantidad.Text) / FuncionesMercancias.Equivalencia(MyConn, lblInfo, txtCodigo.Text, cmbUnidad.Text), TarifaVenta)
                            If OtorgaOferta = 0 Then 'Jytsuite
                                txtDesc_ofe.Enabled = False
                                txtComentarioOferta.Text = "Dscto. otorgado x JytSuite"
                            Else 'Asesor Comercial
                                txtDesc_ofe.Enabled = True
                                txtComentarioOferta.Text = "Dscto. otorgado x Asesor Comercial"
                            End If
                        Else
                            txtComentarioOferta.Text = ""
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Function OtorgaPorcentajeOferta(MyConn As MySqlConnection, lblInfo As Label, Fecha As Date, _
        CodigoMercancia As String, UnidadVenta As String, Cantidad As Double, TarifaO As String) As Boolean

        OtorgaPorcentajeOferta = ft.DevuelveScalarBooleano(MyConn, " SELECT b.otorgapor FROM jsmerencofe a, jsmerrenofe b  " _
                                                            & " WHERE " _
                                                            & " a.CODOFE = b.CODOFE AND " _
                                                            & " a.DESDE <= '" & ft.FormatoFechaMySQL(Fecha) & "' AND " _
                                                            & " a.HASTA >= '" & ft.FormatoFechaMySQL(Fecha) & "' AND " _
                                                            & " b.CODART = '" & CodigoMercancia & "' AND " _
                                                            & " b.UNIDAD = '" & UnidadVenta & "' AND " _
                                                            & " b.LIMITEI <= " & Cantidad & " AND " _
                                                            & " b.LIMITES >= " & Cantidad & " AND " _
                                                            & " a.TARIFA_" & TarifaO & " = '1' AND " _
                                                            & " b.ID_EMP = '" & jytsistema.WorkID & "' AND " _
                                                            & " a.ID_EMP = '" & jytsistema.WorkID & "' ORDER BY DESDE DESC LIMIT 1 ")

    End Function

    Private Function Tarifa() As String
        Tarifa = "A"
        If txtCodigo.Text <> "" Then
            If txtCodigo.Text.Substring(0, 1) <> "$" Then
                If ft.DevuelveScalarCadena(MyConn, " select codart from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") = txtCodigo.Text Then
                    Dim rEqu As Double = FuncionesMercancias.Equivalencia(MyConn, lblInfo, txtCodigo.Text, cmbUnidad.Text)
                    If cmbCostoPrecio.Visible Then

                        If ValorNumero(txtCostoPrecio.Text) = ft.DevuelveScalarDoble(MyConn, " select precio_A from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") / rEqu Then Tarifa = "A"
                        If ValorNumero(txtCostoPrecio.Text) = ft.DevuelveScalarDoble(MyConn, " select precio_B from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") / rEqu Then Tarifa = "B"
                        If ValorNumero(txtCostoPrecio.Text) = ft.DevuelveScalarDoble(MyConn, " select precio_C from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") / rEqu Then Tarifa = "C"
                        If ValorNumero(txtCostoPrecio.Text) = ft.DevuelveScalarDoble(MyConn, " select precio_D from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") / rEqu Then Tarifa = "D"
                        If ValorNumero(txtCostoPrecio.Text) = ft.DevuelveScalarDoble(MyConn, " select precio_E from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") / rEqu Then Tarifa = "E"
                        If ValorNumero(txtCostoPrecio.Text) = ft.DevuelveScalarDoble(MyConn, " select precio_F from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") / rEqu Then Tarifa = "F"
                    Else
                        If ValorNumero(txtCostoPrecio.Text) <= ft.DevuelveScalarDoble(MyConn, " select precio_A from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") / rEqu Then Tarifa = "A"
                        If ValorNumero(txtCostoPrecio.Text) <= ft.DevuelveScalarDoble(MyConn, " select precio_B from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") / rEqu Then Tarifa = "B"
                        If ValorNumero(txtCostoPrecio.Text) <= ft.DevuelveScalarDoble(MyConn, " select precio_C from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") / rEqu Then Tarifa = "C"
                        If ValorNumero(txtCostoPrecio.Text) <= ft.DevuelveScalarDoble(MyConn, " select precio_D from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") / rEqu Then Tarifa = "D"
                        If ValorNumero(txtCostoPrecio.Text) <= ft.DevuelveScalarDoble(MyConn, " select precio_E from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") / rEqu Then Tarifa = "E"
                        If ValorNumero(txtCostoPrecio.Text) <= ft.DevuelveScalarDoble(MyConn, " select precio_F from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") / rEqu Then Tarifa = "F"
                    End If
                End If

            Else
                Tarifa = "A"
            End If
        End If
    End Function

    Private Sub ItemNoEncontrado()
        cmbUnidad.Items.Clear()
        cmbCostoPrecio.Items.Clear()
        CodigoBarras = ""
        txtDescripcion.Text = ""
        txtCostoPrecio.Text = ft.FormatoNumero(0.0)
        txtCostoPrecioTotal.Text = ft.FormatoNumero(0.0)
        txtPesoTotal.Text = ft.FormatoCantidad(0.0)
        txtPrecioIVA.Text = txtCostoPrecio.Text
        txtTotalIVA.Text = txtCostoPrecioTotal.Text
        AceptaDescuento = True
    End Sub
    Private Sub btnCodigo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigo.Click
        If EsServicio Then
            Dim g As New jsMerArcServiciosA
            g.Cargar(MyConn)
            txtCodigo.Text = g.Seleccionado
            g = Nothing
        Else

            Dim f As New jsMerArcListaCostosPreciosNormal
            Select Case nModulo
                Case "TRF", "REC", "GAS", "COM", "NCC", "NDC"
                    f.Cargar(MyConn, TipoListaPrecios.Costos, "", ProveedorLista)
                    txtCodigo.Text = f.Seleccionado
                Case "PVE", "COT", "PPE", "PED", "PFC", "FAC", "NCV", "NDV"
                    f.Cargar(MyConn, TipoListaPrecios.Precios_IVA, "", , TarifaActual, 0)
                    txtCodigo.Text = f.Seleccionado
                Case "ORD"
                    f.Cargar(MyConn, TipoListaPrecios.CostosSugerido, , ProveedorLista)
                    txtCodigo.Text = f.Seleccionado
            End Select
            f = Nothing
        End If
    End Sub

    Private Sub cmbCostoPrecio_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCostoPrecio.SelectedIndexChanged
        txtCostoPrecio.Text = cmbCostoPrecio.SelectedItem
        If UBound(aDescuentosMercancias) > 0 Then
            txtDesc_art.Text = ft.FormatoNumero(aDescuentosMercancias(cmbCostoPrecio.SelectedIndex))
        Else
            txtDesc_art.Text = ft.FormatoNumero(0.0)
        End If
    End Sub

    Private Sub btnDescripcion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDescripcion.Click
        If txtDescripcion.Text <> "" Then
            Dim g As New jsGenComentariosRenglones
            Dim aFld() As String = {"numdoc", "origen", "item", "renglon", "id_emp"}
            Dim aStr() As String = {Documento, nModulo, txtCodigo.Text, numRenglon, jytsistema.WorkID}

            If qFound(MyConn, lblInfo, "jsvenrencom", aFld, aStr) Then
                g.Editar(MyConn, Documento, nModulo, txtCodigo.Text, numRenglon)
            Else
                g.Agregar(MyConn, Documento, nModulo, txtCodigo.Text, numRenglon)
            End If
            g = Nothing
        End If
    End Sub

    Private Sub btnLote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLote.Click
        If txtDescripcion.Text <> "" Then
            Dim f As New jsMerArcLotesMercancia
            f.Cargar(MyConn, txtCodigo.Text)
            If f.Seleccion <> "" Then txtLote.Text = f.Seleccion
            f = Nothing
        End If
    End Sub


    Private Sub btnCausa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCausa.Click
        Dim f As New jsControlArcCausasCreditos
        f.Cargar(MyConn, IIf(InStr("NCV.NCC", nModulo) > 0, 0, 1), TipoCargaFormulario.iShowDialog)
        If f.Seleccionado <> "" Then txtCausa.Text = f.Seleccionado
        f = Nothing
    End Sub
    Private Sub txtCausa_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCausa.TextChanged

        Dim aVal As String = ConvertirIntegerEnSiNo(ft.DevuelveScalarEntero(MyConn, "select validaunidad from jsvencaudcr where codigo = '" & txtCausa.Text _
                              & "' and credito_debito = '" & IIf(InStr("NCV.NCC", nModulo) > 0, 0, 1) & "' and id_emp = '" & jytsistema.WorkID & "' ")) & " valida cantidad por unidad de venta"

        ValidaMultiplosDeUnidadDeVenta = ft.DevuelveScalarBooleano(MyConn, "select validaunidad from jsvencaudcr where codigo = '" & txtCausa.Text _
                              & "' and credito_debito = '" & IIf(InStr("NCV.NCC", nModulo) > 0, 0, 1) & "' and id_emp = '" & jytsistema.WorkID & "' ")

        Dim aAju As String = ConvertirIntegerEnSiNo(ft.DevuelveScalarEntero(MyConn, "select ajustaprecio from jsvencaudcr where codigo = '" & txtCausa.Text _
                              & "' and credito_debito = '" & IIf(InStr("NCV.NCC", nModulo) > 0, 0, 1) & "' and id_emp = '" & jytsistema.WorkID & "' ")) & " causa ajuste en los costos de mercancía"
        Dim aEst As String = ConvertirIntegerEnSiNo(ft.DevuelveScalarEntero(MyConn, "select estado from jsvencaudcr where codigo = '" & txtCausa.Text _
                              & "' and credito_debito = '" & IIf(InStr("NCV.NCC", nModulo) > 0, 0, 1) & "' and id_emp = '" & jytsistema.WorkID & "' ")) & " es mercancía en buen estado"
        Dim aInv As String = ConvertirIntegerEnSiNo(ft.DevuelveScalarEntero(MyConn, "select inventario from jsvencaudcr where codigo = '" & txtCausa.Text _
                              & "' and credito_debito = '" & IIf(InStr("NCV.NCC", nModulo) > 0, 0, 1) & "' and id_emp = '" & jytsistema.WorkID & "' ")) & " causa movimiento en inventario "

        lblCausaDEs.Text = aInv & "; " & aVal & "; " & aAju & "; " & aEst

    End Sub

    Private Sub btnFactura_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFactura.Click

        If txtDescripcion.Text <> "" Then
            Select Case nModulo
                Case "NCV", "NDV"
                    Dim f As New jsMerArcListadoItemsEnFacturas
                    f.Cargar(MyConn, CodigoCliente, CodigoAsesor, txtCodigo.Text)
                    If f.Seleccion <> "" Then
                        txtFactura.Text = f.Seleccion
                        cmbUnidad.SelectedIndex = ft.InArray(aUnidades, f.Unidad)
                        txtCostoPrecio.Text = ft.FormatoNumero(f.Precio)
                        HabilitarObjetos(False, True, cmbUnidad, txtCostoPrecio)
                    End If
                    f = Nothing
                Case "NCC", "NDC"
                    Dim f As New jsMerArcListadoItemsEnCompras
                    f.Cargar(MyConn, CodigoProveedorAnterior, txtCodigo.Text)
                    If f.Seleccion <> "" Then
                        txtFactura.Text = f.Seleccion
                        cmbUnidad.SelectedIndex = ft.InArray(aUnidades, f.Unidad)
                        txtCostoPrecio.Text = ft.FormatoNumero(f.Precio)
                    End If
                    f = Nothing
            End Select


        End If

    End Sub

    Private Sub CargarPrecios(CodigoMercancia As String, nModulo As String)

        Dim origenPrecios() As String
        If CodigoMercancia.Substring(1, 1) = "$" Then
            origenPrecios = {"PRECIO"}
        Else
            Select Case nModulo
                Case "TRF", "ORD", "REC", "GAS", "COM", "NCC", "NDC"
                    origenPrecios = {"MONTOULTIMACOMPRA"}
                Case "PVE", "COT", "PPE", "PED", "APT", "PFC", "FAC", "NCV", "NDV"
                    origenPrecios = {"PRECIO_A", "PRECIO_B", "PRECIO_C", "PRECIO_D", "PRECIO_E", "PRECIO_F"}
            End Select
        End If

    End Sub

    Private Sub IniciarMatrixDePrecios(CodigoDeMercancia As String)

        Dim letraPrecio() As String = {"A", "B", "C", "D", "E", "F"}
        For iCont As Integer = 0 To 5
            aPreciosMercancias(iCont, 0) = ft.DevuelveScalarDoble(MyConn, " select PRECIO_" & letraPrecio(iCont) & " FROM jsmerctainv WHERE CODART = '" & CodigoDeMercancia & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
            aPreciosMercancias(iCont, 1) = ft.DevuelveScalarDoble(MyConn, " select DESC_" & letraPrecio(iCont) & " FROM jsmerctainv WHERE CODART = '" & CodigoDeMercancia & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
            aPreciosMercancias(iCont, 2) = ft.DevuelveScalarCadena(MyConn, " select OFERTA_" & letraPrecio(iCont) & " FROM jsmerctainv WHERE CODART = '" & CodigoDeMercancia & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
        Next

    End Sub


End Class