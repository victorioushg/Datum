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

    Private strSQL As String = "select * from jsvencatven where tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by codven "
    Private strSQLDesc As String = ""
    Private strSQLDivition As String = ""
    Private strSQLHierarchy As String = ""
    Private strSQLCierre As String = ""
    Private strSQLRenglonesCierre As String = ""
    Private strSQLFactores As String = ""
    Private strSQLCuotas As String = ""

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

    Private aCondicion() As String = {"Inactivo", "Activo"}
    Private aEstadistica() As String = {"Mercancías", "Categorías", "Marcas", "Divisiones", "Jerarquías", "Cobranza Anterior", "Cobranza Actual"}
    Private aFactores() As String = {"Porcentajes de comisión por cobranza y días vencidos", _
                                     "Porcentajes de comisión de jerarquías", _
                                     "Cuotas por jerarquías"}


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
    Private Sub jsVenArcAsesores_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo

        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)

            InsertarAuditoria(myConn, MovAud.ientrar, sModulo, "")

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaTXT(nPosicionCat)
            Else
                IniciarAsesor(False)
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

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        If dt.Rows.Count > 0 Then

            With dt

                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

                With .Rows(nRow)

                    'Asesor Comercial
                    nPosicionCat = nRow

                    txtCodigo.Text = MuestraCampoTexto(.Item("codven"))
                    txtNombre.Text = MuestraCampoTexto(.Item("nombres"))
                    txtApellidos.Text = MuestraCampoTexto(.Item("apellidos"))
                    txtdescripcion.Text = MuestraCampoTexto(.Item("descar"))
                    '  txtCargo.Text = .Item("estructura").ToString
                    txtFactor.Text = FormatoNumero(.Item("factorcuota"))
                    chkA.Checked = CBool(.Item("lista_a"))
                    chkB.Checked = CBool(.Item("lista_b"))
                    chkC.Checked = CBool(.Item("lista_c"))
                    chkD.Checked = CBool(.Item("lista_d"))
                    chkE.Checked = CBool(.Item("lista_e"))
                    chkF.Checked = CBool(.Item("lista_f"))
                    FechaIngreso = CDate(.Item("ingreso").ToString)

                    RellenaCombo(aCondicion, cmbEstatus, .Item("estatus"))

                    AbrirDivisiones(.Item("codven"))
                    AbrirJerarquias(.Item("codven"))

                    txtCarteraMercas.Text = FormatoEntero(CarteraGrupo(myConn, lblInfo, .Item("codven"), "codart"))
                    txtCarteraMarcas.Text = FormatoEntero(CarteraGrupo(myConn, lblInfo, .Item("codven"), "marca"))
                    txtCarteraCatego.Text = FormatoEntero(CarteraGrupo(myConn, lblInfo, .Item("codven"), "grupo"))
                    txtCarteraJerarquia.Text = FormatoEntero(CarteraGrupo(myConn, lblInfo, .Item("codven"), "tipjer"))
                    txtCarteraDivisiones.Text = FormatoEntero(CarteraGrupo(myConn, lblInfo, .Item("codven"), "division"))

                    'Estadisticas
                    txtCodigoEstadisticas.Text = .Item("codven")
                    txtNombreEstadisticas.Text = MuestraCampoTexto(.Item("apellidos")) & ", " & MuestraCampoTexto(.Item("nombres"))

                    'Cierres
                    txtCodigoCierres.Text = .Item("codven")
                    txtNombreCierres.Text = MuestraCampoTexto(.Item("apellidos")) & ", " & MuestraCampoTexto(.Item("nombres"))

                    'Descuentos
                    txtCodigoDescuentos.Text = .Item("codven")
                    txtNombreDescuentos.Text = MuestraCampoTexto(.Item("apellidos")) & ", " & MuestraCampoTexto(.Item("nombres"))

                    'Factores
                    txtCodigoFactores.Text = .Item("codven")
                    txtNombreFactores.Text = MuestraCampoTexto(.Item("apellidos")) & ", " & MuestraCampoTexto(.Item("nombres"))

                    'Cuotas
                    txtCodigoCuotas.Text = .Item("codven")
                    txtNombreCuotas.Text = MuestraCampoTexto(.Item("apellidos")) & ", " & MuestraCampoTexto(.Item("nombres"))

                End With
            End With
        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub AbrirDivisiones(ByVal CodigoVendedor As String)

        strSQLDivition = " SELECT a.division, b.descrip " _
                & " FROM jsvencatvendiv a " _
                & " LEFT JOIN jsmercatdiv b ON (a.division = b.division AND a.id_emp = b.id_emp) " _
                & " WHERE " _
                & " a.codven = '" & CodigoVendedor & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' order by a.division "

        ds = DataSetRequery(ds, strSQLDivition, myConn, nTablaDivisiones, lblInfo)
        dtDivition = ds.Tables(nTablaDivisiones)

        Dim aCam() As String = {"division", "descrip"}
        Dim aNom() As String = {"División", ""}
        Dim aAnc() As Long = {90, 200}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", ""}

        IniciarTabla(dgDivisiones, dtDivition, aCam, aNom, aAnc, aAli, aFor)
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
        Dim aAnc() As Long = {90, 200}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", ""}

        IniciarTabla(dgJerarquías, dtHierarchy, aCam, aNom, aAnc, aAli, aFor)
        If dtHierarchy.Rows.Count > 0 Then nPosicionHie = 0

    End Sub
    Private Sub IniciarAsesor(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = AutoCodigo(5, ds, nTabla, "codven")
        Else
            txtCodigo.Text = ""
        End If

        IniciarTextoObjetos(FormatoItemListView.iCadena, txtNombre, txtApellidos, txtdescripcion, txtCargo)
        IniciarTextoObjetos(FormatoItemListView.iEntero, txtCarteraCatego, txtCarteraDivisiones, txtCarteraJerarquia, txtCarteraMarcas, txtCarteraMercas)
        IniciarTextoObjetos(FormatoItemListView.iNumero, txtFactor)
        RellenaCombo(aCondicion, cmbEstatus, 1)
        chkA.Checked = True
        chkB.Checked = False
        chkC.Checked = False
        chkD.Checked = False
        chkE.Checked = False
        chkF.Checked = False

        FechaIngreso = jytsistema.sFechadeTrabajo

    End Sub
    Private Sub IniciarDescuentos(ByVal CodAsesor As String)

        If CodAsesor <> "" Then
            strSQLDesc = " SELECT * from jsconcatdes where codven = '" & CodAsesor & "' and tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by inicio desc, coddes "

            ds = DataSetRequery(ds, strSQLDesc, myConn, nTablaDesc, lblInfo)
            dtDescuentos = ds.Tables(nTablaDesc)

            Dim aCam() As String = {"coddes", "descrip", "pordes", "inicio", "fin", "codcli", "codart", ""}
            Dim aNom() As String = {"Código", "Descripción", "% Descuento", "Desde", "Hasta", "Codigo cliente", "Codigo Mercancía", ""}
            Dim aAnc() As Long = {60, 200, 70, 90, 90, 100, 100, 30}
            Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, _
                                     AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                                     AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
            Dim aFor() As String = {"", "", sFormatoNumero, sFormatoFecha, sFormatoFecha, "", "", ""}

            IniciarTabla(dgDescuentos, dtDescuentos, aCam, aNom, aAnc, aAli, aFor)
            If dtDescuentos.Rows.Count > 0 Then nPosicionDes = 0

        End If

    End Sub
    Private Sub IniciarFactores()
        RellenaCombo(aFactores, cmbFactores)
    End Sub
    Private Sub IniciarCierre(ByVal CodigoAsesor As String)
        If CodigoAsesor <> "" Then
            strSQLCierre = " select * from jsvenenccie where codven = '" & CodigoAsesor & "' and id_emp = '" & jytsistema.WorkID & "' order by fecha "
            ds = DataSetRequery(ds, strSQLCierre, myConn, nTablacierre, lblInfo)
            dtCierre = ds.Tables(nTablacierre)
            If dtCierre.Rows.Count > 0 Then nPosicionCie = dtCierre.Rows.Count - 1
        End If
    End Sub
    Private Sub AsignarCierre(ByVal dtCierre As DataTable, ByVal nRowCierre As Long)
        If dtCierre.Rows.Count > 0 Then
            With dtCierre.Rows(nRowCierre)
                txtFechaCierre.Text = FormatoFecha(CDate(.Item("fecha").ToString))
                txtAVisitar.Text = .Item("avisitar")
                txtVisitados.Text = .Item("visitados")
                txtEfectividad.Text = .Item("efectividad")
                txtCierreUnidades.Text = .Item("items_caj")
                txtCierreKilogramos.Text = .Item("items_kgs")

                strSQLRenglonesCierre = " SELECT * from jsvenrencie " _
                            & " where " _
                            & " codven = '" & .Item("codven") & "' and " _
                            & " fecha = '" & FormatoFechaMySQL(CDate(.Item("fecha").ToString)) & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by cliente "

                ds = DataSetRequery(ds, strSQLRenglonesCierre, myConn, nTablaRenglonesCierre, lblInfo)
                dtRenglonesCierre = ds.Tables(nTablaRenglonesCierre)

                Dim aCam() As String = {"cliente", "nombre", "cajas", "kilos", "totalpedido", "totaldev", ""}
                Dim aNom() As String = {"Código", "Nombre o Razón Social", "Cajas", "Peso (KGS)", "Total Pedido", "total Devuelto", ""}
                Dim aAnc() As Long = {60, 300, 70, 70, 100, 100, 100}
                Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, _
                                         AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                         AlineacionDataGrid.Izquierda}
                Dim aFor() As String = {"", "", sFormatoCantidad, sFormatoCantidad, sFormatoNumero, sFormatoNumero, ""}

                IniciarTabla(dgCierre, dtRenglonesCierre, aCam, aNom, aAnc, aAli, aFor)
                If dtRenglonesCierre.Rows.Count > 0 Then nPosicionRenglonesCie = 0

            End With
        End If
    End Sub
    Private Sub ActivarMarco0()
        grpAceptarSalir.Visible = True

        HabilitarObjetos(False, False, C1DockingTabPage2, C1DockingTabPage6, C1DockingTabPage4)
        HabilitarObjetos(True, True, txtApellidos, txtNombre, txtdescripcion, btnCargo, cmbEstatus, txtFactor)
        HabilitarObjetos(True, False, MenuJerarquias, MenuDivisiones, chkA, chkB, chkC, chkD, chkE, chkF)

        If i_modo = movimiento.iEditar Then HabilitarObjetos(False, False, txtCodigo)

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub

    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        HabilitarObjetos(True, False, C1DockingTabPage2, C1DockingTabPage3, C1DockingTabPage4, C1DockingTabPage6)

        HabilitarObjetos(False, True, txtCodigo, txtNombre, txtApellidos, txtdescripcion, txtCargo, txtEstructura, _
                         txtCarteraCatego, txtCarteraDivisiones, txtCarteraJerarquia, txtCarteraMarcas, txtCarteraMercas, _
                         txtFactor, cmbEstatus, btnCargo, txtCodigoCierres, txtNombreCierres, _
                         txtFechaCierre, txtAVisitar, txtVisitados, txtEfectividad, txtCierreUnidades, txtCierreKilogramos, _
                         txtCodigoDescuentos, txtNombreDescuentos, _
                         txtCodigoEstadisticas, txtNombreEstadisticas, _
                         txtCodigoFactores, txtNombreFactores, txtCodigoCuotas, txtNombreCuotas, _
                         txtTotalAcumulado, txtTotalCierre, txtTotalCuota, txtTotalLogro, txtTotalMeta)

        HabilitarObjetos(False, False, chkA, chkB, chkC, chkD, chkE, chkF, MenuDivisiones, MenuJerarquias)
        MenuBarra.Enabled = True
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

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
                AbrirEstadisticas()
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
                AbrirEstadisticas()
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
                AbrirEstadisticas()
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
                AbrirEstadisticas()
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
                Dim gg As New jsVenArcAsesoresCuotasMovimientos
                gg.Agregar(myConn, ds, dtFactores, txtCodigoFactores.Text)
                gg.Dispose()
                gg = Nothing
                IniciarComisionCobranza()
            Case 1 'Comisiones por jerarquía
                AgregarComisionesPorTipo(0)
                IniciarComision_X_Jerarquia()
            Case 2 'Cuotas por jerarquía
                AgregarComisionesPorTipo(1)
                IniciarCuotasJerarquias()
        End Select
    End Sub
    Private Sub AgregarComisionesPorTipo(TipoFactor As Integer)

        Dim dtJerar As DataTable
        Dim nTableJer As String = "tblJer"
        ds = DataSetRequery(ds, " select * from jsmerencjer where id_emp = '" & jytsistema.WorkID & "' ", myConn, nTableJer, lblInfo)
        dtJerar = ds.Tables(nTableJer)

        For Each nRow As DataRow In dtJerar.Rows
            With nRow
                If EjecutarSTRSQL_Scalar(myConn, lblInfo, " select codven from jsvencomven where codven = '" & txtCodigoFactores.Text _
                                         & "' and tipjer = '" & .Item("tipjer") & "' and tipo = " & TipoFactor & " and id_emp = '" & jytsistema.WorkID & "'  ") = "0" Then
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
                    Dim gg As New jsVenArcAsesoresCuotasMovimientos
                    gg.Apuntador = nPocisionFactores
                    gg.Editar(myConn, ds, dtFactores, txtCodigoFactores.Text)
                    nPocisionFactores = gg.Apuntador
                    AsignatxtFactores(nPocisionFactores, True)
                    gg.Dispose()
                    gg = Nothing
                End If
        End Select

    End Sub
    Private Sub AsignatxtFactores(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then IniciarFactores()

        If c >= 0 AndAlso ds.Tables(nTablaFactores).Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaFactores).Position = c
            dgFactores.CurrentCell = dgFactores(0, c)
        End If

    End Sub
    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Select Case tbcAsesores.SelectedTab.Text
            Case "Asesores Comerciales"
                MensajeCritico(lblInfo, "En estos momentos no se puede eliminar una Asesor.Intente desactivarlo...")
            Case "Descuentos"
                EliminarDescuentos()
            Case "Factores"
                EliminarFactor(cmbFactores.SelectedIndex)
        End Select
    End Sub
    Private Sub EliminarFactor(TipoFactor As Integer)
        Select Case TipoFactor
            Case 0
                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                Dim aCamposDel() As String = {"codven", "tipjer", "de", "a", "id_emp"}
                With dtFactores.Rows(nPocisionFactores)

                    Dim aStringsDel() As String = {.Item("codven"), "XX", .Item("de"), .Item("A"), jytsistema.WorkID}
                    sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
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
        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
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
        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
        If sRespuesta = MsgBoxResult.Yes Then
            If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(*) from jsmertramer where vendedor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")) = 0 Then
                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsvencatven", strSQL, aCamposDel, aStringsDel, _
                                                Me.BindingContext(ds, nTabla).Position, True))
            Else
                MensajeCritico(lblInfo, "Este Asesor Comercial posee movimientos asociados. Verifique por favor ...")
            End If
        End If
    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar

        Select Case tbcAsesores.SelectedTab.Text
            Case "Asesores Comerciales"
                Dim Campos() As String = {"codven", "descar", "apellidos", "nombres"}
                Dim Nombres() As String = {"Código", "Descripción", "Apellidos", "Nomnbres"}
                Dim Anchos() As Long = {120, 250, 250, 250}
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
        ''        Dim f As New jsMerRepParametros
        ''      Select Case tbcClientes.SelectedTab.Text
        ''        Case "Clientes i", "Clientes ii"
        ''  f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cFichaMercancia, "Ficha Mercancía", txtCodigo.Text)
        ''    Case "Movimientos CxC"
        ''f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cMovimientosMercancia, "Movimientos de mercancía", txtCodigoMovimientos.Text)
        ''End Select
        ''f.Dispose()
        ''f = Nothing
    End Sub

    Private Function Validado() As Boolean
        If txtApellidos.Text = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar un Apellido Válido...")
            Return False
        End If

        If Not chkA.Checked And Not chkB.Checked And Not chkC.Checked _
            And Not chkD.Checked And Not chkE.Checked And Not chkF.Checked Then
            MensajeAdvertencia(lblInfo, "Debe indicar una tarifa asignable por lo menos...")
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
                                  ValorNumero(txtFactor.Text), cmbEstatus.SelectedIndex, "", "", 0.0, 0.0)

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        sRespuesta = MsgBox(" ¿ Desea actualizar cuotas de mercancías a partir de las divisiones y jerarquías actuales ?", MsgBoxStyle.YesNo, "Actualización de cuotas ... ")
        If sRespuesta = MsgBoxResult.Yes Then VerificaCuotasDeMercanciasPorDivisionYJerarquia(txtCodigo.Text)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionCat
        AsignaTXT(nPosicionCat)
        DesactivarMarco0()
        tbcAsesores.SelectedTab = C1DockingTabPage1
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
         txtNombre.GotFocus

        Select Case sender.name
            Case "txtCodigo"
                MensajeEtiqueta(lblInfo, " Indique el código de Asesor comercial ... ", TipoMensaje.iInfo)
            Case "cmbEstatus"
                MensajeEtiqueta(lblInfo, " Sleccione estatus de asesor comercial ... ", TipoMensaje.iInfo)
            Case "txtApellidos"
                MensajeEtiqueta(lblInfo, " Indique los apellidos del asesor comercial ... ", TipoMensaje.iInfo)
            Case "txtNombre"
                MensajeEtiqueta(lblInfo, " Indique los nombres del asesor comercial ... ", TipoMensaje.iInfo)
            Case "txtdescripcion"
                MensajeEtiqueta(lblInfo, " Indique descripción del cargo ... ", TipoMensaje.iInfo)
            Case "txtFactor"
                MensajeEtiqueta(lblInfo, " Indique factor para el cálculo de cuotas en base a asignación fija ... ", TipoMensaje.iInfo)
        End Select

    End Sub

    Private Sub tbcAsesores_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcAsesores.SelectedIndexChanged

        Select Case tbcAsesores.SelectedIndex
            Case 0 ' Asesores Comerciales
                AsignaTXT(nPosicionCat)
                ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            Case 1 '' Estadísticas
                rBtnV1.Checked = True
                RellenaCombo(aEstadistica, cmbTipoEstadistica)
                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nPosicionCat, ds.Tables(nTabla).Rows.Count)
            Case 2 '' Cierres
                IniciarCierre(txtCodigo.Text)
            Case 3 'Descuentos
                IniciarDescuentos(txtCodigo.Text)
                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nPosicionDes, ds.Tables(nTabla).Rows.Count)
            Case 4 ' Factores
                IniciarFactores()
            Case 5
                IniciarCuotas()

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
        Dim nTableCuota As String = "tblCuota"
        ds = DataSetRequery(ds, CuotasAcumuladoAsesor(txtCodigo.Text, "cuota"), myConn, nTableCuota, lblInfo)
        dtt = ds.Tables(nTableCuota)

        Dim dttAnteriores As New DataTable
        Dim nTableAcumulado As String = "tblAcumulado"
        ds = DataSetRequery(ds, CuotasAcumuladoAsesor(txtCodigo.Text, "acumulado"), myConn, nTableAcumulado, lblInfo)
        dttAnteriores = ds.Tables(nTableAcumulado)

        aaY = ValoresMensuales(dtt)
        abY = ValoresMensuales(dttAnteriores)

        Dim aFFld() As String = {"id_emp"}
        Dim aSStr() As String = {jytsistema.WorkID}

        ay.Text = "Kilogramos"
        ax.Text = cmbTipoEstadistica.Text & " mes a mes"

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

    Private Sub txtSugerido_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub

    Private Sub rBtnV1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rBtnV1.CheckedChanged, _
        rBtnV2.CheckedChanged, rBtnV3.CheckedChanged
        If cmbTipoEstadistica.Items.Count > 6 Then AbrirEstadisticas()
    End Sub

    Private Sub AbrirEstadisticas()

        Dim aCam() As String = {"item", "descrip", "cuota", "acumulado", "logro", "meta", "cierre", ""}
        Dim aNom() As String = {"Código", "Descripción", "cuota", "Acumulado", "% Logro", "Meta Diaria", "Cierre", ""}
        Dim aAnc() As Long = {90, 300, 90, 90, 60, 90, 90, 70}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}

        Dim aFor() As String = {"", "", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, ""}

        Dim dtStat As New DataTable
        Dim nTablaStat As String = "tblEstadistica"
        ds = DataSetRequery(ds, SeleccionaEstadisticaAsesor(txtCodigo.Text, cmbTipoEstadistica.SelectedIndex, IIf(rBtnV1.Checked, 0, 1)), myConn, nTablaStat, lblInfo)
        dtStat = ds.Tables(nTablaStat)

        IniciarTabla(dgEstadistica, dtStat, aCam, aNom, aAnc, aAli, aFor)

        CalculaTotalesEstadistica()

        VerHistograma(C1Chart2, TipoDatoMercancia.iKilogramos)

    End Sub
    Private Sub CalculaTotalesEstadistica()
        txtTotalCuota.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(cuota) from jsvenstaven where tipo = " & cmbTipoEstadistica.SelectedIndex & " and codven = '" & txtCodigoEstadisticas.Text & "' and id_emp = '" & jytsistema.WorkID & "' group by id_emp ")))
        txtTotalAcumulado.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(acumulado) from jsvenstaven where tipo = " & cmbTipoEstadistica.SelectedIndex & " and codven = '" & txtCodigoEstadisticas.Text & "' and id_emp = '" & jytsistema.WorkID & "' group by id_emp ")))
        txtTotalLogro.Text = FormatoNumero(IIf(ValorNumero(txtTotalCuota.Text) > 0, FormatoNumero(ValorNumero(txtTotalAcumulado.Text) / ValorNumero(txtTotalCuota.Text) * 100), 0.0))
        txtTotalMeta.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(meta) from jsvenstaven where tipo = " & cmbTipoEstadistica.SelectedIndex & " and codven = '" & txtCodigoEstadisticas.Text & "' and id_emp = '" & jytsistema.WorkID & "' group by id_emp ")))
        txtTotalCierre.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(cierre) from jsvenstaven where tipo = " & cmbTipoEstadistica.SelectedIndex & " and codven = '" & txtCodigoEstadisticas.Text & "' and id_emp = '" & jytsistema.WorkID & "' group by id_emp ")))
    End Sub

    Private Function SeleccionaEstadisticaAsesor(ByVal CodigoAsesor As String, ByVal Tipo As Integer, _
                                                 ByVal UnidadesKilosMoneda As Integer) As String

        Dim aCampo() As String = {"nomart", "descrip", "descrip", "descrip", "descrip", "nombre", "nombre"}
        Dim aTabla() As String = {"jsmerctainv", "jsconctatab", "jsconctatab", "jsmercatdiv", "jsmerencjer", "jsvencatcli", "jsvencatcli"}
        Dim aLeftJoin() As String = {"codart", "codigo", "codigo", "division", "tipjer", "codcli", "codcli"}
        Dim aWhere() As String = {"", " b.modulo = '00002' and ", " b.modulo = '00003' and ", "", "", "", ""}

        SeleccionaEstadisticaAsesor = " select a.item, b." & aCampo(Tipo) & " descrip, " _
            & " if(a.cuota/" & IIf(UnidadesKilosMoneda = 0, "b.pesounidad", "1") & " is null, 0.00, a.cuota/" & IIf(UnidadesKilosMoneda = 0, "b.pesounidad", "1") & ") cuota, " _
            & " if(a.acumulado/" & IIf(UnidadesKilosMoneda = 0, "b.pesounidad", "1") & " is null, 0.00,a.acumulado/" & IIf(UnidadesKilosMoneda = 0, "b.pesounidad", "1") & ") acumulado, a.logro, " _
            & " if(a.meta/" & IIf(UnidadesKilosMoneda = 0, "b.pesounidad", "1") & " is null, 0.00, a.meta/" & IIf(UnidadesKilosMoneda = 0, "b.pesounidad", "1") & ") meta, " _
            & " if(a.cierre/" & IIf(UnidadesKilosMoneda = 0, "b.pesounidad", "1") & " is null, 0.00, a.cierre/" & IIf(UnidadesKilosMoneda = 0, "b.pesounidad", "1") & " ) cierre " _
            & " from jsvenstaven a " _
            & " left join " & aTabla(Tipo) & " b on (a.item = b." & aLeftJoin(Tipo) & " and a.id_emp = b.id_emp) " _
            & " where  " _
            & aWhere(Tipo) _
            & " a.codven = '" & CodigoAsesor & "' and " _
            & " month(a.fecha) = " & jytsistema.sFechadeTrabajo.Month & " and " _
            & " year(a.fecha) = " & jytsistema.sFechadeTrabajo.Year & " and " _
            & " a.tipo = " & Tipo & " and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by a.item "

    End Function

    Private Sub cmbTipoEstadistica_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipoEstadistica.SelectedIndexChanged
        Select Case cmbTipoEstadistica.SelectedIndex
            Case 0
                rBtnV1.Checked = True
                HabilitarObjetos(True, False, rBtnV1, rBtnV2)
                HabilitarObjetos(False, False, rBtnV3)
            Case 1, 2, 3, 4
                rBtnV2.Checked = True
                HabilitarObjetos(True, False, rBtnV2)
                HabilitarObjetos(False, False, rBtnV1, rBtnV3)
            Case 5, 6
                rBtnV3.Checked = True
                HabilitarObjetos(False, False, rBtnV1, rBtnV2)
                HabilitarObjetos(True, False, rBtnV3)
        End Select
        If cmbTipoEstadistica.Items.Count > 6 Then AbrirEstadisticas()

    End Sub

    Private Sub txtNombre_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNombre.TextChanged
        txtNombreFactores.Text = txtNombre.Text
    End Sub

    Private Sub txtCargo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCargo.TextChanged
        If txtCargo.Text <> "" Then txtEstructura.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select nombre from jsnomestcar where codigo = " & txtCargo.Text.Split(".")(UBound(txtCargo.Text.Split("."))) & " ")
    End Sub

    Private Sub cmbFactores_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFactores.SelectedIndexChanged
        Select Case cmbFactores.SelectedIndex
            Case 0 'Porcentajes de comisión por cobranza y días de vencimiento
                IniciarComisionCobranza()
            Case 1 'porcentaje por jerarquías
                IniciarComision_X_Jerarquia()
            Case 2 'Cuotas Jerarquía
                IniciarCuotasJerarquias()
        End Select
    End Sub
    Private Sub IniciarComisionCobranza()
        If txtCodigo.Text <> "" Then

            strSQLFactores = " select * from jsvencomvencob " _
            & " where " _
            & " codven = '" & txtCodigo.Text & "' and " _
            & " tipjer = 'XX' and id_emp = '" & jytsistema.WorkID & "' "

            ds = DataSetRequery(ds, strSQLFactores, myConn, nTablaFactores, lblInfo)
            dtFactores = ds.Tables(nTablaFactores)

            Dim aCam() As String = {"de", "a", "por_cobranza", ""}
            Dim aNom() As String = {"Días vencidos desde", "Días vencidos hasta", "% Comisión", ""}
            Dim aAnc() As Long = {90, 90, 90, 90}
            Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                     AlineacionDataGrid.Izquierda}
            Dim aFor() As String = {sFormatoEntero, sFormatoEntero, sFormatoNumero, ""}

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
            & " a.tipo = 0 AND " _
            & " a.codven = '" & txtCodigo.Text & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' "

            ds = DataSetRequery(ds, strSQLFactores, myConn, nTablaFactores, lblInfo)
            dtFactores = ds.Tables(nTablaFactores)

            Dim aCam() As String = {"tipjer", "descrip", "por_ventas", ""}
            Dim aNom() As String = {"Jerarquía", "", "% Comisión", ""}
            Dim aAnc() As Long = {80, 300, 120, 90}
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
            & " a.tipo = 1 AND " _
            & " a.codven = '" & txtCodigo.Text & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' "

            ds = DataSetRequery(ds, strSQLFactores, myConn, nTablaFactores, lblInfo)
            dtFactores = ds.Tables(nTablaFactores)

            Dim aCam() As String = {"tipjer", "descrip", "por_ventas", ""}
            Dim aNom() As String = {"Jerarquía", "", "Cuotas", ""}
            Dim aAnc() As Long = {80, 300, 120, 90}
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

                                Dim CuotaENE As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select mes01 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")) * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaFEB As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select mes02 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")) * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaMAR As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select mes03 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")) * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaABR As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select mes04 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")) * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaMAY As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select mes05 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")) * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaJUN As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select mes06 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")) * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaJUL As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select mes07 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")) * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaAGO As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select mes08 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")) * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaSEP As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select mes09 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")) * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaOCT As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select mes10 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")) * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaNOV As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select mes11 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")) * ValorNumero(txtFactor.Text) / 100
                                Dim CuotaDIC As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select mes12 from jsmerctacuo where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")) * ValorNumero(txtFactor.Text) / 100

                                EjecutarSTRSQL(myConn, lblInfo, " insert into jsvencuoart set codven = '" & txtCodigo.Text & "', codart = '" & .Item("codart") & "', " _
                                               & " esmes01 = " & CuotaENE & ", esmes02 = " & CuotaFEB & ", esmes03 = " & CuotaMAR & ", " _
                                               & " esmes04 = " & CuotaABR & ", esmes05 = " & CuotaMAY & ", esmes06 = " & CuotaJUN & ", " _
                                               & " esmes07 = " & CuotaJUL & ", esmes08 = " & CuotaAGO & ", esmes09 = " & CuotaSEP & ", " _
                                               & " esmes10 = " & CuotaOCT & ", esmes11 = " & CuotaNOV & ", esmes12 = " & CuotaDIC & ", ejercicio = '" & jytsistema.WorkExercise & "', id_emp = '" & jytsistema.WorkID & "' ")

                            End If
                        Else
                            EjecutarSTRSQL_Scalar(myConn, lblInfo, " delete from jsvencuoart where codven = '" & txtCodigo.Text & "' and codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        End If
                    Else
                        EjecutarSTRSQL_Scalar(myConn, lblInfo, " delete from jsvencuoart where codven = '" & txtCodigo.Text & "' and codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    End If
                End With

            Next
        End If


    End Sub
    Private Sub JerarquiasAñadidas(ByVal CodigoVendedor As String)

        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvencatvenjer where codven = '" & CodigoVendedor & "' and id_emp = '" & jytsistema.WorkID & "'  ")

        EjecutarSTRSQL(myConn, lblInfo, " replace into jsvencatvenjer SELECT '" & CodigoVendedor & "' codven, a.tipjer, a.id_emp " _
                               & " FROM jsmerctainv a " _
                               & " WHERE " _
                               & " a.division IN (SELECT division FROM jsvencatvendiv WHERE codven = '" & CodigoVendedor & "' AND id_emp = '" & jytsistema.WorkID & "') AND  " _
                               & " a.estatus = 1 and " _
                               & " a.cuota = 1 and " _
                               & " a.id_emp = '" & jytsistema.WorkID & "' " _
                               & " GROUP BY a.tipjer ")

    End Sub
    Private Sub DivisionesAñadidas(ByVal CodigoVendedor As String)

        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvencatvendiv where codven = '" & CodigoVendedor & "' and id_emp = '" & jytsistema.WorkID & "'  ")

        EjecutarSTRSQL(myConn, lblInfo, " replace into jsvencatvendiv SELECT '" & CodigoVendedor & "' codven, a.division, a.id_emp " _
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

        EjecutarSTRSQL_Scalar(myConn, lblInfo, " delete from jsvencatvendiv where codven = '" & txtCodigo.Text & "' and  id_emp = '" & jytsistema.WorkID & "'  ")
        AbrirDivisiones(txtCodigo.Text)

        f.Cargar("Divisiones", ds, dtDiv, nTableDiv)

        Dim jCont As Integer
        Dim arrList As ArrayList = f.Seleccion
        For jCont = 0 To arrList.Count - 1
            EjecutarSTRSQL_Scalar(myConn, lblInfo, " replace into jsvencatvendiv set codven = '" & txtCodigo.Text & "', division = '" & arrList.Item(jCont) & "', id_emp = '" & jytsistema.WorkID & "'  ")
        Next

        JerarquiasAñadidas(txtCodigo.Text)

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

        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvencatvenjer where codven = '" & txtCodigo.Text & "' and  id_emp = '" & jytsistema.WorkID & "'  ")
        AbrirJerarquias(txtCodigo.Text)

        f.Cargar("Jerarquías", ds, dtJer, nTablejer)

        Dim jCont As Integer
        Dim arrList As ArrayList = f.Seleccion
        For jCont = 0 To arrList.Count - 1
            EjecutarSTRSQL(myConn, lblInfo, " replace into jsvencatvenjer set codven = '" & txtCodigo.Text & "', tipjer = '" & arrList.Item(jCont) & "', id_emp = '" & jytsistema.WorkID & "'  ")
        Next

        DivisionesAñadidas(txtCodigo.Text)

        AbrirDivisiones(txtCodigo.Text)
        AbrirJerarquias(txtCodigo.Text)


        f.Dispose()
        f = Nothing

        dtJer.Dispose()
        dtJer = Nothing

    End Sub

    Private Sub rBtnU_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rBtnU.CheckedChanged, _
        rBtnK.CheckedChanged
        If cmbCuota.Items.Count > 6 Then AbrirCuotasVendedor(txtCodigo.Text, rBtnU.Checked)

    End Sub
    Private Sub IniciarCuotas()
        rBtnU.Checked = True
        RellenaCombo(aEstadistica, cmbCuota)

    End Sub
    Private Sub AbrirCuotasVendedor(ByVal CodigoVendedor As String, ByVal UnidadOKilos As Boolean)

        Dim strUK As String = IIf(UnidadOKilos, ")", "*b.pesounidad)")
        Dim aGrupo() As String = {"codart", "grupo", "marca", "division", "tipjer", "codart", "codart"}
        Dim aGrupoM() As String = {"b.nomart", "c.descrip", "d.descrip", "e.descrip", "f.descrip", "b.nomart", "b.nomart"}

        strSQLCuotas = " SELECT b." & aGrupo(cmbCuota.SelectedIndex) & " codart, " & aGrupoM(cmbCuota.SelectedIndex) & " nomart, " _
                & " sum(a.esmes01" & strUK & " esmes01, sum(a.esmes02" & strUK & " esmes02, sum(a.esmes03" & strUK & " esmes03, " _
                & " sum(a.esmes04" & strUK & " esmes04, sum(a.esmes05" & strUK & " esmes05, sum(a.esmes06" & strUK & " esmes06, " _
                & " sum(a.esmes07" & strUK & " esmes07, sum(a.esmes08" & strUK & " esmes08, sum(a.esmes09" & strUK & " esmes09, " _
                & " sum(a.esmes10" & strUK & " esmes10, sum(a.esmes11" & strUK & " esmes11, sum(a.esmes12" & strUK & " esmes12 " _
                & " FROM jsvencuoart a " _
                & " LEFT JOIN jsmerctainv b ON (a.codart = b.codart AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN jsconctatab c on (b.grupo = c.codigo and a.id_emp = c.id_emp and c.modulo = '00002') " _
                & " LEFT JOIN jsconctatab d on (b.marca = d.codigo and a.id_emp = d.id_emp and d.modulo = '00003') " _
                & " LEFT JOIN jsmercatdiv e on (b.division = e.division and a.id_emp = e.id_emp) " _
                & " LEFT JOIN jsmerencjer f on (b.tipjer = f.tipjer and a.id_emp = f.id_emp ) " _
                & " WHERE " _
                & " a.codven = '" & CodigoVendedor & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' group by b." & aGrupo(cmbCuota.SelectedIndex)

        ds = DataSetRequery(ds, strSQLCuotas, myConn, nTablaCuotas, lblInfo)
        dtCuotas = ds.Tables(nTablaCuotas)

        Dim aCam() As String = {"codart", "nomart", "esmes01", "esmes02", "esmes03", "esmes04", "esmes05", "esmes06", "esmes07", "esmes08", "esmes09", "esmes10", "esmes11", "esmes12", ""}
        Dim aNom() As String = {"Código", "Descripción", "ENE", "FEB", "MAR", "ABR", "MAY", "JUN", "JUL", "AGO", "SEP", "OCT", "NOV", "DIC", ""}
        Dim aAnc() As Long = {60, 200, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                 AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                 AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda}

        Dim aFor() As String = {"", "", sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, ""}

        IniciarTabla(dgCuotas, dtCuotas, aCam, aNom, aAnc, aAli, aFor, , True, 8)
        If dtCuotas.Rows.Count > 0 Then nPosicionCuo = 0

    End Sub

    Private Sub cmbCuota_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCuota.SelectedIndexChanged
        If cmbCuota.Items.Count > 6 Then AbrirCuotasVendedor(txtCodigo.Text, rBtnU.Checked)
    End Sub

    Private Sub dgFactores_CellEndEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgFactores.CellEndEdit
        dgFactores.Rows(e.RowIndex).ErrorText = String.Empty
        MensajeAyuda(lblInfo, "Puede indicar valor en para modificar")
    End Sub

   
    Private Sub dgFactores_CellValidated(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgFactores.CellValidated
        If cmbFactores.SelectedIndex >= 1 Then
            Select Case dgFactores.CurrentCell.ColumnIndex
                Case 2
                    InsertEditVENTASComisionesJerarquias(myConn, lblInfo, False, txtCodigoFactores.Text, dgFactores.CurrentRow.Cells(0).Value, _
                                CDbl(dgFactores.CurrentCell.Value), cmbFactores.SelectedIndex)
            End Select
        End If
    End Sub

    Private Sub dgFactores_CellValidating(sender As Object, e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles dgFactores.CellValidating
       
        If cmbFactores.SelectedIndex >= 1 Then

            If e.ColumnIndex = 2 Then
                If (String.IsNullOrEmpty(e.FormattedValue.ToString())) Then
                    MensajeAdvertencia(lblInfo, "Debe indicar dígito(s) válido...")
                    e.Cancel = True
                End If

                If Not IsNumeric(e.FormattedValue.ToString()) Then
                    MensajeAdvertencia(lblInfo, "Debe indicar un número válido...")
                    e.Cancel = True
                End If


                If cmbFactores.SelectedIndex = 1 Then
                    If ValorNumero(e.FormattedValue.ToString()) > 100 Or _
                         ValorNumero(e.FormattedValue.ToString()) < 0 Then
                        MensajeAdvertencia(lblInfo, "PORCENTAJE NO VALIDO. VERIFIQUE POR FAVOR...")
                        e.Cancel = True
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
End Class