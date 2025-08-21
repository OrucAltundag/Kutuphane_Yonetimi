using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kutuphane_Yonetimi.Models.Entity;

namespace Kutuphane_Yonetimi.Controllers
{
    
    public class DuyuruController : Controller
    {
        // GET: Duyuru
        DBKUTUPHANEEntities db = new DBKUTUPHANEEntities();
        public ActionResult Index()
        {
            var duyurular = db.TBL_DUYURULAR.ToList();
            return View(duyurular);

           
        }
        [HttpGet]
        public ActionResult YeniDuyuru()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniDuyuru(TBL_DUYURULAR p)
        {
            db.TBL_DUYURULAR.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DuyuruSil(int id)
        {
            var duyuru = db.TBL_DUYURULAR.Find(id);
            db.TBL_DUYURULAR.Remove(duyuru);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DuyuruGuncelle(TBL_DUYURULAR p)
        {
            // Güncellenecek duyuruyu bul
            var duyuru = db.TBL_DUYURULAR.Find(p.ID);
            if (duyuru != null)
            {
                duyuru.KATEGORI = p.KATEGORI;
                duyuru.ICERIK = p.ICERIK;

                db.SaveChanges();
            }

            // Güncellemeden sonra tekrar listeye dön
            return RedirectToAction("Index");
        }
    }
}