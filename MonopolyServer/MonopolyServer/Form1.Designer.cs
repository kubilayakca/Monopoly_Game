namespace MonopolyServer
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txt_status = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_baslat = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btn_oyunu_bitir = new System.Windows.Forms.Button();
            this.check_zamanli_mi = new System.Windows.Forms.CheckBox();
            this.txt_saat = new System.Windows.Forms.NumericUpDown();
            this.txt_dakika = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.gb_oyun_suresi = new System.Windows.Forms.GroupBox();
            this.tmr_game_time = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txt_saat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_dakika)).BeginInit();
            this.gb_oyun_suresi.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_status
            // 
            this.txt_status.Location = new System.Drawing.Point(202, 59);
            this.txt_status.Name = "txt_status";
            this.txt_status.ReadOnly = true;
            this.txt_status.Size = new System.Drawing.Size(396, 300);
            this.txt_status.TabIndex = 0;
            this.txt_status.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(216, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "Monopoly Game Server";
            // 
            // btn_baslat
            // 
            this.btn_baslat.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_baslat.Location = new System.Drawing.Point(12, 59);
            this.btn_baslat.Name = "btn_baslat";
            this.btn_baslat.Size = new System.Drawing.Size(170, 36);
            this.btn_baslat.TabIndex = 2;
            this.btn_baslat.Text = "Yeni Oyun Başlat";
            this.btn_baslat.UseVisualStyleBackColor = true;
            this.btn_baslat.Click += new System.EventHandler(this.btn_baslat_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button3.Location = new System.Drawing.Point(12, 188);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(170, 36);
            this.button3.TabIndex = 5;
            this.button3.Text = "Oyunu Kaydet";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button4.Location = new System.Drawing.Point(12, 146);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(170, 36);
            this.button4.TabIndex = 6;
            this.button4.Text = "Kayıtlı Oyunu Aç";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // btn_oyunu_bitir
            // 
            this.btn_oyunu_bitir.Enabled = false;
            this.btn_oyunu_bitir.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btn_oyunu_bitir.Location = new System.Drawing.Point(12, 104);
            this.btn_oyunu_bitir.Name = "btn_oyunu_bitir";
            this.btn_oyunu_bitir.Size = new System.Drawing.Size(170, 36);
            this.btn_oyunu_bitir.TabIndex = 7;
            this.btn_oyunu_bitir.Text = "Oyunu Bitir";
            this.btn_oyunu_bitir.UseVisualStyleBackColor = true;
            // 
            // check_zamanli_mi
            // 
            this.check_zamanli_mi.AutoSize = true;
            this.check_zamanli_mi.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.check_zamanli_mi.Location = new System.Drawing.Point(12, 232);
            this.check_zamanli_mi.Name = "check_zamanli_mi";
            this.check_zamanli_mi.Size = new System.Drawing.Size(150, 30);
            this.check_zamanli_mi.TabIndex = 8;
            this.check_zamanli_mi.Text = "Zamanlı Oyun";
            this.check_zamanli_mi.UseVisualStyleBackColor = true;
            this.check_zamanli_mi.Visible = false;
            this.check_zamanli_mi.CheckedChanged += new System.EventHandler(this.check_zamanli_mi_CheckedChanged);
            // 
            // txt_saat
            // 
            this.txt_saat.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_saat.Location = new System.Drawing.Point(10, 45);
            this.txt_saat.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.txt_saat.Name = "txt_saat";
            this.txt_saat.Size = new System.Drawing.Size(43, 33);
            this.txt_saat.TabIndex = 9;
            // 
            // txt_dakika
            // 
            this.txt_dakika.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txt_dakika.Location = new System.Drawing.Point(91, 45);
            this.txt_dakika.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.txt_dakika.Name = "txt_dakika";
            this.txt_dakika.Size = new System.Drawing.Size(43, 33);
            this.txt_dakika.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(63, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 26);
            this.label2.TabIndex = 11;
            this.label2.Text = "-";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 26);
            this.label3.TabIndex = 12;
            this.label3.Text = "Saat";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(79, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 26);
            this.label4.TabIndex = 12;
            this.label4.Text = "Dakika";
            // 
            // gb_oyun_suresi
            // 
            this.gb_oyun_suresi.Controls.Add(this.label3);
            this.gb_oyun_suresi.Controls.Add(this.label4);
            this.gb_oyun_suresi.Controls.Add(this.txt_saat);
            this.gb_oyun_suresi.Controls.Add(this.txt_dakika);
            this.gb_oyun_suresi.Controls.Add(this.label2);
            this.gb_oyun_suresi.Location = new System.Drawing.Point(12, 267);
            this.gb_oyun_suresi.Name = "gb_oyun_suresi";
            this.gb_oyun_suresi.Size = new System.Drawing.Size(170, 92);
            this.gb_oyun_suresi.TabIndex = 13;
            this.gb_oyun_suresi.TabStop = false;
            this.gb_oyun_suresi.Text = "Oyun Süresi";
            this.gb_oyun_suresi.Visible = false;
            // 
            // tmr_game_time
            // 
            this.tmr_game_time.Interval = 1000;
            this.tmr_game_time.Tick += new System.EventHandler(this.tmr_game_time_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 371);
            this.Controls.Add(this.gb_oyun_suresi);
            this.Controls.Add(this.check_zamanli_mi);
            this.Controls.Add(this.btn_oyunu_bitir);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btn_baslat);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_status);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Monopoly SERVER 1.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txt_saat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_dakika)).EndInit();
            this.gb_oyun_suresi.ResumeLayout(false);
            this.gb_oyun_suresi.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txt_status;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_baslat;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btn_oyunu_bitir;
        private System.Windows.Forms.CheckBox check_zamanli_mi;
        private System.Windows.Forms.NumericUpDown txt_saat;
        private System.Windows.Forms.NumericUpDown txt_dakika;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gb_oyun_suresi;
        private System.Windows.Forms.Timer tmr_game_time;
    }
}

