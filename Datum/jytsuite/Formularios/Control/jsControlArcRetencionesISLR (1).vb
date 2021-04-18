Imports MySql.Data.MySqlClient
Public Class jsControlArcRetencionesISLR
    Private Const sModulo As String = "Retenciones del Impuesto Sobre La Renta (ISLR)"
    Private Const lRegion As String = "RibbonButton171"
    Private Const nTabla As String = "tblRetISLR"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable

    Private strSQL As String = " select * from jscontabret where id_emp = '" & jytsistema.WorkID & "' order by codret "
    Private Posicion As Long

    Private n_Seleccionado As String
    Private BindingSource1 As New BindingSource
    Private FindField As String
    Public Property Seleccionado() As String
        Get
            Return n_Seleccionado
        End Get
        Set(ByVal value As String)
            n_Seleccionado = value
        End Set
    End Property
    Public Sub Cargar(ByVal Mycon As MySqlConnection, ByVal TipoCarga As Integer)

        ' 0 = show() ; 1 = showdialog()

        myConn = Mycon

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        IniciarGrilla()
        AsignarTooltips()
        If dt.Rows.Count > 0 Then Posicion = 0
        AsignaTXT(Posicion, True)
        MensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", TipoMensaje.iAyuda)

        If TipoCarga = 0 Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If


    End Sub
    Private Sub jsControlArcRetencionesISLR_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt = Nothing
        ds = Nothing
    End Sub

    Private Sub jsControlArcRetencionesISLR_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
        C1SuperTooltip1.SetToolTip(btnProcesar, "<B>Recalcular</B> pagos mínimos y sustraendos <BR> a partir de la Unidad Tributaria vigente")


    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        If c >= 0 AndAlso ds.Tables(nTabla).Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = c
            With dt
                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)
            End With
            dg.CurrentCell = dg(0, c)
        End If
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
    End Sub
    Private Sub IniciarGrilla()
        Dim aCampos() As String = {"codret", "concepto", "tipo", "Baseimp", "Tarifa", "pagomin", "menos", "Acumula"}
        Dim aNombres() As String = {"Código", "Descripción/Concepto", "Persona", "% Base Imponible", "% Tarifa", "Pago Mínimo", "Sustraendo", "Acumula"}
        Dim aAnchos() As Long = {50, 440, 60, 70, 70, 120, 75, 65}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Centro}
        Dim aFormatos() As String = {"", "", "", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, ""}
        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

        FindField = "codret"
        BindingSource1.DataSource = dt
        BindingSource1.Filter = FindField & " like '" & txtBuscar.Text & "%'"
        dg.DataSource = BindingSource1

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim f As New jsControlArcRetencionesOSLRMovimiento
        f.Agregar(myConn, ds, dt)
        If f.Apuntador >= 0 Then AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim f As New jsControlArcRetencionesOSLRMovimiento
        f.Apuntador = Me.BindingContext(ds, nTabla).Position
        f.Editar(myConn, ds, dt)
        AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        'Dim sRespuesta As Integer
        'Posicion = Me.BindingContext(ds, nTabla).Position
        'sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", vbYesNo, "Eliminar registro en " & sModulo & " ...")
        'If sRespuesta = vbYes Then
        '    Dim aCampos() As String = {"codret", "id_emp"}
        '    Dim aString() As String = {dt.Rows(Posicion).Item("codret"), jytsistema.WorkID}
        '    EliminarRegistros(myConn, lblInfo, ds, nTabla, "jscontabret", strSQL, aCampos, aString, Posicion)
        'End If
        'AsignaTXT(Posicion, False)
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"codret", "concepto"}
        Dim Nombres() As String = {"Código ", "Nombre o descripción"}
        Dim Anchos() As Long = {50, 2500}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Retenciones del impuesto sobre la renta...")
        AsignaTXT(f.Apuntador, False)
        f = Nothing
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        Posicion = Me.BindingContext(ds, nTabla).Position
        If dt.Rows.Count > 0 Then
            Seleccionado = dt.Rows(Posicion).Item("codret").ToString
            InsertarAuditoria(myConn, MovAud.iSeleccionar, Me.Text, "")
        End If
        Me.Close()
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

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub
    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dg.CellFormatting
        Dim aAcumula() As String = {"NO", "SI"}
        Dim aPersona() As String = {"PNR", "PNNR", "PJD","PJND", "PJNCD"}
        Select Case dg.Columns(e.ColumnIndex).Name
            Case "Acumula"
                e.Value = aAcumula(e.Value)
            Case "tipo"
                e.Value = aPersona(e.Value)
        End Select
    End Sub
    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        Posicion = Me.BindingContext(ds, nTabla).Position
        If dt.Rows.Count > 0 Then
            Seleccionado = dt.Rows(Posicion).Item("codret").ToString
            InsertarAuditoria(myConn, MovAud.iSeleccionar, Me.Text, "")
        End If
        Me.Close()
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.CellMouseClick
        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        AsignaTXT(e.RowIndex, False)
    End Sub

    Private Sub dg_RegionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.RegionChanged
        Posicion = Me.BindingContext(ds, nTabla).Position
    End Sub

    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.ColumnHeaderMouseClick
        FindField = dt.Columns(e.ColumnIndex).ColumnName
        lblBuscar.Text = dg.Columns(e.ColumnIndex).HeaderText
    End Sub

    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        BindingSource1.DataSource = dt
        If dt.Columns(FindField).DataType Is GetType(String) Then _
            BindingSource1.Filter = FindField & " like '%" & txtBuscar.Text & "%'"
        dg.DataSource = BindingSource1
    End Sub

    Private Sub btnProcesar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        Dim fCont As Integer
        Dim Fx As Double = 1000 / 12
        Dim UnidadTributaria As Double = ValorUnidadTributaria(myConn, lblInfo, jytsistema.sFechadeTrabajo)
        For fCont = 0 To dt.Rows.Count - 1
            With dt.Rows(fCont)
                If .Item("tipo") = 0 Then
                    Dim PagoMinimo As Double = Math.Round(UnidadTributaria * Fx, 2)
                    Dim Sustraendo As Double = Math.Round(UnidadTributaria * Fx * .Item("Tarifa") / 100, 2)
                    EjecutarSTRSQL(myConn, lblInfo, " update jscontabret set pagomin = " & PagoMinimo & ", menos = " & Sustraendo & " where codret = '" & .Item("codret") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                End If
            End With
        Next
        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        dg.Refresh()

    End Sub
End Class