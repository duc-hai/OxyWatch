using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace _52000657_LuuDucHai
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("BotDetectCaptcha.ashx/{*pathInfo}");

            routes.MapRoute("Product", "{type}/{meta}",
                new { controller ="Product", action = "Index", meta = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    {"type", "danh-muc"}
                },
                namespaces: new[] {"_52000657_LuuDucHai.Controllers"} );

            routes.MapRoute("Detail", "{type}/{meta}",
                new { controller = "Product", action = "detailProduct", meta = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    {"type", "san-pham"}
                },
                namespaces: new[] { "_52000657_LuuDucHai.Controllers" });

            routes.MapRoute("Blog", "{type}",
                new { controller = "Blog", action = "Index"},
                new RouteValueDictionary
                {
                    {"type", "blog"}
                },
                namespaces: new[] { "_52000657_LuuDucHai.Controllers" });

            routes.MapRoute("BlogDetail", "{type}/{meta}",
                new { controller = "Blog", action = "Detail", meta = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    {"type", "blog"}
                },
                namespaces: new[] { "_52000657_LuuDucHai.Controllers" });

            routes.MapRoute(
                name: "Search",
                url: "tim-kiem",
                defaults: new { controller = "Product", action = "searchProduct", id = UrlParameter.Optional },
                namespaces: new[] { "_52000657_LuuDucHai.Controllers" }
            );

            routes.MapRoute(
                name: "Add-Cart",
                url: "them-gio-hang",
                defaults: new { controller = "Cart", action = "addItem", id = UrlParameter.Optional },
                namespaces: new[] { "_52000657_LuuDucHai.Controllers" }
            );

            routes.MapRoute(
                name: "View-Cart",
                url: "gio-hang",
                defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "_52000657_LuuDucHai.Controllers" }
            );

            routes.MapRoute(
                name: "Order-Product",
                url: "dat-hang",
                defaults: new { controller = "Cart", action = "orderProduct", id = UrlParameter.Optional },
                namespaces: new[] { "_52000657_LuuDucHai.Controllers" }
            );

            routes.MapRoute(
                name: "Payment-Product",
                url: "phuong-thuc-thanh-toan",
                defaults: new { controller = "Cart", action = "payment", id = UrlParameter.Optional },
                namespaces: new[] { "_52000657_LuuDucHai.Controllers" }
            );

            routes.MapRoute(
                name: "Register",
                url: "dang-ky",
                defaults: new { controller = "Account", action = "signup", id = UrlParameter.Optional },
                namespaces: new[] { "_52000657_LuuDucHai.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "Account", action = "login", id = UrlParameter.Optional },
                namespaces: new[] { "_52000657_LuuDucHai.Controllers" }
            );

            routes.MapRoute(
                name: "Orders",
                url: "don-hang",
                defaults: new { controller = "Order", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "_52000657_LuuDucHai.Controllers" }
            );

            routes.MapRoute("DetailOrder", "{type}/{id}",
                new { controller = "Order", action = "detailOrder", id = UrlParameter.Optional },
                new RouteValueDictionary
                {
                    {"type", "chi-tiet-don-hang"}
                },
                namespaces: new[] { "_52000657_LuuDucHai.Controllers" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "_52000657_LuuDucHai.Controllers" }
            );
        }
    }
}
