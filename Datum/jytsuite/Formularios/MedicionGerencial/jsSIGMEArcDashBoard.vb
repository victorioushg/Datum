Imports MySql.Data.MySqlClient

Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting

Public Class jsSIGMEArcDashBoard
    Private Const sModulo As String = "Mercancías"
    Private Const lRegion As String = "RibbonButton114"
    Private Const nTabla As String = "mercancias"
    Private Const nTablaMovimientos As String = "movimientos"
    Private Const nTablaEquivalencias As String = "equivalencias"
    Private Const nTablaCompras As String = "compras"
    Private Const nTablaVentas As String = "ventas"
    Private Const nTablaIVA As String = "iva"
    Private Const nTablaCuotas As String = "tblCuotasAsesor"

    Private strSQL As String = "select * from jsmerctainv where id_emp = '" & jytsistema.WorkID & "' order by codart "
    Private strSQLMov As String
    Private strSQLEQu As String
    Private strSQLCompras As String
    Private strSQLVentas As String
    Private strSQLCuotas As String = ""
    Private strSQLIVA As String = " select tipo from jsconctaiva group by tipo "

    Private myConn As New MySqlConnection(jytsistema.strConn)
    Private myCom As New MySqlCommand(strSQL, myConn)
    Private da As New MySqlDataAdapter(myCom)
    Private ds As New DataSet()
    Private dt As New DataTable
    Private dtMovimientos As New DataTable
    Private dtEquivalencias As New DataTable
    Private dtCuotas As New DataTable
    Private ft As New Transportables

    Private aIVA() As String
    Private aTipo() As String = {"Venta", "Uso interno", "POP", "Alquiler", "Préstamo", "Materia prima", "Venta & Envase", "Otros"}
    Private aMix() As String = {"Económico", "Estandar", "Superior"}
    Private aCondicion() As String = {"Activo", "Inactivo"}
    Private aTipoMovimiento() As String = {"Todos", "Entradas", "Salidas", "Ajuste de Entrada", "Ajuste de Salida", "Ajuste de Costo"}

    Private strUnidad As String = ""
    Private aUnidad() As String = {}

    Private i_modo As Integer
    Private nPosicionCat As Long, nPosicionMov As Long, nPosicionEqu As Long
    Private nPosicionCuo As Long
    Private CaminoFoto As String = ""

    Private Sub jsMerArcMercancias_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsMerArcMercancias_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Dock = DockStyle.Fill
        Try
            myConn.Open()
           
            DesactivarMarco0()

            tbcSIGME.SelectedTab = C1DockingTabPage1

        Catch ex As MySql.Data.MySqlClient.MySqlException
            ft.mensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try

    End Sub


    Private Sub ActivarMarco0()
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)
    End Sub

    Private Sub DesactivarMarco0()
        ft.mensajeEtiqueta(lblInfo, "Haga click sobre cualquier botón de la barra menu...", Transportables.tipoMensaje.iAyuda)
    End Sub
    Private Sub ColorOfertas(ByVal txt As TextBox, ByVal Color1 As Color)
        If ValorNumero(txt.Text) > 0 Then
            txt.BackColor = Color.PaleGreen
        Else
            txt.BackColor = Color1
        End If
    End Sub
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub
    Private Sub GuardarTXT()

    End Sub

    Private Sub VerTortaSaldoBancos()

        With chrtSaldosBancos
            .Legends.Clear()
            .Series.Clear()
            .ChartAreas.Clear()
            .Titles.Clear()
        End With

        Dim areas1 As ChartArea = chrtSaldosBancos.ChartAreas.Add("Areas1")

        With areas1
            .Name = "SALDOS DE BANCOS"
            .Area3DStyle.Enable3D = True
            .Area3DStyle.Inclination = 25
            .BackColor = ColorDeshabilitado
        End With

        Dim series1 As Series = chrtSaldosBancos.Series.Add("Series1")

        Dim dtSaldosBancos As New DataTable
        dtSaldosBancos = ft.AbrirDataTable(ds, "tblSaldosBancos", myConn, " select codban, nomban, saldoact " _
                                           & " from jsbancatban " _
                                           & " where id_emp = '" & jytsistema.WorkID & "' order by codban ")

        With series1
            .ChartArea = areas1.Name
            .ChartType = SeriesChartType.Pie
            For Each nRow As DataRow In dtSaldosBancos.Rows
                .Points.AddXY(nRow.Item("codban") + "|" + nRow.Item("nomban"), nRow.Item("saldoact"))
            Next

        End With

        Dim legend1 As Legend = chrtSaldosBancos.Legends.Add("Legends1")

        ' TITULO DEL DIAGRAMA
        Dim T As Title = chrtSaldosBancos.Titles.Add("SALDOS")
        With T
            .ForeColor = Color.Black
            .BackColor = Color.LightBlue
            .Font = New System.Drawing.Font("Consolas", 11, System.Drawing.FontStyle.Bold)
            .BorderColor = Color.Black
        End With

        ' Mostrar legendas
        chrtSaldosBancos.Legends(0).Enabled = True
        chrtSaldosBancos.Legends(0).Docking = Docking.Bottom
        chrtSaldosBancos.Legends(0).Alignment = System.Drawing.StringAlignment.Center

        ' colocar etiquetas y formnatos de etiquetas
        chrtSaldosBancos.Series(series1.Name)("PieLabelStyle") = "Outside"
        chrtSaldosBancos.Series(series1.Name).Label = "#PERCENT{P2}"
        chrtSaldosBancos.Series(series1.Name).LegendText = "#VALX (#VALY{#,#0.00})"
        chrtSaldosBancos.Series(series1.Name).IsValueShownAsLabel = True
        chrtSaldosBancos.Series(series1.Name).BackSecondaryColor = ColorDeshabilitado
        chrtSaldosBancos.DataManipulator.Sort(PointSortOrder.Descending, chrtSaldosBancos.Series(series1.Name))

        dtSaldosBancos.Dispose()
        dtSaldosBancos = Nothing

    End Sub

    Private Sub VerTortaSaldoBancosXMes()

        With chrtSaldosBancosXMes
            .Legends.Clear()
            .Series.Clear()
            .ChartAreas.Clear()
            .Titles.Clear()
        End With

        Dim areas1 As ChartArea = chrtSaldosBancosXMes.ChartAreas.Add("Areas1")

        With areas1
            .Name = "SALDOS DE BANCOS POR MES"
            .BackColor = ColorDeshabilitado
        End With


        Dim dtSaldosBancos As New DataTable
        dtSaldosBancos = ft.AbrirDataTable(ds, "tblSaldosBancos", myConn, SeleccionBANSaldosXMes("", ""))
        For Each nRow As DataRow In dtSaldosBancos.Rows

            Dim series1 As Series = chrtSaldosBancosXMes.Series.Add(nRow.Item("CODBAN") & "|" & nRow.Item("NOMBAN"))
            With series1
                .ChartArea = areas1.Name
                .ChartType = SeriesChartType.Line
                .Points.AddXY("ant", nRow.Item("CRE00"))
                .Points.AddXY("ENE", nRow.Item("CRE01"))
                .Points.AddXY("FEB", nRow.Item("CRE02"))
                .Points.AddXY("MAR", nRow.Item("CRE03"))
                .Points.AddXY("ABR", nRow.Item("CRE04"))
                .Points.AddXY("MAY", nRow.Item("CRE05"))
                .Points.AddXY("JUN", nRow.Item("CRE06"))
                .Points.AddXY("JUL", nRow.Item("CRE07"))
                .Points.AddXY("AGO", nRow.Item("CRE08"))
                .Points.AddXY("SEP", nRow.Item("CRE09"))
                .Points.AddXY("OCT", nRow.Item("CRE10"))
                .Points.AddXY("NOV", nRow.Item("CRE11"))
                .Points.AddXY("DIC", nRow.Item("CRE12"))

                '.Points.AddXY("ant", nRow.Item("CRE00") - nRow.Item("DEB00"))
                '.Points.AddXY("ENE", nRow.Item("CRE01") - nRow.Item("DEB01"))
                '.Points.AddXY("FEB", nRow.Item("CRE02") - nRow.Item("DEB02"))
                '.Points.AddXY("MAR", nRow.Item("CRE03") - nRow.Item("DEB03"))
                '.Points.AddXY("ABR", nRow.Item("CRE04") - nRow.Item("DEB04"))
                '.Points.AddXY("MAY", nRow.Item("CRE05") - nRow.Item("DEB05"))
                '.Points.AddXY("JUN", nRow.Item("CRE06") - nRow.Item("DEB06"))
                '.Points.AddXY("JUL", nRow.Item("CRE07") - nRow.Item("DEB07"))
                '.Points.AddXY("AGO", nRow.Item("CRE08") - nRow.Item("DEB08"))
                '.Points.AddXY("SEP", nRow.Item("CRE09") - nRow.Item("DEB09"))
                '.Points.AddXY("OCT", nRow.Item("CRE10") - nRow.Item("DEB10"))
                '.Points.AddXY("NOV", nRow.Item("CRE11") - nRow.Item("DEB11"))
                '.Points.AddXY("DIC", nRow.Item("CRE12") - nRow.Item("DEB12"))

            End With

        Next



        Dim legend1 As Legend = chrtSaldosBancosXMes.Legends.Add("Legends1")

        ' TITULO DEL DIAGRAMA
        Dim T As Title = chrtSaldosBancosXMes.Titles.Add("SALDOS POR MES")
        With T
            .ForeColor = Color.Black
            .BackColor = Color.LightBlue
            .Font = New System.Drawing.Font("Consolas", 11, System.Drawing.FontStyle.Bold)
            .BorderColor = Color.Black
        End With

        '' Mostrar legendas
        chrtSaldosBancosXMes.Legends(0).Enabled = True
        chrtSaldosBancosXMes.Legends(0).Docking = Docking.Bottom
        chrtSaldosBancosXMes.Legends(0).Alignment = System.Drawing.StringAlignment.Center

        '' colocar etiquetas y formnatos de etiquetas
        'chrtSaldosBancosXMes.Series(series1.Name)("PieLabelStyle") = "Outside"
        'chrtSaldosBancosXMes.Series(series1.Name).Label = "#PERCENT{P2}"
        'chrtSaldosBancosXMes.Series(series1.Name).LegendText = "#VALX (#VALY{#,#0.00})"
        'chrtSaldosBancosXMes.Series(series1.Name).IsValueShownAsLabel = True
        'chrtSaldosBancosXMes.Series(series1.Name).BackSecondaryColor = ColorDeshabilitado
        'chrtSaldosBancosXMes.DataManipulator.Sort(PointSortOrder.Descending, chrtSaldosBancosXMes.Series(series1.Name))

        dtSaldosBancos.Dispose()
        dtSaldosBancos = Nothing

    End Sub

    Private Sub tbcMercas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbcSIGME.SelectedIndexChanged
        Select Case tbcSIGME.SelectedIndex
            Case 0 ' BANCOS
                VerTortaSaldoBancos()
                VerTortaSaldoBancosXMes()
            Case 1 ' Movimientos

            Case 2 ' Compras

            Case 3 ' Ventas

            Case 4 ' Existencias

            Case 5 'Cuotas

            Case 6 ' Expediente

        End Select
    End Sub
  
    Private Function ValoresMensuales(ByVal dtValores As DataTable) As Double()
        Dim aMes As Double() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        If dtValores.Rows.Count > 0 Then
            With dtValores.Rows(0)
                aMes(0) = CDbl(.Item("ENE"))
                aMes(1) = CDbl(.Item("FEB"))
                aMes(2) = CDbl(.Item("MAR"))
                aMes(3) = CDbl(.Item("ABR"))
                aMes(4) = CDbl(.Item("MAY"))
                aMes(5) = CDbl(.Item("JUN"))
                aMes(6) = CDbl(.Item("JUL"))
                aMes(7) = CDbl(.Item("AGO"))
                aMes(8) = CDbl(.Item("SEP"))
                aMes(9) = CDbl(.Item("OCT"))
                aMes(10) = CDbl(.Item("NOV"))
                aMes(11) = CDbl(.Item("DIC"))
            End With
        End If
        ValoresMensuales = aMes

    End Function
  

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

   
    Private Sub txtIVA_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    
 
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class