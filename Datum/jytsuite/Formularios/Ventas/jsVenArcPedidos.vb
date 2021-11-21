Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Imports Syncfusion.WinForms.ListView.Enums

Public Class jsVenArcPedidos
    Private Const sModulo As String = "Pedidos"
    Private Const nTabla As String = "tblEncabPedidos"
    Private Const lRegion As String = "RibbonButton82"
    Private Const nTablaRenglones As String = "tblRenglones_Pedidos"
    Private Const nTablaIVA As String = "tblIVA"
    Private Const nTablaDescuentos As String = "tblDescuentos"



    'limit 1000

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
    Private interchangeList As New List(Of CambioMonedaPlus)

    Private cliente As New Customer()
    Private asesor As New SalesForce()


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
    Private strSQL As String = " (select a.*, b.nombre from jsvenencped a " _
           & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
           & " where " _
           & " a.emision >= '" & ft.FormatoFechaMySQL(DateAdd("m", -MesesAtras.i6, jytsistema.sFechadeTrabajo)) & "' and " _
           & " a.emision <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
           & " a.id_emp = '" & jytsistema.WorkID & "' order by a.numped desc ) order by emision, numped "
    Private Impresa As Integer

    Private Sub jsVenArcPedidos_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsMerArcPrePedidos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo
        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)
            IniciarControles()
            If dt.Rows.Count > 0 Then
                nPosicionEncab = dt.Rows.Count - 1
                Me.BindingContext(ds, nTabla).Position = nPosicionEncab
                AsignaTXT(nPosicionEncab)
            Else
                IniciarDocumento(False)
            End If
            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub IniciarControles()
        Dim dates As SfDateTimeEdit() = {txtEmision, txtEntrega}
        SetSizeDateObjects(dates)
        '' Clientes
        InitiateDropDown(Of Customer)(myConn, cmbCliente)
        ''Asesores 
        InitiateDropDown(Of SalesForce)(myConn, cmbAsesores)
        '' Monedas
        ' interchangeList = GetListaDeMonedasyCambios(myConn, jytsistema.sFechadeTrabajo)
        InitiateDropDownInterchangeCurrency(myConn, cmbMonedas, jytsistema.sFechadeTrabajo, True)

        DesactivarMarco0()
        AsignarTooltips()
    End Sub
    Private Sub AsignarTooltips()
        Dim menus As New List(Of ToolStrip) From {MenuBarra, MenuBarraRenglon, MenuDescuentos}
        AsignarToolTipsMenuBarraToolStrip(menus, "Pedido")
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
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, cmbCliente)
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            If nRow >= 0 Then
                With .Rows(nRow)
                    'Encabezado 
                    txtCodigo.Text = .Item("numped")
                    txtEmision.Value = .Item("emision")
                    txtEntrega.Value = .Item("entrega")
                    txtEstatus.Text = aEstatus(.Item("estatus"))
                    cmbCliente.SelectedValue = .Item("codcli")
                    txtComentario.Text = ft.muestraCampoTexto(.Item("comen"))
                    cmbAsesores.SelectedValue = .Item("codven")
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

                    SetComboCurrency(.Item("Currency"), cmbMonedas, lblTotal)
                    Impresa = .Item("impresa")

                    'Renglones
                    AsignarMovimientos(.Item("numped"))

                    'Totales
                    CalculaTotales()

                End With
            End If

        End With
    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroPrePedido As String)
        strSQLMov = "select * from jsvenrenped " _
                            & " where " _
                            & " numped  = '" & NumeroPrePedido & "' and " _
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
                                   "estatus.Tipo Renglon.70.C.",
                                   "numcot.N° Prepedido.100.I.",
                                   "sada..100.I."}


        ft.IniciarTablaPlus(dg, dtRenglones, aCampos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "PETMP" & ft.NumeroAleatorio(100000)
        Else
            txtCodigo.Text = ""
        End If

        cmbCliente.SelectedIndex = -1
        cmbAsesores.SelectedIndex = -1
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

        SetComboCurrency(0, cmbMonedas, lblTotal)
        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(txtCodigo.Text)
        CalculaTotales()

    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon, MenuDescuentos)
        ft.habilitarObjetos(True, True, txtComentario, txtEmision, txtEntrega, cmbCliente, cmbTarifa)
        If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPEDPA09")) Then cmbAsesores.Enabled = True
        ft.visualizarObjetos(True, grpDisponibilidad)
        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtEmision, txtEntrega, txtEstatus, cmbMonedas,
                cmbCliente, txtComentario, cmbAsesores, cmbTarifa, txtVencimiento, txtCondicionPago)

        ft.habilitarObjetos(False, True, txtDescuentos, txtCargos, MenuDescuentos, txtSubTotal, txtTotalIVA, txtTotal)
        ft.visualizarObjetos(False, grpDisponibilidad)

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

        ft.Ejecutar_strSQL(myConn, " delete from jsvenrenped where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvenivaped where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvendesped where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'PED' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim f As New jsGenFormasPago
            f.CondicionPago = CondicionDePago
            f.TipoCredito = TipoCredito
            f.Vencimiento = FechaVencimiento
            f.Cargar(myConn, "PED", txtCodigo.Text, IIf(i_modo = movimiento.iAgregar, True, False), ValorNumero(txtTotal.Text), cmbMonedas.SelectedValue)
            If f.DialogResult = Windows.Forms.DialogResult.OK Then
                CondicionDePago = f.CondicionPago
                TipoCredito = f.TipoCredito
                FechaVencimiento = f.Vencimiento
                GuardarTXT()
            End If
        End If
    End Sub
    Private Function Validado() As Boolean

        If FechaUltimoBloqueo(myConn, "jsvenencped") >= txtEmision.Value Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If cmbCliente.SelectedIndex < 0 Then
            ft.mensajeCritico("Debe indicar un cliente válido...")
            Return False
        End If

        If cmbAsesores.SelectedIndex < 0 Then
            ft.mensajeCritico("Debe indicar un nombre de Asesor válido...")
            Return False
        End If

        If txtEntrega.Value < txtEmision.Value Then
            ft.mensajeCritico("Fecha de emision es mayor a fecha de vencimiento. Verifique ...")
            Return False
        End If

        If dtRenglones.Rows.Count = 0 Then
            ft.mensajeCritico("Debe incluir al menos un ítem...")
            Return False
        End If

        Dim Disponible As Double = CDbl(Trim(Split(lblDisponibilidad.Text, ":")(1)))
        If Disponible < 0 Then
            ft.mensajeCritico("Este pedido excede la disponibilidad...")
            Return False
        End If

        If cliente.CodigoEstatus = 1 Or cliente.CodigoEstatus = 2 Then
            ft.mensajeCritico("Este Cliente posee estatus  " & cliente.Estatus & ". Favor remitir a Administración")
        End If

        If Not CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPARAM19")) Then
            If ClientePoseeChequesFuturos(myConn, lblInfo, cliente.Codcli) Then
                ft.mensajeCritico(" SE TOMARA EL PEDIDO. PERO HASTA QUE EL CHEQUE A FUTURO O POST DATADO NO SE HAGA EFECTIVO " & vbCrLf &
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
            Codigo = Contador(myConn, lblInfo, Gestion.iVentas, "VENNUMPED", "05")
            ft.Ejecutar_strSQL(myConn, " update jsvenrenped set numped = '" & Codigo & "' where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvenivaped set numped = '" & Codigo & "' where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvendesped set numped = '" & Codigo & "' where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & txtCodigo.Text & "' and origen = 'PED' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        InsertEditVENTASEncabezadoPedido(myConn, lblInfo, Inserta, Codigo, txtEmision.Value,
                                         txtEntrega.Value, cliente.Codcli, txtComentario.Text,
                                         asesor.Codigo, cmbTarifa.Text, ValorNumero(txtSubTotal.Text),
                 0.0, ValorNumero(txtDescuentos.Text), ValorNumero(txtCargos.Text), ValorNumero(txtTotalIVA.Text),
                 ValorNumero(txtTotal.Text), FechaVencimiento, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
                 jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo,
                 ft.InArray(aEstatus, txtEstatus.Text), dtRenglones.Rows.Count, 0.0, ValorCantidad(tslblPesoT.Text),
                 CondicionDePago, TipoCredito, "", "", "", 0.0, "", 0, 0, 0, 0, Impresa,
                 cmbMonedas.SelectedValue, DateTime.Now())

        ActualizarMovimientos(Codigo)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub ActualizarMovimientos(ByVal NumeroPedido As String)


        '1.- colocar cantidad de pedidos en tabla de mercancías
        ft.Ejecutar_strSQL(myConn,
                              " UPDATE jsmerctainv a , (SELECT a.item, SUM(IF( ISNULL(b.UVALENCIA), ABS(a.CANTIDAD), ABS(a.CANTIDAD/b.EQUIVALE))) cantidad, a.id_emp FROM " _
                              & " jsvenrenped a LEFT JOIN jsmerequmer b " _
                              & " ON (a.ITEM = b.CODART AND a.UNIDAD = b.UVALENCIA AND a.id_emp = b.id_emp) " _
                              & " WHERE " _
                              & " substring(a.item,1,1) <> '$' and " _
                              & " a.numped = '" & NumeroPedido & "' and " _
                              & " a.estatus <> 2 AND " _
                              & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                              & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                              & " GROUP BY a.ITEM ) b " _
                              & " set a.pedidos = b.cantidad " _
                              & " WHERE " _
                              & " a.codart =  b.item AND " _
                              & " a.id_emp = b.id_emp and " _
                              & " a.id_emp = '" & jytsistema.WorkID & "' ")

        '2.- Actualiza Cantidades en tránsito de los presupuestos y  el estatus del documento


    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique comentario ... ", Transportables.tipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, cmbCliente)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        Dim aCamposAdicionales() As String = {"numped|'" & txtCodigo.Text & "'",
                                             "codcli|'" & cliente.Codcli & "'"}
        If DocumentoBloqueado(myConn, "jsvenencped", aCamposAdicionales) Then
            ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
        Else
            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPEDPA01")) Then
                i_modo = movimiento.iEditar
                nPosicionEncab = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
                ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, cmbCliente)
            Else
                ft.mensajeCritico("Edición de pedidos NO está permitida...")
            End If
        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Dim aCamposAdicionales() As String = {"numped|'" & txtCodigo.Text & "'",
                                             "codcli|'" & cliente.Codcli & "'"}
        If DocumentoBloqueado(myConn, "jsvenencped", aCamposAdicionales) Then
            ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
        Else
            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPEDPA01")) Then
                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                nPosicionEncab = Me.BindingContext(ds, nTabla).Position

                sRespuesta = MsgBox(" ¿ Está seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")

                If sRespuesta = MsgBoxResult.Yes Then

                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numped"))
                    ft.Ejecutar_strSQL(myConn, " delete from jsvenencped where " _
                        & " numped = '" & txtCodigo.Text & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "'")

                    ft.Ejecutar_strSQL(myConn, " delete from jsvenrenped where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " delete from jsvenivaped where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'PED' and id_emp = '" & jytsistema.WorkID & "' ")

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
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Pedidos de mercancías...")
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

        txtSubTotal.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrenped", "numped", "totren", txtCodigo.Text, 0))

        Dim discountTable As New SimpleTableProperties("jsvendesped", "numped", txtCodigo.Text)
        MostrarDescuentosAlbaran(myConn, discountTable, dgDescuentos, txtDescuentos)

        txtCargos.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrenped", "numped", "totren", txtCodigo.Text, 1))
        CalculaDescuentoEnRenglones()

        CalculaTotalIVAVentas(myConn, lblInfo, "jsvendesped", "jsvenivaped", "jsvenrenped", "numped", txtCodigo.Text, "impiva",
                              "totrendes", txtEmision.Value, "totren")

        Dim ivaTable As New SimpleTableProperties("jsvenivaped", "numped", txtCodigo.Text)
        MostrarIVAAlbaran(myConn, ivaTable, dgIVA, txtTotalIVA)

        txtTotal.Text = ft.FormatoNumero(ValorNumero(txtSubTotal.Text) - ValorNumero(txtDescuentos.Text) + ValorNumero(txtCargos.Text) + ValorNumero(txtTotalIVA.Text))

        tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsvenrenped", "peso", "numped", txtCodigo.Text))

    End Sub
    Private Sub CalculaDescuentoEnRenglones()

        ft.Ejecutar_strSQL(myConn, " update jsvenrenped set totrendes = totren - totren * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
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
        If cmbCliente.SelectedIndex >= 0 Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "PED", txtCodigo.Text, txtEmision.Value, , cmbTarifa.Text, cliente.Codcli, , , , , , , , , , asesor.Codigo)
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
            f.Editar(myConn, ds, dtRenglones, "PED", txtCodigo.Text, txtEmision.Value, , cmbTarifa.Text, cliente.Codcli,
                     IIf(dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position).Item("item").ToString.Substring(0, 1) = "$", True, False), , , , , , , , , asesor.Codigo)
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


                    ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and " _
                            & " origen = 'PED' and item = '" & .Item("item") & "' and renglon = '" & .Item("renglon") & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsvenrenped", strSQLMov, aCamposDel, aStringsDel,
                                                  Me.BindingContext(ds, nTablaRenglones).Position, True)


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


        If Not CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPEDPA08")) Then
            ft.mensajeCritico("Impresión de PRESUPUESTO NO permitida...")
            Exit Sub
        End If

        If DocumentoImpreso(myConn, lblInfo, "jsvenencped", "numped", txtCodigo.Text) AndAlso
           CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPEDPA07")) Then
            ft.mensajeCritico("Este documento ya fue impreso...")
            Exit Sub
        End If

        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cPedido, "Pedido",
                 cliente.Codcli, txtCodigo.Text, txtEmision.Value)
        f = Nothing

    End Sub

    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs)
        If e.ColumnIndex = 1 Then e.Value = ft.FormatoNumero(e.Value) & "%"
    End Sub

    Private Sub btnAgregaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaDescuento.Click
        If cmbCliente.SelectedIndex >= 0 Then
            If ClienteFacturaAPartirDeCostos(myConn, lblInfo, cliente.Codcli) Then
                ft.mensajeCritico("DESCUENTOS NO PERMITIDOS")
            Else
                Dim f As New jsGenDescuentosVentas
                f.Agregar(myConn, ds, dtDescuentos, "jsvendesped", "numped", txtCodigo.Text, sModulo, asesor.Codigo, 0,
                          txtEmision.Value, ValorNumero(txtSubTotal.Text))
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
                nPosicionDescuento = EliminarRegistros(myConn, lblInfo, ds, nTablaDescuentos, "jsvendesped", strSQLDescuentos, aCamposDel, aStringsDel,
                                                      Me.BindingContext(ds, nTablaDescuentos).Position, True)
            End With
            CalculaTotales()
        End If
    End Sub

    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If cmbCliente.SelectedIndex >= 0 Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "PED", txtCodigo.Text,
                      txtEmision.Value, , cmbTarifa.Text, cliente.Codcli,
                      True, , , , , , , , , asesor.Codigo)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Private Sub dgDescuento_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs)
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

                f.Cambiar(myConn, ds, dtRenglones, txtCodigo.Text, cliente.Codcli, .Item("renglon"), .Item("item"), .Item("unidad"), .Item("cantidad"), .Item("precio"))
                If f.NuevoPrecio <> PrecioAnterior Then
                    ft.Ejecutar_strSQL(myConn, " update jsvenrenped " _
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

    Private Sub cmbCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCliente.SelectedIndexChanged
        cliente = cmbCliente.SelectedItem
        If cmbCliente.SelectedItem >= 0 Then
            MostrarDisponibilidad(myConn, lblInfo, cliente.Codcli, cliente.Rif, ds, dgDisponibilidad)
            Dim mTotalFac As Double = ValorNumero(txtTotal.Text)
            If i_modo = movimiento.iAgregar Then
                mTotalFac = 0.0
                FechaVencimiento = FechaVencimientoFactura(myConn, cliente.CodigoEstatus, txtEmision.Value)
                CondicionDePago = CondicionPagoProveedorCliente(myConn, lblInfo, cliente.FormaPago)
                cmbTarifa.SelectedIndex = ft.InArray(aTarifa, cliente.Tarifa)
                cmbAsesores.SelectedValue = cliente.Asesor
            End If
            lblDisponibilidad.Text = "Disponible menos este Documento : " & ft.FormatoNumero(cliente.Disponible + MontoAnterior - mTotalFac)

        End If
    End Sub
    Private Sub cmbAsesores_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAsesores.SelectedIndexChanged
        asesor = cmbAsesores.SelectedItem
    End Sub

End Class