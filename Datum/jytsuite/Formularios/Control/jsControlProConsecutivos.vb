Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsControlProConsecutivos
    Private Const sModulo As String = "Verificar números consecutivos"
    Private Const nTabla As String = "proConsecutivos"

    Private tblNoConsecutivos As String

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private dtNC As DataTable
    Private ft As New Transportables

    Private PrimerNumero As String
    Private UltimoNumero As String

    Private aConsecutivos() As String = {"Números de control"}

    Public Sub Cargar(ByVal MyCon As MySqlConnection)
        myConn = MyCon
        Me.Tag = sModulo

        tblNoConsecutivos = "TablaNC" & ft.NumeroAleatorio(10000)

        Dim dates As SfDateTimeEdit() = {txtFechaDesde, txtFechaHasta}
        SetSizeDateObjects(dates)

        lblLeyenda.Text = " Mediante este proceso se verifican los números consecutivos asignados a los diferentes documentos " + vbCr +
                " durante un período determinado. " + vbCr +
                " - Escoja el período para revisión " + vbCr +
                " - SI NO ESTA SEGURO por favor consulte CON EL ADMINISTRADOR "

        txtFechaDesde.Value = PrimerDiaMes(jytsistema.sFechadeTrabajo)
        txtFechaHasta.Value = UltimoDiaMes(jytsistema.sFechadeTrabajo)
        ft.RellenaCombo(aConsecutivos, cmbContadores)

        ft.mensajeEtiqueta(lblInfo, " ... ", Transportables.tipoMensaje.iAyuda)

        Me.Show()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub jsControlProConsecutivos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt.Dispose()
    End Sub

    Private Sub jsControlProConsecutivos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Procesar()
    End Sub
    Private Sub Procesar()
        HabilitarCursorEnEspera()


        DeshabilitarCursorEnEspera()
        ft.mensajeInformativo(" Proceso culminado con éxito... ")
    End Sub
    Private Sub IniciarConsecutivos()

        Dim strSQl As String = " SELECT replace(num_control, '-','') num_control, numdoc, emision " _
            & " FROM jsconnumcon " _
            & " WHERE " _
            & " emision >= '" & ft.FormatoFechaMySQL(txtFechaDesde.Value) & "' AND " _
            & " emision <= '" & ft.FormatoFechaMySQL(txtFechaHasta.Value) & "' AND " _
            & " org in ('FAC', 'NCR', 'NDB','PVE', 'CON') AND " _
            & " origen in ('FAC', 'PVE') AND " _
            & " mid(num_control,1,3) <> 'FCT' and " _
            & " num_control <> '' and " _
            & " id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY num_control "

        Dim aFld() As String = {"num_control", "numdoc", "emision"}
        Dim aStr() As String = {" Número Control", "Documento", "Fecha emisión"}
        Dim aAnc() As Integer = {90, 90, 60}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda,
                                 AlineacionDataGrid.Centro}
        Dim aFor() As String = {"", "", sFormatoFechaCorta}

        ds = DataSetRequery(ds, strSQl, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        If dt.Rows.Count > 0 Then
            PrimerNumero = ""
            Dim iCont As Integer = 0
            While PrimerNumero = "" And iCont < dt.Rows.Count - 1
                PrimerNumero = Replace(dt.Rows(iCont).Item("num_control").ToString, "-", "")
                iCont += 1
            End While
            UltimoNumero = Replace(dt.Rows(dt.Rows.Count - 1).Item("num_control").ToString, "-", "")
        End If

        Dim col(1) As DataColumn
        col(0) = dt.Columns(0)
        dt.PrimaryKey = col

        IniciarTabla(dg, dt, aFld, aStr, aAnc, aAli, aFor)

    End Sub
    Private Sub BuscaNoConsecutivos()

        Dim aFldExp() As String = {"NoConsecutivo.cadena.15.0"}
        ft.Ejecutar_strSQL(myConn, " drop table if exists " & tblNoConsecutivos)
        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblNoConsecutivos, aFldExp)

        If dt.Rows.Count > 0 Then


            Dim numConsecutivo As String = PrimerNumero

            While numConsecutivo <> UltimoNumero
                Dim row As DataRow = dt.Rows.Find(numConsecutivo)
                If (row Is Nothing) Then
                    ft.Ejecutar_strSQL(myConn, " insert into " & tblNoConsecutivos & " set noconsecutivo = '" & numConsecutivo & "' ")
                    IniciarNoConsecutivos(tblNoConsecutivos)
                End If
                numConsecutivo = IncrementarCadena(numConsecutivo)
            End While

        End If

        ft.mensajeInformativo("Búsqueda de números faltantes terminada")


    End Sub
    Private Sub IniciarNoConsecutivos(ByVal tblNoConsecutivos As String)

        Dim nTablaNC As String = "tblNoConsecutivos"

        Dim aFld() As String = {"noconsecutivo", ""}
        Dim aStr() As String = {" Números Control faltantes", ""}
        Dim aAnc() As Integer = {100, 100}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", ""}

        ds = DataSetRequery(ds, " select * from " & tblNoConsecutivos & " order by 1 ", myConn, nTablaNC, lblInfo)
        dtNC = ds.Tables(nTablaNC)

        IniciarTabla(dgFaltantes, dtNC, aFld, aStr, aAnc, aAli, aFor)

        lblCantidad.Text = ft.FormatoEntero(dtNC.Rows.Count)

    End Sub

    Private Sub cmbContadores_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbContadores.SelectedIndexChanged
        IniciarConsecutivos()
    End Sub

    Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGo.Click
        HabilitarCursorEnEspera()
        dgFaltantes.Enabled = False
        BuscaNoConsecutivos()
        dgFaltantes.Enabled = True
        DeshabilitarCursorEnEspera()
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        If dtNC.Rows.Count > 0 Then ImprimirConsecutivosFaltantes(myConn, lblInfo, dtNC)
    End Sub
End Class