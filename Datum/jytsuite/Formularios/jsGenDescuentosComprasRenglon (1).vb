Imports MySql.Data.MySqlClient
Public Class jsGenDescuentosComprasRenglon

    Private Const sModulo As String = "Descuentos por renglón"
    Private Const nTabla As String = "tbldesrencom"

    Private MyConn As New MySqlConnection
    Private dsLocal As New DataSet
    Private dtLocal As New DataTable

    Private strSQlDes As String = ""

    Private i_modo As Integer
    Private nPosicion As Integer
    Private n_Descuento As Double

    Private Documento As String = ""
    Private Proveedor As String = ""
    Private ModuloOrigen As String = ""
    Private CodigoArticulo As String = ""
    Private RenglonItem As String = ""
    Public Property Descuento() As Double
        Get
            Return n_Descuento
        End Get
        Set(ByVal value As Double)
            n_Descuento = value
        End Set
    End Property
    Public Sub Cargar(ByVal MyCon As MySqlConnection,
                       ByVal NumeroDocumento As String, ByVal CodigoProveedor As String, Origen As String, _
                       ByVal Item As String, ByVal Renglon As String, ByVal MontoRenglon As Double)

        Documento = NumeroDocumento
        Proveedor = CodigoProveedor
        ModuloOrigen = Origen
        CodigoArticulo = Item
        RenglonItem = Renglon


        strSQlDes = " select * from jsprorendes where " _
            & " numdoc = '" & Documento & "' and " _
            & " codpro = '" & Proveedor & "' and " _
            & " origen = '" & Origen & "' and " _
            & " item = '" & Item & "' and " _
            & " renglon = '" & Renglon & "' and " _
            & " id_emp = '" & jytsistema.WorkID & "' order by num_desc "

        MyConn = MyCon

        dsLocal = DataSetRequery(dsLocal, strSQlDes, MyConn, nTabla, lblInfo)
        dtLocal = dsLocal.Tables(nTabla)

        HabilitarObjetos(False, True, txtMontoInicial, txtMontoyDescuentos, txtPorDescuentoTotal)

        txtMontoInicial.Text = FormatoNumero(MontoRenglon)
        txtMontoyDescuentos.Text = FormatoNumero(MontoMasDescuentos())
        IniciarGrilla()

        Me.Text += " " & sModulo

        Me.ShowDialog()

    End Sub
    Private Function MontoMasDescuentos() As Double
        Dim Resto As Double
        Dim Descuento As Double
        Resto = ValorNumero(txtMontoInicial.Text)
        If dtLocal.Rows.Count > 0 Then
            For iCont As Integer = 0 To dtLocal.Rows.Count - 1
                With dtLocal.Rows(iCont)
                    Descuento = Resto * .Item("pordes") / 100
                    Resto = Resto - Descuento
                    EjecutarSTRSQL(MyConn, lblInfo, " update jsprorendes set descuento = " & Descuento _
                        & " where " _
                        & " num_desc = '" & .Item("num_desc") & "' and " _
                        & " numdoc = '" & .Item("numdoc") & "' and " _
                        & " codpro = '" & Proveedor & "' and " _
                        & " origen = '" & .Item("Origen") & "' and " _
                        & " item = '" & .Item("Item") & "' and " _
                        & " renglon = '" & .Item("Renglon") & "' and " _
                        & " id_emp = '" & jytsistema.WorkID & "'")

                End With
            Next
        End If

        txtPorDescuentoTotal.Text = (ValorNumero(txtMontoInicial.Text) - Resto) / ValorNumero(txtMontoInicial.Text) * 100
        MontoMasDescuentos = Resto

    End Function
    Private Sub IniciarGrilla()

        dsLocal = DataSetRequery(dsLocal, strSQlDes, MyConn, nTabla, lblInfo)
        dtLocal = dsLocal.Tables(nTabla)


        Dim aCam() As String = {"num_desc", "pordes", "descuento"}
        Dim aNom() As String = {"Código", "Porcentaje Descuento", "Monto Descuento"}
        Dim aAnc() As Long = {70, 100, 100}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}
        Dim aFor() As String = {"", sFormatoNumero, sFormatoNumero}

        IniciarTabla(dgDescuentos, dtLocal, aCam, aNom, aAnc, aAli, aFor, True, True, 9, True)
        If dtLocal.Rows.Count > 0 Then nPosicion = 0

        dgDescuentos.ReadOnly = False
        dgDescuentos.Columns("num_desc").ReadOnly = True
        dgDescuentos.Columns("descuento").ReadOnly = True

    End Sub
    Private Sub jsGenArcDescuentosVentas_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dsLocal = Nothing
        dtLocal = Nothing
    End Sub

    Private Sub jsGenArcDescuentosVentas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, cmbDescuentos.Text)
    End Sub

    Private Function Validado() As Boolean

        Validado = False

        For Each nRow As DataRow In dtLocal.Rows
            With nRow
                If .Item("pordes") = 0 Then
                    MensajeCritico(lblInfo, "DEBE INDICAR UN PORCENTAJE VALIDO EN RENGLON " & .Item("NUM_DESC"))
                    Return False
                End If
            End With
        Next


        Validado = True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then

            Descuento = ValorNumero(txtPorDescuentoTotal.Text)
            Me.Close()

        End If
    End Sub

    Private Sub btnCancel_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Me.Close()
    End Sub
    Private Sub btnAgregaDescuento_Click(sender As System.Object, e As System.EventArgs) Handles btnAgregaDescuento.Click

        Dim aWherefields() As String = {"codpro", "numdoc", "Origen", "item", "renglon", "id_emp"}
        Dim aWhereValues() As String = {Proveedor, Documento, ModuloOrigen, CodigoArticulo, RenglonItem, jytsistema.WorkID}
        EjecutarSTRSQL(MyConn, lblInfo, " insert into jsprorendes " _
            & " set pordes = 0.00, " _
            & " codpro = '" & Proveedor & "', " _
            & " numdoc = '" & Documento & "', " _
            & " origen = '" & ModuloOrigen & "', " _
            & " item = '" & CodigoArticulo & "', " _
            & " renglon = '" & RenglonItem & "', " _
            & " num_desc = '" & AutoCodigoXPlus(MyConn, "num_desc", "jsprorendes", aWherefields, aWhereValues, 5) & "', " _
            & " descuento = " & 0.0 & ", " _
            & " id_emp = '" & jytsistema.WorkID & "'")

        IniciarGrilla()

    End Sub


    Private Sub dgDescuentos_CancelRowEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.QuestionEventArgs) Handles dgDescuentos.CancelRowEdit
        MensajeAdvertencia(lblInfo, "Cancelando")
    End Sub
    Private Sub dgDescuentos_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgDescuentos.CellValidated
        If dgDescuentos.CurrentCell.ColumnIndex = 1 Then

            Dim porDescuento As Double = dgDescuentos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
            If Not String.IsNullOrEmpty(porDescuento.ToString) Then
                EjecutarSTRSQL(MyConn, lblInfo, " update jsprorendes " _
                                & " set pordes = " & porDescuento & " " _
                                & " where " _
                                & " numdoc = '" & Documento & "' and " _
                                & " codpro = '" & Proveedor & "' and " _
                                & " origen = '" & ModuloOrigen & "' and " _
                                & " item = '" & CodigoArticulo & "' and " _
                                & " renglon = '" & RenglonItem & "' and " _
                                & " num_desc = '" & dgDescuentos.Rows(e.RowIndex).Cells(0).Value.ToString & "' and " _
                                & " id_emp = '" & jytsistema.WorkID & "'")

                txtMontoyDescuentos.Text = FormatoNumero(MontoMasDescuentos())

                dsLocal = DataSetRequery(dsLocal, strSQlDes, MyConn, nTabla, lblInfo)
                dtLocal = dsLocal.Tables(nTabla)

                dgDescuentos.Refresh()
                dgDescuentos.CurrentCell = dgDescuentos(0, e.RowIndex)

            End If

        End If





    End Sub

    Private Sub dgDescuentos_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
            Handles dgDescuentos.CellValidating
        If Not String.IsNullOrEmpty(dgDescuentos.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then
            If e.ColumnIndex = 1 Then
                If IsNumeric(e.FormattedValue.ToString()) Then
                    If ValorNumero(e.FormattedValue.ToString()) > 100 Or _
                        ValorNumero(e.FormattedValue.ToString()) < 0 Then
                        MensajeAdvertencia(lblInfo, "DEBE INDICAR UN PORCENTAJE VALIDO ...")
                        e.Cancel = True
                    End If
                Else
                    MensajeAdvertencia(lblInfo, "DEBE INDICAR UN PORCENTAJE VALIDO ...")
                    e.Cancel = True
                End If

            End If
        End If

    End Sub

    Private Sub dg_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
        Handles dgDescuentos.CellEndEdit
        dgDescuentos.Rows(e.RowIndex).ErrorText = String.Empty
    End Sub


    Private Sub btnEliminaDescuento_Click(sender As System.Object, e As System.EventArgs) Handles btnEliminaDescuento.Click
        If dgDescuentos.RowCount > 0 Then
            If dgDescuentos.SelectedCells(0).RowIndex >= 0 Then
                Dim nRow As Long = dgDescuentos.SelectedCells(0).RowIndex
                Dim aCamposDel() As String = {"numdoc", "codpro", "origen", "item", "renglon", "num_desc", "id_emp"}
                Dim aStringsDel() As String = {Documento, Proveedor, ModuloOrigen, CodigoArticulo, RenglonItem, dtLocal.Rows(nRow).Item("num_desc"), jytsistema.WorkID}
                EliminarRegistros(MyConn, lblInfo, dsLocal, nTabla, "jsprorendes", strSQlDes, aCamposDel, aStringsDel, _
                                                      nRow, True)

                txtMontoyDescuentos.Text = FormatoNumero(MontoMasDescuentos())

            End If
        End If
    End Sub

End Class