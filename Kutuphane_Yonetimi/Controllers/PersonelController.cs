using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kutuphane_Yonetimi.Models.Entity;


namespace Kutuphane_Yonetimi.Controllers
{
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
            db.TBL_PERSONEL.Add(p); // Yeni Personel ekle
            db.SaveChanges();        // Değişiklikleri kaydet
            return View();    //RedirectToAction("Index"); // Ekleme işleminden sonra Index sayfasına yönlendir
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
            var per = db.TBL_PERSONEL.Find(p.ID); // Güncellenecek Personel bul
            per.PERSONEL = p.PERSONEL;                // Personel adını güncelle
            db.SaveChanges();                              // Değişiklikleri kaydet
            return RedirectToAction("Index");              // Güncel
        }
    }
}