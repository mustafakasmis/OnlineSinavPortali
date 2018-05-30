using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSinavPortali.Models;
using System.IO;
using OnlineSinavPortali.Filters;

namespace OnlineSinavPortali.Controllers
{

    [AkademisyenFilter]
    public class Akademisyen2Controller : Controller
    {

        private DBIslem db = new DBIslem();


        // SINAVLARIN TUTULDUĞU KISIM
        private static List<SelectListItem> sinavSoruCevap = new List<SelectListItem>();


        // SORU VE CEVAP İŞLEMLERİ ANASAYFA
        public ActionResult SoruCevapIslemleriAnasayfa()
        {
            
            sinavSoruCevap = db.sinavlariGetir();
            return View();
            
        }


        // SORU VE CEVAP EKLEME SAYFASI
        public ActionResult SoruCevapEkle()
        {
            return View();
        }


        // İLGİLİ KATEGORİYE SORU SECENEK VS LERİN EKLENDİĞİ KISIM
        [HttpPost,ValidateInput(false)]
        public ActionResult SoruCevapEkle(string kategoriAdi,string soru,string soruSeviye,string secenek1,string secenek2,
            string secenek3,string secenek4, string cevap, HttpPostedFileBase soruResim,
            HttpPostedFileBase cevapDokuman)
        {

            if(soruResim==null && cevapDokuman == null)
            {
                bool durum=db.sorucevapEkle(kategoriAdi,soru,soruSeviye,secenek1,secenek2,secenek3,secenek4,cevap,null,null);

                if (durum)
                    ViewBag.txt = "BAŞARIYLA EKLENDİ";
            }

            else if (soruResim == null && cevapDokuman != null)
            {

                string dosyaAdi = Path.GetFileName(cevapDokuman.FileName);
                string yol = Path.Combine(Server.MapPath("~/files/cevapDokumanlar"), dosyaAdi);
                string yol2 = "/files/cevapDokumanlar/"+ dosyaAdi;
                cevapDokuman.SaveAs(yol);

         bool durum = db.sorucevapEkle(kategoriAdi, soru, soruSeviye, secenek1, secenek2, secenek3, secenek4, cevap, null, yol2);

                if (durum)
                    ViewBag.txt = "BAŞARIYLA EKLENDİ";

            }

            else if (soruResim != null && cevapDokuman == null)
            {

                string dosyaAdi = Path.GetFileName(soruResim.FileName);
                string yol = Path.Combine(Server.MapPath("~/images/soruResim"), dosyaAdi);
                string yol2 = "/images/soruResim/"+ dosyaAdi;
                soruResim.SaveAs(yol);

                bool durum = db.sorucevapEkle(kategoriAdi, soru, soruSeviye, secenek1, secenek2, secenek3, secenek4, cevap, yol2, null);

                if (durum)
                    ViewBag.txt = "BAŞARIYLA EKLENDİ";

            }

            else
            {

                string dosyaAdi = Path.GetFileName(soruResim.FileName);
                string yol = Path.Combine(Server.MapPath("~/images/soruResim"), dosyaAdi);
                string yol2 = "/images/soruResim/"+ dosyaAdi;
                soruResim.SaveAs(yol);

                string dosyaAdi2 = Path.GetFileName(cevapDokuman.FileName);
                string yolcevap = Path.Combine(Server.MapPath("~/files/cevapDokumanlar"), dosyaAdi2);
                string yolcevap2 = "/files/cevapDokumanlar/"+ dosyaAdi;
                cevapDokuman.SaveAs(yolcevap);

                bool durum = db.sorucevapEkle(kategoriAdi, soru, soruSeviye, secenek1, secenek2, secenek3, secenek4, cevap, yol2, yolcevap2);

                if (durum)
                    ViewBag.txt = "BAŞARIYLA EKLENDİ";

            }

            return View();

        }


        // SORU VE CEVAP LİSTELEME SAYFASI
        public ActionResult SoruCevapListele()
        {
            ViewBag.sinavsorucevap = sinavSoruCevap;
            return View();
        }


        // İLGİLİ KATEGORİYE AİT SORU SECENEK VS LERİN GETİRİLDİĞİ KISIM
        [HttpPost]
        public ActionResult SoruCevapListele(string Sinavlar)
        {
            db.sorucevapListele(Sinavlar);
            ViewBag.sinavsorucevap = sinavSoruCevap;
            return View(Tuple.Create(db.Sorular,db.Secenekler,db.Cevaplar,db.Resimler));
        }


