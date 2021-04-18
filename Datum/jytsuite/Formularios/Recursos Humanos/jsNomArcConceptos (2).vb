Imports MySql.Data.MySqlClient

Public Class jsNomArcConceptos
    Private Const sModulo As String = "Conceptos de nómina"
    Private Const lRegion As String = "RibbonButton39"
    Private Const nTabla As String = "conceptos"
    Private strSQLConceptos As String = "select * from jsnomcatcon where id_emp = '" & jytsistema.WorkID & "' order by codcon "

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable

    Private i_modo As Integer
    Private i As Integer
    Private Posicion As Long

    Private Sub jsNomArcConceptos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ds = Nothing
        dt = Nothing
        myConn.Close()
        myConn = Nothing
    End Sub

    Private Sub jsNomArcConceptos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill

        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQLConceptos, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)


            AsignarTooltips()
            IniciarGrilla()
            If dt.Rows.Count > 0 Then Posicion = 0
            AsignaTXT(Posicion, True)
            MensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", TipoMensaje.iAyuda)

        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try
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

        C1SuperTooltip1.SetToolTip(btnDuplicarConcepto, "<B>Duplica</B> el concepto señalado")
        C1SuperTooltip1.SetToolTip(btnProbarConcepto, "<B>Prueba</B> la sintaxis del concepto señalado")

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLConceptos, myConn, nTabla, lblInfo)
        If c >= 0 AndAlso ds.Tables(nTabla).Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = c
            With dt
                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)
            End With
            dg.CurrentCell = dg(0, c)
        End If
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
    End Sub

    Private Sub IniciarGrilla()

        Dim aCampos() As String = {"codcon", "nomcon", "tipo", "estatus", "CODNOM", "control"}
        Dim aNombres() As String = {"Código Concepto", "Descripción", "Tipo", "Estatus", "Código Nómina", ""}
        Dim aAnchos() As Long = {100, 400, 100, 100, 100, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {"", "", "", "", "", ""}

        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim f As New jsNomArcConceptosMovimientos
        f.Agregar(myConn, ds, dt)
        If f.Apuntador >= 0 Then AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub
    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        Dim f As New jsNomArcConceptosMovimientos
        f.Apuntador = Me.BindingContext(ds, nTabla).Position
        f.Editar(myConn, ds, dt)
        AsignaTXT(f.Apuntador, True)
        f = Nothing

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Dim sRespuesta As Integer
        Posicion = Me.BindingContext(ds, nTabla).Position
        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", vbYesNo, "Eliminar registro en " & sModulo & " ...")
        If sRespuesta = vbYes Then
            Dim aCampos() As String = {"codcon", "id_emp"}
            Dim aString() As String = {dt.Rows(Posicion).Item("codcon"), dt.Rows(Posicion).Item("id_emp")}
            AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsnomcatcon", strSQLConceptos, aCampos, aString, Posicion), True)
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"concon", "nomcon"}
        Dim Nombres() As String = {"Código concepto", "Descripción"}
        Dim Anchos() As Long = {100, 300}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Conceptos de nómina...")
        AsignaTXT(f.Apuntador, False)
        f = Nothing
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        '
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

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsNomRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteNomina.cConceptos, "Conceptos de nómina")
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

        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        AsignaTXT(e.RowIndex, False)
    End Sub

    Private Sub btnDuplicarConcepto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDuplicarConcepto.Click
        Dim sRespuesta As Integer
        Posicion = Me.BindingContext(ds, nTabla).Position
        sRespuesta = MsgBox(" ¿ Esta Seguro que desea DUPLICAR este concepto de nómina ?", vbYesNo, "Duplicar registro en " & sModulo & " ...")
        If sRespuesta = vbYes Then
            If Posicion >= 0 Then
                With dt.Rows(Posicion)
                    Dim nConcepto As String = AutoCodigo(5, ds, nTabla, "codcon")
                    InsertEditNOMINAConcepto(myConn, lblInfo, True, nConcepto, .Item("nomcon"), .Item("tipo"), .Item("cuota"), .Item("conjunto"), _
                        .Item("formula"), .Item("condicion"), .Item("agrupadopor"), .Item("codpro"), .Item("estatus"), .Item("CONCEPTO_POR_ASIG"), .Item("CODCON"), .Item("CODNOM"))
                    ds = DataSetRequery(ds, strSQLConceptos, myConn, nTabla, lblInfo)
                    dt = ds.Tables(nTabla)
                    Posicion = dt.Rows.Count - 1
                End With

            End If
        End If
        AsignaTXT(Posicion, False)
    End Sub

    Private Sub btnProbarConcepto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProbarConcepto.Click
        If Posicion >= 0 Then

            Dim CodigoConcepto As String = dt.Rows(Posicion).Item("codcon")
            Dim CodigoNomina As String = dt.Rows(Posicion).Item("codnom")

            Dim strFecha As String = "'" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "'"
            Dim strEmpresa As String = "'" & jytsistema.WorkID & "'"
            Dim strTrabajador As String = "'00000001'"
            Dim str As String = Replace(Replace(Replace(ConsultaAPartirDeConcepto(myConn, ds, CodigoConcepto, "00000001", CodigoNomina, lblInfo), "@Fecha", strFecha), "@Empresa", strEmpresa), "@Trabajador", strTrabajador)
            EjecutarSTRSQL_Scalar(myConn, lblInfo, str)
            MensajeInformativo(lblInfo, "Prueba Terminada...")
        End If

    End Sub
End Class