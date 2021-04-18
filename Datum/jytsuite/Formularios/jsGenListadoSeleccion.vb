Imports MySql.Data.MySqlClient
Imports System.ComponentModel
Public Class jsGenListadoSeleccion
    Private Const sModulo As String = "Listado selección"
    Private Const nTabla As String = "tblSeleccion"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private strSQL As String = ""
    Private tblListado As String = ""
    Private Posicion As Long
    Private FindField As String

    Private aaNombres() As String
    Private aaCampos() As String

    Private n_Seleccion() As String = {}
    Private BindingSource1 As New BindingSource


    Public Property Seleccion() As String()
        Get
            Return n_Seleccion
        End Get
        Set(ByVal value As String())
            n_Seleccion = value
        End Set
    End Property
    Public Sub Cargar(ByVal Mycon As MySqlConnection, ByVal dsLocal As DataSet, ByVal Titulo As String, ByVal strSQL As String, _
                      ByVal aFields() As String,
                      ByVal aNombres() As String, ByVal aCampos() As String, ByVal aAnchos() As Integer, _
                      ByVal aAlineacion() As HorizontalAlignment, ByVal aFormato() As String)

        myConn = Mycon
        ds = dsLocal
        tblListado = "tbllistado" & ft.NumeroAleatorio(100000)

        Me.Text = Titulo
        aaNombres = aNombres
        aaCampos = aCampos

        IniciarListaSeleccion(myConn, strSQL, aFields, aNombres, aCampos, aAnchos, aAlineacion, aFormato)
        ft.habilitarObjetos(False, True, txtDocSel)

        Me.ShowDialog()

    End Sub
    Private Sub IniciarListaSeleccion(ByVal MyConn As MySqlConnection, ByVal strSQL As String, ByVal aFields() As String, ByVal aNombres() As String, _
                                      ByVal aCampos() As String, ByVal aAnchos() As Integer, ByVal aAlineacion() As HorizontalAlignment, _
                                      ByVal aFormato() As String)

        '1. CREAR TABLA SELECCION
        CrearTabla(MyConn, lblInfo, jytsistema.WorkDataBase, True, tblListado, aFields)

        '2. INSERTAR EN ESA TABLA LA SELECCION - OJO LA STRSQL DEBE VENIR CON BIT INICIAL PARA SELECCCION
        ft.Ejecutar_strSQL(myconn, " insert into " & tblListado _
                    & strSQL)

        ds = DataSetRequery(ds, " SELECT * FROM " & tblListado & " ", MyConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        IniciarTablaSeleccion(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormato, , True)
        dg.ReadOnly = False
        For Each col As DataGridViewColumn In dg.Columns
            If col.Index > 0 Then col.ReadOnly = True
        Next

        FindField = aCampos(1)
        Label1.Text = "Esta buscando por : " & aNombres(1)

        IniciarSeleccion()



    End Sub

    Private Sub IniciarSeleccion()

        If Seleccion.Length > 0 Then
            For Each nRow As DataGridViewRow In dg.Rows
                If ft.InArray(Seleccion, nRow.Cells(1).Value.ToString) >= 0 Then
                    nRow.Cells(0).Value = True
                End If
            Next
        End If

        CalculaTotales()

    End Sub
    Private Sub CalculaTotales()


        Dim iSel As Integer = 0
        Seleccion = New String() {}
        For Each nRow As DataGridViewRow In dg.Rows  ' selectedItem As DataGridViewRow In dg.Rows
            If CBool(nRow.Cells(0).Value) Then
                iSel += 1
                ReDim Preserve Seleccion(UBound(Seleccion) + 1)
                Seleccion(UBound(Seleccion)) = nRow.Cells(1).Value.ToString
            End If
        Next

        'For Each nRow As DataRow In dt.Rows  ' selectedItem As DataGridViewRow In dg.Rows
        '    If CBool(nRow.Item("Sel")) Then
        '        iSel += 1
        '        ReDim Preserve Seleccion(UBound(Seleccion) + 1)
        '        Seleccion(UBound(Seleccion)) = nRow.Item(1).ToString
        '    End If
        'Next

        txtDocSel.Text = ft.FormatoEntero(iSel)

    End Sub
    Private Sub jsGenListadoSeleccion_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt = Nothing
        ds = Nothing
    End Sub

    Private Sub jsGenListadoSeleccion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub dg_CellMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.CellMouseUp
        dg.CommitEdit(DataGridViewDataErrorContexts.Commit)
        CalculaTotales()
    End Sub


    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        BindingSource1.Filter = Nothing
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        BindingSource1.Filter = Nothing
        Seleccion = New String() {}
        Me.Close()
    End Sub

    Public Sub CreateGraphicsColumn()
        Dim treeIcon As New Icon(Me.GetType(), "tree.ico")
        Dim iconColumn As New DataGridViewImageColumn()
        With iconColumn
            .Image = treeIcon.ToBitmap()
        End With
        dg.Columns.Insert(0, iconColumn)
    End Sub

    Private Sub dg_ColumnHeaderMouseClick(sender As Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.ColumnHeaderMouseClick
        If e.ColumnIndex = 0 Then
            Dim habilitado As Boolean
            If dg.Columns(0).HeaderText = "v" Then
                dg.Columns(0).HeaderText = ""
                habilitado = False
            Else
                dg.Columns(0).HeaderText = "v"
                habilitado = True
            End If

            For Each nRow As DataGridViewRow In dg.Rows
                nRow.Cells(0).Value = habilitado
            Next

        Else

            Dim dgB As DataGridView = sender
            Label1.Text = "Esta buscando por : " & aaNombres(e.ColumnIndex)
            FindField = dt.Columns(dgB.Columns(e.ColumnIndex).Name).ColumnName

            dgB.Sort(dgB.Columns(e.ColumnIndex), ListSortDirection.Ascending)
        
            TextBox1.Focus()
            ft.enfocarTexto(TextBox1)

        End If
    End Sub
    Private Sub dg_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellValidated
        If dg.CurrentCell.ColumnIndex = 0 Then

            ft.Ejecutar_strSQL(myconn, " UPDATE  " & tblListado & " set SEL  = " & CInt(dg.CurrentCell.Value) & " " _
                                & " WHERE " _
                                & " CODIGO = '" & dg.CurrentRow.Cells(1).Value.ToString & "'  ")


        End If
    End Sub


    Private Function ElQueBuscaEncuentra() As Long

        Dim str As String = ""
        If dg.SelectedRows.Count > 0 Then
            For Each nCell As DataGridViewCell In dg.SelectedRows(0).Cells
                If TypeOf (nCell.Value) Is String AndAlso nCell.Value.ToString <> "" Then
                    str += dg.Columns(nCell.ColumnIndex).Name & " = '" & nCell.Value.ToString.Replace("'", "''") & "' and "
                ElseIf TypeOf (nCell.Value) Is Double AndAlso nCell.Value.ToString <> "" Then
                    str += dg.Columns(nCell.ColumnIndex).Name & " = " & nCell.Value.ToString & " and "
                Else
                End If
            Next
            str = Mid(str, 1, Len(str) - 4)
        End If

        BindingSource1.Filter = Nothing

        If dg.Rows.Count > 0 Then
            Dim row As DataRow = dt.Select(str)(0)
            ElQueBuscaEncuentra = dt.Rows.IndexOf(row)
        End If

    End Function

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged

        TextBox1.Text = Replace(TextBox1.Text, "'", "")

        BindingSource1.DataSource = dt
        If dt.Columns(FindField).DataType Is GetType(String) Then
            BindingSource1.Filter = FindField & " like '%" & TextBox1.Text & "%'"
        ElseIf dt.Columns(FindField).DataType Is GetType(Double) And TextBox1.Text <> "" Then
            BindingSource1.Filter = FindField & " >= " & ValorNumero(TextBox1.Text) - 5 & " and " & FindField & " <= " & ValorNumero(TextBox1.Text) + 5
        ElseIf dt.Columns(FindField).DataType Is GetType(MySql.Data.Types.MySqlDateTime) And TextBox1.Text.Length = 10 Then
            BindingSource1.Filter = " CONVERT(" & FindField & ", System.DateTime) = '" & CDate(TextBox1.Text).ToString & "' "
        End If
        dg.DataSource = BindingSource1

    End Sub
   Private Sub dg_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick
        If e.ColumnIndex = 0 Then dt.Rows(e.RowIndex).Item(0) = Not CBool(dt.Rows(e.RowIndex).Item(0).ToString)
    End Sub
End Class