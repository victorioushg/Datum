Imports MySql.Data.MySqlClient
Public Class jsBanArcBancosMovimientosPlus
    Private Const sModulo As String = "Movimiento bancario"

    Private MyConn As New MySqlConnection
    Private ds As DataSet
    Private dt As DataTable

    Private dtMovimientos As DataTable
    Private strSQLMov As String = ""
    Private nTablamovimientos As String = "tblMovimientos"
    Private nPosicionRenglon As Integer

    Private i_modo As Integer
    Private Codigobanco As String
    Private aTipo() As String = {"Deposito", "Nota Crédito", "Cheque", "Nota Débito"}
    Private aTipoDeposito() As String = {"Normal", "Diferido"}
    Private aaTipo() As String = {"DP", "NC", "CH", "ND"}
    Private n_Apuntador As Long
    Private numComprobante As String

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal Codban As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        ds = dsMov
        dt = dtMov
        Codigobanco = Codban
        If dt.Rows.Count = 0 Then Apuntador = -1
        IniciarTXT()
        HabilitarObjetos(False, True, txtFecha, cmbTipoDeposito)

        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()


        txtFecha.Text = FormatoFechaMedia(jytsistema.sFechadeTrabajo)
        txtDocumento.Text = ""
        txtConcepto.Text = ""
        txtImporte.Text = FormatoNumero(0.0)

        txtCodigoBeneficiario.Text = ""
        txtBeneficiario.Text = ""

        RellenaCombo(aTipo, cmbTipo)
        RellenaCombo(aTipoDeposito, cmbTipoDeposito)

        numComprobante = Contador(MyConn, lblInfo, Gestion.iBancos, "BANNUMRCC", "03")
        AbrirAsiento(numComprobante)

        ActualizarPrimerRegistroAsiento()

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal dsMov As DataSet, ByVal dtMov As DataTable, ByVal Codban As String)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        ds = dsMov
        dt = dtMov
        Codigobanco = Codban
        AsignarTXT(Apuntador)
        HabilitarObjetos(False, True, cmbTipo, cmbTipoDeposito, txtFecha)

        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dt

            txtFecha.Text = FormatoFechaMedia(CDate(.Rows(nPosicion).Item("fechamov").ToString))
            RellenaCombo(aTipo, cmbTipo, InArray(aaTipo, .Rows(nPosicion).Item("tipomov")) - 1)
            RellenaCombo(aTipoDeposito, cmbTipoDeposito, .Rows(nPosicion).Item("multican"))
            txtDocumento.Text = .Rows(nPosicion).Item("numdoc")
            txtConcepto.Text = IIf(IsDBNull(.Rows(nPosicion).Item("concepto")), "", .Rows(nPosicion).Item("concepto"))
            txtImporte.Text = FormatoNumero(Math.Abs(.Rows(nPosicion).Item("importe")))
            txtCodigoBeneficiario.Text = IIf(IsDBNull(.Rows(nPosicion).Item("prov_cli")), "", .Rows(nPosicion).Item("prov_cli"))
            txtBeneficiario.Text = IIf(IsDBNull(.Rows(nPosicion).Item("benefic")), "", .Rows(nPosicion).Item("benefic"))
            numComprobante = IIf(IsDBNull(.Rows(nPosicion).Item("comproba")), "", .Rows(nPosicion).Item("comproba"))

            AbrirAsiento(numComprobante)

        End With
    End Sub
    Private Sub AbrirAsiento(ByVal NumeroComprobante As String)

        strSQLMov = " select a.* from jsbanordpag a" _
            & " where " _
            & " a.comproba = '" & NumeroComprobante & "' and " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, MyConn, nTablaMovimientos, lblInfo)
        dtMovimientos = ds.Tables(nTablaMovimientos)

        Dim aCampos() As String = {"renglon", "codcon", "refer", "concepto", "importe", ""}
        Dim aNombres() As String = {"N°", "Código cuenta", "Nº Referencia", "Concepto", "Importe", ""}
        Dim aAnchos() As Long = {50, 200, 130, 300, 100, 10}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {"", "", "", "", sFormatoNumero, ""}
        IniciarTabla(dgAsiento, dtMovimientos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtMovimientos.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        If c >= 0 Then
            Me.BindingContext(ds, nTablaMovimientos).Position = c
            dgAsiento.Refresh()
            dgAsiento.CurrentCell = dgAsiento(0, c)
        End If
    End Sub

    Private Sub jsBanArcBancosMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFecha.Text)
    End Sub

    Private Sub jsBanArcBancosMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtFecha.Text)
        Me.Tag = sModulo
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub
    Private Function Validado() As Boolean
        Validado = False
        If Trim(txtDocumento.Text) = "" Then
            MensajeEtiqueta(lblInfo, "Debe indicar un número de documento válido ...", TipoMensaje.iAdvertencia)
            EnfocarTexto(txtDocumento)
            Exit Function
        End If

        Dim aCampos() As String = {"tipomov", "numdoc", "id_emp"}
        Dim aValores() As String = {aaTipo(cmbTipo.SelectedIndex), txtDocumento.Text, jytsistema.WorkID}
        If qFound(MyConn, lblInfo, "jsbantraban", aCampos, aValores) AndAlso i_modo = movimiento.iAgregar Then
            MensajeEtiqueta(lblInfo, "Esté documento ya fué incluido, verifique ...", TipoMensaje.iAdvertencia)
            EnfocarTexto(txtDocumento)
            Exit Function
        End If

        If Trim(txtConcepto.Text) = "" Then
            MensajeEtiqueta(lblInfo, "Debe indicar un concepto ó comentario válido ...", TipoMensaje.iAdvertencia)
            EnfocarTexto(txtConcepto)
            Exit Function
        End If

        If cmbTipo.SelectedIndex = 2 AndAlso Trim(txtBeneficiario.Text) = "" Then
            MensajeEtiqueta(lblInfo, "Debe indicar un nombre de beneficiario ...", TipoMensaje.iAdvertencia)
            EnfocarTexto(txtBeneficiario)
            Exit Function
        End If

        If Not IsNumeric(txtImporte.Text) Then
            EnfocarTexto(txtImporte)
            Exit Function
        End If


        If CBool(ParametroPlus(MyConn, Gestion.iBancos, "BANPARAM08")) Then
            Dim montoAsiento As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select sum(importe) from jsbanordpag where comproba = '" & numComprobante & "' and id_emp = '" & jytsistema.WorkID & "' GROUP BY COMPROBA "))

            If montoAsiento <> 0 And dtMovimientos.Rows.Count > 0 Then
                MensajeAdvertencia(lblInfo, "DISCRIMINACION CONTABLE NO ES VALIDA. VERIFIQUE POR FAVOR...")
                Return False
            End If

            Dim DebitosAsiento As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select sum(importe) from jsbanordpag where comproba = '" & numComprobante & "' and debito_credito = 0 and id_emp = '" & jytsistema.WorkID & "' GROUP BY COMPROBA "))

            If DebitosAsiento < Math.Abs(ValorNumero(txtImporte.Text)) Then
                MensajeAdvertencia(lblInfo, "EL MONTO DEL MOVIMIENTO ES MAYOR AL CUADRE CONTABLE. VERIFIQUE POR FAVOR...")
                Return False
            End If

        End If


        If CBool(ParametroPlus(MyConn, Gestion.iBancos, "BANPARAM09")) AndAlso txtCodigoBeneficiario.Text.Trim() = "" _
            AndAlso cmbTipo.SelectedIndex = 2 Then
            MensajeCritico(lblInfo, "DEBE INDICAR UN PROVEEDOR VALIDO ...")
            Return False
        End If

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim Insertar As Boolean = False
        If Validado() Then
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = 0

            End If

            InsertEditBANCOSMovimientoBanco(MyConn, lblInfo, Insertar, CDate(txtFecha.Text), txtDocumento.Text, aaTipo(cmbTipo.SelectedIndex), _
                Codigobanco, "", txtConcepto.Text, IIf(cmbTipo.SelectedIndex < 2, ValorNumero(txtImporte.Text), -1 * ValorNumero(txtImporte.Text)), _
                "BAN", txtDocumento.Text, IIf(Trim(txtBeneficiario.Text) = "", ".", txtBeneficiario.Text), _
                numComprobante, "0", MyDate, MyDate, cmbTipo.SelectedIndex, "", MyDate, _
                cmbTipoDeposito.SelectedIndex, txtCodigoBeneficiario.Text, "")

            If cmbTipo.SelectedIndex = 2 And txtCodigoBeneficiario.Text <> "" Then

                Dim numCtaBan As String = CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select ctaban from jsbancatban where codban = '" & Codigobanco & "' and id_emp = '" & jytsistema.WorkID & "' "))
                Dim tipoProveedor As String = CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select tipo from jsprocatpro where codpro = '" & txtCodigoBeneficiario.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))

                EjecutarSTRSQL(MyConn, lblInfo, " delete from jsprotrapag where comproba = '" & numComprobante & "' and codpro = '" & txtCodigoBeneficiario.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                InsertEditCOMPRASCXP(MyConn, lblInfo, True, txtCodigoBeneficiario.Text, "NC", txtDocumento.Text, CDate(txtFecha.Text), _
                                     Format(Now(), "hh:mm:ss"), CDate(txtFecha.Text), numComprobante, txtConcepto.Text, _
                                     ValorNumero(txtImporte.Text), 0.0, "CH", txtDocumento.Text, Codigobanco, txtBeneficiario.Text, _
                                     "BAN", "", "", "", "", txtDocumento.Text, "0", _
                                     "", CDate(txtFecha.Text), "", "", "", 0.0, 0.0, numComprobante, Codigobanco, _
                                     numCtaBan, "", "", "", tipoProveedor, "1", "0")
            End If

            ImprimirComprobante()

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, Codigobanco & " " & txtDocumento.Text)
            Me.Close()
        End If

    End Sub
    Private Sub ImprimirComprobante()
        If cmbTipo.SelectedIndex = 2 Then
            Dim resp As Integer
            resp = MsgBox(" ¿Desea imprimir comprobante de egreso? ", MsgBoxStyle.YesNo, sModulo)
            If resp = MsgBoxResult.Yes Then
                Dim f As New jsBanRepParametros
                f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cComprobanteDeEgreso, "Comprobante de pago", Codigobanco, numComprobante)
                f = Nothing
            End If
        End If
    End Sub
    Private Sub btnIngreso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngreso.Click
        txtFecha.Text = SeleccionaFecha(CDate(txtFecha.Text), Me, btnIngreso)
    End Sub


    Private Sub cmbTipo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTipo.GotFocus
        MensajeEtiqueta(lblInfo, "seleccione el tipo de movimiento a realizar", TipoMensaje.iInfo)
    End Sub

    Private Sub txtDocumento_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDocumento.GotFocus
        MensajeEtiqueta(lblInfo, "Indique el número de documento para este movimiento ...", TipoMensaje.iInfo)
    End Sub

    Private Sub txtConcepto_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConcepto.GotFocus
        MensajeEtiqueta(lblInfo, "Indique el concepto por el cual se realiza este movimiento ...", TipoMensaje.iInfo)
    End Sub

    Private Sub txtImporte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtImporte.Click, _
        txtDocumento.Click, txtBeneficiario.Click, txtConcepto.Click
        Dim objTXT As TextBox = sender
        EnfocarTexto(objTXT)
    End Sub

    Private Sub txtImporte_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtImporte.GotFocus
        MensajeEtiqueta(lblInfo, "Indique el importe monetario de este movimiento ...", TipoMensaje.iInfo)
        EnfocarTexto(txtImporte)
    End Sub

    Private Sub txtBeneficiario_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBeneficiario.GotFocus
        MensajeEtiqueta(lblInfo, "Indique el nombre y apellido del beneficiario de este movimiento ...", TipoMensaje.iInfo)
    End Sub

    Private Sub txtImporte_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtImporte.KeyPress
        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub

    Private Sub txtImporte_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtImporte.TextChanged

    End Sub

    Private Sub cmbTipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipo.SelectedIndexChanged
        HabilitarObjetos(False, True, cmbTipoDeposito, txtCodigoBeneficiario, txtBeneficiario)

        Select Case cmbTipo.SelectedIndex
            Case 0 'Deposito
                HabilitarObjetos(True, True, cmbTipoDeposito)
            Case 1 'Nota de Crédito

            Case 2 'Cheque

                If CBool(ParametroPlus(MyConn, Gestion.iBancos, "BANPARAM09")) Then
                    HabilitarObjetos(True, True, btnBeneficiario)
                Else
                    HabilitarObjetos(True, True, txtBeneficiario)
                End If
            Case 3 'Nota Débito

        End Select


        ActualizarPrimerRegistroAsiento()


    End Sub


    Private Sub ActualizarPrimerRegistroAsiento()
        If CBool(ParametroPlus(MyConn, Gestion.iBancos, "BANPARAM08")) Then 'si exige movimientos contables obligatorios
            If numComprobante <> "" Then

                Dim Insertar As Boolean = Not CBool(EjecutarSTRSQL_ScalarPLUS(MyConn, " select count(*) from jsbanordpag where comproba = '" & numComprobante & "' and renglon = '00001' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "' "))
                Dim CodigoContable As String = EjecutarSTRSQL_ScalarPLUS(MyConn, " select codcon from jsbancatban where codban = '" & Codigobanco & "' and id_emp = '" & jytsistema.WorkID & "'")
                Dim signo As Integer = 1
                Select Case cmbTipo.SelectedIndex
                    Case 0, 1
                        signo = 1
                    Case Else
                        signo = -1
                End Select
                InsertEditBANCOSRenglonORDENPAGO(MyConn, lblInfo, Insertar, numComprobante, "00001", CodigoContable, _
                                                   txtDocumento.Text, txtConcepto.Text, signo * ValorNumero(txtImporte.Text), _
                                                    0, IIf(signo * ValorNumero(txtImporte.Text) > 0, 0, 1))
                AbrirAsiento(numComprobante)

            End If
        End If
    End Sub

    Private Sub btnAgregaCC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaCC.Click


        Dim f As New jsBanArcBancosMovimientosAsiento
        f.Apuntador = Me.BindingContext(ds, nTablamovimientos).Position
        Dim CodigoReferencia As String = ""
        If dtMovimientos.Rows.Count > 0 Then CodigoReferencia = dtMovimientos.Rows(Me.BindingContext(ds, nTablamovimientos).Position).Item("codcon").ToString
        f.Agregar(MyConn, ds, dtMovimientos, numComprobante, CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select sum(importe) from jsbanordpag where comproba = '" & numComprobante & "' and id_emp = '" & jytsistema.WorkID & "' group by comproba ")), CodigoReferencia)
        If f.Apuntador >= 0 Then AbrirAsiento(numComprobante)
        f = Nothing


    End Sub

    Private Sub btnEliminaCC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaCC.Click
        EliminarMovimiento()
    End Sub

    Private Sub EliminarMovimiento()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionRenglon = Me.BindingContext(ds, nTablamovimientos).Position

        If nPosicionRenglon >= 0 Then

            sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
            If sRespuesta = MsgBoxResult.Yes Then
                Dim nComproba As String = dtMovimientos.Rows(nPosicionRenglon).Item("comproba")
                With dtMovimientos.Rows(nPosicionRenglon)

                    InsertarAuditoria(MyConn, MovAud.iEliminar, sModulo, .Item("comproba") & .Item("renglon"))

                    Dim aFld() As String = {"comproba", "renglon", "codcon", "refer", "ejercicio", "id_emp"}
                    Dim aNFld() As String = {.Item("comproba"), .Item("renglon"), .Item("codcon"), .Item("refer"), jytsistema.WorkExercise, jytsistema.WorkID}
                    nPosicionRenglon = EliminarRegistros(MyConn, lblInfo, ds, nTablamovimientos, "jsbanordpag", strSQLMov, aFld, aNFld, nPosicionRenglon)

                End With



                ds = DataSetRequery(ds, strSQLMov, MyConn, nTablamovimientos, lblInfo)
                dtMovimientos = ds.Tables(nTablamovimientos)
                If dtMovimientos.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtMovimientos.Rows.Count - 1

                AbrirAsiento(nComproba)




            End If

        End If

    End Sub

    Private Sub btnBeneficiario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBeneficiario.Click
        txtCodigoBeneficiario.Text = CargarTablaSimple(MyConn, lblInfo, ds, " SELECT codpro codigo, nombre descripcion, rif from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by codpro ", _
                                                "Proveedores Gastos/Compras", txtCodigoBeneficiario.Text)

    End Sub

    Private Sub txtCodigoBeneficiario_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigoBeneficiario.TextChanged
        Dim benefic As String = CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select nombre from jsprocatpro where codpro = '" & txtCodigoBeneficiario.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
        If benefic <> "0" Then txtBeneficiario.Text = benefic
    End Sub

    Private Sub txtDocumento_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtDocumento.TextChanged, txtConcepto.TextChanged, _
        txtImporte.TextChanged
        ActualizarPrimerRegistroAsiento()
    End Sub
End Class