Imports MySql.Data.MySqlClient
Module FuncionesRecursosHumanos

    Private ft As New Transportables

    Public Sub AsignaConceptosATrabajador(MyConn As MySqlConnection, lblInfo As Label, ds As DataSet, _
                                          ByVal CodigoTrabajador As String, CodigoNomina As String)
        Dim nTableConceptos As String = "tblconceptos"
        Dim dtConcepto As DataTable
        Dim hCont As Integer

        dtConcepto = ft.AbrirDataTable(ds, nTableConceptos, MyConn, " select * from jsnomcatcon where  " _
                            & " codnom IN (SELECT codnom FROM jsnomrennom " _
                            & "             WHERE " _
                            & "             codtra = '" & CodigoTrabajador & "' AND " _
                            & "             id_emp = '" & jytsistema.WorkID & "') AND " _
                            & " estatus = 1 and " _
                            & " CODNOM = '" & cODIGOnOMINA & "' AND " _
                            & " id_emp = '" & jytsistema.WorkID & "'  ORDER BY CODCON ")

        For hCont = 0 To dtConcepto.Rows.Count - 1


            With dtConcepto.Rows(hCont)
                Dim CodigoConcepto As String = .Item("CODCON")
                If ft.DevuelveScalarEntero(MyConn, " select count(*) from jsnomtrades where " _
                                           & " codnom = '" & CodigoNomina & "' and " _
                                           & " codtra = '" & CodigoTrabajador & "' and " _
                                           & " codcon = '" & CodigoConcepto & "' and " _
                                           & " id_emp = '" & jytsistema.WorkID & "' ") = 0 Then

                    ft.Ejecutar_strSQL(MyConn, " insert into jsnomtrades set " _
                                       & " CODTRA = '" & CodigoTrabajador & "', " _
                                       & " CODCON = '" & CodigoConcepto & "', " _
                                       & " IMPORTE = 0.00, " _
                                       & " PORCENTAJE_ASIG = 0.00, " _
                                       & " ID_EMP =  '" & jytsistema.WorkID & "' , " _
                                       & " CODNOM = '" & CodigoNomina & "' ")
                End If

                Conceptos_A_Trabajadores(MyConn, lblInfo, ds, .Item("codcon"), .Item("CODNOM"), IIf(IsDBNull(.Item("conjunto")), "", .Item("conjunto")), .Item("estatus"), _
                                IIf(IsDBNull(.Item("formula")), "", .Item("formula")), IIf(IsDBNull(.Item("condicion")), "", .Item("condicion")), CodigoTrabajador, CodigoTrabajador)
            End With
        Next


        dtConcepto.Dispose()
        dtConcepto = Nothing

    End Sub

    Public Sub Conceptos_A_Trabajadores(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal ds As DataSet, ByVal CodigoConcepto As String, _
                                        CodigoNomina As String, _
                                        ByVal ConjuntoConcepto As String, ByVal EstatusConcepto As Integer, _
                                        ByVal strFormula As String, ByVal strWhere As String, _
                                        Optional ByVal CodTraDesde As String = "", Optional ByVal CodtraHasta As String = "")

        Dim nTableWorker As String = "tblWorker"
        Dim dtTrabajadores As DataTable

        Dim iCont As Integer
        Dim strCodTra As String = "", strCodTra1 As String = ""

        If Trim(strWhere) <> "" Then strWhere = " and " & AsignarConstantes(MyConn, ds, strWhere, lblInfo)
        If Trim(strFormula) <> "" Then strFormula = AsignarConstantes(MyConn, ds, strFormula, lblInfo)

        If Trim(CodTraDesde) <> "" Then strCodTra += " a.codtra >= '" & CodTraDesde & "' and "
        If Trim(CodtraHasta) <> "" Then strCodTra += " a.codtra <= '" & CodtraHasta & "' and "
        If Trim(CodTraDesde) <> "" Then strCodTra1 += " codtra >= '" & CodTraDesde & "' and "
        If Trim(CodtraHasta) <> "" Then strCodTra1 += " codtra <= '" & CodtraHasta & "' and "

        '1. Eliminar concepto asginado anteriormente en todos los trabajadores

        If strFormula <> "" Then

            If strFormula <> "0" Then ft.Ejecutar_strSQL(myconn, " delete from jsnomtrades " _
                & " where codcon = '" & CodigoConcepto & "' and CODNOM = '" & CodigoNomina & "' and " & strCodTra1 & " id_emp = '" & jytsistema.WorkID & "'")

            '2. Asigno el concepto nuevo ó modificado a todos los trabajadores
            If EstatusConcepto > 0 Then

                ds = DataSetRequery(ds, " SELECT a.codtra FROM jsnomcattra a " _
                                            & " LEFT JOIN jsnomrennom b ON (a.codtra = b.codtra and a.id_emp = b.id_emp)  " _
                                            & " where " _
                                            & " b.codnom = '" & CodigoNomina & "' AND " _
                                            & strCodTra _
                                             & " a.id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTableWorker, lblInfo)

                dtTrabajadores = ds.Tables(nTableWorker)
                If dtTrabajadores.Rows.Count > 0 Then
                    For iCont = 0 To dtTrabajadores.Rows.Count - 1

                        With dtTrabajadores.Rows(iCont)
                            Dim strFecha As String = " '" & ft.FormatoFechaMySQL(jytsistema.sFechadeTrabajo) & "'"
                            Dim strEmpresa As String = "'" & jytsistema.WorkID & "'"
                            Dim strTrabajador As String = "'" & .Item("codtra") & "'"
                            Dim strMontoConcepto As String = Replace(Replace(Replace(ConsultaAPartirDeConcepto(MyConn, ds, CodigoConcepto, .Item("codtra"), CodigoNomina, lblInfo), "@Fecha", strFecha), "@Empresa", strEmpresa), "@Trabajador", strTrabajador)

                            Dim MontoConcepto As Double = ft.DevuelveScalarDoble(MyConn, strMontoConcepto)
                            Dim aCamT() As String = {"codtra", "codcon", "id_emp", "CODNOM"}
                            Dim aStrT() As String = {.Item("codtra"), CodigoConcepto, jytsistema.WorkID, CodigoNomina}
                            If MontoConcepto >= 0 Then

                                Dim ConceptoAsignacion As String = ft.DevuelveScalarCadena(MyConn, " select concepto_por_asig from jsnomcatcon where codcon = '" & CodigoConcepto & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                Dim MontoConceptoAsignacion As Double = ft.DevuelveScalarDoble(MyConn, "select importe from jsnomtrades where codtra = '" & .Item("codtra") & "' and codcon = '" & ConceptoAsignacion & "' and id_emp = '" & jytsistema.WorkID & "' ")
                                Dim PorcentajeAsignacion As Double = 0
                                If MontoConceptoAsignacion > 0 Then PorcentajeAsignacion = MontoConcepto / MontoConceptoAsignacion * 100
                                If qFound(MyConn, lblInfo, "jsnomtrades", aCamT, aStrT) Then
                                    If strFormula <> "0" Then InsertEditNOMINAConceptoTrabajador(MyConn, lblInfo, False, .Item("codtra"), CodigoConcepto, MontoConcepto, PorcentajeAsignacion, CodigoNomina)
                                Else
                                    InsertEditNOMINAConceptoTrabajador(MyConn, lblInfo, True, .Item("codtra"), CodigoConcepto, MontoConcepto, PorcentajeAsignacion, CodigoNomina)
                                End If
                            End If

                        End With
                    Next
                End If
            Else
                ft.Ejecutar_strSQL(MyConn, " delete from jsnomtrades " _
                & " where codcon = '" & CodigoConcepto & "' and CODNOM = '" & CodigoNomina & "' and " & strCodTra1 & " id_emp = '" & jytsistema.WorkID & "'")
            End If
        End If
    End Sub
    Public Function ConsultaAPartirDeConcepto(ByVal MyConn As MySqlConnection, ByVal ds As DataSet, ByVal NumeroConcepto As String, _
                                              ByVal CodigoTrabajador As String, CodigoNomina As String, ByVal lblInfo As Label) As String

        Dim aCam() As String = {"codnom", "codcon", "id_emp"}
        Dim aStr() As String = {CodigoNomina, NumeroConcepto, jytsistema.WorkID}

        Dim sConjunto As String = qFoundAndSign(MyConn, lblInfo, "jsnomcatcon", aCam, aStr, "conjunto")
        Dim tblConjunto As String = "tblConjunto"
        Dim dtConjunto As DataTable
        ds = DataSetRequery(ds, " select * from jsconcojtab where codigo = '" & sConjunto & "' and id_emp = '" & jytsistema.WorkID & "' order by letra ", MyConn, tblConjunto, lblInfo)
        dtConjunto = ds.Tables(tblConjunto)


        Dim str As String = " SELECT "
        str += AsignarConceptos(MyConn, ds, AsignarConstantes(MyConn, ds, qFoundAndSign(MyConn, lblInfo, "jsnomcatcon", aCam, aStr, "FORMULA"), lblInfo), CodigoTrabajador, CodigoNomina, lblInfo)
        str += " FROM "

        Dim gCont As Integer
        For gCont = 0 To dtConjunto.Rows.Count - 1
            Dim aInner() As String = {"", " LEFT JOIN ", " INNER JOIN ", "RIGTH JOIN "}
            With dtConjunto.Rows(gCont)
                str += aInner(.Item("TIPO")) + .Item("tabla") + " " + .Item("letra") + " " + IIf(.Item("RELACION") <> "", " ON ", "") + .Item("RELACION") + " "
            End With
        Next

        If qFoundAndSign(MyConn, lblInfo, "jsnomcatcon", aCam, aStr, "CONDICION") <> "" Then
            str += " WHERE " + AsignarConstantes(MyConn, ds, qFoundAndSign(MyConn, lblInfo, "jsnomcatcon", aCam, aStr, "CONDICION"), lblInfo)
        End If

        If qFoundAndSign(MyConn, lblInfo, "jsnomcatcon", aCam, aStr, "AGRUPADOPOR") <> "" Then
            str += " GROUP BY " + qFoundAndSign(MyConn, lblInfo, "jsnomcatcon", aCam, aStr, "AGRUPADOPOR")
        End If

        ConsultaAPartirDeConcepto = str

    End Function
    Private Function AsignarConstantes(ByVal MyConn As MySqlConnection, ByVal ds As DataSet, ByVal str As String, ByVal lblInfo As Label) As String
        Dim dtConstantes As DataTable
        Dim nTablaCons As String = "tblconstantes"
        Dim fCont As Integer
        ds = DataSetRequery(ds, " select * from jsnomcatcot where id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaCons, lblInfo)
        dtConstantes = ds.Tables(nTablaCons)
        If dtConstantes.Rows.Count > 0 Then
            For fCont = 0 To dtConstantes.Rows.Count - 1
                str = Replace(str, "[" & dtConstantes.Rows(fCont).Item("constante") & "]", dtConstantes.Rows(fCont).Item("valor"))
            Next
        End If
        AsignarConstantes = str
        dtConstantes.Dispose()
        dtConstantes = Nothing
    End Function
    Private Function AsignarConceptos(ByVal MyConn As MySqlConnection, ByVal ds As DataSet, ByVal str As String, _
                                      ByVal CodigoTrabajador As String, CodigoNomina As String, ByVal lblInfo As Label) As String
        Dim dtConcepto As DataTable
        Dim nTabla As String = "tblconceptos"
        Dim fCont As Integer
        ds = DataSetRequery(ds, " select * from jsnomtrades where codtra = '" & CodigoTrabajador & "' and id_emp = '" & jytsistema.WorkID & "' and codnom = '" & CodigoNomina & "' ", MyConn, nTabla, lblInfo)
        dtConcepto = ds.Tables(nTabla)
        If dtConcepto.Rows.Count > 0 Then
            For fCont = 0 To dtConcepto.Rows.Count - 1
                With dtConcepto.Rows(fCont)
                    str = Replace(str, "{" & .Item("codcon") & "}", .Item("importe"))
                End With
            Next
        End If

        'SE ELIMINARAN LOS CONCEPTOS MAL ASIGNADOS 
        ds = DataSetRequery(ds, " select * from jsnomcatcon where id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTabla, lblInfo)
        dtConcepto = ds.Tables(nTabla)
        If dtConcepto.Rows.Count > 0 Then
            For fCont = 0 To dtConcepto.Rows.Count - 1
                With dtConcepto.Rows(fCont)
                    str = Replace(str, "{" & .Item("codcon") & "}", "0")
                End With
            Next
        End If

        AsignarConceptos = str
        dtConcepto.Dispose()
        dtConcepto = Nothing
    End Function

    Public Function IniciarDiasTrabajo(ByVal MyConn As MySqlConnection, ByVal ds As DataSet, ByVal FechaTrabajo As Date, ByVal lblProc As Label, _
                                  Optional ByVal CodigoTrabajador As String = "") As Boolean

        If Not ProcesoAutomaticoInicial(MyConn, lblProc, ProcesoAutomatico.iDiasDeTrabajo, jytsistema.sFechadeTrabajo) Then


            Dim dtT As DataTable
            Dim ntablaT As String = "tblT"
            Dim iCont As Integer


            ds = DataSetRequery(ds, "select codtra, codhp from jsnomcattra where " & IIf(CodigoTrabajador <> "", " codtra = '" & CodigoTrabajador & "' and ", "") & " id_emp = '" & jytsistema.WorkID & "' order by codtra ", MyConn, _
                                ntablaT, lblProc)

            dtT = ds.Tables(ntablaT)

            Dim FechaInicial As Date
            FechaInicial = CDate("01/01/" & Year(jytsistema.sFechadeTrabajo))
            Dim Fecha As Date

            If dtT.Rows.Count > 0 Then
                For iCont = 0 To dtT.Rows.Count - 1
                    With dtT.Rows(iCont)

                        Application.DoEvents()
                        Fecha = FechaInicial
                        While Fecha <= FechaTrabajo
                            Dim afldd() As String = {"codtra", "dia", "id_emp"}
                            Dim aStrr() As String = {.Item("codtra"), ft.FormatoFechaMySQL(Fecha), jytsistema.WorkID}
                            If Not qFound(MyConn, lblProc, "jsnomtratur", afldd, aStrr) Then
                                Dim aTipo As Integer
                                aTipo = TipoMarca(MyConn, ds, .Item("codtra"), Fecha, lblProc)

                                ft.Ejecutar_strSQL(MyConn, "insert into jsnomtratur set codtra = '" & .Item("codtra") _
                                    & "', codhp = '" & .Item("codhp") _
                                    & "', dia = '" & ft.FormatoFechaMySQL(Fecha) & "', " _
                                    & " entrada = '" & ft.FormatoFechaHoraMySQL(Fecha) & "', " _
                                    & " salida = '" & ft.FormatoFechaHoraMySQL(Fecha) & "', " _
                                    & " descanso = '" & ft.FormatoFechaHoraMySQL(Fecha) & "', " _
                                    & " retorno = '" & ft.FormatoFechaHoraMySQL(Fecha) & "', " _
                                    & " tipo = " & aTipo & ", id_emp = '" & jytsistema.WorkID & "' ")
                            End If
                            Fecha = DateAdd(DateInterval.Day, 1, Fecha)
                        End While
                    End With
                Next
            End If

            dtT.Dispose()
            dtT = Nothing

            ActualizarProcesoAutomaticoInicial(MyConn, lblProc, ProcesoAutomatico.iDiasDeTrabajo, jytsistema.sFechadeTrabajo)

        End If

        Return True

    End Function
    Private Function TipoMarca(ByVal MyConn As MySqlConnection, ByVal ds As DataSet, ByVal CodigoTrabajador As String, ByVal Fecha As Date, ByVal lblInfo As Label) As Integer

        TipoMarca = 0
        If TipoMarca = 0 And DiaLibreTurno(MyConn, ds, CodigoTrabajador, Fecha, lblInfo) Then TipoMarca = 2
        If TipoMarca = 0 And DiaLibreFeriado(MyConn, Fecha, lblInfo) Then TipoMarca = 3
        If TipoMarca = 0 And DiaLibreContratoColectivo(MyConn, ds, Fecha, CodigoTrabajador, lblInfo) Then TipoMarca = 1

    End Function
    Public Function DiaLibreTurno(ByVal MyConn As MySqlConnection, ByVal ds As DataSet, _
        ByVal CodigoTrabajador As String, ByVal Fecha As Date, ByVal lblInfo As Label) As Boolean

        DiaLibreTurno = False
        Dim tblTurno As String = "tblTurno"
        Dim dtTurno As DataTable

        ds = DataSetRequery(ds, " select a.codtra, b.* from jsnomturtra a " _
            & " left join jsnomcattur b on (a.codtur = b.codtur and a.id_emp = b.id_emp) " _
            & " where " _
            & " a.codtra = '" & CodigoTrabajador & "' and " _
            & " a.id_emp = '" & jytsistema.WorkID & "' ", MyConn, tblTurno, lblInfo)

        dtTurno = ds.Tables(tblTurno)

        Dim iDay As Integer
        iDay = Weekday(Fecha, vbMonday)
        If dtTurno.Rows.Count > 0 Then
            With dtTurno.Rows(0)
                Select Case iDay
                    Case 1 'lunes
                        DiaLibreTurno = Not .Item("L")
                    Case 2 'martes
                        DiaLibreTurno = Not .Item("M")
                    Case 3 'miércoles
                        DiaLibreTurno = Not .Item("I")
                    Case 4 'jueves
                        DiaLibreTurno = Not .Item("J")
                    Case 5 'viernes
                        DiaLibreTurno = Not .Item("V")
                    Case 6 'sábado
                        DiaLibreTurno = Not .Item("S")
                    Case 7 'domingo
                        DiaLibreTurno = Not .Item("D")
                End Select
            End With
        End If

        dtTurno.Dispose()
        dtTurno = Nothing

    End Function
    Public Function DiaLibreFeriado(ByVal MyConn As MySqlConnection, ByVal Fecha As Date, ByVal lblInfo As Label) As Boolean
        DiaLibreFeriado = False
        Dim afld() As String = {"mes", "ano", "dia", "MODULO", "TIPO", "id_emp"}
        Dim aStr() As String = {Month(Fecha), Year(Fecha), Fecha.Day, 0, 1, jytsistema.WorkID}
        If qFound(MyConn, lblInfo, "jsconcatper", afld, aStr) Then DiaLibreFeriado = True

    End Function
    Public Function DiaLibreContratoColectivo(ByVal MyConn As MySqlConnection, ByVal ds As DataSet, ByVal Fecha As Date, ByVal CodigoTrabajador As String, ByVal lblInfo As Label) As Boolean

        DiaLibreContratoColectivo = False

        Dim dtLibre As DataTable
        Dim tblLibre As String = "tblLibre"

        ds = DataSetRequery(ds, " select codtra, freedays, periodo, datefreeday, rotatorio from jsnomcattra where codtra = '" & CodigoTrabajador & "' and id_emp = '" & jytsistema.WorkID & "' ", _
                             MyConn, tblLibre, lblInfo)

        dtLibre = ds.Tables(tblLibre)

        If dtLibre.Rows.Count > 0 Then
            With dtLibre.Rows(0)
                Dim DiasLibres As Integer = .Item("freedays")
                Dim Periodo As Integer = .Item("periodo")
                Dim FechaInicio As Date = CDate(.Item("datefreeday").ToString)
                Dim Rotatorio As Integer = .Item("rotatorio")
                If DiasLibres > 0 Then
                    Dim FechaLibre As Date = FechaInicio
                    Dim Intervalo As Integer
                    Select Case Periodo
                        Case 0
                            Intervalo = 7
                        Case 1
                            Intervalo = 15
                        Case 2
                            Intervalo = 30
                        Case 3
                            Intervalo = 365
                    End Select
                    While FechaLibre < Fecha
                        If DiaLibreTurno(MyConn, ds, CodigoTrabajador, FechaLibre, lblInfo) _
                            Or DiaLibreFeriado(MyConn, FechaLibre, lblInfo) Then
                            FechaLibre = DateAdd(DateInterval.Day, 1, FechaLibre)
                        Else
                            FechaLibre = DateAdd(DateInterval.Day, Intervalo, FechaLibre)
                        End If
                    End While
                    If FechaLibre = Fecha Then DiaLibreContratoColectivo = True
                End If
            End With
        End If

        dtLibre.Dispose()
        dtLibre = Nothing

    End Function

    Public Sub EliminarAsignacionesYDeducciones(MyConn As MySqlConnection, lblInfo As Label, CodigoTrabajador As String)
        ft.Ejecutar_strSQL(myconn, " delete from jsnomtrades where codtra = '" & CodigoTrabajador & "' and id_emp = '" & jytsistema.WorkID & "' ")
    End Sub

End Module
