Imports MySql.Data.MySqlClient
Public Class jsMerProPROCESARConteo
    Private Const sModulo As String = "Procesar conteo de inventario"
    Private Const nTabla As String = "tblProConteo"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ConteoNumero As String
    
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        myConn = MyCon
        lblLeyenda.Text = " 1. Transfiere las diferencias entre el conteo seleccionado y el inventario para producir los ajustes " + vbCrLf + _
                          "    al mismo. Si el conteo es cero ajusta a cero las cantidades de INV a cero por los tanto debe colocar " + vbCrLf + _
                          "    todas las existencias en almacén para todas las mercancías " + vbCrLf + _
                          " 2. Coloca el conteo como procesado y la asigna la fecha de proceso al conteo seleccionado " + vbCrLf + _
                          " 3. Recalcula las existencias de inventario "

        MensajeEtiqueta(lblInfo, "Seleccione el conteo de inventario que desea procesar...", TipoMensaje.iAyuda)
        HabilitarObjetos(False, False, txtCategoriaDesde, txtJerarquiaDesde)
        VisualizarObjetos(False, Label2, txtJerarquiaDesde, btnJerarquiaDesde)
        Me.Show()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub jsMerProProcesarConteo_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        InsertarAuditoria(myConn, MovAud.iSalir, sModulo, "")
    End Sub

    Private Sub jsMerProProcesarConteo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(myConn, MovAud.ientrar, sModulo, "")
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Reprocesar()
        MensajeInformativoPlus(" Proceso culminado ...")
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

                    lblProgreso.Text = " Ajustando mercancía : " & .Item("codart") & " " & .Item("nomart")
                    ProgressBar1.Value = (hCont + 1) / dt.Rows.Count * 100
                    Application.DoEvents()

                    Dim FechaAsiento As Date = CDate(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select fechacon from jsmerenccon where conmer = '" & txtCategoriaDesde.Text & "' and id_emp = '" & jytsistema.WorkID & "' ").ToString)
                    Dim Almacen As String = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select almacen from jsmerenccon where conmer = '" & txtCategoriaDesde.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If Almacen = "" Then Almacen = "00001"
                    Dim PesoArticulo As Double = EjecutarSTRSQL_Scalar(myConn, lblInfo, "select pesounidad from jsmerctainv where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                    Dim CantidadAjuste As Double = ExistenciaEnAlmacen(myConn, .Item("codart"), Almacen, lblInfo) - .Item("conteo")

                    If chk.Checked Then 'Ajusta todas las mercancías
                        If CantidadAjuste > 0 Then

                            InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("codart"), FechaAsiento, _
                                "AS", txtCategoriaDesde.Text, .Item("unidad"), Math.Abs(CantidadAjuste), Math.Abs(CantidadAjuste) * PesoArticulo, _
                                Math.Abs(CantidadAjuste) * .Item("costou"), Math.Abs(CantidadAjuste) * .Item("costou"), "INV", txtCategoriaDesde.Text, "", "", 0.0, 0.0, 0.0, 0.0, "", Almacen, _
                                Format(hCont, "00000"), jytsistema.sFechadeTrabajo)
                            ActualizarExistencias(myConn, .Item("codart"), lblInfo)

                        ElseIf CantidadAjuste < 0 Then

                            InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("codart"), FechaAsiento, _
                                "AE", txtCategoriaDesde.Text, .Item("unidad"), Math.Abs(CantidadAjuste), Math.Abs(CantidadAjuste) * PesoArticulo, Math.Abs(CantidadAjuste) * .Item("costou"), _
                                Math.Abs(CantidadAjuste) * .Item("costou"), "INV", txtCategoriaDesde.Text, "", "", 0.0, 0.0, 0.0, 0.0, "", Almacen, _
                                Format(hCont, "00000"), jytsistema.sFechadeTrabajo)
                            ActualizarExistencias(myConn, .Item("codart"), lblInfo)

                        End If
                    Else 'Ajusta mercancías cuyo conteo sea mayor a cero (> 0) 
                        If .Item("conteo") > 0 Then
                            If CantidadAjuste > 0 Then

                                InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("codart"), FechaAsiento, _
                                    "AS", txtCategoriaDesde.Text, .Item("unidad"), Math.Abs(CantidadAjuste), Math.Abs(CantidadAjuste) * PesoArticulo, Math.Abs(CantidadAjuste) * .Item("costou"), _
                                    Math.Abs(CantidadAjuste) * .Item("costou"), "INV", txtCategoriaDesde.Text, "", "", 0.0, 0.0, 0.0, 0.0, "", Almacen, _
                                    Format(hCont, "00000"), jytsistema.sFechadeTrabajo)
                                ActualizarExistencias(myConn, .Item("codart"), lblInfo)

                            ElseIf CantidadAjuste < 0 Then

                                InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("codart"), FechaAsiento, _
                                    "AE", txtCategoriaDesde.Text, .Item("unidad"), Math.Abs(CantidadAjuste), Math.Abs(CantidadAjuste) * PesoArticulo, Math.Abs(CantidadAjuste) * .Item("costou"), _
                                    Math.Abs(CantidadAjuste) * .Item("costou"), "INV", txtCategoriaDesde.Text, "", "", 0.0, 0.0, 0.0, 0.0, "", Almacen, _
                                    Format(hCont, "00000"), jytsistema.sFechadeTrabajo)
                                ActualizarExistencias(myConn, .Item("codart"), lblInfo)

                            End If

                        End If
                    End If

                End With
            Next
        Else
            MensajeCritico(lblInfo, "No existen movimientos para procesar...")
        End If

        EjecutarSTRSQL(myConn, lblInfo, " update jsmerenccon set procesado = 1, fechapro = '" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & _
                       "' where conmer ='" & txtCategoriaDesde.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")


    End Sub


    Private Sub btnCategoriaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCategoriaDesde.Click
        txtCategoriaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, " SELECT CONMER codigo, comentario descripcion, fechacon, almacen " _
                                                   & " from jsmerenccon where procesado = 0 and id_emp = '" & jytsistema.WorkID & "' order by conmer", _
                                                   "Conteos de Inventario NO PROCESADOS...", txtCategoriaDesde.Text)
    End Sub

   
    Private Sub btnJerarquiaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnJerarquiaDesde.Click
        Dim strSQLTipjer As String = " SELECT tipjer codigo, descrip descripcion FROM jsmerencjer WHERE  id_emp  = '" & jytsistema.WorkID & "' order by 1 "
        txtJerarquiaDesde.Text = CargarTablaSimple(myConn, lblInfo, ds, strSQLTipjer, "Jerarquías de Mercancías", txtJerarquiaDesde.Text)
    End Sub

  
End Class