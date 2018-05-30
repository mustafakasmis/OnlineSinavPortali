using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSinavPortali.Filters;
using OnlineSinavPortali.Models;

namespace OnlineSinavPortali.Controllers
{

    [DegerlendiriciFilter]
    public class DegerlendiriciController : Controller
    {

        private DBIslem db = new DBIslem();


        // SINAVA GİREN KULLANICILARIN VE SINAVLARIN TUTULDUĞU KISIMLAR
        private static List<SelectListItem> kisiler = null;
        private static List<SelectListItem> sinavlar = null;

         
        // KULLANICININ TUTULDUĞU KISIM
        private static string kisi = null;


        // DEĞERLENDİRİCİ ANASAYFA
        public ActionResult DegerlendiriciAnasayfa()
        {
            return View();
        }


        // DEĞERLENDİRİCİ SINAV SONUÇLARINI VE İSTATİSTİKLERİNİ HESAPLAMA SAYFASI
        public ActionResult SinavSonucHesapla()
        {
            kisiler = db.sinavliKisileriGetir();
            ViewBag.Kisiler = kisiler;
            return View();
        }


        // KULLANICININ GİRDİĞİ SINAVLARIN GETİRİLDİĞİ KISIM 
        [HttpPost]
        public ActionResult SinavSonucHesapla(string kullanici)
        {
            ViewBag.Kisiler = kisiler;
            kisi = kullanici;
            sinavlar = db.kisiSinavlariGetir(kullanici);
            ViewBag.Sinavlar = sinavlar;
            return View();
        }
        

        // KULLANICININ SINAV SONUÇLARI VE İSTATİSTİKLERİNİN HESAPLANDIĞI YER
        public ActionResult SinavSonucHesapla2(string sinav)
        {

            ViewBag.Kisiler = kisiler;
            ViewBag.Sinavlar = sinavlar;

            bool durum = db.sinavSonuclariHesapla(kisi, sinav);

            if (durum == true)
            {
                TempData["sonucDurum"]="SINAV SONUCU BAŞARIYLA HESAPLANDI VE KAYDEDİLDİ";
                return View();
            }

            else
            {
                TempData["sonucDurum"] = "SINAV SONUCU HESAPLANAMADI";
                return View();
            }

        }


        // SINAV SONUÇLARI VE İSTATİSTİKLERİNİ HESAPLAMA YÖNERGESİ 
        public ActionResult SinavHesapYonerge()
        {
            return View();
        }

    
        // DEĞERLENDİRİCİNİN SİSTEMDEN ÇIKIŞ YAPTIĞI KISIM
        public RedirectToRouteResult DegerlendiriciCikis()
        {
            Session.Remove("degerlendiriciAd");
            Session.Remove("uyelikTarih");
            return RedirectToAction("DegerlendiriciGiris", "Home");
        }


    }
}