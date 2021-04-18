Imports MySql.Data.MySqlClient
Public Class jsVenArcCanalTiponegocio
    Private Const sModulo As String = "Canal de distribución y tipo de negocio"
    Private Const lRegion As String = "RibbonButton76"
    Private Const nTablaG0 As String = "tblsg0"
    Private Const nTablaG1 As String = "tblsg1"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dtG0 As New DataTable
    Private dtG1 As New DataTable
    Private ft As New Transportables

    Private strSQLG0 As String = " select * from jsvenliscan where id_emp = '" & jytsistema.WorkID & "' order by descrip "
    Private strSQLG1 As String = ""

    Private PosicionG0 As Long
    Private PosicionG1 As Long

    Private n_Grupo0 As String
    Private n_Grupo1 As String

    Private PrimeraVEZ As Boolean = False
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
        IniciarGrilla(dtG0, dg0, "Canal de Distribución")
        If Grupo0 <> "" AndAlso Grupo1 <> "" Then PrimeraVEZ = True
        If dtG0.Rows.Count > 0 Then PosicionG0 = 0

        If Grupo0 <> "" AndAlso PrimeraVEZ Then
            Dim row As DataRow = dtG0.Select(" codigo = '" & Grupo0 & "'")(0)
            PosicionG0 = dtG0.Rows.IndexOf(row)
        End If
        AsignaTXTG0(PosicionG0, True)

        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)


        If TipoCarga = TipoCargaFormulario.iShow Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If


    End Sub
    Private Sub jsVenArcCanalTiponegocio_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dtG0 = Nothing
        dtG1 = Nothing
        ds = Nothing
    End Sub

    Private Sub jsVenArcCanalTiponegocio_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
        If Actualiza Then ds = DataSetRequery(ds, strSQLG0, myConn, nTablaG0, lblInfo)
        dtG0 = ds.Tables(nTablaG0)

        If c >= 0 AndAlso ds.Tables(nTablaG0).Rows.Count > 0 Then

            Me.BindingContext(ds, nTablaG0).Position = c
            dg0.CurrentCell = dg0(0, c)

            Grupo0 = ds.Tables(nTablaG0).Rows(c).Item("codigo")
            strSQLG1 = " select * from jsvenlistip " _
                & " where " _
                & " antec = '" & Grupo0 & "'  and " _
                & " id_emp = '" & jytsistema.WorkID & "' order by descrip "

            ds = DataSetRequery(ds, strSQLG1, myConn, nTablaG1, lblInfo)
            dtG1 = ds.Tables(nTablaG1)
            IniciarGrilla(dtG1, dg1, "Tipo de Negocio")
            If dtG1.Rows.Count > 0 Then PosicionG1 = 0
            If Grupo1 <> "" AndAlso PrimeraVEZ Then
                Dim row As DataRow = dtG1.Select(" codigo = '" & Grupo1 & "'")(0)
                PosicionG1 = dtG1.Rows.IndexOf(row)
                PrimeraVEZ = False
            End If

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
        Dim aCampos() As String = {"descrip"}
        Dim aNombres() As String = {NombreCampo}
        Dim aAnchos() As Integer = {500}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {""}
        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

        Dim f As New jsVenArcCanalTipoNegocioMovimientos
        dtG0 = ds.Tables(nTablaG0)
        f.Agregar(myConn, ds, dtG0, "", "jsvenliscan")
        If f.Apuntador >= 0 Then AsignaTXTG0(f.Apuntador, True)
        f = Nothing

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim f As New jsVenArcCanalTipoNegocioMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaG0).Position
        dtG0 = ds.Tables(nTablaG0)
        f.Editar(myConn, ds, dtG0, "", "jsvenliscan")
        AsignaTXTG0(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        PosicionG0 = Me.BindingContext(ds, nTablaG0).Position
        If ft.PreguntaEliminarRegistro() = DialogResult.Yes Then
            If dg1.RowCount > 0 Then
                ft.mensajeCritico("No es posible eliminar, pues posee Tipos de Negocio asociados...")
            Else
                Dim aCampos() As String = {"codigo", "id_emp"}
                Dim aString() As String = {dtG0.Rows(PosicionG0).Item("codigo"), jytsistema.WorkID}
                PosicionG0 = EliminarRegistros(myConn, lblInfo, ds, nTablaG0, "jsvenliscan", strSQLG0, aCampos, aString, PosicionG0)
            End If
        End If
        AsignaTXTG0(PosicionG0, False)
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim f As New frmBuscar
        'Dim Campos() As String = {"grupo", "descrip"}
        'Dim Nombres() As String = {"Código grupo", "Descripción grupo"}
        'Dim Anchos() As Integer = {100, 2500}
        'f.Buscar(dtG0, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaG0).Position)
        'AsignaTXTG0(f.Apuntador, False)
        'f = Nothing
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Grupo0 = ""
        Grupo1 = ""
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
        Dim f As New jsVenArcCanalTipoNegocioMovimientos
        dtG1 = ds.Tables(nTablaG1)
        f.Agregar(myConn, ds, dtG1, Grupo0, "jsvenlistip")
        If f.Apuntador >= 0 Then AsignaTXTG1(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditarS1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarS1.Click
        Dim f As New jsVenArcCanalTipoNegocioMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaG1).Position
        dtG1 = ds.Tables(nTablaG1)
        f.Editar(myConn, ds, dtG1, Grupo0, "jsvenlistip")
        AsignaTXTG1(f.Apuntador, True)
        f = Nothing
    End Sub


    Private Sub btnSeleccionarS1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionarS1.Click
        Seleccion()
    End Sub

    Private Sub btnImprimir_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cGrupoSubGrupo, "Canales de Distribución y Tipo de Negocio", , , , _
                " select a.descrip grupo, b.descrip subgrupo " _
                & " from jsvenliscan a " _
                & " left join jsvenlistip b on (a.codigo = b.antec and a.id_emp = b.id_emp) " _
                & " where " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " order by a.codigo ", "CANAL DE DISTRIBUCION", "TIPO DE NEGOCIO")
        f.Dispose()
        f = Nothing

    End Sub

    Private Sub btnEliminarS1_Click(sender As System.Object, e As System.EventArgs) Handles btnEliminarS1.Click

        PosicionG1 = Me.BindingContext(ds, nTablaG1).Position
        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
            Dim aCampos() As String = {"codigo", "id_emp"}
            Dim aString() As String = {dtG1.Rows(PosicionG1).Item("codigo"), jytsistema.WorkID}
            PosicionG1 = EliminarRegistros(myConn, lblInfo, ds, nTablaG1, "jsvenlistip", strSQLG1, aCampos, aString, PosicionG1)
        End If
        AsignaTXTG0(PosicionG0, False)
    End Sub
End Class