using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using OnlineSinavPortali.Models;
using OnlineSinavPortali.Filters;

namespace OnlineSinavPortali.Controllers
{

    [AkademisyenFilter]
    public class AkademisyenController : Controller
    {

        private DBIslem db = new DBIslem();


        // KATEGORİLER VE SINAVLARIN TUTULDUĞU KISIM
        private static List<SelectListItem> anaKategoriler = new List<SelectListItem>();
        private static List<SelectListItem> gstKategoriler = new List<SelectListItem>();
        private static List<SelectListItem> butunsinavlar = new List<SelectListItem>();


        // KİŞİLER VE GİRDİKLERİ SINAVLARIN TUTULDUĞU KISIM
        private static List<girisler> kisiler = null;
        private static List<sinavIstatistik> kisiSinavIs = null;

        // KİŞİLERİN VE SINAV YORUMLARININ TUTULDUĞU KISIM
        private static List<girisler> kisiYorum = null;
        private static List<degerlendirme> kisiSinavYorum = null;

        // YORUMLARIN TUTULDUĞU KISIM
        private static List<string> sinavYorumlar = new List<string>();

        
        // AKADEMİSYEN ANASAYFA
        public ActionResult AkademisyenAnasayfa()
        {
            gstKategoriler = db.tumKategorilerGetir();
            anaKategoriler = db.anaKategorileriGetir();
            butunsinavlar = db.sinavlariGetir();
            return View();
        }


        // KATEGORİ EKLEME SAYFASI
        public ActionResult KategoriEkle()
        {
          
            ViewBag.tumListe = gstKategoriler;

            return View();
        }


        // KATEGORİNİN EKLENDİĞİ KISIM
        [HttpPost]
        public ActionResult KategoriEkle(string kategoriAdi,string TumKategoriler)
        {
            bool sonuc = db.kategoriEkle(kategoriAdi, TumKategoriler);

            if (sonuc)
                ViewBag.txt = "KAYIT BAŞARIYLA EKLENDİ";

            ViewBag.tumListe = gstKategoriler;

            return View();
        }


        // KATEGORİLERİN LİSTELENDİĞİ KISIM
        public ActionResult KategoriListele()
        {

            ViewBag.tumListe = anaKategoriler;

            return View();

        }


        // BU KISIM SEÇİLEN KATEGORİNİN ALT KATEGORİLERİNİ GETİRME KISMI
        [HttpPost]
        public ActionResult KategoriListele(string Kategoriler)
        {
            List<kategoriler> gelenler = db.kategorileriCek(Kategoriler);

            ViewBag.tumListe = anaKategoriler;

            return View(gelenler.ToList());
        }

        
        // KATEGORİ GÜNCELLEME SAYFASI
        public ActionResult KategoriGuncelle()
        {
           
            ViewBag.tumListe = gstKategoriler;

            return View();
        }


        // KATEGORİNİN GÜNCELLENDİĞİ KISIM
        [HttpPost]
        public ActionResult KategoriGuncelle(string TumKategorilerGuncel, string kategoriAdi, string TumUstKategorilerGuncel)
        {
            bool sonuc = db.kategoriGuncelle(TumKategorilerGuncel, kategoriAdi, TumUstKategorilerGuncel);

            if (sonuc)
                ViewBag.txt = "KATEGORİ BAŞARIYLA GÜNCELLENDİ";


            ViewBag.tumListe = gstKategoriler;

            return View();
        }


        // KATEGORİ SİLME SAYFASI
        public ActionResult KategoriSil()
        {
            ViewBag.tumListe = gstKategoriler;
            return View();
        }


        // KATEGORİNİN VE ALT KATEGORİLERİNİN SİLİNDİĞİ KISIM
        [HttpPost]
        public ActionResult KategoriSil(string TumKategoriler)
        {
            bool sonuc = db.kategoriSil(TumKategoriler);

            if (sonuc)
                ViewBag.txt = "KATEGORİ BAŞARI İLE SİLİNDİ";

            ViewBag.tumListe = gstKategoriler;

            return View();
        }


        // SORU İŞLEMLERİ KISMI 
        public RedirectToRouteResult SoruIslemleri()
        {
            return RedirectToAction("SoruIslemleriAnasayfa", "Akademisyen2");
        }


        // SINAV İSTATİSTİKLERİ SAYFASI
        public ActionResult SinavIstatistikleri()
        {
            ViewBag.tumSinavlar = butunsinavlar;
            return View();
        }


        // SINAV İSTATİSTİKLERİNİN GERİ DÖNDERİLDİĞİ KISIM
        [HttpPost]
        public ActionResult SinavIstatistikleri(FormCollection frm)
        {
            string kategori = frm["Sinavlar"];

           db.akademisyenSinavIstatistikDon(kategori);

            kisiler = db.kisiler;
            kisiSinavIs = db.kisiSinavIs;

            ViewBag.tumSinavlar = butunsinavlar;

            return View(Tuple.Create(kisiler,kisiSinavIs));
        }


        // SINAV YORUMLARI SAYFASI
        public ActionResult SinavYorumlari()
        {
            ViewBag.sinavYorum = butunsinavlar;
            return View();
        }


        // SINAV YORUMLARININ GERİ DÖNDERİLDİĞİ KISIM
        [HttpPost]
        public ActionResult SinavYorumlari(string sinav)
        {
            ViewBag.sinavYorum = butunsinavlar;

            db.akademisyenSinavYorumDon(sinav);

            kisiYorum = db.yorumluKisiler;
            kisiSinavYorum = db.kisiSinavYorumlar;

            return View(Tuple.Create(kisiYorum,kisiSinavYorum));
        }


        // HERHANGİ BİR İŞLEMİ İPTAL ETME KISMI
        public RedirectToRouteResult Iptal()
        {
            return RedirectToAction("AkademisyenAnasayfa", "Akademisyen");
        }


        // AKADEMİSYENİN SİSTEMDEN ÇIKIŞ YAPTIĞI YER
        public RedirectToRouteResult AkademisyenCikis()
        {
            Session.Remove("akademisyenAd");
            Session.Remove("uyelikTarih");
            return RedirectToAction("AkademisyenGiris", "Home");
        }

    }
}