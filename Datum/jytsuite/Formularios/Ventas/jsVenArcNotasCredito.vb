Imports MySql.Data.MySqlClient
Imports Microsoft.Win32
Imports Syncfusion.WinForms.Input
Public Class jsVenArcNotasCredito
    Private Const sModulo As String = "Notas de Crédito"
    Private Const lREgion As String = "RibbonButton86"
    Private Const nTabla As String = "tblEncabNotasCredito"
    Private Const nTablaRenglones As String = "tblRenglones_NotasCredito"
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

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long
    Private aEstatus() As String = {"", "", "Anulada"}
    Private aTarifa() As String = {"A", "B", "C", "D", "E", "F"}
    Private aImpresa() As String = {"", "I"}

    Private TarifaCliente As String = "A"
    Private CondicionDePago As Integer = CondicionPago.iCredito

    Private Eliminados As New ArrayList

    Private Impresa As Integer

    Private strSQL As String = " (select a.*, b.nombre from jsvenencncr a " _
           & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
           & " where " _
           & " a.emision >= '" & ft.FormatoFechaMySQL(DateAdd("m", -MesesAtras.i24, jytsistema.sFechadeTrabajo)) & "' and " _
           & " a.emision <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
           & " a.id_emp = '" & jytsistema.WorkID & "' order by a.numncr desc) order by numncr "

    Private Sub jsVenArcNotasCredito_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsVenArcNotasCredito_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            AsignarTooltips()
            Dim dates As SfDateTimeEdit() = {txtEmision}
            SetSizeDateObjects(dates)

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva Nota de Crédito")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> Nota de Crédito")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> Nota de Crédito")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> Nota de Crédito")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primer</B> Nota de Crédito")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a Nota de Crédito <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a Nota de Crédito <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>último Nota de Crédito</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> Nota de Crédito")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnDuplicar, "<B>Duplicar</B> este Nota de Crédito")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en Nota de Crédito")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón en Nota de Crédito")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón en Nota de Crédito")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en Nota de Crédito")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón en Nota de Crédito")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón en Nota de Crédito")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> en Nota de Crédito")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la Nota de Crédito")
        C1SuperTooltip1.SetToolTip(btnTraerCompras, "Trae renglones de <B>FACTURAS</B> para la Nota de Crédito")


    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)

        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)

        If c >= 0 And c < dg.RowCount Then
            Me.BindingContext(ds, nTablaRenglones).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, c, dtRenglones.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
            txtFactura.Text = dtRenglones.Rows(0).Item("numfac")
        End If

        ft.ActivarMenuBarra(myConn, ds, dtRenglones, lREgion, MenuBarraRenglon, jytsistema.sUsuario)
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtCliente, btnCliente)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtCodigo.Text = ft.MuestraCampoTexto(.Item("numncr"))
                txtEmision.Value = .Item("emision")

                Dim numControl As String = ft.DevuelveScalarCadena(myConn, " select num_control from jsconnumcon where " _
                                                                 & " numdoc = '" & .Item("numncr") & "' and  origen = 'FAC' and " _
                                                                 & " org = 'NCR' and id_emp = '" & jytsistema.WorkID & "' ")

                txtControl.Text = IIf(numControl <> "0", numControl, "")
                txtFactura.Text = ft.MuestraCampoTexto(.Item("numfac"))
                txtEstatus.Text = aEstatus(.Item("estatus"))
                txtImpresion.Text = aImpresa(.Item("impresa"))
                txtCliente.Text = ft.MuestraCampoTexto(.Item("codcli"))
                txtComentario.Text = ft.MuestraCampoTexto(.Item("comen"))
                txtAsesor.Text = ft.MuestraCampoTexto(.Item("codven"))
                txtTransporte.Text = ft.MuestraCampoTexto(.Item("transporte"))
                txtAlmacen.Text = ft.MuestraCampoTexto(.Item("almacen"))
                ft.RellenaCombo(aTarifa, cmbTarifa, ft.InArray(aTarifa, .Item("tarifa")))
                txtReferencia.Text = ft.MuestraCampoTexto(.Item("refer"))
                txtCodigoContable.Text = ft.MuestraCampoTexto(.Item("codcon"))

                tslblPesoT.Text = ft.FormatoCantidad(.Item("kilos"))

                txtSubTotal.Text = ft.FormatoNumero(.Item("tot_net"))
                txtTotalIVA.Text = ft.FormatoNumero(.Item("imp_iva"))
                txtTotal.Text = ft.FormatoNumero(.Item("tot_ncr"))

                Impresa = .Item("impresa")

                'Renglones
                AsignarMovimientos(.Item("numncr"))

                'Totales
                CalculaTotales()

            End With
        End With
    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroNotaCredito As String)
        strSQLMov = "select a.*, if(b.descrip is null, '', b.descrip) nomCausa from jsvenrenncr a " _
            & " left join jsvencaudcr b on (a.causa = b.codigo and a.id_emp = b.id_emp and b.credito_debito = 0 ) " _
            & " where " _
            & " " _
            & " a.numncr  = '" & NumeroNotaCredito & "' and " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"item.Item.90.I.", _
                                   "descrip.Descripción.300.I.", _
                                   "iva.IVA.30.C.", _
                                   "cantidad.Cantidad.90.D.Cantidad", _
                                   "unidad.UND.35.C.", _
                                   "precio.Precio Unitario.110.D.Numero", _
                                   "por_acepta_dev.% Devoluc.65.D.Numero", _
                                   "totren.Precio Total.110.D.Numero", _
                                   "estatus.Tipo Renglon.70.C.", _
                                   "causa..50.C.", _
                                   "nomcausa.Causa devolución.150.I."}


        ft.IniciarTablaPlus(dg, dtRenglones, aCampos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)

        End If

    End Sub



    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "NCTMP" & ft.NumeroAleatorio(100000)
        Else
            txtCodigo.Text = ""
        End If

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtControl, txtCliente, txtNombreCliente, txtAsesor, txtNombreAsesor, txtComentario, txtTransporte, _
                            txtAlmacen, txtReferencia, txtCodigoContable, txtFactura, txtImpresion)

        Dim nTransporte As String = ft.DevuelveScalarCadena(myConn, "SELECT codtra FROM jsconctatra WHERE id_emp = '" & jytsistema.WorkID & "' ORDER BY codtra LIMIT 1")
        If nTransporte <> "0" Then txtTransporte.Text = nTransporte

        Dim nAlmacen As String = ft.DevuelveScalarCadena(myConn, "SELECT codalm FROM jsmercatalm WHERE id_emp = '" & jytsistema.WorkID & "' ORDER BY codalm LIMIT 1")
        If nAlmacen <> "0" Then txtAlmacen.Text = nAlmacen

        ft.RellenaCombo(aTarifa, cmbTarifa)
        txtEmision.Value = jytsistema.sFechadeTrabajo

        txtEstatus.Text = aEstatus(0)
        tslblPesoT.Text = ft.FormatoCantidad(0)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtSubTotal, txtTotalIVA, txtTotalIVA)

        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(txtCodigo.Text)
        CalculaTotales()

    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon)
        ft.habilitarObjetos(True, True, txtComentario, txtEmision, btnCliente, txtCliente, cmbTarifa,
                         btnAsesor, btnTransporte, btnAlmacen,
                         txtReferencia, txtFactura, btnCodigoContable)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtEmision, txtControl, txtEstatus,
                txtCliente, txtNombreCliente, btnCliente, txtComentario, txtAsesor, btnAsesor, txtNombreAsesor,
                cmbTarifa, txtTransporte, txtAlmacen, btnTransporte, btnAlmacen, txtFactura,
                txtNombreTransporte, txtNombreAlmacen, txtReferencia, txtCodigoContable, btnCodigoContable, txtImpresion)

        ft.habilitarObjetos(False, True, txtSubTotal, txtTotalIVA, txtTotal)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)
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

        ft.Ejecutar_strSQL(myconn, " delete from jsvenrenncr where numncr = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myconn, " delete from jsvenivancr where numncr = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myconn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'NCV' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            jytsistema.WorkBox = Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveImpresorFiscal, "")
            GuardarTXT()
            ValidarNumeroControlFacturaAfectada()
            Imprimir()
        End If
    End Sub
    Private Sub ValidarNumeroControlFacturaAfectada()
        If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENNCRPA23")) Then
            Dim f As New jsVenArcValidacionControlDebitoCredito
            f.NumeroDeFactura = txtFactura.Text
            f.Cargar(myConn, txtCodigo.Text)
            f.Dispose()
            f = Nothing
        End If
    End Sub

    Private Sub Imprimir()

        If DocumentoImpreso(myConn, lblInfo, "jsvenencncr", "numncr", txtCodigo.Text) Then
            Dim f As New jsVenRepParametros
            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cNotaCredito, "Nota Crédito", txtCliente.Text, txtCodigo.Text, txtEmision.Value)
            f = Nothing
        Else
            If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNCRPA08")) Then
                '1. Imprimir Nota de Entrega Fiscal
                jytsistema.WorkBox = Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveImpresorFiscal, "")
                If jytsistema.WorkBox = "" Then
                    ft.MensajeCritico("DEBE INDICAR UNA FORMA DE IMPRESION FISCAL")
                Else
                    '2. Colocar Nota de Entrega como impresa
                    Dim NumeroFacturaAfectada As String = txtFactura.Text
                    Dim NumeroSerialFacturaAfectada As String = ft.DevuelveScalarCadena(myConn, " select num_control from jsconnumcon where numdoc = '" & txtFactura.Text & "' and prov_cli = '" & txtCliente.Text & "' and org = 'FAC' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ")

                    NumeroSerialFacturaAfectada = IIf(NumeroSerialFacturaAfectada.Length > 0, Mid(NumeroSerialFacturaAfectada, 9), "")
                    Dim EmisionFacturaFactada As String = Format(ft.DevuelveScalarFecha(myConn, " select emision from jsvenencncr where numncr = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' "), sFormatoFechaFiscal)
                    Dim HoraFacturaAfectada As String = Format(Now(), "HHmm")


                    Dim nTipoImpreFiscal As Integer = TipoImpresoraFiscal(myConn, jytsistema.WorkBox)
                    Select Case nTipoImpreFiscal
                        Case 0 ' FACTURA FISCAL FORMA LIBRE
                            ' SE DIRIGE A LA IMPRESORA POR DEFECTO
                            ImprimirNotaCreditoGrafica(myConn, lblInfo, ds, txtCodigo.Text)
                        Case 1 'FACTURA FISCAL PRE-IMPRESA
                        Case 2, 5, 6, 7 'IMPRESORA FISCAL TIPO ACLAS/BIXOLON
                            If nTipoImpreFiscal = 7 Then
                                ''lrkdgjwdrgdfgdf()
                            Else
                                ImprimirNotaCreditoPP1F3(myConn, lblInfo, txtCodigo.Text, NumeroFacturaAfectada, NumeroSerialFacturaAfectada, txtNombreCliente.Text,
                                                                ft.DevuelveScalarCadena(myConn, "Select RIF from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "'"),
                                                                ft.DevuelveScalarCadena(myConn, "Select DIRFISCAL from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "'"),
                                                                ft.DevuelveScalarCadena(myConn, "Select TELEF1 from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "'"),
                                                                txtEmision.Value, txtCliente.Text, 0, txtEmision.Value, txtAsesor.Text, txtNombreAsesor.Text,
                                                                ValorNumero(txtTotal.Text), nTipoImpreFiscal)
                            End If
                        Case 3 'IMPRESORA FISCAL TIPO BEMATECH
                        Case 4 'IMPRESORA FISCAL TIPO EPSON/PNP
                            ImprimirNotaCreditoPnP(myConn, lblInfo, txtCodigo.Text, txtFactura.Text, NumeroSerialFacturaAfectada,
                                                            EmisionFacturaFactada, HoraFacturaAfectada, txtNombreCliente.Text, ft.DevuelveScalarCadena(myConn, "Select RIF from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "'"),
                                                            ft.DevuelveScalarCadena(myConn, "Select DIRFISCAL from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "'"),
                                                            txtEmision.Value, txtEmision.Value)

                    End Select
                End If

                MsgBox("SE HA ENVIADO NOTA DE CREDITO A LA IMPRESORA FISCAL...", MsgBoxStyle.Information)


            Else
                ft.MensajeCritico("Impresión de NOTA CREDITO no permitida...")
            End If
        End If

    End Sub

    Private Function Validado() As Boolean

        If FechaUltimoBloqueo(myConn, "jsvenencncr") >= txtEmision.Value Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        If txtNombreCliente.Text = "" Then
            ft.MensajeCritico("Debe indicar un cliente válido...")
            Return False
        End If

        If txtNombreAsesor.Text = "" Then
            ft.MensajeCritico("Debe indicar un nombre de Asesor válido...")
            Return False
        End If

        If txtNombreTransporte.Text = "" Then
            ft.MensajeCritico("Debe indicar un transporte válido...")
            Return False
        End If

        If txtNombreAlmacen.Text = "" Then
            ft.MensajeCritico("Debe indicar un almacén válido...")
            Return False
        End If

        If dtRenglones.Rows.Count <= 0 Then
            ft.MensajeCritico("Debe introducir por lo menos un ítem")
            Return False
        End If

        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNCRPA21")) Then
            If txtFactura.Text.Trim = "" Then
                ft.MensajeCritico("DEBE INDICAR EL NUMERO DE FACTURA/DEBITO QUE AFECTA ESTA DEVOLUCION. VERIFIQUE POR FAVOR...")
                Return False
            Else
                If ft.DevuelveScalarCadena(myConn, " SELECT NUMFAC FROM jsvenencfac where numfac = '" & txtFactura.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") = "" Then
                    If ft.DevuelveScalarCadena(myConn, " SELECT NUMNDB FROM jsvenencndb where numndb = '" & txtFactura.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") = "" Then
                        ft.mensajeCritico("EL N° DE FACTURA/DEBITO INDICADO NO EXISTE. VERIFIQUE POR FAVOR ...")
                        Return False
                    End If
                End If
            End If
        End If


        If ft.DevuelveScalarEntero(myConn, " select COUNT(*) from jsvenrenncr where causa = '' and numncr = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") > 0 Then
            ft.mensajeCritico(" DEBE INDICAR LA CAUSA DEVOLUCION EN RENGLONES. VERIFIQUE POR FAVOR...")
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
            Codigo = Contador(myConn, lblInfo, Gestion.iVentas, "VENNUMNCR", "09")

            ft.Ejecutar_strSQL(myconn, " update jsvenrenncr set numncr = '" & Codigo & "' where numncr = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myconn, " update jsvenivancr set numncr = '" & Codigo & "' where numncr = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            ft.Ejecutar_strSQL(myconn, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & txtCodigo.Text & "' and origen = 'NCV' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        InsertEditVENTASEncabezadoNotaCredito(myConn, lblInfo, Inserta, Codigo, txtFactura.Text, txtEmision.Value, txtEmision.Value,
                                               txtCliente.Text, txtComentario.Text, txtAsesor.Text, txtAlmacen.Text, txtTransporte.Text,
                                               txtReferencia.Text, txtCodigoContable.Text, cmbTarifa.Text, ValorEntero(dtRenglones.Rows.Count),
                                               0.0, ValorCantidad(tslblPesoT.Text), ValorNumero(txtSubTotal.Text), ValorNumero(txtTotalIVA.Text),
                                               0.0, ValorNumero(txtTotal.Text), txtEmision.Value, 0, 1, "", "", "", "",
                                               "", "NCR", IIf(i_modo = movimiento.iAgregar, 1, ft.InArray(aEstatus, txtEstatus.Text)),
                                               "", jytsistema.sFechadeTrabajo, Impresa, "", "")

        ActualizarMovimientos(Codigo)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" NUMNCR = '" & Codigo & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub
    Private Sub ActualizarMovimientos(ByVal NumeroNotaCredito As String)


        '1.- Elimina movimientos de inventarios anteriores de la Factura
        EliminarMovimientosdeInventario(myConn, NumeroNotaCredito, "NCV", lblInfo)

        '2.- Actualizar Movimientos de Inventario con Nota de Crédito
        strSQLMov = "select a.*, if(b.descrip is null, '', b.descrip) nomCausa from jsvenrenncr a " _
            & " left join jsvencaudcr b on (a.causa = b.codigo and a.id_emp = b.id_emp and b.credito_debito = 0 ) " _
            & " where " _
            & " " _
            & " a.numncr  = '" & NumeroNotaCredito & "' and " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        For Each dtRow As DataRow In dtRenglones.Rows
            With dtRow

                Dim CostoActual As Double = UltimoCostoAFecha(myConn, .Item("item"), txtEmision.Value) / Equivalencia(myConn, .Item("item"), .Item("unidad"))

                Dim CausaCredito As String = ft.DevuelveScalarCadena(myConn, " select codigo from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "' ")
                Dim DevBuenEstado As Boolean = ft.DevuelveScalarBooleano(myConn, " select estado from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "' ")

                If CausaCredito <> "0" Then

                    Dim MueveInventario As Boolean = ft.DevuelveScalarBooleano(myConn, " select inventario from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "' ")

                    If MueveInventario Then _
                        InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), txtEmision.Value, "EN", NumeroNotaCredito,
                                                                             .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("cantidad") * CostoActual,
                                                                             .Item("cantidad") * CostoActual, "NCV", NumeroNotaCredito, .Item("lote"), txtCliente.Text,
                                                                             .Item("totren"), .Item("totrendes"), 0, 0.0, txtAsesor.Text,
                                                                             IIf(DevBuenEstado, txtAlmacen.Text, "00002"), .Item("renglon"), jytsistema.sFechadeTrabajo)

                    Dim AjustaPrecio As Boolean = ft.DevuelveScalarBooleano(myConn, " select ajustaprecio from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 0 and id_emp = '" & jytsistema.WorkID & "' ")
                    If AjustaPrecio Then _
                        InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), txtEmision.Value, "AP", NumeroNotaCredito,
                                                                             .Item("unidad"), 0.0, 0.0, 0.0,
                                                                              0.0, "NCV", .Item("numfac"), .Item("lote"), txtCliente.Text,
                                                                             .Item("totren"), .Item("totrendes"), 0, 0.0, txtAsesor.Text,
                                                                             txtAlmacen.Text, .Item("renglon"), jytsistema.sFechadeTrabajo)
                Else
                    InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), txtEmision.Value, "EN", NumeroNotaCredito,
                                                                         .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("cantidad") * CostoActual,
                                                                         .Item("cantidad") * CostoActual, "NCV", NumeroNotaCredito, .Item("lote"), txtCliente.Text,
                                                                         .Item("totren"), .Item("totrendes"), 0, 0.0, txtAsesor.Text,
                                                                         IIf(DevBuenEstado, txtAlmacen.Text, "00002"), .Item("renglon"), jytsistema.sFechadeTrabajo)
                End If

                ActualizarExistenciasPlus(myConn, .Item("item"))
            End With
        Next

        '3.- Actualizar existencias de renglones eliminados
        For Each aSTR As Object In Eliminados
            ActualizarExistenciasPlus(myConn, aSTR)
        Next

        '4.- Actualizar CxC 
        ft.Ejecutar_strSQL(myconn, " DELETE from jsventracob WHERE " _
                                            & " TIPOMOV = 'NC' AND  " _
                                            & " NUMMOV = '" & NumeroNotaCredito & "' AND " _
                                            & " ORIGEN = 'NCR' AND " _
                                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                            & " ID_EMP = '" & jytsistema.WorkID & "'  ")

        InsertEditVENTASCXC(myConn, lblInfo, True, txtCliente.Text, "NC", NumeroNotaCredito, txtEmision.Value, ft.FormatoHora(Now),
                            txtEmision.Value, txtReferencia.Text, "NOTA CREDITO N° : " & NumeroNotaCredito, -1 * ValorNumero(txtTotal.Text),
                            ValorNumero(txtTotalIVA.Text), "", "", "", "", "", "NCR", NumeroNotaCredito, "0", "", jytsistema.sFechadeTrabajo,
                            txtCodigoContable.Text, "", "", 0.0, 0.0, "", "", "", "", txtAsesor.Text, txtAsesor.Text, "0", "1", "")



    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique comentario ... ", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtCliente, btnCliente)

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        Dim aCamposAdicionales() As String = {"numncr|'" & txtCodigo.Text & "'", _
                                              "codcli|'" & txtCliente.Text & "'"}
        If DocumentoBloqueado(myConn, "jsvenencncr", aCamposAdicionales) Then
            ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
        Else
            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENNCRPA01")) Then
                If dt.Rows(nPosicionEncab).Item("RELNCR") = "" Then
                    If dt.Rows(nPosicionEncab).Item("RELFACTURAS") = "" Then
                        i_modo = movimiento.iEditar
                        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
                        ActivarMarco0()
                        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtCliente, btnCliente)
                    Else
                        ft.mensajeCritico("Esta NOTA CREDITO pertenece a relación de facturas N° " + dt.Rows(nPosicionEncab).Item("RELFACTURAS") + vbLf _
                                      + "por lo tanto su MODIFICACION no está permitida")
                    End If
                Else
                    ft.mensajeCritico("Esta NOTA CREDITO pertenece a relación de Notas Crédito N° " + dt.Rows(nPosicionEncab).Item("RELNCR") + vbLf _
                                      + "por lo tanto su MODIFICACION no está permitida")
                End If
            Else
                ft.mensajeCritico("Edición de Notas de Crédito NO está permitida...")
            End If
        End If
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Dim aCamposAdicionales() As String = {"numncr|'" & txtCodigo.Text & "'", _
                                             "codcli|'" & txtCliente.Text & "'"}
        If DocumentoBloqueado(myConn, "jsvenencncr", aCamposAdicionales) Then
            ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
        Else
            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENNCRPA01")) Then
                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                nPosicionEncab = Me.BindingContext(ds, nTabla).Position

                If dt.Rows(nPosicionEncab).Item("RELNCR") = "" Then
                    If dt.Rows(nPosicionEncab).Item("RELFACTURAS") = "" Then
                        sRespuesta = MsgBox(" ¿ Está seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")
                        If sRespuesta = MsgBoxResult.Yes Then

                            InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numncr"))

                            For Each dtRow As DataRow In dtRenglones.Rows
                                With dtRow
                                    Eliminados.Add(.Item("item"))
                                End With
                            Next

                            ft.Ejecutar_strSQL(myConn, " delete from jsvenencncr where numncr = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "'")
                            ft.Ejecutar_strSQL(myConn, " delete from jsvenrenncr where numncr = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                            ft.Ejecutar_strSQL(myConn, " delete from jsvenivancr where numncr = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                            ft.Ejecutar_strSQL(myConn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'NCV' and id_emp = '" & jytsistema.WorkID & "' ")
                            ft.Ejecutar_strSQL(myConn, " DELETE FROM  jsventracob where nummov = '" & txtCodigo.Text & "' AND ORIGEN = 'NCR' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "'")
                            EliminarMovimientosdeInventario(myConn, txtCodigo.Text, "NCV", lblInfo)

                            For Each aSTR As Object In Eliminados
                                ActualizarExistenciasPlus(myConn, aSTR)
                            Next

                            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                            dt = ds.Tables(nTabla)
                            If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                            AsignaTXT(nPosicionEncab)

                        End If
                    Else
                        ft.mensajeCritico("Esta NOTA CREDITO pertenece a relación de facturas N° " + dt.Rows(nPosicionEncab).Item("RELFACTURAS") + vbLf _
                                      + "por lo tanto su eliminación no está permitida")
                    End If
                Else
                    ft.mensajeCritico("Esta NOTA CREDITO pertenece a relación de Notas Crédito N° " + dt.Rows(nPosicionEncab).Item("RELNCR") + vbLf _
                                      + "por lo tanto su eliminación no está permitida")
                End If
            Else
                ft.mensajeCritico("Eliminación de NOTAS CREDITO NO está permitida...")
            End If
        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numNCR", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Nombre", "Emisión", "Comentario"}
        Dim Anchos() As Integer = {150, 400, 100, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Notas de crédito ventas")
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
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub CalculaTotales()

        txtSubTotal.Text = ft.FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrenncr", "numncr", "totren", txtCodigo.Text, 0))
        CalculaTotalIVAVentas(myConn, lblInfo, "", "jsvenivancr", "jsvenrenncr", "numncr", txtCodigo.Text, "impiva", "totrendes", txtEmision.Value, "totren")

        Dim ivaTable As New SimpleTableProperties("jsvenivancr", "numncr", txtCodigo.Text)
        MostrarIVAAlbaran(myConn, ivaTable, dgIVA, txtTotalIVA)

        txtTotal.Text = ft.FormatoNumero(ValorNumero(txtSubTotal.Text) + ValorNumero(txtTotalIVA.Text))

        tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsvenrenncr", "peso", "numncr", txtCodigo.Text))

        ''MostrarTotalesCambioMoneda(myConn, txtEmision, txtTotal,txtTotalCambioEmision,txtTotalActual)

    End Sub

    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        If txtNombreCliente.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "NCV", txtCodigo.Text, txtEmision.Value, txtAlmacen.Text, cmbTarifa.Text, txtCliente.Text, , , , , , , , txtFactura.Text, , txtAsesor.Text)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        If dtRenglones.Rows.Count > 0 Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Editar(myConn, ds, dtRenglones, "NCV", txtCodigo.Text, txtEmision.Value, txtAlmacen.Text, cmbTarifa.Text, txtCliente.Text,
                     IIf(dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position).Item("item").ToString.Substring(0, 1) = "$", True, False), , , , , , , txtFactura.Text, , txtAsesor.Text)
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
            sRespuesta = MsgBox(" ¿ Está seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
            If sRespuesta = MsgBoxResult.Yes Then
                With dtRenglones.Rows(nPosicionRenglon)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("item"))

                    Eliminados.Add(.Item("item"))

                    Dim aCamposDel() As String = {"numncr", "item", "renglon", "id_emp"}
                    Dim aStringsDel() As String = {txtCodigo.Text, .Item("item"), .Item("renglon"), jytsistema.WorkID}

                    ft.Ejecutar_strSQL(myconn, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and " _
                           & " origen = 'NCV' and item = '" & .Item("item") & "' and renglon = '" & .Item("renglon") & "' and " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsvenrenncr", strSQLMov, aCamposDel, aStringsDel, _
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

    Private Sub btnAsesor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesor.Click
        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNCRPA09")) Then
            txtAsesor.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, CONCAT( apellidos, ', ', nombres) descripcion from jsvencatven where tipo = '" & TipoVendedor.iFuerzaventa & "'  and estatus = 1 and id_emp = '" & jytsistema.WorkID & "'  order by 1 ", "Asesores Comerciales", _
                                               txtAsesor.Text)
        Else
            ft.MensajeCritico("Escogencia de asesor comercial no permitida...")
        End If
    End Sub

    Private Sub txtCliente_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCliente.TextChanged
        If txtCliente.Text <> "" Then
            Dim aFld() As String = {"codcli", "id_emp"}
            Dim aStr() As String = {txtCliente.Text, jytsistema.WorkID}
            Dim FormaDePagoCliente As String = qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "formapago")
            txtNombreCliente.Text = qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "nombre")
            If i_modo = movimiento.iAgregar Then

                If qFound(myConn, lblInfo, "jsvencatcli", aFld, aStr) Then
                    cmbTarifa.SelectedIndex = ft.InArray(aTarifa, qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "tarifa"))
                    Dim cAsesor As String = ft.DevuelveScalarCadena(myConn, "SELECT b.codven " _
                                                            & " FROM jsvenrenrut a  " _
                                                            & " LEFT JOIN jsvenencrut b ON (a.codrut = b.codrut AND a.tipo = b.tipo AND a.id_emp = b.id_emp) " _
                                                            & " WHERE " _
                                                            & " a.tipo = '0' AND " _
                                                            & " a.cliente = '" & txtCliente.Text & "' AND " _
                                                            & " a.id_emp = '" & jytsistema.WorkID & "' ")

                    txtAsesor.Text = cAsesor

                    If qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "transporte") <> "" Then txtTransporte.Text = qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "transporte")
                End If

            End If
        End If
    End Sub

    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dgIVA.CellFormatting
        If e.ColumnIndex = 1 Then e.Value = ft.FormatoNumero(e.Value) & "%"
    End Sub

    Private Sub txtAsesor_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAsesor.TextChanged
        Dim aFld() As String = {"codven", "tipo", "id_emp"}
        Dim aStr() As String = {txtAsesor.Text, "0", jytsistema.WorkID}
        txtNombreAsesor.Text = qFoundAndSign(myConn, lblInfo, "jsvencatven", aFld, aStr, "apellidos") & ", " _
                & qFoundAndSign(myConn, lblInfo, "jsvencatven", aFld, aStr, "nombres")
    End Sub

    Private Sub btnCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCliente.Click
        txtCliente.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcli codigo, nombre descripcion, disponible, elt(estatus+1, 'Activo', 'Bloqueado', 'Inactivo', 'Desincorporado') estatus from jsvencatcli where estatus < 3 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Clientes", _
                                            txtCliente.Text)
    End Sub

    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If txtNombreCliente.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "NCV", txtCodigo.Text, txtEmision.Value, txtAlmacen.Text,
                      cmbTarifa.Text, txtCliente.Text, True, , , , , , , , , txtAsesor.Text)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Private Sub txtTransporte_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTransporte.TextChanged
        txtNombreTransporte.Text = ft.DevuelveScalarCadena(myConn, " select nomtra from jsconctatra where codtra = '" & txtTransporte.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub txtAlmacen_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlmacen.TextChanged
        txtNombreAlmacen.Text = ft.DevuelveScalarCadena(myConn, " select desalm from jsmercatalm where codalm = '" & txtAlmacen.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub btnTransporte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransporte.Click
        txtTransporte.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codtra codigo, nomtra descripcion from jsconctatra where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Transportes", _
                                               txtTransporte.Text)
    End Sub

    Private Sub btnAlmacen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlmacen.Click
        txtAlmacen.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codalm codigo, desalm descripcion from jsmercatalm where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Almacenes", _
                                            txtAlmacen.Text)
    End Sub

    Private Sub btnCodigoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigoContable.Click
        txtCodigoContable.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables", _
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

    Private Sub btnCortar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCortar.Click
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
            With dtRenglones.Rows(nPosicionRenglon)
                Dim f As New jsGenCambioPrecioEnAlbaranes
                f.NuevoPrecio = .Item("precio")
                Dim PrecioAnterior As Double = f.NuevoPrecio
                f.Cambiar(myConn, ds, dtRenglones, txtCodigo.Text, txtCliente.Text, .Item("renglon"), .Item("item"), .Item("unidad"), .Item("cantidad"), .Item("precio"))
                If f.NuevoPrecio <> PrecioAnterior Then

                    ft.Ejecutar_strSQL(myconn, " update jsvenrenncr " _
                                   & " set precio = " & f.NuevoPrecio & ", " _
                                   & " totren = " & f.NuevoPrecio * .Item("cantidad") * .Item("por_acepta_dev") / 100 & ", " _
                                   & " totrendes = " & f.NuevoPrecio * .Item("cantidad") * (.Item("por_acepta_dev") / 100) & " " _
                                   & " where " _
                                   & " numnncr = '" & txtCodigo.Text & "' and " _
                                   & " renglon = '" & .Item("renglon") & "' and " _
                                   & " item = '" & .Item("item") & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' ")

                    ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
                    AsignaMov(nPosicionRenglon, True)
                    CalculaTotales()

                End If

                f = Nothing
            End With
        End If

    End Sub

    Private Sub btnTraerCompras_Click(sender As System.Object, e As System.EventArgs) Handles btnTraerCompras.Click
        If txtCodigo.Text <> "" Then
            If txtCliente.Text <> "" Then

                Dim f As New jsGenListadoSeleccion
                Dim aNombres() As String = {"", "Factura N°", "Emisión", "Monto"}
                Dim aCampos() As String = {"sel", "numfac", "emision", "tot_fac"}
                Dim aAnchos() As Integer = {20, 120, 120, 200}
                Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Right}
                Dim aFormato() As String = {"", "", sFormatoFecha, sFormatoNumero}
                Dim aFields() As String = {"sel.entero.1.0", "numfac.cadena.20.0", "emision.fecha.0.0", "tot_fac.doble.19.2"}

                Dim str As String = "  select 0 sel, numfac, emision, tot_fac from jsvenencfac where " _
                        & " codcli = '" & txtCliente.Text & "' AND " _
                        & " ESTATUS <= 1 AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND" _
                        & " ID_EMP = '" & jytsistema.WorkID & "' order by numFAC desc "

                f.Cargar(myConn, ds, "FACTURAS NO ANULADAS", str, _
                    aFields, aNombres, aCampos, aAnchos, aAlineacion, aFormato)

                If f.Seleccion.Length > 0 Then
                    Dim cod As String
                    For Each cod In f.Seleccion
                        Dim nTablaRenglonesOrden As String = "tblRenglonesOrden" & ft.NumeroAleatorio(100000)
                        Dim dtRenglonesOrden As New DataTable
                        ds = DataSetRequery(ds, " select * from jsvenrenfac where numfac = '" & cod & "' and id_emp = '" & jytsistema.WorkID & "' order by renglon ", myConn, nTablaRenglonesOrden, lblInfo)
                        dtRenglonesOrden = ds.Tables(nTablaRenglonesOrden)

                        For Each rRow As DataRow In dtRenglonesOrden.Rows
                            With rRow
                                Dim pesoTransito As Double = .Item("peso")

                                Dim numRenglon As String = ft.autoCodigo(myConn, "renglon", "jsvenrenncr", "numncr.id_emp", txtCodigo.Text + "." + jytsistema.WorkID, 5)

                                InsertEditVENTASRenglonNotaDeCredito(myConn, lblInfo, True, txtCodigo.Text, numRenglon, _
                                     .Item("item"), .Item("descrip"), .Item("iva"), "", .Item("unidad"), 0.0, .Item("cantidad"), pesoTransito, "", _
                                     .Item("estatus"), .Item("precio"), 0.0, 0.0, 0.0, 100, .Item("totren"), .Item("totrendes"), .Item("numfac"), "", _
                                      0, "", "1")

                            End With
                            AsignarMovimientos(txtCodigo.Text)
                            CalculaTotales()
                        Next
                        dtRenglonesOrden.Dispose()
                        dtRenglonesOrden = Nothing
                    Next

                End If
                f = Nothing
            Else
                ft.MensajeCritico("DEBE INDICAR UN CLIENTE VALIDO...")
            End If
        Else
            ft.MensajeCritico("DEBE INDICAR UN NUMERO DE FACTURA VALIDO...")
        End If
    End Sub
End Class