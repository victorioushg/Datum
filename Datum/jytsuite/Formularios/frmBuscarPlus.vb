Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Imports fTransport.Models
Imports Newtonsoft.Json
Imports System.ComponentModel
Imports Syncfusion.Data

Public Class frmBuscarPlus

    Private dtBuscar As New DataTable, dtOrigen As New DataTable
    Private ft As New Transportables
    Private FindField As String
    Public Property Id() As Long
    Public Sub Buscar(Of T)(ByVal dataList As List(Of T), fieldList As List(Of fTransport.Models.SfDataGridField), ByVal NombreBusqueda As String)
        Me.Text = NombreBusqueda
        ft.IniciarDataGridWithList(Of T)(dg, dataList, fieldList)
        Me.ShowDialog()
    End Sub
    Private Sub frmBuscar_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ft = Nothing
    End Sub
    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs)
        FindField = dg.Columns(e.ColumnIndex).MappingName
        txtBuscar.Focus()
        ft.enfocarTexto(txtBuscar)
    End Sub
    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        dg.SearchController.Search(sender.Text)
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub frmBuscar_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Select Case e.KeyCode
            Case Keys.Down, Keys.Up
                Id = dg.SelectedItem.Id
        End Select
    End Sub
    Private Sub frmBuscar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnOK.Image = ImageList1.Images(0)
        btnCancel.Image = ImageList1.Images(1)
    End Sub
    Private Sub dg_CellDoubleClick(sender As Object, e As Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs) Handles _
            dg.CellDoubleClick, btnOK.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        If (Not dg.SelectedItem Is Nothing) Then
            Id = dg.SelectedItem.Id
            Me.Close()
        End If
    End Sub
    Private Sub frmBuscar_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        txtBuscar.Focus()
    End Sub
End Class