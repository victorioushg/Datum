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
namespace fTransport
{
    public class Datum
    {

        private const string NombreSistema = "DATUM";
        
        public const string cFormatoEntero = "#,##0";
        public const string cFormatoNumero = "#,##0.00";
        public const string cFormatoCantidad = "#,##0.000";
        public const string cFormatoFecha = "dd/MM/yyyy";

        public enum lado { izquierdo, derecho };
         
        #region BDMySQL-Datum

        public DataTable AbrirDataTable(DataSet rDataset, string nTabla, MySqlConnection MyConn, string strSQL){
            return DataSetRequery(rDataset, nTabla, MyConn, strSQL).Tables[nTabla]; 
        }
        public  DataSet DataSetRequery(DataSet rDataset, string  nTabla, MySqlConnection MyConn,  string strSQL){
            
            rDataset.EnforceConstraints = false;    
            MySqlDataAdapter nDataAdapter = new MySqlDataAdapter();

            if (rDataset.Tables[nTabla] != null) {
                rDataset.Tables[nTabla].Clear();
            }

            try
            {
                nDataAdapter.SelectCommand = new MySqlCommand(strSQL, MyConn);
                nDataAdapter.Fill(rDataset, nTabla);
            }
            catch (MySqlException ex)
            {
                MensajeCritico(ex.Message + ". Error base de datos");
            }
            finally
            {
                nDataAdapter.Dispose();
                nDataAdapter = null;
            }
            return rDataset;
             
        }
        public bool Ejecutar_strSQL(MySqlConnection MyConn, string strSQL){

            try{
                MySqlCommand mycomm = new MySqlCommand();
                mycomm.Connection = MyConn;
                mycomm.CommandText = strSQL;
                mycomm.ExecuteNonQuery();
                mycomm = null;
                return true;
            }
            catch(MySqlException ex){
                MensajeCritico(ex.Number + " Error Base de Datos : " + ex.Message + " // " + strSQL);
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
                obj = mycomm.ExecuteScalar();

                if (obj == null) { return 0; }
            }
            catch (MySqlException ex)
            {
                MensajeCritico("Error Base de Datos : " + ex.Message);
            }
            finally {
                mycomm.Dispose();
                mycomm = null;
            }
            return obj;
        }
        public  ArrayList Ejecutar_strSQL_DevuelveLista(MySqlConnection MyConn, string strSQL){
            
            MySqlCommand myComm = new MySqlCommand();
            ArrayList obj = new ArrayList();

            try{
                myComm.Connection = MyConn;
                myComm.CommandText = strSQL;
                MySqlDataReader nReader;
                nReader = myComm.ExecuteReader();

                if (nReader.HasRows) {
                    while(nReader.Read()){
                    obj.Add(nReader.GetValue(0));
                    }
                }

                nReader.Close();
                nReader = null;

            }catch(MySqlException ex){
                MensajeCritico("Error Base de Datos : " + ex.Message);
            }finally{
                myComm.Dispose();
                myComm = null;
            }

            return obj;
        }
        public string muestraCampoTexto(Object campo) {
            return Convert.ToString(campo); 
        }
        public string muestraCampoNumero(Object campo) {
            return FormatoNumero(Convert.ToDouble(campo));  
        }
        public string muestraCampoCantidad(Object campo) {
            return FormatoCantidad(Convert.ToDouble(campo)); 
        }
        public string muestraCampoEntero(Object campo) {
            return FormatoEntero(Convert.ToInt32(campo));
        }
        public string muestraCampoFecha(Object campo) {
            return FormatoFecha(Convert.ToDateTime(campo));   
        } 
        #endregion
        #region Tablas y Grillas; Menus

