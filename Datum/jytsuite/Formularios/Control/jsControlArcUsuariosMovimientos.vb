Imports MySql.Data.MySqlClient
Public Class jsControlArcUsuariosMovimientos

    Private Const sModulo As String = "Movimientos de Usuarios"
    Private Const nTabla As String = "Usuarios"


    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private dtEmpresas As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private aEstatus() As String = {"Inactivo", "Activo"}
    Private aStat() As String = {"No", "Si"}
    Private aNivel = {"Usuario", "SuperUsuario", "Soporte"}
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

        ft.habilitarObjetos(False, True, txtCodigo, txtMapa)

        txtCodigo.Text = ft.autoCodigo(MyConn, "ID_USER", "jsconctausu", "", "", 5, True)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtLogin, txtContraseña0, txtContraseña1, txtNombre, _
                                txtMapa, lblMapa)

        ft.RellenaCombo(aNivel, cmbNivel, 0)
        ft.RellenaCombo(aEstatus, cmbEstatus, 1)

        VerEmpresas(txtCodigo.Text)

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

        ft.habilitarObjetos(False, True, txtCodigo, txtLogin, txtMapa)

        With dtLocal.Rows(nPosicion)

            txtCodigo.Text = ft.muestraCampoTexto(.Item("id_user"))
            txtLogin.Text = ft.muestraCampoTexto(.Item("usuario"))
            txtContraseña0.Text = ft.muestraCampoTexto(.Item("password"))
            txtContraseña1.Text = ft.muestraCampoTexto(.Item("password"))
            txtNombre.Text = ft.muestraCampoTexto(.Item("nombre"))
            txtMapa.Text = ft.muestraCampoTexto(.Item("mapa"))

            ft.RellenaCombo(aNivel, cmbNivel, .Item("nivel"))
            ft.RellenaCombo(aEstatus, cmbEstatus, .Item("estatus"))

            'EMPRESAS
            VerEmpresas(txtCodigo.Text)

        End With
    End Sub

    Private Sub VerEmpresas(Usuario As String)

        dtEmpresas = ft.AbrirDataTable(dsLocal, "tblEmpresas", MyConn, "SELECT  a.id_emp, a.nombre,  IFNULL(b.permite_empresa, 0) permite_empresa, IFNULL(b.empresa_inicial, 0) empresa_inicial " _
                                       & " FROM jsconctaemp a " _
                                       & " LEFT JOIN jsconctausuemp b ON (a.id_emp = b.id_emp AND b.id_user = '" & Usuario & "') " _
                                       & " ORDER BY a.id_emp")

        Dim aCampos() As String = {"id_emp.ID.30.C.", _
                                   "nombre.Nombre.250.I.", _
                                   "permite_empresa.Permite.70.C.", _
                                   "empresa_inicial.Inicial.70.C."}

        ft.IniciarTablaPlus(dg, dtEmpresas, aCampos, , True, , False)
        Dim aCamp() As String = {"id_emp", "Nombre"}
        ft.EditarColumnasEnDataGridView(dg, aCamp)

    End Sub

    Private Sub btnMapa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMapa.Click
        txtMapa.Text = CargarTablaSimple(MyConn, lblInfo, dsLocal, " select mapa codigo, nombre descripcion from jsconencmap order by mapa", "Mapas de Seguridad", _
                                            txtMapa.Text)
    End Sub
    Private Sub jsConUsuariosMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        ft = Nothing
        dtEmpresas.Dispose()
        dtEmpresas = Nothing

    End Sub

    Private Sub jsConUsuariosMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
        cmbNivel.Enabled = False
        If NivelUsuario(MyConn, lblInfo, jytsistema.sUsuario) >= 1 Then cmbNivel.Enabled = True
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el código de usuario...", Transportables.tipoMensaje.iInfo)
    End Sub

    Private Function Validado() As Boolean

        If txtLogin.Text.Trim = "" Then
            ft.mensajeAdvertencia("Debe indicar un login válido...")
            ft.enfocarTexto(txtLogin)
            Return False
        End If

        If Trim(txtContraseña0.Text) = "" Or Trim(txtContraseña1.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar una contraseña válido...")
            ft.enfocarTexto(txtContraseña0)
            Return False
        Else
            If txtContraseña0.Text <> txtContraseña1.Text Then
                ft.mensajeAdvertencia("Las contraseñas no coinciden...")
                ft.enfocarTexto(txtContraseña0)
                Return False
            End If
        End If

        If Trim(txtMapa.Text) = "" Then
            ft.mensajeAdvertencia("Debe seleccionar un mapa válido...")
            btnMapa.Focus()
            Return False
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

            ActualizarEmpresasPorUsuario()

            InsertEditCONTROLUsuario(MyConn, lblInfo, IIf(i_modo = movimiento.iAgregar, True, False), txtLogin.Text, _
                txtContraseña0.Text, txtNombre.Text, txtCodigo.Text, txtMapa.Text, "", _
                0, cmbNivel.SelectedIndex, cmbEstatus.SelectedIndex, "", "")

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)

            Me.Close()

        End If
    End Sub

    Private Sub ActualizarEmpresasPorUsuario()
        For Each nRow As DataGridViewRow In dg.Rows
            With nRow
                Dim Insertar As Boolean = Not CBool(ft.DevuelveScalarEntero(MyConn, " select count(*) from jsconctausuemp " _
                                           & " where " _
                                           & " id_user = '" & txtCodigo.Text & "' AND id_emp = '" & .Cells("id_emp").Value & "' "))
                InsertEditCONTROLUsuarioEmpresas(MyConn, lblInfo, Insertar, txtCodigo.Text, .Cells("id_emp").Value, _
                                                 IIf(CBool(.Cells("permite_empresa").Value), 1, 0), _
                                                 IIf(CBool(.Cells("empresa_inicial").Value), 1, 0))
            End With
        Next
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtMapa_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMapa.TextChanged
        lblMapa.Text = ft.DevuelveScalarCadena(MyConn, " select nombre from jsconencmap where mapa = '" & txtMapa.Text & "' ")
    End Sub

    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dg.CellFormatting
        Select Case dg.Columns(e.ColumnIndex).Name
            Case "permite_empresa", "empresa_inicial"
                If CBool(e.Value) Then
                    e.Value = "Si"
                Else
                    e.Value = "No"
                End If
        End Select
    End Sub

    Private Sub dg_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellClick

        If dtEmpresas.Rows(e.RowIndex).Item("id_emp").ToString.Length <> 0 Then
            Select Case e.ColumnIndex
                Case 2 'PERMITE EMPRESA
                    With dtEmpresas.Rows(e.RowIndex)
                        Dim ff As Integer = IIf(CBool(.Item(dg.Columns(e.ColumnIndex).Name).ToString) = True, 0, 1)
                        .Item(dg.Columns(e.ColumnIndex).Name) = Not CBool(.Item(dg.Columns(e.ColumnIndex).Name).ToString)

                    End With
                Case 3
                    Dim ff As Integer = IIf(CBool(dtEmpresas.Rows(e.RowIndex).Item("EMPRESA_INICIAL").ToString) = True, 0, 1)
                    For Each nRow As DataRow In dtEmpresas.Rows
                        With nRow
                            If .Item("id_emp") = dtEmpresas.Rows(e.RowIndex).Item("id_emp") Then
                                .Item("EMPRESA_INICIAL") = CBool(ff)
                            Else
                                .Item("EMPRESA_INICIAL") = Not CBool(ff)
                            End If
                        End With
                    Next
            End Select

            'Ninguna PERMITE EMPRESAS = NO EMPRESA INICIAL
            Dim numEmpresasPermitidas As Int16 = 0
            For Each dgRow As DataGridViewRow In dg.Rows
                If CBool(dgRow.Cells(2).Value) Then numEmpresasPermitidas += 1
            Next

            If numEmpresasPermitidas = 0 Then
                For Each nRow As DataRow In dtEmpresas.Rows
                    nRow.Item("EMPRESA_INICIAL") = False
                Next
            ElseIf numEmpresasPermitidas = 1 Then
                For Each nRow As DataRow In dtEmpresas.Rows
                    nRow.Item("EMPRESA_INICIAL") = nRow.Item("PERMITE_EMPRESA")
                Next
            End If
           
        End If


    End Sub

End Class