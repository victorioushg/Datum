using System;
using System.Windows.Forms;  
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TfhkaNet.IF;
using TfhkaNet.IF.VE; 

namespace FP_AclasBixolon
{
   public class AclasBixolon
    {
       
       public bool bRet = false;
       public enum ComandoFiscalAclasBixolon
       {   iEncabezadoPiePaginaDatos,
           iRenglon, 
           iPago,
           iNombrePago,
           iRazonSocial,
           iRif,
           iFacturaAfectada, 
           iSerialNDNC, 
           iFechaNDNC, 
           iEncabezadoPiePaginaProgramacion,
           iIVA,
           iHora, 
           iFecha, 
           iSubtotal,
           iDescuento,
           iPagodirecto,
           iLineaNoFiscal,
           iLineaNoFiscalCierre,
           iMensajeFactura,
           iPagoNotaCredito,
           iResetImpresora,
           iReimpresionFactura,
           iReimpresionNotaCredito,
           iReimpresionUltimoDocumento,
           iRetiroDeCaja,
           iFondoDeCaja,
           iFin_RetiroFondo,
           iValoresProgramados,
           iAnularDocumento,
           iReporteX,
           iReporteZ, 
           iReimpresionDocumentoNOFiSCAL }

       
       private const int FORMATO_ENTERO10 = 10; //  RETIRO/FONDO ENTERO
       private const int FORMATO_ENTERO08 = 8;  //  PRECIO ENTERO
       private const int FORMATO_ENTERO05 = 5;  //  CANTIDAD ENTERA 
       private const int FORMATO_ENTERO02 = 2;  //  PRECIO/RETIRO/FONDO DECIMAL 
       private const int FORMATO_ENTERO03 = 3;  //  CANTIDAD DECIMAL
       private const string FORMATO_FECHA = "ddmmyy";
       private Tfhka Impresora = new Tfhka();
        
       public enum tipoDocumentoFiscal 
           // FC = FACTURA;  NC = NOTA CREDITO; NF = NO FISCAL; NR = NUMERO REGISTRO; ND = NOTA DEBITO  
           // UDFNF = Ultimo documento fiscal ó no fiscal
          {Factura, 
           NotaCredito, 
           Nofiscal, 
           numRegistro, 
           FC_SRP812, 
           NC_SRP812, 
           NF_SRP812, 
           NR_SRP812, 
           ND_SRP812,
           NR_SRP350, 
           UDFNF_Todas}

       #region Funciones Impresora

        // Abrir puerto impresora Fiscal 
       public bool abrirPuerto(string puertoSerial){
            //true = abierto ; falSe = cerrado
           return Impresora.OpenFpCtrl(puertoSerial);
       }

       public void cerrarPuerto() {
            Impresora.CloseFpCtrl();
       }

       public bool impresoraEncendida() {
            // true = encendida
           return Impresora.CheckFPrinter();
       }
       public bool estatusImpresora() {
            // true = En Espera ; false = ocupada
            return Impresora.ReadFpStatus(); 
       }
       public bool reiniciarImpresora() {
           return enviarComando(ComandoFiscalAclasBixolon.iResetImpresora, 0, ""); 
       }


