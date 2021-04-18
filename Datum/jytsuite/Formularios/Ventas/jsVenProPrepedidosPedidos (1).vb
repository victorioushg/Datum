Imports MySql.Data.MySqlClient
Public Class jsVenProPrepedidosPedidos

    Private Const sModulo As String = "Procesar pre-pedidos a pedidos"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dtAsesores As New DataTable
    Private dtPrepedidos As New DataTable
    Private dtKilos As New DataTable
    Private dtDescuentos As New DataTable
    Private dtGRupos As New DataTable

    Private n_Apuntador As Long

    Private strSQLAsesores As String = ""
    Private strSQLPrepedidos As String = ""

    Private nTablaAsesores As String = "tblAsesores" & NumeroAleatorio(1000000)
    Private nTablaPrepedidos As String = "tblprepedidos" & NumeroAleatorio(1000000)
    Private nTablaKilos As String = "tblKilos" & NumeroAleatorio(1000000)
    Private nTablaDescuentos As String = "tblDescuentos" & NumeroAleatorio(1000000)
    Private nTablaGrupos As String = "tblGrupos" & NumeroAleatorio(1000000)

    Private m_SortingColumn As ColumnHeader

    Private MontoTotal As Double
    Private KilosTotal As Double
    Private Items As Integer = 0
    Private aGrupos() As String = {"Todos", "Division", "Jerarquía", "Marca", "Categoría"}
    Private eGrupo As Integer = 0
    Public Property Apuntador() As Long
        Get
            Return n_Apuntador
        End Get
        Set(ByVal value As Long)
            n_Apuntador = value
        End Set
    End Property
    Public Sub Cargar(ByVal MyCon As MySqlConnection)

        MyConn = MyCon
        IniciarTXT()
        Me.ShowDialog()

    End Sub
    Private Sub IniciarTXT()

        HabilitarObjetos(False, True, txtFecha, txtDesde, txtHasta, txtItems, txtPesoPrepedidos, txtTotalPrepedidos)

        txtFecha.Text = FormatoFechaCorta(jytsistema.sFechadeTrabajo)
        txtDesde.Text = FormatoFechaCorta(jytsistema.sFechadeTrabajo)
        txtHasta.Text = FormatoFechaCorta(jytsistema.sFechadeTrabajo)

        txtItemsFactura.Text = FormatoEntero(CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, _
                                                                        " select venfacpa17 from jsconctapar where id_emp = '" & _
                                                                        jytsistema.WorkID & "' ")))

        RellenaCombo(aGrupos, cmbGrupos, 1)

        txtItems.Text = FormatoEntero(0)
        txtPesoPrepedidos.Text = FormatoCantidad(0.0)
        txtTotalPrepedidos.Text = FormatoNumero(0.0)

        strSQLAsesores = "select codven, concat(nombres,' ',apellidos) nombre from jsvencatven where estatus = 1 and tipo = " & TipoVendedor.iFuerzaventa & " and ID_EMP = '" & jytsistema.WorkID & "' order by codven "
        ds = DataSetRequery(ds, strSQLAsesores, MyConn, nTablaAsesores, lblInfo)
        dtAsesores = ds.Tables(nTablaAsesores)

        CargaListViewAsesores(lvAsesores, dtAsesores)

        strSQLAsesores = ""
        lvPrepedidos.Items.Clear()

        tbc.SelectedTab = tbcPesos

    End Sub

    Private Sub jsVenProPrepedidosPedidos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtFecha.Text)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFecha.Text)
        Me.Close()
    End Sub

    Private Function Validado() As Boolean
        Validado = False

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then

            Dim nTablaProceso As String = "tblProceso" & NumeroAleatorio(100000)
            Dim dtProceso As New DataTable
            Dim strPro As String = ""


            Dim lvCont As Integer
            For lvCont = 0 To lvPrepedidos.Items.Count - 1

                ProgressBar1.Value = (lvCont + 1) / lvPrepedidos.Items.Count * 100

                Dim NumeroPrepedioAFacturar As String = lvPrepedidos.Items(lvCont).Text

                If lvPrepedidos.Items(lvCont).Checked Then



                    lblProgreso.Text = " Prepedido No. " & NumeroPrepedioAFacturar

                    Select Case cmbGrupos.SelectedIndex
                        Case 0 'todos
                            strPro = " SELECT '00000' codigo, COUNT( * ) cuenta FROM jsvenrenpedrgv a  " _
                                & " WHERE " _
                                & " a.numped = '" & NumeroPrepedioAFacturar & "' AND " _
                                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                & " GROUP BY codigo "
                        Case 1 'division
                            strPro = " SELECT IF( c.codigo IS NULL, '00000', c.codigo) codigo, COUNT(*) cuenta " _
                                & " FROM jsvenrenpedrgv a " _
                                & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                                & " LEFT JOIN jsvengruposfacturacion c ON (b.division = c.codigo AND b.id_emp = c.id_emp AND c.agrupadopor = " & cmbGrupos.SelectedIndex & " )  " _
                                & " WHERE " _
                                & " a.numped = '" & NumeroPrepedioAFacturar & "' AND " _
                                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                & " GROUP BY codigo "
                        Case 2 'jerarquia
                            strPro = " SELECT IF( c.codigo IS NULL, '00000', c.codigo) codigo, COUNT(*) cuenta " _
                                & " FROM jsvenrenpedrgv a " _
                                & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                                & " LEFT JOIN jsvengruposfacturacion c ON (b.TIPJER = c.codigo AND b.id_emp = c.id_emp AND c.agrupadopor = " & cmbGrupos.SelectedIndex & " )  " _
                                & " WHERE " _
                                & " a.numped = '" & NumeroPrepedioAFacturar & "' AND " _
                                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                & " GROUP BY codigo "
                        Case 3 'marca
                            strPro = " SELECT IF( c.codigo IS NULL, '00000', c.codigo) codigo, COUNT(*) cuenta " _
                                & " FROM jsvenrenpedrgv a " _
                                & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                                & " LEFT JOIN jsvengruposfacturacion c ON (b.MARCA = c.codigo AND b.id_emp = c.id_emp AND c.agrupadopor = " & cmbGrupos.SelectedIndex & " )  " _
                                & " WHERE " _
                                & " a.numped = '" & NumeroPrepedioAFacturar & "' AND " _
                                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                & " GROUP BY codigo "
                        Case 4 'categoria
                            strPro = " SELECT IF( c.codigo IS NULL, '00000', c.codigo) codigo, COUNT(*) cuenta " _
                                & " FROM jsvenrenpedrgv a " _
                                & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                                & " LEFT JOIN jsvengruposfacturacion c ON (b.GRUPO = c.codigo AND b.id_emp = c.id_emp AND c.agrupadopor = " & cmbGrupos.SelectedIndex & " )  " _
                                & " WHERE " _
                                & " a.numped = '" & NumeroPrepedioAFacturar & "' AND " _
                                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                & " GROUP BY codigo "
                    End Select

                    ds = DataSetRequery(ds, strPro, MyConn, nTablaProceso, lblInfo)
                    dtProceso = ds.Tables(nTablaProceso)

                    For lcont As Integer = 0 To dtProceso.Rows.Count - 1

                        Dim numeroRenglones As Integer = dtProceso.Rows(lcont).Item("cuenta")  ''   CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) cuenta from jsvenrenpedrgv where numped = '" & lvPrepedidos.Items(lvCont).Text & "' and id_emp = '" & jytsistema.WorkID & "' "))
                        Dim codRenglon As String = dtProceso.Rows(lcont).Item("codigo")
                        Dim nNum As Integer = -1
                        Dim numMaxRenglonesXPedido As Integer = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select venfacpa17 from jsconctapar where id_emp = '" & jytsistema.WorkID & "' "))

                        Dim NumeroPedido As String = ""

                        While nNum <= numeroRenglones

                            NumeroPedido = Contador(MyConn, lblInfo, Gestion.iVentas, "VENNUMPED", "04")

                            'TransferirRenglones
                            TransferirRenglones(MyConn, NumeroPrepedioAFacturar, NumeroPedido, nNum + 1, numMaxRenglonesXPedido, cmbGrupos.SelectedIndex, codRenglon)

                            'Transfiere y calcula Descuentos Globales o de Pedido
                            DescuentoGlobalEnPedido(NumeroPedido, NumeroPrepedioAFacturar)

                            'VERIFICAR PRECIOS DE FACTURACION
                            VerificarFacturacionAPartirDePrecios(NumeroPrepedioAFacturar, NumeroPedido)

                            'Calcular IVA
                            CalcularIVAPedido(NumeroPedido)

                            'Crear Encabezado pedido
                            If CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsvenrenped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "'")) > 0 Then _
                                                                CrearEncabezadoPedido(NumeroPedido, NumeroPrepedioAFacturar, codRenglon)

                            nNum += numMaxRenglonesXPedido

                        End While

                    Next
                    'Cambiar Estatus de prepedido
                    EjecutarSTRSQL(MyConn, lblInfo, " update jsvenencpedrgv set estatus = 1 where numped = '" & NumeroPrepedioAFacturar & "' and id_emp = '" & jytsistema.WorkID & "' ")


                End If

            Next


            MsgBox("Proceso terminado...", MsgBoxStyle.Information, sModulo)

            ProgressBar1.Value = 0
            lblProgreso.Text = ""

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFecha.Text)

            dtProceso.Dispose()
            dtProceso = Nothing

            Me.Close()

        End If

    End Sub
    Private Sub VerificarFacturacionAPartirDePrecios(ByVal NumeroPrepedido As String, ByVal NumeroPedido As String)

        Dim CodigoClientePrepedido As String = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select codcli from jsvenencpedrgv where numped = '" & NumeroPrepedido & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim clienteFacturaAPartirDeCostos As Boolean = CBool(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select codcre from jsvencatcli where codcli = '" & CodigoClientePrepedido & "' and id_emp = '" & jytsistema.WorkID & "' "))

        If clienteFacturaAPartirDeCostos Then
            Dim porcentajeGanancia As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select des_cli from jsvencatcli where codcli = '" & CodigoClientePrepedido & "' and id_emp = '" & jytsistema.WorkID & "' "))

            EjecutarSTRSQL(MyConn, lblInfo, " UPDATE jsvenrenped a, jsmerctainv b " _
                            & " SET a.precio = ROUND(b.montoultimacompra*(1+" & porcentajeGanancia & "/100),2), " _
                            & " des_cli = 0.00, des_art = 0.00, des_ofe = 0.00, " _
                            & " a.totren = a.cantran*ROUND(b.montoultimacompra*(1+" & porcentajeGanancia & "/100),2), " _
                            & " a.totrendes = a.cantran * ROUND(b.montoultimacompra * (1+" & porcentajeGanancia & "/ 100), 2) " _
                            & " WHERE " _
                            & " a.item = b.codart AND " _
                            & " a.id_emp = b.id_emp AND " _
                            & " a.numped = '" & NumeroPedido & "' AND " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' ")

        End If


    End Sub

    Private Sub TransferirRenglones(ByVal MyConn As MySqlConnection, ByVal NumeroPrepedido As String, ByVal NumeroPedido As String, _
                                    ByVal renglonInicial As Integer, ByVal renglonFinal As Integer, ByVal TipoRenglon As Integer, ByVal CodigoRenglon As String)

        Dim CodigoCliente As String = CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select codcli from jsvenencpedrgv where numped = '" & NumeroPrepedido & "' and id_emp = '" & jytsistema.WorkID & "' "))
        Dim DescuentoCliente As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select des_cli from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' "))
        Dim nVendedor As String = CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select codven from jsvenencpedrgv where numped = '" & NumeroPrepedido & "' and id_emp = '" & jytsistema.WorkID & "' "))


        'Renglones
        Select Case TipoRenglon
            Case 0 'TODOS
                EjecutarSTRSQL(MyConn, lblInfo, " insert into jsvenrenped " _
                      & " SELECT '" & NumeroPedido & "' NUMPED, a.RENGLON, a.ITEM, a.DESCRIP, a.IVA, a.ICS, IF( b.unidad = 'KGR', 'KGR', a.UNIDAD) unidad, " _
                      & " a.BULTOS, a.cantran cantidad, a.cantran, a.INVENTARIO, a.SUGERIDO, a.REFUERZO, IF( b.unidad = 'KGR', a.cantran, a.peso/a.cantidad*a.cantran) peso,  " _
                      & " a.LOTE, IF(b.REGULADO = 1, 1, IF( b.DESCUENTO = 0, 1, a.ESTATUS)), ROUND( IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A)),2) precio, " _
                      & " IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & ", a.DES_ART, a.DES_OFE, " _
                      & " ROUND((IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100 -  " _
                      & " (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100)*a.des_ofe/100) , 2)  " _
                      & " TOTREN, " _
                      & " ROUND((IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100 -  " _
                      & " (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100)*a.des_ofe/100) , 2)  " _
                      & " TOTRENDES, '" & NumeroPrepedido & "', a.RENGLON, a.CODCON, a.ACEPTADO, a.EDITABLE, a.EJERCICIO, a.ID_EMP " _
                      & " FROM jsvenrenpedrgv a " _
                      & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                      & " LEFT JOIN jsmerequmer c ON (a.item = c.codart AND a.unidad = c.uvalencia AND a.id_emp = c.id_emp) " _
                      & " WHERE " _
                      & " a.numped = '" & NumeroPrepedido & "' AND " _
                      & " a.cantran > 0 AND " _
                      & " a.id_emp = '" & jytsistema.WorkID & "' order by renglon limit " & renglonInicial & "," & renglonFinal & " ")

            Case 1 'DIVISION

                EjecutarSTRSQL(MyConn, lblInfo, " insert into jsvenrenped " _
                      & " SELECT '" & NumeroPedido & "' NUMPED, a.RENGLON, a.ITEM, a.DESCRIP, a.IVA, a.ICS, IF( b.unidad = 'KGR', 'KGR', a.UNIDAD) unidad, " _
                      & " a.BULTOS, a.cantran cantidad, a.cantran, a.INVENTARIO, a.SUGERIDO, a.REFUERZO, IF( b.unidad = 'KGR', a.cantran, a.peso/a.cantidad*a.cantran) peso,  " _
                      & " a.LOTE, IF(b.REGULADO = 1, 1, IF( b.DESCUENTO = 0, 1, a.ESTATUS)), ROUND( IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A)),2) precio, " _
                      & " IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & ", a.DES_ART, a.DES_OFE, " _
                      & " ROUND((IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100 -  " _
                      & " (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100)*a.des_ofe/100) , 2)  " _
                      & " TOTREN, " _
                      & " ROUND((IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100 -  " _
                      & " (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100)*a.des_ofe/100) , 2)  " _
                      & " TOTRENDES, '" & NumeroPrepedido & "', a.RENGLON, a.CODCON, a.ACEPTADO, a.EDITABLE, a.EJERCICIO, a.ID_EMP " _
                      & " FROM jsvenrenpedrgv a " _
                      & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                      & " LEFT JOIN jsmerequmer c ON (a.item = c.codart AND a.unidad = c.uvalencia AND a.id_emp = c.id_emp) " _
                      & " LEFT JOIN jsvengruposfacturacion d ON (b.division = d.codigo AND b.id_emp = d.id_emp AND d.agrupadopor = 1 ) " _
                      & " WHERE " _
                      & " d.codigo " & IIf(CodigoRenglon = "00000", " IS NULL AND ", " = '" & CodigoRenglon & "' AND ") _
                      & " a.numped = '" & NumeroPrepedido & "' AND " _
                      & " a.cantran > 0 AND " _
                      & " a.id_emp = '" & jytsistema.WorkID & "' order by renglon limit " & renglonInicial & "," & renglonFinal & " ")

            Case 2 'JERARQUIA

                EjecutarSTRSQL(MyConn, lblInfo, " insert into jsvenrenped " _
                      & " SELECT '" & NumeroPedido & "' NUMPED, a.RENGLON, a.ITEM, a.DESCRIP, a.IVA, a.ICS, IF( b.unidad = 'KGR', 'KGR', a.UNIDAD) unidad, " _
                      & " a.BULTOS, a.cantran cantidad, a.cantran, a.INVENTARIO, a.SUGERIDO, a.REFUERZO, IF( b.unidad = 'KGR', a.cantran, a.peso/a.cantidad*a.cantran) peso,  " _
                      & " a.LOTE, IF(b.REGULADO = 1, 1, IF( b.DESCUENTO = 0, 1, a.ESTATUS)), ROUND( IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A)),2) precio, " _
                      & " IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & ", a.DES_ART, a.DES_OFE, " _
                      & " ROUND((IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100 -  " _
                      & " (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100)*a.des_ofe/100) , 2)  " _
                      & " TOTREN, " _
                      & " ROUND((IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100 -  " _
                      & " (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100)*a.des_ofe/100) , 2)  " _
                      & " TOTRENDES, '" & NumeroPrepedido & "', a.RENGLON, a.CODCON, a.ACEPTADO, a.EDITABLE, a.EJERCICIO, a.ID_EMP " _
                      & " FROM jsvenrenpedrgv a " _
                      & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                      & " LEFT JOIN jsmerequmer c ON (a.item = c.codart AND a.unidad = c.uvalencia AND a.id_emp = c.id_emp) " _
                      & " LEFT JOIN jsvengruposfacturacion d ON (b.TIPJER = d.codigo AND b.id_emp = d.id_emp AND d.agrupadopor = 2 ) " _
                      & " WHERE " _
                      & " d.codigo " & IIf(CodigoRenglon = "00000", " IS NULL AND ", " = '" & CodigoRenglon & "' AND ") _
                      & " a.numped = '" & NumeroPrepedido & "' AND " _
                      & " a.cantran > 0 AND " _
                      & " a.id_emp = '" & jytsistema.WorkID & "' order by renglon limit " & renglonInicial & "," & renglonFinal & " ")
            Case 3 'MARCA

                EjecutarSTRSQL(MyConn, lblInfo, " insert into jsvenrenped " _
                      & " SELECT '" & NumeroPedido & "' NUMPED, a.RENGLON, a.ITEM, a.DESCRIP, a.IVA, a.ICS, IF( b.unidad = 'KGR', 'KGR', a.UNIDAD) unidad, " _
                      & " a.BULTOS, a.cantran cantidad, a.cantran, a.INVENTARIO, a.SUGERIDO, a.REFUERZO, IF( b.unidad = 'KGR', a.cantran, a.peso/a.cantidad*a.cantran) peso,  " _
                      & " a.LOTE, IF(b.REGULADO = 1, 1, IF( b.DESCUENTO = 0, 1, a.ESTATUS)), ROUND( IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A)),2) precio, " _
                      & " IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & ", a.DES_ART, a.DES_OFE, " _
                      & " ROUND((IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100 -  " _
                      & " (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100)*a.des_ofe/100) , 2)  " _
                      & " TOTREN, " _
                      & " ROUND((IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100 -  " _
                      & " (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100)*a.des_ofe/100) , 2)  " _
                      & " TOTRENDES, '" & NumeroPrepedido & "', a.RENGLON, a.CODCON, a.ACEPTADO, a.EDITABLE, a.EJERCICIO, a.ID_EMP " _
                      & " FROM jsvenrenpedrgv a " _
                      & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                      & " LEFT JOIN jsmerequmer c ON (a.item = c.codart AND a.unidad = c.uvalencia AND a.id_emp = c.id_emp) " _
                      & " LEFT JOIN jsvengruposfacturacion d ON (b.MARCA = d.codigo AND b.id_emp = d.id_emp AND d.agrupadopor = 3 ) " _
                      & " WHERE " _
                      & " d.codigo " & IIf(CodigoRenglon = "00000", " IS NULL AND ", " = '" & CodigoRenglon & "' AND ") _
                      & " a.numped = '" & NumeroPrepedido & "' AND " _
                      & " a.cantran > 0 AND " _
                      & " a.id_emp = '" & jytsistema.WorkID & "' order by renglon limit " & renglonInicial & "," & renglonFinal & " ")
            Case 4 'CATEGORIA
                EjecutarSTRSQL(MyConn, lblInfo, " insert into jsvenrenped " _
                      & " SELECT '" & NumeroPedido & "' NUMPED, a.RENGLON, a.ITEM, a.DESCRIP, a.IVA, a.ICS, IF( b.unidad = 'KGR', 'KGR', a.UNIDAD) unidad, " _
                      & " a.BULTOS, a.cantran cantidad, a.cantran, a.INVENTARIO, a.SUGERIDO, a.REFUERZO, IF( b.unidad = 'KGR', a.cantran, a.peso/a.cantidad*a.cantran) peso,  " _
                      & " a.LOTE, IF(b.REGULADO = 1, 1, IF( b.DESCUENTO = 0, 1, a.ESTATUS)), ROUND( IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A)),2) precio, " _
                      & " IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & ", a.DES_ART, a.DES_OFE, " _
                      & " ROUND((IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100 -  " _
                      & " (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100)*a.des_ofe/100) , 2)  " _
                      & " TOTREN, " _
                      & " ROUND((IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100 -  " _
                      & " (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100 - (IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran - IF(b.unidad = 'KGR', b.precio_A, IF( a.unidad = c.uvalencia,   b.precio_a / c.equivale , b.precio_A))*a.cantran*IF(b.REGULADO = 1, 0, IF( b.DESCUENTO = 0, 0, 1))*" & DescuentoCliente & "/100)*a.des_art/100)*a.des_ofe/100) , 2)  " _
                      & " TOTRENDES, '" & NumeroPrepedido & "', a.RENGLON, a.CODCON, a.ACEPTADO, a.EDITABLE, a.EJERCICIO, a.ID_EMP " _
                      & " FROM jsvenrenpedrgv a " _
                      & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                      & " LEFT JOIN jsmerequmer c ON (a.item = c.codart AND a.unidad = c.uvalencia AND a.id_emp = c.id_emp) " _
                      & " LEFT JOIN jsvengruposfacturacion d ON (b.GRUPO = d.codigo AND b.id_emp = d.id_emp AND d.agrupadopor = 4 ) " _
                      & " WHERE " _
                      & " d.codigo " & IIf(CodigoRenglon = "00000", " IS NULL AND ", " = '" & CodigoRenglon & "' AND ") _
                      & " a.numped = '" & NumeroPrepedido & "' AND " _
                      & " a.cantran > 0 AND " _
                      & " a.id_emp = '" & jytsistema.WorkID & "' order by renglon limit " & renglonInicial & "," & renglonFinal & " ")
        End Select

        'AJUSTAR POR CUOTA DE VENDEDORES
        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFVEPA02")) Then
            AjustarPorCuotaVendedor(MyConn, lblInfo, NumeroPedido, CDate(txtFecha.Text), nVendedor, "NUMPED", "jsvenrenped", "PED")
        End If


    End Sub
    Private Sub DescuentoGlobalEnPedido(ByVal NumeroPedido As String, ByVal NumeroPrepedido As String)

        'Tranfiere descuentos
        EjecutarSTRSQL(MyConn, lblInfo, " insert into jsvendesped " _
                        & " SELECT '" & NumeroPedido & "' numped, renglon, descrip, pordes, descuento, subtotal, aceptado, ejercicio, id_emp " _
                        & " FROM jsvendespedrgv " _
                        & " WHERE " _
                        & " numped = '" & NumeroPrepedido & "' AND " _
                        & " id_emp = '" & jytsistema.WorkID & "' ")

        'Recalcula descuentos
        Dim dtDescuentos As DataTable
        Dim nTablaDesc As String = "tblDescuentos"
        ds = DataSetRequery(ds, " select * from jsvendesped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaDesc, lblInfo)
        dtDescuentos = ds.Tables(nTablaDesc)
        Dim MontoInicial As Double = CalculaTotalRenglonesVentas(MyConn, lblInfo, "jsvenrenped", "numped", "totren", NumeroPedido, 0)
        ActualizarDescuentoVentas(MyConn, dtDescuentos, "jsvendesped", "numped", MontoInicial, lblInfo)

        dtDescuentos.Dispose()
        dtDescuentos = Nothing

    End Sub

    Private Sub CalcularIVAPedido(ByVal NumeroPedido As String)

        CalculaTotalIVAVentas(MyConn, lblInfo, "jsvendesped", "jsvenivaped", "jsvenrenped", "numped", NumeroPedido, _
                              "impiva", "totrendes", CDate(txtFecha.Text))

    End Sub
    Private Sub CrearEncabezadoPedido(ByVal NumeroPedido As String, ByVal NumeroPrePedido As String, ByVal codGrupo As String)

        Dim SubTotal As Double = CalculaTotalRenglonesVentas(MyConn, lblInfo, "jsvenrenped", "numped", "totren", NumeroPedido, 0)

        Dim Descuentos As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select if( sum(descuento) is null, 0, sum(descuento)) from jsvendesped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' "))
        Dim Cargos As Double = CalculaTotalRenglonesVentas(MyConn, lblInfo, "jsvenrenped", "numped", "totren", NumeroPedido, 1)
        Dim ImpuestoIVA As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select sum(impiva) from jsvenivaped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' group by numped "))
        Dim PorcentajeDescuento As Double = IIf(SubTotal <= 0.0, 0.0, (1 - (SubTotal - Descuentos) / SubTotal) * 100)
        Dim PesoTotal As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select sum(peso) from jsvenrenped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' group by numped "))
        Dim numItems As Integer = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) cuenta from jsvenrenped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' group by numped "))

        Dim FechaEmision As String = FormatoFechaMySQL(CDate(txtFecha.Text))
        Dim FechaEntrega As String = FormatoFechaMySQL(DateAdd(DateInterval.Day, 1, CDate(txtFecha.Text)))
        Dim FechaVencimiento As String = FormatoFechaMySQL(DateAdd(DateInterval.Day, 1, CDate(txtFecha.Text)))

        EjecutarSTRSQL(MyConn, lblInfo, " INSERT INTO jsvenencped " _
                       & " SELECT '" & NumeroPedido & "' , '" & FechaEmision & "', " _
                       & " '" & FechaEntrega & "' , " _
                       & " CODCLI, '" & codGrupo & "', CODVEN, TARIFA, " & SubTotal & " , " & PorcentajeDescuento & " , " _
                       & " " & Descuentos & "  , " _
                       & " " & Cargos & " , " _
                       & " " & ImpuestoIVA & " , " _
                       & " " & SubTotal - Descuentos + Cargos + ImpuestoIVA & " , " _
                       & " '" & FechaVencimiento & "' , PORDES1,PORDES2 ,PORDES3, PORDES4, " _
                       & " DESCUEN1, DESCUEN2, DESCUEN3, DESCUEN4, VENCE1, VENCE2, VENCE3, VENCE4, ESTATUS, " & numItems & ", CAJAS, " _
                       & " " & PesoTotal & ", " _
                       & " CONDPAG, TIPOCREDITO, FORMAPAG, NOMPAG, NUMPAG, ABONO, SERIE, NUMGIRO, PERGIRO, INTERES, PORINT, IMPRESA, " _
                       & " EJERCICIO, ID_EMP " _
                       & " FROM jsvenencpedrgv " _
                       & " WHERE  " _
                       & " numped = '" & NumeroPrePedido & "' AND " _
                       & " id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Private Sub btnFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFecha.Click
        txtFecha.Text = FormatoFechaCorta(SeleccionaFecha(CDate(txtFecha.Text), Me, btnFecha))
    End Sub
    Private Sub btnDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesde.Click
        txtDesde.Text = FormatoFechaCorta(SeleccionaFecha(CDate(txtDesde.Text), Me, sender))
    End Sub
    Private Sub btnHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHasta.Click
        txtHasta.Text = FormatoFechaCorta(SeleccionaFecha(CDate(txtHasta.Text), Me, sender))
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


        AbrirPedidos("( " & strSQLAsesores.Substring(0, strSQLAsesores.Length - 3) & " ) AND ")

    End Sub

    Private Sub AbrirPedidos(ByVal strSQLAsesor As String)

        Dim iCont As Integer

        Dim strTotren As String

        strTotren = "((b.cantran * b.Precio - b.cantran * b.Precio * b.des_cli / 100 - (b.cantran * b.Precio - b.cantran * b.Precio * b.des_cli / 100) * des_art / 100) - " _
           & " (b.cantran * b.Precio - b.cantran * b.Precio * b.des_cli / 100 - (b.cantran * b.Precio - b.cantran * b.Precio * b.des_cli / 100) * b.des_art / 100) * b.des_ofe / 100 ) * " _
           & " ( b.totrendes/b.totren ) "

        ds = DataSetRequery(ds, " select a.NUMPED, a.EMISION, a.comen, a.ENTREGA, a.TOT_PED, " _
           & " sum( if( b.cantran > 0, " _
           & " if( b.iva = ' ', " & strTotren & " , " _
           & " if( b.iva = 'A', " & (1 + PorcentajeIVA(MyConn, lblInfo, CDate(txtFecha.Text), "A") / 100) & "*" & strTotren & " , " _
           & " if( b.iva = 'B', " & (1 + PorcentajeIVA(MyConn, lblInfo, CDate(txtFecha.Text), "B") / 100) & "*" & strTotren & " , " _
           & " if( b.iva = 'C', " & (1 + PorcentajeIVA(MyConn, lblInfo, CDate(txtFecha.Text), "C") / 100) & "*" & strTotren & " , " _
           & " if( b.iva = 'D', " & (1 + PorcentajeIVA(MyConn, lblInfo, CDate(txtFecha.Text), "D") / 100) & "*" & strTotren & ", " & strTotren & " )) ))) " _
           & " , 0)) as Backorder, " _
           & " sum(if(b.cantran > 0, b.peso/b.cantidad*b.cantran , 0 )) as kilos, c.codcli, c.nombre, c.disponible " _
           & " FROM jsvenencpedrgv a " _
           & " left join jsvenrenpedrgv b on (a.numped = b.numped and a.ejercicio = b.ejercicio and a.id_emp = b.id_emp) " _
           & " left join jsvencatcli c on (a.codcli = c.codcli and a.id_emp = c.id_emp) " _
           & " Where " & strSQLAsesor _
           & " a.EMISION >= '" & FormatoFechaMySQL(CDate(txtDesde.Text)) & "' AND " _
           & " a.EMISION <= '" & FormatoFechaMySQL(CDate(txtHasta.Text)) & "' AND " _
           & " a.ESTATUS = 0  and " _
           & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
           & " group by a.numped " _
           & " order by a.CODVEN, a.NUMPED ", MyConn, nTablaPrepedidos, lblInfo)

        dtPrepedidos = ds.Tables(nTablaPrepedidos)

        CargaListViewPrepedidos(lvPrepedidos, dtPrepedidos)

        'SELECCIONAR TODOS LOS PEDIDOS
        For iCont = 0 To lvPrepedidos.Items.Count - 1
            lvPrepedidos.Items(iCont).Checked = True
        Next



    End Sub

    Private Sub lvPrepedidos_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvPrepedidos.ItemChecked

        If e.Item.SubItems.Count > 3 Then
            If e.Item.Checked Then
                MontoTotal += ValorNumero(e.Item.SubItems(4).Text)
                KilosTotal += ValorNumero(e.Item.SubItems(5).Text)
                Items += 1
                strSQLPrepedidos += " NUMPED = '" & e.Item.Text & "' OR"
            Else
                MontoTotal -= ValorNumero(e.Item.SubItems(4).Text)
                KilosTotal -= ValorNumero(e.Item.SubItems(5).Text)
                Items -= 1
                strSQLPrepedidos = Replace(strSQLPrepedidos, "NUMPED = '" & e.Item.Text & "' OR", "")
            End If
            If Items <= 0 Then strSQLPrepedidos = " NUMPED = '' OR"

            strSQLPrepedidos = RTrim(strSQLPrepedidos)

            txtItems.Text = FormatoEntero(CInt(Items))
            txtTotalPrepedidos.Text = FormatoNumero(MontoTotal)
            txtPesoPrepedidos.Text = FormatoCantidad(KilosTotal)

         
            If strSQLPrepedidos <> "" Then AbrirPedidosKilos("( " & Mid(strSQLPrepedidos, 1, strSQLPrepedidos.Length - 3) & ") AND")

            If strSQLPrepedidos <> "" Then AbrirPedidosDescuentos("( " & Mid(strSQLPrepedidos, 1, strSQLPrepedidos.Length - 3) & ") AND")

        End If

    End Sub
    Private Sub AbrirPedidosKilos(ByVal strSQLPedidos As String)
        Dim strSQL As String = " SELECT a.numped, a.item, a.renglon, a.descrip, a.cantran, a.cantidad " _
                    & " FROM jsvenrenpedrgv a " _
                    & " LEFT JOIN jsmerctainv b on (a.item = b.codart and a.id_emp = b.id_emp) " _
                    & " WHERE " _
                    & strSQLPedidos _
                    & " b.unidad = 'KGR' and " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' order by a.numped "

        ds = DataSetRequery(ds, strSQL, MyConn, nTablaKilos, lblInfo)
        dtKilos = ds.Tables(nTablaKilos)

        Dim aCampos() As String = {"numped", "item", "renglon", "descrip", "cantran"}
        Dim aNombres() As String = {"No. Pedido", "Item", "", "Descripción", "Kilos"}
        Dim aAnchos() As Long = {110, 70, 40, 360, 50}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha}

        Dim aFormatos() As String = {"", "", "", "", sFormatoCantidad}
        IniciarTabla(dg, dtKilos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, True, True, 8, False)

        dg.ReadOnly = False
        dg.Columns("numped").ReadOnly = True
        dg.Columns("item").ReadOnly = True
        dg.Columns("renglon").ReadOnly = True
        dg.Columns("descrip").ReadOnly = True

    End Sub

    Private Sub AbrirPedidosDescuentos(ByVal strSQLPedidos As String)
        Dim strSQLP As String = " SELECT a.numped, a.item, a.renglon, a.descrip, a.cantidad, a.unidad, a.precio, a.des_art " _
                    & " FROM jsvenrenpedrgv a " _
                    & " LEFT JOIN jsmerctainv b on (a.item = b.codart and a.id_emp = b.id_emp) " _
                    & " WHERE " _
                    & strSQLPedidos _
                    & " a.des_art <> 0.00 and " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' order by a.numped "

        ds = DataSetRequery(ds, strSQLP, MyConn, nTablaDescuentos, lblInfo)
        dtDescuentos = ds.Tables(nTablaDescuentos)

        Dim aCampos() As String = {"numped", "item", "renglon", "descrip", "cantidad", "unidad", "precio", "des_art", ""}
        Dim aNombres() As String = {"No. Pedido", "Item", "Renglón", "Descripción", "Cantidad", "UND", "Precio", "Dscto.", ""}
        Dim aAnchos() As Long = {110, 70, 50, 360, 70, 40, 70, 60, 90}
        Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                        AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda}

        Dim aFormatos() As String = {"", "", "", "", sFormatoCantidad, "", sFormatoNumero, sFormatoNumero, ""}
        IniciarTabla(dgDescuentos, dtDescuentos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, True, True, 8, False)

        dgDescuentos.ReadOnly = False
        Dim col As New DataGridViewTextBoxColumn
        For Each col In dgDescuentos.Columns
            If col.Name <> "des_art" Then col.ReadOnly = True
        Next

    End Sub

    Private Sub dg_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
         Handles dg.CellEndEdit
        dg.Rows(e.RowIndex).ErrorText = String.Empty
    End Sub

    Private Sub dg_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellValidated
        If dg.CurrentCell.ColumnIndex = 4 Then

            'Dim CantidadReal As Double = CantidadEquivalente(MyConn, lblInfo, CStr(dg.CurrentRow.Cells(1).Value), "", CDbl(dg.CurrentCell.Value))

            EjecutarSTRSQL(MyConn, lblInfo, " update jsvenrenpedrgv set cantran = " & CDbl(dg.CurrentCell.Value) & " " _
                            & " where " _
                            & " numped = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' and " _
                            & " item = '" & CStr(dg.CurrentRow.Cells(1).Value) & "' and " _
                            & " renglon = '" & CStr(dg.CurrentRow.Cells(2).Value) & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

        End If
    End Sub

    Private Sub dgDescuentos_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgDescuentos.CellValidated
        If dgDescuentos.CurrentCell.ColumnIndex = 7 Then

            EjecutarSTRSQL(MyConn, lblInfo, " update jsvenrenpedrgv set des_art = " & CDbl(dgDescuentos.CurrentCell.Value) & " " _
                            & " where " _
                            & " numped = '" & CStr(dgDescuentos.CurrentRow.Cells(0).Value) & "' and " _
                            & " item = '" & CStr(dgDescuentos.CurrentRow.Cells(1).Value) & "' and " _
                            & " renglon = '" & CStr(dgDescuentos.CurrentRow.Cells(2).Value) & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

        End If
    End Sub

    Private Sub dg_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
           Handles dg.CellValidating

        Dim headerText As String = _
            dg.Columns(e.ColumnIndex).HeaderText

        If Not headerText.Equals("Kilos") Then Return

        If (String.IsNullOrEmpty(e.FormattedValue.ToString())) Then
            MensajeAdvertencia(lblInfo, "Debe indicar dígito(s) válido...")
            e.Cancel = True
        End If

        If Not IsNumeric(e.FormattedValue.ToString()) Then
            MensajeAdvertencia(lblInfo, "Debe indicar un número válido...")
            e.Cancel = True
        End If

    End Sub

    Private Sub dgDescuentos_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
           Handles dgDescuentos.CellValidating

        Dim headerText As String = _
            dgDescuentos.Columns(e.ColumnIndex).HeaderText

        If Not headerText.Equals("Dscto.") Then Return

        If (String.IsNullOrEmpty(e.FormattedValue.ToString())) Then
            MensajeAdvertencia(lblInfo, "Debe indicar dígito(s) válido...")
            e.Cancel = True
        End If

        If Not IsNumeric(e.FormattedValue.ToString()) Then
            MensajeAdvertencia(lblInfo, "Debe indicar un número válido...")
            e.Cancel = True
        End If

    End Sub

    Private Sub dg_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick

    End Sub

    Private Sub txtItemsFactura_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtItemsFactura.TextChanged
        EjecutarSTRSQL(MyConn, lblInfo, " update jsconctapar set venfacpa17 = " & txtItemsFactura.Text & " where id_emp = '" & jytsistema.WorkID & "' ")
        EjecutarSTRSQL(MyConn, lblInfo, " update jsconparametros set valor = " & txtItemsFactura.Text _
                       & " where " _
                       & " gestion = 5 and modulo = 4 and codigo = 'VENFACPA17' and " _
                       & " id_emp = '" & jytsistema.WorkID & "' ")

    End Sub

    Private Sub cmbGrupos_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbGrupos.SelectedIndexChanged
        eGrupo = cmbGrupos.SelectedIndex
        AbrirGruposFacturacion(eGrupo)
    End Sub

    Private Sub btnAgregaGrupo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregaGrupo.Click
        Dim codSelected As String = ""
        Select Case cmbGrupos.SelectedIndex
            Case 0 'Todos
            Case 1 'Division
                codSelected = CargarTablaSimple(MyConn, lblInfo, ds, " select division codigo, descrip descripcion from jsmercatdiv where id_emp = '" & jytsistema.WorkID & "' order by division ", "DIVISION", "")
            Case 2 'Jerarquía
                codSelected = CargarTablaSimple(MyConn, lblInfo, ds, " select tipjer codigo, descrip descripcion from jsmerencjer where id_emp = '" & jytsistema.WorkID & "' order by tipjer ", "JERARQUIA", "")
            Case 3 'Marca
                codSelected = CargarTablaSimple(MyConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iMarcaMerca) & "' and id_emp = '" & jytsistema.WorkID & "' order by codigo  ", "MARCAS", "")
            Case 4 'Categoria
                codSelected = CargarTablaSimple(MyConn, lblInfo, ds, " select codigo, descrip descripcion from jsconctatab where modulo = '" & FormatoTablaSimple(Modulo.iCategoriaMerca) & "' and id_emp = '" & jytsistema.WorkID & "' order by codigo  ", "MARCAS", "")
        End Select
        If codSelected <> "" Then
            EjecutarSTRSQL(MyConn, lblInfo, " REPLACE INTO jsvengruposfacturacion set agrupadopor = " & cmbGrupos.SelectedIndex & ", CODIGO = '" & codSelected & "', ID_EMP = '" & jytsistema.WorkID & "' ")
        End If
        AbrirGruposFacturacion(cmbGrupos.SelectedIndex)
    End Sub

    Private Sub btnEliminaGrupo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaGrupo.Click

    End Sub

    Private Sub AbrirGruposFacturacion(ByVal AgrupadoPor As Integer)

        Dim str As String = ""
        Select Case AgrupadoPor
            Case 1
                str = " LEFT JOIN jsmercatdiv b on ( a.codigo = b.division and a.id_emp = b.id_emp )"
            Case 2
                str = " LEFT JOIN jsmerencjer b on ( a.codigo = b.tipjer and a.id_emp = b.id_emp )"
            Case 3
                str = " LEFT JOIN jsconctatab b on ( a.codigo = b.codigo and a.id_emp = b.id_emp and b.modulo = '" & FormatoTablaSimple(Modulo.iMarcaMerca) & "' )"
            Case 4
                str = " LEFT JOIN jsconctatab b on ( a.codigo = b.codigo and a.id_emp = b.id_emp and b.modulo = '" & FormatoTablaSimple(Modulo.iCategoriaMerca) & "')"
        End Select

        If AgrupadoPor > 0 Then
            Dim strSQLGR As String = " SELECT a.codigo, b.descrip, a.id_emp " _
                        & " FROM jsvengruposfacturacion a " _
                        & str _
                        & " WHERE " _
                        & " a.agrupadopor = " & AgrupadoPor & " and " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' order by a.codigo "

            ds = DataSetRequery(ds, strSQLGR, MyConn, nTablaGrupos, lblInfo)
            dtGRupos = ds.Tables(nTablaGrupos)

            Dim aCampos() As String = {"codigo", "descrip"}
            Dim aNombres() As String = {"Código Grupo", "Descripción"}
            Dim aAnchos() As Long = {40, 100}
            Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}

            Dim aFormatos() As String = {"", ""}
            IniciarTabla(dgGrupos, dtGRupos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False, , 8, False)
        Else
            dgGrupos.Columns.Clear()
            dg.DataSource = Nothing
        End If

    End Sub
End Class