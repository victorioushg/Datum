Imports MySql.Data.MySqlClient
Public Class jsContabArcCuentas
    Private Const sModulo As String = "Cuentas Contables"
    Private Const lRegion As String = "RibbonButton1"
    Private Const nTabla As String = "tblcc"
    Private Const nTablaMovimientos As String = "tblmovcc"
    Private Const nTablaMovMes As String = "tblmovccmes"

    Private strSQL As String = "select a.*,  lpad( a.descripcion, length(a.codcon) + length(a.descripcion), ' ' ) nombre   from jscotcatcon a where a.id_emp = '" & jytsistema.WorkID & "' order by a.codcon"
    Private strSQLMov As String
    Private strSQLMovMes As String

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtMovimientos As New DataTable
    Private dtMovimientosMes As New DataTable
    Private ft As New Transportables


    Private i_modo As Integer
    Private nPosicionCat As Long, nPosicionMov As Long, nPosicionMes As Long
    Private PrimerDia As Date = IIf(FechaCierreEjercicio(myConn, lblInfo) < jytsistema.sFechadeTrabajo, FechaInicioEjercicio(myConn, lblInfo), PrimerDiaMes(jytsistema.sFechadeTrabajo))
    Private UltimoDia As Date = IIf(FechaCierreEjercicio(myConn, lblInfo) < jytsistema.sFechadeTrabajo, FechaCierreEjercicio(myConn, lblInfo), UltimoDiaMes(jytsistema.sFechadeTrabajo))

    Private Sub jsContabArcCuentas_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsContabArcCuentas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo
        Try
            myConn.Open()
            tbcCuentas.SelectedTab = C1DockingTabPage1
            IniciarCuentas()

            txtDesde.Text = ft.FormatoFecha(PrimerDia)
            txtHasta.Text = ft.FormatoFecha(UltimoDia)

            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaCat(nPosicionCat, False)
            Else
                IniciarCuenta()
            End If

            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub IniciarCuentas()

        dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

        Dim aCampos() As String = {"codcon.Código Contable.200.I.", _
                                   "nombre.Nombre o Descripción.2000.I."}
        ft.IniciarTablaPlus(dgCuentas, dt, aCampos)

        If dt.Rows.Count > 0 Then nPosicionCat = 0

    End Sub
    Private Sub AsignaCat(ByVal nRow As Long, ByVal Actualiza As Boolean)

        nPosicionCat = nRow

        If Actualiza Then dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

        If nPosicionCat >= 0 AndAlso dt.Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = nPosicionCat
            MostrarItemsEnMenuBarra(MenuBarra, CInt(nPosicionCat), dt.Rows.Count)
            dgCuentas.Refresh()
            dgCuentas.CurrentCell = dgCuentas(0, CInt(nPosicionCat))
        End If

        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
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

        'Adicionbales menu barra

    End Sub
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)

        If c >= 0 AndAlso dtMovimientos.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaMovimientos).Position = c
            MostrarItemsEnMenuBarra(MenuBarra, c, dtMovimientos.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)

        End If
        ft.ActivarMenuBarra(myConn, ds, dtMovimientos, lRegion, MenuBarra, jytsistema.sUsuario)
    End Sub

    Private Sub AsignaMes(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMovMes, myConn, nTablaMovMes, lblInfo)

        If c >= 0 AndAlso dtMovimientosMes.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaMovMes).Position = c
            MostrarItemsEnMenuBarra(MenuBarra, c, dtMovimientosMes.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)

        End If
        ft.ActivarMenuBarra(myConn, ds, dtMovimientosMes, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt

            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)
                txtCodigo.Text = .Item("codcon")
                txtNombre.Text = IIf(IsDBNull(.Item("descripcion")), "", .Item("descripcion"))



                Dim nCuentaActual As String = .Item("codcon").ToString
                AbrirMovimientoCuenta(nCuentaActual, DateTime.Parse(txtDesde.Text), DateTime.Parse(txtHasta.Text))

                txtCodigoS.Text = .Item("codcon")
                txtNombreS.Text = IIf(IsDBNull(.Item("descripcion")), "", .Item("descripcion"))

                AbrirSaldosPorMES(.Item("codcon"))

            End With
        End With

    End Sub
    Private Sub AbrirSaldosPorMES(CodigoCuenta As String)
        Dim tblTemp As String
        Dim mesCont As Integer
        Dim aMes() As String = {"Ejercicio Anterior", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"}

        tblTemp = "tbl" & ft.NumeroAleatorio(10000)

        Dim aFld() As String = {"nummes.entero.1.0", "mes.cadena.30.0", "debitos.doble.19.2", "creditos.doble.19.2", "saldo.doble.19.2", "control.cadena.5.0"}
        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblTemp, aFld, " nummes ")

        For mesCont = 0 To 12
            ft.Ejecutar_strSQL(myConn, " insert into " & tblTemp _
                & " select " & mesCont & " nummes , '" & aMes(mesCont) & "' mes, deb" & Format(mesCont, "00") & " debitos, " _
                & " Abs(cre" & Format(mesCont, "00") & ") creditos, (deb" & Format(mesCont, "00") & " + cre" & Format(mesCont, "00") & ") saldo, '' control " _
                & " from jscotdaacon " _
                & " Where " _
                & " codcon = '" & CodigoCuenta & "' and " _
                & " (isnull(ejercicio)  or ejercicio = '" & jytsistema.WorkExercise & "') and " _
                & " id_emp = '" & jytsistema.WorkID & "' " _
                & " ")
        Next

        strSQLMovMes = " select * from " & tblTemp & " order by nummes "
        ds = DataSetRequery(ds, strSQLMovMes, myConn, nTablaMovMes, lblInfo)
        dtMovimientosMes = ds.Tables(nTablaMovMes)
        Dim aCam() As String = {"mes", "debitos", "creditos", "saldo", "control"}
        Dim aNom() As String = {"Mes", "Débitos", "Créditos", "Saldo", ""}
        Dim aAnc() As Integer = {150, 200, 200, 200, 200}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, _
            AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", sFormatoNumero, sFormatoNumero, sFormatoNumero, ""}
        IniciarTabla(dgSaldos, dtMovimientosMes, aCam, aNom, aAnc, aAli, aFor)
        If dtMovimientosMes.Rows.Count > 0 Then nPosicionMes = 0

        ft.Ejecutar_strSQL(myConn, " drop temporary table " & tblTemp)
    End Sub

    Private Sub AbrirMovimientoCuenta(CuentaContable As String, FechaDesde As Date, FechaHasta As Date)

        strSQLMov = " select a.asiento, a.fechasi, b.codcon, b.referencia, b.concepto, b.importe " _
                           & " from jscotencasi a " _
                           & " left join jscotrenasi b on (a.asiento = b.asiento and a.actual = b.actual and a.ejercicio = b.ejercicio and a.id_emp = b.id_emp) " _
                           & " Where " _
                           & " a.fechasi >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and " _
                           & " a.fechasi <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
                           & " Substring(b.codcon,1, " & CuentaContable.Length & ") = '" & CuentaContable & "' and " _
                           & " a.actual = 1 and " _
                           & " (isnull(a.ejercicio)  or a.ejercicio = '" & jytsistema.WorkExercise & "') and " _
                           & " a.id_emp = '" & jytsistema.WorkID & "' " _
                           & " order by a.fechasi desc, a.asiento "

        dtMovimientos = ft.AbrirDataTable(ds, nTablaMovimientos, myConn, strSQLMov)

        Dim aCampos() As String = {"fechasi.FECHA.100.C.fecha", _
                                   "asiento.Asiento N°.150.I.", _
                                   "referencia.Referencia.200.I.", _
                                   "concepto.Concepto.500.I.", _
                                   "importe.Importe.150.D.Numero", _
                                   "sada..10.I."}

        ft.IniciarTablaPlus(dg, dtMovimientos, aCampos)
        If dtMovimientos.Rows.Count > 0 Then nPosicionMov = 0

        Dim nDebitos As Double = 0.0
        Dim nCreditos As Double = 0.0
        For Each nRow As DataRow In dtMovimientos.Rows
            With nRow
                If .Item("importe") < 0 Then
                    nDebitos += .Item("importe")
                Else
                    nCreditos += .Item("importe")
                End If
            End With
        Next
        txtTotalDebitos.Text = ft.FormatoNumero(nDebitos)
        txtTotalCreditos.Text = ft.FormatoNumero(nCreditos)
        txtTotalSaldo.Text = ft.FormatoNumero(nDebitos + nCreditos)

    End Sub
    Private Sub IniciarCuenta()

        txtCodigo.Text = ""
        txtNombre.Text = ""

        dg.Columns.Clear()

    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Select Case tbcCuentas.SelectedTab.Text
            Case "Cuentas contables"
                AsignaCat(0, False)
            Case "Movimientos"
                Me.BindingContext(ds, nTablaMovimientos).Position = 0
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Saldos mensuales"
                Me.BindingContext(ds, nTablaMovMes).Position = 0
                AsignaMes(Me.BindingContext(ds, nTablaMovMes).Position, False)
        End Select

    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Select Case tbcCuentas.SelectedTab.Text
            Case "Cuentas contables"
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaCat(Me.BindingContext(ds, nTabla).Position, False)
            Case "Movimientos"
                Me.BindingContext(ds, nTablaMovimientos).Position -= 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Saldos mensuales"
                Me.BindingContext(ds, nTablaMovMes).Position -= 1
                AsignaMes(Me.BindingContext(ds, nTablaMovMes).Position, False)
        End Select

    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Select Case tbcCuentas.SelectedTab.Text
            Case "Cuentas contables"
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaCat(Me.BindingContext(ds, nTabla).Position, False)
            Case "Movimientos"
                Me.BindingContext(ds, nTablaMovimientos).Position += 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Saldos mensuales"
                Me.BindingContext(ds, nTablaMovMes).Position += 1
                AsignaMes(Me.BindingContext(ds, nTablaMovMes).Position, False)
        End Select

    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Select Case tbcCuentas.SelectedTab.Text
            Case "Cuentas contables"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaCat(Me.BindingContext(ds, nTabla).Position, False)
            Case "Movimientos"
                Me.BindingContext(ds, nTablaMovimientos).Position = ds.Tables(nTablaMovimientos).Rows.Count - 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Saldos mensuales"
                Me.BindingContext(ds, nTablaMovMes).Position = ds.Tables(nTablaMovMes).Rows.Count - 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End Select

    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Select Case tbcCuentas.SelectedTab.Text
            Case "Cuentas contables"
                Dim f As New jsContabArcCuentasMovimientos
                Dim CuentaAnterior As String
                If Me.BindingContext(ds, nTabla).Position >= 0 Then
                    CuentaAnterior = ds.Tables(nTabla).Rows(Me.BindingContext(ds, nTabla).Position).Item("codcon")
                Else
                    CuentaAnterior = ""
                End If

                f.numCuenta = CuentaAnterior
                f.Agregar(myConn, ds, dt, CuentaAnterior)

                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                dt = ds.Tables(nTabla)
                If dt.Rows.Count > 0 Then
                    Dim row As DataRow = dt.Select(" codcon = '" & f.numCuenta & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
                    nPosicionCat = dt.Rows.IndexOf(row)
                    If f.Apuntador >= 0 Then AsignaCat(nPosicionCat, True)
                End If
                f = Nothing

            Case "Movimientos"
            Case "Saldos mensuales"
        End Select
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Select Case tbcCuentas.SelectedTab.Text
            Case "Cuentas contables"
                Dim f As New jsContabArcCuentasMovimientos
                f.Apuntador = Me.BindingContext(ds, nTabla).Position
                f.Editar(myConn, ds, dt)
                If f.Apuntador >= 0 Then AsignaCat(f.Apuntador, True)
                f = Nothing
            Case "Movimientos"
            Case "Saldos mensuales"
        End Select
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Select Case tbcCuentas.SelectedTab.Text
            Case "Cuentas contables"
                EliminaCuenta()
            Case "Movimientos"
            Case "Saldos mensuales"
        End Select
    End Sub
    Private Sub EliminaCuenta()

        Dim aCamposDel() As String = {"codcon", "id_emp"}
        Dim aStringsDel() As String = {dt.Rows(nPosicionCat).Item("codcon"), jytsistema.WorkID}

        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
            If dtMovimientos.Rows.Count = 0 Then
                ft.Ejecutar_strSQL(myConn, " delete from jscotdaacon where codcon = '" & dt.Rows(nPosicionCat).Item("codcon") & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                AsignaCat(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jscotcatcon", strSQL, aCamposDel, aStringsDel, _
                                              Me.BindingContext(ds, nTabla).Position, True), True)
            Else
                ft.mensajeAdvertencia("Esta cuenta contable posee movimientos. Verifique por favor ...")
            End If
        End If

    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar
        Select Case tbcCuentas.SelectedTab.Text
            Case "Cuentas contables"
                Dim Campos() As String = {"codcon", "descripcion"}
                Dim Nombres() As String = {"Código Contable", "Descripción"}
                Dim Anchos() As Integer = {200, 400}

                f.Apuntador = nPosicionCat
                f.Buscar(dt, Campos, Nombres, Anchos, nPosicionCat, "Cuentas contable...")
                nPosicionCat = f.Apuntador
                Me.BindingContext(ds, nTabla).Position = nPosicionCat
                AsignaCat(nPosicionCat, False)

                f = Nothing
            Case "Movimientos"
            Case "Saldos mensuales"
        End Select

    End Sub

    Private Sub tbcCuentas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcCuentas.SelectedIndexChanged
        Select Case tbcCuentas.SelectedIndex
            Case 0 'CUENTAS
            Case 1 'MOVIMIENTOS
                AsignaTXT(nPosicionCat)
            Case 2 'SALDOS MENSUALES

        End Select
    End Sub

    Private Sub btnColumnas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

        Select Case tbcCuentas.SelectedTab.Text
            Case "Cuentas contables"
                Dim f As New jsContabRepParametros
                f.Cargar(TipoCargaFormulario.iShowDialog, ReporteContabilidad.cCuentasContables, "Cuentas Contables")
                f = Nothing
            Case "Movimientos"
            Case "Saldos mensuales"
        End Select

    End Sub

    Private Sub dgCuentas_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) _
        Handles dgCuentas.RowHeaderMouseClick, dgCuentas.CellMouseClick

        If e.RowIndex >= 0 Then
            Me.BindingContext(ds, nTabla).Position = e.RowIndex
            nPosicionCat = e.RowIndex
            MostrarItemsEnMenuBarra(MenuBarra, e.RowIndex, ds.Tables(nTabla).Rows.Count)
            AsignaTXT(nPosicionCat)
        End If

    End Sub
    Private Sub dgSaldos_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dgSaldos.RowHeaderMouseClick, dgSaldos.CellMouseClick
        Me.BindingContext(ds, nTablaMovMes).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, e.RowIndex, ds.Tables(nTablaMovMes).Rows.Count)
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) _
        Handles dg.RowHeaderMouseClick, dg.CellMouseClick
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
    End Sub
    Private Sub dgCuentas_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgCuentas.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(ds, nTabla).Position += 1
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaCat(nPosicionCat, False)
            Case Keys.Up
                Me.BindingContext(ds, nTabla).Position -= 1
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaCat(nPosicionCat, False)
        End Select
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

    Private Sub dgSaldos_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgSaldos.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(ds, nTablaMovMes).Position += 1
                nPosicionMes = Me.BindingContext(ds, nTablaMovMes).Position
                AsignaMes(nPosicionMes, False)
            Case Keys.Up
                Me.BindingContext(ds, nTablaMovMes).Position -= 1
                nPosicionMes = Me.BindingContext(ds, nTablaMovMes).Position
                AsignaMes(nPosicionMes, False)
        End Select
    End Sub

    Private Sub txtDesde_TextChanged(sender As Object, e As EventArgs) Handles txtDesde.TextChanged
        txtHasta.Text = txtDesde.Text
    End Sub
    Private Sub txtHasta_TextChanged(sender As Object, e As EventArgs) Handles txtHasta.TextChanged
        If tbcCuentas.SelectedIndex = 1 Then _
            AbrirMovimientoCuenta(txtCodigo.Text, Convert.ToDateTime(txtDesde.Text), Convert.ToDateTime(txtHasta.Text))
    End Sub

    Private Sub btnDesde_Click(sender As Object, e As EventArgs) Handles btnDesde.Click
        txtDesde.Text = SeleccionaFecha(Convert.ToDateTime(txtDesde.Text), Me, tbcCuentas, btnDesde)
    End Sub

    Private Sub btnHasta_Click(sender As Object, e As EventArgs) Handles btnHasta.Click
        txtHasta.Text = SeleccionaFecha(Convert.ToDateTime(txtHasta.Text), Me, tbcCuentas, btnHasta)
    End Sub

   
End Class