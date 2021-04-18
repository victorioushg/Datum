Imports MySql.Data.MySqlClient

Public Class jsProArcFormulacionesRenglones

    Private Const sModulo As String = "Movimiento renglones fórmulas"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable

    Private i_modo As Integer

    Private nPosicion As Integer
    Private nModulo As String
    Private Documento As String
    Private Fechadocumento As Date
    Private Almacen As String
    Private numRenglon As String
    Private n_Apuntador As Long

    Private AceptaDescuento As Boolean = True
    Private CostoActual As Double
    Private PesoActual As Double
    Private Equivalencia As Double = 1
    Private aUnidades() As String = {}
    Private aCostos() As String = {}

    Private EsServicio As Boolean = False

    Private ValidaMultiplosDeUnidadDeVenta As Boolean = True


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
                       Optional ByVal Servicio As Boolean = False)

        i_modo = movimiento.iAgregar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt
        Documento = NumeroDocumento
        Almacen = AlmacenDoc
        EsServicio = Servicio

        AsignarTooltips()
        Iniciar(nModulo)
        Habilitar(nModulo)
        VerificarParametros(nModulo)
        IniciarTXT()

        Me.ShowDialog()

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                      ByVal Modulo As String, ByVal NumeroDocumento As String, _
                      Optional ByVal AlmacenDoc As String = "", _
                      Optional ByVal Servicio As Boolean = False)


        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        nModulo = Modulo
        Documento = NumeroDocumento
        Almacen = AlmacenDoc
        EsServicio = Servicio
     
        AsignarTooltips()
        Iniciar(nModulo)
        Habilitar(nModulo)
        AsignarTXT(Apuntador)
        VerificarParametros(nModulo)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnCodigo, "<B>Seleccionar</B> mercancía")
        C1SuperTooltip1.SetToolTip(btnDescripcion, "<B>Indicar o agregar comentario </B>adicional a la descripción...")
        C1SuperTooltip1.SetToolTip(btnCausa, "selecciona la<B> causa</B> de la devolución...")

    End Sub
    Private Sub Habilitar(ByVal nomModulo As String)
     
        HabilitarObjetos(False, True, txtCostoTotal, txtPesoTotal)

    End Sub
    Private Sub VerificarParametros(ByVal nomModulo As String)
        
        VisualizarObjetos(True, txtCostoUnitario)
        If EsServicio Then
            VisualizarObjetos(True, txtCostoUnitario)
            HabilitarObjetos(True, True, txtCostoUnitario)
        End If

    End Sub


    Private Sub Iniciar(ByVal nomModulo As String)
        VisualizarObjetos(False, lblPorResidual, txtPorResidual)
        VisualizarObjetos(False, lblAlmacen, txtAlmacen, btnCausa, lblAlmacenDescripcion)
        VisualizarObjetos(False, cmbSubEnsamble, lblTipoRenglon)

    End Sub
    Private Sub IniciarTXT()

        numRenglon = AutoCodigo(5, dsLocal, dtLocal.TableName, "renglon")
        txtCodigo.Text = ""
        txtDescripcion.Text = ""
        cmbUnidad.Items.Clear()
        txtCostoUnitario.Text = FormatoNumero(0.0)

        txtPorResidual.Text = FormatoNumero(0.0)
        txtCostoTotal.Text = FormatoNumero(0.0)
        txtAlmacen.Text = ""
        lblAlmacenDescripcion.Text = ""
        txtPesoTotal.Text = FormatoCantidad(0.0)
        txtCantidad.Text = FormatoCantidad(1.0)

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)

            numRenglon = .Item("renglon")
            If .Item("item").ToString.Substring(0, 1) = "$" Then EsServicio = True
            txtCodigo.Text = .Item("item")
            txtDescripcion.Text = .Item("descrip")

            RellenaCombo(aUnidades, cmbUnidad, InArray(aUnidades, .Item("unidad")) - 1)


            txtCostoUnitario.Text = FormatoNumero(.Item("costou"))

            txtPorResidual.Text = FormatoNumero(0.0)
            txtCostoTotal.Text = FormatoNumero(.Item("totren"))
            txtAlmacen.Text = "" '  .Item("causa")


            txtPesoTotal.Text = FormatoCantidad(.Item("peso"))
            txtCantidad.Text = FormatoCantidad(.Item("cantidad"))


        End With
    End Sub
    Private Sub jsGenRenglonesMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        '
    End Sub

    Private Sub jsGenRenglonesMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo & nModulo, txtCodigo.Text)
        txtCodigo.Focus()
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtDescripcion.GotFocus, cmbUnidad.GotFocus, txtCantidad.GotFocus, _
        txtCostoUnitario.GotFocus, _
        txtPorResidual.GotFocus, btnCausa.GotFocus, btnCodigo.GotFocus, _
        btnDescripcion.GotFocus

        Select Case sender.name
            Case "txtCodigo"
                MensajeEtiqueta(lblInfo, "Indique el código de la mercancía ...", TipoMensaje.iInfo)
            Case "txtDescripcion"
                MensajeEtiqueta(lblInfo, "Indique la descripción o nombre de la mercancía ...", TipoMensaje.iInfo)
            Case "cmbUnidad"
                MensajeEtiqueta(lblInfo, "Seleccione la unidad para movimiento de mercancía ...", TipoMensaje.iInfo)
            Case "txtFactura"
                MensajeEtiqueta(lblInfo, "Indique el Número de factura de la cual se desea hacer la devolución de mercancía ...", TipoMensaje.iInfo)
            Case "txtLote"
                MensajeEtiqueta(lblInfo, "Indique el número de lote al cual pertenece la mercancía...", TipoMensaje.iInfo)
            Case "txtCantidad"
                MensajeEtiqueta(lblInfo, "Indique la cantidad del movimiento de mercancía", TipoMensaje.iInfo)
            Case "txtCostoPrecio"
                If InStr("TRF.ORD.REC.GAS.COM.NCC.NDC", nModulo) = 0 Then
                    MensajeEtiqueta(lblInfo, "Indique el precio de la mercancía... ", TipoMensaje.iInfo)
                Else
                    MensajeEtiqueta(lblInfo, "Indique el costo de la mercancía... ", TipoMensaje.iInfo)
                End If

            Case "cmbCostoPrecio"
                If InStr("TRF.ORD.REC.GAS.COM.NCC.NDC", nModulo) = 0 Then
                    MensajeEtiqueta(lblInfo, "Seleccione el precio de la mercancía... ", TipoMensaje.iInfo)
                Else
                    MensajeEtiqueta(lblInfo, "Seleccione el costo de la mercancía... ", TipoMensaje.iInfo)
                End If
            Case "txtDesc_art"
                MensajeEtiqueta(lblInfo, "Indique descuento por mercancía...", TipoMensaje.iInfo)
            Case "txtDesc_cli"
                If InStr("TRF.ORD.REC.GAS.COM.NCC.NDC", nModulo) = 0 Then
                    MensajeEtiqueta(lblInfo, "Indique descuento por cliente...", TipoMensaje.iInfo)
                Else
                    MensajeEtiqueta(lblInfo, "Indique descuento por Proveedor ...", TipoMensaje.iInfo)
                End If
            Case "txtDesc_ofe"
                MensajeEtiqueta(lblInfo, "Indique descuento por oferta...", TipoMensaje.iInfo)
            Case "txtPorAceptaDev"
                MensajeEtiqueta(lblInfo, "Indique porcentaje devolución...", TipoMensaje.iInfo)
            Case "btnCausa"
                MensajeEtiqueta(lblInfo, "Seleccione la causa de devolución", TipoMensaje.iInfo)
            Case "btnCantidadTC"
                MensajeEtiqueta(lblInfo, "Seleccione las cantidades por talla y color de esta mercancía...", TipoMensaje.iInfo)
            Case "btnPesoCaptura"
                MensajeEtiqueta(lblInfo, "Seleccione el peso de la mercancía desde la balanza... ", TipoMensaje.iInfo)
            Case "btnLote"
                MensajeEtiqueta(lblInfo, "Seleccione el Número de lote de inventario...", TipoMensaje.iInfo)
            Case "btnCodigo"
                MensajeEtiqueta(lblInfo, "Seleccione el código de mercancía para el movimiento...", TipoMensaje.iInfo)
            Case "btnFactura"
                MensajeEtiqueta(lblInfo, "Seleccione el número de factura a la cual pertenece la mercancía", TipoMensaje.iInfo)
            Case "btnDescripcion"
                MensajeEtiqueta(lblInfo, "Agregar comentario más extenso a la descripción presentada", TipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False

        Dim CantidadReal As Double = ValorCantidad(txtCantidad.Text) / IIf(Equivalencia = 0, 1, Equivalencia)
        Dim CantidadMovimiento As Double = MovimientoXDocumentoRenglonAlmacen(MyConn, txtCodigo.Text, Documento, nModulo, numRenglon, Almacen, lblInfo)

        If Trim(txtDescripcion.Text) = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar una descripción válida...")
            txtDescripcion.Focus()
            Exit Function
        End If

        If txtCodigo.Text.Substring(0, 1) <> "$" Then

            'CODIGO VALIDO
            If CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select codart from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")) = "0" Then
                MensajeAdvertencia(lblInfo, "Mercancía NO VALIDA...")
                Exit Function
            End If

            If InStr(nModulo, "PVE.COT.PPE.PED.PFC.FAC.NDV") > 0 Then

                Dim MercanciaInactiva As Boolean = CBool(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "Select estatus from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))

                If MercanciaInactiva Then
                    MensajeCritico(lblInfo, "ESTA MERCANCIA POSEE ESTATUS DE INACTIVA POR FAVOR DIRIGASE AL SUPERVISOR ...")
                    Return False
                End If

            End If


            'EXISTENCIAS
            If nModulo = "TRF" AndAlso CBool(ParametroPlus(MyConn, Gestion.iMercancías, "MERTRAPA01")) Then
                If CantidadReal > (ExistenciaEnAlmacen(MyConn, txtCodigo.Text, Almacen, lblInfo) + CantidadMovimiento) Then
                    MensajeAdvertencia(lblInfo, "Cantidad es mayor a la existente en el almacén " & Almacen & " ...")
                    EnfocarTexto(txtCantidad)
                    Return False
                End If
            End If

            If nModulo = "PVE" AndAlso CBool(ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM21")) Then
                If CantidadReal > (ExistenciaEnAlmacen(MyConn, txtCodigo.Text, Almacen, lblInfo) + CantidadMovimiento) Then
                    MensajeCritico(lblInfo, "Cantidad es mayor a la existente en el almacén " & Almacen & " ...")
                    EnfocarTexto(txtCantidad)
                    Return False
                End If
            End If

            If nModulo = "PFC" AndAlso CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNOTPA13").ToString) Then
                If CantidadReal > (ExistenciaEnAlmacen(MyConn, txtCodigo.Text, Almacen, lblInfo) + CantidadMovimiento) Then
                    MensajeCritico(lblInfo, "Cantidad es mayor a la existente en el almacén " & Almacen & " ...")
                    EnfocarTexto(txtCantidad)
                    Return False
                End If
            End If

            If nModulo = "FAC" AndAlso CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA12").ToString) Then
                If CantidadReal > (ExistenciaEnAlmacen(MyConn, txtCodigo.Text, Almacen, lblInfo) + CantidadMovimiento) Then
                    MensajeAdvertencia(lblInfo, "Cantidad es mayor a la existente en el almacén " & Almacen & " ...")
                    EnfocarTexto(txtCantidad)
                    Return False
                End If
            End If


            If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNDBPA14").ToString) AndAlso nModulo = "NDB" Then
                If CantidadReal > (ExistenciaEnAlmacen(MyConn, txtCodigo.Text, Almacen, lblInfo) + CantidadMovimiento) Then
                    MensajeAdvertencia(lblInfo, "Cantidad es mayor a la existente en el almacén " & Almacen & " ...")
                    EnfocarTexto(txtCantidad)
                    Return False
                End If
            End If

            If ValorNumero(txtCantidad.Text) <= 0 Then
                MensajeCritico(lblInfo, "DEBE INDICAR UNA CANTIDAD VALIDA")
                EnfocarTexto(txtCantidad)
                Exit Function
            End If

            If ValorNumero(txtCostoTotal.Text) <= 0 Then
                MensajeCritico(lblInfo, "MERCANCIA SIN CANTIDAD O PRECIO POR FAVOR VERIFIQUE...")
                EnfocarTexto(txtCantidad)
                Exit Function
            End If

            ' CANTIDAD MULTIPLO VALIDO 
            If ValidaMultiplosDeUnidadDeVenta Then

                If cmbUnidad.Text <> "KGR" Then
                    If MultiploValido(MyConn, txtCodigo.Text, cmbUnidad.Text, ValorCantidad(txtCantidad.Text), lblInfo) Then
                    Else
                        MensajeCritico(lblInfo, "La cantidad NO es múltiplo válido para este movimiento. Verifique por favor...")
                        EnfocarTexto(txtCantidad)
                        Exit Function
                    End If
                End If
            End If

            If ValorNumero(txtCostoUnitario.Text) = 0.0 AndAlso InStr(nModulo, "ORD.TRF.REC.GAS.COM.NCC.NDC") > 0 Then
                MensajeAdvertencia(lblInfo, "Movimiento no puede tener costo CERO (0.00). Veriqfique por favor ")
                EnfocarTexto(txtCostoUnitario)
                Exit Function
            End If

            'CAUSA CREDITO DEBITO


        Else
            If EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select codser from jsmercatser where codser = '" & txtCodigo.Text.Substring(1, txtCodigo.Text.Length - 1) & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                MensajeAdvertencia(lblInfo, "Servicio NO VALIDO...")
                Exit Function
            End If

            If ValorNumero(txtCostoUnitario.Text) = 0.0 Then
                MensajeAdvertencia(lblInfo, "Movimiento no puede tener costo/precio CERO (0.00). Veriqfique por favor ")
                EnfocarTexto(txtCostoUnitario)
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

            Dim NumeroSerialFiscal As String = ""
            If jytsistema.WorkBox <> "" Then NumeroSerialFiscal = NumeroSERIALImpresoraFISCAL(MyConn, lblInfo, jytsistema.WorkBox)


            'InsertEditMERCASRenglonTransferencia(MyConn, lblInfo, Insertar, Documento, numRenglon, txtCodigo.Text, txtDescripcion.Text, cmbUnidad.Text, ValorCantidad(txtCantidad.Text), ValorCantidad(txtPesoTotal.Text), txtLote.Text, ValorNumero(txtCostoPrecio.Text), ValorNumero(txtCostoPrecioTotal.Text), "1")

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo & nModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.Click, _
        txtCantidad.Click, txtCostoUnitario.Click
        Dim txt As TextBox = sender
        EnfocarTexto(txt)
    End Sub

    Private Sub cmbUnidad_SelectedItemChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        cmbUnidad.SelectedIndexChanged

        If txtCodigo.Text.Substring(0, 1) = "$" Then
            Dim afld() As String = {"codser", "id_emp"}
            Dim aStr() As String = {txtCodigo.Text.Substring(1, txtCodigo.Text.Length - 1), jytsistema.WorkID}

            Equivalencia = 1

            PesoActual = 0
            CostoActual = ValorNumero(qFoundAndSign(MyConn, lblInfo, "jsmercatser", afld, aStr, "precio"))

            txtCostoUnitario.Text = FormatoNumero(CostoActual)
            txtDescripcion.Text = qFoundAndSign(MyConn, lblInfo, "jsmercatser", afld, aStr, "desser")

        Else
            Dim afld() As String = {"codart", "id_emp"}
            Dim aStr() As String = {txtCodigo.Text, jytsistema.WorkID}

            Dim afldX() As String = {"codart", "uvalencia", "id_emp"}
            Dim aStrX() As String = {txtCodigo.Text, CType(cmbUnidad.SelectedItem, String), jytsistema.WorkID}

            Equivalencia = FuncionesMercancias.Equivalencia(MyConn, lblInfo, txtCodigo.Text, aUnidades(cmbUnidad.SelectedIndex))
            aCostos = {0.0}

            txtCostoUnitario.Text = FormatoNumero(0.0)

            PesoActual = ValorNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "pesounidad")) / IIf(Equivalencia = 0, 1, Equivalencia)

            Select Case nModulo
                Case "TRF", "ORD", "REC", "GAS", "COM", "NCC", "NDC"
                    CostoActual = ValorNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "montoultimacompra"))
            End Select

            AceptaDescuento = CBool(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "descuento"))

            If Not AceptaDescuento Then
                If InStr("PVE.COT.PPE.PED.PFC.FAC.NDV", nModulo) > 0 Then
                    cmbSubEnsamble.SelectedIndex = 1
                    cmbSubEnsamble.Enabled = False
                Else
                    cmbSubEnsamble.SelectedIndex = 0
                    cmbSubEnsamble.Enabled = True
                    cmbSubEnsamble.Visible = True
                End If
            Else

            End If


            txtCostoUnitario.Text = FormatoNumero(CostoActual / IIf(Equivalencia = 0, 1, Equivalencia))
            txtDescripcion.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "nomart")

        End If


    End Sub

    Private Sub txtCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCantidad.KeyPress, _
        txtCostoUnitario.KeyPress, txtPorResidual.KeyPress
        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub

    Private Sub txtCantidad_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCantidad.TextChanged, _
        txtCostoUnitario.TextChanged, txtPorResidual.TextChanged

        'txtCostoPrecioTotal.Text = FormatoNumero(PrecioTotal(ValorCantidad(txtCantidad.Text), ValorNumero(txtCostoPrecio.Text), _
        '                                                     ValorNumero(txtPorAceptaDev.Text), ValorNumero(txtDesc_art.Text), _
        '                                                     ValorNumero(txtDesc_cli.Text), ValorNumero(txtDesc_ofe.Text)))

        txtPesoTotal.Text = FormatoCantidad(PesoActual * ValorCantidad(txtCantidad.Text))

    End Sub

    Private Function PrecioTotal(ByVal Cantidad As Double, ByVal PrecioCosto As Double, ByVal PorcentajeAceptacion As Double, _
                                 ByVal PorcentajeDescuentoArticulo As Double, ByVal PorcentajeDescuentoCliente As Double, _
                                 ByVal PorcentajeDescuentoOferta As Double) As Double
        Dim PrecioNeto As Double = Cantidad * PrecioCosto * IIf(nModulo = "NCV" Or nModulo = "NCC", PorcentajeAceptacion / 100, 1)

        Dim DescuentoArticulo As Double = 0.0
        Dim DescuentoCliente As Double = 0.0
        Dim DescuentoOferta As Double = 0.0

        Select Case nModulo
            Case "NCV", "NCC"
            Case Else
                DescuentoArticulo = PrecioNeto * PorcentajeDescuentoArticulo / 100
                DescuentoCliente = (PrecioNeto - DescuentoArticulo) * PorcentajeDescuentoCliente / 100
                DescuentoOferta = (PrecioNeto - DescuentoArticulo - DescuentoCliente) * PorcentajeDescuentoOferta / 100
        End Select

        PrecioTotal = PrecioNeto - DescuentoArticulo - DescuentoCliente - DescuentoOferta

    End Function

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
                        RellenaCombo(aUnidades, cmbUnidad)
                        If InStr("NCV.NCC", nModulo) > 0 Then txtPorResidual.Text = FormatoNumero(100)
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

                txtDescripcion.Text = CStr(EjecutarSTRSQL_ScalarPLUS(MyConn, " select nomart from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                aUnidades = ArregloUnidades(MyConn, lblInfo, txtCodigo.Text, CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select UNIDAD from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")))
                RellenaCombo(aUnidades, cmbUnidad)

                Dim equiv As Double = 0.0 ' FuncionesMercancias.Equivalencia(MyConn, lblInfo, txtCodigo.Text, aUnidades(cmbUnidad.SelectedIndex))
                Dim UnidadDetalX As String = CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select UNIDADDETAL from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))


                If InStr("NCV.NCC", nModulo) > 0 Then txtPorResidual.Text = FormatoNumero(CDbl(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "por_acepta_dev")))

            Else
                ItemNoEncontrado()
            End If

        End If

    End Sub



    Private Sub ItemNoEncontrado()
        cmbUnidad.Items.Clear()
        txtDescripcion.Text = ""
        txtCostoUnitario.Text = FormatoNumero(0.0)
        txtCostoTotal.Text = FormatoNumero(0.0)
        txtPesoTotal.Text = FormatoCantidad(0.0)
        AceptaDescuento = True
    End Sub
    Private Sub btnCodigo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigo.Click
        If EsServicio Then
            Dim g As New jsMerArcServiciosA
            g.Cargar(MyConn)
            txtCodigo.Text = g.Seleccionado
            g = Nothing
        Else

            Dim f As New jsMerArcListaCostosPreciosPlus
            f.Cargar(MyConn, TipoListaPrecios.Costos, Almacen)
            txtCodigo.Text = f.Seleccionado
            f = Nothing
        End If
    End Sub


    Private Sub btnDescripcion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDescripcion.Click
        If txtDescripcion.Text <> "" Then
            Dim g As New jsGenComentariosRenglones
            Dim aFld() As String = {"numdoc", "origen", "item", "renglon", "id_emp"}
            Dim aStr() As String = {Documento, nModulo, txtCodigo.Text, numRenglon, jytsistema.WorkID}

            If qFound(MyConn, lblInfo, "jsvenrencom", aFld, aStr) Then
                g.Editar(MyConn, Documento, nModulo, txtCodigo.Text, numRenglon)
            Else
                g.Agregar(MyConn, Documento, nModulo, txtCodigo.Text, numRenglon)
            End If
            g = Nothing
        End If
    End Sub

    Private Sub btnCausa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCausa.Click
        Dim f As New jsControlArcCausasCreditos
        f.Cargar(MyConn, IIf(InStr("NCV.NCC", nModulo) > 0, 0, 1), TipoCargaFormulario.iShowDialog)
        If f.Seleccionado <> "" Then txtAlmacen.Text = f.Seleccionado
        f = Nothing
    End Sub
    Private Sub txtCausa_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlmacen.TextChanged

        Dim aVal As String = ConvertirIntegerEnSiNo(CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select validaunidad from jsvencaudcr where codigo = '" & txtAlmacen.Text _
                              & "' and credito_debito = '" & IIf(InStr("NCV.NCC", nModulo) > 0, 0, 1) & "' and id_emp = '" & jytsistema.WorkID & "' "))) & " valida cantidad por unidad de venta"

        ValidaMultiplosDeUnidadDeVenta = CBool(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select validaunidad from jsvencaudcr where codigo = '" & txtAlmacen.Text _
                              & "' and credito_debito = '" & IIf(InStr("NCV.NCC", nModulo) > 0, 0, 1) & "' and id_emp = '" & jytsistema.WorkID & "' "))

        Dim aAju As String = ConvertirIntegerEnSiNo(CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select ajustaprecio from jsvencaudcr where codigo = '" & txtAlmacen.Text _
                              & "' and credito_debito = '" & IIf(InStr("NCV.NCC", nModulo) > 0, 0, 1) & "' and id_emp = '" & jytsistema.WorkID & "' "))) & " causa ajuste en los costos de mercancía"
        Dim aEst As String = ConvertirIntegerEnSiNo(CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select estado from jsvencaudcr where codigo = '" & txtAlmacen.Text _
                              & "' and credito_debito = '" & IIf(InStr("NCV.NCC", nModulo) > 0, 0, 1) & "' and id_emp = '" & jytsistema.WorkID & "' "))) & " es mercancía en buen estado"
        Dim aInv As String = ConvertirIntegerEnSiNo(CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select inventario from jsvencaudcr where codigo = '" & txtAlmacen.Text _
                              & "' and credito_debito = '" & IIf(InStr("NCV.NCC", nModulo) > 0, 0, 1) & "' and id_emp = '" & jytsistema.WorkID & "' "))) & " causa movimiento en inventario "

        lblAlmacenDescripcion.Text = aInv & "; " & aVal & "; " & aAju & "; " & aEst

    End Sub

   
    Private Sub CargarPrecios(CodigoMercancia As String, nModulo As String)

        Dim origenPrecios() As String
        If CodigoMercancia.Substring(1, 1) = "$" Then
            origenPrecios = {"PRECIO"}
        Else
            Select Case nModulo
                Case "TRF", "ORD", "REC", "GAS", "COM", "NCC", "NDC"
                    origenPrecios = {"MONTOULTIMACOMPRA"}
                Case "PVE", "COT", "PPE", "PED", "APT", "PFC", "FAC", "NCV", "NDV"
                    origenPrecios = {"PRECIO_A", "PRECIO_B", "PRECIO_C", "PRECIO_D", "PRECIO_E", "PRECIO_F"}
            End Select
        End If

    End Sub



End Class