using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _52000657_LuuDucHai.Models;

namespace _52000657_LuuDucHai.Areas.admin.Controllers
{
    public class ordersController : Controller
    {
        private OxyWatchEntities1 db = new OxyWatchEntities1();

        // GET: admin/orders
        public ActionResult Index()
        {
            var v = (from t in db.orders
                    orderby t.id descending
                    select t).ToList();
            return View(v);
        }

        public ActionResult getListProduct(int id)
        {
            var v = (from t in db.orderDetails
                     where t.orderId == id
                     select t).ToList();
            return PartialView(v);
        }

        public ActionResult getProduct (int id)
        {
            var v = (from t in db.products
                     where t.id == id
                     select t).FirstOrDefault();
            return PartialView(v);
        }

        // GET: admin/orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            var v = (from t in db.accounts
                     where t.id == order.customerId
                     select t).FirstOrDefault();
            ViewBag.usernameCus = v.username;
            return View(order);
        }

        public JsonResult updateStatus (int id, string status)
        {
            try
            {
                var rel = db.orders.SingleOrDefault(v => v.id == id);
                if (rel != null)
                {
                    rel.status = status;
                    db.SaveChanges();
                }
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = false,
                    message = e.Message
                });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
