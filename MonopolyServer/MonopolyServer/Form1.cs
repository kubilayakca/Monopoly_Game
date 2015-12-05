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
using System.Diagnostics;

namespace MonopolyServer
{
    public partial class Form1 : Form
    {
        #region Global Variables
        Host MonopolyGameServer;
        MonopolyEntities10 MonopolyDB = new MonopolyEntities10();
        List<TapuSenetleri> TapuSenetleriListesi = new List<TapuSenetleri>();
        List<OyunAlani> OyunTahtasi = new List<OyunAlani>();
        List<KartTipleri> kartTurleri = new List<KartTipleri>();
        List<OyunKurallari> OyunKurallari = new List<OyunKurallari>();
        List<TapularinSahipleri> TapuSahipleriListesi = new List<TapularinSahipleri>();
        List<KayitliOyunDetay> OyuncularListesi = new List<KayitliOyunDetay>();
        KayitliOyunDetay Oyuncu1;
        KayitliOyunDetay Oyuncu2;
        KayitliOyunDetay Oyuncu3;
        int gecenZaman;
        int siraOyuncuIDSayac=0;
        int oyunBitisZamani = 0;
        //TapularinSahipleri tempYeniTapuSahibi;
        KayitliOyunDetay TekOyuncuDetay = new KayitliOyunDetay
        {
            ciftZarAtmaSayisi = 0,
            hapisahenedeMi = "0",
            hapishaneTurSayisi = 0,
            oyuncuID = "",
            oyuncuPara = 1200,
            oyundakiKareYeri = 0,
            oyundanCekilme = "0" 
            
        };
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        #region Form onLoad
        private void Form1_Load(object sender, EventArgs e)
        {
            //server ile socket bağlantısı
            MonopolyGameServer = new Host(6745);
            MonopolyGameServer.SendBufferSize = 3000;
            MonopolyGameServer.ReceiveBufferSize = 3000;
            MonopolyGameServer.NoDelay = true;
            MonopolyGameServer.StartConnection();
            //socket event'leri
            MonopolyGameServer.onConnection += MonopolyGameServer_onConnection;
            MonopolyGameServer.DataReceived +=MonopolyGameServer_DataReceived;
            MonopolyGameServer.ConnectionClosed += MonopolyGameServer_ConnectionClosed;
           

            TapuSenetleriListesi = MonopolyDB.TapuSenetleri.ToList();
            OyunTahtasi = MonopolyDB.OyunAlani.ToList();
            kartTurleri = MonopolyDB.KartTipleri.ToList();
            OyunKurallari = MonopolyDB.OyunKurallari.ToList();
          

        }
        #endregion

        #region SERVER onConnectionClosed
        void MonopolyGameServer_ConnectionClosed()
        {
            txt_status.AppendText("Connection Closed" + Environment.NewLine);
        }
        #endregion

