using SnacksShop.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SnacksShop.Controllers
{
    public class PayController : Controller
    {
        private SnacksShopDBEntities db = new SnacksShopDBEntities();
        // GET: Pay
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int id,double money)
        {
            Orders order = db.Orders.Find(id);
            order.States = 1;
            //order.DeliveryDate = DateTime.Now;
            order.Orderdate = DateTime.Now;

            var detail = db.OrdersDetail.FirstOrDefault(x => x.OrdersID == id);
            var product = db.Product.FirstOrDefault(x => x.ProductId == detail.ProductId);
            if (product.Stock < detail.Quantity)
            {
                ViewBag.Error = "Inventory shortage";
                return View();
            }

            product.Stock = product.Stock - detail.Quantity;
            db.Entry(product).State = System.Data.Entity.EntityState.Modified;
            db.Entry(order).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            ClearCar();
            return Redirect("/MyUser/MyOrders");
        }

        private void ClearCar()
        {
            Orders car = GetCar();
            car.OrdersDetail.Clear();
            Session["car"] = car;
        }

        public Orders GetCar()
        {
            Orders car;
            if (Session["car"] == null)
            {
                car = new Orders();
                car.OrdersDetail = new List<OrdersDetail>();
                Session["car"] = car;
            }
            else
            {
                car = (Orders)Session["car"];
            }
            return car;
        }
    }
}