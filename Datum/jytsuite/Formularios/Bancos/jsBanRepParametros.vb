Imports MySql.Data.MySqlClient
Imports System.IO
Imports ReportesDeBancos
Imports fTransport
Public Class jsBanRepParametros

    Private Const sModulo As String = "Reportes de Bancos"

    Private ReporteNumero As Integer
    Private ReporteNombre As String
    Private CodigoBanco As String, Documento As String, Origen As String
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

    Private PeriodoTipo As TipoPeriodo

    Private aInNOTIn() As String = {"EN", "NO EN"}
    Private bOrigenBancos As Boolean = False
    Private cadOrigen As String = ""
    Private Enum DesdeHasta
        cDesdeHasta = 0
        cDesde = 1
        cHasta = 2
    End Enum

    Public Sub Cargar(ByVal TipoCarga As Integer, ByVal numReporte As Integer, ByVal nomReporte As String, _
                      Optional ByVal CodBanco As String = "", Optional ByVal numDocumento As String = "", _
                      Optional ByVal Fecha As Date = #1/1/2009#, Optional nOrigen As String = "BAN")


        Me.Tag = sModulo
        Me.Dock = DockStyle.Fill
        myConn = New MySqlConnection(jytsistema.strConn)
        myConn.Open()

        ReporteNumero = numReporte
        ReporteNombre = nomReporte
        CodigoBanco = CodBanco
        Origen = nOrigen
        Documento = numDocumento
        FechaParametro = Fecha

        ft.RellenaCombo(aDesde, cmbOrdenDesde)
        ft.RellenaCombo(aHasta, cmbOrdenHasta)
        ft.RellenaCombo(aInNOTIn, cmbOrigen)

        ft.habilitarObjetos(False, True, cmbOrdenDesde, cmbOrdenHasta)

        PresentarReporte(numReporte, nomReporte, CodBanco)

        If TipoCarga = TipoCargaFormulario.iShow Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If

    End Sub
    Private Sub PresentarReporte(ByVal NumeroReporte As Integer, ByVal NombreDelReporte As String, Optional ByVal CodigoBanco As String = "")
        lblNombreReporte.Text += " - " + NombreDelReporte
        Select Case NumeroReporte
            Case ReporteBancos.cFichaBanco
                Dim vOrdenNombres() As String = {"Código Banco"}
                Dim vOrdenCampos() As String = {"codban"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {5}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoBanco)
                bOrigenBancos = True

            Case ReporteBancos.cDisponibilidad, ReporteBancos.cConciliacion, ReporteBancos.cEstadoCuenta, _
                ReporteBancos.cIDB, ReporteBancos.cListadoBancos, ReporteBancos.cChequesDevueltos
                Dim vOrdenNombres() As String = {"Código Banco"}
                Dim vOrdenCampos() As String = {"codban"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {5}
                Inicializar(ReporteNombre, True, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoBanco)
                bOrigenBancos = True

            Case ReporteBancos.cSaldosMensuales, ReporteBancos.cIDBMes
                Dim vOrdenNombres() As String = {"Código Banco"}
                Dim vOrdenCampos() As String = {"codban"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {5}
                Inicializar(ReporteNombre, True, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoBanco)
                bOrigenBancos = True

            Case ReporteBancos.cMovimientoBanco
                Dim vOrdenNombres() As String = {"Banco, Fecha emisión"}
                Dim vOrdenCampos() As String = {"codban"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {5}
                Inicializar(ReporteNombre, False, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoBanco)
                bOrigenBancos = True

            Case ReporteBancos.cMovimientoCaja
                Dim vOrdenNombres() As String = {"Código Caja, Fecha Emisión "}
                Dim vOrdenCampos() As String = {"caja"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {2}
                Inicializar(ReporteNombre, False, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoBanco)
                bOrigenBancos = False

            Case ReporteBancos.cArqueoCajas, ReporteBancos.cMovimientosPostDatados
                Dim vOrdenNombres() As String = {"Código Caja, Fecha Emisión "}
                Dim vOrdenCampos() As String = {"caja"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {2}
                Inicializar(ReporteNombre, True, False, True, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoBanco)
                bOrigenBancos = False

            Case ReporteBancos.cDepositos
                Dim vOrdenNombres() As String = {"Banco, Fecha emisión"}
                Dim vOrdenCampos() As String = {"codban"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {5}
                Inicializar(ReporteNombre, True, False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoBanco)
                bOrigenBancos = True

            Case ReporteBancos.cChequeDevuelto
                Dim vOrdenNombres() As String = {"Nº Cheque"}
                Dim vOrdenCampos() As String = {"numorg"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {15}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoBanco)
            Case ReporteBancos.cTicketDevuelto
                Dim vOrdenNombres() As String = {"Nº Ticket"}
                Dim vOrdenCampos() As String = {"numorg"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {30}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoBanco)
            Case ReporteBancos.cTransferencia
                Dim vOrdenNombres() As String = {"Nº TRANSFERENCIA"}
                Dim vOrdenCampos() As String = {"numdoc"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {30}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoBanco)
            Case ReporteBancos.cRemesasTickets
                Dim vOrdenNombres() As String = {"Nº REMESA/SOBRE"}
                Dim vOrdenCampos() As String = {"nomsobre"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {30}
                Inicializar(ReporteNombre, True, False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoBanco)
            Case ReporteBancos.cComprobanteDeEgreso, ReporteBancos.cCheque
                Dim vOrdenNombres() As String = {"Nº COMPROBANTE"}
                Dim vOrdenCampos() As String = {"comproba"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {30}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteBancos.cFormatoCheque
                Dim vOrdenNombres() As String = {"Código Banco"}
                Dim vOrdenCampos() As String = {"CODBAN"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {30}
                Inicializar(ReporteNombre, False, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, CodigoBanco)
                bOrigenBancos = True
            Case ReporteBancos.cReposicionSaldoCaja
                Dim vOrdenNombres() As String = {"Documento"}
                Dim vOrdenCampos() As String = {"comproba"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {30}
                Inicializar(ReporteNombre, True, False, False, False, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
            Case ReporteBancos.cResumenReposicionSaldoCaja
                Dim vOrdenNombres() As String = {"Documento"}
                Dim vOrdenCampos() As String = {"comproba"}
                Dim vOrdenTipo() As String = {"S"}
                Dim vOrdenLongitud() As Integer = {30}
                Inicializar(ReporteNombre, True, False, True, True, vOrdenNombres, vOrdenCampos, vOrdenTipo, vOrdenLongitud, Documento)
        End Select
    End Sub
    Private Sub Inicializar(ByVal nEtiqueta As String, ByVal TabOrden As Boolean, ByVal TabGrupo As Boolean, _
        ByVal TabCriterio As Boolean, ByVal TabConstantes As Boolean, ByVal aNombreOrden() As String, _
        ByVal aCampoOrden() As String, ByVal aTipoOrden() As String, ByVal aLongitudOrden() As Integer, _
        Optional ByVal CodBanco As String = "")


        HabilitarTabs(TabOrden, TabGrupo, TabCriterio, TabConstantes)
        txtOrdenDesde.Text = CodBanco
        txtOrdenHasta.Text = CodBanco
        IniciarOrden(aNombreOrden, aCampoOrden, aTipoOrden, aLongitudOrden)
        IniciarGrupos()
        IniciarCriterios()
        IniciarConstantes()

    End Sub
    Private Sub HabilitarTabs(ByVal Orden As Boolean, ByVal GRupo As Boolean, ByVal Criterio As Boolean, ByVal Constante As Boolean)

        ft.habilitarObjetos(Orden, False, grpOrden)
        ft.habilitarObjetos(GRupo, False, grpGrupos)
        ft.habilitarObjetos(Criterio, False, grpCriterios)
        ft.habilitarObjetos(Constante, False, grpConstantes)

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
        VerCriterio_Documento(False)
        VerCriterio_Deposito(False)
        VerCriterio_Origen(False)
        VerCriterio_Caja(False)

        Select Case ReporteNumero
            Case ReporteBancos.cFichaBanco, ReporteBancos.cSaldosMensuales, ReporteBancos.cIDBMes, _
                ReporteBancos.cChequeDevuelto, ReporteBancos.cTicketDevuelto

            Case ReporteBancos.cMovimientoBanco
                VerCriterio_Periodo(True, DesdeHasta.cDesdeHasta)
                VerCriterio_TipoDocumento(True)
                VerCriterio_Origen(True)

            Case ReporteBancos.cMovimientoCaja
                VerCriterio_Periodo(True, DesdeHasta.cDesdeHasta)
                VerCriterio_TipoDocumento(True, 1)
                VerCriterio_Origen(True)

            Case ReporteBancos.cDisponibilidad
                VerCriterio_Periodo(True, DesdeHasta.cHasta, TipoPeriodo.iDiario)
                VerCriterio_TipoDocumento(False)
            Case ReporteBancos.cEstadoCuenta, ReporteBancos.cIDB, ReporteBancos.cChequesDevueltos, _
                ReporteBancos.cRemesasTickets
                VerCriterio_Periodo(True, DesdeHasta.cDesdeHasta)
            Case ReporteBancos.cArqueoCajas
                VerCriterio_Periodo(True, DesdeHasta.cDesdeHasta, TipoPeriodo.iDiario)
            Case ReporteBancos.cListadoBancos
                VerCriterio_Periodo(True, DesdeHasta.cHasta, TipoPeriodo.iDiario)
            Case ReporteBancos.cConciliacion
                VerCriterio_MesAño(True)
            Case ReporteBancos.cDepositos
                VerCriterio_Periodo(True, DesdeHasta.cDesdeHasta)
                VerCriterio_Documento(True)
                VerCriterio_Deposito(True)
            Case ReporteBancos.cMovimientosPostDatados
                VerCriterio_Periodo(True, DesdeHasta.cDesde, TipoPeriodo.iDiario)
                VerCriterio_TipoDocumento(True, 2)
            Case ReporteBancos.cResumenReposicionSaldoCaja
                VerCriterio_Periodo(True, DesdeHasta.cDesdeHasta, TipoPeriodo.iMensual)
                VerCriterio_Caja(True)
        End Select

    End Sub

    Private Sub VerCriterio_Periodo(ByVal Ver As Boolean, ByVal CompletoDesdeHasta As Integer, Optional ByVal Periodo As TipoPeriodo = TipoPeriodo.iMensual)
        'CompletoDesdeHasta 0 = Complete , 1 = Desde , 2 = Hasta 
        ft.visualizarObjetos(False, lblPeriodoDesde, lblPeriodoHasta, lblPeriodo, txtPeriodoDesde, txtPeriodoHasta, btnPeriodoDesde, btnPeriodoHasta)
        ft.habilitarObjetos(False, True, txtPeriodoDesde, txtPeriodoHasta)
        PeriodoTipo = Periodo
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
    Private Sub VerCriterio_TipoDocumento(ByVal ver As Boolean, Optional ByVal DocumentosTipo As Integer = 0)
        'DocumentosTipo : 0 = Bancos, 1 = caja, 2 = Forma de pago
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
            Case 2
                Dim aTipoDocumento() As String = {"EF", "CH", "TA", "CT"}
                Dim aSel() As Boolean = {True, True, True, True}
                RellenaListaSeleccionable(chkList, aTipoDocumento, aSel)
                txtTipDoc.Text = ".EF.CH.TA.CT"
        End Select
    End Sub
    Private Sub VerCriterio_MesAño(ByVal ver As Boolean)
        ft.visualizarObjetos(ver, lblMesAño, lblMes, lblAño, cmbMes, cmbAño)
        IniciarCriterioMesAño()
    End Sub
    Private Sub VerCriterio_Origen(ver As Boolean)
        ft.visualizarObjetos(ver, lblOrigen, cmbOrigen, btnOrigen, lblOrigenSeleccion)
    End Sub
    Private Sub VerCriterio_Documento(ByVal ver As Boolean)

        ft.visualizarObjetos(ver, lblDocHasta, lblDocDesde, lblDocumento, txtDocumentoDesde, txtDocumentoHasta)

        txtDocumentoDesde.Enabled = True : txtDocumentoDesde.MaxLength = 15
        txtDocumentoHasta.Enabled = True : txtDocumentoHasta.MaxLength = 15

    End Sub
    Private Sub VerCriterio_Caja(ByVal ver As Boolean)

        ft.visualizarObjetos(ver, lblCaja, lblCajaDesde, lblCajaHasta, txtCajaDesde, txtCajaHasta)
        txtCajaDesde.Enabled = True : txtCajaDesde.MaxLength = 2
        txtCajaHasta.Enabled = True : txtCajaHasta.MaxLength = 2

    End Sub
    Private Sub VerCriterio_Deposito(ByVal ver As Boolean)

        ft.visualizarObjetos(ver, Label1, Label2, Label3, txtDepositoDesde, txtDepositoHasta)

        txtDepositoDesde.Enabled = True : txtDepositoDesde.MaxLength = 35
        txtDepositoHasta.Enabled = True : txtDepositoHasta.MaxLength = 35

    End Sub

    Private Sub IniciarConstantes()
        verConstate_Resumen(False)
        verConstate_Resumen(False)

        Select Case ReporteNumero
            Case ReporteBancos.cDepositos
                verConstate_Resumen(True)
            Case ReporteBancos.cRemesasTickets
                verConstate_Resumen(True)
                verConstate_Remesa(True, True)
            Case ReporteBancos.cResumenReposicionSaldoCaja
                verConstate_Resumen(True)
        End Select

    End Sub
    Private Sub verConstate_Resumen(ByVal Ver As Boolean, Optional ByVal Valor As Boolean = False)
        ft.visualizarObjetos(Ver, lblConsResumen, chkConsResumen)
        chkConsResumen.Checked = Valor
        If Not chkConsResumen.Checked AndAlso ReporteNumero = ReporteBancos.cRemesasTickets Then verConstate_Tickets(True)
    End Sub
    Private Sub verConstate_Tickets(ByVal Ver As Boolean, Optional ByVal Valor As Boolean = False)
        ft.visualizarObjetos(Ver, lblconsTickets, chkTickets)
        chkTickets.Checked = Valor
    End Sub
    Private Sub verConstate_Remesa(ByVal Ver As Boolean, Optional ByVal Valor As Boolean = False)
        ft.visualizarObjetos(Ver, lblConsRemesas, rdbDepositadas, rdbXDepositar)
        rdbDepositadas.Checked = Valor
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
                'txtOrdenDesde.Text = CodigoBanco
                'txtOrdenHasta.Text = CodigoBanco
        End Select

    End Sub
    Private Sub IniciarCriterioMesAño()

        Dim Fecha As Date = IIf(ft.FormatoFechaMySQL(FechaParametro) = "2009-01-01", jytsistema.sFechadeTrabajo, FechaParametro)

        Dim aMeses() As String = {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"}
        ft.RellenaCombo(aMeses, cmbMes, Month(Fecha) - 1)
        Dim aAño() As String = {"2000", "2001", "2002", "2003", "2004", "2005", "2006", "2007", "2008", "2009", "2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019", "2020", "2021", "2022", "2023", "2024", "2025"}
        ft.RellenaCombo(aAño, cmbAño, Year(Fecha) - 2000)

    End Sub

    Private Sub jsBanRepParametros_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

    End Sub

    Private Sub jsBanRepParametros_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(myConn, MovAud.ientrar, sModulo, ReporteNumero)
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        myConn.Close()
        myConn = Nothing
        Me.Close()
    End Sub
    Private Sub IniciarCadenas()

        cadOrigen = prepararCadena(lblOrigenSeleccion.Text, cmbOrigen)

    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

        HabilitarCursorEnEspera()

        Dim r As New frmViewer
        Dim dsBan As New dsBancos
        Dim str As String = ""
        Dim nTabla As String = ""
        Dim oReporte As New CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim PresentaArbol As Boolean = False

        IniciarCadenas()

        Select Case ReporteNumero
            Case ReporteBancos.cFichaBanco
                nTabla = "dtBancosFicha"
                oReporte = New rptBanBancosFicha
                str = SeleccionBANBancos(CodigoBanco, CodigoBanco, CDate(txtPeriodoHasta.Text))
            Case ReporteBancos.cListadoBancos
                nTabla = "dtBancosFicha"
                oReporte = New rptBanBancosListado
                str = SeleccionBANBancos(txtOrdenDesde.Text, txtOrdenHasta.Text, CDate(txtPeriodoHasta.Text))
            Case ReporteBancos.cMovimientoBanco
                nTabla = "dtBancosMovimientos"
                oReporte = New rptBanBancosMovimientos
                str = SeleccionBANMovimientosBancos(CodigoBanco, CodigoBanco, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), txtTipDoc.Text, cadOrigen)
            Case ReporteBancos.cMovimientoCaja
                nTabla = "dtCajaMovimientos"
                oReporte = New rptBanCajaMovimientos
                str = SeleccionBANMovimientosCaja(CodigoBanco, CodigoBanco, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), cadOrigen, txtTipDoc.Text)
            Case ReporteBancos.cArqueoCajas
                nTabla = "dtCajaMovimientos"
                oReporte = New rptBanCajaCierre
                str = SeleccionBANMovimientosCaja(txtOrdenDesde.Text, txtOrdenHasta.Text, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), cadOrigen)
                PresentaArbol = True
            Case ReporteBancos.cDisponibilidad
                nTabla = "dtBancosFicha"
                oReporte = New rptBanListadoBancos_Disponibilidad
                str = SeleccionBANBancos(txtOrdenDesde.Text, txtOrdenHasta.Text, CDate(txtPeriodoHasta.Text), True)
            Case ReporteBancos.cConciliacion
                nTabla = "dtConciliacion"
                oReporte = New rptBanConciliacionBancaria
                str = SeleccionBANConciliacion(txtOrdenDesde.Text, txtOrdenHasta.Text, CDate("01/" + Format(cmbMes.SelectedIndex + 1, "00") + "/" + CStr(cmbAño.SelectedIndex + 2000)))
                PresentaArbol = True
            Case ReporteBancos.cSaldosMensuales
                nTabla = "dtSaldosMes"
                oReporte = New rptBanSaldosPorMes
                str = SeleccionBANSaldosXMes(txtOrdenDesde.Text, txtOrdenHasta.Text)
            Case ReporteBancos.cEstadoCuenta
                nTabla = "dtEstadoCuenta"
                oReporte = New rptBanEstadodeCuenta
                str = SeleccionBANEstadoCuenta(CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), txtOrdenDesde.Text, txtOrdenHasta.Text)
            Case ReporteBancos.cDepositos
                nTabla = "dtDepositos"
                oReporte = New rptBanDepositos
                str = SeleccionBANDepositos(txtOrdenDesde.Text, txtOrdenHasta.Text, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), _
                                            txtDocumentoDesde.Text, txtDocumentoHasta.Text, txtDepositoDesde.Text, txtDepositoHasta.Text)
            Case ReporteBancos.cIDB
                nTabla = "dtIDB"
                oReporte = New rptBanBancosIDB
                str = SeleccionBANDebitoBancario(txtOrdenDesde.Text, txtOrdenHasta.Text, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text))
            Case ReporteBancos.cIDBMes
                nTabla = "dtIDBMes"
                oReporte = New rptBanBancosIDBMes
                str = SeleccionBANDebitoBancarioMes(txtOrdenDesde.Text, txtOrdenHasta.Text, Year(jytsistema.sFechadeTrabajo))
                PresentaArbol = True
            Case ReporteBancos.cChequeDevuelto
                nTabla = "dtChequeDevuelto"
                oReporte = New rptBanNotaChequeDevuelto
                str = SeleccionBANChequeDevuelto(CodigoBanco, FechaParametro, Documento)
            Case ReporteBancos.cTicketDevuelto
                nTabla = "dtTicketDevuelto"
                oReporte = New rptBanNotaTicketDevuelto
                str = SeleccionBANTicketDevuelto(txtOrdenDesde.Text)
            Case ReporteBancos.cTransferencia
                nTabla = "dtTransfer"
                oReporte = New rptBanTransferencia
                str = SeleccionBANTransferencia(txtOrdenDesde.Text)
            Case ReporteBancos.cChequesDevueltos
                nTabla = "dtChequesDevueltos"
                oReporte = New rptBanChequesDevueltos
                str = SeleccionBANChequesDevueltosBancos(txtOrdenDesde.Text, txtOrdenHasta.Text, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text))
                PresentaArbol = True
            Case ReporteBancos.cRemesasTickets
                nTabla = "dtRemesas"
                oReporte = New rptBanRemesasCestaTicket
                str = SeleccionBANRemesaCestaTicket(txtOrdenDesde.Text, txtOrdenHasta.Text, CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text), rdbDepositadas.Checked)
                PresentaArbol = True
            Case ReporteBancos.cMovimientosPostDatados
                nTabla = "dtNoDepositado"
                oReporte = New rptBanCajaNoDepositada
                str = SeleccionBANCajaNoDepositado(txtOrdenDesde.Text, txtOrdenHasta.Text, CDate(txtPeriodoDesde.Text), txtTipDoc.Text)
            Case ReporteBancos.cComprobanteDeEgreso
                nTabla = "dtComprobantePago"
                If Origen = "BAN" Then
                    Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                    sRespuesta = MsgBox(" ¿ Imprimir comprobantes con POLIZA contable ?", MsgBoxStyle.YesNo, "POLIZA CONTABLE ... ")
                    If sRespuesta = MsgBoxResult.Yes Then
                        oReporte = New rptBanComprobanteEgresoContable
                        str = SeleccionBANComprobanteChequeContable(CodigoBanco, Documento)
                    Else
                        oReporte = New rptBanComprobanteEgreso
                        str = SeleccionBANComprobanteCheque(CodigoBanco, Documento)
                    End If

                Else
                    oReporte = New rptBanComprobanteEgreso
                    str = SeleccionCOMPRASComprobantePagoCH(Documento)
                End If
            Case ReporteBancos.cCheque
                nTabla = "dtComprobantePago"
                oReporte = New rptBanFormatoCheque
                str = SeleccionBANComprobanteCheque(CodigoBanco, Documento)
            Case ReporteBancos.cFormatoCheque
                nTabla = "dtComprobantePago"
                oReporte = New rptBanFormatoCheque
                str = SeleccionBANFormatoCheque()
            Case ReporteBancos.cReposicionSaldoCaja
                nTabla = "dtDepositos"
                oReporte = New rptBanReposisionSaldoCaja
                str = SeleccionBANReposicionSaldoCaja(txtOrdenDesde.Text, txtOrdenHasta.Text, CodigoBanco, CodigoBanco)
            Case ReporteBancos.cResumenReposicionSaldoCaja
                nTabla = "dtDepositos"
                oReporte = New rptBanReposisionSaldoCaja
                str = SeleccionBANReposicionSaldoCaja(txtOrdenDesde.Text, txtOrdenHasta.Text, _
                                                      txtCajaDesde.Text, txtCajaHasta.Text, _
                                                      CDate(txtPeriodoDesde.Text), CDate(txtPeriodoHasta.Text))

            Case Else
                oReporte = Nothing
        End Select

        If nTabla <> "" Then
            dsBan = DataSetRequery(dsBan, str, myConn, nTabla, lblInfo)
            If dsBan.Tables(nTabla).Rows.Count > 0 Then
                oReporte = PresentaReporte(oReporte, dsBan, nTabla)
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
            Case ReporteBancos.cDisponibilidad, ReporteBancos.cListadoBancos
                oReporte.SetParameterValue("SaldoAl", " Saldo al " + txtPeriodoHasta.Text)
            Case ReporteBancos.cConciliacion
                oReporte.SetParameterValue("SaldoAl", " Saldo al " + ft.FormatoFecha(DateAdd(DateInterval.Day, -1, CDate("01/" + Format(cmbMes.SelectedIndex + 1, "00") + "/" + CStr(cmbAño.SelectedIndex + 2000)))))
            Case ReporteBancos.cEstadoCuenta
                oReporte.SetParameterValue("SaldoAl", " Saldo al " + ft.FormatoFecha(DateAdd(DateInterval.Day, -1, CDate(txtPeriodoDesde.Text))))
            Case ReporteBancos.cDepositos
                oReporte.ReportDefinition.Sections("DetailSection1").SectionFormat.EnableSuppress = chkConsResumen.Checked
            Case ReporteBancos.cResumenReposicionSaldoCaja
                oReporte.ReportDefinition.Sections("DetailSection1").SectionFormat.EnableSuppress = chkConsResumen.Checked

            Case ReporteBancos.cChequeDevuelto
                Dim myTextObjectOnReport As CrystalDecisions.CrystalReports.Engine.TextObject
                myTextObjectOnReport = CType(oReporte.ReportDefinition.ReportObjects.Item("txtBloqueo"), CrystalDecisions.CrystalReports.Engine.TextObject)
                If CDbl(IIf(ParametroPlus(MyConn, Gestion.iBancos, "VENPARAM03").ToString = "", "0", ParametroPlus(MyConn, Gestion.iBancos, "VENPARAM03").ToString)) > 0 Then
                    If ds.Tables(nTabla).Rows(0).Item("chequesano") >= CInt(ParametroPlus(MyConn, Gestion.iBancos, "VENPARAM03")) Then
                        myTextObjectOnReport.Text = "Bloqueado"
                    Else
                        myTextObjectOnReport.Text = "OK!"
                    End If
                End If
            Case ReporteBancos.cRemesasTickets
                oReporte.ReportDefinition.Sections("GroupHeaderSection3").SectionFormat.EnableSuppress = chkConsResumen.Checked
                oReporte.ReportDefinition.Sections("DetailSection1").SectionFormat.EnableSuppress = chkConsResumen.Checked
                oReporte.ReportDefinition.Sections("DetailSection1").SectionFormat.EnableSuppress = chkTickets.Checked
            Case ReporteBancos.cComprobanteDeEgreso, ReporteBancos.cCheque
                Dim aFld() As String = {"codban", "comproba", "id_emp"}
                Dim aStr() As String = {CodigoBanco, Documento, jytsistema.WorkID}
                Dim MontoEgreso As Double = Math.Abs(ft.DevuelveScalarDoble(myConn, " select importe from jsbantraban where codban = '" & CodigoBanco & "' and comproba = '" & Documento & "' and id_emp = '" & jytsistema.WorkID & "' "))
                Dim Decimales As Integer = MontoEgreso - Int(MontoEgreso)

                oReporte.SetParameterValue("MontoEnLetras", UCase(NumerosATexto(MontoEgreso)))
                oReporte.SetParameterValue("MontoEnNumeros", "***" + ft.FormatoNumero(MontoEgreso) + "***")

                Dim aaFld() As String = {"codban", "id_emp"}
                Dim aaStr() As String = {CodigoBanco, jytsistema.WorkID}
                Dim FormatoCheque As String = qFoundAndSign(myConn, lblInfo, "jsbancatban", aaFld, aaStr, "formato")

                Dim aaaFld() As String = {"formato", "id_emp"}
                Dim aaaStr() As String = {FormatoCheque, jytsistema.WorkID}

                Dim MontoEnLetrasTop As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "montoletratop"))
                Dim MontoEnLetrasLeft As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "montoletraleft"))
                Dim MontoTop As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "montotop"))
                Dim MontoLeft As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "montoleft"))
                Dim NombreTop As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "nombretop"))
                Dim NombreLeft As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "nombreleft"))
                Dim FechaTop As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "fechatop"))
                Dim FechaLeft As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "fechaleft"))
                Dim NoEndosableTop As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "noendosabletop"))
                Dim NoEndosableLeft As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "noendosableleft"))

                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("MontoEnNumeros1").Left = MontoLeft
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("MontoEnNumeros1").Top = MontoTop
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("benefic1").Left = NombreLeft
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("benefic1").Top = NombreTop
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("MontoEnLetras2").Left = MontoEnLetrasLeft
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("MontoEnLetras2").Top = MontoEnLetrasTop
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("emision2").Left = FechaLeft
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("emision2").Top = FechaTop
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("NoEndosable").Left = NoEndosableLeft
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("NoEndosable").Top = NoEndosableTop

            Case ReporteBancos.cFormatoCheque

                oReporte.SetParameterValue("MontoEnLetras", UCase(NumerosATexto(1234567.89)))
                oReporte.SetParameterValue("MontoEnNumeros", "***" + ft.FormatoNumero(1234567.89) + "***")

                Dim FormatoCheque As String = Documento

                Dim aaaFld() As String = {"formato", "id_emp"}
                Dim aaaStr() As String = {FormatoCheque, jytsistema.WorkID}

                Dim MontoEnLetrasTop As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "montoletratop"))
                Dim MontoEnLetrasLeft As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "montoletraleft"))
                Dim MontoTop As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "montotop"))
                Dim MontoLeft As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "montoleft"))
                Dim NombreTop As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "nombretop"))
                Dim NombreLeft As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "nombreleft"))
                Dim FechaTop As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "fechatop"))
                Dim FechaLeft As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "fechaleft"))
                Dim NoEndosableTop As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "noendosabletop"))
                Dim NoEndosableLeft As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsbancatfor", aaaFld, aaaStr, "noendosableleft"))

                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("MontoEnNumeros1").Left = MontoLeft
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("MontoEnNumeros1").Top = MontoTop
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("benefic1").Left = NombreLeft
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("benefic1").Top = NombreTop
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("MontoEnLetras2").Left = MontoEnLetrasLeft
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("MontoEnLetras2").Top = MontoEnLetrasTop
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("emision2").Left = FechaLeft
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("emision2").Top = FechaTop
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("NoEndosable").Left = NoEndosableLeft
                oReporte.ReportDefinition.Sections("PageHeaderSection3").ReportObjects("NoEndosable").Top = NoEndosableTop

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
    Private Function LineaCriterios() As String
        LineaCriterios = ""
        If lblPeriodo.Visible Then LineaCriterios += "Período: " & IIf(lblPeriodoDesde.Visible, txtPeriodoDesde.Text, "") & IIf(lblPeriodoDesde.Visible AndAlso lblPeriodoHasta.Visible, "/", "") & IIf(lblPeriodoHasta.Visible, txtPeriodoHasta.Text, "")
        If lblTipodocumento.Visible Then LineaCriterios += " - " + " Tipo Documentos : " + txtTipDoc.Text
        If lblMesAño.Visible Then LineaCriterios += " Mes y Año " + cmbMes.Text + "/" + cmbAño.Text
        If lblDocumento.Visible Then LineaCriterios += " - " + " Documento : " + txtDocumentoDesde.Text + "/" + txtDocumentoHasta.Text
    End Function
    Private Function LineaConstantes() As String
        LineaConstantes = ""
        If lblConsResumen.Visible Then LineaConstantes += " Resumido : " + IIf(chkConsResumen.Checked, "Si", "No")
        If lblconsTickets.Visible Then LineaConstantes += " - Muestra tickets : " + IIf(chkTickets.Checked, "Si", "No")
        If lblConsResumen.Visible Then LineaConstantes += " - Remesas : " + IIf(rdbDepositadas.Checked, "Depositadas", "Por depositar ")
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
            Case "Código Banco", "Banco, Fecha emisión"
                ds = DataSetRequery(ds, "select codban codigo, nomban descripcion from jsbancatban where id_emp = '" & jytsistema.WorkID & "' order by codban ", myConn, nTAbleOrden, lblInfo)
                dtDeOrden = ds.Tables(nTAbleOrden)
                f.Cargar("Bancos", ds, dtDeOrden, nTAbleOrden, TipoCargaFormulario.iShowDialog, False)
                txt.Text = f.Seleccion
            Case "Código Caja, Fecha Emisión "
                ds = DataSetRequery(ds, "select caja codigo, nomcaja descripcion from jsbanenccaj where id_emp = '" & jytsistema.WorkID & "' order by caja ", myConn, nTAbleOrden, lblInfo)
                dtDeOrden = ds.Tables(nTAbleOrden)
                f.Cargar("Cajas", ds, dtDeOrden, nTAbleOrden, TipoCargaFormulario.iShowDialog, False)
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

    Private Sub chkConsResumen_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkConsResumen.CheckedChanged
        If ReporteNumero = ReporteBancos.cRemesasTickets Then
            If chkConsResumen.Checked Then
                verConstate_Tickets(False, True)
            Else
                verConstate_Tickets(True)
            End If

        End If
    End Sub

    Private Sub cmbOrdenDesde_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOrdenDesde.SelectedIndexChanged
        If cmbOrdenHasta.Items.Count > 0 Then cmbOrdenHasta.SelectedIndex = cmbOrdenDesde.SelectedIndex
    End Sub

    Private Sub cmbOrdenHasta_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOrdenHasta.SelectedIndexChanged
        If cmbOrdenDesde.Items.Count > 0 Then cmbOrdenDesde.SelectedIndex = cmbOrdenHasta.SelectedIndex
    End Sub

    Private Sub txtPeriodoDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPeriodoDesde.TextChanged
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

    Private Sub txtDocumentoDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtDocumentoDesde.TextChanged
        txtDocumentoHasta.Text = txtDocumentoDesde.Text
    End Sub

    Private Sub txtDepositoDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtDepositoDesde.TextChanged
        txtDepositoHasta.Text = txtDepositoDesde.Text
    End Sub

    Private Sub btnOrigen_Click(sender As System.Object, e As System.EventArgs) Handles btnOrigen.Click
        If bOrigenBancos Then
            AbrirTabla("ORIGENES DOCUMENTOS", " SELECT  0 sel, origen CODIGO, '' DESCRIP FROM jsbantraban GROUP BY 2 ", lblOrigenSeleccion)
        Else
            AbrirTabla("ORIGENES DOCUMENTOS", " SELECT  0 sel, origen CODIGO, '' DESCRIP FROM jsbantracaj GROUP BY 2 ", lblOrigenSeleccion)
        End If
    End Sub
    Private Sub AbrirTabla(Titulo As String, strSQL As String, lblSeleccion As Label)
        Dim f As New jsGenListadoSeleccion
        Dim aNombres() As String = {"", "CÓDIGO", "DESCRIPCION"}
        Dim aCampos() As String = {"sel", "codigo", "descrip"}
        Dim aAnchos() As Integer = {20, 80, 150}
        Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Left}
        Dim aFormato() As String = {"", "", ""}

        f.Seleccion = SubeSeleccion(lblSeleccion.Text)
        f.Cargar(myConn, ds, Titulo, strSQL, _
            {"sel.entero.1.0", "codigo.cadena.15.0", "descrip.cadena.150.0"}, aNombres, aCampos, aAnchos, aAlineacion, aFormato)
        lblSeleccion.Text = bajaSeleccion(f.Seleccion)

        f.Dispose()
        f = Nothing
    End Sub
    Private Function SubeSeleccion(textoSeleccion As String) As String()
        SubeSeleccion = {}
        If textoSeleccion.Trim <> "" Then
            SubeSeleccion = Replace(textoSeleccion, "'", "").Split(",")
        End If
    End Function
    Private Function bajaSeleccion(arraySeleccion() As String) As String
        bajaSeleccion = ""
        If arraySeleccion.Length > 0 Then bajaSeleccion = "'" & String.Join("','", arraySeleccion) & "'"
    End Function
    Private Function prepararCadena(str As String, cmb As ComboBox) As String
        Return IIf(str.Trim() = "", "", IIf(cmb.SelectedIndex = 0, " IN ( ", " NOT IN ( ") & str & ") ")
    End Function

    Private Sub txtCajaDesde_TextChanged(sender As Object, e As EventArgs) Handles txtCajaDesde.TextChanged
        txtCajaHasta.Text = txtCajaDesde.Text
    End Sub
End Class