Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Imports System.IO
Imports System.Text
Public Class jsComProRetencionesISLR
    Private Const sModulo As String = "Construcción Archivo Retenciones ISLR"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dtRetenciones As New DataTable
    Private ft As New Transportables

    Private strRetenciones As String = ""
    Private nTablaRetenciones As String = "tblRetenciones"


    Private CaminoArchivo As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
    Private NombreArchivoBase As String = "\RetencionesISLR"
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        MyConn = MyCon
        Dim dates As SfDateTimeEdit() = {txtFechaDesde, txtFechaHasta}
        IniciarTXT()
        Me.Show()

    End Sub
    Private Sub IniciarTXT()

        txtFechaDesde.Value = PrimerDiaMes(jytsistema.sFechadeTrabajo)
        txtFechaHasta.Value = jytsistema.sFechadeTrabajo

        txtRutaArchivo.Text = CamnioNombreArchivo()

    End Sub

    Private Function CamnioNombreArchivo() As String
        Return CaminoArchivo & NombreArchivoBase & Format(txtFechaDesde.Value, "ddMMyy") & "_" & Format(txtFechaHasta.Value, "ddMMyy") & ".xml"
    End Function

    Private Sub jsComProRetencionesISLR_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
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


            ProgressBar1.Value = 0
            lblProgreso.Text = ""

            strRetenciones = SeleccionCOMPRASListadoRetencionesISLR(txtFechaDesde.Value, txtFechaHasta.Value, "", "")
            ds = DataSetRequery(ds, strRetenciones, MyConn, nTablaRetenciones, lblInfo)
            dtRetenciones = ds.Tables(nTablaRetenciones)

            Dim sb As New StringBuilder()
            Dim XMLRecSetName As String = "DetalleRetencion"

            If dtRetenciones.Rows.Count > 0 Then
                sb.AppendLine("<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "ISO-8859-1" & Chr(34) & "?>")
                sb.AppendLine("<RelacionRetencionesISLR RifAgente= " & Chr(34) & dtRetenciones.Rows(0).Item("RifAgente") & Chr(34) _
                              & " " & "Periodo=" & Chr(34) & dtRetenciones.Rows(0).Item("PERIODO") & Chr(34) & ">")

                Dim iCont As Integer
                For iCont = 0 To dtRetenciones.Rows.Count - 1
                    With dtRetenciones.Rows(iCont)
                        ProgressBar1.Value = CInt(iCont / dtRetenciones.Rows.Count * 100)
                        lblProgreso.Text = " Retenciones ISLR : " & .Item("rif") & " " & .Item("numcom") & " " & .Item("num_control")

                        sb.AppendLine("<" & XMLRecSetName & ">")
                        sb.AppendLine("<RifRetenido>" & Mid(.Item("rifRetenido"), 1, 1) & _
                                      RellenaCadenaConCaracter(Mid(.Item("rifretenido"), 2, Len(.Item("rifretenido"))), "I", 9, "0") & "</RifRetenido>")

                        sb.AppendLine("<NumeroFactura>" & .Item("numcom") & "</NumeroFactura>")
                        sb.AppendLine("<NumeroControl>" & .Item("num_control") & "</NumeroControl>")
                        sb.AppendLine("<FechaOperacion>" & ft.FormatoFechaISLR(CDate(.Item("fec_retencion").ToString)) & "</FechaOperacion>")
                        sb.AppendLine("<CodigoConcepto>" & .Item("CodigoConcepto") & "</CodigoConcepto>")
                        sb.AppendLine("<MontoOperacion>" & .Item("MontoOperacion") & "</MontoOperacion>")
                        sb.AppendLine("<PorcentajeRetencion>" & .Item("PorcentajeRetencion") & "</PorcentajeRetencion>")
                        sb.AppendLine("</" & XMLRecSetName & ">")

                    End With
                Next
                sb.AppendLine("</RelacionRetencionesISLR>")

                Using outfile As New StreamWriter(txtRutaArchivo.Text)
                    outfile.Write(sb.ToString())
                End Using

                InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFechaDesde.Text)
                Me.Close()
            End If

            MsgBox("Proceso terminado...", MsgBoxStyle.Information, sModulo)

        End If

    End Sub

    Private Sub btnClienteDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRutaArchivo.Click
        Dim ofd As New OpenFileDialog()

        ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        ofd.Filter = "Archivos XML |*.xml"
        ofd.FilterIndex = 2
        ofd.RestoreDirectory = True

        If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            CaminoArchivo = ofd.FileName
        End If

        ofd = Nothing
    End Sub

    Private Sub txtFechaDesde_ValueChanged(sender As Object, e As Events.DateTimeValueChangedEventArgs) Handles txtFechaDesde.ValueChanged
        If txtFechaDesde.Text <> "" AndAlso txtFechaHasta.Text <> "" Then txtRutaArchivo.Text = CamnioNombreArchivo()
    End Sub

    Private Sub txtFechaHasta_ValueChanged(sender As Object, e As Events.DateTimeValueChangedEventArgs) Handles txtFechaHasta.ValueChanged
        If txtFechaDesde.Text <> "" AndAlso txtFechaHasta.Text <> "" Then txtRutaArchivo.Text = CamnioNombreArchivo()
    End Sub
End Class