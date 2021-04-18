Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Public Class jsBanArcBancosMovimientosPlus
    Private Const sModulo As String = "Movimiento bancario"

    Private MyConn As New MySqlConnection
    Private ds As DataSet
    Private dt As DataTable
    Private dtMovimientos As DataTable
    Private ft As New Transportables

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
    Private nConciliado As String = "0"
    Private nMesConcilia As Date = MyDate
    Private nFecConcilia As Date = MyDate
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
        ft.habilitarObjetos(False, True, cmbTipoDeposito)

        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()


        txtFecha.Value = jytsistema.sFechadeTrabajo
        txtDocumento.Text = ""
        txtConcepto.Text = ""
        txtImporte.Text = ft.FormatoNumero(0.0)

        txtCodigoBeneficiario.Text = ""
        txtBeneficiario.Text = ""

        ft.RellenaCombo(aTipo, cmbTipo)
        ft.RellenaCombo(aTipoDeposito, cmbTipoDeposito)

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
        ft.habilitarObjetos(False, True, cmbTipo, cmbTipoDeposito)

        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dt.Rows(nPosicion)


            txtFecha.Value = .Item("fechamov")
            ft.RellenaCombo(aTipo, cmbTipo, ft.InArray(aaTipo, .Item("tipomov")))
            ft.RellenaCombo(aTipoDeposito, cmbTipoDeposito, .Item("multican"))
            txtDocumento.Text = ft.muestraCampoTexto(.Item("numdoc"))
            txtConcepto.Text = ft.muestraCampoTexto(.Item("concepto"))
            txtImporte.Text = ft.muestraCampoNumero(Math.Abs(.Item("importe")))
            txtCodigoBeneficiario.Text = ft.muestraCampoTexto(.Item("prov_cli"))
            txtBeneficiario.Text = ft.muestraCampoTexto(.Item("benefic"))
            numComprobante = ft.muestraCampoTexto(.Item("comproba"))

            nConciliado = .Item("conciliado")
            nMesConcilia = CDate(.Item("mesconcilia").ToString)
            nFecConcilia = CDate(.Item("fecconcilia").ToString)

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
        Dim aAnchos() As Integer = {50, 200, 130, 300, 100, 10}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {"", "", "", "", sFormatoNumero, ""}
        IniciarTabla(dgAsiento, dtMovimientos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtMovimientos.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        Else
            ActualizarPrimerRegistroAsiento()
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
        ft = Nothing
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFecha.Value)
    End Sub

    Private Sub jsBanArcBancosMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dates As SfDateTimeEdit() = {txtFecha}
        SetSizeDateObjects(dates)

        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtFecha.Value)
        Me.Tag = sModulo

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Function Validado() As Boolean

        If FechaUltimoBloqueo(MyConn, "jsbantraban") >= txtFecha.Value Then
            ft.mensajeEtiqueta(lblInfo, "FECHA MENOR QUE ULTIMA FECHA DE CIERRE...", Transportables.tipoMensaje.iAdvertencia)
            Return False
        End If

        If Trim(txtDocumento.Text) = "" Then
            ft.mensajeEtiqueta(lblInfo, "Debe indicar un número de documento válido ...", Transportables.TipoMensaje.iAdvertencia)
            ft.enfocarTexto(txtDocumento)
            Return False
        End If

        Dim aCampos() As String = {"tipomov", "numdoc", "id_emp"}
        Dim aValores() As String = {aaTipo(cmbTipo.SelectedIndex), txtDocumento.Text, jytsistema.WorkID}
        If qFound(MyConn, lblInfo, "jsbantraban", aCampos, aValores) AndAlso i_modo = movimiento.iAgregar Then
            ft.mensajeEtiqueta(lblInfo, "Esté documento ya fué incluido, verifique ...", Transportables.TipoMensaje.iAdvertencia)
            ft.enfocarTexto(txtDocumento)
            Return False
        End If

        If Trim(txtConcepto.Text) = "" Then
            ft.mensajeEtiqueta(lblInfo, "Debe indicar un concepto ó comentario válido ...", Transportables.TipoMensaje.iAdvertencia)
            ft.enfocarTexto(txtConcepto)
            Return False
        End If

        If cmbTipo.SelectedIndex = 2 AndAlso Trim(txtBeneficiario.Text) = "" Then
            ft.mensajeEtiqueta(lblInfo, "Debe indicar un nombre de beneficiario ...", Transportables.TipoMensaje.iAdvertencia)
            ft.enfocarTexto(txtBeneficiario)
            Return False
        End If

        If Not ft.isNumeric(txtImporte.Text) Then
            ft.enfocarTexto(txtImporte)
            Return False
        End If


        If CBool(ParametroPlus(MyConn, Gestion.iBancos, "BANPARAM08")) Then
            Dim montoAsiento As Double = ft.DevuelveScalarDoble(MyConn, " select sum(importe) from jsbanordpag where comproba = '" & numComprobante & "' and id_emp = '" & jytsistema.WorkID & "' GROUP BY COMPROBA ")

            If montoAsiento <> 0 And dtMovimientos.Rows.Count > 0 Then
                ft.mensajeAdvertencia("DISCRIMINACION CONTABLE NO ES VALIDA. VERIFIQUE POR FAVOR...")
                Return False
            End If

            Dim DebitosAsiento As Double = ft.DevuelveScalarDoble(MyConn, " select SUM(IMPORTE) from jsbanordpag where comproba = '" & numComprobante & "' and debito_credito = 0 and id_emp = '" & jytsistema.WorkID & "' GROUP BY COMPROBA ")

            If DebitosAsiento < Math.Abs(ValorNumero(txtImporte.Text)) Then
                ft.mensajeAdvertencia("EL MONTO DEL MOVIMIENTO ES MAYOR AL CUADRE CONTABLE. VERIFIQUE POR FAVOR...")
                Return False
            End If

        End If


        If CBool(ParametroPlus(MyConn, Gestion.iBancos, "BANPARAM09")) AndAlso txtCodigoBeneficiario.Text.Trim() = "" _
            AndAlso cmbTipo.SelectedIndex = 2 Then
            ft.MensajeCritico("DEBE INDICAR UN PROVEEDOR VALIDO ...")
            Return False
        End If

        Return True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Dim Insertar As Boolean = False

        If Validado() Then

            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = 0

            End If

            InsertEditBANCOSMovimientoBanco(MyConn, lblInfo, Insertar, txtFecha.Value, txtDocumento.Text, aaTipo(cmbTipo.SelectedIndex),
                Codigobanco, "", txtConcepto.Text, IIf(cmbTipo.SelectedIndex < 2, ValorNumero(txtImporte.Text), -1 * ValorNumero(txtImporte.Text)),
                "BAN", txtDocumento.Text, IIf(Trim(txtBeneficiario.Text) = "", ".", txtBeneficiario.Text),
                numComprobante, nConciliado, nMesConcilia, nFecConcilia, aaTipo(cmbTipo.SelectedIndex), "", MyDate,
                cmbTipoDeposito.SelectedIndex, txtCodigoBeneficiario.Text, "")

            If cmbTipo.SelectedIndex = 2 And txtCodigoBeneficiario.Text <> "" Then

                Dim numCtaBan As String = ft.DevuelveScalarCadena(MyConn, " select ctaban from jsbancatban where codban = '" & Codigobanco & "' and id_emp = '" & jytsistema.WorkID & "' ")
                Dim tipoProveedor As String = ft.DevuelveScalarCadena(MyConn, " select tipo from jsprocatpro where codpro = '" & txtCodigoBeneficiario.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                ft.Ejecutar_strSQL(MyConn, " delete from jsprotrapag where comproba = '" & numComprobante & "' and codpro = '" & txtCodigoBeneficiario.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                InsertEditCOMPRASCXP(MyConn, lblInfo, True, txtCodigoBeneficiario.Text, "NC", txtDocumento.Text, txtFecha.Value,
                                     Format(Now(), "hh:mm:ss"), txtFecha.Value, numComprobante, txtConcepto.Text,
                                     ValorNumero(txtImporte.Text), 0.0, "CH", txtDocumento.Text, Codigobanco, txtBeneficiario.Text,
                                     "BAN", "", "", "", "", txtDocumento.Text, "0",
                                     "", txtFecha.Value, "", "", "", 0.0, 0.0, numComprobante, Codigobanco,
                                     numCtaBan, "", "", "", tipoProveedor, "1", "0")
            End If


            IncluirImpuestoDebitoBancario(MyConn, lblInfo, aaTipo(cmbTipo.SelectedIndex), Codigobanco, txtDocumento.Text,
                                            txtFecha.Value, Convert.ToDouble(txtImporte.Text))
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

    Private Sub cmbTipo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTipo.GotFocus
        ft.mensajeEtiqueta(lblInfo, "seleccione el tipo de movimiento a realizar", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtDocumento_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDocumento.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el número de documento para este movimiento ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtConcepto_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtConcepto.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el concepto por el cual se realiza este movimiento ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtImporte_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtImporte.Click, _
        txtDocumento.Click, txtBeneficiario.Click, txtConcepto.Click
        Dim objTXT As TextBox = sender
        ft.enfocarTexto(objTXT)
    End Sub

    Private Sub txtImporte_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtImporte.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el importe monetario de este movimiento ...", Transportables.TipoMensaje.iInfo)
        ft.enfocarTexto(txtImporte)
    End Sub

    Private Sub txtBeneficiario_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBeneficiario.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el nombre y apellido del beneficiario de este movimiento ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtImporte_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtImporte.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub txtImporte_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtImporte.TextChanged

    End Sub

    Private Sub cmbTipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipo.SelectedIndexChanged
        ft.habilitarObjetos(False, True, cmbTipoDeposito, txtCodigoBeneficiario, txtBeneficiario)

        Select Case cmbTipo.SelectedIndex
            Case 0 'Deposito
                ft.habilitarObjetos(True, True, cmbTipoDeposito)
            Case 1 'Nota de Crédito

            Case 2 'Cheque

                If CBool(ParametroPlus(MyConn, Gestion.iBancos, "BANPARAM09")) Then
                    ft.habilitarObjetos(True, True, btnBeneficiario)
                Else
                    ft.habilitarObjetos(True, True, txtBeneficiario)
                End If
            Case 3 'Nota Débito

        End Select


        ActualizarPrimerRegistroAsiento()


    End Sub


    Private Sub ActualizarPrimerRegistroAsiento()
        If CBool(ParametroPlus(MyConn, Gestion.iBancos, "BANPARAM08")) Then 'si exige movimientos contables obligatorios
            If numComprobante <> "" Then

                Dim Insertar As Boolean = Not ft.DevuelveScalarBooleano(MyConn, " select count(*) from jsbanordpag where comproba = '" & numComprobante & "' and renglon = '00001' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "' ")
                Dim CodigoContable As String = ft.DevuelveScalarCadena(MyConn, " select codcon from jsbancatban where codban = '" & Codigobanco & "' and id_emp = '" & jytsistema.WorkID & "'")
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
        f.Agregar(MyConn, ds, dtMovimientos, numComprobante, ft.DevuelveScalarDoble(MyConn, " select SUM(IMPORTE) from jsbanordpag where comproba = '" & numComprobante & "' and id_emp = '" & jytsistema.WorkID & "' group by comproba "), CodigoReferencia)
        If f.Apuntador >= 0 Then AbrirAsiento(numComprobante)
        f = Nothing


    End Sub
    Private Sub btnEditarCC_Click(sender As Object, e As EventArgs) Handles btnEditarCC.Click

        Dim f As New jsBanArcBancosMovimientosAsiento
        f.Apuntador = Me.BindingContext(ds, nTablamovimientos).Position
        f.Editar(MyConn, ds, dtMovimientos, numComprobante)
        If f.Apuntador >= 0 Then AbrirAsiento(numComprobante)
        f = Nothing

    End Sub
    Private Sub btnEliminaCC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaCC.Click
        EliminarMovimiento()
    End Sub

    Private Sub EliminarMovimiento()


        nPosicionRenglon = Me.BindingContext(ds, nTablamovimientos).Position

        If nPosicionRenglon >= 0 Then


            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
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
        Dim benefic As String = ft.DevuelveScalarCadena(MyConn, " select nombre from jsprocatpro where codpro = '" & txtCodigoBeneficiario.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        If benefic <> "0" Then txtBeneficiario.Text = benefic
    End Sub

    Private Sub txtDocumento_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtDocumento.TextChanged, txtConcepto.TextChanged, _
        txtImporte.TextChanged
        ActualizarPrimerRegistroAsiento()
    End Sub

    Private Sub dgAsiento_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgAsiento.RowHeaderMouseClick,
        dgAsiento.CellMouseClick, dgAsiento.RegionChanged
        Me.BindingContext(ds, nTablamovimientos).Position = e.RowIndex
        nPosicionRenglon = e.RowIndex
    End Sub
End Class