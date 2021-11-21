using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.DataGrid;

namespace fTransport.Models
{
    public class DataGridField
    {
        public string Campo { get; set; }
        public string Nombre { get; set; }
        public int Ancho { get; set; }
        public DataGridViewContentAlignment Alineacion {get; set;}
        public string Formato { get; set; }
        public DataGridField ( string campo, string nombre, int ancho, DataGridViewContentAlignment  alineacion, string formato)
        {
            Campo = campo;
            Nombre = nombre;
            Ancho = ancho;
            Alineacion = alineacion;
            Formato = formato;
        }
    }
    public class SfDataGridField
    {
        public TypeColumn Type { get; set;}
        public string Campo { get; set; }
        public string Nombre { get; set; }
        public int Ancho { get; set; }
        public HorizontalAlignment Alineacion { get; set; }
        public string Formato { get; set; }

        public SfDataGridField(TypeColumn type, string campo, string nombre, int ancho, HorizontalAlignment alineacion, string formato)
        {
            Type = type; 
            Campo = campo; 
            Nombre = nombre;
            Ancho = ancho;
            Alineacion = alineacion;
            Formato = formato;
        }
    }  


    public enum TypeColumn
    {
        TextColumn = 0,
        DateTimeColumn = 1,
        NumericColumn = 2,
        CheckBoxColumn = 3
    }


}
