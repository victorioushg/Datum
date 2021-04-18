Imports MySql.Data.MySqlClient
Public Class jsControlArcRubrosSADAMovimientos

    Private Const sModulo As String = "Movimiento Rubros SADA"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Apuntador As Long
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
       
        txtCodigo.Text = ft.autoCodigo(MyConn, "CODIGO", "jsmercodsica", "", "", 3)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNombre, txtCategoria)
        ft.iniciarTextoObjetos(FormatoItemListView.iEntero, txtCantidad, txtFrecuencia)

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
        With dtLocal
            txtCodigo.Text = .Rows(nPosicion).Item("codigo")
            txtNombre.Text = .Rows(nPosicion).Item("rubro")
            txtCantidad.Text = ft.FormatoEntero(.Rows(nPosicion).Item("cantidad"))
            txtFrecuencia.Text = ft.FormatoEntero(.Rows(nPosicion).Item("frecuencia"))
            txtCategoria.Text = .Rows(nPosicion).Item("categoria")
            If txtCategoria.Text = "" Then Label6.Text = ""
        End With
        txtCodigo.Enabled = False
    End Sub
    Private Sub jsControlDivisionesMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
        ft = Nothing
    End Sub

    Private Sub jiConTarjetasovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el código de RUBRO ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If i_modo = MovAud.iIncluir AndAlso ft.DevuelveScalarCadena(MyConn, " select codigo from jsmercodsica where codigo = '" & txtCodigo.Text & "'  ") <> "" Then
            ft.mensajeCritico("CODIGO DE RUBRO YA EXISTE. VERIFIQUE POR FAVOR...")
            ft.enfocarTexto(txtCodigo)
            Exit Function
        End If

        If Trim(txtNombre.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre o descripción válido...")
            txtNombre.Focus()
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
            ft.Ejecutar_strSQL(myconn, " REPLACE jsmercodsica values('" & txtCodigo.Text & "', '" & txtNombre.Text & "'," & ValorEntero(txtCantidad.Text) & "," & ValorEntero(txtFrecuencia.Text) & ",'" & txtCategoria.Text & "') ")
            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub



    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombre.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el nombre o descripción del RUBRO SADA ...", Transportables.TipoMensaje.iInfo)
    End Sub
    Private Sub txtCantidad_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCantidad.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique la cantidad a vender por cliente en unidades ...", Transportables.TipoMensaje.iInfo)
    End Sub
    Private Sub txtFrecuencia_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFrecuencia.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique la Frecuencia de venta para este producto por cliente en dias ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub btnCategoriaSADA_Click(sender As System.Object, e As System.EventArgs) Handles btnCategoriaSADA.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("CATEGORIAS SADA", FormatoTablaSimple(Modulo.iMER_CategoriaSADA), True, TipoCargaFormulario.iShowDialog)
        txtCategoria.Text = f.Seleccion
        f.Dispose()
        f = Nothing
    End Sub

    Private Sub txtCategoria_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtCategoria.TextChanged
        Dim nomCategoria As String = ft.DevuelveScalarCadena(MyConn, " select descrip from jsconctatab " _
                                                             & " where " _
                                                             & " codigo = '" & txtCategoria.Text & "' AND " _
                                                             & " modulo = '" & FormatoTablaSimple(Modulo.iMER_CategoriaSADA) & "' and " _
                                                             & " id_emp = '" & jytsistema.WorkID & "' ").ToString
        If nomCategoria = "" Then
            Label6.Text = ""
        Else
            Label6.Text = nomCategoria
        End If
    End Sub

End Class