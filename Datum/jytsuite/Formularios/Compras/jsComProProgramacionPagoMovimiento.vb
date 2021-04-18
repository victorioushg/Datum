Imports MySql.Data.MySqlClient

Public Class jsComProProgramacionPagoMovimiento
    Private Const sModulo As String = "Pagos programados - Movimientos"
    Private Const lRegion As String = ""
    Private Const nTabla As String = "tblpagosProgramadosMovimientos"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private strSQL As String = ""

    Private FechaInicialPagos As Date
    Private FechaFinalPagos As Date
    Private Posicion As Long
    Private n_Seleccionado As ArrayList

    Private BindingSource1 As New BindingSource
    Private FindField As String

    Public Property Seleccionado() As ArrayList
        Get
            Return n_Seleccionado
        End Get
        Set(ByVal value As ArrayList)
            n_Seleccionado = value
        End Set
    End Property
    Public Property bancoSeleccionado As String

    Public Sub Cargar(ByVal Mycon As MySqlConnection, FechaInicial As Date, FechaFinal As Date)

        myConn = Mycon
        FechaInicialPagos = FechaInicial
        FechaFinalPagos = FechaFinal

        Iniciar()

        AsignarTooltips()

        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)
        Me.ShowDialog()

    End Sub
    Private Function IniciarCadena() As String

        strSQL = " SELECT 0 sel, a.codpro, b.nombre, a.nummov, a.tipomov, a.refer, a.emision, a.vence, " _
            + " DATEDIFF( '" + ft.FormatoFechaHoraMySQLFinal(jytsistema.sFechadeTrabajo) + "', a.vence )  DV, " _
            + " DATEDIFF( '" + ft.FormatoFechaHoraMySQLFinal(jytsistema.sFechadeTrabajo) + "', a.emision )  DE, " _
            + " a.importe, if( a.tipomov in ('FC','GR', 'ND'), c.saldo, 0.00) saldo, NOW() emisionch, '' banco, c.saldo aCancelar " _
            + " FROM jsprotrapag a " _
            + " LEFT JOIN jsprocatpro b ON (a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
            + " LEFT JOIN (SELECT codpro, nummov, IFNULL(SUM(IMPORTE),0) saldo FROM jsprotrapag " _
            + "            WHERE id_emp = '" + jytsistema.WorkID + "' GROUP BY codpro, nummov) c ON (a.codpro = c.codpro AND a.nummov = c.nummov ) " _
            + " INNER JOIN (SELECT codpro, nummov, MIN(CONCAT(codpro, nummov,emision,hora)) minimo FROM jsprotrapag " _
            + "             WHERE historico = '0' AND " _
            + "             ID_EMP = '" + jytsistema.WorkID + "' " _
            + "             GROUP BY codpro, nummov) d ON ( a.codpro = d.codpro AND a.nummov = d.nummov AND  CONCAT(a.codpro,a.nummov,a.emision,a.hora) = d.minimo) " _
            + " WHERE " _
            + " a.codpro <> '' AND " _
            + " a.vence <= '" + ft.FormatoFechaMySQL(FechaFinalPagos) + "' AND " _
            + " (c.saldo > 0.001 OR c.saldo < -0.001) AND 	" _
            + " a.historico = '0' AND " _
            + " a.ID_EMP = '" + jytsistema.WorkID + "' " _
            + " ORDER BY  a.codpro, dv DESC,a.nummov  "

        ft.Ejecutar_strSQL(myConn, " drop temporary table if exists " + nTabla)
        ft.Ejecutar_strSQL(myConn, " create temporary table " + nTabla + strSQL)

        Return " SELECT * FROM " + nTabla + " ORDER BY CODPRO, DV DESC, NUMMOV "

    End Function


    Private Sub Iniciar()

        Dim frm As New frmEspere
        frm.ShowDialog()

        strSQL = IniciarCadena()

        ds = DataSetRequeryPlus(ds, nTabla, myConn, strSQL)
        dt = ds.Tables(nTabla)
        BindingSource1.DataSource = dt

        Dim dtBancos As New DataTable
        dtBancos = ft.AbrirDataTable(ds, "nTablaBancosSaldos", myConn, " select codban, CONCAT(RPAD(nomban,30,' '),'|', " _
                                     + " REPLACE(REPLACE(ctaban,'-',''),')','') , '|', " _
                                     + "  LPAD( CAST(FORMAT( saldoact, 2) AS CHAR CHARACTER SET utf8),19, ' ')  ) nomban from jsbancatban where id_emp = '" + jytsistema.WorkID + "' order by nomban ")

        cmbBancos.ValueMember = "nomban"
        cmbBancos.ValueMember = "codban"
        cmbBancos.DataSource = dtBancos
        ft.habilitarObjetos(False, True, txtTotal)

        IniciarGrilla()
        Visualizar()

        frm.Dispose()
        frm = Nothing

        If dt.Rows.Count > 0 Then Posicion = 0
        MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, Posicion, True)

    End Sub

    Private Sub jsMerArcListaCostosPrecios_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt = Nothing
        ds = Nothing
    End Sub

    Private Sub jsMerArcListaCostosPrecios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        FindField = "nomart"
        lblBuscar.Text = "Nombre"
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
        C1SuperTooltip1.SetToolTip(btnTallasColores, "<B>Ver Tallas y Colores</B> de este registro")

        MenuBarra.ImageList = ImageList1

        btnAgregar.Image = ImageList1.Images(0)
        btnEditar.Image = ImageList1.Images(1)
        btnEliminar.Image = ImageList1.Images(2)
        btnBuscar.Image = ImageList1.Images(3)
        btnSeleccionar.Image = ImageList1.Images(4)
        btnPrimero.Image = ImageList1.Images(6)
        btnAnterior.Image = ImageList1.Images(7)
        btnSiguiente.Image = ImageList1.Images(8)
        btnUltimo.Image = ImageList1.Images(9)
        btnImprimir.Image = ImageList1.Images(10)
        btnSalir.Image = ImageList1.Images(11)
        btnTallasColores.Image = ImageList1.Images(12)

        btnTallasColores.Visible = False

    End Sub

    Private Sub IniciarGrilla()

        Dim aCamposEdicion() As String = {}
        Dim aCampos() As String = {"codpro.Código.100.I.", _
                                   "nombre.Nombre y/o razón social.250.I.", _
                                   "nummov.Documento.100.I.", _
                                   "tipomov.TP.30.C.", _
                                   "vence.Fecha Vencimiento.90.C.fecha", _
                                   "dv.Desde Vencimiento.60.C.entero.1.0", _
                                   "de.Desde Emisión.60.C.entero.1.0", _
                                   "importe.Importe inicial.120.D.Numero", _
                                   "saldo.Saldo.120.D.Numero", _
                                   "aCancelar.A Cancelar.120.D.Numero"}

        ft.IniciarTablaPlus(dg, dt, aCampos, , True, , , , , False)
        aCamposEdicion = {"sel", "aCancelar"}
        ft.EditarColumnasEnDataGridView(dg, aCamposEdicion)
        
        ArreglaAnchoVentana()


    End Sub
    Private Sub Visualizar()
        grpLista.Top = MenuBarra.Height
        grpLista.Height = Me.Height - MenuBarra.Height - grpTotal.Height - (Me.Height - lblInfo.Top + 5)
        grpTotal.Top = MenuBarra.Height + grpLista.Height
    End Sub
    Private Sub ArreglaAnchoVentana()

        Dim anchoVentana As Integer = 0
        For Each dgCol As DataGridViewColumn In dg.Columns
            anchoVentana += dgCol.Width
        Next
        Me.Width = anchoVentana + 50
        grpLista.Width = Me.Width - 5
        dg.Width = Me.Width - 10

    End Sub


    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        'Dim f As New frmBuscar
        'Dim Campos() As String = {"codart", "nomart"}
        'Dim Nombres() As String = {"Código", "Descripción"}
        'Dim Anchos() As Integer = {100, 350}
        'f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Listado de Costos/precios...")
        'Posicion = f.Apuntador
        'MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, Posicion, False)
        'f = Nothing
    End Sub
    Private Function Validado()
        For Each nRow As DataRow In dt.Rows
            If CBool(nRow.Item("Sel")) AndAlso nRow.Item("aCancelar") > 0 Then
                ft.mensajeCritico("Existen FILAS Seleccionadas y no poseen monto en A Cancelar...")
                Return False
            End If
        Next
        Return True
    End Function
    Private Function seleccion() As ArrayList
        seleccion = New ArrayList
        For Each nRow As DataRow In dt.Rows
            If CBool(nRow.Item("Sel")) Then seleccion.Add(nRow)
        Next
    End Function
    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        Posicion = Me.BindingContext(ds, nTabla).Position
        If dt.Rows.Count > 0 AndAlso dg.RowCount > 0 Then
            If Validado() Then
                Seleccionado = seleccion()
                InsertarAuditoria(myConn, MovAud.iSeleccionar, Me.Text, "")
            End If
        End If
        bancoSeleccionado = cmbBancos.SelectedValue
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        If dg.RowCount > 0 Then
            Me.BindingContext(ds, nTabla).Position = 0
            Posicion = 0
            MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, Posicion, False)
        End If
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        If dg.RowCount > 0 Then
            Me.BindingContext(ds, nTabla).Position -= 1
            Posicion = Me.BindingContext(ds, nTabla).Position
            MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, Posicion, False)
        End If
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        If dg.RowCount > 0 Then
            Me.BindingContext(ds, nTabla).Position += 1
            Posicion = Me.BindingContext(ds, nTabla).Position
            MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, Posicion, False)
        End If
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        If dg.RowCount > 0 Then
            Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
            Posicion = Me.BindingContext(ds, nTabla).Position
            MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, Posicion, False)
        End If
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Seleccionado = Nothing
        bancoSeleccionado = ""
        Me.Close()
    End Sub

    'Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
    '    Posicion = Me.BindingContext(ds, nTabla).Position
    '    If dt.Rows.Count > 0 AndAlso dg.RowCount > 0 Then
    '        Seleccionado = seleccion()
    '        InsertarAuditoria(myConn, MovAud.iSeleccionar, Me.Text, "")
    '    End If
    '    bancoSeleccionado = cmbBancos.SelectedValue
    '    Me.Close()

    'End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.CellMouseClick
        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        Posicion = Me.BindingContext(ds, nTabla).Position

    End Sub

    Private Sub dg_RegionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.RegionChanged
        Posicion = Me.BindingContext(ds, nTabla).Position
    End Sub

    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        BindingSource1.DataSource = dt
        If dt.Columns(FindField).DataType Is GetType(String) Then _
            BindingSource1.Filter = FindField & " like '%" & txtBuscar.Text & "%'"
        dg.DataSource = BindingSource1
    End Sub

    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.ColumnHeaderMouseClick

        'Dim aCam() As String = {"codart", "nomart", "iva", "existencia", "unidad", "precio_A_iva", "alterno", "BARRAS"}
        'Dim aStr() As String = {"Código", "Descripción", "IVA", "Existencia", "UND", "Precio A + IVA ", "Código ALTERNO", "CODIGO BARRAS"}
        '    If e.ColumnIndex < 2 Or e.ColumnIndex > 5 Then
        '        FindField = dt.Columns(aCam(e.ColumnIndex)).ColumnName
        '        lblBuscar.Text = aStr(e.ColumnIndex)
        '    End If

        'txtBuscar.Focus()

    End Sub


    Private Sub jsMerArcListaCostosPrecios_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        txtBuscar.Focus()
    End Sub


    Private Sub dg_CellMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.CellMouseUp
        dg.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub
    Private Sub dg_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellValidated

        If dg.RowCount = dt.Rows.Count Then
            Select Case dg.CurrentCell.ColumnIndex
                Case 0

                    ft.Ejecutar_strSQL(myConn, " UPDATE  " + nTabla + " set sel  = " + Convert.ToInt16(dg.CurrentCell.Value).ToString + " " _
                                         + " where codpro = '" + CStr(dg.CurrentRow.Cells(1).Value) + "' and  " _
                                                              + " nummov = '" + CStr(dg.CurrentRow.Cells(3).Value) + "' ")
                Case 10
                    ft.Ejecutar_strSQL(myConn, " update " + nTabla + " set aCancelar = " + CDbl(dg.CurrentCell.Value).ToString + " " _
                                                              + " where codpro = '" + CStr(dg.CurrentRow.Cells(1).Value) + "' and  " _
                                                              + " nummov = '" + CStr(dg.CurrentRow.Cells(3).Value) + "' ")

                    Posicion = Me.BindingContext(ds, nTabla).Position
                    'MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, Posicion, True)

                    If CDbl(dg.CurrentCell.Value) < 0 Then dt.Rows(e.RowIndex).Item(0) = True

            End Select
        End If

    End Sub
    Private Sub dg_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick
        If e.ColumnIndex = 0 Then
            dt.Rows(e.RowIndex).Item(0) = Not CBool(dt.Rows(e.RowIndex).Item(0).ToString)
            txtTotal.Text = ft.muestraCampoNumero(CalculaTotales())
        End If

    End Sub
    Private Function CalculaTotales()
        Dim nTotal As Double = 0.0
        For Each nRow As DataRow In dt.Rows
            If Convert.ToBoolean(nRow(0)) Then nTotal += Convert.ToDouble(nRow.Item("acancelar"))
        Next
        Return nTotal
    End Function
    Private Sub dg_CellValidating(sender As Object, e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles dg.CellValidating

        Dim headerText As String = _
            dg.Columns(e.ColumnIndex).HeaderText

        If Not (headerText.Equals("A Cancelar")) Then Return

        If e.ColumnIndex = 10 Then
            If (String.IsNullOrEmpty(e.FormattedValue.ToString())) Then
                ft.mensajeAdvertencia("Debe indicar dígito(s) válido...")
                e.Cancel = True
            End If

            If Not ft.isNumeric(e.FormattedValue.ToString()) Then
                ft.mensajeAdvertencia("Debe indicar un número válido...")
                e.Cancel = True
            End If

            If ValorNumero(e.FormattedValue.ToString()) < dg.Rows(Posicion).Cells(9).Value Then
                ft.mensajeCritico("El monto A Cancelar no puede ser mayor que el Saldo")
                e.Cancel = True
            End If
        End If

    End Sub
End Class