namespace Monopoly
{
    partial class fm_kartCek
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fm_kartCek));
            this.lbl_baslik = new System.Windows.Forms.Label();
            this.lbl_text = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_tamam = new System.Windows.Forms.Button();
            this.pb_bay_monopoly = new System.Windows.Forms.PictureBox();
            this.il_mrmonopoly = new System.Windows.Forms.ImageList(this.components);
            this.lbl_kodestext = new System.Windows.Forms.Label();
            this.lbl_kodesbaslik = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pb_bay_monopoly)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_baslik
            // 
            this.lbl_baslik.AutoSize = true;
            this.lbl_baslik.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_baslik.Location = new System.Drawing.Point(243, 37);
            this.lbl_baslik.Name = "lbl_baslik";
            this.lbl_baslik.Size = new System.Drawing.Size(59, 26);
            this.lbl_baslik.TabIndex = 0;
            this.lbl_baslik.Text = "ŞANS";
            // 
            // lbl_text
            // 
            this.lbl_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_text.Location = new System.Drawing.Point(49, 56);
            this.lbl_text.Name = "lbl_text";
            this.lbl_text.Size = new System.Drawing.Size(281, 187);
            this.lbl_text.TabIndex = 1;
            this.lbl_text.Text = "CADDEBOSTA\'A İLERLE. BAŞLANGIÇ NOKTASINDAN GEÇERSEN 200M AL";
            this.lbl_text.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(32, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(506, 273);
            this.label3.TabIndex = 2;
            // 
            // btn_tamam
            // 
            this.btn_tamam.Location = new System.Drawing.Point(255, 309);
            this.btn_tamam.Name = "btn_tamam";
            this.btn_tamam.Size = new System.Drawing.Size(75, 23);
            this.btn_tamam.TabIndex = 3;
            this.btn_tamam.Text = "Tamam";
            this.btn_tamam.UseVisualStyleBackColor = true;
            this.btn_tamam.Click += new System.EventHandler(this.button1_Click);
            // 
            // pb_bay_monopoly
            // 
            this.pb_bay_monopoly.Location = new System.Drawing.Point(358, 47);
            this.pb_bay_monopoly.Name = "pb_bay_monopoly";
            this.pb_bay_monopoly.Size = new System.Drawing.Size(170, 250);
            this.pb_bay_monopoly.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_bay_monopoly.TabIndex = 4;
            this.pb_bay_monopoly.TabStop = false;
            // 
            // il_mrmonopoly
            // 
            this.il_mrmonopoly.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il_mrmonopoly.ImageStream")));
            this.il_mrmonopoly.TransparentColor = System.Drawing.Color.Transparent;
            this.il_mrmonopoly.Images.SetKeyName(0, "1.png");
            this.il_mrmonopoly.Images.SetKeyName(1, "1.png");
            this.il_mrmonopoly.Images.SetKeyName(2, "2.png");
            this.il_mrmonopoly.Images.SetKeyName(3, "3.png");
            this.il_mrmonopoly.Images.SetKeyName(4, "4.png");
            this.il_mrmonopoly.Images.SetKeyName(5, "5.png");
            this.il_mrmonopoly.Images.SetKeyName(6, "6.png");
            this.il_mrmonopoly.Images.SetKeyName(7, "7.png");
            this.il_mrmonopoly.Images.SetKeyName(8, "8.png");
            this.il_mrmonopoly.Images.SetKeyName(9, "9.png");
            this.il_mrmonopoly.Images.SetKeyName(10, "10.png");
            this.il_mrmonopoly.Images.SetKeyName(11, "11.png");
            this.il_mrmonopoly.Images.SetKeyName(12, "12.png");
            // 
            // lbl_kodestext
            // 
            this.lbl_kodestext.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_kodestext.Location = new System.Drawing.Point(96, 277);
            this.lbl_kodestext.Name = "lbl_kodestext";
            this.lbl_kodestext.Size = new System.Drawing.Size(383, 22);
            this.lbl_kodestext.TabIndex = 5;
            this.lbl_kodestext.Text = "Bu kartı kodesten çıkmak için kullanabilirsiniz.";
            this.lbl_kodestext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_kodestext.Visible = false;
            // 
            // lbl_kodesbaslik
            // 
            this.lbl_kodesbaslik.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_kodesbaslik.Location = new System.Drawing.Point(96, 251);
            this.lbl_kodesbaslik.Name = "lbl_kodesbaslik";
            this.lbl_kodesbaslik.Size = new System.Drawing.Size(383, 22);
            this.lbl_kodesbaslik.TabIndex = 6;
            this.lbl_kodesbaslik.Text = "KODES\'TEN ÜCRETSİZ ÇIK";
            this.lbl_kodesbaslik.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_kodesbaslik.Visible = false;
            // 
            // fm_kartCek
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 337);
            this.Controls.Add(this.lbl_kodesbaslik);
            this.Controls.Add(this.lbl_kodestext);
            this.Controls.Add(this.lbl_baslik);
            this.Controls.Add(this.pb_bay_monopoly);
            this.Controls.Add(this.btn_tamam);
            this.Controls.Add(this.lbl_text);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fm_kartCek";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "SansKarti";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pb_bay_monopoly)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_baslik;
        private System.Windows.Forms.Label lbl_text;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_tamam;
        private System.Windows.Forms.PictureBox pb_bay_monopoly;
        private System.Windows.Forms.ImageList il_mrmonopoly;
        private System.Windows.Forms.Label lbl_kodestext;
        private System.Windows.Forms.Label lbl_kodesbaslik;
    }
}