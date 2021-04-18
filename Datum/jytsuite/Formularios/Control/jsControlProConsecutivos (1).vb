Imports MySql.Data.MySqlClient
Public Class jsControlProConsecutivos
    Private Const sModulo As String = "Verificar números consecutivos"
    Private Const nTabla As String = "proConsecutivos"

    Private tblNoConsecutivos As String = "TablaNC" & NumeroAleatorio(10000)

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private dtNC As DataTable

    Private PrimerNumero As String
    Private UltimoNumero As String

    Private aConsecutivos() As String = {"Números de control"}

    Public Sub Cargar(ByVal MyCon As MySqlConnection)
        myConn = MyCon
        Me.Tag = sModulo

        lblLeyenda.Text = " Mediante este proceso se verifican los números consecutivos asignados a los diferentes documentos " + vbCr + _
                " durante un período determinado. " + vbCr + _
                " - Escoja el período para revisión " + vbCr + _
                " - SI NO ESTA SEGURO por favor consulte CON EL ADMINISTRADOR "

        txtFechaDesde.Text = FormatoFecha(PrimerDiaMes(jytsistema.sFechadeTrabajo))
        txtFechaHasta.Text = FormatoFecha(UltimoDiaMes(jytsistema.sFechadeTrabajo))
        RellenaCombo(aConsecutivos, cmbContadores)

        MensajeEtiqueta(lblInfo, " ... ", TipoMensaje.iAyuda)

        Me.Show()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub jsControlProConsecutivos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

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
        MensajeInformativo(lblInfo, " Proceso culminado con éxito... ")
    End Sub
    Private Sub IniciarConsecutivos()

        Dim strSQl As String = " SELECT replace(num_control, '-','') num_control, numdoc, emision " _
            & " FROM jsconnumcon " _
            & " WHERE " _
            & " emision >= '" & FormatoFechaMySQL(CDate(txtFechaDesde.Text)) & "' AND " _
            & " emision <= '" & FormatoFechaMySQL(CDate(txtFechaHasta.Text)) & "' AND " _
            & " org in ('FAC', 'NCR', 'NDB','PVE', 'CON') AND " _
            & " origen in ('FAC', 'PVE') AND " _
            & " mid(num_control,1,3) <> 'FCT' and " _
            & " num_control <> '' and " _
            & " id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY num_control "

        Dim aFld() As String = {"num_control", "numdoc", "emision"}
        Dim aStr() As String = {" Número Control", "Documento", "Fecha emisión"}
        Dim aAnc() As Long = {90, 90, 60}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
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

        Dim aFldExp() As String = {"NoConsecutivo.cadena15"}
        EjecutarSTRSQL(myConn, lblInfo, " drop table if exists " & tblNoConsecutivos)
        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblNoConsecutivos, aFldExp)

        If dt.Rows.Count > 0 Then


            Dim numConsecutivo As String = PrimerNumero

            While numConsecutivo <> UltimoNumero
                Dim row As DataRow = dt.Rows.Find(numConsecutivo)
                If (row Is Nothing) Then
                    EjecutarSTRSQL(myConn, lblInfo, " insert into " & tblNoConsecutivos & " set noconsecutivo = '" & numConsecutivo & "' ")
                    IniciarNoConsecutivos(tblNoConsecutivos)
                End If
                numConsecutivo = IncrementarCadena(numConsecutivo)
            End While

        End If

        MensajeInformativo(lblInfo, "Búsqueda de números faltantes terminada")


    End Sub
    Private Sub IniciarNoConsecutivos(ByVal tblNoConsecutivos As String)

        Dim nTablaNC As String = "tblNoConsecutivos"

        Dim aFld() As String = {"noconsecutivo", ""}
        Dim aStr() As String = {" Números Control faltantes", ""}
        Dim aAnc() As Long = {100, 100}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", ""}

        ds = DataSetRequery(ds, " select * from " & tblNoConsecutivos & " order by 1 ", myConn, nTablaNC, lblInfo)
        dtNC = ds.Tables(nTablaNC)

        IniciarTabla(dgFaltantes, dtNC, aFld, aStr, aAnc, aAli, aFor)

        lblCantidad.Text = FormatoEntero(dtNC.Rows.Count)

    End Sub
    Private Sub btnFechaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaDesde.Click
        txtFechaDesde.Text = SeleccionaFecha(CDate(txtFechaDesde.Text), btnFechaDesde)
    End Sub

    Private Sub btnFechaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaHasta.Click
        txtFechaHasta.Text = SeleccionaFecha(CDate(txtFechaHasta.Text), btnFechaHasta)
    End Sub

    Private Sub cmbContadores_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbContadores.SelectedIndexChanged, _
        txtFechaDesde.TextChanged, txtFechaHasta.TextChanged

        If txtFechaDesde.Text <> "" AndAlso txtFechaHasta.Text <> "" Then
            IniciarConsecutivos()
        End If

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