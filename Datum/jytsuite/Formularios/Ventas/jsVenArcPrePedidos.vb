Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Public Class jsVenArcPrePedidos
    Private Const sModulo As String = "Pre-Pedidos"
    Private Const lRegion As String = "RibbonButton214"
    Private Const nTabla As String = "tblEncabPre-Pedidos"
    Private Const nTablaRenglones As String = "tblRenglones_Pre-Pedidos"
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
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long, nPosicionDescuento As Long
    Private aEstatus() As String = {"BackOrder", "Procesado"}
    Private aTarifa() As String = {"A", "B", "C", "D", "E", "F"}
    Private CondicionDePago As Integer = 0
    Private TipoCredito As Integer = 0
    Private FechaVencimiento As Date = jytsistema.sFechadeTrabajo
    Private MontoAnterior As Double = 0.0
    Private MontoParaDescuento As Double = 0.0
    Private Disponibilidad As Double = 0.0
    Private TarifaCliente As String = "A"
    Private strSQL As String = " (select a.*, b.nombre from jsvenencpedrgv a " _
            & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
            & " where " _
            & " a.emision >= '" & ft.FormatoFechaMySQL(DateAdd("m", -MesesAtras.i12, jytsistema.sFechadeTrabajo)) & "' and " _
            & " a.emision <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.emision desc,a.numped desc) order by emision, numped "

    Private Impresa As Integer

    Private Sub jsVenArcPrePedidos_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsMerArcPrePedidos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

            Dim dates As SfDateTimeEdit() = {txtEmision, txtEntrega}
            SetSizeDateObjects(dates)

            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo Pre-pedido")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> Pre-pedido")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> Pre-pedido")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> Pre-pedido")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primer</B> Pre-pedido")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a Pre-pedido <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a Pre-pedido <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>último Pre-pedido</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> Pre-pedido")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnDuplicar, "<B>Duplicar</B> este Pre-pedido")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en Pre-pedido")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón en Pre-pedido")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón en Pre-pedido")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en Pre-pedido")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón en Pre-pedido")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón en Pre-pedido")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> en Pre-pedido")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la Pre-pedido")
        C1SuperTooltip1.SetToolTip(btnPresupuestos, "Traer <B>presupuestos</B> en tránsito")

        'Menu Barra Descuento 
        C1SuperTooltip1.SetToolTip(btnEliminaDescuento, "<B>Elimina</B> descuento global de Pre-pedido")


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
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtCliente, btnCliente)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtCodigo.Text = .Item("numped")
                txtEmision.Value = .Item("emision")
                txtEntrega.Value = .Item("entrega")
                txtEstatus.Text = aEstatus(.Item("estatus"))
                txtCliente.Text = .Item("codcli")
                txtComentario.Text = ft.MuestraCampoTexto(.Item("comen"))
                txtAsesor.Text = ft.MuestraCampoTexto(.Item("codven"))
                ft.RellenaCombo(aTarifa, cmbTarifa, ft.InArray(aTarifa, .Item("tarifa")))

                tslblPesoT.Text = ft.FormatoCantidad(.Item("kilos"))

                txtSubTotal.Text = ft.FormatoNumero(.Item("tot_net"))
                txtDescuentos.Text = ft.FormatoNumero(.Item("descuen"))
                txtCargos.Text = ft.FormatoNumero(.Item("cargos"))
                txtTotalIVA.Text = ft.FormatoNumero(.Item("imp_iva"))
                txtTotal.Text = ft.FormatoNumero(.Item("tot_ped"))
                txtVencimiento.Text = ft.FormatoFecha(CDate(.Item("vence").ToString))
                txtCondicionPago.Text = "Condicion de pago : " & IIf(.Item("condpag") = 0, "CREDITO", "CONTADO")
                MontoAnterior = .Item("tot_ped")
                CondicionDePago = .Item("condpag")
                FechaVencimiento = CDate(.Item("vence").ToString)

                Impresa = .Item("impresa")

                'Renglones
                AsignarMovimientos(.Item("numped"))

                'Totales
                CalculaTotales()

            End With
        End With
    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroPrePedido As String)
        strSQLMov = "select * from jsvenrenpedrgv " _
                            & " where " _
                            & " numped  = '" & NumeroPrePedido & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"item.Item.90.I.", _
                                   "descrip.Descripción.300.I.", _
                                   "iva.IVA.30.C.", _
                                   "cantidad.Cantidad.90.D.Cantidad", _
                                   "unidad.UND.35.C.", _
                                   "precio.Precio Unitario.110.D.Numero", _
                                   "des_art.Desc Artículo.65.D.Numero", _
                                   "des_cli.Desc Cliente.65.D.Numero", _
                                   "des_ofe.Desc Oferta.65.D.Numero", _
                                   "totren.Precio Total.110.D.Numero", _
                                   "estatus.Tipo Renglon.70.C.", _
                                   "sada..100.I."}

        ft.IniciarTablaPlus(dg, dtRenglones, aCampos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "PPTMP" & ft.NumeroAleatorio(100000)
        Else
            txtCodigo.Text = ""
        End If


        txtCliente.Text = ""
        txtNombreCliente.Text = ""
        txtAsesor.Text = ""
        ft.RellenaCombo(aTarifa, cmbTarifa)
        txtComentario.Text = ""
        txtEmision.Value = sFechadeTrabajo
        txtEntrega.Value = sFechadeTrabajo
        txtEstatus.Text = aEstatus(0)
        tslblPesoT.Text = ft.FormatoCantidad(0)

        txtSubTotal.Text = ft.FormatoNumero(0.0)
        txtDescuentos.Text = ft.FormatoNumero(0)
        txtCargos.Text = ft.FormatoNumero(0.0)
        txtTotalIVA.Text = ft.FormatoNumero(0.0)
        txtTotal.Text = ft.FormatoNumero(0.0)
        txtVencimiento.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtCondicionPago.Text = ""
        MontoAnterior = 0.0
        CondicionDePago = 1

        dgDisponibilidad.Columns.Clear()
        lblDisponibilidad.Text = "Disponible menos este Documento : 0.00"

        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(txtCodigo.Text)
        CalculaTotales()

    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon, MenuDescuentos)
        ft.habilitarObjetos(True, True, txtComentario, txtEmision, txtEntrega, btnCliente, txtCliente, cmbTarifa,
                         btnAsesor, txtAsesor)
        ft.visualizarObjetos(True, grpDisponibilidad)
        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtEmision, txtEntrega, txtEstatus,
                txtCliente, txtNombreCliente, btnCliente, txtComentario, txtAsesor, btnAsesor, txtNombreAsesor,
                cmbTarifa, txtVencimiento, txtCondicionPago, txtTotalCambioEmision, txtTotalActual)

        ft.habilitarObjetos(False, True, txtDescuentos, txtCargos, MenuDescuentos, txtSubTotal, txtTotalIVA, txtTotal)
        ft.visualizarObjetos(False, grpDisponibilidad)

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

        ft.Ejecutar_strSQL(myconn, " delete from jsvenrenpedrgv where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myconn, " delete from jsvenivapedrgv where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myconn, " delete from jsvendespedrgv where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myconn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'PPD' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim f As New jsGenFormasPago
            f.CondicionPago = CondicionDePago
            f.TipoCredito = TipoCredito
            f.Vencimiento = FechaVencimiento
            f.Cargar(myConn, "PPE", txtCodigo.Text, IIf(i_modo = movimiento.iAgregar, True, False), ValorNumero(txtTotal.Text))
            If f.DialogResult = Windows.Forms.DialogResult.OK Then
                CondicionDePago = f.CondicionPago
                TipoCredito = f.TipoCredito
                FechaVencimiento = f.Vencimiento
                GuardarTXT()
            End If
        End If
    End Sub
    Private Function Validado() As Boolean

        If FechaUltimoBloqueo(myConn, "jsvenencpedrgv") >= txtEmision.Value Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If txtNombreCliente.Text = "" Then
            ft.MensajeCritico("Debe indicar un cliente válido...")
            Return False
        End If

        If txtNombreAsesor.Text = "" Then
            ft.MensajeCritico("Debe indicar un nombre de Asesor válido...")
            Return False
        End If

        If txtEntrega.Value < txtEmision.Value Then
            ft.mensajeCritico("Fecha de emision es mayor a fecha de vencimiento. Verifique ...")
            Return False
        End If

        If dtRenglones.Rows.Count = 0 Then
            ft.MensajeCritico("Debe incluir al menos un ítem...")
            Return False
        End If

        Dim Disponible As Double = CDbl(Trim(Split(lblDisponibilidad.Text, ":")(1)))
        If Disponible < 0 Then
            ft.MensajeCritico("Este pre-pedido excede la disponibilidad...")
            Return False
        End If

        Dim EstatusCliente As Integer = ft.DevuelveScalarEntero(myConn, " select estatus from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        If EstatusCliente = 1 Or EstatusCliente = 2 Then
            ft.MensajeCritico("Este Cliente posee estatus  " & aEstatusCliente(EstatusCliente) & ". Favor remitir a Administración")
        End If

        If Not CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM19")) Then
            If ClientePoseeChequesFuturos(myConn, lblInfo, txtCliente.Text) Then
                ft.MensajeCritico(" SE TOMARA EL PEDIDO. PERO HASTA QUE EL CHEQUE A FUTURO O POST DATADO NO SE HAGA EFECTIVO " & vbCrLf & _
                    " NO PODRA FACTURAR ESTE PEDIDO ")
            End If
        End If

        Return True

    End Function
    Private Sub GuardarTXT()

        Dim Codigo As String = txtCodigo.Text
        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count
            Codigo = Contador(myConn, lblInfo, Gestion.iVentas, "VENNUMPPE", "04")
            ft.Ejecutar_strSQL(myconn, " update jsvenrenpedrgv set numped = '" & Codigo & "' where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myconn, " update jsvenivapedrgv set numped = '" & Codigo & "' where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myconn, " update jsvendespedrgv set numped = '" & Codigo & "' where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myconn, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & txtCodigo.Text & "' and origen = 'PPE' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        InsertEditVENTASEncabezadoPrePedido(myConn, lblInfo, Inserta, Codigo, txtEmision.Value, txtEntrega.Value,
                 txtCliente.Text, txtComentario.Text, txtAsesor.Text, cmbTarifa.Text, ValorNumero(txtSubTotal.Text),
                 0.0, ValorNumero(txtDescuentos.Text), ValorNumero(txtCargos.Text), ValorNumero(txtTotalIVA.Text),
                 ValorNumero(txtTotal.Text), FechaVencimiento, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
                 jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo,
                 ft.InArray(aEstatus, txtEstatus.Text), dtRenglones.Rows.Count, 0.0, ValorCantidad(tslblPesoT.Text),
                 CondicionDePago, TipoCredito, "", "", "", 0.0, "", 0, 0, 0, 0, Impresa)

        ActualizarMovimientos()

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub ActualizarMovimientos()


        ''1.- colocar cantidad de pedidos en tabla de mercancías
        '' ft.Ejecutar_strSQL_DevuelveScalar(myConn, lblInfo, _
        ''                       " UPDATE jsmerctainv a , (SELECT a.item, SUM(IF( ISNULL(b.UVALENCIA), ABS(a.CANTIDAD), ABS(a.CANTIDAD/b.EQUIVALE))) cantidad, a.id_emp FROM " _
        ''                       & " jsvenrenpedrgv a LEFT JOIN jsmerequmer b " _
        ''                       & " ON (a.ITEM = b.CODART AND a.UNIDAD = b.UVALENCIA AND a.id_emp = b.id_emp) " _
        ''                       & " WHERE " _
        ''                       & " a.estatus <> 2 AND " _
        ''                       & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
        ''                       & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
        ''                       & " GROUP BY a.ITEM ) b " _
        ''                       & " a.pedidos = b.cantidad " _
        ''                       & " WHERE " _
        ''                       & " a.codart =  b.item AND " _
        ''                       & " a.id_emp = b.id_emp " _
        ''                       & " a.id_emp = '" & jytsistema.WorkID & "' ")

        ''2.- Actualiza Cantidades en tránsito de los presupuestos y  el estatus del documento


    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique comentario ... ", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtCliente, btnCliente)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim aCamposAdicionales() As String = {"numped|'" & txtCodigo.Text & "'", _
                                              "codcli|'" & txtCliente.Text & "'"}
        If DocumentoBloqueado(myConn, "jsvenencpedrgv", aCamposAdicionales) Then
            ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
        Else
            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPPEPA01")) Then
                i_modo = movimiento.iEditar
                nPosicionEncab = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
                ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtCliente, btnCliente)
            Else
                ft.mensajeCritico("Edición de pedidos NO está permitida...")
            End If
        End If
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        Dim aCamposAdicionales() As String = {"numped|'" & txtCodigo.Text & "'", _
                                             "codcli|'" & txtCliente.Text & "'"}
        If DocumentoBloqueado(myConn, "jsvenencpedrgv", aCamposAdicionales) Then
            ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
        Else
            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPPEPA01")) Then
                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                nPosicionEncab = Me.BindingContext(ds, nTabla).Position

                sRespuesta = MsgBox(" ¿ Está seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")

                If sRespuesta = MsgBoxResult.Yes Then

                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numped"))
                    ft.Ejecutar_strSQL(myConn, " delete from jsvenencpedrgv where " _
                        & " numped = '" & txtCodigo.Text & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "'")

                    ft.Ejecutar_strSQL(myConn, " delete from jsvenrenpedrgv where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " delete from jsvenivapedrgv where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'PPE' and id_emp = '" & jytsistema.WorkID & "' ")

                    ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                    dt = ds.Tables(nTabla)
                    If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                    AsignaTXT(nPosicionEncab)

                End If
            Else
                ft.mensajeCritico("Eliminación de pedidos NO está permitida...")
            End If
        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numped", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Nombre", "Emisión", "Comentario"}
        Dim Anchos() As Integer = {150, 400, 100, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Pre-pedidos de mercancías")
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
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub CalculaTotales()

        txtSubTotal.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrenpedrgv", "numped", "totren", txtCodigo.Text, 0))

        Dim discountTable As New SimpleTableProperties("jsvendespedrgv", "numped", txtCodigo.Text)
        MostrarDescuentosAlbaran(myConn, discountTable, dgDescuentos, txtDescuentos)

        txtCargos.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrenpedrgv", "numped", "totren", txtCodigo.Text, 1))
        CalculaDescuentoEnRenglones()
        CalculaTotalIVAVentas(myConn, lblInfo, "jsvendespedrgv", "jsvenivapedrgv", "jsvenrenpedrgv", "numped", txtCodigo.Text,
                              "impiva", "totrendes", txtEmision.Value, "totren")

        Dim ivaTable As New SimpleTableProperties("jsvenivapedrgv", "numped", txtCodigo.Text)
        MostrarIVAAlbaran(myConn, ivaTable, dgIVA, txtTotalIVA)

        txtTotal.Text = ft.FormatoNumero(ValorNumero(txtSubTotal.Text) - ValorNumero(txtDescuentos.Text) + ValorNumero(txtCargos.Text) + ValorNumero(txtTotalIVA.Text))

        tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsvenrenpedrgv", "peso", "numped", txtCodigo.Text))

        MostrarTotalesCambioMoneda(myConn, txtEmision.Value, txtTotal, txtTotalCambioEmision,
                                   txtTotalActual)

    End Sub
    Private Sub CalculaDescuentoEnRenglones()

        ft.Ejecutar_strSQL(myConn, " update jsvenrenpedrgv set totrendes = totren - totren * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
            & " where " _
            & " numped = '" & txtCodigo.Text & "' and " _
            & " renglon > '' and " _
            & " item > '' and " _
            & " ESTATUS = '0' AND " _
            & " ACEPTADO < '2' and " _
            & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
            & " ID_EMP = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        If txtNombreCliente.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "PPE", txtCodigo.Text, txtEmision.Value, , cmbTarifa.Text, txtCliente.Text, , , , , , , , , , txtAsesor.Text)
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
            f.Editar(myConn, ds, dtRenglones, "PPE", txtCodigo.Text, txtEmision.Value, , cmbTarifa.Text, txtCliente.Text,
                     IIf(dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position).Item("item").ToString.Substring(0, 1) = "$", True, False), , , , , , , , , txtAsesor.Text)
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
                With dtRenglones.Rows(nPosicionRenglon)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("item"))
                    Dim aCamposDel() As String = {"numped", "item", "renglon", "id_emp"}
                    Dim aStringsDel() As String = {txtCodigo.Text, .Item("item"), .Item("renglon"), jytsistema.WorkID}

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsvenrenpedrgv", strSQLMov, aCamposDel, aStringsDel, _
                                                  Me.BindingContext(ds, nTablaRenglones).Position, True)
                    ft.Ejecutar_strSQL(myconn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and " _
                            & " origen = 'PPE' and item = '" & .Item("item") & "' and renglon = '" & .Item("renglon") & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

                    If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1
                    AsignaMov(nPosicionRenglon, True)
                    CalculaTotales()
                End With
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


        If Not CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPPEPA08")) Then
            ft.MensajeCritico("Impresión de PRE-PEDIDO NO permitida...")
            Exit Sub
        End If

        If DocumentoImpreso(myConn, lblInfo, "jsvenencpedrgv", "numped", txtCodigo.Text) AndAlso _
           CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPPEPA07")) Then
            ft.MensajeCritico("Este documento ya fue impreso...")
            Exit Sub
        End If


        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cPrePedido, "PrePedido de Mercancías",
                 txtCliente.Text, txtCodigo.Text, txtEmision.Value)
        f = Nothing

    End Sub
    Private Sub Imprimir()

        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPPEPA08")) Then
            ft.MensajeCritico("Impresión de PRE-PREDIDO NO permitida...")
            Exit Sub
        End If

        If DocumentoImpreso(myConn, lblInfo, "jsvenencpedrgv", "numped", txtCodigo.Text) AndAlso _
            CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPPEPA07")) Then
            ft.MensajeCritico("Este documento ya fue impreso...")
            Exit Sub
        End If

        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cPrePedido, "PREPEDIDO", ,
                 txtCodigo.Text, txtEmision.Value)
        f.Dispose()
        f = Nothing

        ft.Ejecutar_strSQL(myconn, "update jsvenencpedrgv set impresa = 1 where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub

    Private Sub btnAsesor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesor.Click
        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPPEPA09")) Then
            txtAsesor.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, CONCAT( apellidos, ', ', nombres) descripcion from jsvencatven where tipo = '" & TipoVendedor.iFuerzaventa & "'  and estatus = 1 and id_emp = '" & jytsistema.WorkID & "'  order by 1 ", "Asesores Comerciales", _
                                               txtAsesor.Text)
        Else
            ft.MensajeCritico("Escogencia de asesor comercial no permitida...")
        End If
    End Sub

    Private Sub txtCliente_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCliente.TextChanged
        If txtCliente.Text <> "" Then
            Dim aFld() As String = {"codcli", "id_emp"}
            Dim aStr() As String = {txtCliente.Text, jytsistema.WorkID}
            Dim FormaDePagoCliente As String = qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "formapago")
            txtNombreCliente.Text = qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "nombre")
            MostrarDisponibilidad(myConn, lblInfo, txtCliente.Text, qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "rif"), _
                            ds, dgDisponibilidad)

            Disponibilidad = ft.DevuelveScalarDoble(myConn, " select disponible from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            Dim mTotalFac As Double = ValorNumero(txtTotal.Text)
            If i_modo = movimiento.iAgregar Then
                mTotalFac = 0.0
                FechaVencimiento = FechaVencimientoFactura(myConn, txtCliente.Text,
                                                           txtEmision.Value)

                CondicionDePago = CondicionPagoProveedorCliente(myConn, lblInfo, FormaDePagoCliente)
                If qFound(myConn, lblInfo, "jsvencatcli", aFld, aStr) Then
                    Dim cAsesor As String = ft.DevuelveScalarCadena(myConn, "SELECT b.codven " _
                                                             & " FROM jsvenrenrut a  " _
                                                             & " LEFT JOIN jsvenencrut b ON (a.codrut = b.codrut AND a.tipo = b.tipo AND a.id_emp = b.id_emp) " _
                                                             & " WHERE " _
                                                             & " a.tipo = '0' AND " _
                                                             & " a.cliente = '" & txtCliente.Text & "' AND " _
                                                             & " a.id_emp = '" & jytsistema.WorkID & "' ")

                    txtAsesor.Text = cAsesor
                    cmbTarifa.SelectedIndex = ft.InArray(aTarifa, qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "tarifa"))
                End If

            End If
            lblDisponibilidad.Text = "Disponible menos este Documento : " & ft.FormatoNumero(Disponibilidad + MontoAnterior - mTotalFac)

        End If
    End Sub

    Private Sub txtAsesor_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAsesor.TextChanged
        Dim aFld() As String = {"codven", "tipo", "id_emp"}
        Dim aStr() As String = {txtAsesor.Text, "0", jytsistema.WorkID}
        txtNombreAsesor.Text = qFoundAndSign(myConn, lblInfo, "jsvencatven", aFld, aStr, "apellidos") & ", " _
                & qFoundAndSign(myConn, lblInfo, "jsvencatven", aFld, aStr, "nombres")
    End Sub

    Private Sub btnAgregaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaDescuento.Click
        If txtCliente.Text.Trim <> "" Then
            If ClienteFacturaAPartirDeCostos(myConn, lblInfo, txtCliente.Text) Then
                ft.MensajeCritico("DESCUENTOS NO PERMITIDOS")
            Else
                Dim f As New jsGenDescuentosVentas
                f.Agregar(myConn, ds, dtDescuentos, "jsvendespedrgv", "numped", txtCodigo.Text,
                          sModulo, txtAsesor.Text, 0, txtEmision.Value, ValorNumero(txtSubTotal.Text))
                CalculaTotales()
                f = Nothing
            End If
        End If

    End Sub
    Private Sub btnEliminaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaDescuento.Click
        If nPosicionDescuento >= 0 Then
            With dtDescuentos.Rows(nPosicionDescuento)
                Dim aCamposDel() As String = {"numped", "renglon", "id_emp"}
                Dim aStringsDel() As String = {txtCodigo.Text, .Item("renglon"), jytsistema.WorkID}
                nPosicionDescuento = EliminarRegistros(myConn, lblInfo, ds, nTablaDescuentos, "jsvendespedrgv", strSQLDescuentos, aCamposDel, aStringsDel, _
                                                      Me.BindingContext(ds, nTablaDescuentos).Position, True)
            End With
            CalculaTotales()
        End If
    End Sub
    Private Sub btnCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCliente.Click
        txtCliente.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcli codigo, nombre descripcion, disponible, elt(estatus+1, 'Activo', 'Bloqueado', 'Inactivo', 'Desincorporado') estatus from jsvencatcli where estatus < 3 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Clientes", _
                                            txtCliente.Text)
    End Sub



    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If txtNombreCliente.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "PPE", txtCodigo.Text, txtEmision.Value, ,
                      cmbTarifa.Text, txtCliente.Text, True)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Private Sub txtNombreCliente_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNombreCliente.TextChanged
        If txtNombreCliente.Text = "" Then dgDisponibilidad.Columns.Clear()
    End Sub

    Private Sub dgDescuento_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgDescuentos.RowHeaderMouseClick, _
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

    Private Sub btnCortar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCortar.Click
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
            With dtRenglones.Rows(nPosicionRenglon)
                Dim f As New jsGenCambioPrecioEnAlbaranes
                f.NuevoPrecio = .Item("precio")
                Dim PrecioAnterior As Double = f.NuevoPrecio

                f.Cambiar(myConn, ds, dtRenglones, txtCodigo.Text, txtCliente.Text, .Item("renglon"), .Item("item"), .Item("unidad"), .Item("cantidad"), .Item("precio"))
                If f.NuevoPrecio <> PrecioAnterior Then
                    ft.Ejecutar_strSQL(myconn, " update jsvenrenpedrgv " _
                                   & " set precio = " & f.NuevoPrecio & ", " _
                                   & " totren = " & f.NuevoPrecio * .Item("cantidad") & ", " _
                                   & " totrendes = " & f.NuevoPrecio * .Item("cantidad") * (1 - .Item("des_art") / 100) * (1 - .Item("des_cli") / 100) * (1 - .Item("des_ofe") / 100) & " " _
                                   & " where " _
                                   & " numped = '" & txtCodigo.Text & "' and " _
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