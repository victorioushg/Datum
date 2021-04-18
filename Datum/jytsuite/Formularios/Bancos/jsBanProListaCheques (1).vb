Imports MySql.Data.MySqlClient
Public Class jsBanProListaCheques
    Private Const sModulo As String = "Listado de cheques"
    Private Const lRegion As String = "RibbonButton9"
    Private Const nTabla As String = "tblLisCheques"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable

    Private strSQL As String
    Private Posicion As Long

    Private n_NumeroCheque As String
    Private n_BancoCheque As String
    Private n_BancoDeposito As String
    Private n_MontoCheque As Double
    Private n_NumeroDeposito As String
    Private n_NumeroCancelacion As String


    Public Property NumeroCheque() As String
        Get
            Return n_NumeroCheque
        End Get
        Set(ByVal value As String)
            n_NumeroCheque = value
        End Set
    End Property
    Public Property BancoDeposito() As String
        Get
            Return n_BancoDeposito
        End Get
        Set(ByVal value As String)
            n_BancoDeposito = value
        End Set
    End Property
    Public Property BancoCheque() As String
        Get
            Return n_BancoCheque
        End Get
        Set(ByVal value As String)
            n_BancoCheque = value
        End Set
    End Property
    Public Property NumeroDeposito() As String
        Get
            Return n_NumeroDeposito
        End Get
        Set(ByVal value As String)
            n_NumeroDeposito = value
        End Set
    End Property
    Public Property NumeroCancelacion() As String
        Get
            Return n_NumeroCancelacion
        End Get
        Set(ByVal value As String)
            n_NumeroCancelacion = value
        End Set
    End Property
    Public Property Montocheque() As Double
        Get
            Return n_MontoCheque
        End Get
        Set(ByVal value As Double)
            n_MontoCheque = value
        End Set
    End Property

    Public Sub Cargar(ByVal Mycon As MySqlConnection, ByVal FechaDesde As Date)


        myConn = Mycon
        Me.Tag = sModulo

        strSQL = " select * from jsbantracaj where " _
            & "(ORIGEN = 'CXC' OR ORIGEN = 'FAC') AND " _
            & "TIPOMOV = 'EN' AND FORMPAG = 'CH' AND DEPOSITO <> '' AND " _
            & "FECHA >= '" & FormatoFechaMySQL(DateAdd("m", -3, FechaDesde)) & "' AND " _
            & "ID_EMP = '" & jytsistema.WorkID & "' "

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        IniciarGrilla()
        AsignarTooltips()
        If dt.Rows.Count > 0 Then Posicion = 0
        AsignaTXT(Posicion, True)
        MensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", TipoMensaje.iAyuda)

        Me.ShowDialog()

    End Sub
    Private Sub jsBanProListaCheques_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt = Nothing
        ds = Nothing
    End Sub

    Private Sub jsBanProListaCheques_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
        Dim aCampos() As String = {"fecha", "numpag", "refpag", "deposito", "fecha_dep", "importe", "nummov"}
        Dim aNombres() As String = {"Fecha", "Cheque Nº", "Banco", "Depósito", "Fecha Dep.", "Importe", "Nº Cancelación"}
        Dim aAnchos() As Long = {100, 100, 70, 100, 100, 120, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {sFormatoFecha, "", "", "", sFormatoFecha, sFormatoNumero, ""}
        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
     
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
      
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
       
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numpag", "refpag", "importe"}
        Dim Nombres() As String = {"Cheque Nº", "Banco", "Importe"}
        Dim Anchos() As Long = {150, 100, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Lista de Cheques ")
        AsignaTXT(f.Apuntador, False)
        f = Nothing
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        Posicion = Me.BindingContext(ds, nTabla).Position
        If dt.Rows.Count > 0 Then
            NumeroCheque = dt.Rows(Posicion).Item("numpag").ToString
            BancoCheque = dg.Rows(Posicion).Cells(2).Value
            BancoDeposito = dt.Rows(Posicion).Item("codban").ToString
            Montocheque = CDbl(dt.Rows(Posicion).Item("importe").ToString)
            NumeroDeposito = dt.Rows(Posicion).Item("deposito").ToString
            NumeroCancelacion = dt.Rows(Posicion).Item("nummov").ToString
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
            NumeroCheque = dt.Rows(Posicion).Item("numpag").ToString
            BancoCheque = dg.Rows(Posicion).Cells(2).Value
            BancoDeposito = dt.Rows(Posicion).Item("codban").ToString
            Montocheque = CDbl(dt.Rows(Posicion).Item("importe").ToString)
            NumeroDeposito = dt.Rows(Posicion).Item("deposito").ToString
            NumeroCancelacion = dt.Rows(Posicion).Item("nummov").ToString
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

    Private Sub dg_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub
End Class