        public void IniciarTabla(DataGridView dg, DataTable dt, string[] aCampos, bool Encabezado = true, 
            bool EditaCampos = false , Single FontSize = 9, bool EncabezadoDeFila = true , int  AltoDeFila = 18, 
            bool AsignaDataSource = true, bool SeleccionSimple = true){

            dg.Columns.Clear();
            dg.AutoGenerateColumns = false;
            dg.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.AliceBlue;
            dg.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dg.RowHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkBlue;
            Font fnt = new Font("Consolas", FontSize, FontStyle.Regular);
            dg.RowsDefaultCellStyle.Font = fnt;
            dg.ColumnHeadersVisible = Encabezado;
            dg.RowHeadersVisible = EncabezadoDeFila;

            dg.AllowUserToAddRows = false;
            dg.RowTemplate.Height =  AltoDeFila;
            dg.RowHeadersWidth = 25;

            if (EditaCampos){
                dg.SelectionMode = DataGridViewSelectionMode.CellSelect;
            }else{
                dg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            dg.MultiSelect = false;
            dg.EditMode =  ( (EditaCampos) ? DataGridViewEditMode.EditOnKeystrokeOrF2 : DataGridViewEditMode.EditProgrammatically );

            if ( !SeleccionSimple ) {
                DataGridViewCheckBoxColumn colCero = new DataGridViewCheckBoxColumn();
                colCero.Name = "sel";
                colCero.HeaderText = "";
                colCero.DataPropertyName = "sel";
                colCero.Width = 20;
                dg.Columns.Add(colCero);
                dg.MultiSelect = true;
            }

            //int AnchoColumnas = 0;

            for ( int iCont = 0; iCont < aCampos.Length; iCont++) {
                
                if (aCampos[iCont] != null) {
                    
                    DataGridViewColumn dgCol = new DataGridViewColumn();
                    DataGridViewCell dgCell = new DataGridViewTextBoxCell();

                    dgCol.CellTemplate = dgCell;
 
                    dgCol.Name = aCampos[iCont].Split('.')[0];
                    dgCol.HeaderText = aCampos[iCont].Split('.')[1];
                    dgCol.DataPropertyName = aCampos[iCont].Split('.')[0];
                    dgCol.Width = Convert.ToInt32(aCampos[iCont].Split('.')[2]) ;
                    dgCol.Resizable = DataGridViewTriState.False;
                    dgCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgCol.HeaderCell.Style.Font = new Font("Consolas", FontSize, FontStyle.Bold);
                    dgCol.SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgCol.DefaultCellStyle.Alignment = (aCampos[iCont].Split('.')[3] == "I" ? DataGridViewContentAlignment.MiddleLeft :
                        (aCampos[iCont].Split('.')[3] == "C" ? DataGridViewContentAlignment.MiddleCenter  : DataGridViewContentAlignment.MiddleRight ) );
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
                    }
                    if (iCont == aCampos.Length) { dgCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;  }

                    //AnchoColumnas += aAnchos[iCont];

                    dg.Columns.Add(dgCol);

                    
                }
            }
        
            if (AsignaDataSource){dg.DataSource = dt;}
                        
        }

        public void EditarColumnasEnDataGridView( DataGridView dg, string[] aCampos){
            if (dg.Columns.Count > 0){
                dg.ReadOnly = false;
                foreach (DataGridViewColumn campo in dg.Columns){
                    if (Array.Exists(aCampos, element => element == campo.Name )){
                    }else{
                        dg.Columns[campo.Name].ReadOnly = true;
                    }
                }
            }
        }

        public DataSet MostrarFilaEnTabla(MySqlConnection myConn, DataSet ds, string nTabla, string  strSQL,
            BindingContext Contexto, ToolStrip MenuBarra, DataGridView dg, string Region,  
            string Usuario, long nRow, bool ActualizaDataset){
            if (ActualizaDataset){ds = DataSetRequery(ds, nTabla, myConn, strSQL); }
            if (nRow >= 0 && ds.Tables[nTabla].Rows.Count > 0){
                Contexto[ds, nTabla].Position = Convert.ToInt32(nRow);  
                MostrarItemsEnMenuBarra(MenuBarra, Convert.ToInt32(nRow), ds.Tables[nTabla].Rows.Count);
                dg.CurrentCell = dg[0, Convert.ToInt32(nRow)];  
            }
            ActivarMenuBarra(myConn, ds, ds.Tables[nTabla], Region, MenuBarra, Usuario);
            return ds;
        }


  
        public void MostrarItemsEnMenuBarra(ToolStrip MenuBarra, int ItemPosicion, int ItemCantidad) {
            if (MenuBarra.Name == "MenuBarra"){
                MenuBarra.Items["items"].Text = FormatoEntero(ItemPosicion + 1);
                MenuBarra.Items["lblitems"].Text = String.Format("de {0}", FormatoEntero(ItemCantidad));
            }else{
                MenuBarra.Items["itemsrenglon"].Text = FormatoEntero(ItemPosicion + 1);
                MenuBarra.Items["lblitemsrenglon"].Text = String.Format("de {0}", FormatoEntero(ItemCantidad));
            }
         }

