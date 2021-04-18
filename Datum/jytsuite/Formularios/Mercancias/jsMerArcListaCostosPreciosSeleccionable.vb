Imports MySql.Data.MySqlClient

Public Class jsMerArcListaCostosPreciosSelecionable
    Private Const sModulo As String = "Listado de mercancías Seleccionable"
    Private Const lRegion As String = ""
    Private Const nTabla As String = "tblListaMercas"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private strSQL As String = ""

    Private Posicion As Long
    Private n_Seleccionado As ArrayList

    Private nTipoLista As Integer
    Private nMeses As Integer = 3
    Private nDiasSugerido As Integer = 10
    Private sProveedor As String = ""
    Private TARIFA As String = "A"

    Private CodigoAlmacenOrigen As String = ""
    Private CodigoAlmacenDestino As String = ""

    Private CodigoProveedor As String = ""
    Private EstatusMercancia As Integer = 9

    Private BindingSource1 As New BindingSource
    Private FindField As String
    Private strEstatus As String = ""
    Private strAlmacen As String = ""

    Private _Control As Integer = 0
    Private aCatalogo() As String = {"Catálogo Proveedor", "Catálogo completo"}

    Public Property Seleccionado() As ArrayList
        Get
            Return n_Seleccionado
        End Get
        Set(ByVal value As ArrayList)
            n_Seleccionado = value
        End Set
    End Property
    Public Sub Cargar(ByVal Mycon As MySqlConnection, ByVal TipoLista As Integer, _
                      Optional ByVal AlmacenOrigen As String = "", Optional AlmacenDestino As String = "", _
                      Optional ByVal xProveedor As String = "", _
                      Optional ByVal TipoTarifa As String = "A", Optional ByVal iEstatusMercancia As Integer = 9)

        'Tipo Lista
        ' 0 = Normal ; 1=Costos ; 2=Precios ; 3=Precios+IVA; 4 = CostosSugerido

        'Estatus mercancia
        '0 = Activa; 1 Inactiva; 9 = Todas

        myConn = Mycon

        CodigoAlmacenOrigen = AlmacenOrigen
        CodigoAlmacenDestino = AlmacenDestino

        TARIFA = TipoTarifa
        EstatusMercancia = iEstatusMercancia
        CodigoProveedor = xProveedor
        nTipoLista = TipoLista

        AsignarTooltips()

        ft.habilitarObjetos(True, False, grpSugerido, txtPromedioMeses, txtSugeridoDias)
        txtPromedioMeses.Text = ft.FormatoEntero(nMeses)
        txtSugeridoDias.Text = ft.FormatoEntero(nDiasSugerido)

        If CodigoAlmacenDestino <> "" Then aCatalogo = {"Catálogo Almacén", "Catálogo Completo"}
        If TipoLista = TipoListaPrecios.CostosSugerido Or TipoLista = TipoListaPrecios.Costos _
             Or TipoLista = TipoListaPrecios.CostosSugeridoPlus Then
            ft.RellenaCombo(aCatalogo, cmbEstatus, 0)
        Else
            ft.RellenaCombo(aCatalogo, cmbEstatus, 1)
        End If


        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)
        Me.ShowDialog()

    End Sub
    Private Function IniciarCadena(Tipolista As Integer, proveedor As String, EstatusMercancia As Integer, AlmacenDestino As String) As String

        strEstatus = IIf(EstatusMercancia <> 9, " a.estatus = " & EstatusMercancia & " AND ", "")
        sProveedor = IIf(proveedor <> "", " where c.ultimoproveedor = '" & proveedor & "' ", "")
        strAlmacen = IIf(AlmacenDestino <> "", " a.almacen = '" & AlmacenDestino & "' and ", "")

        If Tipolista = TipoListaPrecios.CostosSugerido Or Tipolista = TipoListaPrecios.CostosSugeridoPlus Then

            strSQL = " SELECT 0 sel, a.codart, a.nomart, a.alterno, a.iva, ROUND(IF(b.existencia IS NULL, 0.00, b.existencia),3) existencia, " _
                & IIf(CodigoAlmacenDestino <> "", " ROUND(IF(c.existencia IS NULL, 0.00, c.existencia),3) existenciaOrigen, ", "") _
                & " ROUND(IF(b.inventario IS NULL, 0.00, b.inventario),3) inventario, " _
                & " ROUND(IF(b.sugerido IS NULL, 0.00, b.sugerido),3) sugerido, 0 pedido, a.unidad, a.montoultimacompra " _
                & " FROM jsmerctainv a " _
                & " LEFT JOIN  ( SELECT a.codart, SUM(IF( ISNULL(f.uvalencia), a.existencia, a.existencia/f.equivale)) existencia, " _
                & " SUM(IF( ISNULL(f.uvalencia), a.existencia, a.existencia/f.equivale)) / (  SUM(IF( ISNULL(f.uvalencia), a.ventas, a.ventas/f.equivale)) / d.DiasHabiles ) Inventario , " _
                & " ROUND(SUM(IF( ISNULL(f.uvalencia), " & nDiasSugerido & "*a.ventas, " & nDiasSugerido & "*a.ventas/f.equivale)) / d.DiasHabiles, 3) sugerido  " _
                & " " _
                & " FROM ( SELECT  a.codart, a.unidad,  SUM(IF( a.TIPOMOV IN( 'EN', 'AE', 'DV') , a.CANTIDAD, -1 * a.CANTIDAD )) existencia, " _
                & "             SUM(IF( a.origen IN ('FAC', 'PVE', 'PFC') AND a.fechamov >= DATE_ADD('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL -" & nMeses & " MONTH) AND  a.fechamov <= DATE_ADD('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL 1 DAY) ,  a.cantidad, 0.000 ) ) ventas, " _
                & "             a.id_emp " _
                & "             FROM jsmertramer a " _
                & "             WHERE " _
                & IIf(CodigoAlmacenDestino <> "", "             a.almacen = '" + CodigoAlmacenDestino + "' AND ", "") _
                & "      	    SUBSTRING(a.codart,1,1) <> '$' AND " _
                & "             a.tipomov <> 'AC' AND " _
                & "             a.id_emp = '" & jytsistema.WorkID & "' AND  " _
                & "             a.ejercicio = '' AND  " _
                & "             DATE_FORMAT(a.fechamov, '%Y-%m-%d') <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' GROUP BY a.codart, a.unidad ) a  " _
                & " LEFT JOIN jsmerequmer f ON (a.codart = f.codart AND a.unidad = f.uvalencia AND a.id_emp = f.id_emp )  " _
                & " LEFT JOIN jsmerctainv c ON (a.codart = c.codart AND a.id_emp = c.id_emp) " _
                & " LEFT JOIN (SELECT '1' num, (DATEDIFF('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', DATE_ADD('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "', INTERVAL -" & nMeses & " MONTH))) -  IFNULL(COUNT(*),0) DiasHabiles  " _
                & "         FROM (SELECT CONCAT(ano,'-',IF(LENGTH(mes)=1,CONCAT('0',mes),mes),'-',IF(LENGTH(dia)=1,CONCAT('0',dia),dia)) AS fecha FROM jsconcatper WHERE MODULO = 1 AND ID_EMP = '" & jytsistema.WorkID & "'  " _
                & "         HAVING  (fecha <'" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' AND fecha>DATE_ADD('" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "',INTERVAL -" & nMeses & " MONTH)) )  a) d ON ( d.num = '1') " _
                & " GROUP BY 1 ) b ON a.codart = b.codart " _
                & IIf(CodigoAlmacenDestino <> "", " LEFT JOIN (SELECT  a.codart, a.unidad,  SUM(IF( a.TIPOMOV IN( 'EN', 'AE', 'DV') , 1, -1)*( IF( ISNULL(b.uvalencia), a.cantidad, a.cantidad/b.equivale ) )) existencia, b.equivale, a.id_emp  " _
                & "            FROM jsmertramer a " _
                & "            LEFT JOIN jsmerequmer b ON (a.codart = b.codart AND a.unidad = b.uvalencia AND a.id_emp = b.id_emp) " _
                & "            WHERE " _
                & IIf(CodigoAlmacenOrigen <> "", "            a.almacen = '" & CodigoAlmacenOrigen & "' AND ", "") _
                & "            SUBSTRING(a.codart,1,1) <> '$' AND " _
                & "            a.tipomov <> 'AC' AND " _
                & "            a.id_emp = '" & jytsistema.WorkID & "' AND  " _
                & "            a.ejercicio = '' AND  " _
                & "            DATE_FORMAT(a.fechamov, '%Y-%m-%d') <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' GROUP BY a.codart) c ON a.codart = c.codart AND a.id_emp = c.id_emp ", "") _
                & " WHERE " _
                & " a.estatus = 0 AND " _
                & IIf(proveedor <> "", " a.ultimoproveedor = '" & proveedor & "' AND ", "") _
                & IIf(CodigoAlmacenDestino <> "", " not c.existencia is null and round(c.existencia,3) > 0 AND ", "") _
                & " a.id_emp = '" & jytsistema.WorkID & "'  order by a.codart"


        Else

            strSQL = " select 0 sel, a.codart, a.nomart, a.alterno, a.barras,  a.unidad, a.iva, if( isnull(sum(b.existencia)), 0.000, sum(b.existencia))  existencia, " _
                                & " a.montoultimacompra, a.costo_prom, a.costo_prom_des,  " _
                                & " a.precio_a, a.precio_b, a.precio_c, a.precio_d, a.precio_f, a.precio_e, " _
                                & " round(a.precio_a*( 1 + ifnull(c.monto,0.00)/100 ) ,0) precio_a_iva, " _
                                & " round(a.precio_b*if( isnull(c.monto),1.00, c.monto),0)  precio_b_iva, round(a.precio_c*if( isnull(c.monto),1.00, c.monto),0)  precio_c_iva, round(a.precio_d*if( isnull(c.monto),1.00, c.monto),0)  precio_d_iva, round(a.precio_f*if( isnull(c.monto),1.00, c.monto),0)  precio_e_iva, round(a.precio_e*if( isnull(c.monto),1.00, c.monto),0) precio_f_iva " _
                                & " from jsmerctainv a " _
                                & " left join (select a.* from jsmerextalm a where " & strAlmacen & " a.id_emp = '" & jytsistema.WorkID & "') b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
                                & " left join (" & SeleccionGENTablaIVA(jytsistema.sFechadeTrabajo) & ") c on a.iva = c.tipo " _
                                & " where  " _
                                & IIf(proveedor <> "", " a.ultimoproveedor = '" & proveedor & "' and ", "") _
                                & strEstatus _
                                & " a.id_emp = '" & jytsistema.WorkID & "' group by a.codart "

        End If

        ft.Ejecutar_strSQL(myConn, " drop temporary table if exists " + nTabla)
        ft.Ejecutar_strSQL(myConn, " create temporary table " + nTabla + strSQL)

        Return " SELECT * FROM " + nTabla + " ORDER BY codart "

    End Function



    Private Sub Iniciar(ByVal strSQL As String, ByVal TipoLista As Integer, ByVal Almacen As String)

        Dim frm As New frmEspere
        frm.ShowDialog()

        strSQL = IniciarCadena(nTipoLista, IIf(cmbEstatus.SelectedIndex = 0, CodigoProveedor, ""), EstatusMercancia, CodigoAlmacenDestino)

        ds = DataSetRequeryPlus(ds, nTabla, myConn, strSQL)
        dt = ds.Tables(nTabla)
        BindingSource1.DataSource = dt

        IniciarGrilla(TipoLista)

        frm.Dispose()
        frm = Nothing

        If dt.Rows.Count > 0 Then Posicion = 0
        MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, Posicion, True)

    End Sub

    Private Sub jsMerArcListaCostosPrecios_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
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

    Private Sub IniciarGrilla(ByVal TipoLista As Integer)

        Dim aCamposEdicion() As String = {}
        Select Case TipoLista
            Case TipoListaPrecios.Normal

                Dim aCampos() As String = {"codart.Código.80.I.", _
                                           "nomart.Descripción.300.I.", _
                                           "iva.IVA.30.C.", _
                                           "existencia.Existencia.70.D.Cantidad", _
                                           "unidad.UND.40.C."}

                ft.IniciarTablaPlus(dg, dt, aCampos, , , , , , , False)
                aCamposEdicion = {"sel"}
                ft.EditarColumnasEnDataGridView(dg, aCamposEdicion)
                Visualizar(0)

            Case TipoListaPrecios.Costos, TipoListaPrecios.CostosTodos

                Dim aCampos() As String = {"codart.Código.80.I.", _
                                           "nomart.Descripción.300.I.", _
                                           "ALTERNO.ALTERNO.80.I.", _
                                           "iva.IVA.30.C.", _
                                           "existencia.Existencia.100.D.Cantidad", _
                                           "unidad.UND.40.C.", _
                                           "montoultimacompra.Ultimo Costo.100.D.Numero", _
                                           "costo_prom.Costo Promedio.100.D.Numero", _
                                           "costo_prom_des.Costo Promedio y Dsctos.140.D.Numero"}

                ft.IniciarTablaPlus(dg, dt, aCampos, , , , , , , False)
                aCamposEdicion = {"sel"}
                ft.EditarColumnasEnDataGridView(dg, aCamposEdicion)

                If TipoLista = TipoListaPrecios.CostosTodos Then
                    Visualizar(0)
                Else
                    Visualizar(1)
                End If


            Case TipoListaPrecios.CostosSugerido

                Dim aCampos() As String = {"codart.Código.80.I.", _
                                           "nomart.Descripción.350.I.", _
                                           "ALTERNO.ALTERNO.80.I.", _
                                           "iva.IVA.30.C.", _
                                           "existenciaOrigen.Existencia en Origen.90.D.Cantidad", _
                                           "existencia.Existencia " + CodigoAlmacenDestino + ".90.D.Cantidad", _
                                           "inventario.Inventario (Días).90.C.entero.1.0", _
                                           "sugerido.Sugerido.90.D.Cantidad", _
                                           "pedido.A Solicitar.90.D.Cantidad", _
                                           "unidad.UND.40.C."}

                ft.IniciarTablaPlus(dg, dt, aCampos, , True, , , , , False)
                aCamposEdicion = {"sel", "pedido"}
                ft.EditarColumnasEnDataGridView(dg, aCamposEdicion)
                Visualizar(2)

            Case TipoListaPrecios.CostosSugeridoPlus

                Dim aCampos() As String = {"codart.Código.80.I.", _
                                           "nomart.Descripción.350.I.", _
                                           "ALTERNO.ALTERNO.80.I.", _
                                           "iva.IVA.30.C.", _
                                           "existencia.Existencia.90.D.Cantidad", _
                                           "inventario.Inventario (Días).90.C.entero.1.0", _
                                           "sugerido.Sugerido.90.D.Cantidad", _
                                           "pedido.A Solicitar.90.D.Cantidad", _
                                           "unidad.UND.40.C."}

                ft.IniciarTablaPlus(dg, dt, aCampos, , True, , , , , False)
                aCamposEdicion = {"sel", "pedido"}
                ft.EditarColumnasEnDataGridView(dg, aCamposEdicion)
                Visualizar(2)

            Case TipoListaPrecios.Precios

                Dim aCampos() As String = {"codart.Código.80.I.", _
                                           "nomart.Descripción.300.I.", _
                                           "iva.IVA.30.C.", _
                                           "existencia.Existencia.65.D.Cantidad", _
                                           "unidad.UND.40.C.", _
                                           "precio_a.Precio A.60.D.Numero", _
                                           "precio_b.Precio B.60.D.Numero", _
                                           "precio_c.Precio C.60.D.Numero", _
                                           "precio_d.Precio D.60.D.Numero", _
                                           "precio_e.Precio E.60.D.Numero", _
                                           "precio_f.Precio F.60.D.Numero"}

                ft.IniciarTablaPlus(dg, dt, aCampos, , , , , , , False)
                aCamposEdicion = {"sel"}
                ft.EditarColumnasEnDataGridView(dg, aCamposEdicion)

                Visualizar(0)

            Case TipoListaPrecios.Precios_IVA

                Dim aCampos() As String = {"codart.Código.80.I.", _
                                            "nomart.Descripción.300.I.", _
                                            "iva.IVA.30.C.", _
                                            "existencia.Existencia.65.D.Cantidad", _
                                            "unidad.UND.40.C.", _
                                            "precio_a_iva.Precio A & IVA.60.D.Numero", _
                                            "precio_b_iva.Precio B & IVA.60.D.Numero", _
                                            "precio_c_iva.Precio C & IVA.60.D.Numero", _
                                            "precio_d_iva.Precio D & IVA.60.D.Numero", _
                                            "precio_e_iva.Precio E & IVA.60.D.Numero", _
                                            "precio_f_iva.Precio F & IVA.60.D.Numero"}
                If TARIFA <> "" Then
                    aCampos = {"codart.Código.80.I.", _
                               "nomart.Descripción.320.I.", _
                               "iva.IVA.30.C.", _
                               "existencia.Existencia.65.D.Cantidad", _
                               "unidad.UND.40.C.", _
                               "precio_" & TARIFA & "_iva.Precio " & TARIFA & " & IVA.100.D.Numero", _
                               "alterno.Código Alterno.100.I.", _
                               "BARRAS.Código Barras.200.I."}
                End If

                ft.IniciarTablaPlus(dg, dt, aCampos, , , , , , , False)
                aCamposEdicion = {"sel"}
                ft.EditarColumnasEnDataGridView(dg, aCamposEdicion)

                Visualizar(0)


        End Select
        ArreglaAnchoVentana()



    End Sub
    Private Sub Visualizar(tipoVision As Integer)
        Select Case tipoVision
            Case 0
                ft.visualizarObjetos(False, grpCatalogo, grpSugerido)
                grpLista.Top = MenuBarra.Height
                grpLista.Height = Me.Height - MenuBarra.Height - (Me.Height - lblInfo.Top + 5)
            Case 1
                ft.visualizarObjetos(False, grpSugerido)
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
    Private Sub ArreglaAnchoVentana()

        Dim anchoVentana As Integer = 0
        For Each dgCol As DataGridViewColumn In dg.Columns
            anchoVentana += dgCol.Width
        Next
        Me.Width = anchoVentana + 50
        grpLista.Width = Me.Width - 5
        grpCatalogo.Width = Me.Width - 5
        grpSugerido.Width = Me.Width - 5
        dg.Width = Me.Width - 10

    End Sub


    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        Dim Campos() As String = {"codart", "nomart"}
        Dim Nombres() As String = {"Código", "Descripción"}
        Dim Anchos() As Integer = {100, 350}
        f.Buscar(dt, Campos, Nombres, Anchos, Me.BindingContext(ds, nTabla).Position, "Listado de Costos/precios...")
        Posicion = f.Apuntador
        MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, Posicion, False)
        f = Nothing
    End Sub
    Private Function Validado()
        For Each nRow As DataRow In dt.Rows
            If CBool(nRow.Item("Sel")) And nRow.Item("pedido") <= 0 Then
                ft.MensajeCritico("Existen FILAS Seleccionadas y no poseen pedido...")
                Return False
            End If
        Next
        Return True
    End Function
    Private Function seleccion() As ArrayList
        seleccion = New ArrayList
        For Each nRow As DataRow In dt.Rows
            If CBool(nRow.Item("Sel")) Then seleccion.Add(nRow)
        Next
    End Function
    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        Posicion = Me.BindingContext(ds, nTabla).Position
        If dt.Rows.Count > 0 AndAlso dg.RowCount > 0 Then
            If Validado() Then
                Seleccionado = seleccion()
                InsertarAuditoria(myConn, MovAud.iSeleccionar, Me.Text, "")
            End If
        End If
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        If dg.RowCount > 0 Then
            Me.BindingContext(ds, nTabla).Position = 0
            Posicion = 0
            MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, Posicion, False)
        End If
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        If dg.RowCount > 0 Then
            Me.BindingContext(ds, nTabla).Position -= 1
            Posicion = Me.BindingContext(ds, nTabla).Position
            MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, Posicion, False)
        End If
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        If dg.RowCount > 0 Then
            Me.BindingContext(ds, nTabla).Position += 1
            Posicion = Me.BindingContext(ds, nTabla).Position
            MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, Posicion, False)
        End If
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        If dg.RowCount > 0 Then
            Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
            Posicion = Me.BindingContext(ds, nTabla).Position
            MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, Posicion, False)
        End If
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        'Posicion = Me.BindingContext(ds, nTabla).Position
        'If dt.Rows.Count > 0 AndAlso dg.RowCount > 0 Then
        '    Seleccionado = seleccion()
        '    InsertarAuditoria(myConn, MovAud.iSeleccionar, Me.Text, "")
        'End If
        'Me.Close()

    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.CellMouseClick
        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        Posicion = Me.BindingContext(ds, nTabla).Position
        'MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, Posicion, False)

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
                If e.ColumnIndex >= 1 And e.ColumnIndex <= 3 Then
                    FindField = dt.Columns(aCam(e.ColumnIndex - 1)).ColumnName
                    lblBuscar.Text = aStr(e.ColumnIndex - 1)
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
        e.Handled = ft.validaNumeroEnTextbox(e)
    End Sub

    Private Sub txtPromedioMeses_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPromedioMeses.TextChanged, txtSugeridoDias.TextChanged
        If _Control > 1 Then CambiarGrilla()
        _Control += 1
    End Sub

    Private Sub CambiarGrilla()
        If myConn.State = ConnectionState.Open Then

            If ValorEntero(txtPromedioMeses.Text) > 0 Then nMeses = ValorEntero(txtPromedioMeses.Text)
            If ValorEntero(txtSugeridoDias.Text) > 0 Then nDiasSugerido = ValorEntero(txtSugeridoDias.Text)
            Iniciar(strSQL, nTipoLista, CodigoAlmacenDestino)
        End If
    End Sub

    Private Sub jsMerArcListaCostosPrecios_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        txtBuscar.Focus()
    End Sub

    Private Sub cmbEstatus_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbEstatus.SelectedIndexChanged
        If cmbEstatus.SelectedIndex = 0 Then
            If cmbEstatus.Text = "Catálogo Proveedor" Then
                lblProveedor.Text = CodigoProveedor + " " + ft.DevuelveScalarCadena(myConn, " select nombre from jsprocatpro where codpro = '" + CodigoProveedor + "' and id_emp = '" + jytsistema.WorkID + "' ")
            ElseIf cmbEstatus.Text = "Catálogo Almacén" Then
                lblProveedor.Text = CodigoAlmacenDestino + " " + ft.DevuelveScalarCadena(myConn, " select desalm from jsmercatalm where codalm = '" + CodigoAlmacenDestino + "' and id_emp = '" + jytsistema.WorkID + "' ")
            Else
                lblProveedor.Text = ""
            End If
        End If

        If _Control > 1 Then CambiarGrilla()
        _Control += 1
    End Sub

    Private Sub dg_CellMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.CellMouseUp
        dg.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub
    Private Sub dg_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellValidated

        If dg.RowCount = dt.Rows.Count Then
            Select Case dg.CurrentCell.ColumnIndex
                Case 0

                    ft.Ejecutar_strSQL(myConn, " UPDATE  " + nTabla + " set sel  = " + Convert.ToInt16(dg.CurrentCell.Value).ToString + " " _
                                        + " WHERE " _
                                        + " codart = '" + dg.CurrentRow.Cells(1).Value.ToString + "'  ")
                Case 8, 9
                    ft.Ejecutar_strSQL(myConn, " update " + nTabla + " set pedido = " & CDbl(dg.CurrentCell.Value) & " " _
                                                              & " where codart = '" & CStr(dg.CurrentRow.Cells(1).Value) & "' ")

                    'Posicion = Me.BindingContext(ds, nTabla).Position
                    'MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, Posicion, True)
                    If CDbl(dg.CurrentCell.Value) > 0 Then dt.Rows(e.RowIndex).Item(0) = True
            End Select
        End If

    End Sub
    Private Sub dg_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick
        If e.ColumnIndex = 0 Then dt.Rows(e.RowIndex).Item(0) = Not CBool(dt.Rows(e.RowIndex).Item(0).ToString)
    End Sub

    Private Sub dg_CellValidating(sender As Object, e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles dg.CellValidating

        Dim headerText As String = _
            dg.Columns(e.ColumnIndex).HeaderText

        If Not (headerText.Equals("A Solicitar")) Then Return

        If e.ColumnIndex = 9 Then
            If (String.IsNullOrEmpty(e.FormattedValue.ToString())) Then
                ft.mensajeAdvertencia("Debe indicar dígito(s) válido...")
                e.Cancel = True
            End If

            If Not ft.isNumeric(e.FormattedValue.ToString()) Then
                ft.mensajeAdvertencia("Debe indicar un número válido...")
                e.Cancel = True
            End If

            If ValorNumero(e.FormattedValue.ToString()) > dg.Rows(Posicion).Cells(5).Value Then
                ft.MensajeCritico("El pedido no puede ser mayor que la Existencia Origen")
                e.Cancel = True
            End If
        End If

    End Sub
End Class