using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetComm;
using Newtonsoft.Json;



namespace Monopoly
{
    public partial class fm_game : Form
    {

        #region Global Variables
        //nick bir önceki formdan bu formun constructrına geliyor
        string nick;
        ImageList piyonListesi;
        //piyonun ekranda ilk durduğu nokta 
        int piyonX = 695;
        int piyonY = 695;
        //tıklanan kareyi kontol etmek için kullandığım bir global değişken.
        
        MonopolyEntities5 MonopolyDB = new MonopolyEntities5();
        short isClicked = -1;
        Client MonopolyGameClienth;
        fm_waiting BeklemeEkrani;
        Label[] HoverLabel = new Label[41];
        int currentKare;
        List<TapuSenetleri> TapuSenetleriDetayListesi = new List<TapuSenetleri>();
        List<OyunAlani> OyunTahtasi = new List<OyunAlani>();
        List<SansKartlari> SansKartlariListesi = new List<SansKartlari>();
        List<KamuFonuKartlari> KamuFonuKartlariListesi = new List<KamuFonuKartlari>();
        List<TapularinSahipleri> TapuSahipleriListesi = new List<TapularinSahipleri>();
        TapularinSahipleri tempTapuSahibiBilgisi = new TapularinSahipleri();
        TapuSenetleri tempTapu = new TapuSenetleri();
        KayitliOyunDetay BuOyuncu = new KayitliOyunDetay();
        List<TapuSenetleri> tempTapuSenetleriGrubuListesi = new List<TapuSenetleri>();
        int kiraBedeli;
        Color OyuncuRengi;
        int[] X_point = new int[41];
        int[] Y_point = new int[41];
        Color transparanRenk = Color.Transparent;
        KayitliOyunDetay Oyuncu1;
        KayitliOyunDetay Oyuncu2;
        KayitliOyunDetay Oyuncu3;
        PictureBox piyon1;
        PictureBox piyon2;
        PictureBox piyon3;
        int piyon1CurrentKare;
        int piyon2CurrentKare;
        int piyon3CurrentKare;
        string serverID;
        bool siraBuOyuncudaMi;
        int zarlarinToplami;
        bool kamuFonu10kat = false;
        int piyonID;
        int oyuncuSayisi = 0;
        int tiklanilanKart;
        int kamuOdemesi;
        bool oyunBaslangici = true;



        #endregion

        public fm_game(string nickname,int piyonid,ImageList imgl)
        {
           

            InitializeComponent();
            //Windows Form'un bir kaç style özelliğini değiştiriyorum.
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            BuOyuncu.oyuncuID = nickname;

            piyonID = piyonid;
            

            piyonListesi = imgl;
            //nick bir önceki login formundan geliyor.
            nick = nickname;
            
            
            //lbl_oyuncu1_isim.Text = nickname;
            //pb_oyuncu1_piyon.Image = piyonListesi.Images[piyonid];

            //LAbelların noktaları
            initialize_label_points();

            //btn_sira.Image = piyonListesi.Images[piyonID];
            //btn_sira.Image.Size = new System.Drawing.Size(50, 50);

            
            
        }

        


        //windows formun bir fonksiyonuna override ediyorumki transparan olan piyon hareket ederken titreme olmasın
        //bu foknsiyonu stackoverflow'dan tamamen hazır olarak aldım.
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }


        //Form LOAD:
        private void fm_game_Load(object sender, EventArgs e)
        {
            //oyun alanına label'ları çizer.
            CreateLabels();

            //socket connection'ı kuruyorum.
            MonopolyGameClienth = new Client();
            MonopolyGameClienth.SendBufferSize = 3000;
            MonopolyGameClienth.ReceiveBufferSize = 3000;
            MonopolyGameClienth.NoDelay = true;
            MonopolyGameClienth.Connect("127.0.0.1", 6745,nick);

         
            MonopolyGameClienth.DataReceived += MonopolyGameClienth_DataReceived;
            MonopolyGameClienth.Disconnected += MonopolyGameClienth_Disconnected;
            
        

            //burada groupboxları istediğim noktalara yerleştiriyorm:
            tab_aktif.Controls.Add(gb_kartlogo_bilgi);
            gb_kartlogo_bilgi.Location = new Point(6, 6);
            gb_tapuinfo.Controls.Add(gb_satin_al);
            gb_satin_al.Location = new Point(250, 70);
            gb_tapuinfo.Controls.Add(gb_kiraode);
            gb_kiraode.Location = new Point(250, 70);
            tab_kartBilgi.Controls.Add(gb_statik_kartbilgi);
            gb_statik_kartbilgi.Location = new Point(6,2);
            gb_tapubilgi_container.Controls.Add(gb_bilgi_kamu);
            gb_tapubilgi_container.Controls.Add(gb_bilgi_ulasim);
            gb_tapuinfo.Controls.Add(gb_aktif_kamu_kurulusu);
            gb_tapuinfo.Controls.Add(gb_aktif_ulasim_karti);
            gb_kartlogo_bilgi.Controls.Add(gb_kodes);
            gb_tapubilgi_container.Controls.Add(lbl_secili_degil);

            gb_bilgi_kamu.Location = new Point(9, 16);
            gb_kodes.Location = new Point(270, 40);
            gb_bilgi_ulasim.Location = new Point(9, 16);
            gb_aktif_ulasim_karti.Location = new Point(8, 12);
            gb_aktif_kamu_kurulusu.Location = new Point(8, 12);
            //btn_zarAt.Location = new Point(1150, 290);
            btn_devam.Location = new Point(1100 ,350);
            

            sendInfoToServer("piyonBilgisi", piyonID);

            //ekrana bir bekleyiniz yazısı çıkartıyorum. server'dan cevap gelene kadar bu yazı kalıyor
            BeklemeEkrani = new fm_waiting();
            this.Hide();
            BeklemeEkrani.StartPosition = FormStartPosition.CenterScreen;
            BeklemeEkrani.ShowDialog();


            TapuSenetleriDetayListesi = MonopolyDB.TapuSenetleri.ToList();
            OyunTahtasi = MonopolyDB.OyunAlani.ToList();
            SansKartlariListesi = MonopolyDB.SansKartlari.ToList();
            KamuFonuKartlariListesi = MonopolyDB.KamuFonuKartlari.ToList();


            showCardinfo(kareninBilgisiniGetir(currentKare));

        }

        void MonopolyGameClienth_Disconnected()
        {
            statusYaziEkle("Bağlantı koptu.");
        }

        //Labelları yaratan fonksiyon.
        private void CreateLabels()
        {

            int ynoktasi = 691;
            int xnoktasi = 691;
            for (int i = 0; i <= 30; i += 10)
            {
                if (i == 10)
                {
                    ynoktasi = 691;
                    xnoktasi = 0;
                }
                else if (i == 20)
                {
                    ynoktasi = 0;
                    xnoktasi = 0;
                }
                else if (i == 30)
                {
                    ynoktasi = 0;
                    xnoktasi = 691;
                }
                //bu bolum tam köşedekileri yazdırır.

                HoverLabel[i] = new System.Windows.Forms.Label();
                HoverLabel[i].BackColor = System.Drawing.Color.Transparent;
                HoverLabel[i].Location = new System.Drawing.Point(xnoktasi, ynoktasi);
                HoverLabel[i].Font = new Font(Font.FontFamily, 1);
                pb_playboard.Controls.Add(HoverLabel[i]);
                HoverLabel[i].Size = new System.Drawing.Size(104, 104);
                HoverLabel[i].TabIndex = 5;
                HoverLabel[i].Text = i.ToString();
                HoverLabel[i].MouseHover += fm_game_MouseHover;
                HoverLabel[i].MouseLeave += fm_game_MouseLeave;
                HoverLabel[i].Click += fm_game_Click;
                HoverLabel[i].Tag = transparanRenk;
            }

            // bu for, 2 satırı yazdırmak için.
            xnoktasi = 106;
            for (int i = 9; i > 0; i--)
            {



                //bu bolum aşadağıdaki satırı yazdırır.

                HoverLabel[i] = new System.Windows.Forms.Label();
                HoverLabel[i].BackColor = System.Drawing.Color.Transparent;
                HoverLabel[i].Location = new System.Drawing.Point(xnoktasi, 691);

                pb_playboard.Controls.Add(HoverLabel[i]);
                HoverLabel[i].Size = new System.Drawing.Size(63, 104);
                HoverLabel[i].TabIndex = 5;
                HoverLabel[i].Text = i.ToString();
                HoverLabel[i].MouseHover += fm_game_MouseHover;
                HoverLabel[i].MouseLeave += fm_game_MouseLeave;
                HoverLabel[i].Click += fm_game_Click;
                HoverLabel[i].Font = new Font(Font.FontFamily, 1);
                HoverLabel[i].Tag = transparanRenk;


                //bu bolum üst satırı yazdırır.

                HoverLabel[30 - i] = new System.Windows.Forms.Label();
                HoverLabel[30 - i].BackColor = System.Drawing.Color.Transparent;
                HoverLabel[30 - i].Location = new System.Drawing.Point(xnoktasi, 0);
                HoverLabel[30 - i].Font = new Font(Font.FontFamily, 1);
                pb_playboard.Controls.Add(HoverLabel[30 - i]);
                HoverLabel[30 - i].Size = new System.Drawing.Size(63, 104);
                HoverLabel[30 - i].TabIndex = 5;
                HoverLabel[30 - i].Text = (30 - i).ToString();
                HoverLabel[30 - i].MouseHover += fm_game_MouseHover;
                HoverLabel[30 - i].MouseLeave += fm_game_MouseLeave;
                HoverLabel[30 - i].Click += fm_game_Click;
                HoverLabel[30 - i].Tag = transparanRenk;


                xnoktasi += 65;
            }

            // bu for, 2 sütunu yazdırmak için.
            ynoktasi = 106;
            for (int i = 19; i > 10; i--)
            {

                //bu bolum soldaki sütunu yazdırır.

                HoverLabel[i] = new System.Windows.Forms.Label();
                HoverLabel[i].BackColor = System.Drawing.Color.Transparent;
                HoverLabel[i].Location = new System.Drawing.Point(0, ynoktasi);

                pb_playboard.Controls.Add(HoverLabel[i]);
                HoverLabel[i].Size = new System.Drawing.Size(104, 63);
                HoverLabel[i].TabIndex = 5;
                HoverLabel[i].Text = i.ToString();
                HoverLabel[i].MouseHover += fm_game_MouseHover;
                HoverLabel[i].MouseLeave += fm_game_MouseLeave;
                HoverLabel[i].Click += fm_game_Click;

                HoverLabel[i].Font = new Font(Font.FontFamily, 1);
                HoverLabel[i].Tag = transparanRenk;

                //bu bolum sağdaki sütunu yazdırır.

                HoverLabel[50 - i] = new System.Windows.Forms.Label();
                HoverLabel[50 - i].BackColor = System.Drawing.Color.Transparent;
                HoverLabel[50 - i].Location = new System.Drawing.Point(690, ynoktasi);
                HoverLabel[50 - i].Font = new Font(Font.FontFamily, 1);
                pb_playboard.Controls.Add(HoverLabel[50 - i]);
                HoverLabel[50 - i].Size = new System.Drawing.Size(104, 63);
                HoverLabel[50 - i].TabIndex = 5;
                HoverLabel[50 - i].Text = (50 - i).ToString();
                HoverLabel[50 - i].MouseHover += fm_game_MouseHover;
                HoverLabel[50 - i].MouseLeave += fm_game_MouseLeave;
                HoverLabel[50 - i].Click += fm_game_Click;
                HoverLabel[50 - i].Tag = transparanRenk;

                ynoktasi += 65;


            }
        }
        

