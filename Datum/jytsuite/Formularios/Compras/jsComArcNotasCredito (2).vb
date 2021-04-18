Imports MySql.Data.MySqlClient
Public Class jsComArcNotasCredito
    Private Const sModulo As String = "Notas Crédito"
    Private Const lRegion As String = "RibbonButton62"
    Private Const nTabla As String = "tblEncab"
    Private Const nTablaRenglones As String = "tblRenglones_"
    Private Const nTablaIVA As String = "tblIVA"
    Private Const nTablaDescuentos As String = "tblDescuentos"

    Private strSQL As String = " (select a.*, b.nombre from jsproencncr a " _
            & " left join jsprocatpro b on (a.codpro = b.codpro and a.id_emp = b.id_emp) " _
            & " where a.id_emp = '" & jytsistema.WorkID & "' order by a.emision, a.numncr desc ) order by emision, numncr "

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
    Private nPosicionEncab As Long, nPosicionRenglon As Long


    Private Eliminados As New ArrayList

    Private Impresa As Integer
    Private CodigoProveedorAnterior As String = ""
    Private NumeroDocumentoAnterior As String = ""
    Private Sub jsComArcNotasDebito_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva Nota Crédito")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> Nota Crédito")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> Nota Crédito")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> Nota Crédito")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primer</B> Nota Crédito")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a Nota Crédito <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a Nota Crédito <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>último Nota Crédito</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> Nota Crédito")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnDuplicar, "<B>Duplicar</B> este Nota Crédito")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en Nota Crédito")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón en Nota Crédito")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón en Nota Crédito")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en Nota Crédito")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón en Nota Crédito")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón en Nota Crédito")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> en Nota Crédito")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la Nota Crédito")

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

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtCodigo.Text = MuestraCampoTexto(.Item("numncr"))
                txtNumeroSerie.Text = MuestraCampoTexto(.Item("serie_numncr"))
                NumeroDocumentoAnterior = .Item("numncr")
                txtEmision.Text = FormatoFecha(CDate(.Item("emision").ToString))
                txtFacturaAfectada.Text = .Item("numcom")
                txtEmisionIVA.Text = FormatoFecha(CDate(.Item("emisioniva").ToString))
                txtProveedor.Text = MuestraCampoTexto(.Item("codpro"))
                CodigoProveedorAnterior = .Item("codpro")
                txtReferencia.Text = MuestraCampoTexto(.Item("refer"))
                Dim numControl As String = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select num_control from jsconnumcon where numdoc = '" & .Item("numncr") & "' and  " _
                                                                 & " prov_cli = '" & .Item("codpro") & "' and " _
                                                                 & " origen = 'COM' and org = 'NCR' and id_emp = '" & jytsistema.WorkID & "' ")

                txtControl.Text = IIf(numControl <> "0", numControl, "")
                txtComentario.Text = MuestraCampoTexto(.Item("comen"))
                txtAlmacen.Text = MuestraCampoTexto(.Item("almacen"))
                txtCodigoContable.Text = MuestraCampoTexto(.Item("codcon"))

                tslblPesoT.Text = FormatoCantidad(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, "select if( sum(peso) is null, 0.000, sum(peso)) from jsprorenncr where numncr = '" & .Item("numncr") & "' and codpro = '" & .Item("codpro") & "' and id_emp = '" & jytsistema.WorkID & "' ")))

                txtSubTotal.Text = FormatoNumero(.Item("tot_net"))
                txtTotalIVA.Text = FormatoNumero(.Item("imp_iva"))
                txtTotal.Text = FormatoNumero(.Item("tot_ncr"))

                'Renglones
                AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)

                'Totales
                CalculaTotales()

            End With
        End With
    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroCompra As String, ByVal CodigoProveedor As String)

        strSQLMov = "select * from jsprorenncr " _
                            & " where " _
                            & " numncr  = '" & NumeroCompra & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"item", "descrip", "iva", "cantidad", "unidad", "precio", "por_acepta_dev", "totren", "estatus", ""}
        Dim aNombres() As String = {"Item", "Descripción", "IVA", "Cant.", "UND", "Costo Unitario", "% Acep.", "Costo Total", "Tipo Renglon", ""}
        Dim aAnchos() As Long = {90, 300, 30, 60, 45, 80, 60, 90, 60, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", sFormatoCantidad, "", sFormatoNumero, sFormatoNumero, _
                                     sFormatoNumero, "", ""}
        IniciarTabla(dg, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub
    Private Sub AbrirIVA(ByVal NumeroDocumento As String, ByVal CodigoProveedor As String)

        strSQLIVA = "select * from jsproivancr " _
                            & " where " _
                            & " numncr = '" & NumeroDocumento & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
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

        txtTotalIVA.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(impiva) from jsproivancr where numncr = '" & NumeroDocumentoAnterior & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' group by numncr ")))



    End Sub

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)


        txtCodigo.Text = ""

        NumeroDocumentoAnterior = "TMPNCC" & NumeroAleatorio(1000000)
        CodigoProveedorAnterior = "PRTMP" & NumeroAleatorio(100000)
        txtNumeroSerie.Text = ""

        IniciarTextoObjetos(FormatoItemListView.iCadena, txtControl, txtProveedor, txtNombreProveedor, txtComentario, _
                            txtAlmacen, txtReferencia, txtCodigoContable, txtFacturaAfectada)

        Dim nAlmacen As String = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, "SELECT codalm FROM jsmercatalm WHERE id_emp = '" & jytsistema.WorkID & "' ORDER BY codalm LIMIT 1"))
        If nAlmacen <> "0" Then txtAlmacen.Text = nAlmacen

        txtEmision.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        txtEmisionIVA.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        tslblPesoT.Text = FormatoCantidad(0)
        dgIVA.Columns.Clear()
        IniciarTextoObjetos(FormatoItemListView.iNumero, txtSubTotal, txtTotalIVA, txtTotal)

        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", 0, 0)
        AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
        AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)

    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        HabilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon)
        HabilitarObjetos(True, True, txtCodigo, txtNumeroSerie, txtComentario, btnEmision, btnProveedor, txtProveedor, btnEmisionIVA, _
                         txtControl, btnEmision, txtAlmacen, btnAlmacen, _
                         txtReferencia, btnCodigoContable, txtFacturaAfectada)

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        HabilitarObjetos(False, True, txtCodigo, txtNumeroSerie, txtEmision, btnEmision, txtControl, txtEmisionIVA, btnEmisionIVA, _
                txtProveedor, txtNombreProveedor, btnProveedor, txtComentario, _
                txtAlmacen, btnAlmacen, txtNombreAlmacen, txtReferencia, txtCodigoContable, btnCodigoContable, txtFacturaAfectada)

        HabilitarObjetos(False, True, txtSubTotal, txtTotalIVA, txtTotal)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
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

        EjecutarSTRSQL(myConn, lblInfo, " delete from jsprorenncr where numncr = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsproivancr where numncr = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'NCC' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
            Imprimir()
        End If
    End Sub
    Private Sub Imprimir()

        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cNotaCredito, "Nota Crédito", txtProveedor.Text, txtCodigo.Text, CDate(txtEmision.Text))
        '///ImprimirFacturaGrafica(myConn, lblInfo, ds, txtCodigo.Text)

    End Sub
    Private Function Validado() As Boolean

        If txtCodigo.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un Número de Nota Crédito válida...")
            Return False
        End If

        If txtNombreProveedor.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un proveedor válido...")
            Return False
        End If

        If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(*) from jsproencncr where numncr = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "'  ")) > 0 AndAlso _
            i_modo = movimiento.iAgregar Then
            MensajeCritico(lblInfo, "Número de Nota Crédito YA existe para este proveedor ...")
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

        Return True

    End Function
    Private Sub GuardarTXT()

        Dim Codigo As String = txtCodigo.Text
        Dim Inserta As Boolean = False

        If i_modo = movimiento.iAgregar Then

            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count

        End If

        EjecutarSTRSQL(myConn, lblInfo, " update jsprorenncr set numncr = '" & Codigo & "', codpro = '" & txtProveedor.Text & "' where codpro = '" & CodigoProveedorAnterior & "' and numncr = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " update jsproivancr set numncr = '" & Codigo & "', codpro = '" & txtProveedor.Text & "' where codpro = '" & CodigoProveedorAnterior & "' and numncr = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & NumeroDocumentoAnterior & "' and origen = 'NCC' and id_emp = '" & jytsistema.WorkID & "' ")

        Dim numCajas As Double = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(cantidad) from jsprorenncr where numncr = '" & Codigo & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' group by numncr ")

        InsertEditCOMPRASEncabezadoNOTACREDITO(myConn, lblInfo, Inserta, Codigo, txtNumeroSerie.Text, txtFacturaAfectada.Text, CDate(txtEmision.Text), CDate(txtEmisionIVA.Text), _
                txtProveedor.Text, txtComentario.Text, "", txtAlmacen.Text, "", txtReferencia.Text, txtCodigoContable.Text, "", dtRenglones.Rows.Count, numCajas, _
                ValorCantidad(tslblPesoT.Text), ValorNumero(txtSubTotal.Text), ValorNumero(txtTotalIVA.Text), ValorNumero(txtTotal.Text), jytsistema.sFechadeTrabajo, _
                "0", "", jytsistema.sFechadeTrabajo, Impresa, _
                CodigoProveedorAnterior, NumeroDocumentoAnterior)

        InsertEditCONTROLNumeroControl(myConn, lblInfo, Inserta, Codigo, txtProveedor.Text, txtControl.Text, CDate(txtEmisionIVA.Text), "COM", "NCR", NumeroDocumentoAnterior, CodigoProveedorAnterior)

        ActualizarMovimientos(Codigo, txtProveedor.Text)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" NUMNCR = '" & Codigo & "' AND CODPRO = '" & txtProveedor.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub ActualizarMovimientos(ByVal NumeroNotaDebito As String, ByVal CodigoProveedor As String)

        '1.- Elimina movimientos de inventarios anteriores DE LA NOTA DEBITO
        EliminarMovimientosdeInventario(myConn, NumeroNotaDebito, "NCC", lblInfo, , , CodigoProveedor)

        '2.- Actualizar Movimientos de Inventario con los de LA NOTA CREDITO
        strSQLMov = "select * from jsprorenncr " _
                            & " where " _
                            & " numncr  = '" & NumeroNotaDebito & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        For Each dtRow As DataRow In dtRenglones.Rows
            With dtRow
                If .Item("item").ToString.Substring(1, 1) <> "$" Then

                    Dim CausaDebito As String = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select codigo from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "' "))

                    If CausaDebito <> "0" Then
                        Dim MueveInventario As Boolean = CBool(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select inventario from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "' "))
                        If MueveInventario Then _
                        InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(txtEmision.Text), "SA", NumeroNotaDebito, _
                                                             .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("totren"), _
                                                             .Item("totrendes"), "NCC", NumeroNotaDebito, .Item("lote"), CodigoProveedor, _
                                                             0.0, 0.0, 0, 0.0, "", _
                                                             txtAlmacen.Text, .Item("renglon"), jytsistema.sFechadeTrabajo)

                        Dim AjustaPrecio As Boolean = CBool(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select ajustaprecio from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "' "))
                        If AjustaPrecio Then _
                            InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(txtEmision.Text), "AC", NumeroNotaDebito, _
                                                                                 .Item("unidad"), 0.0, 0.0, .Item("totren"), _
                                                                                 .Item("totrendes"), "NCC", .Item("numncr"), .Item("lote"), txtProveedor.Text, _
                                                                                 0.0, 0.0, 0, 0.0, "", _
                                                                                 txtAlmacen.Text, .Item("renglon"), jytsistema.sFechadeTrabajo)
                    Else

                        InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(txtEmision.Text), "SA", NumeroNotaDebito, _
                                                         .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("totren"), _
                                                         .Item("totrendes"), "NCC", NumeroNotaDebito, .Item("lote"), txtProveedor.Text, _
                                                         0.0, 0.0, 0, 0.0, "", _
                                                         txtAlmacen.Text, .Item("renglon"), jytsistema.sFechadeTrabajo)

                    End If

                    ActualizarExistenciasPlus(myConn, .Item("item"))

                End If
            End With
        Next

        '3.- Actualizar existencias de renglones eliminados
        For Each aSTR As Object In Eliminados
            ActualizarExistenciasPlus(myConn, aSTR)
        Next

        '4.- Actualizar CxP 
        EjecutarSTRSQL(myConn, lblInfo, " DELETE from jsprotrapag WHERE " _
                                            & " TIPOMOV = 'NC' AND  " _
                                            & " codpro = '" & CodigoProveedorAnterior & "' and " _
                                            & " NUMMOV = '" & NumeroDocumentoAnterior & "' AND " _
                                            & " ORIGEN = 'NCR' AND " _
                                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                            & " ID_EMP = '" & jytsistema.WorkID & "'  ")

        InsertEditCOMPRASCXP(myConn, lblInfo, True, txtProveedor.Text, "NC", NumeroNotaDebito, CDate(txtEmision.Text), FormatoHora(Now), _
                            CDate(txtEmision.Text), txtReferencia.Text, "NOTA CREDITO N° : " & NumeroNotaDebito, ValorNumero(txtTotal.Text), _
                            ValorNumero(txtTotalIVA.Text), "", "", "", "", "NCR", "", "", "", "", NumeroNotaDebito, "0", "", jytsistema.sFechadeTrabajo, _
                            txtCodigoContable.Text, "", "", 0.0, 0.0, "", "", "", "", "", "", "0", "1", "0")

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
        If nPosicionEncab >= 0 Then
            With dt.Rows(nPosicionEncab)
                If DocumentoPoseeRetencionIVA(myConn, lblInfo, .Item("numncr"), .Item("codpro")) Then
                    MensajeCritico(lblInfo, "Esta NOTA DE CREDITO posee RETENCION DE IVA. MODIFICACION NO esta permitida ...")
                Else
                    If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(*) from jsprotrapag " _
                                                  & " where " _
                                                  & " tipomov <> 'NC' and " _
                                                  & " codpro = '" & .Item("codpro") & "' and " _
                                                  & " nummov = '" & .Item("NUMNCR") & "' and " _
                                                  & " id_emp = '" & jytsistema.WorkID & "' ")) = 0 Then

                        ActivarMarco0()
                        HabilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtProveedor, btnProveedor, txtCodigo, txtNumeroSerie)
                    Else
                        If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) = 0 Then
                            MensajeCritico(lblInfo, "Esta NOTA CREDITO posee movimientos asociados. MODIFICACION NO esta permitida ...")
                        Else
                            ActivarMarco0()
                        End If
                    End If
                End If
            End With
        End If
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        With dt.Rows(nPosicionEncab)
            If DocumentoPoseeRetencionIVA(myConn, lblInfo, .Item("numncr"), .Item("codpro")) Then
                MensajeCritico(lblInfo, "Esta NOTA CREDITO posee RETENCION DE IVA. MODIFICACION NO esta permitida ...")
            Else
                If CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select count(*) from jsprotrapag " _
                                              & " where " _
                                              & " tipomov <> 'NC' and " _
                                              & " codpro = '" & .Item("codpro") & "' and " _
                                              & " nummov = '" & .Item("NUMNCR") & "' and " _
                                              & " id_emp = '" & jytsistema.WorkID & "' ")) = 0 Then


                    sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")

                    If sRespuesta = MsgBoxResult.Yes Then

                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numncr"))

                        For Each dtRow As DataRow In dtRenglones.Rows
                            With dtRow
                                Eliminados.Add(.Item("item"))
                            End With
                        Next

                        EjecutarSTRSQL(myConn, lblInfo, " delete from jsproencncr where numncr = '" & txtCodigo.Text & "' AND CODPRO = '" & txtProveedor.Text & "' and ID_EMP = '" & jytsistema.WorkID & "'")
                        EjecutarSTRSQL(myConn, lblInfo, " delete from jsprorenncr where numncr = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " delete from jsproivancr where numncr = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'NCC' and id_emp = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " DELETE FROM jsprotrapag where CODPRO = '" & txtProveedor.Text & "' AND TIPOMOV = 'NC' AND NUMMOV = '" & txtCodigo.Text _
                                       & "' AND ORIGEN = 'NCR' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                        EliminarMovimientosdeInventario(myConn, txtCodigo.Text, "NCC", lblInfo, , , txtProveedor.Text)

                        For Each aSTR As Object In Eliminados
                            ActualizarExistenciasPlus(myConn, aSTR)
                        Next

                        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                        dt = ds.Tables(nTabla)
                        If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                        AsignaTXT(nPosicionEncab)

                    End If
                Else
                    MensajeCritico(lblInfo, "Esta NOTA CREDITO posee movimientos asociados. ELIMINACION NO esta permitida ...")
                End If
            End If
        End With

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numncr", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Nombre", "Emisión", "Comentario"}
        Dim Anchos() As Long = {150, 400, 100, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Notas de Crédito Compras...")
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
            Case 8
                e.Value = aTipoRenglon(e.Value)
        End Select
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "ItemsRenglon", "lblItemsRenglon", e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub CalculaTotales()

        If Not DocumentoPoseeRetencionIVA(myConn, lblInfo, NumeroDocumentoAnterior, CodigoProveedorAnterior) Then

            txtSubTotal.Text = FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsprorenncr", "numncr", "totren", NumeroDocumentoAnterior, 0, CodigoProveedorAnterior))

            CalculaTotalIVACompras(myConn, lblInfo, CodigoProveedorAnterior, "", "jsproivancr", "jsprorenncr", "numncr", NumeroDocumentoAnterior, "impiva", "totrendes", CDate(txtEmision.Text), "totren")

            AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)

            txtTotal.Text = FormatoNumero(ValorNumero(txtSubTotal.Text) + ValorNumero(txtTotalIVA.Text))

            tslblPesoT.Text = FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsprorenncr", "peso", "numncr", NumeroDocumentoAnterior))

        Else

            AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)
            txtTotal.Text = FormatoNumero(ValorNumero(txtSubTotal.Text) + ValorNumero(txtTotalIVA.Text))
            tslblPesoT.Text = FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsprorenncr", "peso", "numncr", NumeroDocumentoAnterior))

        End If

    End Sub

    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        If txtNombreProveedor.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "NCC", NumeroDocumentoAnterior, CDate(txtEmision.Text), txtAlmacen.Text, _
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
            f.Editar(myConn, ds, dtRenglones, "NCC", NumeroDocumentoAnterior, CDate(txtEmision.Text), txtAlmacen.Text, , , _
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

                    Dim aCamposDel() As String = {"numncr", "item", "renglon", "id_emp"}
                    Dim aStringsDel() As String = {NumeroDocumentoAnterior, .Item("item"), .Item("renglon"), jytsistema.WorkID}

                    EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & NumeroDocumentoAnterior & "' and " _
                           & " origen = 'NCC' and item = '" & .Item("item") & "' and renglon = '" & .Item("renglon") & "' and " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsprorenncr", strSQLMov, aCamposDel, aStringsDel, _
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
        f.Buscar(dtRenglones, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaRenglones).Position, "Renglones de notas de crédito...")
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
            Dim aFld() As String = {"codpro", "id_emp"}
            Dim aStr() As String = {txtProveedor.Text, jytsistema.WorkID}
            Dim FormaDePagoCliente As String = qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "formapago")
            txtNombreProveedor.Text = qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "nombre")
            'If txtNombreProveedor.Text <> "" AndAlso i_modo = movimiento.iEditar Then CodigoProveedorAnterior = txtProveedor.Text
            Dim mTotalFac As Double = ValorNumero(txtTotal.Text)
            If i_modo = movimiento.iAgregar Then mTotalFac = 0.0
        End If
    End Sub

    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dgIVA.CellFormatting
        If e.ColumnIndex = 1 Then e.Value = FormatoNumero(e.Value) & "%"
    End Sub

    Private Sub btnCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProveedor.Click
        txtProveedor.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codpro codigo, nombre descripcion, RIF from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Proveedores De Compras/Gastos", _
                                            txtProveedor.Text)
    End Sub

    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If txtNombreProveedor.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "NCC", NumeroDocumentoAnterior, CDate(txtEmision.Text), txtAlmacen.Text, , , True, , , , , , , , , , CodigoProveedorAnterior)
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

  
    Private Sub btnReconstruir_Click(sender As System.Object, e As System.EventArgs) Handles btnReconstruir.Click
        '2.- Elimina movimientos de inventarios anteriores del gasto
        EliminarMovimientosdeInventario(myConn, txtCodigo.Text, "NCC", lblInfo, , , txtProveedor.Text)

        '3.- Actualizar Movimientos de Inventario con los del gasto
        For Each dtRow As DataRow In dtRenglones.Rows
            With dtRow
                If .Item("item").ToString.Substring(1, 1) <> "$" Then

                    If CausaMueveInventarioNotasCredito(myConn, lblInfo, .Item("CAUSA")) Then

                        InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(txtEmision.Text), "SA", txtCodigo.Text, _
                                                             .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("totren"), _
                                                             .Item("totrendes"), "NCC", txtCodigo.Text, .Item("lote"), txtProveedor.Text, _
                                                              0.0, 0.0, 0, 0.0, "", _
                                                             txtAlmacen.Text, .Item("renglon"), jytsistema.sFechadeTrabajo)

                        'Dim MontoUltimaCompra As Double = UltimoCostoAFecha(myConn, lblInfo, .Item("item"), jytsistema.sFechadeTrabajo) ' IIf(.Item("cantidad") > 0, .Item("costototdes") / .Item("cantidad"), .Item("costototdes")) / Equivalencia(myConn, lblInfo, .Item("item"), .Item("unidad"))
                        'Dim nUltimoProveedor As String = UltimoProveedor(myConn, lblInfo, .Item("item"), jytsistema.sFechadeTrabajo)
                        'Dim nUltimaFechaCompra As Date = UltimaFechaCompra(myConn, lblInfo, .Item("item"), jytsistema.sFechadeTrabajo)

                        'EjecutarSTRSQL(myConn, lblInfo, " update jsmerctainv set fecultcosto = '" & FormatoFechaMySQL(nUltimaFechaCompra) _
                        '               & "', ultimoproveedor = '" & nUltimoProveedor _
                        '               & "', montoultimacompra = " & MontoUltimaCompra & " where codart = '" & .Item("item") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    End If

                End If

                ActualizarExistenciasPlus(myConn, .Item("item"))

            End With
        Next

        MensajeInformativoPlus("Proceso terminado!!! ")
    End Sub
End Class