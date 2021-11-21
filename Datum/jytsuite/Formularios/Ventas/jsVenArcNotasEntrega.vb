Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Imports Syncfusion.WinForms.ListView.Enums

Public Class jsVenArcNotasEntrega
    Private Const sModulo As String = "Notas de Entrega"
    Private Const nTabla As String = "tblEncabNotasEntrega"
    Private Const lRegion As String = "RibbonButton84"
    Private Const nTablaRenglones As String = "tblRenglones_NotasEntrega"
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
    Private interchangeList As New List(Of CambioMonedaPlus)

    Private cliente As New Customer()
    Private asesor As New SalesForce()

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
    Private strSQL As String = " (select a.*, b.nombre from jsvenencnot a " _
            & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
            & " where " _
            & " a.emision >= '" & ft.FormatoFechaMySQL(DateAdd("m", -MesesAtras.i24, jytsistema.sFechadeTrabajo)) & "' and " _
            & " a.emision <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.numfac desc) order by numfac "
    Private Eliminados As New ArrayList

    Private Impresa As Integer

    Private Sub jsVenArcNotasEntrega_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsVenArcNotasEntrega_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
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
        Catch ex As MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub IniciarControles()
        '' Clientes
        InitiateDropDown(Of Customer)(myConn, cmbCliente)
        ''Asesores 
        InitiateDropDown(Of SalesForce)(myConn, cmbAsesores)

        '' Monedas
        'interchangeList = GetListaDeMonedasyCambios(myConn, jytsistema.sFechadeTrabajo)
        InitiateDropDownInterchangeCurrency(myConn, cmbMonedas, jytsistema.sFechadeTrabajo, True)

        DesactivarMarco0()
        Dim dates As SfDateTimeEdit() = {txtEmision}
        SetSizeDateObjects(dates)
        AsignarTooltips()
    End Sub
    Private Sub AsignarTooltips()
        Dim menus As New List(Of ToolStrip) From {MenuBarra, MenuBarraRenglon, MenuDescuentos}
        AsignarToolTipsMenuBarraToolStrip(menus, "Nota Débito")
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
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, cmbCliente)

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
                    txtCodigo.Text = ft.muestraCampoTexto(.Item("numfac"))
                    txtEmision.Value = .Item("emision")
                    txtVencimiento.Text = ft.FormatoFecha(CDate(.Item("vence").ToString))

                    Dim numControl As String = ft.DevuelveScalarCadena(myConn, " select num_control from jsconnumcon where numdoc = '" & .Item("numfac") & "' and  origen = 'FAC' and org = 'PFC' and id_emp = '" & jytsistema.WorkID & "' ")
                    txtControl.Text = IIf(numControl <> "0", numControl, "")

                    txtEstatus.Text = aEstatus(.Item("estatus"))
                    cmbCliente.SelectedValue = .Item("codcli")
                    txtComentario.Text = ft.muestraCampoTexto(.Item("comen"))
                    cmbAsesores.SelectedValue = .Item("codven")
                    txtTransporte.Text = ft.muestraCampoTexto(.Item("transporte"))
                    txtAlmacen.Text = ft.muestraCampoTexto(.Item("almacen"))
                    ft.RellenaCombo(aTarifa, cmbTarifa, ft.InArray(aTarifa, .Item("tarifa")))
                    txtReferencia.Text = ft.muestraCampoTexto(.Item("refer"))
                    txtCodigoContable.Text = ft.muestraCampoTexto(.Item("codcon"))

                    tslblPesoT.Text = ft.FormatoCantidad(.Item("kilos"))

                    txtSubTotal.Text = ft.FormatoNumero(.Item("tot_net"))
                    txtDescuentos.Text = ft.FormatoNumero(.Item("descuen"))
                    txtCargos.Text = ft.FormatoNumero(.Item("cargos"))
                    txtTotalIVA.Text = ft.FormatoNumero(.Item("imp_iva"))
                    txtTotal.Text = ft.FormatoNumero(.Item("tot_fac"))

                    txtCondicionPago.Text = "Condicion de pago : " & IIf(.Item("condpag") = 0, "CREDITO", "CONTADO")
                    MontoAnterior = .Item("tot_fac")
                    CondicionDePago = .Item("condpag")
                    FechaVencimiento = CDate(.Item("vence").ToString)

                    SetComboCurrency(.Item("Currency"), cmbMonedas, lblTotal)
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
        Dim aNombres() As String = {"Item", "Descripción", "IVA", "Cant.", "UND", "Precio Unitario", "Desc. Art.", "Desc. Cli.", "Desc. Ofe.", "Precio Total", "Tipo Renglon", ""}
        Dim aAnchos() As Integer = {70, 350, 30, 60, 45, 70, 50, 50, 50, 100, 70, 100}
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

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "NETMP" & ft.NumeroAleatorio(100000)
        Else
            txtCodigo.Text = ""
        End If

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtControl, txtComentario, txtTransporte,
                            txtAlmacen, txtReferencia, txtCodigoContable, txtVencimiento, txtCondicionPago)

        cmbCliente.SelectedIndex = -1
        cmbAsesores.SelectedIndex = -1

        Dim nTransporte As String = ft.DevuelveScalarCadena(myConn, "SELECT codtra FROM jsconctatra WHERE id_emp = '" & jytsistema.WorkID & "' ORDER BY codtra LIMIT 1")
        If nTransporte <> "0" Then txtTransporte.Text = nTransporte

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
        ft.habilitarObjetos(True, True, txtComentario, txtEmision, cmbCliente, cmbTarifa,
                         txtTransporte, btnTransporte, txtAlmacen, btnAlmacen,
                         txtReferencia, btnCodigoContable)

        ft.visualizarObjetos(True, grpDisponibilidad)
        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtEmision, txtControl, txtEstatus,
                cmbCliente, txtComentario, cmbAsesores, cmbMonedas,
                cmbTarifa, txtVencimiento, txtCondicionPago, txtTransporte, txtAlmacen, btnTransporte, btnAlmacen,
                txtNombreTransporte, txtNombreAlmacen, txtReferencia, txtCodigoContable, btnCodigoContable)

        If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENNOTPA09")) Then cmbAsesores.Enabled = True

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

        ft.Ejecutar_strSQL(myConn, " delete from jsvenrennot where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvenivanot where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvendesnot where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'PFC' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim f As New jsGenFormasPago
            f.CondicionPago = CondicionDePago
            f.TipoCredito = TipoCredito
            f.Vencimiento = FechaVencimiento
            f.Caja = Caja
            f.Cargar(myConn, "PFC", txtCodigo.Text, IIf(i_modo = movimiento.iAgregar, True, False), ValorNumero(txtTotal.Text), cmbMonedas.SelectedValue)
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

        If FechaUltimoBloqueo(myConn, "jsvenencnot") >= txtEmision.Value Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If cmbCliente.SelectedIndex < 0 Then
            ft.mensajeCritico("Debe indicar un cliente válido...")
            Return False
        End If

        If txtNombreTransporte.Text = "" Then
            ft.mensajeCritico("Debe indicar un transporte válido...")
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
        If Disponible < 0 Then
            ft.mensajeCritico("Esta nota de entrega excede la disponibilidad...")
            Return False
        End If

        If cliente.CodigoEstatus = 1 Or cliente.CodigoEstatus = 2 Then
            ft.mensajeCritico("Este Cliente posee estatus  " & cliente.Estatus & ". Favor remitir a Administración")
            Return False
        End If

        If dtRenglones.Rows.Count <= 0 Then
            ft.mensajeCritico("Debe introducir por lo menos un ítem")
            Return False
        End If

        If Not CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPARAM19")) Then
            If ClientePoseeChequesFuturos(myConn, lblInfo, cliente.Codcli) Then
                ft.mensajeCritico(" NO SE PUEDE REALIZAR ESTA OPERACION PUES ESTE CLIENTE POSEE CHEQUES A FUTURO ")
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
            ft.Ejecutar_strSQL(myConn, " update jsvenrennot set numfac = '" & Codigo & "' where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvenivanot set numfac = '" & Codigo & "' where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvendesnot set numfac = '" & Codigo & "' where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & txtCodigo.Text & "' and origen = 'PFC' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        InsertEditVENTASEncabezadoNotaEntrega(myConn, lblInfo, Inserta, Codigo, txtEmision.Value, cliente.Codcli,
                                               txtComentario.Text, asesor.Codigo, txtAlmacen.Text, txtTransporte.Text,
                                               CDate(txtVencimiento.Text), txtReferencia.Text, txtCodigoContable.Text,
                                               ValorEntero(dtRenglones.Rows.Count), 0.0, ValorCantidad(tslblPesoT.Text),
                                               ValorNumero(txtSubTotal.Text), 0.0, 0.0, 0.0, 0.0, 0.0, ValorNumero(txtDescuentos.Text),
                                               0.0, 0.0, 0.0, 0.0, ValorNumero(txtCargos.Text), 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
                                               jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo,
                                               CondicionDePago, TipoCredito, "", "", "", "", 0.0, "", "", 0.0, "", "", 0.0, "", "",
                                               0.0, "", "", 0.0, 0.0, "", 0, 0, 0.0, 0.0, "", jytsistema.sFechadeTrabajo, 0.0, 0.0,
                                               ValorNumero(txtTotalIVA.Text), 0.0, 0, 0, ValorNumero(txtTotal.Text), ft.InArray(aEstatus, txtEstatus.Text),
                                               cmbTarifa.Text, "", "", "", "", "", Impresa, cmbMonedas.SelectedValue, DateTime.Now())


        ActualizarMovimientos(Codigo)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub ActualizarMovimientos(ByVal NumeroNotaEntrega As String)

        '1.- Aplica Descuento Global sobre total Renglón con descuento "totrendes"
        ft.Ejecutar_strSQL(myConn, " update jsvenrennot set totrendes = totren - totren * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
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
                Dim CostoActual As Double = UltimoCostoAFecha(myConn, .Item("item"), txtEmision.Value) / Equivalencia(myConn, .Item("item"), .Item("unidad"))
                InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), txtEmision.Value, "SA", NumeroNotaEntrega,
                                                     .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("cantidad") * CostoActual,
                                                     .Item("cantidad") * CostoActual, "PFC", NumeroNotaEntrega, .Item("lote"), cliente.Codcli,
                                                     .Item("totren"), .Item("totrendes"), 0, .Item("totren") - .Item("totrendes"), asesor.Codigo,
                                                     txtAlmacen.Text, .Item("renglon"), jytsistema.sFechadeTrabajo)

                ActualizarExistenciasPlus(myConn, .Item("item"))
            End With
        Next

        '4.- Actualizar existencias de renglones eliminados
        For Each aSTR As Object In Eliminados
            ActualizarExistenciasPlus(myConn, aSTR)
        Next

        '5.- Actualizar CxC si es una entrega a crédito
        If CondicionDePago = CondicionPago.iCredito Then
            ft.Ejecutar_strSQL(myConn, " DELETE from jsventracob WHERE " _
                                            & " TIPOMOV = 'NE' AND  " _
                                            & " NUMMOV = '" & NumeroNotaEntrega & "' AND " _
                                            & " ORIGEN = 'PFC' AND " _
                                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                            & " ID_EMP = '" & jytsistema.WorkID & "'  ")

            InsertEditVENTASCXC(myConn, lblInfo, True, cliente.Codcli, "NE", NumeroNotaEntrega, txtEmision.Value, ft.FormatoHora(Now),
                                FechaVencimiento, txtReferencia.Text, "Nota Entrega : " & NumeroNotaEntrega, ValorNumero(txtTotal.Text),
                                ValorNumero(txtTotalIVA.Text), "", "", "", "", "", "PFC", NumeroNotaEntrega, "0", "", jytsistema.sFechadeTrabajo,
                                txtCodigoContable.Text, "", "", 0.0, 0.0, "", "", "", "", asesor.Codigo, asesor.Codigo, "0", "0", "",
                                jytsistema.WorkCurrency.Id, DateTime.Now())


        Else '6.- Actualizar Caja y Bancos si es una entrega a contado

            ft.Ejecutar_strSQL(myConn, "DELETE FROM jsbantracaj WHERE " _
                        & " ORIGEN = 'PFC' AND " _
                        & " TIPOMOV = 'EN' AND " _
                        & " NUMMOV = '" & NumeroNotaEntrega & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "'")

            ft.Ejecutar_strSQL(myConn, "DELETE FROM jsbantraban WHERE " _
                        & " ORIGEN = 'PFC' AND " _
                        & " TIPOMOV = 'DP' AND " _
                        & " NUMORG = '" & NumeroNotaEntrega & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' ")


            Dim hCont As Integer
            Dim dtFP As DataTable
            Dim nTablaFP As String = "tblFP"
            ds = DataSetRequery(ds, "select * from jsvenforpag where numfac = '" & NumeroNotaEntrega & "' and origen = 'PFC' and id_emp = '" & jytsistema.WorkID & "' ",
                                 myConn, nTablaFP, lblInfo)
            dtFP = ds.Tables(nTablaFP)

            For hCont = 0 To dtFP.Rows.Count - 1
                With dtFP.Rows(hCont)
                    Select Case .Item("formapag")
                        Case "EF", "CH", "TA"

                            Dim cajaFP As String = "VENPARAM" & IIf(.Item("FORMAPAG") = "EF", "12",
                                IIf(.Item("FORMAPAG") = "CH", "13", IIf(.Item("FORMAPAG") = "TA", "14", "11")))

                            Dim aCaja As String = ParametroPlus(myConn, Gestion.iVentas, cajaFP)

                            InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, aCaja, UltimoCajaMasUno(myConn, lblInfo, aCaja),
                                                        CDate(.Item("VENCE").ToString), "PFC", "EN", NumeroNotaEntrega, .Item("formapag"),
                                                        .Item("numpag"), .Item("nompag"), .Item("importe"), "",
                                                        "NOTA DE ENTREGA N° :" & NumeroNotaEntrega, "", jytsistema.sFechadeTrabajo, 1, "", 0, "",
                                                        jytsistema.sFechadeTrabajo, cliente.Codcli, asesor.Codigo, "1",
                                                        jytsistema.WorkCurrency.Id, DateTime.Now())

                            CalculaSaldoCajaPorFP(myConn, aCaja, .Item("formapag"), lblInfo)
                        Case "CT"

                            Dim CantidadXCorredor As Integer = ft.DevuelveScalarEntero(myConn,
                                                                                       "select count(*) from jsventabtic WHERE " _
                                                                                       & " ID_EMP = '" & jytsistema.WorkID & "' AND " _
                                                                                       & " CORREDOR = '" & .Item("NOMPAG") & "' AND " _
                                                                                       & " NUMCAN = '" & NumeroNotaEntrega & "' GROUP BY CORREDOR  ")

                            Dim aCaja As String = ParametroPlus(myConn, Gestion.iVentas, "VENPARAM11")


                            InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, aCaja, UltimoCajaMasUno(myConn, lblInfo, aCaja),
                                                CDate(.Item("VENCE").ToString), "PFC", "EN", NumeroNotaEntrega, "CT",
                                                 .Item("numpag"), .Item("nompag"), .Item("importe"), "",
                                                "NOTA DE ENTREGA N° :" & NumeroNotaEntrega, "", jytsistema.sFechadeTrabajo, CantidadXCorredor, "", 0, "",
                                                jytsistema.sFechadeTrabajo, cliente.Codcli, asesor.Codigo, "1", jytsistema.WorkCurrency.Id, DateTime.Now())

                            CalculaSaldoCajaPorFP(myConn, aCaja, "CT", lblInfo)

                        Case "DP"

                            InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, CDate(.Item("VENCE").ToString), .Item("NUMPAG"), .Item("FORMAPAG"),
                                                            .Item("NOMPAG"), "", "NOTA DE ENTREGA N° :" & NumeroNotaEntrega, .Item("IMPORTE"), "PFC", NumeroNotaEntrega,
                                                            "", NumeroNotaEntrega, "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, "NE",
                                                            "", jytsistema.sFechadeTrabajo, "0", cliente.Codcli, asesor.Codigo,
                                                            jytsistema.WorkCurrency.Id, DateTime.Now())

                            CalculaSaldoBanco(myConn, lblInfo, .Item("nompag"), True, jytsistema.sFechadeTrabajo)

                    End Select
                End With
            Next

            dtFP.Dispose()
            dtFP = Nothing

        End If

        '7.- Actualiza Cantidades en tránsito de los presupuestos y estatus del documento
        ActualizarRenglonesEnPresupuestos(myConn, lblInfo, ds, dtRenglones, "jsvenrennot")

        '8.- Actualiza cantidades en tránsito de los prepedidos y estatus de documento
        ActualizarRenglonesEnPrepedidos(myConn, lblInfo, ds, dtRenglones, "jsvenrennot")

        '9.- Actualiza cantidades en tránsito de los pedidos y estatus del documento
        ActualizarRenglonesEnPedidos(myConn, lblInfo, ds, dtRenglones, "jsvenrennot")


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
        Dim aCamposAdicionales() As String = {"numfac|'" & txtCodigo.Text & "'",
                                              "codcli|'" & cliente.Codcli & "'"}
        If DocumentoBloqueado(myConn, "jsvenencnot", aCamposAdicionales) Then
            ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
        Else
            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENNOTPA01")) Then
                i_modo = movimiento.iEditar
                nPosicionEncab = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
                ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, cmbCliente)
            Else
                ft.mensajeCritico("Edición de Notas de Entrega NO está permitida...")
            End If
        End If
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Dim aCamposAdicionales() As String = {"numfac|'" & txtCodigo.Text & "'",
                                             "codcli|'" & cliente.Codcli & "'"}
        If DocumentoBloqueado(myConn, "jsvenencnot", aCamposAdicionales) Then
            ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
        Else
            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENNOTPA01")) Then
                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                nPosicionEncab = Me.BindingContext(ds, nTabla).Position

                sRespuesta = MsgBox(" ¿ Está seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")

                If sRespuesta = MsgBoxResult.Yes Then

                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numfac"))

                    For Each dtRow As DataRow In dtRenglones.Rows
                        With dtRow
                            Eliminados.Add(.Item("item"))
                        End With
                    Next

                    ft.Ejecutar_strSQL(myConn, " delete from jsvenencnot where numfac = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "'")
                    ft.Ejecutar_strSQL(myConn, " DELETE from jsvendesnot where NUMFAC = '" & txtCodigo.Text & "' and ID_EMP = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " delete from jsvenrennot where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " delete from jsvenivanot where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'PFC' and id_emp = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " DELETE FROM jsbantraban where NUMDOC = '" & txtCodigo.Text & "' AND TIPOMOV = 'DP' AND ORIGEN = 'PFC' AND NUMORG = '" & txtCodigo.Text & "' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " DELETE FROM jsbantracaj where NUMMOV = '" & txtCodigo.Text & "' AND ORIGEN = 'PFC' AND TIPOMOV = 'EN' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " delete from jsventracob where NUMMOV = '" & txtCodigo.Text & "' AND ORIGEN = 'PFC' AND TIPOMOV = 'NE' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " DELETE FROM jsventabtic WHERE NUMCAN = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " delete from jsvenrengui where codigofac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")


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
                ft.mensajeCritico("Eliminación de Notas de Entrega NO está permitida...")
            End If
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numfac", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Nombre", "Emisión", "Comentario"}
        Dim Anchos() As Integer = {150, 400, 100, 150}
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

        txtSubTotal.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrennot", "numfac", "totren", txtCodigo.Text, 0))

        Dim discountTable As New SimpleTableProperties("jsvendesnot", "numfac", txtCodigo.Text)
        MostrarDescuentosAlbaran(myConn, discountTable, dgDescuentos, txtDescuentos)

        txtCargos.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrennot", "numfac", "totren", txtCodigo.Text, 1))
        CalculaDescuentoEnRenglones()
        CalculaTotalIVAVentas(myConn, lblInfo, "jsvendesnot", "jsvenivanot", "jsvenrennot", "numfac", txtCodigo.Text, "impiva", "totrendes",
                              txtEmision.Value, "totren")

        Dim ivaTable As New SimpleTableProperties("jsvenivanot", "numfac", txtCodigo.Text)
        MostrarIVAAlbaran(myConn, ivaTable, dgIVA, txtTotalIVA)

        txtTotal.Text = ft.FormatoNumero(ValorNumero(txtSubTotal.Text) - ValorNumero(txtDescuentos.Text) + ValorNumero(txtCargos.Text) + ValorNumero(txtTotalIVA.Text))

        tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsvenrennot", "peso", "numfac", txtCodigo.Text))

    End Sub

    Private Sub CalculaDescuentoEnRenglones()

        ft.Ejecutar_strSQL(myConn, " update jsvenrennot set totrendes = totren - totren * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
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
        If cmbCliente.SelectedIndex >= 0 Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "PFC", txtCodigo.Text, txtEmision.Value, txtAlmacen.Text, cmbTarifa.Text, cliente.Codcli, , , , , , , , , , asesor.Codigo)
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
                    f.Editar(myConn, ds, dtRenglones, "PFC", txtCodigo.Text, txtEmision.Value, txtAlmacen.Text, cmbTarifa.Text, cliente.Codcli,
                             IIf(.Item("item").ToString.Substring(0, 1) = "$", True, False), , , , , , , , , asesor.Codigo)
                    ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
                    AsignaMov(f.Apuntador, True)
                    CalculaTotales()
                    f = Nothing
                Else
                    ft.mensajeCritico("Renglón NO EDITABLE...")
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
            sRespuesta = MsgBox(" ¿ Está seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
            If sRespuesta = MsgBoxResult.Yes Then
                With dtRenglones.Rows(nPosicionRenglon)

                    If .Item("EDITABLE") = 0 Then
                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("item"))

                        Eliminados.Add(.Item("item"))

                        Dim aCamposDel() As String = {"numfac", "item", "renglon", "id_emp"}
                        Dim aStringsDel() As String = {txtCodigo.Text, .Item("item"), .Item("renglon"), jytsistema.WorkID}

                        ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and " _
                               & " origen = 'PFC' and item = '" & .Item("item") & "' and renglon = '" & .Item("renglon") & "' and " _
                               & " id_emp = '" & jytsistema.WorkID & "' ")

                        nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsvenrennot", strSQLMov, aCamposDel, aStringsDel,
                                                      Me.BindingContext(ds, nTablaRenglones).Position, True)

                        If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1
                        AsignaMov(nPosicionRenglon, True)
                        CalculaTotales()
                    Else
                        ft.mensajeCritico("Renglón NO EDITABLE...")

                    End If
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
    Private Sub Imprimir()

        Dim f As New jsVenRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cNotaDeEntrega, "Nota de Entrega",
                 cliente.Codcli, txtCodigo.Text, txtEmision.Value)
        f = Nothing

    End Sub



    Private Sub IncluirFleteAutomatico(Codigocliente As String, montoFlete As Double)

        If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA24")) Then

            Dim codServicioFlete As String = CStr(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA25"))
            Dim monServicioFlete As Double = montoFlete
            If monServicioFlete = 0 Then
                monServicioFlete = ft.DevuelveScalarDoble(myConn, " select IFNULL(precio,0) from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'")
            End If

            Dim desServicioFlete As String = ft.DevuelveScalarCadena(myConn, " select desser from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'")
            Dim ivaServicioFlete As String = ft.DevuelveScalarCadena(myConn, " select tipoiva from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'")

            Dim numRenglon As String = ft.autoCodigo(myConn, "renglon", "jsvenrennot", "numfac.id_emp", txtCodigo.Text + "." + jytsistema.WorkID, 5)

            If monServicioFlete <> 0 Then

                InsertEditVENTASRenglonNotasEntrega(myConn, lblInfo, True, txtCodigo.Text, numRenglon, "$" & codServicioFlete, desServicioFlete,
                                                    ivaServicioFlete, "", "UND", 0.0, 1.0,
                                                    0.0, 0.0, 0, "", "", 0.0, "", "1",
                                                    monServicioFlete, 0.0, 0.0,
                                                    0.0, monServicioFlete, monServicioFlete,
                                                    "", "", "", "", "", "",
                                                    "", "1", 1)

                ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
                AsignaMov(0, True)
                CalculaTotales()

            End If


        End If

    End Sub

    Private Sub btnAgregaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaDescuento.Click
        If cmbCliente.SelectedIndex >= 0 Then
            If ClienteFacturaAPartirDeCostos(myConn, lblInfo, cliente.Codcli) Then
                ft.mensajeCritico("DESCUENTOS NO PERMITIDOS")
            Else
                Dim f As New jsGenDescuentosVentas
                f.Agregar(myConn, ds, dtDescuentos, "jsvendesnot", "numfac", txtCodigo.Text, sModulo,
                         asesor.Codigo, 0, txtEmision.Value, ValorNumero(txtSubTotal.Text))
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
                nPosicionDescuento = EliminarRegistros(myConn, lblInfo, ds, nTablaDescuentos, "jsvendesnot", strSQLDescuentos, aCamposDel, aStringsDel,
                                                      Me.BindingContext(ds, nTablaDescuentos).Position, True)
            End With
            CalculaTotales()
        End If
    End Sub

    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If cmbCliente.SelectedIndex >= 0 Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "PFC", txtCodigo.Text, txtEmision.Value,
                      txtAlmacen.Text, cmbTarifa.Text, cliente.Codcli, True)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Private Sub txtTransporte_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTransporte.TextChanged
        txtNombreTransporte.Text = ft.DevuelveScalarCadena(myConn, " select nomtra from jsconctatra where codtra = '" & txtTransporte.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub txtAlmacen_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlmacen.TextChanged
        txtNombreAlmacen.Text = ft.DevuelveScalarCadena(myConn, " select desalm from jsmercatalm where codalm = '" & txtAlmacen.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub btnTransporte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransporte.Click
        txtTransporte.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codtra codigo, nomtra descripcion from jsconctatra where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Transportes",
                                               txtTransporte.Text)
    End Sub

    Private Sub btnAlmacen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacen.Click
        txtAlmacen.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codalm codigo, desalm descripcion from jsmercatalm where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Almacenes",
                                            txtAlmacen.Text)
    End Sub

    Private Sub btnCodigoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigoContable.Click
        txtCodigoContable.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables", txtCodigoContable.Text)

    End Sub
    Private Sub dgDescuentos_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs)
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
                    ft.Ejecutar_strSQL(myConn, " update jsvenrennot " _
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
    Private Sub cmbCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCliente.SelectedIndexChanged
        cliente = cmbCliente.SelectedItem
        If cmbCliente.SelectedIndex >= 0 Then
            MostrarDisponibilidad(myConn, lblInfo, cliente.Codcli, cliente.Rif, ds, dgDisponibilidad)

            Disponibilidad = cliente.Disponible
            Dim mTotalFac As Double = ValorNumero(txtTotal.Text)
            If i_modo = movimiento.iAgregar Then
                mTotalFac = 0.0
                FechaVencimiento = FechaVencimientoFactura(myConn, cliente.Codcli, txtEmision.Value)
                CondicionDePago = CondicionPagoProveedorCliente(myConn, lblInfo, cliente.FormaPago)

                cmbTarifa.SelectedIndex = ft.InArray(aTarifa, cliente.Tarifa)
                cmbAsesores.SelectedValue = cliente.Asesor
                txtTransporte.Text = cliente.Transporte
                IncluirFleteAutomatico(cliente.Codcli, montoFletesPorRegion(myConn, cliente.Codcli))
            End If

            lblDisponibilidad.Text = "Disponible menos este Documento : " & ft.FormatoNumero(Disponibilidad + MontoAnterior - mTotalFac)

        End If
    End Sub
    Private Sub cmbAsesores_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAsesores.SelectedIndexChanged
        asesor = cmbAsesores.SelectedItem
    End Sub

End Class