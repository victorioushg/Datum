Imports MySql.Data.MySqlClient
Imports dgField = fTransport.Models.DataGridField
Imports dgFieldSF = fTransport.Models.SfDataGridField
Imports fTransport.Models
Public Class jsBanArcCajas
    Private Const sModulo As String = "Cajas"
    Private Const lRegion As String = ""
    Private Const nTablaMovimientos As String = "movimientos_caja"

    Private strSQLMov As String

    Private myConn As New MySqlConnection(jytsistema.strConn)

    Private ds As New DataSet()
    'Private dtMovimientos As New DataTable
    Private ft As New Transportables

    Private cashBoxList As New List(Of Saving)
    Private accountList As New List(Of AccountBase)
    Private savingTransactionList As New List(Of SavingLines)
    Private cashBox As New Saving()
    Private account As New AccountBase()
    Private savingTransaction As New SavingLines()

    Private i_modo As Integer

    Private posicionEncab As Integer = 0
    Private posicionRenglon As Integer = 0

    Private Sub jsBanArcCajas_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsBanCajas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Me.Tag = sModulo
        Try
            myConn.Open()

            IniciarControles()
            DesactivarMarco0()

            If cashBoxList.Count = 0 Then
                IniciarCaja(False)
            Else
                AsignaTXT(cashBoxList.FirstOrDefault)
            End If

            '' ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            AsignarTooltips()
            IniciarCajasyCestatickets()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub IniciarControles()

        cashBoxList = GetCashBoxes(myConn)
        accountList = InitiateDropDown(Of AccountBase)(myConn, cmbCC)
        'accountList.Where(Function(c) c.CodigoContable)

        InitiateDropDownInterchangeCurrency(myConn, cmbMonedas, jytsistema.sFechadeTrabajo)
        cmbMonedas.SelectedValue = jytsistema.WorkCurrency.Id

    End Sub
    Private Sub AsignarTooltips()
        Dim menus As New List(Of ToolStrip) From {MenuBarra, MenuBarraRenglon}
        AsignarToolTipsMenuBarraToolStrip(menus, "Caja")
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
        If Actualiza Then
            Cursor.Current = Cursors.WaitCursor
            savingTransactionList = GetSavingLines(myConn, cashBox.Codigo)
            dg.DataSource = savingTransactionList
        End If
        dg.Rows().Item(nRow).Selected = True
        MostrarItemsEnMenuBarra(MenuBarraRenglon, CInt(nRow), savingTransactionList.Count)
        posicionRenglon = dg.SelectedRows(0).Index
        savingTransaction = savingTransactionList.Item(posicionRenglon)
        AsignaSaldos(cashBox.Codigo)
    End Sub


    Private Sub AsignaTXT(Caja As Saving)

        MostrarItemsEnMenuBarra(MenuBarra, cashBoxList.IndexOf(Caja), cashBoxList.Count)
        cashBox = Caja

        posicionEncab = cashBoxList.IndexOf(Caja)
        txtCodigo.Text = cashBox.Codigo
        txtNombre.Text = cashBox.Descripcion
        cmbCC.SelectedValue = cashBox.CodigoContable

        Cursor.Current = Cursors.WaitCursor
        ''Movimientos
        savingTransactionList = GetSavingLines(myConn, Caja.Codigo)
        Dim aCampos As List(Of dgField) = New List(Of dgField)() From {
            New dgField("Fecha", "FECHA", 80, DataGridViewContentAlignment.MiddleCenter, FormatoFecha.FormatoFecha),
            New dgField("TipoMovimiento", "TP", 30, DataGridViewContentAlignment.MiddleCenter, ""),
            New dgField("NumeroMovimiento", "Documento", 120, DataGridViewContentAlignment.MiddleLeft, ""),
            New dgField("FormaDePago", "FP", 35, DataGridViewContentAlignment.MiddleCenter, ""),
            New dgField("NumeroDePago", "No Pago", 120, DataGridViewContentAlignment.MiddleLeft, ""),
            New dgField("ReferenciaDePago", "Referencia Pago", 120, DataGridViewContentAlignment.MiddleLeft, ""),
            New dgField("Importe", "Importe", 120, DataGridViewContentAlignment.MiddleRight, FormatoNumero.FormatoNumero),
            New dgField("CodigoIso", "Moneda", 50, DataGridViewContentAlignment.MiddleCenter, ""),
            New dgField("ImporteReal", "Importe Real", 120, DataGridViewContentAlignment.MiddleRight, FormatoNumero.FormatoNumero),
            New dgField("Origen", "ORG", 30, DataGridViewContentAlignment.MiddleCenter, ""),
            New dgField("ProveedorCliente", "Prov/Cli", 120, DataGridViewContentAlignment.MiddleLeft, ""),
            New dgField("CodigoVendedor", "Asesor", 50, DataGridViewContentAlignment.MiddleCenter, ""),
            New dgField("Concepto", "Concepto", 350, DataGridViewContentAlignment.MiddleLeft, ""),
            New dgField("Sada", "", 100, DataGridViewContentAlignment.MiddleLeft, "")
        }
        ft.IniciarTablaListPlus(Of SavingLines)(dg, savingTransactionList, aCampos)

        If savingTransactionList.Count > 0 Then
            posicionRenglon = 0
            AsignaMov(posicionRenglon, True)
        Else
            AsignaSaldos(cashBox.Codigo)
        End If



    End Sub
    Private Sub AsignaSaldos(ByVal numCaja As String)

        txtEF.Text = Math.Round(savingTransactionList.Where(Function(w) w.FormaDePago = "EF").Sum(Function(s) s.ImporteReal), 2)
        txtCH.Text = Math.Round(savingTransactionList.Where(Function(w) w.FormaDePago = "CH").Sum(Function(s) s.ImporteReal), 2)
        txtTA.Text = Math.Round(savingTransactionList.Where(Function(w) w.FormaDePago = "TA").Sum(Function(s) s.ImporteReal), 2)
        txtCT.Text = Math.Round(savingTransactionList.Where(Function(w) w.FormaDePago = "CT").Sum(Function(s) s.ImporteReal), 2)
        txtOT.Text = Math.Round(savingTransactionList.Where(Function(w) w.FormaDePago = "OT").Sum(Function(s) s.ImporteReal), 2)
        txtSaldo.Text = Math.Round(savingTransactionList.Sum(Function(s) s.ImporteReal), 2)

    End Sub

    Private Sub IniciarCaja(ByVal Inicio As Boolean)
        If Inicio Then
            txtCodigo.Text = ft.autoCodigo(myConn, "caja", "jsbanenccaj", "id_emp", jytsistema.WorkID, 2, True)
        Else
            txtCodigo.Text = ""
        End If

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtNombre)
        ft.iniciarTextoObjetos(Transportables.tipoDato.Numero, txtSaldo, txtEF, txtCH, txtTA, txtCT, txtOT)

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        dg.Columns.Clear()
    End Sub
    Private Sub ActivarMarco0()

        ft.visualizarObjetos(True, grpAceptarSalir)
        ft.habilitarObjetos(True, True, txtNombre, cmbCC)
        cmbCC.Enabled = True
        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub


    Private Sub DesactivarMarco0()

        ft.visualizarObjetos(False, grpAceptarSalir)
        ft.habilitarObjetos(False, True, txtCodigo, txtNombre, cmbMonedas, cmbCC, txtCH, txtCT, txtEF, txtOT, txtSaldo, txtTA)
        cmbCC.Enabled = False

        MenuBarra.Enabled = True
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If cashBoxList.Count = 0 Then
            IniciarCaja(False)
        Else
            AsignaTXT(cashBox)
        End If
        DesactivarMarco0()
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado() As Boolean
        If txtNombre.Text = "" Then
            ft.mensajeCritico("INDIQUE NOMBRE DE CAJA VALIDO...")
            Return False
        End If

        If CBool(ParametroPlus(myConn, Gestion.iBancos, "BANPARAM17")) Then
            If cmbCC.Text = "" Then
                ft.mensajeCritico("Debe indicar una CUENTA CONTABLE VALIDA...")
                Return False
            End If
        End If

        Return True

    End Function
    Private Sub GuardarTXT()

        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then Inserta = True

        InsertEditBANCOSEncabezadoCaja(myConn, lblInfo, Inserta, txtCodigo.Text, txtNombre.Text, cmbCC.SelectedValue,
                                       ValorNumero(txtSaldo.Text), cmbMonedas.SelectedValue, jytsistema.sFechadeTrabajo)

        cashBoxList = GetCashBoxes(myConn)
        AsignaTXT(cashBoxList.LastOrDefault())
        DesactivarMarco0()

        '' TODO Add Functionality weith list
        '' ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        ActivarMarco0()
        IniciarCaja(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        i_modo = movimiento.iEditar
        ActivarMarco0()
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If cashBox.Codigo <> "00" Then
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        '     Dim fields As New List(Of fTransport.Models.DataGridField)
        '     fields.Add(New fTransport.Models.DataGridField("Codigo", "Codigo Caja", 100, DataGridViewContentAlignment.MiddleLeft, ""))
        '     fields.Add(New fTransport.Models.DataGridField("Descripcion", "Nombre Caja", 2500, DataGridViewContentAlignment.MiddleLeft, ""))

        '        Dim f As New frmBuscarPlus
        '        f.Buscar(cajas, fields, "Cajas")
        '        AsignaTXT(f.Apuntador)

    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        AsignaTXT(cashBoxList.FirstOrDefault())
    End Sub
    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        posicionEncab -= 1
        If posicionEncab < 0 Then posicionEncab = 0
        AsignaTXT(cashBoxList.Item(posicionEncab))
    End Sub
    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        posicionEncab += 1
        If posicionEncab >= cashBoxList.Count Then posicionEncab = cashBoxList.Count - 1
        AsignaTXT(cashBoxList.Item(posicionEncab))
    End Sub
    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        AsignaTXT(cashBoxList.LastOrDefault())
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click

        Dim f As New jsBanArcCajasMovimientos
        f.Apuntador = posicionRenglon
        '' TODO 
        f.Agregar(myConn, cashBox.Codigo)
        AsignaMov(f.Apuntador, True)
        f = Nothing

    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        If posicionRenglon > 0 Then
            If ft.FormatoFechaMySQL(savingTransaction.FechaBloqueo) > ft.FormatoFechaMySQL(FechaMinimaBloqueo) Then
                ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
            Else
                If savingTransaction.Origen = "CAJ" Then
                    Dim f As New jsBanArcCajasMovimientos
                    '' TODO
                    '' f.Editar(myConn, ds, dtMovimientos, cashBox.Codigo)
                    AsignaMov(f.Apuntador, True)
                    f = Nothing
                End If
            End If
        End If

    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click

        EliminarMovimiento()

    End Sub
    Private Sub EliminarMovimiento()

        If posicionRenglon >= 0 Then
            If ft.FormatoFechaMySQL(savingTransaction.FechaBloqueo) > ft.FormatoFechaMySQL(FechaMinimaBloqueo) Then
                ft.mensajeCritico("DOCUMENTO BLOQUEADO. POR FAVOR VERIFIQUE...")
            Else
                If savingTransaction.Origen = "CAJ" Then
                    If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, savingTransaction.NumeroMovimiento)

                        ft.Ejecutar_strSQL(myConn, "DELETE FROM jsbantracaj where Id = '" & savingTransaction.Id & "' ")

                        If savingTransactionList.Count - 1 < posicionRenglon Then _
                            posicionRenglon = savingTransactionList.Count - 1
                        AsignaMov(posicionRenglon, True)

                    End If
                Else
                    ft.mensajeCritico("Movimiento proveniente de " & savingTransaction.Origen & ". ")
                End If

            End If
        End If
    End Sub
    Private Sub btnBuscarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarMovimiento.Click
        Dim aCampos As List(Of dgFieldSF) = New List(Of dgFieldSF)() From {
            New dgFieldSF(TypeColumn.DateTimeColumn, "Fecha", "FECHA", 80, HorizontalAlignment.Center, FormatoFecha.FormatoFecha),
            New dgFieldSF(TypeColumn.TextColumn, "NumeroMovimiento", "Documento", 120, HorizontalAlignment.Left, ""),
            New dgFieldSF(TypeColumn.TextColumn, "FormaDePago", "FP", 35, HorizontalAlignment.Center, ""),
            New dgFieldSF(TypeColumn.TextColumn, "NumeroDePago", "No Pago", 120, HorizontalAlignment.Left, ""),
            New dgFieldSF(TypeColumn.TextColumn, "ReferenciaDePago", "Referencia Pago", 120, HorizontalAlignment.Left, ""),
            New dgFieldSF(TypeColumn.NumericColumn, "ImporteReal", "Importe Real", 120, HorizontalAlignment.Right, FormatoNumero.FormatoNumero),
            New dgFieldSF(TypeColumn.TextColumn, "ProveedorCliente", "Prov/Cli", 120, HorizontalAlignment.Left, ""),
            New dgFieldSF(TypeColumn.TextColumn, "CodigoVendedor", "Asesor", 50, HorizontalAlignment.Center, "")
        }
        Dim f As New frmBuscarPlus
        f.Buscar(savingTransactionList, aCampos, "Movimientos de Caja")
        If f.Id > 0 Then
            Dim index = savingTransactionList.FindIndex(Function(item) item.Id = f.Id)
            SetSelectedRowByIndex(dg, index)
        End If
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
       dg.RowHeaderMouseClick, dg.CellMouseClick
        AsignaMov(e.RowIndex, False)
    End Sub
    Private Sub btnPrimerMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimerMovimiento.Click
        Dim nRow As Integer = dg.Rows.GetFirstRow(DataGridViewElementStates.None)
        AsignaMov(nRow, False)
    End Sub

    Private Sub btnAnteriorMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnteriorMovimiento.Click
        Dim nRow As Integer = dg.Rows.GetPreviousRow(dg.SelectedRows(0).Index, DataGridViewElementStates.None)
        AsignaMov(IIf(nRow < 0, 0, nRow), False)
    End Sub

    Private Sub btnSiguienteMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguienteMovimiento.Click
        Dim nRow As Integer = dg.Rows.GetNextRow(dg.SelectedRows(0).Index, DataGridViewElementStates.None)
        AsignaMov(IIf(nRow < 0, 0, nRow), False)
    End Sub

    Private Sub btnUltimoMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimoMovimiento.Click
        Dim nRow As Integer = dg.Rows.GetLastRow(DataGridViewElementStates.None)
        AsignaMov(nRow, False)
    End Sub
    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                Dim nRow As Integer = dg.Rows.GetNextRow(dg.SelectedRows(0).Index, DataGridViewElementStates.None)
                AsignaMov(IIf(nRow < 0, 0, nRow), False)
            Case Keys.Up
                Dim nRow As Integer = dg.Rows.GetPreviousRow(dg.SelectedRows(0).Index, DataGridViewElementStates.None)
                AsignaMov(IIf(nRow < 0, 0, nRow), False)
        End Select
    End Sub
    Private Sub btnRemesas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemesas.Click
        '
    End Sub
    Private Sub btnRemesas_DropDownItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles btnRemesas.DropDownItemClicked
        posicionRenglon = Me.BindingContext(ds, nTablaMovimientos).Position
        Dim f As New jsBanArcRemesasCestaTicket
        f.Remesas(myConn, ds, cashBox.Codigo, e.ClickedItem.Text)
        AsignaMov(posicionRenglon, True)
        f = Nothing
    End Sub

    Private Sub btnAdelantoEfectivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdelantoEfectivo.Click
        Dim f As New jsBanArcCajasAvanceEF
        f.Apuntador = posicionRenglon
        '' TODO
        '' f.Agregar(myConn, ds, dtMovimientos, cashBox.Codigo)
        AsignaMov(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cMovimientoCaja, "Movimientos Caja", cashBox.Codigo)
        f = Nothing
    End Sub

    Private Sub dg_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dg.DataBindingComplete
        Cursor.Current = Cursors.Default
    End Sub
End Class