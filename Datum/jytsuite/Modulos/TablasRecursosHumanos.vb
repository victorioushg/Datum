Imports MySql.Data.MySqlClient
Module TablasRecursosHumanos
    Private ft As New Transportables

    Public Sub InsertEditNOMINATrabajador(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal CodigoTrabajador As String, ByVal CodigoHP As String, _
    ByVal Apellidos As String, ByVal Nombres As String, ByVal Profesion As String, ByVal Ingreso As Date, ByVal Nacimiento As Date, ByVal LugarNacimiento As String, _
    ByVal Pais As String, ByVal Nacionalizado As String, ByVal Nacionalidad As String, ByVal NumeroGaceta As String, ByVal CI As String, ByVal Edo_Civil As Integer, _
    ByVal Sexo As String, ByVal Ascendentes As Integer, ByVal Descendentes As Integer, ByVal Num_SSO As String, ByVal Estatus_SSO As String, _
    ByVal Vivienda As Integer, ByVal Vehiculos As Integer, ByVal Direccion As String, ByVal Telef1 As String, ByVal Telef2 As String, ByVal email As String, _
    ByVal Condicion As Integer, ByVal TipoNomina As Integer, ByVal FormaPago As Integer, ByVal CodigoBanco As String, ByVal Num_Cta As String, ByVal CodigoBanco1 As String, ByVal Num_Cta1 As String, ByVal CodigoBanco2 As String, ByVal Num_Cta2 As String, _
    ByVal NombreConyuge As String, ByVal Con_Nacionalidad As String, ByVal Con_CI As String, ByVal Con_Profesion As String, ByVal Con_FechaNacimiento As Date, _
    ByVal Con_LugarNac As String, ByVal Con_Pais As String, ByVal Grupo As String, ByVal Nivel1 As String, ByVal Nivel2 As String, ByVal Nivel3 As String, _
    ByVal Nivel4 As String, ByVal Nivel5 As String, ByVal Nivel6 As String, ByVal CodigoCargo As String, ByVal TurnoDesde As Date, _
    ByVal Sueldo As Double, RetencionISLR As Double, ByVal DiasLibres As Integer, ByVal Periodo As Integer, ByVal FechaDiasLibres As Date, ByVal DiaLibreRotatorio As Boolean, ByVal FechaRetiro As Date)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsnomcattra SET "
        Else
            strSQLInicio = " UPDATE jsnomcattra SET "
            strSQLFin = " WHERE " _
                & " codtra = '" & CodigoTrabajador & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL += ModificarCadena(CodigoTrabajador, "codtra")
        strSQL += ModificarCadena(CodigoHP, "codhp")
        strSQL += ModificarCadena(Apellidos, "apellidos")
        strSQL += ModificarCadena(Nombres, "nombres")
        strSQL += ModificarCadena(Profesion, "profesion")
        strSQL += ModificarFecha(Ingreso, "ingreso")
        strSQL += ModificarFecha(Nacimiento, "fnacimiento")
        strSQL += ModificarCadena(LugarNacimiento, "lugarnac")
        strSQL += ModificarCadena(Pais, "pais")
        strSQL += ModificarCadena(Nacionalizado, "nacionalizado")
        strSQL += ModificarCadena(Nacionalidad, "nacional")
        strSQL += ModificarCadena(NumeroGaceta, "nogaceta")
        strSQL += ModificarCadena(CI, "cedula")
        strSQL += ModificarEntero(Edo_Civil, "edocivil")
        strSQL += ModificarEntero(Sexo, "sexo")
        strSQL += ModificarEnteroLargo(Ascendentes, "ascendentes")
        strSQL += ModificarEnteroLargo(Descendentes, "descendentes")
        strSQL += ModificarCadena(Num_SSO, "nosso")
        strSQL += ModificarCadena(Estatus_SSO, "statussso")
        strSQL += ModificarEntero(Vivienda, "vivienda")
        strSQL += ModificarEnteroLargo(Vehiculos, "vehiculos")
        strSQL += ModificarCadena(Direccion, "direccion")
        strSQL += ModificarCadena(Telef1, "telef1")
        strSQL += ModificarCadena(Telef2, "telef2")
        strSQL += ModificarCadena(email, "email")
        strSQL += ModificarEntero(Condicion, "condicion")
        strSQL += ModificarEntero(TipoNomina, "tiponom")
        strSQL += ModificarEnteroLargo(FormaPago, "formapago")
        strSQL += ModificarCadena(CodigoBanco, "banco")
        strSQL += ModificarCadena(Num_Cta, "ctaban")
        strSQL += ModificarCadena(CodigoBanco1, "banco_1")
        strSQL += ModificarCadena(Num_Cta1, "ctaban_1")
        strSQL += ModificarCadena(CodigoBanco2, "banco_2")
        strSQL += ModificarCadena(Num_Cta2, "ctaban_2")
        strSQL += ModificarCadena(NombreConyuge, "conyuge")
        strSQL += ModificarCadena(Con_Nacionalidad, "co_nacion")
        strSQL += ModificarCadena(Con_CI, "co_cedula")
        strSQL += ModificarCadena(Con_Profesion, "co_profesion")
        strSQL += ModificarFecha(Con_FechaNacimiento, "co_fecnac")
        strSQL += ModificarCadena(Con_LugarNac, "co_lugnac")
        strSQL += ModificarCadena(Con_Pais, "co_pais")
        strSQL += ModificarCadena(Grupo, "grupo")
        strSQL += ModificarCadena(Nivel1, "subnivel1")
        strSQL += ModificarCadena(Nivel2, "subnivel2")
        strSQL += ModificarCadena(Nivel3, "subnivel3")
        strSQL += ModificarCadena(Nivel4, "subnivel4")
        strSQL += ModificarCadena(Nivel5, "subnivel5")
        strSQL += ModificarCadena(Nivel6, "subnivel6")
        strSQL += ModificarCadena(CodigoCargo, "cargo")
        strSQL += ModificarFecha(TurnoDesde, "turnodesde")
        strSQL += ModificarDoble(Sueldo, "sueldo")
        strSQL += ModificarDoble(RetencionISLR, "RETISLR")
        strSQL += ModificarEnteroLargo(DiasLibres, "freedays")
        strSQL += ModificarEnteroLargo(Periodo, "periodo")
        strSQL += ModificarFecha(FechaDiasLibres, "datefreeday")
        strSQL += ModificarEnteroLargo(CInt(DiaLibreRotatorio), "rotatorio")
        strSQL += ModificarFecha(FechaRetiro, "fecharet")

        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub GuardarFotoTrabajador(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoTrabajador As String, ByVal MyFoto() As Byte)
        Dim myComm As New MySqlCommand
        Try
            myComm.Connection = MyConn
            myComm.CommandText = "update jsnomcattra set foto = ?foto where codtra = '" & CodigoTrabajador & "' "
            myComm.Parameters.AddWithValue("?foto", MyFoto)
            myComm.ExecuteNonQuery()
            myComm = Nothing
        Catch ex As MySqlException
            ft.MensajeCritico("Error en conexión de base de datos: " & ex.Message)
        End Try
    End Sub
    Public Sub InsertEditNOMINAConstante(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                                            ByVal Constante As String, ByVal Tipo As Integer, _
                                            ByVal Valor As String)
        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsnomcatcot SET "
        Else
            strSQLInicio = " UPDATE jsnomcatcot SET "
            strSQLFin = " WHERE " _
                & " constante = '" & Constante & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL = strSQL & ModificarCadena(Constante, "constante")
        strSQL = strSQL & ModificarCadena(Valor, "valor")
        strSQL = strSQL & ModificarEntero(Tipo, "tipo")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditNOMINAConcepto(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal CodigoConcepto As String, ByVal NombreConcepto As String, ByVal TipoConcepto As Integer, _
        ByVal Cuota As String, ByVal Conjunto As String, ByVal DescripcionConcepto As String, ByVal Formula As String, ByVal Condicion As String, _
        ByVal AgrupadoPor As String, ByVal CodigoProveedor As String, ByVal Estatus As Integer, _
        ConceptoPorcentajeAsignacion As String, CodigoContable As String, CodigoNomina As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsnomcatcon SET "
        Else
            strSQLInicio = " UPDATE jsnomcatcon SET "
            strSQLFin = " WHERE " _
                & " codcon = '" & CodigoConcepto & "' and " _
                & " codnom = '" & CodigoNomina & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL += ModificarCadena(CodigoConcepto, "codcon")
        strSQL += ModificarCadena(NombreConcepto, "nomcon")
        strSQL += ModificarEntero(TipoConcepto, "tipo")
        strSQL += ModificarCadena(Cuota, "cuota")
        strSQL += ModificarCadena(Conjunto, "conjunto")
        strSQL += ModificarCadena(DescripcionConcepto, "descripcion")
        strSQL += ModificarCadena(Formula, "formula")
        strSQL += ModificarCadena(Condicion, "condicion")
        strSQL += ModificarCadena(AgrupadoPor, "agrupadopor")
        strSQL += ModificarCadena(CodigoProveedor, "codpro")
        strSQL += ModificarCadena(ConceptoPorcentajeAsignacion, "CONCEPTO_POR_ASIG")
        strSQL += ModificarCadena(CodigoContable, "CODIGOCON")
        strSQL += ModificarEntero(Estatus, "estatus")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")
        strSQL += ModificarCadena(CodigoNomina, "CODNOM")

        ft.Ejecutar_strSQL(Myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditNOMINAGrupo(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
       ByVal CodigoNivelAnterior As String, ByVal CodigoGrupo As String, ByVal Descripcion As String, _
       ByVal Nivel As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsnomrengru SET "
        Else
            strSQLInicio = " UPDATE jsnomrengru SET "
            strSQLFin = " WHERE " _
                & " cod_nivel_ant = '" & CodigoNivelAnterior & "' and " _
                & " cod_nivel = '" & CodigoGrupo & "' and " _
                & " nivel = " & Nivel & " and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL += ModificarCadena("", "grupo")
        strSQL += ModificarCadena(CodigoNivelAnterior, "cod_nivel_ant")
        strSQL += ModificarCadena(CodigoGrupo, "cod_nivel")
        strSQL += ModificarCadena(Descripcion, "des_nivel")
        strSQL += ModificarEnteroLargo(Nivel, "nivel")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditNOMINA_NOMINA(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
    ByVal CodigoNomina As String, ByVal DescripcionNomina As String, ByVal TipoNomina As Integer, _
    ByVal CodigoContable As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsnomencnom SET "
        Else
            strSQLInicio = " UPDATE jsnomencnom SET "
            strSQLFin = " WHERE " _
                & " codnom = '" & CodigoNomina & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoNomina, "codnom")
        strSQL += ModificarCadena(DescripcionNomina, "descripcion")
        strSQL += ModificarEntero(TipoNomina, "tiponom")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditNOMINA_NOMINA_MOVIMIENTOS(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
                        ByVal CodigoNomina As String, ByVal CodigoTrabajador As String, ByVal CodigoContable As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsnomrennom SET "
        Else
            strSQLInicio = " UPDATE jsnomrennom SET "
            strSQLFin = " WHERE " _
                & " codnom = '" & CodigoNomina & "' and " _
                & " codtra = '" & CodigoTrabajador & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoNomina, "codnom")
        strSQL += ModificarCadena(CodigoTrabajador, "codtra")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub



    Public Sub InsertEditNOMINACargo(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
       ByVal Codigo As Integer, ByVal Descripcion As String, ByVal Codigoempresa As String, _
       ByVal SueldoBase As Double, ByVal Antecesor As Integer, ByVal Nivel As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsnomestcar SET codigo = null, "
        Else
            strSQLInicio = " UPDATE jsnomestcar SET codigo = " & Codigo & ", "
            strSQLFin = " WHERE " _
                & " codigo = " & Codigo & " and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL += ModificarCadena(Descripcion, "nombre")
        strSQL += ModificarCadena(Codigoempresa, "codigoempresa")
        strSQL += ModificarDoble(SueldoBase, "sueldobase")
        strSQL += ModificarEnteroLargo(Antecesor, "antecesor")
        strSQL += ModificarEnteroLargo(Nivel, "nivel")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditNOMINAExpedienteTrabajador(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal CodigoTrabajador As String, ByVal fecha As Date, FechaRetorno As Date, ByVal Comentario As String, _
        ByVal Causa As Integer, Estatus As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsnomexptra SET "
        Else
            strSQLInicio = " UPDATE jsnomexptra SET "
            strSQLFin = " WHERE " _
                & " codtra = " & CodigoTrabajador & " and " _
                & " Fecha = '" & ft.FormatoFechaMySQL(fecha) & "' and " _
                & " causa = " & Causa & " and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL += ModificarCadena(CodigoTrabajador, "codtra")
        strSQL += ModificarFecha(fecha, "FECHA")
        strSQL += ModificarFecha(FechaRetorno, "FECHA_FIN")
        strSQL += ModificarCadena(Comentario, "comentario")
        strSQL += ModificarEnteroLargo(Causa, "causa")
        strSQL += ModificarEnteroLargo(Estatus, "ESTATUS")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(Myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditNOMINAConceptoTrabajador(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
       ByVal CodigoTrabajador As String, ByVal CodigoConcepto As String, ByVal Importe As Double, PorcentajeSobreAsignacion As Double, _
       CodigoNomina As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsnomtrades SET "
        Else
            strSQLInicio = " UPDATE jsnomtrades SET "
            strSQLFin = " WHERE " _
                & " codtra = " & CodigoTrabajador & " AND " _
                & " codcon = '" & CodigoConcepto & "' AND " _
                & " CODNOM = '" & CodigoNomina & "' AND " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL += ModificarCadena(CodigoTrabajador, "codtra")
        strSQL += ModificarCadena(CodigoConcepto, "codcon")
        strSQL += ModificarDoble(Importe, "importe")
        strSQL += ModificarDoble(PorcentajeSobreAsignacion, "PORCENTAJE_ASIG")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")
        strSQL += ModificarCadena(CodigoNomina, "CODNOM")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditNOMINAEncabezadoCuota(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
      ByVal CodigoTrabajador As String, CodigoNomina As String, ByVal CodigoPrestamo As String, ByVal Descripcion As String, _
      ByVal MontoTotal As Double, ByVal FechaPrestamo As Date, ByVal FechaInicio As Date, ByVal TipoInteres As Integer, _
      ByVal Por_interes As Double, ByVal NumeroCuotas As Integer, ByVal Saldo As Double, ByVal Estatus As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsnomencpre SET "
        Else
            strSQLInicio = " UPDATE jsnomencpre SET "
            strSQLFin = " WHERE " _
                & " codtra = " & CodigoTrabajador & " and " _
                & " codnom = '" & CodigoNomina & "' and " _
                & " codpre = '" & CodigoPrestamo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL += ModificarCadena(CodigoTrabajador, "codtra")
        strSQL += ModificarCadena(CodigoNomina, "codnom")
        strSQL += ModificarCadena(CodigoPrestamo, "codpre")
        strSQL += ModificarCadena(Descripcion, "descrip")
        strSQL += ModificarDoble(MontoTotal, "montotal")
        strSQL += ModificarFecha(FechaPrestamo, "fechaprestamo")
        strSQL += ModificarFecha(FechaInicio, "fechainicio")
        strSQL += ModificarEnteroLargo(TipoInteres, "tipointeres")
        strSQL += ModificarDoble(Por_interes, "por_interes")
        strSQL += ModificarEnteroLargo(NumeroCuotas, "numcuotas")
        strSQL += ModificarDoble(Saldo, "saldo")
        strSQL += ModificarEntero(Estatus, "estatus")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(Myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditNOMINARenglonCuota(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
      ByVal CodigoTrabajador As String, CodigoNomina As String, ByVal CodigoPrestamo As String, ByVal num_cuota As Integer, _
      ByVal Monto As Double, ByVal Capital As Double, ByVal Interes As Double, ByVal Procesada As Integer, _
      ByVal FechaInicio As Date, ByVal FechaFin As Date)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsnomrenpre SET "
        Else
            strSQLInicio = " UPDATE jsnomrenpre SET "
            strSQLFin = " WHERE " _
                & " codtra = " & CodigoTrabajador & " and " _
                & " codnom = '" & CodigoNomina & "' and " _
                & " codpre = '" & CodigoPrestamo & "' and " _
                & " num_cuota = " & num_cuota & " and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL += ModificarCadena(CodigoTrabajador, "codtra")
        strSQL += ModificarCadena(CodigoNomina, "codnom")
        strSQL += ModificarCadena(CodigoPrestamo, "codpre")
        strSQL += ModificarEnteroLargo(num_cuota, "num_cuota")
        strSQL += ModificarDoble(Monto, "monto")
        strSQL += ModificarDoble(Capital, "capital")
        strSQL += ModificarDoble(Interes, "interes")
        strSQL += ModificarEntero(Procesada, "procesada")
        strSQL += ModificarFecha(FechaInicio, "fechainicio")
        strSQL += ModificarFecha(FechaFin, "fechafin")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(Myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditNOMINAHistoricoConcepto(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
      ByVal CodigoTrabajador As String, ByVal CodigoConcepto As String, ByVal Desde As Date, ByVal Hasta As Date, _
      ByVal Importe As Double, ByVal CodigoPrestamo As String, ByVal num_Cuota As Integer, ByVal Asiento As String, _
      ByVal FechaAsiento As Date, PorcentajeSobreAsignacion As Double, CodigoContable As String, CodigoNomina As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsnomhisdes SET "
        Else
            strSQLInicio = " UPDATE jsnomhisdes SET "
            strSQLFin = " WHERE " _
                & " codtra = " & CodigoTrabajador & " and " _
                & " codcon = '" & CodigoConcepto & "' and " _
                & " desde = '" & ft.FormatoFechaMySQL(Desde) & "' and " _
                & " hasta = '" & ft.FormatoFechaMySQL(Hasta) & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' AND " _
                & " CODNOM = '" & CodigoNomina & "' "

        End If
        strSQL += ModificarCadena(CodigoTrabajador, "codtra")
        strSQL += ModificarCadena(CodigoConcepto, "codcon")
        strSQL += ModificarFecha(Desde, "desde")
        strSQL += ModificarFecha(Hasta, "hasta")
        strSQL += ModificarDoble(Importe, "importe")
        strSQL += ModificarEnteroLargo(num_Cuota, "num_cuota")
        strSQL += ModificarCadena(CodigoPrestamo, "codpre")
        strSQL += ModificarCadena(Asiento, "asiento")
        strSQL += ModificarFecha(FechaAsiento, "fechasi")

        strSQL += ModificarDoble(PorcentajeSobreAsignacion, "PORCENTAJE_ASIG")
        strSQL += ModificarCadena(CodigoContable, "CODIGOCON")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")
        strSQL += ModificarCadena(CodigoNomina, "CODNOM")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditNOMINATurnoHorario(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
      ByVal CodigoTurno As String, ByVal NombreTurno As String, ByVal TipoNomina As Integer, _
      ByVal HoraExtraDiurna As Date, ByVal HoraExtraNocturna As Date, ByVal MarcancionesXTurno As Integer, ByVal MarcacionesXDescanso As Integer, _
      ByVal ToleranciaEntrada As Integer, ByVal ToleranciaSalida As Integer, ByVal ToleranciaDescanso As Integer, ByVal ToleranciaRetorno As Integer, _
      ByVal Lunes As Integer, ByVal EntradaLunes As Date, ByVal SalidaLunes As Date, ByVal DescansoLunes As Date, ByVal RetornoLunes As Date, _
      ByVal Martes As Integer, ByVal EntradaMartes As Date, ByVal SalidaMartes As Date, ByVal DescansoMartes As Date, ByVal RetornoMartes As Date, _
      ByVal Miercoles As Integer, ByVal EntradaMiercoles As Date, ByVal SalidaMiercoles As Date, ByVal DescansoMiercoles As Date, ByVal RetornoMiercoles As Date, _
      ByVal Jueves As Integer, ByVal EntradaJueves As Date, ByVal SalidaJueves As Date, ByVal DescansoJueves As Date, ByVal RetornoJueves As Date, _
      ByVal Viernes As Integer, ByVal EntradaViernes As Date, ByVal SalidaViernes As Date, ByVal DescansoViernes As Date, ByVal RetornoViernes As Date, _
      ByVal Sabado As Integer, ByVal EntradaSabado As Date, ByVal SalidaSabado As Date, ByVal DescansoSabado As Date, ByVal RetornoSabado As Date, _
      ByVal Domingo As Integer, ByVal EntradaDomingo As Date, ByVal SalidaDomingo As Date, ByVal DescansoDomingo As Date, ByVal RetornoDomingo As Date)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsnomcattur SET "
        Else
            strSQLInicio = " UPDATE jsnomcattur SET "
            strSQLFin = " WHERE " _
                & " codtur = " & CodigoTurno & " and " _
                & " id_emp = '" & jytsistema.WorkID & "' "

        End If
        strSQL += ModificarCadena(CodigoTurno, "codtur")
        strSQL += ModificarCadena(NombreTurno, "nombre")
        strSQL += ModificarEnteroLargo(TipoNomina, "tipo")
        strSQL += ModificarHora(HoraExtraDiurna, "horadiurna")
        strSQL += ModificarHora(HoraExtraNocturna, "horanocturna")
        strSQL += ModificarEnteroLargo(MarcancionesXTurno, "marcaturno")
        strSQL += ModificarEnteroLargo(MarcacionesXDescanso, "marcadescanso")
        strSQL += ModificarEnteroLargo(ToleranciaEntrada, "tol_ent")
        strSQL += ModificarEnteroLargo(ToleranciaSalida, "tol_sal")
        strSQL += ModificarEnteroLargo(ToleranciaDescanso, "tol_ini_des")
        strSQL += ModificarEnteroLargo(ToleranciaRetorno, "tol_fin_des")
        strSQL += ModificarEnteroLargo(Lunes, "L")
        strSQL += ModificarHora(EntradaLunes, "L_E")
        strSQL += ModificarHora(SalidaLunes, "L_S")
        strSQL += ModificarHora(DescansoLunes, "L_DE")
        strSQL += ModificarHora(RetornoLunes, "L_DS")
        strSQL += ModificarEnteroLargo(Martes, "M")
        strSQL += ModificarHora(EntradaMartes, "M_E")
        strSQL += ModificarHora(SalidaMartes, "M_S")
        strSQL += ModificarHora(DescansoMartes, "M_DE")
        strSQL += ModificarHora(RetornoMartes, "M_DS")
        strSQL += ModificarEnteroLargo(Miercoles, "I")
        strSQL += ModificarHora(EntradaMiercoles, "I_E")
        strSQL += ModificarHora(SalidaMiercoles, "I_S")
        strSQL += ModificarHora(DescansoMiercoles, "I_DE")
        strSQL += ModificarHora(RetornoMiercoles, "I_DS")
        strSQL += ModificarEnteroLargo(Jueves, "J")
        strSQL += ModificarHora(EntradaJueves, "J_E")
        strSQL += ModificarHora(SalidaJueves, "J_S")
        strSQL += ModificarHora(DescansoJueves, "J_DE")
        strSQL += ModificarHora(RetornoJueves, "J_DS")
        strSQL += ModificarEnteroLargo(Viernes, "V")
        strSQL += ModificarHora(EntradaViernes, "V_E")
        strSQL += ModificarHora(SalidaViernes, "V_S")
        strSQL += ModificarHora(DescansoViernes, "V_DE")
        strSQL += ModificarHora(RetornoViernes, "V_DS")
        strSQL += ModificarEnteroLargo(Sabado, "S")
        strSQL += ModificarHora(EntradaSabado, "S_E")
        strSQL += ModificarHora(SalidaSabado, "S_S")
        strSQL += ModificarHora(DescansoSabado, "S_DE")
        strSQL += ModificarHora(RetornoSabado, "S_DS")
        strSQL += ModificarEnteroLargo(Domingo, "D")
        strSQL += ModificarHora(EntradaDomingo, "D_E")
        strSQL += ModificarHora(SalidaDomingo, "D_S")
        strSQL += ModificarHora(DescansoDomingo, "D_DE")
        strSQL += ModificarHora(RetornoDomingo, "D_DS")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditNOMINAAsistenciaTrabajador(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
      ByVal CodigoTrabajador As String, ByVal Fecha As Date, _
      ByVal Entrada As Date, ByVal Salida As Date, ByVal Descanso As Date, ByVal Retorno As Date, _
      ByVal HorasTrabajadas As String, ByVal TipoDia As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsnomtratur SET "
        Else
            strSQLInicio = " UPDATE jsnomtratur SET "
            strSQLFin = " WHERE " _
                & " codtra = " & CodigoTrabajador & " and " _
                & " dia = '" & ft.FormatoFechaMySQL(Fecha) & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL += ModificarCadena(CodigoTrabajador, "codtra")
        strSQL += ModificarFecha(Fecha, "dia")
        strSQL += ModificarFechaTiempoPlus(Entrada, "entrada")
        strSQL += ModificarFechaTiempoPlus(Salida, "Salida")
        strSQL += ModificarFechaTiempoPlus(Descanso, "descanso")
        strSQL += ModificarFechaTiempoPlus(Retorno, "retorno")
        strSQL += ModificarCadena(HorasTrabajadas, "horas")
        strSQL += ModificarEnteroLargo(TipoDia, "tipo")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

End Module
