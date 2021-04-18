Imports MySql.Data.MySqlClient
Imports Microsoft.Win32
Public Class jsNomArcNominas
    Private Const sModulo As String = "Contrato"
    Private Const lRegion As String = "RibbonButton298"
    Private Const nTabla As String = "tblEncabNominas"
    Private Const nTablaRenglones As String = "tblRenglones_Nominas"
   
    Private strSQL As String = " select * from jsnomencnom where id_emp = '" & jytsistema.WorkID & "' order by codnom "

    Private strSQLMov As String = ""
    Private strSQLIVA As String = ""
    Private strSQLDescuentos As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtRenglones As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer

    Private nPosicionEncab As Long, nPosicionRenglon As Long

    Private FechaVencimiento As Date = jytsistema.sFechadeTrabajo
    Private MontoAnterior As Double = 0.0

    Private TarifaCliente As String = "A"

    Private Eliminados As New ArrayList

    Private Impresa As Integer

    Private Sub jsNomArcNominas_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsVenArcFacturas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionEncab = dt.Rows.Count - 1
                Me.BindingContext(ds, nTabla).Position = nPosicionEncab
                AsignaTXT(nPosicionEncab)
            Else
                IniciarDocumento(False)
            End If
            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()


        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva Nómina")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> Nómina")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> Nómina")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> Nómina")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primer</B> Nómina")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a Nómina <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a Nómina <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>último Nómina</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> Nómina")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en Nómina")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón en Nómina")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón en Nómina")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en Nómina")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón en Nómina")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón en Nómina")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> en Nómina")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la Nómina")



    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)

        If Actualiza Then
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            dtRenglones = ds.Tables(nTablaRenglones)
        End If

        If c >= 0 And dtRenglones.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaRenglones).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, c, dtRenglones.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        ft.ActivarMenuBarra(myConn, ds, dtRenglones, lRegion, MenuBarraRenglon, jytsistema.sUsuario)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        i_modo = movimiento.iConsultar

        If nRow >= 0 Then
            With dt

                nPosicionEncab = nRow
                Me.BindingContext(ds, nTabla).Position = nRow

                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

                With .Rows(nRow)
                    'Encabezado 
                    txtCodigo.Text = ft.MuestraCampoTexto(.Item("codnom"))
                    txtDescripcionNomina.Text = ft.MuestraCampoTexto(.Item("DESCRIPCION"))
                    ft.RellenaCombo(aTipoNomina, cmbTipoNomina, .Item("TIPONOM"))
                    txtDesde.Text = ft.FormatoFecha(CDate(.Item("ULT_DESDE").ToString))
                    txtHasta.Text = ft.FormatoFecha(CDate(.Item("ULT_HASTA").ToString))
                    txtCodigoContable.Text = ft.MuestraCampoTexto(.Item("codcon"))

                    'Renglones
                    AsignarMovimientos(.Item("CODNOM"))

                End With
            End With
        Else
            IniciarDocumento(False)
        End If

    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroNomina As String)

        strSQLMov = "select a.codtra, concat(b.apellidos, ', ', b.nombres) nombre, a.codcon from jsnomrennom a " _
                            & " left join jsnomcattra b on (a.codtra = b.codtra and a.id_emp = b.id_emp) " _
                            & " where " _
                            & " a.codnom  = '" & NumeroNomina & "' and " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.codtra "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"codtra", "nombre", "codcon", ""}
        Dim aNombres() As String = {"Código Trabajador", "Apellidos y Nombres", "Código Contable", ""}
        Dim aAnchos() As Integer = {100, 500, 200, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", ""}

        IniciarTabla(dg, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        txtCodigo.Text = ft.autoCodigo(myConn, "codnom", "jsnomencnom", "id_emp", jytsistema.WorkID, 5)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtDescripcionNomina, txtCodigoContable)
        txtDesde.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtHasta.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        ft.RellenaCombo(aTipoNomina, cmbTipoNomina)

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(txtCodigo.Text)

    End Sub

    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, MenuBarraRenglon)
        ft.habilitarObjetos(True, True, txtDescripcionNomina, cmbTipoNomina, btnCodigoContable)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)


    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtDescripcionNomina, txtDesde, _
                cmbTipoNomina, txtCodigoContable, btnCodigoContable)


        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then AgregaYCancela()

        If dt.Rows.Count = 0 Then
            IniciarDocumento(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            AsignaTXT(nPosicionEncab)
        End If

        DesactivarMarco0()
    End Sub
    Private Sub AgregaYCancela()

        ft.Ejecutar_strSQL(myconn, " delete from jsnomrennom where codnom = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Sub Imprimir()

    End Sub
    Private Function Validado() As Boolean


        If txtDescripcionNomina.Text = "" Then
            ft.MensajeCritico("Debe indicar una descripción de nómina válido...")
            Return False
        End If

        If dtRenglones.Rows.Count = 0 Then
            ft.MensajeCritico("Debe incluir al menos un ítem...")
            Return False
        End If


        If dtRenglones.Rows.Count <= 0 Then
            ft.MensajeCritico("Debe introducir por lo menos un ítem")
            Return False
        End If


        Return True

    End Function
    Private Sub GuardarTXT()

        Dim Codigo As String = txtCodigo.Text
        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count

        End If

        InsertEditNOMINA_NOMINA(myConn, lblInfo, Inserta, Codigo, txtDescripcionNomina.Text, cmbTipoNomina.SelectedIndex, txtCodigoContable.Text)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)


        Dim row As DataRow = dt.Select(" codnom = '" & Codigo & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)

        ActualizarMovimientos()

        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)




    End Sub
    Private Sub ActualizarMovimientos()

        '1.- Aplica Descuento Global sobre total Renglón con descuento "totrendes"
        For Each nRow As DataRow In dtRenglones.Rows
            With nRow
                AsignaConceptosATrabajador(myConn, lblInfo, ds, .Item("CODTRA"), txtCodigo.Text)
            End With
        Next

        '2.- Actualizar renglones eliminados
        For Each aSTR As Object In Eliminados
            AsignaConceptosATrabajador(myConn, lblInfo, ds, aSTR, txtCodigo.Text)
        Next



    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        i_modo = movimiento.iEditar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()


    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
     
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
       
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
        Me.Close()
    End Sub

    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dg.CellFormatting
        'Dim aTipoRenglon() As String = {"Normal", "Sin Desc.", "Bonificación"}
        'Select Case e.ColumnIndex
        '    Case 10
        '        e.Value = aTipoRenglon(e.Value)
        'End Select
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub


    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click

        If txtDescripcionNomina.Text <> "" Then
            Dim f As New jsGenListadoSeleccion
            Dim aNombres() As String = {"", "Código Trabajador", "Apellidos/Nombres Trabajador"}
            Dim aCampos() As String = {"sel", "codigo", "nombre"}
            Dim aAnchos() As Integer = {20, 120, 380}
            Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Left}
            Dim aFormato() As String = {"", "", ""}
            Dim aFields() As String = {"sel.entero.1.0", "codigo.cadena.20.0", "nombre.cadena.250.0"}

            Dim str As String = " SELECT 0 sel, codtra codigo, concat(apellidos,', ', nombres ) nombre from jsnomcattra where " _
                                & " codtra not in (SELECT codtra FROM jsnomrennom WHERE codnom = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "') AND " _
                                & " CONDICION <= 1 AND " _
                                & " ID_EMP = '" & jytsistema.WorkID & "' order by 3 "

            f.Cargar(myConn, ds, "TRABAJADORES", str, _
                    aFields, aNombres, aCampos, aAnchos, aAlineacion, aFormato)

            If f.Seleccion.Length > 0 Then
                Dim cod As String
                For Each cod In f.Seleccion
                    ft.Ejecutar_strSQL(myconn, " INSERT INTO jsnomrennom values ('" & txtCodigo.Text & "', '" & cod & "','" & txtCodigoContable.Text & "','" & jytsistema.WorkID & "') ")
                Next
            End If
            f = Nothing

            AsignarMovimientos(txtCodigo.Text)

        Else
            ft.MensajeCritico("DEBE INDICAR UNA DESCRIPCION DE NOMINA...")
        End If

    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        If dtRenglones.Rows.Count > 0 Then
            With dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position)
                Dim f As New jsNomArcNominasMovimientos
                f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
                dtRenglones = ds.Tables(nTablaRenglones)
                f.Editar(myConn, ds, dtRenglones, txtCodigo.Text, .Item("codtra"))
                AsignaMov(f.Apuntador, True)
                f = Nothing
            End With
        End If

    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click

        EliminarMovimiento()

    End Sub
    Private Sub EliminarMovimiento()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then
            sRespuesta = MsgBox(" ¿ Está seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
            If sRespuesta = MsgBoxResult.Yes Then
                With dtRenglones.Rows(nPosicionRenglon)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("codtra"))

                    Eliminados.Add(.Item("codtra"))

                    Dim aCamposDel() As String = {"codnom", "codtra", "id_emp"}
                    Dim aStringsDel() As String = {txtCodigo.Text, .Item("codtra"), jytsistema.WorkID}

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsnomrennom", strSQLMov, aCamposDel, aStringsDel, _
                                                  Me.BindingContext(ds, nTablaRenglones).Position, True)

                    If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1
                    AsignaMov(nPosicionRenglon, True)

                End With
            End If
        End If

    End Sub
    Private Sub btnBuscarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarMovimiento.Click

        'Dim f As New frmBuscar
        'Dim Campos() As String = {"item", "descripcion"}
        'Dim Nombres() As String = {"Item", "Descripción"}
        'Dim Anchos() As Integer = {140, 350}
        'f.Text = "Movimientos "
        'f.Buscar(dtRenglones, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaRenglones).Position, " " & Me.Tag & "...")
        'AsignaMov(f.Apuntador, False)
        'f = Nothing

    End Sub

    Private Sub btnPrimerMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimerMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position = 0
        AsignaMov(Me.BindingContext(ds, nTablaRenglones).Position, False)
    End Sub

    Private Sub btnAnteriorMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnteriorMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position -= 1
        AsignaMov(Me.BindingContext(ds, nTablaRenglones).Position, False)
    End Sub

    Private Sub btnSiguienteMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguienteMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position += 1
        AsignaMov(Me.BindingContext(ds, nTablaRenglones).Position, False)
    End Sub

    Private Sub btnUltimoMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimoMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position = ds.Tables(nTablaRenglones).Rows.Count - 1
        AsignaMov(Me.BindingContext(ds, nTablaRenglones).Position, False)
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Imprimir()
    End Sub

    

    Private Sub btnCodigoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigoContable.Click
        txtCodigoContable.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion " _
                                                   & " from jscotcatcon where " _
                                                   & " marca = 0 and " _
                                                   & " id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables", _
                                                   txtCodigoContable.Text)
    End Sub

    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(ds, nTablaRenglones).Position += 1
                nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
                AsignaMov(nPosicionRenglon, False)
            Case Keys.Up
                Me.BindingContext(ds, nTablaRenglones).Position -= 1
                nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
                AsignaMov(nPosicionRenglon, False)
        End Select
    End Sub

End Class