using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SnacksShop.App_Start;
using SnacksShop.Models;
using Webdiyer.WebControls.Mvc;


namespace SnacksShop.Controllers
{
    [UserAuthentication]
    public class MyUserController : Controller
    {
        private SnacksShopDBEntities db = new SnacksShopDBEntities();
        public ActionResult MyInfo()
        {
            if (string.IsNullOrEmpty(MyAuthentication.GetUserId()))
            {
                return Redirect("/Home/Login");
            }

            int id = int.Parse(MyAuthentication.GetUserId());
            Users users = db.Users.Find(id);
            if (users != null)
            {
                return View(users);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Myinfo([Bind(Include ="UserId,UserName,Pwd,Email,Nick,Picture,AddressID")] Users user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("/Home/Myinfo");
            }
            return View(user);
        }


        public ActionResult ChangePwd()
        {
            int id = int.Parse(MyAuthentication.GetUserId());
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePwd(int UserId,string OldPwd,string NewPwd,string NewPwd2) 
        {
            Users user = db.Users.Find(UserId);
            if (ModelState.IsValid)
            {
                if (OldPwd != user.Pwd)
                {
                    ModelState.AddModelError("", "The original password is incorrect");
                }else if (NewPwd != NewPwd2)
                {
                    ModelState.AddModelError("", "Passwords do not match");
                }
                else
                {
                    user.Pwd = NewPwd;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    ModelState.AddModelError("", "Password reset complete");
                }
            }
            return View(user);
        }

        public ActionResult Confirm(int id)
        {
            Orders order = db.Orders.Find(id);
            order.DeliveryDate = DateTime.Now;
            order.States = 3;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MyOrders");
        }

        public ActionResult MyOrders(int? id,int? states)
        {
            List<Orders> list;
            int uid = int.Parse(MyAuthentication.GetUserId());
            if (states == null)
            {
                list = db.Orders.Include(t => t.Address)
                    .Where(t => t.UserId == uid).OrderByDescending(t=>t.OrdersID).ToList();
            }
            else
            {
                list = db.Orders.Where(t => t.States == states).Include(t => t.Address).Where(t=>t.UserId==uid).OrderByDescending(t=>t.OrdersID).ToList();

            }
            int pageIndex = id ?? 1;
            PagedList<Orders> mPage = list.AsQueryable().ToPagedList(pageIndex, 5);
            return View(mPage);
        }
    }


}