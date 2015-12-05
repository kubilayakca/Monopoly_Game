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
    public partial class frm_login : Form
    {
        public frm_login()
        {
            InitializeComponent();
        }
        MonopolyEntities5 MonopolyDB = new MonopolyEntities5();

        private void frm_login_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(0xCD, 0xE6, 0xD0);

            
            
            cb_piyon.DataSource = MonopolyDB.Piyonlar.ToList();
            cb_piyon.DisplayMember = "isim";
            cb_piyon.ValueMember = "id";
            pb_piyon.Image = img_piyon_list.Images[0];

        }

        private void cb_piyon_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int secili_index = Convert.ToInt32(cb_piyon.SelectedValue);
                pb_piyon.Image = img_piyon_list.Images[secili_index];
            }
            catch
            {

            }
            
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            if(txt_nick.Text=="")
            {
                lbl_hata.Text = "Lütfen bir oyuncu ismi giriniz!";
                lbl_hata.Visible = true;
                
            }
            else if (txt_nick.Text.Length < 3)
            {
                lbl_hata.Text = "En az 3 karakter girmelisiniz.";
                lbl_hata.Visible = true;
            }
            else
            {
                fm_game OyunForm = new fm_game(txt_nick.Text, Convert.ToInt32(cb_piyon.SelectedValue), img_piyon_list);
                this.Hide();
                OyunForm.Show();
                
            }
            
        }
    }
}
