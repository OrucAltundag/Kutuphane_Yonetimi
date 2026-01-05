using Kutuphane_Yonetimi.Models.Entity;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Kutuphane_Yonetimi.Controllers
{
    [AllowAnonymous]
    public class AdminController : Controller
    {
        DBKUTUPHANEEntities db = new DBKUTUPHANEEntities();



        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(TBL_ADMIN p)
        {
            var bilgiler = db.TBL_ADMIN.FirstOrDefault(x => x.KULLANICI == p.KULLANICI && x.SIFRE == p.SIFRE);


            if (bilgiler != null) // Basit bir doğrulama örneği
            {
                FormsAuthentication.SetAuthCookie(bilgiler.KULLANICI, false); // Kullanıcıyı doğrula ve oturum aç
                Session["KULLANICI"] = bilgiler.KULLANICI.ToString(); // Kullanıcı adını oturuma kaydet
                Session["LoginType"] = "ADMIN";
                return RedirectToAction("Index", "Home"); // Başarılı girişte anasayfaya yönlendirme

            }
            ViewBag.Error = "Geçersiz kullanıcı adı veya şifre.";
            return View();
        }

        [Authorize]
        public ActionResult YorumOnay()
        {
            if ((string)Session["LoginType"] != "ADMIN")
                return RedirectToAction("Login");

            var bekleyen = db.TBL_DUYURU_YORUM
                .Where(x => x.ONAY == false)
                .OrderByDescending(x => x.TARIH)
                .ToList();

            return View(bekleyen); // Views/Admin/YorumOnay.cshtml
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult YorumOnayla(int id)
        {
            if ((string)Session["LoginType"] != "ADMIN")
                return RedirectToAction("Login");

            var yorum = db.TBL_DUYURU_YORUM.Find(id);
            if (yorum == null) return HttpNotFound();

            yorum.ONAY = true;
            db.SaveChanges();
            return RedirectToAction("YorumOnay");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult YorumSil(int id)
        {
            if ((string)Session["LoginType"] != "ADMIN")
                return RedirectToAction("Login");

            var yorum = db.TBL_DUYURU_YORUM.Find(id);
            if (yorum == null) return HttpNotFound();

            db.TBL_DUYURU_YORUM.Remove(yorum);
            db.SaveChanges();
            return RedirectToAction("YorumOnay");
        }
    }
}