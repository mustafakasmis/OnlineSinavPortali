﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineSinavPortali
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SinavPortalEntities : DbContext
    {
        public SinavPortalEntities()
            : base("name=SinavPortalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<cevaplar> cevaplar { get; set; }
        public virtual DbSet<degerlendirme> degerlendirme { get; set; }
        public virtual DbSet<girisler> girisler { get; set; }
        public virtual DbSet<kategoriler> kategoriler { get; set; }
        public virtual DbSet<kullanici_cevaplar> kullanici_cevaplar { get; set; }
        public virtual DbSet<kullanicilar> kullanicilar { get; set; }
        public virtual DbSet<resimler> resimler { get; set; }
        public virtual DbSet<secenekler> secenekler { get; set; }
        public virtual DbSet<sinavIstatistik> sinavIstatistik { get; set; }
        public virtual DbSet<sorular> sorular { get; set; }
    }
}
