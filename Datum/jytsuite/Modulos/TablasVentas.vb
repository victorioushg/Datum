Imports MySql.Data.MySqlClient
Imports fTransport
Public Module TablasVentas


    Public ft As New Transportables

    Public Sub InsertEditVENTASCXC(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean, ByVal CodigoCliente As String,
    ByVal TipoMovimiento As String, ByVal NumeroMovimiento As String, ByVal FechaEmision As Date, ByVal Hora As String,
    ByVal FechaVencimiento As Date, ByVal Referencia As String, ByVal Concepto As String, ByVal Importe As Double,
    ByVal ImporteIVA As Double, ByVal FormaPago As String, ByVal CajaPago As String, ByVal NumeroPago As String,
    ByVal NombrePago As String, ByVal Beneficiario As String, ByVal Origen As String, ByVal NumeroOrigen As String,
    ByVal MultiCancelacion As String, ByVal Asiento As String, ByVal FechaAsiento As Date, ByVal CodigoContable As String,
    ByVal Multidocumento As String, ByVal TipoDocumentoCancelado As String, ByVal Interes As Double,
    ByVal Capital As Double, ByVal Comprobante As String, ByVal Banco As String, ByVal CuentaBancaria As String,
    ByVal Remesa As String, ByVal CodigoVendedor As String, ByVal CodigoCobrador As String, ByVal Historico As String,
    ByVal DebitoCredito As String, ByVal Division As String,
                                   ByVal Currency As Integer, ByVal CurrencyDate As DateTime)

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        Dim aF() As String = {"codcli", "id_emp"}
        Dim aFN() As String = {CodigoCliente, jytsistema.WorkID}

        If CodigoVendedor = "" Then CodigoVendedor = qFoundAndSign(MyConn, lblInfo, "jsvencatcli", aF, aFN, "vendedor")
        If CodigoCobrador = "" Then CodigoCobrador = qFoundAndSign(MyConn, lblInfo, "jsvencatcli", aF, aFN, "cobrador")

        If Insertar Then

            strSQLInicio = " insert into jsventracob SET "
            strSQL = ""
            strSQLFin = " "
            strSQL += ModificarFechaTiempoPlus(CurrencyDate, "currency_date")
            strSQL += ModificarEntero(Currency, "currency")

        Else
            Dim strHORA As String = ""
            If Hora <> "" Then strHORA = " HORA = '" & Hora & "' AND "

            strSQLInicio = " UPDATE jsventracob SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " codcli = '" & CodigoCliente & "' AND " _
                & " tipomov = '" & TipoMovimiento & "' AND " _
                & " nummov = '" & NumeroMovimiento & "' AND " _
                & " emision = '" & ft.FormatoFechaMySQL(FechaEmision) & "' AND " _
                & strHORA _
                & " ejercicio = '" & jytsistema.WorkExercise & "' AND " _
                & " division = '" & Division & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "


        End If

        strSQL = strSQL & ModificarCadena(CodigoCliente, "codcli")
        strSQL = strSQL & ModificarCadena(TipoMovimiento, "tipomov")
        strSQL = strSQL & ModificarCadena(NumeroMovimiento, "nummov")
        strSQL = strSQL & ModificarFecha(FechaEmision, "emision")
        strSQL = strSQL & ModificarCadena(Hora, "hora")
        strSQL = strSQL & ModificarFecha(FechaVencimiento, "vence")
        strSQL = strSQL & ModificarCadena(Referencia, "refer")
        strSQL = strSQL & ModificarCadena(Concepto, "concepto")
        strSQL = strSQL & ModificarDoble(Importe, "importe")
        strSQL = strSQL & ModificarDoble(ImporteIVA, "impiva")
        strSQL = strSQL & ModificarCadena(FormaPago, "formapag")
        strSQL = strSQL & ModificarCadena(CajaPago, "cajapag")
        strSQL = strSQL & ModificarCadena(NumeroPago, "numpag")
        strSQL = strSQL & ModificarCadena(NombrePago, "nompag")
        strSQL = strSQL & ModificarCadena(Beneficiario, "benefic")
        strSQL = strSQL & ModificarCadena(Origen, "origen")
        strSQL = strSQL & ModificarCadena(NumeroOrigen, "numorg")
        strSQL = strSQL & ModificarCadena(MultiCancelacion, "multican")
        strSQL = strSQL & ModificarCadena(Asiento, "asiento")
        strSQL = strSQL & ModificarFecha(FechaAsiento, "fechasi")
        strSQL = strSQL & ModificarCadena(CodigoContable, "codcon")
        strSQL = strSQL & ModificarCadena(Multidocumento, "multidoc")
        strSQL = strSQL & ModificarCadena(TipoDocumentoCancelado, "tipdoccan")
        strSQL = strSQL & ModificarDoble(Interes, "interes")
        strSQL = strSQL & ModificarDoble(Capital, "capital")
        strSQL = strSQL & ModificarCadena(Comprobante, "comproba")
        strSQL = strSQL & ModificarCadena(Banco, "banco")
        strSQL = strSQL & ModificarCadena(CuentaBancaria, "ctabanco")
        strSQL = strSQL & ModificarCadena(Remesa, "remesa")
        strSQL = strSQL & ModificarCadena(CodigoVendedor, "codven")
        strSQL = strSQL & ModificarCadena(CodigoCobrador, "codcob")
        strSQL = strSQL & ModificarCadena(Historico, "historico")
        strSQL = strSQL & ModificarCadena(DebitoCredito, "fotipo")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL = strSQL & ModificarCadena(Division, "division")
        strSQL = strSQL & ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditVENTASCancelacion(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
   ByVal CodigoCliente As String, ByVal TipoMovimiento As String, ByVal NumeroMovimiento As String,
   ByVal Emision As Date, ByVal Referencia As String, ByVal Concepto As String, ByVal Importe As Double,
   ByVal Comprobante As String, ByVal CodigoVendedor As String,
                                           ByVal Currency As Integer, ByVal CurrencyDate As DateTime)


        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsventracobcan SET "
            strSQL += ModificarFechaTiempoPlus(CurrencyDate, "currency_date")
            strSQL += ModificarEntero(Currency, "currency")
        Else
            strSQLInicio = " UPDATE jsventracobcan set "
            strSQLFin = " WHERE " _
                & " codpro = '" & CodigoCliente & "' and " _
                & " tipomov = '" & TipoMovimiento & "' and " _
                & " nummov = '" & NumeroMovimiento & "' and " _
                & " comproba = '" & Comprobante & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoCliente, "codcli")
        strSQL += ModificarCadena(TipoMovimiento, "tipomov")
        strSQL += ModificarCadena(NumeroMovimiento, "nummov")
        strSQL += ModificarFecha(Emision, "emision")
        strSQL += ModificarCadena(Referencia, "refer")
        strSQL += ModificarCadena(Concepto, "concepto")
        strSQL += ModificarDoble(Importe, "importe")
        strSQL += ModificarCadena(Comprobante, "comproba")
        strSQL += ModificarCadena(CodigoVendedor, "codven")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub


    Public Sub InsertEditVENTASVendedor(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
    ByVal CodigoVendedor As String, ByVal Cargo As String, ByVal Apellidos As String, ByVal Nombres As String,
    ByVal Direccion As String, ByVal Telefono As String, ByVal Celular As String, ByVal email As String,
    ByVal Fianza As Double, ByVal Tipo As String, ByVal Zona As String, ByVal Estructura As String,
    ByVal clave As String, ByVal Ingreso As Date, ByVal CarteraClientes As Integer, ByVal CarteraArticulos As Integer,
    ByVal CarteraMarcas As Integer, ByVal ListaA As Integer, ByVal ListaB As Integer, ByVal ListaC As Integer,
    ByVal ListaD As Integer, ByVal ListaE As Integer, ByVal ListaF As Integer, ByVal FactorCuota As Double,
    ByVal Estatus As Integer, ByVal Division As String, ByVal Supervisor As String, ByVal ComisionVenta As Double,
    ByVal ComisionCobranza As Double, Clase As Integer)

        ' Tipo : Asesor Fuerza Venta = 0 ; Cajero = 1 ;  Todos = 2 ;  Repuestos = 3 ; Servicios = 4
        '       Vehiculos = 5; Mecanicos = 6 ; VendedorPiso = 7 ; Mesero = 8

        Dim strSQL As String
        Dim strSQLInicio As String
        Dim strSQLFin As String

        If Insertar Then
            strSQLInicio = " insert into jsvencatven SET "
            strSQL = ""
            strSQLFin = " "

        Else
            strSQLInicio = " UPDATE jsvencatven SET "
            strSQL = ""
            strSQLFin = " WHERE " _
                & " codven = '" & CodigoVendedor & "' and " _
                & " tipo = '" & Tipo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoVendedor, "CODVEN")
        strSQL += ModificarCadena(Cargo, "DESCAR")
        strSQL += ModificarCadena(Apellidos, "APELLIDOS")
        strSQL += ModificarCadena(Nombres, "NOMBRES")
        strSQL += ModificarCadena(Direccion, "DIRECCION")
        strSQL += ModificarCadena(Telefono, "TELEFONO")
        strSQL += ModificarCadena(Celular, "CELULAR")
        strSQL += ModificarCadena(email, "EMAIL")
        strSQL += ModificarCadena(Tipo, "TIPO")
        strSQL += ModificarEntero(Clase, "CLASE")
        strSQL += ModificarDoble(Fianza, "FIANZA")
        strSQL += ModificarCadena(Zona, "ZONA")
        strSQL += ModificarCadena(Estructura, "ESTRUCTURA")
        strSQL += ModificarCadena(clave, "CLAVE")
        strSQL += ModificarFecha(Ingreso, "INGRESO")
        strSQL += ModificarEnteroLargo(CarteraClientes, "CARTERACLI")
        strSQL += ModificarEnteroLargo(CarteraArticulos, "CARTERAART")
        strSQL += ModificarEnteroLargo(CarteraMarcas, "CARTERAMAR")
        strSQL += ModificarEntero(ListaA, "LISTA_A")
        strSQL += ModificarEntero(ListaB, "LISTA_B")
        strSQL += ModificarEntero(ListaC, "LISTA_C")
        strSQL += ModificarEntero(ListaD, "LISTA_D")
        strSQL += ModificarEntero(ListaE, "LISTA_E")
        strSQL += ModificarEntero(ListaF, "LISTA_F")
        strSQL += ModificarDoble(FactorCuota, "factorcuota")
        strSQL += ModificarEntero(Estatus, "ESTATUS")
        strSQL += ModificarCadena(Division, "DIVISION")
        strSQL += ModificarCadena(Supervisor, "supervisor")
        strSQL += ModificarDoble(ComisionVenta, "COM_VEN")
        strSQL += ModificarDoble(ComisionCobranza, "COM_COB")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASEncabezadoGuiaDespacho(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                            ByVal CodigoGuia As String, ByVal Descripcion As String, ByVal Elaborador As String, ByVal Transporte As String,
                            ByVal FechaGuia As Date, ByVal FEchaDesde As Date, ByVal FechaHasta As Date, ByVal Items As Integer,
                            ByVal TotalGuia As Double, ByVal TotalKilos As Double, Estatus As Integer, Impresa As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenencgui SET "
        Else
            strSQLInicio = " UPDATE jsvenencgui SET "
            strSQLFin = " WHERE " _
                & " codigoguia = '" & CodigoGuia & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoGuia, "codigoguia")
        strSQL += ModificarCadena(Descripcion, "descripcion")
        strSQL += ModificarCadena(Elaborador, "elaborador")
        strSQL += ModificarCadena(Transporte, "transporte")
        strSQL += ModificarFecha(FechaGuia, "fechaguia")
        strSQL += ModificarFecha(FEchaDesde, "emisionfac")
        strSQL += ModificarFecha(FechaHasta, "hastafac")
        strSQL += ModificarEnteroLargo(Items, "items")
        strSQL += ModificarDoble(TotalGuia, "totalguia")
        strSQL += ModificarDoble(TotalKilos, "totalkilos")
        strSQL += ModificarEntero(Estatus, "estatus")
        strSQL += ModificarEntero(Impresa, "impresa")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASEncabezadoGuiaPedidos(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                            ByVal CodigoGuia As String, ByVal Descripcion As String, ByVal Elaborador As String, ByVal Transporte As String,
                            ByVal FechaGuia As Date, ByVal FEchaDesde As Date, ByVal FechaHasta As Date, ByVal Items As Integer,
                            ByVal TotalGuia As Double, ByVal TotalKilos As Double, ByVal Estatus As Integer, ByVal Impresa As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenencguipedidos SET "
        Else
            strSQLInicio = " UPDATE jsvenencguipedidos SET "
            strSQLFin = " WHERE " _
                & " codigoguia = '" & CodigoGuia & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoGuia, "codigoguia")
        strSQL += ModificarCadena(Descripcion, "descripcion")
        strSQL += ModificarCadena(Elaborador, "elaborador")
        strSQL += ModificarCadena(Transporte, "transporte")
        strSQL += ModificarFecha(FechaGuia, "fechaguia")
        strSQL += ModificarFecha(FEchaDesde, "emisionfac")
        strSQL += ModificarFecha(FechaHasta, "hastafac")
        strSQL += ModificarEnteroLargo(Items, "items")
        strSQL += ModificarDoble(TotalGuia, "totalguia")
        strSQL += ModificarDoble(TotalKilos, "totalkilos")
        strSQL += ModificarEntero(Estatus, "estatus")
        strSQL += ModificarEntero(Impresa, "impresa")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASEncabezadoRelacionDeFacturas(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                            ByVal CodigoGuia As String, ByVal Descripcion As String, ByVal Elaborador As String, ByVal Responsable As String,
                            ByVal FechaGuia As Date, ByVal FEchaDesde As Date, ByVal FechaHasta As Date, ByVal Items As Integer,
                            ByVal TotalGuia As Double, ByVal TotalKilos As Double, Estatus As Integer, Impresa As Integer, Tipo As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenencrel SET "
        Else
            strSQLInicio = " UPDATE jsvenencrel SET "
            strSQLFin = " WHERE " _
                & " codigoguia = '" & CodigoGuia & "' and " _
                & " Tipo = " & Tipo & " and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoGuia, "codigoguia")
        strSQL += ModificarCadena(Descripcion, "descripcion")
        strSQL += ModificarCadena(Elaborador, "elaborador")
        strSQL += ModificarCadena(Responsable, "responsable")
        strSQL += ModificarEntero(Tipo, "tipo")
        strSQL += ModificarFecha(FechaGuia, "fechaguia")
        strSQL += ModificarFecha(FEchaDesde, "emisionfac")
        strSQL += ModificarFecha(FechaHasta, "hastafac")
        strSQL += ModificarEnteroLargo(Items, "items")
        strSQL += ModificarDoble(TotalGuia, "totalguia")
        strSQL += ModificarDoble(TotalKilos, "totalkilos")
        strSQL += ModificarEntero(Estatus, "estatus")
        strSQL += ModificarEntero(Impresa, "impresa")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASRenglonGuiaDespacho(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                            ByVal CodigoGuia As String, ByVal CodigoFactura As String, ByVal Emision As Date, ByVal CodigoCliente As String,
                            ByVal NombreCliente As String, ByVal CodigoAsesor As String, ByVal KilosFactura As Double, TotalFactura As Double)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenrengui SET "
        Else
            strSQLInicio = " UPDATE jsvenrengui SET "
            strSQLFin = " WHERE " _
                & " codigoguia = '" & CodigoGuia & "' and " _
                & " codigofac = '" & CodigoFactura & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoGuia, "codigoguia")
        strSQL += ModificarCadena(CodigoFactura, "codigofac")
        strSQL += ModificarFecha(Emision, "emision")
        strSQL += ModificarCadena(CodigoCliente, "codcli")
        strSQL += ModificarCadena(NombreCliente, "nomcli")
        strSQL += ModificarCadena(CodigoAsesor, "codven")
        strSQL += ModificarDoble(KilosFactura, "kilosfac")
        strSQL += ModificarDoble(TotalFactura, "totalfac")
        strSQL += ModificarEntero(1, "Aceptado")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASRenglonGuiaPedidos(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                           ByVal CodigoGuia As String, ByVal CodigoFactura As String, ByVal Emision As Date, ByVal CodigoCliente As String,
                           ByVal NombreCliente As String, ByVal CodigoAsesor As String, ByVal KilosFactura As Double, ByVal TotalFactura As Double)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenrenguipedidos SET "
        Else
            strSQLInicio = " UPDATE jsvenrenguipedidos SET "
            strSQLFin = " WHERE " _
                & " codigoguia = '" & CodigoGuia & "' and " _
                & " codigofac = '" & CodigoFactura & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoGuia, "codigoguia")
        strSQL += ModificarCadena(CodigoFactura, "codigofac")
        strSQL += ModificarFecha(Emision, "emision")
        strSQL += ModificarCadena(CodigoCliente, "codcli")
        strSQL += ModificarCadena(NombreCliente, "nomcli")
        strSQL += ModificarCadena(CodigoAsesor, "codven")
        strSQL += ModificarDoble(KilosFactura, "kilosfac")
        strSQL += ModificarDoble(TotalFactura, "totalfac")
        strSQL += ModificarEntero(1, "Aceptado")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASRenglonRelacionDeFacturas(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                            ByVal CodigoGuia As String, ByVal CodigoFactura As String, ByVal Emision As Date, ByVal CodigoCliente As String,
                            ByVal NombreCliente As String, ByVal CodigoAsesor As String, ByVal KilosFactura As Double, TotalFactura As Double,
                            Tipo As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenrenrel SET "
        Else
            strSQLInicio = " UPDATE jsvenrenrel SET "
            strSQLFin = " WHERE " _
                & " codigoguia = '" & CodigoGuia & "' and " _
                & " codigofac = '" & CodigoFactura & "' and " _
                & " tipo = '" & Tipo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoGuia, "codigoguia")
        strSQL += ModificarCadena(CodigoFactura, "codigofac")
        strSQL += ModificarFecha(Emision, "emision")
        strSQL += ModificarCadena(CodigoCliente, "codcli")
        strSQL += ModificarCadena(NombreCliente, "nomcli")
        strSQL += ModificarCadena(CodigoAsesor, "codven")
        strSQL += ModificarDoble(KilosFactura, "kilosfac")
        strSQL += ModificarDoble(TotalFactura, "totalfac")
        strSQL += ModificarEntero(1, "Aceptado")
        strSQL += ModificarEntero(Tipo, "tipo")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASEncabezadoRuta(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                            ByVal CodigoRuta As String, ByVal NombreRuta As String, ByVal Comentario As String, ByVal CodigoZona As String,
                            ByVal CodigoVendedor As String, ByVal CodigoCobrador As String, ByVal Dia As Integer, ByVal Condicion As Integer,
                            ByVal CodigoTrabajador As String, ByVal Tipo As Char, ByVal Items As Integer, ByVal Division As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenencrut SET "
        Else
            strSQLInicio = " UPDATE jsvenencrut SET "
            strSQLFin = " WHERE " _
                & " codrut = '" & CodigoRuta & "' and " _
                & " Tipo = '" & Tipo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoRuta, "codrut")
        strSQL += ModificarCadena(NombreRuta, "nomrut")
        strSQL += ModificarCadena(Comentario, "comen")
        strSQL += ModificarCadena(CodigoZona, "codzon")
        strSQL += ModificarCadena(CodigoVendedor, "codven")
        strSQL += ModificarCadena(CodigoCobrador, "codcob")
        strSQL += ModificarEntero(Dia, "dia")
        strSQL += ModificarEntero(Condicion, "condicion")
        strSQL += ModificarCadena(CodigoTrabajador, "codtra")
        strSQL += ModificarCadena(Tipo, "tipo")
        strSQL += ModificarEnteroLargo(Items, "items")
        strSQL += ModificarCadena(Division, "division")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASEncabezadoPresupuesto(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                ByVal NumeroPresupuesto As String, ByVal EMISION As Date, ByVal VENCE As Date, ByVal CodigoCliente As String,
                                ByVal Comentario As String, ByVal CodigoVendedor As String, ByVal Tarifa As String, ByVal TotalNeto As Double,
                                ByVal Descuento As Double, ByVal Cargos As Double,
                                ByVal ImpuestoIVA As Double, ByVal TotalPresupuesto As Double,
                                ByVal Currency As Integer, ByVal CurrencyDate As DateTime,
                                ByVal ESTATUS As String, ByVal ITEMS As Integer,
                                ByVal Cajas As Double, ByVal KILOS As Double, ByVal IMPRESA As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenenccot SET "
            strSQL += ModificarFechaTiempoPlus(CurrencyDate, "currency_date")
            strSQL += ModificarEntero(Currency, "currency")
        Else
            strSQLInicio = " UPDATE jsvenenccot SET "
            strSQLFin = " WHERE " _
                & " numcot = '" & NumeroPresupuesto & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroPresupuesto, "numcot")
        strSQL += ModificarFecha(EMISION, "emision")
        strSQL += ModificarFecha(VENCE, "vence")
        strSQL += ModificarCadena(CodigoCliente, "codcli")
        strSQL += ModificarCadena(Comentario, "comen")
        strSQL += ModificarCadena(CodigoVendedor, "codven")
        strSQL += ModificarCadena(Tarifa, "tarifa")
        strSQL += ModificarDoble(TotalNeto, "tot_net")
        strSQL += ModificarDoble(Descuento, "descuen")
        strSQL += ModificarDoble(Cargos, "cargos")
        strSQL += ModificarDoble(ImpuestoIVA, "imp_iva")
        strSQL += ModificarDoble(TotalPresupuesto, "tot_cot")
        strSQL += ModificarCadena(ESTATUS, "estatus")
        strSQL += ModificarEnteroLargo(ITEMS, "items")
        strSQL += ModificarDoble(Cajas, "Cajas")
        strSQL += ModificarDoble(KILOS, "kilos")
        strSQL += ModificarEntero(IMPRESA, "impresa")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditVENTASEncabezadoPedido(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                               ByVal NumeroPedido As String, ByVal EMISION As Date, ByVal Entrega As Date, ByVal CodigoCliente As String,
                               ByVal Comentario As String, ByVal CodigoVendedor As String, ByVal Tarifa As String, ByVal TotalNeto As Double,
                               ByVal PorcentajeDescuento As Double, ByVal Descuento As Double, ByVal Cargos As Double,
                               ByVal ImpuestoIVA As Double, ByVal TotalPrePedido As Double,
                               ByVal Vencimiento As Date,
                               ByVal PorcentajeDescuento1 As Double, ByVal PorcentajeDescuento2 As Double, ByVal PorcentajeDescuento3 As Double, ByVal PorcentajeDescuento4 As Double,
                               ByVal Descuento1 As Double, ByVal Descuento2 As Double, ByVal Descuento3 As Double, ByVal Descuento4 As Double,
                               ByVal Vencimiento1 As Date, ByVal Vencimiento2 As Date, ByVal Vencimiento3 As Date, ByVal Vencimiento4 As Date,
                               ByVal ESTATUS As String, ByVal ITEMS As Integer, ByVal Cajas As Double, ByVal KILOS As Double,
                               ByVal CondicionDePago As Integer, ByVal TipoCredito As Integer,
                               ByVal FormaDePago As String, ByVal NumeroDePago As String, ByVal NombreDePago As String,
                               ByVal Abono As Double, ByVal Serie As String, ByVal NumGiros As Integer, ByVal PeriodoEntreGiros As Integer,
                               ByVal Interes As Double, ByVal PorcentajeInteres As Double, ByVal IMPRESA As Integer,
                                                ByVal Currency As Integer, ByVal CurrencyDate As DateTime)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenencped SET "
            strSQL += ModificarFechaTiempoPlus(CurrencyDate, "currency_date")
            strSQL += ModificarEntero(Currency, "currency")
        Else
            strSQLInicio = " UPDATE jsvenencped SET "
            strSQLFin = " WHERE " _
                & " numped = '" & NumeroPedido & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroPedido, "numped")
        strSQL += ModificarFecha(EMISION, "emision")
        strSQL += ModificarFecha(Entrega, "entrega")
        strSQL += ModificarCadena(CodigoCliente, "codcli")
        strSQL += ModificarCadena(Comentario, "comen")
        strSQL += ModificarCadena(CodigoVendedor, "codven")
        strSQL += ModificarCadena(Tarifa, "tarifa")
        strSQL += ModificarDoble(TotalNeto, "tot_net")
        strSQL += ModificarDoble(PorcentajeDescuento, "pordes")
        strSQL += ModificarDoble(Descuento, "descuen")
        strSQL += ModificarDoble(Cargos, "cargos")
        strSQL += ModificarDoble(ImpuestoIVA, "imp_iva")
        strSQL += ModificarDoble(TotalPrePedido, "tot_ped")
        strSQL += ModificarFecha(Vencimiento, "vence")
        strSQL += ModificarDoble(PorcentajeDescuento1, "pordes1")
        strSQL += ModificarDoble(PorcentajeDescuento2, "pordes2")
        strSQL += ModificarDoble(PorcentajeDescuento3, "pordes3")
        strSQL += ModificarDoble(PorcentajeDescuento4, "pordes4")
        strSQL += ModificarDoble(Descuento1, "descuen1")
        strSQL += ModificarDoble(Descuento2, "descuen2")
        strSQL += ModificarDoble(Descuento3, "descuen3")
        strSQL += ModificarDoble(Descuento4, "descuen4")
        strSQL += ModificarFecha(Vencimiento1, "vence1")
        strSQL += ModificarFecha(Vencimiento2, "vence2")
        strSQL += ModificarFecha(Vencimiento3, "vence3")
        strSQL += ModificarFecha(Vencimiento4, "vence4")
        strSQL += ModificarCadena(ESTATUS, "estatus")
        strSQL += ModificarEnteroLargo(ITEMS, "items")
        strSQL += ModificarDoble(Cajas, "Cajas")
        strSQL += ModificarDoble(KILOS, "kilos")
        strSQL += ModificarEntero(CondicionDePago, "condpag")
        strSQL += ModificarEntero(TipoCredito, "tipocredito")
        strSQL += ModificarCadena(FormaDePago, "formapag")
        strSQL += ModificarCadena(NumeroDePago, "numpag")
        strSQL += ModificarCadena(NombreDePago, "nompag")
        strSQL += ModificarDoble(Abono, "abono")
        strSQL += ModificarCadena(Serie, "serie")
        strSQL += ModificarEnteroLargo(NumGiros, "numgiro")
        strSQL += ModificarEnteroLargo(PeriodoEntreGiros, "pergiro")
        strSQL += ModificarDoble(Interes, "interes")
        strSQL += ModificarDoble(PorcentajeInteres, "porint")
        strSQL += ModificarEntero(IMPRESA, "impresa")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditVENTASEncabezadoPrePedido(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                               ByVal NumeroPrePedido As String, ByVal EMISION As Date, ByVal Entrega As Date, ByVal CodigoCliente As String,
                               ByVal Comentario As String, ByVal CodigoVendedor As String, ByVal Tarifa As String, ByVal TotalNeto As Double,
                               ByVal PorcentajeDescuento As Double, ByVal Descuento As Double, ByVal Cargos As Double,
                               ByVal ImpuestoIVA As Double, ByVal TotalPrePedido As Double,
                               ByVal Vencimiento As Date,
                               ByVal PorcentajeDescuento1 As Double, ByVal PorcentajeDescuento2 As Double, ByVal PorcentajeDescuento3 As Double, ByVal PorcentajeDescuento4 As Double,
                               ByVal Descuento1 As Double, ByVal Descuento2 As Double, ByVal Descuento3 As Double, ByVal Descuento4 As Double,
                               ByVal Vencimiento1 As Date, ByVal Vencimiento2 As Date, ByVal Vencimiento3 As Date, ByVal Vencimiento4 As Date,
                               ByVal ESTATUS As String, ByVal ITEMS As Integer, ByVal Cajas As Double, ByVal KILOS As Double,
                               ByVal CondicionDePago As Integer, ByVal TipoCredito As Integer,
                               ByVal FormaDePago As String, ByVal NumeroDePago As String, ByVal NombreDePago As String,
                               ByVal Abono As Double, ByVal Serie As String, ByVal NumGiros As Integer, ByVal PeriodoEntreGiros As Integer,
                               ByVal Interes As Double, ByVal PorcentajeInteres As Double, ByVal IMPRESA As Integer,
                                                   ByVal Currency As Integer, ByVal CurrencyDate As DateTime)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenencpedrgv SET "
            strSQL += ModificarFechaTiempoPlus(CurrencyDate, "currency_date")
            strSQL += ModificarEntero(Currency, "currency")
        Else
            strSQLInicio = " UPDATE jsvenencpedrgv SET "
            strSQLFin = " WHERE " _
                & " numped = '" & NumeroPrePedido & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroPrePedido, "numped")
        strSQL += ModificarFecha(EMISION, "emision")
        strSQL += ModificarFecha(Entrega, "entrega")
        strSQL += ModificarCadena(CodigoCliente, "codcli")
        strSQL += ModificarCadena(Comentario, "comen")
        strSQL += ModificarCadena(CodigoVendedor, "codven")
        strSQL += ModificarCadena(Tarifa, "tarifa")
        strSQL += ModificarDoble(TotalNeto, "tot_net")
        strSQL += ModificarDoble(PorcentajeDescuento, "pordes")
        strSQL += ModificarDoble(Descuento, "descuen")
        strSQL += ModificarDoble(Cargos, "cargos")
        strSQL += ModificarDoble(ImpuestoIVA, "imp_iva")
        strSQL += ModificarDoble(TotalPrePedido, "tot_ped")
        strSQL += ModificarFecha(Vencimiento, "vence")
        strSQL += ModificarDoble(PorcentajeDescuento1, "pordes1")
        strSQL += ModificarDoble(PorcentajeDescuento2, "pordes2")
        strSQL += ModificarDoble(PorcentajeDescuento3, "pordes3")
        strSQL += ModificarDoble(PorcentajeDescuento4, "pordes4")
        strSQL += ModificarDoble(Descuento1, "descuen1")
        strSQL += ModificarDoble(Descuento2, "descuen2")
        strSQL += ModificarDoble(Descuento3, "descuen3")
        strSQL += ModificarDoble(Descuento4, "descuen4")
        strSQL += ModificarFecha(Vencimiento1, "vence1")
        strSQL += ModificarFecha(Vencimiento2, "vence2")
        strSQL += ModificarFecha(Vencimiento3, "vence3")
        strSQL += ModificarFecha(Vencimiento4, "vence4")
        strSQL += ModificarCadena(ESTATUS, "estatus")
        strSQL += ModificarEnteroLargo(ITEMS, "items")
        strSQL += ModificarDoble(Cajas, "Cajas")
        strSQL += ModificarDoble(KILOS, "kilos")
        strSQL += ModificarEntero(CondicionDePago, "condpag")
        strSQL += ModificarEntero(TipoCredito, "tipocredito")
        strSQL += ModificarCadena(FormaDePago, "formapag")
        strSQL += ModificarCadena(NumeroDePago, "numpag")
        strSQL += ModificarCadena(NombreDePago, "nompag")
        strSQL += ModificarDoble(Abono, "abono")
        strSQL += ModificarCadena(Serie, "serie")
        strSQL += ModificarEnteroLargo(NumGiros, "numgiro")
        strSQL += ModificarEnteroLargo(PeriodoEntreGiros, "pergiro")
        strSQL += ModificarDoble(Interes, "interes")
        strSQL += ModificarDoble(PorcentajeInteres, "porint")
        strSQL += ModificarEntero(IMPRESA, "impresa")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditVENTASEncabezadoNotaEntrega(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                               ByVal NumeroNotaEntrega As String, ByVal EMISION As Date, ByVal CodigoCliente As String,
                               ByVal Comentario As String, ByVal CodigoVendedor As String, ByVal Almacen As String, ByVal Transporte As String,
                               ByVal Vencimiento As Date, ByVal Referencia As String, ByVal CodigoContable As String, ByVal Items As Integer, ByVal Cajas As Integer,
                               ByVal KILOS As Double, ByVal TotalNeto As Double,
                               ByVal PorcentajeDescuento As Double, ByVal PorcentajeDescuento1 As Double, ByVal PorcentajeDescuento2 As Double,
                               ByVal PorcentajeDescuento3 As Double, ByVal PorcentajeDescuento4 As Double, ByVal Descuento As Double,
                               ByVal Descuento1 As Double, ByVal Descuento2 As Double, ByVal Descuento3 As Double, ByVal Descuento4 As Double,
                               ByVal Cargos As Double, ByVal Cargos1 As Double, ByVal Cargos2 As Double, ByVal Cargos3 As Double, ByVal Cargos4 As Double,
                               ByVal TotalFactura1 As Double, ByVal TotalFactura2 As Double, ByVal TotalFactura3 As Double, ByVal TotalFactura4 As Double,
                               ByVal Vencimiento1 As Date, ByVal Vencimiento2 As Date, ByVal Vencimiento3 As Date, ByVal Vencimiento4 As Date,
                               ByVal CondicionDePago As Integer, ByVal TipoCredito As Integer,
                               ByVal FormaDePago As String, ByVal NumeroDePago As String, ByVal NombreDePago As String, ByVal Caja As String,
                               ByVal ImporteEfectivo As Double, ByVal NumeroCheque As String, ByVal BancoCheque As String, ByVal ImporteCheque As Double,
                               ByVal NumeroTarjeta As String, ByVal CodigoTarjeta As String, ByVal ImporteTarjeta As Double,
                               ByVal NumeroCestaTicket As String, ByVal NombreCestaTicket As String, ByVal ImporteCestaTicket As Double,
                               ByVal NumeroDeposito As String, ByVal BancoDeposito As String, ByVal ImporteDeposito As Double,
                               ByVal Abono As Double, ByVal Serie As String, ByVal NumGiros As Integer, ByVal PeriodoEntreGiros As Integer,
                               ByVal Interes As Double, ByVal PorcentajeInteres As Double, ByVal Asiento As String, ByVal FechaAsiento As Date,
                               ByVal BaseIVA As Double, ByVal PorcentajeIVA As Double, ByVal ImpuestoIVA As Double, ByVal ImpuestoICS As Double,
                               ByVal TipoFactura As Integer, ByVal Tipo As Integer, ByVal TotalFactura As Double, ByVal ESTATUS As String,
                               ByVal Tarifa As String, ByVal NumeroCXC As String, ByVal OtraCXC As String, ByVal OtroCliente As String,
                               ByVal RelacionGuia As String, ByVal RelacionFacturas As String, ByVal IMPRESA As Integer,
                                                     ByVal Currency As Integer, ByVal CurrencyDate As DateTime)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenencnot SET "
            strSQL += ModificarFechaTiempoPlus(CurrencyDate, "currency_date")
            strSQL += ModificarEntero(Currency, "currency")
        Else
            strSQLInicio = " UPDATE jsvenencnot SET "
            strSQLFin = " WHERE " _
               & " numfac = '" & NumeroNotaEntrega & "' and " _
               & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroNotaEntrega, "numfac")
        strSQL += ModificarFecha(EMISION, "emision")
        strSQL += ModificarCadena(CodigoCliente, "codcli")
        strSQL += ModificarCadena(Comentario, "comen")
        strSQL += ModificarCadena(CodigoVendedor, "codven")
        strSQL += ModificarCadena(Almacen, "almacen")
        strSQL += ModificarCadena(Transporte, "transporte")
        strSQL += ModificarFecha(Vencimiento, "vence")
        strSQL += ModificarCadena(Referencia, "refer")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarEnteroLargo(Items, "items")
        strSQL += ModificarDoble(Cajas, "Cajas")
        strSQL += ModificarDoble(KILOS, "kilos")
        strSQL += ModificarDoble(TotalNeto, "tot_net")
        strSQL += ModificarDoble(PorcentajeDescuento, "pordes")
        strSQL += ModificarDoble(PorcentajeDescuento1, "pordes1")
        strSQL += ModificarDoble(PorcentajeDescuento2, "pordes2")
        strSQL += ModificarDoble(PorcentajeDescuento3, "pordes3")
        strSQL += ModificarDoble(PorcentajeDescuento4, "pordes4")
        strSQL += ModificarDoble(Descuento, "descuen")
        strSQL += ModificarDoble(Descuento1, "descuen1")
        strSQL += ModificarDoble(Descuento2, "descuen2")
        strSQL += ModificarDoble(Descuento3, "descuen3")
        strSQL += ModificarDoble(Descuento4, "descuen4")
        strSQL += ModificarDoble(Cargos, "cargos")
        strSQL += ModificarDoble(Cargos1, "cargos1")
        strSQL += ModificarDoble(Cargos2, "cargos2")
        strSQL += ModificarDoble(Cargos3, "cargos3")
        strSQL += ModificarDoble(Cargos4, "cargos4")
        strSQL += ModificarDoble(TotalFactura1, "tot_fac1")
        strSQL += ModificarDoble(TotalFactura2, "tot_fac2")
        strSQL += ModificarDoble(TotalFactura3, "tot_fac3")
        strSQL += ModificarDoble(TotalFactura4, "tot_fac4")
        strSQL += ModificarFecha(Vencimiento1, "vence1")
        strSQL += ModificarFecha(Vencimiento2, "vence2")
        strSQL += ModificarFecha(Vencimiento3, "vence3")
        strSQL += ModificarFecha(Vencimiento4, "vence4")
        strSQL += ModificarEntero(CondicionDePago, "condpag")
        strSQL += ModificarEntero(TipoCredito, "tipocre")
        strSQL += ModificarCadena(FormaDePago, "formapag")
        strSQL += ModificarCadena(NumeroDePago, "numpag")
        strSQL += ModificarCadena(NombreDePago, "nompag")
        strSQL += ModificarCadena(Caja, "caja")
        strSQL += ModificarDoble(ImporteEfectivo, "importeefectivo")
        strSQL += ModificarCadena(NumeroCheque, "numerocheque")
        strSQL += ModificarCadena(BancoCheque, "bancocheque")
        strSQL += ModificarDoble(ImporteCheque, "importecheque")
        strSQL += ModificarCadena(NumeroTarjeta, "numerotarjeta")
        strSQL += ModificarCadena(CodigoTarjeta, "codigotarjeta")
        strSQL += ModificarDoble(ImporteTarjeta, "importetarjeta")
        strSQL += ModificarCadena(NumeroCestaTicket, "numerocestaticket")
        strSQL += ModificarCadena(NombreCestaTicket, "nombrecestaticket")
        strSQL += ModificarDoble(ImporteCestaTicket, "importecestaticket")
        strSQL += ModificarCadena(NumeroDeposito, "numerodeposito")
        strSQL += ModificarCadena(BancoDeposito, "bancodeposito")
        strSQL += ModificarDoble(ImporteDeposito, "importedeposito")
        strSQL += ModificarDoble(Abono, "abono")
        strSQL += ModificarCadena(Serie, "serie")
        strSQL += ModificarEnteroLargo(NumGiros, "numgiro")
        strSQL += ModificarEnteroLargo(PeriodoEntreGiros, "pergiro")
        strSQL += ModificarDoble(Interes, "interes")
        strSQL += ModificarDoble(PorcentajeInteres, "porint")
        strSQL += ModificarCadena(Asiento, "asiento")
        strSQL += ModificarFecha(FechaAsiento, "fechasi")
        strSQL += ModificarDoble(BaseIVA, "baseiva")
        strSQL += ModificarDoble(PorcentajeIVA, "poriva")
        strSQL += ModificarDoble(ImpuestoIVA, "imp_iva")
        strSQL += ModificarDoble(ImpuestoICS, "imp_ics")
        strSQL += ModificarEntero(TipoFactura, "tipofac")
        strSQL += ModificarEntero(Tipo, "tipo")
        strSQL += ModificarDoble(TotalFactura, "tot_fac")
        strSQL += ModificarCadena(ESTATUS, "estatus")
        strSQL += ModificarCadena(Tarifa, "tarifa")
        strSQL += ModificarCadena(NumeroCXC, "numcxc")
        strSQL += ModificarCadena(OtraCXC, "otra_cxc")
        strSQL += ModificarCadena(OtroCliente, "otro_cli")
        strSQL += ModificarCadena(RelacionGuia, "relguia")
        strSQL += ModificarCadena(RelacionFacturas, "relfacturas")
        strSQL += ModificarEntero(IMPRESA, "impresa")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASEncabezadoFactura(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                               ByVal NumeroFactura As String, ByVal EMISION As Date, ByVal CodigoCliente As String,
                               ByVal Comentario As String, ByVal CodigoVendedor As String, ByVal Almacen As String, ByVal Transporte As String,
                               ByVal Vencimiento As Date, ByVal Referencia As String, ByVal CodigoContable As String, ByVal Items As Integer, ByVal Cajas As Integer,
                               ByVal KILOS As Double, ByVal TotalNeto As Double,
                               ByVal PorcentajeDescuento As Double, ByVal PorcentajeDescuento1 As Double, ByVal PorcentajeDescuento2 As Double,
                               ByVal PorcentajeDescuento3 As Double, ByVal PorcentajeDescuento4 As Double, ByVal Descuento As Double,
                               ByVal Descuento1 As Double, ByVal Descuento2 As Double, ByVal Descuento3 As Double, ByVal Descuento4 As Double,
                               ByVal Cargos As Double, ByVal Cargos1 As Double, ByVal Cargos2 As Double, ByVal Cargos3 As Double, ByVal Cargos4 As Double,
                               ByVal TotalFactura1 As Double, ByVal TotalFactura2 As Double, ByVal TotalFactura3 As Double, ByVal TotalFactura4 As Double,
                               ByVal Vencimiento1 As Date, ByVal Vencimiento2 As Date, ByVal Vencimiento3 As Date, ByVal Vencimiento4 As Date,
                               ByVal CondicionDePago As Integer, ByVal TipoCredito As Integer,
                               ByVal FormaDePago As String, ByVal NumeroDePago As String, ByVal NombreDePago As String, ByVal Caja As String,
                               ByVal ImporteEfectivo As Double, ByVal NumeroCheque As String, ByVal BancoCheque As String, ByVal ImporteCheque As Double,
                               ByVal NumeroTarjeta As String, ByVal CodigoTarjeta As String, ByVal ImporteTarjeta As Double,
                               ByVal NumeroCestaTicket As String, ByVal NombreCestaTicket As String, ByVal ImporteCestaTicket As Double,
                               ByVal NumeroDeposito As String, ByVal BancoDeposito As String, ByVal ImporteDeposito As Double,
                               ByVal Abono As Double, ByVal Serie As String, ByVal NumGiros As Integer, ByVal PeriodoEntreGiros As Integer,
                               ByVal Interes As Double, ByVal PorcentajeInteres As Double, ByVal Asiento As String, ByVal FechaAsiento As Date,
                               ByVal BaseIVA As Double, ByVal PorcentajeIVA As Double, ByVal ImpuestoIVA As Double, ByVal ImpuestoICS As Double,
                               ByVal TipoFactura As Integer, ByVal Tipo As Integer, ByVal TotalFactura As Double, ByVal ESTATUS As String,
                               ByVal Tarifa As String, ByVal NumeroCXC As String, ByVal OtraCXC As String, ByVal OtroCliente As String,
                               ByVal RelacionGuia As String, ByVal RelacionFacturas As String, ByVal IMPRESA As Integer,
                                                 ByVal Currency As Integer, ByVal CurrencyDate As DateTime)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenencfac SET "
            strSQL += ModificarFechaTiempoPlus(CurrencyDate, "currency_date")
            strSQL += ModificarEntero(Currency, "currency")
        Else
            strSQLInicio = " UPDATE jsvenencfac SET "
            strSQLFin = " WHERE " _
               & " numfac = '" & NumeroFactura & "' and " _
               & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroFactura, "numfac")
        strSQL += ModificarFecha(EMISION, "emision")
        strSQL += ModificarCadena(CodigoCliente, "codcli")
        strSQL += ModificarCadena(Comentario, "comen")
        strSQL += ModificarCadena(CodigoVendedor, "codven")
        strSQL += ModificarCadena(Almacen, "almacen")
        strSQL += ModificarCadena(Transporte, "transporte")
        strSQL += ModificarFecha(Vencimiento, "vence")
        strSQL += ModificarCadena(Referencia, "refer")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarEnteroLargo(Items, "items")
        strSQL += ModificarDoble(Cajas, "Cajas")
        strSQL += ModificarDoble(KILOS, "kilos")
        strSQL += ModificarDoble(TotalNeto, "tot_net")
        strSQL += ModificarDoble(PorcentajeDescuento, "pordes")
        strSQL += ModificarDoble(PorcentajeDescuento1, "pordes1")
        strSQL += ModificarDoble(PorcentajeDescuento2, "pordes2")
        strSQL += ModificarDoble(PorcentajeDescuento3, "pordes3")
        strSQL += ModificarDoble(PorcentajeDescuento4, "pordes4")
        strSQL += ModificarDoble(Descuento, "descuen")
        strSQL += ModificarDoble(Descuento1, "descuen1")
        strSQL += ModificarDoble(Descuento2, "descuen2")
        strSQL += ModificarDoble(Descuento3, "descuen3")
        strSQL += ModificarDoble(Descuento4, "descuen4")
        strSQL += ModificarDoble(Cargos, "cargos")
        strSQL += ModificarDoble(Cargos1, "cargos1")
        strSQL += ModificarDoble(Cargos2, "cargos2")
        strSQL += ModificarDoble(Cargos3, "cargos3")
        strSQL += ModificarDoble(Cargos4, "cargos4")
        strSQL += ModificarDoble(TotalFactura1, "tot_fac1")
        strSQL += ModificarDoble(TotalFactura2, "tot_fac2")
        strSQL += ModificarDoble(TotalFactura3, "tot_fac3")
        strSQL += ModificarDoble(TotalFactura4, "tot_fac4")
        strSQL += ModificarFecha(Vencimiento1, "vence1")
        strSQL += ModificarFecha(Vencimiento2, "vence2")
        strSQL += ModificarFecha(Vencimiento3, "vence3")
        strSQL += ModificarFecha(Vencimiento4, "vence4")
        strSQL += ModificarEntero(CondicionDePago, "condpag")
        strSQL += ModificarEntero(TipoCredito, "tipocre")
        strSQL += ModificarCadena(FormaDePago, "formapag")
        strSQL += ModificarCadena(NumeroDePago, "numpag")
        strSQL += ModificarCadena(NombreDePago, "nompag")
        strSQL += ModificarCadena(Caja, "caja")
        strSQL += ModificarDoble(ImporteEfectivo, "importeefectivo")
        strSQL += ModificarCadena(NumeroCheque, "numerocheque")
        strSQL += ModificarCadena(BancoCheque, "bancocheque")
        strSQL += ModificarDoble(ImporteCheque, "importecheque")
        strSQL += ModificarCadena(NumeroTarjeta, "numerotarjeta")
        strSQL += ModificarCadena(CodigoTarjeta, "codigotarjeta")
        strSQL += ModificarDoble(ImporteTarjeta, "importetarjeta")
        strSQL += ModificarCadena(NumeroCestaTicket, "numerocestaticket")
        strSQL += ModificarCadena(NombreCestaTicket, "nombrecestaticket")
        strSQL += ModificarDoble(ImporteCestaTicket, "importecestaticket")
        strSQL += ModificarCadena(NumeroDeposito, "numerodeposito")
        strSQL += ModificarCadena(BancoDeposito, "bancodeposito")
        strSQL += ModificarDoble(ImporteDeposito, "importedeposito")
        strSQL += ModificarDoble(Abono, "abono")
        strSQL += ModificarCadena(Serie, "serie")
        strSQL += ModificarEnteroLargo(NumGiros, "numgiro")
        strSQL += ModificarEnteroLargo(PeriodoEntreGiros, "pergiro")
        strSQL += ModificarDoble(Interes, "interes")
        strSQL += ModificarDoble(PorcentajeInteres, "porint")
        strSQL += ModificarCadena(Asiento, "asiento")
        strSQL += ModificarFecha(FechaAsiento, "fechasi")
        strSQL += ModificarDoble(BaseIVA, "baseiva")
        strSQL += ModificarDoble(PorcentajeIVA, "poriva")
        strSQL += ModificarDoble(ImpuestoIVA, "imp_iva")
        strSQL += ModificarDoble(ImpuestoICS, "imp_ics")
        strSQL += ModificarEntero(TipoFactura, "tipofac")
        strSQL += ModificarEntero(Tipo, "tipo")
        strSQL += ModificarDoble(TotalFactura, "tot_fac")
        strSQL += ModificarCadena(ESTATUS, "estatus")
        strSQL += ModificarCadena(Tarifa, "tarifa")
        strSQL += ModificarCadena(NumeroCXC, "numcxc")
        strSQL += ModificarCadena(OtraCXC, "otra_cxc")
        strSQL += ModificarCadena(OtroCliente, "otro_cli")
        strSQL += ModificarCadena(RelacionGuia, "relguia")
        strSQL += ModificarCadena(RelacionFacturas, "relfacturas")
        strSQL += ModificarEntero(IMPRESA, "impresa")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASEncabezadoNotaCredito(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                              ByVal NumeroNotaCredito As String, ByVal NumeroFactura As String, ByVal EMISION As Date, ByVal FechaIVA As Date,
                              ByVal CodigoCliente As String, ByVal Comentario As String, ByVal CodigoVendedor As String, ByVal Almacen As String, ByVal Transporte As String,
                              ByVal Referencia As String, ByVal CodigoContable As String, ByVal Tarifa As String, ByVal Items As Integer,
                              ByVal Cajas As Integer, ByVal KILOS As Double, ByVal TotalNeto As Double, ByVal ImporteIVA As Double, ByVal ImporteICS As Double,
                              ByVal TotalNotaCredito As Double, ByVal Vencimiento As Date, ByVal CondicionDePago As Integer, ByVal TipoCredito As Integer,
                              ByVal FormaDePago As String, ByVal NumeroDePago As String, ByVal NombreDePago As String, ByVal Beneficiario As String,
                              ByVal Caja As String, ByVal Origen As String, ByVal Estatus As Integer, ByVal Asiento As String, ByVal FechaAsiento As Date,
                              ByVal Impresa As Integer, ByVal RelacionFacturas As String, ByVal RelacionNotasCredito As String,
                                                     ByVal Currency As Integer, ByVal CurrencyDate As DateTime)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenencncr SET "
            strSQL += ModificarFechaTiempoPlus(CurrencyDate, "currency_date")
            strSQL += ModificarEntero(Currency, "currency")
        Else
            strSQLInicio = " UPDATE jsvenencncr SET "
            strSQLFin = " WHERE " _
               & " numncr = '" & NumeroNotaCredito & "' and " _
               & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroNotaCredito, "numncr")
        strSQL += ModificarCadena(NumeroFactura, "numfac")
        strSQL += ModificarFecha(EMISION, "emision")
        strSQL += ModificarFecha(FechaIVA, "fechaiva")
        strSQL += ModificarCadena(CodigoCliente, "codcli")
        strSQL += ModificarCadena(Comentario, "comen")
        strSQL += ModificarCadena(CodigoVendedor, "codven")
        strSQL += ModificarCadena(Almacen, "almacen")
        strSQL += ModificarCadena(Transporte, "transporte")
        strSQL += ModificarCadena(Referencia, "refer")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarCadena(Tarifa, "tarifa")
        strSQL += ModificarEnteroLargo(Items, "items")
        strSQL += ModificarDoble(Cajas, "Cajas")
        strSQL += ModificarDoble(KILOS, "kilos")
        strSQL += ModificarDoble(TotalNeto, "tot_net")
        strSQL += ModificarDoble(ImporteIVA, "imp_iva")
        strSQL += ModificarDoble(ImporteICS, "imp_ics")
        strSQL += ModificarDoble(TotalNotaCredito, "tot_ncr")
        strSQL += ModificarFecha(Vencimiento, "vence")
        strSQL += ModificarEntero(CondicionDePago, "condpag")
        strSQL += ModificarEntero(TipoCredito, "tipocredito")
        strSQL += ModificarCadena(FormaDePago, "formapag")
        strSQL += ModificarCadena(NumeroDePago, "numpag")
        strSQL += ModificarCadena(NombreDePago, "nompag")
        strSQL += ModificarCadena(Beneficiario, "benefic")
        strSQL += ModificarCadena(Caja, "caja")
        strSQL += ModificarCadena(Origen, "origen")
        strSQL += ModificarCadena(Estatus, "estatus")
        strSQL += ModificarCadena(Asiento, "asiento")
        strSQL += ModificarFecha(FechaAsiento, "fechasi")
        strSQL += ModificarEntero(Impresa, "impresa")
        strSQL += ModificarCadena(RelacionFacturas, "relfacturas")
        strSQL += ModificarCadena(RelacionNotasCredito, "relncr")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASEncabezadoNOTADEBITO(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                              ByVal NumeroNotaDebito As String, ByVal NumeroFactura As String, ByVal EMISION As Date,
                              ByVal CodigoCliente As String, ByVal Comentario As String, ByVal CodigoVendedor As String, ByVal Almacen As String, ByVal Transporte As String,
                              ByVal Referencia As String, ByVal CodigoContable As String, ByVal Tarifa As String, ByVal Items As Integer,
                              ByVal Cajas As Integer, ByVal KILOS As Double, ByVal TotalNeto As Double, ByVal ImporteIVA As Double,
                              ByVal TotalNotaDebito As Double, ByVal Vencimiento As Date, ByVal Estatus As Integer, ByVal Asiento As String, ByVal FechaAsiento As Date,
                              ByVal Impresa As Integer, ByVal RelacionFacturas As String,
                                                    ByVal Currency As Integer, ByVal CurrencyDate As DateTime)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenencndb SET "
            strSQL += ModificarFechaTiempoPlus(CurrencyDate, "currency_date")
            strSQL += ModificarEntero(Currency, "currency")
        Else
            strSQLInicio = " UPDATE jsvenencndb SET "
            strSQLFin = " WHERE " _
               & " numndb = '" & NumeroNotaDebito & "' and " _
               & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroNotaDebito, "numndb")
        strSQL += ModificarCadena(NumeroFactura, "numfac")
        strSQL += ModificarFecha(EMISION, "emision")
        strSQL += ModificarCadena(CodigoCliente, "codcli")
        strSQL += ModificarCadena(Comentario, "comen")
        strSQL += ModificarCadena(CodigoVendedor, "codven")
        strSQL += ModificarCadena(Almacen, "almacen")
        strSQL += ModificarCadena(Transporte, "transporte")
        strSQL += ModificarCadena(Referencia, "refer")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarCadena(Tarifa, "tarifa")
        strSQL += ModificarEnteroLargo(Items, "items")
        strSQL += ModificarDoble(Cajas, "Cajas")
        strSQL += ModificarDoble(KILOS, "kilos")
        strSQL += ModificarDoble(TotalNeto, "tot_net")
        strSQL += ModificarDoble(ImporteIVA, "imp_iva")
        strSQL += ModificarDoble(TotalNotaDebito, "tot_ndb")
        strSQL += ModificarFecha(Vencimiento, "vence")
        strSQL += ModificarCadena(Estatus, "estatus")
        strSQL += ModificarCadena(Asiento, "asiento")
        strSQL += ModificarFecha(FechaAsiento, "fechasi")
        strSQL += ModificarEntero(Impresa, "impresa")
        strSQL += ModificarCadena(RelacionFacturas, "relfacturas")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditVENTASRenglonRuta(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                ByVal CodigoRuta As String, ByVal Numero As Integer, ByVal Cliente As String, ByVal NombreCliente As String,
                                ByVal Tipo As Integer, ByVal Aceptado As Integer, ByVal Division As String,
                                ByVal Condicion As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenrenrut SET "
        Else
            strSQLInicio = " UPDATE jsvenrenrut SET "
            strSQLFin = " WHERE " _
                & " codrut = '" & CodigoRuta & "' and " _
                & " cliente = '" & Cliente & "'  and " _
                & " tipo = '" & Tipo & "' and " _
                & " condicion = '" & Condicion & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoRuta, "codrut")
        strSQL += ModificarEnteroLargo(Numero, "numero")
        strSQL += ModificarCadena(Cliente, "cliente")
        strSQL += ModificarCadena(NombreCliente, "nomcli")
        strSQL += ModificarEntero(Tipo, "tipo")
        strSQL += ModificarCadena(Aceptado, "aceptado")
        strSQL += ModificarCadena(Division, "division")
        strSQL += ModificarEntero(Condicion, "condicion")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub IncluyendoRenglonRuta(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Numero As Integer,
                                        ByVal CodigoRuta As String, ByVal Tipo As String, ByVal Division As String,
                                        ByVal Condicion As Integer)

        ft.Ejecutar_strSQL(MyConn, "UPDATE jsvenrenrut SET " _
            & " NUMERO = NUMERO + 1 WHERE " _
            & " NUMERO >= " & Numero & " AND " _
            & " CODRUT = '" & CodigoRuta & "' AND " _
            & " TIPO = '" & Tipo & "' AND " _
            & " division = '" & Division & "' and " _
            & " condicion = " & Condicion & " and " _
            & " ID_EMP = '" & jytsistema.WorkID & "' ")


    End Sub
    Public Sub ModificandoRenglonRuta(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroActual As Integer, ByVal NuevoNumero As Integer,
        ByVal CodigoRuta As String, ByVal Tipo As String, ByVal Division As String, ByVal Condicion As Integer)

        If NumeroActual > NuevoNumero Then
            ft.Ejecutar_strSQL(MyConn, "UPDATE jsvenrenrut SET " _
                    & " NUMERO = NUMERO + 1 WHERE " _
                    & " NUMERO >= " & NuevoNumero & " AND " _
                    & " NUMERO < " & NumeroActual & " AND " _
                    & " CODRUT = '" & CodigoRuta & "' AND " _
                    & " TIPO = '" & Tipo & "' AND " _
                    & " division = '" & Division & "' and " _
                    & " condicion = " & Condicion & " and " _
                    & " ID_EMP = '" & jytsistema.WorkID & "' ")

        ElseIf NumeroActual < NuevoNumero Then
            ft.Ejecutar_strSQL(MyConn, "UPDATE jsvenrenrut SET " _
                    & " NUMERO = NUMERO - 1 WHERE " _
                    & " NUMERO > " & NumeroActual & " AND " _
                    & " NUMERO <= " & NuevoNumero & " AND " _
                    & " CODRUT = '" & CodigoRuta & "' AND " _
                    & " TIPO = '" & Tipo & "' AND " _
                    & " division = '" & Division & "' and " _
                    & " condicion = " & Condicion & " and " _
                    & " ID_EMP = '" & jytsistema.WorkID & "' ")
        End If

    End Sub

    Public Sub EliminandoRenglonRuta(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Numero As Integer,
        ByVal CodigoRuta As String, ByVal Tipo As String, ByVal Division As String, ByVal Condicion As Integer)

        ft.Ejecutar_strSQL(MyConn, "UPDATE jsvenrenrut SET " _
                    & " NUMERO = NUMERO - 1 WHERE " _
                    & " NUMERO > " & Numero & " AND " _
                    & " CODRUT = '" & CodigoRuta & "' AND " _
                    & " TIPO = '" & Tipo & "' AND " _
                    & " division = '" & Division & "' and " _
                    & " condicion = " & Condicion & " and " _
                    & " ID_EMP = '" & jytsistema.WorkID & "' ")

    End Sub

    Public Sub InsertEditVENTASRenglonPresupuesto(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                ByVal NumeroPresupuesto As String, ByVal Renglon As String, ByVal Item As String, ByVal Descripcion As String,
                                ByVal IVA As String, ByVal ICS As String, ByVal Unidad As String, ByVal Bultos As Double, ByVal Cantidad As Double,
                                ByVal CantidadTransito As Double, ByVal Peso As Double, ByVal Estatus As Integer, ByVal Precio As Double,
                                ByVal DescuentoCliente As Double, ByVal DescuentoArticulo As Double, ByVal DescuentoOferta As Double,
                                ByVal TotalRenglon As Double, ByVal TotalRenglonConDescuento As Double, ByVal Aceptado As String, ByVal Editable As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenrencot SET "
        Else
            strSQLInicio = " UPDATE jsvenrencot SET "
            strSQLFin = " WHERE " _
                & " numcot = '" & NumeroPresupuesto & "' and " _
                & " renglon = '" & Renglon & "'  and " _
                & " item = '" & Item & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroPresupuesto, "numcot")
        strSQL += ModificarCadena(Renglon, "renglon")
        strSQL += ModificarCadena(Item, "item")
        strSQL += ModificarCadena(Descripcion, "descrip")
        strSQL += ModificarCadena(IVA, "iva")
        strSQL += ModificarCadena(ICS, "ics")
        strSQL += ModificarCadena(Unidad, "unidad")
        strSQL += ModificarDoble(Bultos, "bultos")
        strSQL += ModificarDoble(Cantidad, "cantidad")
        strSQL += ModificarDoble(CantidadTransito, "cantran")
        strSQL += ModificarDoble(Peso, "peso")
        strSQL += ModificarCadena(Estatus, "estatus")
        strSQL += ModificarDoble(Precio, "precio")
        strSQL += ModificarDoble(DescuentoArticulo, "des_art")
        strSQL += ModificarDoble(DescuentoCliente, "des_cli")
        strSQL += ModificarDoble(DescuentoOferta, "des_ofe")
        strSQL += ModificarDoble(TotalRenglon, "totren")
        strSQL += ModificarDoble(TotalRenglonConDescuento, "totrendes")
        strSQL += ModificarEntero(Aceptado, "aceptado")
        strSQL += ModificarEntero(Editable, "editable")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditVENTASRenglonPrePedidos(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                ByVal NumeroPrePedido As String, ByVal Renglon As String, ByVal Item As String, ByVal Descripcion As String,
                                ByVal IVA As String, ByVal ICS As String, ByVal Unidad As String, ByVal Bultos As Double, ByVal Cantidad As Double,
                                ByVal CantidadTransito As Double, ByVal Inventario As Double, ByVal Sugerido As Double, ByVal Refuerzo As Integer,
                                ByVal Peso As Double, ByVal Lote As String, ByVal Estatus As Integer, ByVal Precio As Double,
                                ByVal DescuentoCliente As Double, ByVal DescuentoArticulo As Double, ByVal DescuentoOferta As Double,
                                ByVal TotalRenglon As Double, ByVal TotalRenglonDescuento As Double, ByVal NumeroPresupuesto As String,
                                ByVal RenglonPresupuesto As String, ByVal CodigoContable As String, ByVal Aceptado As String, ByVal Editable As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenrenpedrgv SET "
        Else
            strSQLInicio = " UPDATE jsvenrenpedrgv SET "
            strSQLFin = " WHERE " _
                & " numped = '" & NumeroPrePedido & "' and " _
                & " renglon = '" & Renglon & "'  and " _
                & " item = '" & Item & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroPrePedido, "numped")
        strSQL += ModificarCadena(Renglon, "renglon")
        strSQL += ModificarCadena(Item, "item")
        strSQL += ModificarCadena(Descripcion, "descrip")
        strSQL += ModificarCadena(IVA, "iva")
        strSQL += ModificarCadena(ICS, "ics")
        strSQL += ModificarCadena(Unidad, "unidad")
        strSQL += ModificarDoble(Bultos, "bultos")
        strSQL += ModificarDoble(Cantidad, "cantidad")
        strSQL += ModificarDoble(CantidadTransito, "cantran")
        strSQL += ModificarDoble(Inventario, "inventario")
        strSQL += ModificarDoble(Sugerido, "sugerido")
        strSQL += ModificarEntero(Refuerzo, "refuerzo")
        strSQL += ModificarDoble(Peso, "peso")
        strSQL += ModificarCadena(Lote, "lote")
        strSQL += ModificarCadena(Estatus, "estatus")
        strSQL += ModificarDoble(Precio, "precio")
        strSQL += ModificarDoble(DescuentoArticulo, "des_art")
        strSQL += ModificarDoble(DescuentoCliente, "des_cli")
        strSQL += ModificarDoble(DescuentoOferta, "des_ofe")
        strSQL += ModificarDoble(TotalRenglon, "totren")
        strSQL += ModificarDoble(TotalRenglonDescuento, "totrendes")
        strSQL += ModificarCadena(NumeroPresupuesto, "numcot")
        strSQL += ModificarCadena(RenglonPresupuesto, "rencot")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarEntero(Aceptado, "aceptado")
        strSQL += ModificarEntero(Editable, "editable")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditVENTASRenglonPedidos(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                               ByVal NumeroPedido As String, ByVal Renglon As String, ByVal Item As String, ByVal Descripcion As String,
                               ByVal IVA As String, ByVal ICS As String, ByVal Unidad As String, ByVal Bultos As Double, ByVal Cantidad As Double,
                               ByVal CantidadTransito As Double, ByVal Inventario As Double, ByVal Sugerido As Double, ByVal Refuerzo As Integer,
                               ByVal Peso As Double, ByVal Lote As String, ByVal Estatus As Integer, ByVal Precio As Double,
                               ByVal DescuentoCliente As Double, ByVal DescuentoArticulo As Double, ByVal DescuentoOferta As Double,
                               ByVal TotalRenglon As Double, ByVal TotalRenglonDescuento As Double, ByVal NumeroPresupuesto As String,
                               ByVal RenglonPresupuesto As String, ByVal CodigoContable As String, ByVal Aceptado As String, ByVal Editable As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenrenped SET "
        Else
            strSQLInicio = " UPDATE jsvenrenped SET "
            strSQLFin = " WHERE " _
                & " numped = '" & NumeroPedido & "' and " _
                & " renglon = '" & Renglon & "'  and " _
                & " item = '" & Item & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroPedido, "numped")
        strSQL += ModificarCadena(Renglon, "renglon")
        strSQL += ModificarCadena(Item, "item")
        strSQL += ModificarCadena(Descripcion, "descrip")
        strSQL += ModificarCadena(IVA, "iva")
        strSQL += ModificarCadena(ICS, "ics")
        strSQL += ModificarCadena(Unidad, "unidad")
        strSQL += ModificarDoble(Bultos, "bultos")
        strSQL += ModificarDoble(Cantidad, "cantidad")
        strSQL += ModificarDoble(CantidadTransito, "cantran")
        strSQL += ModificarDoble(Inventario, "inventario")
        strSQL += ModificarDoble(Sugerido, "sugerido")
        strSQL += ModificarEntero(Refuerzo, "refuerzo")
        strSQL += ModificarDoble(Peso, "peso")
        strSQL += ModificarCadena(Lote, "lote")
        strSQL += ModificarCadena(Estatus, "estatus")
        strSQL += ModificarDoble(Precio, "precio")
        strSQL += ModificarDoble(DescuentoArticulo, "des_art")
        strSQL += ModificarDoble(DescuentoCliente, "des_cli")
        strSQL += ModificarDoble(DescuentoOferta, "des_ofe")
        strSQL += ModificarDoble(TotalRenglon, "totren")
        strSQL += ModificarDoble(TotalRenglonDescuento, "totrendes")
        strSQL += ModificarCadena(NumeroPresupuesto, "numcot")
        strSQL += ModificarCadena(RenglonPresupuesto, "rencot")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarEntero(Aceptado, "aceptado")
        strSQL += ModificarEntero(Editable, "editable")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASRenglonNotasEntrega(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                              ByVal NumeroNotaEntrega As String, ByVal Renglon As String, ByVal Item As String, ByVal Descripcion As String,
                              ByVal IVA As String, ByVal ICS As String, ByVal Unidad As String, ByVal Bultos As Double, ByVal Cantidad As Double,
                              ByVal Inventario As Double, ByVal Sugerido As Double, ByVal Refuerzo As Integer, ByVal Color As String, ByVal Sabor As String,
                              ByVal Peso As Double, ByVal Lote As String, ByVal Estatus As Integer, ByVal Precio As Double,
                              ByVal DescuentoCliente As Double, ByVal DescuentoArticulo As Double, ByVal DescuentoOferta As Double,
                              ByVal TotalRenglon As Double, ByVal TotalRenglonDescuento As Double, ByVal NumeroPresupuesto As String,
                              ByVal RenglonPresupuesto As String, ByVal NumeroPedido As String, ByVal RenglonPedido As String,
                              ByVal FacturaDevolucion As String, ByVal CausaDevolucion As String, ByVal CodigoContable As String,
                              ByVal Aceptado As String, ByVal Editable As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenrennot SET "
        Else
            strSQLInicio = " UPDATE jsvenrennot SET "
            strSQLFin = " WHERE " _
                & " numfac = '" & NumeroNotaEntrega & "' and " _
                & " renglon = '" & Renglon & "'  and " _
                & " item = '" & Item & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroNotaEntrega, "numfac")
        strSQL += ModificarCadena(Renglon, "renglon")
        strSQL += ModificarCadena(Item, "item")
        strSQL += ModificarCadena(Descripcion, "descrip")
        strSQL += ModificarCadena(IVA, "iva")
        strSQL += ModificarCadena(ICS, "ics")
        strSQL += ModificarCadena(Unidad, "unidad")
        strSQL += ModificarDoble(Bultos, "bultos")
        strSQL += ModificarDoble(Cantidad, "cantidad")
        strSQL += ModificarDoble(Inventario, "inventario")
        strSQL += ModificarDoble(Sugerido, "sugerido")
        strSQL += ModificarEntero(Refuerzo, "refuerzo")
        strSQL += ModificarDoble(Precio, "precio")
        strSQL += ModificarDoble(Peso, "peso")
        strSQL += ModificarCadena(Lote, "lote")
        strSQL += ModificarCadena(Color, "color")
        strSQL += ModificarCadena(Sabor, "sabor")
        strSQL += ModificarCadena(Estatus, "estatus")
        strSQL += ModificarDoble(DescuentoArticulo, "des_art")
        strSQL += ModificarDoble(DescuentoCliente, "des_cli")
        strSQL += ModificarDoble(DescuentoOferta, "des_ofe")
        strSQL += ModificarDoble(TotalRenglon, "totren")
        strSQL += ModificarDoble(TotalRenglonDescuento, "totrendes")
        strSQL += ModificarCadena(NumeroPresupuesto, "numcot")
        strSQL += ModificarCadena(RenglonPresupuesto, "rencot")
        strSQL += ModificarCadena(NumeroPedido, "numped")
        strSQL += ModificarCadena(RenglonPedido, "renped")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarCadena(FacturaDevolucion, "facdev")
        strSQL += ModificarCadena(CausaDevolucion, "causadev")
        strSQL += ModificarEntero(Aceptado, "aceptado")
        strSQL += ModificarEntero(Editable, "editable")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditVENTASRenglonFactura(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                              ByVal NumeroFactura As String, ByVal Renglon As String, ByVal Item As String, ByVal Descripcion As String,
                              ByVal IVA As String, ByVal ICS As String, ByVal Unidad As String, ByVal Bultos As Double, ByVal Cantidad As Double,
                              ByVal Inventario As Double, ByVal Sugerido As Double, ByVal Refuerzo As Integer, ByVal Color As String, ByVal Sabor As String,
                              ByVal Peso As Double, ByVal Lote As String, ByVal Estatus As Integer, ByVal Precio As Double,
                              ByVal DescuentoCliente As Double, ByVal DescuentoArticulo As Double, ByVal DescuentoOferta As Double,
                              ByVal TotalRenglon As Double, ByVal TotalRenglonDescuento As Double, ByVal NumeroPresupuesto As String,
                              ByVal RenglonPresupuesto As String, ByVal NumeroPedido As String, ByVal RenglonPedido As String,
                              ByVal NumeroNotaEntrega As String, ByVal RenglonNotaEntrega As String,
                              ByVal FacturaDevolucion As String, ByVal CausaDevolucion As String, ByVal CodigoContable As String,
                              ByVal Aceptado As String, ByVal Editable As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenrenfac SET "
        Else
            strSQLInicio = " UPDATE jsvenrenfac SET "
            strSQLFin = " WHERE " _
                & " numfac = '" & NumeroFactura & "' and " _
                & " renglon = '" & Renglon & "'  and " _
                & " item = '" & Item & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroFactura, "numfac")
        strSQL += ModificarCadena(Renglon, "renglon")
        strSQL += ModificarCadena(Item, "item")
        strSQL += ModificarCadena(Descripcion, "descrip")
        strSQL += ModificarCadena(IVA, "iva")
        strSQL += ModificarCadena(ICS, "ics")
        strSQL += ModificarCadena(Unidad, "unidad")
        strSQL += ModificarDoble(Bultos, "bultos")
        strSQL += ModificarDoble(Cantidad, "cantidad")
        strSQL += ModificarDoble(Inventario, "inventario")
        strSQL += ModificarDoble(Sugerido, "sugerido")
        strSQL += ModificarEntero(Refuerzo, "refuerzo")
        strSQL += ModificarDoble(Precio, "precio")
        strSQL += ModificarDoble(Peso, "peso")
        strSQL += ModificarCadena(Lote, "lote")
        strSQL += ModificarCadena(Color, "color")
        strSQL += ModificarCadena(Sabor, "sabor")
        strSQL += ModificarCadena(Estatus, "estatus")
        strSQL += ModificarDoble(DescuentoArticulo, "des_art")
        strSQL += ModificarDoble(DescuentoCliente, "des_cli")
        strSQL += ModificarDoble(DescuentoOferta, "des_ofe")
        strSQL += ModificarDoble(TotalRenglon, "totren")
        strSQL += ModificarDoble(TotalRenglonDescuento, "totrendes")
        strSQL += ModificarCadena(NumeroPresupuesto, "numcot")
        strSQL += ModificarCadena(RenglonPresupuesto, "rencot")
        strSQL += ModificarCadena(NumeroPedido, "numped")
        strSQL += ModificarCadena(RenglonPedido, "renped")
        strSQL += ModificarCadena(NumeroNotaEntrega, "numnot")
        strSQL += ModificarCadena(RenglonNotaEntrega, "rennot")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarCadena(FacturaDevolucion, "facdev")
        strSQL += ModificarCadena(CausaDevolucion, "causadev")
        strSQL += ModificarEntero(Aceptado, "aceptado")
        strSQL += ModificarEntero(Editable, "editable")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASRenglonNotaDeCredito(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                              ByVal NumeroNotaDeCredito As String, ByVal Renglon As String, ByVal Item As String, ByVal Descripcion As String,
                              ByVal IVA As String, ByVal ICS As String, ByVal Unidad As String, ByVal Bultos As Double, ByVal Cantidad As Double,
                              ByVal Peso As Double, ByVal Lote As String, ByVal Estatus As Integer, ByVal Precio As Double,
                              ByVal DescuentoCliente As Double, ByVal DescuentoArticulo As Double, ByVal DescuentoOferta As Double, ByVal PorcentajeAceptacion As Double,
                              ByVal TotalRenglon As Double, ByVal TotalRenglonDescuento As Double, ByVal NumeroFactura As String,
                              ByVal CodigoContable As String, ByVal Editable As Integer, ByVal CausaDevolucion As String,
                              ByVal Aceptado As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenrenncr SET "
        Else
            strSQLInicio = " UPDATE jsvenrenncr SET "
            strSQLFin = " WHERE " _
                & " numncr = '" & NumeroNotaDeCredito & "' and " _
                & " renglon = '" & Renglon & "'  and " _
                & " item = '" & Item & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroNotaDeCredito, "numncr")
        strSQL += ModificarCadena(Renglon, "renglon")
        strSQL += ModificarCadena(Item, "item")
        strSQL += ModificarCadena(Descripcion, "descrip")
        strSQL += ModificarCadena(IVA, "iva")
        strSQL += ModificarCadena(ICS, "ics")
        strSQL += ModificarCadena(Unidad, "unidad")
        strSQL += ModificarDoble(Bultos, "bultos")
        strSQL += ModificarDoble(Cantidad, "cantidad")
        strSQL += ModificarDoble(Precio, "precio")
        strSQL += ModificarDoble(Peso, "peso")
        strSQL += ModificarCadena(Lote, "lote")
        strSQL += ModificarCadena(Estatus, "estatus")
        strSQL += ModificarDoble(DescuentoArticulo, "des_art")
        strSQL += ModificarDoble(DescuentoCliente, "des_cli")
        strSQL += ModificarDoble(DescuentoOferta, "des_ofe")
        strSQL += ModificarDoble(PorcentajeAceptacion, "por_acepta_dev")
        strSQL += ModificarDoble(TotalRenglon, "totren")
        strSQL += ModificarDoble(TotalRenglonDescuento, "totrendes")
        strSQL += ModificarCadena(NumeroFactura, "numfac")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarEntero(Editable, "editable")
        strSQL += ModificarCadena(CausaDevolucion, "causa")
        strSQL += ModificarEntero(Aceptado, "aceptado")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditVENTASRenglonNOTASDEBITO(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                             ByVal NumeroNotaDebito As String, ByVal Renglon As String, ByVal Item As String, ByVal Descripcion As String,
                             ByVal IVA As String, ByVal ICS As String, ByVal Unidad As String, ByVal Bultos As Double, ByVal Cantidad As Double,
                             ByVal Peso As Double, ByVal Lote As String, ByVal Estatus As Integer, ByVal Precio As Double,
                             ByVal DescuentoCliente As Double, ByVal DescuentoArticulo As Double, ByVal DescuentoOferta As Double,
                             ByVal PorcentajeAceptacion As Double,
                             ByVal TotalRenglon As Double, ByVal TotalRenglonDescuento As Double, ByVal NumeroFactura As String,
                             ByVal RenglonFacturaCodigoContable As String, ByVal Editable As Integer, ByVal CausaDebito As String,
                             ByVal Aceptado As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenrenndb SET "
        Else
            strSQLInicio = " UPDATE jsvenrenndb SET "
            strSQLFin = " WHERE " _
                & " numndb = '" & NumeroNotaDebito & "' and " _
                & " renglon = '" & Renglon & "'  and " _
                & " item = '" & Item & "' and " _
                & " ejercicio = '" & jytsistema.WorkExercise & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroNotaDebito, "numndb")
        strSQL += ModificarCadena(Renglon, "renglon")
        strSQL += ModificarCadena(Item, "item")
        strSQL += ModificarCadena(Descripcion, "descrip")
        strSQL += ModificarCadena(IVA, "iva")
        strSQL += ModificarCadena(ICS, "ics")
        strSQL += ModificarCadena(Unidad, "unidad")
        strSQL += ModificarDoble(Bultos, "bultos")
        strSQL += ModificarDoble(Cantidad, "cantidad")
        strSQL += ModificarDoble(Peso, "peso")
        strSQL += ModificarCadena(Lote, "lote")
        strSQL += ModificarCadena(Estatus, "estatus")
        strSQL += ModificarDoble(Precio, "precio")
        strSQL += ModificarDoble(DescuentoArticulo, "des_art")
        strSQL += ModificarDoble(DescuentoCliente, "des_cli")
        strSQL += ModificarDoble(DescuentoOferta, "des_ofe")
        strSQL += ModificarDoble(PorcentajeAceptacion, "por_acepta_dev")
        strSQL += ModificarDoble(TotalRenglon, "totren")
        strSQL += ModificarDoble(TotalRenglonDescuento, "totrendes")
        strSQL += ModificarCadena(NumeroFactura, "numfac")
        strSQL += ModificarCadena(RenglonFacturaCodigoContable, "codcon")
        strSQL += ModificarEntero(Editable, "editable")
        strSQL += ModificarCadena(CausaDebito, "causa")
        strSQL += ModificarEntero(Aceptado, "aceptado")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditVENTASIVA(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                        ByVal NombreTablaEnBaseDatos As String, ByVal CampoDocumento As String,
                                        ByVal NumeroDocumento As String, ByVal TipoIVA As String, ByVal PorcentajeIVA As Double,
                                        ByVal BaseImponible As Double, ByVal ImpuestoIVA As Double)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into " & NombreTablaEnBaseDatos & " SET "
        Else
            strSQLInicio = " UPDATE " & NombreTablaEnBaseDatos & " SET "
            strSQLFin = " WHERE " _
                & " " & CampoDocumento & " = '" & NumeroDocumento & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroDocumento, CampoDocumento)
        strSQL += ModificarCadena(TipoIVA, "tipoiva")
        strSQL += ModificarDoble(PorcentajeIVA, "poriva")
        strSQL += ModificarDoble(BaseImponible, "baseiva")
        strSQL += ModificarDoble(ImpuestoIVA, "impiva")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASCliente(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
            ByVal CodigoCliente As String, ByVal Nombre As String, ByVal Categoria As String, ByVal Unidad As String, ByVal Rif As String,
            ByVal Nit As String, ByVal CI As String, ByVal Alterno As String, ByVal DireccionFiscal As String, ByVal PaisFiscal As Integer,
            ByVal EstadoFiscal As Integer, ByVal Municipiofiscal As Integer, ByVal ParroquiaFiscal As Integer, ByVal CiudadFiscal As Integer, ByVal BarrioFiscal As Integer,
            ByVal ZipFiscal As String, ByVal DireccionDespacho As String, ByVal PaisDespacho As Integer,
            ByVal EstadoDespacho As Integer, ByVal MunicipioDespacho As Integer, ByVal ParroquiaDespacho As Integer, ByVal CiudadDespacho As Integer, ByVal BarrioDespacho As Integer,
            ByVal ZipDespacho As String, ByVal CodigoGeografico As Integer, ByVal email1 As String, ByVal email2 As String, ByVal email3 As String, ByVal email4 As String,
            ByVal Telefono1 As String, ByVal Telefono2 As String, ByVal Telefono3 As String, ByVal Fax As String, ByVal Gerente As String,
            ByVal TelefonoGerente As String, ByVal Contacto As String, ByVal TelefonoContacto As String, ByVal LimiteCredito As Double,
            ByVal Disponible As Double, ByVal DescuentoCliente As Double, ByVal DescuentoPP1 As Double, ByVal DescuentoPP2 As Double,
            ByVal DescuentoPP3 As Double, ByVal DescuentoPP4 As Double, ByVal DesdePP1 As Integer, ByVal DesdePP2 As Integer,
            ByVal DesdePP3 As Integer, ByVal DesdePP4 As Integer, ByVal HastaPP1 As Integer, ByVal HastaPP2 As Integer, ByVal HastaPP3 As Integer,
            ByVal HastaPP4 As Integer, ByVal Saldo As Double, ByVal RegimenIVA As String, ByVal Tarifa As String,
            ByVal ListaDePrecios As String, ByVal FormaDePago As String, ByVal FechaIngreso As Date,
            ByVal CodigoContable As String, ByVal CodigoCreacion As Integer, ByVal Estatus As Integer, ByVal RequisitoRIF As Integer,
            ByVal RequisitoNIT As Integer, ByVal RequisitoRecibo As Integer, ByVal RequisitoCI As Integer, ByVal RequisitoRegistro As Integer,
            ByVal RequisitoRegistroActual As Integer, ByVal RequisitoReferenciaBanco As Integer, ByVal RequisitoReferComercio As Integer,
            ByVal FrecuenciaVisita As Integer, ByVal FechaUltimaVisita As Date, ByVal InicioVisitas As Date, ByVal Merchandising As Integer,
            ByVal PermiteChequesDevueltos As Integer, ByVal DiaDePago As Integer, ByVal HoraDePago As String, ByVal HoraAPago As String,
            ByVal Ranking As Integer, ByVal FechaRanking As Date, ByVal Comentario As String, ByVal TipoContribuyente As Integer,
            ByVal Share As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvencatcli SET "
        Else
            strSQLInicio = " UPDATE jsvencatcli SET "
            strSQLFin = " WHERE " _
                & " codcli = '" & CodigoCliente & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoCliente, "codcli")
        strSQL += ModificarCadena(Nombre, "nombre")
        strSQL += ModificarCadena(Categoria, "categoria")
        strSQL += ModificarCadena(Unidad, "unidad")
        strSQL += ModificarCadena(Rif, "rif")
        strSQL += ModificarCadena(Nit, "nit")
        strSQL += ModificarCadena(CI, "CI")
        strSQL += ModificarCadena(Alterno, "alterno")
        strSQL += ModificarCadena(DireccionFiscal, "dirfiscal")
        strSQL += ModificarEnteroLargo(PaisFiscal, "fpais")
        strSQL += ModificarEnteroLargo(EstadoFiscal, "festado")
        strSQL += ModificarEnteroLargo(Municipiofiscal, "fmunicipio")
        strSQL += ModificarEnteroLargo(ParroquiaFiscal, "fparroquia")
        strSQL += ModificarEnteroLargo(CiudadFiscal, "fciudad")
        strSQL += ModificarEnteroLargo(BarrioFiscal, "fbarrio")
        strSQL += ModificarCadena(ZipFiscal, "fzip")
        strSQL += ModificarCadena(DireccionDespacho, "dirDespa")
        strSQL += ModificarEnteroLargo(PaisDespacho, "dpais")
        strSQL += ModificarEnteroLargo(EstadoDespacho, "destado")
        strSQL += ModificarEnteroLargo(MunicipioDespacho, "dmunicipio")
        strSQL += ModificarEnteroLargo(ParroquiaDespacho, "dparroquia")
        strSQL += ModificarEnteroLargo(CiudadDespacho, "dciudad")
        strSQL += ModificarEnteroLargo(BarrioDespacho, "dbarrio")
        strSQL += ModificarCadena(ZipDespacho, "dzip")
        strSQL += ModificarEnteroLargo(CodigoGeografico, "codgeo")
        strSQL += ModificarCadena(email1, "email1")
        strSQL += ModificarCadena(email2, "email2")
        strSQL += ModificarCadena(email3, "email3")
        strSQL += ModificarCadena(email4, "email4")
        strSQL += ModificarCadena(Telefono1, "telef1")
        strSQL += ModificarCadena(Telefono2, "telef2")
        strSQL += ModificarCadena(Telefono3, "telef3")
        strSQL += ModificarCadena(Fax, "fax")
        strSQL += ModificarCadena(Gerente, "gerente")
        strSQL += ModificarCadena(TelefonoGerente, "telger")
        strSQL += ModificarCadena(Contacto, "contacto")
        strSQL += ModificarCadena(TelefonoContacto, "telcon")
        strSQL += ModificarDoble(LimiteCredito, "limitecredito")
        strSQL += ModificarDoble(Disponible, "disponible")
        strSQL += ModificarDoble(DescuentoCliente, "des_cli")
        strSQL += ModificarDoble(DescuentoPP1, "desc_cli_1")
        strSQL += ModificarDoble(DescuentoPP2, "desc_cli_2")
        strSQL += ModificarDoble(DescuentoPP3, "desc_cli_3")
        strSQL += ModificarDoble(DescuentoPP4, "desc_cli_4")
        strSQL += ModificarEnteroLargo(DesdePP1, "desde_1")
        strSQL += ModificarEnteroLargo(DesdePP2, "desde_2")
        strSQL += ModificarEnteroLargo(DesdePP3, "desde_3")
        strSQL += ModificarEnteroLargo(DesdePP4, "desde_4")
        strSQL += ModificarEnteroLargo(HastaPP1, "hasta_1")
        strSQL += ModificarEnteroLargo(HastaPP2, "hasta_2")
        strSQL += ModificarEnteroLargo(HastaPP3, "hasta_3")
        strSQL += ModificarEnteroLargo(HastaPP4, "hasta_4")
        strSQL += ModificarDoble(Saldo, "saldo")
        strSQL += ModificarCadena(RegimenIVA, "regimeniva")
        strSQL += ModificarCadena(Tarifa, "tarifa")
        strSQL += ModificarCadena(ListaDePrecios, "lispre")
        strSQL += ModificarCadena(FormaDePago, "formapago")
        strSQL += ModificarFecha(FechaIngreso, "ingreso")
        strSQL += ModificarCadena(CodigoContable, "codcon")
        strSQL += ModificarEntero(CodigoCreacion, "codcre")
        strSQL += ModificarEntero(Estatus, "estatus")
        strSQL += ModificarEntero(RequisitoRIF, "req_rif")
        strSQL += ModificarEntero(RequisitoNIT, "req_nit")
        strSQL += ModificarEntero(RequisitoRecibo, "req_rec")
        strSQL += ModificarEntero(RequisitoCI, "req_cis")
        strSQL += ModificarEntero(RequisitoRegistro, "req_reg")
        strSQL += ModificarEntero(RequisitoRegistroActual, "req_rea")
        strSQL += ModificarEntero(RequisitoReferenciaBanco, "req_ban")
        strSQL += ModificarEntero(RequisitoReferComercio, "req_com")
        strSQL += ModificarEntero(FrecuenciaVisita, "fecvisita")
        strSQL += ModificarFecha(InicioVisitas, "inivisita")
        strSQL += ModificarFecha(FechaUltimaVisita, "fecultvisita")
        strSQL += ModificarEntero(Merchandising, "merchandising")
        strSQL += ModificarEntero(PermiteChequesDevueltos, "backorder")
        strSQL += ModificarEntero(DiaDePago, "diapago")
        strSQL += ModificarCadena(HoraDePago, "depago")
        strSQL += ModificarCadena(HoraAPago, "apago")
        strSQL += ModificarEntero(Ranking, "ranking")
        strSQL += ModificarFecha(FechaRanking, "fecharank")
        strSQL += ModificarCadena(Comentario, "comentario")
        strSQL += ModificarEntero(TipoContribuyente, "especial")
        strSQL += ModificarEntero(Share, "share")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASDescuento(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                       ByVal NombreTablaEnBD As String, ByVal NombreCampoDocumentoEnBD As String,
                                       ByVal NumeroDocumento As String, ByVal numRenglon As String, ByVal DescripcionDescuento As String,
                                       ByVal PorcentajeDescuento As Double, ByVal MontoDescuento As Double)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into " & NombreTablaEnBD & " SET "
        Else
            strSQLInicio = " UPDATE " & NombreTablaEnBD & " SET "
            strSQLFin = " WHERE " _
                & " " & NombreCampoDocumentoEnBD & " = '" & NumeroDocumento & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroDocumento, NombreCampoDocumentoEnBD)
        strSQL += ModificarCadena(numRenglon, "renglon")
        strSQL += ModificarCadena(DescripcionDescuento, "descrip")
        strSQL += ModificarDoble(PorcentajeDescuento, "pordes")
        strSQL += ModificarDoble(MontoDescuento, "descuento")
        strSQL += ModificarDoble(0.0, "subtotal") 'NO ME ACUERDO PORQUE ES CERO, PERO INFIERO QUE PARA CALCULAR INDIVIDUALMENTE SIN ORDEN D1 + D2 + ...
        strSQL += ModificarCadena("1", "aceptado")
        strSQL += ModificarCadena(jytsistema.WorkExercise, "ejercicio")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditVENTASComentarioRenglon(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                       ByVal NumeroDocumento As String, ByVal Origen As String,
                                       ByVal Item As String, ByVal numRenglon As String,
                                       ByVal Comentario As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenrencom SET "
        Else
            strSQLInicio = " UPDATE jsvenrencom SET "
            strSQLFin = " WHERE " _
                & " numdoc = '" & NumeroDocumento & "' and " _
                & " origen = '" & Origen & "' and " _
                & " item = '" & Item & "' and " _
                & " renglon = '" & numRenglon & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(NumeroDocumento, "numdoc")
        strSQL += ModificarCadena(numRenglon, "renglon")
        strSQL += ModificarCadena(Origen, "origen")
        strSQL += ModificarCadena(Item, "item")
        strSQL += ModificarCadena(Comentario, "comentario")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditVENTASCanaldistribucionTiponegocio(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                       ByVal Codigo As String, ByVal Descripcion As String,
                                       ByVal Antecesor As String, ByVal nomTabla As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into " & nomTabla & " SET "
        Else
            strSQLInicio = " UPDATE " & nomTabla & " SET "
            strSQLFin = " WHERE " _
                & " codigo = '" & Codigo & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(Codigo, "codigo")
        strSQL += ModificarCadena(Descripcion, "descrip")
        If Antecesor <> "" Then strSQL += ModificarCadena(Antecesor, "antec")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditVENTASAsociado(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                        ByVal CodigoCliente As String, ByVal Nacionalidad As String, ByVal CedulaIdentidad As String,
                                        ByVal NombreAsociado As String, ByVal Expediente As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvencedsoc SET "
        Else
            strSQLInicio = " UPDATE jsvencedsoc SET "
            strSQLFin = " WHERE " _
                & " codcli = '" & CodigoCliente & "' and " _
                & " nacional = '" & Nacionalidad & "' and " _
                & " ci = '" & CedulaIdentidad & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoCliente, "codcli")
        strSQL += ModificarCadena(Nacionalidad, "nacional")
        strSQL += ModificarCadena(CedulaIdentidad, "ci")
        strSQL += ModificarCadena(NombreAsociado, "nombre")
        strSQL += ModificarCadena(Expediente, "expediente")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASClientesVisitasDespachosPagos(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                                       ByVal CodigoCliente As String, ByVal Dia As Integer, ByVal desde As String, ByVal Hasta As String,
                                       ByVal Desdepm As String, ByVal Hastapm As String, ByVal Tipo As Integer, ByVal Division As String)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvencatvis SET "
        Else
            strSQLInicio = " UPDATE jsvencatvis SET "
            strSQLFin = " WHERE " _
                & " codcli = '" & CodigoCliente & "' and " _
                & " dia = " & Dia & " and " _
                & " tipo = " & Tipo & " and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoCliente, "codcli")
        strSQL += ModificarEntero(Dia, "dia")
        strSQL += ModificarCadena(desde, "desde")
        strSQL += ModificarCadena(Hasta, "hasta")
        strSQL += ModificarCadena(Desdepm, "desdepm")
        strSQL += ModificarCadena(Hastapm, "hastapm")
        strSQL += ModificarEntero(Tipo, "tipo")
        strSQL += ModificarCadena(Division, "division")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASExpedienteCliente(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                        ByVal CodigoCliente As String, ByVal FechaMovimiento As Date,
                        ByVal Comentario As String, Condicion As Integer, Causa As String,
                        ByVal TipoCondicion As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvenexpcli SET "
        Else
            strSQLInicio = " UPDATE jsvenexpcli SET "
            strSQLFin = " WHERE " _
                & " codcli = '" & CodigoCliente & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoCliente, "codcli")
        strSQL += ModificarFechaTiempo(FechaMovimiento, "fecha")
        strSQL += ModificarCadena(Comentario, "comentario")
        strSQL += ModificarEntero(Condicion, "condicion")
        strSQL += ModificarCadena(Causa, "causa")
        strSQL += ModificarEntero(TipoCondicion, "tipocondicion")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

    Public Sub InsertEditVENTASDescuentosAsesores(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                        ByVal CodigoDescuento As String, Descripcion As String, PorcentajeDescuento As Double,
                        ByVal FechaInicio As Date, FechaFinal As Date, CodigoVendedor As String,
                        TipoVendedor As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsconcatdes SET "
        Else
            strSQLInicio = " UPDATE jsconcatdes SET "
            strSQLFin = " WHERE " _
                & " coddes = '" & CodigoDescuento & "' and " _
                & " codven = '" & CodigoVendedor & "' AND " _
                & " tipo = '" & TipoVendedor & "' and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoDescuento, "coddes")
        strSQL += ModificarCadena(Descripcion, "descrip")
        strSQL += ModificarDoble(PorcentajeDescuento, "pordes")
        strSQL += ModificarFecha(FechaInicio, "inicio")
        strSQL += ModificarFecha(FechaFinal, "fin")
        strSQL += ModificarCadena(CodigoVendedor, "codven")
        strSQL += ModificarEntero(TipoVendedor, "tipo")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub



    Public Sub insertEditVentasCuotasMercancias(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoAsesor As String)
        ft.Ejecutar_strSQL(MyConn, " replace into jsvencuoart " _
                       & " SELECT '" & CodigoAsesor & "', a.codart, 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0, '', a.id_emp " _
                       & " FROM jsmerctainv a " _
                       & " WHERE " _
                       & " a.id_emp = '" & jytsistema.WorkID & "' ")
    End Sub


    Public Sub InsertEditVENTASComisionesJerarquias(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                        ByVal CodigoVendedor As String, CodigoJerarquia As String, Porcentaje_Cuota As Double,
                        TipoFactor As Integer)

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvencomven SET "
        Else
            strSQLInicio = " UPDATE jsvencomven SET "
            strSQLFin = " WHERE " _
                & " codven = '" & CodigoVendedor & "' and " _
                & " tipjer = '" & CodigoJerarquia & "' AND " _
                & " tipo = " & TipoFactor & " and " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoVendedor, "codven")
        strSQL += ModificarCadena(CodigoJerarquia, "tipjer")
        strSQL += ModificarDoble(Porcentaje_Cuota, "por_ventas")
        strSQL += ModificarEntero(TipoFactor, "tipo")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub
    Public Sub InsertEditVENTASComisionesCobranza(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal Insertar As Boolean,
                        ByVal CodigoVendedor As String, Desde As Integer, Hasta As Integer, Porcentaje_Cobranza As Double,
                        Optional CodigoJerarquia As String = "XX")

        Dim strSQL As String = ""
        Dim strSQLInicio As String = ""
        Dim strSQLFin As String = ""

        If Insertar Then
            strSQLInicio = " insert into jsvencomvencob SET "
        Else
            strSQLInicio = " UPDATE jsvencomvencob SET "
            strSQLFin = " WHERE " _
                & " codven = '" & CodigoVendedor & "' and " _
                & " tipjer = '" & CodigoJerarquia & "' AND " _
                & " DE = " & Desde & " AND " _
                & " A = " & Hasta & " AND " _
                & " id_emp = '" & jytsistema.WorkID & "' "
        End If

        strSQL += ModificarCadena(CodigoVendedor, "codven")
        strSQL += ModificarCadena(CodigoJerarquia, "tipjer")
        strSQL += ModificarEnteroLargo(Desde, "de")
        strSQL += ModificarEnteroLargo(Hasta, "a")
        strSQL += ModificarDoble(Porcentaje_Cobranza, "por_cobranza")
        strSQL += ModificarCadena(jytsistema.WorkID, "id_emp")

        ft.Ejecutar_strSQL(MyConn, Actualizar_strSQL(strSQLInicio, strSQL, strSQLFin))

    End Sub

End Module
