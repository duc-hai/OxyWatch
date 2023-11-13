using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _52000657_LuuDucHai.Models;

namespace _52000657_LuuDucHai.Areas.admin.Controllers
{
    public class bannersController : Controller
    {
        private OxyWatchEntities1 db = new OxyWatchEntities1();

        // GET: admin/banners
        public ActionResult Index()
        {
            return View(db.banners.ToList());
        }

        // GET: admin/banners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            banner banner = db.banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // GET: admin/banners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/banners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,title1,title2,src,link,meta,hide,arrange,datebegin")] banner banner, HttpPostedFileBase src)
        {
            var path = "";
            var filename = "";

            if (ModelState.IsValid)
            {
                if (src != null)
                {
                    filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + src.FileName;
                    path = Path.Combine(Server.MapPath("~/Uploads/banners"), filename);
                    src.SaveAs(path);
                    banner.src = filename;
                }
                else
                {
                    banner.src = "banner.png";
                }
                banner.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                db.banners.Add(banner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(banner);
        }

        // GET: admin/banners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            banner banner = db.banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // POST: admin/banners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,title1,title2,src,link,meta,hide,arrange,datebegin")] banner banner, HttpPostedFileBase src)
        {
            try
            {
                var path = "";
                var filename = "";
                banner temp = getById(banner.id);
                if (ModelState.IsValid)
                {
                    if (src != null)
                    {
                        filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + src.FileName;
                        path = Path.Combine(Server.MapPath("~/Uploads/banners"), filename);
                        src.SaveAs(path);
                        temp.src = filename;
                    }

                    temp.title1 = banner.title1;
                    temp.title2 = banner.title2;
                    temp.link = banner.link;
                    temp.hide = banner.hide;
                    temp.arrange = banner.arrange;
                    temp.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(banner);
        }

        public banner getById(long id)
        {
            return db.banners.Where(x => x.id == id).FirstOrDefault();

        }

        // GET: admin/banners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            banner banner = db.banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // POST: admin/banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            banner banner = db.banners.Find(id);
            db.banners.Remove(banner);
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
