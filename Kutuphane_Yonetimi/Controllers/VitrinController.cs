using Kutuphane_Yonetimi.Models.Entity;
using Kutuphane_Yonetimi.Models.Siniflarim;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kutuphane_Yonetimi.Controllers
{
    [AllowAnonymous]
    public class VitrinController : Controller
    {
        DBKUTUPHANEEntities db = new DBKUTUPHANEEntities();

        // Anasayfa
        [HttpGet]
        public ActionResult Index()
        {
            Class1 cs = new Class1();

            var kitaplar = db.TBL_KITAP.ToList();
            cs.deger1 = kitaplar.Where(k => UrlCheck(k.KITAPRESIM)).Take(6).ToList();
            cs.deger2 = db.TBL_HAKKIMIZDA.ToList();

            return View(cs);
        }

        [HttpPost]
        public ActionResult Index(TBL_ILETISIM t)
        {
            db.TBL_ILETISIM.Add(t);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Galeri Sayfası
        public ActionResult Galeri()
        {
            string klasorYolu = Server.MapPath("~/web2/resimler/");
            if (!Directory.Exists(klasorYolu))
            {
                Directory.CreateDirectory(klasorYolu);
            }

            var resimler = Directory.GetFiles(klasorYolu)
                                    .Select(Path.GetFileName)
                                    .ToList();

            ViewBag.Resimler = resimler;
            return View();
        }

        // Resim Yükleme
        [HttpPost]
        public ActionResult ResimYukle(HttpPostedFileBase dosya)
        {
            if (dosya != null && dosya.ContentLength > 0)
            {
                string klasorYolu = Server.MapPath("~/web2/resimler/");
                if (!Directory.Exists(klasorYolu))
                {
                    Directory.CreateDirectory(klasorYolu);
                }

                string dosyaAdi = Path.GetFileName(dosya.FileName);
                string yol = Path.Combine(klasorYolu, dosyaAdi);
                dosya.SaveAs(yol);

                TempData["Uyari"] = "✅ Resim başarıyla yüklendi: " + dosyaAdi;
            }
            else
            {
                TempData["Uyari"] = "⚠️ Resim seçilmedi veya yüklenemedi.";
            }

            return RedirectToAction("Galeri");
        }

        // Link kontrolü
        private bool UrlCheck(string url)
        {
            try
            {
                var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = 3000; // 3 sn
                using (var response = (System.Net.HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == System.Net.HttpStatusCode.OK &&
                           response.ContentType.StartsWith("image");
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
