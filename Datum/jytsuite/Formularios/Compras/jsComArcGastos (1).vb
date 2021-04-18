Imports MySql.Data.MySqlClient
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

    'a.emision >= '" & FormatoFechaMySQL(DateAdd("m", -MesesAtras.i2, jytsistema.sFechadeTrabajo)) & "' and 

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
    Private Sub jsComArcGastos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
            ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo Gasto")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> Gasto")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> Gasto")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> Gasto")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primer</B> Gasto")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a Gasto <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a Gasto <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>último Gasto</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> Gasto")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnDuplicar, "<B>Duplicar</B> este Gasto")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en Gasto")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón en Gasto")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón en Gasto")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en Gasto")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón en Gasto")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón en Gasto")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> en Gasto")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la Gasto")

        'Menu Barra Descuento 
        C1SuperTooltip1.SetToolTip(btnAgregaDescuento, "<B>Agrega </B> descuento global a Gasto")
        C1SuperTooltip1.SetToolTip(btnEliminaDescuento, "<B>Elimina</B> descuento global de Gasto")



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

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        i_modo = movimiento.iConsultar

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtCodigo.Text = MuestraCampoTexto(.Item("numgas"))
                txtNumeroSerie.Text = MuestraCampoTexto(.Item("serie_numgas"))
                NumeroDocumentoAnterior = .Item("numgas")
                txtEmision.Text = FormatoFecha(CDate(.Item("emision").ToString))
                txtVencimiento.Text = FormatoFecha(CDate(.Item("vence").ToString))
                txtEmisionIVA.Text = FormatoFecha(CDate(.Item("emisioniva").ToString))
                txtProveedor.Text = MuestraCampoTexto(.Item("codpro"))
                CodigoProveedorAnterior = .Item("codpro")
                txtRIF.Text = MuestraCampoTexto(.Item("rif"))
                txtReferencia.Text = MuestraCampoTexto(.Item("refer"))
                Dim numControl As String = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select num_control from jsconnumcon where numdoc = '" & .Item("numgas") & "' and " _
                                                                 & " prov_cli = '" & .Item("codpro") & "' and " _
                                                                 & " origen = 'COM' and org = 'GAS' and id_emp = '" & jytsistema.WorkID & "' ")

                txtControl.Text = IIf(numControl <> "0", numControl, "")
                RellenaCombo(aTipoGasto, cmbTipoGasto, .Item("tipogasto"))
                txtComentario.Text = MuestraCampoTexto(.Item("comen"))
                txtZona.Text = MuestraCampoTexto(.Item("ZONA"))
                txtAlmacen.Text = MuestraCampoTexto(.Item("almacen"))
                txtCodigoContable.Text = MuestraCampoTexto(.Item("codcon"))

                Grupo = .Item("grupo")
                SubGrupo = .Item("subgrupo")

                txtGrupo.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select nombre from jsprogrugas where codigo = '" & Grupo & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                txtSubgrupo.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select nombre from jsprogrugas where codigo = '" & SubGrupo & "' and id_emp = '" & jytsistema.WorkID & "'  ")

                tslblPesoT.Text = FormatoCantidad(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, "select if( sum(peso) is null, 0.000, sum(peso)) from jsprorengas where numgas = '" & .Item("numgas") & "' and codpro = '" & .Item("codpro") & "' and id_emp = '" & jytsistema.WorkID & "' ")))

                txtSubTotal.Text = FormatoNumero(.Item("tot_net"))
                txtDescuentos.Text = FormatoNumero(.Item("descuen"))
                txtCargos.Text = FormatoNumero(.Item("cargos"))
                txtTotalIVA.Text = FormatoNumero(.Item("imp_iva"))
                txtTotal.Text = FormatoNumero(.Item("tot_gas"))



                CondicionDePago = .Item("condpag")
                Dim FormaDePago As String = ""
                If .Item("formapag").ToString.Trim() <> "" Then _
                    FormaDePago = aFormaPago(InArray(aFormaPagoAbreviada, .Item("FORMAPAG")) - 1) & "  " & _
                        .Item("NUMPAG") & " " & _
                        IIf(.Item("nompag").ToString.Trim() <> "", CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, " SELECT NOMBAN from jsbancatban where codban = '" & .Item("nompag") & "' and id_emp = '" & jytsistema.WorkID & "' ")), "")

                txtCondicionPago.Text = IIf(.Item("condpag") = 0, "CREDITO", "CONTADO") & " - Forma de Pago : " & FormaDePago

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
        Dim aAnchos() As Long = {90, 300, 30, 70, 45, 100, 50, 50, 120, 60, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", sFormatoCantidad, "", sFormatoNumero, sFormatoNumero, sFormatoNumero, _
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
        Dim aAnchos() As Long = {15, 45, 95, 90}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {"", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero}
        IniciarTabla(dgIVA, dtIVA, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False, , 8, False)

        txtTotalIVA.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(impiva) from jsproivagas where numgas = '" & NumeroDocumentoAnterior & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' group by numgas ")))

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
        Dim aAnchos() As Long = {60, 45, 60}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {"", sFormatoNumero, sFormatoNumero}
        IniciarTabla(dgDescuentos, dtDescuentos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False, , 8, False)

        txtDescuentos.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(descuento) from jsprodesgas where numgas = '" & NumeroDocumentoAnterior & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' group by numgas ")))

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

        IniciarTextoObjetos(FormatoItemListView.iCadena, txtControl, txtProveedor, txtNombreProveedor, txtRIF, txtZona, txtSubgrupo, txtComentario, _
                            txtAlmacen, txtReferencia, txtCodigoContable, txtVencimiento, txtCondicionPago)

        RellenaCombo(aTipoGasto, cmbTipoGasto)

        Dim nAlmacen As String = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, "SELECT codalm FROM jsmercatalm WHERE id_emp = '" & jytsistema.WorkID & "' ORDER BY codalm LIMIT 1"))
        If nAlmacen <> "0" Then txtAlmacen.Text = nAlmacen

        txtEmision.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        txtEmisionIVA.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        tslblPesoT.Text = FormatoCantidad(0)
        IniciarTextoObjetos(FormatoItemListView.iNumero, txtSubTotal, txtDescuentos, txtCargos, txtTotalIVA, txtTotalIVA)
        txtVencimiento.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        txtTotal.Text = "0.00"

        CondicionDePago = 0

        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", 0, 0)
        AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
        AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)
        AbrirDescuentos(NumeroDocumentoAnterior, CodigoProveedorAnterior)


    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        HabilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon, MenuDescuentos)
        HabilitarObjetos(True, True, txtCodigo, txtNumeroSerie, txtComentario, btnEmision, btnProveedor, txtProveedor, btnEmisionIVA, _
                         txtRIF, txtControl, txtZona, btnZona, btnEmision, txtAlmacen, btnAlmacen, _
                         txtReferencia, btnCodigoContable, btnGrupoSubGrupo, cmbTipoGasto)

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        HabilitarObjetos(False, True, txtCodigo, txtNumeroSerie, txtEmision, btnEmision, txtControl, txtEmisionIVA, btnEmisionIVA, _
                txtProveedor, txtNombreProveedor, btnProveedor, txtComentario, txtRIF, txtZona, btnZona, txtNombreZona, _
                txtVencimiento, txtCondicionPago, txtGrupo, txtSubgrupo, txtAlmacen, btnZona, btnAlmacen, _
                txtNombreAlmacen, txtReferencia, txtCodigoContable, btnCodigoContable, btnGrupoSubGrupo, cmbTipoGasto)

        HabilitarObjetos(False, True, txtDescuentos, txtCargos, MenuDescuentos, txtSubTotal, txtTotalIVA, txtTotal)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then AgregaYCancela()

        If dt.Rows.Count = 0 Then
            IniciarDocumento(False)
        Else
            If dtRenglones.Rows.Count = 0 Then
                MensajeCritico(lblInfo, "DEBE INCLUIR AL MENOS UN ITEM...")
                Return
            End If
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            AsignaTXT(nPosicionEncab)
        End If

        DesactivarMarco0()
    End Sub
    Private Sub AgregaYCancela()

        EjecutarSTRSQL(myConn, lblInfo, " delete from jsprorengas where numgas = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsproivagas where numgas = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsprodesgas where numgas = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'GAS' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim f As New jsComFormasPago
            f.CondicionPago = CondicionDePago
            f.TipoCredito = TipoCredito
            f.Vencimiento = IIf(CondicionDePago = 0, FechaVencimiento, CDate(txtEmision.Text))
            f.Caja = Caja
            f.FormaPago = FormaPago


            f.Cargar(myConn, "GAS", NumeroDocumentoAnterior, IIf(i_modo = movimiento.iAgregar, True, False), ValorNumero(txtTotal.Text), True)

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
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cGasto, "Gasto", _
                 txtProveedor.Text, txtCodigo.Text, CDate(txtEmision.Text))
        f = Nothing


    End Sub
    Private Function Validado() As Boolean

        If txtCodigo.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un Número de Gasto válido...")
            Return False
        End If

        If txtNombreProveedor.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un proveedor válido...")
            Return False
        End If

        If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(*) from jsproencgas where numgas = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "'  ")) > 0 AndAlso _
            i_modo = movimiento.iAgregar Then
            MensajeCritico(lblInfo, "Número de gasto YA existe para este proveedor ...")
            Return False
        End If

        If txtSubgrupo.Text = "" Then
            MensajeCritico(lblInfo, "Debe seleccionar SubGrupo de Gastos válido...")
            Return False
        End If

        If txtGrupo.Text = "" Then
            MensajeCritico(lblInfo, "Debe seleccionar Grupo de Gastos válido...")
            Return False
        End If

        If txtNombreAlmacen.Text = "" AndAlso CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(*) from jsprorengas " _
                                                                    & " where " _
                                                                    & " substring(item,1,1) <> '$' and  " _
                                                                    & " codpro = '" & txtProveedor.Text & "' and " _
                                                                    & " numgas = '" & txtCodigo.Text & "' and " _
                                                                    & " id_emp = '" & jytsistema.WorkID & "'")) > 0 Then

            MensajeCritico(lblInfo, "Debe indicar un almacén válido...")
            Return False
        End If


        If dtRenglones.Rows.Count = 0 Then
            MensajeCritico(lblInfo, "Debe incluir al menos un ítem...")
            Return False
        End If

        '//////////// VALIDAR SERVICIOS Y MERCANCIAS POR SEPARADO



        Return True

    End Function
    Private Sub GuardarTXT()

        Dim Codigo As String = txtCodigo.Text
        Dim Inserta As Boolean = False


        EjecutarSTRSQL(myConn, lblInfo, " update jsprorengas set codpro = '" & txtProveedor.Text & "', numgas = '" & Codigo & "'  where codpro = '" & CodigoProveedorAnterior & "' and numgas = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " update jsproivagas set codpro = '" & txtProveedor.Text & "', numgas = '" & Codigo & "'  where codpro = '" & CodigoProveedorAnterior & "' and numgas = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " update jsprodesgas set codpro = '" & txtProveedor.Text & "', numgas = '" & Codigo & "'  where codpro = '" & CodigoProveedorAnterior & "' and numgas = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & NumeroDocumentoAnterior & "' and origen = 'GAS' and id_emp = '" & jytsistema.WorkID & "' ")

        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count
        End If

        Dim porcentajeDescuento As Double = (1 - ValorNumero(txtDescuentos.Text) / ValorNumero(txtSubTotal.Text)) * 100

        InsertEditCOMPRASEncabezadoGastos(myConn, lblInfo, Inserta, Codigo, txtNumeroSerie.Text, CDate(txtEmision.Text), CDate(txtEmisionIVA.Text), _
                txtProveedor.Text, txtNombreProveedor.Text, txtRIF.Text, "", txtComentario.Text, txtReferencia.Text, txtAlmacen.Text, _
                txtCodigoContable.Text, Grupo, SubGrupo, ValorNumero(txtSubTotal.Text), porcentajeDescuento, ValorNumero(txtDescuentos.Text), _
                ValorNumero(txtCargos.Text), "", 0.0, 0.0, ValorNumero(txtTotalIVA.Text), 0.0, 0.0, "", jytsistema.sFechadeTrabajo, _
                ValorNumero(txtTotal.Text), FechaVencimiento, CondicionDePago, TipoCredito, FormaPago, NumeroPago, NombrePago, _
                Beneficiario, Caja, 0.0, "", 0, 0, 0.0, 0.0, "", jytsistema.sFechadeTrabajo, 0.0, "", jytsistema.sFechadeTrabajo, _
                0.0, 0.0, "", "", "", txtZona.Text, cmbTipoGasto.SelectedIndex, Impresa, NumeroDocumentoAnterior, CodigoProveedorAnterior)

        InsertEditCONTROLNumeroControl(myConn, lblInfo, Inserta, Codigo, txtProveedor.Text, txtControl.Text, CDate(txtEmisionIVA.Text), "COM", "GAS", NumeroDocumentoAnterior, CodigoProveedorAnterior)

        ActualizarMovimientos(Codigo, txtProveedor.Text)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" NUMGAS = '" & Codigo & "' AND CODPRO = '" & txtProveedor.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub ActualizarMovimientos(ByVal NumeroGasto As String, ByVal CodigoProveedor As String)

        '1.- Aplica Descuento Global sobre total Renglón con descuento "costototdes"
        EjecutarSTRSQL(myConn, lblInfo, " update jsprorengas set costototdes = costotot - costotot * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
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
                If .Item("item").ToString.Substring(1, 1) <> "$" Then _
                InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(txtEmision.Text), "EN", NumeroGasto, _
                                                     .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("costotot"), _
                                                     .Item("costototdes"), "GAS", NumeroGasto, .Item("lote"), txtProveedor.Text, _
                                                     .Item("costotot"), .Item("costototdes"), 0, .Item("costotot") - .Item("costototdes"), txtZona.Text, _
                                                     txtAlmacen.Text, .Item("renglon"), jytsistema.sFechadeTrabajo)

                ActualizarExistenciasPlus(myConn, .Item("item"))
            End With
        Next

        '4.- Actualizar existencias de renglones eliminados
        For Each aSTR As Object In Eliminados
            ActualizarExistenciasPlus(myConn, aSTR)
        Next

        '5.- Actualizar CxP si es un Gasto a crédito
        If CondicionDePago = CondicionPago.iCredito Then
            EjecutarSTRSQL(myConn, lblInfo, " DELETE from jsprotrapag WHERE " _
                                            & " TIPOMOV = 'FC' AND  " _
                                            & " codpro = '" & CodigoProveedorAnterior & "' and " _
                                            & " NUMMOV = '" & NumeroDocumentoAnterior & "' AND " _
                                            & " ORIGEN = 'GAS' AND " _
                                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                            & " ID_EMP = '" & jytsistema.WorkID & "'  ")

            InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, "FC", NumeroGasto, CDate(txtEmision.Text), FormatoHora(Now), _
                                FechaVencimiento, txtReferencia.Text, "Gasto N° : " & NumeroGasto, -1 * ValorNumero(txtTotal.Text), _
                                ValorNumero(txtTotalIVA.Text), "", "", "", "", "GAS", "", "", "", Caja, NumeroGasto, "0", "", jytsistema.sFechadeTrabajo, _
                                txtCodigoContable.Text, "", "", 0.0, 0.0, "", "", "", "", "", "", "0", "0", "0")

            EjecutarSTRSQL(myConn, lblInfo, " update jsprotrapag set nummov = '" & NumeroGasto & "', codpro = '" & CodigoProveedor & "' WHERE " _
                                            & " TIPOMOV in ('AB', 'CA', 'NC') AND  " _
                                            & " codpro = '" & CodigoProveedorAnterior & "' and " _
                                            & " NUMMOV = '" & NumeroDocumentoAnterior & "' AND " _
                                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                            & " ID_EMP = '" & jytsistema.WorkID & "'  ")


        Else '6.- Actualizar Caja y Bancos si es un Gasto a contado
            ' 6.1.- Elimina movimientos anteriores de caja y bancos
            EjecutarSTRSQL(myConn, lblInfo, "DELETE FROM jsbantracaj WHERE " _
                        & " ORIGEN = 'GAS' AND " _
                        & " TIPOMOV = 'SA' AND " _
                        & " NUMMOV = '" & NumeroDocumentoAnterior & "' AND " _
                        & " PROV_CLI = '" & CodigoProveedorAnterior & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "'")

            EjecutarSTRSQL(myConn, lblInfo, "DELETE FROM jsbantraban WHERE " _
                        & " ORIGEN = 'GAS' AND " _
                        & " concepto = 'CANC. GASTO N° " & NumeroDocumentoAnterior & "' and " _
                        & " prov_cli = '" & CodigoProveedorAnterior & "' and " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' ")


            Select Case FormaPago
                Case "EF"

                    InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, Caja, UltimoCajaMasUno(myConn, lblInfo, Caja), _
                                                FechaDocumentoPago, "GAS", "SA", NumeroGasto, FormaPago, _
                                                NumeroPago, NombrePago, -1 * ValorNumero(txtTotal.Text), txtCodigoContable.Text, _
                                                "GASTO N° :" & NumeroGasto, "", jytsistema.sFechadeTrabajo, 1, "", 0, "", _
                                                jytsistema.sFechadeTrabajo, CodigoProveedor, "", "1")

                    CalculaSaldoCajaPorFP(myConn, Caja, FormaPago, lblInfo)

                Case "CH"

                    InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, FechaDocumentoPago, NumeroPago, FormaPago, _
                                                    NombrePago, "", "GASTO N° :" & txtCodigo.Text, -1 * ValorNumero(txtTotal.Text), "GAS", txtCodigo.Text, _
                                                    Beneficiario, txtCodigo.Text, "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, "FC", _
                                                    "", CDate(txtEmision.Text), "0", txtProveedor.Text, "")

                    CalculaSaldoBanco(myConn, lblInfo, NombrePago, True, jytsistema.sFechadeTrabajo)

                Case "DP"



            End Select

        End If

    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        MensajeEtiqueta(lblInfo, " Indique comentario ... ", TipoMensaje.iInfo)
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
        If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) >= 1 Then
            ActivarMarco0()
        Else
            With dt.Rows(nPosicionEncab)
                If DocumentoPoseeRetencionIVA(myConn, lblInfo, .Item("numgas"), .Item("codpro")) Then
                    MensajeCritico(lblInfo, "Esta FACTURA posee RETENCION DE IVA. MODIFICACION NO esta permitida ...")
                Else
                    If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(*) from jsprotrapag " _
                                                  & " where " _
                                                  & " tipomov <> 'FC' and ORIGEN <> 'GAS' AND " _
                                                  & " codpro = '" & .Item("codpro") & "' and " _
                                                  & " nummov = '" & .Item("NUMGAS") & "' and " _
                                                  & " id_emp = '" & jytsistema.WorkID & "' ")) = 0 Then

                        ActivarMarco0()
                    Else
                        If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) = 0 Then
                            MensajeCritico(lblInfo, "Esta FACTURA posee movimientos asociados. MODIFICACION NO esta permitida ...")
                        Else
                            ActivarMarco0()
                        End If
                    End If
                End If
            End With
        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        'If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA01")) Then
        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position

        With dt.Rows(nPosicionEncab)
            If DocumentoPoseeRetencionIVA(myConn, lblInfo, .Item("numgas"), .Item("codpro")) Then
                MensajeCritico(lblInfo, "Esta FACTURA posee RETENCION DE IVA. MODIFICACION NO esta permitida ...")
            Else
                If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(*) from jsprotrapag " _
                                              & " where " _
                                              & " tipomov <> 'FC' and ORIGEN <> 'GAS' AND " _
                                              & " codpro = '" & .Item("codpro") & "' and " _
                                              & " nummov = '" & .Item("NUMGAS") & "' and " _
                                              & " id_emp = '" & jytsistema.WorkID & "' ")) = 0 Then


                    sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")

                    If sRespuesta = MsgBoxResult.Yes Then

                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numgas"))

                        For Each dtRow As DataRow In dtRenglones.Rows
                            With dtRow
                                Eliminados.Add(.Item("item"))
                            End With
                        Next

                        EjecutarSTRSQL(myConn, lblInfo, " delete from jsproencgas where NUMGAS = '" & txtCodigo.Text & "' AND CODPRO = '" & txtProveedor.Text & "' and ID_EMP = '" & jytsistema.WorkID & "'")
                        EjecutarSTRSQL(myConn, lblInfo, " delete from jsprorengas where numgas = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " delete from jsprodesgas where numgas = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " delete from jsproivagas where numgas = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'GAS' and id_emp = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " DELETE FROM jsbantraban where ORIGEN = 'GAS' AND NUMORG = '" & txtCodigo.Text & "' AND prov_cli = '" & txtProveedor.Text & "' and EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " DELETE FROM jsbantracaj where nummov = '" & txtCodigo.Text & "' AND prov_cli = '" & txtProveedor.Text & "' and origen = 'GAS' AND tipomov = 'SA' AND ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " DELETE FROM jsprotrapag where CODPRO = '" & txtProveedor.Text & "' AND TIPOMOV = 'FC' AND NUMMOV = '" & txtCodigo.Text & "' AND ORIGEN = 'GAS' AND NUMORG = '" & txtCodigo.Text & "' and EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                        EliminarMovimientosdeInventario(myConn, txtCodigo.Text, "GAS", lblInfo, , , txtProveedor.Text)

                        For Each aSTR As Object In Eliminados
                            ActualizarExistenciasPlus(myConn, aSTR)
                        Next

                        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                        dt = ds.Tables(nTabla)
                        If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                        AsignaTXT(nPosicionEncab)

                    End If
                Else
                    MensajeCritico(lblInfo, "Esta FACTURA posee movimientos asociados. ELIMINACION NO esta permitida ...")
                End If
            End If
        End With

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numgas", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Nombre", "Emisión", "Comentario"}
        Dim Anchos() As Long = {150, 400, 100, 150}
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
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "ItemsRenglon", "lblItemsRenglon", e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub CalculaTotales()

        Dim DocPoseeRetIVA As Boolean = DocumentoPoseeRetencionIVA(myConn, lblInfo, NumeroDocumentoAnterior, CodigoProveedorAnterior)
        If Not DocPoseeRetIVA Then
            txtSubTotal.Text = FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsprorengas", "numgas", "costotot", NumeroDocumentoAnterior, 0, CodigoProveedorAnterior))

            AbrirDescuentos(NumeroDocumentoAnterior, CodigoProveedorAnterior)

            txtCargos.Text = FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsprorengas", "numgas", "costotot", NumeroDocumentoAnterior, 1, CodigoProveedorAnterior))

            CalculaDescuentoEnRenglones()

            CalculaTotalIVACompras(myConn, lblInfo, CodigoProveedorAnterior, "jsprodesgas", "jsproivagas", "jsprorengas", "numgas", NumeroDocumentoAnterior, "impiva", "costototdes", CDate(txtEmision.Text), "costotot")

            AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)

            txtTotal.Text = FormatoNumero(ValorNumero(txtSubTotal.Text) - ValorNumero(txtDescuentos.Text) + ValorNumero(txtCargos.Text) + ValorNumero(txtTotalIVA.Text))

            tslblPesoT.Text = FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsprorengas", "peso", "numgas", NumeroDocumentoAnterior))
        Else

            AbrirDescuentos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
            CalculaTotalIVACompras(myConn, lblInfo, CodigoProveedorAnterior, "jsprodesgas", "jsproivagas", "jsprorengas", "numgas", NumeroDocumentoAnterior, "impiva", "costototdes", CDate(txtEmision.Text), "costotot")
            AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)
            txtTotal.Text = FormatoNumero(ValorNumero(txtSubTotal.Text) - ValorNumero(txtDescuentos.Text) + ValorNumero(txtCargos.Text) + ValorNumero(txtTotalIVA.Text))
            tslblPesoT.Text = FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsprorengas", "peso", "numgas", NumeroDocumentoAnterior))

        End If

    End Sub
    Private Sub CalculaDescuentoEnRenglones()

        EjecutarSTRSQL_Scalar(myConn, lblInfo, " update jsprorengas set costototdes = costotot - costotot * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
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
        If txtNombreProveedor.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "GAS", NumeroDocumentoAnterior, CDate(txtEmision.Text), txtAlmacen.Text, _
                      , , , , , , , , , , txtProveedor.Text, , CodigoProveedorAnterior)
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
            f.Editar(myConn, ds, dtRenglones, "GAS", NumeroDocumentoAnterior, CDate(txtEmision.Text), txtAlmacen.Text, , , _
                     IIf(dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position).Item("item").ToString.Substring(1, 1) = "$", True, False), _
                     , , , , , , , txtProveedor.Text, , CodigoProveedorAnterior)
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
            sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
            If sRespuesta = MsgBoxResult.Yes Then
                With dtRenglones.Rows(nPosicionRenglon)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("item"))

                    Eliminados.Add(.Item("item"))

                    Dim aCamposDel() As String = {"numgas", "codpro", "item", "renglon", "id_emp"}
                    Dim aStringsDel() As String = {NumeroDocumentoAnterior, CodigoProveedorAnterior, .Item("item"), .Item("renglon"), jytsistema.WorkID}

                    EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & NumeroDocumentoAnterior & "' and " _
                           & " origen = 'GAS' and item = '" & .Item("item") & "' and renglon = '" & .Item("renglon") & "' and " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsprorengas", strSQLMov, aCamposDel, aStringsDel, _
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
        Dim Anchos() As Long = {140, 350}
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

    Private Sub btnEmision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmision.Click
        txtEmision.Text = SeleccionaFecha(CDate(txtEmision.Text), Me, sender)
    End Sub

    Private Sub btnAsesor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZona.Click
        txtZona.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iZonaProveedor) & "' and id_emp = '" & jytsistema.WorkID & "'  order by 1 ", "Zonas Proveedores", _
                                                txtZona.Text)
    End Sub

    Private Sub txtCliente_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProveedor.TextChanged
        If txtProveedor.Text <> "" Then
            Dim aFld() As String = {"codpro", "id_emp"}
            Dim aStr() As String = {txtProveedor.Text, jytsistema.WorkID}
            Dim FormaDePagoCliente As String = qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "formapago")
            txtNombreProveedor.Text = qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "nombre")
            Dim mTotalFac As Double = ValorNumero(txtTotal.Text)
            If i_modo = movimiento.iAgregar Then
                mTotalFac = 0.0
                FechaVencimiento = DateAdd(DateInterval.Day, DiasCreditoAlVencimiento(myConn, FormaDePagoCliente), CDate(txtEmision.Text))
                CondicionDePago = CondicionPagoProveedorCliente(myConn, lblInfo, FormaDePagoCliente)
                If qFound(myConn, lblInfo, "jsprocatpro", aFld, aStr) Then
                    txtZona.Text = qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "zona")
                    If qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "RIF") <> "" Then txtRIF.Text = qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "RIF")
                End If

            End If

        End If
    End Sub

    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dgIVA.CellFormatting
        If e.ColumnIndex = 1 Then e.Value = FormatoNumero(e.Value) & "%"
    End Sub

    Private Sub txtAsesor_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtZona.TextChanged
        txtNombreZona.Text = IIf(txtZona.Text <> "", EjecutarSTRSQL_Scalar(myConn, lblInfo, " select descrip from jsconctatab where codigo = '" & txtZona.Text & "' and modulo = '" & FormatoTablaSimple(Modulo.iZonaProveedor) & "' and id_emp = '" & jytsistema.WorkID & "' "), "")
    End Sub

    Private Sub btnAgregaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaDescuento.Click
        Dim f As New jsGenArcDescuentosCompras
        f.Agregar(myConn, ds, dtDescuentos, "jsprodesgas", "numgas", NumeroDocumentoAnterior, sModulo, CodigoProveedorAnterior, 1, CDate(txtEmision.Text), ValorNumero(txtSubTotal.Text))
        CalculaTotales()
        f = Nothing
    End Sub
    Private Sub btnEliminaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaDescuento.Click
        If nPosicionDescuento >= 0 Then
            With dtDescuentos.Rows(nPosicionDescuento)
                Dim aCamposDel() As String = {"numgas", "codpro", "renglon", "id_emp"}
                Dim aStringsDel() As String = {NumeroDocumentoAnterior, CodigoProveedorAnterior, .Item("renglon"), jytsistema.WorkID}
                nPosicionDescuento = EliminarRegistros(myConn, lblInfo, ds, nTablaDescuentos, "jsprodesgas", strSQLDescuentos, aCamposDel, aStringsDel, _
                                                      Me.BindingContext(ds, nTablaDescuentos).Position, True)
            End With
            CalculaTotales()
        End If
    End Sub
    Private Sub btnCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProveedor.Click
        txtProveedor.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codpro codigo, nombre descripcion, RIF from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Proveedores De Gastos/Compras", _
                                            txtProveedor.Text)
    End Sub



    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If txtNombreProveedor.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "GAS", NumeroDocumentoAnterior, CDate(txtEmision.Text), txtAlmacen.Text, , , True, , , , , , , , , , CodigoProveedorAnterior)
            nPosicionRenglon = f.Apuntador
            AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Private Sub txtAlmacen_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlmacen.TextChanged
        txtNombreAlmacen.Text = IIf(txtAlmacen.Text <> "", EjecutarSTRSQL_Scalar(myConn, lblInfo, " select desalm from jsmercatalm where codalm = '" & txtAlmacen.Text & "' and id_emp = '" & jytsistema.WorkID & "' "), "")
    End Sub

    Private Sub btnTransporte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrupoSubGrupo.Click
        Dim f As New jsComArcGrupoSubgrupo
        f.Grupo0 = Grupo
        f.Grupo1 = SubGrupo
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        Grupo = f.Grupo0
        SubGrupo = f.Grupo1

        txtGrupo.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select nombre from jsprogrugas where codigo = " & Grupo & " and id_emp = '" & jytsistema.WorkID & "' ")
        txtSubgrupo.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select nombre from jsprogrugas where codigo = " & SubGrupo & " and id_emp = '" & jytsistema.WorkID & "' ")

        f = Nothing
    End Sub

    Private Sub btnAlmacen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacen.Click
        txtAlmacen.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codalm codigo, desalm descripcion from jsmercatalm where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Almacenes", _
                                            txtAlmacen.Text)
    End Sub

    Private Sub btnCodigoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigoContable.Click
        txtCodigoContable.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables", _
                                                   txtCodigoContable.Text)
    End Sub

    Private Sub btnEmisionIVA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmisionIVA.Click
        txtEmisionIVA.Text = SeleccionaFecha(CDate(txtEmisionIVA.Text), Me, sender)
    End Sub

    Private Sub dgDescuentos_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgDescuentos.RowHeaderMouseClick, _
      dgDescuentos.CellMouseClick
        Me.BindingContext(ds, nTablaDescuentos).Position = e.RowIndex
        nPosicionDescuento = e.RowIndex
    End Sub

    Private Sub dgDescuentos_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgDescuentos.CellContentClick

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

    Private Sub txtEmision_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEmision.TextChanged
        txtEmisionIVA.Text = txtEmision.Text
    End Sub
End Class