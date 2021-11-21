Imports MySql.Data.MySqlClient
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Public Class jsPOSRepParametros

    Private Const sModulo As String = "Reportes de Punto de Venta"

    Private ReporteNumero As Integer
    Private ReporteNombre As String
    Private NumeroCaja As String, Documento As String
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

    Private vAgrupadoPor() As String = {"Ninguno"}
    Private aAgrupadoPor() As String = {""}
    Public Sub Cargar(ByVal TipoCarga As Integer, ByVal numReporte As Integer, ByVal nomReporte As String, _
                      Optional ByVal CodigoCaja As String = "", Optional ByVal numDocumento As String = "", _
                      Optional ByVal Fecha As Date = #1/1/2009#)

        Me.Dock = DockStyle.Fill
        myConn.Open()

        ReporteNumero = numReporte
        ReporteNombre = nomReporte
        NumeroCaja = CodigoCaja
        Documento = numDocumento
        FechaParametro = Fecha

        PresentarReporte(numReporte, nomReporte, NumeroCaja)

        If TipoCarga = TipoCargaFormulario.iShow Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If

    End Sub
    Private Sub PresentarReporte(ByVal NumeroReporte As Integer, ByVal NombreDelReporte As String, Optional ByVal NumeroCaja As String = "")
        lblNombreReporte.Text += " - " + NombreDelReporte
        Select Case NumeroReporte
            Case ReportePuntoDeVenta.cReporteX, ReportePuntoDeVenta.cReporteZ
                Dim vOrdenNombres() As String = {"Caja"}
                Dim vOrdenCampos() As String = {"caja"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, NumeroCaja)
        End Select
    End Sub
    Private Sub Inicializar(ByVal nEtiqueta As String, ByVal TabOrden As Boolean, ByVal TabGrupo As Boolean, _
        ByVal TabCriterio As Boolean, ByVal TabConstantes As Boolean, ByVal aNombreOrden() As String, _
        ByVal aCampoOrden() As String, ByVal aTipoOrden() As String, ByVal aLongitudOrden() As Integer, _
        Optional ByVal NumeroCaja As String = "")


        HabilitarTabs(TabOrden, TabGrupo, TabCriterio, TabConstantes)
        txtOrdenDesde.Text = NumeroCaja
        txtOrdenHasta.Text = NumeroCaja
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
        'RellenaCombo(vAgrupadoPor, cmbAgrupadorPor)
    End Sub

    Private Sub IniciarCriterios()

        VerCriterio_Periodo(False, 0)
        VerCriterio_TipoDocumento(False)
        VerCriterio_MesAño(False)
        VerCriterio_Documento(False)

        Select Case ReporteNumero
            Case ReportePuntoDeVenta.cReporteX, ReportePuntoDeVenta.cReporteZ
                VerCriterio_Periodo(True, 0, TipoPeriodo.iDiario)
        End Select

    End Sub

    Private Sub VerCriterio_Periodo(ByVal Ver As Boolean, ByVal CompletoDesdeHasta As Integer, Optional ByVal Periodo As TipoPeriodo = TipoPeriodo.iMensual)
        'CompletoDesdeHasta 0 = Complete , 1 = Desde , 2 = Hasta 
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


        Select Case Periodo
            Case TipoPeriodo.iDiario
                txtPeriodoDesde.Text = ft.muestracampofecha(jytsistema.sFechadeTrabajo)
                txtPeriodoHasta.Text = ft.muestracampofecha(jytsistema.sFechadeTrabajo)
            Case TipoPeriodo.iSemanal
                txtPeriodoDesde.Text = ft.muestracampofecha(PrimerDiaSemana(jytsistema.sFechadeTrabajo))
                txtPeriodoHasta.Text = ft.muestracampofecha(UltimoDiaSemana(jytsistema.sFechadeTrabajo))
            Case TipoPeriodo.iMensual
                txtPeriodoDesde.Text = ft.muestracampofecha(PrimerDiaMes(jytsistema.sFechadeTrabajo))
                txtPeriodoHasta.Text = ft.muestracampofecha(UltimoDiaMes(jytsistema.sFechadeTrabajo))
            Case TipoPeriodo.iAnual
                txtPeriodoDesde.Text = ft.muestracampofecha(PrimerDiaAño(jytsistema.sFechadeTrabajo))
                txtPeriodoHasta.Text = ft.muestracampofecha(UltimoDiaAño(jytsistema.sFechadeTrabajo))
            Case Else
                txtPeriodoDesde.Text = ft.muestracampofecha(jytsistema.sFechadeTrabajo)
                txtPeriodoHasta.Text = ft.muestracampofecha(jytsistema.sFechadeTrabajo)
        End Select

    End Sub
    Private Sub VerCriterio_TipoDocumento(ByVal ver As Boolean, Optional ByVal DocumentosTipo As Integer = 0)
        'DocumentosTipo : 0 = Bancos, 1 = caja, 2 = Forma de pago
        '   Dim aOBJ() As Object = {lblTipodocumento, chkList}
        '  ft.visualizarObjetos(aOBJ, ver)
        ' ft.habilitarObjetos(aOBJ, ver)
        'Select Case DocumentosTipo
        '    Case 0
        'Dim aTipoDocumento() As String = {"CH", "DP", "NC", "ND"}
        'Dim aSel() As Boolean = {True, True, True, True}
        'RellenaListaSeleccionable(chkList, aTipoDocumento, aSel)
        'txtTipDoc.Text = ".CH.DP.NC.ND"
        '    Case 1
        'Dim aTipoDocumento() As String = {"EN", "SA"}
        'Dim aSel() As Boolean = {True, True}
        'RellenaListaSeleccionable(chkList, aTipoDocumento, aSel)
        'txtTipDoc.Text = ".EN.SA"
        '    Case 2
        'Dim aTipoDocumento() As String = {"EF", "CH", "TA", "CT"}
        'Dim aSel() As Boolean = {True, True, True, True}
        'RellenaListaSeleccionable(chkList, aTipoDocumento, aSel)
        'txtTipDoc.Text = ".EF.CH.TA.CT"
        'End Select
    End Sub
    Private Sub VerCriterio_MesAño(ByVal ver As Boolean)
        'Dim aOBJ() As Object = {lblMesAño, lblMes, lblAño, cmbMes, cmbAño}
        'ft.visualizarObjetos(aOBJ, ver)
        'IniciarCriterioMesAño()
    End Sub
    Private Sub VerCriterio_Documento(ByVal ver As Boolean)
        '        Dim aOBJ() As Object = {lblDocHasta, lblDocDesde, lblDocumento, txtDocumentoDesde, txtDocumentoHasta}
        '        ft.visualizarObjetos(aOBJ, ver)
        '        txtDocumentoDesde.Enabled = True : txtDocumentoDesde.MaxLength = 15
        '        txtDocumentoHasta.Enabled = True : txtDocumentoHasta.MaxLength = 15
    End Sub

    Private Sub IniciarConstantes()
        verConstate_Resumen(False)
        verConstate_TipoNomina(False)

        Select Case ReporteNumero
            Case ReportePuntoDeVenta.cReporteX, ReportePuntoDeVenta.cReporteZ
                verConstate_Resumen(True)
        End Select

    End Sub
    Private Sub verConstate_Resumen(ByVal Ver As Boolean, Optional ByVal Valor As Boolean = False)
        ft.visualizarObjetos(Ver, lblConsResumen, chkConsResumen)
        chkConsResumen.Checked = Valor
    End Sub
    Private Sub verConstate_TipoNomina(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblconTipoNomina, cmbTipoNomina)
    End Sub
    Private Sub LongitudMaximaOrden(ByVal iLongitud As Integer)
        txtOrdenDesde.MaxLength = iLongitud
        txtOrdenHasta.MaxLength = iLongitud
    End Sub
    Private Sub TipoOrden(ByVal cTipo As String)
        Select Case vOrdenTipo(IndiceReporte)
            Case "D"
                txtOrdenDesde.Text = ft.muestracampofecha(PrimerDiaMes(jytsistema.sFechadeTrabajo))
                txtOrdenHasta.Text = ft.muestracampofecha(UltimoDiaMes(jytsistema.sFechadeTrabajo))
                txtOrdenDesde.Enabled = False
                txtOrdenHasta.Enabled = False
            Case Else
                txtOrdenDesde.Text = NumeroCaja
                txtOrdenHasta.Text = NumeroCaja
        End Select

    End Sub
    Private Sub IniciarCriterioMesAño()

        '        Dim aMeses() As String = {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"}
        '       RellenaCombo(aMeses, cmbMes, Month(jytsistema.sFechadeTrabajo) - 1)
        '      Dim aAño() As String = {"2000", "2001", "2002", "2003", "2004", "2005", "2006", "2007", "2008", "2009", "2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019", "2020", "2021", "2022", "2023", "2024", "2025"}
        '     RellenaCombo(aAño, cmbAño, Year(jytsistema.sFechadeTrabajo) - 2000)

    End Sub

    Private Sub jsPOSRepParametros_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(myConn, MovAud.ientrar, sModulo, ReporteNumero)
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

        HabilitarCursorEnEspera()

        Dim r As New frmViewer
        Dim dsPV As New dsPOS
        Dim str As String = ""
        Dim nTabla As String = ""
        Dim oReporte As New CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim PresentaArbol As Boolean = False

        Select Case ReporteNumero
            Case ReportePuntoDeVenta.cReporteX
                nTabla = "dtReporteX"
                oReporte = New rptReporteX
                str = SeleccionPuntodeVentaReporteX(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), NumeroCaja, jytsistema.sUsuario)
            Case ReportePuntoDeVenta.cReporteZ
                nTabla = "dtReporteX"
                oReporte = New rptReporteZ
                str = SeleccionPuntodeVentaReporteX(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), NumeroCaja, jytsistema.sUsuario)
            Case Else
                oReporte = Nothing
        End Select

        If nTabla <> "" Then
            dsPV = DataSetRequery(dsPV, str, myConn, nTabla, lblInfo)
            If dsPV.Tables(nTabla).Rows.Count > 0 Then
                oReporte = PresentaReporte(oReporte, dsPV, nTabla)
                r.CrystalReportViewer1.ReportSource = oReporte
                r.CrystalReportViewer1.ToolPanelView = IIf(PresentaArbol, CrystalDecisions.Windows.Forms.ToolPanelViewType.GroupTree, _
                                              CrystalDecisions.Windows.Forms.ToolPanelViewType.None)
                r.CrystalReportViewer1.ShowGroupTreeButton = PresentaArbol
                r.CrystalReportViewer1.Refresh()
                r.Cargar(ReporteNombre)
                DeshabilitarCursorEnEspera()
            Else
                ft.mensajeAdvertencia("No existe información que cumpla con estos criterios y/o constantes ")
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
            BaseDatosAImagen(dtEmpresa.Rows(0), "logo", .Item("id_emp").ToString)
            CaminoImagen = My.Computer.FileSystem.CurrentDirectory & "\" & "logo" & .Item("id_emp") & ".jpg"
        End With
        dtEmpresa.Dispose()
        dtEmpresa = Nothing

        oReporte.SetDataSource(ds)
        oReporte.Refresh()
        oReporte.SetParameterValue("strLogo", CaminoImagen)
        Select Case ReporteNumero
            Case ReportePuntoDeVenta.cReporteX, ReportePuntoDeVenta.cReporteZ
                oReporte.SetParameterValue("Orden", "CAJA Nº : " + txtOrdenDesde.Text)
                oReporte.SetParameterValue("Resumido", chkConsResumen.Checked)
            Case Else
                oReporte.SetParameterValue("Orden", "Ordenado por : " + cmbOrdenadoPor.Text + " " + txtOrdenDesde.Text + "/" + txtOrdenHasta.Text)
                oReporte.SetParameterValue("Resumido", chkConsResumen.Checked)
        End Select
        oReporte.SetParameterValue("RIF", rif)
        oReporte.SetParameterValue("Grupo", "")
        oReporte.SetParameterValue("Criterios", LineaCriterios)
        oReporte.SetParameterValue("Constantes", LineaConstantes)
        oReporte.SetParameterValue("Empresa", jytsistema.WorkName)


        PresentaReporte = oReporte

    End Function
    Private Sub UbicacionDatosCheque()

    End Sub
    Private Sub btnPeriodoDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btnPeriodoDesde.Click
        '   txtPeriodoDesde.Text = ft.muestracampofecha(SeleccionaFecha(CDate(txtPeriodoDesde.Text), Me, sender))
    End Sub

    Private Sub btnPeriodoHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles btnPeriodoHasta.Click
        ' txtPeriodoHasta.Text = ft.muestracampofecha(SeleccionaFecha(CDate(txtPeriodoHasta.Text), Me, sender))
    End Sub

    Private Sub btnLimpiar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLimpiar.Click

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
    Private Function LineaCriterios() As String
        LineaCriterios = ""
        If lblPeriodo.Visible Then LineaCriterios += "Período: " & IIf(lblPeriodoDesde.Visible, txtPeriodoDesde.Text, "") & IIf(lblPeriodoDesde.Visible AndAlso lblPeriodoHasta.Visible, "/", "") & IIf(lblPeriodoHasta.Visible, txtPeriodoHasta.Text, "")
        ' If lblTipodocumento.Visible Then LineaCriterios += " - " + " Tipo Documentos : " + txtTipDoc.Text
        ' If lblMesAño.Visible Then LineaCriterios += " Mes y Año " + cmbMes.Text + "/" + cmbAño.Text
        'If lblDocumento.Visible Then LineaCriterios += " - " + " Documento : " + txtDocumentoDesde.Text + "/" + txtDocumentoHasta.Text
    End Function
    Private Function LineaConstantes() As String
        LineaConstantes = ""
        If lblConsResumen.Visible Then LineaConstantes += "Resumido : " + IIf(chkConsResumen.Checked, "Si", "No")
        If LineaConstantes <> "" Then LineaConstantes += " - "
        If lblconTipoNomina.Visible Then LineaConstantes += "Tiponomina : " + aTipoNomina(cmbTipoNomina.SelectedIndex)
    End Function
    Private Sub btnOrdenDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrdenDesde.Click
        'txtDeOrden(txtOrdenDesde)
    End Sub

    Private Sub btnOrdenHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrdenHasta.Click
        'txtDeOrden(txtOrdenHasta)
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
                f.Cargar("Conceptos de Nómina", ds, dtDeOrden, nTAbleOrden, False)
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
        'RellenaCombo(aTipoNomina, cmbTNDesde)
        'RellenaCombo(aTipoNomina, cmbTNHasta)
        'ft.visualizarObjetos(aOBJ, True)

        'End Select
    End Sub
End Class