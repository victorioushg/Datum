Imports MySql.Data.MySqlClient
Public Class jsVenProMercanciasEnDespachos
    Private Const sModulo As String = "Mercancías en Despachos"
    Private Const nTabla As String = "tblMercanciasEnDespachos"
    Private Const lRegion As String = "RibbonButton272"


    Private strSQLMov As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dtRenglones As New DataTable

    Private i_modo As Integer
    Private nPosicion As Long

    Private tblTemp As String = "tbl" & NumeroAleatorio(100000)

    
    Private Sub jsVenProMercanciasEnDespachos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo
        Try
            myConn.Open()



            HabilitarObjetos(False, True, txtAsesorDesde, txtAsesorHasta, txtFechaDesde, txtFechaHasta, txtMercanciaDesde, txtMercanciaHasta)
            txtFechaDesde.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
            txtFechaHasta.Text = FormatoFecha(jytsistema.sFechadeTrabajo)


        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
  
    Private Sub AsignarMovimientos()

        Dim aFld() As String = {"numped.cadena15", "item.cadena15", "descrip.cadena150", "cantidad.doble10", "cantran.doble10", "unidad.cadena5", "emision.fecha", "codcli.cadena15", "nombre.cadena250", "renglon.cadena5", "precio.doble19", "des_art.doble6", "des_cli.doble6", "des_ofe.doble6"}
        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblTemp, aFld, " numped ")
        EjecutarSTRSQL(myConn, lblInfo, " INSERT INTO " & tblTemp _
                        & " SELECT a.numped, a.item, a.descrip, a.cantidad, a.cantran, a.unidad, b.emision, b.codcli, c.nombre, a.renglon, a.precio, a.des_art, a.des_cli, a.des_ofe " _
                        & " FROM jsvenrenped a " _
                        & " LEFT JOIN jsvenencped b  ON (a.numped = b.numped AND a.id_emp = b.id_emp) " _
                        & " LEFT JOIN jsvencatcli c ON ( b.codcli = c.codcli AND b.id_emp = c.id_emp) " _
                        & " WHERE " _
                        & " a.item >= '" & txtMercanciaDesde.Text & "' AND  a.item <= '" & txtMercanciaHasta.Text & "' AND " _
                        & " b.codven >= '" & txtAsesorDesde.Text & "' AND b.codven <= '" & txtAsesorHasta.Text & "' AND " _
                        & " b.emision >= '" & FormatoFechaMySQL(CDate(txtFechaDesde.Text)) & "' AND " _
                        & " b.emision <= '" & FormatoFechaMySQL(CDate(txtFechaHasta.Text)) & "' AND " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' ")

        strSQLMov = " SELECT * FROM " & tblTemp & " ORDER BY numped "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTabla, lblInfo)
        dtRenglones = ds.Tables(nTabla)
        Dim aCampos() As String = {"numped", "item", "descrip", "cantran", "unidad", "emision", "codcli", "nombre"}
        Dim aNombres() As String = {"Número Factura", "Item", "Descripción", "Cantidad Tránsito", "Unidad", "Emisión", "Cliente", ""}
        Dim aAnchos() As Long = {100, 100, 370, 100, 50, 100, 100, 300}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                       AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", sFormatoCantidad, "", sFormatoFecha, "", ""}
        IniciarTabla(dg, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, , True)
        If dtRenglones.Rows.Count > 0 Then nPosicion = 0

        dg.ReadOnly = False
        dg.Columns("numped").ReadOnly = True
        dg.Columns("item").ReadOnly = True
        dg.Columns("descrip").ReadOnly = True
        dg.Columns("unidad").ReadOnly = True
        dg.Columns("emision").ReadOnly = True
        dg.Columns("codcli").ReadOnly = True
        dg.Columns("nombre").ReadOnly = True
        dg.Columns("cantran").ReadOnly = False


    End Sub



    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        'If i_modo = movimiento.iAgregar Then AgregaYCancela()

        'If dt.Rows.Count = 0 Then
        '    IniciarDocumento(False)
        'Else
        '    Me.BindingContext(ds, nTabla).CancelCurrentEdit()
        '    AsignaTXT(nPosicionEncab)
        'End If

        'DesactivarMarco0()
        Me.Close()

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado() As Boolean

        If txtAsesorDesde.Text = "" Or txtAsesorHasta.Text = "" Then
            MensajeCritico(lblInfo, "Debe INDICAR un asesor válido...")
            Return False
        End If

        If txtMercanciaDesde.Text = "" Or txtMercanciaHasta.Text = "" Then
            MensajeCritico(lblInfo, "Debe INDICAR una mercancía válida...")
            Return False
        End If

        Return True

    End Function
    Private Sub GuardarTXT()


        dtRenglones.AcceptChanges()

        For dCont As Integer = 0 To dtRenglones.Rows.Count - 1

            With dtRenglones.Rows(dCont)
                '1.- ACTUALIZAR RENGLONES EN PEDIDO . OJO. DESCUENTOS POR RENGLON
                If .Item("cantran") > 0 Then
                    Dim TotalRenglon As Double = Math.Round(.Item("cantran") * .Item("precio") * (1 - .Item("des_art") / 100) * (1 - .Item("des_cli") / 100) * (1 - .Item("des_ofe") / 100), 2)

                    EjecutarSTRSQL(myConn, lblInfo, " update jsvenrenped set cantidad = " & .Item("cantran") _
                                    & ", cantran = " & .Item("cantran") & ", totren = " & TotalRenglon & ", totrendes = " & TotalRenglon & " " _
                                    & " where " _
                                    & " numped = '" & .Item("numped") & "' and " _
                                    & " renglon = '" & .Item("renglon") & "' and " _
                                    & " item = '" & .Item("item") & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "' ")
                Else
                    EjecutarSTRSQL(myConn, lblInfo, " delete from  jsvenrenped " _
                                   & " where " _
                                   & " numped = '" & .Item("numped") & "' and " _
                                   & " renglon = '" & .Item("renglon") & "' and " _
                                   & " item = '" & .Item("item") & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' ")
                End If

                Dim SubtotalPedido As Double = CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrenped", "numped", "totren", .Item("numped"), 0)
                Dim DescuentoGlobal As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(descuento) from jsvendesped where numped = '" & .Item("numped") & "' and id_emp = '" & jytsistema.WorkID & "' group by numped "))
                Dim Cargos As Double = CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrenped", "numped", "totren", .Item("numped"), 1)

                EjecutarSTRSQL_Scalar(myConn, lblInfo, " update jsvenrenped set totrendes = totren - totren * " & DescuentoGlobal / IIf(SubtotalPedido > 0, SubtotalPedido, 1) & " " _
                    & " where " _
                    & " numped = '" & .Item("numped") & "' and " _
                    & " renglon > '' and " _
                    & " item > '' and " _
                    & " ESTATUS = '0' AND " _
                    & " ACEPTADO < '2' and " _
                    & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
                    & " ID_EMP = '" & jytsistema.WorkID & "' ")

                CalculaTotalIVAVentas(myConn, lblInfo, "jsvendesped", "jsvenivaped", "jsvenrenped", "numped", .Item("numped"), "impiva", "totrendes", CDate(.Item("emision").ToString), "totren")
                Dim TotalIVA As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(impiva) from jsvenivaped where numped = '" & .Item("numped") & "' and id_emp = '" & jytsistema.WorkID & "' group by numped "))

                Dim TotalPedido As Double = SubtotalPedido - DescuentoGlobal + Cargos + TotalIVA
                Dim TotalPesoPedido As Double = CalculaPesoDocumento(myConn, lblInfo, "jsvenrenped", "peso", "numped", .Item("numped"))

                'ACTUALIZA ENCABEZADO
                EjecutarSTRSQL_Scalar(myConn, lblInfo, " UPDATE jsvenencped set TOT_NET = " & SubtotalPedido & ", DESCUEN = " & DescuentoGlobal & ", CARGOS = " & Cargos & ", IMP_IVA = " & TotalIVA & " , TOT_PED = " & TotalPedido & ", KILOS = " & TotalPesoPedido & "  WHERE numped = '" & .Item("numped") & "' and id_emp = '" & jytsistema.WorkID & "'  ")


            End With


        Next


    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFechaHasta.GotFocus
        MensajeEtiqueta(lblInfo, " Indique comentario ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub btnFechaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaDesde.Click
        txtFechaDesde.Text = SeleccionaFecha(CDate(txtFechaDesde.Text), Me, grpEncab, btnFechaDesde)
    End Sub

    Private Sub btnFechaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFechaHasta.Click
        txtFechaHasta.Text = SeleccionaFecha(CDate(txtFechaHasta.Text), Me, grpEncab, btnFechaHasta)
    End Sub

    Private Sub btnMercanciaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMercanciaDesde.Click
        txtMercanciaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codart codigo, nomart descripcion from jsmerctainv where id_emp = '" & jytsistema.WorkID & "' order by codart ", "Mercancías", txtMercanciaDesde.Text)
    End Sub

    Private Sub btnMercanciaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMercanciaHasta.Click
        txtMercanciaHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codart codigo, nomart descripcion from jsmerctainv where id_emp = '" & jytsistema.WorkID & "' order by codart ", "Mercancías", txtMercanciaHasta.Text)
    End Sub

    Private Sub btnAsesorDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesorDesde.Click
        txtAsesorDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, concat(apellidos, ' ', nombres) descripcion from jsvencatven where tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by codven ", "Asesores Comerciales", txtAsesorDesde.Text)
    End Sub

    Private Sub btnAsesorHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesorHasta.Click
        txtAsesorHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, concat(apellidos, ' ', nombres) descripcion from jsvencatven where tipo = 0 and id_emp = '" & jytsistema.WorkID & "' order by codven ", "Asesores Comerciales", txtAsesorHasta.Text)
    End Sub

    Private Sub txtFechaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFechaDesde.TextChanged
        txtFechaHasta.Text = txtFechaDesde.Text
    End Sub

    Private Sub txtMercanciaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMercanciaDesde.TextChanged
        txtMercanciaHasta.Text = txtMercanciaDesde.Text
    End Sub

    Private Sub txtAsesorDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAsesorDesde.TextChanged
        txtAsesorHasta.Text = txtAsesorDesde.Text
    End Sub

    Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGo.Click
        If Validado() Then
            AsignarMovimientos()
        End If
    End Sub
    Private Sub dg_CancelRowEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.QuestionEventArgs) Handles dg.CancelRowEdit
        MensajeAdvertencia(lblInfo, "CANCELANDO...")
    End Sub
    Private Sub dg_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
            Handles dg.CellValidating

        If e.ColumnIndex = 3 Then
            If (String.IsNullOrEmpty(e.FormattedValue.ToString())) Then
                MensajeCritico(lblInfo, "Debe indicar dígito(s) válido...")
                e.Cancel = True
            End If

            If Not IsNumeric(e.FormattedValue.ToString()) Then
                MensajeCritico(lblInfo, "Debe indicar un número válido...")
                e.Cancel = True
            End If
        End If

    End Sub
    Private Sub dg_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub
End Class