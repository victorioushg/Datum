Imports MySql.Data.MySqlClient

Public Class jsMerArcPedidosAlmacen
    Private Const sModulo As String = "Pedidos de Almacén"
    Private Const lRegion As String = "RibbonButton301"
    Private Const nTabla As String = "tblEncab"
    Private Const nTablaRenglones As String = "tblRenglones"
    Private Const nTablaIVA As String = "tblIVA"

    Private strSQL As String = " select a.* from jsmerencped a " _
            & " where a.id_emp = '" & jytsistema.WorkID & "' order by a.numped "

    Private strSQLMov As String = ""
    Private strSQLIVA As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtRenglones As New DataTable
    Private dtIVA As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long
    Private aEstatus() As String = {"Tránsito", "Procesado"}

    Private Impresa As Integer

    Private Sub jsMerArcPedidosAlmacen_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsMerArcPedidosAlmacen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            ds = DataSetRequeryPlus(ds, nTabla, myConn, strSQL)
            dt = ds.Tables(nTabla)

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
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva Pedido Almacén")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> Pedido Almacén")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> Pedido Almacén")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> Pedido Almacén")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primer</B> Pedido Almacén")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a Pedido Almacén <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a Pedido Almacén <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>última Pedido Almacén</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> Pedido Almacén")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnDuplicar, "<B>Duplicar</B> esta Pedido Almacén")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en Pedido Almacén")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón en Pedido Almacén")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón en Pedido Almacén")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en Pedido Almacén")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón en Pedido Almacén")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón en Pedido Almacén")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> en Pedido Almacén")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la Pedido Almacén")

    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequeryPlus(ds, nTablaRenglones, myConn, strSQLMov)

        If c >= 0 AndAlso dtRenglones.Rows.Count > 0 Then
            Me.BindingContext(ds, nTablaRenglones).Position = c
            MostrarItemsEnMenuBarra(MenuBarraRenglon, c, dtRenglones.Rows.Count)
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        'ft.habilitarObjetos(IIf(dtRenglones.Rows.Count > 0, False, True), True, txtOrigen, txtDestino, btnOrigen, btnDestino, txtCodigo)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)
        If dt.Rows.Count > 0 Then
            With dt

                nPosicionEncab = nRow
                Me.BindingContext(ds, nTabla).Position = nRow

                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)


                With .Rows(nRow)
                    'Encabezado 
                    txtCodigo.Text = .Item("numped")
                    txtEmision.Text = ft.FormatoFecha(CDate(.Item("emision").ToString))
                    txtVence.Text = ft.FormatoFecha(CDate(.Item("entrega").ToString))
                    txtEstatus.Text = aEstatus(.Item("estatus"))
                    txtDestino.Text = .Item("ALM_DESTINO")
                    txtOrigen.Text = .Item("ALM_ORIGEN")
                    txtComentario.Text = ft.muestraCampoTexto(.Item("comen"))

                    tslblPesoT.Text = ft.FormatoCantidad(ft.DevuelveScalarDoble(myConn, "select SUM(PESO) from jsmerrenped where numped = '" & .Item("numped") & "' and id_emp = '" & jytsistema.WorkID & "' "))

                    txtTotal.Text = ft.FormatoNumero(.Item("tot_ped"))
                    Impresa = .Item("impresa")

                    'Renglones
                    AsignarMovimientos(.Item("numped"))

                    'Totales
                    CalculaTotales()

                End With
            End With
        End If
    End Sub
    Private Sub AsignarMovimientos(ByVal NumeroDocumento As String)
        strSQLMov = "select * from jsmerrenped " _
                            & " where " _
                            & " numped  = '" & NumeroDocumento & "' and " _
                            & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by renglon "

        ds = DataSetRequeryPlus(ds, nTablaRenglones, myConn, strSQLMov)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"item", "descrip", "iva", "cantidad", "unidad", "costou", "costotot", "estatus", ""}
        Dim aNombres() As String = {"Item", "Descripción", "IVA", "Cant.", "UND", "Costo Unitario", "Costo Total", "Tipo Renglon", ""}
        Dim aAnchos() As Integer = {90, 300, 30, 90, 45, 150, 170, 60, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", sFormatoCantidad, "", sFormatoNumero, _
                                     sFormatoNumero, "", ""}
        IniciarTabla(dg, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtRenglones.Rows.Count > 0 Then nPosicionRenglon = 0
        AsignaMov(nPosicionRenglon, True)

    End Sub


    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = "OCTemp" & ft.NumeroAleatorio(100000)
        Else

            txtCodigo.Text = ""
        End If

        txtDestino.Text = ""

        txtComentario.Text = ""
        txtEmision.Text = ft.FormatoFecha(sFechadeTrabajo)
        txtVence.Text = ft.FormatoFecha(sFechadeTrabajo)
        txtEstatus.Text = aEstatus(0)
        tslblPesoT.Text = ft.FormatoCantidad(0)

        txtTotal.Text = ft.FormatoNumero(0.0)

        Impresa = 0

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(txtCodigo.Text)


    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, grpTotales, MenuBarraRenglon)
        ft.habilitarObjetos(True, True, txtComentario, btnEmision, btnVence, btnDestino, btnOrigen)

        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtEmision, btnEmision, txtVence, btnVence, txtEstatus, _
                txtDestino, txtNombreDestino, btnOrigen, txtOrigen, btnDestino, txtNombreOrigen, txtComentario)

        ft.habilitarObjetos(False, True, txtTotal)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.TipoMensaje.iAyuda)
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

        ft.Ejecutar_strSQL(myConn, " delete from jsmerrenped where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " delete from jsmerivaped where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado() As Boolean

        If txtNombreDestino.Text.Trim() = "" OrElse txtDestino.Text.Trim() = "" Then
            ft.MensajeCritico("Debe indicar un ALMACEN DESTINO válido...")
            Return False
        End If

        If txtNombreOrigen.Text.Trim() = "" OrElse txtOrigen.Text.Trim() = "" Then
            ft.MensajeCritico("Debe indicar un ALMACEN ORIGEN válido...")
            Return False
        End If

        If CDate(txtVence.Text) < CDate(txtEmision.Text) Then
            ft.MensajeCritico("Fecha de emision es mayor a fecha de entrega. Verifique ...")
            Return False
        End If

        If dtRenglones.Rows.Count = 0 Then
            ft.MensajeCritico("Debe incluir al menos un ítem...")
            Return False
        End If

        Return True

    End Function
    Private Sub GuardarTXT()

        Dim Codigo As String = txtCodigo.Text
        Dim Inserta As Boolean = False


        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count

            Codigo = Contador(myConn, lblInfo, Gestion.iMercancías, "INVNUMPAL", "04")

        End If

        ft.Ejecutar_strSQL(myConn, " update jsmerrenped set numped = '" & Codigo & "' where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myConn, " update jsmerivaped set numped = '" & Codigo & "' where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

        InsertEditMERCASEncabezadoPedidoAlmacen(myConn, lblInfo, Inserta, Codigo, CDate(txtEmision.Text), CDate(txtVence.Text), _
                                               txtOrigen.Text, txtDestino.Text, txtComentario.Text, _
                                               0, 0, ValorNumero(txtTotal.Text), _
                                               ft.InArray(aEstatus, txtEstatus.Text), dtRenglones.Rows.Count, Impresa)


        ds = DataSetRequeryPlus(ds, nTabla, myConn, strSQL)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub

    Private Sub txtComentario_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique comentario ... ", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        If dt.Rows(nPosicionEncab).Item("estatus") = "0" Then
            i_modo = movimiento.iEditar
            ActivarMarco0()
        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        nPosicionEncab = Me.BindingContext(ds, nTabla).Position

        If dt.Rows(nPosicionEncab).Item("estatus") = "0" Then
            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numped"))
                ft.Ejecutar_strSQL(myConn, " delete from jsmerencped where " _
                    & " numped = '" & txtCodigo.Text & "' AND " _
                    & " ID_EMP = '" & jytsistema.WorkID & "'")

                ft.Ejecutar_strSQL(myConn, " delete from jsmerrenped where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myConn, " delete from jsmerivaped where numped = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                ds = DataSetRequeryPlus(ds, nTabla, myConn, strSQL)
                dt = ds.Tables(nTabla)
                If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                AsignaTXT(nPosicionEncab)

            End If
        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"numped", "emision", "comen"}
        Dim Nombres() As String = {"Número ", "Emisión", "Comentario"}
        Dim Anchos() As Integer = {150, 150, 250}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Pedidos de Almacén...")
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
            Case 7
                e.Value = aTipoRenglon(IIf(e.Value = "", "0", e.Value))
        End Select
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)
    End Sub

    Private Sub CalculaTotales()

        If dtRenglones.Rows.Count > 0 Then
            txtTotal.Text = ft.FormatoNumero(ft.DevuelveScalarDoble(myConn, " select SUM(COSTOTOT) from jsmerrenped where numped = '" & txtCodigo.Text & "' and id_emp = '" + jytsistema.WorkID + "'  "))
            tslblPesoT.Text = ft.FormatoCantidad(CalculaPesoDocumento(myConn, lblInfo, "jsmerrenped", "peso", "numped", txtCodigo.Text))
        End If

    End Sub

    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click
        If txtCodigo.Text.Trim() = "" Then
            ft.mensajeAdvertencia("Debe indicar un número de PEDIDO válido...")
        Else
            If txtNombreDestino.Text.Trim() = "" Then
                ft.mensajeAdvertencia("Debe indicar un ALMACEN DESTINO válido...")
            Else
                If txtNombreOrigen.Text.Trim() = "" Then
                    ft.mensajeAdvertencia("Debe indicar un ALMACEN ORIGEN válido...")
                Else
                    Dim f As New jsMerArcListaCostosPreciosSelecionable
                    f.Cargar(myConn, TipoListaPrecios.CostosSugerido, txtOrigen.Text, txtDestino.Text)
                    If Not f.Seleccionado Is Nothing Then
                        For Each nRow As Object In f.Seleccionado
                            If ft.DevuelveScalarEntero(myConn, " select COUNT(*) from jsmerrenped where numped = '" & txtCodigo.Text & "' and item = '" + nRow(1).ToString + "' and id_emp = '" + jytsistema.WorkID + "' group by item ") = 0 Then

                                Dim peso As Double = ft.DevuelveScalarDoble(myConn, " select pesounidad from jsmerctainv where codart = '" + nRow(1).ToString + "' and id_emp = '" + jytsistema.WorkID + "' ")
                                Dim costo As Double = ft.DevuelveScalarDoble(myConn, " select montoultimacompra from jsmerctainv where codart = '" + nRow(1).ToString + "' and id_emp = '" + jytsistema.WorkID + "' ")

                                InsertEditMERCASRenglonPedidoAlmacen(myConn, lblInfo, True, txtCodigo.Text, _
                                    ft.autoCodigo(myConn, "RENGLON", "jsmerrenped", "NUMPED.id_emp", txtCodigo.Text + "." + jytsistema.WorkID, 5), _
                                    nRow(1).ToString, nRow(2).ToString, nRow(4).ToString, nRow(10).ToString, _
                                    ValorCantidad(nRow(9).ToString), peso * ValorCantidad(nRow(9).ToString), "", 0, _
                                    costo, costo * ValorCantidad(nRow(9).ToString), "1")


                            End If
                        Next
                        AsignarMovimientos(txtCodigo.Text)
                        CalculaTotales()
                    End If
                    f.Dispose()
                    f = Nothing
                End If
            End If
        End If
    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = nPosicionRenglon
            f.Editar(myConn, ds, dtRenglones, "PAL", txtCodigo.Text, CDate(txtEmision.Text), txtDestino.Text, , , _
                     IIf(dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position).Item("item").ToString.Substring(0, 1) = "$", True, False), _
                     , , , , , , , , , , txtOrigen.Text)
            ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
            AsignaMov(f.Apuntador, True)
            CalculaTotales()
            f = Nothing

        End If

    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click

        EliminarMovimiento()

    End Sub
    Private Sub EliminarMovimiento()

        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dtRenglones.Rows(nPosicionRenglon).Item("item"))
                Dim aCamposDel() As String = {"NUMPED", "ITEM", "RENGLON", "ID_EMP"}
                Dim aStringsDel() As String = {txtCodigo.Text, dtRenglones.Rows(nPosicionRenglon).Item("item"), dtRenglones.Rows(nPosicionRenglon).Item("renglon"), jytsistema.WorkID}

                nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsmerrenped", strSQLMov, aCamposDel, aStringsDel, _
                                              Me.BindingContext(ds, nTablaRenglones).Position, True)

                If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1
                AsignarMovimientos(txtCodigo.Text)
                CalculaTotales()
            End If
        End If

    End Sub
    Private Sub btnBuscarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarMovimiento.Click

        'Dim f As New frmBuscar
        'Dim Campos() As String = {"item", "descripcion"}
        'Dim Nombres() As String = {"Item", "Descripción"}
        'Dim Anchos() As Integer = {140, 350}
        'f.Buscar(dtRenglones, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaRenglones).Position, "Renglones de órdenes de compra")
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

    Private Sub Imprimir()

        Dim f As New jsMerRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cPedidosAlmacen, "Pedido Almacén", txtCodigo.Text)
        f.Dispose()
        f = Nothing

        'ft.Ejecutar_strSQL ( myconn, "update jsproencord set impresa = 1 where numord = '" & txtCodigo.Text & "' and codpro = '" & txtProveedor.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub

    Private Sub btnEmision_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmision.Click
        txtEmision.Text = SeleccionaFecha(CDate(txtEmision.Text), Me, sender)
    End Sub

    Private Sub btnVence_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVence.Click
        txtVence.Text = SeleccionaFecha(CDate(txtVence.Text), Me, sender)
    End Sub


    Private Sub txtDestino_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDestino.TextChanged
        txtNombreDestino.Text = ft.DevuelveScalarCadena(myConn, " select desalm from jsmercatalm where codalm = '" + txtDestino.Text + "' and id_emp = '" + jytsistema.WorkID + "'")
    End Sub
    Private Sub txtOrigen_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOrigen.TextChanged
        txtNombreOrigen.Text = ft.DevuelveScalarCadena(myConn, " select desalm from jsmercatalm where codalm = '" + txtOrigen.Text + "' and id_emp = '" + jytsistema.WorkID + "'")
    End Sub

    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs)
        If e.ColumnIndex = 1 Then e.Value = ft.FormatoNumero(e.Value) & "%"
    End Sub

    Private Sub btnAlmacen_Origen(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrigen.Click
        txtOrigen.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codalm codigo, desalm descripcion from jsmercatalm where id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Almacén Origen", _
                                            txtOrigen.Text)
    End Sub
    Private Sub btnAlmacen_Destino(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDestino.Click
        If txtNombreOrigen.Text.Trim() <> "" Then
            txtDestino.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codalm codigo, desalm descripcion from jsmercatalm where codalm <> '" & txtOrigen.Text & "' and id_emp = '" & jytsistema.WorkID & "' order by 1 ", "Almacen Destino", _
                                                txtDestino.Text)
        Else
            ft.MensajeCritico("Debe indicar un almacén origen Válido...")
        End If
    End Sub

    Private Sub btnAgregarServicio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarServicio.Click
        If txtNombreDestino.Text <> "" Then
            Dim f As New jsGenRenglonesMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Agregar(myConn, ds, dtRenglones, "PAL", txtCodigo.Text, CDate(txtEmision.Text), txtDestino.Text)
            nPosicionRenglon = f.Apuntador
            AsignarMovimientos(txtCodigo.Text)
            CalculaTotales()
            f = Nothing
        End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub btnDuplicar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDuplicar.Click

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