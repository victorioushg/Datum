Imports MySql.Data.MySqlClient
Public Class jsContabProProcesaAsientos
    Private Const sModulo As String = "Procesar asientos contables"
    Private Const nTabla As String = "proasicon"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private documentos_procesados As String = ""
    Private seleccionarTodo As Boolean = False


    Public Sub Cargar(ByVal MyCon As MySqlConnection)
        myConn = MyCon
        Me.Tag = sModulo
        lblLeyenda.Text = " Mediante este proceso se pasan los asientos contables diferidos a asientos " + vbCr + _
                " contables actuales.  " + vbCr + _
                " Para proceder los asientos contables diferidos deben poseer saldo cero o lo que es igual, " + vbCr + _
                " la suma de los débitos debe ser igual a la suma de los créditos. "

        MensajeEtiqueta(lblInfo, "Seleccione asientos diferidos ... ", TipoMensaje.iAyuda)
        CargarAsientosDiferidos()
        Me.Show()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub CargarAsientosDiferidos()

        IniciarProgreso()

        Dim tblAsientos As String = "tblasientos" & NumeroAleatorio(100000)
        
        Dim aFields() As String = {"sel.entero", "asiento.cadena20", "fechasi.fecha", "descripcion.cadena250", "id_emp.cadena2"}
        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblAsientos, aFields)


        EjecutarSTRSQL(myConn, lblInfo, " insert into " & tblAsientos _
                    & " select 0 sel,  asiento, fechasi, descripcion, id_emp " _
                    & " FROM jscotencasi " _
                    & " where " _
                    & " DEBITOS = -CREDITOS AND " _
                    & " actual = 0 AND " _
                    & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                    & " ID_EMP = '" & jytsistema.WorkID & "'" _
                    & " ORDER BY ASIENTO ")


        Dim aNombres() As String = {"", "Asiento", "Fecha", "Descripción"}
        Dim aCampos() As String = {"sel", "asiento", "fechasi", "descripcion"}
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

        ProcesarAsientos()
        CargarAsientosDiferidos()
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
                    ActualizaCuentasSegunAsiento(myConn, lblInfo, ds, .Cells(1).Value.ToString, CDate(.Cells(2).Value.ToString))

                    If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(*) from jscotrenasi where trim(codcon) = '' AND asiento = '" & .Cells(1).Value.ToString & "' and ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ")) = 0 Then
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotencasi set actual = 1 " _
                            & " where asiento = '" & .Cells(1).Value.ToString & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " update jscotrenasi set actual = 1 " _
                            & " where asiento = '" & .Cells(1).Value.ToString & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "' ")

                    End If
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
        End If


    End Sub

   
End Class