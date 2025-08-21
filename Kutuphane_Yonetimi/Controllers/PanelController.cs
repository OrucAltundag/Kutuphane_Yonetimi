using Kutuphane_Yonetimi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kutuphane_Yonetimi.Controllers
{
  
    public class PanelController : Controller
    {
        // GET: Panel
        DBKUTUPHANEEntities db = new DBKUTUPHANEEntities(); 
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var kullanici = (string)Session["KULLANICIADI"];
           

            var degerler = db.TBL_DUYURULAR.ToList();

            ViewBag.LoginType = Session["LoginType"]; // oturum açma türünü ViewBag ile gönder

            var AdSoyad = db.TBL_UYELER.Where(x => x.KULLANICIADI == kullanici).Select(y => y.AD + " " + y.SOYAD).FirstOrDefault();
            ViewBag.AdSoyad = AdSoyad;

            var Resim = db.TBL_UYELER.Where(x => x.KULLANICIADI == kullanici).Select(y => y.FOTOGRAF ).FirstOrDefault();
            ViewBag.Resim = Resim;

            var Okul = db.TBL_UYELER.Where(x => x.KULLANICIADI == kullanici).Select(y => y.OKUL).FirstOrDefault();
            ViewBag.Okul = Okul;

            var Tel = db.TBL_UYELER.Where(x => x.KULLANICIADI == kullanici).Select(y => y.TELEFON).FirstOrDefault();
            ViewBag.Tel = Tel;

            var Mail = db.TBL_UYELER.Where(x => x.KULLANICIADI == kullanici).Select(y => y.MAIL).FirstOrDefault();
            ViewBag.Mail = Mail;

            var UyeId = db.TBL_UYELER.Where(x => x.KULLANICIADI == kullanici).Select(y => y.ID).FirstOrDefault();

            int KitapSayisi = db.TBL_HAREKET.Count(x => x.UYE == UyeId);
            ViewBag.KitapSayisi = KitapSayisi;

            var tur = db.TBL_HAREKET
                .Where(x => x.UYE == UyeId && x.UyeGetirTarih != null)
                .GroupBy(x => x.TBL_KITAP.TBL_KATEGORI.AD)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
            ViewBag.Tur = tur ?? "Henüz okunmamış";

            var yazar = db.TBL_HAREKET
                  .Where(x => x.UYE == UyeId && x.UyeGetirTarih != null)
                  .GroupBy(x => x.TBL_KITAP.TBL_YAZAR.AD + " " + x.TBL_KITAP.TBL_YAZAR.SOYAD)
                  .OrderByDescending(g => g.Count()) 
                  .Select(g => g.Key)
                  .FirstOrDefault();
            ViewBag.Yazar = yazar ?? "Henüz okunmamış";

            var ceza = db.TBL_CEZALAR
                  .Where(x => x.UYE == UyeId)
                  .Sum(x => (decimal?)x.PARA) ?? 0;
            ViewBag.Ceza = ceza.ToString("0.00") + " ₺";


            return View(degerler);
        }

        [HttpPost]
        public ActionResult Index2(TBL_UYELER p)
        {
            var kullanici = (string)Session["KULLANICIADI"];
            var uye = db.TBL_UYELER.FirstOrDefault(x => x.KULLANICIADI == kullanici);
            uye.SIFRE = p.SIFRE;
            uye.AD = p.AD;
            uye.SOYAD = p.SOYAD;
            uye.FOTOGRAF = p.FOTOGRAF;
            uye.OKUL = p.OKUL;
            uye.MAIL = p.MAIL;
            uye.KULLANICIADI = p.KULLANICIADI;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Kitaplarim()
        {
            int uyeId = int.Parse(Session["ID"].ToString());
            var uye = db.TBL_UYELER.Find(uyeId);
            var degerler = db.TBL_HAREKET.Where(x => x.UYE == uye.ID).ToList();
            return View(degerler);
        }

        public ActionResult CikisYap()
        {
            Session.Abandon();
            return RedirectToAction("GirisYap", "Login");
        }

        public ActionResult Duyurular()
        {
            var duyurular = db.TBL_DUYURULAR.ToList();
            return View(duyurular);
        }

        public PartialViewResult Partial1()
        {
            
            return PartialView();
        }

        public ActionResult LikeDuyuru(int id)
        {
            var duyuru = db.TBL_DUYURULAR.Find(id);
            if (duyuru != null)
            {
                duyuru.LikeSayısı = (duyuru.LikeSayısı ?? 0) + 1;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Panel"); // kendi sayfana dön
        }

        public PartialViewResult PartialAyarlar()
        {
            var kullanici = (string)Session["KULLANICIADI"];
            var UyeId = db.TBL_UYELER.Where(x => x.KULLANICIADI == kullanici).Select(y => y.ID).FirstOrDefault();
            var UyeBul = db.TBL_UYELER.Find(UyeId);


            return PartialView("PartialAyarlar",UyeBul);
        }

        [HttpPost]
        public ActionResult SifreGuncelle(string EskiSifre, string YeniSifre, string YeniSifreTekrar)
        {
            var kullanici = (string)Session["KULLANICIADI"];
            var uye = db.TBL_UYELER.FirstOrDefault(x => x.KULLANICIADI == kullanici);

            if (uye == null)
                return RedirectToAction("Index");

            // Eski şifre doğru mu?
            if (uye.SIFRE != EskiSifre)
            {
                TempData["SifreHata"] = "Eski şifre yanlış!";
                return RedirectToAction("Index");
            }

            // Yeni şifreler aynı mı?
            if (YeniSifre != YeniSifreTekrar)
            {
                TempData["SifreHata"] = "Yeni şifreler uyuşmuyor!";
                return RedirectToAction("Index");
            }

            // Güncelle
            uye.SIFRE = YeniSifre;
            db.SaveChanges();

            TempData["SifreBasari"] = "Şifre başarıyla güncellendi.";
            return RedirectToAction("Index");
        }




    }
}