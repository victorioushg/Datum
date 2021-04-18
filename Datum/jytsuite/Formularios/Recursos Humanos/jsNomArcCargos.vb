Imports MySql.Data.MySqlClient
Public Class jsNomArcCargos
    Private Const sModulo As String = "Estructura de cargos"
    Private Const nTabla As String = "tblArbol"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private strSQL As String = "select * from jsnomestcar where id_emp = '" & jytsistema.WorkID & "' order by codigo "

    Private Posicion As Long
    Private n_CodigoCargo As String
    Public Property CodigoCargo() As String
        Get
            Return n_CodigoCargo
        End Get
        Set(ByVal value As String)
            n_CodigoCargo = value
        End Set
    End Property
    Public Sub Cargar(ByVal Mycon As MySqlConnection, ByVal TipoCarga As Integer)

        ' 0 = show() ; 1 = showdialog()

        myConn = Mycon
        AsignarTooltips()

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        IniciarNodos()

        If CodigoCargo <> "" Then NodoInicial()

        ft.mensajeEtiqueta(lblInfo, "Agregue, modifique ó elimine cualquier nodo en la estructura de cargos...", Transportables.TipoMensaje.iAyuda)

        If TipoCarga = 0 Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If


    End Sub
    Private Sub NodoInicial()

        Dim tni As TreeNode
        For Each tni In tv.Nodes
            NodoInicio(tni)
        Next

    End Sub
    Private Sub NodoInicio(ByVal n As TreeNode)

        Dim aNode As TreeNode
        For Each aNode In n.Nodes
            If aNode.Tag = CodigoCargo Then tv.SelectedNode = aNode
            NodoInicio(aNode)
        Next

    End Sub
    Private Sub IniciarNodos()

        tv.Nodes.Clear()
        CrearNodosDelPadre(0, Nothing)
        tv.ExpandAll()

    End Sub
    Private Sub CrearNodosDelPadre(ByVal indicePadre As Integer, ByVal nodePadre As TreeNode)

        Dim dataViewHijos As DataView

        ' Crear un DataView con los Nodos que dependen del Nodo padre pasado como parámetro.
        dataViewHijos = New DataView(ds.Tables(nTabla))
        dataViewHijos.RowFilter = ds.Tables(nTabla).Columns("antecesor").ColumnName + " = " + indicePadre.ToString()

        ' Agregar al TreeView los nodos Hijos que se han obtenido en el DataView.
        For Each dataRowCurrent As DataRowView In dataViewHijos

            Dim nuevoNodo As New TreeNode
            nuevoNodo.Text = dataRowCurrent("nombre").ToString().Trim()
            nuevoNodo.Tag = dataRowCurrent("codigo").ToString().Trim()

            ' si el parámetro nodoPadre es nulo es porque es la primera llamada, son los Nodos
            ' del primer nivel que no dependen de otro nodo.
            If nodePadre Is Nothing Then
                tv.Nodes.Add(nuevoNodo)
            Else
                ' se añade el nuevo nodo al nodo padre.
                nodePadre.Nodes.Add(nuevoNodo)
            End If

            ' Llamada recurrente al mismo método para agregar los Hijos del Nodo recién agregado.
            CrearNodosDelPadre(Int32.Parse(dataRowCurrent("codigo").ToString()), nuevoNodo)
        Next dataRowCurrent

    End Sub

    Private Sub jsNomArcGrupos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt = Nothing
        ds = Nothing
        ft = Nothing
    End Sub

    Private Sub jsNomArcCargos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub AsignarTooltips()

        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo registro")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnSeleccionar, "<B>Seleccionar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B>")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        If Not tv.SelectedNode Is Nothing Then
            Dim f As New jsNomArcCargosMovimientos
            Dim aFld() As String = {"codigo", "id_emp"}
            Dim aStr() As String = {tv.SelectedNode.Tag, jytsistema.WorkID}
            f.Agregar(myConn, ds, dt, tv.SelectedNode.Tag, CInt(qFoundAndSign(myConn, lblInfo, "jsnomestcar", aFld, aStr, "nivel")), tv)
            f = Nothing
        Else
            ft.mensajeAdvertencia("Debe seleccionar un nodo para agregar ...")
        End If
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        If Not tv.SelectedNode Is Nothing Then
            Dim f As New jsNomArcCargosMovimientos
            Dim aFld() As String = {"codigo", "id_emp"}
            Dim aStr() As String = {tv.SelectedNode.Tag, jytsistema.WorkID}
            f.Editar(myConn, ds, dt, tv.SelectedNode.Tag, CInt(qFoundAndSign(myConn, lblInfo, "jsnomestcar", aFld, aStr, "nivel")), tv)
            NodoSeleccionado(tv.SelectedNode)
            f = Nothing
        Else
            ft.mensajeAdvertencia("Debe seleccionar un nodo para modificar ...")
        End If
    End Sub
    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If Not tv.SelectedNode Is Nothing Then
            If tv.SelectedNode.Nodes.Count > 0 Then
            Else
                ft.Ejecutar_strSQL(myconn, " delete from jsnomestcar where codigo = " & tv.SelectedNode.Tag & " and id_emp = '" & jytsistema.WorkID & "' ")
                tv.Nodes.Remove(tv.SelectedNode)
            End If
        End If
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub tv_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tv.AfterSelect
        NodoSeleccionado(e.Node)
    End Sub
    Private Sub NodoSeleccionado(ByVal tn As TreeNode)
        Dim afld() As String = {"codigo", "id_emp"}
        Dim aStr() As String = {tn.Tag, jytsistema.WorkID}
        txtEstructura.Text = qFoundAndSign(myConn, lblInfo, "jsnomestcar", afld, aStr, "codigoempresa")
        CodigoCargo = qFoundAndSign(myConn, lblInfo, "jsnomestcar", afld, aStr, "codigo")
        txtSueldo.Text = ft.FormatoNumero(CDbl(qFoundAndSign(myConn, lblInfo, "jsnomestcar", afld, aStr, "sueldobase").ToString))
        Dim aCadena() As String = Split(tn.FullPath, "\")
        Dim kCont As Integer
        LBL.Text = ""
        For kCont = 0 To UBound(aCadena)
            LBL.Text = LBL.Text + aCadena(kCont) + vbCr + Space(kCont * 2 + 3)
        Next
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        CodigoCargo = tv.SelectedNode.Tag
        Me.Close()
    End Sub

    Private Sub tv_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tv.DoubleClick
        CodigoCargo = tv.SelectedNode.Tag
        Dim Nivel As Integer = ft.DevuelveScalarEntero(myConn, " select nivel from jsnomestcar where codigo = '" & CodigoCargo & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim Antecesor As String = ft.DevuelveScalarCadena(myConn, " select antecesor from jsnomestcar where codigo = '" & CodigoCargo & "' and id_emp = '" & jytsistema.WorkID & "' ")
        While Nivel <> 0
            CodigoCargo = ft.DevuelveScalarCadena(myConn, " select codigo from jsnomestcar where codigo = '" & Antecesor & "' and id_emp = '" & jytsistema.WorkID & "' ") & "." & CodigoCargo
            Nivel = ft.DevuelveScalarEntero(myConn, " select nivel from jsnomestcar where codigo = '" & Antecesor & "' and id_emp = '" & jytsistema.WorkID & "' ")
            Antecesor = ft.DevuelveScalarCadena(myConn, " select antecesor from jsnomestcar where codigo = '" & Antecesor & "' and id_emp = '" & jytsistema.WorkID & "' ")
        End While

        Me.Close()
    End Sub

    Private Sub btnImprimir_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

    End Sub
End Class