Imports MySql.Data.MySqlClient
Module TablasBancos
    Private ft As New Transportables
    Public Sub InsertEditBANCOSBanco(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal CodigoBanco As String, ByVal NombreBanco As String,
       ByVal CtaBancaria As String, ByVal Agencia As String, ByVal Direccion As String, ByVal Telef1 As String, ByVal Telef2 As String,
       ByVal Fax As String, ByVal email As String, ByVal web As String, ByVal Contacto As String, ByVal Titulo As String,
       ByVal Comision As Double, ByVal Ingreso As Date, ByVal SaldoActual As Double, ByVal CodigoContable As String,
       ByVal Formato As String, ByVal Estatus As Integer, Currency As Integer, CurrencyDate As DateTime)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsbancatban SET "
            strSQL += ModificarFechaTiempoPlus(CurrencyDate, "currency_date")
            strSQL += ModificarEntero(Currency, "currency")
        Else
            strSQLInicio = " UPDATE jsbancatban SET "
            strSQLFin = " WHERE " _
                & " codban = '" & CodigoBanco & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL = strSQL & ModificarCadena(CodigoBanco, "codban")
        strSQL = strSQL & ModificarCadena(NombreBanco, "nomban")
        strSQL = strSQL & ModificarCadena(CtaBancaria, "ctaban")
        strSQL = strSQL & ModificarCadena(Agencia, "agencia")
        strSQL = strSQL & ModificarCadena(Direccion, "direccion")
        strSQL = strSQL & ModificarCadena(Telef1, "telef1")
        strSQL = strSQL & ModificarCadena(Telef2, "telef2")
        strSQL = strSQL & ModificarCadena(Fax, "fax")
        strSQL = strSQL & ModificarCadena(email, "email")
        strSQL = strSQL & ModificarCadena(web, "website")
        strSQL = strSQL & ModificarCadena(Contacto, "contacto")
        strSQL = strSQL & ModificarCadena(Titulo, "titulo")
        strSQL = strSQL & ModificarDoble(Comision, "comision")
        strSQL = strSQL & ModificarFecha(Ingreso, "fechacrea")
        strSQL = strSQL & ModificarDoble(SaldoActual, "saldoact")
        strSQL = strSQL & ModificarCadena(CodigoContable, "codcon")
        strSQL = strSQL & ModificarCadena(Formato, "formato")
        strSQL = strSQL & ModificarEntero(Estatus, "estatus")

        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")
        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditBANCOSTarjetaBanco(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal CodigoBanco As String, ByVal CodigoTarjeta As String, _
       ByVal Comision As Double, ByVal ISLR As Double)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsbancatbantar SET "
        Else
            strSQLInicio = " UPDATE jsbancatbantar SET "
            strSQLFin = " WHERE " _
                & " codban = '" & CodigoBanco & "' and " _
                & " codtar = '" & CodigoTarjeta & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL = strSQL & ModificarCadena(CodigoBanco, "codban")
        strSQL = strSQL & ModificarCadena(CodigoTarjeta, "codtar")
        strSQL = strSQL & ModificarCadena(Comision, "com1")
        strSQL = strSQL & ModificarCadena(ISLR, "com2")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditBANCOSMovimientoBanco(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal FechaMovimiento As Date,
        ByVal NumeroDocumento As String, ByVal TipoMovimiento As String, ByVal CodigoBanco As String,
        ByVal Caja As String, ByVal Concepto As String, ByVal Importe As Double, ByVal Origen As String, ByVal NumeroOrigen As String,
        ByVal Beneficiario As String, ByVal Comprobante As String, ByVal Conciliado As String, ByVal MesConciliacion As Date,
        ByVal FechaConciliación As Date, ByVal TipoOrigen As String, ByVal Asiento As String, ByVal FechaAsiento As Date, ByVal MultiCancelacion As String,
        ByVal ProveedorCliente As String, ByVal Vendedor As String,
        ByVal Currency As Integer, ByVal CurrencyDate As DateTime)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsbantraban SET "
            strSQL += ModificarFechaTiempoPlus(CurrencyDate, "currency_date")
            strSQL += ModificarEntero(Currency, "currency")
        Else
            strSQLInicio = " UPDATE jsbantraban SET "
            strSQLFin = " WHERE " _
                & " fechamov = '" & ft.FormatoFechaMySQL(FechaMovimiento) & "' AND " _
                & " numdoc = '" & NumeroDocumento & "' AND " _
                & " tipomov = '" & TipoMovimiento & "' AND " _
                & " codban = '" & CodigoBanco & "' AND " _
                & " caja = '" & Caja & "' AND " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL = strSQL & " caja = '" & Caja & "', "
        strSQL = strSQL & " ejercicio = '" & jytsistema.WorkExercise & "', "
        strSQL = strSQL & ModificarFecha(FechaMovimiento, "fechamov")
        strSQL = strSQL & ModificarCadena(NumeroDocumento, "numdoc")
        strSQL = strSQL & ModificarCadena(TipoMovimiento, "tipomov")
        strSQL = strSQL & ModificarCadena(CodigoBanco, "codban")
        strSQL = strSQL & ModificarCadena(Concepto, "concepto")
        strSQL = strSQL & ModificarDoble(Importe, "importe")
        strSQL = strSQL & ModificarCadena(Origen, "origen")
        strSQL = strSQL & ModificarCadena(NumeroOrigen, "numorg")
        strSQL = strSQL & ModificarCadena(Beneficiario, "benefic")
        strSQL = strSQL & ModificarCadena(Comprobante, "comproba")
        strSQL = strSQL & ModificarCadena(Conciliado, "conciliado")
        strSQL = strSQL & ModificarFecha(MesConciliacion, "mesconcilia")
        strSQL = strSQL & ModificarFecha(FechaConciliación, "fecconcilia")
        strSQL = strSQL & ModificarCadena(TipoOrigen, "tiporg")
        strSQL = strSQL & ModificarCadena(Asiento, "asiento")
        strSQL = strSQL & ModificarFecha(FechaAsiento, "fechasi")
        strSQL = strSQL & ModificarCadena(MultiCancelacion, "multican")
        strSQL = strSQL & ModificarCadena(ProveedorCliente, "prov_cli")
        strSQL = strSQL & ModificarCadena(Vendedor, "codven")

        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))
        ft.Ejecutar_strSQL(MyConn, " update jsbancatban set saldoact = " & CalculaSaldoBanco(MyConn, lblInfo, CodigoBanco) & " where codban = '" & CodigoBanco & "' and id_emp = '" & jytsistema.WorkID & "' ")


    End Sub

    Public Sub InsertEditBANCOSMovimientoConciliacion(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
            ByVal FechaMovimiento As Date, ByVal NumeroDocumento As String, ByVal TipoMovimiento As String, ByVal CodigoBanco As String, _
            ByVal Concepto As String, ByVal Importe As Double, ByVal Conciliado As String, ByVal Origen As String, _
            ByVal MesConciliacion As Date, ByVal FechaConciliación As Date)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsbantracon SET "
        Else
            strSQLInicio = " UPDATE jsbantracon SET "
            strSQLFin = " WHERE " _
                & " fechamov = '" & ft.FormatoFechaMySQL(FechaMovimiento) & "' AND " _
                & " numdoc = '" & NumeroDocumento & "' AND " _
                & " tipban = '" & TipoMovimiento & "' AND " _
                & " codban = '" & CodigoBanco & "' AND " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL = strSQL & ModificarFecha(FechaMovimiento, "fechamov")
        strSQL = strSQL & ModificarCadena(NumeroDocumento, "numdoc")
        strSQL = strSQL & ModificarCadena(TipoMovimiento, "tipban")
        strSQL = strSQL & ModificarCadena(CodigoBanco, "codban")
        strSQL = strSQL & ModificarCadena(Concepto, "concepto")
        strSQL = strSQL & ModificarDoble(Importe, "importe")
        strSQL = strSQL & ModificarCadena(Origen, "origen")
        strSQL = strSQL & ModificarCadena(Conciliado, "conciliado")
        strSQL = strSQL & ModificarFecha(MesConciliacion, "mesconcilia")
        strSQL = strSQL & ModificarFecha(FechaConciliación, "fecconcilia")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditBANCOSEncabezadoCaja(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal CodigoCaja As String, ByVal NombreCaja As String,
                                              CodigoContable As String, ByVal SaldoCaja As Double, Currency As Integer, CurrencyDate As DateTime)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsbanenccaj SET "
            strSQL += ModificarFechaTiempoPlus(CurrencyDate, "currency_date")
            strSQL += ModificarEntero(Currency, "currency")
        Else
            strSQLInicio = " UPDATE jsbanenccaj SET "
            strSQLFin = " WHERE " _
                & " caja = '" & CodigoCaja & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL = strSQL & " ejercicio = '" & jytsistema.WorkExercise & "', "
        strSQL = strSQL & ModificarCadena(CodigoCaja, "caja")
        strSQL = strSQL & ModificarCadena(NombreCaja, "nomcaja")
        strSQL = strSQL & ModificarCadena(CodigoContable, "codcon")
        strSQL = strSQL & ModificarDoble(SaldoCaja, "saldo")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))
        ft.Ejecutar_strSQL(MyConn, " update jsbanenccaj set saldo = " & CalculaSaldoCajaPorFP(MyConn, CodigoCaja, "", lblInfo) & " where caja = '" & CodigoCaja & "' and id_emp = '" & jytsistema.WorkID & "' ")

    End Sub
    Public Sub InsertEditBANCOSRenglonCaja(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal CodigoCaja As String, ByVal Renglon As String, ByVal Fecha As Date,
        ByVal Origen As String, ByVal TipoMovimiento As String, ByVal NumeroMovimiento As String,
        ByVal Formapago As String, ByVal NumeroPago As String, ByVal ReferenciaPago As String, ByVal Importe As Double,
        ByVal CodigoContable As String, ByVal Concepto As String, ByVal Deposito As String, ByVal FechaDeposito As Date,
        ByVal CantidadDocumentos As Integer, ByVal CodigoBanco As String, ByVal Multicancelacion As String, ByVal Asiento As String,
        ByVal FechaAsiento As Date, ByVal ProveedorCliente As String, ByVal Vendedor As String,
        ByVal Aceptado As String, ByVal Currency As Integer, ByVal CurrencyDate As DateTime)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsbantracaj SET "
            Renglon = UltimoCajaMasUno(Myconn, lblInfo, CodigoCaja)
            strSQL += ModificarFechaTiempoPlus(CurrencyDate, "currency_date")
            strSQL += ModificarEntero(Currency, "currency")
        Else
            strSQLInicio = " update jsbantracaj SET "
            strSQLFin = " where " _
                & " caja = '" & CodigoCaja & "' and " _
                & " renglon = '" & Renglon & "' and " _
                & " fecha = '" & ft.FormatoFechaMySQL(Fecha) & "' and " _
                & " origen = '" & Origen & "' and " _
                & " tipomov = '" & TipoMovimiento & "' and " _
                & " nummov = '" & NumeroMovimiento & "' and " _
                & " aceptado = '" & Aceptado & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL = strSQL & " aceptado = '" & Aceptado & "', "
        strSQL = strSQL & " ejercicio = '" & jytsistema.WorkExercise & "', "
        strSQL = strSQL & " deposito = '" & Deposito & "', "
        strSQL = strSQL & ModificarCadena(CodigoCaja, "caja")
        strSQL = strSQL & ModificarCadena(Renglon, "renglon")
        strSQL = strSQL & ModificarFecha(Fecha, "fecha")
        strSQL = strSQL & ModificarCadena(Origen, "origen")
        strSQL = strSQL & ModificarCadena(TipoMovimiento, "tipomov")
        strSQL = strSQL & ModificarCadena(NumeroMovimiento, "nummov")
        strSQL = strSQL & ModificarCadena(Formapago, "formpag")
        strSQL = strSQL & ModificarCadena(NumeroPago, "numpag")
        strSQL = strSQL & ModificarCadena(ReferenciaPago, "refpag")
        strSQL = strSQL & ModificarDoble(Importe, "importe")
        strSQL = strSQL & ModificarCadena(CodigoContable, "codcon")
        strSQL = strSQL & ModificarCadena(Concepto, "concepto")
        strSQL = strSQL & ModificarFecha(FechaDeposito, "fecha_dep")
        strSQL = strSQL & ModificarEnteroLargo(CantidadDocumentos, "cantidad")
        strSQL = strSQL & ModificarCadena(CodigoBanco, "codban")
        strSQL = strSQL & ModificarCadena(Multicancelacion, "multican")
        strSQL = strSQL & ModificarCadena(Asiento, "asiento")
        strSQL = strSQL & ModificarFecha(FechaAsiento, "fechasi")
        strSQL = strSQL & ModificarCadena(ProveedorCliente, "prov_cli")
        strSQL = strSQL & ModificarCadena(Vendedor, "codven")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(Myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

        ft.Ejecutar_strSQL(Myconn, " update jsbanenccaj set saldo = " & CalculaSaldoCajaPorFP(Myconn, CodigoCaja, "", lblInfo) & " where caja = '" & CodigoCaja & "' and id_emp = '" & jytsistema.WorkID & "' ")


    End Sub
    Function UltimoCajaMasUno(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal sCaja As String) As String
        Return Contador(MyConn, lblInfo, Gestion.iBancos, "BANNUMTRACAJ", "05")
    End Function
    Public Sub InsertEditBANCOSPlantillaCheque(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal Codigo As String, _
                                                ByVal Descripcion As String, ByVal MontoTop As Integer, ByVal Montoleft As Integer, _
                                                ByVal NombreTop As Integer, ByVal Nombreleft As Integer, ByVal MontoLetrasTop As Integer, _
                                                ByVal MontoLetrasleft As Integer, ByVal FechaTop As Integer, ByVal Fechaleft As Integer, _
                                                ByVal NoEndosableTop As Integer, ByVal noEndosableLeft As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String
        Dim strSQLFin As String = " "

        If Insertar Then
            strSQLInicio = " insert into jsbancatfor SET "
        Else
            strSQLInicio = " update jsbancatfor SET "
            strSQLFin = " where " _
                & " formato = '" & Codigo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If
        strSQL = strSQL & ModificarCadena(Codigo, "formato")
        strSQL = strSQL & ModificarCadena(Descripcion, "descrip")
        strSQL = strSQL & ModificarEnteroLargo(MontoTop, "montotop")
        strSQL = strSQL & ModificarEnteroLargo(Montoleft, "montoleft")
        strSQL = strSQL & ModificarEnteroLargo(NombreTop, "nombretop")
        strSQL = strSQL & ModificarEnteroLargo(Nombreleft, "nombreleft")
        strSQL = strSQL & ModificarEnteroLargo(MontoLetrasTop, "montoletratop")
        strSQL = strSQL & ModificarEnteroLargo(MontoLetrasleft, "montoletraleft")
        strSQL = strSQL & ModificarEnteroLargo(FechaTop, "fechatop")
        strSQL = strSQL & ModificarEnteroLargo(Fechaleft, "fechaleft")
        strSQL = strSQL & ModificarEnteroLargo(NoEndosableTop, "noendosabletop")
        strSQL = strSQL & ModificarEnteroLargo(noEndosableLeft, "noendosableleft")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(Myconn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))


    End Sub

    Public Sub InsertEditBANCOSRenglonORDENPAGO(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, _
       ByVal NumeroComprobante As String, ByVal Renglon As String, ByVal CodigoContable As String, ByVal Referencia As String, _
       ByVal Concepto As String, ByVal Importe As Double, ByVal Libro As Integer, _
       ByVal Debito_Credito As Integer)

        'LIBRO 0 = BANCOS; 1 = CXC

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsbanordpag SET "
        Else
            strSQLInicio = " UPDATE jsbanordpag set "
            strSQLFin = " WHERE " _
                & " comproba = '" & NumeroComprobante & "' and " _
                & " renglon = '" & Renglon & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroComprobante, "comproba")
        strSQL += ModificarCadena(Renglon, "renglon")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarCadena(Referencia, "refer")
        strSQL += ModificarCadena(Concepto, "concepto")
        strSQL += ModificarDoble(Importe, "importe")

        strSQL += ModificarEntero(Debito_Credito, "debito_credito")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub


End Module
