using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSinavPortali.Filters;
using OnlineSinavPortali.Models;
using System.Web.Helpers;

namespace OnlineSinavPortali.Controllers
{

    [KullaniciFilter]
    public class KullaniciController : Controller
    {

        private DBIslem d = new DBIslem();


        // KULLANICI PROFİL DÜZENLEMEDE KULLANILAN KISIMLAR
        private string kad;
        private girisler gelenGirisKisi;
        private kullanicilar gelenKullaniciKisi;


        // KULLANICI İSTATİSTİKLERİNİN HESAPLANMASI İÇİN GEREKEN KULLANICININ GİRDİĞİ SINAVLAR
        private static List<SelectListItem> kullaniciGirilenSinav = null;


        // MEVCUT SINAV VE KATEGORİLERİ KULLANICI TARAFINA GELEN KISIMLARI
        private static List<kategoriler> gelenSinavlar = null;
        private static List<kategoriler> gelenKategoriler = null;


        // İLGİLİ SINAVIN KULLANICIYA YANITLANMASI İÇİN VERİLERİN TUTULDUĞU KISIM
        private List<sorular> sinavSorular = null;
        private List<secenekler> sinavSecenekler = null;
        private List<resimler> sinavResimler = null;


        // SINAV SORULARI VE İSTATİSTİK İLE ALAKALI BAZI VERİLERİN TUTULDUĞU KISIM
        private static string SINAVID = null;
        private static string SINAV = null;
        private static string CEVAP = null;
        private static string PUAN = null;

        private static string SoruBilgi=null;
        private static string[] soruBilgisi = null;

        private bool istatistikVarmi = false;
        

        // KULLANICI ANASAYFA
        public ActionResult KullaniciAnasayfa()
        {
            TempData["tarih"] = Session["uyelikTarih"].ToString();
            return View();
        }


        // SINAV KURALLARI SAYFASI
        public ActionResult KullaniciSinavKural()
        {
            TempData["tarih"] = Session["uyelikTarih"].ToString();

            return View();
        }


        // KULLANICI PROFİL DÜZENLEME SAYFASI
        public ActionResult KullaniciProfilDuzenle()
        {
            kad = Session["kullaniciAd"].ToString();
            TempData["tarih"] = Session["uyelikTarih"].ToString();

            gelenGirisKisi = d.profilGirisKisiGetir(kad);
            gelenKullaniciKisi = d.profilKullaniciKisiGetir(gelenGirisKisi.kid);

            return View(Tuple.Create(gelenKullaniciKisi,gelenGirisKisi));
        }

        
        // KULLANICI SINAVA GİRME SAYFASI
        public ActionResult KullaniciSinavGir()
        {
            TempData["tarih"] = Session["uyelikTarih"].ToString();


            gelenSinavlar = d.sistemMevcutSinav();
            gelenKategoriler = d.sistemMevcutKategori();


            return View(Tuple.Create(gelenSinavlar,gelenKategoriler));
        }


        // SINAVDA SORU CEVAPLANDIKTAN SONRA GERİ DÖNÜLEN SAYFA
        [HttpGet]
        public ActionResult KullaniciSinavSayfasiGt(string sinav)
        {
            SINAV = sinav;
            string sinavIsim = d.sinavlariOlustur(sinav);

            sinavSorular = d.sinavSorular;
            sinavSecenekler = d.sinavSecenekler;
            sinavResimler = d.sinavResimler;

            TempData["tarih"] = Session["uyelikTarih"].ToString();

            return View(Tuple.Create(sinavIsim, sinavSorular, sinavSecenekler, sinavResimler));
        }