       public bool enviarComando( ComandoFiscalAclasBixolon tipoComando, int linea, string texto) {
            string cmd = "";
            switch(tipoComando){
                case ComandoFiscalAclasBixolon.iRazonSocial: // ND/NC IMP SRP 812 
                    cmd = "iS*" + texto; 
                    break;
                case ComandoFiscalAclasBixolon.iRif: // ND/NC IMP SRP 812 
                    cmd = "iR*" + texto;
                    break;
                case ComandoFiscalAclasBixolon.iFacturaAfectada: // ND/NC IMP SRP 812 
                    cmd = "iF*" + texto;
                    break;
                case ComandoFiscalAclasBixolon.iSerialNDNC: // ND/NC IMP SRP 812 
                    cmd = "iI*" + texto;
                    break;
                case ComandoFiscalAclasBixolon.iFechaNDNC: // ND/NC IMP SRP 812  / texto = DD/MM/AA
                    cmd = "iD*" + texto;
                    break;
                case ComandoFiscalAclasBixolon.iEncabezadoPiePaginaDatos:  //ENCABEZADO TODAS + // ND/NC IMP SRP 812 
                    cmd = "i" + formatoEnteroFiscal(linea, 2) + texto; 
                    break;
                case ComandoFiscalAclasBixolon.iRenglon: //RENGLON
                    cmd = texto;
                    break;
                case ComandoFiscalAclasBixolon.iPago: //PAGO
                    cmd = "2" + formatoEnteroFiscal( linea, 2 ) + texto;
                    break;
                case ComandoFiscalAclasBixolon.iNombrePago: //' NOMBRE PAGO
                    cmd = "PE" + formatoEnteroFiscal(linea, 2) + texto;
                    break;
                case ComandoFiscalAclasBixolon.iEncabezadoPiePaginaProgramacion: //MENSAJE PIE DE PAGINA
                    cmd = "PH9" + String.Format("{0}",linea) + texto;
                    break;
                case ComandoFiscalAclasBixolon.iIVA: //PROGRAMACION TASAS IVA
                    cmd = "PT" + texto;
                    break;
                case ComandoFiscalAclasBixolon.iHora: //'HORA
                    cmd = "PF" + texto;
                    break;
                case ComandoFiscalAclasBixolon.iFecha: //FECHA
                    cmd = "PG" + texto;
                    break;
                case ComandoFiscalAclasBixolon.iSubtotal: //Imprimir Subtotal Descuento
                    cmd = "3";
                    break;
                case ComandoFiscalAclasBixolon.iDescuento: //Descuento
                    cmd = "p-" + texto;
                    break;
                case ComandoFiscalAclasBixolon.iPagodirecto://PAGO DIRECTO
                    cmd = "1" + formatoEnteroFiscal(linea, 2);
                    break;
                case ComandoFiscalAclasBixolon.iLineaNoFiscal: //NO FISCAL
                    cmd = "800" + texto;
                    break;
                case ComandoFiscalAclasBixolon.iLineaNoFiscalCierre: //NO FISCAL CIERRE
                    cmd = "810" + texto;
                    break;
                case ComandoFiscalAclasBixolon.iMensajeFactura: //Mensaje en Factura
                    cmd = "@" + texto;
                    break;
                case ComandoFiscalAclasBixolon.iPagoNotaCredito: //FORMA DE PAGO EN NOTA CREDITO
                    cmd = "f" + formatoEnteroFiscal(linea, 2) + texto;
                    break;
                case ComandoFiscalAclasBixolon.iResetImpresora: //REINICIAR IMPRESORA
                    cmd = "e";
                    break;
                case ComandoFiscalAclasBixolon.iReimpresionFactura: //re-impresion FACTURA
                    cmd = "RF" + (texto.Length > 0 ? texto.Substring(1) + texto.Substring(1) : "");
                    break;
                case ComandoFiscalAclasBixolon.iReimpresionNotaCredito: //re-impresion nota credito
                    cmd = "RC" + (texto.Length > 0 ? texto.Substring(1) + texto.Substring(1) : ""); 
                    break;
                case ComandoFiscalAclasBixolon.iReimpresionDocumentoNOFiSCAL : //re-impresion ultimo NO FISCAL 
                    cmd = "RT" + (texto.Length > 0 ? texto.Substring(1) + texto.Substring(1) : "");
                    break;
                case ComandoFiscalAclasBixolon.iReimpresionUltimoDocumento: //re-IMPRESION ULTIMO DOCUMENTO FISCAL/NO FISCAL
                    cmd = "RU00000000000000";
                    break;
                case ComandoFiscalAclasBixolon.iRetiroDeCaja: //Retiro de Caja
                    cmd = "90" + texto;
                    break;
                case ComandoFiscalAclasBixolon.iFondoDeCaja://Fondo De Caja
                    cmd = "91" + texto;
                    break;
                case ComandoFiscalAclasBixolon.iFin_RetiroFondo: //Fin Retiro/Fondo
                    cmd = "t";
                    break;
                case ComandoFiscalAclasBixolon.iValoresProgramados: //IMPRIMIR VALORES PROGRAMADOS
                    cmd = "D";
                    break;
                case ComandoFiscalAclasBixolon.iAnularDocumento: //ANULACION DOCUMENTO
                    cmd = "7";
                    break;
                case ComandoFiscalAclasBixolon.iReporteX : //RE3PORTE FISCAL X
                    cmd = "I0X";
                    break;
                case ComandoFiscalAclasBixolon.iReporteZ :
                    cmd = "I0Z";
                    break;
                default:
                    cmd = "RU00000000000000";
                    break;
          } 

            return Impresora.SendCmd(cmd);
       }

     
       #endregion
       #region Funciones Impresora Ampliadas

