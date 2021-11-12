using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.Types;
using MySql.Data.MySqlClient; 
using System.Windows.Forms;
using System.Drawing;
using System.IO.Ports;
using System.Management;  
using System.Media;
using System.Net.NetworkInformation; 
using System.Reflection;
using Microsoft.Win32;
using C1.Win.C1SuperTooltip;

namespace fTransport
{
    public class Transportables
    {

        private const string NombreSistema = "DATUM";
        
        public const string cFormatoEntero = "#,##0";
        public const string cFormatoNumero = "#,##0.00";
        public const string cFormatoCantidad = "#,##0.000";
        public const string cFormatoCantidadLarga = "#,##0.00000";
        
        public const string cFormatoFecha = "dd-MM-yyyy";
        public const string cFormatoFechaHora = "dd-MM-yyyy hh:mm:ss";
        public const string cFormatoFechaISLR = "dd/MM/yyyy";
        public const string cFormatoFechaMySQL = "yyyy-MM-dd";
        public const string cFormatoHora = "hh:mm:ss";
        public const string cFormatoHoraCorta = "HH:mm";

        public const string cFormatoPorcentaje = "0.00";
        public const string cFormatoPorcentajeLargo = "0.0000";

        public const string DirReg = "HKEY_CURRENT_USER\\SOFTWARE\\Tecnologías Jytsuite\\Jytsuite\\CurrentVersion\\"; 
        public const string cClaveAleatoria = "SemillaAleatoria";

        public enum lado { izquierdo, derecho };

        public enum tipoMensaje { iNinguno, iError, iInfo, iAdvertencia, iAyuda };
        public enum usuarioPuede{ iIncluir, iModificar, iEliminar };

        public enum idioma { iEspañol, iEnglish };

        public enum tipoDato{ Cadena, Entero, Cantidad, Numero, Fecha, Hora }

        private string[] aLabelsText = {"lblUsuario.Usuario|User", 
                                       "lblPassword.Clave|Password",
                                       "lblDescripcionSistema.Sistema Contable, administrativo y gerencial|Accounting, Administrative and Management System",
                                       "lblCodigo|lblCode|lblCodigo1.Código|ID Code",
                                       "lblServidor.Servidor ó IP del Host|Server or Host IP", 
                                       "lblEstatus|lblStatus.Estatus|Status",
                                       "lblGrupo.Grupo|Group",
                                       "lblUbica|lblUbicacion.Ubicacion|Ubication",
                                       "lblBaseDatos.Base de datos|Database",
                                       "lblDescripcion.Descripción|Description",
                                       "lblNombre.Nombre|Name",
                                       "lblIngreso.Ingreso|Registration",
                                       "lblMoneda.Moneda|Currency",
                                       "lblTasa.Tasa|Rate", 
                                       "lblSerial.Serial|Serial",
                                       "lblVida.Vida útil (Años/Meses)|Useful life (Years/Months)",
                                       "lblTipo.Tipo|Type", 
                                       "lblMetodo.Método|Method",
                                       "lblAdquisicion.Adquisición|Acquisition",
                                       "lblMontoAdquisicion.Monto adquisicion|Acquisition amount", 
                                       "lblSalvamento.Monto salvamento|Recovery amount",
                                       "lblValuacion.Fecha valuación|Value date",
                                       "lblTituloValuacion.Valuación|Valuation",
                                       "lblTituloCuentas.Cuentas|Accounts",
                                       "lblActivoPasivo.Activos/Pasivos|Assets/Liabilities",
                                       "lblGastosIngresos.Gastos/Ingresos|Expenses/Incomes", 
                                       "lblRepreciacion.Repreciacion acumulada|Accumulated repricing",
                                       "btnAceptar|btnOK.Aceptar|Acept", 
                                       "btnCancelar|btnCancel.Cancelar|Cancel"
                                      };

        private string[] aBoxMessages = {
                                        "txtCodigo|txtCodigo1.Indique un código o ID|Enter ID or code",
                                        "txtAlterno.Indique un código Alterno|Enter alternate code",
                                        "txtBarras|txtBarraA|txtBarraB|txtBarraC|txtBarraD|txtBarraE|txtBarraF.Indique un código de Barras|Enter bar code",
                                        "txtDescripcion|txtNombre.Indique un Nombre ó Descripción|Enter a name or description", 
                                        "txtPresentacion.Indique presentación de mercancías (ej 12x185g = 12 unidades de 185 gramos)|Merchandise presentation (ex 12x185oz = 12 units of 185 ounces)",
                                        "txtSugerido.Indique precio de venta sugerido por proveedor|Enter provider suggested price", 
                                        "txtPorDevoluciones.Indique porcentaje aceptación en devolución (ej 70% = se devuelve por 70 por ciento del precio con el que fue vendida|Enter acceptance percentage in return (ex 70% means it's returned for 70 percent of the price with which it was sold)",
                                        "txtExMin.Indique existencia mínima de esta mercancía|Enter minimal existence",
                                        "txtExMax.Indique existencia máxima de esta mercancía|Enter maximal existence", 
                                        "txtPesoUnidad.Indique el peso de la unidad de venta|Enter sale unit's weight", 
                                        "txtAlto.Indique el alto de la mercancía|Enter merchandise's high",
                                        "txtAncho.Indique el ancho de la mercancía|Enter merchandise's width",
                                        "txtProfun.Indique el profundidad de la mercancía|Enter merchandise's depth", 
                                        "txtPrecioA|txtPrecioB|txtPrecioC|txtPrecioD|txtPrecioE|txtPrecioF.Indique precio de venta de mercancía|Enter sale's price",
                                        "txtDescA|txtDescB|txtDescC|txtDescD|txtDescE|txtDescF.Indique descuento de venta por precio de la mercancía|Enter sale's discount",
                                        "txtGanA|txtGanB|txtGanC|txtGanD|txtGanE|txtGanF.Indique la ganancia por precio de la mercancía|Enter price's profit", 
                                        "txtCtaBan.Indique número de cuenta bancaria|Enter bank account number", 
                                        "txtAgencia.Indique un Nombre de Agencia|Enter a Agency office name", 
                                        "txtDireccion.Indique Dirección completa|Enter an Address", 
                                        "txtTelefono|txtTelef2|txtTelef3|txtTelef1|txtTelef.Indique un número telefónico|Enter a telephone number", 
                                        "txtFAX|txtFax.Indique un Número de FAX|Enter a FAX number", 
                                        "txtEmail.Indique una dirección de correo electrónico|Enter an email address", 
                                        "txtWEB|txtWeb.Indique una URL del web site|Enter a Web site URL", 
                                        "txtContacto.Indique el nombre de la persona Contacto|Enter a contact name", 
                                        "btnFormatos.Seleccione un formato de cheque|Select a check format", 
                                        "txtAdquisicion.Indique o seleccione una fecha de adquisicion|Select or enter an acquisition date", 
                                        "txtInicioValuacion.Indique o seleccione una fecha Inicio de Valuaciones|Select or enter a date when valuations began", 
                                        "txtIngreso|btnIngreso.Indique o seleccione una fecha de Ingreso|Select or enter date of admission", 
                                        "txtAdquisicion|btnFechaAdquisicion.Indique o seleccione una fecha de Adquisicion|Select or enter date of acquisition", 
                                        "txtInicioValuacion|btnInicioValuacion.Indique o seleccione una fecha Inicio de Valuaciones|Select or enter valuation start date", 
                                        "txtFecha|btnFecha.Indique o seleccione una fecha|Select or enter date", 
                                        "txtSerial.Indique un Número Serial|Enter a Serial Number", 
                                        "txtGrupo|btnGrupo.Indique un codigo de grupo|Enter a group code",
                                        "txtMoneda|btnMoneda.Indique un tipo de moneda|Indicate a coin type",
                                        "txtUbicacion|txtUbica1|txtUbica2|txtUbica3|btnUbicacion|btnUbica.Indique una ubicación|Enter an ubication", 
                                        "txtAdquisicionMonto|txtSaldo.Indique un monto o cantidad|Enter an amount or quantity ", 
                                        "txtAños.Indique cantidad en años|Indicate quantity of years",
                                        "txtMeses.Indique cantidad en meses|Indicate quantity of months", 
                                        "txtTasa|txtComision.Indique tasa o porcentaje (número entre Cero (0) y cien (100)|Indicate percent (%) number (0 to 100) ", 
                                        "btnCuentaContable|btnCtaCon|btnCuentaActivos|btnCuentaGastos|btnCuentaRepreciacion|txtCuentaContable|txtCodCon.Seleccione o indique una cuenta contable|Select or Enter an Accounting Account", 
                                        "cmbCondicion|cmbEstatus|cmbTipo|cmbMetodo.Seleccione un opción|Select an option"}; 
         
