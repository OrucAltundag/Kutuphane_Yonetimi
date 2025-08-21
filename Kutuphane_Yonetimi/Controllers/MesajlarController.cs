using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kutuphane_Yonetimi.Models.Entity;


namespace Kutuphane_Yonetimi.Controllers
{
 
    public class MesajlarController : Controller
    {
        // GET: Mesajlar
        DBKUTUPHANEEntities db = new DBKUTUPHANEEntities();

        public ActionResult Index()
        {
            int uyeId = int.Parse(Session["ID"].ToString());
            var uye = db.TBL_UYELER.Find(uyeId);
            var mesajlar = db.TBL_MESAJLAR.Where(x => x.ALICI == uye.MAIL.ToString()).ToList();
            return View(mesajlar);
        }

        public ActionResult GidenMesajlar()
        {
            int uyeId = int.Parse(Session["ID"].ToString());
            var uye = db.TBL_UYELER.Find(uyeId);
            var mesajlar = db.TBL_MESAJLAR.Where(x => x.GONDEREN == uye.MAIL.ToString()).ToList();
            return View(mesajlar);
        }


        [HttpGet]
        public ActionResult YeniMesaj()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniMesaj(TBL_MESAJLAR P)
        {
            int uyeId = int.Parse(Session["ID"].ToString());
            var uye = db.TBL_UYELER.Find(uyeId);
            P.GONDEREN = uye.MAIL;
            P.TARIH = DateTime.Parse(DateTime.Now.ToShortDateString());
            db.TBL_MESAJLAR.Add(P);
            db.SaveChanges();

            return RedirectToAction("GidenMesajlar","Mesajlar");
        }

        public PartialViewResult Partial1()
        {
            int uyeId = int.Parse(Session["ID"].ToString());
            var uye = db.TBL_UYELER.Find(uyeId);
            ViewBag.GelenSayisi = db.TBL_MESAJLAR.Where(x => x.ALICI == uye.MAIL).Count();
            ViewBag.GidenSayisi = db.TBL_MESAJLAR.Where(x => x.GONDEREN == uye.MAIL).Count();
            return PartialView();
        }

    }
}