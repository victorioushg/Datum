Imports MySql.Data.MySqlClient
Public Class jsPOSArcCajas
    Private Const sModulo As String = "Cajas ó puntos de  venta"
    Private Const lRegion As String = "RibbonButton91"
    Private Const nTabla As String = "cajaspos"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable

    Private strSQL As String = "select * from jsvencatcaj where id_emp = '" & jytsistema.WorkID & "' order by codcaj"
    Private Posicion As Long

    Private n_Seleccionado As String
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
    Private Sub jsPOSArcCajas_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt = Nothing
        ds = Nothing
    End Sub

    Private Sub jsPOSArcCajas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
        Dim aCampos() As String = {"codcaj", "descrip", "impre_fiscal", ""}
        Dim aNombres() As String = {"Código", "Nombre", "Tipo Impresora", ""}
        Dim aAnchos() As Long = {70, 200, 90, 10}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {"", "", "", ""}
        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim f As New jsPOSArcCajasMovimientos
        f.Agregar(myConn, ds, dt)
        If f.Apuntador >= 0 Then AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim f As New jsPOSArcCajasMovimientos
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
            Dim aCampos() As String = {"codcaj", "id_emp"}
            Dim aString() As String = {dt.Rows(Posicion).Item("codcaj"), jytsistema.WorkID}
            EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsvencatcaj", strSQL, aCampos, aString, Posicion)
        End If
        AsignaTXT(Posicion, False)
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"codcaj", "descrip", "maquinafiscal"}
        Dim Nombres() As String = {"Código Caja", "Nombre Caja", "Serial Caja"}
        Dim Anchos() As Long = {100, 200, 100}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Cajas de punto de venta...")
        AsignaTXT(f.Apuntador, False)
        f = Nothing
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        Posicion = Me.BindingContext(ds, nTabla).Position
        If dt.Rows.Count > 0 Then
            Seleccionado = dt.Rows(Posicion).Item("codcaj").ToString
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

    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        Posicion = Me.BindingContext(ds, nTabla).Position
        If dt.Rows.Count > 0 Then
            Seleccionado = dt.Rows(Posicion).Item("codcaj").ToString
            InsertarAuditoria(myConn, MovAud.iSeleccionar, Me.Text, "")
        End If
        Me.Close()
    End Sub
    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dg.CellFormatting
        Select Case dg.Columns(e.ColumnIndex).Name
            Case "tipoimpresora"
                e.Value = aFiscal(e.Value)
        End Select
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.CellMouseClick
        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        AsignaTXT(e.RowIndex, False)
    End Sub

    Private Sub dg_RegionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.RegionChanged
        Posicion = Me.BindingContext(ds, nTabla).Position
    End Sub

End Class