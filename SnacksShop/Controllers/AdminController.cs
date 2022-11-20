using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SnacksShop.App_Start;

using SnacksShop.Models;
using Webdiyer.WebControls.Mvc;

namespace SnacksShop.Controllers
{
    public class AdminController : Controller
    {
        private SnacksShopDBEntities db = new SnacksShopDBEntities();

        [AdminAuthcntication]
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Admins(int? id = 1)
        {
            var list = db.AdminUser.ToList();
            int pageIndex = id ?? 1;         
            PagedList<AdminUser> mPage = list.AsQueryable().ToPagedList(pageIndex, 5);
            return View(mPage);
        }

        [HttpPost]
        public ActionResult Login(string UserName,string Pwd)
        {
            if (ModelState.IsValid)
            {
                AdminUser admin = db.AdminUser.FirstOrDefault(t => t.UserName == UserName && t.Pwd == Pwd);
                if (admin != null)
                {
                    MyAuthentication.SetCookie(admin.UserName, admin.SuId.ToString(), "admin");
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Incorrect username or password");
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