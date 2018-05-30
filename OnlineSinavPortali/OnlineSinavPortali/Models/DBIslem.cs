using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using System.IO;
using System.Threading;

namespace OnlineSinavPortali.Models
{

    public class DBIslem:DbContext
    {

        // İLGİLİ VERİTABANI ENTİTY NESNESİ
        private SinavPortalEntities sp = new SinavPortalEntities();
       
        // BÜTÜN KATEGORİLERİ VE ALT KATEGORİLERİ TOPLADIĞIMIZ LİSTLER
        private List<kategoriler> kats;
        private List<kategoriler> subkats;


        // İLGİLİ SINAVLAR, KATEGORİLER, SINAVA GİREN KİŞİLER İÇİN KULLANILAN KISIMLAR
        private List<SelectListItem> rootKats = new List<SelectListItem>();
        private List<SelectListItem> tumKategoriler = new List<SelectListItem>();
        private List<SelectListItem> tumSinavlar = new List<SelectListItem>();
        private List<SelectListItem> sinavKisiler = new List<SelectListItem>();
        private List<SelectListItem> kisiSinavlar = new List<SelectListItem>();


        //SİSTEME GİRİŞ YAPAN KULLANICININ GİRDİĞİ SINAVLARIN TUTULDUĞU KISIM
        private List<SelectListItem> girilenSinavlar = new List<SelectListItem>();


        // KULLANICI TARAFINA DÖNDÜRÜLECEK SINAVLARIN EKLENDİĞİ KISIMLAR
        private  List<kategoriler> mevcutSinavlar = new List<kategoriler>();
        private  List<kategoriler> sinavUstKategori = new List<kategoriler>();


        // İLGİLİ VERİLERİ EKLERKEN VE GÜNCELLERKEN KULLANILAN TABLO NESNELERİ
        public string kat;
        public string prtKat;

        public int srdid;
        public int ktgid;

        public sorular s = new sorular();
        public cevaplar c = new cevaplar();
        public resimler r = new resimler();
        public secenekler scnk = new secenekler();
        public degerlendirme deger = new degerlendirme();

        public degerlendirme guncelleDeğ = new degerlendirme();

        public kullanici_cevaplar kc = new kullanici_cevaplar();

        public sinavIstatistik si = new sinavIstatistik();

        // AKADEMİSYEN TARAFINA İLGİLİ SINAV SORU,CEVAP VS. GERİ DÖNDÜRMEK İÇİN KULLANILAN NESNELER
        public List<sorular> Sorular;
        public List<secenekler> Secenekler;
        public List<cevaplar> Cevaplar;
        public List<resimler> Resimler;


        // KULLANICI TARAFINDA SINAV KISMINDA GÖSTERİLECEK OLAN SORULAR,RESİMLER VE SEÇENEKLER
        public List<sorular> sinavSorular=new List<sorular>();
        public List<secenekler> sinavSecenekler=new List<secenekler>();
        public List<resimler> sinavResimler=new List<resimler>();


        //AKADEMİSYEN TA0RAFINA SINAV İSTATİĞİ DÖNDÜRMEK İÇİN KULLANILIR
        public List<girisler> kisiler = new List<girisler>();
        public List<sinavIstatistik> kisiSinavIs = new List<sinavIstatistik>();


        //AKADEMİSYEN TARAFINA SINAV YORUMLARININ DONDURULMESİ
        public List<girisler> yorumluKisiler = new List<girisler>();
        public List<degerlendirme> kisiSinavYorumlar = new List<degerlendirme>();


        //KULLANICILARIN İSTATİSTİKLERİNİ HESAPLAMADA KULLANILANLAR
        public int dogru = 0, yanlis = 0, puan = 0;


        /* BU KISIM KULLANICI BAZLI YAPILAN İŞLEMLERİN OLDUĞU KISIMDIR*/


        // SİSTEME KAYIT OLUNAN KISIM
        public bool kullaniciEkle(string ad,string soyad,string e_mail,string kullanici_ad,string parola,string durum,string uyelik_tarih)
        {
            kullanicilar k = new kullanicilar();

            k.ad = ad; k.soyad = soyad; k.e_mail = e_mail;
            k.uyelik_tarihi = uyelik_tarih;

            sp.kullanicilar.Add(k);
            sp.SaveChanges();

            var kayit = sp.kullanicilar.ToList();

            var veri = kayit.Last();

            girisler g = new girisler();

            g.kid = veri.id;  g.kullanici_adi = kullanici_ad;
            g.durum = durum; g.parola = parola;

            sp.girisler.Add(g);

            sp.SaveChanges();

            k = null;
            g = null;

            kayit = null;
            veri = null;

            return true;
        }

