Imports MySql.Data.MySqlClient
Public Class jsMerArcOfertasMovimiento

    Private Const sModulo As String = "Movimiento de porcentaje de oferta"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private dtBonos As DataTable
    Private nTableBonos As String = "nnTablaBonos"

    Private i_modo As Integer

    Private nPosicion As Integer
    Private nPosicionBono As Integer
    Private n_Apuntador As Long

    Private aOtorgapor() As String = {"Datum", "Asesor"}
    Private NumeroOferta As String
    Private strSQLBono As String = ""
    Private numRenglon As String = ""

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property

    Public Sub Agregar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal NumOferta As String)

        i_modo = movimiento.iAgregar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt
        NumeroOferta = NumOferta

        AsignarTooltips()
        Habilitar()
        IniciarTXT()

        Me.ShowDialog()

    End Sub
    Public Sub Editar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal numOferta As String)

        i_modo = movimiento.iEditar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt
        NumeroOferta = numOferta
        AsignarTooltips()
        Habilitar()
        AsignarTXT(Apuntador)


        Me.ShowDialog()

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnCodigo, "<B>Selecciona mercancia</B> para este movimiento ...")
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar bonificacacion</B> para esta oferta ...")
        C1SuperTooltip1.SetToolTip(btnCodigo, "<B>Modificar bonificacion</B> para esta oferta ...")
        C1SuperTooltip1.SetToolTip(btnCodigo, "<B>Eliminar bonificacion</B> para esta oferta ...")

    End Sub
    Private Sub Habilitar()
        HabilitarObjetos(False, True, txtUnidad, txtDescripcion)
        If i_modo = movimiento.iEditar Then _
            HabilitarObjetos(False, True, txtCodigo, btnCodigo)
    End Sub
    Private Sub IniciarTXT()

        txtCodigo.Text = ""
        txtDescripcion.Text = ""
        txtLimiteInferior.Text = FormatoCantidad(0.0)
        txtLimiteSuperior.Text = FormatoCantidad(0.0)
        txtUnidad.Text = ""
        txtPorcentaje.Text = FormatoNumero(0.0)
        RellenaCombo(aOtorgapor, cmbOtorgaPor)
        AsignarBonificaciones(NumeroOferta, txtCodigo.Text)
        numRenglon = AutoCodigo(5, dsLocal, dtLocal.TableName, "renglon")

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)

            txtCodigo.Text = .Item("codart")
            txtDescripcion.Text = MuestraCampoTexto(.Item("descrip"), "")
            txtUnidad.Text = MuestraCampoTexto(.Item("unidad"), "")
            txtLimiteInferior.Text = FormatoNumero(.Item("limitei"))
            txtLimiteSuperior.Text = FormatoNumero(.Item("limites"))
            txtPorcentaje.Text = FormatoNumero(.Item("porcentaje"))
            RellenaCombo(aOtorgapor, cmbOtorgaPor, InArray(aOtorgapor, .Item("otorgapor")) - 1)
            AsignarBonificaciones(NumeroOferta, txtCodigo.Text)
            numRenglon = .Item("renglon")

        End With
    End Sub
    Private Sub AsignarBonificaciones(ByVal CodigoOferta As String, ByVal CodigoArticulo As String)

        strSQLBono = "select a.codart, a.renglon, a.unidad, a.cantidad, a.cantidadbon, a.cantidadinicio, a.unidadbon, a.itembon,  " _
                           & " a.nombreitembon, a.codart codigoart, a.unidad unidadart, elt( a.otorgacan + 1, 'Datum','Asesor') otorgacan  " _
                           & " from jsmerrenbon a " _
                           & " where " _
                           & " a.codofe = '" & CodigoOferta & "' and " _
                           & " a.codart = '" & CodigoArticulo & "' and " _
                           & " a.id_emp = '" & jytsistema.WorkID & "' " _
                           & " order by a.renglon "

        dsLocal = DataSetRequery(dsLocal, strSQLBono, MyConn, nTableBonos, lblInfo)
        dtBonos = dsLocal.Tables(nTableBonos)

        Dim aCampos() As String = {"cantidad", "unidad", "codart", "cantidadbon", "unidadbon", "itembon", "nombreitembon", "cantidadinicio", "unidadart", "codigoart", "otorgacan"}
        Dim aNombres() As String = {"A partir de...", " ", "de...", "Se bonificara...", " ", " ", " ", "Por cada...", "", "de...", "Otorgado por..."}
        Dim aAnchos() As Long = {70, 40, 90, 70, 40, 90, 200, 70, 40, 90, 90}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {sFormatoCantidad, "", "", sFormatoCantidad, "", "", "", sFormatoCantidad, "", "", ""}
        IniciarTabla(dgBonos, dtBonos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtBonos.Rows.Count > 0 Then nPosicionBono = 0

    End Sub
    Private Sub jsMerArcOfertasMovimiento_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        '
    End Sub

    Private Sub jsMerArcOfertasMovimiento_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtDescripcion.GotFocus, txtUnidad.GotFocus, txtLimiteInferior.GotFocus, _
        btnCodigo.GotFocus, txtPorcentaje.GotFocus

        Select Case sender.name
            Case "txtCodigo"
                MensajeEtiqueta(lblInfo, "Indique el codigo de la mercancía...", TipoMensaje.iInfo)
            Case "txtPorcentaje"
                MensajeEtiqueta(lblInfo, "Indique un porcentaje oferta para esta mercancía... ", TipoMensaje.iInfo)
            Case "btnCodigo"
                MensajeEtiqueta(lblInfo, "Seleccione la mercancía a cambiar el precio ...", TipoMensaje.iInfo)
            Case "txtLimiteInferior", "txtLimiteSuperior"
                MensajeEtiqueta(lblInfo, "Indique cantidad de mercancía en esta oferta ... ", TipoMensaje.iInfo)
            Case "cmbOtorgaPor"
                MensajeEtiqueta(lblInfo, "Seleccione el otorgante de esta orferta...", TipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If ValorNumero(txtLimiteInferior.Text) < 0.0 Then
            MensajeAdvertencia(lblInfo, "El límite inferior no puede ser menor de CERO (0.00). Veriqfique por favor ")
            EnfocarTexto(txtLimiteInferior)
            Exit Function
        End If

        If ValorNumero(txtLimiteSuperior.Text) < 0.0 Then
            MensajeAdvertencia(lblInfo, "El límite superior no puede ser menor de CERO (0.00). Veriqfique por favor ")
            EnfocarTexto(txtLimiteSuperior)
            Exit Function
        End If

        If ValorNumero(txtPorcentaje.Text) < 0.0 Or ValorNumero(txtPorcentaje.Text) > 100 Then
            MensajeAdvertencia(lblInfo, "Indique un porcentaje valido... ")
            EnfocarTexto(txtPorcentaje)
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

            InsertEditMERCASRenglonOferta(MyConn, lblInfo, Insertar, NumeroOferta, numRenglon, _
                                           txtCodigo.Text, txtDescripcion.Text, txtUnidad.Text, _
                                           CDbl(txtLimiteInferior.Text), CDbl(txtLimiteSuperior.Text), _
                                           CDbl(txtPorcentaje.Text), cmbOtorgaPor.SelectedIndex, "1")

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.Click
        Dim txt As TextBox = sender
        EnfocarTexto(txt)
    End Sub


    Private Sub txtCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtLimiteInferior.KeyPress, _
        txtLimiteSuperior.KeyPress, txtPorcentaje.KeyPress
        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub

    Private Sub txtCodigo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigo.TextChanged
        Dim afld() As String = {"codart", "id_emp"}
        Dim aStr() As String = {txtCodigo.Text, jytsistema.WorkID}
        txtDescripcion.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "nomart")
        txtUnidad.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "unidad")
    End Sub

    Private Sub btnCodigo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigo.Click
        Dim f As New jsMerArcListaCostosPreciosPlus
        f.Cargar(MyConn, 0)
        txtCodigo.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub dgBonos_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgBonos.CellContentClick

    End Sub
    Private Sub dgBonos_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgBonos.RowHeaderMouseClick, _
      dgBonos.CellMouseClick
        nPosicionBono = e.RowIndex
        Me.BindingContext(dsLocal, nTableBonos).Position = e.RowIndex
        AsignaMov(nPosicionBono, False)
    End Sub
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then
            dsLocal = DataSetRequery(dsLocal, strSQLBono, MyConn, nTableBonos, lblInfo)
            dtBonos = dsLocal.Tables(nTableBonos)
        End If

        If c >= 0 AndAlso dtBonos.Rows.Count > 0 Then
            Me.BindingContext(dsLocal, nTableBonos).Position = c
            dgBonos.Refresh()
            dgBonos.CurrentCell = dgBonos(0, c)
        End If

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim f As New jsMerArcOfertasMovimientosBono
        f.Apuntador = Me.BindingContext(dsLocal, nTableBonos).Position
        f.Agregar(MyConn, dsLocal, dtBonos, NumeroOferta, txtCodigo.Text)
        dsLocal = DataSetRequery(dsLocal, strSQLBono, MyConn, nTableBonos, lblInfo)
        AsignaMov(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim f As New jsMerArcOfertasMovimientosBono
        f.Apuntador = Me.BindingContext(dsLocal, nTableBonos).Position
        f.Editar(MyConn, dsLocal, dtBonos, NumeroOferta, txtCodigo.Text)
        dsLocal = DataSetRequery(dsLocal, strSQLBono, MyConn, nTableBonos, lblInfo)
        AsignaMov(f.Apuntador, True)
        f = Nothing

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        EliminarMovimiento()
    End Sub
    Private Sub EliminarMovimiento()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionBono = Me.BindingContext(dsLocal, nTableBonos).Position

        If nPosicionBono >= 0 Then
            sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
            If sRespuesta = MsgBoxResult.Yes Then
                With dtBonos.Rows(nPosicionBono)
                    InsertarAuditoria(MyConn, MovAud.iEliminar, sModulo, .Item("codofe") & .Item("codart"))
                    Dim aCamposDel() As String = {"codofe", "codart", "id_emp"}
                    Dim aStringsDel() As String = {.Item("codofe"), .Item("codart"), jytsistema.WorkID}
                    nPosicionBono = EliminarRegistros(MyConn, lblInfo, dsLocal, nTableBonos, "jsmerrenbon", strSQLBono, aCamposDel, aStringsDel, _
                                                  Me.BindingContext(dsLocal, nTableBonos).Position, True)

                    If dtBonos.Rows.Count - 1 < nPosicionBono Then nPosicionBono = dtBonos.Rows.Count - 1
                    AsignarBonificaciones(NumeroOferta, txtCodigo.Text)
                End With

            End If
        End If

    End Sub
End Class