        // SINAV SORULARININ KULLANICIYA GERİ DÖNDERİLDİĞİ YER
        [HttpPost]
        public ActionResult KullaniciSinavSayfasiPst(string sinav)
        {
            SINAV = sinav;
            string sinavIsim= d.sinavlariOlustur(sinav);

            sinavSorular = d.sinavSorular;
            sinavSecenekler = d.sinavSecenekler;
            sinavResimler = d.sinavResimler;

            TempData["tarih"] = Session["uyelikTarih"].ToString();

            return View(Tuple.Create(sinavIsim, sinavSorular, sinavSecenekler, sinavResimler));
        }


        // SORU CEVAPLANDIKTAN SONRA YÖNLENECEK SAYFAYA GÖNDEREN KISIM
        [HttpPost]
        public RedirectToRouteResult KullaniciSoruCevap(FormCollection frm)
        {

            CEVAP = frm["secenek"];

            SoruBilgi = d.cevapSoruIdDon(CEVAP);
           
            soruBilgisi = SoruBilgi.Split('-');

  bool eklenmeDurum = d.kullaniciSinavCevapEkle(Convert.ToInt32(soruBilgisi[0]), Convert.ToInt32(soruBilgisi[1]),Session["kullaniciAd"].ToString(),CEVAP);

            if (eklenmeDurum == true)
            {
                TempData["soruCevaplanmaDurum"]="SORU BAŞARIYLA CEVAPLANDI";
                return RedirectToAction("KullaniciSinavSayfasiGt", "Kullanici", new { SINAV });
            }
            else
            {
                TempData["soruCevaplanmaDurum"] = "SORU CEVAPLANAMADI";
                return RedirectToAction("KullaniciSinavSayfasiGt", "Kullanici", new { SINAV });
            }

        }


        // KULLANICI SINAV PUANLAMA VE DEĞERLENDİRME SAYFASI
        public ActionResult KullaniciSinavDegerlendirme()
        {
            return View();
        }


        // KULLANICI SINAV PUANLAMA VE DEĞERLENDİRME SAYFASI
        [HttpPost]
        public ActionResult KullaniciSinavDegerlendirme(string kategoriId)
        {
            TempData["tarih"] = Session["uyelikTarih"].ToString();
            SINAVID = kategoriId;
            return View();   
        }


        // KULLANICIN SINAVA PUAN VERİP KAYDEDİLDİĞİ KISIM
        [HttpPost]
        public RedirectToRouteResult KullaniciSinavDegerlendirmePuan(string sinavPuan)
        {

            TempData["tarih"] = Session["uyelikTarih"].ToString();

            PUAN = sinavPuan;

      bool puanEklenmeDurum = d.kullaniciDeğPuanEkle(Convert.ToInt32(SINAVID), Convert.ToInt32(PUAN),Session["kullaniciAd"].ToString());
            /* dbye puanı kaydet  daha sonra ise yorum kısmında ise dbdeki o kısma update cek*/

            if (puanEklenmeDurum == true)
            {
                TempData["puanEkDurum"] = "PUAN BAŞARIYLA EKLENDİ";
                return RedirectToAction("KullaniciSinavDegerlendirme");
            }

            else
            {
                TempData["puanEkDurum"] = "PUAN EKLENEMEDİ";
                return RedirectToAction("KullaniciSinavDegerlendirme");
            }

        }


        // KULLANICININ SINAVA YORUM YAPIP KAYDEDİLDİĞİ KISIM
        [HttpPost, ValidateInput(false)]
        public ActionResult KullaniciSinavDegerlendirmeYorum(string sinavYorum)
        {

            TempData["tarih"] = Session["uyelikTarih"].ToString();

            bool yorumEklenmeDurum = d.kullaniciDeğYorumEkle(Convert.ToInt32(SINAVID), sinavYorum, Session["kullaniciAd"].ToString());
          
            if (yorumEklenmeDurum == true)
            {
                TempData["yorumEkDurum"] = "YORUM BAŞARIYLA EKLENDİ";
                return RedirectToAction("KullaniciSinavDegerlendirme");
            }

            else
            {
                TempData["yorumEkDurum"] = "YORUM EKLENEMEDİ";
                return RedirectToAction("KullaniciSinavDegerlendirme");
            }

        }