        private string[] aToolTips = {"txtBarras.<B>Indique o Escaneé</B> un código de barras válido|<B>Type or Scan</B> a valid bar code", 
                                      "btnPor.<B>Multiplicador</B> de cantidades en renglón|row quantity <B>Multiplier</B>", 
                                      "btnAgregarMovimiento.<B>Agrega renglón</B> de forma manual|<B>Add</B> row", 
                                      "btnAgregarServicio.<B>Agrega renglón de servicios</B> de forma manual|<B>Add Service</B> row", 
                                      "btnEditarMovimiento.<B>Edita</B> el renglón actual|<B>Edit</B> actual row", 
                                      "btnEliminarMovimiento.<B>Elimina</B> el renglon actual|<B>Delete</B> actual row",
                                      "btnDescuentosRenglon.<B>Asigna descuentos</B> en el renglón actual|<B>Add discount</B> actual row", 
                                      "btnPrimerMovimiento.ir al <B>primer renglón</B>|<B>first</B> row",
                                      "btnAnteriorMovimiento.ir al renglón <B>anterior</B>|<B>previous</B> row", 
                                      "btnSiguienteMovimiento.ir al renglón <B>siguiente</B>|<B>next</B> row", 
                                      "btnUltimoMovimiento.ir al <B>último</B> renglon|<B>last</B> row",
                                      "btnEliminarFactura.<B>Eliminar</B> documento actual|<B>Delete</B> actual document",
                                      "btnNotas.<B>Baja renglones</B> de una o varias notas de entrega|", 
                                      "btnCliente.seleccionar, escojer y/o buscar <B>cliente</B>|Select <B>customer</B>", 
                                      "btnProveedor.seleccionar, escojer y/o buscar <B>proveedor</B>|Select <B>provider</B>",
                                      "btnVendedor|btnAsesor.<B>escojer y/o buscar</B> vendedor|Select <B>Commercial Advisor</B>", 
                                      "btnDescuentosFactura.<B>Asignar descuentos</B> por factura|<B>Add discount</B> actual document", 
                                      "btnIVA.<B>Verificar</B> impuesto al valor agregado (IVA) de este documento|Select <B>TAX</B> sale",
                                      "btnAgregar.<B>Agregar</B> nuevo registro|<B>Add</B> new record", 
                                      "btnEditar.<B>Editar o mofificar</B> registro actual|<B>Edit</B> actual record", 
                                      "btnEliminar.<B>Eliminar</B> registro actual|<B>Delete</B> actual record", 
                                      "btnBuscar.<B>Buscar</B> registro deseado|<B>Search</B> wanted record", 
                                      "btnSeleccionar.<B>Seleccionar</B> registro actual|<B>Select</B> actual record", 
                                      "btnPrimero.ir al <B>primer</B> registro|<B>First</B> record", 
                                      "btnSiguiente.ir al <B>siguiente</B> registro|<B>Next</B> record", 
                                      "btnAnterior.ir al registro <B>anterior</B>|<B>Previous</B> record", 
                                      "btnUltimo.ir al <B>último registro</B>|<B>Last</B> record", 
                                      "btnImprimir.<B>Imprimir</B>|<B>Print</B> ", 
                                      "btnSalir.<B>Cerrar</B> esta ventana|<B>Close</B> this window", 
                                      "btnRecalcular.<B>Recalcular</B>|<B>Recalculate</B>", 
                                      "btnCategoria.Seleccione la <B>Categoría</B>|Select <B>Category</B>", 
                                      "btnMarca.Seleccione la <B>marca</B>|Select <B>Brand</B>", 
                                      "btnDivision.Seleccione la <B>Division</B>|Select <B>Divition</B>",
                                      "btnJerarquia|btnTipJer.Seleccione la <B>jerarquía</B>|Select <B>hierarchy</B>", 
                                      "btnFabricante.Seleccione la <B>Fabricante</B>|Select <B>Manufacturer</B>", 
                                      "btnFactura.<B>Seleccionar factura</B> de la cual proviene la devolución|Select <B>Bill</B> for devolution",
                                      "btnCantidadTC.permite <B>Seleccionar</B> las cantidades en tallas y colores de la mercancía|Select <B>size & color</B> of the item",
                                      "btnMaterial.Seleccione la <B>Material</B>|Select <B>Material</B>", 
                                      "btnFechaAdquisicion|btnFechaRevision|btnFechaFabricacion|btnIngreso|btnEmision|btnEmisionIVA|btnVence|btnVencimiento|btnFecha|btnInicioVisita|btnFechaNacimiento|btnFechaTurno|btnFechaDiaLibre|btnInicioValuacion.Seleccione una <B>Fecha</B>|Select a <B>date</B>", 
                                      "btnLote.Seleccione el número de <B>Lote</B>|Select <B>Lot</B> number", 
                                      "btnAlmacen.seleccionar, escojer y/o buscar <B>almacén</B>|Select <B>warehouse</B>", 
                                      "btnCodigoArticulo|btnMercancia.seleccionar, escojer y/o buscar <B>artículo/mercancía</B>|Select <B>item</B>", 
                                      "btnComentarioAdicional.Agregar <B>comentario adicional</B> a la descripción|Additional <B>comment</B>",
                                      "btnTalla.seleccionar, escojer y/o buscar <B> tallas </B>|Select <B>size</B>",
                                      "btnColor.seleccionar, escojer y/o buscar <B> color</B>|Select <B>color</B>", 
                                      "btnSICA.seleccionar, escojer y/o buscar <B> código SICA </B>|Select SICA code", 
                                      "btnCEP.seleccionar, escojer y/o buscar <B>código CEP</B>|",
                                      "btnSACS.seleccionar, escojer y/o buscar <B>código SACS</B>|", 
                                      "btnFoto.seleccionar, escojer y/o buscar <B>Foto</B>|Add <B>picture</B>", 
                                      "btnCombo.Mercancías del <B>COMBO</B>|<B>COMBO</B> items", 
                                      "btnRecalculaPesos.<B>Recalcula pesos</B> de todos los movimientos|<B>Recalculate</B> weight", 
                                      "btnSubirHistorico.<B>Subir</B> movimientos a histórico|<B>upload</B> historic", 
                                      "btnBajarHistorico.<B>Bajar</B> movimientos de histórico|<B>download</B> historic" , 
                                      "btnRIF.<B>Consultar RIF</B> en página del seniat|",
                                      "btnSADA.<B>Consultar SADA</B> en la página del SICA|",
                                      "btnTablaSADA.<B>Ver códigos SADA</B> en archivo|" ,
                                      "btnCanal.Seleccione la <B>canal de distribución y tipo de negocio</B>|Select <B>Distribution Channel/Bussines type</B> ", 
                                      "btnTerritorioFiscal.Seleccione el <B>Territorio de la dirección fiscal</B>|Select <B>Fiscal Territory</B> ", 
                                      "btnTerritorioDespacho.Seleccione la <B>Territorio de la dirección de despacho</B>|Select <B>Delivery Territory</B> ", 
                                      "btnForma.Seleccione la <B>forma de pago</B>|Select <B>pay</B>", 
                                      "btnListaPrecios.Seleccione una <B>lista de precios </B>|Select <B>price</B> list", 
                                      "btnAuditoria.Consulta <B>Auditoría </B> de este módulo|Check module <B>audit</B>", 
                                      "btnReprocesar.<B>Reprocesar </B> movimientos|<B>Reprocess</B>", 
                                      "btnExP.<B>Transferir</B> CxP a ExP/ExP a CxP|<B>Transfer</B> ", 
                                      "btnUnidad.Seleccione la <B>Unidad y categoría de negocio</B>|Select bussines <B>Unit & Category</B>", 
                                      "btnUbica|btnUbicacion.Seleccione la <B>Ubicación</B>|Select <B>Location</B>", 
                                      "btnBanco|btnBancoDeposito.Seleccione un <B>Banco</B>|Select <B>Bank</B>", 
                                      "btnZona.Seleccione la <B>Zona DDN</B>|Select DDN <B>Zone</B>", 
                                      "btnDepositarCestaTicket.Depositar remesas de <B>cheques de alimentación</B>|<B>Food Stamps</B> deposit",
                                      "btnDepositarEfectivo.Depositar <B>efectivo y/o cheques</B>|<B>Cash & Checks</B> deposit", 
                                      "btnDepositarTarjetas.Depositar <B>tarjetas</B> de crédito y/o débito|<B>Debit & Credit Cards</B> deposit", 
                                      "btnReposicion.<B>Reposición</B> de saldo en cajas|<B>Balance reposition</B> in Cash",
                                      "btnCodigoContable|btnCuentaContable|btnCuentaActivos|btnCuentaGastos|btnCuentaRepreciacion.seleccione una <B>Cuenta Contable</B>|Select <B>Accounting Account</B>", 
                                      "btnFormatos.Seleccione y/o incluya los <B>Fornmatos</B> correspondientes|Select <B>formats</B>",
                                      "btnRemesas.Construcción de <B>remesas</B> de cheques de alimentación|Construction of <B>Food Stamps</B> deposit", 
                                      "btnAdelantoEfectivo.Adelanto, cambio ó intercambio de <B>Efectivo</B> por tarjetas y/o cheques|Cash <B>advances</B>",
                                      "btnDuplicar.<B>Duplica</B> este documento|<B>Duplicate</B> this document",
                                      "btnRecepciones. Trae <B>recepciones</B> a este documento|<B>Receptions</B>", 
                                      "btnOrdenes.Trae <B>órdenes</B> a este documento|<B>Orders</B>", 
                                      "btnAgregaDescuento. <B>agrega descuentos</B> a este documento|<B>Add</B> discount",
                                      "btnEliminaDescuento. <B>elimina descuentos</B> de este documento|<B>Delete</B> discount", 
                                      "btnProcesarConceptos.<B>Re-Procesar</B> conceptos a trabajadores|<B>Reprocess</B> concepts to workers", 
                                      "btnReprocesarEmpleado.<B>Re-Procesar</B> conceptos de trabajador|<B>Reprocess</B> concepts of worker", 
                                      "btnAgregaTurno. <B>Agregar</B> turno|<B>Add</B> time schedule",
                                      "btnEliminaTurno.<B>Elimina</B> turno|<B>Delete</B> time schedule", 
                                      "btnGrupo|btnG1|btnG2|btnG3|btnG4|btnG5|btnG6.<B>Seleccionar</B> grupo y/o subgrupo|Select <B>Group & subgroup</B>",
                                      "btnReferescar.<B>Actualizar</B> vista de este módulo|<B>Refresh</B> view", 
                                      "btnPesoCaptura.<B>Captura</B> el peso desde una balanza conectada al sistema|<B>get</B> the weight from the scale", 
                                      "btnCantidadTC.<B>Indicar la cantidad indicada</B> por tallas y colores|Indicate <B>size & color</B> quantities",
                                      "btnCausa.selecciona la<B> causa</B> de la devolución|Select <B>devolution cause</B>",
                                      "btnAActual.Transferir asiento de diferido a <B>ACTUAL</B>|<B>Transfer</B> deferred policies to actual policies ",
                                      "btnDeActual.revertir asiento de actual a <B>DIFERIDO|</B><B>reverse</B> actual policies to deferred policies ",
                                      "btnReconstruir.<B>RECONSTRUIR</B> o refrescar información|<B>Rebuild</B> information",
                                      "btnSubir.<B>SUBIR</B> en el orden de precedencia|go <B>UP</B>",
                                      "btnBajar.<B>BAJAR</B> en el orden de precedencia|go <B>DOWN</B>",
                                      "lblBuscar.Está buscando por"
                                     };

