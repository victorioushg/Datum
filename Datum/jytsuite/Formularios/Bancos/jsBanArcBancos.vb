Imports MySql.Data.MySqlClient
Imports ReportesDeBancos
Imports Syncfusion.WinForms.Input
Public Class jsBanArcBancos
    Private Const sModulo As String = "Bancos"
    Private Const lRegion As String = "RibbonButton9"
    Private Const nTabla As String = "bancos"
    Private Const nTablaMovimientos As String = "movimientos"
    Private Const nTablaTarjetas As String = "tarjetasbanco"

    Private strSQL As String = "select * from jsbancatban where id_emp = '" & jytsistema.WorkID & "' order by codban"
    Private strSQLMov As String
    Private strSQLTar As String

    Private myConn As New MySql.Data.MySqlClient.MySqlConnection(jytsistema.strConn)

    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtMovimientos As New DataTable
    Private dtTarjetas As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicionCat As Long, nPosicionMov As Long, nPosicionTar As Long
    Private CredentialCheck As Boolean = True

    Private Sub jsBanArcBancos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
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

            ft.colocaOpcionesEnCombos(jytsistema.WorkLanguage, Me)

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaTXT(nPosicionCat)
            Else
                IniciarBanco(False)
            End If
            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            AsignarTooltips()
            IniciarCajasyCestatickets()
            HabilitarBotonesAdicionalesBarra(False)

            Dim dates As SfDateTimeEdit() = {txtIngreso}
            SetSizeDateObjects(dates)

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub HabilitarBotonesAdicionalesBarra(ByVal Habilitar As Boolean)
        ft.habilitarObjetos(Habilitar, False, btnDepositarEfectivo, btnDepositarTarjetas, btnDepositarCestaTicket, btnReposicion)
    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnSeleccionar, btnPrimero, _
                          btnSiguiente, btnAnterior, btnUltimo, btnImprimir, btnSalir, btnDepositarCestaTicket, _
                          btnDepositarEfectivo, btnDepositarTarjetas, btnReposicion)

        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, txtIngreso, btnFormatos, btnCuentaContable)

    End Sub
    Private Sub IniciarCajasyCestatickets()

        Dim iCont As Integer

        Dim dtCajas As New DataTable
        dtCajas = ft.AbrirDataTable(ds, "cajas", myConn, " select * from jsbanenccaj where id_emp = '" & jytsistema.WorkID & "' order by caja ")

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
        dtCorredores = ft.AbrirDataTable(ds, "corredores", myConn, "select * from jsvencestic where id_emp = '" & jytsistema.WorkID & "' order by codigo ")

        For iCont = 0 To dtCorredores.Rows.Count - 1
            Dim tsCT As New ToolStripMenuItem(dtCorredores.Rows(iCont).Item("codigo").ToString & " | " & dtCorredores.Rows(iCont).Item("descrip").ToString, Nothing, New EventHandler(AddressOf btnDepositarCestaTicket_Click))
            btnDepositarCestaTicket.DropDownItems.Add(tsCT)
        Next
        CType(btnDepositarCestaTicket.DropDown, ToolStripDropDownMenu).ShowImageMargin = False

    End Sub
    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)

        dtMovimientos = ft.MostrarFilaEnTabla(myConn, ds, nTablaMovimientos, strSQLMov, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, nRow, Actualiza)
        txtSaldo.Text = ft.FormatoNumero(CalculaSaldoBanco(myConn, lblInfo, txtCodigo.Text))
        txtSaldo1.Text = txtSaldo.Text

    End Sub
    Private Sub AsignaTar(ByVal nRow As Long, ByVal Actualiza As Boolean)
        dtTarjetas = ft.MostrarFilaEnTabla(myConn, ds, nTablaTarjetas, strSQLTar, Me.BindingContext, MenuComisiones, _
                              dgTar, lRegion, jytsistema.sUsuario, nRow, Actualiza, False)
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        If dt.Rows.Count > 0 Then
            With dt

                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

                With .Rows(nRow)
                    'Bancos 
                    nPosicionCat = nRow

                    txtCodigo.Text = ft.muestraCampoTexto(.Item("codban"))
                    txtNombre.Text = ft.muestraCampoTexto(.Item("nomban"))
                    txtCtaBan.Text = ft.muestraCampoTexto(.Item("ctaban"))
                    txtAgencia.Text = ft.muestraCampoTexto(.Item("agencia"))
                    txtDireccion.Text = ft.muestraCampoTexto(.Item("direccion"))
                    txtTelef1.Text = ft.muestraCampoTexto(.Item("telef1"))
                    txtTelef2.Text = ft.muestraCampoTexto(.Item("telef2"))
                    txtFax.Text = ft.muestraCampoTexto(.Item("fax"))
                    txtEmail.Text = ft.muestraCampoTexto(.Item("email"))
                    txtCuentaContable.Text = ft.muestraCampoTexto(.Item("codcon"))
                    txtContacto.Text = ft.muestraCampoTexto(.Item("contacto"))
                    txtComision.Text = ft.muestraCampoNumero(.Item("comision"))
                    txtIngreso.Value = .Item("fechacrea")
                    txtSaldo.Text = ft.muestraCampoNumero(CalculaSaldoBanco(myConn, lblInfo, .Item("codban")))
                    txtFormato.Text = ft.muestraCampoTexto(.Item("formato"))
                    cmbCondicion.SelectedIndex = .Item("estatus")
                    'cmbTitulo.SelectedIndex = ft.InArray( Items() As string  cmbTitulo.Items.CopyTo(   , .Item("titulo"))

                    'Movimientos
                    txtCodigo1.Text = ft.muestraCampoTexto(.Item("codban"))
                    txtNombre1.Text = ft.muestraCampoTexto(.Item("nomban"))
                    txtCtaBan1.Text = ft.muestraCampoTexto(.Item("ctaban"))
                    txtSaldo1.Text = txtSaldo.Text

                    strSQLTar = " select a.codtar, b.nomtar, a.com1, a.com2 from jsbancatbantar a " _
                        & " left join jsconctatar b on (a.codtar = b.codtar and a.id_emp = b.id_emp ) " _
                        & " where a.codban = '" & .Item("codban") & "' and a.id_emp = '" & jytsistema.WorkID & "' "

                    dtTarjetas = ft.AbrirDataTable(ds, nTablaTarjetas, myConn, strSQLTar)

                    Dim aCamTar() As String = {"codtar.Código.75.C.", "nomtar.Nombre.250.I.", "com1.Comisión.80.D.Numero", "com2.ISLR.80.D.Numero"}

                    ft.IniciarTablaPlus(dgTar, dtTarjetas, aCamTar)

                    If dtTarjetas.Rows.Count > 0 Then nPosicionTar = 0

                End With
            End With

            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

            HabilitarBotonesAdicionalesBarra(False)
        End If

    End Sub
    Private Sub AbrirMovimientos(ByVal CodigoBanco As String)

        strSQLMov = "select a.fechamov, a.tipomov, a.numdoc, a.concepto, a.importe, a.origen, a.benefic, a.prov_cli, '' nombre,  a.codven, " _
                          & " a.numorg, a.tiporg, a.codban, a.caja, a.comproba, a.multican, a.conciliado, a.fecconcilia, a.mesconcilia " _
                            & " from jsbantraban a " _
                            & " where " _
                            & " a.codban  = '" & CodigoBanco & "' and " _
                            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " order by a.fechamov desc, a.tipomov, a.numdoc "

        dtMovimientos = ft.AbrirDataTable(ds, nTablaMovimientos, myConn, strSQLMov)

        Dim aCampos() As String = {"fechamov.Emisión.80.C.fecha", _
                                   "tipomov.TP.25.C.", _
                                   "numdoc.Documento.120.I.", _
                                   "concepto.Concepto.350.I.", _
                                   "importe.Importe.120.D.Numero", _
                                   "origen.ORG.30.C.", _
                                   "prov_cli.Prov/Cli.100.I.", _
                                   "codven.Asesor.60.C.", _
                                   "comproba.Comprobante Pago.120.I.", _
                                   "sada..100.I."}
        ft.IniciarTablaPlus(dg, dtMovimientos, aCampos)

        If dtMovimientos.Rows.Count > 0 Then nPosicionMov = 0

    End Sub
    Private Sub IniciarBanco(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = ft.autoCodigo(myConn, "CODBAN", "jsbancatban", "id_emp", jytsistema.WorkID, 5, True)
        Else
            txtCodigo.Text = ""
        End If
        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNombre, txtCtaBan, txtAgencia, txtDireccion, txtTelef1, _
                            txtTelef2, txtFax, txtEmail, txtWeb, txtContacto, txtComision, txtFormato, txtCodigo1, _
                            txtNombre1, txtCtaBan1, txtCuentaContable)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtSaldo, txtSaldo1)
        txtIngreso.Value = jytsistema.sFechadeTrabajo
        txtFormato.Text = "01"
        cmbCondicion.SelectedIndex = 1

        'Movimientos
        dg.Columns.Clear()

    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(False, False, C1DockingTabPage2)
        ft.habilitarObjetos(True, True, txtNombre, txtCtaBan, txtAgencia, txtDireccion, txtTelef1, txtTelef2,
                    txtFax, txtEmail, txtWeb, txtContacto, cmbTitulo, btnCuentaContable,
                    txtComision, btnFormatos, cmbCondicion)

        ft.habilitarObjetos(True, False, MenuComisiones)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False

        ft.habilitarObjetos(True, False, C1DockingTabPage2)
        ft.habilitarObjetos(False, True, txtCodigo, txtCodigo1, txtNombre, txtNombre1, txtCtaBan, txtCtaBan1, txtSaldo,
                            txtAgencia, txtDireccion, txtTelef1, txtTelef2, txtFax, txtEmail, txtWeb, cmbTitulo, txtContacto,
                            txtCuentaContable, btnCuentaContable, txtIngreso, txtComision, txtFormato, btnFormatos,
                            cmbCondicion, MenuComisiones)


        MenuBarra.Enabled = True
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)
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

                With dtMovimientos.Rows(Me.BindingContext(ds, nTablaMovimientos).Position)
                    Dim aCamposAdicionales() As String = {"codban|'" & txtCodigo.Text & "'", _
                                                         "fechamov|'" & ft.FormatoFechaMySQL(Convert.ToDateTime(.Item("FECHAMOV"))) & "'", _
                                                          "numdoc|'" & .Item("NUMDOC") & "'", _
                                                          "tipomov|'" & .Item("TIPOMOV") & "'", _
                                                          "origen|'" & .Item("ORIGEN") & "'", _
                                                          "numorg|'" & .Item("NUMORG") & "'", _
                                                          "prov_cli|'" & .Item("PROV_CLI") & "'"}

                    If DocumentoBloqueado(myConn, "jsbantraban", aCamposAdicionales) Then
                        ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                    Else
                        Dim f As New jsBanArcBancosMovimientosPlus
                        f.Apuntador = Me.BindingContext(ds, nTablaMovimientos).Position
                        f.Editar(myConn, ds, dtMovimientos, txtCodigo.Text)
                        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                        AsignaTXT(nPosicionCat)
                        AsignaMov(f.Apuntador, True)
                        f = Nothing
                    End If

                End With

            End If

        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If tbcBancos.SelectedTab.Text = "Bancos" Then
            EliminaBanco()
        Else
            If cmbCondicion.Text = "Activo" Then
                With dtMovimientos.Rows(nPosicionMov)
                    Dim aCamposAdicionales() As String = {"codban|'" & txtCodigo.Text & "'", _
                                                              "fechamov|'" & ft.FormatoFechaMySQL(Convert.ToDateTime(.Item("FECHAMOV"))) & "'", _
                                                              "numdoc|'" & .Item("NUMDOC") & "'", _
                                                              "tipomov|'" & .Item("TIPOMOV") & "'", _
                                                              "origen|'" & .Item("ORIGEN") & "'", _
                                                              "numorg|'" & .Item("NUMORG") & "'", _
                                                              "prov_cli|'" & .Item("PROV_CLI") & "'"}

                    If DocumentoBloqueado(myConn, "jsbantraban", aCamposAdicionales) Then
                        ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                    Else
                        EliminarMovimiento()
                    End If
                End With
            Else
                ft.mensajeCritico("Esta Cuenta - Banco está Inactiva ....")
            End If
        End If
    End Sub
    Private Sub EliminaBanco()

        Dim aCamposDel() As String = {"codban", "id_emp"}
        Dim aStringsDel() As String = {txtCodigo.Text, jytsistema.WorkID}

        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

            If dtMovimientos.Rows.Count = 0 Then

                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsbancatban", strSQL, aCamposDel, aStringsDel, _
                                              Me.BindingContext(ds, nTabla).Position, True))

            Else
                ft.mensajeCritico("Este banco posee movimientos. Verifique por favor ...")
            End If
        End If

    End Sub
    Private Sub EliminarMovimiento()


        Dim NumeroOrigen As String
        Dim numComproba As String = ""
        Dim TipoOrigen As String
        Dim Tipo As String
        Dim nPosicionMovi As Long

        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        nPosicionMovi = nPosicionMov

        If nPosicionMov >= 0 Then
            With dtMovimientos.Rows(nPosicionMov)

                If CBool(.Item("conciliado")) Then
                    ft.mensajeCritico("MOVIMIENTO CONCILIADO. NO ES POSIBLE ELIMINAR!!!!!")
                    Return
                End If

                If .Item("origen") = "BAN" Then

                    If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("numdoc"))
                        NumeroOrigen = IIf(IsDBNull(.Item("numorg")), "", .Item("numorg"))
                        TipoOrigen = IIf(IsDBNull(.Item("tiporg")), "", .Item("tiporg"))
                        Tipo = IIf(IsDBNull(.Item("tipomov")), "", .Item("tipomov"))
                        numComproba = IIf(IsDBNull(.Item("comproba")), "", .Item("comproba"))

                        Dim DescripcionIDB As String = Convert.ToString(ParametroPlus(myConn, Gestion.iBancos, "BANPARAM04"))


                        If NumeroOrigen.Substring(0, 2) = "TB" Then
                            Dim sResp As Microsoft.VisualBasic.MsgBoxResult = MsgBox("¿ MOVIMIENTO PROVIENE DE UNA TRANSFERENCIA BANCARIA, POR LO TANTO SE ELIMINARA EN TODOS LOS BANCOS. DESEA CONTINUAR ?", MsgBoxStyle.YesNo, "TRANSFERENCIA... ")
                            If sResp = MsgBoxResult.Yes Then

                                ft.Ejecutar_strSQL(myConn, "DELETE from jsbantraban where " _
                                    & " NUMDOC = '" & .Item("numdoc") & "' and " _
                                    & " NUMORG = '" & NumeroOrigen & "' and " _
                                    & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
                                    & " ID_EMP = '" & jytsistema.WorkID & "'")

                                ft.Ejecutar_strSQL(myConn, " delete from jsbanordpag where " _
                                    & " comproba = '" & numComproba & "' and  " _
                                    & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                    & " ID_EMP = '" & jytsistema.WorkID & "' ")

                            End If
                        End If


                        EliminarImpuestoDebitoBancario(myConn, lblInfo, .Item("codban"), .Item("numdoc"), CDate(.Item("fechamov").ToString))

                        ft.Ejecutar_strSQL(myConn, "DELETE FROM jsbantracaj where " _
                            & " REFPAG = '" & .Item("codban") & "' AND " _
                            & " FECHA = '" & ft.FormatoFechaMySQL(CDate(.Item("fechamov").ToString)) & "' AND " _
                            & " NUMMOV = '" & .Item("numdoc") & "' AND " _
                            & " TIPOMOV = 'EN' AND FORMPAG = '" & Tipo & "' AND" _
                            & " NUMPAG = '" & .Item("numdoc") & "' AND " _
                            & " IMPORTE = " & -1 * .Item("importe") & " AND " _
                            & " CAJA = '" & .Item("caja") & "' AND " _
                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'")

                        ft.Ejecutar_strSQL(myConn, "DELETE from jsbantraban where " _
                            & " CODBAN = '" & .Item("codban") & "' and " _
                            & " FECHAMOV = '" & ft.FormatoFechaMySQL(CDate(.Item("fechamov").ToString)) & "' AND " _
                            & " NUMDOC = '" & .Item("numdoc") & "' and " _
                            & " NUMORG = '" & .Item("numorg") & "' and " _
                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'")

                        If Tipo = "CH" Then _
                            ft.Ejecutar_strSQL(myConn, " delete from jsbanordpag where " _
                            & " comproba = '" & numComproba & "' and  " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " ID_EMP = '" & jytsistema.WorkID & "' ")

                        If TipoOrigen = "CH" And Tipo = "ND" Then

                            ft.Ejecutar_strSQL(myConn, "Delete from jsventracob where " _
                               & " TIPOMOV = 'ND' AND " _
                               & " NUMORG = '" & NumeroOrigen & "' AND " _
                               & " ORIGEN = 'BAN' AND " _
                               & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
                               & " ID_EMP = '" & jytsistema.WorkID & "'")

                            ft.Ejecutar_strSQL(myConn, "DELETE FROM jsbanchedev where " _
                               & " NUMCHEQUE = '" & NumeroOrigen & "' AND " _
                               & " ID_EMP = '" & jytsistema.WorkID & "' ")

                        End If

                        If numComproba.Length > 0 AndAlso numComproba.Substring(0, 2) = "CP" Then
                            ft.Ejecutar_strSQL(myConn, " delete from jsbantracaj where tipomov = 'EN' and nummov = '" & numComproba & "' and id_emp = '" & jytsistema.WorkID & "' ")
                            ft.Ejecutar_strSQL(myConn, " update jsbantracaj set deposito = '' where deposito = '" & numComproba & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        End If

                        ft.Ejecutar_strSQL(myConn, " update jsbanenccaj set saldo = " & CalculaSaldoCajaPorFP(myConn, .Item("caja"), "", lblInfo) & " where caja = '" & .Item("caja") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        ft.Ejecutar_strSQL(myConn, " update jsbancatban set saldoact = " & CalculaSaldoBanco(myConn, lblInfo, txtCodigo1.Text) & " where codban = '" & txtCodigo1.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

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

                        If ft.PreguntaEliminarRegistro(" Movimiento proveniente de caja, estos se deshabilitarán en la caja!!!") = Windows.Forms.DialogResult.Yes Then

                            InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("numdoc"))

                            ft.Ejecutar_strSQL(myConn, "DELETE from jsbantraban where " _
                                & " CODBAN = '" & .Item("codban") & "' and  " _
                                & " FECHAMOV = '" & ft.FormatoFechaMySQL(CDate(.Item("fechamov").ToString)) & "' and " _
                                & " NUMDOC = '" & .Item("numdoc") & "' and " _
                                & " EJERCICIO = '" & jytsistema.WorkExercise & "' and" _
                                & " ID_EMP = '" & jytsistema.WorkID & "'")

                            ft.Ejecutar_strSQL(myConn, "UPDATE jsbantracaj SET " _
                                & "DEPOSITO = '' " _
                                & "where " _
                                & "DEPOSITO = '" & sDeposito & "' and " _
                                & "EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
                                & "ID_EMP = '" & jytsistema.WorkID & "'")

                            ft.Ejecutar_strSQL(myConn, "UPDATE jsventabtic SET " _
                                & "NUMDEP = '', FECHADEP = '0000-00-00', BANCODEP = '' " _
                                & "WHERE " _
                                & "NUMDEP = '" & sDeposito & "' AND " _
                                & "BANCODEP = '" & .Item("codban") & "' and " _
                                & "ID_EMP = '" & jytsistema.WorkID & "'")

                            If TipoOrigen = "CT" Then ft.Ejecutar_strSQL(myConn, " delete from jsproencgas where " _
                               & " NUMGAS = '" & NumeroOrigen & "' AND " _
                               & " ID_EMP = '" & jytsistema.WorkID & "'")

                            ft.Ejecutar_strSQL(myConn, " update jsbanenccaj set saldo = " & CalculaSaldoCajaPorFP(myConn, .Item("caja"), "", lblInfo) & " where caja = '" & .Item("caja") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                            ft.Ejecutar_strSQL(myConn, " update jsbancatban set saldoact = " & CalculaSaldoBanco(myConn, lblInfo, txtCodigo1.Text) & " where codban = '" & txtCodigo1.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                            dtMovimientos = ds.Tables(nTablaMovimientos)
                            If dtMovimientos.Rows.Count - 1 < nPosicionMovi Then nPosicionMovi = dtMovimientos.Rows.Count - 1
                            AsignaTXT(nPosicionCat)
                            AsignaMov(nPosicionMov, True)

                        End If
                    Else
                        ft.mensajeCritico("Movimiento proveniente de " & .Item("origen") & ". ")
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
            Dim Anchos() As Integer = {100, 2500}
            f.Text = "Bancos"
            f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, " Bancos...")
            AsignaTXT(f.Apuntador)
            f = Nothing
        Else
            Dim Campos() As String = {"numdoc", "fechamov", "concepto", "nombre"}
            Dim Nombres() As String = {"Nº Movimiento", "Emisión", "Concepto", "Cliente ó Proveedor"}
            Dim Anchos() As Integer = {120, 100, 2450, 2450}
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
            ft.mensajeAdvertencia("Código de banco YA existe. Verifique por favor...")
            Return False
        End If

        If Trim(txtNombre.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre para esta cuenta de banco...")
            Return False
        End If

        If Trim(txtCtaBan.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un número de cuenta bancaria ...")
            Return False
        End If

        If Trim(txtFormato.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un formato de cheques...")
            Return False
        End If

        If CBool(ParametroPlus(myConn, Gestion.iBancos, "BANPARAM16")) Then
            If txtCuentaContable.Text.Trim = "" Then
                ft.MensajeCritico("Debe indicar una CUENTA CONTABLE VALIDA...")
                Return False
            Else
                If ft.DevuelveScalarEntero(myConn, " Select count(*) from jscotcatcon where codcon = '" & txtCuentaContable.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                    ft.mensajeCritico(" Debe indicar una CUENTA CONTABLE Válida...")
                    Return False
                End If
                If ft.DevuelveScalarEntero(myConn, " select count(*) from jsbancatban where codcon = '" & txtCuentaContable.Text & "' and codban <> '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") > 0 Then
                    ft.mensajeCritico(" La cuenta contable YA ha sido asignada a otro banco ... ")
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

        InsertEditBANCOSBanco(myConn, lblInfo, Inserta, txtCodigo.Text, txtNombre.Text, txtCtaBan.Text, txtAgencia.Text, txtDireccion.Text,
            txtTelef1.Text, txtTelef2.Text, txtFax.Text, txtEmail.Text, txtWeb.Text, txtContacto.Text,
            cmbTitulo.SelectedItem, ValorNumero(IIf(txtComision.Text = "", "0.00", txtComision.Text)), txtIngreso.Value,
            ValorNumero(IIf(txtSaldo.Text = "", "0.00", txtSaldo.Text)),
            txtCuentaContable.Text, txtFormato.Text, cmbCondicion.SelectedIndex)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" CODBAN = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionCat = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionCat
        AsignaTXT(nPosicionCat)
        DesactivarMarco0()
        tbcBancos.SelectedTab = C1DockingTabPage1
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, txtNombre.GotFocus, _
        txtCtaBan.GotFocus, txtAgencia.GotFocus, txtDireccion.GotFocus, txtTelef1.GotFocus, txtTelef2.GotFocus, txtFax.GotFocus, txtEmail.GotFocus, _
        txtWeb.GotFocus, txtContacto.GotFocus, txtComision.GotFocus, cmbCondicion.GotFocus, cmbTitulo.GotFocus, btnCuentaContable.GotFocus, btnFormatos.GotFocus

        ft.colocaMensajeEnEtiqueta(sender, jytsistema.WorkLanguage, lblInfo)

    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
        dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        nPosicionMov = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
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
        f.Cargar(myConn)
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

                Dim aCamDel() As String = {"codban", "codtar", "id_emp"}
                Dim aStrDel() As String = {txtCodigo.Text, .Rows(nPosicionTar).Item("codtar").ToString, jytsistema.WorkID}

                If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
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
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub



    Private Sub btObtenerPuertos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btObtenerPuertos.Click
        'Dim puertosSerie As List(Of String)
        'Dim i As Integer

        'cmbPuerto.Items.Clear()

        'puertosSerie = obtenerPuertosSeriePC()
        'For i = 0 To puertosSerie.Count - 1
        '    cmbPuerto.Items.Add(puertosSerie(i).ToString)
        'Next

        'If cmbPuerto.Items.Count >= 1 Then
        '    cmbPuerto.Text = cmbPuerto.Items(0)
        'Else
        '    cmbPuerto.Text = ""
        'End If

        'If puertosSerie.Count = 0 Then
        '    MsgBox("No se han detectado puertos serie en su equipo, " +
        '           "asegúrese de que están correctamente configurados.",
        '           MsgBoxStyle.Information + MsgBoxStyle.OkOnly)
        'End If
        '   ft.mensajeInformativo(InStr("FAC.PVE.PFC.NDV", "fac"))
        ft.mensajeInformativo(getMacAddress() & "  " & driveser(""))
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