        public void ActivarMenuBarra( MySqlConnection MyConn, DataSet ds, DataTable dt, 
            string Region, ToolStrip MenuBarra, string Usuario, string[] aObj = null ){

            foreach ( ToolStripItem c in  MenuBarra.Items ){
                string nombreBoton = c.Name.Trim();
                if (dt == null){
                    c.Enabled = false;
                }else{
                    if (dt.Rows.Count > 0)
                    {
                        c.Enabled = true;
                        if (nombreBoton == "btnAgregar") { c.Enabled = UsuarioPuedeIncluir(MyConn, Region, Usuario); }
                        if (nombreBoton == "btnEditar") { c.Enabled = UsuarioPuedeModificar(MyConn, Region, Usuario); }
                        if (nombreBoton == "btnEliminar") { c.Enabled = UsuarioPuedeEliminar(MyConn, Region, Usuario); }
                    }
                    else {
                        c.Enabled = false;
                        if (nombreBoton == "btnAgregar") { c.Enabled = UsuarioPuedeIncluir(MyConn, Region, Usuario); }
                        if (nombreBoton == "btnSalir") { c.Enabled = true;  }
                    }
                    if (nombreBoton == "lblTitulo") { c.Enabled = true;  }
                    if (nombreBoton == "Items") { c.Enabled = false; }
                    if (aObj != null) {
                        if (Array.Exists(aObj, element => element == nombreBoton)) { c.Enabled = true; } 
                    }
                }
            }

        } 
 
   
        public bool UsuarioPuedeIncluir(MySqlConnection  MyConn, string Region, string Usuario){
            if (Region.Trim() == ""){
                return Convert.ToBoolean(Convert.ToInt32(Ejecutar_strSQL_DevuelveScalar(MyConn, "select if(b.incluye = '', 1 , b.incluye) from jsconctausu a, jsconencmap b "
                                                          + " where "
                                                          + " a.mapa = b.mapa and "
                                                          + " a.id_user = '" + Usuario + "' ").ToString()));
            }else{
                return Convert.ToBoolean( Convert.ToInt32 ( Ejecutar_strSQL_DevuelveScalar(MyConn, " SELECT b.incluye FROM jsconctausu a, jsconrenglonesmapa b " 
                                            + " WHERE " 
                                            + " b.region = '" + Region + "' AND " 
                                            + " a.mapa = b.mapa AND " 
                                            + " a.id_user = '" + Usuario + "'").ToString()) );
            }
        }
        public bool UsuarioPuedeModificar(MySqlConnection MyConn, string Region, string Usuario)
        {
            if (Region.Trim() == "")
            {
                return Convert.ToBoolean(Convert.ToInt32 (Ejecutar_strSQL_DevuelveScalar(MyConn, "select if(b.modifica = '', 1 , b.modifica) from jsconctausu a, jsconencmap b "
                                                          + " where "
                                                          + " a.mapa = b.mapa and "
                                                          + " a.id_user = '" + Usuario + "' ").ToString()));
            }
            else
            {
                return Convert.ToBoolean( Convert.ToInt32 ( Ejecutar_strSQL_DevuelveScalar(MyConn, " SELECT b.modifica FROM jsconctausu a, jsconrenglonesmapa b "
                                            + " WHERE "
                                            + " b.region = '" + Region + "' AND "
                                            + " a.mapa = b.mapa AND "
                                            + " a.id_user = '" + Usuario + "'").ToString())) ;
            }
        }
        public bool UsuarioPuedeEliminar(MySqlConnection MyConn, string Region, string Usuario)
        {
            if (Region.Trim() == "")
            {
                return Convert.ToBoolean(Convert.ToInt32 ( Ejecutar_strSQL_DevuelveScalar(MyConn, "select if(b.elimina = '', 1 , b.elimina) from jsconctausu a, jsconencmap b "
                                                          + " where "
                                                          + " a.mapa = b.mapa and "
                                                          + " a.id_user = '" + Usuario + "' ").ToString() ));
            }
            else
            {
                return Convert.ToBoolean(Convert.ToInt32 ( Ejecutar_strSQL_DevuelveScalar(MyConn, " SELECT b.elimina FROM jsconctausu a, jsconrenglonesmapa b "
                                            + " WHERE "
                                            + " b.region = '" + Region + "' AND "
                                            + " a.mapa = b.mapa AND "
                                            + " a.id_user = '" + Usuario + "'").ToString()));
            }
        }



