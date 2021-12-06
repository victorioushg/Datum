Imports MySql.Data.MySqlClient
Imports Microsoft.Win32
Imports Syncfusion.WinForms.Input
Imports Syncfusion.WinForms.ListView.Enums

Public Class jsVenArcFacturas
    Private Const sModulo As String = "Facturas"
    Private Const lRegion As String = "RibbonButton85"
    Private Const nTabla As String = "tblEncabFacturas"
    Private Const nTablaRenglones As String = "tblRenglones_Facturas"
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
    Private accountList As New List(Of AccountBase)
    Private cliente As New Customer()
    Private asesor As New SalesForce()
    Private moneda As New CambioMonedaPlus()
    Private transporte As New SimpleTable()
    Private almacen As New SimpleTable()

    Private i_modo As Integer

    Private nPosicionEncab As Long, nPosicionRenglon As Long, nPosicionDescuento As Long

    Private aEstatus() As String = {"Confirmar", "Procesada", "Anulada"}
    Private aTarifa() As String = {"A", "B", "C", "D", "E", "F"}
    Private aImpresa() As String = {"", "I"}

    Private CondicionDePago As Integer = 0
    Private TipoCredito As Integer = 0
    Private Caja As String = "00"

    Private FechaVencimiento As Date = jytsistema.sFechadeTrabajo
    Private MontoAnterior As Double = 0.0
    Private Disponibilidad As Double = 0.0
    Private TarifaCliente As String = "A"
    Private strSQL As String = " (select a.*, b.nombre from jsvenencfac a " _
            & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
            & " where " _
            & " a.emision >= '" & ft.FormatoFechaMySQL(DateAdd("m", -MesesAtras.i12, jytsistema.sFechadeTrabajo)) & "' and " _
            & " a.emision <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.numfac desc) order by numfac "

    Private Eliminados As New ArrayList

    Private Impresa As Integer

    Private Sub jsVenArcFacturas_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsVenArcFacturas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)


            IniciarControles()

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

            Dim dates As SfDateTimeEdit() = {txtEmision}
            SetSizeDateObjects(dates)

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub IniciarControles()

        '' Clientes
        InitiateDropDown(Of Customer)(myConn, cmbCliente)
        ''Asesores 
        InitiateDropDown(Of SalesForce)(myConn, cmbAsesores)

        '' Monedas
        interchangeList = GetListaDeMonedasyCambios(myConn, jytsistema.sFechadeTrabajo)
        InitiateDropDownInterchangeCurrency(myConn, cmbMonedas, jytsistema.sFechadeTrabajo)

        accountList = InitiateDropDown(Of AccountBase)(myConn, cmbCC)

        '' Transportes
        InitiateDropDown(Of SimpleTable)(myConn, cmbTransportes, Tipo.Transportes)
        '' Alamacenes 
        InitiateDropDown(Of SimpleTable)(myConn, cmbAlmacenes, Tipo.Almacenes)


    End Sub
    Private Sub AsignarTooltips()
        Dim menus As New List(Of ToolStrip) From {MenuBarra, MenuBarraRenglon, MenuDescuentos}
        AsignarToolTipsMenuBarraToolStrip(menus, "Factura")

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

                    Dim numControl As String = ft.DevuelveScalarCadena(myConn, " select num_control from jsconnumcon where numdoc = '" & .Item("numfac") & "' and  origen = 'FAC' and org = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ")
                    txtControl.Text = IIf(numControl <> "0", numControl, "")

                    txtEstatus.Text = aEstatus(.Item("estatus"))
                    txtImpresion.Text = aImpresa(.Item("impresa"))
                    cmbCliente.SelectedValue = .Item("codcli")
                    txtComentario.Text = ft.muestraCampoTexto(.Item("comen"))
                    cmbAsesores.SelectedValue = .Item("codven")
                    cmbTransportes.SelectedValue = .Item("transporte")
                    cmbAlmacenes.SelectedValue = .Item("almacen")
                    ft.RellenaCombo(aTarifa, cmbTarifa, ft.InArray(aTarifa, .Item("tarifa").ToString))
                    txtReferencia.Text = ft.muestraCampoTexto(.Item("refer"))
                    cmbCC.SelectedValue = .Item("codcon")

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

                    Impresa = .Item("impresa")

                    SetComboCurrency(.Item("Currency"), cmbMonedas, lblTotal)

                    'Renglones
                    AsignarMovimientos(.Item("numfac"))

                    'Totales
                    CalculaTotales()

                End With
            End With
        Else
            IniciarDocumento(False)
        End If

    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroFactura As String)

        strSQLMov = "select * from jsvenrenfac " _
                            & " where " _
                            & " numfac  = '" & NumeroFactura & "' and " _
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
                                   "numcot.N° Pre-Pedido.100.I.",
                                   "numped.Pedido N°.90.I.",
                                   "sada..100.I."}

        ft.IniciarTablaPlus(dg, dtRenglones, aCampos)

        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "FCTMP" & ft.NumeroAleatorio(100000)
        Else
            txtCodigo.Text = ""
        End If

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtControl, txtComentario, txtReferencia, txtVencimiento, txtCondicionPago, txtImpresion)
        cmbCliente.SelectedIndex = -1
        cmbAsesores.SelectedIndex = -1
        cmbCC.SelectedIndex = -1
        cmbTransportes.SelectedIndex = 0
        cmbAlmacenes.SelectedIndex = 0

        ft.RellenaCombo(aTarifa, cmbTarifa)
        txtEmision.Value = jytsistema.sFechadeTrabajo

        txtEstatus.Text = aEstatus(0)
        tslblPesoT.Text = ft.FormatoCantidad(0)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtSubTotal, txtDescuentos, txtCargos, txtTotalIVA, txtTotalIVA)
        txtVencimiento.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        MontoAnterior = 0.0
        CondicionDePago = 1

        dgDisponibilidad.Columns.Clear()
        lblDisponibilidad.Text = "Disponible menos este Documento : 0.00"
        txtTotal.Text = "0.00"

        SetComboCurrency(0, cmbMonedas, lblTotal)
        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(txtCodigo.Text)
        CalculaTotales()

    End Sub
    Private Sub IncluirFleteAutomatico(Codigocliente As String, montoFlete As Double)

        If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA24")) Then

            Dim codServicioFlete As String = CStr(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA25"))
            Dim monServicioFlete As Double = montoFlete
            If monServicioFlete = 0 Then
                monServicioFlete = ft.DevuelveScalarDoble(myConn, " select precio from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'")
            End If

            Dim desServicioFlete As String = ft.DevuelveScalarCadena(myConn, " select desser from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'")
            Dim ivaServicioFlete As String = ft.DevuelveScalarCadena(myConn, " select tipoiva from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'")

            Dim numRenglon As String = ft.autoCodigo(myConn, "renglon", "jsvenrenfac", "numfac.id_emp", txtCodigo.Text + "." + jytsistema.WorkID, 5)

            If monServicioFlete <> 0 Then

                InsertEditVENTASRenglonFactura(myConn, lblInfo, True, txtCodigo.Text, numRenglon, "$" & codServicioFlete, desServicioFlete,
                                                    ivaServicioFlete, "", "UND", 0.0, 1.0,
                                                    0.0, 0.0, 0, "", "", 0.0, "", "1",
                                                    monServicioFlete, 0.0, 0.0,
                                                    0.0, monServicioFlete, monServicioFlete,
                                                    "", "", "", "", "", "",
                                                    "", "", "", "1", 1)

                ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
                AsignaMov(0, True)
                CalculaTotales()

            End If


        End If

    End Sub

    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon, MenuDescuentos)
        ft.habilitarObjetos(True, True, txtComentario, txtEmision, cmbCliente, cmbTarifa, cmbTransportes, cmbAlmacenes,
                         txtReferencia, cmbCC)
        If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA09")) Then cmbAsesores.Enabled = True
        ft.visualizarObjetos(True, grpDisponibilidad)
        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)


    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtEmision, txtControl, txtEstatus,
                cmbCliente, txtComentario, cmbAsesores, cmbMonedas,
                cmbTarifa, txtVencimiento, txtCondicionPago, cmbTransportes, cmbAlmacenes,
                txtReferencia, cmbCC, txtImpresion)

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

        ft.Ejecutar_strSQL(myConn, " delete from jsvenrenfac where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvenivafac where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvendesfac where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            jytsistema.WorkBox = Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveImpresorFiscal, "")

            Dim f As New jsGenFormasPago

            f.CondicionPago = CondicionDePago
            f.TipoCredito = TipoCredito
            f.Vencimiento = FechaVencimiento
            f.Caja = Caja

            Dim permiteSeleccionarCrCo As Boolean = CBool(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA21"))

            If Not permiteSeleccionarCrCo Then f.CondicionPago = CondicionPago.iCredito
            f.Cargar(myConn, "FAC", txtCodigo.Text, IIf(permiteSeleccionarCrCo, IIf(i_modo = movimiento.iAgregar, True, False), False), ValorNumero(txtTotal.Text), cmbMonedas.SelectedValue)
            'INICIO DECRETO 3085 
            'CAMBIA TASA DE IVA DE LA FACTURA
            ' ActualizarRenglonesDocumento_NuevoIVA(myConn, "jsvenivafac", "jsvenrenfac", "numfac", txtCodigo.Text)
            'FIN DECRETO 3085

            CalculaTotales()

            If f.DialogResult = Windows.Forms.DialogResult.OK Then
                CondicionDePago = f.CondicionPago
                TipoCredito = f.TipoCredito
                FechaVencimiento = f.Vencimiento
                Caja = f.Caja
                GuardarTXT()
                Imprimir()
            End If

        End If
    End Sub
    Private Sub Imprimir()

        If DocumentoImpreso(myConn, lblInfo, "jsvenencfac", "numfac", txtCodigo.Text) Then
            Dim f As New jsVenRepParametros
            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cFactura, "Factura", cliente.Codcli, txtCodigo.Text, txtEmision.Value)
            f = Nothing
        Else
            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA08")) Then
                '1. Imprimir Nota de Entrega Fiscal
                jytsistema.WorkBox = Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveImpresorFiscal, "")
                If jytsistema.WorkBox = "" Then
                    ft.mensajeCritico("DEBE INDICAR UNA FORMA DE IMPRESION FISCAL")
                Else
                    '2. Colocar Nota de Entrega como impresa
                    Dim nTipoImpreFiscal As Integer = TipoImpresoraFiscal(myConn, jytsistema.WorkBox)
                    Select Case nTipoImpreFiscal
                        Case 0 ' FACTURA FISCAL FORMA LIBRE
                            ' SE DIRIGE A LA IMPRESORA POR DEFECTO
                            ImprimirFacturaGrafica(myConn, lblInfo, ds, txtCodigo.Text)
                        Case 1 'FACTURA FISCAL PRE-IMPRESA
                            Select Case jytsistema.WorkDataBase
                                Case "jytsuitebuhito"
                                    ImprimirFacturaGraficaBUHITO(myConn, lblInfo, ds, txtCodigo.Text)
                            End Select
                        Case 2, 5, 6,
                            7  'IMPRESORA FISCAL TIPO ACLAS/BIXOLON
                            ImprimirFacturaFiscalPP1F3(myConn, lblInfo, txtCodigo.Text, NumeroSERIALImpresoraFISCAL(myConn, lblInfo, jytsistema.WorkBox),
                                cliente.Nombre, cliente.Codcli, cliente.Rif, cliente.Dirfiscal,
                                txtEmision.Value, ft.DevuelveScalarCadena(myConn, "Select CONDPAG from jsvenencfac where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "'"),
                                CDate(txtVencimiento.Text), asesor.Codigo, asesor.NombreAsesor, nTipoImpreFiscal)

                            Dim ultFACFiscal As String = UltimaFACTURAImpresoraFiscal(myConn, lblInfo, jytsistema.WorkBox)
                            ft.Ejecutar_strSQL(myConn, " update jsvenencfac set refer = '" & ultFACFiscal & "' where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                        Case 3 'IMPRESORA FISCAL TIPO BEMATECH
                        Case 4 'IMPRESORA FISCAL TIPO EPSON/PNP
                            ImprimirFacturaFiscalPnP(myConn, lblInfo, txtCodigo.Text, cliente.Nombre, cliente.Codcli,
                                cliente.Rif, cliente.Dirfiscal,
                                txtEmision.Value, ft.DevuelveScalarCadena(myConn, "Select CONDPAG from jsvenencfac where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "'"),
                                CDate(txtVencimiento.Text), asesor.Codigo, asesor.NombreAsesor)
                    End Select
                End If

                ft.mensajeInformativo("SE HA ENVIADO FACTURA A LA IMPRESORA FISCAL...")

            Else
                ft.mensajeCritico("Impresión de FACTURA no permitida...")
            End If
        End If

    End Sub
    Private Function Validado() As Boolean

        If FechaUltimoBloqueo(myConn, "jsvenencgui") >= txtEmision.Value Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If cmbCliente.SelectedItem Is Nothing Then
            ft.mensajeCritico("Debe indicar un cliente válido...")
            Return False
        End If

        If cmbAsesores.SelectedItem Is Nothing Then
            ft.mensajeCritico("Debe indicar un nombre de Asesor válido...")
            Return False
        End If

        If cmbTransportes.SelectedItem Is Nothing Then
            ft.mensajeCritico("Debe indicar un transporte válido...")
            Return False
        End If

        If cmbAlmacenes.SelectedItem Is Nothing Then
            ft.mensajeCritico("Debe indicar un almacén válido...")
            Return False
        End If


        If dtRenglones.Rows.Count = 0 Then
            ft.mensajeCritico("Debe incluir al menos un ítem...")
            Return False
        End If

        Dim Disponible As Double = CDbl(Trim(Split(lblDisponibilidad.Text, ":")(1)))

        If Disponible < 0 Then
            ft.mensajeCritico("Esta Factura excede la disponibilidad...")
            Return False
        End If

        If Not CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPARAM37")) Then 'FACTURAR SI POSEE ESTATUS > 0
            If cliente.CodigoEstatus = 1 Or cliente.CodigoEstatus = 2 Then
                ft.mensajeCritico("Este Cliente posee estatus  " & cliente.Estatus & ". Favor remitir a Administración")
                Return False
            End If
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
            Codigo = Contador(myConn, lblInfo, Gestion.iVentas, "VENNUMFAC", "08")

            ft.Ejecutar_strSQL(myConn, " update jsvenrenfac set numfac = '" & Codigo & "' where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvenivafac set numfac = '" & Codigo & "' where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvendesfac set numfac = '" & Codigo & "' where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & txtCodigo.Text & "' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myConn, " update jsvenforpag set numfac = '" & Codigo & "' where numfac = '" & txtCodigo.Text & "' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        InsertEditVENTASEncabezadoFactura(myConn, lblInfo, Inserta, Codigo, txtEmision.Value, cliente.Codcli,
                                               txtComentario.Text, asesor.Codigo, almacen.Codigo, transporte.Codigo,
                                               CDate(txtVencimiento.Text), txtReferencia.Text, cmbCC.SelectedValue,
                                               ValorEntero(dtRenglones.Rows.Count), 0.0, ValorCantidad(tslblPesoT.Text),
                                               ValorNumero(txtSubTotal.Text), 0.0, 0.0, 0.0, 0.0, 0.0, ValorNumero(txtDescuentos.Text),
                                               0.0, 0.0, 0.0, 0.0, ValorNumero(txtCargos.Text), 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0,
                                               jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo,
                                               CondicionDePago, TipoCredito, "", "", "", "", 0.0, "", "", 0.0, "", "", 0.0, "", "",
                                               0.0, "", "", 0.0, 0.0, "", 0, 0, 0.0, 0.0, "", jytsistema.sFechadeTrabajo, 0.0, 0.0,
                                               ValorNumero(txtTotalIVA.Text), 0.0, 0, 1, ValorNumero(txtTotal.Text),
                                               IIf(i_modo = movimiento.iAgregar, 1, ft.InArray(aEstatus, txtEstatus.Text)),
                                               cmbTarifa.Text, "", "", "", "", "", Impresa,
                                               cmbMonedas.SelectedValue, DateTime.Now())


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

        '1.- Aplica Descuento Global sobre total Renglón con descuento "totrendes"
        ft.Ejecutar_strSQL(myConn, " update jsvenrenfac set totrendes = totren - totren * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
                                        & " where " _
                                        & " numfac = '" & NumeroFactura & "' and " _
                                        & " renglon > '' and " _
                                        & " item > '' and " _
                                        & " estatus = '0' AND " _
                                        & " aceptado < '2' and " _
                                        & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                        & " id_emp = '" & jytsistema.WorkID & "' ")

        '2.- Elimina movimientos de inventarios anteriores de la Factura
        EliminarMovimientosdeInventario(myConn, NumeroFactura, "FAC", lblInfo)

        '3.- Actualizar Movimientos de Inventario con Factura
        strSQLMov = "select * from jsvenrenfac " _
                            & " where " _
                            & " numfac  = '" & NumeroFactura & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        For Each dtRow As DataRow In dtRenglones.Rows
            With dtRow
                Dim CostoActual As Double = UltimoCostoAFecha(myConn, .Item("item"), txtEmision.Value) / Equivalencia(myConn, .Item("item"), .Item("unidad"))

                InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), txtEmision.Value, "SA", NumeroFactura,
                                                     .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("cantidad") * CostoActual,
                                                     .Item("cantidad") * CostoActual, "FAC", NumeroFactura, .Item("lote"), cliente.Codcli,
                                                     .Item("totren"), .Item("totrendes"), 0, .Item("totren") - .Item("totrendes"), asesor.Codigo,
                                                     almacen.Codigo, .Item("renglon"), jytsistema.sFechadeTrabajo)

                If .Item("NUMNOT") <> "" Then EliminarMovimientosdeInventario(myConn, .Item("NUMNOT"), "PDF", lblInfo, , , cliente.Codcli, .Item("ITEM"), .Item("RENNOT"))

                ActualizarExistenciasPlus(myConn, .Item("item"))

            End With
        Next

        '4.- Actualizar existencias de renglones eliminados
        For Each aSTR As Object In Eliminados
            ActualizarExistenciasPlus(myConn, aSTR)
        Next

        '5.- Actualizar CxC si es una Factura a crédito
        If CondicionDePago = CondicionPago.iCredito Then
            ft.Ejecutar_strSQL(myConn, " DELETE from jsventracob WHERE " _
                                            & " TIPOMOV = 'FC' AND  " _
                                            & " NUMMOV = '" & NumeroFactura & "' AND " _
                                            & " ORIGEN = 'FAC' AND " _
                                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                            & " ID_EMP = '" & jytsistema.WorkID & "'  ")

            InsertEditVENTASCXC(myConn, lblInfo, True, cliente.Codcli, "FC", NumeroFactura, txtEmision.Value, ft.FormatoHora(Now),
                                FechaVencimiento, txtReferencia.Text, "Factura : " & NumeroFactura, ValorNumero(txtTotal.Text),
                                ValorNumero(txtTotalIVA.Text), "", "", "", "", "", "FAC", NumeroFactura, "0", "", jytsistema.sFechadeTrabajo,
                                cmbCC.SelectedValue, "", "", 0.0, 0.0, "", "", "", "", asesor.Codigo, asesor.Codigo, "0", "0", "",
                                moneda.Moneda, DateTime.Now())


        Else '6.- Actualizar Caja y Bancos si es una Factura a contado
            ' 6.1.- Elimina movimientos anteriores de caja y bancos
            ft.Ejecutar_strSQL(myConn, "DELETE FROM jsbantracaj WHERE " _
                        & " ORIGEN = 'FAC' AND " _
                        & " TIPOMOV = 'EN' AND " _
                        & " NUMMOV = '" & NumeroFactura & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "'")

            ft.Ejecutar_strSQL(myConn, "DELETE FROM jsbantraban WHERE " _
                        & " ORIGEN = 'FAC' AND " _
                        & " TIPOMOV = 'DP' AND " _
                        & " NUMORG = '" & NumeroFactura & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' ")


            Dim hCont As Integer
            Dim dtFP As DataTable
            Dim nTablaFP As String = "tblFP"
            ds = DataSetRequery(ds, "select * from jsvenforpag where numfac = '" & NumeroFactura & "' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ",
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
                                                        CDate(.Item("VENCE").ToString), "FAC", "EN", NumeroFactura, .Item("formapag"),
                                                        .Item("numpag"), .Item("nompag"), .Item("importe"), "",
                                                        "FACTURA N° :" & NumeroFactura, "", jytsistema.sFechadeTrabajo, 1, "", 0, "",
                                                        jytsistema.sFechadeTrabajo, cliente.Codcli, asesor.Codigo, "1",
                                                        .Item("Currency"), .Item("Currency_Date"))

                            CalculaSaldoCajaPorFP(myConn, aCaja, .Item("formapag"), lblInfo)

                        Case "CT"

                            Dim CantidadXCorredor As Integer = ft.DevuelveScalarEntero(myConn,
                                                                                       "select count(*) from jsventabtic WHERE " _
                                                                                       & " ID_EMP = '" & jytsistema.WorkID & "' AND " _
                                                                                       & " CORREDOR = '" & .Item("NOMPAG") & "' AND " _
                                                                                       & " NUMCAN = '" & NumeroFactura & "' GROUP BY CORREDOR  ")

                            Dim aCaja As String = ParametroPlus(myConn, Gestion.iVentas, "VENPARAM11")


                            InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, aCaja, UltimoCajaMasUno(myConn, lblInfo, aCaja),
                                                CDate(.Item("VENCE").ToString), "FAC", "EN", NumeroFactura, "CT",
                                                 .Item("numpag"), .Item("nompag"), .Item("importe"), "",
                                                "FACTURA N° :" & NumeroFactura, "", jytsistema.sFechadeTrabajo, CantidadXCorredor, "", 0, "",
                                                jytsistema.sFechadeTrabajo, cliente.Codcli, asesor.Codigo, "1",
                                                .Item("Currency"), .Item("Currency_Date"))

                            CalculaSaldoCajaPorFP(myConn, aCaja, "CT", lblInfo)

                        Case "DP"

                            InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, CDate(.Item("VENCE").ToString), .Item("NUMPAG"), .Item("FORMAPAG"),
                                                            .Item("NOMPAG"), "", "FACTURA N° :" & NumeroFactura, .Item("IMPORTE"), "FAC", NumeroFactura,
                                                            "", NumeroFactura, "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, "FC",
                                                            "", jytsistema.sFechadeTrabajo, "0", cliente.Codcli, asesor.Codigo,
                                                            .Item("Currency"), .Item("Currency_Date"))

                            CalculaSaldoBanco(myConn, lblInfo, .Item("nompag"), True, jytsistema.sFechadeTrabajo)

                    End Select
                End With
            Next

            dtFP.Dispose()
            dtFP = Nothing

        End If

        '7.- Actualiza Cantidades en tránsito de los presupuestos y estatus del documento
        ActualizarRenglonesEnPresupuestos(myConn, lblInfo, ds, dtRenglones, "jsvenrenfac")

        '8.- Actualiza cantidades en tránsito de los prepedidos y estatus de documento
        ActualizarRenglonesEnPrepedidos(myConn, lblInfo, ds, dtRenglones, "jsvenrenfac")

        '9.- Actualiza cantidades en tránsito de los pedidos y estatus del documento
        ActualizarRenglonesEnPedidos(myConn, lblInfo, ds, dtRenglones, "jsvenrenfac")

        '10.- Actualiza cantidades en tránsito de las notas de entrega y estatus del documento
        ActualizarRenglonesEnNotasDeEntrega(myConn, lblInfo, ds, dtRenglones, "jsvenrenfac")

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
        If DocumentoBloqueado(myConn, "jsvenencfac", aCamposAdicionales) Then
            ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
        Else
            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA01")) Then
                If dt.Rows(nPosicionEncab).Item("RELGUIA") = "" Then
                    If dt.Rows(nPosicionEncab).Item("RELFACTURAS") = "" Then
                        If DocumentoPoseeCancelacionesAbonosDepositados(myConn, lblInfo, txtCodigo.Text) Then
                            ft.mensajeCritico("DOCUMENTO POSEE CANCELACIONES Y/O ABONOS DEPOSITADOS. VERIFIQUE POR FAVOR ...")
                        Else
                            If DocumentoPoseeCancelacionesAbonos(myConn, lblInfo, txtCodigo.Text) Then
                                ft.mensajeCritico("DOCUMENTO POSEE CANCELACIONES Y/O ABONOS. VERIFIQUE POR FAVOR...")
                            Else
                                i_modo = movimiento.iEditar
                                nPosicionEncab = Me.BindingContext(ds, nTabla).Position
                                ActivarMarco0()
                                ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, cmbCliente)
                            End If
                        End If
                    Else
                        ft.mensajeCritico("Esta FACTURA pertenece a LA RELACION DE FACTURAS N° " + dt.Rows(nPosicionEncab).Item("RELFACTURAS") + vbLf _
                                          + "por lo tanto su MODIFICACIÓN no está permitida")
                    End If
                Else
                    ft.mensajeCritico("Esta FACTURA pertenece a LA GUIA DE DESPACHO N° " + dt.Rows(nPosicionEncab).Item("RELGUIA") + vbLf _
                                      + "por lo tanto su MODIFICACION no está permitida")
                End If
            Else
                ft.mensajeCritico("Edición de Facturas NO está permitida...")
            End If
        End If
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Dim aCamposAdicionales() As String = {"numfac|'" & txtCodigo.Text & "'",
                                              "codcli|'" & cliente.Codcli & "'"}
        If DocumentoBloqueado(myConn, "jsvenencfac", aCamposAdicionales) Then
            ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
        Else
            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA01")) Then
                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                nPosicionEncab = Me.BindingContext(ds, nTabla).Position

                If dt.Rows(nPosicionEncab).Item("RELGUIA") = "" Then
                    If dt.Rows(nPosicionEncab).Item("RELFACTURAS") = "" Then
                        If DocumentoPoseeCancelacionesAbonos(myConn, lblInfo, txtCodigo.Text) Then
                            ft.mensajeCritico("ESTE DOCUMENTO POSEE ABONOS Y/O CANCELACIONES. VERIFIQUE POR FAVOR...")
                        Else
                            sRespuesta = MsgBox(" ¿ Está seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")
                            If sRespuesta = MsgBoxResult.Yes Then

                                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numfac"))
                                '
                                For Each dtRow As DataRow In dtRenglones.Rows
                                    With dtRow
                                        Eliminados.Add(.Item("item"))
                                        'Actualiza Cantidades en tránsito de los presupuestos y estatus del documento
                                        ActualizarRenglonesEnPresupuestos(myConn, lblInfo, ds, dtRenglones, "jsvenrenfac")

                                        'Actualiza cantidades en tránsito de los prepedidos y estatus de documento
                                        ActualizarRenglonesEnPrepedidos(myConn, lblInfo, ds, dtRenglones, "jsvenrenfac")

                                        'Actualiza cantidades en tránsito de los pedidos y estatus del documento
                                        ActualizarRenglonesEnPedidos(myConn, lblInfo, ds, dtRenglones, "jsvenrenfac")

                                        'Actualiza cantidades en tránsito de las notas de entrega y estatus del documento
                                        ActualizarRenglonesEnNotasDeEntrega(myConn, lblInfo, ds, dtRenglones, "jsvenrenfac")
                                    End With
                                Next

                                ft.Ejecutar_strSQL(myConn, " delete from jsvenencfac where numfac = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "'")
                                ft.Ejecutar_strSQL(myConn, " delete from jsvenrenfac where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " delete from jsvenivafac where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " DELETE from jsvendesfac where NUMFAC = '" & txtCodigo.Text & "' and ID_EMP = '" & jytsistema.WorkID & "' and EJERCICIO =  '" & jytsistema.WorkExercise & "' ")
                                ft.Ejecutar_strSQL(myConn, " DELETE FROM jsbantraban where NUMDOC = '" & txtCodigo.Text & "' AND TIPOMOV = 'DP' AND ORIGEN = 'FAC' AND NUMORG = '" & txtCodigo.Text & "' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " DELETE FROM jsbantracaj where NUMMOV = '" & txtCodigo.Text & "' AND ORIGEN = 'FAC' AND TIPOMOV = 'EN' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " delete from jsventracob where NUMMOV = '" & txtCodigo.Text & "' AND ORIGEN = 'FAC' AND TIPOMOV = 'FC' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " DELETE FROM jsventabtic WHERE NUMCAN = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " delete from jsvenrengui where codigofac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                EliminarMovimientosdeInventario(myConn, txtCodigo.Text, "FAC", lblInfo)

                                For Each aSTR As Object In Eliminados
                                    ActualizarExistenciasPlus(myConn, aSTR)
                                Next

                                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                                dt = ds.Tables(nTabla)
                                If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                                AsignaTXT(nPosicionEncab)

                            End If

                        End If
                    Else
                        ft.mensajeCritico("Esta FACTURA pertenece a LA RELACION DE FACTURAS N° " + dt.Rows(nPosicionEncab).Item("RELFACTURAS") + vbLf _
                                          + "por lo tanto su eliminación no está permitida")
                    End If
                Else
                    ft.mensajeCritico("Esta FACTURA pertenece a LA GUIA DE DESPACHO N° " + dt.Rows(nPosicionEncab).Item("RELGUIA") + vbLf _
                                      + "por lo tanto su eliminación no está permitida")
                End If
            Else
                ft.mensajeCritico("Eliminación de Facturas NO está permitida...")
            End If
        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numfac", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Nombre", "Emisión", "Comentario"}
        Dim Anchos() As Integer = {150, 400, 100, 150}
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

        txtSubTotal.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrenfac", "numfac", "totren", txtCodigo.Text, 0))

        Dim discountTable As New SimpleTableProperties("jsvendesfac", "numfac", txtCodigo.Text)
        MostrarDescuentosAlbaran(myConn, discountTable, dgDescuentos, txtDescuentos)

        txtCargos.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrenfac", "numfac", "totren", txtCodigo.Text, 1))
        CalculaDescuentoEnRenglones()
        CalculaTotalIVAVentas(myConn, lblInfo, "jsvendesfac", "jsvenivafac", "jsvenrenfac", "numfac", txtCodigo.Text, "impiva", "totrendes", txtEmision.Value, "totren")

        Dim ivaTable As New SimpleTableProperties("jsvenivafac", "numfac", txtCodigo.Text)
        MostrarIVAAlbaran(myConn, ivaTable, dgIVA, txtTotalIVA)

        txtTotal.Text = ft.FormatoNumero(ValorNumero(txtSubTotal.Text) - ValorNumero(txtDescuentos.Text) + ValorNumero(txtCargos.Text) + ValorNumero(txtTotalIVA.Text))

        tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsvenrenfac", "peso", "numfac", txtCodigo.Text))

        ''   MostrarTotalesCambioMoneda(myConn, txtEmision, txtTotal, txtTotalCambioEmision, txtTotalActual)

    End Sub

    Private Sub CalculaDescuentoEnRenglones()

        ft.Ejecutar_strSQL(myConn, " update jsvenrenfac set totrendes = totren - totren * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
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
            f.Agregar(myConn, ds, dtRenglones, "FAC", txtCodigo.Text, txtEmision.Value, almacen.Codigo,
                      cmbTarifa.Text, cliente.Codcli, , , , , , , , , , asesor.Codigo)
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
                    f.Editar(myConn, ds, dtRenglones, "FAC", txtCodigo.Text, txtEmision.Value, almacen.Codigo, cmbTarifa.Text, cliente.Codcli,
                             IIf(.Item("item").ToString.Substring(0, 1) = "$", True, False), , , , , , , , , asesor.Codigo)
                    ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
                    AsignaMov(f.Apuntador, True)
                    CalculaTotales()
                    f = Nothing
                Else
                    ft.mensajeCritico(" Renglón NO EDITABLE ")
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
                               & " origen = 'FAC' and item = '" & .Item("item") & "' and renglon = '" & .Item("renglon") & "' and " _
                               & " id_emp = '" & jytsistema.WorkID & "' ")

                        nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsvenrenfac", strSQLMov, aCamposDel, aStringsDel,
                                                      Me.BindingContext(ds, nTablaRenglones).Position, True)

                        If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1
                        AsignaMov(nPosicionRenglon, True)

                        CalculaTotales()
                    Else
                        ft.mensajeCritico(" Renglón NO EDITABLE ")
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



    Private Sub btnAgregaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaDescuento.Click
        If cmbCliente.SelectedIndex >= 0 Then
            If ClienteFacturaAPartirDeCostos(myConn, lblInfo, cliente.Codcli) Then
                ft.mensajeCritico("DESCUENTOS NO PERMITIDOS")
            Else
                Dim f As New jsGenDescuentosVentas
                f.Agregar(myConn, ds, dtDescuentos, "jsvendesfac", "numfac", txtCodigo.Text, sModulo, asesor.Codigo, 0, txtEmision.Value, ValorNumero(txtSubTotal.Text))
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
                nPosicionDescuento = EliminarRegistros(myConn, lblInfo, ds, nTablaDescuentos, "jsvendesfac", strSQLDescuentos, aCamposDel, aStringsDel,
                                                      Me.BindingContext(ds, nTablaDescuentos).Position, True)
            End With
            CalculaTotales()
        End If
    End Sub

    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If cmbCliente.SelectedIndex >= 0 Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "FAC", txtCodigo.Text, txtEmision.Value, almacen.Codigo, cmbTarifa.Text, cliente.Codcli, True)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Private Sub dgDescuentos_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Down) _
            Or e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Up) Then
            MsgBox("BAJO")
        End If
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
                    ft.Ejecutar_strSQL(myConn, " update jsvenrenfac " _
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

    Private Sub btnRecalcular_Click(sender As System.Object, e As System.EventArgs) Handles btnRecalcular.Click
        If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) > 0 Then
            CalculaTotales()
            ft.Ejecutar_strSQL(myConn, " update jsvenencfac set tot_net = " & ValorNumero(txtSubTotal.Text) & ", descuen = " & ValorNumero(txtDescuentos.Text) & " , cargos = " & ValorNumero(txtCargos.Text) & " , imp_iva = " & ValorNumero(txtTotalIVA.Text) & ", tot_fac = " & ValorNumero(txtTotal.Text) & "  where numfac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            AsignaTXT(nPosicionEncab)
        End If
    End Sub

    Private Sub btnNotaEntrega_Click(sender As Object, e As EventArgs) Handles btnNotaEntrega.Click
        ' Todo bring delivery notes
        If txtCodigo.Text.Trim() <> "" Then
            If cmbCliente.SelectedIndex >= 0 Then
                Dim f As New jsGenListadoSeleccion
                Dim aNombres() As String = {"", "Entrega N°", "Emisión", "Monto", ""}
                Dim aCampos() As String = {"sel", "codigo", "emision", "tot_not", ""}
                Dim aAnchos() As Integer = {20, 120, 120, 200, 100}
                Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Center,
                                                            HorizontalAlignment.Center, HorizontalAlignment.Center,
                                                            HorizontalAlignment.Right, HorizontalAlignment.Right}
                Dim aFormato() As String = {"", "", sFormatoFecha, sFormatoNumero, ""}
                Dim aFields() As String = {"sel.entero.1.0", "codigo.cadena.20.0", "emision.fecha.0.0", "tot_fac.doble.19.2", ".cadena.20.0"}

                Dim str As String = "  select 0 sel, numfac codigo, emision, tot_fac from jsvenencnot where " _
                        & " codcli = '" & cliente.Codcli & "' AND " _
                        & " ESTATUS < '1' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND" _
                        & " ID_EMP = '" & jytsistema.WorkID & "' order by numfac desc "

                f.Cargar(myConn, ds, "Notas de entrega sin facturar", str,
                    aFields, aNombres, aCampos, aAnchos, aAlineacion, aFormato)
            End If
        End If

    End Sub

    Private Sub btnPedido_Click(sender As Object, e As EventArgs) Handles btnPedido.Click
        If txtCodigo.Text.Trim() <> "" Then
            If cmbCliente.SelectedIndex >= 0 Then

                Dim f As New jsGenListadoSeleccion
                Dim aNombres() As String = {"", "Pedido N°", "Emisión", "Monto", ""}
                Dim aCampos() As String = {"sel", "codigo", "emision", "tot_ped", ""}
                Dim aAnchos() As Integer = {20, 120, 120, 200, 100}
                Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Center,
                                                            HorizontalAlignment.Center, HorizontalAlignment.Center,
                                                            HorizontalAlignment.Right, HorizontalAlignment.Right}
                Dim aFormato() As String = {"", "", sFormatoFecha, sFormatoNumero, ""}
                Dim aFields() As String = {"sel.entero.1.0", "codigo.cadena.20.0", "emision.fecha.0.0", "tot_ped.doble.19.2", ".cadena.20.0"}

                Dim str As String = "  select 0 sel, numped codigo, emision, tot_ped from jsvenencped where " _
                        & " codcli = '" & cliente.Codcli & "' AND " _
                        & " ESTATUS < '1' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND" _
                        & " ID_EMP = '" & jytsistema.WorkID & "' order by numped desc "

                f.Cargar(myConn, ds, "Pedidos en Tránsito", str,
                    aFields, aNombres, aCampos, aAnchos, aAlineacion, aFormato)

                If f.Seleccion.Length > 0 Then
                    Dim cod As String
                    For Each cod In f.Seleccion
                        Dim nTablaRenglonesped As String = "tblRenglonesped" & ft.NumeroAleatorio(100000)
                        Dim dtRenglonesped As New DataTable
                        ds = DataSetRequery(ds, " select * from jsvenrenped " _
                                            & " where " _
                                            & " cantran > 0 AND " _
                                            & " numped = '" & cod & "' and " _
                                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon ", myConn, nTablaRenglonesped, lblInfo)

                        dtRenglonesped = ds.Tables(nTablaRenglonesped)

                        For Each rRow As DataRow In dtRenglonesped.Rows
                            With rRow
                                Dim pesoTransito As Double = .Item("peso") / .Item("cantidad") * .Item("cantran")

                                Dim numRenglon As String = ft.autoCodigo(myConn, "RENGLON", "jsvenrenfac", "numfac.id_emp",
                                                                      txtCodigo.Text + "." + jytsistema.WorkID, 5)

                                Dim Incluye As Boolean = True
                                '1. SI ES MULTIPLO VALIDO

                                '2. EXISTENCIAS
                                Dim Equivalencia As Double = FuncionesMercancias.Equivalencia(myConn, .Item("ITEM"), .Item("UNIDAD"))
                                Dim CantidadReal As Double = .Item("CANTRAN") / IIf(Equivalencia = 0, 1, Equivalencia)

                                Dim extAlm As Double = ExistenciasEnAlmacenes(myConn, .Item("ITEM"), almacen.Codigo)
                                If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA12").ToString) And
                                    CantidadReal > extAlm Then Incluye = False

                                '3. CUOTA FIJA ASESOR
                                '4. PROVEEDOR EXCLUSIVO

                                '5. PRECIO Y CANTIDAD MAYOR QUE CERO 
                                Dim PrecioTotal As Double = .Item("precio") * .Item("cantran") * (1 - .Item("des_cli") / 100) * (1 - .Item("des_art") * (1 - .Item("des_ofe")) / 100)
                                Dim UltCosto As Double = UltimoCostoAFecha(myConn, .Item("Item"), txtEmision.Value, .Item("UNIDAD"))

                                If PrecioTotal <= 0 Then Incluye = False
                                If .Item("precio") = 0.0 Then Incluye = False
                                If .Item("precio") < UltCosto Then Incluye = False

                                '//////////
                                If Incluye Then _
                                InsertEditVENTASRenglonFactura(myConn, lblInfo, True, txtCodigo.Text, numRenglon,
                                    .Item("item"), .Item("descrip"), .Item("iva"), "", .Item("unidad"), 0.0, .Item("cantran"), 0, 0, 0, "", "",
                                    pesoTransito, "", .Item("estatus"), .Item("precio"), .Item("des_cli"), .Item("des_art"), .Item("des_ofe"),
                                    .Item("precio") * .Item("cantran"), .Item("precio") * .Item("cantran") * (1 - .Item("des_cli") / 100) * (1 - .Item("des_art") * (1 - .Item("des_ofe")) / 100),
                                    .Item("numcot"), .Item("rencot"), .Item("numped"), .Item("renglon"), "", "", "", "", "", "1", 0)

                            End With

                        Next

                        AsignarMovimientos(txtCodigo.Text)
                        CalculaTotales()

                        dtRenglonesped.Dispose()
                        dtRenglonesped = Nothing
                    Next

                End If
                f = Nothing
            Else
                ft.mensajeCritico("DEBE INDICAR UN CLIENTE VALIDO...")
            End If
        Else
            ft.mensajeCritico("DEBE INDICAR UN NUMERO DE FACTURA VALIDO...")
        End If

    End Sub

    Private Sub btnPrepedido_Click(sender As Object, e As EventArgs) Handles btnPrepedido.Click
        If txtCodigo.Text.Trim() <> "" Then
            If cmbCliente.SelectedIndex >= 0 Then

                Dim f As New jsGenListadoSeleccion
                Dim aNombres() As String = {"", "Pre-Pedido N°", "Emisión", "Monto", ""}
                Dim aCampos() As String = {"sel", "codigo", "emision", "tot_ped", ""}
                Dim aAnchos() As Integer = {20, 120, 120, 200, 100}
                Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Center,
                                                            HorizontalAlignment.Center, HorizontalAlignment.Center,
                                                            HorizontalAlignment.Right, HorizontalAlignment.Right}
                Dim aFormato() As String = {"", "", sFormatoFecha, sFormatoNumero, ""}
                Dim aFields() As String = {"sel.entero.1.0", "codigo.cadena.20.0", "emision.fecha.0.0", "tot_ped.doble.19.2", ".cadena.20.0"}

                Dim str As String = "  select 0 sel, numped codigo, emision, tot_ped from jsvenencpedrgv where " _
                        & " codcli = '" & cliente.Codcli & "' AND " _
                        & " ESTATUS < '1' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND" _
                        & " ID_EMP = '" & jytsistema.WorkID & "' order by numped desc "

                f.Cargar(myConn, ds, "Pre-Pedidos en Tránsito", str,
                    aFields, aNombres, aCampos, aAnchos, aAlineacion, aFormato)

                If f.Seleccion.Length > 0 Then
                    Dim cod As String
                    For Each cod In f.Seleccion
                        Dim nTablaRenglonesped As String = "tblRenglonesped" & ft.NumeroAleatorio(100000)
                        Dim dtRenglonesped As New DataTable
                        ds = DataSetRequery(ds, " select * from jsvenrenpedrgv " _
                                            & " where " _
                                            & " cantran > 0 AND " _
                                            & " numped = '" & cod & "' and " _
                                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon ", myConn, nTablaRenglonesped, lblInfo)

                        dtRenglonesped = ds.Tables(nTablaRenglonesped)

                        For Each rRow As DataRow In dtRenglonesped.Rows
                            With rRow

                                Dim pesoTransito As Double = .Item("peso") / .Item("cantidad") * .Item("cantran")

                                Dim numRenglon As String = ft.autoCodigo(myConn, "RENGLON", "jsvenrenfac", "numfac.id_emp",
                                                                      txtCodigo.Text + "." + jytsistema.WorkID, 5)

                                Dim Incluye As Boolean = True
                                '1. SI ES MULTIPLO VALIDO

                                '2. EXISTENCIAS
                                Dim Equivalencia As Double = FuncionesMercancias.Equivalencia(myConn, .Item("ITEM"), .Item("UNIDAD"))
                                Dim CantidadReal As Double = .Item("CANTRAN") / IIf(Equivalencia = 0, 1, Equivalencia)
                                Dim extAlm As Double = ExistenciasEnAlmacenes(myConn, .Item("ITEM"), almacen.Codigo)

                                If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA12").ToString) And
                                    CantidadReal > extAlm Then Incluye = False

                                '3. CUOTA FIJA ASESOR
                                '4. PROVEEDOR EXCLUSIVO
                                '5. PRECIO Y CANTIDAD MAYOR QUE CERO 
                                Dim PrecioTotal As Double = .Item("precio") * .Item("cantran") * (1 - .Item("des_cli") / 100) * (1 - .Item("des_art") * (1 - .Item("des_ofe")) / 100)
                                Dim UltCosto As Double = UltimoCostoAFecha(myConn, .Item("Item"), txtEmision.Value, .Item("UNIDAD"))

                                If PrecioTotal <= 0 Then Incluye = False
                                If .Item("precio") = 0.0 Then Incluye = False
                                If .Item("precio") < UltCosto Then Incluye = False

                                If Incluye Then _
                                InsertEditVENTASRenglonFactura(myConn, lblInfo, True, txtCodigo.Text, numRenglon,
                                     .Item("item"), .Item("descrip"), .Item("iva"), "", .Item("unidad"), 0.0, .Item("cantran"), 0, 0, 0, "", "",
                                      pesoTransito, "", .Item("estatus"), .Item("precio"), .Item("des_cli"), .Item("des_art"), .Item("des_ofe"),
                                      .Item("precio") * .Item("cantran"), .Item("precio") * .Item("cantran") * (1 - .Item("des_cli") / 100) * (1 - .Item("des_art") * (1 - .Item("des_ofe")) / 100),
                                      .Item("numcot"), .Item("rencot"), .Item("numped"), .Item("renglon"), "", "", "", "", "", "1", 0)

                            End With
                            AsignarMovimientos(txtCodigo.Text)
                            CalculaTotales()
                        Next
                        dtRenglonesped.Dispose()
                        dtRenglonesped = Nothing
                    Next

                End If
                f = Nothing
            Else
                ft.mensajeCritico("DEBE INDICAR UN CLIENTE VALIDO...")
            End If
        Else
            ft.mensajeCritico("DEBE INDICAR UN NUMERO DE FACTURA VALIDO...")
        End If
    End Sub

    Private Sub cmbCliente_Enter(sender As Object, e As EventArgs) Handles cmbCliente.Enter
        cmbCliente.Text = ""
    End Sub

    Private Sub cmbMonedas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbMonedas.SelectedIndexChanged
        moneda = cmbMonedas.SelectedItem
    End Sub

    Private Sub cmbTransportes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTransportes.SelectedIndexChanged
        transporte = cmbTransportes.SelectedItem
    End Sub

    Private Sub cmbAlmacenes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAlmacenes.SelectedIndexChanged
        almacen = cmbAlmacenes.SelectedItem
    End Sub

    Private Sub cmbCliente_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCliente.SelectedIndexChanged

        If cmbCliente.SelectedIndex >= 0 Then
            cliente = cmbCliente.SelectedItem
            MostrarDisponibilidad(myConn, lblInfo, cliente.Codcli, cliente.Rif, ds, dgDisponibilidad)

            Dim mTotalFac As Double = ValorNumero(txtTotal.Text)
            If i_modo = movimiento.iAgregar Then

                mTotalFac = 0.0
                FechaVencimiento = FechaVencimientoFactura(myConn, cliente.Codcli, txtEmision.Value)
                CondicionDePago = CondicionPagoProveedorCliente(myConn, lblInfo, cliente.FormaPago)
                cmbTarifa.SelectedIndex = ft.InArray(aTarifa, cliente.Tarifa)
                cmbAsesores.SelectedValue = cliente.Asesor
                cmbTransportes.SelectedValue = IIf(cliente.Transporte = "", transporte.Codigo, cliente.Transporte)
                IncluirFleteAutomatico(cliente.Codcli, montoFletesPorRegion(myConn, cliente.Codcli))

            End If
            lblDisponibilidad.Text = "Disponible menos este Documento : " & ft.FormatoNumero(cliente.Disponible + MontoAnterior - mTotalFac)

        End If

    End Sub
    Private Sub cmbAsesores_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAsesores.SelectedIndexChanged
        asesor = cmbAsesores.SelectedItem
    End Sub

End Class