Imports MySql.Data.MySqlClient
Public Class jsMerArcServiciosA
    Private Const sModulo As String = "Listado de servicios"
    Private Const lRegion As String = ""
    Private Const nTabla As String = "tblListaServicios"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private strSQL As String = ""

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
    Public Sub Cargar(ByVal Mycon As MySqlConnection)

        myConn = Mycon

        'strSQL = "select codser, desser, tipoiva, precio, elt(tiposervicio +1, 'ISLR','NORMAL') tiposervicio, CODCON from jsmercatser where id_emp = '" & jytsistema.WorkID & "' order by codser "
        strSQL = "select * from jsmercatser where id_emp = '" & jytsistema.WorkID & "' order by codser "

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        BindingSource1.DataSource = dt

        IniciarGrilla()
        AsignarTooltips()
        If dt.Rows.Count > 0 Then Posicion = 0
        AsignaTXT(Posicion, True)
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)
        rezicer()
        Me.ShowDialog()


    End Sub
    Private Sub jsMerArcServiciosA_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt = Nothing
        ds = Nothing
    End Sub

    Private Sub jsMerArcServiciosA_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        FindField = "codser"
        lblBuscar.Text = "Código"

    End Sub
    Public Sub rezicer()

        With Me
            .Width = dg.Width + 150
            .Height = MenuBarra.Height + dg.Height + lblInfo.Height + 15
        End With

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnSeleccionar, btnPrimero, _
                          btnSiguiente, btnAnterior, btnUltimo, btnImprimir, btnSalir)

        MenuBarra.ImageList = ImageList1

        btnAgregar.Image = ImageList1.Images(0)
        btnEditar.Image = ImageList1.Images(1)
        btnEliminar.Image = ImageList1.Images(2)
        btnBuscar.Image = ImageList1.Images(3)
        btnSeleccionar.Image = ImageList1.Images(4)
        btnPrimero.Image = ImageList1.Images(6)
        btnAnterior.Image = ImageList1.Images(7)
        btnSiguiente.Image = ImageList1.Images(8)
        btnUltimo.Image = ImageList1.Images(9)
        btnImprimir.Image = ImageList1.Images(10)
        btnSalir.Image = ImageList1.Images(11)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean)
        dt = ft.MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, _
                               jytsistema.sUsuario, nRow, Actualiza)
    End Sub
    Private Sub IniciarGrilla()

        Dim aCampos() As String = {"codser.Código.80.I.", _
                                   "desser.Descripción.400.I.", _
                                   "tipoiva.IVA.30.C.", _
                                   "precio.Precio.110.D.Numero", _
                                   "tiposervicio.Tipo Servicio.70.C.", _
                                   "codcon.Código Contable.140.I."}
        ft.IniciarTablaPlus(dg, dt, aCampos)

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        BindingSource1.Filter = Nothing
        Dim f As New jsMerArcServiciosMovimientos
        Posicion = Me.BindingContext(ds, nTabla).Position
        f.Apuntador = Me.BindingContext(ds, nTabla).Position
        f.Agregar(myConn, ds, dt)
        AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        If dt.Rows.Count > 0 Then
            Dim f As New jsMerArcServiciosMovimientos
            f.Apuntador = Posicion
            f.Editar(myConn, ds, dt)
            AsignaTXT(f.Apuntador, True)
            f = Nothing
        End If
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar
        Dim Campos() As String = {"codser", "desser"}
        Dim Nombres() As String = {"Código", "Descripción"}
        Dim Anchos() As Integer = {100, 350}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Listado de servicios")
        Me.BindingContext(ds, nTabla).Position = f.Apuntador
        Posicion = Me.BindingContext(ds, nTabla).Position
        AsignaTXT(Posicion, False)
        f = Nothing

    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click

        Posicion = Me.BindingContext(ds, nTabla).Position
        If dt.Rows.Count > 0 AndAlso dg.RowCount > 0 Then
            Seleccionado = "$" & dg.SelectedRows(0).Cells(0).Value.ToString ' dt.Rows(Posicion).Item("codser").ToString
            InsertarAuditoria(myConn, MovAud.iSeleccionar, Me.Text, "")
        End If
        Me.Close()

    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        If dg.RowCount > 0 Then
            Me.BindingContext(ds, nTabla).Position = 0
            Posicion = Me.BindingContext(ds, nTabla).Position
            AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
        End If
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        If dg.RowCount > 0 Then
            Me.BindingContext(ds, nTabla).Position -= 1
            Posicion = Me.BindingContext(ds, nTabla).Position
            AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
        End If
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        If dg.RowCount > 0 Then
            Me.BindingContext(ds, nTabla).Position += 1
            Posicion = Me.BindingContext(ds, nTabla).Position
            AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
        End If
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        If dg.RowCount > 0 Then
            Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
            Posicion = Me.BindingContext(ds, nTabla).Position
            AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
        End If
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        Posicion = Me.BindingContext(ds, nTabla).Position
        If dt.Rows.Count > 0 AndAlso dg.RowCount > 0 Then
            Seleccionado = "$" & dg.SelectedRows(0).Cells(0).Value.ToString 'dt.Rows(Posicion).Item("codser").ToString
            InsertarAuditoria(myConn, MovAud.iSeleccionar, Me.Text, "")
        End If
        Me.Close()
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.CellMouseClick

        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        Posicion = e.RowIndex
        AsignaTXT(e.RowIndex, False)

    End Sub

    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        BindingSource1.DataSource = dt
        If dt.Columns(FindField).DataType Is GetType(String) Then _
            BindingSource1.Filter = FindField & " like '%" & txtBuscar.Text & "%'"
        dg.DataSource = BindingSource1
    End Sub

    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.ColumnHeaderMouseClick
        If e.ColumnIndex < 2 Then
            Dim aCam() As String = {"codser", "desser"}
            Dim aStr() As String = {"Código : ", "Descripción : "}
            FindField = dt.Columns(aCam(e.ColumnIndex)).ColumnName
            lblBuscar.Text = aStr(e.ColumnIndex)
            txtBuscar.Focus()
        End If
    End Sub

    Private Sub dg_CellFormatting(sender As Object, e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dg.CellFormatting
        Me.BindingContext(ds, nTabla).Position = e.RowIndex

        Dim aT() As String = {"ISLR", "NORMAL"}

        If e.ColumnIndex = 4 Then
            If e.Value Then
                e.Value = aT(1)
            Else
                e.Value = aT(0)
            End If
        End If
    End Sub

   
    Private Sub dg_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub
End Class