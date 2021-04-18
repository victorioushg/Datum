Imports MySql.Data.MySqlClient
Public Class jsVenProGuiaDespachoImpresion

    Private Const sModulo As String = "Proceso escogencia de facturas para imprimir de guía de despacho"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dtFacturas As New DataTable
    Private ft As New Transportables

    Private nTablaFacturas As String = "tblFacturas"

    Private Items As Integer
    Private NumeroDeGuia As String

    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal NumeroGuia As String, ByVal dtFac As DataTable)

        MyConn = MyCon
        NumeroDeGuia = NumeroGuia
        dtFacturas = dtFac

        Label13.Text = "Impresión de facturas guía N° " & NumeroGuia

        IniciarTXT()

        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()

        Items = dtFacturas.Rows.Count
        ft.habilitarObjetos(False, True, txtItems)
        AbrirFacturas()
        'txtItems.Text = ft.FormatoEntero(dtFacturas.Rows.Count)

    End Sub

    Private Sub jsVenProGuiaDespachoImpresion_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsVenProGuiaDespachoMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, NumeroDeGuia)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, NumeroDeGuia)
        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Dim lvCont As Integer
        For lvCont = 0 To lvPedidos.Items.Count - 1
            ProgressBar1.Value = (lvCont + 1) / lvPedidos.Items.Count * 100
            If lvPedidos.Items(lvCont).Checked Then
                Imprimir(dtFacturas.Rows(lvCont).Item("CODIGOFAC"))
                EsperateUnPoquito(MyConn)
            End If
        Next

        MsgBox(dtFacturas.Rows.Count.ToString & "FACTURAS de GUIA N° " & NumeroDeGuia & " han sido enviadas a la impresora fiscal", MsgBoxStyle.Information)
        MsgBox("Proceso terminado...", MsgBoxStyle.Information, sModulo)

        ProgressBar1.Value = 0
        lblProgreso.Text = ""

        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, NumeroDeGuia)
        Me.Close()

    End Sub

    Private Sub AbrirFacturas()

        CargaListViewImprimirFacturasGuiaDespacho(lvPedidos, dtFacturas)

        'SELECCIONAR TODAS LAS FACTURAS
        For Each lr As ListViewItem In lvPedidos.Items
            lr.Checked = True
        Next

    End Sub

    Private Sub lvPedidos_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvPedidos.ItemChecked

        If e.Item.SubItems.Count > 3 Then
            If e.Item.Checked Then
                Items += 1
            Else
                Items -= 1
            End If
            txtItems.Text = ft.FormatoEntero(CInt(Items))
        End If

    End Sub

    Private Sub Imprimir(ByVal CodigoFactura)

        If Not DocumentoImpreso(myConn, lblInfo, "jsvenencfac", "numfac", CodigoFactura) Then

            ft.mensajeInformativo("Imprimiendo Factura N° : " & CodigoFactura)
            EsperateUnPoquito(myConn)

            Dim dtFactura As DataTable
            Dim nTablaFac As String = "tblFacturaGuia"

            ds = DataSetRequery(ds, " select * from jsvenencfac where numfac = '" & CodigoFactura & "' and id_emp = '" & jytsistema.WorkID & "' ", myConn, nTablaFac, lblInfo)
            dtFactura = ds.Tables(nTablaFac)

            If dtFactura.Rows.Count > 0 Then
                For kCont As Integer = 0 To dtFactura.Rows.Count - 1
                    With dtFactura.Rows(kCont)

                        Dim NombreCliente As String = ft.DevuelveScalarCadena(MyConn, " select nombre from jsvencatcli where codcli = '" & .Item("codcli") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        Dim NombreAsesor As String = ft.DevuelveScalarCadena(MyConn, " SELECT CONCAT(apellidos, ' ', nombres) FROM jsvencatven WHERE codven = '" & .Item("codven") & "' AND tipo = 0 AND id_emp = '" & jytsistema.WorkID & "'  ")

                        Dim nTipoImpreFiscal As Integer = TipoImpresoraFiscal(MyConn, jytsistema.WorkBox)
                        Select Case nTipoImpreFiscal
                            Case 0 ' FACTURA FISCAL FORMA LIBRE
                                ' SE DIRIGE A LA IMPRESORA POR DEFECTO
                                ImprimirFacturaGrafica(MyConn, lblInfo, ds, CodigoFactura)
                            Case 1 'FACTURA FISCAL PRE-IMPRESA
                            Case 2, 5, 6, 7 'IMPRESORA FISCAL TIPO ACLAS/BIXOLON

                                ImprimirFacturaFiscalPP1F3(MyConn, lblInfo, CodigoFactura, NumeroSERIALImpresoraFISCAL(MyConn, lblInfo, jytsistema.WorkBox), _
                                    NombreCliente, .Item("codcli"), ft.DevuelveScalarCadena(MyConn, "Select RIF from jsvencatcli where codcli = '" & .Item("codcli") & "' and id_emp = '" & jytsistema.WorkID & "'"), _
                                    ft.DevuelveScalarCadena(MyConn, "Select DIRFISCAL from jsvencatcli where codcli = '" & .Item("codcli") & "' and id_emp = '" & jytsistema.WorkID & "'"), _
                                    CDate(.Item("emision").ToString), CStr(.Item("condpag")), CDate(.Item("vence").ToString), _
                                    .Item("codven"), NombreAsesor, nTipoImpreFiscal)

                            Case 3 'IMPRESORA FISCAL TIPO BEMATECH
                            Case 4 'IMPRESORA FISCAL TIPO EPSON/PNP
                                ImprimirFacturaFiscalPnP(MyConn, lblInfo, CodigoFactura, NombreCliente, .Item("codcli"), _
                                    ft.DevuelveScalarCadena(MyConn, "Select RIF from jsvencatcli where codcli = '" & .Item("codcli") & "' and id_emp = '" & jytsistema.WorkID & "'"), _
                                    ft.DevuelveScalarCadena(MyConn, "Select DIRFISCAL from jsvencatcli where codcli = '" & .Item("codcli") & "' and id_emp = '" & jytsistema.WorkID & "'"), _
                                    CDate(.Item("emision").ToString), .Item("condpag"), CDate(.Item("vence").ToString), .Item("codven"), NombreAsesor)
                        End Select
                    End With
                Next
            End If

            dtFactura.Dispose()
            dtFactura = Nothing

        Else
            ft.MensajeCritico("FACTURA N° " & CodigoFactura & " NO IMPRESA...")

        End If

    End Sub

End Class