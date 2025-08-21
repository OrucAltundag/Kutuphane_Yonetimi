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


            if (bilgiler != null ) // Basit bir doğrulama örneği
            {
                FormsAuthentication.SetAuthCookie(bilgiler.KULLANICI, false); // Kullanıcıyı doğrula ve oturum aç
                Session["KULLANICI"] = bilgiler.KULLANICI.ToString(); // Kullanıcı adını oturuma kaydet
                return RedirectToAction("Index", "Home"); // Başarılı girişte anasayfaya yönlendirme
            }
            ViewBag.Error = "Geçersiz kullanıcı adı veya şifre.";
            return View();
        }
    }
}