using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kutuphane_Yonetimi.Models.Entity;

namespace Kutuphane_Yonetimi.Controllers
{
  
    public class IslemController : Controller
    {
        // GET: Islem
        DBKUTUPHANEEntities db = new DBKUTUPHANEEntities();
        public ActionResult Index()
        {
            var degerler = db.TBL_HAREKET.Where(X => X.IslemDurumu == true).ToList();
            return View(degerler);
        }
    }
}