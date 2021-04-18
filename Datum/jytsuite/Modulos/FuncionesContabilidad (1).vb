Imports MySql.Data.MySqlClient
Module FuncionesContabilidad
    Public Structure CodRegType
        Private Nivel As Integer
        Private Codigo As String
    End Structure

    Public EstructuraCod() As Integer
    Public LongNivelesCod() As Integer
    Public CerosEstruCod() As String
    Public EstructuraCodActual() As Integer
    Public LongNivelesCodActual() As Integer
    Public CadenaNivelesCodActual() As String
    Public NivelReg() As CodRegType
    Public Function CountPredictor(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal numCuenta As String) As String
        Dim nCuenta As String = numCuenta
        If nCuenta = "" Then
            nCuenta = "1."
        Else
            Dim Encontrado As Boolean = True
            While Encontrado
                nCuenta = IncrementarCadena(nCuenta)
                Dim aFld() As String = {"codcon", "id_emp"}
                Dim aStr() As String = {nCuenta, jytsistema.WorkID}
                If qFound(MyConn, lblinfo, "jscotcatcon", aFld, aStr) Then
                    Encontrado = True
                Else
                    Encontrado = False
                End If
            End While
        End If
        CountPredictor = nCuenta
    End Function
    Public Function ValidarEstructuraActual(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal CadenaPrueb As String, ByVal EstructuraPrueb() As Integer, ByVal LongNivelesPrueb() As Integer) As Boolean

        Dim i As Integer

        If EstructuraValida(Myconn, lblInfo, CadenaPrueb, EstructuraPrueb, LongNivelesPrueb) Then
            ValidarEstructuraActual = True

            If UBound(EstructuraCod) >= UBound(EstructuraPrueb) Then
                For i = 0 To UBound(EstructuraPrueb)
                    If EstructuraPrueb(i) <> EstructuraCod(i) Then
                        ValidarEstructuraActual = False
                        Exit Function
                    End If
                Next i
                If Not ValidarCaracteres(CadenaPrueb) Then
                    ValidarEstructuraActual = False
                End If
                If UBound(LongNivelesCod) >= UBound(LongNivelesPrueb) Then
                    For i = 0 To UBound(LongNivelesPrueb)
                        If LongNivelesPrueb(i) <> LongNivelesCod(i) Then
                            ValidarEstructuraActual = False
                            Exit Function
                        End If
                    Next i
                End If
            End If
        End If
    End Function
    Public Function EstructuraValida(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Cadena As String, ByVal Estructura() As Integer, ByVal LongEstruc() As Integer) As Boolean
        Dim i As Integer
        EstructuraValida = True
        If Len(Cadena) > 0 Then
            ReDim Estructura(0)
            For i = 1 To Len(Cadena)
                If Mid(Cadena, i, 1) = "." Then
                    Estructura(Estructura.Length) = i
                    ReDim Preserve Estructura(Estructura.Length + 1)
                End If
            Next
            If Estructura.Length > 0 Then
                ReDim Preserve Estructura(Estructura.Length - 1)
                For i = 1 To Estructura.Length
                    If Estructura(i) - Estructura(i - 1) = 1 Then
                        EstructuraValida = False
                        Exit Function
                    End If
                Next
            Else
                Estructura(0) = Len(Cadena) + 1
            End If
            If Mid(Cadena, 1, 1) = "." Or (Len(Cadena) = Len(ParametroPlus(MyConn, Gestion.iContabilidad, "CONPARAM01")) _
              And Mid(Cadena, Len(Cadena), 1) = ".") Then
                EstructuraValida = False
                Exit Function
            End If
            Call DeterminarLongitudNiveles(Cadena, LongEstruc)
        Else
            EstructuraValida = False
        End If
    End Function
    Public Sub DeterminarLongitudNiveles(ByVal Cadena As String, ByVal LongEstruc() As Integer)
        Dim i As Integer
        Dim j As Integer
        If Len(Cadena) > 0 Then
            ReDim LongEstruc(0)
            ReDim CadenaNivelesCodActual(0)
            j = 0
            For i = 1 To Len(Cadena)
                CadenaNivelesCodActual(j) = CadenaNivelesCodActual(j) + Mid(Cadena, i, 1)
                If Mid(Cadena, i, 1) = "." Then
                    If i <> Len(Cadena) Then
                        j = j + 1
                        ReDim Preserve LongEstruc(j)
                        ReDim Preserve CadenaNivelesCodActual(j)
                        CadenaNivelesCodActual(j) = CadenaNivelesCodActual(j - 1)
                    End If
                Else
                    LongEstruc(j) = LongEstruc(j) + 1
                End If
            Next i
        End If
    End Sub
    Public Function ValidarCaracteres(ByVal Cadena As String) As Boolean
        Dim i As Integer
        Dim Caracter As String
        If Len(Cadena) > 0 Then
            ValidarCaracteres = True
            For i = 1 To Len(Cadena)
                Caracter = Asc(Mid(Cadena, i, 1))
                If Not (Caracter = 46 Or _
                  (Caracter > 47 And Caracter < 58) Or _
                  (Caracter > 64 And Caracter < 91)) Then
                    ValidarCaracteres = False
                    Exit Function
                End If
            Next
        End If
    End Function
    Public Function NivelCuentaContable(ByVal CodigoCuenta As String) As Integer
        NivelCuentaContable = Math.Round((Len(CodigoCuenta) - Len(Replace(CodigoCuenta, ".", ""))) / Len(".") + If(Right(CodigoCuenta, 1) = ".", 0, 1), 0)
    End Function
    Public Sub ActualizaSaldosCuentasContables(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCuenta As String, ByVal Fecha As Date)

        Dim aCuenta() As String
        Dim iCont As Integer
        Dim codCuenta As String = ""

        aCuenta = Split(CodigoCuenta, ".")
        For iCont = 0 To UBound(aCuenta)
            codCuenta += aCuenta(iCont) & IIf(iCont = UBound(aCuenta), "", ".")
            ActualizaSaldoCuenta(MyConn, lblInfo, codCuenta, Fecha)
        Next

    End Sub
    Public Sub ActualizaSaldoCuenta(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoCuenta As String, ByVal Fecha As Date)

        Dim strFecha As String
        Dim strMes As String

        If Fecha < FechaInicioEjercicio(MyConn, lblInfo) Then
            strFecha = " b.fechasi < '" & FormatoFechaMySQL(FechaInicioEjercicio(MyConn, lblInfo)) & "' and "
            strMes = "00"
        Else
            strFecha = " month(b.fechasi) = " & Month(Fecha) & " and " _
                & " year(b.fechasi) = " & Year(Fecha) & " and "
            strMes = Format(Month(Fecha), "00")
        End If

        Dim aFld() As String = {"codcon", "ejercicio", "id_emp"}
        Dim aSFld() As String = {CodigoCuenta, jytsistema.WorkExercise, jytsistema.WorkID}
        If Not qFound(MyConn, lblInfo, "jscotdaacon", aFld, aSFld) Then _
             InsertEditCONTABMovimientoCuentaContable(MyConn, lblInfo, True, CodigoCuenta, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)

        EjecutarSTRSQL(MyConn, lblInfo, " update jscotdaacon a, (select substring(a.codcon,1," & Len(CodigoCuenta) & ") codcon, sum( if( a.debito_credito = 0 , a.importe, 0) ) debitos, " _
                & " sum( if( a.debito_credito = 1 , a.importe, 0) ) creditos, a.ejercicio, a.id_emp " _
                & " from jscotrenasi a " _
                & " Left join jscotencasi b on (a.asiento = b.asiento and a.actual = b.actual and a.ejercicio = b.ejercicio And a.id_emp = b.id_emp) " _
                & " Where " _
                & " substring(a.codcon,1," & Len(CodigoCuenta) & ") = '" & CodigoCuenta & "' and " _
                & " b.actual = 1 and " _
                & strFecha _
                & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " a.id_emp = '" & jytsistema.WorkID & "' " _
                & " group by substring(a.codcon,1," & Len(CodigoCuenta) & ")) b " _
                & " set a.deb" & strMes & " = b.debitos, a.cre" & strMes & " = b.creditos " _
                & " Where " _
                & " a.codcon = b.codcon and " _
                & " a.ejercicio = b.ejercicio and " _
                & " a.id_emp = b.id_emp and " _
                & " a.ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " a.id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Public Function ConsultaAPartirDeRegla(ByVal MyConn As MySqlConnection, ByVal ds As DataSet, ByVal NumeroRegla As String, ByVal lblInfo As Label) As String

        Dim aCam() As String = {"plantilla", "id_emp"}
        Dim aStr() As String = {NumeroRegla, jytsistema.WorkID}

        Dim sConjunto As String = qFoundAndSign(MyConn, lblInfo, "jscotcatreg", aCam, aStr, "conjunto")
        Dim tblConjunto As String = "tblConjunto"
        Dim dtConjunto As DataTable
        ds = DataSetRequery(ds, " select * from jsconcojtab where codigo = '" & sConjunto & "' and id_emp = '" & jytsistema.WorkID & "' order by letra ", MyConn, tblConjunto, lblInfo)
        dtConjunto = ds.Tables(tblConjunto)


        Dim str As String = " SELECT "
        Dim aaStr() As String = qFoundAndSign(MyConn, lblInfo, "jscotcatreg", aCam, aStr, "FORMULA").ToString.Split(";")

        str += aaStr(0) + " IMPORTEREGLA "
        If aaStr.Length > 1 Then
            For ii As Integer = 1 To aaStr.Length - 1
                str += ", " + aaStr(ii)
            Next
        End If


        Dim gCont As Integer
        Dim strXX As String = ""
        For gCont = 0 To dtConjunto.Rows.Count - 1
            Dim aInner() As String = {"", " LEFT JOIN ", " INNER JOIN ", "RIGTH JOIN "}
            With dtConjunto.Rows(gCont)
                str += ", " + .Item("letra") + ".* "
                strXX += aInner(.Item("TIPO")) + .Item("tabla") + " " + .Item("letra") + " " + IIf(.Item("RELACION") <> "", " ON ", "") + .Item("RELACION") + " "
            End With
        Next

        str += " FROM " & strXX

        If qFoundAndSign(MyConn, lblInfo, "jscotcatreg", aCam, aStr, "CONDICION") <> "" Then _
            str += " WHERE " + qFoundAndSign(MyConn, lblInfo, "jscotcatreg", aCam, aStr, "CONDICION")

        If qFoundAndSign(MyConn, lblInfo, "jscotcatreg", aCam, aStr, "AGRUPADOPOR") <> "" Then _
            str += " GROUP BY " + qFoundAndSign(MyConn, lblInfo, "jscotcatreg", aCam, aStr, "AGRUPADOPOR")

        ConsultaAPartirDeRegla = str

    End Function

   
    Public Sub CargarListaDesdeAsientosDefinidos(ByVal dg As DataGridView, ByVal dt As DataTable)

        Dim aFld() As String = {"sel", "asiento", "descripcion", "fecha_ult_con", "inicio_ult_con", "fin_ult_con"}
        Dim aNom() As String = {"", "Asiento", "Descripción Asiento", "Fecha Ultima Contabilización", "Inicio período última contabilización", "Fin período última contabilización"}
        Dim aAnc() As Long = {20, 60, 380, 120, 120, 120}
        Dim aAli() As Integer = {AlineacionDataGrid.Centro, AlineacionDataGrid.Centro, AlineacionDataGrid.Izquierda, AlineacionDataGrid.Centro, _
                                     AlineacionDataGrid.Centro, AlineacionDataGrid.Centro}
        Dim aFor() As String = {"", "", "", sFormatoFechaCorta, sFormatoFechaCorta, sFormatoFechaCorta}


        IniciarTablaSeleccion(dg, dt, aFld, aNom, aAnc, aAli, aFor)

    End Sub

    Public Sub Contabilizar_Plantilla(ByVal MyConn As MySqlConnection, lblInfo As Label, ByVal ds As DataSet, _
                                      ByVal Fecha As Date, ByVal NumeroPlantilla As String, ByVal DescripcionPlantilla As String, _
                                      FechaInicial As Date, FechaFinal As Date, lblProgreso As Label, pb As ProgressBar, _
                                      Optional numAsiento As String = "")

        Dim IncluyeAsiento As Boolean = False
        Dim NumeroAsiento As String = IIf(numAsiento <> "", numAsiento, AutoCodigoMensual(MyConn, lblInfo, "jscotencasi", "asiento", Fecha))
        Dim Etiqueta As String = lblProgreso.Text

        Dim tblPlantilla As String = "tblPlantilla"
        Dim dtPlantilla As DataTable
        ds = DataSetRequery(ds, " Select * from jscotrendef where asiento = '" & NumeroPlantilla & "' and id_emp = '" & jytsistema.WorkID & "' order by renglon ", MyConn, tblPlantilla, lblInfo)
        dtPlantilla = ds.Tables(tblPlantilla)

        Dim strFecha As String = "'" & FormatoFechaMySQL(Fecha) & "'"
        Dim strEmpresa As String = "'" & jytsistema.WorkID & "'"
        Dim strFechaDesde As String = "'" & FormatoFechaMySQL(FechaInicial) & "'"
        Dim strFechaHasta As String = "'" & FormatoFechaMySQL(FechaFinal) & "'"

        If dtPlantilla.Rows.Count > 0 Then
            pb.Value = 0
            Dim fCont As Integer
            For fCont = 0 To dtPlantilla.Rows.Count - 1
                pb.Value = (fCont + 1) / dtPlantilla.Rows.Count * 100
                With dtPlantilla.Rows(fCont)

                    lblProgreso.Text = Etiqueta + " Renglon : " + .Item("renglon") + " " + .Item("concepto")
                    refrescaBarraprogresoEtiqueta(pb, lblProgreso)
                    'refrescaBarraprogresoEtiqueta(ProgressBar1, lblProgreso)

                    Dim nRenglon As String = .Item("renglon")
                    Dim nCodCon As String = .Item("codcon")
                    Dim nRefer As String = .Item("referencia")
                    Dim nConcepto As String = .Item("concepto")
                    Dim nRegla As String = .Item("regla")
                    Dim nSigno As Integer = IIf(.Item("signo") = 0, 1, -1)


                    Dim strConsul As String = Replace(Replace(Replace(Replace(ConsultaAPartirDeRegla(MyConn, ds, nRegla, lblInfo), _
                                                          "@Empresa", strEmpresa), _
                                                              "@FechaDesde", strFechaDesde), _
                                                                 "@FechaHasta", strFechaHasta), _
                                                                    "@Fecha", strFecha)

                    Dim nTablaRenglonAsiento As String = "tblRenglonAsiento"
                    Dim dtRenglonAsiento As DataTable
                    ds = DataSetRequery(ds, strConsul, MyConn, nTablaRenglonAsiento, lblInfo)

                    dtRenglonAsiento = ds.Tables(nTablaRenglonAsiento)

                    If dtRenglonAsiento.Rows.Count > 0 Then
                        For gCont As Integer = 0 To dtRenglonAsiento.Rows.Count - 1

                            Dim xCodcon As String = nCodCon

                            If nCodCon.Trim.Substring(0, 1) = "{" Then
                                xCodcon = dtRenglonAsiento.Rows(gCont).Item("CODIGOCONTABLE").ToString
                            End If

                            Dim xRefer As String = ""
                            If nRefer <> "" Then
                                xRefer = ReemplazarCampoEnCadena(nRefer, dtRenglonAsiento.Rows(gCont)).Trim()  '   dtRenglonAsiento.Rows(gCont).Item(nRefer.Split(".")(1))
                            End If

                            Dim xConcepto As String = ""
                            If nConcepto <> "" Then
                                xConcepto = ReemplazarCampoEnCadena(nConcepto, dtRenglonAsiento.Rows(gCont)) '   dtRenglonAsiento.Rows(gCont).Item(nRefer.Split(".")(1))
                            End If

                            Dim Monto As Double = Math.Abs(CDbl(dtRenglonAsiento.Rows(gCont).Item("IMPORTEREGLA")))
                            nRenglon = Format(fCont, "00000") + Format(gCont, "00000") ''.Item("renglon") + Format(gCont, "00000")
                            If Monto > 0 Then
                                IncluyeAsiento = True
                                Dim aFld() As String = {"asiento", "renglon", "codcon", "referencia", "actual", "ejercicio", "id_emp"}
                                Dim aFldN() As String = {NumeroAsiento, nRenglon, xCodcon, xRefer, 0, jytsistema.WorkExercise, jytsistema.WorkID}
                                Dim InsertarRenglon As Boolean = False
                                If Not qFound(MyConn, lblInfo, "jscotrenasi", aFld, aFldN) Then InsertarRenglon = True
                                InsertEditCONTABRenglonAsiento(MyConn, lblInfo, InsertarRenglon, NumeroAsiento, nRenglon, xCodcon, xRefer, _
                                     xConcepto, nSigno * Monto, 0, IIf(nSigno > 0, 0, 1), 0, nRegla)
                            End If

                        Next
                    End If

                    dtRenglonAsiento.Dispose()
                    dtRenglonAsiento = Nothing

                End With
            Next
        End If

        dtPlantilla.Dispose()
        dtPlantilla = Nothing

        Dim Debitos As Double = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select sum(importe) debitos from jscotrenasi where importe > 0 and asiento = '" & NumeroAsiento & "' and id_emp = '" & jytsistema.WorkID & "' group by asiento ")
        Dim Creditos As Double = EjecutarSTRSQL_Scalar(MyConn, lblInfo, " select sum(importe) creditos from jscotrenasi where importe <= 0 and asiento = '" & NumeroAsiento & "' and id_emp = '" & jytsistema.WorkID & "' group by asiento ")

        If IncluyeAsiento Then
            Dim InsertarEncab As Boolean = False
            Dim aCam() As String = {"asiento", "actual", "ejercicio", "id_emp"}
            Dim aStr() As String = {NumeroAsiento, 0, jytsistema.WorkExercise, jytsistema.WorkID}
            If Not qFound(MyConn, lblInfo, "jscotencasi", aCam, aStr) Then InsertarEncab = True

            InsertEditCONTABEncabezadoAsiento(MyConn, lblInfo, InsertarEncab, NumeroAsiento, NumeroAsiento, Fecha, DescripcionPlantilla, _
                                 Debitos, Creditos, 0, NumeroPlantilla)

        End If


    End Sub
    Public Sub ActualizaCuentasSegunAsiento(MyConn As MySqlConnection, lblInfo As Label, ds As DataSet, _
                                            numAsiento As String, FechaAsiento As Date)

        Dim dtMov As DataTable
        Dim nTablaMov As String = "tblMov1"
        ds = DataSetRequery(ds, " select * from jscotrenasi where asiento = '" & numAsiento & "' and id_emp = '" & jytsistema.WorkID & "' order by renglon ", _
                             MyConn, nTablaMov, lblInfo)
        dtMov = ds.Tables(nTablaMov)

        If dtMov.Rows.Count > 0 Then
            For Each nRow As DataRow In dtMov.Rows
                With nRow
                    ActualizaSaldosCuentasContables(MyConn, lblInfo, .Item("codcon"), FechaAsiento)
                End With
            Next
        End If

    End Sub

End Module
