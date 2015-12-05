namespace Monopoly
{
    partial class frm_zar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_zar));
            this.pb_zarGif = new System.Windows.Forms.PictureBox();
            this.il_zarlar = new System.Windows.Forms.ImageList(this.components);
            this.pb_zar1 = new System.Windows.Forms.PictureBox();
            this.pb_zar2 = new System.Windows.Forms.PictureBox();
            this.tmrZar = new System.Windows.Forms.Timer(this.components);
            this.btn_close = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pb_zarGif)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_zar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_zar2)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_zarGif
            // 
            this.pb_zarGif.Image = global::Monopoly.Properties.Resources.dice;
            this.pb_zarGif.Location = new System.Drawing.Point(127, 93);
            this.pb_zarGif.Name = "pb_zarGif";
            this.pb_zarGif.Size = new System.Drawing.Size(136, 70);
            this.pb_zarGif.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pb_zarGif.TabIndex = 0;
            this.pb_zarGif.TabStop = false;
            // 
            // il_zarlar
            // 
            this.il_zarlar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il_zarlar.ImageStream")));
            this.il_zarlar.TransparentColor = System.Drawing.Color.Transparent;
            this.il_zarlar.Images.SetKeyName(0, "1.png");
            this.il_zarlar.Images.SetKeyName(1, "1.png");
            this.il_zarlar.Images.SetKeyName(2, "2.png");
            this.il_zarlar.Images.SetKeyName(3, "3.png");
            this.il_zarlar.Images.SetKeyName(4, "4.png");
            this.il_zarlar.Images.SetKeyName(5, "5.png");
            this.il_zarlar.Images.SetKeyName(6, "6.png");
            // 
            // pb_zar1
            // 
            this.pb_zar1.Location = new System.Drawing.Point(81, 75);
            this.pb_zar1.Name = "pb_zar1";
            this.pb_zar1.Size = new System.Drawing.Size(78, 78);
            this.pb_zar1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pb_zar1.TabIndex = 1;
            this.pb_zar1.TabStop = false;
            this.pb_zar1.Visible = false;
            // 
            // pb_zar2
            // 
            this.pb_zar2.Location = new System.Drawing.Point(240, 75);
            this.pb_zar2.Name = "pb_zar2";
            this.pb_zar2.Size = new System.Drawing.Size(78, 78);
            this.pb_zar2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pb_zar2.TabIndex = 2;
            this.pb_zar2.TabStop = false;
            this.pb_zar2.Visible = false;
            // 
            // tmrZar
            // 
            this.tmrZar.Enabled = true;
            this.tmrZar.Interval = 2000;
            this.tmrZar.Tick += new System.EventHandler(this.tmrZar_Tick);
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(161, 211);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(75, 23);
            this.btn_close.TabIndex = 3;
            this.btn_close.Text = "Tamam";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Visible = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // frm_zar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.DarkGreen;
            this.ClientSize = new System.Drawing.Size(388, 246);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.pb_zar2);
            this.Controls.Add(this.pb_zar1);
            this.Controls.Add(this.pb_zarGif);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_zar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_zar";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pb_zarGif)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_zar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_zar2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_zarGif;
        private System.Windows.Forms.ImageList il_zarlar;
        private System.Windows.Forms.PictureBox pb_zar1;
        private System.Windows.Forms.PictureBox pb_zar2;
        private System.Windows.Forms.Timer tmrZar;
        private System.Windows.Forms.Button btn_close;
    }
}