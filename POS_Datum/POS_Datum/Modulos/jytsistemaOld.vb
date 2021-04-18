Module jytsistema

    'Constantes Publicas
    Public nVersion As String
    Public Const DirReg As String = "HKEY_CURRENT_USER\SOFTWARE\Tecnologías Datum_AEDA\POSDatum\CurrentVersion\"
    Public Const ClaveServidor As String = "Servidor"
    Public Const ClaveBaseDatos As String = "MyDB"
    Public Const ClaveEncriptacion As String = "1234567890"

    Public Const strCon As String = " user id=root; password=_Antonio3105; pooling=false; Allow Zero Datetime=True "

    Public Const sFormatoFechaMySQL As String = "yyyy-MM-dd"
    Public Const sFormatoPorcentaje As String = "0.00"
    Public Const sFormatoPorcentajeLargo As String = "0.0000"
    Public Const sFormatoEntero As String = "#,##"
    Public Const sFormatoNumero As String = "#,##0.00"
    Public Const sFormatoCantidad As String = "#,##0.000"
    Public Const sFormatoCantidadLarga As String = "#,##0.00000"
    Public Const sFormatoFecha As String = "dd-MMM-yyyy"
    Public Const sFormatoFechaMedia As String = "dd-MMM-yyyy"
    Public Const sFormatoFechaCorta As String = "dd-MM-yyyy"
    Public Const sFormatoFechaLarga As String = "D"
    Public Const sFormatoFechaHoraMySQL As String = "yyyy-MM-dd HH:mm:ss"
    Public Const sFormatoHora As String = "hh:mm:ss"

    Public Const sValorNumero As String = "0000000000000000.00"
    Public Const sValorEntero As String = "0000000000000000"
    Public Const sValorCantidad As String = "000000000000000.000"
    Public Const sValorCantidadLasga As String = "000000000.00000"
    Public Const sValorPorcentajeLargo As String = "000000000000000.0000"

    Public Const MyDate As Date = #1/1/2008#

    Public nGestion As Integer

    Public sUsuario As String
    Public sNombreUsuario As String
    Public sFechadeTrabajo As Date = Now()
    Public strConn As String

    Public WorkID As String
    Public WorkName As String
    Public WorkExercise As String = ""
    Public WorkDataBase As String
    Public WorkDivition As String = ""

    Public Enum Gestion

        iContabilidad = 1
        iBancos = 2
        iRecursosHumanos = 3
        iCompras = 4
        iVentas = 5
        iPuntosdeVentas = 6
        iMercancías = 7
        iProduccion = 8
        iMedicionGerencial = 9
        iControlDeGestiones = 10
        iCilindros = 11
        iServicios = 12

    End Enum
    Public aGestion() As String = {"Contabilidad", "Bancos y Cajas", "Recursos humanos", "Compras y cuentas por pagar", _
                                   "Ventas y cuentas por cobrar", "Puntos de venta", "Mercancías", "Producción", "Medición gerencial", _
                                   "Control de gestiones", "Cilindros", "Servicios"}

    Public aMenuArchivosContabilidad() As String = {"RibbonTab1.Contabilidad.1.0.0", _
                                                    "RibbonGroup1.Archivos Contabilidad.1.1.0", _
                                                    "RibbonButton1.Cuentas Contables.1.2.0", _
                                                    "RibbonButton2.Asientos No procesados.1.2.0", _
                                                    "RibbonButton3.Asientos Procesados.1.2.0", _
                                                    "RibbonButton4.Presupuesto.1.2.0", _
                                                    "RibbonButton5.Estados Financieros.1.2.0", _
                                                    "RibbonButton7.Plantillas de asientos.1.2.0", _
                                                    "RibbonButton8.Reglas de contabilización.1.2.0"}
    Public aMenuProcesosContabilidad() As String = {"RibbonGroup11.Procesos Contabilidad.1.1.1", _
                                                    "RibbonButton11.Procesar asientos diferidos.1.2.1", _
                                                    "RibbonButton12.Contabilizar desde plantillas.1.2.1"}
    Public aMenuReportesContabilidad() As String = {"RibbonGroup12.Reportes Contabilidad.1.1.2", _
                                                    "RibbonButton13.Asientos contables.1.2.2", _
                                                    "RibbonButton14.Mayor analítico.1.2.2", _
                                                    "RibbonButton15.Balance de comprobación.1.2.2", _
                                                    "RibbonButton16.Libro de diario.1.2.2.", _
                                                    "RibbonButton17.Balance general.1.2.2", _
                                                    "RibbonButton18.Estado general de ganancias y pérdidas.1.2.2"}
    Public aMenuArchivosBancos() As String = {"RibbonTab2.Bancos y Cajas.2.0.0", _
                                              "RibbonGroup2.Archivos Bancos y Cajas.2.1.0", _
                                              "RibbonButton9.Bancos y movimientos.2.2.0", _
                                              "RibbonButton10.Cajas y movimientos.2.2.0"}
    Public aMenuProcesosBancos() As String = {"RibbonGroup15.Procesos Bancos y Cajas.2.1.1", _
                                              "RibbonButton31.Conciliar cuentas bancarias.2.2.1", _
                                              "RibbonButton194.Procesar IDB.2.2.1", _
                                              "RibbonButton195.Reversar IDB.2.2.1", _
                                              "RibbonButton51.Transferencias.2.2.1", _
                                              "RibbonButton52.Devolución de cheques.2.2.1", _
                                              "RibbonButton53.Devolución de cheques de alimentación.2.2.1", _
                                              "RibbonButton191.Cheques de alimentación falsificados.2.2.1", _
                                              "RibbonButton192.Anulación de cancelaciones a partir de un cheque de alimentación.2.2.1", _
                                              "RibbonButton196.Actualización de número para depósitos temporales.2.2.1"}
    Public aMenuReportesBancos() As String = {"RibbonGroup16.Reportes bancos y cajas.2.1.2", _
                                              "RibbonButton19.Disponibilidad bancaria.2.2.2", _
                                              "RibbonButton20.Conciliaciones de cuentas.2.2.2", _
                                              "RibbonButton21.Saldos mensuales.2.2.2", _
                                              "RibbonButton22.Estado de cuentas.2.2.2", _
                                              "RibbonButton188.Impuesto al Débito Bancario en un período.2.2.2", _
                                              "RibbonButton189.Impuesto al débito Bancario mes a mes.2.2.2", _
                                              "RibbonButton24.Depósitos de cajas.2.2.2", _
                                              "RibbonButton25.Arqueos de cajas.2.2.2", _
                                              "RibbonButton26.Bancos.2.2.2", _
                                              "RibbonButton27.Cheques devueltos.2.2.2", _
                                              "RibbonButton28.Remesas de cheques de alimentación.2.2.2", _
                                              "RibbonButton29.Movimientos diferidos.2.2.2", _
                                              "RibbonButton193.Resumen de reposiciones de saldo en caja chica.2.2.2"}
    Public aMenuArchivosNomina() As String = {"RibbonTab3.Recursos Humanos.3.0.0", _
                                              "RibbonGroup3.Archivos recursos humanos.3.1.0", _
                                              "RibbonButton32.Trabajadores.3.2.0", _
                                             "RibbonButton33.Grupos de trabajadores.3.2.0", _
                                             "RibbonButton34.Estructura de cargos.3.2.0", _
                                             "RibbonButton38.Turnos y horarios.3.2.0", _
                                             "RibbonButton35.Trabajadores por concepto.3.2.0", _
                                             "RibbonButton39.Definición de conceptos.3.2.0", _
                                             "RibbonButton40.Definición de constantes.3.2.0"}
    Public aMenuProcesosNomina() As String = {"RibbonGroup13.Procesos recursos humanos.3.1.1"}

    Public aMenuReportesNomina() As String = {"RibbonGroup14.Reportes recursos humanos.3.1.2", _
                                              "RibbonButton41.Trabajadores.3.2.2", _
                                              "RibbonButton42.Planilla de ingreso.3.2.2", _
                                              "RibbonButton43.Trabajadores por concepto.3.2.2", _
                                              "RibbonButton44.Prenómina.3.2.2", _
                                              "RibbonButton45.Resumen de nómina.3.2.2", _
                                              "RibbonButton46.Nómina.3.2.2", _
                                              "RibbonButton47.Recibos de nómina.3.2.2", _
                                              "RibbonButton48.Listado de firmas.3.2.2", _
                                              "RibbonButton49.Acumulados por concepto.3.2.2", _
                                              "RibbonButton50.Histórico de conceptos.3.2.2"}

    Public aMenuArchivosMercancias() As String = {"RibbonTab7.Mercancías.7.0.0", _
                                                  "RibbonGroup7.Archivos mercancías.7.1.0", _
                                                  "RibbonButton114.Mercancías.7.2.0", _
                                                  "RibbonButton115.Categorías.7.2.0", _
                                                  "RibbonButton135.Marcas.7.2.0", _
                                                  "RibbonButton136.Jerarquías.7.2.0", _
                                                  "RibbonButton137.Transferencias y Consumos internos.7.2.0", _
                                                  "RibbonButton138.Cuotas Anuales.7.2.0", _
                                                  "RibbonButton139.Conteos de inventarios.7.2.0", _
                                                  "RibbonButton140.Precios a futuro.7.2.0", _
                                                  "RibbonButton141.Precios especiales.7.2.0", _
                                                  "RibbonButton142.Ofertas.7.2.0", _
                                                  "RibbonButton143.Servicios.7.2.0"}
    Public aMenuProcesosMercancias() As String = {"RibbonGroup23.Procesos mercancías.7.1.1"}
    Public aMenuReportesMercancias() As String = {"RibbonGroup24.Reportes mercancías.7.1.2"}

    Public aMenuArchivosControl() As String = {"RibbonTab10.Control de Gestión.10.0.0", _
                                               "RibbonGroup10.Archivos control de gestión.10.1.0", _
                                               "RibbonButton165.Empresas.10.2.0", _
                                               "RibbonButton166.Calendario de trabajo.10.2.0", _
                                               "RibbonButton167.Usuarios.10.2.0", _
                                               "RibbonButton168.Mapas de acceso.10.2.0", _
                                               "RibbonButton169.Auditorías de acceso.10.2.0", _
                                               "RibbonButton170.Impuesto al Valor Agregado.10.2.0", _
                                               "RibbonButton171.Retenciones del Impuesto Sobre La Renta.10.2.0", _
                                               "RibbonButton172.Contadores.10.2.0", _
                                               "RibbonButton173.Parámetrso del sistema.10.2.0", _
                                               "RibbonButton174.Tipos de condiciones de pago y/o cobro a crédito.10.2.0", _
                                               "RibbonButton175.Tarjetas de crédito y/o débito.10.2.0", _
                                               "RibbonButton176.Tipos de estatus de clientes.10.2.0", _
                                               "RibbonButton177.Causas para créditos.10.2.0", _
                                               "RibbonButton178.Causas para débitos.10.2.0", _
                                               "RibbonButton179.Divisiones.10.2.0", _
                                               "RibbonButton180.Transportes.10.2.0", _
                                               "RibbonButton181.Almacenes.10.2.0", _
                                               "RibbonButton182.Bancos de la plaza.10.2.0", _
                                               "RibbonButton183.Agentes de cheques de alimentación.10.2.0", _
                                               "RibbonButton184.División política territorial.10.2.0", _
                                               "RibbonButton185.Monedas y cambios.10.2.0"}
    Public aMenuProcesosControl() As String = {"RibbonGroup27.Procesos control de gestión.10.1.1"}
    Public aMenuReportesControl() As String = {"RibbonGroup28.Reportes control de gestión.10.1.2"}
    Public Enum MovAud

        ientrar = 1
        iSalir = 2
        iIncluir = 3
        imodificar = 4
        iEliminar = 5
        iSeleccionar = 6
        iImprimir = 7
        iProcesar = 8

    End Enum
    Public Enum Asiento
        iDiferido = 0
        iActual = 1
    End Enum
    Public Enum movimiento
        iAgregar = 1
        iEditar = 2
        iEliminar = 3
        iConsultar = 4
    End Enum
    Public Enum TipoCargaFormulario
        iShow = 0
        iShowDialog = 1
    End Enum
    Public Enum iProceso
        Procesar = 0
        Reversar = 1
    End Enum
    Public Enum Modulo
        iAlmacen = 1
        iCategoriaMerca = 2
        iMarcaMerca = 3
        iCategoriaclientes = 4
        iZonasClientes = 5
        iUnidadClientes = 6
        iCategoriaProveedor = 7
        iZonaProveedor = 8
        iUnidadProveedor = 9
        iBancos = 10
        iCestaTicket = 11
        iGrupoNom = 12
        iGrupoGasto = 13
        iSubGrupoGasto = 14
        iCausasDevolucion = 15
        iCausasDebitos = 16
        iCausasEstatusActivo = 17
        iCausasEstatusBloqueado = 18
        iCausasEstatusInactivo = 19
        iCausasEstatusDesincorporado = 20
        iCausaMercanciaActiva = 21
        iCausaMercanciaInactiva = 22
        iFabricanteCilindros = 23
        iImportadorCilindros = 24
        iModeloVehiculo = 25
        iSeccionAvisoClasificado = 26
        iAreaRestaturant = 27
        iConcesionarios = 28
    End Enum
    Public Enum AlineacionDataGrid
        Izquierda = DataGridViewContentAlignment.MiddleLeft
        Centro = DataGridViewContentAlignment.MiddleCenter
        Derecha = DataGridViewContentAlignment.MiddleRight
    End Enum
    Public Enum TipoPeriodo
        iDiario = 0
        iSemanal = 1
        iMensual = 2
        iAnual = 3
    End Enum
    Public Enum ReportesContabilidad

        cCuentasContables = 101
        cPolizaDiferida = 102
        cPolizaActual = 103
        cPolizasActuales = 104
        cMayorAnalitico = 105
        cBalanceComprobacion = 106
        cDiario = 107
        cBalanceGeneral = 108
        cGananciasPerdidas = 109

    End Enum
    Public Enum ReporteBancos

        cFichaBanco = 201
        cMovimientoBanco = 202
        cMovimientoCaja = 203
        cDisponibilidad = 204
        cConciliacion = 205
        cSaldosMensuales = 206
        cEstadoCuenta = 207
        cDepositos = 208
        cIDB = 209
        cIDBMes = 210
        cChequeDevuelto = 211
        cTicketDevuelto = 212
        cTransferencia = 213
        cArqueoCajas = 214
        cListadoBancos = 215
        cChequesDevueltos = 216
        cRemesasTickets = 217
        cMovimientosPostDatados = 218
        cComprobanteDeEgreso = 219
        cFormatoCheque = 220
        cCheque = 221
        cReposicionSaldoCaja = 222
        cResumenReposicionSaldoCaja = 223

    End Enum
    Public Enum ReporteNomina

        cFichaTrabajador = 301
        cTrabajadores = 302
        cConstantes = 303
        cConceptos = 304
        cConceptosXTrabajador = 305
        cConceptosXTrabajadores = 306
        cPrenomina = 307
        cRegistroTrabajador = 308

    End Enum
    Public Enum FormatoItemListView
        iFecha = 1
        iEntero = 2
        iCadena = 3
        iNumero = 4
        iCantidad = 5
        iFechaHora = 6
        iSino = 7
        iBoolean = 8
    End Enum
    Public Enum DiferenciaFechas
        iDias = 0
        iMeses = 1
        iAños = 2
        iAñosMeses = 3
        iAñosMesesDias = 4
    End Enum
    
    Public ColorDeshabilitado As Color = Color.FromArgb(234, 241, 250)
    Public ColorHabilitado As Color = Color.White

    Public aFormaPago() As String = {"Efectivo", "Cheque", "Tarjeta", "Cupón de Alimentación", "Depósito", "Transferencia"}
    Public aFormaPagoAbreviada() As String = {"EF", "CH", "TA", "CT", "DP", "TR"}

    Public aTipoNomina() As String = {"DIARIA", "SEMANAL", "QUINCENAL", "MENSUAL", "ANUAL", "EVENTUAL"}
    Public aCausaExpedienteNomina() As String = {"Ingreso", "Vacaciones", "Permiso remunerado", "Reposo médico", _
                                                 "Permiso no remunerado", "Ausencia laboral", "Retrasos", _
                                                 "Enfermedad ocupacional", "suspención", "Egreso/Desincorporación"}

    Public aUnidad() As String = {"Unidad", "Caja", "Bulto", "Paleta", "Kilogramo", "Gramo", "Tonelada", "Litro", "MiliLitro", "Decilitro", "Centímetro Cúbico", "Metro Cúbico", "Tambor", "Galón", "Cuñete", "Docena", "Pote", "Paquete", "Pieza", "Display", "Estuche", "Rollo", "Saco", "Stiva", "Cesta", "Tanque", "Bolsa", "Centímetro", "Metro", "Kilómetro", "Hectárea", "Carrete", "Otro"}
    Public aUnidadAbreviada() As String = {"UND", "CAJ", "BUL", "PAL", "KGR", "GRS", "TON", "LTS", "MLT", "DLT", "CC", "MC", "TMB", "GAL", "CUÑ", "DOC", "POT", "PAQ", "PZA", "DPL", "EST", "ROL", "SAC", "STI", "CES", "TAN", "BOL", "CM", "MTR", "KMR", "HEC", "CAR", "OTR"}


End Module
