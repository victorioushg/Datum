Imports MySql.Data.MySqlClient
Module ConsultasPuntosdeVentas

    Function SeleccionPOSFacturas(ByVal NumeroFacturaDesde As String, ByVal NumeroFacturaHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                  ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                  ByVal CajeroDesde As String, ByVal CajeroHasta As String, _
                                  ByVal CajaDesde As String, ByVal CajaHasta As String) As String

        Dim str As String = ""

        str += " a.emision >= '" & FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' and "
        If CajeroDesde <> "" Then str += " a.codven >= '" & CajeroDesde & "' and "
        If CajeroHasta <> "" Then str += " a.codven <= '" & CajeroHasta & "' and "
        If CajaDesde <> "" Then str += " a.codcaj >= '" & CajaDesde & "' and "
        If CajaHasta <> "" Then str += " a.codcaj <= '" & CajaHasta & "' and "


        SeleccionPOSFacturas = " select a.numfac, a.tipo, a.emision, a.rif, " _
        & " a.nomcli, if( a.tipo = 1, -1, 1)*a.tot_net tot_net, if( a.tipo = 1, -1, 1)*a.descuen descuen, if( a.tipo = 1, -1, 1)*a.imp_iva imp_iva, if( a.tipo = 1, -1, 1)*a.tot_fac tot_fac " _
        & " from jsvenencpos a " _
        & " Where " _
        & str _
        & " a.id_emp  = '" & jytsistema.WorkID & "' order by a." & OrdenadoPor

    End Function
    Public Function SeleccionPOSReporteX(ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                                  ByVal CodigoCajaDesde As String, ByVal CodigoCajaHasta As String, _
                                                  ByVal CodigoCajeroDesde As String, ByVal CodigoCajeroHasta As String) As String

        Dim str As String = ""
        If CodigoCajaDesde <> "" Then str += " a.caja >= '" & CodigoCajaDesde & "' and "
        If CodigoCajaHasta <> "" Then str += " a.caja <= '" & CodigoCajaHasta & "' and "
        If CodigoCajeroDesde <> "" Then str += " a.cajero >= '" & CodigoCajeroDesde & "' and "
        If CodigoCajeroHasta <> "" Then str += " a.cajero <= '" & CodigoCajeroHasta & "' and "

        SeleccionPOSReporteX = " SELECT a.caja, a.fecha, a.origen, a.tipomov, a.nummov, (CASE WHEN a.formpag = 'EF' THEN 'EFECTIVO' " _
                & "                                 WHEN a.formpag = 'CH' then 'CHEQUE' " _
                & "                                 WHEN a.formpag = 'TA' then 'TARJETA' " _
                & "                                 WHEN a.formpag = 'CT' then 'CUPON DE ALIMENTACION' " _
                & "                                 WHEN a.formpag = 'DP' then 'DEPOSITO' " _
                & "                                 WHEN a.formpag = 'TR' then 'TRANSFERENCIA' " _
                & "                                 WHEN a.formpag = 'CR' then 'CREDITO' " _
                & "                                 END) formpag, " _
                & " a.numpag, " _
                & " (CASE WHEN a.formpag = 'EF' THEN '' " _
                & " WHEN a.formpag = 'CH' THEN b.descrip " _
                & " WHEN a.formpag = 'TA' THEN c.nomtar " _
                & " WHEN a.formpag = 'CT' then a.nompag " _
                & " WHEN a.formpag = 'CR' then '' " _
                & " ELSE d.nomban " _
                & " END) nompag, " _
                & " IF( a.tipomov in ('EN', 'CR'), a.importe, -1*a.importe) importe, a.fechacierre, a.cantidad, a.cajero FROM jsventrapos a " _
                & " left join jsconctatab b on (a.nompag = b.codigo and a.id_emp = b.id_emp and b.modulo = '" & FormatoTablaSimple(Modulo.iBancos) & "') " _
                & " left join jsconctatar c on (a.nompag = c.codtar and a.id_emp = c.id_emp )" _
                & " left join jsbancatban d on (a.nompag = d.codban and a.id_emp = d.id_emp )" _
                & " WHERE " _
                & " a.FECHA >= '" & FormatoFechaMySQL(FechaDesde) & "' AND " _
                & " a.fecha <= '" & FormatoFechaMySQL(FechaHasta) & "' and " _
                & str _
                & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND  " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "' ORDER BY NUMMOV "

    End Function

End Module
