Imports MySql.Data.MySqlClient
Public Class jsContabArcAsientosPlantilla
    Private sModulo As String = "Plantillas de Asientos Contables "
    Private Const lRegion As String = "RibbonButton7"
    Private Const nTabla As String = "AsientoP"
    Private Const nTablaMovimientos As String = "movimientos_asientoP"

    Private strSQL As String = ""
    Private strSQLMov As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtMovimientos As New DataTable

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        Me.Dock = DockStyle.Fill
        myConn = MyCon
        Me.Tag = sModulo



        strSQL = " select * from jscotencdef " _
            & " Where " _
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

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
        AsignarTooltips()

        Me.Show()


    End Sub
    Private Sub jsContabArcAsientosPlantilla_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
    End Sub

    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva plantilla asiento contable")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> plantilla asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> plantilla asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> plantilla asiento contable deseado")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir al <B>primera</B> plantilla asiento contable")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a <B>siguiente</B> plantilla asiento contable")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a plantilla asiento contable <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>última plantilla asiento contable</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> plantilla de asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> movimiento plantilla asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> movimiento plantilla de asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> movimiento de asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un movimiento plantilla en asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> movimiento plantilla de asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> movimiento plantilla de asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al movimiento plantilla <B>siguiente </B> de asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> movimiento plantilla de asiento contable actual")
        C1SuperTooltip1.SetToolTip(btnSubir, " <B>Subir</B> renglón en el orden de precedencia")
        C1SuperTooltip1.SetToolTip(btnBajar, "<B>Bajar</B> renglón en el orden de precedencia")

    End Sub
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)

        End If


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
                        AbrirMovimientos(.Item("asiento").ToString)
                    End With
                End If
            End With
        Else
            nRow = 0
            txtAsiento.Text = ""
            txtDescripcion.Text = ""
            MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", 0, 0)
        End If
        MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, dt.Rows.Count)

    End Sub
    Private Sub AbrirMovimientos(ByVal NumeroAsiento As String, Optional nPosicionRenglon As Long = 0)

        strSQLMov = " select a.*, b.descripcion from jscotrendef a" _
            & " left join jscotcatcon b on (a.codcon = b.codcon and a.id_emp = b.id_emp) " _
            & " where " _
            & " a.asiento = '" & NumeroAsiento & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        dtMovimientos = ds.Tables(nTablaMovimientos)

        Dim aCampos() As String = {"codcon", "descripcion", "referencia", "concepto", "regla", "signo", ""}
        Dim aNombres() As String = {"Código cuenta", "Descripción", "Nº Referencia", "Concepto", " Regla Nº ", "Signo", ""}
        Dim aAnchos() As Long = {150, 200, 150, 400, 60, 40, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha}
        Dim aFormatos() As String = {"", "", "", "", "", "", ""}
        IniciarTabla(dg, dtMovimientos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtMovimientos.Rows.Count > 0 Then
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub
    Private Sub IniciarAsiento(ByVal Inicio As Boolean)
        If Inicio Then
            txtAsiento.Text = "ASITMP" + Format(NumeroAleatorio(10000000), "00000000")
        Else
            txtAsiento.Text = ""
        End If
        txtDescripcion.Text = ""

        'Movimientos
        dg.Columns.Clear()
        AbrirMovimientos(txtAsiento.Text)
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", 0, 0)


    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True
        grpEncab.Enabled = True

        HabilitarObjetos(True, True, txtDescripcion)

        MenuBarra.Enabled = False
        MenuBarraRenglon.Enabled = True

        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        HabilitarObjetos(False, True, txtAsiento, txtDescripcion)

        grpEncab.Enabled = False
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
        If txtDescripcion.Text = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar una descripción Válida ...")
            txtDescripcion.Focus()
            Exit Function
        End If

        Validado = True

    End Function
    Private Sub GuardarTXT()

        Dim Inserta As Boolean = False
        Dim numAsiento As String = txtAsiento.Text
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count
            numAsiento = AutoCodigo(5, ds, nTabla, "asiento")

        End If

        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set asiento = '" & numAsiento & "' " _
                            + " where " _
                            + " asiento = '" & txtAsiento.Text & "' and " _
                            + " id_emp = '" & jytsistema.WorkID & "' ")

        InsertEditCONTABEncabezadoAsientoPlantilla(myConn, lblInfo, Inserta, numAsiento, txtDescripcion.Text)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab

        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

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

        If LoginUser(myConn, lblInfo) = "jytsuite" Then
            If dt.Rows.Count > 0 Then
                Dim aFld() As String = {"asiento", "id_emp"}
                Dim aSFld() As String = {txtAsiento.Text, jytsistema.WorkID}
                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                sRespuesta = MsgBox("¿Desea eliminiar asiento contable?", MsgBoxStyle.YesNo, sModulo)
                If sRespuesta = MsgBoxResult.Yes Then
                    EliminarRegistros(myConn, lblInfo, ds, nTabla, "jscotencdef", strSQL, aFld, aSFld, nPosicionEncab)
                    EliminarRegistros(myConn, lblInfo, ds, nTablaMovimientos, "jscotrendef", strSQLMov, aFld, aSFld, nPosicionRenglon)
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
        Dim Campos() As String = {"asiento", "descripcion", "fecha"}
        Dim Nombres() As String = {"Código Asiento", "Descripción", "Fecha"}
        Dim Anchos() As Long = {100, 2500, 100}

        f.Apuntador = nPosicionEncab
        f.Buscar(dt, Campos, Nombres, Anchos, nPosicionEncab, "Plantilla de asientos contables...")
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

    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dg.CellFormatting
        Select Case e.ColumnIndex
            Case 5
                Dim aSigno() As String = {"+", "-"}
                e.Value = aSigno(e.Value)
        End Select
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        nPosicionRenglon = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "ItemsRenglon", "lblItemsRenglon", e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
    End Sub

    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click

        Dim f As New jsContabArcAsientosPlantillaMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaMovimientos).Position
        f.Agregar(myConn, ds, dtMovimientos, txtAsiento.Text)
        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        If f.Apuntador > 0 Then AsignaMov(f.Apuntador, True)
        f = Nothing

    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        Dim f As New jsContabArcAsientosPlantillaMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaMovimientos).Position
        f.Editar(myConn, ds, dtMovimientos, txtAsiento.Text)
        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        AsignaMov(f.Apuntador, True)
        f = Nothing

    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click

        EliminarMovimiento()

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
                    Dim aFld() As String = {"asiento", "renglon", "codcon", "referencia", "id_emp"}
                    Dim aNFld() As String = {.Item("asiento"), .Item("renglon"), .Item("codcon"), .Item("referencia"), jytsistema.WorkID}
                    EliminarRegistros(myConn, lblInfo, ds, nTablaMovimientos, "jscotrendef", strSQLMov, aFld, aNFld, nPosicionRenglon)
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
        f.Text = "Movimientos de cuentas contables"
        f.Buscar(dtMovimientos, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaMovimientos).Position, "Movimientos de plantillas de asientos contables...")
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
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteContabilidad.cPLantillaAsiento, "Plantilla asiento contable", txtAsiento.Text)
        f = Nothing
    End Sub
    Private Sub btnSubir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubir.Click

        Dim RenglonDestino As String = ""
        If dtMovimientos.Rows.Count > 0 Then
            nPosicionRenglon = Me.BindingContext(ds, nTablaMovimientos).Position
            If dtMovimientos.Rows(0).Item("renglon") <> "" Then
                If dtMovimientos.Rows(nPosicionRenglon).Item("renglon") > dtMovimientos.Rows(0).Item("renglon") Then
                    'numRenglon = CType(dtMovimientos.Rows(nPosicionRenglon).Item("renglon").ToString, Integer)
                    'RenglonDestino = Format(numRenglon - 1, "00000")
                    RenglonDestino = dtMovimientos.Rows(nPosicionRenglon - 1).Item("renglon").ToString
                    Dim afld() As String = {"asiento", "renglon", "id_emp"}
                    Dim asFld() As String = {txtAsiento.Text, RenglonDestino, jytsistema.WorkID}
                    If qFound(myConn, lblInfo, "jscotrendef", afld, asFld) Then
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set renglon = 'XXXXX' where asiento = '" & txtAsiento.Text & "' and renglon = '" & RenglonDestino & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set renglon = '" & RenglonDestino & "' where asiento = '" & txtAsiento.Text & "' and renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' and id_emp = '" & jytsistema.WorkID & "'")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' where asiento = '" & txtAsiento.Text & "' and renglon = 'XXXXX' and id_emp = '" & jytsistema.WorkID & "'")
                    Else
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set renglon = '" & RenglonDestino & "' where asiento = '" & txtAsiento.Text & "' and renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                    End If
                    nPosicionRenglon -= 1
                    AbrirMovimientos(txtAsiento.Text, nPosicionRenglon)
                End If
            End If
        End If
    End Sub

    Private Sub btnBajar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBajar.Click

        Dim RenglonDestino As String = ""
        If dtMovimientos.Rows.Count > 0 Then
            nPosicionRenglon = Me.BindingContext(ds, nTablaMovimientos).Position
            If dtMovimientos.Rows(dtMovimientos.Rows.Count - 1).Item("renglon") <> "" Then
                If dtMovimientos.Rows(nPosicionRenglon).Item("renglon") < dtMovimientos.Rows(dtMovimientos.Rows.Count - 1).Item("renglon") Then
                    RenglonDestino = dtMovimientos.Rows(nPosicionRenglon + 1).Item("renglon").ToString
                    Dim afld() As String = {"asiento", "renglon", "id_emp"}
                    Dim asFld() As String = {txtAsiento.Text, RenglonDestino, jytsistema.WorkID}
                    If qFound(myConn, lblInfo, "jscotrendef", afld, asFld) Then
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set renglon = 'XXXXX' where asiento = '" & txtAsiento.Text & "' and renglon = '" & RenglonDestino & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set renglon = '" & RenglonDestino & "' where asiento = '" & txtAsiento.Text & "' and renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' where asiento = '" & txtAsiento.Text & "' and renglon = 'XXXXX' and id_emp = '" & jytsistema.WorkID & "'  ")
                    Else
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set renglon = '" & RenglonDestino & "' where asiento = '" & txtAsiento.Text & "' and renglon = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                    End If
                    nPosicionRenglon += 1
                    AbrirMovimientos(txtAsiento.Text, nPosicionRenglon)
                End If
            End If
        End If
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

End Class