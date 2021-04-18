Imports MySql.Data.MySqlClient

Public Class jsControlArcUsuarios
    Private Const sModulo As String = "Usuarios"
    Private Const lRegion As String = "RibbonButton167"
    Private Const strSQLUsuarios As String = "select id_user, usuario, aes_decrypt(password,usuario) password, " _
        & " nombre, mapa, ini_emp, nivel, estatus, division, moneda from jsconctausu order by id_user"
    Private Const nTabla As String = "Usuarios"

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private i As Integer
    Private Posicion As Long

    Private Sub jsConUsuarios_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ds = Nothing
        dt = Nothing
        myConn.Close()
        myConn = Nothing
        ft = Nothing
    End Sub

    Private Sub jsConUsuarios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQLUsuarios)

            AsignarTooltips()
            IniciarGrilla()

            If dt.Rows.Count > 0 Then Posicion = 0
            AsignaTXT(Posicion, True)

            ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnSeleccionar, btnPrimero, _
                          btnSiguiente, btnAnterior, btnUltimo, btnImprimir, btnSalir)
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean)

        dt = ft.MostrarFilaEnTabla(myConn, ds, nTabla, strSQLUsuarios, Me.BindingContext, MenuBarra, _
                               dg, lRegion, jytsistema.sUsuario, nRow, Actualiza)

    End Sub

    Private Sub IniciarGrilla()

        Dim aCampos() As String = {"id_user.Código.50.C.", _
                                   "usuario.Login.100.I.", _
                                   "nombre.Nombre.320.I.", _
                                   "mapa.Mapa Acceso.60.C.", _
                                   "nivel.Nivel.80.I.", _
                                   "estatus.Estatus.80.I.", _
                                   "sada..10.I."}

        ft.IniciarTablaPlus(dg, dt, aCampos)

    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim f As New jsControlArcUsuariosMovimientos
        f.Agregar(myConn, ds, dt)
        If f.Apuntador >= 0 Then AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub
    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        Dim f As New jsControlArcUsuariosMovimientos
        f.Apuntador = Me.BindingContext(ds, nTabla).Position
        f.Editar(myConn, ds, dt)
        AsignaTXT(f.Apuntador, True)
        f = Nothing

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) = 2 Then

            Posicion = Me.BindingContext(ds, nTabla).Position

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                Dim aCampos() As String = {"id_user"}
                Dim aString() As String = {dt.Rows(Posicion).Item("id_user")}
                Posicion = EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsconctausu", strSQLUsuarios, aCampos, aString, Posicion)
            End If
            AsignaTXT(Posicion, True)
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"id_user", "usuario"}
        Dim Nombres() As String = {"Código Usuario", "Nombre Usuario"}
        Dim Anchos() As Integer = {100, 2500}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Usuarios del sistema...")
        AsignaTXT(f.Apuntador, False)
        f = Nothing
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        '
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Me.BindingContext(ds, nTabla).Position = 0
        AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Me.BindingContext(ds, nTabla).Position -= 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Me.BindingContext(ds, nTabla).Position += 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        '
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click

        Me.Close()
    End Sub

    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dg.CellFormatting

        Select Case dg.Columns(e.ColumnIndex).Name
            Case "nivel"
                Select Case e.Value
                    Case 0
                        e.Value = "Usuario"
                    Case 1
                        e.Value = "Superusuario"
                    Case 2
                        e.Value = "Soporte"
                End Select
            Case "estatus"
                Select Case CBool(e.Value)
                    Case False
                        e.Value = "Inactivo"
                    Case Else
                        e.Value = "Activo"
                End Select
        End Select
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.CellMouseClick
        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        AsignaTXT(e.RowIndex, False)
    End Sub


    Private Sub dg_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub
End Class