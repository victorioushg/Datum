Imports MySql.Data.MySqlClient

Public Class jsBanArcFormatoCheques

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private Const lRegion As String = ""
    Private nTablaTS As String = "tblFormato"
    Private strSQL As String = "select * from jsbancatfor where id_emp = '" & jytsistema.WorkID & "' order by formato "

    Private sModulo As String, Modulo As String

    Private bIncModEli As Boolean = False

    Dim aCampos() As String = {"formato.Código.50.C.", _
                               "descrip.Bancos.200.I.", _
                               "montotop.Monto Y.50.D.Entero", _
                               "montoleft.Monto X.50.D.Entero", _
                               "nombretop.Nombre Y.50.D.Entero", _
                               "nombreleft.Nombre X.50.D.Entero", _
                               "montoletratop.Letra Y.50.D.Entero", _
                               "montoletraleft.Letra X.50.D.Entero", _
                               "fechatop.Fecha Y.50.D.Entero", _
                               "fechaleft.Fecha X.50.D.Entero", _
                               "noendosabletop.Endoso Y.50.D.Entero", _
                               "noendosableleft.Endoso X.50.D.Entero"}

    Private n_Apuntador As Long
    Private n_Seleccion As String
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Property Seleccion() As String
        Get
            Return n_Seleccion
        End Get
        Set(ByVal value As String)
            n_Seleccion = value
        End Set
    End Property

    Public Sub Cargar(MyCon As MySqlConnection)

        Me.Tag = sModulo
        myConn = MyCon

        Try

            dt = ft.AbrirDataTable(ds, nTablaTS, myConn, strSQL)

            ft.IniciarTablaPlus(dg, dt, aCampos)

            If dt.Rows.Count > 0 Then n_Apuntador = 0

            AsignaTXT(n_Apuntador, False)
            AsignarTooltips()
            ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.tipoMensaje.iAyuda)

            Me.ShowDialog()


        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnSeleccionar, btnPrimero, _
                          btnSiguiente, btnAnterior, btnUltimo, btnImprimir, btnSalir)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean)
        dt = ft.MostrarFilaEnTabla(myConn, ds, nTablaTS, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, _
                               jytsistema.sUsuario, nRow, Actualiza)

    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        n_Apuntador = CInt(dt.Rows(Me.BindingContext(ds, nTablaTS).Position).Item("formato").ToString)
        Seleccion = dt.Rows(Me.BindingContext(ds, nTablaTS).Position).Item("formato").ToString
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Me.BindingContext(ds, nTablaTS).Position = 0
        AsignaTXT(Me.BindingContext(ds, nTablaTS).Position, False)
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Me.BindingContext(ds, nTablaTS).Position -= 1
        AsignaTXT(Me.BindingContext(ds, nTablaTS).Position, False)
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Me.BindingContext(ds, nTablaTS).Position += 1
        AsignaTXT(Me.BindingContext(ds, nTablaTS).Position, False)
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Me.BindingContext(ds, nTablaTS).Position = ds.Tables(nTablaTS).Rows.Count - 1
        AsignaTXT(Me.BindingContext(ds, nTablaTS).Position, False)
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        n_Apuntador = -1
        Me.Dispose()
    End Sub

    Private Sub jsBanFormatoCheques_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt = Nothing
        ds = Nothing
        ft = Nothing
    End Sub

    Private Sub jsBanFormatoCheques_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim f As New jsBanArcFormatoChequesMovimiento
        f.Agregar(myConn, ds, dt)
        ds = DataSetRequery(ds, strSQL, myConn, dt.TableName, lblInfo)
        If f.Apuntador >= 0 Then AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim f As New jsBanArcFormatoChequesMovimiento
        f.Apuntador = Me.BindingContext(ds, dt.TableName).Position
        f.Editar(myConn, ds, dt)
        ds = DataSetRequery(ds, strSQL, myConn, dt.TableName, lblInfo)
        AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        'If bIncModEli Then
        Apuntador = Me.BindingContext(ds, nTablaTS).Position
        EliminaFila()
        ' End If
    End Sub
    Private Sub EliminaFila()

        Dim aCamposDel() As String = {"formato", "id_emp"}
        Dim aStringsDel() As String = {dt.Rows(Apuntador).Item("formato"), jytsistema.WorkID}

        If ft.PreguntaEliminarRegistro = Windows.Forms.DialogResult.Yes Then
            AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTablaTS, "jsbancatfor", strSQL, aCamposDel, aStringsDel, _
                                     Apuntador, True), True)
        End If
    End Sub
    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        n_Apuntador = CInt(dt.Rows(Me.BindingContext(ds, nTablaTS).Position).Item("formato").ToString)
        Seleccion = dt.Rows(Me.BindingContext(ds, nTablaTS).Position).Item("formato").ToString
        Me.Close()
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaTS).Position = e.RowIndex
        AsignaTXT(e.RowIndex, False)
    End Sub

    Private Sub dg_RegionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.RegionChanged
        n_Apuntador = Me.BindingContext(ds, nTablaTS).Position
        Seleccion = dt.Rows(Me.BindingContext(ds, nTablaTS).Position).Item("formato").ToString
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        'Dim f As New frmBuscar
        'f.Buscar(dt, aCampos, aNombres, aAnchos, n_Apuntador, " Formnato de Cheques")
        'AsignaTXT(f.Apuntador, False)
        'f = Nothing
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cFormatoCheque, "FORMATO CHEQUE", , dt.Rows(Me.BindingContext(ds, nTablaTS).Position).Item("formato"))
        f = Nothing
    End Sub
    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp

        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(ds, nTablaTS).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTablaTS).Position, False)
            Case Keys.Up
                Me.BindingContext(ds, nTablaTS).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTablaTS).Position, False)
        End Select

    End Sub
End Class