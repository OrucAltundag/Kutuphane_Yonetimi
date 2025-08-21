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
        public ActionResult Index()
        {
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


            var uye = db.TBL_UYELER.Find(p.ID);
            uye.AD = p.AD;
            uye.SOYAD = p.SOYAD;
            uye.MAIL = p.MAIL;
            uye.KULLANICIADI = p.KULLANICIADI;
            uye.SIFRE = p.SIFRE;
            uye.FOTOGRAF = p.FOTOGRAF;  
            uye.TELEFON = p.TELEFON;    
            uye.OKUL= p.OKUL;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}