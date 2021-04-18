Imports MySql.Data.MySqlClient
Public Class jsVenProPedidosFacturacion

    Private Const sModulo As String = "Procesar pedidos a facturación"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dtAsesores As New DataTable
    Private dtPedidos As New DataTable
    Private ft As New Transportables

    Private n_Apuntador As Long

    Private strSQLAsesores As String = ""
    Private strSQLAsesor As String = ""
    Private strSQLPedidos As String = ""

    Private nTablaAsesores As String = "tblAsesores"
    Private nTablaPedidos As String = "tbpedidos"

    Private m_SortingColumn As ColumnHeader

    Private MontoTotal As Double
    Private KilosTotal As Double
    Private Items As Integer = 0

    Private aMetodo() As String = {"Asignaciones completas", "Asignaciones parciales", "Asignaciones parciales por histórico", _
                                   "Asignaciones parciales por cuota asesor"}

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
        Me.Show()

    End Sub
    Private Sub IniciarTXT()

        ft.habilitarObjetos(False, True, txtFecha, txtDesde, txtHasta, txtItems, txtPesoPedidos, txtTotalpedidos)

        ft.iniciarTextoObjetos(Transportables.tipoDato.Fecha, txtFecha, txtDesde, txtHasta)
        ft.RellenaCombo(aMetodo, cmbMetodo)

        IniciarCajas()
        IniciarAlmacen()
        IniciarTransporte()

        txtItems.Text = ft.muestraCampoEntero(0)
        txtPesoPedidos.Text = ft.muestraCampoCantidad(0.0)
        txtTotalpedidos.Text = ft.muestraCampoNumero(0.0)

        CargarAsesores()

        strSQLAsesores = ""
        dgPedidos.Columns.Clear()

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
   
    Private Sub IniciarAlmacen()

        RellenaComboConDatatable(cmbAlmacen, ft.AbrirDataTable(ds, "tblAlmacen", MyConn, " select * from jsmercatalm " _
                                                               & " where " _
                                                               & " id_emp = '" & jytsistema.WorkID & "' order by codalm "), "codalm", "codalm")
    End Sub
    Private Sub IniciarTransporte()
     
        RellenaComboConDatatable(cmbTransporte, ft.AbrirDataTable(ds, "tblTransporte", MyConn, " select * from jsconctatra " _
                                                                  & " where " _
                                                                  & " id_emp = '" & jytsistema.WorkID & "' order by codtra "), "codtra", "codtra")

    End Sub
    Private Sub IniciarCajas()
        RellenaComboConDatatable(cmbCaja, ft.AbrirDataTable(ds, "tblCaja", MyConn, " select * from jsbanenccaj where id_emp = '" & jytsistema.WorkID & "' order by caja "), "caja", "caja")
    End Sub

    Private Sub jsVenProPedidosFacturacion_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ft = Nothing
    End Sub
    Private Sub jsVenProPedidosFacturacion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Tag = sModulo
        InsertarAuditoria(MyConn, MovAud.ientrar, sModulo, txtFecha.Text)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFecha.Text)
        Me.Close()
    End Sub

    Private Function Validado() As Boolean

        If FechaUltimoBloqueo(MyConn, "jsvenencfac") >= Convert.ToDateTime(txtFecha.Text) Then
            ft.mensajeCritico("FECHA MENOR QUE ULTIMA FECHA DE CIERRE...")
            Return False
        End If

        Dim CapacidadCamion As Double = ft.DevuelveScalarDoble(MyConn, " select capacidad from jsconctatra where codtra = '" & cmbTransporte.SelectedValue & "' and id_emp = '" & jytsistema.WorkID & "'  ")

        If ValorNumero(txtPesoPedidos.Text) > CapacidadCamion Then
            ft.mensajeCritico("El peso total de los pedidos escogidos excede a la capacidad de carga del transporte escogido... ")
        End If

        Return True

    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If Validado() Then

            For lvCont As Integer = 0 To dgPedidos.Rows.Count - 1
                ProgressBar1.Value = (lvCont + 1) / dgPedidos.Rows.Count * 100
                With dgPedidos.Rows(lvCont)

                    If CBool(.Cells("sel").Value) Then

                        Dim PermiteFacturar As Boolean = False
                        Dim CodigoCliente As String = .Cells("CODCLI").Value
                        Dim dDisponibilidadCliente As Double = ft.DevuelveScalarDoble(MyConn, _
                                                                " select disponible from jsvencatcli where codcli = '" _
                                                                & CodigoCliente & _
                                                                "' and id_emp = '" & jytsistema.WorkID & "' ")

                        Dim iComision As Integer = ft.DevuelveScalarEntero(MyConn, _
                                                                " select backorder from jsvencatcli where codcli = '" _
                                                                & CodigoCliente & _
                                                                "' and id_emp = '" & jytsistema.WorkID & "' ")

                        Dim dMontoPeddido As Double = ValorNumero(.Cells("TOT_PED").Value)

                        lblProgreso.Text = " Pedido No. " & .Cells("NUMPED").Value

                        '-1.- VERIFICA SI EL MONTO PEDIDO ES MAYOR A CERO
                        If dMontoPeddido > 0.0 Then

                            '0.- VERIFICACIÓN DE CHEQUES DEVUELTOS DE CLIENTE
                            If PoseeChequesDevueltosSinCancelar(MyConn, lblInfo, CodigoCliente) Then
                                If iComision = 1 Then PermiteFacturar = True
                            Else
                                PermiteFacturar = True
                            End If

                            '1.1.- VERIFICACION DE CHEQUES FECHADOS A FUTUR0
                            If Not CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM19")) Then
                                If ClientePoseeChequesFuturos(MyConn, lblInfo, CodigoCliente) Then PermiteFacturar = False
                            End If
                            '1.2.- VERIFICACION DE ESTATUS DE CLIENTE
                            If Not CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM37")) Then
                                Dim EstatusCliente As Integer = ft.DevuelveScalarEntero(MyConn, " select estatus from jsvencatcli " _
                                                                                & " where " _
                                                                                & " codcli = '" & CodigoCliente & "' and " _
                                                                                & " id_emp = '" & jytsistema.WorkID & "' ")
                                If EstatusCliente >= 1 Then PermiteFacturar = False
                            End If


                            If PermiteFacturar Then

                                '2.- VERIFICA DISPONIBILDAD DE CLIENTE
                                If dDisponibilidadCliente >= dMontoPeddido Then

                                    Dim NumeroFacturaProvisional As String = "FCTMP" & ft.NumeroAleatorio(1000000)

                                    '3.- TRANSFERIR RENGLONES EN PEDIDOS
                                    TransferirRenglones(MyConn, .Cells("NUMPED").Value, NumeroFacturaProvisional, cmbAlmacen.SelectedValue)

                                    'INCLUSION DE FLETES AUTOMATICOS
                                    IncluirFleteAutomatico(CodigoCliente, montoFletesPorRegion(MyConn, CodigoCliente), NumeroFacturaProvisional)

                                    'Transfiere y calcula Descuentos Globales o de factura
                                    DescuentoEnFactura(NumeroFacturaProvisional, .Cells("NUMPED").Value)

                                    'VERIFICAR PRECIOS DE FACTURACION
                                    VerificarFacturacionAPartirDePrecios(.Cells("NUMPED").Value, NumeroFacturaProvisional)

                                    'Calcular IVA
                                    CalcularIVAFactura(NumeroFacturaProvisional)

                                    'Crear Encabezado factura
                                    If ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsvenrenfac where numfac = '" & NumeroFacturaProvisional & "' and id_emp = '" & jytsistema.WorkID & "'") > 0 Then

                                        'ACTUALIZA TABLAS FACTURA CON EL NUMERO DE FACTURA REAL
                                        Dim NumeroFactura As String = Contador(MyConn, lblInfo, Gestion.iVentas, "VENNUMFAC", "8")
                                        ft.Ejecutar_strSQL(MyConn, " update jsvenrenfac set numfac = '" & NumeroFactura & "' where numfac = '" & NumeroFacturaProvisional & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                        ft.Ejecutar_strSQL(MyConn, " update jsvenivafac set numfac = '" & NumeroFactura & "' where numfac = '" & NumeroFacturaProvisional & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                        ft.Ejecutar_strSQL(MyConn, " update jsvendesfac set numfac = '" & NumeroFactura & "' where numfac = '" & NumeroFacturaProvisional & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                        ft.Ejecutar_strSQL(MyConn, " update jsvenrencom set numdoc = '" & NumeroFactura & "' where numdoc = '" & NumeroFacturaProvisional & "' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ")
                                        ft.Ejecutar_strSQL(MyConn, " update jsvenforpag set numfac = '" & NumeroFactura & "' where numfac = '" & NumeroFacturaProvisional & "' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ")
                                        ft.Ejecutar_strSQL(MyConn, " update jsmertramer set numdoc = '" & NumeroFactura & "', numorg = '" & NumeroFactura & "' where numdoc = '" & NumeroFacturaProvisional & "' and id_emp = '" & jytsistema.WorkID & "' ")

                                        'ENCABEZADO DE FACTURA
                                        CrearEncabezadoFactura(NumeroFactura, .Cells("NUMPED").Value, .Cells("CONDPAG").Value)

                                        CalcularIVAFactura(NumeroFactura)

                                        'Actualizar CXC, Caja y/o Bancos
                                        ActualizarCXCCajaBancos(MyConn, lblInfo, NumeroFactura)

                                        'Cambiar Estatus de pedido
                                        Dim elEstatus As Double = ft.DevuelveScalarDoble(MyConn, "select sum(cantran) from jsvenrenped where numped = '" & .Cells("NUMPED").Value & "' and id_emp = '" & jytsistema.WorkID & "' group by numped ")
                                        If elEstatus = 0.0 Then ft.Ejecutar_strSQL(MyConn, " update jsvenencped set estatus = 1 where numped = '" & .Cells("NUMPED").Value & "' and id_emp = '" & jytsistema.WorkID & "' ")

                                    End If

                                End If

                            End If

                        End If

                    End If

                End With

            Next
            MsgBox("Proceso terminado...", MsgBoxStyle.Information, sModulo)

            ProgressBar1.Value = 0
            lblProgreso.Text = ""

            InsertarAuditoria(MyConn, MovAud.iSalir, sModulo, txtFecha.Text)
            Me.Close()

        End If

    End Sub

    Private Sub IncluirFleteAutomatico(Codigocliente As String, montoFlete As Double, DocumentoNumero As String)

        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA24")) Then

            Dim codServicioFlete As String = CStr(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA25"))
            Dim monServicioFlete As Double = montoFlete
            If monServicioFlete = 0 Then
                monServicioFlete = ft.DevuelveScalarDoble(MyConn, " select precio from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'")
            End If

            Dim desServicioFlete As String = ft.DevuelveScalarCadena(MyConn, " select desser from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'")
            Dim ivaServicioFlete As String = ft.DevuelveScalarCadena(MyConn, " select tipoiva from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'")

            Dim numRenglon As String = ft.autoCodigo(MyConn, "renglon", "jsvenrenfac", "numfac.id_emp", DocumentoNumero + "." + jytsistema.WorkID, 5)

            If monServicioFlete <> 0 Then

                InsertEditVENTASRenglonFactura(MyConn, lblInfo, True, DocumentoNumero, numRenglon, "$" & codServicioFlete, desServicioFlete, _
                                                    ivaServicioFlete, "", "UND", 0.0, 1.0, _
                                                    0.0, 0.0, 0, "", "", 0.0, "", "1", _
                                                    monServicioFlete, 0.0, 0.0, _
                                                    0.0, monServicioFlete, monServicioFlete, _
                                                    "", "", "", "", "", "", _
                                                    "", "", "", "1", 1)



            End If


        End If

    End Sub

    Private Sub VerificarFacturacionAPartirDePrecios(ByVal NumeroPedido As String, ByVal NumeroFactura As String)

        Dim CodigoClientePrepedido As String = ft.DevuelveScalarCadena(MyConn, " select codcli from jsvenencped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim clienteFacturaAPartirDeCostos As Boolean = ft.DevuelveScalarBooleano(MyConn, " select codcre from jsvencatcli where codcli = '" & CodigoClientePrepedido & "' and id_emp = '" & jytsistema.WorkID & "' ")

        If clienteFacturaAPartirDeCostos Then
            Dim porcentajeGanancia As Double = ft.DevuelveScalarDoble(MyConn, " select des_cli from jsvencatcli where codcli = '" & CodigoClientePrepedido & "' and id_emp = '" & jytsistema.WorkID & "' ")

            ft.Ejecutar_strSQL(MyConn, " UPDATE jsvenrenfac a, jsmerctainv b " _
                            & " SET a.precio = IF ( " & porcentajeGanancia & " = 0, b.PRECIO_A, ROUND(b.montoultimacompra*(1+" & porcentajeGanancia & "/100),2) ), " _
                            & " des_cli = 0.00, des_art = 0.00, des_ofe = 0.00, " _
                            & " a.totren = a.cantidad*ROUND(b.montoultimacompra*(1+" & porcentajeGanancia & "/100),2), " _
                            & " a.totrendes = a.cantidad*ROUND(b.montoultimacompra * (1+" & porcentajeGanancia & "/ 100), 2) " _
                            & " WHERE " _
                            & " a.item = b.codart AND " _
                            & " a.id_emp = b.id_emp AND " _
                            & " a.numfac = '" & NumeroPedido & "' AND " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' ")

        End If


    End Sub

    Private Sub ActualizarCXCCajaBancos(MyConn As MySqlConnection, lblInfo As Label, numFactura As String)


        Dim tblEncFac As String = "tbl" & numFactura
        Dim dtFac As DataTable

        ds = DataSetRequery(ds, " select * from jsvenencfac where numfac = '" & numFactura & "' and id_emp = '" & jytsistema.WorkID & "'  ", _
                             MyConn, tblEncFac, lblInfo)
        dtFac = ds.Tables(tblEncFac)

        Dim strTipo As String = "FC"
        Dim strTipo1 As String = "FACTURA N° "
        Dim sFOTipo As String = "0"
        Dim sSigno As Integer = 1

        With dtFac.Rows(0)
            InsertEditVENTASCXC(MyConn, lblInfo, True, .Item("codcli"), strTipo, numFactura, CDate(.Item("emision").ToString), ft.FormatoHora(Now()), _
                CDate(.Item("vence").ToString), "", strTipo1 & ": " & numFactura, sSigno * .Item("tot_fac"), .Item("imp_iva"), _
                .Item("formapag"), .Item("caja"), .Item("numpag"), .Item("nompag"), "", "FAC", numFactura, _
                "0", "", jytsistema.sFechadeTrabajo, "", "", "", 0.0#, 0.0#, "", "", "", "", .Item("codven"), _
                .Item("codven"), "0", sFOTipo, jytsistema.WorkDivition)

            SaldoCxC(MyConn, lblInfo, .Item("codcli"))
        End With

        dtFac.Dispose()
        dtFac = Nothing

    End Sub
    Private Sub TransferirRenglonesPlus(MyConn As MySqlConnection, NumeroPedido As String, NumeroFactura As String, Almacen As String)

        Dim dtRenglonesPedido As DataTable = ft.AbrirDataTable(ds, "ntablRenglonesPedidos", MyConn, _
                    " SELECT NUMPED, RENGLON, ITEM, DESCRIP, IVA, ICS, UNIDAD, BULTOS, cantran cantidad, " _
                    & " INVENTARIO, SUGERIDO, REFUERZO, PRECIO, " _
                    & " peso/cantidad*cantran peso, LOTE, '','', ESTATUS, DES_CLI, DES_ART, DES_OFE, ROUND((precio*cantran - precio*cantran*des_cli/100 - (precio*cantran - precio*cantran*des_cli/100)*des_art/100 - " _
                    & " (precio*cantran - precio*cantran*des_cli/100 - (precio*cantran - precio*cantran*des_cli/100)*des_art/100)*des_ofe/100) , 2) " _
                    & " TOTREN, " _
                    & " ROUND((precio*cantran - precio*cantran*des_cli/100 - (precio*cantran - precio*cantran*des_cli/100)*des_art/100 - " _
                    & " (precio*cantran - precio*cantran*des_cli/100 - (precio*cantran - precio*cantran*des_cli/100)*des_art/100)*des_ofe/100) , 2) " _
                    & " TOTRENDES, NUMCOT, RENCOT, '" & NumeroPedido & "', RENGLON, '', '', CODCON, '', '',  ACEPTADO, EDITABLE, EJERCICIO, ID_EMP " _
                    & " FROM jsvenrenped " _
                    & " WHERE " _
                    & " estatus < 2 and " _
                    & " cantran > 0 AND " _
                    & " numped = '" & NumeroPedido & "' AND " _
                    & " id_emp = '" & jytsistema.WorkID & "'")

        Dim nRenglon As String = ""
        Dim nCantidad As Double = 0.0
        Dim nPeso As Double = 0.0
        Dim nPrecio As Double = 0.0
        Dim nTotalRenglon As Double = 0.0
        Dim nTotalRenglonDescuento As Double = 0.0
        For Each nRow As DataRow In dtRenglonesPedido.Rows
            With nRow

                nRenglon = .Item("renglon")
                nCantidad = .Item("cantran")
                nPeso = .Item("peso") / .Item("cantidad") * .Item("cantran")
                If .Item("PRECIO") = 0.0 Then
                    Dim Equivalencia As Double = FuncionesMercancias.Equivalencia(myConn,  .Item("ITEM"), .Item("UNIDAD"))
                    nPrecio = ft.DevuelveScalarDoble(MyConn, " select precio_a from jsmerctainv where codart = '" & .Item("ITEM") _
                                                     & "' and id_emp = '" & jytsistema.WorkID & "' ")

                    If nPrecio = 0.0 Then _
                        nPrecio = ft.DevuelveScalarDoble(MyConn, " select montoultimacompra from jsmerctainv where codart = '" & .Item("ITEM") & "' and id_emp = '" & jytsistema.WorkID & "' ")

                    nPrecio = Math.Round(nPrecio / Equivalencia, 2)
                Else
                    nPrecio = .Item("PRECIO")
                End If

                Dim nDescuentoCliente As Double = nPrecio * nCantidad * .Item("des_cli") / 100
                Dim nDescuentoArticulo As Double = (nPrecio * nCantidad - nDescuentoCliente) * .Item("des_art") / 100
                Dim nDescuentoOferta As Double = (nPrecio * nCantidad - nDescuentoCliente - nDescuentoArticulo) * .Item("des_ofe") / 100

                nTotalRenglon = nPrecio * nCantidad - nDescuentoCliente - nDescuentoArticulo - nDescuentoOferta

                InsertEditVENTASRenglonFactura(MyConn, lblInfo, True, NumeroFactura, nRenglon, .Item("item"), .Item("DESCRIP"), .Item("IVA"), "", .Item("UNIDAD"), _
                                            0.0, nCantidad, 0.0, 0.0, 0, "", "", nPeso, .Item("LOTE"), .Item("ESTATUS"), nPrecio, .Item("DES_CLI"), .Item("DES_ART"), .Item("DES_OFE"), _
                                            nTotalRenglon, nTotalRenglon, .Item("NUMCOT"), .Item("RENCOT"), .Item("NUMPED"), .Item("RENGLON"), "", "", _
                                            "", "", "", .Item("ACEPTADO"), .Item("EDITABLE"))
            End With
        Next

    End Sub

    Private Sub TransferirRenglones(ByVal MyConn As MySqlConnection, ByVal NumeroPedido As String, ByVal NumeroFactura As String, _
                                    ByVal Almacen As String)

        'INSERTAR Renglones DESDE EL PEDIDO CON LA CANTIDAD EN TRANSITO

        Dim nCliente As String = ft.DevuelveScalarCadena(MyConn, " SELECT codcli from jsvenencped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim nVendedor As String = ft.DevuelveScalarCadena(MyConn, "SELECT codven from jsvenencped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' ")

        ft.Ejecutar_strSQL(MyConn, " insert into jsvenrenfac " _
                    & " SELECT '" & NumeroFactura & "' NUMPED, RENGLON, ITEM, DESCRIP, IVA, ICS, UNIDAD, BULTOS, cantran cantidad, " _
                    & " INVENTARIO, SUGERIDO, REFUERZO, PRECIO, " _
                    & " peso/cantidad*cantran peso, LOTE, '','', ESTATUS, DES_CLI, DES_ART, DES_OFE, ROUND((precio*cantran - precio*cantran*des_cli/100 - (precio*cantran - precio*cantran*des_cli/100)*des_art/100 - " _
                    & " (precio*cantran - precio*cantran*des_cli/100 - (precio*cantran - precio*cantran*des_cli/100)*des_art/100)*des_ofe/100) , 2) " _
                    & " TOTREN, " _
                    & " ROUND((precio*cantran - precio*cantran*des_cli/100 - (precio*cantran - precio*cantran*des_cli/100)*des_art/100 - " _
                    & " (precio*cantran - precio*cantran*des_cli/100 - (precio*cantran - precio*cantran*des_cli/100)*des_art/100)*des_ofe/100) , 2) " _
                    & " TOTRENDES, NUMCOT, RENCOT, '" & NumeroPedido & "', RENGLON, '', '', CODCON, '', '',  ACEPTADO, EDITABLE, EJERCICIO, ID_EMP " _
                    & " FROM jsvenrenped " _
                    & " WHERE " _
                    & " estatus < 2 and " _
                    & " cantran > 0 AND " _
                    & " numped = '" & NumeroPedido & "' AND " _
                    & " id_emp = '" & jytsistema.WorkID & "'")

        'INSERTAR comentarios
        ft.Ejecutar_strSQL(myconn, " insert into jsvenrencom " _
                       & " SELECT '" & NumeroFactura & "' numdoc, 'FAC' origen, item, renglon, comentario, id_emp " _
                       & " FROM jsvenrencom " _
                       & " WHERE " _
                       & " numdoc = '" & NumeroPedido & "' AND " _
                       & " origen = 'PED' AND " _
                       & " id_emp = '" & jytsistema.WorkID & "'")


        'CALCULA BONIFICACIONES
        CalculaBonificaciones(MyConn, lblInfo, ModuloBonificacion.iFactura, NumeroFactura, CDate(txtFecha.Text), _
                              ft.DevuelveScalarCadena(MyConn, "SELECT tarifa from jsvenencped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' "))

        'AJUSTAR POR EXISTENCIAS
        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA12")) Then
            AjustarExistencias(MyConn, NumeroFactura, lblInfo, Almacen, "NUMFAC", "jsvenrenfac", "FAC")
        End If

        'AJUSTAR POR CUOTA DE VENDEDORES
        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFVEPA02")) Then
            AjustarPorCuotaVendedor(MyConn, lblInfo, NumeroFactura, CDate(txtFecha.Text), nVendedor, "NUMFAC", "jsvenrenfac", "FAC")
        End If


        'ACTUALIZA MOVIMIENTOS DE INVENTARIO Y CANTIDAD EN TRANSITO EN PEDIDOS
        Dim dtFacMOV As DataTable
        Dim nTableFacMOV As String = "tblfac" & NumeroFactura
        ds = DataSetRequery(ds, " select * from jsvenrenfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' order by renglon  ", MyConn, nTableFacMOV, lblInfo)
        dtFacMOV = ds.Tables(nTableFacMOV)

        If dtFacMOV.Rows.Count > 0 Then
            Dim JCont As Integer
            For JCont = 0 To dtFacMOV.Rows.Count - 1
                With dtFacMOV.Rows(JCont)

                    'ACTUALIZANDO MOVIMIENTO DE INVENTARIO
                    Dim CostoItem As Double = IIf(Mid(.Item("item"), 1, 1) = "$", 0, ft.DevuelveScalarDoble(MyConn, " select montoultimacompra from jsmerctainv where codart = '" & .Item("item") & "' and id_emp = '" & jytsistema.WorkID & "' "))

                    Dim Equivale As Double = Equivalencia(myConn,  .Item("ITEM"), .Item("UNIDAD"))

                    InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, .Item("item"), CDate(txtFecha.Text), _
                                       "SA", NumeroFactura, .Item("UNIDAD"), _
                                       .Item("CANTIDAD"), .Item("peso"), CostoItem * .Item("CANTIDAD") / IIf(Equivale = 0.0, 1, Equivale), _
                                       CostoItem * .Item("CANTIDAD") / IIf(Equivale = 0.0, 1, Equivale), _
                                       "FAC", NumeroFactura, "", nCliente, .Item("TOTREN"), .Item("TOTRENDES"), 0.0, 0.0, nVendedor, Almacen, _
                                       .Item("RENGLON"), jytsistema.sFechadeTrabajo)


                    'ACTUALIZANDO CANTIDAD EN TRANSITO EN PEDIDOS
                    ft.Ejecutar_strSQL(myconn, " update jsvenrenped set " _
                                               & " CANTRAN = CANTRAN - " & .Item("CANTIDAD") & " " _
                                               & " Where " _
                                               & " numPED = '" & NumeroPedido & "' and " _
                                               & " item = '" & .Item("item") & "' and " _
                                               & " renglon = '" & .Item("renglon") & "' and " _
                                               & " estatus = '" & .Item("estatus") & "' and " _
                                               & " id_emp = '" & jytsistema.WorkID & "' ")



                End With
            Next
        End If
        dtFacMOV.Dispose()
        dtFacMOV = Nothing


    End Sub

   

    Private Sub DescuentoEnFactura(ByVal NumeroFactura As String, ByVal NumeroPedido As String)

        'Tranfiere descuentos
        ft.Ejecutar_strSQL(myconn, " insert into jsvendesfac " _
                        & " SELECT '" & NumeroFactura & "' numped, renglon, descrip, pordes, descuento, subtotal, aceptado, ejercicio, id_emp " _
                        & " FROM jsvendesped " _
                        & " WHERE " _
                        & " numped = '" & NumeroPedido & "' AND " _
                        & " id_emp = '" & jytsistema.WorkID & "' ")

        'Recalcula descuentos
        Dim dtDescuentos As DataTable
        Dim nTablaDesc As String = "tblDescuentos"
        ds = DataSetRequery(ds, " select * from jsvendesfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaDesc, lblInfo)
        dtDescuentos = ds.Tables(nTablaDesc)
        Dim MontoInicial As Double = CalculaTotalRenglonesVentas(MyConn, lblInfo, "jsvenrenfac", "numfac", "totren", NumeroFactura, 0)
        ActualizarDescuentoVentas(MyConn, dtDescuentos, "jsvendesfac", "numfac", MontoInicial, lblInfo)

        Dim TotalDescuento As Double = ft.DevuelveScalarDoble(MyConn, "  SELECT SUM(DESCUENTO) FROM jsvendesfac WHERE numfac = '" & NumeroFactura & "'  AND id_emp = '" & jytsistema.WorkID & "' group by numfac ")

        '//////Reparte la porcion del descuento general en los renglones de factura

        If ft.DevuelveScalarEntero(MyConn, " SELECT count(*) " _
                                 & " FROM jsvenrenfac a " _
                                 & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                                 & " WHERE " _
                                 & " a.estatus = 0 AND " _
                                 & " b.descuento = 1 AND " _
                                 & " b.regulado = 0 AND " _
                                 & " a.numfac = '" & NumeroFactura & "'  AND " _
                                 & " a.id_emp = '" & jytsistema.WorkID & "' GROUP BY NUMFAC") > 0 Then

            ft.Ejecutar_strSQL(MyConn, " UPDATE jsvenrenfac a, (SELECT a.numfac, a.renglon, a.item, a.totren, " _
                    & " IF  (  (a.totren - ROUND(a.totren*" & TotalDescuento & "/d.subtotal,2)) IS NULL, 0.00, " _
                    & " (a.totren - ROUND(a.totren*" & TotalDescuento & "/d.subtotal,2))  ) totrendes, a.id_emp " _
                    & " FROM jsvenrenfac a " _
                    & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp)" _
                    & " LEFT JOIN ( SELECT a.numfac, SUM(a.totren) subtotal " _
                    & "             FROM jsvenrenfac a " _
                    & "             LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                    & "             Where " _
                    & "             a.estatus = 0 and " _
                    & "             b.descuento = 1 AND " _
                    & "             b.regulado = 0 AND " _
                    & "             a.numfac = '" & NumeroFactura & "'  AND " _
                    & "             a.id_emp = '" & jytsistema.WorkID & "') d ON (d.numfac = a.numfac) " _
                    & " Where " _
                    & " b.descuento = 1 AND " _
                    & " b.regulado = 0 AND " _
                    & " a.estatus = 0 and " _
                    & " a.numfac = '" & NumeroFactura & "' AND " _
                    & " a.id_emp = '" & jytsistema.WorkID & "') b " _
                    & " Set a.totrendes = b.totrendes " _
                    & " Where " _
                    & " a.numfac = b.numfac AND " _
                    & " a.renglon = b.renglon and " _
                    & " a.item = b.item AND " _
                    & " a.id_emp = b.id_emp ")


        End If


        dtDescuentos.Dispose()
        dtDescuentos = Nothing

    End Sub

    Private Sub CalcularIVAFactura(ByVal NumeroFactura As String)

        CalculaTotalIVAVentas(MyConn, lblInfo, "jsvendesfac", "jsvenivafac", "jsvenrenfac", "numfac", NumeroFactura, _
                              "impiva", "totrendes", CDate(txtFecha.Text))

    End Sub
    Private Sub CrearEncabezadoFactura(ByVal NumeroFactura As String, ByVal NumeroPedido As String, Credito_Contado As String)

        Dim dtEncabPed As DataTable
        Dim nTablaEncabPED As String = "tblEncabPed"

        ds = DataSetRequery(ds, " select * from jsvenencped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaEncabPED, lblInfo)
        dtEncabPed = ds.Tables(nTablaEncabPED)

        With dtEncabPed.Rows(0)
            Dim SubTotal As Double = CalculaTotalRenglonesVentas(MyConn, lblInfo, "jsvenrenfac", "numfac", "totren", NumeroFactura, 0)
            Dim Descuentos As Double = ft.DevuelveScalarDoble(MyConn, " select SUM(DESCUENTO) from jsvendesfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' ")
            Dim Cargos As Double = CalculaTotalRenglonesVentas(MyConn, lblInfo, "jsvenrenfac", "numfac", "totren", NumeroFactura, 1)
            Dim ImpuestoIVA As Double = ft.DevuelveScalarDoble(MyConn, " select SUM(IMPIVA) from jsvenivafac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac ")
            Dim PorcentajeDescuento As Double = IIf(SubTotal <= 0.0, 0.0, (1 - (SubTotal - Descuentos) / SubTotal) * 100)
            Dim PesoTotal As Double = ft.DevuelveScalarDoble(MyConn, " select SUM(PESO) from jsvenrenfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac")
            Dim numItems As Integer = ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jsvenrenfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac")

            Dim FechaEmision As String = ft.FormatoFechaMySQL(CDate(txtFecha.Text))
            Dim FormaDePago As String = ft.DevuelveScalarCadena(MyConn, " select formapago from jsvencatcli where codcli = '" & .Item("codcli") & "' and id_emp = '" & jytsistema.WorkID & "' ")

            Dim FechaVencimiento As String = ft.FormatoFechaMySQL(FechaVencimientoFactura(MyConn, .Item("CODCLI"), CDate(txtFecha.Text)))
            Dim CR_CO As Integer = IIf(Credito_Contado = "CR", 0, 1)
            If CR_CO = 1 Then FechaVencimiento = ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo)

            ft.Ejecutar_strSQL(MyConn, " INSERT INTO jsvenencfac " _
                           & " SELECT '" & NumeroFactura & "' , '" & FechaEmision & "', " _
                           & " CODCLI, COMEN, CODVEN, '" & cmbAlmacen.SelectedValue & "','" & cmbTransporte.SelectedValue _
                           & "', '" & FechaVencimiento & "', '', '', " & numItems & ", 0.00, " & PesoTotal & ", " _
                           & SubTotal & " , " & PorcentajeDescuento & " , pordes1, pordes2, pordes3, pordes4, " _
                           & " " & Descuentos & ", descuen1, descuen2, descuen3, descuen4, " _
                           & " " & Cargos & ", 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, vence1, vence2, vence3, vence4, " _
                           & " " & CR_CO & ", tipocredito,  FORMAPAG, NOMPAG, NUMPAG, '" & cmbCaja.SelectedValue & "', " _
                           & " 0.00, '', '', 0.00, '', '', 0.00, '', '', 0.00, '', '', 0.00, 0.00, '', 0, 0, 0.00, 0.00, '', '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "',  " _
                           & " 0.00, 0.00, " & ImpuestoIVA & ", 0.00, 1, 1, " _
                           & " " & SubTotal - Descuentos + Cargos + ImpuestoIVA & " , " _
                           & " ESTATUS, TARIFA, '', '', '', '', '', 0,  " _
                           & IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenencped", "block_date"), " '2009-01-01', ", " ") _
                           & " EJERCICIO, ID_EMP " _
                           & " FROM jsvenencped " _
                           & " WHERE  " _
                           & " numped = '" & NumeroPedido & "' AND " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")

            'InsertEditVENTASEncabezadoFactura(MyConn, lblInfo, True, NumeroFactura, CDate ( FechaEmision), .Item("CODCLI"), .Item("COMEN"), _ 
            '                                  .Item("CODVEN"), cmbAlmacen.SelectedValue , cmbTransporte.SelectedValue , CDate(FechaVencimiento ), "","", numItems , _ 
            '                                  0.00, PesoTotal , SubTotal , PorcentajeDescuento , .Item("PORDES1"), .Item("PORDES2"), .Item("PORDES3"), .Item("PORDES4"), _
            '                                  Descuentos , .Item("DESCUEN1"), .Item("DESCUEN2"), .Item("DESCUEN3"), .Item("DESCUEN4") , Cargos , _ 
            '                                  0.00,0.00,0.00,0.00, 0.00,0.00,0.00,0.00,  CDate(.Item("VENCE1").ToString ),  CDate( .Item("VENCE2").ToString )  , _ 
            '                                  CDate(.Item("VENCE3").ToString ) , CDate(.Item("VENCE4").ToString ) ,  CR_CO, .Item("TIPOCREDITO"), _ 
            '                                  .Item("FORMAPAG"), .Item("NOMPAG"), .Item("NUMPAG"), cmbCaja.SelectedValue , 0.00, "", "", 0.00, _ 
            '                                  "","", 0.00, "","",0.00, "","",0.00, 0.00,  "", 0, 0, 0.00, 0.00, '', jytsistema.sFechadeTrabajo ,  _
            '                                   & " 0.00, 0.00, " & ImpuestoIVA & ", 0.00, 1, 1, " _
            '               & " " & SubTotal - Descuentos + Cargos + ImpuestoIVA & " , " _
            '               & " ESTATUS, TARIFA, '', '', '', '', '', 0,  " _
            '               & " EJERCICIO, ID_EMP " _  

        End With

        dtEncabPed.Dispose()
        dtEncabPed = Nothing

    End Sub
    Private Sub btnFecha_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFecha.Click
        txtFecha.Text = ft.FormatoFecha(SeleccionaFecha(CDate(txtFecha.Text), Me, btnFecha))
    End Sub
    Private Sub btnDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesde.Click
        txtDesde.Text = ft.FormatoFecha(SeleccionaFecha(CDate(txtDesde.Text), Me, sender))
    End Sub
    Private Sub btnHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHasta.Click
        txtHasta.Text = ft.FormatoFecha(SeleccionaFecha(CDate(txtHasta.Text), Me, sender))
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
        Dim aFields() As String = {"SEL.entero.1.0", "NUMPED.cadena.20.0", "EMISION.fecha.0.0", "COMEN.cadena.100.0", "CONDPAG.cadena.2.0", _
                                   "ENTREGA.fecha.0.0", "TOT_PED.doble.19.2", "BACKORDER.doble.10.3", "KILOS.doble.10.3", _
                                   "CODCLI.cadena.20.0", "NOMBRE.cadena.250.0", "DISPONIBLE.doble.19.2"}

        CrearTabla(MyConn, lblInfo, jytsistema.WorkDataBase, True, tblCaja, aFields)

        strTotren = "((b.cantran * b.Precio - b.cantran * b.Precio * b.des_cli / 100 - (b.cantran * b.Precio - b.cantran * b.Precio * b.des_cli / 100) * des_art / 100) - " _
           & " (b.cantran * b.Precio - b.cantran * b.Precio * b.des_cli / 100 - (b.cantran * b.Precio - b.cantran * b.Precio * b.des_cli / 100) * b.des_art / 100) * b.des_ofe / 100 ) * " _
           & " ( b.totrendes/b.totren ) "

        ft.Ejecutar_strSQL(MyConn, " insert into " & tblCaja _
           & " select 1 sel, a.NUMPED, a.EMISION, a.comen, ELT(a.condpag+1, 'CR','CO') CONDPAG,  a.ENTREGA, a.TOT_PED, " _
           & " sum( if( b.cantran > 0, " _
           & " if( b.iva = ' ', " & strTotren & " , " _
           & " if( b.iva = 'A', " & (1 + PorcentajeIVA(MyConn, lblInfo, CDate(txtFecha.Text), "A") / 100) & "*" & strTotren & " , " _
           & " if( b.iva = 'B', " & (1 + PorcentajeIVA(MyConn, lblInfo, CDate(txtFecha.Text), "B") / 100) & "*" & strTotren & " , " _
           & " if( b.iva = 'C', " & (1 + PorcentajeIVA(MyConn, lblInfo, CDate(txtFecha.Text), "C") / 100) & "*" & strTotren & " , " _
           & " if( b.iva = 'D', " & (1 + PorcentajeIVA(MyConn, lblInfo, CDate(txtFecha.Text), "D") / 100) & "*" & strTotren & ", " & strTotren & " )) ))) " _
           & " , 0)) as Backorder, " _
           & " sum(if(b.cantran > 0, b.peso/b.cantidad*b.cantran , 0 )) as kilos, c.codcli, c.nombre, c.disponible " _
           & " FROM jsvenencped a " _
           & " left join jsvenrenped b on (a.numped = b.numped and a.ejercicio = b.ejercicio and a.id_emp = b.id_emp) " _
           & " left join jsvencatcli c on (a.codcli = c.codcli and a.id_emp = c.id_emp) " _
           & " Where " & strSQLAsesor _
           & " a.EMISION >= '" & ft.FormatoFechaMySQL(CDate(txtDesde.Text)) & "' AND " _
           & " a.EMISION <= '" & ft.FormatoFechaMySQL(CDate(txtHasta.Text)) & "' AND " _
           & " a.ESTATUS = 0  and " _
           & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
           & " group by a.numped " _
           & " order by a.CODVEN, a.NUMPED ")

        dtPedidos = ft.AbrirDataTable(ds, nTablaPedidos, MyConn, " select * from " & tblCaja)

        Dim aCampos() As String = {"NUMPED.No Prepedido.90.I.", _
                                   "CODCLI.Cliente.60.I.", _
                                   "NOMBRE.Nombre ó Razón social.220.I.", _
                                   "EMISION.Emisión.60.C.fecha", _
                                   "TOT_PED.Total.80.D.Numero", _
                                   "KILOS.Peso (Kgr).80.D.Cantidad", _
                                   "COMEN.Grupo.50.C.", _
                                   "CONDPAG.CR/CO.30.C."}


        ft.IniciarTablaPlus(dgPedidos, dtPedidos, aCampos, , True, New Font("Consolas", 7, FontStyle.Regular), False, , , False)
        Dim aCamposEdicion() As String = {"sel"}
        ft.EditarColumnasEnDataGridView(dgpedidos, aCamposEdicion)

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
        For Each selectedItem As DataRow In dtPedidos.Rows
            With selectedItem
                If CBool(.Item("sel")) Then
                    MontoTotal += .Item("tot_ped")
                    KilosTotal += .Item("Kilos")
                    Items += 1
                    strSQLPedidos += " NUMPED = '" & .Item("NUMPED") & "' OR"
                End If
            End With
        Next

        If Items <= 0 Then strSQLPedidos = " NUMPED = '' OR"
        strSQLPedidos = RTrim(strSQLPedidos)

        txtItems.Text = ft.FormatoEntero(CInt(Items))
        txtTotalpedidos.Text = ft.FormatoNumero(MontoTotal)
        txtPesoPedidos.Text = ft.FormatoCantidad(KilosTotal)

    End Sub

   
    Private Sub dgAsesores_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgAsesores.CellContentClick
        If e.ColumnIndex = 0 Then dtAsesores.Rows(e.RowIndex).Item(0) = Not CBool(dtAsesores.Rows(e.RowIndex).Item(0).ToString)
        PedidosAsesores()
    End Sub

    Private Sub dgPedidos_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgPedidos.CellContentClick
        If e.ColumnIndex = 0 Then dtPedidos.Rows(e.RowIndex).Item(0) = Not CBool(dtPedidos.Rows(e.RowIndex).Item(0).ToString)
        If e.ColumnIndex = 8 And e.RowIndex >= 0 Then
            With dtPedidos.Rows(e.RowIndex).Item("CONDPAG")
                If .ToString.Equals("CR") Then
                    dtPedidos.Rows(e.RowIndex).Item("CONDPAG") = "CO"
                Else
                    dtPedidos.Rows(e.RowIndex).Item("CONDPAG") = "CR"
                End If
            End With
        End If
        CalcularTotales()
    End Sub
End Class