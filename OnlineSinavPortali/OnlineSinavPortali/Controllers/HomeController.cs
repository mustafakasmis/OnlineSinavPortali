using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSinavPortali.Models;
using System.Web.Helpers;
using System.Net.Mail;
using System.Net;

namespace OnlineSinavPortali.Controllers
{
    public class HomeController : Controller
    {

        private DBIslem db = new DBIslem();


        // ANASAYFA
        public ActionResult Anasayfa()
        {
            return View();
        }

        // KAYIT SAYFASI
        public ActionResult KayitSayfasi()
        {
            return View();
        }


        // KULLANICI EKLEME
        [HttpPost]
        public ActionResult KayitSayfasi(FormCollection frm,string ad,string soyad,string e_mail,string kullanici_ad,
            string parola)
        {

            if (db.kullaniciVarMi(e_mail,kullanici_ad))
            {
                ViewBag.kullaniciVar = "SİSTEME KAYIT OLUNAMADI. GİRİLEN E_MAİL ADRESİ VE KULLANICI ADI SİSTEM TARAFINDA MEVCUTTUR. " +
                    "LÜTFEN FARKLI E_MAİL ADRESİ VE KULLANICI ADI GİRİNİZ";
                return View();
            }

            else
            {
                parola = Crypto.Hash(parola, algorithm: "md5");
                string tarih = DateTime.Now.ToShortDateString();
                string durum = frm["Durumlar"];

                bool sonuc = db.kullaniciEkle(ad, soyad, e_mail, kullanici_ad, parola, durum, tarih);

                if (sonuc)
                    ViewBag.mesaj = "SİSTEME BAŞARIYLA KAYIT OLUNDU";

                return View();
            }

        }


        // KULLANICI GİRİŞ SAYFASI
        public ActionResult KullaniciGiris()
        {
            return View();
        }

        
        // KULLANICILARIN SİSTEME GİRİŞ YAPTIĞI YER
        [HttpPost]
        public ActionResult KullaniciGiris(string kullanici_ad, string parola)
        {
            girisler kisiDurum = db.kullaniciGetir(kullanici_ad, Crypto.Hash(parola,algorithm:"md5"));

            if (kisiDurum != null)
            {

                if (kisiDurum.durum == "Kullanici")
                {
                    Session.Add("kullaniciAd", kisiDurum.kullanici_adi);
                    Session.Add("uyelikTarih", kisiDurum.kullanicilar.uyelik_tarihi);
                    kisiDurum = null;
                    return RedirectToAction("KullaniciAnasayfa", "Kullanici");
                }

                else
                {
                    kisiDurum = null;
                    Response.Write("<script>HATALI KULLANICI ADI VEYA PAROLA VEYA YETKİSİZ ERİŞİM</script>");
                    return RedirectToAction("KullaniciGiris", "Home");
                }
            }

            else{
                Response.Write("<script>HATALI KULLANICI ADI VEYA PAROLA VEYA YETKİSİZ ERİŞİM</script>");
                return RedirectToAction("KullaniciGiris", "Home");
            }
           
        }


        // AKADEMİSYEN GİRİŞ SAYFASI
        public ActionResult AkademisyenGiris()
        {
            return View();
        }


        // AKADEMİSYENİN SİSTEME GİRİŞ YAPTIĞI YER
        [HttpPost]
        public ActionResult AkademisyenGiris(string kullanici_ad,string parola)
        {
            girisler kisiDurum = db.kullaniciGetir(kullanici_ad, Crypto.Hash(parola, algorithm: "md5"));

            if (kisiDurum != null)
            {
                if (kisiDurum.durum == "Akademisyen")
                {
                    Session.Add("akademisyenAd", kisiDurum.kullanici_adi);
                    Session.Add("uyelikTarih", kisiDurum.kullanicilar.uyelik_tarihi);

                    HttpCookie ck = new HttpCookie("kullaniciIsim", kisiDurum.kullanici_adi);

                    ck.Expires = DateTime.Now.AddDays(1);

                    Response.Cookies.Add(ck);

                    kisiDurum = null;
                    return RedirectToAction("AkademisyenAnasayfa", "Akademisyen");
                }

                else
                {
                    kisiDurum = null;
                    Response.Write("<script>HATALI KULLANICI ADI VEYA PAROLA VEYA YETKİSİZ ERİŞİM</script>");
                    return RedirectToAction("AkademisyenGiris", "Home");
                }
            }

            else
            {
                Response.Write("<script>HATALI KULLANICI ADI VEYA PAROLA VEYA YETKİSİZ ERİŞİM</script>");
                return RedirectToAction("AkademisyenGiris", "Home");
            }
            
        }


