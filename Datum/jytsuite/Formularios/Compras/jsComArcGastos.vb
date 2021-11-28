Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsComArcGastos
    Private Const sModulo As String = "Gastos"
    Private Const lRegion As String = "RibbonButton58"
    Private Const nTabla As String = "tblEncab"
    Private Const nTablaRenglones As String = "tblRenglones_"
    Private Const nTablaIVA As String = "tblIVA"
    Private Const nTablaDescuentos As String = "tblDescuentos"

    Private strSQL As String = " (select a.*, b.nombre from jsproencgas a " _
            & " left join jsprocatpro b on (a.codpro = b.codpro and a.id_emp = b.id_emp) " _
            & " where a.id_emp = '" & jytsistema.WorkID & "' order by a.emision, a.numgas desc ) order by emision, numgas "

    'a.emision >= '" & ft.FormatoFechaMySQL(DateAdd("m", -MesesAtras.i2, jytsistema.sFechadeTrabajo)) & "' and 

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

    Private proveedor As New Vendor()
    Private moneda As New CambioMonedaPlus()
    Private almacen As New SimpleTable()
    Private accountList As New List(Of AccountBase)
    Private interchangeList As New List(Of CambioMonedaPlus)

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long, nPosicionDescuento As Long

    Private CondicionDePago As Integer = 0
    Private TipoCredito As Integer = 0
    Private Caja As String = "00"
    Private FechaVencimiento As Date = jytsistema.sFechadeTrabajo
    Private FormaPago As String = ""
    Private NumeroPago As String = ""
    Private NombrePago As String = ""
    Private Beneficiario As String = ""
    Private FechaDocumentoPago As Date = jytsistema.sFechadeTrabajo

    Private Grupo As String = ""
    Private SubGrupo As String = ""

    Private Eliminados As New ArrayList

    Private Impresa As Integer
    Private CodigoProveedorAnterior As String = ""
    Private NumeroDocumentoAnterior As String = ""
    Private aTipoGasto() As String = {"Deducible", "No deducible"}

    Private Sub jsComArcGastos_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsComArcGastos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub IniciarControles()
        Dim dates As SfDateTimeEdit() = {txtEmision, txtEmisionIVA}
        SetSizeDateObjects(dates)

        InitiateDropDown(Of Vendor)(myConn, cmbProveedor)
        ''Asesores 

        '' Monedas
        interchangeList = GetListaDeMonedasyCambios(myConn, jytsistema.sFechadeTrabajo)
        InitiateDropDownInterchangeCurrency(myConn, cmbMonedas, jytsistema.sFechadeTrabajo)

        '' Cuentas Contables
        accountList = InitiateDropDown(Of AccountBase)(myConn, cmbCC)

        '' Alamacenes y Zonas
        InitiateDropDown(Of SimpleTable)(myConn, cmbAlmacenes, Tipo.Almacenes)
        InitiateDropDown(Of SimpleTable)(myConn, cmbZonaProveedor, Tipo.ZonaProveedor)

    End Sub

    Private Sub AsignarTooltips()
        Dim menus As New List(Of ToolStrip) From {MenuBarra, MenuBarraRenglon, MenuDescuentos}
        AsignarToolTipsMenuBarraToolStrip(menus, "Gasto")
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

        i_modo = movimiento.iConsultar

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtCodigo.Text = ft.muestraCampoTexto(.Item("numgas"))
                txtNumeroSerie.Text = ft.muestraCampoTexto(.Item("serie_numgas"))
                NumeroDocumentoAnterior = .Item("numgas")
                txtEmision.Value = .Item("emision")
                txtVencimiento.Text = ft.FormatoFecha(CDate(.Item("vence").ToString))
                txtEmisionIVA.Value = .Item("emisioniva")
                cmbProveedor.SelectedValue = .Item("codpro")
                CodigoProveedorAnterior = .Item("codpro")
                txtRIF.Text = ft.muestraCampoTexto(.Item("rif"))
                txtReferencia.Text = ft.muestraCampoTexto(.Item("refer"))
                Dim numControl As String = ft.DevuelveScalarCadena(myConn, " select num_control from jsconnumcon where numdoc = '" & .Item("numgas") & "' and " _
                                                                 & " prov_cli = '" & .Item("codpro") & "' and " _
                                                                 & " origen = 'COM' and org = 'GAS' and id_emp = '" & jytsistema.WorkID & "' ")

                txtControl.Text = IIf(numControl <> "0", numControl, "")
                ft.RellenaCombo(aTipoGasto, cmbTipoGasto, .Item("tipogasto"))
                txtComentario.Text = ft.muestraCampoTexto(.Item("comen"))
                cmbZonaProveedor.SelectedValue = .Item("ZONA")
                cmbAlmacenes.SelectedValue = .Item("almacen")
                cmbCC.SelectedValue = .Item("codcon")

                Grupo = .Item("grupo")
                SubGrupo = .Item("subgrupo")

                txtGrupo.Text = ft.DevuelveScalarCadena(myConn, " select nombre from jsprogrugas where codigo = '" & Grupo & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                txtSubgrupo.Text = ft.DevuelveScalarCadena(myConn, " select nombre from jsprogrugas where codigo = '" & SubGrupo & "' and id_emp = '" & jytsistema.WorkID & "'  ")

                tslblPesoT.Text = ft.FormatoCantidad(ft.DevuelveScalarDoble(myConn, "select SUM(PESO) from jsprorengas where numgas = '" & .Item("numgas") & "' and codpro = '" & .Item("codpro") & "' and id_emp = '" & jytsistema.WorkID & "' "))

                txtSubTotal.Text = ft.FormatoNumero(.Item("tot_net"))
                txtDescuentos.Text = ft.FormatoNumero(.Item("descuen"))
                txtCargos.Text = ft.FormatoNumero(.Item("cargos"))
                txtTotalIVA.Text = ft.FormatoNumero(.Item("imp_iva"))
                txtTotal.Text = ft.FormatoNumero(.Item("tot_gas"))

                CondicionDePago = .Item("condpag")
                Dim FormaDePago As String = ""
                If .Item("formapag").ToString.Trim() <> "" Then _
                    FormaDePago = formasDePago.FirstOrDefault(Function(forma) forma.Value = .Item("FORMAPAG")).Text & "  " &
                        .Item("NUMPAG") & " " &
                        IIf(.Item("nompag").ToString.Trim() <> "", ft.DevuelveScalarCadena(myConn, " SELECT NOMBAN from jsbancatban where codban = '" & .Item("nompag") & "' and id_emp = '" & jytsistema.WorkID & "' "), "")

                txtCondicionPago.Text = IIf(.Item("condpag") = 0, "CREDITO", "CONTADO") _
                    & " - Forma de Pago : " & FormaDePago _
                    & IIf(.Item("formapag") <> "EF", "", " CAJA : " & .Item("caja"))

                FechaVencimiento = CDate(.Item("vence").ToString)

                'Impresa = .Item("impresa")

                'Renglones
                AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)

                'Totales
                CalculaTotales()

            End With
        End With
    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroGasto As String, ByVal CodigoProveedor As String)

        strSQLMov = "select * from jsprorengas " _
                            & " where " _
                            & " numgas  = '" & NumeroGasto & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"item", "descrip", "iva", "cantidad", "unidad", "costou", "des_art", "des_pro", "costotot", "estatus", ""}
        Dim aNombres() As String = {"Item", "Descripción", "IVA", "Cant.", "UND", "Costo Unitario", "Desc. Art.", "Desc. Prov.", "Costo Total", "Tipo Renglon", ""}
        Dim aAnchos() As Integer = {90, 300, 30, 70, 45, 100, 50, 50, 120, 60, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda,
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", sFormatoCantidad, "", sFormatoNumero, sFormatoNumero, sFormatoNumero,
                                     sFormatoNumero, "", ""}
        IniciarTabla(dg, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub
    Private Sub AbrirIVA(ByVal NumeroDocumento As String, ByVal CodigoProveedor As String)

        strSQLIVA = "select * from jsproivagas " _
                            & " where " _
                            & " numgas = '" & NumeroDocumento & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by tipoiva "

        ds = DataSetRequery(ds, strSQLIVA, myConn, nTablaIVA, lblInfo)
        dtIVA = ds.Tables(nTablaIVA)

        Dim aCampos() As String = {"tipoiva", "poriva", "baseiva", "impiva"}
        Dim aNombres() As String = {"", "", "", ""}
        Dim aAnchos() As Integer = {15, 45, 95, 90}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha,
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {"", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero}
        IniciarTabla(dgIVA, dtIVA, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False, , 8, False)

        txtTotalIVA.Text = ft.FormatoNumero(ft.DevuelveScalarDoble(myConn, " select SUM(IMPIVA) from jsproivagas where numgas = '" & NumeroDocumentoAnterior & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' group by numgas "))

    End Sub
    Private Sub AbrirDescuentos(ByVal NumeroDocumento As String, ByVal CodigoProveedor As String)

        strSQLDescuentos = "select * from jsprodesgas " _
                            & " where " _
                            & " numgas  = '" & NumeroDocumento & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
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

        txtDescuentos.Text = ft.FormatoNumero(ft.DevuelveScalarDoble(myConn, " select SUM(DESCUENTO) from jsprodesgas where numgas = '" & NumeroDocumentoAnterior & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' group by numgas "))

    End Sub

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)
        If Inicio Then
            txtCodigo.Text = Contador(myConn, lblInfo, Gestion.iCompras, "COMNUMGAS", 4)
        Else
            txtCodigo.Text = ""
        End If
        txtNumeroSerie.Text = ""
        NumeroDocumentoAnterior = txtCodigo.Text
        CodigoProveedorAnterior = ""
        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtControl, cmbProveedor, txtRIF, cmbZonaProveedor, txtComentario,
                            cmbAlmacenes, txtReferencia, cmbCC, txtVencimiento, txtCondicionPago)
        cmbMonedas.SelectedItem = jytsistema.WorkCurrency
        cmbProveedor.SelectedIndex = -1
        cmbAlmacenes.SelectedIndex = 0
        cmbZonaProveedor.SelectedIndex = 0
        cmbCC.SelectedIndex = -1
        ft.RellenaCombo(aTipoGasto, cmbTipoGasto)
        txtEmision.Value = jytsistema.sFechadeTrabajo
        txtEmisionIVA.Value = jytsistema.sFechadeTrabajo
        tslblPesoT.Text = ft.FormatoCantidad(0)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtSubTotal, txtDescuentos, txtCargos, txtTotalIVA, txtTotalIVA)
        txtVencimiento.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtTotal.Text = "0.00"
        CondicionDePago = 0
        Impresa = 0
        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
        AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)
        AbrirDescuentos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon, MenuDescuentos)
        ft.habilitarObjetos(True, True, txtCodigo, txtNumeroSerie, txtComentario, txtEmision, cmbProveedor, txtEmisionIVA,
                         txtRIF, txtControl, cmbZonaProveedor, cmbAlmacenes,
                         txtReferencia, cmbCC, cmbMonedas, btnGrupoSubGrupo, cmbTipoGasto)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtNumeroSerie, txtEmision, txtControl, txtEmisionIVA,
                cmbProveedor, txtComentario, txtRIF, cmbZonaProveedor, cmbMonedas,
                txtVencimiento, txtCondicionPago, txtGrupo, txtSubgrupo, cmbAlmacenes,
                txtReferencia, cmbCC, btnGrupoSubGrupo, cmbTipoGasto)

        ft.habilitarObjetos(False, True, txtDescuentos, txtCargos, MenuDescuentos, txtSubTotal, txtTotalIVA, txtTotal)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then
            If ft.DevuelveScalarEntero(myConn, " select count(*) from jsproencgas " _
                                       & " where " _
                                       & " numgas = '" & txtCodigo.Text & "' and " _
                                       & " codpro = '" & proveedor.Codigo & "' and " _
                                       & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                AgregaYCancela()
            End If
        Else
            If dt.Rows.Count = 0 Then
                IniciarDocumento(False)
            Else
                If dtRenglones.Rows.Count = 0 Then
                    ft.mensajeCritico("DEBE INCLUIR AL MENOS UN ITEM...")
                    Return
                End If
                Me.BindingContext(ds, nTabla).CancelCurrentEdit()
                AsignaTXT(nPosicionEncab)
            End If
        End If

        DesactivarMarco0()
    End Sub
    Private Sub AgregaYCancela()

        ft.Ejecutar_strSQL(myConn, " delete from jsprorengas where numgas = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsproivagas where numgas = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsprodesgas where numgas = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'GAS' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim f As New jsComFormasPago
            f.CondicionPago = CondicionDePago
            f.TipoCredito = TipoCredito
            f.Vencimiento = IIf(CondicionDePago = 0, FechaVencimiento, txtEmisionIVA.Value)
            f.Caja = Caja
            f.FormaPago = FormaPago


            f.Cargar(myConn, "GAS", NumeroDocumentoAnterior,
                     IIf(i_modo = movimiento.iAgregar, True, False),
                     ValorNumero(txtTotal.Text), True)

            If f.DialogResult = Windows.Forms.DialogResult.OK Then
                CondicionDePago = f.CondicionPago
                TipoCredito = f.TipoCredito
                FechaVencimiento = f.Vencimiento
                Caja = f.Caja
                FormaPago = f.FormaPago
                NumeroPago = f.NumeroPago
                NombrePago = f.NombrePago
                Beneficiario = f.Beneficiario
                FechaDocumentoPago = f.FechaDocumentoPago
                GuardarTXT()
                'Imprimir()
            End If

        End If
    End Sub
    Private Sub Imprimir()

        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cGasto, "Gasto",
                 proveedor.Codigo, txtCodigo.Text, txtEmision.Value)
        f = Nothing


    End Sub
    Private Function Validado() As Boolean
        If FechaUltimoBloqueo(myConn, "jsproencgas") >= txtEmision.Value Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If
        If txtCodigo.Text = "" Then
            ft.mensajeCritico("Debe indicar un Número de Gasto válido...")
            Return False
        End If
        If cmbProveedor.SelectedIndex < 0 Then
            ft.mensajeCritico("Debe indicar un proveedor válido...")
            Return False
        End If
        If ft.DevuelveScalarEntero(myConn, " select count(*) from jsproencgas where numgas = '" & txtCodigo.Text & "' and codpro = '" & proveedor.Codigo & "' and id_emp = '" & jytsistema.WorkID & "'  ") > 0 AndAlso
            i_modo = movimiento.iAgregar Then
            ft.mensajeCritico("Número de gasto YA existe para este proveedor ...")
            Return False
        End If
        If txtSubgrupo.Text = "" Then
            ft.mensajeCritico("Debe seleccionar SubGrupo de Gastos válido...")
            Return False
        End If
        If txtGrupo.Text = "" Then
            ft.mensajeCritico("Debe seleccionar Grupo de Gastos válido...")
            Return False
        End If
        If cmbAlmacenes.SelectedIndex < 0 Then
            ft.mensajeCritico("Debe indicar un almacén válido...")
            Return False
        End If
        If dtRenglones.Rows.Count = 0 Then
            ft.mensajeCritico("Debe incluir al menos un ítem...")
            Return False
        End If

        '//////////// VALIDAR SERVICIOS Y MERCANCIAS POR SEPARADO
        Return True
    End Function
    Private Sub GuardarTXT()

        Dim Codigo As String = txtCodigo.Text
        Dim Inserta As Boolean = False
        ft.Ejecutar_strSQL(myConn, " update jsprorengas set codpro = '" & proveedor.Codigo & "', numgas = '" & Codigo & "'  where codpro = '" & CodigoProveedorAnterior & "' and numgas = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " update jsproivagas set codpro = '" & proveedor.Codigo & "', numgas = '" & Codigo & "'  where codpro = '" & CodigoProveedorAnterior & "' and numgas = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " update jsprodesgas set codpro = '" & proveedor.Codigo & "', numgas = '" & Codigo & "'  where codpro = '" & CodigoProveedorAnterior & "' and numgas = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & NumeroDocumentoAnterior & "' and origen = 'GAS' and id_emp = '" & jytsistema.WorkID & "' ")

        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count
        End If

        Dim porcentajeDescuento As Double = (1 - ValorNumero(txtDescuentos.Text) / ValorNumero(txtSubTotal.Text)) * 100

        InsertEditCOMPRASEncabezadoGastos(myConn, lblInfo, Inserta, Codigo, txtNumeroSerie.Text, txtEmision.Value, txtEmisionIVA.Value,
                proveedor.Codigo, proveedor.Nombre, txtRIF.Text, "", txtComentario.Text, txtReferencia.Text,
                cmbAlmacenes.SelectedValue, cmbCC.SelectedValue, Grupo, SubGrupo, ValorNumero(txtSubTotal.Text), porcentajeDescuento,
                ValorNumero(txtDescuentos.Text),
                ValorNumero(txtCargos.Text), "", 0.0, 0.0, ValorNumero(txtTotalIVA.Text), 0.0, 0.0, "", jytsistema.sFechadeTrabajo,
                ValorNumero(txtTotal.Text), FechaVencimiento, CondicionDePago, TipoCredito, FormaPago, NumeroPago, NombrePago,
                Beneficiario, Caja, 0.0, "", 0, 0, 0.0, 0.0, "", jytsistema.sFechadeTrabajo, 0.0, "", jytsistema.sFechadeTrabajo,
                0.0, 0.0, "", "", "", cmbZonaProveedor.SelectedValue,
                cmbTipoGasto.SelectedIndex, Impresa, NumeroDocumentoAnterior, CodigoProveedorAnterior, cmbMonedas.SelectedValue,
                jytsistema.sFechadeTrabajo)

        InsertEditCONTROLNumeroControl(myConn, Codigo, proveedor.Codigo, txtControl.Text, txtEmisionIVA.Value, "COM", "GAS")

        ActualizarMovimientos(Codigo, proveedor.Codigo)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" NUMGAS = '" & Codigo & "' AND CODPRO = '" & proveedor.Codigo & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub ActualizarMovimientos(ByVal NumeroGasto As String, ByVal CodigoProveedor As String)

        '1.- Aplica Descuento Global sobre total Renglón con descuento "costototdes"
        ft.Ejecutar_strSQL(myConn, " update jsprorengas set costototdes = costotot - costotot * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
                                        & " where " _
                                        & " numgas = '" & NumeroGasto & "' and " _
                                        & " renglon > '' and " _
                                        & " item > '' and " _
                                        & " estatus = '0' AND " _
                                        & " aceptado < '2' and " _
                                        & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                        & " id_emp = '" & jytsistema.WorkID & "' ")

        '2.- Elimina movimientos de inventarios anteriores del gasto
        EliminarMovimientosdeInventario(myConn, NumeroGasto, "GAS", lblInfo, , , CodigoProveedor)

        '3.- Actualizar Movimientos de Inventario con los del gasto
        strSQLMov = "select * from jsprorengas " _
                            & " where " _
                            & " numgas  = '" & NumeroGasto & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        For Each dtRow As DataRow In dtRenglones.Rows
            With dtRow
                If .Item("item").ToString.Substring(0, 1) <> "$" Then _
                InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), txtEmision.Value, "EN", NumeroGasto,
                                                     .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("costotot"),
                                                     .Item("costototdes"), "GAS", NumeroGasto, .Item("lote"), proveedor.Codigo,
                                                     .Item("costotot"), .Item("costototdes"), 0, .Item("costotot") - .Item("costototdes"),
                                                     cmbZonaProveedor.SelectedValue, cmbAlmacenes.SelectedValue,
                                                     .Item("renglon"), jytsistema.sFechadeTrabajo)

                ActualizarExistenciasPlus(myConn, .Item("item"))
            End With
        Next

        '4.- Actualizar existencias de renglones eliminados
        For Each aSTR As Object In Eliminados
            ActualizarExistenciasPlus(myConn, aSTR)
        Next

        '5.- Actualizar CxP si es un Gasto a crédito
        If CondicionDePago = CondicionPago.iCredito Then
            ft.Ejecutar_strSQL(myConn, " DELETE from jsprotrapag WHERE " _
                                            & " TIPOMOV = 'FC' AND  " _
                                            & " codpro = '" & CodigoProveedorAnterior & "' and " _
                                            & " NUMMOV = '" & NumeroDocumentoAnterior & "' AND " _
                                            & " ORIGEN = 'GAS' AND " _
                                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                            & " ID_EMP = '" & jytsistema.WorkID & "'  ")

            InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, "FC", NumeroGasto, txtEmision.Value, ft.FormatoHora(Now),
                                FechaVencimiento, txtReferencia.Text, "Gasto N° : " & NumeroGasto, -1 * ValorNumero(txtTotal.Text),
                                ValorNumero(txtTotalIVA.Text), "", "", "", "", "GAS", "", "", "", Caja, NumeroGasto, "0", "", txtEmisionIVA.Value,
                                cmbCC.SelectedValue, "", "", 0.0, 0.0, "", "", "", "", "", "", "0", "0", "0",
                                cmbMonedas.SelectedValue, jytsistema.sFechadeTrabajo)

            ft.Ejecutar_strSQL(myConn, " update jsprotrapag set nummov = '" & NumeroGasto & "', codpro = '" & CodigoProveedor & "' WHERE " _
                                            & " TIPOMOV in ('AB', 'CA', 'NC') AND  " _
                                            & " codpro = '" & CodigoProveedorAnterior & "' and " _
                                            & " NUMMOV = '" & NumeroDocumentoAnterior & "' AND " _
                                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                            & " ID_EMP = '" & jytsistema.WorkID & "'  ")


        Else '6.- Actualizar Caja y Bancos si es un Gasto a contado
            ' 6.1.- Elimina movimientos anteriores de caja y bancos
            ft.Ejecutar_strSQL(myConn, "DELETE FROM jsbantracaj WHERE " _
                        & " ORIGEN = 'GAS' AND " _
                        & " TIPOMOV = 'SA' AND " _
                        & " NUMMOV = '" & NumeroDocumentoAnterior & "' AND " _
                        & " PROV_CLI = '" & CodigoProveedorAnterior & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "'")

            ft.Ejecutar_strSQL(myConn, "DELETE FROM jsbantraban WHERE " _
                        & " ORIGEN = 'GAS' AND " _
                        & " concepto = 'CANC. GASTO N° " & NumeroDocumentoAnterior & "' and " _
                        & " prov_cli = '" & CodigoProveedorAnterior & "' and " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' ")


            Select Case FormaPago
                Case "EF"

                    InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, Caja, UltimoCajaMasUno(myConn, lblInfo, Caja),
                                                FechaDocumentoPago, "GAS", "SA", NumeroGasto, FormaPago,
                                                NumeroPago, NombrePago, -1 * ValorNumero(txtTotal.Text), cmbCC.SelectedValue,
                                                "GASTO N° :" & NumeroGasto, "", jytsistema.sFechadeTrabajo, 1, "", 0, "",
                                                txtEmisionIVA.Value, CodigoProveedor, "", "1", jytsistema.WorkCurrency.Id, DateTime.Now())

                    CalculaSaldoCajaPorFP(myConn, Caja, FormaPago, lblInfo)

                Case "CH", "TA", "TR"

                    InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, FechaDocumentoPago, NumeroPago, FormaPago,
                                                    NombrePago, "", "GASTO N° :" & txtCodigo.Text, -1 * ValorNumero(txtTotal.Text), "GAS", txtCodigo.Text,
                                                    Beneficiario, txtCodigo.Text, "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, "FC",
                                                    "", txtEmisionIVA.Value, "0", proveedor.Codigo, "", jytsistema.WorkCurrency.Id, DateTime.Now())

                    IncluirImpuestoDebitoBancario(myConn, lblInfo, IIf(FormaPago = "CH", FormaPago, "ND"), NombrePago, NumeroPago, FechaDocumentoPago, ValorNumero(txtTotal.Text))

                    CalculaSaldoBanco(myConn, lblInfo, NombrePago, True, jytsistema.sFechadeTrabajo)

                Case "DP"



            End Select

        End If

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

        i_modo = movimiento.iEditar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPAGAS03")) Then
            If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) >= 1 Then
                ActivarMarco0()
            Else
                With dt.Rows(nPosicionEncab)
                    Dim aCamposAdicionales() As String = {"numgas|'" & txtCodigo.Text & "'",
                                                      "codpro|'" & proveedor.Codigo & "'"}
                    If DocumentoBloqueado(myConn, "jsproencgas", aCamposAdicionales) Then
                        ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                    Else
                        If DocumentoPoseeRetencionIVA(myConn, lblInfo, .Item("numgas"), .Item("codpro")) Then
                            ft.mensajeCritico("Esta FACTURA posee RETENCION DE IVA. MODIFICACION NO esta permitida ...")
                        Else

                            If ft.DevuelveScalarCadena(myConn, "SELECT DEPOSITO FROM jsbantracaj " _
                                                  & " WHERE NUMMOV = '" & .Item("NUMGAS") & "' AND " _
                                                  & " ORIGEN = 'GAS' AND TIPOMOV = 'SA' AND " _
                                                  & " PROV_CLI = '" & .Item("CODPRO") & "' AND " _
                                                  & " ID_EMP = '" & jytsistema.WorkID & "' ") = "" Then


                                If ft.DevuelveScalarEntero(myConn, " select count(*) from jsprotrapag " _
                                                              & " where " _
                                                              & " tipomov <> 'FC' and ORIGEN <> 'GAS' AND " _
                                                              & " codpro = '" & .Item("codpro") & "' and " _
                                                              & " nummov = '" & .Item("NUMGAS") & "' and " _
                                                              & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then

                                    ActivarMarco0()
                                Else
                                    If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) = 0 Then
                                        ft.mensajeCritico("Esta FACTURA posee movimientos asociados. MODIFICACION NO esta permitida ...")
                                    Else
                                        ActivarMarco0()
                                    End If
                                End If
                            Else
                                ft.mensajeCritico("Esta FACTURA pertenece a una relacion de caja chica. MODIFICACION NO esta permitida ...")
                            End If

                        End If
                    End If
                End With
            End If
        Else
            ft.mensajeCritico("Factura de gastos NO puede ser modificada...")
        End If
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPAGAS03")) Then

            nPosicionEncab = Me.BindingContext(ds, nTabla).Position

            With dt.Rows(nPosicionEncab)
                Dim aCamposAdicionales() As String = {"numgas|'" & txtCodigo.Text & "'",
                                                      "codpro|'" & proveedor.Codigo & "'"}
                If DocumentoBloqueado(myConn, "jsproencgas", aCamposAdicionales) Then
                    ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                Else

                    If DocumentoPoseeRetencionIVA(myConn, lblInfo, .Item("numgas"), .Item("codpro")) Then
                        ft.mensajeCritico("Esta FACTURA posee RETENCION DE IVA. MODIFICACION NO esta permitida ...")
                    Else

                        If ft.DevuelveScalarCadena(myConn, "SELECT DEPOSITO FROM jsbantracaj " _
                                                   & " WHERE NUMMOV = '" & .Item("NUMGAS") & "' AND " _
                                                   & " ORIGEN = 'GAS' AND TIPOMOV = 'SA' AND " _
                                                   & " PROV_CLI = '" & .Item("CODPRO") & "' AND " _
                                                   & " ID_EMP = '" & jytsistema.WorkID & "' ") = "" Then


                            If ft.DevuelveScalarEntero(myConn, " select count(*) from jsprotrapag " _
                                                          & " where " _
                                                          & " tipomov <> 'FC' and ORIGEN <> 'GAS' AND " _
                                                          & " codpro = '" & .Item("codpro") & "' and " _
                                                          & " nummov = '" & .Item("NUMGAS") & "' and " _
                                                          & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then


                                If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

                                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numgas"))


                                    If .Item("CONDPAG") = 1 And (.Item("FORMAPAG") = "CH" _
                                                                 Or .Item("FORMAPAG") = "TR" _
                                                                 Or .Item("FORMAPAG") = "TA") Then
                                        EliminarImpuestoDebitoBancario(myConn, lblInfo, .Item("NOMPAG"), .Item("NUMPAG"),
                                                                       CDate(.Item("VENCE1").ToString))
                                    End If

                                    For Each dtRow As DataRow In dtRenglones.Rows
                                        With dtRow
                                            Eliminados.Add(.Item("item"))
                                        End With
                                    Next

                                    ft.Ejecutar_strSQL(myConn, " delete from jsproencgas where NUMGAS = '" & txtCodigo.Text & "' AND CODPRO = '" & proveedor.Codigo & "' and ID_EMP = '" & jytsistema.WorkID & "'")
                                    ft.Ejecutar_strSQL(myConn, " delete from jsprorengas where numgas = '" & txtCodigo.Text & "' and codpro = '" & proveedor.Codigo & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                    ft.Ejecutar_strSQL(myConn, " delete from jsprodesgas where numgas = '" & txtCodigo.Text & "' and codpro = '" & proveedor.Codigo & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                    ft.Ejecutar_strSQL(myConn, " delete from jsproivagas where numgas = '" & txtCodigo.Text & "' and codpro = '" & proveedor.Codigo & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                    ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'GAS' and id_emp = '" & jytsistema.WorkID & "' ")
                                    ft.Ejecutar_strSQL(myConn, " DELETE FROM jsbantraban where ORIGEN = 'GAS' AND NUMORG = '" & txtCodigo.Text & "' AND prov_cli = '" & proveedor.Codigo & "' and EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                                    ft.Ejecutar_strSQL(myConn, " DELETE FROM jsbantracaj where nummov = '" & txtCodigo.Text & "' AND prov_cli = '" & proveedor.Codigo & "' and origen = 'GAS' AND tipomov = 'SA' AND ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ")
                                    ft.Ejecutar_strSQL(myConn, " DELETE FROM jsprotrapag where CODPRO = '" & proveedor.Codigo & "' AND TIPOMOV = 'FC' AND NUMMOV = '" & txtCodigo.Text & "' AND ORIGEN = 'GAS' AND NUMORG = '" & txtCodigo.Text & "' and EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")

                                    EliminarMovimientosdeInventario(myConn, txtCodigo.Text, "GAS", lblInfo, , , proveedor.Codigo)

                                    For Each aSTR As Object In Eliminados
                                        ActualizarExistenciasPlus(myConn, aSTR)
                                    Next

                                    ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                                    dt = ds.Tables(nTabla)
                                    If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                                    AsignaTXT(nPosicionEncab)

                                End If
                            Else
                                ft.mensajeCritico("Esta FACTURA posee movimientos asociados. ELIMINACION NO esta permitida ...")
                            End If
                        Else
                            ft.mensajeCritico("Esta FACTURA pertenece a una relacion de caja chica. ELIMINACION NO esta permitida ...")
                        End If

                    End If
                End If
            End With
        Else
            ft.mensajeCritico("Esta Factura de Gastos no se puede modificar...")
        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numgas", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Nombre", "Emisión", "Comentario"}
        Dim Anchos() As Integer = {150, 400, 100, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Gastos...")
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
            Case 9
                e.Value = aTipoRenglon(e.Value)
        End Select
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick,
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub CalculaTotales()

        Dim DocPoseeRetIVA As Boolean = DocumentoPoseeRetencionIVA(myConn, lblInfo, NumeroDocumentoAnterior, CodigoProveedorAnterior)
        If Not DocPoseeRetIVA Then
            txtSubTotal.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsprorengas", "numgas", "costotot", NumeroDocumentoAnterior, 0, CodigoProveedorAnterior))

            AbrirDescuentos(NumeroDocumentoAnterior, CodigoProveedorAnterior)

            txtCargos.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsprorengas", "numgas", "costotot", NumeroDocumentoAnterior, 1, CodigoProveedorAnterior))

            CalculaDescuentoEnRenglones()

            CalculaTotalIVACompras(myConn, lblInfo, CodigoProveedorAnterior, "jsprodesgas", "jsproivagas", "jsprorengas", "numgas", NumeroDocumentoAnterior, "impiva", "costototdes", txtEmision.Value, "costotot")

            AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)

            txtTotal.Text = ft.FormatoNumero(ValorNumero(txtSubTotal.Text) - ValorNumero(txtDescuentos.Text) + ValorNumero(txtCargos.Text) + ValorNumero(txtTotalIVA.Text))

            tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsprorengas", "peso", "numgas", NumeroDocumentoAnterior))
        Else

            AbrirDescuentos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
            CalculaTotalIVACompras(myConn, lblInfo, CodigoProveedorAnterior, "jsprodesgas", "jsproivagas", "jsprorengas", "numgas", NumeroDocumentoAnterior, "impiva", "costototdes", txtEmision.Value, "costotot")
            AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)
            txtTotal.Text = ft.FormatoNumero(ValorNumero(txtSubTotal.Text) - ValorNumero(txtDescuentos.Text) + ValorNumero(txtCargos.Text) + ValorNumero(txtTotalIVA.Text))
            tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsprorengas", "peso", "numgas", NumeroDocumentoAnterior))

        End If

    End Sub
    Private Sub CalculaDescuentoEnRenglones()

        ft.Ejecutar_strSQL(myConn, " update jsprorengas set costototdes = costotot - costotot * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
            & " where " _
            & " NUMGAS = '" & NumeroDocumentoAnterior & "' and " _
            & " renglon > '' and " _
            & " item > '' and " _
            & " ESTATUS = '0' AND " _
            & " ACEPTADO < '2' and " _
            & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
            & " ID_EMP = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        If proveedor.Codigo <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "GAS", NumeroDocumentoAnterior, txtEmision.Value, cmbAlmacenes.SelectedValue,
                      , , , , , , , , , , proveedor.Codigo, , CodigoProveedorAnterior)
            nPosicionRenglon = f.Apuntador
            AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        If dtRenglones.Rows.Count > 0 Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Editar(myConn, ds, dtRenglones, "GAS", NumeroDocumentoAnterior, txtEmision.Value, cmbAlmacenes.SelectedValue, , ,
                     IIf(dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position).Item("item").ToString.Substring(0, 1) = "$", True, False),
                     , , , , , , , proveedor.Codigo, , CodigoProveedorAnterior)
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

                    Dim aCamposDel() As String = {"numgas", "codpro", "item", "renglon", "id_emp"}
                    Dim aStringsDel() As String = {NumeroDocumentoAnterior, CodigoProveedorAnterior, .Item("item"), .Item("renglon"), jytsistema.WorkID}

                    ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & NumeroDocumentoAnterior & "' and " _
                           & " origen = 'GAS' and item = '" & .Item("item") & "' and renglon = '" & .Item("renglon") & "' and " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsprorengas", strSQLMov, aCamposDel, aStringsDel,
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
        f.Buscar(dtRenglones, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaRenglones).Position, "Renglones de gastos...")
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
    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dgIVA.CellFormatting
        If e.ColumnIndex = 1 Then e.Value = ft.FormatoNumero(e.Value) & "%"
    End Sub
    Private Sub btnAgregaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaDescuento.Click
        Dim f As New jsGenArcDescuentosCompras
        f.Agregar(myConn, ds, dtDescuentos, "jsprodesgas", "numgas", NumeroDocumentoAnterior, sModulo, CodigoProveedorAnterior, 1, txtEmision.Value, ValorNumero(txtSubTotal.Text))
        CalculaTotales()
        f = Nothing
    End Sub
    Private Sub btnEliminaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaDescuento.Click
        If nPosicionDescuento >= 0 Then
            With dtDescuentos.Rows(nPosicionDescuento)
                Dim aCamposDel() As String = {"numgas", "codpro", "renglon", "id_emp"}
                Dim aStringsDel() As String = {NumeroDocumentoAnterior, CodigoProveedorAnterior, .Item("renglon"), jytsistema.WorkID}
                nPosicionDescuento = EliminarRegistros(myConn, lblInfo, ds, nTablaDescuentos, "jsprodesgas", strSQLDescuentos, aCamposDel, aStringsDel,
                                                      Me.BindingContext(ds, nTablaDescuentos).Position, True)
            End With
            CalculaTotales()
        End If
    End Sub
    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If proveedor.Codigo <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "GAS", NumeroDocumentoAnterior, txtEmision.Value,
                      cmbAlmacenes.SelectedValue, , , True, , , , , , , , , , CodigoProveedorAnterior)
            nPosicionRenglon = f.Apuntador
            AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Private Sub btnTransporte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrupoSubGrupo.Click
        Dim f As New jsComArcGrupoSubgrupo
        f.Grupo0 = Grupo
        f.Grupo1 = SubGrupo
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        Grupo = f.Grupo0
        SubGrupo = f.Grupo1
        If Grupo <> "" Then
            txtGrupo.Text = ft.DevuelveScalarCadena(myConn, " select nombre from jsprogrugas where codigo = " & Grupo & " and id_emp = '" & jytsistema.WorkID & "' ")
            If SubGrupo <> "" Then
                txtSubgrupo.Text = ft.DevuelveScalarCadena(myConn, " select nombre from jsprogrugas where codigo = " & SubGrupo & " and id_emp = '" & jytsistema.WorkID & "' ")
            Else
                txtSubgrupo.Text = ""
            End If
        Else
            txtGrupo.Text = ""
        End If
        f = Nothing
    End Sub

    Private Sub dgDescuentos_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgDescuentos.RowHeaderMouseClick,
      dgDescuentos.CellMouseClick
        Me.BindingContext(ds, nTablaDescuentos).Position = e.RowIndex
        nPosicionDescuento = e.RowIndex
    End Sub

    Private Sub cmbProveedor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbProveedor.SelectedIndexChanged
        If proveedor.Codigo <> "" Then
            proveedor = cmbProveedor.SelectedItem
            Dim FormaDePagoCliente As String = proveedor.FormaDePago
            Dim mTotalFac As Double = ValorNumero(txtTotal.Text)
            If i_modo = movimiento.iAgregar Then
                mTotalFac = 0.0
                FechaVencimiento = DateAdd(DateInterval.Day, DiasCreditoAlVencimiento(myConn, FormaDePagoCliente), CDate(txtEmision.Value.ToString))
                CondicionDePago = CondicionPagoProveedorCliente(myConn, lblInfo, FormaDePagoCliente)
                cmbZonaProveedor.SelectedValue = proveedor.Zona
                txtRIF.Text = proveedor.Rif
            End If
        End If

    End Sub

    Private Sub txtEmision_ValueChanged(sender As Object, e As Events.DateTimeValueChangedEventArgs) Handles txtEmision.ValueChanged
        txtEmisionIVA.Value = txtEmision.Value
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

End Class