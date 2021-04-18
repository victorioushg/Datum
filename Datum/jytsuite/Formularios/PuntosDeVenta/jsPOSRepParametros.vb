Imports MySql.Data.MySqlClient
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports ReportesDePuntosDeVentas
Public Class jsPOSRepParametros

    Private Const sModulo As String = "Reportes de Puntos de Ventas"

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

    Private aSiNoTodos() As String = {"Si", "No", "Todos"}

    Private aEstatusFacturas() As String = {"Por confirmar", "Confirmadas", "Anuladas", "Todas"}
    Private aCondicionPago() As String = {"Crédito", "Contado", "Todos"}
    Private aEstatus() As String = {"Activo", "Bloqueado", "Inactivo", "Desincorporado", "Todos"}
    Private aTipo() As String = {"Ordinario", "Especial", "Formal", "No contribuyente", "Todos"}
    Private PeriodoTipo As TipoPeriodo
    Public Sub Cargar(ByVal TipoCarga As Integer, ByVal numReporte As Integer, ByVal nomReporte As String, _
                      Optional ByVal CodCliente As String = "", Optional ByVal numDocumento As String = "", _
                      Optional ByVal Fecha As Date = #1/1/2009#)



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
            Case ReportePuntoDeVenta.cFacturacionBackEnd
                Dim vOrdenNombres() As String = {"Número Factura"}
                Dim vOrdenCampos() As String = {"numfac"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoCliente)
            Case ReportePuntoDeVenta.cReporteX, ReportePuntoDeVenta.cReporteXPlus
                Dim vOrdenNombres() As String = {"Caja"}
                Dim vOrdenCampos() As String = {"caja"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud)

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
        ft.RellenaCombo(vCXCAgrupadoPor, cmbCXCAgrupadorPor)
    End Sub

    Private Sub IniciarCriterios()

        VerCriterio_Periodo(False, 0)
        VerCriterio_TipoDocumento(False)
        VerCriterio_Cliente(False)
        VerCriterio_Cajero(False)
        Vercriterio_Caja(False)

        Select Case ReporteNumero
            Case ReportePuntoDeVenta.cFacturacionBackEnd
                VerCriterio_Periodo(True, 0, TipoPeriodo.iDiario)
                VerCriterio_Cajero(True)
                Vercriterio_Caja(True)
            Case ReportePuntoDeVenta.cReporteX, ReportePuntoDeVenta.cReporteXPlus
                VerCriterio_Periodo(True, 0, TipoPeriodo.iDiario)
                VerCriterio_Cajero(True)
            Case Else

        End Select

    End Sub

    Private Sub VerCriterio_Periodo(ByVal Ver As Boolean, ByVal CompletoDesdeHasta As Integer, Optional ByVal Periodo As TipoPeriodo = TipoPeriodo.iMensual)
        'CompletoDesdeHasta 0 = Complete , 1 = Desde , 2 = Hasta 
        PeriodoTipo = Periodo
        ft.visualizarObjetos(False, lblPeriodoDesde, lblPeriodoHasta, lblPeriodo, txtPeriodoDesde, txtPeriodoHasta, btnPeriodoDesde, btnPeriodoHasta)
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
        'DocumentosTipo : 0 = Bancos, 1 = caja, 2 = Forma de pago
        ft.visualizarObjetos(ver, lblTipodocumento, chkList)

    End Sub
    Private Sub VerCriterio_Cliente(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblcliente, lblClienteDesde, txtClienteDesde, btnClienteDesde, _
                                lblClienteHasta, txtClienteHasta, btnClienteHasta)
        ft.habilitarObjetos(True, True, txtClienteDesde, txtClienteHasta)
        txtClienteDesde.MaxLength = 15
        txtClienteHasta.MaxLength = 15

    End Sub

    Private Sub VerCriterio_Cajero(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblCajero, txtCajeroDesde, btnCajeroDesde, _
                                txtCajeroHasta, btnCajeroHasta)
        ft.habilitarObjetos(True, True, txtCajeroDesde, txtCajeroHasta)
        txtCajeroDesde.MaxLength = 5
        txtCajeroHasta.MaxLength = 5

    End Sub

    Private Sub VerCriterio_Caja(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblCaja, txtCajaDesde, btnCajaDesde, _
                                txtCajaHasta, btnCajaHasta)
        ft.habilitarObjetos(True, True, txtCajaDesde, txtCajaHasta)
        txtCajeroDesde.MaxLength = 5
        txtCajeroHasta.MaxLength = 5

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

        Select Case ReporteNumero
            Case ReportePuntoDeVenta.cReporteX, ReportePuntoDeVenta.cReporteXPlus
                verConstante_Resumen(True)
                VerConstante_peso(True)
                lblPeso.Text = "EXCLUYE RETENCIONES/RETIROS"
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
    Private Sub TipoOrden(ByVal cTipo As String)
        Select Case vOrdenTipo(IndiceReporte)
            Case "D"
                txtOrdenDesde.Text = ft.FormatoFecha(PrimerDiaMes(jytsistema.sFechadeTrabajo))
                txtOrdenHasta.Text = ft.FormatoFecha(UltimoDiaMes(jytsistema.sFechadeTrabajo))
                txtOrdenDesde.Enabled = False
                txtOrdenHasta.Enabled = False
            Case Else
                txtOrdenDesde.Text = CodigoCliente
                txtOrdenHasta.Text = CodigoCliente
        End Select

    End Sub

    Private Sub jsPOSRepParametros_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsVenRepParametros_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(myConn, MovAud.ientrar, sModulo, ReporteNumero)
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

        HabilitarCursorEnEspera()

        EsperaPorFavor()

        Dim r As New frmViewer
        Dim dsVen As New DataSet
        Dim str As String = ""
        Dim nTabla As String = ""
        Dim oReporte As New CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim PresentaArbol As Boolean = False

        Select Case ReporteNumero
            Case ReportePuntoDeVenta.cFacturacionBackEnd
                nTabla = "dtPOSFacturas"
                oReporte = New rptPOSFacturas

                str = SeleccionPOSFacturas(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), cmbOrdenDesde.SelectedIndex, _
                                           CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), txtCajeroDesde.Text, txtCajeroHasta.Text, txtCajaDesde.Text, txtCajaHasta.Text)
            Case ReportePuntoDeVenta.cReporteX
                nTabla = "dtPOSReporteXPlus"
                oReporte = New rptPOSReporteXPlus

                str = SeleccionPOSReporteX(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), _
                                           txtOrdenDesde.Text, txtOrdenHasta.Text, _
                                           txtCajeroDesde.Text, txtCajeroHasta.Text, chkPeso.Checked)


            Case Else
                oReporte = Nothing
        End Select

        If nTabla <> "" Then
            dsVen = DataSetRequery(dsVen, str, myConn, nTabla, lblInfo)
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
                ft.mensajeCritico("No existe información que cumpla con estos criterios y/o constantes ")
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

        oReporte.SetDataSource(ds)
        oReporte.Refresh()
        oReporte.SetParameterValue("strLogo", CaminoImagen)
        oReporte.SetParameterValue("Orden", "Ordenado por : " + cmbOrdenadoPor.Text + " " + txtOrdenDesde.Text + "/" + txtOrdenHasta.Text)
        oReporte.SetParameterValue("RIF", rif)
        oReporte.SetParameterValue("Grupo", LineaGrupos)
        oReporte.SetParameterValue("Criterios", LineaCriterios)
        oReporte.SetParameterValue("Constantes", LineaConstantes)
        oReporte.SetParameterValue("Empresa", jytsistema.WorkName.TrimEnd(" "))
        Select Case ReporteNumero
            Case ReportePuntoDeVenta.cReporteX
                oReporte.SetParameterValue("Resumido", chkConsResumen.Checked)
                'oReporte.ReportDefinition.Sections("DetailSection3").SectionFormat.EnableSuppress = chkConsResumen.Checked
                '
        End Select

        ' Select Case ReporteNumero
        '     Case ReporteVentas.cClientes, ReporteVentas.cListadoFacturas, ReporteVentas.cSaldos
        ' Select Case cmbCXCAgrupadorPor.SelectedIndex
        '     Case 1, 2, 3, 4, 5
        ' oReporte.SetParameterValue("Grupo1", "canal")
        ' Dim FieldDef1 As FieldDefinition
        ' FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex))
        ' oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
        '     Case 7, 8
        ' oReporte.SetParameterValue("Grupo1", "canal")
        ' oReporte.SetParameterValue("Grupo2", "tiponegocio")
        ' Dim aFld() As String = Split(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex), ",")
        ' Dim FieldDef1, FieldDef2 As FieldDefinition
        ' FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(0)))
        ' FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(1)))
        ' oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
        ' oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
        '     Case 9, 10
        ' oReporte.SetParameterValue("Grupo1", "asesor")
        ' oReporte.SetParameterValue("Grupo2", "canal")
        ' oReporte.SetParameterValue("Grupo3", "tiponegocio")
        ' Dim aFld() As String = Split(aCXCAgrupadoPor(cmbCXCAgrupadorPor.SelectedIndex), ",")
        ' Dim FieldDef1, FieldDef2, FieldDef3 As FieldDefinition
        ' FieldDef1 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(0)))
        ' FieldDef2 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(1)))
        ' FieldDef3 = oReporte.Database.Tables.Item(0).Fields.Item(Trim(aFld(2)))
        ' oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef1
        ' oReporte.DataDefinition.Groups.Item(1).ConditionField = FieldDef2
        ' oReporte.DataDefinition.Groups.Item(2).ConditionField = FieldDef3
        '     Case Else
        ' End Select
        ' End Select

        PresentaReporte = oReporte

    End Function
    Private Sub btnPeriodoDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btnPeriodoDesde.Click
        txtPeriodoDesde.Text = SeleccionaFecha(CDate(txtPeriodoDesde.Text), Me, sender)
        Select Case PeriodoTipo
            Case TipoPeriodo.iDiario
                txtPeriodoHasta.Text = txtPeriodoDesde.Text
            Case TipoPeriodo.iMensual
                txtPeriodoHasta.Text = ft.FormatoFecha(UltimoDiaMes(CDate(txtPeriodoDesde.Text)))
        End Select
    End Sub

    Private Sub btnPeriodoHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btnPeriodoHasta.Click
        txtPeriodoHasta.Text = SeleccionaFecha(CDate(txtPeriodoHasta.Text), Me, sender)
    End Sub

    Private Sub btnLimpiar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLimpiar.Click
        LimpiarOrden()
        LimpiarGrupos()
        limpiarCriterios()
    End Sub
    Private Sub LimpiarGrupos()
        LimpiarTextos(txtCanalDesde, txtCanalHasta, txtRutaHasta, txtAsesorDesde, txtPais, txtAsesorHasta, _
            txtEstado, txtMunicipio, txtZonaDesde, txtZonaHasta, txtTipoNegocioDesde, txtTipoNegocioHasta)
    End Sub
    Private Sub LimpiarOrden()
        LimpiarTextos(txtOrdenDesde, txtOrdenHasta)
    End Sub
    Private Sub limpiarCriterios()
        LimpiarTextos(txtCajaDesde, txtCajaHasta, txtCajeroDesde, txtCajeroHasta)
    End Sub

    Private Sub chkList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim iCont As Integer
        'txtTipDoc.Text = ""
        'For iCont = 0 To chkList.Items.Count - 1
        ' If chkList.GetItemCheckState(iCont) = CheckState.Checked Then
        ' txtTipDoc.Text += "." + chkList.Items(iCont).ToString
        ' End If
        ' Next
    End Sub
    Private Function LineaGrupos() As String
        LineaGrupos = ""
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


    End Function
    Private Function LineaCriterios() As String
        LineaCriterios = ""
        If lblPeriodo.Visible Then LineaCriterios += "Período: " & IIf(lblPeriodoDesde.Visible, txtPeriodoDesde.Text, "") & IIf(lblPeriodoDesde.Visible AndAlso lblPeriodoHasta.Visible, "/", "") & IIf(lblPeriodoHasta.Visible, txtPeriodoHasta.Text, "")
        If LineaCriterios <> "" Then LineaCriterios += " - "
        If lblTipodocumento.Visible Then LineaCriterios += " Tipo Documentos : " + txtTipDoc.Text
        If LineaCriterios <> "" Then LineaCriterios += " - "
        If lblcliente.Visible Then LineaCriterios += " Cliente : " + txtClienteDesde.Text + "/" + txtClienteHasta.Text
    End Function
    Private Function LineaConstantes() As String
        LineaConstantes = ""
        If lblConsResumen.Visible Then LineaConstantes += "Resumido : " + IIf(chkConsResumen.Checked, "Si", "No")
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblCondicionPago.Visible Then LineaConstantes += " Condición Pago : " + aCondicionPago(cmbCondicionPago.SelectedIndex)
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblEstatusFactura.Visible Then LineaConstantes += " Estatus Documento : " + aEstatusFacturas(cmbEstatusFacturas.SelectedIndex)
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblTipo.Visible Then LineaConstantes += " Tipo Cliente : " + aTipo(cmbTipo.SelectedIndex)
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblEstatus.Visible Then LineaConstantes += " Estatus Clientes : " + aEstatus(cmbEstatus.SelectedIndex)
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
        Select Case cmbOrdenadoPor.Text
            Case "Código cliente"
            Case "Nombre cliente"
            Case "Número Factura"
        End Select
        f.Close()
        f = Nothing
        dtDeOrden.Dispose()
        dtDeOrden = Nothing

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
        txtRutaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codrut codigo, nomrut descripcion from jsvenencrut where tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1", "Rutas de visita", _
                                              txtRutaDesde.Text)
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
        txtPais.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, nombre descripcion from jsconcatter where tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1", "Paises", txtPais.Text)
    End Sub

    Private Sub btnEstado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEstado.Click
        If txtPais.Text <> "" Then _
            txtEstado.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, nombre descripcion from jsconcatter where tipo = 1 and antecesor = " & txtPais.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Estados", txtEstado.Text)

    End Sub

    Private Sub btnMunicipio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMunicipio.Click
        If txtEstado.Text <> "" Then _
            txtMunicipio.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, nombre descripcion from jsconcatter where tipo = 2 and antecesor = " & txtEstado.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Municipios", txtMunicipio.Text)
    End Sub

    Private Sub btnParroquia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnParroquia.Click
        If txtMunicipio.Text <> "" Then _
            txtParroquia.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, nombre descripcion from jsconcatter where tipo = 2 and antecesor = " & txtMunicipio.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Parroquias", txtParroquia.Text)
    End Sub

    Private Sub btnCiudad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCiudad.Click
        If txtParroquia.Text <> "" Then _
                    txtCiudad.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, nombre descripcion from jsconcatter where tipo = 3 and antecesor = " & txtParroquia.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Ciudades", txtCiudad.Text)
    End Sub

    Private Sub btnBarrio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBarrio.Click
        If txtCiudad.Text <> "" Then _
                    txtBarrio.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, nombre descripcion from jsconcatter where tipo = 4 and antecesor = " & txtCiudad.Text & " and id_emp = '" & jytsistema.WorkID & "' order by 1", "Barrios", txtBarrio.Text)
    End Sub


    Private Sub btnCajaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCajaDesde.Click
        txtCajaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcaj codigo, descrip descripcion from jsvencatcaj where id_emp = '" & jytsistema.WorkID & "' order by codcaj ", "Cajas", txtCajaDesde.Text)
    End Sub

    Private Sub btnCajaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCajaHasta.Click
        txtCajaHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcaj codigo, descrip descripcion from jsvencatcaj where id_emp = '" & jytsistema.WorkID & "' order by codcaj ", "Cajas", txtCajaHasta.Text)
    End Sub

    Private Sub txtCajaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCajaDesde.TextChanged
        txtCajaHasta.Text = txtCajaDesde.Text
    End Sub

    Private Sub btnCajeroDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCajeroDesde.Click
        txtCajeroDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, apellidos descripcion from jsvencatven where tipo = 1 and id_emp = '" & jytsistema.WorkID & "' order by codven ", "Cajeros", txtCajeroDesde.Text)
    End Sub

    Private Sub btnCajeroHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCajeroHasta.Click
        txtCajeroHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, apellidos descripcion from jsvencatven where tipo = 1 and id_emp = '" & jytsistema.WorkID & "' order by codven ", "Cajeros", txtCajeroHasta.Text)
    End Sub

    Private Sub txtCajeroDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCajeroDesde.TextChanged
        txtCajeroHasta.Text = txtCajeroDesde.Text
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