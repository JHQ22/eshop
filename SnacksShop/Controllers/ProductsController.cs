using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SnacksShop.Models;
using SnacksShop.App_Start;
using Webdiyer.WebControls.Mvc;
using System.Net;
using System.Security.Cryptography;

using SnacksShop.ViewModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace SnacksShop.Controllers
{
    
    public class ProductsController : Controller
    {
        private SnacksShopDBEntities db = new SnacksShopDBEntities();
        // GET: Products
        [AdminAuthcntication]
        public ActionResult Index(int? id = 1)
        {
            var query = from a in db.Product.AsQueryable()
                        join c in db.Category
                        on a.CateId equals c.CateId
                        select new ProductModel
                        {
                            ProductId = a.ProductId,
                            Title = a.Title,
                            PostTime = a.PostTime,
                            Price = a.Price,
                            Stock = a.Stock,
                            MarketPrice = a.MarketPrice,
                            CateId = a.CateId,
                            Photo = a.ImageUrl,
                            CategoryName = c.CateName,
                            Content = a.Content
                        };
            query = query.OrderByDescending(x => x.PostTime);

            //分页
            int pageIndex = id ?? 1;
            PagedList<ProductModel> mPage = query.AsQueryable().ToPagedList(pageIndex, 8);
            return View(mPage);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product p = db.Product.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            List<OrdersDetail> details = db.OrdersDetail.Where(d => d.ProductId == p.ProductId).ToList();
            int totalSale = 0;
            foreach (OrdersDetail item in details)
            {
                totalSale += item.Quantity;
            }
            ViewBag.TotalSale = totalSale;
            return View(p);

        }

        [AdminAuthcntication]

        public ActionResult Edit(int id)
        {
            Product P = db.Product.Include("Category").Where(t => t.ProductId == id).FirstOrDefault();


            var data = db.Category.ToList();
            SelectList selectList = new SelectList(data, "CateId", "CateName", null);
            ViewBag.list = selectList;


            return View(P);

        }

        [AdminAuthcntication]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Product P, HttpPostedFileBase ImageUpload)
        {
            if (ImageUpload != null)
            {
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssss");
                P.ImageUrl = "/images/" + fileName + "_" + ImageUpload.FileName;
                ImageUpload.SaveAs(Server.MapPath(P.ImageUrl));
            }

            db.Entry<Product>(P).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Redirect("/Products/Index");
        }

        [AdminAuthcntication]

        public ActionResult Create()
        {

            int userId = Convert.ToInt32(MyAuthentication.GetUserId());
            var admusers = db.AdminUser.Where(u => u.SuId == userId).FirstOrDefault();
            if (admusers != null)
            {
                ViewBag.Name = admusers.UserName;
            }

            var data = db.Category.ToList();
            ViewData["CateId"] = new SelectList(data, "CateId", "CateName");
            return View();
        }

        [AdminAuthcntication]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product model, HttpPostedFileBase ImageUrl)
        {
            if (ImageUrl != null)
            {
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssss");
                model.ImageUrl = "/images/" + fileName + "_" + ImageUrl.FileName;
                ImageUrl.SaveAs(Server.MapPath(model.ImageUrl));
            }

            model.PostTime = DateTime.Now;
            db.Entry(model).State = EntityState.Added;

            try
            {
                db.SaveChanges();
            }
            catch
            {
                ((IObjectContextAdapter)db).ObjectContext.Refresh(RefreshMode.ClientWins, db.Product);
                db.SaveChanges();
            }
            return Content("<script>alert('Added successfully');window.location.href = '/Products/Index';</script>");
        }

        [AdminAuthcntication]

        public ActionResult Delete(int id)
        {
            var p = db.OrdersDetail.Where(n => n.ProductId == id).ToList();
            db.OrdersDetail.RemoveRange(p);
            db.SaveChanges();
            Product pro = db.Product.Where(n => n.ProductId == id).FirstOrDefault();
            db.Product.Remove(pro);
            db.SaveChanges();

            return Content("<script>alert('Successfully deleted');window.location.href = '/Products/Index';</script>");
        }

    }
}