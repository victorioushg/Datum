Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports C1.Win.C1Ribbon
Imports Microsoft.Win32
Imports System.Globalization
Imports ReportesDeBancos
Imports FontAwesomeNet

Public Class scrMain

    Private Const sModulo As String = "Datum"
    Private Const strSQLEmpresa As String = ""
    Private Const NombreTabla As String = "empresa"

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private myCom As New MySqlCommand(strSQLEmpresa, myConn)
    Private ds As New DataSet
    Private dt As New DataTable


    Private tsLFecha As New ToolStripStatusLabel
    Private tsLEmpresa As New ToolStripStatusLabel
    Private tslUsuario As New ToolStripStatusLabel
    Private tsLNFecha As New ToolStripStatusLabel
    Private tsLNEmpresa As New ToolStripStatusLabel
    Private tslNUsuario As New ToolStripStatusLabel
    Private tsLWin As New ToolStripStatusLabel
    Private tsLNWin As New ToolStripStatusLabel
    Private tslEjercicio As New ToolStripStatusLabel
    Private tslNEjercicio As New ToolStripStatusLabel

    Private ft As New Transportables

    Private inicioHorarios As Boolean = False
    Private lblInfo As New Label
    Private strGes As String = "Datum - "

    Private aGestionVisible(0 To 9) As Boolean

    Private m_ChildFormNumber As Integer = 0

    Private Sub scrMain_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        ft = Nothing
        dt = Nothing
        ds = Nothing
        myCom = Nothing
        myConn.Close()
        myConn = Nothing

    End Sub

    Private Sub scrMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.WindowState = FormWindowState.Maximized

        lblInfo.Visible = False

        Try

            myConn.Open()

            jytsistema.WorkID = ""
            Dim nUser As String = ft.DevuelveScalarCadena(myConn, " SELECT USUARIO FROM jsconctausu where id_user = '" & jytsistema.sUsuario & "'  ")
            jytsistema.sFechadeTrabajo = Now()
            If ExisteTabla(myConn, jytsistema.WorkDataBase, "jsconctausuemp") Then jytsistema.WorkID = ft.DevuelveScalarCadena(myConn, " select ID_EMP FROM jsconctausuemp where id_user = '" & jytsistema.sUsuario & "' and empresa_inicial = 1 ")
            If jytsistema.WorkID = "" Then jytsistema.WorkID = ft.DevuelveScalarCadena(myConn, " select INI_EMP FROM jsconctausu where id_user = '" & jytsistema.sUsuario & "' ")
            If jytsistema.WorkID = "" Then jytsistema.WorkID = ft.DevuelveScalarCadena(myConn, " select id_emp from jsconctaemp order by id_emp limit 1 ")

            jytsistema.WorkName = ft.DevuelveScalarCadena(myConn, " SELECT NOMBRE FROM jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ")
            jytsistema.WorkExercise = ""
            jytsistema.WorkCurrency = GetWorkCurrency(myConn)

            Me.Text += " " & jytsistema.nVersion & "  " &
                IIf(nUser = "jytsuite", jytsistema.strConn.Split(";")(0), "")

            IniciarParametros_Y_Contadores()

            IniciarGestiones()
            IniciarCultura()

            IniciarBloqueoDeClientes()
            IniciarCierreDiarioVentas()
            IniciarCierreDeRutas(myConn, ds)
            ''      inicioHorarios = IniciarDiasTrabajo(myConn, ds, jytsistema.sFechadeTrabajo, lblInfo)

            IniciarReporteador()
            IniciarUnidadesDeMedida(myConn)

        Catch ex As MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub IniciarReconversion_20180604()

        Dim VerificaReconversion As Integer = ft.DevuelveScalarEntero(myConn, " select RECONVERSION_20180604 from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ")

        If VerificaReconversion = 0 Then
            If ft.Pregunta("Desea realizar reconversión monetaria", "RECONVERSION MONETARIA") = DialogResult.OK Then

            End If
        End If

    End Sub

    Private Sub IniciarCierreDeRutas(MyConn As MySqlConnection, ds As DataSet)
        Dim par As String = ParametroPlus(MyConn, Gestion.iVentas, "VENFVEPA06")
        If CBool(par) Then  'SI PROCESO AUTOMATICO
            If Not ProcesoAutomaticoInicial(MyConn, lblInfo, ProcesoAutomatico.iCierreDiarioDeRutas, jytsistema.sFechadeTrabajo) Then

                Dim dtAsesores As New DataTable
                dtAsesores = ft.AbrirDataTable(ds, "tblAsesores", MyConn, " select * from jsvencatven " _
                                                                & " where tipo = '" & TipoVendedor.iFuerzaventa & "' and " _
                                                                & " id_emp = '" & jytsistema.WorkID & "' order by codven ")
                For Each nRow As DataRow In dtAsesores.Rows
                    With nRow

                        Dim DiasRetroceso As Integer = IIf(jytsistema.sFechadeTrabajo.DayOfWeek = DayOfWeek.Monday, 3, 1)
                        Dim nFecha As Date = DateAdd(DateInterval.Day, -1 * DiasRetroceso, jytsistema.sFechadeTrabajo)

                        Dim procesada = Format(nFecha, "yyyyMMdd") & .Item("codven") & jytsistema.WorkID

                        If ft.DevuelveScalarEntero(MyConn, " SELECT COUNT(*) FROM jsvenrutcie WHERE rutacerrada = '" & procesada & "' ") = 0 Then _
                            ft.Ejecutar_strSQL(MyConn, " INSERT INTO jsvenrutcie SET rutacerrada = '" & procesada & "' ")


                    End With

                Next

                dtAsesores.Dispose()
                dtAsesores = Nothing

                ActualizarProcesoAutomaticoInicial(MyConn, lblInfo, ProcesoAutomatico.iCierreDiarioDeRutas, jytsistema.sFechadeTrabajo)

            End If

        End If

    End Sub
    Private Sub IniciarCierreDiarioVentas()

        If LoginUser(myConn, lblInfo) <> "jytsuite" Then
            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENFVEPA01")) Then  'SI PROCESO AUTOMATICO
                If Not ProcesoAutomaticoInicial(myConn, lblInfo, ProcesoAutomatico.iCierreDiario, DateAdd(DateInterval.Day, -1, jytsistema.sFechadeTrabajo)) Then
                    Dim f As New jsVenProCierreDiario
                    f.Cargar(myConn, TipoCargaFormulario.iShowDialog, DateAdd(DateInterval.Day, -1, jytsistema.sFechadeTrabajo))
                    f.Dispose()
                    f = Nothing
                    ActualizarProcesoAutomaticoInicial(myConn, lblInfo, ProcesoAutomatico.iCierreDiario, DateAdd(DateInterval.Day, -1, jytsistema.sFechadeTrabajo))
                End If
            End If
        End If

    End Sub
    Private Sub IniciarBloqueoDeClientes()

        If LoginUser(myConn, lblInfo) <> "jytsuite" Then
            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPARAM24")) Then  'SI PROCESO AUTOMATICO
                If Not ProcesoAutomaticoInicial(myConn, lblInfo, ProcesoAutomatico.iBloqueoClientes, jytsistema.sFechadeTrabajo) Then

                    Dim DiasBloqueo As Integer = CInt(ParametroPlus(myConn, Gestion.iVentas, "VENPARAM34"))
                    Dim CausaBloqueo As String = CStr(ParametroPlus(myConn, Gestion.iVentas, "VENPARAM35"))
                    Dim NoFacturar As Boolean = CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPARAM36"))

                    Dim dtClientes As DataTable
                    Dim nTablaClientes As String = "tbl" & ft.NumeroAleatorio(100000)

                    ds = DataSetRequery(ds, " select * from jsvencatcli where id_emp = '" & jytsistema.WorkID & "' order by codcli ", myConn, nTablaClientes, lblInfo)
                    dtClientes = ds.Tables(nTablaClientes)

                    Dim iCont As Integer
                    If dtClientes.Rows.Count > 0 Then
                        For iCont = 0 To dtClientes.Rows.Count - 1
                            With dtClientes.Rows(iCont)
                                VerificaVencimientoCliente(myConn, lblInfo, ds, .Item("CODCLI"), .Item("ESTATUS"),
                                                            DiasBloqueo, CausaBloqueo, NoFacturar)

                            End With
                        Next

                    End If

                    ActualizarProcesoAutomaticoInicial(myConn, lblInfo, ProcesoAutomatico.iBloqueoClientes, jytsistema.sFechadeTrabajo)

                    InsertarAuditoria(myConn, MovAud.iProcesar, sModulo, "Días : " & DiasBloqueo & " - Causa : " & CausaBloqueo & " - CHD : " & NoFacturar.ToString)



                End If
            End If
        End If

    End Sub

    Private Sub IniciarReporteador()
        '////////// MI PRIMER REPORTE CRISTAL
        Dim r As New frmViewer
        Dim dsBan As New dsBancos
        Dim oReporte As New CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim nTabla As String = "dtBancosFicha"
        oReporte = New rptBanBancosFicha
        Dim str As String = " select * from jsbancatban where codban = '00001' and id_emp = '" & jytsistema.WorkID & "' "
        oReporte.SetDataSource(ds)
        dsBan = DataSetRequery(dsBan, str, myConn, nTabla, lblInfo)

        r.CrystalReportViewer1.ReportSource = oReporte
        r.CrystalReportViewer1.Refresh()
        r.Close()
        r = Nothing
        oReporte.Close()
        oReporte = Nothing
    End Sub
    Private Sub IniciarCultura()
        '//////////// LA CULTURA POPULAR TIENE AMIGOS A MONTON
        Dim forceDotCulture As CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture.Clone()
        forceDotCulture.NumberFormat.NumberDecimalSeparator = "."
        forceDotCulture.NumberFormat.NumberGroupSeparator = ","
        System.Threading.Thread.CurrentThread.CurrentCulture = forceDotCulture

    End Sub

    Private Sub IniciarParametros_Y_Contadores()

        Dim bCampos() As String = {"id_emp"}, bStrings() As String = {jytsistema.WorkID}
        If Not qFound(myConn, lblInfo, "jsconctapar", bCampos, bStrings) Then
            ft.Ejecutar_strSQL(myconn, "insert into jsconctapar set id_emp = '" & jytsistema.WorkID & "' ")
        End If

    End Sub
    Private Sub IniciarGestiones()

        'iLic = {'contabilidad', 'Bancos', 'Recursos Humanos', 'Compras', 'Ventas', 'PVE', 'Mercancias', 'SIGME', 'Producción', 'Control'}
        If jytsistema.sUsuario <> "00000" Then RibbonButton307.Visible = False

        Dim aLic(10) As Integer
        Dim aItems() As String = {}
        Select Case AplicacionTipo
            Case TipoAplicacion.iBasic
                aItems = {"RibbonButton53", "RibbonButton191", "RibbonButton192", "RibbonButton189", "RibbonButton28",
                          "RibbonButton236",
                          "RibbonButton138", "RibbonButton140", "RibbonButton141", "RibbonButton213", "RibbonButton206", "RibbonButton293", "RibbonButton156", "RibbonButton157", "RibbonButton150", "RibbonButton159", "RibbonButton160", "RibbonMenu47", "RibbonButton280",
                          "RibbonButton179", "RibbonButton180", "RibbonButton183", "RibbonButton184", "RibbonButton185", "RibbonButton197", "RibbonButton234"}
                aLic = {0, 1, 0, 1, 0, 1, 1, 0, 0, 1}
            Case TipoAplicacion.iNormal
                aLic = {1, 1, 1, 1, 1, 1, 1, 0, 0, 1}
            Case TipoAplicacion.iPlus
                aLic = {1, 1, 1, 1, 1, 1, 1, 1, 0, 1}
            Case TipoAplicacion.iPlusProduccion
                aLic = {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            Case TipoAplicacion.iPlusGases
                aLic = {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            Case TipoAplicacion.iPlusPrensa
                aItems = {"RibbonButton138", "RibbonButton139", "RibbonButton140", "RibbonButton141", "RibbonButton142", "RibbonButton210", "RibbonButton284",
                          "RibbonButton213", "RibbonButton206", "RibbonButton293",
                          "RibbonMenu13", "RibbonMenu36",
                          "RibbonButton146", "RibbonButton147", "RibbonButton148", "RibbonButton149", "RibbonButton150", "RibbonButton265",
                          "RibbonMenu23", "RibbonMenu24", "RibbonMenu47", "RibbonMenu49"}
                aLic = {1, 1, 1, 1, 0, 0, 1, 0, 0, 1}
            Case Else
                aLic = {1, 1, 1, 1, 1, 1, 1, 1, 0, 1}
        End Select

        aGestionVisible(0) = aLic(0) 'Contabilidad
        aGestionVisible(1) = aLic(1) 'Bancos
        aGestionVisible(2) = aLic(2) 'Recursos Humanos
        aGestionVisible(3) = aLic(3) 'Compras
        aGestionVisible(4) = aLic(4) 'Ventas
        aGestionVisible(5) = aLic(5) 'Puntos de ventas
        aGestionVisible(6) = aLic(6) 'Mercancías
        aGestionVisible(7) = aLic(7) 'Medición Gerencial
        aGestionVisible(8) = aLic(8) 'Producción
        aGestionVisible(9) = aLic(9) 'Control de gestiones

        Dim fCont As Integer
        For fCont = 0 To 9
            C1Ribbon1.Tabs(fCont).Visible = aGestionVisible(fCont)
        Next

        HiddingValidation()
        HiddingMenuItems_DesdeMenuUsuario()
        HiddingMenuItems_DesdeTipoAplicacion(aItems)



    End Sub

    Private Sub HiddingValidation()
        If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) > 1 Then
            RibbonSeparator71.Visible = True
            RibbonButton307.Visible = True
        End If
    End Sub

    Private Sub HiddingMenuItems_DesdeTipoAplicacion(aItems() As String)

        For Each strItem As String In aItems
            HiddingMenuItems(strItem)
        Next

    End Sub
    Private Sub HiddingMenuItems_DesdeMenuUsuario()

        Dim dtBloqueos As DataTable
        Dim tblBloqueos As String = "tblblq"
        Dim nMapa As String = ft.DevuelveScalarCadena(myConn, " select mapa from jsconctausu where id_user = '" + jytsistema.sUsuario + "' ")

        ds = DataSetRequery(ds, "SELECT a.region, a.acceso FROM jsconrenglonesmapa a LEFT JOIN jsconctausu b ON (a.mapa = b.mapa) WHERE " _
                             & " b.id_user = '" & jytsistema.sUsuario & "' AND " _
                             & " a.acceso = 0 AND " _
                             & " a.region <> '' ORDER BY gestion, modulo, region", myConn, tblBloqueos, lblInfo)

        dtBloqueos = ds.Tables(tblBloqueos)

        Dim lCont As Integer
        If dtBloqueos.Rows.Count > 0 Then
            For lCont = 0 To dtBloqueos.Rows.Count - 1
                Dim r As String = dtBloqueos.Rows(lCont).Item("region").ToString
                HiddingMenuItems(r)
            Next
        End If

        dtBloqueos.Dispose()
        dtBloqueos = Nothing

    End Sub
    Private Sub HiddingMenuItems(r As String)
        Dim oTab As RibbonTab
        For Each oTab In C1Ribbon1.Tabs
            If oTab.ID = r Then oTab.Visible = False
            Dim oGrp As RibbonGroup
            For Each oGrp In oTab.Groups
                If oGrp.ID = r Then oGrp.Visible = False
                RecorrerYVisualizarRibbonItems(oGrp.Items, r)

            Next
        Next
    End Sub
    Private Sub RecorrerYVisualizarRibbonItems(ByVal pControl As RibbonItemCollection,
                                  ByVal r As String)

        Dim vControl As RibbonItem
        For Each vControl In pControl
            If vControl.ID = r Then vControl.Visible = False
            If TypeOf vControl Is RibbonMenu Then
                Dim rr As RibbonMenu = vControl
                RecorrerYVisualizarRibbonItems(rr.Items, r)
            End If
        Next

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick

        Dim fnt As Font = New Font("Arial", 8, FontStyle.Bold)

        tsLNFecha.Font = fnt
        tsLNFecha.Text = "  Fecha : "
        tsLFecha.Text = Format(jytsistema.sFechadeTrabajo, "D")
        tsLNEmpresa.Font = fnt
        tsLNEmpresa.Text = "  Empresa : "
        tsLEmpresa.Text = jytsistema.WorkName
        tslNUsuario.Font = fnt
        tslNUsuario.Text = "  Usuario : "
        tslUsuario.Text = jytsistema.sUsuario & " " & jytsistema.sNombreUsuario

        tsLFecha.TextAlign = ContentAlignment.MiddleLeft
        tsLEmpresa.TextAlign = ContentAlignment.MiddleLeft
        tslUsuario.TextAlign = ContentAlignment.MiddleLeft
        tsLNFecha.TextAlign = ContentAlignment.MiddleLeft
        tsLNEmpresa.TextAlign = ContentAlignment.MiddleLeft
        tslNUsuario.TextAlign = ContentAlignment.MiddleLeft

        Dim activeChild As New Form
        activeChild = Me.ActiveMdiChild
        If activeChild Is Nothing Then
            tsLNWin.Text = ""
            tsLWin.Text = ""
        Else
            tsLNWin.Font = fnt
            tsLNWin.Text = "  Ventana activa : "
            tsLWin.Text = activeChild.Tag
        End If
        activeChild = Nothing

        If jytsistema.WorkExercise = "" Then
            tslNEjercicio.Text = ""
            tslEjercicio.Text = ""
        Else
            tslNEjercicio.Font = fnt
            tslNEjercicio.Text = "  Ejercicio : "
            Dim aFld() As String = {"ejercicio", "id_emp"}
            Dim aStr() As String = {jytsistema.WorkExercise, jytsistema.WorkID}
            tslEjercicio.Text = jytsistema.WorkExercise & " | " & ft.FormatoFecha(CDate(qFoundAndSign(myConn, lblInfo, "jsconctaeje", aFld, aStr, "inicio").ToString)) &
                        " | " & ft.FormatoFecha(CDate(qFoundAndSign(myConn, lblInfo, "jsconctaeje", aFld, aStr, "cierre").ToString))
        End If

        With StatusStrip
            .Items.Clear()
            .Items.Add(tsLNFecha)
            .Items.Add(tsLFecha)
            .Items.Add(tsLNEmpresa)
            .Items.Add(tsLEmpresa)
            .Items.Add(tslNUsuario)
            .Items.Add(tslUsuario)
            .Items.Add(tsLNWin)
            .Items.Add(tsLWin)
            .Items.Add(tslNEjercicio)
            .Items.Add(tslEjercicio)
        End With

    End Sub

    Private Sub CreateRecentDocumentList()
        C1Ribbon1.ApplicationMenu.RightPaneItems.Clear()

        Dim listItem As RibbonListItem = New RibbonListItem(New RibbonLabel("Módulos abiertos"))
        listItem.AllowSelection = False
        C1Ribbon1.ApplicationMenu.RightPaneItems.Add(listItem)
        C1Ribbon1.ApplicationMenu.RightPaneItems.Add(New RibbonListItem(New RibbonSeparator()))

        Dim oForm As Form
        For Each oForm In Me.MdiChildren
            ' each item consists of the name of the document and a push pin
            listItem = New RibbonListItem()
            Dim rl As New RibbonLabel
            rl.ID = oForm.Name
            rl.Text = oForm.Tag

            'listItem.Items.Add(New RibbonLabel(oForm.Tag))
            listItem.Items.Add(rl)

            AddHandler listItem.Click, AddressOf ListItemButton_Click

            ' allow the button to be selectable so we can toggle it
            'Dim pin As RibbonToggleButton = New RibbonToggleButton()
            'pin.SmallImage = My.Resources.unpinned
            'pin.AllowSelection = True
            'pin.Tag = oForm.Name
            'listItem.Items.Add(pin)
            'AddHandler pin.Click, AddressOf pinButton_Click

            C1Ribbon1.ApplicationMenu.RightPaneItems.Add(listItem)

        Next
    End Sub
    Private Sub ListItemButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim li As RibbonListItem = CType(sender, RibbonListItem)
        'Dim pin As RibbonToggleButton = CType(sender, RibbonToggleButton)
        'If pin.Pressed Then
        ' pin.SmallImage = My.Resources.pinned
        ' Else
        ' pin.SmallImage = My.Resources.unpinned
        ' End If
        MsgBox(li.ID)
    End Sub

#Region "Menu Contabilidad"
#Region "Archivos Contabilidad"
    '////////////////////////////////////////
    'CONTABILIDAD - ARCHIVOS
    Private Sub RibbonButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton1.Click
        'CUENTAS CONTABLES 
        Dim f As New jsContabArcCuentas
        OpenChildForm(f)
    End Sub
    Private Sub RibbonButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton2.Click
        Dim f As New jsContabArcAsientos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, Asiento.iDiferido)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton3.Click
        Dim gg As New jsContabArcAsientos
        If ChildFormOpen(Me, gg) Then
            gg.Activate()
        Else
            gg.MdiParent = Me
            gg.Cargar(myConn, Asiento.iActual)
        End If
        gg = Nothing
    End Sub
    Private Sub RibbonButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton5.Click
        'Activos Fijos  
        Dim f As New jsContabArcActivosFijos
        OpenChildForm(f)

    End Sub
    Private Sub RibbonButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton7.Click
        Dim f As New jsContabArcAsientosPlantilla
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton8.Click
        'Conceptos
        Dim f As New jsContabArcReglas
        OpenChildForm(f)
    End Sub
