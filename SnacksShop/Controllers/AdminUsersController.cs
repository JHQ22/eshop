using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SnacksShop.Models;

namespace SnacksShop.Controllers
{
    [AdminAuthcntication]
    public class AdminUsersController : Controller
    {

        private SnacksShopDBEntities db = new SnacksShopDBEntities();

        // GET: AdminUsers
        public ActionResult Index()
        {
            return View(db.AdminUser.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                return View(db.AdminUser.Find(id));
            }
            return View();
        }



        public ActionResult Create()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Admin1", Value = "1" });
            list.Add(new SelectListItem { Text = "Admin2", Value = "0" });
            ViewData["RoleList"] = list;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName,Pwd,role")] AdminUser admin)
        {
            if (ModelState.IsValid)
            {
                if (db.AdminUser.Any(x => x.UserName == admin.UserName))
                {
                    ModelState.AddModelError("", "This username has been registered!");
                    return View();
                }

                db.AdminUser.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        public ActionResult Edit(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Admin1", Value = "1" });
            list.Add(new SelectListItem { Text = "Admin2", Value = "0" });
            ViewData["RoleList"] = list;

            AdminUser user = db.AdminUser.Where(t => t.SuId == id).FirstOrDefault();
            return View(user);

        }

        [HttpPost]
        public ActionResult Edit(AdminUser u)
        {
            db.Entry<AdminUser>(u).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Redirect("/AdminUsers/Index");
        }


        public ActionResult Delete(int id)
        {
            AdminUser u = db.AdminUser.Find(id);
            db.AdminUser.Remove(u);
            db.SaveChanges();
            return Redirect("/AdminUsers/Index");

        }
    }
}