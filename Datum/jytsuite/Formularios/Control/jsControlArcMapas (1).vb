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
        MensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", TipoMensaje.iAyuda)
        Me.Dock = DockStyle.Fill
        Me.Show()


    End Sub
    Private Sub IniciarModulo(ByVal numModulo As Integer)

        strSQLMapas = " select * from jsconencmap order by mapa "
        ds = DataSetRequery(ds, strSQLMapas, myConn, nTabla, lblInfo)
        dtMapa = ds.Tables(nTabla)

        DesactivarMarco()
        RellenaCombo(aGestion, cmbGestion, iiGestion)
        If dtMapa.Rows.Count > 0 Then
            Posicion = 0
            Me.BindingContext(ds, nTabla).Position = Posicion
            AsignaTXT(Posicion)
        ElseIf dtMapa.Rows.Count = 0 Then
            i_modo = movimiento.iAgregar
            Posicion = Me.BindingContext(ds, nTabla).Position
            ActivarMarco()
            IniciarMapa(True)
        Else
            IniciarMapa(False)
        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtMapa, MenuBarra)
        AsignarTooltips()


    End Sub
    Private Sub IniciarMapa(ByVal Inicio As Boolean)

        Dim c As Control
        For Each c In grpMap.Controls
            If c.GetType Is txtMapa.GetType Then c.Text = ""
        Next
        Dim aWhereFields() As String = {}
        Dim aWhereValues() As String = {}

        chkAcceso.Checked = True
        chkIncluir.Checked = True
        chkModificar.Checked = True
        chkEliminar.Checked = True

        dg.Columns.Clear()

        If Inicio Then
            txtMapa.Text = AutoCodigoXPlus(myConn, "mapa", "jsconencmap", aWhereFields, aWhereValues, 3, True)
            VerificaMapa()
        End If


    End Sub
    Private Sub jsControlArcMapas_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

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

            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

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
                AbrirRenglonMapa(lMapa)
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
            End With
        End With

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtMapa, MenuBarra)


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

        'Dim aCampos() As String = {"mapa", "region", "gestion", "nivel", "modulo"}
        Dim aCampos() As String = {"mapa", "region"}

        Dim aStr() As String
        If aStrings.Length > 0 Then
            For i = 0 To UBound(aStrings)
                aStr = Split(aStrings(i).ToString, ".")
                'Dim aString() As String = {Mapa, aStr(0), aStr(2), aStr(3), aStr(4)}
                Dim aString() As String = {Mapa, aStr(0)}

                If qFound(myConn, lblInfo, "jsconrenglonesmapa", aCampos, aString) Then
                    EjecutarSTRSQL(myConn, lblInfo, " update jsconrenglonesmapa set descripcion = '" & aStr(1) & "', nivel = " & aStr(3) & ", orden = " & i & "  where mapa = '" & Mapa & "' and gestion = " & aStr(2) & " and region = '" & aStr(0) & "' and modulo = " & aStr(4) & " ")
                Else
                    EjecutarSTRSQL(myConn, lblInfo, " insert into jsconrenglonesmapa set mapa = '" & Mapa & "', " _
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

        HabilitarObjetos(True, True, txtDescripcion, cmbGestion)
        HabilitarObjetos(True, False, dg, chkAcceso, chkEliminar, chkIncluir, chkModificar)

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
    End Sub
    Private Sub DesactivarMarco()

        grpAceptarSalir.Visible = False

        HabilitarObjetos(False, True, txtDescripcion, txtMapa, cmbGestion)
        HabilitarObjetos(False, False, dg, chkAcceso, chkEliminar, chkIncluir, chkModificar)

        MenuBarra.Enabled = True
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
    End Sub

    Private Sub IniciarGrilla()
        Dim aCampos() As String = {"descrip", "acceso", "incluye", "modifica", "elimina", "control"}
        Dim aNombres() As String = {"Descripción", "Acceso", "Incluye", "Modifica", "Elimina", ""}
        Dim aAnchos() As Long = {400, 100, 100, 100, 100, 100}
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
        Dim sRespuesta As Integer
        Posicion = Me.BindingContext(ds, nTabla).Position

        Dim nReg As Integer = CInt(EjecutarSTRSQL_ScalarPLUS(myConn, " select count(*) from jsconctausu where mapa = '" & txtMapa.Text & "' "))
        If nReg > 0 Then
            MensajeCritico(lblInfo, "(" & txtMapa.Text & ") MAPA NO ELIMINABLE...")
        Else
            sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", vbYesNo, "Eliminar registro en " & sModulo & " ...")
            If sRespuesta = vbYes Then
                Dim aCampos() As String = {"mapa"}
                Dim aString() As String = {txtMapa.Text}
                Posicion = EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsconencmap", strSQLMapas, aCampos, aString, Posicion)
                Me.BindingContext(ds, nTabla).Position = Posicion
                EjecutarSTRSQL(myConn, lblInfo, " delete from jsconrenglonesmapa where mapa = '" & txtMapa.Text & "' ")
            End If
            AsignaTXT(Posicion)
        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"mapa", "nombre"}
        Dim Nombres() As String = {"Código", "Descripción"}
        Dim Anchos() As Long = {100, 2500}
        f.Buscar(dtMapa, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Mapas de usuario...")
        Posicion = f.Apuntador
        Me.BindingContext(ds, nTabla).Position = Posicion
        AsignaTXT(Posicion)
        f = Nothing
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        'Posicion = Me.BindingContext(ds, nTabla).Position
        'If dt.Rows.Count > 0 Then
        ' Seleccionado = dt.Rows(Posicion).Item("codtar").ToString
        ' InsertarAuditoria(myConn, MovAud.iSeleccionar, Me.Text, "")
        ' End If
        ' Me.Close()
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
                EjecutarSTRSQL(myConn, lblInfo, " update jsconrenglonesmapa set " & dg.Columns(e.ColumnIndex).Name & " =  " & ff & " where region = '" & dtMov.Rows(e.RowIndex).Item("region").ToString & "' and mapa = '" & txtMapa.Text & "' ")
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
        End If
    End Sub
    Private Function Validado() As Boolean
        If txtDescripcion.Text = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar una descipción válida para este mapa...")
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
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtMapa, MenuBarra)
        i_modo = movimiento.iConsultar

    End Sub
    Private Sub AgregaYCancela()

        EjecutarSTRSQL(myConn, lblInfo, " delete from jsconencmap where mapa = '" & txtMapa.Text & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsconrenglonesmapa where mapa = '" & txtMapa.Text & "' ")
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

    Private Sub chkAcceso_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAcceso.CheckedChanged, _
        chkIncluir.CheckedChanged, chkModificar.CheckedChanged, chkEliminar.CheckedChanged
        Dim Campo As String = IIf(sender.name = "chkAcceso", "acceso", IIf(sender.name = "chkIncluir", "incluye", IIf(sender.name = "chkModificar", "modifica", "elimina")))
        EjecutarSTRSQL(myConn, lblInfo, " update jsconrenglonesmapa set " & Campo & " = " & sender.Checked & " where mapa = '" & txtMapa.Text & "' and gestion = " & cmbGestion.SelectedIndex + 1 & " ")

        AbrirRenglonMapa(txtMapa.Text)

    End Sub

    Private Sub btnVerifica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVerifica.Click

        EsperaPorFavor()

        VerificaMapa()

        MensajeInformativoPlus("Verificación de menú terminada...")


    End Sub

    Private Sub VerificaMapa()
        VerificaMenu(txtMapa.Text, jytsistema.aMenuArchivosContabilidad)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuProcesosContabilidad)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuReportesContabilidad)

        VerificaMenu(txtMapa.Text, jytsistema.aMenuArchivosBancos)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuProcesosBancos)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuReportesBancos)

        VerificaMenu(txtMapa.Text, jytsistema.aMenuArchivosNomina)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuProcesosNomina)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuReportesNomina)

        VerificaMenu(txtMapa.Text, jytsistema.aMenuArchivosCompras)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuProcesosCompras)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuReportesCompras)

        VerificaMenu(txtMapa.Text, jytsistema.aMenuArchivosVentas)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuProcesosVentas)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuReportesVentas)

        VerificaMenu(txtMapa.Text, jytsistema.aMenuArchivosPuntoDeVenta)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuProcesosPuntoDeVenta)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuReportesPuntoDeVenta)

        VerificaMenu(txtMapa.Text, jytsistema.aMenuArchivosMercancias)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuProcesosMercancias)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuReportesMercancias)

        VerificaMenu(txtMapa.Text, jytsistema.aMenuReportesMedicionGerencial)

        VerificaMenu(txtMapa.Text, jytsistema.aMenuArchivosProduccion)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuProcesosProduccion)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuReportesProduccion)

        VerificaMenu(txtMapa.Text, jytsistema.aMenuArchivosControl)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuProcesosControl)
        VerificaMenu(txtMapa.Text, jytsistema.aMenuReportesControl)

        AbrirRenglonMapa(txtMapa.Text)
    End Sub

End Class