        // DEĞERLENDİRİCİ GİRİŞ SAYFASI
        public ActionResult DegerlendiriciGiris()
        {
            return View();
        }


        // DEĞERLENDİRİCİNİN SİSTEME GİRİŞ YAPTIĞI YER
        [HttpPost]
        public ActionResult DegerlendiriciGiris(string kullanici_ad,string parola)
        {
            girisler kisiDurum = db.kullaniciGetir(kullanici_ad, Crypto.Hash(parola, algorithm: "md5"));

            if (kisiDurum != null)
            {
                if (kisiDurum.durum == "Degerlendirici")
                {
                    Session.Add("degerlendiriciAd", kisiDurum.kullanici_adi);
                    Session.Add("uyelikTarih", kisiDurum.kullanicilar.uyelik_tarihi);
                    kisiDurum = null;
                    return RedirectToAction("DegerlendiriciAnasayfa", "Degerlendirici");
                }

                else
                {
                    kisiDurum = null;
                    Response.Write("<script>HATALI KULLANICI ADI VEYA PAROLA VEYA YETKİSİZ ERİŞİM</script>");
                    return RedirectToAction("DegerlendiriciGiris", "Home");
                }
            }

            else
            {
                Response.Write("<script>HATALI KULLANICI ADI VEYA PAROLA VEYA YETKİSİZ ERİŞİM</script>");
                return RedirectToAction("DegerlendiriciGiris", "Home");
            }

        }


        // PAROLAMI UNUTTUM SAYFASI
        public ActionResult ParolamiUnuttum()
        {
            return View();
        }


        // PAROLA SIFIRLAMA İŞLEMİ YAPILMAKTADIR
        [HttpPost]
        public ActionResult ParolamiUnuttum(string yeniParola,string yeniParolaDogrula,string e_mail)
        {

            if (yeniParola.Equals(yeniParolaDogrula))
            {

                string parolaSifreli = Crypto.Hash(yeniParola, algorithm: "md5");

                bool sifirla = db.parolaSifirla(parolaSifreli, e_mail);

                if (sifirla)
                    ViewBag.sifirlamaDurum = "PAROLA BAŞARIYLA SIFIRLANDI";

                else
                    ViewBag.sifirlamaDurum = "PAROLA SIFIRLAMA BAŞARISIZ!";

                return View();
            }

            else
            {
                ViewBag.sifirlamaDurum = "PAROLALAR EŞLEŞMİYOR.LÜTFEN TEKRAR DENEYİNİZ";
                return View();
            }

            
        }


        // HAKKIMIZDA SAYFASI
        public ActionResult Hakkimizda()
        {
            return View();
        }


        // İLETİŞİM SAYFASI
        public ActionResult Iletisim()
        {
            return View();
        }


        // İLETİŞİM SAYFASINDAN MAİL GÖNDERİMİNİN YAPILDIĞI YER
        [HttpPost,ValidateInput(false)]
        public ActionResult Iletisim(string ad,string soyad,string e_mail,string mesaj)
        {

            var fromAddress = new MailAddress(e_mail);
            var toAddress = new MailAddress("abc@gmail.com");
            const string subject = "Site Adı | Sitenizden Gelen İletişim Formu";

            using (var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "12345678")
            })
            {
                using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = mesaj })
                {
                    smtp.Send(message);
                }
            }

            return View();
        }

    }
}
