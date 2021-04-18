Imports MySql.Data.MySqlClient
Public Class jsControlArcTerritorioMovimiento
    Private Const sModulo As String = "Movimiento Territorio"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer

    Private cParentCod As String
    Private iLevel As Integer
    Private Codigo As Integer
    Private tv As New TreeView
    Private n_node As String

    Private aNivel() As String = {"País", "Estado/Provincia", "Municipio/Región", "Parroquia/Comunidad", "Ciudad/Pueblo/Aldea", "Barrio/Urbanización/Sector"}

    Public Property NodoSeleccionado() As String
        Get
            Return n_node
        End Get
        Set(ByVal value As String)
            n_node = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, _
                       ByVal dt As DataTable, _
                       ByVal cCodigoPadre As String, ByVal iNivel As Integer, ByVal tVV As TreeView)

        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        cParentCod = cCodigoPadre
        iLevel = iNivel
        tv = tVV

        IniciarTXT()
        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()

        txtNivel.Text = CStr(iLevel + 1) + ". " + aNivel(iLevel + 1)
        txtDescripcion.Text = ""
        txtMontoFlete.Text = ft.FormatoNumero(0.0)
        ft.habilitarObjetos(False, True, txtNivel)

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, _
                      ByVal dt As DataTable, ByVal cCodigoPadre As String, ByVal iNivel As Integer, ByVal tVV As TreeView)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        Dim aCampos() As String = {"codigo"}
        Dim aCadenas() As String = {cCodigoPadre}
        cParentCod = qFoundAndSign(MyConn, lblInfo, "jsconcatter", aCampos, aCadenas, "antecesor")
        iLevel = iNivel
        tv = tVV
        NodoSeleccionado = cCodigoPadre
        AsignarTXT()

        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT()

        Dim aFld() As String = {"codigo", "id_emp"}
        Dim aStr() As String = {NodoSeleccionado, jytsistema.WorkID}
        txtDescripcion.Text = qFoundAndSign(MyConn, lblInfo, "jsconcatter", aFld, aStr, "nombre")
        txtMontoFlete.Text = ft.FormatoNumero(qFoundAndSign(MyConn, lblInfo, "jsconcatter", aFld, aStr, "montoflete"))

        txtNivel.Text = CStr(iLevel) + ". " + aNivel(iLevel)
        ft.habilitarObjetos(False, True, txtNivel)

    End Sub

    Private Sub jsControlArcTerritorioMovimiento_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsControlArcTerritorioMovimiento_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Private Sub txtDescripcion_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescripcion.GotFocus
        Select Case sender.name
            Case "txtDescripcion"
                ft.mensajeEtiqueta(lblInfo, "Indique el nombre o descripción ...", Transportables.TipoMensaje.iInfo)
            Case "txtMontoFlete"
                ft.mensajeEtiqueta(lblInfo, "Indique el monto de flete ...", Transportables.TipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtDescripcion.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre o descripción válida ...")
            ft.enfocarTexto(txtDescripcion)
            Exit Function
        End If

        Validado = True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then

            Dim Insertar As Boolean = False
            Dim tn As New TreeNode
            If i_modo = movimiento.iAgregar Then
                Insertar = True
            End If

            InsertEditCONTROLTerritorio(MyConn, lblInfo, Insertar, _
                                         CInt(tv.SelectedNode.Tag), txtDescripcion.Text, CDbl(txtMontoFlete.Text), cParentCod, iLevel + 1)


            If Insertar Then

                NodoSeleccionado = ft.DevuelveScalarCadena(MyConn, "SELECT LAST_INSERT_ID() FROM jsconcatter")

                tn.Tag = NodoSeleccionado
                tn.Text = txtDescripcion.Text
                tv.SelectedNode.Nodes.Add(tn)
            Else
                tv.SelectedNode.Text = txtDescripcion.Text
                tn = tv.SelectedNode

            End If


            ActualizarMontoEnHijos(MyConn, tn.Tag, CDbl(txtMontoFlete.Text))

            tv.SelectedNode = tn

            Me.Close()

        End If
    End Sub
    Private Sub ActualizarMontoEnHijos(MyConn As MySqlConnection, nCodigoNodoPadre As Integer, Monto As Double)

        Dim dss As New DataSet
        Dim dtHijos As DataTable
        Dim nTableHijos As String = "tbl" & ft.NumeroAleatorio(100000)

        dss = DataSetRequery(dss, " select * from jsconcatter where antecesor = '" & nCodigoNodoPadre & "' ", MyConn, nTableHijos, lblInfo)
        dtHijos = dss.Tables(nTableHijos)

        For Each nRow As DataRow In dtHijos.Rows
            With nRow
                ft.Ejecutar_strSQL(MyConn, " update jsconcatter set montoflete = " & Monto & " where codigo = " & .Item("codigo") & " ")
                ActualizarMontoEnHijos(MyConn, .Item("codigo"), Monto)
            End With
        Next

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class