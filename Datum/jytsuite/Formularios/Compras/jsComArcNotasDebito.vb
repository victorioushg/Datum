Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

Public Class jsComArcNotasDebito
    Private Const sModulo As String = "Compras"
    Private Const lRegion As String = "RibbonButton63"
    Private Const nTabla As String = "tblEncab"
    Private Const nTablaRenglones As String = "tblRenglones_"
    Private Const nTablaIVA As String = "tblIVA"
    Private Const nTablaDescuentos As String = "tblDescuentos"

    Private strSQL As String = " (select a.*, b.nombre from jsproencndb a " _
            & " left join jsprocatpro b on (a.codpro = b.codpro and a.id_emp = b.id_emp) " _
            & " where a.id_emp = '" & jytsistema.WorkID & "' order by a.emision, a.numndb desc ) order by emision, numndb "

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
    Private nPosicionEncab As Long, nPosicionRenglon As Long


    Private Eliminados As New ArrayList

    Private Impresa As Integer
    Private CodigoProveedorAnterior As String = ""
    Private NumeroDocumentoAnterior As String = ""

    Private Sub jsComArcNotasDebito_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsComArcNotasDebito_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

            Dim dates As SfDateTimeEdit() = {txtEmision, txtEmisionIVA}
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
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnPrimero, btnSiguiente,
                          btnAnterior, btnUltimo, btnImprimir, btnSalir, btnDuplicar, btnAgregarMovimiento, btnEditarMovimiento,
                          btnEliminarMovimiento, btnBuscarMovimiento, btnPrimerMovimiento, btnAnteriorMovimiento,
                          btnSiguienteMovimiento, btnUltimoMovimiento)

    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)

        dtRenglones = ft.MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon,
                                             dg, lRegion, jytsistema.sUsuario, nRow, Actualiza)

        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtProveedor, btnProveedor, txtCodigo, txtNumeroSerie)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 

                NumeroDocumentoAnterior = ft.muestraCampoTexto(.Item("numndb"))
                CodigoProveedorAnterior = ft.muestraCampoTexto(.Item("codpro"))

                txtCodigo.Text = ft.muestraCampoTexto(.Item("numndb"))
                txtNumeroSerie.Text = ft.muestraCampoTexto(.Item("serie_numndb"))
                txtEmision.Value = ft.muestraCampoFecha(.Item("emision").ToString)
                txtFacturaAfectada.Text = ft.muestraCampoTexto(.Item("NUMCOM"))
                txtEmisionIVA.Value = ft.muestraCampoFecha(.Item("emisioniva").ToString)
                txtProveedor.Text = ft.muestraCampoTexto(.Item("codpro"))

                txtReferencia.Text = ft.muestraCampoTexto(.Item("refer"))
                Dim numControl As String = ft.DevuelveScalarCadena(myConn, " select num_control from jsconnumcon where numdoc = '" & .Item("numndb") & "' and " _
                                                                 & " prov_cli = '" & .Item("codpro") & "' and " _
                                                                 & " origen = 'COM' and org = 'NDB' and id_emp = '" & jytsistema.WorkID & "' ")

                txtControl.Text = IIf(numControl <> "0", numControl, "")
                txtComentario.Text = ft.muestraCampoTexto(.Item("comen"))
                txtAlmacen.Text = ft.muestraCampoTexto(.Item("almacen"))
                txtCodigoContable.Text = ft.muestraCampoTexto(.Item("codcon"))

                tslblPesoT.Text = ft.muestraCampoCantidad(ft.DevuelveScalarDoble(myConn, "select SUM(PESO) from jsprorenndb where numndb = '" & .Item("numndb") & "' and codpro = '" & .Item("codpro") & "' and id_emp = '" & jytsistema.WorkID & "' "))

                txtSubTotal.Text = ft.muestraCampoNumero(.Item("tot_net"))
                txtTotalIVA.Text = ft.muestraCampoNumero(.Item("imp_iva"))
                txtTotal.Text = ft.muestraCampoNumero(.Item("tot_ndb"))

                'Renglones
                AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)

                'Totales
                CalculaTotales()

            End With
        End With
    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroCompra As String, ByVal CodigoProveedor As String)

        strSQLMov = "select * from jsprorenndb " _
                            & " where " _
                            & " numndb  = '" & NumeroCompra & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        dtRenglones = ft.AbrirDataTable(ds, nTablaRenglones, myConn, strSQLMov)

        Dim aCampos() As String = {"item.Item.100.I.",
                                   "descrip.Descripción.300.I.",
                                   "iva.IVA.30.C.",
                                   "cantidad.Cantidad.90.D.Cantidad",
                                   "unidad.UND.45.C.",
                                   "costou.Costo Unitario.120.D.Numero",
                                   "des_art.Desc Art.50.D.Numero",
                                   "des_pro.Desc Pro.50.D.Numero",
                                   "totren.Total Renglón.120.D.Numero",
                                   "estatus.Tipo Renglón.60.I."}

        ft.IniciarTablaPlus(dg, dtRenglones, aCampos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub
    Private Sub AbrirIVA(ByVal NumeroDocumento As String, ByVal CodigoProveedor As String)

        strSQLIVA = "select * from jsproivandb " _
                            & " where " _
                            & " numndb = '" & NumeroDocumento & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by tipoiva "

        ds = DataSetRequery(ds, strSQLIVA, myConn, nTablaIVA, lblInfo)
        dtIVA = ds.Tables(nTablaIVA)

        Dim aCampos() As String = {"tipoiva..15.C.", "poriva..45.D.Numero", "baseiva..65.D.Numero", "impiva..60.D.Numero"}
        ft.IniciarTablaPlus(dgIVA, dtIVA, aCampos, False, , New Font("Consolas", 8, FontStyle.Regular), False)

        txtTotalIVA.Text = ft.muestraCampoNumero(ft.DevuelveScalarDoble(myConn, " select SUM(IMPIVA) from jsproivandb where numndb = '" & NumeroDocumentoAnterior & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' group by numndb "))



    End Sub

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = Contador(myConn, lblInfo, Gestion.iCompras, "COMNUMNDC", 9)
            NumeroDocumentoAnterior = txtCodigo.Text
            ft.habilitarObjetos(True, True, txtCodigo)
            txtReferencia.Text = ContadorNC_Financiera(myConn, "00003", "CXP")
        Else
            txtCodigo.Text = ""
            NumeroDocumentoAnterior = "NDTMP" & ft.NumeroAleatorio(1000000)
            ft.habilitarObjetos(False, True, txtCodigo)
            txtReferencia.Text = ""
        End If

        CodigoProveedorAnterior = "PRTMP" & ft.NumeroAleatorio(10000)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNumeroSerie, txtControl, txtProveedor, txtNombreProveedor, txtComentario,
                            txtAlmacen, txtCodigoContable, txtFacturaAfectada)

        Dim nAlmacen As String = ft.DevuelveScalarCadena(myConn, "SELECT codalm FROM jsmercatalm WHERE id_emp = '" & jytsistema.WorkID & "' ORDER BY codalm LIMIT 1")

        ft.iniciarTextoObjetos(Transportables.tipoDato.Fecha, txtEmision, txtEmisionIVA)

        tslblPesoT.Text = ft.FormatoCantidad(0)

        dgIVA.Columns.Clear()

        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtSubTotal, txtTotalIVA, txtTotalIVA)

        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
        AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)

    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon)
        ft.habilitarObjetos(True, True, txtCodigo, txtComentario, txtEmision, btnProveedor, txtProveedor, txtEmisionIVA,
                         txtControl, txtAlmacen, btnAlmacen,
                         txtReferencia, btnCodigoContable, txtFacturaAfectada)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        i_modo = movimiento.iConsultar

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtEmision, txtControl, txtEmisionIVA,
                txtProveedor, txtNombreProveedor, btnProveedor, txtComentario,
                txtAlmacen, btnAlmacen, txtNombreAlmacen, txtReferencia, txtCodigoContable, btnCodigoContable, txtFacturaAfectada)

        ft.habilitarObjetos(False, True, txtSubTotal, txtTotalIVA, txtTotal)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then
            If ft.DevuelveScalarEntero(myConn, " select count(*) from jsproencndb " _
                                       & " where " _
                                       & " numndb = '" & txtCodigo.Text & "' and " _
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

        ft.Ejecutar_strSQL(myConn, " delete from jsprorenndb where numndb = '" & NumeroDocumentoAnterior & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsproivandb where numndb = '" & NumeroDocumentoAnterior & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & NumeroDocumentoAnterior & "' and origen = 'NDC' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
            Imprimir()
        End If
    End Sub
    Private Sub Imprimir()

        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cNotaDebito, "Nota Débito", txtProveedor.Text, txtCodigo.Text, txtEmision.Value)
        f = Nothing

    End Sub
    Private Function Validado() As Boolean

        If FechaUltimoBloqueo(myConn, "jsproencndb") >= Convert.ToDateTime(txtEmision.Value) Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If txtCodigo.Text = "" Then
            ft.mensajeCritico("Debe indicar un Número de Nota Débito válido...")
            Return False
        End If

        If txtNombreProveedor.Text = "" Then
            ft.mensajeCritico("Debe indicar un proveedor válido...")
            Return False
        End If

        If ft.DevuelveScalarEntero(myConn, " select COUNT(*) from jsproencndb where numndb = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "'  ") > 0 AndAlso
            i_modo = movimiento.iAgregar Then
            ft.mensajeCritico("Número de Nota Débito YA existe para este proveedor ...")
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

        Return True

    End Function
    Private Sub GuardarTXT()

        Dim Codigo As String = txtCodigo.Text
        Dim Inserta As Boolean = False

        If i_modo = movimiento.iAgregar Then

            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count

        End If

        ft.Ejecutar_strSQL(myConn, " update jsprorenndb set numndb = '" & Codigo & "', codpro = '" & txtProveedor.Text & "' where codpro = '" & CodigoProveedorAnterior & "' and numndb = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " update jsproivandb set numndb = '" & Codigo & "', codpro = '" & txtProveedor.Text & "' where codpro = '" & CodigoProveedorAnterior & "' and numndb = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & NumeroDocumentoAnterior & "' and origen = 'NDC' and id_emp = '" & jytsistema.WorkID & "' ")

        Dim numCajas As Double = ft.DevuelveScalarDoble(myConn, " select sum(cantidad) from jsprorenndb where numndb = '" & Codigo & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' group by numndb ")

        InsertEditCOMPRASEncabezadoNOTADEBITO(myConn, lblInfo, Inserta, Codigo, txtNumeroSerie.Text, txtFacturaAfectada.Text, txtEmision.Value, txtEmisionIVA.Value,
                txtProveedor.Text, txtComentario.Text, txtAlmacen.Text, txtReferencia.Text, txtCodigoContable.Text, dtRenglones.Rows.Count, numCajas,
                ValorCantidad(tslblPesoT.Text), ValorNumero(txtSubTotal.Text), ValorNumero(txtTotalIVA.Text),
                ValorNumero(txtTotal.Text), jytsistema.sFechadeTrabajo, "", jytsistema.sFechadeTrabajo,
                CodigoProveedorAnterior, NumeroDocumentoAnterior)

        InsertEditCONTROLNumeroControl(myConn, Codigo, txtProveedor.Text, txtControl.Text, txtEmisionIVA.Value, "COM", "NDB")

        ActualizarMovimientos(NumeroDocumentoAnterior, Codigo, txtProveedor.Text)

        dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

        Dim row As DataRow = dt.Select(" NUMNDB = '" & Codigo & "' AND CODPRO = '" & txtProveedor.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub ActualizarMovimientos(ByVal NotaDebitoAnterior As String, ByVal NumeroNotaDebito As String, ByVal CodigoProveedor As String)

        '1.- Elimina movimientos de inventarios anteriores del NotaDebito
        EliminarMovimientosdeInventario(myConn, NotaDebitoAnterior, "NDC", lblInfo, , , CodigoProveedor)

        '2.- Actualizar Movimientos de Inventario con los del gasto
        strSQLMov = "select * from jsprorenndb " _
                            & " where " _
                            & " numndb  = '" & NumeroNotaDebito & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        For Each dtRow As DataRow In dtRenglones.Rows
            With dtRow
                If .Item("item").ToString.Substring(0, 1) <> "$" Then

                    Dim CausaDebito As String = ft.DevuelveScalarCadena(myConn, " select codigo from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 1 and id_emp = '" & jytsistema.WorkID & "' ")
                    If CausaDebito <> "0" Then
                        Dim MueveInventario As Boolean = ft.DevuelveScalarBooleano(myConn, " select inventario from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 1 and id_emp = '" & jytsistema.WorkID & "' ")
                        If MueveInventario Then _
                        InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), txtEmision.Value, "EN", NumeroNotaDebito,
                                                             .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("totren"),
                                                             .Item("totrendes"), "NDC", NumeroNotaDebito, .Item("lote"), CodigoProveedor,
                                                             0.0, 0.0, 0, 0.0, "",
                                                             txtAlmacen.Text, .Item("renglon"), jytsistema.sFechadeTrabajo)

                        Dim AjustaPrecio As Boolean = ft.DevuelveScalarBooleano(myConn, " select ajustaprecio from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 1 and id_emp = '" & jytsistema.WorkID & "' ")
                        If AjustaPrecio Then _
                            InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), txtEmision.Value, "AC", NumeroNotaDebito,
                                                                                 .Item("unidad"), 0.0, 0.0, .Item("totren"),
                                                                                 .Item("totrendes"), "NDC", .Item("numndb"), .Item("lote"), txtProveedor.Text,
                                                                                 0.0, 0.0, 0, 0.0, "",
                                                                                 txtAlmacen.Text, .Item("renglon"), jytsistema.sFechadeTrabajo)
                    Else

                        InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), txtEmision.Value, "EN", NumeroNotaDebito,
                                                         .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("totren"),
                                                         .Item("totrendes"), "NDC", NumeroNotaDebito, .Item("lote"), txtProveedor.Text,
                                                         0.0, 0.0, 0, 0.0, "",
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

        '4.- Actualizar CxP si es un Compra a crédito
        ft.Ejecutar_strSQL(myConn, " DELETE from jsprotrapag WHERE " _
                                            & " TIPOMOV = 'ND' AND  " _
                                            & " codpro = '" & CodigoProveedorAnterior & "' and " _
                                            & " NUMMOV = '" & NumeroDocumentoAnterior & "' AND " _
                                            & " ORIGEN = 'NDB' AND " _
                                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                            & " ID_EMP = '" & jytsistema.WorkID & "'  ")

        InsertEditCOMPRASCXP(myConn, lblInfo, True, CodigoProveedor, "ND", NumeroNotaDebito, txtEmision.Value, ft.FormatoHora(Now),
                            txtEmision.Value, txtReferencia.Text, "NOTA DEBITO N° : " & NumeroNotaDebito, -1 * ValorNumero(txtTotal.Text),
                            ValorNumero(txtTotalIVA.Text), "", "", "", "", "NDB", "", "", "", "", NumeroNotaDebito, "0", "", jytsistema.sFechadeTrabajo,
                            txtCodigoContable.Text, "", "", 0.0, 0.0, "", "", "", "", "", "", "0", "0", "0")

    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique comentario ... ", Transportables.tipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtProveedor, btnProveedor, txtCodigo, txtNumeroSerie)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPANDC01")) Then
            i_modo = movimiento.iEditar
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position

            If nPosicionEncab >= 0 Then
                With dt.Rows(nPosicionEncab)
                    Dim aCamposAdicionales() As String = {"numndb|'" & txtCodigo.Text & "'",
                                                          "codpro|'" & txtProveedor.Text & "'"}
                    If DocumentoBloqueado(myConn, "jsproencndb", aCamposAdicionales) Then
                        ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                    Else
                        If DocumentoPoseeRetencionIVA(myConn, lblInfo, .Item("numndb"), .Item("codpro")) Then
                            ft.mensajeCritico("Esta NOTA DEBITO posee RETENCION DE IVA. MODIFICACION NO esta permitida ...")
                        Else

                            If ft.DevuelveScalarEntero(myConn, " select COUNT(*) from jsprotrapag " _
                                                          & " where " _
                                                          & " tipomov <> 'ND' and " _
                                                          & " codpro = '" & .Item("codpro") & "' and " _
                                                          & " nummov = '" & .Item("NUMNDB") & "' and " _
                                                          & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then

                                ActivarMarco0()
                                ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtProveedor, btnProveedor, txtCodigo, txtNumeroSerie)
                            Else
                                If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) = 0 Then
                                    ft.mensajeCritico("Esta NOTA DEBITO posee movimientos asociados. MODIFICACION NO esta permitida ...")
                                Else
                                    ActivarMarco0()
                                End If
                            End If
                        End If
                    End If
                End With
            End If
        Else
            ft.mensajeCritico("Este Documento NO puede ser MODIFICADO")
        End If
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPANDC01")) Then

            nPosicionEncab = Me.BindingContext(ds, nTabla).Position

            With dt.Rows(nPosicionEncab)
                Dim aCamposAdicionales() As String = {"numndb|'" & txtCodigo.Text & "'",
                                                      "codpro|'" & txtProveedor.Text & "'"}
                If DocumentoBloqueado(myConn, "jsproencndb", aCamposAdicionales) Then
                    ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                Else
                    If DocumentoPoseeRetencionIVA(myConn, lblInfo, .Item("numndb"), .Item("codpro")) Then
                        ft.mensajeCritico("Esta NOTA DEBITO posee RETENCION DE IVA. MODIFICACION NO esta permitida ...")
                    Else
                        If ft.DevuelveScalarEntero(myConn, " select COUNT(*) from jsprotrapag " _
                                                      & " where " _
                                                      & " tipomov <> 'ND' and " _
                                                      & " codpro = '" & .Item("codpro") & "' and " _
                                                      & " nummov = '" & .Item("NUMNDB") & "' and " _
                                                      & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then

                            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

                                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numndb"))

                                For Each dtRow As DataRow In dtRenglones.Rows
                                    With dtRow
                                        Eliminados.Add(.Item("item"))
                                    End With
                                Next

                                ft.Ejecutar_strSQL(myConn, " delete from jsproencndb where NUMNDB = '" & txtCodigo.Text & "' AND CODPRO = '" & txtProveedor.Text & "' and ID_EMP = '" & jytsistema.WorkID & "'")
                                ft.Ejecutar_strSQL(myConn, " delete from jsprorenndb where numndb = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " delete from jsproivandb where numndb = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'NDC' and id_emp = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " DELETE FROM jsprotrapag where CODPRO = '" & txtProveedor.Text & "' AND TIPOMOV = 'ND' AND NUMMOV = '" & txtCodigo.Text &
                                               "' AND ORIGEN = 'NDB' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                                EliminarMovimientosdeInventario(myConn, txtCodigo.Text, "NDC", lblInfo, , , txtProveedor.Text)

                                For Each aSTR As Object In Eliminados
                                    ActualizarExistenciasPlus(myConn, aSTR)
                                Next

                                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                                dt = ds.Tables(nTabla)
                                If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                                If nPosicionEncab < 0 Then
                                    IniciarDocumento(True)
                                Else
                                    AsignaTXT(nPosicionEncab)
                                End If


                            End If
                        Else
                            ft.mensajeCritico("Esta NOTA DEBITO posee movimientos asociados. Eliminación NO está permitida...")
                        End If
                    End If
                End If
            End With
        Else
            ft.mensajeCritico("Este Documento NO puede ser ELIMINADO...")
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numndb", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Nombre", "Emisión", "Comentario"}
        Dim Anchos() As Integer = {150, 400, 100, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Notas débito Compras")
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

        If Not DocumentoPoseeRetencionIVA(myConn, lblInfo, NumeroDocumentoAnterior, CodigoProveedorAnterior) Then

            txtSubTotal.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsprorenndb", "numndb", "totren", NumeroDocumentoAnterior, 0, CodigoProveedorAnterior))

            CalculaTotalIVACompras(myConn, lblInfo, CodigoProveedorAnterior, "", "jsproivandb", "jsprorenndb", "numndb", NumeroDocumentoAnterior, "impiva", "totrendes", txtEmision.Value, "totren")

            AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)

            txtTotal.Text = ft.FormatoNumero(ValorNumero(txtSubTotal.Text) + ValorNumero(txtTotalIVA.Text))

            tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsprorenndb", "peso", "numndb", NumeroDocumentoAnterior))
        Else
            AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)
            txtTotal.Text = ft.FormatoNumero(ValorNumero(txtSubTotal.Text) + ValorNumero(txtTotalIVA.Text))
            tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsprorenndb", "peso", "numndb", NumeroDocumentoAnterior))
        End If

    End Sub

    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        If txtNombreProveedor.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "NDC", NumeroDocumentoAnterior, txtEmision.Value, txtAlmacen.Text, , , , , , , , , , , txtProveedor.Text, , CodigoProveedorAnterior)
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
            f.Editar(myConn, ds, dtRenglones, "NDC", NumeroDocumentoAnterior, txtEmision.Value, txtAlmacen.Text, , ,
                     IIf(dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position).Item("item").ToString.Substring(0, 1) = "$", True, False),
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

        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                With dtRenglones.Rows(nPosicionRenglon)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("item"))

                    Eliminados.Add(.Item("item"))

                    Dim aCamposDel() As String = {"numndb", "item", "renglon", "id_emp"}
                    Dim aStringsDel() As String = {NumeroDocumentoAnterior, .Item("item"), .Item("renglon"), jytsistema.WorkID}

                    ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & NumeroDocumentoAnterior & "' and " _
                           & " origen = 'NDC' and item = '" & .Item("item") & "' and renglon = '" & .Item("renglon") & "' and " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsprorenndb", strSQLMov, aCamposDel, aStringsDel,
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
        f.Buscar(dtRenglones, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaRenglones).Position, "Notas débito compras...")
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

    Private Sub txtCliente_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProveedor.TextChanged
        If txtProveedor.Text <> "" Then
            Dim aFld() As String = {"codpro", "id_emp"}
            Dim aStr() As String = {txtProveedor.Text, jytsistema.WorkID}
            Dim FormaDePagoCliente As String = qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "formapago")
            txtNombreProveedor.Text = qFoundAndSign(myConn, lblInfo, "jsprocatpro", aFld, aStr, "nombre")
            Dim mTotalFac As Double = ValorNumero(txtTotal.Text)
            If i_modo = movimiento.iAgregar Then mTotalFac = 0.0
        End If
    End Sub

    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dgIVA.CellFormatting
        If e.ColumnIndex = 1 Then e.Value = ft.FormatoNumero(e.Value) & "%"
    End Sub

    Private Sub btnCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProveedor.Click
        txtProveedor.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codpro codigo, nombre descripcion, RIF from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Proveedores De Compras/Gastos",
                                            txtProveedor.Text)
    End Sub


    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If txtNombreProveedor.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "NDC", NumeroDocumentoAnterior, txtEmision.Value, txtAlmacen.Text, , , True, , , , , , , , , , CodigoProveedorAnterior)
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
        txtAlmacen.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codalm codigo, desalm descripcion from jsmercatalm where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Almacenes",
                                            txtAlmacen.Text)
    End Sub

    Private Sub btnCodigoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigoContable.Click
        txtCodigoContable.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables",
                                                   txtCodigoContable.Text)
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