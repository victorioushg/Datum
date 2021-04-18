Imports MySql.Data.MySqlClient

Public Class jsControlArcTablaSimple

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private dsTS As New DataSet
    Private dtTS As New DataTable
    Private ft As New Transportables

    Private Const nCarga1 As Integer = 1
    Private Const nCarga2 As Integer = 2
    Private Const lRegion As String = ""

    Private nTablaTS As String
    Private strSQL As String

    Private sModulo As String, Modulo As String

    Private bIncModEli As Boolean = False
    Private nCarga As Integer

    Dim aCampos() As String = {"codigo", "descripcion"}
    Dim aNombres() As String = {"Código", "Nombre"}
    Dim aAnchos() As Integer = {150, 500}
    Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
    Dim aFormatos() As String = {"", ""}

    Private n_Apuntador As Long
    Private n_Seleccion As String
    Private n_strSQL As String
    Private BindingSource1 As New BindingSource
    Private FindField As String
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
    Public Property stringSQL() As String
        Get
            Return n_strSQL
        End Get
        Set(ByVal value As String)
            n_strSQL = value
        End Set
    End Property
    Public Sub Cargar(ByVal NombreModulo As String, ByVal ds As DataSet, ByVal dt As DataTable, ByVal nTabla As String, _
                      ByVal TipoCarga As Integer, _
                      Optional ByVal incModEli As Boolean = True, Optional ByVal Codigo As String = "")
        nCarga = nCarga1
        dsTS = ds
        dtTS = dt
        nTablaTS = nTabla
        Me.Text = NombreModulo
        Me.Tag = NombreModulo
        bIncModEli = incModEli
        strSQL = stringSQL

        myConn.Open()

        Dim iCont As Integer
        For iCont = 0 To dt.Columns.Count - 1
            With dt.Columns(iCont)
                If ft.InArray(aCampos, .ColumnName) < 0 Then
                    aCampos = InsertArrayItemString(aCampos, iCont, .ColumnName)
                    aNombres = InsertArrayItemString(aNombres, iCont, .ColumnName)
                    Dim maxlength As Long
                    Select Case dt.Columns(iCont).DataType
                        Case System.Type.GetType("System.Double")
                            maxlength = 120
                        Case System.Type.GetType("System.DateTime")
                            maxlength = 100
                        Case System.Type.GetType("System.String")
                            maxlength = dt.AsEnumerable().Select(Function(r) r.Field(Of String)(.ColumnName).Length).Max * 10
                        Case Else
                            maxlength = 120
                    End Select

                    aAnchos = InsertArrayItemInteger(aAnchos, iCont, maxlength)
                    aAlineacion = InsertArrayItemInteger(aAlineacion, iCont, IIf(.DataType.ToString = "System.String", AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha))
                    aFormatos = InsertArrayItemString(aFormatos, iCont, IIf(.DataType.ToString = "System.String", "", _
                                                                            IIf(.DataType.ToString = "System.Double", sFormatoNumero, _
                                                                                IIf(.DataType.ToString = "System.Integer", sFormatoEntero, _
                                                                                    IIf(.DataType.ToString = "System.Date", sFormatoFecha, "")))))

                End If

            End With

        Next

        IniciarTabla(dg, dtTS, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)
        BindingSource1.DataSource = dtTS

        If Codigo <> "" Then
            Try
                Dim row As DataRow = dtTS.Select(" codigo = '" & Codigo & "' ")(0)
                n_Apuntador = dtTS.Rows.IndexOf(row)
            Catch ex As Exception
                ' ft.MensajeCritico( "Cuenta Contable no encontrada...")
            End Try

        End If

        Me.BindingContext(dsTS, nTablaTS).Position = n_Apuntador
        AsignaTXT(n_Apuntador)

        AsignarTooltips()
        txtBuscar.Focus()

        Me.BringToFront()
        Me.Width = dg.Width + 130
        If TipoCarga = TipoCargaFormulario.iShow Then
            Me.Show()
        Else
            Me.ShowDialog()
        End If

    End Sub
    Public Sub Cargar(ByVal NombreModulo As String, ByVal nModulo As String, ByVal IncModEli As Boolean, ByVal TipoCarga As Integer)
        nCarga = nCarga2
        Try
            strSQL = "select codigo, descrip descripcion from jsconctatab where modulo = '" & nModulo & "' and id_emp = '" & jytsistema.WorkID & "' order by codigo "
            nTablaTS = nModulo
            Modulo = nModulo
            sModulo = NombreModulo

            bIncModEli = IncModEli

            Me.Text = NombreModulo
            Me.Tag = NombreModulo

            myConn.Open()

            dsTS = DataSetRequery(dsTS, strSQL, myConn, nTablaTS, lblInfo)
            dtTS = dsTS.Tables(nTablaTS)

            IniciarTabla(dg, dtTS, aCampos, aNombres, aAnchos, aAlineacion, aFormatos)

            If dtTS.Rows.Count > 0 Then n_Apuntador = 0
            AsignaTXT(n_Apuntador)
            AsignarTooltips()

            txtBuscar.Focus()

            If TipoCarga = TipoCargaFormulario.iShowDialog Then
                Me.ShowDialog()
            Else
                Me.Show()
            End If

            ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)

        Catch ex As MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        Catch ex As Exception
            ' ft.mensajeCritico("Error " & ex.InnerException.ToString & ex.Message)
        End Try
    End Sub
    Private Sub AsignarTooltips()
        'Menu Barra 
        ft.colocaToolTip(C1SuperTooltip1, btnAgregar, btnEditar, btnEliminar, lblBuscar, btnBuscar, btnSeleccionar, btnPrimero, _
                          btnSiguiente, btnAnterior, btnUltimo, btnImprimir, btnSalir)
        ft.colocaImagenesEnBarra(MenuBarra)
        ft.ajustarAnchoForma(Me, dg, MenuBarra)

    End Sub
    Private Sub AsignaTXT(ByVal nRow As Long)

        If strSQL <> "" Then
            dtTS = ft.MostrarFilaEnTabla(myConn, dsTS, nTablaTS, strSQL, Me.BindingContext, MenuBarra, dg, lRegion, _
                                          jytsistema.sUsuario, nRow, True)
        Else
            If dg.Rows.Count >= 0 Then
                dg.Rows(nRow).Selected = True
            End If
        End If

    End Sub

    Private Sub btnSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeleccionar.Click
        If dtTS.Rows.Count > 0 AndAlso dg.RowCount > 0 Then
            Apuntador = Me.BindingContext(dsTS, nTablaTS).Position
            Seleccion = dg.SelectedRows(0).Cells(0).Value.ToString
            Me.Close()
        End If
    End Sub

    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        If dg.RowCount = dtTS.Rows.Count Then
            Me.BindingContext(dsTS, nTablaTS).Position = 0
            AsignaTXT(Me.BindingContext(dsTS, nTablaTS).Position)
        End If
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        If dg.RowCount = dtTS.Rows.Count Then
            Me.BindingContext(dsTS, nTablaTS).Position -= 1
            AsignaTXT(Me.BindingContext(dsTS, nTablaTS).Position)
        End If
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        If dg.RowCount = dtTS.Rows.Count Then
            Me.BindingContext(dsTS, nTablaTS).Position += 1
            AsignaTXT(Me.BindingContext(dsTS, nTablaTS).Position)
        End If
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        If dg.RowCount = dtTS.Rows.Count Then
            Me.BindingContext(dsTS, nTablaTS).Position = dsTS.Tables(nTablaTS).Rows.Count - 1
            AsignaTXT(Me.BindingContext(dsTS, nTablaTS).Position)
        End If
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click

        'Select Case Modulo
        '    Case "00002" 'Categorias
        '        Dim f As New jsMerRepParametros
        '        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cCategorias, sModulo)
        '        f = Nothing
        '    Case "00003" 'Marcas
        '        Dim f As New jsMerRepParametros
        '        f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cMarcas, sModulo)
        '        f = Nothing
        'End Select



    End Sub

    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        n_Apuntador = -1
        Seleccion = ""
        Me.Dispose()
    End Sub

    Private Sub jsConTablaSimple_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dtTS.Dispose()
        dtTS = Nothing
        myConn.Close()
    End Sub

    Private Sub jsConTablaSimple_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)
        FindField = aCampos(0)
        lblBuscar.Text = aNombres(0)
    End Sub

    Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click
        If bIncModEli Then
            FindField = dtTS.Columns(aCampos(0)).ColumnName
            lblBuscar.Text = aNombres(0)

            If dtTS.Columns("codigo").DataType Is GetType(String) Then _
            BindingSource1.Filter = "codigo" & " like '%%'"

            Dim f As New jsControlArcTablaSimpleMovimientos
            f.Agregar(myConn, dsTS, dtTS, Modulo)
            dsTS = DataSetRequery(dsTS, strSQL, myConn, dtTS.TableName, lblInfo)
            If f.Apuntador >= 0 Then AsignaTXT(f.Apuntador)
            f = Nothing
        End If
    End Sub

    Private Sub btnEditar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditar.Click
        If bIncModEli Then
            Dim f As New jsControlArcTablaSimpleMovimientos
            f.Apuntador = Me.BindingContext(dsTS, dtTS.TableName).Position
            f.Editar(myConn, dsTS, dtTS, Modulo)
            dsTS = DataSetRequery(dsTS, strSQL, myConn, dtTS.TableName, lblInfo)
            AsignaTXT(f.Apuntador)
            f = Nothing
        End If
    End Sub

    Private Sub btnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminar.Click
        If bIncModEli Then
            Apuntador = Me.BindingContext(dsTS, nTablaTS).Position
            Dim nReg As Integer = 0
            Select Case Modulo
                Case "00001" 'almacenes
                    nReg = NumeroDeRegistrosEnTabla(myConn, "jsmertramer", " almacen = '" & dtTS.Rows(Apuntador).Item("codigo") & "' ")
                    If nReg > 0 Then
                        ft.MensajeCritico("(" & nReg & ") ALMACEN NO ELIMINABLE...")
                        Exit Sub
                    End If
                Case "00002" 'categorias
                    nReg = NumeroDeRegistrosEnTabla(myConn, "jsmerctainv", " GRUPO = '" & dtTS.Rows(Apuntador).Item("codigo") & "' ")
                    If nReg > 0 Then
                        ft.MensajeCritico("(" & nReg & ") CATEGORIA NO ELIMINABLE...")
                        Exit Sub
                    End If
                Case "00003" 'marcas
                    nReg = NumeroDeRegistrosEnTabla(myConn, "jsmerctainv", " MARCA = '" & dtTS.Rows(Apuntador).Item("codigo") & "' ")
                    If nReg > 0 Then
                        ft.MensajeCritico("(" & nReg & ") MARCA NO ELIMINABLE...")
                        Exit Sub
                    End If
                Case "00005" 'ZONAS
                    nReg = NumeroDeRegistrosEnTabla(myConn, "jsvenencrut", " CODZON = '" & dtTS.Rows(Apuntador).Item("codigo") & "' ")
                    If nReg > 0 Then
                        ft.MensajeCritico("(" & nReg & ") ZONA NO ELIMINABLE...")
                        Exit Sub
                    End If
                Case "00008" 'ZONAS PROVEEDOR
                    nReg = NumeroDeRegistrosEnTabla(myConn, "jsprocatprot", " ZONA = '" & dtTS.Rows(Apuntador).Item("codigo") & "' ")
                    If nReg > 0 Then
                        ft.MensajeCritico("(" & nReg & ") ZONA NO ELIMINABLE...")
                        Exit Sub
                    End If
                Case "00010" 'BANCOS DE LA PLAZA
                    nReg = NumeroDeRegistrosEnTabla(myConn, "jsventracob", " FORMAPAG = 'CH' AND NOMPAG = '" & dtTS.Rows(Apuntador).Item("codigo") & "' ")
                    If nReg > 0 Then
                        ft.MensajeCritico("(" & nReg & ") BANCO NO ELIMINABLE...")
                        Exit Sub
                    End If
                Case "00035"
                    nReg = ft.DevuelveScalarEntero(myConn, " SELECT COUNT(*) FROM jsmertramer WHERE unidad = '" & dtTS.Rows(Apuntador).Item("CODIGO") & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")
                    If nReg > 0 Then
                        ft.mensajeCritico(" (" & nReg.ToString & ") UNIDAD NO ELIMINABLE... ")
                        Exit Sub
                    End If
            End Select

            EliminaFilaTS()
        End If
    End Sub
    Private Sub EliminaFilaTS()

        Dim aCamposDel() As String = {"codigo", "modulo", "id_emp"}
        Dim aStringsDel() As String = {dtTS.Rows(Apuntador).Item("codigo"), Modulo, jytsistema.WorkID}

        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
            AsignaTXT(EliminarRegistros(myConn, lblInfo, dsTS, nTablaTS, "jsconctatab", strSQL, aCamposDel, aStringsDel, _
                                     Apuntador, True))
        End If
    End Sub
    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        Apuntador = Me.BindingContext(dsTS, nTablaTS).Position
        If dtTS.Rows.Count > 0 AndAlso dg.RowCount > 0 Then
            Seleccion = dg.SelectedRows(0).Cells(0).Value.ToString
            Me.Close()
        End If
    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, dg.CellMouseClick

        Me.BindingContext(dsTS, nTablaTS).Position = e.RowIndex
        n_Apuntador = e.RowIndex
        AsignaTXT(e.RowIndex)
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim f As New frmBuscar
        f.Buscar(dtTS, aCampos, aNombres, aAnchos, n_Apuntador, Me.Text)
        AsignaTXT(f.Apuntador)
        f = Nothing
    End Sub

    Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.ColumnHeaderMouseClick
        FindField = dtTS.Columns(aCampos(e.ColumnIndex)).ColumnName
        lblBuscar.Text = aNombres(e.ColumnIndex)

        txtBuscar.Focus()
    End Sub

    Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        BindingSource1.DataSource = dtTS
        If dtTS.Columns(FindField).DataType Is GetType(String) Then _
            BindingSource1.Filter = FindField & " like '%" & txtBuscar.Text & "%'"
        dg.DataSource = BindingSource1
    End Sub

    
    Private Sub jsControlArcTablaSimple_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        txtBuscar.Focus()
    End Sub

End Class