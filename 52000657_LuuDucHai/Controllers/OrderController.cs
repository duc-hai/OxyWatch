using _52000657_LuuDucHai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _52000657_LuuDucHai.Controllers
{
    public class OrderController : Controller
    {
        OxyWatchEntities1 db = new OxyWatchEntities1();
        // GET: Order
        public ActionResult Index()
        {
            if (checkLogin() == false)
            {
                return Redirect("dang-nhap");
            }
            var userId = (int)Session["ID"];
            var v = (from t in db.orders
                         where t.customerId == userId //Lấy list đơn hàng tương ứng với id người dùng
                         orderby t.datebegin descending //Sắp xếp theo ngày đặt để tiện theo dõi
                         select t).ToList();
            return View(v);
        }

        public ActionResult detailOrder(int id) //Lấy dữ liệu của bảng orderDetail
        {
            if (checkLogin() == false)
            {
                return Redirect("dang-nhap");
            }
            var v = (from t in db.orderDetails
                     where t.orderId == id
                     select t).ToList();
            return View(v);
        }

        public ActionResult getProductById (int id) //Lấy dữ liệu bảng product dựa vào id
        {
            var v = (from t in db.products
                     where t.id == id
                     select t).FirstOrDefault();
            return PartialView(v);
        }

        public ActionResult getOrderById (int id) //Lấy dữ liệu bảng order dựa vào id
        {
            var v = (from t in db.orders
                     where t.id == id
                     select t).FirstOrDefault();
            return PartialView(v);
        }

        public bool checkLogin()
        {
            if (Session["username"] == null)
            {
                return false;
            }
            return true;
        }

    }
}