       public bool FinalizarFactura(){
            return enviarComando(ComandoFiscalAclasBixolon.iPagodirecto, 1, "");
       }

       public bool AnularDocumento(){
           return enviarComando(ComandoFiscalAclasBixolon.iAnularDocumento, 0, "");
        }

       public bool ImprimirProgramacion(){
            return  enviarComando(ComandoFiscalAclasBixolon.iValoresProgramados, 0, "");
       }

       public bool RetiroDeCaja(double Monto, int NumeroFormaDePago){
            string numR = montoString(Monto, FORMATO_ENTERO02 , FORMATO_ENTERO10 );
            enviarComando(ComandoFiscalAclasBixolon.iRetiroDeCaja, 0, formatoEnteroFiscal(NumeroFormaDePago, FORMATO_ENTERO02 ) + numR);
            return enviarComando(ComandoFiscalAclasBixolon.iFin_RetiroFondo, 0, "");
       }

       public bool FondoDeCaja(double Monto, int NumeroFormaDePago){
            string numR = montoString(Monto, FORMATO_ENTERO02 , FORMATO_ENTERO10 );
            enviarComando( ComandoFiscalAclasBixolon.iFondoDeCaja, 0, formatoEnteroFiscal(NumeroFormaDePago , FORMATO_ENTERO02 ) + numR);
            return enviarComando(ComandoFiscalAclasBixolon.iFin_RetiroFondo, 0, "");
       }

       public bool ReporteXFiscal(){
            return enviarComando(ComandoFiscalAclasBixolon.iReporteX ,0,"");
       }

       public bool ReporteZFiscal(){
           return enviarComando(ComandoFiscalAclasBixolon.iReporteZ, 0, "");
       }

