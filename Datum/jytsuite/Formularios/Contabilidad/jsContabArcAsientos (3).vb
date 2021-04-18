Imports MySql.Data.MySqlClient
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

        InsertarAuditoria(myConn, MovAud.ientrar, sModulo, "")

        strSQL = " select * from jscotencasi " _
            & " Where " _
            & " actual = " & TipoAsi & " and " _
            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " id_emp = '" & jytsistema.WorkID & "' " _
            & " order by asiento "

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        DesactivarMarco0()

        If dt.Rows.Count > 0 Then
            nPosicionEncab = dt.Rows.Count - 1
            AsignaTXT(nPosicionEncab)
        Else
            IniciarAsiento(False)
        End If

        Dim iArray() As String = {"btnReconstruir"}
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra, iArray)
        AsignarTooltips()

        Me.Show()


    End Sub
    Private Sub jsContabArcAsientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
    End Sub

    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo asiento contable")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> asiento contable deseado")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir al <B>primera</B> asiento contable")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a <B>siguiente</B> asiento contable")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a asiento contable <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>último  asiento contable</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> movimientos de asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnAActual, "Pasar asiento a <B>actual</B>")
        C1SuperTooltip1.SetToolTip(btnDeActual, "Reversar asiento a <B>diferido</B>")
        C1SuperTooltip1.SetToolTip(btnReconstruir, "Refrescar asientos contables")
        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> movimiento en asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> movimiento de asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> movimiento de asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un movimiento en asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> movimiento de asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> movimiento de asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al movimiento <B>siguiente </B> de asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> movimiento de asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnSubir, " <B>Subir</B> renglón en el orden de precedencia")
        C1SuperTooltip1.SetToolTip(btnBajar, "<B>Bajar</B> renglón en el orden de precedencia")

    End Sub
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)

        If c >= 0 Then
            Me.BindingContext(ds, nTablaMovimientos).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", c, dtMovimientos.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtMovimientos, MenuBarraRenglon)
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)
        If dt.Rows.Count > 0 Then
            With dt
                If nRow >= 0 Then
                    Me.BindingContext(ds, nTabla).Position = nRow
                    With .Rows(nRow)
                        'Asiento 
                        txtAsiento.Text = .Item("asiento")
                        txtDescripcion.Text = IIf(IsDBNull(.Item("descripcion")), "", .Item("descripcion"))
                        txtFecha.Text = FormatoFecha(CDate(.Item("fechasi").ToString))
                        PlantillaOrigen = .Item("plantilla_origen").ToString
                        AbrirMovimientos(.Item("asiento").ToString)
                        CalculaTotales()
                    End With
                End If
            End With
        Else
            nRow = 0
            txtAsiento.Text = ""
            txtDescripcion.Text = ""
            txtDebitos.Text = ""
            txtCreditos.Text = ""
            txtSaldos.Text = ""
            PlantillaOrigen = ""
            MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", 0, 0)
        End If
        MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, dt.Rows.Count)

    End Sub
    Private Sub AbrirMovimientos(ByVal NumeroAsiento As String)

        strSQLMov = " select a.*, b.descripcion from jscotrenasi a" _
            & " left join jscotcatcon b on (a.codcon = b.codcon and a.id_emp = b.id_emp) " _
            & " where " _
            & " a.asiento = '" & NumeroAsiento & "' and " _
            & " a.actual = " & TipoAsi & " and " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        dtMovimientos = ds.Tables(nTablaMovimientos)

        Dim aCampos() As String = {"codcon", "descripcion", "referencia", "concepto", "importe", ""}
        Dim aNombres() As String = {"Código cuenta", "Descripción", "Nº Referencia", "Concepto", "Importe", ""}
        Dim aAnchos() As Long = {150, 200, 120, 220, 160, 10}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {"", "", "", "", sFormatoNumero, ""}
        IniciarTabla(dg, dtMovimientos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtMovimientos.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub
    Private Sub IniciarAsiento(ByVal Inicio As Boolean)
        If Inicio Then
            Dim TipoAutocodigo As Integer = CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select valor from jsconparametros where gestion = " & Gestion.iContabilidad & " and codigo = 'conparam05' and id_emp = '" & jytsistema.WorkID & "'  "))
            If TipoAutocodigo = 0 Then
                txtAsiento.Text = Contador(myConn, lblInfo, Gestion.iContabilidad, "connumasi", "01")
            Else
                txtAsiento.Text = "ASITMP" + Format(NumeroAleatorio(10000000), "00000000")
            End If
        Else
            txtAsiento.Text = ""
        End If
        txtDescripcion.Text = ""
        txtFecha.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        txtDebitos.Text = FormatoNumero(0.0)
        txtCreditos.Text = FormatoNumero(0.0)
        txtSaldos.Text = FormatoNumero(0.0)

        'Movimientos
        dg.Columns.Clear()
        If txtAsiento.Text <> "" Then AbrirMovimientos(txtAsiento.Text)
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", 0, 0)


    End Sub
    Private Sub ActivarMarco0()

        Dim param As Integer = CInt(ParametroPlus(MyConn, Gestion.iContabilidad, "conparam04"))
        grpAceptarSalir.Visible = True
        HabilitarObjetos(True, False, grpAceptarSalir, grpEncab)
        HabilitarObjetos(True, True, txtDescripcion, btnFecha)
        If param = 1 Then HabilitarObjetos(True, True, txtAsiento)

        MenuBarra.Enabled = False
        MenuBarraRenglon.Enabled = True

        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        HabilitarObjetos(False, True, grpEncab, grpTotales)
        HabilitarObjetos(False, True, txtAsiento, txtFecha, btnFecha, txtDescripcion, txtDebitos, txtCreditos, txtSaldos)

        grpEncab.Enabled = False
        grpAceptarSalir.Visible = False
        MenuBarra.Enabled = True
        MenuBarraRenglon.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

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
        Validado = False

        If txtAsiento.Text <> "" Then
            Dim afld() As String = {"asiento", "id_emp"}
            Dim aStr() As String = {txtAsiento.Text, jytsistema.WorkID}
            If i_modo = movimiento.iAgregar AndAlso qFound(myConn, lblInfo, "jscotencasi", afld, aStr) Then
                MensajeAdvertencia(lblInfo, "Código de asiento YA existe, por favor verifique...")
                Exit Function
            End If
        Else
            MensajeAdvertencia(lblInfo, "Debe indicar un código de asiento válido...")
            Exit Function
        End If

        If txtDescripcion.Text = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar una descripción Válida ...")
            txtDescripcion.Focus()
            Exit Function
        End If

        If CDate(txtFecha.Text) > FechaCierreEjercicio(myConn, lblInfo) Then
            MensajeAdvertencia(lblInfo, "Fecha Asiento mayor que fecha Cierre de ejercicio ...")
            btnFecha.Focus()
            Exit Function
        End If

        If TipoAsi = Asiento.iActual AndAlso (ValorNumero(txtDebitos.Text) + ValorNumero(txtCreditos.Text) <> 0.0#) Then
            MensajeAdvertencia(lblInfo, "El saldo debe ser igual a CERO (0) ...")
            Exit Function
        End If

        If i_modo = movimiento.iAgregar AndAlso CDate(txtFecha.Text) < FechaInicioEjercicio(myConn, lblInfo) Then
            MensajeAdvertencia(lblInfo, "Fecha asiento menor que Fecha Inicio de ejercicio ..." & vbCrLf & _
                 " Este asiento se contabilizará como asiento de apertura")
        End If

        Validado = True

    End Function
    Private Sub GuardarTXT()

        Dim Inserta As Boolean = False
        Dim numAsiento As String = txtAsiento.Text
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count
            Dim TipoAutocodigo As Integer = CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select valor from jsconparametros where gestion = " & Gestion.iContabilidad & " and codigo = 'conparam05' and id_emp = '" & jytsistema.WorkID & "'  "))
            Select Case TipoAutocodigo
                Case 0 'unica
                    numAsiento = Contador(myConn, lblInfo, Gestion.iContabilidad, "connumasi", "01")
                Case 1 'Diaria
                    numAsiento = AutoCodigoDiario(myConn, lblInfo, "jscotencasi", "asiento", CDate(txtFecha.Text))
                Case 2 'Mensual
                    numAsiento = AutoCodigoMensual(myConn, lblInfo, "jscotencasi", "asiento", CDate(txtFecha.Text))
            End Select


        End If

        EjecutarSTRSQL(myConn, lblInfo, " update jscotrenasi set asiento = '" & numAsiento & "' " _
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
        ActualizaCuentasSegunAsiento(myConn, lblInfo, numAsiento)
        DesactivarMarco0()
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub ActualizaCuentasSegunAsiento(MyConn As MySqlConnection, lblInfo As Label, numAsiento As String)

        Dim dtMov As DataTable
        Dim nTablaMov As String = "tblMov1"
        ds = DataSetRequery(ds, " select * from jscotrenasi where asiento = '" & numAsiento & "' and id_emp = '" & jytsistema.WorkID & "' order by renglon ", _
                             MyConn, nTablaMov, lblInfo)
        dtMov = ds.Tables(nTablaMov)

        If dtMov.Rows.Count > 0 Then
            For Each nRow As DataRow In dtMov.Rows
                With nRow
                    ActualizaSaldosCuentasContables(MyConn, lblInfo, .Item("codcon"), CDate(txtFecha.Text))
                End With
            Next
        End If

    End Sub

    Private Sub txtDescripcion_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescripcion.GotFocus
        MensajeEtiqueta(lblInfo, " Indique el descripción para este asiento contable ... ", TipoMensaje.iInfo)
        txtDescripcion.MaxLength = 150
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarAsiento(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        i_modo = movimiento.iEditar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If dt.Rows.Count > 0 Then
            Dim aFld() As String = {"asiento", "ejercicio", "id_emp"}
            Dim aSFld() As String = {txtAsiento.Text, jytsistema.WorkExercise, jytsistema.WorkID}
            Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
            sRespuesta = MsgBox("¿Desea eliminiar asiento contable?", MsgBoxStyle.YesNo, sModulo)
            If sRespuesta = MsgBoxResult.Yes Then
                EliminarRegistros(myConn, lblInfo, ds, nTabla, "jscotencasi", strSQL, aFld, aSFld, nPosicionEncab)
                EliminarRegistros(myConn, lblInfo, ds, nTablaMovimientos, "jscotrenasi", strSQLMov, aFld, aSFld, nPosicionRenglon)
                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, txtAsiento.Text)
                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                dt = ds.Tables(nTabla)
                If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                AsignaTXT(nPosicionEncab)
            End If
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"asiento", "descripcion", "fechaSI"}
        Dim Nombres() As String = {"Código Asiento", "Descripción", "Fecha"}
        Dim Anchos() As Long = {100, 400, 100}

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
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "ItemsRenglon", "lblItemsRenglon", e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
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
        txtDebitos.Text = FormatoNumero(0.0)
        txtCreditos.Text = FormatoNumero(0.0)
        If dtTotal.Rows.Count > 0 Then
            With dtTotal.Rows(0)
                txtDebitos.Text = FormatoNumero(.Item("debitos"))
                txtCreditos.Text = FormatoNumero(.Item("creditos"))
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

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionRenglon = Me.BindingContext(ds, nTablaMovimientos).Position

        If nPosicionRenglon >= 0 Then


            sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
            If sRespuesta = MsgBoxResult.Yes Then
                With dtMovimientos.Rows(nPosicionRenglon)
                    Dim CodigoEliminado As String
                    CodigoEliminado = .Item("codcon")
                    Dim aFld() As String = {"asiento", "renglon", "codcon", "referencia", "actual", "ejercicio", "id_emp"}
                    Dim aNFld() As String = {.Item("asiento"), .Item("renglon"), .Item("codcon"), .Item("referencia"), .Item("actual"), jytsistema.WorkExercise, jytsistema.WorkID}
                    EliminarRegistros(myConn, lblInfo, ds, nTablaMovimientos, "jscotrenasi", strSQLMov, aFld, aNFld, nPosicionRenglon)
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
        Dim Anchos() As Long = {150, 250, 1500}
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
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrenasi set renglon = 'XXXXX' where asiento = '" & txtAsiento.Text & "' and renglon = '" & RenglonDestino & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrenasi set renglon = '" & RenglonDestino & "' where asiento = '" & txtAsiento.Text & "' and renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrenasi set renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' where asiento = '" & txtAsiento.Text & "' and renglon = 'XXXXX' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'")
                    Else
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrenasi set renglon = '" & RenglonDestino & "' where asiento = '" & txtAsiento.Text & "' and renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'  ")
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
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrenasi set renglon = 'XXXXX' where asiento = '" & txtAsiento.Text & "' and renglon = '" & RenglonDestino & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrenasi set renglon = '" & RenglonDestino & "' where asiento = '" & txtAsiento.Text & "' and renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrenasi set renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' where asiento = '" & txtAsiento.Text & "' and renglon = 'XXXXX' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                    Else
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrenasi set renglon = '" & RenglonDestino & "' where asiento = '" & txtAsiento.Text & "' and renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                    End If
                    nPosicionRenglon += 1
                    AsignaMov(nPosicionRenglon, True)
                End If
            End If
        End If
    End Sub
    Private Sub txtDebitos_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDebitos.TextChanged, _
        txtCreditos.TextChanged
        txtSaldos.Text = FormatoNumero(ValorNumero(txtDebitos.Text) + ValorNumero(txtCreditos.Text))
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
        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult

        nPosicionEncab = Me.BindingContext(ds, nTabla).Position

        sRespuesta = MsgBox("¿Desea transferir este asiento a ACTUAL?", MsgBoxStyle.YesNo, sModulo)
        If sRespuesta = MsgBoxResult.Yes Then
            If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(*) from jscotrenasi where trim(codcon) = '' AND asiento = '" & AsientoActual & "' and ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ")) = 0 Then
                If ValorNumero(txtSaldos.Text) = 0 Then
                    ActualizaCuentasSegunAsiento(myConn, lblInfo, AsientoActual)
                    EjecutarSTRSQL_Scalar(myConn, lblInfo, " update jscotencasi set actual = " & Asiento.iActual & " where asiento = '" & AsientoActual & "' and ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ")
                    EjecutarSTRSQL_Scalar(myConn, lblInfo, " update jscotrenasi set actual = " & Asiento.iActual & " where asiento = '" & AsientoActual & "' and ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ")
                    ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                    dt = ds.Tables(nTabla)
                    If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                    AsignaTXT(nPosicionEncab)
                Else
                    MensajeCritico(lblInfo, "ASIENTO DESCUADRADO. VERIFIQUE POR FAVOR...")
                End If
            Else
                MensajeCritico(lblInfo, "EXISTEN MOVIMIENTOS EN ESTE ASIENTO SIN CODIGO CONTABLE. VERIFIQUE POR FAVOR...")
            End If

        End If


    End Sub

    Private Sub btnDeActual_Click(sender As System.Object, e As System.EventArgs) Handles btnDeActual.Click

        Dim AsientoActual As String = txtAsiento.Text
        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult

        nPosicionEncab = Me.BindingContext(ds, nTabla).Position

        sRespuesta = MsgBox("¿Desea transferir este asiento a DIFERIDO ?", MsgBoxStyle.YesNo, sModulo)
        If sRespuesta = MsgBoxResult.Yes Then
            EjecutarSTRSQL_Scalar(myConn, lblInfo, " update jscotencasi set actual = " & Asiento.iDiferido & " where asiento = '" & AsientoActual & "' and ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ")
            EjecutarSTRSQL_Scalar(myConn, lblInfo, " update jscotrenasi set actual = " & Asiento.iDiferido & " where asiento = '" & AsientoActual & "' and ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ")
            ActualizaCuentasSegunAsiento(myConn, lblInfo, AsientoActual)
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)
            If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
            AsignaTXT(nPosicionEncab)
        End If
    End Sub

    Private Sub btnReconstruir_Click(sender As System.Object, e As System.EventArgs) Handles btnReconstruir.Click
        If TipoAsi = Asiento.iDiferido Then
            ReconstruirAsientoActual()
            RefrescarAsientos(nPosicionEncab)
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

                    EjecutarSTRSQL(myConn, lblInfo, " delete from jscotencasi " _
                                   & " where " _
                                   & " asiento = '" & txtAsiento.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")


                    EjecutarSTRSQL(myConn, lblInfo, " delete from jscotrenasi " _
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
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra, iArray)
    End Sub

End Class