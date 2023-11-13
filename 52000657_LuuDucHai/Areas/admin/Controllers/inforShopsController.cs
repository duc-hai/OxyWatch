using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _52000657_LuuDucHai.Models;

namespace _52000657_LuuDucHai.Areas.admin.Controllers
{
    public class inforShopsController : Controller
    {
        private OxyWatchEntities1 db = new OxyWatchEntities1();

        // GET: admin/inforShops
        public ActionResult Index()
        {
            return View(db.inforShops.ToList());
        }

        // GET: admin/inforShops/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inforShop inforShop = db.inforShops.Find(id);
            if (inforShop == null)
            {
                return HttpNotFound();
            }
            return View(inforShop);
        }

        // GET: admin/inforShops/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/inforShops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,shopname,logoshop,introduce,address,email,phone,linkfb,meta,hide,order,datebegin,lat,lng")] inforShop inforShop, HttpPostedFileBase logoshop)
        {
            var path = "";
            var filename = "";
            if (ModelState.IsValid)
            {
                if (logoshop != null)
                {
                    filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + logoshop.FileName;
                    path = Path.Combine(Server.MapPath("~/Uploads/images"), filename);
                    logoshop.SaveAs(path);
                    inforShop.logoshop = filename;
                }
                else
                {
                    inforShop.logoshop = "logo.png";
                }
                inforShop.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                db.inforShops.Add(inforShop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inforShop);
        }

        // GET: admin/inforShops/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inforShop inforShop = db.inforShops.Find(id);
            if (inforShop == null)
            {
                return HttpNotFound();
            }
            return View(inforShop);
        }

        // POST: admin/inforShops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,shopname,logoshop,introduce,address,email,phone,linkfb,meta,hide,order,datebegin,lat,lng")] inforShop inforShop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inforShop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inforShop);
        }

        // GET: admin/inforShops/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inforShop inforShop = db.inforShops.Find(id);
            if (inforShop == null)
            {
                return HttpNotFound();
            }
            return View(inforShop);
        }

        // POST: admin/inforShops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            inforShop inforShop = db.inforShops.Find(id);
            db.inforShops.Remove(inforShop);
            db.SaveChanges();
            return RedirectToAction("Index");
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
