Imports MySql.Data.MySqlClient
Imports System.IO
Imports ReportesDeContabilidad
Imports Syncfusion.WinForms.Input

Public Class jsContabRepParametros
    Private Const sModulo As String = "Reportes de Contabilidad"

    Private ReporteNumero As Integer
    Private ReporteNombre As String
    Private Codigo As String, Documento As String

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet
    Private ft As New Transportables

    Private vOrdenNombres() As String
    Private vOrdenCampos() As String
    Private vOrdenTipo() As String
    Private vOrdenLongitud() As Integer
    Private IndiceReporte As Integer
    Private strIndiceReporte As String
    Private periodoTipo As TipoPeriodo
    Public Sub Cargar(ByVal TipoCarga As Integer, ByVal numReporte As Integer, ByVal nomReporte As String,
                      Optional ByVal Code As String = "", Optional ByVal numDocumento As String = "")

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo
        myConn.Open()

        Dim dates As SfDateTimeEdit() = {txtPeriodoDesde, txtPeriodoHasta}
        SetSizeDateObjects(dates)

        ReporteNumero = numReporte
        ReporteNombre = nomReporte
        Codigo = Code
        Documento = numDocumento

        PresentarReporte(numReporte, nomReporte, Code)

        If TipoCarga = TipoCargaFormulario.iShow Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If

    End Sub
    Private Sub PresentarReporte(ByVal NumeroReporte As Integer, ByVal NombreDelReporte As String, Optional ByVal Codigo As String = "")
        lblNombreReporte.Text += " - " + NombreDelReporte
        Select Case NumeroReporte
            Case ReporteContabilidad.cCuentasContables
                Dim vOrdenNombres() As String = {"Código Cuenta"}
                Dim vOrdenCampos() As String = {"codcon"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {25}
                Inicializar(ReporteNombre, True, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Codigo)

            Case ReporteContabilidad.cPolizaDiferida, ReporteContabilidad.cPolizaActual, ReporteContabilidad.cPLantillaAsiento
                Dim vOrdenNombres() As String = {"Asiento Contable"}
                Dim vOrdenCampos() As String = {"asiento"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Codigo)
            Case ReporteContabilidad.cPolizasActuales
                Dim vOrdenNombres() As String = {"Asiento Contable"}
                Dim vOrdenCampos() As String = {"asiento"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, True, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Codigo)
            Case ReporteContabilidad.cMayorAnalitico, ReporteContabilidad.cBalanceComprobacion
                Dim vOrdenNombres() As String = {"Código Cuenta"}
                Dim vOrdenCampos() As String = {"codcon"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {35}
                Inicializar(ReporteNombre, True, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Codigo)
            Case ReporteContabilidad.cDiario
                Dim vOrdenNombres() As String = {"Código Cuenta"}
                Dim vOrdenCampos() As String = {"codcon"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {35}
                Inicializar(ReporteNombre, True, False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Codigo)
            Case ReporteContabilidad.cBalanceGeneral, ReporteContabilidad.cGananciasPerdidas
                Dim vOrdenNombres() As String = {"Código Cuenta"}
                Dim vOrdenCampos() As String = {"codcon"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {35}
                Inicializar(ReporteNombre, False, False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Codigo)
            Case ReporteContabilidad.cReglaContabilizacion
                Dim vOrdenNombres() As String = {"Código Reglas"}
                Dim vOrdenCampos() As String = {"plantilla"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {35}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Codigo)
            Case ReporteContabilidad.cReglasContabilizacion
                Dim vOrdenNombres() As String = {"Código Reglas"}
                Dim vOrdenCampos() As String = {"plantilla"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {35}
                Inicializar(ReporteNombre, True, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Codigo)

        End Select
    End Sub
    Private Sub Inicializar(ByVal nEtiqueta As String, ByVal TabOrden As Boolean, ByVal TabGrupo As Boolean,
        ByVal TabCriterio As Boolean, ByVal TabConstantes As Boolean, ByVal aNombreOrden() As String,
        ByVal aCampoOrden() As String, ByVal aTipoOrden() As String, ByVal aLongitudOrden() As Integer,
        Optional ByVal Codigo As String = "")


        HabilitarTabs(TabOrden, TabGrupo, TabCriterio, TabConstantes)
        txtOrdenDesde.Text = Codigo
        txtOrdenHasta.Text = Codigo
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

    End Sub
    Private Sub IniciarCriterios()
        VerCriterio_Periodo(False, 0)
        VerCriterio_TipoDocumento(False)
        VerCriterio_MesAño(False)
        Select Case ReporteNumero
            Case ReporteContabilidad.cCuentasContables, ReporteContabilidad.cPolizaDiferida,
                ReporteContabilidad.cPolizaActual
            Case ReporteContabilidad.cPolizasActuales, ReporteContabilidad.cMayorAnalitico,
                ReporteContabilidad.cBalanceComprobacion, ReporteContabilidad.cDiario
                VerCriterio_Periodo(True, 0)
            Case ReporteContabilidad.cBalanceGeneral, ReporteContabilidad.cGananciasPerdidas
                VerCriterio_Periodo(True, 0, TipoPeriodo.iAnual)
        End Select

    End Sub
    Private Sub VerCriterio_Periodo(ByVal Ver As Boolean, ByVal CompletoDesdeHasta As Integer, Optional ByVal Periodo As TipoPeriodo = TipoPeriodo.iMensual)
        'CompletoDesdeHasta 0 = Complete , 1 = Desde , 2 = Hasta 

        ft.visualizarObjetos(False, lblPeriodo, txtPeriodoDesde, txtPeriodoHasta)
        periodoTipo = Periodo
        If Ver Then

            Select Case CompletoDesdeHasta
                Case 0
                    ft.visualizarObjetos(Ver, lblPeriodo, txtPeriodoDesde, txtPeriodoHasta)
                Case 1
                    ft.visualizarObjetos(Ver, lblPeriodo, txtPeriodoDesde)
                Case 2
                    ft.visualizarObjetos(Ver, lblPeriodo, txtPeriodoHasta)
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
        'DocumentosTipo : 0 = Bancos, 1 = caja

        ft.visualizarObjetos(ver, lblTipodocumento, chkList)
        ft.habilitarObjetos(ver, True, lblTipodocumento, chkList)
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
        End Select


    End Sub

    Private Sub VerCriterio_MesAño(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblMesAño, lblMes, lblAño, cmbMes, cmbAño)
        IniciarCriterioMesAño()
    End Sub

    Private Sub IniciarConstantes()
        VerConstante_TipoCuenta(False)
        VerConstante_NivelCuenta(False)
        Select Case ReporteNumero
            Case ReporteContabilidad.cDiario
                VerConstante_TipoCuenta(True)
            Case ReporteContabilidad.cBalanceGeneral, ReporteContabilidad.cGananciasPerdidas
                VerConstante_NivelCuenta(True)
        End Select
    End Sub
    Private Sub VerConstante_TipoCuenta(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblConstante1, cmbConstante_TipoCuenta)
        Select Case ReporteNumero
            Case ReporteContabilidad.cDiario
                Dim aTipoCuenta() As String = {"Todas", "Resultado"}
                ft.RellenaCombo(aTipoCuenta, cmbConstante_TipoCuenta)
        End Select
    End Sub
    Private Sub VerConstante_NivelCuenta(ByVal Ver As Boolean)
        ft.visualizarObjetos(Ver, lblNivel, cmbNivel)
        Dim aNivel() As String = {"Nivel 1", "Nivel 2", "Nivel 3", "Nivel 4", "Nivel 5", "Nivel 6", "Nivel 7"}
        ft.RellenaCombo(aNivel, cmbNivel, 5)
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
                txtOrdenDesde.Text = Codigo
                txtOrdenHasta.Text = Codigo
        End Select

    End Sub
    Private Sub IniciarCriterioMesAño()

        Dim aMeses() As String = {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"}
        ft.RellenaCombo(aMeses, cmbMes, Month(jytsistema.sFechadeTrabajo) - 1)
        Dim aAño() As String = {"2000", "2001", "2002", "2003", "2004", "2005", "2006", "2007", "2008", "2009", "2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019", "2020", "2021", "2022", "2023", "2024", "2025"}
        ft.RellenaCombo(aAño, cmbAño, Year(jytsistema.sFechadeTrabajo) - 2000)

    End Sub

    Private Sub jsContabRepParametros_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsContabRepParametros_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(myConn, MovAud.ientrar, sModulo, ReporteNumero)
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

        HabilitarCursorEnEspera()

        EsperaPorFavor()

        Dim r As New frmViewer
        Dim dsCon As New dsContab
        Dim str As String = ""
        Dim nTabla As String = ""

        Dim oReporte As New CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim PresentaArbol As Boolean = False
        Select Case ReporteNumero
            Case ReporteContabilidad.cCuentasContables
                nTabla = "dtCuentas"
                oReporte = New rptContabCuentas
                str = SeleccionCOTCuentasContables(txtOrdenDesde.Text, txtOrdenHasta.Text)
            Case ReporteContabilidad.cPolizaDiferida, ReporteContabilidad.cPolizaActual
                nTabla = "dtAsiento"
                oReporte = New rptContabAsiento
                str = SeleccionCOTPolizasContables(IIf(ReporteNumero = ReporteContabilidad.cPolizaDiferida, Asiento.iDiferido, Asiento.iActual), Codigo, Codigo)
            Case ReporteContabilidad.cPLantillaAsiento
                nTabla = "dtAsiento"
                oReporte = New rptContabPlantillaAsiento
                str = SeleccionCOTPlantillaAsiento(txtOrdenDesde.Text, txtOrdenHasta.Text)
            Case ReporteContabilidad.cPolizaActual
                nTabla = "dtAsiento"
                oReporte = New rptContabAsiento
                str = SeleccionCOTPolizasContables(Asiento.iActual, Codigo, Codigo)
            Case ReporteContabilidad.cPolizasActuales
                nTabla = "dtAsiento"
                oReporte = New rptContabAsiento
                str = SeleccionCOTPolizasContables(Asiento.iActual, txtOrdenDesde.Text, txtOrdenHasta.Text, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text))
            Case ReporteContabilidad.cMayorAnalitico
                nTabla = "dtMayorAnalitico"
                oReporte = New rptContabMayorAnalitico
                str = SeleccionCOTMayorAnalitico(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), txtOrdenDesde.Text, txtOrdenHasta.Text)
                PresentaArbol = True
            Case ReporteContabilidad.cBalanceComprobacion
                nTabla = "dtBalanceComprobacion"
                oReporte = New rptContabBalanceComprobacion
                str = SeleccionCOTBalanceDeComprobacionPlus(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), txtOrdenDesde.Text, txtOrdenHasta.Text, True, True)
            Case ReporteContabilidad.cDiario
                nTabla = "dtBalanceComprobacion"
                oReporte = New rptContabDiario
                str = SeleccionCOTBalanceDeComprobacionPlus(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), txtOrdenDesde.Text, txtOrdenHasta.Text, False, True, IIf(cmbConstante_TipoCuenta.SelectedIndex = 0, False, True))

            Case ReporteContabilidad.cBalanceGeneral
                Dim CuentaResultado As String = ParametroPlus(myConn, Gestion.iContabilidad, "CONPARAM03")
                If CuentaResultado <> "" Then
                    nTabla = "dtBalanceGeneral"
                    oReporte = New rptContabBalanceGeneral
                    str = SeleccionCOTBalanceGeneral(myConn, lblInfo, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text),
                                                     cmbNivel.SelectedIndex + 1, True, jytsistema.WorkExercise)
                Else
                    ft.mensajeAdvertencia("Debe indicar una cuenta resultado en los parámetros del sistema...  ")
                    nTabla = ""
                End If
            Case ReporteContabilidad.cGananciasPerdidas
                nTabla = "dtBalanceGeneral"
                oReporte = New rptContabGananciasPerdidas
                str = SeleccionCOTBalanceGeneral(myConn, lblInfo, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text),
                                                 cmbNivel.SelectedIndex + 1, False, jytsistema.WorkExercise)
            Case ReporteContabilidad.cReglaContabilizacion, ReporteContabilidad.cReglasContabilizacion
                nTabla = "dtReglas"
                oReporte = New rptContabReglas
                str = SeleccionCOTReglas(txtOrdenDesde.Text, txtOrdenHasta.Text)
            Case Else
                oReporte = Nothing
        End Select

        If nTabla <> "" Then
            dsCon = DataSetRequery(dsCon, str, myConn, nTabla, lblInfo)
            If dsCon.Tables(nTabla).Rows.Count > 0 Then
                oReporte = PresentaReporte(oReporte, dsCon, nTabla)
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
        oReporte.SetParameterValue("Constantes", "")
        oReporte.SetParameterValue("Empresa", jytsistema.WorkName.TrimEnd(" ") + "      R.I.F. : " & rif)
        Select Case ReporteNumero
            Case ReporteContabilidad.cMayorAnalitico
                oReporte.SetParameterValue("SaldoAl", " Saldo al " + ft.FormatoFecha(DateAdd(DateInterval.Day, -1, CDate(txtPeriodoDesde.Text))))
            Case ReporteContabilidad.cCuentasContables
                oReporte.SetParameterValue("SaldoAl", "")
        End Select

        PresentaReporte = oReporte

    End Function

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
    Private Function LineaCriterios() As String
        LineaCriterios = ""
        If lblPeriodo.Visible Then LineaCriterios += "Período: " & IIf(txtPeriodoDesde.Visible, txtPeriodoDesde.Text, "") &
            IIf(txtPeriodoDesde.Visible AndAlso txtPeriodoHasta.Visible, "/", "") & IIf(txtPeriodoHasta.Visible, txtPeriodoHasta.Text, "")
        If lblTipodocumento.Visible Then LineaCriterios += " - " + " Tipo Documentos : " + txtTipDoc.Text
        If lblMesAño.Visible Then LineaCriterios += " Mes y Año " + cmbMes.Text + "/" + cmbAño.Text
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
            Case "Código Cuenta"
                txt.Text = CargarTablaSimple(myConn, lblInfo, ds, "select codcon codigo, descripcion from jscotcatcon where id_emp = '" & jytsistema.WorkID & "' order by codcon ",
                                             "Cuentas Contables", txt.Text)
            Case "Asiento Contable"
                txt.Text = CargarTablaSimple(myConn, lblInfo, ds, "select asiento codigo, descripcion from jscotencasi where actual = 1 and id_emp = '" & jytsistema.WorkID & "' order by asiento ",
                                             "Asientos Contables", txt.Text)
        End Select
        f.Close()
        f = Nothing
        dtDeOrden.Dispose()
        dtDeOrden = Nothing

    End Sub
    Private Sub txtOrdenDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOrdenDesde.TextChanged
        txtOrdenHasta.Text = txtOrdenDesde.Text
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
End Class