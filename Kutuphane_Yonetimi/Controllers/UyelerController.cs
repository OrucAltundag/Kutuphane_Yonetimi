using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kutuphane_Yonetimi.Models.Entity; // Assuming this is the correct namespace for your models

namespace Kutuphane_Yonetimi.Controllers
{
    
    public class UyelerController : Controller
    {
        // GET: Uyeler
        DBKUTUPHANEEntities db = new DBKUTUPHANEEntities();
        public ActionResult Index(string secim, int? kitapId, string kitapAd, int? uyeId, string uyeAd)
        {
            ViewBag.SecimModu = secim; // "uye" gelirse view'da kontrol edeceğiz


            // Önceden seçilen değerleri de taşı
            ViewBag.SecimModu = secim;
            ViewBag.SecilenKitapId = kitapId;
            ViewBag.SecilenKitapAd = kitapAd;
            ViewBag.SecilenUyeId = uyeId;
            ViewBag.SecilenUyeAd = uyeAd;



            var degerler = db.TBL_UYELER.ToList();
            return View(degerler);
        }


        [HttpGet]
        public ActionResult UyeEkle()
        {
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UyeEkle(TBL_UYELER p)
        {
            if (!ModelState.IsValid)
            {
                return View("UyeEkle");
            }

            db.TBL_UYELER.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult UyeSil(int id)
        {
            var uye = db.TBL_UYELER.Find(id);
            db.TBL_UYELER.Remove(uye);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UyeGetir(int id)
        {
            var uye = db.TBL_UYELER.Find(id);
            
            return View("UyeGetir", uye);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UyeGuncelle(TBL_UYELER p)
        {


            // Önce Validasyon kontrolü (Form kurallara uygun mu?)
            if (!ModelState.IsValid)
            {
                return View("UyeGetir", p); // Hata varsa sayfayı geri döndür
            }

            var uye = db.TBL_UYELER.Find(p.ID);

            // Verileri güncelle
            uye.AD = p.AD;
            uye.SOYAD = p.SOYAD;
            uye.MAIL = p.MAIL;
            uye.KULLANICIADI = p.KULLANICIADI;
            uye.FOTOGRAF = p.FOTOGRAF;
            uye.TELEFON = p.TELEFON;
            uye.OKUL = p.OKUL;

            

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UyeKitapGecmis(int id)
        {
            var uye = db.TBL_UYELER.Find(id);
            ViewBag.UyeAdSoyad = uye.AD + " " + uye.SOYAD;
            var kitaplar = db.TBL_HAREKET.Where(x => x.UYE == id).ToList();
            return View(kitaplar);
        }


    }
}