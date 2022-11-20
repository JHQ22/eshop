using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using SnacksShop.Models;
using SnacksShop.App_Start;


namespace SnacksShop.Controllers
{
    [UserAuthentication]
    public class CarController : Controller
    {
        private SnacksShopDBEntities db = new SnacksShopDBEntities();
        // GET: Car

        public CarController()
        {
            ViewBag.Categories = db.Category.Include("Product").OrderBy(t => t.CateId).ToList();
        }

        public ActionResult Index()
        {
            Orders car = GetCar();
            car.Total = SumTotal();
            return View(car);
        }


        public ActionResult Order()
        {
            Orders car = GetCar();

            if (MyAuthentication.IsLoging())
            {
                int uid = int.Parse(MyAuthentication.GetUserId());
                Users user = db.Users.Include("Address").First(t => t.UserId == uid);

                if (car.OrdersDetail.Count == 0)
                {
                    TempData["msg"] = "Please select at least one item";
                }
                else
                {                  
                    ViewBag.Total = SumTotal();
                    TempData["Total"] = SumTotal();   
                }

                car.Users = user;
                car.AddressID = user.AddressID;
                car.Orderdate = DateTime.Now.Date;
                TempData["orderdtail"] = car.OrdersDetail;
                return View(car);
            }
            else
            {
                return Redirect("/Home/Login");
            }

        }

        [HttpPost]
        public ActionResult Order(Orders orders, OrdersDetail OrdersDetail)
        {
            int uId = int.Parse(MyAuthentication.GetUserId());
            List<OrdersDetail> o = TempData["orderdtail"] as List<OrdersDetail>;

            if (o.Count == 0)
            {
                TempData["msg"] = "Please select at least one item";
                return View();
            }

            var id = 0;

            orders.UserId = uId;
            orders.States = 0;
            orders.Orderdate = DateTime.Now.Date;
                                                 
            db.Orders.Add(orders);
            if (db.SaveChanges() > 0)
            {
                var data = db.Orders.Where(n => n.UserId == uId & n.Orderdate == orders.Orderdate & n.States == orders.States)
                    .OrderByDescending(t => t.OrdersID).FirstOrDefault();
                foreach (OrdersDetail item in o)
                {
                    OrdersDetail orders1 = new OrdersDetail()
                    {
                        OrdersID = item.OrdersID,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        States = item.States

                    };
                    orders1.OrdersID = data.OrdersID;
                    db.OrdersDetail.Add(orders1);
                    db.SaveChanges();
                }
                id = data.OrdersID;
            }

            if (id > 0)
            {
                return Redirect("~/Car/OrderPay/?id=" + id);
            }
            else
            {
                return View();
            }
        }

    
        public ActionResult OrderPay(int id, double? price)
        {
            
            if (price != null)
            {
                TempData["Total"] = price;
            }
            return View(db.Orders.Find(id));
        }

        [HttpPost]
        public ActionResult OrderPay(Orders o)
        {
            Orders order = db.Orders.Find(o.OrdersID);
            order.States = 1;
            //order.DeliveryDate = DateTime.Now;
            order.Orderdate = DateTime.Now;

            db.Entry(order).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MyOrders", "MyUser");
        }

        public ActionResult Add(int id, int quantity = 1)
        {
            if (!AddCar(id, quantity))
            {
                ViewBag.Error = "Inventory shortage";
                return View();
            }

            return Redirect("/Car/Index");
        }

        public ActionResult Del(int id)
        {
            DelCar(id);
            return RedirectToAction("Index");
        }

        public ActionResult Remove(int id)
        {
            RemoveCar(id);
            return RedirectToAction("Index");

        }

        public ActionResult Clear()
        {
            ClearCar();
            return RedirectToAction("Index");
        }


        private void ClearCar()
        {
            Orders car = GetCar();
            car.OrdersDetail.Clear();
            Session["car"] = car;
        }
    
        private void RemoveCar(int id)
        {
            Orders car = GetCar();
            OrdersDetail d = car.OrdersDetail.FirstOrDefault(t => t.ProductId == id);
            if (d != null)
            {
                car.OrdersDetail.Remove(d);
                Session["car"] = car;
            }
        }
        private void DelCar(int id)
        {
            Orders car = GetCar();
            OrdersDetail d = car.OrdersDetail.FirstOrDefault(t => t.ProductId == id);
            if (d != null && d.Quantity > 1)
            {
                d.Quantity -= 1;
                Session["car"] = car;
            }
        }
        private bool AddCar(int id, int quantity)
        {
            Product p = db.Product.Find(id);
            if (p.Stock < quantity)
            {
                return false;
            }

            Orders car = GetCar();
            OrdersDetail d = car.OrdersDetail.FirstOrDefault(t => t.ProductId == id);
            if (d != null)
            {
                d.Quantity += quantity;
            }
            else
            {

                d = new OrdersDetail();
                d.ProductId = id;
                d.Product = p;
                d.Quantity = 1;
                car.OrdersDetail.Add(d);
            }
            Session["car"] = car;
            return true;
        }
        public decimal SumTotal()
        {
            Orders car = GetCar();
            decimal sum = 0;
            foreach (OrdersDetail item in car.OrdersDetail)
            {
                sum += item.Quantity * item.Product.Price;
            }
            return sum;
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