#End Region
#Region "Proceso Contabilidad"
    '////////////////////////////////////////
    'CONTABILIDAD - PROCESOS
    '
    Private Sub RibbonButton11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton11.Click
        Dim f As New jsContabProProcesaAsientos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton299_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton299.Click
        Dim f As New jsContabProReversaraAsientos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton12.Click
        Dim f As New jsContabProContabilizar
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton305_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton305.Click
        Dim f As New jsContabProProcesarCuentas
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton292_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton292.Click
        Dim f As New jsContabProApertura
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
#End Region
#Region "REportes Contabilidad"
    '////////////////////////////////////////
    'CONTABILIDAD - REPORTES 
    '
    Private Sub RibbonButton13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton13.Click
        Dim f As New jsContabRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteContabilidad.cPolizasActuales, "Asientos Contables")
        f = Nothing
    End Sub
    Private Sub RibbonButton14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton14.Click
        Dim f As New jsContabRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteContabilidad.cMayorAnalitico, "Mayor Analítico")
        f = Nothing
    End Sub
    Private Sub RibbonButton15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton15.Click
        Dim f As New jsContabRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteContabilidad.cBalanceComprobacion, "Balance de comprobación ó Mayor general")
        f = Nothing
    End Sub
    Private Sub RibbonButton16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton16.Click
        Dim f As New jsContabRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteContabilidad.cDiario, "Libro Diario")
        f = Nothing
    End Sub
    Private Sub RibbonButton17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton17.Click
        Dim f As New jsContabRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteContabilidad.cBalanceGeneral, "Balance General")
        f = Nothing
    End Sub
    Private Sub RibbonButton18_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton18.Click
        Dim f As New jsContabRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteContabilidad.cGananciasPerdidas, "Estado General de Ganancias y Pérdidas")
        f = Nothing
    End Sub
    Private Sub RibbonButton109_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton109.Click
        Dim f As New jsContabRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteContabilidad.cReglasContabilizacion, "Reglas de Contabilización")
        f = Nothing
    End Sub
