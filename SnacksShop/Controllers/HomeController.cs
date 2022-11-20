using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using SnacksShop.App_Start;
using SnacksShop.Models;

using SnacksShop.ViewModel;

namespace SnacksShop.Controllers
{
    public class HomeController : Controller
    {
        private SnacksShopDBEntities db = new SnacksShopDBEntities();

        public HomeController()
        {
            ViewBag.Categories = db.Category.Include("Product").OrderBy(t => t.CateId).ToList();
        }

        public ActionResult Index()
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
            var products = query
                .Where(x => !string.IsNullOrEmpty(x.Photo))
                .OrderByDescending(x => x.PostTime).Take(20).ToList();

            ViewBag.Products = products;
            return View();
        }

        public ActionResult Category(int? id)
        {
            int cid = id ?? 1;
            ViewBag.category = db.Category.Find(id);
            return View(db.Product.Where(t => t.CateId == cid).ToList());
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string UserName,string Pwd)
        {
            if (ModelState.IsValid)
            {
                using(SnacksShopDBEntities db=new SnacksShopDBEntities())
                {
                    Users user = db.Users.FirstOrDefault(t => t.UserName == UserName && t.Pwd == Pwd);
                    if (user != null)
                    {
                        MyAuthentication.SetCookie(user.Nick, user.UserId.ToString(), "user");
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Incorrect username or password!");
                }
            }
            return View();
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Users user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using(SnacksShopDBEntities db=new SnacksShopDBEntities())
                    {
                        if (db.Users.Any(x => x.UserName == user.UserName))
                        {
                            ModelState.AddModelError("", "Registration failed! This username has been registered!");
                            return View();
                        }

                        db.Users.Add(user);
                        db.SaveChanges();
                        return Content("<script>alert('Registration successfull');window.location.href = '/Home/Login';</script>");
                    }
                }
                catch(Exception exp)
                {
                    ModelState.AddModelError("", "Registration failed!" + exp.Message);
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            MyAuthentication.LogOut();
            return RedirectToAction("Index");
        }
    }
}