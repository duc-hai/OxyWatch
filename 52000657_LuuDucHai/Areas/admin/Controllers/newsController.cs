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
using _52000657_LuuDucHai.Helper;
using _52000657_LuuDucHai.Models;

namespace _52000657_LuuDucHai.Areas.admin.Controllers
{
    public class newsController : Controller
    {
        private OxyWatchEntities1 db = new OxyWatchEntities1();

        // GET: admin/news
        public ActionResult Index()
        {
            return View(db.news.ToList());
        }

        // GET: admin/news/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: admin/news/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/news/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,name,image,description,detail,meta,hide,order,datebegin")] news news, HttpPostedFileBase image)
        {
            var path = "";
            var filename = "";
            if (ModelState.IsValid)
            {
                if (image != null) 
                {
                    //Tên của file sẽ là ngày giờ hiện tại + tên file để tránh trùng tên
                    filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + image.FileName; 
                    //Chọn đường dẫn nơi lưu file
                    path = Path.Combine(Server.MapPath("~/Content/uploads/imgs/news"), filename);
                    image.SaveAs(path); //Lưu lại file vào đường dẫn đã chọn
                    news.image = filename; 
                }
                else //Người dùng không upload file thì set tên file mặc định là logo.png
                {
                    news.image = "logo.png";
                }
                news.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString()); //Lấy ngày hiện tại
                db.news.Add(news); //Thêm vào db và lưu lại
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(news);
        }

        // GET: admin/news/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: admin/news/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,name,image,description,detail,meta,hide,order,datebegin")] news news, HttpPostedFileBase image)
        {
            try
            {
                var path = "";
                var filename = "";
                news temp = getById(news.id);
                if (ModelState.IsValid)
                {
                    if (image != null) //Nếu người dùng có upload ảnh lên thì cập nhật ảnh mới, nếu không thì không cập nhật (giữ lại ảnh cũ)
                    {
                        filename = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + image.FileName;
                        path = Path.Combine(Server.MapPath("~/Content/uploads/imgs/news"), filename);
                        image.SaveAs(path);
                        temp.image = filename;
                    }

                    temp.name = news.name;
                    temp.description = news.description;
                    temp.detail = news.detail;
                    temp.meta = news.meta; 
                    temp.hide = news.hide;
                    temp.order = news.order;
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
            return View(news);
        }

        public news getById(long id)
        {
            return db.news.Where(x => x.id == id).FirstOrDefault();

        }

        // GET: admin/news/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: admin/news/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            news news = db.news.Find(id);
            db.news.Remove(news);
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
