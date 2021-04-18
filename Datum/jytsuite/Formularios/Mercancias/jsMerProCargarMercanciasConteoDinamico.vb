Imports MySql.Data.MySqlClient
Public Class jsMerProCargarMercanciasConteoDinamico

    Private Const sModulo As String = "Cargar Mercancías en conteo de inventario"
    Private Const nTabla As String = "tblConteoDinamico"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private ConteoNumero As String
    Private tipoUnidad As Integer
    Private numeroCuenta As Integer
    Private CodigoAlmacen As String
    Private nPosicion As Integer

    Private CodigoArticulo As String = ""
    Private NombreArticulo As String = ""
    Private UnidadArticulo As String = ""

    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal NumeroConteo As String, ByVal CodAlmacen As String, _
                      tipUnidad As Integer, numCuenta As Integer)

        '                         -ALM--EST--UBI-   
        'txtCodUbicacion .Text = '000010000100001' 
        '                         012345678901234  
        myConn = MyCon
        Me.Tag = sModulo & IIf(CodAlmacen = "", " de todos los almacenes", " del almacén " & CodAlmacen)
        ConteoNumero = NumeroConteo
        CodigoAlmacen = CodAlmacen
        txtCodUbicacion.Text = CodAlmacen
        tipoUnidad = tipUnidad
        numeroCuenta = numCuenta

        iniciarTXT()

        Me.ShowDialog()

    End Sub
    Private Sub iniciarTXT()


        If txtCodUbicacion.Text.Length >= 15 Then

            strSQL = " select * from jsmerconmerdinamica " _
                                   & " WHERE " _
                                   & " conmer = '" & ConteoNumero & "' AND " _
                                   & " codest = '" & txtCodUbicacion.Text.Substring(5, 5) & "' AND " _
                                   & " codubi = '" & txtCodUbicacion.Text.Substring(10) & "' AND " _
                                   & " CUENTA = " & numeroCuenta & " AND " _
                                   & " id_emp = '" & jytsistema.WorkID & "' "

            dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)

            Dim aCampos() As String = {"CODART.Código.120.I.", _
                                       "NOMART.Nombre ó Descripción.400.I.", _
                                       "UNIDAD.UND.50.C.", _
                                       "CANTIDAD.CANTIDAD.120.D.Numero", _
                                       "SADA..20.I."}

            ft.IniciarTablaPlus(dg, dt, aCampos, , True, , False)
            If dt.Rows.Count > 0 Then nPosicion = 0
            Dim aCol() As String = {"CANTIDAD"}
            ft.EditarColumnasEnDataGridView(dg, aCol)

        End If

        txtBarras.BackColor = Color.LightPink


    End Sub

    Private Sub AsignaUbicatex(ByVal nRow As Long, ByVal Actualiza As Boolean)

        Dim c As Integer = CInt(nRow)
        If Actualiza Then dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)
        If c >= 0 AndAlso dt.Rows.Count > 0 Then
            If c > dt.Rows.Count - 1 Then c = dt.Rows.Count - 1
            Me.BindingContext(ds, nTabla).Position = c
            dg.Refresh()
            dg.CurrentCell = dg(3, c)
        End If

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If dt.Rows.Count > 0 Then
            If ft.Pregunta(" SI CANCELA SE ELIMINARAN TODOS LOS MOVIMIENTOS DE ESTA UBICACION Y CUENTA. ESTA SEGURO ... ", sModulo) = Windows.Forms.DialogResult.Yes Then
                ft.Ejecutar_strSQL(myConn, " DELETE FROM jsmerconmerdinamica " _
                                       & " WHERE " _
                                       & " conmer = '" & ConteoNumero & "' AND " _
                                       & " codest = '" & txtCodUbicacion.Text.Substring(5, 5) & "' AND " _
                                       & " codubi = '" & txtCodUbicacion.Text.Substring(10) & "' AND " _
                                       & " CUENTA = " & numeroCuenta & " AND " _
                                       & " id_emp = '" & jytsistema.WorkID & "' ")
            End If
        End If
        Me.Close()
    End Sub

    Private Sub jsMerProCargarMercanciasConteo_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsMerProCargarMercanciasConteo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub txtBarras_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBarras.Click
        txtBarras.BackColor = Color.PaleGreen
        ft.enfocarTexto(txtBarras)
    End Sub
    Private Sub txtBarras_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBarras.GotFocus
        If txtBarras.BackColor = Color.LightPink Then txtBarras.BackColor = Color.PaleGreen
    End Sub

    Private Sub txtBarras_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBarras.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                If lblUBICACION.Text = "" Then
                    ft.mensajeCritico("Debe indeicar una UBICACION VALIDA...")
                Else
                    CodigoArticulo = getCodigoMercancia(myConn, txtBarras.Text)

                    If Not CodigoArticulo.Equals("") Then

                        NombreArticulo = ft.DevuelveScalarCadena(myConn, " select NOMART from jsmerctainv where barras = '" & txtBarras.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        UnidadArticulo = ft.DevuelveScalarCadena(myConn, " select " & IIf(tipoUnidad = 0, "UNIDAD", "UNIDADDETAL") & " from jsmerctainv where barras = '" & txtBarras.Text & "' and id_emp = '" & jytsistema.WorkID & "' ")
                        IncluirItem(CodigoArticulo, NombreArticulo, UnidadArticulo)

                    End If
                End If
                ft.enfocarTexto(txtBarras)
        End Select
    End Sub

    Private Sub IncluirItem(CodigoArticulo As String, NombreArticulo As String, UnidadArticulo As String)


        If ft.DevuelveScalarEntero(myConn, " select count(*) from jsmerconmerdinamica " _
                                                       & " where " _
                                                       & " conmer = '" & ConteoNumero & "' and " _
                                                       & " codest = '" & txtCodUbicacion.Text.Substring(5, 5) & "' and " _
                                                       & " codubi = '" & txtCodUbicacion.Text.Substring(10) & "' and " _
                                                       & " codart = '" & CodigoArticulo & "' and " _
                                                       & " cuenta = " & numeroCuenta & " and " _
                                                       & " id_emp = '" & jytsistema.WorkID & "' ") > 0 Then
            ft.mensajeCritico("Esta mercancía '" & CodigoArticulo & "' YA se encuentra en ESTA CUENTA y EN ESTE CONTEO...")

        Else
            ft.Ejecutar_strSQL(myConn, " insert into jsmerconmerdinamica SET " _
                                                            & " CONMER = '" & ConteoNumero & "', " _
                                                            & " CODEST = '" & txtCodUbicacion.Text.Substring(5, 5) & "', " _
                                                            & " CODUBI = '" & txtCodUbicacion.Text.Substring(10) & "', " _
                                                            & " CODART = '" & CodigoArticulo & "', " _
                                                            & " NOMART = '" & NombreArticulo & "', " _
                                                            & " UNIDAD = '" & UnidadArticulo & "', " _
                                                            & " CUENTA = " & numeroCuenta & ", " _
                                                            & " CANTIDAD = 0.000, " _
                                                            & " ID_EMP = '" & jytsistema.WorkID & "' ")
        End If

        dt = ft.AbrirDataTable(ds, nTabla, myConn, strSQL)
        Dim row As DataRow = dt.Select(" CODART = '" & CodigoArticulo & "' AND ID_EMP = '" & jytsistema.WorkID & "' ")(0)
        nPosicion = dt.Rows.IndexOf(row)

        If dt.Rows.Count > 0 Then ft.habilitarObjetos(False, True, txtCodUbicacion)

        Me.BindingContext(ds, nTabla).Position = nPosicion

        AsignaUbicatex(nPosicion, False)

    End Sub

    Private Sub txtBarras_LostFocus(sender As Object, e As EventArgs) Handles txtBarras.LostFocus
        txtBarras.BackColor = Color.LightPink
    End Sub

    Private Sub btnProveedor_Click(sender As Object, e As EventArgs) Handles btnMercancia.Click
        If lblUBICACION.Text = "" Then
            ft.mensajeCritico("Debe indicar una UBICACION VALIDA...")
        Else
            Dim f As New jsMerArcListaCostosPreciosNormal
            f.Cargar(myConn, TipoListaPrecios.CostosTodos, "")
            CodigoArticulo = f.Seleccionado
            NombreArticulo = ft.DevuelveScalarCadena(myConn, " select NOMART from jsmerctainv where codart = '" & f.Seleccionado & "' and id_emp = '" & jytsistema.WorkID & "' ")
            UnidadArticulo = ft.DevuelveScalarCadena(myConn, " select " & IIf(tipoUnidad = 0, "UNIDAD", "UNIDADDETAL") & " from jsmerctainv where CODART = '" & f.Seleccionado & "' and id_emp = '" & jytsistema.WorkID & "' ")
            IncluirItem(CodigoArticulo, NombreArticulo, UnidadArticulo)
            f.Dispose()
            f = Nothing
        End If

    End Sub


    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Reprocesar()
        ft.mensajeInformativo(" Proceso culminado ...")
        ProgressBar1.Value = 0
        lblProgreso.Text = ""
        Me.Close()

    End Sub

    Private Sub Reprocesar()

        Dim hCont As Integer
        Dim strFilter As String = ""

        If dt.Rows.Count > 0 Then
            For hCont = 0 To dt.Rows.Count - 1
                With dt.Rows(hCont)

                    refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, (hCont + 1) / dt.Rows.Count * 100, " Mercancía : " & .Item("codart") & " " & .Item("nomart"))

                    Dim incluir As Boolean = True

                    Dim CantidadConeto1 As Double = 0
                    Dim CantidadConeto2 As Double = 0
                    Dim CantidadConeto3 As Double = 0
                    Dim CantidadConeto4 As Double = 0
                    Dim CantidadConeto5 As Double = 0

                    If ft.DevuelveScalarEntero(myConn, " select count(*) from jsmerconmer " _
                                                & " where " _
                                                & " conmer = '" & ConteoNumero & "' and " _
                                                & " codart = '" & .Item("CODART") & "' and " _
                                                & " id_emp = '" & jytsistema.WorkID & "'  ") > 0 Then

                        CantidadConeto1 = ft.DevuelveScalarDoble(myConn, " select cont1 from jsmerconmer where conmer = '" & ConteoNumero & "' and codart = '" & .Item("CODART") & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        CantidadConeto2 = ft.DevuelveScalarDoble(myConn, " select cont2 from jsmerconmer where conmer = '" & ConteoNumero & "' and codart = '" & .Item("CODART") & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        CantidadConeto3 = ft.DevuelveScalarDoble(myConn, " select cont3 from jsmerconmer where conmer = '" & ConteoNumero & "' and codart = '" & .Item("CODART") & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        CantidadConeto4 = ft.DevuelveScalarDoble(myConn, " select cont4 from jsmerconmer where conmer = '" & ConteoNumero & "' and codart = '" & .Item("CODART") & "' and id_emp = '" & jytsistema.WorkID & "'  ")
                        CantidadConeto5 = ft.DevuelveScalarDoble(myConn, " select cont5 from jsmerconmer where conmer = '" & ConteoNumero & "' and codart = '" & .Item("CODART") & "' and id_emp = '" & jytsistema.WorkID & "'  ")

                        incluir = False

                    End If

                    Dim Existencia As Double = ExistenciasEnAlmacenes(myConn, .Item("codart"), CodigoAlmacen)
                    Dim Costo As Double = UltimoCostoAFecha(myConn, .Item("codart"), jytsistema.sFechadeTrabajo, .Item("UNIDAD"))
                    Dim CantidadReal As Double = ft.DevuelveScalarDoble(myConn, " SELECT SUM(cantidad) FROM jsmerconmerdinamica " _
                                                                        & " WHERE " _
                                                                        & " CONMER = '" & ConteoNumero & "' AND  " _
                                                                        & " CODART = '" & .Item("codart") & "' AND  " _
                                                                        & " CUENTA = " & numeroCuenta & " AND  " _
                                                                        & " ID_EMP = '" & jytsistema.WorkID & "' ")

                    InsertEditMERCASRenglonesConteo(myConn, lblInfo, incluir, ConteoNumero, txtCodUbicacion.Text.Substring(5, 5), _
                                                    txtCodUbicacion.Text.Substring(10), .Item("codart"), _
                                                    .Item("nomart"), .Item("UNIDAD"), 0.0, Existencia, 0.0, _
                                                     IIf(numeroCuenta = 0, CantidadReal, CantidadConeto1), 0.0, _
                                                     IIf(numeroCuenta = 1, CantidadReal, CantidadConeto2), 0.0, _
                                                     IIf(numeroCuenta = 2, CantidadReal, CantidadConeto3), 0.0, _
                                                     IIf(numeroCuenta = 3, CantidadReal, CantidadConeto4), 0.0, _
                                                     IIf(numeroCuenta = 4, CantidadReal, CantidadConeto5), Costo, _
                                                    Math.Round(Existencia * Costo, 2), "1")

                    ft.Ejecutar_strSQL(myConn, " UPDATE jsmerconmer " _
                                       & " SET CONTEO = IF( CONT5 <> 0, CONT5, IF( CONT4 <> 0, CONT4, IF( CONT3 <> 0, CONT3, IF( CONT2 <> 0, CONT2, CONT1  ) ) ) ), " _
                                       & " COSTO_TOT = COSTOU*IF( CONT5 <> 0, CONT5, IF( CONT4 <> 0, CONT4, IF( CONT3 <> 0, CONT3, IF( CONT2 <> 0, CONT2, CONT1  ) ) ) ) " _
                                       & " WHERE " _
                                       & " CONMER = '" & ConteoNumero & "' AND  " _
                                       & " CODART = '" & .Item("CODART") & "' AND " _
                                       & " ID_EMP = '" & jytsistema.WorkID & "' ")

                End With
            Next
        Else
            ft.mensajeCritico("No existen movimientos para procesar...")
        End If

    End Sub

    Private Sub txtCodUbicacion_GotFocus(sender As Object, e As EventArgs) Handles txtCodUbicacion.GotFocus
        Me.KeyPreview = False
    End Sub


    Private Sub txtCodUbicacion_TextChanged(sender As Object, e As EventArgs) Handles txtCodUbicacion.TextChanged
        If txtCodUbicacion.Text.Length = 15 Then
            If ft.DevuelveScalarEntero(myConn, " SELECT COUNT(*) " _
                                       & " FROM jsmercatubi a " _
                                       & " LEFT JOIN jsmercatest b ON (a.codest = b.codest AND a.id_emp = b.id_emp) " _
                                       & " WHERE " _
                                       & " b.codalm = '" & txtCodUbicacion.Text.Substring(0, 5) & "' AND " _
                                       & " b.codest = '" & txtCodUbicacion.Text.Substring(5, 5) & "' AND " _
                                       & " a.codubi = '" & txtCodUbicacion.Text.Substring(10) & "' AND " _
                                       & " a.id_emp = '" & jytsistema.WorkID & "'") > 0 Then

                lblUBICACION.Text = ft.DevuelveScalarCadena(myConn, " SELECT CONCAT(LAD,'-',FIL,'-', COL) " _
                                       & " FROM jsmercatubi a " _
                                       & " LEFT JOIN jsmercatest b ON (a.codest = b.codest AND a.id_emp = b.id_emp) " _
                                       & " WHERE " _
                                       & " b.codalm = '" & txtCodUbicacion.Text.Substring(0, 5) & "' AND " _
                                       & " b.codest = '" & txtCodUbicacion.Text.Substring(5, 5) & "' AND " _
                                       & " a.codubi = '" & txtCodUbicacion.Text.Substring(10) & "' AND " _
                                       & " a.id_emp = '" & jytsistema.WorkID & "'")

                iniciarTXT()

            End If
        End If
    End Sub

    Private Sub dg_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg.CellDoubleClick

    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) _
       Handles dg.RowHeaderMouseClick, dg.CellMouseClick

        If e.RowIndex >= 0 Then
            Me.BindingContext(ds, nTabla).Position = e.RowIndex
            nPosicion = e.RowIndex
            AsignaUbicatex(nPosicion, False)
        End If

    End Sub
  
    Private Sub dg_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub

    Private Sub btnEliminaUbica_Click(sender As Object, e As EventArgs) Handles btnEliminaUbica.Click

        If dt.Rows.Count > 0 Then
            Dim aCamposDel() As String = {"conmer", "codest", "codubi", "codart", "cuenta", "id_emp"}
            Dim aStringsDel() As String = {ConteoNumero, _
                                           txtCodUbicacion.Text.Substring(5, 5), _
                                           txtCodUbicacion.Text.Substring(10), _
                                           dt.Rows(nPosicion).Item("codart"), _
                                           numeroCuenta, _
                                           jytsistema.WorkID}

            If ft.PreguntaEliminarRegistro() = Windows.Forms.DialogResult.Yes Then
                AsignaUbicatex(EliminarRegistros(myConn, lblInfo, ds, nTabla, "jsmerconmerdinamica", strSQL, aCamposDel, aStringsDel, _
                                                  Me.BindingContext(ds, nTabla).Position, True), True)
                If dt.Rows.Count = 0 Then ft.habilitarObjetos(True, True, txtCodUbicacion)
            End If
        End If

    End Sub

    Private Sub dg_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dg.CellValidating
        Dim headerText As String = _
            dg.Columns(e.ColumnIndex).HeaderText

        If Not headerText.Equals("CANTIDAD") Then Return

        If (String.IsNullOrEmpty(e.FormattedValue.ToString())) Then
            ft.mensajeAdvertencia("Debe indicar dígito(s) válido...")
            e.Cancel = True
        End If

        If Not ft.isNumeric(e.FormattedValue.ToString()) Then
            ft.mensajeAdvertencia("Debe indicar un número válido...")
            e.Cancel = True
        End If

    End Sub

    Private Sub dg_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dg.CellEndEdit
        dg.Rows(e.RowIndex).ErrorText = String.Empty
    End Sub

    Private Sub dg_CellValidated(sender As Object, e As DataGridViewCellEventArgs) Handles dg.CellValidated
        Select Case dg.CurrentCell.ColumnIndex
            Case 3
                CodigoArticulo = dg.Rows(dg.CurrentCell.RowIndex).Cells(0).Value
                ft.Ejecutar_strSQL(myConn, " UPDATE jsmerconmerdinamica SET " _
                                                            & " CANTIDAD = " & dg.CurrentCell.Value & " " _
                                                            & " WHERE " _
                                                            & " CONMER = '" & ConteoNumero & "' AND " _
                                                            & " CODEST = '" & txtCodUbicacion.Text.Substring(5, 5) & "' AND " _
                                                            & " CODUBI = '" & txtCodUbicacion.Text.Substring(10) & "' AND " _
                                                            & " CODART = '" & CodigoArticulo & "' AND " _
                                                            & " CUENTA = " & numeroCuenta & " AND " _
                                                            & " ID_EMP = '" & jytsistema.WorkID & "' ")
        End Select
    End Sub

    
End Class