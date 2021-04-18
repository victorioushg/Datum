Imports MySql.Data.MySqlClient
Public Class jsControlArcParametros
    Private Const sModulo As String = "Parámetros de extensibilidad de sistema"
    Private Const lRegion As String = "RibbonButton173"
    Private Const nTabla As String = "parametros"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private strSQL As String
    Private Posicion As Long

    Private n_Seleccionado As String

    Public Sub Cargar(ByVal Mycon As MySqlConnection)

        myConn = Mycon
        ft.RellenaCombo(aGestion, cmbGestion)
        IniciarParametros(myConn, lblInfo)
        IniciarModulo(cmbGestion.SelectedIndex, cmbMod.SelectedValue.ToString)
        AsignarTooltips()
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)

        Me.Show()

    End Sub
    Private Sub IniciarModulo(ByVal numModulo As Integer, numInicio As String)

        strSQL = " select * from jsconparametros " _
            & " where " _
            & " gestion = " & numModulo + 1 & " and " _
            & " substring(descripcion,1,2) = '" & numInicio.Substring(0, 2) & "' and " _
            & " id_emp = '" & jytsistema.WorkID & "' " _
            & " order by  substring_index(descripcion, ' ', 1) "
        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        IniciarGrilla()
        If dt.Rows.Count > 0 Then Posicion = 0
        AsignaTXT(Posicion, True)

    End Sub
    Private Sub jsControlParametros_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt = Nothing
        ds = Nothing
    End Sub

    Private Sub jsControlParametros_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
    End Sub


    Private Sub frmBuscar_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                dg.Focus()
                Me.BindingContext(dt.DataSet, dt.TableName).Position += 1
                Posicion = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(Posicion, False)
            Case Keys.Up
                dg.Focus()
                Me.BindingContext(dt.DataSet, dt.TableName).Position -= 1
                Posicion = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(Posicion, False)
        End Select

    End Sub

    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo registro")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> registro deseado")
        C1SuperTooltip1.SetToolTip(btnSeleccionar, "<B>Seleccionar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir al <B>primer</B> registro")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir al <B>siguiente</B> registro")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir al registro <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir al <B>último registro</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B>")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")

        C1SuperTooltip1.SetToolTip(btnReconstruir, "<B>reconstrucción</B> de parámetros y sus respectivos valores por defecto")


    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean)

        Dim c As Integer = CInt(nRow)

        If Actualiza Then ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)

        If c >= 0 AndAlso ds.Tables(nTabla).Rows.Count > 0 Then
            Posicion = c
            Me.BindingContext(ds, nTabla).Position = c
            With dt
                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)
            End With
            dg.CurrentCell = dg(0, c)
        Else
            MostrarItemsEnMenuBarra(MenuBarra, 0, 0)
        End If
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
        btnReconstruir.Enabled = True

    End Sub

    Private Sub IniciarGrilla()
        Dim aCampos() As String = {"descripcion", "valor"}
        Dim aNombres() As String = {"Descripción", "Valor"}
        Dim aAnchos() As Integer = {700, 300}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {"", ""}
        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim f As New jsControlArcParametrosMovimientos
        f.Apuntador = Posicion
        f.Editar(myConn, ds, dt)
        AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"codigo", "descripcion"}
        Dim Nombres() As String = {"Código", "Descripción"}
        Dim Anchos() As Integer = {100, 2500}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Parámetros del sistema...")
        Me.BindingContext(ds, nTabla).Position = f.Apuntador
        Posicion = f.Apuntador
        AsignaTXT(Posicion, False)
        f = Nothing
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

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dg.CellFormatting

        Me.BindingContext(ds, nTabla).Position = e.RowIndex

        If dg.Columns(e.ColumnIndex).Name = "valor" Then
            Select Case ds.Tables(nTabla).Rows(e.RowIndex).Item("tipo")
                Case TipoParametro.iNoSi
                    e.Value = IIf(CBool(e.Value), "Si", "No")
                Case TipoParametro.iDiaSemana
                    Dim aDia() As String = {"Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"}
                    e.Value = aDia(CInt(e.Value))
                Case TipoParametro.iFormatoPapel
                    Dim aPapel() As String = {"Media Carta", "3/4 Carta", "Carta"}
                    e.Value = aPapel(CInt(e.Value))
                Case TipoParametro.iModoImpresora
                    Dim aModo() As String = {"Draft", "Gráfico"}
                    e.Value = aModo(CInt(e.Value))
                Case TipoParametro.iTipoImpresoraFiscal
                    e.Value = aFiscal(CInt(e.Value))
                Case TipoParametro.iNumeracionContable
                    Dim aNum() As String = {"Unica", "Diaria", "Mensual"}
                    e.Value = aNum(CInt(e.Value))
                Case TipoParametro.iMoneda
                    Dim moneda As String = ft.DevuelveScalarCadena(myConn, " select concat(UnidadMonetaria,'|',simbolo) from  " _
                                                     & " jsconcatmon where id = " & CInt(e.Value) & " ")
                    e.Value = moneda
                Case TipoParametro.iTabla
                    e.Value = SystemInformation.ComputerName
                Case Else
            End Select
        Else
            If e.Value IsNot Nothing Then
                Dim stringValue As String = CType(e.Value, String)
                If InStr("0. 1. 2. 3. 4. 5. 6. 7. 8. 9. 10. ", Mid(stringValue, 1, 3)) > 0 Then
                    e.CellStyle.Font = New Font("Tahoma", e.CellStyle.Font.Size, FontStyle.Bold)
                End If
            End If
        End If

    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.CellMouseClick
        Posicion = e.RowIndex
        AsignaTXT(Posicion, False)
    End Sub

    Private Sub btnReconstruir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReconstruir.Click
        IniciarParametros(myConn, lblInfo)
        IniciarModulo(cmbGestion.SelectedIndex, cmbMod.SelectedValue.ToString)
    End Sub


    Private Sub cmbGestion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbGestion.SelectedIndexChanged

        dg.Columns.Clear()

        Dim strSubModulo As String = " SELECT substring(DESCRIPCION,1,2) COD, DESCRIPCION FROM jsconparametros " _
                                    & " WHERE " _
                                    & " gestion = " & cmbGestion.SelectedIndex + 1 & " AND " _
                                    & " LENGTH(SUBSTRING_INDEX(descripcion, ' ', 1)) = 2 AND " _
                                    & " id_emp = '" & jytsistema.WorkID & "' " _
                                    & " ORDER BY 1 "

        Dim dtSub As DataTable
        Dim tblSub As String = "tblSubmodulo"

        ds = DataSetRequery(ds, strSubModulo, myConn, tblSub, lblInfo)
        dtSub = ds.Tables(tblSub)

        RellenaComboConDatatable(cmbMod, dtSub, "DESCRIPCION", "COD")

        dt.Dispose()
        dtSub = Nothing


    End Sub

    Private Sub cmbModulo_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbMod.SelectedIndexChanged

        If Not cmbMod.SelectedValue Is Nothing Then IniciarModulo(cmbGestion.SelectedIndex, cmbMod.SelectedValue.ToString)

    End Sub
End Class