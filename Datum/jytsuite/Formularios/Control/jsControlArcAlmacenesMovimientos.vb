Imports MySql.Data.MySqlClient
Public Class jsControlArcAlmacenesMovimientos

    Private Const sModulo As String = "Movimiento Almacenes"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private dtEst As DataTable
    Private nTabla As String = "tblEstantes"
    Private ft As New Transportables

    Private strSQLEstantes As String = ""
    Private i_modo As Integer
    Private nPosicion As Integer
    Private nPosicionEstante As Integer
    Private n_Apuntador As Long
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable)

        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        IniciarTXT()
        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()

        txtCodigo.Text = ft.autoCodigo(MyConn, "codalm", "jsmercatalm", "id_emp", jytsistema.WorkID, 5)
        txtNombre.Text = ""
        txtReponsable.Text = ""
        ft.RellenaCombo(aTipoAlmacen, cmbTipoAlmacen)

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        AsignarTXT(Apuntador)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        If dtLocal.Rows.Count > 0 Then
            With dtLocal.Rows(nPosicion)

                txtCodigo.Text = ft.muestraCampoTexto(.Item("codalm"))
                txtNombre.Text = ft.muestraCampoTexto(.Item("desalm"))
                txtReponsable.Text = ft.muestraCampoTexto(.Item("RESPONSABLE"))
                ft.RellenaCombo(aTipoAlmacen, cmbTipoAlmacen, .Item("tipoalm"))

                AbrirEstantes(.Item("codalm"))

            End With
        End If
    End Sub
    Private Sub AbrirEstantes(ByVal CodigoAlmacen As String)

        strSQLEstantes = " select * from jsmercatest " _
                                   & " where " _
                                   & " codalm = '" & CodigoAlmacen & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "'  order by codest "

        dtEst = ft.AbrirDataTable(dsLocal, nTabla, MyConn, strsqlEstantes)

        Dim aCampos() As String = {"codest.Estante.70.I.", _
                                   "descrip.Descripción.170.I.", _
                                   "tipo.Tipo.90.I.", _
                                   "UBICA_ALM.Ubic Almacén.90.I."}

        ft.IniciarTablaPlus(dg, dtEst, aCampos, , , , False)
        If dtEst.Rows.Count > 0 Then nPosicionEstante = 0
        AsignaEstantes(nPosicionEstante, False)

    End Sub
    Private Sub AsignaEstantes(ByVal nRow As Long, ByVal Actualiza As Boolean)

        Dim c As Integer = CInt(nRow)
        If Actualiza Then dtEst = ft.AbrirDataTable(dsLocal, nTabla, MyConn, strSQLEstantes)
        If c >= 0 AndAlso dtEst.Rows.Count > 0 Then
            If c > dtEst.Rows.Count - 1 Then c = dtEst.Rows.Count - 1
            Me.BindingContext(dsLocal, nTabla).Position = c
            dg.Refresh()
            dg.CurrentCell = dg(0, c)
        End If

    End Sub
    Private Sub jsControlDivisionesMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing

    End Sub

    Private Sub jiConTarjetasovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
        ft.habilitarObjetos(False, True, txtCodigo)
        ft.visualizarObjetos(False, pb)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el código de división ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Function Validado() As Boolean

        Validado = False

        If txtNombre.Text.Trim() = "" Then
            ft.mensajeAdvertencia("Debe indicar un nombre o descripción válido...")
            txtNombre.Focus()
            Exit Function
        Else
            If NumeroDeRegistrosEnTabla(MyConn, "jsmercatalm", " desalm = '" & txtNombre.Text.Trim() & "' ") > 0 AndAlso _
               i_modo = movimiento.iAgregar Then
                ft.MensajeCritico("DESCRIPCION EXISTENTE. Indique descripción válida...")
                ft.enfocarTexto(txtNombre)
                Exit Function
            End If

        End If

        Validado = True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dtLocal.Rows.Count
            End If
            InsertEditMERCASAlmacen(MyConn, lblInfo, Insertar, txtCodigo.Text, txtNombre.Text, txtReponsable.Text, _
                                    cmbTipoAlmacen.SelectedIndex)
            AlmacenesPorItem(txtCodigo.Text)
            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub AlmacenesPorItem(ByVal CodigoAlmacen As String)

        ft.visualizarObjetos(True, pb)
        Dim nTableItems As String = "tblItems"
        Dim dtItems As DataTable

        dtItems = ft.AbrirDataTable(dsLocal, nTableItems, MyConn, " select * from jsmerctainv " _
                                    & " where " _
                                    & " id_emp = '" & jytsistema.WorkID & "' order by codart ")


        Dim ExisteAlmacen As String = ""
        For bCont As Integer = 0 To dtItems.Rows.Count - 1
            With dtItems.Rows(bCont)
                If ft.DevuelveScalarEntero(MyConn, " select count(*) from jsmerextalm where " _
                                            & " codart = '" & .Item("codart") & "' and " _
                                            & " almacen = '" & CodigoAlmacen & "' and " _
                                            & " id_emp = '" & jytsistema.WorkID & "'  ") = 0 Then
                        ft.Ejecutar_strSQL(MyConn, " INSERT INTO jsmerextalm SET CODART = '" & .Item("CODART") & "', ALMACEN = '" & CodigoAlmacen & "', EXISTENCIA = 0.00,  UBICACION = '', ID_EMP = '" & jytsistema.WorkID & "'  ")
                End If
            End With
            refrescaBarraprogresoEtiqueta(pb, Nothing, Convert.ToInt32(bCont / dtItems.Rows.Count * 100), "")
        Next

        dtItems.Dispose()
        dtItems = Nothing

    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombre.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el nombre almacén ...", Transportables.TipoMensaje.iInfo)
    End Sub

    Private Sub btnAgregaEstante_Click(sender As Object, e As EventArgs) Handles btnAgregaEstante.Click
        Dim f As New jsControlArcAlmacenesEstantes
        f.Agregar(MyConn, dsLocal, dtEst, txtCodigo.Text)
        AsignaEstantes(f.Apuntador, True)
        If f.Apuntador > 0 Then nPosicionEstante = f.Apuntador
    End Sub

    Private Sub btnEditaEstante_Click(sender As Object, e As EventArgs) Handles btnEditaEstante.Click

        Dim f As New jsControlArcAlmacenesEstantes
        f.Apuntador = Me.BindingContext(dsLocal, nTabla).Position
        f.Editar(MyConn, dsLocal, dtEst, txtCodigo.Text)
        AsignaEstantes(f.Apuntador, True)
        f = Nothing

    End Sub

    Private Sub btnEliminaEstante_Click(sender As Object, e As EventArgs) Handles btnEliminaEstante.Click

        nPosicionEstante = Me.BindingContext(dsLocal, nTabla).Position

        If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
            Dim aCamDel() As String = {"codest", "codalm", "id_emp"}
            Dim aStrDel() As String = {dtEst.Rows(nPosicionEstante).Item("codest"), txtCodigo.Text, jytsistema.WorkID}

            AsignaEstantes(EliminarRegistros(MyConn, lblInfo, dsLocal, nTabla, "jsmercatest", _
                                       strSQLEstantes, aCamDel, aStrDel, nPosicionEstante), True)

        End If
    End Sub

    Private Sub dg_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dg.CellFormatting
        Select Case e.ColumnIndex
            Case 2 '
                If Not IsDBNull(e.Value) Then
                    e.Value = aTipoEstante(e.Value)
                End If

        End Select
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.RowHeaderMouseClick, _
       dg.CellMouseClick
        Me.BindingContext(dsLocal, nTabla).Position = e.RowIndex
        nPosicionEstante = e.RowIndex
    End Sub

    Private Sub dg_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub
End Class