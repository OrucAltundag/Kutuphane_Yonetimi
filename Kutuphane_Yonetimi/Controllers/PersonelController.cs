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
    public class PersonelController : Controller
    {
        DBKUTUPHANEEntities db = new DBKUTUPHANEEntities();
        // GET: Personel
        public ActionResult Index()
        {
            var degerler = db.TBL_PERSONEL.ToList();
            return View(degerler);
        }

        [HttpGet] // Personel ekleme sayfası için
        public ActionResult PersonelEkle()
        {
            return View(); // Personel ekleme sayfasını döndür
        }
        [HttpPost] // Personel ekleme işlemi için
        public ActionResult PersonelEkle(TBL_PERSONEL p)
        {
            if(!ModelState.IsValid)
            {
                return View("PersonelEkle"); // Model geçerli değilse, aynı sayfayı döndür
            }
            db.TBL_PERSONEL.Add(p); // Yeni Personel ekle
            db.SaveChanges();        // Değişiklikleri kaydet
            return RedirectToAction("Index"); // Ekleme işleminden sonra Index sayfasına yönlendir
        }

        public ActionResult PersonelSil(int id)
        {
            var per = db.TBL_PERSONEL.Find(id); // Silinecek Personel bul
            db.TBL_PERSONEL.Remove(per);         // Personel sil
            db.SaveChanges();                    // Değişiklikleri kaydet
            return RedirectToAction("Index");    // Silme işleminden sonra Index sayfasına yönlendir
        }

        public ActionResult PersonelGetir(int id)
        {
            
            var per = db.TBL_PERSONEL.Find(id); // Güncellenecek Personel bul
            return View("PersonelGetir", per);  // Kategori güncelleme sayfasını döndür
        }

        public ActionResult PersonelGuncelle(TBL_PERSONEL p)
        {
            if (!ModelState.IsValid)
            {
                return View("PersonelGetir"); // Model geçerli değilse, aynı sayfayı döndür
            }

            var per = db.TBL_PERSONEL.Find(p.ID); // Güncellenecek Personel bul
            per.PERSONEL = p.PERSONEL;                // Personel adını güncelle
            db.SaveChanges();                              // Değişiklikleri kaydet
            return RedirectToAction("Index");              // Güncel
        }


        [HttpPost]
        public ActionResult GirisYap(TBL_PERSONEL p)
        {

            // 1. Data Annotation Kontrolü (Regex ve Zorunluluklar)
            // Eğer TC 11 hane değilse veya harf içeriyorsa burası "false" döner.
            if (!ModelState.IsValid)
            {
                ViewBag.Panel = "Personel"; // Kartın personel yüzü açık kalsın
                return View("~/Views/Login/GirisYap.cshtml");
            }

            var bilgiler = db.TBL_PERSONEL.FirstOrDefault(x => x.TC == p.TC && x.SIFRE == p.SIFRE);

            if (bilgiler != null)
            {
                // Personel giriş başarılı → kimlik oluştur
                FormsAuthentication.SetAuthCookie(bilgiler.TC, false);
                Session["TC"] = bilgiler.TC.ToString();
                Session["PERSONELID"] = bilgiler.ID.ToString();
                Session["ADSOYAD"] = bilgiler.PERSONEL.ToString();
                Session["LoginType"] = "PERSONEL";

                return RedirectToAction("Index", "Home"); // Personel paneline yönlendirme
            }
            else
            {
                ViewBag.hata = "TC veya Şifre hatalı!";
                ViewBag.Panel = "Personel";
                return View("~/Views/Login/GirisYap.cshtml"); // Aynı sayfaya geri dön
            }
        }


    }
}