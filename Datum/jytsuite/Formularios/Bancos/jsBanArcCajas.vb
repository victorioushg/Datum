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
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long

    Private Sub jsBanArcCajas_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsBanCajas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo
        Try
            myConn.Open()

            dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionEncab = 0
                AsignaTXT(nPosicionEncab)
            Else
                IniciarCaja(False)
            End If
            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            AsignarTooltips()
            IniciarCajasyCestatickets()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnAgregar, btnEditar, btnEliminar, btnBuscar, btnPrimero, btnSiguiente, _
                          btnAnterior, btnUltimo, btnImprimir, btnSalir, btnRemesas, btnAdelantoEfectivo)

        'Menu barra renglón
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnAgregarMovimiento, btnEditarMovimiento, btnEliminarMovimiento, _
                          btnBuscarMovimiento, btnPrimerMovimiento, btnAnteriorMovimiento, btnSiguienteMovimiento, _
                          btnUltimoMovimiento)

        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnCodigoContable)

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
        dtMovimientos = ft.MostrarFilaEnTabla(myConn, ds, nTablaMovimientos, strSQLMov, Me.BindingContext, MenuBarraRenglon, _
                               dg, lRegion, jytsistema.sUsuario, nRow, Actualiza)

        AsignaSaldos(txtCodigo.Text)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt
            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)
                'Bancos 
                txtCodigo.Text = ft.muestraCampoTexto(.Item("caja"))
                txtNombre.Text = ft.muestraCampoTexto(.Item("nomcaja"))
                txtCodigoContable.Text = ft.muestraCampoTexto(.Item("codcon"))

                'Movimientos

                strSQLMov = "select * from jsbantracaj " _
                    & " where " _
                    & " caja  = '" & .Item("caja") & "' and " _
                    & " deposito = '' and " _
                    & " fecha <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
                    & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                    & " id_emp = '" & jytsistema.WorkID & "' order by fecha desc, tipomov, nummov "

                dtMovimientos = ft.AbrirDataTable(ds, nTablaMovimientos, myConn, strSQLMov)

                Dim aCampos() As String = {"fecha.FECHA.80.C.fecha", _
                                           "tipomov.TP.30.C.", _
                                           "nummov.Documento.100.I.", _
                                           "formpag.FP.25.C.", _
                                           "numpag.Nº Pago.75.I.", _
                                           "refpag.Referencia Pago.75.I.", _
                                           "Importe.Importe.120.D.Numero", _
                                           "origen.ORG.30.C.", _
                                           "prov_cli.Prov/Cli.80.I.", _
                                           "codven.Asesor.50.C.", _
                                           "concepto.Concepto.250.I.", _
                                           "sada..100.I."}
                ft.IniciarTablaPlus(dg, dtMovimientos, aCampos)

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

        txtEF.Text = ft.muestraCampoNumero(CalculaSaldoCajaPorFP(myConn, numCaja, "EF", lblInfo))
        txtCH.Text = ft.muestraCampoNumero(CalculaSaldoCajaPorFP(myConn, numCaja, "CH", lblInfo))
        txtTA.Text = ft.muestraCampoNumero(CalculaSaldoCajaPorFP(myConn, numCaja, "TA", lblInfo))
        txtCT.Text = ft.muestraCampoNumero(CalculaSaldoCajaPorFP(myConn, numCaja, "CT", lblInfo))
        txtOT.Text = ft.muestraCampoNumero(CalculaSaldoCajaPorFP(myConn, numCaja, "OT", lblInfo))
        txtSaldo.Text = ft.muestraCampoNumero(CalculaSaldoCajaPorFP(myConn, numCaja, "", lblInfo))

    End Sub

    Private Sub IniciarCaja(ByVal Inicio As Boolean)
        If Inicio Then
            txtCodigo.Text = ft.autoCodigo(myConn, "caja", "jsbanenccaj", "id_emp", jytsistema.WorkID, 2, True)
        Else
            txtCodigo.Text = ""
        End If

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNombre, txtCodigoContable)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtSaldo, txtEF, txtCH, txtTA, txtCT, txtOT)

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        dg.Columns.Clear()
    End Sub
    Private Sub ActivarMarco0()

        ft.visualizarObjetos(True, grpAceptarSalir)
        ft.habilitarObjetos(True, True, txtNombre, btnCodigoContable)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub


    Private Sub DesactivarMarco0()

        ft.visualizarObjetos(False, grpAceptarSalir)
        ft.habilitarObjetos(False, True, txtCodigo, txtCH, txtCodigoContable, txtCT, txtEF, txtNombre, txtOT, txtSaldo, txtTA, btnCodigoContable)

        MenuBarra.Enabled = True
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If dt.Rows.Count = 0 Then
            IniciarCaja(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            AsignaTXT(nPosicionEncab)
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
            ft.MensajeCritico("INDIQUE NOMBRE DE CAJA VALIDO...")
            Return False
        End If

        If txtCodigoContable.Text.Trim = "" Then
            ft.MensajeCritico("INDIQUE CODIGO CONTABLE CAJA VALIDO...")
            Return False
        End If

        If CBool(ParametroPlus(myConn, Gestion.iBancos, "BANPARAM17")) Then
            If txtCodigoContable.Text.Trim = "" Then
                ft.MensajeCritico("Debe indicar una CUENTA CONTABLE VALIDA...")
                Return False
            Else
                If ft.DevuelveScalarEntero(myConn, " Select count(*) from jscotcatcon where codcon = '" & txtCodigoContable.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
                    ft.mensajeCritico(" Debe indicar una CUENTA CONTABLE Válida...")
                    Return False
                End If
                If ft.DevuelveScalarEntero(myConn, " select count(*) from jsbanenccaj where codcon = '" & txtCodigoContable.Text & "' and caja <> '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ") > 0 Then
                    ft.mensajeCritico(" La cuenta contable YA ha sido asignada a otra CAJA ... ")
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
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombre.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique el nombre de la caja ... ", Transportables.TipoMensaje.iInfo)
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
        Dim Anchos() As Integer = {100, 2500}
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
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaMovimientos).Rows.Count)
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
        With dtMovimientos.Rows(nPosicionRenglon)

            Dim aCamposAdicionales() As String = {"CAJA|'" & txtCodigo.Text & "'", _
                                                  "RENGLON|'" & .Item("RENGLON") & "'", _
                                                  "FECHA|'" & ft.FormatoFechaMySQL(Convert.ToDateTime(.Item("FECHA"))) & "'", _
                                                  "TIPOMOV|'" & .Item("TIPOMOV") & "'", _
                                                  "ORIGEN|'" & .Item("ORIGEN") & "'", _
                                                  "NUMMOV|'" & .Item("NUMMOV") & "'"}

            If DocumentoBloqueado(myConn, "jsbantracaj", aCamposAdicionales) Then
                ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
            Else
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

            End If
        End With

    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click

        EliminarMovimiento()

    End Sub
    Private Sub EliminarMovimiento()

        nPosicionRenglon = Me.BindingContext(ds, nTablaMovimientos).Position

        With dtMovimientos.Rows(nPosicionRenglon)

            Dim aCamposAdicionales() As String = {"CAJA|'" & txtCodigo.Text & "'", _
                                                  "RENGLON|'" & .Item("RENGLON") & "'", _
                                                  "FECHA|'" & ft.FormatoFechaMySQL(Convert.ToDateTime(.Item("FECHA"))) & "'", _
                                                  "TIPOMOV|'" & .Item("TIPOMOV") & "'", _
                                                  "ORIGEN|'" & .Item("ORIGEN") & "'", _
                                                  "NUMMOV|'" & .Item("NUMMOV") & "'"}

            If DocumentoBloqueado(myConn, "jsbantracaj", aCamposAdicionales) Then
                ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
            Else

                If nPosicionRenglon >= 0 Then
                    If dtMovimientos.Rows(nPosicionRenglon).Item("origen") = "CAJ" Then

                        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

                            InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dtMovimientos.Rows(nPosicionRenglon).Item("nummov"))

                            ft.Ejecutar_strSQL(myConn, "DELETE FROM jsbantracaj where " _
                                & " RENGLON = '" & dtMovimientos.Rows(nPosicionRenglon).Item("renglon") & "' AND " _
                                & " ID_EMP = '" & jytsistema.WorkID & "'")

                            ft.Ejecutar_strSQL(myConn, " update jsbanenccaj set saldo = " & CalculaSaldoCajaPorFP(myConn, dtMovimientos.Rows(nPosicionRenglon).Item("caja"), "", lblInfo) & " where caja = '" & dtMovimientos.Rows(nPosicionRenglon).Item("caja") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaMovimientos, lblInfo)
                            dtMovimientos = ds.Tables(nTablaMovimientos)
                            If dtMovimientos.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtMovimientos.Rows.Count - 1
                            AsignaMov(nPosicionRenglon, True)

                        End If
                    Else
                        ft.mensajeCritico("Movimiento proveniente de " & dtMovimientos.Rows(nPosicionRenglon).Item("origen") & ". ")
                    End If

                End If
            End If
        End With

    End Sub
    Private Sub btnBuscarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarMovimiento.Click

        Dim f As New frmBuscar
        Dim Campos() As String = {"fecha", "nummov", "formpag", "numpag", "refpag", "prov_cli", "codven", "importe"}
        Dim Nombres() As String = {"Emisión", "Nº Movimiento", "Forma Pago", "Número Pago", "Referencia Pago", "Proveedor/Cliente", "Asesor Comercial", "Importe"}
        Dim Anchos() As Integer = {140, 140, 100, 150, 150, 150, 120, 150}
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

    Private Sub btnCodCon_Click(sender As System.Object, e As System.EventArgs) Handles btnCodigoContable.Click
        txtCodigoContable.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codcon codigo, descripcion from jscotcatcon where marca = 0 and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Cuentas Contables", _
                                                   txtCodigoContable.Text)
    End Sub
End Class