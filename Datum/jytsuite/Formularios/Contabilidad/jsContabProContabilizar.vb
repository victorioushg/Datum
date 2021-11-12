Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsContabProContabilizar
    Private Const sModulo As String = "Proceso de contabilización"
    Private Const nTabla As String = "proasicon"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private tbl As String = ""
    Private seleccionarTodo As Boolean = False

    Public Sub Cargar(ByVal MyCon As MySqlConnection)
        myConn = MyCon
        Me.Tag = sModulo

        Dim dates As SfDateTimeEdit() = {txtFechaDesde, txtFechaHasta}
        SetSizeDateObjects(dates)

        tbl = "tbl" & ft.NumeroAleatorio(100000)

        lblLeyenda.Text = " Mediante este proceso se construyen los asientos diferidos a partir de las plantillas " + vbCr +
                " de asientos y las reglas de contabilización.  " + vbCr +
                " Para proceder debe seleccionar los asientos plantillas deseados y las fechas inicial y final de contabilización, " + vbCr +
                " Estos asientos diferidos se construirán de forma diaria ... "

        ft.mensajeEtiqueta(lblInfo, " ... ", Transportables.tipoMensaje.iAyuda)
        txtFechaDesde.Value = jytsistema.sFechadeTrabajo
        txtFechaHasta.Value = jytsistema.sFechadeTrabajo
        IniciarAsientosDefinidos()
        Me.Show()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub IniciarAsientosDefinidos()


        Dim aFields() As String = {"sel.entero.1.0", "asiento.cadena.5.0", "descripcion.cadena.50.0", "fecha_ult_con.fecha.0.0", "inicio_ult_con.fecha.0.0",
                                   "fin_ult_con.fecha.0.0", "id_emp.cadena.2.0"}

        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tbl, aFields)

        ft.Ejecutar_strSQL(myConn, " insert into " & tbl _
            & " SELECT 0 sel, a.asiento, a.descripcion, a.fecha_ult_con, a.inicio_ult_con, a.fin_ult_con, a.id_emp  " _
            & " FROM jscotencdef a " _
            & " WHERE " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY " _
            & " a.asiento ")


        ds = DataSetRequery(ds, " select * from " & tbl, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        CargarListaDesdeAsientosDefinidos(dg, dt)
        dg.ReadOnly = False
        For Each col As DataGridViewColumn In dg.Columns
            If col.Index > 0 Then col.ReadOnly = True
        Next

    End Sub

    Private Sub jsContabProContabilizar_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt.Dispose()
    End Sub

    Private Sub jsContabProProcesaAsientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If CDate(txtFechaDesde.Text) > CDate(txtFechaHasta.Text) Then
            ft.mensajeCritico("Debe indicar una rango de fechas válido...")
            Exit Sub
        End If

        ProcesarAsientos()
        ' Me.Close()
    End Sub
    Private Sub ProcesarAsientos()


        Dim Fecha As Date = CDate(txtFechaDesde.Text)
        While Fecha <= CDate(txtFechaHasta.Text)
            Dim iCont As Integer = 0
            For Each selectedItem As DataGridViewRow In dg.Rows
                iCont += 1
                If CBool(selectedItem.Cells(0).Value) Then

                    Dim nPlantillaOrigen As String = selectedItem.Cells(1).Value

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, iCont / dg.RowCount * 100,
                                                  " Asiento : " + selectedItem.Cells(1).Value + " Fecha : " + ft.FormatoFecha(Fecha))

                    Dim Asiento As String = ft.DevuelveScalarCadena(myConn, " select asiento from jscotencasi " _
                                                                    & " where " _
                                                                    & " fechasi = '" & ft.FormatoFechaMySQL(Fecha) & "' and " _
                                                                    & " plantilla_origen = '" & nPlantillaOrigen & "' and " _
                                                                    & " id_emp = '" & jytsistema.WorkID & "' ")
                    If Asiento = "" Then

                        Contabilizar_Plantilla(myConn, lblInfo, ds, Fecha, nPlantillaOrigen, selectedItem.Cells(2).Value,
                                               CDate(txtFechaDesde.Text), CDate(txtFechaHasta.Text), lblProgreso, ProgressBar1)


                    Else

                        ft.Ejecutar_strSQL(myConn, " delete from jscotencasi " _
                                                           & " where " _
                                                           & " asiento = '" & Asiento & "' and id_emp = '" & jytsistema.WorkID & "' ")

                        ft.Ejecutar_strSQL(myConn, " delete from jscotrenasi " _
                                       & " where " _
                                       & " asiento = '" & Asiento & "' and id_emp = '" & jytsistema.WorkID & "' ")

                        Contabilizar_Plantilla(myConn, lblInfo, ds, Fecha, nPlantillaOrigen, selectedItem.Cells(2).Value,
                                               CDate(txtFechaDesde.Text), CDate(txtFechaHasta.Text), lblProgreso, ProgressBar1, Asiento)


                    End If

                    ft.Ejecutar_strSQL(myConn, " update jscotencdef set fecha_ult_con = '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', " _
                                                          & " inicio_ult_con = '" & ft.FormatoFechaMySQL(CDate(txtFechaDesde.Text)) & "', " _
                                                          & " fin_ult_con = '" & ft.FormatoFechaMySQL(CDate(txtFechaHasta.Text)) & "' " _
                                                          & " where " _
                                                          & " asiento = '" & selectedItem.Cells(1).Value & "' and " _
                                                          & " id_emp = '" & jytsistema.WorkID & "'  ")

                End If
            Next
            Fecha = DateAdd(DateInterval.Day, 1, Fecha)
        End While

        ft.mensajeInformativo("PROCESO CULMINADO ...")
        lblProgreso.Text = ""
        ProgressBar1.Value = 0
        IniciarAsientosDefinidos()

    End Sub

    Private Sub txtFechaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFechaDesde.ValueChanged
        txtFechaHasta.Value = txtFechaDesde.Value
    End Sub

    Private Sub dg_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick
        If e.ColumnIndex = 0 Then
            dt.Rows(e.RowIndex).Item(0) = Not CBool(dt.Rows(e.RowIndex).Item(0).ToString)
        End If

    End Sub

    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
       dg.ColumnHeaderMouseClick

        If e.ColumnIndex = 0 Then
            For Each nRow As DataGridViewRow In dg.Rows
                nRow.Cells(0).Value = Not seleccionarTodo
            Next
            seleccionarTodo = Not seleccionarTodo
        End If


    End Sub

    Private Sub dg_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellValidated
        If dg.CurrentCell.ColumnIndex = 0 Then

            ft.Ejecutar_strSQL(myConn, " update  " & tbl & " set sel  = " & CInt(dg.CurrentCell.Value) & " " _
                            & " where " _
                            & " asiento = '" & CStr(dg.CurrentRow.Cells(1).Value) & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

        End If
    End Sub

End Class