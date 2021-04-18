Imports MySql.Data.MySqlClient
Public Class jsMerProCargarMercanciasConteo
    Private Const sModulo As String = "Cargar Mercancías en conteo de inventario"
    Private Const nTabla As String = "tblProConteo"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private ConteoNumero As String
    Private aTipo() As String = {"Venta", "Uso interno", "POP", "Alquiler", "Préstamo", "Materia prima", "Otros"}
    Private aCondicion() As String = {"Activo", "Inactivo", "Todas"}
    Private TipoUnidad As Integer
    Private CodigoAlmacen As String
    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal NumeroConteo As String, ByVal CodAlmacen As String, _
                      tipUnidad As Integer)

        myConn = MyCon
        Me.Tag = sModulo & IIf(CodAlmacen = "", " de todos los almacenes", " del almacén " & CodAlmacen)
        ConteoNumero = NumeroConteo
        ft.RellenaCombo(aTipo, cmbTipo)
        ft.RellenaCombo(aCondicion, cmbCondicion)
        TipoUnidad = tipUnidad

        CodigoAlmacen = CodAlmacen


        ft.mensajeEtiqueta(lblInfo, "Seleccione categorías, marcas, división o código de mercancías para el proceso ... ", Transportables.tipoMensaje.iAyuda)
        Me.ShowDialog()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub jsMerProCargarMercanciasConteo_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsMerProCargarMercanciasConteo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Reprocesar()
        ft.mensajeInformativo(" Proceso culminado ...")
        ProgressBar1.Value = 0
        lblProgreso.Text = ""
        Me.Close()

    End Sub

    Private Sub Reprocesar()

        Dim hCont As Integer
        Dim strFilter As String = ""

        If txtCategoriaDesde.Text <> "" Then strFilter += " a.grupo >= '" & txtCategoriaDesde.Text & "' and "
        If txtCategoriaHasta.Text <> "" Then strFilter += " a.grupo <= '" & txtCategoriaHasta.Text & "' and "
        If txtMarcaDesde.Text <> "" Then strFilter += " a.marca >= '" & txtMarcaDesde.Text & "' and "
        If txtMarcaHasta.Text <> "" Then strFilter += " a.marca <= '" & txtMarcaDesde.Text & "' and "
        If txtJerarquiaDesde.Text <> "" Then strFilter += " a.tipjer >= '" & txtJerarquiaDesde.Text & "' and "
        If txtJerarquiaHasta.Text <> "" Then strFilter += " a.tipjer <= '" & txtJerarquiaHasta.Text & "' and "
        If txtDivisionDesde.Text <> "" Then strFilter += " a.division >= '" & txtDivisionDesde.Text & "' and "
        If txtDivisionHasta.Text <> "" Then strFilter += " a.division <= '" & txtDivisionHasta.Text & "' and "
        If txtCodigoDesde.Text <> "" Then strFilter += " a.codart >= '" & txtCodigoDesde.Text & "' and "
        If txtCodigoHasta.Text <> "" Then strFilter += " a.codart <= '" & txtCodigoHasta.Text & "' and "
        strFilter += " a.tipoart = " & cmbTipo.SelectedIndex & " and "
        If cmbCondicion.SelectedIndex <> 2 Then strFilter += " a.estatus = " & cmbCondicion.SelectedIndex & " and "
        If CodigoAlmacen <> "" Then strFilter += " b.almacen = '" & CodigoAlmacen & "' and "

        ds = DataSetRequery(ds, " SELECT a.codart, a.nomart, a.UNIDAD, a.UNIDADDETAL, a.montoultimacompra,  " _
                            & " a.montoultimacompra / IF( c.equivale IS NULL ,1, c.equivale) montoultimacompraDetal,  " _
                            & " SUM(IF(b.existencia IS NULL , 0.000, b.existencia)) existencia, " _
                            & " SUM(IF(b.existencia IS NULL , 0.000, b.existencia)) / IF( c.equivale IS NULL ,1, c.equivale) existenciaDetal " _
                            & " FROM jsmerctainv a " _
                            & " LEFT JOIN jsmerextalm b ON (a.codart = b.codart AND a.id_emp = b.id_emp) " _
                            & " LEFT JOIN jsmerequmer c ON (a.codart = c.codart AND a.unidaddetal = c.uvalencia AND a.id_emp = c.id_emp) " _
                            & " where " _
                            & strFilter _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " GROUP BY a.codart " _
                            & " ORDER BY a.codart ", myConn, nTabla, lblInfo)

        dt = ds.Tables(nTabla)
        If dt.Rows.Count > 0 Then
            For hCont = 0 To dt.Rows.Count - 1
                With dt.Rows(hCont)

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (hCont + 1) / dt.Rows.Count * 100, _
                                                  " Mercancía : " & .Item("codart") & " " & .Item("nomart"))

                    Dim aFld() As String = {"conmer", "codart", "id_emp"}
                    Dim aStr() As String = {ConteoNumero, .Item("codart"), jytsistema.WorkID}
                    If Not qFound(myConn, lblInfo, "jsmerconmer", aFld, aStr) Then

                        Dim Unidad As String = If(TipoUnidad.Equals(0), .Item("UNIDAD"), .Item("UNIDADDETAL"))
                        Dim Existencia As Double = If(TipoUnidad.Equals(0), .Item("EXISTENCIA"), .Item("EXISTENCIADETAL"))
                        Dim Costo As Double = If(TipoUnidad.Equals(0), .Item("MONTOULTIMACOMPRA"), .Item("MONTOULTIMACOMPRADETAL"))

                        InsertEditMERCASRenglonesConteo(myConn, lblInfo, True, ConteoNumero, "", "", .Item("codart"), _
                                                        .Item("nomart"), Unidad, 0.0, Existencia, 0.0, _
                                                         0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, Costo, _
                                                        Math.Round(Existencia * Costo, 2), "1")
                    End If

                End With
            Next
        Else
            ft.MensajeCritico("No existen movimientos para procesar...")
        End If

    End Sub
   

    Private Sub txtCategoriaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCategoriaDesde.TextChanged
        txtCategoriaHasta.Text = txtCategoriaDesde.Text
    End Sub

    Private Sub txtMarcaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMarcaDesde.TextChanged
        txtMarcaHasta.Text = txtMarcaDesde.Text
    End Sub

    Private Sub txtJerarquiaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtJerarquiaDesde.TextChanged
        txtJerarquiaHasta.Text = txtJerarquiaDesde.Text
    End Sub

    Private Sub txtDivisionDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDivisionDesde.TextChanged
        txtDivisionHasta.Text = txtDivisionDesde.Text
    End Sub

    Private Sub txtCodigoDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigoDesde.TextChanged
        txtCodigoHasta.Text = txtCodigoDesde.Text
    End Sub

    Private Sub btnCategoriaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoriaDesde.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Categorías", FormatoTablaSimple(Modulo.iCategoriaMerca), False, TipoCargaFormulario.iShowDialog)
        txtCategoriaDesde.Text = f.Seleccion
        f = Nothing
    End Sub

    Private Sub btnCategoriaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoriaHasta.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Categorías de mercancías", FormatoTablaSimple(Modulo.iCategoriaMerca), False, TipoCargaFormulario.iShowDialog)
        txtCategoriaHasta.Text = f.Seleccion
        f = Nothing
    End Sub

    Private Sub btnMarcaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarcaDesde.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Marcas de mercancías", FormatoTablaSimple(Modulo.iMarcaMerca), False, TipoCargaFormulario.iShowDialog)
        txtMarcaDesde.Text = f.Seleccion
        f = Nothing
    End Sub

    Private Sub btnMarcaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarcaHasta.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Marcas de mercancías", FormatoTablaSimple(Modulo.iMarcaMerca), False, TipoCargaFormulario.iShowDialog)
        txtMarcaHasta.Text = f.Seleccion
        f = Nothing
    End Sub


    Private Sub btnDivisionDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDivisionDesde.Click
        Dim f As New jsControlArcDivisiones
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        txtDivisionDesde.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub btnDivisionHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDivisionHasta.Click
        Dim f As New jsControlArcDivisiones
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        txtDivisionHasta.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub btnCodigoDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigoDesde.Click
        Dim f As New jsMerArcListaCostosPreciosNormal
        f.Cargar(myConn, 0)
        txtCodigoDesde.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub btnCodigoHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigoHasta.Click
        Dim f As New jsMerArcListaCostosPreciosNormal
        f.Cargar(myConn, 0)
        txtCodigoHasta.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub btnJerarquiaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnJerarquiaDesde.Click
        Dim strSQLTipjer As String = " SELECT tipjer codigo, descrip descripcion FROM jsmerencjer WHERE  id_emp  = '" & jytsistema.WorkID & "' order by 1 "
        Dim dtTJ As DataTable
        Dim nTableTJ As String = "tbltipjer"
        ds = DataSetRequery(ds, strSQLTipjer, myConn, nTableTJ, lblInfo)
        dtTJ = ds.Tables(nTableTJ)

        Dim f As New jsControlArcTablaSimple
        f.Cargar(" Tipo de Jerarquía", ds, dtTJ, nTableTJ, TipoCargaFormulario.iShowDialog, False)

        txtJerarquiaDesde.Text = f.Seleccion

        f = Nothing
        dtTJ = Nothing
    End Sub

    Private Sub btnJerarquiaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnJerarquiaHasta.Click
        Dim strSQLTipjer As String = " SELECT tipjer codigo, descrip descripcion FROM jsmerencjer WHERE  id_emp  = '" & jytsistema.WorkID & "' order by 1 "
        Dim dtTJ As DataTable
        Dim nTableTJ As String = "tbltipjer"
        ds = DataSetRequery(ds, strSQLTipjer, myConn, nTableTJ, lblInfo)
        dtTJ = ds.Tables(nTableTJ)

        Dim f As New jsControlArcTablaSimple
        f.Cargar(" Tipo de Jerarquía", ds, dtTJ, nTableTJ, TipoCargaFormulario.iShowDialog, False)

        txtJerarquiaHasta.Text = f.Seleccion

        f = Nothing
        dtTJ = Nothing
    End Sub
End Class