Imports MySql.Data.MySqlClient
Imports ReportesDeBancos
Public Class jsBanArcBancos
    Private Const sModulo As String = "Bancos"
    Private Const lRegion As String = "RibbonButton9"
    Private Const nTabla As String = "bancos"
    Private Const nTablaMovimientos As String = "movimientos"
    Private Const nTablaTarjetas As String = "tarjetasbanco"

    Private strSQL As String = "select * from jsbancatban where id_emp = '" & jytsistema.WorkID & "' order by codban"
    Private strSQLMov As String
    Private strSQLTar As String

    Private myConn As New MySqlConnection(jytsistema.strConn)
    
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtMovimientos As New DataTable
    Private dtTarjetas As New DataTable

    Private aCondicion() As String = {"Inactivo", "Activo"}
    Private aTitulos() As String = {"Sr.", "Sra.", "Srta.", "Lic.", "Dr.", "Ing.", "Ab.", "Arq.", "Otr."}

    Private i_modo As Integer
    Private nPosicionCat As Long, nPosicionMov As Long, nPosicionTar As Long
    Private CredentialCheck As Boolean = True

    Private Sub jsBanArcBancos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

    End Sub

    Private Sub jsBanBancos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo



        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)



            If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) > 0 Then
                txtSaldo.Visible = True
                txtSaldo1.Visible = True
                lblDisponible.Visible = True
                lblDisponibleMov.Visible = True
            Else
                txtSaldo.Visible = False
                txtSaldo1.Visible = False
                lblDisponible.Visible = False
                lblDisponibleMov.Visible = False
            End If

            RellenaCombo(aCondicion, cmbCondicion)
            RellenaCombo(aTitulos, cmbTitulo)
            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaTXT(nPosicionCat)
            Else
                IniciarBanco(False)
            End If
            ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            AsignarTooltips()
            IniciarCajasyCestatickets()
            HabilitarBotonesAdicionalesBarra(False)
        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub HabilitarBotonesAdicionalesBarra(ByVal Habilitar As Boolean)
        HabilitarObjetos(Habilitar, False, btnDepositarEfectivo, btnDepositarTarjetas, btnDepositarCestaTicket, btnReposicion)
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

        'Adicionbales menu barra
        C1SuperTooltip1.SetToolTip(btnDepositarCestaTicket, "Depositar remesas de <B>cheques de alimentación</B>")
        C1SuperTooltip1.SetToolTip(btnDepositarEfectivo, "Depositar <B>efectivo y/o cheques</B>")
        C1SuperTooltip1.SetToolTip(btnDepositarTarjetas, "Depositar <B>tarjetas</B> de crédito y/o débito")
        C1SuperTooltip1.SetToolTip(btnReposicion, "<B>Reposición</B> de saldo en cajas")

        'Botones Adicionales
        C1SuperTooltip1.SetToolTip(btnIngreso, "Seleccione la fecha de ingreso ó<br> la fecha de apertura/activación de la cuenta")
        C1SuperTooltip1.SetToolTip(btnFormatos, "Seleccione el <B>formato</B> para impresión de cheque en el voucer de pago")


    End Sub
    Private Sub IniciarCajasyCestatickets()

        Dim iCont As Integer
        Dim dtCajas As New DataTable
        Dim nTablaCajas As String = "cajas"
        ds = DataSetRequery(ds, " select * from jsbanenccaj where id_emp = '" & jytsistema.WorkID & "' order by caja ", myConn, nTablaCajas, lblInfo)
        dtCajas = ds.Tables(nTablaCajas)
        For iCont = 0 To dtCajas.Rows.Count - 1
            Dim tsCPE As New ToolStripMenuItem(dtCajas.Rows(iCont).Item("caja").ToString & " | " & dtCajas.Rows(iCont).Item("nomcaja").ToString, Nothing, New EventHandler(AddressOf btnDepositarEfectivo_Click))
            btnDepositarEfectivo.DropDownItems.Add(tsCPE)
            Dim tsCPT As New ToolStripMenuItem(dtCajas.Rows(iCont).Item("caja").ToString & " | " & dtCajas.Rows(iCont).Item("nomcaja").ToString, Nothing, New EventHandler(AddressOf btnDepositarEfectivo_Click))
            btnDepositarTarjetas.DropDownItems.Add(tsCPT)
            Dim tsCPR As New ToolStripMenuItem(dtCajas.Rows(iCont).Item("caja").ToString & " | " & dtCajas.Rows(iCont).Item("nomcaja").ToString, Nothing, New EventHandler(AddressOf btnDepositarEfectivo_Click))
            btnReposicion.DropDownItems.Add(tsCPR)
        Next

        CType(btnDepositarEfectivo.DropDown, ToolStripDropDownMenu).ShowImageMargin = False
        CType(btnDepositarTarjetas.DropDown, ToolStripDropDownMenu).ShowImageMargin = False
        CType(btnReposicion.DropDown, ToolStripDropDownMenu).ShowImageMargin = False

        Dim dtCorredores As New DataTable
        Dim nTablaCorredores As String = "corredores"
        ds = DataSetRequery(ds, "select * from jsvencestic where id_emp = '" & jytsistema.WorkID & "' order by codigo ", myConn, nTablaCorredores, lblInfo)
        dtCorredores = ds.Tables(nTablaCorredores)
        For iCont = 0 To dtCorredores.Rows.Count - 1
            Dim tsCT As New ToolStripMenuItem(dtCorredores.Rows(iCont).Item("codigo").ToString & " | " & dtCorredores.Rows(iCont).Item("descrip").ToString, Nothing, New EventHandler(AddressOf btnDepositarCestaTicket_Click))
            btnDepositarCestaTicket.DropDownItems.Add(tsCT)
        Next
        CType(btnDepositarCestaTicket.DropDown, ToolStripDropDownMenu).ShowImageMargin = False

    End Sub
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)

        If c >= 0 AndAlso dtMovimientos.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaMovimientos).Position = c
            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", c, dtMovimientos.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If
        txtSaldo.Text = FormatoNumero(CalculaSaldoBanco(myConn, lblInfo, txtCodigo.Text))
        txtSaldo1.Text = txtSaldo.Text

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtMovimientos, MenuBarra)
    End Sub
    Private Sub AsignaTar(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLTar, myConn, nTablaTarjetas, lblInfo)

        If c >= 0 AndAlso dtTarjetas.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaTarjetas).Position = c
            dgTar.Refresh()
            dgTar.CurrentCell = dgTar(0, c)
        End If

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        If dt.Rows.Count > 0 Then
            With dt

                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

                With .Rows(nRow)
                    'Bancos 
                    nPosicionCat = nRow

                    txtCodigo.Text = .Item("codban")
                    txtNombre.Text = IIf(IsDBNull(.Item("nomban")), "", .Item("nomban"))
                    txtCtaBan.Text = IIf(IsDBNull(.Item("ctaban")), "", .Item("ctaban"))
                    txtAgencia.Text = IIf(IsDBNull(.Item("agencia")), "", .Item("agencia"))
                    txtDireccion.Text = IIf(IsDBNull(.Item("direccion")), "", .Item("direccion"))
                    txtTelef1.Text = IIf(IsDBNull(.Item("telef1")), "", .Item("telef1"))
                    txtTelef2.Text = IIf(IsDBNull(.Item("telef2")), "", .Item("telef2"))
                    txtFax.Text = IIf(IsDBNull(.Item("fax")), "", .Item("fax"))
                    txtEmail.Text = IIf(IsDBNull(.Item("email")), "", .Item("email"))
                    txtCuentaContable.Text = IIf(IsDBNull(.Item("codcon")), "", .Item("codcon"))
                    txtContacto.Text = IIf(IsDBNull(.Item("contacto")), "", .Item("contacto"))
                    txtComision.Text = IIf(IsDBNull(.Item("comision")), FormatoNumero(0.0), FormatoNumero(.Item("comision")))
                    txtIngreso.Text = IIf(IsDBNull(.Item("fechacrea").ToString), "", FormatoFechaMedia(.Item("fechacrea").ToString))
                    txtSaldo.Text = FormatoNumero(CalculaSaldoBanco(myConn, lblInfo, .Item("codban")))
                    'txtSaldo.Text = FormatoNumero(.Item("saldoact"))
                    txtFormato.Text = .Item("formato")
                    cmbCondicion.SelectedIndex = .Item("estatus")
                    cmbTitulo.SelectedIndex = IIf(InArray(aTitulos, .Item("titulo")) - 1 <= 0, 0, InArray(aTitulos, .Item("titulo")) - 1)

                    'Movimientos
                    txtCodigo1.Text = .Item("codban")
                    txtNombre1.Text = IIf(IsDBNull(.Item("nomban")), "", .Item("nomban"))
                    txtCtaBan1.Text = IIf(IsDBNull(.Item("ctaban")), "", .Item("ctaban"))
                    txtSaldo1.Text = txtSaldo.Text

                    strSQLTar = " select a.codtar, b.nomtar, a.com1, a.com2 from jsbancatbantar a " _
                        & " left join jsconctatar b on (a.codtar = b.codtar and a.id_emp = b.id_emp ) " _
                        & " where a.codban = '" & .Item("codban") & "' and a.id_emp = '" & jytsistema.WorkID & "' "
                    ds = DataSetRequery(ds, strSQLTar, myConn, nTablaTarjetas, lblInfo)
                    dtTarjetas = ds.Tables(nTablaTarjetas)

                    Dim aCamTar() As String = {"codtar", "nomtar", "com1", "com2"}
                    Dim aNomTar() As String = {"Código", "Nombre", "Comisión", "ISLR"}
                    Dim aAncTar() As Long = {75, 250, 80, 80}
                    Dim aAliTar() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}
                    Dim aForTar() As String = {"", "", sFormatoNumero, sFormatoNumero}

                    IniciarTabla(dgTar, dtTarjetas, aCamTar, aNomTar, aAncTar, aAliTar, aForTar)
                    If dtTarjetas.Rows.Count > 0 Then nPosicionTar = 0

                End With
            End With

            ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            HabilitarBotonesAdicionalesBarra(False)
        End If

    End Sub
    Private Sub AbrirMovimientos(ByVal CodigoBanco As String)

        strSQLMov = "select a.fechamov, a.tipomov, a.numdoc, a.concepto, a.importe, a.origen, a.benefic, a.prov_cli, '' nombre,  a.codven, " _
                          & " a.numorg, a.tiporg, a.codban, a.caja, a.comproba, a.multican " _
                            & " from jsbantraban a " _
                            & " where " _
                            & " a.codban  = '" & CodigoBanco & "' and " _
                            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " order by a.fechamov desc, a.tipomov, a.numdoc "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
        dtMovimientos = ds.Tables(nTablaMovimientos)
        Dim aCampos() As String = {"fechamov", "tipomov", "numdoc", "concepto", "importe", "origen", "prov_cli", "nombre", "codven"}
        Dim aNombres() As String = {"Emisión", "TP", "Documento", "Concepto", "Importe", "ORG", "Prov/Cli", "Nombre o Razón Social", "Asesor"}
        Dim aAnchos() As Long = {80, 25, 100, 300, 95, 30, 80, 300, 50}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, _
            AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, _
            AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aFormatos() As String = {sFormatoFecha, "", "", "", sFormatoNumero, "", "", "", ""}
        IniciarTabla(dg, dtMovimientos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtMovimientos.Rows.Count > 0 Then nPosicionMov = 0

    End Sub
    Private Sub IniciarBanco(ByVal Inicio As Boolean)

        If Inicio Then
            Dim aWherefields() As String = {"id_emp"}
            Dim aWhereValues() As String = {jytsistema.WorkID}
            txtCodigo.Text = AutoCodigoXPlus(myConn, "codban", "jsbancatban", aWherefields, aWhereValues, 5, True)  '  AutoCodigo(5, ds, nTabla, "codban")
        Else
            txtCodigo.Text = ""
        End If
        IniciarTextoObjetos(FormatoItemListView.iCadena, txtNombre, txtCtaBan, txtAgencia, txtDireccion, txtTelef1, _
                            txtTelef2, txtFax, txtEmail, txtWeb, txtContacto, txtComision, txtFormato, txtCodigo1, _
                            txtNombre1, txtCtaBan1, txtCuentaContable)

        IniciarTextoObjetos(FormatoItemListView.iNumero, txtSaldo, txtSaldo1)
        txtIngreso.Text = FormatoFechaMedia(jytsistema.sFechadeTrabajo)
        txtFormato.Text = "01"
        cmbCondicion.SelectedIndex = 1

        'Movimientos
        dg.Columns.Clear()

    End Sub
    Private Sub ActivarMarco0()
        Dim c As Control
        Dim i As Integer
        grpAceptarSalir.Visible = True

        C1DockingTabPage2.Enabled = False

        For i = 0 To 1
            For Each c In tbcBancos.TabPages(i).Controls
                If i < 1 Then
                    If c.Name = "txtCodigo" Or c.Name = "txtIngreso" Or _
                        c.Name = "txtSaldo" Or c.Name = "txtFormato" Then
                        c.Enabled = False
                    Else
                        c.Enabled = True
                        If c.GetType Is txtCodigo.GetType Then
                            c.BackColor = Color.White
                        Else
                            If c.GetType Is cmbCondicion.GetType Then
                                c.BackColor = Color.White
                            End If
                        End If
                    End If
                End If
            Next
        Next

        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
    End Sub
    Private Sub DesactivarMarco0()
        Dim c As Control
        Dim i As Integer
        grpAceptarSalir.Visible = False

        C1DockingTabPage2.Enabled = True
        For i = 0 To 1
            For Each c In tbcBancos.TabPages(i).Controls
                c.Enabled = False
                If c.GetType Is txtCodigo.GetType Then
                    c.BackColor = Color.AliceBlue
                Else
                    If c.GetType Is cmbCondicion.GetType Then
                        c.BackColor = Color.AliceBlue
                    End If
                End If
            Next

        Next
        MenuBarra.Enabled = True
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
    End Sub
    Private Sub btnIngreso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngreso.Click
        txtIngreso.Text = SeleccionaFecha(CDate(txtIngreso.Text), Me, btnIngreso)
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        If tbcBancos.SelectedTab.Text = "Bancos" Then
            Me.BindingContext(ds, nTabla).Position = 0
            AsignaTXT(Me.BindingContext(ds, nTabla).Position)
        Else
            Me.BindingContext(ds, nTablaMovimientos).Position = 0
            AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End If
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        If tbcBancos.SelectedTab.Text = "Bancos" Then
            Me.BindingContext(ds, nTabla).Position -= 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position)
        Else
            Me.BindingContext(ds, nTablaMovimientos).Position -= 1
            AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End If
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        If tbcBancos.SelectedTab.Text = "Bancos" Then
            Me.BindingContext(ds, nTabla).Position += 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position)
        Else
            Me.BindingContext(ds, nTablaMovimientos).Position += 1
            AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End If

    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        If tbcBancos.SelectedTab.Text = "Bancos" Then
            Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position)
        Else
            Me.BindingContext(ds, nTablaMovimientos).Position = ds.Tables(nTablaMovimientos).Rows.Count - 1
            AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End If
    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        If tbcBancos.SelectedTab.Text = "Bancos" Then
            i_modo = movimiento.iAgregar
            nPosicionCat = Me.BindingContext(ds, nTabla).Position
            ActivarMarco0()
            IniciarBanco(True)
        Else
            If Trim(txtCodigo.Text) <> "" Then
                Dim f As New jsBanArcBancosMovimientosPlus
                f.Agregar(myConn, ds, dtMovimientos, txtCodigo.Text)
                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                AsignaTXT(nPosicionCat)
                If f.Apuntador >= 0 Then AsignaMov(f.Apuntador, True)
                f = Nothing
            End If
        End If

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        If tbcBancos.SelectedTab.Text = "Bancos" Then
            i_modo = movimiento.iEditar
            nPosicionCat = Me.BindingContext(ds, nTabla).Position
            ActivarMarco0()
        Else
            If dtMovimientos.Rows(Me.BindingContext(ds, nTablaMovimientos).Position).Item("origen") = "BAN" Then
                Dim f As New jsBanArcBancosMovimientosPlus
                f.Apuntador = Me.BindingContext(ds, nTablaMovimientos).Position
                f.Editar(myConn, ds, dtMovimientos, txtCodigo.Text)
                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                AsignaTXT(nPosicionCat)
                AsignaMov(f.Apuntador, True)
                f = Nothing
            End If

        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If tbcBancos.SelectedTab.Text = "Bancos" Then
            EliminaBanco()
        Else
            If cmbCondicion.Text = "Activo" Then
                EliminarMovimiento()
            Else
                MensajeCritico(lblInfo, "Esta Cuenta - Banco está Inactiva ....")
            End If
        End If
    End Sub
    Private Sub EliminaBanco()
        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        Dim aCamposDel() As String = {"codban", "id_emp"}
        Dim aStringsDel() As String = {txtCodigo.Text, jytsistema.WorkID}
        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
        If sRespuesta = MsgBoxResult.Yes Then
            If dtMovimientos.Rows.Count = 0 Then

                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsbancatban", strSQL, aCamposDel, aStringsDel, _
                                              Me.BindingContext(ds, nTabla).Position, True))

            Else
                MensajeCritico(lblInfo, "Este banco posee movimientos. Verifique por favor ...")
            End If
        End If

    End Sub
    Private Sub EliminarMovimiento()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        Dim NumeroOrigen As String
        Dim numComproba As String = ""
        Dim TipoOrigen As String
        Dim Tipo As String
        Dim nPosicionMovi As Long

        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        nPosicionMovi = nPosicionMov

        If nPosicionMov >= 0 Then
            With dtMovimientos.Rows(nPosicionMov)
                If .Item("origen") = "BAN" Then
                    sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                    If sRespuesta = MsgBoxResult.Yes Then

                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("numdoc"))
                        NumeroOrigen = IIf(IsDBNull(.Item("numorg")), "", .Item("numorg"))
                        TipoOrigen = IIf(IsDBNull(.Item("tiporg")), "", .Item("tiporg"))
                        Tipo = IIf(IsDBNull(.Item("tipomov")), "", .Item("tipomov"))
                        numComproba = IIf(IsDBNull(.Item("comproba")), "", .Item("comproba"))


                        If NumeroOrigen.Substring(0, 2) = "TB" Then
                            Dim sResp As Microsoft.VisualBasic.MsgBoxResult = MsgBox("¿ MOVIMIENTO PROVIENE DE UNA TRANSFERENCIA BANCARIA, POR LO TANTO SE ELIMINARA EN TODOS LOS BANCOS. DESEA CONTINUAR ?", MsgBoxStyle.YesNo, "TRANSFERENCIA... ")
                            If sResp = MsgBoxResult.Yes Then

                                EjecutarSTRSQL(myConn, lblInfo, "DELETE from jsbantraban where " _
                                    & " NUMDOC = '" & .Item("numdoc") & "' and " _
                                    & " NUMORG = '" & NumeroOrigen & "' and " _
                                    & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
                                    & " ID_EMP = '" & jytsistema.WorkID & "'")

                                EjecutarSTRSQL(myConn, lblInfo, " delete from jsbanordpag where " _
                                    & " comproba = '" & numComproba & "' and  " _
                                    & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                    & " ID_EMP = '" & jytsistema.WorkID & "' ")

                            End If
                        End If



                        EjecutarSTRSQL(myConn, lblInfo, "DELETE FROM jsbantracaj where " _
                            & " REFPAG = '" & .Item("codban") & "' AND " _
                            & " FECHA = '" & FormatoFechaMySQL(CDate(.Item("fechamov").ToString)) & "' AND " _
                            & " NUMMOV = '" & .Item("numdoc") & "' AND " _
                            & " TIPOMOV = 'EN' AND FORMPAG = '" & Tipo & "' AND" _
                            & " NUMPAG = '" & .Item("numdoc") & "' AND " _
                            & " IMPORTE = " & -1 * .Item("importe") & " AND " _
                            & " CAJA = '" & .Item("caja") & "' AND " _
                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'")

                        EjecutarSTRSQL(myConn, lblInfo, "DELETE from jsbantraban where " _
                            & " CODBAN = '" & .Item("codban") & "' and " _
                            & " FECHAMOV = '" & FormatoFechaMySQL(CDate(.Item("fechamov").ToString)) & "' AND " _
                            & " NUMDOC = '" & .Item("numdoc") & "' and " _
                            & " NUMORG = '" & .Item("numorg") & "' and " _
                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'")

                        If Tipo = "CH" Then _
                            EjecutarSTRSQL(myConn, lblInfo, " delete from jsbanordpag where " _
                            & " comproba = '" & numComproba & "' and  " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " ID_EMP = '" & jytsistema.WorkID & "' ")

                        If TipoOrigen = "CH" And Tipo = "ND" Then


                            EjecutarSTRSQL(myConn, lblInfo, "Delete from jsventracob where " _
                               & " TIPOMOV = 'ND' AND " _
                               & " NUMORG = '" & NumeroOrigen & "' AND " _
                               & " ORIGEN = 'BAN' AND " _
                               & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
                               & " ID_EMP = '" & jytsistema.WorkID & "'")

                            EjecutarSTRSQL(myConn, lblInfo, "DELETE FROM jsbanchedev where " _
                               & " NUMCHEQUE = '" & NumeroOrigen & "' AND " _
                               & " ID_EMP = '" & jytsistema.WorkID & "' ")
                        End If

                        If numComproba.Substring(0, 2) = "CP" Then
                            EjecutarSTRSQL(myConn, lblInfo, " delete from jsbantracaj where tipomov = 'EN' and deposito = '" & numComproba & "' and id_emp = '" & jytsistema.WorkID & "' ")
                            EjecutarSTRSQL(myConn, lblInfo, " update jsbantracaj set deposito = '' where tipomov = 'SA' and deposito = '" & numComproba & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        End If

                        EjecutarSTRSQL(myConn, lblInfo, " update jsbanenccaj set saldo = " & CalculaSaldoCajaPorFP(myConn, .Item("caja"), "", lblInfo) & " where caja = '" & .Item("caja") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        EjecutarSTRSQL(myConn, lblInfo, " update jsbancatban set saldoact = " & CalculaSaldoBanco(myConn, lblInfo, txtCodigo1.Text) & " where codban = '" & txtCodigo1.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                        'EjecutarstrSQL(" call saldosbancomensuales('" & txtCodigo1.Text & "','" & jytsistema.WorkExercise & "','" & jytsistema.WorkID & "') ")


                        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                        dtMovimientos = ds.Tables(nTablaMovimientos)
                        If dtMovimientos.Rows.Count - 1 < nPosicionMovi Then nPosicionMovi = dtMovimientos.Rows.Count - 1
                        AsignaTXT(nPosicionCat)
                        AsignaMov(nPosicionMovi, False)

                    End If
                Else
                    If .Item("origen") = "CAJ" Then
                        Dim sDeposito As String

                        NumeroOrigen = IIf(IsDBNull(.Item("numorg")), "", .Item("numorg"))
                        sDeposito = .Item("numdoc")
                        TipoOrigen = .Item("tiporg")
                        sRespuesta = MsgBox(" Movimiento de Caja. Se deshabilitarán estos en Caja. ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                        If sRespuesta = vbYes Then
                            InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("numdoc"))

                            EjecutarSTRSQL(myConn, lblInfo, "DELETE from jsbantraban where " _
                                & " CODBAN = '" & .Item("codban") & "' and  " _
                                & " FECHAMOV = '" & FormatoFechaMySQL(CDate(.Item("fechamov").ToString)) & "' and " _
                                & " NUMDOC = '" & .Item("numdoc") & "' and " _
                                & " EJERCICIO = '" & jytsistema.WorkExercise & "' and" _
                                & " ID_EMP = '" & jytsistema.WorkID & "'")

                            EjecutarSTRSQL(myConn, lblInfo, "UPDATE jsbantracaj SET " _
                                & "DEPOSITO = '' " _
                                & "where " _
                                & "DEPOSITO = '" & sDeposito & "' and " _
                                & "EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
                                & "ID_EMP = '" & jytsistema.WorkID & "'")

                            EjecutarSTRSQL(myConn, lblInfo, "UPDATE jsventabtic SET " _
                                & "NUMDEP = '', FECHADEP = '0000-00-00', BANCODEP = '' " _
                                & "WHERE " _
                                & "NUMDEP = '" & sDeposito & "' AND " _
                                & "BANCODEP = '" & .Item("codban") & "' and " _
                                & "ID_EMP = '" & jytsistema.WorkID & "'")

                            If TipoOrigen = "CT" Then EjecutarSTRSQL(myConn, lblInfo, " delete from jsproencgas where " _
                               & " NUMGAS = '" & NumeroOrigen & "' AND " _
                               & " ID_EMP = '" & jytsistema.WorkID & "'")

                            EjecutarSTRSQL(myConn, lblInfo, " update jsbanenccaj set saldo = " & CalculaSaldoCajaPorFP(myConn, .Item("caja"), "", lblInfo) & " where caja = '" & .Item("caja") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                            EjecutarSTRSQL(myConn, lblInfo, " update jsbancatban set saldoact = " & CalculaSaldoBanco(myConn, lblInfo, txtCodigo1.Text) & " where codban = '" & txtCodigo1.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                            'EjecutarstrSQL(" call saldosbancomensuales('" & txtCodigo1.Text & "','" & jytsistema.WorkExercise & "','" & jytsistema.WorkID & "') ")

                            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                            dtMovimientos = ds.Tables(nTablaMovimientos)
                            If dtMovimientos.Rows.Count - 1 < nPosicionMovi Then nPosicionMovi = dtMovimientos.Rows.Count - 1
                            AsignaTXT(nPosicionCat)
                            AsignaMov(nPosicionMov, True)

                        End If
                    Else
                        MensajeCritico(lblInfo, "Movimiento proveniente de " & .Item("origen") & ". ")
                    End If
                End If
            End With
        End If

    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar

        If tbcBancos.SelectedTab.Text = "Bancos" Then
            Dim Campos() As String = {"codban", "nomban"}
            Dim Nombres() As String = {"Código Banco", "Nombre Banco"}
            Dim Anchos() As Long = {100, 2500}
            f.Text = "Bancos"
            f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, " Bancos...")
            AsignaTXT(f.Apuntador)
            f = Nothing
        Else
            Dim Campos() As String = {"fechamov", "numdoc", "concepto", "nombre"}
            Dim Nombres() As String = {"Emisión", "Nº Movimiento", "Concepto", "Cliente ó Proveedor"}
            Dim Anchos() As Long = {100, 120, 2450, 2450}
            f.Text = "Movimientos bancarios"
            f.Buscar(dtMovimientos, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaMovimientos).Position, "Movimientos de bancos....")
            nPosicionMov = f.Apuntador
            AsignaMov(nPosicionMov, False)
            f = Nothing
        End If

    End Sub


    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        If tbcBancos.SelectedTab.Text = "Bancos" Then
            Dim f As New jsBanRepParametros
            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cFichaBanco, "Ficha de Banco", txtCodigo.Text)
            f = Nothing
        Else
            Dim nComproba As String = dtMovimientos.Rows(nPosicionMov).Item("COMPROBA").ToString
            Dim f As New jsBanRepParametros
            If dtMovimientos.Rows(nPosicionMov).Item("TIPOMOV") = "CH" Then
                f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cComprobanteDeEgreso, "COMPROBANTE DE PAGO", txtCodigo1.Text, nComproba)
                'f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cCheque, "CHEQUE", txtCodigo1.Text, dtMovimientos.Rows(nPosicionMov).Item("COMPROBA"))
            Else
                f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cMovimientoBanco, "Movimientos banco", txtCodigo1.Text)
            End If

            f = Nothing
        End If

    End Sub

    Private Function Validado() As Boolean

        Dim afldR() As String = {"codban", "id_emp"}
        Dim aStrR() As String = {txtCodigo.Text, jytsistema.WorkID}
        If i_modo = movimiento.iAgregar AndAlso qFound(myConn, lblInfo, "jsbancatban", afldR, aStrR) Then
            MensajeAdvertencia(lblInfo, "Código de banco YA existe. Verifique por favor...")
            Return False
        End If

        If Trim(txtNombre.Text) = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar un nombre para esta cuenta de banco...")
            Return False
        End If

        If Trim(txtCtaBan.Text) = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar un número de cuenta bancaria ...")
            Return False
        End If

        If Trim(txtFormato.Text) = "" Then
            MensajeAdvertencia(lblInfo, "Debe indicar un formato de cheques...")
            Return False
        End If

        If CBool(ParametroPlus(myConn, Gestion.iBancos, "BANPARAM16")) Then
            If txtCuentaContable.Text.Trim = "" Then
                MensajeCritico(lblInfo, "Debe indicar una CUENTA CONTABLE VALIDA...")
                Return False
            Else
                If CInt(EjecutarSTRSQL_ScalarPLUS(myConn, " Select count(*) from jscotcatcon where codcon = '" & txtCuentaContable.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")) = 0 Then
                    MensajeCritico(lblInfo, " Debe indicar una CUENTA CONTABLE Válida...")
                    Return False
                End If
                If CInt(EjecutarSTRSQL_ScalarPLUS(myConn, " select count(*) from jsbancatban where codcon = '" & txtCuentaContable.Text & "' and codban <> '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")) > 0 Then
                    MensajeCritico(lblInfo, " La cuenta contable YA ha sido asignada a otro banco ... ")
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
            nPosicionCat = ds.Tables(nTabla).Rows.Count
        End If

        InsertEditBANCOSBanco(myConn, lblInfo, Inserta, txtCodigo.Text, txtNombre.Text, txtCtaBan.Text, txtAgencia.Text, txtDireccion.Text, _
            txtTelef1.Text, txtTelef2.Text, txtFax.Text, txtEmail.Text, txtWeb.Text, txtContacto.Text, _
            cmbTitulo.SelectedItem, ValorNumero(IIf(txtComision.Text = "", "0.00", txtComision.Text)), CDate(txtIngreso.Text), _
            ValorNumero(IIf(txtSaldo.Text = "", "0.00", txtSaldo.Text)), _
            txtCuentaContable.Text, txtFormato.Text, cmbCondicion.SelectedIndex)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" CODBAN = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionCat = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionCat
        AsignaTXT(nPosicionCat)
        DesactivarMarco0()
        tbcBancos.SelectedTab = C1DockingTabPage1
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        MensajeEtiqueta(lblInfo, " Indique el código del banco ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombre.GotFocus
        MensajeEtiqueta(lblInfo, " Indique el nombre del banco (por favor omita la palabra banco)... ", TipoMensaje.iInfo)
    End Sub

    Private Sub txtCtaBan_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCtaBan.GotFocus
        MensajeEtiqueta(lblInfo, " Indique el número de cuenta bancaria ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub txtAgencia_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAgencia.GotFocus
        MensajeEtiqueta(lblInfo, " Indique el nombre de la agencia del banco ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub txtDireccion_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDireccion.GotFocus
        MensajeEtiqueta(lblInfo, " Indique la dirección de la agencia del banco  ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub txtTelef1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTelef1.GotFocus
        MensajeEtiqueta(lblInfo, " Indique el número de teléfono principal ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub txtTelef2_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTelef2.GotFocus
        MensajeEtiqueta(lblInfo, " Indique el número de teléfono secundario ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub txtFax_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFax.GotFocus
        MensajeEtiqueta(lblInfo, " Indique el número de fax ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub txtEmail_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEmail.GotFocus
        MensajeEtiqueta(lblInfo, " Indique la dirección de correo electrónico de este banco ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub txtWeb_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtWeb.GotFocus
        MensajeEtiqueta(lblInfo, " Indique el sitio web de este banco ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub txtContacto_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtContacto.GotFocus
        MensajeEtiqueta(lblInfo, " Indique el nombre de la persona contacto en este banco ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub txtComision_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComision.GotFocus
        MensajeEtiqueta(lblInfo, " Indique el porcentaje de comisión retenido por este banco ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub cmbCondicion_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCondicion.GotFocus
        MensajeEtiqueta(lblInfo, " Indique el estatus o condición de este banco ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
        dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        nPosicionMov = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
    End Sub

    Private Sub tbcBancos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcBancos.SelectedIndexChanged

        Dim aObj() As Object = {btnDepositarEfectivo, btnDepositarTarjetas, btnDepositarCestaTicket, btnReposicion}

        If tbcBancos.SelectedIndex = 0 Then
            AsignaTXT(nPosicionCat)
            HabilitarBotonesAdicionalesBarra(False)
        Else 'movimientos
            AbrirMovimientos(txtCodigo1.Text)
            AsignaMov(nPosicionMov, True)
            dg.Enabled = True
            HabilitarBotonesAdicionalesBarra(True)
        End If
    End Sub

    Private Sub btnDepositarEfectivo_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles btnDepositarEfectivo.DropDownItemClicked
        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        If tbcBancos.SelectedTab.Text = "Movimientos" Then
            Dim f As New jsBanArcDepositarCajaPlus
            f.Depositar(myConn, ds, txtCodigo.Text, e.ClickedItem.Text, 0)
            AsignaMov(nPosicionMov, True)
            f = Nothing
        End If
    End Sub
    Private Sub btnDepositarTarjetas_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles btnDepositarTarjetas.DropDownItemClicked
        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        If tbcBancos.SelectedTab.Text = "Movimientos" Then
            Dim f As New jsBanArcDepositarCajaPlus
            f.Depositar(myConn, ds, txtCodigo.Text, e.ClickedItem.Text, 1)
            AsignaMov(nPosicionMov, True)
            f = Nothing
        End If
    End Sub
    Private Sub btnDepositarCestaTicket_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles btnDepositarCestaTicket.DropDownItemClicked
        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        If tbcBancos.SelectedTab.Text = "Movimientos" Then
            Dim f As New jsBanArcDepositarCajaPlus
            f.Depositar(myConn, ds, txtCodigo.Text, "", 2, e.ClickedItem.Text)
            AsignaMov(nPosicionMov, True)
            f = Nothing
        End If
    End Sub
    Private Sub btnReposicion_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles btnReposicion.DropDownItemClicked
        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        If tbcBancos.SelectedTab.Text = "Movimientos" Then
            Dim f As New jsBanArcReposicionCaja
            f.Cargar(myConn, ds, dtMovimientos, e.ClickedItem.Text, txtCodigo.Text)
            AsignaMov(nPosicionMov, True)
            f = Nothing
        End If
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If dt.Rows.Count = 0 Then
            IniciarBanco(False)
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

    Private Sub btnFormatos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFormatos.Click
        Dim f As New jsBanArcFormatoCheques
        f.Cargar()
        txtFormato.Text = f.Seleccion
        f.Dispose()
        f = Nothing
    End Sub

    Private Sub btnDepositarEfectivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDepositarEfectivo.Click
        '
    End Sub
    Private Sub btnDepositarCestaTicket_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDepositarCestaTicket.Click
        '
    End Sub

    Private Sub btnAgregaTarjeta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaTarjeta.Click
        Dim g As New jsBanArcBancosTarjetas
        g.Agregar(myConn, ds, dtTarjetas, txtCodigo.Text)
        AsignaTXT(nPosicionCat)
        If g.Apuntador >= 0 Then AsignaTar(g.Apuntador, True)
        g = Nothing

    End Sub

    Private Sub btnEditaTarjeta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditaTarjeta.Click
        Dim g As New jsBanArcBancosTarjetas
        g.Editar(myConn, ds, dtTarjetas, txtCodigo.Text)
        AsignaTXT(nPosicionCat)
        If g.Apuntador >= 0 Then AsignaTar(g.Apuntador, True)
        g = Nothing
    End Sub

    Private Sub btnEliminaTarjeta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaTarjeta.Click
        With dtTarjetas
            If .Rows.Count > 0 Then
                nPosicionTar = Me.BindingContext(ds, nTablaTarjetas).Position
                Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
                Dim aCamDel() As String = {"codban", "codtar", "id_emp"}
                Dim aStrDel() As String = {txtCodigo.Text, .Rows(nPosicionTar).Item("codtar").ToString, jytsistema.WorkID}

                sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
                If sRespuesta = MsgBoxResult.Yes Then
                    AsignaTar(EliminarRegistros(myConn, lblInfo, ds, nTablaTarjetas, "jsbancatbantar", _
                                                strSQLTar, aCamDel, aStrDel, nPosicionTar), True)

                End If
            End If
        End With

    End Sub

    Private Sub dgTar_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgTar.RowHeaderMouseClick, _
        dgTar.CellMouseClick, dgTar.RegionChanged
        Me.BindingContext(ds, nTablaTarjetas).Position = e.RowIndex
    End Sub

   
    Private Sub txtComision_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtComision.KeyPress
        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub


  
    Private Sub btObtenerPuertos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btObtenerPuertos.Click
        Dim puertosSerie As List(Of String)
        Dim i As Integer

        cmbPuerto.Items.Clear()

        puertosSerie = obtenerPuertosSeriePC()
        For i = 0 To puertosSerie.Count - 1
            cmbPuerto.Items.Add(puertosSerie(i).ToString)
        Next

        If cmbPuerto.Items.Count >= 1 Then
            cmbPuerto.Text = cmbPuerto.Items(0)
        Else
            cmbPuerto.Text = ""
        End If

        If puertosSerie.Count = 0 Then
            MsgBox("No se han detectado puertos serie en su equipo, " +
                   "asegúrese de que están correctamente configurados.",
                   MsgBoxStyle.Information + MsgBoxStyle.OkOnly)
        End If
    End Sub
    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(ds, nTablaMovimientos).Position += 1
                nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(nPosicionMov, False)
            Case Keys.Up
                Me.BindingContext(ds, nTablaMovimientos).Position -= 1
                nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
                AsignaMov(nPosicionMov, False)
        End Select
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        'Dim r As New frmViewer
        'Dim dsBan As New dsBancos
        'Dim oReporte As New CrystalDecisions.CrystalReports.Engine.ReportClass
        'Dim nTabla As String = "dtBancosFicha"
        'oReporte = New rptBanBancosFicha
        'Dim str As String = " select * from jsbancatban where codban = '00001' and id_emp = '" & jytsistema.WorkID & "' "
        'oReporte.SetDataSource(ds)
        'dsBan = DataSetRequery(dsBan, str, myConn, nTabla, lblInfo)

        'r.CrystalReportViewer1.ReportSource = oReporte
        'r.CrystalReportViewer1.Refresh()
        ''r.Cargar("")
        'r.Close()
        'r = Nothing
        'oReporte.Close()
        'oReporte = Nothing

        'Dim bRet As Boolean = UltimoDocumentoFiscalAclasBixolon(myConn, lblInfo, "FC")

    End Sub

    Private Sub dg_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub

    Private Sub btnCuentaContable_Click(sender As System.Object, e As System.EventArgs) Handles btnCuentaContable.Click
        txtCuentaContable.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by codcon ", "Cuentas Contables", txtCuentaContable.Text)
    End Sub

    Private Sub btnDepositarTarjetas_Click(sender As System.Object, e As System.EventArgs) Handles btnDepositarTarjetas.Click

    End Sub
End Class