Imports MySql.Data.MySqlClient
Public Class jsControlArcTransportesMovimientos

    Private Const sModulo As String = "Movimientos Transportes"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long
    Private CreditoDebito As Integer
    Private aTipo() As String = {"Carro", "Camioneta", "Camión", "Gandola"}
    Private aCombustible() As String = {"Gasoil", "Gasolina 91 cp", "Gasolina 95 sp"}

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable)
        i_modo = movimiento.iAgregar

        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        IniciarTXT()
        Me.ShowDialog()
    End Sub

    Private Sub IniciarTXT()
        txtCodigo.Text = ft.autoCodigo(MyConn, "codtra", "jsconctatra", "id_emp", jytsistema.WorkID, 5)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNombre, txtChofer, txtAcompañante, txtMarca, txtColor, txtPlacas, txtSerialMotor, txtSerialCarro, _
                            txtSerialCava, txtAño)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtCap, txtAutonomia, txtTanque, txtValorFinal, txtValorinicial)

        txtPuestos.Text = "0"
        txtAdquisicion.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)

        ft.RellenaCombo(aCombustible, cmbCombustible)
        ft.RellenaCombo(aTipo, cmbVehiculo)
        ft.habilitarObjetos(False, True, txtCodigo, txtAdquisicion)

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable)
        i_modo = movimiento.iEditar

        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)

            txtCodigo.Text = ft.muestraCampoTexto(.Item("codtra"))
            txtNombre.Text = ft.muestraCampoTexto(.Item("nomtra"))
            txtChofer.Text = ft.muestraCampoTexto(.Item("chofer"))
            txtAcompañante.Text = ft.muestraCampoTexto(.Item("acompa"))
            txtCap.Text = ft.FormatoNumero(.Item("capacidad"))
            txtPuestos.Text = ft.FormatoEntero(.Item("puestos"))
            txtMarca.Text = ft.muestraCampoTexto(.Item("marca"))
            txtColor.Text = ft.muestraCampoTexto(.Item("color"))
            txtPlacas.Text = ft.muestraCampoTexto(.Item("placas"))
            txtSerialMotor.Text = ft.muestraCampoTexto(.Item("serial1"))
            txtSerialCarro.Text = ft.muestraCampoTexto(.Item("serial2"))
            txtSerialCava.Text = ft.muestraCampoTexto(.Item("serial3"))
            txtAño.Text = ft.muestraCampoTexto(.Item("modelo"))
            txtAutonomia.Text = ft.FormatoNumero(.Item("autono"))
            txtTanque.Text = ft.FormatoNumero(.Item("captanque"))
            txtAdquisicion.Text = ft.FormatoFecha(CDate(.Item("fechadq").ToString))
            txtValorinicial.Text = ft.FormatoNumero(.Item("valorini"))
            txtValorFinal.Text = ft.FormatoNumero(.Item("valorfin"))

            ft.RellenaCombo(aCombustible, cmbCombustible, .Item("tipocon"))
            ft.RellenaCombo(aTipo, cmbVehiculo, .Item("tipo"))

            ft.habilitarObjetos(False, True, txtCodigo, txtAdquisicion)

        End With
    End Sub
    Private Sub jsControlTransportesMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jiConTarjetasovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtNombre.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre o descripción válido...")
            ft.enfocarTexto(txtNombre)
            Exit Function
        End If

        If Trim(txtChofer.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre de chofer válido")
            ft.enfocarTexto(txtChofer)
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
            InsertEditCONTROLTransportes(MyConn, lblInfo, Insertar, txtCodigo.Text, txtNombre.Text, txtChofer.Text, txtAcompañante.Text, _
                                         txtCap.Text, cmbVehiculo.SelectedIndex, ValorEntero(txtPuestos.Text), txtMarca.Text, _
                                         txtColor.Text, txtPlacas.Text, txtSerialMotor.Text, txtSerialCarro.Text, txtSerialCava.Text, _
                                         ValorNumero(txtAutonomia.Text), cmbCombustible.SelectedIndex, ValorNumero(txtTanque.Text), _
                                         txtAño.Text, ValorNumero(txtValorinicial.Text), "", ValorNumero(txtValorFinal.Text), _
                                         CDate(txtAdquisicion.Text))

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombre.GotFocus, txtCodigo.GotFocus, _
        txtChofer.GotFocus, txtAcompañante.GotFocus, txtCap.GotFocus, txtPuestos.GotFocus, txtMarca.GotFocus, txtColor.GotFocus, _
        txtPlacas.GotFocus, txtSerialCarro.GotFocus, txtSerialCava.GotFocus, txtSerialMotor.GotFocus, txtAño.GotFocus, _
        txtAutonomia.GotFocus, txtTanque.GotFocus, btnAdquisicion.GotFocus, txtValorFinal.GotFocus, txtValorinicial.GotFocus
        Select Case sender.name
            Case "txtCodigo"
                ft.mensajeEtiqueta(lblInfo, "   Indique código de TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.maxLength = 5
            Case "txtNombre"
                ft.mensajeEtiqueta(lblInfo, "   Indique nombre de TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 50
            Case "txtChofer"
                ft.mensajeEtiqueta(lblInfo, "   Indique NOMBRE de CONDUCTOR DEL TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 50
            Case "txtAcompañante"
                ft.mensajeEtiqueta(lblInfo, "   Indique nombre de ACOMAPAÑANTE DEL TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 50
            Case "txtCap"
                ft.mensajeEtiqueta(lblInfo, "   Indique  LA CAPACIDAD DEL TRANSPORTE (EN Kilogramos)  ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 8
            Case "txtPuestos"
                ft.mensajeEtiqueta(lblInfo, "   Indique NUMERO DE PUESTOS DEL TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 2
            Case "txtMarca"
                ft.mensajeEtiqueta(lblInfo, "   Indique LA MARCA DEL TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 15
            Case "txtColor"
                ft.mensajeEtiqueta(lblInfo, "   Indique EL COLOR DEL TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 15
            Case "txtPlacas"
                ft.mensajeEtiqueta(lblInfo, "   Indique PLACA DEL TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 10
            Case "txtSerialMotor"
                ft.mensajeEtiqueta(lblInfo, "   Indique SERIAL MOTOR DE TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 25
            Case "txtSerialCarro"
                ft.mensajeEtiqueta(lblInfo, "   Indique SERIAL CARROCERIA TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 25
            Case "txtSerialCava"
                ft.mensajeEtiqueta(lblInfo, "   Indique SERIAL CAVA TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 25
            Case "txtAño"
                ft.mensajeEtiqueta(lblInfo, "   Indique MODELO O AÑO DEL TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 5
            Case "txtAutonomia"
                ft.mensajeEtiqueta(lblInfo, "   Indique AUTONOMIA EN KILOMETROS DEL TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 6
            Case "txtTanque"
                ft.mensajeEtiqueta(lblInfo, "   Indique CAPACIDAD DE COMBUSTIBLE EN LITROS DE TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 5
            Case "btnAdquisicion"
                ft.mensajeEtiqueta(lblInfo, "   Indique FECHA DE ADQUISICION TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)

            Case "txtValorinicial"
                ft.mensajeEtiqueta(lblInfo, "   Indique VALOR INICIAL TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 15
            Case "txtValorFinal"
                ft.mensajeEtiqueta(lblInfo, "   Indique VALOR ACTUAL TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 15
            Case "txtCodcon"
                ft.mensajeEtiqueta(lblInfo, "   Indique CODIGO CONTABLE TRANSPORTE ... ", Transportables.TipoMensaje.iInfo)
                sender.MaxLength = 20
        End Select

    End Sub

    Private Sub txtCap_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCap.KeyPress, _
        txtAutonomia.KeyPress, txtTanque.KeyPress, txtPuestos.KeyPress, txtValorFinal.KeyPress, txtValorinicial.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

End Class