        // KULLANICININ PROFİLİNİN DÜZENLENİP GÜNCELLENDİĞİ KISIM
        [HttpPost]
        public ActionResult KullaniciProfilDuzenle(string ad,string soyad,string e_mail,string kullanici_ad,string parola) 
        {
            TempData["tarih"] = Session["uyelikTarih"].ToString();

            gelenSinavlar = d.sistemMevcutSinav();
            gelenKategoriler = d.sistemMevcutKategori();

            if (parola != null)
            {

                bool guncellemeDurum = d.profilGuncelle(Session["kullaniciAd"].ToString(), ad, soyad, e_mail, kullanici_ad, Crypto.Hash(parola,algorithm:"md5"));

                if (guncellemeDurum)
                    ViewBag.guncelleme = "GÜNCELLEME BAŞARIYLA GERÇEKLEŞTİ";

                else
                    ViewBag.guncelleme = "GUNCELLEME GERÇEKLEŞMEDİ";

            }

            else
            {

                bool guncellemeDurum = d.profilGuncelle(Session["kullaniciAd"].ToString(), ad, soyad, e_mail, kullanici_ad,null);

                if (guncellemeDurum)
                    ViewBag.guncelleme = "GÜNCELLEME BAŞARIYLA GERÇEKLEŞTİ";

                else
                    ViewBag.guncelleme = "GUNCELLEME GERÇEKLEŞMEDİ";

            }

            kad = Session["kullaniciAd"].ToString();

            gelenGirisKisi = d.profilGirisKisiGetir(kad);
            gelenKullaniciKisi = d.profilKullaniciKisiGetir(gelenGirisKisi.kid);

            return View(Tuple.Create(gelenKullaniciKisi, gelenGirisKisi));

        }


        // KULLANICI SINAV İSTATİSTİK SAYFASI
        public ActionResult KullaniciSinavIstatistik()
        {
            TempData["tarih"] = Session["uyelikTarih"].ToString();

            kullaniciGirilenSinav = d.kullaniciGirilenSinavlar(Session["kullaniciAd"].ToString());

            ViewBag.gelenSinavlar = kullaniciGirilenSinav;

            return View();
        }


        // KULLANICIYA SINAV İSTATİSTİKLERİNİN GERİ DÖNDERİLDİĞİ KISIM
        [HttpPost]
        public ActionResult KullaniciSinavIstatistik(string sinav)
        {

            if (d.sinavIstatistigiVarMi(Session["kullaniciAd"].ToString()))
            {
                ViewBag.istatistikVarmi = true;

                TempData["tarih"] = Session["uyelikTarih"].ToString();

                kullaniciGirilenSinav = d.kullaniciGirilenSinavlar(Session["kullaniciAd"].ToString());

                ViewBag.gelenSinavlar = kullaniciGirilenSinav;

                sinavIstatistik kullanici = d.kullaniciIstatistigi(Session["kullaniciAd"].ToString(), sinav);

                List<string> basariliKullanici = d.sinavBasariliKullanici(sinav);

                return View(Tuple.Create(kullanici, basariliKullanici));

            }

            else
            {
                TempData["tarih"] = Session["uyelikTarih"].ToString();
                ViewBag.istatistikVarmi = false;
                return View();
            }

        }


        // KULLANICI PROFİL GÜNCELLEME İPTAL ETME KISMI
        public RedirectToRouteResult ProfilGuncellemeIptal()
        {
            return RedirectToAction("KullaniciAnasayfa");
        }

        // KULLANICININ SİSTEMDEN ÇIKIŞ YAPTIĞI YER
        public RedirectToRouteResult KullaniciCikis()
        {
            Session.Remove("kullaniciAd");
            Session.Remove("uyelikTarih");
            return RedirectToAction("KullaniciGiris", "Home");
        }

    }

}