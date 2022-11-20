using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SnacksShop.Models;
using SnacksShop.App_Start;
using Webdiyer.WebControls.Mvc;


namespace SnacksShop.Controllers
{
    public class OrdersController : Controller
    {
        private SnacksShopDBEntities db = new SnacksShopDBEntities();

        public OrdersController()
        {
            ViewBag.Categories = db.Category.Include("Product").OrderBy(t => t.CateId).ToList();
        }


        public ActionResult Index(int? states, int? id = 1)
        {
            var query = db.Orders.Include("Address").Include("Users").AsQueryable();
            if (states != null)
            {
                query = query.Where(t => t.States == states);
            }
            var list = query.OrderByDescending(t => t.Orderdate).ToList();

            int pageIndex = id ?? 1;
            PagedList<Orders> mPage = list.AsQueryable().ToPagedList(pageIndex, 8);

            return View(mPage);
        }


  
        public ActionResult Details(int id)
        {
            Orders o = db.Orders.Include("OrdersDetail")
                .Where(t => t.OrdersID == id).FirstOrDefault();
            return View(o);
        }

        public ActionResult Send(int id)
        {
            Orders o = db.Orders.Find(id);
            o.States = 2;
            db.SaveChanges();
            return Redirect("/Orders/Index");
        }

        public ActionResult Close(int id)
        {
            Orders o = db.Orders.Find(id);
            o.States = 4;
            db.SaveChanges();
            return Redirect("/Orders/Index");
            
        }
    }
}