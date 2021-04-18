Imports MySql.Data.MySqlClient
Imports C1.Win.C1Chart
Public Class jsVenArcAsesores
    Private Const sModulo As String = "Asesores Comerciales"
    Private Const lRegion As String = "RibbonButton80"
    Private Const nTabla As String = "tblAsesores"
    Private Const nTablaDivisiones As String = "tblDivitions"
    Private Const nTablaJerarquias As String = "tblHierarchies"
    Private Const nTablaDesc As String = "tblDescuentos"
    Private Const nTablacierre As String = "tblCierre"
    Private Const nTablaRenglonesCierre As String = "tblRenglonesCierre"
    Private Const nTablaFactores As String = "tblFactores"
    Private Const nTablaCuotas As String = "tblCuotasAsesor"
    Private Const nTablaVen As String = "tblVendedores"

    Private strSQL As String = "select * from jsvencatven where tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by codven "
    Private strSQLDesc As String = ""
    Private strSQLDivition As String = ""
    Private strSQLHierarchy As String = ""
    Private strSQLCierre As String = ""
    Private strSQLRenglonesCierre As String = ""
    Private strSQLFactores As String = ""
    Private strSQLCuotas As String = ""
    Private strVendedores As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtDescuentos As New DataTable
    Private dtCierre As New DataTable
    Private dtRenglonesCierre As New DataTable
    Private dtFactores As New DataTable
    Private dtDivition As New DataTable
    Private dtHierarchy As New DataTable
    Private dtCuotas As New DataTable
    Private dtVendedores As New DataTable
    Private dtStat As New DataTable
    Private ft As New Transportables

    Private aCondicion() As String = {"Inactivo", "Activo"}
    Private aClase() As String = {"VENDEDOR", "SUPERVISOR", "GERENTE DE VENTAS"}
    Private aEstadistica() As String = {"Mercancías", "Categorías", "Marcas", "Divisiones", "Jerarquías", "Cobranza Anterior", "Cobranza Actual"}
    Private aFactores() As String = {"Porcentajes de comisión por cobranza y días vencidos general", _
                                     "Porcentajes de comisión por ventas de jerarquías", _
                                     "Cuotas por jerarquías", _
                                     "Porcentajes de comisión por cobranza y días vencidos por jerarquías "}

    Private aCuotasTipo() As String = {"MERCANCIAS", "CATEGORIAS", "MARCAS", "DIVISIONES", "JERARQUIAS"}

    Private strUnidad As String = ""
    Private aUnidad() As String = {}


    Private i_modo As Integer

    Private nPosicionCat As Long
    Private FechaIngreso As Date
    Private nPosicionEst As Long
    Private nPosicionDes As Long
    Private nPosicionCie As Long
    Private nPosicionRenglonesCie As Long
    Private nPocisionFactores As Long
    Private nPosicionDiv As Long
    Private nPosicionHie As Long
    Private nPosicionCuo As Long
    Private nPosicionVen As Long

    Private Sub jsVenArcAsesores_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub



    Private Sub jsVenArcAsesores_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo

        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaTXT(nPosicionCat)
            Else
                IniciarAsesor(False)
            End If
            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub

    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnSeleccionar, btnPrimero, _
                          btnSiguiente, btnAnterior, btnUltimo, btnImprimir, btnSalir)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        If dt.Rows.Count > 0 Then

            With dt

                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

                With .Rows(nRow)

                    'Asesor Comercial
                    nPosicionCat = nRow

                    txtCodigo.Text = ft.MuestraCampoTexto(.Item("codven"))
                    txtNombre.Text = ft.MuestraCampoTexto(.Item("nombres"))
                    txtApellidos.Text = ft.MuestraCampoTexto(.Item("apellidos"))
                    txtdescripcion.Text = ft.MuestraCampoTexto(.Item("descar"))
                    '  txtCargo.Text = .Item("estructura").ToString
                    txtFactor.Text = ft.FormatoNumero(.Item("factorcuota"))
                    chkA.Checked = CBool(.Item("lista_a"))
                    chkB.Checked = CBool(.Item("lista_b"))
                    chkC.Checked = CBool(.Item("lista_c"))
                    chkD.Checked = CBool(.Item("lista_d"))
                    chkE.Checked = CBool(.Item("lista_e"))
                    chkF.Checked = CBool(.Item("lista_f"))
                    FechaIngreso = CDate(.Item("ingreso").ToString)

                    ft.RellenaCombo(aCondicion, cmbEstatus, .Item("estatus"))
                    ft.RellenaCombo(aClase, cmbClase, .Item("clase"))

                    If .Item("clase") = 1 Then
                        AbrirVendedoresSupervisor(.Item("codven"))
                    Else
                        dgVendedores.Columns.Clear()
                    End If

                    AbrirDivisiones(.Item("codven"))
                    AbrirJerarquias(.Item("codven"))


                    txtCarteraMercas.Text = ft.FormatoEntero(CarteraGrupo(myConn, lblInfo, .Item("codven"), "codart"))
                    txtCarteraMarcas.Text = ft.FormatoEntero(CarteraGrupo(myConn, lblInfo, .Item("codven"), "marca"))
                    txtCarteraCatego.Text = ft.FormatoEntero(CarteraGrupo(myConn, lblInfo, .Item("codven"), "grupo"))
                    txtCarteraJerarquia.Text = ft.FormatoEntero(CarteraGrupo(myConn, lblInfo, .Item("codven"), "tipjer"))
                    txtCarteraDivisiones.Text = ft.FormatoEntero(CarteraGrupo(myConn, lblInfo, .Item("codven"), "division"))

                    'Estadisticas
                    txtCodigoEstadisticas.Text = .Item("codven")
                    txtNombreEstadisticas.Text = ft.MuestraCampoTexto(.Item("apellidos")) & ", " & ft.MuestraCampoTexto(.Item("nombres"))

                    'Cierres
                    txtCodigoCierres.Text = .Item("codven")
                    txtNombreCierres.Text = ft.MuestraCampoTexto(.Item("apellidos")) & ", " & ft.MuestraCampoTexto(.Item("nombres"))

                    'Descuentos
                    txtCodigoDescuentos.Text = .Item("codven")
                    txtNombreDescuentos.Text = ft.MuestraCampoTexto(.Item("apellidos")) & ", " & ft.MuestraCampoTexto(.Item("nombres"))

                    'Factores
                    txtCodigoFactores.Text = .Item("codven")
                    txtNombreFactores.Text = ft.MuestraCampoTexto(.Item("apellidos")) & ", " & ft.MuestraCampoTexto(.Item("nombres"))

                    'Cuotas
                    txtCodigoCuotas.Text = .Item("codven")
                    txtNombreCuotas.Text = ft.MuestraCampoTexto(.Item("apellidos")) & ", " & ft.MuestraCampoTexto(.Item("nombres"))

                End With
            End With
        End If

        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub AbrirVendedoresSupervisor(ByVal CodigoSupervisor As String)

        strVendedores = " SELECT a.codven, concat(a.apellidos,', ',a.nombres) vendedor  " _
                & " FROM jsvencatven a " _
                & " WHERE " _
                & " a.supervisor = '" & CodigoSupervisor & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' order by a.codven "

        dtVendedores = ft.AbrirDataTable(ds, nTablaVen, myConn, strVendedores)

        Dim aCam() As String = {"codven.Código.50.C.", "Vendedor.Asesor Comercial.200.I."}
        ft.IniciarTablaPlus(dgVendedores, dtVendedores, aCam)
        If dtVendedores.Rows.Count > 0 Then nPosicionVen = 0

    End Sub

    Private Sub AbrirDivisiones(ByVal CodigoVendedor As String)

        strSQLDivition = " SELECT a.division, b.descrip " _
                & " FROM jsvencatvendiv a " _
                & " LEFT JOIN jsmercatdiv b ON (a.division = b.division AND a.id_emp = b.id_emp) " _
                & " WHERE " _
                & " a.codven = '" & CodigoVendedor & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' order by a.division "

        dtDivition = ft.AbrirDataTable(ds, nTablaDivisiones, myConn, strSQLDivition)
        Dim aCam() As String = {"division.División.90.C.", "descrip..20.I."}
        ft.IniciarTablaPlus(dgDivisiones, dtDivition, aCam)
        If dtDivition.Rows.Count > 0 Then nPosicionDiv = 0

    End Sub
    Private Sub AbrirJerarquias(ByVal CodigoVendedor As String)

        strSQLHierarchy = " SELECT a.tipjer, b.descrip " _
        & " FROM jsvencatvenjer a " _
        & " LEFT JOIN jsmerencjer b ON (a.tipjer = b.tipjer AND a.id_emp = b.id_emp) " _
        & " WHERE " _
        & " a.codven = '" & CodigoVendedor & "' AND " _
        & " a.id_emp = '" & jytsistema.WorkID & "' order by a.tipjer "

        ds = DataSetRequery(ds, strSQLHierarchy, myConn, nTablaJerarquias, lblInfo)
        dtHierarchy = ds.Tables(nTablaJerarquias)

        Dim aCam() As String = {"tipjer", "descrip"}
        Dim aNom() As String = {"Jerarquía", ""}
        Dim aAnc() As Integer = {90, 200}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", ""}

        IniciarTabla(dgJerarquías, dtHierarchy, aCam, aNom, aAnc, aAli, aFor)
        If dtHierarchy.Rows.Count > 0 Then nPosicionHie = 0

    End Sub
    Private Sub IniciarAsesor(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = ft.autoCodigo(myConn, "codven", "jsvencatven", "tipo.id_emp", "0." + jytsistema.WorkID, 5)
        Else
            txtCodigo.Text = ""
        End If

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNombre, txtApellidos, txtdescripcion, txtCargo)
        ft.iniciarTextoObjetos(FormatoItemListView.iEntero, txtCarteraCatego, txtCarteraDivisiones, txtCarteraJerarquia, txtCarteraMarcas, txtCarteraMercas)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtFactor)

        ft.RellenaCombo(aCondicion, cmbEstatus, 1)
        ft.RellenaCombo(aClase, cmbClase)

        chkA.Checked = True
        chkB.Checked = False
        chkC.Checked = False
        chkD.Checked = False
        chkE.Checked = False
        chkF.Checked = False

        FechaIngreso = ft.FormatoFecha(jytsistema.sFechadeTrabajo)

    End Sub
    Private Sub IniciarDescuentos(ByVal CodAsesor As String)

        If CodAsesor <> "" Then
            strSQLDesc = " SELECT * from jsconcatdes where codven = '" & CodAsesor & "' and tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by inicio desc, coddes "

            ds = DataSetRequery(ds, strSQLDesc, myConn, nTablaDesc, lblInfo)
            dtDescuentos = ds.Tables(nTablaDesc)

            Dim aCam() As String = {"coddes", "descrip", "pordes", "inicio", "fin", "codcli", "codart", ""}
            Dim aNom() As String = {"Código", "Descripción", "% Descuento", "Desde", "Hasta", "Codigo cliente", "Codigo Mercancía", ""}
            Dim aAnc() As Integer = {60, 200, 70, 90, 90, 100, 100, 30}
            Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, _
                                     AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                                     AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
            Dim aFor() As String = {"", "", sFormatoNumero, sFormatoFecha, sFormatoFecha, "", "", ""}

            IniciarTabla(dgDescuentos, dtDescuentos, aCam, aNom, aAnc, aAli, aFor)
            If dtDescuentos.Rows.Count > 0 Then nPosicionDes = 0

        End If

    End Sub
    Private Sub IniciarFactores()
        ft.RellenaCombo(aFactores, cmbFactores)
    End Sub
    Private Sub IniciarCierre(ByVal CodigoAsesor As String)
        If CodigoAsesor <> "" Then
            strSQLCierre = " select * from jsvenenccie where codven = '" & CodigoAsesor & "' and id_emp = '" & jytsistema.WorkID & "' order by fecha "
            ds = DataSetRequery(ds, strSQLCierre, myConn, nTablacierre, lblInfo)
            dtCierre = ds.Tables(nTablacierre)
            If dtCierre.Rows.Count > 0 Then
                nPosicionCie = dtCierre.Rows.Count - 1
                AsignarCierre(dtCierre, nPosicionCie)
            End If

        End If
    End Sub
    Private Sub AsignarCierre(ByVal dtCierre As DataTable, ByVal nRowCierre As Long)

        If dtCierre.Rows.Count > 0 Then

            With dtCierre.Rows(nRowCierre)

                Dim nAsesor As String = .Item("codven")
                Dim nClase As Integer = ft.DevuelveScalarEntero(myConn, " select clase from jsvencatven where codven = '" & .Item("codven") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                Dim nPorCentajeComision As Double = 0.0
                Dim nAVisitar As Integer = 0
                Select Case nClase
                    Case 0 'ASESOR COMERCIAL
                        nPorCentajeComision = CDbl(ParametroPlus(MyConn, Gestion.iVentas, "VENFVEPA03"))
                        nAVisitar = .Item("avisitar")
                    Case 1 'SUPERVISOR
                        nPorCentajeComision = CDbl(ParametroPlus(MyConn, Gestion.iVentas, "VENFVEPA04"))
                        nAVisitar = ClientesAVisitarSupervisor(myConn, lblInfo, .Item("codven"), jytsistema.sFechadeTrabajo)
                    Case 2 'GERENCIA
                        nPorCentajeComision = CDbl(ParametroPlus(MyConn, Gestion.iVentas, "VENFVEPA05"))
                        nAVisitar = 0
                    Case Else
                        nPorCentajeComision = CDbl(ParametroPlus(MyConn, Gestion.iVentas, "VENFVEPA03"))
                        nAVisitar = .Item("AVISITAR")
                End Select


                txtFechaCierre.Text = ft.FormatoFecha(CDate(.Item("fecha").ToString))
                txtAVisitar.Text = ft.FormatoEntero(nAVisitar)
                txtVisitados.Text = ft.FormatoEntero(.Item("visitados"))
                txtEfectividad.Text = ft.FormatoNumero(.Item("efectividad"))
                txtCierreUnidades.Text = ft.FormatoCantidad(.Item("items_caj"))
                txtCierreKilogramos.Text = ft.FormatoCantidad(.Item("items_kgs"))
                txtMontoCD.Text = ft.FormatoNumero(.Item("monto_cd"))
                txtComisionesCD.Text = ft.FormatoNumero(.Item("monto_com_cd"))
                txtCantidadCD.Text = ft.FormatoEntero(.Item("cantidad_cd"))

                txtCostosVentas.Text = ft.FormatoNumero(.Item("costosventas"))
                txtComision.Text = ft.FormatoNumero(.Item("costosventas") * nPorCentajeComision / 100)

                strSQLRenglonesCierre = " SELECT * from jsvenrencie " _
                            & " where " _
                            & " codven = '" & .Item("codven") & "' and " _
                            & " MONTH(fecha) = " & CDate(.Item("fecha").ToString).Month & " and " _
                            & " YEAR(fecha) = " & CDate(.Item("fecha").ToString).Year & " and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by kilos desc "

                ds = DataSetRequery(ds, strSQLRenglonesCierre, myConn, nTablaRenglonesCierre, lblInfo)
                dtRenglonesCierre = ds.Tables(nTablaRenglonesCierre)

                Dim aCam() As String = {"cliente", "nombre", "cajas", "kilos", "totalpedido", "totaldev"}
                Dim aNom() As String = {"Código", "Nombre o Razón Social", "Ventas (UV)", "Ventas (KGR)", "Pedidos (KGR)", "% Satisfacción "}
                Dim aAnc() As Integer = {100, 400, 100, 100, 100, 100}
                Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, _
                                         AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}
                Dim aFor() As String = {"", "", sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoNumero}

                IniciarTabla(dgCierre, dtRenglonesCierre, aCam, aNom, aAnc, aAli, aFor)
                If dtRenglonesCierre.Rows.Count > 0 Then nPosicionRenglonesCie = 0

            End With
        End If
    End Sub
    Private Sub ActivarMarco0()
        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(False, False, C1DockingTabPage2, C1DockingTabPage6, C1DockingTabPage4)
        ft.habilitarObjetos(True, True, txtApellidos, txtNombre, txtdescripcion, btnCargo, cmbEstatus, cmbClase, txtFactor)
        ft.habilitarObjetos(True, False, MenuJerarquias, MenuDivisiones, MenuVendedores, chkA, chkB, chkC, chkD, chkE, chkF)

        'If i_modo = movimiento.iEditar Then ft.habilitarObjetos(False, False, txtCodigo, cmbClase)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub

    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(True, False, C1DockingTabPage2, C1DockingTabPage3, C1DockingTabPage4, C1DockingTabPage6)

        ft.habilitarObjetos(False, True, txtCodigo, txtNombre, txtApellidos, txtdescripcion, txtCargo, txtEstructura, _
                         txtCarteraCatego, txtCarteraDivisiones, txtCarteraJerarquia, txtCarteraMarcas, txtCarteraMercas, _
                         txtFactor, cmbEstatus, cmbClase, btnCargo, txtCodigoCierres, txtNombreCierres, _
                         txtFechaCierre, txtAVisitar, txtVisitados, txtEfectividad, txtCierreUnidades, txtCierreKilogramos, _
                         txtComision, txtCostosVentas, _
                         txtCodigoDescuentos, txtNombreDescuentos, _
                         txtCodigoEstadisticas, txtNombreEstadisticas, _
                         txtCodigoFactores, txtNombreFactores, txtCodigoCuotas, txtNombreCuotas, _
                         txtTotalAcumulado, txtTotalCierre, txtTotalCuota, txtTotalLogro, txtTotalMeta)

        ft.habilitarObjetos(False, False, chkA, chkB, chkC, chkD, chkE, chkF, MenuDivisiones, MenuJerarquias, MenuVendedores)
        MenuBarra.Enabled = True
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Select Case tbcAsesores.SelectedTab.Text
            Case "Asesores Comerciales"
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case "Estadísticas"
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarEstadistica()
            Case "Cierres"
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarCierre(txtCodigo.Text)
            Case "Descuentos"
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarDescuentos(txtCodigo.Text)
            Case "Factores"
        End Select



    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Select Case tbcAsesores.SelectedTab.Text
            Case "Asesores Comerciales"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case "Estadísticas"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarEstadistica()
            Case "Cierres"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarCierre(txtCodigo.Text)
            Case "Descuentos"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarDescuentos(txtCodigo.Text)
            Case "Factores"
        End Select

    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Select Case tbcAsesores.SelectedTab.Text
            Case "Asesores Comerciales"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case "Estadísticas"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarEstadistica()
            Case "Cierres"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarCierre(txtCodigo.Text)
            Case "Descuentos"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarDescuentos(txtCodigo.Text)
            Case "Factores"

        End Select

    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Select Case tbcAsesores.SelectedTab.Text
            Case "Asesores Comerciales"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case "Estadísticas"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarEstadistica()
            Case "Cierres"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarCierre(txtCodigo.Text)
            Case "Descuentos"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarDescuentos(txtCodigo.Text)
            Case "Factores"
        End Select

    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

        Select Case tbcAsesores.SelectedTab.Text
            Case "Asesores Comerciales"
                i_modo = movimiento.iAgregar
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
                IniciarAsesor(True)
            Case "Descuentos"
                Dim f As New jsVenArcAsesoresDescuentos
                f.Apuntador = nPosicionDes
                f.Agregar(myConn, ds, dtDescuentos, txtCodigo.Text, TipoVendedor.iFuerzaventa)
                ds = DataSetRequery(ds, strSQLDesc, myConn, nTablaDesc, lblInfo)
                IniciarDescuentos(txtCodigo.Text)
                f = Nothing
            Case "Factores"
                AgregarFactores()
        End Select


    End Sub
    Private Sub AgregarFactores()
        Select Case cmbFactores.SelectedIndex
            Case 0 'porcentajes comisión por rangos y días vencidos
                Dim gg As New jsVenArcAsesoresMovimientosPorcentajesComisionCob
                gg.Agregar(myConn, ds, dtFactores, txtCodigoFactores.Text)
                gg.Dispose()
                gg = Nothing
                IniciarComisionCobranza()
            Case 1 'Comisiones por jerarquía
                AgregarComisionesPorTipo(1)
                IniciarComision_X_Jerarquia()
            Case 2 'Cuotas por jerarquía
                AgregarComisionesPorTipo(2)
                IniciarCuotasJerarquias()
            Case 3
                Dim gg As New jsVenArcAsesoresMovimientosPorcentajesComisionCobJerarquias
                gg.Agregar(myConn, ds, dtFactores, txtCodigoFactores.Text)
                gg.Dispose()
                gg = Nothing
                IniciarComisionCobranzaJerarquiasYDiasVencidos()
        End Select
    End Sub
    Private Sub AgregarComisionesPorTipo(TipoFactor As Integer)

        Dim dtJerar As DataTable
        Dim nTableJer As String = "tblJer"
        ds = DataSetRequery(ds, " select * from jsmerencjer where id_emp = '" & jytsistema.WorkID & "' ", myConn, nTableJer, lblInfo)
        dtJerar = ds.Tables(nTableJer)

        For Each nRow As DataRow In dtJerar.Rows
            With nRow
                If ft.DevuelveScalarCadena(myConn, " select codven from jsvencomven where codven = '" & txtCodigoFactores.Text _
                                         & "' and tipjer = '" & .Item("tipjer") & "' and tipo = " & TipoFactor & " and id_emp = '" & jytsistema.WorkID & "'  ") = "" Then
                    InsertEditVENTASComisionesJerarquias(myConn, lblInfo, True, txtCodigoFactores.Text, .Item("tipjer"), 0.0, TipoFactor)
                End If
            End With
        Next
        dtJerar.Dispose()
        dtJerar = Nothing


    End Sub
    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Select Case tbcAsesores.SelectedTab.Text
            Case "Asesores Comerciales"
                i_modo = movimiento.iEditar
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
            Case "Descuentos"
                If dtDescuentos.Rows.Count > 0 Then
                    Dim f As New jsVenArcAsesoresDescuentos
                    f.Apuntador = Me.BindingContext(ds, nTablaDesc).Position
                    f.Editar(myConn, ds, dtDescuentos, txtCodigo.Text, TipoVendedor.iFuerzaventa)
                    ds = DataSetRequery(ds, strSQLDesc, myConn, nTablaDesc, lblInfo)
                    IniciarDescuentos(txtCodigo.Text)
                    f = Nothing
                End If
            Case "Factores"
                If dtFactores.Rows.Count > 0 Then
                    Select Case cmbFactores.SelectedIndex
                        Case 0
                            Dim gg As New jsVenArcAsesoresMovimientosPorcentajesComisionCob
                            gg.Apuntador = nPocisionFactores
                            gg.Editar(myConn, ds, dtFactores, txtCodigoFactores.Text)
                            nPocisionFactores = gg.Apuntador
                            AsignatxtFactores(nPocisionFactores, True)
                            gg.Dispose()
                            gg = Nothing
                        Case 3
                            Dim gg As New jsVenArcAsesoresMovimientosPorcentajesComisionCobJerarquias
                            gg.Apuntador = nPocisionFactores
                            gg.Editar(myConn, ds, dtFactores, txtCodigoFactores.Text)
                            nPocisionFactores = gg.Apuntador
                            AsignatxtFactores(nPocisionFactores, True)
                            gg.Dispose()
                            gg = Nothing
                    End Select

                End If
        End Select

    End Sub
    Private Sub AsignatxtFactores(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then
            Select Case cmbFactores.SelectedIndex
                Case 0 'Porcentajes de comisión por cobranza y días de vencimiento GENERAL
                    IniciarComisionCobranza()
                Case 1 'porcentaje por jerarquías
                    IniciarComision_X_Jerarquia()
                Case 2 'Cuotas Jerarquía
                    IniciarCuotasJerarquias()
                Case 3 'Porcentajes de comisión por cobranza y días de vencimiento POR JERARQUIAS
                    IniciarComisionCobranzaJerarquiasYDiasVencidos()
            End Select
        End If

        If c >= 0 AndAlso ds.Tables(nTablaFactores).Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaFactores).Position = c
            nPocisionFactores = c
            dgFactores.CurrentCell = dgFactores(0, c)
        End If

    End Sub
    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Select Case tbcAsesores.SelectedTab.Text
            Case "Asesores Comerciales"
                ft.MensajeCritico("En estos momentos no se puede eliminar una Asesor.Intente desactivarlo...")
            Case "Descuentos"
                EliminarDescuentos()
            Case "Factores"
                EliminarFactor(cmbFactores.SelectedIndex)
        End Select
    End Sub
    Private Sub EliminarFactor(TipoFactor As Integer)
        Select Case TipoFactor
            Case 0, 3
                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                Dim aCamposDel() As String = {"codven", "tipjer", "de", "a", "id_emp"}
                With dtFactores.Rows(nPocisionFactores)

                    Dim aStringsDel() As String = {.Item("codven"), .Item("tipjer"), .Item("de"), .Item("A"), jytsistema.WorkID}
                    sRespuesta = MsgBox(" ¿ Está seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                    If sRespuesta = MsgBoxResult.Yes Then
                        AsignatxtFactores(EliminarRegistros(myConn, lblInfo, ds, nTablaFactores, "jsvencomvencob", strSQLFactores, aCamposDel, aStringsDel, _
                                                            Me.BindingContext(ds, nTablaFactores).Position, True), True)

                    End If
                End With

        End Select
    End Sub

    Private Sub EliminarDescuentos()
        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        Dim aCamposDel() As String = {"coddes", "codven", "tipo", "id_emp"}
        Dim aStringsDel() As String = {dtDescuentos.Rows(nPosicionDes).Item("coddes"), txtCodigo.Text, TipoVendedor.iFuerzaventa, jytsistema.WorkID}
        sRespuesta = MsgBox(" ¿ Está seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
        If sRespuesta = MsgBoxResult.Yes Then

            EliminarRegistros(myConn, lblInfo, ds, nTablaDesc, "jsconcatdes", strSQLDesc, aCamposDel, aStringsDel, _
                                                Me.BindingContext(ds, nTablaDesc).Position, True)
            IniciarDescuentos(txtCodigo.Text)
        End If

    End Sub
    Private Sub EliminaCliente()
        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        Dim aCamposDel() As String = {"codven", "id_emp"}
        Dim aStringsDel() As String = {txtCodigo.Text, jytsistema.WorkID}
        sRespuesta = MsgBox(" ¿ Está seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
        If sRespuesta = MsgBoxResult.Yes Then
            If ft.DevuelveScalarEntero(myConn, " select COUNT(*) from jsmertramer where vendedor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsvencatven", strSQL, aCamposDel, aStringsDel, _
                                                Me.BindingContext(ds, nTabla).Position, True))
            Else
                ft.mensajeCritico("Este Asesor Comercial posee movimientos asociados. Verifique por favor ...")
            End If
        End If
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar

        Select Case tbcAsesores.SelectedTab.Text
            Case "Asesores Comerciales"
                Dim Campos() As String = {"codven", "descar", "apellidos", "nombres"}
                Dim Nombres() As String = {"Código", "Descripción", "Apellidos", "Nomnbres"}
                Dim Anchos() As Integer = {120, 250, 250, 250}
                f.Text = "Asesores Comerciales"
                f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Asesores de ventas...")
                nPosicionCat = f.Apuntador
                Me.BindingContext(ds, nTabla).Position = nPosicionCat
                AsignaTXT(nPosicionCat)
            Case "Movimientos CxC"
        End Select

        f = Nothing


    End Sub


    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

    End Sub

    Private Function Validado() As Boolean
        If txtApellidos.Text = "" Then
            ft.MensajeCritico("Debe indicar un Apellido Válido...")
            Return False
        End If

        If dtVendedores.Rows.Count = 0 And cmbClase.SelectedIndex = 1 Then
            ft.MensajeCritico("Este Supervisor no tiene asesores/vendedores asociados...")
            Return False
        End If

        If Not chkA.Checked And Not chkB.Checked And Not chkC.Checked _
            And Not chkD.Checked And Not chkE.Checked And Not chkF.Checked Then
            ft.MensajeCritico("Debe indicar una tarifa asignable por lo menos...")
            Return False
        End If

        Return True
    End Function

    Private Sub GuardarTXT()

        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionCat = ds.Tables(nTabla).Rows.Count
            insertEditVentasCuotasMercancias(myConn, lblInfo, txtCodigo.Text)

        End If

        InsertEditVENTASVendedor(myConn, lblInfo, Inserta, txtCodigo.Text, txtdescripcion.Text, txtApellidos.Text, txtNombre.Text, _
                                  "", "", "", "", 0.0, TipoVendedor.iFuerzaventa, "", txtCargo.Text, "", FechaIngreso, _
                                  0, ValorEntero(txtCarteraMercas.Text), ValorEntero(txtCarteraMarcas.Text), _
                                  IIf(chkA.Checked, 1, 0), IIf(chkB.Checked, 1, 0), IIf(chkC.Checked, 1, 0), _
                                  IIf(chkD.Checked, 1, 0), IIf(chkE.Checked, 1, 0), IIf(chkF.Checked, 1, 0), _
                                  ValorNumero(txtFactor.Text), cmbEstatus.SelectedIndex, "", "", 0.0, 0.0, _
                                  cmbClase.SelectedIndex)

        'Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        'sRespuesta = MsgBox(" ¿ Desea actualizar cuotas de mercancías a partir de las divisiones y jerarquías actuales ?", MsgBoxStyle.YesNo, "Actualización de cuotas ... ")
        'If sRespuesta = MsgBoxResult.Yes Then VerificaCuotasDeMercanciasPorDivisionYJerarquia(txtCodigo.Text)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionCat
        AsignaTXT(nPosicionCat)
        DesactivarMarco0()
        tbcAsesores.SelectedTab = C1DockingTabPage1
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
         txtNombre.GotFocus

        Select Case sender.name
            Case "txtCodigo"
                ft.mensajeEtiqueta(lblInfo, " Indique el código de Asesor comercial ... ", Transportables.TipoMensaje.iInfo)
            Case "cmbEstatus"
                ft.mensajeEtiqueta(lblInfo, " Sleccione estatus de asesor comercial ... ", Transportables.TipoMensaje.iInfo)
            Case "txtApellidos"
                ft.mensajeEtiqueta(lblInfo, " Indique los apellidos del asesor comercial ... ", Transportables.TipoMensaje.iInfo)
            Case "txtNombre"
                ft.mensajeEtiqueta(lblInfo, " Indique los nombres del asesor comercial ... ", Transportables.TipoMensaje.iInfo)
            Case "txtdescripcion"
                ft.mensajeEtiqueta(lblInfo, " Indique descripción del cargo ... ", Transportables.TipoMensaje.iInfo)
            Case "txtFactor"
                ft.mensajeEtiqueta(lblInfo, " Indique factor para el cálculo de cuotas en base a asignación fija ... ", Transportables.TipoMensaje.iInfo)
        End Select

    End Sub

    Private Sub tbcAsesores_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcAsesores.SelectedIndexChanged

        Select Case tbcAsesores.SelectedIndex
            Case 0 ' Asesores Comerciales
                AsignaTXT(nPosicionCat)
                ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            Case 1 ' Estadísticas
                ft.RellenaCombo(aEstadistica, cmbTipoEstadistica)
                MostrarItemsEnMenuBarra(MenuBarra, nPosicionCat, ds.Tables(nTabla).Rows.Count)
            Case 2 '' Cierres
                IniciarCierre(txtCodigo.Text)
            Case 3 'Descuentos
                IniciarDescuentos(txtCodigo.Text)
                MostrarItemsEnMenuBarra(MenuBarra, nPosicionDes, ds.Tables(nTabla).Rows.Count)
            Case 4 ' Factores
                IniciarFactores()
            Case 5
                strUnidad = ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM07")
                If strUnidad <> "" Then
                    aUnidad = {"MEDIDA PRINCIPAL (KGR)", "VENTAS", "MEDIDA SECUNDARIA (" & strUnidad & ")"}
                Else
                    aUnidad = {"MEDIDA PRINCIPAL (KGR)", "VENTA"}
                End If
                ft.RellenaCombo(aUnidad, cmbUNIDAD)
                IniciarTiposCuota()
        End Select
    End Sub
    Private Sub IniciarEstadistica()
        ft.RellenaCombo(aEstadistica, cmbTipoEstadistica)
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
        Dim nTableCuota As String = "tblCuota"
        ds = DataSetRequery(ds, CuotasAcumuladoAsesor(myConn, txtCodigo.Text, "cuota", cmbTipoEstadistica.SelectedIndex, cmbUnidadStat.SelectedIndex), myConn, nTableCuota, lblInfo)
        dtt = ds.Tables(nTableCuota)

        Dim dttAnteriores As New DataTable
        Dim nTableAcumulado As String = "tblAcumulado"
        ds = DataSetRequery(ds, CuotasAcumuladoAsesor(myConn, txtCodigo.Text, "acumulado", cmbTipoEstadistica.SelectedIndex, cmbUnidadStat.SelectedIndex), myConn, nTableAcumulado, lblInfo)
        dttAnteriores = ds.Tables(nTableAcumulado)

        aaY = ValoresMensuales(dtt)
        abY = ValoresMensuales(dttAnteriores)

        Dim aFFld() As String = {"id_emp"}
        Dim aSStr() As String = {jytsistema.WorkID}


        ay.Text = cmbUnidadStat.Text
        ax.Text = cmbTipoEstadistica.Text & " MES a MES "

        Histograma.ChartGroups(0).ChartData.SeriesList(0).Y.CopyDataIn(aaY)
        Histograma.ChartGroups(0).ChartData.SeriesList(1).Y.CopyDataIn(abY)

        Histograma.ChartGroups(0).ChartData.SeriesList(0).Label = "Cuota"
        Histograma.ChartGroups(0).ChartData.SeriesList(1).Label = "Acumulado"

        dtt = Nothing
        dttAnteriores = Nothing


    End Sub


    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If dt.Rows.Count = 0 Then
            IniciarAsesor(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            If Me.BindingContext(ds, nTabla).Position > 0 Then _
                nPosicionCat = Me.BindingContext(ds, nTabla).Position

            AsignaTXT(nPosicionCat)
        End If
        DesactivarMarco0()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub



    Private Sub AbrirEstadisticas()

        Dim aCam() As String = {"item", "descrip", "unidad", "cuota", "acumulado", "logro", "meta", "cierre", ""}
        Dim aNom() As String = {"Código", "Descripción", "UND", "cuota", "Acumulado", "% Logro", "Meta Diaria", "Cierre", ""}
        Dim aAnc() As Integer = {90, 350, 30, 120, 120, 60, 120, 120, 70}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}

        Dim aFor() As String = {"", "", "", IIf(cmbTipoEstadistica.SelectedIndex >= 5, sFormatoNumero, sFormatoCantidad), _
                                  IIf(cmbTipoEstadistica.SelectedIndex >= 5, sFormatoNumero, sFormatoCantidad), _
                                sFormatoNumero, _
                                IIf(cmbTipoEstadistica.SelectedIndex >= 5, sFormatoNumero, sFormatoCantidad), _
                                IIf(cmbTipoEstadistica.SelectedIndex >= 5, sFormatoNumero, sFormatoCantidad), ""}

        Dim nTablaStat As String = "tblEstadistica"
        ds = DataSetRequery(ds, SeleccionaEstadisticaAsesor(txtCodigo.Text, cmbTipoEstadistica.SelectedIndex, cmbUnidadStat.SelectedIndex), myConn, nTablaStat, lblInfo)
        dtStat = ds.Tables(nTablaStat)

        IniciarTabla(dgEstadistica, dtStat, aCam, aNom, aAnc, aAli, aFor)

        CalculaTotalesEstadistica()

        VerHistograma(C1Chart2, TipoDatoMercancia.iKilogramos)

    End Sub
    Private Sub CalculaTotalesEstadistica()

        txtTotalCuota.Left = dgEstadistica.Left + dgEstadistica.RowHeadersWidth + dgEstadistica.Columns(0).Width _
            + dgEstadistica.Columns(1).Width + dgEstadistica.Columns(2).Width
        txtTotalCuota.Width = dgEstadistica.Columns(3).Width

        txtTotalAcumulado.Left = txtTotalCuota.Left + dgEstadistica.Columns(3).Width
        txtTotalAcumulado.Width = dgEstadistica.Columns(4).Width

        txtTotalLogro.Left = txtTotalAcumulado.Left + dgEstadistica.Columns(4).Width
        txtTotalLogro.Width = dgEstadistica.Columns(5).Width

        txtTotalMeta.Left = txtTotalLogro.Left + dgEstadistica.Columns(5).Width
        txtTotalMeta.Width = dgEstadistica.Columns(6).Width

        txtTotalCierre.Left = txtTotalMeta.Left + dgEstadistica.Columns(6).Width
        txtTotalCierre.Width = dgEstadistica.Columns(7).Width

        Dim nCuota As Double = 0.0
        Dim nAcumulado As Double = 0.0
        Dim nCierre As Double = 0.0
        Dim nMeta As Double = 0.0
        For Each nRow As DataRow In dtStat.Rows
            With nRow
                nCuota += IIf(IsDBNull(.Item("cuota")), 0.0, .Item("cuota"))
                nAcumulado += IIf(IsDBNull(.Item("acumulado")), 0.0, .Item("acumulado"))
                nMeta += IIf(IsDBNull(.Item("meta")), 0.0, .Item("meta"))
                nCierre += IIf(IsDBNull(.Item("cierre")), 0.0, .Item("cierre"))
            End With
        Next

        txtTotalCuota.Text = ft.FormatoNumero(nCuota)
        txtTotalAcumulado.Text = ft.FormatoNumero(nAcumulado)
        txtTotalLogro.Text = ft.FormatoNumero(Math.Abs(nAcumulado) / IIf(nCuota = 0, Math.Abs(nAcumulado), nCuota) * 100)
        txtTotalMeta.Text = ft.FormatoNumero(nMeta)
        txtTotalCierre.Text = ft.FormatoNumero(nCierre)


    End Sub

    Private Function SeleccionaEstadisticaAsesor(ByVal CodigoAsesor As String, ByVal TipoEstadistica As Integer, _
                                                 ByVal UnidadesKilosMoneda As Integer) As String

        Dim aCampo() As String = {"nomart", "descrip", "descrip", "descrip", "descrip", "nombre", "nombre"}
        Dim aTabla() As String = {"jsmerctainv", "jsconctatab", "jsconctatab", "jsmercatdiv", "jsmerencjer", "jsvencatcli", "jsvencatcli"}
        Dim aLeftJoin() As String = {"codart", "codigo", "codigo", "division", "tipjer", "codcli", "codcli"}
        Dim aWhere() As String = {"", " b.modulo = '00002' and ", " b.modulo = '00003' and ", "", "", "", ""}

        Dim str As String = ""
        Dim strKGR As String = " SELECT a.item, b.nomart descrip, 'KGR' unidad, " _
                            & " IFNULL(a.cuota, 0.00) cuota, " _
                            & " IFNULL(a.acumulado, 0.00) acumulado, a.logro, " _
                            & " IFNULL(a.meta, 0.00) meta, " _
                            & " IFNULL(a.cierre, 0.00) cierre, b.grupo, b.marca, b.division, b.tipjer, a.id_emp " _
                            & " FROM jsvenstaven a " _
                            & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp)  " _
                            & " WHERE " _
                            & " a.codven = '" & txtCodigo.Text & "' AND " _
                            & " MONTH(a.fecha) = " & jytsistema.sFechadeTrabajo.Month & " AND " _
                            & " YEAR(a.fecha) = " & jytsistema.sFechadeTrabajo.Year & " AND " _
                            & " a.tipo = 0 AND " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " ORDER BY a.item "

        '& " (a.cuota  > 0 OR a.acumulado > 0)  AND " _

        Dim strUV As String = " SELECT a.item, b.nomart descrip, b.unidad unidad, " _
                            & " IF(a.cuota IS NULL, 0.00, a.cuota/b.pesounidad) cuota, " _
                            & " IF(a.acumulado IS NULL, 0.00, a.acumulado/b.pesounidad) acumulado, a.logro, " _
                            & " IF(a.meta IS NULL, 0.00, a.meta/b.pesounidad) meta, " _
                            & " IF(a.cierre IS NULL, 0.00, a.cierre/b.pesounidad) cierre " _
                            & " FROM jsvenstaven a " _
                            & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                            & " WHERE " _
                            & " a.codven = '" & txtCodigo.Text & "' AND " _
                            & " MONTH(a.fecha) = " & jytsistema.sFechadeTrabajo.Month & " AND " _
                            & " YEAR(a.fecha) = " & jytsistema.sFechadeTrabajo.Year & " AND " _
                            & " a.tipo = 0 AND " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " ORDER BY a.item "

        '        & " (a.cuota  > 0 OR a.acumulado > 0)  AND " _

        Dim strUMS As String = " SELECT a.item, b.nomart descrip, d.uvalencia unidad, " _
                            & " IF(a.cuota IS NULL, 0.00, a.cuota*d.equivale/b.pesounidad) cuota, " _
                            & " IF(a.acumulado IS NULL, 0.00, a.acumulado*d.equivale/b.pesounidad) acumulado, a.logro, " _
                            & " IF(a.meta IS NULL, 0.00, a.meta*d.equivale/b.pesounidad) meta, " _
                            & " IF(a.cierre IS NULL, 0.00, a.cierre*d.equivale/b.pesounidad) cierre, b.grupo, b.marca, b.division, b.tipjer, a.id_emp " _
                            & " FROM jsvenstaven a " _
                            & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                            & " LEFT JOIN (SELECT a.codart, a.unidad, a.equivale, a.uvalencia, a.id_emp  " _
                            & "            FROM jsmerequmer a " _
                            & "            WHERE " _
                            & "            a.id_emp = '" & jytsistema.WorkID & "' " _
                            & "            UNION " _
                            & "            SELECT a.codart, a.unidad, 1 equivale, a.unidad uvalencia, a.id_emp " _
                            & "            FROM jsmerctainv a " _
                            & "            WHERE " _
                            & "            a.id_emp = '" & jytsistema.WorkID & "') d  ON (a.item = d.codart AND d.uvalencia = '" & ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM07") & "' AND a.id_emp = d.id_emp ) " _
                            & " WHERE " _
                            & " a.codven = '" & txtCodigo.Text & "' AND " _
                            & " MONTH(a.fecha) = " & jytsistema.sFechadeTrabajo.Month & " AND " _
                            & " YEAR(a.fecha) = " & jytsistema.sFechadeTrabajo.Year & " AND " _
                            & " a.tipo = 0 AND " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " ORDER BY a.item "

        '& " (a.cuota  > 0 OR a.acumulado > 0)  AND " _
        Select Case TipoEstadistica
            Case 0 'MERCANCIAS
                Select Case UnidadesKilosMoneda
                    Case 0  'KGR
                        str = strKGR
                    Case 1   'UV
                        str = strUV
                    Case 2   'UMS
                        str = strUMS
                End Select
            Case 1 'CATEGORIAS
                If UnidadesKilosMoneda = 0 Then 'KGR
                    str = "  SELECT a.grupo item, b.descrip, a.unidad, SUM(a.cuota) cuota, SUM(a.acumulado) acumulado, " _
                        & " SUM(a.acumulado)/IF(SUM(a.cuota) = 0, SUM(a.acumulado), SUM(a.cuota))  *100 logro, SUM(a.meta) meta, SUM(a.cierre) cierre  " _
                        & " FROM (" & strKGR & ")  a " _
                        & " LEFT JOIN jsconctatab b ON (a.grupo = b.codigo AND a.id_emp = b.id_emp AND b.modulo = '00002') " _
                        & " GROUP BY a.grupo "
                Else 'UMS
                    str = "  SELECT a.grupo item, b.descrip, a.unidad, SUM(a.cuota) cuota, SUM(a.acumulado) acumulado, " _
                        & " SUM(a.acumulado)/IF(SUM(a.cuota) = 0, SUM(a.acumulado), SUM(a.cuota))*100 logro, SUM(a.meta) meta, SUM(a.cierre) cierre  " _
                        & " FROM (" & strUMS & ")  a " _
                        & " LEFT JOIN jsconctatab b ON (a.grupo = b.codigo AND a.id_emp = b.id_emp AND b.modulo = '00002') " _
                        & " GROUP BY a.grupo "
                End If
            Case 2 'MARCAS
                If UnidadesKilosMoneda = 0 Then 'KGR
                    str = "  SELECT a.marca item, b.descrip, a.unidad, SUM(a.cuota) cuota, SUM(a.acumulado) acumulado, " _
                        & " SUM(a.acumulado)/IF(SUM(a.cuota) = 0, SUM(a.acumulado), SUM(a.cuota))  *100 logro, SUM(a.meta) meta, SUM(a.cierre) cierre  " _
                        & " FROM (" & strKGR & ")  a " _
                        & " LEFT JOIN jsconctatab b ON (a.marca = b.codigo AND a.id_emp = b.id_emp AND b.modulo = '00003') " _
                        & " GROUP BY a.marca "
                Else 'UMS
                    str = "  SELECT a.marca item, b.descrip, a.unidad, SUM(a.cuota) cuota, SUM(a.acumulado) acumulado, " _
                        & " SUM(a.acumulado)/IF(SUM(a.cuota) = 0, SUM(a.acumulado), SUM(a.cuota))*100 logro, SUM(a.meta) meta, SUM(a.cierre) cierre  " _
                        & " FROM (" & strUMS & ")  a " _
                        & " LEFT JOIN jsconctatab b ON (a.marca = b.codigo AND a.id_emp = b.id_emp AND b.modulo = '00003') " _
                        & " GROUP BY a.marca "
                End If
            Case 3 'DIVISIONES
                If UnidadesKilosMoneda = 0 Then 'KGR
                    str = "  SELECT a.division item, b.descrip, a.unidad, SUM(a.cuota) cuota, SUM(a.acumulado) acumulado, " _
                        & " SUM(a.acumulado)/IF(SUM(a.cuota) = 0, SUM(a.acumulado), SUM(a.cuota))  *100 logro, SUM(a.meta) meta, SUM(a.cierre) cierre  " _
                        & " FROM (" & strKGR & ")  a " _
                        & " LEFT JOIN jsmercatdiv b ON (a.division = b.division AND a.id_emp = b.id_emp) " _
                        & " GROUP BY a.division "
                Else 'UMS
                    str = "  SELECT a.division item, b.descrip, a.unidad, SUM(a.cuota) cuota, SUM(a.acumulado) acumulado, " _
                        & " SUM(a.acumulado)/IF(SUM(a.cuota) = 0, SUM(a.acumulado), SUM(a.cuota))*100 logro, SUM(a.meta) meta, SUM(a.cierre) cierre  " _
                        & " FROM (" & strUMS & ")  a " _
                        & " LEFT JOIN jsmercatdiv b ON (a.division = b.division AND a.id_emp = b.id_emp ) " _
                        & " GROUP BY a.division "
                End If
            Case 4 'JERARQUIAS
                If UnidadesKilosMoneda = 0 Then 'KGR
                    str = "  SELECT a.TIPJER item, b.descrip, a.unidad, SUM(a.cuota) cuota, SUM(a.acumulado) acumulado, " _
                        & " SUM(a.acumulado)/IF(SUM(a.cuota) = 0, SUM(a.acumulado), SUM(a.cuota))  *100 logro, SUM(a.meta) meta, SUM(a.cierre) cierre  " _
                        & " FROM (" & strKGR & ")  a " _
                        & " LEFT JOIN jsmerencjer b ON (a.tipjer = b.tipjer AND a.id_emp = b.id_emp ) " _
                        & " GROUP BY a.tipjer "
                Else 'UMS
                    str = "  SELECT a.tipjer item, b.descrip, a.unidad, SUM(a.cuota) cuota, SUM(a.acumulado) acumulado, " _
                        & " SUM(a.acumulado)/IF(SUM(a.cuota) = 0, SUM(a.acumulado), SUM(a.cuota))*100 logro, SUM(a.meta) meta, SUM(a.cierre) cierre  " _
                        & " FROM (" & strUMS & ")  a " _
                        & " LEFT JOIN jsmerencjer b ON (a.tipjer = b.tipjer AND a.id_emp = b.id_emp) " _
                        & " GROUP BY a.tipjer "
                End If
            Case 5, 6

                str = " SELECT a.item, b.nombre descrip, 'BsF' unidad, IF(a.cuota IS NULL, 0.00, a.cuota) cuota," _
                    & " IF(a.acumulado IS NULL, 0.00, a.acumulado) acumulado, a.logro, IF(a.meta IS NULL, 0.00, a.meta) meta, " _
                    & " IF(a.cierre IS NULL, 0.00, a.cierre) cierre " _
                    & " FROM jsvenstaven a " _
                    & " LEFT JOIN jsvencatcli b ON (a.item = b.codcli AND a.id_emp = b.id_emp) " _
                    & " WHERE " _
                    & " (a.cuota  > 0 OR a.acumulado > 0)  AND " _
                    & " a.codven = '" & CodigoAsesor & "' AND " _
                    & " MONTH(a.fecha) = " & jytsistema.sFechadeTrabajo.Month & " AND " _
                    & " YEAR(a.fecha) = " & jytsistema.sFechadeTrabajo.Year & " AND " _
                    & " a.tipo = " & TipoEstadistica & " AND " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " ORDER BY a.item "

        End Select

        SeleccionaEstadisticaAsesor = str


    End Function

    Private Sub cmbTipoEstadistica_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipoEstadistica.SelectedIndexChanged

        Select Case cmbTipoEstadistica.SelectedIndex
            Case 0
                strUnidad = ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM07")
                If strUnidad <> "" Then
                    aUnidad = {"MEDIDA PRINCIPAL (KGR)", "VENTAS", "MEDIDA SECUNDARIA (" & strUnidad & ")"}
                Else
                    aUnidad = {"MEDIDA PRINCIPAL (KGR)", "VENTA"}
                End If
            Case 1, 2, 3, 4
                strUnidad = ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM07")
                If strUnidad <> "" Then
                    aUnidad = {"MEDIDA PRINCIPAL (KGR)", "MEDIDA SECUNDARIA (" & strUnidad & ")"}
                Else
                    aUnidad = {"MEDIDA PRINCIPAL (KGR)"}
                End If
            Case 5, 6
                aUnidad = {"MONETARIOS"}
        End Select
        ft.RellenaCombo(aUnidad, cmbUnidadStat)

        If cmbUnidadStat.Items.Count > 0 Then AbrirEstadisticas()

    End Sub

    Private Sub txtNombre_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNombre.TextChanged
        txtNombreFactores.Text = txtNombre.Text
    End Sub

    Private Sub txtCargo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCargo.TextChanged
        If txtCargo.Text <> "" Then txtEstructura.Text = ft.DevuelveScalarCadena(myConn, " select nombre from jsnomestcar where codigo = " & txtCargo.Text.Split(".")(UBound(txtCargo.Text.Split("."))) & " ")
    End Sub

    Private Sub cmbFactores_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFactores.SelectedIndexChanged
        Select Case cmbFactores.SelectedIndex
            Case 0 'Porcentajes de comisión por cobranza y días de vencimiento GENERAL
                IniciarComisionCobranza()
            Case 1 'porcentaje por jerarquías
                IniciarComision_X_Jerarquia()
            Case 2 'Cuotas Jerarquía
                IniciarCuotasJerarquias()
            Case 3 'Porcentajes de comisión por cobranza y días de vencimiento POR JERARQUIAS
                IniciarComisionCobranzaJerarquiasYDiasVencidos()
        End Select
    End Sub
    Private Sub IniciarComisionCobranza()
        If txtCodigo.Text <> "" Then

            strSQLFactores = " select a.* from jsvencomvencob a " _
            & " where " _
            & " a.codven = '" & txtCodigo.Text & "' and " _
            & " a.tipjer = 'XX' and id_emp = '" & jytsistema.WorkID & "' order by tipjer, de "

            ds = DataSetRequery(ds, strSQLFactores, myConn, nTablaFactores, lblInfo)
            dtFactores = ds.Tables(nTablaFactores)

            Dim aCam() As String = {"de", "a", "por_cobranza", ""}
            Dim aNom() As String = {"Días vencidos desde", "Días vencidos hasta", "% Comisión", ""}
            Dim aAnc() As Integer = {90, 90, 90, 90}
            Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                     AlineacionDataGrid.Izquierda}
            Dim aFor() As String = {sFormatoEntero, sFormatoEntero, sFormatoNumero, ""}

            IniciarTabla(dgFactores, dtFactores, aCam, aNom, aAnc, aAli, aFor)
            If dtFactores.Rows.Count > 0 Then nPocisionFactores = 0

        End If
    End Sub

    Private Sub IniciarComisionCobranzaJerarquiasYDiasVencidos()
        If txtCodigo.Text <> "" Then

            strSQLFactores = " select a.*, b.descrip from jsvencomvencob a " _
                & " left join jsmerencjer b on (a.tipjer = b.tipjer and a.id_emp = b.id_emp) " _
            & " where " _
            & " a.codven = '" & txtCodigo.Text & "' and " _
            & " a.tipjer <> 'XX' and a.id_emp = '" & jytsistema.WorkID & "' order by a.tipjer, a.de "

            ds = DataSetRequery(ds, strSQLFactores, myConn, nTablaFactores, lblInfo)
            dtFactores = ds.Tables(nTablaFactores)

            Dim aCam() As String = {"tipjer", "descrip", "de", "a", "por_cobranza", ""}
            Dim aNom() As String = {"Jerarquía", "Descripción", "Días vencidos desde", "Días vencidos hasta", "% Comisión", ""}
            Dim aAnc() As Integer = {70, 150, 90, 90, 90, 90}
            Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                     AlineacionDataGrid.Izquierda}
            Dim aFor() As String = {"", "", sFormatoEntero, sFormatoEntero, sFormatoNumero, ""}

            IniciarTabla(dgFactores, dtFactores, aCam, aNom, aAnc, aAli, aFor)
            If dtFactores.Rows.Count > 0 Then nPocisionFactores = 0

        End If
    End Sub

    Private Sub IniciarComision_X_Jerarquia()
        If txtCodigo.Text <> "" Then

            strSQLFactores = " select a.tipjer, b.descrip, a.por_ventas " _
            & " from jsvencomven a " _
            & " left join jsmerencjer b on (a.tipjer = b.tipjer and a.id_emp = b.id_emp) " _
            & " where " _
            & " a.tipo = 1 AND " _
            & " a.codven = '" & txtCodigo.Text & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' "

            ds = DataSetRequery(ds, strSQLFactores, myConn, nTablaFactores, lblInfo)
            dtFactores = ds.Tables(nTablaFactores)

            Dim aCam() As String = {"tipjer", "descrip", "por_ventas", ""}
            Dim aNom() As String = {"Jerarquía", "", "% Comisión", ""}
            Dim aAnc() As Integer = {80, 300, 120, 90}
            Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, _
                                     AlineacionDataGrid.Izquierda}
            Dim aFor() As String = {"", "", sFormatoNumero, ""}

            IniciarTabla(dgFactores, dtFactores, aCam, aNom, aAnc, aAli, aFor, , True)
            If dtFactores.Rows.Count > 0 Then nPocisionFactores = 0

            dgFactores.ReadOnly = False
            dgFactores.Columns("tipjer").ReadOnly = True
            dgFactores.Columns("descrip").ReadOnly = True
            dgFactores.Columns("").ReadOnly = True

        End If
    End Sub

    Private Sub IniciarCuotasJerarquias()
        If txtCodigo.Text <> "" Then

            strSQLFactores = " select a.tipjer, b.descrip, a.por_ventas " _
            & " from jsvencomven a " _
            & " left join jsmerencjer b on (a.tipjer = b.tipjer and a.id_emp = b.id_emp) " _
            & " where " _
            & " a.tipo = 2 AND " _
            & " a.codven = '" & txtCodigo.Text & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' "

            ds = DataSetRequery(ds, strSQLFactores, myConn, nTablaFactores, lblInfo)
            dtFactores = ds.Tables(nTablaFactores)

            Dim aCam() As String = {"tipjer", "descrip", "por_ventas", ""}
            Dim aNom() As String = {"Jerarquía", "", "Cuotas", ""}
            Dim aAnc() As Integer = {80, 300, 120, 90}
            Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, _
                                     AlineacionDataGrid.Izquierda}
            Dim aFor() As String = {"", "", sFormatoNumero, ""}

            IniciarTabla(dgFactores, dtFactores, aCam, aNom, aAnc, aAli, aFor, , True)
            If dtFactores.Rows.Count > 0 Then nPocisionFactores = 0

            dgFactores.ReadOnly = False
            dgFactores.Columns("tipjer").ReadOnly = True
            dgFactores.Columns("descrip").ReadOnly = True
            dgFactores.Columns("").ReadOnly = True

        End If
    End Sub


    Private Sub btnCargo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCargo.Click
        Dim f As New jsNomArcCargos
        f.CodigoCargo = txtCargo.Text
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        txtCargo.Text = f.CodigoCargo
        f = Nothing
    End Sub



    Private Sub VerificaCuotasDeMercanciasPorDivisionYJerarquia(ByVal CodigoVendedor As String)
        Dim dtMercas As DataTable
        Dim strSQLMercas As String = " select * from jsmerctainv where id_emp = '" & jytsistema.WorkID & "' order by codart "
        Dim nTablaMercas As String = "tblMercas"

        ds = DataSetRequery(ds, strSQLMercas, myConn, nTablaMercas, lblInfo)
        dtMercas = ds.Tables(nTablaMercas)
        If dtMercas.Rows.Count > 0 Then

            'EsperaPorFavor()

            Dim jCont As Integer
            For jCont = 0 To dtMercas.Rows.Count - 1
                With dtMercas.Rows(jCont)
                    Dim aCam() As String = {"codart", "id_emp"}
                    Dim aStr() As String = {.Item("codart"), jytsistema.WorkID}

                    If CInt(.Item("estatus")) = 1 AndAlso CInt(.Item("cuota")) = 1 Then

                        Dim aCamD() As String = {"codven", "division", "id_emp"}
                        Dim aStrD() As String = {txtCodigo.Text, .Item("division"), jytsistema.WorkID}
                        Dim aCamJ() As String = {"codven", "tipjer", "id_emp"}
                        Dim aStrJ() As String = {txtCodigo.Text, .Item("tipjer"), jytsistema.WorkID}

                        If qFound(myConn, lblInfo, "jsvencatvendiv", aCamD, aStrD) AndAlso qFound(myConn, lblInfo, "jsvencatvenjer", aCamJ, aStrJ) Then

                            Dim aCamC() As String = {"codven", "codart", "id_emp"}
                            Dim aStrC() As String = {txtCodigo.Text, .Item("codart"), jytsistema.WorkID}
                            If Not qFound(myConn, lblInfo, "jsvencuoart", aCamC, aStrC) Then

                                Dim CuotaENE As Double = ft.DevuelveScalarDoble(myConn, " select mes01 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ") * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaFEB As Double = ft.DevuelveScalarDoble(myConn, " select mes02 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ") * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaMAR As Double = ft.DevuelveScalarDoble(myConn, " select mes03 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ") * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaABR As Double = ft.DevuelveScalarDoble(myConn, " select mes04 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ") * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaMAY As Double = ft.DevuelveScalarDoble(myConn, " select mes05 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ") * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaJUN As Double = ft.DevuelveScalarDoble(myConn, " select mes06 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ") * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaJUL As Double = ft.DevuelveScalarDoble(myConn, " select mes07 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ") * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaAGO As Double = ft.DevuelveScalarDoble(myConn, " select mes08 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ") * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaSEP As Double = ft.DevuelveScalarDoble(myConn, " select mes09 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ") * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaOCT As Double = ft.DevuelveScalarDoble(myConn, " select mes10 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ") * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaNOV As Double = ft.DevuelveScalarDoble(myConn, " select mes11 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ") * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaDIC As Double = ft.DevuelveScalarDoble(myConn, " select mes12 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ") * ValorNumero(txtFactor.Text) / 100

                                ft.Ejecutar_strSQL(myconn, " insert into jsvencuoart set codven = '" & txtCodigo.Text & "', codart = '" & .Item("codart") & "', " _
                                               & " esmes01 = " & CuotaENE & ", esmes02 = " & CuotaFEB & ", esmes03 = " & CuotaMAR & ", " _
                                               & " esmes04 = " & CuotaABR & ", esmes05 = " & CuotaMAY & ", esmes06 = " & CuotaJUN & ", " _
                                               & " esmes07 = " & CuotaJUL & ", esmes08 = " & CuotaAGO & ", esmes09 = " & CuotaSEP & ", " _
                                               & " esmes10 = " & CuotaOCT & ", esmes11 = " & CuotaNOV & ", esmes12 = " & CuotaDIC & ", ejercicio = '" & jytsistema.WorkExercise & "', id_emp = '" & jytsistema.WorkID & "' ")

                            End If
                        Else
                            ft.Ejecutar_strSQL(myConn, " delete from jsvencuoart where codven = '" & txtCodigo.Text & "' and codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        End If
                    Else
                        ft.Ejecutar_strSQL(myConn, " delete from jsvencuoart where codven = '" & txtCodigo.Text & "' and codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    End If
                End With

            Next
        End If


    End Sub
    Private Sub JerarquiasAñadidas(ByVal CodigoVendedor As String)

        ft.Ejecutar_strSQL(myconn, " delete from jsvencatvenjer where codven = '" & CodigoVendedor & "' and id_emp = '" & jytsistema.WorkID & "'  ")

        ft.Ejecutar_strSQL(myconn, " replace into jsvencatvenjer SELECT '" & CodigoVendedor & "' codven, a.tipjer, a.id_emp " _
                               & " FROM jsmerctainv a " _
                               & " WHERE " _
                               & " a.division IN (SELECT division FROM jsvencatvendiv WHERE codven = '" & CodigoVendedor & "' AND id_emp = '" & jytsistema.WorkID & "') AND  " _
                               & " a.estatus = 1 and " _
                               & " a.cuota = 1 and " _
                               & " a.id_emp = '" & jytsistema.WorkID & "' " _
                               & " GROUP BY a.tipjer ")

    End Sub
    Private Sub DivisionesAñadidas(ByVal CodigoVendedor As String)

        ft.Ejecutar_strSQL(myconn, " delete from jsvencatvendiv where codven = '" & CodigoVendedor & "' and id_emp = '" & jytsistema.WorkID & "'  ")

        ft.Ejecutar_strSQL(myconn, " replace into jsvencatvendiv SELECT '" & CodigoVendedor & "' codven, a.division, a.id_emp " _
                               & " FROM jsmerctainv a " _
                               & " WHERE " _
                               & " a.tipjer IN (SELECT tipjer FROM jsvencatvenjer WHERE codven = '" & CodigoVendedor & "' AND id_emp = '" & jytsistema.WorkID & "') AND  " _
                               & " a.estatus = 1 and " _
                               & " a.cuota = 1 and " _
                               & " a.id_emp = '" & jytsistema.WorkID & "' " _
                               & " GROUP BY a.division ")
    End Sub
    Private Sub btnAgregarDivision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarDivision.Click

        Dim dtDiv As DataTable
        Dim strSQLDiv As String = " select a.division codigo, a.descrip descripcion from jsmercatdiv a where a.id_emp = '" & jytsistema.WorkID & "' order by a.division "
        Dim nTableDiv As String = "tblDiv"
        ds = DataSetRequery(ds, strSQLDiv, myConn, nTableDiv, lblInfo)
        dtDiv = ds.Tables(nTableDiv)

        Dim f As New jsControlArcTablaSimpleSeleccion
        f.Seleccion = CopyDataTableColumnToArray(dtDivition, "division")

        ft.Ejecutar_strSQL(myConn, " delete from jsvencatvendiv where codven = '" & txtCodigo.Text & "' and  id_emp = '" & jytsistema.WorkID & "'  ")
        AbrirDivisiones(txtCodigo.Text)

        f.Cargar("Divisiones", ds, dtDiv)

        Dim jCont As Integer
        Dim arrList As ArrayList = f.Seleccion
        For jCont = 0 To arrList.Count - 1
            ft.Ejecutar_strSQL(myConn, " replace into jsvencatvendiv set codven = '" & txtCodigo.Text & "', division = '" & arrList.Item(jCont) & "', id_emp = '" & jytsistema.WorkID & "'  ")
        Next

        'JerarquiasAñadidas(txtCodigo.Text)

        AbrirDivisiones(txtCodigo.Text)
        AbrirJerarquias(txtCodigo.Text)


        f.Dispose()
        f = Nothing

        dtDiv.Dispose()
        dtDiv = Nothing

    End Sub
    Private Sub btnAgregaJerarquia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaJerarquia.Click
        Dim dtJer As DataTable
        Dim strSQLJer As String = " select a.tipjer codigo, a.descrip descripcion from jsmerencjer a where a.id_emp = '" & jytsistema.WorkID & "' order by a.tipjer "
        Dim nTablejer As String = "tbljer"
        ds = DataSetRequery(ds, strSQLJer, myConn, nTablejer, lblInfo)
        dtJer = ds.Tables(nTablejer)

        Dim f As New jsControlArcTablaSimpleSeleccion
        f.Seleccion = CopyDataTableColumnToArray(dtHierarchy, "tipjer")

        ft.Ejecutar_strSQL(myconn, " delete from jsvencatvenjer where codven = '" & txtCodigo.Text & "' and  id_emp = '" & jytsistema.WorkID & "'  ")
        AbrirJerarquias(txtCodigo.Text)

        f.Cargar("Jerarquías", ds, dtJer)

        Dim jCont As Integer
        Dim arrList As ArrayList = f.Seleccion
        For jCont = 0 To arrList.Count - 1
            ft.Ejecutar_strSQL(myconn, " replace into jsvencatvenjer set codven = '" & txtCodigo.Text & "', tipjer = '" & arrList.Item(jCont) & "', id_emp = '" & jytsistema.WorkID & "'  ")
        Next

        DivisionesAñadidas(txtCodigo.Text)

        AbrirDivisiones(txtCodigo.Text)
        AbrirJerarquias(txtCodigo.Text)


        f.Dispose()
        f = Nothing

        dtJer.Dispose()
        dtJer = Nothing

    End Sub
    Private Sub IniciarTiposCuota()

        ft.RellenaCombo(aCuotasTipo, cmbCuota)

    End Sub

    Private Sub IniciarCuotas(KilosUnidadCajas As Integer, TipoCuota As Integer, CodigoFiltro As String)

        '0 = Kilos ; 1 = Unidad de Venta ; 2 = Cajas

        Dim strLeft As String = ""
        Dim strLeftD As String = " LEFT JOIN (SELECT a.codart, a.unidad, a.equivale, a.uvalencia, a.id_emp " _
                    & " FROM jsmerequmer a " _
                    & " WHERE " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " UNION " _
                    & " SELECT a.codart, a.unidad, 1 equivale, a.unidad uvalencia, a.id_emp " _
                    & " FROM jsmerctainv a " _
                    & " WHERE " _
                    & " a.id_emp = '" & jytsistema.WorkID & "') d  ON (a.codart = d.codart AND d.uvalencia = '" & ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM07") & "' AND a.id_emp = d.id_emp ) "
        Dim strWhere As String = ""
        Dim strXX As String = ""

        Select Case KilosUnidadCajas
            Case 1
                strXX = "/a.pesounidad "
            Case 2
                strXX = "*d.equivale/a.pesounidad"
        End Select


        Select Case TipoCuota
            Case 1 'CATEGORIAS
                strLeft = " left join jsconctatab c on (a.grupo = c.codigo and a.id_emp = c.id_emp and c.modulo = '00002') "
                If CodigoFiltro <> "" And CodigoFiltro <> "00000" Then strWhere = " a.grupo = '" & CodigoFiltro & "' and "
            Case 2 'MARCAS
                strLeft = " left join jsconctatab c on (a.marca = c.codigo and a.id_emp = c.id_emp and c.modulo = '00003') "
                If CodigoFiltro <> "" And CodigoFiltro <> "00000" Then strWhere = " a.marca = '" & CodigoFiltro & "' and "
            Case 3 'DIVISIONES
                strLeft = " left join jsmercatdiv c on (a.division = c.division and a.id_emp = c.id_emp) "
                If CodigoFiltro <> "" And CodigoFiltro <> "00000" Then strWhere = " c.division = '" & CodigoFiltro & "' and "
            Case 4 'JERARQUIAS
                strLeft = " left join jsmerencjer c on (a.tipjer = c.tipjer and a.id_emp = c.id_emp) "
                If CodigoFiltro <> "" And CodigoFiltro <> "00000" Then strWhere = " a.tipjer = '" & CodigoFiltro & "' and "

        End Select

        strSQLCuotas = " select a.CODART, a.NOMART, if(" & KilosUnidadCajas & " = 0, 'KGR', if(" & KilosUnidadCajas & " = 1, a.unidad, d.uvalencia ) )  unidad, a.CODJER, a.TIPJER, a.EXISTE_ACT existencia, " _
            & " b.ESMES01" & strXX & " MES01, b.ESMES02" & strXX & " MES02, b.ESMES03" & strXX & " MES03, " _
            & " b.ESMES04" & strXX & " MES04, b.ESMES05" & strXX & " MES05, b.ESMES06" & strXX & " MES06, " _
            & " b.ESMES07" & strXX & " MES07, b.ESMES08" & strXX & " MES08, b.ESMES09" & strXX & " MES09, " _
            & " b.ESMES10" & strXX & " MES10, b.ESMES11" & strXX & " MES11, b.ESMES12" & strXX & " MES12,  " _
            & " (b.ESMES01 + b.ESMES02 + b.ESMES03 + " _
            & " b.ESMES04 + b.ESMES05 + b.ESMES06 + " _
            & " b.ESMES07 + b.ESMES08 + b.ESMES09 + " _
            & " b.ESMES10 + b.ESMES11 + b.ESMES12)" & strXX & "  total " _
            & " from jsmerctainv a " _
            & " left join jsvencuoart b on (a.codart = b.codart and a.id_emp = b.id_emp ) " _
            & strLeft _
            & strLeftD _
            & " where " _
            & strWhere _
            & " b.codven = '" & txtCodigo.Text & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' order by a.CODART "

        ds = DataSetRequery(ds, strSQLCuotas, myConn, nTablaCuotas, lblInfo)
        dtCuotas = ds.Tables(nTablaCuotas)

        IniciarGrilla()

        ' AsignarTooltips()
        If dtCuotas.Rows.Count > 0 Then nPosicionCuo = 0

    End Sub

    Private Sub IniciarGrilla()

        Dim aCam() As String = {"codart", "nomart", "unidad", "mes01", "mes02", "mes03", "mes04", "mes05", "mes06", "mes07", "mes08", "mes09", "mes10", "mes11", "mes12", "total"}
        Dim aNom() As String = {"Código", "Descripción", "UND", "ENE", "FEB", "MAR", "ABR", "MAY", "JUN", "JUL", "AGO", "SEP", "OCT", "NOV", "DIC", "TOTAL"}
        Dim aAnc() As Integer = {60, 300, 30, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                 AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                 AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}

        Dim aFor() As String = {"", "", "", sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad}

        IniciarTabla(dgCuotas, dtCuotas, aCam, aNom, aAnc, aAli, aFor, , True, 8)
        If dtCuotas.Rows.Count > 0 Then nPosicionCuo = 0

        dgCuotas.ReadOnly = False
        dgCuotas.Columns("codart").ReadOnly = True
        dgCuotas.Columns("nomart").ReadOnly = True
        dgCuotas.Columns("unidad").ReadOnly = True
        dgCuotas.Columns("total").ReadOnly = True

    End Sub

    Private Sub cmbCuota_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCuota.SelectedIndexChanged
        If cmbCuota.Items.Count > 0 Then AbrirTipoCuota(cmbCuota.SelectedIndex)
    End Sub
    Private Sub AbrirTipoCuota(TipoCuota As Integer)
        Dim nTablaFiltro As String = "tblfiltro" & ft.NumeroAleatorio(100000)
        Dim dtFilto As DataTable
        Dim strSQLFiltr As String = " select '00000' codigo, ' TODAS' descrip from jsmercatalm where codalm = '00001' and id_emp = '" & jytsistema.WorkID & "' "
        Dim strSQLFiltro As String = ""
        Select Case TipoCuota
            Case 0
                strSQLFiltro = strSQLFiltr
            Case 1 'CATEGORIAS
                strSQLFiltro = " select codigo, descrip from jsconctatab where " _
                    & " modulo = '00002' and " _
                    & " id_emp = '" & jytsistema.WorkID & "' " _
                    & " UNION " _
                    & strSQLFiltr _
                    & " order by descrip "
            Case 2 'MARCAS
                strSQLFiltro = " select codigo, descrip from jsconctatab where " _
                    & " modulo = '00003' and " _
                    & " id_emp = '" & jytsistema.WorkID & "' " _
                    & " UNION " _
                    & strSQLFiltr _
                    & " order by descrip "
            Case 3 'DIVISIONES
                strSQLFiltro = " select DIVISION codigo, descrip from jsmercatdiv where " _
                    & " id_emp = '" & jytsistema.WorkID & "' " _
                    & " UNION " _
                    & strSQLFiltr _
                    & " order by descrip "
            Case 4 'JERARQUIAS
                strSQLFiltro = " select TIPJER codigo, descrip from jsmerencjer where " _
                     & " id_emp = '" & jytsistema.WorkID & "' " _
                     & " UNION " _
                     & strSQLFiltr _
                     & " order by descrip "
        End Select
        ds = DataSetRequery(ds, strSQLFiltro, myConn, nTablaFiltro, lblInfo)
        dtFilto = ds.Tables(nTablaFiltro)

        RellenaComboConDatatable(cmbFiltro, dtFilto, "descrip", "codigo")

    End Sub

    Private Sub dgFactores_CellEndEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgFactores.CellEndEdit
        dgFactores.Rows(e.RowIndex).ErrorText = String.Empty
        '' ft.mensajeInformativo("Puede indicar valor en para modificar")
    End Sub



    Private Sub dgFactores_CellValidated(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgFactores.CellValidated
        If cmbFactores.SelectedIndex >= 1 Then
            Select Case dgFactores.CurrentCell.ColumnIndex
                Case 2
                    InsertEditVENTASComisionesJerarquias(myConn, lblInfo, False, txtCodigoFactores.Text, dgFactores.CurrentRow.Cells(0).Value, _
                                CDbl(dgFactores.CurrentCell.Value), cmbFactores.SelectedIndex)
            End Select

            'Select Case cmbFactores.SelectedIndex
            '    Case 1 'porcentaje por jerarquías
            '        IniciarComision_X_Jerarquia()
            '    Case 2 'Cuotas Jerarquía
            '        IniciarCuotasJerarquias()
            '    Case 3 'Porcentajes de comisión por cobranza y días de vencimiento POR JERARQUIAS
            '        IniciarComisionCobranzaJerarquiasYDiasVencidos()
            'End Select

        End If
    End Sub

    Private Sub dgFactores_CellValidating(sender As Object, e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles dgFactores.CellValidating

        If cmbFactores.SelectedIndex >= 1 Then

            If e.ColumnIndex = 2 Then
                If (String.IsNullOrEmpty(e.FormattedValue.ToString())) Then
                    ft.MensajeCritico("Debe indicar dígito(s) válido...")
                    e.Cancel = True
                    Return
                End If

                If Not ft.isNumeric(e.FormattedValue.ToString()) Then
                    ft.mensajeCritico("Debe indicar un número válido...")
                    e.Cancel = True
                    Return
                End If


                If cmbFactores.SelectedIndex = 1 Then
                    If ValorNumero(e.FormattedValue.ToString()) > 100 Or _
                         ValorNumero(e.FormattedValue.ToString()) < 0 Then
                        ft.MensajeCritico("PORCENTAJE NO VALIDO. VERIFIQUE POR FAVOR...")
                        e.Cancel = True
                        Return
                    End If
                End If

            End If

        End If
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgFactores.RowHeaderMouseClick, _
       dgFactores.CellMouseClick
        Me.BindingContext(ds, nTablaFactores).Position = e.RowIndex
        nPocisionFactores = e.RowIndex
    End Sub

    Private Sub btnAgregarAsesor_Click(sender As System.Object, e As System.EventArgs) Handles btnAgregarAsesor.Click

        If cmbClase.SelectedIndex = 1 Then

            Dim dtAse As DataTable
            Dim strSQLAse As String = " select a.codven codigo, concat(a.apellidos, ', ', a.nombres) descripcion " _
                                      & " from jsvencatven a " _
                                      & " where " _
                                      & " tipo = '0' AND a.clase = '0' AND a.supervisor = '' and " _
                                      & " a.id_emp = '" & jytsistema.WorkID & "' order by a.codven "

            Dim nTableAse As String = "tblAsesorSup"

            ds = DataSetRequery(ds, strSQLAse, myConn, nTableAse, lblInfo)
            dtAse = ds.Tables(nTableAse)

            Dim f As New jsControlArcTablaSimpleSeleccion
            f.Cargar("Asesores Comerciales", ds, dtAse)

            If Not f.Seleccion Is Nothing Then
                For Each oCod As Object In f.Seleccion
                    ft.Ejecutar_strSQL(myconn, " update jsvencatven set supervisor = '" & txtCodigo.Text & "' where codven = '" & oCod.ToString & "' and id_emp = '" & jytsistema.WorkID & "' ")
                Next
            End If

            AbrirVendedoresSupervisor(txtCodigo.Text)

        End If


    End Sub

    Private Sub dgVendedores_CellMouseClick(sender As Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgVendedores.CellMouseClick, _
        dgVendedores.RegionChanged, dgVendedores.RowHeaderMouseDoubleClick

        Me.BindingContext(ds, nTablaVen).Position = e.RowIndex
        nPosicionVen = e.RowIndex

    End Sub

    Private Sub btnEliminarAsesor_Click(sender As System.Object, e As System.EventArgs) Handles btnEliminarAsesor.Click

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        sRespuesta = MsgBox(" ¿ Está seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro " _
                            & dtVendedores.Rows(nPosicionVen).Item("codven") & "... ")
        If sRespuesta = MsgBoxResult.Yes AndAlso nPosicionVen >= 0 Then
            With dtVendedores.Rows(nPosicionVen)
                ft.Ejecutar_strSQL(myconn, " update jsvencatven set supervisor = '' where " _
                               & " codven = '" & .Item("codven") & "' AND supervisor = '" & txtCodigo.Text & "' AND id_emp = '" & jytsistema.WorkID & "' ")
            End With
            AbrirVendedoresSupervisor(txtCodigo.Text)
        End If

    End Sub

    'JOJOJOJOJ VALIDAR CAMBIO DE CLASE DEL ASESOR COMERCIAL (DEBEN ELIMINARSE TODOS LOS VENDEDORES SUPERVISADOS)

    Private Sub cmbUNIDAD_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbUNIDAD.SelectedIndexChanged, _
        cmbFiltro.SelectedIndexChanged
        If cmbUNIDAD.Items.Count > 0 And cmbFiltro.Items.Count > 0 And Not cmbFiltro.SelectedValue Is Nothing Then
            IniciarCuotas(cmbUNIDAD.SelectedIndex, cmbCuota.SelectedIndex, cmbFiltro.SelectedValue.ToString)
        End If

    End Sub

    Private Sub dgCuotas_CellBeginEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles dgCuotas.CellBeginEdit
        If e.ColumnIndex >= 3 And e.ColumnIndex <= 14 Then
            If (String.IsNullOrEmpty(dgCuotas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString)) Then
                ft.MensajeCritico("No Editable... Favor revisar Peso Unidad de Venta y/o Equivalencia de la Unidad Secundaria de Medida (USM) ")
                e.Cancel = True
            End If
        End If
    End Sub
    Private Sub dgCuotas_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgCuotas.CellDoubleClick
        nPosicionCuo = Me.BindingContext(ds, nTablaCuotas).Position
    End Sub
    Private Sub dgCuotas_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dgCuotas.RowHeaderMouseClick, dgCuotas.CellMouseClick, dgCuotas.RegionChanged
        Me.BindingContext(ds, nTablaCuotas).Position = e.RowIndex
        nPosicionCuo = Me.BindingContext(ds, nTabla).Position
    End Sub

    Private Sub dg_RowValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgCuotas.RowValidated
        nPosicionCuo = Me.BindingContext(ds, nTablaCuotas).Position
        If dgCuotas.CurrentCell.ColumnIndex >= 3 And dgCuotas.CurrentCell.ColumnIndex <= 14 Then
            'AsignaTXT(Posicion, True, dg.CurrentCell.ColumnIndex)
        End If
    End Sub
    Private Sub dgCuotas_CancelRowEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.QuestionEventArgs) Handles dgCuotas.CancelRowEdit
        ft.mensajeAdvertencia("Cancelando")
    End Sub
    Private Sub dgCuotas_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgCuotas.CellValidated
        Select Case dgCuotas.CurrentCell.ColumnIndex
            Case 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14

                If Not String.IsNullOrEmpty(dgCuotas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then

                    Dim Mes As String = "ESMES" & Format(dgCuotas.CurrentCell.ColumnIndex - 2, "00")
                    Dim nValorCuotas As Double = 0.0
                    Dim CodigoMercancia As String = CStr(dgCuotas.CurrentRow.Cells(0).Value)
                    Select Case cmbUNIDAD.SelectedIndex
                        Case 0 'KILOGRAMOS
                            nValorCuotas = CDbl(dgCuotas.CurrentCell.Value)
                        Case 1 'UNIDAD VENTA
                            nValorCuotas = CDbl(dgCuotas.CurrentCell.Value) * ft.DevuelveScalarDoble(myConn, " select pesounidad from jsmerctainv where codart = '" & CodigoMercancia & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        Case 2 'UNIDAD MEDIDA SECUNDARIA
                            nValorCuotas = CDbl(dgCuotas.CurrentCell.Value) / Equivalencia(myConn,  CodigoMercancia, ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM07")) * ft.DevuelveScalarDoble(myConn, " select pesounidad from jsmerctainv where codart = '" & CodigoMercancia & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    End Select
                    If ft.DevuelveScalarEntero(myConn, " SELECT count(*) from jsvencuoart where codart = '" & CodigoMercancia & "' and codven = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                        ft.Ejecutar_strSQL(myConn, " insert into jsvencuoart values('" & txtCodigo.Text & "','" & CodigoMercancia & "', 0,0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0,0,0,'" & jytsistema.WorkExercise & "','" & jytsistema.WorkID & "' ) ")
                    End If
                    ft.Ejecutar_strSQL(myconn, " update jsvencuoart set " & Mes & " = " & nValorCuotas & " " _
                                            & " where codven = '" & txtCodigo.Text & "' and codart = '" & CodigoMercancia & "' and id_emp = '" & jytsistema.WorkID & "' ")
                End If

        End Select
    End Sub

    Private Sub dgCuotas_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
            Handles dgCuotas.CellValidating

        Dim headerText As String = _
            dgCuotas.Columns(e.ColumnIndex).HeaderText

        If Mid(headerText, 1, 5) = "ESMES" Then Return

        If Not String.IsNullOrEmpty(dgCuotas.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then
            If e.ColumnIndex >= 3 And e.ColumnIndex <= 14 Then

                If Not ft.isNumeric(e.FormattedValue.ToString()) Then
                    ft.mensajeAdvertencia("DEBE INDICAR UNA CANTIDAD VALIDA ...")
                    e.Cancel = True
                End If

            End If
        End If

    End Sub

    Private Sub dg_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dgCuotas.CellEndEdit
        dgCuotas.Rows(e.RowIndex).ErrorText = String.Empty
    End Sub

    Private Sub cmbUnidadStat_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbUnidadStat.SelectedIndexChanged
        If cmbUnidadStat.Items.Count > 0 Then AbrirEstadisticas()
    End Sub


End Class