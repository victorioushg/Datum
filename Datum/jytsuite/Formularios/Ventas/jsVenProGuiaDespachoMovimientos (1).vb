Imports MySql.Data.MySqlClient
Public Class jsVenProGuiaDespachoMovimientos

    Private Const sModulo As String = "Proceso escogencia de facturas para guía de despacho"

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
    Private NumeroDeGuia As String

    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Cargar(ByVal MyCon As MySqlConnection, FechaInicial As Date, FechaFinal As Date, NumeroGuia As String)

        MyConn = MyCon
        FechaInicio = FechaInicial
        FechaFin = FechaFinal
        NumeroDeGuia = NumeroGuia

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

    Private Sub jsVenProGuiaDespachoMovimientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, NumeroDeGuia)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, NumeroDeGuia)
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

                    InsertEditVENTASRenglonGuiaDespacho(MyConn, lblInfo, True, NumeroDeGuia, _
                                    lvPedidos.Items(lvCont).Text, _
                                    CDate(lvPedidos.Items(lvCont).SubItems(1).Text), _
                                    lvPedidos.Items(lvCont).SubItems(2).Text, _
                                    lvPedidos.Items(lvCont).SubItems(3).Text, _
                                    lvPedidos.Items(lvCont).SubItems(6).Text, _
                                    CDbl(lvPedidos.Items(lvCont).SubItems(5).Text), _
                                    MontoFactura)

                End If
            Next
            MsgBox("Proceso terminado...", MsgBoxStyle.Information, sModulo)

            ProgressBar1.Value = 0
            lblProgreso.Text = ""

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, NumeroDeGuia)
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

        ds = DataSetRequery(ds, " select a.numfac, a.emision, a.codcli, a.comen, " _
                    & " b.nombre ,a.kilos, a.tot_fac, a.codven, a.relguia from jsvenencfac a " _
                    & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp ) " _
                    & " Where " _
                    & strSQLAsesor _
                    & " a.emision >= '" & FormatoFechaMySQL(CDate(txtDesde.Text)) & "' AND " _
                    & " a.emision <= '" & FormatoFechaMySQL(CDate(txtHasta.Text)) & "' AND " _
                    & " a.relguia = '' AND " _
                    & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                    & " a.id_emp = '" & jytsistema.WorkID & "'  " _
                    & " order by codven, numfac", MyConn, nTablaFacturas, lblInfo)

        dtFacturas = ds.Tables(nTablaFacturas)

        CargaListViewFacturasGuiaDespacho(lvPedidos, dtFacturas)

        'SELECCIONAR TODAS LAS FACTURAS
        For iCont = 0 To lvPedidos.Items.Count - 1
            lvPedidos.Items(iCont).Checked = True
        Next

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

End Class