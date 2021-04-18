Imports MySql.Data.MySqlClient

Public Class jsProArcFormulacionesRenglones

    Private Const sModulo As String = "Movimiento renglones fórmulas"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer

    Private nPosicion As Integer
    Private Documento As String
    Private Almacen As String
    Private numRenglon As String
    Private n_Apuntador As Long

    Private CostoActual As Double
    Private PesoActual As Double
    Private Equivalencia As Double = 1
    Private aUnidades() As String = {}
    Private aCostos() As String = {}

    Private EsServicio As Boolean = False
    Private esResidual As Boolean = False

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property

    Public Sub Agregar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                       ByVal NumeroDocumento As String, Optional ByVal AlmacenDoc As String = "", _
                       Optional ByVal Servicio As Boolean = False, _
                       Optional Residual As Boolean = False)

        i_modo = movimiento.iAgregar

        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt
        Documento = NumeroDocumento
        Almacen = AlmacenDoc
        EsServicio = Servicio
        esResidual = Residual

        AsignarTooltips()
        Iniciar()
        Habilitar()
        IniciarTXT()

        Me.ShowDialog()

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                      ByVal NumeroDocumento As String, _
                      Optional ByVal AlmacenDoc As String = "", _
                      Optional ByVal Servicio As Boolean = False, _
                      Optional Residual As Boolean = False)


        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        Documento = NumeroDocumento
        Almacen = AlmacenDoc
        EsServicio = Servicio
        esResidual = Residual

        AsignarTooltips()
        Iniciar()
        Habilitar()
        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnCodigo, "<B>Seleccionar</B> mercancía")
        C1SuperTooltip1.SetToolTip(btnDescripcion, "<B>Indicar o agregar comentario </B>adicional a la descripción...")
        C1SuperTooltip1.SetToolTip(btnAlmacen, "selecciona la<B> causa</B> de la devolución...")

    End Sub
    Private Sub Habilitar()
        ft.habilitarObjetos(False, True, txtCostoTotal, txtPesoTotal)
    End Sub
    
    Private Sub Iniciar()

        If esResidual Then
            ft.visualizarObjetos(False, lblCostoUnitario, txtCostoUnitario, lblCostoTotal, txtCostoTotal, _
                              lblPesoDes, lblPesoTotal, txtPesoTotal, cmbSubEnsamble, lblTipoRenglon)
            lblAlmacen.Text = "Almacén Entrada"
        Else
            ft.visualizarObjetos(False, lblPorResidual, txtPorResidual, lblAlmacen, txtAlmacen, btnAlmacen, lblAlmacenDescripcion, _
                              cmbSubEnsamble, lblTipoRenglon)

        End If
        ft.habilitarObjetos(False, True, txtAlmacen)

    End Sub
    Private Sub IniciarTXT()

        numRenglon = ft.autoCodigo(MyConn, "renglon", "jsfabrenfor", "codfor.residual.id_emp", txtCodigo.Text + "." + IIf(esResidual, "1", "0") + "." + jytsistema.WorkID, 5)
        txtCodigo.Text = ""
        txtDescripcion.Text = ""
        cmbUnidad.Items.Clear()
        txtCostoUnitario.Text = ft.FormatoNumero(0.0)

        txtPorResidual.Text = ft.FormatoNumero(0.0)
        txtCostoTotal.Text = ft.FormatoNumero(0.0)
        txtAlmacen.Text = ParametroPlus(MyConn, Gestion.iProduccion, "PROPARAM01")
        lblAlmacenDescripcion.Text = ""
        txtPesoTotal.Text = ft.FormatoCantidad(0.0)
        txtCantidad.Text = ft.FormatoCantidad(1.0)

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)

            numRenglon = ft.muestraCampoTexto(.Item("renglon"))
            If .Item("item").ToString.Substring(0, 1) = "$" Then EsServicio = True
            txtCodigo.Text = ft.muestraCampoTexto(.Item("item"))
            txtDescripcion.Text = ft.muestraCampoTexto(.Item("descrip"))

            ft.RellenaCombo(aUnidades, cmbUnidad, ft.InArray(aUnidades, .Item("unidad")))
            txtCantidad.Text = ft.muestraCampoCantidad(.Item("cantidad"))

            txtCostoUnitario.Text = ft.muestraCampoNumero(.Item("costou"))
            txtPorResidual.Text = ft.muestraCampoNumero(.Item("porcentaje"))
            txtCostoTotal.Text = ft.muestraCampoNumero(.Item("totren"))
            txtAlmacen.Text = ft.muestraCampoTexto(.Item("almacen_salida"))
            txtPesoTotal.Text = ft.muestraCampoCantidad(.Item("peso_renglon"))



        End With
    End Sub
    Private Sub jsGenRenglonesMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsGenRenglonesMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
        txtCodigo.Focus()
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtDescripcion.GotFocus, cmbUnidad.GotFocus, txtCantidad.GotFocus, _
        txtCostoUnitario.GotFocus, _
        txtPorResidual.GotFocus, btnAlmacen.GotFocus, btnCodigo.GotFocus, _
        btnDescripcion.GotFocus

        Select Case sender.name
            Case "txtCodigo"
                ft.mensajeEtiqueta(lblInfo, "Indique el código producto ...", Transportables.TipoMensaje.iInfo)
            Case "txtDescripcion"
                ft.mensajeEtiqueta(lblInfo, "Indique la descripción o nombre de producto...", Transportables.TipoMensaje.iInfo)
            Case "cmbUnidad"
                ft.mensajeEtiqueta(lblInfo, "Seleccione la unidad para movimiento de producto ...", Transportables.TipoMensaje.iInfo)
            Case "txtCantidad"
                ft.mensajeEtiqueta(lblInfo, "Indique la cantidad del movimiento de producto", Transportables.TipoMensaje.iInfo)
            Case "txtCostoUnitario"
                ft.mensajeEtiqueta(lblInfo, "Indique el costo unitario de producto... ", Transportables.TipoMensaje.iInfo)
            Case "txtPorResidual"
                ft.mensajeEtiqueta(lblInfo, "Indique porcentaje residual de costo.. ", Transportables.TipoMensaje.iInfo)
            Case "txtCostoTotal"
                ft.mensajeEtiqueta(lblInfo, "Seleccione el costo total de producto... ", Transportables.TipoMensaje.iInfo)
            Case "txtAlmacen"
                ft.mensajeEtiqueta(lblInfo, "Seleccione el costo total de producto... ", Transportables.TipoMensaje.iInfo)

        End Select

    End Sub

    Private Function Validado() As Boolean

       If Trim(txtDescripcion.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar una descripción válida...")
            txtDescripcion.Focus()
            Return False
        End If

        If txtCodigo.Text.Substring(0, 1) <> "$" Then

            'CODIGO VALIDO
            If ft.DevuelveScalarCadena(MyConn, "select codart from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") = "0" Then
                ft.mensajeAdvertencia("Mercancía NO VALIDA...")
                Return False
            End If

            If ValorNumero(txtCantidad.Text) <= 0 Then
                ft.MensajeCritico("DEBE INDICAR UNA CANTIDAD VALIDA")
                ft.enfocarTexto(txtCantidad)
                Return False
            End If

            If ValorNumero(txtCostoTotal.Text) <= 0 Then
                ft.MensajeCritico("MERCANCIA SIN CANTIDAD O PRECIO POR FAVOR VERIFIQUE...")
                ft.enfocarTexto(txtCantidad)
                Return False
            End If

            If ValorNumero(txtCostoUnitario.Text) = 0.0 Then
                ft.mensajeAdvertencia("Movimiento no puede tener costo CERO (0.00). Veriqfique por favor ")
                ft.enfocarTexto(txtCostoUnitario)
                Return False
            End If

            If txtAlmacen.Text.Trim() = "" Then
                ft.mensajeCritico("DEBE INDICAR UN ALMACEN DE SALIDA PARA ESTE PRODUCTO...")
                btnAlmacen.Focus()
                Return False
            End If

        Else

            If ft.DevuelveScalarCadena(MyConn, "select codser from jsmercatser where codser = '" & txtCodigo.Text.Substring(1, txtCodigo.Text.Length - 1) & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                ft.mensajeAdvertencia("Servicio NO VALIDO...")
                Exit Function
            End If

            If ValorNumero(txtCostoUnitario.Text) = 0.0 Then
                ft.mensajeAdvertencia("Movimiento no puede tener costo/precio CERO (0.00). Veriqfique por favor ")
                ft.enfocarTexto(txtCostoUnitario)
                Exit Function
            End If
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

            InsertEditPRODUCCIONRenglonesFormulas(MyConn, lblInfo, Insertar, Documento, numRenglon, txtCodigo.Text, txtDescripcion.Text, _
                                                  ValorCantidad(txtCantidad.Text), cmbUnidad.Text, ValorCantidad(txtPesoTotal.Text), _
                                                  ValorNumero(txtCostoUnitario.Text), ValorNumero(txtCostoTotal.Text), txtAlmacen.Text, IIf(esResidual, 1, 0), _
                                                  cmbSubEnsamble.SelectedIndex, ValorNumero(txtPorResidual.Text))

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.Click, _
        txtCantidad.Click, txtCostoUnitario.Click
        Dim txt As TextBox = sender
        ft.enfocarTexto(txt)
    End Sub

    Private Sub cmbUnidad_SelectedItemChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        cmbUnidad.SelectedIndexChanged

        If txtCodigo.Text.Substring(0, 1) = "$" Then
            Dim afld() As String = {"codser", "id_emp"}
            Dim aStr() As String = {txtCodigo.Text.Substring(1, txtCodigo.Text.Length - 1), jytsistema.WorkID}

            Equivalencia = 1

            PesoActual = 0
            CostoActual = ValorNumero(qFoundAndSign(MyConn, lblInfo, "jsmercatser", afld, aStr, "precio"))

            txtCostoUnitario.Text = ft.FormatoNumero(CostoActual)
            txtDescripcion.Text = qFoundAndSign(MyConn, lblInfo, "jsmercatser", afld, aStr, "desser")

        Else
            Dim afld() As String = {"codart", "id_emp"}
            Dim aStr() As String = {txtCodigo.Text, jytsistema.WorkID}

            Dim afldX() As String = {"codart", "uvalencia", "id_emp"}
            Dim aStrX() As String = {txtCodigo.Text, CType(cmbUnidad.SelectedItem, String), jytsistema.WorkID}

            Equivalencia = FuncionesMercancias.Equivalencia(myConn,  txtCodigo.Text, aUnidades(cmbUnidad.SelectedIndex))
            aCostos = {0.0}

            txtCostoUnitario.Text = ft.FormatoNumero(0.0)

            PesoActual = ValorNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "pesounidad")) / IIf(Equivalencia = 0, 1, Equivalencia)

            CostoActual = ValorNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "montoultimacompra"))

            'cmbSubEnsamble.SelectedIndex = 0
            'cmbSubEnsamble.Enabled = True
            'cmbSubEnsamble.Visible = True

            txtCostoUnitario.Text = ft.FormatoNumero(CostoActual / IIf(Equivalencia = 0, 1, Equivalencia))
            txtDescripcion.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "nomart")

        End If


    End Sub

    Private Sub txtCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCantidad.KeyPress, _
        txtCostoUnitario.KeyPress, txtPorResidual.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub txtCantidad_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCantidad.TextChanged, _
        txtCostoUnitario.TextChanged, txtPorResidual.TextChanged

        txtCostoTotal.Text = ft.muestraCampoNumero(ValorCantidad(txtCantidad.Text) * ValorNumero(txtCostoUnitario.Text))
        txtPesoTotal.Text = ft.FormatoCantidad(PesoActual * ValorCantidad(txtCantidad.Text))

    End Sub

    Private Sub txtCodigo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigo.TextChanged
        If EsServicio Then
            If txtCodigo.Text <> "" Then
                If txtCodigo.Text.Substring(0, 1) = "$" Then
                    Dim afld() As String = {"codser", "id_emp"}
                    Dim aStr() As String = {txtCodigo.Text.Substring(1, txtCodigo.Text.Length - 1), jytsistema.WorkID}
                    If qFound(MyConn, lblInfo, "jsmercatser", afld, aStr) Then
                        Dim aUND() As String = {"UND"}
                        Dim aPRE() As String = {qFoundAndSign(MyConn, lblInfo, "jsmercatser", afld, aStr, "PRECIO")}
                        aUnidades = aUND
                        aCostos = aPRE
                        ft.RellenaCombo(aUnidades, cmbUnidad)
                    Else
                        ItemNoEncontrado()
                    End If
                Else
                    ItemNoEncontrado()
                End If
            End If
        Else

            Dim afld() As String = {"codart", "ESTATUS", "id_emp"}
            Dim aStr() As String = {txtCodigo.Text, "0", jytsistema.WorkID}
            Dim aP() As String = {"A", "B", "C", "D", "E", "F"}
            If qFound(MyConn, lblInfo, "jsmerctainv", afld, aStr) Then

                txtDescripcion.Text = ft.DevuelveScalarCadena(MyConn, " select nomart from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                aUnidades = ArregloUnidades(MyConn, lblInfo, txtCodigo.Text, ft.DevuelveScalarCadena(MyConn, " select UNIDAD from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                ft.RellenaCombo(aUnidades, cmbUnidad)

                Dim equiv As Double = 0.0 ' FuncionesMercancias.Equivalencia(myConn,  txtCodigo.Text, aUnidades(cmbUnidad.SelectedIndex))
                Dim UnidadDetalX As String = ft.DevuelveScalarCadena(MyConn, " select UNIDADDETAL from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

            Else
                ItemNoEncontrado()
            End If

        End If

    End Sub

    Private Sub ItemNoEncontrado()
        cmbUnidad.Items.Clear()
        txtDescripcion.Text = ""
        txtCostoUnitario.Text = ft.FormatoNumero(0.0)
        txtCostoTotal.Text = ft.FormatoNumero(0.0)
        txtPesoTotal.Text = ft.FormatoCantidad(0.0)
    End Sub
    Private Sub btnCodigo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigo.Click
        If EsServicio Then
            Dim g As New jsMerArcServiciosA
            g.Cargar(MyConn)
            txtCodigo.Text = g.Seleccionado
            g = Nothing
        Else
            Dim f As New jsMerArcListaCostosPreciosNormal
            f.Cargar(MyConn, TipoListaPrecios.CostosFormulaciones, Almacen)
            txtCodigo.Text = f.Seleccionado
            f = Nothing
        End If
    End Sub


    Private Sub btnDescripcion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDescripcion.Click
        'If txtDescripcion.Text <> "" Then
        '    Dim g As New jsGenComentariosRenglones
        '    Dim aFld() As String = {"numdoc", "origen", "item", "renglon", "id_emp"}
        '    Dim aStr() As String = {Documento, nModulo, txtCodigo.Text, numRenglon, jytsistema.WorkID}

        '    If qFound(MyConn, lblInfo, "jsvenrencom", aFld, aStr) Then
        '        g.Editar(MyConn, Documento, nModulo, txtCodigo.Text, numRenglon)
        '    Else
        '        g.Agregar(MyConn, Documento, nModulo, txtCodigo.Text, numRenglon)
        '    End If
        '    g = Nothing
        'End If
    End Sub

    Private Sub txtAlmacen_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtAlmacen.TextChanged
        lblAlmacenDescripcion.Text = ft.DevuelveScalarCadena(MyConn, " SELECT DESALM FROM jsmercatalm where codalm = '" + txtAlmacen.Text + "' and id_emp = '" + jytsistema.WorkID + "' ")
    End Sub

    Private Sub btnAlmacen_Click(sender As System.Object, e As System.EventArgs) Handles btnAlmacen.Click
        txtAlmacen.Text = CargarTablaSimple(MyConn, lblInfo, dsLocal, " select codalm codigo, desalm descripcion from jsmercatalm where id_emp = '" + jytsistema.WorkID + "' order by codalm ", "ALMACENES", txtAlmacen.Text)
    End Sub
End Class