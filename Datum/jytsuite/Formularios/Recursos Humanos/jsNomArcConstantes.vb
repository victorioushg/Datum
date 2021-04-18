Imports MySql.Data.MySqlClient

Public Class jsNomArcConstantes
    Private Const sModulo As String = "Constantes"
    Private Const lRegion As String = "RibbonButton40"
    Private Const nTabla As String = "tblNomContantes"
    Private strSQLConstantes As String = "select constante, tipo, valor, id_emp from jsnomcatcot where id_emp = '" & jytsistema.WorkID & "' order by constante "

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private i As Integer
    Private Posicion As Long

    Private Sub jsNomArcContantes_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        ds = Nothing
        dt = Nothing
        myConn.Close()
        myConn = Nothing
    End Sub

    Private Sub jsNomArcConstantes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill

        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQLConstantes, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)


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
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnSeleccionar, btnPrimero, btnSiguiente, _
                          btnAnterior, btnUltimo, btnImprimir, btnSalir)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean)

        dt = ft.MostrarFilaEnTabla(myConn, ds, nTabla, strSQLConstantes, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, _
                                 nRow, Actualiza)

    End Sub

    Private Sub IniciarGrilla()

        Dim aCampos() As String = {"constante.Código constantes.200.I.", "tipo.Tipo.200.I.", "valor.Valor.220.D.", "control..100.I."}
        ft.IniciarTablaPlus(dg, dt, aCampos)

    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim f As New jsNomArcConstantesMovimientos
        f.Agregar(myConn, ds, dt)
        If f.Apuntador >= 0 Then AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub
    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        If dt.Rows.Count > 0 Then
            Dim f As New jsNomArcConstantesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTabla).Position
            f.Editar(myConn, ds, dt)
            AsignaTXT(f.Apuntador, True)
            f = Nothing
        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        If dt.Rows.Count > 0 Then
            Posicion = Me.BindingContext(ds, nTabla).Position

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                Dim aCampos() As String = {"constante", "id_emp"}
                Dim aString() As String = {dt.Rows(Posicion).Item("constante"), dt.Rows(Posicion).Item("id_emp")}
                Posicion = EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsnomcatcot", strSQLConstantes, aCampos, aString, Posicion)
            End If
            AsignaTXT(Posicion, True)

        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        If dt.Rows.Count > 0 Then
            Dim f As New frmBuscar
            Dim Campos() As String = {"constante"}
            Dim Nombres() As String = {"Código Constante"}
            Dim Anchos() As Integer = {100}
            f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Constantes de nómina...")
            AsignaTXT(f.Apuntador, False)
            f = Nothing
        End If

    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        '
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        If dt.Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = 0
            AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
        End If
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        If dt.Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position -= 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
        End If
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        If dt.Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position += 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
        End If
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        If dt.Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
        End If
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsNomRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cConstantes, "Constantes de nómina")
        f = Nothing
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click

        Me.Close()
    End Sub

    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dg.CellFormatting

        Select Case dg.Columns(e.ColumnIndex).Name
            Case "tipo"
                Select Case e.Value
                    Case 0
                        e.Value = "Numérica"
                    Case 1
                        e.Value = "Fecha"
                    Case 2
                        e.Value = "Caracter"
                    Case 3
                        e.Value = "Booleana"
                End Select
        End Select
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
            dg.RowHeaderMouseClick, dg.CellMouseClick
        If dt.Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = e.RowIndex
            AsignaTXT(e.RowIndex, False)
        End If

    End Sub

End Class