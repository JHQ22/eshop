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
    [UserAuthentication]
    public class AddressController : Controller
    {

        private SnacksShopDBEntities db = new SnacksShopDBEntities();
        // GET: Users
        public ActionResult Index(int? id = 1)
        {
            List<Address> list = db.Address.Include("Users").ToList();
            int pageIndex = id ?? 1;
            PagedList<Address> mPage = list.AsQueryable().ToPagedList(pageIndex, 5);
            return View(mPage);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Address Address = db.Address.Find(id);
            if (Address == null)
            {
                return HttpNotFound();
            }
            return View(Address);
        }

        public ActionResult Create()
        {
            int uID = int.Parse(MyAuthentication.GetUserId());
            Address Address = db.Address.FirstOrDefault(t => t.UserId == uID);
            ViewBag.User = db.Users.Include("Address").FirstOrDefault(t => t.UserId == uID);
            return View(Address);
        }

        [HttpPost]
        public ActionResult Create(Address Addressa)
        {
            Addressa.UserId = Convert.ToInt32(MyAuthentication.GetUserId());
            ViewBag.User = db.Users.Include("Address").FirstOrDefault(t => t.UserId == Addressa.UserId);
            db.Address.Add(Addressa);
            db.SaveChanges();
            return View(Addressa);
        }


        public ActionResult Edit(int id)
        {
            Address Address = db.Address.Where(t => t.AddressID == id).FirstOrDefault();
            return View(Address);

        }

        [HttpPost]
        public ActionResult Edit(Address Address)
        {
            db.Entry<Address>(Address).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Redirect("/Address/Create");
        }

        public ActionResult Delete(int? id)
        {
            var data = db.Address.Where(n => n.AddressID == id).ToList();
            db.Address.RemoveRange(data);
            db.SaveChanges();
            return Redirect("/Address/Create");

        }

        public ActionResult SetDefault(int? id)
        {
            int UserID = Convert.ToInt32(MyAuthentication.GetUserId());
            Users u = db.Users.Find(UserID);
            u.AddressID = id;
            db.SaveChanges();
            return Redirect("/Address/Create");
        }
    }
}