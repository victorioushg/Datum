Imports MySql.Data.MySqlClient
Public Class jsMerProReconstruirMovimientosMercancias
    Private Const sModulo As String = "Reconstruir movimientos de mercancías"
    Private Const nTabla As String = "tblProReconstruir"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private ft As New Transportables

    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        myConn = MyCon
        lblLeyenda.Text = " 1. Se reconstruyen los movimientos de mercancías desde Ventas (Notas Entrega, Facturas, Notas crédito, " + vbCrLf + _
                          "    Notas débito), Compras (Recepciones, Compras, Notas Crédito, Notas Débito) y  Puntos de Ventas.  " + vbCrLf + _
                          " 2. Se recalculan las existencias y " + vbCrLf + _
                          " 3. Se recalculan los costos "

        ft.mensajeEtiqueta(lblInfo, "Verifique la fecha desde/hasta que desea reconstruir ...", Transportables.TipoMensaje.iAyuda)
        ft.habilitarObjetos(False, False, txtFechaDesde, txtFechaHasta)

        txtFechaDesde.Text = ft.FormatoFecha(PrimerDiaMes(jytsistema.sFechadeTrabajo))
        txtFechaHasta.Text = ft.FormatoFecha(UltimoDiaMes(jytsistema.sFechadeTrabajo))

        chkActualizaExistencias.Checked = True


        Me.Show()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub jsMerProReconstruirMovimientosMercancias_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsMerProReconstruirMovimientosMercancias_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Procesar()
        ft.mensajeInformativo(" Proceso culminado ...")
        ProgressBar1.Value = 0
        lblProgreso.Text = ""
        Me.Close()

    End Sub

    Private Sub Procesar()

        'ProcesarRecepciones
        'ProcesarCompras
        'ProcesarNotasCreditoCompras
        'ProcesarNotasDebitoCompras

        'ProcesarPrefacturacion
        If chkFacturas.Checked Then ProcesarFacturas()
        'ProcesarNotasCreditoVentas
        'ProcesarNotasDebitoVentas
        'ProcesarPuntoDeVenta

        If chkActualizaExistencias.Checked Then
            If LoginUser(myConn, lblInfo) = "jytsuite" Then
                ActualizandoMercanciasPlus()
            Else
                ActualizandoMercancias()
            End If
        End If

    End Sub
    Private Sub ActualizarCostosEnMovimientos(CodigoArticulo As String)
        Dim dtMovSal As DataTable
        Dim nTablaaMovSal As String = "tblmovsalida"

        ds = DataSetRequery(ds, " select * " _
                            & " from jsmertramer " _
                            & " where " _
                            & " fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(CDate(txtFechaDesde.Text)) & "' and " _
                            & " fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(CDate(txtFechaHasta.Text)) & "' and " _
                            & " codart = '" & CodigoArticulo & "' AND " _
                            & " origen in ('FAC', 'PVE', 'NDV', 'TRF', 'INV') AND " _
                            & " tipomov in ('SA', 'AS' ) AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'  " _
                            & " UNION select * " _
                            & " from jsmertramer " _
                            & " where " _
                            & " fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(CDate(txtFechaDesde.Text)) & "' and " _
                            & " fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(CDate(txtFechaHasta.Text)) & "' and " _
                            & " codart = '" & CodigoArticulo & "' AND " _
                            & " origen in ('NCV') AND " _
                            & " tipomov in ('EN') AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'  " _
                            & " ORDER BY FECHAMOV ", myConn, nTablaaMovSal, lblInfo)

        dtMovSal = ds.Tables(nTablaaMovSal)

        Dim jCont As Integer = 0
        For jCont = 0 To dtMovSal.Rows.Count - 1

            With dtMovSal.Rows(jCont)

                Dim nCosto As Double = UltimoCostoAFecha(myConn, .Item("codart"), CDate(.Item("fechamov").ToString))
                Dim nEquivale As Double = Equivalencia(myConn,  .Item("codart"), .Item("unidad"))
                Dim Descuento As Double = 0


                Dim Costotal As Double = nCosto * .Item("cantidad") / IIf(nEquivale = 0, 1, nEquivale)
                Dim CostotalDescuento As Double = nCosto * (1 - Descuento / 100) * .Item("cantidad") / IIf(nEquivale = 0, 1, nEquivale)

                InsertEditMERCASMovimientoInventario(myConn, lblInfo, False, .Item("codart"), CDate(.Item("fechamov").ToString), _
                                                      .Item("tipomov"), .Item("numdoc"), .Item("unidad"), .Item("cantidad"), .Item("peso"), _
                                                      Costotal, CostotalDescuento, .Item("origen"), .Item("numorg"), _
                                                      .Item("lote"), .Item("prov_cli"), .Item("ventotal"), .Item("ventotaldes"),
                                                      .Item("impiva"), .Item("descuento"), .Item("vendedor"), .Item("almacen"), _
                                                      .Item("asiento"), CDate(.Item("fechasi").ToString))

                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(jCont / dtMovSal.Rows.Count * 100), _
                                              " ARTICULO : " & .Item("CODART") & " " & .Item("numdoc") & " " & .Item("fechamov").ToString)

            End With
        Next

        dtMovSal.Dispose()
        dtMovSal = Nothing

    End Sub
    Private Sub ActualizandoMercanciasPlus()

        Dim dtItems As DataTable
        ds = DataSetRequery(ds, " select * from jsmerctainv where " _
                            & IIf(txtCodigoDesde.Text <> "", " CODART >= '" & txtCodigoDesde.Text & "' AND ", "") _
                            & IIf(txtCodigoHasta.Text <> "", " CODART <= '" & txtCodigoHasta.Text & "' AND ", "") _
                            & " id_emp = '" & jytsistema.WorkID & "' order by codart ", _
                             myConn, nTabla, lblInfo)

        dtItems = ds.Tables(nTabla)

        lblTarea.Text = " ACTUALIZANDO COSTOS EN MOVIMIENTOS "

        If dtItems.Rows.Count > 0 Then
            For jCont As Integer = 0 To dtItems.Rows.Count - 1
                With dtItems.Rows(jCont)
                   
                    ActualizarCostosEnMovimientos(.Item("CODART"))
                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(jCont / dtItems.Rows.Count * 100), _
                                                   " ARTICULO : " & .Item("CODART") & " " & .Item("NOMART"))
                End With
            Next
            ProgressBar1.Value = 0
        End If

        dtItems.Dispose()
        dtItems = Nothing

        lblProgreso.Text = ""
        lblTarea.Text = ""

    End Sub

    Private Sub ActualizandoMercancias()

        Dim dtItems As DataTable
        ds = DataSetRequery(ds, " select * from jsmerctainv where " _
                            & IIf(txtCodigoDesde.Text <> "", " CODART >= '" & txtCodigoDesde.Text & "' AND ", "") _
                            & IIf(txtCodigoHasta.Text <> "", " CODART <= '" & txtCodigoHasta.Text & "' AND ", "") _
                            & " id_emp = '" & jytsistema.WorkID & "' order by codart ", _
                             myConn, nTabla, lblInfo)

        dtItems = ds.Tables(nTabla)

        lblTarea.Text = " ACTUALIZANDO EXISTENCIAS Y COSTOS "

        If dtItems.Rows.Count > 0 Then
            For jCont As Integer = 0 To dtItems.Rows.Count - 1
                With dtItems.Rows(jCont)
                  
                    ActualizarExistenciasPlus(myConn, .Item("CODART"))
                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(jCont / dtItems.Rows.Count * 100), _
                                                  " ARTICULO : " & .Item("CODART") & " " & .Item("NOMART"))
                End With
            Next
            ProgressBar1.Value = 0
        End If

        dtItems.Dispose()
        dtItems = Nothing

        lblProgreso.Text = ""
        lblTarea.Text = ""

    End Sub

    Private Sub ProcesarFacturas()

        Dim dt As DataTable

        ds = DataSetRequery(ds, " SELECT a.numfac, a.emision, a.codcli, a.codven, a.almacen, " _
                            & " b.renglon, b.item, b.descrip, b.cantidad, b.unidad, b.peso, b.lote, b.totren, b.totrendes " _
                            & " FROM jsvenencfac a " _
                            & " LEFT JOIN jsvenrenfac b ON (a.numfac = b.numfac AND a.id_emp = b.id_emp) " _
                            & " WHERE " _
                            & " substring(b.item,1,1) <> '$' and " _
                            & " a.emision >= '" & ft.FormatoFechaMySQL(CDate(txtFechaDesde.Text)) & "' AND " _
                            & " a.emision <= '" & ft.FormatoFechaMySQL(CDate(txtFechaHasta.Text)) & "' AND " _
                            & " a.estatus < 2 AND " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " ORDER BY numfac, renglon ", _
                             myConn, nTabla, lblInfo)

        dt = ds.Tables(nTabla)

        lblTarea.Text = " PROCESANDO FACTURAS "

        If dt.Rows.Count > 0 Then
            For jCont As Integer = 0 To dt.Rows.Count - 1
                With dt.Rows(jCont)

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(jCont / dt.Rows.Count * 100), _
                                                  " FACTURA N° : " & .Item("NUMFAC") & "    ITEM : " & .Item("ITEM") & " " & .Item("DESCRIP"))

                    '1.- Elimina movimientos de inventarios anteriores de la Factura
                    EliminarMovimientosdeInventario(myConn, .Item("NUMFAC"), "FAC", lblInfo, , , , .Item("item"), .Item("renglon"))

                    '2.- Actualizar Movimientos de Inventario con Factura
                    Dim CostoActual As Double = UltimoCostoAFecha(myConn, .Item("item"), CDate(.Item("emision").ToString)) / Equivalencia(myConn, .Item("item"), .Item("unidad"))

                    InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(.Item("emision").ToString), "SA", .Item("numfac"), _
                                                         .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("cantidad") * CostoActual, _
                                                         .Item("cantidad") * CostoActual, "FAC", .Item("numfac"), .Item("lote"), .Item("codcli"), _
                                                         .Item("totren"), .Item("totrendes"), 0, .Item("totren") - .Item("totrendes"), .Item("codven"), _
                                                         .Item("almacen"), .Item("renglon"), jytsistema.sFechadeTrabajo)

                    '3.- Actualizar Existencias 
                    ActualizarExistenciasPlus(myConn, .Item("item"))

                End With
            Next
            ProgressBar1.Value = 0
        End If

        dt.Dispose()
        dt = Nothing

        lblProgreso.Text = ""
        lblTarea.Text = ""

    End Sub

    Private Sub btnFechaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaDesde.Click
        txtFechaDesde.Text = SeleccionaFecha(CDate(txtFechaDesde.Text), Me, grpCaja, btnFechaDesde)
    End Sub


    Private Sub btnFechaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaHasta.Click
        txtFechaHasta.Text = SeleccionaFecha(CDate(txtFechaHasta.Text), Me, grpCaja, btnFechaHasta)
    End Sub


    Private Sub btnCodigoDesde_Click(sender As System.Object, e As System.EventArgs) Handles btnCodigoDesde.Click
        txtCodigoDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " Select codart codigo, nomart descripcion, alterno from jsmerctainv where id_emp = '" & jytsistema.WorkID & "' order by codart ", "MERCANCIAS...", txtCodigoDesde.Text)
    End Sub

    Private Sub btnCodigoHasta_Click(sender As System.Object, e As System.EventArgs) Handles btnCodigoHasta.Click
        txtCodigoHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " Select codart codigo, nomart descripcion, alterno from jsmerctainv where id_emp = '" & jytsistema.WorkID & "' order by codart ", "MERCANCIAS...", txtCodigoDesde.Text)
    End Sub

    Private Sub txtCodigoDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCodigoDesde.TextChanged
        txtCodigoHasta.Text = txtCodigoDesde.Text
    End Sub

    Private Sub txtFechaDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtFechaDesde.TextChanged
        txtFechaHasta.Text = txtFechaDesde.Text
    End Sub
End Class