        //Piyonun Labellar üzerinde durduğu noktalar:
        private void initialize_label_points()
        {
            X_point[0] = 720;
            Y_point[0] = 715;

            X_point[1] = 637;
            Y_point[1] = 745;

            X_point[2] = 573;
            Y_point[2] = 745;

            X_point[3] = 507;
            Y_point[3] = 745;

            X_point[4] = 442;
            Y_point[4] = 745;

            X_point[5] = 377;
            Y_point[5] = 745;

            X_point[6] = 312;
            Y_point[6] = 754;

            X_point[7] = 247;
            Y_point[7] = 745;

            X_point[8] = 182;
            Y_point[8] = 745;

            X_point[9] = 117;
            Y_point[9] = 745;

            X_point[10] = -5;
            Y_point[10] = 745;

            X_point[11] = 10;
            Y_point[11] = 647;

            X_point[12] = 10;
            Y_point[12] = 577;

            X_point[13] = 10;
            Y_point[13] = 517;

            X_point[14] = 10;
            Y_point[14] = 450;

            X_point[15] = 10;
            Y_point[15] = 378;

            X_point[16] = 10;
            Y_point[16] = 320;

            X_point[17] = 10;
            Y_point[17] = 250;

            X_point[18] = 10;
            Y_point[18] = 190;

            X_point[19] = 10;
            Y_point[19] = 125;

            X_point[20] = 20;
            Y_point[20] = 20;

            X_point[21] = 115;
            Y_point[21] = 10;

            X_point[22] = 180;
            Y_point[22] = 10;

            X_point[23] = 245;
            Y_point[23] = 10;

            X_point[24] = 310;
            Y_point[24] = 5;

            X_point[25] = 377;
            Y_point[25] = 10;

            X_point[26] = 442;
            Y_point[26] = 10;

            X_point[27] = 507;
            Y_point[27] = 10;

            X_point[28] = 572;
            Y_point[28] = 10;

            X_point[29] = 637;
            Y_point[29] = 10;

            X_point[30] = 725;
            Y_point[30] = 23;

            X_point[31] = 725;
            Y_point[31] = 127;

            X_point[32] = 725;
            Y_point[32] = 190;

            X_point[33] = 740;
            Y_point[33] = 245;

            X_point[34] = 725;
            Y_point[34] = 322;

            X_point[35] = 743;
            Y_point[35] = 376;

            X_point[36] = 700;
            Y_point[36] = 445;

            X_point[37] = 727;
            Y_point[37] = 517;

            X_point[38] = 740;
            Y_point[38] = 570;

            X_point[39] = 735;
            Y_point[39] = 636;

            //40 Numara kodes için:
            X_point[40] = 45;
            Y_point[40] = 705;


        }

        //Piyonları yaratan fonksiyon.
       
        void createPiyon(int piyonid, int oyuncuID)
        {
            //piyonu yaratıyorum

            switch (oyuncuID)
            {
                case 1:
                    piyon1 = new System.Windows.Forms.PictureBox();
                    piyon1.BackColor = System.Drawing.Color.Transparent;
                    piyon1.Location = new System.Drawing.Point(piyonX, piyonY);
                    piyon1.Name = "piyon1";
                    piyon1.Size = new System.Drawing.Size(40, 40);
                    piyon1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                    piyon1.TabIndex = 4;
                    piyon1.TabStop = false;
                    piyon1.Image = piyonListesi.Images[piyonid];
                    pb_playboard.Controls.Add(piyon1);
                    piyon1.BringToFront();
                    break;
                case 2:
                    piyon2 = new System.Windows.Forms.PictureBox();
                    piyon2.BackColor = System.Drawing.Color.Transparent;
                    piyon2.Location = new System.Drawing.Point(piyonX + 50, piyonY);
                    piyon2.Name = "piyon2";
                    piyon2.Size = new System.Drawing.Size(40, 40);
                    piyon2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                    piyon2.TabIndex = 5;
                    piyon2.TabStop = false;
                    piyon2.Image = piyonListesi.Images[piyonid];
                    pb_playboard.Controls.Add(piyon2);
                    piyon2.BringToFront();
                    break;
                case 3:
                    piyon3 = new System.Windows.Forms.PictureBox();
                    piyon3.BackColor = System.Drawing.Color.Transparent;
                    piyon3.Location = new System.Drawing.Point(piyonX, piyonY + 50);
                    piyon3.Name = "piyon3";
                    piyon3.Size = new System.Drawing.Size(40, 40);
                    piyon3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                    piyon3.TabIndex = 6;
                    piyon3.TabStop = false;
                    piyon3.Image = piyonListesi.Images[piyonid];
                    pb_playboard.Controls.Add(piyon3);
                    piyon3.BringToFront();
                    break;

            }

        }
    


        //verilen Kare ID'nin tapu bilgisini return eder.
        public TapuSenetleri kareninBilgisiniGetir (int ID)
        {
            //answer answerForRequest = new answer();
            //answerForRequest.type = "kartBilgi";
            TapuSenetleri istenenKareninBilgisi = new TapuSenetleri();
            OyunAlani requestedKare = OyunTahtasi.Where(searchID => searchID.id == ID).FirstOrDefault();
            istenenKareninBilgisi = requestedKare.TapuSenetleri;
            return istenenKareninBilgisi;
        }

        //Bu Fonksiyon piyonun ilerleyip geldiği karenin bilgisini Aktif Kart tabında gösterir.
        
        private void showCardinfo(TapuSenetleri gelenTapuSenedi)
        {

            gb_aktif_ulasim_karti.Visible = false;
            gb_aktif_kamu_kurulusu.Visible = false;
            gb_kartlogo_bilgi.Visible = false;
            btn_devam.Visible = false;
            btn_kira_devam.Visible = false;
            lbl_M_logo.Visible = true;

            //yani alınabilir bir kart ise: 
            if(gelenTapuSenedi.kartTipi==1)
            {
                
               
                groupBoxlariGizle();
                gb_tapu.Visible = true;
                lbl_kart_top.BackColor = kartRengi(gelenTapuSenedi);
                lbl_kart_isim.Text = gelenTapuSenedi.tapu_ismi;
                lbl_arsa_kira.Text = gelenTapuSenedi.arsa_kira.ToString();
                lbl_1ev_kira.Text = gelenTapuSenedi.ev1.ToString();
                lbl_2ev_kira.Text = gelenTapuSenedi.ev2.ToString();
                lbl_3ev_kira.Text = gelenTapuSenedi.ev3.ToString();
                lbl_4ev_kira.Text = gelenTapuSenedi.ev4.ToString();
                lbl_otel_kira.Text = gelenTapuSenedi.otel_kira.ToString();
                lbl_ipotek_fiyat.Text = (gelenTapuSenedi.fiyati / 2).ToString();
                lbl_ev_maliyeti.Text = gelenTapuSenedi.ev_maliyeti.ToString();
                lbl_otel_maliyeti.Text = gelenTapuSenedi.ev_maliyeti.ToString();

                try
                {

                    if (TapuSahipleriListesi.Where(search => search.tapu_id == gelenTapuSenedi.id).SingleOrDefault() == null)
                    {
                        groupBoxlariGizle();
                        gb_tapuinfo.Visible = true;
                        gb_satin_al.Visible = true;
                        gb_satin_al.BringToFront();
                        lbl_satinal_fiyat.Text = gelenTapuSenedi.fiyati.ToString();
                    }
                    else
                    {
                        tempTapuSahibiBilgisi = TapuSahipleriListesi.Where(search => search.tapu_id == gelenTapuSenedi.id).SingleOrDefault();
                        groupBoxlariGizle();                      
                        gb_kiraode.Visible = true;
                        gb_tapuinfo.Visible = true;
                        btn_kiraOde.Visible = true;
                        lbl_gbkiraode_sahibi.Text = tempTapuSahibiBilgisi.oyuncu_id;
                        if (tempTapuSahibiBilgisi.oyuncu_id != nick)
                        {
                            if(tempTapuSahibiBilgisi.ipotekliMi=="1")
                            {
                                lbl_gbkiraode_kira.Text = "İPOTEKLİ";
                                lbl_M_logo.Visible = false;
                                btn_kira_devam.Visible = true;
                                btn_kiraOde.Visible = false;
                            }
                            else
                            {
                                kiraBedeli = kirayiHesapla(tempTapuSahibiBilgisi.tapu_id, tempTapuSahibiBilgisi.oyuncu_id);
                                lbl_gbkiraode_kira.Text = kiraBedeli.ToString();
                            }
                            
                        }
                        else
                        {
                            groupBoxlariGizle();
                            gb_tapusenin.Visible = true;
                            gb_tapuinfo.Visible=true;
                            btn_devam.Visible = true;
        
                        }
                        



                    }
                }
                catch
                {   
                    MessageBox.Show("Üzgünüz, tapuyu bulmakta bir  yaşadık  :(","Tapu Bulunamadı");
                }
            }
            else if (gelenTapuSenedi.kartTipi == 4 || gelenTapuSenedi.kartTipi == 5)
            {
                //gelenTapuSenedi.kartTipi 4 ise ulaşım aracıdır, 5 ise kamu kuruluşudur.

                groupBoxlariGizle();
                gb_tapu.Visible = false;
                

                if (gelenTapuSenedi.kartTipi == 4)
                {
                    gb_aktif_ulasim_karti.Visible = true;
                    gb_aktif_kamu_kurulusu.Visible = false;
                    lbl_ulasim_kartismi.Text = gelenTapuSenedi.tapu_ismi;
                    // id 3 ise haydarpaşa, 26 ise sirkeci tren istasyonu, 11 ise kadıköy, 18 ise kabataş vapur iskelesidir.
                    if(gelenTapuSenedi.id==3 || gelenTapuSenedi.id==26)
                    {
                        
                        pb_ulasim_resim.Image = Monopoly.Properties.Resources.tren;
                    }
                    else
                    {
                        pb_ulasim_resim.Image = Monopoly.Properties.Resources.deniz;

                    }
                }
                else
                {
                    gb_aktif_ulasim_karti.Visible = false;
                    gb_aktif_kamu_kurulusu.Visible = true;
                    lbl_kamu_kurulusu_kartismi.Text = gelenTapuSenedi.tapu_ismi;
                    //eğer id 8 ise tedaş,21 ise iski su.
                    if(gelenTapuSenedi.id==21)
                    {
                        pb_kamu_kurulusu.Image = Monopoly.Properties.Resources.iski;
                    }
                    else
                    {
                        pb_kamu_kurulusu.Image = Monopoly.Properties.Resources.tedas;
                    }
                }

                pb_kamu_kurulusu.SizeMode = PictureBoxSizeMode.Zoom;
                pb_ulasim_resim.SizeMode = PictureBoxSizeMode.Zoom;
                

                if (TapuSahipleriListesi.Where(search => search.tapu_id == gelenTapuSenedi.id).SingleOrDefault() == null)
                {
                    groupBoxlariGizle();
                    gb_tapuinfo.Visible = true;
                    gb_satin_al.Visible = true;
                    gb_satin_al.BringToFront();
                    lbl_satinal_fiyat.Text = gelenTapuSenedi.fiyati.ToString();
                }
                else
                {
                    tempTapuSahibiBilgisi = TapuSahipleriListesi.Where(search => search.tapu_id == gelenTapuSenedi.id).SingleOrDefault();
                    groupBoxlariGizle();
                    gb_kiraode.Visible = true;
                    gb_tapuinfo.Visible = true;
                    btn_kiraOde.Visible = true;
                    lbl_gbkiraode_sahibi.Text = tempTapuSahibiBilgisi.oyuncu_id;
                    if (tempTapuSahibiBilgisi.oyuncu_id != nick)
                    {
                        if (tempTapuSahibiBilgisi.ipotekliMi == "1")
                        {
                            lbl_gbkiraode_kira.Text = "İPOTEKLİ";
                            lbl_M_logo.Visible = false;
                            btn_kira_devam.Visible = true;
                            btn_kiraOde.Visible = false;
                        }
                        else
                        {
                            kiraBedeli = kirayiHesapla(tempTapuSahibiBilgisi.tapu_id, tempTapuSahibiBilgisi.oyuncu_id);
                            lbl_gbkiraode_kira.Text = kiraBedeli.ToString();
                        }
                    }
                    else
                    {
                        groupBoxlariGizle();
                        gb_tapusenin.Visible = true;
                        gb_tapuinfo.Visible = true;
                        btn_devam.Visible = true;
                    }




                }
            }
            else
            {
                groupBoxlariGizle();
                gb_tapu.Visible = false;
                gb_tapuinfo.Visible = true;
                gb_kartlogo_bilgi.Visible = true;
                gb_kartlogo_bilgi.BringToFront();
                btn_devam.BringToFront();
                gb_kodes.Visible = false;
                switch (gelenTapuSenedi.kartTipi)
                {
                    case 2:
                        lbl_kart_bilgi.Text = "Bir Kamu Fonu kartı çek!";
                       
                        kartBilgiButonlariniGizle("Kamu Fonu Kartı Çek");
                        pb_kartlogo.Image = Monopoly.Properties.Resources.kamu_fonu;
                        break;
                    case 3:
                        lbl_kart_bilgi.Text = "Bir Şans kartı çek!";
                        kartBilgiButonlariniGizle("Şans Kartı Çek");
                        pb_kartlogo.Image = Monopoly.Properties.Resources.sans;
                        break;
                    case 6:
                        lbl_kart_bilgi.Text = "Başlangıç noktasındasın!";
                        pb_kartlogo.Image = Monopoly.Properties.Resources.baslangic;
                        kartBilgiButonlariniGizle("Başlangıç");
                        if (oyunBaslangici == false)
                        {
                            btn_devam.Visible = true;
                        }
                        
                        break;
                    case 7:
                        if(currentKare>12)
                        {
                            lbl_kart_bilgi.Text = "Ücretsiz Otopark!";
                            pb_kartlogo.Image = Monopoly.Properties.Resources.otopark;                            
                            kartBilgiButonlariniGizle("Ücretsiz Otopark");
                            btn_devam.Visible = true;
                        }
                        else if(BuOyuncu.hapisahenedeMi=="1")
                        {
                            //burası kodeste olma durumu
                            lbl_kart_bilgi.Text = "Kodestesin!";
                            pb_kartlogo.Image = Monopoly.Properties.Resources.kodes;                            
                            kartBilgiButonlariniGizle("Kodestesin");
                            if(siraBuOyuncudaMi == true)
                            {
                                gb_kodes.Visible = true;
                                gb_kodes.BringToFront();
                            }
                            
                            if(BuOyuncu.kodestenCikKartiSayisi>0)
                            {
                                btn_kodeskarti_kullan.Enabled = true;
                            }
                            else
                            {
                                btn_kodeskarti_kullan.Enabled = false;

                            }

                        }
                        else
                        {
                            lbl_kart_bilgi.Text = "Kodeste ziyaretçisin!";
                            pb_kartlogo.Image = Monopoly.Properties.Resources.kodes;                            
                            kartBilgiButonlariniGizle("Kodeste Ziyaretçisin");
                            btn_devam.Visible = true;
                        }
                        
                        break;
                    case 8:
                        lbl_kart_bilgi.Text = "Doğru Kodese Gir!";
                        pb_kartlogo.Image = Monopoly.Properties.Resources.kodesegir;
                        kartBilgiButonlariniGizle("Kodes'e Gir");
                        break;
                    case 9:
                        
                        if(currentKare>10)
                        {
                            pb_kartlogo.Image = Monopoly.Properties.Resources.luks_vergi;                            
                            lbl_kart_bilgi.Text = "Lüks Vergisi ödemelisin!";
                            kartBilgiButonlariniGizle("Lüks Vergisi Öde");
                        }
                        else
                        {
                            pb_kartlogo.Image = Monopoly.Properties.Resources.gelir_vergi;
                            lbl_kart_bilgi.Text = "Gelir Vergisi ödemelisin!";
                            kartBilgiButonlariniGizle("Gelir Vergisi Öde");    
                        }
                        break;
                }
            }
        }
        

