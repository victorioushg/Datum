Imports MySql.Data.MySqlClient
Module ConsultasMercancias
    Private ft As New Transportables
    Public Function CadenaComplementoMercancias(ByVal LetraTabla As String, ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, _
                                            ByVal Operador As Integer, ByVal ClavePrincipal As String, _
                                            Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                            Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                            Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                            Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                            Optional ByVal Estatus As Integer = 9, Optional ByVal Cartera As Integer = 9, _
                                            Optional ByVal SoloPeso As Boolean = False, Optional ByVal TipoMercancia As Integer = 99, _
                                            Optional ByVal Regulada As Integer = 9) As String

        Dim str As String = ""

        If Operador = 0 Then
            If MercanciaDesde <> "" Then str += " " & LetraTabla & ".CODART >= '" & MercanciaDesde & "' and "
            If MercanciaHasta <> "" Then str += " " & LetraTabla & ".CODART <= '" & MercanciaHasta & "' and "
        Else
            If MercanciaDesde <> "" Then str += " " & LetraTabla & ".CODART LIKE '%" & MercanciaDesde & "%' and "
            If MercanciaHasta <> "" Then str += " " & LetraTabla & ".CODART LIKE '%" & MercanciaHasta & "%' and "
        End If
        If TipoJerarquia <> "" Then str += " " & LetraTabla & ".tipjer = '" & TipoJerarquia & "' and "
        If Nivel1 <> "" Then str += " " & LetraTabla & ".codjer1 = '" & Nivel1 & "' and "
        If Nivel2 <> "" Then str += " " & LetraTabla & ".codjer2 = '" & Nivel2 & "' and "
        If Nivel3 <> "" Then str += " " & LetraTabla & ".codjer3 = '" & Nivel3 & "' and "
        If Nivel4 <> "" Then str += " " & LetraTabla & ".codjer4 = '" & Nivel4 & "' and "
        If Nivel5 <> "" Then str += " " & LetraTabla & ".codjer5 = '" & Nivel5 & "' and "
        If Nivel6 <> "" Then str += " " & LetraTabla & ".codjer6 = '" & Nivel6 & "' and "
        If CategoriaDesde <> "" Then str += " " & LetraTabla & ".grupo >= '" & CategoriaDesde & "' and "
        If CategoriaHasta <> "" Then str += " " & LetraTabla & ".grupo <= '" & CategoriaHasta & "' and "
        If MarcaDesde <> "" Then str += " " & LetraTabla & ".marca >= '" & MarcaDesde & "' and "
        If MarcaHasta <> "" Then str += " " & LetraTabla & ".marca <= '" & MarcaHasta & "' and "
        If DivisionDesde <> "" Then str += " " & LetraTabla & ".division >= '" & DivisionDesde & "' and "
        If DivisionHasta <> "" Then str += " " & LetraTabla & ".division <= '" & DivisionHasta & "' and "
        If Estatus <= 1 Then str += " " & LetraTabla & ".estatus = " & Estatus & " and "
        If Cartera <= 1 Then str += " " & LetraTabla & ".cuota = " & Cartera & " and "
        If SoloPeso Then str += " " & LetraTabla & ".unidad = 'KGR' and "
        If TipoMercancia <= 6 Then str += " " & LetraTabla & ".tipoart = " & TipoMercancia & " and "
        If Regulada = 0 Or Regulada = 1 Then
            str += " " & LetraTabla & ".regulado = " & IIf(Regulada = 0, 1, 0) & " and "
        End If


        CadenaComplementoMercancias = str

    End Function
    
    Function SeleccionMERMercancias(ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                    Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                    Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                    Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                    Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                    Optional ByVal AlmacenDesde As String = "", Optional ByVal AlmacenHasta As String = "", _
                                    Optional ByVal Estatus As Integer = 9, Optional ByVal Cartera As Integer = 9, _
                                    Optional ByVal SoloPeso As Boolean = False, Optional ByVal TipoMercancia As Integer = 99, _
                                    Optional ByVal Regulada As Integer = 9, Optional ByVal TarifaA As Boolean = True, _
                                    Optional ByVal TarifaB As Boolean = True, _
                                    Optional ByVal TarifaC As Boolean = True, _
                                    Optional ByVal TarifaD As Boolean = True, _
                                    Optional ByVal TarifaE As Boolean = True, _
                                    Optional ByVal TarifaF As Boolean = True, _
                                    Optional ByVal PreciosConIVA As Boolean = False, _
                                    Optional ByVal Existencias As Integer = 2) As String

        Dim str As String = CadenaComplementoMercancias("a", MercanciaDesde, MercanciaHasta, Operador, OrdenadoPor, TipoJerarquia, Nivel1, _
                                                         Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, CategoriaDesde, CategoriaHasta, _
                                                         MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta, Estatus, _
                                                         Cartera, SoloPeso, TipoMercancia, Regulada)

        If Existencias = 0 Then str += " c.existencia > 0.000 and "
        If Existencias = 1 Then str += " c.existencia = 0.000 and "

        Dim strAlm As String = ""
        If AlmacenDesde <> "" Then strAlm += " a.almacen >= '" & AlmacenDesde & "' and "
        If AlmacenHasta <> "" Then strAlm += " a.almacen <= '" & AlmacenHasta & "' and "
        Dim strPrecios As String = IIf(PreciosConIVA, " if(" & TarifaA & ", ROUND(IF( ISNULL(b.MONTO), a.PRECIO_A , (b.MONTO/100 + 1)*a.PRECIO_A),0), 0.00) precio_a, " _
                                    & " if(" & TarifaB & ", ROUND(IF( ISNULL(b.MONTO), a.PRECIO_B , (b.MONTO/100 + 1)*a.PRECIO_B),0), 0.00) PRECIO_B, " _
                                    & " if(" & TarifaC & ", ROUND(IF( ISNULL(b.MONTO), a.PRECIO_C , (b.MONTO/100 + 1)*a.PRECIO_C),0), 0.00) PRECIO_C, " _
                                    & " if(" & TarifaD & ", ROUND(IF( ISNULL(b.MONTO), a.PRECIO_D , (b.MONTO/100 + 1)*a.PRECIO_D),0), 0.00) PRECIO_D, " _
                                    & " if(" & TarifaE & ", ROUND(IF( ISNULL(b.MONTO), a.PRECIO_E , (b.MONTO/100 + 1)*a.PRECIO_E),0), 0.00) PRECIO_E, " _
                                    & " if(" & TarifaF & ", ROUND(IF( ISNULL(b.MONTO), a.PRECIO_F , (b.MONTO/100 + 1)*a.PRECIO_F),0), 0.00) PRECIO_F, ", _
                                    " if( " & TarifaA & ", a.precio_a, 0.00) precio_A, if( " & TarifaB & ", a.precio_b, 0.00) precio_b, " _
                                    & " if( " & TarifaC & ", a.precio_c, 0.00) precio_C, if( " & TarifaD & ", a.precio_d, 0.00) precio_d, " _
                                    & " if( " & TarifaE & ", a.precio_E, 0.00) precio_E, if( " & TarifaF & ", a.precio_F, 0.00) precio_f, ")

        SeleccionMERMercancias = "select a.codart, a.nomart, a.alterno, a.barras, date_format(a.creacion,'%d-%m-%Y') creacion,  " _
              & " a.nombrecategoria categoria, a.nombremarca marca, a.nombrejerarquia tipjer, codjer1, codjer2, codjer3, " _
              & " codjer4, codjer5, codjer6,  elt(a.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') mix, a.nombredivision division, " _
              & " if( b.monto is null, 0.00, b.monto) iva, " & strPrecios _
              & " a.DESC_A, a.DESC_B, a.DESC_C, a.DESC_D, a.DESC_E, a.DESC_F, " _
              & " a.presentacion, a.sugerido, a.unidad, a.pesounidad, IF( ISNULL(b.monto), 0.00, b.monto) monto,  " _
              & " c.existencia, " _
              & " o.codartcom, o.descrip as descripcom, o.cantidad as cantidadcom, o.unidad as unidadcom, o.costo costocom  " _
              & " from (" & SeleccionGENMercancias() & ") a " _
              & " left join (" & SeleccionGENTablaIVA(jytsistema.sFechadeTrabajo) & ") b on (a.iva = b.tipo) " _
              & " left join (select a.codart, sum(a.existencia) existencia, a.id_emp from jsmerextalm a where " & strAlm & " a.id_emp = '" & jytsistema.WorkID & "' group by a.codart ) c on (a.codart = c.codart and a.id_emp = c.id_emp) " _
              & " left join jsmercatcom o on (a.codart = o.codart and a.id_emp = o.id_emp) " _
              & " Where " & str _
              & " a.id_emp = '" & jytsistema.WorkID & "' order by a." & OrdenadoPor & " "

    End Function
    Function seleccionMERCAS_UbicacionAlmacen(MyConn As MySqlConnection, CodigoUbicacion As String, CodigoEstante As String)

        Dim cod As String = Codigo128(ft.DevuelveScalarCadena(MyConn, " SELECT CONCAT(a.codubi, a.codest, b.codalm) " _
                                                              & " FROM jsmercatubi a " _
                                                              & " LEFT JOIN jsmercatest b ON (a.codest = b.codest AND a.id_emp = b.id_emp) " _
                                                              & " WHERE " _
                                                              & " a.codest = '" & CodigoEstante & "' AND " _
                                                              & " a.codubi = '" & CodigoUbicacion & "' AND " _
                                                              & " a.ID_EMP = '" & jytsistema.WorkID & "'  ")).Replace("'", String.Empty)

        Return " SELECT '" & cod & "' BARRAS,  CONCAT(b.codalm,'-', a.codest,'-' ,a.codubi ) BARRASREAL,  " _
                & " CONCAT('UBICACION : ', a.LAD,'-',a.FIL,'-',a.COL, '   ESTANTE : ', b.DESCRIP, '   ALMACEN : ' , b.codalm) NOMART " _
                & " FROM jsmercatubi a " _
                & " LEFT JOIN jsmercatest b ON (a.codest = b.codest AND a.id_emp = b.id_emp) " _
                & " WHERE " _
                & " a.codest = '" & CodigoEstante & "' AND " _
                & " a.codubi = '" & CodigoUbicacion & "' AND " _
                & " a.ID_EMP = '" & jytsistema.WorkID & "'  "

    End Function
    Function SeleccionMERCodigoBarras(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoMercancia As String) As String

        Dim cod As String = Codigo128(ft.DevuelveScalarCadena(MyConn, " select a.barras from jsmerctainv a where a.codart = '" & CodigoMercancia & "' and a.id_emp = '" & jytsistema.WorkID & "'  ")).Replace("'", String.Empty)

        Return "  SELECT a.codart, '" & cod & "' barras, a.barras barrasreal, a.nomart, " _
            & " ROUND( (IF( ISNULL(b.monto), 0, b.monto )/100 + 1) *a.precio_A/IF( ISNULL(c.equivale), 1, c.equivale), 2) PRECIO_A, " _
            & " ROUND( (IF( ISNULL(b.monto), 0, b.monto )/100 + 1) *a.precio_B/IF( ISNULL(c.equivale), 1, c.equivale), 2) PRECIO_B, " _
            & " ROUND( (IF( ISNULL(b.monto), 0, b.monto )/100 + 1) *a.precio_C/IF( ISNULL(c.equivale), 1, c.equivale), 2) PRECIO_C, " _
            & " ROUND( (IF( ISNULL(b.monto), 0, b.monto )/100 + 1) *a.precio_D/IF( ISNULL(c.equivale), 1, c.equivale), 2) PRECIO_D, " _
            & " ROUND( (IF( ISNULL(b.monto), 0, b.monto )/100 + 1) *a.precio_E/IF( ISNULL(c.equivale), 1, c.equivale), 2) PRECIO_E, " _
            & " ROUND( (IF( ISNULL(b.monto), 0, b.monto )/100 + 1) *a.precio_F/IF( ISNULL(c.equivale), 1, c.equivale), 2) PRECIO_F, ROUND( (IF( ISNULL(b.monto), 0, b.monto )/100 + 1) *PRECIO_D, 2 ) PRECIO_CAJA " _
            & " FROM jsmerctainv a " _
            & " LEFT JOIN (" & SeleccionGENTablaIVA(jytsistema.sFechadeTrabajo) & ") b ON (a.iva = b.tipo) " _
            & " LEFT JOIN jsmerequmer c ON (a.codart = c.codart AND a.unidaddetal = c.uvalencia  AND a.id_emp = c.id_emp) " _
            & " WHERE " _
            & " a.codart = '" & CodigoMercancia & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' "


    End Function

    Function SeleccionMERInventarioLegalPlus(ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                     ByVal FechaInicial As Date, ByVal FechaFinal As Date, _
                                Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                Optional ByVal AlmacenDesde As String = "", Optional ByVal AlmacenHasta As String = "", _
                                Optional ByVal Estatus As Integer = 9, Optional ByVal Cartera As Integer = 9, _
                                Optional ByVal SoloPeso As Boolean = False, Optional ByVal TipoMercancia As Integer = 99, _
                                Optional ByVal Regulada As Integer = 9, Optional ByVal TarifaA As Boolean = True, _
                                Optional ByVal Existencias As Integer = 2) As String

        Dim str As String = CadenaComplementoMercancias("a", MercanciaDesde, MercanciaHasta, Operador, OrdenadoPor, TipoJerarquia, Nivel1, _
                                                         Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, CategoriaDesde, CategoriaHasta, _
                                                         MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta, Estatus, _
                                                         Cartera, SoloPeso, TipoMercancia, Regulada)

        If Existencias = 0 Then str += " c.existencia > 0.000 and "
        If Existencias = 1 Then str += " c.existencia = 0.000 and "

        Dim strAlm As String = ""
        If AlmacenDesde <> "" Then strAlm += " b.almacen >= '" & AlmacenDesde & "' AND "
        If AlmacenHasta <> "" Then strAlm += " b.almacen <= '" & AlmacenHasta & "' AND "

        Dim strFechaInicial As String = ft.FormatoFechaHoraMySQLInicial(FechaInicial)
        Dim strFechaFinal As String = ft.FormatoFechaHoraMySQLFinal(FechaFinal)

        SeleccionMERInventarioLegalPlus = " SELECT a.codart, a.nomart, a.grupo, a.marca, a.division, a.unidad, " _
                & " IF( b.existencia IS NULL, 0.00, IF(b.existencia = 0.00, 0.00, b.existencia)) InventarioInicial,  " _
                & " IF( b.existenciafinal IS NULL, 0.00, IF(b.existenciafinal = 0.00, 0.00, b.existenciafinal)) InventarioFinal,  " _
                & " IF (b.costoInicialTotal IS NULL, 0.00,  b.costoInicialtotal) costoInicialTotal,  " _
                & " IF (b.entradas IS NULL, 0.00, b.entradas) Entradas, " _
                & " IF (b.salidas IS NULL, 0.00, b.salidas) salidas, " _
                & " IF (b.costoi IS NULL, 0.00,  b.costoi) CostoInicial, " _
                & " IF (b.costoe IS NULL, 0.00,  b.costoe) CostoEntradas, " _
                & " IF (b.costof IS NULL, 0.00,  b.costof) Costopromedio,  " _
                & " IF (b.costos IS NULL, 0.00,  b.costos) costoSalidas,  " _
                & " IF (b.costoFinalTotal IS NULL, 0.00,  b.costofinaltotal) costoFinalTotal,  " _
                & " 0.000 autoconsumo, 0.00 costoautoconsumo, 0.000 retiros, 0.00 costoretiros " _
                & " FROM jsmerctainv a " _
                & " LEFT JOIN (SELECT a.codart, " _
                & "             SUM(IF( ISNULL(f.uvalencia), a.existenciaInicial, a.existenciaInicial/f.equivale)) existencia, " _
                & "             SUM(IF( ISNULL(f.uvalencia), a.existenciafinal, a.existenciafinal/f.equivale)) existenciafinal, " _
                & "             SUM(a.costopromedioinicial) costoi, " _
                & "             SUM(a.costopromediofinal) costof , " _
                & "             SUM(a.costopromedioinicial)*SUM(IF( ISNULL(f.uvalencia), a.existenciaInicial, a.existenciaInicial/f.equivale)) costoInicialtotal , " _
                & "             SUM(a.costopromedioFinal)*SUM(IF( ISNULL(f.uvalencia), a.existenciaFinal, a.existenciaFinal/f.equivale)) costofinaltotal , " _
                & "             SUM(a.costopromedioFinal)*SUM(IF( ISNULL(f.uvalencia), a.entradas, a.entradas/f.equivale )) costoe, " _
                & "             SUM(IF( ISNULL(f.uvalencia), a.entradas, a.entradas/f.equivale )) entradas, " _
                & "             SUM(a.costopromediofinal)*SUM(IF( ISNULL(f.uvalencia), a.salidas, a.salidas/f.equivale )) costos, " _
                & "             SUM(IF( ISNULL(f.uvalencia), a.salidas, a.salidas/f.equivale )) salidas " _
                & "             FROM (SELECT b.codart, b.unidad, " _
                & "                     SUM( IF ( b.tipomov IN ('EN','AE','DV','AC') AND b.fechamov < '" & strFechaInicial & "', b.costotaldes, 0.00))/SUM( IF( b.fechamov < '" & strFechaInicial & "',  IF( b.TIPOMOV IN ('EN','AE','DV') , b.cantidad,  0  ) , 0 ) )  CostoPromedioInicial, " _
                & "                     SUM( IF( b.fechamov < '" & strFechaInicial & " ',  IF( b.TIPOMOV IN ('EN','AE','DV') , b.cantidad, IF( b.tipomov IN('SA','AS','DC'), -1 * b.cantidad, 0)), 0 ) ) ExistenciaInicial,  " _
                & "                     SUM( IF( b.fechamov <= '" & strFechaFinal & "',  IF( b.TIPOMOV IN ('EN','AE','DV') , b.cantidad, IF( b.tipomov IN('SA','AS','DC'), -1 * b.cantidad, 0)), 0 ) ) ExistenciaFinal,  " _
                & "                     SUM( IF ( b.tipomov IN ('EN','AE','DV','AC') AND b.fechamov < '" & strFechaInicial & "', b.costotaldes, 0.00)) CostoTotalInicial, " _
                & "                     SUM( IF ( b.tipomov IN ('EN','AE','DV','AC'), b.costotaldes, 0.00))/SUM( IF( b.TIPOMOV IN ('EN','AE','DV') , b.cantidad,  0  )  )  CostoPromedioFinal, " _
                & "                     SUM( IF ( b.tipomov IN ('EN','AE','DV','AC'), b.costotaldes, 0.00)) CostoTotalFinal, " _
                & "                     SUM( IF ( b.fechamov >= '" & strFechaInicial & "',  IF( b.tipomov IN ('EN','AE','DV'), b.cantidad, 0.00), 0.00)) entradas, " _
                & "                     SUM( IF ( b.tipomov IN ('EN','AE','DV','AC') AND b.fechamov >= '" & strFechaInicial & "', b.costotaldes, 0.00)) CostoEntradas, " _
                & "                     SUM( IF ( b.fechamov >= '" & strFechaInicial & "',  IF( b.tipomov IN ('EN','AE','DV'), b.cantidad, 0.00), 0.00))*SUM( IF ( b.tipomov IN ('EN','AE','DV','AC'), b.costotaldes, 0.00))/SUM( IF( b.TIPOMOV IN ('EN','AE','DV') , b.cantidad,  0  )  ) CostoTotalEntradasConPromedio,  " _
                & "                     SUM( IF ( b.fechamov >= '" & strFechaInicial & "',  IF( b.tipomov IN ('SA','AS','DC'), b.cantidad, 0.00), 0.00)) salidas, " _
                & "                     SUM( IF ( b.fechamov >= '" & strFechaInicial & "',  IF( b.tipomov IN ('SA','AS','DC'), b.cantidad, 0.00), 0.00))*SUM( IF ( b.tipomov IN ('EN','AE','DV','AC'), b.costotaldes, 0.00))/SUM( IF( b.TIPOMOV IN ('EN','AE','DV') , b.cantidad,  0  )  ) CostoTotalSalidasConPromedio " _
                & "                   FROM jsmertramer b " _
                & "                   WHERE " _
                & strAlm _
                & "                     b.id_emp = '" & jytsistema.WorkID & "' AND " _
                & "                     b.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                & "                     b.fechamov <= '" & strFechaFinal & "' " _
                & "                     GROUP BY b.codart, b.unidad  ) a " _
                & "             LEFT JOIN jsmerequmer f ON (a.codart = f.codart AND a.unidad = f.uvalencia AND f.id_emp = '" & jytsistema.WorkID & "') " _
                & "             GROUP BY a.codart ) b " _
                & " ON (a.codart = b.codart) Where " & str & " a.id_emp = '" & jytsistema.WorkID & "' ORDER BY a." & OrdenadoPor & " "


    End Function
    Function SeleccionMERMercanciasPreciosYEquivalencias(ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                    Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                    Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                    Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                    Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                    Optional ByVal AlmacenDesde As String = "", Optional ByVal AlmacenHasta As String = "", _
                                    Optional ByVal Estatus As Integer = 9, Optional ByVal Cartera As Integer = 9, _
                                    Optional ByVal SoloPeso As Boolean = False, Optional ByVal TipoMercancia As Integer = 99, _
                                    Optional ByVal Regulada As Integer = 9, Optional ByVal Tarifa As Integer = 0, _
                                    Optional ByVal PreciosConIVA As Boolean = False, _
                                    Optional ByVal Existencias As Integer = 2) As String
        Dim aTarifa() As String = {"A", "B", "C", "D", "E", "F"}

        Dim str As String = CadenaComplementoMercancias("a", MercanciaDesde, MercanciaHasta, Operador, OrdenadoPor, TipoJerarquia, Nivel1, _
                                                         Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, CategoriaDesde, CategoriaHasta, _
                                                         MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta, Estatus, _
                                                         Cartera, SoloPeso, TipoMercancia, Regulada)

        If Existencias = 0 Then str += " round(c.existencia,0) > 0.000 and "
        If Existencias = 1 Then str += " round(c.existencia,0) = 0.000 and "

        'CalculaTotalIVAVentas 

        Dim strAlm As String = ""
        If AlmacenDesde <> "" Then strAlm += " b.almacen >= '" & AlmacenDesde & "' and "
        If AlmacenHasta <> "" Then strAlm += " b.almacen <= '" & AlmacenHasta & "' and "

        SeleccionMERMercanciasPreciosYEquivalencias = "select a.codart, a.nomart, a.alterno, a.barras, date_format(a.creacion,'%d-%m-%Y') creacion, " _
              & " a.nombrecategoria categoria, a.nombremarca marca, a.nombrejerarquia tipjer, codjer1, codjer2, codjer3, codjer4, codjer5, codjer6, " _
              & " elt(a.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') as mix, a.nombredivision division, " _
              & " if( b.monto is null, 0.00, b.monto) iva, " _
              & " a.presentacion, a.sugerido, a.unidad, a.pesounidad, IF( ISNULL(b.monto), 0.00, b.monto) monto,  " _
              & " c.existencia, " _
              & " ROUND(a.precio_" & aTarifa(Tarifa) & "*(1+IF(b.monto IS NULL, 0.00, b.monto)/100),2) precio, q.equivale, " _
              & " q.uvalencia, ROUND(a.precio_" & aTarifa(Tarifa) & "/q.equivale*(1+IF(b.monto IS NULL, 0.00, b.monto)/100),2) precioEquivalente, " _
              & " ELT(q.divide+1,'No','Si') divideuv " _
              & " from (" & SeleccionGENMercancias() & ") a " _
              & " left join (" & SeleccionGENTablaIVA(jytsistema.sFechadeTrabajo) & ") b on (a.iva = b.tipo) " _
              & " left join (select b.codart, sum(b.existencia) existencia, b.id_emp from jsmerextalm b where " & strAlm & " b.id_emp = '" & jytsistema.WorkID & "' group by b.codart ) c on (a.codart = c.codart and a.id_emp = c.id_emp) " _
              & " left join jsmerequmer q on (a.codart = q.codart and a.id_emp = q.id_emp)  " _
              & " Where " _
              & str _
              & " a.id_emp = '" & jytsistema.WorkID & "' order by a." & OrdenadoPor & " "


    End Function

    Function SeleccionMERMercanciasPreciosYEquivalenciasSinIVA(ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                    Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                    Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                    Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                    Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                    Optional ByVal AlmacenDesde As String = "", Optional ByVal AlmacenHasta As String = "", _
                                    Optional ByVal Estatus As Integer = 9, Optional ByVal Cartera As Integer = 9, _
                                    Optional ByVal SoloPeso As Boolean = False, Optional ByVal TipoMercancia As Integer = 99, _
                                    Optional ByVal Regulada As Integer = 9, Optional ByVal Tarifa As Integer = 0, _
                                    Optional ByVal PreciosConIVA As Boolean = False, _
                                    Optional ByVal Existencias As Integer = 2) As String
        Dim aTarifa() As String = {"A", "B", "C", "D", "E", "F"}

        Dim str As String = CadenaComplementoMercancias("a", MercanciaDesde, MercanciaHasta, Operador, OrdenadoPor, TipoJerarquia, Nivel1, _
                                                         Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, CategoriaDesde, CategoriaHasta, _
                                                         MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta, Estatus, _
                                                         Cartera, SoloPeso, TipoMercancia, Regulada)

        If Existencias = 0 Then str += " round(c.existencia,0) > 0.000 and "
        If Existencias = 1 Then str += " round(c.existencia,0) = 0.000 and "

        'CalculaTotalIVAVentas 

        Dim strAlm As String = ""
        If AlmacenDesde <> "" Then strAlm += " b.almacen >= '" & AlmacenDesde & "' and "
        If AlmacenHasta <> "" Then strAlm += " b.almacen <= '" & AlmacenHasta & "' and "

        SeleccionMERMercanciasPreciosYEquivalenciasSinIVA = "select a.codart, a.nomart, a.alterno, a.barras, date_format(a.creacion,'%d-%m-%Y') creacion, " _
              & " a.nombrecategoria categoria, a.nombremarca marca, a.nombrejerarquia tipjer, codjer1, codjer2, codjer3, codjer4, codjer5, codjer6, " _
              & " elt(a.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') as mix, a.nombredivision division, " _
              & " if( b.monto is null, 0.00, b.monto) iva, " _
              & " a.presentacion, a.sugerido, a.unidad, a.pesounidad, IF( ISNULL(b.monto), 0.00, b.monto) monto,  " _
              & " c.existencia, " _
              & " ROUND(a.precio_" & aTarifa(Tarifa) & ",2) precio, q.equivale, " _
              & " q.uvalencia, ROUND(a.precio_" & aTarifa(Tarifa) & "/q.equivale,2) precioEquivalente, " _
              & " ELT(q.divide+1,'No','Si') divideuv " _
              & " from (" & SeleccionGENMercancias() & ") a " _
              & " left join (" & SeleccionGENTablaIVA(jytsistema.sFechadeTrabajo) & ") b on (a.iva = b.tipo) " _
              & " left join (select b.codart, sum(b.existencia) existencia, b.id_emp from jsmerextalm b where " & strAlm & " b.id_emp = '" & jytsistema.WorkID & "' group by b.codart ) c on (a.codart = c.codart and a.id_emp = c.id_emp) " _
              & " left join jsmerequmer q on (a.codart = q.codart and a.id_emp = q.id_emp)  " _
              & " Where " _
              & str _
              & " a.id_emp = '" & jytsistema.WorkID & "' order by a." & OrdenadoPor & " "

    End Function

    Function SeleccionMERMercanciasYEquivalencias(ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                Optional ByVal AlmacenDesde As String = "", Optional ByVal AlmacenHasta As String = "", _
                                Optional ByVal Estatus As Integer = 9, Optional ByVal Cartera As Integer = 9, _
                                Optional ByVal SoloPeso As Boolean = False, Optional ByVal TipoMercancia As Integer = 99, _
                                Optional ByVal Regulada As Integer = 9) As String

        Dim str As String = CadenaComplementoMercancias("a", MercanciaDesde, MercanciaHasta, Operador, OrdenadoPor, TipoJerarquia, Nivel1, _
                                                         Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, CategoriaDesde, CategoriaHasta, _
                                                         MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta, Estatus, _
                                                         Cartera, SoloPeso, TipoMercancia, Regulada)

        Dim strAlm As String = ""
        If AlmacenDesde <> "" Then strAlm += " a.almacen >= '" & AlmacenDesde & "' and "
        If AlmacenHasta <> "" Then strAlm += " a.almacen <= '" & AlmacenHasta & "' and "

        SeleccionMERMercanciasYEquivalencias = "select a.codart, a.nomart, a.alterno, a.barras, date_format(a.creacion,'%d-%m-%Y') creacion, " _
              & " a.nombrecategoria categoria, a.nombremarca marca, a.nombrejerarquia tipjer, codjer1, codjer2, codjer3, codjer4, codjer5, codjer6, " _
              & " elt(a.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') as mix, a.nombredivision division, " _
              & " a.presentacion, a.sugerido, a.unidad, a.pesounidad, if( b.equivale is null, 0.000, b.equivale) equivale, " _
              & " if( b.uvalencia is null, '', b.uvalencia) uvalencia, IF(b.divide = 0, 'Si', 'No') divideuv " _
              & " from (" & SeleccionGENMercancias() & ") a " _
              & " left join jsmerequmer b on (a.codart = b.codart and a.id_emp = b.id_emp)" _
              & " Where " & str _
              & " a.id_emp = '" & jytsistema.WorkID & "' order by a." & OrdenadoPor & " "

    End Function

    Function SeleccionMERObsolescencias(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                    Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                    Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                    Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                    Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                    Optional ByVal Estatus As Integer = 9, Optional ByVal Cartera As Integer = 9, _
                                    Optional ByVal TipoMercancia As Integer = 99, Optional ByVal Regulada As Integer = 9, _
                                    Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                    Optional ByVal AlmacenDesde As String = "", Optional ByVal AlmacenHasta As String = "") As String

        Dim str As String = CadenaComplementoMercancias("a", "", "", Operador, OrdenadoPor, TipoJerarquia, Nivel1, _
                                                         Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, CategoriaDesde, CategoriaHasta, _
                                                         MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta, Estatus, _
                                                         Cartera, , TipoMercancia, Regulada)

        Dim tbl As String = "tbl" & ft.NumeroAleatorio(100000)
        Dim strVendedor As String = ""
        If AsesorDesde <> "" Then strVendedor += " a.vendedor >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then strVendedor += " a.vendedor <= '" & AsesorHasta & "' and "
        If AlmacenDesde <> "" Then strVendedor += " a.almacen >= '" & AlmacenDesde & "' and "
        If AlmacenHasta <> "" Then strVendedor += " a.almacen <= '" & AlmacenHasta & "' and "


        ft.Ejecutar_strSQL(MyConn, " UPDATE jsmerctainv a, (SELECT a.codart, DATE_FORMAT(MAX(a.fechamov), '%Y-%m-%d') fecultventa, a.id_emp FROM jsmertramer a " _
                            & "                             WHERE " _
                            & "                             MID(a.codart,1,1) <> '$' AND " _
                            & "                             a.origen IN ('FAC', 'NDV', 'PVE', 'PFC') AND " _
                            & "                             a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " GROUP BY a.codart) b SET a.fecultventa = b.fecultventa " _
                            & " WHERE " _
                            & " a.codart = b.codart AND " _
                            & " a.id_emp = b.id_emp ")

        ft.Ejecutar_strSQL(MyConn, " DROP TEMPORARY TABLE IF EXISTS " & tbl)
        ft.Ejecutar_strSQL(MyConn, " CREATE TEMPORARY TABLE " & tbl _
                            & " SELECT a.codart, a.nomart, a.unidad, a.estatus, a.tipoart, " _
                            & " a.grupo, a.marca, a.tipjer, a.codjer1, a.codjer2, a.codjer3, a.codjer4, a.codjer5, a.codjer6,  ELT(a.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') mix, a.division, " _
                            & " DATEDIFF( a.fecultventa, DATE_SUB( a.fecultventa, INTERVAL 3 MONTH) ) - SUM( IF( b.fecha >= DATE_SUB( a.fecultventa, INTERVAL 3 MONTH) AND b.fecha <= a.fecultventa  ,  1,  0  ) ) dias_habiles_venta, " _
                            & " a.fecultventa, a.cuota, " _
                            & " DATEDIFF( NOW(), a.fecultventa ) - SUM( IF( b.fecha >= a.fecultventa AND b.fecha <= NOW()  ,  1,  0  ) ) dias_habiles_NO_venta, " _
                            & " a.id_emp, a.existe_act existencia, 0000000000.000 cantidad " _
                            & " FROM jsmerctainv a " _
                            & " LEFT JOIN ( SELECT id_emp, DATE_FORMAT(CONCAT(ano,'-',mes,'-',dia),'%Y-%m-%d') fecha " _
                            & "             FROM jsconcatper WHERE MODULO = 1 AND " _
                            & "             id_emp = '" & jytsistema.WorkID & "') b ON (a.id_emp = b.id_emp) " _
                            & " WHERE " _
                            & " a.id_emp = '" & jytsistema.WorkID & "' " _
                            & " GROUP BY a.codart ")

        ft.Ejecutar_strSQL(MyConn, " UPDATE " & tbl & " a, (SELECT a.codart, ROUND( SUM( IF( a.fechamov > DATE_SUB(c.fecultventa, INTERVAL 3 MONTH), " _
                    & " IF( b.uvalencia IS NULL, a.cantidad, a.cantidad / b.equivale ), 0  )), 3) cantidad, a.id_emp  " _
                    & " FROM jsmertramer a " _
                    & " LEFT JOIN jsmerequmer b ON (a.codart = b.codart AND a.unidad = b.uvalencia AND a.id_emp = b.id_emp) " _
                    & " LEFT JOIN jsmerctainv c ON (a.codart = c.codart AND a.id_emp = c.id_emp) " _
                    & " WHERE " _
                    & strVendedor _
                    & " a.fechamov >= '2007-12-31' AND " _
                    & " MID(a.codart,1,1) <> '$' AND " _
                    & " a.origen IN ('FAC', 'NDV', 'PVE', 'PFC') AND " _
                    & " a.id_emp = '" & jytsistema.WorkID & "' " _
                    & " GROUP BY a.codart) b SET a.cantidad = b.cantidad " _
                    & " WHERE a.codart = b.codart ")

        SeleccionMERObsolescencias = "select a.codart, a.nomart, a.unidad, " _
              & " j.descrip As categoria, k.descrip As marca, l.descrip As tipjer, codjer1, codjer2, codjer3, codjer4, codjer5, codjer6,  a.mix, p.descrip division, " _
              & " a.existencia, date_format(a.fecultventa,'%d-%m-%Y') fecultventa, a.cantidad/a.dias_habiles_venta ventaspromedio, a.dias_habiles_no_venta obsolescencia, a.dias_habiles_no_venta*a.cantidad/a.dias_habiles_venta ventanohecha  " _
              & " from " & tbl & " a " _
              & " left join jsconctatab j on (a.grupo = j.codigo and a.id_emp = j.id_emp and j.modulo = '00002') " _
              & " left join jsconctatab k on (a.marca = k.codigo and a.id_emp = k.id_emp and k.modulo = '00003') " _
              & " left join jsmerencjer l on (a.tipjer = l.tipjer and a.id_emp = l.id_emp) " _
              & " left join jsmercatdiv p on (a.division = p.division and a.id_emp = p.id_emp) " _
              & " Where " & str _
              & " a.existencia > 0.002 and " _
              & " a.id_emp = '" & jytsistema.WorkID & "' order by a.dias_habiles_no_venta desc"

    End Function

    Function SeleccionMERMovimientosMercancias(ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, _
                                ByVal OrdenadoPor As String, ByVal Operador As Integer, ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                Optional ByVal TipoDocumento As String = "", _
                                Optional ByVal AlmacenDesde As String = "", Optional ByVal AlmacenHasta As String = "", _
                                Optional ByVal ProvCliDesde As String = "", Optional ByVal ProvCliHasta As String = "", _
                                Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                Optional ByVal LoteDesde As String = "", Optional ByVal LoteHasta As String = "", _
                                Optional ByVal DocumentoDesde As String = "", Optional ByVal DocumentoHasta As String = "", _
                                Optional ByVal OrigenDesde As String = "", Optional ByVal OrigenHasta As String = "", _
                                Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", _
                                Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                Optional ByVal Estatus As Integer = 9, Optional ByVal Cartera As Integer = 9, _
                                Optional ByVal SoloPeso As Boolean = False, Optional ByVal TipoMercancia As Integer = 99, _
                                Optional ByVal Regulada As Integer = 9) As String

        Dim str As String = CadenaComplementoMercancias("a", MercanciaDesde, MercanciaHasta, Operador, OrdenadoPor, _
                                                         TipoJerarquia, Nivel1, Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, _
                                                         CategoriaDesde, CategoriaHasta, MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta, _
                                                         Estatus, Cartera, SoloPeso, TipoMercancia, Regulada)

        If MercanciaDesde <> "" Then str += " b.codart >= '" & MercanciaDesde & "' and "
        If MercanciaHasta <> "" Then str += " b.codart <= '" & MercanciaHasta & "' and "
        str += " b.fechamov >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and "
        str += " b.fechamov <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and "
        If TipoDocumento <> "" Then str += " locate(b.tipomov, '" & TipoDocumento & "') > 0 and "

        If AlmacenDesde <> "" Then str += " b.almacen >= '" & AlmacenDesde & "' and "
        If AlmacenHasta <> "" Then str += " b.almacen <= '" & AlmacenHasta & "' and "
        If ProvCliDesde <> "" Then str += " b.prov_cli >= '" & ProvCliDesde & "' and "
        If ProvCliHasta <> "" Then str += " b.prov_cli <= '" & ProvCliHasta & "' and "
        If AsesorDesde <> "" Then str += " b.vendedor >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " b.vendedor <= '" & AsesorHasta & "' and "
        If LoteDesde <> "" Then str += " b.lote >= '" & LoteDesde & "' and "
        If LoteHasta <> "" Then str += " b.lote <= '" & LoteHasta & "' and "
        If DocumentoDesde <> "" Then str += " b.numdoc >= '" & DocumentoDesde & "' and "
        If DocumentoHasta <> "" Then str += " b.numdoc <= '" & DocumentoHasta & "' and "
        If OrigenDesde <> "" Then str += " b.origen >= '" & OrigenDesde & "' and "
        If OrigenHasta <> "" Then str += " b.origen <= '" & OrigenHasta & "' and "

        SeleccionMERMovimientosMercancias = " select a.codart, a.nomart, " _
            & " j.descrip categoria, k.descrip marca, l.descrip As tipjer, " _
            & " elt(a.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') as mix, p.descrip division, " _
            & " date_format(b.fechamov,'%d-%m-%Y') fechamov, b.tipomov, b.numdoc, a.unidad, " _
            & " if( isnull(n.uvalencia), (if(b.tipomov in ('EN','AE','DV'), b.cantidad, -1*b.cantidad)), " _
            & " (if(b.tipomov in ('EN','AE','DV'), b.cantidad, -1*b.cantidad)) / n.equivale ) cantidad, " _
            & " if(b.tipomov in ('EN','AE','DV'), b.peso, -1*b.peso) peso, " _
            & " if(b.tipomov in ('EN','AE','DV'), b.costotal, -1*b.costotal) costotal, " _
            & " if(b.tipomov in ('EN','AE','DV'), b.costotaldes, -1*b.costotaldes) costotaldes, " _
            & " b.origen, b.numorg, b.lote, b.prov_cli, b.ventotal, b.ventotaldes, b.impiva, b.descuento, b.vendedor, " _
            & " b.almacen,  b.asiento renglonestatus, if( b.origen in ('FAC', 'PFC', 'NCV', 'NDV' ), r.nombre, if( b.origen in ('COM', 'GAS', 'REP', 'NCC', 'NDC'), s.nombre, '' ) ) proveedorcliente  " _
            & " from jsmertramer b " _
            & " left join jsmerctainv a on (b.codart = a.codart and b.id_emp = a.id_emp) " _
            & " left join jsconctatab j on (a.grupo = j.codigo and b.id_emp = j.id_emp and j.modulo = '00002') " _
            & " left join jsconctatab k on (a.marca = k.codigo and b.id_emp = k.id_emp and k.modulo = '00003') " _
            & " left join jsmerencjer l on (a.tipjer = l.tipjer and b.id_emp = l.id_emp) " _
            & " left join jsmercatdiv p on (a.division = p.division and b.id_emp = p.id_emp) " _
            & " left join jsmerequmer n on (b.codart = n.codart and b.unidad = n.uvalencia and b.id_emp = n.id_emp) " _
            & " left join jsvencatcli r on (b.prov_cli = r.codcli and b.id_emp = r.id_emp)" _
            & " left join jsprocatpro s on (b.prov_cli = s.codpro and b.id_emp = s.id_emp)" _
            & " Where " _
            & str _
            & " b.id_emp = '" & jytsistema.WorkID & "' " _
            & " Order By " _
            & "a." & OrdenadoPor & ", b.fechamov "

    End Function

    Function SeleccionMERMovimientosServicios(ByVal ServicioDesde As String, ByVal ServicioHasta As String, _
                                ByVal OrdenadoPor As String, ByVal Operador As Integer, ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                Optional ByVal TipoDocumento As String = "", _
                                Optional ByVal AlmacenDesde As String = "", Optional ByVal AlmacenHasta As String = "", _
                                Optional ByVal ProvCliDesde As String = "", Optional ByVal ProvCliHasta As String = "", _
                                Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                Optional ByVal DocumentoDesde As String = "", Optional ByVal DocumentoHasta As String = "", _
                                Optional ByVal OrigenDesde As String = "", Optional ByVal OrigenHasta As String = "", _
                                Optional ByVal Estatus As Integer = 9) As String

        Dim str As String = ""

        If ServicioDesde <> "" Then str += " a.item >= '$" & ServicioDesde & "' and "
        If ServicioHasta <> "" Then str += " a.item <= '$" & ServicioHasta & "' and "
        str += " a.fechamov >= '" & ft.FormatoFechaMySQL(FechaDesde) & "' and "
        str += " a.fechamov <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' and "
        If TipoDocumento <> "" Then str += " locate(a.tipomov, '" & TipoDocumento & "') > 0 and "
        If ProvCliDesde <> "" Then str += " a.prov_cli >= '" & ProvCliDesde & "' and "
        If ProvCliHasta <> "" Then str += " a.prov_cli <= '" & ProvCliHasta & "' and "
        If AsesorDesde <> "" Then str += " a.vendedor >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.vendedor <= '" & AsesorHasta & "' and "
        If DocumentoDesde <> "" Then str += " a.numdoc >= '" & DocumentoDesde & "' and "
        If DocumentoHasta <> "" Then str += " a.numdoc <= '" & DocumentoHasta & "' and "
        If OrigenDesde <> "" Then str += " a.origen >= '" & OrigenDesde & "' and "
        If OrigenHasta <> "" Then str += " a.origen <= '" & OrigenHasta & "' and "

        Dim strSQLMov As String = " SELECT a.item, b.emision fechamov, 'EN' tipomov, a.numcom numdoc, a.unidad, b.almacen, " _
                & " a.cantidad, a.costotot costotal, a.costototdes costotaldes, '' lote, 0.00 peso, 'COM' origen, b.codpro prov_cli, " _
                & " c.nombre proveedorcliente, b.asiento, b.fechasi, '' vendedor , " _
                & " '' nomvendedor, a.id_emp " _
                & " FROM jsprorencom a " _
                & " LEFT JOIN jsproenccom b ON (a.numcom = b.numcom AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN jsprocatpro c ON (b.codpro = c.codpro AND b.id_emp = c.id_emp) " _
                & " WHERE " _
                & " substring(a.ITEM,1,1) = '$' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " UNION SELECT a.item, b.emision fechamov, 'EN' tipomov, a.numgas numdoc, a.unidad, b.almacen, " _
                & " a.cantidad, a.costotot costotal, a.costototdes costotaldes, '' lote, 0.00 peso, 'GAS' origen, b.codpro prov_cli, " _
                & " c.nombre proveedorcliente, b.asiento, b.fechasi, '' vendedor , " _
                & " '' nomvendedor, a.id_emp " _
                & " FROM jsprorengas a " _
                & " LEFT JOIN jsproencgas b ON (a.numgas = b.numgas AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN jsprocatpro c ON (b.codpro = c.codpro AND b.id_emp = c.id_emp) " _
                & " WHERE " _
                & " substring(a.ITEM,1,1) = '$' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " UNION SELECT a.item, b.emision fechamov, 'SA' tipomov, a.numfac numdoc, a.unidad, b.almacen, " _
                & " a.cantidad, a.totren costotal, a.totrendes costotaldes, '' lote, 0.00 peso, 'FAC' origen, b.codcli prov_cli, " _
                & " c.nombre proveedorcliente, b.asiento, b.fechasi, b.codven vendedor , " _
                & " '' nomvendedor, a.id_emp " _
                & " FROM jsvenrenfac a " _
                & " LEFT JOIN jsvenencfac b ON (a.numfac = b.numfac AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN jsvencatcli c ON (b.codcli = c.codcli AND b.id_emp = c.id_emp) " _
                & " WHERE " _
                & " substring(a.ITEM,1,1) = '$' AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' "

        Return " select *, a.item codart, b.desser nomart, b.codcon, c.descripcion nombrecontable  from (" & strSQLMov & ") a " _
            & " LEFT JOIN jsmercatser b on (a.item = concat('$',b.codser) and a.id_emp = b.id_emp) " _
            & " LEFT JOIN jscotcatcon c on (b.codcon = c.codcon and b.id_emp = c.id_emp) " _
            & " where " _
            & str _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " order by a.fechamov, a.numdoc, a.tipomov "

    End Function

    Function SeleccionMERExistenciasAFecha(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
        ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
        Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", _
        Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
        Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
        Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", Optional ByVal AlmacenDesde As String = "", Optional ByVal AlmacenHasta As String = "", _
        Optional ByVal FechaHasta As Date = MyDate, Optional ByVal Estatus As Integer = 9, Optional ByVal Cartera As Integer = 9, _
        Optional ByVal SoloPeso As Boolean = False, Optional ByVal TipoMercancia As Integer = 99, _
        Optional ByVal Regulada As Integer = 9, Optional ByVal Existencias As Integer = 2, _
        Optional porcentajeGastos As Double = 0.0, Optional TarifaPrecio As String = "A") As String

        Dim str As String = CadenaComplementoMercancias("a", MercanciaDesde, MercanciaHasta, Operador, OrdenadoPor, _
                                                         TipoJerarquia, Nivel1, Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, _
                                                         CategoriaDesde, CategoriaHasta, MarcaDesde, MarcaHasta, DivisionDesde, _
                                                         DivisionHasta, Estatus, Cartera, SoloPeso, TipoMercancia, Regulada)
        Dim diasHabiles As Integer = 90
        Dim strAlm As String = ""
        If AlmacenDesde <> "" Then strAlm += " b.almacen >= '" & AlmacenDesde & "' and "
        If AlmacenHasta <> "" Then strAlm += " b.almacen <= '" & AlmacenHasta & "' and "

        If Estatus < 2 Then str += " a.estatus = " & Estatus & " and "

        Dim strExistencia As String = IIf(Existencias < 2, IIf(Existencias = 1, "having round(cantidad,3) = 0.000 ", " having round(cantidad,3) > 0.000 "), "")

        Dim tbltramer As String = "tbltramer"
        Dim tblCostos As String = "tblCostos"
        Dim tblext As String = "tblext"
        Dim tblextReal As String = "tblextReal"
        Dim tblDiaria As String = "tblDiaria"
        Dim tblDiariaReal = "tblDiariaReal"

        'existencias
        ft.Ejecutar_strSQL(myconn, " drop temporary table if exists " & tblext)
        ft.Ejecutar_strSQL(myconn, " create temporary table " & tblext _
            & " select b.codart, b.unidad, " _
            & " SUM(IF( b.TIPOMOV in ('EN','AE', 'DV'), b.CANTIDAD, -1 * b.CANTIDAD)) existencia " _
            & " From jsmertramer b " _
            & " Where " _
            & strAlm _
            & " not tipomov in ('AC','AP') AND " _
            & " b.id_emp = '" & jytsistema.WorkID & "' and " _
            & " b.ejercicio = '" & jytsistema.WorkExercise & "' and " _
            & " b.fechamov < '" & ft.FormatoFechaMySQL(DateAdd("d", 1, FechaHasta)) & "' " _
            & " group by b.codart, b.unidad ")

        ft.Ejecutar_strSQL(myconn, " drop temporary table if exists " & tblextReal)
        ft.Ejecutar_strSQL(myconn, " Create temporary table " & tblextReal _
                        & " select a.codart, sum(if( isnull(f.uvalencia), a.existencia, a.existencia/f.equivale)) existencia " _
                        & " from " & tblext & " a " _
                        & " left join jsmerequmer f on (a.codart = f.codart and a.unidad = f.uvalencia and f.id_emp = '" & jytsistema.WorkID & "') " _
                        & " group by a.codart ")

        Dim DiasNH As Integer = ft.DevuelveScalarEntero(MyConn, " SELECT COUNT(id_emp) FROM jsconcatper WHERE " _
                                                        & " MODULO = 1 AND " _
                                                        & " DATE_FORMAT(CONCAT(ano,'-',mes,'-',dia),'%Y-%m-%d') >= '" & ft.FormatoFechaMySQL(DateAdd("d", -1 * diasHabiles, FechaHasta)) & "' AND " _
                                                        & " DATE_FORMAT(CONCAT(ano,'-',mes,'-',dia),'%Y-%m-%d') <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
                                                        & " id_emp = '" & jytsistema.WorkID & "' GROUP BY id_emp")

        ft.Ejecutar_strSQL(myconn, "Drop temporary table if exists " & tblDiaria)
        ft.Ejecutar_strSQL(myconn, " create temporary table " & tblDiaria _
            & " select b.codart, b.unidad, sum(IF( b.TIPOMOV = 'EN' OR b.tipomov = 'AE' OR b.TIPOMOV = 'DV', 0.00, b.CANTIDAD))/ " _
            & " (to_days('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_days('" & ft.FormatoFechaMySQL(DateAdd("d", -1 * IIf(diasHabiles = 0, 1, diasHabiles), FechaHasta)) & "') - " & DiasNH & " )  salidas " _
            & " from jsmertramer b " _
            & " Where " _
            & " b.id_emp = '" & jytsistema.WorkID & "' and " _
            & " b.ejercicio = '' and " _
            & " b.fechamov >= '" & ft.FormatoFechaMySQL(DateAdd("d", -1 * diasHabiles, FechaHasta)) & "' and " _
            & " b.fechamov <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' " _
            & " group by b.codart, b.unidad ")

        ft.Ejecutar_strSQL(myconn, "drop temporary table if exists " & tblDiariaReal)
        ft.Ejecutar_strSQL(myconn, " create temporary table " & tblDiariaReal & " select a.codart, sum(if( isnull(f.uvalencia), a.salidas, a.salidas/f.equivale)) salidasdiarias " _
            & " from " & tblDiaria & " a left join jsmerequmer f on (a.codart = f.codart and a.unidad = f.uvalencia and f.id_emp = '" & jytsistema.WorkID & "') " _
            & " group by a.codart ")

        'costos
        ft.Ejecutar_strSQL(myconn, " drop temporary table if exists " & tbltramer)
        ft.Ejecutar_strSQL(myconn, " create temporary table " & tbltramer & " (codart varchar(15)  not null, fechamov datetime not null) ")
        ft.Ejecutar_strSQL(myconn, " create index " & tbltramer & " on " & tbltramer & " (codart, fechamov) ")
        ft.Ejecutar_strSQL(myconn, " insert into " & tbltramer & " select codart, max(fechamov) as fechamov from jsmertramer where " _
            & " fechamov <= '" & ft.FormatoFechaMySQL(DateAdd("d", 1, FechaHasta)) & "' and " _
            & " tipomov in ('EN','AE') and " _
            & " origen in ('COM','INV') and " _
            & " id_emp = '" & jytsistema.WorkID & "' " _
            & " group by codart ")

        ft.Ejecutar_strSQL(myconn, " drop temporary table if exists " & tblCostos)
        ft.Ejecutar_strSQL(myconn, " create temporary table " & tblCostos & " (codart varchar(15) not null, " _
            & " fechamov datetime not null, tipomov char(2) not null, numdoc varchar(15) not null," _
            & " unidad char(3) not null, cantidad double(19,2) default '0.00' not null, " _
            & " peso double(10,3) default '0.000' not null, costounitario double(19,2) default '0.00' not null) ")
        ft.Ejecutar_strSQL(myconn, " create index " & tblCostos & " on " & tblCostos & " (codart)")
        ft.Ejecutar_strSQL(myconn, " insert into " & tblCostos & " select a.codart, a.fechamov, a.tipomov, a.numdoc, a.unidad, sum(a.cantidad) cantidad, sum(a.peso) peso, " _
            & " sum(a.costotaldes)/ if(  sum(a.cantidad) > 0, sum(a.cantidad) ,1)  costounitario from jsmertramer a, " & tbltramer & " b " _
            & " where (a.codart = b.codart and a.fechamov=b.fechamov) and " _
            & " a.fechamov <= '" & ft.FormatoFechaMySQL(DateAdd("d", 1, FechaHasta)) & "' and " _
            & " a.tipomov in ('EN','AE') and " _
            & " a.origen in ('COM','INV') and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by a.codart, a.numdoc, a.unidad " _
            & " order by a.codart ")

        SeleccionMERExistenciasAFecha = " select elt(a.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') AS mix, " _
            & " a.codart, a.nomart, a.alterno, a.presentacion,  a.unidad, c.descrip as categoria, d.descrip as marca, " _
            & " p.descrip division, e.descrip as Tipjer, " _
            & " if( b.existencia is null, 0.00, round( if ( abs(b.existencia) <= 0.01, 0.00, b.existencia) ,3 ) ) Cantidad, " _
            & " if( f.costounitario is null, 0.00,  f.costounitario) costotal, " _
            & " if( f.costounitario is null, 0.00,  f.costounitario) costounitario, " _
            & " if( f.costounitario is null, 0.00,  f.costounitario)*(" & porcentajeGastos & "/100) gastosScostos, " _
            & " a.precio_" & TarifaPrecio & " precio, " _
            & " a.pesounidad * if(b.existencia is null, 0.00, round( b.existencia,3 )) cantidadkilos, u.salidasdiarias, u.salidasdiarias*a.pesounidad salidaskilos " _
            & " from jsmerctainv a " _
            & " left join " & tblextReal & " b on (a.codart = b.codart) " _
            & " left join ( SELECT a.codart, a.fechamov, a.tipomov, a.numdoc, a.unidad, IF( c.equivale IS NULL, a.cantidad, a.cantidad/c.equivale) cantidad, " _
            & "             a.peso,  IF( c.equivale IS NULL, a.costounitario, c.equivale*a.costounitario) costounitario, c.equivale " _
            & "             FROM (SELECT a.codart, a.fechamov, a.tipomov, a.numdoc, a.unidad, SUM(a.cantidad) cantidad, SUM(a.peso) peso,  " _
            & "                         SUM(a.costotaldes)/ IF(  SUM(a.cantidad) > 0, SUM(a.cantidad) ,1)  costounitario " _
            & "                   FROM jsmertramer a, (SELECT codart, MAX(fechamov) AS fechamov FROM jsmertramer WHERE " _
            & "                     fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' AND " _
            & "                     tipomov IN ('EN','AE') AND  " _
            & "                     origen IN ('COM','INV') AND  " _
            & "                     id_emp = '" & jytsistema.WorkID & "' " _
            & "                   GROUP BY codart ) b " _
            & "             WHERE (a.codart = b.codart AND a.fechamov=b.fechamov) AND  " _
            & "             a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' AND " _
            & "             a.tipomov IN ('EN','AE') AND " _
            & "             a.origen IN ('COM','INV') AND " _
            & "             a.id_emp = '" & jytsistema.WorkID & "' " _
            & "             GROUP BY a.codart, a.numdoc, a.unidad  ) a " _
            & "             LEFT JOIN jsmerequmer c ON (a.codart = c.codart AND a.unidad = c.uvalencia AND c.id_emp = '" & jytsistema.WorkID & "')) f on (a.codart = f.codart) " _
            & " left join " & tblDiariaReal & " u on (a.codart = u.codart ) " _
            & " left join jsconctatab c on (a.grupo = c.codigo and a.id_emp = c.id_emp and c.modulo = '00002') " _
            & " left join jsconctatab d on (a.marca = d.codigo and a.id_emp = d.id_emp and d.modulo = '00003') " _
            & " left join jsmerencjer e on (a.tipjer = e.tipjer and a.id_emp = e.id_emp) " _
            & " left join jsmercatdiv p on (a.division = p.division and a.id_emp = p.id_emp) " _
            & " Where " _
            & str _
            & " a.precio_" & TarifaPrecio & " > 0  AND  " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by a.codart " _
            & strExistencia _
            & " order by a." & OrdenadoPor

    End Function

    Public Function SeleccionMERTransferencia(ByVal CodigoTransferencia As String) As String

        SeleccionMERTransferencia = " SELECT a.numtra, date_format(a.emision,'%d-%m-%Y') emision, a.alm_sale, a.alm_entra, a.comen, a.totaltra, " _
            & " a.totalcan, a.items, a.pesototal, a.tipo, " _
            & " b.renglon, b.item codart, b.descrip, b.unidad,  b.cantidad, b.peso, b.lote, b.costou, " _
            & " b.totren " _
            & " FROM jsmerenctra a " _
            & " LEFT JOIN jsmerrentra b ON (a.numtra = b.numtra AND a.id_emp = b.id_emp) " _
            & " WHERE " _
            & " a.numtra = '" & CodigoTransferencia & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' "

    End Function
    Public Function SeleccionMERPedidosAlmacen(ByVal CodigoPedido As String) As String

        Return " SELECT a.numped numtra, date_format(a.emision,'%d-%m-%Y') emision, a.alm_origen alm_sale, a.alm_destino alm_entra, a.comen, a.tot_ped, " _
            & "  a.items, " _
            & " b.renglon, b.item codart, b.descrip, b.unidad,  b.cantidad, b.peso, b.lote, b.costou, " _
            & " b.costotot totren " _
            & " FROM jsmerencped a " _
            & " LEFT JOIN jsmerrenped b ON (a.numped = b.numped AND a.id_emp = b.id_emp) " _
            & " WHERE " _
            & " a.numped = '" & CodigoPedido & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' "

    End Function
    Public Function SeleccionMERConteo(ByVal NumeroDeConteo As String, numConteo As Integer) As String

        SeleccionMERConteo = " SELECT a.conmer, date_format(a.fechacon, '%d-%m-%Y') fechacon, a.almacen, a.comentario, a.procesado, date_format(a.fechapro, '%d-%m-%Y') fechapro, " _
                & " b.codart, b.nomart, b.unidad, b.existencia, " _
                & " if( " & numConteo & " = 0  ,  b.cont1, if(" & numConteo & " = 1 , b.cont2, b.cont1 - b.cont2 ) ) conteo, " _
                & " b.cont1, b.cont2, b.cont3, b.cont4, b.cont5, " _
                & " b.costou, b.costo_tot " _
                & " FROM jsmerenccon a " _
                & " LEFT JOIN jsmerconmer b ON (a.conmer = b.conmer AND a.id_emp = b.id_emp) " _
                & " WHERE " _
                & " a.conmer = '" & NumeroDeConteo & "' AND  " _
                & IIf(numConteo = 2, " b.cont1 <> b.cont2 AND ", "") _
                & " a.id_emp = '" & jytsistema.WorkID & "' order by a.conmer, b.codart "

    End Function
    Public Function SeleccionMERPreciosFuturos() As String
        SeleccionMERPreciosFuturos = " select date_format(a.fecha, '%d-%m-%Y') fecha, a.codart, a.tipoprecio, a.monto, a.des_art, a.procesado, b.nomart, b.unidad " _
            & " from jsmerlispre a " _
            & " left join jsmerctainv b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
            & " Where " _
            & " a.procesado = 0 and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by codart, tipoprecio, fecha desc  "
    End Function
    Public Function SeleccionMERPreciosEspeciales(ByVal NumeroLista As String) As String
        SeleccionMERPreciosEspeciales = " SELECT a.codlis, a.descrip,  Date_format(a.emision, '%d-%m-%Y') emision , " _
            & " date_format(a.vence, '%d-%m-%Y') vence , b.codart, c.nomart, c.unidad, b.precio " _
            & " FROM jsmerenclispre a " _
            & " LEFT JOIN jsmerrenlispre b ON (a.codlis = b.codlis AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsmerctainv c ON (b.codart = c.codart AND b.id_emp = c.id_emp) " _
            & " WHERE " _
            & " a.codlis = '" & NumeroLista & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by b.codart "

    End Function

    Public Function SeleccionMEROfertas(ByVal NumeroOferta As String) As String
        SeleccionMEROfertas = " SELECT a.codofe, a.descrip, date_format(a.desde,'%d-%m-%Y') desde, date_format(a.hasta,'%d-%m-%Y') hasta, ELT(a.tarifa_a+1,'','A') TARIFA_A, " _
                & "  ELT(a.tarifa_b+1,'','B') TARIFA_B, ELT(a.tarifa_c+1,'','C') TARIFA_C, " _
                & "  ELT(a.tarifa_d+1,'','D') TARIFA_D, ELT(a.tarifa_e+1,'','E') TARIFA_E, " _
                & "  ELT(a.tarifa_f+1,'','F') TARIFA_F,  b.renglon, b.codart, b.descrip descripcion, b.unidad, " _
                & "  b.limitei, b.limites, b.porcentaje, ELT(b.otorgapor+1,'Datum', 'Asesor') otorgapor, " _
                & "  d.cantidad, d.cantidadbon, d.unidadbon, d.itembon, d.nombreitembon, d.cantidadinicio, " _
                & "  ELT(d.otorgacan+1,'Datum', 'Asesor') otorgacan " _
                & "  FROM jsmerencofe a " _
                & "  LEFT JOIN jsmerrenofe b ON (a.codofe = b.codofe AND a.id_emp = b.id_emp) " _
                & "  LEFT JOIN jsmerctainv c ON (b.codart = c.codart AND b.id_emp = c.id_emp) " _
                & "  LEFT JOIN jsmerrenbon d ON (b.codofe = d.codofe AND b.codart = d.codart AND b.id_emp = d.id_emp) " _
                & "  WHERE " _
                & "  a.codofe = '" & NumeroOferta & "' AND " _
                & "  a.id_emp = '" & jytsistema.WorkID & "'  " _
                & "  order by a.codofe, b.codart, b.renglon, d.renglon "

    End Function
    Public Function SeleccionMERJerarquia(ByVal TipoJerarquia As String) As String
        SeleccionMERJerarquia = " SELECT a.tipjer, a.descrip, " _
            & " IF( b.nivel = 1,  a.descrip1, IF( b.nivel = 2 , a.descrip2, IF( b.nivel = 3 , a.descrip3, IF( b.nivel = 4, a.descrip4, IF( b.nivel = 5, a.descrip5, a.descrip6 ) ) ) )) grupo  , " _
            & " b.nivel, b.codjer, b.desjer " _
            & " FROM jsmerencjer a " _
            & " LEFT JOIN jsmerrenjer b ON (a.tipjer = b.tipjer AND a.id_emp = b.id_emp) " _
            & " WHERE " _
            & " a.tipjer = '" & TipoJerarquia & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY a.tipjer, b.nivel, b.codjer "
    End Function
    Public Function SeleccionMERServicios() As String
        SeleccionMERServicios = "select a.codser, a.desser, a.tipoiva, if( b.monto is null, 0.00, b.monto) monto, a.precio, elt(a.tiposervicio +1, 'ISLR','NORMAL') tiposervicio, a.CODCON " _
            & " from jsmercatser a " _
            & " left join (" & SeleccionGENTablaIVA(jytsistema.sFechadeTrabajo) & ") b on (a.tipoiva = b.tipo) " _
            & " where a.id_emp = '" & jytsistema.WorkID & "' order by a.codser "
    End Function

    Function SeleccionMERVentasMercanciasPLANA(ByVal TipoJerarquiaDesde As String, ByVal TipoJerarquiaHasta As String, _
                                               ByVal FechaDesde As Date, ByVal FechaHasta As Date) As String

        Dim str As String = ""
        str += " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaDesde) & "' and "
        str += " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' and "
        If TipoJerarquiaDesde <> "" Then str += " b.tipjer >= '" & TipoJerarquiaDesde & "' and "
        If TipoJerarquiaHasta <> "" Then str += " b.tipjer <= '" & TipoJerarquiaHasta & "' and  "

        Return " SELECT a.vendedor, CONCAT(c.apellidos, ' ', c.nombres) nomVendedor, " _
            & " a.prov_cli, IF( d.nombre IS NULL, 'PUNTO DE VENTA', d.nombre) nomCliente, " _
            & " e.descrip tipoNegocio, a.codart, b.nomart, b.alterno, a.numdoc, a.fechamov, a.cantidad, a.unidad, " _
            & " ROUND(IF( ISNULL(f.uvalencia), a.cantidad, a.cantidad/f.equivale),2) cantidadUV, b.unidad UV, a.peso, " _
            & " a.costotaldes COSTO, a.ventotaldes VENTA, 0.0 CantidadDEV, 0.00 PesoDEV, 0.00 COSTODEV, 0.00 VENTADEV, g.descrip jerarquia, h.desjer grupo,  i.desjer segmento " _
            & " FROM jsmertramer a " _
            & " LEFT JOIN jsmerctainv b ON (a.codart = b.codart AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvencatven c ON (a.vendedor = c.codven AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsvencatcli d ON (a.prov_cli = d.codcli AND a.id_emp = d.id_emp) " _
            & " LEFT JOIN jsvenlistip e ON (d.unidad  = e.codigo AND d.id_emp = e.id_emp ) " _
            & " LEFT JOIN jsmerequmer f ON (a.codart = f.codart AND a.unidad = f.uvalencia AND a.id_emp = f.id_emp) " _
            & " LEFT JOIN jsmerencjer g ON (b.tipjer = g.tipjer AND  b.id_emp = g.id_emp ) " _
            & " LEFT JOIN jsmerrenjer h ON (b.tipjer = h.tipjer AND b.codjer1 = h.codjer AND b.id_emp =  h.id_emp) " _
            & " LEFT JOIN jsmerrenjer i ON (b.tipjer = i.tipjer AND b.codjer2 = i.codjer AND b.id_emp =  i.id_emp) " _
            & " WHERE " _
            & str _
            & " a.tipomov = 'SA' AND " _
            & " a.origen IN ('FAC', 'PFC', 'PVE','NDV') AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " UNION " _
            & " SELECT a.vendedor, CONCAT(c.apellidos, ' ', c.nombres) nomVendedor,  " _
            & " a.prov_cli, IF( d.nombre IS NULL, 'PUNTO DE VENTA', d.nombre) nomCliente,  " _
            & " e.descrip tipoNegocio, a.codart, b.nomart, b.alterno, a.numdoc, a.fechamov, a.cantidad, a.unidad, " _
            & " 0.00  cantidadUV, b.unidad UV, 0.00 peso, 0.00 COSTO, 0.00 VENTA, " _
            & " ROUND(IF( ISNULL(f.uvalencia), a.cantidad, a.cantidad/f.equivale),2) CantidadDEV, a.peso PesoDEV, a.costotaldes COSTODEV, a.ventotaldes VENTADEV, g.descrip jerarquia, h.desjer grupo,  i.desjer segmento " _
            & " FROM jsmertramer a " _
            & " LEFT JOIN jsmerctainv b ON (a.codart = b.codart AND a.id_emp = b.id_emp) " _
            & " LEFT JOIN jsvencatven c ON (a.vendedor = c.codven AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsvencatcli d ON (a.prov_cli = d.codcli AND a.id_emp = d.id_emp) " _
            & " LEFT JOIN jsvenlistip e ON (d.unidad  = e.codigo AND d.id_emp = e.id_emp ) " _
            & " LEFT JOIN jsmerequmer f ON (a.codart = f.codart AND a.unidad = f.uvalencia AND a.id_emp = f.id_emp) " _
            & " LEFT JOIN jsmerencjer g ON (b.tipjer = g.tipjer AND  b.id_emp = g.id_emp ) " _
            & " LEFT JOIN jsmerrenjer h ON (b.tipjer = h.tipjer AND b.codjer1 = h.codjer AND b.id_emp =  h.id_emp) " _
            & " LEFT JOIN jsmerrenjer i ON (b.tipjer = i.tipjer AND b.codjer2 = i.codjer AND b.id_emp =  i.id_emp) " _
            & " WHERE " _
            & str _
            & " a.tipomov = 'EN' AND " _
            & " a.origen IN ('NCV', 'PVE') AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY 1, 3, 9 "

    End Function

    Function SeleccionMERVentasMercancias(ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, ByVal OrdenadoPor As String, ByVal Operador As Integer, _
                                   Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                   Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                   Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                   Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                   Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                   Optional ByVal TipoNegocioDesde As String = "", Optional ByVal TipoNegocioHasta As String = "", _
                                   Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                   Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                   Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                   Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                   Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                   Optional ByVal AlmacenDesde As String = "", Optional ByVal AlmacenHasta As String = "", _
                                   Optional ByVal FechaEmisionDesde As Date = MyDate, Optional ByVal FechaEmisionHasta As Date = MyDate, _
                                   Optional ByVal ClienteDesde As String = "", Optional ByVal ClienteHasta As String = "", _
                                   Optional ByVal Estatus As Integer = 9, Optional ByVal Cartera As Integer = 9, _
                                   Optional ByVal SoloPeso As Boolean = False, Optional ByVal TipoMercancia As Integer = 99, _
                                   Optional ByVal Regulada As Integer = 9) As String

        Dim str As String = CadenaComplementoMercancias("b", MercanciaDesde, MercanciaHasta, Operador, OrdenadoPor, TipoJerarquia, Nivel1, _
                                                         Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, CategoriaDesde, CategoriaHasta, _
                                                         MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta, Estatus, _
                                                         Cartera, SoloPeso, TipoMercancia, Regulada)

        Dim strX As String = ""

        If AlmacenDesde <> "" Then str += " a.almacen >= '" & AlmacenDesde & "' and "
        If AlmacenHasta <> "" Then str += " a.almacen <= '" & AlmacenHasta & "' and "
        If ClienteDesde <> "" Then str += " a.prov_cli >= '" & ClienteDesde & "' and "
        If ClienteHasta <> "" Then str += " a.prov_cli <= '" & ClienteHasta & "' and "
        If CanalDesde <> "" Then str += " f.categoria >= '" & CanalDesde & "' and "
        If CanalHasta <> "" Then str += " f.categoria <= '" & CanalHasta & "' and "
        If TipoNegocioDesde <> "" Then str += " f.unidad >= '" & TipoNegocioDesde & "' and "
        If TipoNegocioHasta <> "" Then str += " f.unidad <= '" & TipoNegocioHasta & "' and "
        If AsesorDesde <> "" Then str += " a.vendedor >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " a.vendedor <= '" & AsesorHasta & "' and "

        Dim strOrden As String = IIf(OrdenadoPor = "CODART", "a.codart", "a.nomart")

        SeleccionMERVentasMercancias = "SELECT a.*, '' codrut, '' ruta, '' codzon, '' zona FROM (SELECT ELT(b.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') AS mix, a.codart, b.NOMART, " _
                & " b.unidad, c.descrip categoria, d.descrip marca, p.descrip division, e.descrip tipjer,  CONCAT(f.nombre,'(' , GROUP_CONCAT(a.NUMDOC) ,')') nombre, a.prov_cli, " _
                & " a.vendedor, g.descrip canal, h.descrip tiponegocio, " _
                & " concat(i.apellidos, ', ', i.nombres) asesor, " _
                & " SUM(IF( a.tipomov ='SA', IF( ISNULL(m.uvalencia), a.cantidad, a.cantidad/m.equivale),-1*IF( ISNULL(m.uvalencia), a.cantidad, a.cantidad/m.equivale))) AS CantidadTotal, " _
                & " SUM(IF( a.tipomov ='SA', a.peso,  -1*a.peso )) AS PesoTotal, " _
                & " SUM(IF( a.tipomov ='SA', a.costotaldes, -1*a.costotaldes )) AS CosTotal, " _
                & " SUM(IF( a.tipomov ='SA',a.ventotaldes,  -1*a.ventotaldes )) AS VenTotal,  " _
                & " f.rif, f.dirfiscal " _
                & " FROM jsmertramer a " _
                & " LEFT JOIN jsmerctainv b ON (a.codart = b.codart AND a.id_emp = b.id_emp ) " _
                & " LEFT JOIN jsconctatab c ON (b.grupo = c.codigo AND b.id_emp = c.id_emp AND c.modulo = '00002') " _
                & " LEFT JOIN jsconctatab d ON (b.marca = d.codigo AND b.id_emp = d.id_emp AND d.modulo = '00003') " _
                & " LEFT JOIN jsmercatdiv p ON (b.division = p.division AND b.id_emp = p.id_emp) " _
                & " LEFT JOIN jsmerencjer e ON (b.tipjer = e.tipjer AND b.id_emp = e.id_emp) " _
                & " LEFT JOIN jsvencatcli f ON (a.prov_cli = f.codcli AND a.id_emp = f.id_emp) " _
                & " LEFT JOIN jsvenliscan g ON (f.categoria = g.codigo AND a.id_emp = g.id_emp ) " _
                & " LEFT JOIN jsvenlistip h ON (f.unidad = h.codigo AND a.id_emp = h.id_emp AND g.codigo = h.antec ) " _
                & " LEFT JOIN jsmerequmer m ON (a.codart = m.codart AND a.unidad = m.uvalencia AND a.id_emp = m.id_emp) " _
                & " LEFT JOIN jsvencatven i on (a.vendedor = i.codven and a.id_emp = i.id_emp and i.tipo = 0 )" _
                & " WHERE " & str _
                & " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaEmisionDesde) & "' AND " _
                & " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaEmisionHasta) & "' AND " _
                & " a.origen IN ('FAC','NDV','PVE','PFC', 'NCV') AND " _
                & " a.tipomov IN ('EN','SA') AND " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " GROUP BY a.codart, a.prov_cli ) a " _
                & " order by " & strOrden

    End Function

    Function SeleccionMERVentasMercanciasAct(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                                            ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, ByVal OrdenadoPor As String, _
                                            Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                                            Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                                            Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                                            Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                                            Optional ByVal Estatus As Integer = 9, Optional ByVal Cartera As Integer = 9, _
                                            Optional ByVal SoloPeso As Boolean = False, Optional ByVal TipoMercancia As Integer = 99, _
                                            Optional ByVal Regulada As Integer = 9, _
                                            Optional ByVal CanalDesde As String = "", Optional ByVal CanalHasta As String = "", _
                                            Optional ByVal TiponegocioDesde As String = "", Optional ByVal TipoNegocioHasta As String = "", _
                                            Optional ByVal ZonaDesde As String = "", Optional ByVal ZonaHasta As String = "", _
                                            Optional ByVal RutaDesde As String = "", Optional ByVal RutaHasta As String = "", _
                                            Optional ByVal AsesorDesde As String = "", Optional ByVal AsesorHasta As String = "", _
                                            Optional ByVal Pais As String = "", Optional ByVal Estado As String = "", Optional ByVal Municipio As String = "", _
                                            Optional ByVal Parroquia As String = "", Optional ByVal Ciudad As String = "", Optional ByVal Barrio As String = "", _
                                            Optional ByVal EmisionDesde As Date = MyDate, Optional ByVal EmisionHasta As Date = MyDate, _
                                            Optional ByVal AlmacenDesde As String = "", Optional ByVal AlmacenHasta As String = "", _
                                            Optional ByVal ClienteDesde As String = "", Optional ByVal ClienteHasta As String = "", _
                                            Optional ByVal Resumido As Boolean = False, Optional ByVal siMeses As Boolean = False) As String


        Dim strC As String = CadenaComplementoMercancias("a", MercanciaDesde, MercanciaHasta, 0, OrdenadoPor, TipoJerarquia, Nivel1, _
                                                         Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, CategoriaDesde, CategoriaHasta, _
                                                         MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta, Estatus, _
                                                         Cartera, SoloPeso, TipoMercancia, Regulada)


        Dim str As String = ""

        If siMeses Then
            str += " year(b.fechamov) = " & Year(EmisionDesde) & " and "
        Else
            str += " b.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(EmisionDesde) & "' and "
            str += " b.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(EmisionHasta) & "' and "
        End If

        If AlmacenDesde <> "" Then str += "  b.almacen >= '" & AlmacenDesde & "' and "
        If AlmacenHasta <> "" Then str += " b.almacen <= '" & AlmacenHasta & "'  and "
        If ClienteDesde <> "" Then str += " b.prov_cli >= '" & ClienteDesde & "' and"
        If ClienteHasta <> "" Then str += " b.prov_cli <= '" & ClienteHasta & "' and "
        If AsesorDesde <> "" Then str += " b.vendedor >= '" & AsesorDesde & "' and "
        If AsesorHasta <> "" Then str += " b.vendedor <= '" & AsesorHasta & "' and "

        If CanalDesde <> "" Then strC += " f.categoria >= '" & CanalDesde & "' and  "
        If CanalHasta <> "" Then strC += " f.categoria <= '" & CanalHasta & "' and "
        If TiponegocioDesde <> "" Then strC += " f.unidad >= '" & TiponegocioDesde & "' and "
        If TipoNegocioHasta <> "" Then strC += " f.unidad <= '" & TipoNegocioHasta & "' and "
        If ZonaDesde <> "" Then strC += " f.zona >= '" & ZonaDesde & "' and "
        If ZonaHasta <> "" Then strC += " f.zona <= '" & ZonaHasta & "' and "
        If RutaDesde <> "" Then strC += " f.Ruta_visita >= '" & RutaDesde & "' and "
        If RutaHasta <> "" Then strC += " f.Ruta_visita <= '" & RutaHasta & "' and "
        If Pais <> "" Then strC += " f.fpais = '" & ValorEntero(Pais) & "' and "
        If Estado <> "" Then strC += " f.festado = '" & ValorEntero(Estado) & "' and "
        If Municipio <> "" Then strC += " f.fmunicipio = '" & ValorEntero(Municipio) & "' and "
        If Parroquia <> "" Then strC += " f.fparroquia = '" & ValorEntero(Parroquia) & "' and "
        If Ciudad <> "" Then strC += " f.fciudad = '" & ValorEntero(Ciudad) & "' and "
        If Barrio <> "" Then strC += " f.fbarrio = '" & ValorEntero(Barrio) & "' and "

        Dim StrMeses As String = ", 0 pv01, 0 pdbe01, 0 pdme01, 0 pn01, 0 act01,  " _
                      & " 0 pv02, 0 pdbe02, 0 pdme02, 0 pn02, 0 act02,  " _
                      & " 0 pv03, 0 pdbe03, 0 pdme03, 0 pn03, 0 act03,  " _
                      & " 0 pv04, 0 pdbe04, 0 pdme04, 0 pn04, 0 act04,  " _
                      & " 0 pv05, 0 pdbe05, 0 pdme05, 0 pn05, 0 act05,  " _
                      & " 0 pv06, 0 pdbe06, 0 pdme06, 0 pn06, 0 act06,  " _
                      & " 0 pv07, 0 pdbe07, 0 pdme07, 0 pn07, 0 act07,  " _
                      & " 0 pv08, 0 pdbe08, 0 pdme08, 0 pn08, 0 act08,  " _
                      & " 0 pv09, 0 pdbe09, 0 pdme09, 0 pn09, 0 act09,  " _
                      & " 0 pv10, 0 pdbe10, 0 pdme10, 0 pn10, 0 act10,  " _
                      & " 0 pv11, 0 pdbe11, 0 pdme11, 0 pn11, 0 act11,  " _
                      & " 0 pv12, 0 pdbe12, 0 pdme12, 0 pn12, 0 act12  "
        If siMeses Then _
            StrMeses = ", sum(if( b.tipomov = 'SA' and month(b.fechamov) = 1,  b.peso, 0)) as pv01, sum(if( b.tipomov = 'EN' and b.almacen = '00001' and month(b.fechamov) = 1,  -1*b.peso, 0)) as pdbe01, sum(if( b.tipomov = 'EN' and b.almacen = '00002' and month(b.fechamov) = 1,  -1*b.peso, 0)) as pdme01, sum(if( b.tipomov = 'SA' and month(b.fechamov) = 1, b.peso , 0)) - sum(if( b.tipomov = 'EN' and month(b.fechamov) = 1, b.peso, 0 )) as pn01, count(distinct( if ( month(b.fechamov) = 1, prov_cli, null))) as act01,  " _
                      & " sum(if( b.tipomov = 'SA' and month(b.fechamov) = 2,  b.peso, 0)) as pv02, sum(if( b.tipomov = 'EN' and b.almacen = '00001' and month(b.fechamov) = 2,  -1*b.peso, 0)) as pdbe02, sum(if( b.tipomov = 'EN' and b.almacen = '00002' and month(b.fechamov) = 2,  -1*b.peso, 0)) as pdme02, sum(if( b.tipomov = 'SA' and month(b.fechamov) = 2, b.peso , 0)) - sum(if( b.tipomov = 'EN' and month(b.fechamov) = 2, b.peso, 0 )) as pn02, count(distinct( if ( month(b.fechamov) = 2, prov_cli, null))) as act02,  " _
                      & " sum(if( b.tipomov = 'SA' and month(b.fechamov) = 3,  b.peso, 0)) as pv03, sum(if( b.tipomov = 'EN' and b.almacen = '00001' and month(b.fechamov) = 3,  -1*b.peso, 0)) as pdbe03, sum(if( b.tipomov = 'EN' and b.almacen = '00002' and month(b.fechamov) = 3,  -1*b.peso, 0)) as pdme03, sum(if( b.tipomov = 'SA' and month(b.fechamov) = 3, b.peso , 0)) - sum(if( b.tipomov = 'EN' and month(b.fechamov) = 3, b.peso, 0 )) as pn03, count(distinct( if ( month(b.fechamov) = 3, prov_cli, null))) as act03,  " _
                      & " sum(if( b.tipomov = 'SA' and month(b.fechamov) = 4,  b.peso, 0)) as pv04, sum(if( b.tipomov = 'EN' and b.almacen = '00001' and month(b.fechamov) = 4,  -1*b.peso, 0)) as pdbe04, sum(if( b.tipomov = 'EN' and b.almacen = '00002' and month(b.fechamov) = 4,  -1*b.peso, 0)) as pdme04, sum(if( b.tipomov = 'SA' and month(b.fechamov) = 4, b.peso , 0)) - sum(if( b.tipomov = 'EN' and month(b.fechamov) = 4, b.peso, 0 )) as pn04, count(distinct( if ( month(b.fechamov) = 4, prov_cli, null))) as act04,  " _
                      & " sum(if( b.tipomov = 'SA' and month(b.fechamov) = 5,  b.peso, 0)) as pv05, sum(if( b.tipomov = 'EN' and b.almacen = '00001' and month(b.fechamov) = 5,  -1*b.peso, 0)) as pdbe05, sum(if( b.tipomov = 'EN' and b.almacen = '00002' and month(b.fechamov) = 5,  -1*b.peso, 0)) as pdme05, sum(if( b.tipomov = 'SA' and month(b.fechamov) = 5, b.peso , 0)) - sum(if( b.tipomov = 'EN' and month(b.fechamov) = 5, b.peso, 0 )) as pn05, count(distinct( if ( month(b.fechamov) = 5, prov_cli, null))) as act05,  " _
                      & " sum(if( b.tipomov = 'SA' and month(b.fechamov) = 6,  b.peso, 0)) as pv06, sum(if( b.tipomov = 'EN' and b.almacen = '00001' and month(b.fechamov) = 6,  -1*b.peso, 0)) as pdbe06, sum(if( b.tipomov = 'EN' and b.almacen = '00002' and month(b.fechamov) = 6,  -1*b.peso, 0)) as pdme06, sum(if( b.tipomov = 'SA' and month(b.fechamov) = 6, b.peso , 0)) - sum(if( b.tipomov = 'EN' and month(b.fechamov) = 6, b.peso, 0 )) as pn06, count(distinct( if ( month(b.fechamov) = 6, prov_cli, null))) as act06,  " _
                      & " sum(if( b.tipomov = 'SA' and month(b.fechamov) = 7,  b.peso, 0)) as pv07, sum(if( b.tipomov = 'EN' and b.almacen = '00001' and month(b.fechamov) = 7,  -1*b.peso, 0)) as pdbe07, sum(if( b.tipomov = 'EN' and b.almacen = '00002' and month(b.fechamov) = 7,  -1*b.peso, 0)) as pdme07, sum(if( b.tipomov = 'SA' and month(b.fechamov) = 7, b.peso , 0)) - sum(if( b.tipomov = 'EN' and month(b.fechamov) = 7, b.peso, 0 )) as pn07, count(distinct( if ( month(b.fechamov) = 7, prov_cli, null))) as act07,  " _
                      & " sum(if( b.tipomov = 'SA' and month(b.fechamov) = 8,  b.peso, 0)) as pv08, sum(if( b.tipomov = 'EN' and b.almacen = '00001' and month(b.fechamov) = 8,  -1*b.peso, 0)) as pdbe08, sum(if( b.tipomov = 'EN' and b.almacen = '00002' and month(b.fechamov) = 8,  -1*b.peso, 0)) as pdme08, sum(if( b.tipomov = 'SA' and month(b.fechamov) = 8, b.peso , 0)) - sum(if( b.tipomov = 'EN' and month(b.fechamov) = 8, b.peso, 0 )) as pn08, count(distinct( if ( month(b.fechamov) = 8, prov_cli, null))) as act08,  " _
                      & " sum(if( b.tipomov = 'SA' and month(b.fechamov) = 9,  b.peso, 0)) as pv09, sum(if( b.tipomov = 'EN' and b.almacen = '00001' and month(b.fechamov) = 9,  -1*b.peso, 0)) as pdbe09, sum(if( b.tipomov = 'EN' and b.almacen = '00002' and month(b.fechamov) = 9,  -1*b.peso, 0)) as pdme09, sum(if( b.tipomov = 'SA' and month(b.fechamov) = 9, b.peso , 0)) - sum(if( b.tipomov = 'EN' and month(b.fechamov) = 9, b.peso, 0 )) as pn09, count(distinct( if ( month(b.fechamov) = 9, prov_cli, null))) as act09,  " _
                      & " sum(if( b.tipomov = 'SA' and month(b.fechamov) = 10,  b.peso, 0)) as pv10, sum(if( b.tipomov = 'EN' and b.almacen = '00001' and month(b.fechamov) = 10,  -1*b.peso, 0)) as pdbe10, sum(if( b.tipomov = 'EN' and b.almacen = '00002' and month(b.fechamov) = 10,  -1*b.peso, 0)) as pdme10, sum(if( b.tipomov = 'SA' and month(b.fechamov) = 10, b.peso , 0)) - sum(if( b.tipomov = 'EN' and month(b.fechamov) = 10, b.peso, 0 )) as pn10, count(distinct( if ( month(b.fechamov) = 10, prov_cli, null))) as act10,  " _
                      & " sum(if( b.tipomov = 'SA' and month(b.fechamov) = 11,  b.peso, 0)) as pv11, sum(if( b.tipomov = 'EN' and b.almacen = '00001' and month(b.fechamov) = 11,  -1*b.peso, 0)) as pdbe11, sum(if( b.tipomov = 'EN' and b.almacen = '00002' and month(b.fechamov) = 11,  -1*b.peso, 0)) as pdme11, sum(if( b.tipomov = 'SA' and month(b.fechamov) = 11, b.peso , 0)) - sum(if( b.tipomov = 'EN' and month(b.fechamov) = 11, b.peso, 0 )) as pn11, count(distinct( if ( month(b.fechamov) = 11, prov_cli, null))) as act11,  " _
                      & " sum(if( b.tipomov = 'SA' and month(b.fechamov) = 12,  b.peso, 0)) as pv12, sum(if( b.tipomov = 'EN' and b.almacen = '00001' and month(b.fechamov) = 12,  -1*b.peso, 0)) as pdbe12, sum(if( b.tipomov = 'EN' and b.almacen = '00002' and month(b.fechamov) = 12,  -1*b.peso, 0)) as pdme12, sum(if( b.tipomov = 'SA' and month(b.fechamov) = 12, b.peso , 0)) - sum(if( b.tipomov = 'EN' and month(b.fechamov) = 12, b.peso, 0 )) as pn12, count(distinct( if ( month(b.fechamov) = 12, prov_cli, null))) as act12  "

        Dim tbltramer As String = " select b.codart, b.prov_cli, sum(if( b.tipomov = 'SA',  b.peso, 0)) as pesoventas, " _
            & " sum(if( b.tipomov = 'SA',  if( isnull(m.uvalencia), b.cantidad, b.cantidad/m.equivale), 0)) as cantidadventas, " _
            & " sum(if( b.tipomov = 'SA',  b.costotaldes, 0)) as costoventas, " _
            & " sum(if( b.tipomov = 'SA',  b.ventotaldes, 0)) as ventasventas, " _
            & " sum(if( b.tipomov = 'EN' and b.almacen = '00001',  -1*b.peso, 0)) as pesodbe, " _
            & " sum(if( b.tipomov = 'EN' and b.almacen = '00001',  -1*if( isnull(m.uvalencia), b.cantidad, b.cantidad/m.equivale), 0)) as cantidaddbe, " _
            & " sum(if( b.tipomov = 'EN' and b.almacen = '00001',  -1*b.costotaldes, 0)) as costodbe, " _
            & " sum(if( b.tipomov = 'EN' and b.almacen = '00001',  -1*b.ventotaldes, 0)) as ventasdbe, " _
            & " sum(if( b.tipomov = 'EN' and b.almacen = '00002',  -1*b.peso, 0)) as pesodme, " _
            & " sum(if( b.tipomov = 'EN' and b.almacen = '00002',  -1*if( isnull(m.uvalencia), b.cantidad, b.cantidad/m.equivale), 0)) as cantidaddme, " _
            & " sum(if( b.tipomov = 'EN' and b.almacen = '00002',  -1*b.costotaldes, 0)) as costodme, " _
            & " sum(if( b.tipomov = 'EN' and b.almacen = '00002',  -1*b.ventotaldes, 0)) as ventasdme, " _
            & " (sum(if( b.tipomov = 'SA' , b.peso , 0)) - sum(if( b.tipomov = 'EN', b.peso, 0 ))) as pesoneto, " _
            & " (sum(if( b.tipomov = 'SA' , if( isnull(m.uvalencia), b.cantidad, b.cantidad/m.equivale) , 0)) - sum(if( b.tipomov = 'EN', if( isnull(m.uvalencia), b.cantidad, b.cantidad/m.equivale), 0 ))) as cantidadneta, " _
            & " (sum(if( b.tipomov = 'SA' , b.costotaldes , 0)) - sum(if( b.tipomov = 'EN', b.costotaldes, 0 ))) as costoneto, " _
            & " (sum(if( b.tipomov = 'SA' , b.ventotaldes , 0)) - sum(if( b.tipomov = 'EN', b.ventotaldes, 0 ))) as ventasnetas " _
            & StrMeses _
            & " from jsmertramer b " _
            & " left join jsmerequmer m on (b.codart = m.codart and b.unidad = m.uvalencia and b.id_emp = m.id_emp) " _
            & " where " & str _
            & " b.origen in ('FAC','PFC','PVE','NCV','NDV') and " _
            & " b.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by b.codart, b.prov_cli "

        SeleccionMERVentasMercanciasAct = " select elt(a.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') AS mix, a.codart, a.nomart, a.unidad, c.descrip categoria, d.descrip as marca, p.descrip division, e.descrip as tipjer, f.nombre , " _
            & " count(distinct b.prov_cli) activacion, b.prov_cli, f.vendedor, g.descrip as canal, h.descrip As tiponegocio, i.descrip as zona, j.nomrut As ruta, concat(f.vendedor,' ', k.apellidos, ', ', k.nombres) as asesor, " _
            & " n.nombre pais, o.nombre estado, m.nombre municipio, q.nombre parroquia, r.nombre ciudad, s.nombre barrio, " _
            & " 0.000 cantidadtotal, " _
            & " b.pesoventas, b.cantidadventas, b.costoventas, b.ventasventas, b.pesodbe, b.cantidaddbe, b.costodbe, b.ventasdbe, b.pesodme, b.cantidaddme, b.costodme, b.ventasdme, b.pesoneto, b.cantidadneta, b.costoneto, b.ventasnetas, " _
            & " b.pv01, b.pdbe01, b.pdme01, b.pn01, b.act01, b.pv02, b.pdbe02, b.pdme02, b.pn02, b.act02, b.pv03, b.pdbe03, b.pdme03, b.pn03, b.act03, b.pv04, b.pdbe04, b.pdme04, b.pn04, b.act04, b.pv05, b.pdbe05, b.pdme05, b.pn05, b.act05, b.pv06, b.pdbe06, b.pdme06, b.pn06, b.act06, " _
            & " b.pv07, b.pdbe07, b.pdme07, b.pn07, b.act07, b.pv08, b.pdbe08, b.pdme08, b.pn08, b.act08, b.pv09, b.pdbe09, b.pdme09, b.pn09, b.act09, b.pv10, b.pdbe10, b.pdme10, b.pn10, b.act10, b.pv11, b.pdbe11, b.pdme11, b.pn11, b.act11, b.pv12, b.pdbe12, b.pdme12, b.pn12, b.act12 " _
            & " from jsmerctainv a " _
            & " left join (" & tbltramer & ") b on (a.codart = b.codart) " _
            & " left join jsconctatab c on (a.grupo = c.codigo and a.id_emp = c.id_emp and c.modulo = '00002') " _
            & " left join jsconctatab d on (a.marca = d.codigo and a.id_emp = d.id_emp and d.modulo = '00003') " _
            & " left join jsmercatdiv p on (a.division = p.division and a.id_emp = p.id_emp) " _
            & " left join jsmerencjer e on (a.tipjer = e.tipjer and a.id_emp = e.id_emp) " _
            & " left join jsvencatcli f on (b.prov_cli = f.codcli and a.id_emp = f.id_emp) " _
            & " left join jsvenliscan g on (f.categoria = g.codigo and f.id_emp = g.id_emp) " _
            & " left join jsvenlistip h on (f.unidad = h.codigo and f.id_emp = h.id_emp) " _
            & " left join jsconctatab i on (f.zona = i.codigo and f.id_emp = i.id_emp and i.modulo = '00005') " _
            & " left join jsvenencrut j on (f.ruta_visita = j.codrut and f.id_emp = j.id_emp and j.tipo = '0')" _
            & " left join jsvencatven k on (f.vendedor = k.codven and f.id_emp = k.id_emp ) " _
            & " left join jsconcatter n on (f.fpais = n.codigo and f.id_emp = n.id_emp ) " _
            & " left join jsconcatter o on (f.festado = o.codigo and a.id_emp = o.id_emp ) " _
            & " left join jsconcatter m on (f.fmunicipio = m.codigo and a.id_emp = m.id_emp ) " _
            & " left join jsconcatter q on (f.fparroquia = q.codigo and a.id_emp = q.id_emp ) " _
            & " left join jsconcatter r on (f.fciudad = r.codigo and a.id_emp = r.id_emp ) " _
            & " left join jsconcatter s on (f.fbarrio = s.codigo and a.id_emp = s.id_emp ) " _
            & " Where " _
            & strC _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by a.codart, b.prov_cli " _
            & " having activacion > 0 "

    End Function

    Public Function SeleccionMERListadoTransferencias(ByVal Desde As Date, ByVal Hasta As Date, _
                                                      CausaDesde As String, CausaHasta As String) As String

        Dim str As String = ""
        If CausaDesde <> "" Then str += " a.causa >= '" & CausaDesde & "' and "
        If CausaHasta <> "" Then str += " a.causa <= '" & CausaHasta & "' and "

        SeleccionMERListadoTransferencias = " SELECT a.emision, a.alm_sale, b.desalm almacensalida," _
            & " a.alm_entra, c.desalm almacenentrada, a.comen, a.causa, a.totaltra, a.totalcan, a.items, a.pesototal, a.tipo, d.* " _
            & " FROM jsmerenctra a " _
            & " LEFT JOIN jsmercatalm b ON (a.alm_sale = b.codalm AND a.id_emp = b.id_emp)  " _
            & " LEFT JOIN jsmercatalm c ON (a.alm_entra = c.codalm AND a.id_emp = c.id_emp) " _
            & " LEFT JOIN jsmerrentra d ON (a.numtra = d.numtra AND a.id_emp = d.id_emp)" _
            & " WHERE " _
            & str _
            & " a.emision >= '" & ft.FormatoFechaMySQL(Desde) & "' AND " _
            & " a.emision <= '" & ft.FormatoFechaMySQL(Hasta) & "' AND " _
            & " a.id_emp  = '" & jytsistema.WorkID & "' ORDER BY a.numtra "

    End Function

    Function SeleccionMERMovimientosMercanciasResumen(ByVal CodigoDesde As String, ByVal CodigoHasta As String, _
            ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal AlmacenDesde As String, ByVal AlmacenHasta As String, _
            Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
            Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
            Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
            Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", _
            Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", _
            Optional ByVal Nivel6 As String = "") As String

        Dim str As String = CadenaComplementoMercancias("b", CodigoDesde, CodigoHasta, "0", "codart", TipoJerarquia, Nivel1, Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, _
                                                            CategoriaDesde, CategoriaHasta, MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta)
        Dim strX As String = ""
        Dim strAlm As String = ""
        Dim strAlmx As String = ""

        If CodigoDesde <> "" Then str = str & " a.codart >= '" & CodigoDesde & "' and "
        If CodigoHasta <> "" Then str = str & " a.codart <= '" & CodigoHasta & "' and "

        If CodigoDesde <> "" Then strX = strX & " b.codart >= '" & CodigoDesde & "' and "
        If CodigoHasta <> "" Then strX = strX & " b.codart <= '" & CodigoHasta & "' and "

        If AlmacenDesde <> "" Then strAlm = strAlm & " a.almacen >= '" & AlmacenDesde & "' and "
        If AlmacenHasta <> "" Then strAlm = strAlm & " a.almacen <= '" & AlmacenHasta & "' and "

        If AlmacenDesde <> "" Then strAlmx = strAlmx & " b.almacen >= '" & AlmacenDesde & "' and "
        If AlmacenHasta <> "" Then strAlmx = strAlmx & " b.almacen <= '" & AlmacenHasta & "' and "

        SeleccionMERMovimientosMercanciasResumen = "SELECT a.codart, b.nomart, b.unidad,  SUM(IF( " & strAlm & " a.tipomov = 'EN' AND origen = 'INV',  a.cantidad, 0.000) ) entradas," _
            & " SUM(IF( origen = 'PVE', IF( a.tipomov = 'SA',  a.cantidad, -1*a.cantidad) , 0.000) ) salidas, if( e.existencia is null, 0.00, e.existencia)  existencias,  " _
            & " SUM(a.ventotaldes) preciototal, " _
            & " ROUND( SUM(a.ventotaldes) / SUM(IF( origen = 'PVE', IF( a.tipomov = 'SA',  a.cantidad, -1*a.cantidad) , 0.000) ),2) preciopromedio " _
            & " FROM jsmertramer a " _
            & " LEFT JOIN jsmerctainv b ON (a.codart = b.codart AND a.id_emp = b.id_emp) " _
            & " left join (SELECT a.codart, c.unidad, SUM(IF( ISNULL(f.uvalencia), a.existencia, a.existencia/f.equivale)) existencia, a.id_emp " _
            & "             FROM (SELECT  b.codart, b.unidad, SUM(IF( b.TIPOMOV IN( 'EN', 'AE', 'DV') , b.CANTIDAD, -1 * b.CANTIDAD )) existencia, b.id_emp " _
            & "                     FROM jsmertramer b " _
            & "                         Where " _
            & strAlmx & strX _
            & "                     b.tipomov <> 'AC' AND " _
            & "                     b.id_emp = '" & jytsistema.WorkID & "' AND " _
            & "                     b.ejercicio = '" & jytsistema.WorkExercise & "' AND " _
            & "                     b.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' " _
            & "                     GROUP BY b.codart, b.unidad ) a " _
            & "             LEFT JOIN jsmerequmer f ON (a.codart = f.codart AND a.unidad = f.uvalencia AND a.id_emp = f.id_emp ) " _
            & "             LEFT JOIN jsmerctainv c ON (a.codart = c.codart AND a.id_emp = c.id_emp) " _
            & "             group by a.codart) e on (a.codart = e.codart and a.id_emp = e.id_emp)  " _
            & " Where " _
            & str _
            & " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(FechaDesde) & "' AND " _
            & " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " GROUP BY a.codart "

    End Function

    Function SeleccionMERComprasMercancias(ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, _
            Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
            Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
            Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
            Optional ByVal UnidadProvDesde As String = "", Optional ByVal UnidadProvHasta As String = "", _
            Optional ByVal CategoriaProvDesde As String = "", Optional ByVal CategoriaProvHasta As String = "", _
            Optional ByVal EmisionDesde As Date = MyDate, Optional ByVal EmisionHasta As Date = MyDate, _
            Optional ByVal AlmacenDesde As String = "", Optional ByVal AlmacenHasta As String = "", _
            Optional ByVal ProveedorDesde As String = "", Optional ByVal ProveedorHasta As String = "", _
            Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", _
            Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "") As String

        Dim strSQL As String = CadenaComplementoMercancias("b", MercanciaDesde, MercanciaHasta, "0", "codart", TipoJerarquia, Nivel1, Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, _
                                                            CategoriaDesde, CategoriaHasta, MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta)


        strSQL += " a.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(EmisionDesde) & "' AND "
        strSQL += " a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(EmisionHasta) & "' AND "

        If ProveedorDesde <> "" Then strSQL += " a.prov_cli >= '" & ProveedorDesde & "' AND "
        If ProveedorHasta <> "" Then strSQL += " a.prov_cli <= '" & ProveedorHasta & "' AND "
        If AlmacenDesde <> "" Then strSQL += " a.almacen >= '" & AlmacenDesde & "' AND "
        If AlmacenHasta <> "" Then strSQL += "  a.almacen <= '" & AlmacenHasta & "' AND "

        If CategoriaProvDesde <> "" Then strSQL += " f.categoria >= '" & CategoriaProvDesde & "' and "
        If CategoriaProvHasta <> "" Then strSQL += " f.categoria <= '" & CategoriaProvHasta & "' and "
        If UnidadProvDesde <> "" Then strSQL += " f.unidad >= '" & UnidadProvDesde & "' and "
        If UnidadProvHasta <> "" Then strSQL += " f.unidad <= '" & UnidadProvHasta & "' and "

        SeleccionMERComprasMercancias = "select elt(b.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') mix, " _
            & " a.codart, b.nomart, b.unidad, c.descrip as categoria, d.descrip as marca, p.descrip division, " _
            & " e.descrip as tipjer, f.nombre, a.prov_cli, a.vendedor, g.descrip categoriaprov, " _
            & " h.descrip unidadnegocioprov, " _
            & " SUM(if( a.origen in ('COM','REC','NDC'), if( isnull(m.uvalencia), a.cantidad, a.cantidad/m.equivale),-1 * if( isnull(m.uvalencia), a.cantidad, a.cantidad/m.equivale))) AS cantidadtotal, " _
            & " SUM(if( a.origen in ('COM','REC','NDC'), if( a.tipomov <> 'AC', a.peso, 0) ,  -1 * if( a.tipomov <> 'AC', a.peso, 0) )) AS pesototal, " _
            & " SUM(if( a.origen in ('COM','REC','NDC'), if( a.tipomov <> 'AC', a.costotaldes, 0), -1 *if( a.tipomov <> 'AC', a.costotaldes, 0))) as costotal " _
            & " from jsmertramer a " _
            & " left join jsmerctainv b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
            & " left join jsconctatab c on (b.grupo = c.codigo and b.id_emp = c.id_emp and c.modulo = '00002') " _
            & " left join jsconctatab d on (b.marca = d.codigo and b.id_emp = d.id_emp and d.modulo = '00003') " _
            & " left join jsmercatdiv p on (b.division = p.division and b.id_emp = p.id_emp) " _
            & " left join jsmerencjer e on (b.tipjer = e.tipjer and b.id_emp = e.id_emp) " _
            & " left join jsprocatpro f on (a.prov_cli = f.codpro and a.id_emp = f.id_emp) " _
            & " left join jsproliscat g on (f.categoria = g.codigo and f.id_emp = g.id_emp) " _
            & " left join jsprolisuni h on (f.unidad = h.codigo and f.id_emp = h.id_emp) " _
            & " left join jsmerequmer m on (a.codart = m.codart and a.unidad = m.uvalencia and a.id_emp = m.id_emp)" _
            & " Where " _
            & strSQL & "  " _
            & " a.ORIGEN IN ('COM','REC','NDC','NCC')  and a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by  a.codart " _
            & " order by  a.codart "

    End Function

    Function SeleccionMERComprasMercanciasAct(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                ByVal MercanciaDesde As String, ByVal MercanciaHasta As String, _
                Optional ByVal CategoriaDesde As String = "", Optional ByVal CategoriaHasta As String = "", _
                Optional ByVal MarcaDesde As String = "", Optional ByVal MarcaHasta As String = "", _
                Optional ByVal DivisionDesde As String = "", Optional ByVal DivisionHasta As String = "", _
                Optional ByVal UnidadDesde As String = "", Optional ByVal UnidadHasta As String = "", _
                Optional ByVal CategoriaProvDesde As String = "", Optional ByVal CategoriaProvHasta As String = "", _
                Optional ByVal EmisionDesde As Date = MyDate, Optional ByVal EmisionHasta As Date = MyDate, _
                Optional ByVal AlmacenDesde As String = "", Optional ByVal AlmacenHasta As String = "", _
                Optional ByVal ProveedorDesde As String = "", Optional ByVal ProveedorHasta As String = "", _
                Optional ByVal TipoJerarquia As String = "", Optional ByVal Nivel1 As String = "", Optional ByVal Nivel2 As String = "", _
                Optional ByVal Nivel3 As String = "", Optional ByVal Nivel4 As String = "", Optional ByVal Nivel5 As String = "", Optional ByVal Nivel6 As String = "", _
                Optional ByVal siMeses As Boolean = False) As String

        Dim strSQL As String
        Dim StrMeses As String = ""


        strSQL = CadenaComplementoMercancias("a", MercanciaDesde, MercanciaHasta, 0, "codart", TipoJerarquia, _
                                                       Nivel1, Nivel2, Nivel3, Nivel4, Nivel5, Nivel6, CategoriaDesde, _
                                                       CategoriaHasta, MarcaDesde, MarcaHasta, DivisionDesde, DivisionHasta)

        If siMeses Then
            strSQL += " year(b.fechamov) = " & Year(EmisionDesde) & " AND "
        Else
            strSQL += " b.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(EmisionDesde) & "' AND "
            strSQL += " b.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(EmisionHasta) & "' AND "
        End If

        If AlmacenDesde <> "" Then strSQL += " b.almacen >= '" & AlmacenDesde & "' AND "
        If AlmacenHasta <> "" Then strSQL += " b.almacen <= '" & AlmacenHasta & "' AND "

        If CategoriaProvDesde <> "" Then strSQL += " f.categoria >= '" & CategoriaProvDesde & "' AND "
        If CategoriaProvHasta <> "" Then strSQL += " f.categoria <= '" & CategoriaProvHasta & "' AND "
        If UnidadDesde <> "" Then strSQL += " f.unidad >= '" & UnidadDesde & "' AND "
        If UnidadHasta <> "" Then strSQL += " f.unidad <= '" & UnidadHasta & "' AND "
        If ProveedorDesde <> "" Then strSQL += " b.prov_cli >= '" & ProveedorDesde & "' AND "
        If ProveedorHasta <> "" Then strSQL += " b.prov_cli <= '" & ProveedorHasta & "' AND "

        '    If Not Resumido Then strSQLREsumen = ", b.prov_cli "

        If siMeses Then _
            StrMeses = " sum(if( b.tipomov = 'EN' and month(b.fechamov) = 1,  b.peso, 0)) as pc01, sum(if( b.tipomov = 'SA' and b.almacen = '00001' and month(b.fechamov) = 1,  -1*b.peso, 0)) as pdbe01, sum(if( b.tipomov = 'SA' and b.almacen = '00002' and month(b.fechamov) = 1,  -1*b.peso, 0)) as pdme01, sum(if( b.tipomov = 'EN' and month(b.fechamov) = 1, b.peso , 0)) - sum(if( b.tipomov = 'SA' and month(b.fechamov) = 1, b.peso, 0 )) as pn01, count(distinct( if ( month(b.fechamov) = 1, prov_cli, null))) as act01,  " _
            & " sum(if( b.tipomov = 'EN' and month(b.fechamov) = 2,  b.peso, 0)) as pc02, sum(if( b.tipomov = 'SA' and b.almacen = '00001' and month(b.fechamov) = 2,  -1*b.peso, 0)) as pdbe02, sum(if( b.tipomov = 'SA' and b.almacen = '00002' and month(b.fechamov) = 2,  -1*b.peso, 0)) as pdme02, sum(if( b.tipomov = 'EN' and month(b.fechamov) = 2, b.peso , 0)) - sum(if( b.tipomov = 'SA' and month(b.fechamov) = 2, b.peso, 0 )) as pn02, count(distinct( if ( month(b.fechamov) = 2, prov_cli, null))) as act02,  " _
            & " sum(if( b.tipomov = 'EN' and month(b.fechamov) = 3,  b.peso, 0)) as pc03, sum(if( b.tipomov = 'SA' and b.almacen = '00001' and month(b.fechamov) = 3,  -1*b.peso, 0)) as pdbe03, sum(if( b.tipomov = 'SA' and b.almacen = '00002' and month(b.fechamov) = 3,  -1*b.peso, 0)) as pdme03, sum(if( b.tipomov = 'EN' and month(b.fechamov) = 3, b.peso , 0)) - sum(if( b.tipomov = 'SA' and month(b.fechamov) = 3, b.peso, 0 )) as pn03, count(distinct( if ( month(b.fechamov) = 3, prov_cli, null))) as act03,  " _
            & " sum(if( b.tipomov = 'EN' and month(b.fechamov) = 4,  b.peso, 0)) as pc04, sum(if( b.tipomov = 'SA' and b.almacen = '00001' and month(b.fechamov) = 4,  -1*b.peso, 0)) as pdbe04, sum(if( b.tipomov = 'SA' and b.almacen = '00002' and month(b.fechamov) = 4,  -1*b.peso, 0)) as pdme04, sum(if( b.tipomov = 'EN' and month(b.fechamov) = 4, b.peso , 0)) - sum(if( b.tipomov = 'SA' and month(b.fechamov) = 4, b.peso, 0 )) as pn04, count(distinct( if ( month(b.fechamov) = 4, prov_cli, null))) as act04,  " _
            & " sum(if( b.tipomov = 'EN' and month(b.fechamov) = 5,  b.peso, 0)) as pc05, sum(if( b.tipomov = 'SA' and b.almacen = '00001' and month(b.fechamov) = 5,  -1*b.peso, 0)) as pdbe05, sum(if( b.tipomov = 'SA' and b.almacen = '00002' and month(b.fechamov) = 5,  -1*b.peso, 0)) as pdme05, sum(if( b.tipomov = 'EN' and month(b.fechamov) = 5, b.peso , 0)) - sum(if( b.tipomov = 'SA' and month(b.fechamov) = 5, b.peso, 0 )) as pn05, count(distinct( if ( month(b.fechamov) = 5, prov_cli, null))) as act05,  " _
            & " sum(if( b.tipomov = 'EN' and month(b.fechamov) = 6,  b.peso, 0)) as pc06, sum(if( b.tipomov = 'SA' and b.almacen = '00001' and month(b.fechamov) = 6,  -1*b.peso, 0)) as pdbe06, sum(if( b.tipomov = 'SA' and b.almacen = '00002' and month(b.fechamov) = 6,  -1*b.peso, 0)) as pdme06, sum(if( b.tipomov = 'EN' and month(b.fechamov) = 6, b.peso , 0)) - sum(if( b.tipomov = 'SA' and month(b.fechamov) = 6, b.peso, 0 )) as pn06, count(distinct( if ( month(b.fechamov) = 6, prov_cli, null))) as act06,  " _
            & " sum(if( b.tipomov = 'EN' and month(b.fechamov) = 7,  b.peso, 0)) as pc07, sum(if( b.tipomov = 'SA' and b.almacen = '00001' and month(b.fechamov) = 7,  -1*b.peso, 0)) as pdbe07, sum(if( b.tipomov = 'SA' and b.almacen = '00002' and month(b.fechamov) = 7,  -1*b.peso, 0)) as pdme07, sum(if( b.tipomov = 'EN' and month(b.fechamov) = 7, b.peso , 0)) - sum(if( b.tipomov = 'SA' and month(b.fechamov) = 7, b.peso, 0 )) as pn07, count(distinct( if ( month(b.fechamov) = 7, prov_cli, null))) as act07,  " _
            & " sum(if( b.tipomov = 'EN' and month(b.fechamov) = 8,  b.peso, 0)) as pc08, sum(if( b.tipomov = 'SA' and b.almacen = '00001' and month(b.fechamov) = 8,  -1*b.peso, 0)) as pdbe08, sum(if( b.tipomov = 'SA' and b.almacen = '00002' and month(b.fechamov) = 8,  -1*b.peso, 0)) as pdme08, sum(if( b.tipomov = 'EN' and month(b.fechamov) = 8, b.peso , 0)) - sum(if( b.tipomov = 'SA' and month(b.fechamov) = 8, b.peso, 0 )) as pn08, count(distinct( if ( month(b.fechamov) = 8, prov_cli, null))) as act08,  " _
            & " sum(if( b.tipomov = 'EN' and month(b.fechamov) = 9,  b.peso, 0)) as pc09, sum(if( b.tipomov = 'SA' and b.almacen = '00001' and month(b.fechamov) = 9,  -1*b.peso, 0)) as pdbe09, sum(if( b.tipomov = 'SA' and b.almacen = '00002' and month(b.fechamov) = 9,  -1*b.peso, 0)) as pdme09, sum(if( b.tipomov = 'EN' and month(b.fechamov) = 9, b.peso , 0)) - sum(if( b.tipomov = 'SA' and month(b.fechamov) = 9, b.peso, 0 )) as pn09, count(distinct( if ( month(b.fechamov) = 9, prov_cli, null))) as act09,  " _
            & " sum(if( b.tipomov = 'EN' and month(b.fechamov) = 10,  b.peso, 0)) as pc10, sum(if( b.tipomov = 'SA' and b.almacen = '00001' and month(b.fechamov) = 10,  -1*b.peso, 0)) as pdbe10, sum(if( b.tipomov = 'SA' and b.almacen = '00002' and month(b.fechamov) = 10,  -1*b.peso, 0)) as pdme10, sum(if( b.tipomov = 'EN' and month(b.fechamov) = 10, b.peso , 0)) - sum(if( b.tipomov = 'SA' and month(b.fechamov) = 10, b.peso, 0 )) as pn10, count(distinct( if ( month(b.fechamov) = 10, prov_cli, null))) as act10,  " _
            & " sum(if( b.tipomov = 'EN' and month(b.fechamov) = 11,  b.peso, 0)) as pc11, sum(if( b.tipomov = 'SA' and b.almacen = '00001' and month(b.fechamov) = 11,  -1*b.peso, 0)) as pdbe11, sum(if( b.tipomov = 'SA' and b.almacen = '00002' and month(b.fechamov) = 11,  -1*b.peso, 0)) as pdme11, sum(if( b.tipomov = 'EN' and month(b.fechamov) = 11, b.peso , 0)) - sum(if( b.tipomov = 'SA' and month(b.fechamov) = 11, b.peso, 0 )) as pn11, count(distinct( if ( month(b.fechamov) = 11, prov_cli, null))) as act11,  " _
            & " sum(if( b.tipomov = 'EN' and month(b.fechamov) = 12,  b.peso, 0)) as pc12, sum(if( b.tipomov = 'SA' and b.almacen = '00001' and month(b.fechamov) = 12,  -1*b.peso, 0)) as pdbe12, sum(if( b.tipomov = 'SA' and b.almacen = '00002' and month(b.fechamov) = 12,  -1*b.peso, 0)) as pdme12, sum(if( b.tipomov = 'EN' and month(b.fechamov) = 12, b.peso , 0)) - sum(if( b.tipomov = 'SA' and month(b.fechamov) = 12, b.peso, 0 )) as pn12, count(distinct( if ( month(b.fechamov) = 12, prov_cli, null))) as act12,  "

        Dim str As String

        str = " sum(if( b.tipomov = 'EN',  b.peso, 0)) as pesoventas, " _
        & " sum(if( b.tipomov = 'EN',  if( isnull(m.uvalencia), b.cantidad, b.cantidad/m.equivale), 0)) as cantidadventas, " _
        & " sum(if( b.tipomov = 'EN',  b.costotaldes, 0)) as costoventas, " _
        & " sum(if( b.tipomov = 'EN',  b.ventotaldes, 0)) as ventasventas, " _
        & " sum(if( b.tipomov = 'SA' and b.almacen = '00001',  -1*b.peso, 0)) as pesodbe, " _
        & " sum(if( b.tipomov = 'SA' and b.almacen = '00001',  -1*if( isnull(m.uvalencia), b.cantidad, b.cantidad/m.equivale), 0)) as cantidaddbe, " _
        & " sum(if( b.tipomov = 'SA' and b.almacen = '00001',  -1*b.costotaldes, 0)) as costodbe, " _
        & " sum(if( b.tipomov = 'SA' and b.almacen = '00001',  -1*b.ventotaldes, 0)) as ventasdbe, " _
        & " sum(if( b.tipomov = 'SA' and b.almacen = '00002',  -1*b.peso, 0)) as pesodme, " _
        & " sum(if( b.tipomov = 'SA' and b.almacen = '00002',  -1*if( isnull(m.uvalencia), b.cantidad, b.cantidad/m.equivale), 0)) as cantidaddme, " _
        & " sum(if( b.tipomov = 'SA' and b.almacen = '00002',  -1*b.costotaldes, 0)) as costodme, " _
        & " sum(if( b.tipomov = 'SA' and b.almacen = '00002',  -1*b.ventotaldes, 0)) as ventasdme, " _
        & " (sum(if( b.tipomov = 'EN' , b.peso , 0)) - sum(if( b.tipomov = 'SA', b.peso, 0 ))) as pesoneto, " _
        & " (sum(if( b.tipomov = 'EN' , if( isnull(m.uvalencia), b.cantidad, b.cantidad/m.equivale) , 0)) - sum(if( b.tipomov = 'SA', if( isnull(m.uvalencia), b.cantidad, b.cantidad/m.equivale), 0 ))) as cantidadneta, " _
        & " (sum(if( b.tipomov = 'EN' , b.costotaldes, 0)) - sum(if( b.tipomov = 'SA', b.costotaldes, 0 ))) as costoneto, " _
        & " (sum(if( b.tipomov = 'EN' , b.ventotaldes, 0)) - sum(if( b.tipomov = 'SA', b.ventotaldes, 0 ))) as ventasnetas, "


        SeleccionMERComprasMercanciasAct = " select elt(a.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') AS mix, a.codart, a.nomart, a.unidad, c.descrip categoria, d.descrip as marca, p.descrip division, e.descrip as tipjer, f.nombre , " _
                    & str _
                    & " count(distinct b.prov_cli) activacion, " & StrMeses & " b.prov_cli, g.descrip categoriaprov, h.descrip unidadnegocioprov, " _
                    & " SUM(if( b.tipomov = 'EN', if( isnull(m.uvalencia), b.cantidad, b.cantidad/m.equivale),-1 * if( isnull(m.uvalencia), b.cantidad, b.cantidad/m.equivale))) AS cantidadtotal " _
                    & " from jsmerctainv a " _
                    & " left join jsmertramer b on (a.codart = b.codart and a.id_emp = b.id_emp) " _
                    & " left join jsconctatab c on (a.grupo = c.codigo and a.id_emp = c.id_emp and c.modulo = '00002') " _
                    & " left join jsconctatab d on (a.marca = d.codigo and a.id_emp = d.id_emp and d.modulo = '00003') " _
                    & " left join jsmercatdiv p on (a.division = p.division and a.id_emp = p.id_emp) " _
                    & " left join jsmerencjer e on (a.tipjer = e.tipjer and a.id_emp = e.id_emp) " _
                    & " left join jsprocatpro f on (b.prov_cli = f.codpro and b.id_emp = f.id_emp) " _
                    & " left join jsproliscat g on (f.categoria = g.codigo and f.id_emp = g.id_emp) " _
                    & " left join jsprolisuni h on (f.unidad = h.codigo and f.id_emp = h.id_emp) " _
                    & " left join jsmerequmer m on (b.codart = m.codart and b.unidad = m.uvalencia and b.id_emp = m.id_emp) " _
                    & " Where " & strSQL _
                    & " a.id_emp = '" & jytsistema.WorkID & "' and b.origen in('COM','REC','NCC','NDC') " _
                    & " group by a.codart, b.prov_cli order by a.codart, b.prov_cli "


    End Function


    '///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    '*******************************************************************************************************************************
    '///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Function SeleccionMERCAS_Mercancias(strMercancias As String, _
                            strCategorias As String, strMarcas As String, strDivisiones As String, _
                            strJerarquias As String, strNivel1 As String, strNivel2 As String, _
                            strNivel3 As String, strNivel4 As String, strNivel5 As String, strNivel6 As String, _
                            strAlmacen As String, _
                            Optional ByVal Estatus As Integer = 9, Optional ByVal Cartera As Integer = 9, _
                            Optional ByVal SoloPeso As Boolean = False, Optional ByVal TipoMercancia As Integer = 99, _
                            Optional ByVal Regulada As Integer = 9, Optional ByVal TarifaA As Boolean = True, _
                            Optional ByVal TarifaB As Boolean = True, _
                            Optional ByVal TarifaC As Boolean = True, _
                            Optional ByVal TarifaD As Boolean = True, _
                            Optional ByVal TarifaE As Boolean = True, _
                            Optional ByVal TarifaF As Boolean = True, _
                            Optional ByVal PreciosConIVA As Boolean = False, _
                            Optional ByVal Existencias As Integer = 2) As String

        Dim str As String = CadenaPLUS_MERCANCIAS("a", strMercancias, strCategorias, strMarcas, _
                                                  strDivisiones, strJerarquias, strNivel1, strNivel2, strNivel3, strNivel4, strNivel5, strNivel6, _
                                                  Estatus, Cartera, SoloPeso, TipoMercancia, Regulada)


        If Existencias = 0 Then str += " c.existencia > 0.000 and "
        If Existencias = 1 Then str += " c.existencia = 0.000 and "

        Dim strAlm As String = CadenaPLUS_MERCANCIAS_MOVIMIENTOS("a", strAlmacen)
        Dim strPrecios As String = IIf(PreciosConIVA, " if(" & TarifaA & ", ROUND(IF( ISNULL(b.MONTO), a.PRECIO_A , (b.MONTO/100 + 1)*a.PRECIO_A),0), 0.00) precio_a, " _
                                    & " if(" & TarifaB & ", ROUND(IF( ISNULL(b.MONTO), a.PRECIO_B , (b.MONTO/100 + 1)*a.PRECIO_B),0), 0.00) PRECIO_B, " _
                                    & " if(" & TarifaC & ", ROUND(IF( ISNULL(b.MONTO), a.PRECIO_C , (b.MONTO/100 + 1)*a.PRECIO_C),0), 0.00) PRECIO_C, " _
                                    & " if(" & TarifaD & ", ROUND(IF( ISNULL(b.MONTO), a.PRECIO_D , (b.MONTO/100 + 1)*a.PRECIO_D),0), 0.00) PRECIO_D, " _
                                    & " if(" & TarifaE & ", ROUND(IF( ISNULL(b.MONTO), a.PRECIO_E , (b.MONTO/100 + 1)*a.PRECIO_E),0), 0.00) PRECIO_E, " _
                                    & " if(" & TarifaF & ", ROUND(IF( ISNULL(b.MONTO), a.PRECIO_F , (b.MONTO/100 + 1)*a.PRECIO_F),0), 0.00) PRECIO_F, ", _
                                    " if( " & TarifaA & ", a.precio_a, 0.00) precio_A, if( " & TarifaB & ", a.precio_b, 0.00) precio_b, " _
                                    & " if( " & TarifaC & ", a.precio_c, 0.00) precio_C, if( " & TarifaD & ", a.precio_d, 0.00) precio_d, " _
                                    & " if( " & TarifaE & ", a.precio_E, 0.00) precio_E, if( " & TarifaF & ", a.precio_F, 0.00) precio_f, ")

        SeleccionMERCAS_Mercancias = "select a.codart, a.nomart, a.alterno, a.barras, date_format(a.creacion,'%d-%m-%Y') creacion, " _
              & " j.descrip categoria, k.descrip As marca, l.descrip As tipjer, codjer1, codjer2, codjer3, codjer4, codjer5, codjer6,  elt(a.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') as mix, p.descrip division, " _
              & " if( b.monto is null, 0.00, b.monto) iva, " & strPrecios _
              & " a.DESC_A, a.DESC_B, a.DESC_C, a.DESC_D, a.DESC_E, a.DESC_F, " _
              & " a.presentacion, a.sugerido, a.unidad, a.pesounidad, IF( ISNULL(b.monto), 0.00, b.monto) monto, b.fecha, " _
              & " c.existencia, " _
              & " o.codartcom, o.descrip as descripcom, o.cantidad as cantidadcom, o.unidad as unidadcom, o.costo costocom  " _
              & " from jsmerctainv a " _
              & " left join (" & SeleccionGENTablaIVA(jytsistema.sFechadeTrabajo) & ") b on (a.iva = b.tipo) " _
              & " left join (select a.codart, sum(a.existencia) existencia, a.id_emp from jsmerextalm a where " & strAlm & " a.id_emp = '" & jytsistema.WorkID & "' group by a.codart ) c on (a.codart = c.codart and a.id_emp = c.id_emp) " _
              & " left join jsconctatab j on (a.grupo = j.codigo and a.id_emp = j.id_emp and j.modulo = '00002') " _
              & " left join jsconctatab k on (a.marca = k.codigo and a.id_emp = k.id_emp and k.modulo = '00003') " _
              & " left join jsmerencjer l on (a.tipjer = l.tipjer and a.id_emp = l.id_emp) " _
              & " left join jsmercatdiv p on (a.division = p.division and a.id_emp = p.id_emp) " _
              & " left join jsmercatcom o on (a.codart = o.codart and a.id_emp = o.id_emp) " _
              & " Where " & str _
              & " a.id_emp = '" & jytsistema.WorkID & "' order by a.codart "

    End Function
    Function SeleccionMERCAS_ExistenciasAFecha(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, _
                        strMercancias As String, _
                        strCategorias As String, strMarcas As String, strDivisiones As String, _
                        strJerarquias As String, strNivel1 As String, strNivel2 As String, _
                        strNivel3 As String, strNivel4 As String, strNivel5 As String, strNivel6 As String, _
                        strAlmacen As String, _
                        FechaHasta As Date, CantidadPesoMoneda As Integer, DiasHabiles As Integer, _
                        OrdenadoPor As String, _
                        Optional ByVal Estatus As Integer = 9, Optional ByVal Cartera As Integer = 9, _
                        Optional ByVal SoloPeso As Boolean = False, Optional ByVal TipoMercancia As Integer = 99, _
                        Optional ByVal Regulada As Integer = 9, Optional ByVal Existencias As Integer = 2, _
                        Optional porcentajeGastos As Double = 0.0, Optional TarifaPrecio As String = "A") As String

        Dim str As String = CadenaPLUS_MERCANCIAS("a", strMercancias, strCategorias, strMarcas, strDivisiones, _
                                                  strJerarquias, strNivel1, strNivel2, strNivel3, strNivel4, strNivel5, _
                                                  strNivel6, Estatus, Cartera, SoloPeso, TipoMercancia, Regulada)

        Dim strAlm As String = CadenaPLUS_MERCANCIAS_MOVIMIENTOS("b", strAlmacen)
        Dim strExistencia As String = IIf(Existencias < 2, IIf(Existencias = 1, "having round(cantidad,3) = 0.000 ", " having round(cantidad,3) > 0.000 "), "")

        'existencias

        Dim strCan As String = IIf(CantidadPesoMoneda = 3, " b.peso*n.equivale ", ReporteTipo(CantidadPesoMoneda, "b", "a", "n").strCan)
        Dim strCanX As String = IIf(CantidadPesoMoneda = 3, "/a.pesounidad ", "")
        Dim strUnidad As String = ReporteTipo(CantidadPesoMoneda, "b", "a", "n").strUnidad
        Dim strUND As String = ReporteTipo(CantidadPesoMoneda, "b", "a", "n").strUND


        Dim DiasNH As Integer = ft.DevuelveScalarEntero(MyConn, " SELECT COUNT(id_emp) FROM jsconcatper WHERE " _
                                                        & " MODULO = 1 AND " _
                                                        & " DATE_FORMAT(CONCAT(ano,'-',mes,'-',dia),'%Y-%m-%d') >= '" & ft.FormatoFechaMySQL(DateAdd("d", -1 * DiasHabiles, FechaHasta)) & "' AND " _
                                                        & " DATE_FORMAT(CONCAT(ano,'-',mes,'-',dia),'%Y-%m-%d') <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
                                                        & " id_emp = '" & jytsistema.WorkID & "' GROUP BY id_emp")

        SeleccionMERCAS_ExistenciasAFecha = " select a.codart, a.nomart, a.NOMBRECATEGORIA CATEGORIA, " _
            & " a.NOMBREMARCA MARCA, a.NOMBREJERARQUIA TIPJER, a.NOMBREDIVISION DIVISION, " & strUND & " unidad, " _
            & " a.codjer1, a.codjer2, a.codjer3, a.codjer4, a.codjer5, a.codjer6,  elt(a.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') mix," _
            & " if( b.existencia is null, 0.00, round( if ( abs(b.existencia) <= 0.01, 0.00, b.existencia) ,3 ) ) Cantidad, " _
            & " if( f.costounitario is null, 0.00,  f.costounitario) costotal, " _
            & " if( f.costounitario is null, 0.00,  f.costounitario) costounitario, " _
            & " if( f.costounitario is null, 0.00,  f.costounitario)*(" & porcentajeGastos & "/100) gastosScostos, " _
            & " a.precio_" & TarifaPrecio & " precio, " _
            & " if( u.salidasdiarias is null, 0.00, u.salidasdiarias) salidasdiarias , if( u.salidasdiarias is null, 0.00, u.salidasdiarias)*a.pesounidad salidaskilos " _
            & " from (" & SeleccionGENMercancias() & ") a " _
            & " left join (" & SeleccionGENExistenciasAFecha(FechaHasta, strAlm, strUnidad, strCan, strCanX) & ") b on (a.codart = b.codart) " _
            & " left join ( SELECT a.codart, a.fechamov, a.tipomov, a.numdoc, a.unidad, IF( c.equivale IS NULL, a.cantidad, a.cantidad/c.equivale) cantidad, " _
            & "             a.peso,  IF( c.equivale IS NULL, a.costounitario, c.equivale*a.costounitario) costounitario, c.equivale " _
            & "             FROM (SELECT a.codart, a.fechamov, a.tipomov, a.numdoc, a.unidad, SUM(a.cantidad) cantidad, SUM(a.peso) peso,  " _
            & "                   SUM(a.costotaldes)/ IF(SUM(a.cantidad) > 0, SUM(a.cantidad) ,1)  costounitario " _
            & "                   FROM jsmertramer a, (SELECT codart, MAX(fechamov) AS fechamov FROM jsmertramer WHERE " _
            & "                     fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' AND " _
            & "                     tipomov IN ('EN','AE') AND  " _
            & "                     origen IN ('COM','INV') AND  " _
            & "                     id_emp = '" & jytsistema.WorkID & "' " _
            & "                   GROUP BY codart ) b " _
            & "             WHERE (a.codart = b.codart AND a.fechamov=b.fechamov) AND  " _
            & "             a.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' AND " _
            & "             a.tipomov IN ('EN','AE') AND " _
            & "             a.origen IN ('COM','INV') AND " _
            & "             a.id_emp = '" & jytsistema.WorkID & "' " _
            & "             GROUP BY a.codart, a.numdoc, a.unidad  ) a " _
            & "             LEFT JOIN jsmerequmer c ON (a.codart = c.codart AND a.unidad = c.uvalencia AND c.id_emp = '" & jytsistema.WorkID & "')) f on (a.codart = f.codart) " _
            & " left join (select b.codart, sum(  IF( b.TIPOMOV = 'EN' OR b.tipomov = 'AE' OR b.TIPOMOV = 'DV', 0, 1)*" & strCan & " )/ " _
            & "             (to_days('" & ft.FormatoFechaMySQL(FechaHasta) & "') - to_days('" & ft.FormatoFechaMySQL(DateAdd("d", -1 * IIf(DiasHabiles = 0, 1, DiasHabiles), FechaHasta)) & "') - " & DiasNH & " )  salidasdiarias " _
            & "             from jsmertramer b " _
            & "             left join (" & SeleccionGENTablaEquivalencias() & ") n on (b.codart = n.codart and " & strUnidad & " = n.uvalencia and b.id_emp = n.id_emp) " _
            & "             left join jsmerctainv a on (b.codart = a.codart and b.id_emp = a.id_emp) " _
            & "             Where " _
            & strAlm _
            & "             not tipomov in ('AC','AP') AND " _
            & "             b.id_emp = '" & jytsistema.WorkID & "' and " _
            & "             b.ejercicio = '' and " _
            & "             b.fechamov >= '" & ft.FormatoFechaHoraMySQLInicial(DateAdd("d", -1 * DiasHabiles, FechaHasta)) & "' and " _
            & "             b.fechamov <= '" & ft.FormatoFechaHoraMySQLFinal(FechaHasta) & "' " _
            & "             group by b.codart ) u on (a.codart = u.codart) " _
            & " Where " _
            & str _
            & " a.precio_" & TarifaPrecio & " > 0  AND  " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by a.codart " _
            & strExistencia _
            & " order by a." & OrdenadoPor

    End Function

    Function SeleccionMERCAS_MercanciasYEquivalencias(strMercancias As String, _
                            strCategorias As String, strMarcas As String, strDivisiones As String, _
                            strJerarquias As String, strNivel1 As String, strNivel2 As String, _
                            strNivel3 As String, strNivel4 As String, strNivel5 As String, strNivel6 As String, _
                            OrdenadoPor As String, _
                            Optional ByVal Estatus As Integer = 9, Optional ByVal Cartera As Integer = 9, _
                            Optional ByVal SoloPeso As Boolean = False, Optional ByVal TipoMercancia As Integer = 99, _
                            Optional ByVal Regulada As Integer = 9) As String

        Dim str As String = CadenaPLUS_MERCANCIAS("a", strMercancias, strCategorias, strMarcas, strDivisiones, _
                                                strJerarquias, strNivel1, strNivel2, strNivel3, strNivel4, strNivel5, _
                                                strNivel6, Estatus, Cartera, SoloPeso, TipoMercancia, Regulada)


        SeleccionMERCAS_MercanciasYEquivalencias = "select a.codart, a.nomart, a.alterno, a.barras, date_format(a.creacion,'%d-%m-%Y') creacion, " _
              & " j.descrip categoria, k.descrip marca, l.descrip As tipjer, codjer1, codjer2, codjer3, codjer4, codjer5, codjer6,  elt(a.mix+1,'ECONOMICO','ESTANDAR','SUPERIOR') as mix, p.descrip division, " _
              & " a.presentacion, a.sugerido, a.unidad, a.pesounidad, if( b.equivale is null, 0.000, b.equivale) equivale, " _
              & " if( b.uvalencia is null, '', b.uvalencia) uvalencia, IF(b.divide = 0, 'Si', 'No') divideuv " _
              & " from jsmerctainv a " _
              & " left join jsmerequmer b on (a.codart = b.codart and a.id_emp = b.id_emp)" _
              & " left join jsconctatab j on (a.grupo = j.codigo and a.id_emp = j.id_emp and j.modulo = '00002') " _
              & " left join jsconctatab k on (a.marca = k.codigo and a.id_emp = k.id_emp and k.modulo = '00003') " _
              & " left join jsmerencjer l on (a.tipjer = l.tipjer and a.id_emp = l.id_emp) " _
              & " left join jsmercatdiv p on (a.division = p.division and a.id_emp = p.id_emp) " _
              & " Where " & str _
              & " a.id_emp = '" & jytsistema.WorkID & "' order by a." & OrdenadoPor & " "

    End Function


End Module
