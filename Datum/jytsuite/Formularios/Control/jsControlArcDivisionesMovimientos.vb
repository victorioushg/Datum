Imports MySql.Data.MySqlClient
Public Class jsControlArcDivisionesMovimientos

    Private Const sModulo As String = "Movimiento División "
    Private Const nTabla As String = "tblDivisiones"

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

        txtCodigo.Text = ft.autoCodigo(MyConn, "division", "jsmercatdiv", "id_emp", jytsistema.WorkID, 5)
        txtNombre.Text = ""
        txtColor.BackColor = Color.White
        txtColor.Text = Color.White.R.ToString + ", " + Color.White.G.ToString + ", " + Color.White.B.ToString

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
            txtCodigo.Text = .Rows(nPosicion).Item("division")
            txtNombre.Text = .Rows(nPosicion).Item("descrip")
            txtColor.Text = .Rows(nPosicion).Item("color")
            If Split(txtColor.Text, ",").Length = 3 Then
                txtColor.BackColor = Color.FromArgb(Split(txtColor.Text, ",").GetValue(0), _
                                                                Split(txtColor.Text, ",").GetValue(1), _
                                                                Split(txtColor.Text, ",").GetValue(2))
            Else
                txtColor.BackColor = Color.White
            End If

        End With
    End Sub
    Private Sub jsControlDivisionesMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jiConTarjetasovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el código de división ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtNombre.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre válido...")
            txtNombre.Focus()
            Exit Function
        Else

            If NumeroDeRegistrosEnTabla(MyConn, "jsmercatdiv", " descrip = '" & txtNombre.Text.Trim() & "' ") > 0 AndAlso _
               i_modo = movimiento.iAgregar Then
                ft.MensajeCritico("DESCRIPCION EXISTENTE. Indique descripción válida...")
                ft.enfocarTexto(txtNombre)
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
            InsertEditMERCASDivision(MyConn, lblInfo, Insertar, txtCodigo.Text, txtNombre.Text, txtColor.Text)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txtComision_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtColor.Click, _
        txtColor.GotFocus
        Dim txt As TextBox = sender
        ft.enfocarTexto(txt)
    End Sub

    Private Sub btnColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnColor.Click
        With dlgColour
            .Color = txtColor.BackColor
            .ShowDialog()
            txtColor.BackColor = .Color
            txtColor.Text = .Color.R.ToString + ", " + .Color.G.ToString + ", " + .Color.B.ToString
        End With

    End Sub
End Class