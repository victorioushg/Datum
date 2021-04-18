Imports MySql.Data.MySqlClient
Public Class jsMerArcPreciosFuturos
    Private Const sModulo As String = "Precios a futuro"
    Private Const lRegion As String = "RibbonButton140"
    Private Const nTabla As String = "tblPrecios"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private strSQL As String
    Private Posicion As Long
    Public Sub Cargar(ByVal Mycon As MySqlConnection)

        Me.Dock = DockStyle.Fill
        myConn = Mycon

        strSQL = "select a.*, b.nomart, b.unidad " _
            & " from jsmerlispre a " _
            & " left join jsmerctainv b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
            & " Where " _
            & " a.procesado = 0 " _
            & " and a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by codart, tipoprecio, fecha desc "

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        IniciarGrilla()
        AsignarTooltips()
        If dt.Rows.Count > 0 Then Posicion = 0
        AsignaTXT(Posicion, True)
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)

        Me.Show()

    End Sub
    Private Sub jsMerArcPreciosFuturos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt = Nothing
        ds = Nothing
    End Sub

    Private Sub jsMerArcPreciosFuturos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean, Optional ByVal NumeroCelda As Integer = 0)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        If c >= 0 AndAlso ds.Tables(nTabla).Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = c
            With dt
                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)
            End With
            dg.CurrentCell = dg(NumeroCelda, c)
        End If
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
    End Sub
    Private Sub IniciarGrilla()
        Dim aCampos() As String = {"fecha", "codart", "nomart", "unidad", "tipoprecio", "monto", "des_art", ""}
        Dim aNombres() As String = {"Fecha Precio", "Código", "Descripción", "UND", "Tarifa Precio", "Precio", "Descuento por tarifa", ""}
        Dim aAnchos() As Integer = {100, 90, 350, 40, 50, 120, 70, 70}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {sFormatoFecha, "", "", "", "", sFormatoNumero, sFormatoNumero, ""}
        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim f As New jsMerArcPreciosFuturosMovimientos
        f.Apuntador = Me.BindingContext(ds, nTabla).Position
        f.Agregar(myConn, ds, dt)
        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim f As New jsMerArcPreciosFuturosMovimientos
        f.Apuntador = Me.BindingContext(ds, nTabla).Position
        f.Editar(myConn, ds, dt)
        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        Posicion = Me.BindingContext(ds, nTabla).Position

        If Posicion >= 0 Then
            If ft.PreguntaEliminarRegistro() = DialogResult.Yes Then
                With dt.Rows(Posicion)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(Posicion).Item("codart"))
                    Dim aCamposDel() As String = {"fecha", "codart", "tipoprecio", "id_emp"}
                    Dim aStringsDel() As String = {ft.FormatoFechaMySQL(.Item("fecha").ToString), .Item("codart"), .Item("tipoprecio"), jytsistema.WorkID}

                    Posicion = EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsmerlispre", strSQL, aCamposDel, aStringsDel, _
                                                  Me.BindingContext(ds, nTabla).Position, True)

                    If dt.Rows.Count - 1 < Posicion Then Posicion = dt.Rows.Count - 1
                    AsignaTXT(Posicion, True)
                End With

            End If
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"codart", "nomart", ""}
        Dim Nombres() As String = {"Código", "Descripción", ""}
        Dim Anchos() As Integer = {150, 450, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Lista de precios a futuro...")
        AsignaTXT(f.Apuntador, False)
        f = Nothing
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

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.CellMouseClick
        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        AsignaTXT(e.RowIndex, False)
    End Sub

    Private Sub dg_RegionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.RegionChanged
        Posicion = Me.BindingContext(ds, nTabla).Position
    End Sub

    Private Sub btnImprimir_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cPreciosFuturos, "Precios A Futuro")
        f = Nothing
    End Sub
End Class