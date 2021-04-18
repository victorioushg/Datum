Imports MySql.Data.MySqlClient
Public Class jsMerArcOfertasMovimientosBono

    Private Const sModulo As String = "Movimiento de Bonificación de oferta"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

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
        ft.habilitarObjetos(False, True, txtUnidad, txtCodigo, txtDescripcion, txtDescripcionBono, txtUnidadCada, txtCodigoCada, _
                         txtDescripcionCada)
        If i_modo = movimiento.iEditar Then _
            ft.habilitarObjetos(False, True, txtCodigoBono, btnCodigoBono)
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
        txtCantidad.Text = ft.FormatoCantidad(0.0)
        txtCantidadBono.Text = ft.FormatoCantidad(0.0)
        txtCantidadCada.Text = ft.FormatoCantidad(0.0)
        ft.RellenaCombo(aOtorgante, cmbOtorgante)
        txtCodigoBono.Text = ""
        cmbUnidadBono.Items.Clear()
        txtCantidadBono.Text = ft.FormatoCantidad(0.0)
        txtDescripcionBono.Text = ""
        numRenglon = ft.autoCodigo(MyConn, "renglon", "jsmerrenbon", "codofe.id_emp", NumeroOferta + "." + jytsistema.WorkID, 5)

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)

        With dtLocal.Rows(nPosicion)
            Dim aFld() As String = {"codart", "id_emp"}
            Dim aStr() As String = {.Item("codart"), jytsistema.WorkID}

            txtCantidad.Text = ft.muestraCampoCantidad(.Item("cantidad"))
            txtUnidad.Text = .Item("unidad")
            txtCodigo.Text = .Item("codart")
            txtDescripcion.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "nomart")

            txtCodigoBono.Text = .Item("itembon")
            txtDescripcionBono.Text = ft.muestraCampoTexto(.Item("nombreitembon"))
            txtCantidadBono.Text = ft.muestraCampoCantidad(.Item("cantidadbon"))
            ft.RellenaCombo(aUnidades, cmbUnidadBono, ft.InArray(aUnidades, .Item("unidadbon")))

            txtCantidadCada.Text = ft.FormatoCantidad(.Item("cantidadinicio"))
            txtUnidadCada.Text = .Item("unidad")
            txtCodigoCada.Text = .Item("codart")
            txtDescripcionCada.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", aFld, aStr, "nomart")

            ft.RellenaCombo(aOtorgante, cmbOtorgante, ft.InArray(aOtorgante, .Item("otorgacan")))

            numRenglon = .Item("renglon")

        End With
    End Sub

    Private Sub jsMerArcOfertasMovimientosBono_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing '
    End Sub

    Private Sub jsMerArcOfertasMovimientosBono_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigoBono.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigoBono.GotFocus, _
        txtDescripcionBono.GotFocus, txtUnidad.GotFocus, txtCantidadBono.GotFocus, _
        btnCodigoBono.GotFocus

        Select Case sender.name
            Case "txtCodigo"
                ft.mensajeEtiqueta(lblInfo, "Indique el codigo de la mercancía...", Transportables.TipoMensaje.iInfo)
            Case "txtPrecio"
                ft.mensajeEtiqueta(lblInfo, "Indique el precio de la mercancía... ", Transportables.TipoMensaje.iInfo)
            Case "btnCodigo"
                ft.mensajeEtiqueta(lblInfo, "Seleccione la mercancía a cambiar el precio ...", Transportables.TipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If ValorCantidad(txtCantidad.Text) < 0.0 Then
            ft.mensajeAdvertencia("Debe indicar una cantidad de inicio válida para esta bonificación. Verifique por favor...")
            ft.enfocarTexto(txtCantidad)
            Exit Function
        End If

        If txtDescripcionBono.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar una mercancía a bonificar válida. Verifique por favor...")
            ft.enfocarTexto(txtCodigoBono)
            Exit Function
        End If

        If ValorNumero(txtCantidadBono.Text) <= 0.0 Then
            ft.mensajeAdvertencia("Debe indicar una cantidad a bonificar válida. Verifique por favor ")
            ft.enfocarTexto(txtCantidadBono)
            Exit Function
        Else
            If MultiploValido(MyConn, txtCodigoBono.Text, cmbUnidadBono.Text, ValorCantidad(txtCantidadBono.Text), lblInfo) Then
            Else
                ft.mensajeAdvertencia("La cantidad a bonificar NO es múltiplo válido para este movimiento. Verifique por favor...")
                ft.enfocarTexto(txtCantidadBono)
                Exit Function
            End If
        End If

        If ValorNumero(txtCantidadCada.Text) <= 0.0 Then
            ft.mensajeAdvertencia("Debe indicar una cantidad del período por bonificar. Verifique por favor ")
            ft.enfocarTexto(txtCantidadCada)
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
        ft.enfocarTexto(txt)
    End Sub


    Private Sub txtCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCantidadBono.KeyPress, _
        txtCantidad.KeyPress, txtCantidadCada.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub txtCodigoBono_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigoBono.TextChanged
        Dim afld() As String = {"codart", "id_emp"}
        Dim aStr() As String = {txtCodigoBono.Text, jytsistema.WorkID}


        If qFound(MyConn, lblInfo, "jsmerctainv", afld, aStr) Then
            aUnidades = ArregloUnidades(MyConn, lblInfo, txtCodigoBono.Text, ft.DevuelveScalarCadena(MyConn, " select UNIDAD from jsmerctainv where codart = '" & txtCodigoBono.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
            txtDescripcionBono.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "nomart")
            ft.RellenaCombo(aUnidades, cmbUnidadBono)
        Else
            cmbUnidadBono.Items.Clear()
            txtDescripcionBono.Text = ""
        End If

    End Sub

    Private Sub btnCodigo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigoBono.Click
        Dim f As New jsMerArcListaCostosPreciosNormal
        f.Cargar(MyConn, 0)
        txtCodigoBono.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub txtCodigoBono_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCodigoBono.KeyPress
        e.Handled = ValidaAlfaNumericoEnTextbox(e)
    End Sub
End Class