using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using _52000657_LuuDucHai.Models;

namespace _52000657_LuuDucHai.Controllers
{
    public class DefaultController : Controller
    {
        OxyWatchEntities1 _db = new OxyWatchEntities1();
        // GET: Default
        public ActionResult Index()
        {
            ViewBag.Title = "Trang chủ"; 
            ViewBag.Keywords = "Đồng hồ nam nữ"; //Sử dụng để SEO Web tốt hơn
            ViewBag.Description = "Đồng hồ nam nữ thời trang uy tín, chất lượng, chính hãng 100%";

            var cart = Session["cartSession"]; //Lấy ra danh sách cart từ session
            var list = new List<CartItem>();
            if (cart != null) //Nếu cart rỗng thì khởi tạo danh sách mới
            {
                list = (List<CartItem>)cart;
                Session["cartNumber"] = list.Count();
            }
            else
            {
                Session["cartNumber"] = 0;
            }

            return View();
        }

        //Cache nội dung trả về trên server hoặc client giúp cho việc tải nội dung nhanh hơn. Mỗi lần load lại trang chỉ cần lấy dữ liệu 
        //có sẵn trong bộ nhớ cache thay vì phải truy xuất vào db, từ đó giúp tăng hiệu suất (chỉ cache một số nội dung ít có sự thay đổi)
        [OutputCache(Duration = 60 * 60, VaryByParam = "None")]
        public ActionResult getMenu ()
        {
            var v = from t in _db.menus
                    where t.hide == true
                    orderby t.arrange ascending
                    select t;
            return PartialView(v.ToList());
        }

        [OutputCache(Duration = 60 * 60, VaryByParam = "None")]
        public ActionResult getBanner ()
        {
            var v = from t in _db.banners
                    where t.hide == true
                    orderby t.arrange ascending
                    select t;
            return PartialView(v.ToList());
        }

        [OutputCache(Duration = 60 * 60, VaryByParam = "None")]
        public ActionResult getHeader() //Bảng inforshop là thông tin của shop, có thể lấy thông tin này dùng chung cho cả header và footer
        {
            return getFooter();
        }

        [OutputCache(Duration = 60 * 60, VaryByParam = "None")]
        public ActionResult getFooter ()
        {
            var rel = (from t in _db.inforShops
                       where t.hide == true
                       orderby t.order ascending
                       select t).FirstOrDefault();
            return PartialView(rel);
        }

        [OutputCache(Duration = 60 * 60, VaryByParam = "None")]
        public ActionResult getMap ()
        {
            return getFooter();
        }

        public ActionResult getCategory(int id)
        {
            ViewBag.meta = "danh-muc";
            var v = from t in _db.categories
                    where t.hide == true && t.idmenu == id
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }
        
        public ActionResult notFoundUrl ()
        {
            return View();
        }
    }
}