#End Region
#End Region
#Region "Menu Bancos"
#Region "Archivos Bancos"
    '////////////////////////////////////////
    'BANCOS Y CAJAS - ARCHIVOS 
    '
    Private Sub RibbonButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton9.Click
        'Bancos 
        Dim f As New jsBanArcBancos
        OpenChildForm(f)
    End Sub
    Private Sub RibbonButton10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton10.Click
        'Cajas
        Dim f As New jsBanArcCajas
        OpenChildForm(f)
    End Sub
#End Region
#Region "Procesos Bancos"
    '////////////////////////////////////////
    'BANCOS Y CAJAS - PROCESOS
    '
    Private Sub RibbonButton31_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton31.Click
        'Conciliaciones bancarias
        Dim f As New jsBanProConciliacionPlus
        f.Cargar(myConn)
        f = Nothing
    End Sub
    Private Sub RibbonButton194_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton194.Click
        'Cálculo débito bancario
        Dim f As New jsBanProDebitoBancario
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, True)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton195_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton195.Click
        'Reverso débito bancario.
        Dim f As New jsBanProDebitoBancario
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, False)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton51_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton51.Click
        'transferencias bancarias
        Dim f As New jsBanProTransferencias
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton52_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton52.Click
        'Devolución de cheques de clientes
        Dim f As New jsBanProChequeDevuelto
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton53_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton53.Click,
        RibbonButton191.Click, RibbonButton192.Click
        'Devolución de cheques de alimentación 
        Dim f As New jsBanProCestaTicketDevuelto
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, IIf(sender.tag = "key402006", 0, IIf(sender.tag = "key402007", 1, 2)))
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton196_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton196.Click
        'ACTUALIZACION DE NUMEROS PARA DEPOSITOS TEMPORALES
        Dim f As New jsBanProCambioNumDeposito
        f.Cargar(myConn)
        f = Nothing
    End Sub
#End Region
#Region "Reportes Bancos"
    '////////////////////////////////////////
    'BANCOS Y CAJAS - REPORTES
    '
    Private Sub RibbonButton19_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton19.Click
        'BANCOS - DISPONIBILIDAD
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cDisponibilidad, "Cuentas Bancarias y Disponibilidad")
        f = Nothing
    End Sub
    Private Sub RibbonButton20_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton20.Click
        'BANCOS - CONCILIACION BANCARIA
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cConciliacion, "Conciliación Bancaria")
        f = Nothing
    End Sub
    Private Sub RibbonButton21_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton21.Click
        'BANCOS - SALDOS POR MES 
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cSaldosMensuales, "Saldos de bancos mes a mes")
        f = Nothing
    End Sub
    Private Sub RibbonButton22_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton22.Click
        'BANCOS - ESTADO DE CUENTA
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cEstadoCuenta, "Estado de cuenta bancario")
        f = Nothing
    End Sub
    Private Sub RibbonButton188_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton188.Click
        'BANCOS - IMPUESTO AL DEBITO BANCARIO 
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cIDB, "Impuesto al Débito Bancario")
        f = Nothing
    End Sub
    Private Sub RibbonButton189_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton189.Click
        'BANCOS - IMPUESTO AL DEBITO BANCARIO mes a mes
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cIDBMes, "Impuesto al Débito Bancario mes a mes")
        f = Nothing
    End Sub
    Private Sub RibbonButton24_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton24.Click
        'BANCOS - DEPOSITOS DE CAJAS 
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cDepositos, "Depósitos de caja")
        f = Nothing
    End Sub
    Private Sub RibbonButton25_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton25.Click
        'BANCOS - Arqueos de cajas 
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cArqueoCajas, "Arqueos de cajas")
        f = Nothing
    End Sub
    Private Sub RibbonButton26_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton26.Click
        'BANCOS - LISTADO DE BANCOS
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cListadoBancos, "Listado de Bancos")
        f = Nothing
    End Sub
    Private Sub RibbonButton27_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton27.Click
        'BANCOS - cheques dewvueltos bancos
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cChequesDevueltos, "Cheques Devueltos")
        f = Nothing
    End Sub
    Private Sub RibbonButton28_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton28.Click
        'BANCOS - remesa/sobres de cheques de alimentación
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cRemesasTickets, "Remesas/Sobres de cheques de alimentación")
        f = Nothing
    End Sub
    Private Sub RibbonButton29_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton29.Click
        'BANCOS - Movimientos no depositados
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cMovimientosPostDatados, "Movimientos de caja no depositados")
        f = Nothing
    End Sub
    Private Sub RibbonButton193_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton193.Click
        'BANCOS - Reposición de caja chica
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cResumenReposicionSaldoCaja, "RESUMEN Reposición de saldo en caja")
        f = Nothing
    End Sub
#End Region
#End Region
#Region "Menu Recursos Humanos"
#Region "Archivos Recursos Humanos"
    '////////////////////////////////////////
    'RECURSOS HUMANOS - ARCHIVOS 
    '
    Private Sub RibbonButton32_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton32.Click
        'Trabajadores  
        Dim f As New jsNomArcTrabajadores
        OpenChildForm(f)
    End Sub
    Private Sub RibbonButton33_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton33.Click
        'Grupos Trabajadores  
        Dim f As New jsControlArcTablaSimple
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar("Grupos de Trabajadores", FormatoTablaSimple(Modulo.iGrupoNom), True, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton34_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton34.Click
        Dim f As New jsNomArcCargos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton38_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton38.Click
        Dim f As New jsNomArcTurnosHorarios
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    'FACTURAS
    Private Sub RibbonButton298_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton298.Click

        Dim f As New jsNomArcNominas
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing

    End Sub

    Private Sub RibbonButton35_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton35.Click
        'Trabajadores por concepto
        Dim f As New jsNomArcTrabajadoresXConcepto
        OpenChildForm(f)
    End Sub

    Private Sub RibbonButton39_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton39.Click
        'Conceptos
        Dim f As New jsNomArcConceptos
        OpenChildForm(f)
    End Sub
    Private Sub RibbonButton40_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton40.Click
        'Constannes
        Dim f As New jsNomArcConstantes
        OpenChildForm(f)
    End Sub
#End Region
#Region "Procesos Recursos Humanos"
    '////////////////////////////////////////
    'RECURSOS HUMANOS - PROCESOS
    '
    Private Sub RibbonButton200_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton200.Click
        'PROCESAR NOMINA
        Dim f As New jsNomProProcesarNomina
        f.Cargar(myConn)
        f = Nothing
    End Sub
    Private Sub RibbonButton201_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton201.Click
        'REVERSAR NOMINA
        Dim f As New jsNomProReversarNomina
        f.Cargar(myConn)
        f = Nothing
    End Sub
    Private Sub RibbonButton231_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton231.Click
        'REPROCESAR CONCEPTOS
        Dim f As New jsNomProReProcesarConceptos
        f.Cargar(myConn)
        f.Dispose()
        f = Nothing
    End Sub
#End Region
#Region "Reportes Recursos Humanos"
    '////////////////////////////////////////
    'RECURSOS HUMANOS - REPORTES
    '
    Private Sub RibbonButton41_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton41.Click
        'RECURSOS HUMANOS  - Trabajadores
        Dim f As New jsNomRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cTrabajadores, "Trabajadores")
        f = Nothing
    End Sub
    Private Sub RibbonButton42_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton42.Click
        'RECURSOS HUMANOS  - Trabajadores
        Dim f As New jsNomRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cRegistroTrabajador, "Planilla registro de trabajador", "XXX")
        f = Nothing
    End Sub
    Private Sub RibbonButton43_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton43.Click
        'RECURSOS HUMANOS  - Conceptos de Nómina
        Dim f As New jsNomRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cConceptosXTrabajadores, "Conceptos y Trabajadores")
        f = Nothing
    End Sub

    Private Sub RibbonButton44_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton44.Click

        'RECURSOS HUMANOS  - Prenómina
        Dim f As New jsNomRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cPrenomina, "Pre-Nómina")
        f = Nothing
    End Sub
    Private Sub RibbonButton45_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton45.Click
        'RECURSOS HUMANOS  - Resumen nómina
        Dim f As New jsNomRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cResumenXConcepto, "Resumen de nómina por concepto")
        f = Nothing
    End Sub
    Private Sub RibbonButton46_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton46.Click
        'RECURSOS HUMANOS  - Nómina
        Dim f As New jsNomRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cNomina, "Nómina")
        f = Nothing
    End Sub
    Private Sub RibbonButton47_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton47.Click
        'RECURSOS HUMANOS  - Recibos Nómina
        Dim f As New jsNomRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cRecibos, "Recibos de Nómina")
        f = Nothing
    End Sub
    Private Sub RibbonButton48_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton48.Click
        'RECURSOS HUMANOS  - Listado de firmas de Nómina
        Dim f As New jsNomRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cFirmas, "Listado de firmas de Nómina")
        f = Nothing
    End Sub
    Private Sub RibbonButton49_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton49.Click
        'RECURSOS HUMANOS  - Acumulados por concepto
        Dim f As New jsNomRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cAcumuladosPorConcepto, "Acumulados por concepto")
        f = Nothing
    End Sub
    Private Sub RibbonButton50_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton50.Click
        'RECURSOS HUMANOS  - Conceptos por trabajador mes a mes
        Dim f As New jsNomRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cConceptosXTrabajadorMesAMes, "Conceptos por trabajador Mes A Mes")
        f = Nothing
    End Sub
    Private Sub RibbonButton242_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton242.Click
        'RECURSOS HUMANOS  - Conceptos por trabajador mes a mes
        Dim f As New jsNomRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cConceptosMesAMes, "Conceptos Mes A Mes")
        f = Nothing
    End Sub
