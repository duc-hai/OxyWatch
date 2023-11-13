using _52000657_LuuDucHai.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _52000657_LuuDucHai.Areas.admin.Controllers
{
    public class DefaultController : Controller
    {
        OxyWatchEntities1 db = new OxyWatchEntities1();
        // GET: admin/Default
        public ActionResult Index()
        {
            if (Session["usernameAD"] == null)
            {
                return RedirectToAction("Index", "login", new { area = "admin" });
            }
            return View();
        }

        public ActionResult getTotalCustomer () //Lấy toàn bộ số lượng khách hàng (bảng account)
        {
            var total = (from t in db.accounts
                         where t.role == 1
                         select t).Count();
            ViewBag.total = total;
            return PartialView();
        }

        public ActionResult getNewOrder () //Lấy số lượng đơn hàng mới (Trạng thái: Chờ xác nhận)
        {
            var total = (from t in db.orders
                         where t.status.Contains("Chờ xác nhận")
                         select t).Count();
            ViewBag.newOrder = total;
            return PartialView();
        }

        public ActionResult getShippingOrder() //Lấy số lượng các đơn hàng đang giao (Trạng thái: Đang giao hàng)
        {
            var total = (from t in db.orders
                         where t.status.Contains("Đang giao hàng")
                         select t).Count();
            ViewBag.shipping = total;
            return PartialView();
        }

        public ActionResult getRevenue() //Lấy doanh thu của tháng hiện tại
        {
            //Danh sách đơn hàng đã giao thành công của tháng hiện tại
            var orderThisMonth = (from t in db.orders
                                  where t.status == "Giao hàng thành công" && t.datebegin.Value.Month == DateTime.Today.Month
                                  select t).ToList();
            var sum = 0;
            //Tính tổng danh thu 
            foreach (var i in orderThisMonth)
            {
                var details = (from t in db.orderDetails
                         where t.orderId == i.id
                         select t).ToList();
                foreach (var j in details)
                {
                    sum += (int)j.price * (int)j.quantity;
                }
            }
            ViewBag.revenue = sum;
            return PartialView();
        }
    }
}