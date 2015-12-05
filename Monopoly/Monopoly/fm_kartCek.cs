using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Monopoly
{
    public partial class fm_kartCek : Form
    {

       
        string kartTuru;
        public fm_kartCek(string kartType,string metin,string bayMonopolyID)
        {
            
            InitializeComponent();

            int mrMonopolydID = Convert.ToInt32(bayMonopolyID);
            kartTuru = kartType;
            

            this.BackColor = Color.White;

            if (kartTuru == "şans")
            {
                lbl_baslik.Text = "ŞANS";
            }
            else
            {
                lbl_baslik.Text = "KAMU FONU";
            }

            
            if(mrMonopolydID!=3 && mrMonopolydID!=12)
            {
                lbl_text.Text = metin;
                pb_bay_monopoly.Image = il_mrmonopoly.Images[mrMonopolydID];
            }
            else
            {
                pb_bay_monopoly.Location= new Point(99, 69);
                pb_bay_monopoly.Size = new Size(380, 174);
                lbl_text.Visible = false;
                lbl_kodestext.Visible = true;
                lbl_kodesbaslik.Visible = true;

                if (mrMonopolydID == 12)
                {
                    pb_bay_monopoly.Image = Monopoly.Properties.Resources.kodesten_ucretsiz_cik;
                }
                else
                {
                    lbl_kodestext.Text = "BAŞLANGIÇ NOKTASINDAN GEÇME. 200M ALMA.";
                    lbl_kodesbaslik.Text = "DOĞRU KODES'E GİR.";
                    pb_bay_monopoly.Image = Monopoly.Properties.Resources.kodese_gir;
                }
            }


          
                

            
        }



        private void button1_Click(object sender, EventArgs e)
        {
            fm_game oyunFormu = this.Owner as fm_game;

            //oyunFormu.btn_kartlogo_action.Visible = false;
            this.Close();
            this.Dispose();
             
        }

        

        

        
    }
}
