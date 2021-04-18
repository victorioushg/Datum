Imports MySql.Data.MySqlClient
Public Class jsVenProCierreMensual
    Private Const sModulo As String = "Cierre mensual de ventas"

    Private Const nTabla As String = "tblAsesores"

    Private strSQLAsesores As String = " select * from jsvencatven where tipo = " & TipoVendedor.iFuerzaventa & " and id_emp = '" & jytsistema.WorkID & "' order by codven "

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        myConn = MyCon
        Me.Tag = sModulo

        lblLeyenda.Text = " Mediante este proceso se procede a realizar el cierre mensual de las ventas " + vbCr + _
                " de la empresa. " + vbCr + _
                " - Se calculan los porcentajes de participación en las ventas del mes anterior y se coloca como factor " + vbCr + _
                "   de asignación de cuota fija en cada asesor comercial " + vbCr + _
                " - Se calculan las cuotas de este mes en base a la venta ocurrida en el mes anterior y el porcentaje de " + vbCr + _
                "   crecimiento deseado. la CUOTAS asignadas según las COMPRAS de mercancía se colocara el resto no vendido " + vbCr + _
                "   como cuota "

        txtFechaProceso.Text = FormatoFecha(DateAdd(DateInterval.Day, -1, PrimerDiaMes(jytsistema.sFechadeTrabajo)))
        txtPorcentaje.Text = FormatoNumero(0.0)

        HabilitarObjetos(False, True, txtFechaProceso, txtPorcentaje, Label2)
        MensajeEtiqueta(lblInfo, " ... ", TipoMensaje.iAyuda)

        Me.Show()

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub jsControlProVerificarBD_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        InsertarAuditoria(myConn, MovAud.iSalir, sModulo, "")
    End Sub

    Private Sub jsControlProVerificarBD_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(myConn, MovAud.ientrar, sModulo, "")

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Procesar()
    End Sub
    Private Sub Procesar()

        DeshabilitarCursorEnEspera()


        ProcesarFactorDeVentas()
        If chk7.Checked Then ProcesarCuotas()

        HabilitarCursorEnEspera()
        MensajeInformativo(lblInfo, " Proceso culminado con éxito... ")
        ProgressBar1.Value = 0
        lblProgreso.Text = ""

    End Sub

    Private Sub ProcesarCuotas()

        Dim mesTrabajo As String = Format(jytsistema.sFechadeTrabajo.Month, "00")
        Dim dtVenta As DataTable
        Dim nTablaVentas As String = "tblVentas"
        Dim strSQLVenta As String = ""

        ds = DataSetRequery(ds, strSQLAsesores, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        If dt.Rows.Count > 0 Then
            For Each nRowAsesor As DataRow In dt.Rows
                With nRowAsesor
                    Dim nAsesor As String = .Item("codven")
                    Dim strAsesores As String = IIf(.Item("CLASE") = 0, "('" & nAsesor & "')", AsesoresDeSupervisor(myConn, lblInfo, nAsesor))


                    strSQLVenta = " SELECT a.codart,  b.nomart, ROUND(SUM( IF( a.origen IN ('NCV') , -1, 1)* IF( c.uvalencia IS NULL, a.cantidad, a.cantidad/c.equivale )),3) cantidad, " _
                        & " b.unidad, SUM( IF( a.origen IN ('NCV') , -1, 1)*a.peso ) peso  " _
                        & " FROM jsmertramer a " _
                        & " LEFT JOIN jsmerctainv b ON (a.codart = b.codart AND a.id_emp = b.id_emp) " _
                        & " LEFT JOIN jsmerequmer c ON (a.codart = c.codart AND a.unidad = c.uvalencia AND a.id_emp = c.id_emp ) " _
                        & " WHERE " _
                        & " MONTH(a.fechamov ) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                        & " YEAR(a.fechamov) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                        & " a.origen IN ('FAC', 'PFC', 'PVE', 'NCV', 'NDV') AND " _
                        & " a.vendedor IN " & strAsesores & " AND " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' " _
                        & " GROUP BY a.codart "

                    EjecutarSTRSQL(myConn, lblInfo, " update jsvencuoart set esmes" & mesTrabajo & " =  " & 0.0 & ",  " _
                                                       & " remes" & mesTrabajo & " = " & 0.0 & " " _
                                                       & " where " _
                                                       & " codven = '" & nAsesor & "' and " _
                                                       & " id_emp = '" & jytsistema.WorkID & "' ")

                    ds = DataSetRequery(ds, strSQLVenta, myConn, nTablaVentas, lblInfo)
                    dtVenta = ds.Tables(nTablaVentas)
                    Dim iCont As Integer = 1
                    If dtVenta.Rows.Count > 0 Then
                        For Each nRowVentas As DataRow In dtVenta.Rows
                            With nRowVentas

                                lblProgreso.Text = " Asesor : " & nAsesor & " - Mercancía :  " & .Item("codart") & " " & .Item("nomart")
                                ProgressBar1.Value = (iCont) / dtVenta.Rows.Count * 100
                                Application.DoEvents()

                                iCont += 1

                                Dim esCuotaFija As Boolean = CBool(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select cuotafija from jsmerctainv where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "'"))

                                Dim cantidadPeso As Double = .Item("peso") * (1 + ValorNumero(txtPorcentaje.Text) / 100)
                                Dim cantidadUnidad As Double = .Item("cantidad") * (1 + ValorNumero(txtPorcentaje.Text) / 100)



                                If esCuotaFija Then

                                Else
                                    If EjecutarSTRSQL_Scalar(myConn, lblInfo, " select codven from jsvencuoart " _
                                                             & " where " _
                                                             & " codven = '" & nAsesor & "' and " _
                                                             & " codart = '" & .Item("codart") & "' and " _
                                                             & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then

                                        EjecutarSTRSQL(myConn, lblInfo, " insert into jsvencuoart values ('" & nAsesor & "', '" & .Item("codart") & "', " _
                                                       & " 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0, '" & jytsistema.WorkExercise & "','" & jytsistema.WorkID & "') " _
                                                       & "  ")

                                    End If
                                    EjecutarSTRSQL(myConn, lblInfo, " update jsvencuoart set esmes" & mesTrabajo & " =  " & cantidadPeso & ",  " _
                                                       & " remes" & mesTrabajo & " = " & cantidadUnidad & " " _
                                                       & " where " _
                                                       & " codven = '" & nAsesor & "' and " _
                                                       & " codart = '" & .Item("codart") & "' and  " _
                                                       & " id_emp = '" & jytsistema.WorkID & "' ")

                                End If

                            End With
                        Next
                    End If

                End With

            Next
        End If



    End Sub
    Private Sub ProcesarFactorDeVentas()

        Dim dtPorcentajes As DataTable
        Dim nTablaPorcentajes As String = "tblPorcentajes"

        EjecutarSTRSQL(myConn, lblInfo, " update jsvencatven set " _
                                   & " factorcuota = " & 0.0 & " " _
                                   & " where " _
                                   & " id_emp = '" & jytsistema.WorkID & "' ")

        Dim strSQLPorcentajes As String = " SELECT a.vendedor, round(SUM( IF( a.origen = 'NCV', -1, 1) * a.peso  )/b.pesototal*100,2) porcentaje,  a.id_emp " _
                                                                        & " FROM jsmertramer a " _
                                                                        & " LEFT JOIN (SELECT a.id_emp, SUM( IF( a.origen = 'NCV', -1, 1) * a.peso  ) pesototal " _
                                                                        & "     		FROM jsmertramer a " _
                                                                        & "             WHERE " _
                                                                        & "             MONTH(a.fechamov) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                                                                        & "             YEAR(a.fechamov) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                                        & " 		    a.origen IN ('FAC', 'PVE', 'PFC', 'NDV', 'NCV') AND " _
                                                                        & " 		    a.id_emp = '" & jytsistema.WorkID & "' " _
                                                                        & " 		    GROUP BY a.id_emp) b ON (a.id_emp = b.id_emp) " _
                                                                        & " WHERE " _
                                                                        & " MONTH(a.fechamov) = " & CDate(txtFechaProceso.Text).Month & " AND " _
                                                                        & " YEAR(a.fechamov) = " & CDate(txtFechaProceso.Text).Year & " AND " _
                                                                        & " a.origen IN ('FAC', 'PVE', 'PFC', 'NDV', 'NCV') AND " _
                                                                        & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                                                        & " GROUP BY a.vendedor "


        ds = DataSetRequery(ds, strSQLPorcentajes, myConn, nTablaPorcentajes, lblInfo)
        dtPorcentajes = ds.Tables(nTablaPorcentajes)

        If dtPorcentajes.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtPorcentajes.Rows.Count - 1
                With dtPorcentajes.Rows(iCont)
                    lblProgreso.Text = " Asesor : " & .Item("vendedor")
                    ProgressBar1.Value = (iCont + 1) / dtPorcentajes.Rows.Count * 100
                    Application.DoEvents()
                    EjecutarSTRSQL(myConn, lblInfo, " update jsvencatven set " _
                                   & " factorcuota = " & .Item("porcentaje") & " " _
                                   & " where " _
                                   & " codven = '" & .Item("vendedor") & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' ")
                End With
            Next
        End If

        ds = DataSetRequery(ds, " select a.supervisor vendedor, sum(a.factorcuota) porcentaje, a.id_emp " _
                            & " from jsvencatven a " _
                            & " where " _
                            & " a.clase = 0 and " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " group by a.supervisor ", myConn, nTablaPorcentajes, lblInfo)

        dtPorcentajes = ds.Tables(nTablaPorcentajes)

        If dtPorcentajes.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtPorcentajes.Rows.Count - 1
                With dtPorcentajes.Rows(iCont)
                    lblProgreso.Text = " Asesor : " & .Item("vendedor")
                    ProgressBar1.Value = (iCont + 1) / dtPorcentajes.Rows.Count * 100
                    Application.DoEvents()
                    EjecutarSTRSQL(myConn, lblInfo, " update jsvencatven set " _
                                   & " factorcuota = " & .Item("porcentaje") & " " _
                                   & " where " _
                                   & " codven = '" & .Item("vendedor") & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' ")
                End With
            Next
        End If


        dtPorcentajes.Dispose()
        dtPorcentajes = Nothing

    End Sub

    
    'Private Sub btnFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFecha.Click
    '    txtFechaProceso.Text = SeleccionaFecha(CDate(txtFechaProceso.Text), btnFecha)
    'End Sub

    Private Sub chk7_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chk7.CheckedChanged
        If chk7.Checked Then
            HabilitarObjetos(True, True, txtPorcentaje)
        Else
            HabilitarObjetos(False, True, txtPorcentaje)
            FormatoNumero(0.0)
        End If
    End Sub

    Private Sub txtImporte_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPorcentaje.KeyPress
        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub

End Class