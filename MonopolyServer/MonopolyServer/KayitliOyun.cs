//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MonopolyServer
{
    using System;
    using System.Collections.Generic;
    
    public partial class KayitliOyun
    {
        public KayitliOyun()
        {
            this.KayitliOyunDetay = new HashSet<KayitliOyunDetay>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public int oyuncu_sayisi { get; set; }
        public System.DateTime tarih { get; set; }
    
        public virtual ICollection<KayitliOyunDetay> KayitliOyunDetay { get; set; }
    }
}
