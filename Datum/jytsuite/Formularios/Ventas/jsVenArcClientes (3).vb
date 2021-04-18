Imports MySql.Data.MySqlClient
Imports C1.Win.C1Chart
Public Class jsVenArcClientes
    Private Const sModulo As String = "Clientes y CxC"
    Private Const lRegion As String = "RibbonButton30"
    Private Const nTabla As String = "clientes"
    Private Const nTablaMovimientos As String = "movimientos"
    Private Const nTablaCIs As String = "tblCIs"
    Private Const nTablaSaldos As String = "tblSaldosDocs"
    Private Const nTablaVisitas As String = "tblVisitas"
    Private Const nTablaDespachos As String = "tblDespachos"
    Private Const nTablaRutasVisita As String = "tblrutavisita"
    Private Const nTablaRutasDespacho As String = "tblrutaDespacho"
    Private Const nTablaProveedores As String = "tblProveedores"

    Private strSQL As String = "select * from jsvencatcli where id_emp = '" & jytsistema.WorkID & "' order by codcli "
    Private strSQLMov As String = ""
    Private strSQLCIs As String = ""
    Private strSQLIVA As String = " select tipo from jsconctaiva group by tipo "
    Private strSQLSaldos As String = ""
    Private strSQLVisitas As String = ""
    Private strSQLDespachos As String = ""
    Private strSQLRutasVisita As String = ""
    Private strSQLRutasDespacho As String = ""
    Private strSQLProveedores As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private myCom As New MySqlCommand(strSQL, myConn)
    Private da As New MySqlDataAdapter(myCom)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtMovimientos As New DataTable
    Private dtSaldos As New DataTable
    Private dtCIs As New DataTable
    Private dtVisitas As New DataTable
    Private dtDespachos As New DataTable
    Private dtRutasV As New DataTable
    Private dtRutasD As New DataTable
    Private dtProveedores As New DataTable

    Private aIVA() As String
    Private aCondicion() As String = {"Activo", "Bloqueado", "Inactivo", "Desincorporado"}
    Private aFrecuenciaVisita() As String = {"Diaria", "Semanal", "Quincenal", "Mensual", "Bimensual", "Trimestral", "Otra"}
    Private aDiaPago() = {"Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sabado", "Domingo"}
    Private aContribuyente() As String = {"Contribuyente ordinario", "Contribuyente especial", "Contribuyente formal", "No Contribuyente"}
    Private aShare() As String = {"Exclusivo", "Compartido", "Potencial"}
    Private aRanking() As String = {"Regular", "Bueno", "Excelente"}
    Private aSaldos() As String = {"Actuales", "Históricos"}
    Private aEstadistica() As String = {"Ventas", "Devoluciones"}
    Private aFacturaDesde() As String = {"Precio", "Costo"}

    Private aTipoMovimiento() As String = {"Todos", "Débitos", "Créditos", "Facturas", "Giros", "Notas Débito", "Abonos", "Cancelaciones", "Notas Crédito"}

    Private i_modo As Integer
    Private nPosicionCat As Long, nPosicionMov As Long, nPocisionCI As Long
    Private nPosicionEst As Long, nPosicionProveedor As Long
    Private nPosicionVisitas As Long, nPosicionDespachos As Long
    Private nPosicionRutasVisita As Long, nPosicionRutasDespacho As Long

    Private iSel As Integer = 0
    Private dSel As Double = 0.0
    Private strSel As String = " nummov = '' OR "
    Private LimiteCreditoAnterior As Double = 0.0

    Private tblSaldos As String = ""

    Private Sub jsVenArcClientes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)

            InsertarAuditoria(myConn, MovAud.ientrar, sModulo, "")
            RellenaCombo(aSaldos, cmbSaldos)

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaTXT(nPosicionCat)
            Else
                IniciarCliente(False)
            End If
            ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub

    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo registro")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> registro deseado")
        C1SuperTooltip1.SetToolTip(btnSeleccionar, "<B>Seleccionar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir al <B>primer</B> registro")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir al <B>siguiente</B> registro")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir al registro <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir al <B>último registro</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B>")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnSubirHistorico, "<B>Subir</B> movimientos a histórico")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Bajar</B> movimientos de histórico")


        'Botones Adicionales
        C1SuperTooltip1.SetToolTip(btnCanal, "Seleccione la <B>canal de distribución y tipo de negocio</B> para esta cliente")
        C1SuperTooltip1.SetToolTip(btnTerritorioFiscal, "Seleccione el <B>Territorio de la dirección fiscal</B> para este cliente ")
        C1SuperTooltip1.SetToolTip(btnTerritorioDespacho, "Seleccione la <B>Territorio de la dirección de despacho</B> para esta cliente ")
        C1SuperTooltip1.SetToolTip(btnIVA, "Agregue o modifique las diferentes <B>tasas de IVA</B> ")
        C1SuperTooltip1.SetToolTip(btnForma, "Seleccione la <B>forma de pago</B> de este cliente")
        C1SuperTooltip1.SetToolTip(btnListaPrecios, "Seleccione una <B>lista de precios especial</B> para este cliente")
        C1SuperTooltip1.SetToolTip(btnInicioVisita, "Seleccione fecha para el <B>inicio de visitas</B> ")
        C1SuperTooltip1.SetToolTip(btnIngreso, "Selecione fecha de <B>ingreso</B> al sistema ")

        C1Chart2.ToolTip.Enabled = True
        C1Chart2.ToolTip.SelectAction = SelectActionEnum.MouseOver
        C1Chart2.ToolTip.PlotElement = PlotElementEnum.Points
        C1Chart2.ToolTip.AutomaticDelay = 0
        C1Chart2.ToolTip.InitialDelay = 0


    End Sub
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        dtMovimientos = ds.Tables(nTablaMovimientos)
        If c >= 0 AndAlso dtMovimientos.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaMovimientos).Position = c
            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", c, dtMovimientos.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtMovimientos, MenuBarra)
        HabilitarObjetos(True, False, btnSubirHistorico, btnBajarHistorico)

        txtSaldoActual.Text = FormatoNumero(SaldoCxC(myConn, lblInfo, txtCodigo.Text))
        txtUltimoPago.Text = FormatoNumero(Math.Abs(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(importe) importe from jsventracob where codcli = '" & txtCodigo.Text & "' and " _
                            & " tipomov in ('AB','CA', 'ND') and " _
                            & " comproba <> '' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' " _
                            & " group by comproba " _
                            & " order by emision desc limit 1 "))))

        Dim FechaUltimoPago As String = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select emision from jsventracob where codcli = '" & txtCodigo.Text & "' and " _
                            & " tipomov in ('AB','CA', 'ND') and " _
                            & " comproba <> '' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' " _
                            & " group by comproba " _
                            & " order by emision desc limit 1 ").ToString()
        If FechaUltimoPago = "0" Then
            txtFechaUltimoPago.Text = ""
        Else
            txtFechaUltimoPago.Text = FormatoFecha(CDate(FechaUltimoPago))
        End If

        Dim UltimaFormaDePago As String = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select formapag from jsventracob where codcli = '" & txtCodigo.Text & "' and " _
                            & " tipomov in ('AB','CA', 'ND') and " _
                            & " comproba <> '' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' " _
                            & " group by comproba " _
                            & " order by emision desc limit 1 ")

        If InArray(aFormaPagoAbreviada, UltimaFormaDePago) > 0 Then
            txtFormaUltimoPago.Text = aFormaPago(InArray(aFormaPagoAbreviada, UltimaFormaDePago) - 1)
        Else
            txtFormaUltimoPago.Text = ""
        End If

    End Sub
    Private Sub AsignaCI(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLCIs, myConn, nTablaCIs, lblInfo)
        dtCIs = ds.Tables(nTablaCIs)
        If c >= 0 AndAlso dgCIs.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaCIs).Position = c
            dgCIs.Refresh()
            dgCIs.CurrentCell = dgCIs(0, c)
        End If
    End Sub

    Private Sub AsignaProveedores(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLProveedores, myConn, nTablaProveedores, lblInfo)
        dtProveedores = ds.Tables(nTablaProveedores)
        If c >= 0 AndAlso dtProveedores.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaProveedores).Position = c
            dgProveedores.Refresh()
            dgProveedores.CurrentCell = dgProveedores(0, c)
        End If
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        If dt.Rows.Count > 0 Then

            With dt

                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

                With .Rows(nRow)

                    'Cliente i
                    nPosicionCat = nRow

                    txtCodigo.Text = MuestraCampoTexto(.Item("codcli"))
                    txtNombre.Text = MuestraCampoTexto(.Item("nombre"))
                    txtAlterno.Text = MuestraCampoTexto(.Item("alterno"))
                    txtRIF.Text = MuestraCampoTexto(.Item("rif"))
                    txtCanal.Text = MuestraCampoTexto(.Item("categoria"))
                    txtTipoNegocio.Text = MuestraCampoTexto(.Item("unidad"))

                    txtDireccionFiscal.Text = MuestraCampoTexto(.Item("dirfiscal"))
                    txtPaisFiscal.Text = MuestraCampoTexto(.Item("fpais"))
                    txtMunicipioFiscal.Text = MuestraCampoTexto(.Item("fMunicipio"))
                    txtEstadoFiscal.Text = MuestraCampoTexto(.Item("fEstado"))
                    txtParroquiaFiscal.Text = MuestraCampoTexto(.Item("fparroquia"))
                    txtCiudadFiscal.Text = MuestraCampoTexto(.Item("fciudad"))
                    txtBarrioFiscal.Text = MuestraCampoTexto(.Item("fbarrio"))
                    txtTerritorioFiscal.Text = Territorio(txtPaisFiscal.Text, txtEstadoFiscal.Text, txtMunicipioFiscal.Text, _
                                                              txtParroquiaFiscal.Text, txtCiudadFiscal.Text, txtBarrioFiscal.Text)

                    txtZIPFiscal.Text = MuestraCampoTexto(.Item("fZip"))
                    txtDireccionDespacho.Text = MuestraCampoTexto(.Item("dirdespa"))
                    txtPaisDespacho.Text = MuestraCampoTexto(.Item("dpais"))
                    txtMunicipioDespacho.Text = MuestraCampoTexto(.Item("dMunicipio"))
                    txtEstadoDespacho.Text = MuestraCampoTexto(.Item("dEstado"))
                    txtParroquiaDespacho.Text = MuestraCampoTexto(.Item("dparroquia"))
                    txtCiudadDespacho.Text = MuestraCampoTexto(.Item("dciudad"))
                    txtBarrioDespacho.Text = MuestraCampoTexto(.Item("dbarrio"))
                    txtTerritorioDespacho.Text = Territorio(txtPaisDespacho.Text, txtEstadoDespacho.Text, txtMunicipioDespacho.Text, _
                                                              txtParroquiaDespacho.Text, txtCiudadDespacho.Text, txtBarrioDespacho.Text)

                    txtZIPDespacho.Text = MuestraCampoTexto(.Item("dzip"))

                    txttelef1.Text = MuestraCampoTexto(.Item("telef1"))
                    txtTelef2.Text = MuestraCampoTexto(.Item("telef2"))
                    txtTelef3.Text = MuestraCampoTexto(.Item("telef3"))
                    txtFax.Text = MuestraCampoTexto(.Item("fax"))
                    txtemail1.Text = MuestraCampoTexto(.Item("email1"))
                    txtemail2.Text = MuestraCampoTexto(.Item("email2"))

                    txtGerente.Text = MuestraCampoTexto(.Item("gerente"))
                    txtTelefonoGerente.Text = MuestraCampoTexto(.Item("telger"))
                    txtContacto.Text = MuestraCampoTexto(.Item("contacto"))
                    txtTelefonoContacto.Text = MuestraCampoTexto(.Item("telcon"))

                    txtLimiteCredito.Text = FormatoNumero(.Item("limitecredito"))
                    txtSaldo.Text = FormatoNumero(SaldoCxC(myConn, lblInfo, .Item("codcli")))
                    txtDisponible.Text = FormatoNumero(ValorNumero(txtLimiteCredito.Text) - ValorNumero(txtSaldo.Text))


                    chkDoc1.Checked = .Item("req_rif")
                    chkDoc2.Checked = .Item("req_rec")
                    chkDoc3.Checked = .Item("req_reg")
                    chkDoc4.Checked = .Item("req_rea")
                    chkDoc5.Checked = .Item("req_ban")
                    chkDoc6.Checked = .Item("req_com")

                    Dim aIVA() As String = ArregloIVA(myConn, lblInfo)
                    RellenaCombo(aIVA, cmbIVA, InArray(aIVA, .Item("regimeniva")) - 1)
                    txtDescuentoCliente.Text = FormatoNumero(.Item("des_cli"))
                    txtListaPrecios.Text = MuestraCampoTexto(.Item("lispre"))

                    txtDescA.Text = FormatoNumero(.Item("desc_cli_1"))
                    txtDescB.Text = FormatoNumero(.Item("desc_cli_2"))
                    txtDescC.Text = FormatoNumero(.Item("desc_cli_3"))
                    txtDescD.Text = FormatoNumero(.Item("desc_cli_4"))

                    txtDeA.Text = FormatoEntero(.Item("desde_1"))
                    txtDeB.Text = FormatoEntero(.Item("desde_2"))
                    txtDeC.Text = FormatoEntero(.Item("desde_3"))
                    txtDeD.Text = FormatoEntero(.Item("desde_4"))

                    txtAA.Text = FormatoEntero(.Item("hasta_1"))
                    txtAB.Text = FormatoEntero(.Item("hasta_2"))
                    txtAC.Text = FormatoEntero(.Item("hasta_3"))
                    txtAD.Text = FormatoEntero(.Item("Hasta_4"))

                    RellenaCombo(aTarifa, cmbTarifa, InArray(aTarifa, .Item("tarifa")) - 1)
                    txtFormaPago.Text = .Item("formapago")

                    RellenaCombo(aFacturaDesde, cmbFacturaDesde, .Item("codcre"))


                    strSQLCIs = " select concat(nacional, '-',ci ) cedula, nombre, nacional, ci, expediente, id_emp from jsvencedsoc " _
                                            & " where codcli = '" & .Item("codcli") & "' and " _
                                            & " id_emp = '" & jytsistema.WorkID & "' order by 1 "
                    ds = DataSetRequery(ds, strSQLCIs, myConn, nTablaCIs, lblInfo)
                    dtCIs = ds.Tables(nTablaCIs)

                    Dim aCamCI() As String = {"cedula", "nombre"}
                    Dim aNomCI() As String = {"Cédula Identidad", "Nombre"}
                    Dim aAncCI() As Long = {70, 200}
                    Dim aAliCI() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
                    Dim aForCI() As String = {"", ""}

                    IniciarTabla(dgCIs, dtCIs, aCamCI, aNomCI, aAncCI, aAliCI, aForCI, False, , 8, False)
                    If dtCIs.Rows.Count > 0 Then nPocisionCI = 0

                    'Cliente ii
                    txtCodigoClientesii.Text = .Item("codcli")
                    txtNombreClienteii.Text = MuestraCampoTexto(.Item("nombre"))

                    RellenaCombo(aFrecuenciaVisita, cmbFrecuenciaVisita, .Item("fecvisita"))
                    txtInicioVisita.Text = FormatoFecha(CDate(.Item("inivisita").ToString))
                    RellenaCombo(aDiaPago, cmbDiaPago, .Item("diapago"))
                    txtHoraPagoDe.Text = .Item("depago")
                    txtHoraPagoA.Text = .Item("apago")
                    txtUltimaVisita.Text = FormatoFecha(CDate(.Item("fecultvisita").ToString))
                    txtComentario.Text = .Item("comentario")
                    RellenaCombo(aRanking, cmbRanking, .Item("ranking"))
                    txtFechaRanking.Text = FormatoFecha(CDate(.Item("fecharank").ToString))
                    RellenaCombo(aContribuyente, cmbContribuyente, .Item("especial"))
                    RellenaCombo(aShare, cmbShare, .Item("share"))
                    txtIngreso.Text = FormatoFecha(CDate(.Item("ingreso").ToString))
                    RellenaCombo(aEstatusCliente, cmbEstatus, .Item("estatus"))
                    chkMerchandising.Checked = .Item("merchandising")
                    chkFacturaConCHD.Checked = .Item("backorder")

                    AbrirVisitas(.Item("codcli"))
                    AbrirDespachos(.Item("codcli"))
                    AbrirRutasVisita(.Item("codcli"))
                    AbrirRutasDespacho(.Item("codcli"))

                    strSQLProveedores = " select a.codpro, b.nombre from jsvenprocli a " _
                                            & " left join jsprocatpro b on (a.codpro = b.codpro and a.id_emp = b.id_emp ) " _
                                            & " where a.codcli = '" & .Item("codcli") & "' and " _
                                            & " a.id_emp = '" & jytsistema.WorkID & "' order by 1 "

                    ds = DataSetRequery(ds, strSQLProveedores, myConn, nTablaProveedores, lblInfo)
                    dtProveedores = ds.Tables(nTablaProveedores)

                    Dim aCamPro() As String = {"codpro", "nombre"}
                    Dim aNomPro() As String = {"Código", "Nombre/Razón social proveedor"}
                    Dim aAncPro() As Long = {70, 250}
                    Dim aAliPro() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
                    Dim aForPro() As String = {"", ""}

                    IniciarTabla(dgProveedores, dtProveedores, aCamPro, aNomPro, aAncPro, aAliPro, aForPro, False, , 8, False)
                    If dtProveedores.Rows.Count > 0 Then nPosicionProveedor = 0


                    'Movimientos CxC
                    txtCodigoMovimientos.Text = .Item("codcli")
                    txtNombreMovimientos.Text = MuestraCampoTexto(.Item("nombre"))
                    txtSaldoActual.Text = FormatoNumero(.Item("saldo"))
                    txtUltimoPago.Text = FormatoNumero(.Item("ultcobro"))
                    txtFechaUltimoPago.Text = FormatoFecha(CDate(.Item("fecultcobro").ToString))
                    If MuestraCampoTexto(.Item("forultcobro")) <> "" Then
                        txtFormaUltimoPago.Text = IIf(InArray(aFormaPagoAbreviada, .Item("forultcobro")) > 0, aFormaPago(InArray(aFormaPagoAbreviada, .Item("forultcobro")) - 1), "")
                    Else
                        txtFormaUltimoPago.Text = ""
                    End If

                    'Estadisticas
                    txtCodigoEstadisticas.Text = .Item("codcli")
                    txtNombreEstadisticas.Text = MuestraCampoTexto(.Item("nombre"))

                    'Expediente
                    txtCodigoExpediente.Text = .Item("codcli")
                    txtNombreExpediente.Text = MuestraCampoTexto(.Item("nombre"))

                    'Saldos por documento
                    txtCodigoSaldos.Text = .Item("codcli")
                    txtNombreSaldos.Text = MuestraCampoTexto(.Item("nombre"))

                    ''   AbrirMovimientos(.Item("codcli"))


                End With
            End With
        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub AbrirVisitas(ByVal CodigoCliente As String)


        strSQLVisitas = " select CODCLI, ELT(DIA + 1, 'Lunes','Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado','Domingo') as DIA, " _
                    & " DESDE, HASTA, DESDEPM, HASTAPM , TIPO, ID_EMP from jsvencatvis where " _
                    & " CODCLI = '" & CodigoCliente & "' AND " _
                    & " TIPO = 0 AND " _
                    & " division = '' and " _
                    & " ID_EMP = '" & jytsistema.WorkID & "' "

        ds = DataSetRequery(ds, strSQLVisitas, myConn, nTablaVisitas, lblInfo)
        dtVisitas = ds.Tables(nTablaVisitas)

        Dim aCam() As String = {"dia", "desde", "hasta", "desdepm", "hastapm"}
        Dim aNom() As String = {"Día", "Desde a.m.", "Hasta a.m.", "Desde p.m.", "Hasta p.m."}
        Dim aAnc() As Long = {60, 60, 60, 60, 60}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, _
                                 AlineacionDataGrid.Centro, AlineacionDataGrid.Centro}
        Dim aFor() As String = {"", "", "", "", ""}

        IniciarTabla(dgDiasVisita, dtVisitas, aCam, aNom, aAnc, aAli, aFor, False, , 8, False)
        If dtVisitas.Rows.Count > 0 Then nPosicionVisitas = 0

    End Sub

    Private Sub AbrirDespachos(ByVal CodigoCliente As String)


        strSQLDespachos = " select CODCLI, ELT(DIA + 1, 'Lunes','Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado','Domingo') as DIA, " _
                    & " DESDE, HASTA, DESDEPM, HASTAPM , TIPO, ID_EMP from jsvencatvis where " _
                    & " CODCLI = '" & CodigoCliente & "' AND " _
                    & " TIPO = 1 AND " _
                    & " division = '' and " _
                    & " ID_EMP = '" & jytsistema.WorkID & "' "

        ds = DataSetRequery(ds, strSQLDespachos, myConn, nTablaDespachos, lblInfo)
        dtDespachos = ds.Tables(nTablaDespachos)

        Dim aCam() As String = {"dia", "desde", "hasta", "desdepm", "hastapm"}
        Dim aNom() As String = {"Día", "Desde a.m.", "Hasta a.m.", "Desde p.m.", "Hasta p.m."}
        Dim aAnc() As Long = {60, 60, 60, 60, 60}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, _
                                 AlineacionDataGrid.Centro, AlineacionDataGrid.Centro}
        Dim aFor() As String = {"", "", "", "", ""}

        IniciarTabla(dgDiasDespacho, dtDespachos, aCam, aNom, aAnc, aAli, aFor, False, , 8, False)
        If dtDespachos.Rows.Count > 0 Then nPosicionDespachos = 0


    End Sub

    Private Sub AbrirRutasVisita(ByVal CodigoCliente As String)


        strSQLRutasVisita = " SELECT  b.codzon, d.descrip nomZona, a.codrut, b.nomrut, b.codven, CONCAT( c.apellidos, ' ', c.nombres ) nomVendedor, a.numero " _
                & " FROM jsvenrenrut a " _
                & " LEFT JOIN jsvenencrut b ON (a.codrut = b.codrut AND a.tipo = b.tipo AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN jsvencatven c ON (b.codven = c.codven AND b.id_emp = c.id_emp ) " _
                & " LEFT JOIN jsconctatab d ON (b.codzon = d.codigo AND b.id_emp = d.id_emp AND d.modulo = '00005') " _
                & " WHERE " _
                & " a.cliente = '" & CodigoCliente & "' AND " _
                & " a.tipo = 0 AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' "

        ds = DataSetRequery(ds, strSQLRutasVisita, myConn, nTablaRutasVisita, lblInfo)
        dtRutasV = ds.Tables(nTablaRutasVisita)

        Dim aCam() As String = {"codzon", "nomzona", "codrut", "nomrut", "codven", "nomVendedor", "numero"}
        Dim aNom() As String = {"Zona", "", "Ruta", "", "Asesor", "", "#Visita"}
        Dim aAnc() As Long = {40, 150, 40, 150, 50, 160, 40}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                                 AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                                 AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro}
        Dim aFor() As String = {"", "", "", "", "", "", sFormatoEntero}

        IniciarTabla(dgRutasVentas, dtRutasV, aCam, aNom, aAnc, aAli, aFor, , , 8)
        If dtRutasV.Rows.Count > 0 Then nPosicionRutasVisita = 0


    End Sub
    Private Sub AbrirRutasDespacho(ByVal CodigoCliente As String)


        strSQLRutasDespacho = " SELECT  b.codzon, d.descrip nomZona, a.codrut, b.nomrut, b.codtra, c.nomtra nomTransporte, " _
            & " a.numero " _
            & " FROM jsvenrenrut a " _
            & " LEFT JOIN jsvenencrut b ON (a.codrut = b.codrut AND a.tipo = b.tipo AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsconctatra c ON (b.codtra = c.codtra AND b.id_emp = c.id_emp) " _
            & " LEFT JOIN jsconctatab d ON (b.codzon = d.codigo AND b.id_emp = d.id_emp AND d.modulo = '00005') " _
            & " WHERE " _
            & " a.cliente = '" & CodigoCliente & "' AND " _
            & " a.tipo = 1 AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' "

        ds = DataSetRequery(ds, strSQLRutasDespacho, myConn, nTablaRutasDespacho, lblInfo)
        dtRutasD = ds.Tables(nTablaRutasDespacho)

        Dim aCam() As String = {"codzon", "nomzona", "codrut", "nomrut", "codtra", "nomTransporte", "numero"}
        Dim aNom() As String = {"Zona", "", "Ruta", "", "Transp.", "", "#Despacho"}
        Dim aAnc() As Long = {40, 150, 40, 150, 50, 160, 40}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                                 AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                                 AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro}
        Dim aFor() As String = {"", "", "", "", "", "", sFormatoEntero}

        IniciarTabla(dgRutasDespacho, dtRutasD, aCam, aNom, aAnc, aAli, aFor, , , 8)
        If dtRutasD.Rows.Count > 0 Then nPosicionRutasDespacho = 0


    End Sub

    Private Sub AbrirMovimientos(ByVal CodigoCliente As String)

        dg.DataSource = Nothing

        strSQLMov = " select a.emision, a.tipomov, a.nummov, a.vence, a.refer, a.formapag, a.comproba, a.importe, a.origen, a.codven, " _
                    & " a.fotipo, " _
                    & " concat(b.nombres, ' ', b.apellidos) nomvendedor, a.remesa, a.codcli, a.multican, a.cajapag, a.numpag, a.nompag, " _
                    & " a.concepto " _
                    & " from jsventracob a " _
                    & " left join jsvencatven b on (a.codven = b.codven and a.id_emp = b.id_emp) " _
                    & " where " _
                    & " a.codcli = '" & CodigoCliente & "' and " _
                    & " a.historico = '0' and " _
                    & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " order by " _
                    & " a.NUMMOV, a.FOTIPO, a.TIPOMOV, a.EMISION"

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        dtMovimientos = ds.Tables(nTablaMovimientos)

        Dim aCampos() As String = {"emision", "tipomov", "nummov", "vence", "refer", "formapag", "comproba", "importe", "origen", "codven", "nomvendedor"}
        Dim aNombres() As String = {"Emisión", "TP", "Documento", "Vence", "Referencia", "Forma de pago", "Comprobante", "Importe", "ORG", "Asesor", "Nombre"}
        Dim aAnchos() As Long = {80, 25, 90, 80, 80, 45, 90, 100, 35, 50, 80}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {sFormatoFecha, "", "", sFormatoFecha, "", "", "", sFormatoNumero, "", "", ""}
        IniciarTabla(dg, dtMovimientos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtMovimientos.Rows.Count > 0 Then nPosicionMov = 0

        RellenaCombo(aTipoMovimiento, cmbTipoMovimiento)

        txtSaldoActual.Text = FormatoNumero(SaldoCxC(myConn, lblInfo, txtCodigo.Text))


    End Sub
    Private Sub IniciarCliente(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = Contador(myConn, lblInfo, Gestion.iVentas, "VENNUMCLI", "10")
        Else
            txtCodigo.Text = ""
        End If

        IniciarTextoObjetos(FormatoItemListView.iCadena, txtAlterno, txtRIF, txtNombre, txtCanal, txtTipoNegocio, txtDireccionFiscal, _
                            txtPaisFiscal, txtEstadoFiscal, txtMunicipioFiscal, txtParroquiaFiscal, txtCiudadFiscal, txtBarrioFiscal, _
                            txtDireccionDespacho, txtPaisDespacho, txtEstadoDespacho, txtMunicipioDespacho, txtParroquiaDespacho, txtCiudadDespacho, txtBarrioDespacho, _
                            txtTerritorioFiscal, txtTerritorioDespacho, txtZIPFiscal, txtZIPDespacho, _
                            txttelef1, txtTelef2, txtTelef3, txtFax, txtemail1, txtemail2, _
                            txtGerente, txtTelefonoGerente, txtContacto, txtTelefonoContacto, txtFormaPago, txtFormaPagoNombre, _
                            txtListaPrecios, txtZona, txtComentario)

        IniciarTextoObjetos(FormatoItemListView.iNumero, txtLimiteCredito, txtSaldo, txtDisponible, _
                            txtDescuentoCliente, txtDescA, txtDescB, txtDescC, txtDescD)

        IniciarTextoObjetos(FormatoItemListView.iNumero, txtDeA, txtDeB, txtDeC, txtDeD, txtAA, txtAB, txtAC, txtAD)
        IniciarTextoObjetos(FormatoItemListView.iFecha, txtInicioVisita, txtIngreso, txtFechaRanking, txtUltimaVisita)
        IniciarTextoObjetos(FormatoItemListView.iHora, txtHoraPagoDe, txtHoraPagoA)


        LimiteCreditoAnterior = 0.0
        RellenaCombo(aEstatusCliente, cmbEstatus)
        ' RellenaCombo(aIVA, cmbIVA)
        RellenaCombo(aTarifa, cmbTarifa)
        RellenaCombo(aFrecuenciaVisita, cmbFrecuenciaVisita, 1)
        RellenaCombo(aDiaPago, cmbDiaPago)
        RellenaCombo(aRanking, cmbRanking)
        RellenaCombo(aContribuyente, cmbContribuyente)
        RellenaCombo(aShare, cmbShare)


        aIVA = ArregloIVA(myConn, lblInfo)
        If cmbIVA.Items.Count > 0 Then cmbIVA.SelectedIndex = InArray(aIVA, "A") - 1
        If cmbTarifa.Items.Count > 0 Then cmbTarifa.SelectedIndex = 0

        Dim ncodfor As String = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select codfor from jsconctafor where nomfor = 'FACTURA CONTADO COD' and id_emp = '" & jytsistema.WorkID & "'  ")
        If ncodfor <> "" Then txtFormaPago.Text = ncodfor
        txtLimiteCredito.Text = FormatoNumero(10000)


        chkDoc5.Checked = False
        chkDoc4.Checked = False
        chkDoc2.Checked = False
        chkDoc1.Checked = False
        chkDoc3.Checked = False
        chkDoc6.Checked = False
        chkMerchandising.Checked = False
        chkFacturaConCHD.Checked = False

        dg.Columns.Clear()
        dgCIs.Columns.Clear()
        dgDiasDespacho.Columns.Clear()
        dgDiasVisita.Columns.Clear()
        dgRutasDespacho.Columns.Clear()
        dgRutasVentas.Columns.Clear()
        dgProveedores.Columns.Clear()


    End Sub
    Private Sub ActivarMarco0()
        grpAceptarSalir.Visible = True

        HabilitarObjetos(False, False, C1DockingTabPage2, C1DockingTabPage6, C1DockingTabPage4, C1DockingTabPage5)
        HabilitarObjetos(True, True, txtAlterno, txtRIF, txtNombre, btnCanal, txtDireccionFiscal, txtZIPFiscal, _
                         btnTerritorioFiscal, txtDireccionDespacho, txtZIPDespacho, btnTerritorioDespacho, _
                         txttelef1, txtTelef2, txtTelef3, txtFax, txtemail1, txtemail2, txtGerente, txtTelefonoGerente, _
                         txtContacto, txtTelefonoContacto, txtLimiteCredito, _
                         cmbIVA, btnForma, txtDescuentoCliente, btnListaPrecios, cmbTarifa, txtDescA, txtDescB, txtDescC, txtDescD, _
                         txtDeA, txtDeB, txtDeC, txtDeD, txtAA, txtAB, txtAC, txtAD, cmbFrecuenciaVisita, _
                         btnInicioVisita, cmbDiaPago, txtHoraPagoDe, txtHoraPagoA, txtComentario, _
                         cmbEstatus, cmbContribuyente, cmbRanking, cmbShare, btnIngreso, btnIVA)

        HabilitarObjetos(True, False, MenuCIs, chkDoc1, chkDoc2, chkDoc3, chkDoc4, chkDoc5, grpFormaPago, _
                         chkMerchandising, chkFacturaConCHD, _
                         chkDoc6, MenuDiasDespacho, MenuDiasVisita, MenuProveedorExclusivo)

        If i_modo = movimiento.iEditar Then HabilitarObjetos(False, False, txtCodigo)

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub

    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        HabilitarObjetos(True, False, C1DockingTabPage2, C1DockingTabPage3, C1DockingTabPage4, C1DockingTabPage5, C1DockingTabPage6)

        HabilitarObjetos(False, True, txtCodigo, txtAlterno, txtRIF, txtNombre, btnCanal, txtCanalNombre, txtTipoNegocioNombre, txtTipoContribuyente, _
                         txtDireccionFiscal, txtZIPFiscal, btnTerritorioFiscal, txtTerritorioFiscal, txtDireccionDespacho, txtZIPDespacho, btnTerritorioDespacho, txtTerritorioDespacho, _
                         txttelef1, txtTelef2, txtTelef3, txtFax, txtemail1, txtemail2, txtGerente, txtTelefonoGerente, _
                         txtContacto, txtTelefonoContacto, _
                         txtLimiteCredito, txtSaldo, txtDisponible, _
                          cmbIVA, txtIVA, btnIVA, _
                         txtDescuentoCliente, txtListaPrecios, btnListaPrecios, cmbTarifa, _
                         btnForma, txtFormaPagoNombre, txtDescA, txtDescB, txtDescD, txtDescC, _
                         txtDeA, txtDeB, txtDeC, txtDeD, txtAA, txtAB, txtAC, txtAD, _
                         txtCodigoClientesii, txtNombreClienteii, cmbFrecuenciaVisita, txtInicioVisita, btnInicioVisita, _
                         cmbDiaPago, txtHoraPagoDe, txtHoraPagoA, txtUltimaVisita, _
                         txtComentario, cmbRanking, txtFechaRanking, _
                         cmbContribuyente, cmbShare, _
                         txtIngreso, btnIngreso, cmbEstatus, _
                         txtCodigoMovimientos, txtNombreMovimientos, txtUltimoPago, txtFechaUltimoPago, txtFormaUltimoPago, txtSaldoActual, _
                         txtCodigoSaldos, txtNombreSaldos, txtDocSel, txtSaldoSel, _
                         txtCodigoEstadisticas, txtNombreEstadisticas, _
                         txtCodigoExpediente, txtNombreExpediente, txtChMes, txtChAno, txtCHTotal, txtCHMontoMes, txtChMontoAno, txtCHMontoTotal)

        HabilitarObjetos(False, False, chkDoc1, chkDoc2, chkDoc3, chkDoc4, chkDoc5, chkDoc6, _
                         MenuProveedorExclusivo, MenuCIs, MenuDiasDespacho, MenuDiasVisita, _
                         grpFormaPago, chkMerchandising, chkFacturaConCHD)

        MenuBarra.Enabled = True

        i_modo = movimiento.iConsultar

        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Select Case tbcClientes.SelectedTab.Text
            Case "Clientes i", "Clientes ii"
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case "Movimientos CxC"
                Me.BindingContext(ds, nTablaMovimientos).Position = 0
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Saldos por Documento"
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarSaldosPorDocumento(cmbSaldos.SelectedIndex, txtCodigo.Text)
            Case "Estadísticas"
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AbrirEstadisticas()
            Case "Expediente"
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarExpediente(txtCodigo.Text)
        End Select



    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Select Case tbcClientes.SelectedTab.Text
            Case "Clientes i", "Clientes ii"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case "Movimientos CxC"
                Me.BindingContext(ds, nTablaMovimientos).Position -= 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Saldos por Documento"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarSaldosPorDocumento(cmbSaldos.SelectedIndex, txtCodigo.Text)
            Case "Estadísticas"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AbrirEstadisticas()
            Case "Expediente"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarExpediente(txtCodigo.Text)
        End Select

    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Select Case tbcClientes.SelectedTab.Text
            Case "Clientes i", "Clientes ii"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case "Movimientos CxC"
                Me.BindingContext(ds, nTablaMovimientos).Position += 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Saldos por Documento"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarSaldosPorDocumento(cmbSaldos.SelectedIndex, txtCodigo.Text)
            Case "Estadísticas"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AbrirEstadisticas()
            Case "Expediente"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarExpediente(txtCodigo.Text)
        End Select

    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Select Case tbcClientes.SelectedTab.Text
            Case "Clientes i", "Clientes ii"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case "Movimientos CxC"
                Me.BindingContext(ds, nTablaMovimientos).Position = ds.Tables(nTablaMovimientos).Rows.Count - 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Saldos por Documento"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarSaldosPorDocumento(cmbSaldos.SelectedIndex, txtCodigo.Text)
            Case "Estadísticas"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AbrirEstadisticas()
            Case "Expediente"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarExpediente(txtCodigo.Text)
        End Select

    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

        Select Case tbcClientes.SelectedTab.Text
            Case "Clientes i", "Cliente ii"
                i_modo = movimiento.iAgregar
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
                IniciarCliente(True)
            Case "Movimientos CxC"
                If Trim(txtCodigo.Text) <> "" Then
                    Dim f As New jsVenArcClientesCXCPlus
                    'Dim f As New jsVenArcClientesCXC
                    f.Agregar(myConn, ds, dtMovimientos, txtCodigo.Text)
                    ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                    AbrirMovimientos(txtCodigo.Text)

                    If f.Comprobante <> "" Then
                        Select Case f.TipoMovimientoCXC
                            Case 0 'Debitos
                                Dim row As DataRow = dtMovimientos.Select(" NUMMOV = '" & f.Comprobante & "'  ")(0)
                                nPosicionMov = dtMovimientos.Rows.IndexOf(row)
                            Case 1 'Creditos
                                Dim row As DataRow = dtMovimientos.Select(" COMPROBA = '" & f.Comprobante & "'  ")(0)
                                nPosicionMov = dtMovimientos.Rows.IndexOf(row)
                            Case 2 'Retencion IVA
                                Dim row As DataRow = dtMovimientos.Select(" REFER = '" & f.Comprobante & "'  ")(0)
                                nPosicionMov = dtMovimientos.Rows.IndexOf(row)
                            Case 3 'Retencion ISLR
                                Dim row As DataRow = dtMovimientos.Select(" REFER = '" & f.Comprobante & "'  ")(0)
                                nPosicionMov = dtMovimientos.Rows.IndexOf(row)
                            Case Else
                                nPosicionMov = f.Apuntador
                        End Select
                    Else
                        nPosicionMov = f.Apuntador
                    End If
                    If nPosicionMov >= 0 Then AsignaMov(nPosicionMov, True)
                    f = Nothing

                End If
            Case "Expediente"
                Dim g As New jsVenArcClientesExpediente
                g.Agregar(myConn, txtCodigo.Text, cmbEstatus.SelectedIndex)
                AsignarExpediente(txtCodigo.Text)
                g.Dispose()
                g = Nothing

        End Select


    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Select Case tbcClientes.SelectedTab.Text
            Case "Clientes i", "clientes ii"
                i_modo = movimiento.iEditar
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
            Case "Movimientos'CxC"
                If dtMovimientos.Rows(Me.BindingContext(ds, nTablaMovimientos).Position).Item("origen") = "CXC" Then
                    Dim f As New jsVenArcClientesCXC
                    f.Apuntador = Me.BindingContext(ds, nTablaMovimientos).Position
                    f.Editar(myConn, ds, dtMovimientos, txtCodigo.Text)
                    ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                    AsignaMov(f.Apuntador, True)
                    f = Nothing
                End If
        End Select




    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Select Case tbcClientes.SelectedTab.Text
            Case "Clientes i", "Clientes ii"
                EliminaCliente()
            Case "Movimientos CxC"
                If cmbEstatus.Text = "Activo" Then
                    EliminarMovimiento()
                Else
                    MensajeCritico(lblInfo, "Este proveedor NO está activo ....")
                End If
            Case "Expediente"

        End Select
    End Sub
    Private Sub EliminaCliente()
        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        Dim aCamposDel() As String = {"codcli", "id_emp"}
        Dim aStringsDel() As String = {txtCodigo.Text, jytsistema.WorkID}
        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
        If sRespuesta = MsgBoxResult.Yes Then
            If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, "select count(*) from jsventracob where codcli = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")) = 0 Then
                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsvencatcli", strSQL, aCamposDel, aStringsDel, _
                                              Me.BindingContext(ds, nTabla).Position, True))
            Else
                MensajeCritico(lblInfo, "Este cliente posee movimientos. Verifique por favor ...")
            End If
        End If
    End Sub
    Private Function PoseeMovimientosAsociados(ByVal MyConn As MySqlConnection, ByVal lblInfo As System.Windows.Forms.Label, _
                                               ByVal CodigoCliente As String, Optional ByVal NumeroMovimiento As String = "", _
                                               Optional ByVal TipoMovimiento As String = "") As Boolean

        Dim cuenta As Integer = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) cuenta from jsventracob where " _
                                                           & " codcli  = '" & CodigoCliente & "' and " _
                                                           & IIf(NumeroMovimiento <> "", " nummov = '' and ", "") _
                                                           & IIf(TipoMovimiento <> "", " tipomov <> '' and ", "") _
                                                           & " id_emp = '" & jytsistema.WorkID & "' "))
        If cuenta > 0 Then Return True

    End Function
    Private Sub EliminarMovimiento()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position

        If nPosicionMov >= 0 Then
            With dtMovimientos.Rows(nPosicionMov)
                If .Item("origen") = "CXC" Then
                    If .Item("REMESA") <> "" AndAlso .Item("FORMAPAG") = "CT" Then
                        MensajeCritico(lblInfo, "No puede realizarse la operación pues YA ha sido asignada a una remesa...")
                    Else
                        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                        If sRespuesta = MsgBoxResult.Yes Then

                            InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("nummov"))
                            Dim TipoMovimiento As String = .Item("tipomov")
                            Select Case TipoMovimiento
                                Case "FC", "GR", "ND", "CD"
                                    If PoseeMovimientosAsociados(myConn, lblInfo, .Item("codcli"), .Item("nummov"), .Item("tipomov")) Then
                                        MensajeAdvertencia(lblInfo, "Este documento posee documentos asociados a él...")
                                    Else
                                        Dim aeCam() As String = {"codcli", "tipomov", "emision", "nummov", "refer", "ejercicio", "id_emp"}
                                        Dim aeStr() As String = {.Item("codcli"), .Item("tipomov"), FormatoFechaMySQL(CDate(.Item("emision").ToString)), _
                                                                 .Item("nummov"), .Item("refer"), jytsistema.WorkExercise, jytsistema.WorkID}

                                        AsignaMov(EliminarRegistros(myConn, lblInfo, ds, nTablaMovimientos, "jsventracob", strSQLMov, aeCam, _
                                                                    aeStr, nPosicionMov, True), False)

                                    End If
                                Case "AB", "CA", "NC"
                                    Dim sRespuesta2 As Microsoft.VisualBasic.MsgBoxResult
                                    If CBool(.Item("multican").ToString) Then
                                        sRespuesta2 = MsgBox("Este documento pertenece a una cancelación múltiple. Se eliminarán todos los documentos ¿ Desea Eliminar ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                                        If sRespuesta2 = MsgBoxResult.No Then
                                            Return
                                        End If
                                    End If

                                    '////////////////////////////////////////////77777
                                    'NO PUEDE ELIMINAR SI EL DOCUMENTO ESTA DEPOSITADO
                                    '//////////////////////////////////////////////////
                                    If .Item("COMPROBA") <> "" Then
                                        If CancelacionAbonoDepositado(myConn, lblInfo, .Item("comproba")) Then
                                            MensajeCritico(lblInfo, "ESTE DOCUMENTO YA FUE DEPOSITADO. VERIFIQUE POR FAVOR...")
                                            Return
                                        End If
                                    End If

                                    EjecutarSTRSQL(myConn, lblInfo, "DELETE from jsventracob where " _
                                        & " codcli = '" & .Item("codcli") & "' AND " _
                                        & " REFER = '" & .Item("refer") & "' AND " _
                                        & " COMPROBA = '" & .Item("comproba") & "' AND  " _
                                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                        & " ID_EMP = '" & jytsistema.WorkID & "' ")

                                    EjecutarSTRSQL(myConn, lblInfo, "DELETE from jsventracobcan where " _
                                        & " codcli = '" & .Item("codcli") & "' AND " _
                                        & " COMPROBA = '" & .Item("comproba") & "' AND  " _
                                        & " ID_EMP = '" & jytsistema.WorkID & "' ")

                                    'elimina movimiento en caja
                                    EjecutarSTRSQL(myConn, lblInfo, " delete from jsbantracaj where " _
                                        & " nummov = '" & .Item("comproba") & "' and " _
                                        & " origen = 'CXC' and " _
                                        & " caja = '" & .Item("cajapag") & "' and " _
                                        & " id_emp = '" & jytsistema.WorkID & "'")

                                    EjecutarSTRSQL(myConn, lblInfo, "DELETE FROM jsventabtic where " _
                                        & "NUMCAN = '" & .Item("comproba") & "' AND " _
                                        & "ID_EMP = '" & jytsistema.WorkID & "' ")

                                    'elimina movimiento en banco
                                    EjecutarSTRSQL(myConn, lblInfo, " delete from jsbantraban where " _
                                        & " numorg = '" & .Item("comproba") & "' and " _
                                        & " origen = 'CXC' and " _
                                        & " codban = '" & .Item("nompag") & "' and " _
                                        & " id_emp = '" & jytsistema.WorkID & "'")
                            End Select

                            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                            dtMovimientos = ds.Tables(nTablaMovimientos)

                            If nPosicionMov > dtMovimientos.Rows.Count - 1 Then nPosicionMov = dtMovimientos.Rows.Count - 1
                            AsignaMov(nPosicionMov, False)


                        End If
                    End If
                ElseIf .Item("origen") = "BAN" Then
                    If .Item("tipomov") = "ND" AndAlso .Item("nummov").ToString.Substring(0, 2) = "ND" Then
                        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                        If sRespuesta = MsgBoxResult.Yes Then
                            Dim aeCam() As String = {"codcli", "tipomov", "emision", "nummov", "refer", "ejercicio", "id_emp"}
                            Dim aeStr() As String = {.Item("codcli"), .Item("tipomov"), FormatoFechaMySQL(CDate(.Item("emision").ToString)), _
                                                     .Item("nummov"), .Item("refer"), jytsistema.WorkExercise, jytsistema.WorkID}

                            AsignaMov(EliminarRegistros(myConn, lblInfo, ds, nTablaMovimientos, "jsventracob", strSQLMov, aeCam, _
                                                        aeStr, nPosicionMov, True), False)

                        End If

                    End If
                Else
                    MensajeAdvertencia(lblInfo, "Movimiento no puede ser eliminado desde CXC...")
                End If
            End With
        End If

    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar

        Select Case tbcClientes.SelectedTab.Text
            Case "Clientes i", "Clientes ii"
                Dim Campos() As String = {"codcli", "nombre", "rif", "alterno"}
                Dim Nombres() As String = {"Código", "Nombre", "RIF", "Código alterno"}
                Dim Anchos() As Long = {120, 850, 100, 120}
                f.Text = "Clientes"
                f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, " Clientes...", 0)
                nPosicionCat = f.Apuntador
                Me.BindingContext(ds, nTabla).Position = nPosicionCat
                AsignaTXT(nPosicionCat)
            Case "Movimientos CxC"
                Dim Campos() As String = {"emision", "nummov", "vence"}
                Dim Nombres() As String = {"Emisión", "Nº Movimiento", "vence"}
                Dim Anchos() As Long = {100, 120, 12}
                f.Text = "Movimientos de cliente"
                f.Buscar(dtMovimientos, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaMovimientos).Position, "Movimientos de CxC...")
                nPosicionMov = f.Apuntador
                Me.BindingContext(ds, nTablaMovimientos).Position = nPosicionMov
                AsignaMov(nPosicionMov, False)
        End Select

        f = Nothing


    End Sub


    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsVenRepParametros
        Select Case tbcClientes.SelectedTab.Text
            Case "Clientes i", "Clientes ii"
                f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cCliente, "Cliente", txtCodigo.Text)
            Case "Movimientos CxC"
                If dtMovimientos.Rows(nPosicionMov).Item("comproba") <> "" Then
                    Dim Respuesta As Microsoft.VisualBasic.MsgBoxResult
                    Respuesta = MsgBox(" ¿ Desea imprimir RECIBO DE CANCELACION ?", MsgBoxStyle.YesNo, "IMPRIMIR RECIBO ... ")
                    If Respuesta = MsgBoxResult.Yes Then
                        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cCxC, "Recibo cancelación y/o Abono CXC", , _
                                 dtMovimientos.Rows(nPosicionMov).Item("comproba"))
                    End If
                Else
                    f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cMovimientosClientes, "Movimientos de Cliente", txtCodigoMovimientos.Text)
                End If
            Case "Saldos por Documento"
                Dim ff As New jsVenRepParametrosDos
                ff.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cSaldosPorDocumento, "SALDOS POR DOCUMENTO", txtCodigo.Text)
                ff.Dispose()
                ff = Nothing
        End Select
        f.Dispose()
        f = Nothing
    End Sub


    Private Function Validado() As Boolean
        Validado = False

        If txtCodigo.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un código de cliente válido...")
            EnfocarTexto(txtCodigo)
            Exit Function
        Else
            If i_modo = movimiento.iAgregar Then
                If EjecutarSTRSQL_Scalar(myConn, lblInfo, " select codcli from jsvencatcli where codcli = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") <> 0 Then
                    MensajeCritico(lblInfo, "Código de cliente YA existe. Verifique por favor...")
                    EnfocarTexto(txtCodigo)
                    Exit Function
                End If
            End If
        End If

        If txtRIF.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un RIF de cliente válido...")
            EnfocarTexto(txtRIF)
            Exit Function
        Else
            If i_modo = movimiento.iAgregar Then
                Dim rRif As String = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select codcli from jsvencatcli where RIF = '" & txtRIF.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                If rRif <> "0" Then
                    If CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select ALTERNO from jsvencatcli where RIF = '" & txtRIF.Text & "' AND ALTERNO = '" & txtAlterno.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")) <> "0" Then
                        MensajeCritico(lblInfo, "EXISTE CLIENTE CON EL MISMO RIF E IGUAL CODIGO ALTERNO(SADA). VERIFIQUE POR FAVOR...")
                        EnfocarTexto(txtAlterno)
                        Exit Function
                    End If
                End If
            End If
        End If

        If ValorNumero(txtLimiteCredito.Text) = 0.0 Then
            MensajeCritico(lblInfo, "Debe indicar un LIMITE DE CREDITO VALIDO....")
            EnfocarTexto(txtLimiteCredito)
            Exit Function
        End If

        If txtFormaPagoNombre.Text.TrimEnd() = "" Then
            MensajeCritico(lblInfo, "Debe indicar una FORMA DE PAGO VALIDA...")
            Exit Function
        End If

        If cmbFacturaDesde.SelectedIndex = 1 AndAlso _
            ValorNumero(txtDescuentoCliente.Text) = 0.0 Then
            MensajeCritico(lblInfo, "DEBE INDICAR UN VALOR DE GANANCIA PARA LA FACTURACIÓN DE ESTE CLIENTE...")
            Exit Function
        End If

        Validado = True

    End Function
    Private Sub GuardarTXT()

        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionCat = ds.Tables(nTabla).Rows.Count

        End If



        InsertEditVENTASCliente(myConn, lblInfo, Inserta, txtCodigo.Text, txtNombre.Text, txtCanal.Text, txtTipoNegocio.Text, _
                                 txtRIF.Text, "", "", txtAlterno.Text, txtDireccionFiscal.Text, ValorEntero(txtPaisFiscal.Text), _
                                 ValorEntero(txtEstadoFiscal.Text), ValorEntero(txtMunicipioFiscal.Text), ValorEntero(txtParroquiaFiscal.Text), _
                                 ValorEntero(txtCiudadFiscal.Text), ValorEntero(txtBarrioFiscal.Text), txtZIPFiscal.Text, _
                                 txtDireccionDespacho.Text, ValorEntero(txtPaisDespacho.Text), ValorEntero(txtEstadoDespacho.Text), _
                                 ValorEntero(txtMunicipioDespacho.Text), ValorEntero(txtParroquiaDespacho.Text), ValorEntero(txtCiudadDespacho.Text), _
                                 ValorEntero(txtBarrioDespacho.Text), txtZIPDespacho.Text, 0, txtemail1.Text, txtemail2.Text, _
                                 "", "", txttelef1.Text, txtTelef2.Text, txtTelef3.Text, txtFax.Text, txtGerente.Text, txtTelefonoGerente.Text, _
                                 txtContacto.Text, txtTelefonoContacto.Text, ValorNumero(txtLimiteCredito.Text), ValorNumero(txtLimiteCredito.Text) - LimiteCreditoAnterior + ValorNumero(txtDisponible.Text), _
                                 ValorNumero(txtDescuentoCliente.Text), ValorNumero(txtDescA.Text), ValorNumero(txtDescB.Text), _
                                 ValorNumero(txtDescC.Text), ValorNumero(txtDescD.Text), ValorEntero(txtDeA.Text), ValorEntero(txtDeB.Text), ValorEntero(txtDeC.Text), ValorEntero(txtDeD.Text), _
                                 ValorEntero(txtAA.Text), ValorEntero(txtAB.Text), ValorEntero(txtAC.Text), ValorEntero(txtAD.Text), _
                                 ValorNumero(txtSaldo.Text), cmbIVA.Text, cmbTarifa.Text, txtListaPrecios.Text, txtFormaPago.Text, _
                                 CDate(txtIngreso.Text), "", cmbFacturaDesde.SelectedIndex, cmbEstatus.SelectedIndex, If(chkDoc1.Checked, 1, 0), 0, _
                                 If(chkDoc2.Checked, 1, 0), 0, If(chkDoc3.Checked, 1, 0), If(chkDoc4.Checked, 1, 0), If(chkDoc5.Checked, 1, 0), If(chkDoc6.Checked, 1, 0), _
                                 cmbFrecuenciaVisita.SelectedIndex, CDate(txtUltimaVisita.Text), CDate(txtInicioVisita.Text), _
                                 If(chkMerchandising.Checked, 1, 0), If(chkFacturaConCHD.Checked, 1, 0), cmbDiaPago.SelectedIndex, _
                                 txtHoraPagoDe.Text, txtHoraPagoA.Text, cmbRanking.SelectedIndex, CDate(txtFechaRanking.Text), _
                                 txtComentario.Text, cmbContribuyente.SelectedIndex, cmbShare.SelectedIndex)

        ''
        If Inserta Then

            Dim RutaInicial As String = Parametro(myConn, lblInfo, Gestion.iVentas, "VENPARAM27")
            Dim AsesorInicial As String = EjecutarSTRSQL_Scalar(myConn, lblInfo, " SELECT codven FROM jsvenencrut WHERE CODRUT = '" & RutaInicial & "' AND tipo = 0 AND id_emp = '" & jytsistema.WorkID & "' ")
            Dim NumeroInicialEnRuta As Integer = CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " SELECT items FROM jsvenencrut WHERE CODRUT = '" & RutaInicial & "' AND tipo = 0 AND id_emp = '" & jytsistema.WorkID & "' ")) + 1
            Dim CondicionInicialRuta As Integer = CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " SELECT condicion FROM jsvenencrut WHERE CODRUT = '" & RutaInicial & "' AND tipo = 0 AND id_emp = '" & jytsistema.WorkID & "' "))

            InsertEditVENTASRenglonRuta(myConn, lblInfo, True, RutaInicial, NumeroInicialEnRuta, txtCodigo.Text, txtNombre.Text, 0, 1, "", CondicionInicialRuta)

            EjecutarSTRSQL(myConn, lblInfo, " update jsvenencrut set  items = " & NumeroInicialEnRuta & " where codrut = '" & RutaInicial & "' and id_emp = '" & jytsistema.WorkID & "' ")

            EjecutarSTRSQL(myConn, lblInfo, " update jsvencatcli set vendedor = '" & AsesorInicial & "', cobrador = '" & AsesorInicial _
                           & "', ruta_visita ='" & RutaInicial & "', num_visita = " & NumeroInicialEnRuta _
                           & " where " _
                           & " codcli = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" CODCLI = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionCat = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionCat
        AsignaTXT(nPosicionCat)
        DesactivarMarco0()
        tbcClientes.SelectedTab = C1DockingTabPage1
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtAlterno.GotFocus, txtRIF.GotFocus, txtNombre.GotFocus, btnCanal.GotFocus, txtDireccionFiscal.GotFocus, txtZIPFiscal.GotFocus, _
        btnTerritorioFiscal.GotFocus, txtDireccionDespacho.GotFocus, txtZIPDespacho.GotFocus, btnTerritorioDespacho.GotFocus, _
        txttelef1.GotFocus, txtTelef2.GotFocus, txtTelef3.GotFocus, txtFax.GotFocus, txtemail1.GotFocus, txtemail2.GotFocus, _
        txtGerente.GotFocus, txtTelefonoGerente.GotFocus, txtContacto.GotFocus, txtTelefonoContacto.GotFocus, txtLimiteCredito.GotFocus, _
        txtDescA.GotFocus, txtDescB.GotFocus, txtDescC.GotFocus, txtDescD.GotFocus, txtDeA.GotFocus, txtDeB.GotFocus, txtDeC.GotFocus, _
        txtDeD.GotFocus, txtAA.GotFocus, txtAB.GotFocus, txtAC.GotFocus, txtAD.GotFocus, txtDescuentoCliente.GotFocus, _
        btnListaPrecios.GotFocus, btnForma.GotFocus, cmbTarifa.GotFocus, cmbFrecuenciaVisita.GotFocus, btnInicioVisita.GotFocus, _
        cmbDiaPago.GotFocus, txtHoraPagoA.GotFocus, txtHoraPagoDe.GotFocus, txtComentario.GotFocus, cmbContribuyente.GotFocus, _
        cmbShare.GotFocus, btnIngreso.GotFocus, cmbEstatus.GotFocus

        Select Case sender.name
            Case "txtCodigo"
                MensajeEtiqueta(lblInfo, " Indique el código de CLIENTE ... ", TipoMensaje.iInfo)
            Case "txtAlterno"
                MensajeEtiqueta(lblInfo, " Indique código alternativo para el cliente ... ", TipoMensaje.iInfo)
            Case "txtRIF"
                MensajeEtiqueta(lblInfo, " Indique RIF para el cliente ... ", TipoMensaje.iInfo)
            Case "txtNombre"
                MensajeEtiqueta(lblInfo, " Indique Nombre o razón social del cliente ... ", TipoMensaje.iInfo)
            Case "btnCanal"
                MensajeEtiqueta(lblInfo, " Seleccione canal y tipo de negocio para este cliente ... ", TipoMensaje.iInfo)
            Case "txtDireccionFiscal"
                MensajeEtiqueta(lblInfo, " Indique la dirección fiscal de este cliente ... ", TipoMensaje.iInfo)
            Case "txtZIPFiscal"
                MensajeEtiqueta(lblInfo, " Indique el código postal de la dirección fiscal de este cliente  ... ", TipoMensaje.iInfo)
            Case "btnTerritorioFiscal"
                MensajeEtiqueta(lblInfo, " Seleccione el territorio de la dirección fiscal donde se ubica este cliente  ... ", TipoMensaje.iInfo)
            Case "txtDireccionDespacho"
                MensajeEtiqueta(lblInfo, " Indique la dirección de despacho de este cliente ... ", TipoMensaje.iInfo)
            Case "txtZIPDespacho"
                MensajeEtiqueta(lblInfo, " Indique el código postal de la dirección de despacho de este cliente  ... ", TipoMensaje.iInfo)
            Case "btnTerritorioDespacho"
                MensajeEtiqueta(lblInfo, " Seleccione el territorio de la dirección de despacho donde se ubica este cliente  ... ", TipoMensaje.iInfo)
            Case "txtTelef1", "txtTelef2", "txtTelef3"
                MensajeEtiqueta(lblInfo, " Indique el número teléfónico de este cliente ... ", TipoMensaje.iInfo)
            Case "txtFAX"
                MensajeEtiqueta(lblInfo, " Indique el número del 'FAX telefónico de este cliente ... ", TipoMensaje.iInfo)
            Case "txtemail1", "txtemail2"
                MensajeEtiqueta(lblInfo, " Indique dirección de correo electrónico de este cliente  ... ", TipoMensaje.iInfo)
            Case "txtGerente"
                MensajeEtiqueta(lblInfo, " Indique el nombre del gerente ... ", TipoMensaje.iInfo)
            Case "txtTelefonoGerente"
                MensajeEtiqueta(lblInfo, " Indique número teléfónico del gerente ... ", TipoMensaje.iInfo)
            Case "txtContacto"
                MensajeEtiqueta(lblInfo, " Indique nombre de la persona contacto de este cliente ... ", TipoMensaje.iInfo)
            Case "txtTelefonoContacto"
                MensajeEtiqueta(lblInfo, " Indique el número de teléfono contacto ... ", TipoMensaje.iInfo)
            Case "txtLimiteCredito"
                MensajeEtiqueta(lblInfo, " Indique el Límite de crédito aprobado para este cliente ... ", TipoMensaje.iInfo)
            Case "txtDescA", "txtDescB", "txtDescC", "txtDescD"
                MensajeEtiqueta(lblInfo, " Indique el descuento de venta post venta para este cliente ... ", TipoMensaje.iInfo)
            Case "txtDeA", "txtDeB", "txtDeC", "txtDeD"
                MensajeEtiqueta(lblInfo, " Indique dias desde ... ", TipoMensaje.iInfo)
            Case "txtAA", "txtAB", "txtAC", "txtAD"
                MensajeEtiqueta(lblInfo, " Indique dias hasta ... ", TipoMensaje.iInfo)
            Case "txtDescuentoCliente"
                MensajeEtiqueta(lblInfo, " Indique descuento especial para este cliente ... ", TipoMensaje.iInfo)
            Case "btnListaPrecios"
                MensajeEtiqueta(lblInfo, " Seleccione lista de precios especiales para este cliente ... ", TipoMensaje.iInfo)
            Case "btnForma"
                MensajeEtiqueta(lblInfo, " Seleccione la forma de pago para este cliente ... ", TipoMensaje.iInfo)
            Case "cmbTarifa"
                MensajeEtiqueta(lblInfo, " Seleccione la tarifa de precios que se aplica a esgte cliente ... ", TipoMensaje.iInfo)
            Case "cmbFrecuenciaVisita"
                MensajeEtiqueta(lblInfo, " Seleccione la frecuencia de visita para este cliente ... ", TipoMensaje.iInfo)
            Case "btnInicioVisita"
                MensajeEtiqueta(lblInfo, " Seleccione la fecha en que se iniciarian las visitas a este cliente ... ", TipoMensaje.iInfo)
            Case "cmbDiaPago"
                MensajeEtiqueta(lblInfo, " Seleccione el día de pago de este cliente ... ", TipoMensaje.iInfo)
            Case "txtHoraPagoA"
                MensajeEtiqueta(lblInfo, " Indique hora de pago hasta ... ", TipoMensaje.iInfo)
            Case "txtHoraPagoDe"
                MensajeEtiqueta(lblInfo, " Indique hora de pago desde ... ", TipoMensaje.iInfo)
            Case "txtComentario"
                MensajeEtiqueta(lblInfo, " Indique comentario ... ", TipoMensaje.iInfo)
            Case "cmbContribuyente"
                MensajeEtiqueta(lblInfo, " seleccione el tipo de contribuyente de este cliente ... ", TipoMensaje.iInfo)
            Case "cmbShare"
                MensajeEtiqueta(lblInfo, " Seleccione el Share de mercado de este cliente ... ", TipoMensaje.iInfo)
            Case "btnIngreso"
                MensajeEtiqueta(lblInfo, " Seleccione la fecha de ingreso al sistema de este cliente ... ", TipoMensaje.iInfo)
            Case "cmbEstatus"
                MensajeEtiqueta(lblInfo, " Seleccione el estatus del cliente ... ", TipoMensaje.iInfo)

        End Select

    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
        dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        nPosicionMov = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
    End Sub

    Private Sub tbcClientes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcClientes.SelectedIndexChanged

        Select Case tbcClientes.SelectedIndex
            Case 0, 1 ' Cliente i, Clientes ii
                If i_modo <> movimiento.iAgregar Then
                    AsignaTXT(nPosicionCat)
                    ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
                End If
            Case 2 'Movimientos
                AbrirMovimientos(txtCodigo.Text)
                AsignaMov(nPosicionMov, True)
                dg.Enabled = True
            Case 3 'Saldos por documento
                cmbSaldos.SelectedIndex = 0
                strSel = " nummov = '' OR "
                IniciarSaldosPorDocumento(0, txtCodigo.Text)
                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nPosicionCat, ds.Tables(nTabla).Rows.Count)
            Case 4 ' Estatísticas
                rBtnV1.Checked = True
                RellenaCombo(aEstadistica, cmbTipoEstadistica)
                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nPosicionCat, ds.Tables(nTabla).Rows.Count)
            Case 5 ' Expediente
                AsignarExpediente(txtCodigo.Text)

        End Select
    End Sub
    Private Function ValoresMensuales(ByVal dtValores As DataTable) As Double()
        Dim aMes As Double() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        If dtValores.Rows.Count > 0 Then
            With dtValores.Rows(0)
                aMes(0) = CDbl(.Item("mENE"))
                aMes(1) = CDbl(.Item("mFEB"))
                aMes(2) = CDbl(.Item("mMAR"))
                aMes(3) = CDbl(.Item("mABR"))
                aMes(4) = CDbl(.Item("mMAY"))
                aMes(5) = CDbl(.Item("mJUN"))
                aMes(6) = CDbl(.Item("mJUL"))
                aMes(7) = CDbl(.Item("mAGO"))
                aMes(8) = CDbl(.Item("mSEP"))
                aMes(9) = CDbl(.Item("mOCT"))
                aMes(10) = CDbl(.Item("mNOV"))
                aMes(11) = CDbl(.Item("mDIC"))
            End With
        End If
        ValoresMensuales = aMes

    End Function
    Private Sub VerHistograma(ByVal Histograma As C1Chart, ByVal CantidadKilosMoney As Integer)

        Dim Area As Area = Histograma.ChartArea
        Dim ax As Axis = Area.AxisX
        Dim ay As Axis = Area.AxisY

        ax.ValueLabels.Clear()
        ax.ValueLabels.Add(1, "Ene")
        ax.ValueLabels.Add(2, "Feb")
        ax.ValueLabels.Add(3, "Mar")
        ax.ValueLabels.Add(4, "Abr")
        ax.ValueLabels.Add(5, "May")
        ax.ValueLabels.Add(6, "Jun")
        ax.ValueLabels.Add(7, "Jul")
        ax.ValueLabels.Add(8, "Ago")
        ax.ValueLabels.Add(9, "Sep")
        ax.ValueLabels.Add(10, "Oct")
        ax.ValueLabels.Add(11, "Nov")
        ax.ValueLabels.Add(12, "Dic")

        Dim aaY As Double() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim abY As Double() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Histograma.ChartGroups(0).ChartData.SeriesList(0).Y.CopyDataIn(aaY)
        Histograma.ChartGroups(0).ChartData.SeriesList(1).Y.CopyDataIn(abY)

        Dim dtt As New DataTable
        dtt = ConsultaEstadistica(myConn, ds, lblInfo, txtCodigo.Text, "VEN", cmbTipoEstadistica.SelectedIndex, IIf(rBtnV1.Checked, TipoDatoMercancia.iUnidadesDeVenta, IIf(rBtnV2.Checked, TipoDatoMercancia.iKilogramos, TipoDatoMercancia.iMonetarios)), jytsistema.sFechadeTrabajo, _
                                  "tblResumenAnoActualVentas", " a.id_emp ")

        Dim dttAnteriores As New DataTable
        dttAnteriores = ConsultaEstadistica(myConn, ds, lblInfo, txtCodigo.Text, "VEN", cmbTipoEstadistica.SelectedIndex, IIf(rBtnV1.Checked, TipoDatoMercancia.iUnidadesDeVenta, IIf(rBtnV2.Checked, TipoDatoMercancia.iKilogramos, TipoDatoMercancia.iMonetarios)), DateAdd(DateInterval.Year, -1, jytsistema.sFechadeTrabajo), _
                                            "tblResumenAnoAnteriorVentas", " a.id_emp ")

        aaY = ValoresMensuales(dtt)
        abY = ValoresMensuales(dttAnteriores)

        Dim aFFld() As String = {"id_emp"}
        Dim aSStr() As String = {jytsistema.WorkID}

        ay.Text = IIf(CantidadKilosMoney = 0, "Unidad de Venta", IIf(CantidadKilosMoney = 1, "Kilogramos", qFoundAndSign(myConn, lblInfo, "jsconctaemp", aFFld, aSStr, "moneda")))
        ax.Text = cmbTipoEstadistica.Text & " mes a mes"

        Histograma.ChartGroups(0).ChartData.SeriesList(0).Y.CopyDataIn(aaY)
        Histograma.ChartGroups(0).ChartData.SeriesList(1).Y.CopyDataIn(abY)

        Histograma.ChartGroups(0).ChartData.SeriesList(0).Label = Year(jytsistema.sFechadeTrabajo)
        Histograma.ChartGroups(0).ChartData.SeriesList(1).Label = Year(jytsistema.sFechadeTrabajo) - 1

        dtt = Nothing
        dttAnteriores = Nothing


    End Sub


    Private Sub AsignarExpediente(ByVal nCodigoCliente As String)

        Dim dtExp As DataTable
        Dim nTablaExp As String = "tblExpediente"
        Dim strSQLExp As String = " SELECT a.codcli, a.fecha, a.COMENTARIO, a.CONDICION, " _
                    & " ELT(a.condicion + 1, 'Activo', 'Bloqueado', 'Inactivo', 'Desincorporado') estatus, " _
                    & " IF(a.causa = '', 'NOTA DE USUARIO',  " _
                    & " IF( a.condicion = 0,  b.descrip, IF( a.condicion = 1, c.descrip, IF( a.condicion = 2, d.descrip, e.descrip ) ) )) descripcion, a.tipocondicion  " _
                    & " FROM jsvenexpcli a " _
                    & " LEFT JOIN jsconctatab b ON (a.causa = b.codigo AND a.id_emp = b.id_emp AND b.modulo = '00017') " _
                    & " LEFT JOIN jsconctatab c ON (a.causa = c.codigo AND a.id_emp = c.id_emp AND c.modulo = '00018') " _
                    & " LEFT JOIN jsconctatab d ON (a.causa = d.codigo AND a.id_emp = d.id_emp AND d.modulo = '00019') " _
                    & " LEFT JOIN jsconctatab e ON (a.causa = e.codigo AND a.id_emp = e.id_emp AND e.modulo = '00020') " _
                    & " WHERE " _
                    & " a.codcli = '" & nCodigoCliente & "' AND " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " ORDER BY a.fecha DESC "

        ds = DataSetRequery(ds, strSQLExp, myConn, nTablaExp, lblInfo)
        dtExp = ds.Tables(nTablaExp)

        Dim aCamExp() As String = {"fecha", "descripcion", "estatus", "comentario"}
        Dim aNomExp() As String = {"Fecha", "Descripción causa", "Condición", "Comentario"}
        Dim aAncexp() As Long = {140, 240, 100, 350}
        Dim aAliExp() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aForExp() As String = {sFormatoFechaCorta, "", "", ""}

        IniciarTabla(dgExpediente, dtExp, aCamExp, aNomExp, aAncexp, aAliExp, aForExp)

        txtChMes.Text = FormatoEntero(CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(*) from jsbanchedev where prov_cli = '" & nCodigoCliente & "' and year(fechadev) = " & Year(jytsistema.sFechadeTrabajo) & " and month(fechadev) = " & Month(jytsistema.sFechadeTrabajo) & " and id_emp = '" & jytsistema.WorkID & "' ")))
        txtChAno.Text = FormatoEntero(CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(*) from jsbanchedev where prov_cli = '" & nCodigoCliente & "' and year(fechadev) = " & Year(jytsistema.sFechadeTrabajo) & " and id_emp = '" & jytsistema.WorkID & "' ")))
        txtCHTotal.Text = FormatoEntero(CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(*) from jsbanchedev where prov_cli = '" & nCodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")))

        txtCHMontoMes.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(monto) from jsbanchedev where prov_cli = '" & nCodigoCliente & "' and year(fechadev) = " & Year(jytsistema.sFechadeTrabajo) & " and month(fechadev) = " & Month(jytsistema.sFechadeTrabajo) & " and id_emp = '" & jytsistema.WorkID & "' group by prov_cli ")))
        txtChMontoAno.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(monto) from jsbanchedev where prov_cli = '" & nCodigoCliente & "' and year(fechadev) = " & Year(jytsistema.sFechadeTrabajo) & " and id_emp = '" & jytsistema.WorkID & "' group by prov_cli ")))
        txtCHMontoTotal.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(monto) from jsbanchedev where prov_cli = '" & nCodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' group by prov_cli ")))

        Dim dtCHDev As DataTable
        Dim nTablaCHDev As String = "tblCHequesDevueltos"
        Dim strSQLCHDev As String = " SELECT a.numcheque, a.monto, a.fechadev, ELT( a.causa + 1, '1. FECHA DEFECTUOSA ', '2. FECHA ADELANTADA', '3. FALTA FIRMA', " _
                & " '4. DEFECTO FIRMA Y/O SELLO', '6. DEFECTO DE ENDOSO', '8. CUENTA CERRADA', " _
                & " '9. GIRA SOBRE FONDOS NO DISPONIBLES', '10. PAGO SUSPENDIDO', '11. NO ES A NUESTRO CARGO', " _
                & " '13. GIRADOR FALLECIDO', '14. PRESENTAR POR TAQUILLA', '15. DIRIGIRSE AL GIRADOR', " _
                & " '16. CANTIDAD DEFECTUOSA', '17. FALTA SELLO COMPENSACION', '18. PASO 2 VECES COMPENSACION', " _
                & " '19. CAUSA EXTERNA', '20. OTRA') causa " _
                & " FROM jsbanchedev a " _
                & " WHERE " _
                & " a.prov_cli = '" & nCodigoCliente & "' and " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " ORDER BY fechadev DESC "

        ds = DataSetRequery(ds, strSQLCHDev, myConn, nTablaCHDev, lblInfo)
        dtCHDev = ds.Tables(nTablaCHDev)

        Dim aCamCHD() As String = {"fechadev", "numcheque", "monto", "causa"}
        Dim aNomCHD() As String = {"Fecha", "Cheque", "Monto", "Causa Devolución"}
        Dim aAncCHD() As Long = {90, 90, 100, 200}
        Dim aAliCHD() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda}
        Dim aForCHD() As String = {sFormatoFechaCorta, "", sFormatoNumero, ""}

        IniciarTabla(dgCheques, dtCHDev, aCamCHD, aNomCHD, aAncCHD, aAliCHD, aForCHD, , , 8, False)


    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If dt.Rows.Count = 0 Then
            IniciarCliente(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            If Me.BindingContext(ds, nTabla).Position > 0 Then _
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
            i_modo = movimiento.iConsultar
            AsignaTXT(nPosicionCat)
        End If
        DesactivarMarco0()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub

    Private Sub btnAgregaCI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaCI.Click
        Dim g As New jsVenArcClientesCIs
        g.Agregar(myConn, ds, dtCIs, txtCodigo.Text, "0")
        AsignaCI(nPocisionCI, True)
        g = Nothing

    End Sub


    Private Sub btnEliminaCI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaCI.Click
        With dtCIs
            If .Rows.Count > 0 Then
                nPocisionCI = Me.BindingContext(ds, nTablaCIs).Position
                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                Dim aCamDel() As String = {"codcli", "nacional", "ci", "id_emp"}
                Dim aStrDel() As String = {txtCodigo.Text, Split(.Rows(nPocisionCI).Item("cedula"), "-")(0), Split(.Rows(nPocisionCI).Item("cedula"), "-")(1), jytsistema.WorkID}

                sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                If sRespuesta = MsgBoxResult.Yes Then AsignaCI(EliminarRegistros(myConn, lblInfo, ds, nTablaCIs, "jsvencedsoc", strSQLCIs, aCamDel, aStrDel, nPocisionCI, True), False)
            End If
        End With

    End Sub

    Private Sub dgCIs_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgCIs.RowHeaderMouseClick, _
        dgCIs.CellMouseClick, dgCIs.RegionChanged
        Me.BindingContext(ds, nTablaCIs).Position = e.RowIndex
        nPocisionCI = e.RowIndex
    End Sub


    Private Sub txtSugerido_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFax.KeyPress, _
        txtZIPDespacho.KeyPress, txtTerritorioDespacho.KeyPress, txtZIPFiscal.KeyPress, txttelef1.KeyPress, _
        txtTelef2.KeyPress, txtTelef3.KeyPress, txtAA.KeyPress, txtDisponible.KeyPress, _
        txtSaldo.KeyPress, txtTelefonoContacto.KeyPress, txtLimiteCredito.KeyPress, txtDescB.KeyPress, _
        txtFormaPago.KeyPress, txtDescA.KeyPress, txtDescuentoCliente.KeyPress, txtListaPrecios.KeyPress, _
         txtDeA.KeyPress, txtDeB.KeyPress, txtDeC.KeyPress, txtDeD.KeyPress

        e.Handled = ValidaNumeroEnTextbox(e)

    End Sub

    Private Sub cmbIVA_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbIVA.SelectedIndexChanged
        txtIVA.Text = FormatoNumero(PorcentajeIVA(myConn, lblInfo, jytsistema.sFechadeTrabajo, cmbIVA.Text)) & "%"
    End Sub

    Private Sub txtCanal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCanal.TextChanged
        Dim aFld() As String = {"codigo", "id_emp"}
        Dim aStr() As String = {txtCanal.Text, jytsistema.WorkID}
        txtCanalNombre.Text = qFoundAndSign(myConn, lblInfo, "jsvenliscan", aFld, aStr, "descrip")

    End Sub

    Private Sub txtTipoNegocio_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTipoNegocio.TextChanged
        Dim aFld() As String = {"codigo", "id_emp"}
        Dim aStr() As String = {txtTipoNegocio.Text, jytsistema.WorkID}
        txtTipoNegocioNombre.Text = qFoundAndSign(myConn, lblInfo, "jsvenlistip", aFld, aStr, "descrip")
    End Sub

    Private Sub btnCanal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCanal.Click
        Dim f As New jsVenArcCanalTiponegocio
        f.Grupo0 = txtCanal.Text
        f.Grupo1 = txtTipoNegocio.Text
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        If f.Grupo0 <> "" Then txtCanal.Text = f.Grupo0
        If f.Grupo1 <> "" Then txtTipoNegocio.Text = f.Grupo1
        f = Nothing
    End Sub
    Private Sub btnListaPrecios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnListaPrecios.Click
        txtListaPrecios.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codlis codigo, descrip descripcion from jsmerenclispre where id_emp = '" & jytsistema.WorkID & "' ", "Lista de precios", _
                                                 txtListaPrecios.Text)
    End Sub

    Private Sub rBtnV1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rBtnV1.CheckedChanged, _
        rBtnV2.CheckedChanged, rBtnV3.CheckedChanged
        If cmbTipoEstadistica.Items.Count = 2 AndAlso sender.Checked Then AbrirEstadisticas()
    End Sub


    Private Sub btnIVA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIVA.Click
        Dim f As New jsControlArcIVA
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)

        aIVA = ArregloIVA(myConn, lblInfo)
        RellenaCombo(aIVA, cmbIVA, InArray(aIVA, f.Seleccionado) - 1)

        f = Nothing

    End Sub

    Private Sub btnForma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForma.Click
        txtFormaPago.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codfor codigo, nomfor descripcion from jsconctafor where id_emp = '" & jytsistema.WorkID & "' ", "Formas de pago", _
                                              txtFormaPago.Text)
    End Sub

    Private Sub cmbTipoMovimiento_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipoMovimiento.SelectedIndexChanged
        Dim bs As New BindingSource
        Dim aTTipo() As String = {"", "0", "1", "FC", "GR", "ND", "AB", "CA", "NC"}
        bs.DataSource = dtMovimientos
        Select cmbTipoMovimiento.SelectedIndex
            Case 1, 2
                If dtMovimientos.Columns("fotipo").DataType Is GetType(String) Then _
                bs.Filter = " fotipo like '%" & aTTipo(cmbTipoMovimiento.SelectedIndex) & "%'"
            Case Else
                If dtMovimientos.Columns("tipomov").DataType Is GetType(String) Then _
                 bs.Filter = " tipomov like '%" & aTTipo(cmbTipoMovimiento.SelectedIndex) & "%'"
        End Select
        dg.DataSource = bs

    End Sub
    Private Function Territorio(ByVal Pais As String, ByVal Estado As String, ByVal Municipio As String, _
                                ByVal Parroquia As String, ByVal Ciudad As String, ByVal Barrio As String)

        Territorio = ColocaTerritorio(myConn, Pais) + IIf(Estado <> "", "->", "") _
                        + ColocaTerritorio(myConn, Estado) + IIf(Municipio <> "", "->", "") _
                        + ColocaTerritorio(myConn, Municipio) + IIf(Parroquia <> "", "->", "") _
                        + ColocaTerritorio(myConn, Parroquia) + IIf(Ciudad <> "", "->", "") _
                        + ColocaTerritorio(myConn, Ciudad) + IIf(Barrio <> "", "->", "") _
                        + ColocaTerritorio(myConn, Barrio)

    End Function
    Private Function ColocaTerritorio(ByVal MyConn As MySqlConnection, ByVal Territorio As String) As String

        Dim aCam() As String = {"codigo", "id_emp"}
        Dim aStr() As String = {Territorio, jytsistema.WorkID}
        ColocaTerritorio = qFoundAndSign(MyConn, lblInfo, "jsconcatter", aCam, aStr, "nombre")

    End Function


    Private Sub txtFormaPago_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFormaPago.TextChanged
        Dim afld() As String = {"codfor", "id_emp"}
        Dim aStr() As String = {txtFormaPago.Text, jytsistema.WorkID}
        txtFormaPagoNombre.Text = qFoundAndSign(myConn, lblInfo, "jsconctafor", afld, aStr, "nomfor")
    End Sub

    Private Sub cmbSaldos_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSaldos.SelectedIndexChanged
        IniciarSaldosPorDocumento(cmbSaldos.SelectedIndex, txtCodigo.Text)
    End Sub

    Private Sub IniciarSaldosPorDocumento(ByVal ActualHistorico As Integer, ByVal CodCliente As String)

        tblSaldos = "tbl" & NumeroAleatorio(100000)

        txtDocSel.Text = FormatoEntero(0)
        txtSaldoSel.Text = FormatoNumero(0.0)

        If CodCliente <> "" Then

            Dim aFields() As String = {"sel.entero", "codcli.cadena15", "nombre.cadena250", "nummov.cadena20", "tipomov.cadena2", _
                                       "refer.cadena15", "emision.fecha", "vence.fecha", "importe.doble19", "saldo.doble19", _
                                       "codven.cadena5", "nomVendedor.cadena50"}

            CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblSaldos, aFields)


            EjecutarSTRSQL(myConn, lblInfo, " INSERT INTO " & tblSaldos _
                        & " select 0 sel, a.codcli, b.nombre, a.nummov, a.tipomov, a.refer, a.emision, a.vence, a.importe, " _
                        & " IF(c.saldo IS NULL, 0.00, c.saldo) saldo, a.codven, concat(d.nombres, ' ', d.apellidos) nomVendedor " _
                        & " FROM jsventracob a  " _
                        & " LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
                        & " LEFT JOIN (SELECT codcli, nummov, SUM(importe) saldo " _
                        & "         	FROM jsventracob WHERE codcli = '" & CodCliente & "' AND id_emp = '" & jytsistema.WorkID & "' GROUP BY nummov HAVING ABS(ROUND(saldo,2)) > 0 ) c ON (a.codcli = c.codcli AND a.nummov = c.nummov ) " _
                        & " INNER JOIN (SELECT codcli, nummov, MIN(CONCAT(fechasi, nummov,emision,hora)) minimo " _
                        & "             FROM jsventracob WHERE historico = '" & ActualHistorico & "' AND ID_EMP = '" & jytsistema.WorkID & "' AND CODCLI = '" & txtCodigo.Text & "' GROUP BY nummov) d ON (CONCAT(a.fechasi,a.nummov,a.emision,a.hora) = d.minimo) " _
                        & " left join jsvencatven d on (a.codven = d.codven and a.id_emp = d.id_emp  ) " _
                        & " WHERE " _
                        & " a.historico = '" & ActualHistorico & "' AND " _
                        & " a.ID_EMP = '" & jytsistema.WorkID & "' AND " _
                        & " a.CODCLI = '" & CodCliente & "' " _
                        & " ORDER BY a.fechasi, a.nummov, a.emision, a.hora ")

            ds = DataSetRequery(ds, " select * from " & tblSaldos, myConn, nTablaSaldos, lblInfo)
            dtSaldos = ds.Tables(nTablaSaldos)

            CargarListaSaldosDocumentosCliente(dgSaldos, dtSaldos)
            dgSaldos.ReadOnly = False
            For Each col As DataGridViewColumn In dgSaldos.Columns
                If col.Index > 0 Then col.ReadOnly = True
            Next

            dgDocumentos.Columns.Clear()
            dgMercasDocumentos.Columns.Clear()

            txtDocSel.Text = FormatoEntero(0)
            txtSaldoSel.Text = FormatoNumero(0.0)

            CalculaTotalesSaldos()

        End If
    End Sub

    Private Sub CalculaTotalesSaldos()

        Dim strSiSel As String = ""
        strSel = " nummov = 'XX XX' OR "
        iSel = 0
        dSel = 0
        For Each selectedItem As DataGridViewRow In dgSaldos.Rows
            If selectedItem.Cells(0).Value Then
                iSel += 1
                dSel += CDbl(selectedItem.Cells(6).Value)
                strSel += " nummov = '" & selectedItem.Cells(1).Value & "' OR "
            End If
        Next

        strSiSel = "(" & strSel.Substring(0, strSel.Length - 4) & ") and "

        AbrirDocumentosSaldo(strSiSel)
        AbrirMercanciasSaldo(strSiSel)
        txtDocSel.Text = FormatoEntero(iSel)
        txtSaldoSel.Text = FormatoNumero(dSel)

    End Sub

    Private Sub AbrirDocumentosSaldo(ByVal strDocs As String)
        Dim dtDocSal As DataTable
        Dim nTablaDocSal As String = "tbldocumentosSaldo"

        Dim strSQLDocsSaldo As String = " select a.emision, a.tipomov, a.nummov, a.vence, a.refer, a.formapag, a.comproba, a.importe, a.origen, a.codven, " _
                    & " a.fotipo, " _
                    & " concat(b.nombres, ' ', b.apellidos) nomvendedor " _
                    & " from jsventracob a " _
                    & " left join jsvencatven b on (a.codven = b.codven and a.id_emp = b.id_emp ) " _
                    & " where " _
                    & strDocs _
                    & " a.codcli = '" & txtCodigo.Text & "' and " _
                    & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " order by " _
                    & " a.NUMMOV, a.FOTIPO, a.TIPOMOV, a.EMISION"

        ds = DataSetRequery(ds, strSQLDocsSaldo, myConn, nTablaDocSal, lblInfo)
        dtDocSal = ds.Tables(nTablaDocSal)

        Dim aCampos() As String = {"emision", "tipomov", "nummov", "vence", "refer", "formapag", "comproba", "importe", "origen", "codven", "nomvendedor"}
        Dim aNombres() As String = {"Emisión", "TP", "Documento", "Vence", "Referencia", "FP", "Comprobante", "Importe", "ORG", "Asesor", "Nombre"}
        Dim aAnchos() As Long = {80, 25, 90, 80, 80, 25, 90, 80, 35, 50, 80}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {sFormatoFecha, "", "", sFormatoFecha, "", "", "", sFormatoNumero, "", "", ""}
        IniciarTabla(dgDocumentos, dtDocSal, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

    End Sub
    Private Sub AbrirMercanciasSaldo(ByVal strDocs As String)

        Dim dtMerSal As DataTable
        Dim nTablaMerSal As String = "tblMercasSaldo"

        Dim strSQLMerSaldo As String = " SELECT a.numfac, a.item, a.descrip, a.iva, a.unidad, a.cantidad, a.precio, " _
            & " a.des_art, a.des_cli, a.des_ofe, a.totren, ELT(a.estatus+1,'','Sin Descuento','Bonificación') tipo FROM jsvenrenfac a " _
            & " left join jsvenencfac b on (a.numfac = b.numfac and a.id_emp = b.id_emp) " _
            & " WHERE " _
            & Replace(strDocs, "nummov", "a.numfac") _
            & " SUBSTRING(a.item,1,1) <> '$' AND " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
            & " union " _
            & " select a.numncr numfac, a.item, a.descrip, a.iva, a.unidad, a.cantidad, a.precio, " _
            & " a.des_art, a.des_cli, a.des_ofe, a.totren, elt(a.estatus+1,'','Sin Descuento','Bonificación') tipo from jsvenrenncr a " _
            & " left join jsvenencncr b on (a.numncr = b.numncr and a.id_emp = b.id_emp) " _
            & " where " _
            & Replace(strDocs, "nummov", "a.numncr") _
            & " substring(a.item,1,1) <> '$' and " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
            & " union " _
            & " select a.numndb numfac, a.item, a.descrip, a.iva, a.unidad, a.cantidad, a.precio, " _
            & " a.des_art, a.des_cli, a.des_ofe, a.totren, elt(a.estatus+1,'','Sin Descuento','Bonificación') tipo from jsvenrenndb a " _
            & " left join jsvenencndb b on (a.numndb = b.numndb and a.id_emp = b.id_emp) " _
            & " where " _
            & Replace(strDocs, "nummov", "a.numndb") _
            & " substring(a.item,1,1) <> '$' and " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' order by numfac, item "

        ds = DataSetRequery(ds, strSQLMerSaldo, myConn, nTablaMerSal, lblInfo)
        dtMerSal = ds.Tables(nTablaMerSal)

        Dim aCampos() As String = {"numfac", "item", "descrip", "iva", "unidad", "cantidad", "precio", "des_art", "des_cli", "des_ofe", "totren", "tipo"}
        Dim aNombres() As String = {"Documento", "ítem", "Descripción", "IVA", "UND", "Cantidad", "Precio", "Desc. Art.", "Desc. Cli.", "Desc. Ofe.", "Total", "Tipo"}
        Dim aAnchos() As Long = {90, 90, 150, 25, 35, 70, 70, 45, 45, 45, 70, 30}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", "", "", sFormatoCantidad, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, ""}
        IniciarTabla(dgMercasDocumentos, dtMerSal, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

    End Sub
    Private Sub AbrirEstadisticas()

        EsperaPorFavor()

        Dim aCam() As String = {"codart", "nomart", "unidad", "mEne", "mFeb", "mMar", "mAbr", "mMay", "mJun", "mJul", "mAgo", "mSep", "mOct", "mNov", "mDic"}
        Dim aNom() As String = {"Código", "Nombre artículo", "UND", "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"}
        Dim aAnc() As Long = {70, 280, 35, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}

        Dim aFor() As String = {"", "", "", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero}

        Dim dtStat As New DataTable
        dtStat = ConsultaEstadistica(myConn, ds, lblInfo, txtCodigo.Text, "VEN", _
                cmbTipoEstadistica.SelectedIndex, IIf(rBtnV1.Checked, TipoDatoMercancia.iUnidadesDeVenta, IIf(rBtnV2.Checked, TipoDatoMercancia.iKilogramos, TipoDatoMercancia.iMonetarios)), jytsistema.sFechadeTrabajo, "tblMovimientosAnoActualVentas")

        IniciarTabla(dgEstadistica, dtStat, aCam, aNom, aAnc, aAli, aFor)

        VerHistograma(C1Chart2, IIf(rBtnV1.Checked, TipoDatoMercancia.iUnidadesDeVenta, IIf(rBtnV2.Checked, TipoDatoMercancia.iKilogramos, TipoDatoMercancia.iMonetarios)))

    End Sub

    Private Sub cmbTipoEstadistica_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipoEstadistica.SelectedIndexChanged
        If cmbTipoEstadistica.Items.Count = 2 Then AbrirEstadisticas()
    End Sub

    Private Sub txtNombre_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNombre.TextChanged
        txtNombreClienteii.Text = txtNombre.Text
    End Sub

    Private Sub btnInicioVisita_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInicioVisita.Click
        txtInicioVisita.Text = SeleccionaFecha(CDate(txtInicioVisita.Text), Me.Parent, Me, grpCompras, btnInicioVisita)
    End Sub

    Private Sub btnIngreso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngreso.Click
        txtIngreso.Text = SeleccionaFecha(CDate(txtIngreso.Text), Me.Parent, Me, grpOtros, btnIngreso)
    End Sub

    Private Sub btnSubirHistorico_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubirHistorico.Click
        If tbcClientes.SelectedIndex = 2 Then
            Dim f As New jsVenProHistoricoClientes
            f.Cargar(myConn, iProceso.Procesar, txtCodigoMovimientos.Text)
            f = Nothing
            AbrirMovimientos(txtCodigoMovimientos.Text)
        End If
    End Sub

    Private Sub btnBajarHistorico_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBajarHistorico.Click
        If tbcClientes.SelectedIndex = 2 Then
            Dim f As New jsVenProHistoricoClientes
            f.Cargar(myConn, iProceso.Reversar, txtCodigoMovimientos.Text)
            f = Nothing
            AbrirMovimientos(txtCodigoMovimientos.Text)
        End If
    End Sub

    Private Sub btnAgregarVisita_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarVisita.Click
        Dim f As New jsVenArcClientesVisitas
        f.Agregar(myConn, ds, dtVisitas, txtCodigo.Text, 0)
        f = Nothing
        AbrirVisitas(txtCodigo.Text)
    End Sub

    Private Sub btnAgregarDespacho_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarDespacho.Click
        Dim f As New jsVenArcClientesVisitas
        f.Agregar(myConn, ds, dtDespachos, txtCodigo.Text, 1)
        f = Nothing
        AbrirDespachos(txtCodigo.Text)
    End Sub

    Private Sub btnEliminarProveedorExclusivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarProveedorExclusivo.Click
        With dtProveedores
            If .Rows.Count > 0 Then
                nPosicionProveedor = Me.BindingContext(ds, nTablaProveedores).Position
                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                Dim aCamDel() As String = {"codcli", "codpro", "id_emp"}
                Dim aStrDel() As String = {txtCodigo.Text, .Rows(nPosicionProveedor).Item("codpro"), jytsistema.WorkID}

                sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                If sRespuesta = MsgBoxResult.Yes Then AsignaProveedores(EliminarRegistros(myConn, lblInfo, ds, nTablaProveedores, _
                        "jsvenprocli", strSQLProveedores, aCamDel, aStrDel, nPosicionProveedor, True), False)
            End If
        End With
    End Sub

    Private Sub btnAgregarProveedorExclusivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarProveedorExclusivo.Click
        Dim f As New jsGenListadoSeleccion
        Dim aNombres() As String = {"", "Código", "Nombre y/o Razón social proveedor"}
        Dim aCampos() As String = {"sel", "codpro", "nombre"}
        Dim aAnchos() As Long = {20, 100, 550}
        Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left}
        Dim aFormato() As String = {"", "", ""}
        Dim aFields() As String = {"sel.entero", "codpro.cadena20", "nombre.cadena250"}


        f.Cargar(myConn, ds, "Listado de Proveedores", " select 0 sel, codpro, nombre from jsprocatpro where tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by codpro ", _
            aFields, aNombres, aCampos, aAnchos, aAlineacion, aFormato)

        If f.Seleccion.Length > 0 Then
            Dim cod As String
            For Each cod In f.Seleccion
                If CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select codpro from jsvenprocli where codcli = '" & txtCodigo.Text _
                                         & "' and codpro = '" & cod & "' and id_emp = '" & jytsistema.WorkID & "'  ")) = "0" Then
                    EjecutarSTRSQL(myConn, lblInfo, " insert into jsvenprocli set codcli = '" & txtCodigo.Text & "', codpro = '" & cod & "', id_emp = '" & jytsistema.WorkID & "'   ")
                End If
            Next
        AsignaProveedores(0, True)
        End If
        f = Nothing

    End Sub

    Private Sub dgProveedores_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgProveedores.RowHeaderMouseClick, _
         dgProveedores.CellMouseClick, dgProveedores.RegionChanged
        Me.BindingContext(ds, nTablaProveedores).Position = e.RowIndex
        nPosicionProveedor = e.RowIndex
    End Sub


    Private Sub btnEliminarVisita_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarVisita.Click
        With dtVisitas
            If .Rows.Count > 0 Then
                nPosicionVisitas = Me.BindingContext(ds, nTablaVisitas).Position
                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                Dim aCamDel() As String = {"codcli", "dia", "tipo", "id_emp"}
                Dim aStrDel() As String = {txtCodigo.Text, .Rows(nPosicionVisitas).Item("dia"), 0, jytsistema.WorkID}

                sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                If sRespuesta = MsgBoxResult.Yes Then
                    EliminarRegistros(myConn, lblInfo, ds, nTablaVisitas, _
                                           "jsvencatvis", strSQLVisitas, aCamDel, aStrDel, nPosicionVisitas, True)
                    AbrirVisitas(txtCodigo.Text)
                End If
            End If

        End With
    End Sub

    Private Sub btnEliminarDespacho_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarDespacho.Click
        With dtDespachos
            If .Rows.Count > 0 Then
                nPosicionDespachos = Me.BindingContext(ds, nTablaDespachos).Position
                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                Dim aCamDel() As String = {"codcli", "dia", "tipo", "id_emp"}
                Dim aStrDel() As String = {txtCodigo.Text, .Rows(nPosicionVisitas).Item("dia"), 1, jytsistema.WorkID}

                sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                If sRespuesta = MsgBoxResult.Yes Then
                    EliminarRegistros(myConn, lblInfo, ds, nTablaDespachos, _
                                           "jsvencatvis", strSQLDespachos, aCamDel, aStrDel, nPosicionDespachos, True)
                    AbrirDespachos(txtCodigo.Text)
                End If
            End If

        End With
    End Sub

    Private Sub btnTerritorioFiscal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTerritorioFiscal.Click
        Dim f As New jsControlArcTerritorio
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        Dim aTerritorio() As String = Split(f.CodigoTerritorio, ".")

        If aTerritorio.Length >= 1 AndAlso aTerritorio(0) <> "" Then txtPaisFiscal.Text = aTerritorio(0).ToString
        If aTerritorio.Length >= 2 AndAlso aTerritorio(1) <> "" Then txtEstadoFiscal.Text = aTerritorio(1).ToString
        If aTerritorio.Length >= 3 AndAlso aTerritorio(2) <> "" Then txtMunicipioFiscal.Text = aTerritorio(2).ToString
        If aTerritorio.Length >= 4 AndAlso aTerritorio(3) <> "" Then txtParroquiaFiscal.Text = aTerritorio(3).ToString
        If aTerritorio.Length >= 5 AndAlso aTerritorio(4) <> "" Then txtCiudadFiscal.Text = aTerritorio(4).ToString
        If aTerritorio.Length >= 6 AndAlso aTerritorio(5) <> "" Then txtBarrioFiscal.Text = aTerritorio(5).ToString

        txtTerritorioFiscal.Text = Territorio(txtPaisFiscal.Text, txtEstadoFiscal.Text, txtMunicipioFiscal.Text, _
                                                              txtParroquiaFiscal.Text, txtCiudadFiscal.Text, txtBarrioFiscal.Text)

        f.Dispose()
        f = Nothing

    End Sub

    Private Sub btnTerritorioDespacho_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTerritorioDespacho.Click

        Dim f As New jsControlArcTerritorio
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        Dim aTerritorio() As String = Split(f.CodigoTerritorio, ".")

        If aTerritorio.Length >= 1 AndAlso aTerritorio(0) <> "" Then txtPaisDespacho.Text = aTerritorio(0).ToString
        If aTerritorio.Length >= 2 AndAlso aTerritorio(1) <> "" Then txtEstadoDespacho.Text = aTerritorio(1).ToString
        If aTerritorio.Length >= 3 AndAlso aTerritorio(2) <> "" Then txtMunicipioDespacho.Text = aTerritorio(2).ToString
        If aTerritorio.Length >= 4 AndAlso aTerritorio(3) <> "" Then txtParroquiaDespacho.Text = aTerritorio(3).ToString
        If aTerritorio.Length >= 5 AndAlso aTerritorio(4) <> "" Then txtCiudadDespacho.Text = aTerritorio(4).ToString
        If aTerritorio.Length >= 6 AndAlso aTerritorio(5) <> "" Then txtBarrioDespacho.Text = aTerritorio(5).ToString

        txtTerritorioDespacho.Text = Territorio(txtPaisDespacho.Text, txtEstadoDespacho.Text, txtMunicipioFiscal.Text, _
                                                              txtParroquiaFiscal.Text, txtCiudadFiscal.Text, txtBarrioDespacho.Text)

        f.Dispose()
        f = Nothing
    End Sub

    Private Sub cmbRanking_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbRanking.SelectedIndexChanged
        txtFechaRanking.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
    End Sub

    Private Sub txtRIF_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtRIF.TextChanged
        txtTipoContribuyente.Text = ""

        Dim RifModificado As String = Replace(txtRIF.Text, ".", "")

        If RifModificado <> "" Then
            If RifModificado.Length > 3 Then
                If Mid(RifModificado, RifModificado.Length, 1) <> "." Then
                    If Mid(RifModificado, RifModificado.Length - 1, 1) = "-" Then
                        txtTipoContribuyente.Text = "C"
                    Else
                        txtTipoContribuyente.Text = "NC"
                    End If
                End If
            Else
                txtTipoContribuyente.Text = "NC"
            End If
        End If

        VerificaSucursal(txtRIF.Text)

    End Sub
    Private Sub VerificaSucursal(ByVal RIF As String)
        If i_modo = movimiento.iAgregar Then
            If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, "SELECT COUNT(*) FROM jsvencatcli WHERE RIF = '" & RIF & "' AND id_emp = '" & jytsistema.WorkID & "' GROUP BY rif")) > 0 Then

                MsgBox("ESTE CLIENTE YA SE ENCUENTRA EN BASE DE DATOS. LOS DATOS INGRESADOS A CONTINUACION " + _
                       "SERAN TRATADOS COMO DE UNA SUCURSAL. POR FAVOR COLOQUE EN CODIGO ALTERNO EL CODIGO " + _
                       "DE DESPACHO <<SADA>>. SI ES ATENDIDO POR UN ASESOR COMERCIAL DEBE SER INCLUIDO EN  " + _
                       "UNA RUTA DE VISITA. MIENTRAS SE ASIGNARA A UNA RUTA PROVISIONAL", MsgBoxStyle.Information)

                Dim dtClientePrincipal As DataTable
                Dim nTablaClientePrincipal As String = "tblClientePrincipal"
                ds = DataSetRequery(ds, " select * from jsvencatcli where RIF = '" & RIF & "' and id_emp = '" & jytsistema.WorkID & "' ORDER BY CODCLI LIMIT 1 ", myConn, nTablaClientePrincipal, lblInfo)

                dtClientePrincipal = ds.Tables(nTablaClientePrincipal)

                If dtClientePrincipal.Rows.Count > 0 Then
                    With dtClientePrincipal.Rows(0)
                        txtNombre.Text = .Item("nombre")
                        txtDireccionFiscal.Text = .Item("dirfiscal")
                        txtCanal.Text = .Item("categoria")
                        txtTipoNegocio.Text = .Item("unidad")
                        txtZIPFiscal.Text = .Item("fzip")
                        txtPaisFiscal.Text = .Item("fpais")
                        txtEstadoFiscal.Text = .Item("festado")
                        txtMunicipioFiscal.Text = .Item("fmunicipio")
                        txtParroquiaFiscal.Text = .Item("fParroquia")
                        txtCiudadFiscal.Text = .Item("fCiudad")
                        txtBarrioFiscal.Text = .Item("fBarrio")
                    End With
                    HabilitarObjetos(False, True, txtNombre, btnCanal, txtDireccionFiscal, txtZIPFiscal, btnTerritorioFiscal)
                End If

                dtClientePrincipal.Dispose()
                dtClientePrincipal = Nothing
            Else
                    txtNombre.Text = ""
                    txtDireccionFiscal.Text = ""
                    txtCanal.Text = ""
                    txtTipoNegocio.Text = ""
                    txtZIPFiscal.Text = ""
                    txtPaisFiscal.Text = ""
                    txtEstadoFiscal.Text = ""
                    txtMunicipioFiscal.Text = ""
                    txtParroquiaFiscal.Text = ""
                    txtCiudadFiscal.Text = ""
                    txtBarrioFiscal.Text = ""
                    HabilitarObjetos(True, True, txtNombre, btnCanal, txtDireccionFiscal, txtZIPFiscal, btnTerritorioFiscal)
                End If
            End If

    End Sub
    Private Sub txtLimiteCredito_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtLimiteCredito.TextChanged

    End Sub

    Private Sub txtCodigo_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCodigo.TextChanged
        txtCodigoClientesii.Text = txtCodigo.Text
        txtCodigoMovimientos.Text = txtCodigo.Text
        txtCodigoSaldos.Text = txtCodigo.Text
        txtCodigoEstadisticas.Text = txtCodigo.Text
        txtCodigoExpediente.Text = txtCodigo.Text
    End Sub

    
    Private Sub c1Chart1_ShowTooltip(ByVal sender As Object, ByVal e As C1.Win.C1Chart.ShowTooltipEventArgs) Handles C1Chart2.ShowTooltip
        If TypeOf sender Is ChartDataSeries Then
            ' Create new tooltip text
            'If c1Chart1.ToolTip.PlotElement = PlotElementEnum.Series Then
            Dim ds As ChartDataSeries = CType(sender, ChartDataSeries)

            Dim p As Point = Control.MousePosition
            p = C1Chart2.PointToClient(p)

            Dim x As Double = 0
            Dim y As Double = 0

            ' Callculate data coordinates
            Dim aNom() As String = {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"}
            If ds.Group.CoordToDataCoord(p.X, p.Y, x, y) Then
                e.TooltipText = String.Format("{0}" + ControlChars.Lf + "Mes = " + aNom(Math.Round(x) - 1) + _
                                              ControlChars.Lf + "Valor = {2:#.##}", ds.Label, x, y)
            Else
                e.TooltipText = ""
            End If
            'End If
        End If
    End Sub
    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(ds, nTablaMovimientos).Position += 1
                nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(nPosicionMov, False)
            Case Keys.Up
                Me.BindingContext(ds, nTablaMovimientos).Position -= 1
                nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(nPosicionMov, False)
        End Select
    End Sub

    
    Private Sub dgSaldos_CellMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgSaldos.CellMouseUp
        CalculaTotalesSAldos()
    End Sub

    Private Sub dgSaldos_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgSaldos.CellContentClick
        If e.ColumnIndex = 0 Then
            dtSaldos.Rows(e.RowIndex).Item(0) = Not CBool(dtSaldos.Rows(e.RowIndex).Item(0).ToString)
        End If
    End Sub

    Private Sub dgSaldos_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgSaldos.CellValidated
        If dgSaldos.CurrentCell.ColumnIndex = 0 Then

            With dgSaldos.CurrentRow
                EjecutarSTRSQL(myConn, lblInfo, " update  " & tblSaldos & " set sel  = " & CInt(dgSaldos.CurrentCell.Value) & " " _
                                    & " where " _
                                    & " emision = '" & FormatoFechaMySQL(CDate(.Cells(3).Value.ToString)) & "' and " _
                                    & " vence = '" & FormatoFechaMySQL(CDate(.Cells(4).Value.ToString)) & "' and " _
                                    & " tipomov = '" & CStr(.Cells(2).Value) & "' and " _
                                    & " nummov = '" & CStr(.Cells(1).Value) & "' ")
            End With

        End If

    End Sub

    Private Sub cmbFacturaDesde_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbFacturaDesde.SelectedIndexChanged

        If cmbFacturaDesde.SelectedIndex <> 0 Then
            If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) = 0 Then
                MensajeCritico(lblInfo, "NO POSEE PERMISOS NECESARIO PARA CAMBIAR ESTA CONDICION...")
                cmbFacturaDesde.SelectedIndex = 0
            End If
        End If
    End Sub
End Class