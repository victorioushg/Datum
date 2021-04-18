Imports MySql.Data.MySqlClient
Imports Microsoft.Win32
Public Class jsVenArcNotasDebito
    Private Const sModulo As String = "Notas Débito"
    Private Const nTabla As String = "tblEncabND"
    Private Const lRegion As String = "RibbonButton87"
    Private Const nTablaRenglones As String = "tblRenglones_ND"
    Private Const nTablaIVA As String = "tblIVA"
    Private Const nTablaDescuentos As String = "tblDescuentos"

    Private strSQL As String = " (select a.*, b.nombre from jsvenencndb a " _
            & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
            & " where " _
            & " a.emision >= '" & FormatoFechaMySQL(DateAdd("m", -MesesAtras.i24, jytsistema.sFechadeTrabajo)) & "' and " _
            & " a.emision <= '" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.numndb desc) order by numndb "

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
    Private aTarifa() As String = {"A", "B", "C", "D", "E", "F"}
    Private aImpresa() As String = {"", "I"}

    Private TarifaCliente As String = "A"

    Private Eliminados As New ArrayList

    Private Impresa As Integer
    Private Sub jsVenArcNotasDebito_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva Nota Débito")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> Nota Débito")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> Nota Débito")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> Nota Débito")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primer</B> Nota Débito")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a Nota Débito <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a Nota Débito <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>último Nota Débito</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> Nota Débito")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnDuplicar, "<B>Duplicar</B> este Nota Débito")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en Nota Débito")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón en Nota Débito")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón en Nota Débito")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en Nota Débito")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón en Nota Débito")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón en Nota Débito")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> en Nota Débito")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la Nota Débito")

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
        HabilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtCliente, btnCliente)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtCodigo.Text = MuestraCampoTexto(.Item("numndb"))
                txtEmision.Text = FormatoFecha(CDate(.Item("emision").ToString))

                txtFactura.Text = MuestraCampoTexto(.Item("numfac"))
                Dim numControl As String = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select num_control from jsconnumcon where numdoc = '" & .Item("numndb") & "' and  origen = 'FAC' and org = 'NDB' and id_emp = '" & jytsistema.WorkID & "' ")
                txtControl.Text = IIf(numControl <> "0", numControl, "")

                txtEstatus.Text = IIf(.Item("estatus") = 0, "", "ANULADA")
                txtImpresion.Text = aImpresa(.Item("impresa"))
                txtCliente.Text = MuestraCampoTexto(.Item("codcli"))
                txtComentario.Text = MuestraCampoTexto(.Item("comen"))
                txtAsesor.Text = MuestraCampoTexto(.Item("codven"))
                txtTransporte.Text = MuestraCampoTexto(.Item("transporte"))
                txtAlmacen.Text = MuestraCampoTexto(.Item("almacen"))
                RellenaCombo(aTarifa, cmbTarifa, InArray(aTarifa, .Item("tarifa")) - 1)
                txtReferencia.Text = MuestraCampoTexto(.Item("refer"))
                txtCodigoContable.Text = MuestraCampoTexto(.Item("codcon"))

                tslblPesoT.Text = FormatoCantidad(.Item("kilos"))

                txtSubTotal.Text = FormatoNumero(.Item("tot_net"))
                txtTotalIVA.Text = FormatoNumero(.Item("imp_iva"))
                txtTotal.Text = FormatoNumero(.Item("tot_ndb"))

                Impresa = .Item("impresa")

                'Renglones
                AsignarMovimientos(.Item("numndb"))

                'Totales
                CalculaTotales()

            End With
        End With
    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroNotaDebito As String)

        strSQLMov = "select a.*, if(b.descrip is null, '', b.descrip) nomCausa from jsvenrenndb a " _
            & " left join jsvencaudcr b on (a.causa = b.codigo and a.id_emp = b.id_emp and  b.credito_debito = 1) " _
            & " where " _
            & "  " _
            & " a.numndb  = '" & NumeroNotaDebito & "' and " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"item", "descrip", "iva", "cantidad", "unidad", "precio", "des_art", "des_cli", "des_ofe", "totren", "causa", "nomCausa"}
        Dim aNombres() As String = {"Item", "Descripción", "IVA", "Cant.", "UND", "Precio Unitario", "Desc. Art.", "Desc. Cli.", "Desc. Ofe.", "Precio Total", "Causa", ""}
        Dim aAnchos() As Long = {70, 350, 30, 60, 45, 70, 50, 50, 50, 100, 60, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", sFormatoCantidad, "", sFormatoNumero, sFormatoNumero, sFormatoNumero, _
                                     sFormatoNumero, sFormatoNumero, "", ""}
        IniciarTabla(dg, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub
    Private Sub AbrirIVA(ByVal NumeroDocumento As String)

        strSQLIVA = "select * from jsvenivandb " _
                            & " where " _
                            & " numndb  = '" & NumeroDocumento & "' and " _
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

        txtTotalIVA.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(impiva) from jsvenivandb where numndb = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' group by numndb ")))

    End Sub


    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "NDTMP" & NumeroAleatorio(100000)
        Else
            txtCodigo.Text = ""
        End If

        IniciarTextoObjetos(FormatoItemListView.iCadena, txtControl, txtCliente, txtNombreCliente, txtAsesor, txtNombreAsesor, txtComentario, txtTransporte, _
                            txtAlmacen, txtReferencia, txtCodigoContable, txtEstatus, txtFactura, txtImpresion)

        Dim nTransporte As String = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, "SELECT codtra FROM jsconctatra WHERE id_emp = '" & jytsistema.WorkID & "' ORDER BY codtra LIMIT 1"))
        If nTransporte <> "0" Then txtTransporte.Text = nTransporte

        Dim nAlmacen As String = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, "SELECT codalm FROM jsmercatalm WHERE id_emp = '" & jytsistema.WorkID & "' ORDER BY codalm LIMIT 1"))
        If nAlmacen <> "0" Then txtAlmacen.Text = nAlmacen

        RellenaCombo(aTarifa, cmbTarifa)
        txtEmision.Text = FormatoFecha(sFechadeTrabajo)

        tslblPesoT.Text = FormatoCantidad(0)

        IniciarTextoObjetos(FormatoItemListView.iNumero, txtSubTotal, txtTotalIVA, txtTotalIVA)

        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", 0, 0)
        AsignarMovimientos(txtCodigo.Text)
        AbrirIVA(txtCodigo.Text)

    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        HabilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon)
        HabilitarObjetos(True, True, txtComentario, btnEmision, btnCliente, txtCliente, cmbTarifa, _
                         btnAsesor, txtAsesor, txtTransporte, btnTransporte, txtAlmacen, btnAlmacen, _
                         txtReferencia, txtFactura, btnCodigoContable)

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        HabilitarObjetos(False, True, txtCodigo, txtEmision, btnEmision, txtControl, txtEstatus, _
                txtCliente, txtNombreCliente, btnCliente, txtComentario, txtAsesor, btnAsesor, txtNombreAsesor, _
                cmbTarifa, txtTransporte, txtAlmacen, btnTransporte, btnAlmacen, txtFactura, _
                txtNombreTransporte, txtNombreAlmacen, txtReferencia, txtCodigoContable, btnCodigoContable, txtImpresion)

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

        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrenndb where numndb = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenivandb where numndb = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'NDV' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
            Imprimir()

        End If
    End Sub


    Private Sub Imprimir()

        If DocumentoImpreso(myConn, lblInfo, "jsvenencndb", "numndb", txtCodigo.Text) Then
            Dim f As New jsVenRepParametros
            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cNotaDebito, "Nota Débito", txtCliente.Text, txtCodigo.Text, CDate(txtEmision.Text))
            f = Nothing
        Else
            If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNDBPA10")) Then
                '1. Imprimir Nota de Entrega Fiscal
                jytsistema.WorkBox = Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveImpresorFiscal, "")
                If jytsistema.WorkBox = "" Then
                    MensajeCritico(lblInfo, "DEBE INDICAR UNA FORMA DE IMPRESION FISCAL")
                Else
                    '2. 
                    Dim nTipoImpreFiscal As Integer = TipoImpresoraFiscal(myConn, lblInfo, jytsistema.WorkBox)
                    Select Case nTipoImpreFiscal
                        Case 0 ' FACTURA FISCAL FORMA LIBRE
                            ' SE DIRIGE A LA IMPRESORA POR DEFECTO
                            ImprimirNotaDebitoGrafica(myConn, lblInfo, ds, txtCodigo.Text)
                        Case 1 'FACTURA FISCAL PRE-IMPRESA
                        Case 2, 5, 6  'IMPRESORA FISCAL TIPO ACLAS/BIXOLON
                            ImprimirNotaDebitoFiscalPP1F3(myConn, lblInfo, txtCodigo.Text, NumeroSERIALImpresoraFISCAL(myConn, lblInfo, jytsistema.WorkBox), _
                                txtNombreCliente.Text, txtCliente.Text, EjecutarSTRSQL_Scalar(myConn, lblInfo, "Select RIF from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "'"), _
                                EjecutarSTRSQL_Scalar(myConn, lblInfo, "Select DIRFISCAL from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "'"), _
                                CDate(txtEmision.Text), "0", _
                                CDate(txtEmision.Text), txtAsesor.Text, txtNombreAsesor.Text, nTipoImpreFiscal)
                        Case 3 'IMPRESORA FISCAL TIPO BEMATECH
                        Case 4 'IMPRESORA FISCAL TIPO EPSON/PNP
                            ImprimirNotaDebitoFiscalPnP(myConn, lblInfo, txtCodigo.Text, txtNombreCliente.Text, txtCliente.Text, _
                                EjecutarSTRSQL_Scalar(myConn, lblInfo, "Select RIF from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "'"), _
                                EjecutarSTRSQL_Scalar(myConn, lblInfo, "Select DIRFISCAL from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "'"), _
                                CDate(txtEmision.Text), "0", _
                                CDate(txtEmision.Text), txtAsesor.Text, txtNombreAsesor.Text)
                    End Select
                End If

                MsgBox("SE HA ENVIADO NOTA DEBITO A LA IMPRESORA FISCAL...", MsgBoxStyle.Information)


            Else
                MensajeCritico(lblInfo, "Impresión de NOTA DEBITO no permitida...")
            End If
        End If

    End Sub

    Private Function Validado() As Boolean


        If txtNombreCliente.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un cliente válido...")
            Return False
        End If

        If txtNombreAsesor.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un nombre de Asesor válido...")
            Return False
        End If

        If txtNombreTransporte.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un transporte válido...")
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

        Dim EstatusCliente As Integer = CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select estatus from jsvencatcli where codcli = '" & txtCliente.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
        If EstatusCliente = 1 Or EstatusCliente = 2 Then
            MensajeCritico(lblInfo, "Este Cliente posee estatus  " & aEstatusCliente(EstatusCliente) & ". Favor remitir a Administración")
            Return False
        End If

        If dtRenglones.Rows.Count <= 0 Then
            MensajeCritico(lblInfo, "Debe introducir por lo menos un ítem")
            Return False
        End If

        Return True

    End Function
    Private Sub GuardarTXT()

        Dim Codigo As String = txtCodigo.Text
        Dim Inserta As Boolean = False
        Dim aEstatus() As String = {"", "ANULADA"}
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count
            Codigo = Contador(myConn, lblInfo, Gestion.iVentas, "VENNUMNDB", "08")

            EjecutarSTRSQL(myConn, lblInfo, " update jsvenrenndb set numndb = '" & Codigo & "' where numndb = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            EjecutarSTRSQL(myConn, lblInfo, " update jsvenivandb set numndb = '" & Codigo & "' where numndb = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            EjecutarSTRSQL(myConn, lblInfo, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & txtCodigo.Text & "' and origen = 'NDV' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        InsertEditVENTASEncabezadoNOTADEBITO(myConn, lblInfo, Inserta, Codigo, txtFactura.Text, CDate(txtEmision.Text), txtCliente.Text, _
                                               txtComentario.Text, txtAsesor.Text, txtAlmacen.Text, txtTransporte.Text, _
                                               txtReferencia.Text, txtCodigoContable.Text, cmbTarifa.SelectedValue, _
                                               ValorEntero(dtRenglones.Rows.Count), 0.0, ValorCantidad(tslblPesoT.Text),
                                               ValorNumero(txtSubTotal.Text), ValorNumero(txtTotalIVA.Text), ValorNumero(txtTotal.Text), _
                                               CDate(txtEmision.Text), IIf(i_modo = movimiento.iAgregar, 1, InArray(aEstatus, txtEstatus.Text) - 1), _
                                               "", jytsistema.sFechadeTrabajo, Impresa, "")


        ActualizarMovimientos(Codigo)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)


        Dim row As DataRow = dt.Select(" numndb = '" & Codigo & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub ActualizarMovimientos(ByVal NumeroNotaDebito As String)


        '1.- Elimina movimientos de inventarios anteriores de la Factura
        EliminarMovimientosdeInventario(myConn, NumeroNotaDebito, "NDV", lblInfo)

        '2.- Actualizar Movimientos de Inventario con Nota Débito
        strSQLMov = "select a.*, if(b.descrip is null, '', b.descrip) nomCausa from jsvenrenndb a " _
            & " left join jsvencaudcr b on (a.causa = b.codigo and a.id_emp = b.id_emp and  b.credito_debito = 1) " _
            & " where " _
            & "  " _
            & " a.numndb  = '" & NumeroNotaDebito & "' and " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        For Each dtRow As DataRow In dtRenglones.Rows
            With dtRow
                Dim CostoActual As Double = UltimoCostoAFecha(myConn, lblInfo, .Item("item"), CDate(txtEmision.Text)) / Equivalencia(myConn, lblInfo, .Item("item"), .Item("unidad"))

                Dim CausaDebito As String = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select codigo from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 1 and id_emp = '" & jytsistema.WorkID & "' "))
                Dim DevBuenEstado As Boolean = CBool(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select estado from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 1 and id_emp = '" & jytsistema.WorkID & "' "))

                If CausaDebito <> "0" Then

                    Dim MueveInventario As Boolean = CBool(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select inventario from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 1 and id_emp = '" & jytsistema.WorkID & "' "))
                    If MueveInventario Then _
                        InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(txtEmision.Text), "SA", NumeroNotaDebito, _
                                                             .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("cantidad") * CostoActual, _
                                                             .Item("cantidad") * CostoActual, "NDV", NumeroNotaDebito, .Item("lote"), txtCliente.Text, _
                                                             .Item("totren"), .Item("totrendes"), 0, .Item("totren") - .Item("totrendes"), txtAsesor.Text, _
                                                              If(DevBuenEstado, .Item("almacen"), "00002"), .Item("renglon"), jytsistema.sFechadeTrabajo)

                    Dim AjustaPrecio As Boolean = CBool(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select ajustaprecio from jsvencaudcr where codigo = '" & .Item("causa") & "' and credito_debito = 1 and id_emp = '" & jytsistema.WorkID & "' "))
                    If AjustaPrecio Then _
                        InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(txtEmision.Text), "AP", NumeroNotaDebito, _
                                                                             .Item("unidad"), 0.0, 0.0, 0.0, _
                                                                             0.0, "NDV", .Item("numfac"), .Item("lote"), txtCliente.Text, _
                                                                             .Item("totren"), .Item("totrendes"), 0, 0.0, txtAsesor.Text, _
                                                                              If(DevBuenEstado, .Item("almacen"), "00002"), .Item("renglon"), jytsistema.sFechadeTrabajo)
                Else
                    InsertEditMERCASMovimientoInventario(myConn, lblInfo, True, .Item("item"), CDate(txtEmision.Text), "SA", NumeroNotaDebito, _
                                                         .Item("unidad"), .Item("cantidad"), .Item("peso"), .Item("cantidad") * CostoActual, _
                                                         .Item("cantidad") * CostoActual, "NDV", NumeroNotaDebito, IIf(IsDBNull(.Item("lote")), "", .Item("lote")), txtCliente.Text, _
                                                         .Item("totren"), .Item("totrendes"), 0, .Item("totren") - .Item("totrendes"), txtAsesor.Text, _
                                                          If(DevBuenEstado, .Item("almacen"), "00002"), .Item("renglon"), jytsistema.sFechadeTrabajo)
                End If

                ActualizarExistenciasPlus(myConn, .Item("item"))
            End With
        Next

        '4.- Actualizar existencias de renglones eliminados
        For Each aSTR As Object In Eliminados
            ActualizarExistenciasPlus(myConn, aSTR)
        Next

        '5.- Actualizar CxC 
        EjecutarSTRSQL(myConn, lblInfo, " DELETE from jsventracob WHERE " _
                                            & " TIPOMOV = 'ND' AND  " _
                                            & " NUMMOV = '" & NumeroNotaDebito & "' AND " _
                                            & " ORIGEN = 'NDB' AND " _
                                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                                            & " ID_EMP = '" & jytsistema.WorkID & "'  ")

        InsertEditVENTASCXC(myConn, lblInfo, True, txtCliente.Text, "ND", NumeroNotaDebito, CDate(txtEmision.Text), FormatoHora(Now), _
                            CDate(txtEmision.Text), txtReferencia.Text, "Nota Débito : " & NumeroNotaDebito, ValorNumero(txtTotal.Text), _
                            ValorNumero(txtTotalIVA.Text), "", "", "", "", "", "NDB", NumeroNotaDebito, "0", "", jytsistema.sFechadeTrabajo, _
                            txtCodigoContable.Text, "", "", 0.0, 0.0, "", "", "", "", txtAsesor.Text, txtAsesor.Text, "0", "0", "")


    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        MensajeEtiqueta(lblInfo, " Indique comentario ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
        HabilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtCliente, btnCliente)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNDBPA01")) Then
            i_modo = movimiento.iEditar
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position
            ActivarMarco0()
            HabilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtCliente, btnCliente)
        Else
            MensajeCritico(lblInfo, "Edición de Notas Débito NO está permitida...")
        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNDBPA01")) Then
            Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position

            sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")

            If sRespuesta = MsgBoxResult.Yes Then
                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numndb"))
                For Each dtRow As DataRow In dtRenglones.Rows
                    With dtRow
                        Eliminados.Add(.Item("item"))
                    End With
                Next

                EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenencndb where numndb = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "'")

                EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrenndb where numndb = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenivandb where numndb = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'NDV' and id_emp = '" & jytsistema.WorkID & "' ")
                EjecutarSTRSQL(myConn, lblInfo, " delete from jsventracob where nummov = '" & txtCodigo.Text & "' and origen = 'NDB' AND Ejercicio = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "'")

                'ELIMINIAR DE CHEQUES DEVUELTOS
                EjecutarSTRSQL(myConn, lblInfo, " delete from jsbanchedev where refer = '" & txtReferencia.Text & "' and prov_cli = '" & txtCliente.Text & "' and fechadev = '" & FormatoFechaMySQL(CDate(txtEmision.Text)) & "' AND ID_EMP = '" & jytsistema.WorkID & "'")
                'ELIMINAR MOVIMIENTO EN BANCOS DE CHEQUE DEVUELTO
                EjecutarSTRSQL(myConn, lblInfo, " delete from jsbantraban where numdoc = '" & txtReferencia.Text & "' and numorg = '" & txtReferencia.Text & "' and prov_cli = '" & txtCliente.Text & "' and fechamov = '" & FormatoFechaMySQL(CDate(txtEmision.Text)) & "' AND ID_EMP = '" & jytsistema.WorkID & "'")


                EliminarMovimientosdeInventario(myConn, txtCodigo.Text, "NDV", lblInfo)

                For Each aSTR As Object In Eliminados
                    ActualizarExistenciasPlus(myConn, aSTR)
                Next

                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                dt = ds.Tables(nTabla)
                If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                AsignaTXT(nPosicionEncab)

            End If
        Else
            MensajeCritico(lblInfo, "Eliminación de Notas Débito NO está permitida...")
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numndb", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Nombre", "Emisión", "Comentario"}
        Dim Anchos() As Long = {150, 400, 100, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Notas débito ventas...")
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

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "ItemsRenglon", "lblItemsRenglon", e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub CalculaTotales()

        txtSubTotal.Text = FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrenndb", "numndb", "totren", txtCodigo.Text, 0))
        CalculaTotalIVAVentas(myConn, lblInfo, "", "jsvenivandb", "jsvenrenndb", "numndb", txtCodigo.Text, "impiva", "totrendes", CDate(txtEmision.Text), "totren")
        AbrirIVA(txtCodigo.Text)
        txtTotal.Text = FormatoNumero(ValorNumero(txtSubTotal.Text) + ValorNumero(txtTotalIVA.Text))

        tslblPesoT.Text = FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsvenrenndb", "peso", "numndb", txtCodigo.Text))

    End Sub

    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        If txtNombreCliente.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "NDV", txtCodigo.Text, CDate(txtEmision.Text), txtAlmacen.Text, cmbTarifa.Text, txtCliente.Text, , , , , , , , , , txtAsesor.Text)
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
            f.Editar(myConn, ds, dtRenglones, "NDV", txtCodigo.Text, CDate(txtEmision.Text), txtAlmacen.Text, cmbTarifa.Text, txtCliente.Text, _
                     IIf(dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position).Item("item").ToString.Substring(1, 1) = "$", True, False), , , , , , , , , txtAsesor.Text)
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

                    Dim aCamposDel() As String = {"numndb", "item", "renglon", "id_emp"}
                    Dim aStringsDel() As String = {txtCodigo.Text, .Item("item"), .Item("renglon"), jytsistema.WorkID}

                    EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and " _
                           & " origen = 'NDV' and item = '" & .Item("item") & "' and renglon = '" & .Item("renglon") & "' and " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsvenrenndb", strSQLMov, aCamposDel, aStringsDel, _
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

    Private Sub btnEmision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmision.Click
        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNDBPA03")) Then
            txtEmision.Text = SeleccionaFecha(CDate(txtEmision.Text), Me, sender)
        End If
    End Sub

    Private Sub btnAsesor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesor.Click
        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENNDBPA11")) Then
            txtAsesor.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codven codigo, CONCAT( apellidos, ', ', nombres) descripcion from jsvencatven where tipo = '" & TipoVendedor.iFuerzaventa & "'  and estatus = 1 and id_emp = '" & jytsistema.WorkID & "'  order by 1 ", "Asesores Comerciales", _
                                               txtAsesor.Text)
        Else
            MensajeCritico(lblInfo, "Escogencia de asesor comercial no permitida...")
        End If
    End Sub

    Private Sub txtCliente_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCliente.TextChanged
        If txtCliente.Text <> "" Then
            Dim aFld() As String = {"codcli", "id_emp"}
            Dim aStr() As String = {txtCliente.Text, jytsistema.WorkID}
            Dim FormaDePagoCliente As String = qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "formapago")
            txtNombreCliente.Text = qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "nombre")

          
            Dim mTotalFac As Double = ValorNumero(txtTotal.Text)
            If i_modo = movimiento.iAgregar Then
                mTotalFac = 0.0

                If qFound(myConn, lblInfo, "jsvencatcli", aFld, aStr) Then
                    cmbTarifa.SelectedIndex = InArray(aTarifa, qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "tarifa")) - 1
                    Dim cAsesor As String = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, "SELECT IF( b.codven IS NULL, '0', b.codven) codven  " _
                                                            & " FROM jsvenrenrut a  " _
                                                            & " LEFT JOIN jsvenencrut b ON (a.codrut = b.codrut AND a.tipo = b.tipo AND a.id_emp = b.id_emp) " _
                                                            & " WHERE " _
                                                            & " a.tipo = '0' AND " _
                                                            & " a.cliente = '" & txtCliente.Text & "' AND " _
                                                            & " a.id_emp = '" & jytsistema.WorkID & "' "))

                    If cAsesor = "0" Then cAsesor = ""
                    txtAsesor.Text = cAsesor
                    If qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "transporte") <> "" Then txtTransporte.Text = qFoundAndSign(myConn, lblInfo, "jsvencatcli", aFld, aStr, "transporte")
                End If

            End If

        End If
    End Sub

    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles _
        dgIVA.CellFormatting
        If e.ColumnIndex = 1 Then e.Value = FormatoNumero(e.Value) & "%"
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
            f.Agregar(myConn, ds, dtRenglones, "NDV", txtCodigo.Text, CDate(txtEmision.Text), txtAlmacen.Text, cmbTarifa.Text, txtCliente.Text, True)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    
    Private Sub txtTransporte_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTransporte.TextChanged
        txtNombreTransporte.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select nomtra from jsconctatra where codtra = '" & txtTransporte.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub txtAlmacen_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlmacen.TextChanged
        txtNombreAlmacen.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select desalm from jsmercatalm where codalm = '" & txtAlmacen.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
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

                    EjecutarSTRSQL(myConn, lblInfo, " update jsvenrenndb " _
                                   & " set precio = " & f.NuevoPrecio & ", " _
                                   & " totren = " & f.NuevoPrecio * .Item("cantidad") & ", " _
                                   & " totrendes = " & f.NuevoPrecio * .Item("cantidad") * (1 - .Item("des_art") / 100) * (1 - .Item("des_cli") / 100) * (1 - .Item("des_ofe") / 100) & " " _
                                   & " where " _
                                   & " numndb = '" & txtCodigo.Text & "' and " _
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

End Class