        #region SERVER onDataReceived
        private void MonopolyGameServer_DataReceived(string ID, byte[] Data)
        {


            string comingJsonString = (UTF8Encoding.UTF8.GetString(Data));
            Information comingRequest = JsonConvert.DeserializeObject<Information>(comingJsonString);
            statusYaziEkle("Gelen Bilgi:" + ID + " oyuncusundan şu istek: " + comingRequest.type);
            //txt_status.AppendText("Server Gelen Bilgi :" + comingRequest.type + " Kare ID = " + comingRequest.kareID + Environment.NewLine);

            answer sendAnswer = new answer();
            switch(comingRequest.type)
            {
                case "zarAtildi":
                  
                    sendAnswer.zarlar = comingRequest.Zar;
                    sendAnswer.oyuncuID = ID;
                    statusYaziEkle(ID + " zar attı= " + comingRequest.Zar.zar1.ToString() + "-" + comingRequest.Zar.zar2.ToString());
                    break;
                case "tapuSatinAlindi":
                    
                    TapularinSahipleri tempYeniTapuSahibi = new TapularinSahipleri();
                    tempYeniTapuSahibi = comingRequest.tempTapuSahibi;
                    TapuSahipleriListesi.Add(tempYeniTapuSahibi);
                    //statusYaziEkle(comingRequest.mesaj);
                    statusYaziEkle(comingRequest.mesaj + " " + ID + " tarafından satın alındı." + Environment.NewLine);
                    sendAnswer.tempTapuSahibi = comingRequest.tempTapuSahibi;
                    sendAnswer.mesaj = comingRequest.mesaj;
                    sendAnswer.oyuncu_rengi = comingRequest.renk;
                    sendAnswer.kareID = comingRequest.kareID;
                    sendAnswer.oyuncu_rengi = comingRequest.renk;
                    //sendAnswer = kareSatinAl(comingRequest, ID, TapuSenediBilgisiGetir(comingRequest.kareID));
                    break;
                case "kiraOdendi":
                    sendAnswer.ucret = comingRequest.ucret;
                    sendAnswer.oyuncuID = ID;
                    sendAnswer.kareID = comingRequest.kareID;
                    break;
                case "şans":
                    sendAnswer.oyuncuID = ID;
                    sendAnswer.mesaj = comingRequest.mesaj;
                    break;
                case "kamufonu":
                    sendAnswer.oyuncuID = ID;
                    sendAnswer.mesaj = comingRequest.mesaj;
                    break;
                case "piyonBilgisi":
                    //OyuncuListesi.Where(search=> search.OyuncuBilgi.oyuncuID==ID).Single().OyuncuBilgi.piyon = comingRequest.kareID;
                    if(Oyuncu1.oyuncuID==ID)
                    {
                        Oyuncu1.piyon = comingRequest.kareID;
                    }
                    else if (Oyuncu2.oyuncuID == ID)
                    {
                        Oyuncu2.piyon = comingRequest.kareID;
                    }
                    else if (Oyuncu3.oyuncuID == ID)
                    {
                        Oyuncu3.piyon = comingRequest.kareID;
                    }
                    statusYaziEkle(ID + " oyuncusunun piyonu : " + comingRequest.kareID.ToString());
                    break;
                case "kodesegir":
                case "kodestencik":
                    statusYaziEkle(comingRequest.type + " : " + comingRequest.Zar.userServerID + " from " + ID );
                    sendAnswer.zarlar = comingRequest.Zar;
                    sendAnswer.oyuncuID = ID;
                    break;
                case "sadecegit":
                case "biyeregit":
                case "heroyuncuya50ode":
                case "3haneilerigit":
                case "biyeregitkamu10":
                case "oyundanCik":
                    
                    sendAnswer.zarlar = comingRequest.Zar;
                    sendAnswer.oyuncuID = ID;
                    sendAnswer.kareID = comingRequest.kareID;
                    break;
                case "ipotekEdildi":
                case "ipotekKaldirildi":
                case "evKuruldu":
                case "evIadeEdildi":
                case "otelKuruldu":
                case "caddeOnarimlari":
                case "otelIadeEdildi":
                    sendAnswer.oyuncuID = ID;
                    sendAnswer.kareID = comingRequest.kareID;
                    sendAnswer.ucret = comingRequest.ucret;
                    break;
                case "sirayiIlerlet":
                    if(comingRequest.kareID>0)
                    {
                        sendAnswer.oyuncuID = ID;
                    }
                    else
                    {
                        sendAnswer.oyuncuID = siraKimde();
                    }
                    
                    comingRequest.type = "siraSende";
                    statusYaziEkle("Sıra şu oyuncuda: " + sendAnswer.oyuncuID);
                    break;

                    

                }
            broadcastData(comingRequest.type, sendAnswer);
        }

        #endregion

        #region SERVER onConnection
        int renksayac = 0;
        void MonopolyGameServer_onConnection(string ID)
        {
            statusYaziEkle(ID + " is Connected.");
            string oyuncuRengi;
            answer serverAnswer = new answer();
            Color renk = new Color();
            switch (renksayac)
            {
                case 0:
                    oyuncuRengi="kırmızı";
                    renk = Color.Red;
                    Oyuncu1 = new KayitliOyunDetay();
                    Oyuncu1.oyuncuID = ID;
                    Oyuncu1.renk = oyuncuRengi;
                    break;
                case 1:
                    oyuncuRengi="yeşil";
                    renk = Color.Green;
                    Oyuncu2 = new KayitliOyunDetay();
                    Oyuncu2.oyuncuID = ID;
                    Oyuncu2.renk = oyuncuRengi;
                    break;
                case 2:
                    oyuncuRengi="mavi";
                    renk = Color.Blue;
                    Oyuncu3 = new KayitliOyunDetay();
                    Oyuncu3.oyuncuID = ID;
                    Oyuncu3.renk = oyuncuRengi;
                    break;
                case 3:
                    oyuncuRengi="sarı";
                    renk = Color.Yellow;
                    break;
                default:
                    renksayac = 0;
                    oyuncuRengi = "sarı";
                    break;
            }
            renksayac++;

            serverAnswer.oyuncu_rengi = renk;
            serverAnswer.mesaj = oyuncuRengi;
            sendDataToClienth("renkBilgisi", serverAnswer,ID);
            broadcastData("renkBilgisi", serverAnswer);
            statusYaziEkle(ID + " oyuncusunun rengi: " + oyuncuRengi); 
        }
        #endregion

