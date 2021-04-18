using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fTransport; 
namespace posDATUM.Formularios
{
    public partial class frmFecha : Form
    {
        private static DateTime _Fecha;
        private Transportables ft = new Transportables();

        public DateTime Fecha { get; set; }

        public frmFecha(DateTime FechaInicial, params Control[] oObjetos)
        {
            InitializeComponent();
            int nLeft = 0; 
            int nTop = 0;
            foreach( Control oObjeto in oObjetos){
                nLeft += oObjeto.Left;
                nTop += oObjeto.Top;
            }

            Fecha = FechaInicial; 
            Calendario.SetDate(Fecha);

            Point rPoint = new Point(nLeft, nTop);
            if ( nLeft + nTop == 0) {
                this.StartPosition = FormStartPosition.CenterScreen;
            }else{
                this.StartPosition = FormStartPosition.Manual;
                this.Location = PointToClient(rPoint);
            }
            
            this.ShowDialog();

        }
            
       private void Calendario_DateSelected(object sender, DateRangeEventArgs e)
       {
           Fecha = e.Start.Date;
           this.Close();
       }

        private void frmFecha_Resize(object sender, EventArgs e)
        {
            this.ClientSize = new Size(Calendario.Width, Calendario.Height + lblInfo.Height);
        }

        private void frmFecha_Load(object sender, EventArgs e)
        {
            ft.mensajeEtiqueta(lblInfo, "Escoja fecha...",  Transportables.tipoMensaje.iInfo);
        }

       
    }
}
