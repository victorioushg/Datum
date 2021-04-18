Imports MySql.Data.MySqlClient
Public Class jsComProComprasInventarioR
    Private Const sModulo As String = "Compras de Inventarios"
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

    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal TipoProceso As Integer, Optional ByVal CodProveedor As String = "")
        myConn = MyCon

        tbl = "tbl" & ft.NumeroAleatorio(100000)

        ft.mensajeEtiqueta(lblInfo, " Escoja el o los documentos cuyas mercancías se reversaran de los Inventarios ...", Transportables.TipoMensaje.iAyuda)

        txtFechaProceso.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)

        IniciarCompras()

        'Dim iRow As DataGridViewRow
        'For Each iRow In dg.Rows
        '    iRow.Selected = False
        'Next

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
            & " a.recibido = 1 AND " _
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
        For Each selectedItem As DataGridViewRow In dg.Rows
            iCont += 1

            refrescaBarraprogresoEtiqueta(pb, lblProgreso, iCont / dg.RowCount * 100, "procesando..." & selectedItem.Cells(2).Value.ToString)
            If Not CBool(selectedItem.Cells(0).Value) Then

                ft.Ejecutar_strSQL(myConn, "UPDATE jsproenccom SET vence3 = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', " _
                                & " RECIBIDO = 0 " _
                                & " where " _
                                & " NUMCOM = '" & selectedItem.Cells(2).Value & "' and " _
                                & " CODPRO = '" & selectedItem.Cells(3).Value & "' and " _
                                & " ID_EMP ='" & jytsistema.WorkID & "' ")

                ActualizarInventarios(selectedItem.Cells(2).Value.ToString, selectedItem.Cells(3).Value.ToString)

            End If

        Next
        lblProgreso.Text = ""

    End Sub
    Private Function Validado() As Boolean
        Validado = False

        Validado = True

    End Function

    Private Sub CalculaTotales()


    End Sub

    Private Sub dg_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick
        If e.ColumnIndex = 0 Then
            dt.Rows(e.RowIndex).Item(0) = Not CBool(dt.Rows(e.RowIndex).Item(0).ToString)
        End If

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

    Private Sub ActualizarInventarios(ByVal NumeroCompra As String, ByVal CodigoProveedor As String)

        '1.- Elimina movimientos de inventarios anterior compra
        EliminarMovimientosdeInventario(myConn, NumeroCompra, "COM", lblInfo, , , CodigoProveedor)

        '2.- Actualizar Movimientos de Inventario con los renglones de la compra
        Dim dtRenglones As DataTable
        Dim tblRenglones As String = "tblRenglones"
        ds = DataSetRequery(ds, " select * from jsprorencom where codpro = '" & CodigoProveedor & "' and numcom = '" & NumeroCompra & "' and id_emp = '" & jytsistema.WorkID & "' ", myConn, tblRenglones, lblInfo)
        dtRenglones = ds.Tables(tblRenglones)

        Dim codAlmacen As String = ft.DevuelveScalarCadena(myConn, " select almacen from jsproenccom where numcom = '" & NumeroCompra & "' and codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "'")

        For Each dtRow As DataRow In dtRenglones.Rows
            With dtRow
                'If .Item("item").ToString.Substring(0, 1) <> "$" Then
                '    InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(txtFechaProceso.Text), "EN", NumeroCompra, _
                '                                         .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("costotot"), _
                '                                         .Item("costototdes"), "COM", NumeroCompra, .Item("lote"), CodigoProveedor, _
                '                                          0.0, 0.0, 0, 0.0, "", _
                '                                         codAlmacen, .Item("renglon"), jytsistema.sFechadeTrabajo)

                '    Dim MontoUltimaCompra As Double = IIf(.Item("cantidad") > 0, .Item("costototdes") / .Item("cantidad"), .Item("costototdes")) / Equivalencia(myConn,  .Item("item"), .Item("unidad"))

                '    ft.Ejecutar_strSQL ( myconn, " update jsmerctainv set fecultcosto = '" & ft.FormatoFechaMySQL(CDate(txtFechaProceso.Text)) & "', ultimoproveedor = '" & CodigoProveedor & "', montoultimacompra = " & MontoUltimaCompra & " where codart = '" & .Item("item") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                'End If

                ActualizarExistenciasPlus(myConn, .Item("item"))

            End With
        Next

        dtRenglones.Dispose()
        dtRenglones = Nothing

    End Sub


End Class