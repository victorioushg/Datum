Imports MySql.Data.MySqlClient
Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Controls

Public Class jsContabProProcesaAsientos
    Private Const sModulo As String = "Procesar asientos contables"
    Private Const nTabla As String = "proasicon"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private documentos_procesados As String = ""
    Private seleccionarTodo As Boolean = False

    Public Sub Cargar(ByVal MyCon As MySqlConnection)
        myConn = MyCon
        Me.Tag = sModulo
        lblLeyenda.Text = " Mediante este proceso se pasan los asientos contables diferidos a asientos " + vbCr + _
                " contables actuales.  " + vbCr + _
                " Para proceder los asientos contables diferidos deben poseer saldo cero o lo que es igual, " + vbCr + _
                " la suma de los débitos debe ser igual a la suma de los créditos. "

        ft.mensajeEtiqueta(lblInfo, "Seleccione asientos diferidos ... ", Transportables.tipoMensaje.iAyuda)
        CargarAsientosDiferidos()
        Me.Show()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub CargarAsientosDiferidos()

        IniciarProgreso()

        Dim tblAsientos As String = "tblasientos" & ft.NumeroAleatorio(100000)
        
        Dim aFields() As String = {"sel.entero.1.0", "asiento.cadena.20.0", "fechasi.fecha.0.0", "descripcion.cadena.250.0", "id_emp.cadena.2.0"}
        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblAsientos, aFields)


        ft.Ejecutar_strSQL(myconn, " insert into " & tblAsientos _
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
        Dim aAnchos() As Integer = {20, 100, 100, 400}
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
        ft = Nothing
    End Sub

    Private Sub jsContabProProcesaAsientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        ProcesarAsientos()
        'backGW.RunWorkerAsync()
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
            UpdateAsiento(selectedItem, CInt((iCont + 1) / dg.RowCount * 100))
            iCont += 1
        Next
        ft.mensajeInformativo("PROCESO CULMINADO CON EXITO ...")
    End Sub
    Private Sub UpdateAsiento(selectedItem As DataGridViewRow, progreso As Integer)

        With selectedItem

            refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, progreso, .Cells(1).Value.ToString)

            If CBool(selectedItem.Cells(0).Value) Then
                documentos_procesados += .Cells(1).Value.ToString + " "
                If ft.DevuelveScalarEntero(myConn, " select count(*) from jscotrenasi where trim(codcon) = '' AND asiento = '" & .Cells(1).Value.ToString & "' and ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                    ft.Ejecutar_strSQL(myConn, " update jscotencasi set actual = 1 " _
                        & " where asiento = '" & .Cells(1).Value.ToString & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " update jscotrenasi set actual = 1 " _
                        & " where asiento = '" & .Cells(1).Value.ToString & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "' ")

                End If
                ActualizaCuentasSegunAsiento(myConn, lblInfo, ds, .Cells(1).Value.ToString, CDate(.Cells(2).Value.ToString))

            End If

        End With

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


    Private Sub backGW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles backGW.DoWork
        Dim worker As BackgroundWorker = sender
        '  ProcesarAsientos(worker)
    End Sub

    Private Sub backGW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles backGW.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
    End Sub
End Class