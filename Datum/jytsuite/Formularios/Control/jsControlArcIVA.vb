Imports MySql.Data.MySqlClient

Public Class jsControlArcIVA
    Private Const sModulo As String = "Tasas de Impuesto al Valor Agregado"
    Private Const lRegion As String = "RibbonButton170"
    Private Const nTabla As String = "tblIVA"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private strSQL As String = " SELECT a.fecha, a.tipo, b.monto, b.desde, b.hasta, b.monto_1, b.desde_1, b.hasta_1, b.monto_2, b.desde_2, b.hasta_2 FROM (SELECT MAX(fecha) fecha, tipo " _
            & " FROM jsconctaiva " _
            & " WHERE fecha <= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' " _
            & " GROUP BY tipo) a " _
            & " LEFT JOIN jsconctaiva b ON (a.fecha = b.fecha AND a.tipo = b.tipo) " _
            & " ORDER BY a.tipo "

    Private Posicion As Long

    Private n_Seleccionado As String


    Public Property Seleccionado() As String
        Get
            Return n_Seleccionado
        End Get
        Set(ByVal value As String)
            n_Seleccionado = value
        End Set
    End Property
    Public Sub Cargar(ByVal Mycon As MySqlConnection, ByVal TipoCarga As Integer)

        ' 0 = show() ; 1 = showdialog()

        myConn = Mycon

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        IniciarGrilla()
        AsignarTooltips()
        If dt.Rows.Count > 0 Then Posicion = 0
        AsignaTXT(Posicion, True)
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)

        If TipoCarga = 0 Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If


    End Sub
    Private Sub jsControlArcIVA_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt = Nothing
        ds = Nothing
    End Sub

    Private Sub jsControlArcIVA_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, jytsistema.WorkLanguage, btnAgregar, btnEditar, btnSeleccionar, btnSalir)
    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean)
        dt = ft.MostrarFilaEnTabla(myConn, ds, nTabla, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, jytsistema.sUsuario, _
                               nRow, Actualiza)
    End Sub
    Private Sub IniciarGrilla()

        Dim aCampos() As String = {"tipo.Tasa.40.C.", _
                                   "Fecha.FECHA.80.C.fecha", _
                                   "Desde.Desde.120.D.Numero", _
                                   "HASTA.Hasta.120.D.Numero", _
                                   "monto.% IVA.60.D.Numero", _
                                   "Desde_1.Desde.120.D.Numero", _
                                   "HASTA_1.Hasta.120.D.Numero", _
                                   "monto_1.% IVA.60.D.Numero", _
                                   "Desde_2.Desde.120.D.Numero", _
                                   "HASTA_2.Hasta.120.D.Numero", _
                                   "monto_2.% IVA.60.D.Numero"}

        ft.IniciarTablaPlus(dg, dt, aCampos, , , New Font("Consolas", 8, FontStyle.Regular))

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

        Dim f As New jsControlArcIVAMovimientos
        f.Agregar(myConn, ds, dt)

        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        Dim row As DataRow = dt.Select(" TIPO = '" & f.TIPO & "' ")(0)
        Dim APUNTADOR As Long = dt.Rows.IndexOf(row)
        Me.BindingContext(ds, nTabla).Position = APUNTADOR
        If f.Apuntador >= 0 Then
            AsignaTXT(APUNTADOR, True)
        Else
            If dt.Rows.Count > 0 Then AsignaTXT(0, True)
        End If


        f = Nothing

    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim f As New jsControlArcIVAMovimientos
        f.Apuntador = Me.BindingContext(ds, nTabla).Position
        f.Editar(myConn, ds, dt)
        AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub

    'Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

    '    Posicion = Me.BindingContext(ds, nTabla).Position

    '    If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then

    '        Dim aCampos() As String = {"tipo", "fecha"}
    '        With dt.Rows(Posicion)
    '            Dim aString() As String = {.Item("tipo"), ft.FormatoFechaMySQL(CDate(.Item("fecha").ToString))}
    '            Posicion = EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsconctaiva", strSQL, aCampos, aString, Posicion)
    '        End With

    '    End If
    '    AsignaTXT(Posicion, False)
    'End Sub


    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        Posicion = Me.BindingContext(ds, nTabla).Position
        If dt.Rows.Count > 0 Then
            Seleccionado = dt.Rows(Posicion).Item("tipo").ToString
            InsertarAuditoria(myConn, MovAud.iSeleccionar, Me.Text, "")
        End If
        Me.Close()
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        Posicion = Me.BindingContext(ds, nTabla).Position
        If dt.Rows.Count > 0 Then
            Seleccionado = dt.Rows(Posicion).Item("tipo").ToString
            InsertarAuditoria(myConn, MovAud.iSeleccionar, Me.Text, "")
        End If
        Me.Close()
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.CellMouseClick
        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        Posicion = Me.BindingContext(ds, nTabla).Position
        AsignaTXT(Posicion, False)
    End Sub

    Private Sub dg_RegionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.RegionChanged
        Posicion = Me.BindingContext(ds, nTabla).Position
    End Sub

End Class