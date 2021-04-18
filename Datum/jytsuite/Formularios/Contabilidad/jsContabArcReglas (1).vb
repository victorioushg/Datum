Imports MySql.Data.MySqlClient

Public Class jsContabArcReglas
    Private Const sModulo As String = "Reglas de contabilización"
    Private Const lRegion As String = "RibbonButton8"

    Private Const nTabla As String = "reglas"
    Private strSQL As String = "select * from jscotcatreg where id_emp = '" & jytsistema.WorkID & "' order by plantilla "

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable

    Private i_modo As Integer
    Private i As Integer
    Private Posicion As Long

    Private Sub jsContabArcReglas_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ds = Nothing
        dt = Nothing
        myConn.Close()
        myConn = Nothing
    End Sub

    Private Sub jsContabArcReglas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo
        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)

            AsignarTooltips()
            IniciarGrilla()
            If dt.Rows.Count > 0 Then Posicion = 0
            AsignaTXT(Posicion, True)
            MensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", TipoMensaje.iAyuda)

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
        C1SuperTooltip1.SetToolTip(btnSubir, "<B>Subir</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnBajar, "<B>Bajar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir al <B>primer</B> registro")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir al <B>siguiente</B> registro")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir al registro <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir al <B>último registro</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B>")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnDuplicarRegla, "<B>Duplica</B> la regla señalada")
        C1SuperTooltip1.SetToolTip(btnProbarRegla, "<B>Probar</B> regla de contabilización señalada")

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        If c >= 0 AndAlso ds.Tables(nTabla).Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = c
            Posicion = c
            With dt
                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)
            End With
            dg.CurrentCell = dg(0, c)
        End If
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
    End Sub

    Private Sub IniciarGrilla()

        Dim aCampos() As String = {"plantilla", "referencia", ""}
        Dim aNombres() As String = {"Código plantilla", "Descripción plantilla", ""}
        Dim aAnchos() As Long = {80, 500, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
            AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {"", "", ""}

        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim f As New jsContabArcReglasMovimientos
        f.Agregar(myConn, ds, dt)
        If f.Apuntador >= 0 Then AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub
    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim f As New jsContabArcReglasMovimientos
        f.Apuntador = Me.BindingContext(ds, nTabla).Position
        f.Editar(myConn, ds, dt)
        AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Dim sRespuesta As Integer
        Posicion = Me.BindingContext(ds, nTabla).Position
        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", vbYesNo, "Eliminar registro en " & sModulo & " ...")
        If sRespuesta = vbYes Then
            Dim aCampos() As String = {"plantilla", "id_emp"}
            Dim aString() As String = {dt.Rows(Posicion).Item("plantilla"), dt.Rows(Posicion).Item("id_emp")}
            AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jscotcatreg", strSQL, aCampos, aString, Posicion), True)
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"plantilla", "referencia"}
        Dim Nombres() As String = {"Código", "Referencia"}
        Dim Anchos() As Long = {100, 300}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Reglas de integración contable...")
        AsignaTXT(f.Apuntador, False)
        f = Nothing
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        '
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Me.BindingContext(ds, nTabla).Position = 0
        AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Me.BindingContext(ds, nTabla).Position -= 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Me.BindingContext(ds, nTabla).Position += 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsContabRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteContabilidad.cReglaContabilizacion, _
                 "Regla de Contabilización", dt.Rows(Me.BindingContext(ds, nTabla).Position).Item("plantilla"))
        f = Nothing
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click

        Me.Close()
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
            dg.RowHeaderMouseClick, dg.CellMouseClick

        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        Posicion = e.RowIndex
        AsignaTXT(e.RowIndex, False)
    End Sub

    Private Sub btnDuplicarRegla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDuplicarRegla.Click
        Dim sRespuesta As Integer
        Posicion = Me.BindingContext(ds, nTabla).Position
        sRespuesta = MsgBox(" ¿ Esta Seguro que desea DUPLICAR esta regla de contabilización ?", vbYesNo, "Duplicar registro en " & sModulo & " ...")
        If sRespuesta = vbYes Then
            If Posicion >= 0 Then
                With dt.Rows(Posicion)
                    Dim nRegla As String = AutoCodigo(5, ds, nTabla, "plantilla")
                    InsertEditCONTABRegla(myConn, lblInfo, True, nRegla, .Item("referencia"), .Item("comen"), .Item("conjunto"), _
                        .Item("condicion"), .Item("formula"), .Item("agrupadopor"), .Item("codcon"))
                    ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                    dt = ds.Tables(nTabla)
                    Posicion = dt.Rows.Count - 1
                End With

            End If
        End If
        AsignaTXT(Posicion, False)

    End Sub

    Private Sub btnProbarRegla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProbarRegla.Click
        If Posicion >= 0 Then
            Dim nRegla As String = dt.Rows(Posicion).Item("plantilla")
            Dim strFecha As String = "'" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "'"
            Dim strEmpresa As String = "'" & jytsistema.WorkID & "'"
            Dim str As String = Replace(Replace(ConsultaAPartirDeRegla(myConn, ds, nRegla, lblInfo), "@Fecha", strFecha), "@Empresa", strEmpresa)
            EjecutarSTRSQL_Scalar(myConn, lblInfo, str)
            MensajeInformativo(lblInfo, "Prueba Terminada...")
        End If
    End Sub

    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(ds, nTabla).Position += 1
                Posicion = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(Posicion, False)
            Case Keys.Up
                Me.BindingContext(ds, nTabla).Position -= 1
                Posicion = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(Posicion, False)
        End Select
    End Sub

    Private Sub btnSubir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubir.Click
        Dim RenglonDestino As String = ""
        If dt.Rows.Count > 0 Then
            Posicion = Me.BindingContext(ds, nTabla).Position
            If dt.Rows(0).Item("plantilla") <> "" Then
                If dt.Rows(Posicion).Item("plantilla") > dt.Rows(0).Item("plantilla") Then
                    RenglonDestino = dt.Rows(Posicion - 1).Item("plantilla").ToString
                    Dim afld() As String = {"plantilla", "id_emp"}
                    Dim asFld() As String = {RenglonDestino, jytsistema.WorkID}
                    If qFound(myConn, lblInfo, "jscotcatreg", afld, asFld) Then
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotcatreg set plantilla = 'XXXXX' where plantilla = '" & RenglonDestino & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set regla = 'XXXXX' where regla = '" & RenglonDestino & "' and id_emp = '" & jytsistema.WorkID & "'  ")

                        EjecutarSTRSQL(myConn, lblInfo, " update jscotcatreg set plantilla = '" & RenglonDestino & "' where plantilla = '" & dt.Rows(Posicion).Item("plantilla") & "' and id_emp = '" & jytsistema.WorkID & "'")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set regla = '" & RenglonDestino & "' where regla = '" & dt.Rows(Posicion).Item("plantilla") & "' and id_emp = '" & jytsistema.WorkID & "'")

                        EjecutarSTRSQL(myConn, lblInfo, " update jscotcatreg set plantilla = '" & dt.Rows(Posicion).Item("plantilla") & "' where plantilla = 'XXXXX' and id_emp = '" & jytsistema.WorkID & "'")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set regla = '" & dt.Rows(Posicion).Item("plantilla") & "' where regla = 'XXXXX' and id_emp = '" & jytsistema.WorkID & "'")
                    Else
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotcatreg set plantilla = '" & RenglonDestino & "' where renglon = '" & dt.Rows(Posicion).Item("plantilla") & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set regla = '" & RenglonDestino & "' where regla = '" & dt.Rows(Posicion).Item("plantilla") & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                    End If
                    Posicion -= 1
                    IniciarGrilla()
                    AsignaTXT(Posicion, True)
                End If
            End If
        End If
    End Sub

    Private Sub btnBajar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBajar.Click

        Dim RenglonDestino As String = ""
        If dt.Rows.Count > 0 Then
            Posicion = Me.BindingContext(ds, nTabla).Position
            If dt.Rows(dt.Rows.Count - 1).Item("plantilla") <> "" Then
                If dt.Rows(Posicion).Item("plantilla") < dt.Rows(dt.Rows.Count - 1).Item("plantilla") Then
                    RenglonDestino = dt.Rows(Posicion + 1).Item("plantilla").ToString
                    Dim afld() As String = {"plantilla", "id_emp"}
                    Dim asFld() As String = {RenglonDestino, jytsistema.WorkID}
                    If qFound(myConn, lblInfo, "jscotcatreg", afld, asFld) Then
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotcatreg set plantilla = 'XXXXX' where plantilla = '" & RenglonDestino & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set regla = 'XXXXX' where regla = '" & RenglonDestino & "' and id_emp = '" & jytsistema.WorkID & "'  ")

                        EjecutarSTRSQL(myConn, lblInfo, " update jscotcatreg set plantilla = '" & RenglonDestino & "' where plantilla = '" & dt.Rows(Posicion).Item("plantilla") & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set regla = '" & RenglonDestino & "' where regla = '" & dt.Rows(Posicion).Item("plantilla") & "' and id_emp = '" & jytsistema.WorkID & "'  ")

                        EjecutarSTRSQL(myConn, lblInfo, " update jscotcatreg set plantilla = '" & dt.Rows(Posicion).Item("plantilla") & "' where plantilla = 'XXXXX' and id_emp = '" & jytsistema.WorkID & "'  ")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set regla = '" & dt.Rows(Posicion).Item("plantilla") & "' where regla = 'XXXXX' and id_emp = '" & jytsistema.WorkID & "'  ")

                    Else
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotcatreg set plantilla = '" & RenglonDestino & "' where plantilla = '" & dt.Rows(Posicion).Item("plantilla") & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrendef set regla = '" & RenglonDestino & "' where regla = '" & dt.Rows(Posicion).Item("plantilla") & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                    End If
                    Posicion += 1
                    IniciarGrilla()
                    AsignaTXT(Posicion, True)
                End If
            End If
        End If
    End Sub
End Class