       public string UltimoDocumentoFiscal(tipoDocumentoFiscal TipoDocumento)
       {

           string filename;
           string cmd = "S1";
           int nPos = 0;
           int iLen = 8;
           Random rnd = new Random();
           string dret = "";

           try
           {
               switch (TipoDocumento)
               {
                   case tipoDocumentoFiscal.FC_SRP812:
                       nPos = 21;
                       iLen = 8;
                       break;
                   case tipoDocumentoFiscal.ND_SRP812:
                       nPos = 34;
                       iLen = 8;
                       break;
                   case tipoDocumentoFiscal.NC_SRP812:
                       nPos = 47;
                       iLen = 8;
                       break;
                   case tipoDocumentoFiscal.NF_SRP812:
                       nPos = 60;
                       iLen = 8;
                       break;
                   case tipoDocumentoFiscal.NR_SRP812:
                       nPos = 92;
                       iLen = 10;
                       break;
                   case tipoDocumentoFiscal.Factura:
                        nPos = 21;
                        iLen = 8;
                        break;
                   case tipoDocumentoFiscal.NotaCredito:
                        nPos = 88;
                        iLen = 8;
                        break;
                   case tipoDocumentoFiscal.Nofiscal:
                        nPos = 34;
                        iLen = 8;
                        break;
                   case tipoDocumentoFiscal.numRegistro:
                        nPos = 74;
                        iLen = 10;
                        break;
                   case tipoDocumentoFiscal.NR_SRP350:
                       nPos = 66;
                       iLen = 10;
                       break;
                   default:
                        break;
               }

               filename = System.IO.Path.Combine(System.Environment.CurrentDirectory ,"doc" +  
                   formatoEnteroFiscal(rnd.Next(1000, 10001), FORMATO_ENTERO08) + ".txt");

               bRet = Impresora.UploadStatusCmd(cmd, filename);
                         
              
               string str = "";
               using (StreamReader sr = new StreamReader(filename))
               {
                   string line;
                   while ((line = sr.ReadLine()) != null)
                   {
                       str +=line;
                   }
               }
               
               System.IO.File.Delete(@filename);

               if (str.Length > 0) {
                   dret = str.Substring(nPos, iLen);
                   //MessageBox.Show(dret + "" + str); 
               }
       
               return dret;   
           }

    
           finally
           {
               //return dret;
           }
       }

       public bool ReimprimirUltimoDocumentoFiscal(tipoDocumentoFiscal TipoDocumento, string numeroDocumento) {

           switch (TipoDocumento) {
               case tipoDocumentoFiscal.FC_SRP812 :
               case tipoDocumentoFiscal.Factura :
                   bRet = enviarComando(ComandoFiscalAclasBixolon.iReimpresionFactura, 0,numeroDocumento);
                   break;
               case tipoDocumentoFiscal.NC_SRP812: 
               case  tipoDocumentoFiscal.NotaCredito :
                   bRet = enviarComando(ComandoFiscalAclasBixolon.iReimpresionNotaCredito, 0, numeroDocumento);
                   break;
               case tipoDocumentoFiscal.Nofiscal :
               case tipoDocumentoFiscal.NF_SRP812 :
                   bRet = enviarComando(ComandoFiscalAclasBixolon.iReimpresionDocumentoNOFiSCAL, 0, numeroDocumento);
                   break;
               case tipoDocumentoFiscal.UDFNF_Todas :
                   bRet = enviarComando(ComandoFiscalAclasBixolon.iReimpresionUltimoDocumento , 0, numeroDocumento);
                   break;
               default:
                   bRet = false;
                   break;
           }
           return bRet;
       }


           
        #endregion


        #region Utilitarios
        private string  FormatoCantidadFiscal(double dNumero){
            return montoString(dNumero, FORMATO_ENTERO03, FORMATO_ENTERO05);
        }

        private string FormatoPrecioFiscal(double dNumero){
            return montoString(dNumero, FORMATO_ENTERO02 , FORMATO_ENTERO08 );
        }

        private string formatoEnteroFiscal(int numEntero, int posiciones){
           string str = "";
           return String.Format("{0:" + str.PadLeft(posiciones, '0') + "}", numEntero);
        }

        private string FormatoFechaFiscal(DateTime sFecha){
            return sFecha.ToString(FORMATO_FECHA);
        }

        private string montoString(double monto, int posicionesDecimales, int posicionesEnteras) {
            String[] aMonto = Convert.ToString(monto).Split('.'); 
            string parteEntera = formatoEnteroFiscal(Convert.ToInt32(aMonto[0]), posicionesEnteras);
            string parteDecimal = "00";
            if ( Convert.ToString(monto).Split('.').Length > 1 ){
                parteDecimal = formatoEnteroFiscal(Convert.ToInt32(aMonto[1]), posicionesDecimales);
            }
            return parteEntera + parteDecimal;
        }


        #endregion




    }
}
