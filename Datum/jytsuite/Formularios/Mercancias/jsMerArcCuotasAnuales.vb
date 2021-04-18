Imports MySql.Data.MySqlClient
Public Class jsMerArcCuotasAnuales

    Private Const sModulo As String = "Cuotas Anuales de mercancías"
    Private Const lRegion As String = "RibbonButton138"
    Private Const nTabla As String = "tblCuotas"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private strSQL As String
    Private Posicion As Long
    Private BindingSource1 As New BindingSource
    Private FindField As String
    Private aCuotasTipo() As String = {"MERCANCIAS", "CATEGORIAS", "MARCAS", "DIVISIONES", "JERARQUIAS"}

    Private strUnidad As String = ""
    Private aUnidad() As String = {}


    Public Sub Cargar(ByVal Mycon As MySqlConnection)

        Me.Dock = DockStyle.Fill
        myConn = Mycon

        strUnidad = ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM07")
        If strUnidad <> "" Then
            aUnidad = {"MEDIDA PRINCIPAL (KGR)", "VENTAS", "MEDIDA SECUNDARIA (" & strUnidad & ")"}
        Else
            aUnidad = {"MEDIDA PRINCIPAL (KGR)", "VENTA"}
        End If
        ft.RellenaCombo(aUnidad, cmbUNIDAD)
        IniciarTiposCuota()
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)

        Me.Show()

    End Sub

    Private Sub IniciarCuotas(KilosUnidadCajas As Integer, TipoCuota As Integer, CodigoFiltro As String)

        '0 = Kilos ; 1 = Unidad de Venta ; 2 = Cajas

        Dim strLeft As String = ""
        Dim strLeftD As String = " LEFT JOIN (SELECT a.codart, a.unidad, a.equivale, a.uvalencia, a.id_emp " _
                    & " FROM jsmerequmer a " _
                    & " WHERE " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " UNION " _
                    & " SELECT a.codart, a.unidad, 1 equivale, a.unidad uvalencia, a.id_emp " _
                    & " FROM jsmerctainv a " _
                    & " WHERE " _
                    & " a.id_emp = '" & jytsistema.WorkID & "') d  ON (a.codart = d.codart AND d.uvalencia = '" & ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM07") & "' AND a.id_emp = d.id_emp ) "
        Dim strWhere As String = ""
        Dim strXX As String = ""

        Select Case KilosUnidadCajas
            Case 1
                strXX = "/a.pesounidad "
            Case 2
                strXX = "*d.equivale/a.pesounidad"
        End Select


        Select Case TipoCuota
            Case 1 'CATEGORIAS
                strLeft = " left join jsconctatab c on (a.grupo = c.codigo and a.id_emp = c.id_emp and c.modulo = '00002') "
                If CodigoFiltro <> "" And CodigoFiltro <> "00000" Then strWhere = " a.grupo = '" & CodigoFiltro & "' and "
            Case 2 'MARCAS
                strLeft = " left join jsconctatab c on (a.marca = c.codigo and a.id_emp = c.id_emp and c.modulo = '00003') "
                If CodigoFiltro <> "" And CodigoFiltro <> "00000" Then strWhere = " a.marca = '" & CodigoFiltro & "' and "
            Case 3 'DIVISIONES
                strLeft = " left join jsmercatdiv c on (a.division = c.division and a.id_emp = c.id_emp) "
                If CodigoFiltro <> "" And CodigoFiltro <> "00000" Then strWhere = " c.division = '" & CodigoFiltro & "' and "
            Case 4 'JERARQUIAS
                strLeft = " left join jsmerencjer c on (a.tipjer = c.tipjer and a.id_emp = c.id_emp) "
                If CodigoFiltro <> "" And CodigoFiltro <> "00000" Then strWhere = " a.tipjer = '" & CodigoFiltro & "' and "

        End Select

        strSQL = " select a.CODART, a.NOMART, if(" & KilosUnidadCajas & " = 0, 'KGR', if(" & KilosUnidadCajas & " = 1, a.unidad, d.uvalencia ) )  unidad, a.CODJER, a.TIPJER, a.EXISTE_ACT existencia, " _
                    & " b.MES01" & strXX & " MES01, b.MES02" & strXX & " MES02, b.MES03" & strXX & " MES03, " _
                    & " b.MES04" & strXX & " MES04, b.MES05" & strXX & " MES05, b.MES06" & strXX & " MES06, " _
                    & " b.MES07" & strXX & " MES07, b.MES08" & strXX & " MES08, b.MES09" & strXX & " MES09, " _
                    & " b.MES10" & strXX & " MES10, b.MES11" & strXX & " MES11, b.MES12" & strXX & " MES12,  " _
                    & " (b.MES01 + b.MES02 + b.MES03 + " _
                    & " b.MES04 + b.MES05 + b.MES06 + " _
                    & " b.MES07 + b.MES08 + b.MES09 + " _
                    & " b.MES10 + b.MES11 + b.MES12)" & strXX & "  total " _
                    & " from jsmerctainv a " _
                    & " left join jsmerctacuo b on (a.codart = b.codart and a.id_emp = b.id_emp ) " _
                    & strLeft _
                    & strLeftD _
                    & " where " _
                    & strWhere _
                    & " a.ID_EMP = '" & jytsistema.WorkID & "' order by a.CODART "

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        IniciarGrilla()

        AsignarTooltips()
        If dt.Rows.Count > 0 Then Posicion = 0
        AsignaTXT(Posicion, True)


    End Sub

    Private Sub IniciarTiposCuota()

        ft.RellenaCombo(aCuotasTipo, cmbCuota)

    End Sub

    Private Sub jsMerArcCuotasAnuales_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt = Nothing
        ds = Nothing
        ft = Nothing
    End Sub

    Private Sub jsMerArcCuotasAnuales_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        FindField = "codart"
        lblBuscar.Text = "Código"
    End Sub
    Private Sub cmbCuota_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbCuota.SelectedIndexChanged
        If cmbCuota.Items.Count > 0 Then AbrirTipoCuota(cmbCuota.SelectedIndex)
    End Sub
    Private Sub AbrirTipoCuota(TipoCuota As Integer)
        Dim nTablaFiltro As String = "tblfiltro" & ft.NumeroAleatorio(100000)
        Dim dtFilto As DataTable
        Dim strSQLFiltr As String = " select '00000' codigo, ' TODAS' descrip from jsmercatalm where codalm = '00001' and id_emp = '" & jytsistema.WorkID & "' "
        Dim strSQLFiltro As String = ""
        Select Case TipoCuota
            Case 0
                strSQLFiltro = strSQLFiltr
            Case 1 'CATEGORIAS
                strSQLFiltro = " select codigo, descrip from jsconctatab where " _
                    & " modulo = '00002' and " _
                    & " id_emp = '" & jytsistema.WorkID & "' " _
                    & " UNION " _
                    & strSQLFiltr _
                    & " order by descrip "
            Case 2 'MARCAS
                strSQLFiltro = " select codigo, descrip from jsconctatab where " _
                    & " modulo = '00003' and " _
                    & " id_emp = '" & jytsistema.WorkID & "' " _
                    & " UNION " _
                    & strSQLFiltr _
                    & " order by descrip "
            Case 3 'DIVISIONES
                strSQLFiltro = " select DIVISION codigo, descrip from jsmercatdiv where " _
                    & " id_emp = '" & jytsistema.WorkID & "' " _
                    & " UNION " _
                    & strSQLFiltr _
                    & " order by descrip "
            Case 4 'JERARQUIAS
                strSQLFiltro = " select TIPJER codigo, descrip from jsmerencjer where " _
                     & " id_emp = '" & jytsistema.WorkID & "' " _
                     & " UNION " _
                     & strSQLFiltr _
                     & " order by descrip "
        End Select
        ds = DataSetRequery(ds, strSQLFiltro, myConn, nTablaFiltro, lblInfo)
        dtFilto = ds.Tables(nTablaFiltro)

        RellenaComboConDatatable(cmbFiltro, dtFilto, "descrip", "codigo")

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


    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean, Optional ByVal NumeroCelda As Integer = 0)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        If c >= 0 AndAlso ds.Tables(nTabla).Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = c
            With dt
                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)
            End With
            dg.CurrentCell = dg(NumeroCelda, c)
        End If
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
    End Sub
    Private Sub IniciarGrilla()
        Dim aCampos() As String = {"codart", "nomart", "unidad", "mes01", "mes02", "mes03", "mes04", "mes05", "mes06", "mes07", "mes08", "mes09", "mes10", "mes11", "mes12", "total", ""}
        Dim aNombres() As String = {"Código", "Descripción", "UND", "ENE", "FEB", "MAR", "ABR", "MAY", "JUN", "JUL", "AGO", "SEP", "OCT", "NOV", "DIC", "TOTAL", ""}
        Dim aAnchos() As Integer = {90, 250, 40, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, sFormatoCantidad, ""}
        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, , True, 8)

        FindField = "codart"
        BindingSource1.DataSource = dt
        dg.DataSource = BindingSource1

        dg.ReadOnly = False
        dg.Columns("codart").ReadOnly = True
        dg.Columns("nomart").ReadOnly = True
        dg.Columns("unidad").ReadOnly = True
        dg.Columns("total").ReadOnly = True
        dg.Columns("").ReadOnly = True
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"codart", "nomart", ""}
        Dim Nombres() As String = {"Código", "Descripción", ""}
        Dim Anchos() As Integer = {150, 450, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Cuotas anuales de fuerza de venta...")
        AsignaTXT(f.Apuntador, False)
        f = Nothing
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        Posicion = Me.BindingContext(ds, nTabla).Position

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

    Private Sub dg_CellBeginEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles dg.CellBeginEdit
        If e.ColumnIndex >= 3 And e.ColumnIndex <= 14 Then
            If (String.IsNullOrEmpty(dg.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString)) Then
                ft.MensajeCritico("No Editable... Favor revisar Peso Unidad de Venta y/o Equivalencia de la Unidad Secundaria de Medida (USM) ")
                e.Cancel = True
            End If
        End If
    End Sub



    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        Posicion = Me.BindingContext(ds, nTabla).Position
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        Posicion = Me.BindingContext(ds, nTabla).Position
    End Sub

    Private Sub dg_RowValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.RowValidated
        Posicion = Me.BindingContext(ds, nTabla).Position
        If dg.CurrentCell.ColumnIndex >= 3 And dg.CurrentCell.ColumnIndex <= 14 Then
            AsignaTXT(Posicion, True, dg.CurrentCell.ColumnIndex)
        End If
    End Sub
    Private Sub dg_CancelRowEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.QuestionEventArgs) Handles dg.CancelRowEdit
        ft.mensajeAdvertencia("Cancelando")
    End Sub
    Private Sub dg_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
            Handles dg.CellValidating

        Dim headerText As String = _
            dg.Columns(e.ColumnIndex).HeaderText

        If Mid(headerText, 1, 3) = "MES" Then Return

        If Not String.IsNullOrEmpty(dg.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then
            If e.ColumnIndex >= 3 And e.ColumnIndex <= 14 Then

                If Not ft.isNumeric(e.FormattedValue.ToString()) Then
                    ft.mensajeAdvertencia("DEBE INDICAR UNA CANTIDAD VALIDA ...")
                    e.Cancel = True
                End If

            End If
        End If

    End Sub

    Private Sub dg_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dg.CellEndEdit
        dg.Rows(e.RowIndex).ErrorText = String.Empty
    End Sub
    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.ColumnHeaderMouseClick
        If e.ColumnIndex < 2 Then
            Dim aCam() As String = {"codart", "nomart"}
            Dim aStr() As String = {"Código", "Nombre"}
            FindField = dt.Columns(aCam(e.ColumnIndex)).ColumnName
            lblBuscar.Text = aStr(e.ColumnIndex)
        End If
    End Sub

    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        BindingSource1.DataSource = dt
        If dt.Columns(FindField).DataType Is GetType(String) Then _
            BindingSource1.Filter = FindField & " like '%" & txtBuscar.Text & "%'"
        dg.DataSource = BindingSource1
    End Sub

    Private Sub dg_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellValidated
        Select Case dg.CurrentCell.ColumnIndex
            Case 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14

                If Not String.IsNullOrEmpty(dg.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then

                    Dim Mes As String = "MES" & Format(dg.CurrentCell.ColumnIndex - 2, "00")
                    Dim nValorCuotas As Double = 0.0
                    Dim CodigoMercancia As String = CStr(dg.CurrentRow.Cells(0).Value)
                    Select Case cmbUNIDAD.SelectedIndex
                        Case 0 'KILOGRAMOS
                            nValorCuotas = CDbl(dg.CurrentCell.Value)
                        Case 1 'UNIDAD VENTA
                            nValorCuotas = CDbl(dg.CurrentCell.Value) * ft.DevuelveScalarDoble(myConn, " select pesounidad from jsmerctainv where codart = '" & CodigoMercancia & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        Case 2 'UNIDAD MEDIDA SECUNDARIA
                            nValorCuotas = CDbl(dg.CurrentCell.Value) / Equivalencia(myConn,  CodigoMercancia, ParametroPlus(myConn, Gestion.iMercancías, "MERPARAM07")) * ft.DevuelveScalarDoble(myConn, " select pesounidad from jsmerctainv where codart = '" & CodigoMercancia & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    End Select
                    If ft.DevuelveScalarEntero(myConn, " SELECT count(*) from jsmerctacuo where codart = '" & CodigoMercancia & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                        ft.Ejecutar_strSQL(myConn, " insert into jsmerctacuo values('" & CodigoMercancia & "', 0,0,0,0,0,0,0,0,0,0,0,0,'" & jytsistema.WorkExercise & "','" & jytsistema.WorkID & "' ) ")
                    End If
                    ft.Ejecutar_strSQL(myconn, " update jsmerctacuo set " & Mes & " = " & nValorCuotas & " " _
                                            & " where codart = '" & CodigoMercancia & "' and id_emp = '" & jytsistema.WorkID & "' ")
                End If

        End Select
    End Sub


    Private Sub cmbFiltro_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbFiltro.SelectedIndexChanged,
        cmbUNIDAD.SelectedIndexChanged
        If cmbUNIDAD.Items.Count > 0 And cmbFiltro.Items.Count > 0 And Not cmbFiltro.SelectedValue Is Nothing Then
            IniciarCuotas(cmbUNIDAD.SelectedIndex, cmbCuota.SelectedIndex, cmbFiltro.SelectedValue.ToString)
        End If

    End Sub


End Class