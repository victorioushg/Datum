Imports MySql.Data.MySqlClient
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

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long
    Private aTipo() As String = {"POR PROCESAR", "PROCESADO"}
    Private FechaProceso As Date = MyDate

    Private BindingSource1 As New BindingSource
    Private FindField As String
    Dim aCampos() As String = {"CODART", "NOMART"}
    Dim aNombres() As String = {"Código", "Nombre"}
    Private Sub jsMerArcConteos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)



            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionEncab = dt.Rows.Count - 1
                Me.BindingContext(ds, nTabla).Position = nPosicionEncab
                AsignaTXT(nPosicionEncab)
                FindField = aCampos(0)
                lblBuscar.Text = aNombres(0)
            Else
                IniciarConteo(False)
            End If
            ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva tranaferencia ó consumo")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> transferencia ó consumo deseada")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primera</B> transferencia ó consumo")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a transferencia ó consumo <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a transrencia ó consumo <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>última  transferencia ó consumo</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnRefrescar, "<B>Actualizar</B> vista de esta ventana")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en la transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón de la transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón de la transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en la transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón de la transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón de la transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> de la transferencia ó consumo actual")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la transferencia ó consumo actual")

        'Encabezado
        C1SuperTooltip1.SetToolTip(btnEmision, "Seleccione la <B>fecha</B> cuando se crea este conteo...")
        C1SuperTooltip1.SetToolTip(btnAlmacen, "Seleccione el <B>Almacén</B> donde se hace el conteo. <B>Deje en blanco</B> para todos los almacenes")


    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)

        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)

        If c >= 0 Then
            Me.BindingContext(ds, nTablaRenglones).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", c, dtRenglones.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If


        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtRenglones, MenuBarraRenglon)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt
            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtConteo.Text = .Item("conmer")
                txtComentario.Text = IIf(IsDBNull(.Item("comentario")), "", .Item("comentario"))
                txtEmision.Text = FormatoFecha(CDate(.Item("fechacon").ToString))
                RellenaCombo(aTipo, cmbEstatus, CInt(.Item("procesado").ToString))
                txtAlmacen.Text = MuestraCampoTexto(.Item("almacen"), "")
                txtFechaProceso.Text = IIf(.Item("procesado") = 0, "", FormatoFecha(CDate(.Item("fechapro").ToString)))
                If .Item("procesado") = 0 Then
                    FechaProceso = MyDate
                Else
                    FechaProceso = CDate(.Item("fechapro").ToString)
                End If
                'Renglones
                AsignarMovimientos(.Item("conmer"))

                'FindField = "codigo"
                'BindingSource1.DataSource = dtRenglones
                'BindingSource1.Filter = " codigo like '" & Codigo & "%'"
                'dg.DataSource = BindingSource1

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

        strSQLMov = "select * from jsmerconmer " _
                        & " where CONMER = '" & NumeroDeConteo & "' and " _
                        & " aceptado < '2' and " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' and " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' " _
                        & " order by CODART "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        txtItems.Text = FormatoEntero(dtRenglones.Rows.Count)

        Dim aCampos() As String = {"codart", "nomart", "unidad", "existencia", "conteo", "cont1", "cont2", "cont3", "costou", "costo_tot", ""}
        Dim aNombres() As String = {"Item", "Descripción", "Unidad", "Existencia", "Conteo total", "Conteo 1", "Conteo 2", "Conteo 3", "Costo Unitario", "Costo Total", ""}
        Dim aAnchos() As Long = {70, 300, 45, 70, 70, 70, 70, 70, 70, 70, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {"", "", "", sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoNumero, sFormatoNumero, ""}
        IniciarTabla(dg, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, , True)

        If i_modo = movimiento.iEditar Then

            dg.ReadOnly = False
            dg.Columns("codart").ReadOnly = True
            dg.Columns("nomart").ReadOnly = True
            dg.Columns("unidad").ReadOnly = True
            dg.Columns("existencia").ReadOnly = True
            dg.Columns("conteo").ReadOnly = True
            'dg.Columns("costou").ReadOnly = True
            dg.Columns("costo_tot").ReadOnly = True
            dg.Columns("").ReadOnly = True

        Else
            dg.ReadOnly = True
        End If

        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub

    Private Sub IniciarConteo(ByVal Inicio As Boolean)

        If Inicio Then
            txtConteo.Text = "CIT" & NumeroAleatorio(100000)
        Else
            txtConteo.Text = ""
        End If

        txtComentario.Text = ""
        txtEmision.Text = FormatoFecha(sFechadeTrabajo)
        txtFechaProceso.Text = ""
        RellenaCombo(aTipo, cmbEstatus)
        txtItems.Text = FormatoEntero(0)

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", 0, 0)
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

        HabilitarObjetos(True, False, grpEncab, MenuBarraRenglon)
        HabilitarObjetos(True, True, txtComentario, btnEmision, btnAlmacen)

        If dg.RowCount > 0 Then

            dg.ReadOnly = False
            dg.Columns("codart").ReadOnly = True
            dg.Columns("nomart").ReadOnly = True
            dg.Columns("unidad").ReadOnly = True
            dg.Columns("existencia").ReadOnly = True
            dg.Columns("conteo").ReadOnly = True
            'dg.Columns("costou").ReadOnly = True
            dg.Columns("costo_tot").ReadOnly = True
            dg.Columns("").ReadOnly = True

        End If

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()
        Dim c As Control
        grpAceptarSalir.Visible = False

        For Each c In grpEncab.Controls
            HabilitarObjetos(False, True, c)
        Next

        dg.ReadOnly = True
        HabilitarObjetos(False, True, grpEncab)
        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
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
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsmerconmer where conmer = '" & txtConteo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado() As Boolean
        Validado = True
    End Function
    Private Sub GuardarTXT()

        Dim CodigoConteo As String = txtConteo.Text
        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count

            CodigoConteo = Contador(myConn, lblInfo, Gestion.iMercancías, "INVNUMCON", "03")
            EjecutarSTRSQL(myConn, lblInfo, " update jsmerconmer set conmer = '" & CodigoConteo & "' where conmer = '" & txtConteo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        InsertEditMERCASEncabezadoConteo(myConn, lblInfo, Inserta, CodigoConteo, CDate(txtEmision.Text), txtAlmacen.Text, txtComentario.Text, cmbEstatus.SelectedIndex, _
                                           FechaProceso)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" conmer = '" & CodigoConteo & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab

        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

        If dtRenglones.Rows.Count > 0 Then
            FindField = dtRenglones.Columns(aCampos(0)).ColumnName
            lblBuscar.Text = aNombres(0)
            BindingSource1.Filter = " codart " & " like '%%'"
            txtBuscar.Text = ""
        End If

    End Sub
    Private Sub txtComentario_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        MensajeEtiqueta(lblInfo, " Indique un comentario o descripción del proceso ... ", TipoMensaje.iInfo)
        txtComentario.MaxLength = 100
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarConteo(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        If cmbEstatus.Text = "POR PROCESAR" Then
            i_modo = movimiento.iEditar
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position
            ActivarMarco0()
        Else
            MensajeCritico(lblInfo, "ESTE CONTEO YA FUE PROCESADO. VERIFIQUE POR FAVOR...")
        End If
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        If dt.Rows(nPosicionEncab).Item("procesado").ToString = "0" Then

            Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position

            sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")

            If sRespuesta = MsgBoxResult.Yes Then

                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("conmer"))

                EjecutarSTRSQL(myConn, lblInfo, " delete from jsmerenccon where " _
                    & " conmer = '" & txtConteo.Text & "' AND " _
                    & " ID_EMP = '" & jytsistema.WorkID & "'")

                EjecutarSTRSQL(myConn, lblInfo, " delete from jsmerconmer where conmer = '" & txtConteo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                dt = ds.Tables(nTabla)
                If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                AsignaTXT(nPosicionEncab)

            End If
        Else
            MensajeCritico(lblInfo, "ESTE CONTEO YA FUE PROCESADO. VERIFIQUE POR FAVOR...")
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"conmer", "comentario", "fechacon"}
        Dim Nombres() As String = {"Número de Conteo", "Comentario", "Emisión"}
        Dim Anchos() As Long = {150, 650, 150}
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

    Private Sub dg_RowEnter(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.RowEnter

    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        nPosicionRenglon = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "ItemsRenglon", "lblItemsRenglon", e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
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
                    EjecutarSTRSQL(myConn, lblInfo, " update jsmerconmer set costou = " & Conteo & " , costo_tot = " & Conteo & "*conteo " _
                                                          & " where conmer = '" & txtConteo.Text & "' and codart = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' and id_emp = '" & jytsistema.WorkID & "' ")
                Else
                    EjecutarSTRSQL(myConn, lblInfo, " update jsmerconmer set conteo = " & Conteo & ", costo_tot = " & Conteo & "*costou " _
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
            MensajeAdvertencia(lblInfo, "Debe indicar dígito(s) válido...")
            e.Cancel = True
        End If

        If Not IsNumeric(e.FormattedValue.ToString()) Then
            MensajeAdvertencia(lblInfo, "Debe indicar un número válido...")
            e.Cancel = True
        End If

    End Sub

    Private Sub dg_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dg.CellEndEdit
        dg.Rows(e.RowIndex).ErrorText = String.Empty
    End Sub
    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        FindField = dtRenglones.Columns(aCampos(0)).ColumnName
        lblBuscar.Text = aNombres(0)

        If dtRenglones.Columns(FindField).DataType Is GetType(String) Then _
        BindingSource1.Filter = FindField & " like '%%'"

        Dim f As New jsMerProCargarMercanciasConteo
        f.Cargar(myConn, txtConteo.Text, txtAlmacen.Text)
        AsignarMovimientos(txtConteo.Text)
        f = Nothing
    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click

        EliminarMovimiento()

    End Sub
    Private Sub EliminarMovimiento()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then
            sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
            If sRespuesta = MsgBoxResult.Yes Then
                With dtRenglones.Rows(nPosicionRenglon)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("conmer") & " " & .Item("codart"))
                    Dim aCamposDel() As String = {"conmer", "codart", "id_emp"}
                    Dim aStringsDel() As String = {.Item("conmer"), .Item("codart"), jytsistema.WorkID}

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsmerconmer", strSQLMov, aCamposDel, aStringsDel, _
                                                  Me.BindingContext(ds, nTablaRenglones).Position, True)

                    If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1
                    AsignaMov(nPosicionRenglon, True)
                End With

            End If
        End If

    End Sub
    Private Sub btnBuscarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarMovimiento.Click

        Dim f As New frmBuscar
        Dim Campos() As String = {"codart", "nomart"}
        Dim Nombres() As String = {"Item", "Descripción"}
        Dim Anchos() As Long = {140, 350}
        f.Text = "Renglones de Conteo de inventario No. " & txtConteo.Text
        f.Buscar(dtRenglones, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaRenglones).Position, " " & Me.Tag & "...")
        AsignaMov(f.Apuntador, False)
        f = Nothing

    End Sub

    Private Sub btnPrimerMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimerMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position = 0
        AsignaMov(Me.BindingContext(ds, nTablaRenglones).Position, False)
    End Sub

    Private Sub btnAnteriorMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnteriorMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position -= 1
        AsignaMov(Me.BindingContext(ds, nTablaRenglones).Position, False)
    End Sub

    Private Sub btnSiguienteMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguienteMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position += 1
        AsignaMov(Me.BindingContext(ds, nTablaRenglones).Position, False)
    End Sub

    Private Sub btnUltimoMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimoMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position = ds.Tables(nTablaRenglones).Rows.Count - 1
        AsignaMov(Me.BindingContext(ds, nTablaRenglones).Position, False)
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
                EjecutarSTRSQL(myConn, lblInfo, " update jsmerconmer set " & Conteo & " = " & CDbl(dg.CurrentCell.Value) & " " _
                                        & " where conmer = '" & txtConteo.Text & "' and codart = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' and id_emp = '" & jytsistema.WorkID & "' ")

                If dg.CurrentCell.ColumnIndex = 8 Then
                    EjecutarSTRSQL(myConn, lblInfo, " update jsmerconmer set costou = " & Conteo & " , costo_tot = " & Conteo & "*conteo " _
                                                          & " where conmer = '" & txtConteo.Text & "' and codart = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' and id_emp = '" & jytsistema.WorkID & "' ")
                Else
                    EjecutarSTRSQL(myConn, lblInfo, " update jsmerconmer set conteo = " & Conteo & ", costo_tot = " & Conteo & "*costou " _
                                                           & " where conmer = '" & txtConteo.Text & "' and codart = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' and id_emp = '" & jytsistema.WorkID & "' ")
                End If


                ''AsignaMov(Me.BindingContext(ds, nTablaRenglones).Position, False)
        End Select
    End Sub
    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.ColumnHeaderMouseClick
        FindField = dtRenglones.Columns(aCampos(e.ColumnIndex)).ColumnName
        lblBuscar.Text = aNombres(e.ColumnIndex)
    End Sub
    
   

    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        BindingSource1.DataSource = dtRenglones
        If dtRenglones.Columns(FindField).DataType Is GetType(String) Then _
            BindingSource1.Filter = FindField & " like '%" & txtBuscar.Text & "%'"
        dg.DataSource = BindingSource1
    End Sub

    Private Sub btnRefrescar_Click(sender As System.Object, e As System.EventArgs) Handles btnRefrescar.Click



        If nPosicionEncab > -1 Then nPosicionEncab = Me.BindingContext(ds, nTabla).Position

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        If dt.Rows.Count > 0 Then

            Me.BindingContext(ds, nTabla).Position = nPosicionEncab
            AsignaTXT(nPosicionEncab)

        End If

    End Sub

    Private Sub dg_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub
End Class