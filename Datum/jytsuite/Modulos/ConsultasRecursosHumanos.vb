Module ConsultasRecursosHumanos
    Private ft As New Transportables

    Public Function SeleccionNOM_TrabajadoresXNomina(CodigoNomina As String) As String

        Return " select a.*, b.codnom from jsnomcattra a " _
            & " left join jsnomrennom b on (a.codtra = b.codtra AND a.ID_EMP = b.ID_EMP ) " _
            & " where " _
            & " b.CODNOM = '" & CodigoNomina & "' AND " _
            & " a.ID_EMP = '" & jytsistema.WorkID & "' "

    End Function
    Public Function SeleccionNOMTrabajadores(ByVal TrabajadorDesde As String, ByVal TrabajadorHasta As String, ByVal Orden As String, _
                                             CodigoNomina As String, _
                                             Optional EstatusTrabajador As Integer = 1) As String

        Dim str As String = ""

        If TrabajadorDesde <> "" Then str += " a.codtra >= '" & TrabajadorDesde & "' and "
        If TrabajadorHasta <> "" Then str += " a.codtra <= '" & TrabajadorHasta & "' and "
        If EstatusTrabajador < 3 Then str += " a.condicion = " & EstatusTrabajador & " and "

        SeleccionNOMTrabajadores = " select a.codtra, a.codhp, a.apellidos, a.nombres, a.profesion, a.ingreso, " _
            & " a.fnacimiento, a.lugarnac, a.edonac, a.pais, a.nacionalizado, a.nacional, a.nogaceta, a.cedula, " _
            & " elt(a.edocivil + 1, 'Soltero(a)', 'Casado(a)', 'Divorciado(a)', 'Concubino(a)', 'Otro(a)' ) edocivil, " _
            & " elt(a.sexo +1, 'Masculino', 'Femenino') Sexo, a.ascendentes, a.descendentes, a.nosso, a.statussso, elt(vivienda+1,'Propia','Alquilada','Préstamo','Otra') vivienda, " _
            & " a.vehiculos, a.direccion, a.telef1, a.telef2, a.email, elt(a.condicion+1,'Inactivo', 'Activo', 'Desincorporado') condicion,   " _
            & " Elt(a.tiponom+1, 'DIARIA', 'SEMANAL', 'QUINCENAL', 'MENSUAL', 'ANUAL', 'EVENTUAL') tiponom, " _
            & " elt(a.formapago+1, 'Efectivo','Cheque','Depósito/Transferencia', 'Otro') formapago, " _
            & " a.banco, c.descrip nombanco, a.ctaban, a.conyuge, a.co_nacion, a.co_cedula, a.co_profesion, a.co_fecnac, " _
            & " a.co_lugnac, a.co_edonac, a.co_pais, a.grupo, a.subnivel1, a.subnivel2, a.subnivel3, a.subnivel4, " _
            & " a.subnivel5, a.subnivel6, a.estructura, b.nombre cargo, a.sueldo, a.foto, a.turnodesde, a.freedays, " _
            & " Elt(a.periodo+1, 'DIARIA', 'SEMANAL', 'QUINCENAL', 'MENSUAL', 'ANUAL', 'EVENTUAL') periodo, a.datefreeday, a.rotatorio, a.fecharet, a.id_emp " _
            & " from (" & SeleccionNOM_TrabajadoresXNomina(CodigoNomina) & ") a " _
            & " left join jsnomestcar b on (a.cargo = b.codigo and a.id_emp = b.id_emp) " _
            & " left join jsconctatab c on (a.banco = c.codigo and a.id_emp = c.id_emp and c.modulo = '" & FormatoTablaSimple(Modulo.iBancos) & "') " _
            & " where " _
            & str _
            & " a.id_emp = '" & jytsistema.WorkID & "' order by " & Orden

    End Function

    Public Function SeleccionNOMConstantes() As String

        SeleccionNOMConstantes = " SELECT a.constante, ELT(a.tipo + 1, 'Numérica' , 'Fecha', 'Caracter', 'Booleana')  tipo,  " _
            & " IF ( a.tipo = 3, IF ( a.valor = 0, 'Falso', 'Verdadero' ), a.valor )  valor " _
            & " FROM jsnomcatcot a " _
            & " WHERE a.id_emp = '" & jytsistema.WorkID & "' "

    End Function

    Public Function SeleccionNOMConceptos(CodigoNomina As String) As String

        SeleccionNOMConceptos = " SELECT a.codcon, a.nomcon, ELT(a.tipo + 1, 'Asignación', 'Deducción', 'Adicional', 'Especial') tipo, " _
                & " a.conjunto, a.condicion, a.formula, ELT(a.estatus + 1, 'Inactivo', 'Activo') estatus, CODNOM " _
                & " FROM jsnomcatcon a " _
                & " WHERE " _
                & " a.codnom = '" & CodigoNomina & "' and " _
                & " a.id_emp = '" & jytsistema.WorkID & "'"

    End Function


    Public Function SeleccionNOMConceptosXTrabajador(ByVal ConceptoDesde As String, ByVal ConceptoHasta As String, _
                                                     CodigoNomina As String) As String
        Dim str As String = ""
        If ConceptoDesde <> "" Then str += " a.codcon >= '" & ConceptoDesde & "' and "
        If ConceptoHasta <> "" Then str += " a.codcon <= '" & ConceptoHasta & "' and "

        SeleccionNOMConceptosXTrabajador = " SELECT a.codcon, c.nomcon descripcion, a.codtra, CONCAT(b.apellidos, ', ', b.nombres) nombre, a.importe " _
                & " FROM jsnomtrades a " _
                & " LEFT JOIN (" & SeleccionNOM_TrabajadoresXNomina(CodigoNomina) & ") b ON (a.codtra = b.codtra AND a.id_emp = b.id_emp and a.codnom = b.codnom) " _
                & " LEFT JOIN jsnomcatcon c ON (a.codcon = c.codcon AND a.codnom = c.codnom AND a.id_emp = c.id_emp)  " _
                & " WHERE " _
                & str _
                & " a.codnom = '" & CodigoNomina & "' and " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " ORDER BY a.codcon, a.codtra "

    End Function

    Public Function SeleccionNOMPrenomina(ByVal Orden As String, CodigoNomina As String, _
                                          TrabajadorDesde As String, TrabajadorHasta As String) As String

        SeleccionNOMPrenomina = " SELECT a.codtra, CONCAT(a.nacional, '-',a.cedula) CI, " _
            & " a.apellidos, a.nombres, a.cargo, IF(d.nombre IS NULL, '', d.nombre) nomCargo, b.codcon, c.nomcon, c.tipo,  " _
            & " IF( c.tipo =  0,  b.importe, 0.00) Asignaciones, " _
            & " IF( c.tipo =  1,  b.importe, 0.00) Deducciones,  " _
            & " IF( c.tipo =  2,  b.importe, 0.00) Adicional,  " _
            & " IF( c.tipo =  3,  b.importe, 0.00) Especial, concat(b.porcentaje_asig, ' / Concepto ', c.concepto_por_asig) percent " _
            & " FROM (" & SeleccionNOM_TrabajadoresXNomina(CodigoNomina) & ") a " _
            & " LEFT JOIN jsnomtrades b ON (a.codtra = b.codtra AND a.id_emp = b.id_emp and a.codnom = b.codnom) " _
            & " LEFT JOIN jsnomcatcon c ON (b.codcon = c.codcon AND b.id_emp = c.id_emp and b.codnom = c.codnom) " _
            & " LEFT JOIN jsnomestcar d ON (a.cargo = d.codigo AND a.id_emp = d.id_emp) " _
            & " WHERE " _
            & " b.importe > 0 and " _
            & IIf(TrabajadorDesde <> "", " a.codtra >= '" & TrabajadorDesde & "'  AND ", "") _
            & IIf(TrabajadorHasta <> "", " a.codtra <= '" & TrabajadorHasta & "'  AND ", "") _
            & " a.condicion = 1 and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY   a." & Orden & ", c.tipo, b.codcon "

    End Function

    Public Function SeleccionNOMNomina(ByVal Orden As String, ByVal CodigoNomina As String, _
                                       ByVal FechaDesde As Date, ByVal FechaHasta As Date, _
                                       TrabajadorDesde As String, TrabajadorHasta As String) As String

        SeleccionNOMNomina = " SELECT a.codtra, CONCAT(a.nacional, '-',a.cedula) CI, " _
            & " a.apellidos, a.nombres, a.cargo, IF(d.nombre IS NULL, '', d.nombre) nomCargo, b.codcon, c.nomcon, c.tipo,  " _
            & " IF( c.tipo =  0,  b.importe, 0.00) Asignaciones, " _
            & " IF( c.tipo =  1,  b.importe, 0.00) Deducciones, " _
            & " IF( c.tipo =  2,  b.importe, 0.00) Adicional, " _
            & " IF( c.tipo =  3,  b.importe, 0.00) Especial, num_cuota, concat(b.porcentaje_asig, ' / Concepto ', c.concepto_por_asig ) percent " _
            & " FROM (" & SeleccionNOM_TrabajadoresXNomina(CodigoNomina) & ") a " _
            & " LEFT JOIN jsnomhisdes b ON (a.codtra = b.codtra AND a.id_emp = b.id_emp AND a.codnom = b.codnom) " _
            & " LEFT JOIN jsnomcatcon c ON (b.codcon = c.codcon AND b.id_emp = c.id_emp AND b.codnom = c.codnom) " _
            & " LEFT JOIN jsnomestcar d ON (a.cargo = d.codigo AND a.id_emp = d.id_emp) " _
            & " WHERE " _
            & " b.importe > 0 and " _
            & " b.codnom = '" & CodigoNomina & "' AND " _
            & " b.desde = '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " b.hasta = '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
            & IIf(TrabajadorDesde <> "", " a.codtra >= '" & TrabajadorDesde & "'  AND ", "") _
            & IIf(TrabajadorHasta <> "", " a.codtra <= '" & TrabajadorHasta & "'  AND ", "") _
            & " a.condicion < 2 AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " ORDER BY c.tipo, b.codcon "

    End Function

    Public Function SeleccionNOMResumenXConcepto(ByVal CodigoNomina As String, ByVal FechaDesde As Date, ByVal FEchaHasta As Date) As String

        SeleccionNOMResumenXConcepto = " SELECT b.codcon, c.nomcon, c.tipo,  " _
            & " SUM(IF( c.tipo =  0,  b.importe, 0.00)) Asignaciones, " _
            & " SUM(IF( c.tipo =  1,  b.importe, 0.00)) Deducciones, " _
            & " SUM(IF( c.tipo =  2,  b.importe, 0.00)) Adicional, " _
            & " SUM(IF( c.tipo =  3,  b.importe, 0.00)) Especial " _
            & " FROM (" & SeleccionNOM_TrabajadoresXNomina(CodigoNomina) & ") a " _
            & " LEFT JOIN jsnomhisdes b ON (a.codtra = b.codtra AND a.id_emp = b.id_emp AND a.codnom = b.codnom) " _
            & " LEFT JOIN jsnomcatcon c ON (b.codcon = c.codcon AND b.id_emp = c.id_emp AND a.codnom = b.codnom) " _
            & " WHERE " _
            & " b.codnom = '" & CodigoNomina & "' AND " _
            & " b.importe > 0 and " _
            & " b.desde = '" & ft.FormatoFechaMySQL(FechaDesde) & "' AND " _
            & " b.hasta = '" & ft.FormatoFechaMySQL(FEchaHasta) & "' AND " _
            & " a.id_emp = '" & jytsistema.WorkID & "' " _
            & " group by b.codcon " _
            & " ORDER BY c.tipo, b.codcon "

    End Function
    Public Function SeleccionNOMAcumuladosXConcepto(CodigoNomina As String, ByVal FechaHasta As Date) As String

        SeleccionNOMAcumuladosXConcepto = " SELECT a.codcon, b.nomcon, SUM( IF( b.tipo = 0,  a.importe, 0) ) Asignaciones, " _
                & " SUM( IF( b.tipo = 1,  a.importe, 0) ) Deducciones,  " _
                & " SUM( IF( b.tipo = 2,  a.importe, 0) ) Especiales,  " _
                & " SUM( IF( b.tipo = 3,  a.importe, 0) ) Adicionales " _
                & " FROM jsnomhisdes a  " _
                & " LEFT JOIN jsnomcatcon b ON (a.codcon = b.codcon AND a.CODNOM = b.CODNOM AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN (" & SeleccionNOM_TrabajadoresXNomina(CodigoNomina) & ") c ON (a.codtra = c.codtra AND a.id_emp = c.id_emp AND a.codnom = c.codnom ) " _
                & " WHERE " _
                & " a.hasta >= '" & ft.FormatoFechaMySQL(PrimerDiaAño(FechaHasta)) & "' and " _
                & " a.hasta <= '" & ft.FormatoFechaMySQL(FechaHasta) & "' AND " _
                & " a.codnom = '" & CodigoNomina & "' and " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " GROUP BY a.codcon " _
                & " HAVING ( asignaciones + deducciones + especiales + adicionales) > 0 " _
                & " ORDER BY b.tipo, a.codcon "

    End Function
    Public Function SeleccionNOMConceptosXTrabajadorMesAMes(ByVal CodigoNomina As String, ByVal ConceptoDesde As String, ByVal ConceptoHasta As String) As String

        Dim str As String = ""

        If ConceptoDesde <> "" Then str += " a.codcon >= '" & ConceptoDesde & "' AND "
        If ConceptoHasta <> "" Then str += " a.codcon <= '" & ConceptoHasta & "' AND "

        SeleccionNOMConceptosXTrabajadorMesAMes = " SELECT a.codcon, b.nomcon, ELT(b.tipo+1, 'Asignación', 'Deducción', 'Especial', 'Adicional') Tipo, a.codtra, " _
                & " CONCAT(c.apellidos,', ',c.nombres) nombres, CONCAT(c.nacional,'-',c.cedula) ci, " _
                & " SUM( IF( MONTH(a.hasta) = 1,  a.importe, 0) ) ENE,  SUM( IF( MONTH(a.hasta) = 2,  a.importe, 0) ) FEB, " _
                & " SUM( IF( MONTH(a.hasta) = 3,  a.importe, 0) ) MAR,  SUM( IF( MONTH(a.hasta) = 4,  a.importe, 0) ) ABR, " _
                & " SUM( IF( MONTH(a.hasta) = 5,  a.importe, 0) ) MAY,  SUM( IF( MONTH(a.hasta) = 6,  a.importe, 0) ) JUN, " _
                & " SUM( IF( MONTH(a.hasta) = 7,  a.importe, 0) ) JUL,  SUM( IF( MONTH(a.hasta) = 8,  a.importe, 0) ) AGO, " _
                & " SUM( IF( MONTH(a.hasta) = 9,  a.importe, 0) ) SEP,  SUM( IF( MONTH(a.hasta) = 10,  a.importe, 0) ) OCT, " _
                & " SUM( IF( MONTH(a.hasta) = 11, a.importe, 0) ) NOV,  SUM( IF( MONTH(a.hasta) = 12,  a.importe, 0) ) DIC, " _
                & " SUM( a.importe) TOT " _
                & " FROM jsnomhisdes a  " _
                & " LEFT JOIN jsnomcatcon b ON (a.codcon = b.codcon AND a.CODNOM = b.CODNOM AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN (" & SeleccionNOM_TrabajadoresXNomina(CodigoNomina) & ") c ON (a.codtra = c.codtra AND a.id_emp = c.id_emp AND a.codnom = c.codnom) " _
                & " WHERE " _
                & " a.hasta >= '" & ft.FormatoFechaMySQL(PrimerDiaAño(jytsistema.sFechadeTrabajo)) & "' AND " _
                & " a.hasta <= '" & ft.FormatoFechaMySQL(UltimoDiaAño(jytsistema.sFechadeTrabajo)) & "' AND " _
                & str _
                & " a.codnom = '" & CodigoNomina & "' and " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " GROUP BY a.codcon, a.codtra " _
                & " HAVING tot > 0  "

    End Function

    Public Function SeleccionNOMConceptosMesAMes(ByVal CodigoNomina As String, ByVal ConceptoDesde As String, ByVal ConceptoHasta As String) As String

        Dim str As String = ""

        If ConceptoDesde <> "" Then str += " a.codcon >= '" & ConceptoDesde & "' AND "
        If ConceptoHasta <> "" Then str += " a.codcon <= '" & ConceptoHasta & "' AND "

        SeleccionNOMConceptosMesAMes = " SELECT a.codcon, b.nomcon, ELT(b.tipo+1, 'Asi', 'Ded', 'Esp', 'Adi') Tipo, " _
                & " SUM( IF( MONTH(a.hasta) = 1,  a.importe, 0) ) ENE,  SUM( IF( MONTH(a.hasta) = 2,  a.importe, 0) ) FEB, " _
                & " SUM( IF( MONTH(a.hasta) = 3,  a.importe, 0) ) MAR,  SUM( IF( MONTH(a.hasta) = 4,  a.importe, 0) ) ABR, " _
                & " SUM( IF( MONTH(a.hasta) = 5,  a.importe, 0) ) MAY,  SUM( IF( MONTH(a.hasta) = 6,  a.importe, 0) ) JUN, " _
                & " SUM( IF( MONTH(a.hasta) = 7,  a.importe, 0) ) JUL,  SUM( IF( MONTH(a.hasta) = 8,  a.importe, 0) ) AGO, " _
                & " SUM( IF( MONTH(a.hasta) = 9,  a.importe, 0) ) SEP,  SUM( IF( MONTH(a.hasta) = 10,  a.importe, 0) ) OCT, " _
                & " SUM( IF( MONTH(a.hasta) = 11, a.importe, 0) ) NOV,  SUM( IF( MONTH(a.hasta) = 12,  a.importe, 0) ) DIC, " _
                & " SUM( a.importe) TOT " _
                & " FROM jsnomhisdes a  " _
                & " LEFT JOIN jsnomcatcon b ON (a.codcon = b.codcon AND a.CODNOM = b.CODNOM AND a.id_emp = b.id_emp) " _
                & " LEFT JOIN (" & SeleccionNOM_TrabajadoresXNomina(CodigoNomina) & ") c ON (a.codtra = c.codtra AND a.id_emp = c.id_emp and a.codnom = c.codnom ) " _
                & " WHERE " _
                & " a.hasta >= '" & ft.FormatoFechaMySQL(PrimerDiaAño(jytsistema.sFechadeTrabajo)) & "' AND " _
                & " a.hasta <= '" & ft.FormatoFechaMySQL(UltimoDiaAño(jytsistema.sFechadeTrabajo)) & "' AND " _
                & str _
                & " a.codnom = '" & CodigoNomina & "' and " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " GROUP BY a.codcon " _
                & " HAVING tot > 0  "

    End Function

End Module