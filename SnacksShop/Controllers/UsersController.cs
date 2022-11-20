using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SnacksShop.Models;
using Webdiyer.WebControls.Mvc;


namespace SnacksShop.Controllers
{
    [AdminAuthcntication]
    public class UsersController : Controller
    {
        private SnacksShopDBEntities db = new SnacksShopDBEntities();
        // GET: Users
        public ActionResult Index(int? id = 1)
        {
            IList<Users> list = db.Users.ToList();
            int pageIndex = id ?? 1;
            PagedList<Users> mPage = list.AsQueryable().ToPagedList(pageIndex, 10);
            return View(mPage);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,UserName,Pwd,Email,Nick,Picture,AddressID")] Users users)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(x => x.UserName == users.UserName))
                {
                    ModelState.AddModelError("", "Registration failed! this username has been registered!");
                    return View();
                }

                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(users);
        }

        public ActionResult Edit(int id)
        {
       
            Users user = db.Users.Where(t => t.UserId == id).FirstOrDefault();
            return View(user);

        }

        [HttpPost]
        public ActionResult Edit(Users u)
        {
            db.Entry<Users>(u).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Redirect("/Users/Index");
        }


        public ActionResult Delete(int id)
        {
            Users u = db.Users.Find(id);
            db.Users.Remove(u);
            db.SaveChanges();
            return Redirect("/Users/Index");
        }
    }
}