        //tapunun daha önce alınıp alnımmadığını kontrol eden fonksiyon
        private bool tapuAlinmismikontrolEt(TapuSenetleri tapuSenedi)
        {
            if (TapuSahipleriListesi.Where(search => search.tapu_id == tapuSenedi.id).SingleOrDefault() == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        //bu fonksiyon üzerine tıklanılan kartin bilgisini gösterir.
        private void kartBilgisiniGoster(TapuSenetleri gelenTapuSenedi, int tiklananKartID)
        {
            tiklanilanKart = tiklananKartID;
            gb_kartbilgi_sahibi.Visible = true;
            gb_tapusenedi_bilgi.Visible = true;
            gb_tapuislemleri.Visible = false;
            gb_bilgi_kamu.Visible = false;
            gb_bilgi_ulasim.Visible = false;
            //yani alınabilir bir kart ise: 
            if (gelenTapuSenedi.kartTipi == 1 || gelenTapuSenedi.kartTipi == 4 || gelenTapuSenedi.kartTipi == 5)
            {
               if(gelenTapuSenedi.kartTipi==1)
               {
                   lbl_kartbilgi_top.BackColor = kartRengi(gelenTapuSenedi);
                   lbl_kartbilgi_isim.Text = gelenTapuSenedi.tapu_ismi;
                   lbl_kartbilgi_arsakira.Text = gelenTapuSenedi.arsa_kira.ToString();
                   lbl_kartbilgi_1ev.Text = gelenTapuSenedi.ev1.ToString();
                   lbl_kartbilgi_2ev.Text = gelenTapuSenedi.ev2.ToString();
                   lbl_kartbilgi_3ev.Text = gelenTapuSenedi.ev3.ToString();
                   lbl_kartbilgi_4ev.Text = gelenTapuSenedi.ev4.ToString();
                   lbl_kartbilgi_otelkira.Text = gelenTapuSenedi.otel_kira.ToString();
                   lbl_kartbilgi_ipotek.Text = (gelenTapuSenedi.fiyati / 2).ToString();
                   lbl_kartbilgi_evmaliyeti.Text = gelenTapuSenedi.ev_maliyeti.ToString();
                   lbl_kartbilgi_otelmaliyeti.Text = gelenTapuSenedi.ev_maliyeti.ToString();
               }
               else if (gelenTapuSenedi.kartTipi==4)
               {
                   gb_tapusenedi_bilgi.Visible = false;
                   gb_bilgi_ulasim.Visible = true;
                   lbl_bilgi_ulasim_kartismi.Text = gelenTapuSenedi.tapu_ismi;
                   if(gelenTapuSenedi.id==11 || gelenTapuSenedi.id==18)
                   {
                       pb_bilgi_ulasim.Image = Monopoly.Properties.Resources.deniz;
                   }
                   else
                   {
                       pb_bilgi_ulasim.Image = Monopoly.Properties.Resources.tren;
                   }
               }
               else
               {
                   gb_tapusenedi_bilgi.Visible=false;
                   gb_bilgi_kamu.Visible=true;
                   lbl_bilgi_kamu_kartismi.Text = gelenTapuSenedi.tapu_ismi;
                   //yani tedaş elektrik:
                   if(gelenTapuSenedi.id ==8)
                   {
                       pb_bilgi_kamu.Image = Monopoly.Properties.Resources.tedas;
                   }
                   else
                   {
                       pb_bilgi_kamu.Image = Monopoly.Properties.Resources.iski;
                   }
               }
                

                try
                {
                    gb_tapubilgi_container.Visible = true;
                    gb_statik_kartbilgi.Visible = false;
                    btn_ipotek_kaldir.Tag = gelenTapuSenedi;
                    btn_ev_iade.Tag = gelenTapuSenedi;
                    btn_otel_kur.Tag = gelenTapuSenedi;
                    btn_otel_iade.Tag = gelenTapuSenedi;
                    if (TapuSahipleriListesi.Where(search => search.tapu_id == gelenTapuSenedi.id).SingleOrDefault() == null)
                    {

                        
                        lbl_kartbilgi_sahibi.Text = "BANKA";
                        gb_tapuislemleri.Visible = false;
                    }
                    else
                    {
                        tempTapuSahibiBilgisi = TapuSahipleriListesi.Where(search => search.tapu_id == gelenTapuSenedi.id).SingleOrDefault();
                        lbl_kartbilgi_sahibi.Text = tempTapuSahibiBilgisi.oyuncu_id;
                        //Yani bu kart bu oyuncuya ait ise:
                        if (tempTapuSahibiBilgisi.oyuncu_id == nick && siraBuOyuncudaMi==true)
                        {

                            gb_tapuislemleri.Visible = true;
                            btn_ipotek_ettir.Tag = gelenTapuSenedi;

                            if(tempTapuSahibiBilgisi.ipotekliMi=="1")
                            {
                                //btn_ipotek_ettir.Visible = false;
                                //btn_ipotek_kaldir.Visible = true;
                                btn_ev_kur.Enabled = false;
                                btn_otel_kur.Enabled = false;
                                btn_ev_iade.Enabled = false;
                                btn_otel_iade.Enabled = false;
                              
                                btn_ipotek_kaldir.Enabled = true;
                                btn_ipotek_ettir.Enabled = false;
                                
                                
                            }
                            else
                            {

                                btn_ipotek_ettir.Enabled = true;

                                btn_ipotek_kaldir.Enabled = false;

                                if (tempTapuSahibiBilgisi.evSayisi > 0 && evDurumSorgula("eviade", gelenTapuSenedi.id, nick, tempTapuSahibiBilgisi.evSayisi))
                                {
                                    btn_ev_iade.Enabled = true;
                                    btn_ipotek_ettir.Enabled = false;
                                   
                                }
                                else{
                                    btn_ev_iade.Enabled = false;
                                }

                                if (evDurumSorgula("evsatış",gelenTapuSenedi.id, nick,tempTapuSahibiBilgisi.evSayisi))
                                {
                                    // burada ev alabilir falan filan;
                                    btn_ev_kur.Enabled = true;
                                    btn_ev_kur.Tag = gelenTapuSenedi;
                                    //else if(tumTumGrubaSahipMi(gelenTapuSenedi.id,nick))
                                    if (tempTapuSahibiBilgisi.evSayisi == 4)
                                    {
                                        btn_ev_kur.Enabled = false;
                                        if (evDurumSorgula("eviade", gelenTapuSenedi.id, nick, tempTapuSahibiBilgisi.evSayisi))
                                        {
                                            btn_ev_iade.Enabled = true;
                                        }
                                        
                                        btn_otel_kur.Enabled = true;
                                        btn_otel_iade.Enabled = false;
                                    }
                                    if (tempTapuSahibiBilgisi.evSayisi == 5)
                                    {
                                        btn_otel_kur.Enabled = false;
                                        btn_otel_iade.Enabled = true;
                                        btn_ev_kur.Enabled = false;
                                        btn_ev_iade.Enabled = false;
                                    }
                                }
                                else
                                {
                                    btn_ev_kur.Enabled = false;
                                    btn_otel_kur.Enabled = false;
                                }
                            }
                        }
                       
                    }
                }
                catch
                {
                    MessageBox.Show("Çok üzgünüz ancak bir hata oluştu ve tapuyu bulamadık :(","Tapu Bulunamadı");
                }
            }
            else
            {
                //yani statik bir kart ise:

                gb_tapubilgi_container.Visible = false;
                gb_statik_kartbilgi.Visible = true;
                switch (gelenTapuSenedi.kartTipi)
                {
                    case 2:
                        lbl_statikkart.Text = "Bir Kamu Fonu kartı çek!";
                        pb_statikkart.Image = Monopoly.Properties.Resources.kamu_fonu;
                        break;
                    case 3:
                        lbl_statikkart.Text = "Bir Şans kartı çek!";
                        pb_statikkart.Image = Monopoly.Properties.Resources.sans;
                        break;
                    case 6:
                        lbl_statikkart.Text = "Başlangıç noktasındasın!";
                        pb_statikkart.Image = Monopoly.Properties.Resources.baslangic;
                        break;
                    case 7:
                        if (tiklananKartID > 12)
                        {
                            lbl_statikkart.Text = "Ücretsiz Otopark!";
                            pb_statikkart.Image = Monopoly.Properties.Resources.otopark;
                        }
                        else
                        {
                            lbl_statikkart.Text = "Kodeste ziyaretçisin!";
                            pb_statikkart.Image = Monopoly.Properties.Resources.kodes;
                        }

                        break;
                    case 8:
                        lbl_statikkart.Text = "Doğru Kodese Gir!";
                        pb_statikkart.Image = Monopoly.Properties.Resources.kodesegir;
                        break;
                    case 9:

                        if (tiklananKartID > 10)
                        {
                            pb_statikkart.Image = Monopoly.Properties.Resources.luks_vergi;
                            lbl_statikkart.Text = "Lüks Vergisi ödemelisin!";
                        }
                        else
                        {
                            pb_statikkart.Image = Monopoly.Properties.Resources.gelir_vergi;
                            lbl_statikkart.Text = "Gelir Vergisi ödemelisin!";
                        }
                        break;
                }
            }
            if (gelenTapuSenedi.kartTipi != 1)
            {
                btn_ev_kur.Enabled = false;
            }


        }

        //arsa üzerine ev yapılabilir mi yapılamaz mı, bunu donduren fonsiyon.
        private bool evDurumSorgula(string type,int tapuSenedi, string oyuncuNick,int tapununEvSayisi)
        {
            //tumTumGrubaSahipMi(tapuSenedi, oyuncuNick);
            int tapuSayac = 0;
            int enAzEvSayisi = 10;
            int enCokEvSayisi = 0;
            tempTapu = TapuSenetleriDetayListesi.Where(search => search.id == tapuSenedi).SingleOrDefault();
            tempTapuSenetleriGrubuListesi = TapuSenetleriDetayListesi.Where(search => search.renk == tempTapu.renk).ToList();

            foreach (TapuSenetleri tapu in tempTapuSenetleriGrubuListesi)
            {
                if (TapuSahipleriListesi.Where(search => search.tapu_id == tapu.id && search.oyuncu_id == oyuncuNick).SingleOrDefault() != null)
                {
                    TapularinSahipleri tempTapu1 = new TapularinSahipleri();
                    tempTapu1 = TapuSahipleriListesi.Where(search => search.tapu_id == tapu.id && search.oyuncu_id == oyuncuNick).Single();
                    if (tempTapu1.ipotekliMi!="1")
                    {
                        
                        tapuSayac++;
                        if (tempTapu1.evSayisi <= enAzEvSayisi)
                        {
                            enAzEvSayisi = tempTapu1.evSayisi;
                        }
                        if (tempTapu1.evSayisi > enCokEvSayisi)
                        {
                            enCokEvSayisi = tempTapu1.evSayisi;
                        }
                    }
                    else
                    {
                        break;
                    }
                    
                }
                
            }

            if(type=="evsatış")
            {
                if (tapuSayac == tempTapu.grup_sayisi && tapununEvSayisi == enAzEvSayisi)
                {
                    //MessageBox.Show("ev yapabilir");
                    return true;
                }
                else
                {
                    //MessageBox.Show("ev yapamaz!");

                    return false;
                }
            }
            else
            {
                if (tapuSayac == tempTapu.grup_sayisi && tapununEvSayisi == enCokEvSayisi)
                {
                    //MessageBox.Show("ev yapabilir");
                    return true;
                }
                else
                {
                    //MessageBox.Show("ev yapamaz!");

                    return false;
                }
            }

            
        }
        

        //oyuncunun renk grubundaki tüm arsalara sahip olup olmadığını kontrol eden method
        private bool tumTumGrubaSahipMi(int tapu_ID, string oyuncu_ID)
        {
            int tapuSayac = 0;
            tempTapu = TapuSenetleriDetayListesi.Where(search => search.id == tapu_ID).SingleOrDefault();
            tempTapuSenetleriGrubuListesi = TapuSenetleriDetayListesi.Where(search => search.renk == tempTapu.renk).ToList();

            foreach (TapuSenetleri tapu in tempTapuSenetleriGrubuListesi)
            {
                if (TapuSahipleriListesi.Where(search => search.tapu_id == tapu.id && search.oyuncu_id == oyuncu_ID).SingleOrDefault() != null)
                {
                    tapuSayac++;
                }
            }

            if (tapuSayac == tempTapu.grup_sayisi)
            {
               
                return true;
            }
            else
            {
              

                return false;
            }
        }

        //oyuncunun kas istasyonu var
        private int KacIstasyonuVar (int tapu_ID, string oyuncu_ID)
        {
            int tapuSayac = 0;
            tempTapu = TapuSenetleriDetayListesi.Where(search => search.id == tapu_ID).SingleOrDefault();
            tempTapuSenetleriGrubuListesi = TapuSenetleriDetayListesi.Where(search => search.renk == tempTapu.renk).ToList();

            foreach (TapuSenetleri tapu in tempTapuSenetleriGrubuListesi)
            {
                if (TapuSahipleriListesi.Where(search => search.tapu_id == tapu.id && search.oyuncu_id == oyuncu_ID).SingleOrDefault() != null)
                {
                    tapuSayac++;
                }
            }

            return tapuSayac;
        }

        //verilecek kirayi hesaplar
        private int kirayiHesapla(int tapuID,string oyuncuID)
        {
            
            int kira=0;
            tempTapu = TapuSenetleriDetayListesi.Where(search=> search.id == tapuID).SingleOrDefault();

            if(tempTapu.kartTipi==4)
            {
                 //yani bu bir ulaşım aracıysa, kaç istasyonu var ona bakıcaz:
                 switch (KacIstasyonuVar(tapuID, oyuncuID))
                 {
                     case 1:
                         kira = 25;
                         break;
                     case 2:
                         kira = 50;
                         break;
                     case 3:
                         kira = 100;
                         break;
                     case 4:
                         kira = 200;
                         break;
                 }
                 return kira;
             }
            else
            {
                if (tumTumGrubaSahipMi(tapuID, oyuncuID))
                {
                    //MessageBox.Show("hepsi bu adamın  - ev sayisi : " + TapuSahipleriListesi.Where(search => search.tapu_id == tapuID).Single().evSayisi.ToString());
                    //yani bu bir kamu kuruluşuysa:
                    if (tempTapu.kartTipi == 5)
                    {
                        kira = zarlarinToplami * 10;
                    }
                    else
                    {
                        switch (TapuSahipleriListesi.Where(search => search.tapu_id == tapuID).Single().evSayisi)
                        {
                            case 0:
                                kira = (int)tempTapu.arsa_kira * 2;
                                break;
                            case 1:
                                kira = (int)tempTapu.ev1;
                                break;
                            case 2:
                                kira = (int)tempTapu.ev2;
                                break;
                            case 3:
                                kira = (int)tempTapu.ev3;
                                break;
                            case 4:
                                kira = (int)tempTapu.ev4;
                                break;
                            case 5:
                                kira = (int)tempTapu.otel_kira;
                                break;
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("hepsi bu adamın  değil");

                    if (tempTapu.kartTipi == 5)
                    {
                        if(kamuFonu10kat==true)
                        {
                            kamuFonu10kat = false;
                            kira = zarlarinToplami * 10;
                        }
                        else
                        {
                            kira = zarlarinToplami * 4;
                        }
                        
                    }
                    else
                    {
                        kira = (int)tempTapu.arsa_kira;
                    }
                }

            }

            return kira;
 
        }

        //kart bilgi butonlarini gizler (çok işe yaramadı :) )
        private void kartBilgiButonlariniGizle(string butonText) 
        {
            foreach (Control ctrl in gb_kartlogo_bilgi.Controls)
            {
                if (ctrl is Button)
                {
                    if(ctrl.Text!=butonText)
                    {
                        ctrl.Visible = false;
                    }
                    else
                    {
                        ctrl.Visible = true;
                    }
                    
                    
                }
            }
        }

        //Bu fonksiyon ile server'a data gonderiyorum. Overload edilmiş bir fonksiyon.
        #region SendRequesttoServer  
        //bu fonksiyon Server'a data gönderir.
        private void sendInfoToServer(string type, int kareID)
        {

            Information sendRequest = new Information();
            sendRequest.type = type;
            sendRequest.kareID = kareID;
            sendRequest.Zar = new zar();
            sendRequest.Zar.userServerID = serverID;
            string json = JsonConvert.SerializeObject(sendRequest);
            MonopolyGameClienth.SendData(UTF8Encoding.UTF8.GetBytes(json));

        }
        private void sendInfoToServer(string type, string Mesaj)
        {

            Information sendRequest = new Information();
            sendRequest.type = type;
            sendRequest.mesaj = Mesaj;
            string json = JsonConvert.SerializeObject(sendRequest);
            MonopolyGameClienth.SendData(UTF8Encoding.UTF8.GetBytes(json));

        }
        private void sendInfoToServer(string type, zar atilanZar)
        {

            Information sendRequest = new Information();
            sendRequest.type = type;
            sendRequest.Zar = atilanZar;
            string json = JsonConvert.SerializeObject(sendRequest);
            MonopolyGameClienth.SendData(UTF8Encoding.UTF8.GetBytes(json));

        }
        private void sendInfoToServer(string type, TapularinSahipleri tapuSahibi,string tapununAdı)
        {
            
            Information sendRequest = new Information();
            sendRequest.type = type;
            sendRequest.tempTapuSahibi = tapuSahibi;
            sendRequest.mesaj = tapununAdı;
            sendRequest.renk = rengiVer(BuOyuncu.renk);
            sendRequest.kareID = currentKare;
            string json = JsonConvert.SerializeObject(sendRequest);
            MonopolyGameClienth.SendData(UTF8Encoding.UTF8.GetBytes(json));

        }
        private void sendInfoToServer(string type, int kareID,int tutar)
        {
            Information sendRequest = new Information();
            sendRequest.type = type;
            if (type == "kiraOdendi")
            {
                TapuSenetleri tempTapuSenedi = new TapuSenetleri();
                tempTapuSenedi = kareninBilgisiniGetir(kareID);
                sendRequest.kareID = tempTapuSenedi.id;
                sendRequest.ucret = tutar;
                
            }
            else
            {
                sendRequest.kareID = kareID;
                sendRequest.ucret = tutar;
            }
            
            string json = JsonConvert.SerializeObject(sendRequest);
            MonopolyGameClienth.SendData(UTF8Encoding.UTF8.GetBytes(json));

        }
        #endregion
       
        //Server'dan data geldiğinde yapılacak işlemler.
        #region SERVER onDataReceived
        void MonopolyGameClienth_DataReceived(byte[] Data, string ID)
        {
            //haberleşmeyi Json ile sağlıyorum.
            //bunun için bir dll kullandım :  Newtonsoft.Json
            //karakter sorunu olmaması için UTF8 encodingi tercih ettim.
            string JsonComingString = UTF8Encoding.UTF8.GetString(Data);

            answer gelenObject = new answer();
              gelenObject =  JsonConvert.DeserializeObject<answer>(JsonComingString);
            
            switch (gelenObject.type)
            {
                
                case "oyuncu1":
                    Oyuncu1 = new KayitliOyunDetay();
                    Oyuncu1 = gelenObject.OyuncuDetay;
                    siraGuncelle(Oyuncu1.oyuncuID);
                    gb_oyuncu1.Visible = true;
                    lbl_oyuncu1_isim.Text = Oyuncu1.oyuncuID;
                    lbl_oyuncu1_renk.Text = Oyuncu1.renk;
                    pb_sira_piyon.Image = piyonListesi.Images[Oyuncu1.piyon];
                    pb_oyuncu1_piyon.Image = piyonListesi.Images[Oyuncu1.piyon];
                    statusYaziEkle(Oyuncu1.oyuncuID + " " + Oyuncu1.renk + " renk ile " + PiyonIsmiVer(Oyuncu1.piyon) + " piyonu ile oynuyor.");
                    if (Oyuncu1.oyuncuID == nick)
                    {
                        BuOyuncu = Oyuncu1;
                        piyon = piyon1;
                        serverID = "oyuncu1";
                        currentKare = piyon1CurrentKare;

                    }
                    
                        createPiyon(Oyuncu1.piyon, 1);

                        oyuncuSayisi++;

                    break;
                case "oyuncu2":
                    Oyuncu2 = new KayitliOyunDetay();
                    Oyuncu2 = gelenObject.OyuncuDetay;
                    lbl_oyuncu2_isim.Text = Oyuncu2.oyuncuID;
                    lbl_oyuncu2_renk.Text = Oyuncu2.renk;
                    gb_oyuncu2.Visible = true;
                    pb_oyuncu2_piyon.Image = piyonListesi.Images[Oyuncu2.piyon];
                    statusYaziEkle( Oyuncu2.oyuncuID + " " + Oyuncu2.renk + " renk ile " + PiyonIsmiVer(Oyuncu2.piyon) + " piyonu ile oynuyor." );
                    if (Oyuncu2.oyuncuID == nick)
                    {
                        BuOyuncu = Oyuncu2;
                        piyon = piyon2;
                        serverID = "oyuncu2";
                        currentKare = piyon2CurrentKare;
                    }
                        createPiyon(Oyuncu2.piyon,2);
                        oyuncuSayisi++;

                    break;
                case "oyuncu3":
                    Oyuncu3 = new KayitliOyunDetay();
                    Oyuncu3 = gelenObject.OyuncuDetay;
                    lbl_oyuncu3_isim.Text = Oyuncu3.oyuncuID;
                    lbl_oyuncu3_renk.Text = Oyuncu3.renk;
                    gb_oyuncu3.Visible = true;
                    pb_oyuncu3_piyon.Image = piyonListesi.Images[Oyuncu3.piyon];
                    statusYaziEkle( Oyuncu3.oyuncuID + " " + Oyuncu3.renk + " renk ile " + PiyonIsmiVer(Oyuncu3.piyon) + " piyonu ile oynuyor." );
                    if (Oyuncu3.oyuncuID == nick)
                    {
                        
                        BuOyuncu = Oyuncu3;
                        piyon = piyon3;
                        serverID = "oyuncu3";
                        currentKare = piyon3CurrentKare;


                    }
                    createPiyon(Oyuncu1.piyon, 3);
                    oyuncuSayisi++;

                    break;
                case "oyunaBasla":
                    BeklemeEkrani.Close();
                    lbl_para.Text = BuOyuncu.oyuncuPara.ToString();
                    TapuSahipleriListesi = gelenObject.TapularinSahipleriListesi;
                    break;
                case "zarAtildi":
                    PiyonuIlerlet(gelenObject.zarlar.zar1 + gelenObject.zarlar.zar2,gelenObject.zarlar.userServerID);
                    statusYaziEkle(gelenObject.oyuncuID + " zar attı: " + gelenObject.zarlar.zar1.ToString() + " - " + gelenObject.zarlar.zar2.ToString());
                    break;
                case "tapuSatinAlindi":
                    TapularinSahipleri tempTapuSahibiBilgisi = new TapularinSahipleri();
                    tempTapuSahibiBilgisi = gelenObject.tempTapuSahibi;
                    TapuSahipleriListesi.Add(tempTapuSahibiBilgisi);
                    statusYaziEkle(gelenObject.mesaj.Trim() + " " + gelenObject.tempTapuSahibi.oyuncu_id + " tarafından satın alındı.");
                    HoverLabel[gelenObject.kareID].BackColor = Color.FromArgb(121, gelenObject.oyuncu_rengi);
                    kareRenginiDegistir(gelenObject.kareID, gelenObject.oyuncu_rengi);
                    break;
                case "kiraOdendi":
                    TapularinSahipleri tempTapuSahibiBilgisi1 = new TapularinSahipleri();
                    tempTapuSahibiBilgisi1 = TapuSahipleriListesi.Where(search=> search.tapu_id == gelenObject.kareID).SingleOrDefault();
                    if (tempTapuSahibiBilgisi1.oyuncu_id == nick)
                    {
                        parayaEkle(gelenObject.ucret);
                        statusYaziEkle(gelenObject.oyuncuID + " sana " + gelenObject.ucret + "M kira odedi." + Environment.NewLine);
                        //txt_status.AppendText(gelenObject.oyuncuID + " sana " + gelenObject.ucret + "M kira odedi."+ Environment.NewLine);
                    }
                    else
                    {
                        statusYaziEkle(gelenObject.oyuncuID + " " + tempTapuSahibiBilgisi1.oyuncu_id + "'e  " + gelenObject.ucret + "M kira odedi." + Environment.NewLine);
                       // txt_status.AppendText(gelenObject.oyuncuID + " "  + tempTapuSahibiBilgisi1.oyuncu_id + "'e  " + gelenObject.ucret + "M kira odedi." + Environment.NewLine);

                    }
                    break;
                case "şans":
                    statusYaziEkle(gelenObject.oyuncuID + " şans kartı çekti: " + gelenObject.mesaj);
                    break;
                case "kamufonu":
                    statusYaziEkle(gelenObject.oyuncuID + " kamu fonu çekti: " + gelenObject.mesaj);
                    break;
                case "kodesegir":
                    kodes("kodesegir", gelenObject.zarlar.userServerID);
                    statusYaziEkle(gelenObject.oyuncuID + " kodese girdi!");
                    //PiyonuIlerlet(gelenObject.zarlar.userServerID, 10);
                    break;
                case "kodestencik":
                    kodes("kodestencik", gelenObject.zarlar.userServerID);
                    statusYaziEkle(gelenObject.oyuncuID + " kodesten cikti!");
                    //PiyonuIlerlet(gelenObject.zarlar.userServerID, 10);
                    break;
                case "biyeregit":
                    PiyonuIlerlet(gelenObject.zarlar.userServerID, gelenObject.kareID);
                    break;
                case "biyeregitkamu10":
                    PiyonuIlerlet(gelenObject.zarlar.userServerID, gelenObject.kareID);
                    break;
                case "sadecegit":
                    PiyonuIlerlet(gelenObject.zarlar.userServerID, gelenObject.kareID);
                    break;
                case "3haneilerigit":
                    PiyonuIlerlet(3,gelenObject.zarlar.userServerID);
                    break;
                case "heroyuncuya50ode":
                    if (gelenObject.zarlar.userServerID != serverID)
                    {
                        parayaEkle(50);
                    }
                    break;
                case "ipotekEdildi":
                    ipotekResmiGetir(gelenObject.kareID);
                    TapuSahipleriListesi.Where(search => search.tapu_id == gelenObject.ucret).Single().ipotekliMi = "1";
                    statusYaziEkle(gelenObject.oyuncuID + " bir arsasını ipotek ettirdi.");
                    if (gelenObject.oyuncuID == nick)
                    {
                        kartBilgisiniGoster(kareninBilgisiniGetir(gelenObject.kareID), gelenObject.kareID);
                    }
                    break;
                case "evKuruldu":
                    TapuSahipleriListesi.Where(search => search.tapu_id == gelenObject.ucret).Single().evSayisi += 1;
                    evOtelResmiGoster(gelenObject.kareID, TapuSahipleriListesi.Where(search => search.tapu_id == gelenObject.ucret).Single().evSayisi);
                    statusYaziEkle(gelenObject.oyuncuID + " bir ev aldı.");
                    if (gelenObject.oyuncuID == nick)
                    {
                        kartBilgisiniGoster(kareninBilgisiniGetir(gelenObject.kareID), gelenObject.kareID);
                    }
                    break;
                case "ipotekKaldirildi":
                    kareninResminiKaldir(gelenObject.kareID);
                    TapuSahipleriListesi.Where(search => search.tapu_id == gelenObject.ucret).Single().ipotekliMi = "0";
                    statusYaziEkle(gelenObject.oyuncuID + " bir arsasının ipoteğini kaldırttı.");

                    if (gelenObject.oyuncuID == nick)
                    {
                        kartBilgisiniGoster(kareninBilgisiniGetir(gelenObject.kareID), gelenObject.kareID);
                    }
                    
                    break;
                case "evIadeEdildi":
                    TapuSahipleriListesi.Where(search => search.tapu_id == gelenObject.ucret).Single().evSayisi -= 1;
                    evOtelResmiGoster(gelenObject.kareID, TapuSahipleriListesi.Where(search => search.tapu_id == gelenObject.ucret).Single().evSayisi);
                    statusYaziEkle(gelenObject.oyuncuID + " bir evini iade etti.");
                    if (gelenObject.oyuncuID == nick)
                    {
                        kartBilgisiniGoster(kareninBilgisiniGetir(gelenObject.kareID), gelenObject.kareID);
                    }
                    break;
                case "otelKuruldu":
                    TapuSahipleriListesi.Where(search => search.tapu_id == gelenObject.ucret).Single().evSayisi += 1;
                    evOtelResmiGoster(gelenObject.kareID, TapuSahipleriListesi.Where(search => search.tapu_id == gelenObject.ucret).Single().evSayisi);
                    statusYaziEkle(gelenObject.oyuncuID + " bir otelini iade etti.");
                    if (gelenObject.oyuncuID == nick)
                    {
                        kartBilgisiniGoster(kareninBilgisiniGetir(gelenObject.kareID), gelenObject.kareID);
                    }
                    break;
                case "otelIadeEdildi":
                    TapuSahipleriListesi.Where(search => search.tapu_id == gelenObject.ucret).Single().evSayisi -= 1;
                    evOtelResmiGoster(gelenObject.kareID, TapuSahipleriListesi.Where(search => search.tapu_id == gelenObject.ucret).Single().evSayisi);
                    statusYaziEkle(gelenObject.oyuncuID + " bir otelini iade etti.");
                    if (gelenObject.oyuncuID == nick)
                    {
                        kartBilgisiniGoster(kareninBilgisiniGetir(gelenObject.kareID), gelenObject.kareID);
                    }
                    break;
                case "siraSende":
                    siraGuncelle(gelenObject.oyuncuID);
                    break;
                case "oyundanCik":
                    oyundanCik(gelenObject.oyuncuID);
                    statusYaziEkle(gelenObject.oyuncuID + "oyundan çıktı.");
                    break;
                    
                    
            }
        }


        //Oyundan çıkıldığında yapılacak işlemler
        private void oyundanCik(string cikanID)
        {
            List<TapularinSahipleri> cikanOyuncununTapulari = new List<TapularinSahipleri>();
            foreach (TapularinSahipleri geciciTapu in TapuSahipleriListesi)
            {
                if (geciciTapu.oyuncu_id == cikanID)
                {
                    //geciciTapu.evSayisi = 0;
                    //geciciTapu.ipotekliMi = "0";
                    //geciciTapu.oyuncu_id = null;
                    //TapuSahipleriListesi.Remove(geciciTapu);
                    
                    int tempTapuid  = MonopolyDB.OyunAlani.Where(search => search.tapu_id == geciciTapu.tapu_id).Single().id;
                    HoverLabel[tempTapuid].BackColor = System.Drawing.Color.Transparent;
                    HoverLabel[tempTapuid].Image = null;
                    HoverLabel[tempTapuid].Tag = transparanRenk;

                    cikanOyuncununTapulari.Add(geciciTapu);                    
                }
            }

            foreach(TapularinSahipleri temp in cikanOyuncununTapulari)
            {
                TapuSahipleriListesi.Remove(temp);
            }
        }


        //server'a bilgi göndererek sirayi bir sonraki oyuncuya geciriyorum.s
        private void siraGuncelle(string oynayanID)
        {
            KayitliOyunDetay geciciOyuncuDetay = new KayitliOyunDetay();

            if(oynayanID == nick)
            {
                lbl_sira.Text = "Sıra SENDE";
                siraBuOyuncudaMi = true;
                btn_zarAt.Enabled = true;
                pb_sira_piyon.Image = piyonListesi.Images[BuOyuncu.piyon];
            }
            else
            {

               siraBuOyuncudaMi = false; ;
               if(oynayanID==Oyuncu1.oyuncuID)
               {
                   geciciOyuncuDetay = Oyuncu1;
                   pb_sira_piyon.Image = piyonListesi.Images[Oyuncu1.piyon];
                   

               }
               else if(Oyuncu2!=null)
               {
                   if (oynayanID == Oyuncu2.oyuncuID)
                   {
                       geciciOyuncuDetay = Oyuncu2;
                       pb_sira_piyon.Image = piyonListesi.Images[Oyuncu2.piyon];
                      

                   }
                   
               }
               else
               {

                   pb_sira_piyon.Image = piyonListesi.Images[Oyuncu2.piyon];
                   geciciOyuncuDetay = Oyuncu3;
               }
                lbl_sira.Text = geciciOyuncuDetay.oyuncuID;
                btn_zarAt.Enabled = false;
            }

        }

        //label rengini tekrar transparan yapıyorum.
        private void kareninResminiKaldir(int p)
        {
            HoverLabel[p].Image = null;
        }


        private void evOtelResmiGoster(int kareninID,int evSayisi)
        {
            switch (evSayisi)
            {
                case 0:
                    HoverLabel[kareninID].Image = null;
                    break;
                case 1:
                    if(kareninID<10)
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.ev1_alt;
                    }
                    else if(kareninID<20)
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.ev1_sol;
                    }
                    else if(kareninID<30)
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.ev1_ust;
                    }
                    else
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.ev1_sag;
                    }
                    break;
                case 2:
                    if (kareninID < 10)
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.ev2_alt;
                    }                                                                 
                    else if (kareninID < 20)                                          
                    {                                                                 
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.ev2_sol;
                    }                                                                 
                    else if (kareninID < 30)                                          
                    {                                                                 
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.ev2_ust;
                    }                                                                 
                    else                                                              
                    {                                                                 
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.ev2_sag;
                    }
                    break;
                case 3:
                    if (kareninID < 10)
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.ev3_alt;
                    }
                    else if (kareninID < 20)
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.ev3_sol;
                    }
                    else if (kareninID < 30)
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.ev3_ust;
                    }
                    else
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.ev3_sag;
                    }
                    break;
                case 4:
                    if (kareninID < 10)
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.ev4_alt;
                    }
                    else if (kareninID < 20)
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.ev4_sol;
                    }
                    else if (kareninID < 30)
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.ev4_ust;
                    }
                    else
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.ev4_sag;
                    }
                    break;
                case 5:
                    if (kareninID < 10)
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.otel_alt;
                    }
                    else if (kareninID < 20)
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.otel_sol;
                    }
                    else if (kareninID < 30)
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.otel_ust;
                    }
                    else
                    {
                        HoverLabel[kareninID].Image = Monopoly.Properties.Resources.otel_sag;
                    }
                    break;
            }
        }

        private void ipotekResmiGetir(int kareninIDsi)
        {
            if ((kareninIDsi > 0 && kareninIDsi < 10) || ( kareninIDsi>20 && kareninIDsi<30 ))
            {
                HoverLabel[kareninIDsi].Image = Monopoly.Properties.Resources.ipotekli_dikey;
            }
            else
            {
                HoverLabel[kareninIDsi].Image = Monopoly.Properties.Resources.ipotekli_yatay;
            }
        }
        #endregion

        //Oyuncuyu kodese gonderir
        private void kodes(string durum,string oyuncuServerID)
        {
            if (durum == "kodesegir")
            {
                if (oyuncuServerID==serverID)
                {
                    BuOyuncu.hapisahenedeMi = "1";
                }
                
                PiyonuIlerlet("kodesegir", oyuncuServerID);
            }
            else
            {
                if (oyuncuServerID == serverID)
                {
                    BuOyuncu.hapisahenedeMi = "0";
                }
                
                PiyonuIlerlet("kodestencik", oyuncuServerID);
            }
           
        }
        

        //Piyonun ilerlemesi için fonksiyonlar. Birden fazla ilerleme çeşidi var.
        //Bu yüzden overload ettim
        #region Piyonu İlerlet
        private void PiyonuIlerlet(int kacKare,string piyonName)
        {
            for (int a = 1; a <= kacKare; a++)
            {

                switch (piyonName)
                {
                    case "oyuncu1":
                        if (piyon1CurrentKare >= 39)
                        {
                            piyon1CurrentKare = -1;
                         }
                        piyon1.Location = new Point(X_point[piyon1CurrentKare + 1], Y_point[piyon1CurrentKare + 1]);
                        piyon1CurrentKare++;
                        PiyonUstUsteGelmeyiDenetle("oyuncu1");
                        break;
                    case "oyuncu2":
                        if(piyon2CurrentKare>=39)
                        {
                            piyon2CurrentKare = -1;
                        }
                        piyon2.Location = new Point(X_point[piyon2CurrentKare + 1], Y_point[piyon2CurrentKare + 1]);
                        piyon2CurrentKare++;
                        PiyonUstUsteGelmeyiDenetle("oyuncu2");
                        break;
                    case "oyuncu3":
                        if (piyon3CurrentKare >= 39)
                        {
                            piyon3CurrentKare = -1;
                        }
                        piyon3.Location = new Point(X_point[piyon3CurrentKare + 1], Y_point[piyon3CurrentKare + 1]);
                        piyon3CurrentKare++;
                        PiyonUstUsteGelmeyiDenetle("oyuncu2");
                        break;
                }
                
                
                Application.DoEvents();
                System.Threading.Thread.Sleep(500);
            }
            
            if (piyonName == serverID)
            {
                switch (piyonName)
                {
                    case "oyuncu1":
                        currentKare = piyon1CurrentKare;
                        sendInfoToServer("geldigiKart", currentKare);
                        showCardinfo(kareninBilgisiniGetir(piyon1CurrentKare));
                        tabpanel.SelectedTab = tab_aktif;
                        if (piyon1CurrentKare - kacKare < 0)
                        {
                            parayaEkle(200);
                            MessageBox.Show("Başlangıç noktasından geçerek 200M kazandınız.", "Başlangıç Noktası Geçiş");
                        }
                        
                        break;
                    case "oyuncu2":
                        currentKare = piyon2CurrentKare;
                        sendInfoToServer("geldigiKart", piyon2CurrentKare);
                        showCardinfo(kareninBilgisiniGetir(piyon2CurrentKare));
                        tabpanel.SelectedTab = tab_aktif;

                        if (piyon2CurrentKare - kacKare < 0)
                        {
                            parayaEkle(200);
                            MessageBox.Show("Başlangıç noktasından geçerek 200M kazandınız.", "Başlangıç Noktası Geçiş");
                        }
                        break;
                    case "oyuncu3":
                        currentKare = piyon3CurrentKare;
                        sendInfoToServer("geldigiKart", piyon3CurrentKare);
                        showCardinfo(kareninBilgisiniGetir(piyon3CurrentKare));
                        tabpanel.SelectedTab = tab_aktif;

                        if (piyon3CurrentKare - kacKare < 0)
                        {
                            parayaEkle(200);
                            MessageBox.Show("Başlangıç noktasından geçerek 200M kazandınız.", "Başlangıç Noktası Geçiş");
                        }
                        break;
                }
                
                
            }
            
        }

        private void PiyonuIlerlet(string kodesDurum,string oyuncuServerID)
        {
            PictureBox oyuncununPiyonu = buOyuncununPiyonunVer(oyuncuServerID);
            if (kodesDurum == "kodesegir")
            {
                //40. indiste kodesin yeri var.
                oyuncununPiyonu.Location = new Point(X_point[40], Y_point[40]);
                
            }
            else
            {
                oyuncununPiyonu.Location = new Point(X_point[10], Y_point[10]);
            }

            Application.DoEvents();
            System.Threading.Thread.Sleep(500);


            switch (oyuncuServerID)
            {
                case "oyuncu1":
                     piyon1CurrentKare =10 ;
                     PiyonUstUsteGelmeyiDenetle("oyuncu1");

                     break;
                case "oyuncu2":
                     piyon2CurrentKare = 10;
                     PiyonUstUsteGelmeyiDenetle("oyuncu2");

                     break;
                case "oyuncu3":
                     piyon3CurrentKare = 10;
                     PiyonUstUsteGelmeyiDenetle("oyuncu");

                     break;
            }
            if (oyuncuServerID == serverID)
            {
                currentKare = 10;
                showCardinfo(kareninBilgisiniGetir(currentKare));
                tabpanel.SelectedTab = tab_aktif;
                
            }
            
        }

        private void PiyonuIlerlet(string oyuncuServerID, int gidilecekKare)
        {
            PictureBox oyuncununPiyonu = buOyuncununPiyonunVer(oyuncuServerID);
            
            oyuncununPiyonu.Location = new Point(X_point[gidilecekKare], Y_point[gidilecekKare]);

            Application.DoEvents();
            System.Threading.Thread.Sleep(500);
            

            switch (oyuncuServerID)
            {
                case "oyuncu1":
                    piyon1CurrentKare = gidilecekKare;
                    PiyonUstUsteGelmeyiDenetle("oyuncu1");

                    break;
                case "oyuncu2":
                    piyon2CurrentKare = gidilecekKare;
                    PiyonUstUsteGelmeyiDenetle("oyuncu2");

                    break;
                case "oyuncu3":
                    piyon3CurrentKare = gidilecekKare;
                    PiyonUstUsteGelmeyiDenetle("oyuncu3");

                    break;
            }
            if (oyuncuServerID == serverID)
            {
                showCardinfo(kareninBilgisiniGetir(currentKare));
                currentKare = gidilecekKare;
                tabpanel.SelectedTab = tab_aktif;
            }

        }

        private void PiyonUstUsteGelmeyiDenetle(string piyonOyuncuID)
        {
            switch (piyonOyuncuID)
            {
                case "oyuncu1":
                    if (piyon1CurrentKare == piyon2CurrentKare || piyon1CurrentKare == piyon3CurrentKare)
                    {
                        piyon1.BringToFront();
                        if (piyon3 != null)
                        {
                            if (piyon1.Location == piyon2.Location && piyon2.Location == piyon3.Location)
                            {
                                piyon1.Location = new Point(piyon1.Location.X - 10, piyon1.Location.Y - 10);
                            }
                        }
                        else
                        {
                            piyon1.Location = new Point(piyon1.Location.X + 10, piyon1.Location.Y + 10);
                        }
                    }

                    break;
                case "oyuncu2":
                    if (piyon2CurrentKare == piyon1CurrentKare || piyon2CurrentKare == piyon3CurrentKare)
                    {
                        piyon2.BringToFront();
                        if (piyon3 != null)
                        {


                            if (piyon1.Location == piyon2.Location && piyon2.Location == piyon3.Location)
                            {
                                piyon2.Location = new Point(piyon2.Location.X - 10, piyon2.Location.Y - 10);
                            }
                        }
                        else
                        {
                            piyon2.Location = new Point(piyon2.Location.X + 10, piyon2.Location.Y + 10);
                        }
                    }

                    break;
                case "oyuncu3":
                    if (piyon3CurrentKare == piyon2CurrentKare || piyon3CurrentKare == piyon2CurrentKare)
                    {
                        piyon3.BringToFront();
                        if (piyon3 != null)
                        {
                            if (piyon1.Location == piyon2.Location && piyon2.Location == piyon3.Location)
                            {
                                piyon3.Location = new Point(piyon3.Location.X - 10, piyon3.Location.Y - 10);
                            }
                        }
                        else
                        {
                            piyon3.Location = new Point(piyon3.Location.X + 10, piyon3.Location.Y + 10);
                        }

                    }
                    break;
            }

        }
        private PictureBox buOyuncununPiyonunVer(string oyuncuServerID)
        {
            switch (oyuncuServerID)
            {
                case "oyuncu1":
                    return piyon1;
                case "oyuncu2":
                    return piyon2;
                case "oyuncu3":
                    return piyon3;
                default:
                    return null;

            }
        }
        #endregion



        #region Kart Rengini Belirleme
        private Color kartRengi(TapuSenetleri gelenTapuSenedi)
        {
            Color labelBg = new Color();

            switch (gelenTapuSenedi.renk)
            {
                    //brown,white,aqua,pink,orange,red,yellow,green,blue
                case "brown":
                    labelBg = Color.Brown;
                    break;
                case "white":
                case "white2":
                    labelBg = Color.White;
                    break;
                case "aqua":
                    labelBg = Color.Aqua;
                    break;
                case "pink":
                    labelBg = Color.Pink;
                    break;
                case "orange":
                    labelBg = Color.Orange;
                    break;
                case "red":
                    labelBg = Color.Red;
                    break;
                case "yellow":
                    labelBg = Color.Yellow;
                    break;
                case "green":
                    labelBg = Color.Green;
                    break;
                case "blue":
                    labelBg = Color.Blue;
                    break;
                default:
                    labelBg = Color.White;
                    break;
            }

            return labelBg;
        }
