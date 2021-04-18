Imports MySql.Data.MySqlClient
Public Class jsPOSArcSupervisorMovimientos

    Private Const sModulo As String = "Movimiento supervisor"
    Private Const nTabla As String = "superpos"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private i_modo As Integer

    Private nPosicion As Integer
    Private n_Apuntador As Long
    Private aNivelSup() As String = {"Supervisor de Piso", "Gerente de Piso", "Gerente General"}

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

        txtCodigo.Text = ft.autoCodigo(MyConn, "codigo", "jsvencatsup", "id_emp", jytsistema.WorkID, 5)
        txtNombre.Text = ""
        ft.RellenaCombo(aNivelSup, cmbTipo)
        txtPassword0.Text = ""
        txtPassword1.Text = ""
        txtDescrip.Text = ""

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
            txtCodigo.Text = .Item("codigo")
            txtNombre.Text = .Item("nombre")
            txtDescrip.Text = .Item("descrip")
            ft.RellenaCombo(aNivelSup, cmbTipo, .Item("nivel"))
            txtPassword0.Text = .Item("clave")
            txtPassword1.Text = .Item("clave")
        End With
    End Sub
    Private Sub jsPOSArcCajerosMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
        ft = Nothing
    End Sub

    Private Sub jsPOSArcCajerosMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ft.habilitarObjetos(False, True, txtCodigo)
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtNombre.GotFocus, txtDescrip.GotFocus, txtPassword0.GotFocus, txtPassword1.GotFocus
        Select Case sender.name
            Case "txtCodigo"
                ft.mensajeEtiqueta(lblInfo, "Indique el código supervisor ...", Transportables.TipoMensaje.iInfo)
            Case "txtNombre"
                ft.mensajeEtiqueta(lblInfo, "Indique el nombre supervisor  ...", Transportables.TipoMensaje.iInfo)
            Case "txtDescrip"
                ft.mensajeEtiqueta(lblInfo, "Indique descripción ...", Transportables.TipoMensaje.iInfo)
            Case "txtPassword0"
                ft.mensajeEtiqueta(lblInfo, "Indique una clave válida para este cajero... ", Transportables.TipoMensaje.iInfo)
        End Select

    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtNombre.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre válido...")
            ft.enfocarTexto(txtNombre)
            Exit Function
        End If

        If Trim(txtDescrip.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar una descripción ...")
            ft.enfocarTexto(txtNombre)
            Exit Function
        End If

        If txtPassword0.Text <> txtPassword1.Text Then
            ft.mensajeAdvertencia("Debe indicar una clave valida...")
            ft.enfocarTexto(txtPassword0)
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
            InsertarModificarPOSSupervisor(MyConn, lblInfo, Insertar, txtCodigo.Text, txtDescrip.Text, txtNombre.Text, _
                                       txtPassword0.Text, cmbTipo.SelectedIndex)

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

End Class