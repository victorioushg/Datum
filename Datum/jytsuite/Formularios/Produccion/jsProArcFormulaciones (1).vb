Imports MySql.Data.MySqlClient
Imports Microsoft.Win32
Public Class jsProArcFormulaciones
    Private Const sModulo As String = "Formulaciones"
    Private Const lRegion As String = "RibbonButton161"
    Private Const nTabla As String = "tblEncabFormulaciones"
    Private Const nTablaRenglones As String = "tblRenglones_Formulaciones"
    Private Const nTablaRenglonesResidual As String = "tblRenglones_FormulacionesResidual"

    Private Const nTablaIVA As String = "tblIVA"
    Private Const nTablaDescuentos As String = "tblDescuentos"

    Private strSQL As String = " (select a.*, b.nomart from jsfabencfor a " _
            & " left join jsmerctainv b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
            & " where " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by a.codfor desc) order by codfor "

    Private strSQLMov As String = ""
    Private strSQLMovRes As String = ""
    Private strSQLCostosAdicionales As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtRenglones As New DataTable
    Private dtRenglonesResidual As New DataTable
    Private dtCostosAdicionales As New DataTable

    Private i_modo As Integer

    Private nPosicionEncab As Long
    Private nPosicionRenglon As Long
    Private nPosicionRenglonResidual As Long
    Private nPosicionCostosAdicionales As Long

    Private aEstatus() As String = {"Inactiva", "Activa"}

    Private Eliminados As New ArrayList

    Private Sub jsProArcFormulaciones_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva Fórmula")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> Fórmula")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> Fórmula")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> Fórmula")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primer</B> Fórmula")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a Fórmula <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a Fórmula <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>último Fórmula</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> Fórmula")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnDuplicar, "<B>Duplicar</B> este Fórmula")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en Fórmula")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón en Fórmula")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón en Fórmula")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en Fórmula")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón en Fórmula")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón en Fórmula")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> en Fórmula")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la Fórmula")
        C1SuperTooltip1.SetToolTip(btnPresupuesto, "Traer <B>presupuesto</B> a la Fórmula")
        C1SuperTooltip1.SetToolTip(btnPrepedido, "Traer <B>pre-pedido</B> a la Fórmula")
        C1SuperTooltip1.SetToolTip(btnPedido, "Traer <B>pedido</B> a la Fórmula")

        'Menu Barra Descuento 
        C1SuperTooltip1.SetToolTip(btnAgregaDescuento, "<B>Agrega </B> descuento global a Fórmula")
        C1SuperTooltip1.SetToolTip(btnEliminaDescuento, "<B>Elimina</B> descuento global de Fórmula")



    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)

        If Actualiza Then
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            dtRenglones = ds.Tables(nTablaRenglones)
        End If


        If c >= 0 And dtRenglones.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaRenglones).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", c, dtRenglones.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtRenglones, MenuBarraRenglon)
        'HabilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtMercancia, btnMercancia)

    End Sub

    Private Sub AsignaMovResidual(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)

        If Actualiza Then
            ds = DataSetRequery(ds, strSQLMovRes, myConn, nTablaRenglonesResidual, lblInfo)
            dtRenglonesResidual = ds.Tables(nTablaRenglonesResidual)
        End If


        If c >= 0 And dtRenglonesResidual.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaRenglonesResidual).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", c, dtRenglonesResidual.Rows.Count)
            dgResidual.Refresh()
            dgResidual.CurrentCell = dg(0, c)
        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtRenglonesResidual, MenuBarraRenglon)

        '    HabilitarObjetos(IIf(dtRenglonesResidual.Rows.Count > 0, False, True), True, txtMercancia, btnMercancia)

    End Sub

    Private Sub AsignaTXT(ByVal nRow As Long)

        i_modo = movimiento.iConsultar

        If nRow >= 0 Then
            With dt

                nPosicionEncab = nRow
                Me.BindingContext(ds, nTabla).Position = nRow

                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

                With .Rows(nRow)
                    'Encabezado 
                    txtCodigo.Text = MuestraCampoTexto(.Item("codfor"))
                    txtEmision.Text = FormatoFecha(CDate(.Item("fecha").ToString))
                    txtMercancia.Text = MuestraCampoTexto(.Item("codart"))

                    txtDescripProduccion.Text = MuestraCampoTexto(.Item("descrip_1"))
                    txtCantidad.Text = FormatoCantidad(.Item("CANTIDAD"))
                    lblUND.Text = MuestraCampoTexto(.Item("UNIDAD"))
                    txtAlmacen.Text = MuestraCampoTexto(.Item("ALMACEN_DESTINO"))
                    RellenaCombo(aEstatus, cmbEstatus, InArray(aEstatus, .Item("estatus")) - 1)

                    tslblPesoT.Text = FormatoCantidad(.Item("kilos"))

                    txtCostosNetos.Text = FormatoNumero(.Item("TOTAL_NETO"))
                    txtCostosIndirectos.Text = FormatoNumero(.Item("TOTAL_INDIRECTOS"))
                    txtCostoTotal.Text = FormatoNumero(.Item("TOTAL"))

                    'Renglones
                    AsignarMovimientos(.Item("codfor"))

                    'Totales
                    CalculaTotales()

                End With
            End With
        Else
            IniciarDocumento(False)
        End If

    End Sub
    Private Sub AsignarMovimientos(ByVal CodigoFormula As String)

        strSQLMov = "select * from jsfabrenfor " _
                            & " where " _
                            & " codfor  = '" & CodigoFormula & "' and " _
                            & " residual = 0 and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"item", "descrip", "cantidad", "unidad", "Costo", "totren", "almacen_salida", ""}
        Dim aNombres() As String = {"Item", "Descripción", "Cant.", "UND", "Costo Unitario", "Costo Total", "Almacén Salida", ""}
        Dim aAnchos() As Long = {90, 350, 100, 45, 70, 100, 70, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", sFormatoCantidad, "", sFormatoNumero, sFormatoNumero, "", ""}
        IniciarTabla(dg, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub

    Private Sub AsignarMovimientosResidual(ByVal CodigoFormula As String)

        strSQLMovRes = "select * from jsfabrenfor " _
                            & " where " _
                            & " codfor  = '" & CodigoFormula & "' and " _
                            & " residual = 1 and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMovRes, myConn, nTablaRenglonesResidual, lblInfo)
        dtRenglonesResidual = ds.Tables(nTablaRenglonesResidual)

        Dim aCampos() As String = {"item", "descrip", "cantidad", "unidad", "porcentaje", "almacen_salida", ""}
        Dim aNombres() As String = {"Item", "Descripción", "Cant.", "UND", "% Costo Total", "Almacén Entrada", ""}
        Dim aAnchos() As Long = {90, 350, 100, 45, 70, 100, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", sFormatoCantidad, "", sFormatoNumero, "", ""}
        IniciarTabla(dgResidual, dtRenglonesResidual, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtRenglonesResidual.Rows.Count > 0 Then
            nPosicionRenglonResidual = 0
            AsignaMovResidual(nPosicionRenglonResidual, True)
        End If

    End Sub
    'Private Sub AbrirIVA(ByVal NumeroDocumento As String)

    '    strSQLIVA = "select * from jsvenivafac " _
    '                        & " where " _
    '                        & " codfor  = '" & NumeroDocumento & "' and " _
    '                        & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
    '                        & " id_emp = '" & jytsistema.WorkID & "' order by tipoiva "

    '    ds = DataSetRequery(ds, strSQLIVA, myConn, nTablaIVA, lblInfo)
    '    dtIVA = ds.Tables(nTablaIVA)

    '    Dim aCampos() As String = {"tipoiva", "poriva", "baseiva", "impiva"}
    '    Dim aNombres() As String = {"", "", "", ""}
    '    Dim aAnchos() As Long = {15, 45, 65, 60}
    '    Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
    '                                    AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}

    '    Dim aFormatos() As String = {"", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero}
    '    'IniciarTabla(dgIVA, dtIVA, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False, , 8, False)

    '    'txtTotalIVA.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(impiva) from jsvenivafac where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' group by codfor ")))

    'End Sub
    'Private Sub AbrirDescuentos(ByVal NumeroDocumento As String)

    '    strSQLDescuentos = "select * from jsvendesfac " _
    '                        & " where " _
    '                        & " codfor  = '" & NumeroDocumento & "' and " _
    '                        & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
    '                        & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

    '    ds = DataSetRequery(ds, strSQLDescuentos, myConn, nTablaDescuentos, lblInfo)
    '    dtDescuentos = ds.Tables(nTablaDescuentos)

    '    Dim aCampos() As String = {"renglon", "pordes", "descuento"}
    '    Dim aNombres() As String = {"", "", ""}
    '    Dim aAnchos() As Long = {60, 45, 60}
    '    Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
    '                                    AlineacionDataGrid.Derecha}

    '    Dim aFormatos() As String = {"", sFormatoNumero, sFormatoNumero}
    '    IniciarTabla(dgDescuentos, dtDescuentos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False, , 8, False)

    '    txtCostosIndirectos.Text = FormatoNumero(CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select sum(descuento) from jsvendesfac where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' group by codfor ")))

    'End Sub

    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "TMP" & NumeroAleatorio(100000)
        Else
            txtCodigo.Text = ""
        End If

        IniciarTextoObjetos(FormatoItemListView.iCadena, txtMercancia, txtNombreMercancia, txtDescripProduccion, _
                            txtAlmacen)

        Dim nAlmacen As String = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, "SELECT codalm FROM jsmercatalm WHERE id_emp = '" & jytsistema.WorkID & "' ORDER BY codalm LIMIT 1"))
        If nAlmacen <> "0" Then txtAlmacen.Text = nAlmacen

        RellenaCombo(aEstatus, cmbEstatus, 1)
        txtEmision.Text = FormatoFecha(sFechadeTrabajo)
        txtCantidad.Text = FormatoCantidad(1)

        tslblPesoT.Text = FormatoCantidad(0)
        IniciarTextoObjetos(FormatoItemListView.iNumero, txtCostosNetos, txtCostosIndirectos, txtCostoTotal)

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", 0, 0)
        AsignarMovimientos(txtCodigo.Text)
        'AbrirIVA(txtCodigo.Text)
        'AbrirDescuentos(txtCodigo.Text)

    End Sub
    Private Sub IncluirFleteAutomatico(Codigocliente As String, montoFlete As Double)

        If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA24")) Then

            Dim codServicioFlete As String = CStr(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA25"))
            Dim monServicioFlete As Double = montoFlete
            If monServicioFlete = 0 Then
                monServicioFlete = CDbl(EjecutarSTRSQL_ScalarPLUS(myConn, " select precio from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'"))
            End If

            Dim desServicioFlete As String = CStr(EjecutarSTRSQL_ScalarPLUS(myConn, " select desser from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'"))
            Dim ivaServicioFlete As String = CStr(EjecutarSTRSQL_ScalarPLUS(myConn, " select tipoiva from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'"))

            Dim numRenglon As String = AutoCodigoPlus(myConn, "renglon", "jsvenrenfac", "codfor", txtCodigo.Text, 5)

            If monServicioFlete <> 0 Then

                InsertEditVENTASRenglonFactura(myConn, lblInfo, True, txtCodigo.Text, numRenglon, "$" & codServicioFlete, desServicioFlete, _
                                                    ivaServicioFlete, "", "UND", 0.0, 1.0, _
                                                    0.0, 0.0, 0, "", "", 0.0, "", "1", _
                                                    monServicioFlete, 0.0, 0.0, _
                                                    0.0, monServicioFlete, monServicioFlete, _
                                                    "", "", "", "", "", "", _
                                                    "", "", "", "1", 1)

                ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
                AsignaMov(0, True)
                CalculaTotales()

            End If


        End If

    End Sub
    Private Sub HabilitarDesdeParametros()

    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        HabilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon, MenuDescuentos)
        HabilitarObjetos(True, True, txtDescripProduccion, btnEmision, btnMercancia, txtMercancia, cmbEstatus, _
                        btnAlmacen, _
                         txtCantidad)


        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)


    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        HabilitarObjetos(False, True, txtCodigo, txtEmision, btnEmision, _
                txtMercancia, txtNombreMercancia, btnMercancia, txtDescripProduccion, _
                cmbEstatus, btnAlmacen, _
                txtNombreAlmacen, txtCantidad)

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

        EjecutarSTRSQL(myConn, lblInfo, " delete from jsfabrenfor where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsfabfijfor where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(myConn, lblInfo, " delete from jsfabadjfor where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
            Imprimir()
        End If
    End Sub
    Private Sub Imprimir()

        If DocumentoImpreso(myConn, lblInfo, "jsvenencfac", "codfor", txtCodigo.Text) Then
            Dim f As New jsVenRepParametros
            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteVentas.cFactura, "Fórmula", txtMercancia.Text, txtCodigo.Text, CDate(txtEmision.Text))
            f = Nothing
        Else
            If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA08")) Then
                '1. Imprimir Nota de Entrega Fiscal
                jytsistema.WorkBox = Registry.GetValue(jytsistema.DirReg, jytsistema.ClaveImpresorFiscal, "")
                If jytsistema.WorkBox = "" Then
                    MensajeCritico(lblInfo, "DEBE INDICAR UNA FORMA DE IMPRESION FISCAL")
                Else
                    '2. Colocar Nota de Entrega como impresa
                    Dim nTipoImpreFiscal As Integer = TipoImpresoraFiscal(myConn, lblInfo, jytsistema.WorkBox)
                    Select Case nTipoImpreFiscal
                        Case 0 ' Fórmula FISCAL FORMA LIBRE
                            ' SE DIRIGE A LA IMPRESORA POR DEFECTO
                            ImprimirFacturaGrafica(myConn, lblInfo, ds, txtCodigo.Text)
                        Case 1 'Fórmula FISCAL PRE-IMPRESA
                        Case 2, 5, 6  'IMPRESORA FISCAL TIPO ACLAS/BIXOLON

                        Case 3 'IMPRESORA FISCAL TIPO BEMATECH
                        Case 4 'IMPRESORA FISCAL TIPO EPSON/PNP

                    End Select
                End If

                MsgBox("SE HA ENVIADO Fórmula A LA IMPRESORA FISCAL...", MsgBoxStyle.Information)


            Else
                MensajeCritico(lblInfo, "Impresión de Fórmula no permitida...")
            End If
        End If

    End Sub
    Private Function Validado() As Boolean


        If txtNombreMercancia.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un cliente válido...")
            Return False
        End If

        'If txtNombreAsesor.Text = "" Then
        '    MensajeCritico(lblInfo, "Debe indicar un nombre de Asesor válido...")
        '    Return False
        'End If

        'If txtNombreTransporte.Text = "" Then
        '    MensajeCritico(lblInfo, "Debe indicar un transporte válido...")
        '    Return False
        'End If

        If txtNombreAlmacen.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un almacén válido...")
            Return False
        End If


        If dtRenglones.Rows.Count = 0 Then
            MensajeCritico(lblInfo, "Debe incluir al menos un ítem...")
            Return False
        End If


        If Not CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPARAM37")) Then 'FACTURAR SI POSEE ESTATUS > 0
            Dim EstatusCliente As Integer = CInt(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select estatus from jsvencatcli where codcli = '" & txtMercancia.Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
            If EstatusCliente = 1 Or EstatusCliente = 2 Then
                MensajeCritico(lblInfo, "Este Cliente posee estatus  " & aEstatusCliente(EstatusCliente) & ". Favor remitir a Administración")
                Return False
            End If
        End If

        If dtRenglones.Rows.Count <= 0 Then
            MensajeCritico(lblInfo, "Debe introducir por lo menos un ítem")
            Return False
        End If

        If Not CBool(ParametroPlus(myConn, Gestion.iVentas, "VENPARAM19")) Then
            If ClientePoseeChequesFuturos(myConn, lblInfo, txtMercancia.Text) Then
                MensajeCritico(lblInfo, " NO SE PUEDE REALIZAR ESTA OPERACION PUES ESTE CLIENTE POSEE CHEQUES A FUTURO ")
                Return False
            End If
        End If


        Return True

    End Function
    Private Sub GuardarTXT()

        Dim Codigo As String = txtCodigo.Text
        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count
            Codigo = Contador(myConn, lblInfo, Gestion.iVentas, "VENNUMFAC", "08")

            EjecutarSTRSQL(myConn, lblInfo, " update jsvenrenfac set codfor = '" & Codigo & "' where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            EjecutarSTRSQL(myConn, lblInfo, " update jsvenivafac set codfor = '" & Codigo & "' where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            EjecutarSTRSQL(myConn, lblInfo, " update jsvendesfac set codfor = '" & Codigo & "' where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            EjecutarSTRSQL(myConn, lblInfo, " update jsvenrencom set numdoc = '" & Codigo & "' where numdoc = '" & txtCodigo.Text & "' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ")
            EjecutarSTRSQL(myConn, lblInfo, " update jsvenforpag set codfor = '" & Codigo & "' where codfor = '" & txtCodigo.Text & "' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        'InsertEditVENTASEncabezadoFactura(myConn, lblInfo, Inserta, Codigo, CDate(txtEmision.Text), txtMercancia.Text, _
        '                                       txtComentario.Text, txtAsesor.Text, txtAlmacen.Text, txtTransporte.Text, _
        '                                       CDate(txtVencimiento.Text), txtReferencia.Text, txtCodigoContable.Text, _
        '                                       ValorEntero(dtRenglones.Rows.Count), 0.0, ValorCantidad(tslblPesoT.Text), _
        '                                       ValorNumero(txtSubTotal.Text), 0.0, 0.0, 0.0, 0.0, 0.0, ValorNumero(txtDescuentos.Text), _
        '                                       0.0, 0.0, 0.0, 0.0, ValorNumero(txtCargos.Text), 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, _
        '                                       jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, jytsistema.sFechadeTrabajo, _
        '                                       CondicionDePago, TipoCredito, "", "", "", "", 0.0, "", "", 0.0, "", "", 0.0, "", "", _
        '                                       0.0, "", "", 0.0, 0.0, "", 0, 0, 0.0, 0.0, "", jytsistema.sFechadeTrabajo, 0.0, 0.0, _
        '                                       ValorNumero(txtTotalIVA.Text), 0.0, 0, 1, ValorNumero(txtTotal.Text), _
        '                                       0, _
        '                                       cmbEstatus.Text, "", "", "", "", "", Impresa)


        ActualizarMovimientos(Codigo)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)


        Dim row As DataRow = dt.Select(" codfor = '" & Codigo & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub ActualizarMovimientos(ByVal NumeroFactura As String)

        '1.- Aplica Descuento Global sobre total Renglón con descuento "totrendes"
        EjecutarSTRSQL(myConn, lblInfo, " update jsvenrenfac set totrendes = totren - totren * " & ValorNumero(txtCostosIndirectos.Text) / IIf(ValorNumero(txtCostosNetos.Text) > 0, ValorNumero(txtCostosNetos.Text), 1) & " " _
                                        & " where " _
                                        & " codfor = '" & NumeroFactura & "' and " _
                                        & " renglon > '' and " _
                                        & " item > '' and " _
                                        & " estatus = '0' AND " _
                                        & " aceptado < '2' and " _
                                        & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                        & " id_emp = '" & jytsistema.WorkID & "' ")

        '2.- Elimina movimientos de inventarios anteriores de la Fórmula
        EliminarMovimientosdeInventario(myConn, NumeroFactura, "FAC", lblInfo)

        '3.- Actualizar Movimientos de Inventario con Fórmula
        strSQLMov = "select * from jsvenrenfac " _
                            & " where " _
                            & " codfor  = '" & NumeroFactura & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        For Each dtRow As DataRow In dtRenglones.Rows
            With dtRow
                Dim CostoActual As Double = UltimoCostoAFecha(myConn, lblInfo, .Item("item"), CDate(txtEmision.Text)) / Equivalencia(myConn, lblInfo, .Item("item"), .Item("unidad"))

                ActualizarExistenciasPlus(myConn, .Item("item"))
            End With
        Next

        '4.- Actualizar existencias de renglones eliminados
        For Each aSTR As Object In Eliminados
            ActualizarExistenciasPlus(myConn, aSTR)
        Next


        '7.- Actualiza Cantidades en tránsito de los presupuestos y estatus del documento
        ActualizarRenglonesEnPresupuestos(myConn, lblInfo, ds, dtRenglones, "jsvenrenfac")

        '8.- Actualiza cantidades en tránsito de los prepedidos y estatus de documento
        ActualizarRenglonesEnPrepedidos(myConn, lblInfo, ds, dtRenglones, "jsvenrenfac")

        '9.- Actualiza cantidades en tránsito de los pedidos y estatus del documento
        ActualizarRenglonesEnPedidos(myConn, lblInfo, ds, dtRenglones, "jsvenrenfac")

        '10.- Actualiza cantidades en tránsito de las notas de entrega y estatus del documento
        ActualizarRenglonesEnNotasDeEntrega(myConn, lblInfo, ds, dtRenglones, "jsvenrenfac")

    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescripProduccion.GotFocus
        MensajeEtiqueta(lblInfo, " Indique comentario ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
        HabilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtMercancia, btnMercancia)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA01")) Then
            If DocumentoPoseeCancelacionesAbonosDepositados(myConn, lblInfo, txtCodigo.Text) Then
                MensajeCritico(lblInfo, "DOCUMENTO POSEE CANCELACIONES Y/O ABONOS DEPOSITADOS. VERIFIQUE POR FAVOR ...")
            Else
                If DocumentoPoseeCancelacionesAbonos(myConn, lblInfo, txtCodigo.Text) Then
                    MensajeCritico(lblInfo, "DOCUMENTO POSEE CANCELACIONES Y/O ABONOS. VERIFIQUE POR FAVOR...")
                Else
                    i_modo = movimiento.iEditar
                    nPosicionEncab = Me.BindingContext(ds, nTabla).Position
                    ActivarMarco0()
                    HabilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtMercancia, btnMercancia)
                End If
            End If
        Else
            MensajeCritico(lblInfo, "Edición de Formulaciones NO está permitida...")
        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If CBool(ParametroPlus(myConn, Gestion.iVentas, "VENFACPA01")) Then
            Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position
            If DocumentoPoseeCancelacionesAbonos(myConn, lblInfo, txtCodigo.Text) Then
                MensajeCritico(lblInfo, "ESTE DOCUMENTO POSEE ABONOS Y/O CANCELACIONES. VERIFIQUE POR FAVOR...")
            Else
                sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")
                If sRespuesta = MsgBoxResult.Yes Then

                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("codfor"))
                    '
                    For Each dtRow As DataRow In dtRenglones.Rows
                        With dtRow
                            Eliminados.Add(.Item("item"))
                            'Actualiza Cantidades en tránsito de los presupuestos y estatus del documento
                            ActualizarRenglonesEnPresupuestos(myConn, lblInfo, ds, dtRenglones, "jsvenrenfac")

                            'Actualiza cantidades en tránsito de los prepedidos y estatus de documento
                            ActualizarRenglonesEnPrepedidos(myConn, lblInfo, ds, dtRenglones, "jsvenrenfac")

                            'Actualiza cantidades en tránsito de los pedidos y estatus del documento
                            ActualizarRenglonesEnPedidos(myConn, lblInfo, ds, dtRenglones, "jsvenrenfac")

                            'Actualiza cantidades en tránsito de las notas de entrega y estatus del documento
                            ActualizarRenglonesEnNotasDeEntrega(myConn, lblInfo, ds, dtRenglones, "jsvenrenfac")
                        End With
                    Next

                    EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenencfac where codfor = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "'")
                    EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrenfac where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenivafac where codfor = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ")
                    EjecutarSTRSQL(myConn, lblInfo, " DELETE from jsvendesfac where codfor = '" & txtCodigo.Text & "' and ID_EMP = '" & jytsistema.WorkID & "' and EJERCICIO =  '" & jytsistema.WorkExercise & "' ")
                    EjecutarSTRSQL(myConn, lblInfo, " DELETE FROM jsbantraban where NUMDOC = '" & txtCodigo.Text & "' AND TIPOMOV = 'DP' AND ORIGEN = 'FAC' AND NUMORG = '" & txtCodigo.Text & "' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                    EjecutarSTRSQL(myConn, lblInfo, " DELETE FROM jsbantracaj where NUMMOV = '" & txtCodigo.Text & "' AND ORIGEN = 'FAC' AND TIPOMOV = 'EN' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                    EjecutarSTRSQL(myConn, lblInfo, " delete from jsventracob where NUMMOV = '" & txtCodigo.Text & "' AND ORIGEN = 'FAC' AND TIPOMOV = 'FC' AND EJERCICIO = '" & jytsistema.WorkExercise & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                    EjecutarSTRSQL(myConn, lblInfo, " DELETE FROM jsventabtic WHERE NUMCAN = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                    EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrengui where codigofac = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    EliminarMovimientosdeInventario(myConn, txtCodigo.Text, "FAC", lblInfo)

                    For Each aSTR As Object In Eliminados
                        ActualizarExistenciasPlus(myConn, aSTR)
                    Next

                    ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                    dt = ds.Tables(nTabla)
                    If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                    AsignaTXT(nPosicionEncab)

                End If

            End If
        Else
            MensajeCritico(lblInfo, "Eliminación de Formulaciones NO está permitida...")
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"codfor", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Nombre", "Emisión", "Comentario"}
        Dim Anchos() As Long = {150, 400, 100, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Formulaciones...")
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
            Case 10
                e.Value = aTipoRenglon(e.Value)
        End Select
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "ItemsRenglon", "lblItemsRenglon", e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub CalculaTotales()


        'CalculaBonificaciones(myConn, lblInfo, ModuloBonificacion.iFactura, txtCodigo.Text, CDate(txtEmision.Text), cmbTarifa.Text)
        'If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA12")) Then AjustarExistencias(myConn, txtCodigo.Text, lblInfo, txtAlmacen.Text, "codfor", "jsvenrenfac", "FAC")


        txtCostosNetos.Text = FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrenfac", "codfor", "totren", txtCodigo.Text, 0))
        'AbrirDescuentos(txtCodigo.Text)
        'txtCargos.Text = FormatoNumero(CalculaTotalRenglonesVentas(myConn, lblInfo, "jsvenrenfac", "codfor", "totren", txtCodigo.Text, 1))
        'CalculaDescuentoEnRenglones()
        'CalculaTotalIVAVentas(myConn, lblInfo, "jsvendesfac", "jsvenivafac", "jsvenrenfac", "codfor", txtCodigo.Text, "impiva", "totrendes", CDate(txtEmision.Text), "totren")
        'AbrirIVA(txtCodigo.Text)
        'txtCostoTotal.Text = FormatoNumero(ValorNumero(txtSubTotal.Text) - ValorNumero(txtDescuentos.Text) + ValorNumero(txtCargos.Text) + ValorNumero(txtTotalIVA.Text))

        tslblPesoT.Text = FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsvenrenfac", "peso", "codfor", txtCodigo.Text))



    End Sub

    Private Sub CalculaDescuentoEnRenglones()

        EjecutarSTRSQL_Scalar(myConn, lblInfo, " update jsvenrenfac set totrendes = totren - totren * " & ValorNumero(txtCostosIndirectos.Text) / IIf(ValorNumero(txtCostosNetos.Text) > 0, ValorNumero(txtCostosNetos.Text), 1) & " " _
            & " where " _
            & " codfor = '" & txtCodigo.Text & "' and " _
            & " renglon > '' and " _
            & " item > '' and " _
            & " ESTATUS = '0' AND " _
            & " ACEPTADO < '2' and " _
            & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
            & " ID_EMP = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        If txtNombreMercancia.Text <> "" Then
            Dim f As New jsProArcFormulacionesRenglones
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, txtCodigo.Text, txtAlmacen.Text)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        If dtRenglones.Rows.Count > 0 Then
            With dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position)
                Dim f As New jsProArcFormulacionesRenglones
                f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
                f.Editar(myConn, ds, dtRenglones, "FAC", txtCodigo.Text, txtAlmacen.Text,
                         IIf(.Item("item").ToString.Substring(1, 1) = "$", True, False))
                ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
                AsignaMov(f.Apuntador, True)
                CalculaTotales()
                f = Nothing
            End With
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
                    If .Item("EDITABLE") = 0 Then
                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("item"))

                        Eliminados.Add(.Item("item"))

                        Dim aCamposDel() As String = {"codfor", "item", "renglon", "id_emp"}
                        Dim aStringsDel() As String = {txtCodigo.Text, .Item("item"), .Item("renglon"), jytsistema.WorkID}

                        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencom where numdoc = '" & txtCodigo.Text & "' and " _
                               & " origen = 'FAC' and item = '" & .Item("item") & "' and renglon = '" & .Item("renglon") & "' and " _
                               & " id_emp = '" & jytsistema.WorkID & "' ")

                        nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsvenrenfac", strSQLMov, aCamposDel, aStringsDel, _
                                                      Me.BindingContext(ds, nTablaRenglones).Position, True)

                        If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1
                        AsignaMov(nPosicionRenglon, True)

                        CalculaTotales()
                    Else
                        MensajeCritico(lblInfo, " Renglón NO EDITABLE ")
                    End If

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
        txtEmision.Text = SeleccionaFecha(CDate(txtEmision.Text), Me, sender)
    End Sub


    Private Sub txtMercancia_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMercancia.TextChanged
        If txtMercancia.Text <> "" Then
            txtNombreMercancia.Text = EjecutarSTRSQL_ScalarPLUS(myConn, " select nomart from jsmerctainv where codart = '" & txtMercancia.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
            lblUND.Text = EjecutarSTRSQL_ScalarPLUS(myConn, " select UNIDAD from jsmerctainv where codart = '" & txtMercancia.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        End If
    End Sub

    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs)
        If e.ColumnIndex = 1 Then e.Value = FormatoNumero(e.Value) & "%"
    End Sub

    Private Sub txtAsesor_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim aFld() As String = {"codven", "tipo", "id_emp"}
        'Dim aStr() As String = {txtAsesor.Text, "0", jytsistema.WorkID}
        'txtNombreAsesor.Text = qFoundAndSign(myConn, lblInfo, "jsvencatven", aFld, aStr, "apellidos") & ", " _
        '        & qFoundAndSign(myConn, lblInfo, "jsvencatven", aFld, aStr, "nombres")
    End Sub

    Private Sub btnAgregaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaDescuento.Click
        If txtMercancia.Text.Trim <> "" Then
            If ClienteFacturaAPartirDeCostos(myConn, lblInfo, txtMercancia.Text) Then
                MensajeCritico(lblInfo, "DESCUENTOS NO PERMITIDOS")
            Else
                'Dim f As New jsGenDescuentosVentas
                'f.Agregar(myConn, ds, dtDescuentos, "jsvendesfac", "codfor", txtCodigo.Text, sModulo, txtAsesor.Text, 0, CDate(txtEmision.Text), ValorNumero(txtSubTotal.Text))
                'CalculaTotales()
                'f = Nothing
            End If
        End If
    End Sub
    'Private Sub btnEliminaDescuento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaDescuento.Click
    '    If nPosicionDescuento >= 0 Then
    '        With dtDescuentos.Rows(nPosicionDescuento)
    '            Dim aCamposDel() As String = {"codfor", "renglon", "id_emp"}
    '            Dim aStringsDel() As String = {txtCodigo.Text, .Item("renglon"), jytsistema.WorkID}
    '            nPosicionDescuento = EliminarRegistros(myConn, lblInfo, ds, nTablaDescuentos, "jsvendesfac", strSQLDescuentos, aCamposDel, aStringsDel, _
    '                                                  Me.BindingContext(ds, nTablaDescuentos).Position, True)
    '        End With
    '        CalculaTotales()
    '    End If
    'End Sub

    Private Sub btnMercancia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMercancia.Click

        txtMercancia.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codart codigo, nomart descripcion, " _
                                              & " elt(tipoart+1,  'Venta', 'Uso interno', 'POP', 'Alquiler', 'Préstamo', 'Materia prima', 'Venta & Envase', 'Otros') Tipo " _
                                              & " from jsmerctainv where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Mercancías", _
                                                txtMercancia.Text)

    End Sub


    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If txtNombreMercancia.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "FAC", txtCodigo.Text, CDate(txtEmision.Text), txtAlmacen.Text, cmbEstatus.Text, txtMercancia.Text, True)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
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


    'Private Sub dgDescuentos_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgDescuentos.KeyPress
    '    If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Down) _
    '        Or e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Up) Then
    '        MsgBox("BAJO")
    '    End If
    'End Sub

    'Private Sub dgDescuentos_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgDescuentos.RowHeaderMouseClick, _
    '    dgDescuentos.CellMouseClick
    '    Me.BindingContext(ds, nTablaDescuentos).Position = e.RowIndex
    '    nPosicionDescuento = e.RowIndex
    'End Sub


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

                f.Cambiar(myConn, ds, dtRenglones, txtCodigo.Text, txtMercancia.Text, .Item("renglon"), .Item("item"), .Item("unidad"), .Item("cantidad"), .Item("precio"))
                If f.NuevoPrecio <> PrecioAnterior Then
                    EjecutarSTRSQL(myConn, lblInfo, " update jsvenrenfac " _
                                   & " set precio = " & f.NuevoPrecio & ", " _
                                   & " totren = " & f.NuevoPrecio * .Item("cantidad") & ", " _
                                   & " totrendes = " & f.NuevoPrecio * .Item("cantidad") * (1 - .Item("des_art") / 100) * (1 - .Item("des_cli") / 100) * (1 - .Item("des_ofe") / 100) & " " _
                                   & " where " _
                                   & " codfor = '" & txtCodigo.Text & "' and " _
                                   & " renglon = '" & .Item("renglon") & "' and " _
                                   & " item = '" & .Item("item") & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "' ")
                    'f.NuevoPrecio
                    ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
                    AsignaMov(nPosicionRenglon, True)
                    CalculaTotales()
                End If

                f = Nothing
            End With
        End If
    End Sub



    Private Sub tbcRenglones_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles tbcRenglones.SelectedIndexChanged
        Select Case tbcRenglones.SelectedIndex
            Case 0 'FORMULA
                AsignarMovimientos(txtCodigo.Text)
            Case 1 'RESIDUALES
                AsignarMovimientosResidual(txtCodigo.Text)
        End Select
    End Sub
End Class