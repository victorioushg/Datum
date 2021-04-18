Imports MySql.Data.MySqlClient
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports ReportesDeNomina
Public Class jsNomRepParametros

    Private Const sModulo As String = "Reportes de Recursos Humanos"

    Private ReporteNumero As Integer
    Private ReporteNombre As String
    Private Trabajador As String, Documento As String
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

    Private vAgrupadoPor() As String = {"Ninguno", "Tipo Nómina"}
    Private aAgrupadoPor() As String = {"", "TIPONOMDES"}
    Private TipoNomina As Integer
    Private aCondicion() As String = {"Inactivo", "Activo", "Desincorporado", "Todos"}
    Private aaTipoNomina() As String = InsertArrayItemString(aTipoNomina, aTipoNomina.Length, "TODAS")
    Private periodoTipo As TipoPeriodo
    Private Nomina As String

    Public Sub Cargar(ByVal TipoCarga As Integer, ByVal numReporte As Integer, ByVal nomReporte As String, _
                      Optional ByVal CodTrabajador As String = "", Optional ByVal numDocumento As String = "", _
                      Optional ByVal Fecha As Date = #1/1/2009#, Optional CodigoNomina As String = "")



        Me.Dock = DockStyle.Fill
        myConn.Open()

        ReporteNumero = numReporte
        ReporteNombre = nomReporte
        Trabajador = CodTrabajador
        Documento = numDocumento
        FechaParametro = Fecha
        Nomina = CodigoNomina
        TipoNomina = ft.DevuelveScalarEntero(myConn, " select tiponom from jsnomencnom where codnom = '" & Nomina & "' and id_emp = '" & jytsistema.WorkID & "' ")

        PresentarReporte(numReporte, nomReporte, CodTrabajador)


        If TipoCarga = TipoCargaFormulario.iShow Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If

    End Sub
    Private Sub PresentarReporte(ByVal NumeroReporte As Integer, ByVal NombreDelReporte As String, Optional ByVal Trabajador As String = "")
        lblNombreReporte.Text += " - " + NombreDelReporte
        Select Case NumeroReporte
            Case ReporteNomina.cFichaTrabajador, ReporteNomina.cRegistroTrabajador
                Dim vOrdenNombres() As String = {"Código Trabajador"}
                Dim vOrdenCampos() As String = {"codtra"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Trabajador)
            Case ReporteNomina.cTrabajadores
                Dim vOrdenNombres() As String = {"Código Trabajador", "Nombre Trabajador"}
                Dim vOrdenCampos() As String = {"codtra", "apellidos"}
                Dim vOrdenTipo() As String = {"S", "S"}
                Dim vOrdenLongitud() As Integer = {15, 50}
                Inicializar(ReporteNombre, True, False, False, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Trabajador)
            Case ReporteNomina.cConstantes
                Dim vOrdenNombres() As String = {"Contante"}
                Dim vOrdenCampos() As String = {"contante"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud)
            Case ReporteNomina.cConceptos
                Dim vOrdenNombres() As String = {"Concepto"}
                Dim vOrdenCampos() As String = {"codcon"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud)
            Case ReporteNomina.cConceptosXTrabajador, ReporteNomina.cResumenXConcepto
                Dim vOrdenNombres() As String = {"Concepto"}
                Dim vOrdenCampos() As String = {"codcon"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud)
            Case ReporteNomina.cConceptosXTrabajadores, ReporteNomina.cConceptosXTrabajadorMesAMes, ReporteNomina.cConceptosMesAMes
                Dim vOrdenNombres() As String = {"Concepto"}
                Dim vOrdenCampos() As String = {"codcon"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, False, False, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud)
            Case ReporteNomina.cPrenomina, ReporteNomina.cNomina, ReporteNomina.cRecibos, ReporteNomina.cFirmas
                Dim vOrdenNombres() As String = {"Código Trabajador", "Nombre Trabajador"}
                Dim vOrdenCampos() As String = {"codtra", "apellidos"}
                Dim vOrdenTipo() As String = {"S", "S"}
                Dim vOrdenLongitud() As Integer = {15, 50}
                Inicializar(ReporteNombre, True, False, False, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud)
            Case ReporteNomina.cAcumuladosPorConcepto
                Dim vOrdenNombres() As String = {"Concepto"}
                Dim vOrdenCampos() As String = {"codcon"}
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
        'ft.RellenaCombo(vAgrupadoPor, cmbAgrupadorPor)
    End Sub

    Private Sub IniciarCriterios()

        VerCriterio_Periodo(False, 0)
        Select Case ReporteNumero
            Case ReporteNomina.cAcumuladosPorConcepto
                VerCriterio_Periodo(True, 2, TipoPeriodo.iDiario)
        End Select

    End Sub

    Private Sub VerCriterio_Periodo(ByVal Ver As Boolean, ByVal CompletoDesdeHasta As Integer, Optional ByVal Periodo As TipoPeriodo = TipoPeriodo.iMensual)
        'CompletoDesdeHasta 0 = Complete , 1 = Desde , 2 = Hasta 
        ft.visualizarObjetos(False, lblPeriodoDesde, lblPeriodoHasta, lblPeriodo, txtPeriodoDesde, txtPeriodoHasta, btnPeriodoDesde, btnPeriodoHasta)
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
    Private Sub IniciarConstantes()

        verConstante_Resumen(False)
        verConstante_TipoNomina(False)
        verConstante_TipoNominaPlus(False)
        verConstante_Nomina(False)
        verConstante_Estatus(False)

        Select Case ReporteNumero
            Case ReporteNomina.cConceptos
                verConstante_Nomina(True)
                txtCodNom.Text = Nomina
            Case ReporteNomina.cTrabajadores
                verConstante_TipoNominaPlus(True)
                verConstante_Estatus(True)
            Case ReporteNomina.cAcumuladosPorConcepto, ReporteNomina.cConceptosXTrabajador, ReporteNomina.cConceptosXTrabajadores, _
                ReporteNomina.cPrenomina, ReporteNomina.cConceptosXTrabajadorMesAMes, ReporteNomina.cConceptosMesAMes
                verConstante_TipoNomina(True)
            Case ReporteNomina.cNomina, ReporteNomina.cResumenXConcepto, ReporteNomina.cRecibos, ReporteNomina.cFirmas
                verConstante_TipoNominaPlus(True)
                verConstante_Nomina(True)
        End Select

    End Sub
    Private Sub verConstante_TipoNominaPlus(ByVal Ver As Boolean)
        ft.habilitarObjetos(False, True, txtCodNom, cmbTipoNomina)
        ft.visualizarObjetos(Ver, lblNominaConstante, txtCodNom, btnNomina, lblNomina, lblconTipoNomina, cmbTipoNomina)
        txtCodNom.Text = Documento
    End Sub
    Private Sub verConstante_Resumen(ByVal Ver As Boolean, Optional ByVal Valor As Boolean = False)
        ft.visualizarObjetos(Ver, lblConsResumen, chkConsResumen)
        chkConsResumen.Checked = Valor
    End Sub
    Private Sub verConstante_TipoNomina(ByVal Ver As Boolean)
        ft.habilitarObjetos(False, True, txtCodNom, cmbTipoNomina)
        ft.visualizarObjetos(Ver, lblNominaConstante, txtCodNom, btnNomina, lblNomina, lblconTipoNomina, cmbTipoNomina)
    End Sub
    Private Sub verConstante_Estatus(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblCondicion, cmbCondicion)
        ft.RellenaCombo(aCondicion, cmbCondicion)
    End Sub
    Private Sub verConstante_Nomina(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblPeriodoNomina, cmbNomina)
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
                txtOrdenDesde.Text = Trabajador
                txtOrdenHasta.Text = Trabajador
        End Select

    End Sub

    Private Sub jsNomRepParametros_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsNomRepParametros_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(myConn, MovAud.ientrar, sModulo, ReporteNumero)
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub
    Private Function Validado() As Boolean
        Select Case ReporteNumero
            Case ReporteNomina.cNomina, ReporteNomina.cResumenXConcepto, ReporteNomina.cRecibos, _
                ReporteNomina.cConceptosXTrabajadores, ReporteNomina.cTrabajadores, ReporteNomina.cPrenomina
                If txtCodNom.Text = "" Then
                    ft.mensajeAdvertencia("Debe seleccionar una nómina válida...")
                    Return False
                End If
        End Select

        If cmbNomina.Text.Trim = "" Then
            ft.mensajeCritico("DEBE INDICAR UN PERÍODO DE NOMINA...")
            Return False
        End If

        Return True
    End Function
    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

        If Not Validado() Then Exit Sub
        HabilitarCursorEnEspera()

        EsperaPorFavor()

        Dim r As New frmViewer
        Dim dsNom As New dsNomina
        Dim str As String = ""
        Dim nTabla As String = ""
        Dim oReporte As New CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim PresentaArbol As Boolean = False

        Select Case ReporteNumero
            Case ReporteNomina.cFichaTrabajador
                nTabla = "dtCatalogoTrabajadores"
                oReporte = New rptNominaFichaTrabajador
                str = SeleccionNOMTrabajadores(Trabajador, Trabajador, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), txtCodNom.Text, 9)
            Case ReporteNomina.cRegistroTrabajador
                nTabla = "dtCatalogoTrabajadores"
                oReporte = New rptNominaRegistroTrabajador
                str = SeleccionNOMTrabajadores(Trabajador, Trabajador, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), txtCodNom.Text, 9)
            Case ReporteNomina.cTrabajadores
                nTabla = "dtCatalogoTrabajadores"
                If cmbAgrupadorPor.SelectedIndex = 0 Then
                    oReporte = New rptNominaTrabajadores
                Else
                    oReporte = New rptNominaTrabajadores1G
                End If
                str = SeleccionNOMTrabajadores(txtOrdenDesde.Text, txtOrdenHasta.Text, vOrdenCampos(cmbOrdenadoPor.SelectedIndex), txtCodNom.Text, cmbCondicion.SelectedIndex)
            Case ReporteNomina.cConstantes
                nTabla = "dtConstantes"
                oReporte = New rptNominaConstantes
                str = SeleccionNOMConstantes()
            Case ReporteNomina.cConceptos
                nTabla = "dtConceptos"
                oReporte = New rptNominaConceptos
                str = SeleccionNOMConceptos(txtCodNom.Text)
            Case ReporteNomina.cConceptosXTrabajador, ReporteNomina.cConceptosXTrabajadores
                nTabla = "dtConceptosXTrabajador"
                oReporte = New rptNominaConceptosXTrabajador
                str = SeleccionNOMConceptosXTrabajador(txtOrdenDesde.Text, txtOrdenHasta.Text, txtCodNom.Text)
            Case ReporteNomina.cPrenomina
                nTabla = "dtPrenomina"
                oReporte = New rptNominaPreNomina
                str = SeleccionNOMPrenomina(vOrdenCampos(cmbOrdenadoPor.SelectedIndex), txtCodNom.Text, txtOrdenDesde.Text, txtOrdenHasta.Text)
            Case ReporteNomina.cNomina
                nTabla = "dtPrenomina"
                oReporte = New rptNominaNomina
                Dim aFec() As String = Split(cmbNomina.Text, " / ")
                str = SeleccionNOMNomina(vOrdenCampos(cmbOrdenadoPor.SelectedIndex), txtCodNom.Text, _
                      CDate(aFec(0)), CDate(aFec(1)), txtOrdenDesde.Text, txtOrdenHasta.Text)
            Case ReporteNomina.cRecibos
                nTabla = "dtPrenomina"
                oReporte = New rptNominaRecibos
                Dim aFec() As String = Split(cmbNomina.Text, " / ")
                str = SeleccionNOMNomina(vOrdenCampos(cmbOrdenadoPor.SelectedIndex), txtCodNom.Text, _
                      CDate(aFec(0)), CDate(aFec(1)), txtOrdenDesde.Text, txtOrdenHasta.Text)
            Case ReporteNomina.cFirmas
                nTabla = "dtPrenomina"
                oReporte = New rptNominaFirmas
                Dim aFec() As String = Split(cmbNomina.Text, " / ")
                str = SeleccionNOMNomina(vOrdenCampos(cmbOrdenadoPor.SelectedIndex), txtCodNom.Text, _
                      CDate(aFec(0)), CDate(aFec(1)), txtOrdenDesde.Text, txtOrdenHasta.Text)
            Case ReporteNomina.cResumenXConcepto
                nTabla = "dtPrenomina"
                oReporte = New rptNominaNominaResumen
                Dim aFec() As String = Split(cmbNomina.Text, " / ")
                str = SeleccionNOMResumenXConcepto(txtCodNom.Text, _
                      CDate(aFec(0)), CDate(aFec(1)))
            Case ReporteNomina.cAcumuladosPorConcepto
                nTabla = "dtConceptosAcumulados"
                oReporte = New rptNominaConceptoAcumulado
                str = SeleccionNOMAcumuladosXConcepto(txtCodNom.Text, _
                       CDate(txtPeriodoHasta.Text))
            Case ReporteNomina.cConceptosXTrabajadorMesAMes
                nTabla = "dtConceptosMesAMes"
                oReporte = New rptNominaConceptosXTrabajadorMesaMes
                str = SeleccionNOMConceptosXTrabajadorMesAMes(txtCodNom.Text, txtOrdenDesde.Text, txtOrdenHasta.Text)
            Case ReporteNomina.cConceptosMesAMes
                nTabla = "dtConceptosMesAMes"
                oReporte = New rptNominaConceptosMesaMes
                str = SeleccionNOMConceptosMesAMes(txtCodNom.Text, txtOrdenDesde.Text, txtOrdenHasta.Text)
            Case Else
                oReporte = Nothing
        End Select

        If nTabla <> "" Then
            dsNom = DataSetRequery(dsNom, str, myConn, nTabla, lblInfo)
            If dsNom.Tables(nTabla).Rows.Count > 0 Then
                oReporte = PresentaReporte(oReporte, dsNom, nTabla)
                r.CrystalReportViewer1.ReportSource = oReporte
                r.CrystalReportViewer1.ToolPanelView = IIf(PresentaArbol, CrystalDecisions.Windows.Forms.ToolPanelViewType.GroupTree, _
                                              CrystalDecisions.Windows.Forms.ToolPanelViewType.None)
                r.CrystalReportViewer1.ShowGroupTreeButton = PresentaArbol
                r.CrystalReportViewer1.Zoom(1)
                r.CrystalReportViewer1.Refresh()
                r.Cargar(ReporteNombre)
                DeshabilitarCursorEnEspera()
            Else
                If ReporteNumero = ReporteNomina.cRegistroTrabajador Then
                    oReporte = PresentaReporte(oReporte, dsNom, nTabla)
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
        oReporte.SetParameterValue("RIF", "")
        oReporte.SetParameterValue("Grupo", "")
        oReporte.SetParameterValue("Criterios", LineaCriterios)
        oReporte.SetParameterValue("Constantes", LineaConstantes)
        oReporte.SetParameterValue("Empresa", jytsistema.WorkName.TrimEnd(" ") + "      R.I.F. : " & rif)

        Select Case ReporteNumero
            Case ReporteNomina.cTrabajadores
                Select Case cmbAgrupadorPor.SelectedIndex
                    Case 1
                        oReporte.SetParameterValue("Grupo1", "tiponomdes")
                        Dim FieldDef As FieldDefinition
                        FieldDef = oReporte.Database.Tables.Item(0).Fields.Item(aAgrupadoPor(cmbAgrupadorPor.SelectedIndex))
                        oReporte.DataDefinition.Groups.Item(0).ConditionField = FieldDef
                    Case Else
                End Select

        End Select

        PresentaReporte = oReporte

    End Function
    Private Sub UbicacionDatosCheque()

    End Sub
    Private Sub btnPeriodoDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btnPeriodoDesde.Click
        txtPeriodoDesde.Text = SeleccionaFecha(CDate(txtPeriodoDesde.Text), Me, sender)
    End Sub

    Private Sub btnPeriodoHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btnPeriodoHasta.Click
        txtPeriodoHasta.Text = SeleccionaFecha(CDate(txtPeriodoHasta.Text), Me, sender)
    End Sub

    Private Sub btnLimpiar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLimpiar.Click

    End Sub

    Private Function LineaCriterios() As String
        LineaCriterios = ""
        If lblPeriodo.Visible Then LineaCriterios += "Período: " & IIf(lblPeriodoDesde.Visible, txtPeriodoDesde.Text, "") & IIf(lblPeriodoDesde.Visible AndAlso lblPeriodoHasta.Visible, "/", "") & IIf(lblPeriodoHasta.Visible, txtPeriodoHasta.Text, "")
    End Function
    Private Function LineaConstantes() As String
        LineaConstantes = ""
        If lblConsResumen.Visible Then LineaConstantes += "Resumido : " + IIf(chkConsResumen.Checked, "Si", "No")
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblconTipoNomina.Visible Then LineaConstantes += "Tipo nómina : " + aaTipoNomina(cmbTipoNomina.SelectedIndex)
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblPeriodoNomina.Visible Then LineaConstantes += " Nómina : " + cmbNomina.Text

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
            Case "Código Trabajador"
            Case "Concepto"
                ds = DataSetRequery(ds, "select codcon codigo, nomcon descripcion from jsnomcatcon where id_emp = '" & jytsistema.WorkID & "' order by codcon ", myConn, nTAbleOrden, lblInfo)
                dtDeOrden = ds.Tables(nTAbleOrden)
                f.Cargar("Conceptos de Nómina", ds, dtDeOrden, nTAbleOrden, TipoCargaFormulario.iShowDialog, False)
                txt.Text = f.Seleccion
        End Select
        f.Close()
        f = Nothing
        dtDeOrden.Dispose()
        dtDeOrden = Nothing

    End Sub

    Private Sub txtOrdenDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOrdenDesde.TextChanged
        txtOrdenHasta.Text = txtOrdenDesde.Text
    End Sub


    Private Sub cmbAgrupadorPor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAgrupadorPor.SelectedIndexChanged
        Dim aOBJ() As Object = {lblG1D, lblG1H, cmbTNDesde, cmbTNHasta}
        'Select Case cmbAgrupadorPor.SelectedIndex
        '    Case 0
        'cmbTNDesde.Items.Clear()
        'cmbTNHasta.Items.Clear()
        'ft.visualizarObjetos(aOBJ, False)
        '    Case 1
        'ft.RellenaCombo(aTipoNomina, cmbTNDesde)
        'ft.RellenaCombo(aTipoNomina, cmbTNHasta)
        'ft.visualizarObjetos(aOBJ, True)

        'End Select
    End Sub

    Private Sub cmbTipoNomina_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipoNomina.SelectedIndexChanged
        Dim dtNom As DataTable
        Dim tblNom As String = "tblNomina"
        ds = DataSetRequery(ds, " select concat( date_format(desde, '%d-%m-%Y'), ' / ', date_format(hasta, '%d-%m-%Y') ) nomina " _
                            & " from jsnomfecnom where " _
                            & " codnom = '" & txtCodNom.Text & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by desde desc ", myConn, tblNom, lblInfo)
        dtNom = ds.Tables(tblNom)
        cmbNomina.Items.Clear()
        If dtNom.Rows.Count > 0 Then
            Dim aNom(0 To dtNom.Rows.Count - 1) As String
            Dim ifCont As Integer
            For ifCont = 0 To dtNom.Rows.Count - 1
                aNom(ifCont) = dtNom.Rows(ifCont).Item("nomina").ToString
            Next
            ft.RellenaCombo(aNom, cmbNomina)
        End If
        dtNom.Dispose()
        dtNom = Nothing
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

    Private Sub btnNomina_Click(sender As System.Object, e As System.EventArgs) Handles btnNomina.Click
        txtCodNom.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codnom codigo, descripcion from jsnomencnom where id_emp = '" & jytsistema.WorkID & "' order by codnom ", "NOMINAS", txtCodNom.Text)
    End Sub

    Private Sub txtCodNom_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCodNom.TextChanged
        lblNomina.Text = ft.DevuelveScalarCadena(myConn, " SELECT DESCRIPCION FROM jsnomencnom where CODNOM = '" & txtCodNom.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ").ToString
        ft.RellenaCombo(aTipoNomina, cmbTipoNomina, ft.DevuelveScalarEntero(myConn, " SELECT TIPONOM FROM jsnomencnom WHERE CODNOM = '" & txtCodNom.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "'  "))
    End Sub
End Class