        // SİSTEME GİRİŞ YAPAN KİMSENİN BİLGİLERİNİN GETRİLDİĞİ KISIM
        public girisler kullaniciGetir(string ad,string parola)
        {
            var kisi = sp.girisler.Where(x => x.kullanici_adi == ad && x.parola == parola).FirstOrDefault();
            return kisi;
        }

        //SİSTEME KAYIT OLUNURKEN BİRDEN FAZLA AYNI KAYDI KONTROL ETMEK İÇİN YAZILMIŞ BİR METOD
        public bool kullaniciVarMi(string mail,string kad)
        {
            kullanicilar k = sp.kullanicilar.Where(x => x.e_mail == mail).FirstOrDefault();
            girisler g = sp.girisler.Where(x => x.kullanici_adi == kad).FirstOrDefault();

            if (k != null && g!=null)
                return true;

            else
                return false;

        }

        //PAROLA UNUTULDUĞU TAKDİRDE PAROLANIN SIFIRLANDIĞI YER
        public bool parolaSifirla(string parola,string mail)
        {
            kullanicilar kisi = sp.kullanicilar.Where(x => x.e_mail == mail).FirstOrDefault();

            if (kisi != null)
            {
                girisler kisiBilgi = sp.girisler.Where(x => x.kid == kisi.id).FirstOrDefault();

                kisiBilgi.parola = parola;

                sp.SaveChanges();

                return true;
            }

            else
              return false;
            
        }

        //PROFİL DÜZENLEME İÇİN DÖNEN GEREKLİ KİŞNİN GİRİŞ BİLGİLERİ
        public girisler profilGirisKisiGetir(string ad)
        {
            girisler g = sp.girisler.FirstOrDefault(x => x.kullanici_adi == ad);

            return g;
        }

        //PROFİL DÜZENLEME İÇİN DÖNEN GEREKLİ KİŞİNİN KULLANICILARI BİLGİLERİ
        public kullanicilar profilKullaniciKisiGetir(int id)
        {
            kullanicilar k = sp.kullanicilar.FirstOrDefault(x => x.id == id);

            return k;
        }


        //KULLANICININ PROFİLİNİN GÜNCELLENDİĞİ KISIM
        public bool profilGuncelle(string asilKisi, string ad, string soyad, string e_mail, string kullanici_ad, string parola)
        {
            try
            {

                girisler g = sp.girisler.Where(x => x.kullanici_adi == asilKisi).FirstOrDefault();
                kullanicilar k = sp.kullanicilar.Where(x => x.id == g.kid).FirstOrDefault();

                k.ad = ad; k.soyad = soyad;
                k.e_mail = e_mail; g.kullanici_adi = kullanici_ad;

                if (parola != null)
                    g.parola = parola;

                sp.SaveChanges();

            }

            catch (Exception e)
            {
                return false;
            }

            return true;
        }


        //SİSTEME GİRİŞ YAPAN KULLANICININ GİRDİĞİ SINAVLARI GERİ DÖNDÜREN METHOD
        public List<SelectListItem> kullaniciGirilenSinavlar(string kullanici_ad)
        {
            kategoriler kategori = null;

            girisler k = sp.girisler.Where(x => x.kullanici_adi == kullanici_ad).FirstOrDefault();

            List<int> katIdler = sp.kullanici_cevaplar.Where(x => x.klnc_id == k.id).Select(x=>x.KTGID).Distinct().ToList();

            foreach(var item in katIdler)
            {
                kategori = sp.kategoriler.Where(x => x.id == item).FirstOrDefault();
                girilenSinavlar.Add(new SelectListItem { Text = kategori.kategoriAdi, Value = kategori.kategoriAdi });
            }

            return girilenSinavlar;
        }


        //MEVCUT SINAVLARIN VE ÜST KATEGORİLERİNİN KULLANICI TARAFINA DÖNDÜRÜLMESİ
        public List<kategoriler> sistemMevcutSinav()
        {
            var x = sp.sorular.Select(s => s.kategoriId).Distinct().ToList();

            kategoriler kat = null;
            kategoriler ustKat = null;

            foreach(var i in x)
            {
                kat = sp.kategoriler.Where(t => t.id == i).FirstOrDefault();
                mevcutSinavlar.Add(kat);
                ustKat = sp.kategoriler.Where(z => z.id == kat.parentId).FirstOrDefault();
                sinavUstKategori.Add(ustKat);
            }

            return mevcutSinavlar;
        }


        public List<kategoriler> sistemMevcutKategori()
        {
            return sinavUstKategori;
        }


