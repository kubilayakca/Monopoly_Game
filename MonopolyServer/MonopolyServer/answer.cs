using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MonopolyServer
{
    public class answer
    {
        public string type { get; set; }
        public string mesaj { get; set; }
        public zar zarlar { get; set; }
        public TapuSenetleri tapuSenedi { get; set; }
        public int kareID { get; set; }
        public int ucret { get; set; }
        public string isClickable { get; set; }
        public string oyuncuID { get; set; }
        public int oyuncu_parasi { get; set; }
        public Color oyuncu_rengi { get; set; }
        public List<TapularinSahipleri> TapularinSahipleriListesi { get; set; }
        public TapularinSahipleri tempTapuSahibi { get; set; }
        public KayitliOyunDetay OyuncuDetay { get; set; }
    }
}
