using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kutuphane_Yonetimi.Models.Entity;
namespace Kutuphane_Yonetimi.Controllers
{
    
    public class KategoriController : Controller
    {
        
        DBKUTUPHANEEntities db = new DBKUTUPHANEEntities();
        // GET: Kategori
        public ActionResult Index()
        {
            var degerler = db.TBL_KATEGORI.Where(x =>x.DURUM==true).ToList(); // TBL_KATEGORI tablosundaki tüm verileri listele
            return View(degerler);                   // Verileri View'a gönder
        }
        [HttpGet] // Kategori ekleme sayfasını görüntülemek için
        public ActionResult KategoriEkle()
        {
            return View(); // Kategori ekleme sayfasını döndür
        }
        [HttpPost] // Kategori ekleme işlemi için
        public ActionResult KategoriEkle(TBL_KATEGORI p)
        {
            p.DURUM = true;
            db.TBL_KATEGORI.Add(p); // Yeni kategori ekle
            db.SaveChanges();        // Değişiklikleri kaydet
            return View();    //RedirectToAction("Index"); // Ekleme işleminden sonra Index sayfasına yönlendir
        }

        public ActionResult KategoriSil(int id)
        {
            var ktg = db.TBL_KATEGORI.Find(id); // Silinecek kategoriyi bul
            //db.TBL_KATEGORI.Remove(ktg);         // Kategoriyi sil
            ktg.DURUM = false;          // Kategoriyi silmek yerine durumunu false yap
            db.SaveChanges();                    // Değişiklikleri kaydet
            return RedirectToAction("Index");    // Silme işleminden sonra Index sayfasına yönlendir
        }

        public ActionResult KategoriGetir(int id)
        {
            var ktg = db.TBL_KATEGORI.Find(id); // Güncellenecek kategoriyi bul
            return View("KategoriGetir", ktg);  // Kategori güncelleme sayfasını döndür
        }

        public ActionResult KategoriGuncelle(TBL_KATEGORI p)
        {
            var ktg = db.TBL_KATEGORI.Find(p.ID); // Güncellenecek kategoriyi bul
            ktg.AD = p.AD;                // Kategori adını güncelle
            db.SaveChanges();                              // Değişiklikleri kaydet
            return RedirectToAction("Index");              // Güncelleme işleminden sonra Index sayfasına yönlendir
        }
    }
}