        // KULLANICI SINAVINDA GÖSTERİLECEK SORULARIN,SEÇENEKLERİN,RESİMLERİN DOLDURULMASI
        public string sinavlariOlustur(string gelen)
        {

            string[] metinPrc = gelen.Split('-');

            int sinavID = Convert.ToInt32(metinPrc[0]);
            int sinavKatID = Convert.ToInt32(metinPrc[1]);

            string sinavAdi = sp.kategoriler.Where(x => x.id == sinavID && x.parentId == sinavKatID).Select(x => x.kategoriAdi).FirstOrDefault();

            var snvSoru = sp.sorular.Where(x => x.kategoriId == sinavID).ToList();
            var snvSecenek = sp.secenekler.Where(x => x.Ktg_id == sinavID).ToList();
            var snvResim = sp.resimler.Where(x => x.KTG_ID == sinavID).ToList();

            foreach (var a in snvSoru)
                sinavSorular.Add(a);

            foreach (var b in snvSecenek)
                sinavSecenekler.Add(b);

            foreach (var c in snvResim)
                sinavResimler.Add(c);

            return sinavAdi;

        }


        // KULLANICININ CEVAP VERDİĞİ SORUNUN İD VE KATEGORİ İD'SİNİN BULUNUP GERİ DÖNDÜRÜLMESİ
        public string cevapSoruIdDon(string cevap)
        {
            secenekler ktgSrdID1 = sp.secenekler.Where(x => x.secenek1 == cevap).FirstOrDefault();
            secenekler ktgSrdID2 = sp.secenekler.Where(x => x.secenek2 == cevap).FirstOrDefault();
            secenekler ktgSrdID3 = sp.secenekler.Where(x => x.secenek3 == cevap).FirstOrDefault();
            secenekler ktgSrdID4 = sp.secenekler.Where(x => x.secenek4 == cevap).FirstOrDefault();

            if(ktgSrdID1!=null)
            {
                return ktgSrdID1.soru_id + "-" + ktgSrdID1.Ktg_id;
            }

            if (ktgSrdID2 != null)
            {
                return ktgSrdID2.soru_id + "-" + ktgSrdID2.Ktg_id;
            }

            if (ktgSrdID3 != null)
            {
                return ktgSrdID3.soru_id + "-" + ktgSrdID3.Ktg_id;
            }

            if (ktgSrdID4 != null)
            {
                return ktgSrdID4.soru_id + "-" + ktgSrdID4.Ktg_id;
            }

            return null;
        }


        // KULLANICIN SINAV OLURKEN CEVAPLARININ EKLENDİĞİ KISIM
        public bool kullaniciSinavCevapEkle(int soruID,int kategoriID,string kullanici,string cevap)
        {
                girisler puanVeren = sp.girisler.Where(x => x.kullanici_adi == kullanici).FirstOrDefault();

                kc.klnc_id = puanVeren.kid;

                kc.KTGID = kategoriID;

                kc.soruid = soruID;

                kc.klnc_cevap = cevap;

                sp.kullanici_cevaplar.Add(kc);

                sp.SaveChanges();

                return true;


        }


        // KULLANICIN SINAV DEĞERLENDİRMESİNDE VERDİĞİ PUANIN EKLENDİĞİ KISIM

        public bool kullaniciDeğPuanEkle(int kategoriID,int puan,string kullanici)
        {

            try
            {
                girisler puanVeren = sp.girisler.Where(x => x.kullanici_adi == kullanici).FirstOrDefault();

                deger.katId = kategoriID;
                deger.kullanici_id = puanVeren.kid;
                deger.puan = puan;
                deger.yorum = " ";

                sp.degerlendirme.Add(deger);

                sp.SaveChanges();

                return true;
            }

            catch(Exception e)
            {
                return false;
            }

        }

        // KULLANICIN SINAV DEĞERLENDİRMESİNDE VERDİĞİ YORUMUNUN EKLENDİĞİ KISIM

        public bool kullaniciDeğYorumEkle(int kategoriID, string yorum, string kullanici)
        {

            try
            {
                girisler puanVeren = sp.girisler.Where(x => x.kullanici_adi == kullanici).FirstOrDefault();

                degerlendirme guncelleDeğ = sp.degerlendirme.Where(x => x.kullanici_id == puanVeren.kid && x.katId == kategoriID).FirstOrDefault();

                guncelleDeğ.katId = kategoriID;

                guncelleDeğ.kullanici_id = puanVeren.kid;
                
                guncelleDeğ.yorum = yorum;

                sp.SaveChanges();

                return true;
            }

            catch (Exception e)
            {
                return false;
            }

        }

        // SINAV İSTATİSTİĞİ OLUP OLMADIĞININ BELİRLENDİĞİ YER

