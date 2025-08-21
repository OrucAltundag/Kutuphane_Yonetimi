using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kutuphane_Yonetimi.Models.Entity;

namespace Kutuphane_Yonetimi.Controllers
{
   
    public class OduncController : Controller
    {
        DBKUTUPHANEEntities db = new DBKUTUPHANEEntities();
        // GET: Odunc
        public ActionResult Index()
        {
            var degerler = db.TBL_HAREKET.Where(X=>X.IslemDurumu==false).ToList();
            return View(degerler);
        }
        [HttpGet]
        public ActionResult OduncVer(int? kitapId, string kitapAd, int? uyeId, string uyeAd)
        {
            List<SelectListItem> deger1 = (from x in db.TBL_PERSONEL.ToList()
                                          select new SelectListItem
                                          {
                                              Text = x.PERSONEL,
                                              Value = x.ID.ToString()
                                          }).ToList();
            ViewBag.dgr1 = deger1;

            if (kitapId.HasValue)
            {
                ViewBag.SecilenKitapId = kitapId.Value;
                ViewBag.SecilenKitapAd = kitapAd;
            }

            if (uyeId.HasValue)
            {
                ViewBag.SecilenUyeId = uyeId.Value;
                ViewBag.SecilenUyeAd = uyeAd;
            }

            return View();
        }

        [HttpPost]
        public ActionResult OduncVer(TBL_HAREKET h)
        {
            var d = db.TBL_PERSONEL.Where(x => x.ID == h.TBL_PERSONEL.ID).FirstOrDefault();
            h.TBL_PERSONEL = d;
            db.TBL_HAREKET.Add(h);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult OduncIade(TBL_HAREKET p)
        {
            var odn = db.TBL_HAREKET.Find(p.ID);

            DateTime d1 = Convert.ToDateTime(odn.IADETARIH);
            DateTime d2 = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            TimeSpan d3 = d2 - d1;

            ViewBag.fark = d3.TotalDays;

            return View("OduncIade", odn);
        }

        public ActionResult OduncGuncelle(TBL_HAREKET p)
        {
           

            var hrk = db.TBL_HAREKET.Find(p.ID); // Güncellenecek
            hrk.UyeGetirTarih = p.UyeGetirTarih;    
            hrk.IslemDurumu = true;               // İade işlemi tamamlandı
            db.SaveChanges();                              // Değişiklikleri kaydet
            return RedirectToAction("Index");              // Güncel
        }














    }
}