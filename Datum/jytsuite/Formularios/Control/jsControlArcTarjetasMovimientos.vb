Imports MySql.Data.MySqlClient
Public Class jsControlArcTarjetasMovimientos

    Private Const sModulo As String = "Movimiento tarjeta crédito/débito "
    Private Const nTabla As String = "tarjetas"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private aTipo() As String = {"Crédito", "Débito"}
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
        txtCodigo.Text = ft.autoCodigo(MyConn, "codtar", "jsconctatar", "id_emp", jytsistema.WorkID, 5)
        txtNombre.Text = ""
        txtComision.Text = ft.FormatoNumero(0.0)
        txtImpuesto.Text = ft.FormatoNumero(0.0)
        ft.RellenaCombo(aTipo, cmbTipo, 0)
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
            txtCodigo.Text = .Rows(nPosicion).Item("codtar")
            txtNombre.Text = .Rows(nPosicion).Item("nomtar")
            txtComision.Text = ft.FormatoNumero(.Rows(nPosicion).Item("com1"))
            txtImpuesto.Text = ft.FormatoNumero(.Rows(nPosicion).Item("com2"))
            ft.RellenaCombo(aTipo, cmbTipo, .Rows(nPosicion).Item("tipo"))
        End With
    End Sub
    Private Sub jsControlTarjetasMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jiConTarjetasovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el código de tarjeta ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtNombre.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre válido...")
            txtNombre.Focus()
            Exit Function
        Else

            If NumeroDeRegistrosEnTabla(MyConn, "jsconctatar", " nomtar = '" & txtNombre.Text.Trim() & "' ") > 0 AndAlso _
               i_modo = movimiento.iAgregar Then
                ft.mensajeCritico(" DESCRIPCION EXISTENTE. Indique descripción válida...")
                ft.enfocarTexto(txtNombre)
                Exit Function
            End If

        End If


        If Not ft.isNumeric(txtComision.Text) Then
            ft.mensajeAdvertencia("Debe indicar una comisión válida...")
            ft.enfocarTexto(txtComision)
            Exit Function
        End If

        If Not ft.isNumeric(txtImpuesto.Text) Then
            ft.mensajeAdvertencia("Debe indicar un impuesto válido ...")
            ft.enfocarTexto(txtImpuesto)
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
            InsertEditCONTROLTarjeta(MyConn, lblInfo, Insertar, txtCodigo.Text, txtNombre.Text, ValorNumero(txtComision.Text), ValorNumero(txtImpuesto.Text), _
                 cmbTipo.SelectedIndex)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtComision_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComision.Click, txtImpuesto.Click, _
        txtComision.GotFocus, txtImpuesto.GotFocus
        Dim txt As TextBox = sender
        ft.enfocarTexto(txt)
    End Sub

End Class