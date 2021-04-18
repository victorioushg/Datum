Imports MySql.Data.MySqlClient
Public Class jsVenProRelacionNotasCreditoImpresion

    Private Const sModulo As String = "Proceso escogencia de Notas de Crédito para imprimir de La Relación de Notas de Crédito"

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

    Private Sub jsVenProRelacionNotasCreditoImpresion_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
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

        MsgBox(dtFacturas.Rows.Count.ToString & "NOTAS DE CREDITO de RELACION N° " & NumeroDeGuia & " han sido enviadas a la impresora fiscal", MsgBoxStyle.Information)
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
    Private Sub ImprimirNC(ByVal CodigoNotaCredito As String)

        If Not DocumentoImpreso(MyConn, lblInfo, "jsvenencncr", "numncr", CodigoNotaCredito) Then
            If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNCRPA08")) Then
                '1. Imprimir Nota de Entrega Fiscal

                If jytsistema.WorkBox = "" Then
                    ft.MensajeCritico("DEBE INDICAR UNA FORMA DE IMPRESION FISCAL")
                Else
                    '2. Colocar Nota de Entrega como impresa
                    'Dim NumeroFacturaAfectada As String = txtFactura.Text
                    'Dim NumeroSerialFacturaAfectada As String = ft.Ejecutar_strSQL_DevuelveScalar(MyConn, lblInfo, " select num_control from jsconnumcon where numdoc = '" & txtFactura.Text & "' and prov_cli = '" & txtCliente.Text & "' and org = 'FAC' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ")

                    'NumeroSerialFacturaAfectada = IIf(NumeroSerialFacturaAfectada.Length > 0, Mid(NumeroSerialFacturaAfectada, 9), "")
                    'Dim EmisionFacturaFactada As String = Format(ft.DevuelveScalarFecha(MyConn, " select emision from jsvenencncr where numncr = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ").ToString), sFormatoFechaFiscal)
                    'Dim HoraFacturaAfectada As String = Format(Now(), "HHmm")


                    Dim nTipoImpreFiscal As Integer = TipoImpresoraFiscal(MyConn, jytsistema.WorkBox)
                    Select Case nTipoImpreFiscal
                        Case 0 ' FACTURA FISCAL FORMA LIBRE
                            ' SE DIRIGE A LA IMPRESORA POR DEFECTO
                            'ImprimirNotaCreditoGrafica(MyConn, lblInfo, ds, txtCodigo.Text)
                        Case 1 'FACTURA FISCAL PRE-IMPRESA
                        Case 2, 5, 6 'IMPRESORA FISCAL TIPO ACLAS/BIXOLON
                            'ImprimirNotaCreditoPP1F3(MyConn, lblInfo, txtCodigo.Text, NumeroFacturaAfectada, NumeroSerialFacturaAfectada, txtNombreCliente.Text, _
                            '                                ft.Ejecutar_strSQL_DevuelveScalar(MyConn, lblInfo, "Select RIF from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "'"), _
                            '                                ft.Ejecutar_strSQL_DevuelveScalar(MyConn, lblInfo, "Select DIRFISCAL from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "'"), _
                            '                                ft.Ejecutar_strSQL_DevuelveScalar(MyConn, lblInfo, "Select TELEF1 from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "'"), _
                            '                                CDate(txtEmision.Text), txtCliente.Text, 0, CDate(txtEmision.Text), txtAsesor.Text, txtNombreAsesor.Text, _
                            '                                ValorNumero(txtTotal.Text), nTipoImpreFiscal)
                        Case 3 'IMPRESORA FISCAL TIPO BEMATECH
                        Case 4 'IMPRESORA FISCAL TIPO EPSON/PNP
                            'ImprimirNotaCreditoPnP(MyConn, lblInfo, txtCodigo.Text, txtFactura.Text, NumeroSerialFacturaAfectada, _
                            '                                EmisionFacturaFactada, HoraFacturaAfectada, txtNombreCliente.Text, ft.Ejecutar_strSQL_DevuelveScalar(MyConn, lblInfo, "Select RIF from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "'"), _
                            '                                ft.Ejecutar_strSQL_DevuelveScalar(MyConn, lblInfo, "Select DIRFISCAL from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "'"), _
                            '                                CDate(txtEmision.Text), CDate(txtEmision.Text))

                    End Select
                End If

            Else
                ft.MensajeCritico("Impresión de NOTA CREDITO no permitida...")
            End If
        End If


    End Sub
    Private Sub Imprimir(ByVal CodigoFactura As String)

        If Not DocumentoImpreso(MyConn, lblInfo, "jsvenencncr", "numncr", CodigoFactura) Then

            ft.mensajeInformativo("Imprimiendo NOTA CREDITO N° : " & CodigoFactura)
            EsperateUnPoquito(MyConn)

            Dim dtFactura As DataTable
            Dim nTablaFac As String = "tblFacturaGuia"

            ds = DataSetRequery(ds, " select * from jsvenencncr where numncr = '" & CodigoFactura & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaFac, lblInfo)
            dtFactura = ds.Tables(nTablaFac)

            If dtFactura.Rows.Count > 0 Then
                For kCont As Integer = 0 To dtFactura.Rows.Count - 1
                    With dtFactura.Rows(kCont)

                        Dim nNombreCliente As Object = ft.DevuelveScalarCadena(MyConn, " select nombre from jsvencatcli where codcli = '" & .Item("codcli") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        Dim nNombreAsesor As Object = ft.DevuelveScalarCadena(MyConn, " SELECT CONCAT(apellidos, ' ', nombres) nombre FROM jsvencatven WHERE codven = '" & .Item("codven") & "' AND tipo = 0 AND id_emp = '" & jytsistema.WorkID & "'  ")

                        Dim NombreCliente As String = If(IsDBNull(nNombreCliente), "", CStr(nNombreCliente))
                        Dim NombreAsesor As String = IIf(IsDBNull(nNombreAsesor), "", CStr(nNombreAsesor))

                        Dim nTipoImpreFiscal As Integer = TipoImpresoraFiscal(MyConn, jytsistema.WorkBox)
                        Select Case nTipoImpreFiscal
                            Case 0 ' FACTURA FISCAL FORMA LIBRE
                                ' SE DIRIGE A LA IMPRESORA POR DEFECTO
                                ImprimirNotaCreditoGrafica(MyConn, lblInfo, ds, CodigoFactura)
                            Case 1 'FACTURA FISCAL PRE-IMPRESA
                            Case 2, 5, 6, 7 'IMPRESORA FISCAL TIPO ACLAS/BIXOLON

                                If nTipoImpreFiscal = 7 Then
                                    ''hgjfjhkfkf()
                                Else
                                    ImprimirFacturaFiscalPP1F3(MyConn, lblInfo, CodigoFactura, NumeroSERIALImpresoraFISCAL(MyConn, lblInfo, jytsistema.WorkBox), _
                                        NombreCliente, .Item("codcli"), ft.DevuelveScalarCadena(MyConn, "Select RIF from jsvencatcli where codcli = '" & .Item("codcli") & "' and id_emp = '" & jytsistema.WorkID & "'"), _
                                        ft.DevuelveScalarCadena(MyConn, "Select DIRFISCAL from jsvencatcli where codcli = '" & .Item("codcli") & "' and id_emp = '" & jytsistema.WorkID & "'"), _
                                        CDate(.Item("emision").ToString), CStr(.Item("condpag")), CDate(.Item("vence").ToString), _
                                        .Item("codven"), NombreAsesor, nTipoImpreFiscal)
                                End If

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