Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Imports Syncfusion.WinForms.ListView
Public Class jsVenArcPresupuestos
    Private Const sModulo As String = "Presupuestos"
    Private Const lRegion As String = "RibbonButton81"
    Private Const nTabla As String = "tblEncabPresupuesto"
    Private Const nTablaRenglones As String = "tblRenglones_Presupuestos"
    Private Const nTablaIVA As String = "tblIVA"
    Private Const nTablaDescuentos As String = "tblDescuentos"

    Private strSQLMov As String = ""
    Private strSQLIVA As String = ""
    Private strSQLDescuentos As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtRenglones As New DataTable
    Private dtIVA As New DataTable
    Private dtDescuentos As New DataTable
    Private dtClientes As New DataTable
    Private dtAsesores As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long, nPosicionDescuento As Long
    Private aEstatus() As String = {"Tránsito", "Procesado"}
    Private aTarifa() As String = {"A", "B", "C", "D", "E", "F"}
    Private MontoParaDescuento As Double = 0.0
    Private strSQL As String = " select a.* from jsvenenccot a " _
            & " where " _
            & " a.emision >= '" & ft.FormatoFechaMySQL(DateAdd("m", -24, jytsistema.sFechadeTrabajo)) & "' and " _
            & " a.emision <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.numcot"

    Private strSQLCliente = " select codcli, nombre, disponible, elt(estatus+1, 'Activo', 'Bloqueado', 'Inactivo', 'Desincorporado') estatus " &
        " from jsvencatcli where estatus < 3 And id_emp = '" & jytsistema.WorkID & "' order by 1 "
    Private strSQLAsesor = " select codven, CONCAT( apellidos, ', ', nombres) nombre from jsvencatven where tipo = '" & TipoVendedor.iFuerzaventa &
        "' and estatus = 1  and id_emp = '" & jytsistema.WorkID & "'  order by 1 "

    Private Impresa As Integer

    Private Sub jsVenArcPresupuestos_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsMerArcPresupuestos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            ds = DataSetRequery(ds, strSQLCliente, myConn, "tabla_clientes", lblInfo)
            ds = DataSetRequery(ds, strSQLAsesor, myConn, "tabla_asesores", lblInfo)

            dt = ds.Tables(nTabla)
            dtClientes = ds.Tables("tabla_clientes")
            dtAsesores = ds.Tables("tabla_asesores")

            ''Clientes
            sfCBCliente.DisplayMember = "nombre"
            sfCBCliente.ValueMember = "codcli"
            sfCBCliente.DataSource = dtClientes

            ''Asesores 
            sfCBAsesores.DisplayMember = "nombre"
            sfCBAsesores.ValueMember = "codven"
            sfCBAsesores.DataSource = dtAsesores

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionEncab = dt.Rows.Count - 1
                Me.BindingContext(ds, nTabla).Position = nPosicionEncab
                AsignaTXT(nPosicionEncab)
            Else
                IniciarDocumento(False)
            End If
            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

            Dim dates As SfDateTimeEdit() = {txtEmision, txtVence}
            SetSizeDateObjects(dates)

            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo presupuesto")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> presupuesto")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> Presupuesto")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> Presupuesto")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primer</B> Presupuesto")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a presupuesto <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a presupuesto <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>último presupuesto</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> Presupuesto")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnDuplicar, "<B>Duplicar</B> este presupuesto")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en Presupuesto")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón en Presupuesto")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón en Presupuesto")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en Presupuesto")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón en Presupuesto")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón en Presupuesto")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> en Presupuesto")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la Presupuesto")

        'Menu Barra Descuento 
        C1SuperTooltip1.SetToolTip(btnAgregaDescuento, "<B>Agrega </B> descuento global a presupuesto")
        C1SuperTooltip1.SetToolTip(btnEliminaDescuento, "<B>Elimina</B> descuento global de Presupuesto")



    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)

        If c >= 0 Then
            Me.BindingContext(ds, nTablaRenglones).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, c, dtRenglones.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        ft.ActivarMenuBarra(myConn, ds, dtRenglones, lRegion, MenuBarraRenglon, jytsistema.sUsuario)
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, sfCBCliente)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtCodigo.Text = .Item("numcot")
                txtEmision.Value = .Item("emision")
                txtVence.Text = .Item("vence")
                txtEstatus.Text = aEstatus(.Item("estatus"))
                sfCBCliente.SelectedValue = .Item("codcli")
                txtComentario.Text = ft.muestraCampoTexto(.Item("comen"))
                sfCBAsesores.SelectedValue = .Item("codven")
                ft.RellenaCombo(aTarifa, cmbTarifa, ft.InArray(aTarifa, .Item("tarifa")))

                tslblPesoT.Text = ft.FormatoCantidad(.Item("kilos"))

                txtSubTotal.Text = ft.FormatoNumero(.Item("tot_net"))
                txtDescuentos.Text = ft.FormatoNumero(0)
                txtCargos.Text = ft.FormatoNumero(0)
                txtTotalIVA.Text = ft.FormatoNumero(.Item("imp_iva"))
                txtTotal.Text = ft.FormatoNumero(.Item("tot_cot"))


                Impresa = .Item("impresa")

                'Renglones
                AsignarMovimientos(.Item("numcot"))

                'Totales
                CalculaTotales()

            End With
        End With
    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroPresupuesto As String)
        strSQLMov = "select * from jsvenrencot " _
                            & " where " _
                            & " numcot  = '" & NumeroPresupuesto & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"item.Item.90.I.",
                                   "descrip.Descripción.300.I.",
                                   "iva.IVA.30.C.",
                                   "cantidad.Cantidad.90.D.Cantidad",
                                   "unidad.UND.35.C.",
                                   "precio.Precio Unitario.110.D.Numero",
                                   "des_art.Desc Artículo.65.D.Numero",
                                   "des_cli.Desc Cliente.65.D.Numero",
                                   "des_ofe.Desc Oferta.65.D.Numero",
                                   "totren.Precio Total.110.D.Numero",
                                   "estatus.Tipo Renglon.100.C.",
                                   "sada..100.I."}

        ft.IniciarTablaPlus(dg, dtRenglones, aCampos)

        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "COTTemp" & ft.NumeroAleatorio(100000)
        Else
            txtCodigo.Text = ""
        End If

        sfCBCliente.SelectedValue = Nothing
        sfCBAsesores.SelectedValue = Nothing
        ft.RellenaCombo(aTarifa, cmbTarifa)
        txtComentario.Text = ""
        txtEmision.Value = sFechadeTrabajo
        txtVence.Value = sFechadeTrabajo
        txtEstatus.Text = aEstatus(0)
        tslblPesoT.Text = ft.FormatoCantidad(0)

        txtSubTotal.Text = ft.FormatoNumero(0.0)
        txtDescuentos.Text = ft.FormatoNumero(0)
        txtCargos.Text = ft.FormatoNumero(0.0)
        txtTotalIVA.Text = ft.FormatoNumero(0.0)
        txtTotal.Text = ft.FormatoNumero(0.0)

        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(txtCodigo.Text)
        CalculaTotales()


    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon, MenuDescuentos)
        ft.habilitarObjetos(True, True, txtComentario, txtEmision, txtVence, sfCBCliente, cmbTarifa, sfCBAsesores)
        If Not CBool(ParametroPlus(myConn, Gestion.iVentas, "VENCOTPA09")) Then ft.habilitarObjetos(False, True, sfCBAsesores)


        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtEmision, txtVence, txtEstatus,
                sfCBCliente, txtComentario, sfCBAsesores, cmbTarifa, txtTotalCambioEmision, txtTotalActual)

        ft.habilitarObjetos(False, True, txtDescuentos, txtCargos, MenuDescuentos, txtSubTotal, txtTotalIVA, txtTotal)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)
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

        ft.Ejecutar_strSQL(myConn, " delete from jsvenrencot where numcot = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvenivacot where numcot = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvendescot where numcot = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'COT' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado() As Boolean

        If FechaUltimoBloqueo(myConn, "jsvenenccot") >= txtEmision.Value Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If sfCBCliente.SelectedValue = Nothing Then
            ft.mensajeCritico("Debe indicar un cliente válido...")
            Return False
        End If

        If sfCBAsesores.SelectedValue = Nothing Then
            ft.mensajeCritico("Debe indicar un nombre de Asesor válido...")
            Return False
        End If

        If txtVence.Value < txtEmision.Value Then
            ft.mensajeCritico("Fecha de emision es mayor a fecha de vencimiento. Verifique ...")
            Return False
        End If

        If dtRenglones.Rows.Count = 0 Then
            ft.mensajeCritico("Debe incluir al menos un ítem...")
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
            Codigo = Contador(myConn, lblInfo, Gestion.iVentas, "VENNUMCOT", "03")
            ft.Ejecutar_strSQL(myConn, " update jsvenrencot set numcot = '" & Codigo & "' where numcot = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvenivacot set numcot = '" & Codigo & "' where numcot = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvendescot set numcot = '" & Codigo & "' where numcot = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & txtCodigo.Text & "' and origen = 'COT' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        InsertEditVENTASEncabezadoPresupuesto(myConn, lblInfo, Inserta, Codigo, txtEmision.Value, txtVence.Value,
                                               sfCBCliente.SelectedValue, txtComentario.Text, sfCBAsesores.SelectedValue, cmbTarifa.Text,
                                               ValorNumero(txtSubTotal.Text), ValorNumero(txtDescuentos.Text), ValorNumero(txtCargos.Text),
                                               ValorNumero(txtTotalIVA.Text), ValorNumero(txtTotal.Text),
                                               ft.InArray(aEstatus, txtEstatus.Text), dtRenglones.Rows.Count,
                                               0.0, ValorCantidad(tslblPesoT.Text), Impresa)


        ActualizarMovimientos(Codigo)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub ActualizarMovimientos(ByVal NumeroPresupuesto As String)

        '1.- Actualiza cantidad de presupestos en catálogo de mercancías
        ft.Ejecutar_strSQL(myConn,
                               " UPDATE jsmerctainv a, (SELECT a.item, SUM(IF( ISNULL(b.UVALENCIA), ABS(a.CANTIDAD), ABS(a.CANTIDAD/b.EQUIVALE))) cantidad, a.id_emp FROM " _
                                & "                          jsvenrencot a LEFT JOIN jsmerequmer b " _
                                & "                          ON (a.ITEM = b.CODART AND a.UNIDAD = b.UVALENCIA AND a.id_emp = b.id_emp) " _
                                & "                          WHERE " _
                                & "                          substring(a.item,1,1) <> '$' and " _
                                & "                          a.numcot = '" & NumeroPresupuesto & "' and " _
                                & "                          a.estatus <> 2 AND " _
                                & "                          a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                                & "                          a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                & "                          GROUP BY a.ITEM) b " _
                                & " set a.cotizados = b.cantidad " _
                                & " WHERE " _
                                & " a.codart =  b.item AND " _
                                & " a.id_emp = b.id_emp and  " _
                                & " a.id_emp = '" & jytsistema.WorkID & "' ")

    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique comentario ... ", Transportables.tipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, sfCBCliente)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        Dim aCamposAdicionales() As String = {"numcot|'" & txtCodigo.Text & "'",
                                              "codcli|'" & sfCBCliente.SelectedValue & "'"}
        If DocumentoBloqueado(myConn, "jsvenenccot", aCamposAdicionales) Then
            ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
        Else

            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENCOTPA01")) Then
                i_modo = movimiento.iEditar
                nPosicionEncab = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
                ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, sfCBCliente)
            Else
                ft.mensajeCritico("Edición de presupuesto no permitida...")
            End If

        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        Dim aCamposAdicionales() As String = {"numcot|'" & txtCodigo.Text & "'",
                                             "codcli|'" & sfCBCliente.SelectedValue & "'"}
        If DocumentoBloqueado(myConn, "jsvenenccot", aCamposAdicionales) Then
            ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
        Else
            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENCOTPA01")) Then
                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                nPosicionEncab = Me.BindingContext(ds, nTabla).Position

                sRespuesta = MsgBox(" ¿ Está seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")

                If sRespuesta = MsgBoxResult.Yes Then

                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numcot"))
                    ft.Ejecutar_strSQL(myConn, " delete from jsvenenccot where " _
                        & " numcot = '" & txtCodigo.Text & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "'")

                    ft.Ejecutar_strSQL(myConn, " delete from jsvenrencot where numcot = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " delete from jsvenivacot where numcot = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " delete from jsvendescot where numcot = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                    ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                    dt = ds.Tables(nTabla)
                    If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                    AsignaTXT(nPosicionEncab)

                End If
            Else
                ft.mensajeCritico("Eliminación de presupuesto no permitida...")
            End If
        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numcot", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "nombre", "Emisión", "Comentario"}
        Dim Anchos() As Integer = {150, 400, 100, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Presupuestos de mercancías...")
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

    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dg.CellFormatting
        Dim aTipoRenglon() As String = {"Normal", "Sin Desc.", "Bonificación"}
        Select Case e.ColumnIndex
            Case 10
                e.Value = aTipoRenglon(e.Value)
        End Select
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick,
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub CalculaTotales()

        txtSubTotal.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrencot", "numcot", "totren", txtCodigo.Text, 0))

        Dim discountTable As New SimpleTableProperties("jsvendescot", "numcot", txtCodigo.Text)
        MostrarDescuentosAlbaran(myConn, discountTable, dgDescuentos, txtDescuentos)

        txtCargos.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrencot", "numcot", "totren", txtCodigo.Text, 1))
        CalculaDescuentoEnRenglones()
        CalculaTotalIVAVentas(myConn, lblInfo, "jsvendescot", "jsvenivacot", "jsvenrencot", "numcot", txtCodigo.Text, "impiva", "totrendes", txtEmision.Value, "totren")

        Dim ivaTable As New SimpleTableProperties("jsvenivacot", "numcot", txtCodigo.Text)
        MostrarIVAAlbaran(myConn, ivaTable, dgIVA, txtTotalIVA)

        txtTotal.Text = ft.FormatoNumero(ValorNumero(txtSubTotal.Text) - ValorNumero(txtDescuentos.Text) + ValorNumero(txtCargos.Text) + ValorNumero(txtTotalIVA.Text))

        tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsvenrencot", "peso", "numcot", txtCodigo.Text))

        MostrarTotalesCambioMoneda(myConn, txtEmision.Value, txtTotal, txtTotalCambioEmision, txtTotalActual)

    End Sub

    Private Sub CalculaDescuentoEnRenglones()

        ft.Ejecutar_strSQL(myConn, " update jsvenrencot set totrendes = totren - totren * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
            & " where " _
            & " numcot = '" & txtCodigo.Text & "' and " _
            & " renglon > '' and " _
            & " item > '' and " _
            & " ESTATUS = '0' AND " _
            & " ACEPTADO < '2' and " _
            & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
            & " ID_EMP = '" & jytsistema.WorkID & "' ")

    End Sub



    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        If sfCBCliente.SelectedValue <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "COT", txtCodigo.Text, txtEmision.Value, , cmbTarifa.Text, sfCBCliente.SelectedValue, , , , , , , , , , sfCBAsesores.SelectedValue)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        If dtRenglones.Rows.Count > 0 Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Editar(myConn, ds, dtRenglones, "COT", txtCodigo.Text, txtEmision.Value, , cmbTarifa.Text, sfCBCliente.SelectedValue,
                     IIf(dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position).Item("item").ToString.Substring(0, 1) = "$", True, False), , , , , , , , , sfCBAsesores.SelectedValue)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            CalculaTotales()
            f = Nothing
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
                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dtRenglones.Rows(nPosicionRenglon).Item("item"))
                Dim aCamposDel() As String = {"numcot", "item", "renglon", "id_emp"}
                Dim aStringsDel() As String = {txtCodigo.Text, dtRenglones.Rows(nPosicionRenglon).Item("item"), dtRenglones.Rows(nPosicionRenglon).Item("renglon"), jytsistema.WorkID}

                nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsvenrencot", strSQLMov, aCamposDel, aStringsDel,
                                              Me.BindingContext(ds, nTablaRenglones).Position, True)

                If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1
                AsignaMov(nPosicionRenglon, True)
                CalculaTotales()
            End If
        End If

    End Sub
    Private Sub btnBuscarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarMovimiento.Click

        Dim f As New frmBuscar
        Dim Campos() As String = {"item", "descripcion"}
        Dim Nombres() As String = {"Item", "Descripción"}
        Dim Anchos() As Integer = {140, 350}
        f.Text = "Movimientos "
        f.Buscar(dtRenglones, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaRenglones).Position, " " & Me.Tag & "...")
        AsignaMov(f.Apuntador, False)
        f = Nothing

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

    Private Sub Imprimir()

        If Not CBool(ParametroPlus(myConn, Gestion.iVentas, "VENCOTPA08")) Then
            ft.mensajeCritico("Impresión de PRESUPUESTO NO permitida...")
            Exit Sub
        End If

        If DocumentoImpreso(myConn, lblInfo, "jsvenenccot", "numcot", txtCodigo.Text) AndAlso
           CBool(ParametroPlus(myConn, Gestion.iVentas, "VENCOTPA07")) Then
            ft.mensajeCritico("Este documento ya fue impreso...")
            Exit Sub
        End If

        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cPresupuesto, "Presupuesto", , txtCodigo.Text, txtEmision.Value)
        f.Dispose()
        f = Nothing

        ft.Ejecutar_strSQL(myConn, "update jsvenenccot set impresa = 1 where numcot = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub

    Private Sub sfCBCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles sfCBCliente.SelectedIndexChanged

        If i_modo = movimiento.iAgregar Then

            sfCBAsesores.SelectedValue = ft.DevuelveScalarCadena(myConn, "SELECT b.codven " _
                                                                   & " FROM jsvenrenrut a  " _
                                                                   & " LEFT JOIN jsvenencrut b ON (a.codrut = b.codrut AND a.tipo = b.tipo AND a.id_emp = b.id_emp) " _
                                                                   & " WHERE " _
                                                                   & " a.tipo = '0' AND " _
                                                                   & " a.cliente = '" & sfCBCliente.SelectedValue & "' AND " _
                                                                   & " a.id_emp = '" & jytsistema.WorkID & "' ")
        End If

    End Sub

    Private Sub btnAgregaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaDescuento.Click
        If sfCBCliente.SelectedValue <> "" Then
            If ClienteFacturaAPartirDeCostos(myConn, lblInfo, sfCBCliente.SelectedValue) Then
                ft.mensajeCritico("DESCUENTOS NO PERMITIDOS")
            Else
                Dim f As New jsGenDescuentosVentas
                f.Agregar(myConn, ds, dtDescuentos, "jsvendescot", "numcot", txtCodigo.Text, sModulo, sfCBAsesores.SelectedValue, 0,
                          txtEmision.Value, ValorNumero(txtSubTotal.Text))
                CalculaTotales()
                f = Nothing
            End If
        End If
    End Sub
    Private Sub btnEliminaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaDescuento.Click
        If nPosicionDescuento >= 0 Then
            With dtDescuentos.Rows(nPosicionDescuento)
                Dim aCamposDel() As String = {"numcot", "renglon", "id_emp"}
                Dim aStringsDel() As String = {txtCodigo.Text, .Item("renglon"), jytsistema.WorkID}
                nPosicionDescuento = EliminarRegistros(myConn, lblInfo, ds, nTablaDescuentos, "jsvendescot", strSQLDescuentos, aCamposDel, aStringsDel,
                                                      Me.BindingContext(ds, nTablaDescuentos).Position, True)
            End With
            CalculaTotales()
        End If
    End Sub
    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If sfCBCliente.SelectedValue <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "COT", txtCodigo.Text, txtEmision.Value, , cmbTarifa.Text, sfCBCliente.SelectedValue, True)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub dgDescuentos_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgDescuentos.RowHeaderMouseClick,
       dgDescuentos.CellMouseClick
        Me.BindingContext(ds, nTablaDescuentos).Position = e.RowIndex
        nPosicionDescuento = e.RowIndex
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

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCortar.Click
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
            With dtRenglones.Rows(nPosicionRenglon)
                Dim f As New jsGenCambioPrecioEnAlbaranes
                f.NuevoPrecio = .Item("precio")
                Dim PrecioAnterior As Double = f.NuevoPrecio

                f.Cambiar(myConn, ds, dtRenglones, txtCodigo.Text, sfCBCliente.SelectedValue, .Item("renglon"), .Item("item"), .Item("unidad"), .Item("cantidad"), .Item("precio"))
                If f.NuevoPrecio <> PrecioAnterior Then
                    ft.Ejecutar_strSQL(myConn, " update jsvenrencot " _
                                   & " set precio = " & f.NuevoPrecio & ", " _
                                   & " totren = " & f.NuevoPrecio * .Item("cantidad") & ", " _
                                   & " totrendes = " & f.NuevoPrecio * .Item("cantidad") * (1 - .Item("des_art") / 100) * (1 - .Item("des_cli") / 100) * (1 - .Item("des_ofe") / 100) & " " _
                                   & " where " _
                                   & " numcot = '" & txtCodigo.Text & "' and " _
                                   & " renglon = '" & .Item("renglon") & "' and " _
                                   & " item = '" & .Item("item") & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' ")
                    'f.NuevoPrecio
                    ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
                    AsignaMov(nPosicionRenglon, True)
                    CalculaTotales()
                End If

                f = Nothing
            End With
        End If
    End Sub
End Class