Imports MySql.Data.MySqlClient
Public Class jsVenProPedidosFacturacion

    Private Const sModulo As String = "Procesar pedidos a facturación"

    Private MyConn As New MySqlConnection
    Private ds As New DataSet
    Private dtAsesores As New DataTable
    Private dtPedidos As New DataTable

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

        HabilitarObjetos(False, True, txtFecha, txtDesde, txtHasta, txtItems, txtPesoPedidos, txtTotalpedidos)

        txtFecha.Text = FormatoFechaCorta(jytsistema.sFechadeTrabajo)
        txtDesde.Text = FormatoFechaCorta(jytsistema.sFechadeTrabajo)
        txtHasta.Text = FormatoFechaCorta(jytsistema.sFechadeTrabajo)
        RellenaCombo(aMetodo, cmbMetodo)
        IniciarCajas()
        IniciarAlmacen()
        IniciarTransporte()

        txtItems.Text = FormatoEntero(0)
        txtPesoPedidos.Text = FormatoCantidad(0.0)
        txtTotalpedidos.Text = FormatoNumero(0.0)

        strSQLAsesores = "select codven, concat(nombres,' ',apellidos) nombre " _
            & " from jsvencatven " _
            & " where " _
            & " estatus = 1 and " _
            & " tipo = " & TipoVendedor.iFuerzaventa & " and " _
            & " ID_EMP = '" & jytsistema.WorkID & "' order by codven "

        ds = DataSetRequery(ds, strSQLAsesores, MyConn, nTablaAsesores, lblInfo)
        dtAsesores = ds.Tables(nTablaAsesores)

        CargaListViewAsesores(lvAsesores, dtAsesores)

        strSQLAsesores = ""
        lvPedidos.Items.Clear()

    End Sub
    Private Sub IniciarAlmacen()
        Dim str As String = " select * from jsmercatalm where id_emp = '" & jytsistema.WorkID & "' order by codalm "
        Dim nTablaAlm As String = "tblAlm"

        Dim dtAlm As DataTable
        ds = DataSetRequery(ds, str, MyConn, nTablaAlm, lblInfo)
        dtAlm = ds.Tables(nTablaAlm)

        RellenaComboConDatatable(cmbAlmacen, dtAlm, "codalm", "codalm")

    End Sub
    Private Sub IniciarTransporte()
        Dim strTran As String = " select * from jsconctatra where id_emp = '" & jytsistema.WorkID & "' order by codtra "
        Dim nTablaTran As String = "tblTran"

        Dim dtTran As DataTable
        ds = DataSetRequery(ds, strTran, MyConn, nTablaTran, lblInfo)
        dtTran = ds.Tables(nTablaTran)

        RellenaComboConDatatable(cmbTransporte, dtTran, "codtra", "codtra")

    End Sub
    Private Sub IniciarCajas()
        Dim strCajas As String = " select * from jsbanenccaj where id_emp = '" & jytsistema.WorkID & "' order by caja "
        Dim nTablaCaja As String = "tblCaja"

        Dim dtCajas As DataTable
        ds = DataSetRequery(ds, strCajas, MyConn, nTablaCaja, lblInfo)
        dtCajas = ds.Tables(nTablaCaja)

        RellenaComboConDatatable(cmbCaja, dtCajas, "caja", "caja")

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
        Validado = False

        Dim CapacidadCamion As Double = EjecutarSTRSQL_Scalar(MyConn, lblInfo, _
                                                              " select  capacidad from jsconctatra where codtra = '" & cmbTransporte.SelectedValue & "' and id_emp = '" & jytsistema.WorkID & "'  ")

        If ValorNumero(txtPesoPedidos.Text) > CapacidadCamion Then
            MensajeCritico(lblInfo, "El peso total de los pedidos escogidos excede a la capacidad de carga del transporte escogido... ")
        End If

        Validado = True
    End Function

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        If Validado() Then

            For lvCont As Integer = 0 To lvPedidos.Items.Count - 1

                ProgressBar1.Value = (lvCont + 1) / lvPedidos.Items.Count * 100

                If lvPedidos.Items(lvCont).Checked Then

                    Dim PermiteFacturar As Boolean = False

                    Dim CodigoCliente As String = lvPedidos.Items(lvCont).SubItems(1).Text
                    Dim dDisponibilidadCliente As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, _
                                                                                " select disponible from jsvencatcli where codcli = '" _
                                                                                & CodigoCliente & _
                                                                                "' and id_emp = '" & jytsistema.WorkID & "' "))

                    Dim iComision As Integer = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, _
                                                                                " select backorder from jsvencatcli where codcli = '" _
                                                                                & CodigoCliente & _
                                                                                "' and id_emp = '" & jytsistema.WorkID & "' "))

                    Dim dMontoPeddido As Double = ValorNumero(lvPedidos.Items(lvCont).SubItems(4).Text)

                    lblProgreso.Text = " Pedido No. " & lvPedidos.Items(lvCont).Text

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
                            Dim EstatusCliente As Integer = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select estatus from jsvencatcli " _
                                                                                       & " where " _
                                                                                       & " codcli = '" & CodigoCliente & "' and " _
                                                                                       & " id_emp = '" & jytsistema.WorkID & "' "))
                            If EstatusCliente >= 1 Then PermiteFacturar = False
                        End If


                        If PermiteFacturar Then

                            '2.- VERIFICA DISPONIBILDAD DE CLIENTE
                            If dDisponibilidadCliente >= dMontoPeddido Then

                                Dim NumeroFacturaProvisional As String = "FCTMP" & NumeroAleatorio(100000)

                                '3.- TRANSFERIR RENGLONES EN PEDIDOS
                                TransferirRenglones(MyConn, lvPedidos.Items(lvCont).Text, NumeroFacturaProvisional, cmbAlmacen.SelectedValue)

                                'INCLUSION DE FLETES AUTOMATICOS
                                IncluirFleteAutomatico(CodigoCliente, montoFletesPorRegion(MyConn, CodigoCliente), NumeroFacturaProvisional)

                                'Transfiere y calcula Descuentos Globales o de factura
                                DescuentoEnFactura(NumeroFacturaProvisional, lvPedidos.Items(lvCont).Text)

                                'VERIFICAR PRECIOS DE FACTURACION
                                VerificarFacturacionAPartirDePrecios(lvPedidos.Items(lvCont).Text, NumeroFacturaProvisional)

                                'Calcular IVA
                                CalcularIVAFactura(NumeroFacturaProvisional)

                                'Crear Encabezado factura
                                If CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsvenrenfac where numfac = '" & NumeroFacturaProvisional & "' and id_emp = '" & jytsistema.WorkID & "'")) > 0 Then

                                    'ACTUALIZA TABLAS FACTURA CON EL NUMERO DE FACTURA REAL
                                    Dim NumeroFactura As String = Contador(MyConn, lblInfo, Gestion.iVentas, "VENNUMFAC", "8")
                                    EjecutarSTRSQL(MyConn, lblInfo, " update jsvenrenfac set numfac = '" & NumeroFactura & "' where numfac = '" & NumeroFacturaProvisional & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                    EjecutarSTRSQL(MyConn, lblInfo, " update jsvenivafac set numfac = '" & NumeroFactura & "' where numfac = '" & NumeroFacturaProvisional & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                    EjecutarSTRSQL(MyConn, lblInfo, " update jsvendesfac set numfac = '" & NumeroFactura & "' where numfac = '" & NumeroFacturaProvisional & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                    EjecutarSTRSQL(MyConn, lblInfo, " update jsvenrencom set numdoc = '" & NumeroFactura & "' where numdoc = '" & NumeroFacturaProvisional & "' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ")
                                    EjecutarSTRSQL(MyConn, lblInfo, " update jsvenforpag set numfac = '" & NumeroFactura & "' where numfac = '" & NumeroFacturaProvisional & "' and origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' ")
                                    EjecutarSTRSQL(MyConn, lblInfo, " update jsmertramer set numdoc = '" & NumeroFactura & "', numorg = '" & NumeroFactura & "' where numdoc = '" & NumeroFacturaProvisional & "' and id_emp = '" & jytsistema.WorkID & "' ")

                                    'ENCABEZADO DE FACTURA
                                    CrearEncabezadoFactura(NumeroFactura, lvPedidos.Items(lvCont).Text)

                                    CalcularIVAFactura(NumeroFactura)

                                    'Actualizar CXC, Caja y/o Bancos
                                    ActualizarCXCCajaBancos(MyConn, lblInfo, NumeroFactura)

                                    'Cambiar Estatus de pedido
                                    Dim elEstatus As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select sum(cantran) transito from jsvenrenped where numped = '" & lvPedidos.Items(lvCont).Text & "' and id_emp = '" & jytsistema.WorkID & "' group by numped "))
                                    If elEstatus = 0.0 Then EjecutarSTRSQL(MyConn, lblInfo, " update jsvenencped set estatus = 1 where numped = '" & lvPedidos.Items(lvCont).Text & "' and id_emp = '" & jytsistema.WorkID & "' ")

                                End If

                            End If

                        End If

                    End If

                End If

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
                monServicioFlete = CDbl(EjecutarSTRSQL_ScalarPLUS(MyConn, " select precio from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'"))
            End If

            Dim desServicioFlete As String = CStr(EjecutarSTRSQL_ScalarPLUS(MyConn, " select desser from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'"))
            Dim ivaServicioFlete As String = CStr(EjecutarSTRSQL_ScalarPLUS(MyConn, " select tipoiva from jsmercatser where codser = '" & codServicioFlete & "' and id_emp = '" & jytsistema.WorkID & "'"))

            Dim numRenglon As String = AutoCodigoPlus(MyConn, "renglon", "jsvenrenfac", "numfac", DocumentoNumero, 5)

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

        Dim CodigoClientePrepedido As String = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select codcli from jsvenencped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim clienteFacturaAPartirDeCostos As Boolean = CBool(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select codcre from jsvencatcli where codcli = '" & CodigoClientePrepedido & "' and id_emp = '" & jytsistema.WorkID & "' "))

        If clienteFacturaAPartirDeCostos Then
            Dim porcentajeGanancia As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select des_cli from jsvencatcli where codcli = '" & CodigoClientePrepedido & "' and id_emp = '" & jytsistema.WorkID & "' "))

            EjecutarSTRSQL(MyConn, lblInfo, " UPDATE jsvenrenfac a, jsmerctainv b " _
                            & " SET a.precio = ROUND(b.montoultimacompra*(1+" & porcentajeGanancia & "/100),2), " _
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
            InsertEditVENTASCXC(MyConn, lblInfo, True, .Item("codcli"), strTipo, numFactura, CDate(.Item("emision").ToString), FormatoHora(Now()), _
                CDate(.Item("vence").ToString), "", strTipo1 & ": " & numFactura, sSigno * .Item("tot_fac"), .Item("imp_iva"), _
                .Item("formapag"), .Item("caja"), .Item("numpag"), .Item("nompag"), "", "FAC", numFactura, _
                "0", "", jytsistema.sFechadeTrabajo, "", "", "", 0.0#, 0.0#, "", "", "", "", .Item("codven"), _
                .Item("codven"), "0", sFOTipo, jytsistema.WorkDivition)

            SaldoCxC(MyConn, lblInfo, .Item("codcli"))
        End With

        dtFac.Dispose()
        dtFac = Nothing

    End Sub


    Private Sub TransferirRenglones(ByVal MyConn As MySqlConnection, ByVal NumeroPedido As String, ByVal NumeroFactura As String, _
                                    ByVal Almacen As String)

        'INSERTAR Renglones DESDE EL PEDIDO CON LA CANTIDAD EN TRANSITO

        Dim nCliente As String = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT codcli from jsvenencped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim nVendedor As String = EjecutarSTRSQL_Scalar(MyConn, lblInfo, "SELECT codven from jsvenencped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' ")

        EjecutarSTRSQL(MyConn, lblInfo, " insert into jsvenrenfac " _
                    & " SELECT '" & NumeroFactura & "' NUMPED, RENGLON, ITEM, DESCRIP, IVA, ICS, UNIDAD, BULTOS, cantran cantidad, " _
                    & " INVENTARIO, SUGERIDO, REFUERZO, PRECIO, " _
                    & " peso/cantidad*cantran peso, LOTE, '','', ESTATUS, DES_CLI, DES_ART, DES_OFE, ROUND((precio*cantran - precio*cantran*des_cli/100 - (precio*cantran - precio*cantran*des_cli/100)*des_art/100 - " _
                    & " (precio*cantran - precio*cantran*des_cli/100 - (precio*cantran - precio*cantran*des_cli/100)*des_art/100)*des_ofe/100) , 2) " _
                    & " TOTREN, " _
                    & " ROUND((precio*cantran - precio*cantran*des_cli/100 - (precio*cantran - precio*cantran*des_cli/100)*des_art/100 - " _
                    & " (precio*cantran - precio*cantran*des_cli/100 - (precio*cantran - precio*cantran*des_cli/100)*des_art/100)*des_ofe/100) , 2) " _
                    & " TOTRENDES, '', '', '" & NumeroPedido & "', RENGLON, '', '', CODCON, '', '',  ACEPTADO, EDITABLE, EJERCICIO, ID_EMP " _
                    & " FROM jsvenrenped " _
                    & " WHERE " _
                    & " estatus < 2 and " _
                    & " cantran > 0 AND " _
                    & " numped = '" & NumeroPedido & "' AND " _
                    & " id_emp = '" & jytsistema.WorkID & "'")

        'INSERTAR comentarios
        EjecutarSTRSQL(MyConn, lblInfo, " insert into jsvenrencom " _
                       & " SELECT '" & NumeroFactura & "' numdoc, 'FAC' origen, item, renglon, comentario, id_emp " _
                       & " FROM jsvenrencom " _
                       & " WHERE " _
                       & " numdoc = '" & NumeroPedido & "' AND " _
                       & " origen = 'PED' AND " _
                       & " id_emp = '" & jytsistema.WorkID & "'")


        'CALCULA BONIFICACIONES
        CalculaBonificaciones(MyConn, lblInfo, ModuloBonificacion.iFactura, NumeroFactura, CDate(txtFecha.Text), _
                              EjecutarSTRSQL_Scalar(MyConn, lblInfo, "SELECT tarifa from jsvenencped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' "))

        'AJUSTAR POR EXISTENCIAS
        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA12")) Then
            AjustarExistencias(MyConn, NumeroFactura, lblInfo, Almacen, "NUMFAC", "jsvenrenfac", "FAC")
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
                    Dim CostoItem As Double = IIf(Mid(.Item("item"), 1, 1) = "$", 0, CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select montoultimacompra from jsmerctainv where codart = '" & .Item("item") & "' and id_emp = '" & jytsistema.WorkID & "' ")))

                    Dim Equivale As Double = Equivalencia(MyConn, lblInfo, .Item("ITEM"), .Item("UNIDAD"))

                    InsertEditMERCASMovimientoInventario(MyConn, lblInfo, True, .Item("item"), CDate(txtFecha.Text), _
                                       "SA", NumeroFactura, .Item("UNIDAD"), _
                                       .Item("CANTIDAD"), .Item("peso"), CostoItem * .Item("CANTIDAD") / IIf(Equivale = 0.0, 1, Equivale), _
                                       CostoItem * .Item("CANTIDAD") / IIf(Equivale = 0.0, 1, Equivale), _
                                       "FAC", NumeroFactura, "", nCliente, .Item("TOTREN"), .Item("TOTRENDES"), 0.0, 0.0, nVendedor, Almacen, _
                                       .Item("RENGLON"), jytsistema.sFechadeTrabajo)


                    'ACTUALIZANDO CANTIDAD EN TRANSITO EN PEDIDOS
                    EjecutarSTRSQL(MyConn, lblInfo, " update jsvenrenped set " _
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

    'Private Sub CalculaBonificaciones(MyConn As MySqlConnection, lblInfo As Label, NumeroFactura As String, Tarifa As String)

    '    EjecutarSTRSQL(MyConn, lblInfo, " DELETE from jsvenrenfac where NUMFAC = '" & NumeroFactura & "' and EDITABLE = 1 and  ESTATUS = '2' AND " _
    '            & " ID_EMP = '" & jytsistema.WorkID & "' ")

    '    Dim dtBonFac As DataTable
    '    Dim nTableBonFac As String = "tblbon" & NumeroFactura

    '    ds = DataSetRequery(ds, " select * from jsvenrenfac " _
    '                            & " where " _
    '                            & " NUMFAC = '" & NumeroFactura & "' and " _
    '                            & " ESTATUS < '2' and " _
    '                            & " ID_EMP = '" & jytsistema.WorkID & "' ", MyConn, nTableBonFac, lblInfo)

    '    dtBonFac = ds.Tables(nTableBonFac)

    '    If dtBonFac.Rows.Count > 0 Then
    '        Dim jCont As Integer
    '        For jCont = 0 To dtBonFac.Rows.Count - 1
    '            With dtBonFac.Rows(jCont)
    '                If Mid(.Item("item").ToString, 1, 1) <> "$" Then
    '                    Dim fBonificacion As Bonificaciones = BonificacionVigenteOferta(MyConn, lblInfo, CDate(txtFecha.Text), _
    '                                                                                     .Item("ITEM"), .Item("renglon"), Tarifa, NumeroFactura, "FAC", 0)

    '                    Dim CantidadBonificacion As Double = fBonificacion.CantidadABonificar
    '                    Dim ItemOferta As String = fBonificacion.ItemABonificar
    '                    Dim UnidadOferta As String = fBonificacion.UnidadDeBonificacion

    '                    If CantidadBonificacion > 0 Then

    '                        Dim PrecioOferta As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select precio_" & Tarifa & " from jsmerctainv where codart = '" & ItemOferta & "' and id_emp = '" & jytsistema.WorkID & "' "))
    '                        PrecioOferta = Math.Round(PrecioOferta / Equivalencia(MyConn, lblInfo, ItemOferta, UnidadOferta), 2)
    '                        Dim PesoOferta As Double = CantidadBonificacion * _
    '                            CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select PESOUNIDAD from jsmerctainv where codart = '" & ItemOferta & "' and id_emp = '" & jytsistema.WorkID & "' ")) / Equivalencia(MyConn, lblInfo, ItemOferta, UnidadOferta)


    '                        EjecutarSTRSQL(MyConn, lblInfo, "INSERT INTO jsvenrenfac " _
    '                                & "(NUMFAC, RENGLON, ITEM, DESCRIP, IVA, UNIDAD, CANTIDAD," _
    '                                & " PRECIO, DES_ART, DES_CLI, TOTREN, PESO, " _
    '                                & " ESTATUS, ACEPTADO, EDITABLE, EJERCICIO, ID_EMP) VALUES (" _
    '                                & "'" & NumeroFactura & "', " _
    '                                & "'" & Format(jCont + 1, "00000") & "', " _
    '                                & "'" & ItemOferta & "', " _
    '                                & "'" & Mid(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select nomart from jsmerctainv where codart = '" & ItemOferta & "' and id_emp = '" & jytsistema.WorkID & "' "), 1, 50) & "', " _
    '                                & "'" & EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select IVA from jsmerctainv where codart = '" & ItemOferta & "' and id_emp = '" & jytsistema.WorkID & "' ") & "', " _
    '                                & "'" & UnidadOferta & "', " _
    '                                & "" & CantidadBonificacion & ", " _
    '                                & "" & PrecioOferta & ", " _
    '                                & "" & 100 & ", " _
    '                                & "" & 0 & ", " _
    '                                & "" & 0 & ", " _
    '                                & "" & PesoOferta & ", " _
    '                                & "'2', '1', 1, " _
    '                                & "'" & jytsistema.WorkExercise & "', " _
    '                                & "'" & jytsistema.WorkID & "')")

    '                    End If


    '                End If
    '            End With
    '        Next
    '    End If

    '    dtBonFac.Dispose()
    '    dtBonFac = Nothing

    'End Sub

    Private Sub DescuentoEnFactura(ByVal NumeroFactura As String, ByVal NumeroPedido As String)

        'Tranfiere descuentos
        EjecutarSTRSQL(MyConn, lblInfo, " insert into jsvendesfac " _
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

        Dim TotalDescuento As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "  SELECT SUM(descuento) descuento FROM jsvendesfac WHERE numfac = '" & NumeroFactura & "'  AND id_emp = '" & jytsistema.WorkID & "' group by numfac "))

        '//////Reparte la porcion del descuento general en los renglones de factura

        If EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT a.numfac, SUM(a.totren) subtotal " _
                                 & " FROM jsvenrenfac a " _
                                 & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                                 & " WHERE " _
                                 & " a.estatus = 0 AND " _
                                 & " b.descuento = 1 AND " _
                                 & " b.regulado = 0 AND " _
                                 & " a.numfac = '" & NumeroFactura & "'  AND " _
                                 & " a.id_emp = '" & jytsistema.WorkID & "' GROUP BY NUMFAC").ToString <> "0" Then

            EjecutarSTRSQL(MyConn, lblInfo, " UPDATE jsvenrenfac a, (SELECT a.numfac, a.renglon, a.item, a.totren, " _
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
    Private Sub CrearEncabezadoFactura(ByVal NumeroFactura As String, ByVal NumeroPedido As String)

        Dim dtEncabPed As DataTable
        Dim nTablaEncabPED As String = "tblEncabPed"

        ds = DataSetRequery(ds, " select * from jsvenencped where numped = '" & NumeroPedido & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaEncabPED, lblInfo)
        dtEncabPed = ds.Tables(nTablaEncabPED)

        With dtEncabPed.Rows(0)
            Dim SubTotal As Double = CalculaTotalRenglonesVentas(MyConn, lblInfo, "jsvenrenfac", "numfac", "totren", NumeroFactura, 0)
            Dim Descuentos As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select if( sum(descuento) is null, 0, sum(descuento)) from jsvendesfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' "))
            Dim Cargos As Double = CalculaTotalRenglonesVentas(MyConn, lblInfo, "jsvenrenfac", "numfac", "totren", NumeroFactura, 1)
            Dim ImpuestoIVA As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select sum(impiva) from jsvenivafac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac "))
            Dim PorcentajeDescuento As Double = IIf(SubTotal <= 0.0, 0.0, (1 - (SubTotal - Descuentos) / SubTotal) * 100)
            Dim PesoTotal As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select sum(peso) from jsvenrenfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac"))
            Dim numItems As Integer = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsvenrenfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' group by numfac"))

            Dim FechaEmision As String = FormatoFechaMySQL(CDate(txtFecha.Text))
            Dim FormaDePago As String = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select formapago from jsvencatcli where codcli = '" & .Item("codcli") & "' and id_emp = '" & jytsistema.WorkID & "' ")
            Dim FechaVencimiento As String = FormatoFechaMySQL(DateAdd(DateInterval.Day, DiasCreditoAlVencimiento(MyConn, lblInfo, FormaDePago), CDate(txtFecha.Text)))

            If .Item("condpag") = 1 Then FechaVencimiento = FormatoFechaMySQL(jytsistema.sFechadeTrabajo)

            EjecutarSTRSQL(MyConn, lblInfo, " INSERT INTO jsvenencfac " _
                           & " SELECT '" & NumeroFactura & "' , '" & FechaEmision & "', " _
                           & " CODCLI, COMEN, CODVEN, '" & cmbAlmacen.SelectedValue & "','" & cmbTransporte.SelectedValue _
                           & "', '" & FechaVencimiento & "', '', '', " & numItems & ", 0.00, " & PesoTotal & ", " _
                           & SubTotal & " , " & PorcentajeDescuento & " , pordes1, pordes2, pordes3, pordes4, " _
                           & " " & Descuentos & ", descuen1, descuen2, descuen3, descuen4, " _
                           & " " & Cargos & ", 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, vence1, vence2, vence3, vence4, " _
                           & " condpag, tipocredito,  FORMAPAG, NOMPAG, NUMPAG, '" & cmbCaja.SelectedValue & "', " _
                           & " 0.00, '', '', 0.00, '', '', 0.00, '', '', 0.00, '', '', 0.00, 0.00, '', 0, 0, 0.00, 0.00, '', '" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "',  " _
                           & " 0.00, 0.00, " & ImpuestoIVA & ", 0.00, 1, 1, " _
                           & " " & SubTotal - Descuentos + Cargos + ImpuestoIVA & " , " _
                           & " ESTATUS, TARIFA, '', '', '', '', '', 0,  " _
                           & " EJERCICIO, ID_EMP " _
                           & " FROM jsvenencped " _
                           & " WHERE  " _
                           & " numped = '" & NumeroPedido & "' AND " _
                           & " id_emp = '" & jytsistema.WorkID & "' ")
        End With

        dtEncabPed.Dispose()
        dtEncabPed = Nothing

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
            strSQLAsesor += " a.codven = '" & e.Item.Text & "' OR"
            iSel += 1
        Else
            strSQLAsesor = Replace(strSQLAsesor, " a.codven = '" & e.Item.Text & "' OR", "")
            iSel -= 1
        End If
        If iSel <= 0 Then strSQLAsesor = " a.codven = '' OR"

        strSQLAsesor = RTrim(strSQLAsesor)

        If strSQLAsesor <> "" Then AbrirPedidos("( " & strSQLAsesor.Substring(0, strSQLAsesor.Length - 3) & " ) AND ")

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
           & " FROM jsvenencped a " _
           & " left join jsvenrenped b on (a.numped = b.numped and a.ejercicio = b.ejercicio and a.id_emp = b.id_emp) " _
           & " left join jsvencatcli c on (a.codcli = c.codcli and a.id_emp = c.id_emp) " _
           & " Where " & strSQLAsesor _
           & " a.EMISION >= '" & FormatoFechaMySQL(CDate(txtDesde.Text)) & "' AND " _
           & " a.EMISION <= '" & FormatoFechaMySQL(CDate(txtHasta.Text)) & "' AND " _
           & " a.tot_ped > 0 and " _
           & " a.ESTATUS = 0  and " _
           & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
           & " group by a.numped " _
           & " order by a.CODVEN, a.NUMPED ", MyConn, nTablaPedidos, lblInfo)

        dtPedidos = ds.Tables(nTablaPedidos)

        CargaListViewPrepedidos(lvPedidos, dtPedidos)

        'SELECCIONAR TODOS LOS PEDIDOS
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
                strSQLPedidos += " NUMPED = '" & e.Item.Text & "' OR"
            Else
                MontoTotal -= ValorNumero(e.Item.SubItems(4).Text)
                KilosTotal -= ValorNumero(e.Item.SubItems(5).Text)
                Items -= 1
                strSQLPedidos = Replace(strSQLPedidos, "NUMPED = '" & e.Item.Text & "' OR", "")
            End If
            If Items <= 0 Then strSQLPedidos = " NUMPED = '' OR"


            strSQLPedidos = RTrim(strSQLPedidos)

            txtItems.Text = FormatoEntero(CInt(Items))
            txtTotalpedidos.Text = FormatoNumero(MontoTotal)
            txtPesoPedidos.Text = FormatoCantidad(KilosTotal)

        End If

    End Sub
   



End Class