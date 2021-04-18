Imports MySql.Data.MySqlClient
Imports ReportesDeBancos
Public Class jsContabArcActivosFijos
    Private Const sModulo As String = "Activos Fijos"
    Private Const lRegion As String = "RibbonButton5"
    Private Const nTabla As String = "activosfijos"
    Private Const nTablaMovimientos As String = "movimientos"


    Private strSQL As String = "select * from jscotactfij where id_emp = '" & jytsistema.WorkID & "' order by codigo "
    Private strSQLMov As String

    Private myConn As New MySqlConnection(jytsistema.strConn)

    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtMovimientos As New DataTable

    Private ft As New Transportables

    '// Private aCondicion() As String = {"Desincorporado|Inactive", "Activo|Active"}
    'Private aTipoActivo() As String = {"Depreciable", "No depreciable", "Amortizable", "Realizable"}
    'Private aMetodo() As String = {"Linea recta", "Progresivo creciente", "Progresivo decreciente"}

    Private i_modo As Integer
    Private nPosicionCat As Long, nPosicionMov As Long

    Private Sub jsContabArcActivosFijos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsContabArcActivosFijos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo

        ft.colocaIdiomaEtiquetas(jytsistema.WorkLanguage, Me)

        Try
            myConn.Open()
            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)

            ft.colocaOpcionesEnCombos(jytsistema.WorkLanguage, Me)
           
            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionCat = 0
                AsignaTXT(nPosicionCat)
            Else
                IniciarActivo(False)
            End If
            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            AsignarTooltips()
            '         HabilitarBotonesAdicionalesBarra(False)

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub HabilitarBotonesAdicionalesBarra(ByVal Habilitar As Boolean)
        'ft.habilitarObjetos(Habilitar, False, btnDepositarEfectivo, btnDepositarTarjetas, btnDepositarCestaTicket, btnReposicion)
    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnSeleccionar, btnPrimero, _
                          btnSiguiente, btnAnterior, btnUltimo, btnImprimir, btnSalir)

        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnIngreso, btnFechaAdquisicion, btnInicioValuacion, btnGrupo, btnMoneda, btnUbica, _
                         btnCuentaActivos, btnCuentaGastos, btnCuentaRepreciacion)

    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        dtMovimientos = ft.MostrarFilaEnTabla(myConn, ds, nTablaMovimientos, strSQLMov, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, nRow, Actualiza)
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)
        If dt.Rows.Count > 0 Then
            With dt
                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)
                nPosicionCat = nRow
                With .Rows(nRow)

                    txtCodigo.Text = ft.muestraCampoTexto(.Item("CODIGO"))
                    txtDescripcion.Text = ft.muestraCampoTexto(.Item("DESCRIPCION"))

                    cmbCondicion.SelectedIndex = .Item("ESTATUS")

                    txtFecha.Text = ft.muestraCampoFecha(.Item("INGRESO"))
                    txtAdquisicion.Text = ft.muestraCampoFecha(.Item("FECHA_ADQUISICION"))
                    txtAdquisicionMonto.Text = ft.muestraCampoNumero(.Item("VALOR_ADQUISICION"))
                    txtSalvamento.Text = ft.muestraCampoNumero(.Item("SALVAMENTO"))
                    txtInicioValuacion.Text = ft.muestraCampoFecha(.Item("INICIO_VALUACION"))
                    txtGrupo.Text = ft.muestraCampoTexto(.Item("GRUPO"))
                    txtSerial.Text = ft.muestraCampoTexto(.Item("SERIAL"))
                    txtMoneda.Text = ft.muestraCampoTexto(.Item("MONEDA"))
                    txtTasa.Text = ft.muestraCampoCantidadLarga(.Item("TASA"))
                    txtUbica.Text = ft.muestraCampoTexto(.Item("UBICACION"))

                    cmbTipoActivo.SelectedIndex = .Item("TIPO")
                    cmbMetodoValuacion.SelectedIndex = .Item("METODO")

                    txtAños.Text = ft.muestraCampoEntero(.Item("VIDA_ANOS"))
                    txtMeses.Text = ft.muestraCampoEntero(.Item("VIDA_MESES"))


                    txtActivo.Text = ft.muestraCampoTexto(.Item("CUENTA_ACTIVO_PASIVO"))
                    txtGastos.Text = ft.muestraCampoTexto(.Item("CUENTA_GASTOS_INGRESOS"))
                    txtRepreciacion.Text = ft.muestraCampoTexto(.Item("CUENTA_REDEPRECIACION_ACUMULADA"))

                    'Movimientos
                    txtCodigo1.Text = ft.muestraCampoTexto(.Item("CODIGO"))
                    txtNombre1.Text = ft.muestraCampoTexto(.Item("DESCRIPCION"))

                End With
            End With

            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

            HabilitarBotonesAdicionalesBarra(False)
        End If

    End Sub
    'Private Sub AbrirMovimientos(ByVal CodigoBanco As String)

    '    strSQLMov = "select a.fechamov, a.tipomov, a.numdoc, a.concepto, a.importe, a.origen, a.benefic, a.prov_cli, '' nombre,  a.codven, " _
    '                      & " a.numorg, a.tiporg, a.codban, a.caja, a.comproba, a.multican, a.conciliado, a.fecconcilia, a.mesconcilia " _
    '                        & " from jsbantraban a " _
    '                        & " where " _
    '                        & " a.codban  = '" & CodigoBanco & "' and " _
    '                        & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
    '                        & " a.id_emp = '" & jytsistema.WorkID & "' " _
    '                        & " order by a.fechamov desc, a.tipomov, a.numdoc "

    '    dtMovimientos = ft.AbrirDataTable(ds, nTablaMovimientos, myConn, strSQLMov)

    '    Dim aCampos() As String = {"fechamov.Emisión.80.C.fecha", _
    '                               "tipomov.TP.25.C.", _
    '                               "numdoc.Documento.120.I.", _
    '                               "concepto.Concepto.350.I.", _
    '                               "importe.Importe.120.D.Numero", _
    '                               "origen.ORG.30.C.", _
    '                               "prov_cli.Prov/Cli.100.I.", _
    '                               "codven.Asesor.60.C.", _
    '                               "comproba.Comprobante Pago.120.I.", _
    '                               "sada..100.I."}
    '    ft.IniciarTablaPlus(dg, dtMovimientos, aCampos)

    '    If dtMovimientos.Rows.Count > 0 Then nPosicionMov = 0

    'End Sub
    Private Sub IniciarActivo(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = ft.autoCodigo(myConn, "CODIGO", "jscotactfij", "id_emp", jytsistema.WorkID, 15, True)
        Else
            txtCodigo.Text = ""
        End If

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtDescripcion, txtSerial, txtGrupo, txtMoneda, txtUbica, _
                               txtActivo, txtGastos, txtRepreciacion)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Entero, txtAños, txtMeses)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Fecha, txtFecha, txtAdquisicion, txtInicioValuacion)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtAdquisicionMonto, txtSalvamento, txtTasa)

        cmbCondicion.SelectedIndex = 1
        cmbTipoActivo.SelectedIndex = 0
        cmbMetodoValuacion.SelectedIndex = 0

        'Movimientos
        dg.Columns.Clear()

    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(False, False, C1DockingTabPage2)
        ft.habilitarObjetos(True, True, txtDescripcion, txtSerial, txtTasa, txtAdquisicionMonto, txtSalvamento, txtAños, txtMeses, _
                            cmbCondicion, cmbTipoActivo, cmbMetodoValuacion)
        ft.habilitarObjetos(True, False, btnIngreso, btnFechaAdquisicion, btnGrupo, btnMoneda, btnUbica, btnInicioValuacion, btnCuentaActivos, _
                            btnCuentaGastos, btnCuentaRepreciacion)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False

        ft.habilitarObjetos(True, False, C1DockingTabPage2)
        ft.habilitarObjetos(False, True, txtCodigo, txtCodigo1, txtDescripcion, txtNombre1, cmbCondicion, txtFecha, btnIngreso, _
                            txtGrupo, btnGrupo, txtUbica, btnUbica, txtMoneda, btnMoneda, txtTasa, txtSerial, cmbTipoActivo, _
                            cmbMetodoValuacion, txtAdquisicion, btnFechaAdquisicion, txtAdquisicionMonto, txtSalvamento, _
                            txtInicioValuacion, btnInicioValuacion, txtAños, txtMeses, txtActivo, txtGastos, txtRepreciacion, _
                            btnCuentaActivos, btnCuentaGastos, btnCuentaRepreciacion, cmbCondicion, cmbTipoActivo, cmbMetodoValuacion)

        MenuBarra.Enabled = True
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub
    Private Sub btnIngreso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIngreso.Click
        txtFecha.Text = SeleccionaFecha(CDate(txtFecha.Text), Me, btnIngreso)
    End Sub
    Private Sub btnFechaAdquisicion_Click(sender As Object, e As EventArgs) Handles btnFechaAdquisicion.Click
        txtAdquisicion.Text = SeleccionaFecha(CDate(txtAdquisicion.Text), Me, btnFechaAdquisicion)
    End Sub

    Private Sub btnInicioValuacion_Click(sender As Object, e As EventArgs) Handles btnInicioValuacion.Click
        txtInicioValuacion.Text = SeleccionaFecha(CDate(txtInicioValuacion.Text), Me, btnInicioValuacion)
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Select Case tbcActivos.SelectedIndex
            Case 0 ' Catalogo
                Me.BindingContext(ds, nTabla).Position = 0
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case 1 'Movimientos
                Me.BindingContext(ds, nTablaMovimientos).Position = 0
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End Select

    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Select Case tbcActivos.SelectedIndex
            Case 0 'Activos
                Me.BindingContext(ds, nTabla).Position -= 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case 1 'Movimientos
                Me.BindingContext(ds, nTablaMovimientos).Position -= 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End Select
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Select Case tbcActivos.SelectedIndex
            Case 0 'Activos
                Me.BindingContext(ds, nTabla).Position += 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case 1 'Movimientos
                Me.BindingContext(ds, nTablaMovimientos).Position += 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End Select
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Select Case tbcActivos.SelectedIndex
            Case 0 'Catalogo
                Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
                AsignaTXT(Me.BindingContext(ds, nTabla).Position)
            Case 1 'Movimientos
                Me.BindingContext(ds, nTablaMovimientos).Position = ds.Tables(nTablaMovimientos).Rows.Count - 1
                AsignaMov(Me.BindingContext(ds, nTablaMovimientos).Position, False)
        End Select
    End Sub
    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Select Case tbcActivos.SelectedIndex
            Case 0 'Catalogo
                i_modo = movimiento.iAgregar
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
                IniciarActivo(True)
            Case 1 'Movimientos
                'Dim f As New jsBanArcBancosMovimientosPlus
                'f.Agregar(myConn, ds, dtMovimientos, txtCodigo.Text)
                'ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                'ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                'AsignaTXT(nPosicionCat)
                'If f.Apuntador >= 0 Then AsignaMov(f.Apuntador, True)
                'f = Nothing
        End Select

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Select Case tbcActivos.SelectedIndex
            Case 0 'Catalogo
                i_modo = movimiento.iEditar
                nPosicionCat = Me.BindingContext(ds, nTabla).Position
                ActivarMarco0()
            Case 1 'Movimientos
                'If dtMovimientos.Rows(Me.BindingContext(ds, nTablaMovimientos).Position).Item("origen") = "BAN" Then

                '    With dtMovimientos.Rows(Me.BindingContext(ds, nTablaMovimientos).Position)
                '        Dim aCamposAdicionales() As String = {"codban|'" & txtCodigo.Text & "'", _
                '                                             "fechamov|'" & ft.FormatoFechaMySQL(Convert.ToDateTime(.Item("FECHAMOV"))) & "'", _
                '                                              "numdoc|'" & .Item("NUMDOC") & "'", _
                '                                              "tipomov|'" & .Item("TIPOMOV") & "'", _
                '                                              "origen|'" & .Item("ORIGEN") & "'", _
                '                                              "numorg|'" & .Item("NUMORG") & "'", _
                '                                              "prov_cli|'" & .Item("PROV_CLI") & "'"}

                '        If DocumentoBloqueado(myConn, "jsbantraban", aCamposAdicionales) Then
                '            ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                '        Else
                '            Dim f As New jsBanArcBancosMovimientosPlus
                '            f.Apuntador = Me.BindingContext(ds, nTablaMovimientos).Position
                '            f.Editar(myConn, ds, dtMovimientos, txtCodigo.Text)
                '            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                '            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                '            AsignaTXT(nPosicionCat)
                '            AsignaMov(f.Apuntador, True)
                '            f = Nothing
                '        End If

                '    End With

                'End If
        End Select

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        Select Case tbcActivos.SelectedIndex
            Case 0 'Catalago
                EliminaActivo()
            Case 1 'Movimientos
                'If cmbCondicion.Text = "Activo" Then
                '    With dtMovimientos.Rows(nPosicionMov)
                '        Dim aCamposAdicionales() As String = {"codban|'" & txtCodigo.Text & "'", _
                '                                                  "fechamov|'" & ft.FormatoFechaMySQL(Convert.ToDateTime(.Item("FECHAMOV"))) & "'", _
                '                                                  "numdoc|'" & .Item("NUMDOC") & "'", _
                '                                                  "tipomov|'" & .Item("TIPOMOV") & "'", _
                '                                                  "origen|'" & .Item("ORIGEN") & "'", _
                '                                                  "numorg|'" & .Item("NUMORG") & "'", _
                '                                                  "prov_cli|'" & .Item("PROV_CLI") & "'"}

                '        If DocumentoBloqueado(myConn, "jsbantraban", aCamposAdicionales) Then
                '            ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                '        Else
                '            EliminarMovimiento()
                '        End If
                '    End With
                'Else
                '    ft.mensajeCritico("Esta Cuenta - Banco está Inactiva ....")
                'End If
        End Select

    End Sub
    Private Sub EliminaActivo()

        Dim aCamposDel() As String = {"CODIGO", "ID_EMP"}
        Dim aStringsDel() As String = {txtCodigo.Text, jytsistema.WorkID}

        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
            If dtMovimientos.Rows.Count = 0 Then
                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jscotactfij", strSQL, aCamposDel, aStringsDel, _
                                              Me.BindingContext(ds, nTabla).Position, True))
            Else
                ft.mensajeCritico("Este ACTIVO posee movimientos. Verifique por favor ...")
            End If
        End If

    End Sub
    'Private Sub EliminarMovimiento()


    '    Dim NumeroOrigen As String
    '    Dim numComproba As String = ""
    '    Dim TipoOrigen As String
    '    Dim Tipo As String
    '    Dim nPosicionMovi As Long

    '    nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
    '    nPosicionMovi = nPosicionMov

    '    If nPosicionMov >= 0 Then
    '        With dtMovimientos.Rows(nPosicionMov)

    '            If CBool(.Item("conciliado")) Then
    '                ft.mensajeCritico("MOVIMIENTO CONCILIADO. NO ES POSIBLE ELIMINAR!!!!!")
    '                Return
    '            End If

    '            If .Item("origen") = "BAN" Then

    '                If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

    '                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("numdoc"))
    '                    NumeroOrigen = IIf(IsDBNull(.Item("numorg")), "", .Item("numorg"))
    '                    TipoOrigen = IIf(IsDBNull(.Item("tiporg")), "", .Item("tiporg"))
    '                    Tipo = IIf(IsDBNull(.Item("tipomov")), "", .Item("tipomov"))
    '                    numComproba = IIf(IsDBNull(.Item("comproba")), "", .Item("comproba"))

    '                    Dim DescripcionIDB As String = Convert.ToString(ParametroPlus(myConn, Gestion.iBancos, "BANPARAM04"))


    '                    If NumeroOrigen.Substring(0, 2) = "TB" Then
    '                        Dim sResp As Microsoft.VisualBasic.MsgBoxResult = MsgBox("¿ MOVIMIENTO PROVIENE DE UNA TRANSFERENCIA BANCARIA, POR LO TANTO SE ELIMINARA EN TODOS LOS BANCOS. DESEA CONTINUAR ?", MsgBoxStyle.YesNo, "TRANSFERENCIA... ")
    '                        If sResp = MsgBoxResult.Yes Then

    '                            ft.Ejecutar_strSQL(myConn, "DELETE from jsbantraban where " _
    '                                & " NUMDOC = '" & .Item("numdoc") & "' and " _
    '                                & " NUMORG = '" & NumeroOrigen & "' and " _
    '                                & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
    '                                & " ID_EMP = '" & jytsistema.WorkID & "'")

    '                            ft.Ejecutar_strSQL(myConn, " delete from jsbanordpag where " _
    '                                & " comproba = '" & numComproba & "' and  " _
    '                                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
    '                                & " ID_EMP = '" & jytsistema.WorkID & "' ")

    '                        End If
    '                    End If


    '                    EliminarImpuestoDebitoBancario(myConn, lblInfo, .Item("codban"), .Item("numdoc"), CDate(.Item("fechamov").ToString))

    '                    ft.Ejecutar_strSQL(myConn, "DELETE FROM jsbantracaj where " _
    '                        & " REFPAG = '" & .Item("codban") & "' AND " _
    '                        & " FECHA = '" & ft.FormatoFechaMySQL(CDate(.Item("fechamov").ToString)) & "' AND " _
    '                        & " NUMMOV = '" & .Item("numdoc") & "' AND " _
    '                        & " TIPOMOV = 'EN' AND FORMPAG = '" & Tipo & "' AND" _
    '                        & " NUMPAG = '" & .Item("numdoc") & "' AND " _
    '                        & " IMPORTE = " & -1 * .Item("importe") & " AND " _
    '                        & " CAJA = '" & .Item("caja") & "' AND " _
    '                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
    '                        & " ID_EMP = '" & jytsistema.WorkID & "'")

    '                    ft.Ejecutar_strSQL(myConn, "DELETE from jsbantraban where " _
    '                        & " CODBAN = '" & .Item("codban") & "' and " _
    '                        & " FECHAMOV = '" & ft.FormatoFechaMySQL(CDate(.Item("fechamov").ToString)) & "' AND " _
    '                        & " NUMDOC = '" & .Item("numdoc") & "' and " _
    '                        & " NUMORG = '" & .Item("numorg") & "' and " _
    '                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
    '                        & " ID_EMP = '" & jytsistema.WorkID & "'")

    '                    If Tipo = "CH" Then _
    '                        ft.Ejecutar_strSQL(myConn, " delete from jsbanordpag where " _
    '                        & " comproba = '" & numComproba & "' and  " _
    '                        & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
    '                        & " ID_EMP = '" & jytsistema.WorkID & "' ")

    '                    If TipoOrigen = "CH" And Tipo = "ND" Then

    '                        ft.Ejecutar_strSQL(myConn, "Delete from jsventracob where " _
    '                           & " TIPOMOV = 'ND' AND " _
    '                           & " NUMORG = '" & NumeroOrigen & "' AND " _
    '                           & " ORIGEN = 'BAN' AND " _
    '                           & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
    '                           & " ID_EMP = '" & jytsistema.WorkID & "'")

    '                        ft.Ejecutar_strSQL(myConn, "DELETE FROM jsbanchedev where " _
    '                           & " NUMCHEQUE = '" & NumeroOrigen & "' AND " _
    '                           & " ID_EMP = '" & jytsistema.WorkID & "' ")

    '                    End If

    '                    If numComproba.Length > 0 AndAlso numComproba.Substring(0, 2) = "CP" Then
    '                        ft.Ejecutar_strSQL(myConn, " delete from jsbantracaj where tipomov = 'EN' and nummov = '" & numComproba & "' and id_emp = '" & jytsistema.WorkID & "' ")
    '                        ft.Ejecutar_strSQL(myConn, " update jsbantracaj set deposito = '' where deposito = '" & numComproba & "' and id_emp = '" & jytsistema.WorkID & "' ")
    '                    End If

    '                    ft.Ejecutar_strSQL(myConn, " update jsbanenccaj set saldo = " & CalculaSaldoCajaPorFP(myConn, .Item("caja"), "", lblInfo) & " where caja = '" & .Item("caja") & "' and id_emp = '" & jytsistema.WorkID & "' ")
    '                    ft.Ejecutar_strSQL(myConn, " update jsbancatban set saldoact = " & CalculaSaldoBanco(myConn, lblInfo, txtCodigo1.Text) & " where codban = '" & txtCodigo1.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

    '                    ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
    '                    ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
    '                    dtMovimientos = ds.Tables(nTablaMovimientos)
    '                    If dtMovimientos.Rows.Count - 1 < nPosicionMovi Then nPosicionMovi = dtMovimientos.Rows.Count - 1
    '                    AsignaTXT(nPosicionCat)
    '                    AsignaMov(nPosicionMovi, False)

    '                End If
    '            Else
    '                If .Item("origen") = "CAJ" Then
    '                    Dim sDeposito As String

    '                    NumeroOrigen = IIf(IsDBNull(.Item("numorg")), "", .Item("numorg"))
    '                    sDeposito = .Item("numdoc")
    '                    TipoOrigen = .Item("tiporg")

    '                    If ft.PreguntaEliminarRegistro(" Movimiento proveniente de caja, estos se deshabilitarán en la caja!!!") = Windows.Forms.DialogResult.Yes Then

    '                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("numdoc"))

    '                        ft.Ejecutar_strSQL(myConn, "DELETE from jsbantraban where " _
    '                            & " CODBAN = '" & .Item("codban") & "' and  " _
    '                            & " FECHAMOV = '" & ft.FormatoFechaMySQL(CDate(.Item("fechamov").ToString)) & "' and " _
    '                            & " NUMDOC = '" & .Item("numdoc") & "' and " _
    '                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' and" _
    '                            & " ID_EMP = '" & jytsistema.WorkID & "'")

    '                        ft.Ejecutar_strSQL(myConn, "UPDATE jsbantracaj SET " _
    '                            & "DEPOSITO = '' " _
    '                            & "where " _
    '                            & "DEPOSITO = '" & sDeposito & "' and " _
    '                            & "EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
    '                            & "ID_EMP = '" & jytsistema.WorkID & "'")

    '                        ft.Ejecutar_strSQL(myConn, "UPDATE jsventabtic SET " _
    '                            & "NUMDEP = '', FECHADEP = '0000-00-00', BANCODEP = '' " _
    '                            & "WHERE " _
    '                            & "NUMDEP = '" & sDeposito & "' AND " _
    '                            & "BANCODEP = '" & .Item("codban") & "' and " _
    '                            & "ID_EMP = '" & jytsistema.WorkID & "'")

    '                        If TipoOrigen = "CT" Then ft.Ejecutar_strSQL(myConn, " delete from jsproencgas where " _
    '                           & " NUMGAS = '" & NumeroOrigen & "' AND " _
    '                           & " ID_EMP = '" & jytsistema.WorkID & "'")

    '                        ft.Ejecutar_strSQL(myConn, " update jsbanenccaj set saldo = " & CalculaSaldoCajaPorFP(myConn, .Item("caja"), "", lblInfo) & " where caja = '" & .Item("caja") & "' and id_emp = '" & jytsistema.WorkID & "' ")
    '                        ft.Ejecutar_strSQL(myConn, " update jsbancatban set saldoact = " & CalculaSaldoBanco(myConn, lblInfo, txtCodigo1.Text) & " where codban = '" & txtCodigo1.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

    '                        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
    '                        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
    '                        dtMovimientos = ds.Tables(nTablaMovimientos)
    '                        If dtMovimientos.Rows.Count - 1 < nPosicionMovi Then nPosicionMovi = dtMovimientos.Rows.Count - 1
    '                        AsignaTXT(nPosicionCat)
    '                        AsignaMov(nPosicionMov, True)

    '                    End If
    '                Else
    '                    ft.mensajeCritico("Movimiento proveniente de " & .Item("origen") & ". ")
    '                End If
    '            End If
    '        End With
    '    End If

    'End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        Dim f As New frmBuscar
        Select Case tbcActivos.SelectedIndex
            Case 0 'Catalogo
                Dim Campos() As String = {"codban", "nomban"}
                Dim Nombres() As String = {"Código Banco", "Nombre Banco"}
                Dim Anchos() As Integer = {100, 2500}
                f.Text = "Bancos"
                f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, " Bancos...")
                AsignaTXT(f.Apuntador)
                f = Nothing
            Case 1 'Movimientos
                Dim Campos() As String = {"numdoc", "fechamov", "concepto", "nombre"}
                Dim Nombres() As String = {"Nº Movimiento", "Emisión", "Concepto", "Cliente ó Proveedor"}
                Dim Anchos() As Integer = {120, 100, 2450, 2450}
                f.Text = "Movimientos bancarios"
                f.Buscar(dtMovimientos, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaMovimientos).Position, "Movimientos de bancos....")
                nPosicionMov = f.Apuntador
                AsignaMov(nPosicionMov, False)
                f = Nothing
        End Select

    End Sub


    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Select Case tbcActivos.SelectedIndex
            Case 0 'Catalogo
                'Dim f As New jsBanRepParametros
                'f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cFichaBanco, "Ficha de Banco", txtCodigo.Text)
                'f = Nothing
            Case 1 'Movimientos
                'Dim nComproba As String = dtMovimientos.Rows(nPosicionMov).Item("COMPROBA").ToString
                'Dim f As New jsBanRepParametros
                'If dtMovimientos.Rows(nPosicionMov).Item("TIPOMOV") = "CH" Then
                '    f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cComprobanteDeEgreso, "COMPROBANTE DE PAGO", txtCodigo1.Text, nComproba)
                'Else
                '    f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cMovimientoBanco, "Movimientos banco", txtCodigo1.Text)
                'End If

                'f = Nothing
        End Select

    End Sub

    Private Function Validado() As Boolean

        If i_modo = movimiento.iAgregar AndAlso _
            ft.Ejecutar_strSQL_DevuelveScalar(myConn, " SELECT COUNT(*) FROM jscotactfij WHERE " _
                                              & " CODIGO = '" & txtCodigo.Text & "' AND " _
                                              & " ID_EMP = '" & jytsistema.WorkID & "' ") > 0 Then
            ft.mensajeAdvertencia("Código de ACTIVO YA existe. Verifique por favor...")
            Return False
        End If

        If Trim(txtDescripcion.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre para este ACTIVO ...")
            Return False
        End If

        Return True

    End Function
    Private Sub GuardarTXT()

        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionCat = ds.Tables(nTabla).Rows.Count
        End If

        'InsertEditBANCOSBanco(myConn, lblInfo, Inserta, txtCodigo.Text, TxtDescripcion.Text, txtAdquisicion.Text, txtInicioValuacion.Text, txtDireccion.Text, _
        '    txtTelef1.Text, txtTelef2.Text, txtFax.Text, txtEmail.Text, txtWeb.Text, txtContacto.Text, _
        '    cmbTitulo.SelectedItem, ValorNumero(IIf(txtComision.Text = "", "0.00", txtComision.Text)), CDate(txtIngreso.Text), _
        '    ValorNumero(IIf(txtFecha.Text = "", "0.00", txtFecha.Text)), _
        '    txtCuentaContable.Text, txtFormato.Text, cmbCondicion.SelectedIndex)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" CODIGO = '" & txtCodigo.Text & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionCat = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionCat
        AsignaTXT(nPosicionCat)
        DesactivarMarco0()
        tbcActivos.SelectedTab = C1DockingTabPage1
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub txt_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, txtDescripcion.GotFocus, _
        txtAdquisicion.GotFocus, txtInicioValuacion.GotFocus, txtGrupo.GotFocus, txtSerial.GotFocus, txtMoneda.GotFocus, txtUbica.GotFocus, _
        txtAños.GotFocus, txtMeses.GotFocus, txtTasa.GotFocus, txtSalvamento.GotFocus, txtAdquisicionMonto.GotFocus, btnIngreso.GotFocus, _
        btnGrupo.GotFocus, btnMoneda.GotFocus, btnUbica.GotFocus, btnCuentaActivos.GotFocus, btnCuentaGastos.GotFocus, btnCuentaRepreciacion.GotFocus, _
        btnInicioValuacion.GotFocus, btnFechaAdquisicion.GotFocus

        ft.colocaMensajeEnEtiqueta(sender, jytsistema.WorkLanguage, lblInfo)

    End Sub


    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
        dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTablaMovimientos).Position = e.RowIndex
        nPosicionMov = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarra, e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
    End Sub

    Private Sub tbcActivos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcActivos.SelectedIndexChanged

        'Dim aObj() As Object = {btnDepositarEfectivo, btnDepositarTarjetas, btnDepositarCestaTicket, btnReposicion}

        If tbcActivos.SelectedIndex = 0 Then
            '    AsignaTXT(nPosicionCat)
            HabilitarBotonesAdicionalesBarra(False)
        Else 'movimientos
            '    AbrirMovimientos(txtCodigo1.Text)
            '    AsignaMov(nPosicionMov, True)
            '    dg.Enabled = True
            HabilitarBotonesAdicionalesBarra(True)
        End If

    End Sub

    Private Sub btnDepositarEfectivo_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs)
        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        If tbcActivos.SelectedTab.Text = "Movimientos" Then
            Dim f As New jsBanArcDepositarCajaPlus
            f.Depositar(myConn, ds, txtCodigo.Text, e.ClickedItem.Text, 0)
            AsignaMov(nPosicionMov, True)
            f = Nothing
        End If
    End Sub
    Private Sub btnDepositarTarjetas_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs)
        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        If tbcActivos.SelectedTab.Text = "Movimientos" Then
            Dim f As New jsBanArcDepositarCajaPlus
            f.Depositar(myConn, ds, txtCodigo.Text, e.ClickedItem.Text, 1)
            AsignaMov(nPosicionMov, True)
            f = Nothing
        End If
    End Sub
    Private Sub btnDepositarCestaTicket_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs)
        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        If tbcActivos.SelectedTab.Text = "Movimientos" Then
            Dim f As New jsBanArcDepositarCajaPlus
            f.Depositar(myConn, ds, txtCodigo.Text, "", 2, e.ClickedItem.Text)
            AsignaMov(nPosicionMov, True)
            f = Nothing
        End If
    End Sub
    Private Sub btnReposicion_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs)
        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        If tbcActivos.SelectedTab.Text = "Movimientos" Then
            Dim f As New jsBanArcReposicionCaja
            f.Cargar(myConn, ds, dtMovimientos, e.ClickedItem.Text, txtCodigo.Text)
            AsignaMov(nPosicionMov, True)
            f = Nothing
        End If
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If dt.Rows.Count = 0 Then
            IniciarActivo(False)
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


    Private Sub txtAdquisicionMonto_Click(sender As Object, e As EventArgs) Handles txtAdquisicionMonto.Click, _
        txtDescripcion.Click, txtAños.Click, txtMeses.Click, txtSerial.Click, txtTasa.Click, txtSalvamento.Click
        ft.enfocarTexto(sender)
    End Sub
End Class