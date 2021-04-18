Imports MySql.Data.MySqlClient
Public Class jsProArcCostosFijosMovimientos

    Private Const sModulo As String = "Movimiento Costos Fijos"

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
        txtCodigo.Text = ft.autoCodigo(MyConn, "codcosto", "jsfabcatfij", "id_emp", jytsistema.WorkID, 5)
        txtNombre.Text = ""
        txtPorcentaje.Text = ft.muestraCampoNumero(0.0)
        txtImporte.Text = ft.muestraCampoNumero(0.0)
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
            txtCodigo.Text = ft.muestraCampoTexto(.Item("codcosto"))
            txtNombre.Text = ft.muestraCampoTexto(.Item("titulo"))
            txtPorcentaje.Text = ft.muestraCampoNumero(.Item("porcentaje"))
            txtImporte.Text = ft.muestraCampoNumero(.Item("montofijo"))
        End With
    End Sub
    Private Sub jsProArcCostosFijosMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsProArcCostosFijosMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtNombre.GotFocus, txtPorcentaje.GotFocus, txtImporte.GotFocus
        Select Case sender.name
            Case "txtCodigo"
                ft.mensajeEtiqueta(lblInfo, "Indique código costo fijo", Transportables.tipoMensaje.iAyuda)
            Case "txtNombre"
                ft.mensajeEtiqueta(lblInfo, "Indique nombre o descripción de costo fijo", Transportables.tipoMensaje.iAyuda)
            Case "txtPorcentaje"
                ft.mensajeEtiqueta(lblInfo, "Indique porcentaje sobre al costo total de este costo fijo", Transportables.tipoMensaje.iAyuda)
            Case "txtImporte"
                ft.mensajeEtiqueta(lblInfo, "Si no indicó porcentaje, indique monto de costo fijo", Transportables.tipoMensaje.iAyuda)
        End Select
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        'If txtNombre.Text.Trim() = "" Then
        '    ft.mensajeAdvertencia("Debe indicar un nombre o descripción válido...")
        '    txtNombre.Focus()
        '    Exit Function
        'Else
        '    If NumeroDeRegistrosEnTabla(MyConn, "jsmercatalm", " desalm = '" & txtNombre.Text.Trim() & "' ") > 0 AndAlso _
        '       i_modo = movimiento.iAgregar Then
        '        ft.MensajeCritico("DESCRIPCION EXISTENTE. Indique descripción válida...")
        '        ft.enfocarTexto(txtNombre)
        '        Exit Function
        '    End If

        'End If

        Validado = True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dtLocal.Rows.Count
            End If
            InsertEditPRODUCCIONCostoFijo(MyConn, lblInfo, Insertar, txtCodigo.Text, txtNombre.Text, ValorNumero(txtPorcentaje.Text), ValorNumero(txtImporte.Text))
            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class