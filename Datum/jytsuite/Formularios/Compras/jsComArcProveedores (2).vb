Imports MySql.Data.MySqlClient
Imports C1.Win.C1Chart
Public Class jsComArcProveedores

    Private Const sModulo As String = "Proveedores y CxP"
    Private Const lRegion As String = "RibbonButton55"
    Private Const nTabla As String = "tblProveedores"
    Private Const nTablaMovimientos As String = "tblMovimientosProveedor"
    Private Const nTablaMovimientosExP As String = "tblMovimientosProveedorExP"
    Private Const nTablaSaldos As String = "tblSaldosXDocumento"

    Private strSQL As String = "select * from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by codpro "
    Private strSQLMov As String = ""
    Private strSQLMovExP As String = ""
    Private strSQLSaldos As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtMovimientos As New DataTable
    Private dtMovimientosExP As New DataTable
    Private dtSaldos As New DataTable

    Private aIVA() As String
    Private aCondicion() As String = {"Activo", "Inactivo"}
    Private aTipo() As String = {"Compras", "Gastos", "Compras/Gastos", "Otros"}
    Private aTipoEstadistica() As String = {"Compras", "Devoluciones"}
    Private aSaldos() As String = {"Actuales", "Históricos"}

    Private i_modo As Integer
    Private nPosicionCat As Long, nPosicionMov As Long, nPosicionEst As Long, nPosicionMovExP As Long

    Private iSel As Integer = 0
    Private dSel As Double = 0.0
    Private strSel As String = " nummov = 'XX XX' OR "


    Private tblSaldos As String = ""

    Private Sub jsComArcProveedores_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo

        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)

            RellenaCombo(aCondicion, cmbCondicion)
            RellenaCombo(aSaldos, cmbSaldos)

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaTXT(nPosicionCat)
            Else
                IniciarProveedor(False)
            End If
            ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            tbcProveedor.SelectedTab = C1DockingTabPage1
            AsignarTooltips()
        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo registro")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> registro deseado")
        C1SuperTooltip1.SetToolTip(btnSeleccionar, "<B>Seleccionar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir al <B>primer</B> registro")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir al <B>siguiente</B> registro")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir al registro <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir al <B>último registro</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B>")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")

        C1SuperTooltip1.SetToolTip(btnSubirHistorico, "<B>Subir</B> movimientos cancelados a histórico")
        C1SuperTooltip1.SetToolTip(btnBajarHistorico, "<B>Bajar</B> movimientos desde histórico")

        C1SuperTooltip1.SetToolTip(btnReprocesar, "<B>Refrecar</B> información de esta pantalla")
        C1SuperTooltip1.SetToolTip(btnExP, "<B>Transferir</B> CxP a ExP/ExP a CxP")
        C1SuperTooltip1.SetToolTip(btnAuditoria, "Consulta <B>Auditoría</B> de este módulo")

        'Adicionbales menu barra
        'Botones Adicionales
        C1SuperTooltip1.SetToolTip(btnIngreso, "Seleccione la fecha de ingreso ó<br> la fecha de activación del proveedor en el sistema")
        C1SuperTooltip1.SetToolTip(btnUnidad, "Seleccione la unidad de negocio y<br> la categoría del proveedor")
        C1SuperTooltip1.SetToolTip(btnZona, "Seleccione la zona del proveedor")
        C1SuperTooltip1.SetToolTip(btnForma, "Seleccione la forma de pago del proveedor")
        C1SuperTooltip1.SetToolTip(btnBanco, "Seleccione el banco con el cual se le pagará<br> a este proveedor")
        C1SuperTooltip1.SetToolTip(btnDep1, "Seleccione el banco 1 donde se le depositará<br> a este proveedor")
        C1SuperTooltip1.SetToolTip(btnDep2, "Seleccione el banco 1 donde se le depositará<br> a este proveedor")

        c1Chart1.ToolTip.Enabled = True
        c1Chart1.ToolTip.SelectAction = SelectActionEnum.MouseOver
        c1Chart1.ToolTip.PlotElement = PlotElementEnum.Points
        c1Chart1.ToolTip.AutomaticDelay = 0
        c1Chart1.ToolTip.InitialDelay = 0


    End Sub
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)

        If c >= 0 AndAlso dtMovimientos.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaMovimientos).Position = c
            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", c, dtMovimientos.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtMovimientos, MenuBarra)

    End Sub

    Private Sub AsignaMovExP(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMovExP, myConn, nTablaMovimientosExP, lblInfo)

        If c >= 0 AndAlso dtMovimientosExP.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaMovimientosExP).Position = c
            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", c, dtMovimientosExP.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtMovimientosExP, MenuBarra)

    End Sub


    Private Sub AsignaTXT(ByVal nRow As Long)

        If dt.Rows.Count > 0 Then
            With dt

                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

                With .Rows(nRow)
                    '
                    txtCodigo.Text = MuestraCampoTexto(.Item("codpro"))
                    txtNombre.Text = MuestraCampoTexto(.Item("nombre"))
                    RellenaCombo(aTipo, cmbTipo, .Item("tipo"))
                    RellenaCombo(aCondicion, cmbCondicion, .Item("estatus"))
                    txtUnidad.Text = MuestraCampoTexto(.Item("unidad"))
                    txtCategoria.Text = MuestraCampoTexto(.Item("categoria"))
                    txtRIF.Text = MuestraCampoTexto(.Item("rif"))
                    txtNIT.Text = MuestraCampoTexto(.Item("nit"))
                    txtAsignado.Text = MuestraCampoTexto(.Item("asignado"))
                    txtCodigoContable.Text = MuestraCampoTexto(.Item("codcon"))

                    Dim aIVA() As String = ArregloIVA(myConn, lblInfo)
                    RellenaCombo(aIVA, cmbIVA, InArray(aIVA, .Item("regimeniva")) - 1)

                    txtZona.Text = MuestraCampoTexto(.Item("zona"))
                    txtDireccionFiscal.Text = MuestraCampoTexto(.Item("dirfiscal"))
                    txtDireccionAlterna.Text = MuestraCampoTexto(.Item("dirprove"))
                    txtTelefono1.Text = MuestraCampoTexto(.Item("telef1"))
                    txtTelefono2.Text = MuestraCampoTexto(.Item("telef2"))
                    txtTelefono3.Text = MuestraCampoTexto(.Item("telef3"))
                    txtFAX.Text = MuestraCampoTexto(.Item("fax"))
                    txtWebSite.Text = MuestraCampoTexto(.Item("email4"))
                    txtGerente.Text = MuestraCampoTexto(.Item("gerente"))
                    txtTelefonoGerente.Text = MuestraCampoTexto(.Item("telger"))
                    txtemail1.Text = MuestraCampoTexto(.Item("email1"))
                    txtContacto.Text = MuestraCampoTexto(.Item("contacto"))
                    txtTelefonoContacto.Text = MuestraCampoTexto(.Item("telcon"))
                    txtemail2.Text = MuestraCampoTexto(.Item("email2"))
                    txtVendedor.Text = MuestraCampoTexto(.Item("vendedor"))
                    txtemail3.Text = MuestraCampoTexto(.Item("email3"))
                    txtCobrador.Text = MuestraCampoTexto(.Item("cobrador"))
                    txtemail4.Text = MuestraCampoTexto(.Item("sitioweb"))
                    txtFormaPago.Text = MuestraCampoTexto(.Item("formapago"))
                    txtBancoPago.Text = MuestraCampoTexto(.Item("banco"))
                    txtBancoDep1.Text = MuestraCampoTexto(.Item("bancodeposito1"))
                    txtBancoDep2.Text = MuestraCampoTexto(.Item("bancodeposito2"))
                    txtBancoDep1Cta.Text = MuestraCampoTexto(.Item("ctabancodeposito1"))
                    txtBancoDep2Cta.Text = MuestraCampoTexto(.Item("ctabancodeposito2"))
                    txtIngreso.Text = FormatoFecha(CDate(MuestraCampoTexto(.Item("ingreso"), CStr(jytsistema.sFechadeTrabajo))))
                    txtCredito.Text = FormatoNumero(CDbl(MuestraCampoTexto(.Item("limcredito"), "0.00")))
                    txtSaldo.Text = FormatoNumero(CDbl(MuestraCampoTexto(.Item("saldo"), "0.00")))
                    txtDisponible.Text = FormatoNumero(CDbl(MuestraCampoTexto(.Item("disponible"), "0.00")))

                    'Movimientos
                    txtCodigo1.Text = MuestraCampoTexto(.Item("codpro"))
                    txtNombre1.Text = MuestraCampoTexto(.Item("nombre"))
                    txtUltimoPago.Text = FormatoNumero(CDbl(MuestraCampoTexto(.Item("ultpago"), "0.00")))
                    txtFechaUltimopago.Text = FormatoFecha(CDate(MuestraCampoTexto(.Item("fecultpago"), CStr(jytsistema.sFechadeTrabajo))))
                    txtFormaUltimoPago.Text = MuestraCampoTexto(.Item("forultpago"))

                    'Movimientos ExP
                    txtCodigoExP.Text = MuestraCampoTexto(.Item("codpro"))
                    txtNombreExP.Text = MuestraCampoTexto(.Item("nombre"))
                    txtUltimoPagoExP.Text = FormatoNumero(0.0) 'FormatoNumero(CDbl(MuestraCampoTexto(.Item("ultpago"), "0.00")))
                    txtFechaUltimoPagoExP.Text = FormatoFecha(jytsistema.sFechadeTrabajo)    'FormatoFecha(CDate(MuestraCampoTexto(.Item("fecultpago"), CStr(jytsistema.sFechadeTrabajo))))
                    txtFormaUltimoPagoExP.Text = "" 'MuestraCampoTexto(.Item("forultpago"))

                    txtCreditoM.Text = FormatoNumero(CDbl(MuestraCampoTexto(.Item("limcredito"), "0.00")))
                    txtSaldoM.Text = FormatoNumero(CDbl(MuestraCampoTexto(.Item("saldo"), "0.00")))
                    txtDisponibleM.Text = FormatoNumero(CDbl(MuestraCampoTexto(.Item("disponible"), "0.00")))

                    txtCreditoExP.Text = FormatoNumero(0.0) '  FormatoNumero(CDbl(MuestraCampoTexto(.Item("limcredito"), "0.00")))
                    txtSaldoExP.Text = FormatoNumero(0.0) ' FormatoNumero(CDbl(MuestraCampoTexto(.Item("saldo"), "0.00")))
                    txtDisponibleExP.Text = FormatoNumero(0.0) ' FormatoNumero(CDbl(MuestraCampoTexto(.Item("disponible"), "0.00")))


                    AsignaMovimientos(.Item("codpro"))
                    AsignaMovimientosExp(.Item("codpro"))

                    'Salods por Documento 
                    txtCodigoSaldos.Text = MuestraCampoTexto(.Item("codpro"))
                    txtNombreSaldos.Text = MuestraCampoTexto(.Item("nombre"))

                    'Estadística
                    txtCodigo2.Text = MuestraCampoTexto(.Item("codpro"))
                    txtNombre2.Text = MuestraCampoTexto(.Item("nombre"))

                    txtCodigoExpediente.Text = MuestraCampoTexto(.Item("codpro"))
                    txtNombreExpediente.Text = MuestraCampoTexto(.Item("nombre"))

                End With
            End With
        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub AsignaMovimientos(ByVal CodigoProveedor As String)

        Dim dtUltPago As DataTable
        Dim nTablaUltPago As String = "tblUltimoPago"

        ds = DataSetRequery(ds, " select emision, importe, formapag, numpag, nompag from jsprotrapag where codpro = '" & CodigoProveedor & "' and " _
                            & " remesa = '' and " _
                            & " tipomov in ('AB','CA') and " _
                            & " id_emp = '" & jytsistema.WorkID & "' " _
                            & " order by emision desc limit 1 ", myConn, nTablaUltPago, lblInfo)

        dtUltPago = ds.Tables(nTablaUltPago)

        If dtUltPago.Rows.Count > 0 Then
            With dtUltPago.Rows(0)
                txtUltimoPago.Text = .Item("importe")
                txtFechaUltimopago.Text = FormatoFecha(CDate(.Item("emision").ToString))
                txtFormaUltimoPago.Text = .Item("formapag") & " " & .Item("nompag") & " " & .Item("numpag")
            End With
        Else
            txtUltimoPago.Text = "0.00"
            txtFechaUltimopago.Text = ""
            txtFormaUltimoPago.Text = ""
        End If

        Dim aFld() As String = {"codpro", "id_emp"}
        Dim aStr() As String = {CodigoProveedor, jytsistema.WorkID}

        SaldoCxP(myConn, lblInfo, CodigoProveedor)

        txtCredito.Text = FormatoNumero(CDbl(qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "limcredito")))
        txtCreditoM.Text = FormatoNumero(CDbl(qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "limcredito")))
        txtSaldo.Text = FormatoNumero(CDbl(qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "saldo")))
        txtSaldoM.Text = FormatoNumero(CDbl(qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "saldo")))
        txtDisponible.Text = FormatoNumero(CDbl(qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "disponible")))
        txtDisponibleM.Text = FormatoNumero(CDbl(qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "disponible")))

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
                txtFechaUltimoPagoExP.Text = FormatoFecha(CDate(.Item("emision").ToString))
                txtFormaUltimoPagoExP.Text = .Item("formapag") & " " & .Item("nompag") & " " & .Item("numpag")
            End With
        Else
            txtUltimoPagoExP.Text = "0.00"
            txtFechaUltimoPagoExP.Text = ""
            txtFormaUltimoPagoExP.Text = ""
        End If

        Dim aFld() As String = {"codpro", "id_emp"}
        Dim aStr() As String = {CodigoProveedor, jytsistema.WorkID}

        'SaldoExP(myConn, lblInfo, CodigoProveedor)

        'txtCredito.Text = FormatoNumero(CDbl(qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "limcredito")))
        'txtCreditoExP.Text = FormatoNumero(CDbl(qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "limcredito")))
        'txtSaldo.Text = FormatoNumero(CDbl(qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "saldo")))
        txtSaldoExP.Text = FormatoNumero(SaldoExP(myConn, lblInfo, CodigoProveedor))
        'txtDisponible.Text = FormatoNumero(CDbl(qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "disponible")))
        'txtDisponibleM.Text = FormatoNumero(CDbl(qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "disponible")))


    End Sub

    Private Sub AbrirMovimientosExP(ByVal CodigoProveedor As String)


        dgExP.DataSource = Nothing

        AsignaMovimientosExP(CodigoProveedor)

        Dim aTipo() As String = {"FC.GR.ND.AB.CA.NC", "FC", "GR", "ND", "AB", "CA", "NC"}

        strSQLMovExP = "select a.*  from jsprotrapag a " _
                           & " where " _
                           & " a.remesa = '1' and " _
                           & " a.historico = '0' and " _
                           & " a.codpro  = '" & CodigoProveedor & "' and " _
                           & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                           & " a.id_emp = '" & jytsistema.WorkID & "' " _
                           & " order by a.nummov, a.fotipo, a.emision, a.tipomov "

        ds = DataSetRequery(ds, strSQLMovExP, myConn, nTablaMovimientosExP, lblInfo)
        dtMovimientosExP = ds.Tables(nTablaMovimientosExP)
        Dim aCampos() As String = {"emision", "tipomov", "nummov", "concepto", "vence", "refer", "importe", "origen", "formapag", "nompag", "numpag", "comproba"}
        Dim aNombres() As String = {"Emisión", "TP", "Documento", "Concepto", "Vence", "Referencia", "Importe", "ORG", "FP", "Nombre Pago", "Nº Pago", "Comprobante Nº"}
        Dim aAnchos() As Long = {80, 25, 100, 300, 80, 110, 110, 50, 25, 100, 100, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
            AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {sFormatoFecha, "", "", "", sFormatoFecha, "", sFormatoNumero, "", "", "", "", ""}

        IniciarTabla(dgExP, dtMovimientosExP, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtMovimientosExP.Rows.Count > 0 Then nPosicionMovExP = 0


    End Sub


    Private Sub AbrirMovimientos(ByVal CodigoProveedor As String)


        dg.DataSource = Nothing
       
        AsignaMovimientos(CodigoProveedor)

        Dim aTipo() As String = {"FC.GR.ND.AB.CA.NC", "FC", "GR", "ND", "AB", "CA", "NC"}

        strSQLMov = "select a.*  from jsprotrapag a " _
                           & " where " _
                           & " a.remesa = '' and " _
                           & " a.historico = '0' and " _
                           & " a.codpro  = '" & CodigoProveedor & "' and " _
                           & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                           & " a.id_emp = '" & jytsistema.WorkID & "' " _
                           & " order by a.nummov, a.fotipo, a.emision, a.tipomov  "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        dtMovimientos = ds.Tables(nTablaMovimientos)
        Dim aCampos() As String = {"emision", "tipomov", "nummov", "concepto", "vence", "refer", "importe", "origen", "formapag", "nompag", "numpag", "comproba"}
        Dim aNombres() As String = {"Emisión", "TP", "Documento", "Concepto", "Vence", "Referencia", "Importe", "ORG", "FP", "Nombre Pago", "Nº Pago", "Comprobante Nº"}
        Dim aAnchos() As Long = {80, 25, 100, 300, 80, 110, 110, 50, 25, 100, 100, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
            AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {sFormatoFecha, "", "", "", sFormatoFecha, "", sFormatoNumero, "", "", "", "", ""}
        IniciarTabla(dg, dtMovimientos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtMovimientos.Rows.Count > 0 Then nPosicionMov = 0


    End Sub



    Private Sub IniciarProveedor(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = Contador(myConn, lblInfo, Gestion.iCompras, "COMNUMPRO", "01")    'AutoCodigo(8, ds, nTabla, "codpro")
        Else
            txtCodigo.Text = ""
        End If

        RellenaCombo(aTipo, cmbTipo)
        RellenaCombo(aCondicion, cmbCondicion)

        IniciarTextoObjetos(FormatoItemListView.iCadena, txtNombre, txtNombre1, txtUnidad, txtUnidadNombre, txtCategoria, txtCategoriaNombre, txtRIF, _
                            txtNIT, txtAsignado, txtIVA, txtZona, txtZonaNombre, txtDireccionFiscal, _
                            txtDireccionAlterna, txtTelefono1, txtTelefono2, txtTelefono3, txtFAX, _
                            txtemail1, txtemail2, txtemail3, txtemail4, txtWebSite, txtGerente, txtTelefonoGerente, txtContacto, _
                            txtTelefonoContacto, txtVendedor, txtCobrador, txtFormaPago, txtFormaPagoNombre, txtBancoPago, _
                            txtBancoPagoNombre, txtBancoDep1, txtBancoDep2, txtBancoDep1Nombre, _
                            txtBancoDep2Nombre, txtBancoDep1Cta, txtBancoDep2Cta, txtBancoPagoCuenta, txtCodigoContable)

        IniciarTextoObjetos(FormatoItemListView.iNumero, txtCredito, txtSaldo, txtDisponible)
        IniciarTextoObjetos(FormatoItemListView.iFecha, txtFechaUltimopago, txtIngreso)

        Dim aIVA() As String = ArregloIVA(myConn, lblInfo)
        RellenaCombo(aIVA, cmbIVA, InArray(aIVA, "A") - 1)

        'Movimientos
        dg.Columns.Clear()

    End Sub
    Private Sub ActivarMarco0()

        VisualizarObjetos(True, grpAceptarSalir)
        HabilitarObjetos(False, False, MenuBarra, C1DockingTabPage2, C1DockingTabPage3)
        HabilitarObjetos(True, True, cmbTipo, cmbCondicion, txtNombre, btnUnidad, txtNIT, txtRIF, txtAsignado, _
                                cmbIVA, btnIVA, btnZona, txtDireccionAlterna, txtDireccionFiscal, txtTelefono1, _
                                txtTelefono2, txtTelefono3, txtFAX, txtWebSite, txtGerente, txtTelefonoGerente, _
                                txtemail1, txtCobrador, txtContacto, txtVendedor, txtTelefonoContacto, txtemail2, _
                                txtemail3, txtemail4, btnForma, btnIngreso, btnBanco, btnDep1, btnDep2, _
                                txtBancoDep1Cta, txtBancoDep2Cta, txtCredito)

        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        VisualizarObjetos(False, grpAceptarSalir)
        HabilitarObjetos(True, False, MenuBarra, C1DockingTabPage2, C1DockingTabPage3, C1DockingTabPage4, C1DockingTabPage5)

        HabilitarObjetos(False, True, txtCodigo, cmbTipo, txtAsignado, cmbCondicion, txtNombre, txtUnidadNombre, btnUnidad, _
                          txtCategoriaNombre, txtRIF, txtNIT, cmbIVA, txtIVA, btnIVA, btnZona, txtZona, txtDireccionFiscal, _
                          txtDireccionAlterna, txtTelefono1, txtTelefono2, txtTelefono3, txtFAX, txtWebSite, txtemail1, _
                          txtemail2, txtemail3, txtemail4, txtGerente, txtTelefonoGerente, txtContacto, txtTelefonoContacto, _
                          txtFormaPagoNombre, btnForma, btnBanco, txtBancoDep1, txtBancoDep1Cta, btnDep1, btnDep2, _
                          txtBancoDep1Nombre, txtBancoDep2Nombre, txtBancoDep1Cta, txtBancoDep2Cta, txtBancoPagoCuenta, _
                          txtBancoPagoNombre, txtIngreso, btnIngreso, txtCredito, txtSaldo, txtDisponible, _
                          txtUltimoPago, txtFormaUltimoPago, txtFechaUltimopago, txtCreditoM, txtSaldoM, txtDisponibleM, _
                          txtVendedor, txtCobrador, txtZonaNombre, _
                          txtCodigo1, txtNombre1, txtCodigo2, txtNombre2, txtCodigoSaldos, txtNombreSaldos, _
                          txtCodigoExP, txtNombreExP, txtUltimoPagoExP, txtFormaUltimoPagoExP, txtFechaUltimoPagoExP, _
                          txtCreditoExP, txtSaldoExP, txtDisponibleExP, _
                          txtCodigoExpediente, txtNombreExpediente, txtDocSel, txtSaldoSel, txtCodigoContable)

        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub
    Private Sub btnIngreso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngreso.Click
        txtIngreso.Text = SeleccionaFecha(CDate(txtIngreso.Text), Me, btnIngreso)
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
                Me.BindingContext(ds, nTablaMovimientos).Position = 0
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Movimientos ExP"
                nPosicionMovExP = 0
                Me.BindingContext(ds, nTablaMovimientosExP).Position = 0
                AsignaMovExP(Me.BindingContext(ds, nTablaMovimientosExP).Position, False)
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
                Me.BindingContext(ds, nTablaMovimientos).Position -= 1
                nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Movimientos ExP"
                Me.BindingContext(ds, nTablaMovimientosExP).Position -= 1
                nPosicionMovExP = Me.BindingContext(ds, nTablaMovimientosExP).Position
                AsignaMovExP(Me.BindingContext(ds, nTablaMovimientosExP).Position, False)
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
                Me.BindingContext(ds, nTablaMovimientos).Position += 1
                nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Movimientos ExP"
                Me.BindingContext(ds, nTablaMovimientosExP).Position += 1
                nPosicionMovExP = Me.BindingContext(ds, nTablaMovimientosExP).Position
                AsignaMovExP(Me.BindingContext(ds, nTablaMovimientosExP).Position, False)
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
                Me.BindingContext(ds, nTablaMovimientos).Position = ds.Tables(nTablaMovimientos).Rows.Count - 1
                nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
            Case "Movimientos ExP"
                Me.BindingContext(ds, nTablaMovimientosExP).Position = ds.Tables(nTablaMovimientosExP).Rows.Count - 1
                nPosicionMovExP = Me.BindingContext(ds, nTablaMovimientosExP).Position
                AsignaMovExP(Me.BindingContext(ds, nTablaMovimientosExP).Position, False)
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
                    f.Apuntador = Me.BindingContext(ds, nTablaMovimientos).Position
                    f.Agregar(myConn, ds, dtMovimientos, txtCodigo.Text, cmbTipo.SelectedIndex)

                    SaldoCxP(myConn, lblInfo, txtCodigo.Text)

                    ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)

                    AsignaMovimientos(txtCodigo.Text)

                    If f.Comprobante <> "" Then
                        Select Case f.TipoMovimientoCXP
                            Case 0 'Debitos
                                Dim row As DataRow = dtMovimientos.Select(" NUMMOV = '" & f.Comprobante & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
                                nPosicionMov = dtMovimientos.Rows.IndexOf(row)
                            Case 1 'Creditos
                                Dim row As DataRow = dtMovimientos.Select(" COMPROBA = '" & f.Comprobante & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
                                nPosicionMov = dtMovimientos.Rows.IndexOf(row)
                            Case 2 'Retencion IVA
                                Dim row As DataRow = dtMovimientos.Select(" REFER = '" & f.Comprobante & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
                                nPosicionMov = dtMovimientos.Rows.IndexOf(row)
                            Case 3 'Retencion ISLR
                                Dim row As DataRow = dtMovimientos.Select(" REFER = '" & f.Comprobante & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
                                nPosicionMov = dtMovimientos.Rows.IndexOf(row)
                            Case Else
                                nPosicionMov = f.Apuntador
                        End Select
                    Else
                        nPosicionMov = f.Apuntador
                    End If
                    If nPosicionMov >= 0 Then AsignaMov(nPosicionMov, True)
                    f = Nothing
                End If
            Case "Movimientos ExP"
                If Trim(txtCodigo.Text) <> "" Then

                    Dim f As New jsComArcProveedoresCXP
                    f.Apuntador = Me.BindingContext(ds, nTablaMovimientosExP).Position
                    f.Agregar(myConn, ds, dtMovimientosExP, txtCodigo.Text, cmbTipo.SelectedIndex, 1)
                    SaldoExP(myConn, lblInfo, txtCodigo.Text)

                    ds = DataSetRequery(ds, strSQLMovExP, myConn, nTablaMovimientosExP, lblInfo)

                    AsignaMovimientosExP(txtCodigo.Text)

                    If f.Comprobante <> "" Then
                        Select Case f.TipoMovimientoCXP
                            Case 0 'Debitos
                                Dim row As DataRow = dtMovimientosExP.Select(" NUMMOV = '" & f.Comprobante & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
                                nPosicionMovExP = dtMovimientosExP.Rows.IndexOf(row)
                            Case 1 'Creditos
                                Dim row As DataRow = dtMovimientosExP.Select(" COMPROBA = '" & f.Comprobante & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
                                nPosicionMovExP = dtMovimientosExP.Rows.IndexOf(row)
                            Case 2 'Retencion IVA
                                Dim row As DataRow = dtMovimientosExP.Select(" REFER = '" & f.Comprobante & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
                                nPosicionMovExP = dtMovimientosExP.Rows.IndexOf(row)
                            Case 3 'Retencion ISLR
                                Dim row As DataRow = dtMovimientosExP.Select(" REFER = '" & f.Comprobante & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
                                nPosicionMovExP = dtMovimientosExP.Rows.IndexOf(row)
                            Case Else
                                nPosicionMovExP = f.Apuntador
                        End Select
                    Else
                        nPosicionMovExP = f.Apuntador
                    End If
                    If nPosicionMovExP >= 0 Then AsignaMovExP(nPosicionMovExP, True)
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
                If txtCodigo.Text.Trim <> "" Then
                End If
            Case "Estadísticas"
            Case "Expediente"

        End Select

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Select Case tbcProveedor.SelectedTab.Text
            Case "Proveedor"
                EliminaProveedor()
            Case "Movimientos CxP"
                If cmbCondicion.Text = "Activo" Then
                    EliminarMovimiento()
                    AsignaMovimientos(txtCodigo.Text)
                Else
                    MensajeCritico(lblInfo, "Este proveedor está Inactivo ....")
                End If
            Case "Movimientos ExP"
                If cmbCondicion.Text = "Activo" Then
                    EliminarMovimientoExP()
                    AsignaMovimientosExP(txtCodigo.Text)
                Else
                    MensajeCritico(lblInfo, "Este proveedor está Inactivo ....")
                End If

            Case "Estadísticas"
        End Select

    End Sub
    Private Sub EliminaProveedor()
        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        Dim aCamposDel() As String = {"codpro", "id_emp"}
        Dim aStringsDel() As String = {txtCodigo.Text, jytsistema.WorkID}
        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
        If sRespuesta = MsgBoxResult.Yes Then

            If PoseeMovimientosAsociados(myConn, lblInfo, txtCodigo.Text) Then
                MensajeCritico(lblInfo, "Este banco posee movimientos. Verifique por favor ...")
            Else
                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsprocatpro", strSQL, aCamposDel, aStringsDel, _
                                              Me.BindingContext(ds, nTabla).Position, True))
            End If
        End If

    End Sub

    Private Function PoseeMovimientosAsociados(ByVal MyConn As MySqlConnection, ByVal lblInfo As System.Windows.Forms.Label, _
                                               ByVal CodigoProveedor As String, Optional ByVal NumeroMovimiento As String = "", _
                                               Optional ByVal TipoMovimiento As String = "", _
                                               Optional ByVal CxP_ExP As Integer = 0) As Boolean

        Dim cuenta As Integer = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) cuenta from jsprotrapag where " _
                                                           & " codpro = '" & CodigoProveedor & "' and " _
                                                           & IIf(NumeroMovimiento <> "", " nummov = '' and ", "") _
                                                           & IIf(TipoMovimiento <> "", " tipomov <> '' and ", "") _
                                                           & IIf(CxP_ExP = 0, "", " REMESA= '1' AND ") _
                                                           & " id_emp = '" & jytsistema.WorkID & "' "))
        If cuenta > 0 Then Return True

    End Function
    Private Sub EliminarMovimiento()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position

        If nPosicionMov >= 0 Then
            If dtMovimientos.Rows(nPosicionMov).Item("origen") = "CXP" Then
                sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                If sRespuesta = MsgBoxResult.Yes Then
                    With dtMovimientos.Rows(nPosicionMov)
                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("nummov"))
                        Dim TipoMovimiento As String = .Item("tipomov")
                        Select Case TipoMovimiento
                            Case "FC", "GR", "ND"
                                If PoseeMovimientosAsociados(myConn, lblInfo, .Item("codpro"), .Item("nummov"), .Item("tipomov")) Then
                                    MensajeAdvertencia(lblInfo, "Este documento posee documentos asociados a él...")
                                Else
                                    EjecutarSTRSQL(myConn, lblInfo, "DELETE from jsprotrapag where " _
                                        & " CODPRO = '" & .Item("codpro") & "' and " _
                                        & " TIPOMOV ='" & .Item("tipomov") & "' and " _
                                        & " EMISION = '" & FormatoFechaMySQL(CDate(.Item("emision").ToString)) & "'AND " _
                                        & " NUMMOV = '" & .Item("nummov") & "' and " _
                                        & " REFER = '" & .Item("refer") & "' AND " _
                                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                        & " ID_EMP ='" & jytsistema.WorkID & "' ")
                                End If
                            Case "AB", "CA", "NC"
                                Dim sRespuesta2 As Microsoft.VisualBasic.MsgBoxResult
                                If .Item("multican") = "1" Then
                                    sRespuesta2 = MsgBox("Este documento pertenece a una cancelación múltiple. Se eliminarán todos los documentos ¿ Desea Eliminar ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                                    If sRespuesta2 = MsgBoxResult.No Then
                                        Return
                                    End If
                                End If
                                EjecutarSTRSQL(myConn, lblInfo, "DELETE from jsprotrapag where " _
                                    & " CODPRO = '" & .Item("codpro") & "' AND " _
                                    & " REFER = '" & .Item("refer") & "' AND " _
                                    & " COMPROBA = '" & .Item("comproba") & "' AND  " _
                                    & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                    & " ID_EMP = '" & jytsistema.WorkID & "' ")

                                EjecutarSTRSQL(myConn, lblInfo, "DELETE from jsprotrapagcan where " _
                                    & " CODPRO = '" & .Item("codpro") & "' AND " _
                                    & " COMPROBA = '" & .Item("comproba") & "' AND  " _
                                    & " ID_EMP = '" & jytsistema.WorkID & "' ")

                                ' Actualiza encabezado de compra
                                EjecutarSTRSQL(myConn, lblInfo, " update jsproenccom set " _
                                    & " formapag = '', numpag = '', nompag = '', benefic = '', caja = '' " _
                                    & IIf(.Item("tipomov") = "NC" AndAlso Mid(.Item("REFER"), 1, 5) = "ISLR-", ", ret_islr = 0.00, num_ret_islr = '', fecha_ret_islr = '2007-01-01' ", "") _
                                    & IIf(.Item("tipomov") = "NC" AndAlso Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA", ", ret_iva = 0.00, num_ret_iva = '', fecha_ret_iva = '2007-01-01' ", "") _
                                    & " where " _
                                    & " numcom = '" & .Item("nummov") & "' and " _
                                    & " codpro = '" & .Item("codpro") & "' and " _
                                    & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "' ")

                                If .Item("tipomov") = "NC" AndAlso Mid(.Item("REFER"), 1, 5) = "ISLR-" Then
                                    EjecutarSTRSQL(myConn, lblInfo, " update jsproenccom set ret_islr = 0.00, num_ret_islr = '', " _
                                                   & " fecha_ret_islr = '2007-01-01', por_ret_islr = 0.00, base_ret_islr = 0.00 " _
                                                   & " where codpro = '" & .Item("codpro") & "' and num_ret_islr = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                End If

                                If .Item("tipomov") = "NC" AndAlso Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA" Then
                                    EjecutarSTRSQL(myConn, lblInfo, " update jsproenccom set ret_iva = 0.00, num_ret_iva = '', " _
                                                   & " fecha_ret_iva = '2007-01-01' " _
                                                   & " where codpro = '" & .Item("codpro") & "' and num_ret_iva = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                                    EjecutarSTRSQL(myConn, lblInfo, " update jsproivacom set retencion = 0.00, numretencion = '' " _
                                                  & " where codpro = '" & .Item("codpro") & "' and numcom = '" & .Item("nummov") & "' and numretencion = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                                End If


                                ' Actualiza encabezado de gasto
                                EjecutarSTRSQL(myConn, lblInfo, " update jsproencgas set " _
                                    & " formapag = '', numpag = '', nompag = '', benefic = '', caja = '' " _
                                    & IIf(.Item("tipomov") = "NC" AndAlso Mid(.Item("REFER"), 1, 5) = "ISLR-", ", ret_islr = 0.00, num_ret_islr = '', fecha_ret_islr = '2007-01-01' ", "") _
                                    & IIf(.Item("tipomov") = "NC" AndAlso Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA", ", ret_iva = 0.00, num_ret_iva = '', fecha_ret_iva = '2007-01-01' ", "") _
                                    & " where " _
                                    & " numgas = '" & .Item("nummov") & "' and " _
                                    & " codpro = '" & .Item("codpro") & "' and " _
                                    & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "' ")


                                If .Item("tipomov") = "NC" AndAlso Mid(.Item("REFER"), 1, 5) = "ISLR-" Then
                                    EjecutarSTRSQL(myConn, lblInfo, " update jsproencgas set ret_islr = 0.00, num_ret_islr = '', " _
                                                   & " fecha_ret_islr = '2007-01-01', por_ret_islr = 0.00, base_ret_islr = 0.00 " _
                                                   & " where codpro = '" & .Item("codpro") & "' and num_ret_islr = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                End If

                                If .Item("tipomov") = "NC" AndAlso Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA" Then
                                    EjecutarSTRSQL(myConn, lblInfo, " update jsproencgas set ret_iva = 0.00, num_ret_iva = '', " _
                                                   & " fecha_ret_iva = '2007-01-01' " _
                                                   & " where codpro = '" & .Item("codpro") & "' and num_ret_iva = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                                    EjecutarSTRSQL(myConn, lblInfo, " update jsproivagas set retencion = 0.00, numretencion = '' " _
                                                  & " where codpro = '" & .Item("codpro") & "' and numgas = '" & .Item("nummov") & "' and numretencion = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                                End If


                                'elimina movimiento en caja
                                EjecutarSTRSQL(myConn, lblInfo, " delete from jsbantracaj where " _
                                    & " nummov = '" & .Item("comproba") & "' and " _
                                    & " origen = 'CXP' and " _
                                    & " caja = '" & .Item("cajapag") & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "'")

                                'elimina movimiento en banco
                                EjecutarSTRSQL(myConn, lblInfo, " delete from jsbantraban where " _
                                    & " numorg = '" & .Item("comproba") & "' and " _
                                    & " origen = 'CXP' and " _
                                    & " codban = '" & .Item("nompag") & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "'")
                        End Select

                        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                        dtMovimientos = ds.Tables(nTablaMovimientos)

                        If nPosicionMov > dtMovimientos.Rows.Count - 1 Then nPosicionMov = dtMovimientos.Rows.Count - 1
                        AsignaMov(nPosicionMov, False)

                    End With
                End If
            Else
                MensajeAdvertencia(lblInfo, "Movimiento no puede ser eliminado desde CXP...")
            End If
        End If

    End Sub

    Private Sub EliminarMovimientoExP()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionMovExP = Me.BindingContext(ds, nTablaMovimientosExP).Position

        If nPosicionMovExP >= 0 Then
            If dtMovimientosExP.Rows(nPosicionMovExP).Item("origen") = "CXP" Then
                sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                If sRespuesta = MsgBoxResult.Yes Then
                    With dtMovimientosExP.Rows(nPosicionMovExP)
                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("nummov"))
                        Dim TipoMovimiento As String = .Item("tipomov")
                        Select Case TipoMovimiento
                            Case "FC", "GR", "ND"
                                If PoseeMovimientosAsociados(myConn, lblInfo, .Item("codpro"), .Item("nummov"), .Item("tipomov"), 1) Then
                                    MensajeAdvertencia(lblInfo, "Este documento posee documentos asociados a él...")
                                Else
                                    EjecutarSTRSQL(myConn, lblInfo, "DELETE from jsprotrapag where " _
                                        & " CODPRO = '" & .Item("codpro") & "' and " _
                                        & " TIPOMOV ='" & .Item("tipomov") & "' and " _
                                        & " EMISION = '" & FormatoFechaMySQL(CDate(.Item("emision").ToString)) & "'AND " _
                                        & " NUMMOV = '" & .Item("nummov") & "' and " _
                                        & " REFER = '" & .Item("refer") & "' AND " _
                                        & " REMESA = '1' AND " _
                                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                        & " ID_EMP ='" & jytsistema.WorkID & "' ")
                                End If
                            Case "AB", "CA", "NC"
                                Dim sRespuesta2 As Microsoft.VisualBasic.MsgBoxResult
                                If .Item("multican") = "1" Then
                                    sRespuesta2 = MsgBox("Este documento pertenece a una cancelación múltiple. Se eliminarán todos los documentos ¿ Desea Eliminar ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                                    If sRespuesta2 = MsgBoxResult.No Then
                                        Return
                                    End If
                                End If
                                EjecutarSTRSQL(myConn, lblInfo, "DELETE from jsprotrapag where " _
                                    & " CODPRO = '" & .Item("codpro") & "' AND " _
                                    & " REFER = '" & .Item("refer") & "' AND " _
                                    & " COMPROBA = '" & .Item("comproba") & "' AND  " _
                                    & " REMESA = '1' AND " _
                                    & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                    & " ID_EMP = '" & jytsistema.WorkID & "' ")

                                EjecutarSTRSQL(myConn, lblInfo, "DELETE from jsprotrapagcan where " _
                                    & " CODPRO = '" & .Item("codpro") & "' AND " _
                                    & " COMPROBA = '" & .Item("comproba") & "' AND  " _
                                    & " ID_EMP = '" & jytsistema.WorkID & "' ")

                                ' Actualiza encabezado de compra
                                EjecutarSTRSQL(myConn, lblInfo, " update jsproenccom set " _
                                    & " formapag = '', numpag = '', nompag = '', benefic = '', caja = '' " _
                                    & IIf(.Item("tipomov") = "NC" AndAlso Mid(.Item("REFER"), 1, 5) = "ISLR-", ", ret_islr = 0.00, num_ret_islr = '', fecha_ret_islr = '2007-01-01' ", "") _
                                    & IIf(.Item("tipomov") = "NC" AndAlso Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA", ", ret_iva = 0.00, num_ret_iva = '', fecha_ret_iva = '2007-01-01' ", "") _
                                    & " where " _
                                    & " numcom = '" & .Item("nummov") & "' and " _
                                    & " codpro = '" & .Item("codpro") & "' and " _
                                    & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "' ")

                                If .Item("tipomov") = "NC" AndAlso Mid(.Item("REFER"), 1, 5) = "ISLR-" Then
                                    EjecutarSTRSQL(myConn, lblInfo, " update jsproenccom set ret_islr = 0.00, num_ret_islr = '', " _
                                                   & " fecha_ret_islr = '2007-01-01', por_ret_islr = 0.00, base_ret_islr = 0.00 " _
                                                   & " where codpro = '" & .Item("codpro") & "' and num_ret_islr = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                End If

                                If .Item("tipomov") = "NC" AndAlso Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA" Then
                                    EjecutarSTRSQL(myConn, lblInfo, " update jsproenccom set ret_iva = 0.00, num_ret_iva = '', " _
                                                   & " fecha_ret_iva = '2007-01-01' " _
                                                   & " where codpro = '" & .Item("codpro") & "' and num_ret_iva = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                                    EjecutarSTRSQL(myConn, lblInfo, " update jsproivacom set retencion = 0.00, numretencion = '' " _
                                                  & " where codpro = '" & .Item("codpro") & "' and numcom = '" & .Item("nummov") & "' and numretencion = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                                End If


                                ' Actualiza encabezado de gasto
                                EjecutarSTRSQL(myConn, lblInfo, " update jsproencgas set " _
                                    & " formapag = '', numpag = '', nompag = '', benefic = '', caja = '' " _
                                    & IIf(.Item("tipomov") = "NC" AndAlso Mid(.Item("REFER"), 1, 5) = "ISLR-", ", ret_islr = 0.00, num_ret_islr = '', fecha_ret_islr = '2007-01-01' ", "") _
                                    & IIf(.Item("tipomov") = "NC" AndAlso Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA", ", ret_iva = 0.00, num_ret_iva = '', fecha_ret_iva = '2007-01-01' ", "") _
                                    & " where " _
                                    & " numgas = '" & .Item("nummov") & "' and " _
                                    & " codpro = '" & .Item("codpro") & "' and " _
                                    & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "' ")


                                If .Item("tipomov") = "NC" AndAlso Mid(.Item("REFER"), 1, 5) = "ISLR-" Then
                                    EjecutarSTRSQL(myConn, lblInfo, " update jsproencgas set ret_islr = 0.00, num_ret_islr = '', " _
                                                   & " fecha_ret_islr = '2007-01-01', por_ret_islr = 0.00, base_ret_islr = 0.00 " _
                                                   & " where codpro = '" & .Item("codpro") & "' and num_ret_islr = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                End If

                                If .Item("tipomov") = "NC" AndAlso Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA" Then
                                    EjecutarSTRSQL(myConn, lblInfo, " update jsproencgas set ret_iva = 0.00, num_ret_iva = '', " _
                                                   & " fecha_ret_iva = '2007-01-01' " _
                                                   & " where codpro = '" & .Item("codpro") & "' and num_ret_iva = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                                    EjecutarSTRSQL(myConn, lblInfo, " update jsproivagas set retencion = 0.00, numretencion = '' " _
                                                  & " where codpro = '" & .Item("codpro") & "' and numgas = '" & .Item("nummov") & "' and numretencion = '" & .Item("refer") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                                End If


                                'elimina movimiento en caja
                                EjecutarSTRSQL(myConn, lblInfo, " delete from jsbantracaj where " _
                                    & " nummov = '" & .Item("comproba") & "' and " _
                                    & " origen = 'CXP' and " _
                                    & " caja = '" & .Item("cajapag") & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "'")

                                'elimina movimiento en banco
                                EjecutarSTRSQL(myConn, lblInfo, " delete from jsbantraban where " _
                                    & " numorg = '" & .Item("comproba") & "' and " _
                                    & " origen = 'CXP' and " _
                                    & " codban = '" & .Item("nompag") & "' and " _
                                    & " id_emp = '" & jytsistema.WorkID & "'")
                        End Select

                        ds = DataSetRequery(ds, strSQLMovExP, myConn, nTablaMovimientosExP, lblInfo)
                        dtMovimientosExP = ds.Tables(nTablaMovimientosExP)

                        If nPosicionMovExP > dtMovimientosExP.Rows.Count - 1 Then nPosicionMovExP = dtMovimientosExP.Rows.Count - 1
                        AsignaMovExP(nPosicionMovExP, False)

                    End With
                End If
            Else
                MensajeAdvertencia(lblInfo, "Movimiento no puede ser eliminado desde CXP...")
            End If
        End If

    End Sub


    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar
        Select Case tbcProveedor.SelectedTab.Text
            Case "Proveedor"
                Dim Campos() As String = {"codpro", "nombre", "rif"}
                Dim Nombres() As String = {"Código proveedor", "Nombre o razón social proveedor", "RIF"}
                Dim Anchos() As Long = {100, 750, 100}
                f.Text = "Proveedores"
                f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Proveedores", 1)
                nPosicionCat = f.Apuntador
                Me.BindingContext(ds, nTabla).Position = nPosicionCat
                AsignaTXT(nPosicionCat)
                f = Nothing
            Case "Movimientos CxP"
                '                Dim Campos() As String = {"fechamov", "numdoc", "concepto", "nombre"}
                '               Dim Nombres() As String = {"Emisión", "Nº Movimiento", "Concepto", "Cliente ó Proveedor"}
                '               Dim Anchos() As Long = {100, 120, 2450, 2450}
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
                With dtMovimientos.Rows(nPosicionMov)
                    If .Item("tipomov") = "NC" And Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA" Then
                        'IMPRIMIR COMPROBANTE RETENCIOIN IVA
                        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cRetencionIVA, "Comprobante de Retención de IVA", txtCodigo.Text, .Item("refer"))
                    ElseIf .Item("tipomov") = "ND" And Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA" Then
                        'IMPRIMIR COMPROBANTE RETENCION IVA
                        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cRetencionIVA, "Comprobante de Retención de IVA", txtCodigo.Text, .Item("refer"))
                    ElseIf .Item("tipomov") = "NC" And Mid(.Item("REFER"), 1, 5) = "ISLR-" Then
                        'IMPRIMNIR COMPROBANTE RETENCION ISLR
                        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cRetencionISLR, "Comprobante de Retención de ISLR", txtCodigo.Text, .Item("refer"))
                    ElseIf dtMovimientos.Rows(nPosicionMov).Item("comproba") <> "" Then
                        'IMPRIMIR COMPROBANTE DE EGRESO
                        If dtMovimientos.Rows(nPosicionMov).Item("formapag") = "CH" Then
                            Dim fr As New jsBanRepParametros
                            fr.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cComprobanteDeEgreso, "COMPROBANTE DE PAGO", dtMovimientos.Rows(nPosicionMov).Item("nompag"), dtMovimientos.Rows(nPosicionMov).Item("COMPROBA"), , "CXP")
                            fr.Dispose()
                            fr = Nothing
                        Else
                            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cComprobantePago, "COMPROBANTE DE EGRESO", _
                                     txtCodigo.Text, dtMovimientos.Rows(nPosicionMov).Item("COMPROBA"))
                        End If

                    Else
                        'IMPRIMIR MOVIMIENTOS DE CXP
                        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cMovimientosProveedores, "Movimientos Proveedores", _
                                 txtCodigo1.Text)
                    End If
                End With
                f = Nothing
            Case "Movimientos ExP"
                Dim f As New jsComRepParametros
                With dtMovimientosExP.Rows(nPosicionMovExP)
                    If .Item("tipomov") = "NC" And Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA" Then
                        'IMPRIMIR COMPROBANTE RETENCIOIN IVA
                        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cRetencionIVA, "Comprobante de Retención de IVA", _
                                 txtCodigo.Text, .Item("refer"), , , , , 1)
                    ElseIf .Item("tipomov") = "ND" And Mid(.Item("CONCEPTO"), 1, 13) = "RETENCION IVA" Then
                        'IMPRIMIR COMPROBANTE RETENCION IVA
                        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cRetencionIVA, "Comprobante de Retención de IVA", _
                                 txtCodigo.Text, .Item("refer"), , , , , 1)
                    ElseIf .Item("tipomov") = "NC" And Mid(.Item("REFER"), 1, 5) = "ISLR-" Then
                        'IMPRIMNIR COMPROBANTE RETENCION ISLR
                        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cRetencionISLR, "Comprobante de Retención de ISLR", _
                                 txtCodigo.Text, .Item("refer"), , , , , 1)
                    ElseIf dtMovimientosExP.Rows(nPosicionMovExP).Item("comproba") <> "" Then
                        'IMPRIMIR COMPROBANTE DE EGRESO
                        If dtMovimientosExP.Rows(nPosicionMovExP).Item("formapag") = "CH" Then
                            Dim fr As New jsBanRepParametros
                            fr.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cComprobanteDeEgreso, "COMPROBANTE DE PAGO", _
                                      dtMovimientosExP.Rows(nPosicionMovExP).Item("nompag"), _
                                      dtMovimientosExP.Rows(nPosicionMovExP).Item("COMPROBA"), , "CXP")
                            fr.Dispose()
                            fr = Nothing
                        Else
                            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cComprobantePago, "COMPROBANTE DE EGRESO", _
                                     txtCodigo.Text, dtMovimientosExP.Rows(nPosicionMovExP).Item("COMPROBA"))
                        End If

                    Else
                        'IMPRIMIR MOVIMIENTOS DE CXP
                        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cMovimientosProveedores, "Movimientos ExP Proveedores", _
                                 txtCodigo1.Text, , , , , , 1)
                    End If
                End With
                f = Nothing

            Case "Estadísticas"
        End Select

    End Sub


    Private Function Validado() As Boolean

        Dim afld() As String = {"codpro", "id_emp"}
        Dim aStr() As String = {txtCodigo.Text, jytsistema.WorkID}

        If i_modo = movimiento.iAgregar AndAlso qFound(myConn, lblInfo, "jsprocatpro", afld, aStr) Then
            MensajeAdvertencia(lblInfo, "Código proveedor YA existe. Verifique por favor...")
            Return False
        End If

        If Trim(txtNombre.Text) = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar un nombre o razón social válida...")
            Return False
        End If


        If Not IIf(EsRIF(txtRIF.Text.Trim), validarRif(txtRIF.Text.Trim), validarCI(txtRIF.Text.Trim.Split("-")(0) + "-" + txtRIF.Text.Trim.Split("-")(1).Trim)) Then
            MensajeAdvertencia(lblInfo, " CI o RIF no válido. Debe indicarlo de la forma V-11111111 ...")
            EnfocarTextoM(txtRIF)
            Exit Function
        End If

        Dim afldR() As String = {"rif", "id_emp"}
        Dim aStrR() As String = {Cedula_O_RIF(txtRIF.Text), jytsistema.WorkID}

        If i_modo = movimiento.iAgregar AndAlso qFound(myConn, lblInfo, "jsprocatpro", afldR, aStrR) Then
            MensajeAdvertencia(lblInfo, "RIF de proveedor YA existe. Verifique por favor...")
            Return False
        End If

        If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPARAM04")) Then
            If txtCodigoContable.Text.Trim = "" Then
                MensajeAdvertencia(lblInfo, "Debe indicar un código contable para este proveedor...")
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

        InsertEditCOMProveedores(myConn, lblInfo, Inserta, txtCodigo.Text, txtNombre.Text, txtCategoria.Text, txtUnidad.Text, _
                                  sRIF, txtNIT.Text, txtAsignado.Text, txtDireccionFiscal.Text, txtDireccionAlterna.Text, _
                                  txtemail1.Text, txtemail2.Text, txtemail3.Text, txtemail4.Text, "", txtWebSite.Text, _
                                  txtTelefono1.Text, txtTelefono2.Text, txtTelefono3.Text, txtFAX.Text, txtGerente.Text, _
                                  txtTelefonoGerente.Text, txtContacto.Text, txtTelefonoContacto.Text, ValorNumero(txtCredito.Text), _
                                  ValorNumero(txtDisponible.Text), 0.0, 0.0, 0.0, 0.0, 0, 0, "", txtZona.Text, txtCobrador.Text, _
                                  txtVendedor.Text, ValorNumero(txtSaldo.Text), ValorNumero(txtUltimoPago.Text), _
                                  fechaUltima, IIf(txtFormaUltimoPago.Text.Length > 2, Mid(txtFormaUltimoPago.Text, 1, 2), ""), cmbIVA.Text, txtFormaPago.Text, _
                                  txtBancoPago.Text, txtBancoPagoCuenta.Text, txtBancoDep1.Text, txtBancoDep2.Text, _
                                  txtBancoDep1Cta.Text, txtBancoDep2Cta.Text, CDate(txtIngreso.Text), txtCodigoContable.Text, cmbCondicion.SelectedIndex, _
                                  cmbTipo.SelectedIndex)

        SaldoCxP(myConn, lblInfo, txtCodigo.Text)

        InsertarAuditoria(myConn, IIf(Inserta, MovAud.iIncluir, MovAud.imodificar), sModulo & " - " & tbcProveedor.SelectedTab.Text, txtCodigo.Text)


        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" CODPRO = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionCat = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionCat
        AsignaTXT(nPosicionCat)
        DesactivarMarco0()
        tbcProveedor.SelectedTab = C1DockingTabPage1
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
    txtNombre.GotFocus, cmbTipo.GotFocus, cmbCondicion.GotFocus, btnUnidad.GotFocus, txtNIT.GotFocus, _
    txtAsignado.GotFocus, cmbIVA.GotFocus, btnIVA.GotFocus, btnZona.GotFocus, txtDireccionFiscal.GotFocus, txtDireccionAlterna.GotFocus, _
    txtTelefono1.GotFocus, txtTelefono2.GotFocus, txtTelefono3.GotFocus, txtFAX.GotFocus, txtWebSite.GotFocus, _
    txtGerente.GotFocus, txtTelefonoGerente.GotFocus, txtemail1.GotFocus, txtContacto.GotFocus, txtTelefonoContacto.GotFocus, _
    txtemail2.GotFocus, txtVendedor.GotFocus, txtCobrador.GotFocus, txtemail3.GotFocus, txtemail4.GotFocus, _
    btnForma.GotFocus, btnBanco.GotFocus, btnDep1.GotFocus, btnDep2.GotFocus, txtBancoPagoCuenta.GotFocus, _
    txtBancoDep1Cta.GotFocus, txtBancoDep2Cta.GotFocus, btnIngreso.GotFocus, txtCredito.GotFocus

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
        MensajeEtiqueta(lblInfo, mensaje, TipoMensaje.iInfo)
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
        dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        nPosicionMov = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
    End Sub

    Private Sub dgExP_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgExP.RowHeaderMouseClick, _
        dgExP.CellMouseClick, dgExP.RegionChanged
        Me.BindingContext(ds, nTablaMovimientosExP).Position = e.RowIndex
        nPosicionMovExP = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", e.RowIndex, ds.Tables(nTablaMovimientosExP).Rows.Count)
    End Sub

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
                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nPosicionCat, ds.Tables(nTabla).Rows.Count)
            Case 3 '' Estadísticas
                AsignaEstadistica()
            Case 4 '' Expediente
                AsignarExpediente(txtCodigo.Text)
            Case 5 '' Movimientos ExP
                AbrirMovimientosExP(txtCodigo.Text)
                'AsignaMovExP(nPosicionMovExP, False)
                dgExP.Enabled = True
        End Select

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
        txtDocSel.Text = FormatoEntero(iSel)
        txtSaldoSel.Text = FormatoNumero(dSel)

    End Sub
    Private Sub IniciarSaldosPorDocumento(ByVal ActualHistorico As Integer, ByVal CodProveedor As String)

        tblSaldos = "tbl" & NumeroAleatorio(100000)

        txtDocSel.Text = FormatoEntero(0)
        txtSaldoSel.Text = FormatoNumero(0.0)


        If CodProveedor <> "" Then
            Dim aFields() As String = {"sel.entero", "codpro.cadena15", "nombre.cadena250", "nummov.cadena20", "tipomov.cadena2", _
                                       "refer.cadena15", "emision.fecha", "vence.fecha", "importe.doble19", "saldo.doble19", _
                                       "codven.cadena5", "nomVendedor.cadena50"}

            CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblSaldos, aFields)


            EjecutarSTRSQL(myConn, lblInfo, " INSERT INTO " & tblSaldos _
            & " select 0 sel, a.codpro codcli, b.nombre, a.nummov, a.tipomov, a.refer, a.emision, a.vence, a.importe, " _
            & " IF(c.saldo IS NULL, 0.00, c.saldo) saldo, '' codven, '' nomVendedor " _
            & " FROM jsprotrapag a  " _
            & " LEFT JOIN jsprocatpro b ON (a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN (SELECT codpro, nummov, SUM(importe) saldo " _
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

            txtDocSel.Text = FormatoEntero(0)
            txtSaldoSel.Text = FormatoNumero(0.0)

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
        Dim aAnchos() As Long = {80, 25, 90, 80, 80, 25, 90, 80, 35, 50, 80}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
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
        Dim aAnchos() As Long = {90, 70, 190, 30, 35, 70, 70, 45, 45, 50, 70}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {"", "", "", "", "", sFormatoCantidad, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero}
        IniciarTabla(dgMercasDocumentos, dtMerSal, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, , , 8)

    End Sub
    Private Sub AsignaEstadistica()
        rBtn1.Checked = True
        RellenaCombo(aTipoEstadistica, cmbTipoEstadistica)
    End Sub
    Private Sub AbrirEstadisticas()

        EsperaPorFavor()

        Dim aCam() As String = {"codart", "nomart", "unidad", "mEne", "mFeb", "mMar", "mAbr", "mMay", "mJun", "mJul", "mAgo", "mSep", "mOct", "mNov", "mDic"}
        Dim aNom() As String = {"Código", "Nombre artículo", "UND", "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"}
        Dim aAnc() As Long = {70, 280, 35, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70}
        Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}

        Dim aFor() As String = {"", "", "", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero}

        Dim dtStat As New DataTable
        dtStat = ConsultaEstadistica(myConn, ds, lblInfo, txtCodigo.Text, "COM", _
                cmbTipoEstadistica.SelectedIndex, IIf(rBtn1.Checked, TipoDatoMercancia.iUnidadesDeVenta, IIf(rBtn2.Checked, TipoDatoMercancia.iKilogramos, TipoDatoMercancia.iMonetarios)), jytsistema.sFechadeTrabajo, "tblMovimientosAnoActualCompras")

        IniciarTabla(dgEstadistica, dtStat, aCam, aNom, aAnc, aAli, aFor)

        VerHistograma(c1Chart1, IIf(rBtn1.Checked, TipoDatoMercancia.iUnidadesDeVenta, IIf(rBtn2.Checked, TipoDatoMercancia.iKilogramos, TipoDatoMercancia.iMonetarios)))

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If dt.Rows.Count = 0 Then
            IniciarProveedor(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            If Me.BindingContext(ds, nTabla).Position > 0 Then _
                nPosicionCat = Me.BindingContext(ds, nTabla).Position

            AsignaTXT(nPosicionCat)
        End If
        DesactivarMarco0()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub


    Private Sub txtCredito_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCredito.KeyPress
        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub

    Private Sub cmbIVA_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbIVA.SelectedIndexChanged
        txtIVA.Text = FormatoNumero(PorcentajeIVA(myConn, lblInfo, jytsistema.sFechadeTrabajo, cmbIVA.Text)) & "%"
    End Sub

    Private Sub txtUnidad_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUnidad.TextChanged
        Dim aFld() As String = {"codigo", "id_emp"}
        Dim aStr() As String = {txtUnidad.Text, jytsistema.WorkID}
        txtUnidadNombre.Text = qFoundAndSign(myConn, lblInfo, "jsprolisuni", aFld, aStr, "descrip")
    End Sub

    Private Sub txtCategoria_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCategoria.TextChanged
        Dim aFld() As String = {"codigo", "id_emp"}
        Dim aStr() As String = {txtCategoria.Text, jytsistema.WorkID}
        txtCategoriaNombre.Text = qFoundAndSign(myConn, lblInfo, "jsproliscat", aFld, aStr, "descrip")
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
        dtt = ConsultaEstadistica(myConn, ds, lblInfo, txtCodigo.Text, "COM", cmbTipoEstadistica.SelectedIndex, IIf(rBtn1.Checked, TipoDatoMercancia.iUnidadesDeVenta, IIf(rBtn2.Checked, TipoDatoMercancia.iKilogramos, TipoDatoMercancia.iMonetarios)), jytsistema.sFechadeTrabajo, _
                                  "tblResumenAnoActualCompras", " a.id_emp ")

        Dim dttAnteriores As New DataTable
        dttAnteriores = ConsultaEstadistica(myConn, ds, lblInfo, txtCodigo.Text, "COM", cmbTipoEstadistica.SelectedIndex, IIf(rBtn1.Checked, TipoDatoMercancia.iUnidadesDeVenta, IIf(rBtn2.Checked, TipoDatoMercancia.iKilogramos, TipoDatoMercancia.iMonetarios)), DateAdd(DateInterval.Year, -1, jytsistema.sFechadeTrabajo), _
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

    Private Sub rBtn1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rBtn1.CheckedChanged, _
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
        Dim aAncexp() As Long = {140, 240, 100, 350}
        Dim aAliExp() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aForExp() As String = {sFormatoFechaCorta, "", "", ""}

        IniciarTabla(dgExpediente, dtExp, aCamExp, aNomExp, aAncexp, aAliExp, aForExp)


    End Sub


    Private Sub btnDep2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDep2.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Bancos de la plaza", FormatoTablaSimple(Modulo.iBancos), False, TipoCargaFormulario.iShowDialog)
        txtBancoDep2.Text = f.Seleccion
        f = Nothing
    End Sub

    Private Sub btnDep1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDep1.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Bancos de la plaza", FormatoTablaSimple(Modulo.iBancos), False, TipoCargaFormulario.iShowDialog)
        txtBancoDep1.Text = f.Seleccion
        f = Nothing
    End Sub

    Private Sub btnBanco_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBanco.Click
        Dim f As New jsControlArcTablaSimple
        Dim dtBan As DataTable
        Dim nTableBan As String = "tblBan"
        ds = DataSetRequery(ds, " select codban codigo, nomban descripcion from jsbancatban where estatus = '1' and id_emp = '" & jytsistema.WorkID & "' order by codban ", _
                             myConn, nTableBan, lblInfo)
        dtBan = ds.Tables(nTableBan)
        f.Cargar("Bancos de la empresa", ds, dtBan, nTableBan, TipoCargaFormulario.iShowDialog, False)
        txtBancoPago.Text = f.Seleccion
        f = Nothing
        dtBan = Nothing
    End Sub

    Private Sub btnForma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForma.Click
        Dim f As New jsControlArcTablaSimple
        Dim dtForma As DataTable
        Dim nTableFor As String = "tblForma"
        ds = DataSetRequery(ds, " select codfor codigo, nomfor descripcion from jsconctafor where id_emp = '" & jytsistema.WorkID & "' order by codfor ", _
                             myConn, nTableFor, lblInfo)
        dtForma = ds.Tables(nTableFor)
        f.Cargar("Forma de pago", ds, dtForma, nTableFor, TipoCargaFormulario.iShowDialog, False)
        txtFormaPago.Text = f.Seleccion
        f = Nothing
        dtForma = Nothing
    End Sub

    Private Sub btnUnidad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnidad.Click
        Dim f As New jsComArcUnidadCategoria
        f.Grupo0 = txtUnidad.Text
        f.Grupo1 = txtCategoria.Text
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        txtUnidad.Text = f.Grupo0
        txtCategoria.Text = f.Grupo1
        f = Nothing
    End Sub

    Private Sub btnZona_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZona.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Zonas proveedores", FormatoTablaSimple(Modulo.iZonaProveedor), False, TipoCargaFormulario.iShowDialog)
        txtZona.Text = f.Seleccion
        f = Nothing
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
                e.TooltipText = String.Format("{0}" + ControlChars.Lf + "Mes = " + aNom(Math.Round(x) - 1) + _
                                              ControlChars.Lf + "Valor = {2:#.##}", ds.Label, x, y)
            Else
                e.TooltipText = ""
            End If
            'End If
        End If
    End Sub

    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(ds, nTablaMovimientos).Position += 1
                nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(nPosicionMov, False)
            Case Keys.Up
                Me.BindingContext(ds, nTablaMovimientos).Position -= 1
                nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(nPosicionMov, False)
        End Select
    End Sub

    Private Sub dgExP_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgExP.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(ds, nTablaMovimientosExP).Position += 1
                nPosicionMovExP = Me.BindingContext(ds, nTablaMovimientosExP).Position
                AsignaMovExP(nPosicionMovExP, False)
            Case Keys.Up
                Me.BindingContext(ds, nTablaMovimientosExP).Position -= 1
                nPosicionMovExP = Me.BindingContext(ds, nTablaMovimientosExP).Position
                AsignaMovExP(nPosicionMovExP, False)
        End Select
    End Sub


    Private Sub dgSaldos_CellMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgSaldos.CellMouseUp
        CalculaTotalesSAldos()
    End Sub

    Private Sub dgSaldos_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgSaldos.CellContentClick
        If e.ColumnIndex = 0 Then
            dtSaldos.Rows(e.RowIndex).Item(0) = Not CBool(dtSaldos.Rows(e.RowIndex).Item(0).ToString)
        End If
    End Sub

    Private Sub dgSaldos_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgSaldos.CellValidated
        If dgSaldos.CurrentCell.ColumnIndex = 0 Then

            With dgSaldos.CurrentRow
                EjecutarSTRSQL(myConn, lblInfo, " update  " & tblSaldos & " set sel  = " & CInt(dgSaldos.CurrentCell.Value) & " " _
                                    & " where " _
                                    & " emision = '" & FormatoFechaMySQL(CDate(.Cells(3).Value.ToString)) & "' and " _
                                    & " vence = '" & FormatoFechaMySQL(CDate(.Cells(4).Value.ToString)) & "' and " _
                                    & " tipomov = '" & CStr(.Cells(2).Value) & "' and " _
                                    & " nummov = '" & CStr(.Cells(1).Value) & "' ")
            End With

        End If

    End Sub

    Private Sub btnCodigoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigoContable.Click
        txtCodigoContable.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables", _
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
          Select tbcProveedor.SelectedTab.Text
            Case "Movimientos CxP"
                If LoginUser(myConn, lblInfo) = "jytsuite" Then
                    With dtMovimientos.Rows(nPosicionMov)
                        Select Case .Item("FORMAPAG")
                            Case "EF"
                                InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, .Item("CAJAPAG"), UltimoCajaMasUno(myConn, lblInfo, .Item("CAJAPAG")), _
                                    CDate(.Item("EMISION").ToString), "CXP", "SA", .Item("COMPROBA"), .Item("FORMAPAG"), _
                                    .Item("NUMPAG"), .Item("NOMPAG"), -1 * ValorNumero(.Item("IMPORTE")), .Item("CODCON"), .Item("CONCEPTO"), "", jytsistema.sFechadeTrabajo, 1, _
                                    "", "", "", jytsistema.sFechadeTrabajo, .Item("CODPRO"), "", "1")
                            Case "CH", "TA", "DP", "TR"
                                InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, CDate(.Item("EMISION").ToString), .Item("NUMPAG"), _
                                    IIf(.Item("FORMAPAG") <> "CH", "ND", "CH"), .Item("NOMPAG"), "", .Item("CONCEPTO"), -1 * ValorNumero(.Item("IMPORTE")), _
                                    "CXP", .Item("COMPROBA"), .Item("BENEFIC"), .Item("COMPROBA"), "0", CDate(.Item("EMISION").ToString), CDate(.Item("EMISION").ToString), _
                                     .Item("TIPDOCCAN"), "", jytsistema.sFechadeTrabajo, "0", .Item("CODPRO"), "")
                        End Select
                    End With
                End If
        End Select
    End Sub

    Private Sub btnExP_Click(sender As System.Object, e As System.EventArgs) Handles btnExP.Click
        Dim nPos As Long
        Select Case tbcProveedor.SelectedIndex
            Case 1
                If nPosicionMov >= 0 Then

                    nPos = nPosicionMov
                    EjecutarSTRSQL(myConn, lblInfo, " update jsprotrapag set remesa = '1' " _
                        & " where  " _
                        & " codpro = '" & txtCodigo.Text & "' and " _
                        & " nummov = '" & dtMovimientos.Rows(nPosicionMov).Item("nummov") & "' and " _
                        & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                        & " id_emp = '" & jytsistema.WorkID & "' ")

                    AbrirMovimientos(txtCodigo.Text)
                    nPosicionMov = IIf(nPos > dtMovimientos.Rows.Count - 1, dtMovimientos.Rows.Count - 1, nPos)
                    AsignaMov(nPosicionMov, False)


                End If

            Case 5
                If nPosicionMovExP >= 0 Then

                    nPos = nPosicionMovExP
                    EjecutarSTRSQL(myConn, lblInfo, " update jsprotrapag set remesa = '' " _
                        & " where  " _
                        & " codpro = '" & txtCodigo.Text & "' and " _
                        & " nummov = '" & dtMovimientosExP.Rows(nPosicionMovExP).Item("nummov") & "' and " _
                        & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                        & " id_emp = '" & jytsistema.WorkID & "' ")

                    AbrirMovimientosExP(txtCodigo.Text)
                    nPosicionMovExP = IIf(nPos > dtMovimientosExP.Rows.Count - 1, dtMovimientosExP.Rows.Count - 1, nPos)
                    AsignaMovExP(nPosicionMovExP, False)

                End If


        End Select

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
End Class