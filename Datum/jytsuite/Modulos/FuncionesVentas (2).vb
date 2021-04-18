Imports MySql.Data.MySqlClient
Module FuncionesVentas
    Function SaldoCxC(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCliente As String) As Double

        SaldoCxC = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select SUM(IMPORTE) sSaldo from jsventracob " _
                & " where codcli = '" & CodigoCliente & "' " _
                & " and EJERCICIO = '" & jytsistema.WorkExercise & "' " _
                & " and ID_EMP = '" & jytsistema.WorkID & "' group by codcli ")

        EjecutarSTRSQL(MyConn, lblInfo, " UPDATE jsvencatcli SET " _
            & " DISPONIBLE = LIMITECREDITO - " & SaldoCxC & ", " _
            & " SALDO = " & SaldoCxC _
            & " where codcli = '" & CodigoCliente & "' " _
            & " and ID_EMP = '" & jytsistema.WorkID & "'")

    End Function

    Public Sub AjustarExistencias(MyConn As MySqlConnection, NumeroDocumento As String, lblInfo As Label, _
                                  Almacen As String, Campo As String, Tabla As String, Origen As String)

        'AJUSTAR POR EXISTENCIAS
        Dim ds As New DataSet
        Dim dtFac As DataTable
        Dim nTableFac As String = "tblfac" & NumeroDocumento
        ds = DataSetRequery(ds, " select * from " & Tabla & " where " & Campo & " = '" & NumeroDocumento & "' and id_emp = '" & jytsistema.WorkID & "' order by estatus, renglon ", MyConn, nTableFac, lblInfo)
        dtFac = ds.Tables(nTableFac)

        If dtFac.Rows.Count > 0 Then
            For Each nRow As DataRow In dtFac.Rows
                With nRow
                    Dim pesoItem As Double = IIf(.Item("cantidad") > 0, .Item("peso") / .Item("cantidad"), 0.0)
                    Dim Existencia As Double = ExistenciaEnAlmacen(MyConn, .Item("item"), Almacen, lblInfo)
                    Dim Equivalencia As Double = CantidadEquivalente(MyConn, lblInfo, .Item("item"), .Item("unidad"), .Item("cantidad"))
                    If .Item("ESTATUS") <> "0" Then
                        Existencia -= CDbl(EjecutarSTRSQL_ScalarPLUS(MyConn, " select " _
                                                            & " sum(if( isnull(b.uvalencia), a.cantidad, a.cantidad / b.equivale )) Cantidad,  " _
                                                            & " a.unidad from " & Tabla & " a " _
                                                            & " left join jsmerequmer b on (a.item = b.codart and a.unidad = b.uvalencia and a.id_emp = b.id_emp) " _
                                                            & " where " _
                                                            & " a." & Campo & " = '" & NumeroDocumento & "' and " _
                                                            & " a.item = '" & .Item("ITEM") & "' and " _
                                                            & " a.estatus = '0' and " _
                                                            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                                                            & " a.id_emp = '" & jytsistema.WorkID & "' group by a.item "))
                    End If

                    If Existencia > 0 Then
                        If Equivalencia > Existencia Then Equivalencia = Existencia

                        Dim CantidadValida As Double = CantidadDesquivalente(MyConn, lblInfo, .Item("item"), .Item("unidad"), Equivalencia)
                        If MultiploValido(MyConn, .Item("item"), .Item("unidad"), CantidadValida, lblInfo) Then

                            Dim TotalNetoRenglon As Double = CantidadValida * .Item("precio")
                            Dim DescuentoArticulo As Double = IIf(MercanciaRegulada(MyConn, lblInfo, .Item("item")), 0.0, TotalNetoRenglon * .Item("des_art") / 100)
                            Dim DescuentoCliente As Double = IIf(MercanciaRegulada(MyConn, lblInfo, .Item("item")), 0.0, (TotalNetoRenglon - DescuentoArticulo) * .Item("DES_CLI") / 100)
                            Dim DescuentoOferta As Double = IIf(MercanciaRegulada(MyConn, lblInfo, .Item("item")), 0.0, (TotalNetoRenglon - DescuentoArticulo - DescuentoCliente) * .Item("DES_OFE") / 100)
                            Dim TotalRenglon As Double = TotalNetoRenglon - DescuentoArticulo - DescuentoCliente - DescuentoOferta

                            EjecutarSTRSQL(MyConn, lblInfo, " update " & Tabla & " set " _
                                                        & " CANTIDAD = " & CantidadValida & ", " _
                                                        & " TOTREN = " & TotalRenglon & ", " _
                                                        & " TOTRENDES = " & TotalRenglon & ", " _
                                                        & " PESO = " & CantidadValida * pesoItem & " " _
                                                        & " Where " _
                                                        & " " & Campo & " = '" & NumeroDocumento & "' and " _
                                                        & " item = '" & .Item("item") & "' and " _
                                                        & " renglon = '" & .Item("renglon") & "' and " _
                                                        & " estatus = '" & .Item("estatus") & "' and " _
                                                        & " id_emp = '" & jytsistema.WorkID & "' ")
                        Else
                            EjecutarSTRSQL(MyConn, lblInfo, " delete from " & Tabla & " where  " _
                                           & " " & Campo & " = '" & NumeroDocumento & "' and  " _
                                           & " item = '" & .Item("item") & "' and " _
                                           & " renglon = '" & .Item("renglon") & "' and " _
                                           & " estatus = '" & .Item("estatus") & "' and " _
                                           & " id_emp = '" & jytsistema.WorkID & "' ")

                            EjecutarSTRSQL(MyConn, lblInfo, " delete from jsvenrencom where numdoc = '" & NumeroDocumento & "' and " _
                                           & " origen = '" & Origen & "' and " _
                                           & " renglon = '" & .Item("renglon") & "' and " _
                                           & " id_emp = '" & jytsistema.WorkID & "' ")

                        End If
                    Else
                        EjecutarSTRSQL(MyConn, lblInfo, " delete from " & Tabla & " where  " _
                                           & " " & Campo & " = '" & NumeroDocumento & "' and  " _
                                           & " item = '" & .Item("item") & "' and " _
                                           & " renglon = '" & .Item("renglon") & "' and " _
                                           & " estatus = '" & .Item("estatus") & "' and " _
                                           & " id_emp = '" & jytsistema.WorkID & "' ")

                        EjecutarSTRSQL(MyConn, lblInfo, " delete from jsvenrencom where numdoc = '" & NumeroDocumento & "' and " _
                                       & " origen = '" & Origen & "' and " _
                                       & " renglon = '" & .Item("renglon") & "' and " _
                                       & " id_emp = '" & jytsistema.WorkID & "' ")

                    End If
                    ActualizarExistenciasPlus(MyConn, .Item("item"))

                End With
            Next
        End If

        ds.Dispose()
        dtFac.Dispose()
        dtFac = Nothing
        ds = Nothing



    End Sub

    Public Sub CalculaBonificaciones(MyConn As MySqlConnection, lblInfo As Label, nModulo As Integer, numDocumento As String, _
                                     dFechaBonificacion As Date, sTarifa As String)

        'nModulo --> 0 = "FAC", 1 = "PED", 2 = "PPE", 3 = "COT", 4 = "PFC"

        Dim nTabla() As String = {"jsvenrenfac", "jsvenrenped", "jsvenrenpedrgv", "jsvenrencot", "jsvenrennot"}
        Dim nCampo() As String = {"numfac", "numped", "numped", "numcot", "numfac"}

        Dim nTablaRenglones As String = "tblRenglones" & NumeroAleatorio(100000)

        Dim fBonificacion As Bonificaciones
        Dim CantidadBonificacion As Double
        Dim ItemdeOferta As String
        Dim UnidadOferta As String
        Dim PrecioOferta As Double
        Dim PesoOferta As Double

        EjecutarSTRSQL(MyConn, lblInfo, "DELETE from " & nTabla(nModulo) & " where " & nCampo(nModulo) & " = '" & numDocumento & "' and EDITABLE = 1 and " _
            & " ESTATUS = '2' AND " _
            & " ID_EMP = '" & jytsistema.WorkID & "' and " _
            & " EJERCICIO = '" & jytsistema.WorkExercise & "' ")

        Dim ds As New DataSet
        Dim dtRenglones As DataTable

        ds = DataSetRequery(ds, " select * from " & nTabla(nModulo) & " where " & nCampo(nModulo) & " and id_emp = '" & jytsistema.WorkID & "' ORDER BY renglon ", MyConn, nTablaRenglones, lblInfo)
        dtRenglones = ds.Tables(nTablaRenglones)

        If dtRenglones.Rows.Count > 0 Then
            For Each nRow As DataRow In dtRenglones.Rows
                With nRow

                    fBonificacion = BonificacionVigenteOferta(MyConn, lblInfo, dFechaBonificacion, .Item("ITEM"), _
                                                              .Item("RENGLON"), sTarifa, numDocumento, nModulo, 0)

                    CantidadBonificacion = fBonificacion.CantidadABonificar
                    ItemdeOferta = fBonificacion.ItemABonificar
                    UnidadOferta = fBonificacion.UnidadDeBonificacion
                    PrecioOferta = CDbl(EjecutarSTRSQL_ScalarPLUS(MyConn, " select precio_" & sTarifa & " from jsmerctainv where codart = '" & ItemdeOferta & "' and id_emp = '" & jytsistema.WorkID & "' "))
                    PrecioOferta = Math.Round(PrecioOferta / Equivalencia(MyConn, lblInfo, ItemdeOferta, UnidadOferta), 2)
                    PesoOferta = CDbl(EjecutarSTRSQL_ScalarPLUS(MyConn, " select pesounidad from jsmerctainv where codart = '" & ItemdeOferta & "' and id_emp ='" & jytsistema.WorkID & "' "))
                    PesoOferta = CantidadBonificacion * PesoOferta / Equivalencia(MyConn, lblInfo, ItemdeOferta, UnidadOferta)

                    If CantidadBonificacion <= 0 Then
                    Else
                        EjecutarSTRSQL(MyConn, lblInfo, " INSERT INTO " & nTabla(nModulo) _
                            & "(" & nCampo(nModulo) & ", RENGLON, ITEM, DESCRIP, IVA, UNIDAD, CANTIDAD," _
                            & " PRECIO, DES_ART, DES_CLI, TOTREN, PESO, " _
                            & " ESTATUS, ACEPTADO, EDITABLE, EJERCICIO, ID_EMP) VALUES (" _
                            & "'" & numDocumento & "', " _
                            & "'" & AutoCodigoPlus(MyConn, "RENGLON", nTabla(nModulo), nCampo(nModulo), numDocumento, 5) & "', " _
                            & "'" & ItemdeOferta & "', " _
                            & "'" & CStr(EjecutarSTRSQL_ScalarPLUS(MyConn, " SELECT NOMART FROM jsmerctainv WHERE codart = '" & ItemdeOferta & "' AND  id_emp = '" & jytsistema.WorkID & "' ")) & "', " _
                            & "'" & CStr(EjecutarSTRSQL_ScalarPLUS(MyConn, " SELECT IVA FROM jsmerctainv WHERE codart = '" & ItemdeOferta & "' AND  id_emp = '" & jytsistema.WorkID & "' ")) & "', " _
                            & "'" & UnidadOferta & "', " _
                            & "" & CantidadBonificacion & ", " _
                            & "" & PrecioOferta & ", " _
                            & "" & 100 & ", " _
                            & "" & 0 & ", " _
                            & "" & 0 & ", " _
                            & "" & PesoOferta & ", " _
                            & "'2', '1', 1, " _
                            & "'" & jytsistema.WorkExercise & "', " _
                            & "'" & jytsistema.WorkID & "')")

                    End If

                End With
            Next

        End If

        dtRenglones.Dispose()
        ds.Dispose()

        dtRenglones = Nothing
        ds = Nothing

    End Sub


    Function ChequesDevueltosCliente(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Cliente As String) As Integer
        Return CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, "select count(*) Cantidad " _
            & " from jsbanchedev where YEAR('" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "') = YEAR(FECHADEV) AND " _
            & " PROV_CLI = '" & Cliente & "' and " _
            & " ID_EMP = '" & jytsistema.WorkID & "' group by prov_cli "))

    End Function

    Public Function PoseeChequesDevueltosSinCancelar(MyConn As MySqlConnection, lblInfo As Label, CodigoCliente As String) As Boolean

        Dim SaldoPorCancelar As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT " _
            & " IF( SUM(b.importe) IS NULL, 0.00, SUM(b.importe) ) saldo " _
            & " FROM jsvenencndb a " _
            & " LEFT JOIN jsventracob b ON (a.codcli = b.codcli AND a.numndb = b.nummov AND a.id_emp = b.id_emp) " _
            & " Where " _
            & " a.codcli = '" & CodigoCliente & "' AND " _
            & " a.comen = 'CHEQUE DEVUELTO, COMISION CHEQUE DEVUELTO y GASTOS ADM.' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " GROUP BY a.numndb " _
            & " Having Saldo > 0 " _
            & " ORDER BY a.emision desc "))

        If SaldoPorCancelar <= 0.0 Then SaldoPorCancelar = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT " _
            & " SUM(importe) saldo FROM jsventracob " _
            & " WHERE " _
            & " SUBSTRING(nummov,1,2) = 'CD' AND " _
            & " codcli = '" & CodigoCliente & "' " _
            & " GROUP BY nummov " _
            & " HAVING saldo > 0 "))

        PoseeChequesDevueltosSinCancelar = False
        If SaldoPorCancelar > 0 Then PoseeChequesDevueltosSinCancelar = True

    End Function



    Public Function DocumentoPoseeCancelacionesAbonos(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroDocumento As String)
        Dim nCancela As String = EjecutarSTRSQL_Scalar(Myconn, lblInfo, " select comproba from jsventracob " _
                                                       & " where " _
                                                       & " nummov = '" & NumeroDocumento & "' AND " _
                                                       & " tipomov in ('AB', 'CA', 'NC', 'ND' ) AND " _
                                                       & " id_emp = '" & jytsistema.WorkID & "' ")


        DocumentoPoseeCancelacionesAbonos = False
        If nCancela <> "0" Then DocumentoPoseeCancelacionesAbonos = True
    End Function

    Public Function DocumentoPoseeCancelacionesAbonosDepositados(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroDocumento As String)

        Dim dsCan As New DataSet
        Dim dtCan As New DataTable
        Dim nTablaCan As String = "tblCan" & NumeroAleatorio(10000)
        DocumentoPoseeCancelacionesAbonosDepositados = False

        dsCan = DataSetRequery(dsCan, " Select * from jsventracob  " _
                                                        & " where " _
                                                        & " nummov = '" & NumeroDocumento & "' AND " _
                                                        & " tipomov in ('AB', 'CA', 'NC', 'ND' ) AND " _
                                                        & " id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaCan, lblInfo)

        dtCan = dsCan.Tables(nTablaCan)

        For Each nRow As DataRow In dtCan.Rows
            With nRow
                If DocumentoPoseeCancelacionesAbonos(MyConn, lblInfo, .Item("comproba")) Then DocumentoPoseeCancelacionesAbonosDepositados = True
            End With
        Next

        dtCan.Dispose()
        dsCan.Dispose()

        dtCan = Nothing
        dsCan = Nothing


    End Function


    Public Function CancelacionAbonoDepositado(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ComprobanteNumero As String) As Boolean

        Dim Deposito As String = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT " _
            & " IF( a.DEPOSITO IS NULL, '', a.deposito) DEPOSITO " _
            & " FROM jsbantracaj a " _
            & " Where " _
            & " a.nummov = '" & ComprobanteNumero & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' ")

        CancelacionAbonoDepositado = False
        If Deposito = "0" Then Deposito = ""
        If Deposito <> "" Then CancelacionAbonoDepositado = True

    End Function

    Public Sub IniciarDescuentosAsesorEnCombo(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal cmbDescuentos As ComboBox, _
                                              ByVal CodigoAsesor As String, ByVal TipoAsesor As Integer, ByVal FechaDescuento As Date)

        Dim ds As New DataSet
        Dim eCont As Integer
        Dim nTabla As String = "tDescuentosA"
        Dim str As String = ""

        ds = DataSetRequery(ds, " select * from jsconcatdes " _
            & " where " _
            & " codven =  '" & CodigoAsesor & "' and " _
            & " inicio <= '" & FormatoFechaMySQL(FechaDescuento) & "' AND " _
            & " fin >= '" & FormatoFechaMySQL(FechaDescuento) & "' AND " _
            & " tipo = " & TipoAsesor & " and " _
            & " id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTabla, lblInfo)

        With ds.Tables(nTabla)
            Dim aDescuentos(.Rows.Count - 1) As String
            For eCont = 0 To .Rows.Count - 1
                aDescuentos(eCont) = .Rows(eCont).Item("coddes") & " | " & _
                    .Rows(eCont).Item("descrip")
            Next
            RellenaCombo(aDescuentos, cmbDescuentos)
        End With

        ds.Tables(nTabla).Dispose()
        ds.Dispose()
        ds = Nothing

    End Sub

    Public Function DesbloqueoDeCliente(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, ByVal CodigoCliente As String, _
                    ByVal Fecha As Date, ByVal DiasLimite As Integer, ByVal Causa As String, ByVal Comentario As String) As Boolean

        Dim dt As New DataTable
        Dim nTabla As String = "tbl" & NumeroAleatorio(100000)
        Dim strSQL As String = ""

        If CBool(ParametroPlus(MyConn, Gestion.iVentas, "VENPARAM07")) Then
            strSQL = " Union " _
                & " SELECT a.codcli, b.nombre, a.nummov, a.tipomov, a.refer, a.emision, a.vence, a.importe, SUM(a.importe) as saldo, " _
                & " to_days('" & FormatoFechaMySQL(Fecha) & "') - to_Days(a.vence) as DV, " _
                & " if( to_days('" & FormatoFechaMySQL(Fecha) & "') - to_Days(a.vence)>= " & DiasLimite & ", 1, 0) as lapso " _
                & " from jsventracob a " _
                & " LEFT JOIN jsvencatcli b ON  (b.CODCLI = a.CODCLI AND b.ID_EMP = a.ID_EMP) " _
                & " Where a.ID_EMP = '" & jytsistema.WorkID & "' " _
                & " and a.codcli = '" & CodigoCliente & "' " _
                & " and left(a.nummov,2) = 'CD' " _
                & " and historico = 0 " _
                & " GROUP BY a.codcli, a.nummov " _
                & " Having Saldo > 0.001 or Saldo < -0.001 "
        End If


        ds = DataSetRequery(ds, "SELECT a.CODCLI, b.NOMBRE, a.NUMMOV, a.TIPOMOV, a.REFER, a.EMISION, a.VENCE, a.IMPORTE, SUM(a.importe) AS SALDO, " _
            & " to_days('" & FormatoFechaMySQL(Fecha) & "') - to_Days(a.vence) as DV, " _
            & " if( to_days('" & FormatoFechaMySQL(Fecha) & "') - to_Days(a.vence)>= " & DiasLimite & ", 1, 0) as lapso " _
            & " from jsventracob a " _
            & " LEFT JOIN jsvencatcli b ON (b.CODCLI = a.CODCLI AND b.ID_EMP = a.ID_EMP)  " _
            & " Where a.ID_EMP = '" & jytsistema.WorkID & "' " _
            & " and a.tipomov <> 'ND' " _
            & " and a.codcli = '" & CodigoCliente & "' " _
            & " GROUP BY a.codcli, a.nummov Having Saldo > 0.001 AND a.vence < '" & FormatoFechaMySQL(Fecha) & "' and lapso = 1 " _
            & strSQL, MyConn, nTabla, lblInfo)

        dt = ds.Tables(nTabla)
        If dt.Rows.Count = 0 Then
            EjecutarSTRSQL(MyConn, lblInfo, " UPDATE jsvencatcli set estatus = 0 where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "'  ")
            InsertEditVENTASExpedienteCliente(MyConn, lblInfo, True, CodigoCliente, Fecha, Comentario, 0, Causa, 0)
        End If

        dt.Dispose()
        dt = Nothing

    End Function






    Public Function DescuentoAsesorValido(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoDescuento As String, _
                                          ByVal CodigoAsesor As String, ByVal FechaDescuento As Date, _
                                          ByVal TipoAsesor As Integer, ByVal PorcentajeDescuento As Double) As Boolean

        DescuentoAsesorValido = False

        Dim pordes As Double = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select if( pordes is null, 0.00, pordes) from jsconcatdes " _
                                            & " where " _
                                            & " coddes = '" & CodigoDescuento & "' and " _
                                            & " codven =  '" & CodigoAsesor & "' and " _
                                            & " inicio <= '" & FormatoFechaMySQL(FechaDescuento) & "' AND " _
                                            & " fin >= '" & FormatoFechaMySQL(FechaDescuento) & "' AND " _
                                            & " tipo = " & TipoAsesor & " and " _
                                            & " id_emp = '" & jytsistema.WorkID & "' ")

        If pordes = 0.0 Then Return False
        If PorcentajeDescuento > pordes Or PorcentajeDescuento < 0 Then Return False

        Return True

    End Function

    Function ActualizarDescuentoVentas(ByVal MyConn As MySqlConnection, ByVal dt As DataTable, _
                                       ByVal nomTablaEnDB As String, ByVal nomCampoEnBD As String, _
                                       ByVal MontoInicial As Double, ByVal lblInfo As Label) As Double

        Dim Subtotal As Double = MontoInicial
        Dim Descuento As Double = 0.0
        Dim TotalDescuento As Double = 0.0
        Dim eCont As Integer

        If dt.Rows.Count > 0 Then
            For eCont = 0 To dt.Rows.Count - 1
                With dt.Rows(eCont)
                    Descuento = Subtotal * .Item("PORDES") / 100
                    EjecutarSTRSQL(MyConn, lblInfo, " update " & nomTablaEnDB & " Set " _
                        & " descuento = " & Descuento & ", " _
                        & " subtotal = " & Subtotal - Descuento & " " _
                        & " where " _
                        & nomCampoEnBD & " = '" & .Item(nomCampoEnBD) & "' and " _
                        & " renglon = '" & .Item("renglon") & "' and " _
                        & " aceptado = '" & .Item("aceptado") & "' and " _
                        & " ejercicio = '" & .Item("ejercicio") & "' and " _
                        & " id_emp = '" & .Item("id_emp") & "' ")

                    Subtotal -= Descuento
                    TotalDescuento += Descuento

                End With
            Next
            Return TotalDescuento
        Else
            Return 0.0
        End If

    End Function

    Public Function CalculaTotalRenglonesVentas(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                                ByVal nomTablaEnDB As String, ByVal nomCampoClaveEnBD As String, _
                                                ByVal nomCampoImporteEnBD As String, ByVal NumeroDocumento As String, _
                                                Optional ByVal TipoRenglon As Integer = 9, _
                                                Optional ByVal CodigoProveedor As String = "") As Double

        CalculaTotalRenglonesVentas = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select if(  sum(" & nomCampoImporteEnBD & ") is null, 0.00, sum(" & nomCampoImporteEnBD & ") ) from " _
                                                            & nomTablaEnDB _
                                                            & " where " & _
                                                            nomCampoClaveEnBD & " = '" & NumeroDocumento & "' and " _
                                                            & IIf(CodigoProveedor <> "", " codpro = '" & CodigoProveedor & "' and ", "") _
                                                            & IIf(TipoRenglon <= 2, " estatus = '" & TipoRenglon & "' and ", "") _
                                                            & " id_emp = '" & jytsistema.WorkID & "'  "))


    End Function
    Public Function MontoParaDescuentoVentas(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                             ByVal nomCampoClave As String, ByVal nomTablaRenglones As String, _
                                             ByVal NumeroDocumento As String) As Double

        MontoParaDescuentoVentas = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT SUM(a.totren) subtotal " _
                & "             FROM " & nomTablaRenglones & " a " _
                & "             LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                & "             Where " _
                & "             a.estatus = 0 and " _
                & "             b.descuento = 1 AND " _
                & "             a." & nomCampoClave & " = '" & NumeroDocumento & "'  AND " _
                & "             a.id_emp = '" & jytsistema.WorkID & "' group by a." & nomCampoClave & " "))

    End Function
    Public Function CalculaTotalIVAVentas(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal nomTablaDescuentos As String, _
                                                ByVal nomTablaIVA As String, ByVal nomTablaRenglones As String, _
                                                ByVal nomCampoClave As String, ByVal NumeroDocumento As String, _
                                                ByVal nomCampoImporteIVA As String, ByVal nomCampoImporteRenglon As String, _
                                                ByVal FechaIVA As Date, Optional ByVal nombreCampoTotalRenglones As String = "totren", _
                                                Optional ByVal numSerialPOS As String = "", _
                                                Optional ByVal TipoMOV As Integer = 0) As Double

        EjecutarSTRSQL(MyConn, lblInfo, " delete from " & nomTablaIVA & " where " & nomCampoClave & " = '" & NumeroDocumento & "' and " _
                       & IIf(nomTablaRenglones = "jsvenrenpos", _
                             IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenrenpos", "numserial"), " numserial = '" & numSerialPOS & "' AND ", ""), "") _
                       & IIf(nomTablaRenglones = "jsvenrenpos", _
                             IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenrenpos", "tipo"), " tipo = '" & TipoMOV & "' AND ", ""), "") _
                       & " id_emp = '" & jytsistema.WorkID & "'  ")


        EjecutarSTRSQL(MyConn, lblInfo, " insert into " & nomTablaIVA _
                & " SELECT a." & nomCampoClave & ", " _
                & IIf(nomTablaRenglones = "jsvenrenpos", IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenrenpos", "numserial"), " '" & numSerialPOS & "',", ""), "") _
                & IIf(nomTablaRenglones = "jsvenrenpos", IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenrenpos", "tipo"), TipoMOV & ", ", ""), "") _
                & " a.iva, if( b.monto is null, 0.00, b.monto) , SUM(a." & nombreCampoTotalRenglones & "des) baseiva, " _
                & " ROUND(SUM(a." & nombreCampoTotalRenglones & "des*if( b.monto is null, 0.00, b.monto)/100),2) impiva, '' ejercicio, a.id_emp " _
                & " FROM " & nomTablaRenglones & " a " _
                & " LEFT JOIN (SELECT fecha, tipo, monto " _
                & "            From JSCONCTAIVA " _
                & "            Where " _
                & "            fecha IN (SELECT MAX(fecha) FROM jsconctaiva WHERE fecha <= '" & FormatoFechaMySQL(FechaIVA) & "' GROUP BY tipo) AND " _
                & "            fecha <= '" & FormatoFechaMySQL(FechaIVA) & "') b ON (a.iva = b.tipo) " _
                & " Where " _
                & " a." & nomCampoClave & " = '" & NumeroDocumento & "' AND " _
                & IIf(nomTablaRenglones = "jsvenrenpos", _
                             IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenrenpos", "numserial"), " a.numserial = '" & numSerialPOS & "' AND ", ""), "") _
                       & IIf(nomTablaRenglones = "jsvenrenpos", _
                             IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenrenpos", "tipo"), " a.tipo = '" & TipoMOV & "' AND ", ""), "") _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " GROUP BY a.iva ")


        CalculaTotalIVAVentas = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select if(  sum(" & nomCampoImporteIVA & ") is null, 0.00, sum(" & nomCampoImporteIVA & ") ) from " _
                                                            & nomTablaIVA _
                                                            & " where " & _
                                                            nomCampoClave & " = '" & NumeroDocumento & "' and " _
                                                            & IIf(nomTablaRenglones = "jsvenrenpos", _
                                                                IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenrenpos", "numserial"), " numserial = '" & numSerialPOS & "' AND ", ""), "") _
                                                            & IIf(nomTablaRenglones = "jsvenrenpos", _
                                                                IIf(ExisteCampo(MyConn, lblInfo, jytsistema.WorkDataBase, "jsvenrenpos", "tipo"), " tipo = '" & TipoMOV & "' AND ", ""), "") _
                                                            & " id_emp = '" & jytsistema.WorkID & "'  "))


    End Function
    Public Sub MostrarDisponibilidad(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCliente As String, ByVal RIF As String, ByVal ds As DataSet, ByVal dg As DataGridView)
        If RIF <> "" Then
            Dim dtDisp As DataTable
            Dim nTableDisp As String = "tblDisp"

            Dim strSQLDisp As String = " select codcli, saldo, disponible, elt(estatus+1,'Activo','Bloqueado','Inactivo', 'Desincorporado') estatus, id_emp " _
                & " from jsvencatcli where rif = '" & RIF & "' order by id_emp "

            ds = DataSetRequery(ds, strSQLDisp, MyConn, nTableDisp, lblInfo)
            dtDisp = ds.Tables(nTableDisp)

            Dim aCampos() As String = {"codcli", "saldo", "disponible", "estatus", "ID_EMP"}
            Dim aNombres() As String = {"Cliente", "Saldo", "Disponible", "Estatus", "ID"}
            Dim aAnchos() As Long = {75, 90, 90, 60, 15}
            Dim aAlineacion() As Integer = {AlineacionDataGrid.Izquierda, AlineacionDataGrid.Derecha, _
                                            AlineacionDataGrid.Derecha, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro}

            Dim aFormatos() As String = {"", sFormatoNumero, sFormatoNumero, "", ""}
            IniciarTabla(dg, dtDisp, aCampos, aNombres, aAnchos, aAlineacion, aFormatos, True, , 8, False)

            Dim SaldoCliente As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select sum(importe) saldo " _
                                                                    & " from jsventracob " _
                                                                    & " where " _
                                                                    & " codcli = '" & CodigoCliente & "' and " _
                                                                    & " id_emp = '" & jytsistema.WorkID & "' group by codcli "))

            Dim Disponibilidad As Double = CDbl(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select disponible from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' "))
            Dim EstatusCliente As Integer = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select estatus from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' "))

            dg.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            dg.RowsDefaultCellStyle.SelectionForeColor = dg.DefaultCellStyle.ForeColor
            If EstatusCliente = 0 Then
                If SaldoCliente = 0 Then 'Verde 
                    dg.RowsDefaultCellStyle.BackColor = Color.PaleGreen
                    dg.RowsDefaultCellStyle.SelectionBackColor = Color.PaleGreen
                    dg.AlternatingRowsDefaultCellStyle.BackColor = Color.ForestGreen
                Else
                    If SaldoCliente > 0 Then
                        If Disponibilidad > 0 Then 'Amarillo
                            dg.RowsDefaultCellStyle.BackColor = Color.LightYellow
                            dg.RowsDefaultCellStyle.SelectionBackColor = Color.LightYellow
                            dg.AlternatingRowsDefaultCellStyle.BackColor = Color.Yellow
                        Else 'Rojo
                            dg.RowsDefaultCellStyle.BackColor = Color.LavenderBlush
                            dg.RowsDefaultCellStyle.SelectionBackColor = Color.LavenderBlush
                            dg.AlternatingRowsDefaultCellStyle.BackColor = Color.Crimson
                        End If
                    Else 'Rojo
                        dg.RowsDefaultCellStyle.BackColor = Color.LavenderBlush
                        dg.RowsDefaultCellStyle.SelectionBackColor = Color.LavenderBlush
                        dg.AlternatingRowsDefaultCellStyle.BackColor = Color.Crimson
                    End If
                End If
            Else
                dg.RowsDefaultCellStyle.BackColor = Color.LavenderBlush
                dg.RowsDefaultCellStyle.SelectionBackColor = Color.LavenderBlush
                dg.AlternatingRowsDefaultCellStyle.BackColor = Color.Crimson
            End If

        End If

    End Sub
    Public Function ProveedorClienteEnItem(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Proveedor As String, _
                                           ByVal Cliente As String) As Boolean

        Dim SiNo As String = CStr(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select codcli from jsvenprocli where " _
            & " CODPRO = '" & Proveedor & "' AND " _
            & " CODCLI = '" & Cliente & "' AND " _
            & " ID_EMP = '" & jytsistema.WorkID & "' "))

        ProveedorClienteEnItem = False
        If SiNo <> "0" Then ProveedorClienteEnItem = True

    End Function
    Function MercanciaAceptaDescuento(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoArticulo As String) As Boolean
        MercanciaAceptaDescuento = CBool(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select descuento from jsmerctainv where codart = '" & CodigoArticulo & "' and id_emp = '" & jytsistema.WorkID & "' "))
    End Function
    Public Function CondicionPagoProveedorCliente(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal FormaDePago As String) As Integer

        Dim Periodo As Integer = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select periodo from jsconctafor where codfor = '" & FormaDePago & "' and id_emp = '" & jytsistema.WorkID & "' "))
        CondicionPagoProveedorCliente = 1
        If Periodo <> 0 Then CondicionPagoProveedorCliente = 0

    End Function
    Public Function DiasCreditoAlVencimiento(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal FormaDePago As String) As Integer
        DiasCreditoAlVencimiento = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select periodo from jsconctafor where codfor = '" & FormaDePago & "' and id_emp = '" & jytsistema.WorkID & "' "))
    End Function
    Function ClientePoseeChequesFuturos(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCliente As String) As Boolean

        ClientePoseeChequesFuturos = False
        Dim ChequesPostDate As Integer = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) cuenta from jsbantracaj " _
            & " where " _
            & " tipomov = 'EN' and " _
            & " formpag = 'CH' and " _
            & " fecha > '" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and " _
            & " deposito = '' and " _
            & " prov_cli = '" & CodigoCliente & "' and " _
            & " id_emp = '" & jytsistema.WorkID & "' ")

        If ChequesPostDate > 0 Then ClientePoseeChequesFuturos = True

    End Function

    Public Sub CargaListViewAsesores(ByVal LV As ListView, ByVal dt As DataTable)
        Dim iCont As Integer

        LV.Clear()

        LV.BeginUpdate()

        LV.Columns.Add("Código", 60, HorizontalAlignment.Left)
        LV.Columns.Add("Nombre asesor comercial", 150, HorizontalAlignment.Left)

        For iCont = 0 To dt.Rows.Count - 1
            LV.Items.Add(dt.Rows(iCont).Item("codven").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("nombre").ToString)
        Next

        LV.EndUpdate()

    End Sub

    Public Sub CargarListaSeleccionAsesores(ByVal dg As DataGridView, ByVal dt As DataTable, Optional ByVal TamañoLetra As Integer = 9, _
                                            Optional ByVal EncabezadoFila As Boolean = True)

        Dim aFld() As String = {"sel", "codven", "nombre"}
        Dim aNom() As String = {"", "Código", "Nombre"}
        Dim aAnc() As Long = {20, 60, 150}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", "", ""}

        IniciarTablaSeleccion(dg, dt, aFld, aNom, aAnc, aAli, aFor, , True, TamañoLetra, EncabezadoFila)
        dg.ReadOnly = True
        For Each col As DataGridViewColumn In dg.Columns
            If col.Index > 0 Then col.ReadOnly = True
        Next

    End Sub

    Public Sub CargarListaSeleccionFacturasGuiaDespacho(ByVal dg As DataGridView, ByVal dt As DataTable, Optional ByVal TamañoLetra As Integer = 9, _
                                            Optional ByVal EncabezadoFila As Boolean = True, Optional TipoRelacion As Integer = 0)

        'TIPO RELACION    0 = FACTURAS ; 1 = NCR ; 2 = PEDIDOS

        Dim aFld() As String = {"sel", "numfac", "emision", "codcli", "nombre", IIf(TipoRelacion = 0, "tot_fac", IIf(TipoRelacion = 1, "tot_ncr", "tot_ped")), "kilos", "codven", "comen"}
        Dim aNom() As String = {"", "N° Factura", "Emisión", "Cliente", "Nombre y/o Razón social", "Total Documento", "Peso (Kgr)", "Asesor", "Grupo"}
        Dim aAnc() As Long = {20, 80, 60, 90, 240, 70, 70, 50, 50}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, _
                                 AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", "", sFormatoFechaCorta, "", "", sFormatoNumero, sFormatoCantidad, "", ""}

        IniciarTablaSeleccion(dg, dt, aFld, aNom, aAnc, aAli, aFor, , True, TamañoLetra, EncabezadoFila)
        dg.ReadOnly = True
        For Each col As DataGridViewColumn In dg.Columns
            If col.Index > 0 Then col.ReadOnly = True
        Next

    End Sub

    Public Sub CargarListaSeleccionRetencionesIVA(ByVal dg As DataGridView, ByVal dt As DataTable, Optional ByVal TamañoLetra As Integer = 9, _
                                           Optional ByVal EncabezadoFila As Boolean = True)

        Dim aFld() As String = {"sel", "nummov", "tipomov", "num_control", "emision", "vence", "importe", "impiva", "retimpiva"}
        Dim aNom() As String = {"", "N° Documento", "TP", "N° Control", "Emisión", "Vence", "Importe Inicial", "Importe IVA", "Retención Importe IVA"}
        Dim aAnc() As Long = {20, 90, 40, 90, 90, 90, 90, 90, 90}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                 AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, _
                                 AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha}
        Dim aFor() As String = {"", "", "", "", sFormatoFechaCorta, sFormatoFechaCorta, sFormatoNumero, sFormatoNumero, sFormatoNumero}

        IniciarTablaSeleccion(dg, dt, aFld, aNom, aAnc, aAli, aFor, , True, TamañoLetra, EncabezadoFila)
        dg.ReadOnly = False
        For Each col As DataGridViewColumn In dg.Columns
            If col.Index > 0 And col.Index < 8 Then col.ReadOnly = True
        Next

    End Sub

    Public Sub CargarListaSeleccionMercas(ByVal dg As DataGridView, ByVal dt As DataTable, Optional ByVal TamañoLetra As Integer = 9, _
                                            Optional ByVal EncabezadoFila As Boolean = True)

        Dim aFld() As String = {"sel", "codart", "nomart", "alterno"}
        Dim aNom() As String = {"", "Código", "Nombre", "Alterno"}
        Dim aAnc() As Long = {20, 100, 450, 100}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", "", "", ""}

        IniciarTablaSeleccion(dg, dt, aFld, aNom, aAnc, aAli, aFor, , True, TamañoLetra, EncabezadoFila)
        dg.ReadOnly = True
        For Each col As DataGridViewColumn In dg.Columns
            If col.Index > 0 Then col.ReadOnly = True
        Next

    End Sub

    Public Sub CargaListViewPreCobranza(ByVal LV As ListView, ByVal dt As DataTable)
        Dim iCont As Integer

        LV.Clear()

        LV.BeginUpdate()

        LV.Columns.Add("Recibo N°", 90, HorizontalAlignment.Left)
        LV.Columns.Add("Cliente", 60, HorizontalAlignment.Left)
        LV.Columns.Add("Documento N°", 80, HorizontalAlignment.Left)
        LV.Columns.Add("TP", 30, HorizontalAlignment.Center)
        LV.Columns.Add("Concepto", 190, HorizontalAlignment.Left)
        LV.Columns.Add("Importe", 70, HorizontalAlignment.Right)
        LV.Columns.Add("Referencia", 90, HorizontalAlignment.Left)
        LV.Columns.Add("N° Pago", 90, HorizontalAlignment.Left)
        LV.Columns.Add("Nombre pago", 90, HorizontalAlignment.Left)
        LV.Columns.Add("FP", 30, HorizontalAlignment.Center)

        For iCont = 0 To dt.Rows.Count - 1
            LV.Items.Add(dt.Rows(iCont).Item("comproba").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("codcli").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("nummov").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("tipomov").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("concepto").ToString)
            LV.Items(iCont).SubItems.Add(FormatoNumero(CDbl(dt.Rows(iCont).Item("importe").ToString)))
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("refer").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("numpag").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("nompag").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("formapag").ToString)

        Next

        LV.EndUpdate()

    End Sub


    Public Sub CargaListViewPrepedidos(ByVal LV As ListView, ByVal dt As DataTable)
        Dim iCont As Integer

        LV.Clear()

        LV.BeginUpdate()

        LV.Columns.Add("No. Prepedido", 100, HorizontalAlignment.Left)
        LV.Columns.Add("Cliente", 80, HorizontalAlignment.Left)
        LV.Columns.Add("Nombre ó Razón social", 240, HorizontalAlignment.Left)
        LV.Columns.Add("Emisión", 60, HorizontalAlignment.Center)
        LV.Columns.Add("Total", 70, HorizontalAlignment.Right)
        LV.Columns.Add("Peso (Kgr.)", 70, HorizontalAlignment.Right)
        LV.Columns.Add("Grupo", 70, HorizontalAlignment.Left)

        For iCont = 0 To dt.Rows.Count - 1
            LV.Items.Add(dt.Rows(iCont).Item("numped").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("codcli").ToString)
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("nombre").ToString)
            LV.Items(iCont).SubItems.Add(FormatoFechaCorta(CDate(dt.Rows(iCont).Item("emision").ToString)))
            LV.Items(iCont).SubItems.Add(FormatoNumero(CDbl(dt.Rows(iCont).Item("tot_ped").ToString)))
            LV.Items(iCont).SubItems.Add(FormatoCantidad(CDbl(dt.Rows(iCont).Item("kilos").ToString)))
            LV.Items(iCont).SubItems.Add(dt.Rows(iCont).Item("comen").ToString)
        Next

        LV.EndUpdate()

    End Sub


    Public Sub CargaListViewFacturasGuiaDespacho(ByVal LV As ListView, ByVal dt As DataTable, Optional ByVal TipoRelacion As Integer = 0)

        'TIPO RELACION    0 = FACTURAS ; 1 = NCR ; 2 = PEDIDOS

        Dim iCont As Integer

        LV.Clear()

        LV.BeginUpdate()

        LV.Columns.Add("No. Factura", 80, HorizontalAlignment.Left)
        LV.Columns.Add("Emisión", 60, HorizontalAlignment.Center)
        LV.Columns.Add("Cliente", 90, HorizontalAlignment.Left)
        LV.Columns.Add("Nombre ó Razón social", 240, HorizontalAlignment.Left)
        LV.Columns.Add("Total", 70, HorizontalAlignment.Right)
        LV.Columns.Add("Peso (Kgr.)", 70, HorizontalAlignment.Right)
        LV.Columns.Add("Asesor", 40, HorizontalAlignment.Center)
        LV.Columns.Add("Grupo", 40, HorizontalAlignment.Center)

        For iCont = 0 To dt.Rows.Count - 1

            With dt.Rows(iCont)
                LV.Items.Add(.Item("numfac").ToString)
                LV.Items(iCont).SubItems.Add(FormatoFechaCorta(CDate(.Item("emision").ToString)))
                LV.Items(iCont).SubItems.Add(.Item("codcli").ToString)
                LV.Items(iCont).SubItems.Add(.Item("nombre").ToString)
                Select Case TipoRelacion
                    Case 0
                        LV.Items(iCont).SubItems.Add(FormatoNumero(CDbl(.Item("tot_fac").ToString)))
                    Case 1
                        LV.Items(iCont).SubItems.Add(FormatoNumero(CDbl(.Item("tot_ncr").ToString)))
                    Case 2
                        LV.Items(iCont).SubItems.Add(FormatoNumero(CDbl(.Item("tot_PED").ToString)))
                End Select

                LV.Items(iCont).SubItems.Add(FormatoCantidad(CDbl(.Item("kilos").ToString)))
                LV.Items(iCont).SubItems.Add(.Item("codven").ToString)
                LV.Items(iCont).SubItems.Add(.Item("comen").ToString)
            End With
        Next

        LV.EndUpdate()

    End Sub

    

    Public Sub CargaListViewImprimirFacturasGuiaDespacho(ByVal LV As ListView, ByVal dt As DataTable)

        Dim iCont As Integer

        LV.Clear()

        LV.BeginUpdate()

        LV.Columns.Add("No. Factura", 80, HorizontalAlignment.Left)
        LV.Columns.Add("Emisión", 60, HorizontalAlignment.Center)
        LV.Columns.Add("Cliente", 90, HorizontalAlignment.Left)
        LV.Columns.Add("Nombre ó Razón social", 240, HorizontalAlignment.Left)
        LV.Columns.Add("Total", 70, HorizontalAlignment.Right)
        LV.Columns.Add("Peso (Kgr.)", 70, HorizontalAlignment.Right)
        LV.Columns.Add("Asesor", 40, HorizontalAlignment.Center)


        For iCont = 0 To dt.Rows.Count - 1

            With dt.Rows(iCont)
                LV.Items.Add(.Item("codigofac").ToString)
                LV.Items(iCont).SubItems.Add(FormatoFechaCorta(CDate(.Item("emision").ToString)))
                LV.Items(iCont).SubItems.Add(.Item("codcli").ToString)
                LV.Items(iCont).SubItems.Add(.Item("nomcli").ToString)
                LV.Items(iCont).SubItems.Add(FormatoNumero(CDbl(.Item("totalfac").ToString)))
                LV.Items(iCont).SubItems.Add(FormatoCantidad(CDbl(.Item("kilosfac").ToString)))
                LV.Items(iCont).SubItems.Add(.Item("codven").ToString)
            End With
        Next

        LV.EndUpdate()

    End Sub

    Public Sub CargarListaSaldosDocumentosCliente(ByVal dg As DataGridView, ByVal dtdepos As DataTable)

        Dim aFld() As String = {"sel", "nummov", "tipomov", "emision", "vence", "importe", "saldo", "codven", "nomVendedor"}
        Dim aNom() As String = {"", "Documento", "TP", "Emisión", "Vence", "Importe", "Saldo", "", "Asesor"}
        Dim aAnc() As Long = {20, 110, 35, 90, 90, 110, 110, 45, 110}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, _
                                     AlineacionDataGrid.Derecha, AlineacionDataGrid.Derecha, AlineacionDataGrid.Centro, _
                                     AlineacionDataGrid.Izquierda}
        Dim aFor() As String = {"", "", "", sFormatoFechaCorta, sFormatoFechaCorta, sFormatoNumero, sFormatoNumero, "", ""}


        IniciarTablaSeleccion(dg, dtdepos, aFld, aNom, aAnc, aAli, aFor, , True)


    End Sub



    Public Sub CargaLVSaldosDocumentoCliente(ByVal LV As ListView, ByVal dt As DataTable)
        Dim iCont As Integer

        LV.Clear()

        LV.BeginUpdate()

        LV.Columns.Add("No. Documento", 120, HorizontalAlignment.Left)
        LV.Columns.Add("Tipo", 50, HorizontalAlignment.Center)
        LV.Columns.Add("Emisión", 90, HorizontalAlignment.Center)
        LV.Columns.Add("Vence", 90, HorizontalAlignment.Center)
        LV.Columns.Add("Importe", 90, HorizontalAlignment.Right)
        LV.Columns.Add("Saldo", 90, HorizontalAlignment.Right)
        LV.Columns.Add("Asesor", 50, HorizontalAlignment.Center)
        LV.Columns.Add("Nombre Asesor", 200, HorizontalAlignment.Left)

        For iCont = 0 To dt.Rows.Count - 1
            With dt.Rows(iCont)
                LV.Items.Add(.Item("nummov").ToString)
                LV.Items(iCont).SubItems.Add(.Item("tipomov").ToString)
                LV.Items(iCont).SubItems.Add(FormatoFechaCorta(CDate(.Item("emision").ToString)))
                LV.Items(iCont).SubItems.Add(FormatoFechaCorta(CDate(.Item("vence").ToString)))
                LV.Items(iCont).SubItems.Add(FormatoNumero(CDbl(.Item("importe").ToString)))
                LV.Items(iCont).SubItems.Add(FormatoCantidad(CDbl(.Item("saldo").ToString)))
                LV.Items(iCont).SubItems.Add(.Item("codven").ToString)
                LV.Items(iCont).SubItems.Add(.Item("nomVendedor").ToString)
            End With

        Next

        LV.EndUpdate()

    End Sub
    Public Sub ActualizarRenglonesEnPresupuestos(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, ByVal dtRenglon As DataTable, _
                                                 ByVal nombreTablaOrigen As String)

        Dim NumeroPresupuesto As String
        Dim sItem As String
        Dim RenglonPresupuesto As String
        Dim gCont As Integer

        For gCont = 0 To dtRenglon.Rows.Count - 1
            With dtRenglon.Rows(gCont)
                NumeroPresupuesto = IIf(Not IsDBNull(.Item("NUMCOT")), .Item("NUMCOT"), "")
                RenglonPresupuesto = IIf(Not IsDBNull(.Item("RENCOT")), .Item("RENCOT"), "")
                sItem = .Item("ITEM")
                If NumeroPresupuesto <> "" Then
                    If .Item("ESTATUS") < 2 Then
                        ActualizaCantidadTransitoEnRenglon(MyConn, lblInfo, ds, "jsvenrencot", "NUMCOT", NumeroPresupuesto, _
                                                           RenglonPresupuesto, sItem, .Item("CANTIDAD"), _
                                                           .Item("UNIDAD"), .Item("ESTATUS"))

                        ActualizaEstatusDocumento(MyConn, lblInfo, ds, "jsvenenccot", "jsvenrencot", "NUMCOT", NumeroPresupuesto)
                    End If
                End If
            End With
        Next

    End Sub
    Public Sub ActualizarRenglonesEnPrepedidos(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, _
                                           ByVal dtRenglon As DataTable, ByVal sTablaOrigen As String)

        Dim NumeroPedido As String
        Dim RenglonPedido As String

        If dtRenglon.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtRenglon.Rows.Count - 1
                With dtRenglon.Rows(iCont)
                    NumeroPedido = IIf(Not IsDBNull(.Item("numped")), .Item("numped"), "")
                    RenglonPedido = IIf(Not IsDBNull(.Item("renped")), .Item("renped"), "")
                    If NumeroPedido <> "" Then
                        ActualizaCantidadTransitoEnRenglon(MyConn, lblInfo, ds, "jsvenrenpedrgv", "NUMPED", NumeroPedido, _
                                                           RenglonPedido, .Item("item"), .Item("CANTIDAD"), _
                                                           .Item("UNIDAD"), .Item("ESTATUS"))

                        ActualizaEstatusDocumento(MyConn, lblInfo, ds, "jsvenencpedrgv", "jsvenrenpedrgv", "NUMPED", NumeroPedido)
                    End If
                End With
            Next
        End If

    End Sub

    Public Sub ActualizarRenglonesEnPedidos(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, _
                                            ByVal dtRenglon As DataTable, ByVal sTablaOrigen As String)

        Dim NumeroPedido As String
        Dim RenglonPedido As String

        If dtRenglon.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtRenglon.Rows.Count - 1
                With dtRenglon.Rows(iCont)
                    NumeroPedido = IIf(Not IsDBNull(.Item("numped")), .Item("numped"), "")
                    RenglonPedido = IIf(Not IsDBNull(.Item("renped")), .Item("renped"), "")
                    If NumeroPedido <> "" Then
                        ActualizaCantidadTransitoEnRenglon(MyConn, lblInfo, ds, "jsvenrenped", "NUMPED", NumeroPedido, _
                                                           RenglonPedido, .Item("item"), .Item("CANTIDAD"), _
                                                           .Item("UNIDAD"), .Item("ESTATUS"))

                        ActualizaEstatusDocumento(MyConn, lblInfo, ds, "jsvenencped", "jsvenrenped", "NUMPED", NumeroPedido)
                    End If
                End With
            Next
        End If

    End Sub

    Public Sub ActualizarRenglonesEnNotasDeEntrega(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, _
                                        ByVal dtRenglon As DataTable, ByVal sTablaOrigen As String)

        Dim NumeroNotaDeEntrega As String
        Dim RenglonNotaDeEntrega As String

        If dtRenglon.Rows.Count > 0 Then
            Dim iCont As Integer
            For iCont = 0 To dtRenglon.Rows.Count - 1
                With dtRenglon.Rows(iCont)
                    NumeroNotaDeEntrega = IIf(Not IsDBNull(.Item("numnot")), .Item("numnot"), "")
                    RenglonNotaDeEntrega = IIf(Not IsDBNull(.Item("rennot")), .Item("rennot"), "")
                    If NumeroNotaDeEntrega <> "" Then
                        ActualizaCantidadTransitoEnRenglon(MyConn, lblInfo, ds, "jsvenrennot", "NUMFAC", NumeroNotaDeEntrega, _
                                                           RenglonNotaDeEntrega, .Item("item"), .Item("CANTIDAD"), _
                                                           .Item("UNIDAD"), .Item("ESTATUS"))

                        ActualizaEstatusDocumento(MyConn, lblInfo, ds, "jsvenencnot", "jsvenrennot", "NUMFAC", NumeroNotaDeEntrega)
                    End If
                End With
            Next
        End If

    End Sub


    Public Sub ActualizaCantidadTransitoEnRenglon(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, _
                                                  ByVal NombreTablaEnBD As String, ByVal CampoDocumentoEnBD As String, ByVal NumeroDocumento As String, _
                                                  ByVal NumeroRenglon As String, ByVal Item As String, ByVal Cantidad As Double, _
                                                  ByVal UnidadOrigen As String, ByVal Estatus As String)


        Dim dtAct As DataTable
        Dim nTablaAct As String = "tblAct"
        Dim CantidadReal As Double

        ds = DataSetRequery(ds, " select * from " & NombreTablaEnBD & " Where " _
                  & " " & CampoDocumentoEnBD & " = '" & NumeroDocumento & "' AND " _
                  & " RENGLON = '" & NumeroRenglon & "' AND " _
                  & " ITEM = '" & Item & "' AND " _
                  & " ESTATUS = '" & Estatus & "' AND " _
                  & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                  & " ID_EMP = '" & jytsistema.WorkID & "' ", MyConn, nTablaAct, lblInfo)

        dtAct = ds.Tables(nTablaAct)

        If dtAct.Rows.Count > 0 Then

            CantidadReal = ConversiondeUnidades(MyConn, lblInfo, ds, Item, UnidadOrigen, Cantidad, dtAct.Rows(0).Item("UNIDAD"))

            EjecutarSTRSQL(MyConn, lblInfo, _
                " UPDATE " & NombreTablaEnBD & " set " _
                & " CANTRAN =  CANTIDAD - " & CantidadReal & " WHERE " _
                & " " & CampoDocumentoEnBD & " = '" & NumeroDocumento & "' AND " _
                & " RENGLON = '" & NumeroRenglon & "' AND " _
                & " ITEM = '" & Item & "' AND " _
                & " ESTATUS = '" & Estatus & "' AND " _
                & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                & " ID_EMP = '" & jytsistema.WorkID & "' ")
        End If

        dtAct.Dispose()
        dtAct = Nothing

    End Sub

    Function CarteraMercancias(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoAsesor As String) As Integer
        CarteraMercancias = CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsvencuoart where codven = '" & CodigoAsesor & "' and id_emp = '" & jytsistema.WorkID & "' "))
    End Function
    Function CarteraGrupo(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoAsesor As String, _
                          ByVal Grupo As String) As Integer

        Dim Cartera As Object = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " SELECT COUNT(DISTINCT b." & Grupo & ") " _
                                                      & " FROM jsvencuoart a " _
                                                      & " LEFT JOIN jsmerctainv b ON (a.codart = b.codart AND a.id_emp = b.id_emp) " _
                                                      & " WHERE " _
                                                      & " a.codven = '" & CodigoAsesor & "' AND " _
                                                      & " a.id_emp = '" & jytsistema.WorkID & "'")

        CarteraGrupo = CInt(IIf(TypeOf Cartera Is DBNull, 0, Cartera))

    End Function
    Public Sub ActualizaEstatusDocumento(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, _
                                       ByVal sTablaEncabezado As String, ByVal sTablaRenglones As String, ByVal Documento As String, ByVal NumeroDocumento As String)

        Dim dtEst As DataTable
        Dim nTablaEstatus As String = "tblEstatus"

        ds = DataSetRequery(ds, " SELECT SUM(IF(CANTRAN > 0, CANTRAN,0)) AS SUMA FROM " & sTablaRenglones & " WHERE" _
                    & " " & Documento & " = '" & NumeroDocumento & " ' AND " _
                    & " ACEPTADO = '1' AND " _
                    & " ID_EMP = '" & jytsistema.WorkID & "' " _
                    & " GROUP BY " & Documento & " ", MyConn, nTablaEstatus, lblInfo)
        dtEst = ds.Tables(nTablaEstatus)

        If dtEst.Rows.Count > 0 Then
            With dtEst.Rows(0)
                If .Item("SUMA") > 0 Then
                    EjecutarSTRSQL(MyConn, lblInfo, " UPDATE " & sTablaEncabezado & " set ESTATUS = '0' where " _
                        & " " & Documento & " = '" & NumeroDocumento & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' ")
                Else
                    EjecutarSTRSQL(MyConn, lblInfo, " UPDATE " & sTablaEncabezado & " set ESTATUS = '1' where " _
                        & " " & Documento & " = '" & NumeroDocumento & "' AND " _
                        & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                        & " ID_EMP = '" & jytsistema.WorkID & "' ")
                End If
            End With
        Else
            EjecutarSTRSQL(MyConn, lblInfo, " UPDATE " & sTablaEncabezado & " set ESTATUS = '1' where " _
              & " " & Documento & " = '" & NumeroDocumento & "' AND " _
              & " EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
              & " ID_EMP = '" & jytsistema.WorkID & "' ")
        End If

        dtEst.Dispose()
        dtEst = Nothing

    End Sub

    Function NumeroLineasEnNotaCredito(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                  ByVal NumeroNotaCredito As String) As Integer
        'Lineas para comienzo de factura 
        NumeroLineasEnNotaCredito = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENNCRPA18"))

        'Lineas de Encabezado de Factura
        NumeroLineasEnNotaCredito += 9
        'Número de Renglones
        NumeroLineasEnNotaCredito += 2 * CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsvenrenncr where numncr = '" & NumeroNotaCredito & "' and id_emp = '" & jytsistema.WorkID & "' "))

        'Subtotal
        NumeroLineasEnNotaCredito += 2

        'Número renglones de iva
        NumeroLineasEnNotaCredito += CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsvenivancr where numncr = '" & NumeroNotaCredito & "' and id_emp = '" & jytsistema.WorkID & "' "))

        'Firma y Sello (2), Monto total (1), Forma pago (1), Monto Total Letras (1)
        NumeroLineasEnNotaCredito += 5

        'Comentarios x Factura
        NumeroLineasEnNotaCredito += CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsconctacom where origen = 'NCR' and id_emp = '" & jytsistema.WorkID & "' "))

    End Function



    Function NumeroLineasEnFactura(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                   ByVal NumeroFactura As String) As Integer
        'Lineas para comienzo de factura 
        NumeroLineasEnFactura = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENFACPA18"))
        'Lineas de Encabezado de Factura
        NumeroLineasEnFactura += 9
        'Número de Renglones
        NumeroLineasEnFactura += CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsvenrenfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' "))

        'Comentarios de Renglones 
        NumeroLineasEnFactura += NumeroLineasPorComentarios(MyConn, lblInfo, NumeroFactura, "FAC")
        'Cantidad Mercancias de combo + 1 

        'Subtotal
        NumeroLineasEnFactura += 2
        'Descuentos (+2)
        NumeroLineasEnFactura += CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsvendesfac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' "))
        NumeroLineasEnFactura += 2
        'Número renglones de iva
        NumeroLineasEnFactura += CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsvenivafac where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' "))

        'Firma y Sello (2), Monto total (1), Forma pago (1), Monto Total Letras (1)
        NumeroLineasEnFactura += 5

        'Comentarios x Factura
        NumeroLineasEnFactura += CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsconctacom where origen = 'FAC' and id_emp = '" & jytsistema.WorkID & "' "))

    End Function
    Function NumeroLineasEnFacturaPOS(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                  ByVal NumeroFactura As String) As Integer
        'Lineas para comienzo de factura 
        NumeroLineasEnFacturaPOS = CInt(ParametroPlus(MyConn, Gestion.iPuntosdeVentas, "POSPARAM29"))
        'Lineas de Encabezado de Factura
        NumeroLineasEnFacturaPOS += 9
        'Número de Renglones
        NumeroLineasEnFacturaPOS += CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsvenrenpos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' "))

        'Comentarios de Renglones 
        NumeroLineasEnFacturaPOS += NumeroLineasPorComentarios(MyConn, lblInfo, NumeroFactura, "PVE")
        'Cantidad Mercancias de combo + 1 

        'Subtotal
        NumeroLineasEnFacturaPOS += 2
        'Descuentos (+2)
        NumeroLineasEnFacturaPOS += CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsvendespos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' "))
        NumeroLineasEnFacturaPOS += 2
        'Número renglones de iva
        NumeroLineasEnFacturaPOS += CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsvenivapos where numfac = '" & NumeroFactura & "' and id_emp = '" & jytsistema.WorkID & "' "))

        'Firma y Sello (2), Monto total (1), Forma pago (1), Monto Total Letras (1)
        NumeroLineasEnFacturaPOS += 5

        'Comentarios x Factura
        NumeroLineasEnFacturaPOS += CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsconctacom where origen = 'PVE' and id_emp = '" & jytsistema.WorkID & "' "))

    End Function

    Function NumeroLineasEnNotasDebito(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                   ByVal NumeroNotaDebito As String) As Integer
        'Lineas para comienzo de Nota Debito 
        NumeroLineasEnNotasDebito = CInt(ParametroPlus(MyConn, Gestion.iVentas, "VENNDBPA20"))

        'Lineas de Encabezado de Nota Debito
        NumeroLineasEnNotasDebito += 9

        ''Número de Renglones
        NumeroLineasEnNotasDebito += CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsvenrenndb where numndb = '" & NumeroNotaDebito & "' and id_emp = '" & jytsistema.WorkID & "' "))

        ''Comentarios de Renglones 
        NumeroLineasEnNotasDebito += NumeroLineasPorComentarios(MyConn, lblInfo, NumeroNotaDebito, "NDB", 80)

        ''Subtotal
        NumeroLineasEnNotasDebito += 2

        ''Número renglones de iva
        NumeroLineasEnNotasDebito += CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsvenivandb where numndb = '" & NumeroNotaDebito & "' and id_emp = '" & jytsistema.WorkID & "' "))

        ''Firma y Sello (2), Monto total (1), Forma pago (1), Monto Total Letras (1)
        NumeroLineasEnNotasDebito += 5

        ''Comentarios x Nota Debito
        NumeroLineasEnNotasDebito += CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select count(*) from jsconctacom where origen = 'NDB' and id_emp = '" & jytsistema.WorkID & "' "))

    End Function


    Function NumeroLineasPorComentarios(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                          ByVal NumeroDocumento As String, ByVal Origen As String, Optional ByVal numCaracteresEnLinea As Integer = 60) As Integer

        'Para imprimir comentarios de renglones se partiran los mismos en cadenas de 60 caracteres
        NumeroLineasPorComentarios = 0
        Dim dsCom As New DataSet
        Dim nTableCom As String = "tblComentario"
        Dim dtCom As DataTable

        dsCom = DataSetRequery(dsCom, " select * from jsvenrencom where origen = '" & Origen & "' and numdoc = '" & NumeroDocumento & "' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTableCom, lblInfo)
        dtCom = dsCom.Tables(nTableCom)

        Dim iCont As Integer
        For iCont = 0 To dtCom.Rows.Count - 1
            With dtCom.Rows(iCont)
                Dim nLineaAdicional As Double = CStr(.Item("comentario")).Length Mod numCaracteresEnLinea
                Dim nComens As Integer = Int(CStr(.Item("comentario")).Length / numCaracteresEnLinea) + IIf(nLineaAdicional = 0, 0, 1)
                NumeroLineasPorComentarios += nComens
            End With
        Next

        dsCom.Dispose()
        dsCom = Nothing

    End Function


    Function ClienteFacturaAPartirDeCostos(MyConn As MySqlConnection, lblInfo As Label, CodigoCliente As String) As Boolean
        Return CBool(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select codcre from jsvencatcli where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' "))
    End Function


    Public Sub VerificaVencimientoCliente(MyConn As MySqlConnection, lblInfo As Label, ds As DataSet, CodigoCliente As String, Estatus As String, _
                                   DiasVencimiento As Integer, CausaBloqueo As String, NoFacturarAgain As Boolean)

        Dim dts As DataTable
        Dim nTable As String = "tblFActurasPendientes"
        ds = DataSetRequery(ds, "SELECT a.CODCLI, b.NOMBRE, a.NUMMOV, a.TIPOMOV, a.REFER, " _
            & " a.EMISION, a.VENCE, a.IMPORTE, SUM(a.importe) AS SALDO, " _
            & " to_days('" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "') - to_Days(a.vence) as DV, " _
            & " if( to_days('" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "') - to_Days(a.vence)>= " & DiasVencimiento & ", 1, 0) as lapso " _
            & " from jsventracob a LEFT JOIN jsvencatcli b ON  b.CODCLI = a.CODCLI AND b.ID_EMP = a.ID_EMP and " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' Where a.ID_EMP = '" & jytsistema.WorkID & "' " _
            & " and a.tipomov <> 'ND' " _
            & " and a.codcli = '" & CodigoCliente & "' " _
            & " GROUP BY a.codcli, a.nummov Having Saldo > 1 AND a.vence < '" & FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' and lapso = 1 " _
            & " ORDER BY LAPSO, a.CODCLI, NUMMOV, EMISION ASC ", MyConn, nTable, lblInfo)


        dts = ds.Tables(nTable)

        If dts.Rows.Count > 0 Then
            If Estatus = "0" Then
                InsertEditVENTASExpedienteCliente(MyConn, lblInfo, True, CodigoCliente, CDate(FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & " " & FormatoHora(Now())), _
                                                  "FACTURAS PENDIENTES", 1, CausaBloqueo, 0)

                EjecutarSTRSQL(MyConn, lblInfo, " update jsvencatcli set estatus = '1' where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
            End If

        End If

        If NoFacturarAgain Then
            EjecutarSTRSQL(MyConn, lblInfo, " update jsvencatcli set backorder = '0' where codcli = '" & CodigoCliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
        End If

        dts.Dispose()
        dts = Nothing

    End Sub
    Public Function ProcesoAutomaticoInicial(MyConn As MySqlConnection, lblInfo As Label, Proceso As Integer, Fecha As Date) As Boolean

        Return CBool(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select 1 from  jscontrapro where  " _
                                           & " proceso = '" & Format(Proceso, "00") & "' and " _
                                           & " fecha = '" & FormatoFechaMySQL(Fecha) & "' and " _
                                           & " id_emp = '" & jytsistema.WorkID & "' "))

    End Function

    Public Sub ActualizarProcesoAutomaticoInicial(MyConn As MySqlConnection, lblInfo As Label, proceso As ProcesoAutomatico, Fecha As Date)
        Dim proc As String = RellenaCadenaConCaracter(proceso, "D", 2, "0")
        EjecutarSTRSQL(MyConn, lblInfo, " insert into jscontrapro set proceso = '" & proc & "', fecha = '" & FormatoFechaMySQL(Fecha) & "', id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

    Public Function NumeroDeClientesAVisitarMes(MyConn As MySqlConnection, lblInfo As Label, ds As DataSet, _
                                                CodigoAsesor As String, FechaInicial As Date, FechaFinal As Date) As Integer

        Dim nClientes As Integer = 0

        Dim nDiasEnPeriodo As Integer = NumeroDeDiasEnPeriodo(FechaInicial, FechaFinal, DayOfWeek.Monday)
        Dim nDiasEnPeriodoNoLaborables As Integer = NumeroDeDiasEnPeriodoNoLaborables(MyConn, lblInfo, ds, FechaInicial, FechaFinal, DayOfWeek.Monday)
        Dim nClientesAVisitarDia As Integer = ClientesAVisitarEnDia(MyConn, lblInfo, CodigoAsesor, DayOfWeek.Monday)

        nClientes += (nDiasEnPeriodo - _
            nDiasEnPeriodoNoLaborables) * nClientesAVisitarDia


        nClientes += (NumeroDeDiasEnPeriodo(FechaInicial, FechaFinal, DayOfWeek.Tuesday) - _
           NumeroDeDiasEnPeriodoNoLaborables(MyConn, lblInfo, ds, FechaInicial, FechaFinal, DayOfWeek.Tuesday)) * _
           ClientesAVisitarEnDia(MyConn, lblInfo, CodigoAsesor, DayOfWeek.Tuesday)

        nClientes += (NumeroDeDiasEnPeriodo(FechaInicial, FechaFinal, DayOfWeek.Wednesday) - _
            NumeroDeDiasEnPeriodoNoLaborables(MyConn, lblInfo, ds, FechaInicial, FechaFinal, DayOfWeek.Wednesday)) * _
            ClientesAVisitarEnDia(MyConn, lblInfo, CodigoAsesor, DayOfWeek.Wednesday)

        nClientes += (NumeroDeDiasEnPeriodo(FechaInicial, FechaFinal, DayOfWeek.Thursday) - _
           NumeroDeDiasEnPeriodoNoLaborables(MyConn, lblInfo, ds, FechaInicial, FechaFinal, DayOfWeek.Thursday)) * _
           ClientesAVisitarEnDia(MyConn, lblInfo, CodigoAsesor, DayOfWeek.Thursday)

        nClientes += (NumeroDeDiasEnPeriodo(FechaInicial, FechaFinal, DayOfWeek.Friday) - _
            NumeroDeDiasEnPeriodoNoLaborables(MyConn, lblInfo, ds, FechaInicial, FechaFinal, DayOfWeek.Friday)) * _
            ClientesAVisitarEnDia(MyConn, lblInfo, CodigoAsesor, DayOfWeek.Friday)

        nClientes += (NumeroDeDiasEnPeriodo(FechaInicial, FechaFinal, DayOfWeek.Saturday) - _
            NumeroDeDiasEnPeriodoNoLaborables(MyConn, lblInfo, ds, FechaInicial, FechaFinal, DayOfWeek.Saturday)) * _
            ClientesAVisitarEnDia(MyConn, lblInfo, CodigoAsesor, DayOfWeek.Saturday)

        nClientes += (NumeroDeDiasEnPeriodo(FechaInicial, FechaFinal, DayOfWeek.Sunday) - _
            NumeroDeDiasEnPeriodoNoLaborables(MyConn, lblInfo, ds, FechaInicial, FechaFinal, DayOfWeek.Sunday)) * _
            ClientesAVisitarEnDia(MyConn, lblInfo, CodigoAsesor, DayOfWeek.Sunday)

        Return nClientes

    End Function

    Public Function ClientesAVisitarEnDia(MyConn As MySqlConnection, lblInfo As Label, CodigoAsesor As String, Dia As DayOfWeek) As Integer

        Return CInt(EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select items from jsvenencrut where codven = '" & CodigoAsesor _
                                 & "' and dia = " & IIf(Dia = 0, 7, Dia - 1) & " AND tipo = '0' and id_emp = '" & jytsistema.WorkID & "' "))

    End Function

    Public Function ClientesAVisitarSupervisor(MyConn As MySqlConnection, lblInfo As Label, CodigoAsesor As String, Fecha As Date) As Integer
        Dim nVendedores As New ArrayList
        nVendedores = EjecutarSTRSQL_Reader(MyConn, lblInfo, "SELECT codven FROM jsvencatven WHERE supervisor = '" & CodigoAsesor & "' and id_emp = '" & jytsistema.WorkID & "' ")

        ClientesAVisitarSupervisor = 0
        For Each nVen As Object In nVendedores
            ClientesAVisitarSupervisor += EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select avisitar from jsvenenccie where codven = '" & nVen.ToString & "' and YEAR(FECHA) = " & Fecha.Year & " AND month(fecha) = " & Fecha.Month & " and id_emp = '" & jytsistema.WorkID & "' ")
        Next

    End Function

    Public Function AsesoresDeSupervisor(MyConn As MySqlConnection, lblInfo As Label, CodigoSupervisor As String) As String
        Dim nVendedores As New ArrayList
        nVendedores = EjecutarSTRSQL_Reader(MyConn, lblInfo, "SELECT codven FROM jsvencatven WHERE supervisor = '" & CodigoSupervisor & "' and id_emp = '" & jytsistema.WorkID & "' ")

        AsesoresDeSupervisor = " ( "
        For Each nVen As Object In nVendedores
            AsesoresDeSupervisor += "'" & nVen.ToString & "', "
        Next
        If Right(AsesoresDeSupervisor, 2) = ", " Then AsesoresDeSupervisor = Mid(AsesoresDeSupervisor, 1, Len(AsesoresDeSupervisor) - 2)
        AsesoresDeSupervisor += " ) "

        If AsesoresDeSupervisor = " (  ) " Then AsesoresDeSupervisor = ""


    End Function

    Function montoFletesPorRegion(MyConn As MySqlConnection, Cliente As String) As Double

        montoFletesPorRegion = 0.0

        Dim fiscalPais As String = EjecutarSTRSQL_ScalarPLUS(MyConn, " select FPAIS from jsvencatcli where codcli = '" & Cliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim fiscalEstado As String = EjecutarSTRSQL_ScalarPLUS(MyConn, " select FESTADO from jsvencatcli where codcli = '" & Cliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim fiscalMunicipio As String = EjecutarSTRSQL_ScalarPLUS(MyConn, " select FMUNICIPIO from jsvencatcli where codcli = '" & Cliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim fiscalParroquia As String = EjecutarSTRSQL_ScalarPLUS(MyConn, " select FPARROQUIA from jsvencatcli where codcli = '" & Cliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim fiscalCiudad As String = EjecutarSTRSQL_ScalarPLUS(MyConn, " select FCIUDAD from jsvencatcli where codcli = '" & Cliente & "' and id_emp = '" & jytsistema.WorkID & "' ")
        Dim fiscalBarrio As String = EjecutarSTRSQL_ScalarPLUS(MyConn, " select FBARRIO from jsvencatcli where codcli = '" & Cliente & "' and id_emp = '" & jytsistema.WorkID & "' ")

        Dim monto As Double = CDbl(EjecutarSTRSQL_ScalarPLUS(MyConn, " SELECT montoflete FROM jsconcatter WHERE codigo = " & fiscalPais & " "))
        If monto <> 0.0 Then montoFletesPorRegion = monto

        monto = CDbl(EjecutarSTRSQL_ScalarPLUS(MyConn, " SELECT montoflete FROM jsconcatter WHERE codigo = " & fiscalEstado & " "))
        If monto <> 0.0 Then montoFletesPorRegion = monto

        monto = CDbl(EjecutarSTRSQL_ScalarPLUS(MyConn, " SELECT montoflete FROM jsconcatter WHERE codigo = " & fiscalMunicipio & " "))
        If monto <> 0.0 Then montoFletesPorRegion = monto

        monto = CDbl(EjecutarSTRSQL_ScalarPLUS(MyConn, " SELECT montoflete FROM jsconcatter WHERE codigo = " & fiscalParroquia & " "))
        If monto <> 0.0 Then montoFletesPorRegion = monto

        monto = CDbl(EjecutarSTRSQL_ScalarPLUS(MyConn, " SELECT montoflete FROM jsconcatter WHERE codigo = " & fiscalCiudad & " "))
        If monto <> 0.0 Then montoFletesPorRegion = monto

        monto = CDbl(EjecutarSTRSQL_ScalarPLUS(MyConn, " SELECT montoflete FROM jsconcatter WHERE codigo = " & fiscalBarrio & " "))
        If monto <> 0.0 Then montoFletesPorRegion = monto

    End Function


End Module
