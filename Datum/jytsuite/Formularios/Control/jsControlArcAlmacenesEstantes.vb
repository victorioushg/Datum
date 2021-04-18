Imports MySql.Data.MySqlClient
Public Class jsControlArcAlmacenesEstantes

    Private sModulo As String = "Movimiento Estantes - "

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private dtEst As DataTable
    Private nTabla As String = "tblUbicaciones"
    Private strSQL As String = ""
    Private ft As New Transportables

    Private CodigoAlmacen As String
    Private i_modo As Integer
    Private nPosicion As Integer
    Private nPosicionUbicatex As Integer = -1
    Private n_Apuntador As Long
    Private pClick As Integer = 0
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Agregar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                       CodAlmacen As String)
        i_modo = movimiento.iAgregar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoAlmacen = CodAlmacen

        IniciarTXT()

        Me.ShowDialog()
    End Sub
    Private Sub IniciarTXT()

        txtCodigo.Text = ft.autoCodigo(MyConn, "codest", "jsmercatest", "id_emp", jytsistema.WorkID, 5)
        txtNombre.Text = ""
        txtUbicacionAlmacen.Text = ""

        ft.RellenaCombo(aTipoEstante, cmbTipoEstante)

    End Sub

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable, _
                      CodAlmacen As String)
        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt
        CodigoAlmacen = CodAlmacen

        AsignarTXT(Apuntador)
        Me.ShowDialog()
    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        If dtLocal.Rows.Count > 0 Then
            With dtLocal.Rows(nPosicion)
                txtCodigo.Text = ft.muestraCampoTexto(.Item("codest"))
                txtNombre.Text = ft.muestraCampoTexto(.Item("descrip"))
                txtUbicacionAlmacen.Text = ft.muestraCampoTexto(.Item("UBICA_ALM"))

                ft.RellenaCombo(aTipoEstante, cmbTipoEstante, .Item("TIPO"))

                AbrirUbicatex(.Item("CODEST"))

            End With
        End If
    End Sub
    Private Sub AbrirUbicatex(ByVal CodigoEstante As String)

        strSQL = " select * from jsmercatubi " _
                                   & " where " _
                                   & " codest = '" & CodigoEstante & "' and " _
                                   & " id_emp = '" & jytsistema.WorkID & "'  order by codubi "

        dtEst = ft.AbrirDataTable(dsLocal, nTabla, MyConn, strSQL)

        Dim aCampos() As String = {"CODUBI.Código.70.C.", _
                                   "lad.SUB.50.C.", _
                                   "fil.FIL.50.C.", _
                                   "col.COL.50.C.", _
                                   "SADA..20.I."}

        ft.IniciarTablaPlus(dgUbica, dtEst, aCampos, , True, , False)
        If dtEst.Rows.Count > 0 Then nPosicionUbicatex = 0

        dgUbica.ReadOnly = False
        dgUbica.Columns("CODUBI").ReadOnly = True

    End Sub
    Private Sub jsControlDivisionesMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jiConTarjetasovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sModulo += " Almacen : " & CodigoAlmacen
        Me.Text = If(i_modo = MovAud.iIncluir, " Agregar ", " Editar ") & sModulo
        InsertarAuditoria(MyConn, i_modo, sModulo, txtCodigo.Text)
        ft.habilitarObjetos(False, True, txtCodigo, txtUbicacionAlmacen)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If txtNombre.Text.Trim() = "" Then
            ft.mensajeCritico("Debe indicar un nombre ó descripción válido...")
            txtNombre.Focus()
            Return False
        Else
        End If

        If pClick = 0 Then
            If Not dtEst Is Nothing Then
                If dtEst.Rows.Count = 0 Then
                    ft.mensajeCritico("Debe indicar al menos una UBICACION. Verifique por favor...")
                    Return False
                End If
            Else
                Return False
            End If
        End If

        If txtUbicacionAlmacen.Text = "" Then
            ft.mensajeCritico("Debe indicar Ubicación DENTRO del almacén...")
            Return False
        End If



        Return True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        pClick = 0
        If Validado() Then
            Dim Insertar As Boolean = False
            If i_modo = movimiento.iAgregar Then
                Insertar = True
                Apuntador = dtLocal.Rows.Count
            End If
            InsertEditMERCASEstante(MyConn, lblInfo, Insertar, txtCodigo.Text, txtNombre.Text, cmbTipoEstante.SelectedIndex, _
                                     CodigoAlmacen, txtUbicacionAlmacen.Text)
            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub


    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub btnAgregaUbica_Click(sender As Object, e As EventArgs) Handles btnAgregaUbica.Click
        pClick = 1
        If i_modo = movimiento.iAgregar Then
            If Validado() Then
                InsertEditMERCASEstante(MyConn, lblInfo, True, txtCodigo.Text, txtNombre.Text, cmbTipoEstante.SelectedIndex, _
                                     CodigoAlmacen, txtUbicacionAlmacen.Text)
                AbrirUbicatex(txtCodigo.Text)
                i_modo = movimiento.iEditar
            Else
                Return
            End If
        End If

        nPosicionUbicatex += 1

        Dim codUBI As String = ft.autoCodigo(MyConn, "CODUBI", "jsmercatubi", "codest.id_emp", _
                                             txtCodigo.Text & "." & jytsistema.WorkID, 5)
        ft.Ejecutar_strSQL(MyConn, " insert into jsmercatubi set codubi = '" & codUBI & "', LAD = '', FIL = '', COL = '', CODEST = '" & txtCodigo.Text & "', ID_EMP = '" & jytsistema.WorkID & "'  ")
        AsignaUbicatex(nPosicionUbicatex, True)

    End Sub

    Private Sub btnEditaUbica_Click(sender As Object, e As EventArgs) Handles btnEditaUbica.Click

    End Sub

    Private Sub btnEliminaUbica_Click(sender As Object, e As EventArgs) Handles btnEliminaUbica.Click
        nPosicionUbicatex = Me.BindingContext(dsLocal, nTabla).Position

        If dtEst.Rows.Count > 0 Then
            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                Dim aCamDel() As String = {"codubi", "codest", "id_emp"}
                Dim aStrDel() As String = {dtEst.Rows(nPosicionUbicatex).Item("codubi"), txtCodigo.Text, jytsistema.WorkID}

                AsignaUbicatex(EliminarRegistros(MyConn, lblInfo, dsLocal, nTabla, "jsmercatubi", _
                                           strSQL, aCamDel, aStrDel, nPosicionUbicatex), True)

            End If
        End If

    End Sub
    Private Sub AsignaUbicatex(ByVal nRow As Long, ByVal Actualiza As Boolean)

        Dim c As Integer = CInt(nRow)
        If Actualiza Then dtEst = ft.AbrirDataTable(dsLocal, nTabla, MyConn, strSQL)
        If c >= 0 AndAlso dtEst.Rows.Count > 0 Then
            If c > dtEst.Rows.Count - 1 Then c = dtEst.Rows.Count - 1
            Me.BindingContext(dsLocal, nTabla).Position = c
            dgUbica.Refresh()
            dgUbica.CurrentCell = dgUbica(0, c)
        End If

    End Sub
    Private Sub btnDescripcion_Click(sender As Object, e As EventArgs) Handles btnDescripcion.Click
        Dim f As New jsControlArcTablaSimple
        f.Cargar("Ubicación en Almacén", FormatoTablaSimple(Modulo.iMER_UbicacionEstantes), True, TipoCargaFormulario.iShowDialog)
        txtUbicacionAlmacen.Text = f.Seleccion
        f.Close()
        f = Nothing
    End Sub

    Private Sub txtUbicacionAlmacen_TextChanged(sender As Object, e As EventArgs) Handles txtUbicacionAlmacen.TextChanged
        lblUbica.Text = ft.DevuelveScalarCadena(MyConn, " select descrip from jsconctatab " _
                                                & " where " _
                                                & " modulo = '" & FormatoTablaSimple(Modulo.iMER_UbicacionEstantes) & "' and " _
                                                & " codigo = '" & txtUbicacionAlmacen.Text & "' and " _
                                                & " id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Private Sub dgUbica_CellValidated(sender As Object, e As DataGridViewCellEventArgs) Handles dgUbica.CellValidated
        Select Case dgUbica.CurrentCell.ColumnIndex
            Case 1, 2, 3
                Dim aAA() As String = {"LAD", "FIL", "COL"}
                Dim sA As String = aAA(dgUbica.CurrentCell.ColumnIndex - 1)
                ft.Ejecutar_strSQL(MyConn, " UPDATE jsmercatubi SET " & sA & " = '" & dgUbica.CurrentCell.Value & "' " _
                                            & " where " _
                                            & " CODUBI = '" & CStr(dgUbica.CurrentRow.Cells(0).Value) & "' AND " _
                                            & " CODEST = '" & txtCodigo.Text & "' AND " _
                                            & " ID_EMP = '" & jytsistema.WorkID & "' ")

        End Select
    End Sub

    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgUbica.RowHeaderMouseClick, _
       dgUbica.CellMouseClick
        Me.BindingContext(dsLocal, nTabla).Position = e.RowIndex
        nPosicionUbicatex = e.RowIndex
    End Sub

    
    Private Sub btnImprimirUbica_Click(sender As Object, e As EventArgs) Handles btnImprimirUbica.Click
        If dtEst.Rows.Count > 0 Then
            Dim f As New jsMerRepParametros
            f.Cargar(TipoCargaFormulario.iShowDialog, ReporteMercancias.cCodigoBarraUbicacion, _
                     "CODIGO BARRAS UBICACION", dtEst.Rows(nPosicionUbicatex).Item("codubi") & txtCodigo.Text)
            f.Dispose()
            f = Nothing
        End If
    End Sub

   
End Class