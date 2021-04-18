Imports MySql.Data.MySqlClient
Imports FP_AclasBixolon
Public Class jsPOSRetirosEfectivo

    Private Const sModulo As String = "Retiros de Efectivo de Punto de Venta"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As New DataTable
    Private dtRetiros As New DataTable
    Private ft As New Transportables

    Private nTablaDenominacion As String = "tblDenominacion"
    Private nTabla As String = "tblRetiros"
    Private strSQL As String = ""
    Private strSQLDenominacion As String = ""

    Private NumCaja As String = ""
    Private nCodigoInterno As String = ""
    Private nPosicion As Long = 0

    Private nDocumentoNoFiscal As String = ""

    Private IB As New AclasBixolon
    Private puerto As String = "COM1"
    Private bRet As Boolean
    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal CodigoCaja As String)

        MyConn = MyCon
        dsLocal = ds
        NumCaja = CodigoCaja
        IniciarTXT()
        Me.ShowDialog()

    End Sub

    Private Sub IniciarTXT()

        nCodigoInterno = Contador(MyConn, lblInfo, Gestion.iPuntosdeVentas, "POSNUMRET", "05")      ' ft.autoCodigo(MyConn, "NUM_CONTROL_INTERNO", "jsconregistroefectivo", "id_emp", jytsistema.WorkID, 10, True)

        txtNumeroControlInterno.Text = nCodigoInterno

        strSQLDenominacion = " SELECT a.* " _
                & " FROM jsconctatab a " _
                & " WHERE " _
                & " a.modulo = '" & FormatoTablaSimple(Modulo.iDenominacionesMonetarias) & "' and " _
                & " a.id_emp = '" & jytsistema.WorkID & "' ORDER BY a.codigo DESC "

        dtLocal = ft.AbrirDataTable(dsLocal, nTablaDenominacion, MyConn, strSQLDenominacion)
      
        ft.habilitarObjetos(False, True, txtNumeroControlInterno, txtMontoRetiro)
        Dim fechaRecuento As String = ft.FormatoFechaHoraMySQL(jytsistema.sFechadeTrabajo)
        For Each nRow As DataRow In dtLocal.Rows
            With nRow
                ft.Ejecutar_strSQL(MyConn, " insert into jsconregistroefectivo " _
                               & " set CAJA = '" & NumCaja & "', CAJERO = '" & jytsistema.sUsuario & "', " _
                               & " FECHA = '" & fechaRecuento & "', " _
                               & " NUM_CONTROL_INTERNO = '" & nCodigoInterno & "', " _
                               & " NUM_CONTROL_FISCAL = '', " _
                               & " MONTO = '" & .Item("DESCRIP") & "' , CANTIDAD = 0,  ID_EMP = '" & jytsistema.WorkID & "'")
            End With
        Next

        IniciarGrilla()

    End Sub

    Private Sub IniciarGrilla()

        strSQL = " select * from jsconregistroefectivo where num_control_interno = '" & nCodigoInterno & "' and " _
            & " id_emp = '" & jytsistema.WorkID & "'  ORDER BY LPAD(monto,5, '0') "
        dtRetiros = ft.AbrirDataTable(dsLocal, nTabla, MyConn, strSQL)


        Dim aCam() As String = {"MONTO.DENOMINACION.150.C.Entero",
                                "CANTIDAD.CANTIDAD.150.C.Entero"}

        ft.IniciarTablaPlus(dg, dtRetiros, aCam, True, True, New Font("Consolas", 12, FontStyle.Regular), True)
        If dtRetiros.Rows.Count > 0 Then nPosicion = 0

        dg.ReadOnly = False
        dg.Columns("MONTO").ReadOnly = True

    End Sub
    Private Sub CalculaTotales()
        Dim nTotal As Double = 0
        For Each nRow As DataRow In dtRetiros.Rows
            With nRow
                nTotal += ValorNumero(.Item("MONTO")) * ValorNumero(.Item("CANTIDAD"))
            End With
        Next
        txtMontoRetiro.Text = ft.muestraCampoNumero(nTotal)

    End Sub
    Private Sub dg_CellFormatting(sender As Object, e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dg.CellFormatting
        If e.ColumnIndex = 1 Then
            e.Value = ft.muestraCampoEntero(e.Value)
        End If
    End Sub
    Private Sub dgTabla_CancelRowEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.QuestionEventArgs) Handles dg.CancelRowEdit
        ft.mensajeAdvertencia("Cancelando")
    End Sub
    Private Sub dgTabla_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellValidated
        If dg.CurrentCell.ColumnIndex = 1 Then
            If IsNumeric(dg.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then
                Dim iCANTIDAD As Integer = Convert.ToInt32(dg.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString)
                ft.Ejecutar_strSQL(MyConn, " update jsconregistroefectivo " _
                                    & " set CANTIDAD = " & iCANTIDAD & " " _
                                    & " where " _
                                    & " CAJA = '" & NumCaja & "' AND " _
                                    & " CAJERO = '" & jytsistema.sUsuario & "' AND " _
                                    & " NUM_CONTROL_INTERNO = '" & nCodigoInterno & "' AND " _
                                    & " MONTO = '" & dg.Rows(e.RowIndex).Cells(0).Value.ToString & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "'")

                dsLocal = DataSetRequery(dsLocal, strSQL, MyConn, nTabla, lblInfo)
                dtRetiros = dsLocal.Tables(nTabla)

                dg.Refresh()
                dg.CurrentCell = dg(0, e.RowIndex)

                CalculaTotales()

            End If

        End If



    End Sub

    Private Sub dgTabla_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
            Handles dg.CellValidating

        If Not String.IsNullOrEmpty(dg.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then
            If e.ColumnIndex = 1 Then
                'If e.FormattedValue.ToString().Trim() = "" Then
                '    ft.mensajeCritico("DEBE INDICAR UNA CANTIDAD VALIDA...")
                '    e.Cancel = True
                'Else
                If Not IsNumeric(e.FormattedValue.ToString) Then
                    ft.mensajeCritico("DEBE INDICAR UNA CANTIDAD VALIDA...")
                    e.Cancel = True
                End If
                'End If
            End If
        End If
    End Sub

    Private Sub dg_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dg.CellEndEdit
        dg.Rows(e.RowIndex).ErrorText = String.Empty
    End Sub


    Private Sub jsPOSFacturaDevolucion_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
    End Sub

    Private Sub jsPOSFacturaDevolucion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
    End Sub


    Private Function Validado() As Boolean
        Validado = False
        If supervisorValido(MyConn) Then Return True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then

            Dim numSerialFiscal As String = ""
            'IMPRIMIR RECIBO DE EFECTIVO

            ImprimirReciboEfectivoCajera(TipoImpresoraFiscal(MyConn, jytsistema.WorkBox))
            nDocumentoNoFiscal = ""
            Select Case TipoImpresoraFiscal(MyConn, jytsistema.WorkBox)
                Case 2, 5, 6
                    bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox))
                    If bRet Then
                        nDocumentoNoFiscal = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.Nofiscal)
                        numSerialFiscal = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.numRegistro)
                        IB.cerrarPuerto()
                    End If
                Case 7
                    bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox))
                    If bRet Then
                        nDocumentoNoFiscal = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.NF_SRP812)
                        numSerialFiscal = IB.UltimoDocumentoFiscal(AclasBixolon.tipoDocumentoFiscal.NR_SRP812)
                        IB.cerrarPuerto()
                    End If
            End Select

            'ACTUALIZAR NUMERO DOCUMENTO NO FISCAL EN RETIRO
            ft.Ejecutar_strSQL(MyConn, " update jsconregistroefectivo " _
                                & " set NUM_CONTROL_FISCAL = '" & nDocumentoNoFiscal & "' " _
                                & " where " _
                                & " CAJA = '" & NumCaja & "' AND " _
                                & " CAJERO = '" & jytsistema.sUsuario & "' AND " _
                                & " NUM_CONTROL_INTERNO = '" & txtNumeroControlInterno.Text & "' AND " _
                                & " id_emp = '" & jytsistema.WorkID & "'")

            'ELIMINA LOS DE CANTIDAD CERO(0)
            ft.Ejecutar_strSQL(MyConn, " DELETE FROM jsconregistroefectivo " _
                              & " WHERE NUM_CONTROL_FISCAL = '" & nDocumentoNoFiscal & "' AND " _
                              & " CAJA = '" & NumCaja & "' AND " _
                              & " CAJERO = '" & jytsistema.sUsuario & "' AND " _
                              & " NUM_CONTROL_INTERNO = '" & txtNumeroControlInterno.Text & "' AND " _
                              & " CANTIDAD = 0 AND " _
                              & " id_emp = '" & jytsistema.WorkID & "'")


            'GUARDAR REGISTRO DE RETENCION
            InsertarModificarPOSWork(MyConn, lblInfo, True, jytsistema.WorkBox, jytsistema.sFechadeTrabajo, "PVE", "SA", nCodigoInterno, _
                                     numSerialFiscal, _
                                    "EF", nDocumentoNoFiscal, "CTRL EFECTIVO", ValorNumero(txtMontoRetiro.Text), _
                                     jytsistema.sFechadeTrabajo, 1, jytsistema.sUsuario)

            'IMPRIMIR DESGLOSE DE RETIRO  
            ft.mensajeInformativo("RETIRO EFECTIVO N° " & nCodigoInterno & " PROCESADO")
            ImprimirDesgloseEfectivo(TipoImpresoraFiscal(MyConn, jytsistema.WorkBox))

            Me.Close()

        End If
    End Sub
    Private Sub ImprimirReciboEfectivoCajera(Impresora As Integer)
        Select Case Impresora
            Case 2, 5, 6, 7
                bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox))
                If bRet Then
                    IB.RetiroDeCaja(ValorNumero(txtMontoRetiro.Text), 8)
                    IB.cerrarPuerto()
                End If
            Case Else
        End Select
    End Sub
    Private Sub ImprimirDesgloseEfectivo(Impresora As Integer)
        dsLocal = DataSetRequery(dsLocal, strSQL, MyConn, nTabla, lblInfo)
        dtRetiros = dsLocal.Tables(nTabla)
        Dim nCount As Integer = 0
        If dtRetiros.Rows.Count > 0 Then
            Select Case Impresora
                Case 2, 5, 6, 7
                    bRet = IB.abrirPuerto(PuertoImpresoraFiscal(MyConn, lblInfo, jytsistema.WorkBox))
                    If bRet Then
                        For Each nRow As DataRow In dtRetiros.Rows
                            With nRow
                                If nCount = 0 Then
                                    bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iLineaNoFiscal, 0, "N° INTERNO : " & .Item("NUM_CONTROL_INTERNO"))
                                    bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iLineaNoFiscal, 0, "N° FISCAL  : " & .Item("NUM_CONTROL_FISCAL"))
                                    bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iLineaNoFiscal, 0, "FECHA      : " & ft.muestraCampoFecha(Convert.ToDateTime(.Item("FECHA").ToString)))
                                    bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iLineaNoFiscal, 0, "CAJERO     : " & .Item("CAJERO"))
                                    bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iLineaNoFiscal, 0, "CAJA       : " & .Item("CAJA"))
                                    bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iLineaNoFiscal, 0, "---MONTO  ---CANTIDAD ")
                                End If
                                Dim sMonto As String = RellenaCadenaConCaracter(.Item("MONTO").ToString, "D", 8)
                                Dim sCantidad As String = RellenaCadenaConCaracter(.Item("CANTIDAD").ToString, "D", 11)
                                If nCount = dtRetiros.Rows.Count - 1 Then
                                    bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iLineaNoFiscalCierre, 0, sMonto & "  " & sCantidad)
                                Else
                                    bRet = IB.enviarComando(AclasBixolon.ComandoFiscalAclasBixolon.iLineaNoFiscal, 0, sMonto & "  " & sCantidad)
                                End If
                                nCount += 1
                            End With
                        Next
                        IB.cerrarPuerto()
                    End If
                Case Else

            End Select

        End If
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        ft.Ejecutar_strSQL(MyConn, " DELETE FROM jsconregistroefectivo " _
                             & " WHERE NUM_CONTROL_FISCAL = '" & nDocumentoNoFiscal & "' AND " _
                             & " CAJA = '" & NumCaja & "' AND " _
                             & " CAJERO = '" & jytsistema.sUsuario & "' AND " _
                             & " id_emp = '" & jytsistema.WorkID & "' ")

        Me.Close()
    End Sub


End Class