        public bool sinavIstatistigiVarMi(string kullanici)
        {

            girisler g = sp.girisler.Where(x => x.kullanici_adi == kullanici).FirstOrDefault();

            var istatistik = sp.sinavIstatistik.Where(y => y.Kullanici_ID == g.kid).ToList();

            if (istatistik.Count != 0)
            {
                return true;
            }

            else
                return false;

        }

        // KULLANICININ GÖRECEĞİ SINAV İSTATİSTİKLERİ VE SINAVDA BAŞARILI OLAN KULLANICININ İSTATİSTİĞİ

        public sinavIstatistik kullaniciIstatistigi(string kullanici,string sinav)
        {

            girisler g = sp.girisler.Where(x => x.kullanici_adi == kullanici).FirstOrDefault();

            kategoriler k = sp.kategoriler.Where(x => x.kategoriAdi == sinav).FirstOrDefault();

            sinavIstatistik d = sp.sinavIstatistik.Where(x => x.KAT_ID == k.id && x.Kullanici_ID == g.kid).FirstOrDefault();

            return d;
        }

        // BAŞARILI KULLANICININ BELİRLENDİĞİ KISIM

        public List<string> sinavBasariliKullanici(string sinav)
        {
            kategoriler k = sp.kategoriler.Where(x => x.kategoriAdi == sinav).FirstOrDefault();

            var gelenler = sp.sinavIstatistik.Where(x => x.KAT_ID == k.id).ToList();

            int? maxDogru = gelenler.Max(x => x.dogruSay).Value;
            int? minYanlis = gelenler.Min(x => x.yanlisSay).Value;
            int? maxPuan = gelenler.Max(x => x.puan).Value;

   sinavIstatistik kisi = sp.sinavIstatistik.Where(x => x.dogruSay == maxDogru && x.yanlisSay == minYanlis && x.puan == maxPuan).FirstOrDefault();

   string kullaniciAd = sp.girisler.Where(x => x.kid == kisi.Kullanici_ID).FirstOrDefault().kullanici_adi;

   return new List<string>() {kullaniciAd,Convert.ToString(maxDogru), Convert.ToString(minYanlis), Convert.ToString(maxPuan) };

        }


        /* BU KISIM AKADEMİSYEN VE DEĞERLENDİRİCİ BAZLI YAPILAN İŞLEMLERİN OLDUĞU KISIMDIR*/


        public List<SelectListItem> tumKategorilerGetir()
        {
            foreach (var item in sp.kategoriler.ToList())
            {
                tumKategoriler.Add(new SelectListItem { Text = item.kategoriAdi, Value = item.kategoriAdi });
            }

            return tumKategoriler;
        }

        public List<SelectListItem> anaKategorileriGetir()
        {
            foreach( var item in sp.kategoriler.Where(x => x.parentId == null).ToList())
            {
                rootKats.Add(new SelectListItem { Text = item.kategoriAdi, Value = item.kategoriAdi });
            }

            return rootKats;
        }

        public List<SelectListItem> sinavlariGetir()
        {
            
            var sinavlar = sp.sorular.Select(x => x.kategoriId).Distinct().ToList();
            
            foreach (var item in sinavlar)
            {
          var gelenSinav = sp.kategoriler.FirstOrDefault(x=> x.id==item);
        tumSinavlar.Add(new SelectListItem { Text = gelenSinav.kategoriAdi, Value = gelenSinav.kategoriAdi });
            }

            return tumSinavlar;

        }

        public List<SelectListItem> sinavliKisileriGetir()
        {
            var kisiIdSay = sp.kullanici_cevaplar.Select(x => x.klnc_id).Distinct();

            girisler gecici = null;

            foreach(var item in kisiIdSay)
            {
                gecici = sp.girisler.Where(x => x.kid == item).FirstOrDefault();

                sinavKisiler.Add(new SelectListItem { Text = gecici.kullanici_adi, Value = gecici.kullanici_adi });
            }

            return sinavKisiler;
        }

        public List<SelectListItem> kisiSinavlariGetir(string kullanici)
        {

            girisler kisi = sp.girisler.Where(x => x.kullanici_adi==kullanici).FirstOrDefault();

  List<int> sinavid = sp.kullanici_cevaplar.Where(x => x.klnc_id == kisi.kid).Select(x => x.KTGID).Distinct().ToList();

            kategoriler ekleKategori = null;

            foreach(var item in sinavid)
            {
                ekleKategori = sp.kategoriler.Where(x => x.id == item).FirstOrDefault();
                kisiSinavlar.Add(new SelectListItem { Text = ekleKategori.kategoriAdi, Value = ekleKategori.kategoriAdi });
            }

            return kisiSinavlar;
            
        }



