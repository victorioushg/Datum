Imports MySql.Data.MySqlClient
Public Class jsVenArcRutasVisitaMovimientos

    Private Const sModulo As String = "Movimiento de ruta de visita"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long

    Private CodigoRuta As String
    Private Condicion As Integer ' CONDICION REPRESENTA EL TIPO DE RUTA 0 = EXLUSIVA; 1 = CONCURRENTE 
    Private Tipo As Integer ' REPRESENTA LA CLASE DE RUTA 0 = VISITA, 1 = DESPACHO
    Private NumeroAnterior As Integer
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property

    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal iCodRuta As String, _
                       ByVal iTipo As Integer, ByVal ClientesAsignadosARuta As Integer)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoRuta = iCodRuta
        Tipo = iTipo
        Condicion = ClientesAsignadosARuta

        IniciarTXT()
        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()

        txtNumero.Text = ft.FormatoEntero(ft.autoCodigoNumero(MyConn, _
                          "numero", "jsvenrenrut", "codrut.tipo.condicion.id_emp", _
                          CodigoRuta + ".0." + Condicion.ToString + "." + jytsistema.WorkID))

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtCodigo, txtNombreCliente)
        NumeroAnterior = ValorEntero(txtNumero.Text)

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, ByVal iCodRuta As String, _
                      ByVal iTipo As Integer, ByVal iCondicion As Integer)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        Tipo = iTipo

        CodigoRuta = iCodRuta
        Condicion = iCondicion
        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal.Rows(nPosicion)
            txtNumero.Text = .Item("numero")
            txtCodigo.Text = ft.MuestraCampoTexto(.Item("cliente"))
            txtNombreCliente.Text = ft.MuestraCampoTexto(.Item("nomcli"))
            NumeroAnterior = .Item("numero")
        End With
    End Sub

    Private Sub jsVenArcRutasdVisitaMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
        ft = Nothing
    End Sub

    Private Sub jsVenArcRutasVisitaMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnOK.Image = ImageList1.Images(0)
        btnCancel.Image = ImageList1.Images(1)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el código del cliente ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub txtDescripcion_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombreCliente.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el nombre o descripción ...", Transportables.TipoMensaje.iInfo)
    End Sub
    Private Sub txtNumero_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNumero.TextChanged
        ft.mensajeEtiqueta(lblInfo, "Indique el número de precedencia en la ruta de este cliente ...", Transportables.TipoMensaje.iInfo)
    End Sub
    Private Function Validado() As Boolean
        Validado = False

        If i_modo = movimiento.iAgregar AndAlso txtNumero.Text = "" Then
            ft.mensajeEtiqueta(lblInfo, "Indique el nombre o descripción ...", Transportables.TipoMensaje.iInfo)
            ft.enfocarTexto(txtNumero)
            Exit Function
        End If

        If Trim(txtNombreCliente.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar o escoger un cliente válido ...")
            ft.enfocarTexto(txtCodigo)
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
                IncluyendoRenglonRuta(MyConn, lblInfo, ValorEntero(txtNumero.Text), CodigoRuta, Tipo, "", Condicion)
            Else
                ModificandoRenglonRuta(MyConn, lblInfo, NumeroAnterior, ValorEntero(txtNumero.Text), CodigoRuta, Tipo, "", Condicion)
            End If
            InsertEditVENTASRenglonRuta(MyConn, lblInfo, Insertar, CodigoRuta, txtNumero.Text, txtCodigo.Text, _
                                        txtNombreCliente.Text, Tipo, "1", "", Condicion)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnClientes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClientes.Click
        
        txtCodigo.Text = CargarTablaSimple(MyConn, lblInfo, dsLocal, _
                                                " SELECT a.codcli codigo, a.nombre descripcion, b.condicion " _
                                              & " FROM jsvencatcli a " _
                                              & " LEFT JOIN jsvenrenrut b ON (a.codcli = b.cliente " _
                                              & "                               AND a.id_emp = b.id_emp " _
                                              & "                           	AND b.tipo = " & Tipo & " " _
                                              & " 	                            AND b.division = ''  " _
                                              & " 	                            AND b.condicion = " & Condicion & ") " _
                                              & " WHERE " _
                                              & " b.condicion IS NULL AND " _
                                              & " a.id_emp = '" & jytsistema.WorkID & "' ORDER BY codcli ", _
                                            IIf(Condicion = 0, "Clientes No Asignados a Rutas", "Clientes"), txtCodigo.Text)

    End Sub

    Private Sub txtCodigo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigo.TextChanged
        txtNombreCliente.Text = ft.DevuelveScalarCadena(MyConn, " select nombre from jsvencatcli where codcli = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub
End Class