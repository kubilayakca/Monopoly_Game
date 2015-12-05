using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monopoly
{
    public partial class frm_zar : Form
    {
        zar gelenZar;
        public frm_zar(zar Zar)
        {
            gelenZar = Zar;
            InitializeComponent();

        }

        private void tmrZar_Tick(object sender, EventArgs e)
        {
            pb_zarGif.Visible = false;
            pb_zar1.Image = il_zarlar.Images[gelenZar.zar1];
            pb_zar2.Image = il_zarlar.Images[gelenZar.zar2];

            pb_zar1.Visible = true;
            pb_zar2.Visible = true;
            btn_close.Visible = true;
            tmrZar.Stop();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }
    }
}
