Imports MySql.Data.MySqlClient
Public Class jsNomArcTrabajadoresXConcepto
    Private Const sModulo As String = "Trabajadores por Concepto"
    Private Const lRegion As String = "RibbonButton35"
    Private Const nTabla As String = "conceptos"
    Private Const nTablaNominas As String = "tblNominas"
    Private Const nTablaMovimientos As String = "trabajadores_conceptos"

    Private strSQL As String = "" 
    Private strSQLMov As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private myCom As New MySqlCommand(strSQL, myConn)
    Private da As New MySqlDataAdapter(myCom)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtNomina As New DataTable
    Private dtMovimientos As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long

    Private Sub jsNomArcTrabajadoresXConcepto_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsNomArcTrabajadoresXConcepto_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            dtNomina = ft.AbrirDataTable(ds, nTablaNominas, myConn, " select codnom, concat(codnom, ' | ', descripcion) descripcion from jsnomencnom where id_emp = '" + jytsistema.WorkID + "' order by codnom  ")
            If dtNomina.Rows.Count > 0 Then
                RellenaComboConDatatable(cmbNomina, dtNomina, "descripcion", "codnom")
                AbrirPorNomina(cmbNomina.Text.Substring(0, 5))
            End If

            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnPrimero, btnSiguiente, _
                          btnAnterior, btnUltimo, btnImprimir, btnSalir, btnReprocesarEmpleado, btnProcesarConceptos, _
                          btnAgregarMovimiento, btnEditarMovimiento, btnEliminarMovimiento, btnBuscarMovimiento, _
                          btnPrimerMovimiento, btnAnteriorMovimiento, btnSiguienteMovimiento, btnUltimoMovimiento)
    End Sub
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)

        dtMovimientos = ft.MostrarFilaEnTabla(myConn, ds, nTablaMovimientos, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, _
                              jytsistema.sUsuario, nRow, Actualiza)
        AsignaSaldos(txtNomina.Text, txtCodigo.Text)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt
            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)
                'Concepto 
                txtCodigo.Text = .Item("codcon")
                txtNombre.Text = ft.muestraCampoTexto(.Item("nomcon"))

                txtNomina.Text = aTipoNomina(ft.DevuelveScalarEntero(myConn, " SELECT TIPONOM FROM jsnomencnom where codnom = '" + cmbNomina.SelectedValue + "' and id_emp = '" + jytsistema.WorkID + "' "))

                'Trabajadores
                strSQLMov = "select a.codtra, concat(b.apellidos, ', ', b.nombres) nomtra, a.importe from jsnomtrades a" _
                            & " left join jsnomcattra b on (a.codtra = b.codtra and a.id_emp = b.id_emp) " _
                            & " LEFT JOIN jsnomrennom c on (a.codtra = c.codtra AND a.codnom = c.codnom and a.id_emp = c.id_emp ) " _
                            & " where " _
                            & " c.CODNOM = '" + cmbNomina.SelectedValue + "' AND " _
                            & " b.condicion = 1 and " _
                            & " a.codcon  = '" & .Item("codcon") & "' and " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' order by 2 "

                dtMovimientos = ft.AbrirDataTable(ds, nTablaMovimientos, myConn, strSQLMov)

                Dim aCampos() As String = {"codtra.Código.90.C.", _
                                           "nomtra.Nombres y Apellidos Trabajador.600.I.", _
                                           "importe.Importe.150.D.Numero", _
                                           "sada..100.I."}
                Dim aNombres() As String = {"Código", "Nombres y Apellidos trabajador", "Importe", ""}
                Dim aAnchos() As Integer = {90, 600, 150, 100}
                Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda}
                Dim aFormatos() As String = {"", "", sFormatoNumero, ""}

                ft.IniciarTablaPlus(dg, dtMovimientos, aCampos, , True)

                Dim aEditar() As String = {"importe"}
                ft.EditarColumnasEnDataGridView(dg, aEditar)

                If dtMovimientos.Rows.Count > 0 Then
                    nPosicionRenglon = 0
                    AsignaMov(nPosicionRenglon, True)

                End If

                AsignaSaldos(.Item("CODNOM"), .Item("CODCON"))

            End With
        End With
    End Sub
    Private Sub AsignaSaldos(CodigoNomina As String, ByVal CodigoConcepto As String)

        Dim TotalConcepto As Double = ft.DevuelveScalarDoble(myConn, "select SUM(a.IMPORTE) from jsnomtrades a" _
                            & " left join jsnomcattra b on (a.codtra = b.codtra and a.id_emp = b.id_emp) " _
                            & " LEFT JOIN jsnomrennom c on (a.codtra = c.codtra AND a.codnom = c.codnom and a.id_emp = c.id_emp ) " _
                            & " where " _
                            & " c.CODNOM = '" & CodigoNomina & "' AND " _
                            & " a.codcon  = '" & CodigoConcepto & "' and " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' group by a.id_emp ")

        Dim TotalAño As Double = ft.DevuelveScalarDoble(myConn, "select SUM(a.IMPORTE) from jsnomhisdes a" _
                            & " left join jsnomcattra b on (a.codtra = b.codtra and a.id_emp = b.id_emp) " _
                            & " LEFT JOIN jsnomrennom c on (a.codtra = c.codtra AND a.codnom = c.codnom and a.id_emp = c.id_emp ) " _
                            & " where " _
                            & " c.CODNOM = '" & CodigoNomina & "' AND " _
                            & " a.desde >= '" & ft.FormatoFechaMySQL(PrimerDiaAño(jytsistema.sFechadeTrabajo)) & "' and " _
                            & " a.hasta <= '" & ft.FormatoFechaMySQL(UltimoDiaAño(jytsistema.sFechadeTrabajo)) & "' and " _
                            & " a.codcon  = '" & CodigoConcepto & "' and " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' group by a.id_emp ")

        Dim Total As Double = ft.DevuelveScalarDoble(myConn, "select SUM(a.IMPORTE) from jsnomhisdes a" _
                            & " left join jsnomcattra b on (a.codtra = b.codtra and a.id_emp = b.id_emp) " _
                            & " LEFT JOIN jsnomrennom c on (a.codtra = c.codtra AND a.codnom = c.codnom and a.id_emp = c.id_emp ) " _
                            & " where " _
                            & " c.CODNOM = '" & CodigoNomina & "' AND " _
                            & " a.codcon  = '" & CodigoConcepto & "' and " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' group by a.id_emp ")

        txtTotalConcepto.Text = ft.FormatoNumero(TotalConcepto)
        txtTotalAño.Text = ft.FormatoNumero(TotalAño)
        txtTotal.Text = ft.FormatoNumero(Total)

    End Sub
    Private Sub Actualizar(Optional ByVal bCargar As Boolean = True)
        ' Actualizar y guardar cambios  

        If Not dg.DataSource Is Nothing Then

        End If
    End Sub

    Private Sub IniciarConcepto(ByVal Inicio As Boolean)
        If Inicio Then
            txtCodigo.Text = ft.autoCodigo(myConn, "codcon", "jsnomcatcon", "codnom.id_emp", txtNomina.Text + "." + jytsistema.WorkID, 5)
        Else
            txtCodigo.Text = ""
        End If

        txtNombre.Text = ""
        txtTotalConcepto.Text = ft.FormatoNumero(0.0)
        txtTotalAño.Text = ft.FormatoNumero(0.0)
        txtTotal.Text = ft.FormatoNumero(0.0)

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        dg.Columns.Clear()
    End Sub
    Private Sub ActivarMarco0()

        grpEncab.Enabled = True
        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        ft.habilitarObjetos(False, True, txtNomina, txtCodigo, txtNombre, txtTotal, txtTotalAño, txtTotalConcepto)

        grpEncab.Enabled = True
        MenuBarra.Enabled = True
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)
    End Sub


   

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"CODCON", "NOMCON"}
        Dim Nombres() As String = {"Código Concepto", "Nombre Concepto"}
        Dim Anchos() As Integer = {100, 2500}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, " BUSCAR CONCEPTOS")
        If f.Apuntador >= 0 Then
            Me.BindingContext(ds, nTabla).Position = f.Apuntador
            AsignaTXT(f.Apuntador)
        End If
        f = Nothing
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Me.BindingContext(ds, nTabla).Position = 0
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Me.BindingContext(ds, nTabla).Position -= 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Me.BindingContext(ds, nTabla).Position += 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Actualizar(False)
        Me.Close()
    End Sub

    Private Sub dg_CellValidated(sender As Object, e As DataGridViewCellEventArgs) Handles dg.CellValidated
        If dg.CurrentCell.ColumnIndex = 2 Then

            ft.Ejecutar_strSQL(myConn, " update jsnomtrades set importe = " & CDbl(dg.CurrentCell.Value) & " " _
                            & " where " _
                            & " codtra = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' AND " _
                            & " codcon = '" & txtCodigo.Text & "' AND " _
                            & " codnom = '" & cmbNomina.SelectedValue.ToString & "' AND " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

            AsignaSaldos(cmbNomina.SelectedValue.ToString, txtCodigo.Text)

            'Conceptos_A_Trabajadores(myConn, lblInfo, ds, txtCodigo.Text, _
            '                       cmbNomina.SelectedValue, qFoundAndSign(myConn, lblInfo, "jsnomcatcon", aFld, aStr, "conjunto"), _
            '                       CInt(qFoundAndSign(myConn, lblInfo, "jsnomcatcon", aFld, aStr, "estatus")), _
            '                       qFoundAndSign(myConn, lblInfo, "jsnomcatcon", aFld, aStr, "formula"), _
            '                       qFoundAndSign(myConn, lblInfo, "jsnomcatcon", aFld, aStr, "condicion"), _
            '                       CStr(dg.CurrentRow.Cells(0).Value), _
            '                       CStr(dg.CurrentRow.Cells(0).Value))

        End If
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
    End Sub

    Private Sub btnPrimerMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimerMovimiento.Click
        Me.BindingContext(ds, nTablaMovimientos).Position = 0
        AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
    End Sub

    Private Sub btnAnteriorMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnteriorMovimiento.Click
        Me.BindingContext(ds, nTablaMovimientos).Position -= 1
        AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
    End Sub

    Private Sub btnSiguienteMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguienteMovimiento.Click
        Me.BindingContext(ds, nTablaMovimientos).Position += 1
        AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
    End Sub

    Private Sub btnUltimoMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimoMovimiento.Click
        Me.BindingContext(ds, nTablaMovimientos).Position = ds.Tables(nTablaMovimientos).Rows.Count - 1
        AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsNomRepParametros
        Dim nTipoNominas As Integer = ft.DevuelveScalarEntero(myConn, " SELECT TIPONOM FROM jsnomencnom where codnom = '" + cmbNomina.SelectedValue + "' and id_emp = '" + jytsistema.WorkID + "' ")

        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cConceptosXTrabajador, "Trabajadores, conceptos y variaciones", _
                 txtCodigo.Text, cmbNomina.SelectedValue)
        f = Nothing
    End Sub

    Private Sub dg_RowValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.RowValidated
        'If dg.CurrentCell.ColumnIndex = 2 Then

        '    ft.Ejecutar_strSQL(myConn, " update jsnomtrades set importe = " & CDbl(dg.CurrentCell.Value) & " " _
        '                    & " where " _
        '                    & " codtra = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' AND " _
        '                    & " codcon = '" & txtCodigo.Text & "' AND " _
        '                    & " codnom = '" & cmbNomina.SelectedValue.ToString & "' AND " _
        '                    & " id_emp = '" & jytsistema.WorkID & "' ")

        '    Dim aFld() As String = {"CODNOM", "codcon", "id_emp"}
        '    Dim aStr() As String = {cmbNomina.SelectedValue, txtCodigo.Text, jytsistema.WorkID}

        '    Conceptos_A_Trabajadores(myConn, lblInfo, ds, txtCodigo.Text, _
        '                           cmbNomina.SelectedValue, qFoundAndSign(myConn, lblInfo, "jsnomcatcon", aFld, aStr, "conjunto"), _
        '                           CInt(qFoundAndSign(myConn, lblInfo, "jsnomcatcon", aFld, aStr, "estatus")), _
        '                           qFoundAndSign(myConn, lblInfo, "jsnomcatcon", aFld, aStr, "formula"), _
        '                           qFoundAndSign(myConn, lblInfo, "jsnomcatcon", aFld, aStr, "condicion"), _
        '                           CStr(dg.CurrentRow.Cells(0).Value), _
        '                           CStr(dg.CurrentRow.Cells(0).Value))

        'End If

    End Sub

    Private Sub dg_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
            Handles dg.CellValidating

        Dim headerText As String = _
            dg.Columns(e.ColumnIndex).HeaderText

        If Not headerText.Equals("Importe") Then Return

        If (String.IsNullOrEmpty(e.FormattedValue.ToString())) Then
            ft.mensajeAdvertencia("Debe indicar dígito(s) válido...")
            e.Cancel = True
        End If

        If Not ft.isNumeric(e.FormattedValue.ToString()) Then
            ft.mensajeAdvertencia("Debe indicar un número válido...")
            e.Cancel = True
        End If

    End Sub

    Private Sub dg_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dg.CellEndEdit
        dg.Rows(e.RowIndex).ErrorText = String.Empty
    End Sub

    
    Private Sub btnProcesarConceptos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesarConceptos.Click

        Dim f As New jsNomProReProcesarConceptos
        f.CodigoNomina = cmbNomina.SelectedValue
        f.CodigoConcepto = txtCodigo.Text
        f.Cargar(myConn)
        f.Dispose()
        f = Nothing

        AsignaTXT(Me.BindingContext(ds, nTabla).Position)

    End Sub

    Private Sub dg_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub

    Private Sub cmbNomina_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbNomina.SelectedIndexChanged
        AbrirPorNomina(cmbNomina.SelectedValue.ToString)
    End Sub

    Public Sub AbrirPorNomina(CodigoNomina As String)

        strSQL = "select a.*, b.descripcion nombrenomina,  b.tiponom " _
                              & " from jsnomcatcon a " _
                              & " left join jsnomencnom b on (a.codnom = b.codnom and a.id_emp = b.id_emp) " _
                              & " where " _
                              & " a.codnom = '" & CodigoNomina & "' AND " _
                              & " a.id_emp = '" & jytsistema.WorkID & "' order by a.codcon "

        dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

        DesactivarMarco0()
        If dt.Rows.Count > 0 Then
            nPosicionEncab = 0
            AsignaTXT(nPosicionEncab)
        Else
            IniciarConcepto(False)
        End If

        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles btnReprocesarEmpleado.Click
        Dim npos As Long = Me.BindingContext(ds, nTablaMovimientos).Position

        Dim Conjunto As String = ft.DevuelveScalarCadena(myConn, " select CONJUNTO from jsnomcatcon " _
                                                         & " where codcon = '" & txtCodigo.Text & "' AND codnom = '" & cmbNomina.SelectedValue & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim Formula As String = ft.DevuelveScalarCadena(myConn, " select FORMULA from jsnomcatcon " _
                                                         & " where codcon = '" & txtCodigo.Text & "' AND codnom = '" & cmbNomina.SelectedValue & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim Condicion As String = ft.DevuelveScalarCadena(myConn, " select CONDICION from jsnomcatcon " _
                                                         & " where codcon = '" & txtCodigo.Text & "' AND codnom = '" & cmbNomina.SelectedValue & "' and id_emp = '" & jytsistema.WorkID & "' ")

        Conceptos_A_Trabajadores(myConn, lblInfo, ds, txtCodigo.Text, cmbNomina.SelectedValue, Conjunto, _
                                 1, Formula, Condicion, dtMovimientos.Rows(npos).Item("codtra"), dtMovimientos.Rows(npos).Item("codtra"))
        AsignaMov(npos, True)


    End Sub

End Class