#End Region
#End Region
#Region "Menu Compras y Cuentas por pagar"
#Region "Archivos compras"
    '////////////////////////////////////////
    'COMPRAS
    '//////////////////////////////////////// 
    '---- ARCHIVOS
    '---- ARCHIVOS ---- Proveedores
    Private Sub RibbonButton55_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton55.Click
        Dim f As New jsComArcProveedores
        OpenChildForm(f)
    End Sub
    Private Sub RibbonButton56_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton56.Click
        Dim f As New jsComArcUnidadCategoria
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton57_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton57.Click
        Dim f As New jsControlArcTablaSimple
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar("Zonas de proveedores", FormatoTablaSimple(Modulo.iZonaProveedor), True, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton58_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton58.Click
        'GASTOS
        Dim f As New jsComArcGastos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton59_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton59.Click
        'ORDENES DE COMPRA
        Dim f As New jsComArcOrdenesDeCompra
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton60_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton60.Click
        'RECEPCIONES DE MERCANCIAS
        Dim f As New jsComArcRecepciones
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton61_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton61.Click
        'COMPRAS
        Dim f As New jsComArcCompras
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton62_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton62.Click
        'NOTAS CREDITO
        Dim f As New jsComArcNotasCredito
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton63_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton63.Click
        'NOTAS DEBITO
        Dim f As New jsComArcNotasDebito
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
#End Region
#Region "Procesos compras"
    '//////////////// PROCESOS COMPRAS
    Private Sub RibbonButton236_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton236.Click
        Dim f As New jsComProProgramacionPago
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton237_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton237.Click
        Dim f As New jsComProRetencionesIVA
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton238_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton238.Click
        Dim f As New jsComProRetencionesISLR
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton239_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton239.Click
        Dim f As New jsGenProNumerosControl
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, Gestion.iCompras)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton240_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton240.Click
        Dim f As New jsComProHistoricoProveedores
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, iProceso.Procesar)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton241_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton241.Click
        Dim f As New jsComProHistoricoProveedores
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, iProceso.Reversar)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton225_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton225.Click
        Dim f As New jsComProReconstruccionDeSaldos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton286_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton286.Click
        Dim f As New jsComProComprasInventario
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, iProceso.Procesar)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton287_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton287.Click
        Dim f As New jsComProComprasInventarioR
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, iProceso.Procesar)
        End If
        f = Nothing
    End Sub
#End Region
#Region "Reportes Compras"

    '------- REPORTES COMPRAS
    'REPORTES COMPRAS - PROVEEDORES
    Private Sub RibbonButton64_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton64.Click
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cProveedores, "Proveedores")
        f = Nothing
    End Sub

    'REPORTES COMPRAS - MOVIMIENTOS DE PROVEEDORES
    Private Sub RibbonButton288_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton288.Click
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cMovimientosProveedores, "Movimientos Proveedores")
        f = Nothing
    End Sub

    'REPORTES COMPRAS - ESTADO DE CUENTA PROVEEDORES
    Private Sub RibbonButton66_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton66.Click
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cEstadodeCuentasProveedores, "Estado de Cuentas de proveedores")
        f = Nothing
    End Sub

    'REPORTES COMPRAS  - SALDOS PROVEEDORES
    Private Sub RibbonButton67_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton67.Click
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cSaldosProveedores, "Saldos de proveedores")
        f = Nothing
    End Sub

    'REPORTES COMPRAS  - AUDITORIAS DE PROVEEDORES
    Private Sub RibbonButton68_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton68.Click
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cAuditoriasProveedores, "Auditorías de proveedores")
        f = Nothing
    End Sub

    'REPORTES COMPRAS  - VENCIMIENTOS DOCUMENTOS DE PROVEEDORES
    Private Sub RibbonButton69_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton69.Click

        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cVencimientos, "Vencimientos a una fecha")
        f = Nothing
    End Sub

    'REPORTES COMPRAS  - VENCIMIENTOS DOCUMENTOS DE PROVEEDORES
    Private Sub RibbonButton247_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton247.Click
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cVencimientosResumen, "Vencimientos a una fecha (Resumen)")
        f = Nothing
    End Sub

    'REPORTES COMPRAS - LIBRO IVA COMPRAS
    Private Sub RibbonButton70_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton70.Click
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cLibroIVA, "Libro de Compras- Impuesto al Valor Agregado")
        f = Nothing
    End Sub

    'REPORTES COMPRAS - LIstado de retenciones de iva
    Private Sub RibbonButton248_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton248.Click
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cListaRetencionesIVA, "Listado de retenciones de IVA")
        f = Nothing
    End Sub

    'REPORTES COMPRAS - LIstado de retenciones de islr
    Private Sub RibbonButton249_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton249.Click
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cListaRetencionesISLR, "Listado de retenciones de ISLR")
        f = Nothing
    End Sub

    Private Sub RibbonButton71_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton71.Click
        'REPORTES COMPRAS  - LISTADO ORDENES DE COMPRA
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cListadoOrdenesDeCompra, "Ordenes de Compra")
        f = Nothing
    End Sub
    Private Sub RibbonButton72_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton72.Click
        'REPORTES COMPRAS  - LISTADO RECEPCIONES DE MERCANCIAS
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cListadoRecepciones, "Recepciones de mercancías")
        f = Nothing
    End Sub
    Private Sub RibbonButton73_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton73.Click
        'REPORTES COMPRAS  - LISTADO COMPRAS
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cListadoCompras, "Compras")
        f = Nothing
    End Sub
    Private Sub RibbonButton74_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton74.Click
        'REPORTES COMPRAS  - LISTADO notas credito
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cListadoNotasCredito, "Notas de Crédito")
        f = Nothing
    End Sub
    Private Sub RibbonButton75_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton75.Click
        'REPORTES COMPRAS  - LISTADO Notas Débito
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cListadoNotasDebito, "Notas Débito")
        f = Nothing
    End Sub
    Private Sub RibbonButton245_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton245.Click
        'REPORTES COMPRAS  - LISTADO Gastos
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cListadoGastos, "Gastos")
        f = Nothing
    End Sub
    Private Sub RibbonButton277_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton277.Click
        'REPORTES COMPRAS  - LISTADO COMPRAS/GASTOS/NOTAS CREDITO SIN RETENCION IVA
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cListadoDocumentosSinRetencionIVA, "Compras/Gastos/Notas Crédito sin retención IVA")
        f = Nothing
    End Sub