        #endregion
        #region Mensajes
        public void MensajeCritico(string Mensaje){
             MessageBox.Show(Mensaje, NombreSistema, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
        public void MensajeAdvertencia(string Mensaje){
             MessageBox.Show(Mensaje, NombreSistema, MessageBoxButtons.OK, MessageBoxIcon.Warning);
         }
        public void MensajeInformativo(string Mensaje){
             MessageBox.Show(Mensaje, NombreSistema, MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
        public void MensajeEtiquetaInformativa(Label lbl, string Mensaje)
        {
            lbl.Text = "           " + Mensaje;
            lbl.ImageAlign = ContentAlignment.MiddleLeft;
        }
        public void MensajeEtiquetaAdvertencia(Label lbl, string Mensaje)
        {
            lbl.Text = "           " + Mensaje;
            lbl.ImageAlign = ContentAlignment.MiddleLeft;
        }

        #endregion
        #region Utilidades

        public System.Boolean IsNumeric (System.Object Expression){
            if(Expression == null || Expression is DateTime)
                    return false;

            if(Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                    return true;
            try{
                if(Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            } catch {// just dismiss errors but return false
                return false;
            } 
        }
        public string RellenaConCaracter(int numEntero, int posiciones, char caracter, lado Lado){
           string str = "";
           return String.Format("{0:" + ( Lado == lado.izquierdo ?  str.PadLeft(posiciones, caracter ) :  str.PadLeft(posiciones, caracter )  ) + "}", numEntero);
        }
        public string IncrementarCadena(string Cadena){
            
            char cChar;
            int iNum;
            int Suma = 1;
            string iCadena = "";
            for (int iCont = Cadena.Length - 1; iCont == 0; iCont--){
                cChar = Convert.ToChar (Cadena.Substring(iCont , 1 )) ;
                if (IsNumeric(cChar)){
                    iNum = Convert.ToInt32(cChar) + Suma;
                    if (iNum > 9){
                        Suma = 1;
                        iCadena = "0" + iCadena;
                    }else{
                        Suma = 0;
                        iCadena = Convert.ToString(iNum) + iCadena;
                    }
                }else{
                    if (Encoding.ASCII.GetBytes(Convert.ToString(cChar))[0] >= 65 & Encoding.ASCII.GetBytes(Convert.ToString(cChar))[0] <= 90){
                        iNum = Encoding.ASCII.GetBytes(Convert.ToString(cChar))[0] + Suma;
                        if (iNum > 90){
                            Suma = 1;
                            iCadena = "A" + iCadena;
                        }else{
                            Suma = 0;
                            iCadena = (char)iNum + iCadena;
                        }
                    }else{
                        if (cChar == '.' | cChar == '-') {
                            iCadena = cChar + iCadena;
                        }
                    }
                } 
            }

            return iCadena; 
        }
        public string AutoCodigo(MySqlConnection  MyConn, string nombreCampo, string nombreTabla, string[] aWhereFields, string[] aWhereValues, 
            int longitudCampo, bool primerCodigoLibre = false) {
            // Devuelve el SIGUIENTE codigo AUTONUMERICO libre de un campo <nombreCampo> en una tabla <nombreTabla>
            // si el primerCodigoLibre = true DEVUELVE EL PRIMER CODIGO QUE ENCUENTRE LIBRE en una lista de codigos
            int ultimo = 0;
            string strWhere = "";
            string Codigo;

            for (int row = 0; row <= aWhereFields.GetUpperBound(0); row++){
                strWhere += aWhereFields[row] + " = '" + aWhereValues[row] + "' AND ";
            }
    
            if (strWhere.Length>0){ strWhere = strWhere.Substring(0, strWhere.Length - 4);}

            ultimo = Convert.ToInt32(Ejecutar_strSQL_DevuelveScalar( MyConn, " select " + nombreCampo + " from " + nombreTabla +
                (strWhere.Length > 0 ? " WHERE  " + strWhere : "") + " ORDER BY " + nombreCampo 
                + (primerCodigoLibre? "": " DESC ") +" LIMIT 1 "));

            if (ultimo == 0) { ultimo = 1; } 
            Codigo = IncrementarCadena(Convert.ToString(ultimo));

            while (Codigo.Length < longitudCampo){
                Codigo = "0" + Codigo;   
            }
            
            while (Convert.ToInt32 ( Ejecutar_strSQL_DevuelveScalar(MyConn, " select count(*) from " + nombreTabla + " WHERE  " 
                                         + nombreCampo + " = '" + Codigo  + "' " +
                                         (strWhere.Length > 0 ? " AND  " + strWhere : "")
                                         + " ORDER BY " + nombreCampo) ) > 0) {  
                    Codigo = IncrementarCadena(Codigo);
            }

            return Codigo; 

        }

        #endregion
        #region Formatos
        public string FormatoEntero(int Numero) {
            return Numero.ToString(cFormatoEntero);
        }
        public string FormatoNumero(double Numero) {
            return Numero.ToString(cFormatoNumero); 
        }
        public string FormatoCantidad(double Numero) {
            return Numero.ToString(cFormatoCantidad);  
        }
        public string FormatoFecha(DateTime fecha) {
            return fecha.ToString(cFormatoFecha); 
        }

        #endregion
    }
}
