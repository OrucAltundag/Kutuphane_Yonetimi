using Kutuphane_Yonetimi.Models.Entity;
using Kutuphane_Yonetimi.Models.Siniflarim;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kutuphane_Yonetimi.Controllers
{
 
    [AllowAnonymous]
    public class HomeController : Controller
    {
        DBKUTUPHANEEntities db = new DBKUTUPHANEEntities();
        public ActionResult Index()
        {
            var model = new PanelViewModel();

            // Toplam Kitap
            model.ToplamKitap = db.TBL_KITAP.Count();

          // En çok ödünç alınan kitap
            model.EnPopulerKitap = db.TBL_HAREKET
                .GroupBy(h => h.TBL_KITAP.AD)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            // Son eklenen kitap
            model.SonEklenenKitap = db.TBL_KITAP
                .OrderByDescending(k => k.ID)
                .Select(k => k.AD)
                .FirstOrDefault();



            // Bugün Ödünç
            var bugun = DateTime.Today;
            model.BugunOdunc = db.TBL_HAREKET
                .Where(x => DbFunctions.TruncateTime(x.ALISTARIH) == bugun)
                .Select(x => new BugunOduncDto
                {
                    KitapAdi = x.TBL_KITAP.AD,
                    UyeAdi = x.TBL_UYELER.AD + " " + x.TBL_UYELER.SOYAD
                }).ToList();

            model.BugunOduncSayisi = model.BugunOdunc.Count;

            // Toplam Üye
            model.ToplamUye = db.TBL_UYELER.Count();

            model.SonUye = db.TBL_UYELER
                .OrderByDescending(u => u.ID)
                .Select(u => u.AD + " " + u.SOYAD)
                .FirstOrDefault();

            // En Aktif Üye (en çok kitap alan üye)
            model.EnAktifUye = db.TBL_HAREKET
                .GroupBy(h => new { h.UYE })
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key.UYE)
                .Join(db.TBL_UYELER, uyeId => uyeId, uye => uye.ID, (uyeId, uye) => uye.AD + " " + uye.SOYAD)
                .FirstOrDefault();


            // Kategori Sayısı
            model.KategoriSayisi = db.TBL_KATEGORI.Count();

            // Önemli Kategoriler (en çok kitabı olan 5 kategori)
            model.OnemliKategoriler = db.TBL_KITAP
                .GroupBy(k => k.TBL_KATEGORI.AD)
                .Select(g => new KategoriDto
                {
                    Kategori = g.Key,
                    KitapSayisi = g.Count()
                })
                .OrderByDescending(x => x.KitapSayisi)
                .Take(5)
                .ToList();




            // Bugün ödünç alınan kitap sayısı
            //model.BugunOduncSayisi = db.TBL_HAREKET.Count(x => x.ALISTARIH == bugun);

            // Bugün iade edilen kitap sayısı
            model.BugunIadeSayisi = db.TBL_HAREKET.Count(x => x.IADETARIH == bugun);

            
            // Son iade tarihi bugün olan kitaplar
            model.BugunSonIade = db.TBL_HAREKET
                .Where(x => x.IADETARIH == bugun)
                .Select(x => new BugunOduncDto
                {
                    KitapAdi = x.TBL_KITAP.AD,
                    UyeAdi = x.TBL_UYELER.AD + " " + x.TBL_UYELER.SOYAD,
                })
                .ToList();

            model.BugunSonIadeSayisi = model.BugunSonIade.Count;




            // En çok aktif ilk 5 üye
            var aktifUyeler = db.TBL_HAREKET
                .GroupBy(h => h.UYE) // UYE kolonuna göre grupla
                .Select(g => new
                {
                    UyeId = g.Key,
                    Sayisi = g.Count()
                })
                .OrderByDescending(x => x.Sayisi)
                .Take(5)
                .Join(db.TBL_UYELER,
                      g => g.UyeId,
                      u => u.ID,
                      (g, u) => new
                      {
                          AdSoyad = u.AD + " " + u.SOYAD,
                          AktiviteSayisi = g.Sayisi
                      })
                .ToList();

            // En çok ceza alan ilk 5 üye
            var cezaUyeler = db.TBL_CEZALAR
                .GroupBy(c => c.UYE) // UYE kolonuna göre grupla
                .Select(g => new
                {
                    UyeId = g.Key,
                    ToplamCeza = g.Sum(x => x.PARA)  // her üyenin ceza toplamı
                })
                .OrderByDescending(x => x.ToplamCeza)
                .Take(5)
                .Join(db.TBL_UYELER,
                      g => g.UyeId,
                      u => u.ID,
                      (g, u) => new
                      {
                          AdSoyad = u.AD + " " + u.SOYAD,
                          CezaSayisi = g.ToplamCeza
                      })
                .ToList();


            // Oran hesaplamak için toplam değerler
            int toplamAktivite = db.TBL_HAREKET.Count();
            decimal toplamCeza = db.TBL_CEZALAR.Sum(c => (decimal?)c.PARA) ?? 0;

            model.TopAktifUyeler = aktifUyeler.Select(u => new UyeOranDto
            {
                AdSoyad = u.AdSoyad,
                Deger = u.AktiviteSayisi,
                Oran = toplamAktivite == 0 ? 0 : (u.AktiviteSayisi * 100 / toplamAktivite)
            }).ToList();

            model.TopCezaUyeler = cezaUyeler.Select(u => new UyeOranDto
            {
                AdSoyad = u.AdSoyad,
                Deger = (int)u.CezaSayisi,  // PARA decimal olduğu için int'e çeviriyoruz (istersen decimal tutabilirsin)
                Oran = toplamCeza == 0 ? 0 : (int)(u.CezaSayisi * 100 / toplamCeza)
            }).ToList();





            return View(model);





        }



    }
}

