﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MonopolyEntities10 : DbContext
    {
        public MonopolyEntities10()
            : base("name=MonopolyEntities10")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<KamuFonuKartlari> KamuFonuKartlari { get; set; }
        public virtual DbSet<KartTipleri> KartTipleri { get; set; }
        public virtual DbSet<KayitliOyun> KayitliOyun { get; set; }
        public virtual DbSet<KayitliOyunDetay> KayitliOyunDetay { get; set; }
        public virtual DbSet<OyunAlani> OyunAlani { get; set; }
        public virtual DbSet<Piyonlar> Piyonlar { get; set; }
        public virtual DbSet<SansKartlari> SansKartlari { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TapularinSahipleri> TapularinSahipleri { get; set; }
        public virtual DbSet<TapuSenetleri> TapuSenetleri { get; set; }
        public virtual DbSet<OyunKurallari> OyunKurallari { get; set; }
    }
}
