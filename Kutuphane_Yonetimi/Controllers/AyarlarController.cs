using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kutuphane_Yonetimi.Models.Entity;

namespace Kutuphane_Yonetimi.Controllers
{
    [AllowAnonymous]
    public class AyarlarController : Controller
    {
        DBKUTUPHANEEntities db = new DBKUTUPHANEEntities();
        // GET: Ayarlar
        public ActionResult Index()
        {
            var kullanicilar = db.TBL_ADMIN.ToList();
            return View(kullanicilar);
        }
        [HttpGet]
        public ActionResult YeniAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniAdmin(TBL_ADMIN p)
        {
            if (!ModelState.IsValid)
            {
                return View("YeniAdmin");
            }
            db.TBL_ADMIN.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AdminSil(int id)
        {
            var kullanici = db.TBL_ADMIN.Find(id);
            db.TBL_ADMIN.Remove(kullanici);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AdminGuncelle(int id)
        {
            var kullanici = db.TBL_ADMIN.Find(id);
            return View("AdminGuncelle", kullanici);
        }
        [HttpPost]
        public ActionResult AdminGuncelle(TBL_ADMIN p)
        {
            var kullanici = db.TBL_ADMIN.Find(p.ID);
            kullanici.KULLANICI = p.KULLANICI;
            kullanici.SIFRE = p.SIFRE;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}