#endregion

       

      

        #region Label Hover,Click,Leave Olayları
        void fm_game_Click(object sender, EventArgs e)
        {
            Label gonderen = sender as Label;
            short kare_id = Convert.ToInt16(gonderen.Text);
            if(isClicked!=-1)
            {
                if(isClicked==kare_id)
                {
                    isClicked = -1;
                    tabpanel.SelectedTab = tab_aktif;
                }
                else
                {
                    HoverLabel[isClicked].BackColor = Color.Transparent;
                    isClicked = Convert.ToInt16(gonderen.Text);
                    //BURADA TIKLANAN KARENİN BİLGİSİ GÖSTERİLECEk
                    tabpanel.SelectedTab = tab_kartBilgi;
                    kartBilgisiniGoster(kareninBilgisiniGetir(kare_id), kare_id);
                    
                    

                }
            }
            else
            {
                isClicked = kare_id;
                gonderen.BackColor = Color.FromArgb(121, Color.Gray);
               //BURADA TIKLANAN KARENİN BİLGİSİ GÖSTERİLECEK
                tabpanel.SelectedTab = tab_kartBilgi;
                kartBilgisiniGoster(kareninBilgisiniGetir(kare_id), kare_id);
            }
        }


        void fm_game_MouseHover(object sender, EventArgs e)
        {
            Label gonderen = sender as Label;
            gonderen.BackColor = Color.FromArgb(121, Color.Gray); 
        }

        void kareRenginiDegistir(int KareID,Color kareRengi)
        {
            HoverLabel[KareID].BackColor = Color.FromArgb(60, kareRengi);
            HoverLabel[KareID].Tag = Color.FromArgb(60, kareRengi);
        }

        void fm_game_MouseLeave(object sender, EventArgs e)
        {
            Label gonderen = sender as Label;
            Color renk = (Color) gonderen.Tag;
            if (isClicked != Convert.ToInt32(gonderen.Text))
            {
                gonderen.BackColor = renk;
            }
            
        }

        void MouseLeaveOyuncuRengiYap(object sender, EventArgs e)
        {
            Label gonderen = sender as Label;
            if (isClicked != Convert.ToInt32(gonderen.Text))
            {
                gonderen.BackColor = Color.FromArgb(60, OyuncuRengi);
            }

        }
        
        private void sansKartiCek(object sender, EventArgs e)
        {
            Button tiklananButon = sender as Button;
            tiklananButon.Visible = false;
            kartCek("şans");
            
        }

        private int RastgeleSayi()
        {
            Random rastgeleSayi = new Random();
            return rastgeleSayi.Next(0, 16);
        }

        private void kartCek(string kartTuru)
        {
            btn_devam.BringToFront();
            string kartMetni;
            string bayMonopolyID;
            int randomSayi = RastgeleSayi();
            int tutar;
            if (kartTuru == "şans")
            {
                kartMetni = SansKartlariListesi.ElementAt(randomSayi).metin;
                bayMonopolyID = SansKartlariListesi.ElementAt(randomSayi).mr_monopoly_id;
                sendInfoToServer("şans",kartMetni);
            }
            else
            {
                kartMetni = KamuFonuKartlariListesi.ElementAt(randomSayi).metin;
                bayMonopolyID = KamuFonuKartlariListesi.ElementAt(randomSayi).mr_monopoly_id;
                sendInfoToServer("kamufonu", kartMetni);
            }

            fm_kartCek kartForm = new fm_kartCek(kartTuru,kartMetni,bayMonopolyID);
            kartForm.Owner = this;
            kartForm.StartPosition = FormStartPosition.CenterScreen;
            kartForm.ShowDialog();
            kartForm.Dispose();
            kartForm = null;
            
            if(kartTuru=="şans")
            {
               
                switch(randomSayi)
                {

                    case 0:
                        tutar = (int) SansKartlariListesi.ElementAt(randomSayi).tutar;
                        parayaEkle(tutar);
                        break;
                    case 1:
                        //baslangic noktasına ilerle. 200M al;
                        sendInfoToServer("sadecegit",0);
                        parayaEkle(200);
                        btn_devam.BringToFront();
                        btn_devam.Visible = true;
                        //sendInfoToServer("sirayiIlerlet", BuOyuncu.ciftZarAtmaSayisi);
                        break;
                    case 2:
                        //doğru kodese gir
                        //kodes("kodesegir",serverID);
                        sendInfoToServer("kodesegir", 10);
                        //sendInfoToServer("sirayiIlerlet", BuOyuncu.ciftZarAtmaSayisi);
                        break;
                    case 3:
                        //en yakın kamu kuruluşuna git.eğer sahibi yoksa alabilirsin,
                        //Eğer SAHİBİ VARSA,  sahibine attığın zarın 10 katını öde.
                        sendInfoToServer("biyeregitkamu10", enYakinKamuKurulusuKareID(currentKare));
                        kamuFonu10kat = true;
                        break;
                    case 4:
                        //EN YAKIN KAMU KURUŞULUNA GİT. 
                        //Eğer SAHİBİ YOKSA, bankadan satın alabilirsin. 
                        //Eğer SAHİBİ VARSA,  sahibine attığın zarın 10 katını öde.
                        sendInfoToServer("biyeregitkamu10", enYakinKamuKurulusuKareID(currentKare));
                        kamuFonu10kat = true;
                        break;
                    case 5:
                        //HAYDARPAŞA TREN İSTASYONU'NA GİT. 
                        //BAŞLANGIÇ NOKTASINDAN GEÇERSEN 200M AL.
                        sendInfoToServer("sadecegit", 5);
                        if (currentKare>=11)
                        {
                            parayaEkle(200);
                        }
                        break;
                    case 6:
                        //ÜÇ HANE ileri GİT.
                        sendInfoToServer("3haneilerigit", 3);
                        break;
                    case 7:
                        //YÖNETİM KURULU BAŞKANI SEÇİLDİN. HER OYUNCUYA 50M ÖDE.
                        sendInfoToServer("heroyuncuya50ode", 0);
                        if(!paradanDus(50 * (oyuncuSayisi-1)))
                        {
                            MessageBox.Show("Diğer oyunculara ödeme yapabilmek için yeterli paran yok!","Para Yetersiz!");
                        }
                        btn_devam.Visible = true;
                        btn_devam.BringToFront();
                    //sendInfoToServer("sirayiIlerlet", BuOyuncu.ciftZarAtmaSayisi);
                        break;
                    case 8:
                        //BEYOĞLU'NA İLERLE. BAŞLANGIÇ NOKTASINDAN GEÇERSEN 200M AL.
                        sendInfoToServer("sadecegit", 11);
                        if (currentKare>=11)
                        {
                            parayaEkle(200);
                        }
                        break;
                    case 9:
                        //BÜTÜN BİNALARINA GENEL ONARIM YAP. HER EV İÇİN 40M, HER OTEL İÇİN 115M ÖDE.
                        btn_kart_odeme.Visible = true;
                        btn_kart_odeme.BringToFront();
                        kamuOdemesi = CaddeOnarimiHesapla();
                        //kartCekOdemeYap(CaddeOnarimiHesapla());
                        break;
                    case 10:
                        //hisarustune'E İLERLE.
                        sendInfoToServer("sadecegit", 39);
                        break;
                    case 11:
                        //BANKA 50M KAR PAYI ÖDÜYOR.
                        tutar = (int)SansKartlariListesi.ElementAt(randomSayi).tutar;
                        parayaEkle(tutar);
                        btn_devam.BringToFront();
                        btn_devam.Visible = true;    
                    //sendInfoToServer("sirayiIlerlet", BuOyuncu.ciftZarAtmaSayisi);
                        break;
                    case 12:
                        //KODESTEN ÜCRETSİZ ÇIK. Bu kart satılabilir 
                        //veya gerekli olana kadar saklanabilir.
                        BuOyuncu.kodestenCikKartiSayisi++;
                        btn_devam.BringToFront();
                        btn_devam.Visible = true;
                        //sendInfoToServer("sirayiIlerlet", BuOyuncu.ciftZarAtmaSayisi);
                        break;
                    case 13:
                        //AŞIRI HIZ CEZASI. 15M CEZA ÖDE.
                        btn_kart_odeme.Visible = true;
                        btn_kart_odeme.BringToFront();
                        kamuOdemesi = 15;    
                    //tutar = (int)SansKartlariListesi.ElementAt(randomSayi).tutar;
                        //kartCekOdemeYap(tutar);
                        //parayaEkle(tutar);
                        //BU OLMAZZ BU BİR ÖDEME OLMALI. ÖDEYEMEZSE SIKINTI OLMALIII
                        //sendInfoToServer("sirayiIlerlet", BuOyuncu.ciftZarAtmaSayisi);
                        break;
                    case 14:
                        //CADDEBOSTAN'A İLERLE. 
                        //BAŞLANGIÇ NOKTASINDAN GEÇERSEN 200M AL.
                        sendInfoToServer("sadecegit", 24);
                        if (currentKare>=24)
                        {
                            parayaEkle(200);
                        }
                        break;
                    case 15:
                        //EN YAKIN KAMU KURUŞULUNA GİT. Eğer SAHİBİ YOKSA, bankadan satın alabilirsin.
                        //Eğer SAHİBİ VARSA, zar at ve sahibine attığın zarın 10 katını öde.
                        sendInfoToServer("biyeregitkamu10", enYakinKamuKurulusuKareID(currentKare));
                        kamuFonu10kat = true;

                        break;
                }
            }
            else{

                switch(randomSayi)
                {
                    case 7:
                        //DOKTOR VİZİTE ÜCRETİ. 50M ÖDE.
                        //kartCekOdemeYap(50);
                        btn_kart_odeme.Visible = true;
                        btn_kart_odeme.BringToFront();
                        kamuOdemesi = 50;
                        break;
                    case 8:
                        //HASTANEYE 100M ÖDE.
                        btn_kart_odeme.Visible = true;
                        btn_kart_odeme.BringToFront();
                        kamuOdemesi = 100;
                        //kartCekOdemeYap(100);
                        break;
                    case 3:
                        //başlangıç noktasına ilerle, 200M al.
                        sendInfoToServer("sadecegit", 0);
                        parayaEkle(200);
                        btn_devam.BringToFront();
                        btn_devam.Visible = true;
                        //sendInfoToServer("sirayiIlerlet", BuOyuncu.ciftZarAtmaSayisi);
                        break;
                    case 12:
                        //cadde onarımlar: her ev için 40M, her otel için 115M öde
                        btn_kart_odeme.Visible = true;
                        btn_kart_odeme.BringToFront();
                        //btn_kart_odeme.Tag =  kartCekOdemeYap(CaddeOnarimiHesapla());
                        kamuOdemesi = CaddeOnarimiHesapla();
                        //sendInfoToServer("caddeOnarimlari", 0);
                        break;
                    case 15:
                        //kodesten ücretsiz çık kartı;
                        BuOyuncu.kodestenCikKartiSayisi++;
                        btn_devam.BringToFront();
                        btn_devam.Visible = true;
                        break;
                    case 14:
                        //doğru kodese kartı;
                        sendInfoToServer("kodesegir", 10);
                        btn_devam.BringToFront();
                        btn_devam.Visible = true;
                        break;
                    default:
                        tutar = (int)KamuFonuKartlariListesi.ElementAt(randomSayi).tutar;
                        parayaEkle(tutar);
                        btn_devam.BringToFront();
                        btn_devam.Visible = true;
                        break;
                } 
            }

        }

        private void kartCekOdemeYap()
        {
            
            MessageBox.Show(kamuOdemesi + "M ödeyeceksiniz.", "Ödeme Bilgisi");
            if (paradanDus(kamuOdemesi))
            {
                
                btn_kart_odeme.Visible = false;
                btn_devam.BringToFront();
                btn_devam.Visible = true;
                

                //sendInfoToServer("sirayiIlerlet", BuOyuncu.ciftZarAtmaSayisi);
            }
            else
            {
                btn_devam.Visible = false;
                MessageBox.Show("Ödeme için yeterli paranız yok.", "Para Yetersiz!");
            }
        }

        private int CaddeOnarimiHesapla()
        {
            int evSayisi=0;
            int otelSayisi=0;

            foreach(var tapu in TapuSahipleriListesi)
            {
                if(tapu.oyuncu_id == nick && tapu.evSayisi>0)
                {
                    if(tapu.evSayisi<5)
                    {
                        evSayisi += tapu.evSayisi;
                    }
                    else if(tapu.evSayisi==5)
                    {
                        otelSayisi++;
                    }
                }
            }

            return (evSayisi * 40) + (otelSayisi * 115);
        }

        private int enYakinKamuKurulusuKareID(int suanKiKare)
        {
            if (suanKiKare >= 28 || suanKiKare <= 11)
            {
                //yani tedaş elektrik
                return 12;
            }
            else
            {
                //yani iski su
                return 35;
            }
        }

        private void kamuFonuKartiCek(object sender, EventArgs e)
        {
            Button tiklananButon = sender as Button;
            tiklananButon.Visible = false;
            kartCek("kamufonu");
        }

        
