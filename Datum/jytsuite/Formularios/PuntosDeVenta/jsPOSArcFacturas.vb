Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsPOSArcFacturas
    Private Const sModulo As String = "Facturas"
    Private Const lRegion As String = "RibbonButton85"
    Private Const nTabla As String = "tblEncabFacturas"
    Private Const nTablaRenglones As String = "tblRenglones_Facturas"
    Private Const nTablaIVA As String = "tblIVA"
    Private Const nTablaDescuentos As String = "tblDescuentos"
    Private Const nTablaFP As String = "tblFormapago"



    Private strSQLMov As String = ""
    Private strSQLIVA As String = ""
    Private strSQLDescuentos As String = ""
    Private strSQLFP As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtRenglones As New DataTable
    Private dtIVA As New DataTable
    Private dtDescuentos As New DataTable
    Private dtFP As New DataTable
    Private ft As New Transportables

    Private strSQL As String = " (select a.* from jsvenencpos a " _
           & " where " _
           & " a.emision >= '" & ft.FormatoFechaMySQL(DateAdd("m", -MesesAtras.i4, jytsistema.sFechadeTrabajo)) & "' and " _
           & " a.emision <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
           & " a.id_emp = '" & jytsistema.WorkID & "' order by a.numfac desc) order by numfac "

    Private i_modo As Integer

    Private nPosicionEncab As Long, nPosicionRenglon As Long, nPosicionDescuento As Long

    Private aEstatus() As String = {"Por Confirmar", "Procesada", "Anulada"}
    Private aTarifa() As String = {"A", "B", "C", "D", "E", "F"}

    Private CondicionDePago As Integer = 0
    Private TipoCredito As Integer = 0

    Private FechaVencimiento As Date = jytsistema.sFechadeTrabajo
    Private MontoAnterior As Double = 0.0
    Private Disponibilidad As Double = 0.0
    Private TarifaCliente As String = "A"

    Private Eliminados As New ArrayList

    Private Impresa As Integer
    Private TIPODOCUMENTO As Integer
    Private NUMEROSERIAL As String

    Private Sub jsPOSArcFacturas_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsVenArcFacturas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)

            Dim dates As SfDateTimeEdit() = {txtEmision}
            SetSizeDateObjects(dates)

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
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva Factura")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> Factura")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> Factura")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> Factura")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primer</B> Factura")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a Factura <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a Factura <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>último Factura</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> Factura")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnDuplicar, "<B>Duplicar</B> este Factura")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en Factura")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón en Factura")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón en Factura")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en Factura")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón en Factura")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón en Factura")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> en Factura")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la Factura")
        C1SuperTooltip1.SetToolTip(btnPresupuesto, "Traer <B>presupuesto</B> a la Factura")
        C1SuperTooltip1.SetToolTip(btnPrepedido, "Traer <B>pre-pedido</B> a la Factura")
        C1SuperTooltip1.SetToolTip(btnPedido, "Traer <B>pedido</B> a la Factura")

        'Menu Barra Descuento 
        C1SuperTooltip1.SetToolTip(btnAgregaDescuento, "<B>Agrega </B> descuento global a Factura")
        C1SuperTooltip1.SetToolTip(btnEliminaDescuento, "<B>Elimina</B> descuento global de Factura")



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

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)


        If nRow >= 0 Then
            With dt

                nPosicionEncab = nRow
                Me.BindingContext(ds, nTabla).Position = nRow

                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

                With .Rows(nRow)
                    'Encabezado 
                    If ExisteCampo(myConn, lblInfo, jytsistema.WorkDataBase, "jsvenencpos", "tipo") Then
                        lblFactura.Text = IIf(.Item("TIPO") = 0, "FACTURA N°", "DEVOLUCION N°")
                        TIPODOCUMENTO = .Item("TIPO")
                    End If
                    If ExisteCampo(myConn, lblInfo, jytsistema.WorkDataBase, "jsvenencpos", "numserial") Then
                        NUMEROSERIAL = .Item("NUMSERIAL")
                        txtControl.Text = ft.muestraCampoTexto(.Item("numserial"))

                    End If


                    txtCodigo.Text = ft.muestraCampoTexto(.Item("numfac"))
                    txtEmision.Value = CDate(.Item("emision").ToString)
                    txtVencimiento.Text = ft.FormatoFecha(CDate(.Item("vence").ToString))



                    txtEstatus.Text = aEstatus(.Item("estatus"))
                    txtCliente.Text = ft.muestraCampoTexto(.Item("codcli"))
                    txtNombreCliente.Text = ft.muestraCampoTexto(.Item("nomcli"))
                    txtComentario.Text = ft.muestraCampoTexto(.Item("comen"))
                    txtAsesor.Text = ft.muestraCampoTexto(.Item("codven"))
                    txtCaja.Text = ft.muestraCampoTexto(.Item("codcaj"))
                    txtAlmacen.Text = ft.muestraCampoTexto(.Item("almacen"))
                    ft.RellenaCombo(aTarifa, cmbTarifa, ft.InArray(aTarifa, .Item("tarifa")))
                    txtReferencia.Text = ft.muestraCampoTexto(.Item("refer"))
                    txtCodigoContable.Text = ft.muestraCampoTexto(.Item("codcon"))
                    If txtControl.Text.Trim = "" Then
                        Dim nControl As String = ft.DevuelveScalarCadena(myConn, " select num_control from jsconnumcon " _
                                                                       & " where " _
                                                                       & " numdoc = '" & txtCodigo.Text & "' and " _
                                                                       & " prov_cli = '" & txtCliente.Text & "' and " _
                                                                       & " origen = 'PVE' and org = 'PVE' and " _
                                                                       & " id_emp = '" & jytsistema.WorkID & "' ")
                        If nControl <> "0" Then txtControl.Text = nControl
                    End If
                    tslblPesoT.Text = ft.FormatoCantidad(.Item("kilos"))

                    txtSubTotal.Text = ft.FormatoNumero(.Item("tot_net"))
                    txtDescuentos.Text = ft.FormatoNumero(.Item("descuen"))
                    txtCargos.Text = ft.FormatoNumero(.Item("cargos"))
                    txtTotalIVA.Text = ft.FormatoNumero(.Item("imp_iva"))
                    txtTotal.Text = ft.FormatoNumero(.Item("tot_fac"))

                    txtCondicionPago.Text = "Condicion de pago : " & IIf(.Item("condpag") = 0, "CREDITO", "CONTADO ")

                    MontoAnterior = .Item("tot_fac")
                    CondicionDePago = .Item("condpag")
                    FechaVencimiento = CDate(.Item("vence").ToString)

                    Impresa = .Item("impresa")

                    AbrirFormasDePago(.Item("numfac"), NUMEROSERIAL)

                    'Renglones
                    AsignarMovimientos(.Item("numfac"))

                    'Totales
                    CalculaTotales()

                    'ft.mensajeInformativo("")

                End With
            End With
        Else
            IniciarDocumento(False)
        End If

    End Sub
    Private Sub AbrirFormasDePago(ByVal NumeroDocumento As String, ByVal NumeroSerial As String)

        strSQLFP = "select * from jsvenforpag " _
                            & " where " _
                            & " numfac  = '" & NumeroDocumento & "' and " _
                            & " numserial = '" & NumeroSerial & "' and " _
                            & " origen =  'PVE' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by formapag "

        ds = DataSetRequery(ds, strSQLFP, myConn, nTablaFP, lblInfo)
        dtFP = ds.Tables(nTablaFP)

        Dim aC() As String = {"formapag", "numpag", "nompag", "importe", "vence"}
        Dim aN() As String = {"", "", "", "", ""}
        Dim aA() As Integer = {30, 90, 50, 70, 40}
        Dim aL() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda,
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro}

        Dim aFormatos() As String = {"", "", "", sFormatoNumero, sFormatoFechaCorta}
        IniciarTabla(dgFP, dtFP, aC, aN, aA, aL, aFormatos, False, , 8, False)

    End Sub

    Private Sub AsignarMovimientos(ByVal NumeroFactura As String)
        strSQLMov = " select * from jsvenrenpos " _
                            & " where " _
                            & " numfac  = '" & NumeroFactura & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"item", "descrip", "iva", "cantidad", "unidad", "precio", "des_art", "des_cli", "des_ofe", "totren", "estatus", ""}
        Dim aNombres() As String = {"Item", "Descripción", "IVA", "Cant.", "UND", "Precio Unitario", "Desc. Art.", "Desc. Cli.", "Desc. Ofe.", "Precio Total", "Tipo Renglon", ""}
        Dim aAnchos() As Integer = {70, 300, 30, 60, 45, 70, 50, 50, 50, 120, 70, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda,
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", sFormatoCantidad, "", sFormatoNumero, sFormatoNumero, sFormatoNumero,
                                     sFormatoNumero, sFormatoNumero, "", ""}
        IniciarTabla(dg, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub
    Private Sub AbrirIVA(ByVal NumeroDocumento As String)

        strSQLIVA = "select * from jsvenivapos " _
                            & " where " _
                            & " numfac  = '" & NumeroDocumento & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by tipoiva "

        ds = DataSetRequery(ds, strSQLIVA, myConn, nTablaIVA, lblInfo)
        dtIVA = ds.Tables(nTablaIVA)

        Dim aCampos() As String = {"tipoiva", "poriva", "baseiva", "impiva"}
        Dim aNombres() As String = {"", "", "", ""}
        Dim aAnchos() As Integer = {15, 45, 65, 60}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {"", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero}
        IniciarTabla(dgIVA, dtIVA, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False, , 8, False)

        txtTotalIVA.Text = ft.FormatoNumero(ft.DevuelveScalarDoble(myConn, " select SUM(IMPIVA) from jsvenivapos where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac "))

    End Sub
    Private Sub AbrirDescuentos(ByVal NumeroDocumento As String)

        strSQLDescuentos = "select * from jsvendespos " _
                            & " where " _
                            & " numfac  = '" & NumeroDocumento & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLDescuentos, myConn, nTablaDescuentos, lblInfo)
        dtDescuentos = ds.Tables(nTablaDescuentos)

        Dim aCampos() As String = {"renglon", "pordes", "descuento"}
        Dim aNombres() As String = {"", "", ""}
        Dim aAnchos() As Integer = {60, 45, 60}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {"", sFormatoNumero, sFormatoNumero}
        IniciarTabla(dgDescuentos, dtDescuentos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False, , 8, False)

        txtDescuentos.Text = ft.FormatoNumero(ft.DevuelveScalarDoble(myConn, " select SUM(DESCUENTO) from jsvendespos where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac "))

    End Sub

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "FCTMP" & ft.NumeroAleatorio(100000)
        Else
            txtCodigo.Text = ""
        End If

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtControl, txtCliente, txtNombreCliente, txtAsesor, txtNombreAsesor, txtComentario, txtCaja,
                            txtAlmacen, txtReferencia, txtCodigoContable, txtVencimiento, txtCondicionPago)

        Dim nTransporte As String = ft.DevuelveScalarCadena(myConn, "SELECT codtra FROM jsconctatra WHERE id_emp = '" & jytsistema.WorkID & "' ORDER BY codtra LIMIT 1")
        If nTransporte <> "0" Then txtCaja.Text = nTransporte

        Dim nAlmacen As String = ft.DevuelveScalarCadena(myConn, "SELECT codalm FROM jsmercatalm WHERE id_emp = '" & jytsistema.WorkID & "' ORDER BY codalm LIMIT 1")
        If nAlmacen <> "0" Then txtAlmacen.Text = nAlmacen

        ft.RellenaCombo(aTarifa, cmbTarifa)
        txtEmision.Value = sFechadeTrabajo

        txtEstatus.Text = aEstatus(0)
        tslblPesoT.Text = ft.FormatoCantidad(0)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtSubTotal, txtDescuentos, txtCargos, txtTotalIVA, txtTotalIVA)
        txtVencimiento.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        MontoAnterior = 0.0
        CondicionDePago = 1

        dgDisponibilidad.Columns.Clear()
        lblDisponibilidad.Text = "Disponible menos este Documento : 0.00"

        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(txtCodigo.Text)
        AbrirIVA(txtCodigo.Text)
        AbrirDescuentos(txtCodigo.Text)


    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon, MenuDescuentos)
        ft.habilitarObjetos(True, True, txtComentario, txtEmision, btnCliente, txtCliente, cmbTarifa,
                         btnAsesor, txtAsesor, txtCaja, btnTransporte, txtAlmacen, btnAlmacen,
                         txtReferencia, btnCodigoContable)

        ft.visualizarObjetos(True, grpDisponibilidad)
        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtEmision, txtControl, txtEstatus,
                txtCliente, txtNombreCliente, btnCliente, txtComentario, txtAsesor, btnAsesor, txtNombreAsesor,
                cmbTarifa, txtVencimiento, txtCondicionPago, txtCaja, txtAlmacen, btnTransporte, btnAlmacen,
                txtNombreTransporte, txtNombreAlmacen, txtReferencia, txtCodigoContable, btnCodigoContable)

        ft.habilitarObjetos(False, True, txtDescuentos, txtCargos, MenuDescuentos, txtSubTotal, txtTotalIVA, txtTotal)
        ft.visualizarObjetos(False, grpDisponibilidad)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)
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

        ft.Ejecutar_strSQL(myConn, " delete from jsvenrenpos where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvenivapos where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvendespos where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'PVE' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim f As New jsGenFormasPago
            f.CondicionPago = CondicionDePago
            f.TipoCredito = TipoCredito
            f.Vencimiento = FechaVencimiento
            'f.Caja = Caja
            f.Cargar(myConn, "PVE", txtCodigo.Text, IIf(i_modo = movimiento.iAgregar, True, False), ValorNumero(txtTotal.Text), jytsistema.WorkCurrency.Id)
            If f.DialogResult = Windows.Forms.DialogResult.OK Then
                CondicionDePago = f.CondicionPago
                TipoCredito = f.TipoCredito
                FechaVencimiento = f.Vencimiento
                'Caja = f.Caja
                GuardarTXT()
                'Imprimir()
            End If
        End If
    End Sub
    Private Sub Imprimir()

        If DocumentoImpreso(myConn, lblInfo, "jsvenencpos", "numfac", txtCodigo.Text) Then
            'If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA07")) Then
            ' ft.MensajeCritico( "Este documento YA fué impreso...")
            'Else
            Dim f As New jsVenRepParametros
            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cFactura, "Factura", txtCliente.Text, txtCodigo.Text, CDate(txtEmision.Text))
            f = Nothing
            'End If
        Else
            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA08")) Then
                '1. Imprimir Nota de Entrega Fiscal
                '2. Colocar Nota de Entrega como impresa
                ImprimirFacturaGraficaPOS(myConn, lblInfo, ds, txtCodigo.Text)
            Else
                ft.mensajeCritico("Impresión de FACTURA no permitida...")
            End If
        End If

    End Sub
    Private Function Validado() As Boolean


        If txtNombreCliente.Text = "" Then
            ft.mensajeCritico("Debe indicar un cliente válido...")
            Return False
        End If

        If txtNombreAsesor.Text = "" Then
            ft.mensajeCritico("Debe indicar un nombre de Cajero válido...")
            Return False
        End If

        If txtNombreTransporte.Text = "" Then
            ft.mensajeCritico("Debe indicar una caja válida...")
            Return False
        End If

        If txtNombreAlmacen.Text = "" Then
            ft.mensajeCritico("Debe indicar un almacén válido...")
            Return False
        End If


        If dtRenglones.Rows.Count = 0 Then
            ft.mensajeCritico("Debe incluir al menos un ítem...")
            Return False
        End If

        Dim Disponible As Double = CDbl(Trim(Split(lblDisponibilidad.Text, ":")(1)))

        'If Disponible < 0 Then
        ' ft.MensajeCritico( "Esta Factura excede la disponibilidad...")
        ' Return False
        ' End If

        'Dim EstatusCliente As Integer = ft.DevuelveScalarEntero(Myconn,  " select estatus from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
        'If EstatusCliente = 1 Or EstatusCliente = 2 Then
        ' ft.MensajeCritico( "Este Cliente posee estatus  " & aEstatusCliente(EstatusCliente) & ". Favor remitir a Administración")
        ' Return False
        ' End If
        '
        If dtRenglones.Rows.Count <= 0 Then
            ft.mensajeCritico("Debe introducir por lo menos un ítem")
            Return False
        End If

        '       If Not ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM19") Then
        'If ClientePoseeChequesFuturos(myConn, lblInfo, txtCliente.Text) Then
        'ft.MensajeCritico( " NO SE PUEDE REALIZAR ESTA OPERACION PUES ESTE CLIENTE POSEE CHEQUES A FUTURO ")
        'R'eturn False
        'End If
        'End If



        Return True

    End Function
    Private Sub GuardarTXT()

        Dim Codigo As String = txtCodigo.Text
        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count
            Codigo = Contador(myConn, lblInfo, Gestion.iVentas, "VENNUMFAC", "08")

            ft.Ejecutar_strSQL(myConn, " update jsvenrenpos set numfac = '" & Codigo & "' where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvenivapos set numfac = '" & Codigo & "' where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvendespos set numfac = '" & Codigo & "' where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & txtCodigo.Text & "' and origen = 'PVE' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvenforpag set numfac = '" & Codigo & "' where numfac = '" & txtCodigo.Text & "' and origen = 'PVE' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        'InsertarModificarPOSEncabezadoPuntoDeVenta(myConn, lblInfo, Inserta, Codigo, CDate(txtEmision.Text), txtCliente.Text, _
        '                                       txtComentario.Text, txtAsesor.Text, txtAlmacen.Text, txtCaja.Text, _
        '                                       CDate(txtVencimiento.Text), txtReferencia.Text, txtCodigoContable.Text, _
        '                                       ValorEntero(dtRenglones.Rows.Count), 0.0, ValorCantidad(tslblPesoT.Text), _
        '                                       ValorNumero(txtSubTotal.Text), 0.0, 0.0, 0.0, 0.0, 0.0, ValorNumero(txtDescuentos.Text), _
        '                                       0.0, 0.0, 0.0, 0.0, ValorNumero(txtCargos.Text), 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, _
        '                                       jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, _
        '                                       CondicionDePago, TipoCredito, "", "", "", "", 0.0, "", "", 0.0, "", "", 0.0, "", "", _
        '                                       0.0, "", "", 0.0, 0.0, "", 0, 0, 0.0, 0.0, "", jytsistema.sFechadeTrabajo, 0.0, 0.0, _
        '                                       ValorNumero(txtTotalIVA.Text), 0.0, 0, 1, ValorNumero(txtTotal.Text), _
        '                                       IIf(i_modo = movimiento.iAgregar, 1, ft.InArray(aEstatus, txtEstatus.Text)), _
        '                                       cmbTarifa.Text, "", "", "", "", "", Impresa)


        ActualizarMovimientos(Codigo)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)


        Dim row As DataRow = dt.Select(" NUMFAC = '" & Codigo & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub ActualizarMovimientos(ByVal NumeroFactura As String)
        '
        '1.- Aplica Descuento Global sobre total Renglón con descuento "totrendes"
        ft.Ejecutar_strSQL(myConn, " update jsvenrenpos set totrendes = totren - totren * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
                                        & " where " _
                                        & " numfac = '" & NumeroFactura & "' and " _
                                        & " renglon > '' and " _
                                        & " item > '' and " _
                                        & " estatus = '0' AND " _
                                        & " aceptado < '2' and " _
                                        & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                        & " id_emp = '" & jytsistema.WorkID & "' ")

        '2.- Elimina movimientos de inventarios anteriores de la Factura
        EliminarMovimientosdeInventario(myConn, NumeroFactura, "PVE", lblInfo)

        '3.- Actualizar Movimientos de Inventario con Factura
        For Each dtRow As DataRow In dtRenglones.Rows
            With dtRow
                Dim CostoActual As Double = UltimoCostoAFecha(myConn, .Item("item"), CDate(txtEmision.Text)) / Equivalencia(myConn, .Item("item"), .Item("unidad"))
                InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(txtEmision.Text), "SA", NumeroFactura,
                                                     .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("cantidad") * CostoActual,
                                                     .Item("cantidad") * CostoActual, "PVE", NumeroFactura, .Item("lote"), txtCliente.Text,
                                                     .Item("totren"), .Item("totrendes"), 0, .Item("totren") - .Item("totrendes"), txtAsesor.Text,
                                                     txtAlmacen.Text, .Item("renglon"), jytsistema.sFechadeTrabajo)

                ActualizarExistenciasPlus(myConn, .Item("item"))
            End With
        Next

        '4.- Actualizar existencias de renglones eliminados
        'For Each aSTR As Object In Eliminados
        '    ActualizarExistencias(myConn, aSTR, lblInfo)
        'Next

        '5.- Actualizar CxC si es una Factura a crédito

        '    ft.Ejecutar_strSQL ( myconn, " DELETE from jsventracob WHERE " _
        '                                    & " TIPOMOV = 'FC' AND  " _
        '                                    & " NUMMOV = '" & NumeroFactura & "' AND " _
        '                                    & " ORIGEN = 'FAC' AND " _
        '                                    & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
        '                                    & " ID_EMP = '" & jytsistema.WorkID & "'  ")

        '    InsertEditVENTASCXC(myConn, lblInfo, True, txtCliente.Text, "FC", NumeroFactura, CDate(txtEmision.Text), ft.FormatoHora(Now), _
        '                        FechaVencimiento, txtReferencia.Text, "Factura : " & NumeroFactura, ValorNumero(txtTotal.Text), _
        '                        ValorNumero(txtTotalIVA.Text), "", "", "", "", "", "FAC", NumeroFactura, "0", "", jytsistema.sFechadeTrabajo, _
        '                        txtCodigoContable.Text, "", "", 0.0, 0.0, "", "", "", "", txtAsesor.Text, txtAsesor.Text, "0", "0", "")


        'Else '6.- Actualizar Caja y Bancos si es una Factura a contado
        '    ' 6.1.- Elimina movimientos anteriores de caja y bancos
        '    ft.Ejecutar_strSQL ( myconn, "DELETE FROM jsbantracaj WHERE " _
        '                & " ORIGEN = 'FAC' AND " _
        '                & " TIPOMOV = 'EN' AND " _
        '                & " NUMMOV = '" & NumeroFactura & "' AND " _
        '                & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
        '                & " ID_EMP = '" & jytsistema.WorkID & "'")

        '    ft.Ejecutar_strSQL ( myconn, "DELETE FROM jsbantraban WHERE " _
        '                & " ORIGEN = 'FAC' AND " _
        '                & " TIPOMOV = 'DP' AND " _
        '                & " NUMORG = '" & NumeroFactura & "' AND " _
        '                & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
        '                & " ID_EMP = '" & jytsistema.WorkID & "' ")


        '    Dim hCont As Integer
        '    Dim dtFP As DataTable
        '    Dim nTablaFP As String = "tblFP"
        '    ds = DataSetRequery(ds, "select * from jsvenforpag where numfac = '" & NumeroFactura & "' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ", _
        '                         myConn, nTablaFP, lblInfo)
        '    dtFP = ds.Tables(nTablaFP)

        '    For hCont = 0 To dtFP.Rows.Count - 1
        '        With dtFP.Rows(hCont)
        '            Select Case .Item("formapag")
        '                Case "EF", "CH", "TA"

        '                    Dim cajaFP As String = "VENPARAM" & IIf(.Item("FORMAPAG") = "EF", "12", _
        '                        IIf(.Item("FORMAPAG") = "CH", "13", IIf(.Item("FORMAPAG") = "TA", "14", "11")))

        '                    Dim aCaja As String = ParametroPlus(MyConn, Gestion.iVentas, cajaFP)

        '                    InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, aCaja, UltimoCajaMasUno(myConn, lblInfo, aCaja), _
        '                                                CDate(.Item("VENCE").ToString), "FAC", "EN", NumeroFactura, .Item("formapag"), _
        '                                                .Item("numpag"), .Item("nompag"), .Item("importe"), "", _
        '                                                "FACTURA N° :" & NumeroFactura, "", jytsistema.sFechadeTrabajo, 1, "", 0, "", _
        '                                                jytsistema.sFechadeTrabajo, txtCliente.Text, txtAsesor.Text, "1")

        '                    CalculaSaldoCajaPorFP(myConn, aCaja, .Item("formapag"), lblInfo)
        '                Case "CT"

        '                    Dim CantidadXCorredor As Integer = ft.Ejecutar_strSQL_DevuelveScalar(myConn, lblInfo, _
        '                                                                               "select IFNULL(COUNT(*),0) AS CANTIDAD from jsventabtic WHERE " _
        '                                                                               & " ID_EMP = '" & jytsistema.WorkID & "' AND " _
        '                                                                               & " CORREDOR = '" & .Item("NOMPAG") & "' AND " _
        '                                                                               & " NUMCAN = '" & NumeroFactura & "' GROUP BY CORREDOR  ")

        '                    Dim aCaja As String = ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM11")


        '                    InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, aCaja, UltimoCajaMasUno(myConn, lblInfo, aCaja), _
        '                                        CDate(.Item("VENCE").ToString), "FAC", "EN", NumeroFactura, "CT", _
        '                                         .Item("numpag"), .Item("nompag"), .Item("importe"), "", _
        '                                        "FACTURA N° :" & NumeroFactura, "", jytsistema.sFechadeTrabajo, CantidadXCorredor, "", 0, "", _
        '                                        jytsistema.sFechadeTrabajo, txtCliente.Text, txtAsesor.Text, "1")

        '                    CalculaSaldoCajaPorFP(myConn, aCaja, "CT", lblInfo)

        '                Case "DP"

        '                    InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, CDate(.Item("VENCE").ToString), .Item("NUMPAG"), .Item("FORMAPAG"), _
        '                                                    .Item("NOMPAG"), "", "FACTURA N° :" & NumeroFactura, .Item("IMPORTE"), "FAC", NumeroFactura, _
        '                                                    "", NumeroFactura, "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, "FC", _
        '                                                    "", jytsistema.sFechadeTrabajo, "0", txtCliente.Text, txtAsesor.Text)

        '                    CalculaSaldoBanco(myConn, lblInfo, .Item("nompag"), True, jytsistema.sFechadeTrabajo)

        '            End Select
        '        End With
        '    Next

        '    dtFP.Dispose()
        '    dtFP = Nothing

        'End If

        '7.- Actualiza Cantidades en tránsito de los presupuestos y estatus del documento
        'ActualizarRenglonesEnPresupuestos(myConn, lblInfo, ds, dtRenglones, "jsvenrenpos")

        ''8.- Actualiza cantidades en tránsito de los prepedidos y estatus de documento
        'ActualizarRenglonesEnPrepedidos(myConn, lblInfo, ds, dtRenglones, "jsvenrenpos")

        ''9.- Actualiza cantidades en tránsito de los pedidos y estatus del documento
        'ActualizarRenglonesEnPedidos(myConn, lblInfo, ds, dtRenglones, "jsvenrenpos")

        ''10.- Actualiza cantidades en tránsito de las notas de entrega y estatus del documento
        'ActualizarRenglonesEnNotasDeEntrega(myConn, lblInfo, ds, dtRenglones, "jsvenrenpos")

    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique comentario ... ", Transportables.tipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        'i_modo = movimiento.iAgregar
        'nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        'ActivarMarco0()
        'IniciarDocumento(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        ActualizarMovimientos(txtCodigo.Text)
        ft.mensajeInformativo("Mercancías actualizadas...")

        'If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA01")) Then
        '    i_modo = movimiento.iEditar
        '    nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        '    ActivarMarco0()
        'Else
        '    ft.MensajeCritico( "Edición de Facturas NO está permitida...")
        'End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) >= 1 Then

            nPosicionEncab = Me.BindingContext(ds, nTabla).Position

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numfac"))

                For Each dtRow As DataRow In dtRenglones.Rows
                    With dtRow
                        Eliminados.Add(.Item("item"))
                    End With
                Next

                ft.Ejecutar_strSQL(myConn, " delete from jsvenencpos where numfac = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "'")
                ft.Ejecutar_strSQL(myConn, " delete from jsvenrenpos where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myConn, " delete from jsvenivapos where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myConn, " DELETE from jsvendespos where NUMFAC = '" & txtCodigo.Text & "' and ID_EMP = '" & jytsistema.WorkID & "' and EJERCICIO =  '" & jytsistema.WorkExercise & "' ")
                ft.Ejecutar_strSQL(myConn, " DELETE from jsventrapos where NUMMOV = '" & txtCodigo.Text & "' AND ORIGEN = 'PVE' and ID_EMP = '" & jytsistema.WorkID & "' and EJERCICIO =  '" & jytsistema.WorkExercise & "' ")
                ft.Ejecutar_strSQL(myConn, " DELETE FROM jsbantraban where NUMDOC = '" & txtCodigo.Text & "' AND TIPOMOV = 'DP' AND ORIGEN = 'FAC' AND NUMORG = '" & txtCodigo.Text & "' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myConn, " DELETE FROM jsbantracaj where NUMMOV = '" & txtCodigo.Text & "' AND ORIGEN = 'PVE' AND TIPOMOV = 'EN' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myConn, " delete from jsventracob where NUMMOV = '" & txtCodigo.Text & "' AND ORIGEN = 'PVE' AND TIPOMOV = 'FC' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myConn, " DELETE FROM jsventabtic WHERE NUMCAN = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myConn, " delete from jsvenrengui where codigofac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                EliminarMovimientosdeInventario(myConn, txtCodigo.Text, "PVE", lblInfo)

                For Each aSTR As Object In Eliminados
                    ActualizarExistenciasPlus(myConn, aSTR)
                Next

                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                dt = ds.Tables(nTabla)
                If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                AsignaTXT(nPosicionEncab)

            End If
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numfac", "nomcli", "numserial", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Nombre", "N° Serial Imp", "Emisión", "Comentario"}
        Dim Anchos() As Integer = {150, 400, 100, 100, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Facturas...")
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

        txtSubTotal.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrenpos", "numfac", "totren", txtCodigo.Text, 0))

        AbrirDescuentos(txtCodigo.Text)
        txtCargos.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrenpos", "numfac", "totren", txtCodigo.Text, 1))
        CalculaDescuentoEnRenglones()
        CalculaTotalIVAVentas(myConn, lblInfo, "jsvendespos", "jsvenivapos", "jsvenrenpos", "numfac", txtCodigo.Text, "impiva", "totrendes", CDate(txtEmision.Text), "totren",
                              NUMEROSERIAL, TIPODOCUMENTO)
        AbrirIVA(txtCodigo.Text)
        txtTotal.Text = ft.FormatoNumero(ValorNumero(txtSubTotal.Text) - ValorNumero(txtDescuentos.Text) + ValorNumero(txtCargos.Text) + ValorNumero(txtTotalIVA.Text))

        tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsvenrenpos", "peso", "numfac", txtCodigo.Text))

        ActualizaEncabezadoFactura()

    End Sub

    Private Sub CalculaTotalesPlus(NumeroFacturaPlus As String, NumeroSerialPlus As String, TipoDocumentoPlus As Integer, EmisionPlus As Date)

        Dim subTotalPlus As Double = CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrenpos", "numfac", "totren", NumeroFacturaPlus, 0)
        Dim DescuentosPlus As Double = ft.DevuelveScalarDoble(myConn, " select SUM(DESCUENTO) from jsvendespos where numfac = '" & NumeroFacturaPlus & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac ")
        Dim CargosPlus As Double = CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrenpos", "numfac", "totren", NumeroFacturaPlus, 1)

        CalculaDescuentoEnRenglonesPlus(NumeroFacturaPlus, subTotalPlus, DescuentosPlus)

        CalculaTotalIVAVentas(myConn, lblInfo, "jsvendespos", "jsvenivapos", "jsvenrenpos", "numfac", NumeroFacturaPlus, "impiva", "totrendes", EmisionPlus, "totren",
                              NumeroSerialPlus, TipoDocumentoPlus)

        Dim TotalIVAPlus As Double = ft.DevuelveScalarDoble(myConn, " select SUM(IMPIVA) from jsvenivapos where numfac = '" & NumeroFacturaPlus & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac ")

        Dim totalPlus As Double = subTotalPlus - DescuentosPlus + CargosPlus + TotalIVAPlus
        Dim totalPesoPlus As Double = CalculaPesoDocumento(myConn, lblInfo, "jsvenrenpos", "peso", "numfac", NumeroFacturaPlus)

        ft.Ejecutar_strSQL(myConn, " update jsvenencpos set tot_net = " & subTotalPlus & ", " _
                               & " descuen = " & DescuentosPlus & ", " _
                               & " cargos = " & CargosPlus & ", " _
                               & " imp_iva = " & TotalIVAPlus & ", " _
                               & " tot_fac = " & totalPlus & " " _
                               & " where " _
                               & " numfac = '" & NumeroFacturaPlus & "' and " _
                               & " id_emp = '" & jytsistema.WorkID & "' ")

    End Sub

    Private Sub ActualizaEncabezadoFactura()

        ft.Ejecutar_strSQL(myConn, " update jsvenencpos set tot_net = " & ValorNumero(txtSubTotal.Text) & ", " _
                               & " descuen = " & ValorNumero(txtDescuentos.Text) & ", " _
                               & " cargos = " & ValorNumero(txtCargos.Text) & ", " _
                               & " imp_iva = " & ValorNumero(txtTotalIVA.Text) & ", " _
                               & " tot_fac = " & ValorNumero(txtTotal.Text) & " " _
                               & " where " _
                               & " numfac = '" & txtCodigo.Text & "' and " _
                               & " id_emp = '" & jytsistema.WorkID & "' ")
    End Sub
    Private Sub CalculaDescuentoEnRenglonesPlus(NumeroFacturaPlus As String, SubtotalPlus As Double, DescuentosPlus As Double)

        ft.Ejecutar_strSQL(myConn, " update jsvenrenpos set totrendes = totren - totren * " & DescuentosPlus / IIf(SubtotalPlus > 0, SubtotalPlus, 1) & " " _
            & " where " _
            & " numfac = '" & NumeroFacturaPlus & "' and " _
            & " renglon > '' and " _
            & " item > '' and " _
            & " ESTATUS = '0' AND " _
            & " ACEPTADO < '2' and " _
            & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
            & " ID_EMP = '" & jytsistema.WorkID & "' ")

    End Sub

    Private Sub CalculaDescuentoEnRenglones()

        ft.Ejecutar_strSQL(myConn, " update jsvenrenpos set totrendes = totren - totren * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
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
            f.Agregar(myConn, ds, dtRenglones, "FAC", txtCodigo.Text, CDate(txtEmision.Text), txtAlmacen.Text, cmbTarifa.Text, txtCliente.Text, , , , , , , , , , txtAsesor.Text)
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
            f.Editar(myConn, ds, dtRenglones, "FAC", txtCodigo.Text, CDate(txtEmision.Text), txtAlmacen.Text, cmbTarifa.Text, txtCliente.Text,
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
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                With dtRenglones.Rows(nPosicionRenglon)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("item"))

                    Eliminados.Add(.Item("item"))

                    Dim aCamposDel() As String = {"numfac", "item", "renglon", "id_emp"}
                    Dim aStringsDel() As String = {txtCodigo.Text, .Item("item"), .Item("renglon"), jytsistema.WorkID}

                    ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and " _
                           & " origen = 'FAC' and item = '" & .Item("item") & "' and renglon = '" & .Item("renglon") & "' and " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsvenrenpos", strSQLMov, aCamposDel, aStringsDel,
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
        Imprimir()
    End Sub

    Private Sub btnAsesor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesor.Click
        If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA09")) Then
            txtAsesor.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, CONCAT( apellidos, ', ', nombres) descripcion from jsvencatven where tipo = '" & TipoVendedor.iFuerzaventa & "'  and estatus = 1 and id_emp = '" & jytsistema.WorkID & "'  order by 1 ", "Asesores Comerciales",
                                               txtAsesor.Text)
        Else
            ft.mensajeCritico("Escogencia de asesor comercial no permitida...")
        End If
    End Sub

    Private Sub txtCliente_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCliente.TextChanged
        If txtCliente.Text <> "" Then
            Dim aFld() As String = {"codcli", "id_emp"}
            Dim aStr() As String = {txtCliente.Text, jytsistema.WorkID}
            Dim FormaDePagoCliente As String = qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "formapago")

            'If txtCliente.Text = "00000000" Then
            ' txtNombreCliente.Text = dt.Rows(nPosicionEncab).Item("nomcli").ToString()
            'Else
            '   txtNombreCliente.Text = qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "nombre")
            'End If


            MostrarDisponibilidad(myConn, lblInfo, txtCliente.Text, qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "rif"),
                            ds, dgDisponibilidad)

            Disponibilidad = ft.DevuelveScalarDoble(myConn, " select disponible from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

            Dim mTotalFac As Double = ValorNumero(txtTotal.Text)
            If i_modo = movimiento.iAgregar Then
                mTotalFac = 0.0
                FechaVencimiento = DateAdd(DateInterval.Day, DiasCreditoAlVencimiento(myConn, FormaDePagoCliente), CDate(txtEmision.Text))
                CondicionDePago = CondicionPagoProveedorCliente(myConn, lblInfo, FormaDePagoCliente)
                If qFound(myConn, lblInfo, "jsvencatcli", aFld, aStr) Then
                    cmbTarifa.SelectedIndex = ft.InArray(aTarifa, qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "tarifa"))
                    txtAsesor.Text = qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "vendedor")
                    If qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "transporte") <> "" Then txtCaja.Text = qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "transporte")
                End If

            End If
            lblDisponibilidad.Text = "Disponible menos este Documento : " & ft.FormatoNumero(Disponibilidad + MontoAnterior - mTotalFac)

        End If
    End Sub

    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dgIVA.CellFormatting
        If e.ColumnIndex = 1 Then e.Value = ft.FormatoNumero(e.Value) & "%"
    End Sub

    Private Sub txtAsesor_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAsesor.TextChanged
        Dim aFld() As String = {"codven", "tipo", "id_emp"}
        Dim aStr() As String = {txtAsesor.Text, "1", jytsistema.WorkID}
        txtNombreAsesor.Text = qFoundAndSign(myConn, lblInfo, "jsvencatven", aFld, aStr, "apellidos") & ", " _
                & qFoundAndSign(myConn, lblInfo, "jsvencatven", aFld, aStr, "nombres")
    End Sub

    Private Sub btnAgregaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaDescuento.Click
        Dim f As New jsGenDescuentosVentas
        f.Agregar(myConn, ds, dtDescuentos, "jsvendespos", "numfac", txtCodigo.Text, sModulo, txtAsesor.Text, 0, CDate(txtEmision.Text), ValorNumero(txtSubTotal.Text))
        CalculaTotales()
        f = Nothing
    End Sub
    Private Sub btnEliminaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaDescuento.Click
        If nPosicionDescuento >= 0 Then
            With dtDescuentos.Rows(nPosicionDescuento)
                Dim aCamposDel() As String = {"numfac", "renglon", "id_emp"}
                Dim aStringsDel() As String = {txtCodigo.Text, .Item("renglon"), jytsistema.WorkID}
                nPosicionDescuento = EliminarRegistros(myConn, lblInfo, ds, nTablaDescuentos, "jsvendespos", strSQLDescuentos, aCamposDel, aStringsDel,
                                                      Me.BindingContext(ds, nTablaDescuentos).Position, True)
            End With
            CalculaTotales()
        End If
    End Sub

    Private Sub btnCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCliente.Click
        txtCliente.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcli codigo, nombre descripcion, disponible, elt(estatus+1, 'Activo', 'Bloqueado', 'Inactivo', 'Desincorporado') estatus from jsvencatcli where estatus < 3 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Clientes",
                                            txtCliente.Text)
    End Sub


    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If txtNombreCliente.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "FAC", txtCodigo.Text, CDate(txtEmision.Text), txtAlmacen.Text, cmbTarifa.Text, txtCliente.Text, True)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Private Sub txtNombreCliente_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNombreCliente.TextChanged
        If txtNombreCliente.Text = "" Then dgDisponibilidad.Columns.Clear()
    End Sub

    Private Sub txtTransporte_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCaja.TextChanged
        txtNombreTransporte.Text = ft.DevuelveScalarCadena(myConn, " select descrip from jsvencatcaj where codcaj = '" & txtCaja.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub txtAlmacen_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlmacen.TextChanged
        txtNombreAlmacen.Text = ft.DevuelveScalarCadena(myConn, " select desalm from jsmercatalm where codalm = '" & txtAlmacen.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub btnTransporte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransporte.Click
        txtCaja.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codtra codigo, nomtra descripcion from jsconctatra where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Transportes",
                                               txtCaja.Text)
    End Sub

    Private Sub btnAlmacen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacen.Click
        txtAlmacen.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codalm codigo, desalm descripcion from jsmercatalm where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Almacenes",
                                            txtAlmacen.Text)
    End Sub

    Private Sub btnCodigoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigoContable.Click
        txtCodigoContable.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables",
                                                   txtCodigoContable.Text)
    End Sub

    Private Sub dgDescuentos_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgDescuentos.RowHeaderMouseClick,
        dgDescuentos.CellMouseClick
        Me.BindingContext(ds, nTablaDescuentos).Position = e.RowIndex
        nPosicionDescuento = e.RowIndex
    End Sub

    Private Sub btnReconstruirIVA_Click(sender As System.Object, e As System.EventArgs) Handles btnReconstruirIVA.Click


        ft.Ejecutar_strSQL(myConn, " UPDATE jsvenencpos a, jsventracob b   " _
                       & " SET condpag = 0 " _
                       & " WHERE " _
                       & " a.condpag = 1 AND " _
                       & " a.numfac = b.nummov AND  " _
                       & " a.id_emp = b.id_emp AND " _
                       & " b.origen = 'PVE' AND " _
                       & " b.tipomov = 'FC' AND a.ID_EMP = '" & jytsistema.WorkID & "' ")

        If ft.DevuelveScalarCadena(myConn, " select usuario from jsconctausu where id_user = '" & jytsistema.sUsuario & "'") = "jytsuite" Then


            If ft.Pregunta("DESEA RECALCULAR IVA FACTURA", " ... ") = Windows.Forms.DialogResult.Yes Then CalculaTotales()

            If ft.Pregunta(" ¿ DESEA REPROCESAR FACTURAS DE ESTE MES ?", " ... ") = Windows.Forms.DialogResult.Yes Then

                Dim dtrecTabla As DataTable
                Dim nrecTabla As String = "tblrectabla"
                ds = DataSetRequery(ds, " select a.* from jsvenencpos a " _
                                            & " where " _
                                            & " a.emision >= '" & ft.FormatoFechaMySQL(PrimerDiaMes(CDate(txtEmision.Text))) & "' and " _
                                            & " a.emision <= '" & ft.FormatoFechaMySQL(UltimoDiaMes(CDate(txtEmision.Text))) & "' and " _
                                            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.emision ", myConn, nrecTabla, lblInfo)

                dtrecTabla = ds.Tables(nrecTabla)

                Dim sx As String = ft.FormatoFechaMySQL(PrimerDiaMes(CDate(txtEmision.Text)))

                barraProgreso.Visible = True
                lblProg.Visible = True
                Dim numRow As Long = 1
                For Each nRow As DataRow In dtrecTabla.Rows
                    With nRow

                        lblProg.Text = CInt(numRow / dtrecTabla.Rows.Count * 100).ToString + "%" ' numRow.ToString + "/" + dtrecTabla.Rows.Count.ToString
                        barraProgreso.Value = CInt(numRow / dtrecTabla.Rows.Count * 100)

                        CalculaTotalesPlus(.Item("numfac"), .Item("numserial"), .Item("tipo"), CDate(.Item("emision").ToString))
                        numRow += 1

                    End With
                Next
                barraProgreso.Visible = False
                lblProg.Visible = False
                ft.mensajeInformativo("Listo...")


                dtrecTabla.Dispose()
                dtrecTabla = Nothing

            End If

        End If

    End Sub
End Class