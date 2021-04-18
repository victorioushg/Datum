Imports MySql.Data.MySqlClient
Public Class jsVenArcRutasDespacho
    Private Const sModulo As String = "Rutas de Despacho"
    Private Const lRegion As String = "RibbonButton79"
    Private Const nTabla As String = "tblEncabRuta"
    Private Const nTablaRenglones As String = "tblRenglones_Ruta"

    Private strSQL As String = "select * from jsvenencrut where " _
        & "TIPO = '1' AND " _
        & "ID_EMP = '" & jytsistema.WorkID & "' ORDER BY codrut "

    Private strSQLMov As String = ""

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtRenglones As New DataTable

    Private i_modo As Integer
    Private nPosicionEncab As Long, nPosicionRenglon As Long

    Private Sub jsVenArcRutaVisita_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()

            ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
            dt = ds.Tables(nTabla)



            DesactivarMarco0()
            If dt.Rows.Count > 0 Then
                nPosicionEncab = dt.Rows.Count - 1
                Me.BindingContext(ds, nTabla).Position = nPosicionEncab
                AsignaTXT(nPosicionEncab)
            Else
                IniciarDocumento(False)
            End If
            ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
            AsignarTooltips()

        Catch ex As MySql.Data.MySqlClient.MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Agregar</B> nueva ruta despacho")
        C1SuperTooltip1.SetToolTip(btnEditar, "<B>Editar o mofificar</B> ruta despacho")
        C1SuperTooltip1.SetToolTip(btnEliminar, "<B>Eliminar</B> ruta despacho")
        C1SuperTooltip1.SetToolTip(btnBuscar, "<B>Buscar</B> ruta despacho")
        C1SuperTooltip1.SetToolTip(btnPrimero, "ir a la <B>primer</B> ruta despacho")
        C1SuperTooltip1.SetToolTip(btnSiguiente, "ir a ruta despacho <B>siguiente</B>")
        C1SuperTooltip1.SetToolTip(btnAnterior, "ir a ruta despacho <B>anterior</B>")
        C1SuperTooltip1.SetToolTip(btnUltimo, "ir a la <B>último ruta despacho</B>")
        C1SuperTooltip1.SetToolTip(btnImprimir, "<B>Imprimir</B> ruta despacho")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")

        'Menu barra renglón
        C1SuperTooltip1.SetToolTip(btnAgregarMovimiento, "<B>Agregar</B> renglón en ruta despacho")
        C1SuperTooltip1.SetToolTip(btnEditarMovimiento, "<B>Editar</B> renglón en ruta despacho")
        C1SuperTooltip1.SetToolTip(btnEliminarMovimiento, "<B>Eliminar</B> renglón en ruta despacho")


    End Sub

    Private Sub AsignaMov(ByVal nRow As Long, ByVal Actualiza As Boolean)

        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)

        If c >= 0 Then
            Me.BindingContext(ds, nTablaRenglones).Position = c
            tslblPesoT.Text = dtRenglones.Rows.Count
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dtRenglones, MenuBarraRenglon)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        With dt

            nPosicionEncab = nRow
            Me.BindingContext(ds, nTabla).Position = nRow

            MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)

            With .Rows(nRow)
                'Encabezado 
                txtCodigoRuta.Text = MuestraCampoTexto(.Item("codrut"))
                txtNombreRuta.Text = MuestraCampoTexto(.Item("nomrut"))
                txtComentario.Text = MuestraCampoTexto(.Item("comen"))
                txtAsesor.Text = MuestraCampoTexto(.Item("codven"))

                'Renglones
                AsignarMovimientos(.Item("codrut"))

                'Totales
                CalculaTotales()

            End With
        End With
    End Sub
    Private Sub AsignarMovimientos(ByVal CodigoRuta As String)

        strSQLMov = "select *  " _
                & " from jsvenrenrut " _
                & " where " _
                & " codrut = '" & CodigoRuta & "' and " _
                & " TIPO = '1' AND " _
                & " ID_EMP = '" & jytsistema.WorkID & "' " _
                & " order by NUMERO "

        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        Dim aCampos() As String = {"numero", "cliente", "nomcli", ""}
        Dim aNombres() As String = {"Número", "Código Cliente", "Nombre de Cliente y/o Razón Social", ""}
        Dim aAnchos() As Long = {60, 70, 600, 60}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", ""}
        IniciarTabla(dg, dtRenglones, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        If dtRenglones.Rows.Count > 0 Then
            nPosicionRenglon = 0
            AsignaMov(nPosicionRenglon, True)
        End If

    End Sub
    Private Sub IniciarDocumento(ByVal Inicio As Boolean)

        If Inicio Then
            txtCodigoRuta.Text = "RTMP" & NumeroAleatorio(1000)
        Else
            txtCodigoRuta.Text = ""
        End If

        IniciarTextoObjetos(FormatoItemListView.iCadena, txtNombreRuta, txtAsesor, txtComentario)
        'Movimientos
        tslblPesoT.Text = "0"
        AsignarMovimientos(txtCodigoRuta.Text)


    End Sub
    Private Sub ActivarMarco0()

        grpAceptarSalir.Visible = True

        HabilitarObjetos(True, False, grpEncab, MenuBarraRenglon)
        HabilitarObjetos(True, True, txtComentario, txtNombreRuta, btnAsesor, txtAsesor)
        MenuBarra.Enabled = False
        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)

    End Sub
    Private Sub DesactivarMarco0()

        grpAceptarSalir.Visible = False
        HabilitarObjetos(False, True, txtCodigoRuta, txtNombreRuta, _
                txtComentario, txtAsesor, btnAsesor, txtNombreAsesor)

        MenuBarraRenglon.Enabled = False
        MenuBarra.Enabled = True

        MensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", TipoMensaje.iAyuda)
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

        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrenrut where codrut = '" & txtCodigoRuta.Text & "' and tipo = '1' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            GuardarTXT()
        End If
    End Sub
    Private Function Validado() As Boolean

        If txtNombreAsesor.Text = "" Then
            MensajeCritico(lblInfo, "Debe indicar un nombre de transporte válido...")
            Return False
        End If

        If dtRenglones.Rows.Count = 0 Then
            MensajeCritico(lblInfo, "Debe incluir al menos un ítem...")
            Return False
        End If

        Return True

    End Function
    Private Sub GuardarTXT()

        Dim Codigo As String = txtCodigoRuta.Text
        Dim Inserta As Boolean = False
        If i_modo = movimiento.iAgregar Then
            Inserta = True
            nPosicionEncab = ds.Tables(nTabla).Rows.Count
            Codigo = AutoCodigo(5, ds, nTabla, "CODRUT")
            EjecutarSTRSQL(myConn, lblInfo, " update jsvenrenrut set codrut = '" & Codigo & "' where codrut = '" & txtCodigoRuta.Text & "' and tipo = '1' and id_emp = '" & jytsistema.WorkID & "' ")

        End If

        InsertEditVENTASEncabezadoRuta(myConn, lblInfo, Inserta, Codigo, txtNombreRuta.Text, txtComentario.Text, _
                                       "", txtAsesor.Text, txtAsesor.Text, 0, _
                                       0, "", "1", ValorEntero(tslblPesoT.Text), "")

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        Me.BindingContext(ds, nTabla).Position = nPosicionEncab
        AsignaTXT(nPosicionEncab)
        DesactivarMarco0()
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)

    End Sub

    Private Sub txtComentario_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtComentario.GotFocus
        MensajeEtiqueta(lblInfo, " Indique comentario ... ", TipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        i_modo = movimiento.iAgregar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
        IniciarDocumento(True)
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        i_modo = movimiento.iEditar
        nPosicionEncab = Me.BindingContext(ds, nTabla).Position
        ActivarMarco0()
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        'If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENCOTPA01")) Then
        ' Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        ' nPosicionEncab = Me.BindingContext(ds, nTabla).Position

        '        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, sModulo & " Eliminar registro ... ")

        '        If sRespuesta = MsgBoxResult.Yes Then

        '        InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(nPosicionEncab).Item("numcot"))
        '        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenenccot where " _
        '           & " numcot = '" & txtCodigoRuta.Text & "' AND " _
        '           & " ID_EMP = '" & jytsistema.WorkID & "'")

        '        EjecutarSTRSQL(myConn, lblInfo, " delete from jsvenrencot where numcot = '" & txtCodigoRuta.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

        '       ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        '      dt = ds.Tables(nTabla)
        '     If dt.Rows.Count - 1 < nPosicionEncab Then nPosicionEncab = dt.Rows.Count - 1
        '    AsignaTXT(nPosicionEncab)

        '   End If
        '  Else
        ' MensajeCritico(lblInfo, "Eliminación de presupuesto no permitida...")
        'End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"codrut", "nomrut"}
        Dim Nombres() As String = {"N° Ruta", "Nombre Ruta"}
        Dim Anchos() As Long = {150, 400}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Rutas de despacho...")
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
        Me.BindingContext(ds, nTablaRenglones).Position = e.RowIndex
    End Sub

    Private Sub CalculaTotales()
        tslblPesoT.Text = dtRenglones.Rows.Count
    End Sub

    Private Sub btnAgregarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarMovimiento.Click

        Dim f As New jsVenArcRutasVisitaMovimientos
        f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
        f.Agregar(myConn, ds, dtRenglones, txtCodigoRuta.Text, "1", 0)
        ds = DataSetRequery(ds, strSQLMov, myConn, nTablaRenglones, lblInfo)
        AsignaMov(f.Apuntador, True)
        CalculaTotales()
        f = Nothing


    End Sub

    Private Sub btnEditarMovimiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditarMovimiento.Click

        If dtRenglones.Rows.Count > 0 Then
            Dim f As New jsVenArcRutasVisitaMovimientos
            f.Apuntador = Me.BindingContext(ds, nTablaRenglones).Position
            f.Editar(myConn, ds, dtRenglones, txtCodigoRuta.Text, "1", 0)
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

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        nPosicionRenglon = Me.BindingContext(ds, nTablaRenglones).Position

        If nPosicionRenglon >= 0 Then
            sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
            If sRespuesta = MsgBoxResult.Yes Then
                With dtRenglones.Rows(nPosicionRenglon)
                    InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, .Item("numero"))
                    Dim aCamposDel() As String = {"CODRUT", "cliente", "TIPO", "CONDICION", "ID_EMP"}
                    Dim aStringsDel() As String = {txtCodigoRuta.Text, .Item("cliente"), "1", _
                                                   .Item("condicion"), jytsistema.WorkID}

                    EliminandoRenglonRuta(myConn, lblInfo, .Item("numero"), txtCodigoRuta.Text, "1", "", .Item("condicion"))

                    nPosicionRenglon = EliminarRegistros(myConn, lblInfo, ds, nTablaRenglones, "jsvenrenrut", strSQLMov, aCamposDel, aStringsDel, _
                                                 Me.BindingContext(ds, nTablaRenglones).Position, True)

                    If dtRenglones.Rows.Count - 1 < nPosicionRenglon Then nPosicionRenglon = dtRenglones.Rows.Count - 1
                    AsignaMov(nPosicionRenglon, True)
                    CalculaTotales()
                End With
            End If
        End If

    End Sub
    Private Sub FijaNumeroEnRenglones()
        Dim iCont As Integer
        For iCont = 0 To dtRenglones.Rows.Count - 1
            With dtRenglones.Rows(iCont)
                EjecutarSTRSQL(myConn, lblInfo, " update jsvenrenrut set numero = '" & iCont + 1 & "' " _
                                & " where " _
                                & " codrut = '" & .Item("codrut") & "' and " _
                                & " numero = '" & .Item("numero") & "' and " _
                                & " tipo = '" & .Item("tipo") & "' and " _
                                & " condicion = '" & .Item("condicion") & "' and " _
                                & " id_emp = '" & jytsistema.WorkID & "' ")
            End With
        Next
    End Sub
    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        '        If DocumentoImpreso(myConn, lblInfo, "jsvenenccot", "numcot", txtCodigoRuta.Text) AndAlso _
        '            CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENCOTPA07")) Then
        '        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENCOTPA08")) Then
        ' '' Dim f As New jsMerRepParametros
        '' f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cTransferencia, "Transferencia/Nota de Consumo de mercancías", txtCodigo.Text)
        '' f = Nothing
        ' Else
        ' MensajeCritico(lblInfo, "Impresión de presupuesto no permitida...")
        ' End If
        ' Else
        ' MensajeCritico(lblInfo, "Este documento ya fue impreso...")
        ' End If
    End Sub

    Private Sub btnAsesor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAsesor.Click
        txtAsesor.Text = CargarTablaSimple(myConn, lblInfo, ds, " select codtra codigo, nomtra descripcion from jsconctatra where id_emp = '" & jytsistema.WorkID & "'  order by 1 ", _
                                            "Asesores Comerciales", txtAsesor.Text)
    End Sub


    Private Sub txtAsesor_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAsesor.TextChanged
        Dim aFld() As String = {"codtra", "id_emp"}
        Dim aStr() As String = {txtAsesor.Text, jytsistema.WorkID}
        txtNombreAsesor.Text = qFoundAndSign(myConn, lblInfo, "jsconctatra", aFld, aStr, "nomtra")

    End Sub

    Private Sub btnReordenar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReordenar.Click
        FijaNumeroEnRenglones()
        AsignarMovimientos(txtCodigoRuta.Text)
    End Sub

End Class