#End Region
#End Region
#Region "Menu Ventas y Cuentas por cobrar"
#Region "Archivos Ventas y Cuentas por cobrar"
    '/////////////7// VENTAS
    '---- ARCHIVOS VENTAS
    Private Sub RibbonButton30_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton30.Click
        'CLIENTES
        Dim f As New jsVenArcClientes
        OpenChildForm(f)
    End Sub
    Private Sub RibbonButton76_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton76.Click
        Dim f As New jsVenArcCanalTiponegocio
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton77_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton77.Click
        Dim f As New jsControlArcTablaSimple
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar("Zonas de clientes", FormatoTablaSimple(Modulo.iZonasClientes), True, TipoCargaFormulario.iShow)

        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton78_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton78.Click
        'Rutas de Visita
        Dim f As New jsVenArcRutasVisita
        OpenChildForm(f)
        CreateRecentDocumentList()
    End Sub
    Private Sub RibbonButton79_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton79.Click
        'Rutas de Visita
        Dim f As New jsVenArcRutasDespacho
        OpenChildForm(f)
        CreateRecentDocumentList()
    End Sub
    Private Sub RibbonButton80_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton80.Click
        'Asesores Comerciales
        Dim f As New jsVenArcAsesores
        OpenChildForm(f)
    End Sub
    Private Sub RibbonButton81_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton81.Click
        'PRESUPUESTOS
        Dim f As New jsVenArcPresupuestos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton214_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton214.Click
        'PRE-PEDIDOS
        Dim f As New jsVenArcPrePedidos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton82_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton82.Click
        'PEDIDOS
        Dim f As New jsVenArcPedidos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton84_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton84.Click
        'NOTAS DE ENTREGA
        Dim f As New jsVenArcNotasEntrega
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton85_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton85.Click
        'FACTURAS
        Dim f As New jsVenArcFacturas
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton86_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton86.Click
        'NOTAS DE CREDITO
        Dim f As New jsVenArcNotasCredito
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton87_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton87.Click
        Dim f As New jsVenArcNotasDebito
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
#End Region
#Region "Procesos Ventas y Cuentas por cobrar"
    '---- PROCESOS VENTAS
    Private Sub RibbonButton215_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton215.Click
        Dim f As New jsVenProReconstruccionDeSaldos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton216_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton216.Click
        Dim f As New jsVenProBloqueoDeClientes
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton217_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton217.Click
        Dim f As New jsVenProPrepedidosPedidos
        f.Cargar(myConn)
        f = Nothing
    End Sub
    Private Sub RibbonButton218_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton218.Click
        Dim f As New jsVenProPedidosFacturacion
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub

    Private Sub RibbonButton219_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton219.Click
        Dim f As New jsVenProGuiaDespacho
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton220_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton220.Click
        Dim f As New jsVenProRelacionFacturas
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton221_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton221.Click
        Dim f As New jsVenProRelacionNotasCredito
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton250_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton250.Click
        Dim f As New jsVenProCuotasAsesores
        f.Cargar(myConn)
        f.Dispose()
        f = Nothing
    End Sub



    Private Sub RibbonButton226_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton226.Click
        Dim f As New jsVenProLimiteCreditoClientes
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton227_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton227.Click
        Dim f As New jsVenProAnulacionDocumentos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub

    Private Sub RibbonButton228_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton228.Click
        Dim f As New jsGenProNumerosControl
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, Gestion.iVentas)
        End If
        f = Nothing
    End Sub

    Private Sub RibbonButton232_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton232.Click
        Dim f As New jsControlProConsecutivos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub

    Private Sub RibbonButton235_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton235.Click
        Dim f As New jsVenProPreCancelaciones
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub

    Private Sub RibbonButton222_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton222.Click
        Dim f As New jsVenProCierreDiario
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow, jytsistema.sFechadeTrabajo)
        End If
        f = Nothing
    End Sub

    Private Sub RibbonButton223_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton223.Click
        Dim f As New jsVenProCierreMensual
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub

    Private Sub RibbonButton268_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton268.Click
        jytsistema.WorkBox = Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveImpresorFiscal, "")
        'If jytsistema.WorkBox <> "" Then
        '    Dim TipoImpresora As Integer = TipoImpresoraFiscal(myConn, jytsistema.WorkBox)
        '    EsperateUnPoquito(myConn)
        '    Select Case TipoImpresora
        '        Case 0
        '            ft.mensajeInformativo("Imprime factura...")
        '        Case 1 ' Factura Fiscal preimpresa
        '            ft.mensajeInformativo("Imprimienso Factura fiscal")
        '        Case 2, 5, 6 ' Impresora Tipo Aclas (TFHKAIF.DLL)
        '            ReporteXFiscalPP1F3(myConn, lblInfo)
        '        Case 3 ' Impresora Tipo Bematech (BEMAFI32.DLL)
        '            'ReporteXFiscalBematech()
        '        Case 4 'Impresora Fiscal Epson/PnP
        '            ReporteXFiscalPnP(myConn, lblInfo)
        '    End Select
        '    MsgBox("REPORTE X ENVIADO...", MsgBoxStyle.Information)
        'Else
        '    ft.mensajeCritico("DEBE TENER CONFIGURADA UNA IMPRESORA FISCAL...")
        'End If

        'Dim f As New jsPOSRepParametros
        'f.Cargar(TipoCargaFormulario.iShowDialog, ReportePuntoDeVenta.cReporteX, "REPORTE X", jytsistema.WorkBox)
        'f = Nothing

    End Sub
    Private Sub RibbonButton269_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton269.Click
        jytsistema.WorkBox = Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveImpresorFiscal, "")
        'If jytsistema.WorkBox <> "" Then
        '    Dim TipoImpresora As Integer = TipoImpresoraFiscal(myConn, jytsistema.WorkBox)
        '    EsperateUnPoquito(myConn)
        '    Select Case TipoImpresora
        '        Case 0
        '            ft.mensajeInformativo("Imprime factura...")
        '        Case 1 ' Factura Fiscal preimpresa
        '            ft.mensajeInformativo("IMPRIMIENDO Factura fiscal")
        '        Case 2, 5, 6 ' Impresora Tipo Aclas (TFHKAIF.DLL)
        '            ReporteZFiscalPP1F3(myConn, lblInfo)
        '        Case 3 ' Impresora Tipo Bematech (BEMAFI32.DLL)
        '            'ReporteXFiscalBematech()
        '        Case 4 'Impresora Fiscal Epson/PnP
        '            ReporteZFiscalPnP(myConn, lblInfo)
        '    End Select
        '    MsgBox("REPORTE Z ENVIADO...", MsgBoxStyle.Information)
        'Else
        '    ft.mensajeCritico("DEBE TENER CONFIGURADA UNA IMPRESORA FISCAL...")
        'End If
    End Sub
    Private Sub RibbonButton272_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton272.Click
        Dim f As New jsVenProMercanciasEnDespachos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton273_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton273.Click
        Dim f As New jsVenProGuiaPedidos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
