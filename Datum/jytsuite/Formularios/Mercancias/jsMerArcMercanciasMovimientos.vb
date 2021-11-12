Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsMerArcMercanciasMovimientos

    Private Const sModulo As String = "Movimiento inventario de mercancías"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer

    Private nPosicion As Integer
    Private n_Apuntador As Long
    Private nDocumento As String

    Private CostoActual As Double
    Private PesoActual As Double
    Private Equivalencia As Double = 1
    Private aUnidades() As String = {}
    Private numRenglon As String
    Private aTipo() As String = {"Entrada", "Salida", "Ajuste Entrada", "Ajuste Salida", "Ajuste Costo"}
    Private aTipoX() As String = {"EN", "SA", "AE", "AS", "AC"}
    Private CodMercancia As String
    Private DesMercancia As String

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Property Documento() As String
        Get
            Return nDocumento
        End Get
        Set(ByVal value As String)
            nDocumento = value
        End Set
    End Property

    Public Sub Agregar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable,
                       ByVal CodigoMercancia As String, ByVal Descripcion As String)

        i_modo = movimiento.iAgregar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt
        CodMercancia = CodigoMercancia
        DesMercancia = Descripcion

        AsignarTooltips()
        Habilitar()
        IniciarTXT()

        Me.ShowDialog()

    End Sub
    Public Sub Editar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable,
                       ByVal CodigoMercancia As String, ByVal Descripcion As String)

        i_modo = movimiento.iEditar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt
        CodMercancia = CodigoMercancia
        DesMercancia = Descripcion

        AsignarTooltips()
        Habilitar()
        AsignarTXT(Apuntador)

        Me.ShowDialog()

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, txtFecha, btnCantidadTC, btnPesoCaptura, btnAlmacen, btnLote)
    End Sub
    Private Sub Habilitar()
        ft.habilitarObjetos(False, True, txtCostoPrecioTotal, txtPesoTotal, txtAlmacen,
                         txtNumero, txtCodigo, txtDescripcion)
        If i_modo = movimiento.iEditar Then _
            ft.habilitarObjetos(False, True, txtFecha, cmbTipo)
    End Sub
    Private Sub IniciarTXT()

        numRenglon = "00001"
        txtCodigo.Text = CodMercancia
        txtDescripcion.Text = DesMercancia
        txtFecha.Value = jytsistema.sFechadeTrabajo
        ft.RellenaCombo(aTipo, cmbTipo)
        txtNumero.Text = Contador(MyConn, lblInfo, Gestion.iMercancías, "INVNUMMOV", "02")
        txtLote.Text = ""
        txtCantidad.Text = ft.FormatoCantidad(1.0)

        Documento = txtNumero.Text

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)

            numRenglon = .Item("asiento")
            txtCodigo.Text = CodMercancia
            txtDescripcion.Text = DesMercancia
            ft.RellenaCombo(aUnidades, cmbUnidad, ft.InArray(aUnidades, .Item("unidad")))
            txtFecha.Value = CDate(.Item("fechamov").ToString)
            ft.RellenaCombo(aTipo, cmbTipo, ft.InArray(aTipoX, .Item("tipomov")))
            txtNumero.Text = .Item("numdoc")
            txtLote.Text = .Item("lote")
            txtAlmacen.Text = .Item("almacen")
            txtCantidad.Text = ft.FormatoCantidad(.Item("cantidad"))
            txtCostoPrecio.Text = ft.FormatoNumero(.Item("costotal") / .Item("cantidad"))
            txtCostoPrecioTotal.Text = ft.FormatoNumero(.Item("costotal"))
            txtPesoTotal.Text = ft.FormatoCantidad(.Item("peso"))

            Documento = .Item("numdoc")

        End With
    End Sub

    Private Sub jsMerArcMercanciasMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsMerArcMercanciaMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dates As SfDateTimeEdit() = {txtFecha}
        SetSizeDateObjects(dates)
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus,
        txtDescripcion.GotFocus, cmbUnidad.GotFocus, txtNumero.GotFocus, txtCantidad.GotFocus,
        txtCostoPrecio.GotFocus,
         btnCantidadTC.GotFocus, btnPesoCaptura.GotFocus

        Select Case sender.name
            Case "cmbUnidad"
                ft.mensajeEtiqueta(lblInfo, "Seleccione la unidad para movimiento de mercancía ...", Transportables.tipoMensaje.iInfo)
            Case "txtLote"
                ft.mensajeEtiqueta(lblInfo, "Indique el número de lote al cual pertenece la mercancía...", Transportables.tipoMensaje.iInfo)
            Case "txtCantidad"
                ft.mensajeEtiqueta(lblInfo, "Indique la cantidad del movimiento de mercancía", Transportables.tipoMensaje.iInfo)
            Case "txtCostoPrecio"
                ft.mensajeEtiqueta(lblInfo, "Indique el costo de la mercancía... ", Transportables.tipoMensaje.iInfo)
            Case "btnCantidadTC"
                ft.mensajeEtiqueta(lblInfo, "Seleccione las cantidades por talla y color de esta mercancía...", Transportables.tipoMensaje.iInfo)
            Case "btnPesoCaptura"
                ft.mensajeEtiqueta(lblInfo, "Seleccione el peso de la mercancía desde la balanza... ", Transportables.tipoMensaje.iInfo)
            Case "btnLote"
                ft.mensajeEtiqueta(lblInfo, "Seleccione el Número de lote de inventario...", Transportables.tipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean


        If FechaUltimoBloqueo(MyConn, "jsmertramer") >= Convert.ToDateTime(txtFecha.Text) Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        Dim CantidadReal As Double = ValorCantidad(txtCantidad.Text) / IIf(Equivalencia = 0, 1, Equivalencia)
        Dim CantidadMovimiento As Double = MovimientoXDocumentoRenglonAlmacen(MyConn, txtCodigo.Text, txtNumero.Text, "INV", numRenglon, txtAlmacen.Text, lblInfo)

        If Trim(txtDescripcion.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar una descripción válida...")
            txtDescripcion.Focus()
            Return False
        End If


        If cmbUnidad.Text <> "KGR" Then
            If MultiploValido(MyConn, txtCodigo.Text, cmbUnidad.Text, ValorCantidad(txtCantidad.Text), lblInfo) Then
            Else
                ft.mensajeAdvertencia("La cantidad NO es múltiplo válido para este movimiento. Verifique por favor...")
                ft.enfocarTexto(txtCantidad)
                Return False
            End If
        End If

        If ValorNumero(txtCostoPrecio.Text) = 0.0 Then
            ft.mensajeAdvertencia("Movimiento no puede tener costo CERO (0.00). Veriqfique por favor ")
            ft.enfocarTexto(txtCostoPrecio)
            Return False
        End If

        If txtAlmacen.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar un almacén válido para este movimiento...")
            Return False
        Else
            If InStr("SA.AS", aTipoX(cmbTipo.SelectedIndex)) > 0 Then
                If CantidadReal > (ExistenciasEnAlmacenes(MyConn, txtCodigo.Text, txtAlmacen.Text) + CantidadMovimiento) Then
                    ft.mensajeCritico("Cantidad es mayor a la existente en el almacén " & txtAlmacen.Text & " ...")
                    ft.enfocarTexto(txtCantidad)
                    Return False
                End If
            End If
        End If

        Return True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dtLocal.Rows.Count
            End If

            InsertEditMERCASMovimientoInventario(MyConn, lblInfo, Insertar, txtCodigo.Text, CDate(txtFecha.Text),
                                                    aTipoX(cmbTipo.SelectedIndex), txtNumero.Text, cmbUnidad.SelectedItem,
                                                    ValorCantidad(txtCantidad.Text), ValorCantidad(txtPesoTotal.Text),
                                                    ValorNumero(txtCostoPrecioTotal.Text), ValorNumero(txtCostoPrecioTotal.Text),
                                                    "INV", txtNumero.Text, txtLote.Text, "", 0.0, 0.0, 0.0, 0.0, "", txtAlmacen.Text,
                                                    numRenglon, jytsistema.sFechadeTrabajo)

            ActualizarExistenciasPlus(MyConn, txtCodigo.Text)


            Documento = txtNumero.Text


            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Documento = ""
        Me.Close()
    End Sub

    Private Sub txt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.Click,
        txtCantidad.Click, txtCostoPrecio.Click
        Dim txt As TextBox = sender
        ft.enfocarTexto(txt)
    End Sub

    Private Sub cmbUnidad_SelectedItemChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _
        cmbUnidad.SelectedIndexChanged

        Dim afld() As String = {"codart", "id_emp"}
        Dim aStr() As String = {txtCodigo.Text, jytsistema.WorkID}

        Dim afldX() As String = {"codart", "uvalencia", "id_emp"}
        Dim aStrX() As String = {txtCodigo.Text, CType(cmbUnidad.SelectedItem, String), jytsistema.WorkID}

        If cmbUnidad.SelectedIndex > 0 Then
            Equivalencia = CDbl(qFoundAndSign(MyConn, lblInfo, "jsmerequmer", afldX, aStrX, "equivale"))
        Else
            Equivalencia = 1
        End If

        PesoActual = ValorNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "pesounidad")) / IIf(Equivalencia = 0, 1, Equivalencia)
        CostoActual = ValorNumero(qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "montoultimacompra"))

        txtCostoPrecio.Text = ft.FormatoNumero(CostoActual / IIf(Equivalencia = 0, 1, Equivalencia))

        txtDescripcion.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "nomart")

    End Sub

    Private Sub txtCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCantidad.KeyPress,
        txtCostoPrecio.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub txtCantidad_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCantidad.TextChanged,
        txtCostoPrecio.TextChanged
        txtCostoPrecioTotal.Text = ft.FormatoNumero(ValorNumero(txtCostoPrecio.Text) * ValorCantidad(txtCantidad.Text))
        txtPesoTotal.Text = ft.FormatoCantidad(PesoActual * ValorCantidad(txtCantidad.Text))
    End Sub


    Private Sub txtCodigo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigo.TextChanged
        Dim afld() As String = {"codart", "id_emp"}
        Dim aStr() As String = {txtCodigo.Text, jytsistema.WorkID}
        If qFound(MyConn, lblInfo, "jsmerctainv", afld, aStr) Then
            aUnidades = ArregloUnidades(MyConn, lblInfo, txtCodigo.Text, ft.DevuelveScalarCadena(MyConn, " select UNIDAD from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
            ft.RellenaCombo(aUnidades, cmbUnidad)
        End If

    End Sub


    Private Sub btnAlmacen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacen.Click
        Dim f As New jsControlArcAlmacenes
        f.Cargar(MyConn, TipoCargaFormulario.iShowDialog)
        txtAlmacen.Text = f.Seleccionado
        f = Nothing
    End Sub


    Private Sub btnDescripcion_Click(sender As System.Object, e As System.EventArgs) Handles btnDescripcion.Click
        If txtDescripcion.Text <> "" AndAlso txtNumero.Text <> "" Then
            Dim g As New jsGenComentariosRenglones
            Dim aFld() As String = {"numdoc", "origen", "item", "renglon", "id_emp"}
            Dim aStr() As String = {txtNumero.Text, "INV", txtCodigo.Text, "", jytsistema.WorkID}

            If qFound(MyConn, lblInfo, "jsvenrencom", aFld, aStr) Then
                g.Editar(MyConn, txtNumero.Text, "INV", txtCodigo.Text, "")
            Else
                g.Agregar(MyConn, txtNumero.Text, "INV", txtCodigo.Text, "")
            End If
            g = Nothing
        End If
    End Sub
End Class