using System.Web;
using System.Web.Mvc;

namespace Kutuphane_Yonetimi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
