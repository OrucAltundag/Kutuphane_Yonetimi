using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kutuphane_Yonetimi.Models.Entity;

namespace Kutuphane_Yonetimi.Controllers
{
    
    public class KitapController : Controller
    {
        DBKUTUPHANEEntities db = new DBKUTUPHANEEntities();
        // GET: Kitap
        public ActionResult Index(string secim, int? kitapId, string kitapAd, int? uyeId, string uyeAd)
        {
            ViewBag.SecimModu = secim; // "kitap" gelirse view'da kontrol edeceğiz

            ViewBag.SecilenKitapId = kitapId;
            ViewBag.SecilenKitapAd = kitapAd;
            ViewBag.SecilenUyeId = uyeId;
            ViewBag.SecilenUyeAd = uyeAd;

            var kitaplar = db.TBL_KITAP.ToList();
            return View(kitaplar);
        }

        [HttpGet]
        public ActionResult KitapEkle()
        {
            List<SelectListItem> degerler = (from x in db.TBL_KATEGORI.ToList()
                                               select new SelectListItem
                                               {
                                                   Text = x.AD,
                                                   Value = x.ID.ToString()
                                               }).ToList();
            ViewBag.dgr = degerler;

            List<SelectListItem> degerlerYazar = (from y in db.TBL_YAZAR.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = y.AD + " " + y.SOYAD,
                                                 Value = y.ID.ToString()
                                             }).ToList();
            ViewBag.dgry = degerlerYazar;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KitapEkle(TBL_KITAP p)
        {

            if (!ModelState.IsValid)
            {
                return View(p);
            }



            db.TBL_KITAP.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
        public ActionResult KitapSil(int id)
        {
            var kitap = db.TBL_KITAP.Find(id);
            db.TBL_KITAP.Remove(kitap);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult KitapGetir(int id)
        {
            var kitap = db.TBL_KITAP.Find(id);
            List<SelectListItem> degerler = (from x in db.TBL_KATEGORI.ToList() 
                                             select new SelectListItem
                                             {
                                                 Text = x.AD,
                                                 Value = x.ID.ToString()
                                             }).ToList();
            ViewBag.dgr = degerler;

            List<SelectListItem> degerlerYazar = (from y in db.TBL_YAZAR.ToList()
                                                  select new SelectListItem
                                                  {
                                                      Text = y.AD + " " + y.SOYAD,
                                                      Value = y.ID.ToString()
                                                  }).ToList();
            ViewBag.dgry = degerlerYazar;
            return View("KitapGetir", kitap);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KitapGuncelle(TBL_KITAP p)
        {
            if (!ModelState.IsValid)
            {
               
                return View(p);
            }


            var kitap = db.TBL_KITAP.Find(p.ID);  
            kitap.AD = p.AD;
            kitap.BASIMYIL = p.BASIMYIL;
            kitap.DURUM = p.DURUM;
            kitap.SAYFA = p.SAYFA;
            kitap.YAYINEVI = p.YAYINEVI;
            //var ktg = db.TBL_KATEGORI.Where(x => x.ID == p.TBL_KATEGORI.ID).FirstOrDefault();
            //var yzr = db.TBL_YAZAR.Where(x => x.ID == p.TBL_YAZAR.ID).FirstOrDefault();
            //kitap.TBL_KATEGORI = ktg;
            //kitap.TBL_YAZAR = yzr;

            kitap.KATEGORI = (byte)p.KATEGORI;  // int FK
            kitap.YAZAR = p.YAZAR;     // int FK

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}