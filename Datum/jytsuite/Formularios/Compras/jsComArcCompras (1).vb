Imports MySql.Data.MySqlClient
Imports fTransport
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

    Private Sub jsComArcCompras_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsComArcGastos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

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
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menus de barra 
        ft.colocaToolTip(C1SuperTooltip1, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnPrimero, btnSiguiente, _
                          btnAnterior, btnUltimo, btnImprimir, btnSalir, btnDuplicar, btnReconstruir, btnAgregarMovimiento, _
                          btnEditarMovimiento, btnEliminarMovimiento, btnBuscarMovimiento, btnPrimerMovimiento, btnAnteriorMovimiento, _
                          btnSiguienteMovimiento, btnUltimoMovimiento, btnOrdenes, btnRecepciones, btnAgregaDescuento, btnEliminaDescuento, _
                          btnAgregarServicio)

        'botones adicionales 
        ft.colocaToolTip(C1SuperTooltip1, btnEmision, btnEmisionIVA, btnProveedor, btnAlmacen, btnCodigoContable)

    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        dtRenglones = ft.MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, _
                               jytsistema.sUsuario, nRow, Actualiza)
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtProveedor, btnProveedor, txtCodigo, txtNumeroSerie)
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        i_modo = movimiento.iConsultar

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtCodigo.Text = ft.muestraCampoTexto(.Item("numcom"))
                txtNumeroSerie.Text = ft.muestraCampoTexto(.Item("serie_numcom"))
                NumeroDocumentoAnterior = ft.muestraCampoTexto(.Item("numcom"))
                txtEmision.Text = ft.muestraCampoFecha(.Item("emision"))
                txtVencimiento.Text = ft.muestraCampoFecha(.Item("vence4"))
                txtEmisionIVA.Text = ft.muestraCampoFecha(.Item("emisioniva"))
                txtProveedor.Text = ft.muestraCampoTexto(.Item("codpro"))
                CodigoProveedorAnterior = ft.muestraCampoTexto(.Item("codpro"))
                txtReferencia.Text = ft.muestraCampoTexto(.Item("refer"))

                Dim numControl As String = ft.DevuelveScalarCadena(myConn, " select num_control from jsconnumcon where numdoc = '" & .Item("numcom") & "' and " _
                                                                 & " prov_cli = '" & .Item("codpro") & "' and " _
                                                                 & " origen = 'COM' and org = 'COM' and id_emp = '" & jytsistema.WorkID & "' ")

                txtControl.Text = IIf(numControl <> "0", numControl, "")
                txtComentario.Text = ft.muestraCampoTexto(.Item("comen"))
                txtAlmacen.Text = ft.muestraCampoTexto(.Item("almacen"))
                txtCodigoContable.Text = ft.muestraCampoTexto(.Item("codcon"))

                tslblPesoT.Text = ft.muestraCampoCantidad(ft.DevuelveScalarDoble(myConn, "select SUM(PESO) from jsprorencom where numcom = '" & .Item("numcom") & "' and codpro = '" & .Item("codpro") & "' and id_emp = '" & jytsistema.WorkID & "' "))

                txtSubTotal.Text = ft.muestraCampoNumero(.Item("tot_net"))
                txtDescuentos.Text = ft.muestraCampoNumero(.Item("descuen"))
                txtCargos.Text = ft.muestraCampoNumero(.Item("cargos"))
                txtTotalIVA.Text = ft.muestraCampoNumero(.Item("imp_iva"))
                txtTotal.Text = ft.muestraCampoNumero(.Item("tot_com"))

                CondicionDePago = .Item("condpag")
                Dim FormaDePago As String = ""

                If .Item("formapag").ToString.Trim() <> "" Then _
                    FormaDePago = aFormaPago(ft.InArray(aFormaPagoAbreviada, .Item("FORMAPAG"))) & "  " & _
                        .Item("NUMPAG") & " " & _
                        IIf(.Item("nompag").ToString.Trim() <> "", ft.DevuelveScalarCadena(myConn, " SELECT NOMBAN from jsbancatban where codban = '" & .Item("nompag") & "' and id_emp = '" & jytsistema.WorkID & "' "), "")

                FechaVencimiento = CDate(.Item("vence4").ToString)

                txtCondicionPago.Text = IIf(.Item("condpag") = 0, "CREDITO", "CONTADO") & _
                    " - Forma Pago : " & FormaDePago _
                    & IIf(.Item("formapag") <> "EF", "", " CAJA : " & .Item("caja"))

                Impresa = .Item("impresa")

                txtFechaRecibido.Text = ft.muestraCampoFecha(.Item("VENCE3"))
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

        dtRenglones = ft.AbrirDataTable(ds, nTablaRenglones, myConn, strSQLMov)

        Dim aCampos() As String = {"item.Item.90.I.", _
                                   "descrip.Descripción.300.I.", _
                                   "iva.IVA.30.C.", _
                                   "cantidad.Cant.100.D.Cantidad", _
                                   "unidad.UND.45.C.", _
                                   "costou.Costo Unitario.120.D.Numero", _
                                   "des_art.Desc Art.50.D.Numero", _
                                   "des_pro.Desc Prov.50.D.Numero", _
                                   "costotot.Costo Total.120.D.Numero", _
                                   "estatus.Tipo Renglon.60.I.", _
                                   "numord.Nº Orden de Compra.100.I.", _
                                   "numrec.Nº Recepción.100.I.", _
                                   "sada..100.I."}
        
        ft.IniciarTablaPlus(dg, dtRenglones, aCampos)

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

        dtIVA = ft.AbrirDataTable(ds, nTablaIVA, myConn, strSQLIVA)

        Dim aCampos() As String = {"tipoiva..15.C.", "poriva..45.D.Numero", "baseiva..95.D.Numero", "impiva..90.D.Numero"}

        ft.IniciarTablaPlus(dgIVA, dtIVA, aCampos, False, , New Font("Consolas", 8, FontStyle.Regular), False)

        txtTotalIVA.Text = ft.muestraCampoNumero(ft.DevuelveScalarDoble(myConn, " select SUM(IMPIVA) from jsproivacom where numcom = '" & NumeroDocumentoAnterior & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' group by numcom "))


    End Sub
    Private Function AbrirDescuentos(ByVal NumeroDocumento As String, ByVal CodigoProveedor As String) As Double

        strSQLDescuentos = "select * from jsprodescom " _
                            & " where " _
                            & " numcom  = '" & NumeroDocumento & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        dtDescuentos = ft.AbrirDataTable(ds, nTablaDescuentos, myConn, strSQLDescuentos)
        VerificaDescuentosCompra(dtDescuentos)
        dtDescuentos = ft.AbrirDataTable(ds, nTablaDescuentos, myConn, strSQLDescuentos)

        Dim aCampos() As String = {"renglon..60.C.", "pordes..45.D.Numero", "descuento..140.D.Numero"}
        ft.IniciarTablaPlus(dgDescuentos, dtDescuentos, aCampos, False, , New Font("Consolas", 8, FontStyle.Regular), False)

        Return ft.DevuelveScalarDoble(myConn, " select SUM(DESCUENTO) from jsprodescom where numcom = '" & NumeroDocumentoAnterior & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' group by numcom ")

    End Function

    Private Sub VerificaDescuentosCompra(dtDescuentos As DataTable)

        Dim nSubTotal As Double = ft.valorNumero(txtSubTotal.Text)
        For Each nRow As DataRow In dtDescuentos.Rows
            With nRow
                Dim nDesc As Double = nSubTotal * .Item("pordes") / 100
                nSubTotal -= nDesc
                ft.Ejecutar_strSQL(myConn, " update jsprodescom set descuento = " & nDesc & ", subtotal = " & nSubTotal & " " _
                               & " where " _
                               & " numcom = '" & .Item("numcom") & "' and " _
                               & " codpro = '" & .Item("codpro") & "' and " _
                               & " renglon = '" & .Item("renglon") & "' and " _
                               & " id_emp = '" & jytsistema.WorkID & "' ")
            End With
        Next
    End Sub

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        txtNumeroSerie.Text = ""
        txtCodigo.Text = ""

        NumeroDocumentoAnterior = txtCodigo.Text
        CodigoProveedorAnterior = ""

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtControl, txtProveedor, txtNombreProveedor, txtComentario, _
                            txtAlmacen, txtReferencia, txtCodigoContable, txtVencimiento, txtCondicionPago)

        Dim nAlmacen As String = ft.DevuelveScalarCadena(myConn, "SELECT codalm FROM jsmercatalm WHERE id_emp = '" & jytsistema.WorkID & "' ORDER BY codalm LIMIT 1")
        If nAlmacen <> "0" Then txtAlmacen.Text = nAlmacen

        txtEmision.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtEmisionIVA.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        tslblPesoT.Text = ft.FormatoCantidad(0)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtSubTotal, txtDescuentos, txtCargos, txtTotalIVA, txtTotalIVA)
        txtVencimiento.Text = ft.FormatoFecha(jytsistema.sFechadeTrabajo)
        txtTotal.Text = "0.00"
        lblRecibido.Visible = False
        txtFechaRecibido.Text = ft.muestraCampoFecha(jytsistema.sFechadeTrabajo)
        txtFechaRecibido.Visible = False

        CondicionDePago = 0

        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
        AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)
        txtDescuentos.Text = ft.FormatoNumero(AbrirDescuentos(NumeroDocumentoAnterior, CodigoProveedorAnterior))


    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon, MenuDescuentos)
        ft.habilitarObjetos(True, True, txtCodigo, txtNumeroSerie, txtComentario, btnEmision, btnProveedor, txtProveedor, btnEmisionIVA, _
                         txtControl, btnEmision, btnAlmacen, _
                         txtReferencia, btnCodigoContable)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtNumeroSerie, txtEmision, btnEmision, txtControl, txtEmisionIVA, btnEmisionIVA, _
                txtProveedor, txtNombreProveedor, btnProveedor, txtComentario, _
                txtVencimiento, txtCondicionPago, txtAlmacen, btnAlmacen, _
                txtNombreAlmacen, txtReferencia, txtCodigoContable, btnCodigoContable)

        ft.habilitarObjetos(False, True, txtDescuentos, txtCargos, MenuDescuentos, txtSubTotal, txtTotalIVA, txtTotal)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then
            If ft.DevuelveScalarEntero(myConn, " select count(*) from jsproenccom " _
                                       & " where " _
                                       & " numcom = '" & txtCodigo.Text & "' and " _
                                       & " codpro = '" & txtProveedor.Text & "' and " _
                                       & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                AgregaYCancela()
            End If
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

        ft.Ejecutar_strSQL(myconn, " delete from jsprorencom where numcom = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myconn, " delete from jsproivacom where numcom = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myconn, " delete from jsprodescom where numcom = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myconn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'COM' and id_emp = '" & jytsistema.WorkID & "' ")

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

                InsertarAuditoria(myConn, IIf(i_modo = movimiento.iAgregar, MovAud.iIncluir, MovAud.imodificar), _
                                  sModulo, NumeroDocumentoAnterior)

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

        If FechaUltimoBloqueo(myConn, "jsproenccom") >= Convert.ToDateTime(txtEmision.Text) Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If txtCodigo.Text = "" Then
            ft.mensajeCritico("Debe indicar un Número de Compra válido...")
            Return False
        End If

        If txtNombreProveedor.Text = "" Then
            ft.mensajeCritico("Debe indicar un proveedor válido...")
            Return False
        End If

        If ft.DevuelveScalarEntero(myConn, " select count(*) from jsproenccom where numcom = '" & _
                                      txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & _
                                      jytsistema.WorkID & "'  ") > 0 AndAlso _
                                      i_modo = movimiento.iAgregar Then
            ft.mensajeCritico("Número de Compra YA existe para este proveedor ...")
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

        If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPACOM07")) Then
            If ft.DevuelveScalarEntero(myConn, " select count(*) from jsprorencom where substring(item,1,1) <> '$' AND  " _
                                       & " numcom = '" & txtCodigo.Text & "' AND " _
                                       & " numrec = '' AND " _
                                       & " codpro = '" & txtProveedor.Text & "' AND " _
                                       & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then

                ft.mensajeCritico("TODOS los ítems de la factura de COMPRA deben provenir de una RECEPCION")
                Return False
            End If
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

        ft.Ejecutar_strSQL(myconn, " update jsprorencom set numcom = '" & Codigo & "', codpro = '" & txtProveedor.Text & "' where codpro = '" & CodigoProveedorAnterior & "' and numcom = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myconn, " update jsproivacom set numcom = '" & Codigo & "', codpro = '" & txtProveedor.Text & "' where codpro = '" & CodigoProveedorAnterior & "' and numcom = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myconn, " update jsprodescom set numcom = '" & Codigo & "', codpro = '" & txtProveedor.Text & "' where codpro = '" & CodigoProveedorAnterior & "' and numcom = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myconn, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & NumeroDocumentoAnterior & "' and origen = 'COM' and id_emp = '" & jytsistema.WorkID & "' ")

        Dim porcentajeDescuento As Double = (1 - ValorNumero(txtDescuentos.Text) / ValorNumero(txtSubTotal.Text)) * 100
        Dim numCajas As Double = ft.DevuelveScalarDoble(myConn, " select SUM(CANTIDAD) from jsprorencom where numcom = '" & Codigo & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' group by numcom ")

        InsertEditCOMPRASEncabezadoCompras(myConn, lblInfo, Inserta, Codigo, txtNumeroSerie.Text, CDate(txtEmision.Text), CDate(txtEmisionIVA.Text), _
                txtProveedor.Text, txtComentario.Text, txtAlmacen.Text, txtReferencia.Text, txtCodigoContable.Text, dtRenglones.Rows.Count, numCajas, _
                ValorCantidad(tslblPesoT.Text), ValorNumero(txtSubTotal.Text), porcentajeDescuento, ValorNumero(txtDescuentos.Text), _
                ValorNumero(txtCargos.Text), ValorNumero(txtTotal.Text), 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, _
                FechaDocumentoPago, jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, FechaVencimiento, _
                CondicionDePago, TipoCredito, FormaPago, NumeroPago, NombrePago, Beneficiario, Caja, 0.0, "", 0, 0, 0.0, 0.0, "", jytsistema.sFechadeTrabajo, _
                ValorNumero(txtTotalIVA.Text), 0.0, 0.0, "", jytsistema.sFechadeTrabajo, 0.0, "", jytsistema.sFechadeTrabajo, _
                0.0, 0.0, "", "", "", Impresa, CodigoProveedorAnterior, NumeroDocumentoAnterior)

        InsertEditCONTROLNumeroControl(myConn, Codigo, txtProveedor.Text, txtControl.Text, CDate(txtEmisionIVA.Text), "COM", "COM")

        ActualizarMovimientos(Codigo, txtProveedor.Text)
        ActualizarGanancias(Codigo, txtProveedor.Text)

        'ACTUALIZAR RENGLONES DESDE RECEPCIONES Y ORDENES DE COMPRA
        'Actualiza Cantidades en tránsito de las ordenes de compra del documento
        ActualizarRenglonesEnOrdenesDeCompra(myConn, lblInfo, ds, dtRenglones, "jsprorencom")
        'Actualiza Cantidades en tránsito de las RECEPCIONES
        ActualizarRenglonesEnRecepciones(myConn, lblInfo, ds, dtRenglones, "jsprorencom")

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" NUMCOM = '" & Codigo & "' AND CODPRO = '" & txtProveedor.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub ActualizarInventarios(ByVal NumeroCompra As String, ByVal CodigoProveedor As String)

        '1.- Elimina movimientos de inventarios anterior compra
        EliminarMovimientosdeInventario(myConn, NumeroCompra, "COM", lblInfo, , , CodigoProveedor)

        ft.Ejecutar_strSQL(myconn, "UPDATE jsproenccom SET vence3 = '" & IIf(lblRecibido.Visible, ft.FormatoFechaMySQL(CDate(txtFechaRecibido.Text)), _
                                                                                  ft.FormatoFechaMySQL(CDate(txtEmision.Text))) & "', " _
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

                If .Item("item").ToString.Substring(0, 1) <> "$" Then
                    InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(txtEmision.Text), "EN", NumeroCompra, _
                                                         .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("costotot"), _
                                                         .Item("costototdes"), "COM", NumeroCompra, .Item("lote"), txtProveedor.Text, _
                                                          0.0, 0.0, 0, 0.0, "", _
                                                         txtAlmacen.Text, .Item("renglon"), jytsistema.sFechadeTrabajo)
                    Dim MontoUltimaCompra As Double = IIf(.Item("cantidad") > 0, .Item("costototdes") / .Item("cantidad"), .Item("costototdes")) / Equivalencia(myConn, .Item("item"), .Item("unidad"))

                    ft.Ejecutar_strSQL(myConn, " update jsmerctainv set fecultcosto = '" & ft.FormatoFechaMySQL(CDate(txtEmision.Text)) & "', ultimoproveedor = '" & txtProveedor.Text & "', montoultimacompra = " & MontoUltimaCompra & " where codart = '" & .Item("item") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                    If .Item("NUMREC") <> "" Then EliminarMovimientosdeInventario(myConn, .Item("NUMREC"), "REC", lblInfo, , , txtProveedor.Text, .Item("ITEM"), .Item("RENREC"))

                End If

                ActualizarExistenciasPlus(myConn, .Item("item"))

            End With
        Next
    End Sub

    Private Sub ActualizarMovimientos(ByVal NumeroCompra As String, ByVal CodigoProveedor As String)

        '1.- Aplica Descuento Global sobre total Renglón con descuento "costototdes"
        ft.Ejecutar_strSQL(myconn, " update jsprorencom set costototdes = costotot - costotot * " & ValorNumero(txtDescuentos.Text) / IIf(ValorNumero(txtSubTotal.Text) > 0, ValorNumero(txtSubTotal.Text), 1) & " " _
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
                ft.mensajeInformativo("LA MERCANCIA INCLUIDA EN ESTA COMPRA *NO* FUE ACTUALIZADA EN LOS INVENTARIOS. DEBE SER INCLUIDA MEDIANTE EL PROCESO...")
            End If
        End If

        '4.- Actualizar existencias de renglones eliminados
        For Each aSTR As Object In Eliminados
            ActualizarExistenciasPlus(myConn, aSTR)
        Next

        '5.- Actualizar CxP si es un Compra a crédito
        If CondicionDePago = CondicionPago.iCredito Then
            ft.Ejecutar_strSQL(myconn, " DELETE from jsprotrapag WHERE " _
                                            & " TIPOMOV = 'FC' AND  " _
                                            & " codpro = '" & CodigoProveedorAnterior & "' and " _
                                            & " NUMMOV = '" & NumeroDocumentoAnterior & "' AND " _
                                            & " ORIGEN = 'COM' AND " _
                                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                            & " ID_EMP = '" & jytsistema.WorkID & "'  ")

            InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, "FC", NumeroCompra, CDate(txtEmision.Text), ft.FormatoHora(Now), _
                                FechaVencimiento, txtReferencia.Text, "COMPRA N° : " & NumeroCompra, -1 * ValorNumero(txtTotal.Text), _
                                ValorNumero(txtTotalIVA.Text), "", "", "", "", "COM", "", "", "", Caja, NumeroCompra, "0", "", CDate(txtEmisionIVA.Text), _
                                txtCodigoContable.Text, "", "", 0.0, 0.0, "", "", "", "", "", "", "0", "0", "0")


        Else '6.- Actualizar Caja y Bancos si es un Gasto a contado
            ' 6.1.- Elimina movimientos anteriores de caja y bancos
            ft.Ejecutar_strSQL(myconn, "DELETE FROM jsbantracaj WHERE " _
                        & " ORIGEN = 'COM' AND " _
                        & " TIPOMOV = 'SA' AND " _
                        & " NUMMOV = '" & NumeroDocumentoAnterior & "' AND " _
                        & " PROV_CLI = '" & CodigoProveedorAnterior & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "'")

            ft.Ejecutar_strSQL(myconn, "DELETE FROM jsbantraban WHERE " _
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
                                                CDate(txtEmisionIVA.Text), CodigoProveedor, "", "1")

                    CalculaSaldoCajaPorFP(myConn, Caja, FormaPago, lblInfo)

                Case "CH", "TA", "TR"

                    InsertEditBANCOSMovimientoBanco(myConn, lblInfo, True, FechaDocumentoPago, NumeroPago, IIf(FormaPago = "CH", FormaPago, "ND"), _
                                                    NombrePago, "", "COMPRA N° : " & txtCodigo.Text, -1 * ValorNumero(txtTotal.Text), "COM", txtCodigo.Text, _
                                                    Beneficiario, txtCodigo.Text, "0", jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, "FC", _
                                                    "", CDate(txtEmisionIVA.Text), "0", txtProveedor.Text, "")

                    IncluirImpuestoDebitoBancario(myConn, lblInfo, IIf(FormaPago = "CH", FormaPago, "ND"), NombrePago, NumeroPago, FechaDocumentoPago, ValorNumero(txtTotal.Text))

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
                        ft.Ejecutar_strSQL(myconn, "UPDATE jsmerctainv SET " & strSQLPrecios _
                                                & " WHERE CODART = '" & .Item("ITEM") & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                    End With

                Next
            End If
        End If

    End Sub
    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique comentario ... ", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtProveedor, btnProveedor, txtCodigo, txtNumeroSerie)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        i_modo = movimiento.iEditar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPACOM06")) Then
            With dt.Rows(nPosicionEncab)
                Dim aCamposAdicionales() As String = {"numcom|'" & txtCodigo.Text & "'", _
                                                      "codpro|'" & txtProveedor.Text & "'"}
                If DocumentoBloqueado(myConn, "jsproenccom", aCamposAdicionales) Then
                    ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                Else
                    If DocumentoPoseeRetencionIVA(myConn, lblInfo, .Item("numcom"), .Item("codpro")) Then
                        ft.mensajeCritico("Esta FACTURA posee RETENCION DE IVA. MODIFICACION NO esta permitida ...")
                    Else
                        If ft.DevuelveScalarEntero(myConn, " select count(*) from jsprotrapag " _
                                                      & " where " _
                                                          & " tipomov <> 'FC' and ORIGEN <> 'COM' AND " _
                                                          & " codpro = '" & .Item("codpro") & "' and " _
                                                          & " nummov = '" & .Item("numcom") & "' and " _
                                                          & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then

                            ActivarMarco0()
                            ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtProveedor, btnProveedor, txtCodigo, txtNumeroSerie)
                        Else
                            If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) = 0 Then
                                ft.mensajeCritico("Esta FACTURA posee movimientos asociados. MODIFICACION NO esta permitida ...")
                            Else
                                ActivarMarco0()
                            End If
                        End If
                    End If
                End If
            End With
        Else
            ft.mensajeCritico("Este Documento no puede ser MODIFICADO ... ")
        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPACOM06")) Then

            With dt.Rows(nPosicionEncab)
                Dim aCamposAdicionales() As String = {"numcom|'" & txtCodigo.Text & "'", _
                                                      "codpro|'" & txtProveedor.Text & "'"}
                If DocumentoBloqueado(myConn, "jsproenccom", aCamposAdicionales) Then
                    ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                Else
                    If DocumentoPoseeRetencionIVA(myConn, lblInfo, .Item("numcom"), .Item("codpro")) Then
                        ft.mensajeCritico("Esta FACTURA posee RETENCION DE IVA. MODIFICACION NO esta permitida ...")
                    Else
                        If ft.DevuelveScalarEntero(myConn, " select count(*) from jsprotrapag " _
                                                      & " where " _
                                                      & " tipomov <> 'FC' AND ORIGEN <> 'COM' AND " _
                                                      & " codpro = '" & .Item("codpro") & "' and " _
                                                      & " nummov = '" & .Item("numcom") & "' and " _
                                                      & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then


                            nPosicionEncab = Me.BindingContext(ds, nTabla).Position

                            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

                                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numcom"))

                                If .Item("CONDPAG") = 1 And (.Item("FORMAPAG") = "CH" _
                                                             Or .Item("FORMAPAG") = "TR" _
                                                             Or .Item("FORMAPAG") = "TA") Then
                                    EliminarImpuestoDebitoBancario(myConn, lblInfo, .Item("NOMPAG"), .Item("NUMPAG"), _
                                                                   CDate(.Item("VENCE1").ToString))
                                End If

                                For Each dtRow As DataRow In dtRenglones.Rows
                                    With dtRow
                                        Eliminados.Add(.Item("item"))
                                        .Item("cantidad") = 0.0
                                    End With
                                Next


                                ft.Ejecutar_strSQL(myConn, " delete from jsproenccom where numcom = '" & txtCodigo.Text & "' AND CODPRO = '" & txtProveedor.Text & "' and ID_EMP = '" & jytsistema.WorkID & "'")
                                ft.Ejecutar_strSQL(myConn, " delete from jsprorencom where numcom = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " delete from jsprodescom where numcom = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " delete from jsproivacom where numcom = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'COM' and id_emp = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " DELETE FROM jsbantraban where ORIGEN = 'COM' AND NUMORG = '" & txtCodigo.Text & "' AND prov_cli = '" & txtProveedor.Text & "' and EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " DELETE FROM jsbantracaj where nummov = '" & txtCodigo.Text & "' AND prov_cli = '" & txtProveedor.Text & "' and origen = 'COM' AND tipomov = 'SA' AND ejercicio = '" & jytsistema.WorkExercise & "' AND id_emp = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " DELETE FROM jsprotrapag where CODPRO = '" & txtProveedor.Text & "' AND TIPOMOV = 'FC' AND NUMMOV = '" & txtCodigo.Text & _
                                               "' AND ORIGEN = 'COM' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")

                                EliminarMovimientosdeInventario(myConn, txtCodigo.Text, "COM", lblInfo, , , txtProveedor.Text)

                                ActualizarRenglonesEnOrdenesDeCompra(myConn, lblInfo, ds, dtRenglones, "jsprorencom")
                                ActualizarRenglonesEnRecepciones(myConn, lblInfo, ds, dtRenglones, "jsprorenrep")

                                For Each aSTR As Object In Eliminados
                                    ActualizarExistenciasPlus(myConn, aSTR)
                                Next

                                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                                dt = ds.Tables(nTabla)
                                If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                                If dt.Rows.Count > 0 Then
                                    AsignaTXT(nPosicionEncab)
                                Else
                                    IniciarDocumento(False)
                                End If


                            End If
                        Else
                            ft.mensajeCritico("Esta FACTURA posee movimientos asociados. ELIMINACION NO esta permitida ...")
                        End If
                    End If
                End If
            End With

        Else
            ft.mensajeCritico("Este Documento no puede ser ELIMINADO...")
        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numcom", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Nombre", "Emisión", "Comentario"}
        Dim Anchos() As Integer = {150, 400, 100, 150}
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
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub CalculaTotales()

        Dim bDesc As Boolean = False



        If Not DocumentoPoseeRetencionIVA(myConn, lblInfo, NumeroDocumentoAnterior, CodigoProveedorAnterior) Then

            txtSubTotal.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsprorencom", "numcom", "costotot", NumeroDocumentoAnterior, 0, CodigoProveedorAnterior))

            txtDescuentos.Text = ft.FormatoNumero(AbrirDescuentos(NumeroDocumentoAnterior, CodigoProveedorAnterior))

            txtCargos.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsprorencom", "numcom", "costotot", NumeroDocumentoAnterior, 1, CodigoProveedorAnterior))

            bDesc = CalculaDescuentoEnRenglones(ValorNumero(txtDescuentos.Text), ValorNumero(txtSubTotal.Text))

            txtTotalIVA.Text = ft.FormatoNumero(CalculaTotalIVACompras(myConn, lblInfo, CodigoProveedorAnterior, "jsprodescom", "jsproivacom", "jsprorencom", "numcom", NumeroDocumentoAnterior, "impiva", "costototdes", CDate(txtEmision.Text), "costotot"))

            AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)

            txtTotal.Text = ft.FormatoNumero(ValorNumero(txtSubTotal.Text) - ValorNumero(txtDescuentos.Text) + ValorNumero(txtCargos.Text) + ValorNumero(txtTotalIVA.Text))

            tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsprorencom", "peso", "numcom", NumeroDocumentoAnterior))
        Else

            txtDescuentos.Text = ft.FormatoNumero(AbrirDescuentos(NumeroDocumentoAnterior, CodigoProveedorAnterior))
            AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)
            txtTotal.Text = ft.FormatoNumero(ValorNumero(txtSubTotal.Text) - ValorNumero(txtDescuentos.Text) + ValorNumero(txtCargos.Text) + ValorNumero(txtTotalIVA.Text))
            tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsprorencom", "peso", "numcom", NumeroDocumentoAnterior))

        End If

    End Sub
    Private Function CalculaDescuentoEnRenglones(TotalDescuentos As Double, SubTotalCompra As Double) As Boolean



        ft.Ejecutar_strSQL(myConn, " update jsprorencom set costototdes = costotot - costotot * " & TotalDescuentos / IIf(SubTotalCompra > 0, SubTotalCompra, 1) & " " _
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
                ft.MensajeCritico("DEBE INDICAR UN PROVEEDOR VALIDO...")
            End If
        Else
            ft.MensajeCritico("DEBE INDICAR UN NÚMERO DE COMPRA VALIDO...")
        End If
    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        If dtRenglones.Rows.Count > 0 Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position

            f.Editar(myConn, ds, dtRenglones, "COM", NumeroDocumentoAnterior, CDate(txtEmision.Text), txtAlmacen.Text, , , _
                     IIf(dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position).Item("item").ToString.Substring(0, 1) = "$", True, False), _
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

      nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                With dtRenglones.Rows(nPosicionRenglon)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("item"))

                    Eliminados.Add(.Item("item"))

                    Dim aCamposDel() As String = {"numcom", "codpro", "item", "renglon", "id_emp"}
                    Dim aStringsDel() As String = {NumeroDocumentoAnterior, CodigoProveedorAnterior, .Item("item"), .Item("renglon"), jytsistema.WorkID}

                    ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & NumeroDocumentoAnterior & "' and " _
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
        Dim Anchos() As Integer = {140, 350}
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
        If i_modo = movimiento.iAgregar Then txtFechaRecibido.Text = txtEmision.Text
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
        If e.ColumnIndex = 1 Then e.Value = ft.FormatoNumero(e.Value) & "%"
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
        txtNombreAlmacen.Text = ft.DevuelveScalarCadena(myConn, " select desalm from jsmercatalm where codalm = '" & txtAlmacen.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
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
                Dim aAnchos() As Integer = {20, 120, 120, 200}
                Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Right}
                Dim aFormato() As String = {"", "", sFormatoFecha, sFormatoNumero}
                Dim aFields() As String = {"sel.entero.1.0", "codigo.cadena.20.0", "emision.fecha.0.0", "tot_ord.doble.19.2"}

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
                        Dim nTablaRenglonesOrden As String = "tblRenglonesOrden" & ft.NumeroAleatorio(100000)
                        Dim dtRenglonesOrden As New DataTable
                        ds = DataSetRequery(ds, " select * from jsprorenord " _
                                            & " where " _
                                            & " cantran > 0 AND " _
                                            & " numord = '" & cod & "' and " _
                                            & " codpro = '" & txtProveedor.Text & "' and " _
                                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon ", myConn, nTablaRenglonesOrden, lblInfo)

                        dtRenglonesOrden = ds.Tables(nTablaRenglonesOrden)

                        For Each rRow As DataRow In dtRenglonesOrden.Rows
                            With rRow
                                Dim pesoTransito As Double = .Item("peso") / .Item("cantidad") * .Item("cantran")

                                Dim numRenglon As String = ft.autoCodigo(myConn, "RENGLON", "jsprorencom", "numcom.codpro.id_emp", _
                                                                      txtCodigo.Text + "." + txtProveedor.Text + "." + jytsistema.WorkID, 5)

                                InsertEditCOMPRASRenglonCOMPRAS(myConn, lblInfo, True, txtCodigo.Text, numRenglon, _
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
                ft.MensajeCritico("DEBE INDICAR UN PROVEEDOR VALIDO...")
            End If
        Else
            ft.MensajeCritico("DEBE INDICAR UN NUMERO DE COMPRA VALIDO...")
        End If

    End Sub

    Private Sub btnRecepciones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecepciones.Click
        If NumeroDocumentoAnterior.Trim() <> "" Then
            If txtNombreProveedor.Text <> "" Then
                CodigoProveedorAnterior = txtProveedor.Text
                Dim f As New jsGenListadoSeleccion
                Dim aNombres() As String = {"", "Recepción N°", "Emisión", "Monto"}
                Dim aCampos() As String = {"sel", "codigo", "emision", "tot_rec"}
                Dim aAnchos() As Integer = {20, 120, 120, 200}
                Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Right}
                Dim aFormato() As String = {"", "", sFormatoFecha, sFormatoNumero}
                Dim aFields() As String = {"sel.entero.1.0", "codigo.cadena.20.0", "emision.fecha.0.0", "tot_rec.doble.19.2"}

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
                        Dim nTablaRenglonesOrden As String = "tblRenglonesOrden" & ft.NumeroAleatorio(100000)
                        Dim dtRenglonesOrden As New DataTable
                        ds = DataSetRequery(ds, " select * from jsprorenrep where numrec = '" & cod & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' order by renglon ", myConn, nTablaRenglonesOrden, lblInfo)
                        dtRenglonesOrden = ds.Tables(nTablaRenglonesOrden)

                        For Each rRow As DataRow In dtRenglonesOrden.Rows
                            With rRow
                                Dim pesoTransito As Double = .Item("peso") / .Item("cantidad") * .Item("cantran")

                                Dim numRenglon As String = ft.autoCodigo(myConn, "RENGLON", "jsprorencom", _
                                                                     "numcom.codpro.id_emp", _
                                                                     txtCodigo.Text + "." + txtProveedor.Text + "." + jytsistema.WorkID, 5)

                                InsertEditCOMPRASRenglonCOMPRAS(myConn, lblInfo, True, txtCodigo.Text, numRenglon, _
                                    txtProveedor.Text, .Item("item"), .Item("descrip"), .Item("iva"), "", .Item("unidad"), 0.0, .Item("cantran"), pesoTransito, "", "", "", _
                                    .Item("estatus"), .Item("costou"), .Item("des_art"), .Item("des_pro"), .Item("costou") * .Item("cantran"), _
                                    .Item("costou") * .Item("cantran") * (1 - .Item("des_art") / 100) * (1 - .Item("des_pro") / 100), .Item("numord"), .Item("renord"), .Item("numrec"), .Item("renglon"), "", "1")


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
                ft.MensajeCritico("DEBE INDICAR UN PROVEEDOR VALIDO...")
            End If
        Else
            ft.MensajeCritico("DEBE INDICAR UN NÚMERO DE COMPRA VALIDO...")
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
            ft.mensajeInformativo("Proceso terminado!!! ")
        End If
    End Sub

    Private Sub btnAuditoria_Click(sender As Object, e As EventArgs) Handles btnAuditoria.Click
        If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) > 0 Then
            Dim f As New jsControlArcAccesosPlus
            f.nModulo = sModulo
            f.Cargar()
            f.Dispose()
            f = Nothing
        End If
    End Sub
End Class