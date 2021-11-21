Imports MySql.Data.MySqlClient
Imports FP_AclasBixolon
Public Class jsPOSProCierre
    Private Const sModulo As String = "Cierre Diario de caja"
    Private Const nTabla As String = "tbLCierre"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As DataTable
    Private ft As New Transportables


    Private ProcesarCierre As Boolean
    Private DondeLlama As Integer '0 = POS_DATUM ; 1 = DATUM

    Private IB As New AclasBixolon
    Private bRet As Boolean

    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal Procesar As Boolean, Optional ByVal DeDondeLlama As Integer = 0)

        'DondeLlama  : 0 = POS_DATUM ; 1 = DATUM 
        myConn = MyCon

        ProcesarCierre = Procesar
        DondeLlama = DeDondeLlama

        btnCaja.Visible = CBool((DondeLlama))
        btnCajero.Visible = CBool(DondeLlama)
        btnFechaCierre.Visible = CBool(DondeLlama)

        If ProcesarCierre Then
            lblLeyenda.Text = " Este proceso cierra la caja (Reporte Z) para el cajero, relizando la siguientes tareas : " + vbCr + _
                " 1.- Transfiere las mercancias facturadas al inventario, si el método escogido de actualización de inventario es de tipo batch. " + vbCr + _
                " 2.- Transfiere los movimientos de ventas de esta caja a caja principal y a bancos. " + vbCr + _
                " 3.- Bloquea el proceso de facturación para este cajero en esta caja y en este día. " + vbCr + _
                " 4.- Imprime el arqueo de caja (Reducción Z). "
            IniciarTXT()
        Else
            lblLeyenda.Text = " Este proceso cierre de caja " + vbCr + _
                "  " + vbCr + _
                "  "
        End If
        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()
        ft.habilitarObjetos(False, True, txtCaja, txtCodigoCajero, txtFecha, txtNombreCajero)
        txtFecha.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtCodigoCajero.Text = IIf(DondeLlama = 0, jytsistema.sUsuario, "")
        txtNombreCajero.Text = IIf(DondeLlama = 0, jytsistema.sNombreUsuario, "")
        txtCaja.Text = IIf(DondeLlama = 0, jytsistema.WorkBox, "")
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub jsPOSProCierre_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing

    End Sub

    Private Sub jsPOSProCierre_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If ProcesarCierre Then
            GenerarCierre()
            ft.mensajeInformativo(" CIERRE DE CAJA REALIZADO ...")
        Else
            ft.mensajeInformativo(" REVERSO CIERRE DE CAJA REALIZADO ...")
        End If

        ProgressBar1.Value = 0
        lblProgreso.Text = ""
        If DondeLlama = 0 Then Me.Close()

    End Sub
    Private Sub GenerarCierre()



        '2.- MARCAR EL CIERRE DE LA CAJA
        If ft.DevuelveScalarEntero(myConn, " select count(*) from jsveninipos where " _
                                & " FECHA = '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' AND " _
                                & " CODCAJ = '" & jytsistema.WorkBox & "' AND " _
                                & " HORACIERRE = '00:00:00' AND " _
                                & " ESTATUS = 1 AND " _
                                & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                & " ID_EMP = '" & jytsistema.WorkID & "'") > 0 Then

            ft.Ejecutar_strSQL(myConn, " update jsveninipos set horacierre = '" & Format(Now(), "hh:mm:ss") & "', estatus = 1 " _
                                & " where " _
                                & " FECHA = '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' AND " _
                                & " CODCAJ = '" & jytsistema.WorkBox & "' AND " _
                                & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                & " ID_EMP = '" & jytsistema.WorkID & "'")

            '3.- IMPRIMIR REPORTE FISCAL Z
            If DondeLlama = 0 Then
                Dim TipoImpresora As Integer = TipoImpresoraFiscal(myConn, jytsistema.WorkBox)

                Select Case TipoImpresora
                    Case 0
                        ft.mensajeInformativo("Imprime Reporte Z...")
                    Case 1 ' Factura Fiscal preimpresa
                        ft.mensajeInformativo("Imprimienso Reporte Z ")
                    Case 2, 5, 6, 7 ' Impresora Tipo Aclas (TFHKAIF.DLL)
                        bRet = IB.abrirPuerto(PuertoImpresoraFiscal(myConn, lblInfo, jytsistema.WorkBox))
                        If bRet Then
                            bRet = IB.ReporteZFiscal()
                            IB.cerrarPuerto()
                        End If
                    Case 3 ' Impresora Tipo Bematech (BEMAFI32.DLL)
                        ReporteZFiscalBematech()
                    Case 4 ' Impresora tipo epson/PnP
                        ReporteZFiscalPnP(myConn, lblInfo)

                End Select



            End If

        End If


        '4.- IMPRIMIR REPORTE FISCAL Z DEL SISTEMA
        Dim f As New jsPOSRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReportePuntoDeVenta.cReporteZ, "REDUCCION Z", jytsistema.WorkBox)
        f = Nothing

        Dim pb As New Progress_Bar
        pb.WindowTitle = "PROCESANDO MOVIMIENTOS DE CAJA ..."
        pb.TimeOut = 60
        pb.CallerThreadSet = Threading.Thread.CurrentThread

        dt = ft.AbrirDataTable(ds, nTabla, myConn, " select * from jsvenencpos where " _
                                                  & " EMISION = '" & ft.FormatoFechaMySQL(CDate(txtFecha.Text)) & "' AND " _
                                                  & " CODCAJ = '" & txtCaja.Text & "' AND " _
                                                  & " ID_EMP = '" & jytsistema.WorkID & "' order by NUMFAC ")

        '5.- PROCESAR CAJAS
        If dt.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dt.Rows.Count - 1
                With dt.Rows(iCont)
                    pb.OverallProgressText = "Procesando Factura " + .Item("numfac") + " A CAJA..."
                    pb.OverallProgressValue = CInt((iCont + 1) / dt.Rows.Count * 100)
                    'refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, ,)
                    ProcesarCaja(myConn, .Item("numfac"), .Item("numserial"), .Item("codcli"), .Item("codven"), CDate(.Item("emision").ToString))
                    'PROCESAR MERCANCIAS
                    'If Not CBool(ParametroPlus(myConn, Gestion.iPuntosdeVentas, "POSPARAM24")) Then _
                    '    ProcesarMercanciasFactura(myConn, ds, .Item("numfac"), CDate(.Item("emision").ToString), _
                    '        .Item("codcli"), .Item("ALMACEN"), jytsistema.sUsuario)
                End With
            Next
        End If

        pb.OverallProgressValue = 100
        pb.PartialProgressValue = 100




        If Not CBool(ParametroPlus(myConn, Gestion.iPuntosdeVentas, "POSPARAM24")) Then

            If ft.Pregunta(" ¿Desea transferir movimientos mercancías a inventario?", "Movimientos de Inventario") = Windows.Forms.DialogResult.Yes Then

                Dim pbM As New Progress_Bar
                pbM.WindowTitle = "PROCESANDO MOVIMIENTOS INVENTARIO..."
                pbM.TimeOut = 60
                pbM.CallerThreadSet = Threading.Thread.CurrentThread


                If dt.Rows.Count > 0 Then
                    Dim iCont As Integer
                    For iCont = 0 To dt.Rows.Count - 1
                        With dt.Rows(iCont)

                            'PROCESAR MERCANCIAS
                            'ProcesarMercanciasFactura(myConn, ds, .Item("numfac"), CDate(.Item("emision").ToString), _
                            '    .Item("codcli"), .Item("ALMACEN"), jytsistema.sUsuario, pb, CInt((iCont + 1) / dt.Rows.Count * 100))

                            Dim NumeroFactura As String = .Item("NUMFAC")
                            Dim ProgresoDeFacturas As Integer = CInt((iCont + 1) / dt.Rows.Count * 100)
                            Dim FechaFactura As Date = CDate(.Item("emision").ToString)
                            Dim CodigoCliente As String = .Item("CODCLI")
                            Dim AlmacenFacturacion As String = .Item("ALMACEN")

                            '//////////////////////////////////////////

                            Dim dtRenglones As DataTable
                            Dim nTableRenglones As String = "tblRenglones"

                            pbM.OverallProgressText = "Procesando Factura : " + NumeroFactura
                            pbM.OverallProgressValue = ProgresoDeFacturas

                            ft.Ejecutar_strSQL(myConn, " DELETE FROM jsmertramer where numdoc = '" & NumeroFactura & "' and origen = 'PVE' and numorg = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "'  ")

                            dtRenglones = ft.AbrirDataTable(ds, nTableRenglones, myConn, "select * from jsvenrenpos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")

                            For gCont As Integer = 0 To dtRenglones.Rows.Count - 1
                                With dtRenglones.Rows(gCont)

                                    pbM.PartialProgressText = "  Item : " + .Item("ITEM")
                                    pbM.PartialProgressValue = CInt((gCont + 1) / dtRenglones.Rows.Count * 100)

                                    Dim costoUnitario As Double = UltimoCostoAFecha(myConn, .Item("item"), jytsistema.sFechadeTrabajo) / Equivalencia(myConn, .Item("item"), .Item("unidad"))

                                    InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"),
                                                                          FechaFactura, IIf(.Item("tipo") = 0, "SA", "EN"),
                                                                          NumeroFactura, .Item("unidad"), .Item("cantidad"), .Item("peso"),
                                                                          costoUnitario * .Item("cantidad"), costoUnitario * .Item("cantidad"),
                                                                          "PVE", NumeroFactura, "", CodigoCliente, .Item("totren"), .Item("totrendes"),
                                                                          0.0, .Item("totren") - .Item("totrendes"), jytsistema.sUsuario, AlmacenFacturacion,
                                                                          .Item("renglon") & .Item("estatus"), jytsistema.sFechadeTrabajo)
                                End With
                            Next



                            '//////////////////////////////////////////


                        End With
                    Next

                    pbM.PartialProgressValue = 100
                    pbM.OverallProgressValue = 100

                End If

                pbM.Dispose()
                pbM = Nothing

            End If




        End If



        pb.Dispose()
        pb = Nothing



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

        'ds = DataSetRequery(ds, " select * from jsvenforpag where formapag in ('EF', 'CH', 'TA') and numfac = '" & NumeroFactura & "' and numserial = '" & NumeroSerial & "' and id_emp = '" & jytsistema.WorkID & "'  ", MyConn, nTableEFCHTA, lblInfo)

        ds = DataSetRequery(ds, " SELECT a.* " _
                            & " FROM jsvenforpag a " _
                            & " LEFT JOIN jsbantracaj b ON (a.numfac = b.nummov AND " _
                            & " 				a.formapag = b.formpag AND a.origen = b.origen AND " _
                            & " 				a.numpag = b.numpag AND a.nompag = b.refpag AND " _
                            & " 				a.id_emp = b.id_emp) " _
                            & " WHERE " _
                            & " b.caja IS NULL AND " _
                            & " a.numfac = '" & NumeroFactura & "' AND " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTableEFCHTA, lblInfo)


        dtPagosEFCHTA = ds.Tables(nTableEFCHTA)
        If dtPagosEFCHTA.Rows.Count > 0 Then
            For Each nRow As DataRow In dtPagosEFCHTA.Rows
                InsertEditBANCOSRenglonCaja(MyConn, lblInfo, True, CajaPrincipal, UltimoCajaMasUno(MyConn, lblInfo, CajaPrincipal), FechaFactura,
                                           "PVE", IIf(nRow.Item("importe") <= 0, "SA", "EN"), NumeroFactura, nRow.Item("formapag"), nRow.Item("numpag"), nRow.Item("nompag"),
                                           nRow.Item("importe"), "", IIf(nRow.Item("importe") <= 0, "NOTA CREDITO N° ", "FACTURA N° ") & NumeroFactura, "", jytsistema.sFechadeTrabajo, 1, "", "0", "", jytsistema.sFechadeTrabajo,
                                           CodigoCliente, CodigoVendedor, "1", jytsistema.WorkCurrency.Id, DateTime.Now())

            Next
        End If


        Dim nTablaCT As String = "tblPagoCT"
        Dim dtPagosCT As DataTable

        ds = DataSetRequery(ds, " select a.numfac, a.origen, a.formapag, a.numpag, a.nompag, b.importe, a.vence, a.id_emp, b.cantidad " _
                            & " from jsvenforpag a " _
                            & " left join ( select numcan, corredor, IFNULL(COUNT(*),0) cantidad, sum(monto) importe, id_emp from jsventabtic WHERE " _
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
                                             & "' and formpag = '" & .Item("formapag") & "' and numpag = '" & .Item("numpag") _
                                             & "' and refpag = '" & .Item("nompag") _
                                             & "' AND DEPOSITO = '' and id_emp = '" & jytsistema.WorkID & "'  ") = "0" Then _
                    InsertEditBANCOSRenglonCaja(MyConn, lblInfo, True, CajaPrincipal, UltimoCajaMasUno(MyConn, lblInfo, CajaPrincipal),
                                FechaFactura, "PVE", "EN", NumeroFactura, "CT",
                                .Item("numpag"), .Item("nompag"), .Item("importe"), "", "FACTURA N° " & NumeroFactura,
                                "", jytsistema.sFechadeTrabajo, 0, "", "0", "", jytsistema.sFechadeTrabajo,
                                CodigoCliente, CodigoVendedor, "1", jytsistema.WorkCurrency.Id, DateTime.Now())

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
                    InsertEditBANCOSMovimientoBanco(MyConn, lblInfo, True, FechaFactura, .Item("numpag"), "DP", .Item("nompag"), "", "CANC. FACTURA N° " & NumeroFactura,
                                             .Item("importe"), "PVE", NumeroFactura, "", "", "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo,
                                             "FC", "", jytsistema.sFechadeTrabajo, "0", CodigoCliente, CodigoVendedor, jytsistema.WorkCurrency.Id, DateTime.Now())
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
    Private Sub ProcesarMercanciasFactura(ByVal MyDB As MySqlConnection, ByVal ds As DataSet, ByVal NumeroFactura As String, _
                                          ByVal FechaFactura As Date, ByVal CodigoCliente As String, ByVal AlmacenFacturacion As String, _
                                          ByVal CodigoCajero As String, pb As Progress_Bar, PROGRESOdEfACTURAS As Integer)


        Dim dtRenglones As DataTable
        Dim nTableRenglones As String = "tblRenglones"

        pb.OverallProgressText = "Procesando Factura : " + NumeroFactura
        pb.OverallProgressValue = PROGRESOdEfACTURAS

        ft.Ejecutar_strSQL(MyDB, " DELETE FROM jsmertramer where numdoc = '" & NumeroFactura & "' and origen = 'PVE' and numorg = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "'  ")

        ds = DataSetRequery(ds, "select * from jsvenrenpos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ", myConn, nTableRenglones, lblInfo)
        dtRenglones = ds.Tables(nTableRenglones)

        Dim gCont As Integer
        For gCont = 0 To dtRenglones.Rows.Count - 1

            With dtRenglones.Rows(gCont)


                pb.PartialProgressText = "  Item : " + .Item("ITEM")
                pb.PartialProgressValue = CInt((gCont + 1) / dtRenglones.Rows.Count * 100)

                Dim costoUnitario As Double = UltimoCostoAFecha(myConn, .Item("item"), jytsistema.sFechadeTrabajo) / Equivalencia(myConn, .Item("item"), .Item("unidad"))
                InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"),
                                                      FechaFactura, IIf(.Item("tipo") = 0, "SA", "EN"),
                                                      NumeroFactura, .Item("unidad"), .Item("cantidad"), .Item("peso"),
                                                      costoUnitario * .Item("cantidad"), costoUnitario * .Item("cantidad"),
                                                      "PVE", NumeroFactura, "", CodigoCliente, .Item("totren"), .Item("totrendes"),
                                                      0.0, .Item("totren") - .Item("totrendes"), jytsistema.sUsuario, AlmacenFacturacion,
                                                      .Item("renglon") & .Item("estatus"), jytsistema.sFechadeTrabajo)
            End With
        Next

        pb.PartialProgressValue = 100

    End Sub

    Private Sub btnFechaCierre_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaCierre.Click
        '   txtFecha.Text = SeleccionaFecha(CDate(txtFecha.Text), Me, grpCaja, btnFechaCierre)
        Registros()
    End Sub

    Private Sub btnCajero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCajero.Click
        txtCodigoCajero.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, apellidos descripcion from jsvencatven where tipo = 1 and id_emp = '" & jytsistema.WorkID & "' order by codven  ", "Cajeros(as)", txtCodigoCajero.Text)
    End Sub

    Private Sub btnCaja_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCaja.Click
        txtCaja.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcaj codigo, descrip descripcion from jsvencatcaj where  id_emp = '" & jytsistema.WorkID & "' order by codcaj ", "Cajas", txtCaja.Text)
    End Sub

    Private Sub txtCodigoCajero_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigoCajero.TextChanged
        txtNombreCajero.Text = ft.DevuelveScalarCadena(myConn, "select CONCAT(apellidos, ', ', NOMBRES) FROM jsvencatven where codven = '" & txtCodigoCajero.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Registros()

    End Sub

    Private Sub txtCaja_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCaja.TextChanged
        Registros()
    End Sub
    Private Sub Registros()
        If DondeLlama = 0 Then
            lblAProcesar.Text = " Número de registros a procesar : " & ft.DevuelveScalarEntero(myConn, "SELECT COUNT(*) FROM jsvenencpos WHERE " _
                                                              & " EMISION = '" & ft.FormatoFechaMySQL(CDate(txtFecha.Text)) & "' AND " _
                                                              & " CODCAJ = '" & txtCaja.Text & "' AND " _
                                                              & " CODVEN = '" & txtCodigoCajero.Text & "' AND " _
                                                              & " ID_EMP = '" & jytsistema.WorkID & "' ")
        Else
            lblAProcesar.Text = " Número de registros a procesar : " & ft.DevuelveScalarEntero(myConn, "SELECT COUNT(*) FROM jsvenencpos WHERE " _
                                                                          & " EMISION = '" & ft.FormatoFechaMySQL(CDate(txtFecha.Text)) & "' AND " _
                                                                          & " CODCAJ = '" & txtCaja.Text & "' AND " _
                                                                          & " ID_EMP = '" & jytsistema.WorkID & "' ")
        End If
    End Sub
End Class