        // SEÇİLİ KATEGORİNİN ALT KATEGORİLERİ ÇEKİLMEKTEDİR
        public List<kategoriler> kategorileriCek(string kategoriAdi)
        {
            List<kategoriler> gidecek = new List<kategoriler>();

            kats = sp.kategoriler.ToList();
            var kategori = kats.Where(x => x.kategoriAdi == kategoriAdi).FirstOrDefault();

            if (kategori != null)
            {
                AltKategoriler(kategori.id, gidecek);
            }

            return gidecek;
        }

        public void AltKategoriler(int topId, List<kategoriler> gidecek)
        {

            subkats = kats.Where(x => x.parentId == topId).ToList();

            if (subkats.Count == 0)
            {
                return;
            }

            foreach (kategoriler item in subkats)
            {

                kategoriler ktg = new kategoriler();

                ktg.id = item.id;

                ktg.kategoriAdi = item.kategoriAdi;

                ktg.parentId = item.parentId;

                gidecek.Add(ktg);

                ktg = null;

                AltKategoriler(item.id, gidecek);

            }

        }



        //BURADA KATEGORİ EKLENMEKTEDİR
        public bool kategoriEkle(string kategori, string ustKatAdi)
        {

            var kayit = sp.kategoriler.Where(x => x.kategoriAdi == ustKatAdi).FirstOrDefault();

            kategoriler ktg = new kategoriler();

            try
            {

                if (String.IsNullOrEmpty(ustKatAdi))
                {
                    ktg.kategoriAdi = kategori;
                    ktg.parentId = null;

                    sp.kategoriler.Add(ktg);

                    sp.SaveChanges();
                }

                else
                {

                    ktg.kategoriAdi = kategori;
                    ktg.parentId = kayit.id;

                    sp.kategoriler.Add(ktg);

                    sp.SaveChanges();
                   
                }

            }

            catch (Exception e)
            {
                return false;
            }

            ktg = null;
            kayit = null;

            return true;
        }

        public bool kategoriGuncelle(string kategoriAdi,string ad, string parentKategori)
        {

            if (kategoriAdi != null && ad != null && parentKategori != null)
            {
                var kategori = sp.kategoriler.Where(x => x.kategoriAdi == kategoriAdi).FirstOrDefault();
                var gelenKategori = sp.kategoriler.Where(x => x.kategoriAdi == parentKategori).FirstOrDefault();

                kategori.kategoriAdi = ad;
                kategori.parentId = gelenKategori.id;

                sp.SaveChanges();
            }
            
            return true;

        }



        // BURADA SEÇİLİ KATEGORİ VE ALT KATEGORİLERİ SİLİNİYOR
        public bool kategoriSil(string kategoriAdi)
        {
            kats = sp.kategoriler.ToList();

            var kategori = kats.Where(x => x.kategoriAdi == kategoriAdi).FirstOrDefault();

            if (kategori != null)
            {
                katSoruCevapSil(kategori.id);

                sil(kategori.id);

                var kayit = sp.kategoriler.Where(x => x.id == kategori.id).FirstOrDefault();

                sp.kategoriler.Remove(kayit);

                sp.SaveChanges();
            }

            return true;
        }


        public void sil(int id)
        {

            subkats = kats.Where(x => x.parentId == id).ToList();

            if (subkats.Count == 0)
                return;


            foreach (var item in subkats)
            {

                sp.kategoriler.Remove(item);

                sp.SaveChanges();

                sil(item.id);

            }

        }


        public void katSoruCevapSil(int katid)
        {

            var silinecekSorular = sp.sorular.Where(x => x.kategoriId == katid).ToList();
            var silinecekSecenekler = sp.secenekler.Where(x => x.Ktg_id == katid).ToList();
            var silinecekCevaplar = sp.cevaplar.Where(x => x.KTG_id == katid).ToList();

            var silinecekResimler = sp.resimler.Where(x => x.KTG_ID == katid).ToList();
            var silinecekDegerlendirme = sp.degerlendirme.Where(x => x.katId == katid).ToList();
            var silinecekIstatistik = sp.sinavIstatistik.Where(x => x.KAT_ID == katid).ToList();
            var kullaniciCevaplar = sp.kullanici_cevaplar.Where(x => x.KTGID == katid).ToList();

            foreach (var item in silinecekSorular)
                sp.sorular.Remove(item);

            foreach (var item2 in silinecekSecenekler)
                sp.secenekler.Remove(item2);

            foreach (var item3 in silinecekCevaplar)
            {
                sp.cevaplar.Remove(item3);
            }


            foreach (var item4 in silinecekResimler)
            {
                sp.resimler.Remove(item4);
            }
       

            if (silinecekDegerlendirme != null)
            {
                foreach (var item5 in silinecekDegerlendirme)
                    sp.degerlendirme.Remove(item5);
            }

            if (silinecekIstatistik != null)
            {
                foreach (var item6 in silinecekIstatistik)
                    sp.sinavIstatistik.Remove(item6);
            }

            if (silinecekIstatistik != null)
            {
                foreach (var item7 in kullaniciCevaplar)
                    sp.kullanici_cevaplar.Remove(item7);
            }

            silinecekSorular = null; silinecekCevaplar = null; silinecekSecenekler = null; silinecekResimler = null;
     silinecekDegerlendirme = null;  silinecekIstatistik = null; kullaniciCevaplar = null;

        }



