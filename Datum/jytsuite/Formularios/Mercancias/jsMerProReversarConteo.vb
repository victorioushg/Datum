Imports MySql.Data.MySqlClient
Public Class jsMerProREVERSARConteo
    Private Const sModulo As String = "Reversar conteo de inventario"
    Private Const nTabla As String = "tblProConteo"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private ConteoNumero As String

    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        myConn = MyCon
        lblLeyenda.Text = " 1. Elimina el movimiento de inventario producido por el conteo. " + vbCrLf + _
                          " 2. Coloca el conteo como No procesado y la asigna la fecha de reverso al conteo seleccionado. " + vbCrLf + _
                          " 3. Recalcula las existencias de inventario "

        ft.mensajeEtiqueta(lblInfo, "Seleccione el conteo de inventario que desea procesar...", Transportables.TipoMensaje.iAyuda)
        ft.habilitarObjetos(False, False, txtCategoriaDesde, txtJerarquiaDesde)
        ft.visualizarObjetos(False, Label2, txtJerarquiaDesde, btnJerarquiaDesde, Label1, chk)
        Me.Show()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub jsMerProProcesarConteo_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsMerProProcesarConteo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Reprocesar()
        ft.mensajeInformativo(" Proceso culminado ...")
        ProgressBar1.Value = 0
        lblProgreso.Text = ""


    End Sub

    Private Sub Reprocesar()

        Dim hCont As Integer
        Dim strFilter As String = ""

        If txtJerarquiaDesde.Text <> "" Then strFilter += " a.tipjer = '" & txtJerarquiaDesde.Text & "' and "

        ds = DataSetRequery(ds, " SELECT a.conmer, a.codart, a.nomart, a.unidad, a.conteo, a.existencia, a.costou, a.costo_tot, " _
                            & " a.exist1, a.cont1, a.exist2, a.cont2, a.exist3, a.cont3, a.exist4, a.cont4, a.exist5, a.cont5 " _
                            & " FROM jsmerconmer a " _
                            & " WHERE " _
                            & " a.conmer = '" & txtCategoriaDesde.Text & "' AND  " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' ORDER BY codart ", myConn, nTabla, lblInfo)

        dt = ds.Tables(nTabla)
        If dt.Rows.Count > 0 Then
            For hCont = 0 To dt.Rows.Count - 1
                With dt.Rows(hCont)

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (hCont + 1) / dt.Rows.Count * 100, _
                                                  " Ajustando mercancía : " & .Item("codart") & " " & .Item("nomart"))

                    ft.Ejecutar_strSQL(myconn, " delete from jsmertramer where codart = '" & .Item("codart") & _
                                   "' and numdoc = '" & txtCategoriaDesde.Text & "' and origen = 'INV' and numorg = '" & _
                                   txtCategoriaDesde.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                    ActualizarExistenciasPlus(myConn, .Item("codart"))

                End With
            Next
        Else
            ft.MensajeCritico("No existen movimientos para procesar...")
        End If

        ft.Ejecutar_strSQL(myconn, " update jsmerenccon set procesado = 0, fechapro = '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & _
                       "' where conmer ='" & txtCategoriaDesde.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub


    Private Sub btnCategoriaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoriaDesde.Click
        txtCategoriaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT CONMER codigo, comentario descripcion, fechacon, almacen " _
                                                   & " from jsmerenccon where procesado = 1 and id_emp = '" & jytsistema.WorkID & "' order by conmer", _
                                                   "Conteos de Inventario PROCESADOS...", txtCategoriaDesde.Text)
    End Sub


    Private Sub btnJerarquiaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnJerarquiaDesde.Click
        Dim strSQLTipjer As String = " SELECT tipjer codigo, descrip descripcion FROM jsmerencjer WHERE  id_emp  = '" & jytsistema.WorkID & "' order by 1 "
        txtJerarquiaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, strSQLTipjer, "Jerarquías de Mercancías", txtJerarquiaDesde.Text)
    End Sub


End Class