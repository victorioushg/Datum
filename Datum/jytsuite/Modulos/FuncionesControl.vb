Imports MySql.Data.MySqlClient
Imports Syncfusion.WinForms.ListView
Imports Syncfusion.WinForms.ListView.Enums

Module FuncionesControl
    Private ft As New Transportables
    Public Structure aParametros

        Dim sGestion As Integer
        Dim sCodigo As String
        Dim sDescripcion As String
        Dim sTipo As Integer
        Dim sValor As String

    End Structure
    Public Structure aContadores

        Dim sGestion As Integer
        Dim sCodigo As String
        Dim sDescripcion As String
        Dim sContador As String

    End Structure
    Public Enum TipoParametro

        iNoSi = 0
        iDoble = 1
        iCadena = 2
        iDiaSemana = 3
        iEntero = 4
        iCantidadPeso = 5
        iFecha = 6
        iFormatoPapel = 7
        iModoImpresora = 8
        iTipoImpresoraFiscal = 9
        iNumeracionContable = 10
        iVector = 11
        iTabla = 12
        iMoneda = 13

    End Enum

    Public Function NumeroDeRegistrosEnTabla(MyConn As MySqlConnection, nTabla As String, Optional Condicion As String = "") As Integer
        Return ft.DevuelveScalarEntero(MyConn, " select COUNT(*) from " & nTabla & " where " & Condicion & " AND id_emp = '" & jytsistema.WorkID & "' ")
    End Function

    Public Sub InsertarParametro(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal numGestion As Integer, ByVal numModulo As Integer,
        ByVal Codigo As String, ByVal Descripcion As String, ByVal TipoParametro As Integer, ByVal Valor As String, Optional Empresa As String = "")

        If Empresa = "" Then Empresa = jytsistema.WorkID

        If ExisteParametro(MyConn, lblInfo, numGestion, Codigo, Empresa) Then
            ft.Ejecutar_strSQL(MyConn, " update jsconparametros set descripcion = '" & Descripcion & "' where codigo = '" & Codigo & "' and id_emp = '" & Empresa & "'  ")
        Else
            ft.Ejecutar_strSQL(MyConn, " insert into jsconparametros set " _
                & " gestion = " & numGestion & ", modulo = " & numModulo & ", codigo = '" & Codigo & "',  " _
                & " descripcion = '" & Descripcion & "', tipo = " & TipoParametro & ", valor = '" & Valor & "', id_emp = '" & Empresa & "' ")
        End If

    End Sub
    Public Sub InsertarContador(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal numGestion As Integer, ByVal numModulo As String,
        ByVal Codigo As String, ByVal Descripcion As String, ByVal Valor As String, Optional Empresa As String = "")

        If Empresa = "" Then Empresa = jytsistema.WorkID

        If ExisteContador(MyConn, lblInfo, numGestion, Codigo, Empresa) Then
            ft.Ejecutar_strSQL(MyConn, " update jsconcontadores set descripcion = '" & Descripcion & "' " _
                               & " where " _
                               & " codigo = '" & Codigo & "' and " _
                               & " id_emp = '" & Empresa & "'  ")
        Else
            ft.Ejecutar_strSQL(MyConn, " insert into jsconcontadores set " _
                & " gestion = " & numGestion & ", modulo = '" & numModulo & "', codigo = '" & Codigo & "',  " _
                & " descripcion = '" & Descripcion & "', contador = '" & Valor & "', id_emp = '" & Empresa & "' ")
        End If

    End Sub

    Function ExisteParametro(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal numGestion As Integer,
                             ByVal nomParametro As String, Empresa As String) As Boolean
        Dim afld() As String = {"gestion", "codigo", "id_emp"}
        Dim aStr() As String = {numGestion, nomParametro, Empresa}
        If qFound(MyConn, lblInfo, "jsconparametros", afld, aStr) Then ExisteParametro = True
    End Function
    Function ExisteContador(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal numGestion As Integer,
                            ByVal nomContador As String, Empresa As String) As Boolean
        Dim afld() As String = {"gestion", "codigo", "id_emp"}
        Dim aStr() As String = {numGestion, nomContador, Empresa}
        If qFound(MyConn, lblInfo, "jsconcontadores", afld, aStr) Then ExisteContador = True

    End Function

    Public Sub IniciarParametros(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, Optional Empresa As String = "")

        If Empresa = "" Then Empresa = jytsistema.WorkID

        If ExisteTabla(Myconn, jytsistema.WorkDataBase, "jsconparametros") Then

            '1 // CONTABILIDAD
            InsertarParametro(Myconn, lblInfo, Gestion.iContabilidad, 0, "CONPARAM00", "0. GENERAL ", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iContabilidad, 0, "CONPARAM01", "0.01 Máscara o plantilla contable... ", TipoParametro.iCadena, "X.X.XX.XX.XXX.XXXX", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iContabilidad, 0, "CONPARAM02", "0.02 ¿Valida máscara en cuentas contables? ... ", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iContabilidad, 0, "CONPARAM03", "0.03 Cuenta contable de resultado del ejercicio ... ", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iContabilidad, 0, "CONPARAM04", "0.04 Poder indicar el número de asiento manualmente ? ", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iContabilidad, 0, "CONPARAM05", "0.05 Forma de numeración de asientos ó pólizas contable ... ", TipoParametro.iNumeracionContable, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iContabilidad, 0, "CONPARAM06", "0.06 Moneda por defecto ... ", TipoParametro.iMoneda, "0", Empresa)

            '2 // BANCOS Y CAJAS
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM00", "0. GENERAL ", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM16", "0.01 Validar CUENTA CONTABLE al incluir Banco ...", TipoParametro.iNoSi, 0, Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM17", "0.02 Validar CUENTA CONTABLE al incluir Caja ...", TipoParametro.iNoSi, 0, Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM01", "0.03 Tipos de documentos para los cuales aplica el Impuesto al Débito Bancario (I.D.B.) ...", TipoParametro.iCadena, ".ND.CH.", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM02", "0.04 Porcentaje del Impuesto al Débito Bancario (I.D.B.) ...", TipoParametro.iDoble, "0.00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM03", "0.05 Monto mínimo de la transacción para la cual aplica el Impuesto al Débito Bancario (I.D.B.) ...", TipoParametro.iDoble, "0.00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM04", "0.06 Descripción o Nombre con el cual se identifica el Impuesto al Débito Bancario (I.D.B.) ...", TipoParametro.iCadena, "IMPUESTO AL DEBITO BANCARIO", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM10", "0.07 Activar IDB automáticamente cuando se realiza la transacción (Proceso = NO) ...", TipoParametro.iNoSi, 0, Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM11", "0.08 Tipos de documentos para los cuales aplica COMISION ALTO VALOR (CAV) ...", TipoParametro.iCadena, ".CH.", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM12", "0.09 Monto comisión alto valor (CAV) ...", TipoParametro.iDoble, "0.00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM13", "0.10 Monto mínimo de la transacción para la cual aplica la CAV ...", TipoParametro.iDoble, "0.00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM14", "0.11 Descripción o Nombre con el cual se identifica la comisión por alto valor (CAV) ...", TipoParametro.iCadena, "COMISION ALTO VALOR", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM15", "0.12 Activar CAV ...", TipoParametro.iNoSi, 0, Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM16", "0.13 Moneda por defecto ...", TipoParametro.iMoneda, 0, Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM05", "1. CHEQUES DEVUELTOS ", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM06", "1.01 Al registrar CHEQUE DEVUELTO imprimir NOTA DE DEBITO FISCAL por la Comisión y Gastos Administrativos ...", TipoParametro.iNoSi, 1, Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM07", "2. MOVIMIENTOS BANCARIOS ", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM08", "2.01 Al realizar movimiento en bancos REGISTRAR asiento contable obligatoriamente ...", TipoParametro.iNoSi, 1, Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iBancos, 0, "BANPARAM09", "2.02 Al realizar movimiento en cheque VALIDAR proveedor de gasto/compra ...", TipoParametro.iNoSi, 1, Empresa)


            '3 // NOMINA
            InsertarParametro(Myconn, lblInfo, Gestion.iRecursosHumanos, 0, "NOMPARAM00", "0. GENERAL", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iRecursosHumanos, 0, "NOMPARAM01", "0.01 Día inicial de la semana ...", TipoParametro.iDiaSemana, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iRecursosHumanos, 0, "NOMPARAM02", "0.02 ¿Desea validar las cuentas bancarias de empleados Vs. las cuentas de la empresa? ...", TipoParametro.iNoSi, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iRecursosHumanos, 0, "NOMPARAM03", "0.03 Código de Banco 1 para validación de cuentas de empleados ...", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iRecursosHumanos, 0, "NOMPARAM04", "0.04 Código de Banco 2 para validación de cuentas de empleados ...", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iRecursosHumanos, 0, "NOMPARAM05", "0.05 Validar Cuenta por pagar/Cuenta por cobrar en definición de Conceptos ...", TipoParametro.iNoSi, "0")
            InsertarParametro(Myconn, lblInfo, Gestion.iRecursosHumanos, 0, "NOMPARAM06", "0.06 Validar Cuenta Contable en definición de Conceptos ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iRecursosHumanos, 0, "NOMPARAM07", "0.07 Moneda por defecto ...", TipoParametro.iMoneda, "0", Empresa)

            '4 // COMPRAS
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 0, "COMPARAM00", "0. GENERAL", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 0, "COMPARAM01", "0.01 ¿Exigir código contable en la Compra? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 0, "COMPARAM02", "0.02 Para recepciones y/o compras utilizar costo promedio (No = último costo)...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 0, "COMPARAM03", "0.03 ¿Valida existencias en Notas de Crédito (Devoluciones)? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 0, "COMPARAM04", "0.04 ¿Exigir código contable en el proveedor? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 0, "COMPARAM05", "0.05 ¿Exigir código contable en ABONOS, CANCELACIONES, ETC.? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 0, "COMPARAM06", "0.06 Moneda por defecto...", TipoParametro.iMoneda, "0", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 1, "COMPAGAS00", "1. GASTOS", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 1, "COMPAGAS03", "1.01 ¿Permite modificar y/o eliminar GASTOS?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 1, "COMPAGAS01", "1.02 ¿Permite editar número del Gasto? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 1, "COMPAGAS02", "1.03 ¿Permite editar fecha emisión del Gasto? ...", TipoParametro.iNoSi, "0", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 2, "COMPAORD00", "2. ORDENES DE COMPRA", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 2, "COMPAORD01", "2.01 ¿Permite modificar y/o eliminar ORDENES DE COMPRA?", TipoParametro.iNoSi, "0", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 3, "COMPAREC00", "3. RECEPCIONES DE MERCANCIAS", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 3, "COMPAREC01", "3.01 ¿Permite modificar y/o eliminar RECEPCIONES DE MERCANCIAS?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 3, "COMPAREC02", "3.02 ¿Actualizar INVENTARIOS desde recepción ?", TipoParametro.iNoSi, "0", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 4, "COMPACOM00", "4. COMPRAS", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 4, "COMPACOM06", "4.01 ¿Permite modificar y/o eliminar COMPRAS?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 4, "COMPACOM01", "4.02 ¿Permite editar número de la Compra? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 4, "COMPACOM02", "4.03 ¿Permite editar fecha emisión de la Compra? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 4, "COMPACOM03", "4.04 ¿Actualización automática de inventario? ...", TipoParametro.iNoSi, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 4, "COMPACOM04", "4.05 ¿Actualización automática de precios a partir de las ganancias? ...", TipoParametro.iNoSi, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 4, "COMPACOM05", "4.06 ¿Utilizar factor de división para el cálculo de precios automático? ...", TipoParametro.iNoSi, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 4, "COMPACOM07", "4.07 ¿Exigir que todos los ítems de COMPRA provengan de UNA RECEPCION?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 4, "COMPACOM08", "4.08 ¿Validar COMPRA vs. RECEPCION Y producir NOTA DEBITO/CREDITO POR LAS DIFERENCIAS?", TipoParametro.iNoSi, "0", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 5, "COMPANCC00", "5. NOTAS CREDITO", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 5, "COMPANCC01", "5.01 ¿Permite modificar y/o eliminar NOTAS CREDITO?", TipoParametro.iNoSi, "0", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 6, "COMPANDC00", "6. NOTAS DEBITO", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iCompras, 6, "COMPANDC01", "6.01 ¿Permite modificar y/o eliminar NOTAS DEBITO?", TipoParametro.iNoSi, "0", Empresa)



            '5 // VENTAS
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM00", "0. GENERAL", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM01", "0.01 Porcentaje comisión por cheques devueltos ...", TipoParametro.iDoble, "0.00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM02", "0.02 Monto por gastos administratvos de cheques devueltos ...", TipoParametro.iDoble, "0.00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM29", "0.03 Código de servicio para (%) porcentaje comisión por cheque devuelto ...", TipoParametro.iCadena, "00000135", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM30", "0.04 Código de servicio para Gastos Administrativos por cheque devuelto ...", TipoParametro.iCadena, "00000136", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM31", "0.05 Código de servicio para cheque devuelto ...", TipoParametro.iCadena, "00000137", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM03", "0.06 Cantidad o número máximo de cheques devueltos permitidos por cliente en un año ...", TipoParametro.iEntero, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM04", "0.07 ¿Código contable obligatorio?...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM05", "0.08 ¿Valida el diferencial de peso permitido entre el valor de peso transcrito y el valor de peso calculado? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM06", "0.09 Diferencial (Tolerancia) de cantidad permitido entre el valor transcrito y el valor calculado ", TipoParametro.iCantidadPeso, "0.000", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM08", "0.10 ¿Habilitar ventana de impresión de facturas en guía de despacho?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM32", "0.11 ¿Habilitar ventana de impresión de Notas de Crédito en Relaciones de Notas Crédito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM16", "0.12 ¿Valida el peso máximo de carga de los camiones contra el peso estimado en la guía de carga?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM24", "0.13 ¿Proceso de BLOQUEO de clientes automático?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM34", "0.14 Cantidad de días para BLOQUEO después de su VENCIMIENTO", TipoParametro.iEntero, "8", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM35", "0.15 Causa que se utilizará para el BLOQUEO?", TipoParametro.iCadena, "00001", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM36", "0.16 Volver a colocar NO FACTURAR en proceso BLOQUEO si cliente posee CHEQUES DEVUELTOS?", TipoParametro.iNoSi, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM37", "0.17 ¿FACTURAR si cliente posee ESTATUS BLOQUEADO/INACTIVO/DESINCORPORADO?", TipoParametro.iNoSi, "0", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM26", "0.18 ¿Pide y valida Número de Depósito Bancario en proceso de pre-cancelaciones a cancelaciones ?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM27", "0.19 Ruta de Visita inicial para el registro de cliente NUEVO ...", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM33", "0.20 Ruta de Visita para el clientes DESINCORPORADOS ...", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM28", "0.21 Origen del NUMERO DE CONTROL (0=registro, 1=contador, 2=Máquina Fiscal, 3=Contador Anterior...", TipoParametro.iEntero, "2", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM38", "0.22 SI Origen del NUMERO DE CONTROL ES CERO (0=registro) Verificar TABLA de Números de Control ...", TipoParametro.iTabla, SystemInformation.ComputerName, Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM39", "0.23 ACTIVAR FACTURACION SEGUN DECRETO 3085 (REBAJAS DE IVA POR RANGOS/PAGOS ELECTRONICOS)  ...", TipoParametro.iNoSi, "0")
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM40", "0.24 ¿Imprimir adicionalmente total de COT/PED/ENT/FAC//NCR/NDB en moneda EXTRANJERA?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 0, "VENPARAM41", "0.25 Moneda por defecto...", TipoParametro.iMoneda, "0", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 1, "VENCOTPA00", "1. PRESUPUESTOS", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 1, "VENCOTPA01", "1.01 ¿Permite modificar y/o eliminar presupuestos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 1, "VENCOTPA02", "1.02 ¿Permite editar número de presupuestos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 1, "VENCOTPA03", "1.03 ¿Permite editar fecha de presupuestos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 1, "VENCOTPA04", "1.04 ¿Permite editar precio en renglones de presupuestos?...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 1, "VENCOTPA05", "1.05 ¿Permite editar descuento artículo en renglones de presupuestos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 1, "VENCOTPA06", "1.06 ¿Permite editar descuento cliente en renglones de presupuestos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 1, "VENCOTPA07", "1.07 ¿Bloquear impresión de presupuesto después de imprimirlo?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 1, "VENCOTPA08", "1.08 ¿Permite imprimir presupuesto?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 1, "VENCOTPA09", "1.09 ¿Permite escoger asesor comercial en presupuestos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 1, "VENCOTPA10", "1.10 ¿Valida mercancías y clientes en proveedor exclusivo?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 1, "VENCOTPA11", "1.11 TABLA DE COMENTARIOS EN PRESUPUESTO", TipoParametro.iTabla, "COMENTARIOS", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 1, "VENCOTPA12", "1.12 Número de MESES (que retrocede desde hoy) para ver presupuestos en el módulo ", TipoParametro.iEntero, "3", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 2, "VENPPEPA00", "2. PRE-PEDIDOS", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 2, "VENPPEPA01", "2.01 ¿Permite modificar y/o eliminar pre-pedidos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 2, "VENPPEPA02", "2.02 ¿Permite editar número de pre-pedidos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 2, "VENPPEPA03", "2.03 ¿Permite editar fecha de pre-pedidos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 2, "VENPPEPA04", "2.04 ¿Permite editar precio en renglones de pre-pedidos?...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 2, "VENPPEPA05", "2.05 ¿Permite editar descuento artículo en renglones de pre-pedidos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 2, "VENPPEPA06", "2.06 ¿Permite editar descuento cliente en renglones de pre-pedidos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 2, "VENPPEPA07", "2.07 ¿Bloquear impresión de pre-pedido después de imprimirlo?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 2, "VENPPEPA08", "2.08 ¿Permite imprimir pre-pedido?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 2, "VENPPEPA09", "2.09 ¿Permite escoger asesor comercial en pre-pedidos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 2, "VENPPEPA10", "2.10 ¿Valida mercancías y clientes en proveedor exclusivo?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 2, "VENPPEPA11", "2.11 ¿Valida rutas, clientes por ruta y día de visita en los pre-pedidos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 2, "VENPPEPA12", "2.12 Número de MESES (que retrocede desde hoy) para ver pre-pedidos en el módulo ", TipoParametro.iEntero, "3", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 3, "VENPEDPA00", "3. PEDIDOS", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 3, "VENPEDPA01", "3.01 ¿Permite modificar y/o eliminar pedidos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 3, "VENPEDPA02", "3.02 ¿Permite editar número de pedidos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 3, "VENPEDPA03", "3.03 ¿Permite editar fecha de pedidos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 3, "VENPEDPA04", "3.04 ¿Permite editar precio en renglones de pedidos?...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 3, "VENPEDPA05", "3.05 ¿Permite editar descuento artículo en renglones de pedidos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 3, "VENPEDPA06", "3.06 ¿Permite editar descuento cliente en renglones de pedidos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 3, "VENPEDPA07", "3.07 ¿Bloquear impresión de pedido después de imprimirlo?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 3, "VENPEDPA08", "3.08 ¿Permite imprimir pedido?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 3, "VENPEDPA09", "3.09 ¿Permite escoger asesor comercial en pedidos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 3, "VENPEDPA10", "3.10 ¿Valida mercancías y clientes en proveedor exclusivo?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 3, "VENPEDPA11", "3.11 ¿Valida rutas, clientes por ruta y día de visita en los pedidos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 3, "VENPEDPA12", "3.12 Número de MESES (que retrocede desde hoy) para ver pedidos en el módulo ", TipoParametro.iEntero, "3", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 4, "VENNOTPA00", "4. NOTAS DE ENTREGA", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 4, "VENNOTPA01", "4.01 ¿Permite modificar y/o eliminar nota de entrega?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 4, "VENNOTPA02", "4.02 ¿Permite editar número de notas de entrega?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 4, "VENNOTPA03", "4.03 ¿Permite editar fecha de notas de entrega?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 4, "VENNOTPA04", "4.04 ¿Permite editar precio en renglones de notas de entrega?...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 4, "VENNOTPA05", "4.05 ¿Permite editar descuento artículo en renglones de notas de entrega?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 4, "VENNOTPA06", "4.06 ¿Permite editar descuento cliente en renglones de notas de entrega?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 4, "VENNOTPA07", "4.07 ¿Bloquear impresión de nota de entrega después de imprimirla?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 4, "VENNOTPA08", "4.08 ¿Permite imprimir nota de entrega?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 4, "VENNOTPA09", "4.09 ¿Permite escoger asesor comercial en notas de entrega?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 4, "VENNOTPA10", "4.10 ¿Valida mercancías y clientes en proveedor exclusivo?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 4, "VENNOTPA11", "4.11 ¿Valida rutas, clientes por ruta y día de visita en los pedidos?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 4, "VENNOTPA12", "4.12 ¿Incluir numero de lote obligatorio en los renglones de la nota de entrega?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 4, "VENNOTPA13", "4.13 ¿Validar existencias de mercancías por almacén en los renglones de la nota de entrega?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 4, "VENNOTPA14", "4.14 Número de MESES (que retrocede desde hoy) para ver NOTAS DE ENTREGA en el módulo ", TipoParametro.iEntero, "3", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA00", "5. FACTURAS", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA01", "5.01 ¿Permite modificar y/o eliminar facturas?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA02", "5.02 ¿Permite editar número de factura?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA03", "5.03 ¿Permite editar fecha de factura?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA04", "5.04 ¿Permite editar precio en renglones de facturas?...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA05", "5.05 ¿Permite editar descuento artículo en renglones de facturas?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA06", "5.06 ¿Permite editar descuento cliente en renglones de facturas?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA07", "5.07 ¿Bloquear impresión de factura después de imprimirla?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA08", "5.08 ¿Permite imprimir factura?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA09", "5.09 ¿Permite escoger asesor comercial en factura?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA10", "5.10 ¿Valida mercancías y clientes en proveedor exclusivo?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA11", "5.11 ¿Incluir numero de lote obligatorio en los renglones de la factura?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA12", "5.12 ¿Validar existencias de mercancías por almacén en los renglones de factura?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA13", "5.13 Formato de papel en impresora...", TipoParametro.iFormatoPapel, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA14", "5.14 Modo de impresora...", TipoParametro.iModoImpresora, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA15", "5.15 Comentario en factura (hasta 250c)", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA28", "5.16 ¿Imprimir mercancías de COMBO cada una por separado?  ", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA16", "5.17 Número de líneas en factura en impresoras matriciales...", TipoParametro.iEntero, "39", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA17", "5.18 Número máximo de items por factura... ", TipoParametro.iEntero, "8", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA18", "5.19 Número de líneas para comienzo de factura en papel (margen superior)", TipoParametro.iEntero, "8", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA22", "5.20 Número de líneas para finalizar factura en papel (margen inferior)", TipoParametro.iEntero, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA19", "5.21 ¿Imprimir adicionalmente total de factura en moneda alterna?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA20", "5.22 Número de copias por factura", TipoParametro.iEntero, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA21", "5.23 ¿Permite facturar A CONTADO?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA23", "5.24 ¿Incluye precio de Venta Justo (guía cliente) en la factura?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA24", "5.25 ¿Incluye FLETES AUTOMATICOS POR REGION en la factura?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA25", "5.26 Indique codigo  de servicio para FLETES AUTOMATICOS POR REGION ", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA26", "5.27 FECHA VENCIMIENTO factura calculada con base en días hábiles ", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA27", "5.28 VALIDACION DE CUOTAS FIJAS en la facturación, para cada ASESOR COMERCIAL? ", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA29", "5.29 En IMPRESION de facturas colocar CONTADO y dejar las FACTURAS con sus días de CREDITO ? ", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 5, "VENFACPA30", "5.30 Número de MESES (que retrocede desde hoy) para ver FACTURAS en el módulo ", TipoParametro.iEntero, "3", Empresa)


            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA00", "6. NOTAS DE CREDITO", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA01", "6.01 ¿Permite modificar y/o eliminar notas de crédito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA02", "6.02 ¿Permite editar número de nota de crédito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA03", "6.03 ¿Permite editar fecha de nota de crédito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA04", "6.04 ¿Permite editar precio en renglones de nota de crédito?...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA05", "6.05 ¿Permite editar porcentaje de aceptación del valor de la mercancía en renglones de notas de crédito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA06", "6.06 ¿Indicar causa de devolución obligatoria en los renglones de la nota de crédito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA07", "6.07 ¿Bloquear impresión de nota de crédito después de imprimirla?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA08", "6.08 ¿Permite imprimir nota de crédito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA09", "6.09 ¿Permite escoger asesor comercial en nota de crédito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA10", "6.10 ¿Valida mercancías y clientes en proveedor exclusivo?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA11", "6.11 ¿Incluir numero de lote obligatorio en los renglones de la nota de crédito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA12", "6.12 ¿Validar existencias de mercancías por almacén en los renglones de la nota de crédito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA13", "6.13 Formato de papel en impresora...", TipoParametro.iFormatoPapel, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA14", "6.14 Modo de impresora...", TipoParametro.iModoImpresora, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA15", "6.15 Comentario en nota de crédito (hasta 250c)", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA16", "6.16 Número de líneas en nota de crédito en impresoras matriciales...", TipoParametro.iEntero, "39", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA17", "6.17 Número máximo de items por nota de crédito... ", TipoParametro.iEntero, "8", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA18", "6.18 Número de líneas para comienzo de nota de crédito en papel (margen superior)", TipoParametro.iEntero, "8", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA22", "6.19 Número de líneas para finalizar nota de crédito en papel (margen inferior)", TipoParametro.iEntero, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA19", "6.20 ¿Imprimir adicionalmente total de nota de crédito en moneda alterna?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA20", "6.21 Número de copias por Nota de Crédito", TipoParametro.iEntero, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA21", "6.22 ¿Valida N° de factura afectada por devolución?", TipoParametro.iNoSi, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA23", "6.23 ¿Valida N° de CONTROL de Factura afectada en Devolución ?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA24", "6.24 Cantidad de dígitos del Número de Control de Factura afectada ", TipoParametro.iEntero, "8", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 6, "VENNCRPA25", "6.25 Número de MESES (que retrocede desde hoy) para ver NOTAS CREDITO en el módulo ", TipoParametro.iEntero, "3", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA00", "7. NOTAS DE DEBITO", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA01", "7.01 ¿Permite modificar y/o eliminar notas de débito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA02", "7.02 ¿Permite editar número de nota de débito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA03", "7.03 ¿Permite editar fecha de nota de débito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA04", "7.04 ¿Permite editar precio en renglones de nota de débito?...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA05", "7.05 ¿Permite editar porcentaje de descuento artículo en renglones de notas de débito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA06", "7.06 ¿Permite editar porcentaje de descuento cliente en renglones de notas de débito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA07", "7.07 ¿Indicar causa debitación obligatoria en los renglones de la nota de débito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA08", "7.08 ¿Indicar número de factura obligatorio en la nota de débito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA09", "7.09 ¿Bloquear impresión de nota de débito después de imprimirla?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA10", "7.10 ¿Permite imprimir nota de débito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA11", "7.11 ¿Permite escoger asesor comercial en nota de débito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA12", "7.12 ¿Valida mercancías y clientes en proveedor exclusivo?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA13", "7.13 ¿Incluir número de lote obligatorio en los renglones de la nota de débito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA14", "7.14 ¿Validar existencias de mercancías por almacén en los renglones de la nota de débito?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA15", "7.15 Formato de papel en impresora...", TipoParametro.iFormatoPapel, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA16", "7.16 Modo de impresora...", TipoParametro.iModoImpresora, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA17", "7.17 Comentario en nota de débito (hasta 250c)", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA18", "7.18 Número de líneas en nota de débito en impresoras matriciales...", TipoParametro.iEntero, "39", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA19", "7.19 Número máximo de items por nota de débito... ", TipoParametro.iEntero, "8", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA20", "7.20 Número de líneas para comienzo de nota de débito en papel (margen superior)", TipoParametro.iEntero, "8", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA23", "7.21 Número de líneas para pie de nota de débito en papel (margen inferior)", TipoParametro.iEntero, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA21", "7.22 ¿Imprimir adicionalmente total de nota de débito en moneda alterna?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA22", "7.23 Número de copias por Nota de Débito", TipoParametro.iEntero, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 7, "VENNDBPA24", "7.24 Número de MESES (que retrocede desde hoy) para ver NOTAS DEBITO en el módulo ", TipoParametro.iEntero, "3", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENCXCPA00", "8. CUENTAS POR COBRAR (CXC)", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENCXCPA01", "8.01 ¿Permite escoger asesor comercial en movimientos de CXC?", TipoParametro.iNoSi, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENCXCPA02", "8.02 ¿Desea que la casilla de asesor contenga uno por defecto?", TipoParametro.iNoSi, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENPARAM07", "8.03 ¿Al cancelar facturas pendientes desbloquear cliente si este posee cheques devueltos sin cancelar?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENPARAM09", "8.04 ¿Valida tickets en cancelaciones de cheques de alimentación?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENPARAM10", "8.05 ¿Puede ser el monto de cancelaciones mayor al monto dado por documentos seleccionados?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENPARAM11", "8.06 Indique número de caja por defecto para cancelar CXC con cheques de alimentación ...", TipoParametro.iCadena, "00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENPARAM12", "8.07 Indique número de caja por defecto para cancelar CXC en efectivo..", TipoParametro.iCadena, "00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENPARAM13", "8.08 Indique número de caja por defecto para cancelar CXC con cheques ...", TipoParametro.iCadena, "00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENPARAM14", "8.09 Indique número de caja por defecto para cancelar CXC con tarjetas de débito y/o crédito ...", TipoParametro.iCadena, "00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENPARAM15", "8.10 Porcentaje retención de Impuesto al Valor Agregado (IVA) ...", TipoParametro.iDoble, "0.00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENPARAM17", "8.11 ¿Permite cancelar con cheques fechados a futuro (Post Date)?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENPARAM18", "8.12 Cantidad de días de post date para cheques fechados a futuro", TipoParametro.iEntero, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENPARAM19", "8.13 ¿Permite facturar si el cliente posee cheques fechados a futuro? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENPARAM20", "8.14 Porcentaje de interés simple ...", TipoParametro.iDoble, "0.00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENPARAM21", "8.15 Porcentaje de interés compuesto ...", TipoParametro.iDoble, "0.00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENPARAM22", "8.16 Porcentaje de interés de mora ...", TipoParametro.iDoble, "0.00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENPARAM23", "8.17 Calcula cuotas de giros y/o reconversión de deuda en base al interes compuesto...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENPARAM25", "8.18 ¿Bloquear incluir Notas de Crédito cuyo origen es CXC?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENCXCPA03", "8.19 DESCUENTOS Pronto Pago 1 (desde,hasta,porcentaje) ", TipoParametro.iVector, "0,0,0.00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENCXCPA04", "8.20 DESCUENTOS Pronto Pago 2 (desde,hasta,porcentaje) ", TipoParametro.iVector, "0,0,0.00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENCXCPA05", "8.21 DESCUENTOS Pronto Pago 3 (desde,hasta,porcentaje) ", TipoParametro.iVector, "0,0,0.00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 8, "VENCXCPA06", "8.22 DESCUENTOS Pronto Pago 4 (desde,hasta,porcentaje) ", TipoParametro.iVector, "0,0,0.00", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 9, "VENFVEPA00", "9. FUERZA DE VENTA ", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 9, "VENFVEPA01", "9.01 ¿Proceso de Estadísticas de ventas (día anterior) automático?", TipoParametro.iNoSi, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 9, "VENFVEPA02", "9.02 ¿Bloquea facturación de mercancía una vez alcanzada la cuota por asesor comercial?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 9, "VENFVEPA03", "9.03 Porcentaje de comisión sobre el costo de las ventas (Asesores Comerciales) ", TipoParametro.iDoble, "0.00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 9, "VENFVEPA04", "9.04 Porcentaje de comisión sobre el costo de las ventas (Supervisores)", TipoParametro.iDoble, "0.00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 9, "VENFVEPA05", "9.05 Porcentaje de comisión sobre el costo de las ventas (Gerencia de ventas)", TipoParametro.iDoble, "0.00", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iVentas, 9, "VENFVEPA06", "9.06 Proceso automático de cierre de rutas para Datum Mobile ", TipoParametro.iNoSi, "1", Empresa)


            '6 // MERCANCIAS
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERPARAM00", "0. GENERAL", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERPARAM01", "0.01 ¿Indicar jerarquía obligatoriamente? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERPARAM02", "0.02 Número de meses para mostrar movimientos de inventario ...", TipoParametro.iEntero, "6", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERPARAM03", "0.03 ¿Indicar MARCA obligatoriamente? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERPARAM04", "0.04 ¿Indicar CATEGORIA obligatoriamente? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERPARAM05", "0.05 ¿Indicar DIVISION obligatoriamente? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERPARAM06", "0.06 ¿Indicar PESO de la UNIDAD DE VENTA obligatoriamente? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERPARAM07", "0.07 UNIDAD DE MEDIDA adicional a KGR para esta empresa ...", TipoParametro.iCadena, "CAJ", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERPARAM08", "0.08 ¿Valida UNIDAD DE MEDIDA ADICIONAL en catálogo de mercancías? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERPARAM09", "0.09 ¿Poder facturar COMBOS sin existencias? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERPARAM10", "0.10 Moneda por defecto ...", TipoParametro.iMoneda, "0", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERTRAPA00", "1. TRANSFERENCIAS", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERTRAPA01", "1.01 ¿Validar existencias de almacén de salida? ...", TipoParametro.iNoSi, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERTRAPA02", "1.02 ¿Poder Editar/Eliminar Transferencias entre almacenes? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERTRAPA03", "1.03 ¿Validar causa en el autoconsumo y/o desincorporación de almacenes? ...", TipoParametro.iNoSi, "0", Empresa)


            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERSERPA00", "2. SERVICIOS", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERSERPA01", "2.01 ¿Indicar Cuenta Contable obligatoriamente? ...", TipoParametro.iNoSi, "1", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERENVPA00", "3. ENVASES", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERENVPA01", "3.01 ¿Indicar Serial Interno OBLIGATORIAMENTE ? ..", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERENVPA02", "3.02 ¿Indicar Serial Externo OBLIGATORIAMENTE ? ..", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iMercancías, 0, "MERENVPA03", "3.03 ¿Indicar material OBLIGATORIAMENTE ? ..", TipoParametro.iNoSi, "1", Empresa)



            '7 // CONTROL DE GESTIONES
            InsertarParametro(Myconn, lblInfo, Gestion.iControlDeGestiones, 0, "CONTROLPA00", "0. GENERAL", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iControlDeGestiones, 0, "CONTROLPA01", "0.01 ¿Coloca nombre de la empresa en los reportes? ...", TipoParametro.iNoSi, "0", Empresa)

            '8 // PUNTOS DE VENTA

            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM39", "0. GENERAL", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM18", "0.01 Cantidad de meses para vencimientos de apartados", TipoParametro.iEntero, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM20", "0.02 Almacén de movimientos para el punto de venta ...", TipoParametro.iCadena, "00001", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM47", "0.03 Tarifas de mercancías usadas en puntos de venta ...", TipoParametro.iVector, "B", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM21", "0.04 ¿Validar existencias de mercancías por almacén? ", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM22", "0.05 Longitud de código de barras de mercancía por peso ", TipoParametro.iEntero, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM23", "0.06 Longitud de código de barras de precio (comienza al final del código de barras de la mercancia por peso)", TipoParametro.iEntero, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM24", "0.07 ¿ACTUALIZAR INVENTARIO una vez cerrada la factura? ", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM25", "0.08 ¿ver FACTURAS EN ESPERA en todas las cajas? ", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM32", "0.09 Para hacer recambios de mercancías, la DEVOLUCIÓN es igual a EL RECAMBIO?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM33", "0.10 ¿VALIDACION DE REGULACION DE VENTAS EN MERCANCIAS?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM34", "0.11 ¿DESEA USAR OFERTA SOBRE LA UNIDAD DE VENTA ADICIONAL DEL NEGOCIO (CAJ)?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM35", "0.12 DEFINA LA TARIFA DE PRECIO A ASIGNAR PARA OFERTA (A B C D E F) ", TipoParametro.iCadena, "D", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM36", "0.13 AL USAR OFERTA SOBRE LA UNIDAD DE VENTA ADICIONAL ¿DESACTIVAR LOS DESCUENTOS POR RENGLON? ", TipoParametro.iNoSi, "1", Empresa) 'ULTIMO
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM37", "0.14 ¿DESEA USAR MENSAJES EN CINTILLO FACTURA?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM38", "0.15 TABLA DE MENSAJES PARA CINTILLO FACTURA", TipoParametro.iTabla, "MENSAJES", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM40", "0.16 Tiempo de presentación de mensajes en cintillo ", TipoParametro.iEntero, "10", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM51", "0.17 Moneda por defecto... ", TipoParametro.iMoneda, "0", Empresa)


            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM00", "1. FACTURAS", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM01", "1.01 ¿Permite seleccionar clientes?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM02", "1.02 ¿Indicar nombre de cliente obligatoriamente?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM03", "1.03 ¿Indicar número de Registro de Información Fiscal (RIF, Empresa) obligatoriamente?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM04", "1.04 ¿Indicar número de teléfono del cliente obligatoriamente?...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM05", "1.05 ¿Permite reimpresión de factura?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM06", "1.06 ¿Permite cheques como forma de pago?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM07", "1.07 ¿Permite tarjetas de crédito/débito como forma de pago?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM08", "1.08 ¿Permite cheques de alimentación como forma de pago?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM09", "1.09 ¿Permite depósitos en bancos como forma de pago?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM10", "1.10 Redondear a cero (0) totales de factura", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM11", "1.11 Tarifa de precios para facturación en puntos de venta...", TipoParametro.iCadena, "A", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM12", "1.12 ¿Permite editar precio en renglones de factura?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM48", "1.13 ¿Permite DESCUENTOS por CLIENTE? ...", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM13", "1.14 ¿Imprimir total de factura adicionalmente en moneda alterna?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM14", "1.15 Longitud de código de barras de mercancía", TipoParametro.iEntero, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM15", "1.16 Longitud de código de barras de talla (comienza al final del código de barras)", TipoParametro.iEntero, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM16", "1.17 Longitud de código de barras de color (comienza al final del código de barras de talla)", TipoParametro.iEntero, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM17", "1.18 Formato papel impresora ...", TipoParametro.iFormatoPapel, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM26", "1.19 Modo de impresora...", TipoParametro.iModoImpresora, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM27", "1.20 Número de líneas de factura en impresoras matriciales...", TipoParametro.iEntero, "39", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM28", "1.21 Número máximo de items por factura ... ", TipoParametro.iEntero, "8", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM29", "1.22 Número de líneas para comienzo de factura en papel (margen superior)", TipoParametro.iEntero, "8", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM31", "1.23 Número de líneas para pie de factura en papel (margen inferior)", TipoParametro.iEntero, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM30", "1.24 Número de copias por factura", TipoParametro.iEntero, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM19", "1.25 Tipo impresora fiscal...", TipoParametro.iTipoImpresoraFiscal, "0", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM49", "2. DEVOLUCIONES", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM50", "2.01 ¿Validar mercancía en devoluciones contra mercancías de Factura de venta?", TipoParametro.iNoSi, "0", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM41", "3. RETENCIONES IVA", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM42", "3.01 ¿Permite realizar Retenciones de IVA?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM43", "3.02 Número de días límite para aceptación de Retenciones IVA", TipoParametro.iEntero, "15", Empresa)

            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM44", "4. RETIROS DE EFECTIVO", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM45", "4.01 ¿Permite realizar RETIROS en Puntos de Venta para CONTROL DE EFECTIVO?", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iPuntosdeVentas, 0, "POSPARAM46", "4.02 TABLA DENOMINACION DE BILLETES", TipoParametro.iTabla, "BILLETES", Empresa)


            '9 // PRODUCCION
            InsertarParametro(Myconn, lblInfo, Gestion.iProduccion, 0, "PROPARAM00", "0. GENERAL", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iProduccion, 0, "PROPARAM01", "0.01 Almacén de Salida (Materia Prima)", TipoParametro.iCadena, "00001", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iProduccion, 0, "PROPARAM02", "0.02 Almacén de productos terminados", TipoParametro.iCadena, "00001", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iProduccion, 0, "PROPARAM03", "0.03 Indique TIPO del costo para los consumos (0=Promedio 1=Estandar 2=Ultimo)", TipoParametro.iEntero, "2", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iProduccion, 0, "PROPARAM04", "0.04 Indique TIPO del costo para PRODUCTO TERMINADO (0=Promedio 1=Estandar)", TipoParametro.iEntero, "1", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iProduccion, 0, "PROPARAM05", "0.05 Moneda por defecto...", TipoParametro.iMoneda, 0, Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iProduccion, 0, "PROFOR0000", "1. FORMULACIONES", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iProduccion, 0, "PROFOR0001", "1.01 Permite EDITAR formulaciones ", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iProduccion, 0, "PROFOR0002", "1.02 Permite ELIMINAR formulaciones ", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iProduccion, 0, "PROFOR0000", "2. ORDENES DE PRODUCCION", TipoParametro.iCadena, "", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iProduccion, 0, "PROORD0001", "2.01 Permite EDITAR Ordenes de producción ", TipoParametro.iNoSi, "0", Empresa)
            InsertarParametro(Myconn, lblInfo, Gestion.iProduccion, 0, "PROORD0002", "2.02 Permite ELIMINAR Ordenes de producción ", TipoParametro.iNoSi, "0", Empresa)

        End If
    End Sub

    Public Sub IniciarContadores(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, Optional Empresa As String = "")

        If Empresa = "" Then Empresa = jytsistema.WorkID

        If ExisteTabla(Myconn, jytsistema.WorkDataBase, "jsconparametros") Then

            '///////// CONTABILIDAD
            InsertarContador(Myconn, lblInfo, Gestion.iContabilidad, "0", "CONTABILIDAD", "0. Contabilidad ", "", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iContabilidad, "01", "CONNUMASI", "0.01. Número de asiento contable ...", "0000000001", Empresa)

            ' //////// BANCOS
            InsertarContador(Myconn, lblInfo, Gestion.iBancos, "0", "BANCOS", "0. Bancos y Cajas ", "", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iBancos, "01", "BANNUMTRA", "0.01. Número de transferencias entre cuentas bancarias...", "TB00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iBancos, "02", "BANNUMAEF", "0.02. Avance de efectivo ...", "AF00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iBancos, "03", "BANNUMRCC", "0.03. Número de comprobante de pago ...", "CP00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iBancos, "04", "BANNUMDEP", "0.04. Número de depósito temporal ...", "DT00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iBancos, "05", "BANNUMTRACAJ", "0.05. Número MOVIMIENTO DE CAJA ...", "CJ0000000000001", Empresa)


            '//////// RECURSOS HUMANOS
            InsertarContador(Myconn, lblInfo, Gestion.iRecursosHumanos, "0", "NOMINA", "0. Recursos Humanos", "", Empresa)

            '//////// COMPRAS Y CXP
            InsertarContador(Myconn, lblInfo, Gestion.iCompras, "0", "COMPRAS", "0. Compras y cuentas por pagar", "", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iCompras, "01", "COMNUMPRO", "0.01. Número de Proveedor ...", "0000000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iCompras, "02", "COMNUMPRS", "0.02. Número de Proveedor de Servicios ...", "PS00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iCompras, "03", "COMNUMMOV", "0.03. Número de movimiento de cuentas por pagar ...", "FCP0000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iCompras, "04", "COMNUMGAS", "0.04. Número de gasto ...", "GS00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iCompras, "05", "COMNUMORD", "0.05. Número de orden de compras ...", "OC00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iCompras, "06", "COMNUMREC", "0.06. Número de recepción de compra ...", "RC00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iCompras, "07", "COMNUMCOM", "0.07. Número de compra ...", "0000000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iCompras, "08", "COMNUMNCC", "0.08. Número de nota de crédito compras ...", "NC00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iCompras, "09", "COMNUMNDC", "0.09. Número de nota débito compras ...", "ND00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iCompras, "10", "COMNUMCAN", "0.10. Número de comprobante de cancelación/abono de cuentas por pagar ...", "0000000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iCompras, "11", "COMNUMCCR", "0.11. Número de nota crédito de cuentas por pagar ...", "NCC0000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iCompras, "12", "COMNUMCDB", "0.12. Número de nota débito de cuentas por pagar ...", "NDC0000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iCompras, "13", "COMNUMCHD", "0.13. Número de nota débito por cheque devuelto  ...", "CDC0000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iCompras, "14", "COMNUMRETIVA", "0.14. Número de retención Impuesto al Valor Agregado (IVA) ...", "00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iCompras, "15", "COMNUMRETISLR", "0.15. Número de retencion Impuesto Sobre La Renta (ISLR) ...", "00000001", Empresa)


            '//////// VENTAS Y CXC
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "0", "VENTAS", "0. Ventas y Cuentas por Cobrar ", "", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "01", "VENNUMCLI", "0.01. Número de cliente ...", "0000000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "02", "VENNUMMOV", "0.02. Número de movimiento de cuentas por cobrar ...", "MV00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "03", "VENNUMCOT", "0.03. Número de presupuesto de venta ...", "CO00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "04", "VENNUMPPE", "0.04. Número de pre-pedido de mercancías ...", "PP00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "05", "VENNUMPED", "0.05. Número de pedido de mercancías ...", "PE00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "06", "VENNUMAPT", "0.06. Número de apartado de mercancías ...", "AP00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "07", "VENNUMNOT", "0.07. Número de nota de entrega ...", "NE00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "08", "VENNUMFAC", "0.08. Número de factura ...", "FC00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "09", "VENNUMNCR", "0.09. Número de nota de crédito ventas ...", "NC00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "10", "VENNUMNDB", "0.10. Número de nota débito ventas ...", "ND00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "11", "VENNUMCAN", "0.11. Número de cancelación/abono de cuentas por cobrar ...", "CA00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "12", "VENNUMVCR", "0.12. Número de nota de crédito de cuentas por cobrar ...", "NCV0000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "13", "VENNUMVDB", "0.13. Número de nota débito de cuentas por cobrar ...", "NDV0000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "14", "VENNUMCHD", "0.14. Número de nota débito por Cheque devuelto ...", "CD00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "15", "VENNUMGUI", "0.15. Número de guía de carga ...", "0000000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "16", "VENNUMREL", "0.16. Número de relación de facturas ...", "0000000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "17", "VENNUMCONTROL", "0.17. Número de Control para Notas Entrega, Facturas, Notas Crédito y Notas Débito en Ventas  ...", "00-0000000", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iVentas, "18", "VENNUMGUIPED", "0.18. Número de guía DE PEDIDOS ...", "0000000001", Empresa)

            '//////// PUNTOS DE VENTAS
            InsertarContador(Myconn, lblInfo, Gestion.iPuntosdeVentas, "0", "POS", "0. Puntos de Ventas", "", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iPuntosdeVentas, "01", "POSNUMFAC", "0.01. Número de factura de puntos de venta ...", "PV00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iPuntosdeVentas, "02", "POSNUMNCR", "0.02. Número de notas de crédito de puntos de venta ...", "NDV0000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iPuntosdeVentas, "03", "POSNUMPED", "0.03. Número de pedido y/o apartado de puntos de venta ...", "PEV0000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iPuntosdeVentas, "04", "POSNUMREC", "0.04. Número de recambio de puntos de venta ...", "RC00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iPuntosdeVentas, "05", "POSNUMRET", "0.05. Número de Retiro Efectivo de CAJA ...", "RT0000000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iPuntosdeVentas, "06", "POSNUMTMP", "0.06. Número Factura POS Temporal (NO MODIFICAR) ...", "DT0000000001", Empresa)


            '/////// MERCANCIAS
            InsertarContador(Myconn, lblInfo, Gestion.iMercancías, "0", "MERCANCIAS", "0. Mercancías", "", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iMercancías, "01", "INVNUMTRA", "0.01. Número de transferencia y/o número consumo interno de mercancías ...", "TR00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iMercancías, "02", "INVNUMMOV", "0.02. Número de movimiento de inventario ...", "IN00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iMercancías, "03", "INVNUMCON", "0.03. Número de conteo de inventario ...", "0000000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iMercancías, "04", "INVNUMPAL", "0.04. Número Pedido de Inventario ...", "0000000001", Empresa)

            InsertarContador(Myconn, lblInfo, Gestion.iMercancías, "05", "INVNUMMOVENV", "0.05. Número de movimiento de envase ...", "0000000001", Empresa)

            '/////// PRODUCCION
            InsertarContador(Myconn, lblInfo, Gestion.iProduccion, "0", "PRODUCCION", "0. Producción", "", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iProduccion, "01", "PROORDPRO", "0.01. Número de Orden de Producción ...", "OP00000001", Empresa)
            InsertarContador(Myconn, lblInfo, Gestion.iProduccion, "02", "PROENTPRO", "0.02. Número de Entrada de Producción ...", "EP00000001", Empresa)

        End If

    End Sub
    Public Function ContadorNC_Financiera(ByVal myconn As MySqlConnection, ByVal CodigoCausaNC As String,
                             ByVal OrigenCausaNC As String, Optional ByVal Incrementar As Boolean = True) As String

        ContadorNC_Financiera = ft.DevuelveScalarCadena(myconn, " SELECT NUMPAG FROM jsconcausas_notascredito WHERE CODIGO = '" & CodigoCausaNC & "' AND ORIGEN = '" & OrigenCausaNC & "' ").ToString

        If Incrementar Then
            Dim newContador As String = IncrementarCadena(ContadorNC_Financiera)
            ft.Ejecutar_strSQL(myconn, " UPDATE jsconcausas_notascredito SET NUMPAG = '" & newContador & "' WHERE CODIGO = '" & CodigoCausaNC & "' AND ORIGEN = '" & OrigenCausaNC & "' ")
        End If

    End Function

    Public Function Contador(ByVal myconn As MySqlConnection, ByVal lblInfo As Label, ByVal gestion As Integer, ByVal CodigoContador As String,
                             ByVal Modulo As String, Optional ByVal Incrementar As Boolean = True) As String

        Dim afld() As String = {"gestion", "codigo", "id_emp"}
        Dim afldNme() As String = {gestion, CodigoContador, jytsistema.WorkID}
        Contador = qFoundAndSign(myconn, lblInfo, "jsconcontadores", afld, afldNme, "contador")
        If Incrementar Then
            Dim newContador As String = IncrementarCadena(Contador)
            InsertEditCONTROLContador(myconn, lblInfo, False, gestion, Modulo, CodigoContador, ":-)", newContador)
        End If

    End Function
    Public Function ContadorJytsuite(ByVal Myconn As MySqlConnection, ByVal lblInfo As Label, ByVal CodigoContador As String,
                                      Optional ByVal Incrementar As Boolean = True) As String

        ContadorJytsuite = ft.DevuelveScalarCadena(Myconn, " select " & CodigoContador & " from jsconctacon where id_emp = '" & jytsistema.WorkID & "'  ")
        If ContadorJytsuite = "0" Then ContadorJytsuite = "0000000001"

        If Incrementar Then
            Dim Incremento As String = IncrementarCadena(ContadorJytsuite)
            ft.Ejecutar_strSQL(Myconn, " update jsconctacon set  " & CodigoContador & " = '" & Incremento & "' where id_emp = '" & jytsistema.WorkID & "' ")
        End If

        ContadorJytsuite = ft.DevuelveScalarCadena(Myconn, " select " & CodigoContador & " from jsconctacon where id_emp = '" & jytsistema.WorkID & "'  ")
        If ContadorJytsuite = "0" Then ContadorJytsuite = "0000000001"

    End Function

    Public Function ParametroPlus(ByVal myconn As MySqlConnection, ByVal gestion As Integer, ByVal CodigoParametro As String) As String
        Return ft.DevuelveScalarCadena(myconn, " select valor from jsconparametros where gestion = " & gestion & " and codigo = '" & CodigoParametro & "' and id_emp = '" & jytsistema.WorkID & "'  ")
    End Function

    Public Sub ColocaCorredoresCTEnCombo(ByVal MyConn As MySqlConnection, ByVal cmb As ComboBox, ByVal lblInfo As Label)

        Dim ds As New DataSet
        Dim eCont As Integer
        Dim nTablaB As String = "tCorredor"
        Dim str As String = ""

        ds = DataSetRequery(ds, " select * from jsvencestic where id_emp = '" & jytsistema.WorkID & "' order by codigo ", MyConn, nTablaB, lblInfo)

        With ds.Tables(nTablaB)
            Dim aCorredor(.Rows.Count - 1) As String
            For eCont = 0 To .Rows.Count - 1
                aCorredor(eCont) = .Rows(eCont).Item("codigo") & " | " &
                    .Rows(eCont).Item("descrip")
            Next
            ft.RellenaCombo(aCorredor, cmb)
        End With

        ds.Tables(nTablaB).Dispose()
        ds.Dispose()
        ds = Nothing

    End Sub

    Function ChequesDevueltosPermitidos(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label) As Integer
        Return ft.DevuelveScalarEntero(MyConn, " select valor from jsconparametros where gestion = " & Gestion.iVentas & " and " _
                                     & " codigo = 'VENPARAM03' AND id_emp = '" & jytsistema.WorkID & "' ")

    End Function


    Public Function CambioActual(MyConn As MySqlConnection, Optional fecha As DateTime = Nothing) As Double

        Return ft.DevuelveScalarDoble(MyConn,
         "SELECT b.equivale FROM (Select MAX(fecha) fecha, moneda " +
            " FROM jsconctacam " +
            " WHERE fecha <=  '" & ft.FormatoFechaHoraMySQL(IIf(fecha = Nothing, jytsistema.sFechadeTrabajo, fecha)) & "' " +
            " GROUP BY moneda) a " +
            " LEFT JOIN jsconctacam b ON (a.fecha = b.fecha AND a.moneda = b.moneda) " +
            " where a.moneda = (select monedacambio from jsconctaemp where id_emp = '" & jytsistema.WorkID & "' ) ")

    End Function


    Public Function GetListaDeMonedasyCambios(MyConn As MySqlConnection, Optional fecha As DateTime = Nothing) As List(Of CambioMonedaPlus)

        Dim strSQL = SQLSelectCambiosYMonedas(IIf(fecha = Nothing, DateTime.Now, fecha))

        Return Lista(Of CambioMonedaPlus)(MyConn, strSQL)

    End Function
    Public Function GetTransportList(MyConn As MySqlConnection) As List(Of SimpleTable)

        Dim strSQL = " select codtra Codigo, concat( codtra, ' | ', nomtra ) Descripcion from jsconctatra where id_emp = '" & jytsistema.WorkID & "' order by codtra "
        Return Lista(Of SimpleTable)(MyConn, strSQL)

    End Function
    Public Function GetWarehouseList(MyConn As MySqlConnection) As List(Of SimpleTable)

        Dim strSQL = " select codalm Codigo, concat(codalm, ' | ', desalm) Descripcion from jsmercatalm where id_emp = '" & jytsistema.WorkID & "' order by codalm "
        Return Lista(Of SimpleTable)(MyConn, strSQL)

    End Function
    Public Function SQLSelectCambiosYMonedas(fecha As DateTime)
        Return " SELECT aa.moneda, concat( bb.UnidadMonetaria,' | ' , bb.Simbolo ) UnidadMonetaria, bb.Simbolo, bb.CodigoIso, " +
        " aa.equivale, aa.fecha FROM ( SELECT b.moneda, b.equivale, b.fecha FROM " +
        " (SELECT MAX(fecha) fecha, moneda FROM jsconctacam " +
        " WHERE fecha <= '" + ft.FormatoFechaHoraMySQL(fecha) + "' " +
        " GROUP BY moneda ) a " +
        " Left Join jsconctacam b ON (a.fecha = b.fecha And a.moneda = b.moneda) " +
        " WHERE " +
        " b.id_emp = '" + jytsistema.WorkID + "' )  aa " +
        " LEFT JOIN jsconcatmon bb on ( aa.moneda = bb.id ) UNION " +
        " SELECT a.moneda, CONCAT( b.UnidadMonetaria,' | ' , b.Simbolo ) UnidadMonetaria, b.Simbolo, b.CodigoIso, 1.0000 equivale, NOW() fecha " +
        " FROM jsconctaemp a LEFT JOIN jsconcatmon b ON (a.moneda = b.id) WHERE id_emp = '" + jytsistema.WorkID + "' "
    End Function


    Public Function InitiateDropDownInterchangeCurrency(MyConn As MySqlConnection, cmbMonedas As SfComboBox, FechaLista As Date,
                                                   Optional defaultValue As Boolean = False,
                                                   Optional displayMember As String = "UnidadMonetaria") As List(Of CambioMonedaPlus)

        Dim lista = GetListaDeMonedasyCambios(MyConn, FechaLista)
        cmbMonedas.DataSource = lista
        cmbMonedas.DisplayMember = displayMember
        cmbMonedas.ValueMember = "Moneda"
        cmbMonedas.Watermark = "Escoja una moneda"
        cmbMonedas.AutoCompleteMode = AutoCompleteMode.Suggest
        cmbMonedas.DropDownStyle = DropDownStyle.DropDownList
        cmbMonedas.AutoCompleteSuggestMode = AutoCompleteSuggestMode.Contains
        cmbMonedas.MaxDropDownItems = 10
        If defaultValue Then cmbMonedas.SelectedValue = jytsistema.WorkCurrency.Id

        Return lista

    End Function


End Module
