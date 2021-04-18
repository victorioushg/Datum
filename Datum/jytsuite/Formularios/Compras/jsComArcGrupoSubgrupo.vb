Imports MySql.Data.MySqlClient
Public Class jsComArcGrupoSubgrupo
    Private Const sModulo As String = "Grupos y Subgrupos de gastos"
    Private Const lRegion As String = ""
    Private Const nTablaG0 As String = "tblsg0"
    Private Const nTablaG1 As String = "tblsg1"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dtG0 As New DataTable
    Private dtG1 As New DataTable
    Private ft As New Transportables

    Private strSQLG0 As String = " select * from jsprogrugas where antecesor = 0 and  id_emp = '" & jytsistema.WorkID & "' order by nombre "
    Private strSQLG1 As String = ""

    Private PosicionG0 As Long
    Private PosicionG1 As Long

    Private n_Grupo0 As String
    Private n_Grupo1 As String

    Public Property Grupo0() As String
        Get
            Return n_Grupo0
        End Get
        Set(ByVal value As String)
            n_Grupo0 = value
        End Set
    End Property
    Public Property Grupo1() As String
        Get
            Return n_Grupo1
        End Get
        Set(ByVal value As String)
            n_Grupo1 = value
        End Set
    End Property
    Public Sub Cargar(ByVal Mycon As MySqlConnection, ByVal TipoCarga As Integer)

        myConn = Mycon
        AsignarTooltips()
        ds = DataSetRequery(ds, strSQLG0, myConn, nTablaG0, lblInfo)
        dtG0 = ds.Tables(nTablaG0)
        IniciarGrilla(dtG0, dg0, "Grupo de Gastos")

        If dtG0.Rows.Count > 0 Then PosicionG0 = 0
        AsignaTXTG0(PosicionG0, True)
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)


        If TipoCarga = TipoCargaFormulario.iShow Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If


    End Sub
    Private Sub jsComArcGrupoSubgrupo_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dtG0 = Nothing
        dtG1 = Nothing
        ds = Nothing
    End Sub

    Private Sub jsComArcGrupoSubgrupo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub AsignarTooltips()

        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo registro de grupo")
        C1SuperTooltip1.SetToolTip(btnAgregarS1, "<B>Agregar</B> nuevo registro de subgrupo")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> registro actual  de grupo")
        C1SuperTooltip1.SetToolTip(btnEditarS1, "<B>Editar o mofificar</B> registro actual de subgrupo")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> registro actual de grupo")
        C1SuperTooltip1.SetToolTip(btnEliminarS1, "<B>Eliminar</B> registro actual de subgrupo")
        C1SuperTooltip1.SetToolTip(btnSeleccionarS1, "<B>Seleccionar</B> registro actual de subgrupo")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B>")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")

    End Sub

    Private Sub AsignaTXTG0(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLG0, myConn, nTablaG0, lblInfo)
        dtG0 = ds.Tables(nTablaG0)

        If c >= 0 AndAlso ds.Tables(nTablaG0).Rows.Count > 0 Then

            Me.BindingContext(ds, nTablaG0).Position = c
            dg0.CurrentCell = dg0(0, c)

            Grupo0 = ds.Tables(nTablaG0).Rows(c).Item("codigo")
            strSQLG1 = " select * from jsprogrugas " _
                & " where " _
                & " antecesor = '" & Grupo0 & "'  and " _
                & " id_emp = '" & jytsistema.WorkID & "' order by nombre "

            ds = DataSetRequery(ds, strSQLG1, myConn, nTablaG1, lblInfo)
            dtG1 = ds.Tables(nTablaG1)
            IniciarGrilla(dtG1, dg1, "Subgrupo")

            If dtG1.Rows.Count > 0 Then PosicionG1 = 0
            AsignaTXTG1(PosicionG1, True)

        End If
        ft.ActivarMenuBarra(myConn, ds, ds.Tables(nTablaG0), lRegion, MenuBarra, jytsistema.sUsuario)


    End Sub
    Private Sub AsignaTXTG1(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLG1, myConn, nTablaG1, lblInfo)
        dtG1 = ds.Tables(nTablaG1)

        If c >= 0 AndAlso ds.Tables(nTablaG1).Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaG1).Position = c
            dg1.CurrentCell = dg1(0, c)
            Grupo1 = ds.Tables(nTablaG1).Rows(c).Item("codigo")
        End If
        ft.ActivarMenuBarra(myConn, ds, ds.Tables(nTablaG1), lRegion, MenuBarraS1, jytsistema.sUsuario)

    End Sub

    Private Sub IniciarGrilla(ByVal dt As DataTable, ByVal dg As DataGridView, ByVal NombreCampo As String)
        Dim aCampos() As String = {"nombre"}
        Dim aNombres() As String = {NombreCampo}
        Dim aAnchos() As Integer = {500}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {""}
        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim f As New jsComArcGrupoSubgrupoMovimientos
        dtG0 = ds.Tables(nTablaG0)
        f.Agregar(myConn, ds, dtG0, 0)
        If f.Apuntador >= 0 Then AsignaTXTG0(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim f As New jsComArcGrupoSubgrupoMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaG0).Position
        dtG0 = ds.Tables(nTablaG0)
        f.Editar(myConn, ds, dtG0, 0)
        AsignaTXTG0(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        PosicionG0 = Me.BindingContext(ds, nTablaG0).Position
        If ft.DevuelveScalarEntero(myConn, " select count(*) from jsprogrugas where antecesor = '" & dtG0.Rows(PosicionG0).Item("codigo") & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
           
            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                Dim aCampos() As String = {"codigo", "id_emp"}
                Dim aString() As String = {dtG0.Rows(PosicionG0).Item("codigo"), jytsistema.WorkID}
                PosicionG0 = EliminarRegistros(myConn, lblInfo, ds, nTablaG0, "jsprogrugas", strSQLG0, aCampos, aString, PosicionG0)
            End If
        Else
            ft.mensajeCritico("Este Grupo tiene Subgrupos asignados...")
        End If
        AsignaTXTG0(PosicionG0, False)
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As New frmBuscar
        Dim Campos() As String = {"codigo", "nombre"}
        Dim Nombres() As String = {"Código grupo", "Descripción grupo"}
        Dim Anchos() As Integer = {100, 2500}
        f.Buscar(dtG0, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaG0).Position, "Grupo / Subgrupo de gastos...")
        AsignaTXTG0(f.Apuntador, False)
        f = Nothing
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cGrupoSubGrupo, "GRUPO Y SUBSUBGRUPO DE GASTOS...", , , , _
                " select a.descrip grupo, b.descrip subgrupo " _
                & " from jsprogrugas a " _
                & " left join jsprogrugas b on (a.codigo = b.antecesor and a.id_emp = b.id_emp) " _
                & " where " _
                & " a.antecesor = 0 and " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " order by a.codigo ", "GRUPO DE GASTOS", "SUBGRUPO DE GASTOS")
        f.Dispose()
        f = Nothing

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub
    Private Sub Seleccion()

        Grupo0 = dtG0.Rows(PosicionG0).Item("codigo")
        Grupo1 = dtG1.Rows(PosicionG1).Item("codigo")
        Me.Close()

    End Sub
    Private Sub dg1_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dg1.CellDoubleClick

        Seleccion()

    End Sub

    Private Sub dg0_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg0.RowHeaderMouseClick, dg0.CellMouseClick

        Me.BindingContext(ds, nTablaG0).Position = e.RowIndex
        PosicionG0 = Me.BindingContext(ds, nTablaG0).Position
        AsignaTXTG0(e.RowIndex, False)

    End Sub

    Private Sub dg1_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg1.RowHeaderMouseClick, dg1.CellMouseClick

        Me.BindingContext(ds, nTablaG1).Position = e.RowIndex
        PosicionG1 = Me.BindingContext(ds, nTablaG1).Position
        AsignaTXTG1(e.RowIndex, False)

    End Sub

    Private Sub btnAgregarS1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarS1.Click
        Dim f As New jsComArcGrupoSubgrupoMovimientos
        dtG1 = ds.Tables(nTablaG1)
        f.Agregar(myConn, ds, dtG1, Grupo0)
        If f.Apuntador >= 0 Then AsignaTXTG1(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditarS1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarS1.Click
        Dim f As New jsComArcGrupoSubgrupoMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaG1).Position
        dtG1 = ds.Tables(nTablaG1)
        f.Editar(myConn, ds, dtG1, Grupo0)
        AsignaTXTG1(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEliminarS1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarS1.Click
        PosicionG1 = Me.BindingContext(ds, nTablaG1).Position

        If NumeroDeRegistrosEnTabla(myConn, "jsproencgas", " subgrupo = '" & dtG1.Rows(PosicionG1).Item("codigo") & "' ") > 0 Then
            ft.MensajeCritico("SUBGRUPO NO ELIMINABLE...")
        Else
            
            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                Dim aCampos() As String = {"codigo", "id_emp"}
                Dim aString() As String = {dtG1.Rows(PosicionG1).Item("codigo"), jytsistema.WorkID}
                PosicionG1 = EliminarRegistros(myConn, lblInfo, ds, nTablaG1, "jsprogrugas", strSQLG1, aCampos, aString, PosicionG1)
            End If
            AsignaTXTG1(PosicionG1, False)
        End If


    End Sub

    Private Sub btnSeleccionarS1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionarS1.Click
        Seleccion()
    End Sub

    Private Sub btnImprimir_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

End Class