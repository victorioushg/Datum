Imports MySql.Data.MySqlClient
Public Class jsContabProReversaraAsientos
    Private Const sModulo As String = "Reversar Asientos Contables"
    Private Const lRegion As String = "RibbonButton299"
    Private Const nTabla As String = "tbl_reversaasientos"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private documentos_procesados As String = ""
    Private seleccionarTodo As Boolean = False

    Private BindingSource1 As New BindingSource
    Private FindField As String
    Dim aNombres() As String = {}
    Dim aCampos() As String = {}
   
    Public Sub Cargar(ByVal MyCon As MySqlConnection)
        myConn = MyCon
        Me.Tag = sModulo
        lblLeyenda.Text = " Mediante este proceso se reversan los asientos contables actuales a asientos " + vbCr + _
                " contables diferidos.  " + vbCr + _
                "  "

        MensajeEtiqueta(lblInfo, "Seleccione asientos actuales ... ", TipoMensaje.iAyuda)
        CargarAsientosActuales()

        Me.Show()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub CargarAsientosActuales()

        IniciarProgreso()

        Dim tblAsientos As String = "tblasientos" & NumeroAleatorio(100000)

        Dim aFields() As String = {"sel.entero", "asiento.cadena20", "fechasi.fecha", "descripcion.cadena250", "id_emp.cadena2"}
        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblAsientos, aFields)


        EjecutarSTRSQL(myConn, lblInfo, " insert into " & tblAsientos _
                    & " select 0 sel,  asiento, fechasi, descripcion, id_emp " _
                    & " FROM jscotencasi " _
                    & " where " _
                    & " actual = 1 AND " _
                    & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                    & " ID_EMP = '" & jytsistema.WorkID & "'" _
                    & " ORDER BY ASIENTO ")


        aNombres = {"", "Asiento", "Fecha", "Descripción"}
        aCampos = {"sel", "asiento", "fechasi", "descripcion"}
        Dim aAnchos() As Long = {20, 100, 100, 400}
        Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Center, HorizontalAlignment.Left}
        Dim aFormato() As String = {"", "", sFormatoFechaCorta, ""}

        ds = DataSetRequery(ds, " select * from " & tblAsientos, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        IniciarTablaSeleccion(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormato, , True, , False)

        dg.ReadOnly = False
        For Each col As DataGridViewColumn In dg.Columns
            If col.Index > 0 Then col.ReadOnly = True
        Next

        FindField = aCampos(1)
        lblBuscar.Text = "Esta buscando por : " & aNombres(1)

    End Sub

    Private Sub jsContabProProcesaAsientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt.Dispose()
        ds.Dispose()
        dt = Nothing
        ds = Nothing
    End Sub

    Private Sub jsContabProProcesaAsientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        BindingSource1.Filter = Nothing
        ProcesarAsientos()
        CargarAsientosActuales()
        InsertarAuditoria(myConn, MovAud.iProcesar, sModulo, documentos_procesados)
    End Sub
    Private Sub IniciarProgreso()
        ProgressBar1.Value = 0
        lblProgreso.Text = ""
    End Sub
    Private Sub ProcesarAsientos()
        Dim iCont As Integer = 0
        For Each selectedItem As DataGridViewRow In dg.Rows
            ProgressBar1.Value = (iCont + 1) / dg.RowCount * 100
            With selectedItem

                lblProgreso.Text = .Cells(1).Value.ToString
                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso)
                If CBool(selectedItem.Cells(0).Value) Then

                    documentos_procesados += .Cells(1).Value.ToString + " "
                    EjecutarSTRSQL_Scalar(myConn, lblInfo, " update jscotencasi set actual = " & Asiento.iDiferido & " where asiento = '" & .Cells(1).Value.ToString & "' and ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ")
                    EjecutarSTRSQL_Scalar(myConn, lblInfo, " update jscotrenasi set actual = " & Asiento.iDiferido & " where asiento = '" & .Cells(1).Value.ToString & "' and ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ")
                    ActualizaCuentasSegunAsiento(myConn, lblInfo, ds, .Cells(1).Value.ToString, CDate(.Cells(2).Value.ToString))


                End If
            End With
            iCont += 1
        Next
        MensajeInformativoPlus("PROCESO CULMINADO CON EXITO ...")

    End Sub
    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
       dg.ColumnHeaderMouseClick

        If e.ColumnIndex = 0 Then
            For Each nRow As DataGridViewRow In dg.Rows
                nRow.Cells(0).Value = Not seleccionarTodo
            Next
            seleccionarTodo = Not seleccionarTodo
        Else
            Dim dgB As DataGridView = sender
            lblBuscar.Text = "Esta buscando por : " & aNombres(e.ColumnIndex)
            FindField = dt.Columns(dgB.Columns(e.ColumnIndex).Name).ColumnName

            txtBuscar.Focus()
            EnfocarTexto(txtBuscar)
        End If


    End Sub
    Private Sub txtBuscar_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged

        txtBuscar.Text = Replace(txtBuscar.Text, "'", "")

        BindingSource1.DataSource = dt
        If dt.Columns(FindField).DataType Is GetType(String) Then
            BindingSource1.Filter = FindField & " like '%" & txtBuscar.Text & "%'"
        ElseIf dt.Columns(FindField).DataType Is GetType(Double) And txtBuscar.Text <> "" Then
            BindingSource1.Filter = FindField & " >= " & ValorNumero(txtBuscar.Text) - 5 & " and " & FindField & " <= " & ValorNumero(txtBuscar.Text) + 5
        ElseIf dt.Columns(FindField).DataType Is GetType(MySql.Data.Types.MySqlDateTime) And txtBuscar.Text.Length = 10 Then
            BindingSource1.Filter = " CONVERT(" & FindField & ", System.DateTime) = '" & CDate(txtBuscar.Text).ToString & "' "
        End If
        dg.DataSource = BindingSource1

    End Sub

End Class