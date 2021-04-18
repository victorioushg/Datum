Imports MySql.Data.MySqlClient
Public Class jsNomArcTrabajadoresXConcepto
    Private Const sModulo As String = "Trabajadores por Concepto"
    Private Const lRegion As String = "RibbonButton35"
    Private Const nTabla As String = "conceptos"
    Private Const nTablaMovimientos As String = "trabajadores_conceptos"

    Private strSQL As String = "select a.*, b.descripcion nombrenomina,  b.tiponom " _
                               & " from jsnomcatcon a " _
                               & " left join jsnomencnom b on (a.codnom = b.codnom and a.id_emp = b.id_emp) " _
                               & " where " _
                               & " a.CODNOM <> '' AND " _
                               & " a.id_emp = '" & jytsistema.WorkID & "' order by a.codcon "
    Private strSQLMov As String

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private myCom As New MySqlCommand(strSQL, myConn)
    Private da As New MySqlDataAdapter(myCom)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtMovimientos As New DataTable

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long

    Private Sub jsNomArcTrabajadoresXConcepto_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)


            '  RellenaCombo(aTipoNomina, cmbTipo)


            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionEncab = 0
                AsignaTXT(nPosicionEncab)
            Else
                IniciarConcepto(False)
            End If
            ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo concepto ")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> concepto actual")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> concepto actual")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> concepto deseada")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir al <B>primer</B> concepto")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir al <B>siguiente</B> concepto")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir <B>anterior</B> concepto")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir al <B>último concepto</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> trabajadores por concepto")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnProcesarConceptos, "<B>Re-Procesar</B> conceptos a trabajadores ...")


        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> trabajador al concepto")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> trabajador ")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> trabajador ")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> trabajador ")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> trabajador ")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> trabajador ")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al trabajador <B>siguiente </B>")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> trabajador ")


    End Sub
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        End If

        If c >= 0 Then
            Me.BindingContext(ds, nTablaMovimientos).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", c, dtMovimientos.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        AsignaSaldos(txtNomina.Text, txtCodigo.Text)

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtMovimientos, MenuBarraRenglon)
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt
            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

            With .Rows(nRow)
                'Concepto 
                txtCodigo.Text = .Item("codcon")
                txtNombre.Text = IIf(IsDBNull(.Item("nomcon")), "", .Item("nomcon"))
                txtNomina.Text = MuestraCampoTexto(.Item("CODNOM"))
                lblNomina.Text = MuestraCampoTexto(.Item("NOMBRENOMINA"))
                RellenaCombo(aTipoNomina, cmbTipo, .Item("TIPONOM"))

                'Trabajadores
                strSQLMov = "select a.codtra, concat(b.apellidos, ', ', b.nombres) nomtra, a.importe from jsnomtrades a" _
                            & " left join jsnomcattra b on (a.codtra = b.codtra and a.id_emp = b.id_emp) " _
                            & " LEFT JOIN jsnomrennom c on (a.codtra = c.codtra and a.id_emp = c.id_emp ) " _
                            & " where " _
                            & " c.CODNOM = '" & .Item("codnom") & "' AND " _
                            & " b.condicion = 1 and " _
                            & " a.codcon  = '" & .Item("codcon") & "' and " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' order by 2 "

                ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                dtMovimientos = ds.Tables(nTablaMovimientos)

                Dim aCampos() As String = {"codtra", "nomtra", "importe", ""}
                Dim aNombres() As String = {"Código", "Nombres y Apellidos trabajador", "Importe", ""}
                Dim aAnchos() As Long = {90, 600, 150, 100}
                Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda}
                Dim aFormatos() As String = {"", "", sFormatoNumero, ""}

                IniciarTabla(dg, dtMovimientos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, , True)

                dg.ReadOnly = False
                dg.Columns("codtra").ReadOnly = True
                dg.Columns("nomtra").ReadOnly = True
                dg.Columns("").ReadOnly = True

                If dtMovimientos.Rows.Count > 0 Then
                    nPosicionRenglon = 0
                    AsignaMov(nPosicionRenglon, True)

                End If

                AsignaSaldos(.Item("CODNOM"), .Item("CODCON"))

            End With
        End With
    End Sub
    Private Sub AsignaSaldos(CodigoNomina As String, ByVal CodigoConcepto As String)

        Dim TotalConcepto As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, "select SUM(a.importe) from jsnomtrades a" _
                            & " left join jsnomcattra b on (a.codtra = b.codtra and a.id_emp = b.id_emp) " _
                            & " LEFT JOIN jsnomrennom c on (a.codtra = c.codtra and a.id_emp = c.id_emp ) " _
                            & " where " _
                            & " c.CODNOM = '" & CodigoNomina & "' AND " _
                            & " a.codcon  = '" & CodigoConcepto & "' and " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' group by a.id_emp "))

        Dim TotalAño As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, "select SUM(a.importe) from jsnomhisdes a" _
                            & " left join jsnomcattra b on (a.codtra = b.codtra and a.id_emp = b.id_emp) " _
                            & " LEFT JOIN jsnomrennom c on (a.codtra = c.codtra and a.id_emp = c.id_emp ) " _
                            & " where " _
                            & " c.CODNOM = '" & CodigoNomina & "' AND " _
                            & " a.desde >= '" & FormatoFechaMySQL(PrimerDiaAño(jytsistema.sFechadeTrabajo)) & "' and " _
                            & " a.hasta <= '" & FormatoFechaMySQL(UltimoDiaAño(jytsistema.sFechadeTrabajo)) & "' and " _
                            & " a.codcon  = '" & CodigoConcepto & "' and " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' group by a.id_emp "))

        Dim Total As Double = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, "select SUM(a.importe) from jsnomhisdes a" _
                            & " left join jsnomcattra b on (a.codtra = b.codtra and a.id_emp = b.id_emp) " _
                            & " LEFT JOIN jsnomrennom c on (a.codtra = c.codtra and a.id_emp = c.id_emp ) " _
                            & " where " _
                            & " c.CODNOM = '" & CodigoNomina & "' AND " _
                            & " a.codcon  = '" & CodigoConcepto & "' and " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' group by a.id_emp "))

        txtTotalConcepto.Text = FormatoNumero(TotalConcepto)
        txtTotalAño.Text = FormatoNumero(TotalAño)
        txtTotal.Text = FormatoNumero(Total)

    End Sub
    Private Sub Actualizar(Optional ByVal bCargar As Boolean = True)
        ' Actualizar y guardar cambios  

        If Not dg.DataSource Is Nothing Then

        End If
    End Sub

    Private Sub IniciarConcepto(ByVal Inicio As Boolean)
        If Inicio Then
            txtCodigo.Text = AutoCodigo(5, ds, nTabla, "codcon")
        Else
            txtCodigo.Text = ""
        End If

        txtNombre.Text = ""
        txtTotalConcepto.Text = FormatoNumero(0.0)
        txtTotalAño.Text = FormatoNumero(0.0)
        txtTotal.Text = FormatoNumero(0.0)

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", 0, 0)
        dg.Columns.Clear()
    End Sub
    Private Sub ActivarMarco0()
       
        grpEncab.Enabled = True
        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        HabilitarObjetos(False, True, txtNomina, txtCodigo, txtNombre, cmbTipo, txtTotal, txtTotalAño, txtTotalConcepto)

        grpEncab.Enabled = True
        MenuBarra.Enabled = True
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
    End Sub

   
    Private Sub GuardarTXT()

        '  Dim Inserta As Boolean = False
        ' If i_modo = movimiento.iAgregar Then
        'Inserta = True
        'nPosicionEncab = ds.Tables(nTabla).Rows.Count
        'End If

        'InsertEditBANCOSEncabezadoCaja(myConn, Inserta, txtCodigo.Text, txtNombre.Text, ValorNumero(txtSaldo.Text))

        'ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblinfo)
        'dt = ds.Tables(nTabla)
        'Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        'AsignaTXT(nPosicionEncab)
        'DesactivarMarco0()
        'ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        ' i_modo = movimiento.iAgregar
        ' nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ' ActivarMarco0()
        ' IniciarConcepto(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        'i_modo = movimiento.iEditar
        'nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        'ActivarMarco0()
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        ' If dt.Rows(Me.BindingContext(ds, nTabla).Position).Item("caja") <> "00" Then
        '
        '       End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"CODCON", "NOMCON"}
        Dim Nombres() As String = {"Código Concepto", "Nombre Concepto"}
        Dim Anchos() As Long = {100, 2500}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, " BUSCAR CONCEPTOS")
        AsignaTXT(f.Apuntador)
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
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "ItemsRenglon", "lblItemsRenglon", e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
    End Sub


    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click

        ' Dim f As New jsBanArcCajasMovimientos
        ' f.Apuntador = Me.BindingContext(ds, nTablaMovimientos).Position
        ' f.Agregar(myConn, ds, dtMovimientos, txtCodigo.Text)
        ' ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        ' AsignaMov(f.Apuntador, True)
        ' f = Nothing

    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        'Dim f As New jsBanArcCajasMovimientos
        'f.Apuntador = Me.BindingContext(ds, nTablaMovimientos).Position
        'f.Editar(myConn, ds, dtMovimientos, txtCodigo.Text)
        'ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        'AsignaMov(f.Apuntador, True)
        'f = Nothing

    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click

        'EliminarMovimiento()

    End Sub
    Private Sub btnBuscarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarMovimiento.Click

        '       Dim f As New frmBuscar
        '       Dim Campos() As String = {"fecha", "nummov", "formpag", "numpag", "refpag", "prov_cli", "codven", "importe"}
        '       Dim Nombres() As String = {"Emisión", "Nº Movimiento", "Forma Pago", "Número Pago", "Referencia Pago", "Proveedor/Cliente", "Asesor Comercial", "Importe"}
        '      Dim Anchos() As Long = {140, 140, 100, 150, 150, 150, 120, 150}
        '       f.Text = "Movimientos de caja"
        '      f.Buscar(dtMovimientos, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaMovimientos).Position)
        '       AsignaMov(f.Apuntador, False)
        '       f = Nothing

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

    Private Sub btnRemesas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsNomRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cConceptosXTrabajador, "Trabajadores, conceptos y variaciones", txtCodigo.Text, , , cmbTipo.SelectedIndex)
        f = Nothing
    End Sub

    Private Sub dg_RowValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.RowValidated
        If dg.CurrentCell.ColumnIndex = 2 Then

            EjecutarSTRSQL(myConn, lblInfo, " update jsnomtrades set importe = " & CDbl(dg.CurrentCell.Value) & " " _
                            & " where codtra = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' and codcon = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

            Dim aFld() As String = {"CODNOM", "codcon", "id_emp"}
            Dim aStr() As String = {txtNomina.Text, txtCodigo.Text, jytsistema.WorkID}

            Conceptos_A_Trabajadores(myConn, lblInfo, ds, txtCodigo.Text, _
                                   txtNomina.Text, qFoundAndSign(myConn, lblInfo, "jsnomcatcon", aFld, aStr, "conjunto"), _
                                   CInt(qFoundAndSign(myConn, lblInfo, "jsnomcatcon", aFld, aStr, "estatus")), _
                                   qFoundAndSign(myConn, lblInfo, "jsnomcatcon", aFld, aStr, "formula"), _
                                   qFoundAndSign(myConn, lblInfo, "jsnomcatcon", aFld, aStr, "condicion"), _
                                   CStr(dg.CurrentRow.Cells(0).Value), _
                                   CStr(dg.CurrentRow.Cells(0).Value))

        End If
    End Sub

    Private Sub dg_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
            Handles dg.CellValidating

        Dim headerText As String = _
            dg.Columns(e.ColumnIndex).HeaderText

        If Not headerText.Equals("Importe") Then Return

        If (String.IsNullOrEmpty(e.FormattedValue.ToString())) Then
            MensajeAdvertencia(lblInfo, "Debe indicar dígito(s) válido...")
            e.Cancel = True
        End If

        If Not IsNumeric(e.FormattedValue.ToString()) Then
            MensajeAdvertencia(lblInfo, "Debe indicar un número válido...")
            e.Cancel = True
        End If

    End Sub

    Private Sub dg_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dg.CellEndEdit
        dg.Rows(e.RowIndex).ErrorText = String.Empty
    End Sub

    'Private Sub cmbTipo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipo.SelectedIndexChanged
    '    AsignaTXT(Me.BindingContext(ds, nTabla).Position)
    'End Sub


    Private Sub btnProcesarConceptos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProcesarConceptos.Click

        Dim f As New jsNomProReProcesarConceptos
        f.Cargar(myConn)
        f.Dispose()
        f = Nothing

    End Sub

    Private Sub dg_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub
End Class