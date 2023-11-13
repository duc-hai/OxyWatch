using _52000657_LuuDucHai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _52000657_LuuDucHai.Controllers
{
    public class BlogController : Controller
    {
        OxyWatchEntities1 db = new OxyWatchEntities1();
        // GET: Blog
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getNewBlog()
        {
            ViewBag.meta = "blog";
            var v = from t in db.news
                    where t.hide == true
                    orderby t.datebegin descending
                    select t;
            return PartialView(v.Take(5).ToList());
        }

        public ActionResult getBlog()
        {
            ViewBag.meta = "blog";
            var v = from t in db.news
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.Take(10).ToList());
        }

        public ActionResult Detail(string meta)
        {
            var v = from t in db.news
                    where t.meta == meta && t.hide == true
                    select t;
            return View(v.FirstOrDefault());
        }
    }
}