Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Public Class jsVenProReconstruccionDeSaldos
    Private Const sModulo As String = "Reconstrucción de Saldos y movimientos de clientes y mercancías"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dtClientes As New DataTable
    Private ft As New Transportables
  
    Private strSQLClientes As String = ""
  
    Private nTablaClientes As String = "tblClientes"
    

    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        MyConn = MyCon
        IniciarTXT()
        Me.Show()

    End Sub
    Private Sub IniciarTXT()

        ft.habilitarObjetos(True, True, txtClienteDesde, txtClienteHasta, btnClienteDesde, btnClienteHasta, txtFechaDesde, txtFechaHasta)

        txtFechaDesde.Value = PrimerDiaMes(jytsistema.sFechadeTrabajo)
        txtFechaHasta.Value = jytsistema.sFechadeTrabajo

        chkFAC.Checked = True
        chkCxC.Checked = True


    End Sub

    Private Sub jsVenProReconstruccionDeSaldos_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsVenProReconstruccionDeSaldos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        Dim dates As SfDateTimeEdit() = {txtFechaDesde, txtFechaHasta}
        SetSizeDateObjects(dates)

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Function Validado() As Boolean
        Validado = False


        If Not UsuarioClaveAES(MyConn, lblInfo, "jytsuite", txtPassWord.Text) Then
            ft.MensajeCritico("Clave no encontrada. Intente de nuevo...")
            Return False
        Else
            Return True
        End If

        Validado = True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then

            If chkFAC.Checked Then ProcesarFacturas()
            If chkNC.Checked Then ProcesarNotasCreditoVentas()
            If chkPOS.Checked Then ProcesarFacturasPuntosDeVenta()
            If chkCxC.Checked Then ActualizarSaldoClientes()

            MsgBox("Proceso terminado...", MsgBoxStyle.Information, sModulo)
            InsertarAuditoria(MyConn, MovAud.iProcesar, sModulo, "")
            ProgressBar1.Value = 0
            lblProgreso.Text = ""

        End If

    End Sub

    Private Sub btnClienteDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClienteDesde.Click
        txtClienteDesde.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codcli codigo, nombre descripcion, rif, ingreso from jsvencatcli where id_emp = '" & jytsistema.WorkID & "' order by codcli  ", "Clientes", txtClienteDesde.Text)
    End Sub
    Private Sub btnClienteHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClienteHasta.Click
        txtClienteHasta.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codcli codigo, nombre descripcion, rif, ingreso from jsvencatcli where id_emp = '" & jytsistema.WorkID & "' order by codcli  ", "Clientes", txtClienteHasta.Text)
    End Sub

    Private Sub ActualizarSaldoClientes()

        Dim jCont As Integer

        EliminaMovimientosTemporalesCXC()

        lblProgreso.Text = " ACTUALIZANDO CLIENTES Y SALDOS "

        strSQLClientes = "select * from jsvencatcli where " _
            & IIf(txtClienteDesde.Text <> "", " codcli >= '" & txtClienteDesde.Text & "' and ", "") _
            & IIf(txtClienteHasta.Text <> "", " codcli <= '" & txtClienteHasta.Text & "' and ", "") _
            & " id_emp = '" & jytsistema.WorkID & "' order by codcli "

        ds = DataSetRequery(ds, strSQLClientes, MyConn, nTablaClientes, lblInfo)
        dtClientes = ds.Tables(nTablaClientes)

        If dtClientes.Rows.Count > 0 Then
            For jCont = 0 To dtClientes.Rows.Count - 1
                With dtClientes.Rows(jCont)
                    SaldoCxC(MyConn, lblInfo, .Item("CODCLI"))
                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(jCont / dtClientes.Rows.Count * 100), _
                                                  " CLIENTE : " & .Item("CODCLI") & " " & .Item("NOMBRE"))
                End With
            Next
        End If
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 0, "")

    End Sub

    Private Sub EliminaMovimientosTemporalesCXC()

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " ELIMINANDO MOVIMIENTOS ")
        ft.Ejecutar_strSQL(MyConn, " delete from jsventracob where " _
                & " substring(nummov,1,3) in ('TMP','FCT') and " _
                & " emision >= '" & ft.FormatoFechaMySQL(txtFechaDesde.Value) & "' and " _
                & " emision <= '" & ft.FormatoFechaMySQL(txtFechaHasta.Value) & "' and " _
                & IIf(txtClienteDesde.Text <> "", " codcli >= '" & txtClienteDesde.Text & "' and ", "") _
                & IIf(txtClienteHasta.Text <> "", " codcli <= '" & txtClienteHasta.Text & "' and ", "") _
                & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                & " ID_EMP = '" & jytsistema.WorkID & "' ")


    End Sub

    Private Sub EliminaMovimientos(ByVal Origen As String)

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " ELIMINANDO MOVIMIENTOS ")

        ft.Ejecutar_strSQL(MyConn, " delete from jsmertramer where " _
            & " origen = '" & Origen & "' and " _
            & " date_format(fechamov, '%Y-%m-%d') >= '" & ft.FormatoFechaMySQL(txtFechaDesde.Value) & "' and " _
            & " date_format(fechamov, '%Y-%m-%d') <= '" & ft.FormatoFechaMySQL(txtFechaHasta.Value) & "' and " _
            & IIf(txtClienteDesde.Text <> "", " prov_cli >= '" & txtClienteDesde.Text & "' and ", "") _
            & IIf(txtClienteHasta.Text <> "", " prov_cli <= '" & txtClienteHasta.Text & "' and ", "") _
            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & " ID_EMP = '" & jytsistema.WorkID & "' ")


    End Sub

    Private Sub ProcesarNotasCreditoVentas()

        Dim dtEncab, dtRenglones As DataTable
        Dim nTablaEncab As String = "tblEncab"
        Dim nTablaRenglones As String = "tblRenglones"

        Dim iCont, kCont As Integer
        Dim NumeroFactura As String
        Dim Costo As Double

        Dim strTipo As String
        Dim strTipo1 As String
        Dim sFOTipo As String
        Dim sSigno As Integer

        EliminaMovimientos("NCV")
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " NOTAS DE CREDITO VENTAS  ")


        ds = DataSetRequery(ds, " select * from jsvenencncr where " _
            & " emision >= '" & ft.FormatoFechaMySQL(txtFechaDesde.Value) & "' and " _
            & " emision <= '" & ft.FormatoFechaMySQL(txtFechaHasta.Value) & "' and " _
            & IIf(txtClienteDesde.Text <> "", " codcli >= '" & txtClienteDesde.Text & "' and ", "") _
            & IIf(txtClienteHasta.Text <> "", " codcli <= '" & txtClienteHasta.Text & "' and ", "") _
            & " ID_EMP = '" & jytsistema.WorkID & "' " _
            & "", MyConn, nTablaEncab, lblInfo)

        dtEncab = ds.Tables(nTablaEncab)


        If dtEncab.Rows.Count > 0 Then
            For iCont = 0 To dtEncab.Rows.Count - 1

                With dtEncab.Rows(iCont)

                    NumeroFactura = .Item("NUMNCR")

                    ft.Ejecutar_strSQL(myconn, " delete from jsmertramer where numorg = '" & .Item("numncr") _
                                   & "' and tipomov = 'EN' and origen = 'NCV' and prov_cli = '" & .Item("codcli") & "' and id_emp = '" & jytsistema.WorkID & "' ")


                    ds = DataSetRequery(ds, " SELECT * FROM jsvenrenncr where " _
                                     & " numncr = '" & NumeroFactura & "' AND " _
                                     & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                     & " ID_EMP = '" & jytsistema.WorkID & "' " _
                                     & "", MyConn, nTablaRenglones, lblInfo)

                    dtRenglones = ds.Tables(nTablaRenglones)
                    If chkNC.Checked = True Then
                        If dtRenglones.Rows.Count > 0 Then

                            For kCont = 0 To dtRenglones.Rows.Count - 1


                                If Mid(dtRenglones.Rows(kCont).Item("item"), 1, 1) <> "$" Then

                                    Costo = dtRenglones.Rows(kCont).Item("CANTIDAD") * UltimoCostoAFecha(MyConn, dtRenglones.Rows(kCont).Item("ITEM"), CDate(.Item("emision").ToString)) / Equivalencia(MyConn, dtRenglones.Rows(kCont).Item("ITEM"), dtRenglones.Rows(kCont).Item("UNIDAD"))
                                    Dim CausaCredito As String = ft.DevuelveScalarCadena(MyConn, " select codigo from jsvencaudcr where codigo = '" & dtRenglones.Rows(kCont).Item("causa") & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "' ")
                                    Dim DevBuenEstado As Boolean = ft.DevuelveScalarBooleano(MyConn, " select estado from jsvencaudcr where codigo = '" & dtRenglones.Rows(kCont).Item("causa") & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "' ")
                                    If CausaCredito <> "0" Then
                                        Dim MueveInventario As Boolean = ft.DevuelveScalarBooleano(MyConn, " select inventario from jsvencaudcr where codigo = '" & dtRenglones.Rows(kCont).Item("causa") & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "' ")
                                        If MueveInventario Then
                                            If ft.DevuelveScalarCadena(MyConn, " select codart from jsmertramer where codart = '" & dtRenglones.Rows(kCont).Item("ITEM") & "' and tipomov = 'EN' and numdoc = '" & .Item("NUMNCR") & "' and prov_cli = '" & .Item("CODCLI") & "' and asiento = '" & dtRenglones.Rows(kCont).Item("RENGLON") & dtRenglones.Rows(kCont).Item("ESTATUS") & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                                                InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, dtRenglones.Rows(kCont).Item("item"), CDate(.Item("emision").ToString), "EN", _
                                                    dtRenglones.Rows(kCont).Item("numncr"), dtRenglones.Rows(kCont).Item("unidad"), dtRenglones.Rows(kCont).Item("cantidad"), _
                                                    dtRenglones.Rows(kCont).Item("peso"), Costo, Costo, "NCV", dtRenglones.Rows(kCont).Item("numncr"), IIf(IsDBNull(dtRenglones.Rows(kCont).Item("LOTE")), "", dtRenglones.Rows(kCont).Item("lote")), _
                                                    .Item("codcli"), dtRenglones.Rows(kCont).Item("totren"), dtRenglones.Rows(kCont).Item("totrendes"), 0, dtRenglones.Rows(kCont).Item("TOTREN") - dtRenglones.Rows(kCont).Item("TOTRENDES"), .Item("codven"), _
                                                    If(DevBuenEstado, .Item("almacen"), "00002"), dtRenglones.Rows(kCont).Item("renglon"), jytsistema.sFechadeTrabajo)
                                            End If
                                        End If

                                        Dim AjustaPrecio As Boolean = ft.DevuelveScalarBooleano(MyConn, " select ajustaprecio from jsvencaudcr where codigo = '" & dtRenglones.Rows(kCont).Item("causa") & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "' ")
                                        If AjustaPrecio Then
                                            If ft.DevuelveScalarCadena(MyConn, " select codart from jsmertramer where codart = '" & dtRenglones.Rows(kCont).Item("ITEM") & "' and tipomov = 'AP' and numdoc = '" & .Item("NUMNCR") & "' and prov_cli = '" & .Item("CODCLI") & "' and asiento = '" & dtRenglones.Rows(kCont).Item("RENGLON") & dtRenglones.Rows(kCont).Item("ESTATUS") & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                                                InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, dtRenglones.Rows(kCont).Item("item"), CDate(.Item("emision").ToString), "AP", .Item("NUMNCR"), _
                                                                                                     dtRenglones.Rows(kCont).Item("unidad"), 0.0, 0.0, 0.0, _
                                                                                                     0.0, "NCV", dtRenglones.Rows(kCont).Item("numfac"), dtRenglones.Rows(kCont).Item("lote"), .Item("CODCLI"), _
                                                                                                     dtRenglones.Rows(kCont).Item("totren"), dtRenglones.Rows(kCont).Item("totrendes"), 0, 0.0, .Item("codven"), _
                                                                                                      If(DevBuenEstado, .Item("almacen"), "00002"), dtRenglones.Rows(kCont).Item("renglon"), jytsistema.sFechadeTrabajo)
                                            End If
                                        End If


                                    Else
                                        If ft.DevuelveScalarCadena(MyConn, " select codart from jsmertramer where codart = '" & dtRenglones.Rows(kCont).Item("ITEM") & "' and tipomov = 'EN' and numdoc = '" & .Item("NUMNCR") & "' and prov_cli = '" & .Item("CODCLI") & "' and asiento = '" & dtRenglones.Rows(kCont).Item("RENGLON") & dtRenglones.Rows(kCont).Item("ESTATUS") & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                                            InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, dtRenglones.Rows(kCont).Item("ITEM"), CDate(.Item("emision").ToString), "EN", _
                                                   dtRenglones.Rows(kCont).Item("numncr"), dtRenglones.Rows(kCont).Item("UNIDAD"), dtRenglones.Rows(kCont).Item("CANTIDAD"), _
                                                   dtRenglones.Rows(kCont).Item("PESO"), Costo, Costo, "NCV", dtRenglones.Rows(kCont).Item("numncr"), IIf(IsDBNull(dtRenglones.Rows(kCont).Item("LOTE")), "", dtRenglones.Rows(kCont).Item("lote")), _
                                                   .Item("codcli"), dtRenglones.Rows(kCont).Item("TOTREN"), dtRenglones.Rows(kCont).Item("TOTRENDES"), 0.0#, dtRenglones.Rows(kCont).Item("TOTREN") - dtRenglones.Rows(kCont).Item("TOTRENDES"), _
                                                   .Item("codven"), If(DevBuenEstado, .Item("almacen"), "00002"), dtRenglones.Rows(kCont).Item("RENGLON"), jytsistema.sFechadeTrabajo)

                                        End If
                                    End If

                                End If

                                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(iCont / dtEncab.Rows.Count * 100), NumeroFactura & " : " & dtRenglones.Rows(kCont).Item("ITEM") & " " & dtRenglones.Rows(kCont).Item("DESCRIP"))
                            Next



                        End If
                    End If

                    If chkCxC.Checked = True Then
                        If ft.DevuelveScalarCadena(MyConn, " SELECT codcli from jsventracob where codcli = '" & .Item("codcli") & "' and tipomov = 'NC' and nummov = '" & .Item("numncr") & "' and numorg = '" & .Item("numncr") & "' and id_emp = '" & jytsistema.WorkID & "'") = 0 Then

                            strTipo = "NC"
                            strTipo1 = "NOTA CREDITO N° "
                            sFOTipo = "1"
                            sSigno = -1


                            InsertEditVENTASCXC(MyConn, lblInfo, True, .Item("codcli"), strTipo, .Item("numNCR"), CDate(.Item("emision").ToString), ft.FormatoHora(Now()),
                                CDate(.Item("vence").ToString), "", strTipo1 & ": " & .Item("numNCR"), sSigno * .Item("tot_NCR"), .Item("imp_iva"),
                                .Item("formapag"), .Item("caja"), .Item("numpag"), .Item("nompag"), "", "NCR", .Item("numNCR"), "0",
                                "", jytsistema.sFechadeTrabajo, "", "", "", 0.0#, 0.0#, "", "", "", "", .Item("codven"),
                                .Item("codven"), "0", sFOTipo, jytsistema.WorkDivition, jytsistema.WorkCurrency.Id, DateTime.Now())

                            SaldoCxC(MyConn, lblInfo, .Item("codcli"))

                        End If

                    End If
                End With
            Next
        End If
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 0, "")

    End Sub
    Private Sub ProcesarFacturas()

        Dim dtEncab, dtRenglones As DataTable
        Dim nTablaEncab As String = "tblEncab"
        Dim nTablaRenglones As String = "tblRenglones"

        Dim iCont, kCont As Integer
        Dim NumeroFactura As String
        Dim Costo As Double

        Dim strTipo As String
        Dim strTipo1 As String
        Dim sFOTipo As String
        Dim sSigno As Integer

        EliminaMovimientos("FAC")
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " FACTURAS  ")

        ds = DataSetRequery(ds, " select * from jsvenencfac where " _
            & " emision >= '" & ft.FormatoFechaMySQL(txtFechaDesde.Value) & "' and " _
            & " emision <= '" & ft.FormatoFechaMySQL(txtFechaHasta.Value) & "' and " _
            & IIf(txtClienteDesde.Text <> "", " codcli >= '" & txtClienteDesde.Text & "' and ", "") _
            & IIf(txtClienteHasta.Text <> "", " codcli <= '" & txtClienteHasta.Text & "' and ", "") _
            & " ID_EMP = '" & jytsistema.WorkID & "' " _
            & "", MyConn, nTablaEncab, lblInfo)

        dtEncab = ds.Tables(nTablaEncab)

        If dtEncab.Rows.Count > 0 Then
            For iCont = 0 To dtEncab.Rows.Count - 1

                With dtEncab.Rows(iCont)

                    NumeroFactura = .Item("NUMFAC")

                    ft.Ejecutar_strSQL(myconn, " delete from jsmertramer where numorg = '" & .Item("numfac") _
                                   & "' and tipomov = 'SA' and origen = 'FAC' and prov_cli = '" & .Item("codcli") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                    ds = DataSetRequery(ds, " SELECT * FROM jsvenrenfac where " _
                        & " NUMFAC = '" & NumeroFactura & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' " _
                        & "", MyConn, nTablaRenglones, lblInfo)

                    dtRenglones = ds.Tables(nTablaRenglones)
                    If chkFAC.Checked = True Then
                        If dtRenglones.Rows.Count > 0 Then
                            For kCont = 0 To dtRenglones.Rows.Count - 1


                                If Mid(dtRenglones.Rows(kCont).Item("item"), 1, 1) <> "$" Then
                                    Costo = dtRenglones.Rows(kCont).Item("CANTIDAD") * UltimoCostoAFecha(MyConn, dtRenglones.Rows(kCont).Item("ITEM"), CDate(.Item("emision").ToString)) / Equivalencia(MyConn, dtRenglones.Rows(kCont).Item("ITEM"), dtRenglones.Rows(kCont).Item("UNIDAD"))
                                    If ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsmertramer where codart = '" & dtRenglones.Rows(kCont).Item("ITEM") & "' and tipomov = 'SA' and numdoc = '" & .Item("NUMFAC") & "' and prov_cli = '" & .Item("CODCLI") & "' and asiento = '" & dtRenglones.Rows(kCont).Item("RENGLON") & dtRenglones.Rows(kCont).Item("ESTATUS") & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then

                                        InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, dtRenglones.Rows(kCont).Item("ITEM"), CDate(.Item("emision").ToString), "SA", _
                                        dtRenglones.Rows(kCont).Item("numfac"), dtRenglones.Rows(kCont).Item("UNIDAD"), dtRenglones.Rows(kCont).Item("CANTIDAD"), _
                                        dtRenglones.Rows(kCont).Item("PESO"), Costo, Costo, "FAC", dtRenglones.Rows(kCont).Item("numfac"), IIf(IsDBNull(dtRenglones.Rows(kCont).Item("LOTE")), "", dtRenglones.Rows(kCont).Item("lote")), _
                                        .Item("codcli"), dtRenglones.Rows(kCont).Item("TOTREN"), dtRenglones.Rows(kCont).Item("TOTRENDES"), 0.0#, dtRenglones.Rows(kCont).Item("TOTREN") - dtRenglones.Rows(kCont).Item("TOTRENDES"), _
                                        .Item("codven"), .Item("almacen"), dtRenglones.Rows(kCont).Item("RENGLON") & dtRenglones.Rows(kCont).Item("ESTATUS"), jytsistema.sFechadeTrabajo, False)

                                    End If
                                End If
                                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(iCont / dtEncab.Rows.Count * 100), NumeroFactura & " : " & dtRenglones.Rows(kCont).Item("ITEM") & " " & dtRenglones.Rows(kCont).Item("DESCRIP"))
                            Next
                        End If
                    End If
                    If chkCxC.Checked = True Then
                        If ft.DevuelveScalarCadena(MyConn, " SELECT codcli from jsventracob where codcli = '" & .Item("codcli") & "' and tipomov = 'FC' and nummov = '" & .Item("numfac") & "' and numorg = '" & .Item("numfac") & "' and id_emp = '" & jytsistema.WorkID & "'") = 0 Then

                            If .Item("condpag") = 0 Then

                                strTipo = "FC"
                                strTipo1 = "FACTURA N° "
                                sFOTipo = "0"
                                sSigno = 1


                                InsertEditVENTASCXC(MyConn, lblInfo, True, .Item("codcli"), strTipo, .Item("numfac"), CDate(.Item("emision").ToString), ft.FormatoHora(Now()),
                                    CDate(.Item("vence").ToString), "", strTipo1 & ": " & .Item("numfac"), sSigno * .Item("tot_fac"), .Item("imp_iva"),
                                    .Item("formapag"), .Item("caja"), .Item("numpag"), .Item("nompag"), "", "FAC", .Item("numfac"), "0",
                                    "", jytsistema.sFechadeTrabajo, "", "", "", 0.0#, 0.0#, "", "", "", "", .Item("codven"),
                                    .Item("codven"), "0", sFOTipo, jytsistema.WorkDivition, jytsistema.WorkCurrency.Id, DateTime.Now())

                            End If

                            SaldoCxC(MyConn, lblInfo, .Item("codcli"))

                        End If

                    End If

                End With
            Next
        End If

        ActualizarInventarioTodaMercancia(MyConn, ds, lblInfo)
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 0, "")

    End Sub
    Public Sub ActualizarInventarioTodaMercancia(MyConn As MySqlConnection, ds As DataSet, lblInfo As Label)
        Dim nTableINV As String = "tblInventario"
        Dim dtMER As New DataTable
        ds = DataSetRequery(ds, " select * from jsmerctainv where id_emp = '" & jytsistema.WorkID & "' order by codart ", MyConn, nTableINV, lblInfo)
        dtMER = ds.Tables(nTableINV)

        Dim ICont = 1

        For Each nRow As DataRow In dtMER.Rows
            ActualizarExistenciasPlus(MyConn, nRow.Item("codart"))
            ICont += 1
            refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(ICont / dtMER.Rows.Count * 100), " MERCANCIA : " & nRow.Item("CODART") & " " & nRow.Item("NOMART"))
        Next

    End Sub
    Private Sub ProcesarFacturasPuntosDeVenta()

        Dim dtEncab, dtRenglones As DataTable
        Dim nTablaEncab As String = "tblEncab"
        Dim nTablaRenglones As String = "tblRenglones"

        Dim iCont, kCont As Integer
        Dim NumeroFactura As String
        Dim Costo As Double

        EliminaMovimientos("PVE")
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " FACTURAS PUNTO DE VENTAS ")

        ds = DataSetRequery(ds, " select * from jsvenencpos where " _
            & " emision >= '" & ft.FormatoFechaMySQL(txtFechaDesde.Value) & "' and " _
            & " emision <= '" & ft.FormatoFechaMySQL(txtFechaHasta.Value) & "' and " _
            & IIf(txtClienteDesde.Text <> "", " codcli >= '" & txtClienteDesde.Text & "' and ", "") _
            & IIf(txtClienteHasta.Text <> "", " codcli <= '" & txtClienteHasta.Text & "' and ", "") _
            & " ID_EMP = '" & jytsistema.WorkID & "' " _
            & "", MyConn, nTablaEncab, lblInfo)

        dtEncab = ds.Tables(nTablaEncab)

        If dtEncab.Rows.Count > 0 Then
            For iCont = 0 To dtEncab.Rows.Count - 1

                With dtEncab.Rows(iCont)

                    NumeroFactura = .Item("NUMFAC")

                    ft.Ejecutar_strSQL(myconn, " delete from jsmertramer where numorg = '" & .Item("numfac") _
                                   & "' and tipomov = 'SA' and origen = 'PVE' and prov_cli = '" & .Item("codcli") & "' and id_emp = '" & jytsistema.WorkID & "' ")


                    ds = DataSetRequery(ds, " SELECT * FROM jsvenrenpos where " _
                        & " NUMFAC = '" & NumeroFactura & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' " _
                        & "", MyConn, nTablaRenglones, lblInfo)

                    dtRenglones = ds.Tables(nTablaRenglones)
                    If chkPOS.Checked = True Then
                        If dtRenglones.Rows.Count > 0 Then
                            For kCont = 0 To dtRenglones.Rows.Count - 1


                                If Mid(dtRenglones.Rows(kCont).Item("item"), 1, 1) <> "$" Then
                                    Costo = dtRenglones.Rows(kCont).Item("CANTIDAD") * UltimoCostoAFecha(MyConn, dtRenglones.Rows(kCont).Item("ITEM"), CDate(.Item("emision").ToString)) / Equivalencia(MyConn, dtRenglones.Rows(kCont).Item("ITEM"), dtRenglones.Rows(kCont).Item("UNIDAD"))
                                    If ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsmertramer where codart = '" & dtRenglones.Rows(kCont).Item("ITEM") & "' and tipomov = 'SA' and numdoc = '" & .Item("NUMFAC") & "' and prov_cli = '" & .Item("CODCLI") & "' and asiento = '" & dtRenglones.Rows(kCont).Item("RENGLON") & dtRenglones.Rows(kCont).Item("ESTATUS") & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then

                                        InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, dtRenglones.Rows(kCont).Item("ITEM"), CDate(.Item("emision").ToString), "SA", _
                                        dtRenglones.Rows(kCont).Item("numfac"), dtRenglones.Rows(kCont).Item("UNIDAD"), dtRenglones.Rows(kCont).Item("CANTIDAD"), _
                                        dtRenglones.Rows(kCont).Item("PESO"), Costo, Costo, "PVE", dtRenglones.Rows(kCont).Item("numfac"), IIf(IsDBNull(dtRenglones.Rows(kCont).Item("LOTE")), "", dtRenglones.Rows(kCont).Item("lote")), _
                                        .Item("codcli"), dtRenglones.Rows(kCont).Item("TOTREN"), dtRenglones.Rows(kCont).Item("TOTRENDES"), 0.0#, dtRenglones.Rows(kCont).Item("TOTREN") - dtRenglones.Rows(kCont).Item("TOTRENDES"), _
                                        .Item("codven"), .Item("almacen"), dtRenglones.Rows(kCont).Item("RENGLON") & dtRenglones.Rows(kCont).Item("ESTATUS"), jytsistema.sFechadeTrabajo, False)

                                    End If
                                End If
                                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(iCont / dtEncab.Rows.Count * 100), NumeroFactura & " : " & dtRenglones.Rows(kCont).Item("ITEM") & " " & dtRenglones.Rows(kCont).Item("DESCRIP"))
                            Next
                        End If
                    End If

                End With
            Next
        End If

        ActualizarInventarioTodaMercancia(MyConn, ds, lblInfo)
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 0, "")

    End Sub
    Private Sub txtClienteDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtClienteDesde.TextChanged
        txtClienteHasta.Text = txtClienteDesde.Text
    End Sub
End Class