#End Region
#Region "Reportes Ventas y Cuentas por cobrar"
    '---- REPORTES VENTAS
    'REPORTES VENTAS  - CLIENTES
    Private Sub RibbonButton88_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton88.Click

        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cClientes, "Clientes")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - FICHA DE CLIENTES
    Private Sub RibbonButton89_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton89.Click

        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cCliente, "Cliente")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - PLANILLA INSCRIPCION CLIENTES
    Private Sub RibbonButton90_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton90.Click

        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cPlanillaCliente, "Planilla para Cliente", "xxxxxx")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - MOVIMIENTOS DE CLIENTES
    Private Sub RibbonButton116_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton116.Click

        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cMovimientosClientes, "Movimientos de Clientes")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - MOVIMIENTOS DE documentos
    Private Sub RibbonButton117_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton117.Click

        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cMovimientosDocumento, "Movimientos de Documentos CxC")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - ESTADO DE CUENTAS
    Private Sub RibbonButton94_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton94.Click

        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cEstadoDeCuentas, "Estado de Cuentas")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - SALDOS A UNA FECHA
    Private Sub RibbonButton95_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton95.Click

        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cSaldos, "Saldos a una Fecha")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - AUDITORIAS
    Private Sub RibbonButton96_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton96.Click

        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cAuditoriaClientes, "Auditorías de Clientes")
        f = Nothing
    End Sub

    'REPORTES(VENTAS - VENCIMIENTOS)
    Private Sub RibbonButton97_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton97.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cVencimientos, "Vencimientos a una Fecha")
        f = Nothing
    End Sub

    'REPORTES (VENTAS - LIBRO IVA VENTAS)
    Private Sub RibbonButton98_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton98.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cLibroIVA, "Libro de Ventas - Impuesto al Valor Agregado")
        f = Nothing
    End Sub

    'REPORTES (VENTAS - RETENCIONES IVA)
    Private Sub RibbonButton276_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton276.Click
        Dim f As New jsVenRepParametrosDos
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cRetencionesIVAClientes, "Retenciones de IVA Clientes")
        f = Nothing
    End Sub

    'REPORTES(VENTAS - VENCIMIENTOS - resumen)
    Private Sub RibbonButton246_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton246.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cVencimientosResumen, "Vencimientos a una Fecha (Resumen)")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - Presupuestos
    Private Sub RibbonButton99_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton99.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cListadoPresupuestos, "Presupuestos")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - PREPEDIDOS
    Private Sub RibbonButton229_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton229.Click

        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cListadoPrePedidos, "Pre-Pedidos")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - Pedidos
    Private Sub RibbonButton100_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton100.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cListadoPedidos, "Pedidos")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - NOTAS DE ENTREGA
    Private Sub RibbonButton102_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton102.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cListadoNotasDeEntrega, "Notas De Entrega")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - FACTURACION
    Private Sub RibbonButton103_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton103.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cListadoFacturas, "Facturación")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - NOTAS DE CREDITO
    Private Sub RibbonButton104_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton104.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cListadoNotasDeCredito, "Notas de Crédito")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - NOTAS DEBITO
    Private Sub RibbonButton105_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton105.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cListadoNotasDebito, "Notas Débito")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - PEDIDOS SIN FACTURAR
    Private Sub RibbonButton106_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton106.Click

        Dim f As New jsVenRepParametrosDos
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cPedidosSinFacturar, "PEDIDOS SIN FACTURAR")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - CHEQUES DEVUELTOS
    Private Sub RibbonButton107_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton107.Click

        'Dim f As New jsVenRepParametrosDos
        'f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cChequesDevueltos, "CHEQUES DEVUELTOS")
        'f = Nothing
        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cChequesDevueltos, "CHEQUES DEVUELTOS")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - CHEQUES DEVUELTOS MES a MES
    Private Sub RibbonButton108_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton108.Click
        Dim f As New jsVenRepParametrosDos
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cChequesDevueltosMes, "CHEQUES DEVUELTOS MES a MES")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - ACTIVACIONES CLIENTES MERCANCIAS
    Private Sub RibbonButton118_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton118.Click
        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cActivacionesClientesMercas, "ACTIVACION DE CLIENTES Y MERCANCIAS")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - ACTIVACION CLIENTES
    Private Sub RibbonButton119_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton119.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cActivacionesClientes, "Activaciones Clientes")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - VENTAS Y ACTIVACION DE CLIENTES -------------> OJOJOJOJO ----- > DESHABILITADO
    Private Sub RibbonButton121_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton121.Click
        Dim f As New jsVenRepParametrosDos
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cVentasPorCliente, "Ventas por Clientes y activación de clientes")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - VENTAS Y ACTIVACION DE CLIENTES PLUS
    Private Sub RibbonButton275_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton275.Click
        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cVentasPorClientesPlus, "VENTAS CLIENTES Y ACTIVACION (Resumen)")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - POR CLIENTES Y DIVISION
    Private Sub RibbonButton122_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton122.Click
        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cVentasClienteDivision, "Ventas por Clientes y División")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - POR CLIENTES MES MES
    Private Sub RibbonButton123_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton123.Click
        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cVentasPorClienteMesMes, "Ventas por Clientes Mes a Mes")
        f = Nothing
    End Sub


    'REPORTES VENTAS  - COBRANZA OCURRIDA EN EL MES DE MESES ANTERIORES
    Private Sub RibbonButton124_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton124.Click
        Dim f As New jsVenRepParametrosDos
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cCobranzaAnterior, "Cobranza ocurrida en el mes sobre meses anteriores")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - COBRANZA OCURRIDA EN EL MES DEL MES ACTUAL
    Private Sub RibbonButton125_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton125.Click
        Dim f As New jsVenRepParametrosDos
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cCobranzaActual, "Cobranza ocurrida en el mes sobre el mes actual")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - CIERRE FINANCIERO DIARIO POR ASESOR
    Private Sub RibbonButton126_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton126.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cCierreDiario, "Cierre Financiero por Asesor (CREDITO)")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - CIERRE FINANCIERO DIARIO POR ASESOR (CONTADO)
    Private Sub RibbonButton274_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton274.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cCierreDiarioContado, "Cierre Financiero por Asesor (CONTADO)")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - CIERRE DIARIO cesta tickets POR ASESOR
    Private Sub RibbonButton127_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton127.Click
        Dim f As New jsVenRepParametrosDos
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cCierreDiarioCT, "Cierre diario porcesta tickets y por Asesor")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - PRESUPUESTOS MES A MES
    Private Sub RibbonButton128_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton128.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cPresupuestosMesMes, "Presupuestos mes a mes")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - PREPEDIDOS MES A MES
    Private Sub RibbonButton230_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton230.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cPrepedidosMesMes, "Prepedidos mes a mes")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - PEDIDOS MES A MES
    Private Sub RibbonButton129_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton129.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cPedidosMesMes, "Pedidos mes a mes")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - NOTAS ENTREGA MES A MES
    Private Sub RibbonButton131_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton131.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cNotasEntregaMesMes, "Notas de Entrega mes a mes")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - FACTURAS MES A MES
    Private Sub RibbonButton132_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton132.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cFacturasMesAMes, "Factura mes a mes")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - NOTAS CREDITO MES A MES
    Private Sub RibbonButton133_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton133.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cNotasCreditoMesMes, "Notas de Crédito mes a mes")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - NOTAS DEBITO MES A MES
    Private Sub RibbonButton134_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton134.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cNotasDebitoMesMes, "Notas Débito mes a mes")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - Pesos a pedidos
    Private Sub RibbonButton233_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton233.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cPesosPedidos, "Pesos a pedidos")
        f = Nothing
    End Sub

    'REPORTES VENTAS - backorders
    Private Sub RibbonButton110_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton110.Click
        Dim f As New jsVenRepParametrosDos
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cBackorders, " Pedidos en Backorders ")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - pedidos Vs Estatus de clientes
    Private Sub RibbonButton111_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton111.Click
        Dim f As New jsVenRepParametrosDos
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cPedidosVsEstatus, "Pedidos Vs. Estatus de clientes (Semáforo) ")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - pedidos Vs Estatus de clientes (Prepedidos)
    Private Sub RibbonButton291_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton291.Click
        Dim f As New jsVenRepParametrosDos
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cPedidosVsEstatusPrepedidos, "Pedidos Vs. Estatus de clientes (Semáforo con prepedidos) ")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - Ranking de Ventas
    Private Sub RibbonButton112_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton112.Click
        Dim f As New jsVenRepParametrosDos
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cRanking, "Ranking de Ventas ")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - RELACION DE MERCANCIAS PARA GUIA DE PEDIDOS
    Private Sub RibbonButton270_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton270.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cGuiaCargaPedidos, "Relación de mercancías para carga de pedidos")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - COBRANZA PLANA
    Private Sub RibbonButton278_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton278.Click
        Dim f As New jsVenRepParametrosDos
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cCobranzaPlana, "TIEMPO DE COBRANZA")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - VENTAS PLANAS HEINZ
    Private Sub RibbonButton290_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton290.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cVentasHEINZ, "VENTAS HEIZ")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - COMISIONES POR JERARQUIA
    Private Sub RibbonButton281_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton281.Click
        Dim f As New jsVenRepParametrosDos
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cComisionesPorJerarquia, "COMISIONES POR JERARQUIA")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - COMISIONES POR JERARQUIA y FACTURAS
    Private Sub RibbonButton282_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton282.Click
        Dim f As New jsVenRepParametrosDos
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cComisionesPorFacturaYJerarquia, "COMISIONES POR JERARQUIA Y FACTURAS")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - COMISIONES POR COBRANZA ALIMICA
    Private Sub RibbonButton283_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton283.Click
        Dim f As New jsVenRepParametrosDos
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cComisionesPorDiasCobranza, "COMISIONES POR COBRANZA A PARTIR DE SU EMISION")
        f = Nothing
    End Sub

    'REPORTES VENTAS  - COMISIONES POR COBRANZA Y JERARQUIA (ORLYZAM)
    Private Sub RibbonButton285_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton285.Click

        Dim f As New jsVenRepParametrosDos
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cComisionesPorDiasCobranzaJerarquia, "COMISIONES POR COBRANZA A PARTIR DE SU EMISION Y JERARQUIAS")
        f = Nothing
    End Sub


#End Region
#End Region
#Region "Menu POS"
#Region "Archivos POS"
    '////////////////////////////////////////
    'PUNTOS DE VENTA
    '--- ARCHIVOS
    Private Sub RibbonButton91_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton91.Click
        'CAJAS
        Dim f As New jsPOSArcCajas
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton92_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton92.Click
        'CAJEROS O VENDEDORES DE PISO
        Dim f As New jsPOSArcCajeros
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton93_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton93.Click
        'SUPERVISORES
        Dim f As New jsPOSArcSupervisores
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton251_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton251.Click
        'FACTURAS
        Dim f As New jsPOSArcFacturas
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton266_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton266.Click
        '/////////////////////////////////77
        '///// CLIENTES
        Dim f As New jsPOSArcClientes
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
#End Region
#Region "Procesos POS"
    '---- PROCESOS PUNTOS DE VENTAS
    Private Sub RibbonButton54_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton54.Click
        'PROCESAR mercancias
        Dim f As New jsPOSProReprocesarmercancias
        f.Cargar(myConn)
        f = Nothing
    End Sub
    Private Sub RibbonButton271_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton271.Click
        'Reprocesar cierre de caja
        'Dim f As New jsPOSProCierre
        'f.Cargar(myConn, True, 1)
        'f = Nothing
    End Sub

#End Region
#Region "Reportes POS"
    '---- REPORTES PUNTOS DE VENTAS
    '' REPORTE XZ
    Private Sub RibbonButton186_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton186.Click
        Dim f As New jsPOSRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReportePuntoDeVenta.cReporteX, "ReporteXZ Puntos de Ventas")
        f = Nothing
    End Sub

    '' FACTURACION 
    Private Sub RibbonButton198_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton198.Click
        Dim f As New jsPOSRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReportePuntoDeVenta.cFacturacionBackEnd, "Facturación Puntos de Ventas")
        f = Nothing
    End Sub
