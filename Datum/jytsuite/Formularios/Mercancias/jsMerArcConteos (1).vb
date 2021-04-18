Imports MySql.Data.MySqlClient
Imports fTransport
Public Class jsMerArcConteos
    Private Const sModulo As String = "Conteos de inventarios"
    Private Const lRegion As String = "RibbonButton139"
    Private Const nTabla As String = "tblconteos"
    Private Const nTablaRenglones As String = "tblRenglones_conteos"

    Private strSQL As String = "select * from jsmerenccon where ID_EMP = '" & jytsistema.WorkID & "' and " _
        & " ejercicio = '" & jytsistema.WorkExercise & "' order  by fechacon, conmer "

    Private strSQLMov As String

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private myCom As New MySqlCommand(strSQL, myConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtRenglones As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long
    Private aTipo() As String = {"POR PROCESAR", "PROCESADO"}
    Private FechaProceso As Date = MyDate

    Private BindingSource1 As New BindingSource
    Private FindField As String
    Dim aCampos() As String = {"CODART", "NOMART"}
    Dim aNombres() As String = {"Código", "Nombre"}
    Dim aConteo() As String = {"Conteo 1", "Conteo 2", "Conteo 3"}
    Dim aTipoUnidad() As String = {"Mayor", "Detal"}
    Dim tipoUnidadSeleccionado As Integer = 0

    Dim aEditar() As String = {"cont1", "cont2", "cont3", "costou"}

    Private Sub jsMerArcConteos_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    
    Private Sub jsMerArcConteos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            ds = DataSetRequeryPlus(ds, nTabla, myConn, strSQL)
            dt = ds.Tables(nTabla)

            ft.RellenaCombo(aConteo, cmbCuenta)

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionEncab = dt.Rows.Count - 1
                Me.BindingContext(ds, nTabla).Position = nPosicionEncab
                FindField = aCampos(0)
                lblBuscar.Text = aNombres(0)
                AsignaTXT(nPosicionEncab)
            Else
                IniciarConteo(False)
            End If

            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnPrimero, btnSiguiente, _
                          btnAnterior, btnUltimo, btnImprimir, btnSalir, btnRefrescar, btnAgregarMovimiento, btnEditarMovimiento, _
                          btnEliminarMovimiento, btnBuscarMovimiento, btnPrimerMovimiento, btnAnteriorMovimiento, _
                          btnSiguienteMovimiento, btnUltimoMovimiento)
        'Encabezado
        ft.colocaToolTip(C1SuperTooltip1, btnEmision, btnAlmacen)
    End Sub

    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt
            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtConteo.Text = ft.muestraCampoTexto(.Item("conmer"))
                txtComentario.Text = ft.muestraCampoTexto(.Item("comentario"))
                txtEmision.Text = ft.muestraCampoFecha(.Item("fechacon"))
                ft.RellenaCombo(aTipo, cmbEstatus, CInt(.Item("procesado").ToString))
                txtAlmacen.Text = ft.muestraCampoTexto(.Item("almacen"))
                txtFechaProceso.Text = IIf(.Item("procesado") = 0, "", ft.muestraCampoFecha(.Item("fechapro").ToString))
                If .Item("procesado") = 0 Then
                    FechaProceso = MyDate
                Else
                    FechaProceso = ft.muestraCampoFecha(.Item("fechapro").ToString)
                End If

                ft.RellenaCombo(aTipoUnidad, cmbTipoUnidad, .Item("tipounidad"))

                'Renglones
                AsignarMovimientos(.Item("conmer"))

                If dtRenglones.Rows.Count > 0 Then
                    FindField = dtRenglones.Columns(aCampos(0)).ColumnName
                    lblBuscar.Text = aNombres(0)
                    txtBuscar.Text = ""
                    BindingSource1.Filter = " codart " & " like '%%'"
                End If

            End With
        End With

    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroDeConteo As String)

        strSQLMov = "select a.conmer, a.codart, a.nomart, a.unidad, a.existencia, a.conteo, a.cont1, a.cont2, a.cont3, " _
            & " a.costou, a.costo_tot " _
            & " from jsmerconmer a " _
            & " where " _
            & " CONMER = '" & NumeroDeConteo & "' and " _
            & " aceptado < '2' and " _
            & " ID_EMP = '" & jytsistema.WorkID & "' and " _
            & " EJERCICIO = '" & jytsistema.WorkExercise & "' " _
            & " order by CODART "

        dtRenglones = ft.AbrirDataTable(ds, nTablaRenglones, myConn, strSQLMov)

        txtItems.Text = ft.FormatoEntero(dtRenglones.Rows.Count)

        Dim aCampos() As String = {"codart.Item.70.I.", _
                                   "nomart.Descripción.300.I.", _
                                   "unidad.UND.45.C.", _
                                   "existencia.Existencia Actual.100.D.Cantidad", _
                                   "conteo.Cuenta Total.100.D.Cantidad", _
                                   "cont1.Conteo Uno(1).100.D.Cantidad", _
                                   "cont2.Conteo Dos(2).100.D.Cantidad", _
                                   "cont3.Conteo Tres(3).100.D.Cantidad", _
                                   "costou.Costo Unitario.130.D.Numero", _
                                   "costo_tot.Costo Total.130.D.Numero", _
                                   "sada..50.I."}

        ft.IniciarTablaPlus(dg, dtRenglones, aCampos, , True)
        ft.EditarColumnasEnDataGridView(dg, aEditar)

        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, jytsistema.sUsuario, nPosicionRenglon, True)
        End If

    End Sub

    Private Sub IniciarConteo(ByVal Inicio As Boolean)

        If Inicio Then
            txtConteo.Text = "CIT" & ft.RellenaConCaracter(ft.NumeroAleatorio(1000000), 7, "0", Transportables.lado.izquierdo)
        Else
            txtConteo.Text = ""
        End If

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtComentario, txtFechaProceso)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Fecha, txtEmision)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Entero, txtItems)

        ft.RellenaCombo(aTipo, cmbEstatus)
        ft.RellenaCombo(aTipoUnidad, cmbTipoUnidad, tipoUnidadSeleccionado)

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        dg.Columns.Clear()
        strSQLMov = "select * from jsmerconmer " _
                        & " where CONMER = '" & txtConteo.Text & "' and " _
                        & " aceptado < '2' and " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' and " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' " _
                        & " order by CODART "

    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, MenuBarraRenglon)
        ft.habilitarObjetos(True, True, txtComentario, btnEmision, btnAlmacen, cmbCuenta)
        If i_modo = movimiento.iAgregar Then cmbTipoUnidad.Enabled = True

        If dg.RowCount > 0 Then ft.EditarColumnasEnDataGridView(dg, aEditar)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()
        Dim c As Control
        grpAceptarSalir.Visible = False

        For Each c In grpEncab.Controls
            ft.habilitarObjetos(False, True, c)
        Next

        dg.ReadOnly = True
        ft.habilitarObjetos(False, True, grpEncab)
        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then AgregaYCancela()

        If dt.Rows.Count = 0 Then
            IniciarConteo(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            AsignaTXT(nPosicionEncab)
        End If

        DesactivarMarco0()
    End Sub
    Private Sub AgregaYCancela()
        ft.Ejecutar_strSQL(myconn, " delete from jsmerconmer where conmer = '" & txtConteo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado() As Boolean

        If FechaUltimoBloqueo(myConn, "jsmerenccon") >= Convert.ToDateTime(txtEmision.Text) Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        Return True

    End Function
    Private Sub GuardarTXT()

        Dim CodigoConteo As String = txtConteo.Text
        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count

            CodigoConteo = Contador(myConn, lblInfo, Gestion.iMercancías, "INVNUMCON", "03")
            ft.Ejecutar_strSQL(myconn, " update jsmerconmer set conmer = '" & CodigoConteo & "' where conmer = '" & txtConteo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsmerconmerdinamica set conmer = '" & CodigoConteo & "' where conmer = '" & txtConteo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        InsertEditMERCASEncabezadoConteo(myConn, lblInfo, Inserta, CodigoConteo, CDate(txtEmision.Text), txtAlmacen.Text, txtComentario.Text, cmbEstatus.SelectedIndex, _
                                           FechaProceso, cmbTipoUnidad.SelectedIndex)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" conmer = '" & CodigoConteo & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab

        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

        If dtRenglones.Rows.Count > 0 Then
            FindField = dtRenglones.Columns(aCampos(0)).ColumnName
            lblBuscar.Text = aNombres(0)
            BindingSource1.Filter = " codart " & " like '%%'"
            txtBuscar.Text = ""
        End If

    End Sub
    Private Sub txtComentario_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique un comentario o descripción del proceso ... ", Transportables.TipoMensaje.iInfo)
        txtComentario.MaxLength = 100
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarConteo(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        With dt.Rows(nPosicionEncab)
            Dim aCamposAdicionales() As String = {"conmer|'" & txtConteo.Text & "'"}
            If DocumentoBloqueado(myConn, "jsmerenccon", aCamposAdicionales) Then
                ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
            Else
                If cmbEstatus.Text = "POR PROCESAR" Then
                    i_modo = movimiento.iEditar
                    nPosicionEncab = Me.BindingContext(ds, nTabla).Position
                    ActivarMarco0()
                Else
                    ft.mensajeCritico("ESTE CONTEO YA FUE PROCESADO. VERIFIQUE POR FAVOR...")
                End If
            End If
        End With
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        With dt.Rows(nPosicionEncab)
            Dim aCamposAdicionales() As String = {"conmer|'" & txtConteo.Text & "'"}
            If DocumentoBloqueado(myConn, "jsmerenccon", aCamposAdicionales) Then
                ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
            Else
                If dt.Rows(nPosicionEncab).Item("procesado").ToString = "0" Then

                    nPosicionEncab = Me.BindingContext(ds, nTabla).Position

                    If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("conmer"))

                        ft.Ejecutar_strSQL(myConn, " delete from jsmerenccon where " _
                            & " conmer = '" & txtConteo.Text & "' AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'")

                        ft.Ejecutar_strSQL(myConn, " delete from jsmerconmer where conmer = '" & txtConteo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                        dt = ds.Tables(nTabla)
                        If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                        If nPosicionEncab >= 0 Then
                            AsignaTXT(nPosicionEncab)
                        Else
                            IniciarConteo(False)
                        End If

                    End If
                Else
                    ft.mensajeCritico("ESTE CONTEO YA FUE PROCESADO. VERIFIQUE POR FAVOR...")
                End If
            End If
        End With

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"conmer", "comentario", "fechacon"}
        Dim Nombres() As String = {"Número de Conteo", "Comentario", "Emisión"}
        Dim Anchos() As Integer = {150, 650, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Buscar conteos")
        AsignaTXT(f.Apuntador)
        nPosicionEncab = f.Apuntador
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        f = Nothing
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Me.BindingContext(ds, nTabla).Position = 0
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Me.BindingContext(ds, nTabla).Position -= 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Me.BindingContext(ds, nTabla).Position += 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        nPosicionRenglon = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub
    Private Sub dg_RowValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.RowValidated
        Dim Conteo As String = ""

        Select Case dg.CurrentCell.ColumnIndex
            Case 5, 6, 7, 8
                If dg.CurrentCell.ColumnIndex = 5 Then Conteo = "cont1"
                If dg.CurrentCell.ColumnIndex = 6 Then Conteo = "cont2"
                If dg.CurrentCell.ColumnIndex = 7 Then Conteo = "cont3"
                If dg.CurrentCell.ColumnIndex = 8 Then Conteo = "costou"
                If dg.CurrentCell.ColumnIndex = 8 Then
                    ft.Ejecutar_strSQL(myconn, " update jsmerconmer set costou = " & Conteo & " , costo_tot = " & Conteo & "*conteo " _
                                                          & " where conmer = '" & txtConteo.Text & "' and codart = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' and id_emp = '" & jytsistema.WorkID & "' ")
                Else
                    ft.Ejecutar_strSQL(myconn, " update jsmerconmer set conteo = " & Conteo & ", costo_tot = " & Conteo & "*costou " _
                                                           & " where conmer = '" & txtConteo.Text & "' and codart = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' and id_emp = '" & jytsistema.WorkID & "' ")
                End If
        End Select

    End Sub

    Private Sub dg_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
            Handles dg.CellValidating

        Dim headerText As String = _
            dg.Columns(e.ColumnIndex).HeaderText

        If Not (headerText.Equals("cont1") Or _
                 headerText.Equals("cont2") Or _
                  headerText.Equals("cont3") Or _
                  headerText.Equals("costou")) Then Return

        If (String.IsNullOrEmpty(e.FormattedValue.ToString())) Then
            ft.mensajeAdvertencia("Debe indicar dígito(s) válido...")
            e.Cancel = True
        End If

        If Not ft.isNumeric(e.FormattedValue.ToString()) Then
            ft.mensajeAdvertencia("Debe indicar un número válido...")
            e.Cancel = True
        End If

    End Sub

    Private Sub dg_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dg.CellEndEdit
        dg.Rows(e.RowIndex).ErrorText = String.Empty
    End Sub
    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click

        If dtRenglones.Rows.Count > 0 Then
            FindField = dtRenglones.Columns(aCampos(0)).ColumnName
            lblBuscar.Text = aNombres(0)

            If dtRenglones.Columns(FindField).DataType Is GetType(String) Then _
            BindingSource1.Filter = FindField & " like '%%'"
        End If

        Dim f As New jsMerProCargarMercanciasConteo
        f.Cargar(myConn, txtConteo.Text, txtAlmacen.Text, cmbTipoUnidad.SelectedIndex)
        AsignarMovimientos(txtConteo.Text)
        f = Nothing
    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click

        EliminarMovimiento()

    End Sub
    Private Sub EliminarMovimiento()

        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                With dtRenglones.Rows(nPosicionRenglon)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("conmer") & " " & .Item("codart"))
                    Dim aCamposDel() As String = {"conmer", "codart", "id_emp"}
                    Dim aStringsDel() As String = {.Item("conmer"), .Item("codart"), jytsistema.WorkID}

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsmerconmer", strSQLMov, aCamposDel, aStringsDel, _
                                                  Me.BindingContext(ds, nTablaRenglones).Position, True)

                    If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1
                    MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, jytsistema.sUsuario, nPosicionRenglon, True)

                End With

            End If
        End If

    End Sub
    Private Sub btnBuscarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarMovimiento.Click

        Dim f As New frmBuscar
        Dim Campos() As String = {"codart", "nomart"}
        Dim Nombres() As String = {"Item", "Descripción"}
        Dim Anchos() As Integer = {140, 350}
        f.Text = "Renglones de Conteo de inventario No. " & txtConteo.Text
        f.Buscar(dtRenglones, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaRenglones).Position, " " & Me.Tag & "...")
        nPosicionRenglon = f.Apuntador
        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, jytsistema.sUsuario, nPosicionRenglon, True)

        f = Nothing

    End Sub

    Private Sub btnPrimerMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimerMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position = 0
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, jytsistema.sUsuario, nPosicionRenglon, False)
    End Sub

    Private Sub btnAnteriorMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnteriorMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position -= 1
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, jytsistema.sUsuario, nPosicionRenglon, False)
    End Sub

    Private Sub btnSiguienteMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguienteMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position += 1
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, jytsistema.sUsuario, nPosicionRenglon, False)
    End Sub

    Private Sub btnUltimoMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimoMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position = ds.Tables(nTablaRenglones).Rows.Count - 1
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, jytsistema.sUsuario, nPosicionRenglon, False)
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cConteos, "Conteo de Mercancia", txtConteo.Text)
        f = Nothing
    End Sub

    Private Sub btnEmision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmision.Click
        txtEmision.Text = SeleccionaFecha(CDate(txtEmision.Text), Me, sender)
    End Sub

    Private Sub txtAlmacen_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlmacen.TextChanged
        Dim aFld() As String = {"codalm", "id_emp"}
        Dim aStr() As String = {txtAlmacen.Text, jytsistema.WorkID}
        lblAlmacen.Text = qFoundAndSign(myConn, lblInfo, "jsmercatalm", aFld, aStr, "desalm")
    End Sub

    Private Sub btnAlmacen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacen.Click
        Dim f As New jsControlArcAlmacenes
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        txtAlmacen.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub dg_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellValidated
        Dim Conteo As String = ""
        Select Case dg.CurrentCell.ColumnIndex
            Case 5, 6, 7, 8
                If dg.CurrentCell.ColumnIndex = 5 Then Conteo = "cont1"
                If dg.CurrentCell.ColumnIndex = 6 Then Conteo = "cont2"
                If dg.CurrentCell.ColumnIndex = 7 Then Conteo = "cont3"
                If dg.CurrentCell.ColumnIndex = 8 Then Conteo = "costou"
                ft.Ejecutar_strSQL(myConn, " update jsmerconmer set " & Conteo & " = " & CDbl(dg.CurrentCell.Value) & " " _
                                        & " where conmer = '" & txtConteo.Text & "' and codart = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' and id_emp = '" & jytsistema.WorkID & "' ")

                If dg.CurrentCell.ColumnIndex = 8 Then
                    ft.Ejecutar_strSQL(myConn, " update jsmerconmer set costou = " & Conteo & " , costo_tot = " & Conteo & "*conteo " _
                                                          & " where conmer = '" & txtConteo.Text & "' and codart = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' and id_emp = '" & jytsistema.WorkID & "' ")
                Else
                    ft.Ejecutar_strSQL(myConn, " update jsmerconmer set conteo = " & Conteo & ", costo_tot = " & Conteo & "*costou " _
                                                           & " where conmer = '" & txtConteo.Text & "' and codart = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' and id_emp = '" & jytsistema.WorkID & "' ")
                End If

                nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
                MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, jytsistema.sUsuario, nPosicionRenglon, True)

        End Select
    End Sub
    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.ColumnHeaderMouseClick
        If e.ColumnIndex <= aCampos.Length Then
            FindField = dtRenglones.Columns(aCampos(e.ColumnIndex)).ColumnName
            lblBuscar.Text = aNombres(e.ColumnIndex)
        End If
    End Sub



    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        BindingSource1.DataSource = dtRenglones
        If dtRenglones.Columns(FindField).DataType Is GetType(String) Then _
            BindingSource1.Filter = FindField & " like '%" & txtBuscar.Text & "%'"
        dg.DataSource = BindingSource1
    End Sub

    Private Sub btnRefrescar_Click(sender As System.Object, e As System.EventArgs) Handles btnRefrescar.Click



        If nPosicionEncab > -1 Then nPosicionEncab = Me.BindingContext(ds, nTabla).Position

        ds = DataSetRequeryPlus(ds, nTabla, myConn, strSQL)
        dt = ds.Tables(nTabla)

        If dt.Rows.Count > 0 Then

            Me.BindingContext(ds, nTabla).Position = nPosicionEncab
            AsignaTXT(nPosicionEncab)

        End If

    End Sub


    Private Sub btnAgregarR_Click(sender As Object, e As EventArgs) Handles btnAgregarR.Click
        Dim f As New jsMerProCargarMercanciasConteoDinamico
        f.Cargar(myConn, txtConteo.Text, txtAlmacen.Text, cmbTipoUnidad.SelectedIndex, cmbCuenta.SelectedIndex)
        AsignarMovimientos(txtConteo.Text)
        f.Dispose()
        f = Nothing
    End Sub

    Private Sub cmbTipoUnidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTipoUnidad.SelectedIndexChanged
        If cmbTipoUnidad.Items.Count = aTipoUnidad.Count Then _
            tipoUnidadSeleccionado = cmbTipoUnidad.SelectedIndex
    End Sub
End Class