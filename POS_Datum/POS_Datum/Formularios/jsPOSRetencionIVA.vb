Imports MySql.Data.MySqlClient
Imports FP_AclasBixolon

Public Class jsPOSRetencionIVA

    Private Const sModulo As String = "Retención IVA Punto de Venta"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As New DataTable
    Private ft As New Transportables

    Private nTabla As String = "tblFacturasPVE"
    Private strSQL As String = ""

    Private NumCaja As String = ""

    Private IB As New AclasBixolon
    Private bRet As Boolean
    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal CodigoCaja As String)

        MyConn = MyCon
        dsLocal = ds
        NumCaja = CodigoCaja
        IniciarTXT()
        Me.ShowDialog()

    End Sub

    Private Sub IniciarTXT()

        strSQL = " SELECT a.* " _
                & " FROM jsvenencpos a " _
                & " WHERE " _
                & " SUBSTRING(a.numfac,1,2) <> 'DT' AND " _
                & " a.TIPO = 0 AND " _
                & " a.imp_iva > 0 AND " _
                & " a.NOMBRE_RETENCION_IVA <> 'RETENCION-IVA' AND " _
                & " a.EMISION >= DATE_add('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL " & -1 * CInt(ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM43")) & " DAY) AND  " _
                & " SPLIT_STR(RIF, '-', 3) IN ('1','2','3','4','5','6','7','8','9', '0') AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' ORDER BY a.emision DESC "

        dtLocal = ft.AbrirDataTable(dsLocal, nTabla, MyConn, strSQL)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtFacturaFiscal, txtNumeroSerial, txtDocumentoInterno, _
                         txtFechaFactura, txtRIF, txtNombreCliente, txtNumeroRetencion)
        txtFechaRetencion.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtFechaRecepcion.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtSubTotalFactura, txtIVA, txtTotalFactura, txtPorcentajeRetencion, _
                     txtMontoRetencion)

        ft.habilitarObjetos(False, True, txtFacturaFiscal, txtNumeroSerial, txtDocumentoInterno, txtFechaFactura, txtRIF, txtNombreCliente, _
                txtSubTotalFactura, txtIVA, txtTotalFactura, txtFechaRetencion, txtFechaRecepcion, txtMontoRetencion, txtPorcentajeRetencion)

        txtPorcentajeRetencion.Text = ft.muestraCampoNumero(75)

    End Sub

    Private Sub jsPOSFacturaDevolucion_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
    End Sub

    Private Sub jsPOSFacturaDevolucion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
    End Sub


    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtNumeroRetencion.Text) = "" Then
            ft.mensajeCritico("Debe indicar un NUMERO DE RETENCION FISCAL VALIDO ...")
            txtNumeroRetencion.Focus()
            Return False
        End If

        If IsNumeric(txtPorcentajeRetencion.Text) Then

            If ValorNumero(txtPorcentajeRetencion.Text) = 75 Or _
                ValorNumero(txtPorcentajeRetencion.Text) = 100 Then
            Else
                ft.mensajeCritico("Debe indicar un PORCENTAJE DE RETENCION IVA válido (75 Ó 100)...")
                txtPorcentajeRetencion.Focus()
                Return False
            End If

        Else
            ft.mensajeCritico("Debe indicar un PORCENTAJE DE RETENCION IVA válido ...")
            txtPorcentajeRetencion.Focus()
            Return False
        End If

        If supervisorValido(MyConn) Then Return True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            'GUARDAR REGISTRO DE RETENCION
            InsertarModificarPOSWork(MyConn, lblInfo, True, jytsistema.WorkBox, jytsistema.sFechadeTrabajo, "PVE", "SA", txtDocumentoInterno.Text, _
                                      txtNumeroSerial.Text, "EF", txtNumeroRetencion.Text, "RETENCION-IVA", ValorNumero(txtMontoRetencion.Text), _
                                      jytsistema.sFechadeTrabajo, 1, jytsistema.sUsuario)

            ft.Ejecutar_strSQL(MyConn, " update jsvenencpos set " _
                & " NUMERO_RETENCION_IVA = '" & txtNumeroRetencion.Text & "', NOMBRE_RETENCION_IVA = 'RETENCION-IVA', " _
                & " MONTO_RETENCION_IVA = " & ValorNumero(txtMontoRetencion.Text) & ", " _
                & " FECHA_RETENCION_IVA = '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', FECHASI = '" & ft.FormatoFechaMySQL(Convert.ToDateTime(txtFechaRecepcion.Text)) & "' " _
                & " WHERE " _
                & " NUMFAC = '" & txtDocumentoInterno.Text & "' AND " _
                & " NUMSERIAL = '" & txtNumeroSerial.Text & "' AND " _
                & " REFER = '" & txtFacturaFiscal.Text & "' AND " _
                & " ID_EMP = '" & jytsistema.WorkID & "' ")

            'IMPRIMIR RECIBO DE EFECTIVO

            bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox))
            If bRet Then
                IB.RetiroDeCaja(ValorNumero(txtMontoRetencion.Text), 7)
                IB.cerrarPuerto()
            End If
            ft.mensajeInformativo("RETENCION IVA " & txtNumeroRetencion.Text & " PROCESADA")
            Me.Close()

        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numfac", "numserial", "refer", "rif", "nomcli"}
        Dim Nombres() As String = {"N° Factura Interna", "Numero Serial", "N° Factura Fiscal", "RIF", "Nombre y/o Razón Social"}
        Dim Anchos() As Integer = {110, 110, 110, 110, 350}
        f.Buscar(dtLocal, Campos, Nombres, Anchos, Me.BindingContext(dsLocal, nTabla).Position, "FACTURAS DE PUNTO DE VENTA...")
        AsignaTXT(f.Apuntador)
        f = Nothing
    End Sub

    Private Sub AsignaTXT(ByVal nRow As Long)
        If dtLocal.Rows.Count > 0 Then
            If nRow >= 0 Then
                With dtLocal.Rows(nRow)
                    txtDocumentoInterno.Text = ft.muestraCampoTexto(.Item("numfac"))
                    txtFacturaFiscal.Text = ft.muestraCampoTexto(.Item("refer"))
                    txtNumeroSerial.Text = ft.muestraCampoTexto(.Item("numserial"))
                    txtRIF.Text = ft.muestraCampoTexto(.Item("rif"))
                    txtNombreCliente.Text = ft.muestraCampoTexto(.Item("nomcli"))
                    txtFechaFactura.Text = ft.muestraCampoFecha(.Item("emision"))

                    txtSubTotalFactura.Text = ft.muestraCampoNumero(.Item("tot_net") + .Item("cargos") - .Item("descuen"))
                    txtIVA.Text = ft.muestraCampoNumero(.Item("imp_iva"))
                    txtTotalFactura.Text = ft.muestraCampoNumero(.Item("tot_fac"))

                End With

            End If
        End If
    End Sub

    Private Sub txtIVA_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtIVA.TextChanged, _
        txtPorcentajeRetencion.TextChanged
        txtMontoRetencion.Text = ft.muestraCampoNumero(ValorNumero(txtIVA.Text) * ValorNumero(txtPorcentajeRetencion.Text) / 100)
    End Sub

    Private Sub txtPorcentajeRetencion_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPorcentajeRetencion.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub btnFechaRetencion_Click(sender As System.Object, e As System.EventArgs) Handles btnFechaRetencion.Click
        txtFechaRetencion.Text = SeleccionaFecha(Convert.ToDateTime(txtFechaRetencion.Text), Me, grpRetencion, btnFechaRetencion)
    End Sub

    Private Sub txtNumeroRetencion_GotFocus(sender As Object, e As EventArgs) Handles txtNumeroRetencion.GotFocus, _
        txtPorcentajeRetencion.GotFocus
        ft.enfocarTexto(sender)
    End Sub

   
    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click

    End Sub
    Private Sub Label10_Click(sender As Object, e As EventArgs) Handles Label10.Click

    End Sub
    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click

    End Sub
End Class