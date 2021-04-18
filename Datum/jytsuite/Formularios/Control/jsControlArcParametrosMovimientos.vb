Imports MySql.Data.MySqlClient
Public Class jsControlArcParametrosMovimientos

    Private Const sModulo As String = "Movimiento parámetros "
    Private Const nTabla As String = "parametros"

    Private MyConn As New MySqlConnection
    Private dsLocal As DataSet
    Private dtLocal As DataTable
    Private ft As New Transportables

    Private strSQL As String = ""
    Private nTablaTabla As String = "tblTabilta"
    Private dtTabla As New DataTable
    Private nPosicionTabla As Integer

    Private i_modo As Integer
    Private aGestion() As String = {"Contabilidad", "Bancos y Cajas", "Recursos Humanos", "Compras y cuentas por pagar",
                                    "Ventas y cuentas por cobrar", "Puntos de Venta", "Mercancías", "Producción", "Medición Gerencial", "Control de Gestiones"}
    Private aTipo() As String = {"No/Si", "Doble", "Cadena", "Dia Semana", "Entero", "Cantidad o Peso",
                                 "Fecha", "Formato de Papel", "Modo de Impresora", "Tipo de Impresora Fiscal", "Numeración Contable",
                                 "Vector", "Tabla", "Moneda"}
    Private nPosicion As Integer
    Private nModulo As Integer
    Private n_Apuntador As Long
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property

    Public Sub Editar(ByVal MyCon As MySqlConnection, ByVal ds As DataSet, ByVal dt As DataTable)

        i_modo = movimiento.iEditar
        MyConn = MyCon
        dsLocal = ds
        dtLocal = dt

        AsignarTXT(Apuntador)
        ft.habilitarObjetos(False, True, cmbGestion, txtCodigo, txtDescripcion, cmbTipo)
        Me.ShowDialog()

    End Sub
    Private Sub AsignarTXT(ByVal nPosicion As Integer)
        With dtLocal

            Dim aNoSi() As String = {"No", "Si"}
            Dim aDia() As String = {"Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"}
            Dim aPapel() As String = {"Media Carta", "3/4 Carta", "Carta"}
            Dim aModo() As String = {"Draft", "Gráfico"}
            Dim aNum() As String = {"Unica", "Diaria", "Mensual"}
            Dim aTab() As String = {"Tabla"}

            txtCodigo.Text = .Rows(nPosicion).Item("codigo")
            txtDescripcion.Text = .Rows(nPosicion).Item("descripcion")
            nModulo = .Rows(nPosicion).Item("modulo")
            ft.RellenaCombo(aTipo, cmbTipo, CInt(.Rows(nPosicion).Item("tipo")))
            ft.RellenaCombo(aGestion, cmbGestion, .Rows(nPosicion).Item("gestion") - 1)
            ft.visualizarObjetos(False, MenuTabla, dgTabla)

            Select Case CInt(.Rows(nPosicion).Item("tipo"))
                Case TipoParametro.iNoSi
                    ft.RellenaCombo(aNoSi, cmbValor, .Rows(nPosicion).Item("valor"))
                    VerTXT(False)
                Case TipoParametro.iDiaSemana
                    ft.RellenaCombo(aDia, cmbValor, .Rows(nPosicion).Item("valor"))
                    VerTXT(False)
                Case TipoParametro.iFormatoPapel
                    ft.RellenaCombo(aPapel, cmbValor, .Rows(nPosicion).Item("valor"))
                    VerTXT(False)
                Case TipoParametro.iModoImpresora
                    ft.RellenaCombo(aModo, cmbValor, .Rows(nPosicion).Item("valor"))
                    VerTXT(False)
                Case TipoParametro.iTipoImpresoraFiscal
                    ft.RellenaCombo(aFiscal, cmbValor, .Rows(nPosicion).Item("valor"))
                    VerTXT(False)
                Case TipoParametro.iNumeracionContable
                    ft.RellenaCombo(aNum, cmbValor, .Rows(nPosicion).Item("valor"))
                    VerTXT(False)
                Case TipoParametro.iMoneda
                    Dim nTableMoneda As String = "ntableMoneda"
                    Dim dtMoneda As New DataTable
                    Dim strSQLMoneda = "select concat(a.UnidadMonetaria, ' | ', a.simbolo) PaisMoneda,  a.id, " +
                        " a.unidadmonetaria, a.simbolo, a.codigoiso, GROUP_CONCAT(a.pais SEPARATOR '| ') paises " +
                        " FROM jsconcatmon a GROUP BY a.unidadmonetaria "
                    dsLocal = DataSetRequery(dsLocal, strSQLMoneda, MyConn, nTableMoneda, lblInfo)
                    dtMoneda = dsLocal.Tables(nTableMoneda)
                    Dim val As Integer = .Rows(nPosicion).Item("valor")
                    ft.RellenaComboConDatatable(cmbValor, dtMoneda, "PaisMoneda", "id", val)
                    cmbValor.SelectedValue = val
                    VerTXT(False)
                Case TipoParametro.iTabla
                    ft.RellenaCombo(aTab, cmbValor)
                    ft.visualizarObjetos(True, MenuTabla, dgTabla)
                    ft.habilitarObjetos(False, True, txtValor)
                    IniciarGrilla()
                Case Else

                    txtValor.Text = .Rows(nPosicion).Item("valor")
                    VerTXT(True)
            End Select


        End With
    End Sub
    Private Sub IniciarGrilla()
        Select Case txtCodigo.Text
            Case "VENCOTPA11"

                strSQL = " SELECT * FROM jsconctacom WHERE ORIGEN = 'COT' AND ID_EMP = '" & jytsistema.WorkID & "' "
                dtTabla = ft.AbrirDataTable(dsLocal, nTablaTabla, MyConn, strSQL)
                Dim aCam() As String = {"CODIGO.Código.50.C.",
                                        "COMENTARIO.Comentario presupuesto.270.I."}

                ft.IniciarTablaPlus(dgTabla, dtTabla, aCam, , True)
                Dim aEDI() As String = {"COMENTARIO"}
                ft.EditarColumnasEnDataGridView(dgTabla, aEDI)

            Case "VENPARAM38"

                strSQL = " select * from jsconnumcontrol where id_emp = '" & jytsistema.WorkID & "' "
                dsLocal = DataSetRequery(dsLocal, strSQL, MyConn, nTablaTabla, lblInfo)
                dtTabla = dsLocal.Tables(nTablaTabla)


                Dim aCam() As String = {"maquina", "numerocontrol"}
                Dim aNom() As String = {"Estación de Trabajo", "Número de Control"}
                Dim aAnc() As Integer = {170, 100}
                Dim aAli() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro}
                Dim aFor() As String = {"", ""}

                IniciarTabla(dgTabla, dtTabla, aCam, aNom, aAnc, aAli, aFor, True, True, 9, True)
                If dtTabla.Rows.Count > 0 Then nPosicionTabla = 0

                dgTabla.ReadOnly = False
                dgTabla.Columns("maquina").ReadOnly = True
            Case "POSPARAM38"

                strSQL = " select * from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iFrasesCintillo) & "' AND id_emp = '" & jytsistema.WorkID & "' order by codigo "
                dsLocal = DataSetRequery(dsLocal, strSQL, MyConn, nTablaTabla, lblInfo)
                dtTabla = dsLocal.Tables(nTablaTabla)

                Dim aCam() As String = {"CODIGO", "DESCRIP"}
                Dim aNom() As String = {"Código", "Frase para el cintillo PVE"}
                Dim aAnc() As Integer = {50, 270}
                Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
                Dim aFor() As String = {"", ""}

                IniciarTabla(dgTabla, dtTabla, aCam, aNom, aAnc, aAli, aFor, True, True, 9, True)
                If dtTabla.Rows.Count > 0 Then nPosicionTabla = 0

                dgTabla.ReadOnly = False
                dgTabla.Columns("codigo").ReadOnly = True

            Case "POSPARAM46"

                strSQL = " select * from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iDenominacionesMonetarias) & "' AND id_emp = '" & jytsistema.WorkID & "' order by codigo "
                dsLocal = DataSetRequery(dsLocal, strSQL, MyConn, nTablaTabla, lblInfo)
                dtTabla = dsLocal.Tables(nTablaTabla)

                Dim aCam() As String = {"CODIGO", "DESCRIP"}
                Dim aNom() As String = {"Código", "DENOMINACION"}
                Dim aAnc() As Integer = {50, 270}
                Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro}
                Dim aFor() As String = {"", sFormatoEntero}

                IniciarTabla(dgTabla, dtTabla, aCam, aNom, aAnc, aAli, aFor, True, True, 9, True)
                If dtTabla.Rows.Count > 0 Then nPosicionTabla = 0

                dgTabla.ReadOnly = False
                dgTabla.Columns("codigo").ReadOnly = True

        End Select
    End Sub

    Private Sub VerTXT(ByVal Ver As Boolean)
        txtValor.Visible = Ver
        cmbValor.Visible = Not Ver
    End Sub
    Private Sub jsControlParametrosMovimientos_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dtLocal = Nothing
        dsLocal = Nothing

    End Sub

    Private Sub jsControlParametrosMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtCodigo.Text)
    End Sub

    Private Sub txtCodigo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.GotFocus
        ft.mensajeEtiqueta(lblInfo, "Indique el código del parámetro ...", Transportables.tipoMensaje.iInfo)
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        If Trim(txtDescripcion.Text) = "" Then
            ft.mensajeAdvertencia("Debe indicar una descripción válida...")
            txtDescripcion.Focus()
            Exit Function
        End If


        If txtValor.Visible Then
            Select Case cmbTipo.SelectedIndex
                Case TipoParametro.iCadena, TipoParametro.iTabla
                Case Else
                    If txtValor.Text = "" Then
                        ft.mensajeAdvertencia("Debe indicar un valor al parámetro ...")
                        ft.enfocarTexto(txtValor)
                        Exit Function
                    End If
            End Select
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
            InsertEditCONTROLParametro(MyConn, lblInfo, Insertar, cmbGestion.SelectedIndex + 1, nModulo, txtCodigo.Text, txtDescripcion.Text, cmbTipo.SelectedIndex,
                 IIf(txtValor.Visible, txtValor.Text, IIf(cmbTipo.Text = "Moneda", cmbValor.SelectedValue, cmbValor.SelectedIndex)))

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtCodigo.Text)
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub

    Private Sub txt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCodigo.Click,
        txtValor.Click
        Dim txt As TextBox = sender
        ft.enfocarTexto(txt)
    End Sub

    Private Sub btnAgregaDescuento_Click(sender As System.Object, e As System.EventArgs) Handles btnAgregaDescuento.Click
        Select Case txtCodigo.Text
            Case "VENCOTPA11"

                ft.Ejecutar_strSQL(MyConn, " replace into jsconctacom " _
                    & " set codigo = '" & ft.autoCodigo(MyConn, "codigo", "jsconctacom", "origen.id_emp", "COT." + jytsistema.WorkID, 3, True) & "', " _
                    & " COMENTARIO = '', " _
                    & " ORIGEN = 'COT', " _
                    & " id_emp = '" & jytsistema.WorkID & "'")

                IniciarGrilla()
            Case "VENPARAM38"
                ft.Ejecutar_strSQL(MyConn, " replace into jsconnumcontrol " _
                    & " set numerocontrol = '00-000001', " _
                    & " maquina = '" & SystemInformation.ComputerName & "', " _
                    & " id_emp = '" & jytsistema.WorkID & "'")

                IniciarGrilla()

            Case "POSPARAM38"
                ft.Ejecutar_strSQL(MyConn, " replace into jsconctatab " _
                    & " set codigo = '" & ft.autoCodigo(MyConn, "codigo", "jsconctatab", "modulo.id_emp", FormatoTablaSimple(Modulo.iFrasesCintillo) + "." + jytsistema.WorkID, 5, True) & "', " _
                    & " descrip = '', " _
                    & " modulo = '" & FormatoTablaSimple(Modulo.iFrasesCintillo) & "', " _
                    & " id_emp = '" & jytsistema.WorkID & "'")

                IniciarGrilla()
            Case "POSPARAM46"
                ft.Ejecutar_strSQL(MyConn, " replace into jsconctatab " _
                    & " set codigo = '" & ft.autoCodigo(MyConn, "codigo", "jsconctatab", "modulo.id_emp", FormatoTablaSimple(Modulo.iDenominacionesMonetarias) + "." + jytsistema.WorkID, 5, True) & "', " _
                    & " descrip = '', " _
                    & " modulo = '" & FormatoTablaSimple(Modulo.iDenominacionesMonetarias) & "', " _
                    & " id_emp = '" & jytsistema.WorkID & "'")

                IniciarGrilla()
        End Select

    End Sub

    Private Sub btnEliminaDescuento_Click(sender As System.Object, e As System.EventArgs) Handles btnEliminaDescuento.Click
        Select Case txtCodigo.Text
            Case "VENPARAM38"
                If dgTabla.RowCount > 0 Then
                    If dgTabla.SelectedCells(0).RowIndex >= 0 Then
                        Dim nRow As Long = dgTabla.SelectedCells(0).RowIndex
                        Dim aCamposDel() As String = {"maquina", "id_emp"}
                        Dim aStringsDel() As String = {SystemInformation.ComputerName, jytsistema.WorkID}
                        nPosicionTabla = EliminarRegistros(MyConn, lblInfo, dsLocal, nTablaTabla, "jsconnumcontrol", strSQL, aCamposDel, aStringsDel,
                                                              nRow, True)


                    End If
                End If
            Case "VENCOTPA11"
                If dgTabla.RowCount > 0 Then
                    If dgTabla.SelectedCells(0).RowIndex >= 0 Then

                        Dim nRow As Long = dgTabla.SelectedCells(0).RowIndex
                        Dim aCamposDel() As String = {"codigo", "ORIGEN", "id_emp"}
                        Dim aStringsDel() As String = {dtTabla.Rows(nRow).Item("codigo"), "COT", jytsistema.WorkID}
                        nPosicionTabla = EliminarRegistros(MyConn, lblInfo, dsLocal, nTablaTabla, "jsconctacom", strSQL, aCamposDel, aStringsDel,
                                                              nRow, True)


                    End If
                End If
            Case "POSPARAM38"
                If dgTabla.RowCount > 0 Then
                    If dgTabla.SelectedCells(0).RowIndex >= 0 Then

                        Dim nRow As Long = dgTabla.SelectedCells(0).RowIndex
                        Dim aCamposDel() As String = {"codigo", "modulo", "id_emp"}
                        Dim aStringsDel() As String = {dtTabla.Rows(nRow).Item("codigo"), FormatoTablaSimple(Modulo.iFrasesCintillo), jytsistema.WorkID}
                        nPosicionTabla = EliminarRegistros(MyConn, lblInfo, dsLocal, nTablaTabla, "jsconctatab", strSQL, aCamposDel, aStringsDel,
                                                              nRow, True)


                    End If
                End If

            Case "POSPARAM46"
                If dgTabla.RowCount > 0 Then
                    If dgTabla.SelectedCells(0).RowIndex >= 0 Then
                        Dim nRow As Long = dgTabla.SelectedCells(0).RowIndex
                        Dim aCamposDel() As String = {"codigo", "modulo", "id_emp"}
                        Dim aStringsDel() As String = {dtTabla.Rows(nRow).Item("codigo"), FormatoTablaSimple(Modulo.iDenominacionesMonetarias), jytsistema.WorkID}
                        nPosicionTabla = EliminarRegistros(MyConn, lblInfo, dsLocal, nTablaTabla, "jsconctatab", strSQL, aCamposDel, aStringsDel,
                                                              nRow, True)
                    End If
                End If
        End Select
    End Sub


    Private Sub dgTabla_CancelRowEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.QuestionEventArgs) Handles dgTabla.CancelRowEdit
        ft.mensajeAdvertencia("Cancelando")
    End Sub
    Private Sub dgTabla_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgTabla.CellValidated
        Select Case txtCodigo.Text
            Case "VENPARAM38"
                If dgTabla.CurrentCell.ColumnIndex = 1 Then
                    Dim numControl As String = dgTabla.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString
                    If Not String.IsNullOrEmpty(numControl.ToString) Then
                        ft.Ejecutar_strSQL(MyConn, " update jsconnumcontrol " _
                                        & " set numerocontrol = '" & numControl & "' " _
                                        & " where " _
                                        & " maquina = '" & SystemInformation.ComputerName & "' and " _
                                        & " id_emp = '" & jytsistema.WorkID & "'")

                        dsLocal = DataSetRequery(dsLocal, strSQL, MyConn, nTablaTabla, lblInfo)
                        dtTabla = dsLocal.Tables(nTablaTabla)

                        dgTabla.Refresh()
                        dgTabla.CurrentCell = dgTabla(0, e.RowIndex)

                    End If

                End If
            Case "VENCOTPA11"
                If dgTabla.CurrentCell.ColumnIndex = 1 Then
                    If Not String.IsNullOrEmpty(dgTabla.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then

                        ft.Ejecutar_strSQL(MyConn, " update jsconctacom " _
                                        & " set comentario = '" & dgTabla.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString & "' " _
                                        & " where " _
                                        & " codigo = '" & dtTabla.Rows(e.RowIndex).Item("codigo") & "' and " _
                                        & " origen = 'COT' and " _
                                        & " id_emp = '" & jytsistema.WorkID & "'")

                        dsLocal = DataSetRequery(dsLocal, strSQL, MyConn, nTablaTabla, lblInfo)
                        dtTabla = dsLocal.Tables(nTablaTabla)

                        dgTabla.Refresh()
                        dgTabla.CurrentCell = dgTabla(0, e.RowIndex)

                    End If
                End If
            Case "POSPARAM38"
                If dgTabla.CurrentCell.ColumnIndex = 1 Then
                    If Not String.IsNullOrEmpty(dgTabla.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then

                        ft.Ejecutar_strSQL(MyConn, " update jsconctatab " _
                                        & " set descrip = '" & dgTabla.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString & "' " _
                                        & " where " _
                                        & " codigo = '" & dtTabla.Rows(e.RowIndex).Item("codigo") & "' and " _
                                        & " modulo = '" & FormatoTablaSimple(Modulo.iFrasesCintillo) & "' and " _
                                        & " id_emp = '" & jytsistema.WorkID & "'")

                        dsLocal = DataSetRequery(dsLocal, strSQL, MyConn, nTablaTabla, lblInfo)
                        dtTabla = dsLocal.Tables(nTablaTabla)

                        dgTabla.Refresh()
                        dgTabla.CurrentCell = dgTabla(0, e.RowIndex)

                    End If
                End If
            Case "POSPARAM46"
                If dgTabla.CurrentCell.ColumnIndex = 1 Then
                    If Not String.IsNullOrEmpty(dgTabla.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then

                        ft.Ejecutar_strSQL(MyConn, " update jsconctatab " _
                                        & " set descrip = '" & dgTabla.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString & "' " _
                                        & " where " _
                                        & " codigo = '" & dtTabla.Rows(e.RowIndex).Item("codigo") & "' and " _
                                        & " modulo = '" & FormatoTablaSimple(Modulo.iDenominacionesMonetarias) & "' and " _
                                        & " id_emp = '" & jytsistema.WorkID & "'")

                        dsLocal = DataSetRequery(dsLocal, strSQL, MyConn, nTablaTabla, lblInfo)
                        dtTabla = dsLocal.Tables(nTablaTabla)

                        dgTabla.Refresh()
                        dgTabla.CurrentCell = dgTabla(0, e.RowIndex)

                    End If
                End If
        End Select

    End Sub

    Private Sub dgTabla_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
            Handles dgTabla.CellValidating
        Select Case txtCodigo.Text
            Case "VENPARAM38"
                If Not String.IsNullOrEmpty(dgTabla.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then
                    If e.ColumnIndex = 1 Then
                        If e.FormattedValue.ToString().Trim() = "" Then
                            ft.mensajeAdvertencia("DEBE INDICAR UN NUMERO DE CONTROL VALIDO...")
                            e.Cancel = True
                        End If
                    End If
                End If
            Case "VENCOTPA11"
                If Not String.IsNullOrEmpty(dgTabla.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then
                    If e.ColumnIndex = 1 Then
                        If e.FormattedValue.ToString().Trim() = "" Then
                            ft.mensajeAdvertencia("DEBE INDICAR UN COMENTARIO VALIDO...")
                            e.Cancel = True
                        End If
                    End If
                End If
            Case "POSPARAM38"
                If Not String.IsNullOrEmpty(dgTabla.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then
                    If e.ColumnIndex = 1 Then
                        If e.FormattedValue.ToString().Trim() = "" Then
                            ft.mensajeAdvertencia("DEBE INDICAR UNA FRASE PARA CINTILLO VALIDA...")
                            e.Cancel = True
                        End If
                    End If
                End If
            Case "POSPARAM46"
                If Not String.IsNullOrEmpty(dgTabla.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then
                    If e.ColumnIndex = 1 Then
                        If e.FormattedValue.ToString().Trim() = "" Then
                            ft.mensajeAdvertencia("DEBE INDICAR UNA DENNOMINACION MONETARIA VALIDA...")
                            e.Cancel = True
                        End If
                    End If
                End If
        End Select


    End Sub

    Private Sub dg_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dgTabla.CellEndEdit
        dgTabla.Rows(e.RowIndex).ErrorText = String.Empty
    End Sub

End Class