Imports MySql.Data.MySqlClient
Public Class jsMerArcComponentesCombo
    Private Const sModulo As String = "Listado componentes combo mercancías"
    Private Const lRegion As String = ""
    Private Const nTabla As String = "tblLisCombo"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable

    Private strSQL As String
    Private Posicion As Long

    Private CodigoMercancia As String
    Private n_CostoTotal As Double
    Private n_PesoTotal As Double
    Public Property CostoTotal() As Double
        Get
            Return n_CostoTotal
        End Get
        Set(ByVal value As Double)
            n_CostoTotal = value
        End Set
    End Property
    Public Property PesoTotal() As Double
        Get
            Return n_PesoTotal
        End Get
        Set(ByVal value As Double)
            n_PesoTotal = value
        End Set
    End Property

    Public Property precioTotalA As Double
    Public Property precioTotalB As Double
    Public Property precioTotalC As Double
    Public Property precioTotalD As Double
    Public Property precioTotalE As Double
    Public Property precioTotalF As Double



    Public Sub Cargar(ByVal Mycon As MySqlConnection, ByVal CodigoArticulo As String)

        myConn = Mycon
        Me.Tag = sModulo
        CodigoMercancia = CodigoArticulo

        strSQL = " select * from jsmercatcom where codart = '" & CodigoMercancia & "' AND ID_EMP = '" & jytsistema.WorkID & "' order by codartcom "

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        IniciarGrilla()
        AsignarTooltips()
        If dt.Rows.Count > 0 Then Posicion = 0
        AsignaTXT(Posicion, True)
        MensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", TipoMensaje.iAyuda)

        Me.ShowDialog()

    End Sub
    Private Sub jsMerArcComponentesCombo_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt = Nothing
        ds = Nothing
    End Sub

    Private Sub jsMerArcComponentesCombo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        HabilitarObjetos(False, True, txtCostos, txtPesos)
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
            ColocaTotalesPlus()
        End If
        ActivarMenuBarra(myConn, lblInfo, lRegion, ds, dt, MenuBarra)
    End Sub
    Private Sub ColocaTotalesPlus()
        CostoTotal = 0.0
        PesoTotal = 0.0
        precioTotalA = 0.0
        precioTotalB = 0.0
        precioTotalC = 0.0
        precioTotalD = 0.0
        precioTotalE = 0.0
        precioTotalF = 0.0

        For Each nRow As DataRow In dt.Rows
            With nRow
                CostoTotal += .Item("costo")
                PesoTotal += .Item("peso")
                If .Item("CODARTCOM").ToString.Substring(0, 1) = "$" Then
                    precioTotalA += CDbl(EjecutarSTRSQL_ScalarPLUS(myConn, " select precio from jsmercatser where codser = '" & .Item("codartcom").ToString.Substring(1) & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    precioTotalB += CDbl(EjecutarSTRSQL_ScalarPLUS(myConn, " select precio from jsmercatser where codser = '" & .Item("codartcom").ToString.Substring(1) & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    precioTotalC += CDbl(EjecutarSTRSQL_ScalarPLUS(myConn, " select precio from jsmercatser where codser = '" & .Item("codartcom").ToString.Substring(1) & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    precioTotalD += CDbl(EjecutarSTRSQL_ScalarPLUS(myConn, " select precio from jsmercatser where codser = '" & .Item("codartcom").ToString.Substring(1) & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    precioTotalE += CDbl(EjecutarSTRSQL_ScalarPLUS(myConn, " select precio from jsmercatser where codser = '" & .Item("codartcom").ToString.Substring(1) & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    precioTotalF += CDbl(EjecutarSTRSQL_ScalarPLUS(myConn, " select precio from jsmercatser where codser = '" & .Item("codartcom").ToString.Substring(1) & "' and id_emp = '" & jytsistema.WorkID & "' "))
                Else
                    precioTotalA += CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select precio_A from jsmerctainv where codart = '" & .Item("CODARTCOM") & "' and id_emp = '" & jytsistema.WorkID & "' ")) / Equivalencia(myConn, lblInfo, .Item("CODARTCOM"), .Item("UNIDAD"))
                    precioTotalB += CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select precio_B from jsmerctainv where codart = '" & .Item("CODARTCOM") & "' and id_emp = '" & jytsistema.WorkID & "' ")) / Equivalencia(myConn, lblInfo, .Item("CODARTCOM"), .Item("UNIDAD"))
                    precioTotalC += CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select precio_C from jsmerctainv where codart = '" & .Item("CODARTCOM") & "' and id_emp = '" & jytsistema.WorkID & "' ")) / Equivalencia(myConn, lblInfo, .Item("CODARTCOM"), .Item("UNIDAD"))
                    precioTotalD += CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select precio_D from jsmerctainv where codart = '" & .Item("CODARTCOM") & "' and id_emp = '" & jytsistema.WorkID & "' ")) / Equivalencia(myConn, lblInfo, .Item("CODARTCOM"), .Item("UNIDAD"))
                    precioTotalE += CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select precio_E from jsmerctainv where codart = '" & .Item("CODARTCOM") & "' and id_emp = '" & jytsistema.WorkID & "' ")) / Equivalencia(myConn, lblInfo, .Item("CODARTCOM"), .Item("UNIDAD"))
                    precioTotalF += CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " select precio_F from jsmerctainv where codart = '" & .Item("CODARTCOM") & "' and id_emp = '" & jytsistema.WorkID & "' ")) / Equivalencia(myConn, lblInfo, .Item("CODARTCOM"), .Item("UNIDAD"))
                End If
            End With
        Next

        txtCostos.Text = FormatoNumero(CostoTotal)
        txtPesos.Text = FormatoNumero(PesoTotal)

    End Sub
    
    Private Sub IniciarGrilla()
        Dim aCampos() As String = {"codartcom", "descrip", "cantidad", "unidad", "costo", "Peso"}
        Dim aNombres() As String = {"Código", "Descripción", "Cant.", "UND", "Costo", "Peso"}
        Dim aAnchos() As Long = {100, 400, 90, 50, 100, 100}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, _
                                        AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {"", "", sFormatoCantidad, "", sFormatoNumero, sFormatoCantidad}
        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim f As New jsMerArcComponentesComboMovimiento
        f.Agregar(myConn, ds, dt, CodigoMercancia)
        Posicion = f.Apuntador
        f.Dispose()
        f = Nothing
        AsignaTXT(Posicion, True)
        ColocaTotalesPlus()
    End Sub
    Private Sub btnAgregarServicio_Click(sender As System.Object, e As System.EventArgs) Handles btnAgregarServicio.Click
        Dim f As New jsMerArcComponentesComboMovimiento
        f.Agregar(myConn, ds, dt, CodigoMercancia, True)
        Posicion = f.Apuntador
        f.Dispose()
        f = Nothing
        AsignaTXT(Posicion, True)
        ColocaTotalesPlus()
    End Sub
    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim f As New jsMerArcComponentesComboMovimiento
        f.Apuntador = Posicion
        f.Editar(myConn, ds, dt, CodigoMercancia)
        Posicion = f.Apuntador
        f.Dispose()
        f = Nothing
        AsignaTXT(Posicion, True)
        ColocaTotalesPlus()
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        EliminarMovimiento()
        ColocaTotalesPlus()
    End Sub
    Private Sub EliminarMovimiento()

        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult

        Posicion = Me.BindingContext(ds, nTabla).Position

        If Posicion >= 0 Then
            sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
            If sRespuesta = MsgBoxResult.Yes Then

                InsertarAuditoria(myConn, MovAud.iEliminar, sModulo, dt.Rows(Posicion).Item("codart") & _
                    dt.Rows(Posicion).Item("codartcom"))

                Dim aaFld() As String = {"codart", "codartcom", "id_emp"}
                Dim aaVal() As String = {CodigoMercancia, dt.Rows(Posicion).Item("codartcom"), jytsistema.WorkID}

                Posicion = EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsmercatcom", strSQL, aaFld, aaVal, Posicion)
                dt = ds.Tables(nTabla)
                If dt.Rows.Count - 1 < Posicion Then Posicion = dt.Rows.Count - 1
                AsignaTXT(Posicion, True)

            End If

        End If

    End Sub
    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        Posicion = Me.BindingContext(ds, nTabla).Position
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Me.BindingContext(ds, nTabla).Position = 0
        AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Me.BindingContext(ds, nTabla).Position -= 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Me.BindingContext(ds, nTabla).Position += 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Me.BindingContext(ds, nTabla).Position = ds.Tables(nTabla).Rows.Count - 1
        AsignaTXT(Me.BindingContext(ds, nTabla).Position, False)
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        Posicion = Me.BindingContext(ds, nTabla).Position
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.CellMouseClick
        Posicion = e.RowIndex
        Me.BindingContext(ds, nTabla).Position = Posicion
        AsignaTXT(Posicion, False)
    End Sub

    Private Sub dg_RegionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.RegionChanged
        Posicion = Me.BindingContext(ds, nTabla).Position
    End Sub


End Class