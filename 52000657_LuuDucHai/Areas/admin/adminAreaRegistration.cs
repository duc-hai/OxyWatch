using System.Web.Mvc;

namespace _52000657_LuuDucHai.Areas.admin
{
    public class adminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "admin_default",
                "admin/{controller}/{action}/{id}",
                new { controller = "Default", action = "Index", id = UrlParameter.Optional },
                namespaces: new [] { "_52000657_LuuDucHai.Areas.admin.Controllers" }
            );
        }
    }
}