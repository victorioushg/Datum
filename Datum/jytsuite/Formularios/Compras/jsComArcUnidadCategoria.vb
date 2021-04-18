Imports MySql.Data.MySqlClient
Imports fTransport
Public Class jsComArcUnidadCategoria
    Private Const sModulo As String = "Unidad de negocio y categorías de proveedores"
    Private Const lRegion As String = "RibbonButton56"
    Private Const nTablaG0 As String = "tblsg0"
    Private Const nTablaG1 As String = "tblsg1"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dtG0 As New DataTable
    Private dtG1 As New DataTable
    Private ft As New Transportables

    Private strSQLG0 As String = " select * from jsprolisuni where id_emp = '" & jytsistema.WorkID & "' order BY DESCRIP "
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
        ds = DataSetRequeryPlus(ds, nTablaG0, myConn, strSQLG0)
        dtG0 = ds.Tables(nTablaG0)

        IniciarGrilla(dg0, dtG0, "UNIDADES DE NEGOCIO")

        If dtG0.Rows.Count > 0 Then PosicionG0 = 0
        AsignaTXTG0(PosicionG0, True)
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.tipoMensaje.iInfo)

        If TipoCarga = TipoCargaFormulario.iShow Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If


    End Sub
    Private Sub IniciarGrilla(dg As DataGridView, dt As DataTable, nombreCampo As String)
        Dim aCampos() As String = {"DESCRIP." + nombreCampo + ".500.I."}
        ft.IniciarTablaPlus(dg, dt, aCampos, False)
    End Sub
    Private Sub jsComArcUnidadesCategorias_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dtG0 = Nothing
        dtG1 = Nothing
        ds = Nothing
    End Sub

    Private Sub jsComArcUnidadesCategorias_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub AsignarTooltips()

        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo registro de grupo")
        C1SuperTooltip1.SetToolTip(btnAgregarS1, "<B>Agregar</B> nuevo registro de subgrupo 1")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> registro actual  de grupo")
        C1SuperTooltip1.SetToolTip(btnEditarS1, "<B>Editar o mofificar</B> registro actual de subgrupo 1")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> registro actual de grupo")
        C1SuperTooltip1.SetToolTip(btnEliminarS1, "<B>Eliminar</B> registro actual de subgrupo 1")
        C1SuperTooltip1.SetToolTip(btnSeleccionarS1, "<B>Seleccionar</B> registro actual de subgrupo 1")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B>")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")

    End Sub

    Private Sub AsignaTXTG0(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)

        ds = DataSetRequeryPlus(ds, nTablaG0, myConn, strSQLG0)
        dtG0 = ds.Tables(nTablaG0)


        If c >= 0 AndAlso dtG0.Rows.Count > 0 Then

            Me.BindingContext(ds, nTablaG0).Position = c
            dg0.CurrentCell = dg0(0, c)

            Grupo0 = dtG0.Rows(c).Item("codigo")

            strSQLG1 = " select * from jsproliscat " _
                & " where " _
                & " antec = '" & Grupo0 & "'  and " _
                & " id_emp = '" & jytsistema.WorkID & "' order by DESCRIP "


            ds = DataSetRequeryPlus(ds, nTablaG1, myConn, strSQLG1)
            dtG1 = ds.Tables(nTablaG1)
            IniciarGrilla(dg1, dtG1, "CATEGORIAS DE NEGOCIO")

            If dtG1.Rows.Count > 0 Then PosicionG1 = 0
            MostrarFilaEnTabla(myConn, ds, nTablaG1, strSQLG1, Me.BindingContext, MenuBarraS1, dg1, "", jytsistema.sUsuario, PosicionG1, True)

        End If
        ft.ActivarMenuBarra(myConn, ds, dtG0, "", MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim f As New jsVenArcCanalTipoNegocioMovimientos
        dtG0 = ds.Tables(nTablaG0)
        f.Agregar(myConn, ds, dtG0, "", "jsprolisuni")
        If f.Apuntador >= 0 Then AsignaTXTG0(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim f As New jsVenArcCanalTipoNegocioMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaG0).Position
        dtG0 = ds.Tables(nTablaG0)
        f.Editar(myConn, ds, dtG0, "", "jsprolisuni")
        AsignaTXTG0(f.Apuntador, True)
        f = Nothing
    End Sub


    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub
    Private Sub Seleccion()
        Grupo0 = dtG0.Rows(PosicionG0).Item("codigo")
        Grupo1 = dtG1.Rows(PosicionG1).Item("codigo")
    End Sub

    'Private Sub dg1_CellClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg1.CellClick
    '    ft.mensajeInformativo(dtG1.Rows(dg1.CurrentCell.RowIndex).Item("codigo"))
    'End Sub
    'Private Sub dg0_CellClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg0.CellClick
    '    ft.mensajeInformativo(dtG0.Rows(dg0.CurrentCell.RowIndex).Item("codigo"))
    'End Sub
    Private Sub dg1_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dg1.CellDoubleClick
        Seleccion()
        Me.Close()
    End Sub

    Private Sub dg0_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg0.RowHeaderMouseClick, dg0.CellMouseUp

        Me.BindingContext(ds, nTablaG0).Position = e.RowIndex
        PosicionG0 = Me.BindingContext(ds, nTablaG0).Position
        AsignaTXTG0(PosicionG0, True)

    End Sub

    Private Sub dg1_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg1.RowHeaderMouseClick, dg1.CellMouseUp

        Me.BindingContext(ds, nTablaG1).Position = e.RowIndex
        PosicionG1 = Me.BindingContext(ds, nTablaG1).Position
        MostrarFilaEnTabla(myConn, ds, nTablaG1, strSQLG1, Me.BindingContext, MenuBarraS1, dg1, "", jytsistema.sUsuario, PosicionG1, True)

    End Sub

    Private Sub btnAgregarS1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarS1.Click
        Dim f As New jsVenArcCanalTipoNegocioMovimientos
        f.Agregar(myConn, ds, dtG1, Grupo0, "jsproliscat")
        If f.Apuntador >= 0 Then MostrarFilaEnTabla(myConn, ds, nTablaG1, strSQLG1, Me.BindingContext, MenuBarraS1, dg1, "", jytsistema.sUsuario, f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditarS1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarS1.Click
        Dim f As New jsVenArcCanalTipoNegocioMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaG1).Position
        f.Editar(myConn, ds, dtG1, Grupo0, "jsproliscat")
        MostrarFilaEnTabla(myConn, ds, nTablaG1, strSQLG1, Me.BindingContext, MenuBarraS1, dg1, "", jytsistema.sUsuario, f.Apuntador, True)
        f = Nothing
    End Sub


    Private Sub btnSeleccionarS1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionarS1.Click
        Seleccion()
        Me.Close()
    End Sub

    Private Sub btnEliminar_Click(sender As System.Object, e As System.EventArgs) Handles btnEliminar.Click

        PosicionG0 = Me.BindingContext(ds, nTablaG0).Position

        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
            If dg1.RowCount > 0 Then
                ft.mensajeCritico("No es posible eliminar, pues posee CATEGORIAS asociados...")
            Else
                Dim aCampos() As String = {"codigo", "id_emp"}
                Dim aString() As String = {dtG0.Rows(PosicionG0).Item("codigo"), jytsistema.WorkID}
                PosicionG0 = EliminarRegistros(myConn, lblInfo, ds, nTablaG0, "jsprolisuni", strSQLG0, aCampos, aString, PosicionG0)
            End If
        End If
        AsignaTXTG0(PosicionG0, True)

    End Sub

    Private Sub btnEliminarS1_Click(sender As System.Object, e As System.EventArgs) Handles btnEliminarS1.Click

        PosicionG1 = Me.BindingContext(ds, nTablaG1).Position

        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
            Dim aCampos() As String = {"codigo", "id_emp"}
            Dim aString() As String = {dtG1.Rows(PosicionG1).Item("codigo"), jytsistema.WorkID}
            PosicionG1 = EliminarRegistros(myConn, lblInfo, ds, nTablaG1, "jsproliscat", strSQLG1, aCampos, aString, PosicionG1)
        End If
        AsignaTXTG0(PosicionG0, True)
    End Sub

    Private Sub btnImprimir_Click(sender As System.Object, e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cGrupoSubGrupo, "Unidades y Categorías de Negocio", , , , _
                " select a.descrip grupo, b.descrip subgrupo " _
                & " from jsprolisuni a " _
                & " left join jsproliscat b on (a.codigo = b.antec and a.id_emp = b.id_emp) " _
                & " where " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " order by a.codigo ", "UNIDADES DE NEGOCIO", "CATEGORIAS DE NEGOCIO")
        f.Dispose()
        f = Nothing
    End Sub

   
    
End Class