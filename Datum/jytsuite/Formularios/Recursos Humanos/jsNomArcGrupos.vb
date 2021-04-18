Imports MySql.Data.MySqlClient
Public Class jsNomArcGrupos
    Private Const sModulo As String = "Grupos de Trabajadores de Nómina"
    Private Const lRegion As String = "RibbonButton33"
    Private Const nTablaG0 As String = "tblsg0"
    Private Const nTablaG1 As String = "tblsg1"
    Private Const nTablaG2 As String = "tblsg2"
    Private Const nTablaG3 As String = "tblsg3"
    Private Const nTablaG4 As String = "tblsg4"
    Private Const nTablaG5 As String = "tblsg5"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dtG0 As New DataTable
    Private dtG1 As New DataTable
    Private dtG2 As New DataTable
    Private dtG3 As New DataTable
    Private dtG4 As New DataTable
    Private dtG5 As New DataTable
    Private ft As New Transportables

    Private strSQL As String = "select * from jsnomrengru where nivel = 0 and id_emp = '" & jytsistema.WorkID & "' order by cod_nivel "
    Private strSQLG1 As String = ""
    Private strSQLG2 As String = ""
    Private strSQLG3 As String = ""
    Private strSQLG4 As String = ""
    Private strSQLG5 As String = ""

    Private Posicion As Long
    Private PosicionG1 As Long
    Private PosicionG2 As Long
    Private PosicionG3 As Long
    Private PosicionG4 As Long
    Private PosicionG5 As Long

    Private n_Grupo As String
    Private n_SubGrupo1 As String
    Private n_SubGrupo2 As String
    Private n_SubGrupo3 As String
    Private n_SubGrupo4 As String
    Private n_SubGrupo5 As String

    Public Property Grupo() As String
        Get
            Return n_Grupo
        End Get
        Set(ByVal value As String)
            n_Grupo = value
        End Set
    End Property
    Public Property subGrupo1() As String
        Get
            Return n_SubGrupo1
        End Get
        Set(ByVal value As String)
            n_SubGrupo1 = value
        End Set
    End Property
    Public Property SubGrupo2() As String
        Get
            Return n_SubGrupo2
        End Get
        Set(ByVal value As String)
            n_SubGrupo2 = value
        End Set
    End Property
    Public Property SubGrupo3() As String
        Get
            Return n_SubGrupo3
        End Get
        Set(ByVal value As String)
            n_SubGrupo3 = value
        End Set
    End Property
    Public Property SubGrupo4() As String
        Get
            Return n_SubGrupo4
        End Get
        Set(ByVal value As String)
            n_SubGrupo4 = value
        End Set
    End Property
    Public Property SubGrupo5() As String
        Get
            Return n_SubGrupo5
        End Get
        Set(ByVal value As String)
            n_SubGrupo5 = value
        End Set
    End Property
    Public Sub Cargar(ByVal Mycon As MySqlConnection, ByVal TipoCarga As Integer)

        ' 0 = show() ; 1 = showdialog()

        myConn = Mycon
        AsignarTooltips()

        IniciarSubGrupo(ds, strSQL, nTablaG0, dtG0, dg, Posicion)

        If TipoCarga = 0 Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If


    End Sub
    Private Sub jsNomArcGrupos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dtG0 = Nothing
        dtG1 = Nothing
        dtG2 = Nothing
        dtG3 = Nothing
        dtG4 = Nothing
        dtG5 = Nothing
        ds = Nothing
    End Sub

    Private Sub jsControlTarjetas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub AsignarTooltips()

        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo registro de grupo")
        C1SuperTooltip1.SetToolTip(btnAgregarS1, "<B>Agregar</B> nuevo registro de subgrupo 1")
        C1SuperTooltip1.SetToolTip(btnAgregarS2, "<B>Agregar</B> nuevo registro de subgrupo 2")
        C1SuperTooltip1.SetToolTip(btnAgregarS3, "<B>Agregar</B> nuevo registro de subgrupo 3")
        C1SuperTooltip1.SetToolTip(btnAgregarS4, "<B>Agregar</B> nuevo registro de subgrupo 4")
        C1SuperTooltip1.SetToolTip(btnAgregarS5, "<B>Agregar</B> nuevo registro de subgrupo 5")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> registro actual  de grupo")
        C1SuperTooltip1.SetToolTip(btnEditarS1, "<B>Editar o mofificar</B> registro actual de subgrupo 1")
        C1SuperTooltip1.SetToolTip(btnEditarS2, "<B>Editar o mofificar</B> registro actual de subgrupo 2")
        C1SuperTooltip1.SetToolTip(btnEditarS3, "<B>Editar o mofificar</B> registro actual de subgrupo 3")
        C1SuperTooltip1.SetToolTip(btnEditarS4, "<B>Editar o mofificar</B> registro actual de subgrupo 4")
        C1SuperTooltip1.SetToolTip(btnEditarS5, "<B>Editar o mofificar</B> registro actual de subgrupo 5")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> registro actual de grupo")
        C1SuperTooltip1.SetToolTip(btnEliminarS1, "<B>Eliminar</B> registro actual de subgrupo 1")
        C1SuperTooltip1.SetToolTip(btnEliminarS2, "<B>Eliminar</B> registro actual de subgrupo 2")
        C1SuperTooltip1.SetToolTip(btnEliminarS3, "<B>Eliminar</B> registro actual de subgrupo 3")
        C1SuperTooltip1.SetToolTip(btnEliminarS4, "<B>Eliminar</B> registro actual de subgrupo 4")
        C1SuperTooltip1.SetToolTip(btnEliminarS5, "<B>Eliminar</B> registro actual de subgrupo 5")
        C1SuperTooltip1.SetToolTip(btnSeleccionar, "<B>Seleccionar</B> registro actual de grupo")
        C1SuperTooltip1.SetToolTip(btnSeleccionarS1, "<B>Seleccionar</B> registro actual de subgrupo 1")
        C1SuperTooltip1.SetToolTip(btnSeleccionarS2, "<B>Seleccionar</B> registro actual de subgrupo 2")
        C1SuperTooltip1.SetToolTip(btnSeleccionarS3, "<B>Seleccionar</B> registro actual de subgrupo 3")
        C1SuperTooltip1.SetToolTip(btnSeleccionarS4, "<B>Seleccionar</B> registro actual de subgrupo 4")
        C1SuperTooltip1.SetToolTip(btnSeleccionarS5, "<B>Seleccionar</B> registro actual de subgrupo 5")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B>")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")

    End Sub
    Private Sub IniciarSubGrupo(ByVal ds As DataSet, ByVal strSQL As String, ByVal nTabla As String, _
                                ByVal dt As DataTable, ByVal dg As DataGridView, ByVal Posicion As Long)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblinfo)
        dt = ds.Tables(nTabla)

        IniciarGrilla(dt, dg)
        If dt.Rows.Count > 0 Then Posicion = 0

        Select Case dg.Name
            Case "dg"
                dg5.Columns.Clear() : DesactivarMenuBarra(MenuBarraS5)
                dg4.Columns.Clear() : DesactivarMenuBarra(MenuBarraS4)
                dg3.Columns.Clear() : DesactivarMenuBarra(MenuBarraS3)
                dg2.Columns.Clear() : DesactivarMenuBarra(MenuBarraS2)
                dg1.Columns.Clear() : DesactivarMenuBarra(MenuBarraS1)
                AsignaTXTG0(Posicion, True)
            Case "dg1"
                dg5.Columns.Clear() : DesactivarMenuBarra(MenuBarraS5)
                dg4.Columns.Clear() : DesactivarMenuBarra(MenuBarraS4)
                dg3.Columns.Clear() : DesactivarMenuBarra(MenuBarraS3)
                dg2.Columns.Clear() : DesactivarMenuBarra(MenuBarraS2)
                AsignaTXTG1(Posicion, True)
            Case "dg2"
                dg5.Columns.Clear() : DesactivarMenuBarra(MenuBarraS5)
                dg4.Columns.Clear() : DesactivarMenuBarra(MenuBarraS4)
                dg3.Columns.Clear() : DesactivarMenuBarra(MenuBarraS3)
                AsignaTXTG2(Posicion, True)
            Case "dg3"
                dg5.Columns.Clear() : DesactivarMenuBarra(MenuBarraS5)
                dg4.Columns.Clear() : DesactivarMenuBarra(MenuBarraS4)
                AsignaTXTG3(Posicion, True)
            Case "dg4"
                dg5.Columns.Clear() : DesactivarMenuBarra(MenuBarraS5)
                AsignaTXTG4(Posicion, True)
            Case "dg5"
                AsignaTXTG5(Posicion, True)
        End Select


    End Sub
    Private Sub LimpiaGrilla(ByVal dgR As DataGridView, ByVal BarraMenu As System.Windows.Forms.ToolStrip)
        Try
            If dgR.Rows.Count > 0 Then
                dgR.Rows.Clear()
                DesactivarMenuBarra(BarraMenu)
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub AsignaTXTG0(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQL, myConn, nTablaG0, lblInfo)

        dtG0 = ds.Tables(nTablaG0)

        If c >= 0 AndAlso ds.Tables(nTablaG0).Rows.Count > 0 Then

            Me.BindingContext(ds, nTablaG0).Position = c
            dg.CurrentCell = dg(0, c)

            Grupo = ds.Tables(nTablaG0).Rows(c).Item("cod_nivel")
            strSQLG1 = " select * from jsnomrengru " _
                & " where " _
                & " cod_nivel_ant = '" & Grupo & "' and nivel = 1 and " _
                & " id_emp = '" & jytsistema.WorkID & "' order by des_nivel "

            IniciarSubGrupo(ds, strSQLG1, nTablaG1, dtG1, dg1, PosicionG1)
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
            subGrupo1 = ds.Tables(nTablaG1).Rows(c).Item("cod_nivel")
            strSQLG2 = " select * from jsnomrengru " _
                & " where " _
                & " cod_nivel_ant = '" & subGrupo1 & "' and nivel = 2 and " _
                & " id_emp = '" & jytsistema.WorkID & "' order by des_nivel "
            IniciarSubGrupo(ds, strSQLG2, nTablaG2, dtG2, dg2, PosicionG2)
        End If
        ft.ActivarMenuBarra(myConn, ds, ds.Tables(nTablaG1), lRegion, MenuBarraS1, jytsistema.sUsuario)

    End Sub
    Private Sub AsignaTXTG2(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLG2, myConn, nTablaG2, lblInfo)

        dtG2 = ds.Tables(nTablaG2)

        If c >= 0 AndAlso ds.Tables(nTablaG2).Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaG2).Position = c
            dg2.CurrentCell = dg2(0, c)
            SubGrupo2 = ds.Tables(nTablaG2).Rows(c).Item("cod_nivel")
            strSQLG3 = " select * from jsnomrengru " _
                & " where " _
                & " cod_nivel_ant = '" & SubGrupo2 & "' and nivel = 3 and " _
                & " id_emp = '" & jytsistema.WorkID & "' order by des_nivel "
            IniciarSubGrupo(ds, strSQLG3, nTablaG3, dtG3, dg3, PosicionG3)
        End If

        ft.ActivarMenuBarra(myConn, ds, ds.Tables(nTablaG2), lRegion, MenuBarraS2, jytsistema.sUsuario)

    End Sub

    Private Sub AsignaTXTG3(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLG3, myConn, nTablaG3, lblInfo)

        dtG3 = ds.Tables(nTablaG3)

        If c >= 0 AndAlso ds.Tables(nTablaG3).Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaG3).Position = c
            dg3.CurrentCell = dg3(0, c)
            SubGrupo3 = ds.Tables(nTablaG3).Rows(c).Item("cod_nivel")
            strSQLG4 = " select * from jsnomrengru " _
                & " where " _
                & " cod_nivel_ant = '" & SubGrupo3 & "' and nivel = 4 and " _
                & " id_emp = '" & jytsistema.WorkID & "' order by des_nivel "
            IniciarSubGrupo(ds, strSQLG4, nTablaG4, dtG4, dg4, PosicionG4)
        End If
        ft.ActivarMenuBarra(myConn, ds, ds.Tables(nTablaG3), lRegion, MenuBarraS3, jytsistema.sUsuario)

    End Sub

    Private Sub AsignaTXTG4(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLG4, myConn, nTablaG4, lblInfo)

        dtG4 = ds.Tables(nTablaG4)

        If c >= 0 AndAlso ds.Tables(nTablaG4).Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaG4).Position = c
            dg4.CurrentCell = dg4(0, c)
            SubGrupo4 = ds.Tables(nTablaG4).Rows(c).Item("cod_nivel")
            strSQLG5 = " select * from jsnomrengru " _
                & " where " _
                & " cod_nivel_ant = '" & SubGrupo4 & "' and nivel = 5 and " _
                & " id_emp = '" & jytsistema.WorkID & "' order by des_nivel "
            IniciarSubGrupo(ds, strSQLG5, nTablaG5, dtG5, dg5, PosicionG5)
        End If
        ft.ActivarMenuBarra(myConn, ds, ds.Tables(nTablaG4), lRegion, MenuBarraS4, jytsistema.sUsuario)

    End Sub
    Private Sub AsignaTXTG5(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLG5, myConn, nTablaG5, lblInfo)
        dtG5 = ds.Tables(nTablaG5)
        If c >= 0 AndAlso ds.Tables(nTablaG5).Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaG5).Position = c
            dg5.CurrentCell = dg5(0, c)
        End If
        ft.ActivarMenuBarra(myConn, ds, ds.Tables(nTablaG5), lRegion, MenuBarraS5, jytsistema.sUsuario)

    End Sub
    Private Sub IniciarGrilla(ByVal dt As DataTable, ByVal dg As DataGridView)
        Dim aCampos() As String = {"des_nivel"}
        Dim aNombres() As String = {"Nombre Grupo/SubGrupo "}
        Dim aAnchos() As Integer = {500}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {""}
        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim f As New jsNomArcGruposMovimientos
        dtG0 = ds.Tables(nTablaG0)
        f.Agregar(myConn, ds, dtG0, "", 0)
        If f.Apuntador >= 0 Then AsignaTXTG0(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim f As New jsNomArcGruposMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaG0).Position
        dtG0 = ds.Tables(nTablaG0)
        f.Editar(myConn, ds, dtG0, "", 0)
        AsignaTXTG0(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        '      
        '      Posicion = Me.BindingContext(ds, nTablaG0).Position
        '      sRespuesta = MsgBox(" ¿ Está seguro que desea eliminar registro ?", vbYesNo, "Eliminar registro en " & sModulo & " ...")
        '      If sRespuesta = vbYes Then
        ' Dim aCampos() As String = {"codtar", "id_emp"}
        ' Dim aString() As String = {dtG0.Rows(Posicion).Item("codtar"), jytsistema.WorkID}
        ' EliminarRegistros(myConn,lblinfo, ds, nTablaG0, "jsconctatar", strSQL, aCampos, aString, Posicion)
        ' End If
        ' AsignaTXTG0(Posicion, False)
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
        Me.Close()
    End Sub
    Private Sub Seleccion(ByVal dt As DataTable)

        If dt.Rows.Count > 0 Then

            If dt.TableName = "tblsg0" Or dt.TableName = "tblsg1" Or dt.TableName = "tblsg2" Or dt.TableName = "tblsg3" Or dt.TableName = "tblsg4" Or dt.TableName = "tblsg5" Then
                Grupo = dtG0.Rows(Me.BindingContext(ds, nTablaG0).Position).Item("cod_nivel").ToString
            Else
                Grupo = ""
            End If

            If dt.TableName = "tblsg1" Or dt.TableName = "tblsg2" Or dt.TableName = "tblsg3" Or dt.TableName = "tblsg4" Or dt.TableName = "tblsg5" Then
                subGrupo1 = dtG1.Rows(Me.BindingContext(ds, nTablaG1).Position).Item("cod_nivel").ToString
            Else
                subGrupo1 = ""
            End If

            If dt.TableName = "tblsg2" Or dt.TableName = "tblsg3" Or dt.TableName = "tblsg4" Or dt.TableName = "tblsg5" Then
                SubGrupo2 = dtG2.Rows(Me.BindingContext(ds, nTablaG2).Position).Item("cod_nivel").ToString
            Else
                SubGrupo2 = ""
            End If

            If dt.TableName = "tblsg3" Or dt.TableName = "tblsg4" Or dt.TableName = "tblsg5" Then
                SubGrupo3 = dtG3.Rows(Me.BindingContext(ds, nTablaG3).Position).Item("cod_nivel").ToString
            Else
                SubGrupo3 = ""
            End If

            If dt.TableName = "tblsg4" Or dt.TableName = "tblsg5" Then
                SubGrupo4 = dtG4.Rows(Me.BindingContext(ds, nTablaG4).Position).Item("cod_nivel").ToString
            Else
                SubGrupo4 = ""
            End If

            If dt.TableName = "tblsg5" Then
                SubGrupo5 = dtG5.Rows(Me.BindingContext(ds, nTablaG5).Position).Item("cod_nivel").ToString
            Else
                SubGrupo5 = ""
            End If

            InsertarAuditoria(myConn, MovAud.iSeleccionar, Me.Text, "")

        End If

        Me.Close()

    End Sub
    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dg.CellDoubleClick

        Seleccion(dtG0)

    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) _
        Handles dg.RowHeaderMouseClick, dg.CellMouseClick

        Me.BindingContext(ds, nTablaG0).Position = e.RowIndex
        Posicion = Me.BindingContext(ds, nTablaG0).Position
        AsignaTXTG0(e.RowIndex, False)

    End Sub

    Private Sub dg1_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dg1.CellDoubleClick

        Seleccion(dtG1)

    End Sub

    Private Sub dg1_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg1.RowHeaderMouseClick, dg1.CellMouseClick

        Me.BindingContext(ds, nTablaG1).Position = e.RowIndex
        PosicionG1 = Me.BindingContext(ds, nTablaG1).Position
        AsignaTXTG1(e.RowIndex, False)


    End Sub

    Private Sub dg2_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dg2.CellDoubleClick

        Seleccion(dtG2)

    End Sub
    Private Sub dg2_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg2.RowHeaderMouseClick, dg2.CellMouseClick

        Me.BindingContext(ds, nTablaG2).Position = e.RowIndex
        PosicionG2 = Me.BindingContext(ds, nTablaG2).Position
        AsignaTXTG2(e.RowIndex, False)

    End Sub

    Private Sub dg3_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dg3.CellDoubleClick

        Seleccion(dtG3)

    End Sub
    Private Sub dg3_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg3.RowHeaderMouseClick, dg3.CellMouseClick

        Me.BindingContext(ds, nTablaG3).Position = e.RowIndex
        PosicionG3 = Me.BindingContext(ds, nTablaG3).Position
        AsignaTXTG3(e.RowIndex, False)

    End Sub

    Private Sub dg4_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dg4.CellDoubleClick

        Seleccion(dtG4)

    End Sub
    Private Sub dg4_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) _
        Handles dg4.RowHeaderMouseClick, dg4.CellMouseClick

        Me.BindingContext(ds, nTablaG4).Position = e.RowIndex
        PosicionG4 = Me.BindingContext(ds, nTablaG4).Position
        AsignaTXTG4(e.RowIndex, False)
    End Sub

    Private Sub dg5_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dg5.CellDoubleClick

        Seleccion(dtG5)

    End Sub
    Private Sub dg5_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg5.RowHeaderMouseClick, dg5.CellMouseClick
        Me.BindingContext(ds, nTablaG5).Position = e.RowIndex
        PosicionG5 = Me.BindingContext(ds, nTablaG5).Position
        AsignaTXTG5(e.RowIndex, False)
    End Sub

    Private Sub btnAgregarS1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarS1.Click
        Dim f As New jsNomArcGruposMovimientos
        dtG1 = ds.Tables(nTablaG1)
        f.Agregar(myConn, ds, dtG1, Grupo, 1)
        If f.Apuntador >= 0 Then AsignaTXTG1(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditarS1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarS1.Click
        Dim f As New jsNomArcGruposMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaG1).Position
        dtG1 = ds.Tables(nTablaG1)
        f.Editar(myConn, ds, dtG1, Grupo, 1)
        AsignaTXTG1(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnAgregarS2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarS2.Click
        Dim f As New jsNomArcGruposMovimientos
        dtG2 = ds.Tables(nTablaG2)
        f.Agregar(myConn, ds, dtG2, subGrupo1, 2)
        If f.Apuntador >= 0 Then AsignaTXTG2(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditarS2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarS2.Click
        Dim f As New jsNomArcGruposMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaG2).Position
        dtG2 = ds.Tables(nTablaG2)
        f.Editar(myConn, ds, dtG2, subGrupo1, 2)
        AsignaTXTG2(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnAgregarS3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarS3.Click
        Dim f As New jsNomArcGruposMovimientos
        dtG3 = ds.Tables(nTablaG3)
        f.Agregar(myConn, ds, dtG3, SubGrupo2, 3)
        If f.Apuntador >= 0 Then AsignaTXTG3(f.Apuntador, True)
        f = Nothing
    End Sub
    Private Sub btnEditarS3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarS3.Click
        Dim f As New jsNomArcGruposMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaG3).Position
        dtG3 = ds.Tables(nTablaG3)
        f.Editar(myConn, ds, dtG3, SubGrupo2, 3)
        AsignaTXTG3(f.Apuntador, True)
        f = Nothing
    End Sub
    Private Sub btnAgregarS4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarS4.Click
        Dim f As New jsNomArcGruposMovimientos
        dtG4 = ds.Tables(nTablaG4)
        f.Agregar(myConn, ds, dtG4, SubGrupo3, 4)
        If f.Apuntador >= 0 Then AsignaTXTG4(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditarS4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarS4.Click
        Dim f As New jsNomArcGruposMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaG4).Position
        dtG4 = ds.Tables(nTablaG4)
        f.Editar(myConn, ds, dtG4, SubGrupo3, 4)
        AsignaTXTG4(f.Apuntador, True)
        f = Nothing
    End Sub
    Private Sub btnAgregarS5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarS5.Click
        Dim f As New jsNomArcGruposMovimientos
        dtG5 = ds.Tables(nTablaG5)
        f.Agregar(myConn, ds, dtG5, SubGrupo4, 5)
        If f.Apuntador >= 0 Then AsignaTXTG5(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditarS5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarS5.Click
        Dim f As New jsNomArcGruposMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaG5).Position
        dtG5 = ds.Tables(nTablaG5)
        f.Editar(myConn, ds, dtG5, SubGrupo4, 5)
        AsignaTXTG5(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        Seleccion(dtG0)
    End Sub

    Private Sub btnSeleccionarS1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionarS1.Click
        Seleccion(dtG1)
    End Sub

    Private Sub btnSeleccionarS2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionarS2.Click
        Seleccion(dtG2)
    End Sub

    Private Sub btnSeleccionarS3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionarS3.Click
        Seleccion(dtG3)
    End Sub

    Private Sub btnSeleccionarS4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionarS4.Click
        Seleccion(dtG4)
    End Sub

    Private Sub btnSeleccionarS5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionarS5.Click
        Seleccion(dtG5)
    End Sub

    Private Sub btnImprimir_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

    End Sub
End Class