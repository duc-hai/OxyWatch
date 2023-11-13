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
    public class productController : Controller
    {
        private OxyWatchEntities1 db = new OxyWatchEntities1();

        // GET: admin/product
        public ActionResult Index(long? id = null)
        {
            getCategory(id); //Lấy ra danh sách category
            return View();
        }
        public void getCategory(long? selectedId = null)
        {
            //Lấy ra danh sách category trong db dưới dạng một list select
            ViewBag.category = new SelectList(db.categories.Where(x => x.hide == true)
                .OrderBy(x => x.order), "id", "name", selectedId);
        }
        public ActionResult getProduct(long? id)
        {
            if (id == null) //Hiển thị toàn bộ sản phẩm
            {
                var v = db.products.OrderBy(x => x.order).ToList();
                return PartialView(v);
            }
            var m = db.products.Where(x => x.idCate == id).OrderBy(x => x.order).ToList();
            return PartialView(m);
        }


        // GET: admin/product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: admin/product/Create
        public ActionResult Create()
        {
            //ViewBag.idCate = new SelectList(db.categories, "id", "name");
            getCategory();
            return View();
        }

        // POST: admin/product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,name,firstImg,secondImg,oldPrice,newPrice,soluotban,meta,hide,order,datebegin,idCate,status,origin,warranty,caseMaterial,faceMaterial,energyUsed,waterResistant,faceSize,strapSize")] product product, HttpPostedFileBase firstImg, HttpPostedFileBase secondImg)
        {
            try
            {
                var path1 = "";
                var path2 = "";
                var filename1 = "";
                var filename2 = "";
                if (ModelState.IsValid)
                {
                    filename1 = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + firstImg.FileName;
                    path1 = Path.Combine(Server.MapPath("~/Uploads/products"), filename1);
                    firstImg.SaveAs(path1);
                    product.firstImg = filename1; 

                    filename2 = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + secondImg.FileName;
                    path2 = Path.Combine(Server.MapPath("~/Uploads/products"), filename2);
                    secondImg.SaveAs(path2);
                    product.secondImg = filename2;
                }
                else
                {
                    product.firstImg = "img1.png";
                    product.secondImg = "img2.png";
                }

                product.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                product.order = getMaxOrder(product.idCate);
                db.products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index", "product", new { id = product.idCate});
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(product);
        }

        public int getMaxOrder(long? CategoryId)
        {
            if (CategoryId == null)
                return 1; 
            return db.products.Where(x => x.idCate == CategoryId).Count();
        }

        // GET: admin/product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            getCategory(product.idCate);
            //ViewBag.idCate = new SelectList(db.categories, "id", "name", product.idCate);
            return View(product);
        }

        // POST: admin/product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,name,firstImg,secondImg,oldPrice,newPrice,soluotban,meta,hide,order,datebegin,idCate,status,origin,warranty,caseMaterial,faceMaterial,energyUsed,waterResistant,faceSize,strapSize")] product product, HttpPostedFileBase firstImg, HttpPostedFileBase secondImg)
        {
            try
            {
                var path1 = "";
                var path2 = "";
                var filename1 = "";
                var filename2 = "";

                product temp = db.products.Find(product.id);

                if (ModelState.IsValid)
                {
                    if (firstImg != null)
                    {
                        filename1 = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + firstImg.FileName;
                        path1 = Path.Combine(Server.MapPath("~/Uploads/products"), filename1);
                        firstImg.SaveAs(path1);
                        temp.firstImg = filename1;
                    }

                    if (secondImg != null)
                    {
                        filename2 = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss-") + secondImg.FileName;
                        path2 = Path.Combine(Server.MapPath("~/Uploads/products"), filename2);
                        secondImg.SaveAs(path2);
                        temp.secondImg = filename2;
                    }

                    temp.idCate = product.idCate;
                    temp.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    temp.name = product.name;
                    temp.order = product.order;
                    temp.oldPrice = product.oldPrice;
                    temp.newPrice = product.newPrice;
                    temp.soluotban = product.soluotban;
                    temp.meta = product.meta;
                    temp.hide = product.hide;
                    temp.status = product.status;
                    temp.origin = product.origin;
                    temp.warranty = product.warranty;
                    temp.caseMaterial = product.caseMaterial;
                    temp.faceMaterial = product.faceMaterial;
                    temp.energyUsed = product.energyUsed;
                    temp.waterResistant = product.waterResistant;
                    temp.faceSize = product.faceSize;
                    temp.strapSize = product.strapSize;

                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "product", new { id = product.idCate });
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
            return View(product);
        }

        // GET: admin/product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: admin/product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);
            db.products.Remove(product);
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
