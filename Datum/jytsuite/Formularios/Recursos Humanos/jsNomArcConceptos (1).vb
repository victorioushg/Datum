Imports MySql.Data.MySqlClient

Public Class jsNomArcConceptos

    Private Const sModulo As String = "Conceptos de nómina"
    Private Const lRegion As String = "RibbonButton39"
    Private Const nTabla As String = "conceptos"
    Private Const nTablaNominas As String = "nominas"
    Private strSQLConceptos As String = ""
    Private strSQLNominas As String = " select * from jsnomencnom where id_emp = '" & jytsistema.WorkID & "' order by codnom "

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtNomina As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private i As Integer
    Private Posicion As Long

    Private Sub jsNomArcConceptos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        ds = Nothing
        dt = Nothing
        myConn.Close()
        myConn = Nothing
    End Sub

    Private Sub jsNomArcConceptos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill

        Try
            myConn.Open()

            dtNomina = ft.AbrirDataTable(ds, nTablaNominas, myConn, strSQLNominas)
            RellenaComboConDatatable(cmbNomina, dtNomina, "DESCRIPCION", "CODNOM")

            If dtNomina.Rows.Count > 0 Then AbrirConceptos(dtNomina.Rows(0).Item("CODNOM"))

            AsignarTooltips()
            ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.tipoMensaje.iAyuda)

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try
    End Sub
    Private Sub AbrirConceptos(Nomina As String)
        strSQLConceptos = "select * from jsnomcatcon where CODNOM = '" & Nomina & "' and id_emp = '" & jytsistema.WorkID & "' order by codcon "
        dt = AbrirDataTable(ds, nTabla, myConn, strSQLConceptos)
        IniciarGrilla()
        If dt.Rows.Count > 0 Then Posicion = 0
        AsignaTXT(Posicion, True)
    End Sub
    Private Sub AsignarTooltips()

        'Menu Barra
        ft.colocaToolTip(C1SuperTooltip1, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnSeleccionar, btnPrimero, btnSiguiente, _
                          btnAnterior, btnUltimo, btnImprimir, btnSalir, btnDuplicar, btnProbar)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean)

        dt = ft.MostrarFilaEnTabla(myConn, ds, nTabla, strSQLConceptos, Me.BindingContext, MenuBarra, dg, lRegion, _
                                    jytsistema.sUsuario, nRow, Actualiza)

    End Sub

    Private Sub IniciarGrilla()

        Dim aCampos() As String = {"codcon", "nomcon", "tipo", "estatus", "CODNOM", "control"}
        Dim aNombres() As String = {"Código Concepto", "Descripción", "Tipo", "Estatus", "Código Nómina", ""}
        Dim aAnchos() As Integer = {100, 400, 100, 100, 100, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {"", "", "", "", "", ""}

        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        If cmbNomina.Text <> "" Then
            Dim f As New jsNomArcConceptosMovimientos
            f.CodigoNomina = cmbNomina.SelectedValue
            f.Agregar(myConn, ds, dt)
            If f.Apuntador >= 0 Then AsignaTXT(f.Apuntador, True)
            f = Nothing
        End If
    End Sub
    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        If cmbNomina.Text.Trim() <> "" Then
            Dim f As New jsNomArcConceptosMovimientos
            f.Apuntador = Me.BindingContext(ds, nTabla).Position
            f.CodigoNomina = cmbNomina.SelectedValue
            f.Editar(myConn, ds, dt)
            AsignaTXT(f.Apuntador, True)
            f = Nothing
        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        If cmbNomina.Text.Trim <> "" Then

            Posicion = Me.BindingContext(ds, nTabla).Position

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                Dim aCampos() As String = {"codcon", "id_emp", "codnom"}
                Dim aString() As String = {dt.Rows(Posicion).Item("codcon"), dt.Rows(Posicion).Item("id_emp"), dt.Rows(Posicion).Item("codnom")}
                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsnomcatcon", strSQLConceptos, aCampos, aString, Posicion), True)
            End If

        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        If dt.Rows.Count > 0 Then

            Dim f As New frmBuscar
            Dim Campos() As String = {"codcon", "nomcon"}
            Dim Nombres() As String = {"Código concepto", "Descripción"}
            Dim Anchos() As Integer = {100, 300}

            f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Conceptos de nómina...")
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
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cConceptos, "Conceptos de nómina", cmbNomina.SelectedValue)
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
                        e.Value = "Asignación"
                    Case 1
                        e.Value = "Deducción"
                    Case 2
                        e.Value = "Adicional"
                    Case 3
                        e.Value = "Especial"
                End Select
            Case "estatus"
                Select Case e.Value
                    Case 0
                        e.Value = "Inactivo"
                    Case 1
                        e.Value = "Activo"
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

    Private Sub btnDuplicarConcepto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDuplicar.Click

        If dt.Rows.Count > 0 Then
            Posicion = Me.BindingContext(ds, nTabla).Position
            If ft.Pregunta("Está seguro que desea DUPLICAR este concepto de nómina", "Duplicar registro en " & sModulo & " ...") = Windows.Forms.DialogResult.Yes Then
                If Posicion >= 0 Then
                    With dt.Rows(Posicion)
                        Dim nConcepto As String = ft.autoCodigo(myConn, "codcon", "jsnomcatcon", "id_emp", jytsistema.WorkID, 5)

                        InsertEditNOMINAConcepto(myConn, lblInfo, True, nConcepto, .Item("nomcon"), .Item("tipo"), .Item("cuota"), .Item("conjunto"), _
                                                 .Item("descripcion"), .Item("formula"), .Item("condicion"), .Item("agrupadopor"), _
                                                 .Item("codpro"), .Item("estatus"), .Item("CONCEPTO_POR_ASIG"), .Item("CODCON"), _
                                                 .Item("CODNOM"))
                        dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQLConceptos)
                        Posicion = dt.Rows.Count - 1

                    End With

                End If
            End If
            AsignaTXT(Posicion, False)
        End If

    End Sub

    Private Sub btnProbarConcepto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProbar.Click
        If dt.Rows.Count > 0 Then
            Posicion = Me.BindingContext(ds, nTabla).Position
            If Posicion >= 0 Then
                With dt.Rows(Posicion)
                    Dim CodigoConcepto As String = .Item("codcon")
                    Dim CodigoNomina As String = .Item("codnom")

                    Dim strFecha As String = "'" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "'"
                    Dim strEmpresa As String = "'" & jytsistema.WorkID & "'"
                    Dim strTrabajador As String = "'00000001'"
                    Dim str As String = Replace(Replace(Replace(ConsultaAPartirDeConcepto(myConn, ds, CodigoConcepto, "00000001", CodigoNomina, lblInfo), "@Fecha", strFecha), "@Empresa", strEmpresa), "@Trabajador", strTrabajador)

                    ft.Ejecutar_strSQL_DevuelveScalar(myConn, str)
                    Conceptos_A_Trabajadores(myConn, lblInfo, ds, CodigoConcepto, CodigoNomina, .Item("conjunto"), _
                                                 .Item("estatus"), .Item("formula"), .Item("condicion"))

                    ft.mensajeInformativo("Nomina : " & CodigoNomina & " / Concepto : " & CodigoConcepto & "...")

                End With
            End If
        End If
    End Sub

    Private Sub tlscmbNomina_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub tlscmbNomina_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbNomina.SelectedIndexChanged
        AbrirConceptos(cmbNomina.SelectedValue.ToString)
    End Sub

    Private Sub dg_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub
End Class