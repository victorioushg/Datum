Imports MySql.Data.MySqlClient
Public Class jsMerProReconstruirGananciasMercancias
    Private Const sModulo As String = "Reconstruir ganancias de mercancías"
    Private Const nTabla As String = "tblProReconstruirGanancias"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private ft As New Transportables

    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        myConn = MyCon
        lblLeyenda.Text = " 1. Se coloca la nuevas ganancias y se recalculan los nuvos precios A PARTIR DEL ULTIMO COSTO."

        ft.mensajeEtiqueta(lblInfo, "Verifique la fecha desde/hasta que desea reconstruir ...", Transportables.TipoMensaje.iAyuda)
        ft.visualizarObjetos(True, txtMercanciaDesde, btnMercanciaDesde, txtMercanciaHasta, btnMercanciaHasta, txtCategoriaDesde, btnCategoriaDesde, _
            txtCategoriaHasta, btnCategoriaHasta, txtMarcaDesde, btnMarcaDesde, txtMarcaHasta, btnMarcaHasta, txtDivisionDesde, _
            btnDivisionDesde, txtDivisionHasta, btnDivisionHasta, txtTipoJerarquia, btnTipoJerarquia, txtCodjer1, btnCodjer1, _
            txtCodjer2, btnCodjer2, txtCodjer3, txtCodjer4, txtCodjer5, txtCodjer6, btnCodjer3, btnCodjer4, btnCodjer5, btnCodjer6)


        ft.habilitarObjetos(False, False, txtMercanciaDesde, txtMercanciaHasta, txtCategoriaDesde, txtCategoriaHasta, txtMarcaDesde, txtMarcaHasta, txtDivisionDesde, _
                         txtDivisionHasta, txtTipoJerarquia, txtCodjer1, txtCodjer2, txtCodjer3, txtCodjer4, txtCodjer5, txtCodjer6)

        txtMercanciaDesde.Text = ""
        txtMercanciaHasta.Text = ""

        txtporGanA.Text = ft.FormatoNumero(0.0) : chkA.Checked = False
        txtporGanB.Text = ft.FormatoNumero(0.0) : chkB.Checked = False
        txtporGanC.Text = ft.FormatoNumero(0.0) : chkC.Checked = False
        txtporGanD.Text = ft.FormatoNumero(0.0) : chkD.Checked = False
        txtporGanE.Text = ft.FormatoNumero(0.0) : chkE.Checked = False
        txtporGanF.Text = ft.FormatoNumero(0.0) : chkF.Checked = False

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

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        sRespuesta = MsgBox(" ¿ DESEA CALCULAR PRECIOS POR FACTOR DIVISION ?", MsgBoxStyle.YesNo, sModulo)
        If sRespuesta = MsgBoxResult.Yes Then
            ProcesarPorGanancias(True)
        Else
            ProcesarPorGanancias(False)
        End If

        ft.mensajeInformativo(" Proceso culminado ...")
        ProgressBar1.Value = 0
        lblProgreso.Text = ""

    End Sub

    Private Sub ProcesarPorGanancias(ByVal PorDivisison As Boolean)

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
                            & " estatus = 0 and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by codart ", _
                             myConn, nTabla, lblInfo)

        dtItems = ds.Tables(nTabla)

        lblTarea.Text = " ACTUALIZANDO PRECIOS "

        If dtItems.Rows.Count > 0 Then
            For jCont As Integer = 0 To dtItems.Rows.Count - 1
                With dtItems.Rows(jCont)

                    ActualizarGanancias(myConn, lblInfo, .Item("codart"), _
                                        ValorNumero(txtporGanA.Text), chkA.Checked, _
                                         ValorNumero(txtporGanB.Text), chkB.Checked, _
                                          ValorNumero(txtporGanC.Text), chkC.Checked, _
                                           ValorNumero(txtporGanD.Text), chkD.Checked, _
                                            ValorNumero(txtporGanE.Text), chkE.Checked, _
                                             ValorNumero(txtporGanF.Text), chkF.Checked, _
                                        PorDivisison)
                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(jCont / dtItems.Rows.Count * 100), _
                                                  " ARTICULO : " & .Item("CODART") & " " & .Item("NOMART"))
                End With
            Next
            ProgressBar1.Value = 0
            ft.mensajeInformativo("Proceso culminado!!!")
        End If

        dtItems.Dispose()
        dtItems = Nothing

        lblProgreso.Text = ""
        lblTarea.Text = ""

    End Sub
    Private Sub ActualizarGanancias(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoArticulo As String, _
                                 ByVal GananciaA As Double, ActualizaA As Boolean, GananciaB As Double, ActualizaB As Boolean, _
                                 GananciaC As Double, ActualizaC As Boolean, GananciaD As Double, ActualizaD As Boolean, _
                                 GananciaE As Double, ActualizaE As Boolean, GananciaF As Double, ActualizaF As Boolean, _
                                 porDivision As Boolean)

        Dim UltimoCosto As Double = ft.DevuelveScalarDoble(MyConn, " SELECT montoultimacompra from jsmerctainv where " _
                                                           & " codart = '" & CodigoArticulo & "' and id_emp = '" & jytsistema.WorkID & "' ")


        ft.Ejecutar_strSQL(myconn, " update jsmerctainv set id_emp = '" & jytsistema.WorkID & "' " _
                                & IIf(ActualizaA, IIf(porDivision, ", PRECIO_A = " & UltimoCosto / (1 - GananciaA / 100) & " ", ", PRECIO_A = " & UltimoCosto * (1 + GananciaA / 100) & " "), "") _
                                & IIf(ActualizaB, IIf(porDivision, ", PRECIO_B = " & UltimoCosto / (1 - GananciaB / 100) & " ", ", PRECIO_B = " & UltimoCosto * (1 + GananciaB / 100) & " "), "") _
                                & IIf(ActualizaC, IIf(porDivision, ", PRECIO_C = " & UltimoCosto / (1 - GananciaC / 100) & " ", ", PRECIO_C = " & UltimoCosto * (1 + GananciaC / 100) & " "), "") _
                                & IIf(ActualizaD, IIf(porDivision, ", PRECIO_D = " & UltimoCosto / (1 - GananciaD / 100) & " ", ", PRECIO_D = " & UltimoCosto * (1 + GananciaD / 100) & " "), "") _
                                & IIf(ActualizaE, IIf(porDivision, ", PRECIO_E = " & UltimoCosto / (1 - GananciaE / 100) & " ", ", PRECIO_E = " & UltimoCosto * (1 + GananciaE / 100) & " "), "") _
                                & IIf(ActualizaF, IIf(porDivision, ", PRECIO_F = " & UltimoCosto / (1 - GananciaF / 100) & " ", ", PRECIO_F = " & UltimoCosto * (1 + GananciaF / 100) & " "), "") _
                                & IIf(ActualizaA, ", GANAN_A = " & GananciaA & " ", "") _
                                & IIf(ActualizaB, ", GANAN_B = " & GananciaB & " ", "") _
                                & IIf(ActualizaC, ", GANAN_C = " & GananciaC & " ", "") _
                                & IIf(ActualizaD, ", GANAN_D = " & GananciaD & " ", "") _
                                & IIf(ActualizaE, ", GANAN_E = " & GananciaE & " ", "") _
                                & IIf(ActualizaF, ", GANAN_F = " & GananciaF & " ", "") _
                                & " where codart = '" & CodigoArticulo & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub


    Private Sub btnMercanciaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMercanciaDesde.Click
        txtMercanciaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codart codigo, nomart descripcion, alterno from jsmerctainv where " _
                                                    & " id_emp = '" & jytsistema.WorkID & "' order by codart ", "Mercancías", txtMercanciaDesde.Text)
    End Sub


    Private Sub btnMercanciaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMercanciaHasta.Click
        txtMercanciaHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codart codigo, nomart descripcion, alterno from jsmerctainv where " _
                                                    & " id_emp = '" & jytsistema.WorkID & "' order by codart ", "Mercancías", txtMercanciaHasta.Text)
    End Sub


    Private Sub txtMercanciaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMercanciaDesde.TextChanged
        txtMercanciaHasta.Text = txtMercanciaDesde.Text
    End Sub

    Private Sub btnCategoriaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoriaDesde.Click
        txtCategoriaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & _
                                                   FormatoTablaSimple(Modulo.iCategoriaMerca) & "' and id_emp = '" & jytsistema.WorkID & "' order by 1", "Categorías de mercancías", txtCategoriaDesde.Text)

    End Sub

    Private Sub btnCategoriaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoriaHasta.Click
        txtCategoriaHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & _
                                                   FormatoTablaSimple(Modulo.iCategoriaMerca) & "' and id_emp = '" & jytsistema.WorkID & "' order by 1", "Categorías de mercancías", txtCategoriaHasta.Text)
    End Sub

    Private Sub txtCategoriaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCategoriaDesde.TextChanged
        txtCategoriaHasta.Text = txtCategoriaDesde.Text
    End Sub

    Private Sub btnMarcaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarcaDesde.Click
        txtMarcaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & _
                                                   FormatoTablaSimple(Modulo.iMarcaMerca) & "' and id_emp = '" & jytsistema.WorkID & "' order by 1", "Marcas de mercancías", txtMarcaDesde.Text)
    End Sub

    Private Sub btnMarcaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarcaHasta.Click
        txtMarcaHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & _
                                                   FormatoTablaSimple(Modulo.iMarcaMerca) & "' and id_emp = '" & jytsistema.WorkID & "' order by 1", "Marcas de mercancías", txtMarcaHasta.Text)
    End Sub

    Private Sub txtMarcaDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMarcaDesde.TextChanged
        txtMarcaHasta.Text = txtMarcaDesde.Text
    End Sub


    Private Sub btnDivisionDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDivisionDesde.Click
        txtDivisionDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " select division codigo, descrip descripcion from jsmercatdiv where  id_emp = '" & jytsistema.WorkID & "' order by 1", "Divisiones de mercancías", txtDivisionDesde.Text)
    End Sub

    Private Sub btnDivisionHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDivisionHasta.Click
        txtDivisionHasta.Text = CargarTablaSimple(myConn, lblInfo, ds, " select division codigo, descrip descripcion from jsmercatdiv where  id_emp = '" & jytsistema.WorkID & "' order by 1", "Divisiones de mercancías", txtDivisionHasta.Text)
    End Sub

    Private Sub txtDivisionDesde_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDivisionDesde.TextChanged
        txtDivisionHasta.Text = txtDivisionDesde.Text
    End Sub

    Private Sub btnTipoJerarquia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTipoJerarquia.Click
        txtTipoJerarquia.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT tipjer codigo, descrip descripcion FROM jsmerencjer WHERE  id_emp  = '" & jytsistema.WorkID & "' order by 1 ", " Tipo de Jerarquía", _
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
                f.Cargar(" Codigo Jerarquía nivel " & Nivel & " ", ds, dtCJ, nTableCJ, TipoCargaFormulario.iShowDialog, False)
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

    Private Sub txtImporte_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtporGanA.KeyPress, _
        txtporGanB.KeyPress, txtporGanC.KeyPress, txtporGanD.KeyPress, txtporGanE.KeyPress, txtporGanF.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub
End Class