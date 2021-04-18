Imports MySql.Data.MySqlClient
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

        ft.habilitarObjetos(False, True, txtFechaDesde, txtFechaHasta)
        ft.habilitarObjetos(True, True, btnFechaDesde, btnFechaHasta)

        txtFechaDesde.Text = ft.FormatoFecha(PrimerDiaMes(jytsistema.sFechadeTrabajo))
        txtFechaHasta.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)

        txtRutaArchivo.Text = CamnioNombreArchivo()


    End Sub
    Private Function CamnioNombreArchivo() As String
        Return CaminoArchivo & NombreArchivoBase & Format(CDate(txtFechaDesde.Text), "ddMMyy") & "_" & Format(CDate(txtFechaHasta.Text), "ddMMyy") & ".txt"
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
                 My.Computer.FileSystem.DeleteFile( _
                     txtRutaArchivo.Text, _
                     FileIO.UIOption.OnlyErrorDialogs, _
                     FileIO.RecycleOption.DeletePermanently, _
                     FileIO.UICancelOption.DoNothing)




            strRetenciones = SeleccionCOMPRASListadoRetencionesIVA(CDate(txtFechaDesde.Text), CDate(txtFechaHasta.Text), "", "")
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
                    Linea = Mid(.Item("rifemp"), 1, 1) & RellenaCadenaConCaracter(Mid(.Item("rifemp"), 2, Len(.Item("rifemp"))), "I", 9, "0") & _
                    vbTab & .Item("periodo").ToString & _
                    vbTab & ft.FormatoFechaMySQL(CDate(.Item("emision").ToString)) & _
                    vbTab & .Item("tipooperacion") & _
                    vbTab & .Item("tipodocumento") & _
                    vbTab & Mid(RifCliente, 1, 1) & RellenaCadenaConCaracter(Mid(RifCliente, 2, Len(RifCliente)), "I", 9, "0") & _
                    vbTab & Trim(.Item("numcom")) & _
                    vbTab & .Item("num_control") & _
                    vbTab & Format(.Item("tot_com"), "0.00") & _
                    vbTab & Format(.Item("baseiva"), "0.00") & _
                    vbTab & Format(.Item("retencion"), "0.00") & _
                    vbTab & .Item("doc_afectado") & _
                    vbTab & .Item("num_retencion") & _
                    vbTab & Format(.Item("montoexento"), "0.00") & _
                    vbTab & Format(.Item("poriva"), "0.00") & _
                    vbTab & .Item("num_expediente")

                    sb.AppendLine(Linea)

                    'sb.AppendLine("= = = = = =")
                    'sb.Append(sr.ReadToEnd())
                    'sb.AppendLine()
                    'sb.AppendLine()

                End With
            Next

            Using outfile As New StreamWriter(txtRutaArchivo.Text)
                outfile.Write(sb.ToString())
            End Using
            MsgBox("Proceso terminado...", MsgBoxStyle.Information, sModulo)
            InsertarAuditoria(MyConn, MovAud.iProcesar, sModulo, "")
            ProgressBar1.Value = 0
            lblProgreso.Text = ""

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFechaDesde.Text)
            '      Me.Close()

        End If

    End Sub

    Private Sub btnFechaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaDesde.Click
        txtFechaDesde.Text = ft.FormatoFecha(SeleccionaFecha(CDate(txtFechaDesde.Text), Me, btnFechaDesde))
    End Sub
    Private Sub btnFechaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaHasta.Click
        txtFechaHasta.Text = ft.FormatoFecha(SeleccionaFecha(CDate(txtFechaHasta.Text), Me, btnFechaHasta))
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




    Private Sub txtFechaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFechaDesde.TextChanged
        If txtFechaDesde.Text <> "" AndAlso txtFechaHasta.Text <> "" Then txtRutaArchivo.Text = CamnioNombreArchivo()
    End Sub

    Private Sub txtFechaHasta_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFechaHasta.TextChanged
        If txtFechaDesde.Text <> "" AndAlso txtFechaHasta.Text <> "" Then txtRutaArchivo.Text = CamnioNombreArchivo()
    End Sub
End Class