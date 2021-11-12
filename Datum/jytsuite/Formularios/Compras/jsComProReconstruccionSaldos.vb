Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsComProReconstruccionDeSaldos
    Private Const sModulo As String = "Reconstrucción de Saldos y movimientos de Proveedores y mercancías"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dtProveedores As New DataTable
    Private ft As New Transportables

    Private strSQLProveedores As String = ""

    Private nTablaProveedores As String = "tblProveedores"


    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        MyConn = MyCon
        Dim dates As SfDateTimeEdit() = {txtFechaDesde, txtFechaHasta}
        SetSizeDateObjects(dates)
        IniciarTXT()
        Me.Show()

    End Sub
    Private Sub IniciarTXT()

        ft.habilitarObjetos(True, True, txtProveedorDesde, txtProveedorHasta, btnProveedorDesde, btnProveedorHasta)

        txtFechaDesde.Value = PrimerDiaMes(jytsistema.sFechadeTrabajo)
        txtFechaHasta.Value = jytsistema.sFechadeTrabajo

        chkMercancias.Checked = True
        chkCxP.Checked = True


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

            If chkMercancias.Checked Then
                If LoginUser(MyConn, lblInfo) = "jytsuite" Then
                    ProcesarComprasPlus()
                Else
                    ProcesarFacturas()
                    ProcesarNotasCreditoCompras()
                    ProcesarFacturasGastos()
                End If
            End If
            If chkCxP.Checked Then ActualizarSaldoProveedores()

            MsgBox("Proceso terminado...", MsgBoxStyle.Information, sModulo)
            InsertarAuditoria(MyConn, MovAud.iProcesar, sModulo, "")
            pb2.Visible = False
            ProgressBar1.Value = 0
            lblProgreso.Text = ""

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFechaDesde.Text)
            Me.Close()

        End If

    End Sub


    Private Sub btnClienteDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProveedorDesde.Click
        txtProveedorDesde.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codpro codigo, nombre descripcion, rif, ingreso from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by codpro  ", "Proveedores", txtProveedorDesde.Text)
    End Sub
    Private Sub btnClienteHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProveedorHasta.Click
        txtProveedorHasta.Text = CargarTablaSimple(MyConn, lblInfo, ds, " select codpro codigo, nombre descripcion, rif, ingreso from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by codpro  ", "Proveedores", txtProveedorHasta.Text)
    End Sub

    Private Sub ActualizarSaldoProveedores()

        Dim jCont As Integer

        EliminaMovimientosTemporalesCXP()

        lblProgreso.Text = " ACTUALIZANDO Proveedores Y SALDOS "

        strSQLProveedores = "select * from jsprocatpro where " _
            & IIf(txtProveedorDesde.Text <> "", " codpro >= '" & txtProveedorDesde.Text & "' and ", "") _
            & IIf(txtProveedorHasta.Text <> "", " codpro <= '" & txtProveedorHasta.Text & "' and ", "") _
            & " id_emp = '" & jytsistema.WorkID & "' order by codpro "

        ds = DataSetRequery(ds, strSQLProveedores, MyConn, nTablaProveedores, lblInfo)
        dtProveedores = ds.Tables(nTablaProveedores)

        If dtProveedores.Rows.Count > 0 Then
            For jCont = 0 To dtProveedores.Rows.Count - 1
                With dtProveedores.Rows(jCont)
                    SaldoCxP(MyConn, lblInfo, .Item("CODPRO"))
                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(jCont / dtProveedores.Rows.Count * 100),
                                                  "PROVEEDOR : " & .Item("CODPRO") & " " & .Item("NOMBRE"))
                End With
            Next
        End If

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 0, "")

    End Sub

    Private Sub EliminaMovimientosTemporalesCXP()
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " ELIMINANDO MOVIMIENTOS ")
        ft.Ejecutar_strSQL(MyConn, " delete from jsprotrapag where " _
                & " substring(nummov,1,3) in ('TMP','FCT') and " _
                & " emision >= '" & ft.FormatoFechaMySQL(txtFechaDesde.Value) & "' and " _
                & " emision <= '" & ft.FormatoFechaMySQL(txtFechaHasta.Value) & "' and " _
                & IIf(txtProveedorDesde.Text <> "", " codpro >= '" & txtProveedorDesde.Text & "' and ", "") _
                & IIf(txtProveedorHasta.Text <> "", " codpro <= '" & txtProveedorHasta.Text & "' and ", "") _
                & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                & " ID_EMP = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub EliminaMovimientos(ByVal Origen As String)

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " ELIMINANDO MOVIMIENTOS ...")
        ft.Ejecutar_strSQL(MyConn, " delete from jsmertramer where " _
            & " origen = '" & Origen & "' and " _
            & " date_format(fechamov, '%Y-%m-%d') >= '" & ft.FormatoFechaMySQL(txtFechaDesde.Value) & "' and " _
            & " date_format(fechamov, '%Y-%m-%d') <= '" & ft.FormatoFechaMySQL(txtFechaHasta.Value) & "' and " _
            & IIf(txtProveedorDesde.Text <> "", " prov_cli >= '" & txtProveedorDesde.Text & "' and ", "") _
            & IIf(txtProveedorHasta.Text <> "", " prov_cli <= '" & txtProveedorHasta.Text & "' and ", "") _
            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & " ID_EMP = '" & jytsistema.WorkID & "' ")


    End Sub

    Private Sub ProcesarNotasCreditoCompras()

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

        EliminaMovimientos("NCC")
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " NOTAS DE CREDITO COMPRAS  ")


        ds = DataSetRequery(ds, " select * from jsproencncr where " _
            & " emision >= '" & ft.FormatoFechaMySQL(txtFechaDesde.Value) & "' and " _
            & " emision <= '" & ft.FormatoFechaMySQL(txtFechaHasta.Value) & "' and " _
            & IIf(txtProveedorDesde.Text <> "", " codpro >= '" & txtProveedorDesde.Text & "' and ", "") _
            & IIf(txtProveedorHasta.Text <> "", " codpro <= '" & txtProveedorHasta.Text & "' and ", "") _
            & " ID_EMP = '" & jytsistema.WorkID & "' " _
            & "", MyConn, nTablaEncab, lblInfo)

        dtEncab = ds.Tables(nTablaEncab)


        If dtEncab.Rows.Count > 0 Then
            For iCont = 0 To dtEncab.Rows.Count - 1
                With dtEncab.Rows(iCont)

                    NumeroFactura = .Item("NUMNCR")

                    ds = DataSetRequery(ds, " SELECT * FROM jsprorenncr where " _
                                     & " numncr = '" & NumeroFactura & "' AND " _
                                     & " codpro = '" & .Item("CODPRO") & "' AND " _
                                     & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                     & " ID_EMP = '" & jytsistema.WorkID & "' " _
                                     & "", MyConn, nTablaRenglones, lblInfo)

                    dtRenglones = ds.Tables(nTablaRenglones)
                    If chkMercancias.Checked = True Then
                        If dtRenglones.Rows.Count > 0 Then
                            For kCont = 0 To dtRenglones.Rows.Count - 1

                                If Mid(dtRenglones.Rows(kCont).Item("item"), 1, 1) <> "$" Then
                                    Costo = dtRenglones.Rows(kCont).Item("CANTIDAD") * UltimoCostoAFecha(MyConn, dtRenglones.Rows(kCont).Item("ITEM"), CDate(.Item("emision").ToString)) / Equivalencia(MyConn, dtRenglones.Rows(kCont).Item("ITEM"), dtRenglones.Rows(kCont).Item("UNIDAD"))
                                    If ft.DevuelveScalarCadena(MyConn, " select codart from jsmertramer where codart = '" & dtRenglones.Rows(kCont).Item("ITEM") & "' and tipomov = 'SA' and numdoc = '" & .Item("NUMNCR") & "' and prov_cli = '" & .Item("CODPRO") & "' and origen = 'NCC' and asiento = '" & dtRenglones.Rows(kCont).Item("RENGLON") & dtRenglones.Rows(kCont).Item("ESTATUS") & "' and id_emp = '" & jytsistema.WorkID & "' ") <> 0 Then

                                        InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, dtRenglones.Rows(kCont).Item("ITEM"), .Item("emision"), "EN",
                                               dtRenglones.Rows(kCont).Item("numncr"), dtRenglones.Rows(kCont).Item("UNIDAD"), dtRenglones.Rows(kCont).Item("CANTIDAD"),
                                               dtRenglones.Rows(kCont).Item("PESO"), Costo, Costo, "NCC", dtRenglones.Rows(kCont).Item("numncr"), IIf(IsDBNull(dtRenglones.Rows(kCont).Item("LOTE")), "", dtRenglones.Rows(kCont).Item("lote")),
                                               .Item("codpro"), dtRenglones.Rows(kCont).Item("TOTREN"), dtRenglones.Rows(kCont).Item("TOTRENDES"), 0.0#, dtRenglones.Rows(kCont).Item("TOTREN") - dtRenglones.Rows(kCont).Item("TOTRENDES"),
                                               .Item("codven"), .Item("almacen"), dtRenglones.Rows(kCont).Item("RENGLON"), jytsistema.sFechadeTrabajo)

                                    End If
                                End If
                                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(iCont / dtEncab.Rows.Count * 100),
                                                              NumeroFactura & " : " & dtRenglones.Rows(kCont).Item("ITEM") & " " & dtRenglones.Rows(kCont).Item("DESCRIP"))
                            Next
                        End If
                    End If

                    If chkCxP.Checked = True Then
                        If ft.DevuelveScalarEntero(MyConn, " SELECT count(*) from jsprotrapag " _
                                                    & " where " _
                                                    & " codpro = '" & .Item("codpro") & "' and " _
                                                    & " tipomov = 'NC' and " _
                                                    & " nummov = '" & .Item("numncr") & "' and " _
                                                    & "  numorg = '" & .Item("numncr") & "' and " _
                                                    & " id_emp = '" & jytsistema.WorkID & "'") = 0 Then

                            strTipo = "NC"
                            strTipo1 = "NOTA CREDITO N° "
                            sFOTipo = "1"
                            sSigno = 1


                            InsertEditCOMPRASCXP(MyConn, lblInfo, True, .Item("codpro"), strTipo, .Item("numNCR"), CDate(.Item("emision").ToString), ft.FormatoHora(Now()),
                                CDate(.Item("vence").ToString), "", strTipo1 & ": " & .Item("numNCR"), sSigno * .Item("tot_NCR"), .Item("imp_iva"),
                                .Item("formapag"), .Item("numpag"), .Item("nompag"), "", "NCR", "", "", "", .Item("caja"), .Item("numNCR"), "0",
                                "", jytsistema.sFechadeTrabajo, .Item("CODCON"), "", "", 0.0#, 0.0#, "", "", "", "", .Item("codven"),
                                .Item("codven"), 0, sFOTipo, "0")

                            SaldoCxP(MyConn, lblInfo, .Item("codpro"))

                        End If

                    End If
                End With
            Next
        End If

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 0, "")

    End Sub
    Private Sub ProcesarComprasPlus()
        Dim dtEncab, dtRenglones As DataTable
        Dim nTablaEncab As String = "tblEncab"
        Dim nTablaRenglones As String = "tblRenglones"

        Dim iCont As Integer
        Dim NumeroFactura As String
        Dim CodigoProveedor As String = ""
        Dim Almacen As String = ""
        Dim Emision As Date = MyDate

        pb2.Visible = True

        ds = DataSetRequery(ds, " select * from jsproenccom where " _
           & " emision >= '" & ft.FormatoFechaMySQL(txtFechaDesde.Value) & "' and " _
           & " emision <= '" & ft.FormatoFechaMySQL(txtFechaHasta.Value) & "' and " _
           & IIf(txtProveedorDesde.Text <> "", " codpro >= '" & txtProveedorDesde.Text & "' and ", "") _
           & IIf(txtProveedorHasta.Text <> "", " codpro <= '" & txtProveedorHasta.Text & "' and ", "") _
           & " ID_EMP = '" & jytsistema.WorkID & "' " _
           & "", MyConn, nTablaEncab, lblInfo)

        dtEncab = ds.Tables(nTablaEncab)

        If dtEncab.Rows.Count > 0 Then
            For iCont = 0 To dtEncab.Rows.Count - 1

                refrescaBarraprogresoEtiqueta(pb2, Nothing, CInt(iCont / dtEncab.Rows.Count * 100), "")

                With dtEncab.Rows(iCont)

                    NumeroFactura = .Item("NUMCOM")
                    CodigoProveedor = .Item("CODPRO")
                    Almacen = .Item("ALMACEN")
                    Emision = CDate(.Item("EMISION").ToString)

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100,
                                                  " COMPRA N° : " & NumeroFactura & " - PROVEEDOR : " & CodigoProveedor)

                    ds = DataSetRequery(ds, " SELECT * FROM jsprorencom where " _
                       & " NUMCOM = '" & NumeroFactura & "' AND " _
                       & " CODPRO = '" & CodigoProveedor & "' AND " _
                       & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                       & " ID_EMP = '" & jytsistema.WorkID & "' " _
                       & "", MyConn, nTablaRenglones, lblInfo)

                    dtRenglones = ds.Tables(nTablaRenglones)
                    If dtRenglones.Rows.Count > 0 Then

                        EliminarMovimientosdeInventario(MyConn, NumeroFactura, "COM", lblInfo, , , CodigoProveedor)
                        Dim xx As String = lblProgreso.Text
                        '3.- Actualizar Movimientos de Inventario con los del gasto
                        For Each dtRow As DataRow In dtRenglones.Rows
                            With dtRow
                                If .Item("item").ToString.Substring(0, 1) <> "$" Then
                                    InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, .Item("item"), Emision, "EN", NumeroFactura,
                                                                         .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("costotot"),
                                                                         .Item("costototdes"), "COM", NumeroFactura, .Item("lote"), CodigoProveedor,
                                                                          0.0, 0.0, 0, 0.0, "",
                                                                         Almacen, .Item("renglon"), jytsistema.sFechadeTrabajo)

                                    Dim MontoUltimaCompra As Double = UltimoCostoAFecha(MyConn, .Item("item"), jytsistema.sFechadeTrabajo) ' IIf(.Item("cantidad") > 0, .Item("costototdes") / .Item("cantidad"), .Item("costototdes")) / Equivalencia(myConn,  .Item("item"), .Item("unidad"))
                                    Dim nUltimoProveedor As String = UltimoProveedor(MyConn, lblInfo, .Item("item"), jytsistema.sFechadeTrabajo)
                                    Dim nUltimaFechaCompra As Date = UltimaFechaCompra(MyConn, lblInfo, .Item("item"), jytsistema.sFechadeTrabajo)

                                    ft.Ejecutar_strSQL(MyConn, " update jsmerctainv set fecultcosto = '" & ft.FormatoFechaMySQL(nUltimaFechaCompra) _
                                                   & "', ultimoproveedor = '" & nUltimoProveedor _
                                                   & "', montoultimacompra = " & MontoUltimaCompra & " where codart = '" & .Item("item") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                                End If

                                'ActualizarCostosEnMovmimientos(.Item("item"), xx)

                            End With
                        Next
                    End If

                    dtRenglones.Dispose()

                End With
            Next

        End If

        dtEncab.Dispose()
        dtEncab = Nothing
        dtRenglones = Nothing

    End Sub
    Private Sub ActualizarCostosEnMovmimientos(CodigoArticulo As String, xx As String)
        Dim dtMovSal As DataTable
        Dim nTablaaMovSal As String = "tblmovsalida"

        ds = DataSetRequery(ds, " select * " _
                            & " from jsmertramer " _
                            & " where " _
                            & " fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(txtFechaDesde.Value) & "' and " _
                            & " fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(txtFechaHasta.Value) & "' and " _
                            & " codart = '" & CodigoArticulo & "' AND " _
                            & " origen in ('FAC', 'PVE', 'NDV', 'TRF', 'INV') AND " _
                            & " tipomov in ('SA', 'AS' ) AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'  " _
                            & " UNION select * " _
                            & " from jsmertramer " _
                            & " where " _
                            & " fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(txtFechaDesde.Value) & "' and " _
                            & " fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(txtFechaHasta.Value) & "' and " _
                            & " codart = '" & CodigoArticulo & "' AND " _
                            & " origen in ('NCV') AND " _
                            & " tipomov in ('EN') AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'  " _
                            & " ORDER BY FECHAMOV ", MyConn, nTablaaMovSal, lblInfo)

        dtMovSal = ds.Tables(nTablaaMovSal)

        Dim jCont As Integer = 0
        For jCont = 0 To dtMovSal.Rows.Count - 1

            With dtMovSal.Rows(jCont)

                ProgressBar1.Value = CInt(jCont / dtMovSal.Rows.Count * 100)
                lblProgreso.Text = xx & " ARTICULO : " & .Item("CODART") & " " & .Item("numdoc") & " " & .Item("fechamov").ToString

                Dim nCosto As Double = UltimoCostoAFecha(MyConn, .Item("codart"), CDate(.Item("fechamov").ToString))
                Dim nEquivale As Double = Equivalencia(MyConn, .Item("codart"), .Item("unidad"))
                Dim Descuento As Double = 0


                Dim Costotal As Double = nCosto * .Item("cantidad") / IIf(nEquivale = 0, 1, nEquivale)
                Dim CostotalDescuento As Double = nCosto * (1 - Descuento / 100) * .Item("cantidad") / IIf(nEquivale = 0, 1, nEquivale)

                InsertEditMERCASMovimientoInventario(MyConn, lblInfo, False, .Item("codart"), CDate(.Item("fechamov").ToString),
                                                      .Item("tipomov"), .Item("numdoc"), .Item("unidad"), .Item("cantidad"), .Item("peso"),
                                                      Costotal, CostotalDescuento, .Item("origen"), .Item("numorg"),
                                                      .Item("lote"), .Item("prov_cli"), .Item("ventotal"), .Item("ventotaldes"),
                                                      .Item("impiva"), .Item("descuento"), .Item("vendedor"), .Item("almacen"),
                                                      .Item("asiento"), CDate(.Item("fechasi").ToString))

                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, "")

            End With
        Next

        dtMovSal.Dispose()
        dtMovSal = Nothing

    End Sub

    Private Sub ProcesarFacturas()

        Dim dtEncab, dtRenglones As DataTable
        Dim nTablaEncab As String = "tblEncab"
        Dim nTablaRenglones As String = "tblRenglones"

        Dim iCont, kCont As Integer
        Dim NumeroFactura As String
        Dim CodigoProveedor As String = ""
        Dim Costo As Double

        Dim strTipo As String
        Dim strTipo1 As String
        Dim sFOTipo As String
        Dim sSigno As Integer

        EliminaMovimientos("COM")
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " COMPRAS  ")

        ds = DataSetRequery(ds, " select * from jsproenccom where " _
            & " emision >= '" & ft.FormatoFechaMySQL(txtFechaDesde.Value) & "' and " _
            & " emision <= '" & ft.FormatoFechaMySQL(txtFechaHasta.Value) & "' and " _
            & IIf(txtProveedorDesde.Text <> "", " codpro >= '" & txtProveedorDesde.Text & "' and ", "") _
            & IIf(txtProveedorHasta.Text <> "", " codpro <= '" & txtProveedorHasta.Text & "' and ", "") _
            & " ID_EMP = '" & jytsistema.WorkID & "' " _
            & "", MyConn, nTablaEncab, lblInfo)

        dtEncab = ds.Tables(nTablaEncab)

        If dtEncab.Rows.Count > 0 Then
            For iCont = 0 To dtEncab.Rows.Count - 1

                With dtEncab.Rows(iCont)

                    NumeroFactura = .Item("NUMCOM")
                    CodigoProveedor = .Item("CODPRO")

                    ds = DataSetRequery(ds, " SELECT * FROM jsprorencom where " _
                        & " NUMCOM = '" & NumeroFactura & "' AND " _
                        & " CODPRO = '" & CodigoProveedor & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' " _
                        & "", MyConn, nTablaRenglones, lblInfo)

                    dtRenglones = ds.Tables(nTablaRenglones)
                    If chkMercancias.Checked = True Then
                        If dtRenglones.Rows.Count > 0 Then
                            For kCont = 0 To dtRenglones.Rows.Count - 1



                                If Mid(dtRenglones.Rows(kCont).Item("item"), 1, 1) <> "$" Then
                                    Costo = dtRenglones.Rows(kCont).Item("CANTIDAD") * UltimoCostoAFecha(MyConn, dtRenglones.Rows(kCont).Item("ITEM"), CDate(.Item("emision").ToString)) / Equivalencia(MyConn, dtRenglones.Rows(kCont).Item("ITEM"), dtRenglones.Rows(kCont).Item("UNIDAD"))

                                    Dim ExisteONO As String = ft.DevuelveScalarCadena(MyConn, " select codart from jsmertramer where codart = '" & dtRenglones.Rows(kCont).Item("ITEM") _
                                                             & "' and tipomov = 'EN' and numdoc = '" & .Item("NUMCOM") _
                                                             & "' and ORIGEN = 'COM' AND prov_cli = '" & .Item("CODPRO") & "' and ASIENTO = '" &
                                                             dtRenglones.Rows(kCont).Item("RENGLON") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                                    If ExisteONO = "" Then

                                        InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, dtRenglones.Rows(kCont).Item("ITEM"), CDate(.Item("emision").ToString), "EN",
                                        dtRenglones.Rows(kCont).Item("NUMCOM"), dtRenglones.Rows(kCont).Item("UNIDAD"), dtRenglones.Rows(kCont).Item("CANTIDAD"),
                                        dtRenglones.Rows(kCont).Item("PESO"), Costo, Costo, "COM", dtRenglones.Rows(kCont).Item("NUMCOM"), IIf(IsDBNull(dtRenglones.Rows(kCont).Item("LOTE")), "", dtRenglones.Rows(kCont).Item("lote")),
                                        .Item("codPRO"), dtRenglones.Rows(kCont).Item("COSTOTOT"), dtRenglones.Rows(kCont).Item("COSTOTOTDES"), 0.0#, dtRenglones.Rows(kCont).Item("COSTOTOT") - dtRenglones.Rows(kCont).Item("COSTOTOTDES"),
                                        "", .Item("almacen"), dtRenglones.Rows(kCont).Item("RENGLON"), jytsistema.sFechadeTrabajo)

                                    End If
                                End If
                                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(iCont / dtEncab.Rows.Count * 100),
                                                              NumeroFactura & " : " & dtRenglones.Rows(kCont).Item("ITEM") & " " & dtRenglones.Rows(kCont).Item("DESCRIP")
 )
                            Next
                        End If
                    End If
                    If chkCxP.Checked = True Then
                        If ft.DevuelveScalarEntero(MyConn, " SELECT count(*) from jsprotrapag where codpro = '" &
                                                 .Item("codpro") & "' and tipomov = 'FC' and origen = 'COM' and nummov = '" &
                                                 .Item("NUMCOM") & "' and numorg = '" & .Item("NUMCOM") & "' and id_emp = '" &
                                                 jytsistema.WorkID & "'") = 0 Then

                            strTipo = "FC"
                            strTipo1 = "COMPRA N° "
                            sFOTipo = "0"
                            sSigno = -1


                            InsertEditCOMPRASCXP(MyConn, lblInfo, True, .Item("CODPRO"), strTipo, .Item("NUMCOM"), CDate(.Item("emision").ToString), ft.FormatoHora(Now()),
                                CDate(.Item("vence").ToString), "", strTipo1 & ": " & .Item("numfac"), sSigno * .Item("TOT_COM"), .Item("IMP_IVA"),
                                .Item("formapag"), .Item("numpag"), .Item("nompag"), "", "COM", "", "", "", .Item("CAJA"), .Item("NUMCOM"), "0",
                                "", jytsistema.sFechadeTrabajo, .Item("CODCON"), "", "", 0.0#, 0.0#, "", "", "", "", .Item("codven"),
                                .Item("codven"), 0, sFOTipo, "0")

                            SaldoCxP(MyConn, lblInfo, .Item("codpro"))

                        End If

                    End If

                End With
            Next
        End If

        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 0, "")

    End Sub
    Private Sub ProcesarFacturasGastos()

        Dim dtEncab, dtRenglones As DataTable
        Dim nTablaEncab As String = "tblEncab"
        Dim nTablaRenglones As String = "tblRenglones"

        Dim iCont, kCont As Integer
        Dim NumeroFactura As String = ""
        Dim CodigoProveedor As String = ""
        Dim Costo As Double

        EliminaMovimientos("GAS")
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 100, " FACTURAS GASTOS ")

        ds = DataSetRequery(ds, " select * from jsproencgas where " _
            & " emision >= '" & ft.FormatoFechaMySQL(txtFechaDesde.Value) & "' and " _
            & " emision <= '" & ft.FormatoFechaMySQL(txtFechaHasta.Value) & "' and " _
            & IIf(txtProveedorDesde.Text <> "", " codpro >= '" & txtProveedorDesde.Text & "' and ", "") _
            & IIf(txtProveedorHasta.Text <> "", " codpro <= '" & txtProveedorHasta.Text & "' and ", "") _
            & " ID_EMP = '" & jytsistema.WorkID & "' " _
            & "", MyConn, nTablaEncab, lblInfo)

        dtEncab = ds.Tables(nTablaEncab)

        If dtEncab.Rows.Count > 0 Then
            For iCont = 0 To dtEncab.Rows.Count - 1

                With dtEncab.Rows(iCont)

                    NumeroFactura = .Item("NUMGAS")
                    CodigoProveedor = .Item("CODPRO")

                    ds = DataSetRequery(ds, " SELECT * FROM jsprorengas where " _
                        & " NUMGAS = '" & NumeroFactura & "' AND " _
                        & " CODPRO = '" & CodigoProveedor & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' " _
                        & "", MyConn, nTablaRenglones, lblInfo)

                    dtRenglones = ds.Tables(nTablaRenglones)
                    If chkMercancias.Checked = True Then
                        If dtRenglones.Rows.Count > 0 Then
                            For kCont = 0 To dtRenglones.Rows.Count - 1


                                If Mid(dtRenglones.Rows(kCont).Item("item"), 1, 1) <> "$" Then
                                    Costo = dtRenglones.Rows(kCont).Item("CANTIDAD") * UltimoCostoAFecha(MyConn, dtRenglones.Rows(kCont).Item("ITEM"), CDate(.Item("emision").ToString)) / Equivalencia(MyConn, dtRenglones.Rows(kCont).Item("ITEM"), dtRenglones.Rows(kCont).Item("UNIDAD"))
                                    If ft.DevuelveScalarCadena(MyConn, " select codart from jsmertramer where codart = '" & dtRenglones.Rows(kCont).Item("ITEM") & "' and tipomov = 'EN' AND ORIGEN = 'GAS' and numdoc = '" & .Item("NUMGAS") & "' and prov_cli = '" & .Item("CODPRO") & "' and asiento = '" & dtRenglones.Rows(kCont).Item("RENGLON") & dtRenglones.Rows(kCont).Item("ESTATUS") & "' and id_emp = '" & jytsistema.WorkID & "' ") <> 0 Then

                                        InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, dtRenglones.Rows(kCont).Item("ITEM"), .Item("emision"), "EN",
                                        dtRenglones.Rows(kCont).Item("NUMGAS"), dtRenglones.Rows(kCont).Item("UNIDAD"), dtRenglones.Rows(kCont).Item("CANTIDAD"),
                                        dtRenglones.Rows(kCont).Item("PESO"), Costo, Costo, "GAS", dtRenglones.Rows(kCont).Item("NUMGAS"), IIf(IsDBNull(dtRenglones.Rows(kCont).Item("LOTE")), "", dtRenglones.Rows(kCont).Item("lote")),
                                        .Item("CODPRO"), dtRenglones.Rows(kCont).Item("COSTOTOT"), dtRenglones.Rows(kCont).Item("COSTOTOTDES"), 0.0#, dtRenglones.Rows(kCont).Item("COSTOTOT") - dtRenglones.Rows(kCont).Item("COSTOTOTDES"),
                                        .Item("codven"), .Item("almacen"), dtRenglones.Rows(kCont).Item("RENGLON") & dtRenglones.Rows(kCont).Item("ESTATUS"), jytsistema.sFechadeTrabajo)

                                    End If
                                End If
                                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(iCont / dtEncab.Rows.Count * 100),
                                         NumeroFactura & " : " & dtRenglones.Rows(kCont).Item("ITEM") & " " & dtRenglones.Rows(kCont).Item("DESCRIP"))

                            Next
                        End If
                    End If

                End With
            Next
        End If
        refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, 0, "")

    End Sub
    Private Sub txtClienteDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProveedorDesde.TextChanged
        txtProveedorHasta.Text = txtProveedorDesde.Text
    End Sub

    Private Sub txtFechaDesde_ValueChanged(sender As Object, e As Events.DateTimeValueChangedEventArgs) Handles txtFechaDesde.ValueChanged
        txtFechaHasta.Value = txtFechaDesde.Value
    End Sub
End Class