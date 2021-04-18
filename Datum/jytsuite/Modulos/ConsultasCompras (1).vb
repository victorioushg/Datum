Imports MySql.Data.MySqlClient
Module ConsultasCompras
    Public Enum TipoDatoMercancia
        iUnidadesDeVenta = 0
        iKilogramos = 1
        iMonetarios = 2
    End Enum
    Public Enum TipoConsultaMercancia
        iEntradas_O_Salidas = 0
        iDevoluciones = 1
    End Enum
    Function ConsultaEstadistica(ByVal MyConn As MySqlConnection, ByVal ds As DataSet, ByVal lblInfo As Label, ByVal ProveedorCliente As String, _
                                 ByVal Modulo As String, ByVal tipoConsulta As TipoConsultaMercancia, ByVal TipoDato As TipoDatoMercancia, _
                                 ByVal Fecha As Date, ByVal NombreTablaEstadistica As String, _
                                 Optional ByVal AgrupadoPor As String = " a.codart ") As DataTable

        ' Modulo 'COM' = compras;  'VEN'  = ventas

        ' TipoConsulta
        ' 0 = Entradas/Salidas
        ' 1 = Devoluciones

        'TipoDato
        '0 = Unidades de Venta
        '1 = Kilogramos
        '2 = Monetarios

        Dim strTipoMov As String = ""
        Dim strOrigen As String = ""
        Dim strTipoDato As String = ""
        Dim strMain As String

        Select Case CStr(tipoConsulta) & Modulo
            Case "0COM"
                strTipoMov = " a.tipomov = 'EN' and "
                strOrigen = " (a.origen = 'COM' or a.origen = 'REC' or a.origen = 'NDC' or a.origen = 'NDB') and "
            Case "1COM"
                strTipoMov = " a.tipomov = 'SA' and "
                strOrigen = " ( a.origen = 'NCC' or a.origen = 'NCR' )  and "
            Case "0VEN"
                strTipoMov = " a.tipomov = 'SA' and "
                strOrigen = " (a.origen = 'FAC' OR a.origen = 'PFC' OR a.origen = 'PVE' OR origen = 'NDV') AND "
            Case "1VEN"
                strTipoMov = " a.tipomov = 'EN' and "
                strOrigen = " ( a.origen = 'NCV' or a.origen = 'NCR') and "
        End Select

        Select Case TipoDato
            Case TipoDatoMercancia.iUnidadesDeVenta
                strTipoDato = " IF( b.uvalencia IS NULL, a.cantidad, a.cantidad/b.equivale) "
            Case TipoDatoMercancia.iKilogramos
                strTipoDato = " a.peso "
            Case TipoDatoMercancia.iMonetarios
                If Modulo = "COM" Then
                    strTipoDato = " a.costotaldes "
                Else
                    strTipoDato = " a.ventotaldes "
                End If
        End Select

        strMain = " SELECT a.codart, c.nomart, c.unidad, SUM( " & strTipoDato & " ) mTOT, " _
                         & " SUM( IF( MONTH(a.fechamov) = 1 , " & strTipoDato & ", 0 ) ) mENE, " _
                         & " SUM( IF( MONTH(a.fechamov) = 2 , " & strTipoDato & ", 0 ) ) mFEB, " _
                         & " SUM( IF( MONTH(a.fechamov) = 3 , " & strTipoDato & ", 0 ) ) mMAR, " _
                         & " SUM( IF( MONTH(a.fechamov) = 4 , " & strTipoDato & ", 0 ) ) mABR, " _
                         & " SUM( IF( MONTH(a.fechamov) = 5 , " & strTipoDato & ", 0 ) ) mMAY, " _
                         & " SUM( IF( MONTH(a.fechamov) = 6 , " & strTipoDato & ", 0 ) ) mJUN, " _
                         & " SUM( IF( MONTH(a.fechamov) = 7 , " & strTipoDato & ", 0 ) ) mJUL, " _
                         & " SUM( IF( MONTH(a.fechamov) = 8 , " & strTipoDato & ", 0 ) ) mAGO, " _
                         & " SUM( IF( MONTH(a.fechamov) = 9 , " & strTipoDato & ", 0 ) ) mSEP, " _
                         & " SUM( IF( MONTH(a.fechamov) = 10 , " & strTipoDato & ", 0 ) ) mOCT, " _
                         & " SUM( IF( MONTH(a.fechamov) = 11 , " & strTipoDato & ", 0 ) ) mNOV, " _
                         & " SUM( IF( MONTH(a.fechamov) = 12 , " & strTipoDato & ", 0 ) ) mDIC " _
                         & " FROM jsmertramer a " _
                         & " LEFT JOIN jsmerequmer b ON (a.codart = b.codart AND a.unidad = b.uvalencia AND a.id_emp = b.id_emp) " _
                         & " left join jsmerctainv c on (a.codart = c.codart and a.id_emp = c.id_emp) " _
                         & " WHERE " _
                         & " YEAR(a.fechamov) = " & Year(Fecha) & " AND " _
                         & strTipoMov _
                         & strOrigen _
                         & " a.prov_cli = '" & ProveedorCliente & "' AND " _
                         & " a.id_emp = '" & jytsistema.WorkID & "' " _
                         & " GROUP BY " & AgrupadoPor

        ds = DataSetRequery(ds, strMain, MyConn, NombreTablaEstadistica, lblInfo)
        ConsultaEstadistica = ds.Tables(NombreTablaEstadistica)

    End Function
    Function CuotasAcumuladoAsesor(MyConn As MySqlConnection, ByVal CodigoAsesor As String, ByVal nCampo As String, _
                                   TipoEstadistica As Integer, TipoUnidad As Integer) As String

        Dim str As String = ""

        Select Case TipoEstadistica
            Case 0, 1, 2, 3, 4
                Select Case TipoUnidad
                    Case 0

                        str = " SELECT a.codven, a.item, a.fecha, a." & nCampo & " " _
                        & " from jsvenstaven a " _
                        & " where " _
                        & " a.tipo = 0 and " _
                        & " a.codven = '" & CodigoAsesor & "' and " _
                        & " YEAR(a.fecha) = " & jytsistema.sFechadeTrabajo.Year & " AND " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' "

                    Case 1

                        str = " SELECT a.codven, a.item, a.fecha, a." & nCampo & "/b.pesounidad " & nCampo & " " _
                        & " from jsvenstaven a " _
                        & " LEFT JOIN jsmerctainv b on (a.item = b.codart and a.id_emp = b.id_emp) " _
                        & " where " _
                        & " a.tipo = 0 and " _
                        & " a.codven = '" & CodigoAsesor & "' and " _
                        & " YEAR(a.fecha) = " & jytsistema.sFechadeTrabajo.Year & " AND " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' "


                    Case 2

                        str = " SELECT a.codven, a.item, a.fecha, IF(a." & nCampo & " IS NULL, 0.00, a." & nCampo & "*d.equivale/b.pesounidad) " & nCampo & " " _
                            & " FROM jsvenstaven a " _
                            & " LEFT JOIN jsmerctainv b ON (a.item = b.codart AND a.id_emp = b.id_emp) " _
                            & " LEFT JOIN (SELECT a.codart, a.unidad, a.equivale, a.uvalencia, a.id_emp  " _
                            & "            FROM jsmerequmer a " _
                            & "            WHERE " _
                            & "            a.id_emp = '" & jytsistema.WorkID & "' " _
                            & "            UNION " _
                            & "            SELECT a.codart, a.unidad, 1 equivale, a.unidad uvalencia, a.id_emp " _
                            & "            FROM jsmerctainv a " _
                            & "            WHERE " _
                            & "            a.id_emp = '" & jytsistema.WorkID & "') d  ON (a.item = d.codart AND d.uvalencia = '" & ParametroPlus(MyConn, Gestion.iMercancías, "MERPARAM07") & "' AND a.id_emp = d.id_emp ) " _
                            & " WHERE " _
                            & " a.codven = '" & CodigoAsesor & "' AND " _
                            & " YEAR(a.fecha) = " & jytsistema.sFechadeTrabajo.Year & " AND " _
                            & " a.tipo = 0 AND " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' "

                End Select
            Case 5, 6

                str = " SELECT a.codven, a.item, a.fecha, a." & nCampo & " " _
                        & " from jsvenstaven a " _
                        & " where " _
                        & " a.tipo = " & TipoEstadistica & " and " _
                        & " a.codven = '" & CodigoAsesor & "' and " _
                        & " YEAR(a.fecha) = " & jytsistema.sFechadeTrabajo.Year & " AND " _
                        & " a.id_emp = '" & jytsistema.WorkID & "' "

        End Select

        CuotasAcumuladoAsesor = " SELECT a.codven, SUM( " & nCampo & " ) mTot, SUM(IF(MONTH(fecha) = 1, " & nCampo & ", 0)) mEne, " _
            & " SUM(IF(MONTH(fecha) = 2, " & nCampo & ", 0)) mFeb, SUM(IF(MONTH(fecha) = 3, " & nCampo & ", 0)) mMar, " _
            & " SUM(IF(MONTH(fecha) = 4, " & nCampo & ", 0)) mAbr, SUM(IF(MONTH(fecha) = 5, " & nCampo & ", 0)) mMay, " _
            & " SUM(IF(MONTH(fecha) = 6, " & nCampo & ", 0)) mJun, SUM(IF(MONTH(fecha) = 7, " & nCampo & ", 0)) mJul, " _
            & " SUM(IF(MONTH(fecha) = 8, " & nCampo & ", 0)) mAgo, SUM(IF(MONTH(fecha) = 9, " & nCampo & ", 0)) mSep, " _
            & " SUM(IF(MONTH(fecha) = 10, " & nCampo & ", 0)) mOct, SUM(IF(MONTH(fecha) = 11, " & nCampo & ", 0)) mNov, " _
            & " SUM(IF(MONTH(fecha) = 12, " & nCampo & ", 0)) mDic " _
            & " FROM (" & str & ") a " _
            & " GROUP BY  a.codven "

    End Function
    Public Function SeleccionCOMPRASOrdenDeCompra(ByVal numOrden As String, ByVal CodigoProveedor As String, ByVal FechaIVA As Date) As String
        Return " SELECT a.numord, a.emision, a.entrega, a.codpro, c.nombre, c.rif, c.nit, c.dirfiscal, a.comen, " _
            & " a.tot_net, a.imp_iva imp_iva, a.tot_ord,  " _
            & " b.renglon, b.item, b.descrip, e.comentario, b.iva,  d.monto,  b.unidad, b.cantidad, b.peso, b.COSTOU, b.des_art, b.des_pro,  b.costotot, ELT(b.estatus+1,'','Mercancia no sujeta a Descuentos','Bonificaciones') estatus  " _
            & " FROM jsproencord a " _
            & " LEFT JOIN jsprorenord b ON (a.NUMORD = b.NUMORD AND a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvenrencom e ON (b.NUMORD = e.numdoc AND b.id_emp = e.id_emp AND b.renglon = e.renglon AND e.origen = 'ORD')  " _
            & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT fecha, tipo, monto " _
            & "            FROM jsconctaiva " _
            & "            WHERE " _
            & "            fecha IN (SELECT MAX(fecha) FROM jsconctaiva WHERE fecha <= '" & FormatoFechaMySQL(FechaIVA) & "' GROUP BY tipo) AND " _
            & "            fecha <= '" & FormatoFechaMySQL(FechaIVA) & "')  d ON (b.iva = d.tipo) " _
            & " WHERE " _
            & " a.numord = '" & numOrden & "' AND " _
            & " a.codpro = '" & CodigoProveedor & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY b.estatus, b.renglon "

    End Function
    Public Function SeleccionCOMPRASRecepcion(ByVal numRecepcion As String, ByVal CodigoProveedor As String, ByVal FechaIVA As Date) As String
        Return " SELECT a.numrec numord, a.emision, a.codpro, c.nombre, c.rif, c.nit, c.dirfiscal, a.comen, a.responsable, a.almacen, a.numcom, a.items, a.kilos, " _
            & " a.tot_net, a.imp_iva imp_iva, a.tot_rec tot_ord,  " _
            & " b.renglon, b.item, b.descrip, e.comentario, b.iva,  d.monto,  b.unidad, b.cantidad, b.peso, b.COSTOU, b.des_art, b.des_pro,  b.costotot, ELT(b.estatus+1,'','Mercancia no sujeta a Descuentos','Bonificaciones') estatus  " _
            & " FROM jsproencrep a " _
            & " LEFT JOIN jsprorenrep b ON (a.NUMREC = b.NUMREC AND a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvenrencom e ON (b.NUMREC = e.numdoc AND b.id_emp = e.id_emp AND b.renglon = e.renglon AND e.origen = 'REC')  " _
            & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT fecha, tipo, monto " _
            & "            FROM jsconctaiva " _
            & "            WHERE " _
            & "            fecha IN (SELECT MAX(fecha) FROM jsconctaiva WHERE fecha <= '" & FormatoFechaMySQL(FechaIVA) & "' GROUP BY tipo) AND " _
            & "            fecha <= '" & FormatoFechaMySQL(FechaIVA) & "')  d ON (b.iva = d.tipo) " _
            & " WHERE " _
            & " a.numrec = '" & numRecepcion & "' AND " _
            & " a.codpro = '" & CodigoProveedor & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY b.estatus, b.renglon "

    End Function
    Public Function SeleccionCOMPRASGasto(ByVal numGasto As String, ByVal CodigoProveedor As String, ByVal FechaIVA As Date) As String
        Return " SELECT a.numgas numord, a.emision, a.emisioniva, a.codpro, c.nombre, c.rif, c.nit, c.dirfiscal, a.comen, a.almacen, h.items, h.kilos, " _
            & " a.refer referencia, f.nombre grupo, g.nombre subgrupo, " _
            & " ELT( a.condpag + 1 , concat('CREDITO CON VENCIMIENTO AL ', cast( DATE_FORMAT(a.vence, '%d-%m-%Y') as char ) )  , 'CONTADO', '') condicionpago, " _
            & " a.tot_net, a.imp_iva imp_iva, a.tot_gas tot_ord,  " _
            & " b.renglon, b.item, b.descrip, e.comentario, b.iva,  d.monto,  b.unidad, b.cantidad, b.peso, b.COSTOU, b.des_art, b.des_pro,  b.costotot, ELT(b.estatus+1,'','Mercancia no sujeta a Descuentos','Bonificaciones') estatus  " _
            & " FROM jsproencgas a " _
            & " LEFT JOIN jsprorengas b ON (a.numgas = b.numgas AND a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvenrencom e ON (b.numgas = e.numdoc AND b.id_emp = e.id_emp AND b.renglon = e.renglon AND e.origen = 'GAS')  " _
            & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
            & " left join jsprogrugas f on (a.grupo = f.codigo and a.id_emp = f.id_emp) " _
            & " left join jsprogrugas g on (a.subgrupo = g.codigo and a.id_emp = g.id_emp) " _
            & " left join (select a.numgas, count(*) items, sum(a.peso) kilos, a.id_emp from jsprorengas a where numgas = '" & numGasto & "' and codpro = '" & CodigoProveedor & "' and id_emp = '" & jytsistema.WorkID & "' group by numgas   ) h on (a.numgas = h.numgas and a.id_emp = h.id_emp) " _
            & " LEFT JOIN (SELECT fecha, tipo, monto " _
            & "            FROM jsconctaiva " _
            & "            WHERE " _
            & "            fecha IN (SELECT MAX(fecha) FROM jsconctaiva WHERE fecha <= '" & FormatoFechaMySQL(FechaIVA) & "' GROUP BY tipo) AND " _
            & "            fecha <= '" & FormatoFechaMySQL(FechaIVA) & "')  d ON (b.iva = d.tipo) " _
            & " WHERE " _
            & " a.numgas = '" & numGasto & "' AND " _
            & " a.codpro = '" & CodigoProveedor & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY b.estatus, b.renglon "

    End Function
    Public Function SeleccionCOMPRASCompra(ByVal numDocumento As String, ByVal CodigoProveedor As String, ByVal FechaIVA As Date) As String
        Return " SELECT a.numcom numord, a.emision, a.emisioniva entrega, a.codpro, c.nombre, c.rif, c.nit, c.dirfiscal, a.comen, a.almacen, a.items, a.kilos, " _
            & " a.refer referencia, " _
            & " IF( a.condpag = 0 , CONCAT('CREDITO CON VENCIMIENTO AL ', cast( DATE_FORMAT(a.vence4, '%d-%m-%Y') as char ) )   , 'CONTADO')  condicionpago, " _
            & " a.tot_net, a.descuen, a.imp_iva imp_iva, a.tot_com tot_ord,  " _
            & " b.renglon, b.item, b.descrip, e.comentario, b.iva,  d.monto,  b.unidad, b.cantidad, b.peso, b.COSTOU, b.des_art, b.des_pro,  b.costotot, ELT(b.estatus+1,'','Mercancia no sujeta a Descuentos','Bonificaciones') estatus  " _
            & " FROM jsproenccom a " _
            & " LEFT JOIN jsprorencom b ON (a.numcom = b.numcom AND a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvenrencom e ON (b.numcom = e.numdoc AND b.id_emp = e.id_emp AND b.renglon = e.renglon AND e.origen = 'COM')  " _
            & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT fecha, tipo, monto " _
            & "            FROM jsconctaiva " _
            & "            WHERE " _
            & "            fecha IN (SELECT MAX(fecha) FROM jsconctaiva WHERE fecha <= '" & FormatoFechaMySQL(FechaIVA) & "' GROUP BY tipo) AND " _
            & "            fecha <= '" & FormatoFechaMySQL(FechaIVA) & "')  d ON (b.iva = d.tipo) " _
            & " WHERE " _
            & " a.numcom = '" & numDocumento & "' AND " _
            & " a.codpro = '" & CodigoProveedor & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY b.estatus, b.renglon "

    End Function

    Public Function SeleccionCOMPRASNotaCredito(ByVal numDocumento As String, ByVal CodigoProveedor As String, ByVal FechaIVA As Date) As String
        Return " SELECT a.numncr numord, a.emision, a.emisioniva entrega, a.codpro, c.nombre, c.rif, c.nit, c.dirfiscal, a.comen, a.almacen, a.items, a.kilos, " _
            & " a.refer referencia, " _
            & " a.tot_net, a.imp_iva imp_iva, a.tot_ncr tot_ord,  " _
            & " b.renglon, b.item, b.descrip, e.comentario, b.iva,  " _
            & " g.codigo causa, g.descrip nomcausa, " _
            & " d.monto,  b.unidad, b.cantidad, b.peso, b.precio COSTOU, b.por_acepta_dev des_art, b.totren costotot, ELT(b.estatus+1,'','Mercancia no sujeta a Descuentos','Bonificaciones') estatus  " _
            & " FROM jsproencncr a " _
            & " LEFT JOIN jsprorenncr b ON (a.numncr = b.numncr AND a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvenrencom e ON (b.numncr = e.numdoc AND b.id_emp = e.id_emp AND b.renglon = e.renglon AND e.origen = 'NCC')  " _
            & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT fecha, tipo, monto " _
            & "            FROM jsconctaiva " _
            & "            WHERE " _
            & "            fecha IN (SELECT MAX(fecha) FROM jsconctaiva WHERE fecha <= '" & FormatoFechaMySQL(FechaIVA) & "' GROUP BY tipo) AND " _
            & "            fecha <= '" & FormatoFechaMySQL(FechaIVA) & "')  d ON (b.iva = d.tipo) " _
            & " LEFT JOIN jsvencaudcr g on (b.causa = g.codigo AND b.id_emp = g.id_emp AND g.credito_debito = 0 ) " _
            & " WHERE " _
            & " a.numncr = '" & numDocumento & "' AND " _
            & " a.codpro = '" & CodigoProveedor & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY b.estatus, b.renglon "

    End Function
    Public Function SeleccionCOMPRASNotaDebito(ByVal numDocumento As String, ByVal CodigoProveedor As String, ByVal FechaIVA As Date) As String
        Return " SELECT a.numndb numord, a.emision, a.emisioniva entrega, a.codpro, c.nombre, c.rif, c.nit, c.dirfiscal, a.comen, a.almacen, a.items, a.kilos, " _
            & " a.refer referencia, " _
            & " a.tot_net, a.imp_iva imp_iva, a.tot_ndb tot_ord,  " _
            & " b.renglon, b.item, b.descrip, e.comentario, b.iva,  " _
            & " g.codigo causa, g.descrip nomcausa, " _
            & " d.monto,  b.unidad, b.cantidad, b.peso, b.COSTO costou, b.des_art, b.des_pro, b.totren costotot, ELT(b.estatus+1,'','Mercancia no sujeta a Descuentos','Bonificaciones') estatus  " _
            & " FROM jsproencndb a " _
            & " LEFT JOIN jsprorenndb b ON (a.numndb = b.numndb AND a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvenrencom e ON (b.numndb = e.numdoc AND b.id_emp = e.id_emp AND b.renglon = e.renglon AND e.origen = 'NDC')  " _
            & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT fecha, tipo, monto " _
            & "            FROM jsconctaiva " _
            & "            WHERE " _
            & "            fecha IN (SELECT MAX(fecha) FROM jsconctaiva WHERE fecha <= '" & FormatoFechaMySQL(FechaIVA) & "' GROUP BY tipo) AND " _
            & "            fecha <= '" & FormatoFechaMySQL(FechaIVA) & "')  d ON (b.iva = d.tipo) " _
            & " LEFT JOIN jsvencaudcr g on (b.causa = g.codigo AND b.id_emp = g.id_emp AND g.credito_debito = 1 ) " _
            & " WHERE " _
            & " a.numndb = '" & numDocumento & "' AND " _
            & " a.codpro = '" & CodigoProveedor & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY b.estatus, b.renglon "

    End Function
    Public Function SeleccionCOMPRASComprobantePago(ByVal NumComprobante As String) As String

        SeleccionCOMPRASComprobantePago = " SELECT a.comproba, b.emision, a.codpro, c.nombre, a.refer, a.nummov, a.tipomov, d.concepto, " _
                & " a.importe saldos, 0.00 importe, 0 tipo " _
                & " FROM jsprotrapagcan a " _
                & " LEFT JOIN jsprotrapag b ON (a.nummov = b.nummov AND a.comproba = b.comproba AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
                & " LEFT JOIN jsprotrapag d ON (a.nummov = d.nummov AND a.tipomov = d.tipomov AND a.id_emp = d.id_emp) " _
                & " WHERE " _
                & " a.comproba = '" & NumComprobante & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " UNION " _
                & " SELECT a.comproba, a.emision, a.codpro, b.nombre, a.refer, a.nummov, a.tipomov, a.concepto, " _
                & " 0.00 saldos, a.importe, 1 tipo " _
                & " FROM jsprotrapag a " _
                & " LEFT JOIN jsprocatpro b ON (a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
                & " WHERE " _
                & " a.comproba = '" & NumComprobante & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " ORDER BY nummov, tipo "

    End Function

    Public Function SeleccionCOMPRASComprobantePagoCH(ByVal NumComprobante As String) As String

        SeleccionCOMPRASComprobantePagoCH = " SELECT a.codpro, b.tipomov, a.nummov, d.emision, a.emision emisiondoc, b.refer, " _
                & " b.concepto, 0.00 importe,  b.formapag, d.numpag, d.nompag, d.benefic, c.nombre, a.comproba, if( c.Zona is null, '', c.zona) zona, d.nomban, " _
                & " d.ctaban, d.formato, " _
                & " b.importe saldos, a.id_emp, 0 tipo " _
                & " " _
                & " FROM jsprotrapagcan a " _
                & " LEFT JOIN jsprotrapag b ON (a.nummov = b.nummov AND a.codpro = b.codpro AND a.id_emp = b.id_emp AND b.tipomov IN ('FC','GR','NC', 'ND') ) " _
                & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
                & " LEFT JOIN (SELECT a.comproba, a.emision, a.numpag, a.nompag, a.benefic, if( b.nomban is null, '', b.nomban) nomban, if( b.ctaban is null, '', b.ctaban) ctaban , if(b.formato is null, 1, b.formato) formato , a.codpro FROM jsprotrapag a " _
                                & " LEFT JOIN jsbancatban b ON (a.nompag = b.codban AND a.id_emp = b.id_emp) " _
                                & " WHERE a.comproba = '" & NumComprobante _
                                & "' AND a.id_emp = '" & jytsistema.WorkID _
                                & "' GROUP BY a.comproba) d ON (a.comproba = d.comproba and a.codpro = d.codpro) " _
                & " Where " _
                & " a.comproba = '" & NumComprobante & "' AND " _
                & " a.tipomov IN ('FC', 'GR', 'NC', 'ND') AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " UNION " _
                & " SELECT a.codpro, IF( a.tipomov IN ('NC','ND','FC','GR'),'', a.tipomov) tipomov, a.nummov, a.emision emision, a.emision emisiondoc, a.refer, " _
                & " a.concepto, SUM(a.importe) importe,  a.formapag, a.numpag , a.nompag, a.benefic,b.nombre, a.comproba, if ( b.Zona is null, '', b.zona) zona, if ( c.nomban is null, '', c.nomban) nomban, if(c.ctaban is null, '', c.ctaban) ctaban ,if(c.formato is null, 1, c.formato) formato ,  " _
                & " 0.00 saldos, a.id_emp, 1 tipo " _
                & "  " _
                & " FROM jsprotrapag a " _
                & " LEFT JOIN jsprocatpro b ON (a.codpro = b.codpro AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN jsbancatban c ON (a.nompag = c.codban AND a.id_emp = c.id_emp) " _
                & " Where " _
                & " a.comproba = '" & NumComprobante & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " GROUP BY a.comproba " _
                & " order by tipo, tipomov, emisiondoc "

    End Function
    Function SeleccionCOMPRASListadoOrdenesDeCompra(ByVal DocumentoDesde As String, ByVal DocumentoHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                          ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, _
                                          Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                          Optional ByVal UnidadDesde As String = "", Optional ByVal UnidadHasta As String = "", _
                                          Optional ByVal TipoProveedor As Integer = 0) As String
        'Tipo Proveedor: 0 = compras; 1 = gastos; 2 = todos

        Dim str As String = ""

        str += " a.emision >= '" & FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' and "
        If CategoriaDesde <> "" Then str += " c.categoria >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then str += " c.categoria <= '" & CategoriaHasta & "' and "
        If UnidadDesde <> "" Then str += " c.unidad >= '" & UnidadDesde & "' and "
        If UnidadHasta <> "" Then str += " c.unidad <= '" & UnidadHasta & "' and "
        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "
        If DocumentoDesde <> "" Then str += " a.numord >= '" & DocumentoDesde & "' and "
        If DocumentoHasta <> "" Then str += " a.numord <= '" & DocumentoHasta & "' and "
        If TipoProveedor < 2 Then str += " c.tipo = " & TipoProveedor & " and "

        Return " select a.numord numcom, a.emision, a.codpro, c.nombre, c.rif, " _
        & " f.descrip categoriaprov, g.descrip unidadnegocioprov, " _
        & " a.tot_net, 0.00 descuen, 0.00 cargos, a.imp_iva, a.tot_ord tot_com " _
        & " from jsproencord a " _
        & " left join jsprocatpro c on (a.codpro = c.codpro and a.id_emp = c.id_emp) " _
        & " left join jsproliscat f on (c.categoria = f.codigo and c.id_emp = f.id_emp) " _
        & " left join jsprolisuni g on (c.unidad = g.codigo  and c.id_emp = g.id_emp) " _
        & " Where " _
        & str _
        & " a.id_emp  = '" & jytsistema.WorkID & "' order by a.emision, a." & OrdenadoPor

    End Function
    Function SeleccionCOMPRASListadoRecepciones(ByVal DocumentoDesde As String, ByVal DocumentoHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                          ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, _
                                          Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                          Optional ByVal UnidadDesde As String = "", Optional ByVal UnidadHasta As String = "", _
                                          Optional ByVal TipoProveedor As Integer = 0) As String
        'Tipo Proveedor: 0 = compras; 1 = gastos; 2 = todos

        Dim str As String = ""

        str += " a.emision >= '" & FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' and "
        If CategoriaDesde <> "" Then str += " c.categoria >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then str += " c.categoria <= '" & CategoriaHasta & "' and "
        If UnidadDesde <> "" Then str += " c.unidad >= '" & UnidadDesde & "' and "
        If UnidadHasta <> "" Then str += " c.unidad <= '" & UnidadHasta & "' and "
        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "
        If DocumentoDesde <> "" Then str += " a.numrec >= '" & DocumentoDesde & "' and "
        If DocumentoHasta <> "" Then str += " a.numrec <= '" & DocumentoHasta & "' and "
        If TipoProveedor < 2 Then str += " c.tipo = " & TipoProveedor & " and "

        Return " select a.numrec numcom, a.emision, a.codpro, c.nombre, c.rif, " _
        & " f.descrip categoriaprov, g.descrip unidadnegocioprov, " _
        & " a.tot_net, 0.00 descuen, 0.00 cargos, a.imp_iva, a.tot_rec tot_com " _
        & " from jsproencrep a " _
        & " left join jsprocatpro c on (a.codpro = c.codpro and a.id_emp = c.id_emp) " _
        & " left join jsproliscat f on (c.categoria = f.codigo and c.id_emp = f.id_emp) " _
        & " left join jsprolisuni g on (c.unidad = g.codigo  and c.id_emp = g.id_emp) " _
        & " Where " _
        & str _
        & " a.id_emp  = '" & jytsistema.WorkID & "' order by a.emision, a." & OrdenadoPor

    End Function




    Function SeleccionCOMPRASListadoCompras(ByVal DocumentoDesde As String, ByVal DocumentoHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                          ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, _
                                          Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                          Optional ByVal UnidadDesde As String = "", Optional ByVal UnidadHasta As String = "", _
                                          Optional ByVal TipoProveedor As Integer = 0) As String
        'Tipo Proveedor: 0 = compras; 1 = gastos; 2 = todos

        Dim str As String = ""

        str += " a.emision >= '" & FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' and "
        If CategoriaDesde <> "" Then str += " c.categoria >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then str += " c.categoria <= '" & CategoriaHasta & "' and "
        If UnidadDesde <> "" Then str += " c.unidad >= '" & UnidadDesde & "' and "
        If UnidadHasta <> "" Then str += " c.unidad <= '" & UnidadHasta & "' and "
        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "
        If DocumentoDesde <> "" Then str += " a.numcom >= '" & DocumentoDesde & "' and "
        If DocumentoHasta <> "" Then str += " a.numcom <= '" & DocumentoHasta & "' and "
        If TipoProveedor < 2 Then str += " c.tipo = " & TipoProveedor & " and "

        Return " select a.numcom numcom, a.emision, a.codpro, c.nombre, c.rif, " _
        & " f.descrip categoriaprov, g.descrip unidadnegocioprov, " _
        & " a.tot_net, a.descuen, a.cargos, a.imp_iva, a.tot_com tot_com " _
        & " from jsproenccom a " _
        & " left join jsprocatpro c on (a.codpro = c.codpro and a.id_emp = c.id_emp) " _
        & " left join jsproliscat f on (c.categoria = f.codigo and c.id_emp = f.id_emp) " _
        & " left join jsprolisuni g on (c.unidad = g.codigo  and c.id_emp = g.id_emp) " _
        & " Where " _
        & str _
        & " a.id_emp  = '" & jytsistema.WorkID & "' order by a.emision, a." & OrdenadoPor

    End Function


    Function SeleccionCOMPRASListadoComprasGastosNCRSinRetencion(ByVal DocumentoDesde As String, ByVal DocumentoHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                          ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, _
                                          Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                          Optional ByVal UnidadDesde As String = "", Optional ByVal UnidadHasta As String = "", _
                                          Optional ByVal TipoProveedor As Integer = 0) As String


        Dim str As String = ""

        str += " a.emision >= '" & FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' and "
        If CategoriaDesde <> "" Then str += " c.categoria >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then str += " c.categoria <= '" & CategoriaHasta & "' and "
        If UnidadDesde <> "" Then str += " c.unidad >= '" & UnidadDesde & "' and "
        If UnidadHasta <> "" Then str += " c.unidad <= '" & UnidadHasta & "' and "
        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "
        If DocumentoDesde <> "" Then str += " a.numcom >= '" & DocumentoDesde & "' and "
        If DocumentoHasta <> "" Then str += " a.numcom <= '" & DocumentoHasta & "' and "
        If TipoProveedor < 2 Then str += " c.tipo = " & TipoProveedor & " and "

        Return " select a.numcom numcom, a.emision, a.codpro, c.nombre, c.rif, " _
        & " f.descrip categoriaprov, g.descrip unidadnegocioprov, " _
        & " a.tot_net, a.descuen, a.cargos, a.imp_iva, a.tot_com tot_com " _
        & " from jsproenccom a " _
        & " LEFT JOIN jsprotrapag b ON (a.numcom = b.nummov AND a.id_emp = b.id_emp AND b.tipomov = 'NC' AND SUBSTRING(b.concepto,13) = 'RETENCION IVA') " _
        & " left join jsprocatpro c on (a.codpro = c.codpro and a.id_emp = c.id_emp) " _
        & " left join jsproliscat f on (c.categoria = f.codigo and c.id_emp = f.id_emp) " _
        & " left join jsprolisuni g on (c.unidad = g.codigo  and c.id_emp = g.id_emp) " _
        & " Where " _
        & str _
        & " b.NUMMOV IS NULL AND " _
        & " a.imp_iva <> 0.00 AND " _
        & " a.id_emp  = '" & jytsistema.WorkID & "' " _
        & " UNION select a.numgas numcom, a.emision, a.codpro, c.nombre, c.rif, " _
        & " f.descrip categoriaprov, g.descrip unidadnegocioprov, " _
        & " a.tot_net, a.descuen, a.cargos, a.imp_iva, a.tot_gas tot_com " _
        & " from jsproencgas a " _
        & " LEFT JOIN jsprotrapag b ON (a.numgas = b.nummov AND a.id_emp = b.id_emp AND b.tipomov = 'NC' AND SUBSTRING(b.concepto,13) = 'RETENCION IVA') " _
        & " left join jsprocatpro c on (a.codpro = c.codpro and a.id_emp = c.id_emp) " _
        & " left join jsproliscat f on (c.categoria = f.codigo and c.id_emp = f.id_emp) " _
        & " left join jsprolisuni g on (c.unidad = g.codigo  and c.id_emp = g.id_emp) " _
        & " Where " _
        & str _
        & " b.NUMMOV IS NULL AND " _
        & " a.imp_iva <> 0.00 AND " _
        & " a.id_emp  = '" & jytsistema.WorkID & "' " _
        & " UNION select a.numncr numcom, a.emision, a.codpro, c.nombre, c.rif, " _
        & " f.descrip categoriaprov, g.descrip unidadnegocioprov, " _
        & " -1*a.tot_net, 0 descuen, 0 cargos, -1*a.imp_iva, -1*a.tot_ncr tot_com " _
        & " from jsproencncr a " _
        & " LEFT JOIN jsprotrapag b ON (a.numncr = b.nummov AND a.id_emp = b.id_emp AND b.tipomov = 'ND' AND SUBSTRING(b.concepto,13) = 'RETENCION IVA') " _
        & " left join jsprocatpro c on (a.codpro = c.codpro and a.id_emp = c.id_emp) " _
        & " left join jsproliscat f on (c.categoria = f.codigo and c.id_emp = f.id_emp) " _
        & " left join jsprolisuni g on (c.unidad = g.codigo  and c.id_emp = g.id_emp) " _
        & " Where " _
        & str _
        & " b.NUMMOV IS NULL AND " _
        & " a.imp_iva <> 0.00 AND " _
        & " a.id_emp  = '" & jytsistema.WorkID & "' " _
        & " order by emision, numcom "

    End Function



    Function SeleccionCOMPRASListadoNotasCredito(ByVal DocumentoDesde As String, ByVal DocumentoHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                          ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, _
                                          Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                          Optional ByVal UnidadDesde As String = "", Optional ByVal UnidadHasta As String = "", _
                                          Optional ByVal TipoProveedor As Integer = 0) As String
        'Tipo Proveedor: 0 = compras; 1 = gastos; 2 = todos

        Dim str As String = ""

        str += " a.emision >= '" & FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' and "
        If CategoriaDesde <> "" Then str += " c.categoria >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then str += " c.categoria <= '" & CategoriaHasta & "' and "
        If UnidadDesde <> "" Then str += " c.unidad >= '" & UnidadDesde & "' and "
        If UnidadHasta <> "" Then str += " c.unidad <= '" & UnidadHasta & "' and "
        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "
        If DocumentoDesde <> "" Then str += " a.numncr >= '" & DocumentoDesde & "' and "
        If DocumentoHasta <> "" Then str += " a.numncr <= '" & DocumentoHasta & "' and "
        If TipoProveedor < 2 Then str += " c.tipo = " & TipoProveedor & " and "

        Return " select a.numncr numcom, a.emision, a.codpro, c.nombre, c.rif, " _
        & " f.descrip categoriaprov, g.descrip unidadnegocioprov, " _
        & " a.tot_net, 0.00 descuen, 0.00 cargos, a.imp_iva, a.tot_ncr tot_com " _
        & " from jsproencncr a " _
        & " left join jsprocatpro c on (a.codpro = c.codpro and a.id_emp = c.id_emp) " _
        & " left join jsproliscat f on (c.categoria = f.codigo and c.id_emp = f.id_emp) " _
        & " left join jsprolisuni g on (c.unidad = g.codigo  and c.id_emp = g.id_emp) " _
        & " Where " _
        & str _
        & " a.id_emp  = '" & jytsistema.WorkID & "' order by a.emision, a." & OrdenadoPor

    End Function

    Function SeleccionCOMPRASListadoNotasDebito(ByVal DocumentoDesde As String, ByVal DocumentoHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                          ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, _
                                          Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                          Optional ByVal UnidadDesde As String = "", Optional ByVal UnidadHasta As String = "", _
                                          Optional ByVal TipoProveedor As Integer = 0) As String
        'Tipo Proveedor: 0 = compras; 1 = gastos; 2 = todos

        Dim str As String = ""

        str += " a.emision >= '" & FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' and "
        If CategoriaDesde <> "" Then str += " c.categoria >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then str += " c.categoria <= '" & CategoriaHasta & "' and "
        If UnidadDesde <> "" Then str += " c.unidad >= '" & UnidadDesde & "' and "
        If UnidadHasta <> "" Then str += " c.unidad <= '" & UnidadHasta & "' and "
        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "
        If DocumentoDesde <> "" Then str += " a.numndb >= '" & DocumentoDesde & "' and "
        If DocumentoHasta <> "" Then str += " a.numndb <= '" & DocumentoHasta & "' and "
        If TipoProveedor < 2 Then str += " c.tipo = " & TipoProveedor & " and "

        Return " select a.numndb numcom, a.emision, a.codpro, c.nombre, c.rif, " _
        & " f.descrip categoriaprov, g.descrip unidadnegocioprov, " _
        & " a.tot_net, 0.00 descuen, 0.00 cargos, a.imp_iva, a.tot_ndb tot_com " _
        & " from jsproencndb a " _
        & " left join jsprocatpro c on (a.codpro = c.codpro and a.id_emp = c.id_emp) " _
        & " left join jsproliscat f on (c.categoria = f.codigo and c.id_emp = f.id_emp) " _
        & " left join jsprolisuni g on (c.unidad = g.codigo  and c.id_emp = g.id_emp) " _
        & " Where " _
        & str _
        & " a.id_emp  = '" & jytsistema.WorkID & "' order by a.emision, a." & OrdenadoPor

    End Function

    Function SeleccionCOMPRASListadoGastos(ByVal DocumentoDesde As String, ByVal DocumentoHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                      ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, _
                                      Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                      Optional ByVal UnidadDesde As String = "", Optional ByVal UnidadHasta As String = "", _
                                      Optional ByVal Grupo As String = "", Optional ByVal SubGrupo As String = "", _
                                      Optional ByVal TipoProveedor As Integer = 0) As String
        'Tipo Proveedor: 0 = compras; 1 = gastos; 2 = todos

        Dim str As String = ""

        str += " a.emision >= '" & FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' and "
        If CategoriaDesde <> "" Then str += " c.categoria >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then str += " c.categoria <= '" & CategoriaHasta & "' and "
        If UnidadDesde <> "" Then str += " c.unidad >= '" & UnidadDesde & "' and "
        If UnidadHasta <> "" Then str += " c.unidad <= '" & UnidadHasta & "' and "
        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "
        If DocumentoDesde <> "" Then str += " a.numgas >= '" & DocumentoDesde & "' and "
        If DocumentoHasta <> "" Then str += " a.numgas <= '" & DocumentoHasta & "' and "
        If Grupo <> "" Then str += " a.grupo >= " & Grupo & " and "
        If SubGrupo <> "" Then str += " a.subgrupo <= " & SubGrupo & " and "
        If TipoProveedor < 2 Then str += " c.tipo = " & TipoProveedor & " and "

        Return " select a.numgas numcom, a.emision, a.codpro, c.nombre, c.rif, " _
        & " f.descrip categoriaprov, g.descrip unidadnegocioprov, " _
        & " a.tot_net, a.descuen, a.cargos, a.imp_iva, a.tot_gas tot_com " _
        & " from jsproencgas a " _
        & " left join jsprocatpro c on (a.codpro = c.codpro and a.id_emp = c.id_emp) " _
        & " left join jsproliscat f on (c.categoria = f.codigo and c.id_emp = f.id_emp) " _
        & " left join jsprolisuni g on (c.unidad = g.codigo  and c.id_emp = g.id_emp) " _
        & " Where " _
        & str _
        & " a.id_emp  = '" & jytsistema.WorkID & "' order by a.emision, a." & OrdenadoPor

    End Function

    Function SeleccionCOMPRASProveedores(ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                      Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                      Optional ByVal UnidadDesde As String = "", Optional ByVal UnidadHasta As String = "", _
                                      Optional ByVal TipoProveedor As Integer = 0) As String
        'Tipo Proveedor: 0 = compras; 1 = gastos; 2 = todos

        Dim str As String = ""

        If CategoriaDesde <> "" Then str += " a.categoria >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then str += " a.categoria <= '" & CategoriaHasta & "' and "
        If UnidadDesde <> "" Then str += " a.unidad >= '" & UnidadDesde & "' and "
        If UnidadHasta <> "" Then str += " a.unidad <= '" & UnidadHasta & "' and "
        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "
        If TipoProveedor < 2 Then str += " a.tipo = " & TipoProveedor & " and "

        Return " SELECT a.codpro, a.nombre, a.asignado, a.RIF, a.NIT, a.DIRFISCAL, a.dirprove, " _
            & " f.descrip categoriaprov, g.descrip unidadnegocioprov,  a.EMAIL1, a.EMAIL2, a.EMAIL3, a.EMAIL4, email5, a.sitioweb, " _
            & " a.TELEF1, a.TELEF2, a.telef3,  a.fax, a.gerente, a.telger, a.contacto, a.telcon, a.limcredito, a.disponible, a.desc1, a.desc2, a.desc3, a.dias2, a.dias3, a.observacion, a.zona, " _
            & " h.descrip zonanombre, a.cobrador, a.vendedor, a.saldo, a.ultpago, a.fecultpago, a.forultpago, a.regimeniva, a.formapago, a.banco, a.ctabanco, a.bancodeposito1, a.bancodeposito2, a.ctabancodeposito1, a.ctabancodeposito2, a.ingreso, a.codcon, a.estatus, a.tipo,  a.ID_EMP  " _
            & " FROM jsprocatpro a " _
            & " left join jsproliscat f on (a.categoria = f.codigo and a.id_emp = f.id_emp) " _
            & " left join jsprolisuni g on (a.unidad = g.codigo  and a.id_emp = g.id_emp) " _
            & " left join jsconctatab h on (a.zona = h.codigo and a.id_emp = h.id_emp and h.modulo = '00008') " _
            & " Where " _
            & str _
            & " a.id_emp  = '" & jytsistema.WorkID & "' order by a." & OrdenadoPor

    End Function

    Public Function SeleccionCOMPRASSaldoProveedores(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                              ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, ByVal OrdenadoPor As String, ByVal operador As Integer, _
                                              ByVal FechaHasta As Date, _
                                              Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                              Optional ByVal UnidadDesde As String = "", Optional ByVal UnidadHasta As String = "", _
                                              Optional ByVal TipoProveedor As Integer = 0, Optional ByVal EstatusProveedor As Integer = 0, _
                                              Optional CxP_ExP As Integer = 0) As String


        Dim str As String = ""

        If CategoriaDesde <> "" Then str += " a.categoria >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then str += " a.categoria <= '" & CategoriaHasta & "' and "
        If UnidadDesde <> "" Then str += " a.unidad >= '" & UnidadDesde & "' and "
        If UnidadHasta <> "" Then str += " a.unidad <= '" & UnidadHasta & "' and "
        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "
        If TipoProveedor < 2 Then str += " a.tipo = " & TipoProveedor & " and "
        If EstatusProveedor < 2 Then str += " a.estatus = " & EstatusProveedor & " and "

        Dim TablaSaldos As String = "tbl" & Format(NumeroAleatorio(10000), "00000")
        EjecutarSTRSQL(MyConn, lblInfo, " drop temporary table if exists " & TablaSaldos)
        EjecutarSTRSQL(MyConn, lblInfo, " create temporary table " & TablaSaldos & " select a.codpro, sum(a.importe) saldo " _
            & " from jsprotrapag a " _
            & " where " _
            & IIf(CxP_ExP = 0, " a.REMESA = '' AND ", " a.REMESA = '1' AND ") _
            & " a.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by a.codpro " _
            & " having round(saldo,0) <> 0.00 ")

        SeleccionCOMPRASSaldoProveedores = " select a.codpro, a.nombre, a.asignado, a.ingreso, a.rif, f.descrip categoriaprov, " _
            & " g.descrip unidadnegocioprov, h.descrip zona, " _
            & " if( b.saldo is null, 0.00, b.saldo) saldo " _
            & " from jsprocatpro a " _
            & " left join " & TablaSaldos & " b on ( a.codpro = b.codpro ) " _
            & " left join jsproliscat f on (a.categoria = f.codigo and a.id_emp = f.id_emp ) " _
            & " left join jsprolisuni g on (a.unidad = g.codigo and a.id_emp = g.id_emp ) " _
            & " left join jsconctatab h on (a.zona = h.codigo and a.id_emp = h.id_emp  and h.modulo = '00008') " _
            & " Where " _
            & " b.saldo <> 0.00 and " _
            & str _
            & " a.id_emp ='" & jytsistema.WorkID & "' " _
            & " group by a.codpro order by a." & OrdenadoPor

    End Function
    '    Public Function SeleccionCOMPRASEstadoDeCuentaProveedoresPLUS(ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, ByVal OrdenadoPor As String, ByVal operador As Integer, _
    '                                              ByVal FEchaDesde As Date, ByVal FechaHasta As Date, _
    '                                              Optional ByVal TipoProveedor As Integer = 0, Optional ByVal EstatusProveedor As Integer = 0) As String


    '        Dim str As String = ""

    '        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
    '        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "
    '        If TipoProveedor < 2 Then str += " a.tipo = " & TipoProveedor & " and "
    '        If EstatusProveedor < 2 Then str += " a.estatus = " & EstatusProveedor & " and "


    '        Return " SELECT a.codpro, a.nombre, a.asignado, a.ingreso, a.rif, e.nummov, e.tipomov, e.emision, e.hora, e.vence, e.refer, e.concepto, " _
    '            & " IF (e.importe < 0 , ABS(e.importe), 0.00) debitos, IF (e.importe > 0 , ABS(e.importe), 0.00) creditos,  " _
    '            & " IF( b.saldo IS NULL, 0.00, b.saldo) saldo " _
    '            & " FROM jsprocatpro a " _
    '            & " LEFT JOIN (SELECT a.codpro, SUM(a.importe) saldo FROM jsprotrapag a WHERE a.emision < '2014-01-01' AND a.id_emp = '01' GROUP BY a.codpro ) b ON ( a.codpro = b.codpro ) " _
    '            & " LEFT JOIN jsprotrapag e ON (a.codpro = e.codpro AND a.id_emp = e.id_emp AND e.origen NOT IN ('CXC') ) " _
    '            & " WHERE"
    '             e.emision >= '2014-01-01' AND 
    '             e.emision <= '2014-07-30' AND 
    '             a.codpro = '0000000132' AND
    '             a.id_emp ='01' 
    '        UNION()
    'SELECT a.codpro, a.nombre, a.asignado, a.ingreso, a.rif, 
    '             e.nummov, e.tipomov, e.emision, e.hora, e.vence, e.refer, e.concepto, 
    '             IF (e.importe < 0 , ABS(e.importe), 0.00) debitos,  
    '             IF (e.importe > 0 , ABS(e.importe), 0.00) creditos,  
    '             IF( b.saldo IS NULL, 0.00, b.saldo) saldo 
    '             FROM jsprocatpro a 
    '             LEFT JOIN (SELECT a.codpro, SUM(a.importe) saldo 
    '                         FROM jsprotrapag a 
    '                         WHERE 
    '                         a.emision < '2014-01-01' AND 
    '                         a.id_emp = '01' 
    '                         GROUP BY a.codpro ) b ON ( a.codpro = b.codpro ) 
    '             LEFT JOIN jsprotrapag e ON (a.codpro = e.codpro AND a.id_emp = e.id_emp AND e.origen IN ('CXC') AND SUBSTRING(e.concepto,1,9) = 'RETENCION' ) 
    '             WHERE 
    '             e.emision >= '2014-01-01' AND 
    '             e.emision <= '2014-07-30' AND 
    '             a.codpro = '0000000132' AND
    '             a.id_emp ='01'   
    ' UNION
    ' SELECT a.codpro, a.nombre, a.asignado, a.ingreso, a.rif, 
    '             e.nummov, e.tipomov, e.emision, e.hora, e.vence, e.refer, e.concepto, 
    '             IF (e.importe < 0 , ABS(e.importe), 0.00) debitos,  
    '             IF (e.importe > 0 , ABS(e.importe), 0.00) creditos,  
    '             IF( b.saldo IS NULL, 0.00, b.saldo) saldo 
    '             FROM jsprocatpro a 
    '             LEFT JOIN (SELECT a.codpro, SUM(a.importe) saldo 
    '                         FROM jsprotrapag a 
    '                         WHERE 
    '                         a.emision < '2014-01-01' AND 
    '                         a.id_emp = '01' 
    '                         GROUP BY a.codpro ) b ON ( a.codpro = b.codpro ) 
    '             LEFT JOIN jsprotrapag e ON (a.codpro = e.codpro AND a.id_emp = e.id_emp AND e.origen IN ('CXC') AND SUBSTRING(e.concepto,1,9) <> 'RETENCION' AND e.tipomov IN ('FC','ND','NC','GR') ) 
    '             WHERE 
    '             e.emision >= '2014-01-01' AND 
    '             e.emision <= '2014-07-30' AND 
    '             a.codpro = '0000000132' AND
    '             a.id_emp ='01'   
    ' UNION
    ' SELECT a.codpro, a.nombre, a.asignado, a.ingreso, a.rif, 
    '             e.comproba nummov, e.tipomov, e.emision, e.hora, e.vence, e.refer, e.concepto, 
    '             IF (SUM(e.importe) < 0 , ABS(SUM(e.importe)), 0.00) debitos,  
    '             IF (SUM(e.importe) > 0 , ABS(SUM(e.importe)), 0.00) creditos,  
    '             IF( b.saldo IS NULL, 0.00, b.saldo) saldo 
    '             FROM jsprocatpro a 
    '             LEFT JOIN (SELECT a.codpro, SUM(a.importe) saldo 
    '                         FROM jsprotrapag a 
    '                         WHERE 
    '                         a.emision < '2014-01-01' AND 
    '                         a.id_emp = '01' 
    '                         GROUP BY a.codpro ) b ON ( a.codpro = b.codpro ) 
    '             LEFT JOIN jsprotrapag e ON (a.codpro = e.codpro AND a.id_emp = e.id_emp AND e.origen IN ('CXC') AND e.tipomov IN ('AB', 'CA')  ) 
    '             WHERE 
    '             e.emision >= '2014-01-01' AND 
    '             e.emision <= '2014-07-30' AND 
    '             a.codpro = '0000000132' AND
    '             a.id_emp ='01'   
    '             GROUP BY e.comproba            
    '             ORDER BY 7     



    '    End Function

    Public Function SeleccionCOMPRASEstadoDeCuentaProveedores(ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, ByVal OrdenadoPor As String, ByVal operador As Integer, _
                                              ByVal FEchaDesde As Date, ByVal FechaHasta As Date, _
                                              Optional ByVal TipoProveedor As Integer = 0, Optional ByVal EstatusProveedor As Integer = 0, _
                                              Optional CxP_ExP As Integer = 0) As String


        Dim str As String = ""

        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "
        If TipoProveedor < 2 Then str += " a.tipo = " & TipoProveedor & " and "
        If EstatusProveedor < 2 Then str += " a.estatus = " & EstatusProveedor & " and "

        SeleccionCOMPRASEstadoDeCuentaProveedores = " select a.codpro, a.nombre, a.asignado, a.ingreso, a.rif, f.descrip categoriaprov, " _
            & " g.descrip unidadnegocioprov, h.descrip zona, " _
            & " e.nummov, e.tipomov, e.emision, e.hora, e.vence, e.refer, e.concepto, " _
            & " if (e.importe < 0 , abs(e.importe), 0.00) creditos,  " _
            & " if (e.importe > 0 , abs(e.importe), 0.00) debitos,  " _
            & " if( b.saldo is null, 0.00, -1*b.saldo) saldo " _
            & " from jsprocatpro a " _
            & " left join (select a.codpro, sum(a.importe) saldo " _
            & "             from jsprotrapag a " _
            & "             where " _
            & IIf(CxP_ExP = 0, " a.REMESA = '' AND ", " a.REMESA = '1' AND ") _
            & "             a.emision < '" & FormatoFechaMySQL(FEchaDesde) & "' and " _
            & "             a.id_emp = '" & jytsistema.WorkID & "' " _
            & "             group by a.codpro ) b on ( a.codpro = b.codpro ) " _
            & " left join jsprotrapag e on (a.codpro = e.codpro and a.id_emp = e.id_emp ) " _
            & " left join jsproliscat f on (a.categoria = f.codigo and a.id_emp = f.id_emp ) " _
            & " left join jsprolisuni g on (a.unidad = g.codigo and a.id_emp = g.id_emp ) " _
            & " left join jsconctatab h on (a.zona = h.codigo and a.id_emp = h.id_emp  and h.modulo = '00008') " _
            & " Where " _
            & IIf(CxP_ExP = 0, " e.REMESA = '' AND ", " e.REMESA = '1' AND ") _
            & " e.emision >= '" & FormatoFechaMySQL(FEchaDesde) & "' and " _
            & " e.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' and " _
            & str _
            & " a.id_emp ='" & jytsistema.WorkID & "' " _
            & " order by a." & OrdenadoPor & ", e.emision "

    End Function

    Function SeleccionCOMPRASListadoRetencionesISLR(ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                                    ProveedorDesde As String, ProveedorHasta As String) As String

        Dim str As String = ""
        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "

        Return " SELECT a.NUMCOM, a.EMISION emision, a.CODPRO, " _
            & " c.NOMBRE AS NOMPRO, c.RIF, c.dirfiscal,  '' numcredito, f.concepto, '' fac_afectada,  a.TOT_COM, " _
            & " IF( ISNULL(d.baseiva), 0.00, d.baseiva) montoexento, " _
            & " b.TIPOIVA, f.interes PORISLR, f.capital BASEIMPONIBLE, " _
            & " f.IMPorte IMP_Islr, e.num_control, a.ret_islr retencion, a.num_ret_islr num_retencion, " _
            & " a.fecha_ret_islr fec_retencion, REPLACE(g.rif,'-','') RifAgente, CAST(DATE_FORMAT(a.emision,'%Y%m') AS UNSIGNED INTEGER) PERIODO, " _
            & " REPLACE(c.rif, '-','') RifRetenido, SUBSTRING(f.concepto,1,3) CodigoConcepto, " _
            & " a.base_ret_islr MontoOperacion, a.por_ret_islr PorcentajeRetencion " _
            & " FROM jsproenccom a " _
            & " LEFT JOIN jsproivacom b ON (a.numcom = b.numcom AND a.codpro = b.codpro AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp AND NOT b.tipoiva IN ('','E') ) " _
            & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsproivacom d ON (a.numcom = d.numcom AND a.codpro = d.codpro AND a.id_emp = d.id_emp AND d.tipoiva = '' ) " _
            & " LEFT JOIN jsconnumcon e ON (a.numcom = e.numdoc AND a.codpro  = e.prov_cli and a.id_emp = e.id_emp AND e.org = 'COM' AND e.origen = 'COM') " _
            & " left join jsprotrapag f on (a.codpro = f.codpro and a.numcom = f.nummov and a.id_emp = f.id_emp and f.tipomov = 'NC' ) " _
            & " left join jsconctaemp g on (a.id_emp = g.id_emp) " _
            & " Where " _
            & str _
            & " substring(f.refer,1,5) = 'ISLR-' and " _
            & " a.fecha_ret_islr >= '" & FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " a.fecha_ret_islr <= '" & FormatoFechaMySQL(FechaHasta) & "' and " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' union " _
            & " SELECT a.numgas numcom, a.EMISION emision, a.CODPRO, " _
            & " c.NOMBRE AS NOMPRO, c.RIF, c.dirprove,  '' numcredito,  f.concepto,'' fac_afectada,  a.TOT_gas tot_com, " _
            & " IF( ISNULL(d.baseiva), 0.00, d.baseiva) montoexento, " _
            & " b.TIPOIVA, f.interes PORISLR, f.capital BASEIMPONIBLE, " _
            & " f.IMPorte IMP_Islr, e.num_control, a.ret_islr retencion, a.num_ret_islr num_retencion, " _
            & " a.fecha_ret_islr fec_retencion, REPLACE(g.rif,'-','') RifAgente, CAST(DATE_FORMAT(a.emision,'%Y%m') AS UNSIGNED INTEGER) PERIODO,   " _
            & " REPLACE(c.rif, '-','') RifRetenido, SUBSTRING(f.concepto,1,3) CodigoConcepto,  " _
            & " a.base_ret_islr MontoOperacion, a.por_ret_islr PorcentajeRetencion " _
            & " FROM jsproencgas a " _
            & " LEFT JOIN jsproivagas b ON (a.numgas = b.numgas AND a.codpro = b.codpro AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp AND NOT b.tipoiva IN ('','E') ) " _
            & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsproivagas d ON (a.numgas = d.numgas AND a.codpro = d.codpro AND a.id_emp = d.id_emp AND d.tipoiva = '' ) " _
            & " LEFT JOIN jsconnumcon e ON (a.numgas = e.numdoc AND a.codpro = e.prov_cli AND a.id_emp = e.id_emp AND e.org = 'GAS' AND e.origen = 'COM') " _
            & " left join jsprotrapag f on (a.codpro = f.codpro and a.numgas = f.nummov and a.id_emp = f.id_emp and f.tipomov = 'NC' ) " _
            & " left join jsconctaemp g on (a.id_emp = g.id_emp) " _
            & " Where " _
            & str _
            & " substring(f.refer,1,5) = 'ISLR-' and " _
            & " a.fecha_ret_islr >= '" & FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " a.fecha_ret_islr <= '" & FormatoFechaMySQL(FechaHasta) & "' and " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by num_retencion "


    End Function

    Function SeleccionCOMPRASListadoRetencionesIVA(ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                                   ProveedorDesde As String, ProveedorHasta As String) As String

        Dim str As String = ""
        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "

        Return " SELECT a.numgas numcom, a.EMISION emision, a.CODPRO, " _
            & " c.NOMBRE AS NOMPRO, c.RIF, c.dirprove,  '' numcredito, '' fac_afectada,  a.TOT_gas tot_com, " _
            & " IF( ISNULL(d.baseiva), 0.00, d.baseiva) montoexento, " _
            & " b.TIPOIVA, b.PORIVA, b.BASEIVA, " _
            & " b.IMPIVA IMP_IVA, e.num_control, b.retencion, b.numretencion num_retencion, " _
            & " a.fecha_ret_iva fec_retencion, 'C' tipooperacion, '01' tipodocumento, '0' num_expediente, replace(c.rif, '-','') rifpro, " _
            & " CAST(DATE_FORMAT( a.fecha_ret_iva,'%Y%m') AS UNSIGNED  INTEGER) PERIODO, REPLACE(g.rif,'-','') rifemp, '0' doc_afectado " _
            & " FROM jsproencgas a " _
            & " LEFT JOIN jsproivagas b ON (a.numgas = b.numgas AND a.codpro = b.codpro AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp AND b.poriva <> 0.00 ) " _
            & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT a.numgas, a.codpro, '' tipoiva, 0.00 poriva, SUM(a.baseiva) baseiva, 0.00 impiva, a.numretencion, a.ejercicio, a.id_emp " _
               & "            FROM jsproivagas a " _
               & "            WHERE " _
               & "            a.poriva = 0.00 AND " _
               & "            a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.numgas, a.codpro) d ON (a.numgas = d.numgas AND a.codpro = d.codpro AND a.ejercicio = d.ejercicio AND a.id_emp = d.id_emp AND d.poriva = 0.00 ) " _
            & " LEFT JOIN jsconnumcon e ON (a.numgas = e.numdoc and a.codpro = e.prov_cli and e.num_control <> '' AND a.id_emp = e.id_emp AND e.org = 'GAS' AND e.origen = 'COM') " _
            & " LEFT JOIN jsconctaemp g ON (a.id_emp = g.id_emp) " _
            & " Where " _
            & str _
            & " b.retencion <> 0.00 and " _
            & " b.numretencion <> '' and " _
            & " b.numretencion is not null and " _
            & " a.fecha_ret_iva >= '" & FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " a.fecha_ret_iva <= '" & FormatoFechaMySQL(FechaHasta) & "' and " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & "                        UNION " _
            & "  SELECT a.numncr numcom, a.EMISION emision, a.CODPRO, " _
            & " c.NOMBRE AS NOMPRO, c.RIF, c.dirprove,  a.NUMNCR numcredito, '' fac_afectada,  -1*a.TOT_ncr tot_com, " _
            & " -1*IF( ISNULL(d.baseiva), 0.00, d.baseiva) montoexento, " _
            & " b.TIPOIVA, b.PORIVA, -1*b.BASEIVA, " _
            & " -1*b.IMPIVA IMP_IVA, e.num_control, b.retencion, b.numretencion num_retencion, " _
            & " f.Emision fec_retencion, 'C' tipooperacion, '03' tipodocumento, '0' num_expediente, replace(c.rif, '-','') rifpro, " _
            & " CAST(DATE_FORMAT(f.Emision,'%Y%m') AS UNSIGNED  INTEGER) PERIODO, REPLACE(g.rif,'-','') rifemp, 'XXXXXXXXX' doc_afectado " _
            & " FROM jsproencncr a " _
            & " LEFT JOIN jsprotrapag f ON (a.numncr = f.nummov AND a.codpro = f.codpro AND a.id_emp = f.id_emp AND f.tipomov = 'ND') " _
            & " LEFT JOIN jsproivancr b ON (a.numncr = b.numncr AND a.codpro = b.codpro AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp AND b.poriva <> 0.00 ) " _
            & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT a.numncr, a.codpro, '' tipoiva, 0.00 poriva, SUM(a.baseiva) baseiva, 0.00 impiva, a.numretencion, a.ejercicio, a.id_emp " _
               & "            FROM jsproivancr a " _
               & "            WHERE " _
               & "            a.poriva = 0.00 AND " _
               & "            a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.numncr, a.codpro) d ON (a.numncr = d.numncr AND a.codpro = d.codpro AND a.ejercicio = d.ejercicio AND a.id_emp = d.id_emp AND d.poriva = 0.00 ) " _
            & " LEFT JOIN jsconnumcon e ON (a.numncr = e.numdoc and a.codpro = e.prov_cli AND e.num_control <> '' AND a.id_emp = e.id_emp AND e.org = 'NCR' AND e.origen = 'COM') " _
            & " LEFT JOIN jsconctaemp g ON (a.id_emp = g.id_emp) " _
            & " Where " _
            & str _
            & " b.retencion <> 0.00 and " _
            & " b.numretencion <> '' and " _
            & " b.numretencion is not null and " _
            & " MID(f.concepto,1,13) = 'RETENCION IVA' AND " _
            & " f.emision >= '" & FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " f.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & "                          UNION " _
            & " SELECT a.numndb numcom, a.EMISION emision, a.CODPRO, " _
            & " c.NOMBRE AS NOMPRO, c.RIF, c.dirprove,  a.NUMndb numcredito, '' fac_afectada,  -1*a.TOT_ndb tot_com, " _
            & " -1*IF( ISNULL(d.baseiva), 0.00, d.baseiva) montoexento, " _
            & " b.TIPOIVA, b.PORIVA, -1*b.BASEIVA, " _
            & " -1*b.IMPIVA IMP_IVA, e.num_control, b.retencion, b.numretencion num_retencion, " _
            & " f.Emision fec_retencion, 'C' tipooperacion, '03' tipodocumento, '0' num_expediente, replace(c.rif, '-','') rifpro, " _
            & " CAST(DATE_FORMAT(f.Emision,'%Y%m') AS UNSIGNED INTEGER) PERIODO, REPLACE(g.rif,'-','') rifemp, 'XXXXXXXXX' doc_afectado " _
            & " FROM jsproencndb a " _
            & " LEFT JOIN jsprotrapag f ON (a.numndb = f.nummov AND a.codpro = f.codpro AND a.id_emp = f.id_emp AND f.tipomov = 'NC') " _
            & " LEFT JOIN jsproivandb b ON (a.numndb = b.numndb AND a.codpro = b.codpro AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp AND b.poriva <> 0.00 ) " _
            & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN (SELECT a.numndb, a.codpro, '' tipoiva, 0.00 poriva, SUM(a.baseiva) baseiva, 0.00 impiva, a.numretencion, a.ejercicio, a.id_emp " _
               & "            FROM jsproivandb a " _
               & "            WHERE " _
               & "            a.poriva = 0.00 AND " _
               & "            a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.numndb, a.codpro) d ON (a.numndb = d.numndb AND a.codpro = d.codpro AND a.ejercicio = d.ejercicio AND a.id_emp = d.id_emp AND d.poriva = 0.00 ) " _
            & " LEFT JOIN jsconnumcon e ON (a.numndb = e.numdoc and a.codpro = e.prov_cli AND e.num_control <> '' AND a.id_emp = e.id_emp AND e.org = 'NDB' AND e.origen = 'COM') " _
            & " LEFT JOIN jsconctaemp g ON (a.id_emp = g.id_emp) " _
            & " Where " _
            & str _
            & " b.retencion <> 0.00 and " _
            & " b.numretencion <> '' and " _
            & " b.numretencion is not null and " _
            & " MID(f.concepto,1,13) = 'RETENCION IVA' AND " _
            & " f.emision >= '" & FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " f.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & "                    UNION " _
            & " SELECT a.NUMCOM, a.EMISION emision, a.CODPRO, " _
            & " c.NOMBRE AS NOMPRO, c.RIF, c.dirfiscal,  '' numcredito, '' fac_afectada,  a.TOT_COM, " _
            & " IF( ISNULL(d.baseiva), 0.00, d.baseiva) montoexento, " _
            & " b.TIPOIVA, b.PORIVA, b.BASEIVA, " _
            & " b.IMPIVA IMP_IVA, e.num_control, b.retencion, b.numretencion num_retencion, " _
            & " a.fecha_ret_iva fec_retencion, 'C' tipooperacion, '01' tipodocumento, '0' num_expediente, replace(c.rif, '-','') rifpro, " _
            & " CAST(DATE_FORMAT(a.fecha_ret_iva,'%Y%m') AS UNSIGNED  INTEGER) PERIODO, REPLACE(g.rif,'-','') rifemp, '0' doc_afectado " _
            & " FROM jsproenccom a " _
            & " LEFT JOIN jsproivacom b ON (a.numcom = b.numcom AND a.codpro = b.codpro AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp AND b.poriva <> 0.00 ) " _
            & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsconnumcon e ON (a.numcom = e.numdoc and a.codpro = e.prov_cli AND e.num_control <> '' and a.id_emp = e.id_emp AND e.org = 'COM' AND e.origen = 'COM') " _
            & " LEFT JOIN (SELECT a.numcom, a.codpro, '' tipoiva, 0.00 poriva, SUM(a.baseiva) baseiva, 0.00 impiva, a.numretencion, a.ejercicio, a.id_emp " _
               & "            FROM jsproivacom a " _
               & "            WHERE " _
               & "            a.poriva = 0.00 AND " _
               & "            a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.numcom, a.codpro) d ON (a.numcom = d.numcom AND a.codpro = d.codpro AND a.ejercicio = d.ejercicio AND a.id_emp = d.id_emp AND d.poriva = 0.00 ) " _
            & " LEFT JOIN jsconctaemp g ON (a.id_emp = g.id_emp) " _
            & " Where " _
            & str _
            & " b.retencion <> 0.00 and " _
            & " b.numretencion <> '' and " _
            & " b.numretencion is not null and " _
            & " a.fecha_ret_iva >= '" & FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " a.fecha_ret_iva <= '" & FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by 17 "


    End Function
    Public Function SeleccionCOMPRASMovimientosProveedores(ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, ByVal OrdenadoPor As String, ByVal operador As Integer, _
                                              FEchaDesde As Date, ByVal FechaHasta As Date, TipoDocumentos As String, _
                                              Optional ByVal TipoProveedor As Integer = 0, Optional ByVal EstatusProveedor As Integer = 0, _
                                              Optional CxP_ExP As Integer = 0) As String


        Dim str As String = ""

        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "
        If TipoProveedor < 2 Then str += " a.tipo = " & TipoProveedor & " and "
        If EstatusProveedor < 2 Then str += " a.estatus = " & EstatusProveedor & " and "
        str += " LOCATE(e.tipomov, '" & TipoDocumentos & "') > 0 and "

        SeleccionCOMPRASMovimientosProveedores = " select a.codpro, a.nombre, a.asignado, a.ingreso, a.rif, " _
            & " e.nummov, e.tipomov, e.emision, e.hora, e.vence, e.refer, e.concepto, e.importe, e.origen, e.numorg " _
            & " from jsprocatpro a " _
            & " left join jsprotrapag e on (a.codpro = e.codpro and a.id_emp = e.id_emp ) " _
            & " Where " _
            & " e.emision >= '" & FormatoFechaMySQL(FEchaDesde) & "' and " _
            & " e.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' and " _
            & IIf(CxP_ExP = 0, " e.REMESA = '' AND ", " e.REMESA = '1' AND ") _
            & str _
            & " a.id_emp ='" & jytsistema.WorkID & "' " _
            & " order by a." & OrdenadoPor & ", e.emision "

    End Function

    Function SeleccionCOMPRASAuditoriaProveedores(ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                      FechaHasta As Date, Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                              Optional ByVal UnidadDesde As String = "", Optional ByVal UnidadHasta As String = "", _
                                              Optional ByVal TipoProveedor As Integer = 0, Optional ByVal EstatusProveedor As Integer = 0, _
                                              Optional CxP_ExP As Integer = 0) As String


        Dim str As String = ""

        If CategoriaDesde <> "" Then str += " c.categoria >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then str += " c.categoria <= '" & CategoriaHasta & "' and "
        If UnidadDesde <> "" Then str += " c.unidad >= '" & UnidadDesde & "' and "
        If UnidadHasta <> "" Then str += " c.unidad <= '" & UnidadHasta & "' and "
        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "
        If TipoProveedor < 2 Then str += " c.tipo = " & TipoProveedor & " and "
        If EstatusProveedor < 2 Then str += " c.estatus = " & EstatusProveedor & " and "

        SeleccionCOMPRASAuditoriaProveedores = " select a.codpro, c.asignado, c.nombre,  c.rif, a.nummov, a.importe, a.saldo, " _
                & " b.tipomov, b.emision, b.vence, b.refer, b.concepto, if( b.importe < 0, abs(b.importe), 0.00) debitos, " _
                & " if( b.importe >= 0, abs(b.importe), 0.00) creditos, b.formapag " _
                & " from ( select a.codpro,  a.nummov, a.tipomov, a.importe, b.saldo " _
                & "         from jsprotrapag a " _
                & "         left join ( select a.codpro, a.nummov, sum(a.importe) saldo " _
                & "                     from jsprotrapag a " _
                & "                     Where " _
                & IIf(CxP_ExP = 0, " a.REMESA = '' AND ", " a.REMESA = '1' AND ") _
                & "                     a.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' and " _
                & "                     a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & "                     a.ID_EMP = '" & jytsistema.WorkID & "' " _
                & "                     group by a.codpro, a.nummov " _
                & "                     having round(saldo,0) <> 0 ) b on (a.codpro = b.codpro and a.nummov = b.nummov) " _
                & "         where " _
                & IIf(CxP_ExP = 0, " a.REMESA = '' AND ", " a.REMESA = '1' AND ") _
                & "         CONCAT( a.nummov, a.emision, a.hora) IN ( SELECT MIN(CONCAT(a.nummov, a.emision, a.hora)) FROM jsprotrapag WHERE a.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' AND a.ejercicio = '" & jytsistema.WorkExercise & "' AND a.id_emp = '" & jytsistema.WorkID & "')  AND " _
                & "         a.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' AND " _
                & "         a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                & "         a.id_emp = '" & jytsistema.WorkID & "' and " _
                & "         b.saldo is not null " _
                & "         group by a.codpro, a.nummov having b.saldo <> 0.00  ) a " _
                & " left join jsprotrapag b on (a.codpro = b.codpro and a.nummov = b.nummov) " _
                & " left join jsprocatpro c on (a.codpro = c.codpro and b.id_emp = c.id_emp) " _
                & " left join jsvenrenrut d on (a.codpro = d.cliente and b.id_emp = d.id_emp and d.tipo = '0') " _
                & " left join jsvenencrut e on (d.codrut = e.codrut and d.id_emp = e.id_emp and d.tipo = e.tipo  ) " _
                & " where " _
                & IIf(CxP_ExP = 0, " b.REMESA = '' AND ", " b.REMESA = '1' AND ") _
                & str _
                & " b.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' and " _
                & " b.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " b.id_emp = '" & jytsistema.WorkID & "' " _
                & " order by c." & OrdenadoPor _
                & " "


    End Function
    Function SeleccionCOMPRASVencimientos(ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                      FechaHasta As Date, Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                                Optional ByVal UnidadDesde As String = "", Optional ByVal UnidadHasta As String = "", _
                                                Optional ByVal TipoProveedor As Integer = 0, Optional ByVal EstatusProveedor As Integer = 0, _
                                                Optional Lapso1Desde As Integer = 1, Optional lapso1Hasta As Integer = 8, _
                                                Optional Lapso2Desde As Integer = 9, Optional lapso2Hasta As Integer = 15, _
                                                Optional Lapso3Desde As Integer = 16, Optional lapso3Hasta As Integer = 30, _
                                                Optional Lapso4Desde As Integer = 31, _
                                                Optional CxP_ExP As Integer = 0) As String


        Dim str As String = ""

        If CategoriaDesde <> "" Then str += " b.categoria >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then str += " b.categoria <= '" & CategoriaHasta & "' and "
        If UnidadDesde <> "" Then str += " b.unidad >= '" & UnidadDesde & "' and "
        If UnidadHasta <> "" Then str += " b.unidad <= '" & UnidadHasta & "' and "
        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "
        If TipoProveedor < 2 Then str += " b.tipo = " & TipoProveedor & " and "
        If EstatusProveedor < 2 Then str += " b.estatus = " & EstatusProveedor & " and "

        SeleccionCOMPRASVencimientos = "SELECT a.codpro, b.nombre, a.nummov, a.tipomov, a.refer, a.EMISION, a.VENCE, a.importe, " _
            & " min(concat(a.EMISION,a.HORA)) GRUPO, " _
            & " SUM(a.importe) saldo, to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.vence) as DV, to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision) as DE, " _
            & " if( to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision)>= " & Lapso1Desde & " and to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision)<= " & lapso1Hasta & ", '1. Vencimientos de " & Lapso1Desde & " a " & lapso1Hasta & " días'  , " _
            & " if( to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision)>= " & Lapso2Desde & " and to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision)<= " & lapso2Hasta & ", '2. Vencimientos de " & Lapso2Desde & " a " & lapso2Hasta & " días'  , " _
            & " if( to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision)>= " & Lapso3Desde & " and to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision)<= " & lapso3Hasta & ", '3. Vencimientos de " & Lapso3Desde & " a " & lapso3Hasta & " días'  , " _
            & " if( to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision)>= " & Lapso4Desde & ", '4. Vencimientos de " & Lapso4Desde & " días o más' , '0' " _
            & " )))) as lapso " _
            & " from jsprotrapag a " _
            & " LEFT JOIN jsprocatpro b ON ( b.codpro = a.codpro AND b.ID_EMP = a.ID_EMP) " _
            & " Where " _
            & str _
            & IIf(CxP_ExP = 0, " a.REMESA = '' AND ", " a.REMESA = '1' AND ") _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' " _
            & " GROUP BY a.codpro, a.nummov Having (Saldo > 0.001 Or Saldo < -0.001) and lapso <> '0' and dv > 0 " _
            & " ORDER BY LAPSO, a.codpro, NUMMOV, EMISION ASC "


    End Function
    Function SeleccionCOMPRASVencimientosResumen(ByVal ProveedorDesde As String, ByVal ProveedorHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                      FechaHasta As Date, Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                                Optional ByVal UnidadDesde As String = "", Optional ByVal UnidadHasta As String = "", _
                                                Optional ByVal TipoProveedor As Integer = 0, Optional ByVal EstatusProveedor As Integer = 0, _
                                                Optional Lapso1Desde As Integer = 1, Optional lapso1Hasta As Integer = 8, _
                                                Optional Lapso2Desde As Integer = 9, Optional lapso2Hasta As Integer = 15, _
                                                Optional Lapso3Desde As Integer = 16, Optional lapso3Hasta As Integer = 30, _
                                                Optional Lapso4Desde As Integer = 31, _
                                                Optional CxP_ExP As Integer = 0) As String


        Dim str As String = ""

        If CategoriaDesde <> "" Then str += " a.categoria >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then str += " a.categoria <= '" & CategoriaHasta & "' and "
        If UnidadDesde <> "" Then str += " a.unidad >= '" & UnidadDesde & "' and "
        If UnidadHasta <> "" Then str += " a.unidad <= '" & UnidadHasta & "' and "
        If ProveedorDesde <> "" Then str += " a.codpro >= '" & ProveedorDesde & "' and "
        If ProveedorHasta <> "" Then str += " a.codpro <= '" & ProveedorHasta & "' and "
        If TipoProveedor < 2 Then str += " a.tipo = " & TipoProveedor & " and "
        If EstatusProveedor < 2 Then str += " a.estatus = " & EstatusProveedor & " and "

        SeleccionCOMPRASVencimientosResumen = "select a.codpro, a.nombre, " _
        & " Sum(if(b.lapso = '0',b.saldo,0)) saldo0, " _
        & " Sum(if(b.lapso = '1',b.saldo,0)) saldo1, " _
        & " Sum(if(b.lapso = '2',b.saldo,0)) saldo2, " _
        & " Sum(if(b.lapso = '3',b.saldo,0)) saldo3, " _
        & " Sum(if(b.lapso = '4',b.saldo,0)) saldo4 " _
        & " from jsprocatpro a " _
        & " left join (SELECT a.codpro, a.NUMMOV, SUM(a.importe) AS SALDO, " _
        & "             to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.vence) as DV, " _
        & "             to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision) as DE, " _
        & "             if( to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision)>= 1 and to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision)<= 8, '1' , " _
        & "             if( to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision)>= 9 and to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision)<= 15, '2' , " _
        & "             if( to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision)>= 16 and to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision)<= 30, '3' , " _
        & "             if( to_days('" & FormatoFechaMySQL(FechaHasta) & "') - to_Days(a.emision)>= 31, '4' , '0' " _
        & "             )))) as lapso from jsprotrapag a " _
        & "             Where " _
        & IIf(CxP_ExP = 0, " a.REMESA = '' AND ", " a.REMESA = '1' AND ") _
        & "             a.ID_EMP = '" & jytsistema.WorkID & "' " _
        & "             GROUP BY a.codpro, a.nummov " _
        & "             Having (Saldo > 0.001 Or Saldo < -0.001) and lapso <> '0' and dv > 0 ) b on (a.codpro = b.codpro) " _
        & " Where " _
        & str _
        & " a.id_emp = '" & jytsistema.WorkID & "' " _
        & " group by a.codpro " _
        & " having (saldo0+saldo1+saldo2+saldo3+saldo4) <> 0  "


    End Function

    Function SeleccionCOMPRASLibroIVA(MyConn As MySqlConnection, lblInfo As Label, _
                                    ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                    Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                    Optional ByVal UnidadDesde As String = "", Optional ByVal UnidadHasta As String = "", _
                                    Optional ByVal TipoProveedor As Integer = 0) As String

        Dim str As String = ""

        If CategoriaDesde <> "" Then str += " a.categoria >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then str += " a.categoria <= '" & CategoriaHasta & "' and "
        If UnidadDesde <> "" Then str += " a.unidad >= '" & UnidadDesde & "' and "
        If UnidadHasta <> "" Then str += " a.unidad <= '" & UnidadHasta & "' and "
        If TipoProveedor < 2 Then str += " a.tipo = " & TipoProveedor & " and "

        Dim tblTemp As String = "tblIVACompras"
        Dim aFld() As String = {"emision.fecha", "rif.cadena20", _
                    "nompro.cadena250", "numcom.cadena20", "num_control.cadena20", "numSerie.cadena5", "numdebito.cadena20", "numcredito.cadena20", _
                    "tipo.cadena2", "fac_afectada.cadena20", "controladorregistro.cadena150", "tot_imp.doble19", "imp_nogravado.doble19", "baseivaimp.doble19", _
                    "porivaimp.doble6", "impivaimp.doble19", "tipoiva.cadena", "tot_com.doble19", "nogravado.doble19", "baseiva.doble19", _
                    "poriva.doble6", "impiva.doble19", "retencion.doble19", "num_retencion.cadena20", "fec_retencion.fecha", "tipoproveedor.cadena2", _
                    "planillaimportacion.cadena20", "expedienteimportacion.cadena20", "retenciontercero.doble19", "anticipoiva.doble19"}

        EjecutarSTRSQL(MyConn, lblInfo, " drop  temporary table if exists " & tblTemp)
        CrearTabla(MyConn, lblInfo, jytsistema.WorkDataBase, True, tblTemp, aFld)

        'FACTURAS DE COMPRAS
        EjecutarSTRSQL_Scalar(MyConn, lblInfo, "insert into  " & tblTemp & _
            " select a.emision, c.rif, c.nombre nompro, a.numcom, " _
            & " e.num_control, a.serie_numcom,  '' numdebito, '' numcredito, '01' tipo,  '' fac_afectada, concat(a.numcom , e.num_control, '', '', '', '') controladorregistro, " _
            & " 0.00, 0.00, 0.00, 0.00, 0.00, if( b.tipoiva is null , '', b.tipoiva) tipoiva , a.tot_com, if( isnull(d.baseiva), 0.00, d.baseiva) Nogravado,  " _
            & " if( b.baseiva is null, 0.00, b.baseiva), if(b.poriva is null, 0.00, b.poriva), if(b.impiva is null, 0.00, b.impiva), 0.00, '', a.emisioniva fec_retencion,  " _
            & " '', '', '', 0.00, 0.00 " _
            & " FROM jsproenccom a " _
            & " left join jsproivacom b on (a.numcom = b.numcom and a.codpro = b.codpro and a.ejercicio = b.ejercicio and a.id_emp = b.id_emp and b.tipoiva <> '' )" _
            & " left join jsprocatpro c on (a.codpro = c.codpro And a.id_emp = c.id_emp) " _
            & " left join jsproivacom d on (a.numcom = d.numcom and a.codpro = d.codpro and a.id_emp = d.id_emp and d.tipoiva = '' ) " _
            & " left join jsconnumcon e on (a.numcom = e.numdoc and a.codpro = e.prov_cli and e.num_control <> '' and a.id_emp = e.id_emp and e.org = 'COM' and e.origen = 'COM') " _
            & " Where " _
            & " (e.num_control <> '' or not e.num_control is null) and " _
            & " a.emisioniva >= '" & FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " a.emisioniva <= '" & FormatoFechaMySQL(FechaHasta) & "' AND " _
            & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' ")

        'GASTOS
        EjecutarSTRSQL(MyConn, lblInfo, "insert into  " & tblTemp & _
             " select a.emision, c.rif, c.nombre nompro, a.numgas, " _
                 & " e.num_control, a.serie_numgas, '' numdebito, '' numcredito, '01' tipo,  '' fac_afectada, concat(a.numgas , e.num_control, '', '', '', '') controladorregistro, " _
                 & " 0.00, 0.00, 0.00, 0.00, 0.00, if( b.tipoiva is null , '', b.tipoiva) tipoiva, a.tot_gas, if( isnull(d.baseiva), 0.00, d.baseiva) Nogravado,  " _
                 & " if( b.baseiva is null, 0.00, b.baseiva), if(b.poriva is null, 0.00, b.poriva), if(b.impiva is null, 0.00, b.impiva), 0.00, '', a.emisioniva fec_retencion ,  " _
                 & " '', '', '', 0.00, 0.00 " _
                 & " FROM jsproencgas a " _
                 & " left join jsproivagas b on (a.numgas = b.numgas and a.codpro = b.codpro and a.ejercicio = b.ejercicio and a.id_emp = b.id_emp and b.tipoiva <> '' )" _
                 & " left join jsprocatpro c on (a.codpro = c.codpro And a.id_emp = c.id_emp) " _
                 & " left join jsproivagas d on (a.numgas = d.numgas and a.codpro = d.codpro and a.id_emp = d.id_emp and d.tipoiva = '' ) " _
                 & " left join jsconnumcon e on (a.numgas = e.numdoc and a.codpro = e.prov_cli and e.num_control <> '' and a.id_emp = e.id_emp and e.org = 'GAS' and e.origen = 'COM') " _
                 & " Where " _
                 & " (e.num_control <> '' or not e.num_control is null) and " _
                 & " a.emisioniva >= '" & FormatoFechaMySQL(FechaDesde) & "' AND " _
                 & " a.emisioniva <= '" & FormatoFechaMySQL(FechaHasta) & "' AND " _
                 & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                 & " a.id_emp = '" & jytsistema.WorkID & "' ")


        'RETENCIONES IVA FACTURAS
        EjecutarSTRSQL(MyConn, lblInfo, "insert into  " & tblTemp & _
            " select a.emision, c.rif, c.nombre nompro, '', " _
                & " '' num_control, '' numSerie, '' numdebito, '' numcredito, '01' tipo,  a.nummov fac_afectada, concat('', '', '', '', a.refer, a.nummov) controladorregistro, " _
                & " 0.00, 0.00, 0.00, 0.00, 0.00, '', 0.00, 0.00,  " _
                & " 0.00, 0.00, 0.00, a.importe, a.refer, a.emision fec_retencion,  " _
                & " '', '', '', 0.00, 0.00 " _
                & " FROM jsprotrapag a " _
                & " left join jsprocatpro c on (a.codpro = c.codpro And a.id_emp = c.id_emp) " _
                & " Where " _
                & " substring(a.concepto,1,30) = 'RETENCION IVA S/COMPROBANTE Nº' and " _
                & " a.emision >= '" & FormatoFechaMySQL(FechaDesde) & "' AND " _
                & " a.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' AND " _
                & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' ")


        'NOTAS DE CREDITO
        EjecutarSTRSQL(MyConn, lblInfo, "insert into  " & tblTemp & _
            " select a.emision, c.rif, c.nombre nompro, '', " _
                & " e.num_control, a.serie_numncr, '' numdebito, a.numncr, '01' tipo, if( f.numcom = '' or f.numcom is null, a.numcom, f.numcom) fac_afectada, " _
                & " concat('' , e.num_control, '', a.numncr, if( f.numcom = '' or f.numcom is null, a.numcom, f.numcom),  '') controladorregistro, " _
                & " 0.00, 0.00, 0.00, 0.00, 0.00, if( b.tipoiva is null , '', b.tipoiva) tipoiva, -1*a.tot_ncr, -1*if( isnull(d.baseiva), 0.00, d.baseiva) Nogravado,  " _
                & " -1*if( b.baseiva is null, 0.00, b.baseiva), if(b.poriva is null, 0.00, b.poriva), -1*if(b.impiva is null, 0.00, b.impiva), 0.00, '', a.emisioniva fec_retencion ,  " _
                & " '', '', '', 0.00, 0.00 " _
                & " FROM jsproencncr a " _
                & " left join jsproivancr b on (a.numncr = b.numncr and a.codpro = b.codpro and a.ejercicio = b.ejercicio and a.id_emp = b.id_emp and b.tipoiva <> '' )" _
                & " left join jsprocatpro c on (a.codpro = c.codpro And a.id_emp = c.id_emp) " _
                & " left join jsproivancr d on (a.numncr = d.numncr and a.codpro = d.codpro and a.id_emp = d.id_emp and d.tipoiva = '' ) " _
                & " left join jsconnumcon e on (a.numncr = e.numdoc and a.codpro = e.prov_cli and e.num_control <> '' and a.id_emp = e.id_emp and e.org = 'NCR' and e.origen = 'COM') " _
                & " left join jsprorenncr f on (a.numncr = f.numncr and a.id_emp = f.id_emp and f.renglon = '00001') " _
                & " Where " _
                & " (e.num_control <> '' or not e.num_control is null) and " _
                & " a.emisioniva >= '" & FormatoFechaMySQL(FechaDesde) & "' AND " _
                & " a.emisioniva <= '" & FormatoFechaMySQL(FechaHasta) & "' AND " _
                & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' ")

        'NOTAS DEBITO
        EjecutarSTRSQL(MyConn, lblInfo, " insert into  " & tblTemp & _
            " select a.emision, c.rif, c.nombre nompro, '', " _
                & " e.num_control, a.serie_numndb, a.numndb, '', '01' tipo, if( f.numcom = '', a.numcom, f.numcom) fac_afectada, " _
                & " concat('' , e.num_control, a.numndb, '', if( f.numcom = '', a.numcom, f.numcom),  '') controladorregistro, " _
                & " 0.00, 0.00, 0.00, 0.00, 0.00, if( b.tipoiva is null , '', b.tipoiva) tipoiva, a.tot_ndb, if( isnull(d.baseiva), 0.00, d.baseiva) Nogravado,  " _
                & " if( b.baseiva is null, 0.00, b.baseiva), if(b.poriva is null, 0.00, b.poriva), if(b.impiva is null, 0.00, b.impiva), 0.00, '', a.emisioniva fec_retencion ,  " _
                & " '', '', '', 0.00, 0.00 " _
                & " FROM jsproencndb a " _
                & " left join jsproivandb b on (a.numndb = b.numndb and a.codpro = b.codpro and a.ejercicio = b.ejercicio and a.id_emp = b.id_emp and b.tipoiva <> '' )" _
                & " left join jsprocatpro c on (a.codpro = c.codpro And a.id_emp = c.id_emp) " _
                & " left join jsproivandb d on (a.numndb = d.numndb and a.codpro = d.codpro and a.id_emp = d.id_emp and d.tipoiva = '' ) " _
                & " left join jsconnumcon e on (a.numndb = e.numdoc and a.codpro = e.prov_cli and e.num_control <> '' and a.id_emp = e.id_emp and e.org = 'NDB' and e.origen = 'COM') " _
                & " left join jsprorenndb f on (a.numndb = f.numndb and a.id_emp = f.id_emp and f.renglon = '00001') " _
                & " Where " _
                & " (e.num_control <> '' or not e.num_control is null) and " _
                & " a.emision >= '" & FormatoFechaMySQL(FechaDesde) & "' AND " _
                & " a.emision <= '" & FormatoFechaMySQL(FechaHasta) & "' AND " _
                & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' ")

        '//////////////////////////////////////////////////////////
        SeleccionCOMPRASLibroIVA = " select * from " & tblTemp & _
            " order by emision, controladorregistro "

    End Function

    Function SeleccionCOMPRASRetencionIVA(numeroRetencion As String) As String

        Dim str As String


        str = " SELECT a.numgas numcom, a.EMISION emision, a.CODPRO, " _
                & " c.NOMBRE AS NOMPRO, c.RIF, c.dirfiscal, a.serie_numgas numSerie,  '' numcredito, '' numdebito, '' fac_afectada,  a.TOT_gas tot_com, " _
                & " IF( ISNULL(d.baseiva), 0.00, d.baseiva) montoexento, " _
                & " b.TIPOIVA, b.PORIVA, b.BASEIVA, " _
                & " b.IMPIVA IMP_IVA, e.num_control,  b.retencion, a.num_ret_iva num_retencion, " _
                & " a.fecha_ret_iva fec_retencion, a.ret_iva " _
                & " FROM jsproencgas a " _
                & " LEFT JOIN jsproivagas b ON (a.numgas = b.numgas AND a.codpro = b.codpro AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp AND b.poriva <> 0.00 ) " _
                & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
                & " LEFT JOIN (SELECT a.numgas, a.codpro, '' tipoiva, 0.00 poriva, SUM(a.baseiva) baseiva, 0.00 impiva, a.numretencion, a.ejercicio, a.id_emp " _
                & "            FROM jsproivagas a " _
                & "            WHERE " _
                & "            a.poriva = 0.00 AND " _
                & "            a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.numgas, a.codpro) d ON (a.numgas = d.numgas AND a.codpro = d.codpro AND a.id_emp = d.id_emp AND d.poriva = 0.00 ) " _
                & " LEFT JOIN jsconnumcon e ON (a.numgas = e.numdoc and a.codpro = e.prov_cli AND e.num_control <> '' AND a.id_emp = e.id_emp AND e.org = 'GAS' AND e.origen = 'COM') " _
                & " LEFT JOIN jsprotrapag f ON (a.numgas = f.nummov AND a.codpro = f.codpro AND a.id_emp = f.id_emp AND f.tipomov = 'NC') " _
                & " Where " _
                & " f.refer = '" & numeroRetencion & "' AND " _
                & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' "

        str = str & " union SELECT '' numcom, a.EMISION emision, a.CODPRO, " _
                & " c.NOMBRE AS NOMPRO, c.RIF, c.dirfiscal, a.serie_numncr numSerie, a.numncr numcredito, '' numdebito, a.numcom fac_afectada,  a.TOT_ncr tot_com, " _
                & " IF( ISNULL(d.baseiva), 0.00, d.baseiva) montoexento, " _
                & " b.TIPOIVA, b.PORIVA, -1*b.BASEIVA, " _
                & " -1*b.IMPIVA IMP_IVA, e.num_control,  b.retencion, f.refer num_retencion, " _
                & " f.emision fec_retencion, f.importe ret_iva " _
                & " FROM jsproencncr a " _
                & " LEFT JOIN jsproivancr b ON (a.numncr = b.numncr AND a.codpro = b.codpro AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp AND b.poriva <> 0.00 ) " _
                & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
                & " LEFT JOIN (SELECT a.numncr, a.codpro, '' tipoiva, 0.00 poriva, SUM(a.baseiva) baseiva, 0.00 impiva, a.numretencion, a.ejercicio, a.id_emp " _
                & "            FROM jsproivancr a " _
                & "            WHERE " _
                & "            a.poriva = 0.00 AND " _
                & "            a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.numncr, a.codpro) d ON (a.numncr = d.numncr AND a.codpro = d.codpro AND a.id_emp = d.id_emp AND d.poriva = 0.00 ) " _
                & " LEFT JOIN jsconnumcon e ON (a.numncr = e.numdoc AND a.codpro = e.prov_cli AND e.num_control <> '' AND a.id_emp = e.id_emp AND e.org = 'NCR' AND e.origen = 'COM') " _
                & " LEFT JOIN jsprotrapag f ON (a.numncr = f.nummov AND a.codpro = f.codpro AND a.id_emp = f.id_emp AND f.tipomov = 'ND' ) " _
                & " Where " _
                & " f.refer = '" & numeroRetencion & "' AND " _
                & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' "

        str = str & " union SELECT '' numcom, a.EMISION emision, a.CODPRO, " _
                & " c.NOMBRE NOMPRO, c.RIF, c.dirfiscal, a.serie_numndb numSerie, '' numcredito, a.numndb numdebito, a.numcom fac_afectada, a.tot_ndb tot_com, " _
                & " IF( ISNULL(d.baseiva), 0.00, d.baseiva) montoexento, " _
                & " b.TIPOIVA, b.PORIVA, -1*b.BASEIVA, " _
                & " -1*b.IMPIVA IMP_IVA, e.num_control,  b.retencion, f.refer num_retencion, " _
                & " f.emision fec_retencion, f.importe ret_iva " _
                & " FROM jsproencndb a " _
                & " LEFT JOIN jsproivandb b ON (a.numndb = b.numndb AND a.codpro = b.codpro AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp AND b.poriva <> 0.00 ) " _
                & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
                & " LEFT JOIN (SELECT a.numndb, a.codpro, '' tipoiva, 0.00 poriva, SUM(a.baseiva) baseiva, 0.00 impiva, a.numretencion, a.ejercicio, a.id_emp " _
                & "            FROM jsproivandb a " _
                & "            WHERE " _
                & "            a.poriva = 0.00 AND " _
                & "            a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.numndb, a.codpro) d ON (a.numndb = d.numndb AND a.codpro = d.codpro AND a.id_emp = d.id_emp AND d.poriva = 0.00 ) " _
                & " LEFT JOIN jsconnumcon e ON (a.numndb = e.numdoc AND a.codpro = e.prov_cli AND e.num_control <> '' AND a.id_emp = e.id_emp AND e.org = 'NDB' AND e.origen = 'COM') " _
                & " LEFT JOIN jsprotrapag f ON (a.numndb = f.nummov AND a.codpro = f.codpro AND a.id_emp = f.id_emp AND f.tipomov = 'NC') " _
                & " Where " _
                & " f.refer = '" & numeroRetencion & "' AND " _
                & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' "


        SeleccionCOMPRASRetencionIVA = " SELECT a.NUMCOM, a.EMISION emision, a.CODPRO, " _
               & " c.NOMBRE AS NOMPRO, c.RIF, c.dirfiscal, a.serie_numcom numSerie, '' numcredito, '' numdebito, '' fac_afectada,  a.TOT_COM, " _
               & " IF( ISNULL(d.baseiva), 0.00, d.baseiva) montoexento, " _
               & " b.TIPOIVA, b.PORIVA, b.BASEIVA, " _
               & " b.IMPIVA IMP_IVA, e.num_control, b.retencion, a.num_ret_iva num_retencion, " _
               & " a.fecha_ret_iva fec_retencion, a.ret_iva " _
               & " FROM jsproenccom a " _
               & " LEFT JOIN jsproivacom b ON (a.numcom = b.numcom AND a.codpro = b.codpro AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp AND b.poriva <> 0.0 ) " _
               & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
               & " LEFT JOIN (SELECT a.numcom, a.codpro, '' tipoiva, 0.00 poriva, SUM(a.baseiva) baseiva, 0.00 impiva, a.numretencion, a.ejercicio, a.id_emp " _
               & "            FROM jsproivacom a " _
               & "            WHERE " _
               & "            a.poriva = 0.00 AND " _
               & "            a.id_emp = '" & jytsistema.WorkID & "' GROUP BY a.numcom, a.codpro) d ON ( a.numcom = d.numcom AND a.codpro = d.codpro AND a.id_emp = d.id_emp AND d.poriva = 0.00 ) " _
               & " LEFT JOIN jsconnumcon e ON (a.numcom = e.numdoc and a.codpro = e.prov_cli AND e.num_control <> '' AND a.id_emp = e.id_emp AND e.org = 'COM' AND e.origen = 'COM') " _
               & " LEFT JOIN jsprotrapag f ON (a.numcom = f.nummov AND a.codpro = f.codpro AND a.id_emp = f.id_emp AND f.tipomov = 'NC') " _
               & " Where " _
               & " f.refer = '" & numeroRetencion & "' AND " _
               & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
               & " a.id_emp = '" & jytsistema.WorkID & "' union " & str

    End Function

    Function SeleccionCOMPRASRetencionISLR(numeroRetencion As String, CodigoProveedor As String) As String

        Dim str As String

        str = " SELECT a.numgas numcom, a.EMISION emision, a.CODPRO, " _
                & " c.NOMBRE AS NOMPRO, c.RIF, c.dirfiscal, a.serie_numgas numSerie, '' numcredito,  f.concepto,'' fac_afectada,  a.TOT_gas tot_com, " _
                & " IF( ISNULL(d.baseiva), 0.00, d.baseiva) montoexento, " _
                & " b.TIPOIVA, a.por_ret_islr PORISLR, a.base_ret_islr BASEIMPONIBLE, " _
                & " a.por_ret_islr IMP_Islr, e.num_control, a.ret_islr retencion, a.num_ret_islr num_retencion, " _
                & " a.fecha_ret_islr fec_retencion " _
                & " FROM jsproencgas a " _
                & " LEFT JOIN jsproivagas b ON (a.numgas = b.numgas AND a.codpro = b.codpro AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp AND NOT b.tipoiva IN ('', 'E') ) " _
                & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
                & " LEFT JOIN jsproivagas d ON (a.numgas = d.numgas AND a.codpro = d.codpro AND a.id_emp = d.id_emp AND d.tipoiva = '' ) " _
                & " LEFT JOIN jsconnumcon e ON (a.numgas = e.numdoc AND a.id_emp = e.id_emp AND e.org = 'GAS' AND e.origen = 'COM') " _
                & " left join jsprotrapag f on (a.numgas = f.nummov and a.codpro = f.codpro and a.id_emp = f.id_emp and f.tipomov = 'NC' and substring(f.refer,1,5) = 'ISLR-' ) " _
                & " Where " _
                & " a.codpro = '" & CodigoProveedor & "' and " _
                & " not f.concepto is null and " _
                & " a.num_ret_islr = '" & numeroRetencion & "' AND " _
                & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' "

        SeleccionCOMPRASRetencionISLR = " SELECT a.NUMCOM, a.EMISION emision, a.CODPRO, " _
               & " c.NOMBRE AS NOMPRO, c.RIF, c.dirfiscal, a.serie_numcom numSerie, '' numcredito, f.concepto, '' fac_afectada,  a.TOT_COM, " _
               & " IF( ISNULL(d.baseiva), 0.00, d.baseiva) montoexento, " _
               & " b.TIPOIVA, a.por_ret_islr PORISLR, a.base_ret_islr BASEIMPONIBLE, " _
               & " a.por_ret_islr IMP_Islr, e.num_control, a.ret_islr retencion, a.num_ret_islr num_retencion, " _
               & " a.fecha_ret_islr fec_retencion " _
               & " FROM jsproenccom a " _
               & " LEFT JOIN jsproivacom b ON (a.numcom = b.numcom AND a.codpro = b.codpro AND a.ejercicio = b.ejercicio AND a.id_emp = b.id_emp AND NOT b.tipoiva IN ('','E') ) " _
               & " LEFT JOIN jsprocatpro c ON (a.codpro = c.codpro AND a.id_emp = c.id_emp) " _
               & " LEFT JOIN jsproivacom d ON (a.numcom = d.numcom AND a.codpro = d.codpro AND a.id_emp = d.id_emp AND d.tipoiva = '' ) " _
               & " LEFT JOIN jsconnumcon e ON (a.numcom = e.numdoc AND a.id_emp = e.id_emp AND e.org = 'COM' AND e.origen = 'COM') " _
               & " left join jsprotrapag f on (a.numcom = f.nummov and a.codpro = f.codpro and a.id_emp = f.id_emp and f.tipomov = 'NC' and substring(f.refer,1,5) = 'ISLR-' ) " _
               & " Where " _
               & " a.codpro = '" & CodigoProveedor & "' and " _
               & " not f.concepto is null and " _
               & " a.num_ret_islr = '" & numeroRetencion & "' AND " _
               & " a.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
               & " a.id_emp = '" & jytsistema.WorkID & "' union " & str


    End Function

End Module
