Imports MySql.Data.MySqlClient
Public Class jsComProComprasInventario
    Private Const sModulo As String = "Compras a Inventarios"
    Private Const nTabla As String = "noCompras"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Long

    Private tbl As String = ""

    Private aCampos() As String = {}
    Private aNombres() As String = {}

    Private sFacturas As String = ""

    Private dtRenglones As DataTable
    Private tblRenglones As String = "tblRenglones"

    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal TipoProceso As Integer, Optional ByVal CodProveedor As String = "")
        myConn = MyCon
        
        tbl = "tbl" & ft.NumeroAleatorio(100000)

        ft.mensajeEtiqueta(lblInfo, " Escoja el o los documentos cuyas mercancías se habilitarán en los Inventarios ...", Transportables.TipoMensaje.iAyuda)
      
        txtFechaProceso.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)

        IniciarCompras()

        Dim iRow As DataGridViewRow
        For Each iRow In dg.Rows
            iRow.Selected = False
        Next

        Me.Show()


    End Sub
    Private Sub IniciarCompras()

       
        Dim aFields() As String = {"recibido.entero.1.0", "emision.fecha.0.0", "numcom.cadena.30.0", "codpro.cadena.15.0", "nombre.cadena.250.0", _
                                   "emisioniva.fecha.0.0", "fechasi.fecha.0.0", "id_emp.cadena.2.0"}

        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tbl, aFields)

        ft.Ejecutar_strSQL(myconn, " insert into " & tbl _
            & " SELECT a.recibido, a.emision, a.numcom,  a.codpro, b.nombre, a.emisioniva, a.fechasi, a.id_emp " _
            & " FROM jsproenccom a " _
            & " LEFT JOIN jsprocatpro b ON (a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
            & " WHERE " _
            & " a.recibido = 0 AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY " _
            & " a.emision DESC, a.numcom")


        ds = DataSetRequery(ds, " select * from " & tbl, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        CargarListaDesdeCompras(dg, dt)
        dg.ReadOnly = False
        For Each col As DataGridViewColumn In dg.Columns
            If col.Index > 0 Then col.ReadOnly = True
        Next

        CalculaTotales()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub jsBanDepositarCaja_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        InsertarAuditoria(myConn, MovAud.iSalir, sModulo, txtFechaProceso.Text)
        ft = Nothing
    End Sub

    Private Sub jsBanDepositarCaja_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        InsertarAuditoria(myConn, MovAud.ientrar, sModulo, txtFechaProceso.Text)
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarDeposito()
            ft.mensajeInformativo("Proceso culminado!!!")
            Me.Close()
        End If
    End Sub
    Private Sub GuardarDeposito()
        InsertarAuditoria(myConn, MovAud.iIncluir, sModulo, txtFechaProceso.Text)

        Dim iCont As Integer = 0
        For Each selectedItem As DataGridViewRow In dgMercas.Rows
            iCont += 1

            refrescaBarraprogresoEtiqueta(pb, lblProgreso, iCont / dgMercas.RowCount * 100, _
                                          "procesando..." & selectedItem.Cells(2).Value.ToString)

            Dim CodigoProveedor As String = selectedItem.Cells(1).Value
            Dim NumeroDeCompra As String = selectedItem.Cells(2).Value
            Dim NumeroRenglon As String = selectedItem.Cells(4).Value
            Dim CodigoArticulo As String = selectedItem.Cells(3).Value

            If CBool(selectedItem.Cells(0).Value) Then

                ft.Ejecutar_strSQL(myconn, "UPDATE jsprorencom SET aceptado = '2' " _
                                & " where " _
                                & " renglon = '" & NumeroRenglon & "' AND " _
                                & " NUMCOM = '" & NumeroDeCompra & "' and " _
                                & " CODPRO = '" & CodigoProveedor & "' and " _
                                & " ID_EMP ='" & jytsistema.WorkID & "' ")

                ActualizarInventarios(NumeroDeCompra, CodigoProveedor, CodigoArticulo, NumeroRenglon)
                VerificaCompraRecibida(NumeroDeCompra, CodigoProveedor)

            End If

        Next

        lblProgreso.Text = ""


    End Sub
    Private Sub VerificaCompraRecibida(NumeroDeCompra As String, CodigoProveedor As String)

        If ft.DevuelveScalarEntero(myConn, " select count(*) from jsprorencom where aceptado = '1' and numcom = '" & NumeroDeCompra & "' and codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then

            ft.Ejecutar_strSQL(myConn, "UPDATE jsproenccom SET vence3 = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                            & " RECIBIDO = 1 " _
                            & " where " _
                            & " NUMCOM = '" & NumeroDeCompra & "' and " _
                            & " CODPRO = '" & CodigoProveedor & "' and " _
                            & " ID_EMP ='" & jytsistema.WorkID & "' ")
        Else

            ft.Ejecutar_strSQL(myConn, "UPDATE jsproenccom SET vence3 = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "' " _
                              & " where " _
                              & " NUMCOM = '" & NumeroDeCompra & "' and " _
                              & " CODPRO = '" & CodigoProveedor & "' and " _
                              & " ID_EMP ='" & jytsistema.WorkID & "' ")

        End If

    End Sub

    Private Function Validado() As Boolean

        Return True

    End Function

    Private Sub CalculaTotales()

        If dg.Rows.Count > 0 Then

            For Each selectedItem As DataGridViewRow In dg.Rows
                If CBool(selectedItem.Cells(0).Value) Then
                    sFacturas += " ( numcom = '" & selectedItem.Cells(2).Value & "' AND codpro = '" & selectedItem.Cells(3).Value & "' ) AND "
                End If
            Next

            If sFacturas.Trim <> "" Then AbrirMercanciasFacturas(sFacturas)

        End If

    End Sub

    Private Sub AbrirMercanciasFacturas(strFacturas As String)


        Dim nTablaFacturas As String = "tbltmp" & ft.NumeroAleatorio(1000000)
        Dim nTablaFacs As String = "nTablaMercasFacturas"
        Dim aFields() As String = {"sel.entero.1.0", "codpro.cadena.15.0", "numcom.cadena.30.0", "item.cadena.15.0", "renglon.cadena.5.0", "descrip.cadena.250.0", _
                                   "cantidad.doble.10.3", "unidad.cadena.3.0", "peso.cadena.10.0"}
        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, nTablaFacturas, aFields)

        ft.Ejecutar_strSQL(myconn, " insert into " & nTablaFacturas & "  " _
                        & " select a.aceptado, a.codpro, a.numcom, a.item, a.renglon, a.descrip, a.cantidad, a.unidad, a.peso " _
                        & " from jsprorencom a " _
                        & " where " _
                        & strFacturas _
                        & " a.aceptado = '1' and " _
                        & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' " _
                        & " order by " _
                        & " a.CODPRO, a.numcom, a.renglon ")


        Dim strSQLFacturas As String = " select * from " & nTablaFacturas & " order by codpro, numcom, renglon "

        ds = DataSetRequery(ds, strSQLFacturas, myConn, nTablaFacs, lblInfo)
        dtRenglones = ds.Tables(nTablaFacs)

        Dim aCampos() As String = {"sel", "codpro", "numcom", "item", "renglon", "descrip", "cantidad", "unidad"}
        Dim aNombres() As String = {"", "Proveedor", "Documento", "Item", "Renglón", "Descripción", "Cantidad", "UND"}
        Dim aAnchos() As Integer = {20, 100, 100, 100, 60, 350, 120, 50}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Centro}

        Dim aFormatos() As String = {"", "", "", "", "", "", sFormatoNumero, ""}
        IniciarTablaSeleccion(dgMercas, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

        dgMercas.ReadOnly = False
        For Each col As DataGridViewColumn In dgMercas.Columns
            If col.Index > 0 Then col.ReadOnly = True
        Next

    End Sub

    Private Sub dg_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick
        If e.ColumnIndex = 0 Then
            dt.Rows(e.RowIndex).Item(0) = Not CBool(dt.Rows(e.RowIndex).Item(0).ToString)
        End If

    End Sub

    Private Sub dgMercas_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgMercas.CellContentClick
        If e.ColumnIndex = 0 Then
            dtRenglones.Rows(e.RowIndex).Item(0) = Not CBool(dtRenglones.Rows(e.RowIndex).Item(0).ToString)
        End If

    End Sub

    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
       dg.ColumnHeaderMouseClick


        'If dtDepos.Columns(e.ColumnIndex).DataType Is GetType(String) Then
        '    FindField = dtDepos.Columns(dg.Columns(e.ColumnIndex).Name).ColumnName
        '    lblBuscar.Text = "Buscando por : " & aNombres(e.ColumnIndex)
        'Else
        '    MsgBox("NO PUEDE BUSCAR POR ESTE TIPO DE COLUMNA", MsgBoxStyle.Information)
        'End If

        'txtBuscar.Focus()
        'ft.enfocarTexto(txtBuscar)

    End Sub

    Private Sub dg_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellClick

    End Sub

    Private Sub dg_CellMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.CellMouseUp
        CalculaTotales()
    End Sub

    Private Sub dg_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellValidated
        If dg.CurrentCell.ColumnIndex = 0 Then

            ft.Ejecutar_strSQL(myconn, " update  " & tbl & " set recibido  = " & CInt(dg.CurrentCell.Value) & " " _
                            & " where " _
                            & " numcom = '" & CStr(dg.CurrentRow.Cells(2).Value) & "' and " _
                            & " codpro = '" & CStr(dg.CurrentRow.Cells(3).Value) & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")


        End If

    End Sub

    Private Sub btnFecha_Click(sender As System.Object, e As System.EventArgs) Handles btnFecha.Click
        txtFechaProceso.Text = SeleccionaFecha(CDate(txtFechaProceso.Text), Me, sender)
    End Sub


    Private Sub ActualizarInventarios(ByVal NumeroCompra As String, ByVal CodigoProveedor As String, CodigoArticulo As String, NumeroRenglon As String)

        '1.- Elimina movimientos de inventarios anterior compra
        EliminarMovimientosdeInventario(myConn, NumeroCompra, "COM", lblInfo, , , CodigoProveedor, CodigoArticulo, NumeroRenglon)

        '2.- Actualizar Movimientos de Inventario con los renglones de la compra
        Dim dtRens As DataTable
        Dim tblRenglones As String = "tblRenglones"
        ds = DataSetRequery(ds, " select * from jsprorencom where codpro = '" & CodigoProveedor & "' and " _
                            & " numcom = '" & NumeroCompra & "' and " _
                            & " renglon = '" & NumeroRenglon & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ", myConn, tblRenglones, lblInfo)

        dtRens = ds.Tables(tblRenglones)

        Dim codAlmacen As String = ft.DevuelveScalarCadena(myConn, " select almacen from jsproenccom where numcom = '" & NumeroCompra & "' and codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "'")

        For Each dtRow As DataRow In dtRens.Rows
            With dtRow
                If .Item("item").ToString.Substring(0, 1) <> "$" Then
                    InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(txtFechaProceso.Text), "EN", NumeroCompra, _
                                                         .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("costotot"), _
                                                         .Item("costototdes"), "COM", NumeroCompra, .Item("lote"), CodigoProveedor, _
                                                          0.0, 0.0, 0, 0.0, "", _
                                                         codAlmacen, .Item("renglon"), jytsistema.sFechadeTrabajo)

                    Dim MontoUltimaCompra As Double = IIf(.Item("cantidad") > 0, .Item("costototdes") / .Item("cantidad"), .Item("costototdes")) / Equivalencia(myConn, .Item("item"), .Item("unidad"))

                    ft.Ejecutar_strSQL(myConn, " update jsmerctainv set fecultcosto = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', ultimoproveedor = '" & CodigoProveedor & "', montoultimacompra = " & MontoUltimaCompra & " where codart = '" & .Item("item") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                End If

                ActualizarExistenciasPlus(myConn, .Item("item"))

            End With
        Next

        dtRens.Dispose()
        dtRens = Nothing

    End Sub


End Class