Imports MySql.Data.MySqlClient
Public Class jsBanProCestaTicketDevuelto
    Private Const sModulo As String = "Devolución/Anulación/Eliminación canc.  de cesta ticket"
    Private Const nTabla As String = "tblct"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private ValorDebitado As Double = 0.0
    Private ValorTicket As Double = 0.0
    Private Enum MovimientoTipo
        Devolucion = 0
        Anulacion = 1
        Eliminacion = 2
    End Enum
    Private Movimiento As Integer
    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal TipoMovimiento As Integer)
        ' TipoMovimiento : 0 = devolucion; 1 = Anulacion; 2 = Elimina Cancelación 

        Me.Tag = sModulo
        myConn = MyCon
        Movimiento = TipoMovimiento

        IniciarCorredores()
        IniciarTxt()

        Select Case Movimiento
            Case MovimientoTipo.Devolucion
                ft.mensajeEtiqueta(lblInfo, "Seleccione el corredor e indique el cheque de alimentación a devolver, escoja la fecha de la devolución ... ", Transportables.TipoMensaje.iAyuda)
                lblLeyenda.Text = " Este proceso se aplica a un cheque de alimentación que es devuelto por el corredor una vez realizado el " + vbCr + _
                        " depósito de dicho cheque de alimentación. Genera una cuenta por cobrar al cliente, una basada " + vbCr + _
                        " en el monto del cheque alimentacion devuelto. Y, además coloca o registra el cheque de " + vbCr + _
                        " alimentación como falso o no procesable. "
                Label9.Text = "Bancos : Devolución Cesta Ticket"

            Case MovimientoTipo.Anulacion
                ft.mensajeEtiqueta(lblInfo, "Seleccione el corredor e indique el cheque de alimentación a anular o colocar como falso ... ", Transportables.TipoMensaje.iAyuda)
                lblLeyenda.Text = " Este proceso se aplica a un cheque de alimentación FALSO. " + vbCr + _
                        " Coloca o registra el cheque de alimentación como falso o no procesable. "
                OcultaTXTPorAnulacion()
                Label9.Text = "Bancos : Cesta Ticket falsificado"
            Case MovimientoTipo.Eliminacion
                ft.mensajeEtiqueta(lblInfo, "Seleccione el corredor e indique el cheque de alimentación que está en la cancelación a eliminar ... ", Transportables.TipoMensaje.iAyuda)
                lblLeyenda.Text = " Este proceso aplica para aquellas cancelaciones que se desean eliminar a partir de un cheque de " + vbCr + " alimentación. " + vbCr + _
                        " Elimina la cancelación y los tickets asociados a la misma. "
                OcultaTXTPorEliminacion()
                Label9.Text = "Bancos : Eliminar cancelación"
        End Select


        Me.Show()

    End Sub
    Private Sub IniciarTxt()

        Dim txt As Control
        For Each txt In grp.Controls
            If txt.GetType Is txtCheque.GetType Then
                txt.Text = ""
                If txt.Name = "txtCheque" Then
                    txt.Enabled = True
                Else
                    txt.Enabled = False
                End If
            End If

        Next
        txtFecha.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)

    End Sub
    Private Sub OcultaTXTPorAnulacion()

        ft.visualizarObjetos(False, txtFecha, btnFecha, btnImprimir, txtBanco, Label4, Label6, Label7, _
                                Label14, Label13, Label15, Label8, Label11, Label12, _
                                txtAsesor, txtCancelacion, txtCliente, txtDeposito, txtFechaDep, _
                                txtFechaRemesa, txtRemesa)
    End Sub
    Private Sub OcultaTXTPorEliminacion()

        ft.visualizarObjetos(False, txtFecha, btnFecha, btnImprimir, txtBanco, Label4, Label6, Label7, _
                                Label14, Label13, Label15, _
                                txtDeposito, txtFechaDep, txtFechaRemesa, txtRemesa)
    End Sub
    Private Sub IniciarCorredores()

        ColocaCorredoresCTEnCombo(myConn, cmbCorredor, lblInfo)

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub jsBanProCestaTicketDevuelto_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt.Dispose()
    End Sub

    Private Sub jsBanProCestaTicketDevuelto_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Function Validado() As Boolean

        If txtCheque.Text = "" Then
            ft.mensajeAdvertencia(" Debe indicar un número de cheque válido...")
            Return False
        End If

        If txtEstatus.Text <> "OK!" Then
            ft.mensajeAdvertencia("Esta cheque de alimentación YA ha sido PROCESADO...")
            Return False
        End If

        If txtRemesa.Text = "" AndAlso Movimiento = MovimientoTipo.Devolucion Then
            ft.mensajeAdvertencia("Este cheque de alimentación no pertenece a ninguna remesa...")
            Return False
        End If

        If txtCancelacion.Text = "" AndAlso (Movimiento = MovimientoTipo.Devolucion Or _
                                             Movimiento = MovimientoTipo.Eliminacion) Then
            ft.mensajeAdvertencia("Este cheque de alimentación no pertenece a ninguna cancelación...")
            Return False
        End If

        Return True

    End Function
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If Validado() Then
            GenerarProceso()
            ImprimirComprobante()
            IniciarTxt()
            ft.mensajeInformativo(IIf(Movimiento = MovimientoTipo.Devolucion, " DEVOLUCION ", IIf(Movimiento = MovimientoTipo.Anulacion, " ANULACION ", " ELIMINACION DE CANCELACION DE ")) _
                + "CHEQUE DE ALIMENTACION REALIZADA CON EXITO ...")
        End If

        ProgressBar1.Value = 0
        lblProgreso.Text = ""

    End Sub

    Private Sub GenerarProceso()

        Select Case Movimiento
            Case MovimientoTipo.Devolucion
                '1. Debito en CXC por el monto del cheque

                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 50, _
                                              "1. Débito en CXC por el monto menos la comisión ...")

                Dim Documento As String = Contador(myConn, lblInfo, Gestion.iVentas, "VENNUMNDB", "10")

                InsertEditVENTASCXC(myConn, lblInfo, True, Split(txtCliente.Text, "  |  ").GetValue(0).ToString, "ND", Documento, CDate(txtFecha.Text), ft.FormatoHora(Now()), _
                    CDate(txtFecha.Text), txtCheque.Text, "CHEQUE DE ALIMENTACION DEVUELTO", ValorDebitado, 0.0#, "", "", "", "", "", "BAN", _
                    txtCheque.Text, "", "", jytsistema.sFechadeTrabajo, "", "", "", 0.0#, 0.0#, "", "", "", "", Mid(txtAsesor.Text, 1, 5), _
                    Mid(txtAsesor.Text, 1, 5), "0", "0", jytsistema.WorkDivition)


                '2. Coloca el cheque de alimentacion devuelto
               
                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, "2. Coloca el cheque devuelto")
                ft.Ejecutar_strSQL(myconn, " update jsventabtic set condicion = 1 " _
                                & " where  " _
                                & " ticket = '" & txtCheque.Text & "' and  " _
                                & " corredor = '" & Mid(cmbCorredor.Text, 1, 5) & "' ")

            Case MovimientoTipo.Anulacion

                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, "1. Coloca el cheque como falso")
                ft.Ejecutar_strSQL(myconn, " update jsventabtic set falso = 1 " _
                                & " where  " _
                                & " ticket = '" & txtCheque.Text & "' and  " _
                                & " corredor = '" & Mid(cmbCorredor.Text, 1, 5) & "' ")

            Case MovimientoTipo.Eliminacion

                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 33, "1. Eliminando tickets de cancelación")
                ft.Ejecutar_strSQL(myConn, " DELETE FROM jsventabtic WHERE   " _
                    & " NUMCAN = '" & txtCancelacion.Text & "' AND " _
                    & " FALSO = 0 ")

                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 66, "2. Eliminando movimientos de caja ")
                ft.Ejecutar_strSQL(myconn, " DELETE FROM jsbantracaj WHERE " _
                    & " FORMPAG = 'CT' AND  " _
                    & " NUMMOV = '" & txtCancelacion.Text & "' AND " _
                    & " TIPOMOV = 'EN' ")

                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, "3. Eliminando la cuenta por cobrar ")
                ft.Ejecutar_strSQL(myconn, " delete from jsventracob WHERE " _
                    & " COMPROBA = '" & txtCancelacion.Text & "' AND " _
                    & " FORMAPAG = 'CT' ")

        End Select

    End Sub

    Private Sub ImprimirComprobante()
        If Movimiento = MovimientoTipo.Devolucion OrElse Movimiento = MovimientoTipo.Anulacion Then
            Dim f As New jsBanRepParametros
            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cTicketDevuelto, "Devolución de Cheque de Alimentación", txtCheque.Text)
            f = Nothing
        End If
    End Sub

    Private Sub btnCheque_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheque.Click
        Dim dtTicket As DataTable
        Dim dtCRCO As DataTable
        Dim tblTick As String = "tbltick"
        Dim strCRCO As String = ""
        Dim tblCRCO As String = "tblCRCO"

        ds = DataSetRequery(ds, " select * from jsventabtic where TICKET = '" & txtCheque.Text & "' and corredor = '" & Mid(cmbCorredor.Text, 1, 5) & "' ", _
                             myConn, tblTick, lblInfo)

        dtTicket = ds.Tables(tblTick)
        If dtTicket.Rows.Count > 0 Then
            With dtTicket
                txtMonto.Text = ft.FormatoNumero(ValorNumero(.Rows(0).Item("monto").ToString))
                Dim aCBan() As String = {"codban", "id_emp"}
                Dim aSBan() As String = {.Rows(0).Item("bancodep").ToString, jytsistema.WorkID}
                txtBanco.Text = .Rows(0).Item("bancodep").ToString + "  |  " + qFoundAndSign(myConn, lblInfo, "jsbancatban", aCBan, aSBan, "nomban") + "  |  " + qFoundAndSign(myConn, lblInfo, "jsbancatban", aCBan, aSBan, "ctaban")
                txtDeposito.Text = .Rows(0).Item("numdep")
                txtFechaDep.Text = ft.FormatoFecha(CDate(.Rows(0).Item("fechadep").ToString))
                txtRemesa.Text = .Rows(0).Item("numsobre")
                txtFechaRemesa.Text = ft.FormatoFecha(CDate(IIf(.Rows(0).Item("fechasobre").ToString = "00/00/0000", jytsistema.sFechadeTrabajo, .Rows(0).Item("fechasobre").ToString)))
                txtCancelacion.Text = .Rows(0).Item("numcan")
                ValorTicket = CDbl(.Rows(0).Item("monto"))
                ValorDebitado = CDbl(.Rows(0).Item("monto")) - CDbl(.Rows(0).Item("comision"))

                If CInt(.Rows(0).Item("condicion").ToString) = 1 Then
                    txtEstatus.Text = "Devuelto"
                Else
                    txtEstatus.Text = "OK!"
                End If

                If Mid(.Rows(0).Item("numcan"), 1, 2) = "CA" Then 'CREDITO

                    strCRCO = " select a.CODCLI, b.NOMBRE, a.NUMMOV, a.REFER, a.COMPROBA, a.codven VENDEDOR " _
                        & " from jsventracob a, jsvencatcli b where " _
                        & " a.CODCLI = b.CODCLI AND " _
                        & " a.COMPROBA = '" & .Rows(0).Item("numcan").ToString & "' AND " _
                        & " a.ID_EMP = b.ID_EMP and " _
                        & " a.ID_EMP = '" & jytsistema.WorkID & "' "

                Else 'CONTADO

                    strCRCO = " SELECT a.CODCLI, b.NOMBRE, a.codven VENDEDOR " _
                        & " from jsvenencfac a, jsvencatcli b where " _
                        & " a.CODCLI = b.CODCLI AND  " _
                        & " a.NUMFAC = '" & .Rows(0).Item("numcan").ToString & "' AND " _
                        & " a.ID_EMP = b.ID_EMP  AND " _
                        & " a.ID_EMP = '" & jytsistema.WorkID & "' "

                End If
                ds = DataSetRequery(ds, strCRCO, myConn, tblCRCO, lblInfo)

                dtCRCO = ds.Tables(tblCRCO)
                If dtCRCO.Rows.Count > 0 Then
                    With dtCRCO
                        txtCliente.Text = .Rows(0).Item("codcli").ToString + "  |  " + .Rows(0).Item("nombre").ToString
                        txtAsesor.Text = .Rows(0).Item("vendedor").ToString
                    End With
                End If

                dtCRCO.Dispose()
                dtCRCO = Nothing

            End With

        Else
            ft.MensajeCritico("TICKET NO PERTENECE A NINGUNA CANCELACION O PROVEEDOR NO VALIDO")
            IniciarTxt()
        End If

        dtTicket.Dispose()
        dtTicket = Nothing

        ft.enfocarTexto(txtCheque)

    End Sub

    Private Sub btnFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFecha.Click
        txtFecha.Text = SeleccionaFecha(CDate(txtFecha.Text), Me, btnFecha)
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        ImprimirComprobante()
    End Sub

    Private Sub txtEstatus_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEstatus.TextChanged
        If txtEstatus.Text = "Devuelto" Then
            btnImprimir.Enabled = True
        Else
            btnImprimir.Enabled = False
        End If
    End Sub
End Class