        private string[] aComboOptions = {"cmbEstatus|cmbCondicion.Inactivo|Inactive;Activo|Active",
                                          "cmbTipoActivo.Depreciable|Depreciable;No depreciable|Non Depreciable;Amortizable|Redeemable;Realizable|Realizable", 
                                          "cmbMetodoValuacion.Linea recta|Straight Line;Progresivo creciente|Double Declining Balance;Progresivo decreciente|Declining Balance", 
                                          "cmbTitulo.Sr|Mr;Sra|Mrs;Srta|Miss"};
                                      
        #region Base de Datos MySQL
        public DataSet DataSetRequeryPlus(DataSet rDataset, string nTabla, MySqlConnection MyConn, string strSQL)
        {

            rDataset.EnforceConstraints = false;

            using (MySqlDataAdapter nDataAdapter = new MySqlDataAdapter())
            {

                if (rDataset.Tables[nTabla] != null)
                {
                    rDataset.Tables[nTabla].Clear();
                }

                try
                {
                    nDataAdapter.SelectCommand = new MySqlCommand(strSQL, MyConn);
                    nDataAdapter.SelectCommand.CommandTimeout = 0;
                    nDataAdapter.Fill(rDataset, nTabla);

                }
                catch (MySqlException ex)
                {
                    mensajeCritico(ex.Message + ". Error base de datos // " + strSQL );
                }
            }

            return rDataset;

        }
        public DataTable AbrirDataTable(DataSet rDataset, string nTabla, MySqlConnection MyConn, string strSQL)
        {
            return DataSetRequeryPlus(rDataset, nTabla, MyConn, strSQL).Tables[nTabla];
        }
        public bool Ejecutar_strSQL(MySqlConnection MyConn, string strSQL)
        {

            try
            {
                using (MySqlCommand mycomm = new MySqlCommand())
                {
                    mycomm.Connection = MyConn;
                    mycomm.CommandText = strSQL;
                    mycomm.CommandTimeout = int.MaxValue; 
                    mycomm.ExecuteNonQuery();
                    return true;
                }
            }
            catch (MySqlException ex)
            {
                mensajeCritico(ex.Number + " Error Base de Datos : " + ex.Message + " // " + strSQL);
                return false;
            }
        }
        public object Ejecutar_strSQL_DevuelveScalar(MySqlConnection MyConn, string strSQL)
        {

            MySqlCommand mycomm = new MySqlCommand();
            object obj = 0;
            try
            {
                mycomm.Connection = MyConn;
                mycomm.CommandText = strSQL;
                mycomm.CommandTimeout = int.MaxValue;  
                obj = mycomm.ExecuteScalar();
                //if (obj == null) { return 0; }
            }
            catch (MySqlException ex)
            {
                mensajeCritico("Error Base de Datos : " + ex.Message);
            }
            finally
            {
                mycomm.Dispose();
                mycomm = null;
            }
            return obj;
        }
        public string DevuelveScalarCadena(MySqlConnection MyConn, string strSQL)
        {
            strSQL = reconstruirCadenaSQLADevolver(strSQL, "''");
            return Convert.ToString(Ejecutar_strSQL_DevuelveScalar(MyConn, strSQL));
        }
        public double DevuelveScalarDoble(MySqlConnection MyConn, string strSQL){
            strSQL = reconstruirCadenaSQLADevolver(strSQL, "0");
            return Convert.ToDouble(Ejecutar_strSQL_DevuelveScalar(MyConn, strSQL));}
        public Int32 DevuelveScalarEntero(MySqlConnection MyConn, string strSQL){
            strSQL = reconstruirCadenaSQLADevolver(strSQL, "0");
            return Convert.ToInt32(Ejecutar_strSQL_DevuelveScalar(MyConn, strSQL));}
        public Boolean DevuelveScalarBooleano(MySqlConnection MyConn, string strSQL){
            strSQL = reconstruirCadenaSQLADevolver(strSQL, "0");
            return Convert.ToBoolean(Convert.ToInt16(Ejecutar_strSQL_DevuelveScalar(MyConn, strSQL)));}
        public DateTime DevuelveScalarFecha(MySqlConnection MyConn, string strSQL){
            Object fecha =  Ejecutar_strSQL_DevuelveScalar(MyConn, strSQL); 
            return Convert.ToDateTime( (fecha == null ? "2016-01-01" : fecha));}

        public ArrayList Ejecutar_strSQL_DevuelveLista(MySqlConnection MyConn, string strSQL)
        {


            ArrayList obj = new ArrayList();

            try
            {
                using (MySqlCommand myComm = new MySqlCommand())
                {
                    myComm.Connection = MyConn;
                    myComm.CommandText = strSQL;
                    myComm.CommandTimeout = int.MaxValue;  
                    MySqlDataReader nReader;
                    nReader = myComm.ExecuteReader();
                    if (nReader.HasRows)
                    {
                        while (nReader.Read())
                        {
                            obj.Add(nReader.GetValue(0));
                        }
                    }

                    nReader.Close();
                    nReader = null;
                }

            }
            catch (MySqlException ex)
            {
                mensajeCritico("Error Base de Datos : " + ex.Message);
            }

            return obj;
        }

        public string reconstruirCadenaSQLADevolver(string strSQL, string obj){

            string[] str = strSQL.Split(' ');
            string strTemp = "";
            int iCont = 0;
            while (str[iCont].ToUpper() != "FROM") {
                if (str[iCont].ToUpper() == "SELECT")
                {
                    strTemp += " " + str[iCont].ToUpper();
                }else{
                    strTemp += " " + str[iCont];
                }
                iCont++;
            }
            
            strTemp = " IFNULL(" + strTemp.Replace("SELECT", "") + ", " + obj  +" )";

            while (iCont <= str.Length - 1) {
                strTemp += " " + str[iCont];  
                iCont++; 
            } 
            return " SELECT " + strTemp;
        }
        
