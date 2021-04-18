Imports MySql.Data.MySqlClient
Public Class jsMerProCostos
    Private Const sModulo As String = "Recalculo de costos a partir de una fecha"
    Private Const nTabla As String = "tblProConteo"

    Private strSQL As String

    Private myConn As New MySqlConnection
    Private ds As New DataSet
    Private dt As New DataTable
    Private ft As New Transportables

    Private CodigoArticulo As String
    Private Fecha As Date

    Public Sub Cargar(ByVal MyCon As MySqlConnection, ByVal CodigoMercancia As String, ByVal FechaMovimiento As Date)

        myConn = MyCon
        CodigoArticulo = CodigoMercancia
        Fecha = FechaMovimiento
        lblLeyenda.Text = " 1. Recalcula los costos de los movimientos de mercancías apartir de una fecha "

        Me.ShowDialog()


    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub jsMerProProcesarConteo_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsMerProProcesarConteo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        ActualizarCostosEnMovmimientos()
        ft.mensajeInformativo(" Proceso culminado ...")
        ProgressBar1.Value = 0
        lblProgreso.Text = ""
        Me.Close()

    End Sub

    Private Sub ActualizarCostosEnMovmimientos()
        Dim dtMovSal As DataTable
        Dim nTablaaMovSal As String = "tblmovsalida"

        'ds = DataSetRequery(ds, " select * " _
        '                    & " from jsmertramer " _
        '                    & " where " _
        '                    & " NUMDOC IN ('FC00047153','FC00047170') AND " _
        '                    & " codart = '" & CodigoArticulo & "' AND " _
        '                    & " origen in ('FAC', 'PVE', 'NDV', 'TRF', 'INV') AND " _
        '                    & " tipomov in ('SA', 'AS' ) AND " _
        '                    & " ID_EMP = '" & jytsistema.WorkID & "'  " _
        '                    & " UNION select * " _
        '                    & " from jsmertramer " _
        '                    & " where " _
        '                    & " codart = '" & CodigoArticulo & "' AND " _
        '                    & " NUMDOC IN ('FC00047153','FC00047170') AND " _
        '                    & " origen in ('NCV') AND " _
        '                    & " tipomov in ('EN') AND " _
        '                    & " ID_EMP = '" & jytsistema.WorkID & "'  " _
        '                    & " ORDER BY FECHAMOV ", myConn, nTablaaMovSal, lblInfo)

        ds = DataSetRequery(ds, " select * " _
                            & " from jsmertramer " _
                            & " where " _
                            & " codart = '" & CodigoArticulo & "' AND " _
                            & " origen in ('FAC', 'PVE', 'NDV', 'TRF', 'INV') AND " _
                            & " tipomov in ('SA', 'AS' ) AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'  " _
                            & " UNION select * " _
                            & " from jsmertramer " _
                            & " where " _
                            & " codart = '" & CodigoArticulo & "' AND " _
                            & " origen in ('NCV') AND " _
                            & " tipomov in ('EN') AND " _
                            & " ID_EMP = '" & jytsistema.WorkID & "'  " _
                            & " ORDER BY FECHAMOV ", myConn, nTablaaMovSal, lblInfo)

        
        dtMovSal = ds.Tables(nTablaaMovSal)

        Dim jCont As Integer = 0
        For jCont = 0 To dtMovSal.Rows.Count - 1

            With dtMovSal.Rows(jCont)

                Dim nCosto As Double = UltimoCostoAFecha(myConn, .Item("codart"), CDate(.Item("fechamov").ToString))
                Dim nEquivale As Double = Equivalencia(myConn,  .Item("codart"), .Item("unidad"))
                Dim Descuento As Double = 0

                Dim nPesoUnidadVenta As Double = ft.DevuelveScalarDoble(myConn, " select pesounidad from jsmerctainv where codart = '" & .Item("codart") & "' and id_emp = '" & jytsistema.WorkID & "' ")
                Dim nPesoMovimiento As Double = nPesoUnidadVenta * .Item("cantidad") / IIf(nEquivale = 0, 1, nEquivale)

                Dim Costotal As Double = nCosto * .Item("cantidad") / IIf(nEquivale = 0, 1, nEquivale)
                Dim CostotalDescuento As Double = nCosto * (1 - Descuento / 100) * .Item("cantidad") / IIf(nEquivale = 0, 1, nEquivale)

                InsertEditMERCASMovimientoInventario(myConn, lblInfo, False, .Item("codart"), CDate(.Item("fechamov").ToString), _
                                                      .Item("tipomov"), .Item("numdoc"), .Item("unidad"), .Item("cantidad"), nPesoMovimiento, _
                                                      Costotal, CostotalDescuento, .Item("origen"), .Item("numorg"), _
                                                      .Item("lote"), .Item("prov_cli"), .Item("ventotal"), .Item("ventotaldes"),
                                                      .Item("impiva"), .Item("descuento"), .Item("vendedor"), .Item("almacen"), _
                                                      .Item("asiento"), CDate(.Item("fechasi").ToString))
             
                refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso, CInt(jCont / dtMovSal.Rows.Count * 100), _
                                              " ARTICULO : " & .Item("CODART") & " " & .Item("numdoc") & " " & .Item("fechamov").ToString)

            End With
        Next

        dtMovSal.Dispose()
        dtMovSal = Nothing

    End Sub


    

End Class