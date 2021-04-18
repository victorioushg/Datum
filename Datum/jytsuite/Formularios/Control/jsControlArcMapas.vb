Imports MySql.Data.MySqlClient
Public Class jsControlArcMapas
    Private Const sModulo As String = "Mapas de accesibilidad"
    Private Const lRegion As String = "RibbonButton168"
    Private Const nTabla As String = "mapas"
    Private Const nTablaMov As String = "renglonesmapas"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dtMapa As New DataTable
    Private dtMov As New DataTable
    Private ft As New Transportables

    Private strSQLMapas As String
    Private strSQL As String
    Private Posicion As Long

    Private i_modo As Integer
    Private n_Seleccionado As String

    Private iiGestion As Integer = Gestion.iContabilidad - 1
    Private aGestionesVisibles() As Boolean
    Private prevIndex As Integer
    Public Sub Cargar(ByVal Mycon As MySqlConnection, ByVal aGestionVisible() As Boolean)

        myConn = Mycon

        aGestionesVisibles = aGestionVisible

        IniciarModulo(iiGestion)
        AsignarTooltips()
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)
        Me.Dock = DockStyle.Fill
        Me.Show()


    End Sub
    Private Sub IniciarModulo(ByVal numModulo As Integer)

        strSQLMapas = " select * from jsconencmap order by mapa "
        ds = DataSetRequery(ds, strSQLMapas, myConn, nTabla, lblInfo)
        dtMapa = ds.Tables(nTabla)

        DesactivarMarco()
        ft.RellenaCombo(aGestion, cmbGestion, iiGestion)
        If dtMapa.Rows.Count > 0 Then
            Posicion = 0
            Me.BindingContext(ds, nTabla).Position = Posicion
            AsignaTXT(Posicion)
            i_modo = movimiento.iConsultar
        ElseIf dtMapa.Rows.Count = 0 Then
            i_modo = movimiento.iAgregar
            Posicion = Me.BindingContext(ds, nTabla).Position
            ActivarMarco()
            IniciarMapa(True)
        Else
            IniciarMapa(False)
            i_modo = movimiento.iConsultar
        End If

        ft.ActivarMenuBarra(myConn, ds, dtMapa, lRegion, MenuBarra, jytsistema.sUsuario)

        AsignarTooltips()


    End Sub
    Private Sub IniciarMapa(ByVal Inicio As Boolean)

        Dim c As Control
        For Each c In grpMap.Controls
            If c.GetType Is txtMapa.GetType Then c.Text = ""
        Next

        chkAcceso.Checked = True
        chkIncluir.Checked = True
        chkModificar.Checked = True
        chkEliminar.Checked = True

        dg.Columns.Clear()

        If Inicio Then
            txtMapa.Text = ft.autoCodigo(myConn, "mapa", "jsconencmap", "", "", 3, True)
            VerificaMapa(txtMapa.Text)
        End If


    End Sub
    Private Sub jsControlArcMapas_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dtMapa = Nothing
        ds = Nothing
    End Sub

    Private Sub jsControlArcMapas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
        C1SuperTooltip1.SetToolTip(btnVerifica, "<B>Construye renglones del mapa</B>")

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dtMapa

            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)
                Dim lMapa As String = ""
                If i_modo = movimiento.iAgregar Then
                    lMapa = txtMapa.Text
                Else
                    lMapa = .Item("mapa")
                    txtDescripcion.Text = IIf(IsDBNull(.Item("nombre")), "", .Item("nombre"))
                End If
                txtMapa.Text = lMapa

                Dim fld() As String = {"mapa", "gestion", "nivel"}
                Dim str() As String = {lMapa, cmbGestion.SelectedIndex + 1, 0}
                If qFound(myConn, lblInfo, "jsconrenglonesmapa", fld, str) Then
                    chkAcceso.Checked = CBool(qFoundAndSign(myConn, lblInfo, "jsconrenglonesmapa", fld, str, "acceso"))
                    chkIncluir.Checked = CBool(qFoundAndSign(myConn, lblInfo, "jsconrenglonesmapa", fld, str, "incluye"))
                    chkModificar.Checked = CBool(qFoundAndSign(myConn, lblInfo, "jsconrenglonesmapa", fld, str, "modifica"))
                    chkEliminar.Checked = CBool(qFoundAndSign(myConn, lblInfo, "jsconrenglonesmapa", fld, str, "elimina"))
                Else
                    chkAcceso.Checked = False
                    chkIncluir.Checked = False
                    chkModificar.Checked = False
                    chkEliminar.Checked = False
                End If

                AbrirRenglonMapa(lMapa)

            End With
        End With

        ft.ActivarMenuBarra(myConn, ds, dtMapa, lRegion, MenuBarra, jytsistema.sUsuario)


    End Sub
    Private Sub AbrirRenglonMapa(ByVal nMapa As String)

        strSQL = "select mapa, acceso, incluye, modifica, elimina , " _
            & " concat(if( nivel = 0 , '', if(nivel = 1, '    ', if ( nivel = 2, '        ', '               ') ) ), descripcion) descrip, region " _
                    & " from jsconrenglonesmapa " _
                    & " where " _
                    & " gestion = " & iiGestion & " and " _
                    & " mapa = '" & nMapa & "' " _
                    & " order by modulo, orden "

        ds = DataSetRequery(ds, strSQL, myConn, nTablaMov, lblInfo)
        dtMov = ds.Tables(nTablaMov)
        IniciarGrilla()

    End Sub
    Private Sub VerificaMenu(ByVal Mapa As String, ByVal aStrings() As String)

        Dim i As Integer

        Dim aCampos() As String = {"mapa", "region"}

        Dim aStr() As String
        If aStrings.Length > 0 Then
            For i = 0 To UBound(aStrings)
                aStr = Split(aStrings(i).ToString, ".")

                Dim aString() As String = {Mapa, aStr(0)}

                If qFound(myConn, lblInfo, "jsconrenglonesmapa", aCampos, aString) Then
                    ft.Ejecutar_strSQL(myconn, " update jsconrenglonesmapa set descripcion = '" & aStr(1) & "', nivel = " & aStr(3) & ", orden = " & i & "  where mapa = '" & Mapa & "' and gestion = " & aStr(2) & " and region = '" & aStr(0) & "' and modulo = " & aStr(4) & " ")
                Else
                    ft.Ejecutar_strSQL(myconn, " insert into jsconrenglonesmapa set mapa = '" & Mapa & "', " _
                                    & " region = '" & aStr(0) & "', " _
                                    & " descripcion = '" & aStr(1) & "', " _
                                    & " gestion = " & aStr(2) & ", " _
                                    & " nivel = " & aStr(3) & ", " _
                                    & " modulo = " & aStr(4) & ", orden = " & i & ", acceso = 1, incluye = 1, modifica = 1, elimina = 1  ")
                End If
            Next
        End If
    End Sub
    Private Sub ActivarMarco()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, True, txtDescripcion, cmbGestion)
        ft.habilitarObjetos(True, False, dg, chkAcceso, chkEliminar, chkIncluir, chkModificar)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)
    End Sub
    Private Sub DesactivarMarco()

        grpAceptarSalir.Visible = False

        ft.habilitarObjetos(False, True, txtDescripcion, txtMapa, cmbGestion)
        ft.habilitarObjetos(False, False, dg, chkAcceso, chkEliminar, chkIncluir, chkModificar)

        MenuBarra.Enabled = True
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)
    End Sub

    Private Sub IniciarGrilla()
        Dim aCampos() As String = {"descrip", "acceso", "incluye", "modifica", "elimina", "control"}
        Dim aNombres() As String = {"Descripción", "Acceso", "Incluye", "Modifica", "Elimina", ""}
        Dim aAnchos() As Integer = {400, 100, 100, 100, 100, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {"", "", "", "", "", ""}
        IniciarTabla(dg, dtMov, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        Posicion = Me.BindingContext(ds, nTabla).Position
        ActivarMarco()
        IniciarMapa(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        i_modo = movimiento.iEditar
        Posicion = Me.BindingContext(ds, nTabla).Position
        ActivarMarco()
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        Posicion = Me.BindingContext(ds, nTabla).Position

        Dim nReg As Integer = ft.DevuelveScalarEntero(myConn, " select count(*) from jsconctausu where mapa = '" & txtMapa.Text & "' ")
        If nReg > 0 Then
            ft.MensajeCritico("(" & txtMapa.Text & ") MAPA NO ELIMINABLE...")
        Else

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                Dim aCampos() As String = {"mapa"}
                Dim aString() As String = {txtMapa.Text}
                Posicion = EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsconencmap", strSQLMapas, aCampos, aString, Posicion)
                Me.BindingContext(ds, nTabla).Position = Posicion
                ft.Ejecutar_strSQL(myConn, " delete from jsconrenglonesmapa where mapa = '" & txtMapa.Text & "' ")
            End If
            AsignaTXT(Posicion)
        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"mapa", "nombre"}
        Dim Nombres() As String = {"Código", "Descripción"}
        Dim Anchos() As Integer = {100, 2500}
        f.Buscar(dtMapa, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Mapas de usuario...")
        Posicion = f.Apuntador
        Me.BindingContext(ds, nTabla).Position = Posicion
        AsignaTXT(Posicion)
        f = Nothing
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Me.BindingContext(ds, nTabla).Position = 0
        Posicion = 0
        AsignaTXT(Posicion)
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Me.BindingContext(ds, nTabla).Position -= 1
        Posicion = Me.BindingContext(ds, nTabla).Position
        AsignaTXT(Posicion)
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Me.BindingContext(ds, nTabla).Position += 1
        Posicion = Me.BindingContext(ds, nTabla).Position
        AsignaTXT(Posicion)
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
        Posicion = Me.BindingContext(ds, nTabla).Position
        AsignaTXT(Posicion)
    End Sub


    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub dg_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellClick
        If dtMov.Rows(e.RowIndex).Item("region").ToString.Length <> 0 Then
            If e.ColumnIndex >= 1 And e.ColumnIndex <= 4 Then
                Dim ff As Integer
                If CBool(dtMov.Rows(e.RowIndex).Item(dg.Columns(e.ColumnIndex).Name).ToString) = True Then
                    ff = 0
                Else
                    ff = 1
                End If
                ft.Ejecutar_strSQL(myconn, " update jsconrenglonesmapa set " & dg.Columns(e.ColumnIndex).Name & " =  " & ff & " where region = '" & dtMov.Rows(e.RowIndex).Item("region").ToString & "' and mapa = '" & txtMapa.Text & "' ")
                dtMov.Rows(e.RowIndex).Item(dg.Columns(e.ColumnIndex).Name) = Not CBool(dtMov.Rows(e.RowIndex).Item(dg.Columns(e.ColumnIndex).Name).ToString)
            End If
        End If
    End Sub

    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dg.CellFormatting
        Select Case dg.Columns(e.ColumnIndex).Name
            Case "acceso", "incluye", "modifica", "elimina"
                If CBool(e.Value) Then
                    e.Value = "Si"
                Else
                    e.Value = "No"
                End If
        End Select
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
            i_modo = movimiento.iConsultar
        End If
    End Sub
    Private Function Validado() As Boolean
        If txtDescripcion.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar una descipción válida para este mapa...")
            Exit Function
        End If
        Validado = True
    End Function
    Private Sub GuardarTXT()

        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            Posicion = ds.Tables(nTabla).Rows.Count
            Me.BindingContext(ds, nTabla).Position = Posicion
        End If

        InsertEditCONTROLEncabMapa(myConn, lblInfo, Inserta, txtMapa.Text, txtDescripcion.Text, _
                                   chkIncluir.Checked, chkModificar.Checked, chkEliminar.Checked)

        ds = DataSetRequery(ds, strSQLMapas, myConn, nTabla, lblInfo)
        dtMapa = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = Posicion
        AsignaTXT(Posicion)
        DesactivarMarco()
        ft.ActivarMenuBarra(myConn, ds, dtMapa, lRegion, MenuBarra, jytsistema.sUsuario)

        i_modo = movimiento.iConsultar

    End Sub
    Private Sub AgregaYCancela()

        ft.Ejecutar_strSQL(myconn, " delete from jsconencmap where mapa = '" & txtMapa.Text & "' ")
        ft.Ejecutar_strSQL(myconn, " delete from jsconrenglonesmapa where mapa = '" & txtMapa.Text & "' ")
        i_modo = movimiento.iConsultar

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If i_modo = movimiento.iAgregar Then AgregaYCancela()
        If dtMapa.Rows.Count = 0 Then
            IniciarMapa(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            If Me.BindingContext(ds, nTabla).Position > 0 Then _
                Posicion = Me.BindingContext(ds, nTabla).Position
            AsignaTXT(Posicion)
        End If
        DesactivarMarco()
    End Sub

    Private Sub cmbGestion_SelectionChangeCommitted(sender As Object, e As System.EventArgs) Handles cmbGestion.SelectionChangeCommitted
        prevIndex = cmbGestion.SelectedIndex
    End Sub

    Private Sub cmbGestion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbGestion.SelectedIndexChanged

        iiGestion = cmbGestion.SelectedIndex + 1
        If Posicion >= 0 Then AsignaTXT(Posicion)

    End Sub
    Private Sub chkAcceso_Click(sender As Object, e As EventArgs) Handles chkAcceso.Click, chkIncluir.Click, _
        chkModificar.Click, chkEliminar.Click

        Dim Campo As String = IIf(sender.name = "chkAcceso", "acceso", IIf(sender.name = "chkIncluir", "incluye", IIf(sender.name = "chkModificar", "modifica", "elimina")))
        ft.Ejecutar_strSQL(myConn, " update jsconrenglonesmapa set " & Campo & " = " & sender.Checked & " where mapa = '" & txtMapa.Text & "' and gestion = " & cmbGestion.SelectedIndex + 1 & " ")

        AbrirRenglonMapa(txtMapa.Text)
    End Sub
    
    Private Sub btnVerifica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVerifica.Click

        EsperaPorFavor()
        For Each nRow As DataRow In dtMapa.Rows
            With nRow
                VerificaMapa(.Item("mapa"))
            End With
        Next
        ft.mensajeInformativo("Verificación de menú terminada...")

    End Sub

    Private Sub VerificaMapa(CodigoMapa As String)

        VerificaMenu(CodigoMapa, jytsistema.aMenuArchivosContabilidad)
        VerificaMenu(CodigoMapa, jytsistema.aMenuProcesosContabilidad)
        VerificaMenu(CodigoMapa, jytsistema.aMenuReportesContabilidad)

        VerificaMenu(CodigoMapa, jytsistema.aMenuArchivosBancos)
        VerificaMenu(CodigoMapa, jytsistema.aMenuProcesosBancos)
        VerificaMenu(CodigoMapa, jytsistema.aMenuReportesBancos)

        VerificaMenu(CodigoMapa, jytsistema.aMenuArchivosNomina)
        VerificaMenu(CodigoMapa, jytsistema.aMenuProcesosNomina)
        VerificaMenu(CodigoMapa, jytsistema.aMenuReportesNomina)

        VerificaMenu(CodigoMapa, jytsistema.aMenuArchivosCompras)
        VerificaMenu(CodigoMapa, jytsistema.aMenuProcesosCompras)
        VerificaMenu(CodigoMapa, jytsistema.aMenuReportesCompras)

        VerificaMenu(CodigoMapa, jytsistema.aMenuArchivosVentas)
        VerificaMenu(CodigoMapa, jytsistema.aMenuProcesosVentas)
        VerificaMenu(CodigoMapa, jytsistema.aMenuReportesVentas)

        VerificaMenu(CodigoMapa, jytsistema.aMenuArchivosPuntoDeVenta)
        VerificaMenu(CodigoMapa, jytsistema.aMenuProcesosPuntoDeVenta)
        VerificaMenu(CodigoMapa, jytsistema.aMenuReportesPuntoDeVenta)

        VerificaMenu(CodigoMapa, jytsistema.aMenuArchivosMercancias)
        VerificaMenu(CodigoMapa, jytsistema.aMenuProcesosMercancias)
        VerificaMenu(CodigoMapa, jytsistema.aMenuReportesMercancias)

        VerificaMenu(CodigoMapa, jytsistema.aMenuArchivosMedicionGerencial)
        VerificaMenu(CodigoMapa, jytsistema.aMenuReportesMedicionGerencial)

        VerificaMenu(CodigoMapa, jytsistema.aMenuArchivosProduccion)
        VerificaMenu(CodigoMapa, jytsistema.aMenuProcesosProduccion)
        VerificaMenu(CodigoMapa, jytsistema.aMenuReportesProduccion)

        VerificaMenu(CodigoMapa, jytsistema.aMenuArchivosControl)
        VerificaMenu(CodigoMapa, jytsistema.aMenuProcesosControl)
        VerificaMenu(CodigoMapa, jytsistema.aMenuReportesControl)

        AbrirRenglonMapa(CodigoMapa)
    End Sub


End Class