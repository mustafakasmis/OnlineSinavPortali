//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class secenekler
    {
        public int id { get; set; }
        public int Ktg_id { get; set; }
        public int soru_id { get; set; }
        public string secenek1 { get; set; }
        public string secenek2 { get; set; }
        public string secenek3 { get; set; }
        public string secenek4 { get; set; }
    
        public virtual kategoriler kategoriler { get; set; }
        public virtual sorular sorular { get; set; }
    }
}
