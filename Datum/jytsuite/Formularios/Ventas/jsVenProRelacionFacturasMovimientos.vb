Imports MySql.Data.MySqlClient
Public Class jsVenProRelacionFacturasMovimientos

    Private Const sModulo As String = "Proceso escogencia de facturas para Relación de Facturas"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dtAsesores As New DataTable
    Private dtFacturas As New DataTable

    Private n_Apuntador As Long

    Private strSQLAsesores As String = ""
    Private strSQLFacturas As String = ""

    Private nTablaAsesores As String = "tblAsesores"
    Private nTablaFacturas As String = "tblFacturas"

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

        HabilitarObjetos(False, True, txtDesde, txtHasta, txtItems, txtPesoPedidos, txtTotalpedidos)

        txtDesde.Text = FormatoFechaCorta(FechaInicio)
        txtHasta.Text = FormatoFechaCorta(FechaFin)

        txtItems.Text = FormatoEntero(0)
        txtPesoPedidos.Text = FormatoCantidad(0.0)
        txtTotalpedidos.Text = FormatoNumero(0.0)

        strSQLAsesores = "select codven, concat(nombres,' ',apellidos) nombre from jsvencatven where estatus = 1 and tipo = " & TipoVendedor.iFuerzaventa & " and ID_EMP = '" & jytsistema.WorkID & "' order by codven "
        ds = DataSetRequery(ds, strSQLAsesores, MyConn, nTablaAsesores, lblInfo)
        dtAsesores = ds.Tables(nTablaAsesores)

        CargaListViewAsesores(lvAsesores, dtAsesores)

        strSQLAsesores = ""
        lvPedidos.Items.Clear()

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

            Dim lvCont As Integer
            For lvCont = 0 To lvPedidos.Items.Count - 1

                ProgressBar1.Value = (lvCont + 1) / lvPedidos.Items.Count * 100

                If lvPedidos.Items(lvCont).Checked Then

                    Dim CodigoCliente As String = lvPedidos.Items(lvCont).SubItems(2).Text
                    Dim MontoFactura As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select tot_fac from jsvenencfac where numfac = '" & lvPedidos.Items(lvCont).Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    If MontoFactura = 0.0 Then _
                        MontoFactura = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select tot_ncr from jsvenencncr where numncr = '" & lvPedidos.Items(lvCont).Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    If MontoFactura = 0.0 Then _
                        MontoFactura = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select tot_ndb from jsvenencndb where numndb = '" & lvPedidos.Items(lvCont).Text & "' and id_emp = '" & jytsistema.WorkID & "' "))


                    InsertEditVENTASRenglonRelacionDeFacturas(MyConn, lblInfo, True, RelacionDeFacturas, _
                                    lvPedidos.Items(lvCont).Text, _
                                    CDate(lvPedidos.Items(lvCont).SubItems(1).Text), _
                                    lvPedidos.Items(lvCont).SubItems(2).Text, _
                                    lvPedidos.Items(lvCont).SubItems(3).Text, _
                                    lvPedidos.Items(lvCont).SubItems(6).Text, _
                                    CDbl(lvPedidos.Items(lvCont).SubItems(5).Text), _
                                    MontoFactura, 0)

                End If
            Next
            MsgBox("Proceso terminado...", MsgBoxStyle.Information, sModulo)

            ProgressBar1.Value = 0
            lblProgreso.Text = ""

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, RelacionDeFacturas)
            Me.Close()

        End If

    End Sub

    Private Sub lvAsesores_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvAsesores.ItemChecked

        Dim iSel As Integer = 0

        If e.Item.Checked Then
            strSQLAsesores += " a.codven = '" & e.Item.Text & "' OR"
            iSel += 1
        Else
            strSQLAsesores = Replace(strSQLAsesores, " a.codven = '" & e.Item.Text & "' OR", "")
            iSel -= 1
        End If
        If iSel <= 0 Then strSQLAsesores = " a.codven = '' OR"


        AbrirFacturas("( " & strSQLAsesores.Substring(0, strSQLAsesores.Length - 3) & " ) AND ")

    End Sub

    Private Sub AbrirFacturas(ByVal strSQLAsesor As String)

        Dim iCont As Integer

        Items = 0
        MontoTotal = 0.0
        KilosTotal = 0.0

        txtItems.Text = FormatoEntero(Items)
        txtTotalpedidos.Text = FormatoNumero(MontoTotal)
        txtPesoPedidos.Text = FormatoCantidad(KilosTotal)

        ds = DataSetRequery(ds, " select a.NUMFAC, a.emision, a.codcli, a.comen, " _
                    & " b.nombre ,a.kilos, a.tot_fac, a.codven, a.relguia , if( isnull(c.saldo), 0.00, c.saldo) saldo " _
                    & " from jsvenencfac a " _
                    & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
                    & " left join (select a.codcli, a.nummov, sum(a.importe) saldo from jsventracob a group by 1,2 having round(saldo,2) <> 0.00 ) c " _
                    & " on (a.numfac = c.nummov and a.codcli = c.codcli) " _
                    & " Where " _
                    & " c.saldo <> 0 and " _
                    & strSQLAsesor _
                    & " a.emision >= '" & FormatoFechaMySQL(FechaInicio) & "' and " _
                    & " a.emision <= '" & FormatoFechaMySQL(FechaFin) & "' and " _
                    & " a.relfacturas = ''  and " _
                    & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " UNION" _
                    & " select a.numncr numfac, a.emision, a.codcli, a.comen, " _
                    & " b.nombre , -1*a.kilos kilos , -1*a.tot_ncr tot_fac, a.codven, a.asiento , if( isnull(c.saldo), 0.00, c.saldo) saldo " _
                    & " from jsvenencncr a " _
                    & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
                    & " left join (select a.codcli, a.nummov, sum(a.importe) saldo from jsventracob a group by 1,2 having round(saldo,2) <> 0.00 ) c " _
                    & " on (a.numncr = c.nummov and a.codcli = c.codcli) " _
                    & " Where " _
                    & " c.saldo <> 0 and " _
                    & strSQLAsesor _
                    & " a.emision >= '" & FormatoFechaMySQL(FechaInicio) & "' and " _
                    & " a.emision <= '" & FormatoFechaMySQL(FechaFin) & "' and " _
                    & " a.relfacturas = ''  and " _
                    & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " UNION " _
                    & " select a.numndb numfac , a.emision, a.codcli, a.comen, " _
                    & " b.nombre , a.kilos , a.tot_ndb tot_fac, a.codven, a.asiento , if( isnull(c.saldo), 0.00, c.saldo) saldo " _
                    & " from jsvenencndb a " _
                    & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
                    & " left join (select a.codcli, a.nummov, sum(a.importe) saldo from jsventracob a group by 1,2 having round(saldo,2) <> 0.00 ) c " _
                    & " on (a.numndb = c.nummov and a.codcli = c.codcli) " _
                    & " Where " _
                    & " c.saldo <> 0 and " _
                    & strSQLAsesor _
                    & " a.emision >= '" & FormatoFechaMySQL(FechaInicio) & "' and " _
                    & " a.emision <= '" & FormatoFechaMySQL(FechaFin) & "' and " _
                    & " a.relfacturas = ''  and " _
                    & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " order by 1 ", MyConn, nTablaFacturas, lblInfo)

        dtFacturas = ds.Tables(nTablaFacturas)

        CargaListViewFacturasGuiaDespacho(lvPedidos, dtFacturas)

        'SELECCIONAR TODAS LAS FACTURAS
        For iCont = 0 To lvPedidos.Items.Count - 1
            lvPedidos.Items(iCont).Checked = True
        Next

    End Sub

    Private Sub lvPedidos_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvPedidos.ColumnClick
        Dim jCont As Integer
        If e.Column.ToString = "0" Then
            For jCont = 0 To lvPedidos.Items.Count - 1
                lvPedidos.Items(jCont).Checked = False
            Next
        End If
    End Sub

    Private Sub lvPedidos_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvPedidos.ItemChecked

        If e.Item.SubItems.Count > 3 Then
            If e.Item.Checked Then
                MontoTotal += ValorNumero(e.Item.SubItems(4).Text)
                KilosTotal += ValorNumero(e.Item.SubItems(5).Text)
                Items += 1
                strSQLFacturas += " numfac = '" & e.Item.Text & "' OR"
            Else
                MontoTotal -= ValorNumero(e.Item.SubItems(4).Text)
                KilosTotal -= ValorNumero(e.Item.SubItems(5).Text)
                Items -= 1
                strSQLFacturas = Replace(strSQLFacturas, "numfac = '" & e.Item.Text & "' OR", "")
            End If
            If Items <= 0 Then strSQLFacturas = " numfac = '' OR"

            txtItems.Text = FormatoEntero(CInt(Items))
            txtTotalpedidos.Text = FormatoNumero(MontoTotal)
            txtPesoPedidos.Text = FormatoCantidad(KilosTotal)

        End If

    End Sub

    Private Sub lvPedidos_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvPedidos.MouseClick

    End Sub

    Private Sub lvPedidos_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvPedidos.SelectedIndexChanged

    End Sub
End Class