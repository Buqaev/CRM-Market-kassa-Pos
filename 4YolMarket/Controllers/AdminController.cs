using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _4YolMarket.Filters;
using _4YolMarket.Models;

namespace _4YolMarket.Controllers
{
    [LoggedUser]
    public class AdminController : Controller
    {
        // GET: Admin

        kassaEntities db = new kassaEntities();

        public ActionResult Index()
        {
            List<Admin> admins = db.Admins.ToList();

            return View(admins);
        }

        public ActionResult MelumatGetir(int Id)
        {
            Admin admin = db.Admins.FirstOrDefault(x => x.Id == Id);

            return View(admin);
        }

        public ActionResult Update(Admin a)
        {
            Admin admin = db.Admins.FirstOrDefault(x => x.Id == a.Id);
            admin.Ad = a.Ad;
            admin.Soyad = a.Soyad;
            admin.Email = a.Email;
            admin.Sifre = a.Sifre;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}