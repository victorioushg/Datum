Imports MySql.Data.MySqlClient
Module TablasContabilidad
    Private ft As New Transportables
    Public Sub InsertEditCONTABCuentaContable(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
    ByVal CodigoContable As String, ByVal Descripcion As String, ByVal Nivel As Integer, _
    ByVal Marca As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jscotcatcon SET "
        Else
            strSQLInicio = " UPDATE jscotcatcon set "
            strSQLFin = " WHERE " _
                & " codcon = '" & CodigoContable & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarCadena(Descripcion, "descripcion")
        strSQL += ModificarEnteroLargo(Nivel, "nivel")
        strSQL += ModificarEnteroLargo(Marca, "marca")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

        Dim incluyeMov As Boolean = False
        If ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from jscotdaacon where codcon = '" & CodigoContable & "' and ejercicio = '" & jytsistema.WorkExercise & "' and id_emp = '" & jytsistema.WorkID & "' ") = 0 Then
            InsertEditCONTABMovimientoCuentaContable(MyConn, lblInfo, True, CodigoContable, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
        End If

    End Sub

    Public Sub InsertEditCONTABMovimientoCuentaContable(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal CodigoContable As String, Optional ByVal Debitos00 As Double = 0.0, Optional ByVal Creditos00 As Double = 0.0, _
        Optional ByVal Debitos01 As Double = 0.0, Optional ByVal Creditos01 As Double = 0.0, Optional ByVal Debitos02 As Double = 0.0, Optional ByVal Creditos02 As Double = 0.0, _
        Optional ByVal Debitos03 As Double = 0.0, Optional ByVal Creditos03 As Double = 0.0, Optional ByVal Debitos04 As Double = 0.0, Optional ByVal Creditos04 As Double = 0.0, _
        Optional ByVal Debitos05 As Double = 0.0, Optional ByVal Creditos05 As Double = 0.0, Optional ByVal Debitos06 As Double = 0.0, Optional ByVal Creditos06 As Double = 0.0, _
        Optional ByVal Debitos07 As Double = 0.0, Optional ByVal Creditos07 As Double = 0.0, Optional ByVal Debitos08 As Double = 0.0, Optional ByVal Creditos08 As Double = 0.0, _
        Optional ByVal Debitos09 As Double = 0.0, Optional ByVal Creditos09 As Double = 0.0, Optional ByVal Debitos10 As Double = 0.0, Optional ByVal Creditos10 As Double = 0.0, _
        Optional ByVal Debitos11 As Double = 0.0, Optional ByVal Creditos11 As Double = 0.0, Optional ByVal Debitos12 As Double = 0.0, Optional ByVal Creditos12 As Double = 0.0)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jscotdaacon SET "
        Else
            strSQLInicio = " update jscotdaacon set "
            strSQLFin = " WHERE " _
                & " codcon = '" & CodigoContable & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarDoble(Debitos00, "deb00")
        strSQL += ModificarDoble(Creditos00, "cre00")
        strSQL += ModificarDoble(Debitos01, "deb01")
        strSQL += ModificarDoble(Creditos01, "cre01")
        strSQL += ModificarDoble(Debitos02, "deb02")
        strSQL += ModificarDoble(Creditos02, "cre02")
        strSQL += ModificarDoble(Debitos03, "deb03")
        strSQL += ModificarDoble(Creditos03, "cre03")
        strSQL += ModificarDoble(Debitos04, "deb04")
        strSQL += ModificarDoble(Creditos04, "cre04")
        strSQL += ModificarDoble(Debitos05, "deb05")
        strSQL += ModificarDoble(Creditos05, "cre05")
        strSQL += ModificarDoble(Debitos06, "deb06")
        strSQL += ModificarDoble(Creditos06, "cre06")
        strSQL += ModificarDoble(Debitos07, "deb07")
        strSQL += ModificarDoble(Creditos07, "cre07")
        strSQL += ModificarDoble(Debitos08, "deb08")
        strSQL += ModificarDoble(Creditos08, "cre08")
        strSQL += ModificarDoble(Debitos09, "deb09")
        strSQL += ModificarDoble(Creditos09, "cre09")
        strSQL += ModificarDoble(Debitos10, "deb10")
        strSQL += ModificarDoble(Creditos10, "cre10")
        strSQL += ModificarDoble(Debitos11, "deb11")
        strSQL += ModificarDoble(Creditos11, "cre11")
        strSQL += ModificarDoble(Debitos12, "deb12")
        strSQL += ModificarDoble(Creditos12, "cre12")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditCONTABEncabezadoAsiento(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal CodigoAsiento As String, ByVal CodigoAsientoAnterior As String, ByVal FechaAsiento As Date, ByVal Descripcion As String, _
        ByVal Debitos As Double, ByVal Creditos As Double, ByVal Actual As Integer, PlantillaOrigen As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jscotencasi SET "
        Else
            strSQLInicio = " UPDATE jscotencasi set "
            strSQLFin = " WHERE " _
                & " asiento = '" & CodigoAsientoAnterior & "' and " _
                & " actual = " & Actual & " and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoAsiento, "asiento")
        strSQL += ModificarFecha(FechaAsiento, "fechasi")
        strSQL += ModificarCadena(Descripcion, "descripcion")
        strSQL += ModificarDoble(Debitos, "debitos")
        strSQL += ModificarDoble(Creditos, "creditos")
        strSQL += ModificarEntero(Actual, "actual")
        strSQL += ModificarCadena(PlantillaOrigen, "PLANTILLA_ORIGEN")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTABRenglonAsiento(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal CodigoAsiento As String, ByVal Renglon As String, ByVal CodigoContable As String, ByVal Referencia As String, _
        ByVal Concepto As String, ByVal Importe As Double, ByVal Libro As Integer, _
        ByVal Debito_Credito As Integer, ByVal Actual As Integer, ReglaOrigen As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jscotrenasi SET "
        Else
            strSQLInicio = " UPDATE jscotrenasi set "
            strSQLFin = " WHERE " _
                & " asiento = '" & CodigoAsiento & "' and " _
                & " renglon = '" & Renglon & "' and " _
                & " actual = " & Actual & " and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoAsiento, "asiento")
        strSQL += ModificarCadena(Renglon, "renglon")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarCadena(Referencia, "referencia")
        strSQL += ModificarCadena(Concepto, "concepto")
        strSQL += ModificarDoble(Importe, "importe")
        strSQL += ModificarEntero(Libro, "libro")
        strSQL += ModificarEntero(Debito_Credito, "debito_credito")
        strSQL += ModificarEntero(Actual, "actual")
        strSQL += ModificarCadena(ReglaOrigen, "regla_origen")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditCONTABEncabezadoAsientoPlantilla(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal CodigoAsiento As String, ByVal DescripcionAsiento As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jscotencdef set "
        Else
            strSQLInicio = " update jscotencdef set "
            strSQLFin = " where " _
                & " asiento = '" & CodigoAsiento & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoAsiento, "asiento")
        strSQL += ModificarCadena(DescripcionAsiento, "descripcion")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditCONTABRenglonAsientoPlantilla(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal CodigoAsiento As String, ByVal Renglon As String, ByVal CodigoContable As String, _
        ByVal Referencia As String, ByVal Concepto As String, ByVal Regla As String, ByVal Signo As String, _
        ByVal Aceptado As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jscotrendef set "
        Else
            strSQLInicio = " update jscotrendef set "
            strSQLFin = " where " _
            & " asiento = '" & CodigoAsiento & "' and " _
            & " renglon = '" & Renglon & "' and " _
            & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoAsiento, "asiento")
        strSQL += ModificarCadena(Renglon, "renglon")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarCadena(Referencia, "referencia")
        strSQL += ModificarCadena(Concepto, "concepto")
        strSQL += ModificarCadena(Regla, "regla")
        strSQL += ModificarCadena(Signo, "signo")
        strSQL += ModificarCadena(Aceptado, "aceptado")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditCONTABRegla(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
        ByVal CodigoRegla As String, ByVal Referencia As String, ByVal Comentario As String, _
        ByVal Conjunto As String, ByVal Condicion As String, ByVal Formula As String, _
        ByVal Agrupadopor As String, ByVal CodigoContable As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jscotcatreg set "
        Else
            strSQLInicio = " update jscotcatreg set "
            strSQLFin = " where " _
            & " plantilla = '" & CodigoRegla & "' and " _
            & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoRegla, "plantilla")
        strSQL += ModificarCadena(Referencia, "referencia")
        strSQL += ModificarCadena(Comentario, "comen")
        strSQL += ModificarCadena(Conjunto, "conjunto")
        strSQL += ModificarCadena(Condicion, "condicion")
        strSQL += ModificarCadena(Formula, "formula")
        strSQL += ModificarCadena(Agrupadopor, "agrupadopor")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

End Module