        // BU KISIMDA SORULAR SECENEKLER CEVAPLAR VE VARSA RESİMLER EKLENİYOR
        public bool sorucevapEkle(string kategori, string soru, string soruSeviye,string secenek1, string secenek2,
            string secenek3, string secenek4, string cevap, string soruResim,
            string cevapDokuman) {

            var kayit = sp.kategoriler.Where(x => x.kategoriAdi == kategori).FirstOrDefault();

            if (soruResim == null && cevapDokuman == null)
            {

                s.kategoriId = kayit.id;
                s.soru = soru;
                s.soruZorluk = soruSeviye;
                sp.sorular.Add(s);
                sp.SaveChanges();

                var soruBilgi = sp.sorular.ToList();
                var soruKayit = soruBilgi.Last();


                scnk.Ktg_id = kayit.id;
                scnk.soru_id = soruKayit.id;
                scnk.secenek1 = secenek1;
                scnk.secenek2 = secenek2;
                scnk.secenek3 = secenek3;
                scnk.secenek4 = secenek4;

                sp.secenekler.Add(scnk);
                sp.SaveChanges();

                c.KTG_id = kayit.id;
                c.soruID = soruKayit.id;
                c.cevap = cevap;
                c.cevapDokuman = cevapDokuman;

                sp.cevaplar.Add(c);
                sp.SaveChanges();


                var cevapBilgi = sp.cevaplar.ToList();
                var cevapKayit = cevapBilgi.Last();

                r.KTG_ID = kayit.id;
                r.srd_id = soruKayit.id;
                r.cvp_id = cevapKayit.id;
                r.soruResim = soruResim;
                

                sp.resimler.Add(r);
                sp.SaveChanges();
                
            }

            else if (soruResim == null && cevapDokuman != null)
            {


                s.kategoriId = kayit.id;
                s.soru = soru;
                s.soruZorluk = soruSeviye;
                sp.sorular.Add(s);
                sp.SaveChanges();

                var soruBilgi = sp.sorular.ToList();
                var soruKayit = soruBilgi.Last();


                scnk.Ktg_id = kayit.id;
                scnk.soru_id = soruKayit.id;
                scnk.secenek1 = secenek1;
                scnk.secenek2 = secenek2;
                scnk.secenek3 = secenek3;
                scnk.secenek4 = secenek4;

                sp.secenekler.Add(scnk);
                sp.SaveChanges();

                c.KTG_id = kayit.id;
                c.soruID = soruKayit.id;
                c.cevap = cevap;
                c.cevapDokuman = cevapDokuman;

                sp.cevaplar.Add(c);
                sp.SaveChanges();


                var cevapBilgi = sp.cevaplar.ToList();
                var cevapKayit = cevapBilgi.Last();

                r.KTG_ID = kayit.id;
                r.srd_id = soruKayit.id;
                r.cvp_id = cevapKayit.id;
                r.soruResim = soruResim;
                

                sp.resimler.Add(r);
                sp.SaveChanges();

            }

            else if (soruResim != null && cevapDokuman == null)
            {

                s.kategoriId = kayit.id;
                s.soru = soru;
                s.soruZorluk = soruSeviye;
                sp.sorular.Add(s);
                sp.SaveChanges();

                var soruBilgi = sp.sorular.ToList();
                var soruKayit = soruBilgi.Last();


                scnk.Ktg_id = kayit.id;
                scnk.soru_id = soruKayit.id;
                scnk.secenek1 = secenek1;
                scnk.secenek2 = secenek2;
                scnk.secenek3 = secenek3;
                scnk.secenek4 = secenek4;

                sp.secenekler.Add(scnk);
                sp.SaveChanges();

                c.KTG_id = kayit.id;
                c.soruID = soruKayit.id;
                c.cevap = cevap;
                c.cevapDokuman = cevapDokuman;

                sp.cevaplar.Add(c);
                sp.SaveChanges();


                var cevapBilgi = sp.cevaplar.ToList();
                var cevapKayit = cevapBilgi.Last();

                r.KTG_ID = kayit.id;
                r.srd_id = soruKayit.id;
                r.cvp_id = cevapKayit.id;
                r.soruResim = soruResim;
                

                sp.resimler.Add(r);
                sp.SaveChanges();
            }

            else
            {

                s.kategoriId = kayit.id;
                s.soru = soru;
                s.soruZorluk = soruSeviye;
                sp.sorular.Add(s);
                sp.SaveChanges();

                var soruBilgi = sp.sorular.ToList();
                var soruKayit = soruBilgi.Last();


                scnk.Ktg_id = kayit.id;
                scnk.soru_id = soruKayit.id;
                scnk.secenek1 = secenek1;
                scnk.secenek2 = secenek2;
                scnk.secenek3 = secenek3;
                scnk.secenek4 = secenek4;

                sp.secenekler.Add(scnk);
                sp.SaveChanges();

                c.KTG_id = kayit.id;
                c.soruID = soruKayit.id;
                c.cevap = cevap;
                c.cevapDokuman = cevapDokuman;

                sp.cevaplar.Add(c);
                sp.SaveChanges();


                var cevapBilgi = sp.cevaplar.ToList();
                var cevapKayit = cevapBilgi.Last();

                r.KTG_ID = kayit.id;
                r.srd_id = soruKayit.id;
                r.cvp_id = cevapKayit.id;
                r.soruResim = soruResim;
                

                sp.resimler.Add(r);
                sp.SaveChanges();

            }

            return true;
        }


