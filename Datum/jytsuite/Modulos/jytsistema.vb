Module jytsistema

    'Constantes Publicas
    Public nVersion As String
    Public Const DirReg As String = "HKEY_CURRENT_USER\SOFTWARE\Tecnologías Jytsuite\Jytsuite\CurrentVersion\"
    Public Const ClaveServidor As String = "Servidor"
    Public Const ClaveBaseDatos As String = "MyDB"
    Public Const ClaveCaja As String = "Caja"
    Public Const ClaveEmpresa As String = "Empresa"
    Public Const ClaveEncriptacion As String = "1234567890"
    Public Const ClaveNumeroControl As String = "NumeroControl"
    Public Const ClaveImpresorFiscal As String = "CajaImpresionFiscal"
    Public Const ClaveContadorMaesto As String = "CONTADOR"
    Public Const ClaveAleatoria As String = "SemillaAleatoria"

    Public Const strCon As String = " user id=root; password=jytsuite; Persist Security Info=True; default command timeout=0; " _
        & " Connect Timeout=60; pooling=TRUE; convert zero datetime=True "

    Public Const sFormatoFechaMySQL As String = "yyyy-MM-dd"
    Public Const sFormatoPorcentaje As String = "0.00"
    Public Const sFormatoPorcentajeSimbolo As String = "0.00\%"
    Public Const sFormatoPorcentajeLargo As String = "0.0000"
    Public Const sFormatoEntero As String = "#,##"
    Public Const sFormatoNumero As String = "#,##0.00"
    Public Const sFormatoCantidad As String = "#,##0.000"
    Public Const sFormatoCantidadLarga As String = "#,##0.00000"
    Public Const sFormatoFecha As String = "dd-MM-yyyy"
    Public Const sFormatoFechaBarra As String = "dd/MM/yyyy"
    Public Const sFormatoFechaMedia As String = "dd-MM-yyyy"
    Public Const sFormatoFechaCorta As String = "dd-MM-yyyy"
    Public Const sFormatoFechaLarga As String = "D"
    Public Const sFormatoFechaHoraMySQL As String = "yyyy-MM-dd HH:mm:ss"
    Public Const sFormatoHora As String = "hh:mm:ss"
    Public Const sFormatoHoraCorta As String = "HH:mm"
    Public Const sFormatoFechaFiscal = "ddmmyy"
    Public Const FORMATO_PRECIO As String = "0000000000"
    Public Const FORMATO_CANTIDAD As String = "00000000"
    Public Const FORMATO_ENTERO12 As String = "000000000000"
    Public Const FORMATO_ENTERO10 As String = "0000000000"
    Public Const sFormatoCantidadEpson As String = "0.000"
    Public Const sFormatoNumeroEpson As String = "0.00"

    Public Const sValorNumero As String = "0000000000000000.00"
    Public Const sValorEntero As String = "0000000000000000"
    Public Const sValorCantidad As String = "000000000000000.000"
    Public Const sValorCantidadLasga As String = "000000000.00000"
    Public Const sValorPorcentajeLargo As String = "000000000000000.0000"

    Public Const MyDate As Date = #1/1/2010#

    Public nGestion As Integer

    Public sUsuario As String
    Public sNombreUsuario As String
    Public sFechadeTrabajo As DateTime = Now()

    Public strConn As String

    Public WorkID As String
    Public WorkName As String
    Public WorkExercise As String = ""
    Public WorkDataBase As String
    Public WorkDivition As String = ""
    Public WorkBox As String = ""
    Public WorkRegion As String = ""
    Public WorkLanguage As Integer = 0
    Public WorkCurrency As New Moneda()

    Public Structure UnidadDeMedida
        Dim Abreviatura As String
        Dim Descripcion As String
    End Structure

    Public Structure Bonificaciones
        Dim ItemABonificar As String
        Dim CantidadABonificar As Double
        Dim UnidadDeBonificacion As String
    End Structure

    Public Structure Perfil
        Dim Credito As Boolean
        Dim Contado As Boolean
        Dim TarifaA As Boolean
        Dim TarifaB As Boolean
        Dim TarifaC As Boolean
        Dim TarifaD As Boolean
        Dim TarifaE As Boolean
        Dim TarifaF As Boolean
        Dim Almacen As String
        Dim Descuento As Integer
    End Structure

    Public Enum TipoAplicacion

        iBasic = 0 'Bancos- / Compras- / Ventas- / Mercancias- / Punto de Venta / Control
        iNormal = 1 'Contabilidad / Recursos Humanos / Bancos / Compras / Gastos / Ventas / Mercancias / Punto de Venta / Control 
        iPlus = 2 'Contabilidad / Bancos+ / Compras+ / Gastos+ / Ventas+ / Mercancias+ / Punto de Venta / Control / SIGME
        iPlusProduccion = 3 'iPlus + Produccion
        iPlusRestaurant = 4 'iPlus -SIGME  +Cilindros
        iPlusPrensa = 5
        iPlusFarmacia = 6
        iPlusClinicas = 7
        iPlusHotelSpa = 8
        iPlusGases = 9

    End Enum
    Public Enum IdiomaDatum
        iEspañol = 0
        iIngles = 1
    End Enum


    Public AplicacionTipo As TipoAplicacion = TipoAplicacion.iPlusProduccion

    Public Enum Gestion

        iContabilidad = 1
        iBancos = 2
        iRecursosHumanos = 3
        iCompras = 4
        iVentas = 5
        iPuntosdeVentas = 6
        iMercancías = 7
        iMedicionGerencial = 8
        iProduccion = 9
        iControlDeGestiones = 10

    End Enum
    Public aGestion() As String = {"Contabilidad", "Bancos y Cajas", "Recursos humanos", "Compras y cuentas por pagar", _
                                   "Ventas y cuentas por cobrar", "Puntos de venta", "Mercancías", "Medición gerencial", "Producción", _
                                   "Control de gestiones"}

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
                                                    "RibbonButton299.Reversar asientos actuales.1.2.1", _
                                                    "RibbonButton12.Contabilizar desde plantillas.1.2.1", _
                                                    "RibbonButton305.Recalcular saldos de cuentas contables.1.2.1", _
                                                    "RibbonButton292.Generar asiento apertura ejercicio.1.2.1"}
    Public aMenuReportesContabilidad() As String = {"RibbonGroup12.Reportes Contabilidad.1.1.2", _
                                                    "RibbonButton13.Asientos contables.1.2.2", _
                                                    "RibbonButton14.Mayor analítico.1.2.2", _
                                                    "RibbonButton15.Balance de comprobación.1.2.2", _
                                                    "RibbonButton16.Libro de diario.1.2.2.", _
                                                    "RibbonButton17.Balance general.1.2.2", _
                                                    "RibbonButton18.Estado general de ganancias y pérdidas.1.2.2", _
                                                    "RibbonButton109.Reglas de contabilización.1.2.2"}

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
                                              "RibbonButton298.Nóminas de Trabajadores.3.2.0", _
                                              "RibbonButton34.Estructura de cargos.3.2.0", _
                                              "RibbonButton38.Turnos y horarios.3.2.0", _
                                              "RibbonButton35.Trabajadores por concepto.3.2.0", _
                                              "RibbonButton39.Definición de conceptos.3.2.0", _
                                              "RibbonButton40.Definición de constantes.3.2.0"}
    Public aMenuProcesosNomina() As String = {"RibbonGroup13.Procesos recursos humanos.3.1.1", _
                                              "RibbonButton200.Procesar nómina.3.1.2", _
                                              "RibbonButton201.Reversar nómina.3.1.2", _
                                              "RibbonButton231.Reprocesar conceptos nómina de trabajadores.3.1.2", _
                                              "RibbonButton304.Pagar Nóminas.3.1.2"}
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
                                              "RibbonButton50.Conceptos por trabajador mes a mes.3.2.2", _
                                              "RibbonButton242.Conceptos mes a mes.3.2.2"}

    Public aMenuArchivosCompras() As String = {"RibbonTab4.Compras, gastos y cuentas por pagar.4.0.0", _
                                              "RibbonGroup4.Archivos compras, gastos y cuentas por pagar.4.1.0", _
                                              "RibbonButton55.Proveedores y cuentas por pagar.4.2.0", _
                                              "RibbonButton56.Unidades y categorías de negocios.4.2.0", _
                                              "RibbonButton57.Zonas y regiones.4.2.0", _
                                              "RibbonButton58.Gastos.4.2.0", _
                                              "RibbonButton59.Ordenes de compra.4.2.0", _
                                              "RibbonButton60.Recepciones de mercancías.4.2.0", _
                                              "RibbonButton61.Compras.4.2.0", _
                                              "RibbonButton62.Notas de crédito.4.2.0", _
                                              "RibbonButton63.Notas débito.4.2.0"}
    Public aMenuProcesosCompras() As String = {"RibbonGroup17.Procesos compras, gastos y cuentas por pagar.4.1.1", _
                                               "RibbonMenu38.Histórico compras y gastos.4.2.1", _
                                               "RibbonButton240.Pasar a histórico.4.3.1", _
                                               "RibbonButton241.Reversar de histórico.4.3.1", _
                                               "RibbonButton225.Reconstrucción de movimientos de mercancías y saldos de proveedores.4.3.1", _
                                               "RibbonButton236.Programación de pagos.4.2.1", _
                                               "RibbonButton237.Retenciones IVA para el SENIAT (Archivo txt).4.2.1", _
                                               "RibbonButton238.Retenciones ISLR para el SENIAT (Archivo xml).4.2.1", _
                                               "RibbonButton239.Asignar y/o modificar numeros de control compras y gastos.4.2.1", _
                                               "RibbonMenu49.Compras a/de Inventario.4.2.1", _
                                               "RibbonButton286.Compras a Inventario.4.3.1", _
                                               "RibbonButton287.Compras de Inventario.4.3.1"}

    Public aMenuReportesCompras() As String = {"RibbonGroup18.Reportes compras, gastos y cuentas por pagar.4.1.2", _
                                              "RibbonButton64.Proveedores.4.2.2", _
                                              "RibbonButton288.Movimientos de proveedores.4.2.2", _
                                              "RibbonButton66.Estado de cuentas.4.2.2", _
                                              "RibbonButton67.Saldos de proveedores a una fecha.4.2.2", _
                                              "RibbonButton68.Auditorías.4.2.2", _
                                              "RibbonButton69.Vencimientos.4.2.2", _
                                              "RibbonButton247.Vencimientos (Resumen).4.2.2", _
                                              "RibbonButton70.Libro de compras del impuesto al valor agregado.4.2.2", _
                                              "RibbonButton248.Listado de retenciones de IVA.4.2.2", _
                                              "RibbonButton249.Listado de retenciones de ISLR.4.2.2", _
                                              "RibbonButton71.Ordenes de compras.4.2.2", _
                                              "RibbonButton72.Recepciones de mercancías.4.2.2", _
                                              "RibbonButton73.Compras.4.2.2", _
                                              "RibbonButton74.Notas de crédito.4.2.2", _
                                              "RibbonButton75.Notas débito.4.2.2", _
                                              "RibbonButton245.Gastos.4.2.2", _
                                              "RibbonButton277.Compras/Gastos/Notas Crédito sin retención IVA.4.2.2"}

    Public aMenuArchivosVentas() As String = {"RibbonTab5.Ventas y cuentas por cobrar.5.0.0", _
                                              "RibbonGroup5.Archivos ventas y cuentas por cobrar.5.1.0", _
                                              "RibbonButton30.Clientes y cuentas por cobrar.5.2.0", _
                                              "RibbonButton76.Canales y tipos de negocio.5.2.0", _
                                              "RibbonButton77.Rutas y regiones.5.2.0", _
                                              "RibbonButton78.Rutas de ventas.5.2.0", _
                                              "RibbonButton79.Rutas de despacho.5.2.0", _
                                              "RibbonButton80.Grupo de ventas.5.2.0", _
                                              "RibbonButton81.Presupuestos.5.2.0", _
                                              "RibbonButton214.Pre-Pedidos.5.2.0", _
                                              "RibbonButton82.Pedidos.5.2.0", _
                                              "RibbonButton83.Apartados.5.2.0", _
                                              "RibbonButton84.Notas de entrega.5.2.0", _
                                              "RibbonButton85.Facturas.5.2.0", _
                                              "RibbonButton86.Notas de crédito.5.2.0", _
                                              "RibbonButton87.Notas débito.5.2.0"}
    Public aMenuProcesosVentas() As String = {"RibbonGroup19.Procesos ventas y cuentas por cobrar.5.1.1", _
                                              "RibbonButton215.Reconstrucción de movimientos de mercancías y saldos de clientes.5.2.1", _
                                              "RibbonButton216.Cambio, verificación y bloqueo de estatus de clientes.5.2.1", _
                                              "RibbonButton235.Pre-cancelaciones a cancelaciones.5.2.1", _
                                              "RibbonButton217.Pre-pedidos a pedidos.5.2.1", _
                                              "RibbonButton218.Pedidos a facturación.5.2.1", _
                                              "RibbonButton219.Guías de Carga/Despacho.5.2.1", _
                                              "RibbonButton220.Relación de facturas.5.2.1", _
                                              "RibbonButton221.Relación de Notas de Crédito.5.2.1", _
                                              "RibbonMenu37.Cierres.5.2.1", _
                                              "RibbonButton222.Cierre diario de ventas.5.3.1", _
                                              "RibbonButton223.Cierre mensual de ventas.5.3.1", _
                                              "RibbonButton268.Reporte X.5.3.1", _
                                              "RibbonButton269.Reporte Z.5.3.1", _
                                              "RibbonButton224.Cancelar CXC a partir de guía de despacho.5.2.1", _
                                              "RibbonButton226.Reconstrucción de límites de crédito de clientes.5.2.1", _
                                              "RibbonButton227.Anulación de documentos fiscales.5.2.1", _
                                              "RibbonButton228.Asignar y/o modificar números de control fiscal.5.2.1", _
                                              "RibbonButton232.Verificación de consecutivos.5.2.1", _
                                              "RibbonButton272.Mercancías de pedidos por asesor.5.2.1", _
                                              "RibbonButton273.Guías de Pedidos.5.2.1"}
    Public aMenuReportesVentas() As String = {"RibbonGroup20.Reportes ventas y cuentas por cobrar.5.1.2", _
                                              "RibbonMenu32.Clientes.5.2.2", _
                                              "RibbonButton88.Clientes.5.3.2", _
                                              "RibbonButton89.Fichas de clientes.5.3.2", _
                                              "RibbonButton90.Planilla de ingreso.5.3.2", _
                                              "RibbonMenu12.Cuentas por cobrar.5.2.2", _
                                              "RibbonButton116.Movimientos por cliente.5.3.2", _
                                              "RibbonButton117.Movimientos por documento.5.3.2", _
                                              "RibbonButton94.Estado de cuentas.5.3.2", _
                                              "RibbonButton95.Saldos a una fecha.5.3.2", _
                                              "RibbonButton96.Auditorías.5.3.2", _
                                              "RibbonButton97.Vencimientos.5.3.2", _
                                              "RibbonButton246.Vencimientos (Resumen).5.3.2", _
                                              "RibbonButton98.Libro de ventas del impuesto al valor agregado.5.2.2", _
                                              "RibbonButton276.Listado retenciones IVA clientes.5.2.2", _
                                              "RibbonMenu33.Documentos.5.2.2", _
                                              "RibbonButton99.Presupuestos.5.3.2", _
                                              "RibbonButton229.Pre-pedidos.5.3.2", _
                                              "RibbonButton100.Pedidos.5.3.2", _
                                              "RibbonButton101.Apartados.5.3.2", _
                                              "RibbonButton102.Notas de Entrega.5.3.2", _
                                              "RibbonButton103.Facturas.5.3.2", _
                                              "RibbonButton104.Notas de Crédito.5.3.2", _
                                              "RibbonButton105.Notas Débito.5.3.2", _
                                              "RibbonMenu17.Documentos mes a mes.5.2.2", _
                                              "RibbonButton128.Presupuestos.5.3.2", _
                                              "RibbonButton230.Pre-Pedidos.5.3.2", _
                                              "RibbonButton129.Pedidos.5.3.2", _
                                              "RibbonButton130.Apartados.5.3.2", _
                                              "RibbonButton131.Notas de Entrega.5.3.2", _
                                              "RibbonButton132.Facturas.5.3.2", _
                                              "RibbonButton133.Notas de Crédito.5.3.2", _
                                              "RibbonButton134.Notas Débito.5.3.2", _
                                              "RibbonMenu34.Asesores Comerciales.5.2.2", _
                                              "RibbonMenu35.Activaciones.5.3.2", _
                                              "RibbonButton118.Clientes y mercancías.5.4.2", _
                                              "RibbonButton119.Clientes y cheques devueltos.5.4.2", _
                                              "RibbonButton120.Clientes mes a mes.5.4.2", _
                                              "RibbonButton106.Pedidos sin facturar.5.3.2", _
                                              "RibbonButton107.Cheques devueltos.5.3.2", _
                                              "RibbonButton108.Cheques devueltos mes a mes.5.3.2", _
                                              "RibbonMenu14.Ventas.5.2.2", _
                                              "RibbonButton121.por clientes.5.3.2", _
                                              "RibbonButton275.por clientes (R).5.3.2", _
                                              "RibbonButton122.por clientes y división.5.3.2", _
                                              "RibbonButton123.por clientes mes a mes.5.3.2", _
                                              "RibbonMenu15.Cobranzas.5.2.2", _
                                              "RibbonButton124.de meses anteriores.5.3.2", _
                                              "RibbonButton125.del mes actual.5.3.2", _
                                              "RibbonMenu16.Cierres.5.2.2", _
                                              "RibbonButton126.Financieros (CREDITO).5.3.2", _
                                              "RibbonButton274.Financieros (CONTADO).5.3.2", _
                                              "RibbonButton127.de Cheques de Alimentación.5.3.2", _
                                              "RibbonButton233.Pesos para pedidos.5.2.2", _
                                              "RibbonButton110.Backorders.5.2.2", _
                                              "RibbonButton111.Pedidos versus estatus de clientes.5.2.2", _
                                              "RibbonButton291.Pre-Pedidos versus estatus de clientes.5.2.2", _
                                              "RibbonButton112.Ranking de ventas.5.2.2", _
                                              "RibbonButton270.Relación de mercancías de pedidos para carga/despacho.5.2.2", _
                                              "RibbonMenu46.Reportes planos.5.2.2", _
                                              "RibbonButton278.Cobranza.5.3.2", _
                                              "RibbonMenu48.Comisiones.5.2.2", _
                                              "RibbonButton281.Comisiones de ventas por jerarquía.5.3.2", _
                                              "RibbonButton282.Comisiones de ventas por facturas y jerarquía.5.3.2", _
                                              "RibbonButton283.Comisiones de ventas por días de cobranza.5.3.2"}

    Public aMenuArchivosPuntoDeVenta() As String = {"RibbonTab6.Puntos de Venta.6.0.0", _
                                                    "RibbonGroup6.Archivos Puntos de Venta.6.1.0", _
                                                    "RibbonButton91.Cajas ó puntos de venta.6.2.0", _
                                                    "RibbonButton92.Cajeros ó vendedores.6.2.0", _
                                                    "RibbonButton93.Supervisores.6.2.0", _
                                                    "RibbonButton251.Facturas Puntos de Venta.6.2.0", _
                                                    "RibbonButton266.Clientes Puntos de Venta.6.2.0"}
    Public aMenuProcesosPuntoDeVenta() As String = {"RibbonGroup21.Procesos puntos de venta.6.1.1", _
                                                    "RibbonButton54.Reprocesar Facturas.6.2.1", _
                                                    "RibbonButton113.Números de control.6.2.1", _
                                                    "RibbonButton271.Reprocesar cierre de caja.6.2.1"}
    Public aMenuReportesPuntoDeVenta() As String = {"RibbonGroup22.Reportes puntos de venta.6.1.2", _
                                                    "RibbonButton186.Reporte XZ.6.2.2", _
                                                    "RibbonButton198.Facturación.6.2.2", _
                                                    "RibbonButton199.Libro de ventas.6.2.2"}

    Public aMenuArchivosMercancias() As String = {"RibbonTab7.Mercancías.7.0.0", _
                                                  "RibbonGroup7.Archivos mercancías.7.1.0", _
                                                  "RibbonButton114.Mercancías.7.2.0", _
                                                  "RibbonButton115.Categorías.7.2.0", _
                                                  "RibbonButton135.Marcas.7.2.0", _
                                                  "RibbonButton136.Jerarquías.7.2.0", _
                                                  "RibbonButton301.Pedidos de mercancías.7.2.0", _
                                                  "RibbonButton137.Transferencias y Consumos internos.7.2.0", _
                                                  "RibbonButton139.Conteos de inventarios.7.2.0", _
                                                  "RibbonButton140.Precios a futuro.7.2.0", _
                                                  "RibbonButton141.Precios especiales.7.2.0", _
                                                  "RibbonButton142.Ofertas.7.2.0", _
                                                  "RibbonButton143.Servicios.7.2.0", _
                                                  "RibbonButton181.Almacenes.7.2.0", _
                                                  "RibbonButton308.Envases.7.2.0"}

    Public aMenuProcesosMercancias() As String = {"RibbonGroup23.Procesos mercancías.7.1.1", _
                                                  "RibbonMenu13.Conteos de mercancías.7.2.1", _
                                                  "RibbonButton207.Procesar conteos de mercancías.7.3.1", _
                                                  "RibbonButton208.Reversar conteos de mercancías.7.3.1", _
                                                  "RibbonMenu36.Combos de mercancías.7.2.1", _
                                                  "RibbonButton211.Construcción de combos de mercancías.7.3.1", _
                                                  "RibbonButton212.Reversar combos de mercancías.7.3.1", _
                                                  "RibbonButton209.Reconstrucción de movimientos, existencias y costos de mercancías.7.2.1", _
                                                  "RibbonButton210.Reconstrucción de precios de mercancías.7.2.1", _
                                                  "RibbonButton284.Reconstrucción de ganancias de mercancías.7.2.1", _
                                                  "RibbonButton213.Mercancías para almacenistas.7.2.1", _
                                                  "RibbonButton300.Mercancías Detallistas.7.2.1",
                                                  "RibbonButton206.Construcción de cuotas anuales de mercancías.7.2.1", _
                                                  "RibbonButton293.Construcción de cuotas a partir de una compra.7.2.1"}
    Public aMenuReportesMercancias() As String = {"RibbonGroup24.Reportes mercancías.7.1.2", _
                                                  "RibbonButton144.Mercancías.7.2.2", _
                                                  "RibbonButton145.Existencias y días de inventario a una fecha.7.2.2", _
                                                  "RibbonButton243.Inventarios y costos promedios.7.2.2", _
                                                  "RibbonButton146.Equivalencias.7.2.2", _
                                                  "RibbonButton147.Movimientos de mercancías.7.2.2", _
                                                  "RibbonButton265.Movimientos de mercancías (Resumen).7.2.2", _
                                                  "RibbonButton303.Movimientos de Servicios.7.2.2", _
                                                  "RibbonButton148.Obsolescencias.7.2.2", _
                                                  "RibbonMenu22.Precios.7.2.2", _
                                                  "RibbonButton152.Precios de venta con impuesto al valor agregado.7.3.2", _
                                                  "RibbonButton153.Precios de venta sin impuesto al valor agregado.7.3.2", _
                                                  "RibbonButton154.Precios  y equivalencias de mercancías.7.3.2", _
                                                  "RibbonButton311.Precios  y equivalencias de mercancías SIN IVA.7.3.2", _
                                                  "RibbonMenu23.Ventas.7.2.2", _
                                                  "RibbonButton155.Ventas a clientes.7.3.2", _
                                                  "RibbonButton156.Ventas a clientes y activaciones.7.3.2", _
                                                  "RibbonButton157.Ventas a clientes y activaciones mes a mes.7.3.2", _
                                                  "RibbonButton149.Ventas netas (Facturación).7.3.2", _
                                                  "RibbonButton150.Activación de mercancías por clientes.7.3.2", _
                                                  "RibbonMenu24.Compras.7.2.2", _
                                                  "RibbonButton158.Compras a proveedores.7.3.2", _
                                                  "RibbonButton159.Compras a proveedores y activaciones.7.3.2", _
                                                  "RibbonButton160.Compras a proveedores y activaciones mes a mes.7.3.2", _
                                                  "RibbonButton151.Transferencias de mercancías entre almacenes.7.3.2", _
                                                  "RibbonMenu47.Reportes Planos.7.2.2", _
                                                  "RibbonButton279.Ventas por jerarquía.7.3.2", _
                                                  "RibbonButton280.Estructura de costos y ganancias.7.3.2"}

    '//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    Public aMenuArchivosMedicionGerencial() As String = {"RibbonTab8.Medición Gerencial.8.0.0", _
                                                        "RibbonGroup29.Archivos Medición Gerencial.8.1.0", _
                                                        "RibbonButton302.DashBoard.8.2.0", _
                                                        "RibbonButton138.Cuotas Anuales.8.2.0"}

    Public aMenuReportesMedicionGerencial() As String = {"RibbonTab8.Medidición Gerencial.8.0.0", _
                                              "RibbonGroup9.Reportes del Sistema de medición gerencial (SIGME).8.1.2", _
                                              "RibbonMenu39.Contabilidad.8.2.2", _
                                              "RibbonMenu40.Bancos y cajas.8.2.2", _
                                              "RibbonButton252.Ingresos por asesor y formas de pago.8.3.2", _
                                              "RibbonMenu41.Recursos Humanos.8.2.2", _
                                              "RibbonMenu42.Compras y cuentas por pagar.8.2.2", _
                                              "RibbonButton253.Compras comparativas mes a mes.8.3.2", _
                                              "RibbonMenu43.Ventas y cuentas por cobrar.8.2.2", _
                                              "RibbonMenu50.Asesores comerciales.8.3.2", _
                                              "RibbonButton296.Volumen de ventas por asesor comercial.8.4.2", _
                                              "RibbonButton297.Activacion de clientes y mercancías por asesor comercial.8.4.2", _
                                              "RibbonMenu51.Ventas.8.3.2", _
                                              "RibbonButton254.Ventas comprativas mes a mes.8.4.2", _
                                              "RibbonButton255.Ventas mes a mes por asesor.8.4.2", _
                                              "RibbonButton295.Frecuencia devoluciones por causa.8.4.2", _
                                              "RibbonButton261.Activación de clientes mes a mes.8.3.2", _
                                              "RibbonButton262.Drop Size.8.3.2", _
                                              "RibbonButton256.Ganancias brutas en facturación.8.3.2", _
                                              "RibbonButton257.Ganancias brutas por mercancías facturadas.8.3.2", _
                                              "RibbonButton258.Ganancias brutas mes a mes.8.3.2", _
                                              "RibbonButton259.Ganancias por cheques de alimentación en un período.8.3.2", _
                                              "RibbonButton260.Ganancias por cheques de alimentación mes a mes.8.3.2", _
                                              "RibbonButton263.Descuentos otorgados por asesores durante un período.8.3.2", _
                                              "RibbonButton264.Descuentos otorgados por asesores mes a mes.8.3.2", _
                                              "RibbonMenu44.Mercancías.8.2.2", _
                                              "RibbonButton244.Ventas, compras y existencias por jerarquía.8.3.2", _
                                              "RibbonMenu45.Producción.8.2.2"}

    '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Public aMenuArchivosProduccion() As String = {"RibbonTab9.Producción.9.0.0", _
                                                    "RibbonGroup8.Archivos producción.9.1.0", _
                                                    "RibbonButton161.Formulaciones.9.2.0", _
                                                    "RibbonButton162.Ordenes de producción.9.2.0", _
                                                    "RibbonButton163.Seguimientos.9.2.0",
                                                    "RibbonButton164.Costos de Producción.9.2.0"}
    Public aMenuProcesosProduccion() As String = {"RibbonGroup25.Procesos Producción.9.1.1"}

    Public aMenuReportesProduccion() As String = {"RibbonGroup26.Reportes producción.9.1.2"}

    '//////////////////////////////////////////////////////////////////////////////////////////


    Public aMenuArchivosControl() As String = {"RibbonTab10.Control de Gestión.10.0.0",
                                               "RibbonGroup10.Archivos control de gestión.10.1.0",
                                               "RibbonButton165.Empresas.10.2.0",
                                               "RibbonButton166.Calendario de trabajo.10.2.0",
                                               "RibbonButton167.Usuarios.10.2.0",
                                               "RibbonButton168.Mapas de acceso.10.2.0",
                                               "RibbonButton169.Auditorías de acceso.10.2.0",
                                               "RibbonButton170.Impuesto al Valor Agregado.10.2.0",
                                               "RibbonButton171.Retenciones del Impuesto Sobre La Renta.10.2.0",
                                               "RibbonButton203.Unidades Tributarias.10.2.0",
                                               "RibbonButton267.Impresoras fiscales.10.2.0",
                                               "RibbonButton172.Contadores.10.2.0",
                                               "RibbonButton173.Parámetrso del sistema.10.2.0",
                                               "RibbonButton174.Tipos de condiciones de pago y/o cobro a crédito.10.2.0",
                                               "RibbonButton175.Tarjetas de crédito y/o débito.10.2.0",
                                               "RibbonButton176.Tipos de estatus de clientes.10.2.0",
                                               "RibbonButton177.Causas para créditos.10.2.0",
                                               "RibbonButton178.Causas para débitos.10.2.0",
                                               "RibbonButton306.Unidades de Medida.10.2.0",
                                               "RibbonButton179.Divisiones.10.2.0",
                                               "RibbonButton180.Transportes.10.2.0",
                                               "RibbonButton182.Bancos de la plaza.10.2.0",
                                               "RibbonButton183.Agentes de cheques de alimentación.10.2.0",
                                               "RibbonButton184.División política territorial.10.2.0",
                                               "RibbonButton185.Cambios de Moneda.10.2.0",
                                               "RibbonButton312.Monedas.10.2.0",
                                               "RibbonButton197.Conjuntos.10.2.0"}
    Public aMenuProcesosControl() As String = {"RibbonGroup27.Procesos control de gestión.10.1.1", _
                                               "RibbonButton202.Verificar base de datos.10.2.1", _
                                               "RibbonButton205.Cierre de ejercicio.10.2.1", _
                                               "RibbonButton294.Transferencia de datos a históricos.10.2.1", _
                                               "RibbonButton234.Verificación de consecutivos.10.2.1", _
                                               "RibbonButton309.Bloquear módulos por período.10.2.1", _
                                               "RibbonButton310.Desbloquear módulos por períodos.10.2.1", _
                                               "RibbonButton65.Reconversión Monetaria.10.2.1"}

    Public aMenuReportesControl() As String = {"RibbonGroup28.Reportes control de gestión.10.1.2"}
    Public Enum TipoConsultaWEB
        iRIF = 0
        iSADA = 1
    End Enum
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
    Public Enum MesesAtras
        i1 = 1
        i2 = 2
        i4 = 4
        i6 = 6
        i12 = 12
        i24 = 24
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
    Public Enum ProcesoAutomatico

        iActualizacionDePreciosAFuturo = 1
        iBloqueoClientes = 2
        iCierreDiario = 3
        iCierreDiarioDeRutas = 4
        iDiasDeTrabajo = 5

    End Enum
    Public Enum ModuloBonificacion
        iFactura = 0
        iPedido = 1
        iPrePedido = 2
        iPresupuesto = 3
        iNotaEntrega = 4
        iPuntoDeVenta = 5
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
        iPrestamoCuotaNomina = 29
        iMER_CategoriaSADA = 30
        iDenominacionesMonetarias = 31
        iFrasesCintillo = 32
        iCausaDesincorporaciones = 33
        iMER_UbicacionEstantes = 34
        iMER_UnidadesDeMedida = 35
        iMER_Materiales = 36

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
    Public Enum TipoVendedor
        iFuerzaventa = 0
        iCajeros = 1
        iTodos = 2
        iRepuestos = 3
        iServicios = 4
        iVehiculos = 5
        iMecanicos = 6
        iVendedorPiso = 7
        iMesero = 8
    End Enum

    Public Enum ReporteContabilidad

        cCuentasContables = 101
        cPolizaDiferida = 102
        cPolizaActual = 103
        cPolizasActuales = 104
        cMayorAnalitico = 105
        cBalanceComprobacion = 106
        cDiario = 107
        cBalanceGeneral = 108
        cGananciasPerdidas = 109
        cPLantillaAsiento = 110
        cReglaContabilizacion = 111
        cReglasContabilizacion = 112

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
        cNomina = 309
        cResumenXConcepto = 310
        cRecibos = 311
        cFirmas = 312
        cAcumuladosPorConcepto = 313
        cConceptosXTrabajadorMesAMes = 314
        cConceptosMesAMes = 315

    End Enum
    Public Enum ReporteCompras

        cProveedores = 401
        cComprobantePago = 402
        cOrdenDeCompra = 403
        cRecepcion = 404
        cCompra = 405
        cNotaCredito = 406
        cNotaDebito = 407
        cGasto = 408
        cListadoOrdenesDeCompra = 409
        cListadoRecepciones = 410
        cListadoCompras = 411
        cListadoNotasCredito = 412
        cListadoNotasDebito = 413
        cListadoGastos = 414
        cSaldosProveedores = 415
        cEstadodeCuentasProveedores = 416
        cMovimientosProveedores = 417
        cAuditoriasProveedores = 418
        cVencimientos = 419
        cVencimientosResumen = 420
        cLibroIVA = 421
        cFichaProveedor = 422
        cListaRetencionesISLR = 423
        cListaRetencionesIVA = 424
        cRetencionISLR = 425
        cRetencionIVA = 426
        cGrupoSubGrupo = 427
        cListadoDocumentosSinRetencionIVA = 428

    End Enum
    Public Enum ReporteVentas

        cClientes = 501
        cListadoFacturas = 502
        cSaldos = 503
        cPesosPedidos = 504
        cPresupuesto = 505
        cCliente = 506
        cPrePedido = 507
        cListadoPrePedidos = 508
        cListadoPedidos = 509
        cPedido = 510
        cFactura = 511
        cNotaCredito = 512
        cNotaDebito = 513
        cNotaDeEntrega = 514
        cListadoNotasDeEntrega = 515
        cListadoNotasDeCredito = 516
        cListadoNotasDebito = 517
        cListadoPresupuestos = 518
        cEstadoDeCuentas = 519
        cMovimientosClientes = 520
        cMovimientosDocumento = 521
        cAuditoriaClientes = 522
        cVencimientos = 523
        cFacturasMesAMes = 524
        cVencimientosResumen = 525
        cLibroIVA = 526
        cFichaCliente = 527
        cPlanillaCliente = 528
        cActivacionesClientesMercas = 529
        cActivacionesClientes = 530
        cCxC = 531
        cCierreDiario = 532
        cCierreDiarioCT = 533
        cFacturasGuiaDespacho = 534
        cMercanciasGuiaDespacho = 535
        cRelacionFacturas = 536
        cRelacionNotasCredito = 537
        cPedidosSinFacturar = 538
        cChequesDevueltos = 539
        cChequesDevueltosMes = 540
        cVentasPorCliente = 541
        cPresupuestosMesMes = 542
        cPrepedidosMesMes = 543
        cPedidosMesMes = 544
        cNotasEntregaMesMes = 545
        cNotasCreditoMesMes = 546
        cNotasDebitoMesMes = 547
        cVentasClienteDivision = 548
        cVentasPorClienteMesMes = 549
        cCobranzaAnterior = 550
        cCobranzaActual = 551
        cPedidosVsEstatus = 552
        cRanking = 553
        cBackorders = 554
        cGuiaCargaPedidos = 555
        cGuiaPedidos = 556
        cGuiaPedidosMercancias = 557
        cCierreDiarioContado = 558
        cSaldosPorDocumento = 559
        cVentasPorClientesPlus = 560
        cRetencionesIVAClientes = 561
        cCobranzaPlana = 562
        cComisionesPorJerarquia = 563
        cComisionesPorFacturaYJerarquia = 564
        cComisionesPorDiasCobranza = 565
        cComisionesPorDiasCobranzaJerarquia = 566
        cRutasVisita = 567
        cVentasHEINZ = 568
        cPedidosVsEstatusPrepedidos = 569
        cVentasAsesor = 570
        cActivacionAsesor = 571

    End Enum
    Public Enum ReportePuntoDeVenta

        cReporteX = 601
        cReporteZ = 602
        cFacturacionBackEnd = 603
        cReporteXPlus = 604
        cReporteZPlus = 605


    End Enum
    Public Enum ReporteMercancias

        cFichaMercancia = 701
        cCatalogo = 702
        cPrecios = 703
        cPreciosIVA = 704
        cObsolescencia = 705
        cMovimientosMercancia = 706
        cMovimientosMercancias = 707
        cExistenciasAUnaFecha = 708
        cCategorias = 709
        cMarcas = 710
        cTransferencia = 711
        cEquivalencias = 712
        cConteos = 713
        cPreciosFuturos = 714
        cPreciosEspeciales = 715
        cOfertas = 716
        cJerarquias = 717
        cServicios = 718
        cPreciosYEquivalencias = 719
        cVentasDeMercancias = 720
        cInventarioLegal = 721
        cVentasDeMercanciasActivacion = 722
        cVentasDeMercanciasActivacionXMes = 723
        cListadoTransferencias = 724
        cMovimientosResumen = 725
        cComprasDeMercancias = 726
        cComprasdeMercanciasActivacion = 727
        cComprasdeMercanciasActivacionXMes = 728
        cCodigoBarras = 729
        cVentasPlana = 730
        cLeyDeCostos = 731
        cVentasNetasMercancias = 732
        cPedidosAlmacen = 733
        cMovimientosServicios = 734
        cMovimientosServiciosS = 735
        cCodigoBarraUbicacion = 736
        cPreciosYEquivalenciasSinIVA = 737

    End Enum
    Public Enum ReporteMedicionGerencial

        cGananciasPorFactura = 801
        cGananciasPorItem = 802
        cGananciasItemMesMes = 803
        cIngresosPorAsesor = 804
        cComprasComparativasMesMes = 805
        cVentasComparativasMesMes = 806
        cVentasMesMesAsesor = 807
        cActivacionClientesMesMes = 808
        cDropSizeMesMes = 809
        cGananciasCestaTicket = 810
        cGananciasCestaTicketMesMes = 811
        cDescuentosAsesor = 812
        cDescuentosAsesorMesMes = 813
        cVentasComprasExistenciasMesMes = 814
        cDevolucionesAsesorMesMes = 815
        cDevolucionesPorCausa = 816

    End Enum
    Public Enum TipoOrigenNumControl
        iRegistroMaquina = 0
        iContadorDatum = 1
        iImpresoraFiscal = 2
        iContadorJytsuite = 3
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
        iHora = 9
    End Enum
    Public Enum DiferenciaFechas
        iDias = 0
        iMeses = 1
        iAños = 2
        iAñosMeses = 3
        iAñosMesesDias = 4
    End Enum
    Public Enum CondicionPago
        iCredito = 0
        iContado = 1
    End Enum
    Public Enum EstatusFactura
        ePorConfirmar = 0
        eProcesada = 1
        eAnulada = 2
        eApartado = 3
    End Enum
    Public Enum TipoListaPrecios

        Normal = 0
        Costos = 1
        Precios = 2
        Precios_IVA = 3
        CostosSugerido = 4
        CostosTodos = 5
        CostosSugeridoPlus = 6
        CostosFormulaciones = 7
        CostosParaProduccion = 8

    End Enum
    Public Enum TipoDocumento
        Factura = 0
        Giro = 1
        NotaDebito = 2
        Abono = 3
        Cancelacion = 4
        NotaCredito = 5
        RetencionIVA = 7
        RetencionISLR = 8
    End Enum
    Public Enum TipoProveedor
        Compras = 0
        Gastos = 1
    End Enum
    Public Enum FOTipo
        Debito = 0
        Credito = 1
    End Enum
    Public Enum ModoImpresoraRAW
        Draft = 0
        Grafica = 1
    End Enum
    Public Enum TipoImpresoraRAW
        ESCP = 0
        ESCP2 = 1
        IBMPPDS = 2
        OKIML320T = 3
    End Enum
    Public Enum ComandoESCP
        CR = AscW(Chr(13))
        FF = AscW(Chr(12))
    End Enum
   
    Public Enum ImpresoraFiscal
        iFacturaNormal = 0
        iFacturaPreImpresa = 1
        iAclasPPF1F3 = 2
        iBematechMP2100 = 3
        iEpsonPnP_PF220II = 4
        iBixolonSRP350 = 5
        iTallyDascon1125 = 6
    End Enum
    Public ColorDeshabilitado As Color = Color.FromArgb(234, 241, 250)
    Public ColorHabilitado As Color = Color.White
    Public ColorVerdeClaro As Color = Color.PaleGreen
    Public ColorAmarilloClaro As Color = Color.LightGoldenrodYellow
    Public ColorRojoClaro As Color = Color.LightPink

    Public formasDePago As List(Of TextoValor) = New List(Of TextoValor)() From {
            New TextoValor("Efectivo", "EF", 0),
            New TextoValor("Cheque", "CH", 1),
            New TextoValor("Tarjeta", "TA", 2),
            New TextoValor("Cupón de Alimentación", "CT", 3),
            New TextoValor("Depósito", "DP", 4),
            New TextoValor("Transferencia", "TR", 5)
        }

    'Public aFormaPago() As String = {"Efectivo", "Cheque", "Tarjeta", "Cupón de Alimentación", "Depósito", "Transferencia"}

    'Public aFormaPagoAbreviada() As String = {"EF", "CH", "TA", "CT", "DP", "TR"}

    Public aFormaPagoCompras() As String = {"Efectivo", "Cheque", "Tarjeta", "Transferencia"}
    Public aFormaPagoAbreviadaCompras() As String = {"EF", "CH", "TA", "TR"}

    Public aTipoNomina() As String = {"DIARIA", "SEMANAL", "QUINCENAL", "MENSUAL", "ANUAL", "EVENTUAL/TRABAJADOR", "EVENTUAL/GRUPO"}
    Public aCausaExpedienteNomina() As String = {"Ingreso", "Vacaciones", "Permiso remunerado", "Permiso no remunerado", _
                                                 "Reposo médico", "Enfermedad ocupacional", "Ausencia laboral", "Retrasos", _
                                                 "suspención", "Egreso/Desincorporación", "Otros"}

    Public aUnidad() As String = {}
    Public aUnidadAbreviada() As String = {}

    Public aUnidadesIniciales() As String = {"Unidad.UND", "Caja.CAJ", "Bulto.BUL", "Paleta.PAL", "Kilogramo.KGR", _
                                            "Gramo.GRS", "Tonelada.TON", "Litro.LTS", "MiliLitro.MLT", _
                                            "Decilitro.DLT", "Centímetro Cúbico.CC", "Metro Cúbico.MC", "Tambor.TMB", _
                                            "Galón.GAL", "Cuñete.CUÑ", "Docena.DOC", "Pote.POT", "Paquete.PAQ", "Pieza.PZA", _
                                            "Display.DPL", "Estuche.EST", "Rollo.ROL", "Saco.SAC", "Stiva.STI", "Cesta.CES", _
                                            "Tanque.TAN", "Bolsa.BOL", "Centímetro.CM", "Metro.MTR", "Kilómetro.KMR", _
                                            "Hectárea.HEC", "Carrete.CAR", "Otro.OTR"}




    Public aTipoAlmacen() As String = {"Normal", "Tránsito"}
    Public aTipoEstante() As String = {"Gondola", _
                                       "Display", _
                                       "Blister", _
                                       "Mostrador", _
                                       "Vitrina", _
                                       "Nevera", _
                                       "...", _
                                       "...", _
                                       "...", _
                                       "Palé Convencional", _
                                       "Palé Push-Back", _
                                       "Palé Dinámica", _
                                       "Palé Compacta", _
                                       "Palé Autoportante", _
                                       "Cantilever", _
                                       "Picking", _
                                       "Altillos", _
                                       "Comodit", _
                                       "ECO", _
                                       "...", _
                                       "...", _
                                       "...", _
                                       "Archivos Móviles", _
                                       "Archivos Convencionales", _
                                       "Estante"}

    Public aDesde() As String = {"Desde...", "Parecido a..."}
    Public aHasta() As String = {"Hasta...", "Parecido a..."}

    Public aFiscal() As String = {"Factura Normal (FORMA LIBRE)", "Factura pre-impresa fiscal (FORMA FISCAL PRE-IMPRESA)", _
                                  "Aclas PPF1F3", "Bematech MP-2100", "Epson/PnP PF-220-II", _
                                  "Bixolon SRP-350", "Tally Dascon 1125", _
                                  "Bixolon SRP-812"}

    Public aEstatusCliente() As String = {"Activo", "Bloqueado", "Inactivo", "Desincorporado"}
    Public aTarifa() As String = {"A", "B", "C", "D", "F", "E"}
    Public aDias() As String = {"Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"}
    Public aMesesReal() As String = {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", _
                                     "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"}

    Public aTablasReconversion() As String = {"jsbancatban|SALDOACT", "jsbanchedev|monto", _
                                              "jsbandaaban|deb00;deb01;deb02;deb03;deb04;deb05;deb06;deb07;deb08;deb09;deb10;deb11;deb12;cre00;cre01;cre02;cre03;cre04;cre05;cre06;cre07;cre08;cre09;cre10;cre11;cre12", _
                                              "jsbanenccaj|saldo", _
                                              "jsbanordpag|importe", _
                                              "jsbantraban|importe", _
                                              "jsbantracaj|importe", _
                                              "jsbantracon|importe", _
                                              "jscontabret|pagomin;menos", _
                                              "jscotcatcon|deb00;deb01;deb02;deb03;deb04;deb05;deb06;deb07;deb08;deb09;deb10;deb11;deb12;cre00;cre01;cre02;cre03;cre04;cre05;cre06;cre07;cre08;cre09;cre10;cre11;cre12", _
                                              "jscotdaacon|deb00;deb01;deb02;deb03;deb04;deb05;deb06;deb07;deb08;deb09;deb10;deb11;deb12;cre00;cre01;cre02;cre03;cre04;cre05;cre06;cre07;cre08;cre09;cre10;cre11;cre12", _
                                              "jscotencasi|debitos;creditos", _
                                              "jscotrenasi|importe", _
                                              "jsmercatcom|costo", _
                                              "jsmercatser|precio;precio_a;precio_b;precio_c", _
                                              "jsmerconmer|costou;costo_tot", _
                                              "jsmerctainv|precio_A;PRECIO_B;precio_C;precio_D;precio_E;precio_F;montoultimacompra;costo_prom;costo_prom_des;acu_cos;acu_cos_des;acu_cod;acu_cod_des;montoultimaventa;venta_prom;venta_prom_des;acu_pre;acu_pre_des;acu_prd;acu_prd_des", _
                                              "jsmerencped|tot_net;imp_iva;tot_ped", _
                                              "jsmerenctra|totaltra", _
                                              "jsmerivaped|baseiva;impiva", _
                                              "jsmerlispre|monto", _
                                              "jsmerrenconint|costou;totren", _
                                              "jsmerrenlispre|precio", _
                                              "jsmerrenped|costou;costotot", _
                                              "jsmerrentra|costou;totren", _
                                              "jsmertramer|costotal;costotaldes;ventotal;ventotaldes;impiva;descuento", _
                                              "jsnomcattra|sueldo", _
                                              "jsnomencpre|MONTOTAL;SALDO", _
                                              "jsnomestcar|SUELDOBASE", _
                                              "jsnomhisdes|IMPORTE", _
                                              "jsnomrenpre|MONTO;CAPITAL;INTERES", _
                                              "jsnomtrades|IMPORTE", _
                                              "jsposforpag|IMPORTE", _
                                              "jsprocatpro|LIMCREDITO;DISPONIBLE;SALDO;ULTPAGO", _
                                              "jsprodescom|descuento;subtotal", _
                                              "jsprodesgas|descuento;subtotal", _
                                              "jsproenccom|tot_net;descuen;cargos;tot_com;descuen1;descuen2;descuen3;descuen4;abono;interes;imp_iva;imp_ics;ret_iva;ret_islr;base_ret_islr", _
                                              "jsproencgas|tot_net;descuen;cargos;baseiva;imp_iva;imp_ics;ret_iva;tot_gas;abono;interes;ret_islr;base_ret_islr", _
                                              "jsproencncr|tot_net;imp_iva;tot_ncr", _
                                              "jsproencndb|tot_net;imp_iva;tot_ndb", _
                                              "jsproencord|tot_net;imp_iva;tot_ord", _
                                              "jsproencprg|totalprg", _
                                              "jsproencrep|tot_net;imp_iva;tot_rec", _
                                              "jsprohispag|importe;interes;capital", _
                                              "jsproicscom|baseics;impics;retencion", _
                                              "jsproivacom|baseiva;impiva;retencion", _
                                              "jsproivagas|baseiva;impiva;retencion", _
                                              "jsproivancr|baseiva;impiva;retencion", _
                                              "jsproivandb|baseiva;impiva;retencion", _
                                              "jsproivaord|baseiva;impiva;retencion", _
                                              "jsproivarec|baseiva;impiva;retencion", _
                                              "jsprorencom|costou;costotot;costototdes", _
                                              "jsprorendes|descuento", _
                                              "jsprorengas|costou;costotot;costototdes", _
                                              "jsprorenncr|precio;totren;totrendes", _
                                              "jsprorenndb|costo;totren;totrendes", _
                                              "jsprorenord|costou;costotot;costototdes", _
                                              "jsprorenprg|importe;saldo", _
                                              "jsprorenrep|costou;costotot;costototdes", _
                                              "jsprotrapag|importe;poriva", _
                                              "jsprotrapagcan|importe", _
                                              "jsvencatcli|limitecredito;disponible;saldo;ultcobro", _
                                              "jsvencatclidiv|limitecredito;disponible;saldo", _
                                              "jsvencestic|cargos", _
                                              "jsvencobrgv|importe;impiva;interes;capital", _
                                              "jsvencuocob|esmes01;esmes02;esmes03;esmes04;esmes05;esmes06;esmes07;esmes08;esmes09;esmes10;esmes11;esmes12", _
                                              "jsvencxcrgv|importe;impiva;interes;capital", _
                                              "jsvendesapt|descuento;subtotal", _
                                              "jsvendescot|descuento;subtotal", _
                                              "jsvendesfac|descuento;subtotal", _
                                              "jsvendesnot|descuento;subtotal", _
                                              "jsvendesped|descuento;subtotal", _
                                              "jsvendespedrgv|descuento;subtotal", _
                                              "jsvendespos|descuento;subtotal", _
                                              "jsvendesposhis|descuento;subtotal", _
                                              "jsvendevrgv|precio;totren", _
                                              "jsvendivzon|limitecredito;disponible;saldo", _
                                              "jsvenenccie|monto_cd;monto_com_cd;costosventas", _
                                              "jsvenenccot|tot_net;descuen;cargos;Imp_iva;tot_cot", _
                                              "jsvenencfac|tot_net;descuen;descuen1;descuen2;descuen3;descuen4;cargos;cargos1;cargos2;cargos3;cargos4;tot_fac1;tot_fac2;tot_fac3;tot_fac4;abono;interes;baseiva;Imp_iva;imp_ics;tot_fac", _
                                              "jsvenencgui|totalguia", _
                                              "jsvenencguipedidos|totalguia", _
                                              "jsvenencncr|tot_net;Imp_iva;imp_ics;tot_ncr", _
                                              "jsvenencncrrgv|tot_net;Imp_iva;imp_ics;tot_ncr", _
                                              "jsvenencndb|tot_net;Imp_iva;tot_ndb", _
                                              "jsvenencnot|tot_net;descuen;descuen1;descuen2;descuen3;descuen4;cargos;cargos1;cargos2;cargos3;cargos4;tot_fac1;tot_fac2;tot_fac3;tot_fac4;abono;interes;baseiva;Imp_iva;imp_ics;tot_fac", _
                                              "jsvenencped|tot_net;descuen;descuen1;descuen2;descuen3;descuen4;cargos;Imp_iva;tot_ped;abono;interes", _
                                              "jsvenencped_ven|tot_net;descuen;descuen1;descuen2;descuen3;descuen4;cargos;Imp_iva;tot_ped;abono;interes", _
                                              "jsvenencpedrgv|tot_net;descuen;descuen1;descuen2;descuen3;descuen4;cargos;Imp_iva;tot_ped;abono;interes", _
                                              "jsvenencpos|tot_net;descuen;cargos;Imp_iva;tot_fac;monto_retencion_iva", _
                                              "jsvenencposhis|tot_net;descuen;cargos;Imp_iva;tot_fac;monto_retencion_iva", _
                                              "jsvenencrel|totalguia", _
                                              "jsvenforpag|importe", _
                                              "jsvenforpaghis|importe", _
                                              "jsvenforpagrgv|importe", _
                                              "jsvenhiscob|importe;impiva;interes;caPITAL", _
                                              "jsvenicsfac|baseics;impics", _
                                              "jsvenicsncr|baseics;impics", _
                                              "jsvenicsncrrgv|baseics;impics", _
                                              "jsveninipos|montoinicio", _
                                              "jsvenivacot|baseiva;impiva", _
                                              "jsvenivafac|baseiva;impiva", _
                                              "jsvenivancr|baseiva;impiva", _
                                              "jsvenivancrrgv|baseiva;impiva", _
                                              "jsvenivandb|baseiva;impiva", _
                                              "jsvenivanot|baseiva;impiva", _
                                              "jsvenivaped|baseiva;impiva", _
                                              "jsvenivapedrgv|baseiva;impiva", _
                                              "jsvenivapos|baseiva;impiva", _
                                              "jsvenivaposhis|baseiva;impiva", _
                                              "jsvenpedrgv|precio;totren;totrendes", _
                                              "jsvenrencie|costos;ventas;totalpedido;totaldev", _
                                              "jsvenrencot|precio;totren;totrendes", _
                                              "jsvenrenfac|precio;totren;totrendes", _
                                              "jsvenrengui|totalfac", _
                                              "jsvenrenguipedidos|totalfac", _
                                              "jsvenrenncr|precio;totren;totrendes", _
                                              "jsvenrenncrrgv|precio;totren;totrendes", _
                                              "jsvenrenndb|precio;totren;totrendes", _
                                              "jsvenrennot|precio;totren;totrendes", _
                                              "jsvenrenped|precio;totren;totrendes", _
                                              "jsvenrenpedrgv|precio;totren;totrendes", _
                                              "jsvenrenpos|precio;totren;totrendes", _
                                              "jsvenrenposhis|precio;totren;totrendes", _
                                              "jsvenrenrel|totalfac", _
                                              "jsvensalcli|limitecredito;disponible;saldo", _
                                              "jsventabtic|monto;comision", _
                                              "jsventracob|importe;impiva;interes;capital", _
                                              "jsventracobcan|importe", _
                                              "jsventrapos|importe", _
                                              "jsventraposhis|importe"}



End Module