        public void sendDataToClienth(string Type, answer answerObject, string userID)
        {
            answerObject.type = Type;
            string serverAnswerString = JsonConvert.SerializeObject(answerObject, Formatting.Indented,
            new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            MonopolyGameServer.SendData(userID,UTF8Encoding.UTF8.GetBytes(serverAnswerString));
        }

        #region Broadcast Data
        public void broadcastData(string Type, answer answerObject)
        {
            answerObject.type = Type;
            string serverAnswerString = JsonConvert.SerializeObject(answerObject, Formatting.Indented,
            new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            MonopolyGameServer.Brodcast(UTF8Encoding.UTF8.GetBytes(serverAnswerString));
        }
        #endregion


        #region Broadcast Data OVERLOAD -  string type Almaz!
        public void broadcastData(answer answerObject)
        {
            
            string serverAnswerString = JsonConvert.SerializeObject(answerObject, Formatting.Indented,
            new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            MonopolyGameServer.Brodcast(UTF8Encoding.UTF8.GetBytes(serverAnswerString));
        }
        #endregion



        #region Oyunu Başlat
        private void btn_baslat_Click(object sender, EventArgs e)
        {
            if(MonopolyGameServer.Users.Count<2)
            {
                MessageBox.Show("Oyunu başlatabilmek için en az 2 oyuncu gereklidir.", "Yeterli Oyuncu Yok");
            }
            else
            {

                btn_baslat.Enabled = false;
                btn_oyunu_bitir.Enabled = true;
                if(check_zamanli_mi.Checked==true)
                {
                    tmr_game_time.Enabled = true;
                    tmr_game_time.Start();
                    
                    int dakika,saat;

                    dakika = Convert.ToInt32(txt_dakika.Value);
                    saat = Convert.ToInt32(txt_saat.Value);

                    oyunBitisZamani = ((dakika * 60) + (saat * 60 * 60)) * 1000;

                    MessageBox.Show("bitis : " + oyunBitisZamani.ToString());

                }
                else
                {
                    tmr_game_time.Enabled = false;
                }
                answer serverAnswer = new answer();
                serverAnswer.oyuncu_parasi = 15000;
                if (Oyuncu1 != null)
                {
                    Oyuncu1.oyuncuPara = 1500;
                    serverAnswer.OyuncuDetay = Oyuncu1;
                    broadcastData("oyuncu1", serverAnswer);
                }
                if (Oyuncu2 != null)
                {
                    Oyuncu2.oyuncuPara = 1500;
                    serverAnswer.OyuncuDetay = Oyuncu2;
                    broadcastData("oyuncu2", serverAnswer);
                }
                if (Oyuncu3 != null)
                {
                    Oyuncu3.oyuncuPara = 1500;
                    serverAnswer.OyuncuDetay = Oyuncu3;
                    broadcastData("oyuncu3", serverAnswer);
                }


               

                serverAnswer.TapularinSahipleriListesi = TapuSahipleriListesi;
                serverAnswer.OyuncuDetay = TekOyuncuDetay;
                
            
                broadcastData("oyunaBasla", serverAnswer);
                txt_status.AppendText("Oyun başladı.İlk oynama hakkına sahip oyuncu: " + MonopolyGameServer.Users[0] + Environment.NewLine + "Oyuncular: " + Environment.NewLine);
                int i = 0;
                foreach (var nick in MonopolyGameServer.Users)
                {
                    i++;
                    TekOyuncuDetay.oyuncuID = nick;
                    statusYaziEkle("Oyuncu " + i + " = " + TekOyuncuDetay.oyuncuID);
                    
                }    
            }
            
            
        }


        private string siraKimde()
        {
            siraOyuncuIDSayac++;
            if (siraOyuncuIDSayac == MonopolyGameServer.Users.Count())
            {
                siraOyuncuIDSayac = 0;
            }
            return MonopolyGameServer.Users.ElementAt(siraOyuncuIDSayac);
            
        }

        #endregion


        


        private void statusYaziEkle(string metin)
        {
            txt_status.AppendText(metin + Environment.NewLine);
            txt_status.ScrollToCaret();
        }

        private void check_zamanli_mi_CheckedChanged(object sender, EventArgs e)
        {
            if(check_zamanli_mi.Checked==true)
            {
                gb_oyun_suresi.Visible = true;
                
            }
            else
            {
                gb_oyun_suresi.Visible = false;
            }
        }


        private void tmr_game_time_Tick(object sender, EventArgs e)
        {
            
            gecenZaman += 1000;
            if(gecenZaman>=oyunBitisZamani)
            {
                MessageBox.Show("Oyun Bitti");
                tmr_game_time.Stop();
                tmr_game_time.Enabled = false;
                
            }
        }

      
       

        

        
    }
}
