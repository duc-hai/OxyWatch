using _52000657_LuuDucHai.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace _52000657_LuuDucHai.Controllers
{
    public class ProductController : Controller
    {  
        OxyWatchEntities1 _db = new OxyWatchEntities1();
        // GET: Product
        public ActionResult Index(string meta)
        {
            var v = from t in _db.categories    
                    where t.meta == meta
                    select t;
            return View(v.FirstOrDefault()); //Chỉ lấy ra 1 category có meta trùng với meta truyền vào
        }
        public ActionResult getProductByCate(long id, string metaTitle)
        {
            ViewBag.meta = "san-pham";
            var v = from t in _db.products
                    where t.idCate == id
                    select t;
            return PartialView(v.ToList());
        }

        public ActionResult getProduct() //Lấy sản phẩm có lượt bán cao nhất
        {
            ViewBag.meta = "san-pham"; //Meta để truyền qua view
            var v = from t in _db.products
                    where t.hide == true
                    orderby t.soluotban descending
                    select t;
            return PartialView(v.Take(8).ToList()); //Chỉ show 8 sản phẩm ở trang chủ
        }

        public ActionResult getNewProduct() //Lấy sản phẩm mới nhất
        {
            ViewBag.meta = "san-pham";
            var v = from t in _db.products
                    where t.hide == true
                    orderby t.datebegin descending
                    select t;
            return PartialView(v.Take(8).ToList());
        }

        public ActionResult detailProduct(string meta)
        {
            var v = from t in _db.products
                    where t.meta == meta && t.hide == true
                    select t;
            return View(v.FirstOrDefault());
        }

        public ActionResult relateProduct ()
        {
            ViewBag.meta = "san-pham";
            var v = from t in _db.products
                    where t.hide == true
                    select t;
            return PartialView(v.Take(4).ToList());
        }

        [HttpGet]
        public ActionResult searchProduct (string q)
        {
            ViewBag.keySearch = q;
            var v = from t in _db.products
                    where t.hide == true && t.name.Contains(q)
                    select t;
            return View(v.ToList());
        }
    }
}