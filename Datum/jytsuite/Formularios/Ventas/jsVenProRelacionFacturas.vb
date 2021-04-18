Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Public Class jsVenProRelacionFacturas
    Private Const sModulo As String = "Relacion De Facturas"
    Private Const nTabla As String = "tblEncabRelacionFacturas"
    Private Const lRegion As String = "RibbonButton220"
    Private Const nTablaRenglones As String = "tblRenglones_RelacionFacturas"

    Private strSQL As String = "select * from jsvenencrel where tipo = 0 and ejercicio = '" & JytSistema.WorkExercise & "' and ID_EMP = '" & JytSistema.WorkID & "' order by codigoguia "

    Private strSQLMov As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtRenglones As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long

    Private aEstatus() As String = {"Activa", "Procesada"}

    Private Sub jsVenProRelacionFacturas_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsVenProRelacionFacturas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo
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

            Dim dates As SfDateTimeEdit() = {txtEmision, txtFacturaDesde, txtFacturaHasta}
            SetSizeDateObjects(dates)

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva Relación de Facturas")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> Relación de Facturas")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> Relación de Facturas")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> Relación de Facturas")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primer</B> Relación de Facturas")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a Relación de Facturas <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a Relación de Facturas <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>último Relación de Facturas</B>")

        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en Relación de Facturas")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón en Relación de Facturas")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón en Relación de Facturas")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en Relación de Facturas")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón en Relación de Facturas")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón en Relación de Facturas")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> en Relación de Facturas")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la Relación de Facturas")

    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)
        If c >= 0 Then
            Me.BindingContext(ds, nTablaRenglones).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, c, dtRenglones.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        ft.ActivarMenuBarra(myConn, ds, dtRenglones, lRegion, MenuBarraRenglon, jytsistema.sUsuario)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            If nRow >= 0 Then
                With .Rows(nRow)
                    'Encabezado 
                    txtCodigo.Text = .Item("codigoguia")
                    txtEmision.Value = .Item("fechaguia")
                    txtUsuario.Text = .Item("elaborador")
                    txtEstatus.Text = aEstatus(.Item("estatus"))
                    txtComentario.Text = ft.MuestraCampoTexto(.Item("descripcion"))
                    txtReponsable.Text = .Item("responsable")

                    tslblPesoT.Text = ft.FormatoCantidad(.Item("totalkilos"))
                    txtTotal.Text = ft.FormatoNumero(.Item("totalguia"))

                    txtFacturaDesde.Value = .Item("emisionfac")
                    txtFacturaHasta.Value = .Item("hastafac")

                    'Renglones
                    AsignarMovimientos(.Item("codigoguia"))

                    'Totales
                    CalculaTotales()

                End With
            End If

        End With
    End Sub
    Private Sub AsignarMovimientos(ByVal CodigoGuia As String)

        strSQLMov = "select *  " _
                    & " from jsvenrenrel " _
                    & " where CODIGOGUIA = '" & CodigoGuia & "' and " _
                    & " tipo = 0 and " _
                    & " ID_EMP = '" & jytsistema.WorkID & "' and " _
                    & " EJERCICIO = '" & jytsistema.WorkExercise & "' " _
                    & " order by CODVEN, CODIGOFAC "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"codigofac", "nomcli", "totalfac", "kilosfac", ""}
        Dim aNombres() As String = {"Número Factura", "Nombre Cliente", "Total Factura", "Peso Factura (Kgs)", ""}
        Dim aAnchos() As Integer = {150, 470, 120, 120, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda,
                                       AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", sFormatoNumero, sFormatoCantidad, ""}
        IniciarTabla(dg, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub


    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "RELTMP" & ft.NumeroAleatorio(100000)
        Else
            txtCodigo.Text = ""
        End If


        txtUsuario.Text = ""
        txtNombreUsuario.Text = ""
        txtComentario.Text = ""
        txtReponsable.Text = ""
        txtEstatus.Text = aEstatus(0)
        txtEmision.Value = jytsistema.sFechadeTrabajo
        tslblPesoT.Text = ft.FormatoCantidad(0)

        txtTotal.Text = ft.FormatoNumero(0.0)
        txtFacturaDesde.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtFacturaHasta.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)


        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(txtCodigo.Text)


    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon)
        ft.habilitarObjetos(True, True, txtComentario, txtReponsable, txtEmision)
        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtUsuario, txtNombreUsuario, txtReponsable,
                        txtEstatus, txtComentario)

        ft.habilitarObjetos(False, True, txtTotal)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then
            AgregaYCancela()
        Else
            If dtRenglones.Rows.Count = 0 Then
                ft.mensajeCritico("DEBE INCLUIR AL MENOS UN ITEM...")
                Return
            End If
        End If

        If dt.Rows.Count = 0 Then
            IniciarDocumento(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            AsignaTXT(nPosicionEncab)
        End If

        DesactivarMarco0()
    End Sub
    Private Sub AgregaYCancela()

        ft.Ejecutar_strSQL(myconn, " delete from jsvenrenrel where codigoguia = '" & txtCodigo.Text & "' and tipo = 0 and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado() As Boolean

        Dim aAdic() As String = {" tipo = '0' AND "}
        If FechaUltimoBloqueo(myConn, "jsvenencrel", aAdic) >= txtEmision.Value Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If dtRenglones.Rows.Count = 0 Then
            ft.MensajeCritico("Debe incluir al menos un ítem...")
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
            Codigo = Contador(myConn, lblInfo, Gestion.iVentas, "VENNUMREL", "16")

            ft.Ejecutar_strSQL(myconn, " update jsvenrenrel set codigoguia = '" & Codigo & "' where codigoguia = '" & txtCodigo.Text & "' and tipo = '0' and  id_emp = '" & jytsistema.WorkID & "' ")
        End If

        InsertEditVENTASEncabezadoRelacionDeFacturas(myConn, lblInfo, Inserta, Codigo, txtComentario.Text, jytsistema.sUsuario, txtReponsable.Text,
                                               txtEmision.Value, txtFacturaDesde.Value, txtFacturaHasta.Value,
                                               dtRenglones.Rows.Count, CDbl(txtTotal.Text), CDbl(tslblPesoT.Text), 0, 0, 0)

        ft.Ejecutar_strSQL(myConn, " UPDATE jsvenencfac a, jsvenrenrel b " _
                       & " SET a.relfacturas = b.codigoguia " _
                       & " WHERE " _
                       & " b.tipo = '0' AND " _
                       & " b.codigoguia = '" & Codigo & "' AND " _
                       & " a.id_emp = '" & jytsistema.WorkID & "' AND " _
                       & " a.emision = b.emision AND " _
                       & " a.numfac = b.codigofac AND " _
                       & " a.id_emp = b.id_emp ")

        ft.Ejecutar_strSQL(myConn, " UPDATE jsvenencncr a, jsvenrenrel b " _
                       & " SET a.relfacturas = b.codigoguia " _
                       & " WHERE " _
                       & " b.tipo = '0' AND " _
                       & " b.codigoguia = '" & Codigo & "' AND " _
                       & " a.id_emp = '" & jytsistema.WorkID & "' AND " _
                       & " a.emision = b.emision AND " _
                       & " a.numncr = b.codigofac AND " _
                       & " a.id_emp = b.id_emp ")

        ft.Ejecutar_strSQL(myConn, " UPDATE jsvenencndb a, jsvenrenrel b " _
                       & " SET a.relfacturas = b.codigoguia " _
                       & " WHERE " _
                       & " b.tipo = '0' AND " _
                       & " b.codigoguia = '" & Codigo & "' AND " _
                       & " a.id_emp = '" & jytsistema.WorkID & "' AND " _
                       & " a.emision = b.emision AND " _
                       & " a.numndb = b.codigofac AND " _
                       & " a.id_emp = b.id_emp ")


        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" CODIGOGUIA = '" & Codigo & "' AND TIPO = 0 AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab

        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique comentario ... ", Transportables.tipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim aCamposAdicionales() As String = {"codigoguia|'" & txtCodigo.Text & "'", "TIPO|'0'"}
        If DocumentoBloqueado(myConn, "jsvenencrel", aCamposAdicionales) Then
            ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
        Else
            i_modo = movimiento.iEditar
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position
            ActivarMarco0()
        End If
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        Dim aCamposAdicionales() As String = {"codigoguia|'" & txtCodigo.Text & "'", "TIPO|'0'"}
        If DocumentoBloqueado(myConn, "jsvenencrel", aCamposAdicionales) Then
            ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
        Else
            Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position

            sRespuesta = MsgBox(" ¿ Está seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")

            If sRespuesta = MsgBoxResult.Yes Then

                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("CODIGOGUIA"))
                ft.Ejecutar_strSQL(myConn, " delete from jsvenencrel where " _
                    & " codigoguia = '" & txtCodigo.Text & "' AND " _
                    & " tipo = '0' and " _
                    & " ID_EMP = '" & jytsistema.WorkID & "'")

                ft.Ejecutar_strSQL(myConn, " delete from jsvenrenrel where codigoguia = '" & txtCodigo.Text & "' and  tipo = 0 and id_emp = '" & jytsistema.WorkID & "' ")

                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                dt = ds.Tables(nTabla)
                If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                AsignaTXT(nPosicionEncab)
            End If
        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"codigoguia", "descripcion", "fechaguia"}
        Dim Nombres() As String = {"Código Relación", "Comentario", "Fecha Emisión"}
        Dim Anchos() As Integer = {150, 400, 100}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Relación de facturas...")
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
        Me.Close()
    End Sub


    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick,
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub CalculaTotales()

        txtTotal.Text = ft.FormatoNumero(ft.DevuelveScalarDoble(myConn, " select sum(totalfac) from jsvenrenrel where codigoguia = '" & txtCodigo.Text & "' and tipo = 0 and id_emp = '" & jytsistema.WorkID & "' group by codigoguia "))
        tslblPesoT.Text = ft.FormatoCantidad(ft.DevuelveScalarDoble(myConn, " select sum(kilosfac) from jsvenrenrel where codigoguia = '" & txtCodigo.Text & "' and tipo = 0 and id_emp = '" & jytsistema.WorkID & "' group by codigoguia "))

    End Sub

    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        Dim f As New jsVenProRelacionFacturasMovimientosPlus
        f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
        f.Cargar(myConn, txtFacturaDesde.Value, txtFacturaHasta.Value, txtCodigo.Text)
        AsignarMovimientos(txtCodigo.Text)
        f = Nothing
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
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("CODIGOFAC"))
                    Dim aCamposDel() As String = {"codigoguia", "codigofac", "tipo", "id_emp"}
                    Dim aStringsDel() As String = {txtCodigo.Text, .Item("codigofac"), .Item("tipo"), jytsistema.WorkID}

                    ft.Ejecutar_strSQL(myConn, " update jsvenencfac set relfacturas = '' where " _
                                   & " RELFACTURAS = '" & txtCodigo.Text & "' AND numfac = '" & .Item("codigofac") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsvenrenrel", strSQLMov, aCamposDel, aStringsDel,
                                                  Me.BindingContext(ds, nTablaRenglones).Position, True)


                    If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1
                    AsignaMov(nPosicionRenglon, True)
                    CalculaTotales()

                End With
            End If
        End If

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

    Private Sub txtUsuario_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUsuario.TextChanged
        txtNombreUsuario.Text = ft.DevuelveScalarCadena(myConn, " select nombre from jsconctausu where id_user = '" & txtUsuario.Text & "' ")
    End Sub

    Private Sub btnCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        txtUsuario.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcli codigo, nombre descripcion, disponible, elt(estatus+1, 'Activo', 'Bloqueado', 'Inactivo', 'Desincorporado') estatus from jsvencatcli where estatus < 3 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Clientes",
                                            txtUsuario.Text)
    End Sub

    Private Sub btnImprimir_Click(sender As System.Object, e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cRelacionFacturas, "Relación Facturas", , txtCodigo.Text)
        f.Dispose()
        f = Nothing
    End Sub

End Class