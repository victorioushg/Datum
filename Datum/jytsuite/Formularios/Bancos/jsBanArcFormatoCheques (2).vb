Imports MySql.Data.MySqlClient

Public Class jsBanArcFormatoCheques

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private dsTS As New DataSet
    Private dtTS As New DataTable

    Private Const lRegion As String = ""
    Private nTablaTS As String = "tblFormato"
    Private strSQL As String

    Private sModulo As String, Modulo As String

    Private bIncModEli As Boolean = False

    Dim aCampos() As String = {"formato", "descrip", "montotop", "montoleft", "nombretop", "nombreleft", "montoletratop", "montoletraleft", _
                               "fechatop", "fechaleft", "noendosabletop", "noendosableleft"}

    Dim aNombres() As String = {"Código", "Bancos", "Monto Y", "Monto X", "Nombre Y", "Nombre X", "Letra Y", "Letra X", _
                                "Fecha Y", "Fecha X", "Endoso Y", "Endoso X"}
    Dim aAnchos() As Long = {50, 200, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50}
    Dim aAlineacion() As Integer = {DataGridViewContentAlignment.MiddleCenter, DataGridViewContentAlignment.MiddleLeft, _
                                    DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.MiddleRight, _
                                    DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.MiddleRight, _
                                    DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.MiddleRight, _
                                    DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.MiddleRight, _
                                    DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.MiddleRight}
    Dim aFormatos() As String = {sFormatoEntero, "", sFormatoEntero, sFormatoEntero, sFormatoEntero, sFormatoEntero, _
                                 sFormatoEntero, sFormatoEntero, sFormatoEntero, sFormatoEntero, sFormatoEntero, sFormatoEntero}

    Private n_Apuntador As Long
    Private n_Seleccion As String
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Property Seleccion() As String
        Get
            Return n_Seleccion
        End Get
        Set(ByVal value As String)
            n_Seleccion = value
        End Set
    End Property

    Public Sub Cargar()
        Try
            strSQL = "select * from jsbancatfor where id_emp = '" & jytsistema.WorkID & "' order by formato "
            myConn.Open()

            dsTS = DataSetRequery(dsTS, strSQL, myConn, nTablaTS, lblInfo)
            dtTS = dsTS.Tables(nTablaTS)

            IniciarTabla(dg, dtTS, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

            If dtTS.Rows.Count > 0 Then n_Apuntador = 0
            AsignaTXT(n_Apuntador, False)
            AsignarTooltips()

            Me.ShowDialog()
            Me.Tag = sModulo

            MensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", TipoMensaje.iAyuda)

        Catch ex As MySqlException
            MensajeCritico(lblInfo, "Error en conexión de base de datos: " & ex.Message)
        Catch ex As Exception
            MensajeCritico(lblInfo, "Error " & ex.Message)
        End Try
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
        If Actualiza Then dsTS = DataSetRequery(dsTS, strSQL, myConn, nTablaTS, lblInfo)
        If c >= 0 AndAlso dsTS.Tables(nTablaTS).Rows.Count > 0 Then
            Me.BindingContext(dsTS, nTablaTS).Position = c
            With dtTS
                MostrarItemsEnMenuBarra(MenuBarra, "items", "lblitems", nRow, .Rows.Count)
            End With
            dg.CurrentCell = dg(0, c)
        End If
        ActivarMenuBarra(myConn, lblInfo, lRegion, dsTS, dtTS, MenuBarra)
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        n_Apuntador = CInt(dtTS.Rows(Me.BindingContext(dsTS, nTablaTS).Position).Item("formato").ToString)
        Seleccion = dtTS.Rows(Me.BindingContext(dsTS, nTablaTS).Position).Item("formato").ToString
        Me.Close()
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        Me.BindingContext(dsTS, nTablaTS).Position = 0
        AsignaTXT(Me.BindingContext(dsTS, nTablaTS).Position, False)
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        Me.BindingContext(dsTS, nTablaTS).Position -= 1
        AsignaTXT(Me.BindingContext(dsTS, nTablaTS).Position, False)
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        Me.BindingContext(dsTS, nTablaTS).Position += 1
        AsignaTXT(Me.BindingContext(dsTS, nTablaTS).Position, False)
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        Me.BindingContext(dsTS, nTablaTS).Position = dsTS.Tables(nTablaTS).Rows.Count - 1
        AsignaTXT(Me.BindingContext(dsTS, nTablaTS).Position, False)
    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        n_Apuntador = -1
        Me.Dispose()
    End Sub

    Private Sub jsBanFormatoCheques_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        myConn.Close()
    End Sub

    Private Sub jsBanFormatoCheques_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", TipoMensaje.iAyuda)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim f As New jsBanArcFormatoChequesMovimiento
        f.Agregar(myConn, dsTS, dtTS)
        dsTS = DataSetRequery(dsTS, strSQL, myConn, dtTS.TableName, lblInfo)
        If f.Apuntador >= 0 Then AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        Dim f As New jsBanArcFormatoChequesMovimiento
        f.Apuntador = Me.BindingContext(dsTS, dtTS.TableName).Position
        f.Editar(myConn, dsTS, dtTS)
        dsTS = DataSetRequery(dsTS, strSQL, myConn, dtTS.TableName, lblInfo)
        AsignaTXT(f.Apuntador, True)
        f = Nothing
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        'If bIncModEli Then
        Apuntador = Me.BindingContext(dsTS, nTablaTS).Position
        EliminaFila()
        ' End If
    End Sub
    Private Sub EliminaFila()
        Dim sRespuesta As Microsoft.VisualBasic.MsgBoxResult
        Dim aCamposDel() As String = {"formato", "id_emp"}
        Dim aStringsDel() As String = {dtTS.Rows(Apuntador).Item("formato"), jytsistema.WorkID}
        sRespuesta = MsgBox(" ¿ Esta Seguro que desea eliminar registro ?", MsgBoxStyle.YesNo, "Eliminar registro ... ")
        If sRespuesta = MsgBoxResult.Yes Then
            AsignaTXT(EliminarRegistros(myConn, lblInfo, dsTS, nTablaTS, "jsbancatfor", strSQL, aCamposDel, aStringsDel, _
                                     Apuntador, True), True)
        End If
    End Sub
    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        n_Apuntador = CInt(dtTS.Rows(Me.BindingContext(dsTS, nTablaTS).Position).Item("formato").ToString)
        Seleccion = dtTS.Rows(Me.BindingContext(dsTS, nTablaTS).Position).Item("formato").ToString
        Me.Close()
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(dsTS, nTablaTS).Position = e.RowIndex
        AsignaTXT(e.RowIndex, False)
    End Sub

    Private Sub dg_RegionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.RegionChanged
        n_Apuntador = Me.BindingContext(dsTS, nTablaTS).Position
        Seleccion = dtTS.Rows(Me.BindingContext(dsTS, nTablaTS).Position).Item("formato").ToString
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        f.Buscar(dtTS, aCampos, aNombres, aAnchos, n_Apuntador, " Formnato de Cheques")
        AsignaTXT(f.Apuntador, False)
        f = Nothing
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim f As New jsBanRepParametros
        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteBancos.cFormatoCheque, "FORMATO CHEQUE", , dtTS.Rows(Me.BindingContext(dsTS, nTablaTS).Position).Item("formato"))
        f = Nothing
    End Sub
    Private Sub dg_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dg.KeyUp

        Select Case e.KeyCode
            Case Keys.Down
                Me.BindingContext(dsTS, nTablaTS).Position += 1
                AsignaTXT(Me.BindingContext(dsTS, nTablaTS).Position, False)
            Case Keys.Up
                Me.BindingContext(dsTS, nTablaTS).Position -= 1
                AsignaTXT(Me.BindingContext(dsTS, nTablaTS).Position, False)
        End Select

    End Sub
End Class