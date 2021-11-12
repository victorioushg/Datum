Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Imports System.IO
Imports System.Text
Public Class jsVenProNestleBPAID
    Private Const sModulo As String = "Construcción Archivo Retenciones IVA"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dtRetenciones As New DataTable
    Private ft As New Transportables

    Private strRetenciones As String = ""
    Private nTablaRetenciones As String = "tblRetenciones"


    Private CaminoArchivo As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
    Private NombreArchivoBase = "\RetencionesIVA"
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        MyConn = MyCon
        IniciarTXT()
        Me.Show()

    End Sub
    Private Sub IniciarTXT()

        Dim dates As SfDateTimeEdit() = {txtFechaDesde, txtFechaHasta}
        SetSizeDateObjects(dates)

        txtFechaDesde.Value = PrimerDiaMes(jytsistema.sFechadeTrabajo)
        txtFechaHasta.Value = jytsistema.sFechadeTrabajo

        txtRutaArchivo.Text = CamnioNombreArchivo()


    End Sub
    Private Function CamnioNombreArchivo() As String
        Return CaminoArchivo & NombreArchivoBase & Format(txtFechaDesde.Value, "ddMMyy") & "_" & Format(txtFechaHasta.Value, "ddMMyy") & ".txt"
    End Function

    Private Sub jsComProRetencionesIVA_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsComProReconstruccionDeSaldos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then


            If File.Exists(txtRutaArchivo.Text) Then _
                 My.Computer.FileSystem.DeleteFile(
                     txtRutaArchivo.Text,
                     FileIO.UIOption.OnlyErrorDialogs,
                     FileIO.RecycleOption.DeletePermanently,
                     FileIO.UICancelOption.DoNothing)

            strRetenciones = SeleccionCOMPRASListadoRetencionesIVA(txtFechaDesde.Value, txtFechaHasta.Value, "", "")
            ds = DataSetRequery(ds, strRetenciones, MyConn, nTablaRetenciones, lblInfo)
            dtRetenciones = ds.Tables(nTablaRetenciones)

            Dim sb As New StringBuilder()
            Dim Linea As String = ""
            Dim iCont As Integer
            For iCont = 0 To dtRetenciones.Rows.Count - 1
                With dtRetenciones.Rows(iCont)
                    ProgressBar1.Value = CInt(iCont / dtRetenciones.Rows.Count * 100)
                    lblProgreso.Text = " Retenciones IVA : " & .Item("rif") & " " & .Item("numcom") & " " & .Item("num_control")

                    Dim nPer As String = .Item("PERIODO")

                    Dim RifCliente As String = .Item("rifpro").ToString.TrimEnd()
                    Linea = Mid(.Item("rifemp"), 1, 1) & RellenaCadenaConCaracter(Mid(.Item("rifemp"), 2, Len(.Item("rifemp"))), "I", 9, "0") &
                    vbTab & .Item("periodo").ToString &
                    vbTab & ft.FormatoFechaMySQL(CDate(.Item("emision").ToString)) &
                    vbTab & .Item("tipooperacion") &
                    vbTab & .Item("tipodocumento") &
                    vbTab & Mid(RifCliente, 1, 1) & RellenaCadenaConCaracter(Mid(RifCliente, 2, Len(RifCliente)), "I", 9, "0") &
                    vbTab & Trim(.Item("numcom")) &
                    vbTab & .Item("num_control") &
                    vbTab & Format(.Item("tot_com"), "0.00") &
                    vbTab & Format(.Item("baseiva"), "0.00") &
                    vbTab & Format(.Item("retencion"), "0.00") &
                    vbTab & .Item("doc_afectado") &
                    vbTab & .Item("num_retencion") &
                    vbTab & Format(.Item("montoexento"), "0.00") &
                    vbTab & Format(.Item("poriva"), "0.00") &
                    vbTab & .Item("num_expediente")

                    sb.AppendLine(Linea)

                End With
            Next

            Using outfile As New StreamWriter(txtRutaArchivo.Text)
                outfile.Write(sb.ToString())
            End Using
            MsgBox("Proceso terminado...", MsgBoxStyle.Information, sModulo)
            InsertarAuditoria(MyConn, MovAud.iProcesar, sModulo, "")
            ProgressBar1.Value = 0
            lblProgreso.Text = ""

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFechaDesde.Value)
        End If

    End Sub

    Private Sub btnClienteDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRutaArchivo.Click
        Dim ofd As New OpenFileDialog()

        ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        ofd.Filter = "Archivos TXT |*.txt"
        ofd.FilterIndex = 2
        ofd.RestoreDirectory = True

        If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            CaminoArchivo = ofd.FileName
        End If

        ofd = Nothing
    End Sub

    Private Sub txtFechaDesde_ValueChanged(sender As Object, e As Events.DateTimeValueChangedEventArgs) Handles txtFechaDesde.ValueChanged, txtFechaHasta.ValueChanged
        CamnioNombreArchivo()
    End Sub

End Class