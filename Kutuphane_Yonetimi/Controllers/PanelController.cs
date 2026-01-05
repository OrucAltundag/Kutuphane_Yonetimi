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
        [ValidateAntiForgeryToken]
        public ActionResult Index2(TBL_UYELER p, HttpPostedFileBase Foto, HttpPostedFileBase FOTORAF)
        {
            ModelState.Remove("MAIL");
            // DATA ANNOTATION KONTROLÜ 
            if (!ModelState.IsValid)
            {
                
                if (ModelState["TELEFON"] != null && ModelState["TELEFON"].Errors.Count > 0)
                {
                    TempData["TelHata"] = ModelState["TELEFON"].Errors[0].ErrorMessage;
                    TempData["TelDeger"] = p.TELEFON; // Hatalı veriyi geri gönder ki silinmesin
                }
                else
                {
                    
                    var hataMesaji = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
                    TempData["TelHata"] = hataMesaji ?? "Lütfen bilgileri eksiksiz giriniz.";
                }

                return RedirectToAction("Index"); // Hata olduğu için güncelleme yapmadan geri dönüyoruz.
            }

            //  KULLANICIYI BUL
            var kullanici = (string)Session["KULLANICIADI"];
            var uye = db.TBL_UYELER.FirstOrDefault(x => x.KULLANICIADI == kullanici);

            if (uye == null) return RedirectToAction("Index");

            //  GÜNCELLEME İŞLEMİ 
            uye.AD = p.AD;
            uye.SOYAD = p.SOYAD;
            uye.OKUL = p.OKUL;
            uye.KULLANICIADI = p.KULLANICIADI;
            if (!string.IsNullOrEmpty(p.MAIL))
            {
                uye.MAIL = p.MAIL;
            }
            uye.TELEFON = p.TELEFON; 

            //  DOSYA YÜKLEME İŞLEMİ 
            var file = Foto ?? FOTORAF;
            if (file != null && file.ContentLength > 0)
            {
                var ext = System.IO.Path.GetExtension(file.FileName).ToLower();
                string[] izinli = { ".jpg", ".jpeg", ".png", ".bmp" };

                if (!izinli.Contains(ext))
                {
                    TempData["SifreHata"] = "Sadece jpg, jpeg, png, bmp yükleyebilirsin.";
                    return RedirectToAction("Index");
                }

                var klasor = Server.MapPath("~/Uploads/Uyeler/");
                if (!System.IO.Directory.Exists(klasor))
                    System.IO.Directory.CreateDirectory(klasor);

                var dosyaAdi = Guid.NewGuid().ToString("N") + ext;
                var kayitYolu = System.IO.Path.Combine(klasor, dosyaAdi);

                file.SaveAs(kayitYolu);

                // Eski fotoğrafı silme işlemi...
                if (!string.IsNullOrEmpty(uye.FOTOGRAF) && uye.FOTOGRAF.StartsWith("~/Uploads/Uyeler/"))
                {
                    var eskiFiziksel = Server.MapPath(uye.FOTOGRAF);
                    if (System.IO.File.Exists(eskiFiziksel))
                        System.IO.File.Delete(eskiFiziksel);
                }

                uye.FOTOGRAF = "~/Uploads/Uyeler/" + dosyaAdi;
            }

           
            db.SaveChanges();
            TempData["SifreBasari"] = "Profil başarıyla güncellendi.";
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
        [ValidateAntiForgeryToken]
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


        // Duyuru detay + onaylı yorumlar
        [HttpGet]
        [Authorize]
        public ActionResult DuyuruDetay(byte id)
        {
            var duyuru = db.TBL_DUYURULAR.Find(id);
            if (duyuru == null) return HttpNotFound();

            var yorumlar = db.TBL_DUYURU_YORUM
                .Where(x => x.DUYURU_ID == id && x.ONAY == true)
                .OrderByDescending(x => x.TARIH)
                .ToList();

            ViewBag.Yorumlar = yorumlar;
            return View(duyuru); 
        }

        //  Üye yorum ekler (AJAX -> JSON)
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult YorumEkle(byte duyuruId, string mesaj)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mesaj))
                    return Json(new { success = false, message = "Yorum boş olamaz." });

                int uyeId = 0;

                if (Session["ID"] != null)
                {
                    uyeId = Convert.ToInt32(Session["ID"]);
                }
                else
                {
                    var kullanici = (string)Session["KULLANICIADI"];
                    if (!string.IsNullOrWhiteSpace(kullanici))
                    {
                        uyeId = db.TBL_UYELER
                            .Where(x => x.KULLANICIADI == kullanici)
                            .Select(x => x.ID)
                            .FirstOrDefault();
                    }
                }

                if (uyeId == 0)
                    return Json(new { success = false, message = "Üye oturumu bulunamadı (ID yok)." });

                db.TBL_DUYURU_YORUM.Add(new TBL_DUYURU_YORUM
                {
                    DUYURU_ID = duyuruId,   
                    UYE_ID = uyeId,
                    MESAJ = mesaj.Trim(),
                    TARIH = DateTime.Now,
                    ONAY = false
                });

                db.SaveChanges();

                return Json(new { success = true, message = "Yorum gönderildi. Admin onaylayınca görünecek." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Sunucu hatası: " + ex.Message });
            }
        }





    }
}