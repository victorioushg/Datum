Imports MySql.Data.MySqlClient
Imports FP_AclasBixolon
Public Class jsPOSProConfigurarAclas
    Private Const sModulo As String = "Configuración impresora pp1f3"
    Private Const nTabla As String = "tbLConfigAclas"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As DataTable
    Private ft As New Transportables

    Private bRet As Boolean
    Private iStatus As Long
    Private iError As Long

    Private IB As New AclasBixolon
    Private puerto As String = "COM1"
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        myConn = MyCon


        lblLeyenda.Text = " Este proceso configura la impresora tipo PP1F3 de la siguiente manera : " + vbCr + _
                "A.- Configuración de formas de pago. " + vbCr + _
                "   1. EFECTIVO, 2. CHEQUE, 3. TARJETA, 4. CUPON,  " + vbCr + _
                "   5. DEPOSITO, 6. TRANSFERENCIA, 7. RETENCION IVA" + vbCr + _
                "   8. CTRL EFECTIVO, 16. TOTAL "

        IniciarTXT()

        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()
        ft.habilitarObjetos(False, True, txtCaja, txtCodigoCajero, txtNombreCajero, txtUltimaFactura, _
                            txtUltimaND, txtUltimaNC, txtUltimaNF, txtRegistroFiscal)
        txtCodigoCajero.Text = jytsistema.sUsuario
        txtNombreCajero.Text = jytsistema.sNombreUsuario
        txtCaja.Text = jytsistema.WorkBox
        lblImpresora.Text = ft.DevuelveScalarCadena(myConn, "  SELECT CONCAT(b.codigo, '  | Serial : ', b.maquinafiscal, '  | Puerto : ' , b.puerto) " _
                                                  & " FROM jsvencatcaj a " _
                                                  & " LEFT JOIN jsconcatimpfis b ON (a.impre_fiscal = b.codigo AND a.id_emp = b.id_emp ) " _
                                                  & " WHERE " _
                                                  & " a.codcaj = '" & jytsistema.WorkBox & "' AND " _
                                                  & " a.id_emp = '" & jytsistema.WorkID & "'")

        puerto = PuertoImpresoraFiscal(myConn, lblInfo, jytsistema.WorkBox)

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btnDatosImpresora.Click
        bRet = IB.abrirPuerto(puerto)
        If bRet Then
            If TipoImpresoraFiscal(myConn, jytsistema.WorkBox) = 7 Then
                txtUltimaFactura.Text = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.FC_SRP812)
                txtUltimaND.Text = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.ND_SRP812)
                txtUltimaNC.Text = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.NC_SRP812)
                txtUltimaNF.Text = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.NF_SRP812)
                txtRegistroFiscal.Text = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.NR_SRP812)
            Else
                txtUltimaFactura.Text = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.Factura)
                txtUltimaND.Text = ""
                txtUltimaNC.Text = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.NotaCredito)
                txtUltimaNF.Text = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.Nofiscal)
                txtRegistroFiscal.Text = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.numRegistro)
            End If
            IB.cerrarPuerto()
        End If

    End Sub

    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles btnAnular.Click
        bRet = IB.abrirPuerto(puerto)
        If bRet Then
            bRet = IB.AnularDocumento()
            IB.cerrarPuerto()
        End If
    End Sub

    Private Sub btnReset_Click(sender As System.Object, e As System.EventArgs) Handles btnReset.Click
        bRet = IB.abrirPuerto(puerto)
        If bRet Then
            bRet = IB.reiniciarImpresora()
            IB.cerrarPuerto()
        End If

        'Imprimir_NC_SRP812(myConn, lblInfo, "PVD0001205", "JESUS RICARDO GUEDEZ MITTILO", "V-16127613-", "", _
        '                   "", jytsistema.sFechadeTrabajo, "0000001499", "CONTADO", jytsistema.sFechadeTrabajo, _
        '                   "00012", "", 133.55, "00001957", "Z1F0001912", "")

    End Sub

    Private Sub btnProgramarPagos_Click(sender As System.Object, e As System.EventArgs) Handles btnProgramarPagos.Click
        bRet = IB.abrirPuerto(puerto)
        If bRet Then
            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iNombrePago, 1, "EFECTIVO")
            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iNombrePago, 2, "CHEQUE")
            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iNombrePago, 3, "TARJETA")
            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iNombrePago, 4, "CUPON")
            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iNombrePago, 5, "DEPOSITO")
            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iNombrePago, 6, "TRANSFERENCIA")
            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iNombrePago, 7, "RETENCION IVA")
            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iNombrePago, 8, "CTRL EFECTIVO")
            bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iNombrePago, 16, "TOTAL")
            IB.cerrarPuerto()
            ft.mensajeInformativo(" Configuración Formas de PAGO de Impresora  REALIZADA ...")
        End If
    End Sub

    Private Sub btnImprimirProgramacion_Click(sender As System.Object, e As System.EventArgs) Handles btnImprimirProgramacion.Click
        Dim Respuesta As Microsoft.VisualBasic.MsgBoxResult
        Respuesta = MsgBox(" ¿Desea Imprimir programación Impresora Fiscal?", MsgBoxStyle.YesNo, "Imprimir ... ")
        If Respuesta = MsgBoxResult.Yes Then
            bRet = IB.abrirPuerto(puerto)
            If bRet Then
                bRet = IB.ImprimirProgramacion()
                IB.cerrarPuerto()
            End If
        End If

    End Sub

    Private Sub btnFinalizarFactura_Click(sender As System.Object, e As System.EventArgs) Handles btnFinalizarFactura.Click
        bRet = IB.abrirPuerto(puerto)
        If bRet Then
            bRet = IB.FinalizarFactura()
            IB.cerrarPuerto()
        End If
    End Sub
End Class