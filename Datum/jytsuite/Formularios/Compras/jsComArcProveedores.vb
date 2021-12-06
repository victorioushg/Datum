Imports MySql.Data.MySqlClient
Imports C1.Win.C1Chart
Imports Syncfusion.WinForms.Input
Imports fTransport
Imports dgFieldSF = fTransport.Models.SfDataGridField
Imports fTransport.Models

Public Class jsComArcProveedores

    Private Const sModulo As String = "Proveedores y CxP"
    Private Const lRegion As String = "RibbonButton55"
    Private Const nTabla As String = "tblProveedores"
    Private Const nTablaSaldos As String = "tblSaldosXDocumento"
    Private Const nTablaEnvases As String = "tblEnvases"

    Private strSQL As String = "select * from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by codpro "
    Private strSQLSaldos As String = ""
    Private strSQLEnvases As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtSaldos As New DataTable
    Private dtEnvases As New DataTable

    Private ft As New Transportables

    Private vendorTransactionsList As List(Of VendorTransaction)
    Private vendorTransactionsExPList As List(Of VendorTransaction)
    Private proveedor As DataRow
    Private cxpSelected As VendorTransaction

    Private aIVA() As String
    Private aCondicion() As String = {"Activo", "Inactivo"}
    Private aTipo() As String = {"Compras", "Gastos", "Compras/Gastos", "Otros"}
    Private aTipoEstadistica() As String = {"Compras", "Devoluciones"}
    Private aSaldos() As String = {"Actuales", "Históricos"}

    Private i_modo As Integer
    Private nPosicionCat As Long
    Private nPosicionMov As Long
    Private nPosicionEst As Long
    Private nPosicionMovExP As Long
    Private nPosicionEnv As Long


    Private iSel As Integer = 0
    Private dSel As Double = 0.0
    Private strSel As String = " nummov = 'XX XX' OR "

    Private aTipoMovimiento() As String = {"Factura", "Giro", "Nota Débito", "Abono", "Cancelación", "Nota Crédito", "---", "Retención IVA", "Retención ISLR"}
    Private aTipoNick() As String = {"FC", "GR", "ND", "AB", "CA", "NC", "", "NC", "NC"}



    Private tblSaldos As String = ""

    Private Sub jsComArcProveedores_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsComArcProveedores_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo

        Try
            myConn.Open()
            dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

            Dim dates As SfDateTimeEdit() = {txtIngreso}
            SetSizeDateObjects(dates)

            ft.RellenaCombo(aCondicion, cmbCondicion)
            ft.RellenaCombo(aSaldos, cmbSaldos)

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaTXT(nPosicionCat)
            Else
                IniciarProveedor(False)
            End If

            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            tbcProveedor.SelectedTab = C1DockingTabPage1
            AsignarTooltips()


        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()

        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnSeleccionar, btnPrimero,
                         btnSiguiente, btnAnterior, btnUltimo, btnImprimir, btnSalir, btnSubirHistorico, btnBajarHistorico,
                         btnReprocesar, btnExP, btnAuditoria)
        'Botones Adicionales
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnUnidad, btnZona, btnForma, btnBanco, btnBancoDeposito, btnDep2)

        c1Chart1.ToolTip.Enabled = True
        c1Chart1.ToolTip.SelectAction = SelectActionEnum.MouseOver
        c1Chart1.ToolTip.PlotElement = PlotElementEnum.Points
        c1Chart1.ToolTip.AutomaticDelay = 0
        c1Chart1.ToolTip.InitialDelay = 0


    End Sub
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean, Optional Comprobante As String = "")

        If Actualiza Then
            Cursor.Current = Cursors.WaitCursor
            vendorTransactionsList = GetVendorTransactions(myConn, proveedor.Item("codpro"))
            dg.DataSource = vendorTransactionsList
            If Comprobante <> "" Then nRow = vendorTransactionsList.IndexOf(vendorTransactionsList.FirstOrDefault(Function(f) f.NumeroMovimiento = Comprobante))
        End If
        dg.SelectedIndex = nRow

        MostrarItemsEnMenuBarra(MenuBarra, CInt(nRow), vendorTransactionsList.Count)

    End Sub

    Private Sub AsignaMovExP(ByVal nRow As Long, ByVal Actualiza As Boolean, Optional Comprobante As String = "")
        If Actualiza Then
            Cursor.Current = Cursors.WaitCursor
            vendorTransactionsExPList = GetVendorTransactions(myConn, proveedor.Item("codpro"), "1")
            dgExP.DataSource = vendorTransactionsExPList
            dgExP.Refresh()
            If Comprobante <> "" Then nRow = vendorTransactionsExPList.IndexOf(vendorTransactionsExPList.FirstOrDefault(Function(f) f.NumeroMovimiento = Comprobante))
        End If
        MostrarItemsEnMenuBarra(MenuBarra, CInt(nRow), vendorTransactionsExPList.Count)

    End Sub


    Private Sub AsignaTXT(ByVal nRow As Long)

        If dt.Rows.Count > 0 Then

            proveedor = dt.Rows(nRow)

            With dt

                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

                With .Rows(nRow)
                    '
                    txtCodigo.Text = ft.muestraCampoTexto(.Item("codpro"))
                    txtNombre.Text = ft.muestraCampoTexto(.Item("nombre"))
                    ft.RellenaCombo(aTipo, cmbTipo, .Item("tipo"))
                    ft.RellenaCombo(aCondicion, cmbCondicion, .Item("estatus"))
                    txtUnidad.Text = ft.muestraCampoTexto(.Item("unidad"))
                    txtCategoria.Text = ft.muestraCampoTexto(.Item("categoria"))
                    txtRIF.Text = ft.muestraCampoTexto(.Item("rif"))
                    txtNIT.Text = ft.muestraCampoTexto(.Item("nit"))
                    txtAsignado.Text = ft.muestraCampoTexto(.Item("asignado"))
                    txtCodigoContable.Text = ft.muestraCampoTexto(.Item("codcon"))

                    Dim aIVA() As String = ArregloIVA(myConn, lblInfo)
                    ft.RellenaCombo(aIVA, cmbIVA, ft.InArray(aIVA, .Item("regimeniva")))

                    txtZona.Text = ft.muestraCampoTexto(.Item("zona"))
                    txtDireccionFiscal.Text = ft.muestraCampoTexto(.Item("dirfiscal"))
                    txtDireccionAlterna.Text = ft.muestraCampoTexto(.Item("dirprove"))
                    txtTelefono1.Text = ft.muestraCampoTexto(.Item("telef1"))
                    txtTelefono2.Text = ft.muestraCampoTexto(.Item("telef2"))
                    txtTelefono3.Text = ft.muestraCampoTexto(.Item("telef3"))
                    txtFAX.Text = ft.muestraCampoTexto(.Item("fax"))
                    txtWebSite.Text = ft.muestraCampoTexto(.Item("email4"))
                    txtGerente.Text = ft.muestraCampoTexto(.Item("gerente"))
                    txtTelefonoGerente.Text = ft.muestraCampoTexto(.Item("telger"))
                    txtemail1.Text = ft.muestraCampoTexto(.Item("email1"))
                    txtContacto.Text = ft.muestraCampoTexto(.Item("contacto"))
                    txtTelefonoContacto.Text = ft.muestraCampoTexto(.Item("telcon"))
                    txtemail2.Text = ft.muestraCampoTexto(.Item("email2"))
                    txtVendedor.Text = ft.muestraCampoTexto(.Item("vendedor"))
                    txtemail3.Text = ft.muestraCampoTexto(.Item("email3"))
                    txtCobrador.Text = ft.muestraCampoTexto(.Item("cobrador"))
                    txtemail4.Text = ft.muestraCampoTexto(.Item("sitioweb"))
                    txtFormaPago.Text = ft.muestraCampoTexto(.Item("formapago"))
                    txtBancoPago.Text = ft.muestraCampoTexto(.Item("banco"))
                    txtBancoDep1.Text = ft.muestraCampoTexto(.Item("bancodeposito1"))
                    txtBancoDep2.Text = ft.muestraCampoTexto(.Item("bancodeposito2"))
                    txtBancoDep1Cta.Text = ft.muestraCampoTexto(.Item("ctabancodeposito1"))
                    txtBancoDep2Cta.Text = ft.muestraCampoTexto(.Item("ctabancodeposito2"))
                    txtIngreso.Text = .Item("ingreso")
                    txtCredito.Text = ft.muestraCampoNumero(.Item("limcredito"))
                    txtSaldo.Text = ft.muestraCampoNumero(.Item("saldo"))
                    txtDisponible.Text = ft.muestraCampoNumero(.Item("disponible"))

                    'Movimientos
                    txtCodigo1.Text = ft.muestraCampoTexto(.Item("codpro"))
                    txtNombre1.Text = ft.muestraCampoTexto(.Item("nombre"))
                    txtUltimoPago.Text = ft.muestraCampoNumero(.Item("ultpago"))
                    txtFechaUltimopago.Text = ft.muestraCampoFecha(.Item("fecultpago"))
                    txtFormaUltimoPago.Text = ft.muestraCampoTexto(.Item("forultpago"))

                    'Movimientos ExP
                    txtCodigoExP.Text = ft.muestraCampoTexto(.Item("codpro"))
                    txtNombreExP.Text = ft.muestraCampoTexto(.Item("nombre"))
                    txtUltimoPagoExP.Text = ft.FormatoNumero(0.0) 'ft.FormatoNumero(CDbl(ft.MuestraCampoTexto(.Item("ultpago"), "0.00")))
                    txtFechaUltimoPagoExP.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)    'ft.FormatoFecha(CDate(ft.MuestraCampoTexto(.Item("fecultpago"), CStr(jytsistema.sFechadeTrabajo))))
                    txtFormaUltimoPagoExP.Text = "" 'ft.MuestraCampoTexto(.Item("forultpago"))

                    txtCreditoM.Text = ft.muestraCampoNumero(.Item("limcredito"))
                    txtSaldoM.Text = ft.muestraCampoNumero(.Item("saldo"))
                    txtDisponibleM.Text = ft.muestraCampoNumero(.Item("disponible"))

                    txtCreditoExP.Text = ft.FormatoNumero(0.0)
                    txtSaldoExP.Text = ft.FormatoNumero(0.0) ' ft.FormatoNumero(CDbl(ft.MuestraCampoTexto(.Item("saldo"), "0.00")))
                    txtDisponibleExP.Text = ft.FormatoNumero(0.0) ' ft.FormatoNumero(CDbl(ft.MuestraCampoTexto(.Item("disponible"), "0.00")))


                    AsignaMovimientosExP(.Item("codpro"))

                    'Salods por Documento 
                    txtCodigoSaldos.Text = ft.muestraCampoTexto(.Item("codpro"))
                    txtNombreSaldos.Text = ft.muestraCampoTexto(.Item("nombre"))

                    'Estadística
                    txtCodigo2.Text = ft.muestraCampoTexto(.Item("codpro"))
                    txtNombre2.Text = ft.muestraCampoTexto(.Item("nombre"))

                    'Expediente
                    txtCodigoExpediente.Text = ft.muestraCampoTexto(.Item("codpro"))
                    txtNombreExpediente.Text = ft.muestraCampoTexto(.Item("nombre"))

                    'Envases
                    txtCodigoEnvase.Text = ft.muestraCampoTexto(.Item("codpro"))
                    txtNombreEnvase.Text = ft.muestraCampoTexto(.Item("nombre"))

                End With
            End With
        End If

        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub AsignaMovimientosExP(ByVal CodigoProveedor As String)

        Dim dtUltPagoExP As DataTable
        Dim nTablaUltPagoExP As String = "tblUltimoPagoExP"

        ds = DataSetRequery(ds, " select emision, importe, formapag, numpag, nompag from jsprotrapag where codpro = '" & CodigoProveedor & "' and " _
                    & " tipomov in ('AB','CA') and " _
                    & " remesa = '1' and " _
                    & " id_emp = '" & jytsistema.WorkID & "' " _
                    & " order by emision desc limit 1 ", myConn, nTablaUltPagoExP, lblInfo)

        dtUltPagoExP = ds.Tables(nTablaUltPagoExP)

        If dtUltPagoExP.Rows.Count > 0 Then
            With dtUltPagoExP.Rows(0)
                txtUltimoPagoExP.Text = .Item("importe")
                txtFechaUltimoPagoExP.Text = ft.FormatoFecha(CDate(.Item("emision").ToString))
                txtFormaUltimoPagoExP.Text = .Item("formapag") & " " & .Item("nompag") & " " & .Item("numpag")
            End With
        Else
            txtUltimoPagoExP.Text = "0.00"
            txtFechaUltimoPagoExP.Text = ""
            txtFormaUltimoPagoExP.Text = ""
        End If

        Dim aFld() As String = {"codpro", "id_emp"}
        Dim aStr() As String = {CodigoProveedor, jytsistema.WorkID}

        txtSaldoExP.Text = ft.FormatoNumero(SaldoExP(myConn, lblInfo, CodigoProveedor))

    End Sub

    Private Sub AbrirMovimientosExP(ByVal CodigoProveedor As String)

        AsignaMovimientosExP(CodigoProveedor)
        vendorTransactionsExPList = GetVendorTransactions(myConn, CodigoProveedor, "1")
        Dim aCampos As List(Of dgFieldSF) = New List(Of dgFieldSF)() From {
            New dgFieldSF(TypeColumn.DateTimeColumn, "Emision", "Emision", 80, HorizontalAlignment.Center, FormatoFecha.Corta),
            New dgFieldSF(TypeColumn.TextColumn, "TipoMovimiento", "TP", 50, HorizontalAlignment.Center, ""),
            New dgFieldSF(TypeColumn.TextColumn, "NumeroMovimiento", "Documento", 120, HorizontalAlignment.Left, ""),
            New dgFieldSF(TypeColumn.TextColumn, "Concepto", "Concepto", 350, HorizontalAlignment.Left, ""),
            New dgFieldSF(TypeColumn.DateTimeColumn, "Vencimiento", "Vencimiento", 120, HorizontalAlignment.Center, FormatoFecha.Corta),
            New dgFieldSF(TypeColumn.TextColumn, "Referencia", "Referencia", 120, HorizontalAlignment.Left, ""),
            New dgFieldSF(TypeColumn.NumericColumn, "Importe", "Importe", 120, HorizontalAlignment.Right, FormatoNumero.FormatoNumero),
            New dgFieldSF(TypeColumn.NumericColumn, "ImporteReal", "Importe(" + WorkCurrency.CodigoISO + ")", 120, HorizontalAlignment.Right, FormatoNumero.FormatoNumero),
            New dgFieldSF(TypeColumn.TextColumn, "Origen", "ORG", 80, HorizontalAlignment.Center, ""),
            New dgFieldSF(TypeColumn.TextColumn, "FormaDePago", "FP", 50, HorizontalAlignment.Center, ""),
            New dgFieldSF(TypeColumn.TextColumn, "NombreDePago", "Nombre de Pago", 180, HorizontalAlignment.Left, ""),
            New dgFieldSF(TypeColumn.TextColumn, "NumeroDePago", "Numero de Pago", 180, HorizontalAlignment.Left, ""),
            New dgFieldSF(TypeColumn.TextColumn, "Comprobante", "Nº Comprobante", 180, HorizontalAlignment.Left, "")
        }

        ft.IniciarDataGridWithList(Of VendorTransaction)(dgExP, vendorTransactionsExPList, aCampos,,,,,,,,, False)

        If vendorTransactionsExPList.Count > 0 Then
            nPosicionMovExP = 0
            AsignarTextosMovimientosExP(nPosicionMovExP)
        End If

    End Sub


    Private Sub AbrirMovimientos(ByVal CodigoProveedor As String)

        Cursor.Current = Cursors.WaitCursor
        txtUltimoPago.Text = ft.FormatoNumero(proveedor.Item("ultpago"))
        txtFechaUltimopago.Text = ft.FormatoFecha(proveedor.Item("fecultpago").ToString())
        txtFormaUltimoPago.Text = proveedor.Item("forultpago") ''& " " & .Item("nompag") & " " & .Item("numpag")

        SaldoCxP(myConn, lblInfo, CodigoProveedor)

        txtCredito.Text = ft.FormatoNumero(proveedor.Item("limcredito"))
        txtCreditoM.Text = ft.FormatoNumero(proveedor.Item("limcredito"))
        txtSaldo.Text = ft.FormatoNumero(proveedor.Item("saldo"))
        txtSaldoM.Text = ft.FormatoNumero(proveedor.Item("saldo"))
        txtDisponible.Text = ft.FormatoNumero(proveedor.Item("disponible"))
        txtDisponibleM.Text = ft.FormatoNumero(proveedor.Item("disponible"))

        vendorTransactionsList = GetVendorTransactions(myConn, CodigoProveedor)
        Dim aCampos As List(Of dgFieldSF) = New List(Of dgFieldSF)() From {
            New dgFieldSF(TypeColumn.DateTimeColumn, "Emision", "Emision", 80, HorizontalAlignment.Center, FormatoFecha.Corta),
            New dgFieldSF(TypeColumn.TextColumn, "TipoMovimiento", "TP", 50, HorizontalAlignment.Center, ""),
            New dgFieldSF(TypeColumn.TextColumn, "NumeroMovimiento", "Documento", 120, HorizontalAlignment.Left, ""),
            New dgFieldSF(TypeColumn.TextColumn, "Concepto", "Concepto", 350, HorizontalAlignment.Left, ""),
            New dgFieldSF(TypeColumn.DateTimeColumn, "Vencimiento", "Vencimiento", 120, HorizontalAlignment.Center, FormatoFecha.Corta),
            New dgFieldSF(TypeColumn.TextColumn, "Referencia", "Referencia", 120, HorizontalAlignment.Left, ""),
            New dgFieldSF(TypeColumn.NumericColumn, "Importe", "Importe", 120, HorizontalAlignment.Right, FormatoNumero.FormatoNumero),
            New dgFieldSF(TypeColumn.NumericColumn, "ImporteReal", "Importe(" + WorkCurrency.CodigoISO + ")", 120, HorizontalAlignment.Right, FormatoNumero.FormatoNumero),
            New dgFieldSF(TypeColumn.TextColumn, "Origen", "ORG", 80, HorizontalAlignment.Center, ""),
            New dgFieldSF(TypeColumn.TextColumn, "FormaDePago", "FP", 50, HorizontalAlignment.Center, ""),
            New dgFieldSF(TypeColumn.TextColumn, "NombreDePago", "Nombre de Pago", 180, HorizontalAlignment.Left, ""),
            New dgFieldSF(TypeColumn.TextColumn, "NumeroDePago", "Numero de Pago", 180, HorizontalAlignment.Left, ""),
            New dgFieldSF(TypeColumn.TextColumn, "Comprobante", "Nº Comprobante", 180, HorizontalAlignment.Left, "")
        }

        ft.IniciarDataGridWithList(Of VendorTransaction)(dg, vendorTransactionsList, aCampos,,,,,,,,, False)

        If vendorTransactionsList.Count > 0 Then
            nPosicionMov = 0
            AsignarTextosMovimientos(nPosicionMov)

            'For Each Item As DataGridViewRow In dg.Rows
            '    With Item
            '        If .Cells("refer").Value.ToString.Length >= 2 Then
            '            If .Cells("REFER").Value.ToString.Substring(0, 2) = "FL" Then .DefaultCellStyle.BackColor = ColorRojoClaro
            '        End If
            '    End With
            'Next

        End If
    End Sub


    Private Sub IniciarProveedor(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = Contador(myConn, lblInfo, Gestion.iCompras, "COMNUMPRO", "01")
        Else
            txtCodigo.Text = ""
        End If

        ft.RellenaCombo(aTipo, cmbTipo)
        ft.RellenaCombo(aCondicion, cmbCondicion)

        txtRIF.Text = ""
        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNombre, txtNombre1, txtUnidad, txtUnidadNombre, txtCategoria, txtCategoriaNombre,
                            txtNIT, txtAsignado, txtIVA, txtZona, txtZonaNombre, txtDireccionFiscal,
                            txtDireccionAlterna, txtTelefono1, txtTelefono2, txtTelefono3, txtFAX,
                            txtemail1, txtemail2, txtemail3, txtemail4, txtWebSite, txtGerente, txtTelefonoGerente, txtContacto,
                            txtTelefonoContacto, txtVendedor, txtCobrador, txtFormaPago, txtFormaPagoNombre, txtBancoPago,
                            txtBancoPagoNombre, txtBancoDep1, txtBancoDep2, txtBancoDep1Nombre,
                            txtBancoDep2Nombre, txtBancoDep1Cta, txtBancoDep2Cta, txtBancoPagoCuenta, txtCodigoContable)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtCredito, txtSaldo, txtDisponible)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Fecha, txtFechaUltimopago)

        Dim aIVA() As String = ArregloIVA(myConn, lblInfo)
        ft.RellenaCombo(aIVA, cmbIVA, ft.InArray(aIVA, "A"))

        'Movimientos
        dg.Columns.Clear()

    End Sub
    Private Sub ActivarMarco0()

        ft.visualizarObjetos(True, grpAceptarSalir)
        ft.habilitarObjetos(False, False, MenuBarra, tbcProveedor.TabPages(1), tbcProveedor.TabPages(2), tbcProveedor.TabPages(3),
                             tbcProveedor.TabPages(4), tbcProveedor.TabPages(5))
        ft.habilitarObjetos(True, True, cmbTipo, cmbCondicion, txtNombre, btnUnidad, txtNIT, txtRIF, txtAsignado,
                                cmbIVA, btnIVA, btnZona, txtDireccionAlterna, txtDireccionFiscal, txtTelefono1,
                                txtTelefono2, txtTelefono3, txtFAX, txtWebSite, txtGerente, txtTelefonoGerente,
                                txtemail1, txtCobrador, txtContacto, txtVendedor, txtTelefonoContacto, txtemail2,
                                txtemail3, txtemail4, btnForma, txtIngreso, btnBanco, btnBancoDeposito, btnDep2,
                                txtBancoDep1Cta, txtBancoDep2Cta, txtCredito)

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub


    Private Sub DesactivarMarco0()

        ft.visualizarObjetos(False, grpAceptarSalir)
        ft.habilitarObjetos(True, False, MenuBarra, tbcProveedor.TabPages(1), tbcProveedor.TabPages(2), tbcProveedor.TabPages(3),
                             tbcProveedor.TabPages(4), tbcProveedor.TabPages(5), tbcProveedor.TabPages(6))

        ft.habilitarObjetos(False, True, txtCodigo, cmbTipo, txtAsignado, cmbCondicion, txtNombre, txtUnidadNombre, btnUnidad,
                          txtCategoriaNombre, txtRIF, txtNIT, cmbIVA, txtIVA, btnIVA, btnZona, txtZona, txtDireccionFiscal,
                          txtDireccionAlterna, txtTelefono1, txtTelefono2, txtTelefono3, txtFAX, txtWebSite, txtemail1,
                          txtemail2, txtemail3, txtemail4, txtGerente, txtTelefonoGerente, txtContacto, txtTelefonoContacto,
                          txtFormaPagoNombre, btnForma, btnBanco, txtBancoDep1, txtBancoDep1Cta, btnBancoDeposito, btnDep2,
                          txtBancoDep1Nombre, txtBancoDep2Nombre, txtBancoDep1Cta, txtBancoDep2Cta, txtBancoPagoCuenta,
                          txtBancoPagoNombre, txtIngreso, txtCredito, txtSaldo, txtDisponible,
                          txtUltimoPago, txtFormaUltimoPago, txtFechaUltimopago, txtCreditoM, txtSaldoM, txtDisponibleM,
                          txtVendedor, txtCobrador, txtZonaNombre,
                          txtCodigo1, txtNombre1, txtCodigo2, txtNombre2, txtCodigoSaldos, txtNombreSaldos,
                          txtCodigoExP, txtNombreExP, txtUltimoPagoExP, txtFormaUltimoPagoExP, txtFechaUltimoPagoExP,
                          txtCreditoExP, txtSaldoExP, txtDisponibleExP,
                          txtCodigoExpediente, txtNombreExpediente, txtDocSel, txtSaldoSel, txtCodigoContable)

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Select Case tbcProveedor.SelectedTab.Text
            Case "Proveedor"
                Me.BindingContext(ds, nTabla).Position = 0
                nPosicionCat = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case "Movimientos CxP"
                nPosicionMov = 0
                AsignaMov(nPosicionMov, False)
            Case "Movimientos ExP"
                nPosicionMovExP = 0
                AsignaMovExP(nPosicionMovExP, False)
            Case "Estadísticas"
                nPosicionCat = 0
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AbrirEstadisticas()
            Case "Saldos por Documento"
                Me.BindingContext(ds, nTabla).Position = 0
                nPosicionCat = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarSaldosPorDocumento(cmbSaldos.SelectedIndex, txtCodigo.Text)
            Case "Expediente"
                nPosicionCat = 0
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarExpediente(txtCodigo.Text)
        End Select

    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Select Case tbcProveedor.SelectedTab.Text
            Case "Proveedor"
                Me.BindingContext(ds, nTabla).Position -= 1
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case "Movimientos CxP"
                nPosicionMov = IIf(dg.SelectedIndex - 1 < 0, 0, dg.SelectedIndex - 1)
                AsignaMov(nPosicionMov, False)
            Case "Movimientos ExP"
                nPosicionMovExP = IIf(dgExP.SelectedIndex - 1 < 0, 0, dgExP.SelectedIndex - 1)
                AsignaMovExP(nPosicionMovExP, False)
            Case "Estadísticas"
                Me.BindingContext(ds, nTabla).Position -= 1
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AbrirEstadisticas()
            Case "Saldos por Documento"
                Me.BindingContext(ds, nTabla).Position -= 1
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarSaldosPorDocumento(cmbSaldos.SelectedIndex, txtCodigo.Text)
            Case "Expediente"
                Me.BindingContext(ds, nTabla).Position -= 1
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarExpediente(txtCodigo.Text)
        End Select


    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Select Case tbcProveedor.SelectedTab.Text
            Case "Proveedor"
                Me.BindingContext(ds, nTabla).Position += 1
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case "Movimientos CxP"
                nPosicionMov = IIf(dg.SelectedIndex + 1 > dg.RowCount, dg.RowCount, dg.SelectedIndex + 1)
                AsignaMov(nPosicionMov, False)
            Case "Movimientos ExP"
                nPosicionMovExP = IIf(dgExP.SelectedIndex + 1 > dgExP.RowCount, dgExP.RowCount, dgExP.SelectedIndex + 1)
                AsignaMovExP(nPosicionMovExP, False)
            Case "Estadísticas"
                Me.BindingContext(ds, nTabla).Position += 1
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AbrirEstadisticas()
            Case "Saldos por Documento"
                Me.BindingContext(ds, nTabla).Position += 1
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarSaldosPorDocumento(cmbSaldos.SelectedIndex, txtCodigo.Text)
            Case "Expediente"
                Me.BindingContext(ds, nTabla).Position += 1
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarExpediente(txtCodigo.Text)
        End Select

    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Select Case tbcProveedor.SelectedTab.Text
            Case "Proveedor"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case "Movimientos CxP"
                nPosicionMov = dg.RowCount - 1
                AsignaMov(nPosicionMov, False)
            Case "Movimientos ExP"
                nPosicionMovExP = dgExP.RowCount - 1
                AsignaMovExP(nPosicionMovExP, False)
            Case "Estadísticas"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AbrirEstadisticas()
            Case "Saldos por Documento"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                IniciarSaldosPorDocumento(cmbSaldos.SelectedIndex, txtCodigo.Text)
            Case "Expediente"
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
                AsignarExpediente(txtCodigo.Text)
        End Select

    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

        Select Case tbcProveedor.SelectedTab.Text
            Case "Proveedor"
                i_modo = movimiento.iAgregar
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
                IniciarProveedor(True)
            Case "Movimientos CxP"
                If Trim(txtCodigo.Text) <> "" Then
                    Dim f As New jsComArcProveedoresCXP
                    f.Apuntador = 0
                    f.Agregar(myConn, txtCodigo.Text, cmbTipo.SelectedIndex)
                    SaldoCxP(myConn, lblInfo, txtCodigo.Text)
                    If nPosicionMov >= 0 Then
                        AsignaMov(nPosicionMov, True, f.Comprobante)
                    End If
                    f = Nothing
                End If
            Case "Movimientos ExP"
                If Trim(txtCodigo.Text) <> "" Then
                    Dim f As New jsComArcProveedoresCXP
                    f.Agregar(myConn, txtCodigo.Text, cmbTipo.SelectedIndex, 1)
                    SaldoExP(myConn, lblInfo, txtCodigo.Text)
                    If nPosicionMovExP >= 0 Then
                        AsignaMovExP(nPosicionMovExP, True)
                    End If
                    f = Nothing
                End If
            Case "Estadísticas"
            Case "Expediente"
                Dim g As New jsComArcProveedoresExpediente
                g.Agregar(myConn, txtCodigo.Text, cmbCondicion.SelectedIndex)
                AsignarExpediente(txtCodigo.Text)
                g.Dispose()
                g = Nothing
        End Select

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Select Case tbcProveedor.SelectedTab.Text
            Case "Proveedor"
                i_modo = movimiento.iEditar
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
            Case "Movimientos CxP"
            Case "Estadísticas"
            Case "Expediente"
            Case "Movimientos ExP"
        End Select

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Select Case tbcProveedor.SelectedTab.Text
            Case "Proveedor"
                EliminaProveedor()
            Case "Movimientos CxP"
                If cmbCondicion.Text = "Activo" Then
                    EliminarMovimiento()
                Else
                    ft.mensajeCritico("Este proveedor está Inactivo ....")
                End If
            Case "Movimientos ExP"
                If cmbCondicion.Text = "Activo" Then
                    EliminarMovimientoExP()
                    AsignaMovimientosExP(txtCodigo.Text)
                Else
                    ft.mensajeCritico("Este proveedor está Inactivo ....")
                End If

            Case "Estadísticas"
        End Select

    End Sub
    Private Sub EliminaProveedor()

        Dim aCamposDel() As String = {"codpro", "id_emp"}
        Dim aStringsDel() As String = {txtCodigo.Text, jytsistema.WorkID}

        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

            If PoseeMovimientosAsociados(myConn, lblInfo, txtCodigo.Text) Then
                ft.mensajeCritico("Este banco posee movimientos. Verifique por favor ...")
            Else
                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsprocatpro", strSQL, aCamposDel, aStringsDel,
                                              Me.BindingContext(ds, nTabla).Position, True))
            End If
        End If

    End Sub

    Private Function PoseeMovimientosAsociados(ByVal MyConn As MySqlConnection, ByVal lblInfo As System.Windows.Forms.Label,
                                               ByVal CodigoProveedor As String, Optional ByVal NumeroMovimiento As String = "",
                                               Optional ByVal TipoMovimiento As String = "",
                                               Optional ByVal CxP_ExP As Integer = 0) As Boolean

        Dim cuenta As Integer = ft.DevuelveScalarEntero(MyConn, " select count(*) from jsprotrapag where " _
                                                           & " codpro = '" & CodigoProveedor & "' and " _
                                                           & IIf(NumeroMovimiento <> "", " nummov = '' and ", "") _
                                                           & IIf(TipoMovimiento <> "", " tipomov <> '' and ", "") _
                                                           & IIf(CxP_ExP = 0, "", " REMESA= '1' AND ") _
                                                           & " id_emp = '" & jytsistema.WorkID & "' ")
        If cuenta > 0 Then Return True

    End Function
    Private Sub EliminarMovimiento()

        nPosicionMov = vendorTransactionsList.IndexOf(cxpSelected)
        If Not cxpSelected Is Nothing Then
            If cxpSelected.Origen = "CXP" Then

                If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                    If ft.FormatoFechaMySQL(cxpSelected.FechaBloqueo) <> "2009-01-01" Then
                        ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                    Else
                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, cxpSelected.NumeroMovimiento)

                        Select Case cxpSelected.TipoMovimiento
                            Case "FC", "GR", "ND"
                                If PoseeMovimientosAsociados(myConn, lblInfo, cxpSelected.CodigoProveedor, cxpSelected.NumeroMovimiento, cxpSelected.TipoMovimiento) Then
                                    ft.mensajeAdvertencia("Este documento posee documentos asociados a él...")
                                Else
                                    ft.Ejecutar_strSQL(myConn, "DELETE from jsprotrapag where " _
                                        & " CODPRO = '" & cxpSelected.CodigoProveedor & "' and " _
                                        & " TIPOMOV ='" & cxpSelected.TipoMovimiento & "' and " _
                                        & " EMISION = '" & ft.FormatoFechaMySQL(cxpSelected.Emision) & "'AND " _
                                        & " NUMMOV = '" & cxpSelected.NumeroMovimiento & "' and " _
                                        & " REFER = '" & cxpSelected.Referencia & "' AND " _
                                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                        & " ID_EMP ='" & jytsistema.WorkID & "' ")
                                End If
                            Case "AB", "CA", "NC"
                                If cxpSelected.Multicancelacion = "1" Then
                                    If ft.PreguntaEliminarRegistro("Este documento pertenece a una cancelación múltiple. Se eliminará en todos los documentos") = Windows.Forms.DialogResult.No Then
                                        Return
                                    End If
                                End If
                                ft.Ejecutar_strSQL(myConn, "DELETE from jsprotrapag where " _
                                                                & " CODPRO = '" & cxpSelected.CodigoProveedor & "' AND " _
                                                                & " REFER = '" & cxpSelected.Referencia & "' AND " _
                                                                & " COMPROBA = '" & cxpSelected.Comprobante & "' AND  " _
                                                                & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                                                & " ID_EMP = '" & jytsistema.WorkID & "' ")

                                ft.Ejecutar_strSQL(myConn, "DELETE from jsprotrapagcan where " _
                                                                & " CODPRO = '" & cxpSelected.CodigoProveedor & "' AND " _
                                                                & " COMPROBA = '" & cxpSelected.Comprobante & "' AND  " _
                                                                & " ID_EMP = '" & jytsistema.WorkID & "' ")

                                ' Actualiza encabezado de compra
                                ft.Ejecutar_strSQL(myConn, " update jsproenccom set " _
                                                                & " formapag = '', numpag = '', nompag = '', benefic = '', caja = '' " _
                                                                & IIf(cxpSelected.TipoMovimiento = "NC" AndAlso Mid(cxpSelected.Referencia, 1, 5) = "ISLR-", ", ret_islr = 0.00, num_ret_islr = '', fecha_ret_islr = '2007-01-01' ", "") _
                                                                & IIf(cxpSelected.TipoMovimiento = "NC" AndAlso Mid(cxpSelected.Concepto, 1, 13) = "RETENCION IVA", ", ret_iva = 0.00, num_ret_iva = '', fecha_ret_iva = '2007-01-01' ", "") _
                                                                & " where " _
                                                                & " numcom = '" & cxpSelected.NumeroMovimiento & "' and " _
                                                                & " codpro = '" & cxpSelected.CodigoProveedor & "' and " _
                                                                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                                                & " id_emp = '" & jytsistema.WorkID & "' ")

                                If cxpSelected.TipoMovimiento = "NC" AndAlso Mid(cxpSelected.Referencia, 1, 5) = "ISLR-" Then
                                    ft.Ejecutar_strSQL(myConn, " update jsproenccom set ret_islr = 0.00, num_ret_islr = '', " _
                                                                               & " fecha_ret_islr = '2007-01-01', por_ret_islr = 0.00, base_ret_islr = 0.00 " _
                                                                               & " where codpro = '" & cxpSelected.CodigoProveedor & "' and num_ret_islr = '" & cxpSelected.Referencia & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                End If

                                If cxpSelected.TipoMovimiento = "NC" AndAlso Mid(cxpSelected.Concepto, 1, 13) = "RETENCION IVA" Then
                                    ft.Ejecutar_strSQL(myConn, " update jsproenccom set ret_iva = 0.00, num_ret_iva = '', " _
                                                                               & " fecha_ret_iva = '2007-01-01' " _
                                                                               & " where codpro = '" & cxpSelected.CodigoProveedor & "' and num_ret_iva = '" & cxpSelected.Referencia & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                    ft.Ejecutar_strSQL(myConn, " update jsproivacom set retencion = 0.00, numretencion = '' " _
                                                                              & " where codpro = '" & cxpSelected.CodigoProveedor & "' and numcom = '" & cxpSelected.NumeroMovimiento & "' and numretencion = '" & cxpSelected.Referencia & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                End If


                                ' Actualiza encabezado de gasto
                                ft.Ejecutar_strSQL(myConn, " update jsproencgas set " _
                                                                & " formapag = '', numpag = '', nompag = '', benefic = '', caja = '' " _
                                                                & IIf(cxpSelected.TipoMovimiento = "NC" AndAlso Mid(cxpSelected.Referencia, 1, 5) = "ISLR-", ", ret_islr = 0.00, num_ret_islr = '', fecha_ret_islr = '2007-01-01' ", "") _
                                                                & IIf(cxpSelected.TipoMovimiento = "NC" AndAlso Mid(cxpSelected.Concepto, 1, 13) = "RETENCION IVA", ", ret_iva = 0.00, num_ret_iva = '', fecha_ret_iva = '2007-01-01' ", "") _
                                                                & " where " _
                                                                & " numgas = '" & cxpSelected.NumeroMovimiento & "' and " _
                                                                & " codpro = '" & cxpSelected.CodigoProveedor & "' and " _
                                                                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                                                & " id_emp = '" & jytsistema.WorkID & "' ")


                                If cxpSelected.TipoMovimiento = "NC" AndAlso Mid(cxpSelected.Referencia, 1, 5) = "ISLR-" Then
                                    ft.Ejecutar_strSQL(myConn, " update jsproencgas set ret_islr = 0.00, num_ret_islr = '', " _
                                                                               & " fecha_ret_islr = '2007-01-01', por_ret_islr = 0.00, base_ret_islr = 0.00 " _
                                                                               & " where codpro = '" & cxpSelected.CodigoProveedor & "' and num_ret_islr = '" & cxpSelected.Referencia & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                End If

                                If cxpSelected.TipoMovimiento = "NC" AndAlso Mid(cxpSelected.Concepto, 1, 13) = "RETENCION IVA" Then
                                    ft.Ejecutar_strSQL(myConn, " update jsproencgas set ret_iva = 0.00, num_ret_iva = '', " _
                                                                               & " fecha_ret_iva = '2007-01-01' " _
                                                                               & " where codpro = '" & cxpSelected.CodigoProveedor & "' and num_ret_iva = '" & cxpSelected.Referencia & "' and id_emp = '" & jytsistema.WorkID & "' ")

                                    ft.Ejecutar_strSQL(myConn, " update jsproivagas set retencion = 0.00, numretencion = '' " _
                                                   & " where codpro = '" & cxpSelected.CodigoProveedor & "' and numgas = '" & cxpSelected.NumeroMovimiento & "' and numretencion = '" & cxpSelected.Referencia & "' and id_emp = '" & jytsistema.WorkID & "' ")

                                End If


                                'elimina movimiento en caja
                                ft.Ejecutar_strSQL(myConn, " delete from jsbantracaj where " _
                                                                & " nummov = '" & cxpSelected.Comprobante & "' and " _
                                                                & " origen = 'CXP' and " _
                                                                & " caja = '" & cxpSelected.CajaDePago & "' and " _
                                                                & " id_emp = '" & jytsistema.WorkID & "'")

                                'elimina movimiento en banco
                                ft.Ejecutar_strSQL(myConn, " delete from jsbantraban where " _
                                                                & " numorg = '" & cxpSelected.Comprobante & "' and " _
                                                                & " origen = 'CXP' and " _
                                                                & " codban = '" & cxpSelected.NombreDePago & "' and " _
                                                                & " id_emp = '" & jytsistema.WorkID & "'")

                                'ELIMINAR IMPUESTO AL DEBITO BANCARIO
                                If cxpSelected.FormaDePago = "CH" Or cxpSelected.FormaDePago = "TR" Or cxpSelected.FormaDePago = "TA" Then
                                    EliminarImpuestoDebitoBancario(myConn, lblInfo, cxpSelected.NombreDePago, cxpSelected.NumeroDePago, cxpSelected.Emision)
                                End If


                        End Select

                        If nPosicionMov > vendorTransactionsList.Count - 1 Then nPosicionMov = vendorTransactionsList.Count - 1
                        AsignaMov(nPosicionMov, True)

                    End If

                End If
                Else
                ft.mensajeAdvertencia("Movimiento no puede ser eliminado desde CXP...")
            End If
        End If

    End Sub

    Private Sub EliminarMovimientoExP()


        'nPosicionMovExP = Me.BindingContext(ds, nTablaMovimientosExP).Position

        'If nPosicionMovExP >= 0 Then
        '    If dtMovimientosExP.Rows(nPosicionMovExP).Item("origen") = "CXP" Then

        '        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
        '            With dtMovimientosExP.Rows(nPosicionMovExP)

        '                Dim aCamposAdicionales() As String = {"codpro|'" & txtCodigo.Text & "'",
        '                                                      "emision|'" & ft.FormatoFechaMySQL(Convert.ToDateTime(.Item("EMISION"))) & "'",
        '                                                      "nummov|'" & .Item("NUMMOV") & "'",
        '                                                      "tipomov|'" & .Item("TIPOMOV") & "'",
        '                                                      "hora|'" & .Item("HORA") & "'",
        '                                                      "tipo|'" & .Item("TIPO") & "'"}

        '                If DocumentoBloqueado(myConn, "jsprotrapag", aCamposAdicionales) Then
        '                    ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
        '                Else

        '                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("nummov"))
        '                    Dim TipoMovimiento As String = .Item("tipomov")
        '                    Select Case TipoMovimiento
        '                        Case "FC", "GR", "ND"
        '                            If PoseeMovimientosAsociados(myConn, lblInfo, .Item("codpro"), .Item("nummov"), .Item("tipomov"), 1) Then
        '                                ft.mensajeAdvertencia("Este documento posee documentos asociados a él...")
        '                            Else
        '                                ft.Ejecutar_strSQL(myConn, "DELETE from jsprotrapag where " _
        '                                    & " CODPRO = '" & .Item("codpro") & "' and " _
        '                                    & " TIPOMOV ='" & .Item("tipomov") & "' and " _
        '                                    & " EMISION = '" & ft.FormatoFechaMySQL(CDate(.Item("emision").ToString)) & "'AND " _
        '                                    & " NUMMOV = '" & .Item("nummov") & "' and " _
        '                                    & " REFER = '" & .Item("refer") & "' AND " _
        '                                    & " REMESA = '1' AND " _
        '                                    & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
        '                                    & " ID_EMP ='" & jytsistema.WorkID & "' ")
        '                            End If
        '                        Case "AB", "CA", "NC"

        '                            If .Item("multican") = "1" Then
        '                                If ft.PreguntaEliminarRegistro("Este documento pertenece a una cancelación múltiple. Se eliminará en todos los documentos") = Windows.Forms.DialogResult.No Then
        '                                    Return
        '                                End If
        '                            End If
        '                            ft.Ejecutar_strSQL(myConn, "DELETE from jsprotrapag where " _
        '                                & " CODPRO = '" & .Item("codpro") & "' AND " _
        '                                & " REFER = '" & .Item("refer") & "' AND " _
        '                                & " COMPROBA = '" & .Item("comproba") & "' AND  " _
        '                                & " REMESA = '1' AND " _
        '                                & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
        '                                & " ID_EMP = '" & jytsistema.WorkID & "' ")

        '                            ft.Ejecutar_strSQL(myConn, "DELETE from jsprotrapagcan where " _
        '                                & " CODPRO = '" & .Item("codpro") & "' AND " _
        '                                & " COMPROBA = '" & .Item("comproba") & "' AND  " _
        '                                & " ID_EMP = '" & jytsistema.WorkID & "' ")

        '                            ' Actualiza encabezado de compra
        '                            ft.Ejecutar_strSQL(myConn, " update jsproenccom set " _
        '                                & " formapag = '', numpag = '', nompag = '', benefic = '', caja = '' " _
        '                                & IIf(.Item("tipomov") = "NC" AndAlso Mid(.Item("REFER"), 1, 5) = "ISLR-", ", ret_islr = 0.00, num_ret_islr = '', fecha_ret_islr = '2007-01-01' ", "") _
        '                                & IIf(.Item("tipomov") = "NC" AndAlso Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA", ", ret_iva = 0.00, num_ret_iva = '', fecha_ret_iva = '2007-01-01' ", "") _
        '                                & " where " _
        '                                & " numcom = '" & .Item("nummov") & "' and " _
        '                                & " codpro = '" & .Item("codpro") & "' and " _
        '                                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
        '                                & " id_emp = '" & jytsistema.WorkID & "' ")

        '                            If .Item("tipomov") = "NC" AndAlso Mid(.Item("REFER"), 1, 5) = "ISLR-" Then
        '                                ft.Ejecutar_strSQL(myConn, " update jsproenccom set ret_islr = 0.00, num_ret_islr = '', " _
        '                                               & " fecha_ret_islr = '2007-01-01', por_ret_islr = 0.00, base_ret_islr = 0.00 " _
        '                                               & " where codpro = '" & .Item("codpro") & "' and num_ret_islr = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")
        '                            End If

        '                            If .Item("tipomov") = "NC" AndAlso Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA" Then
        '                                ft.Ejecutar_strSQL(myConn, " update jsproenccom set ret_iva = 0.00, num_ret_iva = '', " _
        '                                               & " fecha_ret_iva = '2007-01-01' " _
        '                                               & " where codpro = '" & .Item("codpro") & "' and num_ret_iva = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")

        '                                ft.Ejecutar_strSQL(myConn, " update jsproivacom set retencion = 0.00, numretencion = '' " _
        '                                              & " where codpro = '" & .Item("codpro") & "' and numcom = '" & .Item("nummov") & "' and numretencion = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")

        '                            End If


        '                            ' Actualiza encabezado de gasto
        '                            ft.Ejecutar_strSQL(myConn, " update jsproencgas set " _
        '                                & " formapag = '', numpag = '', nompag = '', benefic = '', caja = '' " _
        '                                & IIf(.Item("tipomov") = "NC" AndAlso Mid(.Item("REFER"), 1, 5) = "ISLR-", ", ret_islr = 0.00, num_ret_islr = '', fecha_ret_islr = '2007-01-01' ", "") _
        '                                & IIf(.Item("tipomov") = "NC" AndAlso Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA", ", ret_iva = 0.00, num_ret_iva = '', fecha_ret_iva = '2007-01-01' ", "") _
        '                                & " where " _
        '                                & " numgas = '" & .Item("nummov") & "' and " _
        '                                & " codpro = '" & .Item("codpro") & "' and " _
        '                                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
        '                                & " id_emp = '" & jytsistema.WorkID & "' ")


        '                            If .Item("tipomov") = "NC" AndAlso Mid(.Item("REFER"), 1, 5) = "ISLR-" Then
        '                                ft.Ejecutar_strSQL(myConn, " update jsproencgas set ret_islr = 0.00, num_ret_islr = '', " _
        '                                               & " fecha_ret_islr = '2007-01-01', por_ret_islr = 0.00, base_ret_islr = 0.00 " _
        '                                               & " where codpro = '" & .Item("codpro") & "' and num_ret_islr = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")
        '                            End If

        '                            If .Item("tipomov") = "NC" AndAlso Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA" Then
        '                                ft.Ejecutar_strSQL(myConn, " update jsproencgas set ret_iva = 0.00, num_ret_iva = '', " _
        '                                               & " fecha_ret_iva = '2007-01-01' " _
        '                                               & " where codpro = '" & .Item("codpro") & "' and num_ret_iva = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")

        '                                ft.Ejecutar_strSQL(myConn, " update jsproivagas set retencion = 0.00, numretencion = '' " _
        '                                              & " where codpro = '" & .Item("codpro") & "' and numgas = '" & .Item("nummov") & "' and numretencion = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")

        '                            End If


        '                            'elimina movimiento en caja
        '                            ft.Ejecutar_strSQL(myConn, " delete from jsbantracaj where " _
        '                                & " nummov = '" & .Item("comproba") & "' and " _
        '                                & " origen = 'CXP' and " _
        '                                & " caja = '" & .Item("cajapag") & "' and " _
        '                                & " id_emp = '" & jytsistema.WorkID & "'")

        '                            'elimina movimiento en banco
        '                            ft.Ejecutar_strSQL(myConn, " delete from jsbantraban where " _
        '                                & " numorg = '" & .Item("comproba") & "' and " _
        '                                & " origen = 'CXP' and " _
        '                                & " codban = '" & .Item("nompag") & "' and " _
        '                                & " id_emp = '" & jytsistema.WorkID & "'")
        '                    End Select

        '                    ds = DataSetRequery(ds, strSQLMovExP, myConn, nTablaMovimientosExP, lblInfo)
        '                    dtMovimientosExP = ds.Tables(nTablaMovimientosExP)

        '                    If nPosicionMovExP > dtMovimientosExP.Rows.Count - 1 Then nPosicionMovExP = dtMovimientosExP.Rows.Count - 1
        '                    AsignaMovExP(nPosicionMovExP, False)
        '                End If

        '            End With
        '        End If
        '    Else
        '        ft.mensajeAdvertencia("Movimiento no puede ser eliminado desde CXP...")
        '    End If
        'End If

    End Sub


    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar
        Select Case tbcProveedor.SelectedTab.Text
            Case "Proveedor"
                Dim Campos() As String = {"codpro", "nombre", "rif"}
                Dim Nombres() As String = {"Código proveedor", "Nombre o razón social proveedor", "RIF"}
                Dim Anchos() As Integer = {100, 750, 100}
                f.Text = "Proveedores"
                f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Proveedores", 1)
                nPosicionCat = f.Apuntador
                Me.BindingContext(ds, nTabla).Position = nPosicionCat
                AsignaTXT(nPosicionCat)
                f = Nothing
            Case "Movimientos CxP"
                '                Dim Campos() As String = {"fechamov", "numdoc", "concepto", "nombre"}
                '               Dim Nombres() As String = {"Emisión", "Nº Movimiento", "Concepto", "Cliente ó Proveedor"}
                '               Dim Anchos() As Integer = {100, 120, 2450, 2450}
                '               f.Text = "Movimientos bancarios"
                '               f.Buscar(dtMovimientos, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaMovimientos).Position)
                '               AsignaMov(f.Apuntador, False)
                '               f = Nothing
            Case "Estadísticas"
        End Select


    End Sub


    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Select Case tbcProveedor.SelectedTab.Text
            Case "Proveedor"
                Dim f As New jsComRepParametros
                f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cFichaProveedor, "Ficha Proveedor", txtCodigo.Text)
                f = Nothing
            Case "Movimientos CxP"
                Dim f As New jsComRepParametros
                With cxpSelected
                    If .TipoMovimiento = "NC" And Mid(.Concepto, 1, 13) = "RETENCION IVA" Then
                        'IMPRIMIR COMPROBANTE RETENCIOIN IVA
                        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cRetencionIVA, "Comprobante de Retención de IVA", txtCodigo.Text, .Referencia)
                    ElseIf .TipoMovimiento = "ND" And Mid(.Concepto, 1, 13) = "RETENCION IVA" Then
                        'IMPRIMIR COMPROBANTE RETENCION IVA
                        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cRetencionIVA, "Comprobante de Retención de IVA", txtCodigo.Text, .Referencia)
                    ElseIf .TipoMovimiento = "NC" And Mid(.Referencia, 1, 5) = "ISLR-" Then
                        'IMPRIMNIR COMPROBANTE RETENCION ISLR
                        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cRetencionISLR, "Comprobante de Retención de ISLR", txtCodigo.Text, .Referencia)
                    ElseIf .Comprobante <> "" Then
                        'IMPRIMIR COMPROBANTE DE EGRESO
                        If .FormaDePago = "CH" Then
                            Dim fr As New jsBanRepParametros
                            fr.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cComprobanteDeEgreso, "COMPROBANTE DE PAGO", .NombreDePago, .Comprobante, , "CXP")
                            fr.Dispose()
                            fr = Nothing
                        Else
                            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cComprobantePago, "COMPROBANTE DE EGRESO",
                                                 txtCodigo.Text, .Comprobante)
                        End If

                    Else
                        'IMPRIMIR MOVIMIENTOS DE CXP
                        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cMovimientosProveedores, "Movimientos Proveedores",
                                         txtCodigo1.Text)
                    End If
                End With
                f = Nothing
            Case "Movimientos ExP"
                'Dim f As New jsComRepParametros
                'With dtMovimientosExP.Rows(nPosicionMovExP)
                '    If .Item("tipomov") = "NC" And Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA" Then
                '        'IMPRIMIR COMPROBANTE RETENCIOIN IVA
                '        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cRetencionIVA, "Comprobante de Retención de IVA",
                '                 txtCodigo.Text, .Item("refer"), , , , , 1)
                '    ElseIf .Item("tipomov") = "ND" And Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA" Then
                '        'IMPRIMIR COMPROBANTE RETENCION IVA
                '        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cRetencionIVA, "Comprobante de Retención de IVA",
                '                 txtCodigo.Text, .Item("refer"), , , , , 1)
                '    ElseIf .Item("tipomov") = "NC" And Mid(.Item("REFER"), 1, 5) = "ISLR-" Then
                '        'IMPRIMNIR COMPROBANTE RETENCION ISLR
                '        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cRetencionISLR, "Comprobante de Retención de ISLR",
                '                 txtCodigo.Text, .Item("refer"), , , , , 1)
                '    ElseIf dtMovimientosExP.Rows(nPosicionMovExP).Item("comproba") <> "" Then
                '        'IMPRIMIR COMPROBANTE DE EGRESO
                '        If dtMovimientosExP.Rows(nPosicionMovExP).Item("formapag") = "CH" Then
                '            Dim fr As New jsBanRepParametros
                '            fr.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cComprobanteDeEgreso, "COMPROBANTE DE PAGO",
                '                      dtMovimientosExP.Rows(nPosicionMovExP).Item("nompag"),
                '                      dtMovimientosExP.Rows(nPosicionMovExP).Item("COMPROBA"), , "CXP")
                '            fr.Dispose()
                '            fr = Nothing
                '        Else
                '            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cComprobantePago, "COMPROBANTE DE EGRESO",
                '                     txtCodigo.Text, dtMovimientosExP.Rows(nPosicionMovExP).Item("COMPROBA"))
                '        End If

                '    Else
                '        'IMPRIMIR MOVIMIENTOS DE CXP
                '        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cMovimientosProveedores, "Movimientos ExP Proveedores",
                '                 txtCodigo1.Text, , , , , , 1)
                '    End If
                'End With
                'f = Nothing

            Case "Estadísticas"
        End Select

    End Sub


    Private Function Validado() As Boolean

        Dim afld() As String = {"codpro", "id_emp"}
        Dim aStr() As String = {txtCodigo.Text, jytsistema.WorkID}

        If i_modo = movimiento.iAgregar AndAlso qFound(myConn, lblInfo, "jsprocatpro", afld, aStr) Then
            ft.mensajeAdvertencia("Código proveedor YA existe. Verifique por favor...")
            Return False
        End If

        If Trim(txtNombre.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre o razón social válida...")
            Return False
        End If


        If Not IIf(EsRIF(txtRIF.Text.Trim), validarRif(txtRIF.Text.Trim), validarCI(txtRIF.Text.Trim.Split("-")(0) + "-" + txtRIF.Text.Trim.Split("-")(1).Trim)) Then
            ft.mensajeAdvertencia(" CI o RIF no válido. Debe indicarlo de la forma V-11111111 ...")
            EnfocarTextoM(txtRIF)
            Exit Function
        End If

        Dim afldR() As String = {"rif", "id_emp"}
        Dim aStrR() As String = {Cedula_O_RIF(txtRIF.Text), jytsistema.WorkID}

        If i_modo = movimiento.iAgregar AndAlso qFound(myConn, lblInfo, "jsprocatpro", afldR, aStrR) Then
            ft.mensajeAdvertencia("RIF de proveedor YA existe. Verifique por favor...")
            Return False
        End If

        If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPARAM04")) Then
            If txtCodigoContable.Text.Trim = "" Then
                ft.mensajeAdvertencia("Debe indicar un código contable para este proveedor...")
                Return False
            End If
        End If

        Validado = True
    End Function
    Private Sub GuardarTXT()

        Dim Inserta As Boolean = False
        nPosicionCat = Me.BindingContext(ds, nTabla).Position
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionCat = ds.Tables(nTabla).Rows.Count
        End If

        Dim fechaUltima As Date
        If txtFechaUltimopago.Text.Trim = "" Then
            fechaUltima = jytsistema.sFechadeTrabajo
        Else
            fechaUltima = CDate(txtFechaUltimopago.Text)
        End If

        Dim sRIF As String
        If EsRIF(txtRIF.Text) Then
            sRIF = txtRIF.Text.Split("-")(0) + "-" + txtRIF.Text.Split("-")(1).Replace("_", "") + "-" + txtRIF.Text.Split("-")(2)
        Else
            sRIF = txtRIF.Text.Split("-")(0) + "-" + txtRIF.Text.Split("-")(1).Replace("_", "")
        End If

        InsertEditCOMPRASProveedores(myConn, lblInfo, Inserta, txtCodigo.Text, txtNombre.Text, txtCategoria.Text, txtUnidad.Text,
                                  sRIF, txtNIT.Text, txtAsignado.Text, txtDireccionFiscal.Text, txtDireccionAlterna.Text,
                                  txtemail1.Text, txtemail2.Text, txtemail3.Text, txtemail4.Text, "", txtWebSite.Text,
                                  txtTelefono1.Text, txtTelefono2.Text, txtTelefono3.Text, txtFAX.Text, txtGerente.Text,
                                  txtTelefonoGerente.Text, txtContacto.Text, txtTelefonoContacto.Text, ValorNumero(txtCredito.Text),
                                  ValorNumero(txtDisponible.Text), 0.0, 0.0, 0.0, 0.0, 0, 0, "", txtZona.Text, txtCobrador.Text,
                                  txtVendedor.Text, ValorNumero(txtSaldo.Text), ValorNumero(txtUltimoPago.Text),
                                  fechaUltima, IIf(txtFormaUltimoPago.Text.Length > 2, Mid(txtFormaUltimoPago.Text, 1, 2), ""), cmbIVA.Text, txtFormaPago.Text,
                                  txtBancoPago.Text, txtBancoPagoCuenta.Text, txtBancoDep1.Text, txtBancoDep2.Text,
                                  txtBancoDep1Cta.Text, txtBancoDep2Cta.Text, txtIngreso.Value, txtCodigoContable.Text, cmbCondicion.SelectedIndex,
                                  cmbTipo.SelectedIndex)

        SaldoCxP(myConn, lblInfo, txtCodigo.Text)

        InsertarAuditoria(myConn, IIf(Inserta, MovAud.iIncluir, MovAud.imodificar), sModulo & " - " & tbcProveedor.SelectedTab.Text, txtCodigo.Text)

        dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

        Dim row As DataRow = dt.Select(" CODPRO = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionCat = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionCat
        AsignaTXT(nPosicionCat)
        DesactivarMarco0()
        tbcProveedor.SelectedTab = C1DockingTabPage1
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus,
    txtNombre.GotFocus, cmbTipo.GotFocus, cmbCondicion.GotFocus, btnUnidad.GotFocus, txtNIT.GotFocus,
    txtAsignado.GotFocus, cmbIVA.GotFocus, btnIVA.GotFocus, btnZona.GotFocus, txtDireccionFiscal.GotFocus, txtDireccionAlterna.GotFocus,
    txtTelefono1.GotFocus, txtTelefono2.GotFocus, txtTelefono3.GotFocus, txtFAX.GotFocus, txtWebSite.GotFocus,
    txtGerente.GotFocus, txtTelefonoGerente.GotFocus, txtemail1.GotFocus, txtContacto.GotFocus, txtTelefonoContacto.GotFocus,
    txtemail2.GotFocus, txtVendedor.GotFocus, txtCobrador.GotFocus, txtemail3.GotFocus, txtemail4.GotFocus,
    btnForma.GotFocus, btnBanco.GotFocus, btnBancoDeposito.GotFocus, btnDep2.GotFocus, txtBancoPagoCuenta.GotFocus,
    txtBancoDep1Cta.GotFocus, txtBancoDep2Cta.GotFocus, txtCredito.GotFocus

        Dim mensaje As String = ""
        Select Case sender.name
            Case "txtCodigo"
                mensaje = "Indique código de proveedor deseado..."
            Case "txtNombre"
                mensaje = "Indique nombre y/o razón social del proveedor ..."
            Case "cmbTipo"
                mensaje = "Seleccione el tipo de proveedor ..."
            Case "cmbCondicion"
                mensaje = "seleccione la condición del proveedor ..."
            Case "btnUnidad"
                mensaje = "seleccione la unidad de negocio y categoría de proveedor ..."
            Case "txtRIF "
                mensaje = "Indique el RIF ó el número registro de información tributaria del proveedor ..."
            Case "txtNIT"
                mensaje = "Indique el NIT ó el número de información tributaria del proveedor ..."
            Case "txtAsignado"
                mensaje = "Indique el codigo asignado por el proveedor a nuestra empresa ..."
            Case "cmbIVA"
                mensaje = "seleccione la tasa de régimen IVA que pose el proveedor ..."
            Case "btnIVA"
                mensaje = "Incluye tasas de IVA..."
            Case "btnZona"
                mensaje = "Seleccione la zona del proveedor ..."
            Case "txtDireccionFiscal"
                mensaje = "Indique la dirección fiscal del proveedor ..."
            Case "txtDireccionAlterna"
                mensaje = "Indique una dirección alterna del proveedor ..."
            Case "txtTelefono1", "txtTelefono2", "txtTelefono3", "txtFAX"
                mensaje = "Indique los números de teléfono del proveedor ..."
            Case "txtWebSite"
                mensaje = "Indique la pagina web o web site del proveedor ..."
            Case "txtGerente"
                mensaje = "Indique el nombre del gerente de proveedor  ..."
            Case "txtTelefonoGerente"
                mensaje = "Indique el teléfono del gerente del proveedor ..."
            Case "txtemail1"
                mensaje = "Indique el correo electrónico del gerente del proveedor ..."
            Case "txtContacto"
                mensaje = "Indique el nombre de la persona contacto del proveedor ..."
            Case "txtTelefonoContacto"
                mensaje = "Indique el teléfono de la persona contacto del proveedor ..."
            Case "txtemail2"
                mensaje = "Indique el correo electrónico de la persona contacto del proveedor ..."
            Case "txtVendedor"
                mensaje = "Indique el nombre del asesor comercial del proveedor ..."
            Case "txtCobrador"
                mensaje = "Indique el nombre de la persona encargada de la cobranza del proveedor ..."
            Case "txtemail3", "txtemail4"
                mensaje = "Indique correo electrónico adicional del proveedor ..."
            Case "btnForma"
                mensaje = "Seleccione la forma de pago de regirá con este proveedor ..."
            Case "btnBanco"
                mensaje = "seleccione el banco con el cual se le pagará a el proveedor ..."
            Case "btnDep1, ""btnDep2"
                mensaje = "Seleccione el banco en cual se le depositará a este proveedor ..."
            Case "txtBancoDep1Cta", "txtBancoDep2Cta"
                mensaje = "Indique el número de cuenta donde se le depositará a este proveedor ..."
            Case "btnIngreso"
                mensaje = "seleccione la fecha de ingreso ó fecha desde que es nuestro proveedor ..."
            Case "txtCredito"
                mensaje = "Indique el monto límite de crédito de este proveedor ..."
        End Select
        ft.mensajeEtiqueta(lblInfo, mensaje, Transportables.tipoMensaje.iInfo)
    End Sub

    'Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles 
    '    dg.CellMouseClick, dg.RegionChanged

    '    Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
    '    nPosicionMov = e.RowIndex
    '    AsignarTextosMovimientos(nPosicionMov)

    'End Sub
    Private Sub AsignarTextosMovimientos(nRow As Long)

        'ft.MostrarItemsEnMenuBarra(MenuBarra, nRow, dtMovimientos.Rows.Count)

        'If dtMovimientos.Rows.Count > 0 Then
        '    With dtMovimientos.Rows(nRow)

        '        If .Item("origen") = "CXP" Then

        '            IniciarTipoMovimiento(.Item("TIPOMOV"), .Item("CONCEPTO"), .Item("REFER"), cmbTipoCxP)

        '            txtDocumentoCXP.Text = ft.muestraCampoTexto(.Item("nummov"))
        '            txtNombrePagoCxP.Text = ft.muestraCampoTexto(.Item("COMPROBA"))
        '            txtEmisionCXP.Text = ft.muestraCampoFecha(.Item("Emision"))
        '            txtCausaNCCXP.Text = ft.muestraCampoTexto(.Item("asiento"))
        '            IniciarCausaCredito(lblDescripCausaNCCxP, txtCausaNCCXP.Text, rbtnCRCXP, rbtnCOCXP, .Item("tipomov"), grpCondicionCXP)
        '            txtReferenciaCXP.Text = ft.muestraCampoTexto(.Item("refer"))
        '            txtConceptoCXP.Text = ft.muestraCampoTexto(.Item("concepto"))
        '            txtImporteCXP.Text = ft.muestraCampoNumero(.Item("importe"))

        '            IniciarCajas(.Item("CAJAPAG"), cmbCajaCxP)

        '            InitiateDropDown(Of TextoValor)(myConn, cmbFPCxP, Tipo.FormaDePago, 0)
        '            cmbFPCxP.SelectedValue = .Item("FORMAPAG")

        '            txtNumeroPagoCxP.Text = ft.muestraCampoTexto(.Item("NUMPAG"))

        '            IniciarNombrePago(.Item("FORMAPAG"), .Item("NOMPAG"), cmbNombrePagoCxP)

        '            txtBeneficiarioCxP.Text = ft.muestraCampoTexto(.Item("BENEFIC"))
        '            txtCodConCxP.Text = ft.muestraCampoTexto(.Item("CODCON"))


        '        End If

        '    End With
        'End If

    End Sub
    Private Sub AsignarTextosMovimientosExP(nRow As Long)

        'ft.MostrarItemsEnMenuBarra(MenuBarra, nRow, dtMovimientosExP.Rows.Count)

        'If dtMovimientosExP.Rows.Count > 0 Then
        '    With dtMovimientosExP.Rows(nRow)

        '        If .Item("origen") = "CXP" Then

        '            IniciarTipoMovimiento(.Item("TIPOMOV"), .Item("CONCEPTO"), .Item("REFER"), cmbTipoExP)

        '            txtDocumentoEXP.Text = ft.muestraCampoTexto(.Item("nummov"))
        '            txtNombrePagoExP.Text = ft.muestraCampoTexto(.Item("COMPROBA"))
        '            txtEmisionEXP.Text = ft.muestraCampoFecha(.Item("Emision"))
        '            txtCausaNCEXP.Text = ft.muestraCampoTexto(.Item("asiento"))
        '            IniciarCausaCredito(lblDescripCausaNCExP, txtCausaNCEXP.Text, rbtnCRExP, rbtnCOExP, .Item("tipomov"), grpCondicionExP)
        '            txtReferenciaExP.Text = ft.muestraCampoTexto(.Item("refer"))
        '            txtConceptoExP.Text = ft.muestraCampoTexto(.Item("concepto"))
        '            txtImporteExP.Text = ft.muestraCampoNumero(.Item("importe"))

        '            IniciarCajas(.Item("CAJAPAG"), cmbCajaExP)

        '            InitiateDropDown(Of TextoValor)(myConn, cmbFPExP, Tipo.FormaDePago)
        '            cmbFPExP.SelectedValue = .Item("FORMAPAG")

        '            txtNumeroPagoExP.Text = ft.muestraCampoTexto(.Item("NUMPAG"))

        '            IniciarNombrePago(.Item("FORMAPAG"), .Item("NOMPAG"), cmbNombrePagoExP)

        '            txtBeneficiarioExP.Text = ft.muestraCampoTexto(.Item("BENEFIC"))
        '            txtCodConExP.Text = ft.muestraCampoTexto(.Item("CODCON"))

        '        End If

        '    End With
        'End If

    End Sub
    Private Sub IniciarCausaCredito(lblDescripCausaNotaCredito As System.Windows.Forms.Label, CausaNotaCredito As String,
                                    rbtnCredito As RadioButton, rbtnContado As RadioButton,
                                    TipoMovimiento As String, grpCondicion As GroupBox)
        ft.visualizarObjetos(False, grpCondicion, lblDescripCausaNotaCredito)
        If TipoMovimiento = "NC" And CausaNotaCredito <> "" Then
            ft.visualizarObjetos(True, grpCondicion, lblDescripCausaNotaCredito)
            lblDescripCausaNotaCredito.Text = ft.DevuelveScalarCadena(myConn, " select descripcion from jsconcausas_notascredito " _
                                                                      & " where codigo = '" & CausaNotaCredito & "' and origen = 'CXP' ")
            If ft.DevuelveScalarBooleano(myConn, " select CR from jsconcausas_notascredito where codigo = '" & CausaNotaCredito & "' and origen = 'CXP' ") Then
                rbtnCredito.Checked = True
            Else
                rbtnContado.Checked = True
            End If
        End If

    End Sub
    Private Sub IniciarTipoMovimiento(TipoMovimiento As String, Concepto As String, Referencia As String, cmb As ComboBox)
        Dim nPos As Integer = 0
        Select Case TipoMovimiento
            Case "FC", "GR", "ND", "AB", "CA"
                nPos = ft.InArray(aTipoNick, TipoMovimiento)
            Case Else
                nPos = 5
                If Concepto.Length > 12 Then _
                    If Concepto.Substring(0, 13) = "RETENCION IVA" Then nPos = 7
                If Referencia.Length > 4 Then _
                    If Referencia.Substring(0, 5) = "ISLR-" Then nPos = 8
        End Select
        ft.RellenaCombo(aTipoMovimiento, cmb, nPos)

    End Sub
    Private Sub IniciarCajas(Caja As String, cmb As ComboBox)
        If Caja.Equals("") Then
            If cmb.Items.Count > 0 Then
                cmb.DataSource = Nothing
                cmb.Items.Clear()
            End If

        Else
            Dim dtCajas As DataTable = ft.AbrirDataTable(ds, "tblCajas", myConn, "select * from jsbanenccaj where id_emp = '" & jytsistema.WorkID & "' order by caja")
            ft.RellenaComboConDatatable(cmb, dtCajas, "nomcaja", "caja", Caja)
        End If
    End Sub

    Private Sub IniciarNombrePago(FormaPago As String, NombrePago As String, cmb As ComboBox)
        Select Case FormaPago
            Case "CH", "DP", "TR"
                Dim dtBancosInt As DataTable = ft.AbrirDataTable(ds, "tblBancosInt", myConn, " select codban, concat(codban , '-', nomban, '-', ctaban) banco from jsbancatban where " _
                                            & " id_emp = '" & jytsistema.WorkID & "' order by codban ")

                ft.RellenaComboConDatatable(cmb, dtBancosInt, "banco", "codban", NombrePago)
            Case Else
                If cmb.Items.Count > 0 Then
                    cmb.DataSource = Nothing
                    cmb.Items.Clear()
                End If
        End Select
    End Sub
    'Private Sub dgExP_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles 
    '    dgExP.CellMouseClick, dgExP.RegionChanged
    '    Me.BindingContext(ds, nTablaMovimientosExP).Position = e.RowIndex
    '    nPosicionMovExP = e.RowIndex
    '    AsignarTextosMovimientosExP(nPosicionMovExP)

    'End Sub

    Private Sub tbcProveedor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcProveedor.SelectedIndexChanged
        Select Case tbcProveedor.SelectedIndex
            Case 0 '' Catálogo
                AsignaTXT(nPosicionCat)
            Case 1 '' Movimientos
                AbrirMovimientos(txtCodigo.Text)
                'AsignaMov(nPosicionMov, False)
                dg.Enabled = True

            Case 2 '' Saldos por documento
                iSel = 0
                cmbSaldos.SelectedIndex = 0
                IniciarSaldosPorDocumento(0, txtCodigo.Text)
                MostrarItemsEnMenuBarra(MenuBarra, nPosicionCat, ds.Tables(nTabla).Rows.Count)
            Case 3 '' Estadísticas
                AsignaEstadistica()
            Case 4 '' Expediente
                AsignarExpediente(txtCodigo.Text)
            Case 5 '' Movimientos ExP
                AbrirMovimientosExP(txtCodigo.Text)
                'AsignaMovExP(nPosicionMovExP, False)
                dgExP.Enabled = True
            Case 6 'Envases
                AbrirEnvases(txtCodigo.Text)
        End Select

    End Sub
    Private Sub AbrirEnvases(ByVal CodigoProveedor As String)

        dgEnvases.DataSource = Nothing

        strSQLEnvases = " SELECT a.*, IF(b.nombre IS NULL, IF( c.nombre IS NULL, '', c.nombre) , b.nombre) nomProv_Cli, " _
            & " CONCAT(d.nombres, ' ' , d.apellidos) nomVendedor, elt(a.estatus + 1, " _
            & " 'Tránsito', 'Cliente', 'Proveedor', 'Vacío/Depósito', 'Lleno/Depósito', 'Reparación', 'Desincorporado', 'Indeterminado') nomEstatus " _
            & " from jsmertraenv a " _
            & " LEFT JOIN jsvencatcli b ON (a.prov_cli = b.codcli AND a.id_emp = b.id_emp AND a.origen IN ('FAC', 'PFC', 'NCV', 'NDV') ) " _
            & " LEFT JOIN jsprocatpro c ON (a.prov_cli = c.codpro AND a.id_emp = c.id_emp AND a.origen IN ('COM', 'REP', 'NCC' 'NDC') ) " _
            & " LEFT JOIN jsvencatven d ON (a.vendedor = d.codven AND a.id_emp = d.id_emp ) " _
            & " WHERE " _
            & " a.prov_cli = '" & CodigoProveedor & "' AND " _
            & " a.origen in ('COM', 'GAS', 'NCC', 'NDC', 'REC') and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY fechamov DESC "

        dtEnvases = ft.AbrirDataTable(ds, nTablaEnvases, myConn, strSQLEnvases)

        Dim aCampos() As String = {"fechamov.Emisión.80.C.fecha",
                                   "tipomov.TP.35.C.",
                                   "numdoc.Documento.100.I.",
                                   "almacen.ALM.50.C.",
                                   "cantidad.Cantidad.70.D.Entero",
                                   "origen.ORG.35.C.",
                                   "prov_cli.Prov/Clie.80.C.",
                                   "nomProv_cli.Nombre o razón social.300.I.",
                                   "vendedor.Asesor.50.C.",
                                   "nomvendedor.Nombre.300.I.",
                                   "nomEstatus.Estatus.200.I.",
                                   "sada..10.I."}

        ft.IniciarTablaPlus(dgEnvases, dtEnvases, aCampos)
        If dtEnvases.Rows.Count > 0 Then nPosicionEnv = 0

    End Sub
    Private Sub CalculaTotalesSaldos()

        Dim strSiSel As String = ""
        strSel = " nummov = 'XX XX' OR "
        iSel = 0
        dSel = 0
        For Each selectedItem As DataGridViewRow In dgSaldos.Rows
            If selectedItem.Cells(0).Value Then
                iSel += 1
                dSel += CDbl(selectedItem.Cells(6).Value)
                strSel += " nummov = '" & selectedItem.Cells(1).Value & "' OR "
            End If
        Next

        strSiSel = "(" & strSel.Substring(0, strSel.Length - 4) & ") and "

        AbrirDocumentosSaldo(strSiSel)
        AbrirMercanciasSaldo(strSiSel)
        txtDocSel.Text = ft.FormatoEntero(iSel)
        txtSaldoSel.Text = ft.FormatoNumero(dSel)

    End Sub
    Private Sub IniciarSaldosPorDocumento(ByVal ActualHistorico As Integer, ByVal CodProveedor As String)

        tblSaldos = "tbl" & ft.NumeroAleatorio(100000)

        txtDocSel.Text = ft.FormatoEntero(0)
        txtSaldoSel.Text = ft.FormatoNumero(0.0)


        If CodProveedor <> "" Then
            Dim aFields() As String = {"sel.entero.1.0", "codpro.cadena.15.0", "nombre.cadena.250.0", "nummov.cadena.20.0", "tipomov.cadena.2.0",
                                       "refer.cadena.15.0", "emision.fecha.0.0", "vence.fecha.0.0", "importe.doble.19.2", "saldo.doble.19.2",
                                       "codven.cadena.5.0", "nomVendedor.cadena.50.0"}

            CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblSaldos, aFields)


            ft.Ejecutar_strSQL(myConn, " INSERT INTO " & tblSaldos _
            & " select 0 sel, a.codpro codcli, b.nombre, a.nummov, a.tipomov, a.refer, a.emision, a.vence, a.importe, " _
            & " IF(c.saldo IS NULL, 0.00, c.saldo) saldo, '' codven, '' nomVendedor " _
            & " FROM jsprotrapag a  " _
            & " LEFT JOIN jsprocatpro b ON (a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN (SELECT codpro, nummov, IFNULL(SUM(IMPORTE),0) saldo " _
            & "         	FROM jsprotrapag WHERE codpro = '" & CodProveedor & "' AND id_emp = '" & jytsistema.WorkID & "' GROUP BY nummov HAVING ABS(ROUND(saldo,2)) > 0 ) c ON (a.codpro = c.codpro AND a.nummov = c.nummov ) " _
            & " INNER JOIN (SELECT codpro, nummov, MIN(CONCAT(fechasi, nummov,emision,hora)) minimo " _
            & "             FROM jsprotrapag WHERE historico = '" & ActualHistorico & "' AND ID_EMP = '" & jytsistema.WorkID & "' AND codpro = '" & txtCodigo.Text & "' GROUP BY nummov) d ON (CONCAT(a.fechasi,a.nummov,a.emision,a.hora) = d.minimo) " _
            & " WHERE " _
            & " a.historico = '" & ActualHistorico & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' AND " _
            & " a.codpro = '" & CodProveedor & "' " _
            & " ORDER BY a.fechasi, a.nummov, a.emision, a.hora ")


            ds = DataSetRequery(ds, " select * from " & tblSaldos, myConn, nTablaSaldos, lblInfo)
            dtSaldos = ds.Tables(nTablaSaldos)

            CargarListaSaldosDocumentosCliente(dgSaldos, dtSaldos)
            dgSaldos.ReadOnly = False
            For Each col As DataGridViewColumn In dgSaldos.Columns
                If col.Index > 0 Then col.ReadOnly = True
            Next

            dgDocumentos.Columns.Clear()
            dgMercasDocumentos.Columns.Clear()

            txtDocSel.Text = ft.FormatoEntero(0)
            txtSaldoSel.Text = ft.FormatoNumero(0.0)

            CalculaTotalesSaldos()

        End If
    End Sub
    Private Sub AbrirDocumentosSaldo(ByVal strDocs As String)
        Dim dtDocSal As DataTable
        Dim nTablaDocSal As String = "tbldocumentosSaldo"

        Dim strSQLDocsSaldo As String = " select a.emision, a.tipomov, a.nummov, a.vence, a.refer, a.formapag, a.comproba, a.importe, a.origen, " _
                    & " a.fotipo " _
                    & " from jsprotrapag a " _
                    & " where " _
                    & strDocs _
                    & " a.codpro = '" & txtCodigo.Text & "' and " _
                    & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " order by " _
                    & " a.NUMMOV, a.FOTIPO, a.TIPOMOV, a.EMISION"

        ds = DataSetRequery(ds, strSQLDocsSaldo, myConn, nTablaDocSal, lblInfo)
        dtDocSal = ds.Tables(nTablaDocSal)

        Dim aCampos() As String = {"emision", "tipomov", "nummov", "vence", "refer", "formapag", "comproba", "importe", "origen", "codven", "nomvendedor"}
        Dim aNombres() As String = {"Emisión", "TP", "Documento", "Vence", "Referencia", "FP", "Comprobante", "Importe", "ORG", "Asesor", "Nombre"}
        Dim aAnchos() As Integer = {80, 25, 90, 80, 80, 25, 90, 80, 35, 50, 80}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro,
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro,
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro,
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro,
                                        AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {sFormatoFecha, "", "", sFormatoFecha, "", "", "", sFormatoNumero, "", "", ""}
        IniciarTabla(dgDocumentos, dtDocSal, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, , , 8)

    End Sub
    Private Sub AbrirMercanciasSaldo(ByVal strDocs As String)

        Dim dtMerSal As DataTable
        Dim nTablaMerSal As String = "tblMercasSaldo"

        Dim strSQLMerSaldo As String = " SELECT a.numcom documento, a.item, a.descrip, a.iva, a.unidad, a.cantidad, a.costou precio, " _
            & " a.des_art, a.des_pro des_cli, 0.00 por_acepta_dev, a.costotot totren, ELT(a.estatus+1,'','Sin Descuento','Bonificación') tipo FROM jsprorencom a " _
            & " left join jsproenccom b on (a.numcom = b.numcom and a.codpro = b.codpro and a.id_emp = b.id_emp) " _
            & " WHERE " _
            & Replace(strDocs, "nummov", "a.numcom") _
            & " SUBSTRING(a.item,1,1) <> '$' AND " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
            & " union " _
            & " select a.numncr documento, a.item, a.descrip, a.iva, a.unidad, a.cantidad, a.precio, " _
            & " a.des_art, a.des_pro des_cli, a.por_acepta_dev, a.totren, elt(a.estatus+1,'','Sin Descuento','Bonificación') tipo from jsprorenncr a " _
            & " left join jsproencncr b on (a.numncr = b.numncr and a.codpro = b.codpro and a.id_emp = b.id_emp) " _
            & " where " _
            & Replace(strDocs, "nummov", "a.numncr") _
            & " substring(a.item,1,1) <> '$' and " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
            & " union " _
            & " select a.numndb documento, a.item, a.descrip, a.iva, a.unidad, a.cantidad, a.costo precio, " _
            & " a.des_art, a.des_pro des_cli, 0.00 por_acepta_dev, a.totren, elt(a.estatus+1,'','Sin Descuento','Bonificación') tipo from jsprorenndb a " _
            & " left join jsproencndb b on (a.numndb = b.numndb and a.codpro = b.codpro and a.id_emp = b.id_emp) " _
            & " where " _
            & Replace(strDocs, "nummov", "a.numndb") _
            & " substring(a.item,1,1) <> '$' and " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' order by 1, 2 "

        ds = DataSetRequery(ds, strSQLMerSaldo, myConn, nTablaMerSal, lblInfo)
        dtMerSal = ds.Tables(nTablaMerSal)

        Dim aCampos() As String = {"documento", "item", "descrip", "iva", "unidad", "cantidad", "precio", "des_art", "des_cli", "por_acepta_dev", "totren"}
        Dim aNombres() As String = {"Documento", "ítem", "Descripción", "IVA", "UND", "Cantidad", "Precio", "Desc. Art.", "Desc. Pro.", "% Acepta.", "Total"}
        Dim aAnchos() As Integer = {90, 70, 190, 30, 35, 70, 70, 45, 45, 50, 70}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda,
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro,
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {"", "", "", "", "", sFormatoCantidad, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero}
        IniciarTabla(dgMercasDocumentos, dtMerSal, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, , , 8)

    End Sub
    Private Sub AsignaEstadistica()
        rBtn1.Checked = True
        ft.RellenaCombo(aTipoEstadistica, cmbTipoEstadistica)
    End Sub
    Private Sub AbrirEstadisticas()

        EsperaPorFavor()

        Dim aCam() As String = {"codart", "nomart", "unidad", "mEne", "mFeb", "mMar", "mAbr", "mMay", "mJun", "mJul", "mAgo", "mSep", "mOct", "mNov", "mDic"}
        Dim aNom() As String = {"Código", "Nombre artículo", "UND", "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"}
        Dim aAnc() As Integer = {70, 280, 35, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}

        Dim aFor() As String = {"", "", "", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero}

        Dim dtStat As New DataTable
        dtStat = ConsultaEstadistica(myConn, ds, lblInfo, txtCodigo.Text, "COM",
                cmbTipoEstadistica.SelectedIndex, IIf(rBtn1.Checked, TipoDatoMercancia.iUnidadesDeVenta, IIf(rBtn2.Checked, TipoDatoMercancia.iKilogramos, TipoDatoMercancia.iMonetarios)), jytsistema.sFechadeTrabajo, "tblMovimientosAnoActualCompras")

        IniciarTabla(dgEstadistica, dtStat, aCam, aNom, aAnc, aAli, aFor)

        VerHistograma(c1Chart1, IIf(rBtn1.Checked, TipoDatoMercancia.iUnidadesDeVenta, IIf(rBtn2.Checked, TipoDatoMercancia.iKilogramos, TipoDatoMercancia.iMonetarios)))

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Select Case tbcProveedor.SelectedTab.Text
            Case "Proveedor"

                If dt.Rows.Count = 0 Then
                    IniciarProveedor(False)
                Else
                    Me.BindingContext(ds, nTabla).CancelCurrentEdit()
                    If Me.BindingContext(ds, nTabla).Position > 0 Then _
                        nPosicionCat = Me.BindingContext(ds, nTabla).Position

                    AsignaTXT(nPosicionCat)
                End If
                DesactivarMarco0()
        End Select
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Select Case tbcProveedor.SelectedTab.Text
            Case "Proveedor"
                If Validado() Then
                    GuardarTXT()
                End If
        End Select
    End Sub

    Private Sub txtCredito_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCredito.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub cmbIVA_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbIVA.SelectedIndexChanged
        txtIVA.Text = ft.FormatoNumero(PorcentajeIVA(myConn, lblInfo, jytsistema.sFechadeTrabajo, cmbIVA.Text)) & "%"
    End Sub

    Private Sub txtUnidad_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUnidad.TextChanged
        If txtUnidad.Text.Trim <> "" Then txtUnidadNombre.Text = ft.DevuelveScalarCadena(myConn, " select descrip from jsprolisuni " _
                                                                                    + " where " _
                                                                                    + " codigo = '" + txtUnidad.Text + "' and " _
                                                                                    + " id_emp = '" + jytsistema.WorkID + "' ")
    End Sub

    Private Sub txtCategoria_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCategoria.TextChanged
        If txtCategoria.Text.Trim <> "" Then txtCategoriaNombre.Text = ft.DevuelveScalarCadena(myConn, " select descrip from jsproliscat " _
                                                                                     + " where " _
                                                                                     + " codigo = '" & txtCategoria.Text & "' and " _
                                                                                     + " antec = '" & txtUnidad.Text & "' and " _
                                                                                     + " id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub txtZona_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtZona.TextChanged
        Dim aFld() As String = {"codigo", "modulo", "id_emp"}
        Dim aStr() As String = {txtZona.Text, FormatoTablaSimple(Modulo.iZonaProveedor), jytsistema.WorkID}
        txtZonaNombre.Text = qFoundAndSign(myConn, lblInfo, "jsconctatab", aFld, aStr, "descrip")
    End Sub

    Private Sub txtFormaPago_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFormaPago.TextChanged
        Dim aFld() As String = {"codfor", "id_emp"}
        Dim aStr() As String = {txtFormaPago.Text, jytsistema.WorkID}
        txtFormaPagoNombre.Text = qFoundAndSign(myConn, lblInfo, "jsconctafor", aFld, aStr, "nomfor")
    End Sub

    Private Sub txtBancoPago_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBancoPago.TextChanged
        Dim aFld() As String = {"codban", "id_emp"}
        Dim aStr() As String = {txtBancoPago.Text, jytsistema.WorkID}
        txtBancoPagoNombre.Text = qFoundAndSign(myConn, lblInfo, "jsbancatban", aFld, aStr, "nomban")
        txtBancoPagoCuenta.Text = qFoundAndSign(myConn, lblInfo, "jsbancatban", aFld, aStr, "ctaban")
    End Sub

    Private Sub txtBancoDep1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBancoDep1.TextChanged
        Dim aFld() As String = {"codigo", "modulo", "id_emp"}
        Dim aStr() As String = {txtBancoDep1.Text, FormatoTablaSimple(Modulo.iBancos), jytsistema.WorkID}
        txtBancoDep1Nombre.Text = qFoundAndSign(myConn, lblInfo, "jsconctatab", aFld, aStr, "descrip")
    End Sub

    Private Sub txtBancoDep2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBancoDep2.TextChanged
        Dim aFld() As String = {"codigo", "modulo", "id_emp"}
        Dim aStr() As String = {txtBancoDep2.Text, FormatoTablaSimple(Modulo.iBancos), jytsistema.WorkID}
        txtBancoDep2Nombre.Text = qFoundAndSign(myConn, lblInfo, "jsconctatab", aFld, aStr, "descrip")
    End Sub

    Private Sub cmbTipoEstadistica_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTipoEstadistica.SelectedIndexChanged
        If cmbTipoEstadistica.Items.Count = 2 Then AbrirEstadisticas()
    End Sub
    Private Sub VerHistograma(ByVal Histograma As C1Chart, ByVal CantidadKilosMoney As Integer)

        Dim Area As Area = Histograma.ChartArea
        Dim ax As Axis = Area.AxisX
        Dim ay As Axis = Area.AxisY

        ax.ValueLabels.Clear()
        ax.ValueLabels.Add(1, "Ene")
        ax.ValueLabels.Add(2, "Feb")
        ax.ValueLabels.Add(3, "Mar")
        ax.ValueLabels.Add(4, "Abr")
        ax.ValueLabels.Add(5, "May")
        ax.ValueLabels.Add(6, "Jun")
        ax.ValueLabels.Add(7, "Jul")
        ax.ValueLabels.Add(8, "Ago")
        ax.ValueLabels.Add(9, "Sep")
        ax.ValueLabels.Add(10, "Oct")
        ax.ValueLabels.Add(11, "Nov")
        ax.ValueLabels.Add(12, "Dic")

        Dim aaY As Double() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim abY As Double() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Histograma.ChartGroups(0).ChartData.SeriesList(0).Y.CopyDataIn(aaY)
        Histograma.ChartGroups(0).ChartData.SeriesList(1).Y.CopyDataIn(abY)

        Dim dtt As New DataTable
        dtt = ConsultaEstadistica(myConn, ds, lblInfo, txtCodigo.Text, "COM", cmbTipoEstadistica.SelectedIndex, IIf(rBtn1.Checked, TipoDatoMercancia.iUnidadesDeVenta, IIf(rBtn2.Checked, TipoDatoMercancia.iKilogramos, TipoDatoMercancia.iMonetarios)), jytsistema.sFechadeTrabajo,
                                  "tblResumenAnoActualCompras", " a.id_emp ")

        Dim dttAnteriores As New DataTable
        dttAnteriores = ConsultaEstadistica(myConn, ds, lblInfo, txtCodigo.Text, "COM", cmbTipoEstadistica.SelectedIndex, IIf(rBtn1.Checked, TipoDatoMercancia.iUnidadesDeVenta, IIf(rBtn2.Checked, TipoDatoMercancia.iKilogramos, TipoDatoMercancia.iMonetarios)), DateAdd(DateInterval.Year, -1, jytsistema.sFechadeTrabajo),
                                            "tblResumenAnoAnteriorCompras", " a.id_emp ")

        aaY = ValoresMensuales(dtt)
        abY = ValoresMensuales(dttAnteriores)

        Dim aFFld() As String = {"id_emp"}
        Dim aSStr() As String = {jytsistema.WorkID}

        ay.Text = IIf(CantidadKilosMoney = 0, "Unidad de Venta", IIf(CantidadKilosMoney = 1, "Kilogramos", qFoundAndSign(myConn, lblInfo, "jsconctaemp", aFFld, aSStr, "moneda")))
        ax.Text = cmbTipoEstadistica.Text & " mes a mes"

        Histograma.ChartGroups(0).ChartData.SeriesList(0).Y.CopyDataIn(aaY)
        Histograma.ChartGroups(0).ChartData.SeriesList(1).Y.CopyDataIn(abY)

        Histograma.ChartGroups(0).ChartData.SeriesList(0).Label = Year(jytsistema.sFechadeTrabajo)
        Histograma.ChartGroups(0).ChartData.SeriesList(1).Label = Year(jytsistema.sFechadeTrabajo) - 1

        'Histograma.ChartGroups(0).ChartData.SeriesList(0).Y.

        dtt = Nothing
        dttAnteriores = Nothing


    End Sub
    Private Function TablaCompras(ByVal nProveedor As String, ByVal CantidadKilosMoney As Integer, Optional ByVal AñosAtras As Integer = 0) As DataTable
        Dim strCantidad As String, strKilos As String, strMoney As String

        strCantidad = "if( a.origen in ('COM','NDC'), if( a.tipomov <> 'AC', a.cantidad , 0 )  , if( a.tipomov <> 'AC', -1*a.cantidad ,0) ) "
        strKilos = "if( a.origen in ('COM','NDC'), if( a.tipomov <> 'AC', a.peso , 0 )  , if( a.tipomov <> 'AC', -1*a.peso ,0) ) "
        strMoney = "if( a.origen in ('COM','NDC'),  a.costotaldes   ,  -1*a.costotaldes ) "

        Dim nTablaCompras As String = "tablacompras" + AñosAtras.ToString
        Dim strSQLCompras As String
        Select Case CantidadKilosMoney
            Case 0 ' Cantidad
                strSQLCompras = " select a.prov_cli, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 1, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 1, " & strCantidad & "/EQUIVALE,0))) AS ENE, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 2, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 2, " & strCantidad & "/EQUIVALE,0))) AS FEB, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 3, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 3, " & strCantidad & "/EQUIVALE,0))) AS MAR, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 4, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 4, " & strCantidad & "/EQUIVALE,0))) AS ABR, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 5, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 5, " & strCantidad & "/EQUIVALE,0))) AS MAY, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 6, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 6, " & strCantidad & "/EQUIVALE,0))) AS JUN, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 7, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 7, " & strCantidad & "/EQUIVALE,0))) AS JUL, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 8, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 8, " & strCantidad & "/EQUIVALE,0))) AS AGO, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 9, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 9, " & strCantidad & "/EQUIVALE,0))) AS SEP, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 10, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 10, " & strCantidad & "/EQUIVALE,0))) AS OCT, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 11, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 11, " & strCantidad & "/EQUIVALE,0))) AS NOV, " _
                & " SUM(IF( ISNULL(UVALENCIA), IF( MONTH(a.FECHAMOV) = 12, " & strCantidad & ",0), IF( MONTH(a.FECHAMOV) = 12, " & strCantidad & "/EQUIVALE,0))) AS DIC " _
                & " from " _
                & " jsmertramer a " _
                & " LEFT JOIN jsmerequmer b ON (a.CODART = b.CODART AND a.UNIDAD = b.UVALENCIA) " _
                & " WHERE " _
                & " a.prov_cli = '" & nProveedor & "' AND " _
                & " a.origen in ('COM','NDC','NCC')  AND " _
                & " year(a.fechamov) = " & Year(jytsistema.sFechadeTrabajo) - AñosAtras & " AND " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                & " group by a.prov_cli "
            Case 1 ' Kilos
                strSQLCompras = " select a.prov_cli, " _
                                & " SUM(IF(MONTH(a.FECHAMOV) = 1, " & strKilos & ", 0)) AS ENE, SUM(IF(MONTH(a.FECHAMOV) = 2, " & strKilos & ", 0)) AS FEB, " _
                                & " SUM(IF(MONTH(a.FECHAMOV) = 3, " & strKilos & ", 0)) AS MAR, SUM(IF(MONTH(a.FECHAMOV) = 4, " & strKilos & ", 0)) AS ABR, " _
                                & " SUM(IF(MONTH(a.FECHAMOV) = 5, " & strKilos & ", 0)) AS MAY, SUM(IF(MONTH(a.FECHAMOV) = 6, " & strKilos & ", 0)) AS JUN, " _
                                & " SUM(IF(MONTH(a.FECHAMOV) = 7, " & strKilos & ", 0)) AS JUL, SUM(IF(MONTH(a.FECHAMOV) = 8, " & strKilos & ", 0)) AS AGO, " _
                                & " SUM(IF(MONTH(a.FECHAMOV) = 9, " & strKilos & ", 0)) AS SEP, SUM(IF(MONTH(a.FECHAMOV) = 10, " & strKilos & ", 0)) AS OCT, " _
                                & " SUM(IF(MONTH(a.FECHAMOV) = 11, " & strKilos & ", 0)) AS NOV, SUM(IF(MONTH(a.FECHAMOV) = 12, " & strKilos & ", 0)) AS DIC " _
                                & " from jsmertramer a where " _
                                & " a.origen in ('COM','NDC','NCC') AND " _
                                & " year(a.fechamov) = " & Year(jytsistema.sFechadeTrabajo) - AñosAtras & " AND " _
                                & " a.prov_cli = '" & nProveedor & "' AND a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                & " GROUP BY a.prov_cli  "
            Case Else ' Money
                strSQLCompras = "select a.prov_cli, " _
                                  & "SUM(IF(MONTH(a.FECHAMOV) = 1, " & strMoney & ", 0)) AS ENE, SUM(IF(MONTH(a.FECHAMOV) = 2, " & strMoney & ", 0)) AS FEB, " _
                                  & "SUM(IF(MONTH(a.FECHAMOV) = 3, " & strMoney & ", 0)) AS MAR, SUM(IF(MONTH(a.FECHAMOV) = 4, " & strMoney & ", 0)) AS ABR, " _
                                  & "SUM(IF(MONTH(a.FECHAMOV) = 5, " & strMoney & ", 0)) AS MAY, SUM(IF(MONTH(a.FECHAMOV) = 6, " & strMoney & ", 0)) AS JUN, " _
                                  & "SUM(IF(MONTH(a.FECHAMOV) = 7, " & strMoney & ", 0)) AS JUL, SUM(IF(MONTH(a.FECHAMOV) = 8, " & strMoney & ", 0)) AS AGO, " _
                                  & "SUM(IF(MONTH(a.FECHAMOV) = 9, " & strMoney & ", 0)) AS SEP, SUM(IF(MONTH(a.FECHAMOV) = 10, " & strMoney & ", 0)) AS OCT, " _
                                  & "SUM(IF(MONTH(a.FECHAMOV) = 11, " & strMoney & ", 0)) AS NOV, SUM(IF(MONTH(a.FECHAMOV) = 12, " & strMoney & ", 0)) AS DIC " _
                                  & "from jsmertramer a where " _
                                  & " a.origen in ('COM','NDC','NCC') AND " _
                                  & " year(a.fechamov) = " & Year(jytsistema.sFechadeTrabajo) - AñosAtras & " AND " _
                                  & " a.prov_cli = '" & nProveedor & "' AND a.ID_EMP = '" & jytsistema.WorkID & "' " _
                                  & " GROUP BY a.prov_cli "
        End Select

        ds = DataSetRequery(ds, strSQLCompras, myConn, nTablaCompras, lblInfo)
        TablaCompras = ds.Tables(nTablaCompras)

    End Function
    Private Function ValoresMensuales(ByVal dtValores As DataTable) As Double()
        Dim aMes As Double() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        If dtValores.Rows.Count > 0 Then
            With dtValores.Rows(0)
                aMes(0) = CDbl(.Item("mENE"))
                aMes(1) = CDbl(.Item("mFEB"))
                aMes(2) = CDbl(.Item("mMAR"))
                aMes(3) = CDbl(.Item("mABR"))
                aMes(4) = CDbl(.Item("mMAY"))
                aMes(5) = CDbl(.Item("mJUN"))
                aMes(6) = CDbl(.Item("mJUL"))
                aMes(7) = CDbl(.Item("mAGO"))
                aMes(8) = CDbl(.Item("mSEP"))
                aMes(9) = CDbl(.Item("mOCT"))
                aMes(10) = CDbl(.Item("mNOV"))
                aMes(11) = CDbl(.Item("mDIC"))
            End With
        End If
        ValoresMensuales = aMes

    End Function

    Private Sub rBtn1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rBtn1.CheckedChanged,
       rBtn2.CheckedChanged, rBtn3.CheckedChanged
        If cmbTipoEstadistica.Items.Count = 2 AndAlso sender.Checked Then AbrirEstadisticas()
    End Sub

    Private Sub AsignarExpediente(ByVal nCodigoProveedor As String)

        Dim dtExp As DataTable
        Dim nTablaExp As String = "tblExpediente"
        Dim strSQLExp As String = " SELECT a.codpro, a.fecha, a.COMENTARIO, a.CONDICION, " _
                    & " ELT(a.condicion + 1, 'Activo', 'Bloqueado', 'Inactivo', 'Desincorporado') estatus, " _
                    & " IF(a.causa = '', 'NOTA DE USUARIO',  " _
                    & " IF( a.condicion = 0,  b.descrip, IF( a.condicion = 1, c.descrip, IF( a.condicion = 2, d.descrip, e.descrip ) ) )) descripcion, a.tipocondicion  " _
                    & " FROM jsproexppro a " _
                    & " LEFT JOIN jsconctatab b ON (a.causa = b.codigo AND a.id_emp = b.id_emp AND b.modulo = '00017') " _
                    & " LEFT JOIN jsconctatab c ON (a.causa = c.codigo AND a.id_emp = c.id_emp AND c.modulo = '00018') " _
                    & " LEFT JOIN jsconctatab d ON (a.causa = d.codigo AND a.id_emp = d.id_emp AND d.modulo = '00019') " _
                    & " LEFT JOIN jsconctatab e ON (a.causa = e.codigo AND a.id_emp = e.id_emp AND e.modulo = '00020') " _
                    & " WHERE " _
                    & " a.codpro = '" & nCodigoProveedor & "' AND " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " ORDER BY a.fecha DESC "

        ds = DataSetRequery(ds, strSQLExp, myConn, nTablaExp, lblInfo)
        dtExp = ds.Tables(nTablaExp)

        Dim aCamExp() As String = {"fecha", "descripcion", "estatus", "comentario"}
        Dim aNomExp() As String = {"Fecha", "Descripción causa", "Condición", "Comentario"}
        Dim aAncexp() As Integer = {140, 240, 100, 350}
        Dim aAliExp() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aForExp() As String = {sFormatoFechaCorta, "", "", ""}

        IniciarTabla(dgExpediente, dtExp, aCamExp, aNomExp, aAncexp, aAliExp, aForExp)


    End Sub


    Private Sub btnDep2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDep2.Click

        txtBancoDep2.Text = CargarTablaSimplePlusReal("BANCOS DE LA PLAZA", Modulo.iBancos)

    End Sub

    Private Sub btnDep1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBancoDeposito.Click

        txtBancoDep1.Text = CargarTablaSimplePlusReal("BANCOS DE LA PLAZA", Modulo.iBancos)

    End Sub

    Private Sub btnBanco_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBanco.Click

        txtBancoPago.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codban codigo, nomban descripcion " _
                                              & " from jsbancatban " _
                                              & " where " _
                                              & " estatus = '1' and " _
                                              & " id_emp = '" & jytsistema.WorkID & "' order by codban ", "BANCOS DE LA EMPRESA",
                                              txtBancoPago.Text)

    End Sub

    Private Sub btnForma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForma.Click

        txtFormaPago.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codfor codigo, nomfor descripcion from jsconctafor where id_emp = '" & jytsistema.WorkID & "' order by codfor ",
                                              "FORMA DE PAGO", txtFormaPago.Text)


    End Sub

    Private Sub btnUnidad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnidad.Click
        Dim f As New jsComArcUnidadCategoria
        f.Grupo0 = txtUnidad.Text
        f.Grupo1 = txtCategoria.Text
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        txtUnidad.Text = ""
        If f.Grupo0 <> "" Then txtUnidad.Text = f.Grupo0
        txtCategoria.Text = ""
        If f.Grupo1 <> "" Then txtCategoria.Text = f.Grupo1
        f = Nothing
    End Sub

    Private Sub btnZona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZona.Click

        txtZona.Text = CargarTablaSimplePlusReal("ZONAS PROVEEDORES", Modulo.iZonaProveedor)

    End Sub

    Private Sub btnSubirHistorico_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubirHistorico.Click
        Dim nTab As Integer = tbcProveedor.SelectedIndex
        If nTab = 1 Or nTab = 5 Then
            Dim f As New jsComProHistoricoProveedores
            f.Cargar(myConn, iProceso.Procesar, txtCodigo1.Text, IIf(nTab = 1, 0, 1))
            f = Nothing
            If nTab = 1 Then
                AbrirMovimientos(txtCodigo1.Text)
            Else
                AbrirMovimientosExP(txtCodigoExP.Text)
            End If

        End If

    End Sub
    Private Sub btnBajarHistorico_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBajarHistorico.Click
        Dim nTab As Integer = tbcProveedor.SelectedIndex
        If nTab = 1 Or nTab = 5 Then
            Dim f As New jsComProHistoricoProveedores
            f.Cargar(myConn, iProceso.Reversar, txtCodigo1.Text, IIf(nTab = 1, 0, 1))
            f = Nothing
            If nTab = 1 Then
                AbrirMovimientos(txtCodigo1.Text)
            Else
                AbrirMovimientosExP(txtCodigoExP.Text)
            End If

        End If
    End Sub

    Private Sub cmbSaldos_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbSaldos.SelectedIndexChanged
        IniciarSaldosPorDocumento(cmbSaldos.SelectedIndex, txtCodigo.Text)
    End Sub


    Private Sub c1Chart1_ShowTooltip(ByVal sender As Object, ByVal e As C1.Win.C1Chart.ShowTooltipEventArgs) Handles c1Chart1.ShowTooltip
        If TypeOf sender Is ChartDataSeries Then
            ' Create new tooltip text
            'If c1Chart1.ToolTip.PlotElement = PlotElementEnum.Series Then
            Dim ds As ChartDataSeries = CType(sender, ChartDataSeries)

            Dim p As Point = Control.MousePosition
            p = c1Chart1.PointToClient(p)

            Dim x As Double = 0
            Dim y As Double = 0

            ' Callculate data coordinates
            Dim aNom() As String = {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"}
            If ds.Group.CoordToDataCoord(p.X, p.Y, x, y) Then
                e.TooltipText = String.Format("{0}" + ControlChars.Lf + "Mes = " + aNom(Math.Round(x) - 1) +
                                              ControlChars.Lf + "Valor = {2:#.##}", ds.Label, x, y)
            Else
                e.TooltipText = ""
            End If
            'End If
        End If
    End Sub

    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Select Case e.KeyCode
        '    Case Keys.Down
        '        Me.BindingContext(ds, nTablaMovimientos).Position += 1
        '        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        '        AsignaMov(nPosicionMov, False)
        '    Case Keys.Up
        '        Me.BindingContext(ds, nTablaMovimientos).Position -= 1
        '        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        '        AsignaMov(nPosicionMov, False)
        'End Select
    End Sub

    Private Sub dgExP_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'Select Case e.KeyCode
        '    Case Keys.Down
        '        Me.BindingContext(ds, nTablaMovimientosExP).Position += 1
        '        nPosicionMovExP = Me.BindingContext(ds, nTablaMovimientosExP).Position
        '        AsignaMovExP(nPosicionMovExP, False)
        '    Case Keys.Up
        '        Me.BindingContext(ds, nTablaMovimientosExP).Position -= 1
        '        nPosicionMovExP = Me.BindingContext(ds, nTablaMovimientosExP).Position
        '        AsignaMovExP(nPosicionMovExP, False)
        'End Select
    End Sub


    Private Sub dgSaldos_CellMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgSaldos.CellMouseUp
        CalculaTotalesSaldos()
    End Sub

    Private Sub dgSaldos_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgSaldos.CellContentClick
        If e.ColumnIndex = 0 Then
            dtSaldos.Rows(e.RowIndex).Item(0) = Not CBool(dtSaldos.Rows(e.RowIndex).Item(0).ToString)
        End If
    End Sub

    Private Sub dgSaldos_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgSaldos.CellValidated
        If dgSaldos.CurrentCell.ColumnIndex = 0 Then

            With dgSaldos.CurrentRow
                ft.Ejecutar_strSQL(myConn, " update  " & tblSaldos & " set sel  = " & CInt(dgSaldos.CurrentCell.Value) & " " _
                                    & " where " _
                                    & " emision = '" & ft.FormatoFechaMySQL(CDate(.Cells(3).Value.ToString)) & "' and " _
                                    & " vence = '" & ft.FormatoFechaMySQL(CDate(.Cells(4).Value.ToString)) & "' and " _
                                    & " tipomov = '" & CStr(.Cells(2).Value) & "' and " _
                                    & " nummov = '" & CStr(.Cells(1).Value) & "' ")
            End With

        End If

    End Sub

    Private Sub btnCodigoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigoContable.Click
        txtCodigoContable.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables",
                                                   txtCodigoContable.Text)
    End Sub

    Private Sub btnRIF_Click(sender As System.Object, e As System.EventArgs) Handles btnRIF.Click

        Dim f As New jsGenConsultaRIF
        f.DireccionURL = "http://contribuyente.seniat.gob.ve/BuscaRif/BuscaRif.jsp"
        f.RIF = txtRIF.Text.Replace("-", "")
        f.Cargar(myConn, "CONSULTA RIF", TipoConsultaWEB.iRIF)
        If f.NombreEmpresa.Trim <> "" Then txtNombre.Text = f.NombreEmpresa
        f.Dispose()
        f = Nothing

    End Sub

    Private Sub btnReprocesar_Click(sender As System.Object, e As System.EventArgs) Handles btnReprocesar.Click
        'Select Case tbcProveedor.SelectedTab.Text
        '    Case "Movimientos CxP"
        '        If LoginUser(myConn, lblInfo) = "jytsuite" Then
        '            With dtMovimientos.Rows(nPosicionMov)
        '                Select Case .Item("FORMAPAG")
        '                    Case "EF"
        '                        InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, .Item("CAJAPAG"), UltimoCajaMasUno(myConn, lblInfo, .Item("CAJAPAG")),
        '                            CDate(.Item("EMISION").ToString), "CXP", "SA", .Item("COMPROBA"), .Item("FORMAPAG"),
        '                            .Item("NUMPAG"), .Item("NOMPAG"), -1 * ValorNumero(.Item("IMPORTE")), .Item("CODCON"), .Item("CONCEPTO"), "", jytsistema.sFechadeTrabajo, 1,
        '                            "", "", "", jytsistema.sFechadeTrabajo, .Item("CODPRO"), "", "1", jytsistema.WorkCurrency.Id, DateTime.Now())
        '                    Case "CH", "TA", "DP", "TR"
        '                        InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, CDate(.Item("EMISION").ToString), .Item("NUMPAG"),
        '                            IIf(.Item("FORMAPAG") <> "CH", "ND", "CH"), .Item("NOMPAG"), "", .Item("CONCEPTO"), -1 * ValorNumero(.Item("IMPORTE")),
        '                            "CXP", .Item("COMPROBA"), .Item("BENEFIC"), .Item("COMPROBA"), "0", CDate(.Item("EMISION").ToString), CDate(.Item("EMISION").ToString),
        '                             .Item("TIPDOCCAN"), "", jytsistema.sFechadeTrabajo, "0", .Item("CODPRO"), "", jytsistema.WorkCurrency.Id, DateTime.Now())
        '                End Select
        '            End With
        '        End If
        'End Select
    End Sub

    Private Sub btnExP_Click(sender As System.Object, e As System.EventArgs) Handles btnExP.Click
        'Dim nPos As Long
        'Select Case tbcProveedor.SelectedIndex
        '    Case 1
        '        If nPosicionMov >= 0 Then

        '            nPos = nPosicionMov
        '            ft.Ejecutar_strSQL(myConn, " update jsprotrapag set remesa = '1' " _
        '                & " where  " _
        '                & " codpro = '" & txtCodigo.Text & "' and " _
        '                & " nummov = '" & dtMovimientos.Rows(nPosicionMov).Item("nummov") & "' and " _
        '                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
        '                & " id_emp = '" & jytsistema.WorkID & "' ")

        '            AbrirMovimientos(txtCodigo.Text)
        '            nPosicionMov = IIf(nPos > dtMovimientos.Rows.Count - 1, dtMovimientos.Rows.Count - 1, nPos)
        '            AsignaMov(nPosicionMov, False)


        '        End If

        '    Case 5
        '        If nPosicionMovExP >= 0 Then

        '            nPos = nPosicionMovExP
        '            ft.Ejecutar_strSQL(myConn, " update jsprotrapag set remesa = '' " _
        '                & " where  " _
        '                & " codpro = '" & txtCodigo.Text & "' and " _
        '                & " nummov = '" & dtMovimientosExP.Rows(nPosicionMovExP).Item("nummov") & "' and " _
        '                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
        '                & " id_emp = '" & jytsistema.WorkID & "' ")

        '            AbrirMovimientosExP(txtCodigo.Text)
        '            nPosicionMovExP = IIf(nPos > dtMovimientosExP.Rows.Count - 1, dtMovimientosExP.Rows.Count - 1, nPos)
        '            AsignaMovExP(nPosicionMovExP, False)

        '        End If


        'End Select

    End Sub


    Private Sub btnAuditoria_Click(sender As System.Object, e As System.EventArgs) Handles btnAuditoria.Click
        If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) > 0 Then
            Dim f As New jsControlArcAccesosPlus
            f.nModulo = sModulo + " - " + tbcProveedor.SelectedTab.Text
            f.Cargar()
            f.Dispose()
            f = Nothing

        End If
    End Sub

    Private Sub dg_DataSourceChanged(sender As Object, e As Syncfusion.WinForms.DataGrid.Events.DataSourceChangedEventArgs) Handles _
            dg.DataSourceChanged, dgExP.DataSourceChanged
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub dg_CellDoubleClick(sender As Object, e As Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs) Handles dg.CellDoubleClick
        'If txtCodigo.Text.Trim <> "" And dg.RowCount > 0 Then
        '    If vendorTransactionsList.Item(nPosicionMov).Origen = "CXP" Then
        '        ActivarMarco1()
        '    End If
        'End If
    End Sub

    Private Sub dg_SelectionChanged(sender As Object, e As Syncfusion.WinForms.DataGrid.Events.SelectionChangedEventArgs) Handles dg.SelectionChanged
        cxpSelected = dg.SelectedItem
        nPosicionMov = dg.SelectedIndex
    End Sub
End Class