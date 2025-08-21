using Kutuphane_Yonetimi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace Kutuphane_Yonetimi.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // GET: Login
        DBKUTUPHANEEntities db = new DBKUTUPHANEEntities();
        [HttpGet]
        public ActionResult GirisYap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GirisYap(TBL_UYELER p)
        {
            var bilgiler = db.TBL_UYELER.FirstOrDefault(x =>
                (x.KULLANICIADI == p.KULLANICIADI || x.MAIL == p.KULLANICIADI)  // kullanıcı adı yerine mail de yazılabilir
                && x.SIFRE == p.SIFRE);

            if (bilgiler != null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.KULLANICIADI, false);
                Session["KULLANICIADI"] = bilgiler.KULLANICIADI.ToString();
                Session["ID"] = bilgiler.ID.ToString();

                if (bilgiler.KULLANICIADI == p.KULLANICIADI)
                    Session["LoginType"] = "KULLANICIADI";
                else
                    Session["LoginType"] = "MAIL";

                return RedirectToAction("Index", "Panel");
            }
            else
            {
                ViewBag.hata = "Kullanıcı Adı / Mail veya Şifre Hatalı";
                return View();
            }
        }

    }
}