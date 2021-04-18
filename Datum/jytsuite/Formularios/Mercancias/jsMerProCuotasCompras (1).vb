Imports MySql.Data.MySqlClient
Public Class jsMerProCuotasCompras

    Private Const sModulo As String = "Asignación de cuotas fijas a Asesores a partir de compras"
    Private Const lRegion As String = "RibbonButton293"
    Private Const nTabla As String = "tblCuotas"
    Private Const nTablaCompras As String = "tblCompras"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private dtCompras As New DataTable

    Private strSQL As String
    Private strSQLAsesores As String = ""
    Private strSQLCompras As String = ""
    Private str As String = ""
    Private Posicion As Long
    Private BindingSource1 As New BindingSource
    Private FindField As String
    Private aAsesores() As String = {}
    Private strCuota As String = ""
    Private aAsignacion() As String = {"fija", "factor asesor"}
    Public Sub Cargar(ByVal Mycon As MySqlConnection)

        Me.Dock = DockStyle.Fill
        myConn = Mycon

        IniciarPeriodo()
        RellenaCombo(aAsignacion, cmbAsignacion)
        chkCuota.Checked = True

        MensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", TipoMensaje.iAyuda)
        Me.ShowDialog()


    End Sub
    Private Sub jsMerArcCuotasAnuales_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        dt = Nothing
        ds = Nothing
    End Sub
    Private Sub IniciarCuotas()

        aAsesores = EjecutarSTRSQL_Scalar(myConn, lblInfo, "SELECT " _
                                                        & " GROUP_CONCAT(a.codven SEPARATOR ';' ) " _
                                                        & " FROM jsvencatven a " _
                                                        & " WHERE " _
                                                        & " a.estatus = 1 AND " _
                                                        & " a.tipo = 0 AND " _
                                                        & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                                        & " ORDER BY a.tipo, a.codven").ToString.Split(";")



        strSQLAsesores = ""
        For Each aa As String In aAsesores
            Dim porcentajeAsesor As Double = 0.0

            If cmbAsignacion.SelectedIndex = 0 Then 'CUOTA FIJA
                porcentajeAsesor = 1 / aAsesores.Count() * 100
            Else 'FACTOR DE ASESOR
                porcentajeAsesor = CDbl(EjecutarSTRSQL_Scalar(myConn, lblInfo, " SELECT factorcuota from jsvencatven where codven = '" & aa & "' and id_emp = '" & jytsistema.WorkID & "' "))
            End If
            strSQLAsesores += ", round(b.cantidad*" & porcentajeAsesor & "/100,3)  '" & aa & "' "

        Next


        strSQL = " select b.item, b.descrip, b.cantidad , b.unidad, a.numcom, a.codpro, a.emision " _
            & strSQLAsesores _
            & " FROM jsproenccom a " _
            & " LEFT JOIN jsprorencom b ON (a.numcom = b.numcom AND a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
            & " Left join jsmerctainv c on (b.item = c.codart and b.id_emp = c.id_emp) " _
            & " WHERE " _
            & strCuota _
            & str _
            & " a.id_emp = '" & jytsistema.WorkID & "' "


        ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        dt = ds.Tables(nTabla)

        IniciarGrilla()

        If dt.Rows.Count > 0 Then
            Posicion = 0
            HabilitarObjetos(False, True, btnFechaDesde, btnFechaHasta, cmbAsignacion, chkCuota)
        Else
            HabilitarObjetos(True, True, btnFechaDesde, btnFechaHasta, cmbAsignacion, chkCuota)
        End If

        AsignaTXT(Posicion, True)

    End Sub
    Private Sub IniciarPeriodo()
        HabilitarObjetos(False, True, txtFechaDesde, txtFechaHasta)
        txtFechaDesde.Text = FormatoFecha(jytsistema.sFechadeTrabajo)
        txtFechaHasta.Text = FormatoFecha(jytsistema.sFechadeTrabajo)

    End Sub
    Private Sub IniciarCompras(FechaDesde As Date, FechaHasta As Date)

        strSQLCompras = " select 0 sel, a.numcom, a.emision, a.codpro, b.nombre " _
            & " from jsproenccom a " _
            & " left join jsprocatpro b on (a.codpro = b.codpro and a.id_emp = b.id_emp) " _
            & " where " _
            & " a.emision >= '" & FormatoFechaMySQL(FechaDesde) & "' and " _
            & " a.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' and " _
            & " a.cuota = 0 and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by a.numcom "

        ds = DataSetRequery(ds, strSQLCompras, myConn, nTablaCompras, lblInfo)
        dtCompras = ds.Tables(nTablaCompras)

        Dim aFld() As String = {"sel", "numcom", "emision", "codpro", "nombre"}
        Dim aNom() As String = {"", "N° Compra", "Emisión", "Código", "Nombre"}
        Dim aAnc() As Long = {20, 120, 90, 100, 350}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                 AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", "", sFormatoFecha, "", ""}

        IniciarTablaSeleccion(dgCompras, dtCompras, aFld, aNom, aAnc, aAli, aFor, , True, 8)
        dg.ReadOnly = True
        For Each col As DataGridViewColumn In dg.Columns
            If col.Index > 0 Then col.ReadOnly = True
        Next

    End Sub


    Private Sub jsMerArcCuotasAnuales_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub AsignaTXT(ByVal nRow As Long, ByVal Actualiza As Boolean, Optional ByVal NumeroCelda As Integer = 0)
        Dim c As Integer = CInt(nRow)
        If Actualiza Then ds = DataSetRequery(ds, strSQL, myConn, nTabla, lblInfo)
        If c >= 0 AndAlso ds.Tables(nTabla).Rows.Count > 0 Then
            Me.BindingContext(ds, nTabla).Position = c

            dg.CurrentCell = dg(NumeroCelda, c)
        End If
    End Sub
    Private Sub IniciarGrilla()

        Dim aCampos() As String = {"item", "descrip", "cantidad", "unidad"}
        Dim aNombres() As String = {"Código", "Descripción", "Cantidad", "UND"}
        Dim aAnchos() As Long = {90, 250, 70, 40}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro}
        Dim aFormatos() As String = {"", "", sFormatoCantidad, ""}

        For Each aa As String In aAsesores
            ReDim Preserve aCampos(aCampos.Length)
            ReDim Preserve aNombres(aNombres.Length)
            ReDim Preserve aAnchos(aAnchos.Length)
            ReDim Preserve aAlineacion(aAlineacion.Length)
            ReDim Preserve aFormatos(aFormatos.Length)
            aCampos(aCampos.Length - 1) = aa.ToString
            aNombres(aNombres.Length - 1) = "Asesor " + aa.ToString
            aAnchos(aAnchos.Length - 1) = 70
            aAlineacion(aAlineacion.Length - 1) = AlineacionDataGrid.Derecha
            aFormatos(aFormatos.Length - 1) = sFormatoCantidad
        Next

        ReDim Preserve aCampos(aCampos.Length)
        ReDim Preserve aNombres(aNombres.Length)
        ReDim Preserve aAnchos(aAnchos.Length)
        ReDim Preserve aAlineacion(aAlineacion.Length)
        ReDim Preserve aFormatos(aFormatos.Length)
        aCampos(aCampos.Length - 1) = ""
        aNombres(aNombres.Length - 1) = ""
        aAnchos(aAnchos.Length - 1) = 70
        aAlineacion(aAlineacion.Length - 1) = AlineacionDataGrid.Derecha
        aFormatos(aFormatos.Length - 1) = sFormatoCantidad

        dg.Columns.Clear()

        IniciarTabla(dg, dt, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, , True, 8)

        FindField = "item"
        BindingSource1.DataSource = dt
        dg.DataSource = BindingSource1

        dg.ReadOnly = False
        dg.Columns("item").ReadOnly = True
        dg.Columns("descrip").ReadOnly = True
        dg.Columns("Cantidad").ReadOnly = True
        dg.Columns("unidad").ReadOnly = True

    End Sub


    Private Sub dg_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellDoubleClick
        Posicion = Me.BindingContext(ds, nTabla).Position

    End Sub
    Private Sub dg_RowHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles _
        dg.RowHeaderMouseClick, dg.CellMouseClick, dg.RegionChanged
        Me.BindingContext(ds, nTabla).Position = e.RowIndex
        Posicion = Me.BindingContext(ds, nTabla).Position
    End Sub

    ''    Private Sub dg_RegionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.RegionChanged
    ''
    ''  End Sub
    'Private Sub dg_RowValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.RowValidated
    '    Posicion = Me.BindingContext(ds, nTabla).Position
    '    If dg.CurrentCell.ColumnIndex >= 3 And dg.CurrentCell.ColumnIndex <= 14 Then
    '        EjecutarSTRSQL(myConn, lblInfo, " update jsmerctacuo set mes" & Format(dg.CurrentCell.ColumnIndex - 2, "00") & " = " & CDbl(dg.CurrentCell.Value) & " " _
    '                        & " where codart = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "' ")
    '        AsignaTXT(Posicion, True, dg.CurrentCell.ColumnIndex)
    '    End If
    'End Sub
    'Private Sub dg_CancelRowEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.QuestionEventArgs) Handles dg.CancelRowEdit
    '    MensajeAdvertencia(lblInfo, "Cancelando")
    'End Sub
    'Private Sub dg_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
    '        Handles dg.CellValidating

    '    Dim headerText As String = _
    '        dg.Columns(e.ColumnIndex).HeaderText

    '    If Mid(headerText, 1, 3) = "MES" Then Return

    '    If e.ColumnIndex >= 3 And e.ColumnIndex <= 14 Then
    '        If (String.IsNullOrEmpty(e.FormattedValue.ToString())) Then
    '            MensajeAdvertencia(lblInfo, "Debe indicar dígito(s) válido...")
    '            e.Cancel = True
    '        End If

    '        If Not IsNumeric(e.FormattedValue.ToString()) Then
    '            MensajeAdvertencia(lblInfo, "Debe indicar un número válido...")
    '            e.Cancel = True
    '        End If
    '    End If

    'End Sub

    'Private Sub dg_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
    '    Handles dg.CellEndEdit
    '    dg.Rows(e.RowIndex).ErrorText = String.Empty
    'End Sub
    'Private Sub dg_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dg.ColumnHeaderMouseClick
    '    If e.ColumnIndex < 2 Then
    '        Dim aCam() As String = {"codart", "nomart"}
    '        Dim aStr() As String = {"Código", "Nombre"}
    '        FindField = dt.Columns(aCam(e.ColumnIndex)).ColumnName
    '        lblBuscar.Text = aStr(e.ColumnIndex)
    '    End If
    'End Sub

    'Private Sub txtBuscar_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
    '    BindingSource1.DataSource = dt
    '    If dt.Columns(FindField).DataType Is GetType(String) Then _
    '        BindingSource1.Filter = FindField & " like '%" & txtBuscar.Text & "%'"
    '    dg.DataSource = BindingSource1
    'End Sub

    'Private Sub dg_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellValidated
    '    Select Case dg.CurrentCell.ColumnIndex
    '        Case 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14
    '            Dim Mes As String = "MES" & Format(dg.CurrentCell.ColumnIndex - 2, "00")
    '            EjecutarSTRSQL(myConn, lblInfo, " update jsmerctacuo set " & Mes & " = " & CDbl(dg.CurrentCell.Value) & " " _
    '                                    & " where codart = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' and id_emp = '" & jytsistema.WorkID & "' ")

    '            'EjecutarSTRSQL(myConn, lblInfo, " update jsmerctacuo set conteo = " & Conteo & ", costo_tot = " & Conteo & "*costou " _
    '            '                       & " where conmer = '" & txtConteo.Text & "' and codart = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' and id_emp = '" & jytsistema.WorkID & "' ")
    '            'AsignaMov(Me.BindingContext(ds, nTablaRenglones).Position, False)
    '    End Select
    'End Sub

    Private Sub dgCompras_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles _
        dgCompras.CellContentClick

        If e.ColumnIndex = 0 Then
            dtCompras.Rows(e.RowIndex).Item(0) = Not CBool(dtCompras.Rows(e.RowIndex).Item(0).ToString)
        End If

        str = ""
        For Each nRow As DataRow In dtCompras.Rows
            If CBool(nRow.Item(0).ToString) Then
                str += " (a.numcom = '" & nRow.Item(1).ToString & "' AND a.codpro  = '" & nRow.Item(3).ToString & "') OR "
            End If
        Next

        If Len(str) > 0 Then
            str = "(" & str.Substring(0, str.Length - 3) & ") and "
            IniciarCuotas()
        Else
            dg.Columns.Clear()
            HabilitarObjetos(True, True, btnFechaDesde, btnFechaHasta, cmbAsignacion, chkCuota)
        End If

    End Sub

    Private Sub btnFechaDesde_Click(sender As System.Object, e As System.EventArgs) Handles btnFechaDesde.Click
        txtFechaDesde.Text = SeleccionaFecha(CDate(txtFechaDesde.Text), Me, grp, btnFechaDesde)
    End Sub

    Private Sub btnFechaHasta_Click(sender As System.Object, e As System.EventArgs) Handles btnFechaHasta.Click
        txtFechaHasta.Text = SeleccionaFecha(CDate(txtFechaHasta.Text), Me, grp, btnFechaHasta)
    End Sub

    Private Sub txtFechaDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtFechaDesde.TextChanged, _
        txtFechaHasta.TextChanged
        If txtFechaDesde.Text <> "" And txtFechaHasta.Text <> "" Then
            If CDate(txtFechaHasta.Text) >= CDate(txtFechaDesde.Text) Then
                IniciarCompras(CDate(txtFechaDesde.Text), CDate(txtFechaHasta.Text))
                str = ""
                dg.Columns.Clear()
            End If
        End If

    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOK_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        ProcesarCompra()
    End Sub
    Private Sub ProcesarCompra()

        For Each nRow As DataRow In dt.Rows

            '1. SE COLOCA EN LA CUOTA PERTENECIENTE AL MES ACTUAL (LA CUOTA ANTERIOR + LA CUOTA DE LA COMPRA)
            Dim nMonth As String = Format(CType(nRow.Item("emision").ToString, Date).Month, "00")

            For Each nCol As DataColumn In dt.Columns
                If nCol.ColumnName.Substring(0, 3) = "000" Then
                    Dim nAsesor As String = nCol.ColumnName
                    If NumeroDeRegistrosEnTabla(myConn, "jsvencuoart", " codven = '" & nAsesor & "' and codart = '" & nRow.Item("ITEM") & "' and tipo = 0 ") = 0 Then
                        EjecutarSTRSQL(myConn, lblInfo, " insert into jsvencuoart values('" & nAsesor & "','" & nRow.Item("ITEM") & "', 0,0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0,0,0,'" & jytsistema.WorkExercise & "','" & jytsistema.WorkID & "' ) ")
                    End If

                    '1.1. CONVIERTA CANTIDAD EN KILOGRAMOS
                    Dim nCantidad As Double = nRow.Item(nCol.ColumnName) * CDbl(EjecutarSTRSQL_ScalarPLUS(myConn, " select pesounidad from jsmerctainv where codart = '" & nRow.Item("ITEM") & "' and id_emp = '" & jytsistema.WorkID & "' "))

                    EjecutarSTRSQL(myConn, lblInfo, " update jsvencuoart set ESMES" & nMonth & " = ESMES" & nMonth & " + " & nCantidad & " " _
                                            & " where codven = '" & nAsesor & "' and codart = '" & nRow.Item("ITEM") & "' and TIPO = 0 AND id_emp = '" & jytsistema.WorkID & "' ")

                End If
            Next

            '2. SE COLOCA LA COMPRA COMO PROCESADA
            EjecutarSTRSQL(myConn, lblInfo, " update jsproenccom set CUOTA = 1 WHERE " _
                & " NUMCOM = '" & nRow.Item("NUMCOM") & "' AND" _
                & " CODPRO = '" & nRow.Item("CODPRO") & "' AND " _
                & " ID_EMP = '" & jytsistema.WorkID & "' ")

            '2. EN EL CIERRE DE MES SE DEBEN TRANSFERIR LAS (CUOTAS - VENTAS_MES_ANTERIOR)  AL MES SIGUIENTE ??????

        Next

        IniciarCompras(CDate(txtFechaDesde.Text), CDate(txtFechaHasta.Text))
        dg.Columns.Clear()


    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkCuota.CheckedChanged
        strCuota = " c.cuotafija = " & IIf(chkCuota.Checked, "1", "0") & " and "
        IniciarCompras(CDate(txtFechaDesde.Text), CDate(txtFechaHasta.Text))
    End Sub
End Class