Module ConsultasPuntoDeVenta
    Private ft As New Transportables
    Public Function SeleccionPuntodeVentaReporteX(ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                                  ByVal CodigoCaja As String, ByVal CodigoCajero As String) As String

        SeleccionPuntodeVentaReporteX = " SELECT a.caja, a.fecha, a.origen, a.tipomov, a.nummov, (CASE WHEN a.formpag = 'EF' THEN 'EFECTIVO' " _
                & "                                 WHEN a.formpag = 'CH' then 'CHEQUE' " _
                & "                                 WHEN a.formpag = 'TA' then 'TARJETA' " _
                & "                                 WHEN a.formpag = 'CT' then 'CUPON DE ALIMENTACION' " _
                & "                                 WHEN a.formpag = 'DP' then 'DEPOSITO' " _
                & "                                 WHEN a.formpag = 'TR' then 'TRANSFERENCIA' " _
                & "                                 WHEN a.formpag = 'CR' then 'CREDITO' " _
                & "                                 END) formpag, " _
                & " a.numpag, " _
                & " (CASE WHEN a.formpag = 'EF' THEN a.nompag " _
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
                & " a.FECHA >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
                & " a.fecha <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
                & " a.CAJA = '" & CodigoCaja & "' AND  " _
                & " a.CAJERO = '" & CodigoCajero & "' AND  " _
                & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND  " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "' ORDER BY NUMMOV "

    End Function
End Module
