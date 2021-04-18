Imports MySql.Data.MySqlClient
Public Class jsControlArcCorredoresCestaTicket
    Private Const sModulo As String = "Corredores de cheques de alimentación"
    Private Const lRegion As String = "RibbonButton183"
    Private Const nTabla As String = "tblCorredores"
    Private Const nTablaTipos As String = "tblTipoCestaTicket"
    Private Const nTablaValores As String = "tblValoresCestaTicket"
    Private Const nTablaIVA As String = "tblIVA"

    Private strSQL As String = "select * from jsvencestic where id_emp = '" & jytsistema.WorkID & "' order by codigo "
    Private strSQLTipos As String
    Private strSQLValores As String
    Private strSQLIVA As String = " SELECT tipo, monto FROM jsconctaiva " _
                                  & " WHERE fecha IN ( SELECT MAX(fecha) FROM jsconctaiva " _
                                  & "                   WHERE fecha <= '" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' " _
                                  & "                   GROUP BY tipo ) " _
                                  & " ORDER BY tipo "

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private myCom As New MySqlCommand(strSQL, myConn)
    Private da As New MySqlDataAdapter(myCom)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtTipos As New DataTable
    Private dtValores As New DataTable
    Private dtIVA As New DataTable

    Private i_modo As Integer
    Private nPosicionCat As Long, nPosicionTip As Long, nPosicionVal As Long
    Private Proveedor As String, Grupo As String, SubGrupo As String

    Private BindingSource1 As New BindingSource
    Private FindField As String

    Private Sub jsControlArcCorredoresCestaTicket_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo
        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)
            ds = DataSetRequery(ds, strSQLIVA, myConn, nTablaIVA, lblInfo)
            dtIVA = ds.Tables(nTablaIVA)


            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaTXT(nPosicionCat)
            Else
                IniciarCorredor(False)
            End If
            ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            AsignarTooltips()
        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nuevo registro")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> registro deseado")
        C1SuperTooltip1.SetToolTip(btnSeleccionar, "<B>Seleccionar</B> registro actual")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir al <B>primer</B> registro")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir al <B>siguiente</B> registro")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir al registro <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir al <B>último registro</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B>")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")

        'Botones Adicionales
        C1SuperTooltip1.SetToolTip(btnCodigoContable, "Seleccione la fecha de ingreso ó<br> la fecha de apertura/activación de la cuenta")
        C1SuperTooltip1.SetToolTip(btnGrupo, "Seleccione el <B>formato</B> para impresión de cheque en el voucer de pago")


    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLValores, myConn, nTablaValores, lblInfo)

        If c >= 0 AndAlso dtValores.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaValores).Position = c
            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", c, dtValores.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtValores, MenuBarra)

    End Sub
    Private Sub AsignaTipo(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLTipos, myConn, nTablaTipos, lblInfo)

        If c >= 0 AndAlso dtTipos.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaTipos).Position = c
            dgCom.Refresh()
            dgCom.CurrentCell = dgCom(0, c)
        End If

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt

            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

            With .Rows(nRow)
                'Corredor
                nPosicionCat = nRow

                txtCodigo.Text = .Item("codigo")
                txtNombre.Text = MuestraCampoTexto(.Item("descrip"))
                txtLongitudBarra.Text = FormatoEntero(.Item("lencodbar"))
                txtInicioPrecio.Text = FormatoEntero(.Item("inicioprecio"))
                txtLongitudPrecio.Text = FormatoEntero(.Item("lenprecio"))
                txtInicioTipo.Text = FormatoEntero(.Item("iniciotipo"))
                txtLongitudTipo.Text = FormatoEntero(.Item("lentipo"))
                txtCargos.Text = FormatoNumero(.Item("cargos"))
                Proveedor = .Item("codpro")
                txtProveedor.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select nombre from jsprocatpro where codpro = '" & Proveedor & "' and id_emp = '" & jytsistema.WorkID & "' ")

                Grupo = .Item("grupo")
                SubGrupo = .Item("subgrupo")
                txtGrupoNombre.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select nombre from jsprogrugas where codigo = '" & Grupo & "' and id_emp = '" & jytsistema.WorkID & "' ")
                txtSubgrupoNombre.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select nombre from jsprogrugas where codigo = '" & SubGrupo & "' and id_emp = '" & jytsistema.WorkID & "' ")

                Dim aIVA() As String = ArregloIVA(myConn, lblInfo)
                Dim tIVA As Integer = InArray(aIVA, .Item("tipoiva").ToString) - 1
                RellenaComboConDatatable(cmbIVA, dtIVA, "monto", "tipo", tIVA)
                txtCodigoContable.Text = MuestraCampoTexto(.Item("codcon"))

                'Movimientos
                txtCodigo1.Text = .Item("codigo")
                txtNombre1.Text = MuestraCampoTexto(.Item("descrip"))

                strSQLTipos = " select * from jsvencestip where corredor = '" & .Item("codigo") & "' and id_emp = '" & jytsistema.WorkID & "' order by tipo "
                ds = DataSetRequery(ds, strSQLTipos, myConn, nTablaTipos, lblInfo)
                dtTipos = ds.Tables(nTablaTipos)

                Dim aCam() As String = {"tipo", "descrip", "com_corredor", "com_cliente"}
                Dim aNom() As String = {"Código Tipo", "Nombre o Descripción", "Comisión Corredor", "Comisión Cliente"}
                Dim aAnc() As Long = {50, 220, 60, 60}
                Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}
                Dim aFor() As String = {"", "", sFormatoNumero, sFormatoNumero}

                IniciarTabla(dgCom, dtTipos, aCam, aNom, aAnc, aAli, aFor)
                If dtTipos.Rows.Count > 0 Then nPosicionTip = 0

            End With
        End With

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub
    Private Sub AbrirMovimientos(ByVal CodigoCorredor As String)

        strSQLValores = "select * from jsvenvaltic " _
                            & " where " _
                            & " codigo  = '" & CodigoCorredor & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' " _
                            & " order by enbarra "

        ds = DataSetRequery(ds, strSQLValores, myConn, nTablaValores, lblInfo)
        dtValores = ds.Tables(nTablaValores)
        Dim aCampos() As String = {"ENBARRA", "Valor", ""}
        Dim aNombres() As String = {"Código en Barra", "Valor Monetario", ""}
        Dim aAnchos() As Long = {75, 100, 300}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {"", sFormatoNumero, ""}
        IniciarTabla(dg, dtValores, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtValores.Rows.Count > 0 Then nPosicionVal = 0

        FindField = "enbarra"
        BindingSource1.DataSource = dtValores
        BindingSource1.Filter = " enbarra like '" & txtBuscar.Text & "%'"
        dg.DataSource = BindingSource1


    End Sub
    Private Sub IniciarCorredor(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = AutoCodigo(5, ds, nTabla, "codigo")
        Else
            txtCodigo.Text = ""
        End If
        IniciarTextoObjetos(FormatoItemListView.iCadena, txtNombre, txtNombre1, txtProveedor, txtGrupoNombre, txtSubgrupoNombre, txtCodigoContable)
        IniciarTextoObjetos(FormatoItemListView.iEntero, txtLongitudBarra, txtLongitudPrecio, txtLongitudTipo, txtInicioPrecio, txtInicioTipo)
        IniciarTextoObjetos(FormatoItemListView.iNumero, txtCargos)
        RellenaComboConDatatable(cmbIVA, dtIVA, "monto", "tipo", 0)
        If dtTipos.Rows.Count > 0 Then dgCom.Columns.Clear()
        'Movimientos
        txtCodigo1.Text = ""
        txtNombre1.Text = ""
        Proveedor = ""
        Grupo = ""
        SubGrupo = ""

        If dtValores.Rows.Count > 0 Then dg.Columns.Clear()

    End Sub
    Private Sub ActivarMarco0()
        grpAceptarSalir.Visible = True
        C1DockingTabPage2.Enabled = False
        HabilitarObjetos(True, True, txtNombre, txtLongitudBarra, txtInicioPrecio, txtLongitudPrecio, txtInicioTipo, _
                         txtLongitudTipo, txtCargos, btnProveedor, btnGrupo, btnCodigoContable, cmbIVA)

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
    End Sub
    Private Sub DesactivarMarco0()
        grpAceptarSalir.Visible = False
        C1DockingTabPage2.Enabled = True

        HabilitarObjetos(False, True, txtCodigo, txtCodigo1, txtNombre, txtNombre1, txtLongitudBarra, txtLongitudPrecio, txtLongitudTipo, _
            txtInicioPrecio, txtInicioTipo, txtCargos, txtGrupoNombre, txtSubgrupoNombre, txtCodigoContable, txtProveedor, _
            btnProveedor, btnCodigoContable, btnGrupo, cmbIVA)

        MenuBarra.Enabled = True
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
    End Sub
    Private Sub btnIngreso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCodigoContable.Click
        txtCodigoContable.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by codcon ", "Cuentas Contables", txtCodigoContable.Text)
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        If tbcBancos.SelectedTab.Text = "Corredores" Then
            Me.BindingContext(ds, nTabla).Position = 0
            AsignaTXT(Me.BindingContext(ds, nTabla).Position)
        Else
            Me.BindingContext(ds, nTablaValores).Position = 0
            AsignaMov(Me.BindingContext(ds, nTablaValores).Position, False)
        End If
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        If tbcBancos.SelectedTab.Text = "Corredores" Then
            Me.BindingContext(ds, nTabla).Position -= 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position)
        Else
            Me.BindingContext(ds, nTablaValores).Position -= 1
            AsignaMov(Me.BindingContext(ds, nTablaValores).Position, False)
        End If
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        If tbcBancos.SelectedTab.Text = "Corredores" Then
            Me.BindingContext(ds, nTabla).Position += 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position)
        Else
            Me.BindingContext(ds, nTablaValores).Position += 1
            AsignaMov(Me.BindingContext(ds, nTablaValores).Position, False)
        End If

    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        If tbcBancos.SelectedTab.Text = "Corredores" Then
            Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position)
        Else
            Me.BindingContext(ds, nTablaValores).Position = ds.Tables(nTablaValores).Rows.Count - 1
            AsignaMov(Me.BindingContext(ds, nTablaValores).Position, False)
        End If
    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        If tbcBancos.SelectedTab.Text = "Corredores" Then
            i_modo = movimiento.iAgregar
            nPosicionCat = Me.BindingContext(ds, nTabla).Position
            ActivarMarco0()
            IniciarCorredor(True)
        Else
            If Trim(txtCodigo.Text) <> "" Then
                Dim f As New jsControlArcCorredoresCestaTicketMovimientos
                f.Agregar(myConn, ds, dtValores, txtCodigo.Text)
                ds = DataSetRequery(ds, strSQLValores, myConn, nTablaValores, lblInfo)
                If f.Apuntador >= 0 Then AsignaMov(f.Apuntador, True)
                f = Nothing
            End If
        End If

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        If tbcBancos.SelectedTab.Text = "Corredores" Then
            i_modo = movimiento.iEditar
            nPosicionCat = Me.BindingContext(ds, nTabla).Position
            ActivarMarco0()
        Else

            If dtValores.Rows.Count > 0 AndAlso dg.RowCount > 0 Then

                Dim row As DataRow = dtValores.Select(" CODIGO = '" & txtCodigo.Text _
                                                      & "' AND ENBARRA = '" & dg.SelectedRows(0).Cells(0).Value.ToString _
                                                      & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
                nPosicionVal = dtValores.Rows.IndexOf(row)

                Dim f As New jsControlArcCorredoresCestaTicketMovimientos
                f.Apuntador = nPosicionVal
                f.Editar(myConn, ds, dtValores, txtCodigo.Text)
                txtBuscar.Text = ""
                ds = DataSetRequery(ds, strSQLValores, myConn, nTablaValores, lblInfo)
                AsignaMov(f.Apuntador, True)
                f = Nothing
            End If
        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If tbcBancos.SelectedTab.Text = "Corredores" Then
            EliminaCorredor()
        Else
            EliminarMovimiento()
        End If
    End Sub
    Private Sub EliminaCorredor()
        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        Dim aCamposDel() As String = {"codigo", "id_emp"}
        Dim aStringsDel() As String = {txtCodigo.Text, jytsistema.WorkID}
        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
        If sRespuesta = MsgBoxResult.Yes Then
            If dtValores.Rows.Count = 0 Then
                'ELIMINAR JSVENCESTIP Y JSVENVALTIC
                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsvencestic", strSQL, aCamposDel, aStringsDel, _
                                              Me.BindingContext(ds, nTabla).Position, True))
            Else
                MensajeCritico(lblInfo, "Este CORREDOR posee movimientos. Verifique por favor ...")
            End If
        End If

    End Sub
    Private Sub EliminarMovimiento()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionVal = Me.BindingContext(ds, nTablaValores).Position

        If nPosicionVal >= 0 Then
            sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
            If sRespuesta = MsgBoxResult.Yes Then
                With dtValores.Rows(nPosicionVal)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("codigo") & .Item("enbarra"))

                    Dim aCam() As String = {"codigo", "enbarra", "id_emp"}
                    Dim aStr() As String = {txtCodigo.Text, .Item("enbarra"), jytsistema.WorkID}
                    nPosicionVal = EliminarRegistros(myConn, lblInfo, ds, nTablaValores, "jsvenvaltic", strSQLValores, _
                                                     aCam, aStr, nPosicionVal)
                    AsignaTXT(nPosicionCat)
                    AsignaMov(nPosicionVal, False)
                End With
            End If
        End If

    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar

        If tbcBancos.SelectedTab.Text = "Corredores" Then
            'Dim Campos() As String = {"codban", "nomban"}
            'Dim Nombres() As String = {"Código Banco", "Nombre Banco"}
            'Dim Anchos() As Long = {100, 2500}
            'f.Text = "Bancos"
            'f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position)
            'AsignaTXT(f.Apuntador)
            'f = Nothing
        Else
            'Dim Campos() As String = {"fechamov", "numdoc", "concepto", "nombre"}
            'Dim Nombres() As String = {"Emisión", "Nº Movimiento", "Concepto", "Cliente ó Proveedor"}
            'Dim Anchos() As Long = {100, 120, 2450, 2450}
            'f.Text = "Movimientos bancarios"
            'f.Buscar(dtValores, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaValores).Position)
            'AsignaMov(f.Apuntador, False)
            'f = Nothing
        End If

    End Sub


    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        'If tbcBancos.SelectedTab.Text = "Bancos" Then
        '    Dim f As New jsBanRepParametros
        '    f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cFichaBanco, "Ficha de Banco", txtCodigo.Text)
        '    f = Nothing
        'Else
        '    Dim f As New jsBanRepParametros
        '    If dtValores.Rows(nPosicionTip).Item("TIPOMOV") = "CH" Then
        '        ' f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cComprobanteDeEgreso, "COMPROBANTE DE PAGO", txtCodigo1.Text, dtValores.Rows(nPosicionTip).Item("COMPROBA"))
        '        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cCheque, "CHEQUE", txtCodigo1.Text, dtValores.Rows(nPosicionTip).Item("COMPROBA"))
        '    Else
        '        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cMovimientoBanco, "Movimientos banco", txtCodigo1.Text)
        '    End If

        '    f = Nothing
        'End If

    End Sub

    Private Function Validado() As Boolean

        If txtNombre.Text = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar un nombre válido...")
            Return False
        End If

        If Proveedor = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar/seleccionar un proveedor válido")
            Return False
        End If

        If Grupo = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar un Grupo/Subgrupo válido...")
            Return False
        End If

        Validado = True

    End Function
    Private Sub GuardarTXT()

        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionCat = ds.Tables(nTabla).Rows.Count
        End If

        InsertEditCONTROLCestaTicket(myConn, lblInfo, Inserta, txtCodigo.Text, txtNombre.Text, 0.0, 0.0, ValorEntero(txtLongitudBarra.Text), _
                                      CInt(txtInicioPrecio.Text), CInt(txtLongitudPrecio.Text), CInt(txtInicioTipo.Text), CInt(txtLongitudTipo.Text), _
                                      CDbl(txtCargos.Text), cmbIVA.SelectedValue, txtCodigoContable.Text, Proveedor, _
                                      Grupo, SubGrupo)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" codigo = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionCat = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionCat
        AsignaTXT(nPosicionCat)
        DesactivarMarco0()
        tbcBancos.SelectedTab = C1DockingTabPage1
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, _
        txtNombre.GotFocus, txtLongitudBarra.GotFocus, txtLongitudPrecio.GotFocus, txtLongitudTipo.GotFocus, _
        txtInicioPrecio.GotFocus, txtInicioTipo.GotFocus, txtCargos.GotFocus, btnProveedor.GotFocus, btnGrupo.GotFocus, _
        btnCodigoContable.GotFocus, cmbIVA.GotFocus
        Select Case sender.name
            Case "txtCodigo"
                MensajeEtiqueta(lblInfo, " Indique el código corredor de cheques de alimentación ... ", TipoMensaje.iInfo)
            Case "txtNombre"
                MensajeEtiqueta(lblInfo, " Indique el nombre corredor de cheques de alimentacion ... ", TipoMensaje.iInfo)
            Case "txtLongitudBarra"
                MensajeEtiqueta(lblInfo, " Indique la longitud del código de barra del ticket ... ", TipoMensaje.iInfo)
            Case "txtLongitudPrecio"
                MensajeEtiqueta(lblInfo, " Indique la longitud de precio en el código de barra ... ", TipoMensaje.iInfo)
            Case "txtLongitudTipo"
                MensajeEtiqueta(lblInfo, " Indique la longitud del tipo en el código de barra  ... ", TipoMensaje.iInfo)
            Case "txtInicioPrecio"
                MensajeEtiqueta(lblInfo, " Indique la posición inicio del precio en el código de barras ... ", TipoMensaje.iInfo)
            Case "txtInicioTipo"
                MensajeEtiqueta(lblInfo, " Indique la posición inicio del tipo de ticket en el código de barras ... ", TipoMensaje.iInfo)
            Case "txtCargos"
                MensajeEtiqueta(lblInfo, " Indique el monto por cargos sobre la valija de tickets ... ", TipoMensaje.iInfo)
            Case "btnProveedor"
                MensajeEtiqueta(lblInfo, " Seleccione Proveedor asociado con el gasto(Cargo) de valija ... ", TipoMensaje.iInfo)
            Case "btnGrupo"
                MensajeEtiqueta(lblInfo, " Seleccione el grupo y subgrupo de gastos ... ", TipoMensaje.iInfo)
            Case "btnCodigoContable"
                MensajeEtiqueta(lblInfo, " Seleccione el código contable de este gasto ... ", TipoMensaje.iInfo)
            Case "cmbIVA"
                MensajeEtiqueta(lblInfo, " Seleccione el porcentaje de IVA para este gasto ... ", TipoMensaje.iInfo)

        End Select


    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
        dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTablaValores).Position = e.RowIndex
        nPosicionTip = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", e.RowIndex, ds.Tables(nTablaValores).Rows.Count)
    End Sub

    Private Sub tbcBancos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcBancos.SelectedIndexChanged
        If tbcBancos.SelectedIndex = 0 Then
            nPosicionTip = Me.BindingContext(ds, nTablaValores).Position
            AsignaTXT(nPosicionCat)
        Else 'movimientos
            nPosicionCat = Me.BindingContext(ds, nTabla).Position
            AbrirMovimientos(txtCodigo1.Text)
            AsignaMov(nPosicionTip, True)
            dg.Enabled = True
        End If
    End Sub


    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If dt.Rows.Count = 0 Then
            IniciarCorredor(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            If Me.BindingContext(ds, nTabla).Position > 0 Then _
                nPosicionCat = Me.BindingContext(ds, nTabla).Position

            AsignaTXT(nPosicionCat)
        End If
        DesactivarMarco0()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub

    Private Sub btnGrupo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrupo.Click
        Dim f As New jsComArcGrupoSubgrupo
        f.Cargar(myConn, TipoCargaFormulario.iShowDialog)
        Grupo = f.Grupo0 : SubGrupo = f.Grupo1
        If Grupo <> "" Then txtGrupoNombre.Text = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select nombre from jsprogrugas where codigo = '" & Grupo & "' and id_emp = '" & jytsistema.WorkID & "' "))
        If SubGrupo <> "" Then txtSubgrupoNombre.Text = CStr(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select nombre from jsprogrugas where codigo = '" & SubGrupo & "' and id_emp = '" & jytsistema.WorkID & "' "))

        f.Dispose()
        f = Nothing
    End Sub

    
    Private Sub btnAgregaTarjeta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaTipo.Click
        Dim g As New jsControlArcCorredoresCestaTicketMovimientosTipo
        g.Agregar(myConn, ds, dtTipos, txtCodigo.Text)
        AsignaTXT(nPosicionCat)
        If g.Apuntador >= 0 Then AsignaTipo(g.Apuntador, True)
        g = Nothing

    End Sub

    Private Sub btnEditaTarjeta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditaTipo.Click
        Dim g As New jsControlArcCorredoresCestaTicketMovimientosTipo
        g.Editar(myConn, ds, dtTipos, txtCodigo.Text)
        AsignaTXT(nPosicionCat)
        If g.Apuntador >= 0 Then AsignaTipo(g.Apuntador, True)
        g = Nothing
    End Sub

    Private Sub btnEliminaTarjeta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaTipo.Click
        With dtTipos
            If .Rows.Count > 0 Then
                nPosicionTip = Me.BindingContext(ds, nTablaTipos).Position

                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                Dim aCamDel() As String = {"corredor", "tipo", "id_emp"}
                Dim aStrDel() As String = {txtCodigo.Text, .Rows(nPosicionTip).Item("tipo").ToString, jytsistema.WorkID}
                sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                If sRespuesta = MsgBoxResult.Yes Then

                    AsignaTipo(EliminarRegistros(myConn, lblInfo, ds, nTablaTipos, "jsvencestip", _
                                                strSQLTipos, aCamDel, aStrDel, nPosicionTip), True)

                End If
            End If
        End With

    End Sub

    Private Sub dgCom_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgCom.RowHeaderMouseClick, _
        dgCom.CellMouseClick, dgCom.RegionChanged
        Me.BindingContext(ds, nTablaTipos).Position = e.RowIndex
    End Sub


    Private Sub txtLongitudBarra_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtLongitudBarra.KeyPress, _
        txtInicioPrecio.KeyPress, txtLongitudPrecio.KeyPress, txtInicioTipo.KeyPress, txtLongitudTipo.KeyPress, _
        txtCargos.KeyPress

        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub

    Private Sub btnProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnProveedor.Click

        Proveedor = CargarTablaSimple(myConn, lblInfo, ds, " select codpro codigo, nombre descripcion from jsprocatpro where id_emp = '" & jytsistema.WorkID & "' order by codpro ", "Proveedores", Proveedor)
        txtProveedor.Text = EjecutarSTRSQL_Scalar(myConn, lblInfo, " select nombre from jsprocatpro where codpro = '" & Proveedor & "' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub


    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        If tbcBancos.SelectedTab.Text = "Corredores" Then
            
        Else
            BindingSource1.DataSource = dtValores
            If dtValores.Columns(FindField).DataType Is GetType(String) Then _
                BindingSource1.Filter = FindField & " like '%" & txtBuscar.Text & "%'"
            dg.DataSource = BindingSource1

        End If
    End Sub

    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.ColumnHeaderMouseClick
        FindField = dtValores.Columns(e.ColumnIndex).ColumnName
        lblBuscar.Text = dg.Columns(e.ColumnIndex).HeaderText
    End Sub

    Private Sub dg_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub

    Private Sub txtBuscar_Click(sender As System.Object, e As System.EventArgs) Handles txtBuscar.Click

    End Sub
End Class