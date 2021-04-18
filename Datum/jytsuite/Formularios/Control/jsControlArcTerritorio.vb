Imports MySql.Data.MySqlClient
Public Class jsControlArcTerritorio
    Private Const sModulo As String = "Territorios"
    Private Const nTabla As String = "tblArbolTerritorio"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private strSQL As String = "select * from jsconcatter order by nombre "

    Private Posicion As Long
    Private n_CodigoCargo As String
    Public Property CodigoTerritorio() As String
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

        If CodigoTerritorio <> "" Then NodoInicial()

        ft.mensajeEtiqueta(lblInfo, "Agregue, modifique ó elimine cualquier nodo en la estructura del territorio...", Transportables.TipoMensaje.iAyuda)

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
            If aNode.Tag = CodigoTerritorio Then tv.SelectedNode = aNode
            NodoInicio(aNode)
        Next

    End Sub
    Private Sub IniciarNodos()

        tv.Nodes.Clear()
        CrearNodosDelPadre(0, Nothing)

        '  tv.ExpandAll()

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

            'Llamada recurrente al mismo método para agregar los Hijos del Nodo recién agregado.
            'CrearNodosDelPadre(Int32.Parse(dataRowCurrent("codigo").ToString()), nuevoNodo)

        Next dataRowCurrent

    End Sub

    Private Sub jsNomArcGrupos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt = Nothing
        ds = Nothing
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

            Dim f As New jsControlArcTerritorioMovimiento
            Dim aFld() As String = {"codigo"}
            Dim aStr() As String = {tv.SelectedNode.Tag}
            Dim iNivel As Integer = CInt(qFoundAndSign(myConn, lblInfo, "jsconcatter", aFld, aStr, "tipo"))
            If iNivel < 5 Then
                f.Agregar(myConn, ds, dt, tv.SelectedNode.Tag, iNivel, tv)
                f = Nothing
            Else
                ft.mensajeAdvertencia("No puede incluir un subnivel inferior...")
            End If
        Else
            ft.mensajeAdvertencia("Debe seleccionar un nodo para agregar ...")
        End If
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        If Not tv.SelectedNode Is Nothing Then
            Dim f As New jsControlArcTerritorioMovimiento
            Dim aFld() As String = {"codigo"}
            Dim aStr() As String = {tv.SelectedNode.Tag}
            f.Editar(myConn, ds, dt, tv.SelectedNode.Tag, CInt(qFoundAndSign(myConn, lblInfo, "jsconcatter", aFld, aStr, "tipo")), tv)
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
                ft.Ejecutar_strSQL(myconn, " delete from jsconcatter where codigo = " & tv.SelectedNode.Tag & "  ")
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

        If e.Node.Nodes.Count = 0 Then CrearNodosDelPadre(e.Node.Tag, e.Node)
        NodoSeleccionado(e.Node)

    End Sub
    Private Sub NodoSeleccionado(ByVal tn As TreeNode)
        Dim aNivel() As String = {"País", "Estado/Provincia", "Municipio/Región", "Parroquia/Comunidad", "Ciudad/Pueblo/Aldea", "Barrio/Urbanización/Sector"}
        Dim afld() As String = {"codigo", "id_emp"}
        Dim aStr() As String = {tn.Tag, jytsistema.WorkID}
        txtEstructura.Text = Territorio(tv)
        CodigoTerritorio = qFoundAndSign(myConn, lblInfo, "jsconcatter", afld, aStr, "codigo")
        Dim aCadena() As String = Split(tn.FullPath, "\")
        Dim kCont As Integer
        LBL.Text = ""
        For kCont = 0 To UBound(aCadena)
            LBL.Text = LBL.Text + aNivel(kCont) + ": " + aCadena(kCont) + vbCr
        Next
    End Sub

   

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click, _
        tv.DoubleClick
        CodigoTerritorio = Territorio(tv)
        Me.Close()
    End Sub

    Private Function Territorio(ByVal tv As TreeView) As String
        Territorio = tv.SelectedNode.Tag
        Dim Nivel As Integer = ft.DevuelveScalarEntero(myConn, " select tipo from jsconcatter where codigo = '" & Territorio & "'  ")
        Dim Antecesor As String = ft.DevuelveScalarCadena(myConn, " select antecesor from jsconcatter where codigo = '" & Territorio & "'  ")
        While Nivel <> 0
            Territorio = ft.DevuelveScalarCadena(myConn, " select codigo from jsconcatter where codigo = '" & Antecesor & "'  ") & "." & Territorio
            Nivel = ft.DevuelveScalarEntero(myConn, " select tipo from jsconcatter where codigo = '" & Antecesor & "'  ")
            Antecesor = ft.DevuelveScalarCadena(myConn, " select antecesor from jsconcatter where codigo = '" & Antecesor & "'  ")
        End While
    End Function

  
End Class