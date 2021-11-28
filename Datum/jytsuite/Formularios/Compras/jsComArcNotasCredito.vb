Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input

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

    'a.emision >= '" & ft.FormatoFechaMySQL(DateAdd("m", -MesesAtras.i24, jytsistema.sFechadeTrabajo)) & "' and 

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
    Private nPosicionEncab As Long, nPosicionRenglon As Long

    Private Eliminados As New ArrayList

    Private Impresa As Integer
    Private CodigoProveedorAnterior As String = ""
    Private NumeroDocumentoAnterior As String = ""

    Private Sub jsComArcNotasCredito_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsComArcNotasDebito_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()
            dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

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

        '' Proveedores
        InitiateDropDown(Of Vendor)(myConn, cmbProveedor)
        '' Monedas
        interchangeList = GetListaDeMonedasyCambios(myConn, jytsistema.sFechadeTrabajo)
        InitiateDropDownInterchangeCurrency(myConn, cmbMonedas, jytsistema.sFechadeTrabajo)
        '' Cuentas Contables
        accountList = InitiateDropDown(Of AccountBase)(myConn, cmbCC)
        '' Alamacenes 
        InitiateDropDown(Of SimpleTable)(myConn, cmbAlmacenes, Tipo.Almacenes)
    End Sub

    Private Sub AsignarTooltips()
        Dim menus As New List(Of ToolStrip) From {MenuBarra, MenuBarraRenglon}
        AsignarToolTipsMenuBarraToolStrip(menus, "Compra")
    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        dtRenglones = ft.MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon,
                                             dg, lRegion, jytsistema.sUsuario, nRow, Actualiza)
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, cmbProveedor, txtNumeroSerie)
    End Sub

    Private Sub AsignaTXT(ByVal nRow As Long)
        With dt
            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtCodigo.Text = ft.muestraCampoTexto(.Item("numncr"))
                txtNumeroSerie.Text = ft.muestraCampoTexto(.Item("serie_numncr"))
                NumeroDocumentoAnterior = .Item("numncr")
                txtEmision.Value = .Item("emision")
                txtFacturaAfectada.Text = ft.muestraCampoTexto(.Item("numcom"))
                txtEmisionIVA.Value = .Item("emisioniva")
                cmbProveedor.SelectedValue = .Item("codpro")
                CodigoProveedorAnterior = .Item("codpro")
                txtReferencia.Text = ft.muestraCampoTexto(.Item("refer"))
                Dim numControl As String = ft.DevuelveScalarCadena(myConn, " select num_control from jsconnumcon where numdoc = '" & .Item("numncr") & "' and  " _
                                                                 & " prov_cli = '" & .Item("codpro") & "' and " _
                                                                 & " origen = 'COM' and org = 'NCR' and id_emp = '" & jytsistema.WorkID & "' ")

                txtControl.Text = IIf(numControl <> "0", numControl, "")
                txtComentario.Text = ft.muestraCampoTexto(.Item("comen"))
                cmbAlmacenes.SelectedValue = .Item("almacen")
                cmbCC.SelectedValue = .Item("codcon")
                cmbMonedas.SelectedValue = .Item("Currency")

                tslblPesoT.Text = ft.muestraCampoCantidad(ft.DevuelveScalarDoble(myConn, "select SUM(PESO) from jsprorenncr where numncr = '" & .Item("numncr") & "' and codpro = '" & .Item("codpro") & "' and id_emp = '" & jytsistema.WorkID & "' "))

                txtSubTotal.Text = ft.muestraCampoNumero(.Item("tot_net"))
                txtTotalIVA.Text = ft.muestraCampoNumero(.Item("imp_iva"))
                txtTotal.Text = ft.muestraCampoNumero(.Item("tot_ncr"))

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

        dtRenglones = ft.AbrirDataTable(ds, nTablaRenglones, myConn, strSQLMov)

        Dim aCampos() As String = {"item.Item.90.I.",
                                   "descrip.Descripción.300.I.",
                                   "iva.IVA.30.C.",
                                   "cantidad.Cant.60.D.Cantidad",
                                   "unidad.UND.45.C.",
                                   "precio.Costo Unitario.80.D.Numero",
                                   "por_acepta_dev.% Acep.60.D.Numero",
                                   "totren.Costo Total.90.D.Numero",
                                   "estatus.Tipo Renglón.60.C.",
                                   "sada..100.I."}

        ft.IniciarTablaPlus(dg, dtRenglones, aCampos)

        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
            If i_modo <> movimiento.iConsultar Then ft.habilitarObjetos(True, True, txtCodigo)
        Else
            ft.habilitarObjetos(False, True, txtCodigo)
        End If

    End Sub
    Private Sub AbrirIVA(ByVal NumeroDocumento As String, ByVal CodigoProveedor As String)

        strSQLIVA = "select * from jsproivancr " _
                            & " where " _
                            & " numncr = '" & NumeroDocumento & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by tipoiva "

        dtIVA = ft.AbrirDataTable(ds, nTablaIVA, myConn, strSQLIVA)

        Dim aCampos() As String = {"tipoiva..15.C.", "poriva..45.D.Numero", "baseiva..65.D.Numero", "impiva..60.D.Numero"}
        ft.IniciarTablaPlus(dgIVA, dtIVA, aCampos, False, , New Font("Consolas", 8, FontStyle.Regular), False)

        txtTotalIVA.Text = ft.FormatoNumero(ft.DevuelveScalarDoble(myConn, " select SUM(IMPIVA) from jsproivancr where numncr = '" & NumeroDocumentoAnterior & "' and codpro = '" & CodigoProveedorAnterior & "' and id_emp = '" & jytsistema.WorkID & "' group by numncr "))



    End Sub

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)
        If Inicio Then
            txtCodigo.Text = Contador(myConn, lblInfo, Gestion.iCompras, "COMNUMNCC", 8)
            NumeroDocumentoAnterior = txtCodigo.Text
            ft.habilitarObjetos(True, True, txtCodigo)
            txtReferencia.Text = ContadorNC_Financiera(myConn, "00003", "CXP")
        Else
            txtCodigo.Text = ""
            NumeroDocumentoAnterior = "TMPNCC" & ft.NumeroAleatorio(1000000)
            ft.habilitarObjetos(False, True, txtCodigo)
            txtReferencia.Text = ""
        End If
        CodigoProveedorAnterior = "PRTMP" & ft.NumeroAleatorio(100000)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNumeroSerie, txtControl, txtComentario, txtFacturaAfectada)
        cmbProveedor.SelectedIndex = -1
        cmbAlmacenes.SelectedIndex = 0
        cmbMonedas.SelectedIndex = 0
        cmbCC.SelectedIndex = -1
        tslblPesoT.Text = ft.FormatoCantidad(0)
        dgIVA.Columns.Clear()
        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtSubTotal, txtTotalIVA, txtTotal)
        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
        AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)
    End Sub
    Private Sub ActivarMarco0()
        grpAceptarSalir.Visible = True
        ft.habilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon)
        ft.habilitarObjetos(True, True, txtCodigo, txtNumeroSerie, txtComentario, txtEmision, cmbProveedor, txtEmisionIVA,
                         txtControl, cmbAlmacenes, txtReferencia, cmbCC, cmbMonedas, txtFacturaAfectada)
        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)
    End Sub
    Private Sub DesactivarMarco0()
        i_modo = movimiento.iConsultar
        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtNumeroSerie, txtEmision, txtControl, txtEmisionIVA,
                cmbProveedor, txtComentario, cmbAlmacenes, txtReferencia, cmbCC, cmbMonedas, txtFacturaAfectada)
        ft.habilitarObjetos(False, True, txtSubTotal, txtTotalIVA, txtTotal)
        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then
            If ft.DevuelveScalarEntero(myConn, " select count(*) from jsproencncr " _
                                      & " where " _
                                      & " numncr = '" & txtCodigo.Text & "' and " _
                                      & " codpro = '" & proveedor.Codigo & "' and " _
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

        ft.Ejecutar_strSQL(myConn, " delete from jsprorenncr where numncr = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsproivancr where numncr = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'NCC' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
            Imprimir()
        End If
    End Sub
    Private Sub Imprimir()

        Dim f As New jsComRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteCompras.cNotaCredito, "Nota Crédito", proveedor.Codigo, txtCodigo.Text, txtEmision.Value)
        f = Nothing

    End Sub
    Private Function Validado() As Boolean
        If FechaUltimoBloqueo(myConn, "jsproencncr") >= txtEmision.Value Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If
        If txtCodigo.Text = "" Then
            ft.mensajeCritico("Debe indicar un Número de Nota Crédito válida...")
            Return False
        End If
        If cmbProveedor.SelectedValue = "" Then
            ft.mensajeCritico("Debe indicar un proveedor válido...")
            Return False
        End If
        If ft.DevuelveScalarEntero(myConn, " select count(*) from jsproencncr where numncr = '" & txtCodigo.Text & "' and codpro = '" & proveedor.Codigo & "' and id_emp = '" & jytsistema.WorkID & "'  ") > 0 AndAlso
            i_modo = movimiento.iAgregar Then
            ft.mensajeCritico("Número de Nota Crédito YA existe para este proveedor ...")
            Return False
        End If
        If cmbAlmacenes.SelectedValue = "" Then
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
        ft.Ejecutar_strSQL(myConn, " update jsprorenncr set numncr = '" & Codigo & "', codpro = '" & proveedor.Codigo & "' where codpro = '" & CodigoProveedorAnterior & "' and numncr = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " update jsproivancr set numncr = '" & Codigo & "', codpro = '" & proveedor.Codigo & "' where codpro = '" & CodigoProveedorAnterior & "' and numncr = '" & NumeroDocumentoAnterior & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & NumeroDocumentoAnterior & "' and origen = 'NCC' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim numCajas As Double = ft.DevuelveScalarDoble(myConn, " select SUM(CANTIDAD) from jsprorenncr where numncr = '" & Codigo & "' and codpro = '" & proveedor.Codigo & "' and id_emp = '" & jytsistema.WorkID & "' group by numncr ")

        InsertEditCOMPRASEncabezadoNOTACREDITO(myConn, lblInfo, Inserta, Codigo, txtNumeroSerie.Text, txtFacturaAfectada.Text,
                                               txtEmision.Value, txtEmisionIVA.Value, proveedor.Codigo, txtComentario.Text, "",
                                               cmbAlmacenes.SelectedValue, "", txtReferencia.Text, cmbCC.SelectedValue, "",
                                               dtRenglones.Rows.Count, numCajas, ValorCantidad(tslblPesoT.Text),
                                               ValorNumero(txtSubTotal.Text), ValorNumero(txtTotalIVA.Text), ValorNumero(txtTotal.Text),
                                               jytsistema.sFechadeTrabajo, "0", "", jytsistema.sFechadeTrabajo, Impresa,
                                               CodigoProveedorAnterior, NumeroDocumentoAnterior, cmbMonedas.SelectedValue, jytsistema.sFechadeTrabajo)

        InsertEditCONTROLNumeroControl(myConn, Codigo, proveedor.Codigo, txtControl.Text, txtEmisionIVA.Value, "COM", "NCR")

        ActualizarMovimientos(NumeroDocumentoAnterior, Codigo, proveedor.Codigo)

        dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)
        Dim row As DataRow = dt.Select(" NUMNCR = '" & Codigo & "' AND CODPRO = '" & proveedor.Codigo & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub ActualizarMovimientos(ByVal NotaCreditoAnterior As String, ByVal NumeroNotaCredito As String, ByVal CodigoProveedor As String)

        '1.- Elimina movimientos de inventarios anteriores DE LA NOTA DEBITO
        EliminarMovimientosdeInventario(myConn, NotaCreditoAnterior, "NCC", lblInfo, , , CodigoProveedor)

        '2.- Actualizar Movimientos de Inventario con los de LA NOTA CREDITO
        strSQLMov = "select * from jsprorenncr " _
                            & " where " _
                            & " numncr  = '" & NumeroNotaCredito & "' and " _
                            & " codpro = '" & CodigoProveedor & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        dtRenglones = ft.AbrirDataTable(ds, nTablaRenglones, myConn, strSQLMov)

        For Each dtRow As DataRow In dtRenglones.Rows
            With dtRow
                If .Item("item").ToString.Substring(0, 1) <> "$" Then

                    Dim CausaDebito As String = ft.DevuelveScalarCadena(myConn, " select codigo from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "' ")

                    If CausaDebito <> "0" Then
                        Dim MueveInventario As Boolean = ft.DevuelveScalarBooleano(myConn, " select inventario from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "' ")
                        If MueveInventario Then _
                        InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), txtEmision.Value, "SA", NumeroNotaCredito,
                                                             .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("totren"),
                                                             .Item("totrendes"), "NCC", NumeroNotaCredito, .Item("lote"), CodigoProveedor,
                                                             0.0, 0.0, 0, 0.0, "", cmbAlmacenes.SelectedValue,
                                                             .Item("renglon"), jytsistema.sFechadeTrabajo)

                        Dim AjustaPrecio As Boolean = ft.DevuelveScalarBooleano(myConn, " select ajustaprecio from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "' ")
                        If AjustaPrecio Then _
                            InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), txtEmision.Value, "AC",
                                                                 NumeroNotaCredito, .Item("unidad"), 0.0, 0.0, .Item("totren"),
                                                                 .Item("totrendes"), "NCC", .Item("numncr"), .Item("lote"),
                                                                 proveedor.Codigo, 0.0, 0.0, 0, 0.0, "", cmbAlmacenes.SelectedValue,
                                                                 .Item("renglon"), jytsistema.sFechadeTrabajo)
                    Else

                        InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), txtEmision.Value, "SA", NumeroNotaCredito,
                                                         .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("totren"),
                                                         .Item("totrendes"), "NCC", NumeroNotaCredito, .Item("lote"), proveedor.Codigo,
                                                         0.0, 0.0, 0, 0.0, "", cmbAlmacenes.SelectedValue, .Item("renglon"),
                                                         jytsistema.sFechadeTrabajo)

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
        ft.Ejecutar_strSQL(myConn, " DELETE from jsprotrapag WHERE " _
                                            & " TIPOMOV = 'NC' AND  " _
                                            & " codpro = '" & CodigoProveedorAnterior & "' and " _
                                            & " NUMMOV = '" & NumeroDocumentoAnterior & "' AND " _
                                            & " ORIGEN = 'NCR' AND " _
                                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                            & " ID_EMP = '" & jytsistema.WorkID & "'  ")

        InsertEditCOMPRASCXP(myConn, lblInfo, True, proveedor.Codigo, "NC", NumeroNotaCredito, txtEmision.Value, ft.FormatoHora(Now),
                            txtEmision.Value, txtReferencia.Text, "NOTA CREDITO N° : " & NumeroNotaCredito, ValorNumero(txtTotal.Text),
                            ValorNumero(txtTotalIVA.Text), "", "", "", "", "NCR", "", "", "", "", NumeroNotaCredito, "0", "", jytsistema.sFechadeTrabajo,
                            cmbCC.SelectedValue, "", "", 0.0, 0.0, "", "", "", "", "", "", "0", "1", "0",
                            cmbMonedas.SelectedValue, jytsistema.sFechadeTrabajo)

    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique comentario ... ", Transportables.tipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, cmbProveedor, txtCodigo, txtNumeroSerie)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPANCC01")) Then
            i_modo = movimiento.iEditar
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position
            If nPosicionEncab >= 0 Then
                With dt.Rows(nPosicionEncab)
                    Dim aCamposAdicionales() As String = {"numncr|'" & txtCodigo.Text & "'",
                                                          "codpro|'" & proveedor.Codigo & "'"}
                    If DocumentoBloqueado(myConn, "jsproencncr", aCamposAdicionales) Then
                        ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                    Else
                        If DocumentoPoseeRetencionIVA(myConn, lblInfo, .Item("numncr"), .Item("codpro")) Then
                            ft.mensajeCritico("Esta NOTA DE CREDITO posee RETENCION DE IVA. MODIFICACION NO esta permitida ...")
                        Else
                            If ft.DevuelveScalarEntero(myConn, " select count(*) from jsprotrapag " _
                                                          & " where " _
                                                          & " tipomov <> 'NC' and " _
                                                          & " codpro = '" & .Item("codpro") & "' and " _
                                                          & " nummov = '" & .Item("NUMNCR") & "' and " _
                                                          & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then

                                ActivarMarco0()
                                ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, cmbProveedor, txtCodigo, txtNumeroSerie)
                                If txtReferencia.Text.Length > 2 And txtReferencia.Text.Substring(0, 2) = "FL" Then ft.habilitarObjetos(True, True, txtCodigo, txtNumeroSerie)

                            Else
                                If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) = 0 Then
                                    ft.mensajeCritico("Esta NOTA CREDITO posee movimientos asociados. MODIFICACION NO esta permitida ...")
                                Else
                                    ActivarMarco0()
                                End If
                            End If
                        End If
                    End If
                End With
            End If
        Else
            ft.mensajeCritico("Este Documento no puede ser MODIFICADO...")
        End If
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If CBool(ParametroPlus(myConn, Gestion.iCompras, "COMPANCC01")) Then

            nPosicionEncab = Me.BindingContext(ds, nTabla).Position
            With dt.Rows(nPosicionEncab)
                Dim aCamposAdicionales() As String = {"numncr|'" & txtCodigo.Text & "'",
                                                      "codpro|'" & proveedor.Codigo & "'"}
                If DocumentoBloqueado(myConn, "jsproencncr", aCamposAdicionales) Then
                    ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                Else
                    If DocumentoPoseeRetencionIVA(myConn, lblInfo, .Item("numncr"), .Item("codpro")) Then
                        ft.mensajeCritico("Esta NOTA CREDITO posee RETENCION DE IVA. MODIFICACION NO esta permitida ...")
                    Else
                        If ft.DevuelveScalarEntero(myConn, " select count(*) from jsprotrapag " _
                                                      & " where " _
                                                      & " tipomov <> 'NC' and " _
                                                      & " codpro = '" & .Item("codpro") & "' and " _
                                                      & " nummov = '" & .Item("NUMNCR") & "' and " _
                                                      & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then

                            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

                                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numncr"))

                                For Each dtRow As DataRow In dtRenglones.Rows
                                    With dtRow
                                        Eliminados.Add(.Item("item"))
                                    End With
                                Next

                                ft.Ejecutar_strSQL(myConn, " delete from jsproencncr where numncr = '" & txtCodigo.Text & "' AND CODPRO = '" & proveedor.Codigo & "' and ID_EMP = '" & jytsistema.WorkID & "'")
                                ft.Ejecutar_strSQL(myConn, " delete from jsprorenncr where numncr = '" & txtCodigo.Text & "' and codpro = '" & proveedor.Codigo & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " delete from jsproivancr where numncr = '" & txtCodigo.Text & "' and codpro = '" & proveedor.Codigo & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'NCC' and id_emp = '" & jytsistema.WorkID & "' ")
                                ft.Ejecutar_strSQL(myConn, " DELETE FROM jsprotrapag where CODPRO = '" & proveedor.Codigo & "' AND TIPOMOV = 'NC' AND NUMMOV = '" & txtCodigo.Text _
                                               & "' AND ORIGEN = 'NCR' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                                EliminarMovimientosdeInventario(myConn, txtCodigo.Text, "NCC", lblInfo, , , proveedor.Codigo)

                                For Each aSTR As Object In Eliminados
                                    ActualizarExistenciasPlus(myConn, aSTR)
                                Next

                                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                                dt = ds.Tables(nTabla)
                                If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                                AsignaTXT(nPosicionEncab)

                            End If
                        Else
                            ft.mensajeCritico("Esta NOTA CREDITO posee movimientos asociados. ELIMINACION NO esta permitida ...")
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
        Dim Campos() As String = {"numncr", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Nombre", "Emisión", "Comentario"}
        Dim Anchos() As Integer = {150, 400, 100, 150}
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
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick,
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub CalculaTotales()

        If Not DocumentoPoseeRetencionIVA(myConn, lblInfo, NumeroDocumentoAnterior, CodigoProveedorAnterior) Then

            txtSubTotal.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsprorenncr", "numncr", "totren", NumeroDocumentoAnterior, 0, CodigoProveedorAnterior))

            CalculaTotalIVACompras(myConn, lblInfo, CodigoProveedorAnterior, "", "jsproivancr", "jsprorenncr", "numncr", NumeroDocumentoAnterior, "impiva", "totrendes", txtEmision.Value, "totren")

            AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)

            txtTotal.Text = ft.FormatoNumero(ValorNumero(txtSubTotal.Text) + ValorNumero(txtTotalIVA.Text))

            tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsprorenncr", "peso", "numncr", NumeroDocumentoAnterior))

        Else

            AbrirIVA(NumeroDocumentoAnterior, CodigoProveedorAnterior)
            txtTotal.Text = ft.FormatoNumero(ValorNumero(txtSubTotal.Text) + ValorNumero(txtTotalIVA.Text))
            tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsprorenncr", "peso", "numncr", NumeroDocumentoAnterior))

        End If

    End Sub

    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        If cmbProveedor.SelectedValue <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "NCC", NumeroDocumentoAnterior, txtEmision.Value, cmbAlmacenes.SelectedValue,
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
            f.Editar(myConn, ds, dtRenglones, "NCC", NumeroDocumentoAnterior, txtEmision.Value, cmbAlmacenes.SelectedValue, , ,
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
                    Dim aCamposDel() As String = {"numncr", "item", "renglon", "id_emp"}
                    Dim aStringsDel() As String = {NumeroDocumentoAnterior, .Item("item"), .Item("renglon"), jytsistema.WorkID}
                    ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & NumeroDocumentoAnterior & "' and " _
                           & " origen = 'NCC' and item = '" & .Item("item") & "' and renglon = '" & .Item("renglon") & "' and " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")
                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsprorenncr", strSQLMov, aCamposDel, aStringsDel,
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
    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dgIVA.CellFormatting
        If e.ColumnIndex = 1 Then e.Value = ft.FormatoNumero(e.Value) & "%"
    End Sub
    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If cmbProveedor.SelectedValue <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "NCC", NumeroDocumentoAnterior, txtEmision.Value, cmbAlmacenes.SelectedValue, , , True, , , , , , , , , , CodigoProveedorAnterior)
            nPosicionRenglon = f.Apuntador
            AsignarMovimientos(NumeroDocumentoAnterior, CodigoProveedorAnterior)
            CalculaTotales()
            f = Nothing
        End If
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
        EliminarMovimientosdeInventario(myConn, txtCodigo.Text, "NCC", lblInfo, , , proveedor.Codigo)

        '3.- Actualizar Movimientos de Inventario con los del gasto
        For Each dtRow As DataRow In dtRenglones.Rows
            With dtRow
                If .Item("item").ToString.Substring(0, 1) <> "$" Then

                    If CausaMueveInventarioNotasCredito(myConn, lblInfo, .Item("CAUSA")) Then

                        InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), txtEmision.Value, "SA", txtCodigo.Text,
                                                             .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("totren"),
                                                             .Item("totrendes"), "NCC", txtCodigo.Text, .Item("lote"), proveedor.Codigo,
                                                              0.0, 0.0, 0, 0.0, "",
                                                             cmbAlmacenes.SelectedValue, .Item("renglon"), jytsistema.sFechadeTrabajo)
                    End If
                End If
                ActualizarExistenciasPlus(myConn, .Item("item"))
            End With
        Next
        ft.mensajeInformativo("Proceso terminado!!! ")
    End Sub

    Private Sub cmbProveedor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbProveedor.SelectedIndexChanged
        If cmbProveedor.SelectedValue <> "" Then
            proveedor = cmbProveedor.SelectedItem
            Dim FormaDePagoCliente As String = proveedor.FormaDePago
            Dim mTotalFac As Double = ValorNumero(txtTotal.Text)
            If i_modo = movimiento.iAgregar Then mTotalFac = 0.0
        End If
    End Sub
    Private Sub txtCodigo_TextChanged(sender As Object, e As EventArgs) Handles txtCodigo.TextChanged, txtControl.TextChanged
        If txtCodigo.Text.Trim <> "" And txtControl.Text.Trim <> "" Then txtReferencia.Text = ""
    End Sub
End Class