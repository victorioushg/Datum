Imports MySql.Data.MySqlClient
Public Class jsGenTablaArchivos
    Private Const sModulo As String = "Almacenamiento de Imágenes y Archivos"
    Private Const lRegion As String = ""
    Private Const nTabla As String = "tblAlmImagenes"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private strSQL As String = ""
    Private Posicion As Long

    Private codeID As String
    Private origenGestion As String
    Private origenModulo As String

    Private CaminoArchivo As String = ""


    Private n_Seleccionado As String
    Public Property Seleccionado() As String
        Get
            Return n_Seleccionado
        End Get
        Set(ByVal value As String)
            n_Seleccionado = value
        End Set
    End Property
    Public Sub Cargar(ByVal Mycon As MySqlConnection, ByVal TipoCarga As Integer, Codigo As String, GestionOrigen As String, _
                      ModuloOrigen As String)

        ' 0 = show() ; 1 = showdialog()

        myConn = Mycon
        codeID = Codigo
        origenGestion = GestionOrigen
        origenModulo = ModuloOrigen
        ColocaNombre(origenGestion, origenModulo)

        strSQL = "select * from jscontablaarchivos " _
            & " where " _
            & " codigo = '" & codeID & "' AND " _
            & " gestion_origen = '" & origenGestion & "' AND " _
            & " modulo_origen = '" & origenModulo & "' AND " _
            & " id_emp = '" & jytsistema.WorkID & "' " _
            & " order by renglon "

        dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)
        IniciarGrilla()
        AsignarTooltips()
        
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.tipoMensaje.iAyuda)

        If TipoCarga = 0 Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If


    End Sub
    Private Sub ColocaNombre(gestionOrigen As String, moduloOrigen As String)
        Select Case gestionOrigen & moduloOrigen
            Case "NOMEXP"
                Me.Text += " - Recursos Humanos - Expediente - " & codeID
            Case "PROFOR"
                Me.Text += " - Producción - Formulación - " & codeID
        End Select
    End Sub
    Private Sub jsControlArcAlmacenes_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt = Nothing
        ds = Nothing
    End Sub

    Private Sub jsControlArcAlmacenes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
   
    Private Sub IniciarGrilla()

        lvArchivos.View = View.SmallIcon

        Dim aNombres() As String = {"Nombre", "Extensión", "codigo", "renglon"}
        Dim aCampos() As String = {"nombre", "Extension", "codigo", "renglon"}
        Dim aAnchos() As Integer = {150, 50, 50, 50}
        Dim aAlineacion() As HorizontalAlignment = {HorizontalAlignment.Center, HorizontalAlignment.Center, _
                                                    HorizontalAlignment.Center, HorizontalAlignment.Center}
        Dim aFormato() As FormatoItemListView = {FormatoItemListView.iCadena, FormatoItemListView.iCadena, _
                                                 FormatoItemListView.iCadena, FormatoItemListView.iCadena}

        CargaListViewDesdeTablaPlus(lvArchivos, dt, aNombres, aCampos, aAnchos, aAlineacion, aFormato)




    End Sub

    Private Sub CargaListViewDesdeTablaPlus(ByVal LV As ListView, ByVal dt As DataTable, ByVal aNombres() As String, _
                                       ByVal aCampos() As String, ByVal aAnchos() As Integer, ByVal aAlineacion() As System.Windows.Forms.HorizontalAlignment, _
                                       ByVal aFormato() As FormatoItemListView)
        Dim iCont As Integer
        Dim jCont As Integer

        LV.Clear()
        LV.BeginUpdate()

        LV.SmallImageList = imgList

        For iCont = 0 To aNombres.Length - 1
            LV.Columns.Add(aNombres(iCont).ToString, aAnchos(iCont), aAlineacion(iCont))
        Next

        For iCont = 0 To dt.Rows.Count - 1
            With dt.Rows(iCont)
                LV.Items.Add(FormatoCampoListView(.Item(aCampos(0).ToString).ToString, aFormato(0)))
                For jCont = 1 To aCampos.Length - 1
                    LV.Items(iCont).SubItems.Add(FormatoCampoListView(.Item(aCampos(jCont).ToString).ToString, aFormato(jCont)))
                    If aCampos(jCont).ToUpper() = "EXTENSION" Then
                        Select Case .Item(aCampos(jCont)).ToString.ToUpper()
                            Case "JPG"
                                LV.Items(iCont).ImageIndex = 0
                            Case "XLS", "XLSX"
                                LV.Items(iCont).ImageIndex = 1
                            Case "DOC", "DOCX"
                                LV.Items(iCont).ImageIndex = 2
                            Case "PDF"
                                LV.Items(iCont).ImageIndex = 3
                            Case "TXT"
                                LV.Items(iCont).ImageIndex = 4
                        End Select

                    End If

                Next
            End With
        Next

        LV.EndUpdate()

    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        Dim ofd As New OpenFileDialog()

        ofd.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments  ' "c:\"
        ofd.Filter = "Imágenes JPG|*.jpg |Archivos PDF|*.pdf|Documentos Word |*.doc;*.docx|Hojas de Cálculo Excel|*.xls;*.xlsx|" _
            & "Archivos Texto sin formato|*.txt"
        ofd.FilterIndex = 2
        ofd.RestoreDirectory = True

        If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
            CaminoArchivo = ofd.FileName
            Dim nRenglon As String = ft.autoCodigo(myConn, "renglon", "jscontablaarchivos", "codigo.gestion_origen.modulo_origen.id_emp", _
                                                   codeID & "." & origenGestion & "." & origenModulo & "." & jytsistema.WorkID, 5, True)
            Dim nNombre As String = CaminoArchivo.Split("\")(CaminoArchivo.Split("\").Length - 1)

            ft.GuardarArchivoEnBaseDeDatos(myConn, True, CaminoArchivo, codeID, nRenglon, origenGestion, origenModulo, _
                                           nNombre.Split(".")(0), nNombre.Split(".")(1), jytsistema.WorkID)

            dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)
            IniciarGrilla()

        End If

        ofd = Nothing
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
     
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

        If lvArchivos.SelectedItems.Count > 0 Then
            Posicion = lvArchivos.SelectedItems(0).Index

            If dt.Rows.Count > 0 And Posicion >= 0 Then
                Dim nRenglon As String = lvArchivos.SelectedItems(0).SubItems(3).Text
                ft.Ejecutar_strSQL(myConn, " delete from jscontablaarchivos " _
                                    & " where " _
                                    & " codigo = '" & codeID & "' AND " _
                                    & " RENGLON = '" & nRenglon & "' AND " _
                                    & " GESTION_ORIGEN = '" & origenGestion & "' AND " _
                                    & " MODULO_ORIGEN = '" & origenModulo & "' AND " _
                                    & " ID_EMP = '" & jytsistema.WorkID & "' ")
                dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)
                IniciarGrilla()
            End If
        End If



    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
      
    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
      
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
      
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
     
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
      
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
     
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub


    Private Sub lvArchivos_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lvArchivos.MouseDoubleClick

        Posicion = lvArchivos.SelectedItems(0).Index
        Dim ext As String = "." & lvArchivos.SelectedItems(0).SubItems(1).Text
        Dim nNombre As String = lvArchivos.SelectedItems(0).Text
        CaminoArchivo = ft.ArchivoEnBaseDatos_A_ArchivoEnDisco(dt.Rows(Posicion), "archivo", nNombre, ext)

        If CaminoArchivo.Trim() <> "" Then
            Dim process As Process = New Process()
            process.StartInfo.FileName = CaminoArchivo
            process.Start()
        End If

    End Sub
End Class