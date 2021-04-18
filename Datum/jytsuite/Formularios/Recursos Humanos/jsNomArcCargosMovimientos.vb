Imports MySql.Data.MySqlClient
Public Class jsNomArcCargosMovimientos
    Private Const sModulo As String = "Movimiento Cargo nómina"

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

        txtDescripcion.Text = ""
        txtCodigoCargo.Text = ""
        txtSueldoBase.Text = ft.FormatoNumero(0.0)

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, _
                      ByVal dt As DataTable, ByVal cCodigoPadre As String, ByVal iNivel As Integer, ByVal tVV As TreeView)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        Dim aCampos() As String = {"codigo", "id_emp"}
        Dim aCadenas() As String = {cCodigoPadre, jytsistema.WorkID}
        cParentCod = qFoundAndSign(MyConn, lblInfo, "jsnomestcar", aCampos, aCadenas, "antecesor")
        iLevel = iNivel
        tv = tVV
        NodoSeleccionado = cCodigoPadre
        AsignarTXT()

        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT()

        Dim aFld() As String = {"codigo", "id_emp"}
        Dim aStr() As String = {NodoSeleccionado, jytsistema.WorkID}
        txtDescripcion.Text = qFoundAndSign(MyConn, lblInfo, "jsnomestcar", aFld, aStr, "nombre")
        txtCodigoCargo.Text = qFoundAndSign(MyConn, lblInfo, "jsnomestcar", aFld, aStr, "codigoempresa")
        txtSueldoBase.Text = ft.FormatoNumero(CDbl(qFoundAndSign(MyConn, lblInfo, "jsnomestcar", aFld, aStr, "sueldobase").ToString))

    End Sub

    Private Sub jsNomArcCargosMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsNomArcCargosMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Private Sub txtDescripcion_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescripcion.GotFocus, _
        txtCodigoCargo.GotFocus, txtSueldoBase.GotFocus
        Select Case sender.name
            Case "txtDescripcion"
                ft.mensajeEtiqueta(lblInfo, "Indique el nombre o descripción ...", Transportables.TipoMensaje.iInfo)
            Case "txtCodigoCargo"
                ft.mensajeEtiqueta(lblInfo, "Indique el código cargo o de estructura ...", Transportables.TipoMensaje.iInfo)
            Case "txtSueldoBase"
                ft.mensajeEtiqueta(lblInfo, "Indique el sueldo base del cargo en la estructura ...", Transportables.TipoMensaje.iInfo)
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

            InsertEditNOMINACargo(MyConn, lblInfo, Insertar, _
                                         CInt(tv.SelectedNode.Tag), txtDescripcion.Text, txtCodigoCargo.Text, _
                                        CDbl(txtSueldoBase.Text), cParentCod, iLevel + 1)

            If Insertar Then
                Dim dtCuenta As DataTable
                Dim ntblCuenta As String = "tblcuenta"

                dsLocal = DataSetRequery(dsLocal, "SELECT LAST_INSERT_ID() ultimo FROM jsnomestcar", MyConn, ntblCuenta, lblInfo)
                dtCuenta = dsLocal.Tables(ntblCuenta)

                NodoSeleccionado = dtCuenta.Rows(0).Item("ultimo").ToString

                tn.Tag = NodoSeleccionado
                tn.Text = txtDescripcion.Text
                tv.SelectedNode.Nodes.Add(tn)
            Else
                tv.SelectedNode.Text = txtDescripcion.Text
                tn = tv.SelectedNode

            End If

            tv.SelectedNode = tn

            Me.Close()

        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class