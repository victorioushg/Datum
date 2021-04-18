Imports MySql.Data.MySqlClient
Public Class jsMerArcListaCostosPreciosPlus
    Private Const sModulo As String = "Listado de mercancías"
    Private Const lRegion As String = ""
    Private Const nTabla As String = "tblListaMercas"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable

    Private strSQL As String = ""

    Private Posicion As Long
    Private n_Seleccionado As String

    Private nTipoLista As Integer
    Private nMeses As Integer = 3
    Private nDiasSugerido As Integer = 10
    Private sProveedor As String = ""
    Private TARIFA As String = "A"

    Private CodigoAlmacen As String = ""
    Private CodigoProveedor As String = ""
    Private EstatusMercancia As Integer = 9

    Private BindingSource1 As New BindingSource
    Private FindField As String
    Private strEstatus As String = ""
    Private strAlmacen As String = ""

    Private _Control As Integer = 0
    Private aCatalogo() As String = {"Catálogo proveedor", "Catálogo completo"}

    Public Property Seleccionado() As String
        Get
            Return n_Seleccionado
        End Get
        Set(ByVal value As String)
            n_Seleccionado = value
        End Set
    End Property
    Public Sub Cargar(ByVal Mycon As MySqlConnection, ByVal TipoLista As Integer, _
                      Optional ByVal Almacen As String = "", Optional ByVal xProveedor As String = "", _
                      Optional ByVal TipoTarifa As String = "A", Optional ByVal iEstatusMercancia As Integer = 9)

        'Tipo Lista
        ' 0 = Normal ; 1=Costos ; 2=Precios ; 3=Precios+IVA; 4 = CostosSugerido

        'Estatus mercancia
        '0 = Activa; 1 Inactiva; 9 = Todas

        myConn = Mycon
        CodigoAlmacen = Almacen
        TARIFA = TipoTarifa
        EstatusMercancia = iEstatusMercancia
        CodigoProveedor = xProveedor
        nTipoLista = TipoLista



        AsignarTooltips()

        HabilitarObjetos(True, False, grpSugerido, txtPromedioMeses, txtSugeridoDias)
        txtPromedioMeses.Text = FormatoEntero(nMeses)
        txtSugeridoDias.Text = FormatoEntero(nDiasSugerido)

        If TipoLista = TipoListaPrecios.CostosSugerido Or TipoLista = TipoListaPrecios.Costos Then
            RellenaCombo(aCatalogo, cmbEstatus, 0)
        Else
            RellenaCombo(aCatalogo, cmbEstatus, 1)
        End If


        MensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", TipoMensaje.iAyuda)
        Me.ShowDialog()

    End Sub
    Private Function IniciarCadena(Tipolista As Integer, proveedor As String, EstatusMercancia As Integer, Almacen As String) As String

        strEstatus = IIf(EstatusMercancia <> 9, " a.estatus = " & EstatusMercancia & " AND ", "")
        sProveedor = IIf(proveedor <> "", " where c.ultimoproveedor = '" & proveedor & "' ", "")
        strAlmacen = IIf(Almacen <> "", " a.almacen = '" & Almacen & "' and ", "")

        If Tipolista = TipoListaPrecios.CostosSugerido Then

            strSQL = " SELECT a.codart, a.nomart, a.alterno, a.iva, ROUND(IF(b.existencia IS NULL, 0.00, b.existencia),3) existencia, " _
                & " ROUND(IF(b.inventario IS NULL, 0.00, b.inventario),3) inventario, " _
                & " ROUND(IF(b.sugerido IS NULL, 0.00, b.sugerido),3) sugerido, a.unidad, a.montoultimacompra " _
                & " FROM jsmerctainv a " _
                & " LEFT JOIN  (SELECT a.codart, SUM(IF( ISNULL(f.uvalencia), a.existencia, a.existencia/f.equivale)) existencia , " _
                & " SUM(IF( ISNULL(f.uvalencia), a.existencia, a.existencia/f.equivale)) / (  SUM(IF( ISNULL(f.uvalencia), a.ventas, a.ventas/f.equivale)) / d.DiasHabiles ) Inventario , " _
                & " ROUND(SUM(IF( ISNULL(f.uvalencia), " & nDiasSugerido & "*a.ventas, " & nDiasSugerido & "*a.ventas/f.equivale)) / d.DiasHabiles, 3) sugerido  " _
                & " " _
                & " FROM ( SELECT  b.codart, b.unidad,  SUM(IF( b.TIPOMOV IN( 'EN', 'AE', 'DV') , b.CANTIDAD, -1 * b.CANTIDAD )) existencia, " _
                & "             SUM(IF( b.origen IN ('FAC', 'PVE', 'PFC') AND b.fechamov >= DATE_ADD('" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL -" & nMeses & " MONTH) AND  b.fechamov <= DATE_ADD('" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL 1 DAY) ,  b.cantidad, 0.000 ) ) ventas, " _
                & "             b.id_emp " _
                & "             FROM jsmertramer b " _
                & "             WHERE " _
                & "      	    SUBSTRING(b.codart,1,1) <> '$' AND " _
                & "             b.tipomov <> 'AC' AND " _
                & "             b.id_emp = '" & jytsistema.WorkID & "' AND  " _
                & "             b.ejercicio = '' AND  " _
                & "             DATE_FORMAT(b.fechamov, '%Y-%m-%d') <= '" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' GROUP BY b.codart, b.unidad ) a  " _
                & " LEFT JOIN jsmerequmer f ON (a.codart = f.codart AND a.unidad = f.uvalencia AND a.id_emp = f.id_emp )  " _
                & " LEFT JOIN jsmerctainv c ON (a.codart = c.codart AND a.id_emp = c.id_emp) " _
                & " LEFT JOIN (SELECT '1' num, (DATEDIFF('" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', DATE_ADD('" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL -" & nMeses & " MONTH))) -  COUNT(*) DiasHabiles  " _
                & "         FROM (SELECT CONCAT(ano,'-',IF(LENGTH(mes)=1,CONCAT('0',mes),mes),'-',IF(LENGTH(dia)=1,CONCAT('0',dia),dia)) AS fecha FROM jsconcatper  " _
                & "         HAVING  (fecha <'" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' AND fecha>DATE_ADD('" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "',INTERVAL -" & nMeses & " MONTH)) )  a) d ON ( d.num = '1') " _
                & sProveedor _
                & " GROUP BY 1 ) b ON a.codart = b.codart " _
                & " WHERE " _
                & IIf(sProveedor.Trim <> "", " NOT b.codart is null AND ", "") _
                & " a.estatus = 0 AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' order by a.codart"


        Else

            strSQL = " select a.codart, a.nomart, a.alterno, a.barras,  a.unidad, a.iva, if( isnull(sum(b.existencia)), 0.000, sum(b.existencia))  existencia, " _
                                & " a.montoultimacompra, a.costo_prom, a.costo_prom_des,  " _
                                & " a.precio_a, a.precio_b, a.precio_c, a.precio_d, a.precio_f, a.precio_e, " _
                                & " round(a.precio_a*if( isnull(c.monto), 1.00, c.monto) ,0) precio_a_iva, round(a.precio_b*if( isnull(c.monto),1.00, c.monto),0)  precio_b_iva, round(a.precio_c*if( isnull(c.monto),1.00, c.monto),0)  precio_c_iva, round(a.precio_d*if( isnull(c.monto),1.00, c.monto),0)  precio_d_iva, round(a.precio_f*if( isnull(c.monto),1.00, c.monto),0)  precio_e_iva, round(a.precio_e*if( isnull(c.monto),1.00, c.monto),0) precio_f_iva " _
                                & " from jsmerctainv a " _
                                & " left join (select a.* from jsmerextalm a where " & strAlmacen & " a.id_emp = '" & jytsistema.WorkID & "') b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
                                & " left join (SELECT fecha, tipo, ROUND(1 + monto /100,2) monto from jsconctaiva WHERE fecha IN (SELECT MAX(fecha) FROM jsconctaiva WHERE fecha <= '" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' group by tipo )) c on a.iva = c.tipo " _
                                & " where  " _
                                & IIf(proveedor <> "", " a.ultimoproveedor = '" & proveedor & "' and ", "") _
                                & strEstatus _
                                & " a.id_emp = '" & jytsistema.WorkID & "' group by a.codart "

        End If

        Return strSQL

    End Function



    Private Sub Iniciar(ByVal strSQL As String, ByVal TipoLista As Integer, ByVal Almacen As String)

        Dim frm As New frmEspere
        frm.ShowDialog()

        strSQL = IniciarCadena(nTipoLista, IIf(cmbEstatus.SelectedIndex = 0, CodigoProveedor, ""), EstatusMercancia, CodigoAlmacen)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)
        BindingSource1.DataSource = dt

        IniciarGrilla(TipoLista)

        frm.Dispose()
        frm = Nothing

        If dt.Rows.Count > 0 Then Posicion = 0
        AsignaTXT(Posicion, True)
    End Sub

    Private Sub jsMerArcListaCostosPrecios_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt = Nothing
        ds = Nothing
    End Sub

    Private Sub jsMerArcListaCostosPrecios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        FindField = "nomart"
        lblBuscar.Text = "Nombre"
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
        C1SuperTooltip1.SetToolTip(btnTallasColores, "<B>Ver Tallas y Colores</B> de este registro")

        MenuBarra.ImageList = ImageList1

        btnAgregar.Image = ImageList1.Images(0)
        btnEditar.Image = ImageList1.Images(1)
        btnEliminar.Image = ImageList1.Images(2)
        btnBuscar.Image = ImageList1.Images(3)
        btnSeleccionar.Image = ImageList1.Images(4)
        btnPrimero.Image = ImageList1.Images(6)
        btnAnterior.Image = ImageList1.Images(7)
        btnSiguiente.Image = ImageList1.Images(8)
        btnUltimo.Image = ImageList1.Images(9)
        btnImprimir.Image = ImageList1.Images(10)
        btnSalir.Image = ImageList1.Images(11)
        btnTallasColores.Image = ImageList1.Images(12)

        btnTallasColores.Visible = False

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        If c >= 0 AndAlso ds.Tables(nTabla).Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = c
            With dt
                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)
            End With
            dg.CurrentCell = dg(0, c)
        End If
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
    End Sub
    Private Sub IniciarGrilla(ByVal TipoLista As Integer)
        Select Case TipoLista
            Case TipoListaPrecios.Normal

                Dim aCampos() As String = {"codart", "nomart", "iva", "existencia", "unidad"}
                Dim aNombres() As String = {"Código", "Descripción", "IVA", "Existencia", "UND"}
                Dim aAnchos() As Long = {80, 300, 30, 70, 40}
                Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro}
                Dim aFormatos() As String = {"", "", "", sFormatoCantidad, ""}
                IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

                Visualizar(0)
                ArreglaAnchoVentana(aAnchos)

            Case TipoListaPrecios.Costos, TipoListaPrecios.CostosTodos

                Dim aCampos() As String = {"codart", "nomart", "ALTERNO", "iva", "existencia", "unidad", "montoultimacompra", "costo_prom", "costo_prom_des"}
                Dim aNombres() As String = {"Código", "Descripción", "ALTERNO", "IVA", "Existencia", "UND", "Ultimo costo", "Costo Promedio", "Costo Promedio y Dsctos"}
                Dim aAnchos() As Long = {80, 300, 80, 30, 100, 40, 100, 100, 140}
                Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                                AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}
                Dim aFormatos() As String = {"", "", "", "", sFormatoCantidad, "", sFormatoNumero, sFormatoNumero, sFormatoNumero}
                IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

                If TipoLista = TipoListaPrecios.CostosTodos Then
                    Visualizar(0)
                Else
                    Visualizar(1)
                End If

                ArreglaAnchoVentana(aAnchos)

            Case TipoListaPrecios.CostosSugerido

                Dim aCampos() As String = {"codart", "nomart", "ALTERNO", "iva", "existencia", "inventario", "sugerido", "unidad", "montoultimacompra"}
                Dim aNombres() As String = {"Código", "Descripción", "ALTERNO", "IVA", "Existencia", "Inventario (Días)", "Sugerido", "UND", "Ultimo costo"}
                Dim aAnchos() As Long = {80, 350, 80, 30, 90, 90, 90, 40, 130}
                Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro,
                                                AlineacionDataGrid.Derecha, _
                                                AlineacionDataGrid.Derecha,
                                                AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha}

                Dim aFormatos() As String = {"", "", "", "", sFormatoCantidad, sFormatoEntero, sFormatoCantidad, "", sFormatoNumero}
                IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

                Visualizar(2)
                ArreglaAnchoVentana(aAnchos)

            Case TipoListaPrecios.Precios

                Dim aCampos() As String = {"codart", "nomart", "iva", "existencia", "unidad", "precio_a", "precio_b", "precio_c", "precio_d", "precio_e", "precio_f"}
                Dim aNombres() As String = {"Código", "Descripción", "IVA", "Existencia", "UND", "Precio A", "Precio B", "Precio C", "Precio D", "Precio E", "Precio F"}
                Dim aAnchos() As Long = {80, 300, 30, 65, 40, 60, 60, 60, 60, 60, 60}
                Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                                AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                                AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}
                Dim aFormatos() As String = {"", "", "", sFormatoCantidad, "", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero}
                IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

                Visualizar(0)
                ArreglaAnchoVentana(aAnchos)

            Case TipoListaPrecios.Precios_IVA

                Dim aCampos() As String = {"codart", "nomart", "iva", "existencia", "unidad", "precio_a_iva", "precio_b_iva", "precio_c_iva", "precio_d_iva", "precio_e_iva", "precio_f_iva"}
                Dim aNombres() As String = {"Código", "Descripción", "IVA", "Existencia", "UND", "Precio A + IVA ", "Precio B + IVA ", "Precio C + IVA ", "Precio D + IVA ", "Precio E + IVA ", "Precio F + IVA "}
                Dim aAnchos() As Long = {80, 300, 30, 65, 40, 60, 60, 60, 60, 60, 60}
                Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                                AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, _
                                                AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}
                Dim aFormatos() As String = {"", "", "", sFormatoCantidad, "", sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero, sFormatoNumero}
                If TARIFA <> "" Then
                    aCampos = {"codart", "nomart", "iva", "existencia", "unidad", "precio_" & TARIFA & "_iva", "alterno", "BARRAS"}
                    aNombres = {"Código", "Descripción", "IVA", "Exist.", "UND", "Precio " & TARIFA & " + IVA ", "Código ALTERNO", "CODIGO BARRAS"}
                    aAnchos = {80, 320, 30, 65, 40, 100, 100, 200}
                    aAlineacion = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                                    AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
                    aFormatos = {"", "", "", sFormatoCantidad, "", sFormatoNumero, "", ""}
                End If
                IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

                Visualizar(0)
                ArreglaAnchoVentana(aAnchos)

        End Select




    End Sub
    Private Sub Visualizar(tipoVision As Integer)
        Select Case tipoVision
            Case 0
                VisualizarObjetos(False, grpCatalogo, grpSugerido)
                grpLista.Top = MenuBarra.Height
                grpLista.Height = Me.Height - MenuBarra.Height - (Me.Height - lblInfo.Top + 5)
            Case 1
                VisualizarObjetos(False, grpSugerido)
                grpCatalogo.Top = MenuBarra.Height
                grpLista.Top = MenuBarra.Height + grpCatalogo.Height
                grpLista.Height = Me.Height - MenuBarra.Height - grpCatalogo.Height - (Me.Height - lblInfo.Top + 5)
            Case 2
                grpCatalogo.Top = MenuBarra.Height
                grpSugerido.Top = MenuBarra.Height + grpCatalogo.Height
                grpLista.Top = MenuBarra.Height + grpCatalogo.Height + grpSugerido.Height
                grpLista.Height = Me.Height - MenuBarra.Height - grpCatalogo.Height - grpSugerido.Height - (Me.Height - lblInfo.Top + 5)
        End Select
        dg.Height = grpLista.Height - 10
    End Sub
    Private Sub ArreglaAnchoVentana(aArray() As Long)

        Dim anchoVentana As Integer = 0
        For Each iElem As Integer In aArray
            anchoVentana += iElem
        Next
        Me.Width = anchoVentana + 15
        grpLista.Width = anchoVentana

    End Sub


    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"codart", "nomart"}
        Dim Nombres() As String = {"Código", "Descripción"}
        Dim Anchos() As Long = {100, 350}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Listado de Costos/precios...")
        AsignaTXT(f.Apuntador, False)
        f = Nothing
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        Posicion = Me.BindingContext(ds, nTabla).Position
        If dt.Rows.Count > 0 AndAlso dg.RowCount > 0 Then
            Seleccionado = dg.SelectedRows(0).Cells(0).Value.ToString
            InsertarAuditoria(myConn, MovAud.iSeleccionar, Me.Text, "")
        End If
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        If dg.RowCount > 0 Then
            Me.BindingContext(ds, nTabla).Position = 0
            AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
        End If
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        If dg.RowCount > 0 Then
            Me.BindingContext(ds, nTabla).Position -= 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
        End If
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        If dg.RowCount > 0 Then
            Me.BindingContext(ds, nTabla).Position += 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
        End If
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        If dg.RowCount > 0 Then
            Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
            AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
        End If
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        Posicion = Me.BindingContext(ds, nTabla).Position
        If dt.Rows.Count > 0 AndAlso dg.RowCount > 0 Then
            Seleccionado = dg.SelectedRows(0).Cells(0).Value.ToString
            InsertarAuditoria(myConn, MovAud.iSeleccionar, Me.Text, "")
        End If
        Me.Close()

    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.CellMouseClick
        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        AsignaTXT(e.RowIndex, False)
    End Sub

    Private Sub dg_RegionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.RegionChanged
        Posicion = Me.BindingContext(ds, nTabla).Position
    End Sub

    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        BindingSource1.DataSource = dt
        If dt.Columns(FindField).DataType Is GetType(String) Then _
            BindingSource1.Filter = FindField & " like '%" & txtBuscar.Text & "%'"
        dg.DataSource = BindingSource1
    End Sub

    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.ColumnHeaderMouseClick

        If nTipoLista = TipoListaPrecios.Precios_IVA And TARIFA <> "" Then
            Dim aCam() As String = {"codart", "nomart", "iva", "existencia", "unidad", "precio_" & TARIFA & "_iva", "alterno", "BARRAS"}
            Dim aStr() As String = {"Código", "Descripción", "IVA", "Existencia", "UND", "Precio " & TARIFA & " + IVA ", "Código ALTERNO", "CODIGO BARRAS"}
            If e.ColumnIndex < 2 Or e.ColumnIndex > 5 Then
                FindField = dt.Columns(aCam(e.ColumnIndex)).ColumnName
                lblBuscar.Text = aStr(e.ColumnIndex)
            End If
        Else
            If TipoListaPrecios.Costos Then
                Dim aCam() As String = {"codart", "nomart", "ALTERNO"}
                Dim aStr() As String = {"Código", "Nombre", "ALTERNO"}
                If e.ColumnIndex <= 2 Then
                    FindField = dt.Columns(aCam(e.ColumnIndex)).ColumnName
                    lblBuscar.Text = aStr(e.ColumnIndex)
                End If
            Else
                Dim aCam() As String = {"codart", "nomart"}
                Dim aStr() As String = {"Código", "Nombre"}
                If e.ColumnIndex < 2 Then
                    FindField = dt.Columns(aCam(e.ColumnIndex)).ColumnName
                    lblBuscar.Text = aStr(e.ColumnIndex)
                End If
            End If
        End If
        txtBuscar.Focus()

    End Sub

    Private Sub txtBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBuscar.Click

    End Sub

    Private Sub txtPromedioMeses_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPromedioMeses.KeyPress,
        txtSugeridoDias.KeyPress
        e.Handled = ValidaNumeroEnTextbox(e)
    End Sub

    Private Sub txtPromedioMeses_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPromedioMeses.TextChanged, txtSugeridoDias.TextChanged
        If _Control > 1 Then CambiarGrilla()
        _Control += 1
    End Sub

    Private Sub CambiarGrilla()
        If myConn.State = ConnectionState.Open Then

            If ValorEntero(txtPromedioMeses.Text) > 0 Then nMeses = ValorEntero(txtPromedioMeses.Text)
            If ValorEntero(txtSugeridoDias.Text) > 0 Then nDiasSugerido = ValorEntero(txtSugeridoDias.Text)
            Iniciar(strSQL, nTipoLista, CodigoAlmacen)
        End If
    End Sub

    Private Sub jsMerArcListaCostosPrecios_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        txtBuscar.Focus()
    End Sub

    Private Sub cmbEstatus_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbEstatus.SelectedIndexChanged
        If _Control > 1 Then CambiarGrilla()
        _Control += 1
    End Sub
End Class