Imports MySql.Data.MySqlClient
Public Class jsVenProGuiaPedidosMovimientos

    Private Const sModulo As String = "Proceso escogencia de pedidos para guía de pedidos"

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
    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal FechaInicial As Date, ByVal FechaFinal As Date, ByVal NumeroGuia As String)

        MyConn = MyCon
        FechaInicio = FechaInicial
        FechaFin = FechaFinal
        NumeroDeGuia = NumeroGuia

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

        strSQLAsesores = "select codven, concat(nombres,' ',apellidos) nombre from jsvencatven where estatus = 1 and tipo = " & TipoVendedor.iFuerzaventa & " and ID_EMP = '" & jytsistema.WorkID & "' order by codven "
        ds = DataSetRequery(ds, strSQLAsesores, MyConn, nTablaAsesores, lblInfo)
        dtAsesores = ds.Tables(nTablaAsesores)

        lvAsesores.Clear()
        CargaListViewAsesores(lvAsesores, dtAsesores)

        strSQLAsesores = ""
        lvPedidos.Items.Clear()

    End Sub

    Private Sub jsVenProGuiaPedidosMovimientos_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
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
                    Dim MontoFactura As Double = ft.DevuelveScalarDoble(MyConn, " select tot_ped from jsvenencped where numped = '" & lvPedidos.Items(lvCont).Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                    InsertEditVENTASRenglonGuiaPedidos(MyConn, lblInfo, True, NumeroDeGuia, _
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
        'Dim tblFactturas As String = "tbl" & ft.NumeroAleatorio(100000)

        Items = 0
        MontoTotal = 0.0
        KilosTotal = 0.0

        txtItems.Text = ft.FormatoEntero(Items)
        txtTotalpedidos.Text = ft.FormatoNumero(MontoTotal)
        txtPesoPedidos.Text = ft.FormatoCantidad(KilosTotal)

        'Dim aFld() As String = {"numfac.cadena.15.0", "emision.fecha.0.0", "codcli.cadena.15.0", "comen.cadena.250.0", "nombre.cadena.250.0", _
        '                        "kilos.doble.10.3", "tot_ped.doble.19.2"}
        'CrearTabla(MyConn, lblInfo, jytsistema.WorkDataBase, True, tblFactturas, aFld)


        ds = DataSetRequery(ds, " select a.numped numfac, a.emision, a.codcli, a.comen, " _
                    & " b.nombre ,a.kilos, a.tot_ped, a.codven, a.numpag from jsvenencped a " _
                    & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp ) " _
                    & " Where " _
                    & strSQLAsesor _
                    & " a.estatus = 0 and " _
                    & " a.emision >= '" & ft.FormatoFechaMySQL(FechaInicio) & "' AND " _
                    & " a.emision <= '" & ft.FormatoFechaMySQL(FechaFin) & "' AND " _
                    & " a.numpag = '' AND " _
                    & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                    & " a.id_emp = '" & jytsistema.WorkID & "'  " _
                    & " order by codven, numped", MyConn, nTablaFacturas, lblInfo)

        dtFacturas = ds.Tables(nTablaFacturas)
        CargaListViewFacturasGuiaDespacho(lvPedidos, dtFacturas, 2)

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
                strSQLFacturas += " numped = '" & e.Item.Text & "' OR"
            Else
                MontoTotal -= ValorNumero(e.Item.SubItems(4).Text)
                KilosTotal -= ValorNumero(e.Item.SubItems(5).Text)
                Items -= 1
                strSQLFacturas = Replace(strSQLFacturas, "numped = '" & e.Item.Text & "' OR", "")
            End If
            If Items <= 0 Then strSQLFacturas = " numped = '' OR"

            txtItems.Text = ft.FormatoEntero(CInt(Items))
            txtTotalpedidos.Text = ft.FormatoNumero(MontoTotal)
            txtPesoPedidos.Text = ft.FormatoCantidad(KilosTotal)

        End If

    End Sub

End Class