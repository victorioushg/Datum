using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace FiscalPrinterBematech
{

	public class BemaFI32
	{	


		#region Funciones de tratamiento de error
		
			/// <summary>
		/// Funci�n para analisar los retorno de la impresora (ST1 y ST2).
		/// </summary>
			public static void Analisa_RetornoImpresora()
			{
				int ACK,ST1,ST2;
				string Errores = "";
				ACK = ST1 = ST2 =0;

				Bematech_FI_RetornoImpresora(ref ACK,ref ST1,ref ST2);

				#region Tratando o ST1
				if ( ST1 >= 128)
				{
					ST1 = ST1 - 128;
					Errores += "BIT 7 - Fin de Papel" + '\x0D';				
				}
				if ( ST1 >= 64)
				{
					ST1 = ST1 - 64;
					Errores += "BIT 6 - Poco Papel" + '\x0D';
				}
				if ( ST1 >= 32)
				{
					ST1 = ST1 - 32;
					Errores += "BIT 5 - Error en el Reloj" + '\x0D';
				}
				if ( ST1 >= 16)
				{
					ST1 = ST1 - 16;
					Errores += "BIT 4 - Impresora en ERROR" + '\x0D';
				}
				if ( ST1 >= 8)
				{
					ST1 = ST1 - 8;
					Errores += "BIT 3 - CMD n�o iniciado com ESC" + '\x0D';
				}
				if ( ST1 >= 4)
				{
					ST1 = ST1 - 4;
					Errores += "BIT 2 - Comando Inexistente" + '\x0D';
				}
				if ( ST1 >= 2)
				{
					ST1 = ST1 - 2;
					Errores += "BIT 1 - Comprobante Abierto" + '\x0D';
				}
				if ( ST1 >= 1)
				{
					ST1 = ST1 - 1;
					Errores += "BIT 0 - N� de Par�metros Inv�lidos" + '\x0D';
				}
				#endregion

				#region Tratando o ST2
				if ( ST2 >= 128)
				{
					ST2 = ST2 - 128;
					Errores += "BIT 7 - Tipo de Par�metro Inv�lido" + '\x0D';
				}
				if ( ST2 >= 64)
				{
					ST2 = ST2 - 64;
					Errores += "BIT 6 - Mem�ria Fiscal Llena" + '\x0D';
				}
				if ( ST2 >= 32)
				{
					ST2 = ST2 - 32;
					Errores += "BIT 5 - CMOS noo Vol�til" + '\x0D';
				}
				if ( ST2 >= 16)
				{
					ST2 = ST2 - 16;
					Errores += "BIT 4 - Alicuota no programada" + '\x0D';
				}
				if ( ST2 >= 8)
				{
					ST2 = ST2 - 8;
					Errores += "BIT 3 - Alicuotas llenas" + '\x0D';
				}
				if ( ST2 >= 4)
				{
					ST2 = ST2 - 4;
					Errores += "BIT 2 - Anulaci�n no permitida" + '\x0D';
				}
				if ( ST2 >= 2)
				{
					ST2 = ST2 - 2;
					Errores += "BIT 1 - RIF no programado" + '\x0D';
				}
				if ( ST2 >= 1)
				{
					ST2 = ST2 - 1;
					Errores += "BIT 0 - Comando no ejecutado" + '\x0D';
				}

				#endregion
				
				if (Errores.Length != 0)
					System.Windows.Forms.MessageBox.Show(Errores,"Error en la ejecuci�n del comando",MessageBoxButtons.OK,MessageBoxIcon.Error);					
			}

			/// <summary>
		/// Fun��o que analiza o retorno da fun��o.
		/// </summary>
		/// <param name="IRetorno">Inteiro com o valor a ser analizado.</param>
			public static void Analisa_iRetorno(int IRetorno)
			{
				string MSG = "";
				string MSGCaption = "Atenci�n";
				MessageBoxIcon MSGIco = MessageBoxIcon.Information;

				switch(IRetorno)
				{
					case  0: 
						MSG = "Error de Comunicaci�n !";
						MSGCaption = "Error";
						MSGIco = MessageBoxIcon.Error;
						break;
					case -1: 
						MSG = "Error de ejecuci�n en la funci�n. Verifique!";
						MSGCaption = "Error";
						MSGIco = MessageBoxIcon.Error;
						break;
					case -2: 
						MSG = "Par�metro Inv�lido !";
						MSGCaption = "Error";
						MSGIco = MessageBoxIcon.Error;
						break;
					case -3: 
						MSG = "Alicuota no programada !";
						break;
					case -4: 
						MSG = "Archivo BemaFI32.INI no encontrado. Verifique!";
						break;
					case -5: 
						MSG = "Error al abrir el puerto de comunicaci�n";
						MSGCaption = "Error";
						MSGIco = MessageBoxIcon.Error;
						break;
					case -6: 
						MSG = "Impresora apagada o desconectada.";
						break;
					case -7: 
						MSG = "Banco no registrado en el Archivo BemaFI32.ini";
						break;
					case -8: 
						MSG = "Error al crear o Grabar en el archivo Retorno.txt o Status.txt.";
						MSGCaption = "Error";
						MSGIco = MessageBoxIcon.Error;
						break;
					case -18: 
						MSG = "No fu� pos�ble abrir el archivo INTPOS.001!";
						break;
					case -19: 
						MSG = "Par�metros diferentes!";
						break;
					case -20: 
						MSG = "Transaci�n anulada por el operador!";
						break;
					case -21: 
						MSG = "La transaci�n no fu� aprobada!";
						break;
					case -22: 
						MSG = "No fu� posible finalizar la impresi�n!";
						break;
					case -23: MSG = "No fu� posible finalizar la operaci�n!";
						break;
					case -24: MSG = "No fu� posible finalizar la operaci�n!";
						break;
					case -25: MSG = "Totalizador no fiscal no programado.";
						break;
					case -26: MSG = "Transaci�n ya efectuada!";
						break;
					case -27: Analisa_RetornoImpresora();
						break;
					case -28: MSG = "No hay informaciones para imprimir!";
						break;
				}
				if (MSG.Length != 0)
					System.Windows.Forms.MessageBox.Show(MSG,MSGCaption,MessageBoxButtons.OK,MSGIco);

			}

		
		#endregion

		#region DECLARACI�N DE LAS FUNCIONES DE LA BEMAFI32.DLL   

		#region Funciones de Inicializaci�n     
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_ProgramaAlicuota(string Alicuota, int ICMS_ISS);     
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_ProgramaRedondeo();   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_ProgramaTruncamiento();   
		#endregion   

		#region Funciones del Cupon Fiscal   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_AbreComprobanteDeVenta(string RIF, string Nombre);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_AbreComprobanteDeVentaEx(string RIF, string Nombre, string Direccion);  
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_VendeArticulo(string Codigo, string Descripcion, string Alicuota, string TipoCantidad, string Cantidad, int CasasDecimales, string ValorUnitario, string TipoDescuento, string Descuento);     
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_AnulaArticuloAnterior();    
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_AnulaCupon();     
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_CierraCupon(string FormaPago, string IncrementoDescuento, string TipoIncrementoDescuento, string ValorIncrementoDescuento, string ValorPago, string Mensaje);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_IniciaCierreCupon(string IncrementoDescuento, string TipoIncrementoDescuento, string ValorIncrementoDescuento);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_EfectuaFormaPago(string FormaPago, string ValorFormaPago);    
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_FinalizaCierreCupon(string Mensaje);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_DevolucionArticulo(string Codigo, string Descripcion, string Alicuota, string TipoCantidad, string Cantidad, int CasasDecimales, string ValorUnit, string TipoDescuento, string ValorDesc);
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_AbreNotaDeCredito(string Nombre, string NumeroSerie, string RIF, string Dia, string Mes, string Ano, string Hora, string Minuto, string Segundo, string COO, string MsjPromocional);   
		#endregion   

		#region Funciones de los Informes Fiscales   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_LecturaX();   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_LecturaXSerial();   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_ReduccionZ(string Fecha, string Hora);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_InformeGerencial(string Texto);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_CierraInformeGerencial();   
		#endregion   

		#region Funciones de las Operaciones No Fiscales   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_RecebimientoNoFiscal(string IndiceTotalizador, string Valor, string FormaPago);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_AbreComprobanteNoFiscalVinculado(string FormaPago, string Valor, string NumeroCupon);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_ImprimeComprobanteNoFiscalVinculado(string Texto);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_CierraComprobanteNoFiscalVinculado();   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_Sangria(string Valor);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_Provision(string Valor, string FormaPago);   
		#endregion   

		#region Funciones de Informaciones de la Impresora   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_Agregado([MarshalAs(UnmanagedType.VBByRefStr)] ref string ValorIncrementos);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_Cancelamientos([MarshalAs(UnmanagedType.VBByRefStr)] ref string ValorCancelamientos);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_DatosUltimaReduccion([MarshalAs(UnmanagedType.VBByRefStr)] ref string DatosReduccion);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_Descuentos([MarshalAs(UnmanagedType.VBByRefStr)] ref string ValorDescuentos);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_NumeroCuponesAnulados([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroCancelamientos);   
        [DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_RetornoAlicuotas([MarshalAs(UnmanagedType.VBByRefStr)] ref string Alicuotas); 

		#endregion   

		#region Funciones de Autenticaci�n y Gaveta de Efectivo   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_AccionaGaveta();   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_Autenticacion();   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_ProgramaCaracterAutenticacion(string Parametros);   
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_VerificaEstadoGaveta(out int EstadoGaveta);   
		#endregion   

		#region Otras Funciones    
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_AbrePuertaSerial();     
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_CierraPuertaSerial();     
		[DllImport("BemaFi32.dll")]  public static extern int Bematech_FI_RetornoImpresora(ref int ACK, ref int ST1, ref int ST2);   
	  
		#endregion 

		#endregion
	}
}
