using Kutuphane_Yonetimi.Models.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kutuphane_Yonetimi.Controllers
{

    public class IstatistikController : Controller
    {
        // GET: Istatistik
        DBKUTUPHANEEntities db = new DBKUTUPHANEEntities();
        public ActionResult Index()
        {
           

            var deger0 = db.TBL_CEZALAR.Sum(X => X.PARA);
            ViewBag.d0 = Math.Round((decimal)deger0 / 41, 2);
            var deger100 = db.TBL_UYELER.Count();
            ViewBag.d10 = deger100;
            var deger20 = db.TBL_KITAP.Count();
            ViewBag.d20 = deger20;
            var deger30 = db.TBL_KITAP.Where(X => X.DURUM == false).Count();
            ViewBag.d30 = deger30;



            return View();

        }

        public ActionResult Hava()
        {
            return View();
        }


        public ActionResult LinqKart()
        {
            var deger1 = db.TBL_KITAP.Count();
            ViewBag.d1 = deger1;
            var deger2 = db.TBL_UYELER.Count();
            ViewBag.d2 = deger2;
            var deger3 = db.TBL_CEZALAR.Sum(X => X.PARA);
            ViewBag.d3 = deger3;
            var deger4 = db.TBL_KITAP.Where(X => X.DURUM == false).Count();
            ViewBag.d4 = deger4;
            var deger5 = db.TBL_KATEGORI.Count();
            ViewBag.d5 = deger5;
            var deger6 = db.EnAktifUye().FirstOrDefault();
            ViewBag.d6 = deger6;
            var deger7 = db.EnCokOkunanKitap().FirstOrDefault();
            ViewBag.d7 = deger7;
            var deger8 = db.EnFazlaKitapliYazar().FirstOrDefault();
            ViewBag.d8 = deger8;
            var deger9 = db.EnIyiYayinEvi().FirstOrDefault();
            ViewBag.d9 = deger9;
            var deger10 = db.EnBasariliPersonel().FirstOrDefault();
            ViewBag.d10 = deger10;
            var deger11 = db.TBL_ILETISIM.Count();
            ViewBag.d11 = deger11;
            var deger12 = db.BugununEmanetleri().FirstOrDefault();
            ViewBag.d12 = deger12;

            return View();
        }



        

    }
}