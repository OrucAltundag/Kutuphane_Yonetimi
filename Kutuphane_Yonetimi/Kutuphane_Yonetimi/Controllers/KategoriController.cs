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
            var degerler = db.TBL_KATEGORI.ToList(); // TBL_KATEGORI tablosundaki tüm verileri listele
            return View(degerler);                   // Verileri View'a gönder
        }
    }
}