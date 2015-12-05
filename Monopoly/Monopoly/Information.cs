using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Monopoly
{
    class Information
    {
        public string type { get; set; }
        public int kareID { get; set; }
        public int ucret { get; set; }
        public string mesaj { get; set; }
        public Color renk { get; set; }
        public TapularinSahipleri tempTapuSahibi { get; set; }
        public TapuSenetleri tempTapu { get; set; }
        public zar Zar { get; set; }
    }
}
