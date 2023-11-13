using System.Web;
using System.Web.Optimization;

namespace _52000657_LuuDucHai
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            //Optimize JS files
            bundles.Add(new ScriptBundle("~/bundles/coreJS").Include(
                        "~/Scripts/js/jquery.min.1.11.0.js",
                        "~/Scripts/js/bootstrap.min.js",
                        "~/Scripts/js/api.jquery.js",
                        "~/Scripts/js/default_script.min.js",
                        "~/Scripts/js/haravan.plugin.1.0.js",
                        "~/Scripts/js/jquery.flexslider.js",
                        "~/Scripts/js/scripts.js"));

            //Optimize CSS files
            bundles.Add(new StyleBundle("~/bundles/core").Include(
                      "~/Content/css/default_style.min.css",
                      "~/Content/css/flexslider.css",
                      "~/Content/css/roboto.css",
                      "~/Content/css/bootstrap.3.3.1.css",
                      "~/Content/css/styles.css"));
            //bundles.IgnoreList.Clear();
            BundleTable.EnableOptimizations = true;
        }
    }
}
