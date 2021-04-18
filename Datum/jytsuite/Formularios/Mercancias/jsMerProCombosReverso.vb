Imports MySql.Data.MySqlClient
Public Class jsMerProCombosReverso

    Private Const sModulo As String = "Reverso de combos de mercancías "

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private nTabla As String = "tblCombosparaReverso"
    Private ft As New Transportables

    Private strSQL As String = ""
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        MyConn = MyCon

        IniciarTXT()

        Me.Show()

    End Sub
    Private Sub IniciarTXT()

        ft.habilitarObjetos(False, True, txtAlmacen)
        txtAlmacen.Text = ""


    End Sub

    Private Sub jsMerProCombosReverso_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsVenProGuiaDespachoMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click


        ReversarCombos()

        MsgBox("Proceso terminado...", MsgBoxStyle.Information, sModulo)

        ProgressBar1.Value = 0
        lblProgreso.Text = ""
        IniciarTXT()

        'InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, NumeroDeGuia)
        Me.Close()

    End Sub

    Private Sub AbrirCombosConExistencias()

        CargaListViewCombosConExistencia(lvPedidos, dt)

    End Sub

    Private Sub ReversarCombos()

        Dim NumeroMovimiento As String
        Dim iCont As Integer = 0

        NumeroMovimiento = Contador(MyConn, lblInfo, Gestion.iMercancías, "INVNUMMOV", "2")

        For iCont = 0 To lvPedidos.Items.Count - 1
            If lvPedidos.Items(iCont).Checked Then
                ' Producir las entradas en almacen de los componentes
                Dim ExistenciaCombo As Double = CDbl(lvPedidos.Items(iCont).SubItems(2).Text)
                Dim dtCombo As DataTable
                Dim nTablaCombo As String = "tblComboCompronentes"
                ds = DataSetRequery(ds, " select * from jsmercatcom where codart = '" & lvPedidos.Items(iCont).Text & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaCombo, lblInfo)
                dtCombo = ds.Tables(nTablaCombo)

                For Each nRow As DataRow In dtCombo.Rows
                    With nRow

                        InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, .Item("codartcom"), jytsistema.sFechadeTrabajo, "EN", _
                                                             NumeroMovimiento, .Item("UNIDAD"), ExistenciaCombo * .Item("CANTIDAD"), _
                                                             ExistenciaCombo * .Item("peso"), ExistenciaCombo * .Item("costo"), ExistenciaCombo * .Item("costo"), _
                                                             "INV", NumeroMovimiento, "", "COMBO_REV", 0.0#, 0.0#, 0.0#, 0.0#, "", txtAlmacen.Text, "", jytsistema.sFechadeTrabajo)

                        ActualizarExistenciasPlus(MyConn, .Item("codartcom"))

                    End With

                Next

                dtCombo.Dispose()
                dtCombo = Nothing
                'produce las salidas en almacen de la mercancia-combo
                Dim PesoMercancia As Double = ExistenciaCombo * ft.DevuelveScalarDoble(MyConn, " select PESOUNIDAD from jsmerctainv where codart = '" & lvPedidos.Items(iCont).Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                Dim CostoMercancia As Double = ExistenciaCombo * ft.DevuelveScalarDoble(MyConn, " select MONTOULTIMACOMPRA from jsmerctainv where codart = '" & lvPedidos.Items(iCont).Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, lvPedidos.Items(iCont).Text, jytsistema.sFechadeTrabajo, _
                 "SA", NumeroMovimiento, lvPedidos.Items(iCont).SubItems(3).Text, CDbl(ExistenciaCombo), PesoMercancia, CostoMercancia, CostoMercancia, "INV", _
                 NumeroMovimiento, "", "COMBO_REV", 0.0#, 0.0#, 0.0#, 0.0#, "", txtAlmacen.Text, "", jytsistema.sFechadeTrabajo)

                ActualizarExistenciasPlus(MyConn, lvPedidos.Items(iCont).Text)

            End If

        Next


    End Sub



    Private Sub txtAlmacen_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtAlmacen.TextChanged

        If txtAlmacen.Text.Trim <> "" Then
            strSQL = " SELECT a.codart, a.nomart ,  COUNT(b.codartcom) cuenta, c.existencia, a.unidad " _
                & " FROM jsmerctainv a " _
                & " LEFT JOIN jsmercatcom b ON (a.codart =  b.codart AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN jsmerextalm c ON (a.codart = c.codart AND a.id_emp = c.id_emp ) " _
                & " WHERE " _
                & " c.almacen = '" & txtAlmacen.Text & "' AND " _
                & " c.existencia >  0 and " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " GROUP BY a.codart " _
                & " HAVING(cuenta) > 0 "
        End If

        ds = DataSetRequery(ds, strSQL, MyConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        AbrirCombosConExistencias()

    End Sub
    Private Sub btnAlmacen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacen.Click
        Dim f As New jsControlArcAlmacenes
        f.Cargar(MyConn, TipoCargaFormulario.iShowDialog)
        txtAlmacen.Text = f.Seleccionado
    End Sub

End Class