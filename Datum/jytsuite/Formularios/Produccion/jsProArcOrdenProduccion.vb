Imports MySql.Data.MySqlClient
Imports Microsoft.Win32
Imports fTransport
Imports Syncfusion.WinForms.Input

Public Class jsProArcOrdenProduccion



    Private Const sModulo As String = "Ordenes de Producción"
    Private Const lRegion As String = "RibbonButton162"
    Private Const nTabla As String = "tblEncabOrdenes"
    Private Const nTablaRenglones As String = "tblRenglones_Ordenes"
    Private Const nTablaComponentes As String = "tblComponentes"
    Private Const nTablaExplosion As String = "tblExplosion"



    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtRenglones As New DataTable
    Private dtComponentes As New DataTable
    Private dtExplosion As New DataTable
    Private ft As New Transportables

    Private i_modo As Integer

    Private nPosicionEncab As Long
    Private nPosicionRenglon As Long
    Private nPosicionComponente As Long
    Private nPosicionExplosion As Long

    Private strSQL As String = " select a.* from jsfabencord a " _
              & " where " _
              & " a.emision <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
              & " a.id_emp = '" & jytsistema.WorkID & "' order by a.codord "

    Private strSQLMov As String = ""
    Private strSQLComponentes As String = ""
    Private strSQLExplosion As String = ""

    Private aEstatus() As String = {"Por iniciar", "En proceso", "Terminadas", "Canceladas"}

    Private Eliminados As New ArrayList

    Private Sub jsProArcOrdenDeProduccion_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsProArcOrdenDeProduccion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)
            Dim dates As SfDateTimeEdit() = {txtEmision, txtEstimada}
            SetSizeDateObjects(dates)

            DesactivarMarco0()
            tbcOrden.SelectedTab = TabPageComponentes

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
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva Orden de Producción")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> Orden de Producción")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> Orden de Producción")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> Orden de Producción")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primera</B> Orden de Producción")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a Orden de Producción <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a Orden de Producción <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>última Orden de Producción</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> Orden de Producción")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")
        C1SuperTooltip1.SetToolTip(btnDuplicar, "<B>Duplicar</B> este Orden de Producción")
        C1SuperTooltip1.SetToolTip(btnRecalcular, "<B>Recalculoar</B> esta Orden de Producción en base a nuevos costos")
        C1SuperTooltip1.SetToolTip(btnExplosion, "<B>Explotar</B> esta Orden de Producción en base a una cantidad deseada")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en Orden de Producción")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón en Orden de Producción")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón en Orden de Producción")
        C1SuperTooltip1.SetToolTip(btnBuscarMovimiento, "<B>Buscar</B> un renglón en Orden de Producción")
        C1SuperTooltip1.SetToolTip(btnPrimerMovimiento, "ir al <B>primer</B> renglón en Orden de Producción")
        C1SuperTooltip1.SetToolTip(btnAnteriorMovimiento, "ir al <B>anterior</B> renglón en Orden de Producción")
        C1SuperTooltip1.SetToolTip(btnSiguienteMovimiento, "ir al renglón <B>siguiente </B> en Orden de Producción")
        C1SuperTooltip1.SetToolTip(btnUltimoMovimiento, "ir al <B>último</B> renglón de la Orden de Producción")


    End Sub


    Private Sub AsignaTXT(ByVal nRow As Long)

        i_modo = movimiento.iConsultar

        If nRow >= 0 Then
            With dt

                nPosicionEncab = nRow
                Me.BindingContext(ds, nTabla).Position = nRow
                MostrarItemsEnMenuBarra(MenuBarra, nRow, .Rows.Count)

                With .Rows(nRow)

                    'Encabezado 
                    txtCodigo.Text = ft.muestraCampoTexto(.Item("CODORD"))
                    txtEmision.Value = .Item("EMISION")
                    txtEstimada.Value = .Item("ESTIMADA")
                    txtDescripProduccion.Text = ft.muestraCampoTexto(.Item("DESCRIP"))

                    txtOrdenOrigen.Text = ft.muestraCampoTexto(.Item("ORDEN_RELACIONADA"))

                    txtPesoTotal.Text = ft.muestraCampoCantidad(.Item("PESO_TOTAL"))
                    txtCostoTotal.Text = ft.muestraCampoNumero(.Item("COSTO_TOTAL"))

                    ft.RellenaCombo(aEstatus, cmbEstatus, .Item("ESTATUS"))

                    'Renglones
                    AsignarMovimientos(.Item("CODORD"))

                    'Costos Fijos
                    AsignarComponentes(.Item("CODORD"))
                    'Totales
                    CalculaTotales()

                End With
            End With
        Else
            IniciarDocumento(False)
        End If

    End Sub

    Private Sub AsignarMovimientos(ByVal CodigoOrdenProduccion As String)

        strSQLMov = "select * from jsfabrenord " _
                            & " where " _
                            & " codord  = '" & CodigoOrdenProduccion & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' order by codfor "

        dtRenglones = ft.AbrirDataTable(ds, nTablaRenglones, myConn, strSQLMov)

        Dim aCampos() As String = {"codfor.Item.120.I.",
                                   "descrip.Descripción.330.I.",
                                   "cantidad.Cantidad.100.D.Cantidad",
                                   "unidad.UND.45.C.",
                                   "costounitario.Costo Unitario.120.D.Numero",
                                   "totalrenglon.Costo Total.120.D.Numero",
                                   "sada..100.C."}

        ft.IniciarTablaPlus(dgRenglones, dtRenglones, aCampos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dgRenglones, lRegion, jytsistema.sUsuario, nPosicionRenglon, True)
        End If

    End Sub

    Private Sub AsignarComponentes(ByVal OrdenProduccion As String)

        strSQLComponentes = "SELECT b.item, b.descrip, b.cantidad * a.cantidad cantidad , b.unidad, " _
            & " d.existencia, IFNULL(c.pendiente, 0.00) pendiente, IFNULL(c.apartada, 0.00) apartada, " _
            & " IFNULL(c.consumida, 0.00) consumida  " _
            & " FROM jsfabrenord a " _
            & " LEFT JOIN jsfabrenfor b ON (a.codfor = b.codfor AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsfabtraord c ON (a.codord = c.codord AND b.codfor = c.codfor AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsmerextalm d ON (b.item = d.codart AND a.id_emp = d.id_emp AND d.almacen = '00004') " _
            & " WHERE " _
            & " a.codord = '" & OrdenProduccion & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' "

        dtComponentes = ft.AbrirDataTable(ds, nTablaComponentes, myConn, strSQLComponentes)

        Dim aCampos() As String = {"item.Item.120.I.",
                                   "descrip.Descripción.330.I.",
                                   "cantidad.Cantidad.100.D.Cantidad",
                                   "unidad.UND.45.C.",
                                   "existencia.Existencia.100.D.Cantidad",
                                   "pendiente.Pendiente.100.D.Cantidad",
                                   "apartada.Apartada.100.D.Cantidad",
                                   "consumida.Consumida.100.D.Cantidad",
                                   "sada..100.C."}

        ft.IniciarTablaPlus(dgComponentes, dtComponentes, aCampos)
        If dtComponentes.Rows.Count > 0 Then
            nPosicionComponente = 0
        End If

    End Sub

    Private Sub AsignarExplosion(OrdeProduccion As String)

        Dim dtExplisonOrden As New DataTable

        Dim tablaExplosion As String = "tblExplosionOrden" & ft.NumeroAleatorio(1000000)
        Dim aFldExp() As String = {"CODART.cadena.15.0", "DESCRIP.cadena.250.0", "CANTIDAD.doble.10.3", "UNIDAD.cadena.3.0", "ID_EMP.cadena.2.0"}
        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tablaExplosion, tablaExplosion)



    End Sub


    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigo.Text = ft.autoCodigo(myConn, "CODORD", "jsfabencord", "id_emp", jytsistema.WorkID, 10)
        Else
            txtCodigo.Text = ""
        End If

        ft.RellenaCombo(aEstatus, cmbEstatus)
        txtEmision.Value = jytsistema.sFechadeTrabajo
        txtEstimada.Value = jytsistema.sFechadeTrabajo
        txtDescripProduccion.Text = ""
        txtPesoTotal.Text = ft.muestraCampoCantidad(0)
        txtCostoTotal.Text = ft.muestraCampoNumero(0.0)

        'Movimientos
        MostrarItemsEnMenuBarra(MenuBarraRenglon, 0, 0)
        AsignarMovimientos(txtCodigo.Text)

    End Sub

    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        ft.habilitarObjetos(True, False, grpEncab, MenuBarraRenglon)
        ft.habilitarObjetos(True, True, txtDescripProduccion, txtEmision, txtEstimada, cmbEstatus)
        MenuBarra.Enabled = False
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        ft.habilitarObjetos(False, True, txtCodigo, txtEmision, txtEstimada, txtDescripProduccion, cmbEstatus,
                            txtOrdenOrigen, txtPesoTotal, txtCostoTotal)
        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        If i_modo = movimiento.iAgregar Then AgregaYCancela()

        If dt.Rows.Count = 0 Then
            IniciarDocumento(False)
        Else
            Me.BindingContext(ds, nTabla).CancelCurrentEdit()
            AsignaTXT(nPosicionEncab)
        End If

        DesactivarMarco0()
    End Sub
    Private Sub AgregaYCancela()
        ft.Ejecutar_strSQL(myConn, " delete from jsfabrenord where codord = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub

    Private Function Validado() As Boolean

        If dtRenglones.Rows.Count = 0 Then
            ft.mensajeCritico("Debe incluir al menos un ítem...")
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
        End If

        InsertEditPRODUCCIONEncabezadoOrdenProduccion(myConn, lblInfo, Inserta, Codigo, txtDescripProduccion.Text, CDate(txtEmision.Text),
                                                CDate(txtEstimada.Text), ValorCantidad(txtPesoTotal.Text),
                                                cmbEstatus.SelectedIndex, "")

        dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

        Dim row As DataRow = dt.Select(" codord = '" & Codigo & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicionEncab = dt.Rows.IndexOf(row)

        Me.BindingContext(ds, nTabla).Position = nPosicionEncab

        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ft.ActivarMenuBarra(myConn, ds, dt, lRegion, MenuBarra, jytsistema.sUsuario)

    End Sub


    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDescripProduccion.GotFocus
        ft.mensajeEtiqueta(lblInfo, " Indique DESCRIPCION para esta Orden de Producción ... ", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click

        If CBool(ParametroPlus(myConn, Gestion.iProduccion, "PROORD0001")) Then
            i_modo = movimiento.iEditar
            nPosicionEncab = Me.BindingContext(ds, nTabla).Position
            ActivarMarco0()
        Else
            ft.mensajeCritico("Edición de Orden de Producción NO está permitida...")
        End If

    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        If CBool(ParametroPlus(myConn, Gestion.iProduccion, "PROORD0002")) Then

            nPosicionEncab = Me.BindingContext(ds, nTabla).Position
            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("CODORD"))

                ft.Ejecutar_strSQL(myConn, " delete from jsfabrenord where codord = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                ft.Ejecutar_strSQL(myConn, " delete from jsfabencord where codord = '" & txtCodigo.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
                dt = ds.Tables(nTabla)
                If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
                AsignaTXT(nPosicionEncab)

            End If
        Else
            ft.mensajeCritico("Eliminación de Ordenes de  producción NO está permitida...")
        End If

    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"codord", "descrip", "emision"}
        Dim Nombres() As String = {"Número ", "Descripción", "Emisión"}
        Dim Anchos() As Integer = {150, 400, 100}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Ordenes de producción...")
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

    Private Sub dg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgComponentes.CellFormatting
        'Dim aEstatus() As String = {"Normal", "Sin Desc.", "Bonificación"}
        'Select Case e.ColumnIndex
        '    Case 10
        '        e.Value = aTipoRenglon(e.Value)
        'End Select
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgComponentes.RowHeaderMouseClick,
       dgComponentes.CellMouseClick
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
        MostrarItemsEnMenuBarra(MenuBarraRenglon, e.RowIndex, ds.Tables(nTablaRenglones).Rows.Count)

    End Sub

    Private Sub CalculaTotales()
        txtPesoTotal.Text = ft.FormatoCantidad(ft.DevuelveScalarDoble(myConn, " select sum(peso) from jsfabrenord where codord = '" + txtCodigo.Text + "' and id_emp = '" + jytsistema.WorkID + "' group by codord "))
        txtCostoTotal.Text = ft.FormatoNumero(ft.DevuelveScalarDoble(myConn, " select sum(TOTALRENGLON) from jsfabrenord where codord = '" + txtCodigo.Text + "' and id_emp = '" + jytsistema.WorkID + "' group by codord "))
    End Sub


    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click

        Dim f As New jsProArcOrdenProduccionRenglones
        f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
        f.Agregar(myConn, ds, dtRenglones, txtCodigo.Text)
        nPosicionRenglon = f.Apuntador
        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dgRenglones, lRegion,
                              jytsistema.sUsuario, nPosicionRenglon, True)
        CalculaTotales()
        f.Dispose()
        f = Nothing

    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        If dtRenglones.Rows.Count > 0 Then
            'With dtRenglones.Rows(Me.BindingContext(ds, nTablaRenglones).Position)
            '    Dim f As New jsProArcFormulacionesRenglones
            '    f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            '    f.Editar(myConn, ds, dtRenglones, "FAC", txtCodigo.Text, txtAlmacen.Text,
            '             IIf(.Item("item").ToString.Substring(0, 1) = "$", True, False))
            '    nPosicionRenglon = f.Apuntador
            '    MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dg, lRegion, _
            '                          jytsistema.sUsuario, nPosicionRenglon, True)

            '    CalculaTotales()
            '    f = Nothing
            'End With
        End If

    End Sub

    Private Sub btnEliminarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarMovimiento.Click
        EliminarMovimiento()
    End Sub
    Private Sub EliminarMovimiento()

        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                With dtRenglones.Rows(nPosicionRenglon)

                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("item"))

                    Eliminados.Add(.Item("item"))

                    Dim aCamposDel() As String = {"codfor", "item", "renglon", "residual", "id_emp"}
                    Dim aStringsDel() As String = {txtCodigo.Text, .Item("item"), .Item("renglon"), 0, jytsistema.WorkID}

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsfabrenfor",
                                                            strSQLMov, aCamposDel, aStringsDel,
                                                            Me.BindingContext(ds, nTablaRenglones).Position, True)

                    If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1

                    MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dgComponentes, lRegion,
                                              jytsistema.sUsuario, nPosicionRenglon, True)
                    CalculaTotales()

                End With
            End If
        End If

    End Sub
    Private Sub btnBuscarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscarMovimiento.Click

        Dim f As New frmBuscar
        Dim Campos() As String = {"item", "descripcion"}
        Dim Nombres() As String = {"Item", "Descripción"}
        Dim Anchos() As Integer = {140, 350}
        f.Text = "Movimientos "
        f.Buscar(dtRenglones, Campos, Nombres, Anchos, Me.BindingContext(ds, nTablaRenglones).Position, " " & Me.Tag & "...")
        nPosicionRenglon = f.Apuntador
        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dgComponentes, lRegion,
                              jytsistema.sUsuario, nPosicionRenglon, False)
        f = Nothing

    End Sub

    Private Sub btnPrimerMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimerMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position = 0
        nPosicionRenglon = 0
        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dgComponentes, lRegion,
                              jytsistema.sUsuario, nPosicionRenglon, False)
    End Sub

    Private Sub btnAnteriorMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnteriorMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position -= 1
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dgComponentes, lRegion,
                              jytsistema.sUsuario, nPosicionRenglon, False)
    End Sub

    Private Sub btnSiguienteMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguienteMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position += 1
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dgComponentes, lRegion,
                              jytsistema.sUsuario, nPosicionRenglon, False)
    End Sub

    Private Sub btnUltimoMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimoMovimiento.Click
        Me.BindingContext(ds, nTablaRenglones).Position = ds.Tables(nTablaRenglones).Rows.Count - 1
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
        MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dgComponentes, lRegion,
                              jytsistema.sUsuario, nPosicionRenglon, False)
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        'Imprimir()
    End Sub

    Private Sub dgIVA_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs)
        If e.ColumnIndex = 1 Then e.Value = ft.FormatoNumero(e.Value) & "%"
    End Sub

    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgComponentes.KeyUp
        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(ds, nTablaRenglones).Position += 1
                nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
                MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dgComponentes, lRegion,
                                      jytsistema.sUsuario, nPosicionRenglon, False)
            Case Keys.Up
                Me.BindingContext(ds, nTablaRenglones).Position -= 1
                nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position
                MostrarFilaEnTabla(myConn, ds, nTablaRenglones, strSQLMov, Me.BindingContext, MenuBarraRenglon, dgComponentes, lRegion,
                                      jytsistema.sUsuario, nPosicionRenglon, False)

        End Select
    End Sub

    Private Sub tbcRenglones_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles tbcOrden.SelectedIndexChanged
        Select Case tbcOrden.SelectedIndex
            Case 0 'FORMULA
                AsignarMovimientos(txtCodigo.Text)
            Case 1 'RESIDUALES
                'AsignarMovimientosResidual(txtCodigo.Text)
        End Select
    End Sub



End Class