#endregion

       
   

        
        //SATIN AL click eventi
        private void btn_satinal_evet_Click(object sender, EventArgs e)
        {
            int tapuFiyati = Convert.ToInt32(lbl_satinal_fiyat.Text);
            if(paradanDus(tapuFiyati))
            {
                tapuyuSatinAl(currentKare);
                btn_devam.Visible = true;
            }
            else
            {
                MessageBox.Show("Burayı satın almak için yeterli paranız yok!","Para Yetersiz!");
            }
            
        }

        //tapuyu satın alır
        private void tapuyuSatinAl(int bulunduguKare)
        {
            TapuSenetleri tempTapu = new TapuSenetleri();
            tempTapu = kareninBilgisiniGetir(bulunduguKare);
            TapularinSahipleri tempTapuSahibiBilgisi = new TapularinSahipleri();
            tempTapuSahibiBilgisi.tapu_id = tempTapu.id;
            tempTapuSahibiBilgisi.oyuncu_id = nick;
            sendInfoToServer("tapuSatinAlindi", tempTapuSahibiBilgisi,tempTapu.tapu_ismi);
            groupBoxlariGizle();
            gb_tapuinfo.Visible = true;
            gb_tapusenin.Visible = true;
            btn_devam.Visible = true;
            MessageBox.Show(tempTapu.tapu_ismi +" artık senin!","Tebrikler!");
        }

        
        private void groupBoxlariGizle()
        {
            gb_satin_al.Visible = false;
            gb_tapusenin.Visible = false;
            gb_kiraode.Visible = false;
            gb_kartlogo_bilgi.Visible = false;            
        }

        //şans kartı çek Click Eventi
        private void btn_sans_Click(object sender, EventArgs e)
        {
            kartCek("şans");
        }

        //kamu fonu çek Click Eventi
        private void btn_kamufonu_Click(object sender, EventArgs e)
        {
            kartCek("kamufonu");

        }
        
        //vergi öde çek Click Eventi
        private void vergiOde(object sender, EventArgs e)
        {
            int vergiTutari;
            if (currentKare < 30)
            {
                vergiTutari = (int)OyunTahtasi.ElementAt(4).TapuSenetleri.fiyati;
            }
            else
            {
                vergiTutari = (int)OyunTahtasi.ElementAt(38).TapuSenetleri.fiyati;
            }
            
            if(paradanDus(vergiTutari))
            {
                Button tiklananButon = sender as Button;
                tiklananButon.Visible = false;
                MessageBox.Show("Vergi için " + vergiTutari + "M ödedin.", "Vergi Bilgi");
                btn_devam.Visible = true;
                //sendInfoToServer("sirayiIlerlet", BuOyuncu.ciftZarAtmaSayisi);
            }
            else
            {
                MessageBox.Show("Vergiyi ödeyebilmek için yeterli paran yok!","Para Yetersiz");
            }
            
        }
        
        //gelen para metreyi paraya ekler/duser. 
        private bool paradanDus(int harcananPara)
        {
            if (BuOyuncu.oyuncuPara-harcananPara>=0)
            {
                BuOyuncu.oyuncuPara -= harcananPara;
                lbl_para.Text = BuOyuncu.oyuncuPara.ToString();
                return true;
            }
            else
            {
                //PARA YETERLİ DEĞİL
                return false;
            }
        }
        private void parayaEkle(int gelenPara)
        {
            BuOyuncu.oyuncuPara += gelenPara;
            lbl_para.Text = BuOyuncu.oyuncuPara.ToString();
        }


        //Zarı atar.
        private void button4_zarat_Click(object sender, EventArgs e)
        {
            zar yeniZar = new zar();


            
            yeniZar.userServerID = serverID;
            frm_zar zarAnimasyon = new frm_zar(yeniZar);
            zarAnimasyon.ShowDialog();
            zarAnimasyon.Dispose();
            zarAnimasyon = null;
            sendInfoToServer("zarAtildi", yeniZar);
  
            if (yeniZar.zar1 == yeniZar.zar2)
            {
                BuOyuncu.ciftZarAtmaSayisi++;
                if (BuOyuncu.ciftZarAtmaSayisi < 3)
                {
                    MessageBox.Show("Tebrikler, çift zar atarak bir kez daha oynama hakkı kazandınız!", "Çift Zar Attınız!", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("3 kez üst üste çift zar attınız! Doğru kodese!", "Doğru Kodese!", MessageBoxButtons.OK);
                    BuOyuncu.ciftZarAtmaSayisi = 0;
                    sendInfoToServer("kodesegir", 10);
                }
            }
            else
            {
                BuOyuncu.ciftZarAtmaSayisi = 0;
            }
            zarlarinToplami = yeniZar.zar1 + yeniZar.zar2;
        }
        public void  zarAt()
        {
            zar yeniZar = new zar();
            Random rastgeleSayi = new Random();
            
            yeniZar.zar1 = rastgeleSayi.Next(1, 7);
            yeniZar.zar2 = rastgeleSayi.Next(1, 7);
            yeniZar.userServerID = serverID;
            frm_zar zarAnimasyon = new frm_zar(yeniZar);
            zarAnimasyon.ShowDialog();
            zarAnimasyon.Dispose();
            zarAnimasyon = null;
            sendInfoToServer("zarAtildi", yeniZar);
            if (yeniZar.zar1 == yeniZar.zar2)
            {
                BuOyuncu.ciftZarAtmaSayisi++;
                if (BuOyuncu.ciftZarAtmaSayisi < 3) 
                {
                    MessageBox.Show("Tebrikler, çift zar atarak bir kez daha oynama hakkı kazandınız!", "Çift Zar Attınız!", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("3 kez üst üste çift zar attınız! Doğru kodese!", "Doğru Kodese!", MessageBoxButtons.OK);
                    BuOyuncu.ciftZarAtmaSayisi = 0;
                    sendInfoToServer("kodesegir", 10);
                    btn_devam.Visible = true;
                    //sendInfoToServer("sirayiIlerlet",BuOyuncu.ciftZarAtmaSayisi);
                }
            }
            else
            {
                BuOyuncu.ciftZarAtmaSayisi = 0;
            }

            zarlarinToplami = yeniZar.zar1 + yeniZar.zar2;
              
        }

        //Kirayi ÖDE click
        private void btn_kirayiode_Click(object sender, EventArgs e)
        {
            //Burada kira odeniyor.
            Button tiklananButon = sender as Button;
            tiklananButon.Visible = false;
            int ucret =Convert.ToInt32(lbl_gbkiraode_kira.Text);
            if (paradanDus(ucret))
            {
                //MessageBox.Show("kira ödendi");
                sendInfoToServer("kiraOdendi", currentKare,ucret);
                btn_devam.Visible = true;
                //sendInfoToServer("sirayiIlerlet", BuOyuncu.ciftZarAtmaSayisi);

            }
            else
            {
                MessageBox.Show("Kirayı ödeyebilmek için yeterli paran yok.","Para Yetersiz");
            }
           
        }

        //Status'e yazı ekler.
        private void statusYaziEkle(string metin)
        {
            txt_status.AppendText(Environment.NewLine + metin + Environment.NewLine);
            txt_status.ScrollToCaret();
        }

        //Kodese Gir Click
        private void btn_kartlogo_kodesgir_Click(object sender, EventArgs e)
        {
            //kodes("kodesegir",serverID);
            sendInfoToServer("kodesegir", 10);
            //sendInfoToServer("sirayiIlerlet", BuOyuncu.ciftZarAtmaSayisi);
        }
     
        private string PiyonIsmiVer(int piyon_id)
        {

            return MonopolyDB.Piyonlar.ToList().Where(search => search.id == piyon_id ).Single().isim;
        }
        //string renk ismini Color olarak return eder.
        private Color rengiVer(string renkIsmi)
        {
            Color renkClr;
            switch (renkIsmi)
            {
                case "kırmızı":
                    renkClr = Color.Red;
                    break;
                case "yeşil":
                    renkClr = Color.Green;
                    break;
                case "mavi":
                    renkClr = Color.Blue;
                    break;
                default:
                    renkClr = Color.Yellow;
                    break;
            }
            return renkClr;
        }

        private void btn_tapusenin_zarat_Click(object sender, EventArgs e)
        {
            zarAt();
        }
        //ipotek ettirme
        private void btn_ipotek_ettir_Click(object sender, EventArgs e)
        {
            Button tiklanan = sender as Button;
            DialogResult mesajYanit = new DialogResult();
            mesajYanit = MessageBox.Show("Arsayı ipotek etmek istediğinize emin misiniz?", "Arsa İpotek Onay", MessageBoxButtons.YesNo);
           // tiklanan.Enabled = false;
            if (mesajYanit == DialogResult.Yes)
            {
                TapuSenetleri tempTapu = new TapuSenetleri();
                tempTapu = (TapuSenetleri)tiklanan.Tag;
                parayaEkle((int)tempTapu.fiyati / 2);
                sendInfoToServer("ipotekEdildi", tiklanilanKart, tempTapu.id);

                //btn_ev_kur.Enabled = false;
                //btn_otel_kur.Enabled = false;
                //btn_acik_arttirma.Enabled = false;
                //btn_ipotek_kaldir.Visible = true;
                //btn_ipotek_kaldir.Enabled = true;
                //btn_ipotek_kaldir.BringToFront();
                
            }
        }
        //ev kurma
        private void btn_ev_kur_Click(object sender, EventArgs e)
        {
            Button tiklanan = sender as Button;
            DialogResult mesajYanit = new DialogResult();
            mesajYanit = MessageBox.Show("Ev kurmak istediğinize emin misiniz?", "Ev Kurma Onay", MessageBoxButtons.YesNo);
            //tiklanan.Enabled = false;
            if (mesajYanit == DialogResult.Yes)
            {
                TapuSenetleri tempTapu = new TapuSenetleri();
                tempTapu = (TapuSenetleri)tiklanan.Tag;
                if(paradanDus((int)tempTapu.ev_maliyeti))
                {
                    sendInfoToServer("evKuruldu", tiklanilanKart, tempTapu.id);
                
                }
                else
                {
                    MessageBox.Show("Ev kurmak için yeterli paranız yok.", "Yetersiz Para");
                }
                
            }
        }
        //ipotek kaldır
        private void btn_ipotek_kaldir_Click(object sender, EventArgs e)
        {
            Button tiklanan = sender as Button;
            DialogResult mesajYanit = new DialogResult();
            TapuSenetleri tempTapu = new TapuSenetleri();
            tempTapu = (TapuSenetleri)tiklanan.Tag;
            //tiklanan.Enabled = false;
            
            mesajYanit = MessageBox.Show("Arsanın ipoteğini " + tempTapu.fiyati/2 + "M karşılığında kaldırmak istediğinize emin misiniz?", "Arsa İpotek Kaldırma Onay", MessageBoxButtons.YesNo);
            if (mesajYanit == DialogResult.Yes)
            {
                
                if (paradanDus((int)tempTapu.fiyati / 2))
                {
                    sendInfoToServer("ipotekKaldirildi", tiklanilanKart, tempTapu.id);                    
                }
                else
                {
                    MessageBox.Show("İpoteği kaldırmak için yeterli paranız yok.", "Para Yetersiz");
                }
                
                
            }

        }
        //ev sayisini azaltır
        private void btn_ev_iade_Click(object sender, EventArgs e)
        {
            Button tiklanan = sender as Button;
            DialogResult mesajYanit = new DialogResult();
            TapuSenetleri tempTapu = new TapuSenetleri();
            tempTapu = (TapuSenetleri)tiklanan.Tag;

            mesajYanit = MessageBox.Show("1 adet evi yarı fiyatı karşılığında iade etmek istediğine emin misin?", "Ev İade Onay", MessageBoxButtons.YesNo);
            if (mesajYanit == DialogResult.Yes)
            {
                parayaEkle((int)tempTapu.ev_maliyeti/2);
                sendInfoToServer("evIadeEdildi", tiklanilanKart, tempTapu.id);
                

            }
        }
        //otel kurar
        private void btn_otel_kur_Click(object sender, EventArgs e)
        {
            Button tiklanan = sender as Button;
            DialogResult mesajYanit = new DialogResult();
            TapuSenetleri tempTapu = new TapuSenetleri();
            tempTapu = (TapuSenetleri)tiklanan.Tag;

            mesajYanit = MessageBox.Show("Otel kurmak istediğinize emin misin?", "Otel Kurma Onay", MessageBoxButtons.YesNo);
            
            if (mesajYanit == DialogResult.Yes)
            {
          
               

                if (paradanDus((int)tempTapu.ev_maliyeti))
                {
                    sendInfoToServer("otelKuruldu", tiklanilanKart, tempTapu.id);
                }
                else
                {
                    MessageBox.Show("Otel kurabilmek için yeterli paranız yok.", "Para Yetersiz");
                }
                
            }
        }
        //otel iade
        private void btn_otel_iade_Click(object sender, EventArgs e)
        {
            Button tiklanan = sender as Button;
            DialogResult mesajYanit = new DialogResult();
            TapuSenetleri tempTapu = new TapuSenetleri();
            tempTapu = (TapuSenetleri)tiklanan.Tag;

            mesajYanit = MessageBox.Show("Oteli iade etmek istediğine emin misin?", "Otel İade Onay", MessageBoxButtons.YesNo);
            if (mesajYanit == DialogResult.Yes)
            {
                parayaEkle((int)tempTapu.ev_maliyeti / 2);
                sendInfoToServer("otelIadeEdildi", tiklanilanKart, tempTapu.id);


            }
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            zarAt();
        }

        private void btn_zarAt_Click(object sender, EventArgs e)
        {
            oyunBaslangici = false;
            btn_zarAt.Enabled = false;
            zarAt();
        }

        private void btn_kodes_bekle_Click(object sender, EventArgs e)
        {
            
            BuOyuncu.hapishaneTurSayisi++;
            if (BuOyuncu.hapishaneTurSayisi == 3)
            {
                sendInfoToServer("kodestencik", 10);
            }
            sendInfoToServer("sirayiIlerlet", BuOyuncu.ciftZarAtmaSayisi);
            
        }

        private void btn_kodes_ucretli_cikis_Click(object sender, EventArgs e)
        {
            DialogResult mesajYanit = new DialogResult();
            mesajYanit = MessageBox.Show("50M karşılığında kodesten çıkmak istediğinize emin misiniz?", "Kodes Çıkış Onay", MessageBoxButtons.YesNo);
            if (mesajYanit == DialogResult.Yes)
            {
                if (paradanDus(50))
                {
                    sendInfoToServer("kodestencik", 10);
                    //MessageBox.Show("Bir sonraki el oynayabileceksiniz.", "Kodes Çıkış");
                    //sendInfoToServer("sirayiIlerlet", BuOyuncu.ciftZarAtmaSayisi);

                }
                {
                    MessageBox.Show("Ödeme için yeterli paranız bulunmamakta.", "Para Yetersiz");
                }
                


            }
        }

        private void btn_kodeskarti_kullan_Click(object sender, EventArgs e)
        {
            DialogResult mesajYanit = new DialogResult();
            mesajYanit = MessageBox.Show("'Kodesten Ücretsiz Çık' kartınızı kullanarak, kodesten çıkmak istediğinize emin misiniz?", "Kodes Çıkış Onay", MessageBoxButtons.YesNo);
            if (mesajYanit == DialogResult.Yes)
            {
                    sendInfoToServer("kodestencik", 10);
                    //MessageBox.Show("Bir sonraki el oynayabileceksiniz.", "Kodes Çıkış");
                    //sendInfoToServer("sirayiIlerlet", BuOyuncu.ciftZarAtmaSayisi);

            }
        }

        private void btn_kira_devam_Click(object sender, EventArgs e)
        {
            btn_kira_devam.Visible=false;
            sendInfoToServer("sirayiIlerlet", BuOyuncu.ciftZarAtmaSayisi);
        }

       

        private void btn_kart_odeme_Click(object sender, EventArgs e)
        {
            kartCekOdemeYap();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult cevap = new DialogResult();

            cevap = MessageBox.Show("Oyundan çıkmak istediğinize emin misiniz?", "Oyundan Çıkma Onay", MessageBoxButtons.YesNo);

            if(cevap==DialogResult.Yes)
            {
                sendInfoToServer("oyundanCik", 0);
                this.Close();
                Application.Exit();
            }
        }

       

        private void btn_devam_Click(object sender, EventArgs e)
        {
            sendInfoToServer("sirayiIlerlet", BuOyuncu.ciftZarAtmaSayisi);
            btn_devam.Visible = false;

        }

        private void btn_satinal_hayir_Click(object sender, EventArgs e)
        {
            DialogResult cevap = new DialogResult();

            cevap = MessageBox.Show("Satın almak istemediğinize emin misiniz?", "Satın Alma İptal Onay", MessageBoxButtons.YesNo);

            if (cevap == DialogResult.Yes)
            {
                btn_devam.Visible = true;
                gb_satin_al.Visible = false;
            }
            
        }

       

        

        

        

        

        

      
        

    }
}
