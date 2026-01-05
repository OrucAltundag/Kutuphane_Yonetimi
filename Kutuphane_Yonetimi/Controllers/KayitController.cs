using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kutuphane_Yonetimi.Models.Entity;

namespace Kutuphane_Yonetimi.Controllers
{
    [AllowAnonymous]
    public class KayitController : Controller
    {
        // GET: Kayit
        DBKUTUPHANEEntities db = new DBKUTUPHANEEntities();
        [HttpGet]
        public ActionResult KayitOl()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KayitOl(TBL_UYELER p)
        {
            if(!ModelState.IsValid)
            {                 
                return View(p);
            }
            db.TBL_UYELER.Add(p);
            db.SaveChanges();

            return RedirectToAction("GirisYap", "Login");
        }
    }
}