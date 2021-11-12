Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Public Class jsMerProCuotasCompras

    Private Const sModulo As String = "Asignación de cuotas fijas a Asesores a partir de compras"
    Private Const lRegion As String = "RibbonButton293"
    Private Const nTabla As String = "tblCuotas"
    Private Const nTablaCompras As String = "tblCompras"

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private dtCompras As New DataTable
    Private ft As New Transportables


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

        Dim dates As SfDateTimeEdit() = {txtFechaDesde, txtFechaHasta}
        SetSizeDateObjects(dates)

        IniciarPeriodo()
        ft.RellenaCombo(aAsignacion, cmbAsignacion)
        chkCuota.Checked = True

        ft.mensajeEtiqueta(lblInfo, " Haga click sobre cualquier botón de la barra menu ...", Transportables.TipoMensaje.iAyuda)
        Me.ShowDialog()


    End Sub
    Private Sub jsMerArcCuotasAnuales_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
        dt = Nothing
        ds = Nothing
    End Sub
    Private Sub IniciarCuotas()

        aAsesores = ft.DevuelveScalarCadena(myConn, "SELECT " _
                                                        & " GROUP_CONCAT(a.codven SEPARATOR ';' ) " _
                                                        & " FROM jsvencatven a " _
                                                        & " WHERE " _
                                                        & " a.estatus = 1 AND " _
                                                        & " a.tipo = 0 AND " _
                                                        & " a.CLASE = 0 AND " _
                                                        & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                                        & " ORDER BY a.tipo, a.codven").ToString.Split(";")



        strSQLAsesores = ""
        For Each aa As String In aAsesores
            Dim porcentajeAsesor As Double = 0.0

            If cmbAsignacion.SelectedIndex = 0 Then 'CUOTA FIJA
                porcentajeAsesor = 1 / aAsesores.Count() * 100
            Else 'FACTOR DE ASESOR
                porcentajeAsesor = ft.DevuelveScalarDoble(myConn, " SELECT factorcuota from jsvencatven where codven = '" & aa & "' and id_emp = '" & jytsistema.WorkID & "' ")
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
            ft.habilitarObjetos(False, True, txtFechaDesde, txtFechaHasta, cmbAsignacion, chkCuota)
        Else
            ft.habilitarObjetos(True, True, txtFechaDesde, txtFechaHasta, cmbAsignacion, chkCuota)
        End If

        AsignaTXT(Posicion, True)

    End Sub
    Private Sub IniciarPeriodo()
        txtFechaDesde.Value = jytsistema.sFechadeTrabajo
        txtFechaHasta.Value = jytsistema.sFechadeTrabajo

    End Sub
    Private Sub IniciarCompras(FechaDesde As Date, FechaHasta As Date)

        strSQLCompras = " select 0 sel, a.numcom, a.emision, a.codpro, b.nombre " _
            & " from jsproenccom a " _
            & " left join jsprocatpro b on (a.codpro = b.codpro and a.id_emp = b.id_emp) " _
            & " where " _
            & " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and " _
            & " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
            & " a.cuota = 0 and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by a.numcom "

        ds = DataSetRequery(ds, strSQLCompras, myConn, nTablaCompras, lblInfo)
        dtCompras = ds.Tables(nTablaCompras)

        Dim aFld() As String = {"sel", "numcom", "emision", "codpro", "nombre"}
        Dim aNom() As String = {"", "N° Compra", "Emisión", "Código", "Nombre"}
        Dim aAnc() As Integer = {20, 120, 90, 100, 350}
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
        Dim aAnchos() As Integer = {80, 200, 70, 40}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro}
        Dim aFormatos() As String = {"", "", sFormatoCantidad, ""}

        For Each aa As String In aAsesores
            ReDim Preserve aCampos(aCampos.Length)
            ReDim Preserve aNombres(aNombres.Length)
            ReDim Preserve aAnchos(aAnchos.Length)
            ReDim Preserve aAlineacion(aAlineacion.Length)
            ReDim Preserve aFormatos(aFormatos.Length)
            aCampos(aCampos.Length - 1) = aa.ToString
            aNombres(aNombres.Length - 1) = "Asesor " + aa.ToString + ft.DevuelveScalarCadena(myConn, " select concat(' ',  nombres, ' ',apellidos) from jsvencatven where codven = '" + aa.ToString + "' and id_emp = '" + jytsistema.WorkID + "' ")
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
            ft.habilitarObjetos(True, True, txtFechaDesde, txtFechaHasta, cmbAsignacion, chkCuota)
        End If

    End Sub


    Private Sub txtFechaDesde_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtFechaHasta.ValueChanged
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
                        ft.Ejecutar_strSQL(myconn, " insert into jsvencuoart values('" & nAsesor & "','" & nRow.Item("ITEM") & "', 0,0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0,0,0,'" & jytsistema.WorkExercise & "','" & jytsistema.WorkID & "' ) ")
                    End If

                    '1.1. CONVIERTA CANTIDAD EN KILOGRAMOS
                    Dim nCantidad As Double = CantidadEquivalente(myConn, lblInfo, nRow.Item("item"), nRow.Item("unidad"), nRow.Item(nCol.ColumnName)) * _
                        ft.DevuelveScalarDoble(myConn, " select pesounidad from jsmerctainv where codart = '" & nRow.Item("ITEM") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                    ft.Ejecutar_strSQL(myconn, " update jsvencuoart set ESMES" & nMonth & " = ESMES" & nMonth & " + " & nCantidad & " " _
                                            & " where codven = '" & nAsesor & "' and codart = '" & nRow.Item("ITEM") & "' and TIPO = 0 AND id_emp = '" & jytsistema.WorkID & "' ")

                End If
            Next

            '2. SE COLOCA LA COMPRA COMO PROCESADA
            ft.Ejecutar_strSQL(myconn, " update jsproenccom set CUOTA = 1 WHERE " _
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

    Private Sub dgCompras_CellValidated(sender As Object, e As DataGridViewCellEventArgs) Handles dgCompras.CellValidated

    End Sub

    Private Sub dg_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dg.CellValidating

        If e.ColumnIndex <= 3 Then Return

        If Not String.IsNullOrEmpty(dg.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString) Then
            If e.ColumnIndex > 3 Then

                If Not ft.isNumeric(e.FormattedValue.ToString()) Then
                    ft.mensajeAdvertencia("DEBE INDICAR UNA CANTIDAD VALIDA ...")
                    e.Cancel = True
                End If

                Dim CantidadRepartida As Double = 0
                Dim CantidadCambio As Double = dg.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                Dim CantidadRecambio As Double = e.FormattedValue
                For i As Integer = 4 To dg.ColumnCount - 1
                    CantidadRepartida += dg.Rows(e.RowIndex).Cells(i).Value
                Next
                If CantidadRepartida - CantidadCambio + CantidadRecambio > dg.Rows(e.RowIndex).Cells(2).Value Then
                    ft.mensajeAdvertencia("La suma de las cuotas es mayor a la cantidad en la compra. Verifique por favor...")
                    e.Cancel = True
                End If
            End If
        End If

    End Sub
End Class