        //SORU,SECENEK,CEVAP VS İLE ALAKALI LİSTELEME İŞLEMİNİN YAPILDIĞI KISIM
        public void sorucevapListele(string kategoriAdi)
        {

            var a = sp.kategoriler.Where(x => x.kategoriAdi == kategoriAdi).FirstOrDefault();

            Sorular = sp.sorular.Where(x => x.kategoriId == a.id).ToList();
            Secenekler= sp.secenekler.Where(x => x.Ktg_id == a.id).ToList();
            Cevaplar = sp.cevaplar.Where(x => x.KTG_id == a.id).ToList();
            Resimler = sp.resimler.Where(x => x.KTG_ID == a.id).ToList();

        }
        

        //SORU,SECENEK,CEVAP VS İLE ALAKALI GÜNCELLEME İŞLEMİNİN YAPILDIĞI KISIM
        public void soruCevapGuncelleForm(string katSorId) {

            
            string[] gelenIdler = katSorId.Split('-');

            int kategori = Convert.ToInt32(gelenIdler[0]);
            int soru = Convert.ToInt32(gelenIdler[1]);

            var xyz = sp.kategoriler.Where(t => t.id == kategori).FirstOrDefault();
            var tvt = sp.kategoriler.Where(t => t.id == xyz.parentId).FirstOrDefault();

            var guncelsoru = sp.sorular.Where(x => x.id == soru && x.kategoriId == kategori).FirstOrDefault();
            var guncelsecenek = sp.secenekler.Where(x => x.soru_id == soru && x.Ktg_id == kategori).FirstOrDefault();
            var guncelcevap = sp.cevaplar.Where(x => x.soruID == soru && x.KTG_id == kategori).FirstOrDefault();
            var guncelresim = sp.resimler.Where(x => x.srd_id == soru && x.KTG_ID == kategori).FirstOrDefault();

            kat = xyz.kategoriAdi;
            prtKat = tvt.kategoriAdi;

            s.soru = guncelsoru.soru;
            s.soruZorluk = guncelsoru.soruZorluk;

            scnk.secenek1 = guncelsecenek.secenek1; scnk.secenek2 = guncelsecenek.secenek2;
            scnk.secenek3 = guncelsecenek.secenek3; scnk.secenek4 = guncelsecenek.secenek4;

            c.cevap = guncelcevap.cevap;
            c.cevapDokuman=guncelcevap.cevapDokuman;

            r.soruResim = guncelresim.soruResim;
            

        }


