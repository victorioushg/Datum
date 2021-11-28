Imports MySql.Data.MySqlClient
Imports ReportesDeBancos
Imports fTransport
Imports Syncfusion.WinForms.Input
Imports dgField = fTransport.Models.DataGridField
Imports dgFieldSF = fTransport.Models.SfDataGridField
Imports fTransport.Models
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
    Private accountList As New List(Of AccountBase)
    Private account As New AccountBase
    Private bankTransactionList As New List(Of BankLine)
    Private bankTransaction As New BankLine

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
            IniciarControles()

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
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub IniciarControles()

        accountList = InitiateDropDown(Of AccountBase)(myConn, cmbCC)
        'accountList.Where(Function(c) c.CodigoContable)

        InitiateDropDownInterchangeCurrency(myConn, cmbMonedas, jytsistema.sFechadeTrabajo)
        cmbMonedas.SelectedValue = jytsistema.WorkCurrency.Id

    End Sub

    Private Sub HabilitarBotonesAdicionalesBarra(ByVal Habilitar As Boolean)
        ft.habilitarObjetos(Habilitar, False, btnDepositarEfectivo, btnDepositarTarjetas, btnDepositarCestaTicket, btnReposicion)
    End Sub
    Private Sub AsignarTooltips()
        Dim menus As New List(Of ToolStrip) From {MenuBarra}
        AsignarToolTipsMenuBarraToolStrip(menus, "Banco")
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

        If Actualiza Then
            Cursor.Current = Cursors.WaitCursor
            bankTransactionList = GetBankLines(myConn, txtCodigo1.Text)
            dg.DataSource = bankTransactionList
            dg.Refresh()
        End If
        dg.Rows().Item(nRow).Selected = True
        MostrarItemsEnMenuBarra(MenuBarra, CInt(nRow), bankTransactionList.Count)
        nPosicionMov = dg.SelectedRows(0).Index
        bankTransaction = bankTransactionList.Item(nPosicionMov)

        ' Colocar Saldos 
        Dim Saldo As Decimal = Math.Round(bankTransactionList.Sum(Function(s) s.ImporteReal), 2)
        txtSaldo.Text = ft.FormatoNumero(Saldo)
        txtSaldo1.Text = txtSaldo.Text

    End Sub
    Private Sub AsignaTar(ByVal nRow As Long, ByVal Actualiza As Boolean)
        dtTarjetas = ft.MostrarFilaEnTabla(myConn, ds, nTablaTarjetas, strSQLTar, Me.BindingContext, MenuComisiones,
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
                    cmbCC.SelectedValue = .Item("codcon")
                    txtContacto.Text = ft.muestraCampoTexto(.Item("contacto"))
                    txtComision.Text = ft.muestraCampoNumero(.Item("comision"))
                    txtIngreso.Value = .Item("fechacrea")
                    txtSaldo.Text = ft.muestraCampoNumero(CalculaSaldoBanco(myConn, lblInfo, .Item("codban")))
                    txtFormato.Text = ft.muestraCampoTexto(.Item("formato"))
                    cmbCondicion.SelectedIndex = .Item("estatus")

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

        Cursor.Current = Cursors.WaitCursor
        ''Movimientos
        bankTransactionList = GetBankLines(myConn, CodigoBanco)
        Dim aCampos As List(Of dgField) = New List(Of dgField)() From {
            New dgField("FechaMovimiento", "FECHA", 80, DataGridViewContentAlignment.MiddleCenter, FormatoFecha.Corta),
            New dgField("TipoMovimiento", "TP", 30, DataGridViewContentAlignment.MiddleCenter, ""),
            New dgField("NumeroMovimiento", "Documento", 120, DataGridViewContentAlignment.MiddleLeft, ""),
            New dgField("Concepto", "Concepto", 350, DataGridViewContentAlignment.MiddleLeft, ""),
            New dgField("Importe", "Importe", 120, DataGridViewContentAlignment.MiddleRight, FormatoNumero.FormatoNumero),
            New dgField("CodigoIso", "Moneda", 50, DataGridViewContentAlignment.MiddleCenter, ""),
            New dgField("ImporteReal", "Importe Real", 120, DataGridViewContentAlignment.MiddleRight, FormatoNumero.FormatoNumero),
            New dgField("Origen", "ORG", 30, DataGridViewContentAlignment.MiddleCenter, ""),
            New dgField("ProveedorCliente", "Prov/Cli", 120, DataGridViewContentAlignment.MiddleLeft, ""),
            New dgField("CodigoVendedor", "Asesor", 50, DataGridViewContentAlignment.MiddleCenter, ""),
            New dgField("Comprobante", "Numero Comprobante", 120, DataGridViewContentAlignment.MiddleLeft, ""),
            New dgField("Sada", "", 100, DataGridViewContentAlignment.MiddleLeft, "")
        }
        ft.IniciarTablaListPlus(Of BankLine)(dg, bankTransactionList, aCampos)

        If bankTransactionList.Count > 0 Then
            nPosicionMov = 0
            AsignaMov(nPosicionMov, False)
        Else

        End If


    End Sub
    Private Sub IniciarBanco(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = ft.autoCodigo(myConn, "CODBAN", "jsbancatban", "id_emp", jytsistema.WorkID, 5, True)
        Else
            txtCodigo.Text = ""
        End If
        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNombre, txtCtaBan, txtAgencia, txtDireccion, txtTelef1,
                            txtTelef2, txtFax, txtEmail, txtWeb, txtContacto, txtComision, txtFormato, txtCodigo1,
                            txtNombre1, txtCtaBan1, cmbCC)

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
                    txtFax, txtEmail, txtWeb, txtContacto, cmbCC, cmbMonedas,
                    txtComision, btnFormatos, cmbCondicion)

        ft.habilitarObjetos(True, False, MenuComisiones)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False

        ft.habilitarObjetos(True, False, C1DockingTabPage2)
        ft.habilitarObjetos(False, True, txtCodigo, txtCodigo1, txtNombre, txtNombre1, txtCtaBan, txtCtaBan1, txtSaldo,
                            txtAgencia, txtDireccion, txtTelef1, txtTelef2, txtFax, txtEmail, txtWeb, txtContacto,
                            cmbCC, cmbMonedas, txtIngreso, txtComision, txtFormato, btnFormatos,
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
            Dim nRow As Integer = dg.Rows.GetFirstRow(DataGridViewElementStates.None)
            AsignaMov(nRow, False)
        End If
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        If tbcBancos.SelectedTab.Text = "Bancos" Then
            Me.BindingContext(ds, nTabla).Position -= 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position)
        Else
            Dim nRow As Integer = dg.Rows.GetPreviousRow(dg.SelectedRows(0).Index, DataGridViewElementStates.None)
            AsignaMov(IIf(nRow < 0, 0, nRow), False)
        End If
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        If tbcBancos.SelectedTab.Text = "Bancos" Then
            Me.BindingContext(ds, nTabla).Position += 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position)
        Else
            Dim nRow As Integer = dg.Rows.GetNextRow(dg.SelectedRows(0).Index, DataGridViewElementStates.None)
            AsignaMov(IIf(nRow < 0, 0, nRow), False)
        End If

    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        If tbcBancos.SelectedTab.Text = "Bancos" Then
            Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position)
        Else
            Dim nRow As Integer = dg.Rows.GetLastRow(DataGridViewElementStates.None)
            AsignaMov(nRow, False)
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
                f.Apuntador = nPosicionMov
                f.Agregar(myConn, ds, dtMovimientos, txtCodigo.Text, cmbMonedas.SelectedValue)
                AsignaMov(f.Apuntador, IIf(f.Apuntador >= 0, True, False))
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
            If bankTransaction.Origen = "BAN" Then
                If bankTransaction.FechaBloqueo > FechaMinimaBloqueo Then
                    ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                Else
                    Dim f As New jsBanArcBancosMovimientosPlus
                    f.Apuntador = nPosicionMov
                    f.Editar(myConn, ds, dtMovimientos, txtCodigo.Text)
                    AsignaMov(f.Apuntador, IIf(f.Apuntador >= 0, True, False))
                    f = Nothing
                End If
            End If
        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If tbcBancos.SelectedTab.Text = "Bancos" Then
            EliminaBanco()
        Else
            If cmbCondicion.Text = "Activo" Then
                If bankTransaction.FechaBloqueo > FechaMinimaBloqueo Then
                    ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
                Else
                    EliminarMovimiento(bankTransaction)
                End If
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

                AsignaTXT(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsbancatban", strSQL, aCamposDel, aStringsDel,
                                              Me.BindingContext(ds, nTabla).Position, True))

            Else
                ft.mensajeCritico("Este banco posee movimientos. Verifique por favor ...")
            End If
        End If

    End Sub
    Private Sub EliminarMovimiento(transaction As BankLine)

        Dim nPosicionMovi As Long
        nPosicionMovi = nPosicionMov
        If nPosicionMov >= 0 Then
            If CBool(transaction.Conciliado) Then
                ft.mensajeCritico("MOVIMIENTO CONCILIADO. NO ES POSIBLE ELIMINAR!!!!!")
                Return
            End If
            If transaction.Origen = "BAN" Then
                If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, transaction.NumeroMovimiento)
                    Dim DescripcionIDB As String = Convert.ToString(ParametroPlus(myConn, Gestion.iBancos, "BANPARAM04"))

                    ' Elimina si provienes de una transferencia bancaria
                    If transaction.NumeroOrigen.Substring(0, 2) = "TB" Then
                        Dim sResp As Microsoft.VisualBasic.MsgBoxResult = MsgBox("¿ MOVIMIENTO PROVIENE DE UNA TRANSFERENCIA BANCARIA, POR LO TANTO SE ELIMINARA EN TODOS LOS BANCOS. DESEA CONTINUAR ?", MsgBoxStyle.YesNo, "TRANSFERENCIA... ")
                        If sResp = MsgBoxResult.Yes Then

                            ft.Ejecutar_strSQL(myConn, "DELETE from jsbantraban where " _
                                    & " NUMDOC = '" & transaction.NumeroMovimiento & "' and " _
                                    & " NUMORG = '" & transaction.NumeroOrigen & "' and " _
                                    & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
                                    & " ID_EMP = '" & jytsistema.WorkID & "'")

                            ft.Ejecutar_strSQL(myConn, " delete from jsbanordpag where " _
                                    & " comproba = '" & transaction.Comprobante & "' and  " _
                                    & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                    & " ID_EMP = '" & jytsistema.WorkID & "' ")

                        End If
                    End If


                    EliminarImpuestoDebitoBancario(myConn, lblInfo, transaction.CodigoBanco,
                                                   transaction.NumeroMovimiento, transaction.FechaMovimiento)

                    ft.Ejecutar_strSQL(myConn, "DELETE FROM jsbantracaj where " _
                            & " REFPAG = '" & transaction.CodigoBanco & "' AND " _
                            & " FECHA = '" & ft.FormatoFechaMySQL(transaction.FechaMovimiento) & "' AND " _
                            & " NUMMOV = '" & transaction.NumeroMovimiento & "' AND " _
                            & " TIPOMOV = 'EN' AND FORMPAG = '" & transaction.TipoMovimiento & "' AND" _
                            & " NUMPAG = '" & transaction.NumeroMovimiento & "' AND " _
                            & " IMPORTE = " & -1 * transaction.Importe & " AND " _
                            & " CAJA = '" & transaction.CodigoCaja & "' AND " _
                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'")

                    ft.Ejecutar_strSQL(myConn, "DELETE from jsbantraban where " _
                            & " CODBAN = '" & transaction.CodigoBanco & "' and " _
                            & " FECHAMOV = '" & ft.FormatoFechaMySQL(transaction.FechaMovimiento) & "' AND " _
                            & " NUMDOC = '" & transaction.NumeroMovimiento & "' and " _
                            & " NUMORG = '" & transaction.NumeroOrigen & "' and " _
                            & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'")

                    If transaction.TipoMovimiento = "CH" Then _
                            ft.Ejecutar_strSQL(myConn, " delete from jsbanordpag where " _
                            & " comproba = '" & transaction.Comprobante & "' and  " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " ID_EMP = '" & jytsistema.WorkID & "' ")

                    If transaction.TipoOrigen = "CH" And transaction.TipoMovimiento = "ND" Then

                        ft.Ejecutar_strSQL(myConn, "Delete from jsventracob where " _
                               & " TIPOMOV = 'ND' AND " _
                               & " NUMORG = '" & transaction.NumeroOrigen & "' AND " _
                               & " ORIGEN = 'BAN' AND " _
                               & " EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
                               & " ID_EMP = '" & jytsistema.WorkID & "'")

                        ft.Ejecutar_strSQL(myConn, "DELETE FROM jsbanchedev where " _
                               & " NUMCHEQUE = '" & transaction.NumeroOrigen & "' AND " _
                               & " ID_EMP = '" & jytsistema.WorkID & "' ")

                    End If

                    If transaction.Comprobante.Length > 0 AndAlso transaction.Comprobante.Substring(0, 2) = "CP" Then
                        ft.Ejecutar_strSQL(myConn, " delete from jsbantracaj where tipomov = 'EN' and nummov = '" & transaction.Comprobante & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        ft.Ejecutar_strSQL(myConn, " update jsbantracaj set deposito = '' where deposito = '" & transaction.Comprobante & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    End If

                    ft.Ejecutar_strSQL(myConn, " update jsbanenccaj set saldo = " & CalculaSaldoCajaPorFP(myConn, transaction.CodigoCaja, "", lblInfo) _
                                       & " where caja = '" & transaction.CodigoCaja & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    ft.Ejecutar_strSQL(myConn, " update jsbancatban set saldoact = " & CalculaSaldoBanco(myConn, lblInfo, transaction.CodigoBanco) _
                                       & " where codban = '" & transaction.CodigoBanco & "' and id_emp = '" & jytsistema.WorkID & "' ")


                    If bankTransactionList.Count - 1 < nPosicionMovi Then nPosicionMovi = bankTransactionList.Count - 1
                    AsignaMov(nPosicionMovi, False)

                End If
            Else
                If transaction.Origen = "CAJ" Then
                    'NumeroOrigen = IIf(IsDBNull(.Item("numorg")), "", .Item("numorg"))
                    'sDeposito = .Item("numdoc")
                    'TipoOrigen = .Item("tiporg")

                    If ft.PreguntaEliminarRegistro(" Movimiento proveniente de caja, estos se deshabilitarán en la caja!!!") = Windows.Forms.DialogResult.Yes Then

                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, transaction.NumeroMovimiento)

                        ft.Ejecutar_strSQL(myConn, "DELETE from jsbantraban where " _
                                & " CODBAN = '" & transaction.CodigoBanco & "' and  " _
                                & " FECHAMOV = '" & ft.FormatoFechaMySQL(transaction.FechaMovimiento) & "' and " _
                                & " NUMDOC = '" & transaction.NumeroMovimiento & "' and " _
                                & " EJERCICIO = '" & jytsistema.WorkExercise & "' and" _
                                & " ID_EMP = '" & jytsistema.WorkID & "'")

                        ft.Ejecutar_strSQL(myConn, "UPDATE jsbantracaj SET " _
                                & "DEPOSITO = '' " _
                                & "where " _
                                & "DEPOSITO = '" & transaction.NumeroMovimiento & "' and " _
                                & "EJERCICIO = '" & jytsistema.WorkExercise & "' and " _
                                & "ID_EMP = '" & jytsistema.WorkID & "'")

                        ft.Ejecutar_strSQL(myConn, "UPDATE jsventabtic SET " _
                                & "NUMDEP = '', FECHADEP = '0000-00-00', BANCODEP = '' " _
                                & "WHERE " _
                                & "NUMDEP = '" & transaction.NumeroMovimiento & "' AND " _
                                & "BANCODEP = '" & transaction.CodigoBanco & "' and " _
                                & "ID_EMP = '" & jytsistema.WorkID & "'")

                        If transaction.TipoOrigen = "CT" Then ft.Ejecutar_strSQL(myConn, " delete from jsproencgas where " _
                               & " NUMGAS = '" & transaction.NumeroOrigen & "' AND " _
                               & " ID_EMP = '" & jytsistema.WorkID & "'")

                        ft.Ejecutar_strSQL(myConn, " update jsbanenccaj set saldo = " & CalculaSaldoCajaPorFP(myConn, transaction.CodigoCaja, "", lblInfo) _
                                           & " where caja = '" & transaction.CodigoCaja & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        ft.Ejecutar_strSQL(myConn, " update jsbancatban set saldoact = " & CalculaSaldoBanco(myConn, lblInfo, transaction.CodigoBanco) _
                                           & " where codban = '" & transaction.CodigoBanco & "' and id_emp = '" & jytsistema.WorkID & "' ")

                        If bankTransactionList.Count - 1 < nPosicionMovi Then nPosicionMovi = bankTransactionList.Count - 1
                        AsignaMov(nPosicionMov, True)

                    End If
                Else
                    ft.mensajeCritico("Movimiento proveniente de " & transaction.Origen & ". ")
                End If
            End If
        End If

    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        If tbcBancos.SelectedTab.Text = "Bancos" Then
            Dim f As New frmBuscar
            Dim Campos() As String = {"codban", "nomban"}
            Dim Nombres() As String = {"Código Banco", "Nombre Banco"}
            Dim Anchos() As Integer = {100, 2500}
            f.Text = "Bancos"
            f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, " Bancos...")
            AsignaTXT(f.Apuntador)
            f = Nothing
        Else
            Dim aCampos As List(Of dgFieldSF) = New List(Of dgFieldSF)() From {
                New dgFieldSF(TypeColumn.DateTimeColumn, "FechaMovimiento", "Fecha Movimiento   ", 80, HorizontalAlignment.Center, FormatoFecha.Corta),
                New dgFieldSF(TypeColumn.TextColumn, "NumeroMovimiento", "Documento   ", 120, HorizontalAlignment.Left, ""),
                New dgFieldSF(TypeColumn.TextColumn, "Concepto", "Concepto   ", 350, HorizontalAlignment.Left, ""),
                New dgFieldSF(TypeColumn.TextColumn, "ProveedorCliente", "Proveedor/Cliente  ", 350, HorizontalAlignment.Left, "")
            }
            Dim f As New frmBuscarPlus
            f.Buscar(bankTransactionList, aCampos, "Movimientos de Bancos")
            If f.Id > 0 Then
                Dim index = bankTransactionList.FindIndex(Function(item) item.Id = f.Id)
                SetSelectedRowByIndex(dg, index)
            End If
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
            If cmbCC.SelectedValue.Text.Trim = "" Then
                ft.mensajeCritico("Debe indicar una CUENTA CONTABLE VALIDA...")
                Return False
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
            "", ValorNumero(IIf(txtComision.Text = "", "0.00", txtComision.Text)), txtIngreso.Value,
            ValorNumero(IIf(txtSaldo.Text = "", "0.00", txtSaldo.Text)),
            cmbCC.SelectedValue, txtFormato.Text, cmbCondicion.SelectedIndex, cmbMonedas.SelectedValue, jytsistema.sFechadeTrabajo)

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

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus, txtNombre.GotFocus,
        txtCtaBan.GotFocus, txtAgencia.GotFocus, txtDireccion.GotFocus, txtTelef1.GotFocus, txtTelef2.GotFocus, txtFax.GotFocus, txtEmail.GotFocus,
        txtWeb.GotFocus, txtComision.GotFocus, cmbCondicion.GotFocus, btnFormatos.GotFocus

        ft.colocaMensajeEnEtiqueta(sender, jytsistema.WorkLanguage, lblInfo)

    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick,
        dg.CellMouseClick, dg.RegionChanged
        AsignaMov(e.RowIndex, False)
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
        '   nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        If tbcBancos.SelectedTab.Text = "Movimientos" Then
            Dim f As New jsBanArcDepositarCajaPlus
            f.Depositar(myConn, ds, txtCodigo.Text, e.ClickedItem.Text, 0)
            AsignaMov(nPosicionMov, True)
            f = Nothing
        End If
    End Sub
    Private Sub btnDepositarTarjetas_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles btnDepositarTarjetas.DropDownItemClicked
        '  nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        If tbcBancos.SelectedTab.Text = "Movimientos" Then
            Dim f As New jsBanArcDepositarCajaPlus
            f.Depositar(myConn, ds, txtCodigo.Text, e.ClickedItem.Text, 1)
            AsignaMov(nPosicionMov, True)
            f = Nothing
        End If
    End Sub
    Private Sub btnDepositarCestaTicket_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles btnDepositarCestaTicket.DropDownItemClicked
        ' nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        If tbcBancos.SelectedTab.Text = "Movimientos" Then
            Dim f As New jsBanArcDepositarCajaPlus
            f.Depositar(myConn, ds, txtCodigo.Text, "", 2, e.ClickedItem.Text)
            AsignaMov(nPosicionMov, True)
            f = Nothing
        End If
    End Sub
    Private Sub btnReposicion_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles btnReposicion.DropDownItemClicked
        'nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
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
        ' Do not delete
    End Sub
    Private Sub btnDepositarCestaTicket_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDepositarCestaTicket.Click
        ' do not delete
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
                    AsignaTar(EliminarRegistros(myConn, lblInfo, ds, nTablaTarjetas, "jsbancatbantar",
                                                strSQLTar, aCamDel, aStrDel, nPosicionTar), True)

                End If
            End If
        End With

    End Sub

    Private Sub dgTar_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgTar.RowHeaderMouseClick,
        dgTar.CellMouseClick, dgTar.RegionChanged
        Me.BindingContext(ds, nTablaTarjetas).Position = e.RowIndex
    End Sub

    Private Sub dg_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dg.DataBindingComplete
        '' 
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub txtComision_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtComision.KeyPress
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub
    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp
        'Select Case e.KeyCode
        '    Case Keys.Down
        '        Me.BindingContext(ds, nTablaMovimientos).Position += 1
        '        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        '        AsignaMov(nPosicionMov, False)
        '    Case Keys.Up
        '        Me.BindingContext(ds, nTablaMovimientos).Position -= 1
        '        nPosicionMov = Me.BindingContext(ds, nTablaMovimientos).Position
        '        AsignaMov(nPosicionMov, False)
        'End Select                                                 
    End Sub
End Class