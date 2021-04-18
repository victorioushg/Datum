Imports MySql.Data.MySqlClient
Public Class jsMerArcEnvasesMovimientos

    Private Const sModulo As String = "Movimiento de envases"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer

    Private nPosicion As Integer
    Private n_Apuntador As Long
    Private nDocumento As String

    Private aTipo() As String = {"Entrada", "Salida"}
    Private aTipoX() As String = {"EN", "SA"}

    Private codEnvase As String
    Private descripcionEnvase As String
    Private numRenglon As String
    Private SerialInterno As String
    Private SerialExterno As String
    Private CodigoArticulo As String

    Private aEstatus() As String = {"En Tránsito", "En Cliente", "En Proveedor", "Vacío/Depósito", "Lleno/Depósito", _
                                    "Por Reparación", "Desincorporado", "Indeterminado"}

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

    Public Sub Agregar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                       ByVal CodigoEnvase As String, ByVal Descripcion As String)

        i_modo = movimiento.iAgregar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt
        codEnvase = CodigoEnvase
        descripcionEnvase = Descripcion

        AsignarTooltips()
        Habilitar()
        IniciarTXT()

        Me.ShowDialog()

    End Sub
    Public Sub Editar(ByVal Mycon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                       ByVal CodigoEnvase As String, ByVal Descripcion As String)

        i_modo = movimiento.iEditar
        MyConn = Mycon
        dsLocal = ds
        dtLocal = dt
        codEnvase = CodigoEnvase
        descripcionEnvase = Descripcion

        AsignarTooltips()
        Habilitar()
        AsignarTXT(Apuntador)

        Me.ShowDialog()

    End Sub
    Private Sub AsignarTooltips()
        ft.colocaToolTip(C1SuperTooltip1, btnLote, btnAlmacen, btnFecha)
    End Sub
    Private Sub Habilitar()
        ft.habilitarObjetos(False, True, txtFecha, txtAlmacen, txtNumero, txtCodigo, txtDescripcion)
        If i_modo = movimiento.iEditar Then ft.habilitarObjetos(False, True, btnFecha, cmbTipo, cmbEstatus)
    End Sub
    Private Sub IniciarTXT()

        numRenglon = "00001"
        txtCodigo.Text = codEnvase
        txtDescripcion.Text = descripcionEnvase
        txtFecha.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        ft.RellenaCombo(aTipo, cmbTipo)
        txtNumero.Text = Contador(MyConn, lblInfo, Gestion.iMercancías, "INVNUMMOVENV", "05")
        txtLote.Text = ""
        txtCantidad.Text = ft.FormatoEntero(1)
        ft.RellenaCombo(aEstatus, cmbEstatus, 3)
        Documento = txtNumero.Text

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)

            numRenglon = .Item("renglon")
            txtCodigo.Text = codEnvase
            txtDescripcion.Text = descripcionEnvase

            txtFecha.Text = ft.muestraCampoFecha(.Item("fechamov"))
            ft.RellenaCombo(aTipo, cmbTipo, ft.InArray(aTipoX, .Item("tipomov")))
            txtNumero.Text = ft.muestraCampoTexto(.Item("numdoc"))

            txtAlmacen.Text = ft.muestraCampoTexto(.Item("almacen"))
            txtCantidad.Text = ft.muestraCampoEntero(.Item("cantidad"))

            ft.RellenaCombo(aEstatus, cmbEstatus, .Item("ESTATUS"))

            Documento = .Item("numdoc")

        End With
    End Sub

    Private Sub jsMerArcEnvasesMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsMerArcEnvasesMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
        ft.visualizarObjetos(False, lblLote, txtLote, btnLote)
        CodigoArticulo = ft.DevuelveScalarCadena(MyConn, " select CODIGO_CONTENIDO from jsmercatenv where codenv = '" & codEnvase & "' and id_emp = '" & jytsistema.WorkID & "' ")
        SerialInterno = ft.DevuelveScalarCadena(MyConn, " select SERIAL_1 from jsmercatenv where codenv = '" & codEnvase & "' and id_emp = '" & jytsistema.WorkID & "' ")
        SerialExterno = ft.DevuelveScalarCadena(MyConn, " select SERIAL_2 from jsmercatenv where codenv = '" & codEnvase & "' and id_emp = '" & jytsistema.WorkID & "' ")


    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtDescripcion.GotFocus, txtFecha.GotFocus, txtNumero.GotFocus, txtCantidad.GotFocus, btnFecha.GotFocus

        Select Case sender.name
            Case "cmbUnidad"
                ft.mensajeEtiqueta(lblInfo, "Seleccione la unidad para movimiento de mercancía ...", Transportables.tipoMensaje.iInfo)
            Case "txtLote"
                ft.mensajeEtiqueta(lblInfo, "Indique el número de lote al cual pertenece la mercancía...", Transportables.tipoMensaje.iInfo)
            Case "txtCantidad"
                ft.mensajeEtiqueta(lblInfo, "Indique la cantidad del movimiento de mercancía", Transportables.tipoMensaje.iInfo)
            Case "btnLote"
                ft.mensajeEtiqueta(lblInfo, "Seleccione el Número de lote de inventario...", Transportables.tipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean


        If Trim(txtDescripcion.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar una descripción válida...")
            ft.enfocarTexto(txtDescripcion)
            Return False
        End If

        If txtAlmacen.Text = "" Then
            ft.mensajeAdvertencia("Debe indicar un almacén válido para este movimiento...")
            Return False
        Else
            If InStr("SA.AS", aTipoX(cmbTipo.SelectedIndex)) > 0 Then
                'If CantidadReal > (ExistenciasEnAlmacenes(MyConn, txtCodigo.Text, txtAlmacen.Text) + CantidadMovimiento) Then
                '    ft.mensajeCritico("Cantidad es mayor a la existente en el almacén " & txtAlmacen.Text & " ...")
                '    ft.enfocarTexto(txtCantidad)
                '    Exit Function
                'End If
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

            InsertEditMERCASEnvaseMovimiento(MyConn, lblInfo, Insertar, txtCodigo.Text, SerialInterno, SerialExterno,
                                              CodigoArticulo, numRenglon, Convert.ToDateTime(txtFecha.Text),
                                              aTipoX(cmbTipo.SelectedIndex), txtNumero.Text, Convert.ToInt32(txtCantidad.Text),
                                              "INV", "", "", "", txtAlmacen.Text, cmbEstatus.SelectedIndex)

            Documento = txtNumero.Text

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()

        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Documento = ""
        Me.Close()
    End Sub

    Private Sub txt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.Click, _
        txtCantidad.Click
        Dim txt As TextBox = sender
        ft.enfocarTexto(txt)
    End Sub

    
    Private Sub txtCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCantidad.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub btnAlmacen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacen.Click
        Dim f As New jsControlArcAlmacenes
        f.Cargar(MyConn, TipoCargaFormulario.iShowDialog)
        txtAlmacen.Text = f.Seleccionado
        f = Nothing
    End Sub

    Private Sub btnFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFecha.Click
        txtFecha.Text = SeleccionaFecha(CDate(txtFecha.Text), Me, btnFecha)
    End Sub

End Class