Imports MySql.Data.MySqlClient
Public Class jsBanArcCajas
    Private Const sModulo As String = "Cajas"
    Private Const lRegion As String = ""
    Private Const nTabla As String = "cajas"
    Private Const nTablaMovimientos As String = "movimientos_caja"

    Private strSQL As String = "select * from jsbanenccaj where id_emp = '" & jytsistema.WorkID & "' order by caja "
    Private strSQLMov As String

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private myCom As New MySqlCommand(strSQL, myConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtMovimientos As New DataTable

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long

    Private Sub jsBanCajas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo
        Try
            myConn.Open()

            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)



            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionEncab = 0
                AsignaTXT(nPosicionEncab)
            Else
                IniciarCaja(False)
            End If
            ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            AsignarTooltips()
            IniciarCajasyCestatickets()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva caja")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> caja actual")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> caja actual")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> caja deseada")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primera</B> caja")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a caja <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a caja <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>última  caja</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> movimientos de caja actual")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnRemesas, "Construcción de <B>remesas</B> de cheques de alimentación")
        C1SuperTooltip1.SetToolTip(btnAdelantoEfectivo, "Adelanto, cambio ó intercambio de <B>Efectivo</B> por tarjetas y/o cheques")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> movimiento en la caja actual")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> movimiento de la caja actual")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> movimiento de la caja actual")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un movimiento en la caja actual")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> movimiento de la caja actual")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> movimiento de la caja actual")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al movimiento <B>siguiente </B> de la caja actual")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> movimiento de la caja actual")


    End Sub
    Private Sub IniciarCajasyCestatickets()
        Dim iCont As Integer
        Dim dtCorredores As New DataTable
        Dim nTablaCorredores As String = "corredores"
        ds = DataSetRequery(ds, "select * from jsvencestic where id_emp = '" & jytsistema.WorkID & "' order by codigo ", myConn, nTablaCorredores, lblInfo)
        dtCorredores = ds.Tables(nTablaCorredores)
        For iCont = 0 To dtCorredores.Rows.Count - 1
            Dim tsCT As New ToolStripMenuItem(dtCorredores.Rows(iCont).Item("codigo").ToString & " | " & dtCorredores.Rows(iCont).Item("descrip").ToString, Nothing, New EventHandler(AddressOf btnRemesas_Click))
            btnRemesas.DropDownItems.Add(tsCT)
        Next
        CType(btnRemesas.DropDown, ToolStripDropDownMenu).ShowImageMargin = False
    End Sub
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)

        If c >= 0 Then
            Me.BindingContext(ds, nTablaMovimientos).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", c, dtMovimientos.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        AsignaSaldos(txtCodigo.Text)

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtMovimientos, MenuBarraRenglon)
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt
            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

            With .Rows(nRow)
                'Bancos 
                txtCodigo.Text = .Item("caja")
                txtNombre.Text = IIf(IsDBNull(.Item("nomcaja")), "", .Item("nomcaja"))
                txtCodigoContable.Text = MuestraCampoTexto(.Item("codcon"))
                '  txtSaldo.Text = IIf(IsDBNull(.Item("saldo")), 0.0, FormatoNumero(.Item("saldo")))

                'Movimientos

                strSQLMov = "select * from jsbantracaj " _
                    & " where " _
                    & " caja  = '" & .Item("caja") & "' and " _
                    & " deposito = '' and " _
                    & " fecha <= '" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
                    & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                    & " id_emp = '" & jytsistema.WorkID & "' order by fecha desc, tipomov, nummov "

                ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                dtMovimientos = ds.Tables(nTablaMovimientos)

                Dim aCampos() As String = {"fecha", "tipomov", "nummov", "formpag", "numpag", "refpag", "Importe", "origen", "prov_cli", "codven", "concepto"}
                Dim aNombres() As String = {"Fecha", "TP", "Documento", "FP", "Nº Pago", "Ref. Pago", "Importe", "ORG", "Prov./Cli.", "Asesor", "Concepto"}
                Dim aAnchos() As Long = {80, 25, 85, 25, 75, 75, 100, 30, 80, 50, 120}
                Dim aAlineacion() As Integer = {DataGridViewContentAlignment.MiddleCenter, DataGridViewContentAlignment.MiddleCenter, _
                    DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleCenter, DataGridViewContentAlignment.MiddleLeft, _
                    DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.MiddleCenter, _
                    DataGridViewContentAlignment.MiddleCenter, DataGridViewContentAlignment.MiddleCenter, DataGridViewContentAlignment.MiddleLeft}
                Dim aFormatos() As String = {sFormatoFecha, "", "", "", "", "", sFormatoNumero, "", "", "", ""}
                IniciarTabla(dg, dtMovimientos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
                If dtMovimientos.Rows.Count > 0 Then
                    nPosicionRenglon = 0
                    AsignaMov(nPosicionRenglon, True)
                Else
                    AsignaSaldos(.Item("caja").ToString)
                End If



            End With
        End With
    End Sub
    Private Sub AsignaSaldos(ByVal numCaja As String)

        txtEF.Text = FormatoNumero(CalculaSaldoCajaPorFP(myConn, numCaja, "EF", lblInfo))
        txtCH.Text = FormatoNumero(CalculaSaldoCajaPorFP(myConn, numCaja, "CH", lblInfo))
        txtTA.Text = FormatoNumero(CalculaSaldoCajaPorFP(myConn, numCaja, "TA", lblInfo))
        txtCT.Text = FormatoNumero(CalculaSaldoCajaPorFP(myConn, numCaja, "CT", lblInfo))
        txtOT.Text = FormatoNumero(CalculaSaldoCajaPorFP(myConn, numCaja, "OT", lblInfo))
        txtSaldo.Text = FormatoNumero(CalculaSaldoCajaPorFP(myConn, numCaja, "", lblInfo))

    End Sub

    Private Sub IniciarCaja(ByVal Inicio As Boolean)
        If Inicio Then
            Dim aWherefields() As String = {"id_emp"}
            Dim aWhereValues() As String = {jytsistema.WorkID}
            txtCodigo.Text = AutoCodigoXPlus(myConn, "caja", "jsbanenccaj", aWherefields, aWhereValues, 2, True)   'AutoCodigo(2, ds, nTabla, "caja")
        Else
            txtCodigo.Text = ""
        End If

        IniciarTextoObjetos(FormatoItemListView.iCadena, txtNombre, txtCodigoContable)

        txtSaldo.Text = FormatoNumero(0.0)
        txtEF.Text = FormatoNumero(0.0)
        txtCH.Text = FormatoNumero(0.0)
        txtTA.Text = FormatoNumero(0.0)
        txtCT.Text = FormatoNumero(0.0)
        txtOT.Text = FormatoNumero(0.0)

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "itemsrenglon", "lblitemsrenglon", 0, 0)
        dg.Columns.Clear()
    End Sub
    Private Sub ActivarMarco0()

        VisualizarObjetos(True, grpAceptarSalir)
        HabilitarObjetos(True, True, txtNombre, btnCodCon)

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub


    Private Sub DesactivarMarco0()

        VisualizarObjetos(False, grpAceptarSalir)
        HabilitarObjetos(False, True, txtCodigo, txtCH, txtCodigoContable, txtCT, txtEF, txtNombre, txtOT, txtSaldo, txtTA, btnCodCon)

        MenuBarra.Enabled = True
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If dt.Rows.Count = 0 Then
            IniciarCaja(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            AsignaTXT(nPosicionEncab)
            ' If Me.BindingContext(ds, nTabla).Position > 0 Then _
            'AsignaTXT(Me.BindingContext(ds, nTabla).Position)
        End If
        DesactivarMarco0()
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado() As Boolean
        If txtNombre.Text.Trim = "" Then
            MensajeCritico(lblInfo, "INDIQUE NOMBRE DE CAJA VALIDO...")
            Return False
        End If

        If txtCodigoContable.Text.Trim = "" Then
            MensajeCritico(lblInfo, "INDIQUE CODIGO CONTABLE CAJA VALIDO...")
            Return False
        End If


        If CBool(ParametroPlus(myConn, Gestion.iBancos, "BANPARAM17")) Then
            If txtCodigoContable.Text.Trim = "" Then
                MensajeCritico(lblInfo, "Debe indicar una CUENTA CONTABLE VALIDA...")
                Return False
            Else
                If CInt(EjecutarSTRSQL_ScalarPLUS(myConn, " Select count(*) from jscotcatcon where codcon = '" & txtCodigoContable.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")) = 0 Then
                    MensajeCritico(lblInfo, " Debe indicar una CUENTA CONTABLE Válida...")
                    Return False
                End If
                If CInt(EjecutarSTRSQL_ScalarPLUS(myConn, " select count(*) from jsbanenccaj where codcon = '" & txtCodigoContable.Text & "' and caja <> '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")) > 0 Then
                    MensajeCritico(lblInfo, " La cuenta contable YA ha sido asignada a otra CAJA ... ")
                    Return False
                End If
            End If
        End If


        Validado = True

    End Function
    Private Sub GuardarTXT()

        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count
        End If

        InsertEditBANCOSEncabezadoCaja(myConn, lblInfo, Inserta, txtCodigo.Text, txtNombre.Text, txtCodigoContable.Text, ValorNumero(txtSaldo.Text))

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombre.GotFocus
        MensajeEtiqueta(lblInfo, " Indique el nombre de la caja ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarCaja(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        i_modo = movimiento.iEditar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If dt.Rows(Me.BindingContext(ds, nTabla).Position).Item("caja") <> "00" Then

        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"caja", "nomcaja"}
        Dim Nombres() As String = {"Código caja", "Nombre caja"}
        Dim Anchos() As Long = {100, 2500}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, " Cajas")
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
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, "ItemsRenglon", "lblItemsRenglon", e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
    End Sub


    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click

        Dim f As New jsBanArcCajasMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaMovimientos).Position
        f.Agregar(myConn, ds, dtMovimientos, txtCodigo.Text)
        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        AsignaMov(f.Apuntador, True)
        f = Nothing

    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        If dtMovimientos.Rows.Count > 0 Then
            If dtMovimientos.Rows(Me.BindingContext(ds, nTablaMovimientos).Position).Item("origen") = "CAJ" Then
                Dim f As New jsBanArcCajasMovimientos
                f.Apuntador = Me.BindingContext(ds, nTablaMovimientos).Position
                f.Editar(myConn, ds, dtMovimientos, txtCodigo.Text)
                ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                AsignaMov(f.Apuntador, True)
                f = Nothing
            End If
        End If

    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click

        EliminarMovimiento()

    End Sub
    Private Sub EliminarMovimiento()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult

        nPosicionRenglon = Me.BindingContext(ds, nTablaMovimientos).Position

        If nPosicionRenglon >= 0 Then
            If dtMovimientos.Rows(nPosicionRenglon).Item("origen") = "CAJ" Then
                sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                If sRespuesta = MsgBoxResult.Yes Then

                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dtMovimientos.Rows(nPosicionRenglon).Item("nummov"))

                    EjecutarSTRSQL(myConn, lblInfo, "DELETE FROM jsbantracaj where " _
                        & " RENGLON = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "'")

                    EjecutarSTRSQL(myConn, lblInfo, " update jsbanenccaj set saldo = " & CalculaSaldoCajaPorFP(myConn, dtMovimientos.Rows(nPosicionRenglon).Item("caja"), "", lblInfo) & " where caja = '" & dtMovimientos.Rows(nPosicionRenglon).Item("caja") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                    ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                    dtMovimientos = ds.Tables(nTablaMovimientos)
                    If dtMovimientos.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtMovimientos.Rows.Count - 1
                    AsignaMov(nPosicionRenglon, True)

                End If
            Else
                MensajeCritico(lblInfo, "Movimiento proveniente de " & dtMovimientos.Rows(nPosicionRenglon).Item("origen") & ". ")
            End If

        End If

    End Sub
    Private Sub btnBuscarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarMovimiento.Click

        Dim f As New frmBuscar
        Dim Campos() As String = {"fecha", "nummov", "formpag", "numpag", "refpag", "prov_cli", "codven", "importe"}
        Dim Nombres() As String = {"Emisión", "Nº Movimiento", "Forma Pago", "Número Pago", "Referencia Pago", "Proveedor/Cliente", "Asesor Comercial", "Importe"}
        Dim Anchos() As Long = {140, 140, 100, 150, 150, 150, 120, 150}
        f.Text = "Movimientos de caja"
        f.Buscar(dtMovimientos, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaMovimientos).Position, " Movimientos de Caja")
        AsignaMov(f.Apuntador, False)
        f = Nothing

    End Sub

    Private Sub btnPrimerMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimerMovimiento.Click
        Me.BindingContext(ds, nTablaMovimientos).Position = 0
        AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
    End Sub

    Private Sub btnAnteriorMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnteriorMovimiento.Click
        Me.BindingContext(ds, nTablaMovimientos).Position -= 1
        AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
    End Sub

    Private Sub btnSiguienteMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguienteMovimiento.Click
        Me.BindingContext(ds, nTablaMovimientos).Position += 1
        AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
    End Sub

    Private Sub btnUltimoMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimoMovimiento.Click
        Me.BindingContext(ds, nTablaMovimientos).Position = ds.Tables(nTablaMovimientos).Rows.Count - 1
        AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
    End Sub

    Private Sub btnRemesas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemesas.Click
        '
    End Sub

    Private Sub btnRemesas_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles btnRemesas.DropDownItemClicked
        nPosicionRenglon = Me.BindingContext(ds, nTablaMovimientos).Position
        Dim f As New jsBanArcRemesasCestaTicket
        f.Remesas(myConn, ds, txtCodigo.Text, e.ClickedItem.Text)
        AsignaMov(nPosicionRenglon, True)
        f = Nothing
    End Sub

    Private Sub btnAdelantoEfectivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdelantoEfectivo.Click
        Dim f As New jsBanArcCajasAvanceEF
        f.Apuntador = Me.BindingContext(ds, nTablaMovimientos).Position
        f.Agregar(myConn, ds, dtMovimientos, txtCodigo.Text)
        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        AsignaMov(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cMovimientoCaja, "Movimientos Caja", txtCodigo.Text)
        f = Nothing
    End Sub

    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(ds, nTablaMovimientos).Position += 1
                nPosicionRenglon = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(nPosicionRenglon, False)
            Case Keys.Up
                Me.BindingContext(ds, nTablaMovimientos).Position -= 1
                nPosicionRenglon = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(nPosicionRenglon, False)
        End Select
    End Sub

    Private Sub btnCodCon_Click(sender As System.Object, e As System.EventArgs) Handles btnCodCon.Click
        txtCodigoContable.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables", _
                                                   txtCodigoContable.Text)
    End Sub
End Class