        #endregion
        #region Tablas y Grillas; Menus

        public void IniciarTablaPlus(DataGridView dg, DataTable dt, string[] aCampos, bool Encabezado = true,
            bool EditaCampos = false, Font fnt = null , bool EncabezadoDeFila = true, int AltoDeFila = 18,
            bool AsignaDataSource = true, bool SeleccionSimple = true, DataGridViewColumn lastCol  = null)
        {

            dg.Columns.Clear();
            dg.AutoGenerateColumns = false;
            dg.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.AliceBlue;
            dg.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dg.RowHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkBlue;
            if (fnt == null) { fnt = new Font("Consolas", 9, FontStyle.Regular );};
            dg.RowsDefaultCellStyle.Font = fnt;
            dg.ColumnHeadersVisible = Encabezado;
            dg.RowHeadersVisible = EncabezadoDeFila;

            dg.AllowUserToAddRows = false;
            dg.RowTemplate.Height = AltoDeFila;
            dg.RowHeadersWidth = 25;

            if (EditaCampos){
                dg.SelectionMode = DataGridViewSelectionMode.CellSelect;
            }else{
                dg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            dg.MultiSelect = false;
            dg.EditMode = ((EditaCampos) ? DataGridViewEditMode.EditOnKeystrokeOrF2 : DataGridViewEditMode.EditProgrammatically);

            if (!SeleccionSimple)
            {
                using (DataGridViewCheckBoxColumn colCero = new DataGridViewCheckBoxColumn())
                {
                    colCero.Name = "sel";
                    colCero.HeaderText = "";
                    colCero.DataPropertyName = "sel";
                    colCero.Width = 20;
                    dg.Columns.Add(colCero);
                    dg.MultiSelect = true;
                }
            }

            for (int iCont = 0; iCont < aCampos.Length; iCont++)
            {

                if (aCampos[iCont] != null)
                {

                    using (DataGridViewColumn dgCol = new DataGridViewColumn())
                    {
                        DataGridViewCell dgCell = new DataGridViewTextBoxCell();

                        dgCol.CellTemplate = dgCell;

                        dgCol.Name = aCampos[iCont].Split('.')[0];
                        dgCol.HeaderText = aCampos[iCont].Split('.')[1];
                        dgCol.DataPropertyName = aCampos[iCont].Split('.')[0];
                        dgCol.Width = Convert.ToInt32(aCampos[iCont].Split('.')[2]);
                        dgCol.Resizable = DataGridViewTriState.False;
                        dgCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        
                        Font headerFont = new Font(fnt.FontFamily , fnt.Size , FontStyle.Bold);
                        dgCol.HeaderCell.Style.Font = headerFont; 

                        dgCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                        dgCol.DefaultCellStyle.Alignment = (aCampos[iCont].Split('.')[3] == "I" ? DataGridViewContentAlignment.MiddleLeft :
                            (aCampos[iCont].Split('.')[3] == "C" ? DataGridViewContentAlignment.MiddleCenter : DataGridViewContentAlignment.MiddleRight));
                        dgCol.DefaultCellStyle.Format = "";
                        switch (aCampos[iCont].Split('.')[4])
                        {
                            case "Entero":
                                dgCol.DefaultCellStyle.Format = cFormatoEntero;
                                break;
                            case "Numero":
                                dgCol.DefaultCellStyle.Format = cFormatoNumero;
                                break;
                            case "Cantidad":
                                dgCol.DefaultCellStyle.Format = cFormatoCantidad;
                                break;
                            case "Fecha":
                                dgCol.DefaultCellStyle.Format = cFormatoFecha;
                                break;
                            case "FechaHora":
                                dgCol.DefaultCellStyle.Format = cFormatoFechaHora;
                                break;
                            case "Hora":
                                dgCol.DefaultCellStyle.Format = cFormatoHoraCorta;
                                break;
                        }
                        if (iCont == aCampos.Length - 1) { dgCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; }

                        dg.Columns.Add(dgCol);
                    }

                }
            }

            if (lastCol != null){dg.Columns.Add(lastCol);}

            if (AsignaDataSource){dg.DataSource = dt;}

        }

        public void IniciarTablaList<T>(DataGridView dg, List<T> list, string[] aCampos, bool Encabezado = true,
            bool EditaCampos = false, Font fnt = null, bool EncabezadoDeFila = true, int AltoDeFila = 18,
            bool AsignaDataSource = true, bool SeleccionSimple = true, DataGridViewColumn lastCol = null)
        {

            dg.Columns.Clear();
            dg.AutoGenerateColumns = false;
            dg.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.AliceBlue;
            dg.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dg.RowHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkBlue;
            if (fnt == null) { fnt = new Font("Consolas", 9, FontStyle.Regular); };
            dg.RowsDefaultCellStyle.Font = fnt;
            dg.ColumnHeadersVisible = Encabezado;
            dg.RowHeadersVisible = EncabezadoDeFila;

            dg.AllowUserToAddRows = false;
            dg.RowTemplate.Height = AltoDeFila;
            dg.RowHeadersWidth = 25;

            if (EditaCampos)
            {
                dg.SelectionMode = DataGridViewSelectionMode.CellSelect;
            }
            else
            {
                dg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            dg.MultiSelect = false;
            dg.EditMode = ((EditaCampos) ? DataGridViewEditMode.EditOnKeystrokeOrF2 : DataGridViewEditMode.EditProgrammatically);

            if (!SeleccionSimple)
            {
                using (DataGridViewCheckBoxColumn colCero = new DataGridViewCheckBoxColumn())
                {
                    colCero.Name = "sel";
                    colCero.HeaderText = "";
                    colCero.DataPropertyName = "sel";
                    colCero.Width = 20;
                    dg.Columns.Add(colCero);
                    dg.MultiSelect = true;
                }
            }

            for (int iCont = 0; iCont < aCampos.Length; iCont++)
            {

                if (aCampos[iCont] != null)
                {

                    using (DataGridViewColumn dgCol = new DataGridViewColumn())
                    {
                        DataGridViewCell dgCell = new DataGridViewTextBoxCell();

                        dgCol.CellTemplate = dgCell;

                        dgCol.Name = aCampos[iCont].Split('.')[0];
                        dgCol.HeaderText = aCampos[iCont].Split('.')[1];
                        dgCol.DataPropertyName = aCampos[iCont].Split('.')[0];
                        dgCol.Width = Convert.ToInt32(aCampos[iCont].Split('.')[2]);
                        dgCol.Resizable = DataGridViewTriState.False;
                        dgCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        Font headerFont = new Font(fnt.FontFamily, fnt.Size, FontStyle.Bold);
                        dgCol.HeaderCell.Style.Font = headerFont;

                        dgCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                        dgCol.DefaultCellStyle.Alignment = (aCampos[iCont].Split('.')[3] == "I" ? DataGridViewContentAlignment.MiddleLeft :
                            (aCampos[iCont].Split('.')[3] == "C" ? DataGridViewContentAlignment.MiddleCenter : DataGridViewContentAlignment.MiddleRight));
                        dgCol.DefaultCellStyle.Format = "";
                        switch (aCampos[iCont].Split('.')[4])
                        {
                            case "Entero":
                                dgCol.DefaultCellStyle.Format = cFormatoEntero;
                                break;
                            case "Numero":
                                dgCol.DefaultCellStyle.Format = cFormatoNumero;
                                break;
                            case "Cantidad":
                                dgCol.DefaultCellStyle.Format = cFormatoCantidad;
                                break;
                            case "Fecha":
                                dgCol.DefaultCellStyle.Format = cFormatoFecha;
                                break;
                            case "FechaHora":
                                dgCol.DefaultCellStyle.Format = cFormatoFechaHora;
                                break;
                            case "Hora":
                                dgCol.DefaultCellStyle.Format = cFormatoHoraCorta;
                                break;
                        }
                        if (iCont == aCampos.Length - 1) { dgCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; }

                        dg.Columns.Add(dgCol);
                    }

                }
            }

            if (lastCol != null) { dg.Columns.Add(lastCol); }

            if (AsignaDataSource) { dg.DataSource = list; }

        }


        public void ajustarAnchoForma(Form oForm, DataGridView dg, ToolStrip ts  )
        {
            
            int textoMasLargoGrilla = 0;
            int anchoGrilla = 0;
            foreach( DataGridViewColumn dc in dg.Columns  )
            {
                string texto = dc.HeaderText;
                if (texto.Length > textoMasLargoGrilla) { textoMasLargoGrilla = texto.Length;}
                anchoGrilla += dc.Width; 
            }
            int espacioBotones = 0;
            foreach (ToolStripItem tsi in ts.Items)
            {
                if (tsi.Name.Equals ("lblBuscar")) 
                { 
                   tsi.Width = textoMasLargoGrilla * 10;
                }
                else
                {
                   espacioBotones += tsi.Width;
                }  
            }

            int espacioTotal = textoMasLargoGrilla * 10 + (anchoGrilla > espacioBotones ?  anchoGrilla : espacioBotones ) ;
            oForm .Width =  espacioTotal;
          
        }

        public void EditarColumnasEnDataGridView(DataGridView dg, string[] aCampos)
        {
            if (dg.Columns.Count > 0)
            {
                dg.ReadOnly = false;
                foreach (DataGridViewColumn campo in dg.Columns){
                    if (Array.IndexOf(aCampos, campo.Name) >= 0 ){
                    }else{
                        dg.Columns[campo.Name].ReadOnly = true;
                    }
                }
            }
        }

        public DataTable MostrarFilaEnTabla(MySqlConnection myConn, DataSet ds, string nTabla, string  strSQL,
            BindingContext Contexto, ToolStrip MenuBarra, DataGridView dg, string Region,  
            string Usuario, long nRow, bool ActualizaDataSet, bool conMenuBarra = true  )
        {

            if (ActualizaDataSet){ds = DataSetRequeryPlus(ds, nTabla, myConn, strSQL); }
            
            DataTable dt = ds.Tables[nTabla];
 
            if (nRow >= 0 && dt.Rows.Count > 0){
                Contexto[ds, nTabla].Position = Convert.ToInt32(nRow);
                if ( conMenuBarra ){ MostrarItemsEnMenuBarra(MenuBarra, Convert.ToInt32(nRow), dt.Rows.Count);}
                dg.CurrentCell = dg[0, Convert.ToInt32(nRow)];  
            }
            if ( conMenuBarra ) { ActivarMenuBarra(myConn, ds, dt , Region, MenuBarra, Usuario);}
            return dt;
        }


  
        public void MostrarItemsEnMenuBarra(ToolStrip MenuBarra, int ItemPosicion, int ItemCantidad) {
            foreach( ToolStripItem item in MenuBarra.Items ){
                if (item.Name.ToUpper()  == "ITEMS" | item.Name.ToUpper() == "ITEMSRENGLON"){ 
                    item.Text = FormatoEntero(ItemPosicion + 1);
                } 
                if (item.Name.ToUpper()  == "LBLITEMS" | item.Name.ToUpper()  == "LBLITEMSRENGLON"){
                    item .Text = String.Format("de {0}", FormatoEntero(ItemCantidad));
                }
            }
        }

        public void ActivarMenuBarra(MySqlConnection MyConn, DataSet ds, DataTable dt,
            string Region, ToolStrip MenuBarra, string Usuario, string[] aObj = null)
        {

            foreach (ToolStripItem c in MenuBarra.Items)
            {
                string nombreBoton = c.Name.Trim();
                if (dt == null)
                {
                    c.Enabled = false;
                }
                else
                {
                    if (dt.Rows.Count > 0 )
                    {
                        c.Enabled = true;
                        if (nombreBoton.Substring(0, 5) == "btnAg") { c.Enabled = UsuarioPuede(usuarioPuede.iIncluir,  MyConn, Region, Usuario); }
                        if (nombreBoton.Substring(0, 5) == "btnEd") { c.Enabled = UsuarioPuede(usuarioPuede.iModificar, MyConn, Region, Usuario); }
                        if (nombreBoton.Substring(0, 5) == "btnEl") { c.Enabled = UsuarioPuede(usuarioPuede.iEliminar, MyConn, Region, Usuario); }
                    }
                    else
                    {
                        c.Enabled = false;
                        if (nombreBoton.Substring(0, 5) == "btnAg") { c.Enabled = UsuarioPuede(usuarioPuede.iIncluir, MyConn, Region, Usuario); }
                        if (nombreBoton.Substring(0, 5) == "btnSa") { c.Enabled = true; }
                        if (nombreBoton == "btnEliminarFactura") { c.Enabled = true; }
                    }
                    if (nombreBoton == "txtBarras") { c.Enabled = true; }
                    if (nombreBoton == "btnPor") { c.Enabled = true; }
                    if (nombreBoton == "lblTitulo") { c.Enabled = true; }
                    if (nombreBoton == "Items") { c.Enabled = false; }
                    if (aObj != null){
                        if (Array.IndexOf(aObj, nombreBoton) >= 0) { c.Enabled = true; }
                    }
                }
            }

        } 
        /// <summary>
        /// Deternima si el usuario puede realizar la tarea que es pasada en el primer parámetro "up"
        /// </summary>
        /// <param name="up"></param>
        /// <param name="MyConn"></param>
        /// <param name="Region"></param>
        /// <param name="Usuario"></param>
        /// <returns></returns>
        public bool UsuarioPuede(usuarioPuede up,  MySqlConnection  MyConn, string Region, string Usuario){

            if( Region.Trim() == "" ) { return true;}  ;
             
            string campo = ( up  == usuarioPuede.iIncluir  ? "incluye" : ( up ==  usuarioPuede.iModificar   ? "modifica" : "elimina" ));
 
            return DevuelveScalarBooleano(MyConn, ( Region.Trim() == "" ? "select b." + campo + " from jsconctausu a, jsconencmap b " 
                                                                            + " where "
                                                                            + " a.mapa = b.mapa and "
                                                                            + " a.id_user = '" + Usuario + "' " : 
                                                                            " SELECT b." + campo + " FROM jsconctausu a, jsconrenglonesmapa b " 
                                                                            + " WHERE " 
                                                                            + " b.region = '" + Region + "' AND " 
                                                                            + " a.mapa = b.mapa AND " 
                                                                            + " a.id_user = '" + Usuario + "'") );
        }
        #endregion
        #region Mensajes
        public void mensajeCritico(string Mensaje){
            MessageBox.Show(Mensaje, NombreSistema, MessageBoxButtons.OK, MessageBoxIcon.Error);}
        public void mensajeAdvertencia(string Mensaje){
            MessageBox.Show(Mensaje, NombreSistema, MessageBoxButtons.OK, MessageBoxIcon.Warning);}
        public void mensajeInformativo(string Mensaje){
            MessageBox.Show(Mensaje, NombreSistema, MessageBoxButtons.OK, MessageBoxIcon.Information);}
        public void mensajeEtiqueta(Label lbl, string Mensaje, tipoMensaje tm ){

            lbl.Text = "           " + Mensaje;
            lbl.TextAlign = ContentAlignment.MiddleLeft; 
            lbl.ImageAlign = ContentAlignment.MiddleLeft;
            switch (tm) {
                case tipoMensaje.iAdvertencia:
                    lbl.Image = fTransport.Properties.Resources.Advertencia_2 ;
                    SystemSounds.Exclamation.Play();  
                    break; 
                case tipoMensaje.iAyuda:
                    lbl.Image = fTransport.Properties.Resources.Ayuda;
                    SystemSounds.Question.Play();  
                    break;
                case tipoMensaje.iError:
                    lbl.Image = fTransport.Properties.Resources.error_1;
                    SystemSounds.Beep.Play();  
                    break;
                case tipoMensaje.iInfo:
                    lbl.Image = fTransport.Properties.Resources.informacion_2;
                    //SystemSounds.Asterisk.Play();  
                    break;
                case tipoMensaje.iNinguno: 
                    break; 
            } 
        }
        public void colocaMensajeEnEtiqueta( Object obj, idioma Idioma, Label lblInfo )
        {
                string nombreObjeto = Convert.ToString(getProperty(obj, "Name"));
                foreach (string aMensaje in aBoxMessages)
                {
                    if (InArray(aMensaje.Split('.')[0].Split('|'), nombreObjeto) >= 0)
                    {
                        mensajeEtiqueta(lblInfo, aMensaje.Split('.')[1].Split('|')[(int)Idioma], tipoMensaje.iInfo); 
                    }
                }
        }

       

        /// <summary>
        /// Coloca el Tooltip a cada boton que es pasado en oObjetos como parámetros
        /// </summary>
        /// <param name="oObjetos"></param>
        //public void colocaToolTip(C1SuperTooltip c1tt, params Control[] oObjetos)
        //{
        //    colocaToolTip(c1tt, idioma.iEnglish , oObjetos); 
        //}

        public void colocaToolTip(C1SuperTooltip c1tt, idioma Idioma, params Control[] oObjetos)
        {
            foreach (Control oObjeto in oObjetos)
            {
                string nombreObjeto = Convert.ToString(getProperty(oObjeto, "Name"));
                foreach (string aTooltip in aToolTips)
                {
                    if (InArray(aTooltip.Split('.')[0].Split('|'), nombreObjeto) >= 0)
                    {
                        c1tt.SetToolTip(oObjeto, aTooltip.Split('.')[1].Split('|')[(int)Idioma]);
                    }
                }

            }
        }

        //public void colocaToolTip(C1SuperTooltip c1tt, params ToolStripItem[] oObjetos)
        //{
        //    colocaToolTip(c1tt, idioma.iEnglish, oObjetos);
        //}
        public void colocaToolTip(C1SuperTooltip c1tt, idioma Idioma, params ToolStripItem[] oObjetos)
        {
            foreach (ToolStripItem oObjeto in oObjetos)
            {
                string nombreObjeto = Convert.ToString(getProperty(oObjeto, "Name"));
                foreach (string aTooltip in aToolTips)
                {
                    if (nombreObjeto.Equals(aTooltip.Split('.')[0]))
                    {
                        c1tt.SetToolTip(oObjeto, aTooltip.Split('.')[1].Split('|')[(int)Idioma]);
                    }
                }
            }
        }

        public void colocaIdiomaEtiquetas(idioma Idioma, params Control[] oObjetos)
        {
            foreach (Control oObjeto in oObjetos)
            {
                List<Control> oObjectList = new List<Control>();
                oObjectList.AddRange(GetControls<Label>(oObjeto));
                oObjectList.AddRange(GetControls<Button>(oObjeto));
                oObjectList.ForEach(p => ApplyText (Idioma, p));
            }
        }
        public void colocaOpcionesEnCombos(idioma Idioma, params Control[] oObjetos)
        {
            foreach (Control oObjeto in oObjetos)
            {
                List<Control> oObjectList = new List<Control>();
                oObjectList.AddRange(GetControls<ComboBox>(oObjeto));
                oObjectList.ForEach(p => ApplyOptions(Idioma, p));
            }
        }

        public void ApplyToolTip(C1SuperTooltip c1tt, idioma Idioma, Control oObjeto)
        {
            string nombreObjeto = Convert.ToString(getProperty(oObjeto, "Name"));
            foreach (string aTooltip in aToolTips)
            {
                if (InArray(aTooltip.Split('.')[0].Split('|'), nombreObjeto) >= 0)
                {
                    c1tt.SetToolTip(oObjeto, aTooltip.Split('.')[1].Split('|')[(int)Idioma]);
                }
            }
        }
        public void ApplyText(idioma Idioma, Control p) 
        {
            string nombreObjeto = Convert.ToString(getProperty(p, "Name"));
            foreach (string aMensaje in aLabelsText)
            {
                if (InArray(aMensaje.Split('.')[0].Split('|'), nombreObjeto) >= 0)
                {
                    p.Text = aMensaje.Split('.')[1].Split('|')[(int)Idioma];
                }
            }
        }
        public void ApplyOptions(idioma Idioma, Control p)
        {
            string nombreObjeto = Convert.ToString(getProperty(p, "Name"));
            foreach (string aMensaje in aComboOptions)
            {
                if (InArray(aMensaje.Split('.')[0].Split('|'), nombreObjeto) >= 0)
                {
                    RellenaComboPlus( aMensaje.Split('.')[1].Split(';'), (ComboBox)p, Idioma, 0);  
                    //p.Text = aMensaje.Split('.')[1].Split('|')[(int)Idioma];
                }
            }
        }

       public List<T> GetControls<T>(Control container) where T : Control
       {
            List<T> controls = new List<T>();
            foreach (Control c in container.Controls)
            {
            if (c is T)
                controls.Add((T)c);
                controls.AddRange(GetControls<T>(c));
            }
            return controls;
        }

        public Boolean  colocaImagenesEnBarra(ToolStrip ts)
        {
            foreach( ToolStripItem tsi in ts.Items  ){
                switch (tsi.Name){
                    case "btnAgregar":
                        tsi.Image = fTransport.Properties.Resources.Agregar;
                        break;
                    case "btnEditar":
                        tsi.Image = fTransport.Properties.Resources.Modificar ;
                        break;
                    case "btnEliminar":
                        tsi.Image = fTransport.Properties.Resources.Eliminar ;
                        break;
                    case "btnBuscar":
                        tsi.Image = fTransport.Properties.Resources.Buscar ;
                        break;
                    case "btnSeleccionar":
                        tsi.Image = fTransport.Properties.Resources.Seleccionar ;
                        break;
                    case "btnPrimero":
                        tsi.Image = fTransport.Properties.Resources.Primero ;
                        break;
                    case "btnAnterior":
                        tsi.Image = fTransport.Properties.Resources.Anterior ;
                        break;
                    case "btnSiguiente":
                        tsi.Image = fTransport.Properties.Resources.Siguiente ;
                        break;
                    case "btnUltimo":
                        tsi.Image = fTransport.Properties.Resources.Ultimo ;
                        break;
                    case "btnImprimir":
                        tsi.Image = fTransport.Properties.Resources.Imprimir ;
                        break;
                    case "btnSalir":
                        tsi.Image = fTransport.Properties.Resources.Apagar;
                        break;
                }
            }
            return true;
       
        }
      


        #endregion
        #region Datum & Base de Datos
        public string autoCodigo(MySqlConnection MyConn, string nombreCampo, string nombreTabla, string WhereFields, string WhereValues,
            int longitudCampo, bool primerCodigoLibre = false, string preFijo = ""){
            // Devuelve el SIGUIENTE codigo (CADENA) AUTONUMERICO libre de un campo <nombreCampo> en una tabla <nombreTabla>
            // si el primerCodigoLibre = true DEVUELVE EL PRIMER CODIGO QUE ENCUENTRE LIBRE en una lista de codigos

            string[] aWhereFields = WhereFields.Split('.');
            string[] aWhereValues = WhereValues.Split('.');

            string strWhere = "";
            string Codigo;

            longitudCampo -= preFijo.Length;

            for (int row = 0; row <= aWhereFields.Length - 1; row++) {
               if (aWhereFields[row].Trim() != "") {strWhere += aWhereFields[row] + " = '" + aWhereValues[row] + "' AND ";} 
            }

            if (strWhere.Length > 0) { strWhere = strWhere.Substring(0, strWhere.Length - 4); }

            string str = " select " + nombreCampo + " from " + nombreTabla +
                (strWhere.Length > 0 ? " WHERE  " + strWhere : "") + " ORDER BY " + nombreCampo
                + (primerCodigoLibre ? "" : " DESC ") + " LIMIT 1 ";

            string cod = DevuelveScalarCadena(MyConn, str);

            if (cod.Equals("")) { return preFijo + RellenaConCaracter(1, longitudCampo  , '0', lado.izquierdo); }
          
            Codigo = cod.Substring(preFijo.Length, cod.Length - preFijo.Length);
            if (Codigo.Length > longitudCampo) { Codigo = Codigo.Substring(Codigo.Length - longitudCampo, longitudCampo); }
          
            while ( DevuelveScalarEntero (MyConn, " select count(*) from " + nombreTabla + " WHERE  "
                                         + nombreCampo + " = '" + preFijo + Codigo + "' " +
                                         (strWhere.Length > 0 ? " AND  " + strWhere : "")
                                         + " GROUP BY " + nombreCampo) > 0){
                Codigo = RellenaConCaracter(Convert.ToInt32(Codigo) + 1, longitudCampo, '0', lado.izquierdo);
            }

            return preFijo + Codigo;

        }


        public int autoCodigoNumero(MySqlConnection MyConn, string nombreCampo, string nombreTabla, string WhereFields, 
            string WhereValues, bool primerCodigoLibre = false)
        {
            // Devuelve el SIGUIENTE codigo (ENTERO) AUTONUMERICO libre de un campo <nombreCampo> en una tabla <nombreTabla>
            // si el primerCodigoLibre = true DEVUELVE EL PRIMER CODIGO QUE ENCUENTRE LIBRE en una lista de codigos

            string[] aWhereFields = WhereFields.Split('.');
            string[] aWhereValues = WhereValues.Split('.');

            string strWhere = "";
          

            for (int row = 0; row <= aWhereFields.Length - 1; row++)
            {
                if (aWhereFields[row].Trim() != "") { strWhere += aWhereFields[row] + " = '" + aWhereValues[row] + "' AND "; }
            }

            if (strWhere.Length > 0) { strWhere = strWhere.Substring(0, strWhere.Length - 4); }

            string str = " select " + nombreCampo + " from " + nombreTabla +
                (strWhere.Length > 0 ? " WHERE  " + strWhere : "") + " ORDER BY " + nombreCampo
                + (primerCodigoLibre ? "" : " DESC ") + " LIMIT 1 ";

            int cod = DevuelveScalarEntero(MyConn, str);

            if (cod.Equals(0)) { return 1; }

           return ++cod;

        }


        #endregion
        #region Utilidades

        public Boolean isNumeric(Object Expression){
            if(Expression == null || Expression is DateTime)
                    return false;
            if(Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                    return true;
            try{
                double retNum;
                bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
                return isNum;
            } catch {
                return false;
            } 
        }

        public void enfocarTexto(object txt)
        {
            if (!String.IsNullOrEmpty( Convert.ToString(getProperty( txt, "Text") ) ) )
            {
                setProperty(txt, "SelectionStart", 0);
                setProperty(txt, "SelectionLength", Convert.ToString(getProperty( txt, "Text")).Length );
            }
        }

        public string RellenaConCaracter(int numEntero, int posiciones, char caracter, lado Lado){
            string str = "";
            return String.Format("{0:" + (Lado == lado.izquierdo ? str.PadLeft(posiciones, caracter) : str.PadLeft(posiciones, caracter)) + "}", numEntero);
        }

        public string ArchivoEnBaseDatos_A_ArchivoEnDisco( DataRow MyRow, string Campo, string Nombre, string extension){
            string Camino = ""; 
            if (!DBNull.Value.Equals(MyRow[Campo])){
                byte[] MyData; 
                Camino = Application.StartupPath + "\\" + Campo + Nombre + extension; 
                if (System.IO.File.Exists(Camino)){ System.IO.File.Delete(Camino);}
                MyData = (byte[])MyRow[Campo];
                using (var  fs  = new System.IO.FileStream(Camino, System.IO.FileMode.OpenOrCreate , 
                    System.IO.FileAccess.Write)){
                    fs.Write(MyData,0,MyData.Length);
                    fs.Close();
                    fs.Dispose();
                }
                
            }
            return Camino;
        }

        public void GuardarArchivoEnBaseDeDatos( MySqlConnection MyConn, Boolean Insertar, String CaminoImagen, 
            String Codigo, String Renglon, String  GestionOrigen, String ModuloOrigen, String  NombreArchivo, 
            String ExtensionArchivo, String Empresa) {

            byte[] MyData;

            try {

                using (var fs = new System.IO.FileStream(CaminoImagen, System.IO.FileMode.Open , System.IO.FileAccess.Read)) {
                    
                    MyData = new Byte[fs.Length];
                    fs.Read(MyData, 0, (int)fs.Length);
                    fs.Close(); 
                    
                    using (MySqlCommand myComm = new MySqlCommand()){
                        myComm.Connection = MyConn;
                        if(Insertar){
                            myComm.CommandText = " insert into jscontablaarchivos " +
                                " set " + 
                                " codigo = '" + Codigo + "', " +
                                " renglon = '" + Renglon + "', " +
                                " gestion_origen = '" + GestionOrigen  + "', " +
                                " modulo_origen = '" + ModuloOrigen  + "', " +
                                " Archivo = ?Archivo, "  +
                                " nombre = '" + NombreArchivo + "', " +
                                " extension = '" + ExtensionArchivo + "', " + 
                                " id_emp = '" + Empresa +  "' "; 
                        }else{
                            myComm.CommandText = " update jscontablaarchivos " + 
                                " set " +
                                " Archivo = ?Archivo, " +
                                " nombre = '" + NombreArchivo + "', " +
                                " extension = '" + ExtensionArchivo + "' " + 
                                " where " +
                                " codigo = '" + Codigo + "' AND " +
                                " renglon = '" + Renglon + "' AND " +
                                " gestion_origen = '" + GestionOrigen + "' AND " +
                                " modulo_origen = '" + ModuloOrigen + "' AND " +
                                " id_emp = '" + Empresa + "' "; 
                        }
                        myComm.Parameters.AddWithValue("?Archivo", MyData); 
                        myComm.ExecuteNonQuery(); 
                    }
                }
            }
            catch (MySqlException ex) {
                mensajeCritico("Error en conexión de base de datos: " + ex.Message); 
            }
        }

        /// <summary>
        /// Devuelve el tiempo transcurrido entre dos fechas como un vector entero [Años,Meses,Dias]
        /// </summary>
        /// <param name="FechaInicial"></param>
        /// <param name="FechaFinal"></param>
        public int[] tiempo_transcurrido(DateTime FechaInicial, DateTime FechaFinal)
        {
 
            int anos = FechaFinal.Year  - FechaInicial.Year - ( FechaInicial.Month  > FechaFinal.Month  ? 1 : 0);
            int meses = FechaFinal.Month  - FechaInicial.Month - ( FechaInicial.Day  > FechaFinal.Day ? 1 : 0);
            int dias = FechaFinal.Day - FechaInicial.Day;

            if ( dias < 0 ){
                int[] aMes = { 31, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30 }; 
                if ( DateTime.IsLeapYear( FechaFinal.Year )){ aMes[3] = 29; }
                dias += aMes[ FechaFinal.Month - 1];
                if (dias < 0)
                {
                    if (dias.Equals(-1))
                    {
                        dias = 30;
                    }else if( dias.Equals(-2) ) {
                        dias = 29;
                    } 
                }
            } 

            if ( meses < 0 ) {
                if (meses >= -1) { anos -= 1; }
                meses += 12;
            }

            int[] tiempo = { anos, meses, dias };
            return tiempo ; 

        }
        
        public DialogResult PreguntaEliminarRegistro(string Comentario = "") {
              return MessageBox.Show((Comentario == "" ? "" : Comentario + "\r\n") + " ¿ Está seguro que desea eliminar registro ?", "Eliminar registro ... ", MessageBoxButtons.YesNo);
        }
        public DialogResult Pregunta(string Pregunta = "", string Titulo = ""){
            return MessageBox.Show("¿ " + Pregunta  + " ?", Titulo , MessageBoxButtons.YesNo);}

        public void setProperty(object oObject, string properyName, object value)
        {
                PropertyInfo propInfo = oObject.GetType().GetProperty(properyName);
                propInfo.SetValue(oObject, value, null);
        }
        public object getProperty(object oObject, string properyName)
        {
            PropertyInfo propInfo = oObject.GetType().GetProperty(properyName);
            return propInfo.GetValue (oObject, null);
        }

        /// <summary>
        /// Hace visible o no los objetos pasados como parámetros
        /// </summary>
        /// <param name="Visualizar"></param>
        /// <param name="oObjetos"></param>
        public void visualizarObjetos(Boolean Visualizar, params object[] oObjetos) 
        {
            foreach (object oObjeto in oObjetos)
            {
               setProperty(oObjeto, "Visible", Visualizar);
            }
        }
        /// <summary>
        /// <BHabilitar/B> o no los objetos pasados como parámetros si CambiaColor
        /// </summary>
        /// <param name="Habilitar"></param>
        /// <param name="CambiaColor"></param>
        /// <param name="oObjetos"></param>
        public void habilitarObjetos(bool Habilitar, bool CambiaColor, params object[] oObjetos)
        {
            foreach (object oObjeto in oObjetos)
            {
                setProperty(oObjeto, "Enabled", Habilitar);
                if (CambiaColor){
                    if (Habilitar) {
                       setProperty(oObjeto, "BackColor", Color.White) ;
                    }else{
                       setProperty(oObjeto, "BackColor", Color.Azure);
                    }
                }
            }
        }

        public void iniciarTextoObjetos( tipoDato Tipo, params object[] oObjetos){
      
            foreach(object oObjeto in oObjetos){

                switch(Tipo)
                {
                    case tipoDato.Cadena: 
                        setProperty(oObjeto, "Text", "") ;
                        if(getProperty(oObjeto , "TextAlign") != null && oObjeto.GetType().Equals(typeof(TextBox))) { 
                            setProperty(oObjeto, "TextAlign",  HorizontalAlignment.Left);
                        } 
                        break;
                    case tipoDato.Entero :
                        setProperty(oObjeto, "Text","0") ;
                        if (getProperty(oObjeto, "TextAlign") != null) { setProperty(oObjeto, "TextAlign", HorizontalAlignment.Right); }
                        break;
                    case tipoDato.Cantidad:
                        setProperty(oObjeto, "Text", "0.000");
                        if (getProperty(oObjeto, "TextAlign") != null) { setProperty(oObjeto, "TextAlign", HorizontalAlignment.Right); }
                        break;
                    case tipoDato.Numero: 
                        setProperty(oObjeto, "Text", "0.00");
                        if (getProperty(oObjeto, "TextAlign") != null) { setProperty(oObjeto, "TextAlign", HorizontalAlignment.Right); }
                        break;
                    case tipoDato.Fecha : 
                       setProperty(oObjeto, "Text", FormatoFecha(DateTime.Now));
                       if (getProperty(oObjeto, "TextAlign") != null) { setProperty(oObjeto, "TextAlign", HorizontalAlignment.Center); }
                        break;
                    case tipoDato.Hora:
                        setProperty(oObjeto, "Text","00:00");
                        if (getProperty(oObjeto, "TextAlign") != null) { setProperty(oObjeto, "TextAlign", HorizontalAlignment.Center); }
                        break;
                    default : 
                        setProperty(oObjeto, "Text", "");
                        if (getProperty(oObjeto, "TextAlign") != null) { setProperty(oObjeto, "TextAlign", HorizontalAlignment.Left); }
                        break;
                }

            }
        }

        public void RellenaComboPlus(object[] Items, ComboBox cmbListado, idioma Idioma, int ItemporDefecto = 0)
        {
            cmbListado.Items.Clear();
            foreach (object item in Items)
            {
                if (item != null) { cmbListado.Items.Add(item.ToString().Split('|')[(int)Idioma]); }
            }
            if (cmbListado.Items.Count > 0) { cmbListado.SelectedIndex = ItemporDefecto; }
        }
        public void RellenaCombo(object[] Items, ComboBox cmbListado, int ItemporDefecto = 0)
        {
            cmbListado.Items.Clear();
            foreach (object item in Items)
            {
                if (item != null) { cmbListado.Items.Add(item.ToString().Split('|')[0]); }
            }
            if (cmbListado.Items.Count > 0) { cmbListado.SelectedIndex = ItemporDefecto; }
        }
        public void RellenaComboConDatatable(ComboBox cmbListado, DataTable datTable, string DisplayMember, string ValueMember, 
            string ValorPorDefecto = "")
        {
            cmbListado.DataSource = null;
            cmbListado.Items.Clear();

            cmbListado.DisplayMember = DisplayMember;
            cmbListado.ValueMember = ValueMember;
            cmbListado.DataSource = datTable;

            if (cmbListado.Items.Count > 0) {
                for (int  ItemPorDefecto = 0 ; ItemPorDefecto <= cmbListado.Items.Count-1;  ItemPorDefecto++ ){
                    if (cmbListado.Items[ItemPorDefecto ].Equals (ValorPorDefecto)) {
                       cmbListado.SelectedIndex = ItemPorDefecto;
                    }
                }
            }
            
        }
        public Int32 NumeroAleatorio(Int32 Base){

            DateTime dt = DateTime.Now ;

            string semilla = Convert.ToString(Registry.GetValue(DirReg, cClaveAleatoria, ""));
            if (semilla.Trim().Equals("")) { semilla = Convert.ToString ( dt.Millisecond) ; }
            
            Random rand = new Random( Convert.ToInt32(semilla));
            int nRandom = rand.Next(0, Base); 
            Registry.SetValue(DirReg, cClaveAleatoria, Convert .ToString (nRandom));

            return nRandom; 

        }
        public string NormalizeString(string value, int maxLength)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > maxLength)
            {
                return value.Substring(0, maxLength);
            }

            return value;
        }
        
        #endregion
        #region Arreglos
        public Int32 InArray(object[] aArray, object Elemento){
            return Array.IndexOf(aArray, Elemento);
        }
        #endregion 
        #region Formatos
        public string muestraCampoTexto(Object campo){
            return Convert.ToString(( campo is DBNull ? "" : campo ) );}
        public string muestraCampoNumero(Object campo){
            return FormatoNumero(Convert.ToDouble((campo is DBNull ? 0.00 : campo)));
        }
        public string muestraCampoCantidad(Object campo){
            return FormatoCantidad(Convert.ToDouble((campo is DBNull ? 0.000 : campo)));
        }
        public string muestraCampoCantidadLarga(Object campo){
            return FormatoCantidadLarga(Convert.ToDouble((campo is DBNull ? 0.000 : campo)));
        }
        public string muestraCampoEntero(Object campo){
            return FormatoEntero(Convert.ToInt32((campo is DBNull ? 0 : campo)));
        }
        public string muestraCampoFecha(Object campo){
            return FormatoFecha(Convert.ToDateTime((campo is DBNull ? "2009-01-01" : campo)));
        }

        public string FormatoEntero(int Numero){
            return Numero.ToString(cFormatoEntero);}
        
        public string FormatoNumero(double Numero){
            return Numero.ToString(cFormatoNumero);}
        public string FormatoPorcentaje(double Numero){
            return Numero.ToString(cFormatoPorcentaje);}
        public string FormatoPorcentajeLargo(double Numero){
            return Numero.ToString(cFormatoPorcentajeLargo);}

        public string FormatoCantidad(double Numero){
            return Numero.ToString(cFormatoCantidad);}
        public string FormatoCantidadLarga(double Numero){
            return Numero.ToString(cFormatoCantidadLarga);}

        public string FormatoFecha(DateTime fecha){
            return fecha.ToString(cFormatoFecha);}
        public string FormatoFechaISLR(DateTime fecha){
            return fecha.ToString(cFormatoFechaISLR);}
        public string FormatoHora(DateTime Hora){
            return Hora.ToString(cFormatoHora);}
        public string FormatoHoraCorta(DateTime Hora){
            return Hora.ToString(cFormatoHoraCorta);}
    
        public string FormatoFechaMySQL(DateTime Fecha){
            return Fecha.ToString(cFormatoFechaMySQL);}

        public string FormatoFechaHoraMySQLInicial(DateTime Fecha){
            return Fecha.ToString(cFormatoFechaMySQL) + " 00:00:00";}
        public string FormatoFechaHoraMySQLFinal(DateTime Fecha){
            return Fecha.ToString(cFormatoFechaMySQL) + " 23:59:59";}
        public string FormatoFechaHoraMySQL(DateTime Fecha){
            return Fecha.ToString(cFormatoFechaMySQL) + " " + Fecha.Hour.ToString("00") + ":" + Fecha.Minute.ToString("00") + ":" + Fecha.Second.ToString("00") ;}
        public string dataGridViewCellFormating(DataGridView dg, System.Windows.Forms.DataGridViewCellFormattingEventArgs e)
        {
              switch(  dg.Columns[e.ColumnIndex].Name) 
              {
                  case "cantidad": 
                  case "peso":
                  case "existencia":
                  case "pesototal":
                    return FormatoCantidad(Convert.ToDouble(e.Value));
                  case "fechamov": 
                    return FormatoFecha(Convert.ToDateTime(e.Value));
                  case "precio":
                  case "preciounitario":
                  case "totalrenglon":
                  case "totren":
                  case "costotal": 
                  case "costotaldes":
                    return FormatoNumero(Convert.ToDouble(e.Value));
                  default:
                    return Convert.ToString(e.Value); 
              }
            
     
        }

        public string FormatoTablaSimple(int Modulo) {
            return RellenaConCaracter( Modulo, 5, '0', lado.izquierdo); 
        }


       #endregion
        #region Valores
        public double valorNumero(Object Numero)
        {
            string CadenaNumero = Convert.ToString(Numero);
            if (CadenaNumero.Trim().Equals("") || CadenaNumero.Trim().Equals("-") || CadenaNumero.Trim().Equals(".")) { 
                CadenaNumero = "0.00"; 
            }
            return Convert.ToDouble(CadenaNumero); 
        }
        public double valorCantidad(Object Numero)
        {
            string CadenaNumero = Convert.ToString(Numero);
            if (CadenaNumero.Trim().Equals("") || CadenaNumero.Trim().Equals("-") || CadenaNumero.Trim().Equals("."))
            {
                CadenaNumero = "0.000";
            }
            return Convert.ToDouble(CadenaNumero);
        }

        #endregion 
        #region Validaciones
        public Boolean validaNumeroEnTextbox(System.Windows.Forms.KeyPressEventArgs e) {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != '-') 
                && (e.KeyChar != '.' )){
                e.Handled = true;
            }
            return e.Handled;
        } 
        #endregion 
        #region Utilidades Maquina
        public string[] getMACAddresses()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            string[] sMacAddress = new string[nics.Length];
            int iCont = 0; 
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                sMacAddress[iCont] = adapter.GetPhysicalAddress().ToString();
                iCont += 1;
            } 
            return sMacAddress;
        }
        public string[] getHardDrivesSerials() 
        {
            ManagementObjectSearcher searcher = new
                ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
            
            string[] aSeriales = {};  
            int iCont = 0;

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                Array.Resize(ref aSeriales, aSeriales.Length + 1);
                aSeriales[iCont] = wmi_HD["SerialNumber"].ToString().Replace(" ", "");
                iCont += 1; 
            }

            return aSeriales; 
        }
 
        #endregion
    }
}
