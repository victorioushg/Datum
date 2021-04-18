Imports MySql.Data.MySqlClient
Public Class jsMerArcComponentesComboMovimiento

    Private Const sModulo As String = "Movimiento componente de combo de mercancías "
    Private Const nTabla As String = "tblComp"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable

    Private i_modo As Integer
    Private nPosicion As Integer

    Private aUnidades() As String = {}
    Private UltimoCosto As Double = 0.0
    Private PesoUnidad As Double = 0.0
    Private Equivalencia As Double = 0.0

    Private CodigoMercancia As String = ""

    Private es_Servicio As Boolean = False

    Private n_Apuntador As Long
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal CodigoArticulo As String, Optional Servicio As Boolean = False)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoMercancia = CodigoArticulo
        es_Servicio = Servicio
        IniciarTXT()
        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()

        txtCodigo.Text = ""
        txtNombre.Text = ""
        txtCosto.Text = FormatoNumero(0.0)
        txtCantidad.Text = FormatoCantidad(1.0)
        txtPeso.Text = "0.000"
        cmbUnidad.Items.Clear()

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal CodigoArticulo As String, Optional Servicio As Boolean = False)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoMercancia = CodigoArticulo
        es_Servicio = Servicio
        AsignarTXT(Apuntador)
        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        HabilitarObjetos(False, True, btnMercancia, txtCodigo)
        With dtLocal.Rows(nPosicion)
            txtCodigo.Text = .Item("codartcom")
            txtNombre.Text = .Item("descrip")
            txtCantidad.Text = FormatoCantidad(.Item("cantidad"))
            txtCosto.Text = FormatoNumero(.Item("costo"))
            txtPeso.Text = FormatoCantidad(.Item("peso"))

            cmbUnidad.SelectedIndex = InArray(aUnidades, .Item("unidad")) - 1

        End With
    End Sub
    Private Sub jsMerArcComponentesComboMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsMerArcComponentesComboMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
        HabilitarObjetos(False, True, txtCosto, txtPeso, txtNombre)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        MensajeEtiqueta(lblInfo, "Indique el código de tarjeta ...", TipoMensaje.iInfo)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtNombre.Text) = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar un nombre válido...")
            txtNombre.Focus()
            Exit Function
        End If

        If Not IsNumeric(txtCantidad.Text) Then
            MensajeAdvertencia(lblInfo, "Debe indicar una cantidad válida ...")
            EnfocarTexto(txtCantidad)
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
            InsertEditMERCASComponenteCombo(MyConn, lblInfo, Insertar, CodigoMercancia, txtCodigo.Text, _
                                             txtNombre.Text, ValorCantidad(txtCantidad.Text), _
                                             cmbUnidad.Text, ValorNumero(txtCosto.Text), ValorCantidad(txtPeso.Text))

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtComision_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCosto.Click, txtCantidad.Click, _
        txtCosto.GotFocus, txtCantidad.GotFocus
        '        Dim txt As TextBox = sender
        '        EnfocarTexto(txt)
    End Sub

    Private Sub txtCodigo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigo.TextChanged

        Dim afld() As String = {"codart", "id_emp"}
        Dim aStr() As String = {txtCodigo.Text, jytsistema.WorkID}
        If txtCodigo.Text.Substring(0, 1) = "$" Then
            txtNombre.Text = EjecutarSTRSQL_ScalarPLUS(MyConn, " select desser from jsmercatser where codser = '" & txtCodigo.Text.Substring(1) & "' and id_emp = '" & jytsistema.WorkID & "' ")
            aUnidades = {"UND"}
            UltimoCosto = EjecutarSTRSQL_ScalarPLUS(MyConn, " select precio from jsmercatser where codser = '" & txtCodigo.Text.Substring(1) & "' and id_emp = '" & jytsistema.WorkID & "' ")
            PesoUnidad = 0.0
            RellenaCombo(aUnidades, cmbUnidad)
        Else
            If qFound(MyConn, lblInfo, "jsmerctainv", afld, aStr) Then
                aUnidades = ArregloUnidades(MyConn, lblInfo, txtCodigo.Text, CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select UNIDAD from jsmerctainv where codart = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")))
                txtNombre.Text = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "nomart")
                UltimoCosto = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "montoultimacompra")
                PesoUnidad = qFoundAndSign(MyConn, lblInfo, "jsmerctainv", afld, aStr, "pesounidad")
                RellenaCombo(aUnidades, cmbUnidad)
            Else
                txtNombre.Text = ""
                UltimoCosto = 0.0
                PesoUnidad = 0.0
                cmbUnidad.Items.Clear()
            End If
        End If
    End Sub

    Private Sub cmbUnidad_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbUnidad.SelectedIndexChanged, _
        txtCantidad.TextChanged

        Dim afldX() As String = {"codart", "uvalencia", "id_emp"}
        Dim aStrX() As String = {txtCodigo.Text, CType(cmbUnidad.SelectedItem, String), jytsistema.WorkID}

        If cmbUnidad.SelectedIndex > 0 Then
            Equivalencia = CDbl(qFoundAndSign(MyConn, lblInfo, "jsmerequmer", afldX, aStrX, "equivale"))
        Else
            Equivalencia = 1
        End If

        txtCosto.Text = FormatoNumero(ValorCantidad(txtCantidad.Text) * UltimoCosto / IIf(Equivalencia = 0, 1, Equivalencia))
        txtPeso.Text = FormatoCantidad(ValorCantidad(txtCantidad.Text) * PesoUnidad / IIf(Equivalencia = 0, 1, Equivalencia))

    End Sub

    Private Sub txtCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCantidad.KeyPress
        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub

    Private Sub btnMercancia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMercancia.Click
        If es_Servicio Then
            Dim f As New jsMerArcServiciosA
            f.Cargar(MyConn)
            txtCodigo.Text = f.Seleccionado
            f = Nothing
        Else
            Dim f As New jsMerArcListaCostosPreciosPlus
            f.Cargar(MyConn, TipoListaPrecios.Costos, "")
            txtCodigo.Text = f.Seleccionado
            f = Nothing
        End If
        
    End Sub
End Class