        public bool sorucevapGuncelle(string kategori,string yeniKat,string soru,string soruSeviye, string secenek1, string secenek2,
             string secenek3, string secenek4, string cevap, string soruResim,
             string cevapDokuman)
        {

            var kayit = sp.kategoriler.Where(x => x.kategoriAdi == kategori).FirstOrDefault();
            var yeniKategori = sp.kategoriler.Where(x => x.kategoriAdi == yeniKat).FirstOrDefault();

            var guncelsoru = sp.sorular.Where(x => x.kategoriId==kayit.id).FirstOrDefault();
            var guncelsecenek = sp.secenekler.Where(x => x.Ktg_id==kayit.id).FirstOrDefault();
            var guncelcevap = sp.cevaplar.Where(x => x.KTG_id==kayit.id).FirstOrDefault();
            var guncelresim = sp.resimler.Where(x => x.KTG_ID==kayit.id).FirstOrDefault();

            guncelsoru.soru = soru;
            guncelsoru.kategoriId = yeniKategori.id;
            guncelsoru.soruZorluk = soruSeviye;

            guncelsecenek.Ktg_id = yeniKategori.id;

            guncelsecenek.secenek1 = secenek1;
            guncelsecenek.secenek2 = secenek2;
            guncelsecenek.secenek3 = secenek3;
            guncelsecenek.secenek4 = secenek4;

            guncelcevap.KTG_id = yeniKategori.id;
            guncelcevap.cevap = cevap;
            guncelcevap.cevapDokuman = cevapDokuman;

            guncelresim.KTG_ID = yeniKategori.id;
            guncelresim.soruResim = soruResim;
            

            sp.SaveChanges();

            return true;
        }


        //SORU,SECENEK,CEVAP VS İLE ALAKALI SİLME İŞLEMİNİN YAPILDIĞI KISIM
        public bool soruCevapSil(string katSorId)
        {
           
            string[] idler = katSorId.Split('-');

            int kategori = Convert.ToInt32(idler[0]);
            int soru = Convert.ToInt32(idler[1]);

            var silsoru = sp.sorular.Where(x => x.id == soru && x.kategoriId == kategori).FirstOrDefault();
            var silsecenek = sp.secenekler.Where(x => x.soru_id == soru && x.Ktg_id == kategori).FirstOrDefault();
            var silcevap = sp.cevaplar.Where(x => x.soruID == soru && x.KTG_id == kategori).FirstOrDefault();
            var silresim = sp.resimler.Where(x => x.srd_id == soru && x.KTG_ID == kategori).FirstOrDefault();


            sp.sorular.Remove(silsoru);
            sp.secenekler.Remove(silsecenek);
            sp.cevaplar.Remove(silcevap);
            sp.resimler.Remove(silresim);

            sp.SaveChanges();

            return true;

        }


        //DEĞERLENDİRİCİNİN SİSTEMDEKİ SINAVLARI DEĞERLENDİRDİĞİ KISIM
        public bool sinavSonuclariHesapla(string kullanici,string sinav)
        {

            try
            {
                girisler g = sp.girisler.Where(x => x.kullanici_adi == kullanici).FirstOrDefault();
                kategoriler k = sp.kategoriler.Where(y => y.kategoriAdi == sinav).FirstOrDefault();

                List<kullanici_cevaplar> kc = sp.kullanici_cevaplar.Where(z => z.klnc_id == g.kid && z.KTGID == k.id).ToList();
                List<cevaplar> c = sp.cevaplar.Where(t => t.KTG_id == k.id).ToList();


                for (int i = 0; i < c.Count; i++)
                {
                    for(int j = 0; j < kc.Count; j++)
                    {
                        if (c[i].soruID == kc[j].soruid)
                        {
                            if (kc[j].klnc_cevap == c[i].cevap)
                            {
                                dogru++;
                                puan += 5;
                            }

                        }
                    }
                }

                si.Kullanici_ID = g.kid;
                si.KAT_ID = k.id;
                si.dogruSay = dogru;
                si.yanlisSay = yanlis;
                si.puan = puan;

                sp.sinavIstatistik.Add(si);

                sp.SaveChanges();

                return true;
            }

            catch(Exception e)
            {
                return false;
            }

        }


        //SINAV İSTATİSTİKLERİNİN AKADEMİSYEN TARAFINA İLETİLDİĞİ KISIM
        public void akademisyenSinavIstatistikDon(string kategori)
        {
            kategoriler k = sp.kategoriler.Where(x => x.kategoriAdi == kategori).FirstOrDefault();
            var records = sp.sinavIstatistik.Where(x => x.KAT_ID == k.id).ToList();

            girisler g = null;

            foreach (var item in records)
            {
                g = sp.girisler.Where(x => x.kid == item.Kullanici_ID).FirstOrDefault();
                kisiler.Add(g);

                kisiSinavIs.Add(item);
            }

        }


        //AKADEMİSYEN TARAFINA SINAV YORUMLARININ DÖNDÜRÜLMESİ
        public void akademisyenSinavYorumDon(string kategori)
        {
            kategoriler k = sp.kategoriler.Where(x => x.kategoriAdi == kategori).FirstOrDefault();

            var fields = sp.degerlendirme.Where(x => x.katId == k.id).ToList();

            girisler g = null;

            foreach (var item in fields)
            {
                g = sp.girisler.Where(x => x.kid == item.kullanici_id).FirstOrDefault();
                yorumluKisiler.Add(g);

                kisiSinavYorumlar.Add(item);
            }

        }

    }

}