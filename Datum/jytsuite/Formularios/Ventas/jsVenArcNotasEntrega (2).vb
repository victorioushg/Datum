Imports MySql.Data.MySqlClient
Public Class jsVenArcNotasEntrega
    Private Const sModulo As String = "Notas de Entrega"
    Private Const nTabla As String = "tblEncabNotasEntrega"
    Private Const lRegion As String = "RibbonButton84"
    Private Const nTablaRenglones As String = "tblRenglones_NotasEntrega"
    Private Const nTablaIVA As String = "tblIVA"
    Private Const nTablaDescuentos As String = "tblDescuentos"

    Private strSQL As String = " (select a.*, b.nombre from jsvenencnot a " _
            & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
            & " where " _
            & " a.emision >= '" & FormatoFechaMySQL(DateAdd("m", -MesesAtras.i1, jytsistema.sFechadeTrabajo)) & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.numfac desc) order by numfac "

    Private strSQLMov As String = ""
    Private strSQLIVA As String = ""
    Private strSQLDescuentos As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtRenglones As New DataTable
    Private dtIVA As New DataTable
    Private dtDescuentos As New DataTable

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long, nPosicionDescuento As Long
    Private aEstatus() As String = {"Por confirmar", "Procesada"}
    Private aTarifa() As String = {"A", "B", "C", "D", "E", "F"}
    Private CondicionDePago As Integer = 0
    Private TipoCredito As Integer = 0
    Private Caja As String = "00"
    Private FechaVencimiento As Date = jytsistema.sFechadeTrabajo
    Private MontoAnterior As Double = 0.0
    Private Disponibilidad As Double = 0.0
    Private TarifaCliente As String = "A"

    Private Eliminados As New ArrayList

    Private Impresa As Integer
    Private Sub jsVenArcNotasEntrega_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)

            InsertarAuditoria(myConn, MovAud.ientrar, sModulo, "")

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionEncab = dt.Rows.Count - 1
                Me.BindingContext(ds, nTabla).Position = nPosicionEncab
                AsignaTXT(nPosicionEncab)
            Else
                IniciarDocumento(False)
            End If
            ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexi�n de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva nota de entrega")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> nota de entrega")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> nota de entrega")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> nota de entrega")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primer</B> nota de entrega")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a nota de entrega <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a nota de entrega <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>�ltimo nota de entrega</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> nota de entrega")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnDuplicar, "<B>Duplicar</B> este nota de entrega")

        'Menu barra rengl�n
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> rengl�n en nota de entrega")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> rengl�n en nota de entrega")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> rengl�n en nota de entrega")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un rengl�n en nota de entrega")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> rengl�n en nota de entrega")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> rengl�n en nota de entrega")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al rengl�n <B>siguiente </B> en nota de entrega")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>�ltimo</B> rengl�n de la nota de entrega")
        C1SuperTooltip1.SetToolTip(btnPresupuesto, "Traer <B>presupuesto</B> a la nota de entrega")
        C1SuperTooltip1.SetToolTip(btnPrepedido, "Traer <B>pre-pedido</B> a la nota de entrega")
        C1SuperTooltip1.SetToolTip(btnPedido, "Traer <B>pedido</B> a la nota de entrega")

        'Menu Barra Descuento 
        C1SuperTooltip1.SetToolTip(btnAgregaDescuento, "<B>Agrega </B> descuento global a nota de entrega")
        C1SuperTooltip1.SetToolTip(btnEliminaDescuento, "<B>Elimina</B> descuento global de nota de entrega")


    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)

        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)

        If c >= 0 Then
            Me.BindingContext(ds, nTablaRenglones).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", c, dtRenglones.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtRenglones, MenuBarraRenglon)
        HabilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtCliente, btnCliente)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        i_modo = movimiento.iConsultar

        If nRow > 0 Then
            With dt

                nPosicionEncab = nRow
                Me.BindingContext(ds, nTabla).Position = nRow

                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

                With .Rows(nRow)
                    'Encabezado 
                    txtCodigo.Text = MuestraCampoTexto(.Item("numfac"))
                    txtEmision.Text = FormatoFecha(CDate(.Item("emision").ToString))
                    txtVencimiento.Text = FormatoFecha(CDate(.Item("vence").ToString))

                    Dim numControl As String = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select num_control from jsconnumcon where numdoc = '" & .Item("numfac") & "' and  origen = 'FAC' and org = 'PFC' and id_emp = '" & jytsistema.WorkID & "' ")
                    txtControl.Text = IIf(numControl <> "0", numControl, "")

                    txtEstatus.Text = aEstatus(.Item("estatus"))
                    txtCliente.Text = MuestraCampoTexto(.Item("codcli"))
                    txtComentario.Text = MuestraCampoTexto(.Item("comen"))
                    txtAsesor.Text = MuestraCampoTexto(.Item("codven"))
                    txtTransporte.Text = MuestraCampoTexto(.Item("transporte"))
                    txtAlmacen.Text = MuestraCampoTexto(.Item("almacen"))
                    RellenaCombo(aTarifa, cmbTarifa, InArray(aTarifa, .Item("tarifa")) - 1)
                    txtReferencia.Text = MuestraCampoTexto(.Item("refer"))
                    txtCodigoContable.Text = MuestraCampoTexto(.Item("codcon"))

                    tslblPesoT.Text = FormatoCantidad(.Item("kilos"))

                    txtSubTotal.Text = FormatoNumero(.Item("tot_net"))
                    txtDescuentos.Text = FormatoNumero(.Item("descuen"))
                    txtCargos.Text = FormatoNumero(.Item("cargos"))
                    txtTotalIVA.Text = FormatoNumero(.Item("imp_iva"))
                    txtTotal.Text = FormatoNumero(.Item("tot_fac"))

                    txtCondicionPago.Text = "Condicion de pago : " & IIf(.Item("condpag") = 0, "CREDITO", "CONTADO")
                    MontoAnterior = .Item("tot_fac")
                    CondicionDePago = .Item("condpag")
                    FechaVencimiento = CDate(.Item("vence").ToString)

                    Impresa = .Item("impresa")

                    'Renglones
                    AsignarMovimientos(.Item("numfac"))

                    'Totales
                    CalculaTotales()

                End With
            End With
        End If
    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroNotaEntrega As String)
        strSQLMov = "select * from jsvenrennot " _
                            & " where " _
                            & " numfac  = '" & NumeroNotaEntrega & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"item", "descrip", "iva", "cantidad", "unidad", "precio", "des_art", "des_cli", "des_ofe", "totren", "estatus", ""}
        Dim aNombres() As String = {"Item", "Descripci�n", "IVA", "Cant.", "UND", "Precio Unitario", "Desc. Art.", "Desc. Cli.", "Desc. Ofe.", "Precio Total", "Tipo Renglon", ""}
        Dim aAnchos() As Long = {70, 350, 30, 60, 45, 70, 50, 50, 50, 100, 70, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", sFormatoCantidad, "", sFormatoNumero, sFormatoNumero, sFormatoNumero, _
                                     sFormatoNumero, sFormatoNumero, "", ""}
        IniciarTabla(dg, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub
    Private Sub AbrirIVA(ByVal NumeroDocumento As String)

        strSQLIVA = "select * from jsvenivanot " _
                            & " where " _
                            & " numfac  = '" & NumeroDocumento & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by tipoiva "

        ds = DataSetRequery(ds, strSQLIVA, myConn, nTablaIVA, lblInfo)
        dtIVA = ds.Tables(nTablaIVA)

        Dim aCampos() As String = {"tipoiva", "poriva", "baseiva", "impiva"}
        Dim aNombres() As String = {"", "", "", ""}
        Dim aAnchos() As Long = {15, 45, 65, 60}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {"", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero}
        IniciarTabla(dgIVA, dtIVA, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False, , 8, False)

        txtTotalIVA.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(impiva) from jsvenivanot where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac ")))

    End Sub
    Private Sub AbrirDescuentos(ByVal NumeroDocumento As String)

        strSQLDescuentos = "select * from jsvendesnot " _
                            & " where " _
                            & " numfac  = '" & NumeroDocumento & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLDescuentos, myConn, nTablaDescuentos, lblInfo)
        dtDescuentos = ds.Tables(nTablaDescuentos)

        Dim aCampos() As String = {"renglon", "pordes", "descuento"}
        Dim aNombres() As String = {"", "", ""}
        Dim aAnchos() As Long = {60, 45, 60}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {"", sFormatoNumero, sFormatoNumero}
        IniciarTabla(dgDescuentos, dtDescuentos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False, , 8, False)

        txtDescuentos.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(descuento) from jsvendesnot where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac ")))

    End Sub

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "NETMP" & NumeroAleatorio(100000)
        Else
            txtCodigo.Text = ""
        End If

        IniciarTextoObjetos(FormatoItemListView.iCadena, txtControl, txtCliente, txtNombreCliente, txtAsesor, txtNombreAsesor, txtComentario, txtTransporte, _
                            txtAlmacen, txtReferencia, txtCodigoContable, txtVencimiento, txtCondicionPago)

        Dim nTransporte As String = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, "SELECT codtra FROM jsconctatra WHERE id_emp = '" & jytsistema.WorkID & "' ORDER BY codtra LIMIT 1"))
        If nTransporte <> "0" Then txtTransporte.Text = nTransporte

        Dim nAlmacen As String = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, "SELECT codalm FROM jsmercatalm WHERE id_emp = '" & jytsistema.WorkID & "' ORDER BY codalm LIMIT 1"))
        If nAlmacen <> "0" Then txtAlmacen.Text = nAlmacen

        RellenaCombo(aTarifa, cmbTarifa)
        txtEmision.Text = FormatoFecha(sFechadeTrabajo)

        txtEstatus.Text = aEstatus(0)
        tslblPesoT.Text = FormatoCantidad(0)

        IniciarTextoObjetos(FormatoItemListView.iNumero, txtSubTotal, txtDescuentos, txtCargos, txtTotalIVA, txtTotalIVA)
        txtVencimiento.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        MontoAnterior = 0.0
        CondicionDePago = 1

        dgDisponibilidad.Columns.Clear()
        lblDisponibilidad.Text = "Disponible menos este Documento : 0.00"

        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", 0, 0)
        AsignarMovimientos(txtCodigo.Text)
        AbrirIVA(txtCodigo.Text)
        AbrirDescuentos(txtCodigo.Text)


    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        HabilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon, MenuDescuentos)
        HabilitarObjetos(True, True, txtComentario, btnEmision, btnCliente, txtCliente, cmbTarifa, _
                         btnAsesor, txtAsesor, txtTransporte, btnTransporte, txtAlmacen, btnAlmacen, _
                         txtReferencia, btnCodigoContable)

        VisualizarObjetos(True, grpDisponibilidad)
        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier bot�n de la barra menu...", TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        HabilitarObjetos(False, True, txtCodigo, txtEmision, btnEmision, txtControl, txtEstatus, _
                txtCliente, txtNombreCliente, btnCliente, txtComentario, txtAsesor, btnAsesor, txtNombreAsesor, _
                cmbTarifa, txtVencimiento, txtCondicionPago, txtTransporte, txtAlmacen, btnTransporte, btnAlmacen, _
                txtNombreTransporte, txtNombreAlmacen, txtReferencia, txtCodigoContable, btnCodigoContable)

        HabilitarObjetos(False, True, txtDescuentos, txtCargos, MenuDescuentos, txtSubTotal, txtTotalIVA, txtTotal)
        VisualizarObjetos(False, grpDisponibilidad)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier bot�n de la barra menu...", TipoMensaje.iAyuda)
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

        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrennot where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenivanot where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvendesnot where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'PFC' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim f As New jsGenFormasPago
            f.CondicionPago = CondicionDePago
            f.TipoCredito = TipoCredito
            f.Vencimiento = FechaVencimiento
            f.Caja = Caja
            f.Cargar(myConn, "PFC", txtCodigo.Text, IIf(i_modo = movimiento.iAgregar, True, False), ValorNumero(txtTotal.Text))
            If f.DialogResult = Windows.Forms.DialogResult.OK Then
                CondicionDePago = f.CondicionPago
                TipoCredito = f.TipoCredito
                FechaVencimiento = f.Vencimiento
                Caja = f.Caja
                GuardarTXT()
            End If
        End If
    End Sub
    Private Function Validado() As Boolean


        If txtNombreCliente.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un cliente v�lido...")
            Return False
        End If

        If txtNombreAsesor.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un nombre de Asesor v�lido...")
            Return False
        End If

        If txtNombreTransporte.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un transporte v�lido...")
            Return False
        End If

        If txtNombreAlmacen.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un almac�n v�lido...")
            Return False
        End If


        If dtRenglones.Rows.Count = 0 Then
            MensajeCritico(lblInfo, "Debe incluir al menos un �tem...")
            Return False
        End If

        Dim Disponible As Double = CDbl(Trim(Split(lblDisponibilidad.Text, ":")(1)))
        If Disponible < 0 Then
            MensajeCritico(lblInfo, "Esta nota de entrega excede la disponibilidad...")
            Return False
        End If

        Dim EstatusCliente As Integer = CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select estatus from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
        If EstatusCliente = 1 Or EstatusCliente = 2 Then
            MensajeCritico(lblInfo, "Este Cliente posee estatus  " & aEstatusCliente(EstatusCliente) & ". Favor remitir a Administraci�n")
            Return False
        End If

        If dtRenglones.Rows.Count <= 0 Then
            MensajeCritico(lblInfo, "Debe introducir por lo menos un �tem")
            Return False
        End If

        If Not CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM19")) Then
            If ClientePoseeChequesFuturos(myConn, lblInfo, txtCliente.Text) Then
                MensajeCritico(lblInfo, " NO SE PUEDE REALIZAR ESTA OPERACION PUES ESTE CLIENTE POSEE CHEQUES A FUTURO ")
                Return False
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
            Codigo = Contador(myConn, lblInfo, Gestion.iVentas, "VENNUMNOT", "07")
            EjecutarSTRSQL(myConn, lblInfo, " update jsvenrennot set numfac = '" & Codigo & "' where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            EjecutarSTRSQL(myConn, lblInfo, " update jsvenivanot set numfac = '" & Codigo & "' where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            EjecutarSTRSQL(myConn, lblInfo, " update jsvendesnot set numfac = '" & Codigo & "' where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            EjecutarSTRSQL(myConn, lblInfo, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & txtCodigo.Text & "' and origen = 'PFC' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        InsertEditVENTASEncabezadoNotaEntrega(myConn, lblInfo, Inserta, Codigo, CDate(txtEmision.Text), txtCliente.Text, _
                                               txtComentario.Text, txtAsesor.Text, txtAlmacen.Text, txtTransporte.Text, _
                                               CDate(txtVencimiento.Text), txtReferencia.Text, txtCodigoContable.Text, _
                                               ValorEntero(dtRenglones.Rows.Count), 0.0, ValorCantidad(tslblPesoT.Text), _
                                               ValorNumero(txtSubTotal.Text), 0.0, 0.0, 0.0, 0.0, 0.0, ValorNumero(txtDescuentos.Text), _
                                               0.0, 0.0, 0.0, 0.0, ValorNumero(txtCargos.Text), 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, _
                                               jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, _
                                               CondicionDePago, TipoCredito, "", "", "", "", 0.0, "", "", 0.0, "", "", 0.0, "", "", _
                                               0.0, "", "", 0.0, 0.0, "", 0, 0, 0.0, 0.0, "", jytsistema.sFechadeTrabajo, 0.0, 0.0, _
                                               ValorNumero(txtTotalIVA.Text), 0.0, 0, 0, ValorNumero(txtTotal.Text), InArray(aEstatus, txtEstatus.Text) - 1, _
                                               cmbTarifa.Text, "", "", "", "", "", Impresa)


        ActualizarMovimientos(Codigo)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub ActualizarMovimientos(ByVal NumeroNotaEntrega As String)

        '1.- Aplica Descuento Global sobre total Rengl�n con descuento "totrendes"
        EjecutarSTRSQL(myConn, lblInfo, " update jsvenrennot set totrendes = totren - totren * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
                                        & " where " _
                                        & " numfac = '" & NumeroNotaEntrega & "' and " _
                                        & " renglon > '' and " _
                                        & " item > '' and " _
                                        & " estatus = '0' AND " _
                                        & " aceptado < '2' and " _
                                        & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                        & " id_emp = '" & jytsistema.WorkID & "' ")

        '2.- Elimina movimientos de inventarios anteriores de la Nota de entrega
        EliminarMovimientosdeInventario(myConn, NumeroNotaEntrega, "PFC", lblInfo)

        '3.- Actualizar Movimientos de Inventario con nota de entrega
        strSQLMov = "select * from jsvenrennot " _
                            & " where " _
                            & " numfac  = '" & NumeroNotaEntrega & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        For Each dtRow As DataRow In dtRenglones.Rows
            With dtRow
                Dim CostoActual As Double = UltimoCostoAFecha(myConn, lblInfo, .Item("item"), CDate(txtEmision.Text)) / Equivalencia(myConn, lblInfo, .Item("item"), .Item("unidad"))
                InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(txtEmision.Text), "SA", NumeroNotaEntrega, _
                                                     .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("cantidad") * CostoActual, _
                                                     .Item("cantidad") * CostoActual, "PFC", NumeroNotaEntrega, .Item("lote"), txtCliente.Text, _
                                                     .Item("totren"), .Item("totrendes"), 0, .Item("totren") - .Item("totrendes"), txtAsesor.Text, _
                                                     txtAlmacen.Text, .Item("renglon"), jytsistema.sFechadeTrabajo)

                ActualizarExistenciasPlus(myConn, .Item("item"))
            End With
        Next

        '4.- Actualizar existencias de renglones eliminados
        For Each aSTR As Object In Eliminados
            ActualizarExistenciasPlus(myConn, aSTR)
        Next

        '5.- Actualizar CxC si es una entrega a cr�dito
        If CondicionDePago = CondicionPago.iCredito Then
            EjecutarSTRSQL(myConn, lblInfo, " DELETE from jsventracob WHERE " _
                                            & " TIPOMOV = 'NE' AND  " _
                                            & " NUMMOV = '" & NumeroNotaEntrega & "' AND " _
                                            & " ORIGEN = 'PFC' AND " _
                                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                            & " ID_EMP = '" & jytsistema.WorkID & "'  ")

            InsertEditVENTASCXC(myConn, lblInfo, True, txtCliente.Text, "NE", NumeroNotaEntrega, CDate(txtEmision.Text), FormatoHora(Now), _
                                FechaVencimiento, txtReferencia.Text, "Nota Entrega : " & NumeroNotaEntrega, ValorNumero(txtTotal.Text), _
                                ValorNumero(txtTotalIVA.Text), "", "", "", "", "", "PFC", NumeroNotaEntrega, "0", "", jytsistema.sFechadeTrabajo, _
                                txtCodigoContable.Text, "", "", 0.0, 0.0, "", "", "", "", txtAsesor.Text, txtAsesor.Text, "0", "0", "")


        Else '6.- Actualizar Caja y Bancos si es una entrega a contado

            EjecutarSTRSQL(myConn, lblInfo, "DELETE FROM jsbantracaj WHERE " _
                        & " ORIGEN = 'PFC' AND " _
                        & " TIPOMOV = 'EN' AND " _
                        & " NUMMOV = '" & NumeroNotaEntrega & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "'")

            EjecutarSTRSQL(myConn, lblInfo, "DELETE FROM jsbantraban WHERE " _
                        & " ORIGEN = 'PFC' AND " _
                        & " TIPOMOV = 'DP' AND " _
                        & " NUMORG = '" & NumeroNotaEntrega & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' ")


            Dim hCont As Integer
            Dim dtFP As DataTable
            Dim nTablaFP As String = "tblFP"
            ds = DataSetRequery(ds, "select * from jsvenforpag where numfac = '" & NumeroNotaEntrega & "' and origen = 'PFC' and id_emp = '" & jytsistema.WorkID & "' ", _
                                 myConn, nTablaFP, lblInfo)
            dtFP = ds.Tables(nTablaFP)

            For hCont = 0 To dtFP.Rows.Count - 1
                With dtFP.Rows(hCont)
                    Select Case .Item("formapag")
                        Case "EF", "CH", "TA"

                            Dim cajaFP As String = "VENPARAM" & IIf(.Item("FORMAPAG") = "EF", "12", _
                                IIf(.Item("FORMAPAG") = "CH", "13", IIf(.Item("FORMAPAG") = "TA", "14", "11")))

                            Dim aCaja As String = ParametroPlus(MyConn, Gestion.iVentas, cajaFP)

                            InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, aCaja, UltimoCajaMasUno(myConn, lblInfo, aCaja), _
                                                        CDate(.Item("VENCE").ToString), "PFC", "EN", NumeroNotaEntrega, .Item("formapag"), _
                                                        .Item("numpag"), .Item("nompag"), .Item("importe"), "", _
                                                        "NOTA DE ENTREGA N� :" & NumeroNotaEntrega, "", jytsistema.sFechadeTrabajo, 1, "", 0, "", _
                                                        jytsistema.sFechadeTrabajo, txtCliente.Text, txtAsesor.Text, "1")

                            CalculaSaldoCajaPorFP(myConn, aCaja, .Item("formapag"), lblInfo)
                        Case "CT"

                            Dim CantidadXCorredor As Integer = EjecutarSTRSQL_Scalar(myConn, lblInfo, _
                                                                                       "select count(*) AS CANTIDAD from jsventabtic WHERE " _
                                                                                       & " ID_EMP = '" & jytsistema.WorkID & "' AND " _
                                                                                       & " CORREDOR = '" & .Item("NOMPAG") & "' AND " _
                                                                                       & " NUMCAN = '" & NumeroNotaEntrega & "' GROUP BY CORREDOR  ")

                            Dim aCaja As String = ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM11")


                            InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, aCaja, UltimoCajaMasUno(myConn, lblInfo, aCaja), _
                                                CDate(.Item("VENCE").ToString), "PFC", "EN", NumeroNotaEntrega, "CT", _
                                                 .Item("numpag"), .Item("nompag"), .Item("importe"), "", _
                                                "NOTA DE ENTREGA N� :" & NumeroNotaEntrega, "", jytsistema.sFechadeTrabajo, CantidadXCorredor, "", 0, "", _
                                                jytsistema.sFechadeTrabajo, txtCliente.Text, txtAsesor.Text, "1")

                            CalculaSaldoCajaPorFP(myConn, aCaja, "CT", lblInfo)

                        Case "DP"

                            InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, CDate(.Item("VENCE").ToString), .Item("NUMPAG"), .Item("FORMAPAG"), _
                                                            .Item("NOMPAG"), "", "NOTA DE ENTREGA N� :" & NumeroNotaEntrega, .Item("IMPORTE"), "PFC", NumeroNotaEntrega, _
                                                            "", NumeroNotaEntrega, "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, "NE", _
                                                            "", jytsistema.sFechadeTrabajo, "0", txtCliente.Text, txtAsesor.Text)

                            CalculaSaldoBanco(myConn, lblInfo, .Item("nompag"), True, jytsistema.sFechadeTrabajo)

                    End Select
                End With
            Next

            dtFP.Dispose()
            dtFP = Nothing

        End If

        '7.- Actualiza Cantidades en tr�nsito de los presupuestos y estatus del documento
        ActualizarRenglonesEnPresupuestos(myConn, lblInfo, ds, dtRenglones, "jsvenrennot")

        '8.- Actualiza cantidades en tr�nsito de los prepedidos y estatus de documento
        ActualizarRenglonesEnPrepedidos(myConn, lblInfo, ds, dtRenglones, "jsvenrennot")

        '9.- Actualiza cantidades en tr�nsito de los pedidos y estatus del documento
        ActualizarRenglonesEnPedidos(myConn, lblInfo, ds, dtRenglones, "jsvenrennot")


    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        MensajeEtiqueta(lblInfo, " Indique comentario ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
        HabilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtCliente, btnCliente)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNOTPA01")) Then
            i_modo = movimiento.iEditar
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position
            ActivarMarco0()
            HabilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtCliente, btnCliente)
        Else
            MensajeCritico(lblInfo, "Edici�n de Notas de Entrega NO est� permitida...")
        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNOTPA01")) Then
            Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position

            sRespuesta = MsgBox(" � Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")

            If sRespuesta = MsgBoxResult.Yes Then

                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numfac"))

                For Each dtRow As DataRow In dtRenglones.Rows
                    With dtRow
                        Eliminados.Add(.Item("item"))
                    End With
                Next

                EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenencnot where numfac = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "'")

                EjecutarSTRSQL(myConn, lblInfo, " DELETE from jsvendesnot where NUMFAC = '" & txtCodigo.Text & "' and ID_EMP = '" & jytsistema.WorkID & "' ")
                EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrennot where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenivanot where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'PFC' and id_emp = '" & jytsistema.WorkID & "' ")
                EliminarMovimientosdeInventario(myConn, txtCodigo.Text, "PFC", lblInfo)

                For Each aSTR As Object In Eliminados
                    ActualizarExistenciasPlus(myConn, aSTR)
                Next

                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                dt = ds.Tables(nTabla)
                If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                AsignaTXT(nPosicionEncab)

            End If
        Else
            MensajeCritico(lblInfo, "Eliminaci�n de Notas de Entrega NO est� permitida...")
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numfac", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"N�mero ", "Nombre", "Emisi�n", "Comentario"}
        Dim Anchos() As Long = {150, 400, 100, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Notas de entrega...")
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
        Dim aTipoRenglon() As String = {"Normal", "Sin Desc.", "Bonificaci�n"}
        Select Case e.ColumnIndex
            Case 10
                e.Value = aTipoRenglon(e.Value)
        End Select
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "ItemsRenglon", "lblItemsRenglon", e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub CalculaTotales()

        txtSubTotal.Text = FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrennot", "numfac", "totren", txtCodigo.Text, 0))
        AbrirDescuentos(txtCodigo.Text)
        txtCargos.Text = FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrennot", "numfac", "totren", txtCodigo.Text, 1))
        CalculaDescuentoEnRenglones()
        CalculaTotalIVAVentas(myConn, lblInfo, "jsvendesnot", "jsvenivanot", "jsvenrennot", "numfac", txtCodigo.Text, "impiva", "totrendes", CDate(txtEmision.Text), "totren")
        AbrirIVA(txtCodigo.Text)
        txtTotal.Text = FormatoNumero(ValorNumero(txtSubTotal.Text) - ValorNumero(txtDescuentos.Text) + ValorNumero(txtCargos.Text) + ValorNumero(txtTotalIVA.Text))

        tslblPesoT.Text = FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsvenrennot", "peso", "numfac", txtCodigo.Text))

    End Sub

    Private Sub CalculaDescuentoEnRenglones()

        EjecutarSTRSQL_Scalar(myConn, lblInfo, " update jsvenrennot set totrendes = totren - totren * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
            & " where " _
            & " numfac = '" & txtCodigo.Text & "' and " _
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
            f.Agregar(myConn, ds, dtRenglones, "PFC", txtCodigo.Text, CDate(txtEmision.Text), txtAlmacen.Text, cmbTarifa.Text, txtCliente.Text, , , , , , , , , , txtAsesor.Text)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        If dtRenglones.Rows.Count > 0 Then
            With dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position)
                If .Item("EDITABLE") = 0 Then
                    Dim f As New jsGenRenglonesMovimientos
                    f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
                    f.Editar(myConn, ds, dtRenglones, "PFC", txtCodigo.Text, CDate(txtEmision.Text), txtAlmacen.Text, cmbTarifa.Text, txtCliente.Text, _
                             IIf(.Item("item").ToString.Substring(1, 1) = "$", True, False), , , , , , , , , txtAsesor.Text)
                    ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
                    AsignaMov(f.Apuntador, True)
                    CalculaTotales()
                    f = Nothing
                Else
                    MensajeCritico(lblInfo, "Rengl�n NO EDITABLE...")
                End If
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
            sRespuesta = MsgBox(" � Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
            If sRespuesta = MsgBoxResult.Yes Then
                With dtRenglones.Rows(nPosicionRenglon)

                    If .Item("EDITABLE") = 0 Then
                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("item"))

                        Eliminados.Add(.Item("item"))

                        Dim aCamposDel() As String = {"numfac", "item", "renglon", "id_emp"}
                        Dim aStringsDel() As String = {txtCodigo.Text, .Item("item"), .Item("renglon"), jytsistema.WorkID}

                        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and " _
                               & " origen = 'PFC' and item = '" & .Item("item") & "' and renglon = '" & .Item("renglon") & "' and " _
                               & " id_emp = '" & jytsistema.WorkID & "' ")

                        nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsvenrennot", strSQLMov, aCamposDel, aStringsDel, _
                                                      Me.BindingContext(ds, nTablaRenglones).Position, True)

                        If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1
                        AsignaMov(nPosicionRenglon, True)
                        CalculaTotales()
                    Else
                        MensajeCritico(lblInfo, "Rengl�n NO EDITABLE...")

                    End If
                End With
            End If
        End If

    End Sub
    Private Sub btnBuscarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarMovimiento.Click

        Dim f As New frmBuscar
        Dim Campos() As String = {"item", "descripcion"}
        Dim Nombres() As String = {"Item", "Descripci�n"}
        Dim Anchos() As Long = {140, 350}
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


        'If DocumentoImpreso(myConn, lblInfo, "jsvenencnot", "numfac", txtCodigo.Text) Then
        'If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPEDPA07")) Then
        'MensajeCritico(lblInfo, "Nota de Entrega YA fu� impresa...")
        'Else
        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cNotaDeEntrega, "Nota de Entrega", txtCliente.Text, txtCodigo.Text, CDate(txtEmision.Text))
        f = Nothing
        'End If
        'Else
        'If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPEDPA08")) Then
        '    '1. Imprimir Nota de Entrega Fiscal
        '    '2. Colocar Nota de Entrega como impresa
        'Else
        '    MensajeCritico(lblInfo, "Impresi�n de NOTA DE ENTREGA no permitida...")
        'End If
        'End If

    End Sub
    Private Sub btnEmision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmision.Click
        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNOTPA03")) Then
            txtEmision.Text = SeleccionaFecha(CDate(txtEmision.Text), Me, sender)
        End If
    End Sub

    Private Sub btnAsesor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesor.Click
        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNOTPA09")) Then
            txtAsesor.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, CONCAT( apellidos, ', ', nombres) descripcion from jsvencatven where tipo = '" & TipoVendedor.iFuerzaventa & "'  and estatus = 1 and id_emp = '" & jytsistema.WorkID & "'  order by 1 ", "Asesores Comerciales", _
                                               txtAsesor.Text)
        Else
            MensajeCritico(lblInfo, "Escogencia de asesor comercial no permitida...")
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

            Disponibilidad = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select disponible from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            Dim mTotalFac As Double = ValorNumero(txtTotal.Text)
            If i_modo = movimiento.iAgregar Then
                mTotalFac = 0.0
                FechaVencimiento = DateAdd(DateInterval.Day, DiasCreditoAlVencimiento(myConn, lblInfo, FormaDePagoCliente), CDate(txtEmision.Text))
                CondicionDePago = CondicionPagoProveedorCliente(myConn, lblInfo, FormaDePagoCliente)
                If qFound(myConn, lblInfo, "jsvencatcli", aFld, aStr) Then
                    cmbTarifa.SelectedIndex = InArray(aTarifa, qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "tarifa")) - 1
                    Dim cAsesor As String = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, "SELECT IF( b.codven IS NULL, '0', b.codven) codven  " _
                                                            & " FROM jsvenrenrut a  " _
                                                            & " LEFT JOIN jsvenencrut b ON (a.codrut = b.codrut AND a.tipo = b.tipo AND a.id_emp = b.id_emp) " _
                                                            & " WHERE " _
                                                            & " a.tipo = '0' AND " _
                                                            & " a.cliente = '" & txtCliente.Text & "' AND " _
                                                            & " a.id_emp = '" & jytsistema.WorkID & "' "))

                    If cAsesor = "0" Then cAsesor = ""
                    txtAsesor.Text = cAsesor
                    If qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "transporte") <> "" Then txtTransporte.Text = qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "transporte")
                End If

            End If
            lblDisponibilidad.Text = "Disponible menos este Documento : " & FormatoNumero(Disponibilidad + MontoAnterior - mTotalFac)
            If i_modo = movimiento.iAgregar Then IncluirFleteAutomatico(txtCliente.Text, montoFletesPorRegion(myConn, txtCliente.Text))

        End If
    End Sub
    Private Sub IncluirFleteAutomatico(Codigocliente As String, montoFlete As Double)

        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA24")) Then

            Dim codServicioFlete As String = CStr(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA25"))
            Dim monServicioFlete As Double = montoFlete
            If monServicioFlete = 0 Then
                monServicioFlete = CDbl(EjecutarSTRSQL_ScalarPLUS(myConn, " select precio from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'"))
            End If

            Dim desServicioFlete As String = CStr(EjecutarSTRSQL_ScalarPLUS(myConn, " select desser from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'"))
            Dim ivaServicioFlete As String = CStr(EjecutarSTRSQL_ScalarPLUS(myConn, " select tipoiva from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'"))

            Dim numRenglon As String = AutoCodigoPlus(myConn, "renglon", "jsvenrennot", "numfac", txtCodigo.Text, 5)

            If monServicioFlete <> 0 Then

                InsertEditVENTASRenglonNotasEntrega(myConn, lblInfo, True, txtCodigo.Text, numRenglon, "$" & codServicioFlete, desServicioFlete, _
                                                    ivaServicioFlete, "", "UND", 0.0, 1.0, _
                                                    0.0, 0.0, 0, "", "", 0.0, "", "1", _
                                                    monServicioFlete, 0.0, 0.0, _
                                                    0.0, monServicioFlete, monServicioFlete, _
                                                    "", "", "", "", "", "", _
                                                    "", "1", 1)

                ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
                AsignaMov(0, True)
                CalculaTotales()

            End If


        End If

    End Sub


    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dgIVA.CellFormatting
        If e.ColumnIndex = 1 Then e.Value = FormatoNumero(e.Value) & "%"
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
                MensajeCritico(lblInfo, "DESCUENTOS NO PERMITIDOS")
            Else
                Dim f As New jsGenDescuentosVentas
                f.Agregar(myConn, ds, dtDescuentos, "jsvendesnot", "numfac", txtCodigo.Text, sModulo, txtAsesor.Text, 0, CDate(txtEmision.Text), ValorNumero(txtSubTotal.Text))
                CalculaTotales()
                f = Nothing
            End If
        End If

    End Sub
    Private Sub btnEliminaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaDescuento.Click
        If nPosicionDescuento >= 0 Then
            With dtDescuentos.Rows(nPosicionDescuento)
                Dim aCamposDel() As String = {"numfac", "renglon", "id_emp"}
                Dim aStringsDel() As String = {txtCodigo.Text, .Item("renglon"), jytsistema.WorkID}
                nPosicionDescuento = EliminarRegistros(myConn, lblInfo, ds, nTablaDescuentos, "jsvendesnot", strSQLDescuentos, aCamposDel, aStringsDel, _
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
            f.Agregar(myConn, ds, dtRenglones, "PFC", txtCodigo.Text, CDate(txtEmision.Text), txtAlmacen.Text, cmbTarifa.Text, txtCliente.Text, True)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Private Sub txtNombreCliente_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNombreCliente.TextChanged
        If txtNombreCliente.Text = "" Then dgDisponibilidad.Columns.Clear()
    End Sub

    Private Sub txtTransporte_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTransporte.TextChanged
        txtNombreTransporte.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select nomtra from jsconctatra where codtra = '" & txtTransporte.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub txtAlmacen_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlmacen.TextChanged
        txtNombreAlmacen.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select desalm from jsmercatalm where codalm = '" & txtAlmacen.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub btnTransporte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransporte.Click
        txtTransporte.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codtra codigo, nomtra descripcion from jsconctatra where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Transportes", _
                                               txtTransporte.Text)
    End Sub

    Private Sub btnAlmacen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacen.Click
        txtAlmacen.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codalm codigo, desalm descripcion from jsmercatalm where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Almacenes", _
                                            txtAlmacen.Text)
    End Sub

    Private Sub btnCodigoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigoContable.Click
        txtCodigoContable.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables", txtCodigoContable.Text)

    End Sub
    Private Sub dgDescuentos_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgDescuentos.RowHeaderMouseClick, _
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
                    EjecutarSTRSQL(myConn, lblInfo, " update jsvenrennot " _
                                   & " set precio = " & f.NuevoPrecio & ", " _
                                   & " totren = " & f.NuevoPrecio * .Item("cantidad") & ", " _
                                   & " totrendes = " & f.NuevoPrecio * .Item("cantidad") * (1 - .Item("des_art") / 100) * (1 - .Item("des_cli") / 100) * (1 - .Item("des_ofe") / 100) & " " _
                                   & " where " _
                                   & " numfac = '" & txtCodigo.Text & "' and " _
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