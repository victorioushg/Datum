Imports MySql.Data.MySqlClient
Public Class jsPOSProReprocesarmercancias
    Private Const sModulo As String = "Reprocesar mercancías puntos de ventas"
    Private Const nTabla As String = "tblReproPVE"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        myConn = MyCon
        Me.Tag = sModulo

        txtFechaDesde.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtFechaHasta.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)

        ft.mensajeEtiqueta(lblInfo, "Seleccione fecha para el proceso ... ", Transportables.TipoMensaje.iAyuda)
        Me.ShowDialog()


    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub jsPOSProReprocesarMercancias_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsPOSProReprocesarMercancias_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ft.habilitarObjetos(True, True, txtFacturaAnterior)
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Reprocesar()
        ft.mensajeInformativo(" Proceso culminado ...")

        ProgressBar1.Value = 0
        lblProgreso.Text = ""

    End Sub
    Private Sub EliminarMovimientos(ByVal MyConn As MySqlConnection, ByVal NumeroDocumento As String)
        Dim strP As String = lblProgreso.Text
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " Eliminando documento... ")
        EliminarMovimientosdeInventario(MyConn, NumeroDocumento, "PVE", lblInfo, NumeroDocumento)
        lblProgreso.Text = strP

    End Sub
    Private Sub Reprocesar()

        Dim hCont As Integer

        ft.Ejecutar_strSQL(myconn, " delete from jsvenencpos where substring(numfac,1,5) = 'TMPFC' ")
        ft.Ejecutar_strSQL(myconn, " delete from jsvenrenpos where substring(numfac,1,5) = 'TMPFC' ")
        ft.Ejecutar_strSQL(myconn, " delete from jsvenivapos where substring(numfac,1,5) = 'TMPFC' ")

        ds = DataSetRequery(ds, " select * from jsvenencpos where emision >= '" & ft.FormatoFechaMySQL(CDate(txtFechaDesde.Text)) & "' and " _
                            & " emision <= '" & ft.FormatoFechaMySQL(CDate(txtFechaHasta.Text)) & "' and id_emp = '" & jytsistema.WorkID & "' ", myConn, nTabla, lblInfo)

        dt = ds.Tables(nTabla)
        If dt.Rows.Count > 0 Then
            For hCont = 0 To dt.Rows.Count - 1
                With dt.Rows(hCont)
                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt((hCont + 1) / dt.Rows.Count * 100), _
                                                  " Factura : " & .Item("numfac").ToString)

                    ProcesarDocumento(myConn, .Item("numfac"), _
                                      CDate(.Item("emision").ToString), .Item("codcli"), .Item("codcaj"), .Item("codven"))
                    ProcesarCaja(myConn, .Item("numfac"), .Item("numserial"), .Item("codcli"), .Item("codven"), CDate(.Item("emision").ToString))
                End With
            Next
        Else
            ft.MensajeCritico("No existen movimientos para procesar...")
        End If

    End Sub
    Private Sub ProcesarDocumento(ByVal MyConn As MySqlConnection, ByVal NumeroDocumento As String, ByVal FechaMovimiento As Date, _
                                   ByVal CodigoCliente As String, ByVal CodigoCaja As String, ByVal CodigoCajero As String)

        EliminarMovimientos(MyConn, NumeroDocumento)

        Dim dtRenglones As DataTable
        Dim nTableRen As String = "tblRenglones"

        ds = DataSetRequery(ds, " select * from jsvenrenpos where numfac = '" & NumeroDocumento & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTableRen, lblInfo)
        dtRenglones = ds.Tables(nTableRen)

        If dtRenglones.Rows.Count > 0 Then
            Dim fCont As Integer
            For fCont = 0 To dtRenglones.Rows.Count - 1
                With dtRenglones.Rows(fCont)
                    Dim CostoTotal As Double = UltimoCostoAFecha(MyConn, .Item("item"), FechaMovimiento) / Equivalencia(MyConn, .Item("item"), .Item("unidad"))
                    Dim aAlmacen As String = ft.DevuelveScalarCadena(MyConn, " select almacen from jsvencatcaj where codcaj = '" & CodigoCaja & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If aAlmacen = "0" Then aAlmacen = "00001"

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, _
                                                  " Factura : " & .Item("numfac") & " ítem ::: " & .Item("item") & " " & .Item("descrip"))

                    InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, .Item("item"), _
                                                          FechaMovimiento, IIf(.Item("tipo") = 0, "SA", "EN"), _
                                                          NumeroDocumento, .Item("unidad"), .Item("cantidad"), .Item("peso"), _
                                                          CostoTotal * .Item("cantidad"), CostoTotal * .Item("cantidad"), _
                                                          "PVE", NumeroDocumento, "", CodigoCliente, .Item("totren"), .Item("totrendes"), _
                                                          0.0, .Item("totren") - .Item("totrendes"), CodigoCajero, aAlmacen, _
                                                          .Item("renglon") & .Item("estatus"), jytsistema.sFechadeTrabajo)

                End With
            Next
        End If

    End Sub


    Private Sub ProcesarCaja(ByVal MyConn As MySqlConnection, ByVal NumeroFactura As String, _
                             ByVal NumeroSerial As String, _
                             ByVal CodigoCliente As String, _
                             ByVal CodigoVendedor As String, ByVal FechaFactura As Date)

        Dim CajaPrincipal As String = "00"

        '1. ELIMINA MOVIMIENTOS ANTERIORES EN CAJA Y BANCOS
        'ft.Ejecutar_strSQL ( myconn, "DELETE FROM jsbantracaj WHERE origen = 'PVE' and nummov = '" & NumeroFactura & "' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "'")
        ft.Ejecutar_strSQL(myconn, "DELETE FROM jsbantraban WHERE origen = 'PVE' and TIPOMOV = 'DP' AND NUMORG = '" & NumeroFactura & "' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")

        '2. INSERTA EN CAJA TODOS LOS PAGOS EN EFECTIVO, CHEQUE, TARJETAS
        Dim dtPagosEFCHTA As DataTable
        Dim nTableEFCHTA As String = "tblEFCHTA"

        ds = DataSetRequery(ds, " select * from jsvenforpag where formapag in ('EF', 'CH', 'TA') and numfac = '" & NumeroFactura & "' and numserial = '" & NumeroSerial & "' and id_emp = '" & jytsistema.WorkID & "'  ", MyConn, nTableEFCHTA, lblInfo)

        dtPagosEFCHTA = ds.Tables(nTableEFCHTA)
        If dtPagosEFCHTA.Rows.Count > 0 Then
            Dim kCont As Integer
            For kCont = 0 To dtPagosEFCHTA.Rows.Count - 1
                With dtPagosEFCHTA.Rows(kCont)

                    If ft.DevuelveScalarCadena(MyConn, " select nummov from jsbantracaj where caja = '" & CajaPrincipal _
                                             & "' and fecha = '" & ft.FormatoFechaMySQL(FechaFactura) & "' and origen = 'PVE' and nummov = '" & NumeroFactura _
                                             & "' and formpag = '" & .Item("formapag") & "' and numpag = '" & .Item("numpag") _
                                             & "' and refpag = '" & .Item("nompag") _
                                             & "' AND DEPOSITO = '' and id_emp = '" & jytsistema.WorkID & "'  ") = "0" Then _
                            InsertEditBANCOSRenglonCaja(MyConn, lblInfo, True, CajaPrincipal, UltimoCajaMasUno(MyConn, lblInfo, CajaPrincipal), FechaFactura, _
                                   "PVE", IIf(.Item("importe") <= 0, "SA", "EN"), NumeroFactura, .Item("formapag"), .Item("numpag"), .Item("nompag"), _
                                    .Item("importe"), "", IIf(.Item("importe") <= 0, "NOTA CREDITO N° ", "FACTURA N° ") & NumeroFactura, "", jytsistema.sFechadeTrabajo, 1, "", "0", "", jytsistema.sFechadeTrabajo, _
                                     CodigoCliente, CodigoVendedor, "1")

                End With
            Next
        End If


        Dim nTablaCT As String = "tblPagoCT"
        Dim dtPagosCT As DataTable

        ds = DataSetRequery(ds, " select a.numfac, a.origen, a.formapag, a.numpag, a.nompag, b.importe, a.vence, a.id_emp, b.cantidad " _
                            & " from jsvenforpag a " _
                            & " left join (select numcan, corredor, IFNULL(COUNT(*),0) cantidad, sum(monto) importe, id_emp from jsventabtic WHERE " _
                            & "             ID_EMP = '" & jytsistema.WorkID & "' AND " _
                            & "             NUMCAN = '" & NumeroFactura & "' group by numcan, corredor ) b on (a.numfac = b.numcan and a.id_emp = b.id_emp ) " _
                            & " where formapag in ('CT') and " _
                            & " a.numfac = '" & NumeroFactura & "' and " _
                            & " a.id_emp = '" & jytsistema.WorkID & "'  ", MyConn, nTablaCT, lblInfo)

        dtPagosCT = ds.Tables(nTablaCT)
        If dtPagosCT.Rows.Count > 0 Then
            Dim hCont As Integer
            For hCont = 0 To dtPagosCT.Rows.Count - 1
                With dtPagosCT.Rows(hCont)
                    If ft.DevuelveScalarCadena(MyConn, " select nummov from jsbantracaj where caja = '" & CajaPrincipal _
                                            & "' and fecha = '" & ft.FormatoFechaMySQL(FechaFactura) & "' and origen = 'PVE' and nummov = '" & NumeroFactura _
                                            & "' and formpag = 'CT' and numpag = '" & .Item("numpag") _
                                            & "' and refpag = '" & .Item("nompag") _
                                            & "' AND DEPOSITO = '' and id_emp = '" & jytsistema.WorkID & "'  ") = "0" Then _
                    InsertEditBANCOSRenglonCaja(MyConn, lblInfo, True, CajaPrincipal, UltimoCajaMasUno(MyConn, lblInfo, CajaPrincipal), _
                               FechaFactura, "PVE", "EN", NumeroFactura, "CT", _
                               .Item("numpag"), .Item("nompag"), .Item("importe"), "", "FACTURA N° " & NumeroFactura, _
                               "", jytsistema.sFechadeTrabajo, 0, "", "0", "", jytsistema.sFechadeTrabajo, _
                               CodigoCliente, CodigoVendedor, "1")

                End With
            Next
        End If

        Dim nTablaDPTR As String = "tblDPTR"
        Dim dtPagosDPTR As DataTable

        ds = DataSetRequery(ds, " select * from jsvenforpag where formapag in('DP', 'TR') and numfac = '" & NumeroFactura & "' and numserial = '" & NumeroSerial & "' and id_emp = '" & jytsistema.WorkID & "'  ", MyConn, nTablaDPTR, lblInfo)
        dtPagosDPTR = ds.Tables(nTablaDPTR)
        If dtPagosDPTR.Rows.Count Then
            Dim gCont As Integer
            For gCont = 0 To dtPagosDPTR.Rows.Count - 1
                With dtPagosDPTR.Rows(gCont)
                    InsertEditBANCOSMovimientoBanco(MyConn, lblInfo, True, FechaFactura, .Item("numpag"), "DP", .Item("nompag"), "", "CANC. FACTURA N° " & NumeroFactura, _
                                             .Item("importe"), "PVE", NumeroFactura, "", "", "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, _
                                             "FC", "", jytsistema.sFechadeTrabajo, "0", CodigoCliente, CodigoVendedor)
                End With
            Next
        End If

        dtPagosEFCHTA.Dispose()
        dtPagosCT.Dispose()
        dtPagosDPTR.Dispose()
        dtPagosEFCHTA = Nothing
        dtPagosCT = Nothing
        dtPagosDPTR = Nothing

    End Sub


    Private Sub btnFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaDesde.Click
        txtFechaDesde.Text = SeleccionaFecha(CDate(txtFechaDesde.Text), Me, grpCaja, btnFechaDesde)
    End Sub

    Private Sub btnFechaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaHasta.Click
        txtFechaHasta.Text = SeleccionaFecha(CDate(txtFechaHasta.Text), Me, grpCaja, btnFechaHasta)
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If jytsistema.sUsuario = "00000" Then
            If txtFacturaAnterior.Text <> "" And txtFacturaActual.Text <> "" Then


                Dim numFacturaReal As String = txtFacturaActual.Text
                Dim numFactura As String = txtFacturaAnterior.Text
                Dim codCaja As String = ft.DevuelveScalarCadena(myConn, "select codcaj from jsvenencpos where numfac = '" & numFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")
                Dim tipoFactura As Integer = ft.DevuelveScalarEntero(myConn, "select tipo from jsvenencpos where numfac = '" & numFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")
                Dim montoFactura As Double = ft.DevuelveScalarDoble(myConn, "select tot_fac from jsvenencpos where numfac = '" & numFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")
                Dim condPago As Integer = ft.DevuelveScalarEntero(myConn, "select CONDPAG from jsvenencpos where numfac = '" & numFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")
                Dim FechaVencimiento As Date = ft.DevuelveScalarFecha(myConn, "select VENCE from jsvenencpos where numfac = '" & numFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")
                Dim rRIF As String = ft.DevuelveScalarCadena(myConn, "select RIF from jsvenencpos where numfac = '" & numFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")

                Dim NumeroSERIALFiscal As String = NumeroSERIALImpresoraFISCAL(myConn, lblInfo, codCaja)

                GuardarFacturaNotaCredito_PuntoDeVenta(myConn, numFacturaReal, NumeroSERIALFiscal, _
                                        numFactura, NumeroSERIALFiscal, 0, _
                                        "")

                ' 4.- Incluye movimiento en caja
                IncluirMovimientosPuntoDeVentaEnCaja(myConn, ds, numFacturaReal, NumeroSERIALFiscal, tipoFactura, _
                                                     montoFactura, FechaVencimiento, condPago)
                '' 5.- Incluye movimiento en CXC
                'IncluyeMovimientoCXC(myConn, numFacturaReal)
                '' 6.- Actualizar movimientos inventario
                Dim ActualizaInventario As Boolean = CBool(ParametroPlus(myConn, Gestion.iPuntosdeVentas, "POSPARAM24"))
                If ActualizaInventario Then _
                        ActualizarMovimientosInventario(myConn, numFacturaReal, NumeroSERIALFiscal, True)
                '' 7.- Incluye cliente pv 
                'IncluirClientePV(myConn, CodigoCliente)
                '' 8.- Cerrar Ventana de pago
                ActualizarMovimientosInventarioCADIP(myConn, numFacturaReal, rRIF, _
                   NumeroSERIALImpresoraFISCAL(myConn, lblInfo, jytsistema.WorkBox), jytsistema.sFechadeTrabajo)
            End If
        End If
        ft.mensajeInformativo(" Listo ....")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If jytsistema.sUsuario = "00000" Then
            'txtFacturaAnterior.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT a.numfac codigo,  b.nummov descrip " _
            '                                            & " FROM jsvenforpag a " _
            '                                            & " LEFT JOIN jsventrapos b ON (a.numfac = SUBSTRING(b.nummov,1,10) AND a.vence = b.fecha) " _
            '                                            & " WHERE " _
            '                                            & " vence = '" & ft.FormatoFechaMySQL(CDate(txtFechaDesde.Text)) & "' AND " _
            '                                            & " b.nummov IS NULL  ", "Factura No Procesadas", txtFacturaAnterior.Text)
        End If
    End Sub

    Private Sub txtFacturaAnterior_TextChanged(sender As Object, e As EventArgs) Handles txtFacturaAnterior.TextChanged
        Dim i = 630546
        Dim wE As Boolean = True
        Dim nFac As String = ""
        While wE
            nFac = "PV" & ft.RellenaConCaracter(i, 8, "0", Transportables.lado.izquierdo)
            If ft.DevuelveScalarEntero(myConn, " select count(*) from jsvenencpos where numfac = '" & nFac & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then wE = False
            i += 1
        End While

        txtFacturaActual.Text = nFac
    End Sub
End Class