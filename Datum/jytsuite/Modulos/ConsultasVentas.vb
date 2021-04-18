Imports MySql.Data.MySqlClient
Module ConsultasVentas
    Private ft As New Transportables

    Public Function CadenaComplementoClientes(ByVal LetraTabla As String, ByVal ClienteDesde As String, ByVal ClienteHasta As String, _
                                                ByVal Operador As Integer, ByVal ClavePrincipal As String, _
                                                Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                                Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                                Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                                Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                                Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                                Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                                Optional ByVal TipoContribuyente As Integer = 9, Optional ByVal EstatusCliente As Integer = 9) As String

        Dim str As String = ""

        If Operador = 0 Then
            If ClienteDesde <> "" Then str += " " & LetraTabla & "." & ClavePrincipal & " >= '" & ClienteDesde & "' and "
            If ClienteHasta <> "" Then str += " " & LetraTabla & "." & ClavePrincipal & " <= '" & ClienteHasta & "' and "
        Else
            If ClienteDesde <> "" Then str += " " & LetraTabla & "." & ClavePrincipal & " LIKE '%" & ClienteDesde & "%' and "
            If ClienteHasta <> "" Then str += " " & LetraTabla & "." & ClavePrincipal & " LIKE '%" & ClienteHasta & "%' and "
        End If

        If CanalDesde <> "" Then str += " " & LetraTabla & ".categoria >= '" & CanalDesde & "' and "
        If CanalHasta <> "" Then str += " " & LetraTabla & ".categoria <= '" & CanalHasta & "' and "
        If TipoDesde <> "" Then str += " " & LetraTabla & ".unidad >= '" & TipoDesde & "' and "
        If TipoHasta <> "" Then str += " " & LetraTabla & ".unidad <= '" & TipoHasta & "' and "
        If Pais <> "" Then str += " " & LetraTabla & ".fpais = '" & Pais & "' and "
        If Estado <> "" Then str += " " & LetraTabla & ".festado = '" & Estado & "' and "
        If Municipio <> "" Then str += " " & LetraTabla & ".fmunicipio = '" & Municipio & "' and "
        If Parroquia <> "" Then str += " " & LetraTabla & ".fparroquia = '" & Parroquia & "' and "
        If Ciudad <> "" Then str += " " & LetraTabla & ".fciudad = " & Ciudad & " and "
        If Barrio <> "" Then str += " " & LetraTabla & ".fbarrio = " & Barrio & " and "
        If ZonaDesde <> "" Then str += " " & LetraTabla & ".zona >= '" & ZonaDesde & "' and "
        If ZonaHasta <> "" Then str += " " & LetraTabla & ".zona <= '" & ZonaHasta & "' and "
        If RutaDesde <> "" Then str += " " & LetraTabla & ".ruta_visita >= '" & RutaDesde & "' and "
        If RutaHasta <> "" Then str += " " & LetraTabla & ".ruta_visita <= '" & RutaHasta & "' and "
        If EstatusCliente < 4 Then str += " " & LetraTabla & ".estatus = " & EstatusCliente & " and "
        If TipoContribuyente < 4 Then str += " " & LetraTabla & ".especial = " & TipoContribuyente & " and "


        CadenaComplementoClientes = str

    End Function
    
    Function SeleccionVENClientes(ByVal ClienteDesde As String, ByVal ClienteHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                      Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                      Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                      Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                      Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                      Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                      Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                      Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                      Optional ByVal TipoCliente As Integer = 4, Optional ByVal EstatusCliente As Integer = 4, _
                                      Optional Descuentos As Integer = 2) As String


        Dim str As String = CadenaComplementoClientes("a", ClienteDesde, ClienteHasta, Operador, OrdenadoPor, _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, _
                                                      Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        If AsesorDesde <> "" Then str += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.codven <= '" & AsesorHasta & "' and "
        If ZonaDesde <> "" Then str += " a.codzon >= '" & ZonaDesde & "' and "
        If ZonaHasta <> "" Then str += " a.codzon <= '" & ZonaHasta & "' and "
        If RutaDesde <> "" Then str += " a.codrut >= '" & RutaDesde & "' and "
        If RutaHasta <> "" Then str += " a.codrut <= '" & RutaHasta & "' and "
        If Descuentos < 2 Then str += " a.codcre = 0 AND " & IIf(Descuentos = 0, " a.des_cli > 0 AND ", " a.des_cli = 0 AND ")

        SeleccionVENClientes = " select a.CODCLI, a.NOMBRE, a.ALTERNO, " _
                        & " a.RIF, a.NIT, a.DIRFISCAL, a.NOMBRECANAL CANAL, a.NOMBRETIPONEGOCIO TIPONEGOCIO, " _
                        & " a.NOMRUT RUTA, a.NOMZONA ZONA, a.CODVEN VENDEDOR, a.EMAIL1, a.EMAIL2, a.EMAIL3, a.EMAIL4, " _
                        & " a.TELEF1, a.TELEF2, a.CONTACTO, a.TELCON, a.NOMBRE_VENDEDOR ASESOR, " _
                        & " a.REQ_RIF, a.REQ_NIT, a.REQ_CIS, a.REQ_REC, a.REQ_REG, a.REQ_REA, a.REQ_BAN, a.REQ_COM, " _
                        & " a.barriofiscal FBARRIO, a.ciudadfiscal FCIUDAD, a.parroquiafiscal FPARROQUIA, " _
                        & " a.municipiofiscal FMUNICIPIO, a.estadofiscal FESTADO, a.paisfiscal FPAIS, " _
                        & " a.FZIP, a.DIRDESPA, " _
                        & " a.barriodespacho DBARRIO, a.ciudaddespacho DCIUDAD, a.parroquiadespacho DPARROQUIA, " _
                        & " a.municipiodespacho DMUNICIPIO, a.estadodespacho DESTADO, a.paisdespacho DPAIS, " _
                        & " a.DZIP, " _
                        & " a.CODGEO, a.TELEF3, a.FAX, a.GERENTE, a.TELGER, a.NUM_VISITA, a.INGRESO, a.COMENTARIO, a.LIMITECREDITO, a.SALDO, " _
                        & " a.DISPONIBLE, a.ESPECIAL, a.ESTATUS, a.TARIFA, a.LISPRE, a.FORMAPAGO, a.BANCO, a.CTABANCO, a.ID_EMP " _
                        & " from (" & SeleccionGENClientes() & ") a " _
                        & " Where " _
                        & str _
                        & " a.id_emp = '" & jytsistema.WorkID & "'" _
                        & " order by a." & OrdenadoPor & " "

    End Function
    Function SeleccionVENCancelacionFormaPago(NumeroCancelacion As String) As String

        Return " select " _
        & " case a.formpag " _
        & " when 'EF' then 'EFECTIVO' " _
        & " when 'CH' then 'CHEQUE' " _
        & " when 'DP' then 'DEPOSITO' " _
        & " when 'TA' then 'TARJETA' " _
        & " when 'CT' then 'CHEQUE ALIMENTACION' else 'OTRO' " _
        & " end formapag, " _
        & " a.numpag, " _
        & " case a.formpag " _
        & " when 'EF' THEN '' " _
        & " when 'CH' THEN concat('BANCO: ', b.descrip) " _
        & " when 'DP' then CONCAT('BANCO: ', c.nomban) " _
        & " when 'TA' then d.nomtar " _
        & " when 'CT' THEN 'CHEQUE ALIMENTACION' " _
        & " end nompag, a.importe " _
        & " from jsbantracaj a " _
        & " left join jsconctatab b on (a.refpag = b.codigo and a.id_emp = b.id_emp and b.modulo = '00010') " _
        & " LEFT JOIN jsbancatban c on (a.refpag = c.codban and a.ID_EMP = c.id_emp ) " _
        & " left join jsconctatar d on (a.refpag = d.codtar and a.id_emp = d.id_emp) " _
        & " Where " _
        & " a.nummov = '" & NumeroCancelacion & "' and " _
        & " a.id_emp = '" & jytsistema.WorkID & "' union " _
        & " select 'DEPOSITO' formapag, a.numdoc numpag, concat('BANCO : ', b.nomban) nompag, a.importe  " _
        & " from jsbantraban a " _
        & " left join jsbancatban b on (a.codban = b.codban and a.id_emp = b.id_emp) " _
        & " Where " _
        & " a.tipomov = 'DP' and " _
        & " a.numorg = '" & NumeroCancelacion & "' and " _
        & " a.id_emp = '" & jytsistema.WorkID & "' " _
        & " order by 1 "


    End Function
    Function SeleccionVENCancelacionRelacionCT(NumeroCancelacion As String) As String

        Return " select b.descrip corredor, a.MONTO, COUNT(a.monto) CANTIDAD, " _
                & " SUM(a.MONTO) AS MONTOTAL, SUM(a.MONTO-a.COMISION) AS MENOSCOMISION " _
                & " from jsventabtic a " _
                & " left join jsvencestic b on (a.corredor = b.codigo and a.id_emp = b.id_emp ) " _
                & " where " _
                & " a.numcan = '" & NumeroCancelacion & "' and " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " GROUP BY a.CORREDOR, a.MONTO " _
                & " order by a.CORREDOR, a.MONTO"


    End Function
    Function SeleccionVENCancelacionPlus(NumeroCancelacion As String) As String

        Return " SELECT a.comproba, a.emision, a.codcli, b.nombre, a.refer, a.nummov, IF( a.tipomov IN ('NC','ND','FC','GR'),'', a.tipomov) tipomov , a.concepto, 0.00 saldos, SUM(a.IMPORTE) importe, a.id_emp, 1 tipo " _
            & " FROM jsventracob a " _
            & " LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
            & " Where " _
            & " a.comproba = '" & NumeroCancelacion & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " GROUP BY a.comproba " _
            & " UNION " _
            & " SELECT a.comproba, d.emision, a.codcli, c.nombre, a.refer , a.nummov, b.tipomov, b.concepto, b.importe saldos, 0.00 importe, a.id_emp, 0 tipo " _
            & " FROM jsventracobcan a " _
            & " LEFT JOIN jsventracob b ON (a.nummov = b.nummov AND a.id_emp = b.id_emp AND b.tipomov IN ('FC','GR','NC', 'ND') ) " _
            & " LEFT JOIN jsvencatcli c ON (a.codcli = c.codcli AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT a.comproba, a.emision FROM jsventracob a WHERE a.comproba = '" & NumeroCancelacion & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.comproba) d ON (a.comproba = d.comproba)" _
            & " Where " _
            & " a.comproba = '" & NumeroCancelacion & "' AND " _
            & " a.tipomov IN ('FC', 'GR', 'NC', 'ND') AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY tipo, tipomov, emision "


    End Function
    Function SeleccionVENSaldosPorDocumento(ByVal CodigoCliente As String, ByVal ActualesHistoricos As Integer) As String

        Return " SELECT a.codcli, b.nombre, b.rif, a.nummov, a.tipomov, a.refer, a.emision, a.vence, a.importe, " _
            & " IF(c.saldo IS NULL, 0.00, c.saldo) saldo, a.codven, CONCAT(d.nombres, ' ', d.apellidos) nomVendedor " _
            & " FROM jsventracob a  " _
            & " LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN (SELECT codcli, nummov, IFNULL(SUM(IMPORTE),0) saldo " _
            & "            	FROM jsventracob WHERE codcli = '" & CodigoCliente & "' AND id_emp = '" & jytsistema.WorkID & "' GROUP BY nummov HAVING ABS(ROUND(saldo,2)) > 0 ) c ON (a.codcli = c.codcli AND a.nummov = c.nummov ) " _
            & " INNER JOIN (SELECT codcli, nummov, MIN(CONCAT(fechasi, nummov,emision,hora)) minimo " _
            & " FROM jsventracob WHERE historico = '" & ActualesHistoricos & "' AND ID_EMP = '" & jytsistema.WorkID & "' AND CODCLI = '" & CodigoCliente & "' GROUP BY nummov) d ON (CONCAT(a.fechasi,a.nummov,a.emision,a.hora) = d.minimo) " _
            & " LEFT JOIN jsvencatven d ON (a.codven = d.codven AND a.id_emp = d.id_emp AND d.tipo = '0' ) " _
            & " WHERE " _
            & " a.historico = '" & ActualesHistoricos & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' AND " _
            & " a.CODCLI = '" & CodigoCliente & "' " _
            & " ORDER BY a.fechasi, a.nummov, a.emision, a.hora  "


    End Function
    Function SeleccionVENListadoPedidos(ByVal NumeroPedidoDesde As String, ByVal NumeroPedidoHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                          ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal ClienteDesde As String, ByVal ClienteHasta As String, _
                                          Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                          Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                          Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                          Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                          Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                          Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                          Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                          Optional ByVal TipoCliente As Integer = 0, Optional ByVal EstatusCliente As Integer = 0, _
                                          Optional ByVal CondicionPago As Integer = 2, Optional ByVal EstatusFacturas As Integer = 3) As String

        Dim str As String = CadenaComplementoClientes("c", ClienteDesde, ClienteHasta, Operador, "codcli", _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, _
                                                      Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        str += " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and "
        If AsesorDesde <> "" Then str += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.codven <= '" & AsesorHasta & "' and "

        If EstatusFacturas < 3 Then str += " a.estatus = " & EstatusFacturas & " and "
        If CondicionPago < 2 Then str += " a.condpag = " & CondicionPago & " and "

        Return " select a.numped numfac, a.emision, a.codcli, c.nombre, c.rif, " _
        & " c.nombrecanal canal, c.nombretiponegocio tiponegocio, c.nomzona zona, c.nomrut ruta, " _
        & " a.codven, concat(c.codven, ' ', c.nombre_vendedor)  asesor, " _
        & " c.paisfiscal pais, c.estadofiscal estado, c.municipiofiscal municipio, c.parroquiafiscal parroquia, " _
        & " c.ciudadfiscal ciudad, c.barriofiscal barrio, " _
        & " a.tot_net, a.descuen, a.cargos, a.imp_iva, a.tot_ped tot_fac, 0.00 SaldoFactura, " _
        & " elt(tipocredito+1,'CR','CO') tipocredito, a.entrega vence " _
        & " from jsvenencped a " _
        & " left join (" & SeleccionGENClientes() & ") c on (a.codcli = c.codcli and a.id_emp = c.id_emp) " _
        & " Where " _
        & str _
        & " a.id_emp  = '" & jytsistema.WorkID & "' order by a." & OrdenadoPor

    End Function
    Function SeleccionVENListadoPrePedidos(ByVal NumeroPrePedidoDesde As String, ByVal NumeroPrePedidoHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                  ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal ClienteDesde As String, ByVal ClienteHasta As String, _
                                          Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                          Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                          Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                          Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                          Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                          Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                          Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                          Optional ByVal TipoCliente As Integer = 0, Optional ByVal EstatusCliente As Integer = 0, _
                                          Optional ByVal CondicionPago As Integer = 2, Optional ByVal EstatusFacturas As Integer = 3) As String


        Dim str As String = CadenaComplementoClientes("c", ClienteDesde, ClienteHasta, Operador, "codcli", _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, _
                                                      Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        str += " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and "
        If AsesorDesde <> "" Then str += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.codven <= '" & AsesorHasta & "' and "

        If EstatusFacturas < 3 Then str += " a.estatus = " & EstatusFacturas & " and "
        If CondicionPago < 2 Then str += " a.condpag = " & CondicionPago & " and "

        Return " select a.numped numfac, a.emision, a.codcli, c.nombre, c.rif, " _
        & " c.nombrecanal canal, c.nombretiponegocio tiponegocio, c.nomzona zona, c.nomrut ruta, " _
        & " a.codven, concat(c.codven,' ', c.nombre_vendedor) asesor, " _
        & " c.paisfiscal pais, c.estadofiscal estado, c.municipiofiscal municipio, c.parroquiafiscal parroquia, " _
        & " c.ciudadfiscal ciudad, c.barriofiscal barrio, " _
        & " a.tot_net, a.descuen, a.cargos, a.imp_iva, a.tot_ped tot_fac, 0.00 SaldoFactura, " _
        & " ELT(TIPOCREDITO+1,'CR','CO') tipoCredito, a.entrega vence  " _
        & " from jsvenencpedrgv a " _
        & " left join (" & SeleccionGENClientes() & ") c on (a.codcli = c.codcli and a.id_emp = c.id_emp) " _
        & " Where " _
        & str _
        & " a.id_emp  = '" & jytsistema.WorkID & "' order by a." & OrdenadoPor


    End Function
    Function SeleccionVENFacturas(ByVal NumeroFacturaDesde As String, ByVal NumeroFacturaHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                  ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal ClienteDesde As String, ByVal ClienteHasta As String, _
                                          Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                          Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                          Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                          Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                          Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                          Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                          Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                          Optional ByVal TipoCliente As Integer = 0, Optional ByVal EstatusCliente As Integer = 0, _
                                          Optional ByVal CondicionPago As Integer = 2, Optional ByVal EstatusFacturas As Integer = 3, _
                                          Optional FacturaPuntoDeVenta As Boolean = True) As String


        Dim str As String = CadenaComplementoClientes("c", ClienteDesde, ClienteHasta, Operador, "codcli", _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, _
                                                      Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        str += " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and "
        If AsesorDesde <> "" Then str += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.codven <= '" & AsesorHasta & "' and "

        If CondicionPago < 2 Then str += " a.condpag = " & CondicionPago & " and "
        If EstatusFacturas < 3 Then str += " a.estatus = " & EstatusFacturas & " and "

        Dim strPos As String = " UNION select a.numfac, a.emision, a.codcli, a.nomcli nombre, a.rif, " _
            & " '' canal, '' tiponegocio, '' zona, '' ruta, a.codven, concat(a.codven,' ', j.nombres, ' ', j.apellidos) asesor, " _
            & " '' pais, '' estado, '' municipio,'' parroquia, '' ciudad, '' barrio, " _
            & " a.tot_net, a.descuen, 0.00 ret_islr, a.cargos, a.imp_iva, 0.00 ret_iva, " _
            & " a.tot_fac, (a.tot_fac - a.descuen) totalACaja , if(SUM(r.importe) is null, 0.00, sum(r.importe)) SaldoFactura, ELT(a.tipocre+1, 'CO', 'CR') TipoCredito, a.vence  " _
            & " from jsvenencpos a " _
            & " left join jsvencatcli c on (a.codcli = c.codcli and a.id_emp = c.id_emp) " _
            & " left join jsvencatven j on (a.codven = j.codven and a.id_emp = j.id_emp) " _
            & " LEFT JOIN jsventracob r on (a.numfac = r.nummov and a.codcli = r.codcli and a.id_emp = r.id_emp) " _
            & " Where " _
            & " substr(a.numfac,1,2) <> 'NC' and " _
            & str _
            & " a.id_emp  = '" & jytsistema.WorkID & "' " _
            & " group by a.numfac " _
            & "  "

        SeleccionVENFacturas = " select a.numfac, a.emision, a.codcli, concat(c.nombre,a.comen) nombre, c.rif, " _
            & " f.descrip canal, g.descrip tiponegocio, h.descrip zona, i.nomrut ruta, a.codven, concat(a.codven,' ', j.nombres, ' ', j.apellidos) asesor, " _
            & " k.nombre pais, l.nombre estado, m.nombre municipio, n.nombre parroquia, o.nombre ciudad, p.nombre barrio, " _
            & " a.tot_net, a.descuen, a.descuen3 ret_islr, a.cargos, a.imp_iva, a.descuen4 ret_iva, " _
            & " a.tot_fac, (a.tot_fac - a.descuen3 - a.descuen4) totalACaja , sum(r.importe) SaldoFactura, ELT(a.tipocre+1, 'CR', 'CO') TipoCredito, a.vence  " _
            & " from jsvenencfac a " _
            & " left join jsvencatcli c on (a.codcli = c.codcli and a.id_emp = c.id_emp) " _
            & " left join jsvenliscan f on (c.categoria = f.codigo and c.id_emp = f.ID_EMP) " _
            & " left join jsvenlistip g on (c.unidad = g.codigo and c.categoria = g.antec and c.id_emp = g.id_emp) " _
            & " left join jsconctatab h on (c.zona = h.codigo and c.ID_EMP = h.ID_EMP and h.modulo = '00005') " _
            & " left join jsvenencrut i on (c.ruta_visita = i.codrut and c.ID_EMP = i.id_emp and i.tipo = '0') " _
            & " left join jsvencatven j on (a.codven = j.codven and a.id_emp = j.id_emp and j.tipo = '0' ) " _
            & " left join jsconcatter k on (c.fpais = k.codigo and c.id_emp = k.id_emp) " _
            & " left join jsconcatter l on (c.festado = l.codigo and c.id_emp = l.id_emp) " _
            & " left join jsconcatter m on (c.fmunicipio = m.codigo and c.id_emp = m.id_emp) " _
            & " left join jsconcatter n on (c.fparroquia = n.codigo and c.id_emp = n.id_emp) " _
            & " left join jsconcatter o on (c.fciudad = o.codigo and c.id_emp = o.id_emp) " _
            & " left join jsconcatter p on (c.fbarrio = p.codigo and c.id_emp = p.id_emp) " _
            & " LEFT JOIN jsventracob r on (a.numfac = r.nummov and a.codcli = r.codcli and a.id_emp = r.id_emp) " _
            & " Where " _
            & str _
            & " a.id_emp  = '" & jytsistema.WorkID & "' " _
            & " group by a.numfac " _
            & "  " _
            & IIf(FacturaPuntoDeVenta, strPos, "") _
            & " order by " & OrdenadoPor


    End Function
    Function SeleccionVENListadoNotasDeEntrega(ByVal NumeroPedidoDesde As String, ByVal NumeroPedidoHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                          ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal ClienteDesde As String, ByVal ClienteHasta As String, _
                                          Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                          Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                          Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                          Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                          Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                          Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                          Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                          Optional ByVal TipoCliente As Integer = 0, Optional ByVal EstatusCliente As Integer = 0, _
                                          Optional ByVal CondicionPago As Integer = 2, Optional ByVal EstatusFacturas As Integer = 3) As String

        Dim str As String = CadenaComplementoClientes("c", ClienteDesde, ClienteHasta, Operador, "codcli", _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, _
                                                      Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        str += " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and "
        If AsesorDesde <> "" Then str += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.codven <= '" & AsesorHasta & "' and "

        If EstatusFacturas < 3 Then str += " a.estatus = " & EstatusFacturas & " and "
        If CondicionPago < 2 Then str += " a.condpag = " & CondicionPago & " and "

        Return " select a.numfac numfac, a.emision, a.codcli, c.nombre, c.rif, " _
        & " f.descrip canal, g.descrip tiponegocio, h.descrip zona, i.nomrut ruta, a.codven, concat(a.codven,' ', j.nombres, ' ', j.apellidos) asesor, " _
        & " k.nombre pais, l.nombre estado, m.nombre municipio, n.nombre parroquia, o.nombre ciudad, p.nombre barrio, " _
        & " a.tot_net, a.descuen, a.cargos, a.imp_iva, a.tot_fac tot_fac, r.saldo SaldoFactura, ELT(a.tipocre+1, 'CR', 'CO') TipoCredito, a.vence  " _
        & " from jsvenencnot a " _
        & " left join jsvencatcli c on (a.codcli = c.codcli and a.id_emp = c.id_emp) " _
        & " left join jsvenliscan f on (c.categoria = f.codigo and c.id_emp = f.ID_EMP) " _
        & " left join jsvenlistip g on (c.unidad = g.codigo and c.categoria = g.antec and c.id_emp = g.id_emp) " _
        & " left join jsconctatab h on (c.zona = h.codigo and c.ID_EMP = h.ID_EMP and h.modulo = '00005') " _
        & " left join jsvenencrut i on (c.ruta_visita = i.codrut and c.ID_EMP = i.id_emp and i.tipo = '0') " _
        & " left join jsvencatven j on (a.codven = j.codven and a.id_emp = j.id_emp and j.tipo = '0' ) " _
        & " left join jsconcatter k on (c.fpais = k.codigo and c.id_emp = k.id_emp) " _
        & " left join jsconcatter l on (c.festado = l.codigo and c.id_emp = l.id_emp) " _
        & " left join jsconcatter m on (c.fmunicipio = m.codigo and c.id_emp = m.id_emp) " _
        & " left join jsconcatter n on (c.fparroquia = n.codigo and c.id_emp = n.id_emp) " _
        & " left join jsconcatter o on (c.fciudad = o.codigo and c.id_emp = o.id_emp) " _
        & " left join jsconcatter p on (c.fbarrio = p.codigo and c.id_emp = p.id_emp) " _
        & " LEFT JOIN ( SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp " _
        & "                 FROM jsventracob a " _
        & "                 WHERE " _
        & "                 a.id_emp = '" & jytsistema.WorkID & "' " _
        & "                 GROUP BY a.codcli, a.nummov HAVING saldo <> 0.00 ) r on (a.numfac = r.nummov and a.codcli = r.codcli and a.id_emp = r.id_emp ) " _
        & " Where " _
        & str _
        & " a.id_emp  = '" & jytsistema.WorkID & "' order by a." & OrdenadoPor

    End Function
    Function SeleccionVENListadoPresupuestos(ByVal NumeroPedidoDesde As String, ByVal NumeroPedidoHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                          ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal ClienteDesde As String, ByVal ClienteHasta As String, _
                                          Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                          Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                          Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                          Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                          Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                          Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                          Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                          Optional ByVal TipoCliente As Integer = 0, Optional ByVal EstatusCliente As Integer = 0, _
                                          Optional ByVal EstatusFacturas As Integer = 3) As String

        Dim str As String = CadenaComplementoClientes("c", ClienteDesde, ClienteHasta, Operador, "codcli", _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, _
                                                      Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        str += " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and "
        If AsesorDesde <> "" Then str += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.codven <= '" & AsesorHasta & "' and "

        If EstatusFacturas < 3 Then str += " a.estatus = " & EstatusFacturas & " and "

        Return " select a.numcot numfac, a.emision, a.codcli, c.nombre, c.rif, " _
        & " f.descrip canal, g.descrip tiponegocio, h.descrip zona, i.nomrut ruta, a.codven, concat(a.codven,' ', j.nombres, ' ', j.apellidos) asesor, " _
        & " k.nombre pais, l.nombre estado, m.nombre municipio, n.nombre parroquia, o.nombre ciudad, p.nombre barrio, " _
        & " a.tot_net, a.descuen, a.cargos, a.imp_iva, a.tot_cot tot_fac, 0.00 SaldoFactura, '' TipoCredito, a.vence " _
        & " from jsvenenccot a " _
        & " left join jsvencatcli c on (a.codcli = c.codcli and a.id_emp = c.id_emp) " _
        & " left join jsvenliscan f on (c.categoria = f.codigo and c.id_emp = f.ID_EMP) " _
        & " left join jsvenlistip g on (c.unidad = g.codigo and c.categoria = g.antec and c.id_emp = g.id_emp) " _
        & " left join jsconctatab h on (c.zona = h.codigo and c.ID_EMP = h.ID_EMP and h.modulo = '00005') " _
        & " left join jsvenencrut i on (c.ruta_visita = i.codrut and c.ID_EMP = i.id_emp and i.tipo = '0') " _
        & " left join jsvencatven j on (a.codven = j.codven and a.id_emp = j.id_emp and j.tipo = '0' ) " _
        & " left join jsconcatter k on (c.fpais = k.codigo and c.id_emp = k.id_emp) " _
        & " left join jsconcatter l on (c.festado = l.codigo and c.id_emp = l.id_emp) " _
        & " left join jsconcatter m on (c.fmunicipio = m.codigo and c.id_emp = m.id_emp) " _
        & " left join jsconcatter n on (c.fparroquia = n.codigo and c.id_emp = n.id_emp) " _
        & " left join jsconcatter o on (c.fciudad = o.codigo and c.id_emp = o.id_emp) " _
        & " left join jsconcatter p on (c.fbarrio = p.codigo and c.id_emp = p.id_emp) " _
        & " Where " _
        & str _
        & " a.id_emp  = '" & jytsistema.WorkID & "' order by a." & OrdenadoPor

    End Function
    Function SeleccionVENListadoNotasDeCredito(ByVal NumeroPedidoDesde As String, ByVal NumeroPedidoHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                          ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal ClienteDesde As String, ByVal ClienteHasta As String, _
                                          Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                          Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                          Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                          Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                          Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                          Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                          Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                          Optional ByVal TipoCliente As Integer = 0, Optional ByVal EstatusCliente As Integer = 0, _
                                          Optional ByVal EstatusFacturas As Integer = 3) As String

        Dim str As String = CadenaComplementoClientes("c", ClienteDesde, ClienteHasta, Operador, "codcli", _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, _
                                                      Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        str += " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and "
        If AsesorDesde <> "" Then str += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.codven <= '" & AsesorHasta & "' and "

        If EstatusFacturas < 3 Then str += " a.estatus = " & EstatusFacturas & " and "

        Return " select a.numncr numfac, a.emision, a.codcli, c.nombre, c.rif, " _
        & " f.descrip canal, g.descrip tiponegocio, h.descrip zona, i.nomrut ruta, a.codven, concat(a.codven,' ', j.nombres, ' ', j.apellidos) asesor, " _
        & " k.nombre pais, l.nombre estado, m.nombre municipio, n.nombre parroquia, o.nombre ciudad, p.nombre barrio, " _
        & " a.tot_net,  0.00 descuen, 0.00 cargos, a.imp_iva, a.tot_ncr tot_fac, r.saldo SaldoFactura, '' TipoCredito, a.vence,  " _
        & " q.item, q.renglon, q.descrip, q.cantidad,  q.unidad, q.totren, q.totrendes, q.causa, rr.descrip nomCausa, q.numfac documentoorigen " _
        & " from jsvenencncr a " _
        & " left join jsvencatcli c on (a.codcli = c.codcli and a.id_emp = c.id_emp) " _
        & " left join jsvenliscan f on (c.categoria = f.codigo and c.id_emp = f.ID_EMP) " _
        & " left join jsvenlistip g on (c.unidad = g.codigo and c.categoria = g.antec and c.id_emp = g.id_emp) " _
        & " left join jsconctatab h on (c.zona = h.codigo and c.ID_EMP = h.ID_EMP and h.modulo = '00005') " _
        & " left join jsvenencrut i on (c.ruta_visita = i.codrut and c.ID_EMP = i.id_emp and i.tipo = '0') " _
        & " left join jsvencatven j on (a.codven = j.codven and a.id_emp = j.id_emp and j.tipo = '0' ) " _
        & " left join jsconcatter k on (c.fpais = k.codigo and c.id_emp = k.id_emp) " _
        & " left join jsconcatter l on (c.festado = l.codigo and c.id_emp = l.id_emp) " _
        & " left join jsconcatter m on (c.fmunicipio = m.codigo and c.id_emp = m.id_emp) " _
        & " left join jsconcatter n on (c.fparroquia = n.codigo and c.id_emp = n.id_emp) " _
        & " left join jsconcatter o on (c.fciudad = o.codigo and c.id_emp = o.id_emp) " _
        & " left join jsconcatter p on (c.fbarrio = p.codigo and c.id_emp = p.id_emp) " _
        & " LEFT JOIN ( SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp " _
        & "                 FROM jsventracob a " _
        & "                 WHERE " _
        & "                 a.id_emp = '" & jytsistema.WorkID & "' " _
        & "                 GROUP BY a.codcli, a.nummov HAVING saldo <> 0.00 ) r on (a.numncr = r.nummov and a.codcli = r.codcli and a.id_emp = r.id_emp ) " _
        & " LEFT JOIN jsvenrenncr q ON (a.numncr = q.numncr AND a.id_emp = q.id_emp) " _
        & " LEFT JOIN jsvencaudcr rr on (q.causa = rr.codigo and q.id_emp = rr.id_emp and rr.credito_debito = 0 ) " _
        & " Where " _
        & str _
        & " a.id_emp  = '" & jytsistema.WorkID & "' order by a." & OrdenadoPor

    End Function
    Function SeleccionVENListadoNotasDebito(ByVal NumeroPedidoDesde As String, ByVal NumeroPedidoHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                      ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal ClienteDesde As String, ByVal ClienteHasta As String, _
                                      Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                      Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                      Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                      Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                      Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                      Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                      Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                      Optional ByVal TipoCliente As Integer = 0, Optional ByVal EstatusCliente As Integer = 0, _
                                      Optional ByVal EstatusFacturas As Integer = 3) As String

        Dim str As String = CadenaComplementoClientes("c", ClienteDesde, ClienteHasta, Operador, "codcli", _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, _
                                                      Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        str += " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and "
        If AsesorDesde <> "" Then str += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.codven <= '" & AsesorHasta & "' and "

        If EstatusFacturas < 3 Then str += " a.estatus = " & EstatusFacturas & " and "

        Return " select a.numndb numfac, a.emision, a.codcli, c.nombre, c.rif, " _
        & " f.descrip canal, g.descrip tiponegocio, h.descrip zona, i.nomrut ruta, a.codven, concat(a.codven,' ', j.nombres, ' ', j.apellidos) asesor, " _
        & " k.nombre pais, l.nombre estado, m.nombre municipio, n.nombre parroquia, o.nombre ciudad, p.nombre barrio, " _
        & " a.tot_net,  0.00 descuen, 0.00 cargos, a.imp_iva, a.tot_ndb tot_fac , r.saldo SaldoFactura, '' TipoCredito, a.vence  " _
        & " from jsvenencndb a " _
        & " left join jsvencatcli c on (a.codcli = c.codcli and a.id_emp = c.id_emp) " _
        & " left join jsvenliscan f on (c.categoria = f.codigo and c.id_emp = f.ID_EMP) " _
        & " left join jsvenlistip g on (c.unidad = g.codigo and c.categoria = g.antec and c.id_emp = g.id_emp) " _
        & " left join jsconctatab h on (c.zona = h.codigo and c.ID_EMP = h.ID_EMP and h.modulo = '00005') " _
        & " left join jsvenencrut i on (c.ruta_visita = i.codrut and c.ID_EMP = i.id_emp and i.tipo = '0') " _
        & " left join jsvencatven j on (a.codven = j.codven and a.id_emp = j.id_emp and j.tipo = '0' ) " _
        & " left join jsconcatter k on (c.fpais = k.codigo and c.id_emp = k.id_emp) " _
        & " left join jsconcatter l on (c.festado = l.codigo and c.id_emp = l.id_emp) " _
        & " left join jsconcatter m on (c.fmunicipio = m.codigo and c.id_emp = m.id_emp) " _
        & " left join jsconcatter n on (c.fparroquia = n.codigo and c.id_emp = n.id_emp) " _
        & " left join jsconcatter o on (c.fciudad = o.codigo and c.id_emp = o.id_emp) " _
        & " left join jsconcatter p on (c.fbarrio = p.codigo and c.id_emp = p.id_emp) " _
        & " LEFT JOIN ( SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp " _
        & "                 FROM jsventracob a " _
        & "                 WHERE " _
        & "                 a.id_emp = '" & jytsistema.WorkID & "' " _
        & "                 GROUP BY a.codcli, a.nummov HAVING saldo <> 0.00 ) r on (a.numndb = r.nummov and a.codcli = r.codcli and a.id_emp = r.id_emp ) " _
        & " Where " _
        & str _
        & " a.id_emp  = '" & jytsistema.WorkID & "' order by a." & OrdenadoPor

    End Function

    Public Function SeleccionVENSaldoClientes(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                              ByVal ClienteDesde As String, ByVal ClienteHasta As String, ByVal OrdenadoPor As String, ByVal operador As Integer, _
                                              ByVal FechaHasta As Date, _
                                              Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                              Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                              Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                              Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                              Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                              Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                              Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                              Optional ByVal TipoCliente As Integer = 0, Optional ByVal EstatusCliente As Integer = 0) As String


        Dim str As String = CadenaComplementoClientes("a", ClienteDesde, ClienteHasta, operador, "codcli", _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, _
                                                      Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        If AsesorDesde <> "" Then str += " a.CODVEN >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.CODVEN <= '" & AsesorHasta & "' and "

        Dim strTablaSaldos As String = " SELECT a.codcli, a.nummov, a.emision, a.vence, " _
                                        & " (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - TO_DAYS(a.vence)) dv, " _
                                        & " IFNULL(SUM(a.IMPORTE),0) saldo " _
                                        & " FROM jsventracob a  " _
                                        & " WHERE " _
                                        & " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
                                        & " a.id_emp = '" & jytsistema.WorkID & "'  " _
                                        & " GROUP BY a.codcli, a.nummov " _
                                        & " HAVING ROUND(saldo, 0) <> 0.0 "

        SeleccionVENSaldoClientes = " select a.codcli, a.nombre, a.alterno, a.ingreso, a.rif, a.nombrecanal canal, " _
            & " a.nombretiponegocio tiponegocio, a.nomzona zona, a.nomrut ruta, " _
            & " a.paisfiscal pais, a.estadofiscal estado, a.municipiofiscal municipio, a.parroquiafiscal parroquia, " _
            & " a.ciudadfiscal ciudad, a.barriofiscal barrio, " _
            & " a.nombre_vendedor asesor, IFNULL(SUM(b.saldo), 0) saldo, " _
            & " IF (  IFNULL(SUM(IF(b.dv <= 0 OR b.saldo < 0, b.saldo, 0 )), 0) < 0 , IFNULL(SUM(b.saldo), 0)  , IFNULL(SUM(IF(b.dv <= 0 OR b.saldo < 0, b.saldo, 0 )), 0)  ) saldoNoVencido  " _
            & " from (" & SeleccionGENClientes() & ") a " _
            & " left join (" & strTablaSaldos & ") b on (a.codcli = b.codcli ) " _
            & " Where " _
            & " (b.saldo <> 0.00) and " _
            & str _
            & " a.id_emp ='" & jytsistema.WorkID & "' " _
            & " group by a.codcli order by a." & OrdenadoPor


    End Function

    Function SeleccionVENPrePedidos(ByVal NumeroPrepedidoDesde As String, ByVal NumeroPrepedidoHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                    ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal ClienteDesde As String, ByVal ClienteHasta As String, _
                                      Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                      Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                      Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                      Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                      Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                      Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                      Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                      Optional ByVal TipoCliente As Integer = 0, Optional ByVal EstatusCliente As Integer = 0, _
                                      Optional ByVal CondicionPago As Integer = 2, Optional ByVal EstatusFacturas As Integer = 3, Optional ByVal Kilos As Boolean = False) As String


        Dim str As String = CadenaComplementoClientes("c", ClienteDesde, ClienteHasta, Operador, "codcli", _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, _
                                                      Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        str += " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and "
        If AsesorDesde <> "" Then str += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.codven <= '" & AsesorHasta & "' and "

        If CondicionPago < 2 Then str += " a.condpag = " & CondicionPago & " and "
        If EstatusFacturas < 3 Then str += " a.estatus = " & EstatusFacturas & " and "
        If Kilos Then str += " q.unidad = 'KGR' and "

        SeleccionVENPrePedidos = " select a.numped, a.emision, a.entrega, a.vence, a.codcli, b.item, b.item item_codart, b.renglon, b.descrip, b.cantidad, b.unidad, xx.resto_asesor, " _
            & " concat(a.codcli, ' ', c.nombre,a.comen) nombre, c.rif, " _
            & " f.descrip canal, g.descrip tiponegocio, h.descrip zona, i.nomrut ruta, a.codven, concat(a.codven,' ', j.nombres, ' ', j.apellidos) asesor, " _
            & " k.nombre pais, l.nombre estado, m.nombre municipio, n.nombre parroquia, o.nombre ciudad, p.nombre barrio, " _
            & " a.tot_net, a.descuen, a.cargos, a.imp_iva, a.tot_ped " _
            & " from jsvenencpedrgv a " _
            & " left join jsvenrenpedrgv b on (a.numped = b.numped and a.id_emp = b.id_emp) " _
            & " LEFT JOIN ( SELECT  a.codart, a.codven, a.ESMES" & Format(FechaDesde.Month, "00") & " - IF( b.venta IS NULL, 0.00, b.venta) resto_asesor, a.id_emp " _
            & "             FROM jsvencuoart a " _
            & "             LEFT JOIN (SELECT vendedor, codart , SUM( IF( origen = 'NCV', -1,1) * peso) venta, id_emp " _
            & "                        FROM jsmertramer a " _
            & "                        WHERE " _
            & "                        YEAR(fechamov) = " & FechaDesde.Year & " AND " _
            & "                        MONTH(fechamov) = " & FechaDesde.Month & " AND " _
            & "                        origen IN ('FAC', 'PVE', 'PFC', 'NDV', 'NCV') AND  " _
            & "                        id_emp = '" & jytsistema.WorkID & "' GROUP BY vendedor , codart) b ON (a.codart = b.codart AND a.codven = b.vendedor AND a.id_emp = b.id_emp) " _
            & "             WHERE " _
            & "             a.id_emp = '" & jytsistema.WorkID & "' ) xx ON (a.codven = xx.codven AND b.item = xx.codart AND a.id_emp = xx.id_emp) " _
            & " left join jsvencatcli c on (a.codcli = c.codcli and a.id_emp = c.id_emp) " _
            & " left join jsvenliscan f on (c.categoria = f.codigo and c.id_emp = f.ID_EMP) " _
            & " left join jsvenlistip g on (c.unidad = g.codigo and c.categoria = g.antec and c.id_emp = g.id_emp) " _
            & " left join jsconctatab h on (c.zona = h.codigo and c.ID_EMP = h.ID_EMP and h.modulo = '00005') " _
            & " left join jsvenencrut i on (c.ruta_visita = i.codrut and c.ID_EMP = i.id_emp and i.tipo = '0') " _
            & " left join jsvencatven j on (a.codven = j.codven and a.id_emp = j.id_emp and j.tipo = '0' ) " _
            & " left join jsconcatter k on (c.fpais = k.codigo and c.id_emp = k.id_emp) " _
            & " left join jsconcatter l on (c.festado = l.codigo and c.id_emp = l.id_emp) " _
            & " left join jsconcatter m on (c.fmunicipio = m.codigo and c.id_emp = m.id_emp) " _
            & " left join jsconcatter n on (c.fparroquia = n.codigo and c.id_emp = n.id_emp) " _
            & " left join jsconcatter o on (c.fciudad = o.codigo and c.id_emp = o.id_emp) " _
            & " left join jsconcatter p on (c.fbarrio = p.codigo and c.id_emp = p.id_emp) " _
            & " left join jsmerctainv q on (b.item = q.codart and b.id_emp = q.id_emp ) " _
            & " Where " _
            & str _
            & " a.estatus = '0' and " _
            & " a.id_emp  = '" & jytsistema.WorkID & "' order by a." & OrdenadoPor

    End Function
    Function SeleccionVENIVADocumento(ByVal NumeroDocumento As String, ByVal NombreTablaEnBD As String, ByVal NombreCampoDocumentoEnBD As String, _
                                      ByVal strAdicional As String) As String
        Return " select " & NombreCampoDocumentoEnBD & " documento, tipoiva, poriva, baseiva, impiva from " & NombreTablaEnBD & " where " _
            & " " & NombreCampoDocumentoEnBD & " = '" & NumeroDocumento & "' and " _
            & strAdicional _
            & " id_emp ='" & jytsistema.WorkID & "' order by poriva "

    End Function
    Function SeleccionVENDescuentosDocumento(ByVal NumeroDocumento As String, ByVal NombreTablaEnBD As String, ByVal NombreCampoDocumentoEnBD As String, _
                                              ByVal strAdicional As String) As String
        Return " select renglon, descrip, pordes, descuento from " & NombreTablaEnBD & " where " _
            & " " & NombreCampoDocumentoEnBD & " = '" & NumeroDocumento & "' and " _
            & strAdicional _
            & " id_emp ='" & jytsistema.WorkID & "' "

    End Function
    Function SeleccionVENPresupuesto(ByVal NumeroPresupuesto As String, ByVal FechaIVA As Date) As String

        Return " SELECT a.numcot, a.emision, a.vence, a.codcli, c.nombre, c.rif, c.nit, c.dirfiscal, a.comen, " _
            & " a.tot_net tot_net, a.imp_iva imp_iva, a.tot_cot tot_cot,  " _
            & " b.renglon, b.item, b.descrip, e.comentario, b.iva,  IF( d.monto IS NULL, 0.00, d.monto) monto,  b.unidad, b.cantidad, b.peso, b.precio precio, b.des_art, b.des_cli, b.des_ofe,  b.totren totren, ELT(b.estatus+1,'','Mercancia no sujeta a Descuentos','Bonificaciones') estatus  " _
            & " FROM jsvenenccot a  " _
            & " LEFT JOIN jsvenrencot b ON (a.numcot = b.numcot AND a.id_emp = b.id_emp)  " _
            & " LEFT JOIN jsvenrencom e ON (b.numcot = e.numdoc AND b.id_emp = e.id_emp AND b.renglon = e.renglon AND e.origen = 'COT')  " _
            & " LEFT JOIN jsvencatcli c ON (a.codcli = c.codcli AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (" & SeleccionGENTablaIVA(FechaIVA) & ")  d ON (b.iva = d.tipo) " _
            & " WHERE " _
            & " a.numcot = '" & NumeroPresupuesto & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by b.estatus, b.renglon "

    End Function
    Function SeleccionVENPrePedido(ByVal numPrePedido As String, ByVal FechaIVA As Date) As String

        Return " SELECT a.numped numcot, a.emision, a.entrega vence, a.codcli, c.nombre, c.rif, c.nit, c.dirfiscal, a.comen, a.items, a.kilos, " _
            & " a.tot_net tot_net, a.descuen descuen, a.imp_iva imp_iva, a.tot_ped tot_cot,  " _
            & " b.renglon, b.item, b.descrip, e.comentario, b.iva,  IF( d.monto IS NULL, 0.00, d.monto) monto,  b.unidad, b.cantidad, b.peso, b.precio precio, b.des_art, b.des_cli, b.des_ofe,  b.totren totren, ELT(b.estatus+1,'','Mercancia no sujeta a Descuentos','Bonificaciones') estatus  " _
            & " FROM jsvenencpedrgv a  " _
            & " LEFT JOIN jsvenrenpedrgv b ON (a.numped = b.numped AND a.id_emp = b.id_emp)  " _
            & " LEFT JOIN jsvenrencom e ON (b.numped = e.numdoc AND b.id_emp = e.id_emp AND b.renglon = e.renglon AND e.origen = 'PPE')  " _
            & " LEFT JOIN jsvencatcli c ON (a.codcli = c.codcli AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (" & SeleccionGENTablaIVA(FechaIVA) & ")  d ON (b.iva = d.tipo) " _
            & " WHERE " _
            & " a.numped = '" & numPrePedido & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by b.estatus, b.renglon "

    End Function
    Function SeleccionVENPedido(ByVal numPedido As String, ByVal FechaIVA As Date) As String

        Return " SELECT a.numped numcot, a.emision, a.entrega vence, a.codcli, c.nombre, c.rif, c.nit, c.dirfiscal, a.comen, a.items, a.kilos, " _
            & " a.tot_net tot_net, a.descuen descuen, a.imp_iva imp_iva, a.tot_ped tot_cot,  " _
            & " b.renglon, b.item, b.descrip, e.comentario, b.iva,  IF( d.monto IS NULL, 0.00, d.monto) monto,  b.unidad, b.cantidad, b.peso, b.precio precio, b.des_art, b.des_cli, b.des_ofe,  b.totren totren, ELT(b.estatus+1,'','Mercancia no sujeta a Descuentos','Bonificaciones') estatus  " _
            & " FROM jsvenencped a  " _
            & " LEFT JOIN jsvenrenped b ON (a.numped = b.numped AND a.id_emp = b.id_emp)  " _
            & " LEFT JOIN jsvenrencom e ON (b.numped = e.numdoc AND b.id_emp = e.id_emp AND b.renglon = e.renglon AND e.origen = 'PPE')  " _
            & " LEFT JOIN jsvencatcli c ON (a.codcli = c.codcli AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (" & SeleccionGENTablaIVA(FechaIVA) & ")  d ON (b.iva = d.tipo) " _
            & " WHERE " _
            & " a.numped = '" & numPedido & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by b.estatus, b.renglon "

    End Function

    Function SeleccionVENNotaDeEntrega(ByVal numNota As String, ByVal FechaIVA As Date) As String

        Return " SELECT a.numfac numcot, a.emision, a.vence vence, a.codcli, c.nombre, c.rif, c.nit, c.dirfiscal, a.comen, a.items, a.kilos, " _
            & " ELT( a.condpag + 1 , concat('CREDITO CON VENCIMIENTO AL ', cast( DATE_FORMAT(a.vence, '%d-%m-%Y') as char ) )  , 'CONTADO', '') condicionpago, " _
            & " CONCAT(a.codven, ' ', f.apellidos,', ', f.nombres) asesor,  " _
            & " a.tot_net tot_net, a.descuen descuen, a.imp_iva imp_iva, a.tot_fac tot_cot,  " _
            & " b.renglon, b.item, b.descrip, e.comentario, b.iva,  IF( d.monto IS NULL, 0.00, d.monto) monto,  b.unidad, b.cantidad, b.peso, b.precio precio, b.des_art, b.des_cli, b.des_ofe,  b.totren totren, ELT(b.estatus+1,'','Mercancia no sujeta a Descuentos','Bonificaciones') estatus  " _
            & " FROM jsvenencnot a  " _
            & " LEFT JOIN jsvenrennot b ON (a.numfac = b.numfac AND a.id_emp = b.id_emp)  " _
            & " LEFT JOIN jsvenrencom e ON (b.numfac = e.numdoc AND b.id_emp = e.id_emp AND b.renglon = e.renglon AND e.origen = 'PFC')  " _
            & " LEFT JOIN jsvencatcli c ON (a.codcli = c.codcli AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsvencatven f ON (a.codven = f.codven AND a.id_emp = f.id_emp and f.tipo = '0' ) " _
            & " LEFT JOIN (" & SeleccionGENTablaIVA(FechaIVA) & ")  d ON (b.iva = d.tipo) " _
            & " WHERE " _
            & " a.numfac = '" & numNota & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by b.estatus, b.renglon "

    End Function
    Function SeleccionVENFactura(ByVal numFactura As String, ByVal FechaIVA As Date) As String

        Return " SELECT a.numfac numcot, a.emision, a.vence vence, a.codcli, c.nombre, c.rif, c.nit, c.dirfiscal, a.comen, a.items, a.kilos, " _
            & " ELT( a.condpag + 1 , concat('CREDITO CON VENCIMIENTO AL ',cast( DATE_FORMAT(a.vence, '%d-%m-%Y') as char ) )  , 'CONTADO', '') condicionpago, " _
            & " CONCAT(a.codven, ' ', f.apellidos,', ', f.nombres) asesor,  " _
            & " a.tot_net tot_net, a.descuen descuen, a.imp_iva imp_iva, a.tot_fac tot_cot,  " _
            & " b.renglon, b.item, b.descrip, e.comentario, b.iva,  IF( d.monto IS NULL, 0.00, d.monto) monto,  b.unidad, b.cantidad, b.peso, b.precio precio, b.des_art, b.des_cli, b.des_ofe,  b.totren totren, ELT(b.estatus+1,'','Mercancia no sujeta a Descuentos','Bonificaciones') estatus  " _
            & " FROM jsvenencfac a  " _
            & " LEFT JOIN jsvenrenfac b ON (a.numfac = b.numfac AND a.id_emp = b.id_emp)  " _
            & " LEFT JOIN jsvenrencom e ON (b.numfac = e.numdoc AND b.id_emp = e.id_emp AND b.renglon = e.renglon AND e.origen = 'FAC')  " _
            & " LEFT JOIN jsvencatcli c ON (a.codcli = c.codcli AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsvencatven f ON (a.codven = f.codven AND a.id_emp = f.id_emp and f.tipo = '0' ) " _
            & " LEFT JOIN (" & SeleccionGENTablaIVA(FechaIVA) & ")  d ON (b.iva = d.tipo) " _
            & " WHERE " _
            & " a.numfac = '" & numFactura & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by b.estatus, b.renglon "

    End Function
    Function SeleccionVENNotaDebito(ByVal numFactura As String, ByVal FechaIVA As Date) As String

        Return " SELECT a.numndb numcot, a.emision, a.vence vence, a.codcli, c.nombre, c.rif, c.nit, c.dirfiscal, a.comen, a.items, a.kilos, " _
            & " CONCAT(a.codven, ' ', f.apellidos,', ', f.nombres) asesor,  " _
            & " a.tot_net tot_net, 0.00 descuen, a.imp_iva imp_iva, a.tot_ndb tot_cot,  " _
            & " b.renglon, b.item, b.descrip, e.comentario, b.iva,  " _
            & " g.codigo causa, g.descrip descripcausa, " _
            & " IF( d.monto IS NULL, 0.00, d.monto) monto,  b.unidad, b.cantidad, b.peso, b.precio precio, b.des_art, b.des_cli, b.des_ofe,  b.totren totren, ELT(b.estatus+1,'','Mercancia no sujeta a Descuentos','Bonificaciones') estatus  " _
            & " FROM jsvenencndb a  " _
            & " LEFT JOIN jsvenrenndb b ON (a.numndb = b.numndb AND a.id_emp = b.id_emp)  " _
            & " LEFT JOIN jsvenrencom e ON (b.numndb = e.numdoc AND b.id_emp = e.id_emp AND b.renglon = e.renglon AND e.origen = 'NDV')  " _
            & " LEFT JOIN jsvencatcli c ON (a.codcli = c.codcli AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsvencatven f ON (a.codven = f.codven AND a.id_emp = f.id_emp and f.tipo = '0' ) " _
            & " LEFT JOIN jsvencaudcr g on (b.causa = g.codigo AND b.id_emp = g.id_emp AND g.credito_debito = 1 ) " _
            & " LEFT JOIN (" & SeleccionGENTablaIVA(FechaIVA) & ")  d ON (b.iva = d.tipo) " _
            & " WHERE " _
            & " a.numndb = '" & numFactura & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by b.estatus, b.renglon "

    End Function

    Function SeleccionVENNotaCredito(ByVal numFactura As String, ByVal FechaIVA As Date) As String

        Return " SELECT a.numncr numcot, a.emision, a.vence vence, a.codcli, c.nombre, c.rif, c.nit, c.dirfiscal, a.comen, a.items, a.kilos, " _
            & " CONCAT(a.codven, ' ', f.apellidos,', ', f.nombres) asesor,  " _
            & " a.tot_net tot_net, 0.00 descuen, a.imp_iva imp_iva, a.tot_ncr tot_cot,  " _
            & " b.renglon, b.item, b.descrip, e.comentario, b.iva,  " _
            & " g.codigo causa, g.descrip descripcausa, " _
            & " IF( d.monto IS NULL, 0.00, d.monto) monto,  b.unidad, b.cantidad, b.peso, b.precio precio, b.por_acepta_dev des_art, b.des_cli, b.des_ofe,  b.totren totren, ELT(b.estatus+1,'','Mercancia no sujeta a Descuentos','Bonificaciones') estatus  " _
            & " FROM jsvenencncr a  " _
            & " LEFT JOIN jsvenrenncr b ON (a.numncr = b.numncr AND a.id_emp = b.id_emp)  " _
            & " LEFT JOIN jsvenrencom e ON (b.numncr = e.numdoc AND b.id_emp = e.id_emp AND b.renglon = e.renglon AND e.origen = 'NDV')  " _
            & " LEFT JOIN jsvencatcli c ON (a.codcli = c.codcli AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsvencatven f ON (a.codven = f.codven AND a.id_emp = f.id_emp and f.tipo = '0' ) " _
            & " LEFT JOIN jsvencaudcr g on (b.causa = g.codigo AND b.id_emp = g.id_emp AND g.credito_debito = 0 ) " _
            & " LEFT JOIN (" & SeleccionGENTablaIVA(FechaIVA) & ")  d ON (b.iva = d.tipo) " _
            & " WHERE " _
            & " a.numncr = '" & numFactura & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by b.estatus, b.renglon "

    End Function
    Public Function SeleccionVENEstadoDeCuentaClientesPlus(ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, ByVal OrdenadoPor As String, ByVal operador As Integer, _
                                              ByVal FEchaDesde As Date, ByVal FechaHasta As Date, _
                                              Optional ByVal TipoCliente As Integer = 0, Optional ByVal EstatusCliente As Integer = 0)

        Dim str As String = ""

        If ProveedorDesde <> "" Then str += " a.codcli >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codcli <= '" & ProveedorHasta & "' and "
        If TipoCliente < 4 Then str += " a.especial = " & TipoCliente & " and "
        If EstatusCliente < 4 Then str += " a.estatus = " & EstatusCliente & " and "

        Return " SELECT a.codcli, a.nombre, a.alterno, a.ingreso, a.rif, e.nummov, e.tipomov, e.emision, e.hora, e.vence, e.refer, e.concepto, " _
            & " IF (e.importe < 0 , ABS(e.importe), 0.00) creditos,  IF (e.importe > 0 , ABS(e.importe), 0.00) debitos,  " _
            & " IF( b.saldo IS NULL, 0.00, b.saldo) saldo " _
            & " FROM jsvencatcli a " _
            & " LEFT JOIN (SELECT a.codcli, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsventracob a WHERE a.emision < '" & ft.FormatoFechaMySQL(FEchaDesde) & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.codcli ) b ON ( a.codcli = b.codcli ) " _
            & " LEFT JOIN jsventracob e ON (a.codcli = e.codcli AND a.id_emp = e.id_emp AND e.origen NOT IN ('CXC') ) " _
            & " WHERE " _
            & " e.emision >= '" & ft.FormatoFechaMySQL(FEchaDesde) & "' AND " _
            & " e.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & str _
            & " a.id_emp ='" & jytsistema.WorkID & "' " _
            & " UNION " _
            & " SELECT a.codcli, a.nombre, a.alterno, a.ingreso, a.rif, e.nummov, e.tipomov, e.emision, e.hora, e.vence, e.refer, e.concepto, " _
            & " IF (e.importe < 0 , ABS(e.importe), 0.00) creditos,  IF (e.importe > 0 , ABS(e.importe), 0.00) debitos,  " _
            & " IF( b.saldo IS NULL, 0.00, b.saldo) saldo " _
            & " FROM jsvencatcli a " _
            & " LEFT JOIN (SELECT a.codcli, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsventracob a WHERE a.emision < '" & ft.FormatoFechaMySQL(FEchaDesde) & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.codcli ) b ON ( a.codcli = b.codcli ) " _
            & " LEFT JOIN jsventracob e ON (a.codcli = e.codcli AND a.id_emp = e.id_emp AND e.origen IN ('CXC') AND SUBSTRING(e.concepto,1,9) = 'RETENCION' ) " _
            & " WHERE " _
            & " e.emision >= '" & ft.FormatoFechaMySQL(FEchaDesde) & "' AND " _
            & " e.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & str _
            & " a.id_emp ='" & jytsistema.WorkID & "' " _
            & " UNION " _
            & " SELECT a.codcli, a.nombre, a.alterno, a.ingreso, a.rif, e.nummov, e.tipomov, e.emision, e.hora, e.vence, e.refer, e.concepto, " _
            & " IF (e.importe < 0 , ABS(e.importe), 0.00) creditos, IF (e.importe > 0 , ABS(e.importe), 0.00) debitos, " _
            & " IF( b.saldo IS NULL, 0.00, b.saldo) saldo " _
            & " FROM jsvencatcli a " _
            & " LEFT JOIN (SELECT a.codcli, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsventracob a WHERE a.emision < '" & ft.FormatoFechaMySQL(FEchaDesde) & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.codcli ) b ON ( a.codcli = b.codcli ) " _
            & " LEFT JOIN jsventracob e ON (a.codcli = e.codcli AND a.id_emp = e.id_emp AND e.origen IN ('CXC') AND SUBSTRING(e.concepto,1,9) <> 'RETENCION' AND e.tipomov IN ('FC','GR','NC') AND e.comproba = '' )  " _
            & " WHERE " _
            & " e.emision >= '" & ft.FormatoFechaMySQL(FEchaDesde) & "' AND " _
            & " e.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & str _
            & " a.id_emp ='" & jytsistema.WorkID & "' " _
            & " UNION " _
            & " SELECT a.codcli, a.nombre, a.alterno, a.ingreso, a.rif, e.comproba nummov, e.tipomov, e.emision, e.hora, e.vence, e.refer, e.concepto, " _
            & " IF (SUM(e.importe) < 0 , ABS(SUM(e.importe)), 0.00) creditos, IF (SUM(e.importe) > 0 , ABS(SUM(e.importe)), 0.00) debitos,  " _
            & " IF( b.saldo IS NULL, 0.00, b.saldo) saldo " _
            & " FROM jsvencatcli a " _
            & " LEFT JOIN (SELECT a.codcli, IFNULL(SUM(a.IMPORTE),0) saldo FROM jsventracob a WHERE a.emision < '" & ft.FormatoFechaMySQL(FEchaDesde) & "' AND a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.codcli ) b ON ( a.codcli = b.codcli ) " _
            & " LEFT JOIN jsventracob e ON (a.codcli = e.codcli AND a.id_emp = e.id_emp AND e.origen IN ('CXC') AND e.tipomov IN ('AB', 'CA', 'ND') AND e.comproba <> ''   ) " _
            & " WHERE " _
            & " e.emision >= '" & ft.FormatoFechaMySQL(FEchaDesde) & "' AND " _
            & " e.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & str _
            & " a.id_emp ='" & jytsistema.WorkID & "' " _
            & " GROUP BY e.comproba " _
            & " ORDER BY 8 "


    End Function
    Public Function SeleccionVENEstadoDeCuentaClientes(ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, ByVal OrdenadoPor As String, ByVal operador As Integer, _
                                              FEchaDesde As Date, ByVal FechaHasta As Date, _
                                              Optional ByVal TipoCliente As Integer = 0, Optional ByVal EstatusCliente As Integer = 0) As String


        Dim str As String = ""

        If ProveedorDesde <> "" Then str += " a.codcli >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codcli <= '" & ProveedorHasta & "' and "
        If TipoCliente < 4 Then str += " a.especial = " & TipoCliente & " and "
        If EstatusCliente < 4 Then str += " a.estatus = " & EstatusCliente & " and "

        SeleccionVENEstadoDeCuentaClientes = " select a.codcli, a.nombre, a.alterno, a.ingreso, a.rif, " _
            & " e.nummov, e.tipomov, e.emision, e.hora, e.vence, e.refer, e.concepto, " _
            & " if (e.importe < 0 , abs(e.importe), 0.00) creditos,  " _
            & " if (e.importe > 0 , abs(e.importe), 0.00) debitos,  " _
            & " if( b.saldo is null, 0.00, b.saldo) saldo " _
            & " from jsvencatcli a " _
            & " left join (select a.codcli, IFNULL(SUM(a.IMPORTE),0) saldo " _
            & "             from jsventracob a " _
            & "             where " _
            & "             a.emision < '" & ft.FormatoFechaMySQL(FEchaDesde) & "' and " _
            & "             a.id_emp = '" & jytsistema.WorkID & "' " _
            & "             group by a.codcli ) b on ( a.codcli = b.codcli ) " _
            & " left join jsventracob e on (a.codcli = e.codcli and a.id_emp = e.id_emp ) " _
            & " Where " _
            & " e.emision >= '" & ft.FormatoFechaMySQL(FEchaDesde) & "' and " _
            & " e.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
            & str _
            & " a.id_emp ='" & jytsistema.WorkID & "' " _
            & " order by a." & OrdenadoPor & ", e.nummov, e.emision "

    End Function

    Public Function SeleccionVENMovimientosClientes(ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, ByVal OrdenadoPor As String, ByVal operador As Integer, _
                                          FEchaDesde As Date, ByVal FechaHasta As Date, TipoDocumentos As String, _
                                          Optional ByVal TipoCliente As Integer = 0, Optional ByVal EstatusCliente As Integer = 0, _
                                          Optional CausaDesde As String = "", Optional CausaHasta As String = "") As String


        Dim str As String = ""

        If ProveedorDesde <> "" Then str += " a.codcli >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codcli <= '" & ProveedorHasta & "' and "
        If CausaDesde <> "" Then str += " e.asiento >= '" & CausaDesde & "' AND "
        If CausaHasta <> "" Then str += " e.ASIENTO <= '" & CausaHasta & "' AND "
        If TipoCliente < 4 Then str += " a.especial = " & TipoCliente & " and "
        If EstatusCliente < 4 Then str += " a.estatus = " & EstatusCliente & " and "
        str += " locate(e.tipomov, '" & TipoDocumentos & "') > 0 and "

        SeleccionVENMovimientosClientes = " select a.codcli, a.nombre, a.alterno, a.ingreso, a.rif, " _
            & " e.nummov, e.tipomov, e.emision, e.hora, e.vence, e.refer, e.concepto, " _
            & " e.importe , e.origen, e.numorg  " _
            & " from jsvencatcli a " _
            & " left join jsventracob e on (a.codcli = e.codcli and a.id_emp = e.id_emp ) " _
            & " Where " _
            & " e.emision >= '" & ft.FormatoFechaMySQL(FEchaDesde) & "' and " _
            & " e.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
            & str _
            & " a.id_emp ='" & jytsistema.WorkID & "' " _
            & " order by a." & OrdenadoPor & ", e.emision "

    End Function

    Public Function SeleccionVENMovimientosDocumentos(ByVal DocumentoDesde As String, ByVal DocumentoHasta As String, ByVal OrdenadoPor As String, ByVal operador As Integer, _
                                          FechaDesde As Date, ByVal FechaHasta As Date, Tipo As Integer) As String

        Dim str As String = ""

        If DocumentoDesde <> "" Then str += " a." & IIf(Tipo = 0, "nummov", "comproba") & " >= '" & DocumentoDesde & "' and "
        If DocumentoHasta <> "" Then str += " a." & IIf(Tipo = 0, "nummov", "comproba") & " <= '" & DocumentoHasta & "' and "

        SeleccionVENMovimientosDocumentos = " select e.codcli, e.nombre, e.alterno, e.ingreso, e.rif, " _
            & " a.nummov, a.tipomov, a.emision, a.hora, a.vence, a.refer, a.concepto, " _
            & " a.importe , a.origen, a.numorg, a.comproba,  " _
            & " if (a.importe < 0 , abs(a.importe), 0.00) creditos,  " _
            & " if (a.importe > 0 , abs(a.importe), 0.00) debitos,  " _
            & " 0.00 saldo " _
            & " from jsventracob a " _
            & " left join jsvencatcli e on (a.codcli = e.codcli and a.id_emp = e.id_emp ) " _
            & " Where " _
            & " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and " _
            & " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
            & str _
            & " a.id_emp ='" & jytsistema.WorkID & "' " _
            & " order by a." & OrdenadoPor & ", a.emision "

    End Function
    Function SeleccionVENAuditoriaClientes(ByVal ClienteDesde As String, ByVal ClienteHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                      FechaHasta As Date, Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                      Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                      Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                      Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                      Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                      Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                      Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                      Optional ByVal TipoCliente As Integer = 4, Optional ByVal EstatusCliente As Integer = 4) As String


        Dim str As String = CadenaComplementoClientes("c", ClienteDesde, ClienteHasta, Operador, OrdenadoPor, _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, , _
                                                      , , , Pais, Estado, Municipio, Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        If AsesorDesde <> "" Then str += IIf(RutaDesde <> "" And RutaHasta <> "", " e.codven >= '", " b.codven >= '") & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += IIf(RutaDesde <> "" And RutaHasta <> "", " e.codven <= '", " b.codven <= '") & AsesorHasta & "' and "
        If ZonaDesde <> "" Then str += " e.codzon >= '" & ZonaDesde & "' and "
        If ZonaHasta <> "" Then str += " e.codzon <= '" & ZonaHasta & "' and "
        If RutaDesde <> "" Then str += " e.codrut >= '" & RutaDesde & "' and "
        If RutaHasta <> "" Then str += " e.codrut <= '" & RutaHasta & "' and "

        SeleccionVENAuditoriaClientes = " select a.codcli, c.alterno, c.nombre,  c.rif, a.nummov, a.importe, a.saldo, " _
                & " b.tipomov, b.emision, b.vence, b.refer, b.concepto, if( b.importe > 0, abs(b.importe), 0.00) debitos, " _
                & " if( b.importe <= 0, abs(b.importe), 0.00) creditos, b.formapag " _
                & " from ( select a.codcli,  a.nummov, a.tipomov, min(concat(a.emision,a.hora)) grupo, a.importe, b.saldo " _
                & "         from jsventracob a " _
                & "         left join ( select a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo " _
                & "                     from jsventracob a " _
                & "                     Where " _
                & "                     a.fechasi <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
                & "                     a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & "                     a.ID_EMP = '" & jytsistema.WorkID & "' " _
                & "                     group by a.codcli, a.nummov " _
                & "                     having round(saldo,0) <> 0 ) b on (a.codcli = b.codcli and a.nummov = b.nummov) " _
                & "         where " _
                & "         a.fechasi <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
                & "         a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                & "         a.id_emp = '" & jytsistema.WorkID & "' and " _
                & "         b.saldo is not null " _
                & "         group by a.codcli, a.nummov ) a " _
                & " left join jsventracob b on (a.codcli = b.codcli and a.nummov = b.nummov) " _
                & " left join jsvencatcli c on (a.codcli = c.codcli and b.id_emp = c.id_emp) " _
                & IIf((RutaDesde <> "" And RutaHasta <> "") Or (ZonaDesde <> "" And ZonaHasta <> ""), " left join jsvenrenrut d on (a.codcli = d.cliente and b.id_emp = d.id_emp and d.tipo = '0') ", "") _
                & IIf((RutaDesde <> "" And RutaHasta <> "") Or (ZonaDesde <> "" And ZonaHasta <> ""), " left join jsvenencrut e on (d.codrut = e.codrut and d.id_emp = e.id_emp and d.tipo = e.tipo  ) ", "") _
                & " where " _
                & str _
                & " b.fechasi <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
                & " b.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " b.id_emp = '" & jytsistema.WorkID & "' " _
                & " order by c." & OrdenadoPor _
                & " "


    End Function
    Function SeleccionVENVencimientos(ByVal ClienteDesde As String, ByVal ClienteHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                      ByVal FechaHasta As Date, Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                      Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                      Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                      Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                      Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                      Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                      Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                      Optional ByVal TipoCliente As Integer = 4, Optional ByVal EstatusCliente As Integer = 4, _
                                      Optional ByVal Lapso1Desde As Integer = 1, Optional ByVal lapso1Hasta As Integer = 8, _
                                      Optional ByVal Lapso2Desde As Integer = 9, Optional ByVal lapso2Hasta As Integer = 15, _
                                      Optional ByVal Lapso3Desde As Integer = 16, Optional ByVal lapso3Hasta As Integer = 30, _
                                      Optional ByVal Lapso4Desde As Integer = 31, Optional ByVal DesdeEmision As Boolean = True) As String


        Dim str As String = CadenaComplementoClientes("b", ClienteDesde, ClienteHasta, Operador, OrdenadoPor, _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        Dim strBoolean As String = "" '" and de >= 0 "
        'If Not DesdeEmision Then strBoolean = " and dv >= 0  "

        If AsesorDesde <> "" Then str += " e.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " e.codven <= '" & AsesorHasta & "' and "
        If ZonaDesde <> "" Then str += " e.codzon >= '" & ZonaDesde & "' and "
        If ZonaHasta <> "" Then str += " e.codzon <= '" & ZonaHasta & "' and "
        If RutaDesde <> "" Then str += " e.codrut >= '" & RutaDesde & "' and "
        If RutaHasta <> "" Then str += " e.codrut <= '" & RutaHasta & "' and "

        SeleccionVENVencimientos = "SELECT a.codcli, b.nombre, a.nummov, a.tipomov, a.refer, a.EMISION, a.VENCE, a.importe, " _
            & " min(concat(a.EMISION,a.HORA)) GRUPO, " _
            & " IFNULL(SUM(a.IMPORTE),0) saldo, (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.vence)) as DV, (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision)) as DE, " _
            & " if( (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision))>= " & Lapso1Desde & " and (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision))<= " & lapso1Hasta & ", '1. Vencimientos de " & Lapso1Desde & " a " & lapso1Hasta & " días'  , " _
            & " if( (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision))>= " & Lapso2Desde & " and (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision))<= " & lapso2Hasta & ", '2. Vencimientos de " & Lapso2Desde & " a " & lapso2Hasta & " días'  , " _
            & " if( (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision))>= " & Lapso3Desde & " and (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision))<= " & lapso3Hasta & ", '3. Vencimientos de " & Lapso3Desde & " a " & lapso3Hasta & " días'  , " _
            & " if( (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision))>= " & Lapso4Desde & ", '4. Vencimientos de " & Lapso4Desde & " días o más' , '0' " _
            & " )))) as lapso " _
            & " from jsventracob a " _
            & " LEFT JOIN jsvencatcli b ON ( b.CODCLI = a.CODCLI AND b.ID_EMP = a.ID_EMP) " _
            & " left join jsvenrenrut d on (a.codcli = d.cliente and b.id_emp = d.id_emp and d.tipo = '0') " _
            & " left join jsvenencrut e on (d.codrut = e.codrut and d.id_emp = e.id_emp and d.tipo = e.tipo  ) " _
            & " Where " _
            & str _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
            & " GROUP BY a.codcli, a.nummov Having (Saldo <> 0 ) " & strBoolean _
            & " ORDER BY LAPSO, a.CODCLI, NUMMOV, EMISION ASC "


    End Function
    Function SeleccionVENVencimientosPlus(ByVal ClienteDesde As String, ByVal ClienteHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                      ByVal FechaHasta As Date, Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                      Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                      Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                      Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                      Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                      Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                      Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                      Optional ByVal TipoCliente As Integer = 4, Optional ByVal EstatusCliente As Integer = 4, _
                                      Optional ByVal Lapso1Desde As Integer = 1, Optional ByVal lapso1Hasta As Integer = 8, _
                                      Optional ByVal Lapso2Desde As Integer = 9, Optional ByVal lapso2Hasta As Integer = 15, _
                                      Optional ByVal Lapso3Desde As Integer = 16, Optional ByVal lapso3Hasta As Integer = 30, _
                                      Optional ByVal Lapso4Desde As Integer = 31, Optional ByVal DesdeEmision As Boolean = True) As String


        Dim str As String = CadenaComplementoClientes("b", ClienteDesde, ClienteHasta, Operador, OrdenadoPor, _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        If AsesorDesde <> "" Then str += " e.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " e.codven <= '" & AsesorHasta & "' and "
        If ZonaDesde <> "" Then str += " e.codzon >= '" & ZonaDesde & "' and "
        If ZonaHasta <> "" Then str += " e.codzon <= '" & ZonaHasta & "' and "
        If RutaDesde <> "" Then str += " e.codrut >= '" & RutaDesde & "' and "
        If RutaHasta <> "" Then str += " e.codrut <= '" & RutaHasta & "' and "

        SeleccionVENVencimientosPlus = "SELECT a.codcli, b.nombre, a.nummov, a.tipomov, a.refer, a.EMISION, a.VENCE, a.importe, a.saldo, a.lapso, a.de, a.dv, a.id_emp " _
            & " from (SELECT a.codcli, a.nummov, a.tipomov, a.emision, a.vence, a.refer, a.importe, a.saldo, a.dv, a.de, " _
            & "       IF(a.DE >= " & Lapso1Desde & " AND a.DE <=" & lapso2Hasta & ", 'Vencimientos de " & Lapso1Desde & " a " & lapso2Hasta & " días ', " _
            & "         IF(a.DE >= " & Lapso2Desde & " AND a.DE <=" & lapso2Hasta & ", 'Vencimientos de " & Lapso2Desde & " a " & lapso2Hasta & " días', " _
            & " 		    IF(a.DE >= " & Lapso3Desde & " AND a.DE <=" & lapso3Hasta & ", 'Vencimientos de " & Lapso3Desde & " a " & Lapso3Desde & " días', " _
            & "  		        IF(a.DE >= " & Lapso4Desde & ", 'Vencimientos de " & Lapso4Desde & " días o mas', 'Vencimientos hasta " & Lapso1Desde & " días o menos')))) lapso, a.id_emp " _
            & "          FROM (SELECT a.codcli, a.nummov, a.tipomov, a.refer, MIN(a.emision) emision, MIN(a.vence) vence, a.importe,  IFNULL(SUM(a.IMPORTE),0) saldo,  " _
            & " (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - TO_DAYS(MIN(a.vence))) AS DV, " _
            & " (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - TO_DAYS(MIN(a.emision))) AS DE, a.id_emp " _
            & " FROM jsventracob a " _
            & " WHERE " _
            & " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " GROUP BY a.codcli, a.nummov " _
            & " HAVING ABS(ROUND(saldo,2)) > 0 ) a ) a " _
            & " LEFT JOIN jsvencatcli b ON ( b.CODCLI = a.CODCLI AND b.ID_EMP = a.ID_EMP) " _
            & " left join jsvenrenrut d on (a.codcli = d.cliente and b.id_emp = d.id_emp and d.tipo = '0') " _
            & " left join jsvenencrut e on (d.codrut = e.codrut and d.id_emp = e.id_emp and d.tipo = e.tipo  ) " _
            & " Where " _
            & str _
            & " not b.codcli is null and " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
            & " GROUP BY a.codcli, a.nummov Having (Saldo <> 0 ) " _
            & " ORDER BY LAPSO, a.CODCLI, NUMMOV, EMISION ASC "


    End Function
    Function SeleccionVENVencimientosResumen(ByVal ClienteDesde As String, ByVal ClienteHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                      ByVal FechaHasta As Date, Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                      Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                      Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                      Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                      Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                      Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                      Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                      Optional ByVal TipoCliente As Integer = 4, Optional ByVal EstatusCliente As Integer = 4, _
                                      Optional ByVal Lapso1Desde As Integer = 1, Optional ByVal lapso1Hasta As Integer = 8, _
                                      Optional ByVal Lapso2Desde As Integer = 9, Optional ByVal lapso2Hasta As Integer = 15, _
                                      Optional ByVal Lapso3Desde As Integer = 16, Optional ByVal lapso3Hasta As Integer = 30, _
                                      Optional ByVal Lapso4Desde As Integer = 31, Optional ByVal DesdeEmision As Boolean = True) As String


        Dim str As String = CadenaComplementoClientes("a", ClienteDesde, ClienteHasta, Operador, OrdenadoPor, _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        Dim strBoolean As String = "" ' " and de >= 0 "
        'If Not DesdeEmision Then strBoolean = " and dv >= 0  "
        Dim CampoFecha As String = "emision"
        If Not DesdeEmision Then CampoFecha = "vence"

        If AsesorDesde <> "" Then str += " e.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " e.codven <= '" & AsesorHasta & "' and "
        If ZonaDesde <> "" Then str += " e.codzon >= '" & ZonaDesde & "' and "
        If ZonaHasta <> "" Then str += " e.codzon <= '" & ZonaHasta & "' and "
        If RutaDesde <> "" Then str += " e.codrut >= '" & RutaDesde & "' and "
        If RutaHasta <> "" Then str += " e.codrut <= '" & RutaHasta & "' and "

        SeleccionVENVencimientosResumen = "select a.codcli, a.nombre, " _
        & " Sum(if(b.lapso = '0',b.saldo,0)) saldo0, " _
        & " Sum(if(b.lapso = '1',b.saldo,0)) saldo1, " _
        & " Sum(if(b.lapso = '2',b.saldo,0)) saldo2, " _
        & " Sum(if(b.lapso = '3',b.saldo,0)) saldo3, " _
        & " Sum(if(b.lapso = '4',b.saldo,0)) saldo4 " _
        & " from jsvencatcli a " _
        & " left join ( SELECT a.CODCLI, a.NUMMOV, IFNULL(SUM(a.IMPORTE),0) AS SALDO, " _
        & "             (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.vence)) as DV, " _
        & "             (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision)) as DE, " _
        & "             if( (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a." & CampoFecha & ")) >= " & Lapso1Desde & " and (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a." & CampoFecha & ")) <= " & lapso1Hasta & ", '1' , " _
        & "             if( (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a." & CampoFecha & ")) >= " & Lapso2Desde & " and (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a." & CampoFecha & ")) <= " & lapso2Hasta & ", '2' , " _
        & "             if( (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a." & CampoFecha & ")) >= " & Lapso3Desde & " and (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a." & CampoFecha & ")) <= " & lapso3Hasta & ", '3' , " _
        & "             if( (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_Days(a." & CampoFecha & ")) >= " & Lapso4Desde & ", '4' , '0' " _
        & "             )))) as lapso from jsventracob a " _
        & "             Where " _
        & "             a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
        & "             a.ID_EMP = '" & jytsistema.WorkID & "' " _
        & "             GROUP BY a.codcli, a.nummov " _
        & "             Having (Saldo <> 0.00) " & strBoolean & " ) b on (a.codcli = b.codcli) " _
        & " left join jsvenrenrut d on (a.codcli = d.cliente and a.id_emp = d.id_emp and d.tipo = '0') " _
        & " left join jsvenencrut e on (d.codrut = e.codrut and d.id_emp = e.id_emp and d.tipo = e.tipo  ) " _
        & " Where " _
        & str _
        & " a.id_emp = '" & jytsistema.WorkID & "' " _
        & " group by a.codcli " _
        & " having (saldo0+ saldo1 + saldo2 + saldo3 + saldo4) <> 0 "


    End Function

    Function SeleccionVENVencimientosResumenPlus(ByVal ClienteDesde As String, ByVal ClienteHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                      ByVal FechaHasta As Date, Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                      Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                      Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                      Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                      Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                      Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                      Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                      Optional ByVal TipoCliente As Integer = 4, Optional ByVal EstatusCliente As Integer = 4, _
                                      Optional ByVal Lapso1Desde As Integer = 1, Optional ByVal lapso1Hasta As Integer = 8, _
                                      Optional ByVal Lapso2Desde As Integer = 9, Optional ByVal lapso2Hasta As Integer = 15, _
                                      Optional ByVal Lapso3Desde As Integer = 16, Optional ByVal lapso3Hasta As Integer = 30, _
                                      Optional ByVal Lapso4Desde As Integer = 31, Optional ByVal DesdeEmision As Boolean = True) As String


        Dim str As String = CadenaComplementoClientes("a", ClienteDesde, ClienteHasta, Operador, OrdenadoPor, _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        Dim strBoolean As String = "" ' " and de >= 0 "
        'If Not DesdeEmision Then strBoolean = " and dv >= 0  "

        If AsesorDesde <> "" Then str += " e.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " e.codven <= '" & AsesorHasta & "' and "
        If ZonaDesde <> "" Then str += " e.codzon >= '" & ZonaDesde & "' and "
        If ZonaHasta <> "" Then str += " e.codzon <= '" & ZonaHasta & "' and "
        If RutaDesde <> "" Then str += " e.codrut >= '" & RutaDesde & "' and "
        If RutaHasta <> "" Then str += " e.codrut <= '" & RutaHasta & "' and "

        SeleccionVENVencimientosResumenPlus = "select a.codcli, a.nombre, " _
        & " b.saldo0, b.saldo1, b.saldo2, b.saldo3, b.saldo4 " _
        & " from jsvencatcli a " _
        & " left join ( SELECT a.codcli, SUM(a.saldo0) saldo0 , SUM(a.saldo1) saldo1, SUM(a.saldo2) saldo2, SUM(a.saldo3) saldo3, SUM(a.saldo4) saldo4 " _
        & "             FROM (SELECT a.codcli, a.nummov, IF(a.DE < " & Lapso1Desde & ", a.saldo, 0) saldo0,  IF(a.DE >= " & Lapso1Desde & " AND a.DE <=" & lapso1Hasta & ", a.saldo, 0) saldo1, " _
        & "                     IF(a.DE >= " & Lapso2Desde & " AND a.DE <=" & lapso2Hasta & ", a.saldo, 0) saldo2, IF(a.DE >= " & Lapso3Desde & " AND a.DE <=" & lapso3Hasta & ", a.saldo, 0) saldo3, " _
        & "                     IF(a.DE >= " & Lapso4Desde & ", a.saldo, 0) saldo4 " _
        & "                     FROM (SELECT a.codcli, a.nummov, MIN(a.emision), IFNULL(SUM(a.IMPORTE),0) saldo, " _
        & "                     	    (TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - TO_DAYS(MIN(a.vence))) AS DV, " _
        & "                         	(TO_DAYS('" & ft.FormatoFechaMySQL(FechaHasta) & "') - TO_DAYS(MIN(a.emision))) AS DE " _
        & "                         	FROM jsventracob a " _
        & "                             WHERE " _
        & " 	                        a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
        & "                         	a.id_emp = '" & jytsistema.WorkID & "' " _
        & "                         	GROUP BY a.codcli, a.nummov " _
        & "                         	HAVING ABS(ROUND(saldo,2)) > 0 ) a " _
        & "                     ) a " _
        & "             GROUP BY a.codcli) b on (a.codcli = b.codcli) " _
        & " left join jsvenrenrut d on (a.codcli = d.cliente and a.id_emp = d.id_emp and d.tipo = '0') " _
        & " left join jsvenencrut e on (d.codrut = e.codrut and d.id_emp = e.id_emp and d.tipo = e.tipo  ) " _
        & " Where " _
        & str _
        & " (b.saldo0 + b.saldo1 + b.saldo2 + b.saldo3 + b.saldo4) <> 0 AND " _
        & " a.id_emp = '" & jytsistema.WorkID & "' " _
        & " group by a.codcli "


    End Function


    Function SeleccionVENFacturacionMensual(ByVal ClienteDesde As String, ByVal ClienteHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                      Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                      Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                      Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                      Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                      Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                      Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                      Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                      Optional ByVal TipoCliente As Integer = 4, Optional ByVal EstatusCliente As Integer = 4) As String


        Dim str As String = CadenaComplementoClientes("c", ClienteDesde, ClienteHasta, Operador, "codcli", _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, _
                                                      Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        If AsesorDesde <> "" Then str += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.codven <= '" & AsesorHasta & "' and "
        If TipoCliente < 4 Then str += " c.especial = " & TipoCliente & " and "
        If EstatusCliente < 4 Then str += " c.estatus = " & EstatusCliente & " and "


        SeleccionVENFacturacionMensual = " SELECT a.codcli, concat(c.nombre,a.comen) nombre, c.rif, " _
                & " f.descrip canal, g.descrip tiponegocio, h.descrip zona, i.nomrut ruta, a.codven, concat(a.codven,' ', j.nombres, ' ', j.apellidos) asesor, " _
                & " k.nombre pais, l.nombre estado, m.nombre municipio, n.nombre parroquia, o.nombre ciudad, p.nombre barrio, " _
                & " SUM(IF( MONTH( a.emision) = 1,  a.tot_net, 0 )) + SUM(IF( MONTH( a.emision) = 1,  a.cargos, 0.00) ) - SUM(IF( MONTH( a.emision) = 1,  a.descuen, 0.00)) neto01, " _
                & " SUM(IF( MONTH( a.emision) = 2,  a.tot_net, 0 )) + SUM(IF( MONTH( a.emision) = 2,  a.cargos, 0.00) ) - SUM(IF( MONTH( a.emision) = 2,  a.descuen, 0.00)) neto02, " _
                & " SUM(IF( MONTH( a.emision) = 3,  a.tot_net, 0 )) + SUM(IF( MONTH( a.emision) = 3,  a.cargos, 0.00) ) - SUM(IF( MONTH( a.emision) = 3,  a.descuen, 0.00)) neto03, " _
                & " SUM(IF( MONTH( a.emision) = 4,  a.tot_net, 0 )) + SUM(IF( MONTH( a.emision) = 4,  a.cargos, 0.00) ) - SUM(IF( MONTH( a.emision) = 4,  a.descuen, 0.00)) neto04, " _
                & " SUM(IF( MONTH( a.emision) = 5,  a.tot_net, 0 )) + SUM(IF( MONTH( a.emision) = 5,  a.cargos, 0.00) ) - SUM(IF( MONTH( a.emision) = 5,  a.descuen, 0.00)) neto05, " _
                & " SUM(IF( MONTH( a.emision) = 6,  a.tot_net, 0 )) + SUM(IF( MONTH( a.emision) = 6,  a.cargos, 0.00) ) - SUM(IF( MONTH( a.emision) = 6,  a.descuen, 0.00)) neto06, " _
                & " SUM(IF( MONTH( a.emision) = 7,  a.tot_net, 0 )) + SUM(IF( MONTH( a.emision) = 7,  a.cargos, 0.00) ) - SUM(IF( MONTH( a.emision) = 7,  a.descuen, 0.00)) neto07, " _
                & " SUM(IF( MONTH( a.emision) = 8,  a.tot_net, 0 )) + SUM(IF( MONTH( a.emision) = 8,  a.cargos, 0.00) ) - SUM(IF( MONTH( a.emision) = 8,  a.descuen, 0.00)) neto08, " _
                & " SUM(IF( MONTH( a.emision) = 9,  a.tot_net, 0 )) + SUM(IF( MONTH( a.emision) = 9,  a.cargos, 0.00) ) - SUM(IF( MONTH( a.emision) = 9,  a.descuen, 0.00)) neto09, " _
                & " SUM(IF( MONTH( a.emision) = 10,  a.tot_net, 0 )) + SUM(IF( MONTH( a.emision) = 10,  a.cargos, 0.00) ) - SUM(IF( MONTH( a.emision) = 10,  a.descuen, 0.00)) neto10, " _
                & " SUM(IF( MONTH( a.emision) = 11,  a.tot_net, 0 )) + SUM(IF( MONTH( a.emision) = 11,  a.cargos, 0.00) ) - SUM(IF( MONTH( a.emision) = 11,  a.descuen, 0.00)) neto11, " _
                & " SUM(IF( MONTH( a.emision) = 12,  a.tot_net, 0 )) + SUM(IF( MONTH( a.emision) = 12,  a.cargos, 0.00) ) - SUM(IF( MONTH( a.emision) = 12,  a.descuen, 0.00)) neto12, " _
                & " SUM(IF( MONTH( a.emision) = 1, a.kilos, 0.00)) kilos1,  " _
                & " SUM(IF( MONTH( a.emision) = 2, a.kilos, 0.00)) kilos2,  " _
                & " SUM(IF( MONTH( a.emision) = 3, a.kilos, 0.00)) kilos3,  " _
                & " SUM(IF( MONTH( a.emision) = 4, a.kilos, 0.00)) kilos4,  " _
                & " SUM(IF( MONTH( a.emision) = 5, a.kilos, 0.00)) kilos5,  " _
                & " SUM(IF( MONTH( a.emision) = 1, a.kilos, 0.00)) kilos6,  " _
                & " SUM(IF( MONTH( a.emision) = 1, a.kilos, 0.00)) kilos7,  " _
                & " SUM(IF( MONTH( a.emision) = 1, a.kilos, 0.00)) kilos8,  " _
                & " SUM(IF( MONTH( a.emision) = 1, a.kilos, 0.00)) kilos9,  " _
                & " SUM(IF( MONTH( a.emision) = 1, a.kilos, 0.00)) kilos10, " _
                & " SUM(IF( MONTH( a.emision) = 1, a.kilos, 0.00)) kilos11, " _
                & " SUM(IF( MONTH( a.emision) = 1, a.kilos, 0.00)) kilos12  " _
                & " FROM jsvenencfac a " _
                & " left join jsvencatcli c on (a.codcli = c.codcli and a.id_emp = c.id_emp) " _
                & " left join jsvenliscan f on (c.categoria = f.codigo and c.id_emp = f.ID_EMP) " _
                & " left join jsvenlistip g on (c.unidad = g.codigo and c.categoria = g.antec and c.id_emp = g.id_emp) " _
                & " left join jsconctatab h on (c.zona = h.codigo and c.ID_EMP = h.ID_EMP and h.modulo = '00005') " _
                & " left join jsvenencrut i on (c.ruta_visita = i.codrut and c.ID_EMP = i.id_emp and i.tipo = '0') " _
                & " left join jsvencatven j on (a.codven = j.codven and a.id_emp = j.id_emp and j.tipo = '0' ) " _
                & " left join jsconcatter k on (c.fpais = k.codigo and c.id_emp = k.id_emp) " _
                & " left join jsconcatter l on (c.festado = l.codigo and c.id_emp = l.id_emp) " _
                & " left join jsconcatter m on (c.fmunicipio = m.codigo and c.id_emp = m.id_emp) " _
                & " left join jsconcatter n on (c.fparroquia = n.codigo and c.id_emp = n.id_emp) " _
                & " left join jsconcatter o on (c.fciudad = o.codigo and c.id_emp = o.id_emp) " _
                & " left join jsconcatter p on (c.fbarrio = p.codigo and c.id_emp = p.id_emp) " _
                & " WHERE " _
                & str _
                & " YEAR(a.emision) = " & Year(jytsistema.sFechadeTrabajo) & " AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " GROUP BY a.codcli "


    End Function

    Function SeleccionVENActivacionesClientesYMercancias(ByVal ClienteDesde As String, ByVal ClienteHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                      Optional ByVal FechaInicio As Date = MyDate, Optional ByVal FechaFin As Date = MyDate, _
                                      Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                      Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                      Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                      Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                      Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                      Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                      Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                      Optional ByVal Activados As Integer = 4, Optional ByVal EstatusCliente As Integer = 5, _
                                      Optional ByVal MercanciaDesde As String = "", Optional MercanciaHasta As String = "", _
                                      Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                      Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                      Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                      Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                      Optional ByVal CondicionMercancia As Integer = 4) As String


        Dim str As String = ""
        str += IIf(Activados <= 1, " a.activo = " & Activados & " and ", " a.activo >= 0 and ")
        str += IIf(EstatusCliente = 0, " a.estatus = 0 and ", _
                   IIf(EstatusCliente = 1, " a.estatus = 1 and ", _
                       IIf(EstatusCliente = 2, " a.estatus = 2 and ", _
                           IIf(EstatusCliente = 3, " a.estatus = 3 and ", IIf(EstatusCliente = 4, " a.estatus <= 4 and ", "  ")))))

        If AsesorDesde <> "" Then str += " c.vendedor >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " c.vendedor <= '" & AsesorHasta & "' and "

        str += CadenaComplementoClientes("c", ClienteDesde, ClienteHasta, Operador, "codcli", _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, _
                                                      Parroquia, Ciudad, Barrio)

        str = Mid(str, 1, str.Length - 4)

        Dim strX As String = CadenaComplementoMercancias("c", MercanciaDesde, MercanciaHasta, Operador, "codart", TipoJerarquia, Nivel1, _
                                                         Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, CategoriaDesde, CategoriaHasta, _
                                                         MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta, CondicionMercancia)



        Dim strC As String = " "
        strC += IIf(ClienteDesde <> "", " a.codcli >= '" & ClienteDesde & "' and ", "")
        strC += IIf(ClienteHasta <> "", " a.codcli <= '" & ClienteHasta & "' and ", "")


        SeleccionVENActivacionesClientesYMercancias = " select a.codcli,  " _
        & " a.nombre, a.ingreso, elt( a.estatus+1, 'Activo', 'Bloqueado', 'Inactivo', 'Desincorporado') estatus, d.descrip canal, e.descrip tiponegocio, " _
        & " f.descrip zona, g.nomrut ruta, concat(c.vendedor,' ', h.apellidos,' ', h.nombres) asesor, m.nombre territorio, " _
        & " a.item, b.nomart, i.descrip categoria, j.descrip marca, Elt(b.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') mix, " _
        & " k.descrip tipjer , codjer1, codjer2, codjer3, codjer4, codjer5, codjer6, r.descrip division, " _
        & " a.unidad, a.cantidad, a.peso, a.num_activados, a.activo, a.activoanterior " _
        & " from (select a.codcli, a.nombre, a.ingreso, a.estatus, b.item, b.unidad, b.cantidad, " _
        & "             b.pesoactivacion peso, b.vecesactivacion num_activados, " _
        & "             if(isnull(sum(b.vecesactivacion)),0,1) Activo, " _
        & "             if(isnull(sum(c.vecesactivacion)),0,1) ActivoAnterior " _
        & "             from jsvencatcli a " _
        & "             left join (select a.prov_cli cliente, a.codart item , c.unidad, " _
        & "                        sum(if(isnull(b.uvalencia), if ( a.origen in('FAC','PFC','PVE','NDV') , a.cantidad, -1*a.cantidad) , if ( a.origen in('FAC','PFC','PVE','NDV') , a.cantidad, -1*a.cantidad)/b.equivale)) as cantidad, " _
        & "             sum( if ( a.origen in('FAC','PFC','PVE','NDV'), a.peso, -1*a.peso)) pesoactivacion, count(a.codart) vecesactivacion " _
        & "             from jsmertramer a " _
        & "             left join jsmerequmer b on (a.codart = b.codart and a.unidad = b.uvalencia and a.id_emp = b.id_emp) " _
        & "             left join jsmerctainv c on (a.codart = c.codart and a.id_emp = c.id_emp) " _
        & "             Where " _
        & strX _
        & "             a.fechamov > '" & ft.FormatoFechaMySQL(DateAdd("d", -1, FechaInicio)) & "' and " _
        & "             a.fechamov < '" & ft.FormatoFechaMySQL(DateAdd("d", 1, FechaFin)) & "' and " _
        & "             a.origen in('FAC','PFC','PVE','NCV','NDV') and " _
        & "             a.id_emp = '" & jytsistema.WorkID & "' " _
        & "             group by a.prov_cli, a.codart " & ") b on (a.codcli = b.cliente) " _
        & "             left join (" & " select a.prov_cli cliente, a.codart, c.unidad, " _
        & "                             sum(if(isnull(b.uvalencia), if ( a.origen in('FAC','PFC','PVE','NDV') , a.cantidad, -1*a.cantidad) , if ( a.origen in('FAC','PFC','PVE','NDV') , a.cantidad, -1*a.cantidad)/b.equivale)) as cantidad, " _
        & "                             sum( if ( a.origen in('FAC','PFC','PVE','NDV'), a.peso, -1*a.peso)) pesoactivacion, count(a.codart) vecesactivacion " _
        & "                             from jsmertramer a " _
        & "                             left join jsmerequmer b on (a.codart = b.codart and a.unidad = b.uvalencia and a.id_emp = b.id_emp) " _
        & "                             left join jsmerctainv c on (a.codart = c.codart and a.id_emp = c.id_emp) " _
        & "                             Where " _
        & strX _
        & "                             a.fechamov > '" & ft.FormatoFechaMySQL(DateAdd("d", -1, CDate("01/01/" & Year(FechaInicio)))) & "' and " _
        & "                             a.fechamov < '" & ft.FormatoFechaMySQL(FechaInicio) & "' and " _
        & "                             a.origen in('FAC','PFC','PVE','NCV','NDV') and " _
        & "                             a.id_emp = '" & jytsistema.WorkID & "' " _
        & "                             group by a.prov_cli, a.codart " & ") c on (a.codcli = c.cliente) " _
        & "             Where " _
        & strC _
        & " a.id_emp ='" & jytsistema.WorkID & "' " _
        & " Group By " _
        & " a.codcli, b.item " & ") a " _
        & " left join jsmerctainv b on (a.item = b.codart and b.id_emp = '" & jytsistema.WorkID & "') " _
        & " left join jsvencatcli c on (a.codcli = c.codcli and c.id_emp = '" & jytsistema.WorkID & "')" _
        & " left join jsvenliscan d on (c.categoria = d.codigo and d.id_emp = '" & jytsistema.WorkID & "')" _
        & " left join jsvenlistip e on (c.unidad = e.codigo and e.id_emp = '" & jytsistema.WorkID & "')" _
        & " left join jsconctatab f on (c.zona = f.codigo and f.id_emp = '" & jytsistema.WorkID & "' and f.modulo = '00005') " _
        & " left join jsvenencrut g on (c.ruta_visita = g.codrut and g.id_emp = '" & jytsistema.WorkID & "' and g.tipo = '0') " _
        & " left join jsvencatven h on (c.vendedor = h.codven and h.id_emp = '" & jytsistema.WorkID & "' and h.tipo = '0' and h.estatus = 1) " _
        & " left join jsconctatab i on (b.grupo = i.codigo and i.id_emp = '" & jytsistema.WorkID & "' and i.modulo = '00002') " _
        & " left join jsconctatab j on (b.marca = j.codigo and j.id_emp = '" & jytsistema.WorkID & "' and j.modulo = '00003') " _
        & " left join jsmerencjer k on (k.tipjer = b.tipjer and k.id_emp = '" & jytsistema.WorkID & "') " _
        & " left join jsconcatter l on (c.fpais = l.codigo and c.id_emp = l.id_emp) " _
        & " left join jsconcatter m on (c.festado = m.codigo and c.id_emp = m.id_emp) " _
        & " left join jsconcatter n on (c.fmunicipio = n.codigo and c.id_emp = n.id_emp) " _
        & " left join jsconcatter o on (c.fparroquia = o.codigo and c.id_emp = o.id_emp) " _
        & " left join jsconcatter p on (c.fciudad = p.codigo and c.id_emp = p.id_emp) " _
        & " left join jsconcatter q on (c.fbarrio = q.codigo and c.id_emp = q.id_emp) " _
        & " left join jsmercatdiv r on (b.division = r.division and b.id_emp = r.id_emp) " _
        & " where " _
        & str _
        & " order by  " _
        & " codcli, item "


    End Function


    Function SeleccionVENActivacionesClientesYMercanciasPLUS(ByVal ClienteDesde As String, ByVal ClienteHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                     Optional ByVal FechaInicio As Date = MyDate, Optional ByVal FechaFin As Date = MyDate, _
                                     Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                     Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                     Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                     Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                     Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                     Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                     Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                     Optional ByVal Activados As Integer = 4, Optional ByVal EstatusCliente As Integer = 5, _
                                     Optional ByVal MercanciaDesde As String = "", Optional MercanciaHasta As String = "", _
                                     Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                     Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                     Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                     Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                     Optional ByVal CondicionMercancia As Integer = 4) As String


        Dim str As String = ""
        str += IIf(Activados <= 1, " a.activo = " & Activados & " and ", " a.activo >= 0 and ")
        str += IIf(EstatusCliente = 0, " a.estatus = 0 and ", _
                   IIf(EstatusCliente = 1, " a.estatus = 1 and ", _
                       IIf(EstatusCliente = 2, " a.estatus = 2 and ", _
                           IIf(EstatusCliente = 3, " a.estatus = 3 and ", IIf(EstatusCliente = 4, " a.estatus <= 4 and ", "  ")))))

        If AsesorDesde <> "" Then str += " c.vendedor >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " c.vendedor <= '" & AsesorHasta & "' and "

        str += CadenaComplementoClientes("c", ClienteDesde, ClienteHasta, Operador, "codcli", _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, _
                                                      Parroquia, Ciudad, Barrio)

        str = Mid(str, 1, str.Length - 4)

        Dim strX As String = CadenaComplementoMercancias("c", MercanciaDesde, MercanciaHasta, Operador, "codart", TipoJerarquia, Nivel1, _
                                                         Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, CategoriaDesde, CategoriaHasta, _
                                                         MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta, CondicionMercancia)



        Dim strC As String = " "
        strC += IIf(ClienteDesde <> "", " a.codcli >= '" & ClienteDesde & "' and ", "")
        strC += IIf(ClienteHasta <> "", " a.codcli <= '" & ClienteHasta & "' and ", "")


        SeleccionVENActivacionesClientesYMercanciasPLUS = " select a.codcli,  " _
        & " a.nombre, a.ingreso, elt( a.estatus+1, 'Activo', 'Bloqueado', 'Inactivo', 'Desincorporado') estatus, d.descrip canal, e.descrip tiponegocio, " _
        & " f.descrip zona, g.nomrut ruta, concat(c.vendedor,' ', h.apellidos,' ', h.nombres) asesor, m.nombre territorio, " _
        & " a.item, b.nomart, i.descrip categoria, j.descrip marca, Elt(b.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') mix, " _
        & " k.descrip tipjer , codjer1, codjer2, codjer3, codjer4, codjer5, codjer6, r.descrip division, " _
        & " a.unidad, a.cantidad, a.peso, a.num_activados, a.activo, a.activoanterior " _
        & " from (select a.codcli, a.nombre, a.ingreso, a.estatus, b.item, b.unidad, b.cantidad, " _
        & "             b.pesoactivacion peso, b.vecesactivacion num_activados, " _
        & "             if(isnull(sum(b.vecesactivacion)),0,1) Activo, " _
        & "             if(isnull(sum(c.vecesactivacion)),0,1) ActivoAnterior " _
        & "             from jsvencatcli a " _
        & "             left join (select a.prov_cli cliente, a.codart item , c.unidad, " _
        & "                        sum(if(isnull(b.uvalencia), if ( a.origen in('FAC','PFC','PVE','NDV') , a.cantidad, -1*a.cantidad) , if ( a.origen in('FAC','PFC','PVE','NDV') , a.cantidad, -1*a.cantidad)/b.equivale)) as cantidad, " _
        & "             sum( if ( a.origen in('FAC','PFC','PVE','NDV'), a.peso, -1*a.peso)) pesoactivacion, count(a.codart) vecesactivacion " _
        & "             from jsmertramer a " _
        & "             left join jsmerequmer b on (a.codart = b.codart and a.unidad = b.uvalencia and a.id_emp = b.id_emp) " _
        & "             left join jsmerctainv c on (a.codart = c.codart and a.id_emp = c.id_emp) " _
        & "             Where " _
        & strX _
        & "             a.fechamov > '" & ft.FormatoFechaMySQL(DateAdd("d", -1, FechaInicio)) & "' and " _
        & "             a.fechamov < '" & ft.FormatoFechaMySQL(DateAdd("d", 1, FechaFin)) & "' and " _
        & "             a.origen in('FAC','PFC','PVE','NCV','NDV') and " _
        & "             a.id_emp = '" & jytsistema.WorkID & "' " _
        & "             group by a.prov_cli, a.codart " & ") b on (a.codcli = b.cliente) " _
        & "             left join (" & " select a.prov_cli cliente, a.codart, c.unidad, " _
        & "                             sum(if(isnull(b.uvalencia), if ( a.origen in('FAC','PFC','PVE','NDV') , a.cantidad, -1*a.cantidad) , if ( a.origen in('FAC','PFC','PVE','NDV') , a.cantidad, -1*a.cantidad)/b.equivale)) as cantidad, " _
        & "                             sum( if ( a.origen in('FAC','PFC','PVE','NDV'), a.peso, -1*a.peso)) pesoactivacion, count(a.codart) vecesactivacion " _
        & "                             from jsmertramer a " _
        & "                             left join jsmerequmer b on (a.codart = b.codart and a.unidad = b.uvalencia and a.id_emp = b.id_emp) " _
        & "                             left join jsmerctainv c on (a.codart = c.codart and a.id_emp = c.id_emp) " _
        & "                             Where " _
        & strX _
        & "                             a.fechamov > '" & ft.FormatoFechaMySQL(DateAdd("d", -1, CDate("01/01/" & Year(FechaInicio)))) & "' and " _
        & "                             a.fechamov < '" & ft.FormatoFechaMySQL(FechaInicio) & "' and " _
        & "                             a.origen in('FAC','PFC','PVE','NCV','NDV') and " _
        & "                             a.id_emp = '" & jytsistema.WorkID & "' " _
        & "                             group by a.prov_cli, a.codart " & ") c on (a.codcli = c.cliente) " _
        & "             Where " _
        & strC _
        & " a.id_emp ='" & jytsistema.WorkID & "' " _
        & " Group By " _
        & " a.codcli, b.item " & ") a " _
        & " left join jsmerctainv b on (a.item = b.codart and b.id_emp = '" & jytsistema.WorkID & "') " _
        & " left join jsvencatcli c on (a.codcli = c.codcli and c.id_emp = '" & jytsistema.WorkID & "')" _
        & " left join jsvenliscan d on (c.categoria = d.codigo and d.id_emp = '" & jytsistema.WorkID & "')" _
        & " left join jsvenlistip e on (c.unidad = e.codigo and e.id_emp = '" & jytsistema.WorkID & "')" _
        & " left join jsconctatab f on (c.zona = f.codigo and f.id_emp = '" & jytsistema.WorkID & "' and f.modulo = '00005') " _
        & " left join jsvenencrut g on (c.ruta_visita = g.codrut and g.id_emp = '" & jytsistema.WorkID & "' and g.tipo = '0') " _
        & " left join jsvencatven h on (c.vendedor = h.codven and h.id_emp = '" & jytsistema.WorkID & "' and h.tipo = '0' and h.estatus = 1) " _
        & " left join jsconctatab i on (b.grupo = i.codigo and i.id_emp = '" & jytsistema.WorkID & "' and i.modulo = '00002') " _
        & " left join jsconctatab j on (b.marca = j.codigo and j.id_emp = '" & jytsistema.WorkID & "' and j.modulo = '00003') " _
        & " left join jsmerencjer k on (k.tipjer = b.tipjer and k.id_emp = '" & jytsistema.WorkID & "') " _
        & " left join jsconcatter l on (c.fpais = l.codigo and c.id_emp = l.id_emp) " _
        & " left join jsconcatter m on (c.festado = m.codigo and c.id_emp = m.id_emp) " _
        & " left join jsconcatter n on (c.fmunicipio = n.codigo and c.id_emp = n.id_emp) " _
        & " left join jsconcatter o on (c.fparroquia = o.codigo and c.id_emp = o.id_emp) " _
        & " left join jsconcatter p on (c.fciudad = p.codigo and c.id_emp = p.id_emp) " _
        & " left join jsconcatter q on (c.fbarrio = q.codigo and c.id_emp = q.id_emp) " _
        & " left join jsmercatdiv r on (b.division = r.division and b.id_emp = r.id_emp) " _
        & " where " _
        & str _
        & " order by  " _
        & " codcli, item "


    End Function


    Function SeleccionVENLibroIVA(MyConn As MySqlConnection, lblInfo As Label, _
                                      ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                      Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                      Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                      Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                      Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                      Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                      Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                      Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "") As String


        Dim str As String = CadenaComplementoClientes("c", "", "", 0, "codcli", _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, _
                                                      Parroquia, Ciudad, Barrio)

        If AsesorDesde <> "" Then str += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.codven <= '" & AsesorHasta & "' and "

        Dim tbl As String = "tblIVAVentas"
        Dim aFld() As String = {"numfac.cadena.30.0", "num_control.cadena.30.0", _
            "emision.fecha.0.0", "nompro.cadena.250.0", "tipo.cadena.5.0", "rif.cadena.20.0", "fac_afectada.cadena.20.0", _
            "num_nc.cadena.20.0", "controladorregistro.cadena.150.0", "tipotransaccion.cadena.2.0", "tipoiva.cadena.1.0", _
            "tot_fac.doble.19.2", "nogravado.doble.19.2", "baseiva.doble.19.2", "poriva.doble.6.2", "impiva.doble.19.2", _
            "baseivanc.doble.19.2", "porivanc.doble.6.2", "impivanc.doble.19.2", "retencion.doble.19.2", "num_retencion.cadena.20.0", _
            "fec_retencion.fecha.0.0"}

        ft.Ejecutar_strSQL(myconn, " drop  temporary table if exists " & tbl)
        CrearTabla(MyConn, lblInfo, jytsistema.WorkDataBase, True, "tblIVAVentas", aFld)

        'FACTURAS
        ft.Ejecutar_strSQL(myconn, " insert into " & tbl _
                & " SELECT a.numfac, if( e.num_control is null, '', e.num_control) , a.emision, IF( a.tot_fac = 0.00, 'FACTURA ANULADA', SUBSTRING_INDEX(c.NOMBRE,'{',1)) NOMPRO, " _
                & " IF( a.tot_fac = 0.00, 'NC', IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-','C', IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),3),1) = '-' ,'C','NC') ) )  tipo, IF( a.tot_fac = 0.00, '',  REPLACE(c.rif,'.','')) rif, " _
                & " '' fac_afectada, '' num_nc, CONCAT(a.emision, if(e.num_control is null, '', e.num_control) , '','') controladorregistro, IF( a.tot_fac = 0.00, '03','01') tipotransaccion, " _
                & " IF( b.TIPOIVA IS NULL, d.tipoiva, b.tipoiva) tipoiva, a.tot_fac tot_fac, IF( ISNULL(d.baseiva), 0.00, d.baseiva) Nogravado, " _
                & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-', IF( b.BASEIVA IS NULL, 0.00, b.baseiva), 0.00)  baseiva, " _
                & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-', IF( b.PORIVA IS NULL, 0.00, b.poriva), 0.00) poriva, " _
                & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-', IF( b.IMPIVA IS NULL, 0.00, b.impiva), 0.00) impiva, " _
                & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-', 0.00, IF( b.BASEIVA IS NULL, 0.00, b.baseiva)) baseivanc, " _
                & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-', 0.00, IF( b.PORIVA IS NULL, 0.00, b.poriva)) porivanc, " _
                & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-', 0.00, IF( b.IMPIVA IS NULL, 0.00, b.impiva)) impivanc, " _
                & " 0.00 retencion, '' num_retencion, a.emision fec_retencion FROM jsvenencfac a " _
                & " LEFT JOIN jsvenivafac b ON (a.numfac = b.numfac AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp AND (b.tipoiva <>  'E' OR b.tipoiva <> '') ) " _
                & " LEFT JOIN jsvencatcli c ON (c.codcli = a.codcli AND c.id_emp = a.id_emp) " _
                & " LEFT JOIN jsvenivafac d ON (a.numfac = d.numfac AND a.id_emp = d.id_emp AND (d.tipoiva = '' OR d.tipoiva = 'E') ) " _
                & " LEFT JOIN jsconnumcon e ON (a.numfac = e.numdoc AND a.id_emp = e.id_emp AND e.org = 'FAC' AND e.origen = 'FAC') " _
                & " Where " _
                & str _
                & " SUBSTRING(a.numfac,1,6) <> 'TMPFAC' AND " _
                & " a.tipo = 1 AND " _
                & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
                & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
                & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
                & " having tipoiva <> '' ")

        'FACTURAS EXENTAS
        ft.Ejecutar_strSQL(myconn, " insert into " & tbl _
                & " SELECT a.numfac, if( e.num_control is null, '', e.num_control) , a.emision, IF( a.tot_fac = 0.00, 'FACTURA ANULADA', SUBSTRING_INDEX(c.NOMBRE,'{',1)) NOMPRO, " _
                & " IF( a.tot_fac = 0.00, 'NC', IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-','C', IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),3),1) = '-' ,'C','NC') ) )  tipo, IF( a.tot_fac = 0.00, '',  REPLACE(c.rif,'.','')) rif, " _
                & " '' fac_afectada, '' num_nc, CONCAT(a.emision, if(e.num_control is null, '', e.num_control) , '','') controladorregistro, IF( a.tot_fac = 0.00, '03','01') tipotransaccion, " _
                & " '' tipoiva, a.tot_fac tot_fac, IF( ISNULL(d.baseiva), 0.00, d.baseiva) Nogravado, " _
                & " 0.00  baseiva, 0.00 poriva,  0.00 impiva, " _
                & " 0.00  baseivanc, 0.00 porivanc, 0.00 impivanc, " _
                & " 0.00 retencion, '' num_retencion, a.emision fec_retencion FROM jsvenencfac a " _
                & " LEFT JOIN jsvencatcli c ON (c.codcli = a.codcli AND c.id_emp = a.id_emp) " _
                & " LEFT JOIN jsvenivafac d ON (a.numfac = d.numfac AND a.id_emp = d.id_emp AND (d.tipoiva = '' OR d.tipoiva = 'E') ) " _
                & " LEFT JOIN jsconnumcon e ON (a.numfac = e.numdoc AND a.id_emp = e.id_emp AND e.org = 'FAC' AND e.origen = 'FAC') " _
                & " Where " _
                & str _
                & " a.imp_iva = 0.00 and " _
                & " SUBSTRING(a.numfac,1,6) <> 'TMPFAC' AND " _
                & " a.tipo = 1 AND " _
                & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
                & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
                & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "'  ")

        'FACTURA DE PUNTOS DE VENTA
        ft.Ejecutar_strSQL(myconn, " insert into " & tbl _
                & " SELECT a.numfac, if( e.num_control is null, '', e.num_control) , a.emision, IF( a.tot_fac = 0.00, 'FACTURA ANULADA', SUBSTRING_INDEX(a.NOMcli,'{',1)) NOMPRO, " _
                & " IF( a.tot_fac = 0.00, 'NC', IF( LEFT(RIGHT(REPLACE(a.rif,'.',''),2),1) = '-','C', 'NC' ) )  tipo, IF( a.tot_fac = 0.00, '',  REPLACE(a.rif,'.','')) rif, " _
                & " '' fac_afectada, '' num_nc, CONCAT(a.emision, if( e.num_control is null, '', e.num_control), '','') controladorregistro, IF( a.tot_fac = 0.00, '03','01') tipotransaccion, " _
                & " IF( b.TIPOIVA IS NULL, d.tipoiva, b.tipoiva) tipoiva, a.tot_fac tot_fac, IF( ISNULL(d.baseiva), 0.00, d.baseiva) Nogravado, " _
                & " IF( LEFT(RIGHT(REPLACE(a.rif,'.',''),2),1) = '-', IF( b.BASEIVA IS NULL, 0.00, b.baseiva), 0.00)  baseiva, " _
                & " IF( LEFT(RIGHT(REPLACE(a.rif,'.',''),2),1) = '-', IF( b.PORIVA IS NULL, 0.00, b.poriva), 0.00) poriva, " _
                & " IF( LEFT(RIGHT(REPLACE(a.rif,'.',''),2),1) = '-', IF( b.IMPIVA IS NULL, 0.00, b.impiva), 0.00) impiva, " _
                & " IF( LEFT(RIGHT(REPLACE(a.rif,'.',''),2),1) = '-', 0.00, IF( b.BASEIVA IS NULL, 0.00, b.baseiva)) baseivanc, " _
                & " IF( LEFT(RIGHT(REPLACE(a.rif,'.',''),2),1) = '-', 0.00, IF( b.PORIVA IS NULL, 0.00, b.poriva)) porivanc, " _
                & " IF( LEFT(RIGHT(REPLACE(a.rif,'.',''),2),1) = '-', 0.00, IF( b.IMPIVA IS NULL, 0.00, b.impiva)) impivanc, " _
                & " 0.00 retencion, '' num_retencion, a.emision fec_retencion FROM jsvenencpos a " _
                & " LEFT JOIN jsvenivapos b ON (a.numfac = b.numfac AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp AND (b.tipoiva <>  'E' OR b.tipoiva <> '') ) " _
                & " LEFT JOIN jsvenivapos d ON (a.numfac = d.numfac AND a.id_emp = d.id_emp AND (d.tipoiva = '' OR d.tipoiva = 'E') ) " _
                & " LEFT JOIN jsconnumcon e ON (a.numfac = e.numdoc AND a.id_emp = e.id_emp AND e.org = 'PVE' AND e.origen = 'PVE') " _
                & " Where " _
                & " left(a.numfac,2) <> 'NC' and " _
                & " left(a.numfac,6) <> 'TMPFAC' AND " _
                & " " _
                & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
                & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
                & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "' having tipoiva <> '' ")

        ' FACTURAS PUNTOS DE VENTA EXENTAS
        ft.Ejecutar_strSQL(myconn, " insert into " & tbl _
                & " SELECT a.numfac, if( e.num_control is null, '', e.num_control) , a.emision, IF( a.tot_fac = 0.00, 'FACTURA ANULADA', SUBSTRING_INDEX(a.NOMcli,'{',1)) NOMPRO, " _
                & " IF( a.tot_fac = 0.00, 'NC', IF( LEFT(RIGHT(REPLACE(a.rif,'.',''),2),1) = '-','C', 'NC' ) )  tipo, IF( a.tot_fac = 0.00, '',  REPLACE(a.rif,'.','')) rif, " _
                & " '' fac_afectada, '' num_nc, CONCAT(a.emision, if( e.num_control is null, '', e.num_control), '','') controladorregistro, IF( a.tot_fac = 0.00, '03','01') tipotransaccion, " _
                & " '' tipoiva, a.tot_fac tot_fac, IF( ISNULL(d.baseiva), 0.00, d.baseiva) Nogravado, " _
                & " 0.00 baseiva, 0.00 poriva, 0.00 impiva, " _
                & " 0.00 baseivanc, 0.00 porivanc, 0.00 impivanc, " _
                & " 0.00 retencion, '' num_retencion, a.emision fec_retencion FROM jsvenencpos a " _
                & " LEFT JOIN jsvenivapos d ON (a.numfac = d.numfac AND a.id_emp = d.id_emp AND (d.tipoiva = '' OR d.tipoiva = 'E') ) " _
                & " LEFT JOIN jsconnumcon e ON (a.numfac = e.numdoc AND a.id_emp = e.id_emp AND e.org = 'PVE' AND e.origen = 'PVE') " _
                & " Where " _
                & " a.imp_iva = 0.00 and " _
                & " left(a.numfac,2) <> 'NC' and " _
                & " left(a.numfac,6) <> 'TMPFAC' AND " _
                & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
                & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
                & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "' ")

        ''NOTAS DE CREDITO
        ft.Ejecutar_strSQL(myconn, " insert into " & tbl _
            & " SELECT  '' numfac, IF(e.num_control IS NULL, '', e.num_control) num_control, a.EMISION, IF( a.tot_ncr = 0.00, 'NOTA CREDITO ANULADA', SUBSTRING_INDEX(c.nombre,'{',1)) NOMPRO, " _
            & " IF( a.tot_ncr = 0.00, 'NC',  IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-','C', IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),3),1) = '-' ,'C','NC') ) ) tipo, " _
            & " IF( a.tot_ncr = 0.00, '',  REPLACE(c.rif,'.','')) RIF, IF(a.numfac = '', if ( f.numfac is null, '', f.numfac), if( a.numfac is null, '', a.numfac) ) fac_afectada, " _
            & " a.NUMNCR num_nc, CONCAT(a.emision, IF(e.num_control IS NULL, '', e.num_control) )  controladorregistro, IF( a.tot_ncr = 0.00, '03','01') tipotransaccion, IF( ISNULL(b.TIPOIVA), d.tipoiva, b.tipoiva) tipoiva, " _
            & " -1*a.TOT_NCR, IF( ISNULL(d.baseiva), 0.00, -1*d.baseiva) Nogravado, " _
            & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-', -1*IF( ISNULL(b.BASEIVA), 0.00, b.baseiva), 0.00)  BASEIVA, " _
            & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-',  IF( ISNULL(b.PORIVA), 0.00, b.poriva), 0.00) poriva, " _
            & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-', -1*IF( ISNULL(b.impiva), 0.00, b.IMPIVA), 0.00) IMPIVA, " _
            & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-', 0.00, -1*IF( ISNULL(b.BASEIVA), 0.00, b.baseiva)) BASEIVAnc, " _
            & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-', 0.00, IF( ISNULL(b.PORIVA), 0.00, b.poriva)) porivanc, " _
            & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-', 0.00, -1*IF( ISNULL(b.impiva), 0.00, b.IMPIVA)) IMPIVAnc, " _
            & " 0.00 retencion, '' num_retencion, a.emision fec_retencion FROM jsvenencncr a " _
            & " LEFT JOIN jsvenivancr b ON (a.numncr = b.numncr AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp AND (b.tipoiva <> '' AND b.tipoiva <> 'E') ) " _
            & " LEFT JOIN jsvencatcli c ON (c.codcli = a.codcli AND c.ID_EMP = a.ID_EMP) " _
            & " LEFT JOIN jsvenivancr d ON (a.numncr = d.numncr AND a.id_emp = d.id_emp AND (d.tipoiva = '' OR d.tipoiva = 'E') ) " _
            & " LEFT JOIN jsconnumcon e ON (a.numncr = e.numdoc AND a.id_emp = e.id_emp AND e.org = 'NCR' AND e.origen = 'FAC') " _
            & " LEFT JOIN jsvenrenncr f ON (a.numncr = f.numncr AND a.id_emp = f.id_emp AND f.renglon = '00001' and f.aceptado = '1') " _
            & " Where " _
            & str _
            & " SUBSTRING(a.numncr,1,6) <> 'TMPNCR' AND " _
            & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' having tipoiva <> '' ")

        ''NOTAS DE CREDITO EXENTAS
        ft.Ejecutar_strSQL(myconn, " insert into " & tbl _
            & " SELECT  '' numfac, IF(e.num_control IS NULL, '', e.num_control) num_control, a.EMISION, IF( a.tot_ncr = 0.00, 'NOTA CREDITO ANULADA', SUBSTRING_INDEX(c.nombre,'{',1)) NOMPRO, " _
            & " IF( a.tot_ncr = 0.00, 'NC',  IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-','C', IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),3),1) = '-' ,'C','NC') ) ) tipo, " _
            & " IF( a.tot_ncr = 0.00, '',  REPLACE(c.rif,'.','')) RIF, IF(a.numfac = '', if ( f.numfac is null, '', f.numfac), a.numfac) fac_afectada, " _
            & " a.NUMNCR num_nc, CONCAT(a.emision,  IF(e.num_control IS NULL, '', e.num_control) )  controladorregistro, IF( a.tot_ncr = 0.00, '03','01') tipotransaccion, '' tipoiva, " _
            & " -1*a.TOT_NCR, IF( ISNULL(d.baseiva), 0.00, -1*d.baseiva) Nogravado, " _
            & " 0.00 BASEIVA, 0.00 poriva, 0.00 IMPIVA, " _
            & " 0.00 BASEIVAnc, 0.00 porivanc, 0.00 IMPIVAnc, " _
            & " 0.00 retencion, '' num_retencion, a.emision fec_retencion FROM jsvenencncr a " _
            & " LEFT JOIN jsvencatcli c ON (c.codcli = a.codcli AND c.ID_EMP = a.ID_EMP) " _
            & " LEFT JOIN jsvenivancr d ON (a.numncr = d.numncr AND a.id_emp = d.id_emp AND (d.tipoiva = '' OR d.tipoiva = 'E') ) " _
            & " LEFT JOIN jsconnumcon e ON (a.numncr = e.numdoc AND a.id_emp = e.id_emp AND e.org = 'NCR' AND e.origen = 'FAC') " _
            & " LEFT JOIN jsvenrenncr f ON (a.numncr = f.numncr AND a.id_emp = f.id_emp AND f.renglon = '00001' and f.aceptado = '1') " _
            & " Where " _
            & str _
            & " a.imp_iva = 0.00 AND " _
            & " SUBSTRING(a.numncr,1,6) <> 'TMPNCR' AND " _
            & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' ")

        ''NOTAS DE CREDITO PUNTOS DE VENTA
        ft.Ejecutar_strSQL(myconn, " insert into  " & tbl _
            & " SELECT '' numfac, if(e.num_control is null, a.numfac, e.num_control) num_control, a.emision,  a.nomcli nompro, IF ( LEFT(RIGHT(g.rif,2),1) = '-' , 'C' , 'NC') tipo, IF( g.rif IS NULL, REPLACE(a.rif,'.',''), g.rif) rif , " _
            & " a.refer fac_afectada, if(e.num_control is null, a.numfac, e.num_control) num_nc, CONCAT(a.emision, IF( e.num_control IS NULL, a.numfac, e.num_control), a.refer) controladorregistro, IF( a.tot_fac = 0.00, '03','01') tipotransaccion,  " _
            & " c.tipoiva, -1*SUM(a.tot_fac), -1*b.nogravado, -1*c.baseiva, c.poriva, -1*c.impiva, -1*c.baseivanc, c.poriva, -1*c.impivanc, 0.00, '', a.emision " _
            & " FROM jsvenencpos a " _
            & " LEFT JOIN jsconnumcon e ON (a.numfac = e.numdoc AND a.id_emp = e.id_emp AND e.org = 'PVE' AND e.origen = 'PVE') " _
            & " LEFT JOIN ( SELECT a.emision, a.numfac, SUM(IF( ISNULL(d.baseiva), 0.00, d.baseiva)) Nogravado, a.id_emp " _
            & "             FROM jsvenencpos a " _
            & "             LEFT JOIN jsvenivapos d ON (a.numfac = d.numfac AND a.id_emp = d.id_emp AND d.tipoiva = '' ) " _
            & "             Where " _
            & "             SUBSTRING(a.numfac ,1,2 ) = 'NC' AND a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & "             a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.emision, a.numfac ) b ON (a.emision = b.emision AND a.numfac = b.numfac AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN (SELECT a.emision, a.numfac, a.codcli,  SUM( IF ( LEFT(RIGHT(e.rif,2),1) = '-',   IF( ISNULL(d.baseiva), 0.00, d.baseiva) , 0.00  )) baseiva, SUM( IF ( LEFT(RIGHT(e.rif,2),1) <> '-' OR LEFT(RIGHT(e.rif,2),1) IS NULL ,   IF( ISNULL(d.baseiva), 0.00, d.baseiva) , 0.00  )) baseivanc, IF(   MAX(d.poriva) IS NULL, 0.00, MAX(d.poriva)   ) poriva, IF( MAX(d.tipoiva) IS NULL, '', MAX(d.tipoiva)) tipoiva, SUM(  IF ( LEFT(RIGHT(e.rif,2),1) = '-',  IF( ISNULL(d.impiva), 0.00, d.impiva) , 0.00 ) ) impiva, SUM(  IF ( LEFT(RIGHT(e.rif,2),1) <> '-' OR LEFT(RIGHT(e.rif,2),1) IS NULL,  IF( ISNULL(d.impiva), 0.00, d.impiva) , 0.00 ) ) impivanc,  a.id_emp " _
            & "            FROM jsvenencpos a " _
            & "              LEFT JOIN jsvenivapos d ON (a.numfac = d.numfac AND a.id_emp = d.id_emp AND d.tipoiva = 'A') " _
            & "              LEFT JOIN jsvencatcli e ON (a.codcli = e.codcli AND a.id_emp = e.id_emp) " _
            & "              Where " _
            & "             SUBSTRING(a.numfac ,1,2 ) = 'NC' AND a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & "              a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.emision, a.numfac ) c  ON ( a.emision = c.emision AND a.numfac = c.numfac AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsvencatcaj f ON (a.codcaj = f.codcaj AND a.id_emp = f.id_emp) " _
            & " LEFT JOIN jsvencatcli g ON (a.codcli = g.codcli AND a.id_emp = g.id_emp) " _
            & " Where " _
            & "  NOT c.poriva IS NULL AND SUBSTRING(a.numfac,1,2) = 'NC' AND a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "'  AND a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' group by a.emision, a.numfac having tipoiva <> '' ")

        ''NOTAS DE CREDITO PUNTOS DE VENTA EXENTAS
        ft.Ejecutar_strSQL(myconn, " insert into  " & tbl _
            & " SELECT '' numfac, if(e.num_control is null, a.numfac, e.num_control) num_control, a.emision,  a.nomcli nompro, IF ( LEFT(RIGHT(g.rif,2),1) = '-' , 'C' , 'NC') tipo, IF( g.rif IS NULL, REPLACE(a.rif,'.',''), g.rif) rif , " _
            & " a.refer fac_afectada, if(e.num_control is null, a.numfac, e.num_control) num_nc, CONCAT(a.emision, IF( e.num_control IS NULL, a.numfac, e.num_control), a.refer) controladorregistro, IF( a.tot_fac = 0.00, '03','01') tipotransaccion,  " _
            & " '' tipoiva, -1*SUM(a.tot_fac), -1*b.nogravado, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, '', a.emision " _
            & " FROM jsvenencpos a " _
            & " LEFT JOIN jsconnumcon e ON (a.numfac = e.numdoc AND a.id_emp = e.id_emp AND e.org = 'PVE' AND e.origen = 'PVE') " _
            & " LEFT JOIN ( SELECT a.emision, a.numfac, SUM(IF( ISNULL(d.baseiva), 0.00, d.baseiva)) Nogravado, a.id_emp " _
            & "             FROM jsvenencpos a " _
            & "             LEFT JOIN jsvenivapos d ON (a.numfac = d.numfac AND a.id_emp = d.id_emp AND d.tipoiva = '' ) " _
            & "             Where " _
            & "             SUBSTRING(a.numfac ,1,2 ) = 'NC' AND a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & "             a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.emision, a.numfac ) b ON (a.emision = b.emision AND a.numfac = b.numfac AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvencatcaj f ON (a.codcaj = f.codcaj AND a.id_emp = f.id_emp) " _
            & " LEFT JOIN jsvencatcli g ON (a.codcli = g.codcli AND a.id_emp = g.id_emp) " _
            & " Where " _
            & " a.imp_iva = 0.00 and " _
            & " SUBSTRING(a.numfac,1,2) = 'NC' AND a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "'  AND a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' group by a.emision, a.numfac having tipoiva <> '' ")

        ''NOTAS DEBITO
        ft.Ejecutar_strSQL(myconn, " insert into " & tbl _
            & " SELECT  '' numfac, IF(e.num_control IS NULL, '', e.num_control) num_control, a.EMISION,IF( a.tot_ndb = 0.00, 'NOTA DEBITO ANULADA', SUBSTRING_INDEX(c.nombre,'{',1)) NOMPRO, " _
            & " IF( a.tot_ndb = 0.00, 'NC',  IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-','C', IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),3),1) = '-' ,'C','NC') ) ) tipo, " _
            & " IF( a.tot_ndb = 0.00, '',  REPLACE(c.rif,'.','')) RIF,  IF(a.numfac = '', if( f.numfac is null, '', f.numfac) , if( a.numfac is null, '', a.numfac) ) fac_afectada, " _
            & " a.NUMNDB num_nc, CONCAT(a.emision, IF(e.num_control IS NULL, '', e.num_control) )  controladorregistro, IF( a.tot_ndb = 0.00, '03','01') tipotransaccion, IF( ISNULL(b.TIPOIVA), d.tipoiva, b.tipoiva) tipoiva, " _
            & " a.TOT_NDB, IF( ISNULL(d.baseiva), 0.00, d.baseiva) Nogravado, " _
            & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-', IF( ISNULL(b.BASEIVA), 0.00, b.baseiva), 0.00)  BASEIVA, " _
            & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-',  IF( ISNULL(b.PORIVA), 0.00, b.poriva), 0.00) poriva, " _
            & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-', IF( ISNULL(b.impiva), 0.00, b.IMPIVA), 0.00) IMPIVA, " _
            & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-', 0.00, IF( ISNULL(b.BASEIVA), 0.00, b.baseiva)) BASEIVAnc, " _
            & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-', 0.00, IF( ISNULL(b.PORIVA), 0.00, b.poriva)) porivanc, " _
            & " IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-', 0.00, IF( ISNULL(b.impiva), 0.00, b.IMPIVA)) IMPIVAnc, " _
            & " 0.00 retencion, '' num_retencion, a.emision fec_retencion FROM jsvenencndb a " _
            & " LEFT JOIN jsvenivandb b ON (a.numndb = b.numndb AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp AND (b.tipoiva <> '' AND b.tipoiva <> 'E') ) " _
            & " LEFT JOIN jsvencatcli c ON (c.codcli = a.codcli AND c.ID_EMP = a.ID_EMP) " _
            & " LEFT JOIN jsvenivandb d ON (a.numndb = d.numndb AND a.id_emp = d.id_emp AND (d.tipoiva = '' OR d.tipoiva = 'E') ) " _
            & " LEFT JOIN jsconnumcon e ON (a.numndb = e.numdoc AND a.id_emp = e.id_emp AND e.org in ('NDB','BAN') AND e.origen = 'FAC') " _
            & " LEFT JOIN jsvenrenndb f ON (a.numndb = f.numndb AND a.id_emp = f.id_emp AND f.renglon = '00001' and f.aceptado = '1') " _
            & " Where " _
            & str _
            & " SUBSTRING(a.numndb,1,6) <> 'TMPNDB' AND " _
            & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' having tipoiva <> '' ")

        ft.Ejecutar_strSQL(myconn, " insert into " & tbl _
            & " SELECT  '' numfac, IF(e.num_control IS NULL, '', e.num_control) num_control, a.EMISION,IF( a.tot_ndb = 0.00, 'NOTA DEBITO ANULADA', SUBSTRING_INDEX(c.nombre,'{',1)) NOMPRO, " _
            & " IF( a.tot_ndb = 0.00, 'NC',  IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),2),1) = '-','C', IF( LEFT(RIGHT(REPLACE(c.rif,'.',''),3),1) = '-' ,'C','NC') ) ) tipo, " _
            & " IF( a.tot_ndb = 0.00, '',  REPLACE(c.rif,'.','')) RIF, IF(a.numfac = '', if ( f.numfac is null, '', f.numfac), a.numfac) fac_afectada, " _
            & " a.numndb num_nc, CONCAT(a.emision, IF(e.num_control IS NULL, '', e.num_control) )  controladorregistro, IF( a.tot_ndb = 0.00, '03','01') tipotransaccion, '' tipoiva, " _
            & " a.TOT_ndb, IF( ISNULL(d.baseiva), 0.00, d.baseiva) Nogravado, " _
            & " 0.00 BASEIVA, 0.00 poriva, 0.00 IMPIVA, " _
            & " 0.00 BASEIVAnc, 0.00 porivanc, 0.00 IMPIVAnc, " _
            & " 0.00 retencion, '' num_retencion, a.emision fec_retencion FROM jsvenencndb a " _
            & " LEFT JOIN jsvencatcli c ON (c.codcli = a.codcli AND c.ID_EMP = a.ID_EMP) " _
            & " LEFT JOIN jsvenivandb d ON (a.numndb = d.numndb AND a.id_emp = d.id_emp AND (d.tipoiva = '' OR d.tipoiva = 'E') ) " _
            & " LEFT JOIN jsconnumcon e ON (a.numndb = e.numdoc AND a.id_emp = e.id_emp AND e.org in('NDB','BAN') AND e.origen = 'FAC') " _
            & " LEFT JOIN jsvenrenndb f ON (a.numndb = f.numndb AND a.id_emp = f.id_emp AND f.renglon = '00001' and f.aceptado = '1') " _
            & " Where " _
            & str _
            & " a.imp_iva = 0.00 AND " _
            & " SUBSTRING(a.numndb,1,6) <> 'TMPNCR' AND " _
            & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' ")


        ''FACTURAS ANULADAS
        ft.Ejecutar_strSQL(myconn, " insert into " & tbl _
                & " select '.', num_control, emision, 'NUMERO DE CONTROL ANULADO', '', '', " _
                & " '', '',  concat(emision, num_control), '03', '', 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, " _
                & " 0.00, '', emision " _
                & " from jsconnumcon a WHERE " _
                & " a.org = 'CON' and origen in ('FAC','NCR')  and " _
                & " substring(a.numdoc,1,6) <> 'TMPFAC' and " _
                & " substring(a.numdoc,1,6) <> 'TMPNCR' and " _
                & " a.EMISION >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
                & " a.EMISION <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "'")


        ''RETENCIONES IVA
        ft.Ejecutar_strSQL(myconn, " INSERT INTO " & tbl _
                & " select '', '', a.fechasi, SUBSTRING_INDEX(c.NOMBRE,'{',1), if( left(right(REPLACE(c.rif,'.',''),2),1) = '-','C', 'NC' ), REPLACE(c.rif,'.',''),  " _
                & " a.nummov , '',  concat(a.fechasi, a.nummov),  " _
                & " '02', 'A', 0.00 , 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, " _
                & " if( a.tipomov = 'NC',  abs(a.importe), -1*abs(a.importe) ), a.refer, a.emision FROM jsventracob a " _
                & " left join jsvencatcli c on (c.codcli = a.codcli And c.id_emp = a.id_emp) " _
                & " where " _
                & str _
                & " a.tipomov in ('NC','ND') and " _
                & " substring(a.concepto,1,30)  = 'RETENCION IVA CLIENTE ESPECIAL' and " _
                & " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
                & " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
                & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "'")

        '//////////////////////////////////////////////////////////
        SeleccionVENLibroIVA = " select a.numfac, a.num_control, a.emision, a.nompro, a.tipo, a.rif, " _
                & " a.fac_afectada, a.num_nc, a.controladorregistro, a.tipotransaccion, a.tipoiva, a.tot_fac, a.nogravado, " _
                & " if( a.poriva = 0.00 , 0.00, a.baseiva ) baseiva, a.poriva, if( a.poriva = 0.00 , 0.00, a.impiva ) impiva, " _
                & " if( a.porivanc = 0.00 , 0.00, a.baseivanc ) baseivanc, a.porivanc, if( a.porivanc = 0.00 , 0.00, a.impivanc ) impivanc, " _
                & " a.retencion, a.num_retencion, a.fec_retencion " _
                & " from " & tbl & " a order by a.controladorregistro, a.tipoiva "


    End Function

    Function SeleccionVENActivacionesClientes(ByVal ClienteDesde As String, ByVal ClienteHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                      Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                      Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                      Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                      Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                      Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                      Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                      Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                      Optional ByVal FechaDesde As Date = MyDate, Optional ByVal FechaHasta As Date = MyDate, _
                                      Optional ByVal TipoCliente As Integer = 4, Optional ByVal EstatusCliente As Integer = 9) As String


        Dim str As String = CadenaComplementoClientes("a", ClienteDesde, ClienteHasta, Operador, OrdenadoPor, _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        If AsesorDesde <> "" Then str += " j.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " j.codven <= '" & AsesorHasta & "' and "
        If ZonaDesde <> "" Then str += " h.codigo >= '" & ZonaDesde & "' and "
        If ZonaHasta <> "" Then str += " h.codigo <= '" & ZonaHasta & "' and "
        If RutaDesde <> "" Then str += " i.codrut >= '" & RutaDesde & "' and "
        If RutaHasta <> "" Then str += " i.codrut <= '" & RutaHasta & "' and "

        SeleccionVENActivacionesClientes = "Select a.codcli, " _
                        & " elt(a.estatus+1,'ACTIVO','BLOQUEADO','INACTIVO','DESINCORPORADO') estatus, " _
                        & " a.alterno, a.nombre, if( isNULL(b.actfac ) , 0, b.actfac)  actfac, " _
                        & " if( isNULL(c.actabn ) , 0, c.actabn)  actabn, if( isNULL(c.actcan ) , 0, c.actcan)  actcan, " _
                        & " if( isNULL(c.chdmes ) , 0, c.chdmes)  chdmes, if( isNULL(d.chdaño ) , 0, d.chdaño)  chdaño, " _
                        & " if( d.chdaño > ven_cheqdev, 'Bloq.','') stcd, f.descrip canal, g.descrip tiponegocio, i.nomrut ruta,   " _
                        & " h.descrip zona, concat(j.codven,' ',j.apellidos, ' ' , j.nombres) asesor, " _
                        & " r.nombre pais, q.nombre estado, p.nombre municipio, o.nombre parroquia, n.nombre ciudad, m.nombre barrio " _
                        & " from jsvencatcli a " _
                        & " left join  ( select a.codcli, count(a.codcli) ACTFAC " _
                        & "                 from jsvenencfac a " _
                        & "                 Where " _
                        & "                 a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and " _
                        & "                 a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
                        & "                 a.id_emp = '" & jytsistema.WorkID & "' " _
                        & "                 group by a.codcli ) b on (a.codcli = b.codcli) " _
                        & " left join ( select a.codcli, " _
                        & "                 sum(  if( a.tipomov IN ('AB'), 1,  0) ) actabn, " _
                        & "                 sum(  if( a.tipomov IN ('CA'), 1,  0) ) actcan, " _
                        & "                 sum(  if( a.tipomov IN ('ND') and mid(a.nummov,1,2)='CD' , 1,  0) ) chdmes " _
                        & "                 from jsventracob a " _
                        & "                 Where " _
                        & "                 a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and " _
                        & "                 a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
                        & "                 a.tipomov in ('AB','CA', 'ND') and " _
                        & "                 a.id_emp = '" & jytsistema.WorkID & "' " _
                        & "                 group by a.codcli ) c on (a.codcli = c.codcli) " _
                        & " left join (select prov_cli codcli, IFNULL(COUNT(*),0) chdaño from jsbanchedev where year(fechadev) = '" & Year(FechaDesde) & "' and id_emp = '" & jytsistema.WorkID & "' group by 1) d on (a.codcli = d.codcli) " _
                        & " left join jsconctaemp e on (a.id_emp = e.id_emp) " _
                        & " left join jsvenliscan f on (a.categoria = f.codigo and a.id_emp = f.id_emp ) " _
                        & " left join jsvenlistip g on (a.unidad = g.codigo and a.id_emp = g.id_emp ) " _
                        & " left join jsconctatab h on (a.zona = h.codigo and a.id_emp = h.id_emp and h.modulo = '00005')" _
                        & " left join jsvenencrut i on (a.ruta_visita = i.codrut and a.id_emp = i.id_emp and i.tipo = '0')" _
                        & " left join jsvencatven j on (a.vendedor = j.codven and a.id_emp = j.id_emp and j.tipo = '0' and j.estatus = 1) " _
                        & " left join jsconcatter m on (a.fbarrio = m.codigo and a.id_emp = m.id_emp ) " _
                        & " left join jsconcatter n on (a.fciudad = n.codigo and a.id_emp = n.id_emp ) " _
                        & " left join jsconcatter o on (a.fparroquia = o.codigo and a.id_emp = o.id_emp ) " _
                        & " left join jsconcatter p on (a.fmunicipio = p.codigo and a.id_emp = p.id_emp ) " _
                        & " left join jsconcatter q on (a.festado = q.codigo and a.id_emp = q.id_emp ) " _
                        & " left join jsconcatter r on (a.fpais = r.codigo and a.id_emp = r.id_emp ) " _
                        & " Where " _
                        & str _
                        & " a.id_emp = '" & jytsistema.WorkID & "' " _
                        & " group by a.codcli "

        '& " and a.estatus < 3 " _

    End Function

    Function SeleccionVENActivacionesClientesMES(ByVal ClienteDesde As String, ByVal ClienteHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                     Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                     Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                     Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                     Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                     Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                     Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                     Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                     Optional ByVal FechaDesde As Date = MyDate, Optional ByVal FechaHasta As Date = MyDate, _
                                     Optional ByVal TipoCliente As Integer = 4, Optional ByVal EstatusCliente As Integer = 9) As String


        Dim str As String = CadenaComplementoClientes("a", ClienteDesde, ClienteHasta, Operador, OrdenadoPor, _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        If AsesorDesde <> "" Then str += " j.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " j.codven <= '" & AsesorHasta & "' and "
        If ZonaDesde <> "" Then str += " h.codigo >= '" & ZonaDesde & "' and "
        If ZonaHasta <> "" Then str += " h.codigo <= '" & ZonaHasta & "' and "
        If RutaDesde <> "" Then str += " i.codrut >= '" & RutaDesde & "' and "
        If RutaHasta <> "" Then str += " i.codrut <= '" & RutaHasta & "' and "

        SeleccionVENActivacionesClientesMES = "Select a.codcli, " _
                        & " elt(a.estatus+1,'ACTIVO','BLOQUEADO','INACTIVO','DESINCORPORADO') estatus, " _
                        & " a.alterno, a.nombre, if( isNULL(b.actfac ) , 0, b.actfac)  actfac, " _
                        & " if( isNULL(b.actfac01 ) , 0, b.actfac01)  actfac01, if( isNULL(b.actfac02 ) , 0, b.actfac02)  actfac02, " _
                        & " if( isNULL(b.actfac03 ) , 0, b.actfac03)  actfac03, if( isNULL(b.actfac04 ) , 0, b.actfac04)  actfac04, " _
                        & " if( isNULL(b.actfac05 ) , 0, b.actfac05)  actfac05, if( isNULL(b.actfac06 ) , 0, b.actfac06)  actfac06, " _
                        & " if( isNULL(b.actfac07 ) , 0, b.actfac07)  actfac07, if( isNULL(b.actfac08 ) , 0, b.actfac08)  actfac08, " _
                        & " if( isNULL(b.actfac09 ) , 0, b.actfac09)  actfac09, if( isNULL(b.actfac10 ) , 0, b.actfac10)  actfac10, " _
                        & " if( isNULL(b.actfac11 ) , 0, b.actfac11)  actfac11, if( isNULL(b.actfac12 ) , 0, b.actfac12)  actfac12, " _
                        & " if( isNULL(c.actfacant01 ) , 0, c.actfacant01)  actfacant01, if( isNULL(c.actfacant02 ) , 0, c.actfacant02)  actfacant02, " _
                        & " if( isNULL(c.actfacant03 ) , 0, c.actfacant03)  actfacant03, if( isNULL(c.actfacant04 ) , 0, c.actfacant04)  actfacant04, " _
                        & " if( isNULL(c.actfacant05 ) , 0, c.actfacant05)  actfacant05, if( isNULL(c.actfacant06 ) , 0, c.actfacant06)  actfacant06, " _
                        & " if( isNULL(c.actfacant07 ) , 0, c.actfacant07)  actfacant07, if( isNULL(c.actfacant08 ) , 0, c.actfacant08)  actfacant08, " _
                        & " if( isNULL(c.actfacant09 ) , 0, c.actfacant09)  actfacant09, if( isNULL(c.actfacant10 ) , 0, c.actfacant10)  actfacant10, " _
                        & " if( isNULL(c.actfacant11 ) , 0, c.actfacant11)  actfacant11, if( isNULL(c.actfacant12 ) , 0, c.actfacant12)  actfacant12, " _
                        & " f.descrip canal, g.descrip tiponegocio, i.nomrut ruta,   " _
                        & " h.descrip zona, concat(j.codven,' ',j.apellidos, ' ' , j.nombres) asesor, " _
                        & " r.nombre pais, q.nombre estado, p.nombre municipio, o.nombre parroquia, n.nombre ciudad, m.nombre barrio " _
                        & " from jsvencatcli a " _
                        & " left join  ( select a.codcli, count(a.codcli) ACTFAC,  " _
                        & "                 COUNT( IF( MONTH(a.emision) = 1,  a.codcli, NULL) ) ACTFAC01, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 2,  a.codcli, NULL) ) ACTFAC02, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 3,  a.codcli, NULL) ) ACTFAC03, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 4,  a.codcli, NULL) ) ACTFAC04, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 5,  a.codcli, NULL) ) ACTFAC05, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 6,  a.codcli, NULL) ) ACTFAC06, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 7,  a.codcli, NULL) ) ACTFAC07, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 8,  a.codcli, NULL) ) ACTFAC08, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 9,  a.codcli, NULL) ) ACTFAC09, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 10,  a.codcli, NULL) ) ACTFAC10, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 11,  a.codcli, NULL) ) ACTFAC11, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 12,  a.codcli, NULL) ) ACTFAC12 " _
                        & "                 from jsvenencfac a " _
                        & "                 Where " _
                        & "                 a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and " _
                        & "                 a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
                        & "                 a.id_emp = '" & jytsistema.WorkID & "' " _
                        & "                 group by a.codcli ) b on (a.codcli = b.codcli) " _
                        & " left join  ( select a.codcli, count(a.codcli) ACTFACANT,  " _
                        & "                 COUNT( IF( MONTH(a.emision) = 1,  a.codcli, NULL) ) ACTFACANT01, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 2,  a.codcli, NULL) ) ACTFACANT02, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 3,  a.codcli, NULL) ) ACTFACANT03, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 4,  a.codcli, NULL) ) ACTFACANT04, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 5,  a.codcli, NULL) ) ACTFACANT05, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 6,  a.codcli, NULL) ) ACTFACANT06, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 7,  a.codcli, NULL) ) ACTFACANT07, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 8,  a.codcli, NULL) ) ACTFACANT08, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 9,  a.codcli, NULL) ) ACTFACANT09, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 10,  a.codcli, NULL) ) ACTFACANT10, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 11,  a.codcli, NULL) ) ACTFACANT11, " _
                        & "                 COUNT( IF( MONTH(a.emision) = 12,  a.codcli, NULL) ) ACTFACANT12 " _
                        & "                 from jsvenencfac a " _
                        & "                 Where " _
                        & "                 a.emision >= '" & ft.FormatoFechaMySQL(DateAdd(DateInterval.Year, -1, FechaDesde)) & "' and " _
                        & "                 a.emision <= '" & ft.FormatoFechaMySQL(DateAdd(DateInterval.Year, -1, FechaHasta)) & "' and " _
                        & "                 a.id_emp = '" & jytsistema.WorkID & "' " _
                        & "                 group by a.codcli ) c on (a.codcli = c.codcli) " _
                        & " left join jsvenliscan f on (a.categoria = f.codigo and a.id_emp = f.id_emp ) " _
                        & " left join jsvenlistip g on (a.unidad = g.codigo and a.id_emp = g.id_emp ) " _
                        & " left join jsconctatab h on (a.zona = h.codigo and a.id_emp = h.id_emp and h.modulo = '00005')" _
                        & " left join jsvenencrut i on (a.ruta_visita = i.codrut and a.id_emp = i.id_emp and i.tipo = '0')" _
                        & " left join jsvencatven j on (a.vendedor = j.codven and a.id_emp = j.id_emp and j.tipo = '0' and j.estatus = 1) " _
                        & " left join jsconcatter m on (a.fbarrio = m.codigo and a.id_emp = m.id_emp ) " _
                        & " left join jsconcatter n on (a.fciudad = n.codigo and a.id_emp = n.id_emp ) " _
                        & " left join jsconcatter o on (a.fparroquia = o.codigo and a.id_emp = o.id_emp ) " _
                        & " left join jsconcatter p on (a.fmunicipio = p.codigo and a.id_emp = p.id_emp ) " _
                        & " left join jsconcatter q on (a.festado = q.codigo and a.id_emp = q.id_emp ) " _
                        & " left join jsconcatter r on (a.fpais = r.codigo and a.id_emp = r.id_emp ) " _
                        & " Where " _
                        & str _
                        & " a.id_emp = '" & jytsistema.WorkID & "' " _
                        & " group by a.codcli "

    End Function

    'Function SeleccionVENPedidosSinFacturar(ByVal ClienteDesde As String, ByVal ClienteHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
    '                                Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
    '                                Optional ByVal FechaDesde As Date = MyDate, Optional ByVal FechaHasta As Date = MyDate) As String

    '    Dim str As String = CadenaComplementoClientes("f", ClienteDesde, ClienteHasta, Operador, OrdenadoPor)

    '    If AsesorDesde <> "" Then str += " g.codven >= '" & AsesorDesde & "' and "
    '    If AsesorHasta <> "" Then str += " g.codven <= '" & AsesorHasta & "' and "

    '    SeleccionVENPedidosSinFacturar = "  select a.item , b.nomart, b.unidad, sum( if( c.uvalencia is null,a.cantran,a.cantran/c.equivale)) cant_sin , " _
    '                & "                 b.montoultimacompra costo, d.nombre, " _
    '                & "                 sum( if( c.uvalencia is null,a.cantran,a.cantran/c.equivale))*b.montoultimacompra total_ren, " _
    '                & "                 b.tipjer, e.codcli, e.codven, f.nombre as cliente,concat(g.codven, ' ', g.apellidos,' ',g.nombres) as vendedor, " _
    '                & "                 sum( if( c.uvalencia is null,a.cantran,a.cantran/c.equivale))*b.pesounidad as pesorenglon, e.emision " _
    '                & "     from jsvenrenped a " _
    '                & "     left join jsmerequmer c on ( a.item = c.codart and a.unidad = c.uvalencia and a.id_emp=c.id_emp) " _
    '                & "     left join jsmerctainv b on ( a.item = b.codart and a.id_emp = b.id_emp ) " _
    '                & "     left join jsvenencped e on ( a.numped = e.numped and a.id_emp = e.id_emp ) " _
    '                & "     left join jsvencatcli f on ( e.codcli = f.codcli and e.id_emp = f.id_emp ) " _
    '                & "     left join jsvencatven g on ( e.codven = g.codven and e.id_emp = g.id_emp ) " _
    '                & " where " _
    '                & " e.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and " _
    '                & " e.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
    '                & " a.cantran > 0 and " _
    '                & " a.estatus in ('0','1') and " _
    '                & str _
    '                & " a.id_emp='" & jytsistema.WorkID & "' " _
    '                & " group by 1,2,3 " _
    '                & " order by e.codcli "

    'End Function



    Function SeleccionVENChequesDevueltosMES(ByVal ClienteDesde As String, ByVal ClienteHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                      Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                      Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                      Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                      Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                      Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                      Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                      Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                      Optional ByVal FechaDesde As Date = MyDate, Optional ByVal FechaHasta As Date = MyDate, _
                                      Optional ByVal TipoCliente As Integer = 4, Optional ByVal EstatusCliente As Integer = 9, _
                                      Optional AgrupadoPOR As String = " 1 ") As String

        'Agrupado por 1 empresa; 2 canal; 3 tiponegocio; 4 zona; 5 ruta; 6 Asesor; 7 Pais; 
        '             8 Estado; 9 Municipio; 10 parroquia; 11 ciudad; 12 Barrio

        Dim str As String = CadenaComplementoClientes("b", ClienteDesde, ClienteHasta, Operador, OrdenadoPor, _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, _
                                                      Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        If AsesorDesde <> "" Then str += " c.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " c.codven <= '" & AsesorHasta & "' and "
        If ZonaDesde <> "" Then str += " h.codigo >= '" & ZonaDesde & "' and "
        If ZonaHasta <> "" Then str += " h.codigo <= '" & ZonaHasta & "' and "
        If RutaDesde <> "" Then str += " i.codrut >= '" & RutaDesde & "' and "
        If RutaHasta <> "" Then str += " i.codrut <= '" & RutaHasta & "' and "

        SeleccionVENChequesDevueltosMES = "Select d.nombre Empresa, e.descrip canal, f.descrip tiponegocio, h.descrip zona, i.nomrut ruta,  " _
                    & " concat(c.codven,' ',c.apellidos,', ',c.nombres) asesor, r.nombre pais, q.nombre estado, " _
                    & " p.nombre municipio, o.nombre parroquia, n.nombre ciudad, m.nombre barrio,   " _
                    & " count(if(month(a.fechadev)='01',a.numcheque,null)) cont01, count(if(month(a.fechadev)='02',a.numcheque,null)) cont02,  count(if(month(a.fechadev)='03',a.numcheque,null)) cont03, count(if(month(a.fechadev)='04',a.numcheque,null)) cont04, " _
                    & " count(if(month(a.fechadev)='05',a.numcheque,null)) cont05, count(if(month(a.fechadev)='06',a.numcheque,null)) cont06,  count(if(month(a.fechadev)='07',a.numcheque,null)) cont07, count(if(month(a.fechadev)='08',a.numcheque,null)) cont08, " _
                    & " count(if(month(a.fechadev)='09',a.numcheque,null)) cont09, count(if(month(a.fechadev)='10',a.numcheque,null)) cont10,  count(if(month(a.fechadev)='11',a.numcheque,null)) cont11, count(if(month(a.fechadev)='12',a.numcheque,null)) cont12, " _
                    & " sum(if(month(a.fechadev)='01',monto,0)) tot01, sum(if(month(a.fechadev)='02',monto,0)) tot02, sum(if(month(a.fechadev)='03',monto,0)) tot03, " _
                    & " sum(if(month(a.fechadev)='04',monto,0)) tot04, sum(if(month(a.fechadev)='05',monto,0)) tot05, sum(if(month(a.fechadev)='06',monto,0)) tot06, " _
                    & " sum(if(month(a.fechadev)='07',monto,0)) tot07, sum(if(month(a.fechadev)='08',monto,0)) tot08, sum(if(month(a.fechadev)='09',monto,0)) tot09, " _
                    & " sum(if(month(a.fechadev)='10',monto,0)) tot10, sum(if(month(a.fechadev)='11',monto,0)) tot11, sum(if(month(a.fechadev)='12',monto,0)) tot12 " _
                    & " from jsbanchedev a " _
                    & " left join jsvencatcli b on (a.prov_cli = b.codcli and a.id_emp=b.id_emp) " _
                    & " left join jsvencatven c on (b.vendedor = c.codven and b.id_emp=c.id_emp) " _
                    & " left join jsconctaemp d on (b.id_emp = d.id_emp) " _
                    & " left join jsvenliscan e on (b.id_emp = e.id_emp and b.categoria = e.codigo) " _
                    & " left join jsvenlistip f on (b.id_emp = f.id_emp and b.unidad = f.codigo) " _
                    & " left join jsconctatab h on (b.zona = h.codigo and b.id_emp = h.id_emp  and h.modulo = '00005') " _
                    & " left join jsvenencrut i on (b.ruta_visita = i.codrut and b.id_emp = i.id_emp and i.tipo = '0' ) " _
                    & " left join jsconcatter m on (b.fbarrio = m.codigo and a.id_emp = m.id_emp ) " _
                    & " left join jsconcatter n on (b.fciudad = n.codigo and a.id_emp = n.id_emp ) " _
                    & " left join jsconcatter o on (b.fparroquia = o.codigo and a.id_emp = o.id_emp ) " _
                    & " left join jsconcatter p on (b.fmunicipio = p.codigo and a.id_emp = p.id_emp ) " _
                    & " left join jsconcatter q on (b.festado = q.codigo and a.id_emp = q.id_emp ) " _
                    & " left join jsconcatter r on (b.fpais = r.codigo and a.id_emp = r.id_emp ) " _
                    & " Where " _
                    & str _
                    & " year(a.fechadev) = " & Year(jytsistema.sFechadeTrabajo) & " and " _
                    & " a.id_emp='" & jytsistema.WorkID & "' " _
                    & " group by " & AgrupadoPOR

    End Function

    Function SeleccionVENCierreFinancieroContado(ByVal EmisionDesde As Date, ByVal EmisionHasta As Date, ByVal AsesorDesde As String, ByVal AsesorHasta As String) As String

        Dim str As String = ""

        If AsesorDesde <> "" Then str += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.codven <= '" & AsesorHasta & "' and "

        SeleccionVENCierreFinancieroContado = "  select a.codven vendedor, a.codcli, b.nombre, 'CA' tipomov, " _
                    & " 'FC' tipdoccan , a.numfac nummov, a.emision, a.refer, " _
                    & " if( c.formapag = 'TA', if (d.tipo = 1, 'TD', 'TC') , c.formapag) formapag , a.numfac comproba, c.importe, 1 fotipo, " _
                    & " if( c.vence > '" & ft.FormatoFechaMySQL(EmisionHasta) & "','SI','') diferido, " _
                    & " c.origen " _
                    & " from jsvenencfac a " _
                    & " left join jsvencatcli b on (a.codcli = b.codcli and a.id_emp = b.id_emp)" _
                    & " left join jsvenforpag c on (a.numfac = c.numfac and a.id_emp = c.id_emp) " _
                    & " left join jsconctatar d on (c.nompag = d.codtar and c.id_emp = d.id_emp) " _
                    & " where " _
                    & str _
                    & " a.emision >= '" & ft.FormatoFechaMySQL(EmisionDesde) & "' and " _
                    & " a.emision <= '" & ft.FormatoFechaMySQL(EmisionHasta) & "' and " _
                    & " a.condpag = 1 AND " _
                    & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " order by a.numfac "


    End Function

    Function SeleccionVENCierreFinanciero(EmisionDesde As Date, EmisionHasta As Date, AsesorDesde As String, AsesorHasta As String) As String

        Dim str As String = ""

        If AsesorDesde <> "" Then str += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.codven <= '" & AsesorHasta & "' and "

        SeleccionVENCierreFinanciero = "select b.vendedor, a.codcli, b.nombre ,a.tipomov, " _
                        & " a.tipdoccan , a.nummov, a.emision, a.refer, " _
                        & " if( a.formapag = 'TA', if (d.tipo = 1, 'TD', 'TC') , a.formapag) formapag , " _
                        & " a.comproba, a.importe importe, " _
                        & " a.fotipo, if(a.emision > '" & ft.FormatoFechaMySQL(EmisionHasta) & "','SI','') diferido " _
                        & " from jsventracob a " _
                        & " LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli and a.id_emp = b.id_emp) " _
                        & " left join jsconctatar d on (a.nompag = d.codtar and a.id_emp = d.id_emp) " _
                        & " WHERE " _
                        & str _
                        & " a.fechasi >= '" & ft.FormatoFechaMySQL(EmisionDesde) & "' and " _
                        & " a.fechasi <= '" & ft.FormatoFechaMySQL(EmisionHasta) & "' and " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' and " _
                        & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                        & " a.tipomov IN  ('CA', 'AB', 'ND', 'NC') and " _
                        & " a.comproba <> '' " _
                        & " order by a.comproba, a.tipdoccan, a.emision "

    End Function

    Function SeleccionVENRelacionFacturas(NumRel As String) As String

        Return "Select a.codigoguia, a.descripcion, a.responsable, a.fechaguia, a.emisionfac, a.hastafac, a.items, " _
            & " b.codigofac , b.Emision, b.CodCli, b.nomcli, b.codven, b.kilosfac, b.totalfac totalfac " _
            & " from jsvenencrel a " _
            & " left join jsvenrenrel b on (a.codigoguia = b.CODIGOGUIA and a.id_emp = b.id_emp) " _
            & " Where " _
            & " a.CODIGOGUIA = '" & NumRel & "' and " _
            & " a.tipo = 0 and " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' and " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' " _
            & " order by b.CODVEN, b.CODIGOFAC "

    End Function

    Function SeleccionVENRelacionNotasCredito(NumRel As String) As String

        Return "Select a.codigoguia, a.descripcion, a.responsable, a.fechaguia, a.emisionfac, a.hastafac, a.items, " _
            & " b.codigofac , b.Emision, b.CodCli, b.nomcli, b.codven, b.kilosfac, b.totalfac totalfac " _
            & " from jsvenencrel a " _
            & " left join jsvenrenrel b on (a.codigoguia = b.CODIGOGUIA and a.id_emp = b.id_emp) " _
            & " Where " _
            & " a.CODIGOGUIA = '" & NumRel & "' and " _
            & " a.tipo = 1 and " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' and " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' " _
            & " order by b.CODVEN, b.CODIGOFAC "

    End Function

    Function SeleccionVENFacturasGuia(Num_Guia As String) As String

        Return "Select a.codigoguia, a.descripcion, a.fechaguia, a.transporte, e.chofer, f.nombre elaboradopor,  a.emisionfac,  a.hastafac, a.items, " _
            & " b.codigofac, b.emision, b.codcli, b.nomcli, d.saldo saldo, b.codven, b.kilosfac, b.totalfac totalfac, elt(c.condpag+1,'CR','CO') as condpag, " _
            & " d.dirfiscal, d.dirdespa " _
            & " from jsvenencgui a " _
            & " left join jsvenrengui b on (a.codigoguia = b.CODIGOGUIA and a.id_emp = b.id_emp) " _
            & " left join jsvenencfac c on (b.codigofac = c.numfac and b.id_emp = c.id_emp) " _
            & " left join jsvencatcli d on (b.codcli = d.codcli and b.id_emp = d.id_emp ) " _
            & " left join jsconctatra e on (a.transporte = e.codtra and a.id_emp = e.id_emp ) " _
            & " left join jsconctausu f on (a.elaborador = f.id_user) " _
            & " Where " _
            & " a.CODIGOGUIA = '" & Num_Guia & "' and " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' and " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' " _
            & " order by b.CODVEN, b.CODIGOFAC "

    End Function

    Function SeleccionVENPedidossGuia(ByVal Num_Guia As String) As String

        Return "Select a.codigoguia, a.descripcion, a.fechaguia, a.transporte, e.chofer, f.nombre elaboradopor,  a.emisionfac,  a.hastafac, a.items, " _
            & " b.codigofac, b.emision, b.codcli, b.nomcli, d.saldo saldo, b.codven, b.kilosfac, b.totalfac totalfac, elt(c.condpag+1,'CR','CO') as condpag " _
            & " from jsvenencguipedidos a " _
            & " left join jsvenrenguipedidos b on (a.codigoguia = b.CODIGOGUIA and a.id_emp = b.id_emp) " _
            & " left join jsvenencped c on (b.codigofac = c.numped and b.id_emp = c.id_emp) " _
            & " left join jsvencatcli d on (b.codcli = d.codcli and b.id_emp = d.id_emp ) " _
            & " left join jsconctatra e on (a.transporte = e.codtra and a.id_emp = e.id_emp ) " _
            & " left join jsconctausu f on (a.elaborador = f.id_user) " _
            & " Where " _
            & " a.CODIGOGUIA = '" & Num_Guia & "' and " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' and " _
            & " a.EJERCICIO = '" & jytsistema.WorkExercise & "' " _
            & " order by b.CODVEN, b.CODIGOFAC "

    End Function
    Function SeleccionVENMercanciasGuiaPedidos(ByVal PedidoInicial As String, ByVal PedidoFinal As String, _
                                               ByVal AsesorInicial As String, ByVal AsesorFinal As String, _
                                               ByVal FechaInicial As Date, ByVal FechaFinal As Date) As String

        Dim str As String = ""

        If AsesorInicial <> "" Then str += " b.codven >= '" & AsesorInicial & "' and "
        If AsesorFinal <> "" Then str += " b.codven <= '" & AsesorFinal & "' and "
        If PedidoInicial <> "" Then str += " a.numped >= '" & PedidoInicial & "' and "
        If PedidoFinal <> "" Then str += " a.numped <= '" & PedidoFinal & "' and "

        Return " SELECT 'REL' codigoguia, 'RELACION DE CARGA' descripcion, '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "' fechaguia, " _
            & " '" & ft.FormatoFechaMySQL(FechaInicial) & "'  emisionfac, '" & ft.FormatoFechaMySQL(FechaFinal) _
            & "' hastafac, 1 items,  '00001' transporte, '' chofer,  '' elaboradopor,  " _
            & " f.division, g.descrip nom_division, f.tipjer, h.descrip nom_jerarquia, " _
            & " a.item, CONCAT(a.item, ' ', a.unidad) itemunidad, a.descrip, a.unidad, SUM(a.cantidad) cantidad, SUM(a.peso) peso, " _
            & " c.numped, c.CodCli, d.Nombre " _
            & " FROM jsvenrenped a " _
            & " LEFT JOIN jsvenencped b ON (a.numped = b.numped AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvenencped c ON (a.numped = c.numped AND a.id_emp = c.id_emp AND a.unidad = 'KGR') " _
            & " LEFT JOIN jsvencatcli d ON (c.CODCLI = d.codcli AND c.id_emp = d.id_emp) " _
            & " LEFT JOIN jsmerctainv f ON (a.item = f.codart AND a.id_emp = f.id_emp) " _
            & " LEFT JOIN jsmercatdiv g ON (f.division = g.division AND f.id_emp = g.id_emp) " _
            & " LEFT JOIN jsmerencjer h ON (f.tipjer = h.tipjer AND f.id_emp = h.id_emp ) " _
            & " WHERE " _
            & str _
            & " b.estatus = 0 and " _
            & " b.emision >= '" & ft.FormatoFechaMySQL(FechaInicial) & "' AND " _
            & " b.emision <= '" & ft.FormatoFechaMySQL(FechaFinal) & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " GROUP BY f.division, a.item, a.unidad, c.numped " _
            & " ORDER BY f.division, f.tipjer, a.item, c.numped "

    End Function

    Function SeleccionVENMercanciasGuia(Num_Guia As String) As String

        Return " select e.codigoguia, e.descripcion, e.fechaguia, " _
                & " e.emisionfac, e.hastafac, e.items, e.transporte, i.chofer, j.nombre elaboradopor,  " _
                & " f.division, g.descrip nom_division, f.tipjer, h.descrip nom_jerarquia, " _
                & " a.item, concat(a.item, ' ', a.unidad) itemunidad, a.descrip, a.unidad, sum(a.cantidad) cantidad, sum(a.peso) peso, " _
                & " c.numfac , c.CodCli, d.Nombre " _
                & " from jsvenrenfac a " _
                & " left join jsvenencfac b on (a.numfac = b.numfac and a.id_emp = b.id_emp) " _
                & " left join jsvenencfac c on (a.numfac = c.numfac and a.id_emp = c.id_emp and a.unidad = 'KGR') " _
                & " left join jsvencatcli d on (c.CODCLI = d.codcli and c.id_emp = d.id_emp) " _
                & " left join jsvenencguipedidos e on (b.relguia = e.codigoguia and b.id_emp = e.id_emp) " _
                & " left join jsmerctainv f on (a.item = f.codart and a.id_emp = f.id_emp) " _
                & " left join jsmercatdiv g on (f.division = g.division and f.id_emp = g.id_emp) " _
                & " left join jsmerencjer h on (f.tipjer = h.tipjer and f.id_emp = h.id_emp ) " _
                & " left join jsconctatra i on (e.transporte = i.codtra and e.id_emp = i.id_emp)" _
                & " left join jsconctausu j on (e.elaborador = j.id_user ) " _
                & " Where " _
                & " b.relguia = '" & Num_Guia & "' and " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " group by f.division, a.item, a.unidad, c.numfac " _
                & " order by f.division, f.tipjer, a.item, c.numfac "

    End Function


    Function SeleccionVENMercanciasGuiaPedidosXXX(ByVal Num_Guia As String) As String

        Return " select e.codigoguia, e.descripcion, e.fechaguia, " _
                & " e.emisionfac, e.hastafac, e.items, e.transporte, i.chofer, j.nombre elaboradopor,  " _
                & " f.division, g.descrip nom_division, f.tipjer, h.descrip nom_jerarquia, " _
                & " a.item, concat(a.item, ' ', a.unidad) itemunidad, a.descrip, a.unidad, sum(a.cantidad) cantidad, sum(a.peso) peso, " _
                & " c.numped , c.CodCli, d.Nombre " _
                & " from jsvenrenped a " _
                & " left join jsvenencped b on (a.numped = b.numped and a.id_emp = b.id_emp) " _
                & " left join jsvenencped c on (a.numped = c.numped and a.id_emp = c.id_emp and a.unidad = 'KGR') " _
                & " left join jsvencatcli d on (c.CODCLI = d.codcli and c.id_emp = d.id_emp) " _
                & " left join jsvenencgui e on (b.numpag = e.codigoguia and b.id_emp = e.id_emp) " _
                & " left join jsmerctainv f on (a.item = f.codart and a.id_emp = f.id_emp) " _
                & " left join jsmercatdiv g on (f.division = g.division and f.id_emp = g.id_emp) " _
                & " left join jsmerencjer h on (f.tipjer = h.tipjer and f.id_emp = h.id_emp ) " _
                & " left join jsconctatra i on (e.transporte = i.codtra and e.id_emp = i.id_emp)" _
                & " left join jsconctausu j on (e.elaborador = j.id_user ) " _
                & " Where " _
                & " b.numpag = '" & Num_Guia & "' and " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " group by f.division, a.item, a.unidad, c.numped " _
                & " order by f.division, f.tipjer, a.item, c.numped "

    End Function

    Function SeleccionVENPedidosSinFacturar(EmisionDesde As Date, EmisionHasta As Date, _
                                         ByVal IndiceDesde As String, IndiceHasta As String, _
                                         AsesorDesde As String, AsesorHasta As String) As String

        Dim strSQLAsesor As String = ""
        Dim strSQLCLIENTE As String = ""

        If AsesorDesde <> "" Then strSQLAsesor += " and e.codven >= '" & AsesorDesde & "' "
        If AsesorHasta <> "" Then strSQLAsesor += " and e.codven <= '" & AsesorHasta & "' "
        If IndiceDesde <> "" Then strSQLCLIENTE += " and e.codcli >= '" & IndiceDesde & "' "
        If IndiceHasta <> "" Then strSQLCLIENTE += " and e.codcli <= '" & IndiceHasta & "' "

        SeleccionVENPedidosSinFacturar = "select a.item , b.nomart, b.unidad, " _
            & " sum( if( c.uvalencia is null,a.cantran,a.cantran/c.equivale)) as cant_sin , " _
            & " b.montoultimacompra costo, d.nombre, " _
            & " sum( if( c.uvalencia is null,a.cantran,a.cantran/c.equivale))*b.montoultimacompra total_ren, " _
            & " b.tipjer,e.codcli, e.codven, f.nombre as cliente,concat(g.codven, ' ', g.apellidos,' ',g.nombres) as vendedor, " _
            & " sum( if( c.uvalencia is null,a.cantran,a.cantran/c.equivale))*b.pesounidad as pesorenglon, e.emision " _
            & " from jsvenrenped a left join jsmerequmer c on " _
            & " (a.item=c.codart and a.unidad=c.uvalencia and a.id_emp=c.id_emp), jsmerctainv b,jsconctaemp d , " _
            & " jsvenencped e,jsvencatcli f,jsvencatven g " _
            & " where  a.id_emp='" & jytsistema.WorkID & "' and a.ejercicio='" & jytsistema.WorkExercise & "' and a.cantran>0 and " _
            & " a.Item = b.Codart And a.id_emp = b.id_emp And a.id_emp = d.id_emp And a.numped = e.numped " _
            & " and a.id_emp=e.id_emp and a.ejercicio=e.ejercicio " _
            & " and a.estatus in ('0','1') " _
            & " and (e.emision between '" & ft.FormatoFechaMySQL(EmisionDesde) & "' and '" & ft.FormatoFechaMySQL(EmisionHasta) & "') " _
            & " and e.codcli=f.codcli and e.id_emp=f.id_emp " _
            & " and e.codven=g.codven and e.id_emp=g.id_emp " _
            & strSQLAsesor & strSQLCLIENTE _
            & " group by 1,2,3,5,6,8,9,10,11,12 " _
            & " order by e.codcli, e.emision "

    End Function


    Function SeleccionVENVentasClientesAct(ClienteDesde As String, ClienteHasta As String, OrdenadoPor As String, _
            Optional TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
            Optional CategoriaDesde As String = "", Optional CategoriaHasta As String = "", _
            Optional MarcaDesde As String = "", Optional MarcaHasta As String = "", _
            Optional DevisionDesde As String = "", Optional DivisionHasta As String = "", _
            Optional CanalDesde As String = "", Optional CanalHasta As String = "", _
            Optional TiponegocioDesde As String = "", Optional TipoNegocioHasta As String = "", _
            Optional ZonaDesde As String = "", Optional ZonaHasta As String = "", _
            Optional RutaDesde As String = "", Optional RutaHasta As String = "", _
            Optional AsesorDesde As String = "", Optional AsesorHasta As String = "", _
            Optional Pais As String = "", Optional Estado As String = "", Optional Municipio As String = "", _
            Optional Parroquia As String = "", Optional Ciudad As String = "", Optional Barrio As String = "", _
            Optional EmisionDesde As Date = MyDate, Optional EmisionHasta As Date = MyDate, _
            Optional AlmacenDesde As String = "", Optional AlmacenHasta As String = "", _
            Optional GrupoAdicional As String = "", _
            Optional MercanciaDesde As String = "", Optional MercanciaHasta As String = "", _
            Optional Resumido As Boolean = False, _
            Optional siMeses As Boolean = False, Optional KilosNetosDesde As Double = 0.0#, Optional Moneda As Boolean = False) As String

        Dim strSQLMercancia As String = ""
        Dim strsqlEmision As String = ""
        Dim strsqlAlmacen As String = ""
        Dim strSQLCategoria As String = ""
        Dim strSQLMarcas As String = ""
        Dim strSQLREsumen As String = ""
        Dim strSQLTipoJerarquia As String = ""
        Dim strSQLCodigoJerarquia As String = ""
        Dim strPesoMayor As String = ""

        Dim strCliente As String = ""
        Dim strGrupoAdicional As String = ""

        If ClienteDesde <> "" Then strSQLMercancia += " a." & OrdenadoPor & " >= '" & ClienteDesde & "' and "
        If ClienteHasta <> "" Then strSQLMercancia += " a." & OrdenadoPor & " <= '" & ClienteHasta & "' and "

        If CategoriaDesde <> "" Then strSQLCategoria += " b.grupo >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then strSQLCategoria += " b.grupo <= '" & CategoriaHasta & "' and "
        If MarcaDesde <> "" Then strSQLMarcas += " b.MARCA  >= '" & MarcaDesde & "' and "
        If MarcaHasta <> "" Then strSQLMarcas += " b.MARCA <= '" & MarcaHasta & "' and "
        If DevisionDesde <> "" Then strSQLMarcas += " b.division >= '" & DevisionDesde & "' and "
        If DivisionHasta <> "" Then strSQLMarcas += " b.division <= '" & DivisionHasta & "' and "
        If TipoJerarquia <> "" Then strSQLTipoJerarquia = " b.tipjer = '" & TipoJerarquia & "' and "
        If Nivel1 <> "" Then strSQLCodigoJerarquia += " b.codjer1 = '" & Nivel1 & "' and "
        If Nivel2 <> "" Then strSQLCodigoJerarquia += " b.codjer2 = '" & Nivel2 & "' and "
        If Nivel3 <> "" Then strSQLCodigoJerarquia += " b.codjer3 = '" & Nivel3 & "' and "
        If Nivel4 <> "" Then strSQLCodigoJerarquia += " b.codjer4 = '" & Nivel4 & "' and "
        If Nivel5 <> "" Then strSQLCodigoJerarquia += " b.codjer5 = '" & Nivel5 & "' and "
        If Nivel6 <> "" Then strSQLCodigoJerarquia += " b.codjer6 = '" & Nivel6 & "' and "

        strsqlEmision += " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(EmisionDesde) & "' and "
        strsqlEmision += " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(EmisionHasta) & "' and "
        If AlmacenDesde <> "" Then strsqlAlmacen += " a.almacen >= '" & AlmacenDesde & "' and "
        If AlmacenHasta <> "" Then strsqlAlmacen += " a.almacen <= '" & AlmacenHasta & "' and "
        If AsesorDesde <> "" Then strsqlAlmacen += " a.vendedor >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then strsqlAlmacen += " a.vendedor <= '" & AsesorHasta & "' and "


        strCliente = CadenaComplementoClientes("f", "", "", 0, "", CanalDesde, CanalHasta, TiponegocioDesde, TipoNegocioHasta, _
                                               ZonaDesde, ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, Parroquia, Ciudad, _
                                                Barrio)

        If Not Resumido Then strSQLREsumen = ", b.codart "
        If GrupoAdicional <> "" Then strGrupoAdicional = GrupoAdicional & ", "

        If KilosNetosDesde >= 0 Then
            If Moneda Then
                strPesoMayor = " , sum(a.ventasnetas) pesocliente "
            Else
                strPesoMayor = " , sum(a.pesoneto) pesocliente "
            End If
        End If


        Dim str As String = " select elt(b.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') AS mix, " _
            & " a.codart, b.nomart, b.unidad, c.descrip categoria, d.descrip marca, l.descrip division, e.descrip tipjer, a.prov_cli, f.nombre, " _
            & " sum(if( a.origen = 'NCV' , 0, 1)*a.peso ) as pesoventas, " _
            & " sum(if( a.origen = 'NCV' , 0, 1)*a.cantidad/m.equivale ) as cantidadventas, " _
            & " sum(if( a.origen = 'NCV' , 0, 1)*a.costotaldes ) as costoventas, " _
            & " sum(if( a.origen = 'NCV' , 0, 1)*a.ventotaldes ) as ventasventas, " _
            & " sum(if( a.origen = 'NCV' AND a.almacen <> '00002', 1, 0)*a.peso ) pesodbe, " _
            & " sum(if( a.origen = 'NCV' AND a.almacen <> '00002', 1, 0)*a.cantidad/m.equivale ) cantidaddbe, " _
            & " sum(if( a.origen = 'NCV' AND a.almacen <> '00002', 1, 0)*a.costotaldes ) costodbe, " _
            & " sum(if( a.origen = 'NCV' AND a.almacen <> '00002', 1, 0)*a.ventotaldes ) ventasdbe, " _
            & " sum(if( a.origen = 'NCV' AND a.almacen = '00002',  1, 0)*a.peso ) pesodme, " _
            & " sum(if( a.origen = 'NCV' AND a.almacen = '00002',  1, 0)*a.cantidad/m.equivale ) cantidaddme, " _
            & " sum(if( a.origen = 'NCV' AND a.almacen = '00002',  1, 0)*a.costotaldes ) costodme, " _
            & " sum(if( a.origen = 'NCV' AND a.almacen = '00002',  1, 0)*a.ventotaldes ) ventasdme, " _
            & " sum(if( a.origen = 'NCV', -1, 1)*a.peso) pesoneto, " _
            & " sum(if( a.origen = 'NCV', -1, 1)*a.cantidad/m.equivale) cantidadneta, " _
            & " sum(if( a.origen = 'NCV', -1, 1)*a.costotaldes) costoneto, " _
            & " sum(if( a.origen = 'NCV', -1, 1)*a.ventotaldes) ventasnetas, " _
            & " 'A' peso, " _
            & " count(distinct a.codart)  activacion, a.prov_cli codcli, a.vendedor, " _
            & " g.descrip as canal, if(h.descrip is null, '', h.descrip) tiponegocio, " _
            & " if( i.descrip is null, '', i.descrip) zona, if( j.nomrut is null, '', j.nomrut) ruta, " _
            & " concat(k.codven,' ', k.apellidos, ', ', k.nombres) as asesor, " _
            & " if( n.nombre is null, '', n.nombre) barrio, " _
            & " if( o.nombre is null, '', o.nombre) ciudad, " _
            & " if( p.nombre is null, '', p.nombre) parroquia, " _
            & " if( q.nombre is null, '', q.nombre) municipio, " _
            & " if( r.nombre is null, '', r.nombre) estado, " _
            & " if( s.nombre is null, '', s.nombre) pais " _
            & " from jsmertramer a " _
            & " left join jsmerctainv b on (b.codart = a.codart and b.id_emp = a.id_emp) " _
            & " left join jsconctatab c on (b.grupo = c.codigo and b.id_emp = c.id_emp and c.modulo = '00002') " _
            & " left join jsconctatab d on (b.marca = d.codigo and b.id_emp = d.id_emp and d.modulo = '00003') " _
            & " left join jsmercatdiv l on (b.division = l.division and b.id_emp = l.id_emp) " _
            & " left join jsmerencjer e on (b.tipjer = e.tipjer and b.id_emp = e.id_emp) " _
            & " left join jsvencatcli f on (a.prov_cli = f.codcli and a.id_emp = f.id_emp) " _
            & " left join jsvenliscan g on (f.categoria = g.codigo and f.id_emp = g.id_emp) " _
            & " left join jsvenlistip h on (f.unidad = h.codigo and f.categoria = h.antec and f.id_emp = h.id_emp) " _
            & " left join jsconctatab i on (f.zona = i.codigo and f.id_emp = i.id_emp and i.modulo = '00005') " _
            & " left join jsvenencrut j on (f.ruta_visita = j.codrut and f.id_emp = j.id_emp and j.tipo = '0')" _
            & " left join jsvencatven k on (a.vendedor = k.codven and a.id_emp = k.id_emp and k.tipo = '0' and k.estatus = 1) " _
            & " left join (" & SeleccionGENTablaEquivalencias() & ") m on (a.codart = m.codart and a.unidad = m.uvalencia and a.id_emp = m.id_emp) " _
            & " left join jsconcatter n on (f.fbarrio = n.codigo and a.id_emp = n.id_emp ) " _
            & " left join jsconcatter o on (f.fciudad = o.codigo and a.id_emp = o.id_emp ) " _
            & " left join jsconcatter p on (f.fparroquia = p.codigo and a.id_emp = p.id_emp ) " _
            & " left join jsconcatter q on (f.fmunicipio = q.codigo and a.id_emp = q.id_emp ) " _
            & " left join jsconcatter r on (f.festado = r.codigo and a.id_emp = r.id_emp ) " _
            & " left join jsconcatter s on (f.fpais = s.codigo and a.id_emp = s.id_emp ) " _
            & " Where   " _
            & strSQLMercancia _
            & strSQLCategoria _
            & strSQLMarcas _
            & strSQLTipoJerarquia _
            & strSQLCodigoJerarquia _
            & " a.id_emp = '" & jytsistema.WorkID & "' and a.origen in('FAC','PFC','PVE','NDV','NCV') and " & strsqlEmision & strsqlAlmacen _
            & strCliente _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' GROUP BY " & strGrupoAdicional & " a.prov_cli, a.codart " _
            & ""

        Dim strPesoCliente As String = " select a.codcli " & strPesoMayor & " from (" & str & ") a  group by codcli "

        SeleccionVENVentasClientesAct = " select a.*, b.pesocliente from (" & str & ") a " _
            & " left join (" & strPesoCliente & ") b on (a.codcli = b.codcli) where " _
            & " pesocliente >= " & KilosNetosDesde _
            & " order by  a." & OrdenadoPor


    End Function

    Function SeleccionVENVentasClientesActPlus(ClienteDesde As String, ClienteHasta As String, OrdenadoPor As String, _
            Optional TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
            Optional CategoriaDesde As String = "", Optional CategoriaHasta As String = "", _
            Optional MarcaDesde As String = "", Optional MarcaHasta As String = "", _
            Optional DivisionDesde As String = "", Optional DivisionHasta As String = "", _
            Optional CanalDesde As String = "", Optional CanalHasta As String = "", _
            Optional TiponegocioDesde As String = "", Optional TipoNegocioHasta As String = "", _
            Optional ZonaDesde As String = "", Optional ZonaHasta As String = "", _
            Optional RutaDesde As String = "", Optional RutaHasta As String = "", _
            Optional AsesorDesde As String = "", Optional AsesorHasta As String = "", _
            Optional Pais As String = "", Optional Estado As String = "", Optional Municipio As String = "", _
            Optional Parroquia As String = "", Optional Ciudad As String = "", Optional Barrio As String = "", _
            Optional EmisionDesde As Date = MyDate, Optional EmisionHasta As Date = MyDate, _
            Optional AlmacenDesde As String = "", Optional AlmacenHasta As String = "", _
            Optional GrupoAdicional As String = "", _
            Optional MercanciaDesde As String = "", Optional MercanciaHasta As String = "", _
            Optional Resumido As Boolean = False, _
            Optional siMeses As Boolean = False, Optional TipoReporte As Integer = 0) As String

        Dim strSQLREsumen As String = ""
        Dim strPesoMayor As String = ""

        Dim strCliente As String = ""
        Dim strGrupoAdicional As String = ""

        Dim sstrr As String = CadenaComplementoMercancias("b", MercanciaDesde, MercanciaHasta, 0, "", TipoJerarquia, _
                                                          Nivel1, Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, CategoriaDesde, CategoriaHasta, _
                                                          MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta)

        If ClienteDesde <> "" Then sstrr += " a." & OrdenadoPor & " >= '" & ClienteDesde & "' and "
        If ClienteHasta <> "" Then sstrr += " a." & OrdenadoPor & " <= '" & ClienteHasta & "' and "

        sstrr += " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(EmisionDesde) & "' and "
        sstrr += " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(EmisionHasta) & "' and "
        If AlmacenDesde <> "" Then sstrr += " a.almacen >= '" & AlmacenDesde & "' and "
        If AlmacenHasta <> "" Then sstrr += " a.almacen <= '" & AlmacenHasta & "' and "
        If AsesorDesde <> "" Then sstrr += " a.vendedor >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then sstrr += " a.vendedor <= '" & AsesorHasta & "' and "


        strCliente = CadenaComplementoClientes("f", "", "", 0, "", CanalDesde, CanalHasta, TiponegocioDesde, TipoNegocioHasta, _
                                               ZonaDesde, ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, Parroquia, Ciudad, _
                                                Barrio)

        If Not Resumido Then strSQLREsumen = ", b.codart "
        If GrupoAdicional <> "" Then strGrupoAdicional = GrupoAdicional & ", "

        Dim strCan As String = ""
        Dim strUnidad As String = ""
        Dim strUND As String = ""

        Select Case TipoReporte
            Case 0 'UNIDAD DE VENTA
                strCan = " a.cantidad/m.equivale "
                strUnidad = " a.unidad "
                strUND = " b.unidad "
            Case 1 'UNIDAD DE MEDIDA PRINCIPAL UMP (KGR)
                strCan = " a.peso "
                strUnidad = " a.unidad "
                strUND = " 'KGR' "
            Case 2 'MONETARIOS VENTAS
                strCan = " a.ventotaldes "
                strUnidad = " a.unidad "
                strUND = " 'BsF' "
            Case 3 'UNIDAD DE MEDIDA SECUNDARIA UMS (CAJ)
                strCan = " a.peso/b.pesounidad*m.equivale "
                strUnidad = " 'CAJ' "
                strUND = " 'CAJ' "
            Case 4 'MONETARIOS COSTOS
                strCan = " a.COSTOTALDES "
                strUnidad = " a.unidad "
                strUND = " 'BsF' "
        End Select


        Return " select elt(b.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') AS mix, " _
            & " a.codart, b.nomart, " & strUND & " unidad, c.descrip categoria, d.descrip marca, l.descrip division, e.descrip tipjer, " _
            & " a.prov_cli, IF(a.prov_cli = '00000000', 'PUNTOS DE VENTA (POS)', f.nombre) nombre, " _
            & " sum(if( a.origen = 'NCV' , 0, 1)*" & strCan & " ) ventasBrutas, " _
            & " sum(if( a.origen = 'NCV' AND a.almacen <> '00002', 1, 0)*" & strCan & " ) devolucionBuenEstado, " _
            & " sum(if( a.origen = 'NCV' AND a.almacen = '00002',  1, 0)*" & strCan & " ) devolucionMalEstado, " _
            & " sum(if( a.origen = 'NCV', -1, 1)*" & strCan & ") ventasNetas, " _
            & " 'A' peso, " _
            & " count(a.codart)  activacion, a.prov_cli codcli, a.vendedor, " _
            & " g.descrip as canal, if(h.descrip is null, '', h.descrip) tiponegocio, " _
            & " if( i.descrip is null, '', i.descrip) zona, if( j.nomrut is null, '', j.nomrut) ruta, " _
            & " concat(k.codven,' ', k.apellidos, ', ', k.nombres) as asesor, " _
            & " if( n.nombre is null, '', n.nombre) barrio, " _
            & " if( o.nombre is null, '', o.nombre) ciudad, " _
            & " if( p.nombre is null, '', p.nombre) parroquia, " _
            & " if( q.nombre is null, '', q.nombre) municipio, " _
            & " if( r.nombre is null, '', r.nombre) estado, " _
            & " if( s.nombre is null, '', s.nombre) pais " _
            & " from jsmertramer a " _
            & " left join jsmerctainv b on (b.codart = a.codart and b.id_emp = a.id_emp) " _
            & " left join jsconctatab c on (b.grupo = c.codigo and b.id_emp = c.id_emp and c.modulo = '00002') " _
            & " left join jsconctatab d on (b.marca = d.codigo and b.id_emp = d.id_emp and d.modulo = '00003') " _
            & " left join jsmercatdiv l on (b.division = l.division and b.id_emp = l.id_emp) " _
            & " left join jsmerencjer e on (b.tipjer = e.tipjer and b.id_emp = e.id_emp) " _
            & " left join jsvencatcli f on (a.prov_cli = f.codcli and a.id_emp = f.id_emp) " _
            & " left join jsvenliscan g on (f.categoria = g.codigo and f.id_emp = g.id_emp) " _
            & " left join jsvenlistip h on (f.unidad = h.codigo and f.categoria = h.antec and f.id_emp = h.id_emp) " _
            & " left join jsconctatab i on (f.zona = i.codigo and f.id_emp = i.id_emp and i.modulo = '00005') " _
            & " left join jsvenencrut j on (f.ruta_visita = j.codrut and f.id_emp = j.id_emp and j.tipo = '0')" _
            & " left join jsvencatven k on (a.vendedor = k.codven and a.id_emp = k.id_emp and k.tipo = '0' and k.estatus = 1) " _
            & " left join (" & SeleccionGENTablaEquivalencias() & ") m on (a.codart = m.codart and " & strUnidad & " = m.uvalencia and a.id_emp = m.id_emp) " _
            & " left join jsconcatter n on (f.fbarrio = n.codigo and a.id_emp = n.id_emp ) " _
            & " left join jsconcatter o on (f.fciudad = o.codigo and a.id_emp = o.id_emp ) " _
            & " left join jsconcatter p on (f.fparroquia = p.codigo and a.id_emp = p.id_emp ) " _
            & " left join jsconcatter q on (f.fmunicipio = q.codigo and a.id_emp = q.id_emp ) " _
            & " left join jsconcatter r on (f.festado = r.codigo and a.id_emp = r.id_emp ) " _
            & " left join jsconcatter s on (f.fpais = s.codigo and a.id_emp = s.id_emp ) " _
            & " Where   " _
            & " a.id_emp = '" & jytsistema.WorkID & "' and a.origen in('FAC','PFC','PVE','NDV','NCV') and " _
            & sstrr _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "'  " & strCliente & " group by " & strGrupoAdicional & " a.prov_cli, a.codart " _
            & ""



    End Function


    Function SeleccionVENRankingDeVentas(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                         ByVal EmisionDesde As Date, ByVal EmisionHasta As Date, ByVal ClienteDesde As String, ByVal ClienteHasta As String, _
                                         ByVal MercancíaDesde As String, ByVal MercanciaHasta As String, ByVal CanalDesde As String, ByVal CanalHasta As String, _
                                         ByVal TiponegocioDesde As String, ByVal TipoNegocioHasta As String, ByVal ZonaDesde As String, ByVal ZonaHasta As String, _
                                         ByVal RutaDesde As String, ByVal RutaHasta As String, ByVal AsesorDesde As String, ByVal AsesorHasta As String, _
                                         ByVal Pais As String, ByVal Estado As String, ByVal Municipio As String, ByVal Parroquia As String, ByVal Ciudad As String, ByVal Barrio As String, _
                                         ByVal CategoriaDesde As String, ByVal CategoriaHasta As String, ByVal MarcaDesde As String, ByVal MarcaHasta As String, _
                                         ByVal DivisionDesde As String, ByVal DivisionHasta As String, _
                                         ByVal TipoJerarquia As String, ByVal Nivel1 As String, ByVal Nivel2 As String, ByVal Nivel3 As String, ByVal Nivel4 As String, ByVal Nivel5 As String, ByVal Nivel6 As String, _
                                         ByVal OrdenDescendente As String, ByVal Primeros As Integer) As String


        Dim strSQL As String = ""
        Dim strSQLPrimeros As String = ""

        If Primeros > 0 Then strSQLPrimeros = " limit " & ft.FormatoEntero(Primeros)

        strSQL += CadenaComplementoClientes("b", ClienteDesde, ClienteHasta, 0, "codcli", CanalDesde, CanalHasta, TiponegocioDesde, _
            TipoNegocioHasta, ZonaDesde, ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, Parroquia, Ciudad, Barrio)

        strSQL += CadenaComplementoMercancias("d", MercancíaDesde, MercanciaHasta, 0, "codart", TipoJerarquia, Nivel1, Nivel2, Nivel3, _
                                              Nivel4, Nivel5, Nivel6, CategoriaDesde, CategoriaHasta, MarcaDesde, MarcaHasta, _
                                              DivisionDesde, DivisionHasta)

        If AsesorDesde <> "" Then strSQL += " a.vendedor >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then strSQL += " a.vendedor <= '" & AsesorHasta & "' and "

        SeleccionVENRankingDeVentas = " select a.prov_cli, b.nombre, sum(if( a.tipomov in ('SA','AS'), a.peso, -1*a.peso)) peso, " _
            & " sum(if( a.tipomov in ('SA','AS'),  if(isnull(e.uvalencia), a.cantidad, a.cantidad/e.equivale), -1* if(isnull(e.uvalencia), a.cantidad, a.cantidad/e.equivale))  ) UnidadVenta, " _
            & " sum(if( a.tipomov in ('SA','AS'),  ventotaldes, -1*ventotaldes)) Venta, " _
            & " f.descrip canal, g.descrip tiponegocio, h.descrip zona, i.nomrut ruta, a.vendedor, concat(a.vendedor,' ', j.nombres, ' ', j.apellidos) asesor, " _
            & " q.nombre barrio, r.nombre ciudad, s.nombre parroquia, t.nombre municipio, u.nombre estado, v.nombre pais , l.descrip categoria, m.descrip marca, " _
            & " n.descrip codjer, elt(d.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') mix, p.descrip division " _
            & " from jsmertramer a " _
            & " left join jsvencatcli b on (a.prov_cli = b.codcli and a.id_emp = b.id_emp) " _
            & " left join jsmerctainv d on (a.codart = d.codart and a.id_emp = d.id_emp) " _
            & " left join jsmerequmer e on (a.codart = e.codart and a.unidad = e.uvalencia and a.id_emp = e.id_emp) " _
            & " left join jsvenliscan f on (b.categoria = f.codigo and b.id_emp = f.id_emp ) " _
            & " left join jsvenlistip g on (b.unidad = g.codigo and b.categoria = g.antec and b.id_emp = g.id_emp ) " _
            & " left join jsconctatab h on (b.zona = h.codigo and b.id_emp = h.id_emp and h.modulo = '00005' ) " _
            & " left join jsvenencrut i on (b.ruta_visita = i.codrut and b.id_emp = i.id_emp and i.tipo = '0' ) " _
            & " left join jsvencatven j on (a.vendedor = j.codven and a.id_emp = j.id_emp and j.tipo = '0' and j.estatus = 1) " _
            & " left join jsconcatter q on (b.fbarrio = q.codigo and b.id_emp = q.id_emp ) " _
            & " left join jsconcatter r on (b.fciudad = r.codigo and b.id_emp = r.id_emp ) " _
            & " left join jsconcatter s on (b.fparroquia = s.codigo and b.id_emp = s.id_emp ) " _
            & " left join jsconcatter t on (b.fmunicipio = t.codigo and b.id_emp = t.id_emp ) " _
            & " left join jsconcatter u on (b.festado = u.codigo and b.id_emp = u.id_emp ) " _
            & " left join jsconcatter v on (b.fpais = v.codigo and b.id_emp = v.id_emp ) " _
            & " left join jsconctatab l on (d.grupo = l.codigo and d.id_emp = l.id_emp and l.modulo = '00002'  ) " _
            & " left join jsconctatab m on (d.marca = m.codigo and d.id_emp = m.id_emp and m.modulo = '00003'  ) " _
            & " inner join jsmerencjer n on (d.tipjer = n.tipjer and d.id_emp = n.id_emp ) " _
            & " left join jsmercatdiv p on (d.division = p.division and d.id_emp = p.id_emp ) " _
            & " Where " _
            & strSQL _
            & " a.origen in ('FAC','PFC','NDV', 'PVE', 'NCV') and " _
            & " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(EmisionDesde) & "' and " _
            & " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(EmisionHasta) & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by 1 " & " order by " & OrdenDescendente & " desc " & strSQLPrimeros

    End Function
    Function SeleccionVENSEMAFORO(ByVal AsesorDesde As String, ByVal AsesorHasta As String, _
                                  ByVal FechaDesde As Date, ByVal FechaHasta As String, _
                                  ByVal Condicion As Integer) As String

        Dim strAsesor As String = ""
        If AsesorDesde <> "" Then strAsesor += " a.codven >= '" & AsesorDesde & "' AND "
        If AsesorHasta <> "" Then strAsesor += " a.codven <= '" & AsesorHasta & "' AND "
        Dim strAsesorR As String = ""
        If AsesorDesde <> "" Then strAsesorR += " a.vendedor >= '" & AsesorDesde & "' AND "
        If AsesorHasta <> "" Then strAsesorR += " a.vendedor <= '" & AsesorHasta & "' AND "

        Dim strColor

        Select Case Condicion
            Case 0
                strColor = " color in ('ROJO') "
            Case 1
                strColor = " color in ('AMARILLO') "
            Case 2
                strColor = " color in ('VERDE') "
            Case 3
                strColor = " color in ('ROJO','AMARILLO') "
            Case 4
                strColor = " color in ('ROJO','VERDE') "
            Case 5
                strColor = " color in ('AMARILLO', 'VERDE') "
            Case Else
                strColor = " color in ('ROJO','AMARILLO', 'VERDE') "
        End Select


        Return " SELECT IF( aa.estatus = 1 , 'ROJO', IF( aa.suma_pedidos > aa.disponible , 'AMARILLO', IF( aa.comision > 0.00, 'AMARILLO', 'VERDE'))) COLOR, " _
            & " aa.cliente,  aa.numero_pedido, aa.monto_pedido, aa.suma_pedidos, aa.disponible, aa.comision, " _
            & " 	IF( aa.estatus = 1 , aa.causa, " _
            & " 		IF( aa.suma_pedidos > aa.disponible , 'DISPONIBILIDAD INSUFICIENTE', " _
            & " 			IF( aa.comision > 0.00, 'COMISION POR CHEQUE DEVUELTO SIN CANCELAR', ''))) causa,  " _
            & "                     aa.nombre_cli, aa.codi_ven, aa.nombre_ven " _
            & " FROM ( " _
            & "     SELECT a.codcli cliente, b.numped numero_pedido , ROUND(b.tot_ped,2) monto_pedido, " _
            & " 	ROUND(c.tot_ped,2) suma_pedidos, a.disponible, " _
            & " 	IF(d.saldo IS NULL, 0.00, d.saldo) comision, g.descrip causa,  " _
            & " 	nombre nombre_cli, a.vendedor codi_ven, CONCAT(f.apellidos, ' ',f.nombres) nombre_ven , a.estatus " _
            & "     FROM jsvencatcli a " _
            & "     LEFT JOIN (SELECT a.numped, a.codcli, a.codven, SUM(b.totrendes/b.cantidad*b.cantran*(1+d.monto/100)) tot_ped, a.id_emp " _
            & " 		FROM jsvenencped a " _
            & " 		LEFT JOIN jsvenrenped b ON (a.numped = b.numped AND a.id_emp = b.id_emp) " _
            & " 		LEFT JOIN (" & SeleccionGENTablaIVA(FechaHasta) & ")  d ON (b.iva = d.tipo) " _
            & "         WHERE " _
            & strAsesor _
            & " 		a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " 		a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " 		a.id_emp = '" & jytsistema.WorkID & "' " _
            & " 		GROUP BY a.numped HAVING tot_ped > 0 ) b ON (a.codcli = b.codcli AND a.vendedor = b.codven AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN (SELECT a.numped, a.codcli, a.codven, SUM(b.totrendes/b.cantidad*b.cantran*(1+d.monto/100)) tot_ped, a.id_emp " _
            & "      		FROM jsvenencped a " _
            & "     		LEFT JOIN jsvenrenped b ON (a.numped = b.numped AND a.id_emp = b.id_emp) " _
            & "      		LEFT JOIN (" & SeleccionGENTablaIVA(FechaHasta) & ")  d ON (b.iva = d.tipo) " _
            & "             WHERE " _
            & strAsesor _
            & "     		a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & "     		a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & "     		a.id_emp = '" & jytsistema.WorkID & "' " _
            & "      		GROUP BY a.codcli HAVING tot_ped > 0 ) c ON (a.codcli = c.codcli AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT a.codcli, a.nummov, b.saldo, a.id_emp " _
            & " 		   FROM jsventracob a " _
            & "        	   LEFT JOIN (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo , a.id_emp " _
            & "                 		FROM jsventracob a " _
            & "            WHERE " _
            & " 		   a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) b ON (a.codcli = b.codcli AND a.nummov = b.nummov AND a.id_emp = b.id_emp)  " _
            & "            WHERE " _
            & "            b.saldo <> 0 AND " _
            & "            a.concepto ='COMISION CHEQUE DEVUELTO y GASTOS ADM.' AND " _
            & "            a.id_emp = '" & jytsistema.WorkID & "') d ON (a.codcli = d.codcli AND a.id_emp = d.id_emp) " _
            & " LEFT JOIN (SELECT a.codcli, a.fecha, a.comentario, a.condicion, a.causa, a.tipocondicion, a.id_emp " _
            & "      		FROM jsvenexpcli a " _
            & "      		LEFT JOIN (SELECT a.codcli, MAX(a.fecha) fecha, a.id_emp  " _
            & "          				FROM jsvenexpcli a " _
            & "         				WHERE a.id_emp = '" & jytsistema.WorkID & "' " _
            & "          				GROUP BY a.codcli ) b ON (a.codcli = b.codcli AND a.fecha = b.fecha AND a.id_emp = b.id_emp) " _
            & " WHERE " _
            & " NOT b.fecha IS NULL AND " _
            & " a.condicion = 1 AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY a.codcli) e ON ( a.codcli = e.codcli AND a.id_emp = e.id_emp) " _
            & " LEFT JOIN jsvencatven f ON (a.vendedor = f.codven AND a.id_emp = f.id_emp AND tipo = 0 ) " _
            & " LEFT JOIN jsconctatab g ON (e.causa = g.codigo AND e.id_emp = g.id_emp AND g.modulo = '00018') " _
            & " " _
            & " WHERE " _
            & " NOT b.numped IS NULL AND " _
            & strAsesorR _
            & " a.id_emp = '" & jytsistema.WorkID & "' ) aa " _
            & " GROUP BY color, aa.numero_pedido " _
            & " HAVING " & strColor


    End Function

    Function SeleccionVENSEMAFOROPREPEDIDOS(ByVal AsesorDesde As String, ByVal AsesorHasta As String, _
                                  ByVal FechaDesde As Date, ByVal FechaHasta As String, _
                                  ByVal Condicion As Integer) As String

        Dim strAsesor As String = ""
        If AsesorDesde <> "" Then strAsesor += " a.codven >= '" & AsesorDesde & "' AND "
        If AsesorHasta <> "" Then strAsesor += " a.codven <= '" & AsesorHasta & "' AND "
        Dim strAsesorR As String = ""
        If AsesorDesde <> "" Then strAsesorR += " a.vendedor >= '" & AsesorDesde & "' AND "
        If AsesorHasta <> "" Then strAsesorR += " a.vendedor <= '" & AsesorHasta & "' AND "

        Dim strColor

        Select Case Condicion
            Case 0
                strColor = " color in ('ROJO') "
            Case 1
                strColor = " color in ('AMARILLO') "
            Case 2
                strColor = " color in ('VERDE') "
            Case 3
                strColor = " color in ('ROJO','AMARILLO') "
            Case 4
                strColor = " color in ('ROJO','VERDE') "
            Case 5
                strColor = " color in ('AMARILLO', 'VERDE') "
            Case Else
                strColor = " color in ('ROJO','AMARILLO', 'VERDE') "
        End Select


        Return " SELECT IF( aa.estatus = 1 , 'ROJO', IF( aa.suma_pedidos > aa.disponible , 'AMARILLO', IF( aa.comision > 0.00, 'AMARILLO', 'VERDE'))) COLOR, " _
            & " aa.cliente,  aa.numero_pedido, aa.monto_pedido, aa.suma_pedidos, aa.disponible, aa.comision, " _
            & " 	IF( aa.estatus = 1 , aa.causa, " _
            & " 		IF( aa.suma_pedidos > aa.disponible , 'DISPONIBILIDAD INSUFICIENTE', " _
            & " 			IF( aa.comision > 0.00, 'COMISION POR CHEQUE DEVUELTO SIN CANCELAR', ''))) causa,  " _
            & "                     aa.nombre_cli, aa.codi_ven, aa.nombre_ven " _
            & " FROM ( " _
            & "     SELECT a.codcli cliente, b.numped numero_pedido , ROUND(b.tot_ped,2) monto_pedido, " _
            & " 	ROUND(c.tot_ped,2) suma_pedidos, a.disponible, " _
            & " 	IF(d.saldo IS NULL, 0.00, d.saldo) comision, g.descrip causa,  " _
            & " 	nombre nombre_cli, a.vendedor codi_ven, CONCAT(f.apellidos, ' ',f.nombres) nombre_ven , a.estatus " _
            & "     FROM jsvencatcli a " _
            & "     LEFT JOIN (SELECT a.numped, a.codcli, a.codven, SUM(b.totrendes/b.cantidad*b.cantran*(1+d.monto/100)) tot_ped, a.id_emp " _
            & " 		FROM jsvenencpedrgv a " _
            & " 		LEFT JOIN jsvenrenpedrgv b ON (a.numped = b.numped AND a.id_emp = b.id_emp) " _
            & " 		LEFT JOIN (" & SeleccionGENTablaIVA(FechaHasta) & ")  d ON (b.iva = d.tipo) " _
            & "         WHERE " _
            & strAsesor _
            & " 		a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " 		a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " 		a.id_emp = '" & jytsistema.WorkID & "' " _
            & " 		GROUP BY a.numped HAVING tot_ped > 0 ) b ON (a.codcli = b.codcli AND a.vendedor = b.codven AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN (SELECT a.numped, a.codcli, a.codven, SUM(b.totrendes/b.cantidad*b.cantran*(1+d.monto/100)) tot_ped, a.id_emp " _
            & "      		FROM jsvenencpedrgv a " _
            & "     		LEFT JOIN jsvenrenpedrgv b ON (a.numped = b.numped AND a.id_emp = b.id_emp) " _
            & "      		LEFT JOIN (" & SeleccionGENTablaIVA(FechaHasta) & ")  d ON (b.iva = d.tipo) " _
            & "             WHERE " _
            & strAsesor _
            & "     		a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & "     		a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & "     		a.id_emp = '" & jytsistema.WorkID & "' " _
            & "      		GROUP BY a.codcli HAVING tot_ped > 0 ) c ON (a.codcli = c.codcli AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT a.codcli, a.nummov, b.saldo, a.id_emp " _
            & " 		   FROM jsventracob a " _
            & "        	   LEFT JOIN (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo , a.id_emp " _
            & "                 		FROM jsventracob a " _
            & "            WHERE " _
            & " 		   a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.nummov) b ON (a.codcli = b.codcli AND a.nummov = b.nummov AND a.id_emp = b.id_emp)  " _
            & "            WHERE " _
            & "            b.saldo <> 0 AND " _
            & "            a.concepto ='COMISION CHEQUE DEVUELTO y GASTOS ADM.' AND " _
            & "            a.id_emp = '" & jytsistema.WorkID & "') d ON (a.codcli = d.codcli AND a.id_emp = d.id_emp) " _
            & " LEFT JOIN (SELECT a.codcli, a.fecha, a.comentario, a.condicion, a.causa, a.tipocondicion, a.id_emp " _
            & "      		FROM jsvenexpcli a " _
            & "      		LEFT JOIN (SELECT a.codcli, MAX(a.fecha) fecha, a.id_emp  " _
            & "          				FROM jsvenexpcli a " _
            & "         				WHERE a.id_emp = '" & jytsistema.WorkID & "' " _
            & "          				GROUP BY a.codcli ) b ON (a.codcli = b.codcli AND a.fecha = b.fecha AND a.id_emp = b.id_emp) " _
            & " WHERE " _
            & " NOT b.fecha IS NULL AND " _
            & " a.condicion = 1 AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY a.codcli) e ON ( a.codcli = e.codcli AND a.id_emp = e.id_emp) " _
            & " LEFT JOIN jsvencatven f ON (a.vendedor = f.codven AND a.id_emp = f.id_emp AND tipo = 0 ) " _
            & " LEFT JOIN jsconctatab g ON (e.causa = g.codigo AND e.id_emp = g.id_emp AND g.modulo = '00018') " _
            & " " _
            & " WHERE " _
            & " NOT b.numped IS NULL AND " _
            & strAsesorR _
            & " a.id_emp = '" & jytsistema.WorkID & "' ) aa " _
            & " GROUP BY color, aa.numero_pedido " _
            & " HAVING " & strColor


    End Function



    Function SeleccionVENPedidosVsEstatusClientes(ByVal myConn As MySqlConnection, ByVal lblInfo As Label, _
                                         ByVal AsesorDesde As String, ByVal AsesorHasta As String, _
                                         ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                         ByVal Condicion As Integer) As String

        '///////////////// SEMAFORO //////////////////////////////

        Dim strSQLAsesor As String = ""
        Dim strSQLPedidos As String = ""

        If AsesorDesde <> "" Then strSQLAsesor += " and a.codven >= '" & AsesorDesde & "' "
        If AsesorHasta <> "" Then strSQLAsesor += " and a.codven <= '" & AsesorHasta & "' "

        Dim aFld() As String = {"tipo.cadena.1.0", "fecha.fecha.0.0"}
        Dim tblUltimoIVA As String = "tbl" & ft.NumeroAleatorio(100000)

        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblUltimoIVA, aFld)
        ft.Ejecutar_strSQL(myconn, "insert into " & tblUltimoIVA & " " _
            & " select tipo, max(fecha) from jsconctaiva " _
            & " where fecha<= '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "'" _
            & " group by 1 ")

        Dim aFld1() As String = {"tipo.cadena.1.0", "poriva.doble.6.2"}
        Dim tblIVA As String = "tbl" & ft.NumeroAleatorio(1000000)
        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblIVA, aFld1)
        ft.Ejecutar_strSQL(myConn, "insert into " & tblIVA & " " _
            & " select  a.tipo,b.monto from " & tblUltimoIVA & " a,jsconctaiva b " _
            & " Where a.tipo = b.tipo And a.fecha = b.fecha " _
            & " " _
            & " Union " _
            & " select '',0  from jsconctaemp where id_emp='" & jytsistema.WorkID & "' ")

        Dim aFld2() As String = {"pedido.cadena.15.0", "cliente.cadena.15.0", "total_ped.doble.19.2", "vendedor.cadena.5.0"}
        Dim tblPedidosPendientes As String = "tbl" & ft.NumeroAleatorio(1000000)
        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblPedidosPendientes, aFld2, " pedido ")
        ft.Ejecutar_strSQL(myconn, "insert into " & tblPedidosPendientes & " " _
            & " select  a.numped, a.codcli, " _
            & " sum(if(b.estatus='0',(cantran*precio-(cantran*precio*b.des_cli/100)-(cantran*precio-(cantran*precio*b.des_cli/100))*des_art/100 " _
            & " -(cantran*precio-(cantran*precio*b.des_cli/100)-(cantran*precio*b.des_cli/100)-(cantran*precio-(cantran*precio*b.des_cli/100))*des_art/100)*des_ofe/100) " _
            & " -(cantran*precio-(cantran*precio*b.des_cli/100)-(cantran*precio-(cantran*precio*b.des_cli/100))*des_art/100 " _
            & " -(cantran*precio-(cantran*precio*b.des_cli/100)-(cantran*precio*b.des_cli/100)-(cantran*precio-(cantran*precio*b.des_cli/100))*des_art/100)*des_ofe/100)*a.pordes/100 " _
            & " +((cantran*precio-(cantran*precio*b.des_cli/100)-(cantran*precio-(cantran*precio*b.des_cli/100))*des_art/100 " _
            & " -(cantran*precio-(cantran*precio*b.des_cli/100)-(cantran*precio*b.des_cli/100)-(cantran*precio-(cantran*precio*b.des_cli/100))*des_art/100)*des_ofe/100) " _
            & " -(cantran*precio-(cantran*precio*b.des_cli/100)-(cantran*precio-(cantran*precio*b.des_cli/100))*des_art/100 " _
            & " -(cantran*precio-(cantran*precio*b.des_cli/100)-(cantran*precio*b.des_cli/100)-(cantran*precio-(cantran*precio*b.des_cli/100))*des_art/100)*des_ofe/100)*a.pordes/100)*c.poriva/100, " _
            & " (cantran*precio-(cantran*precio*b.des_cli/100)-(cantran*precio-(cantran*precio*b.des_cli/100))*des_art/100 " _
            & " -(cantran*precio-(cantran*precio*b.des_cli/100)-(cantran*precio*b.des_cli/100)-(cantran*precio-(cantran*precio*b.des_cli/100))*des_art/100)*des_ofe/100) " _
            & " +((cantran*precio-(cantran*precio*b.des_cli/100)-(cantran*precio-(cantran*precio*b.des_cli/100))*des_art/100 " _
            & " -(cantran*precio-(cantran*precio*b.des_cli/100)-(cantran*precio*b.des_cli/100)-(cantran*precio-(cantran*precio*b.des_cli/100))*des_art/100)*des_ofe/100))*c.poriva/100)), a.codven " _
            & " from jsvenencped a, jsvenrenped b," & tblIVA & " c,jsvencatcli d " _
            & " where a.id_emp='" & jytsistema.WorkID & "' " & strSQLAsesor _
            & " and a.estatus='0' " _
            & " and a.emision>='" & ft.FormatoFechaMySQL(FechaDesde) & "' " _
            & " and a.emision<='" & ft.FormatoFechaMySQL(FechaHasta) & "' " _
            & " and a.id_emp=b.id_emp " _
            & " and a.numped=b.numped " _
            & " and b.cantran > 0 " _
            & " and b.estatus in ('0','1') and  b.iva=c.tipo  and a.id_emp=d.id_emp  and a.codcli=d.codcli  and d.estatus in ('0','1')  group by 1 ")

        Dim aFld3() As String = {"cliente.cadena.15.0"}
        Dim tblClientesDistintos As String = "tbl" & ft.NumeroAleatorio(1000000)
        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblClientesDistintos, aFld3)
        ft.Ejecutar_strSQL(myconn, "insert into " & tblClientesDistintos & " select distinct cliente from " & tblPedidosPendientes & " ")

        Dim aFld4() As String = {"cliente.cadena.15.0", "nummov.cadena.15.0"}
        Dim tblComisiones As String = "tbl" & ft.NumeroAleatorio(1000000)
        CrearTabla(myConn, lblInfo, jytsistema.WorkDataBase, True, tblComisiones, aFld4)
        ft.Ejecutar_strSQL(myconn, "insert into " & tblComisiones & " " _
            & " select a.cliente, b.nummov from " _
            & " " & tblClientesDistintos & " a left join jsventracob b " _
            & " on (a.cliente=b.codcli  and b.concepto='COMISION CHEQUE DEVUELTO y GASTOS ADM.') " _
            & " where b.id_emp='" & jytsistema.WorkID & "' " _
            & " group by 1, 2 ")

        Dim tblClientesComisionesCheques As String = "tbl" & ft.NumeroAleatorio(100000)
        ft.Ejecutar_strSQL(myconn, "create temporary table " & tblClientesComisionesCheques & " (cliente char(15), total_com double(19,2))")
        ft.Ejecutar_strSQL(myconn, "insert into " & tblClientesComisionesCheques & " " _
            & " select a.cliente,sum(b.importe) " _
            & " from " & tblComisiones & " a left join jsventracob b " _
            & " on (a.cliente=b.codcli and a.nummov=b.nummov) " _
            & " where b.id_emp='" & jytsistema.WorkID & "' " _
            & " group by 1 " _
            & " having round(sum(b.importe),0)<> 0 ")

        Dim tblSemaforo As String = "tbl" & ft.NumeroAleatorio(100000)
        ft.Ejecutar_strSQL(myconn, "create temporary table " & tblSemaforo & " (color char(15), cliente char(15),total_pedidos double (19,2), " _
        & " disponible double (19,2), comision double(19,2),nombre_cliente VARchar(150)) ")

        ft.Ejecutar_strSQL(myconn, "insert into " & tblSemaforo & " " _
        & " select 'VERDE   ', b.cliente,sum(b.total_ped),a.disponible,c.total_com,a.nombre " _
        & " from " & tblPedidosPendientes & " b left join jsvencatcli a " _
        & " on (a.codcli=b.cliente and a.estatus='0') " _
        & " left join " & tblClientesComisionesCheques & " c on (b.cliente=c.cliente) " _
        & " where a.id_emp='" & jytsistema.WorkID & "' " _
        & " and c.cliente is null " _
        & " group by 2 " _
        & " Having a.disponible >= Sum(b.total_ped) ")

        ft.Ejecutar_strSQL(myconn, "insert into " & tblSemaforo & " " _
        & " select 'AMARILLO', b.cliente,sum(b.total_ped),a.disponible,c.total_com,a.nombre " _
        & " from " & tblPedidosPendientes & " b left join jsvencatcli a " _
        & " on (a.codcli=b.cliente and a.estatus='0') " _
        & " left join " & tblClientesComisionesCheques & " c on (b.cliente=c.cliente) " _
        & " where a.id_emp='" & jytsistema.WorkID & "' " _
        & " group by 2 " _
        & " Having (a.disponible < Sum(b.total_ped) Or c.total_com Is Not Null) ")

        ft.Ejecutar_strSQL(myconn, " insert into " & tblSemaforo & " " _
        & " select 'ROJO    ', b.cliente,sum(b.total_ped),a.disponible,c.total_com,a.nombre " _
        & " from " & tblPedidosPendientes & " b left join jsvencatcli a " _
        & " on (a.codcli=b.cliente and a.estatus='1') " _
        & " left join " & tblClientesComisionesCheques & " c on (b.cliente=c.cliente) " _
        & " where a.id_emp='" & jytsistema.WorkID & "' " _
        & " group by 2 ")

        Dim tblUltimoBloqueo As String = "tbl" & ft.NumeroAleatorio(1000000)
        ft.Ejecutar_strSQL(myconn, "create temporary table " & tblUltimoBloqueo & " (cliente char(15), fecha date) ")

        ft.Ejecutar_strSQL(myConn, "insert into " & tblUltimoBloqueo & " " _
        & " select a.cliente, max(b.fecha) " _
        & " from " & tblSemaforo & " a,jsvenexpcli b where " _
        & " a.color='ROJO' " _
        & " and a.cliente=b.codcli " _
        & " and b.condicion = '1' " _
        & " and b.id_emp = '" & jytsistema.WorkID & "' " _
        & " group by 1 ")

        Dim strVerde As String
        Dim strRojo As String
        Dim strAmarillo As String


        Dim tblUltimaTabla As String = "tbl" & ft.NumeroAleatorio(1000000)
        ft.Ejecutar_strSQL(myconn, "create temporary table " & tblUltimaTabla & " (color char(15), cliente char(15), numero_pedido Char(15), " _
            & " monto_pedido double(19,2), suma_pedidos double(19,2), disponible double(19,2), " _
            & " comision double(19,2), causa char(35), nombre_cli char(100), codi_ven char(5), nombre_ven char(100) " _
            & " ) ")

        strVerde = " insert into " & tblUltimaTabla & " select a.color as color, a.cliente cliente, b.pedido as numero_pedido, " _
        & " b.total_ped monto_pedido,a.total_pedidos suma_pedidos,a.disponible disponible, " _
        & " 0 as comision,'                    ' as causa, a.nombre_cliente as nombre_cli, " _
        & " b.vendedor as codi_ven, concat(f.codven,' ', f.apellidos,', ',f.nombres) as nombre_ven   " _
        & " from " & tblSemaforo & " a, " & tblPedidosPendientes & " b,jsvencatven f " _
        & " Where a.cliente = b.cliente " _
        & " and a.color = 'VERDE' and f.id_emp='" & jytsistema.WorkID & "' and b.vendedor = f.codven "

        strAmarillo = " insert into " & tblUltimaTabla & " select a.color as color, a.cliente cliente, " _
        & " b.pedido as numero_pedido,b.total_ped as monto_pedido,a.total_pedidos as suma_pedidos, " _
        & " a.disponible as disponible, c.total_com as comision, '                   ' as causa, " _
        & " a.nombre_cliente as nombre_cli, b.vendedor as codi_ven,concat(f.apellidos,', ',f.nombres) as nombre_ven  " _
        & " from  " & tblSemaforo & "  a " _
        & " left join " & tblPedidosPendientes & " b on (a.cliente = b.cliente) " _
        & " left join " & tblClientesComisionesCheques & " c on (a.cliente = c.cliente) " _
        & " left join jsvencatven f on (b.vendedor = f.codven and f.id_emp = '" & jytsistema.WorkID & "' ) " _
        & " Where  " _
        & " a.color = 'AMARILLO' "

        strRojo = " insert into " & tblUltimaTabla & " select a.color as color, a.cliente cliente, b.pedido as numero_pedido, " _
        & " b.total_ped as monto_pedido,a.total_pedidos as suma_pedidos,a.disponible as disponible, " _
        & " 0 as comision, e.descrip as causa, a.nombre_cliente as nombre_cli,b.vendedor as codi_ven, " _
        & " concat(f.codven,' ',f.apellidos,', ',f.nombres) as nombre_ven " _
        & " from " & tblSemaforo & " a, " & tblPedidosPendientes & " b, jsvenexpcli c, " & tblUltimoBloqueo & " d, jsconctatab e,jsvencatven f " _
        & " Where a.cliente = b.cliente " _
        & " and a.color='ROJO' " _
        & " and a.cliente= d.cliente " _
        & " and c.id_emp='" & jytsistema.WorkID & "' " _
        & " and d.cliente=c.codcli " _
        & " and Month(d.fecha) = Month(c.fecha) " _
        & " and year(d.fecha)=year(c.fecha) " _
        & " and dayofyear(d.fecha)=dayofyear(c.fecha) " _
        & " and c.condicion='1' " _
        & " and c.id_emp=e.id_emp " _
        & " and e.modulo='00018' " _
        & " and c.causa=e.codigo " _
        & " and c.id_emp=f.id_emp " _
        & " and b.vendedor=f.codven "

        Select Case Condicion
            Case 0
                ft.Ejecutar_strSQL(myconn, strRojo)
            Case 1
                ft.Ejecutar_strSQL(myconn, strAmarillo)
            Case 2
                ft.Ejecutar_strSQL(myconn, strVerde)
            Case 3
                ft.Ejecutar_strSQL(myconn, strAmarillo)
                ft.Ejecutar_strSQL(myconn, strRojo)
            Case 4
                ft.Ejecutar_strSQL(myconn, strRojo)
                ft.Ejecutar_strSQL(myconn, strVerde)
            Case 5
                ft.Ejecutar_strSQL(myconn, strAmarillo)
                ft.Ejecutar_strSQL(myconn, strVerde)
            Case Else
                ft.Ejecutar_strSQL(myconn, strAmarillo)
                ft.Ejecutar_strSQL(myconn, strRojo)
                ft.Ejecutar_strSQL(myconn, strVerde)
        End Select


        SeleccionVENPedidosVsEstatusClientes = " select * from " & tblUltimaTabla & " order by color "


    End Function


    Function SeleccionVENCierreCestaTicket(ByVal myConn As MySqlConnection, ByVal lblInfo As Label, ByVal VendedorDesde As String, ByVal VendedorHasta As String, _
    ByVal Fecha As Date) As String

        Dim str As String = ""
        Dim tblTemp As String = ""

        If VendedorDesde <> "" Then str += " b.vendedor >= '" & VendedorDesde & "' and "
        If VendedorHasta <> "" Then str += " b.vendedor <= '" & VendedorHasta & "' and "

        tblTemp = "tbl" & ft.NumeroAleatorio(10000)

        ft.Ejecutar_strSQL(myconn, "create temporary table " & tblTemp & "(" _
            & " VENDEDOR        VARCHAR(5)                              NOT NULL, " _
            & " NUMCAN          VARCHAR(15)                             NOT NULL, " _
            & " FECHA           DATE            DEFAULT '0000-00-00'    NOT NULL, " _
            & " IMPORTE         DOUBLE(19,2)    DEFAULT '0.00'          NOT NULL " _
            & ")")


        ft.Ejecutar_strSQL(myConn, " INSERT INTO " & tblTemp & " select b.vendedor, " _
            & " a.comproba, a.fechasi, IFNULL(SUM(a.IMPORTE),0) " _
            & " from jsventracob a, jsvencatcli b " _
            & " Where " _
            & " a.codcli = b.codcli and " _
            & " a.id_emp = b.id_emp and " _
            & str _
            & " a.fechasi = '" & ft.FormatoFechaMySQL(Fecha) & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' and " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " a.tipomov in('CA','AB','ND') and " _
            & " a.formapag = 'CT' and a.comproba <> '' " _
            & " group by a.comproba " _
            & " order by a.comproba ")


        SeleccionVENCierreCestaTicket = "select a.vendedor, c.numcan, d.descrip corredor, " _
            & " c.monto monto, IFNULL(COUNT(*),0) as cantidad, sum(c.monto) montotal, " _
            & " sum(c.monto - c.comision) menoscomision " _
            & " from " & tblTemp & " a left join jsventabtic c on (a.numcan = c.numcan) " _
            & " left join jsvencestic d on (c.corredor = d.codigo and c.id_emp = d.id_emp ) " _
            & " Where " _
            & " c.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by c.corredor, c.monto " _
            & " order by c.corredor, c.monto "


    End Function

    Function SeleccionVENCobranzaPlana(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, _
                                       ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                       ByVal AsesorDesde As String, ByVal AsesorHasta As String) As String
        Dim str As String = ""
        If AsesorDesde <> "" Then str += " a.codven >= '" & AsesorDesde & "' AND "
        If AsesorHasta <> "" Then str += " a.codven <= '" & AsesorHasta & "' AND "

        Dim strX As String = ""
        If AsesorDesde <> "" Then strX += " a.vendedor >= '" & AsesorDesde & "' AND "
        If AsesorHasta <> "" Then strX += " a.vendedor <= '" & AsesorHasta & "' AND "

        Return " SELECT a.vendedor, CONCAT(b.apellidos, ' ', b.nombres) nomAsesor, a.codcli, a.nombre, c.descrip TipoNegocio, " _
            & " 'FC' TP, d.numfac, d.tot_fac montoOriginal, f.saldo ,d.emision, IF(e.tipomov IS NULL, '', e.tipomov) tipomov, " _
            & " IF(e.importe IS NULL, 0.00, e.importe) MontoCobrado, IF(g.importe IS NULL, 0.00, g.importe) NOTAS_CREDITO, " _
            & " IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision) fechacobro, " _
            & " DATEDIFF(IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision ), d.emision) DiasCobranza " _
            & " FROM jsvencatcli a " _
            & " LEFT JOIN jsvencatven b ON (a.vendedor = b.codven AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvenlistip c ON (a.unidad = c.codigo AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsvenencfac d ON (a.codcli = d.codcli AND a.id_emp = d.id_emp) " _
            & " LEFT JOIN (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp  " _
            & "    	       FROM jsventracob a " _
            & "            WHERE " _
            & " 	       a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & str _
            & "   	       a.id_emp = '" & jytsistema.WorkID & "' " _
            & "      	   GROUP BY a.codcli, a.nummov) f ON (d.codcli = f.codcli AND d.numfac = f.nummov AND d.id_emp = f.id_emp) " _
            & " LEFT JOIN jsventracob e ON (d.codcli = e.codcli AND d.numfac = e.nummov AND d.id_emp = e.id_emp AND e.tipomov IN ('AB', 'CA')) " _
            & " LEFT JOIN (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) importe, a.id_emp " _
            & " 	        FROM jsventracob a " _
            & "             WHERE  " _
            & "	            a.tipomov = 'NC' AND " _
            & "	            a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & str _
            & "	            a.id_emp = '" & jytsistema.WorkID & "' " _
            & "	            GROUP BY a.codcli, a.nummov) g ON (d.codcli = g.codcli AND d.numfac = g.nummov AND d.id_emp = g.id_emp ) " _
            & " WHERE " _
            & " d.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " d.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & strX _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " UNION " _
            & " SELECT a.vendedor, CONCAT(b.apellidos, ' ', b.nombres) nomAsesor, a.codcli, a.nombre, c.descrip TipoNegocio, " _
            & " 'NC' TP, d.numncr, d.tot_ncr montoOriginal, f.saldo ,d.emision, IF(e.tipomov IS NULL, '', e.tipomov) tipomov, " _
            & " IF(e.importe IS NULL, 0.00, e.importe) MontoCobrado, 0.00 NOTAS_CREDITO, " _
            & " IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision) fechacobro, " _
            & " DATEDIFF(IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision ), d.emision) DiasCobranza " _
            & " FROM jsvencatcli a " _
            & " LEFT JOIN jsvencatven b ON (a.vendedor = b.codven AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvenlistip c ON (a.unidad = c.codigo AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsvenencncr d ON (a.codcli = d.codcli AND a.id_emp = d.id_emp) " _
            & " LEFT JOIN (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp " _
            & "	            FROM jsventracob a " _
            & "             WHERE " _
            & "	            a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & str _
            & "	            a.id_emp = '" & jytsistema.WorkID & "' " _
            & "	            GROUP BY a.codcli, a.nummov) f ON (d.codcli = f.codcli AND d.numncr = f.nummov AND d.id_emp = f.id_emp) " _
            & " LEFT JOIN jsventracob e ON (d.codcli = e.codcli AND d.numncr = e.nummov AND d.id_emp = e.id_emp AND e.tipomov = 'ND') " _
            & " WHERE " _
            & " d.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " d.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & strX _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " UNION " _
            & " SELECT a.vendedor, CONCAT(b.apellidos, ' ', b.nombres) nomAsesor, a.codcli, a.nombre, c.descrip TipoNegocio, " _
            & " 'ND' TP, d.numndb, d.tot_ndb montoOriginal, f.saldo ,d.emision, IF(e.tipomov IS NULL, '', e.tipomov) tipomov, " _
            & " IF(e.importe IS NULL, 0.00, e.importe) MontoCobrado, 0.00 NOTAS_CREDITO, " _
            & " IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision) fechacobro, " _
            & " DATEDIFF(IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision ), d.emision) DiasCobranza " _
            & " FROM jsvencatcli a " _
            & " LEFT JOIN jsvencatven b ON (a.vendedor = b.codven AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvenlistip c ON (a.unidad = c.codigo AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsvenencndb d ON (a.codcli = d.codcli AND a.id_emp = d.id_emp) " _
            & " LEFT JOIN (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp " _
            & "	            FROM jsventracob a " _
            & "             WHERE " _
            & "	            a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & str _
            & "	            a.id_emp = '" & jytsistema.WorkID & "' " _
            & "	            GROUP BY a.codcli, a.nummov) f ON (d.codcli = f.codcli AND d.numndb = f.nummov AND d.id_emp = f.id_emp) " _
            & " LEFT JOIN jsventracob e ON (d.codcli = e.codcli AND d.numndb = e.nummov AND d.id_emp = e.id_emp AND e.tipomov = 'NC') " _
            & " WHERE " _
            & " d.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " d.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & strX _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " UNION " _
            & " SELECT a.vendedor, CONCAT(b.apellidos, ' ', b.nombres) nomAsesor, a.codcli, a.nombre, c.descrip TipoNegocio, " _
            & " d.tipomov TP, d.nummov numfac, d.importe montoOriginal, f.saldo ,d.emision, IF(e.tipomov IS NULL, '', e.tipomov) tipomov, " _
            & " IF(e.importe IS NULL, 0.00, e.importe) MontoCobrado, 0.00 NOTAS_CREDITO, " _
            & " IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision) fechacobro, " _
            & " DATEDIFF(IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision ), d.emision) DiasCobranza " _
            & " FROM jsvencatcli a " _
            & " LEFT JOIN jsvencatven b ON (a.vendedor = b.codven AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvenlistip c ON (a.unidad = c.codigo AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsventracob d ON (a.codcli = d.codcli AND a.id_emp = d.id_emp AND d.origen IN ('CXC') AND d.tipomov IN ('FC','GR') ) " _
            & " LEFT JOIN (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp  " _
            & "             FROM jsventracob a " _
            & "             WHERE " _
            & "	            a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & str _
            & "	            a.id_emp = '" & jytsistema.WorkID & "' " _
            & "	            GROUP BY a.codcli, a.nummov) f ON (d.codcli = f.codcli AND d.nummov = f.nummov AND d.id_emp = f.id_emp) " _
            & " LEFT JOIN jsventracob e ON (d.codcli = e.codcli AND d.nummov = e.nummov AND d.id_emp = e.id_emp AND e.tipomov IN ('AB', 'CA', 'NC')) " _
            & " WHERE " _
            & " d.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " d.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & strX _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " UNION " _
            & " SELECT a.vendedor, CONCAT(b.apellidos, ' ', b.nombres) nomAsesor, a.codcli, a.nombre, c.descrip TipoNegocio, " _
            & " d.tipomov TP, d.nummov numfac, d.importe montoOriginal, f.saldo ,d.emision, IF(e.tipomov IS NULL, '', e.tipomov) tipomov, " _
            & " IF(e.importe IS NULL, 0.00, e.importe) MontoCobrado, 0.00 NOTAS_CREDITO, " _
            & " IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision) fechacobro, " _
            & " DATEDIFF(IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision ), d.emision) DiasCobranza " _
            & " FROM jsvencatcli a " _
            & " LEFT JOIN jsvencatven b ON (a.vendedor = b.codven AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvenlistip c ON (a.unidad = c.codigo AND a.id_emp = c.id_emp)" _
            & " LEFT JOIN jsventracob d ON (a.codcli = d.codcli AND a.id_emp = d.id_emp AND d.origen = 'BAN' AND d.tipomov = 'ND' AND CONCEPTO = 'CHEQUE DEVUELTO') " _
            & " LEFT JOIN (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp " _
            & "	            FROM jsventracob a " _
            & "             WHERE " _
            & "	            a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & str _
            & "	            a.id_emp = '" & jytsistema.WorkID & "' " _
            & "	            GROUP BY a.codcli, a.nummov) f ON (d.codcli = f.codcli AND d.nummov = f.nummov AND d.id_emp = f.id_emp) " _
            & " LEFT JOIN jsventracob e ON (d.codcli = e.codcli AND d.nummov = e.nummov AND d.id_emp = e.id_emp AND e.tipomov IN ('NC')) " _
            & " WHERE " _
            & " d.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " d.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & strX _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " UNION " _
            & " SELECT a.vendedor, CONCAT(b.apellidos, ' ', b.nombres) nomAsesor, a.codcli, a.nombre, c.descrip TipoNegocio, " _
            & " d.tipomov TP, d.nummov numfac, d.importe montoOriginal, f.saldo ,d.emision, IF(e.tipomov IS NULL, '', e.tipomov) tipomov, " _
            & " IF(e.importe IS NULL, 0.00, e.importe) MontoCobrado, 0.00 NOTAS_CREDITO, " _
            & " IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision) fechacobro, " _
            & " DATEDIFF(IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision ), d.emision) DiasCobranza " _
            & " FROM jsvencatcli a " _
            & " LEFT JOIN jsvencatven b ON (a.vendedor = b.codven AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvenlistip c ON (a.unidad = c.codigo AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsventracob d ON (a.codcli = d.codcli AND a.id_emp = d.id_emp AND d.origen = 'BAN' AND d.tipomov = 'ND' AND SUBSTRING(CONCEPTO,1,8) = 'COMISION') " _
            & " LEFT JOIN (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp " _
            & "	            FROM jsventracob a " _
            & "             WHERE " _
            & "	            a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & str _
            & "	            a.id_emp = '" & jytsistema.WorkID & "' " _
            & "	            GROUP BY a.codcli, a.nummov) f ON (d.codcli = f.codcli AND d.nummov = f.nummov AND d.id_emp = f.id_emp) " _
            & " LEFT JOIN jsventracob e ON (d.codcli = e.codcli AND d.nummov = e.nummov AND d.id_emp = e.id_emp AND e.tipomov IN ('NC')) " _
            & " WHERE " _
            & " d.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " d.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & strX _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " UNION " _
            & " SELECT a.vendedor, CONCAT(b.apellidos, ' ', b.nombres) nomAsesor, a.codcli, a.nombre, c.descrip TipoNegocio, " _
            & " d.tipomov TP, d.nummov numfac, d.importe montoOriginal, f.saldo ,d.emision, IF(e.tipomov IS NULL, '', e.tipomov) tipomov, " _
            & " IF(e.importe IS NULL, 0.00, e.importe) MontoCobrado, 0.00 NOTAS_CREDITO, " _
            & " IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision) fechacobro, " _
            & " DATEDIFF(IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision ), d.emision) DiasCobranza " _
            & " FROM jsvencatcli a  " _
            & " LEFT JOIN jsvencatven b ON (a.vendedor = b.codven AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvenlistip c ON (a.unidad = c.codigo AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsventracob d ON (a.codcli = d.codcli AND a.id_emp = d.id_emp AND d.origen = 'CXC' AND d.tipomov = 'NC' AND SUBSTRING(d.nummov,1,3) = 'NCV' ) " _
            & " LEFT JOIN (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp " _
            & "	            FROM jsventracob a " _
            & "             WHERE  " _
            & "	            a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & str _
            & "	            a.id_emp = '" & jytsistema.WorkID & "' " _
            & "	            GROUP BY a.codcli, a.nummov) f ON (d.codcli = f.codcli AND d.nummov = f.nummov AND d.id_emp = f.id_emp) " _
            & " LEFT JOIN jsventracob e ON (d.codcli = e.codcli AND d.nummov = e.nummov AND d.id_emp = e.id_emp AND e.tipomov IN ('ND')) " _
            & " WHERE " _
            & " d.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " d.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & strX _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " UNION  " _
            & " SELECT a.vendedor, CONCAT(b.apellidos, ' ', b.nombres) nomAsesor, a.codcli, a.nombre, c.descrip TipoNegocio, " _
            & " d.tipomov TP, d.nummov numfac, d.importe montoOriginal, f.saldo ,d.emision, IF(e.tipomov IS NULL, '', e.tipomov) tipomov, " _
            & " IF(e.importe IS NULL, 0.00, e.importe) MontoCobrado, 0.00 NOTAS_CREDITO, " _
            & " IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision) fechacobro, " _
            & " DATEDIFF(IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision ), d.emision) DiasCobranza " _
            & " FROM jsvencatcli a " _
            & " LEFT JOIN jsvencatven b ON (a.vendedor = b.codven AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvenlistip c ON (a.unidad = c.codigo AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsventracob d ON (a.codcli = d.codcli AND a.id_emp = d.id_emp AND d.origen = 'CXC' AND d.tipomov = 'ND' AND SUBSTRING(d.nummov,1,3) = 'NDV' ) " _
            & " LEFT JOIN (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp  " _
            & "	            FROM jsventracob a " _
            & "             WHERE " _
            & "	            a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND" _
            & str _
            & "	            a.id_emp = '" & jytsistema.WorkID & "' " _
            & "	            GROUP BY a.codcli, a.nummov) f ON (d.codcli = f.codcli AND d.nummov = f.nummov AND d.id_emp = f.id_emp)" _
            & " LEFT JOIN jsventracob e ON (d.codcli = e.codcli AND d.nummov = e.nummov AND d.id_emp = e.id_emp AND e.tipomov IN ('NC'))" _
            & " WHERE " _
            & " d.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND" _
            & " d.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND" _
            & strX _
            & " a.id_emp = '" & jytsistema.WorkID & "'" _
            & " UNION" _
            & " SELECT a.vendedor, CONCAT(b.apellidos, ' ', b.nombres) nomAsesor, a.codcli, a.nombre, c.descrip TipoNegocio," _
            & " d.tipomov TP, d.nummov numfac, d.importe montoOriginal, f.saldo ,d.emision, IF(e.tipomov IS NULL, '', e.tipomov) tipomov, " _
            & " IF(e.importe IS NULL, 0.00, e.importe) MontoCobrado, 0.00 NOTAS_CREDITO," _
            & " IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision) fechacobro, " _
            & " DATEDIFF(IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision ), d.emision) DiasCobranza" _
            & " FROM jsvencatcli a " _
            & " LEFT JOIN jsvencatven b ON (a.vendedor = b.codven AND a.id_emp = b.id_emp)" _
            & " LEFT JOIN jsvenlistip c ON (a.unidad = c.codigo AND a.id_emp = c.id_emp)" _
            & " LEFT JOIN jsventracob d ON (a.codcli = d.codcli AND a.id_emp = d.id_emp AND d.origen = 'PVE' AND d.tipomov = 'FC' )" _
            & " LEFT JOIN (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp " _
            & "	            FROM jsventracob a" _
            & "             WHERE " _
            & "	            a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND" _
            & str _
            & "	            a.id_emp = '" & jytsistema.WorkID & "' " _
            & "	            GROUP BY a.codcli, a.nummov) f ON (d.codcli = f.codcli AND d.nummov = f.nummov AND d.id_emp = f.id_emp)" _
            & " LEFT JOIN jsventracob e ON (d.codcli = e.codcli AND d.nummov = e.nummov AND d.id_emp = e.id_emp AND e.tipomov IN ('AB','CA','NC'))" _
            & " WHERE " _
            & " d.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND" _
            & " d.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND" _
            & strX _
            & " a.id_emp = '" & jytsistema.WorkID & "'" _
            & " UNION" _
            & " SELECT a.vendedor, CONCAT(b.apellidos, ' ', b.nombres) nomAsesor, a.codcli, a.nombre, c.descrip TipoNegocio," _
            & " d.tipomov TP, d.nummov numfac, d.importe montoOriginal, f.saldo ,d.emision, IF(e.tipomov IS NULL, '', e.tipomov) tipomov, " _
            & " IF(e.importe IS NULL, 0.00, e.importe) MontoCobrado, 0.00 NOTAS_CREDITO," _
            & " IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision) fechacobro, " _
            & " DATEDIFF(IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision ), d.emision) DiasCobranza" _
            & " FROM jsvencatcli a  " _
            & " LEFT JOIN jsvencatven b ON (a.vendedor = b.codven AND a.id_emp = b.id_emp)" _
            & " LEFT JOIN jsvenlistip c ON (a.unidad = c.codigo AND a.id_emp = c.id_emp)" _
            & " LEFT JOIN jsventracob d ON (a.codcli = d.codcli AND a.id_emp = d.id_emp AND d.origen = 'PVE' AND d.tipomov = 'NC' )" _
            & " LEFT JOIN (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp " _
            & "	            FROM jsventracob a" _
            & "             WHERE " _
            & "	            a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND" _
            & str _
            & "	            a.id_emp = '" & jytsistema.WorkID & "' " _
            & "	            GROUP BY a.codcli, a.nummov) f ON (d.codcli = f.codcli AND d.nummov = f.nummov AND d.id_emp = f.id_emp)" _
            & " LEFT JOIN jsventracob e ON (d.codcli = e.codcli AND d.nummov = e.nummov AND d.id_emp = e.id_emp AND e.tipomov IN ('ND'))" _
            & " WHERE " _
            & " d.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND" _
            & " d.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND" _
            & strX _
            & " a.id_emp = '" & jytsistema.WorkID & "'" _
            & " UNION " _
            & " SELECT a.vendedor, CONCAT(b.apellidos, ' ', b.nombres) nomAsesor, a.codcli, a.nombre, c.descrip TipoNegocio," _
            & " d.tipomov TP, d.nummov numfac, d.importe montoOriginal, f.saldo ,d.emision, IF(e.tipomov IS NULL, '', e.tipomov) tipomov, " _
            & " IF(e.importe IS NULL, 0.00, e.importe) MontoCobrado, 0.00 NOTAS_CREDITO," _
            & " IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision) fechacobro, " _
            & " DATEDIFF(IF(e.emision IS NULL, DATE_FORMAT(NOW(),'%Y-%m-%d'), e.emision ), d.emision) DiasCobranza" _
            & " FROM jsvencatcli a " _
            & " LEFT JOIN jsvencatven b ON (a.vendedor = b.codven AND a.id_emp = b.id_emp)" _
            & " LEFT JOIN jsvenlistip c ON (a.unidad = c.codigo AND a.id_emp = c.id_emp)" _
            & " LEFT JOIN jsventracob d ON (a.codcli = d.codcli AND a.id_emp = d.id_emp AND d.origen = 'PFC'  )" _
            & " LEFT JOIN (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldo, a.id_emp " _
            & "	            FROM jsventracob a" _
            & "             WHERE " _
            & "	            a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND" _
            & str _
            & "	            a.id_emp = '" & jytsistema.WorkID & "' " _
            & "             GROUP BY a.codcli, a.nummov) f ON (d.codcli = f.codcli AND d.nummov = f.nummov AND d.id_emp = f.id_emp)" _
            & " LEFT JOIN jsventracob e ON (d.codcli = e.codcli AND d.nummov = e.nummov AND d.id_emp = e.id_emp AND e.tipomov IN ('AB','CA','NC'))" _
            & " WHERE " _
            & " d.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND" _
            & " d.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND" _
            & strX _
            & " a.id_emp = '" & jytsistema.WorkID & "'" _
            & " ORDER BY 1,3,7  "

    End Function

    Function SeleccionVENCuotasCobranza(ByVal myConn As MySqlConnection, ByVal lblInfo As Label, _
                                        ByVal VendedorDesde As String, ByVal VendedorHasta As String, ByVal FechaActual As Date, _
                                        ByVal TipoSeleccion As Integer) As String

        'TipoSeleccion 0 = mesesanteriores ; 1 = mesactual

        Dim strVen As String = ""
        Dim strVen1 As String = ""

        If VendedorDesde <> "" Then strVen += " a.vendedor >= '" & VendedorDesde & "' AND "
        If VendedorHasta <> "" Then strVen += " a.vendedor <= '" & VendedorHasta & "' AND "

        If VendedorDesde <> "" Then strVen1 += " a.codven >= '" & VendedorDesde & "' AND "
        If VendedorHasta <> "" Then strVen1 += " a.codven <= '" & VendedorHasta & "' AND "


        If TipoSeleccion = 0 Then

            SeleccionVENCuotasCobranza = "  SELECT codcli, nombre , IFNULL(saldo, 0.00) saldo, " _
                & " ABS(IFNULL(pagos, 0.00)) pagos, IFNULL(logro, 0.00) logro,  " _
                & " IFNULL(metadiaria, 0.00) metadiaria, IFNULL(cierre, 0.00) cierre " _
                & " FROM (SELECT CONCAT(b.codven,' ',b.nombres,' ',b.apellidos) AS vendedor, codcli, nombre, SUM(saldo) AS saldo, SUM(pagos) pagos,  IF( SUM(saldo) <> 0 ,-1*SUM(pagos)/SUM(saldo)*100, 0.00) logro, " _
                & "        IF(DiasHabilesRestantesMes('" & ft.FormatoFechaMySQL(FechaActual) & "','" & jytsistema.WorkID & "') <> 0, (SUM(saldo)+SUM(pagos))/DiasHabilesRestantesMes('" & ft.FormatoFechaMySQL(FechaActual) & "','" & jytsistema.WorkID & "'), 0.00)  AS metadiaria, SUM(saldo) + SUM(pagos) AS cierre  " _
                & "         FROM (SELECT a.codcli, a.nombre, a.vendedor, b.saldo, SUM(c.pagos) pagos " _
                & " 		FROM jsvencatcli a " _
                & " 		LEFT JOIN (SELECT a.codcli, ROUND(SUM(b.importe),2) AS saldo " _
                & " 				FROM jsvencatcli a " _
                & " 				LEFT JOIN jsventracob b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
                & "                 WHERE " _
                & " 				b.emision < PrimerDiaMes('" & ft.FormatoFechaMySQL(FechaActual) & "') AND " _
                & " 				a.id_emp = '" & jytsistema.WorkID & "' " _
                & " 				GROUP BY a.codcli " _
                & " 				HAVING  ABS(saldo) > 0.0001) b ON (a.codcli = b.codcli) " _
                & " 		LEFT JOIN (SELECT a.codcli, SUM(b.importe) pagos " _
                & " 				FROM (SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) importe FROM jsventracob a WHERE a.codcli <>  '' AND a.emision < PrimerDiaMes('" & ft.FormatoFechaMySQL(FechaActual) & "') AND a.id_emp = '" & jytsistema.WorkID & "' 	GROUP BY a.codcli, a.nummov HAVING ABS(importe) > 0.0001 ) a " _
                & " 				LEFT JOIN jsventracob  b ON (a.nummov = b.nummov AND a.codcli = b.codcli) " _
                & "                 WHERE " _
                & " 				b.tipomov IN ('NC','ND','CA','AB') AND " _
                & " 				b.emision >= PrimerDiaMes('" & ft.FormatoFechaMySQL(FechaActual) & "') AND " _
                & " 				b.emision <= '" & ft.FormatoFechaMySQL(FechaActual) & "' AND " _
                & " 				b.id_emp = '" & jytsistema.WorkID & "' " _
                & "				GROUP BY a.codcli) c ON (a.codcli = c.codcli) " _
                & " 		WHERE a.id_emp = '" & jytsistema.WorkID & "'  " _
                & " 		GROUP BY a.codcli) a " _
                & "         LEFT JOIN jsvencatven b ON (a.vendedor = b.codven) " _
                & "         WHERE " _
                & strVen _
                & "         b.id_emp = '" & jytsistema.WorkID & "' " _
                & "         GROUP BY codcli) a " _
                & " ORDER BY 1,2 "


        Else

            ft.Ejecutar_strSQL(myconn, " DROP TABLE IF EXISTS saldos")
            ft.Ejecutar_strSQL(myconn, " DROP TABLE IF EXISTS pagos ")

            ft.Ejecutar_strSQL(myConn, " CREATE TEMPORARY TABLE saldos (codcli VARCHAR(15) NOT NULL, nummov VARCHAR(30) NOT NULL, saldos DOUBLE(19,2) NOT NULL,  PRIMARY KEY (codcli,nummov)) " _
                            & " SELECT a.codcli, a.nummov, IFNULL(SUM(a.IMPORTE),0) saldos " _
                            & "                         		FROM jsventracob a " _
                            & "                                 WHERE " _
                            & "                                 a.tipomov in ('FC', 'GR', 'ND') AND " _
                            & "                                 a.emision >= primerdiames('" & ft.FormatoFechaMySQL(FechaActual) & "' ) AND " _
                            & "               	                a.emision <= '" & ft.FormatoFechaMySQL(FechaActual) & "'  AND " _
                            & "                         		a.id_emp = '" & jytsistema.WorkID & "' " _
                            & "                         		GROUP BY a.codcli, a.nummov ")

            ft.Ejecutar_strSQL(myconn, " CREATE TEMPORARY TABLE pagos (codcli VARCHAR(15) NOT NULL, nummov VARCHAR(30) NOT NULL, pagos DOUBLE(19,2) NOT NULL,  PRIMARY KEY (codcli,nummov)) " _
                            & " SELECT b.codcli, b.nummov, SUM(b.importe) pagos " _
                            & " FROM jsventracob b  " _
                            & " WHERE " _
                            & " b.tipomov IN ('NC','CA','AB') AND " _
                            & " b.emision >= primerdiames('" & ft.FormatoFechaMySQL(FechaActual) & "' ) AND " _
                            & " b.emision <= '" & ft.FormatoFechaMySQL(FechaActual) & "'  AND " _
                            & " b.id_emp = '" & jytsistema.WorkID & "' " _
                            & " GROUP BY b.codcli, b.nummov ")


            SeleccionVENCuotasCobranza = " SELECT a.codcli, a.nombre, " _
                        & " IF(b.saldos IS NULL,0.00, b.saldos) saldo, " _
                        & " IF(b.pagos IS NULL,0.00, ABS(b.pagos)) pagos, " _
                        & " IF (  IF(b.pagos IS NULL, 0.00, ABS( b.pagos))/IF(b.saldos IS NULL, 0.00,  b.saldos) IS NULL, 0.00, IF(b.pagos IS NULL, 0.00, ABS(b.pagos))/IF(b.saldos IS NULL, 0.00,  b.saldos) )  *100 logro, " _
                        & " IF( DiasHabilesRestantesMes('" & ft.FormatoFechaMySQL(FechaActual) & "','" & jytsistema.WorkID & "') = 0 , 0, " _
                        & " (IF(b.saldos IS NULL, 0.00,  b.saldos)  + IF(b.pagos IS NULL, 0.00, b.pagos))/DiasHabilesRestantesMes('" & ft.FormatoFechaMySQL(FechaActual) & "','" & jytsistema.WorkID & "')) metadiaria,  " _
                        & " 0 activacion, (IF(b.pagos IS NULL, 0.00, b.pagos) + IF(b.saldos IS NULL, 0.00,  b.saldos)) cierre " _
                        & " FROM jsvencatcli a " _
                        & " LEFT JOIN (SELECT a.codcli, SUM(saldos) saldos, SUM(pagos) pagos " _
                        & "             FROM (SELECT a.codcli, a.nummov, a.saldos, IF ( b.pagos IS NULL, 0.00, b.pagos) pagos " _
                        & "  				    FROM saldos a " _
                        & " 					LEFT JOIN pagos b ON (a.codcli = b.codcli AND a.nummov = b.nummov)) a " _
                        & "             GROUP BY a.codcli) b ON (a.codcli = b.codcli) " _
                        & " WHERE " _
                        & strVen _
                        & " a.id_emp = '" & jytsistema.WorkID & "' "

        End If




    End Function
    Function SeleccionVENBackorders(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                    ByVal PedidoDesde As String, ByVal PedidoHasta As String, _
                                    ByVal EmisionDesde As Date, ByVal EmisionHasta As Date, _
                                    ByVal ClienteDesde As String, ByVal ClienteHasta As String, _
                                    ByVal MercancíaDesde As String, ByVal MercanciaHasta As String, _
                                    ByVal CanalDesde As String, ByVal CanalHasta As String, ByVal TiponegocioDesde As String, ByVal TipoNegocioHasta As String, _
                                    ByVal ZonaDesde As String, ByVal ZonaHasta As String, ByVal RutaDesde As String, ByVal RutaHasta As String, _
                                    ByVal AsesorDesde As String, ByVal AsesorHasta As String, _
                                    ByVal Pais As String, ByVal Estado As String, ByVal Municipio As String, ByVal Parroquia As String, ByVal Ciudad As String, ByVal Barrio As String, _
                                    ByVal CategoriaDesde As String, ByVal CategoriaHasta As String, _
                                    ByVal MarcaDesde As String, ByVal MarcaHasta As String, _
                                    ByVal DivisionDesde As String, ByVal DivisionHasta As String, _
                                    ByVal TipoJerarquia As String, ByVal Nivel1 As String, ByVal Nivel2 As String, ByVal Nivel3 As String, ByVal Nivel4 As String, ByVal Nivel5 As String, ByVal Nivel6 As String) As String

        Dim strBack As String = " b.cantran "
        Dim strFecha As String = ""
        Dim strSQL As String = ""

        strFecha += " a.emision >= '" & ft.FormatoFechaMySQL(EmisionDesde) & "' and "
        strFecha += " a.emision <= '" & ft.FormatoFechaMySQL(EmisionHasta) & "' and "


        strSQL += CadenaComplementoClientes("c", ClienteDesde, ClienteHasta, 0, "codcli", CanalDesde, CanalHasta, TiponegocioDesde, _
            TipoNegocioHasta, ZonaDesde, ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, Parroquia, Ciudad, Barrio)

        If AsesorDesde <> "" Then strSQL += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then strSQL += " a.codven <= '" & AsesorHasta & "' and "

        strSQL += CadenaComplementoMercancias("d", MercancíaDesde, MercanciaHasta, 0, "codart", TipoJerarquia, Nivel1, Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, _
                                              CategoriaDesde, CategoriaHasta, MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta)

        If PedidoDesde <> "" Then strSQL += " a.numped >= '" & PedidoDesde & "' and "
        If PedidoHasta <> "" Then strSQL += " a.numped <= '" & PedidoHasta & "' and "

        SeleccionVENBackorders = " select a.numped, a.emision, a.codcli, c.nombre, c.rif, f.descrip canal, g.descrip tiponegocio, h.descrip zona, i.nomrut ruta, a.codven, concat(a.codven,' ', j.nombres, ' ', j.apellidos) asesor, " _
            & " a.tot_net tot_net, a.descuen descuen, a.cargos cargos, a.imp_iva imp_iva, a.tot_ped tot_ped, b.item, b.descrip, d.unidad, l.descrip categoria, m.descrip marca, n.descrip tipjer,  " _
            & " elt(d.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') mix, p.descrip division, sum(if( e.uvalencia is null, " & strBack & " , " & strBack & "/e.equivale)  ) cantidad, " _
            & " sum(b.peso/b.CANTIDAD*" & strBack & ") peso, sum(b.totren/b.cantidad*" & strBack & ") totalrenglon " _
            & " from jsvenencped a " _
            & " left join jsvenrenped b on (a.numped = b.numped and a.id_emp = b.id_emp and " & strBack & " > 0 and (b.estatus in('0','1'))) " _
            & " inner join jsvencatcli c on (a.codcli = c.codcli and a.id_emp = c.id_emp  ) " _
            & " inner join jsmerctainv d on (b.item = d.codart and b.id_emp = d.id_emp) " _
            & " left join jsmerequmer e on (b.item=  e.codart and b.unidad=e.uvalencia and b.id_emp=e.id_emp) " _
            & " left join jsvenliscan f on (c.CATEGORIA = f.codigo and c.id_emp = f.ID_EMP   ) " _
            & " left join jsvenlistip g on (c.unidad = g.CODIGO and c.categoria = g.antec and c.ID_EMP = g.ID_EMP  ) " _
            & " left join jsconctatab h on (c.zona = h.codigo and c.ID_EMP = h.ID_EMP and h.modulo = '00005' ) " _
            & " left join jsvenencrut i on (c.ruta_visita = i.codrut and c.ID_EMP = i.id_emp and i.tipo = '0' ) " _
            & " left join jsvencatven j on (a.codven = j.codven and a.id_emp = j.id_emp and j.tipo = '0' and j.estatus = 1) " _
            & " left join jsconcatter q on (c.fbarrio = q.codigo and c.id_emp = q.id_emp ) " _
            & " left join jsconcatter r on (c.fciudad = r.codigo and c.id_emp = r.id_emp ) " _
            & " left join jsconcatter s on (c.fparroquia = s.codigo and c.id_emp = s.id_emp ) " _
            & " left join jsconcatter t on (c.fmunicipio = t.codigo and c.id_emp = t.id_emp ) " _
            & " left join jsconcatter u on (c.festado = u.codigo and c.id_emp = u.id_emp ) " _
            & " left join jsconcatter v on (c.fpais = v.codigo and c.id_emp = v.id_emp ) " _
            & " left join jsconctatab l on (d.grupo = l.codigo and d.id_emp = l.id_emp and l.modulo = '00002'  ) " _
            & " left join jsconctatab m on (d.marca = m.codigo and d.id_emp = m.id_emp and m.modulo = '00003'  ) " _
            & " inner join jsmerencjer n on (d.tipjer = n.tipjer and d.id_emp = n.id_emp ) " _
            & " left join jsmercatdiv p on (d.division = p.division and d.id_emp = p.id_emp ) " _
            & " Where " & strFecha _
            & strSQL _
            & " not isnull(b.item) and " _
            & " a.id_emp  = '" & jytsistema.WorkID & "' " & " group by a.numped, b.item order by a.numped, b.item"


    End Function

    Function SeleccionVENDocumentosMesMes(ByVal NombreTablaEnBaseDatos As String, ByVal ClienteDesde As String, ByVal ClienteHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                     Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                     Optional ByVal TipoDesde As String = "", Optional ByVal TipoHasta As String = "", _
                                     Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                     Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                     Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                     Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                     Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                     Optional ByVal TipoCliente As Integer = 4, Optional ByVal EstatusCliente As Integer = 4) As String


        Dim str As String = CadenaComplementoClientes("c", ClienteDesde, ClienteHasta, Operador, "codcli", _
                                                      CanalDesde, CanalHasta, TipoDesde, TipoHasta, ZonaDesde, _
                                                      ZonaHasta, RutaDesde, RutaHasta, Pais, Estado, Municipio, _
                                                      Parroquia, Ciudad, Barrio, TipoCliente, EstatusCliente)

        If AsesorDesde <> "" Then str += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.codven <= '" & AsesorHasta & "' and "
        If TipoCliente < 4 Then str += " c.especial = " & TipoCliente & " and "
        If EstatusCliente < 4 Then str += " c.estatus = " & EstatusCliente & " and "

        Dim strCargo01 As String = ""
        Dim strCargo02 As String = ""
        Dim strCargo03 As String = ""
        Dim strCargo04 As String = ""
        Dim strCargo05 As String = ""
        Dim strCargo06 As String = ""
        Dim strCargo07 As String = ""
        Dim strCargo08 As String = ""
        Dim strCargo09 As String = ""
        Dim strCargo10 As String = ""
        Dim strCargo11 As String = ""
        Dim strCargo12 As String = ""

        If NombreTablaEnBaseDatos <> "jsvenencncr" Or NombreTablaEnBaseDatos = "jsvenencndb" Then
            strCargo01 = " + SUM(IF( MONTH( a.emision) = 1,  a.cargos, 0.00) )" & " - SUM(IF( MONTH( a.emision) = 1,  a.descuen, 0.00)) "
            strCargo02 = " + SUM(IF( MONTH( a.emision) = 2,  a.cargos, 0.00) )" & " - SUM(IF( MONTH( a.emision) = 2,  a.descuen, 0.00)) "
            strCargo03 = " + SUM(IF( MONTH( a.emision) = 3,  a.cargos, 0.00) )" & " - SUM(IF( MONTH( a.emision) = 3,  a.descuen, 0.00)) "
            strCargo04 = " + SUM(IF( MONTH( a.emision) = 4,  a.cargos, 0.00) )" & " - SUM(IF( MONTH( a.emision) = 4,  a.descuen, 0.00)) "
            strCargo05 = " + SUM(IF( MONTH( a.emision) = 5,  a.cargos, 0.00) )" & " - SUM(IF( MONTH( a.emision) = 5,  a.descuen, 0.00)) "
            strCargo06 = " + SUM(IF( MONTH( a.emision) = 6,  a.cargos, 0.00) )" & " - SUM(IF( MONTH( a.emision) = 6,  a.descuen, 0.00)) "
            strCargo07 = " + SUM(IF( MONTH( a.emision) = 7,  a.cargos, 0.00) )" & " - SUM(IF( MONTH( a.emision) = 7,  a.descuen, 0.00)) "
            strCargo08 = " + SUM(IF( MONTH( a.emision) = 8,  a.cargos, 0.00) )" & " - SUM(IF( MONTH( a.emision) = 8,  a.descuen, 0.00)) "
            strCargo09 = " + SUM(IF( MONTH( a.emision) = 9,  a.cargos, 0.00) )" & " - SUM(IF( MONTH( a.emision) = 9,  a.descuen, 0.00)) "
            strCargo10 = " + SUM(IF( MONTH( a.emision) = 10,  a.cargos, 0.00) )" & " - SUM(IF( MONTH( a.emision) = 10,  a.descuen, 0.00)) "
            strCargo11 = " + SUM(IF( MONTH( a.emision) = 11,  a.cargos, 0.00) )" & " - SUM(IF( MONTH( a.emision) = 11,  a.descuen, 0.00)) "
            strCargo12 = " + SUM(IF( MONTH( a.emision) = 12,  a.cargos, 0.00) )" & " - SUM(IF( MONTH( a.emision) = 12,  a.descuen, 0.00)) "
        End If

        SeleccionVENDocumentosMesMes = " SELECT a.codcli, concat(c.nombre,a.comen) nombre, c.rif, " _
                & " f.descrip canal, g.descrip tiponegocio, h.descrip zona, i.nomrut ruta, a.codven, concat(a.codven,' ', j.nombres, ' ', j.apellidos) asesor, " _
                & " k.nombre pais, l.nombre estado, m.nombre municipio, n.nombre parroquia, o.nombre ciudad, p.nombre barrio, " _
                & " SUM(IF( MONTH( a.emision) = 1,  a.tot_net, 0 ))" & strCargo01 & " neto01, " _
                & " SUM(IF( MONTH( a.emision) = 2,  a.tot_net, 0 ))" & strCargo02 & " neto02, " _
                & " SUM(IF( MONTH( a.emision) = 3,  a.tot_net, 0 ))" & strCargo03 & " neto03, " _
                & " SUM(IF( MONTH( a.emision) = 4,  a.tot_net, 0 ))" & strCargo04 & " neto04, " _
                & " SUM(IF( MONTH( a.emision) = 5,  a.tot_net, 0 ))" & strCargo05 & " neto05, " _
                & " SUM(IF( MONTH( a.emision) = 6,  a.tot_net, 0 ))" & strCargo06 & " neto06, " _
                & " SUM(IF( MONTH( a.emision) = 7,  a.tot_net, 0 ))" & strCargo07 & " neto07, " _
                & " SUM(IF( MONTH( a.emision) = 8,  a.tot_net, 0 ))" & strCargo08 & " neto08, " _
                & " SUM(IF( MONTH( a.emision) = 9,  a.tot_net, 0 ))" & strCargo09 & " neto09, " _
                & " SUM(IF( MONTH( a.emision) = 10,  a.tot_net, 0 ))" & strCargo10 & " neto10, " _
                & " SUM(IF( MONTH( a.emision) = 11,  a.tot_net, 0 ))" & strCargo11 & " neto11, " _
                & " SUM(IF( MONTH( a.emision) = 12,  a.tot_net, 0 ))" & strCargo12 & " neto12, " _
                & " SUM(IF( MONTH( a.emision) = 1, a.kilos, 0.00)) kilos1,  " _
                & " SUM(IF( MONTH( a.emision) = 2, a.kilos, 0.00)) kilos2,  " _
                & " SUM(IF( MONTH( a.emision) = 3, a.kilos, 0.00)) kilos3,  " _
                & " SUM(IF( MONTH( a.emision) = 4, a.kilos, 0.00)) kilos4,  " _
                & " SUM(IF( MONTH( a.emision) = 5, a.kilos, 0.00)) kilos5,  " _
                & " SUM(IF( MONTH( a.emision) = 1, a.kilos, 0.00)) kilos6,  " _
                & " SUM(IF( MONTH( a.emision) = 1, a.kilos, 0.00)) kilos7,  " _
                & " SUM(IF( MONTH( a.emision) = 1, a.kilos, 0.00)) kilos8,  " _
                & " SUM(IF( MONTH( a.emision) = 1, a.kilos, 0.00)) kilos9,  " _
                & " SUM(IF( MONTH( a.emision) = 1, a.kilos, 0.00)) kilos10, " _
                & " SUM(IF( MONTH( a.emision) = 1, a.kilos, 0.00)) kilos11, " _
                & " SUM(IF( MONTH( a.emision) = 1, a.kilos, 0.00)) kilos12  " _
                & " FROM " & NombreTablaEnBaseDatos & " a " _
                & " left join jsvencatcli c on (a.codcli = c.codcli and a.id_emp = c.id_emp) " _
                & " left join jsvenliscan f on (c.categoria = f.codigo and c.id_emp = f.ID_EMP) " _
                & " left join jsvenlistip g on (c.unidad = g.codigo and c.categoria = g.antec and c.id_emp = g.id_emp) " _
                & " left join jsconctatab h on (c.zona = h.codigo and c.ID_EMP = h.ID_EMP and h.modulo = '00005') " _
                & " left join jsvenencrut i on (c.ruta_visita = i.codrut and c.ID_EMP = i.id_emp and i.tipo = '0') " _
                & " left join jsvencatven j on (a.codven = j.codven and a.id_emp = j.id_emp and j.tipo = '0' ) " _
                & " left join jsconcatter k on (c.fpais = k.codigo and c.id_emp = k.id_emp) " _
                & " left join jsconcatter l on (c.festado = l.codigo and c.id_emp = l.id_emp) " _
                & " left join jsconcatter m on (c.fmunicipio = m.codigo and c.id_emp = m.id_emp) " _
                & " left join jsconcatter n on (c.fparroquia = n.codigo and c.id_emp = n.id_emp) " _
                & " left join jsconcatter o on (c.fciudad = o.codigo and c.id_emp = o.id_emp) " _
                & " left join jsconcatter p on (c.fbarrio = p.codigo and c.id_emp = p.id_emp) " _
                & " WHERE " _
                & str _
                & " YEAR(a.emision) = " & Year(jytsistema.sFechadeTrabajo) & " AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " GROUP BY a.codcli " _
                & " having (neto01 + neto02 + neto03 + neto04 + neto05 + neto06 + neto07 + neto08 + neto09 + neto10 + neto11 + neto12 ) <> 0.00 "


    End Function
    Function SeleccionVENRetencionesIVAClientes(ByVal FechaInicial As Date, ByVal Fechafinal As Date) As String

        Return " SELECT a.codcli , b.nombre,  a.tipomov, a.nummov, date_format(a.emision,'%Y-%m-%d') emision, a.refer, a.importe, a.codven, CONCAT( c.apellidos, ' ',  c.nombres) vendedor " _
            & " FROM jsventracob a " _
            & " LEFT JOIN jsvencatcli b ON (a.codcli = b.codcli AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvencatven c ON (a.codven = c.codven AND a.id_emp = c.id_emp) " _
            & " WHERE " _
            & " a.emision >= '" & ft.FormatoFechaMySQL(FechaInicial) & "' AND " _
            & " a.emision <= '" & ft.FormatoFechaMySQL(Fechafinal) & "' AND " _
            & " a.CONCEPTO = 'RETENCION IVA CLIENTE ESPECIAL' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' "

    End Function

    Function SeleccionVENComisionesJerarquia(AsesorDesde As String, AsesorHasta As String, _
             EmisionDesde As Date, EmisionHasta As Date, Optional Orden As Integer = 0) As String

        Dim strAsesor As String = ""
        Dim strOrden As String = ""

        Select Case Orden
            Case 0
                strOrden = " a.vendedor, c.tipjer, a.codart, a.prov_cli "
            Case 1
                strOrden = " a.vendedor, a.numdoc, c.tipjer, a.codart "
        End Select

        If AsesorDesde <> "" Then strAsesor += " and a.vendedor >= '" & AsesorDesde & "' "
        If AsesorHasta <> "" Then strAsesor += " and a.vendedor <= '" & AsesorHasta & "' "

        Return " select " _
            & " a.vendedor, concat( d.nombres, ' ' , d.apellidos) nomven , c.tipjer, e.descrip desjer, if( isnull(f.por_ventas), 0.00, f.por_ventas) porventas, " _
            & " a.codart, c.nomart, c.unidad, a.prov_cli, a.numdoc, b.nombre nomcli, " _
            & " SUM(if( a.origen in ('FAC','NDV','PVE','PFC'), if( isnull(m.uvalencia), a.cantidad, a.cantidad/m.equivale),-1 * if( isnull(m.uvalencia), a.cantidad, a.cantidad/m.equivale))) AS CantidadTotal, " _
            & " SUM(if( a.origen in ('FAC','NDV','PVE','PFC'), if( a.tipomov <> 'AP', a.peso, 0) ,  -1 * if( a.tipomov <> 'AP', a.peso, 0) )) AS PesoTotal, " _
            & " SUM(if( a.origen in ('FAC','NDV','PVE','PFC'), if( a.tipomov <> 'AP', a.costotaldes/1, 0), -1 *if( a.tipomov <> 'AP', a.costotaldes/1, 0))) as CosTotal, " _
            & " SUM(if( a.origen in ('FAC','NDV','PVE','PFC'), a.ventotaldes/1, -1 * a.ventotaldes/1 )) as VenTotal , " _
            & " if( isnull(f.por_ventas), 0.00, SUM(if( a.origen in ('FAC','NDV','PVE','PFC'), a.ventotaldes/1, -1 * a.ventotaldes/1 ))*f.por_ventas/100) as Comisiones " _
            & " from jsmertramer a " _
            & " inner join jsvencatcli b on (a.prov_cli = b.codcli and a.id_emp = b.id_emp) " _
            & " inner join jsmerctainv c on (a.codart = c.codart and a.id_emp = c.id_emp) " _
            & " inner join jsvencatven d on (a.vendedor = d.codven and a.id_emp = d.id_emp) " _
            & " left join jsmerencjer e on (c.tipjer = e.tipjer and c.id_emp = e.id_emp) " _
            & " left join jsvencomven f on (c.tipjer = f.tipjer and a.vendedor = f.codven and c.id_emp = f.id_emp) " _
            & " left join jsmerequmer m on (a.codart = m.codart and a.unidad = m.uvalencia and a.id_emp = m.id_emp) " _
            & " Where " _
            & " f.tipo = '1' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' and " _
            & " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(EmisionDesde) & "' and " _
            & " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(EmisionHasta) & "' " & strAsesor _
            & " group by " & strOrden

    End Function



    Function SeleccionVENComisionesCobranzaVencimientos(AsesorDesde As String, AsesorHasta As String, _
                                                           FechaDesde As Date, FechaHasta As Date) As String

        Dim str As String = ""
        If AsesorDesde <> "" Then str += " a.codven >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.codven <= '" & AsesorHasta & "' and "

        Return " SELECT * FROM (SELECT a.codcli, c.nombre, a.codven, CONCAT(d.apellidos, ' ', d.nombres) nomven, a.tipomov, a.nummov, a.emision, a.importe, b.numfac, b.kilos, " _
            & " b.emision emisionfac, b.tot_fac, (b.tot_net + b.cargos - b.descuen) montocan, DATEDIFF(a.emision, b.emision) dv,  " _
            & " IF( e.de <= DATEDIFF(a.emision, b.emision) AND DATEDIFF(a.emision, b.emision) <= e.a, e.por_cobranza , 0.00 ) porCobranza, " _
            & " (b.tot_net + b.cargos - b.descuen)*IF( e.de <= DATEDIFF(a.emision, b.emision) AND DATEDIFF(a.emision, b.emision) <= e.a, e.por_cobranza , 0.00 ) / 100 comisionkilos, " _
            & " e.de,  e.a " _
            & " FROM jsventracob a " _
            & " LEFT JOIN jsvenencfac b ON (a.nummov = b.numfac AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvencatcli c ON (a.codcli = c.codcli AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsvencatven d ON (a.codven = d.codven AND a.id_emp = d.id_emp) " _
            & " LEFT JOIN jsvencomvencob e ON (a.codven = e.codven AND a.id_emp = e.id_emp) " _
            & " WHERE " _
            & str _
            & " NOT b.numfac IS NULL AND " _
            & " a.tipomov IN ('CA') AND  " _
            & " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " UNION " _
            & " SELECT a.codcli, c.nombre, a.codven, CONCAT(d.apellidos, ' ', d.nombres) nomven, a.tipomov, a.nummov, a.emision, a.importe, " _
            & "         b.numndb, b.kilos, b.emision emisionfac, b.tot_ndb, -1*(b.tot_net) montocan, " _
            & "         1 dv, IF( e.de <= 1 AND 1 <= e.a, e.por_cobranza , 0.00)  porCobranza, " _
            & "         -1*(b.tot_net)*IF( e.de <= 1 AND 1 <= e.a, e.por_cobranza , 0.00)/ 100 comisionkilos, e.de,  e.a " _
            & " FROM jsventracob a " _
            & " LEFT JOIN jsvenencndb b ON (a.nummov = b.numndb AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvencatcli c ON (a.codcli = c.codcli AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsvencatven d ON (a.codven = d.codven AND a.id_emp = d.id_emp) " _
            & " LEFT JOIN jsvencomvencob e ON (a.codven = e.codven AND a.id_emp = e.id_emp) " _
            & " WHERE " _
            & str _
            & " NOT b.numndb IS NULL AND " _
            & " a.tipomov IN ('ND') AND  " _
            & " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND  " _
            & " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " UNION " _
            & " SELECT a.codcli, c.nombre, a.codven, CONCAT(d.apellidos, ' ', d.nombres) nomven, a.tipomov, a.nummov, a.emision, a.importe, " _
            & "         a.nummov numndb, 0.00 kilos, a.emision emisionfac, a.importe tot_ndb, -1*(a.importe) montocan, " _
            & "         1 dv, IF( e.de <= 1 AND 1 <= e.a, e.por_cobranza , 0.00)  porCobranza, " _
            & "         -1*(a.importe)*IF( e.de <= 1 AND 1 <= e.a, e.por_cobranza , 0.00)/ 100 comisionkilos, e.de,  e.a " _
            & " FROM jsventracob a " _
            & " LEFT JOIN jsvencatcli c ON (a.codcli = c.codcli AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsvencatven d ON (a.codven = d.codven AND a.id_emp = d.id_emp) " _
            & " LEFT JOIN jsvencomvencob e ON (a.codven = e.codven AND a.id_emp = e.id_emp) " _
            & " WHERE " _
            & str _
            & " a.tipomov IN ('ND') AND  " _
            & " SUBSTRING(a.nummov,1,2) = 'CD' AND a.CONCEPTO = 'CHEQUE DEVUELTO' AND  " _
            & " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND  " _
            & " a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' ) a " _
            & " WHERE " _
            & " a.comisionkilos <> 0.0 " _
            & " ORDER BY dv, emision "

    End Function

    Function SeleccionComisionesCobranzaJerarquia(FechaDesde As Date, FechaHasta As Date, _
    VendedorDesde As String, VendedorHasta As String) As String

        Dim str As String
        str = " a.emision >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and a.emision <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and "
        If VendedorDesde <> "" Then str = str & " a.codven >= '" & VendedorDesde & "' and "
        If VendedorHasta <> "" Then str = str & " a.codven <= '" & VendedorHasta & "' and "

        SeleccionComisionesCobranzaJerarquia = " select a.codven, concat(f.apellidos, ', ', f.nombres) vendedor, a.codcli,  " _
            & " e.nombre,  a.tipomov TP, a.nummov, a.emision emisionpago, " _
            & " round(sum(abs(a.importe) - abs(c.impiva)*abs(a.importe)/abs(c.importe) - if( isnull(g.totrendes), 0.00, g.totrendes)- if( isnull(h.totrendes), 0.00, h.totrendes) ),  2) importe, " _
            & " a.formapag FP, a.refer, c.emision, " _
            & " (to_days(a.emision) - to_days(c.emision)) dias, d.por_cobranza, " _
            & " round(( sum(abs(a.importe) - abs(c.impiva)*abs(a.importe)/abs(c.importe) - if( isnull(g.totrendes), 0.00, g.totrendes)- if( isnull(h.totrendes), 0.00, h.totrendes) )*d.por_cobranza/100),2) comision " _
            & " from jsventracob a " _
            & " left JOIN jsbanchedev b on ( a.numpag = b.numcheque and a.codcli = b.prov_cli) " _
            & " left join jsventracob c on (a.codcli = c.codcli and a.nummov = c.nummov and c.tipomov in('FC','ND') ) " _
            & " left join jsvencomvencob d  on (a.codven = d.codven and a.id_emp = d.id_emp and (to_days(a.emision) - to_days(c.emision)) >= d.de " _
            & " and (to_days(a.emision) - to_days(c.emision)) <= d.a and tipjer = '01' ) " _
            & " left join jsvencatcli e on (a.codcli = e.codcli and a.id_emp = e.id_emp ) " _
            & " left join jsvencatven f on (a.codven = f.codven and a.id_emp = f.id_emp )" _
            & " left join jsvenrenfac g on (a.nummov = g.numfac and a.id_emp = g.id_emp and g.item = '$00000001') " _
            & " left join jsvenrenfac h on (a.nummov = h.numfac and a.id_emp = h.id_emp and h.item = '$00000002') " _
            & " Where " _
            & " isnull(b.numcheque) and " _
            & " a.origen = 'CXC' and " _
            & str _
            & " a.tipomov in ('AB','CA') and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by a.codcli, a.nummov " _
            & " order by dias, a.codcli "

    End Function

    Function SeleccionVENRuta(CodigoRuta As String, TipoRuta As String) As String

        Return "select a.codrut, a.nomrut, a.comen, a.codzon, c.descrip zona, a.codven, concat(a.codven, ' ', d.nombres, ' ', d.apellidos) asesor, " _
            & " ELT(dia+1, 'LUNES','MARTES','MIERCOLES','JUEVES','VIERNES','SABADO','DOMINGO') dia, elt(a.condicion+1,'NORMAL','EXTRAORDINARIA') condicion, a.codtra, e.nomtra, e.chofer, a.items, " _
            & " b.numero, b.cliente, b.nomcli, f.dirfiscal direccion, telef1 telef " _
            & " FROM jsvenencrut a " _
            & " left join jsvenrenrut b on (a.CODRUT = b.CODRUT and a.id_emp = b.ID_EMP and a.tipo = b.tipo AND b.tipo = '" & TipoRuta & "') " _
            & " left join jsconctatab c on (a.codzon = c.codigo AND a.id_emp = c.id_emp AND c.modulo = '00005') " _
            & " left join jsvencatven d on (a.codven = d.codven and a.id_emp = d.ID_EMP ) " _
            & " left join jsconctatra e on (a.codtra = e.codtra and a.id_emp = e.ID_EMP ) " _
            & " left join jsvencatcli f on (b.cliente = f.codcli and a.id_emp = f.id_emp) " _
            & " Where " _
            & " b.cliente is not null and " _
            & " a.codrut = '" & CodigoRuta & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by a.codrut, b.numero"

    End Function


    Function SeleccionVENVentasHEINZ(EmisionDesde As Date, EmisionHasta As Date, AlmacenDesde As String, AlmacenHasta As String, _
                                     CodigoJerarquiaHEINZ As String) As String

        Dim strSQL As String = ""
        If AlmacenDesde <> "" Then strSQL += " a.almacen >= '" & AlmacenDesde & "' AND "
        If AlmacenHasta <> "" Then strSQL += " a.almacen <= '" & AlmacenHasta & "' AND "

        Return " SELECT CONCAT(if ( f.codrut is null, '', f.codrut), ';',if(f.codzon is null, '', f.codzon),';', a.vendedor) vendedor,  IF( c.nombre IS NULL, '00000000; PUNTOS DE VENTA',  CONCAT( a.prov_cli , ';', c.nombre)) cliente , " _
            & " e.descrip canal, d.descrip categoria, " _
            & " CONCAT(b.codart,';',b.alterno,';',b.nomart) articulo, cantidad cajas, peso kilos, costotaldes costosBsF " _
            & " FROM jsmertramer a " _
            & " LEFT JOIN jsmerctainv b ON (a.codart = b.codart AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvencatcli c ON (a.prov_cli = c.codcli AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsconctatab d ON (b.grupo = d.codigo AND b.id_emp = d.id_emp AND d.modulo = '00002') " _
            & " LEFT JOIN jsvenlistip e ON (c.unidad = e.codigo AND c.id_emp = e.id_emp) " _
            & " LEFT JOIN (SELECT a.codrut, a.nomrut, a.codven,  a.codzon, b.cliente , a.id_emp " _
            & "     	    FROM jsvenencrut a " _
            & "     		LEFT JOIN jsvenrenrut b ON (a.codrut = b.codrut AND a.id_emp = b.id_emp) " _
            & "             WHERE " _
            & "     		a.tipo = 0 AND " _
            & "     		a.id_emp = '" & jytsistema.WorkID & "') f ON (a.prov_cli = f.cliente AND a.vendedor = f.codven AND a.id_emp = f.id_emp) " _
            & " WHERE " _
            & strSQL _
            & " b.tipjer = '" & CodigoJerarquiaHEINZ & "' AND " _
            & " a.origen IN ('FAC','PVE') AND " _
            & " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(EmisionDesde) & "' AND " _
            & " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(EmisionHasta) & "' AND " _
            & " a.id_emp ='" & jytsistema.WorkID & "' "

    End Function

    Function SeleccionVENVentasHEINZ_II(EmisionDesde As Date, EmisionHasta As Date, AlmacenDesde As String, AlmacenHasta As String, _
                                     CodigoJerarquiaHEINZ As String) As String

        Dim strSQL As String = ""
        If AlmacenDesde <> "" Then strSQL += " a.almacen >= '" & AlmacenDesde & "' AND "
        If AlmacenHasta <> "" Then strSQL += " a.almacen <= '" & AlmacenHasta & "' AND "

        Return " SELECT a.vendedor, CONCAT(b.apellidos, ', ' , b.nombres) , a.prov_cli, IF(c.nombre IS NULL, 'PUNTO DE VENTA', c.nombre) NOMBRE, " _
        & " a.codart, d.nomart , d.unidad, " _
        & " IF( a.origen = 'NCV', 0.00,  ROUND(a.cantidad/e.equivale, 3))  cantidad, " _
        & " IF( a.origen = 'NCV', 0.00, a.peso) PESO, " _
        & " IF( a.origen = 'NCV', 0.00, a.costotaldes)  costos, " _
        & " IF( a.origen = 'NCV', 0.00, a.ventotaldes) ventas, " _
        & " IF( a.origen <> 'NCV', 0.00,  ROUND(a.cantidad/e.equivale, 3))  cantidadDev, " _
        & " IF( a.origen <> 'NCV', 0.00, a.peso) PESODev , " _
        & " IF( a.origen <> 'NCV', 0.00, a.costotaldes)  costosDev, " _
        & " IF( a.origen <> 'NCV', 0.00, a.ventotaldes) ventasDev, 'HEINZ' jerarquia, " _
        & " IF( a.origen = 'NCV', -1,1) * ROUND(a.cantidad/e.equivale, 3)  cantidadReal, " _
        & " IF( a.origen = 'NCV', -1,1) * a.peso PESOReal ,  " _
        & " IF( a.origen = 'NCV', -1,1) * a.costotaldes  costosReal, " _
        & " IF( a.origen = 'NCV', -1,1) * a.ventotaldes ventasReal " _
        & " FROM jsmertramer a " _
        & " LEFT JOIN jsvencatven b ON (a.vendedor = b.codven AND a.id_emp = b.id_emp) " _
        & " LEFT JOIN jsvencatcli c ON (a.prov_cli = c.codcli AND a.id_emp = c.id_emp) " _
        & " LEFT JOIN jsmerctainv d ON (a.codart = d.codart AND a.id_emp = d.id_emp ) " _
        & " LEFT JOIN (SELECT a.codart, a.unidad, a.equivale, a.uvalencia, a.id_emp  " _
        & "             FROM jsmerequmer a  " _
        & "             WHERE " _
        & "             a.id_emp = '" & jytsistema.WorkID & "' " _
        & "             UNION " _
        & "             SELECT a.codart, a.unidad, 1 equivale, a.unidad uvalencia, a.id_emp  " _
        & "             FROM jsmerctainv a " _
        & "             WHERE  " _
        & "             a.id_emp = '" & jytsistema.WorkID & "') e ON (a.codart = e.codart AND a.unidad = e.uvalencia AND a.id_emp = e.id_emp) " _
        & " WHERE " _
        & " d.tipjer = '" & CodigoJerarquiaHEINZ & "' AND " _
        & strSQL _
        & " a.origen IN ('FAC', 'PFC', 'PVE', 'NCV', 'NDV' ) AND " _
        & " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(EmisionDesde) & "' AND " _
        & " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(EmisionHasta) & "' AND " _
        & " a.id_emp = '" & jytsistema.WorkID & "' " _
        & " ORDER BY a.vendedor "


    End Function



    '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '************************************************************************************************************************
    '////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    Function SeleccionVENPLUS_VEN_VentasClientesMes(strCliente As String, _
                                          strCanal As String, strTipoNegocio As String, _
                                          strZona As String, strRuta As String, strAsesor As String, _
                                          strPais As String, strEstado As String, strMunicipio As String, _
                                          strParroquia As String, strCiudad As String, strBarrio As String, _
                                          TipoReporte As Integer, _
                                          Optional ByVal TipoCliente As Integer = 4, Optional ByVal EstatusCliente As Integer = 4) As String

        Dim strSQL As String = ""
        strSQL += CadenaPLUS_CLIENTES("b", strCliente, strCanal, strTipoNegocio, strZona, strRuta, strAsesor, strPais, strEstado, strMunicipio, strParroquia, strCiudad, strBarrio)

        Dim strCan As String = ReporteTipo(TipoReporte, "a", "c", "m").strCan
        Dim strUnidad As String = ReporteTipo(TipoReporte, "a", "c", "m").strUnidad
        Dim strUND As String = ReporteTipo(TipoReporte, "a", "c", "m").strUND


        SeleccionVENPLUS_VEN_VentasClientesMes = " select a.prov_cli, b.nombre, " _
            & " sum(  if(  month(a.fechamov) = 1,  if( a.origen = 'NCV', -1, 1) *" & strCan & ", 0 )  ) pEne, " _
            & " sum(  if(  month(a.fechamov) = 2,  if( a.origen = 'NCV', -1, 1) *" & strCan & ", 0 )  ) pFeb, " _
            & " sum(  if(  month(a.fechamov) = 3,  if( a.origen = 'NCV', -1, 1) *" & strCan & ", 0 )  ) pMar, " _
            & " sum(  if(  month(a.fechamov) = 4,  if( a.origen = 'NCV', -1, 1) *" & strCan & ", 0 )  ) pAbr, " _
            & " sum(  if(  month(a.fechamov) = 5,  if( a.origen = 'NCV', -1, 1) *" & strCan & ", 0 )  ) pMay, " _
            & " sum(  if(  month(a.fechamov) = 6,  if( a.origen = 'NCV', -1, 1) *" & strCan & ", 0 )  ) pJun, " _
            & " sum(  if(  month(a.fechamov) = 7,  if( a.origen = 'NCV', -1, 1) *" & strCan & ", 0 )  ) pJul, " _
            & " sum(  if(  month(a.fechamov) = 8,  if( a.origen = 'NCV', -1, 1) *" & strCan & ", 0 )  ) pAgo, " _
            & " sum(  if(  month(a.fechamov) = 9,  if( a.origen = 'NCV', -1, 1) *" & strCan & ", 0 )  ) pSep, " _
            & " sum(  if(  month(a.fechamov) = 10,  if( a.origen = 'NCV', -1, 1) *" & strCan & ", 0 )  ) pOct, " _
            & " sum(  if(  month(a.fechamov) = 11,  if( a.origen = 'NCV', -1, 1) *" & strCan & ", 0 )  ) pNov, " _
            & " sum(  if(  month(a.fechamov) = 12,  if( a.origen = 'NCV', -1, 1) *" & strCan & ", 0 )  ) pDic " _
            & " from jsmertramer a " _
            & " left join jsvencatcli b on (a.prov_cli = b.codcli and a.id_emp = b.id_emp) " _
            & " left join (" & SeleccionGENTablaEquivalencias() & ") m on (a.codart = m.codart and " & strUnidad & " = m.uvalencia and a.id_emp = m.id_emp) " _
            & " left join jsmerctainv c on (a.codart = c.codart and a.id_emp = c.id_emp) " _
            & " Where " _
            & " a.prov_cli <> '' and " _
            & " a.origen in ('FAC', 'PFC','NDV','NCV','PVE') and " _
            & " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(PrimerDiaAño(jytsistema.sFechadeTrabajo)) & "' and " _
            & " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(UltimoDiaAño(jytsistema.sFechadeTrabajo)) & "' and " _
            & strSQL _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " Group By " _
            & " Prov_Cli "

    End Function

    Function SeleccionVENPLUS_VEN_ActivacionMESMES(Anio As Integer, _
                                           strCliente As String, _
                                           strCanal As String, strTipoNegocio As String, _
                                           strZona As String, strRuta As String, strAsesor As String, _
                                           strPais As String, strEstado As String, strMunicipio As String, _
                                           strParroquia As String, strCiudad As String, strBarrio As String, _
                                           strMercancias As String, _
                                           strCategorias As String, strMarcas As String, strDivisiones As String, _
                                           strJerarquias As String, strNivel1 As String, strNivel2 As String, _
                                           strNivel3 As String, strNivel4 As String, strNivel5 As String, strNivel6 As String, _
                                           strAlmacen As String, Mercancias_Clientes As Boolean) As String

        Dim strSQL As String = ""
        Dim AñoActual As Integer, AñoAnterior As Integer
        Dim strCuenta As String = ""

        strSQL += CadenaPLUS_CLIENTES("c", strCliente, strCanal, strTipoNegocio, strZona, strRuta, "", strPais, strEstado, strMunicipio, strParroquia, strCiudad, strBarrio)
        strSQL += CadenaPLUS_CLIENTES("a", "", "", "", "", "", strAsesor, "", "", "", "", "", "")
        strSQL += CadenaPLUS_MERCANCIAS("b", strMercancias, strCategorias, strMarcas, strDivisiones, strJerarquias, strNivel1, strNivel2, strNivel3, strNivel4, strNivel5, strNivel6)


        AñoActual = Anio  ' Year(jytsistema.sFechadeTrabajo)
        AñoAnterior = AñoActual - 1


        If Mercancias_Clientes Then
            strCuenta = " a.codart "
        Else
            strCuenta = " a.prov_cli"
        End If

        SeleccionVENPLUS_VEN_ActivacionMESMES = " select " & AñoAnterior & " anio, " _
                & " count(distinct( if ( month(a.fechamov) = 1  , " & strCuenta & ", null))) as act01,  " _
                & " count(distinct( if ( month(a.fechamov) = 2  , " & strCuenta & ", null))) as act02,  " _
                & " count(distinct( if ( month(a.fechamov) = 3  , " & strCuenta & ", null))) as act03,  " _
                & " count(distinct( if ( month(a.fechamov) = 4  , " & strCuenta & ", null))) as act04,  " _
                & " count(distinct( if ( month(a.fechamov) = 5  , " & strCuenta & ", null))) as act05,  " _
                & " count(distinct( if ( month(a.fechamov) = 6  , " & strCuenta & ", null))) as act06,  " _
                & " count(distinct( if ( month(a.fechamov) = 7  , " & strCuenta & ", null))) as act07,  " _
                & " count(distinct( if ( month(a.fechamov) = 8  , " & strCuenta & ", null))) as act08,  " _
                & " count(distinct( if ( month(a.fechamov) = 9  , " & strCuenta & ", null))) as act09,  " _
                & " count(distinct( if ( month(a.fechamov) = 10  , " & strCuenta & ", null))) as act10,  " _
                & " count(distinct( if ( month(a.fechamov) = 11  , " & strCuenta & ", null))) as act11,  " _
                & " count(distinct( if ( month(a.fechamov) = 12  , " & strCuenta & ", null))) as act12 " _
                & " from jsmertramer a " _
                & " left join jsmerctainv b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
                & " left join jsvencatcli c on (a.prov_cli = c.codcli and a.id_emp = c.id_emp) " _
                & " Where " _
                & strSQL _
                & " a.id_emp = '" & jytsistema.WorkID & "' and " _
                & " year(a.fechamov) = " & AñoAnterior & " and " _
                & " a.origen in('FAC','PFC','PVE','NCV','NDV') " _
                & " group by a.id_emp " _
                & " UNION " _
                & " select " & AñoActual & " anio, " _
                & " count(distinct( if ( month(a.fechamov) = 1  , " & strCuenta & ", null))) as act01,  " _
                & " count(distinct( if ( month(a.fechamov) = 2  , " & strCuenta & ", null))) as act02,  " _
                & " count(distinct( if ( month(a.fechamov) = 3  , " & strCuenta & ", null))) as act03,  " _
                & " count(distinct( if ( month(a.fechamov) = 4  , " & strCuenta & ", null))) as act04,  " _
                & " count(distinct( if ( month(a.fechamov) = 5  , " & strCuenta & ", null))) as act05,  " _
                & " count(distinct( if ( month(a.fechamov) = 6  , " & strCuenta & ", null))) as act06,  " _
                & " count(distinct( if ( month(a.fechamov) = 7  , " & strCuenta & ", null))) as act07,  " _
                & " count(distinct( if ( month(a.fechamov) = 8  , " & strCuenta & ", null))) as act08,  " _
                & " count(distinct( if ( month(a.fechamov) = 9  , " & strCuenta & ", null))) as act09,  " _
                & " count(distinct( if ( month(a.fechamov) = 10  , " & strCuenta & ", null))) as act10,  " _
                & " count(distinct( if ( month(a.fechamov) = 11  , " & strCuenta & ", null))) as act11,  " _
                & " count(distinct( if ( month(a.fechamov) = 12  , " & strCuenta & ", null))) as act12 " _
                & " from jsmertramer a " _
                & " left join jsmerctainv b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
                & " left join jsvencatcli c on (a.prov_cli = c.codcli and a.id_emp = c.id_emp) " _
                & " Where " _
                & strSQL _
                & " a.id_emp = '" & jytsistema.WorkID & "' and " _
                & " year(a.fechamov) = " & AñoActual & " and " _
                & " a.origen in('FAC','PFC','PVE','NCV','NDV') " _
                & " group by a.id_emp " _
                & " ORDER BY 1 "



    End Function


    Function SeleccionVENPLUS_VEN_ChequesDevueltos(FechaDesde As Date, FechaHasta As Date, _
                                            strCliente As String, _
                                            strCanal As String, strTipoNegocio As String, _
                                            strZona As String, strRuta As String, strAsesor As String, _
                                            strPais As String, strEstado As String, strMunicipio As String, _
                                            strParroquia As String, strCiudad As String, strBarrio As String, _
                                            Optional ByVal TipoCliente As Integer = 4, Optional ByVal EstatusCliente As Integer = 9, _
                                            Optional Resumido As Boolean = False) As String


        Dim str As String = CadenaPLUS_CLIENTES("d", strCliente, strCanal, strTipoNegocio, strZona, strRuta, "", strPais, strEstado, strMunicipio, strParroquia, strCiudad, strBarrio, TipoCliente, EstatusCliente)
        If strAsesor.Trim() <> "" Then str += " a.CODVEN " & strAsesor & " AND "


        SeleccionVENPLUS_VEN_ChequesDevueltos = " select a.numcheque as cheque, a.codcli codigo_cliente, d.nombre, d.alterno, " _
                     & " f.descrip canal, g.descrip tiponegocio, h.descrip zona, i.nomrut ruta, " _
                     & " concat(e.codven,' ',e.apellidos,' ',e.nombres) asesor,  r.nombre pais, q.nombre estado, " _
                     & " p.nombre municipio, o.nombre parroquia, n.nombre ciudad, m.nombre barrio,   " _
                     & " a.codven as codigo_vendedor,b.nummov as movimiento, b.tipomov tipo, b.emision fecha,  b.hora hora_mov, " _
                     & " b.importe monto_mov, b.formapag forma_pago, b.refer referencia, c.cantidaddev cantidad, " _
                     & " d.nombre cliente, concat(e.apellidos,' ', e.nombres) vendedor, a.dias_mora mora " _
                     & " from (SELECT a.id_emp, a.prov_cli codcli, a.numcheque, b.nummov, b.importe, a.fechadev, IF(c.pagos IS NULL,0,c.pagos) pagos, " _
                     & "        b.importe + IF(c.pagos IS NULL ,0,c.pagos) saldo, b.codven, TO_DAYS(CURRENT_DATE)-TO_DAYS(a.fechadev) dias_mora " _
                     & "        FROM jsbanchedev a " _
                     & "        LEFT JOIN jsventracob b ON (a.numcheque=b.numorg AND a.prov_cli = b.codcli AND a.id_emp=b.id_emp) " _
                     & "        LEFT JOIN (SELECT a.nummov, IFNULL(SUM(a.IMPORTE),0) pagos, a.id_emp " _
                     & "            		FROM jsventracob a " _
                     & "                    WHERE " _
                     & "            		a.tipomov IN ('AB', 'CA','NC') AND " _
                     & "            		a.id_emp = '" & jytsistema.WorkID & "' " _
                     & "            		GROUP BY 1 ) c ON (b.nummov = c.nummov AND b.id_emp = c.id_emp) " _
                     & "        WHERE " _
                     & "        a.fechadev >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and a.fechadev <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
                     & "        a.id_emp = '" & jytsistema.WorkID & "' " _
                     & "        GROUP BY 1,2,3,4,5,6  " & IIf(Resumido, " Having saldo <> 0.00 ", "") & " ) a " _
                     & " left join jsventracob b on (a.nummov = b.nummov and a.id_emp = b.id_emp) " _
                     & " left join (SELECT a.codcli, COUNT(DISTINCT a.numcheque) cantidaddev " _
                     & "            FROM (SELECT a.prov_cli codcli, a.numcheque, b.nummov, b.importe, " _
                     & "                    a.fechadev fecha, IF(c.pagos IS NULL,0,c.pagos) pagos, " _
                     & "        	        b.importe + IF(c.pagos IS NULL,0,c.pagos) saldo, " _
                     & "        	        b.codven, TO_DAYS(CURRENT_DATE)-TO_DAYS(a.fechadev) dias_mora " _
                     & "                    FROM jsbanchedev a " _
                     & "                    LEFT JOIN jsventracob b ON (a.numcheque=b.numorg  AND a.prov_cli = b.codcli  AND a.id_emp=b.id_emp) " _
                     & "                    LEFT JOIN (SELECT a.nummov, IFNULL(SUM(a.IMPORTE),0) pagos, a.id_emp " _
                     & "                        		FROM jsventracob a " _
                     & "                                WHERE " _
                     & "                        		a.tipomov IN ('AB', 'CA','NC') AND " _
                     & "                        		a.id_emp = '" & jytsistema.WorkID & "' " _
                     & "                        		GROUP BY 1 ) c ON (b.nummov = c.nummov AND b.id_emp = c.id_emp) " _
                     & "                    WHERE " _
                     & "                    a.fechadev >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and a.fechadev <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and " _
                     & "                    a.id_emp = '" & jytsistema.WorkID & "' " _
                     & "                    GROUP BY 1,2,3,4,5 " & IIf(Resumido, " Having saldo <> 0.00 ", "") & ") a " _
                     & "            WHERE   YEAR(a.fecha)=YEAR(CURRENT_DATE) GROUP BY 1 ) c on ( a.codcli = c.codcli ) " _
                     & " left join jsvencatcli d on (a.codcli=d.codcli and d.id_emp='" & jytsistema.WorkID & "') " _
                     & " left join jsvencatven e on (a.codven=e.codven and e.id_emp='" & jytsistema.WorkID & "') " _
                     & " left join jsvenliscan f on (d.categoria = f.codigo and d.id_emp = f.id_emp ) " _
                     & " left join jsvenlistip g on (d.unidad = g.codigo and d.id_emp = g.id_emp ) " _
                     & " left join jsconctatab h on (d.zona = h.codigo and d.id_emp = h.id_emp  and h.modulo = '00005') " _
                     & " left join jsvenencrut i on (d.ruta_visita = i.codrut and d.id_emp = i.id_emp and i.tipo = '0' )" _
                     & " left join jsconcatter m on (d.fbarrio = m.codigo and a.id_emp = m.id_emp ) " _
                     & " left join jsconcatter n on (d.fciudad = n.codigo and a.id_emp = n.id_emp ) " _
                     & " left join jsconcatter o on (d.fparroquia = o.codigo and a.id_emp = o.id_emp ) " _
                     & " left join jsconcatter p on (d.fmunicipio = p.codigo and a.id_emp = p.id_emp ) " _
                     & " left join jsconcatter q on (d.festado = q.codigo and a.id_emp = q.id_emp ) " _
                     & " left join jsconcatter r on (d.fpais = r.codigo and a.id_emp = r.id_emp ) " _
                     & " where " _
                     & str _
                     & " b.id_emp = '" & jytsistema.WorkID & "' "

    End Function



End Module
