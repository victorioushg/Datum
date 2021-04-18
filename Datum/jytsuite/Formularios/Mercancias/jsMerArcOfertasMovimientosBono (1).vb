Imports MySql.Data.MySqlClient
Public Class jsMerArcOfertasMovimientosBono

    Private Const sModulo As String = "Movimiento de Bonificación de oferta"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable

    Private i_modo As Integer

    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private NumeroOferta As String
    Private CodigoArticulo As String
    Private aOtorgante() As String = {"Datum", "Asesor"}
    Private aUnidades() As String = {}
    Private numRenglon As String = ""
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property

    Public Sub Agregar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                       ByVal NumOferta As String, ByVal CodArticulo As String)

        i_modo = movimiento.iAgregar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt
        NumeroOferta = NumOferta
        CodigoArticulo = CodArticulo

        AsignarTooltips()
        Habilitar()
        IniciarTXT()

        Me.ShowDialog()

    End Sub
    Public Sub Editar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                      ByVal numOferta As String, ByVal CodArticulo As String)

        i_modo = movimiento.iEditar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt
        NumeroOferta = numOferta
        CodigoArticulo = CodArticulo

        AsignarTooltips()
        Habilitar()
        AsignarTXT(Apuntador)


        Me.ShowDialog()

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnCodigoBono, "<B>Selecciona mercancia</B> para este movimiento ...")

    End Sub
    Private Sub Habilitar()
        HabilitarObjetos(False, True, txtUnidad, txtCodigo, txtDescripcion, txtDescripcionBono, txtUnidadCada, txtCodigoCada, _
                         txtDescripcionCada)
        If i_modo = movimiento.iEditar Then _
            HabilitarObjetos(False, True, txtCodigoBono, btnCodigoBono)
    End Sub
    Private Sub IniciarTXT()

        Dim aFld() As String = {"codart", "id_emp"}
        Dim aStr() As String = {CodigoArticulo, jytsistema.WorkID}

        txtUnidad.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "unidad")
        txtCodigo.Text = CodigoArticulo
        txtDescripcion.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "nomart")
        txtUnidadCada.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "unidad")
        txtCodigoCada.Text = CodigoArticulo
        txtDescripcionCada.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "nomart")
        txtCantidad.Text = FormatoCantidad(0.0)
        txtCantidadBono.Text = FormatoCantidad(0.0)
        txtCantidadCada.Text = FormatoCantidad(0.0)
        RellenaCombo(aOtorgante, cmbOtorgante)
        txtCodigoBono.Text = ""
        cmbUnidadBono.Items.Clear()
        txtCantidadBono.Text = FormatoCantidad(0.0)
        txtDescripcionBono.Text = ""
        numRenglon = AutoCodigo(5, dsLocal, dtLocal.TableName, "renglon")

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)

        With dtLocal.Rows(nPosicion)
            Dim aFld() As String = {"codart", "id_emp"}
            Dim aStr() As String = {.Item("codart"), jytsistema.WorkID}

            txtCantidad.Text = FormatoCantidad(.Item("cantidad"))
            txtUnidad.Text = .Item("unidad")
            txtCodigo.Text = .Item("codart")
            txtDescripcion.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "nomart")

            txtCodigoBono.Text = .Item("itembon")
            txtDescripcionBono.Text = MuestraCampoTexto(.Item("nombreitembon"), "")
            txtCantidadBono.Text = FormatoCantidad(.Item("cantidadbon"))
            RellenaCombo(aUnidades, cmbUnidadBono, InArray(aUnidades, .Item("unidadbon")) - 1)

            txtCantidadCada.Text = FormatoCantidad(.Item("cantidadinicio"))
            txtUnidadCada.Text = .Item("unidad")
            txtCodigoCada.Text = .Item("codart")
            txtDescripcionCada.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "nomart")

            RellenaCombo(aOtorgante, cmbOtorgante, InArray(aOtorgante, .Item("otorgacan")) - 1)

            numRenglon = .Item("renglon")

        End With
    End Sub

    Private Sub jsMerArcOfertasMovimientosBono_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        '
    End Sub

    Private Sub jsMerArcOfertasMovimientosBono_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigoBono.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigoBono.GotFocus, _
        txtDescripcionBono.GotFocus, txtUnidad.GotFocus, txtCantidadBono.GotFocus, _
        btnCodigoBono.GotFocus

        Select Case sender.name
            Case "txtCodigo"
                MensajeEtiqueta(lblInfo, "Indique el codigo de la mercancía...", TipoMensaje.iInfo)
            Case "txtPrecio"
                MensajeEtiqueta(lblInfo, "Indique el precio de la mercancía... ", TipoMensaje.iInfo)
            Case "btnCodigo"
                MensajeEtiqueta(lblInfo, "Seleccione la mercancía a cambiar el precio ...", TipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If ValorCantidad(txtCantidad.Text) < 0.0 Then
            MensajeAdvertencia(lblInfo, "Debe indicar una cantidad de inicio válida para esta bonificación. Verifique por favor...")
            EnfocarTexto(txtCantidad)
            Exit Function
        End If

        If txtDescripcionBono.Text = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar una mercancía a bonificar válida. Verifique por favor...")
            EnfocarTexto(txtCodigoBono)
            Exit Function
        End If

        If ValorNumero(txtCantidadBono.Text) <= 0.0 Then
            MensajeAdvertencia(lblInfo, "Debe indicar una cantidad a bonificar válida. Verifique por favor ")
            EnfocarTexto(txtCantidadBono)
            Exit Function
        Else
            If MultiploValido(MyConn, txtCodigoBono.Text, cmbUnidadBono.Text, ValorCantidad(txtCantidadBono.Text), lblInfo) Then
            Else
                MensajeAdvertencia(lblInfo, "La cantidad a bonificar NO es múltiplo válido para este movimiento. Verifique por favor...")
                EnfocarTexto(txtCantidadBono)
                Exit Function
            End If
        End If

        If ValorNumero(txtCantidadCada.Text) <= 0.0 Then
            MensajeAdvertencia(lblInfo, "Debe indicar una cantidad del período por bonificar. Verifique por favor ")
            EnfocarTexto(txtCantidadCada)
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

            InsertEditMERCASRenglonBonificacion(MyConn, lblInfo, Insertar, NumeroOferta, txtCodigo.Text, _
                                                numRenglon, txtUnidad.Text, ValorCantidad(txtCantidad.Text), _
                                                ValorCantidad(txtCantidadBono.Text), ValorCantidad(txtCantidadCada.Text), _
                                                cmbUnidadBono.Text, txtCodigoBono.Text, txtDescripcionBono.Text, cmbOtorgante.SelectedIndex, "1")

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text & txtCodigoBono.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigoBono.Click
        Dim txt As TextBox = sender
        EnfocarTexto(txt)
    End Sub


    Private Sub txtCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCantidadBono.KeyPress, _
        txtCantidad.KeyPress, txtCantidadCada.KeyPress
        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub

    Private Sub txtCodigoBono_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigoBono.TextChanged
        Dim afld() As String = {"codart", "id_emp"}
        Dim aStr() As String = {txtCodigoBono.Text, jytsistema.WorkID}


        If qFound(MyConn, lblInfo, "jsmerctainv", afld, aStr) Then
            aUnidades = ArregloUnidades(MyConn, lblInfo, txtCodigoBono.Text, CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select UNIDAD from jsmerctainv where codart = '" & txtCodigoBono.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")))
            txtDescripcionBono.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "nomart")
            RellenaCombo(aUnidades, cmbUnidadBono)
        Else
            cmbUnidadBono.Items.Clear()
            txtDescripcionBono.Text = ""
        End If

    End Sub

    Private Sub btnCodigo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigoBono.Click
        Dim f As New jsMerArcListaCostosPreciosPlus
        f.Cargar(MyConn, 0)
        txtCodigoBono.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub txtCodigoBono_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCodigoBono.KeyPress
        e.Handled = ValidaAlfaNumericoEnTextbox(e)
    End Sub
End Class