        // İLGİLİ KAYITA AİT SORU SECENEK VS LERİN GÜNCELLENDİĞİ KISIM
        [HttpPost]
        public ActionResult SoruCevapGuncelleVt(string kategoriAdi,string yeniKategori,string soru, string soruSeviye,
            string secenek1,string secenek2,string secenek3,string secenek4,string cevap,HttpPostedFileBase soruResim,
            HttpPostedFileBase cevapDokuman)
        {

            if (soruResim == null && cevapDokuman == null)
            {
                bool durum = db.sorucevapGuncelle(kategoriAdi, yeniKategori, soru, soruSeviye, secenek1, secenek2, secenek3, secenek4, cevap, null, null);

                if (durum)
                    ViewBag.guncellemeDurum = "BAŞARIYLA GUNCELLENDİ";
            }

            else if (soruResim == null && cevapDokuman != null)
            {

                string dosyaAdi = Path.GetFileName(cevapDokuman.FileName);
                string yol = Path.Combine(Server.MapPath("~/files/cevapDokumanlar"), dosyaAdi);
                string yol2 = "/files/cevapDokumanlar/"+ dosyaAdi;
                cevapDokuman.SaveAs(yol);

                bool durum = db.sorucevapGuncelle(kategoriAdi,yeniKategori, soru, soruSeviye, secenek1, secenek2, secenek3, secenek4, cevap, null, yol2);

                if (durum)
                    ViewBag.guncellemeDurum = "BAŞARIYLA GUNCELLENDİ";

            }

            else if (soruResim != null && cevapDokuman == null)
            {

                string dosyaAdi = Path.GetFileName(soruResim.FileName);
                string yol = Path.Combine(Server.MapPath("~/images/soruResim"), dosyaAdi);
                string yol2 = "/images/soruResim/"+ dosyaAdi;
                soruResim.SaveAs(yol);

                bool durum = db.sorucevapGuncelle(kategoriAdi,yeniKategori, soru, soruSeviye, secenek1, secenek2, secenek3, secenek4, cevap, yol2, null);

                if (durum)
                    ViewBag.guncellemeDurum = "BAŞARIYLA GUNCELLENDİ";

            }

            else
            {

                string dosyaAdi = Path.GetFileName(soruResim.FileName);
                string yol = Path.Combine(Server.MapPath("~/images/soruResim"), dosyaAdi);
                string soruyol = "/images/soruResim/"+ dosyaAdi;
                soruResim.SaveAs(yol);

                string dosyaAdi2 = Path.GetFileName(cevapDokuman.FileName);
                string yol2 = Path.Combine(Server.MapPath("~/files/cevapDokumanlar"), dosyaAdi);
                string cevapyol2 = "/files/cevapDokumanlar/"+ dosyaAdi;
                cevapDokuman.SaveAs(yol2);

                bool durum = db.sorucevapGuncelle(kategoriAdi,yeniKategori, soru, soruSeviye, secenek1, secenek2, secenek3, secenek4, cevap, soruyol, cevapyol2);

                if (durum)
                    ViewBag.guncellemeDurum = "BAŞARIYLA GUNCELLENDİ";

            }

           
            return RedirectToAction("SoruCevapListele", "Akademisyen2");
            
        }


        // İLGİLİ KAYITA AİT SORU SECENEK VS LERİN FORMA DOLDURULDUĞU KISIM
        [HttpPost,ValidateInput(false)]
        public ActionResult SoruCevapGuncelle( string katSorId)
        {
            db.soruCevapGuncelleForm(katSorId);
            ViewBag.anlikKategori = db.kat;
            ViewData["prtKat"]=db.prtKat;
            return View(Tuple.Create(db.s, db.scnk, db.c, db.r));
          
        }


        // İLGİLİ SORU VE CEVABIN SİLİNDİĞİ KISIM
        [HttpPost]
        public ActionResult SoruCevapSil(string katSorId)
        {

            bool durum = db.soruCevapSil(katSorId);


            if (durum)
                ViewBag.silinmeDurum = "KAYIT BAŞARIYLA SİLİNDİ";
            else
                ViewBag.silinmeDurum = "KAYIT SİLME İŞLEMİ BAŞARISIZ";

            return RedirectToAction("SoruCevapListele","Akademisyen2");

        }


        // HERHANGİ BİR İŞLEMİ İPTAL ETME KISMI
        public RedirectToRouteResult IptalAnasayfa()
        {
            return RedirectToAction("AkademisyenAnasayfa", "Akademisyen");
        }


    }

}