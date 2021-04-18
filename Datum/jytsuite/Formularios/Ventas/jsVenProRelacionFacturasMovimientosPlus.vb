Imports MySql.Data.MySqlClient
Public Class jsVenProRelacionFacturasMovimientosPlus

    Private Const sModulo As String = "Proceso escogencia de facturas para Relaci�n de Facturas"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dtAsesores As New DataTable
    Private dtFacturas As New DataTable
    Private ft As New Transportables

    Private n_Apuntador As Long

    Private strSQLAsesores As String = ""
    Private strSQLFacturas As String = ""

    Private nTablaAsesores As String = "tblAsesores"
    Private nTablaFacturas As String = "tblFacturas"

    Private aCampos() As String
    Private FindField As String
    Private aNombres() As String

    Dim tbl As String = "tbl" & ft.NumeroAleatorio(100000)
    Private m_SortingColumn As ColumnHeader

    Private MontoTotal, KilosTotal As Double
    Private Items As Integer
    Private FechaInicio, FechaFin As Date
    Private RelacionDeFacturas As String

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Cargar(ByVal MyCon As MySqlConnection, FechaInicial As Date, FechaFinal As Date, RelacionFacturas As String)

        MyConn = MyCon
        FechaInicio = FechaInicial
        FechaFin = FechaFinal
        RelacionDeFacturas = RelacionFacturas

        IniciarTXT()
        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()

        ft.habilitarObjetos(False, True, txtDesde, txtHasta, txtItems, txtPesoPedidos, txtTotalpedidos)

        txtDesde.Text = ft.FormatoFecha(FechaInicio)
        txtHasta.Text = ft.FormatoFecha(FechaFin)

        txtItems.Text = ft.FormatoEntero(0)
        txtPesoPedidos.Text = ft.FormatoCantidad(0.0)
        txtTotalpedidos.Text = ft.FormatoNumero(0.0)


        CargarAsesores()

        strSQLAsesores = ""

    End Sub

    Private Sub CargarAsesores()

        Dim aFields() As String = {"sel.entero.1.0", "codven.cadena.5.0", "nombre.cadena.50.0"}
        CrearTabla(MyConn, lblInfo, jytsistema.WorkDataBase, True, tbl, aFields)

        ft.Ejecutar_strSQL(myconn, " insert into " & tbl _
                       & " select 0 sel, codven, concat(nombres,' ',apellidos) nombre from jsvencatven where estatus = 1 and tipo = " & TipoVendedor.iFuerzaventa & " and ID_EMP = '" & jytsistema.WorkID & "' order by codven ")

        aCampos = {"sel", "codven", "nombre"}
        aNombres = {"", "Asesor", "Nombre"}

        ds = DataSetRequery(ds, " select * from " & tbl, MyConn, nTablaAsesores, lblInfo)
        dtAsesores = ds.Tables(nTablaAsesores)
        CargarListaSeleccionAsesores(dg, dtAsesores, 7, False)

    End Sub

    Private Sub jsVenProRelacionFacturasMovimientosPlus_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsVenProRelacionFacturasMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, RelacionDeFacturas)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, RelacionDeFacturas)
        Me.Close()
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If Validado() Then

            Dim lCont As Integer = 1
            For Each selectedItem As DataGridViewRow In dgFacturas.Rows

                ProgressBar1.Value = (lCont) / dgFacturas.RowCount * 100
                If selectedItem.Cells(0).Value Then
                    Dim CodigoCliente As String = selectedItem.Cells(3).Value.ToString
                    Dim MontoFactura As Double = ft.DevuelveScalarDoble(MyConn, " select tot_fac from jsvenencfac where numfac = '" & selectedItem.Cells(1).Value.ToString & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If MontoFactura = 0.0 Then _
                        MontoFactura = ft.DevuelveScalarDoble(MyConn, " select tot_ncr from jsvenencncr where numncr = '" & selectedItem.Cells(1).Value.ToString & "' and id_emp = '" & jytsistema.WorkID & "' ")
                    If MontoFactura = 0.0 Then _
                        MontoFactura = ft.DevuelveScalarDoble(MyConn, " select tot_ndb from jsvenencndb where numndb = '" & selectedItem.Cells(1).Value.ToString & "' and id_emp = '" & jytsistema.WorkID & "' ")

                    InsertEditVENTASRenglonRelacionDeFacturas(MyConn, lblInfo, True, RelacionDeFacturas, _
                                    selectedItem.Cells(1).Value.ToString, _
                                    CDate(selectedItem.Cells(2).Value.ToString), _
                                    selectedItem.Cells(3).Value.ToString, _
                                    selectedItem.Cells(4).Value.ToString, _
                                    selectedItem.Cells(7).Value.ToString, _
                                    CDbl(selectedItem.Cells(6).Value.ToString), _
                                    MontoFactura, 0)

                End If
                lCont += 1

            Next


            MsgBox("Proceso terminado...", MsgBoxStyle.Information, sModulo)

            ProgressBar1.Value = 0
            lblProgreso.Text = ""

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, RelacionDeFacturas)
            Me.Close()

        End If

    End Sub

    Private Sub AbrirFacturas(ByVal strSQLAsesor As String)

        Items = 0
        MontoTotal = 0.0
        KilosTotal = 0.0

        txtItems.Text = ft.FormatoEntero(Items)
        txtTotalpedidos.Text = ft.FormatoNumero(MontoTotal)
        txtPesoPedidos.Text = ft.FormatoCantidad(KilosTotal)

        Dim TiempoAtras As String = ft.FormatoFechaMySQL(DateAdd("m", -MesesAtras.i6, jytsistema.sFechadeTrabajo))

        ds = DataSetRequery(ds, " SELECT 1 sel, a.numfac, a.emision, a.codcli, a.comen, c.nombre, a.kilos, a.tot_fac,  " _
                            & " a.codven, a.relguia, SUM(b.importe) saldo " _
                            & " FROM jsvenencfac a " _
                            & " LEFT JOIN jsventracob b ON (a.numfac = b.nummov AND a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
                            & " LEFT JOIN jsvencatcli c ON (a.codcli = c.codcli AND a.id_emp = c.id_emp) " _
                            & " WHERE " _
                            & strSQLAsesor _
                            & " a.relfacturas = '' AND " _
                            & " a.emision >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
                            & " a.emision <= '" & ft.FormatoFechaMySQL(FechaFin) & "' AND " _
                            & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " GROUP BY 2 " _
                            & " HAVING ROUND(saldo,2) <> 0" _
                            & " UNION " _
                            & " SELECT 1 sel, a.numncr numfac, a.emision, a.codcli, a.comen, c.nombre, -1*a.kilos kilos, -1*a.tot_ncr tot_fac,  " _
                            & " a.codven, a.asiento relguia, SUM(b.importe) saldo " _
                            & " FROM jsvenencncr a " _
                            & " LEFT JOIN jsventracob b ON (a.numncr = b.nummov AND a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
                            & " LEFT JOIN jsvencatcli c ON (a.codcli = c.codcli AND a.id_emp = c.id_emp) " _
                            & " WHERE " _
                            & strSQLAsesor _
                            & " a.relfacturas = '' AND " _
                            & " a.emision >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
                            & " a.emision <= '" & ft.FormatoFechaMySQL(FechaFin) & "' AND " _
                            & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " GROUP BY 2 " _
                            & " HAVING ROUND(saldo,2) <> 0 " _
                            & " UNION " _
                            & " SELECT 1 sel, a.numndb numfac, a.emision, a.codcli, a.comen, c.nombre, a.kilos, a.tot_ndb tot_fac, " _
                            & " a.codven, a.asiento relguia, SUM(b.importe) saldo " _
                            & " FROM jsvenencndb a " _
                            & " LEFT JOIN jsventracob b ON (a.numndb = b.nummov AND a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
                            & " LEFT JOIN jsvencatcli c ON (a.codcli = c.codcli AND a.id_emp = c.id_emp) " _
                            & " WHERE " _
                            & strSQLAsesor _
                            & " a.relfacturas = '' AND " _
                            & " a.emision >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
                            & " a.emision <= '" & ft.FormatoFechaMySQL(FechaFin) & "' AND " _
                            & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " GROUP BY 2 " _
                            & " HAVING ROUND(saldo,2) <> 0 " _
                            & " order by 2 ", MyConn, nTablaFacturas, lblInfo)

        dtFacturas = ds.Tables(nTablaFacturas)

        CargarListaSeleccionFacturasGuiaDespacho(dgFacturas, dtFacturas, 7, False)


    End Sub

    Private Sub CalculaTotales()

        Items = 0
        MontoTotal = 0.0
        KilosTotal = 0.0

        For Each selectedItem As DataGridViewRow In dgFacturas.Rows
            If selectedItem.Cells(0).Value Then
                MontoTotal += ValorNumero(selectedItem.Cells(5).Value)
                KilosTotal += ValorNumero(selectedItem.Cells(6).Value)
                Items += 1
            End If
        Next

        txtItems.Text = ft.FormatoEntero(CInt(Items))
        txtTotalpedidos.Text = ft.FormatoNumero(MontoTotal)
        txtPesoPedidos.Text = ft.FormatoCantidad(KilosTotal)

    End Sub
    Private Sub dg_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick
        If e.ColumnIndex = 0 Then
            dtAsesores.Rows(e.RowIndex).Item(0) = Not CBool(dtAsesores.Rows(e.RowIndex).Item(0).ToString)
        End If

    End Sub
    Private Sub dgFacturas_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgFacturas.CellContentClick
        If e.ColumnIndex = 0 Then
            dtFacturas.Rows(e.RowIndex).Item(0) = Not CBool(dtFacturas.Rows(e.RowIndex).Item(0).ToString)
        End If

    End Sub
    Private Sub dg_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles dg.MouseUp
        CargarFacturas()
    End Sub
    Private Sub dgFacturas_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles dgFacturas.MouseUp
        CalculaTotales()
    End Sub

    Private Sub CargarFacturas()
        strSQLAsesores = " a.codven = 'XXXXX' or"
        For Each selectedItem As DataGridViewRow In dg.Rows
            If selectedItem.Cells(0).Value Then strSQLAsesores += " a.codven = '" & selectedItem.Cells(1).Value.ToString & "' OR"
        Next

        If strSQLAsesores.Length > 0 Then strSQLAsesores = "( " & strSQLAsesores.Substring(0, strSQLAsesores.Length - 3) & " ) AND "
        AbrirFacturas(strSQLAsesores)
        CalculaTotales()
    End Sub
End Class