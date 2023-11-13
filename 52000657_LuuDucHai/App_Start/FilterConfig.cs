using System.Web;
using System.Web.Mvc;

namespace _52000657_LuuDucHai
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
