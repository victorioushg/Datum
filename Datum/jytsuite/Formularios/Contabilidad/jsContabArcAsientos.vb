Imports MySql.Data.MySqlClient
Imports fTransport
Public Class jsContabArcAsientos
    Private sModulo As String = "Asientos Contables "
    Private lRegion As String = ""
    Private Const nTabla As String = "Asiento"
    Private Const nTablaMovimientos As String = "movimientos_asiento"

    Private strSQL As String = ""

    Private strSQLMov As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtMovimientos As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long
    Private TipoAsi As Integer
    Private PlantillaOrigen As String = ""
    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal TipoAsiento As Integer)

        Me.Dock = DockStyle.Fill
        myConn = MyCon
        TipoAsi = TipoAsiento
        sModulo += IIf(TipoAsiento = Asiento.iDiferido, " Diferidos ", " Actuales ")
        lRegion = IIf(TipoAsiento = Asiento.iDiferido, "RibbonButton2", "RibbonButton3")
        Label1.Text = IIf(TipoAsiento = Asiento.iDiferido, "Asiento Diferido", "Asiento Actual")

        If TipoAsiento = Asiento.iDiferido Then
            btnAActual.Visible = True
            btnDeActual.Visible = False
        Else
            btnAActual.Visible = False
            btnDeActual.Visible = True
        End If


        Me.Tag = sModulo



        strSQL = " select * from jscotencasi " _
            & " Where " _
            & " actual = " & TipoAsi & " and " _
            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " id_emp = '" & jytsistema.WorkID & "' " _
            & " order by asiento "

        dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)
        DesactivarMarco0()

        If dt.Rows.Count > 0 Then
            nPosicionEncab = dt.Rows.Count - 1
            AsignaTXT(nPosicionEncab)
        Else
            IniciarAsiento(False)
        End If

        Dim iArray() As String = {"btnReconstruir"}

        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario, iArray)

        AsignarTooltips()

        Me.Show()


    End Sub

    Private Sub jsContabArcAsientos_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsContabArcAsientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
    End Sub

    Private Sub AsignarTooltips()

        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnPrimero, btnSiguiente, btnAnterior, _
                          btnUltimo, btnImprimir, btnSalir, btnAActual, btnDeActual, btnReconstruir, btnAgregarMovimiento, _
                          btnEditarMovimiento, btnEliminarMovimiento, btnBuscarMovimiento, btnPrimerMovimiento, btnAnteriorMovimiento, _
                          btnSiguienteMovimiento, btnUltimoMovimiento, btnSubir, btnBajar)
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnFecha)

    End Sub
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)

        dtMovimientos = ft.MostrarFilaEnTabla(myConn, ds, nTablaMovimientos, strSQLMov, Me.BindingContext, MenuBarraRenglon, _
                               dg, lRegion, jytsistema.sUsuario, nRow, Actualiza)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)
        If dt.Rows.Count > 0 Then
            With dt
                If nRow >= 0 Then
                    Me.BindingContext(ds, nTabla).Position = nRow
                    With .Rows(nRow)
                        'Asiento 
                        txtAsiento.Text = ft.muestraCampoTexto(.Item("asiento"))
                        txtDescripcion.Text = ft.muestraCampoTexto(.Item("descripcion"))
                        txtFecha.Text = ft.muestraCampoFecha(.Item("fechasi"))
                        PlantillaOrigen = ft.muestraCampoTexto(.Item("plantilla_origen"))
                        AbrirMovimientos(.Item("asiento"))
                        CalculaTotales()
                    End With
                End If
            End With
        Else
            nRow = 0
            ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtAsiento, txtDescripcion, txtDebitos, txtCreditos, txtSaldos)
            PlantillaOrigen = ""
        End If

        MostrarItemsEnMenuBarra(MenuBarra, nRow, dt.Rows.Count)

    End Sub
    Private Sub AbrirMovimientos(ByVal NumeroAsiento As String)

        strSQLMov = " select a.*, b.descripcion from jscotrenasi a" _
            & " left join jscotcatcon b on (a.codcon = b.codcon and a.id_emp = b.id_emp) " _
            & " where " _
            & " a.asiento = '" & NumeroAsiento & "' and " _
            & " a.actual = " & TipoAsi & " and " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by renglon "

        dtMovimientos = ft.AbrirDataTable(ds, nTablaMovimientos, myConn, strSQLMov)

        Dim aCampos() As String = {"codcon.Código cuenta.150.I.", _
                                   "descripcion.Descripción.250.I.", _
                                   "referencia.Nº Referencia.200.I.", _
                                   "concepto.Concepto.300.I.", _
                                   "importe.Importe.160.D.Numero", _
                                   "sada..100.I."}

        ft.IniciarTablaPlus(dg, dtMovimientos, aCampos)

        If dtMovimientos.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub
    Private Sub IniciarAsiento(ByVal Inicio As Boolean)
        If Inicio Then
            Dim TipoAutocodigo As Integer = ft.DevuelveScalarEntero(myConn, " select valor from jsconparametros where gestion = " & Gestion.iContabilidad & " and codigo = 'conparam05' and id_emp = '" & jytsistema.WorkID & "'  ")
            If TipoAutocodigo = 0 Then
                txtAsiento.Text = Contador(myConn, lblInfo, Gestion.iContabilidad, "connumasi", "01")
            Else
                txtAsiento.Text = "ASITMP" + ft.NumeroAleatorio(10000000).ToString
            End If
        Else
            txtAsiento.Text = ""
        End If
        txtDescripcion.Text = ""
        txtFecha.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtDebitos.Text = ft.FormatoNumero(0.0)
        txtCreditos.Text = ft.FormatoNumero(0.0)
        txtSaldos.Text = ft.FormatoNumero(0.0)

        PlantillaOrigen = ""

        'Movimientos
        dg.Columns.Clear()
        If txtAsiento.Text <> "" Then AbrirMovimientos(txtAsiento.Text)
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)


    End Sub
    Private Sub ActivarMarco0()

        Dim param As Integer = CInt(ParametroPlus(MyConn, Gestion.iContabilidad, "conparam04"))
        grpAceptarSalir.Visible = True
        ft.habilitarObjetos(True, False, grpAceptarSalir, grpEncab)
        ft.habilitarObjetos(True, True, txtDescripcion, btnFecha)
        If param = 1 Then ft.habilitarObjetos(True, True, txtAsiento)

        MenuBarra.Enabled = False
        MenuBarraRenglon.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        ft.habilitarObjetos(False, True, grpEncab, grpTotales)
        ft.habilitarObjetos(False, True, txtAsiento, txtFecha, btnFecha, txtDescripcion, txtDebitos, txtCreditos, txtSaldos)

        grpEncab.Enabled = False
        grpAceptarSalir.Visible = False
        MenuBarra.Enabled = True
        MenuBarraRenglon.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If dt.Rows.Count = 0 Then
            IniciarAsiento(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position
            If nPosicionEncab >= 0 Then AsignaTXT(nPosicionEncab)
        End If
        DesactivarMarco0()
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado() As Boolean

        If txtAsiento.Text <> "" Then
            Dim afld() As String = {"asiento", "id_emp"}
            Dim aStr() As String = {txtAsiento.Text, jytsistema.WorkID}
            If i_modo = movimiento.iAgregar AndAlso qFound(myConn, lblInfo, "jscotencasi", afld, aStr) Then
                ft.mensajeAdvertencia("Código de asiento YA existe, por favor verifique...")
                Return False
            End If
        Else
            ft.mensajeAdvertencia("Debe indicar un código de asiento válido...")
            Return False
        End If

        If txtDescripcion.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar una descripción Válida ...")
            txtDescripcion.Focus()
            Return False
        End If

        If CDate(txtFecha.Text) > FechaCierreEjercicio(myConn, lblInfo) Then
            ft.mensajeAdvertencia("Fecha Asiento mayor que fecha Cierre de ejercicio ...")
            btnFecha.Focus()
            Return False
        End If

        If FechaUltimoBloqueo(myConn, "jscotencasi") >= Convert.ToDateTime(txtFecha.Text) Then
            ft.mensajeCritico("FECHA ASIENTO MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If TipoAsi = Asiento.iActual AndAlso (ValorNumero(txtDebitos.Text) + ValorNumero(txtCreditos.Text) <> 0.0#) Then
            ft.mensajeAdvertencia("El saldo debe ser igual a CERO (0) ...")
            Return False
        End If

        If i_modo = movimiento.iAgregar AndAlso CDate(txtFecha.Text) < FechaInicioEjercicio(myConn, lblInfo) Then
            ft.mensajeAdvertencia("Fecha asiento menor que Fecha Inicio de ejercicio ..." & vbCrLf & _
                 " Este asiento se contabilizará como asiento de apertura")
        End If

        Return True

    End Function
    Private Sub GuardarTXT()

        Dim Inserta As Boolean = False
        Dim numAsiento As String = txtAsiento.Text
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count
            Dim TipoAutocodigo As Integer = ft.DevuelveScalarEntero(myConn, " select valor from jsconparametros where gestion = " & Gestion.iContabilidad & " and codigo = 'conparam05' and id_emp = '" & jytsistema.WorkID & "'  ")
            Select Case TipoAutocodigo
                Case 0 'unica
                    numAsiento = Contador(myConn, lblInfo, Gestion.iContabilidad, "connumasi", "01")
                Case 1 'Diaria
                    numAsiento = AutoCodigoDiario(myConn, lblInfo, "jscotencasi", "asiento", CDate(txtFecha.Text))
                Case 2 'Mensual
                    numAsiento = AutoCodigoMensual(myConn, lblInfo, "jscotencasi", "asiento", CDate(txtFecha.Text))
            End Select


        End If

        ft.Ejecutar_strSQL(myconn, " update jscotrenasi set asiento = '" & numAsiento & "' " _
                            + " where " _
                            + " asiento = '" & txtAsiento.Text & "' and " _
                            + " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            + " id_emp = '" & jytsistema.WorkID & "' ")

        InsertEditCONTABEncabezadoAsiento(myConn, lblInfo, Inserta, numAsiento, numAsiento, CDate(txtFecha.Text), _
                                           txtDescripcion.Text, ValorNumero(txtDebitos.Text), _
                                           ValorNumero(txtCreditos.Text), TipoAsi, PlantillaOrigen)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        ActualizaCuentasSegunAsiento(myConn, lblInfo, ds, numAsiento, CDate(txtFecha.Text))
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    'Public Sub ActualizaCuentasSegunAsiento(MyConn As MySqlConnection, lblInfo As Label, numAsiento As String)

    '    Dim dtMov As DataTable
    '    Dim nTablaMov As String = "tblMov1"
    '    ds = DataSetRequery(ds, " select * from jscotrenasi where asiento = '" & numAsiento & "' and id_emp = '" & jytsistema.WorkID & "' order by renglon ", _
    '                         MyConn, nTablaMov, lblInfo)
    '    dtMov = ds.Tables(nTablaMov)

    '    If dtMov.Rows.Count > 0 Then
    '        For Each nRow As DataRow In dtMov.Rows
    '            With nRow
    '                ActualizaSaldosCuentasContables(MyConn, lblInfo, .Item("codcon"), CDate(txtFecha.Text))
    '            End With
    '        Next
    '    End If

    'End Sub

    Private Sub txtDescripcion_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescripcion.GotFocus, _
        btnFecha.GotFocus
        ft.colocaMensajeEnEtiqueta(sender, jytsistema.WorkLanguage, lblInfo)

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarAsiento(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        If dt.Rows.Count > 0 Then
            Dim strEli() As String = {"asiento|'" & txtAsiento.Text & "'", "actual|'" & TipoAsi & "'"}
            If DocumentoBloqueado(myConn, "jscotencasi", strEli) Then
                ft.mensajeCritico("ESTE DOCUMENTO ESTA BLOQUEADO. POR FAVOR VERIFIQUE...")
            Else
                i_modo = movimiento.iEditar
                nPosicionEncab = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
            End If
        End If
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        If dt.Rows.Count > 0 Then

            Dim strEli() As String = {"asiento|'" & txtAsiento.Text & "'", "actual|'" & TipoAsi & "'"}

            If DocumentoBloqueado(myConn, "jscotencasi", strEli) Then
                ft.mensajeCritico("ESTE DOCUMENTO ESTA BLOQUEADO. POR FAVOR VERIFIQUE...")
            Else
                Dim aFld() As String = {"asiento", "ejercicio", "id_emp"}
                Dim aSFld() As String = {txtAsiento.Text, jytsistema.WorkExercise, jytsistema.WorkID}

                If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                    nPosicionEncab = EliminarRegistros(myConn, lblInfo, ds, nTabla, "jscotencasi", strSQL, aFld, aSFld, nPosicionEncab)
                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaMovimientos, "jscotrenasi", strSQLMov, aFld, aSFld, nPosicionRenglon)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, txtAsiento.Text)
                    ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                    dt = ds.Tables(nTabla)
                    If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                    AsignaTXT(nPosicionEncab)
                End If

            End If

        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"asiento", "descripcion", "fechaSI"}
        Dim Nombres() As String = {"Código Asiento", "Descripción", "Fecha"}
        Dim Anchos() As Integer = {100, 400, 100}

        f.Apuntador = nPosicionEncab
        f.Buscar(dt, Campos, Nombres, Anchos, nPosicionEncab, "Asientos Contables...")
        nPosicionEncab = f.Apuntador
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)

        f = Nothing
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Me.BindingContext(ds, nTabla).Position = 0
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        AsignaTXT(nPosicionEncab)
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Me.BindingContext(ds, nTabla).Position -= 1
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        AsignaTXT(nPosicionEncab)
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Me.BindingContext(ds, nTabla).Position += 1
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        AsignaTXT(nPosicionEncab)
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        AsignaTXT(nPosicionEncab)
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        nPosicionEncab = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
    End Sub

    Private Sub CalculaTotales()
        Dim dtTotal As DataTable
        Dim nTablaTotal As String = "tbltotal"
        ds = DataSetRequery(ds, " select sum( if( debito_credito = 0,  importe,0 )) debitos, " _
            & " sum( if(debito_credito = 1 , importe, 0 )) creditos " _
            & " from jscotrenasi " _
            & " where asiento = '" & txtAsiento.Text & "' and " _
            & " actual = " & TipoAsi & " and " _
            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " id_emp = '" & jytsistema.WorkID & "' group by asiento ", myConn, nTablaTotal, lblInfo)

        dtTotal = ds.Tables(nTablaTotal)
        txtDebitos.Text = ft.FormatoNumero(0.0)
        txtCreditos.Text = ft.FormatoNumero(0.0)
        If dtTotal.Rows.Count > 0 Then
            With dtTotal.Rows(0)
                txtDebitos.Text = ft.FormatoNumero(.Item("debitos"))
                txtCreditos.Text = ft.FormatoNumero(.Item("creditos"))
            End With
        End If

        dtTotal.Dispose()
        dtTotal = Nothing

    End Sub
    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click

        Dim f As New jsContabArcAsientosMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaMovimientos).Position
        Dim CodigoReferencia As String = ""
        If dtMovimientos.Rows.Count > 0 Then CodigoReferencia = dtMovimientos.Rows(Me.BindingContext(ds, nTablaMovimientos).Position).Item("codcon").ToString

        f.Agregar(myConn, ds, dtMovimientos, txtAsiento.Text, TipoAsi, CDbl(txtSaldos.Text), CodigoReferencia)
        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        If f.Apuntador > 0 Then AsignaMov(f.Apuntador, True)
        f = Nothing

        CalculaTotales()

    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        Dim f As New jsContabArcAsientosMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaMovimientos).Position
        f.Editar(myConn, ds, dtMovimientos, txtAsiento.Text, TipoAsi)
        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        AsignaMov(f.Apuntador, True)
        f = Nothing

        CalculaTotales()

    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click

        EliminarMovimiento()
        CalculaTotales()

    End Sub
    Private Sub EliminarMovimiento()


        nPosicionRenglon = Me.BindingContext(ds, nTablaMovimientos).Position

        If nPosicionRenglon >= 0 Then

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                With dtMovimientos.Rows(nPosicionRenglon)
                    Dim CodigoEliminado As String
                    CodigoEliminado = .Item("codcon")
                    Dim aFld() As String = {"asiento", "renglon", "codcon", "referencia", "actual", "ejercicio", "id_emp"}
                    Dim aNFld() As String = {.Item("asiento"), .Item("renglon"), .Item("codcon"), .Item("referencia"), .Item("actual"), jytsistema.WorkExercise, jytsistema.WorkID}
                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaMovimientos, "jscotrenasi", strSQLMov, aFld, aNFld, nPosicionRenglon)
                    If dtMovimientos.Rows.Count > 0 Then ActualizaSaldosCuentasContables(myConn, lblInfo, CodigoEliminado, CDate(dt.Rows(nPosicionEncab).Item("fechasi").ToString))
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, CodigoEliminado)

                End With

                ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                dtMovimientos = ds.Tables(nTablaMovimientos)
                If dtMovimientos.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtMovimientos.Rows.Count - 1

                AsignaMov(nPosicionRenglon, False)
            End If

        End If

    End Sub
    Private Sub btnBuscarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarMovimiento.Click

        Dim f As New frmBuscar
        Dim Campos() As String = {"codcon", "referencia", "concepto"}
        Dim Nombres() As String = {"Codigo Contable", "Referencia", "Concepto"}
        Dim Anchos() As Integer = {150, 250, 1500}
        f.Buscar(dtMovimientos, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaMovimientos).Position, "Movimientos de asientos contables...")
        AsignaMov(f.Apuntador, False)
        f = Nothing

    End Sub

    Private Sub btnPrimerMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimerMovimiento.Click
        Me.BindingContext(ds, nTablaMovimientos).Position = 0
        AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
    End Sub

    Private Sub btnAnteriorMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnteriorMovimiento.Click
        Me.BindingContext(ds, nTablaMovimientos).Position -= 1
        AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
    End Sub

    Private Sub btnSiguienteMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguienteMovimiento.Click
        Me.BindingContext(ds, nTablaMovimientos).Position += 1
        AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
    End Sub

    Private Sub btnUltimoMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimoMovimiento.Click
        Me.BindingContext(ds, nTablaMovimientos).Position = ds.Tables(nTablaMovimientos).Rows.Count - 1
        AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsContabRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, IIf(TipoAsi = Asiento.iDiferido, ReporteContabilidad.cPolizaDiferida, ReporteContabilidad.cPolizaActual), _
                 IIf(TipoAsi = Asiento.iDiferido, "Asiento Diferido", "Asiento Actual"), txtAsiento.Text)
        f = Nothing
    End Sub
    Private Sub btnSubir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubir.Click
        Dim numRenglon As Integer
        Dim RenglonDestino As String = ""
        If dtMovimientos.Rows.Count > 0 Then
            nPosicionRenglon = Me.BindingContext(ds, nTablaMovimientos).Position
            If dtMovimientos.Rows(0).Item("renglon") <> "" Then
                If dtMovimientos.Rows(nPosicionRenglon).Item("renglon") > dtMovimientos.Rows(0).Item("renglon") Then
                    numRenglon = CType(dtMovimientos.Rows(nPosicionRenglon).Item("renglon").ToString, Integer)
                    RenglonDestino = Format(numRenglon - 1, "00000")
                    Dim afld() As String = {"asiento", "renglon", "ejercicio", "id_emp"}
                    Dim asFld() As String = {txtAsiento.Text, RenglonDestino, jytsistema.WorkExercise, jytsistema.WorkID}
                    If qFound(myConn, lblInfo, "jscotrenasi", afld, asFld) Then
                        ft.Ejecutar_strSQL(myconn, " update jscotrenasi set renglon = 'XXXXX' where asiento = '" & txtAsiento.Text & "' and renglon = '" & RenglonDestino & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        ft.Ejecutar_strSQL(myconn, " update jscotrenasi set renglon = '" & RenglonDestino & "' where asiento = '" & txtAsiento.Text & "' and renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'")
                        ft.Ejecutar_strSQL(myconn, " update jscotrenasi set renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' where asiento = '" & txtAsiento.Text & "' and renglon = 'XXXXX' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'")
                    Else
                        ft.Ejecutar_strSQL(myconn, " update jscotrenasi set renglon = '" & RenglonDestino & "' where asiento = '" & txtAsiento.Text & "' and renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                    End If
                    nPosicionRenglon -= 1
                    AsignaMov(nPosicionRenglon, True)
                End If
            End If
        End If
    End Sub

    Private Sub btnBajar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBajar.Click
        Dim numRenglon As Integer
        Dim RenglonDestino As String = ""
        If dtMovimientos.Rows.Count > 0 Then
            nPosicionRenglon = Me.BindingContext(ds, nTablaMovimientos).Position
            If dtMovimientos.Rows(dtMovimientos.Rows.Count - 1).Item("renglon") <> "" Then
                If dtMovimientos.Rows(nPosicionRenglon).Item("renglon") < dtMovimientos.Rows(dtMovimientos.Rows.Count - 1).Item("renglon") Then
                    numRenglon = CType(dtMovimientos.Rows(nPosicionRenglon).Item("renglon").ToString, Integer)
                    RenglonDestino = Format(numRenglon + 1, "00000")
                    Dim afld() As String = {"asiento", "renglon", "ejercicio", "id_emp"}
                    Dim asFld() As String = {txtAsiento.Text, RenglonDestino, jytsistema.WorkExercise, jytsistema.WorkID}
                    If qFound(myConn, lblInfo, "jscotrenasi", afld, asFld) Then
                        ft.Ejecutar_strSQL(myconn, " update jscotrenasi set renglon = 'XXXXX' where asiento = '" & txtAsiento.Text & "' and renglon = '" & RenglonDestino & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        ft.Ejecutar_strSQL(myconn, " update jscotrenasi set renglon = '" & RenglonDestino & "' where asiento = '" & txtAsiento.Text & "' and renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        ft.Ejecutar_strSQL(myconn, " update jscotrenasi set renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' where asiento = '" & txtAsiento.Text & "' and renglon = 'XXXXX' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                    Else
                        ft.Ejecutar_strSQL(myconn, " update jscotrenasi set renglon = '" & RenglonDestino & "' where asiento = '" & txtAsiento.Text & "' and renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                    End If
                    nPosicionRenglon += 1
                    AsignaMov(nPosicionRenglon, True)
                End If
            End If
        End If
    End Sub
    Private Sub txtDebitos_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDebitos.TextChanged, _
        txtCreditos.TextChanged
        txtSaldos.Text = ft.FormatoNumero(ValorNumero(txtDebitos.Text) + ValorNumero(txtCreditos.Text))
    End Sub

    Private Sub btnFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFecha.Click
        txtFecha.Text = SeleccionaFecha(CDate(txtFecha.Text), Me, btnFecha)
    End Sub

    Private Sub txtAsiento_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAsiento.TextChanged
        AbrirMovimientos(txtAsiento.Text)
    End Sub

    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(ds, nTablaMovimientos).Position += 1
                nPosicionRenglon = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(nPosicionRenglon, False)
            Case Keys.Up
                Me.BindingContext(ds, nTablaMovimientos).Position -= 1
                nPosicionRenglon = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(nPosicionRenglon, False)
        End Select
    End Sub

    Private Sub btnAActual_Click(sender As System.Object, e As System.EventArgs) Handles btnAActual.Click
        Dim AsientoActual As String = txtAsiento.Text
       
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position

        If ft.Pregunta("Desea pasar asiento a ACTUAL", sModulo) = Windows.Forms.DialogResult.Yes Then
            If ft.DevuelveScalarEntero(myConn, " select count(*) from jscotrenasi where trim(codcon) = '' AND asiento = '" & AsientoActual & "' and ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                If ValorNumero(txtSaldos.Text) = 0 Then

                    ft.Ejecutar_strSQL(myConn, " update jscotencasi set actual = " & Asiento.iActual & " where asiento = '" & AsientoActual & "' and ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " update jscotrenasi set actual = " & Asiento.iActual & " where asiento = '" & AsientoActual & "' and ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ")

                    ActualizaCuentasSegunAsiento(myConn, lblInfo, ds, AsientoActual, CDate(txtFecha.Text))

                    ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                    dt = ds.Tables(nTabla)
                    If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                    AsignaTXT(nPosicionEncab)
                Else
                    ft.mensajeCritico("ASIENTO DESCUADRADO. VERIFIQUE POR FAVOR...")
                End If
            Else
                ft.mensajeCritico("EXISTEN MOVIMIENTOS EN ESTE ASIENTO SIN CODIGO CONTABLE. VERIFIQUE POR FAVOR...")
            End If

        End If


    End Sub

    Private Sub btnDeActual_Click(sender As System.Object, e As System.EventArgs) Handles btnDeActual.Click

        Dim AsientoActual As String = txtAsiento.Text


        nPosicionEncab = Me.BindingContext(ds, nTabla).Position

        If ft.Pregunta("Desea transferir este asiento a DIFERIDO", sModulo) = Windows.Forms.DialogResult.Yes Then

            ft.Ejecutar_strSQL(myConn, " update jscotencasi set actual = " & Asiento.iDiferido & " where asiento = '" & AsientoActual & "' and ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jscotrenasi set actual = " & Asiento.iDiferido & " where asiento = '" & AsientoActual & "' and ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ")
            ActualizaCuentasSegunAsiento(myConn, lblInfo, ds, AsientoActual, CDate(txtFecha.Text))
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)
            If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
            AsignaTXT(nPosicionEncab)
        End If
    End Sub

    Private Sub btnReconstruir_Click(sender As System.Object, e As System.EventArgs) Handles btnReconstruir.Click
        If TipoAsi = Asiento.iDiferido Then
            If PlantillaOrigen <> "" Then
                ReconstruirAsientoActual()
                RefrescarAsientos(nPosicionEncab)
                ft.mensajeInformativo("Reconstrucción asiento terminada!!!")
            Else
                ft.mensajeCritico("No es posible reconstruir. Asiento Manual... ")
            End If
        End If
    End Sub
    Private Sub ReconstruirAsientoActual()
        If dt.Rows.Count > 0 Then
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position
            With dt.Rows(nPosicionEncab)
                Dim nPlantillaOrigen As String = .Item("PLANTILLA_ORIGEN")
                If nPlantillaOrigen <> "" Then
                    Dim lblp As New Label
                    Dim pb As New ProgressBar

                    ft.Ejecutar_strSQL(myconn, " delete from jscotencasi " _
                                   & " where " _
                                   & " asiento = '" & txtAsiento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")


                    ft.Ejecutar_strSQL(myconn, " delete from jscotrenasi " _
                                   & " where " _
                                   & " asiento = '" & txtAsiento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                    Contabilizar_Plantilla(myConn, lblInfo, ds, CDate(txtFecha.Text), nPlantillaOrigen, _
                                            txtDescripcion.Text, CDate(txtFecha.Text), CDate(txtFecha.Text), _
                                            lblp, pb, txtAsiento.Text)

                End If
            End With


        End If

    End Sub

    Private Sub ReconstruirAsiento()
        If dt.Rows.Count > 0 Then

        End If
    End Sub
    Private Sub RefrescarAsientos(nNuevaPosicion As Long)
        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        DesactivarMarco0()

        If dt.Rows.Count > 0 Then
            'nPosicionEncab = dt.Rows.Count - 1
            AsignaTXT(nNuevaPosicion)
        Else
            IniciarAsiento(False)
        End If
        Dim iArray() As String = {"btnReconstruir"}
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario, iArray)
    End Sub

End Class