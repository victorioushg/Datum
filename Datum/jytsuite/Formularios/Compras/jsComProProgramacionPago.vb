Imports MySql.Data.MySqlClient
Imports fTransport
Imports Syncfusion.WinForms.Input

Public Class jsComProProgramacionPago
    Private Const sModulo As String = "Pagos Programados"
    Private Const lRegion As String = "RibbonButton236"
    Private Const nTabla As String = "tblEncab"
    Private Const nTablaRenglones As String = "tblRenglones_"


    Private strSQL As String = " select a.* from jsproencprg a where a.id_emp = '" & jytsistema.WorkID & "' order by a.numprg "

    Private strSQLMov As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtRenglones As New DataTable

    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicionEncab As Long
    Private nPosicionRenglon As Long

    Private aEstatus() As String = {"Por procesar", "Procesado"}
    Private Eliminados As New ArrayList

    Private Impresa As Integer

    Private Sub jsComArcCompras_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsComArcGastos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)

            Dim dates As SfDateTimeEdit() = {txtEmision, txtDesde, txtHasta}
            SetSizeDateObjects(dates)

            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionEncab = dt.Rows.Count - 1
                Me.BindingContext(ds, nTabla).Position = nPosicionEncab
                AsignaTXT(nPosicionEncab)
            Else
                IniciarDocumento(False)
            End If

            ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva programación")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> programación")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> programación")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> programación")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primer</B> programación")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a programación <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a programación <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>último programación</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> programación")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnDuplicar, "<B>Duplicar</B> este programación")
        C1SuperTooltip1.SetToolTip(btnReconstruir, "<B>Reconstruir</B> movimientos de esta programación")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en programación")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón en programación")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón en programación")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en programación")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón en programación")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón en programación")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> en programación")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la programación")



    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)

        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)

        If c >= 0 And dtRenglones.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaRenglones).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, c, dtRenglones.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        ft.ActivarMenuBarra(myConn, ds, dtRenglones, lRegion, MenuBarraRenglon, jytsistema.sUsuario)
        'ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtCodigo)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        i_modo = movimiento.iConsultar

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtCodigo.Text = ft.muestraCampoTexto(.Item("numprg"))
                txtEmision.Value = .Item("fechaprg")
                txtDesde.Value = .Item("fechaprgdesde")
                txtHasta.Value = .Item("fechaprghasta")
                txtComentario.Text = ft.muestraCampoTexto(.Item("comen"))

                txtEstatus.Text = ft.muestraCampoTexto(aEstatus(.Item("estatusprg")))
                txtFechaProceso.Text = ft.muestraCampoFecha(.Item("fechaproceso"))
                ft.visualizarObjetos(.Item("estatusprg"), lblRecibido, txtFechaProceso)

                txtTotal.Text = ft.FormatoNumero(.Item("totalprg"))

                'Renglones
                AsignarMovimientos(txtCodigo.Text)


                'Totales
                CalculaTotales()

            End With
        End With
    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroCompra As String)

        strSQLMov = "select * from jsprorenprg " _
                            & " where " _
                            & " numprg  = '" & NumeroCompra & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by codpro, nummov "


        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"codpro.Proveedor.90.I.", _
                                   "nummov.Documento N°.90.I.", _
                                   "tipomov.TP.45.C.", _
                                   "emision.fecha Emisión.90.C.fecha", _
                                   "vence.Fecha Vencimiento.90.C.fecha", _
                                   "importe.Importe.120.D.Numero", _
                                   "saldo.Saldo.120.D.Numero", _
                                   "emisionCH.Emision Pago.90.C.fecha", _
                                   "banco.Banco.60.C.", _
                                   "a_cancelar.A Cancelar.120.D.Numero", _
                                   "sada..120.C."}

        ft.IniciarTablaPlus(dg, dtRenglones, aCampos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub


    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        txtCodigo.Text = ft.autoCodigo(myConn, "numprg", "jsproencprg", "id_emp", jytsistema.WorkID, 10)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Cadena, txtComentario)

        txtEmision.Value = jytsistema.sFechadeTrabajo
        txtHasta.Value = jytsistema.sFechadeTrabajo
        txtDesde.Value = jytsistema.sFechadeTrabajo
        txtEstatus.Text = ft.muestraCampoTexto(aEstatus(0))
        txtTotal.Text = "0.00"
        lblRecibido.Visible = False

        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(txtCodigo.Text)


    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon)
        ft.habilitarObjetos(True, True, txtComentario, txtEmision, txtDesde, txtHasta)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtEmision, txtDesde, txtHasta,
                txtComentario)

        ft.habilitarObjetos(False, True, txtTotal)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then
            AgregaYCancela()
        Else
            If dtRenglones.Rows.Count = 0 Then
                ft.mensajeCritico("DEBE INCLUIR AL MENOS UN ITEM...")
                Return
            End If
        End If


        If dt.Rows.Count = 0 Then
            IniciarDocumento(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            AsignaTXT(nPosicionEncab)
        End If

        DesactivarMarco0()


    End Sub
    Private Sub AgregaYCancela()

        ft.Ejecutar_strSQL(myConn, " delete from jsprorenprg where numprg = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
            ' Imprimir()
        End If
    End Sub
    Private Sub Imprimir()

    End Sub
    Private Function Validado() As Boolean

        If txtCodigo.Text = "" Then
            ft.mensajeCritico("Debe indicar un Número de Compra válido...")
            Return False
        End If

        If ft.DevuelveScalarEntero(myConn, " select COUNT(*) from jsproenccom where numcom = '" & _
                                      txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "'  ") > 0 AndAlso _
                                      i_modo = movimiento.iAgregar Then
            ft.mensajeCritico("Número de Compra YA existe para este proveedor ...")
            Return False
        End If

        If dtRenglones.Rows.Count = 0 Then
            ft.mensajeCritico("Debe incluir al menos un ítem...")
            Return False
        End If

        '//////////// VALIDAR SERVICIOS Y MERCANCIAS POR SEPARADO

        Return True

    End Function
    Private Sub GuardarTXT()

        Dim Codigo As String = txtCodigo.Text
        Dim iEstatus As Integer = Array.IndexOf(aEstatus, txtEstatus.Text)
        Dim Inserta As Boolean = False

        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count
        End If

        InsertEditCOMPRASEncabezadoPROGRAMACION(myConn, lblInfo, Inserta, Codigo, txtEmision.Value,
               txtDesde.Value, txtHasta.Value, iEstatus, "", txtComentario.Text,
               Convert.ToDouble(txtTotal.Text))

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" NUMPRG = '" & Codigo & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique comentario ... ", Transportables.tipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
        ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtCodigo)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        i_modo = movimiento.iEditar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position

        With dt.Rows(nPosicionEncab)
            'If DocumentoPoseeRetencionIVA(myConn, lblInfo, .Item("numcom"), .Item("codpro")) Then
            '    ft.MensajeCritico("Esta FACTURA posee RETENCION DE IVA. MODIFICACION NO esta permitida ...")
            'Else
            '    If ft.DevuelveScalarEntero(Myconn, " select IFNULL(COUNT(*),0) from jsprotrapag " _
            '                                  & " where " _
            '                                      & " tipomov <> 'FC' and ORIGEN <> 'COM' AND " _
            '                                      & " codpro = '" & .Item("codpro") & "' and " _
            '                                      & " nummov = '" & .Item("numcom") & "' and " _
            '                                      & " id_emp = '" & jytsistema.WorkID & "' ")) = 0 Then

            ActivarMarco0()
            ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtCodigo)
            '    Else
            'If NivelUsuario(myConn, lblInfo, jytsistema.sUsuario) = 0 Then
            '    ft.mensajeCritico("Esta FACTURA posee movimientos asociados. MODIFICACION NO esta permitida ...")
            'Else
            '    ActivarMarco0()
            'End If
            '    End If
            'End If
        End With

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numcom", "nombre", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Nombre", "Emisión", "Comentario"}
        Dim Anchos() As Integer = {150, 400, 100, 150}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, " Compras de mercancías ...")
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
            Case 9
                'e.Value = aTipoRenglon(e.Value)
        End Select
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub CalculaTotales()

        txtTotal.Text = ft.muestraCampoNumero(ft.DevuelveScalarDoble(myConn, " SELECT SUM(A_CANCELAR) FROM jsprorenprg WHERE NUMPRG = '" + txtCodigo.Text + "' AND ID_EMP = '" + jytsistema.WorkID + "' group by NUMPRG "))

    End Sub

    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        If txtCodigo.Text.Trim() = "" Then
            ft.mensajeAdvertencia("Debe indicar un número de PROGRAMACION VALIDO...")
        Else
            Dim f As New jsComProProgramacionPagoMovimiento
            f.Cargar(myConn, txtDesde.Value, txtHasta.Value)
            If Not f.Seleccionado Is Nothing Then

                For Each nRow As Object In f.Seleccionado

                    Dim fEmision As Date = ft.DevuelveScalarFecha(myConn, " select emision from jsprotrapag " _
                                                                   + " where " _
                                                                   + " codpro = '" + nRow(1).ToString + "' and " _
                                                                   + " nummov = '" + nRow(3).ToString + "' and " _
                                                                   + " id_emp = '" + jytsistema.WorkID + "' " _
                                                                   + " order by emision desc limit 1")

                    InsertEditCOMPRASRenglonPROGRAMACION(myConn, lblInfo, True, txtCodigo.Text, _
                               nRow(1).ToString, nRow(3).ToString, nRow(4).ToString, Convert.ToDateTime(nRow(6).ToString), _
                               Convert.ToDateTime(nRow(7).ToString), Convert.ToDouble(nRow(10).ToString), Convert.ToDouble(nRow(11).ToString), _
                              jytsistema.sFechadeTrabajo, f.bancoSeleccionado, _
                              Convert.ToDouble(nRow(14).ToString))

                Next
                AsignarMovimientos(txtCodigo.Text)
                CalculaTotales()

            End If
            f.Dispose()
            f = Nothing
        End If
    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click

        EliminarMovimiento()

    End Sub
    Private Sub EliminarMovimiento()

        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                With dtRenglones.Rows(nPosicionRenglon)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("numprg") + .Item("codpro") + .Item("nummov"))

                    Eliminados.Add(.Item("item"))

                    Dim aCamposDel() As String = {"numprg", "codpro", "nummov", "id_emp"}
                    Dim aStringsDel() As String = {txtCodigo.Text, .Item("codpro"), .Item("nummov"), jytsistema.WorkID}

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsprorenprg", strSQLMov, aCamposDel, aStringsDel, _
                                                  Me.BindingContext(ds, nTablaRenglones).Position, True)

                    If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1
                    AsignaMov(nPosicionRenglon, True)
                    CalculaTotales()
                End With
            End If
        End If

    End Sub

    Private Sub btnBuscarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarMovimiento.Click

        'Dim f As New frmBuscar
        'Dim Campos() As String = {"item", "descripcion"}
        'Dim Nombres() As String = {"Item", "Descripción"}
        'Dim Anchos() As Integer = {140, 350}
        'f.Text = "Movimientos "
        'f.Buscar(dtRenglones, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaRenglones).Position, " Renglones de compras...")
        'AsignaMov(f.Apuntador, False)
        'f = Nothing

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

End Class