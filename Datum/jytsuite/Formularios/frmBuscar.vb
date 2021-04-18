Imports MySql.Data.MySqlClient

Public Class frmBuscar
    Private dtBuscar As New DataTable, dtOrigen As New DataTable
    Private ft As New Transportables

    Private aCampos() As String
    Private FindField As String
    Private aNombres() As String
    Private aAnchos() As Integer
    Private n_Apuntador As Long
    Private BindingSource1 As New BindingSource
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Buscar(ByVal dt As DataTable, ByVal FieldArray() As String, _
        ByVal FieldNameArray() As String, ByVal FieldWidthArray() As Integer, _
        ByVal Posicion As Long, ByVal NombreBusqueda As String, Optional ByVal CampoInicialBusqueda As Integer = 0)

        aCampos = FieldArray
        aNombres = FieldNameArray
        aAnchos = FieldWidthArray
        dtBuscar = dt.Copy
        Apuntador = Posicion

        Me.Text = NombreBusqueda

        IniciarTabla(dg, dtBuscar, aCampos, aNombres, aAnchos)
        FindField = aCampos(CampoInicialBusqueda)
        Label1.Text = "Esta buscando por : " & aNombres(CampoInicialBusqueda)
        Me.ShowDialog()

    End Sub
    Public Sub BuscarPlus(ByVal dt As DataTable, ByVal FieldArray() As String, _
        ByVal Posicion As Long, ByVal NombreBusqueda As String, Optional ByVal CampoInicialBusqueda As Integer = 0)

        For Each aItem As String In FieldArray
            aCampos(ft.InArray(FieldArray, aItem)) = aItem.Split(".")(0)
            aNombres(ft.InArray(FieldArray, aItem)) = aItem.Split(".")(1)
            aAnchos(ft.InArray(FieldArray, aItem)) = aItem.Split(".")(2)
        Next

        dtBuscar = dt.Copy
        Apuntador = Posicion

        Me.Text = NombreBusqueda

        ft.IniciarTablaPlus(dg, dt, FieldArray)

        FindField = aCampos(CampoInicialBusqueda)

        Label1.Text = "Esta buscando por : " & aNombres(CampoInicialBusqueda)
        Me.ShowDialog()

    End Sub
    Private Sub frmBuscar_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ft = Nothing
        'dg.Columns.Clear()
    End Sub


    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.ColumnHeaderMouseClick

        Dim dgB As DataGridView = sender
        Label1.Text = "Esta buscando por : " & aNombres(e.ColumnIndex)
        FindField = dtBuscar.Columns(dgB.Columns(e.ColumnIndex).Name).ColumnName

        TextBox1.Focus()
        ft.enfocarTexto(TextBox1)

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        TextBox1.Text = Replace(TextBox1.Text, "'", "")
        BindingSource1.DataSource = dtBuscar
        If dtBuscar.Columns(FindField).DataType Is GetType(String) Then
            BindingSource1.Filter = FindField & " like '%" & TextBox1.Text & "%'"
        ElseIf dtBuscar.Columns(FindField).DataType Is GetType(Double) And TextBox1.Text <> "" Then
            BindingSource1.Filter = FindField & " >= " & ValorNumero(TextBox1.Text) - 5 & " and " & FindField & " <= " & ValorNumero(TextBox1.Text) + 5
        ElseIf dtBuscar.Columns(FindField).DataType Is GetType(MySql.Data.Types.MySqlDateTime) And TextBox1.Text.Length = 10 Then
            BindingSource1.Filter = " CONVERT(" & FindField & ", System.DateTime) = '" & CDate(TextBox1.Text).ToString & "' "
        End If
        dg.DataSource = BindingSource1

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Apuntador = ElQueBuscaEncuentra()
        Me.Close()

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


    Private Sub dg_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.DoubleClick

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Apuntador = ElQueBuscaEncuentra()
        Me.Close()

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
                    str += dg.Columns(0).Name & " = '" & dg.Rows(nCell.RowIndex).Cells(0).Value & "' and "
                End If
            Next
            str = Mid(str, 1, Len(str) - 4)
        End If

        BindingSource1.Filter = Nothing

        If dg.Rows.Count > 0 Then
            Dim row As DataRow = dtBuscar.Select(str)(0)
            ElQueBuscaEncuentra = dtBuscar.Rows.IndexOf(row)
        End If

    End Function

    Private Sub frmBuscar_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                dg.Focus()
                Me.BindingContext(dtBuscar.DataSet, dtBuscar.TableName).Position += 1
                Apuntador = Me.BindingContext(dtBuscar.DataSet, dtBuscar.TableName).Position
            Case Keys.Up
                dg.Focus()
                Me.BindingContext(dtBuscar.DataSet, dtBuscar.TableName).Position -= 1
                Apuntador = Me.BindingContext(dtBuscar.DataSet, dtBuscar.TableName).Position
        End Select
    End Sub

    Private Sub frmBuscar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnOK.Image = ImageList1.Images(0)
        btnCancel.Image = ImageList1.Images(1)
    End Sub

    Private Sub frmBuscar_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        TextBox1.Focus()
    End Sub

End Class