Imports MySql.Data.MySqlClient

Public Class jsControlArcTablaSimpleSeleccion

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private dsTS As New DataSet
    Private dtTS As New DataTable
    Private ft As New Transportables

    Private Const nCarga1 As Integer = 1
    Private Const nCarga2 As Integer = 2

    Private strSQL As String

    Private sModulo As String, Modulo As String

    Dim aCampos() As String = {"codigo", "descripcion"}
    Dim aNombres() As String = {"Código", "Nombre"}
    Dim aAnchos() As Integer = {150, 500}
    Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Left, HorizontalAlignment.Left}
    Dim aFormatos() As FormatoItemListView = {FormatoItemListView.iCadena, FormatoItemListView.iCadena}

    Private n_Apuntador As Long
    Private n_Seleccion As Object
    Private Seleccionar As Boolean = True
    Public Property Seleccion() As Object
        Get
            Return n_Seleccion
        End Get
        Set(ByVal value As Object)
            n_Seleccion = value
        End Set
    End Property
    Public Sub Cargar(ByVal NombreModulo As String, ByVal ds As DataSet, ByVal dt As DataTable, _
                      Optional ByVal Codigo As String = "")
        dsTS = ds
        dtTS = dt
        Me.Text = NombreModulo
        Me.Tag = NombreModulo

        CargaListViewDesdeTabla(lv, dtTS, aNombres, aCampos, aAnchos, aAlineacion, aFormatos)
        SeleccionarItems()

        AsignarTooltips()

        If dtTS.Rows.Count > 0 Then n_Apuntador = 0

        Me.BringToFront()
        Me.ShowDialog()

    End Sub
    Private Sub SeleccionarItems()
        Dim iCont As Integer
        For iCont = 0 To lv.Items.Count - 1
            lv.Items(iCont).Checked = False
            If Not Seleccion Is Nothing Then _
                If ft.InArray(Seleccion, lv.Items(iCont).Text) >= 0 Then lv.Items(iCont).Checked = True
        Next
    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        C1SuperTooltip1.SetToolTip(btnAgregar, "<B>Seleccionar/Deseleccionar</B> todos los registros")
        C1SuperTooltip1.SetToolTip(btnSeleccionar, "<B>Escoger</B> registros seleccionados")
        C1SuperTooltip1.SetToolTip(btnSalir, "<B>Cerrar</B> esta ventana")

        MenuBarra.ImageList = ImageList1

        btnAgregar.Image = ImageList1.Images(1)
        btnSeleccionar.Image = ImageList1.Images(3)
        btnSalir.Image = ImageList1.Images(2)


    End Sub


    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        n_Apuntador = -1
        Me.Dispose()
    End Sub

    Private Sub jsConTablaSimple_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dtTS.Dispose()
        dtTS = Nothing
        myConn.Close()
    End Sub

    Private Sub jsConTablaSimple_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim iCont As Integer
        For iCont = 0 To lv.Items.Count - 1
            lv.Items(iCont).Checked = Seleccionar
        Next
        Seleccionar = Not Seleccionar
    End Sub


    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        Dim iCont As Integer
        Dim dataArray As New ArrayList
        For iCont = 0 To lv.Items.Count - 1
            If lv.Items(iCont).Checked Then dataArray.Add(lv.Items(iCont).Text)
        Next
        Seleccion = dataArray

        Me.Close()
        Me.Dispose()


    End Sub
End Class