namespace Monopoly
{
    partial class frm_login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_login));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_nick = new System.Windows.Forms.TextBox();
            this.cb_piyon = new System.Windows.Forms.ComboBox();
            this.btn_login = new System.Windows.Forms.Button();
            this.img_piyon_list = new System.Windows.Forms.ImageList(this.components);
            this.lbl_hata = new System.Windows.Forms.Label();
            this.pb_piyon = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb_piyon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(259, 211);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Oyuncu İsminizi Girin:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(259, 288);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Piyonunuzu seçin:";
            // 
            // txt_nick
            // 
            this.txt_nick.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_nick.Location = new System.Drawing.Point(467, 206);
            this.txt_nick.Name = "txt_nick";
            this.txt_nick.Size = new System.Drawing.Size(197, 29);
            this.txt_nick.TabIndex = 2;
            // 
            // cb_piyon
            // 
            this.cb_piyon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_piyon.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.cb_piyon.FormattingEnabled = true;
            this.cb_piyon.Location = new System.Drawing.Point(467, 280);
            this.cb_piyon.Name = "cb_piyon";
            this.cb_piyon.Size = new System.Drawing.Size(197, 32);
            this.cb_piyon.TabIndex = 3;
            this.cb_piyon.SelectedIndexChanged += new System.EventHandler(this.cb_piyon_SelectedIndexChanged);
            // 
            // btn_login
            // 
            this.btn_login.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_login.Location = new System.Drawing.Point(467, 382);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(197, 37);
            this.btn_login.TabIndex = 4;
            this.btn_login.Text = "GİRİŞ";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // img_piyon_list
            // 
            this.img_piyon_list.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("img_piyon_list.ImageStream")));
            this.img_piyon_list.TransparentColor = System.Drawing.Color.Transparent;
            this.img_piyon_list.Images.SetKeyName(0, "piyon1.jpg");
            this.img_piyon_list.Images.SetKeyName(1, "piyon2.jpg");
            this.img_piyon_list.Images.SetKeyName(2, "piyon3.jpg");
            this.img_piyon_list.Images.SetKeyName(3, "piyon4.jpg");
            this.img_piyon_list.Images.SetKeyName(4, "piyon5.jpg");
            this.img_piyon_list.Images.SetKeyName(5, "piyon6.jpg");
            // 
            // lbl_hata
            // 
            this.lbl_hata.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_hata.ForeColor = System.Drawing.Color.Maroon;
            this.lbl_hata.Location = new System.Drawing.Point(259, 160);
            this.lbl_hata.Name = "lbl_hata";
            this.lbl_hata.Size = new System.Drawing.Size(405, 23);
            this.lbl_hata.TabIndex = 8;
            this.lbl_hata.Text = "HATA";
            this.lbl_hata.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_hata.Visible = false;
            // 
            // pb_piyon
            // 
            this.pb_piyon.Location = new System.Drawing.Point(695, 247);
            this.pb_piyon.Name = "pb_piyon";
            this.pb_piyon.Size = new System.Drawing.Size(100, 100);
            this.pb_piyon.TabIndex = 7;
            this.pb_piyon.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Monopoly.Properties.Resources.monopoly_header;
            this.pictureBox1.Location = new System.Drawing.Point(233, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(484, 145);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Monopoly.Properties.Resources.login;
            this.pictureBox2.Location = new System.Drawing.Point(12, 98);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(232, 358);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // frm_login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 435);
            this.Controls.Add(this.lbl_hata);
            this.Controls.Add(this.pb_piyon);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.cb_piyon);
            this.Controls.Add(this.txt_nick);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frm_login";
            this.Text = "Monopoly\'e Giriş";
            this.Load += new System.EventHandler(this.frm_login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_piyon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_nick;
        private System.Windows.Forms.ComboBox cb_piyon;
        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ImageList img_piyon_list;
        private System.Windows.Forms.PictureBox pb_piyon;
        private System.Windows.Forms.Label lbl_hata;
    }
}