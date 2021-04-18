Imports MySql.Data.MySqlClient
Module FuncionesImpresionBematech

    ' Funciones de Inicialización

    Public Declare Function Bematech_FI_CambiaSimboloMoneda Lib "BEMAFI32.DLL" (ByVal SimboloMoneda As String) As Integer
    Public Declare Function Bematech_FI_ProgramaAlicuota Lib "BEMAFI32.DLL" (ByVal Alicuota As String, ByVal ICMS_ISS As Integer) As Integer
    Public Declare Function Bematech_FI_ProgramaHorarioDeVerano Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_CrearDepartamento Lib "BEMAFI32.DLL" (ByVal Indice As Integer, ByVal Departamento As String) As Integer
    Public Declare Function Bematech_FI_CrearTotalizadorSinIcms Lib "BEMAFI32.DLL" (ByVal Indice As Integer, ByVal Totalizador As String) As Integer
    Public Declare Function Bematech_FI_ProgramaRedondeo Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_ProgramaTruncamiento Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_LineasEntreCupones Lib "BEMAFI32.DLL" (ByVal Lineas As Integer) As Integer
    Public Declare Function Bematech_FI_EspacioEntreLineas Lib "BEMAFI32.DLL" (ByVal Dots As Integer) As Integer
    Public Declare Function Bematech_FI_FuerzaImpactoAgujas Lib "BEMAFI32.DLL" (ByVal FuerzaImpacto As Integer) As Integer

    ' Funciones do Cupon Fiscal

    Public Declare Function Bematech_FI_AbreCupon Lib "BEMAFI32.DLL" (ByVal RIF As String) As Integer
    Public Declare Function Bematech_FI_AbreComprobanteDeVenta Lib "BEMAFI32.DLL" (ByVal RIF As String, ByVal Nombre As String) As Integer
    Public Declare Function Bematech_FI_AbreComprobanteDeVentaEx Lib "BEMAFI32.DLL" (ByVal RIF As String, ByVal Nombre As String, ByVal Direccion As String) As Integer
    Public Declare Function Bematech_FI_AbreNotaDeCredito Lib "BEMAFI32.DLL" (ByVal Nombre As String, ByVal NumeroSerie As String, ByVal RIF As String, ByVal Dia As String, ByVal Mes As String, ByVal Ano As String, ByVal Hora As String, ByVal Minuto As String, ByVal Secundo As String, ByVal COO As String) As Integer
    Public Declare Function Bematech_FI_DevolucionArticulo Lib "BEMAFI32.DLL" (ByVal Codigo As String, ByVal Descripcion As String, ByVal Alicuota As String, ByVal TipoCantidad As String, ByVal Cantidad As String, ByVal CasasDecimales As Integer, ByVal ValorUnit As String, ByVal TipoDescuento As String, ByVal ValorDesc As String) As Integer
    Public Declare Function Bematech_FI_VendeArticulo Lib "BEMAFI32.DLL" (ByVal Codigo As String, ByVal Descripcion As String, ByVal Alicuota As String, ByVal TipoCantidad As String, ByVal Cantidad As String, ByVal CasasDecimales As Integer, ByVal ValorUnitario As String, ByVal TipoDescuento As String, ByVal Descuento As String) As Integer
    Public Declare Function Bematech_FI_AnulaArticuloAnterior Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_AnulaArticuloGenerico Lib "BEMAFI32.DLL" (ByVal NumeroArticulo As String) As Integer
    Public Declare Function Bematech_FI_AnulaCupon Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_CierraCuponReducido Lib "BEMAFI32.DLL" (ByVal Formapago As String, ByVal Mensaje As String) As Integer
    Public Declare Function Bematech_FI_CierraCupon Lib "BEMAFI32.DLL" (ByVal Formapago As String, ByVal DescuentoIncremento As String, ByVal TipoDescuentoIncremento As String, ByVal ValorIncrementoDescuento As String, ByVal ValorPago As String, ByVal Mensaje As String) As Integer
    Public Declare Function Bematech_FI_VendeArticuloDepartamento Lib "BEMAFI32.DLL" (ByVal Codigo As String, ByVal Descripcion As String, ByVal Alicuota As String, ByVal ValorUnitario As String, ByVal Cantidad As String, ByVal Acrecimo As String, ByVal Descuento As String, ByVal IndiceDepartamento As String, ByVal UnidadMedida As String) As Integer
    Public Declare Function Bematech_FI_ExtenderDescripcionArticulo Lib "BEMAFI32.DLL" (ByVal Descripcion As String) As Integer
    Public Declare Function Bematech_FI_UsaUnidadMedida Lib "BEMAFI32.DLL" (ByVal UnidadMedida As String) As Integer
    Public Declare Function Bematech_FI_RectificaFormasPago Lib "BEMAFI32.DLL" (ByVal FormaOrigen As String, ByVal FormaDestino As String, ByVal Valor As String) As Integer
    Public Declare Function Bematech_FI_IniciaCierreCupon Lib "BEMAFI32.DLL" (ByVal IncrementoDescuento As String, ByVal TipoIncrementoDescuento As String, ByVal ValorIncrementoDescuento As String) As Integer
    Public Declare Function Bematech_FI_EfectuaFormaPago Lib "BEMAFI32.DLL" (ByVal Formapago As String, ByVal ValorFormaPago As String) As Integer
    Public Declare Function Bematech_FI_EfectuaFormaPagoDescripcionForma Lib "BEMAFI32.DLL" (ByVal Formapago As String, ByVal ValorFormaPago As String, ByVal DescripcionOpcional As String) As Integer
    Public Declare Function Bematech_FI_FinalizarCierreCupon Lib "BEMAFI32.DLL" (ByVal Mensaje As String) As Integer

    ' Funciones de los Informes Fiscales

    Public Declare Function Bematech_FI_LecturaX Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_LecturaXSerial Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_ReduccionZ Lib "BEMAFI32.DLL" (ByVal Fecha As String, ByVal Hora As String) As Integer
    Public Declare Function Bematech_FI_InformeGerencial Lib "BEMAFI32.DLL" (ByVal cTexto As String) As Integer
    Public Declare Function Bematech_FI_InformeGerencialTEF Lib "BEMAFI32.DLL" (ByVal cTexto As String) As Integer
    Public Declare Function Bematech_FI_CierraInformeGerencial Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_LecturaMemoriaFiscalFecha Lib "BEMAFI32.DLL" (ByVal cFechaInicial As String, ByVal cFechaFinal As String) As Integer
    Public Declare Function Bematech_FI_LecturaMemoriaFiscalReduccion Lib "BEMAFI32.DLL" (ByVal cReduccionInicial As String, ByVal cReduccionFinal As String) As Integer
    Public Declare Function Bematech_FI_LecturaMemoriaFiscalSerialFecha Lib "BEMAFI32.DLL" (ByVal cFechaInicial As String, ByVal cFechaFinal As String) As Integer
    Public Declare Function Bematech_FI_LecturaMemoriaFiscalSerialReduccion Lib "BEMAFI32.DLL" (ByVal cReduccionInicial As String, ByVal cReduccionFinal As String) As Integer

    ' Funciones de las Operaciones No Fiscales

    Public Declare Function Bematech_FI_RecebimientoNoFiscal Lib "BEMAFI32.DLL" (ByVal IndiceTotalizador As String, ByVal Valor As String, ByVal Formapago As String) As Integer
    Public Declare Function Bematech_FI_AbreComprobanteNoFiscalVinculado Lib "BEMAFI32.DLL" (ByVal Formapago As String, ByVal Valor As String, ByVal NumeroCupon As String) As Integer
    Public Declare Function Bematech_FI_ImprimeComprobanteNoFiscalVinculado Lib "BEMAFI32.DLL" (ByVal Texto As String) As Integer
    Public Declare Function Bematech_FI_UsaComprobanteNoFiscalVinculadoTEF Lib "BEMAFI32.DLL" (ByVal Texto As String) As Integer
    Public Declare Function Bematech_FI_CierraComprobanteNoFiscalVinculado Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_Sangria Lib "BEMAFI32.DLL" (ByVal Valor As String) As Integer
    Public Declare Function Bematech_FI_Provision Lib "BEMAFI32.DLL" (ByVal Valor As String, ByVal Formapago As String) As Integer

    ' Funciones de Informaciones de la Impresora

    Public Declare Function Bematech_FI_NumeroSerie Lib "BEMAFI32.DLL" (ByVal NumeroSerie As String) As Integer
    Public Declare Function Bematech_FI_SubTotal Lib "BEMAFI32.DLL" (ByVal SubTotal As String) As Integer
    Public Declare Function Bematech_FI_NumeroCupon Lib "BEMAFI32.DLL" (ByVal NumeroCupon As String) As Integer
    Public Declare Function Bematech_FI_VersionFirmware Lib "BEMAFI32.DLL" (ByVal VersionFirmware As String) As Integer
    Public Declare Function Bematech_FI_CGC_IE Lib "BEMAFI32.DLL" (ByVal CGC As String, ByVal IE As String) As Integer
    Public Declare Function Bematech_FI_GranTotal Lib "BEMAFI32.DLL" (ByVal GranTotal As String) As Integer
    Public Declare Function Bematech_FI_Cancelamientos Lib "BEMAFI32.DLL" (ByVal ValorCancelamientos As String) As Integer
    Public Declare Function Bematech_FI_Descuentos Lib "BEMAFI32.DLL" (ByVal ValorDescuentos As String) As Integer
    Public Declare Function Bematech_FI_NumeroOperacionesNoFiscales Lib "BEMAFI32.DLL" (ByVal NumeroOperaciones As String) As Integer
    Public Declare Function Bematech_FI_NumeroCuponesAnulados Lib "BEMAFI32.DLL" (ByVal NumeroCancelamientos As String) As Integer
    Public Declare Function Bematech_FI_NumeroIntervenciones Lib "BEMAFI32.DLL" (ByVal NumeroIntervenciones As String) As Integer
    Public Declare Function Bematech_FI_NumeroReducciones Lib "BEMAFI32.DLL" (ByVal NumeroReducciones As String) As Integer
    Public Declare Function Bematech_FI_NumeroSustituicionesPropietario Lib "BEMAFI32.DLL" (ByVal NumeroSustituiciones As String) As Integer
    Public Declare Function Bematech_FI_UltimoArticuloVendido Lib "BEMAFI32.DLL" (ByVal NumeroArticulo As String) As Integer
    Public Declare Function Bematech_FI_ClichePropietario Lib "BEMAFI32.DLL" (ByVal Cliche As String) As Integer
    Public Declare Function Bematech_FI_NumeroCaja Lib "BEMAFI32.DLL" (ByVal NumeroCaja As String) As Integer
    Public Declare Function Bematech_FI_NumeroTienda Lib "BEMAFI32.DLL" (ByVal NumeroTienda As String) As Integer
    Public Declare Function Bematech_FI_SimboloMoneda Lib "BEMAFI32.DLL" (ByVal SimboloMoneda As String) As Integer
    Public Declare Function Bematech_FI_MinutosPrendida Lib "BEMAFI32.DLL" (ByVal Minutos As String) As Integer
    Public Declare Function Bematech_FI_MinutosImprimiendo Lib "BEMAFI32.DLL" (ByVal Minutos As String) As Integer
    Public Declare Function Bematech_FI_VerificaModoOperacion Lib "BEMAFI32.DLL" (ByVal Modo As String) As Integer
    Public Declare Function Bematech_FI_VerificaEpromConectada Lib "BEMAFI32.DLL" (ByVal Flag As String) As Integer
    Public Declare Function Bematech_FI_FlagsFiscales Lib "BEMAFI32.DLL" (ByRef Flag As Integer) As Integer
    Public Declare Function Bematech_FI_ValorPagoUltimoCupon Lib "BEMAFI32.DLL" (ByVal ValorCupon As String) As Integer
    Public Declare Function Bematech_FI_FechaHoraImpresora Lib "BEMAFI32.DLL" (ByVal Fecha As String, ByVal Hora As String) As Integer
    Public Declare Function Bematech_FI_ContadoresTotalizadoresNoFiscales Lib "BEMAFI32.DLL" (ByVal Contadores As String) As Integer
    Public Declare Function Bematech_FI_VerificaTotalizadoresNoFiscales Lib "BEMAFI32.DLL" (ByVal Totalizadores As String) As Integer
    Public Declare Function Bematech_FI_FechaHoraReducion Lib "BEMAFI32.DLL" (ByVal Fecha As String, ByVal Hora As String) As Integer
    Public Declare Function Bematech_FI_FechaMovimiento Lib "BEMAFI32.DLL" (ByVal Fecha As String) As Integer
    Public Declare Function Bematech_FI_VerificaTruncamiento Lib "BEMAFI32.DLL" (ByVal Flag As String) As Integer
    Public Declare Function Bematech_FI_Agregado Lib "BEMAFI32.DLL" (ByVal ValorIncrementos As String) As Integer
    Public Declare Function Bematech_FI_ContadorBilletePasaje Lib "BEMAFI32.DLL" (ByVal ContadorPasaje As String) As Integer
    Public Declare Function Bematech_FI_VerificaAlicuotasIss Lib "BEMAFI32.DLL" (ByVal AlicuotasIss As String) As Integer
    Public Declare Function Bematech_FI_VerificaFormasPago Lib "BEMAFI32.DLL" (ByVal Formas As String) As Integer
    Public Declare Function Bematech_FI_VerificaRecebimientoNoFiscal Lib "BEMAFI32.DLL" (ByVal Recebimientos As String) As Integer
    Public Declare Function Bematech_FI_VerificaDepartamentos Lib "BEMAFI32.DLL" (ByVal Departamentos As String) As Integer
    Public Declare Function Bematech_FI_VerificaTipoImpresora Lib "BEMAFI32.DLL" (ByRef TipoImpresora As String) As Integer
    Public Declare Function Bematech_FI_VerificaTotalizadoresParciales Lib "BEMAFI32.DLL" (ByVal cTotalizadores As String) As Integer
    Public Declare Function Bematech_FI_RetornoAlicuotas Lib "BEMAFI32.DLL" (ByVal cAliquotas As String) As Integer
    Public Declare Function Bematech_FI_DatosUltimaReduccion Lib "BEMAFI32.DLL" (ByVal datosreduccion As String) As Integer
    Public Declare Function Bematech_FI_MonitoramentoPapel Lib "BEMAFI32.DLL" (ByRef Lineas As String) As Integer
    Public Declare Function Bematech_FI_ValorFormaPago Lib "BEMAFI32.DLL" (ByVal Formapago As String, ByVal Valor As String) As Integer
    Public Declare Function Bematech_FI_ValorTotalizadorNoFiscal Lib "BEMAFI32.DLL" (ByVal Totalizador As String, ByVal Valor As String) As Integer

    ' Funciones de Autenticación

    Public Declare Function Bematech_FI_Autenticacion Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_ProgramaCaracterAutenticacion Lib "BEMAFI32.DLL" (ByVal Parametros As String) As Integer

    ' Funciones de Gaveta de Dinero

    Public Declare Function Bematech_FI_AccionaGaveta Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_VerificaEstadoGaveta Lib "BEMAFI32.DLL" (ByRef EstadoGaveta As Integer) As Integer

    ' Otras Funciones

    Public Declare Function Bematech_FI_ResetaImpresora Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_AbrePuertaSerial Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_VerificaEstadoImpresora Lib "BEMAFI32.DLL" (ByRef ACK As Integer, ByRef ST1 As Integer, ByRef ST2 As Integer) As Integer
    Public Declare Function Bematech_FI_RetornoImpresora Lib "BEMAFI32.DLL" (ByRef ACK As Integer, ByRef ST1 As Integer, ByRef ST2 As Integer) As Integer
    Public Declare Function Bematech_FI_CierraPuertaSerial Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_VerificaImpresoraPrendida Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_ImprimeConfiguracionesImpresora Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_ImprimeDepartamentos Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_AperturaDelDia Lib "BEMAFI32.DLL" (ByVal Valor As String, ByVal Formapago As String) As Integer
    Public Declare Function Bematech_FI_CierreDelDia Lib "BEMAFI32.DLL" () As Integer
    Public Declare Function Bematech_FI_ImpresionCarne Lib "BEMAFI32.DLL" (ByVal Titulo As String, ByVal Percelas As String, ByVal Fechas As Integer, ByVal Cantidad As Integer, ByVal Texto As String, ByVal Cliente As String, ByVal RG_CPF As String, ByVal Cupon As String, ByVal Vias As Integer, ByVal Firma As Integer) As Integer
    Public Declare Function Bematech_FI_InfoBalanza Lib "BEMAFI32.DLL" (ByVal Puerta As String, ByVal Modelo As Integer, ByVal Peso As String, ByVal PrecioKilo As String, ByVal Total As String) As Integer
    Public Declare Function Bematech_FI_VersionDll Lib "BEMAFI32.DLL" (ByVal Version As String) As Integer
    Public Declare Function Bematech_FI_LeerArchivoRetorno Lib "BEMAFI32.DLL" (ByVal Retorno As String) As Integer

    '   Public Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal LpAplicationName As String, ByVal LpKeyName As Any, ByVal lpDefault As String, ByVal lpReturnString As String, ByVal nSize As Long, ByVal lpFilename As String) As Long
    '  Public Declare Function GetSystemDirectory Lib "kernel32" Alias "GetSystemDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Long) As Long

    Public Retorno As Integer
    Public Funcion As Integer
    Public LocalRetorno As String
    Public ArqRetorno As String

    Private ft As New Transportables

    'Leer los Valores de los parámetros en las secciones del arquivo ini

    'Function LeeParametrosIni(ByVal Secion As String, ByVal Label As String) As String

    '    Dim Const TamanoParametro As Long = 80
    '        Dim ParametroIni As String * TamanoParametro
    '    Dim RetornoFuncion
    '    Dim ArchivoIni As String
    '    Dim Contador As Integer
    '        ParametroIni = ""
    '
    '        RetornoFuncion = GetSystemDirectory(ParametroIni, TamanoParametro)
    '        ArchivoIni = Left(ParametroIni, RetornoFuncion) + "\BemaFI32.ini"
    '        ParametroIni = ""
    '        RetornoFuncion = GetPrivateProfileString(Secion, Label, "-2", ParametroIni, TamanoParametro, ArchivoIni)
    '        RetornoFuncion = Mid(ParametroIni, 1, 2)
    '        If Val(RetornoFuncion) <> -2 Then
    '            Contador = 1
    '            Do
    '                Tst = Mid(ParametroIni, Contador, 1)
    '                If Asc(Tst) <> 0 Then
    '                    Contador = Contador + 1
    '                End If
    '            Loop While ((Asc(Tst) <> 0) And (Contador < Len(ParametroIni)))
    '            RetornoFuncion = Mid(ParametroIni, 1, Contador)
    '            RetornoFuncion = Mid(RetornoFuncion, 1, Len(RetornoFuncion) - 1)
    '
    '        End If
    '        LeeParametrosIni = RetornoFuncion
    '    End Function

    Public Function VerificaRetornoImpresora(ByVal Label As String, ByVal Contenido As String, ByVal Retorno As Integer, ByVal TituloVentana As String) As Boolean
        Dim ACK As Integer
        Dim ST1 As Integer
        Dim ST2 As Integer

        Dim RetornaMensaje As Integer
        Dim StringRetorno As String = ""
        Dim ValorRetorno As String = ""
        Dim RetornoStatus As Integer
        Dim Mensaje As String = ""

        VerificaRetornoImpresora = False

        If Retorno = 0 Then
            MsgBox("Error de comunicación con la impresora.", vbOKOnly + vbCritical, TituloVentana)
            Exit Function

        ElseIf Retorno = 1 Or Retorno = -27 Then
            RetornoStatus = Bematech_FI_RetornoImpresora(ACK, ST1, ST2)
            ValorRetorno = Str(ACK) & "," & Str(ST1) & "," & Str(ST2)
        End If


        If Label <> "" And Retorno <> 0 Then
            RetornaMensaje = 1
        End If

        If ACK = 21 Then
            MsgBox("Status de la Impresora: 21" & vbCr & vbLf & "Comando no ejecutado", vbOKOnly + vbInformation, TituloVentana)
            Exit Function
        End If

        If (ST1 <> 0 Or ST2 <> 0) Then
            If (ST1 >= 128) Then
                StringRetorno = "Fin de Papel" & vbCr
                ST1 = ST1 - 128
            End If

            If (ST1 >= 64) Then
                StringRetorno = StringRetorno & "Poco Papel" & vbCr
                ST1 = ST1 - 64
            End If

            If (ST1 >= 32) Then
                StringRetorno = StringRetorno & "Error en el reloj" & vbCr
                ST1 = ST1 - 32
            End If

            If (ST1 >= 16) Then
                StringRetorno = StringRetorno & "Impresora en error" & vbCr
                ST1 = ST1 - 16
            End If

            If (ST1 >= 8) Then
                StringRetorno = StringRetorno & "Primer dato del comando no fue Esc" & vbCr
                ST1 = ST1 - 8
            End If

            If (ST1 >= 4) Then
                StringRetorno = StringRetorno & "Comando inexistente" & vbCr
                ST1 = ST1 - 4
            End If

            If (ST1 >= 2) Then
                StringRetorno = StringRetorno & "Cupón fiscal abierto" & vbCr
                ST1 = ST1 - 2
            End If

            If (ST1 >= 1) Then
                StringRetorno = StringRetorno & "Número de parámetros inválidos en el comando" & vbCr
                ST1 = ST1 - 1
            End If

            If (ST2 >= 128) Then
                StringRetorno = "Tipo de Parámetro de comando inválido" & vbCr
                ST2 = ST2 - 128
            End If

            If (ST2 >= 64) Then
                StringRetorno = StringRetorno & "Memória fiscal llena" & vbCr
                ST2 = ST2 - 64
            End If

            If (ST2 >= 32) Then
                StringRetorno = StringRetorno & "Error en la CMOS" & vbCr
                ST2 = ST2 - 32
            End If

            If (ST2 >= 16) Then
                StringRetorno = StringRetorno & "Alicuota no programada" & vbCr
                ST2 = ST2 - 16
            End If

            If (ST2 >= 8) Then
                StringRetorno = StringRetorno & "Capacidad de alicuota programables llena" & vbCr
                ST2 = ST2 - 8
            End If

            If (ST2 >= 4) Then
                StringRetorno = StringRetorno & "Cancelamiento no permitido" & vbCr
                ST2 = ST2 - 4
            End If

            If (ST2 >= 2) Then
                StringRetorno = StringRetorno & "RIF del propietario no programados" & vbCr
                ST2 = ST2 - 2
            End If

            If (ST2 >= 1) Then
                StringRetorno = StringRetorno & "Comando no ejecutado" & vbCr
                ST2 = ST2 - 1
            End If

            If RetornaMensaje Then
                Mensaje = "Status de la Impresora: " & ValorRetorno & _
                       vbCr & vbLf & StringRetorno & vbCr & vbLf & _
                       Label & StringRetorno
            Else
                Mensaje = "Status de la Impresora: " & ValorRetorno & _
                   vbCr & vbLf & StringRetorno
            End If

            MsgBox(Mensaje, vbOKOnly + vbInformation, TituloVentana)
            Exit Function
        End If 'fin del ST1 <> 0 y ST2 <> 0

        If RetornaMensaje Then
            Mensaje = Label & Contenido
        End If

        If Mensaje <> "" Then
            MsgBox(Mensaje, vbOKOnly + vbInformation, TituloVentana)
        End If

        VerificaRetornoImpresora = True

        Exit Function

        If Retorno = -1 Then
            MsgBox("Error de ejecución de la función.", vbOKOnly + vbCritical, TituloVentana)
            Exit Function

        ElseIf Retorno = -2 Then
            MsgBox("Parámetro inválido en la función.", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function

        ElseIf Retorno = -3 Then
            MsgBox("Alicuota no programada.", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function

        ElseIf Retorno = -4 Then
            MsgBox("El archivo de inicialización BemaFI32.ini no fue encontrado en el directorio default. " + vbCr + "Por favor, copie ese archivo para el directorio de sistema de Windows." + vbCr + "Si utilizas el Windows 95 o 98 es el directorio 'System' si utilizas el Windows NT,2000 o XP es el directorio 'System32'.", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function

        ElseIf Retorno = -5 Then
            MsgBox("Error al abrir la puerta de comunicación.", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function

        ElseIf Retorno = -6 Then
            MsgBox("Impresora colgada o cable de comunicación desconectado.", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function

        ElseIf Retorno = -7 Then
            MsgBox("Banco no encontrado en el archivo BemaFI32.ini.", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function

        ElseIf Retorno = -8 Then
            MsgBox("Error al crear o grabar en el archivo status.txt o retorno.txt.", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function

        ElseIf Retorno = -18 Then
            MsgBox("No fue posible abrir el archivo INTPOS.001 !", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function

        ElseIf Retorno = -19 Then
            MsgBox("Parámetro diferentes !", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function

        ElseIf Retorno = -20 Then
            MsgBox("Transación anulada por el Operador !", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function

        ElseIf Retorno = -21 Then
            MsgBox("La transación no fue aprobada !", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function

        ElseIf Retorno = -22 Then
            MsgBox("No fue posible finalizar la impresión !", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function

        ElseIf Retorno = -23 Then
            MsgBox("No fue posible finalizar la operación !", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function

        ElseIf Retorno = -24 Then
            MsgBox("Forma de pago no programada.", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function

        ElseIf Retorno = -25 Then
            MsgBox("Totalizador no fiscal no programado.", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function

        ElseIf Retorno = -26 Then
            MsgBox("Transación ya realizada.", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function

        ElseIf Retorno = -28 Then
            MsgBox("No hay datos para serem impresos.", vbOKOnly + vbExclamation, TituloVentana)
            Exit Function
        End If

        VerificaRetornoImpresora = True
    End Function

    ' Public Sub CentralizaVentana(ByVal Form As Form)
    '     Form.Top = (Screen.Height - Form.Height) / 2
    '     Form.Left = (Screen.Width - Form.Width) / 2
    ' End Sub

    Public Function AnalisaFlagsFiscales(ByVal FlagFiscal As Integer) As String
        Dim StringRetorno As String = ""

        If (FlagFiscal >= 128) Then
            StringRetorno = "Memoria fiscal llena" & vbCr
            FlagFiscal = FlagFiscal - 128
        End If

        If (FlagFiscal >= 32) Then
            StringRetorno = StringRetorno & "Permite el cancelamiento del cupón" & vbCr
            FlagFiscal = FlagFiscal - 32
        End If

        If (FlagFiscal >= 8) Then
            StringRetorno = StringRetorno & "Ya hubo reducción 'Z' en el dia" & vbCr
            FlagFiscal = FlagFiscal - 8
        End If

        If (FlagFiscal >= 4) Then
            StringRetorno = StringRetorno & "Horario de verano seleccionado" & vbCr
            FlagFiscal = FlagFiscal - 4
        End If

        If (FlagFiscal >= 2) Then
            StringRetorno = StringRetorno & "Cierre de formas de pago iniciado" & vbCr
            FlagFiscal = FlagFiscal - 2
        End If

        If (FlagFiscal >= 1) Then
            StringRetorno = StringRetorno & "Cupón fiscal abierto" & vbCr
            FlagFiscal = FlagFiscal - 1
        End If

        AnalisaFlagsFiscales = StringRetorno

    End Function


    Public Function AnalisaStatusCheque(ByVal StatusCheque As Integer) As String

        Dim StringRetorno As String = ""

        If (StatusCheque = 1) Then
            StringRetorno = "Impresora ok." & vbCr

        ElseIf (StatusCheque = 2) Then
            StringRetorno = "Cheque en impresión." & vbCr

        ElseIf (StatusCheque = 3) Then
            StringRetorno = "Cheque posicionado." & vbCr

        ElseIf (StatusCheque = 4) Then
            StringRetorno = "Aguardando el posicionamento del cheque." & vbCr

        End If

        AnalisaStatusCheque = StringRetorno

    End Function

    'Public Sub DestacaTexto(ByVal Objeto As TextBox)
    '    Objeto.SelStart = 0
    '    Objeto.SelLength = Len(Objeto.Text)
    'End Sub

    Public Sub ExibeArquivoRetorno()
        If Dir(ArqRetorno) <> "" Then
            Shell("notepad.exe" + " " + ArqRetorno, vbNormalFocus)
        End If
    End Sub

    Public Sub ImprimirFacturaFiscalBematech(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroFactura As String, ByVal Cliente As String, _
                                         ByVal RIF As String, ByVal DireccionFiscal As String, ByVal Telefono As String, _
                                         ByVal FechaEmision As Date, ByVal CodigoCliente As String, ByVal CondicionPago As String, _
                                         ByVal CodigoVendedor As String, ByVal Vendedor As String)

        Dim ds As New DataSet

        Dim bCont As Integer
        'Imprime encabezado ó Abre cupón fiscal extendido
        Retorno = Bematech_FI_AbreComprobanteDeVentaEx(RIF, Cliente, DireccionFiscal)
        Call VerificaRetornoImpresora("", "", Retorno, "Emisión de Comprobante Fiscal")
        While Not VerificaRetornoImpresora("", "", Retorno, "Emisión de Comprobante Fiscal")
            Retorno = Bematech_FI_AbreComprobanteDeVentaEx(RIF, Cliente, DireccionFiscal)
        End While
        '

        Dim dtRenglones As DataTable
        Dim nTablaRenglon As String = "tblrenglones"
        ds = DataSetRequery(ds, " select * from jsvenrenpos where numfac = '" & NumeroFactura & "' and tipo = 0 and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaRenglon, lblinfo)
        dtRenglones = ds.Tables(nTablaRenglon)
        If dtRenglones.Rows.Count > 0 Then
            For bCont = 0 To dtRenglones.Rows.Count - 1
                With dtRenglones.Rows(bCont)
                    Dim aAlicuota As String
                    If PorcentajeIVA(MyConn, lblInfo, FechaEmision, .Item("IVA")) <> 0.0# Then
                        aAlicuota = Format(100 * PorcentajeIVA(MyConn, lblInfo, FechaEmision, .Item("IVA")))
                    Else
                        aAlicuota = "FF"
                    End If

                    Retorno = Bematech_FI_VendeArticulo("", Mid(.Item("descrip"), 1, 29), _
                             aAlicuota, "F", Format(.Item("cantidad"), "###0.000"), 2, _
                            Format(.Item("TOTREN") / .Item("cantidad"), "###0.00"), _
                            "$", Format(0, "###0.00"))

                    Call VerificaRetornoImpresora("", "", Retorno, "Emisión de Comprobante Fiscal")

                    While Not VerificaRetornoImpresora("", "", Retorno, "Emisión de Comprobante Fiscal")
                        Retorno = Bematech_FI_VendeArticulo("", Mid(.Item("descrip"), 1, 29), _
                             aAlicuota, "F", Format(.Item("cantidad"), "###0.000"), 2, _
                            Format(.Item("TOTREN") / .Item("cantidad"), "###0.00"), _
                            "$", Format(0, "###0.00"))
                    End While

                End With
            Next
        End If
        dtRenglones = Nothing

        Dim dtDescuentos As DataTable
        Dim nTablaDescuentos As String = "tblDescuentos"
        ds = DataSetRequery(ds, " select IFNULL(SUM(DESCUENTO),0) descuento from jsvendespos where numfac = '" & NumeroFactura & "' and ID_EMP = '" & jytsistema.WorkID & "'", MyConn, nTablaDescuentos, lblInfo)
        dtDescuentos = ds.Tables(nTablaDescuentos)

        Dim montoDescuento As Double
        montoDescuento = 0.0#
        If dtDescuentos.Rows.Count > 0 Then montoDescuento = IIf(IsDBNull(dtDescuentos.Rows(0).Item("descuento")), 0.0, dtDescuentos.Rows(0).Item("descuento"))

        Retorno = Bematech_FI_IniciaCierreCupon("D", "$", Format(montoDescuento, "###0.00"))
        Call VerificaRetornoImpresora("", "", Retorno, "Emisión de Comprobante Fiscal")
        While Not VerificaRetornoImpresora("", "", Retorno, "Emisión de Comprobante Fiscal")
            Retorno = Bematech_FI_IniciaCierreCupon("D", "$", Format(montoDescuento, "###0.00"))
        End While

        Dim mCont As Integer
        Dim dtFP As DataTable
        Dim nTablaFP As String = "tblfp"
        Dim FP As String

        ds = DataSetRequery(ds, " select * from jsvenforpag where numfac = '" & NumeroFactura & "' and origen = 'PVE' and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaFP, lblInfo)
        dtFP = ds.Tables(nTablaFP)
        If dtFP.Rows.Count > 0 Then
            For mCont = 0 To dtFP.Rows.Count - 1
                With dtFP.Rows(mCont)
                    Select Case .Item("formapag")
                        Case "EF"
                            FP = "EFECTIVO"
                        Case "TA"
                            FP = "TARJETA"
                        Case "CH"
                            FP = "CHEQUE"
                        Case "CT"
                            FP = "CESTA TICKET"
                        Case "DP"
                            FP = "DEPOSITO"
                        Case "TR"
                            FP = "TRANSFERENCIA"
                        Case Else
                            FP = "OTRO"
                    End Select
                    Retorno = Bematech_FI_EfectuaFormaPago(FP, Format(.Item("importe"), "###0.00"))
                    Call VerificaRetornoImpresora("", "", Retorno, "Forma de Pago")
                    While Not VerificaRetornoImpresora("", "", Retorno, "Forma de Pago")
                        Retorno = Bematech_FI_EfectuaFormaPago(FP, Format(.Item("importe"), "###0.00"))
                    End While
                End With
            Next
        End If

        dtFP = Nothing

        Retorno = Bematech_FI_FinalizarCierreCupon("******* Gracias por su compra *******")
        Call VerificaRetornoImpresora("", "", Retorno, "Emisión de Comprobante Fiscal")
        While Not VerificaRetornoImpresora("", "", Retorno, "Emisión de Comprobante Fiscal")
            Retorno = Bematech_FI_FinalizarCierreCupon("******* Gracias por su compra *******")
        End While

        ft.mensajeInformativo("Se ha enviado la FACTURA a la impresora fiscal ")

    End Sub

    Public Sub ImprimirNCFiscalBematech(ByVal MyConn As MySqlConnection, ByVal lblInfo As Label, ByVal NumeroFactura As String, ByVal Serial As String, ByVal Cliente As String, _
                                             ByVal RIF As String, ByVal DireccionFiscal As String, ByVal Telefono As String, _
                                             ByVal FechaEmision As Date, ByVal CodigoCliente As String, ByVal CondicionPago As String, _
                                             ByVal CodigoVendedor As String, ByVal Vendedor As String)

        Dim ds As New DataSet

        Dim bCont As Integer

        'Imprime encabezado ó Abre cupón NC
        Retorno = Bematech_FI_AbreNotaDeCredito(Cliente, Serial, RIF, Format(FechaEmision.Day, "00"), _
                                                Format(FechaEmision.Month, "00"), Right(Format(FechaEmision.Year, "0000"), 2), _
                                                Format(Now.Hour, "00"), Format(Now.Minute, "00"), Format(Now.Second, "00"), _
                                                NumeroFactura)

        Call VerificaRetornoImpresora("", "", Retorno, "Emisión de Nota de Crédito")
        While Not VerificaRetornoImpresora("", "", Retorno, "Emisión de Nota de Crédito")
            Retorno = Bematech_FI_AbreNotaDeCredito(Cliente, Serial, RIF, Format(FechaEmision.Day, "00"), _
                                                Format(FechaEmision.Month, "00"), Right(Format(FechaEmision.Year, "0000"), 2), _
                                                Format(Now.Hour, "00"), Format(Now.Minute, "00"), Format(Now.Second, "00"), _
                                                NumeroFactura)
        End While

        Dim dtRenglones As DataTable
        Dim nTablaRenglon As String = "tblrenglones"
        ds = DataSetRequery(ds, " select * from jsvenrenpos where numfac = '" & NumeroFactura & "' and tipo = 0 and id_emp = '" & jytsistema.WorkID & "' ", MyConn, nTablaRenglon, lblinfo)
        dtRenglones = ds.Tables(nTablaRenglon)
        If dtRenglones.Rows.Count > 0 Then
            For bCont = 0 To dtRenglones.Rows.Count - 1
                With dtRenglones.Rows(bCont)
                    Dim aAlicuota As String
                    If PorcentajeIVA(MyConn, lblInfo, FechaEmision, .Item("IVA")) <> 0.0# Then
                        aAlicuota = Format(100 * PorcentajeIVA(MyConn, lblInfo, FechaEmision, .Item("IVA")))
                    Else
                        aAlicuota = "FF"
                    End If

                    Retorno = Bematech_FI_VendeArticulo("", Mid(.Item("descrip"), 1, 29), _
                             aAlicuota, "F", Format(.Item("cantidad"), "###0.000"), 2, _
                            Format(.Item("TOTREN") / .Item("cantidad"), "###0.00"), _
                            "$", Format(0, "###0.00"))

                    Call VerificaRetornoImpresora("", "", Retorno, "Emisión de Comprobante Fiscal")
                    While Not VerificaRetornoImpresora("", "", Retorno, "Emisión de Comprobante Fiscal")
                        Retorno = Bematech_FI_VendeArticulo("", Mid(.Item("descrip"), 1, 29), _
                             aAlicuota, "F", Format(.Item("cantidad"), "###0.000"), 2, _
                            Format(.Item("TOTREN") / .Item("cantidad"), "###0.00"), _
                            "$", Format(0, "###0.00"))
                    End While

                End With
            Next
        End If
        dtRenglones = Nothing

        Dim dtDescuentos As DataTable
        Dim nTablaDescuentos As String = "tblDescuentos"
        ds = DataSetRequery(ds, " select IFNULL(SUM(DESCUENTO),0) descuento from jsvendespos where numfac = '" & NumeroFactura & "' and ID_EMP = '" & jytsistema.WorkID & "'", MyConn, nTablaDescuentos, lblInfo)
        dtDescuentos = ds.Tables(nTablaDescuentos)

        Dim montoDescuento As Double
        montoDescuento = 0.0#
        If dtDescuentos.Rows.Count > 0 Then montoDescuento = IIf(IsDBNull(dtDescuentos.Rows(0).Item("descuento")), 0.0, dtDescuentos.Rows(0).Item("descuento"))

        Retorno = Bematech_FI_IniciaCierreCupon("D", "$", Format(montoDescuento, "###0.00"))
        Call VerificaRetornoImpresora("", "", Retorno, "Emisión de Comprobante Fiscal")
        While Not VerificaRetornoImpresora("", "", Retorno, "Emisión de Comprobante Fiscal")
            Retorno = Bematech_FI_IniciaCierreCupon("D", "$", Format(montoDescuento, "###0.00"))
        End While

        Retorno = Bematech_FI_FinalizarCierreCupon("******* Gracias por su compra *******")
        Call VerificaRetornoImpresora("", "", Retorno, "Emisión de Comprobante Fiscal")
        While Not VerificaRetornoImpresora("", "", Retorno, "Emisión de Comprobante Fiscal")
            Retorno = Bematech_FI_FinalizarCierreCupon("******* Gracias por su compra *******")
        End While

        ft.mensajeInformativo("Se ha enviado la NOTA CREDITO a la impresora fiscal ")

    End Sub
    Public Sub ReporteXFiscalBematech()
        Retorno = Bematech_FI_LecturaX()
        Dim retornoImpresora As Boolean = VerificaRetornoImpresora("", "", Retorno, "Lectura X")
        While Not retornoImpresora
            retornoImpresora = VerificaRetornoImpresora("", "", Retorno, "Lectura X")
            Retorno = Bematech_FI_LecturaX()
        End While
        ft.mensajeInformativo("Reporte Fiscal X Finalizado...")

    End Sub
    Public Sub ReporteZFiscalBematech()
        Retorno = Bematech_FI_ReduccionZ("", "")
        Call VerificaRetornoImpresora("", "", Retorno, "Reducción Z")
        While Not VerificaRetornoImpresora("", "", Retorno, "Reducción Z")
            Retorno = Bematech_FI_ReduccionZ("", "")
        End While

    End Sub

End Module