#End Region
#End Region
#Region "Menu Mercancías"
#Region "Archivos Mercancias"
    '////////////////////////////////////////
    'MERCANCIAS  
    '--- ARCHIVOS
    Private Sub RibbonButton114_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton114.Click
        'MERCANCIAS
        Dim f As New jsMerArcMercancias
        OpenChildForm(f)
    End Sub
    Private Sub RibbonButton115_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton115.Click
        'CATEGORIAS MERCANCIAS
        Dim f As New jsControlArcTablaSimple
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar("Categorías de mercancías", FormatoTablaSimple(Modulo.iCategoriaMerca), True, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton135_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton135.Click
        'MARCAS MERCANCIAS
        Dim f As New jsControlArcTablaSimple
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar("Marcas de mercancías", FormatoTablaSimple(Modulo.iMarcaMerca), True, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub

    Private Sub RibbonButton136_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton136.Click
        'Jerarquias MERCANCIAS
        Dim f As New jsMerArcJerarquias
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        'If ChildFormOpen(Me, f) Then
        '    f.Activate()
        'Else
        '    f.MdiParent = Me
        '    f.Cargar(myConn, TipoCargaFormulario.iShow)
        'End If
        f = Nothing
    End Sub

    Private Sub RibbonButton301_Click(sender As System.Object, e As System.EventArgs) Handles RibbonButton301.Click
        Dim f As New jsMerArcPedidosAlmacen
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton137_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton137.Click
        'Transferencias de MERCANCIAS
        Dim f As New jsMerArcTransferencias
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton138_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles RibbonButton138.Click
        Dim f As New jsGenTallasColores
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton139_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton139.Click
        Dim f As New jsMerArcConteos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton140_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton140.Click
        Dim f As New jsMerArcPreciosFuturos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton141_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton141.Click
        Dim f As New jsMerArcPreciosEspeciales
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton142_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton142.Click
        Dim f As New jsMerArcOfertas
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton143_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton143.Click
        Dim f As New jsMerArcServicios
        OpenChildForm(f)
    End Sub
    Private Sub RibbonButton308_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton308.Click
        'MERCANCIAS
        Dim f As New jsMerArcEnvases
        OpenChildForm(f)
    End Sub
#End Region
#Region "Procesos Mercancías"
    '--------- PROCESOS MERCANCIAS
    Private Sub RibbonButton207_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton207.Click
        Dim f As New jsMerProPROCESARConteo
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton208_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton208.Click
        Dim f As New jsMerProREVERSARConteo
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton209_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton209.Click
        'RECONSTRUCCION DE MOVIMIENTOS Y EXISTENCIAS
        Dim f As New jsMerProReconstruirMovimientosMercancias
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton210_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton210.Click
        'RECONSTRUCCION DE precios
        Dim f As New jsMerProReconstruirPreciosMercancias
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton211_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton211.Click
        'CONSTRUCCION DE COMBOS
        Dim f As New jsMerProCombos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton212_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton212.Click
        'REVERSO DE COMBOS
        Dim f As New jsMerProCombosReverso
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing

    End Sub
    Private Sub RibbonButton284_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton284.Click
        'RECONSTRUCCION DE GANANCIAS
        Dim f As New jsMerProReconstruirGananciasMercancias
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton213_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton213.Click
        'Mercancías para almacenistas
        Dim f As New jsMerProExistenciasAlmacen
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton300_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton300.Click
        'Mercancías Detallistas
        Dim AlmacenDetal As String = Convert.ToString(ParametroPlus(myConn, Gestion.iPuntosdeVentas, "POSPARAM20"))

        If AlmacenDetal Is Nothing Or AlmacenDetal = "" Then
            ft.mensajeCritico("Debe indicar un almacén de Puntos de Venta en el Módulo Control De Gestiones/Parámetros/Puntos de Venta")
        Else
            Dim f As New jsMerProMercanciasDetallistas
            If ChildFormOpen(Me, f) Then
                f.Activate()
            Else
                f.MdiParent = Me
                f.Show()
            End If
            f = Nothing
        End If

    End Sub

    Private Sub RibbonButton206_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton206.Click
        'RECONSTRUCCION DE precios
        Dim f As New jsMerProCuotasMercancias
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton293_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton293.Click
        'Cuotasa partir de Compras
        Dim f As New jsMerProCuotasCompras
        f.Cargar(myConn)
        f = Nothing
    End Sub
#End Region
#Region "Reportes Mercancías"
    '--------- REPORTES MERCANCIAS
    Private Sub RibbonButton144_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton144.Click
        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cCatalogo, "Catálogo de mercancías")
        f = Nothing
    End Sub
    Private Sub RibbonButton145_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton145.Click
        '' EXISTENCIAS DE MERCANCIAS
        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cExistenciasAUnaFecha, "Existencias de mercancías a una fecha")
        f = Nothing
    End Sub
    Private Sub RibbonButton243_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton243.Click
        '' INVENTARIO LEGAL
        'Dim f As New jsMerRepParametros
        'f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cInventarioLegal, "Inventarios y Costos")
        'f = Nothing
        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cInventarioLegal, "Inventarios y Costos")
        f = Nothing
    End Sub
    Private Sub RibbonButton146_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton146.Click
        '' EQUIVALENCIAS DE MERCANCIAS
        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cEquivalencias, "Equivalencias de mercancías")
        f = Nothing
    End Sub
    Private Sub RibbonButton147_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton147.Click
        '' MOVIMIENTOS DE MERCANCIAS
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cMovimientosMercancias, "Movimientos de mercancías")
        f = Nothing
    End Sub
    Private Sub RibbonButton303_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton303.Click
        '' MOVIMIENTOS DE MERCANCIAS
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cMovimientosServiciosS, "Movimientos de Servicios")
        f = Nothing
    End Sub
    Private Sub RibbonButton265_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton265.Click
        '' MOVIMIENTOS DE MERCANCIAS (resumen) 
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cMovimientosResumen, "Movimientos de mercancías (Resumen)")
        f = Nothing
    End Sub
    Private Sub RibbonButton148_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton148.Click
        '' OBSOLESCENCIAS
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cObsolescencia, "Obsolescencias de mercancías")
        f = Nothing
    End Sub
    Private Sub RibbonButton149_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton149.Click
        '' VENTAS NETAS DE MERCANCIAS
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cVentasNetasMercancias, "VENTAS NETAS DE MERCANCIAS")
        f = Nothing
    End Sub
    Private Sub RibbonButton151_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton151.Click
        'LISTADOS DE TRANSFERENCIAS
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cListadoTransferencias, "LISTADO DE TRANSFERENCIAS")
        f = Nothing
    End Sub
    Private Sub RibbonButton152_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton152.Click
        'PRECIOS con IVA MERCANCIAS
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cPreciosIVA, "Precios de mercancías CON IVA")
        f = Nothing
    End Sub
    Private Sub RibbonButton153_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton153.Click
        'PRECIOS SIN IVA MERCANCIAS
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cPrecios, "Precios de mercancías sin IVA")
        f = Nothing
    End Sub
    Private Sub RibbonButton154_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton154.Click
        'PRECIOS SIN IVA MERCANCIAS
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cPreciosYEquivalencias, "Precios y equivalencias de mercancías")
        f = Nothing
    End Sub
    Private Sub RibbonButton311_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton311.Click
        'PRECIOS SIN IVA MERCANCIAS
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cPreciosYEquivalenciasSinIVA, "Precios y equivalencias de mercancías SIN IVA")
        f = Nothing
    End Sub
    Private Sub RibbonButton155_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton155.Click
        'VENTAS DE MERCANCIAS POR CLIENTES
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cVentasDeMercancias, "Ventas de mercancías por clientes")
        f = Nothing
    End Sub
    Private Sub RibbonButton156_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton156.Click
        'VENTAS DE MERCANCIAS POR CLIENTES y Activados
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cVentasDeMercanciasActivacion, "Ventas de mercancías por clientes y activación")
        f = Nothing
    End Sub
    Private Sub RibbonButton157_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton157.Click
        'VENTAS DE MERCANCIAS POR CLIENTES y Activados Mes a Mes
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cVentasDeMercanciasActivacionXMes, "Ventas de mercancías por clientes y activación mes a mes")
        f = Nothing
    End Sub
    Private Sub RibbonButton158_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton158.Click
        'COMPRAS DE MERCANCIAS
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cComprasDeMercancias, "COMPRAS DE MERCANCIAS")
        f = Nothing
    End Sub
    Private Sub RibbonButton279_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton279.Click
        'REPORTES DE MERCANCIAS - VENTAS POR JERARQUIA PLANA
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cVentasPlana, "VENTAS POR JERARQUIA")
        f = Nothing
    End Sub
    Private Sub RibbonButton159_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton159.Click
        'COMPRAS DE MERCANCIAS y activaciobn
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cComprasdeMercanciasActivacion, "COMPRAS DE MERCANCIAS Y ACTIVACION")
        f = Nothing
    End Sub
    Private Sub RibbonButton280_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton280.Click
        'ESTRUCTURA DE COSTOS
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cLeyDeCostos, "ESTRUCTURA DE COSTOS Y GANANCIAS")
        f = Nothing
    End Sub
#End Region
#End Region
#Region "Menu Medición Gerencial"
#Region "Archivos Medición Gerencial"
    '//////////////////////////////////////////
    '///////// MEDICION GERENCIAL /////////////
    Private Sub RibbonButton302_Click(sender As Object, e As EventArgs) Handles RibbonButton302.Click
        Dim f As New jsSIGMEArcDashBoard
        OpenChildForm(f)
    End Sub
#End Region
#Region "Procesos Medición Gerencial"

#End Region
#Region "Reportes Medición Gerencial"
    '--------- REPORTES MEDICION Bancos ------------
    Private Sub RibbonButton252_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton252.Click
        '' Ingresos por asesor y forma de pago
        Dim f As New jsSIGMERepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cIngresosPorAsesor, "Ingresos por asesor y forma de pago")
        f = Nothing
    End Sub
    Private Sub RibbonButton253_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton253.Click
        '' Compras comprarativas mes a mes
        'Dim f As New jsSIGMERepParametros
        'f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cComprasComparativasMesMes, "Compras comparativas mes a mes")
        'f = Nothing
        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cComprasComparativasMesMes, "COMPRAS COMPARATIVAS MES A MES")
        f = Nothing
    End Sub

    '--------- REPORTES MEDICION GERENCIAL Ventas ------------
    Private Sub RibbonButton296_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton296.Click
        ' VOLUMEN DE VENTAS POR ASESOR COMERCIAL
        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cVentasAsesor, " Volumen de ventas por Asesor Comercial ")
        f = Nothing

    End Sub
    Private Sub RibbonButton297_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton297.Click
        ' ACTIVACION DE CLIENTES Y MERCANCIAS POR ASESOR COMERCIAL
        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cActivacionAsesor, " Activaciones de clientes y mercancías por Asesor Comercial ")
        f = Nothing

    End Sub


    Private Sub RibbonButton254_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton254.Click
        '' Ventas comprarativas mes a mes
        'Dim f As New jsSIGMERepParametros
        'f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cVentasComparativasMesMes, "Ventas comparativas mes a mes")
        'f = Nothing 
        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cVentasComparativasMesMes, "VENTAS COMPARATIVAS MES A MES")
        f = Nothing
    End Sub

    Private Sub RibbonButton255_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton255.Click
        '' Ventas mes a mes por asesor
        'Dim f As New jsSIGMERepParametros
        'f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cVentasMesMesAsesor, "Ventas mes a mes por asesor")
        'f = Nothing
        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cVentasMesMesAsesor, "Ventas mes a mes por asesor")
        f = Nothing
    End Sub
    Private Sub RibbonButton295_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton295.Click
        '' DEVOLUCIONES POR CAUSA
        'Dim f As New jsSIGMERepParametros
        'f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cDevolucionesAsesorMesMes, "Devoluciones mes a mes por asesor")
        'f = Nothing
        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cDevolucionesPorCausa, "FRECUENCIA DEVOLUCIONES POR CAUSA")
        f = Nothing
    End Sub

    Private Sub RibbonButton261_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton261.Click,
        RibbonButton120.Click
        '' Activacion Clientes Mes a Mes
        'Dim f As New jsSIGMERepParametros
        'f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cActivacionClientesMesMes, "Activación de clientes mes a mes")
        'f = Nothing
        Dim f As New jsVenRepParametrosPlus
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cActivacionClientesMesMes, "Activación de clientes mes a mes")
        f = Nothing
    End Sub
    Private Sub RibbonButton262_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton262.Click
        '' DropSize Clientes Mes a Mes
        Dim f As New jsSIGMERepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cDropSizeMesMes, "Drop Size mes a mes")
        f = Nothing
    End Sub
    Private Sub RibbonButton256_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton256.Click
        '' GANANCIAS BRUTAS POR FACTURAS
        Dim f As New jsSIGMERepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cGananciasPorFactura, "Ganancias Brutas por Facturación")
        f = Nothing
    End Sub
    Private Sub RibbonButton257_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton257.Click
        '' GANANCIAS BRUTAS POR ITEM
        Dim f As New jsSIGMERepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cGananciasPorItem, "Ganancias Brutas por mercancía facturada")
        f = Nothing
    End Sub
    Private Sub RibbonButton258_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton258.Click
        '' GANANCIAS BRUTAS MES A MES
        Dim f As New jsSIGMERepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cGananciasItemMesMes, "Ganancias Brutas por mercancía Mes A Mes")
        f = Nothing
    End Sub
    Private Sub RibbonButton259_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton259.Click
        '' GANANCIAS brutas en cheques de alimentacion
        Dim f As New jsSIGMERepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cGananciasCestaTicket, "Ganancias Brutas en cheques de alimentación")
        f = Nothing
    End Sub
    Private Sub RibbonButton260_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton260.Click
        '' GANANCIAS brutas en cheques de alimentacion mes a mes
        Dim f As New jsSIGMERepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cGananciasCestaTicketMesMes, "Ganancias Brutas en cheques de alimentación mes a mes")
        f = Nothing
    End Sub
    Private Sub RibbonButton263_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton263.Click
        '' Descuentos Asesor
        Dim f As New jsSIGMERepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cDescuentosAsesor, "Descuentos otorgados por asesor en un período")
        f = Nothing
    End Sub
    Private Sub RibbonButton264_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton264.Click
        '' Descuentos Asesor mes ames
        Dim f As New jsSIGMERepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cDescuentosAsesorMesMes, "Descuentos otorgados por asesor mes a mes")
        f = Nothing
    End Sub
    Private Sub RibbonButton244_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton244.Click
        '' Ventas, Compras y Existencias mes a mes
        Dim f As New jsSIGMERepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMedicionGerencial.cVentasComprasExistenciasMesMes, "Ventas, compras y existencias por jerarquía mes a mes")
        f = Nothing
    End Sub
