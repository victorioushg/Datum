Imports MySql.Data.MySqlClient
Public Class jsComArcCompras
    Private Const sModulo As String = "Compras"
    Private Const lRegion As String = "RibbonButton61"
    Private Const nTabla As String = "tblEncab"
    Private Const nTablaRenglones As String = "tblRenglones_"
    Private Const nTablaIVA As String = "tblIVA"
    Private Const nTablaDescuentos As String = "tblDescuentos"

    Private strSQL As String = " (select a.*, b.nombre from jsproenccom a " _
            & " left join jsprocatpro b on (a.codpro = b.codpro and a.id_emp = b.id_emp) " _
            & " where a.id_emp = '" & jytsistema.WorkID & "' order by a.emision, a.numcom desc ) order by emision, numcom "

    'a.emision >= '" & FormatoFechaMySQL(DateAdd("m", -MesesAtras.i24, jytsistema.sFechadeTrabajo)) & "' and
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

    Private Eliminados As New ArrayList

    Private Impresa As Integer
    Private CodigoProveedorAnterior As String = ""
    Private NumeroDocumentoAnterior As String = ""
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
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva Compra")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> Compra")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> Compra")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> Compra")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primer</B> Compra")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a Compra <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a Compra <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>último Compra</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> Compra")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnDuplicar, "<B>Duplicar</B> este Compra")
        C1SuperTooltip1.SetToolTip(btnReconstruir, "<B>Reconstruir</B> movimientos de esta Compra")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en Compra")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón en Compra")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón en Compra")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en Compra")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón en Compra")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón en Compra")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> en Compra")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la Compra")
        C1SuperTooltip1.SetToolTip(btnOrdenes, "Traer <B>órdenes de compra</B> pendientes ")
        C1SuperTooltip1.SetToolTip(btnRecepciones, "Traer <B>Recepciones</B> de mercancía pendientes")

        'Menu Barra Descuento 
        C1SuperTooltip1.SetToolTip(btnAgregaDescuento, "<B>Agrega </B> descuento global a Compra")
        C1SuperTooltip1.SetToolTip(btnEliminaDescuento, "<B>Elimina</B> descuento global de Compra")



    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)

        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)

        If c >= 0 And dtRenglones.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaRenglones).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", c, dtRenglones.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtRenglones, MenuBarraRenglon)
        HabilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtProveedor, btnProveedor, txtCodigo, txtNumeroSerie)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        i_modo = movimiento.iConsultar

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtCodigo.Text = MuestraCampoTexto(.Item("numcom"))
                txtNumeroSerie.Text = MuestraCampoTexto(.Item("serie_numcom"))
                NumeroDocumentoAnterior = .Item("numcom")
                txtEmision.Text = FormatoFecha(CDate(.Item("emision").ToString))
                txtVencimiento.Text = FormatoFecha(CDate(.Item("vence4").ToString))
                txtEmisionIVA.Text = FormatoFecha(CDate(.Item("emisioniva").ToString))
                txtProveedor.Text = MuestraCampoTexto(.Item("codpro"))
                CodigoProveedorAnterior = .Item("codpro")
                txtReferencia.Text = MuestraCampoTexto(.Item("refer"))
                Dim numControl As String = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select num_control from jsconnumcon where numdoc = '" & .Item("numcom") & "' and " _
                                                                 & " prov_cli = '" & .Item("codpro") & "' and " _
                                                                 & " origen = 'COM' and org = 'COM' and id_emp = '" & jytsistema.WorkID & "' ")
                txtControl.Text = IIf(numControl <> "0", numControl, "")
                txtComentario.Text = MuestraCampoTexto(.Item("comen"))
                txtAlmacen.Text = MuestraCampoTexto(.Item("almacen"))
                txtCodigoContable.Text = MuestraCampoTexto(.Item("codcon"))

                tslblPesoT.Text = FormatoCantidad(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, "select if( sum(peso) is null, 0.000, sum(peso)) from jsprorencom where numcom = '" & .Item("numcom") & "' and codpro = '" & .Item("codpro") & "' and id_emp = '" & jytsistema.WorkID & "' ")))

                txtSubTotal.Text = FormatoNumero(.Item("tot_net"))
                txtDescuentos.Text = FormatoNumero(.Item("descuen"))
                txtCargos.Text = FormatoNumero(.Item("cargos"))
                txtTotalIVA.Text = FormatoNumero(.Item("imp_iva"))
                txtTotal.Text = FormatoNumero(.Item("tot_com"))



                CondicionDePago = .Item("condpag")
                Dim FormaDePago As String = ""
                If .Item("formapag").ToString.Trim() <> "" Then _
                    FormaDePago = aFormaPago(InArray(aFormaPagoAbreviada, .Item("FORMAPAG")) - 1) & "  " & _
                        .Item("NUMPAG") & " " & _
                        IIf(.Item("nompag").ToString.Trim() <> "", CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, " SELECT NOMBAN from jsbancatban where codban = '" & .Item("nompag") & "' and id_emp = '" & jytsistema.WorkID & "' ")), "")
                FechaVencimiento = CDate(.Item("vence4").ToString)

                txtCondicionPago.Text = IIf(.Item("condpag") = 0, "CREDITO", "CONTADO") & _
                    " - Forma Pago : " & FormaDePago

                Impresa = .Item("impresa")

                txtFechaRecibido.Text = FormatoFecha(CDate(.Item("VENCE3").ToString))
                txtFechaRecibido.Visible = CBool(.Item("recibido"))
                lblRecibido.Visible = CBool(.Item("recibido"))

                'Renglones
                AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)

                'Totales
                CalculaTotales()

            End With
        End With
    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroCompra As String, ByVal CodigoProveedor As String)

        strSQLMov = "select * from jsprorencom " _
                            & " where " _
                            & " numcom  = '" & NumeroCompra & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"item", "descrip", "iva", "cantidad", "unidad", "costou", "des_art", "des_pro", "costotot", "estatus", "numord", "numrec", ""}
        Dim aNombres() As String = {"Item", "Descripción", "IVA", "Cant.", "UND", "Costo Unitario", "Desc. Art.", "Desc. Prov.", "Costo Total", "Tipo Renglon", "Nº Orden de Compra", "Nº Recepción", ""}
        Dim aAnchos() As Long = {90, 300, 30, 70, 45, 100, 50, 50, 120, 60, 100, 100, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda,
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", sFormatoCantidad, "", sFormatoNumero, sFormatoNumero, sFormatoNumero, _
                                     sFormatoNumero, "", "", "", ""}
        IniciarTabla(dg, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub
    Private Sub AbrirIVA(ByVal NumeroDocumento As String, ByVal CodigoProveedor As String)

        strSQLIVA = "select * from jsproivacom " _
                            & " where " _
                            & " numcom = '" & NumeroDocumento & "' and " _
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

        txtTotalIVA.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(impiva) from jsproivacom where numcom = '" & NumeroDocumentoAnterior & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' group by numcom ")))


    End Sub
    Private Function AbrirDescuentos(ByVal NumeroDocumento As String, ByVal CodigoProveedor As String) As Double

        strSQLDescuentos = "select * from jsprodescom " _
                            & " where " _
                            & " numcom  = '" & NumeroDocumento & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLDescuentos, myConn, nTablaDescuentos, lblInfo)
        dtDescuentos = ds.Tables(nTablaDescuentos)

        Dim nSubTotal As Double = ValorNumero(txtSubTotal.Text)
        For Each nRow As DataRow In dtDescuentos.Rows
            With nRow
                Dim nDesc As Double = nSubTotal * .Item("pordes") / 100
                nSubTotal -= nDesc
                EjecutarSTRSQL(myConn, lblInfo, " update jsprodescom set descuento = " & nDesc & ", subtotal = " & nSubTotal & " " _
                               & " where " _
                               & " numcom = '" & .Item("numcom") & "' and " _
                               & " codpro = '" & .Item("codpro") & "' and " _
                               & " renglon = '" & .Item("renglon") & "' and " _
                               & " id_emp = '" & jytsistema.WorkID & "' ")
            End With
        Next

        ds = DataSetRequery(ds, strSQLDescuentos, myConn, nTablaDescuentos, lblInfo)
        dtDescuentos = ds.Tables(nTablaDescuentos)

        Dim aCampos() As String = {"renglon", "pordes", "descuento"}
        Dim aNombres() As String = {"", "", ""}
        Dim aAnchos() As Long = {60, 45, 60}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {"", sFormatoNumero, sFormatoNumero}
        IniciarTabla(dgDescuentos, dtDescuentos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False, , 8, False)

        Return CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(descuento) from jsprodescom where numcom = '" & NumeroDocumentoAnterior & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' group by numcom "))

    End Function

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        txtNumeroSerie.Text = ""
        txtCodigo.Text = ""

        NumeroDocumentoAnterior = txtCodigo.Text
        CodigoProveedorAnterior = ""

        IniciarTextoObjetos(FormatoItemListView.iCadena, txtControl, txtProveedor, txtNombreProveedor, txtComentario, _
                            txtAlmacen, txtReferencia, txtCodigoContable, txtVencimiento, txtCondicionPago)

        Dim nAlmacen As String = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, "SELECT codalm FROM jsmercatalm WHERE id_emp = '" & jytsistema.WorkID & "' ORDER BY codalm LIMIT 1"))
        If nAlmacen <> "0" Then txtAlmacen.Text = nAlmacen

        txtEmision.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        txtEmisionIVA.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        tslblPesoT.Text = FormatoCantidad(0)
        IniciarTextoObjetos(FormatoItemListView.iNumero, txtSubTotal, txtDescuentos, txtCargos, txtTotalIVA, txtTotalIVA)
        txtVencimiento.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        txtTotal.Text = "0.00"
        lblRecibido.Visible = False

        CondicionDePago = 0

        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", 0, 0)
        AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
        AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)
        txtDescuentos.Text = FormatoNumero(AbrirDescuentos(NumeroDocumentoAnterior, CodigoProveedorAnterior))


    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        HabilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon, MenuDescuentos)
        HabilitarObjetos(True, True, txtCodigo, txtNumeroSerie, txtComentario, btnEmision, btnProveedor, txtProveedor, btnEmisionIVA, _
                         txtControl, btnEmision, txtAlmacen, btnAlmacen, _
                         txtReferencia, btnCodigoContable)

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        HabilitarObjetos(False, True, txtCodigo, txtNumeroSerie, txtEmision, btnEmision, txtControl, txtEmisionIVA, btnEmisionIVA, _
                txtProveedor, txtNombreProveedor, btnProveedor, txtComentario, _
                txtVencimiento, txtCondicionPago, txtAlmacen, btnAlmacen, _
                txtNombreAlmacen, txtReferencia, txtCodigoContable, btnCodigoContable)

        HabilitarObjetos(False, True, txtDescuentos, txtCargos, MenuDescuentos, txtSubTotal, txtTotalIVA, txtTotal)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then
            AgregaYCancela()
        Else
            If dtRenglones.Rows.Count = 0 Then
                MensajeCritico(lblInfo, "DEBE INCLUIR AL MENOS UN ITEM...")
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

        EjecutarSTRSQL(myConn, lblInfo, " delete from jsprorencom where numcom = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsproivacom where numcom = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsprodescom where numcom = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'COM' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim f As New jsComFormasPago
            f.CondicionPago = CondicionDePago
            f.TipoCredito = TipoCredito
            f.Vencimiento = IIf(CondicionDePago = 0, FechaVencimiento, CDate(txtEmision.Text))
            f.Caja = Caja
            f.FormaPago = FormaPago

            f.Cargar(myConn, "COM", NumeroDocumentoAnterior, IIf(i_modo = movimiento.iAgregar, True, False), ValorNumero(txtTotal.Text), True)

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
                ' Imprimir()
            End If
        End If
    End Sub
    Private Sub Imprimir()
        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cCompra, "Compra", txtProveedor.Text, txtCodigo.Text, CDate(txtEmision.Text))
        f = Nothing
    End Sub
    Private Function Validado() As Boolean

        If txtCodigo.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un Número de Compra válido...")
            Return False
        End If

        If txtNombreProveedor.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un proveedor válido...")
            Return False
        End If

        If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(*) from jsproenccom where numcom = '" & _
                                      txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & _
                                      jytsistema.WorkID & "'  ")) > 0 AndAlso _
                                      i_modo = movimiento.iAgregar Then
            MensajeCritico(lblInfo, "Número de Compra YA existe para este proveedor ...")
            Return False
        End If

        If txtNombreAlmacen.Text = "" Then
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

        If i_modo = movimiento.iAgregar Then

            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count

        End If

        EjecutarSTRSQL(myConn, lblInfo, " update jsprorencom set numcom = '" & Codigo & "', codpro = '" & txtProveedor.Text & "' where codpro = '" & CodigoProveedorAnterior & "' and numcom = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " update jsproivacom set numcom = '" & Codigo & "', codpro = '" & txtProveedor.Text & "' where codpro = '" & CodigoProveedorAnterior & "' and numcom = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " update jsprodescom set numcom = '" & Codigo & "', codpro = '" & txtProveedor.Text & "' where codpro = '" & CodigoProveedorAnterior & "' and numcom = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & NumeroDocumentoAnterior & "' and origen = 'COM' and id_emp = '" & jytsistema.WorkID & "' ")

        Dim porcentajeDescuento As Double = (1 - ValorNumero(txtDescuentos.Text) / ValorNumero(txtSubTotal.Text)) * 100
        Dim numCajas As Double = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(cantidad) from jsprorencom where numcom = '" & Codigo & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' group by numcom ")

        InsertEditCOMPRASEncabezadoCompras(myConn, lblInfo, Inserta, Codigo, txtNumeroSerie.Text, CDate(txtEmision.Text), CDate(txtEmisionIVA.Text), _
                txtProveedor.Text, txtComentario.Text, txtAlmacen.Text, txtReferencia.Text, txtCodigoContable.Text, dtRenglones.Rows.Count, numCajas, _
                ValorCantidad(tslblPesoT.Text), ValorNumero(txtSubTotal.Text), porcentajeDescuento, ValorNumero(txtDescuentos.Text), _
                ValorNumero(txtCargos.Text), ValorNumero(txtTotal.Text), 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, _
                FechaDocumentoPago, jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, FechaVencimiento, _
                CondicionDePago, TipoCredito, FormaPago, NumeroPago, NombrePago, Beneficiario, Caja, 0.0, "", 0, 0, 0.0, 0.0, "", jytsistema.sFechadeTrabajo, _
                ValorNumero(txtTotalIVA.Text), 0.0, 0.0, "", jytsistema.sFechadeTrabajo, 0.0, "", jytsistema.sFechadeTrabajo, _
                0.0, 0.0, "", "", "", Impresa, CodigoProveedorAnterior, NumeroDocumentoAnterior)

        InsertEditCONTROLNumeroControl(myConn, lblInfo, Inserta, Codigo, txtProveedor.Text, txtControl.Text, CDate(txtEmisionIVA.Text), "COM", "COM", NumeroDocumentoAnterior, CodigoProveedorAnterior)

        ActualizarMovimientos(Codigo, txtProveedor.Text)
        ActualizarGanancias(Codigo, txtProveedor.Text)

        'ACTUALIZAR RENGLONES DESDE RECEPCIONES Y ORDENES DE COMPRA

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" NUMCOM = '" & Codigo & "' AND CODPRO = '" & txtProveedor.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub ActualizarInventarios(ByVal NumeroCompra As String, ByVal CodigoProveedor As String)

        '1.- Elimina movimientos de inventarios anterior compra
        EliminarMovimientosdeInventario(myConn, NumeroCompra, "COM", lblInfo, , , CodigoProveedor)

        EjecutarSTRSQL(myConn, lblInfo, "UPDATE jsproenccom SET vence3 = '" & IIf(lblRecibido.Visible, FormatoFechaMySQL(CDate(txtFechaRecibido.Text)), _
                                                                                  FormatoFechaMySQL(CDate(txtEmision.Text))) & "', " _
                               & " RECIBIDO = 1 " _
                               & " where " _
                               & " NUMCOM = '" & NumeroCompra & "' and " _
                               & " CODPRO = '" & CodigoProveedor & "' and " _
                               & " ID_EMP ='" & jytsistema.WorkID & "' ")


        '2.- Actualizar Movimientos de Inventario con los renglones de la compra
        strSQLMov = "select * from jsprorencom " _
                            & " where " _
                            & " numcom  = '" & NumeroCompra & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "
        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        For Each dtRow As DataRow In dtRenglones.Rows
            With dtRow
                If .Item("item").ToString.Substring(1, 1) <> "$" Then
                    InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(txtEmision.Text), "EN", NumeroCompra, _
                                                         .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("costotot"), _
                                                         .Item("costototdes"), "COM", NumeroCompra, .Item("lote"), txtProveedor.Text, _
                                                          0.0, 0.0, 0, 0.0, "", _
                                                         txtAlmacen.Text, .Item("renglon"), jytsistema.sFechadeTrabajo)
                    Dim MontoUltimaCompra As Double = IIf(.Item("cantidad") > 0, .Item("costototdes") / .Item("cantidad"), .Item("costototdes")) / Equivalencia(myConn, lblInfo, .Item("item"), .Item("unidad"))

                    EjecutarSTRSQL(myConn, lblInfo, " update jsmerctainv set fecultcosto = '" & FormatoFechaMySQL(CDate(txtEmision.Text)) & "', ultimoproveedor = '" & txtProveedor.Text & "', montoultimacompra = " & MontoUltimaCompra & " where codart = '" & .Item("item") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                End If

                ActualizarExistenciasPlus(myConn, .Item("item"))

            End With
        Next
    End Sub

    Private Sub ActualizarMovimientos(ByVal NumeroCompra As String, ByVal CodigoProveedor As String)

        '1.- Aplica Descuento Global sobre total Renglón con descuento "costototdes"
        EjecutarSTRSQL(myConn, lblInfo, " update jsprorencom set costototdes = costotot - costotot * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
                                        & " where " _
                                        & " numcom = '" & NumeroCompra & "' and " _
                                        & " renglon > '' and " _
                                        & " item > '' and " _
                                        & " estatus = '0' AND " _
                                        & " aceptado < '2' and " _
                                        & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                        & " id_emp = '" & jytsistema.WorkID & "' ")

        If CBool(ParametroPlus(MyConn, Gestion.iCompras, "COMPACOM03")) Then
            ActualizarInventarios(NumeroCompra, CodigoProveedor)
        Else
            Dim Actualiza As Microsoft.VisualBasic.MsgBoxResult
            Actualiza = MsgBox(" ¿ Desea actualizar INVENTARIOS de la mercancía incluída en esta COMPRA ?", MsgBoxStyle.YesNo, sModulo & " Repreciar ... ")
            If Actualiza = MsgBoxResult.Yes Then
                ActualizarInventarios(NumeroCompra, CodigoProveedor)
            Else
                MensajeInformativoPlus("LA MERCANCIA INCLUIDA EN ESTA COMPRA *NO* FUE ACTUALIZADA EN LOS INVENTARIOS. DEBE SER INCLUIDA MEDIANTE EL PROCESO...")
            End If
        End If

        '4.- Actualizar existencias de renglones eliminados
        For Each aSTR As Object In Eliminados
            ActualizarExistenciasPlus(myConn, aSTR)
        Next

        '5.- Actualizar CxP si es un Compra a crédito
        If CondicionDePago = CondicionPago.iCredito Then
            EjecutarSTRSQL(myConn, lblInfo, " DELETE from jsprotrapag WHERE " _
                                            & " TIPOMOV = 'FC' AND  " _
                                            & " codpro = '" & CodigoProveedorAnterior & "' and " _
                                            & " NUMMOV = '" & NumeroDocumentoAnterior & "' AND " _
                                            & " ORIGEN = 'COM' AND " _
                                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                            & " ID_EMP = '" & jytsistema.WorkID & "'  ")

            InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, "FC", NumeroCompra, CDate(txtEmision.Text), FormatoHora(Now), _
                                FechaVencimiento, txtReferencia.Text, "COMPRA N° : " & NumeroCompra, -1 * ValorNumero(txtTotal.Text), _
                                ValorNumero(txtTotalIVA.Text), "", "", "", "", "COM", "", "", "", Caja, NumeroCompra, "0", "", jytsistema.sFechadeTrabajo, _
                                txtCodigoContable.Text, "", "", 0.0, 0.0, "", "", "", "", "", "", "0", "0", "0")


        Else '6.- Actualizar Caja y Bancos si es un Gasto a contado
            ' 6.1.- Elimina movimientos anteriores de caja y bancos
            EjecutarSTRSQL(myConn, lblInfo, "DELETE FROM jsbantracaj WHERE " _
                        & " ORIGEN = 'COM' AND " _
                        & " TIPOMOV = 'SA' AND " _
                        & " NUMMOV = '" & NumeroDocumentoAnterior & "' AND " _
                        & " PROV_CLI = '" & CodigoProveedorAnterior & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "'")

            EjecutarSTRSQL(myConn, lblInfo, "DELETE FROM jsbantraban WHERE " _
                        & " ORIGEN = 'COM' AND " _
                        & " concepto = 'CANC. COMPRA N° " & NumeroDocumentoAnterior & "' and " _
                        & " prov_cli = '" & CodigoProveedorAnterior & "' and " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' ")


            Select Case FormaPago
                Case "EF"

                    InsertEditBANCOSRenglonCaja(myConn, lblInfo, True, Caja, UltimoCajaMasUno(myConn, lblInfo, Caja), _
                                                FechaDocumentoPago, "COM", "SA", NumeroCompra, FormaPago, _
                                                NumeroPago, NombrePago, -1 * ValorNumero(txtTotal.Text), txtCodigoContable.Text, _
                                                "COMPRA N° :" & NumeroCompra, "", jytsistema.sFechadeTrabajo, 1, "", 0, "", _
                                                jytsistema.sFechadeTrabajo, CodigoProveedor, "", "1")

                    CalculaSaldoCajaPorFP(myConn, Caja, FormaPago, lblInfo)

                Case "CH"

                    InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, FechaDocumentoPago, NumeroPago, FormaPago, _
                                                    NombrePago, "", "COMPRA N° :" & txtCodigo.Text, -1 * ValorNumero(txtTotal.Text), "COM", txtCodigo.Text, _
                                                    Beneficiario, txtCodigo.Text, "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, "FC", _
                                                    "", CDate(txtEmision.Text), "0", txtProveedor.Text, "")

                    CalculaSaldoBanco(myConn, lblInfo, NombrePago, True, jytsistema.sFechadeTrabajo)

                Case "DP"



            End Select

        End If

    End Sub
    Private Sub ActualizarGanancias(ByVal NumeroDeCompra As String, ByVal CodigoProveedor As String)
        Dim Reprecia As Microsoft.VisualBasic.MsgBoxResult

        If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPACOM04")) Then
            Reprecia = MsgBoxResult.Yes
        Else
            Reprecia = MsgBox(" ¿ Desea actualizar precios según ganancia estipulada ?", MsgBoxStyle.YesNo, sModulo & " Repreciar ... ")
        End If

        If Reprecia = MsgBoxResult.Yes Then
            Dim Divide As Microsoft.VisualBasic.MsgBoxResult
            If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPACOM04")) Then
                Divide = MsgBoxResult.Yes
            Else
                Divide = MsgBox(" ¿ PARA ACTUALIZAR PRECIOS DESEA CALCULO DE GANANCIAS CON FACTOR DIVISION ?", MsgBoxStyle.YesNo, sModulo & " Repreciar ... ")
            End If
            Dim dtRenReal As DataTable
            Dim tblRen As String = "tblRenglonesReales"

            ds = DataSetRequery(ds, " SELECT a.ITEM, a.UNIDAD, SUM(a.CANTIDAD) AS Cantidadrenglon, SUM(a.costotot) COSTO,  " _
                                & " IF (ISNULL(UVALENCIA), a.UNIDAD, b.UNIDAD) AS UNIDAD, " _
                                & " SUM(IF (ISNULL(UVALENCIA), a.CANTIDAD, a.cantidad/b.equivale)) CANTIDAD, " _
                                & " SUM(a.costototdes) COSTOTOTDES " _
                                & " FROM jsprorencom a " _
                                & " LEFT JOIN jsmerequmer b ON ( a.ITEM = b.CODART AND a.UNIDAD = b.UVALENCIA  AND a.ID_EMP = b.ID_EMP ) " _
                                & " WHERE " _
                                & " a.numcom = '" & NumeroDeCompra & "' AND " _
                                & " a.codpro = '" & CodigoProveedor & "' AND " _
                                & " a.aceptado < '2' AND " _
                                & " a.ID_EMP = '" & jytsistema.WorkID & "' AND " _
                                & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' " _
                                & " GROUP BY item ", myConn, tblRen, lblInfo)

            dtRenReal = ds.Tables(tblRen)
            If dtRenReal.Rows.Count > 0 Then
                For Each dtR As DataRow In dtRenReal.Rows
                    Dim strSQLPrecios As String = ""
                    With dtR
                        Dim CostoReal As Double = .Item("costototdes") / .Item("cantidad")
                        If Divide = MsgBoxResult.Yes Then
                            strSQLPrecios = " PRECIO_A = ROUND(IF ( GANAN_A <> 0.00, " & CostoReal & " / (1 - GANAN_A/100),0),2 ), " _
                                & " PRECIO_B = ROUND(IF ( GANAN_B <> 0.00, " & CostoReal & " / (1 - GANAN_B/100),0),2 ), " _
                                & " PRECIO_C = ROUND(IF ( GANAN_C <> 0.00, " & CostoReal & " / (1 - GANAN_C/100),0),2 ), " _
                                & " PRECIO_D = ROUND(IF ( GANAN_D <> 0.00, " & CostoReal & " / (1 - GANAN_D/100),0),2 ), " _
                                & " PRECIO_E = ROUND(IF ( GANAN_E <> 0.00, " & CostoReal & " / (1 - GANAN_E/100),0),2 ), " _
                                & " PRECIO_F = ROUND(IF ( GANAN_F <> 0.00, " & CostoReal & " / (1 - GANAN_F/100),0),2 ) "
                        Else
                            strSQLPrecios = " PRECIO_A = ROUND(IF ( GANAN_A <> 0.00,  " & CostoReal & " * ( 1 + GANAN_A /100),0),2 ), " _
                                & " PRECIO_B = ROUND(IF ( GANAN_B <> 0.00,  " & CostoReal & " * ( 1 + GANAN_B /100),0),2 ), " _
                                & " PRECIO_C = ROUND(IF ( GANAN_C <> 0.00,  " & CostoReal & " * ( 1 + GANAN_C /100),0),2 ), " _
                                & " PRECIO_D = ROUND(IF ( GANAN_D <> 0.00,  " & CostoReal & " * ( 1 + GANAN_D /100),0),2 ), " _
                                & " PRECIO_E = ROUND(IF ( GANAN_E <> 0.00,  " & CostoReal & " * ( 1 + GANAN_E /100),0),2 ), " _
                                & " PRECIO_F = ROUND(IF ( GANAN_F <> 0.00,  " & CostoReal & " * ( 1 + GANAN_F /100),0),2 ) "

                        End If
                        EjecutarSTRSQL(myConn, lblInfo, "UPDATE jsmerctainv SET " & strSQLPrecios _
                                                & " WHERE CODART = '" & .Item("ITEM") & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                    End With

                Next
            End If
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
        HabilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtProveedor, btnProveedor, txtCodigo, txtNumeroSerie)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        i_modo = movimiento.iEditar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position

        With dt.Rows(nPosicionEncab)
            If DocumentoPoseeRetencionIVA(myConn, lblInfo, .Item("numcom"), .Item("codpro")) Then
                MensajeCritico(lblInfo, "Esta FACTURA posee RETENCION DE IVA. MODIFICACION NO esta permitida ...")
            Else
                If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(*) from jsprotrapag " _
                                              & " where " _
                                                  & " tipomov <> 'FC' and ORIGEN <> 'COM' AND " _
                                                  & " codpro = '" & .Item("codpro") & "' and " _
                                                  & " nummov = '" & .Item("numcom") & "' and " _
                                                  & " id_emp = '" & jytsistema.WorkID & "' ")) = 0 Then

                    ActivarMarco0()
                    HabilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtProveedor, btnProveedor, txtCodigo, txtNumeroSerie)
                Else
                    If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) = 0 Then
                        MensajeCritico(lblInfo, "Esta FACTURA posee movimientos asociados. MODIFICACION NO esta permitida ...")
                    Else
                        ActivarMarco0()
                    End If
                End If
            End If
        End With

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        With dt.Rows(nPosicionEncab)
            If DocumentoPoseeRetencionIVA(myConn, lblInfo, .Item("numcom"), .Item("codpro")) Then
                MensajeCritico(lblInfo, "Esta FACTURA posee RETENCION DE IVA. MODIFICACION NO esta permitida ...")
            Else

                If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(*) from jsprotrapag " _
                                              & " where " _
                                              & " tipomov <> 'FC' AND ORIGEN <> 'COM' AND " _
                                              & " codpro = '" & .Item("codpro") & "' and " _
                                              & " nummov = '" & .Item("numcom") & "' and " _
                                              & " id_emp = '" & jytsistema.WorkID & "' ")) = 0 Then

                    Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                    nPosicionEncab = Me.BindingContext(ds, nTabla).Position

                    sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")

                    If sRespuesta = MsgBoxResult.Yes Then

                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numcom"))

                        For Each dtRow As DataRow In dtRenglones.Rows
                            With dtRow
                                Eliminados.Add(.Item("item"))
                                .Item("cantidad") = 0.0
                            End With
                        Next
                        ActualizarRenglonesEnOrdenesDeCompra(myConn, lblInfo, ds, dtRenglones, "jsprorencom")

                        EjecutarSTRSQL(myConn, lblInfo, " delete from jsproenccom where numcom = '" & txtCodigo.Text & "' AND CODPRO = '" & txtProveedor.Text & "' and ID_EMP = '" & jytsistema.WorkID & "'")
                        EjecutarSTRSQL(myConn, lblInfo, " delete from jsprorencom where numcom = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " delete from jsprodescom where numcom = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " delete from jsproivacom where numcom = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'COM' and id_emp = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " DELETE FROM jsbantraban where ORIGEN = 'COM' AND NUMORG = '" & txtCodigo.Text & "' AND prov_cli = '" & txtProveedor.Text & "' and EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " DELETE FROM jsbantracaj where nummov = '" & txtCodigo.Text & "' AND prov_cli = '" & txtProveedor.Text & "' and origen = 'COM' AND tipomov = 'SA' AND ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " DELETE FROM jsprotrapag where CODPRO = '" & txtProveedor.Text & "' AND TIPOMOV = 'FC' AND NUMMOV = '" & txtCodigo.Text & _
                                       "' AND ORIGEN = 'COM' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")

                        EliminarMovimientosdeInventario(myConn, txtCodigo.Text, "COM", lblInfo, , , txtProveedor.Text)

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
        Dim Campos() As String = {"numcom", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Nombre", "Emisión", "Comentario"}
        Dim Anchos() As Long = {150, 400, 100, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, " Compras de mercancías ...")
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

        Dim bDesc As Boolean = False



        If Not DocumentoPoseeRetencionIVA(myConn, lblInfo, NumeroDocumentoAnterior, CodigoProveedorAnterior) Then

            txtSubTotal.Text = FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsprorencom", "numcom", "costotot", NumeroDocumentoAnterior, 0, CodigoProveedorAnterior))

            txtDescuentos.Text = FormatoNumero(AbrirDescuentos(NumeroDocumentoAnterior, CodigoProveedorAnterior))

            txtCargos.Text = FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsprorencom", "numcom", "costotot", NumeroDocumentoAnterior, 1, CodigoProveedorAnterior))

            bDesc = CalculaDescuentoEnRenglones(ValorNumero(txtDescuentos.Text), ValorNumero(txtSubTotal.Text))

            txtTotalIVA.Text = FormatoNumero(CalculaTotalIVACompras(myConn, lblInfo, CodigoProveedorAnterior, "jsprodescom", "jsproivacom", "jsprorencom", "numcom", NumeroDocumentoAnterior, "impiva", "costototdes", CDate(txtEmision.Text), "costotot"))

            AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)

            txtTotal.Text = FormatoNumero(ValorNumero(txtSubTotal.Text) - ValorNumero(txtDescuentos.Text) + ValorNumero(txtCargos.Text) + ValorNumero(txtTotalIVA.Text))

            tslblPesoT.Text = FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsprorencom", "peso", "numcom", NumeroDocumentoAnterior))
        Else

            txtDescuentos.Text = FormatoNumero(AbrirDescuentos(NumeroDocumentoAnterior, CodigoProveedorAnterior))
            AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)
            txtTotal.Text = FormatoNumero(ValorNumero(txtSubTotal.Text) - ValorNumero(txtDescuentos.Text) + ValorNumero(txtCargos.Text) + ValorNumero(txtTotalIVA.Text))
            tslblPesoT.Text = FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsprorencom", "peso", "numcom", NumeroDocumentoAnterior))

        End If

    End Sub
    Private Function CalculaDescuentoEnRenglones(TotalDescuentos As Double, SubTotalCompra As Double) As Boolean



        EjecutarSTRSQL_Scalar(myConn, lblInfo, " update jsprorencom set costototdes = costotot - costotot * " & TotalDescuentos / IIf(SubTotalCompra > 0, SubTotalCompra, 1) & " " _
            & " where " _
            & " NUMCOM = '" & NumeroDocumentoAnterior & "' and " _
            & " renglon > '' and " _
            & " item > '' and " _
            & " ESTATUS = '0' AND " _
            & " ACEPTADO < '2' and " _
            & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
            & " ID_EMP = '" & jytsistema.WorkID & "' ")




        Return True

    End Function
    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        If txtCodigo.Text <> "" Then
            If txtNombreProveedor.Text <> "" Then
                Dim f As New jsGenRenglonesMovimientos
                f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
                f.Agregar(myConn, ds, dtRenglones, "COM", NumeroDocumentoAnterior, CDate(txtEmision.Text), txtAlmacen.Text, _
                          , , , , , , , , , , txtProveedor.Text, , CodigoProveedorAnterior)
                nPosicionRenglon = f.Apuntador
                AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
                CalculaTotales()
                f = Nothing
            Else
                MensajeCritico(lblInfo, "DEBE INDICAR UN PROVEEDOR VALIDO...")
            End If
        Else
            MensajeCritico(lblInfo, "DEBE INDICAR UN NÚMERO DE COMPRA VALIDO...")
        End If
    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        If dtRenglones.Rows.Count > 0 Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Editar(myConn, ds, dtRenglones, "COM", NumeroDocumentoAnterior, CDate(txtEmision.Text), txtAlmacen.Text, , , _
                     IIf(dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position).Item("item").ToString.Substring(1, 1) = "$", True, False), _
                     , , , , , , , txtProveedor.Text, , CodigoProveedorAnterior)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            CalculaTotales()
            AbrirDescuentos(txtCodigo.Text, txtProveedor.Text)
            f = Nothing
        End If


    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click

        EliminarMovimiento()

    End Sub
    Private Sub EliminarMovimiento()

        'Dim rEliminados() As DataRow

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then
            sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
            If sRespuesta = MsgBoxResult.Yes Then
                With dtRenglones.Rows(nPosicionRenglon)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("item"))

                    Eliminados.Add(.Item("item"))

                    Dim aCamposDel() As String = {"numcom", "codpro", "item", "renglon", "id_emp"}
                    Dim aStringsDel() As String = {NumeroDocumentoAnterior, CodigoProveedorAnterior, .Item("item"), .Item("renglon"), jytsistema.WorkID}

                    EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & NumeroDocumentoAnterior & "' and " _
                           & " origen = 'COM' and item = '" & .Item("item") & "' and renglon = '" & .Item("renglon") & "' and " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsprorencom", strSQLMov, aCamposDel, aStringsDel, _
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
        f.Buscar(dtRenglones, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaRenglones).Position, " Renglones de compras...")
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

    Private Sub txtCliente_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProveedor.TextChanged
        If txtProveedor.Text <> "" Then
            CodigoProveedorAnterior = txtProveedor.Text
            Dim aFld() As String = {"codpro", "id_emp"}
            Dim aStr() As String = {txtProveedor.Text, jytsistema.WorkID}
            Dim FormaDePagoCliente As String = qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "formapago")
            txtNombreProveedor.Text = qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "nombre")
            Dim mTotalFac As Double = ValorNumero(txtTotal.Text)
            If i_modo = movimiento.iAgregar Then
                mTotalFac = 0.0
                FechaVencimiento = DateAdd(DateInterval.Day, DiasCreditoAlVencimiento(myConn, FormaDePagoCliente), CDate(txtEmision.Text))
                CondicionDePago = CondicionPagoProveedorCliente(myConn, lblInfo, FormaDePagoCliente)
            End If
        End If
    End Sub

    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dgIVA.CellFormatting
        If e.ColumnIndex = 1 Then e.Value = FormatoNumero(e.Value) & "%"
    End Sub

    Private Sub btnAgregaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaDescuento.Click
        Dim f As New jsGenArcDescuentosCompras
        f.Agregar(myConn, ds, dtDescuentos, "jsprodescom", "numcom", NumeroDocumentoAnterior, sModulo, CodigoProveedorAnterior, 0, CDate(txtEmision.Text), ValorNumero(txtSubTotal.Text))
        CalculaTotales()
        f = Nothing
    End Sub
    Private Sub btnEliminaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaDescuento.Click
        If nPosicionDescuento >= 0 Then
            With dtDescuentos.Rows(nPosicionDescuento)
                Dim aCamposDel() As String = {"numcom", "codpro", "renglon", "id_emp"}
                Dim aStringsDel() As String = {NumeroDocumentoAnterior, CodigoProveedorAnterior, .Item("renglon"), jytsistema.WorkID}
                nPosicionDescuento = EliminarRegistros(myConn, lblInfo, ds, nTablaDescuentos, "jsprodescom", strSQLDescuentos, aCamposDel, aStringsDel, _
                                                      Me.BindingContext(ds, nTablaDescuentos).Position, True)
            End With
            CalculaTotales()
        End If
    End Sub
    Private Sub btnCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProveedor.Click
        txtProveedor.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codpro codigo, nombre descripcion, RIF from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Proveedores De Compras/Gastos", _
                                            txtProveedor.Text)
    End Sub

    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If txtNombreProveedor.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "COM", NumeroDocumentoAnterior, CDate(txtEmision.Text), txtAlmacen.Text, , , True, , , , , , , , , , CodigoProveedorAnterior)
            nPosicionRenglon = f.Apuntador
            AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Private Sub txtAlmacen_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlmacen.TextChanged
        txtNombreAlmacen.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select desalm from jsmercatalm where codalm = '" & txtAlmacen.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
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

    Private Sub btnOrdenes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrdenes.Click
        If NumeroDocumentoAnterior.Trim() <> "" Then
            If txtNombreProveedor.Text <> "" Then
                CodigoProveedorAnterior = txtProveedor.Text
                Dim f As New jsGenListadoSeleccion
                Dim aNombres() As String = {"", "Orden N°", "Emisión", "Monto"}
                Dim aCampos() As String = {"sel", "codigo", "emision", "tot_ord"}
                Dim aAnchos() As Long = {20, 120, 120, 200}
                Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Right}
                Dim aFormato() As String = {"", "", sFormatoFecha, sFormatoNumero}
                Dim aFields() As String = {"sel.entero", "codigo.cadena20", "emision.fecha", "tot_ord.doble19"}

                Dim str As String = "  select 0 sel, numord codigo, emision, tot_ord from jsproencord where " _
                        & " CODPRO = '" & txtProveedor.Text & "' AND " _
                        & " ESTATUS < '1' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND" _
                        & " ID_EMP = '" & jytsistema.WorkID & "' order by numord desc "

                f.Cargar(myConn, ds, "Ordenes de Compra en tránsito", str, _
                    aFields, aNombres, aCampos, aAnchos, aAlineacion, aFormato)

                If f.Seleccion.Length > 0 Then
                    Dim cod As String
                    For Each cod In f.Seleccion
                        Dim nTablaRenglonesOrden As String = "tblRenglonesOrden" & NumeroAleatorio(100000)
                        Dim dtRenglonesOrden As New DataTable
                        ds = DataSetRequery(ds, " select * from jsprorenord where numord = '" & cod & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' order by renglon ", myConn, nTablaRenglonesOrden, lblInfo)
                        dtRenglonesOrden = ds.Tables(nTablaRenglonesOrden)

                        For Each rRow As DataRow In dtRenglonesOrden.Rows
                            With rRow
                                Dim pesoTransito As Double = .Item("peso") / .Item("cantidad") * .Item("cantran")

                                InsertEditCOMPRASRenglonCOMPRAS(myConn, lblInfo, True, txtCodigo.Text, AutoCodigo(5, ds, dtRenglones.TableName, "renglon"), _
                                    txtProveedor.Text, .Item("item"), .Item("descrip"), .Item("iva"), "", .Item("unidad"), 0.0, .Item("cantran"), pesoTransito, "", "", "", _
                                    .Item("estatus"), .Item("costou"), .Item("des_art"), .Item("des_pro"), .Item("costou") * .Item("cantran"), _
                                    .Item("costou") * .Item("cantran") * (1 - .Item("des_art") / 100) * (1 - .Item("des_pro") / 100), .Item("numord"), .Item("renglon"), "", "", "", "1")

                            End With
                            AsignarMovimientos(txtCodigo.Text, txtProveedor.Text)
                            CalculaTotales()
                        Next
                        dtRenglonesOrden.Dispose()
                        dtRenglonesOrden = Nothing
                    Next

                End If
                f = Nothing
            Else
                MensajeCritico(lblInfo, "DEBE INDICAR UN PROVEEDOR VALIDO...")
            End If
        Else
            MensajeCritico(lblInfo, "DEBE INDICAR UN NUMERO DE COMPRA VALIDO...")
        End If

    End Sub

    Private Sub btnRecepciones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecepciones.Click
        If NumeroDocumentoAnterior.Trim() <> "" Then
            If txtNombreProveedor.Text <> "" Then
                CodigoProveedorAnterior = txtProveedor.Text
                Dim f As New jsGenListadoSeleccion
                Dim aNombres() As String = {"", "Recepción N°", "Emisión", "Monto"}
                Dim aCampos() As String = {"sel", "codigo", "emision", "tot_rec"}
                Dim aAnchos() As Long = {20, 120, 120, 200}
                Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Right}
                Dim aFormato() As String = {"", "", sFormatoFecha, sFormatoNumero}
                Dim aFields() As String = {"sel.entero", "codigo.cadena20", "emision.fecha", "tot_rec.doble19"}

                Dim str As String = "  select 0 sel, numrec codigo, emision, tot_rec from jsproencrep where " _
                        & " CODPRO = '" & txtProveedor.Text & "' AND " _
                        & " ESTATUS < '1' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND" _
                        & " ID_EMP = '" & jytsistema.WorkID & "' order by numrec desc "

                f.Cargar(myConn, ds, "Recepciones de Mercancías en tránsito", str, _
                    aFields, aNombres, aCampos, aAnchos, aAlineacion, aFormato)

                If f.Seleccion.Length > 0 Then
                    Dim cod As String
                    For Each cod In f.Seleccion
                        Dim nTablaRenglonesOrden As String = "tblRenglonesOrden" & NumeroAleatorio(100000)
                        Dim dtRenglonesOrden As New DataTable
                        ds = DataSetRequery(ds, " select * from jsprorenrep where numrec = '" & cod & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' order by renglon ", myConn, nTablaRenglonesOrden, lblInfo)
                        dtRenglonesOrden = ds.Tables(nTablaRenglonesOrden)

                        For Each rRow As DataRow In dtRenglonesOrden.Rows
                            With rRow
                                Dim pesoTransito As Double = .Item("peso") / .Item("cantidad") * .Item("cantran")

                                InsertEditCOMPRASRenglonCOMPRAS(myConn, lblInfo, True, txtCodigo.Text, AutoCodigo(5, ds, dtRenglones.TableName, "renglon"), _
                                    txtProveedor.Text, .Item("item"), .Item("descrip"), .Item("iva"), "", .Item("unidad"), 0.0, .Item("cantran"), pesoTransito, "", "", "", _
                                    .Item("estatus"), .Item("costou"), .Item("des_art"), .Item("des_pro"), .Item("costou") * .Item("cantran"), _
                                    .Item("costou") * .Item("cantran") * (1 - .Item("des_art") / 100) * (1 - .Item("des_pro") / 100), .Item("numord"), .Item("renglon"), "", "", "", "1")

                            End With
                            AsignarMovimientos(txtCodigo.Text, txtProveedor.Text)
                            CalculaTotales()
                        Next
                        dtRenglonesOrden.Dispose()
                        dtRenglonesOrden = Nothing
                    Next

                End If
                f = Nothing
            Else
                MensajeCritico(lblInfo, "DEBE INDICAR UN PROVEEDOR VALIDO...")
            End If
        Else
            MensajeCritico(lblInfo, "DEBE INDICAR UN NÚMERO DE COMPRA VALIDO...")
        End If

    End Sub

    Private Sub txtCodigo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCodigo.TextChanged
        If i_modo = movimiento.iAgregar Then NumeroDocumentoAnterior = txtCodigo.Text
    End Sub

    Private Sub dg_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub

    Private Sub txtEmision_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEmision.TextChanged
        txtEmisionIVA.Text = txtEmision.Text
    End Sub

    Private Sub btnReconstruir_Click(sender As System.Object, e As System.EventArgs) Handles btnReconstruir.Click
        If lblRecibido.Visible Then
            ActualizarInventarios(txtCodigo.Text, txtProveedor.Text)
            MensajeInformativoPlus("Proceso terminado!!! ")
        End If
    End Sub

End Class