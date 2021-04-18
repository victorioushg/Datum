Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.Input
Imports fTransport
Public Class jsVenProPrepedidosPedidos

    Private Const sModulo As String = "Procesar pre-pedidos a pedidos"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dtAsesores As New DataTable
    Private dtPrepedidos As New DataTable
    Private dtPrepedidosX As New DataTable
    Private dtKilos As New DataTable
    Private dtDescuentos As New DataTable
    Private dtGRupos As New DataTable
    Private ft As New Transportables


    Private n_Apuntador As Long

    Private strSQLAsesores As String = ""
    Private strSQLPrepedidos As String = ""

    Private nTablaAsesores As String = "tblAsesores" & ft.NumeroAleatorio(1000000)
    Private nTablaPrepedidos As String = "tblprepedidos" & ft.NumeroAleatorio(1000000)
    Private nTablaPrepedidosX As String = "tblprepedidosX" & ft.NumeroAleatorio(1000000)
    Private nTablaKilos As String = "tblKilos" & ft.NumeroAleatorio(1000000)
    Private nTablaDescuentos As String = "tblDescuentos" & ft.NumeroAleatorio(1000000)
    Private nTablaGrupos As String = "tblGrupos" & ft.NumeroAleatorio(1000000)

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

        ft.habilitarObjetos(False, True, txtItems, txtPesoPrepedidos, txtTotalPrepedidos)

        txtFecha.Text = jytsistema.sFechadeTrabajo
        txtDesde.Text = jytsistema.sFechadeTrabajo
        txtHasta.Text = jytsistema.sFechadeTrabajo

        txtItemsFactura.Text = ft.FormatoEntero(ft.DevuelveScalarEntero(MyConn, _
                                                                        " select venfacpa17 from jsconctapar where id_emp = '" & _
                                                                        jytsistema.WorkID & "' "))

        ft.RellenaCombo(aGrupos, cmbGrupos, 1)

        txtItems.Text = ft.FormatoEntero(0)
        txtPesoPrepedidos.Text = ft.FormatoCantidad(0.0)
        txtTotalPrepedidos.Text = ft.FormatoNumero(0.0)

        CargarAsesores()

        strSQLAsesores = ""
        dgPedidos.Columns.Clear()

        tbc.SelectedTab = tbcPesos

    End Sub
    Private Sub CargarAsesores()

        Dim aFields() As String = {"SEL.entero.1.0", "CODVEN.cadena.5.0", "NOMBRE.cadena.100.0"}
        CrearTabla(MyConn, lblInfo, jytsistema.WorkDataBase, True, nTablaAsesores, aFields)

        dtAsesores = ft.AbrirDataTable(ds, nTablaAsesores, MyConn, "select 0 sel, codven, concat(nombres,' ',apellidos) nombre " _
                                       & " from jsvencatven " _
                                       & " where " _
                                       & " estatus = 1 and tipo = " & TipoVendedor.iFuerzaventa & " and " _
                                       & " ID_EMP = '" & jytsistema.WorkID & "' order by codven ")

        Dim aCampos() As String = {"CODVEN.Asesor.50.C.", _
                                   "NOMBRE.Nombre.220.I."}

        ft.IniciarTablaPlus(dgAsesores, dtAsesores, aCampos, , True, New Font("Consolas", 7, FontStyle.Regular), False, , , False)
        Dim aCamposEdicion() As String = {"sel"}
        ft.EditarColumnasEnDataGridView(dgAsesores, aCamposEdicion)

    End Sub
    Private Sub jsVenProPrepedidosPedidos_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub

    Private Sub jsVenProPrepedidosPedidos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        Dim dates As SfDateTimeEdit() = {txtFecha, txtDesde, txtHasta}
        SetSizeDateObjects(dates)
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtFecha.Value)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFecha.Value)
        Me.Close()
    End Sub

    Private Function Validado() As Boolean

        If FechaUltimoBloqueo(MyConn, "jsvenencped") >= Convert.ToDateTime(txtFecha.Value) Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        Return True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Validado() Then

            Dim nTablaProceso As String = "tblProceso" & ft.NumeroAleatorio(100000)
            Dim dtProceso As New DataTable
            Dim strPro As String = ""


            Dim lvCont As Integer
            For lvCont = 0 To dgPedidos.Rows.Count - 1

                ProgressBar1.Value = (lvCont + 1) / dgPedidos.Rows.Count * 100

                With dgPedidos.Rows(lvCont)
                    Dim NumeroPrepedioAFacturar As String = .Cells("numped").Value

                    If CBool(.Cells("sel").Value) Then

                        lblProgreso.Text = " Prepedido No. " & NumeroPrepedioAFacturar

                        Select Case cmbGrupos.SelectedIndex
                            Case 0 'todos
                                strPro = " SELECT '00000' codigo, COUNT( * ) cuenta FROM jsvenrenpedrgv a  " _
                                    & " WHERE " _
                                    & " a.numped = '" & NumeroPrepedioAFacturar & "' AND " _
                                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                    & " GROUP BY codigo "
                            Case 1 'division
                                strPro = " SELECT IF( c.codigo IS NULL, '00000', c.codigo) codigo, IFNULL(COUNT(*),0) cuenta " _
                                    & " FROM jsvenrenpedrgv a " _
                                    & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                                    & " LEFT JOIN jsvengruposfacturacion c ON (b.division = c.codigo AND b.id_emp = c.id_emp AND c.agrupadopor = " & cmbGrupos.SelectedIndex & " )  " _
                                    & " WHERE " _
                                    & " a.numped = '" & NumeroPrepedioAFacturar & "' AND " _
                                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                    & " GROUP BY codigo "
                            Case 2 'jerarquia
                                strPro = " SELECT IF( c.codigo IS NULL, '00000', c.codigo) codigo, IFNULL(COUNT(*),0) cuenta " _
                                    & " FROM jsvenrenpedrgv a " _
                                    & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                                    & " LEFT JOIN jsvengruposfacturacion c ON (b.TIPJER = c.codigo AND b.id_emp = c.id_emp AND c.agrupadopor = " & cmbGrupos.SelectedIndex & " )  " _
                                    & " WHERE " _
                                    & " a.numped = '" & NumeroPrepedioAFacturar & "' AND " _
                                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                    & " GROUP BY codigo "
                            Case 3 'marca
                                strPro = " SELECT IF( c.codigo IS NULL, '00000', c.codigo) codigo, IFNULL(COUNT(*),0) cuenta " _
                                    & " FROM jsvenrenpedrgv a " _
                                    & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                                    & " LEFT JOIN jsvengruposfacturacion c ON (b.MARCA = c.codigo AND b.id_emp = c.id_emp AND c.agrupadopor = " & cmbGrupos.SelectedIndex & " )  " _
                                    & " WHERE " _
                                    & " a.numped = '" & NumeroPrepedioAFacturar & "' AND " _
                                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                                    & " GROUP BY codigo "
                            Case 4 'categoria
                                strPro = " SELECT IF( c.codigo IS NULL, '00000', c.codigo) codigo, IFNULL(COUNT(*),0) cuenta " _
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

                            Dim numeroRenglones As Integer = dtProceso.Rows(lcont).Item("cuenta")
                            Dim codRenglon As String = dtProceso.Rows(lcont).Item("codigo")
                            Dim CR_CO As String = .Cells("CONDPAG").Value
                            Dim nNum As Integer = -1
                            Dim numMaxRenglonesXPedido As Integer = ft.DevuelveScalarEntero(MyConn, " select venfacpa17 from jsconctapar where id_emp = '" & jytsistema.WorkID & "' ")

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
                                If ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsvenrenped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "'") > 0 Then _
                                                CrearEncabezadoPedido(NumeroPedido, NumeroPrepedioAFacturar, codRenglon, CR_CO)

                                nNum += numMaxRenglonesXPedido

                            End While

                        Next
                        'Cambiar Estatus de prepedido
                        ft.Ejecutar_strSQL(MyConn, " update jsvenencpedrgv set estatus = 1 where numped = '" & NumeroPrepedioAFacturar & "' and id_emp = '" & jytsistema.WorkID & "' ")


                    End If
                End With
            Next


            MsgBox("Proceso terminado...", MsgBoxStyle.Information, sModulo)

            ProgressBar1.Value = 0
            lblProgreso.Text = ""

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFecha.Value)

            dtProceso.Dispose()
            dtProceso = Nothing

            Me.Close()

        End If

    End Sub
    Private Sub VerificarFacturacionAPartirDePrecios(ByVal NumeroPrepedido As String, ByVal NumeroPedido As String)

        Dim CodigoClientePrepedido As String = ft.DevuelveScalarCadena(MyConn, " select codcli from jsvenencpedrgv where numped = '" & NumeroPrepedido & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim clienteFacturaAPartirDeCostos As Boolean = ft.DevuelveScalarBooleano(MyConn, " select codcre from jsvencatcli where codcli = '" & CodigoClientePrepedido & "' and id_emp = '" & jytsistema.WorkID & "' ")

        If clienteFacturaAPartirDeCostos Then
            Dim porcentajeGanancia As Double = ft.DevuelveScalarDoble(MyConn, " select des_cli from jsvencatcli where codcli = '" & CodigoClientePrepedido & "' and id_emp = '" & jytsistema.WorkID & "' ")

            ft.Ejecutar_strSQL(myconn, " UPDATE jsvenrenped a, jsmerctainv b " _
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

        Dim CodigoCliente As String = ft.DevuelveScalarCadena(MyConn, " select codcli from jsvenencpedrgv where numped = '" & NumeroPrepedido & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim DescuentoCliente As Double = ft.DevuelveScalarDoble(MyConn, " select des_cli from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim nVendedor As String = ft.DevuelveScalarCadena(MyConn, " select codven from jsvenencpedrgv where numped = '" & NumeroPrepedido & "' and id_emp = '" & jytsistema.WorkID & "' ")


        'Renglones
        Select Case TipoRenglon
            Case 0 'TODOS
                ft.Ejecutar_strSQL(myconn, " insert into jsvenrenped " _
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
                      & " b.precio_A > 0 AND " _
                      & " a.numped = '" & NumeroPrepedido & "' AND " _
                      & " a.cantran > 0 AND " _
                      & " a.id_emp = '" & jytsistema.WorkID & "' order by renglon limit " & renglonInicial & "," & renglonFinal & " ")

            Case 1 'DIVISION

                ft.Ejecutar_strSQL(myconn, " insert into jsvenrenped " _
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
                      & " b.PRECIO_A > 0 AND " _
                      & " a.numped = '" & NumeroPrepedido & "' AND " _
                      & " a.cantran > 0 AND " _
                      & " a.id_emp = '" & jytsistema.WorkID & "' order by renglon limit " & renglonInicial & "," & renglonFinal & " ")

            Case 2 'JERARQUIA

                ft.Ejecutar_strSQL(myconn, " insert into jsvenrenped " _
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
                      & " b.PRECIO_A > 0 AND " _
                      & " a.numped = '" & NumeroPrepedido & "' AND " _
                      & " a.cantran > 0 AND " _
                      & " a.id_emp = '" & jytsistema.WorkID & "' order by renglon limit " & renglonInicial & "," & renglonFinal & " ")
            Case 3 'MARCA

                ft.Ejecutar_strSQL(myconn, " insert into jsvenrenped " _
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
                      & " b.PRECIO_A > 0 AND " _
                      & " a.numped = '" & NumeroPrepedido & "' AND " _
                      & " a.cantran > 0 AND " _
                      & " a.id_emp = '" & jytsistema.WorkID & "' order by renglon limit " & renglonInicial & "," & renglonFinal & " ")
            Case 4 'CATEGORIA
                ft.Ejecutar_strSQL(myconn, " insert into jsvenrenped " _
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
                      & " b.PRECIO_A > 0 AND " _
                      & " a.numped = '" & NumeroPrepedido & "' AND " _
                      & " a.cantran > 0 AND " _
                      & " a.id_emp = '" & jytsistema.WorkID & "' order by renglon limit " & renglonInicial & "," & renglonFinal & " ")
        End Select

        'AJUSTAR POR CUOTA DE VENDEDORES
        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFVEPA02")) Then
            AjustarPorCuotaVendedor(MyConn, lblInfo, NumeroPedido, txtFecha.Value, nVendedor, "NUMPED", "jsvenrenped", "PED")
        End If


    End Sub
    Private Sub DescuentoGlobalEnPedido(ByVal NumeroPedido As String, ByVal NumeroPrepedido As String)

        'Tranfiere descuentos
        ft.Ejecutar_strSQL(myconn, " insert into jsvendesped " _
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

        CalculaTotalIVAVentas(MyConn, lblInfo, "jsvendesped", "jsvenivaped", "jsvenrenped", "numped", NumeroPedido,
                              "impiva", "totrendes", txtFecha.Value)

    End Sub
    Private Sub CrearEncabezadoPedido(ByVal NumeroPedido As String, ByVal NumeroPrePedido As String, ByVal codGrupo As String, Credito_Contado As String)

        Dim SubTotal As Double = CalculaTotalRenglonesVentas(MyConn, lblInfo, "jsvenrenped", "numped", "totren", NumeroPedido, 0)

        Dim Descuentos As Double = ft.DevuelveScalarDoble(MyConn, " select SUM(DESCUENTO) from jsvendesped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim Cargos As Double = CalculaTotalRenglonesVentas(MyConn, lblInfo, "jsvenrenped", "numped", "totren", NumeroPedido, 1)
        Dim ImpuestoIVA As Double = ft.DevuelveScalarDoble(MyConn, " select SUM(IMPIVA) from jsvenivaped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' group by numped ")
        Dim PorcentajeDescuento As Double = IIf(SubTotal <= 0.0, 0.0, (1 - (SubTotal - Descuentos) / SubTotal) * 100)
        Dim PesoTotal As Double = ft.DevuelveScalarDoble(MyConn, " select SUM(PESO) from jsvenrenped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' group by numped ")
        Dim numItems As Integer = ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsvenrenped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' group by numped ")

        Dim FechaEmision As String = ft.FormatoFechaMySQL(txtFecha.Value)
        Dim FechaEntrega As String = ft.FormatoFechaMySQL(DateAdd(DateInterval.Day, 1, txtFecha.Value.GetValueOrDefault()))
        Dim FechaVencimiento As String = ft.FormatoFechaMySQL(DateAdd(DateInterval.Day, 1, txtFecha.Value.GetValueOrDefault()))
        Dim CR_CO As Integer = IIf(Credito_Contado = "CR", 0, 1)

        ft.Ejecutar_strSQL(MyConn, " INSERT INTO jsvenencped " _
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
                       & " " & CR_CO & ", TIPOCREDITO, FORMAPAG, NOMPAG, NUMPAG, ABONO, SERIE, NUMGIRO, PERGIRO, INTERES, PORINT, IMPRESA, " _
                       & IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenencped", "block_date"), " '2009-01-01', ", "") _
                       & " EJERCICIO, ID_EMP " _
                       & " FROM jsvenencpedrgv " _
                       & " WHERE  " _
                       & " numped = '" & NumeroPrePedido & "' AND " _
                       & " id_emp = '" & jytsistema.WorkID & "' ")

    End Sub

    Private Sub PedidosAsesores()

        Dim iSel As Integer = 0
        strSQLAsesores = ""
        For Each selectedItem As DataRow In dtAsesores.Rows
            With selectedItem
                If CBool(.Item("sel")) Then
                    strSQLAsesores += " a.codven = '" & .Item("codven") & "' OR"
                    iSel += 1
                End If
            End With
        Next
        If iSel <= 0 Then strSQLAsesores = " a.codven = '' OR"

        AbrirPedidosPlus("( " & strSQLAsesores.Substring(0, strSQLAsesores.Length - 3) & " ) AND ")

    End Sub

    Private Sub AbrirPedidosPlus(ByVal strSQLAsesor As String)

        Dim tblCaja As String = "tblCaja" & ft.NumeroAleatorio(100000)
        Dim strTotren As String
        Dim aFields() As String = {"SEL.entero.1.0", "NUMPED.cadena.20.0", "EMISION.fecha.0.0", "COMEN.cadena.100.0", "CONDPAG.cadena.2.0",
                                   "ENTREGA.fecha.0.0", "TOT_PED.doble.19.2", "BACKORDER.doble.10.3", "KILOS.doble.10.3",
                                   "CODCLI.cadena.20.0", "NOMBRE.cadena.250.0", "DISPONIBLE.doble.19.2"}

        CrearTabla(MyConn, lblInfo, jytsistema.WorkDataBase, True, tblCaja, aFields)

        strTotren = "((b.cantran * b.Precio - b.cantran * b.Precio * b.des_cli / 100 - (b.cantran * b.Precio - b.cantran * b.Precio * b.des_cli / 100) * des_art / 100) - " _
           & " (b.cantran * b.Precio - b.cantran * b.Precio * b.des_cli / 100 - (b.cantran * b.Precio - b.cantran * b.Precio * b.des_cli / 100) * b.des_art / 100) * b.des_ofe / 100 ) * " _
           & " ( b.totrendes/b.totren ) "

        ft.Ejecutar_strSQL(MyConn, " insert into " & tblCaja _
           & " select 1 sel, a.NUMPED, a.EMISION, a.comen, ELT(a.condpag+1, 'CR','CO') CONDPAG,  a.ENTREGA, a.TOT_PED, " _
           & " sum( if( b.cantran > 0, " _
           & " if( b.iva = ' ', " & strTotren & " , " _
           & " if( b.iva = 'A', " & (1 + PorcentajeIVA(MyConn, lblInfo, txtFecha.Value, "A") / 100) & "*" & strTotren & " , " _
           & " if( b.iva = 'B', " & (1 + PorcentajeIVA(MyConn, lblInfo, txtFecha.Value, "B") / 100) & "*" & strTotren & " , " _
           & " if( b.iva = 'C', " & (1 + PorcentajeIVA(MyConn, lblInfo, txtFecha.Value, "C") / 100) & "*" & strTotren & " , " _
           & " if( b.iva = 'D', " & (1 + PorcentajeIVA(MyConn, lblInfo, txtFecha.Value, "D") / 100) & "*" & strTotren & ", " & strTotren & " )) ))) " _
           & " , 0)) as Backorder, " _
           & " sum(if(b.cantran > 0, b.peso/b.cantidad*b.cantran , 0 )) as kilos, c.codcli, c.nombre, c.disponible " _
           & " FROM jsvenencpedrgv a " _
           & " left join jsvenrenpedrgv b on (a.numped = b.numped and a.ejercicio = b.ejercicio and a.id_emp = b.id_emp) " _
           & " left join jsvencatcli c on (a.codcli = c.codcli and a.id_emp = c.id_emp) " _
           & " Where " & strSQLAsesor _
           & " a.EMISION >= '" & ft.FormatoFechaMySQL(txtDesde.Value) & "' AND " _
           & " a.EMISION <= '" & ft.FormatoFechaMySQL(txtHasta.Value) & "' AND " _
           & " a.ESTATUS = 0  and " _
           & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
           & " group by a.numped " _
           & " order by a.CODVEN, a.NUMPED ")

        dtPrepedidosX = ft.AbrirDataTable(ds, nTablaPrepedidosX, MyConn, " select * from " & tblCaja)

        Dim aCampos() As String = {"NUMPED.No Prepedido.90.I.", _
                                   "CODCLI.Cliente.60.I.", _
                                   "NOMBRE.Nombre ó Razón social.220.I.", _
                                   "EMISION.Emisión.60.C.fecha", _
                                   "TOT_PED.Total.80.D.Numero", _
                                   "KILOS.Peso (Kgr).80.D.Cantidad", _
                                   "COMEN.Grupo.50.C.", _
                                   "CONDPAG.CR/CO.30.C."}


        ft.IniciarTablaPlus(dgPedidos, dtPrepedidosX, aCampos, , True, New Font("Consolas", 7, FontStyle.Regular), False, , , False)
        Dim aCamposEdicion() As String = {"sel"}
        ft.EditarColumnasEnDataGridView(dgPedidos, aCamposEdicion)

        For Each Item As DataGridViewRow In dgPedidos.Rows
            With Item
                Dim lEstatus As Integer = EstatusClienteFacturacion(MyConn, .Cells("codcli").Value, Convert.ToDouble(.Cells("tot_ped").Value))
                Select Case lEstatus
                    Case 0 '
                        .DefaultCellStyle.BackColor = ColorVerdeClaro
                    Case 1
                        .DefaultCellStyle.BackColor = ColorAmarilloClaro
                    Case 2
                        .DefaultCellStyle.BackColor = ColorRojoClaro
                End Select

            End With
        Next

        CalcularTotales()


    End Sub

    Private Sub CalcularTotales()

        MontoTotal = 0.0
        KilosTotal = 0.0
        Items = 0
        strSQLPrepedidos = ""
        For Each selectedItem As DataRow In dtPrepedidosX.Rows
            With selectedItem
                If CBool(.Item("sel")) Then
                    MontoTotal += .Item("tot_ped")
                    KilosTotal += .Item("Kilos")
                    Items += 1
                    strSQLPrepedidos += " NUMPED = '" & .Item("NUMPED") & "' OR"
                End If
            End With


        Next
        If Items <= 0 Then strSQLPrepedidos = " NUMPED = '' OR"
        strSQLPrepedidos = RTrim(strSQLPrepedidos)

        txtItems.Text = ft.FormatoEntero(CInt(Items))
        txtTotalPrepedidos.Text = ft.FormatoNumero(MontoTotal)
        txtPesoPrepedidos.Text = ft.FormatoCantidad(KilosTotal)

        If strSQLPrepedidos <> "" Then AbrirPedidosKilos("( " & Mid(strSQLPrepedidos, 1, strSQLPrepedidos.Length - 3) & ") AND")
        If strSQLPrepedidos <> "" Then AbrirPedidosDescuentos("( " & Mid(strSQLPrepedidos, 1, strSQLPrepedidos.Length - 3) & ") AND")

    End Sub
    Private Sub AbrirPedidosKilos(ByVal strSQLPedidos As String)
        Dim strSQL As String = " SELECT a.numped, a.item, a.renglon, a.descrip, a.cantran, a.cantidad " _
                    & " FROM jsvenrenpedrgv a " _
                    & " LEFT JOIN jsmerctainv b on (a.item = b.codart and a.id_emp = b.id_emp) " _
                    & " WHERE " _
                    & strSQLPedidos _
                    & " b.unidad = 'KGR' and " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' order by a.numped "

        ds = DataSetRequeryPlus(ds, nTablaKilos, MyConn, strSQL)
        dtKilos = ds.Tables(nTablaKilos)

        Dim aCampos() As String = {"numped. N° Pedido.110.I.", _
                                   "item.Item.70.I.", _
                                   "renglon..40.C.", _
                                   "descrip.Descripción.360.I.", _
                                   "cantran.Kilos (KGR).50.D.Cantidad"}

        Dim lastCol As DataGridViewButtonColumn = New DataGridViewButtonColumn()
            lastCol.Name = "captcha"
            lastCol.HeaderText = "Capturar"
            lastCol.DataPropertyName = "captcha"
            lastCol.Width = 50
        ft.IniciarTablaPlus(dg, dtKilos, aCampos, True, True, New Font("Consolas", 8, FontStyle.Regular), False, , , , lastCol)


        Dim aEditar() As String = {"cantran"}
        ft.EditarColumnasEnDataGridView(dg, aEditar)


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
        Dim aAnchos() As Integer = {110, 70, 50, 360, 70, 40, 70, 60, 90}
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

            ft.Ejecutar_strSQL(myconn, " update jsvenrenpedrgv set cantran = " & CDbl(dg.CurrentCell.Value) & " " _
                            & " where " _
                            & " numped = '" & CStr(dg.CurrentRow.Cells(0).Value) & "' and " _
                            & " item = '" & CStr(dg.CurrentRow.Cells(1).Value) & "' and " _
                            & " renglon = '" & CStr(dg.CurrentRow.Cells(2).Value) & "' and " _
                            & " id_emp = '" & jytsistema.WorkID & "' ")

        End If
    End Sub

    Private Sub dgDescuentos_CellValidated(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgDescuentos.CellValidated
        If dgDescuentos.CurrentCell.ColumnIndex = 7 Then

            ft.Ejecutar_strSQL(myconn, " update jsvenrenpedrgv set des_art = " & CDbl(dgDescuentos.CurrentCell.Value) & " " _
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
            ft.mensajeAdvertencia("Debe indicar dígito(s) válido...")
            e.Cancel = True
        End If

        If Not ft.isNumeric(e.FormattedValue.ToString()) Then
            ft.mensajeAdvertencia("Debe indicar un número válido...")
            e.Cancel = True
        End If

    End Sub

    Private Sub dgDescuentos_CellValidating(ByVal sender As Object, ByVal e As DataGridViewCellValidatingEventArgs) _
           Handles dgDescuentos.CellValidating

        Dim headerText As String = _
            dgDescuentos.Columns(e.ColumnIndex).HeaderText

        If Not headerText.Equals("Dscto.") Then Return

        If (String.IsNullOrEmpty(e.FormattedValue.ToString())) Then
            ft.mensajeAdvertencia("Debe indicar dígito(s) válido...")
            e.Cancel = True
        End If

        If Not ft.isNumeric(e.FormattedValue.ToString()) Then
            ft.mensajeAdvertencia("Debe indicar un número válido...")
            e.Cancel = True
        End If

    End Sub

    Private Sub txtItemsFactura_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtItemsFactura.TextChanged
        ft.Ejecutar_strSQL(myconn, " update jsconctapar set venfacpa17 = " & txtItemsFactura.Text & " where id_emp = '" & jytsistema.WorkID & "' ")
        ft.Ejecutar_strSQL(myconn, " update jsconparametros set valor = " & txtItemsFactura.Text _
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
            ft.Ejecutar_strSQL(myconn, " REPLACE INTO jsvengruposfacturacion set agrupadopor = " & cmbGrupos.SelectedIndex & ", CODIGO = '" & codSelected & "', ID_EMP = '" & jytsistema.WorkID & "' ")
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
            Dim aAnchos() As Integer = {40, 100}
            Dim aAlineacion() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}

            Dim aFormatos() As String = {"", ""}
            IniciarTabla(dgGrupos, dtGRupos, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, False, , 8, False)
        Else
            dgGrupos.Columns.Clear()
            dg.DataSource = Nothing
        End If

    End Sub

    Private Sub dg_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dg.CellContentClick
        Dim senderGrid = DirectCast(sender, DataGridView)
        If TypeOf senderGrid.Columns(e.ColumnIndex) Is DataGridViewButtonColumn AndAlso e.RowIndex >= 0 Then
            ft.mensajeInformativo("Por favor configure puerto serial para captura de Peso")
        End If
    End Sub

   
    Private Sub dgPedidos_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgPedidos.CellContentClick
        If e.ColumnIndex = 0 Then dtPrepedidosX.Rows(e.RowIndex).Item(0) = Not CBool(dtPrepedidosX.Rows(e.RowIndex).Item(0).ToString)
        If e.ColumnIndex = 8 And e.RowIndex >= 0 Then
            With dtPrepedidosX.Rows(e.RowIndex).Item("CONDPAG")
                If .ToString.Equals("CR") Then
                    dtPrepedidosX.Rows(e.RowIndex).Item("CONDPAG") = "CO"
                Else
                    dtPrepedidosX.Rows(e.RowIndex).Item("CONDPAG") = "CR"
                End If
            End With
        End If

        CalcularTotales()

    End Sub

    Private Sub dgAsesores_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgAsesores.CellContentClick
        If e.ColumnIndex = 0 Then dtAsesores.Rows(e.RowIndex).Item(0) = Not CBool(dtAsesores.Rows(e.RowIndex).Item(0).ToString)
        PedidosAsesores()
    End Sub
End Class