#End Region
#End Region
#Region "Menu Producción"
#Region "Archivos Producción"
    '//////////////////////////////////////////
    'PRODUCCION
    '
    '/// ARCHIVOS
    Private Sub RibbonButton161_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton161.Click
        'FORMULACIONES  
        Dim f As New jsProArcFormulaciones
        OpenChildForm(f)

    End Sub
    Private Sub RibbonButton162_Click(sender As Object, e As EventArgs) Handles RibbonButton162.Click
        'ORDEN DE PRODUCCION
        Dim f As New jsProArcOrdenProduccion
        OpenChildForm(f)
    End Sub
    Private Sub RibbonButton163_Click(sender As Object, e As EventArgs) Handles RibbonButton163.Click
        'ORDEN DE PRODUCCION
        Dim f As New jsProArcSeguimientoProduccion
        OpenChildForm(f)
    End Sub
#End Region
#Region "Procesos Producción"

#End Region
#Region "Reportes Producción"

#End Region
#End Region
#Region "Menu Control de Gestiones"
#Region "Archivos Control de Gestiones"
    '////////////////////////////////////////
    'CONTROL DE GESTIONES - ARCHIVOS 
    '
    Private Sub OpenChildForm(ByVal f As Form)
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Show()
        End If
        f = Nothing
    End Sub

    Private Sub RibbonButton165_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton165.Click
        'Empresas  
        Dim f As New jsControlArcEmpresas
        OpenChildForm(f)

    End Sub
    Private Sub RibbonButton167_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton167.Click
        'Usuarios del sistema 
        Dim f As New jsControlArcUsuarios
        OpenChildForm(f)
    End Sub
    Private Sub RibbonButton166_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton166.Click
        'Mapas de acceso 
        Dim f As New jsControlArcCalendario
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton168_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton168.Click
        'Mapas de acceso 
        Dim f As New jsControlArcMapas
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, aGestionVisible)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton169_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton169.Click
        'Auditorias de accesos del sistema 
        Dim f As New jsControlArcAccesos
        OpenChildForm(f)
    End Sub
    Private Sub RibbonButton170_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton170.Click
        'Mapas de acceso 
        Dim f As New jsControlArcIVA
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton171_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton171.Click
        'Retenciones de impuesto sobre la renta 
        Dim f As New jsControlArcRetencionesISLR
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton203_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton203.Click
        'Unidades tributarias 
        Dim f As New jsControlArcUT
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton267_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton267.Click
        'Tipos de impresoras fiscales
        Dim f As New jsControlArcImpresoraFiscal
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton172_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton172.Click
        'Contadores 
        Dim f As New jsControlArcContadores
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton173_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton173.Click
        'Parámetros de extensibilidad del sistema 
        Dim f As New jsControlArcParametros
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton174_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton174.Click
        'Tarjetas débito / crédito
        Dim f As New jsControlArcCondicionesPagoCobros
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton175_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton175.Click
        'Tarjetas débito / crédito
        Dim f As New jsControlArcTarjetas
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton177_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton177.Click
        'Causas créditos
        Dim f As New jsControlArcCausasCreditos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, 0, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton178_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton178.Click
        'Causas Debitos
        Dim f As New jsControlArcCausasCreditos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, 1, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton306_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton306.Click
        'UNIDADES DE MEDIDA
        Dim f As New jsControlArcTablaSimple
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar("UNIDADES DE MEDIDA PARA EQUIVALENCIAS DE MERCANCIAS", FormatoTablaSimple(Modulo.iMER_UnidadesDeMedida), True, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton179_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton179.Click
        'Divisiones
        Dim f As New jsControlArcDivisiones
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton180_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton180.Click
        'TRANSPORTES
        Dim f As New jsControlArcTransportes
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton181_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton181.Click
        'Almacenes
        Dim f As New jsControlArcAlmacenes
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton182_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton182.Click
        'Bancos de la plaza
        Dim f As New jsControlArcTablaSimple
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar("Bancos", FormatoTablaSimple(Modulo.iBancos), True, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton183_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton183.Click
        'CORREDORES DE CESTA TICKET
        Dim f As New jsControlArcCorredoresCestaTicket
        OpenChildForm(f)
    End Sub
    Private Sub RibbonButton184_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton184.Click
        'TERRITORIOS
        Dim f As New jsControlArcTerritorio
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton185_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton185.Click
        'CAMBIO DE Monedas
        Dim f As New jsControlArcCambioMonedas
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton312_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton312.Click
        'MONEDAS
        Dim f As New jsControlArcMonedas
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton197_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonButton197.Click
        'CONJUNTOS DE TABLA 
        Dim f As New jsControlArcConjuntos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, TipoCargaFormulario.iShow)
        End If
        f = Nothing
    End Sub

#End Region
#Region "Procesos Control de Gestiones"
    '////////////////////////////////////////
    'CONTROL DE GESTIONES - PROCESOS 
    '
    Private Sub RibbonButton202_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton202.Click
        'VERIFICA  BASES DE DATOS
        Dim f As New jsControlProVerificaBD
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton205_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton205.Click
        Dim f As New jsControlProCierreEjercicio
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton294_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton294.Click
        Dim f As New jsControlProPasaDatosEjercicio
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton234_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton234.Click
        Dim f As New jsControlProConsecutivos
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub

    Private Sub RibbonButton309_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton309.Click
        'CIERRE DE MODULOS POR PERIODOS
        Dim f As New jsControlProCierreDiario
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, 0)
        End If
        f = Nothing
    End Sub

    Private Sub RibbonButton310_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton310.Click
        'CIERRE DE MODULOS POR PERIODOS
        Dim f As New jsControlProCierreDiario
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn, 1)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton65_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton65.Click
        'CIERRE DE MODULOS POR PERIODOS
        Dim f As New jsControlProReconversionMonetaria
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
#End Region
#Region "Reportes Control de Gestiones"

#End Region
#End Region
#Region "Menu Misceláneos"
    '///////////////////////////
    'MISCELANEOS
    '///////////////////////////
    '/MENU
    Private Sub RibbonButton190_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton190.Click

        jytsistema.WorkID = CargarTablaSimple(myConn, lblInfo, ds, " select a.id_emp codigo, a.nombre descripcion " _
                                              & " from jsconctaemp a " _
                                              & " left join jsconctausuemp b on (a.id_emp = b.id_emp ) " _
                                              & " WHERE " _
                                              & " b.permite_empresa = 1 and " _
                                              & " b.id_user = '" & jytsistema.sUsuario & "' " _
                                              & " order by a.id_emp ", "Empresas...", jytsistema.WorkID)
        jytsistema.WorkExercise = ""
        jytsistema.WorkName = ft.DevuelveScalarCadena(myConn, " select nombre from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub RibbonButton37_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton37.Click
        System.Diagnostics.Process.Start("calc.exe")
    End Sub
    Private Sub RibbonButton307_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton307.Click
        Dim f As New frmServidorValidacion
        f.Cargar(myConn)
        f.Dispose()
        f = Nothing
    End Sub
    Private Sub RibbonButton23_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton23.Click
        'Cambio de ejercicio
        Dim f As New jsCambioEjercicio
        If ChildFormOpen(Me, f) Then
            f.Activate()
        Else
            f.MdiParent = Me
            f.Cargar(myConn)
        End If
        f = Nothing
    End Sub
    Private Sub RibbonButton204_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton204.Click
        'Cambio de fecha
        Dim f As New FechaSistema
        f.Cargar(jytsistema.sFechadeTrabajo)
        jytsistema.sFechadeTrabajo = f.Fecha
        f = Nothing
    End Sub
    Private Sub RibbonButton187_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton187.Click
        Dim f As New AcercaDe
        f.ShowDialog()
        f = Nothing
    End Sub

    Private Sub RibbonButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RibbonButton6.Click

        If inicioHorarios Then
            C1Ribbon1.Dispose()
            C1Ribbon1 = Nothing
            Application.Exit()
        Else
            ft.mensajeInformativo(" Se estan generando procesos por favor, intentelo más tarde...")
        End If
    End Sub

    Private Sub RibbonTab1_Select(ByVal sender As Object, ByVal e As System.EventArgs) Handles RibbonTab1.Select,
        RibbonTab2.Select, RibbonTab3.Select, RibbonTab4.Select, RibbonTab5.Select, RibbonTab6.Select,
        RibbonTab7.Select, RibbonTab9.Select, RibbonTab8.Select, RibbonTab10.Select
        Dim nName As String = sender.id.ToString

        nGestion = CInt(IIf(nName.Length = 10, Microsoft.VisualBasic.Right(nName, 1),
                       Microsoft.VisualBasic.Right(nName, 2))) - 1

    End Sub

    Private Sub scrMain_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Cursor.Current = Cursors.Default
    End Sub
#End Region

End Class
