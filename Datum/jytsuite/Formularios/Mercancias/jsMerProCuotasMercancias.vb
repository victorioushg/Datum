Imports MySql.Data.MySqlClient
Public Class jsMerProCuotasMercancias
    Private Const sModulo As String = "Construcci�n de cuotas de mercanc�as"
    Private Const nTabla As String = "tblProReconstruir"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private ft As New Transportables

    Private aTipoAsignacion() As String = {"Cuotas por porcentaje",
                                           "Cuotas por promedio mensual/porcentaje"}

    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        myConn = MyCon
        lblLeyenda.Text = " 1. Este proceso incluye dentro de la tabla CUOTAS ANUALES de mercanc�as, aquellos art�culos o mercanc�as " + vbCrLf + _
                          "    que no se encuentran en ella de acuerdo a cualquiera de los m�todos � tipos de asignaci�n. "

        ft.mensajeEtiqueta(lblInfo, "Verifique la fecha desde/hasta que desea reconstruir ...", Transportables.TipoMensaje.iAyuda)
        ft.visualizarObjetos(True, txtMercanciaDesde, btnMercanciaDesde, txtMercanciaHasta, btnMercanciaHasta, txtCategoriaDesde, btnCategoriaDesde, _
            txtCategoriaHasta, btnCategoriaHasta, txtMarcaDesde, btnMarcaDesde, txtMarcaHasta, btnMarcaHasta, txtDivisionDesde, _
            btnDivisionDesde, txtDivisionHasta, btnDivisionHasta, txtTipoJerarquia, btnTipoJerarquia, txtCodjer1, btnCodjer1, _
            txtCodjer2, btnCodjer2, txtCodjer3, txtCodjer4, txtCodjer5, txtCodjer6, btnCodjer3, btnCodjer4, btnCodjer5, btnCodjer6)


        ft.habilitarObjetos(False, False, txtMercanciaDesde, txtMercanciaHasta, txtCategoriaDesde, txtCategoriaHasta, txtMarcaDesde, txtMarcaHasta, txtDivisionDesde, _
                         txtDivisionHasta, txtTipoJerarquia, txtCodjer1, txtCodjer2, txtCodjer3, txtCodjer4, txtCodjer5, txtCodjer6)

        txtMercanciaDesde.Text = ""
        txtMercanciaHasta.Text = ""

        ft.RellenaCombo(aTipoAsignacion, cmbTipoAsignacion)
     
        txtPorcentaje.Text = ft.FormatoNumero(0.0)

        Me.Show()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub jsMerProCuotasMercancias_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsMerProCuotasMercancias_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If ValorNumero(txtPorcentaje.Text) > 0 Then
            Procesar()
            ft.mensajeInformativo(" Proceso culminado ...")
            ProgressBar1.Value = 0
            lblProgreso.Text = ""
            Me.Close()
        Else
            ft.MensajeCritico("Debe indicar un porcentaje de crecimiento v�lido...")
        End If

    End Sub

    Private Sub Procesar()

        Dim dtItems As DataTable
        Dim strFilter As String = ""

        If txtMercanciaDesde.Text <> "" Then strFilter += " codart >= '" & txtMercanciaDesde.Text & "' and "
        If txtMercanciaHasta.Text <> "" Then strFilter += " codart <= '" & txtMercanciaHasta.Text & "' and  "
        If txtCategoriaDesde.Text <> "" Then strFilter += " grupo >= '" & txtCategoriaDesde.Text & "' and "
        If txtCategoriaHasta.Text <> "" Then strFilter += " grupo <= '" & txtCategoriaHasta.Text & "' and  "
        If txtMarcaDesde.Text <> "" Then strFilter += " marca >= '" & txtMarcaDesde.Text & "' and "
        If txtMarcaHasta.Text <> "" Then strFilter += " marca <= '" & txtMarcaHasta.Text & "' and  "
        If txtDivisionDesde.Text <> "" Then strFilter += " division >= '" & txtDivisionDesde.Text & "' and "
        If txtDivisionHasta.Text <> "" Then strFilter += " division <= '" & txtDivisionHasta.Text & "' and  "
        If txtTipoJerarquia.Text <> "" Then strFilter += " tipjer = '" & txtTipoJerarquia.Text & "' and "
        If txtCodjer1.Text <> "" Then strFilter += " codjer1 = '" & txtCodjer1.Text & "' and  "
        If txtCodjer2.Text <> "" Then strFilter += " codjer2 = '" & txtCodjer2.Text & "' and "
        If txtCodjer3.Text <> "" Then strFilter += " codjer3 = '" & txtCodjer3.Text & "' and  "
        If txtCodjer4.Text <> "" Then strFilter += " codjer4 = '" & txtCodjer4.Text & "' and "
        If txtCodjer5.Text <> "" Then strFilter += " codjer5 = '" & txtCodjer5.Text & "' and  "
        If txtCodjer6.Text <> "" Then strFilter += " codjer6 = '" & txtCodjer6.Text & "' and "

        ds = DataSetRequery(ds, " select * from jsmerctainv where " _
                            & strFilter _
                            & " id_emp = '" & jytsistema.WorkID & "' order by codart ", _
                            myConn, nTabla, lblInfo)

        dtItems = ds.Tables(nTabla)

        lblTarea.Text = " ACTUALIZANDO CUOTAS DE MERCANCIAS "

        If dtItems.Rows.Count > 0 Then
            For jCont As Integer = 0 To dtItems.Rows.Count - 1
                With dtItems.Rows(jCont)
                    ActualizarCuotas(.Item("codart"))
                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(jCont / dtItems.Rows.Count * 100), " ARTICULO : " & .Item("CODART") & " " & .Item("NOMART"))
                End With
            Next
            ProgressBar1.Value = 0
        End If

        dtItems.Dispose()
        dtItems = Nothing

        lblProgreso.Text = ""
        lblTarea.Text = ""

    End Sub

    Private Sub ActualizarCuotas(ByVal CodigoArticulo As String)

        Dim dEne, dFeb, dMar, dAbr, dMay, dJun, dJul, dAgo, dSep, dOct, dNov, dDic As Double

        Select Case cmbTipoAsignacion.SelectedIndex

            Case 0 'Cuotas por porcentaje"
                dEne = VentasA�oAnterior(CodigoArticulo, 1) * (1 + ValorNumero(txtPorcentaje.Text) / 100)
                dFeb = VentasA�oAnterior(CodigoArticulo, 2) * (1 + ValorNumero(txtPorcentaje.Text) / 100)
                dMar = VentasA�oAnterior(CodigoArticulo, 3) * (1 + ValorNumero(txtPorcentaje.Text) / 100)
                dAbr = VentasA�oAnterior(CodigoArticulo, 4) * (1 + ValorNumero(txtPorcentaje.Text) / 100)
                dMay = VentasA�oAnterior(CodigoArticulo, 5) * (1 + ValorNumero(txtPorcentaje.Text) / 100)
                dJun = VentasA�oAnterior(CodigoArticulo, 6) * (1 + ValorNumero(txtPorcentaje.Text) / 100)
                dJul = VentasA�oAnterior(CodigoArticulo, 7) * (1 + ValorNumero(txtPorcentaje.Text) / 100)
                dAgo = VentasA�oAnterior(CodigoArticulo, 8) * (1 + ValorNumero(txtPorcentaje.Text) / 100)
                dSep = VentasA�oAnterior(CodigoArticulo, 9) * (1 + ValorNumero(txtPorcentaje.Text) / 100)
                dOct = VentasA�oAnterior(CodigoArticulo, 10) * (1 + ValorNumero(txtPorcentaje.Text) / 100)
                dNov = VentasA�oAnterior(CodigoArticulo, 11) * (1 + ValorNumero(txtPorcentaje.Text) / 100)
                dDic = VentasA�oAnterior(CodigoArticulo, 12) * (1 + ValorNumero(txtPorcentaje.Text) / 100)

            Case 1 'Cuotas por promedio mensual/porcentaje",
                dEne = VentasA�oAnterior(CodigoArticulo, 1) + VentasA�oAnterior(CodigoArticulo, 2) + _
                    VentasA�oAnterior(CodigoArticulo, 3) + VentasA�oAnterior(CodigoArticulo, 4) + _
                    VentasA�oAnterior(CodigoArticulo, 5) + VentasA�oAnterior(CodigoArticulo, 6) + _
                    VentasA�oAnterior(CodigoArticulo, 7) + VentasA�oAnterior(CodigoArticulo, 8) + _
                    VentasA�oAnterior(CodigoArticulo, 9) + VentasA�oAnterior(CodigoArticulo, 10) + _
                    VentasA�oAnterior(CodigoArticulo, 11) + VentasA�oAnterior(CodigoArticulo, 12)
                dEne = (dEne / 12) * (1 + ValorNumero(txtPorcentaje.Text) / 100)
                dFeb = dEne
                dMar = dEne
                dAbr = dEne
                dMay = dEne
                dJun = dEne
                dJul = dEne
                dAgo = dEne
                dSep = dEne
                dOct = dEne
                dNov = dEne
                dDic = dEne
            Case 2 'Cuotas fijas en Kilogramos" 
            Case 3 'Cuotas por promedio mensual/kilogramos"
            Case Else
        End Select

        Dim Insertar As Boolean = True
        Dim Existe As Integer = ft.DevuelveScalarEntero(myConn, " select COUNT(*) from jsmerctacuo where codart = '" & CodigoArticulo & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "' ")
        If Existe > 0 Then Insertar = False

        InsertEditMERCASCuotasMercancia(myConn, lblInfo, Insertar, CodigoArticulo, dEne, dFeb, dMar, dAbr, dMay, dJun, dJul, dAgo, dSep, dOct, dNov, dDic)

    End Sub
    Private Function VentasA�oAnterior(ByVal CodigoArticulo As String, ByVal nMes As Integer) As Double

        VentasA�oAnterior = ft.DevuelveScalarDoble(myConn, " SELECT ROUND(SUM(IF( a.origen = 'NCV', -1*a.peso, a.peso)), 3) " _
                                                       & " FROM jsmertramer a " _
                                                       & " WHERE " _
                                                       & " a.codart = '" & CodigoArticulo & "' AND  " _
                                                       & " YEAR(a.fechamov) = " & IIf(nMes <= 12, Year(jytsistema.sFechadeTrabajo) - 1, Year(jytsistema.sFechadeTrabajo)) & " AND " _
                                                       & " MONTH(a.fechamov) = " & IIf(nMes <= 12, nMes, nMes - 12) & " AND " _
                                                       & " a.origen IN ('FAC','NDV','NCV','PVE','PFC') AND " _
                                                       & " a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.codart ")

    End Function


    Private Sub btnMercanciaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMercanciaDesde.Click
        txtMercanciaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codart codigo, nomart descripcion, alterno from jsmerctainv where " _
                                                    & " id_emp = '" & jytsistema.WorkID & "' order by codart ", "Mercanc�as", txtMercanciaDesde.Text)
    End Sub


    Private Sub btnMercanciaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMercanciaHasta.Click
        txtMercanciaHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codart codigo, nomart descripcion, alterno from jsmerctainv where " _
                                                    & " id_emp = '" & jytsistema.WorkID & "' order by codart ", "Mercanc�as", txtMercanciaHasta.Text)
    End Sub


    Private Sub txtMercanciaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMercanciaDesde.TextChanged
        txtMercanciaHasta.Text = txtMercanciaDesde.Text
    End Sub

    Private Sub btnCategoriaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoriaDesde.Click
        txtCategoriaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & _
                                                   FormatoTablaSimple(Modulo.iCategoriaMerca) & "' and id_emp = '" & jytsistema.WorkID & "' order by 1", "Categor�as de mercanc�as", txtCategoriaDesde.Text)

    End Sub

    Private Sub btnCategoriaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoriaHasta.Click
        txtCategoriaHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & _
                                                   FormatoTablaSimple(Modulo.iCategoriaMerca) & "' and id_emp = '" & jytsistema.WorkID & "' order by 1", "Categor�as de mercanc�as", txtCategoriaHasta.Text)
    End Sub

    Private Sub txtCategoriaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCategoriaDesde.TextChanged
        txtCategoriaHasta.Text = txtCategoriaDesde.Text
    End Sub

    Private Sub btnMarcaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarcaDesde.Click
        txtMarcaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & _
                                                   FormatoTablaSimple(Modulo.iMarcaMerca) & "' and id_emp = '" & jytsistema.WorkID & "' order by 1", "Marcas de mercanc�as", txtMarcaDesde.Text)
    End Sub

    Private Sub btnMarcaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarcaHasta.Click
        txtMarcaHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & _
                                                   FormatoTablaSimple(Modulo.iMarcaMerca) & "' and id_emp = '" & jytsistema.WorkID & "' order by 1", "Marcas de mercanc�as", txtMarcaHasta.Text)
    End Sub

    Private Sub txtMarcaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMarcaDesde.TextChanged
        txtMarcaHasta.Text = txtMarcaDesde.Text
    End Sub


    Private Sub btnDivisionDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDivisionDesde.Click
        txtDivisionDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select division codigo, descrip descripcion from jsmercatdiv where  id_emp = '" & jytsistema.WorkID & "' order by 1", "Divisiones de mercanc�as", txtDivisionDesde.Text)
    End Sub

    Private Sub btnDivisionHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDivisionHasta.Click
        txtDivisionHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select division codigo, descrip descripcion from jsmercatdiv where  id_emp = '" & jytsistema.WorkID & "' order by 1", "Divisiones de mercanc�as", txtDivisionHasta.Text)
    End Sub

    Private Sub txtDivisionDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDivisionDesde.TextChanged
        txtDivisionHasta.Text = txtDivisionDesde.Text
    End Sub

    Private Sub btnTipoJerarquia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoJerarquia.Click
        txtTipoJerarquia.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT tipjer codigo, descrip descripcion FROM jsmerencjer WHERE  id_emp  = '" & jytsistema.WorkID & "' order by 1 ", " Tipo de Jerarqu�a", _
                                                          txtTipoJerarquia.Text)
    End Sub

    Private Sub CargarJerarquia(ByVal MyConn As MySqlConnection, ByVal ds As DataSet, ByVal TipoJerarquia As String, ByVal Nivel As Integer, _
                                    ByVal txtCodjer As TextBox)

        If TipoJerarquia <> "" Then
            Dim strSQLCodjer As String = " SELECT codjer codigo, desjer descripcion FROM jsmerrenjer WHERE tipjer = '" & TipoJerarquia & "' and nivel = " & Nivel & " and id_emp  = '" & jytsistema.WorkID & "' order by 1 "
            Dim dtCJ As DataTable
            Dim nTableCJ As String = "tblCodjer"
            ds = DataSetRequery(ds, strSQLCodjer, MyConn, nTableCJ, lblInfo)
            dtCJ = ds.Tables(nTableCJ)
            If dtCJ.Rows.Count > 0 Then
                Dim f As New jsControlArcTablaSimple
                f.Cargar(" Codigo Jerarqu�a nivel " & Nivel & " ", ds, dtCJ, nTableCJ, TipoCargaFormulario.iShowDialog, False)
                txtCodjer.Text = f.Seleccion
                f = Nothing
            End If
            dtCJ = Nothing
        End If

    End Sub

    Private Sub btnCodjer1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodjer1.Click
        CargarJerarquia(myConn, ds, txtTipoJerarquia.Text, 1, txtCodjer1)
    End Sub

    Private Sub btnCodjer2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodjer2.Click
        CargarJerarquia(myConn, ds, txtTipoJerarquia.Text, 2, txtCodjer2)
    End Sub
    Private Sub btnCodjer3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodjer3.Click
        CargarJerarquia(myConn, ds, txtTipoJerarquia.Text, 3, txtCodjer3)
    End Sub

    Private Sub btnCodjer4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodjer4.Click
        CargarJerarquia(myConn, ds, txtTipoJerarquia.Text, 4, txtCodjer4)
    End Sub

    Private Sub btnCodjer5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodjer5.Click
        CargarJerarquia(myConn, ds, txtTipoJerarquia.Text, 5, txtCodjer5)
    End Sub

    Private Sub btnCodjer6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodjer6.Click
        CargarJerarquia(myConn, ds, txtTipoJerarquia.Text, 6, txtCodjer6)
    End Sub

  
    Private Sub txtPorcentaje_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPorcentaje.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub
End Class