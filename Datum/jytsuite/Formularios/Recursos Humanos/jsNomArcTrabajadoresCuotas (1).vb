Imports MySql.Data.MySqlClient
Public Class jsNomArcTrabajadoresCuotas

    Private Const sModulo As String = "Cuotas/Prestamos de Trabajadores"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable

    Private strSQLLocalR As String = ""
    Private strSQLCuotas As String = ""
    Private nTablaR As String = "tblLocalRenglon"
    Private nTablaCuotas As String = "tblCuotas"
    Private dtLocalR As DataTable
    Private dtCuotas As DataTable

    Private i_modo As Integer
    Private nPosicion As Integer
    Private nPosicionR As Integer
    Private n_Apuntador As Long
    Private aTipoInteres() As String = {"Ninguno", "Simple", "Compuesto"}
    Private aEstatus() As String = {"Inactivo", "Activo"}
    Private CodigoTrabajador As String
    Private CodigoCuotaAnterior As String
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                       ByVal CodTrabajador As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoTrabajador = CodTrabajador
        IniciarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()

        CodigoCuotaAnterior = "T" & CStr(NumeroAleatorio(1000))
        txtCodigo.Text = ""
        txtNombre.Text = ""
        txtMonto.Text = FormatoNumero(0.0)
        txtAprobacion.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        txtInicio.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        RellenaCombo(aTipoInteres, cmbTipoInteres)
        txtPorcentajeInteres.Text = FormatoNumero(0.0)
        txtCantidadCuotas.Text = FormatoEntero(1)
        txtSaldo.Text = "0.00"
        RellenaCombo(aEstatus, cmbEstatus, 1)

        IniciarMovimiento(CodigoCuotaAnterior)

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                      ByVal CodTrabajador As String)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoTrabajador = CodTrabajador
        AsignarTXT(Apuntador)
        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)

            CodigoCuotaAnterior = .Item("codpre")
            txtCodigo.Text = .Item("codpre")
            txtNombre.Text = .Item("descrip")
            txtMonto.Text = FormatoNumero(.Item("montotal"))
            txtAprobacion.Text = FormatoFecha(CDate(.Item("fechaprestamo").ToString))
            txtInicio.Text = FormatoFecha(CDate(.Item("fechainicio").ToString))
            RellenaCombo(aTipoInteres, cmbTipoInteres, .Item("tipointeres"))
            txtPorcentajeInteres.Text = FormatoNumero(.Item("por_interes"))
            txtCantidadCuotas.Text = FormatoEntero(.Item("numcuotas"))
            txtSaldo.Text = FormatoNumero(.Item("saldo"))
            RellenaCombo(aEstatus, cmbEstatus, .Item("estatus"))

            IniciarMovimiento(CodigoCuotaAnterior)


        End With
    End Sub
    Private Sub IniciarMovimiento(ByVal Codigo As String)

        strSQLLocalR = " select * from jsnomrenpre " _
                           & " where codtra = '" & CodigoTrabajador & "' and " _
                           & " codpre = '" & Codigo & "' and " _
                           & " id_emp = '" & jytsistema.WorkID & "' order by num_cuota "

        dsLocal = DataSetRequery(dsLocal, strSQLLocalR, MyConn, nTablaR, lblInfo)
        dtLocalR = dsLocal.Tables(nTablaR)

        Dim aCam() As String = {"num_cuota", "monto", "capital", "interes", "procesada", "fechainicio", "fechafin", ""}
        Dim aNom() As String = {"Cuota Nº", "Monto", "Capital", "Interes", "Procesada", "Fecha Proceso Inicial", "Fecha Proceso Final", ""}
        Dim aAnc() As Long = {60, 90, 90, 90, 80, 90, 90, 10}
        Dim aAli() As Integer = {AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                 AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}

        Dim aFor() As String = {sFormatoEntero, sFormatoNumero, sFormatoNumero, sFormatoNumero, "", sFormatoFecha, sFormatoFecha, ""}

        IniciarTabla(dg, dtLocalR, aCam, aNom, aAnc, aAli, aFor)
        If dtLocalR.Rows.Count > 0 Then nPosicionR = 0

        CalculaSaldoPrestamo()

    End Sub

    Private Sub jsNomArcTrabajadoresCuotas_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsNomArcTrabajadoresCuotas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
        HabilitarObjetos(False, True, txtCodigo, txtNombre, txtAprobacion, txtInicio, txtSaldo)
        If i_modo = movimiento.iEditar Then
            HabilitarObjetos(False, True, btnPrestamo, txtMonto, btnInicio, btnAprobacion, cmbTipoInteres, txtPorcentajeInteres, _
                             txtCantidadCuotas)
        End If
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtNombre.GotFocus, txtMonto.GotFocus, txtPorcentajeInteres.GotFocus, txtCantidadCuotas.GotFocus, _
        btnAprobacion.GotFocus, btnAprobacion.GotFocus, btnInicio.GotFocus, cmbTipoInteres.GotFocus
        Select Case sender.name
            Case "txtCodigo"
                MensajeEtiqueta(lblInfo, "Indique el código de cuota/préstamo ...", TipoMensaje.iInfo)
            Case "txtNombre"
                MensajeEtiqueta(lblInfo, "Indique nombre o descripción de cuota/préstamo ...", TipoMensaje.iInfo) : txtNombre.MaxLength = 50
            Case "txtMonto"
                MensajeEtiqueta(lblInfo, "Indique Monto de Cuota ó préstamo ...", TipoMensaje.iInfo) : txtMonto.MaxLength = 15
            Case "txtPorcentajeInteres"
                MensajeEtiqueta(lblInfo, "Indique porcentaje de interés ...", TipoMensaje.iInfo) : txtNombre.MaxLength = 6
            Case "txtCantidadCuotas"
                MensajeEtiqueta(lblInfo, "Indique el número o cantidad de cuotas que serán descontadas ...", TipoMensaje.iInfo) : txtNombre.MaxLength = 5
            Case "btnFechaAprobacion", "btnInicio"
                MensajeEtiqueta(lblInfo, "Seleccionar fecha " & Replace(sender.name, "btn", "") & " ...", TipoMensaje.iInfo)
            Case "cmbTipoInteres"
                MensajeEtiqueta(lblInfo, "Seleccione el tipo de interés ...", TipoMensaje.iInfo)
        End Select

    End Sub
    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtNombre.Text) = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar un nombre válido...")
            txtNombre.Focus()
            Exit Function
        End If

        If dtLocalR.Rows.Count <= 0 Then
            MensajeAdvertencia(lblInfo, "Debe indicar un reglón al menos para esta cuota/préstamo...")
            Exit Function
        End If

        Validado = True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dtLocal.Rows.Count
            End If
            EjecutarSTRSQL(MyConn, lblInfo, " update jsnomrenpre set codpre = '" & txtCodigo.Text & "' where codtra = '" & CodigoTrabajador & "' and codpre = '" & CodigoCuotaAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")

            CalculaSaldoPrestamo()

            InsertEditNOMINAEncabezadoCuota(MyConn, lblInfo, Insertar, CodigoTrabajador, txtCodigo.Text, txtNombre.Text, _
                                            ValorNumero(txtMonto.Text), CDate(txtAprobacion.Text), CDate(txtInicio.Text), _
                                            cmbTipoInteres.SelectedIndex, ValorNumero(txtPorcentajeInteres.Text), _
                                            ValorEntero(txtCantidadCuotas.Text), ValorNumero(txtSaldo.Text), cmbEstatus.SelectedIndex)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, CodigoTrabajador + "/" + txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then _
            EjecutarSTRSQL(MyConn, lblInfo, " delete from jsnomrenpre where codtra = '" & CodigoTrabajador & "' and codpre = '" & CodigoCuotaAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")

        Me.Close()
    End Sub
    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dg.CellFormatting

        Select Case dg.Columns(e.ColumnIndex).Name
            Case "procesada"
                Dim aProceso() As String = {"No", "Si"}
                e.Value = aProceso(IIf(e.Value, 1, 0))
        End Select
    End Sub


    Private Sub btnAgregaRenglon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaRenglon.Click
        If ValorNumero(txtMonto.Text) > 0 AndAlso ValorEntero(txtCantidadCuotas.Text) > 0 And i_modo = movimiento.iAgregar Then
            AgregarCuotas()
            IniciarMovimiento(CodigoCuotaAnterior)
        End If
    End Sub
    Private Sub AgregarCuotas()
        If txtCodigo.Text <> "" Then
            If ValorEntero(txtCantidadCuotas.Text) = 9999 Then

                Dim numCuota As Integer = CInt(AutoCodigo(5, dsLocal, nTablaR, "num_cuota"))
                InsertEditNOMINARenglonCuota(MyConn, lblInfo, True, CodigoTrabajador, CodigoCuotaAnterior, numCuota, ValorNumero(txtMonto.Text), _
                            0.0, 0.0, 0, CDate(txtInicio.Text), CDate(txtInicio.Text))

            Else
                Select Case cmbTipoInteres.SelectedIndex
                    Case 0 'Ninguno

                        Dim gCont As Integer
                        Dim aCamT() As String = {"codtra", "id_emp"}
                        Dim aStrT() As String = {CodigoTrabajador, jytsistema.WorkID}
                        Dim nTipoNomina As Integer = CInt(qFoundAndSign(MyConn, lblInfo, "jsnomcattra", aCamT, aStrT, "tiponom"))
                        Dim aSuma() As Integer = {1, 7, 15, 30, 365, 1}
                        Dim nMonto As Double = 0.0
                        For gCont = 1 To ValorEntero(txtCantidadCuotas.Text)
                            Dim MontoReal As Double = IIf(gCont = ValorEntero(txtCantidadCuotas.Text), _
                                                           ValorNumero(txtMonto.Text) - nMonto, _
                                                          Math.Round(ValorNumero(txtMonto.Text) / ValorEntero(txtCantidadCuotas.Text), 2))

                            nMonto += Math.Round(ValorNumero(txtMonto.Text) / ValorEntero(txtCantidadCuotas.Text), 2)

                            InsertEditNOMINARenglonCuota(MyConn, lblInfo, True, CodigoTrabajador, CodigoCuotaAnterior, gCont, MontoReal, MontoReal, 0.0, 0, _
                                            DateAdd(DateInterval.Day, aSuma(nTipoNomina) * gCont, CDate(txtInicio.Text)), _
                                            DateAdd(DateInterval.Day, aSuma(nTipoNomina) * gCont, CDate(txtInicio.Text)))
                        Next

                    Case 1 'Simple
                    Case 2 'Compuesto
                End Select
            End If
        End If

        dsLocal = DataSetRequery(dsLocal, strSQLLocalR, MyConn, nTablaR, lblInfo)
        dtLocalR = dsLocal.Tables(nTablaR)
        AsignaRenglon(dtLocalR.Rows.Count - 1, False)

        CalculaSaldoPrestamo()

    End Sub
    Private Sub CalculaSaldoPrestamo()
        txtSaldo.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select sum(monto) from jsnomrenpre where codtra = '" & CodigoTrabajador & "' and codpre = '" & txtCodigo.Text & "' and procesada = 0 and id_emp = '" & jytsistema.WorkID & "' group by codtra, codpre  ")))
    End Sub
    Private Sub btnEditarRenglon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarRenglon.Click
       
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
         dg.RowHeaderMouseClick, dg.CellMouseClick
        Me.BindingContext(dsLocal, nTablaR).Position = e.RowIndex
        nPosicionR = e.RowIndex
        AsignaRenglon(e.RowIndex, False)
    End Sub

    Private Sub dg_RegionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.RegionChanged
        nPosicionR = Me.BindingContext(dsLocal, nTablaR).Position
    End Sub
    Private Sub AsignaRenglon(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then dsLocal = DataSetRequery(dsLocal, strSQLLocalR, MyConn, nTablaR, lblInfo)
        dtLocalR = dsLocal.Tables(nTablaR)
        If c >= 0 AndAlso dtLocalR.Rows.Count > 0 Then
            Me.BindingContext(dsLocal, nTablaR).Position = c
            dg.CurrentCell = dg(0, c)
        End If
    End Sub

    Private Sub btnPrestamo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrestamo.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Cuotas/Préstamos", FormatoTablaSimple(Modulo.iPrestamoCuotaNomina), True, TipoCargaFormulario.iShowDialog)
        txtCodigo.Text = f.Seleccion
        f.Dispose()
        f = Nothing
    End Sub

    Private Sub txtCodigo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigo.TextChanged
        Dim aCam() As String = {"codigo", "modulo", "id_emp"}
        Dim aStr() As String = {txtCodigo.Text, FormatoTablaSimple(Modulo.iPrestamoCuotaNomina), jytsistema.WorkID}
        txtNombre.Text = qFoundAndSign(MyConn, lblInfo, "jsconctatab", aCam, aStr, "descrip")
        strSQLLocalR = " select * from jsnomrenpre " _
                           & " where codtra = '" & CodigoTrabajador & "' and " _
                           & " codpre = '" & txtCodigo.Text & "' and " _
                           & " id_emp = '" & jytsistema.WorkID & "' order by num_cuota "

    End Sub

    Private Sub btnEliminaRenglon_Click(sender As System.Object, e As System.EventArgs) Handles btnEliminaRenglon.Click

    End Sub

    Private Sub dg_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub
End Class