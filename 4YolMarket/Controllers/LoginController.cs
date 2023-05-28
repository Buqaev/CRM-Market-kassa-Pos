using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using _4YolMarket.Filters;
using _4YolMarket.Models;
namespace _4YolMarket.Controllers
{
    
    public class LoginController : Controller
    {
        // GET: Login
        kassaEntities db = new kassaEntities();

        //[AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        //[AllowAnonymous]
        [HttpPost]
        public ActionResult Giris(String Mail, String Sifre,Log log)
        {
            Admin admin = db.Admins.FirstOrDefault(x => x.Email == Mail && x.Sifre == Sifre);
            User user = db.Users.FirstOrDefault(x => x.Email == Mail && x.Sifre == Sifre && x.Status==true);
            if (admin != null)
            {
                FormsAuthentication.SetAuthCookie(admin.Ad, false);
                Session["Id"] = admin.Id.ToString();
                Session["isId"] = admin.Id.ToString();
                Session["Ad"] = admin.Ad.ToString();
                Session["Soyad"] = admin.Soyad.ToString();

                Session["LoggedUser"] = admin;

                string id = Session["isId"].ToString();
                var ID = int.Parse(id);
                log.UserId = ID;
                log.Tarix = DateTime.Now;
                log.HereketId = admin.Id;
                log.Description =admin.Ad +" "+"Sistemə Daxiloldu";
                db.Logs.Add(log);
                db.SaveChanges();

                return RedirectToAction("Index", "Product");
            }
           

            else if  (user !=null)
            {
                FormsAuthentication.SetAuthCookie(user.Ad, false);
                Session["UId"] = user.Id.ToString();

                Session["isId"] = user.Id.ToString();

                Session["Ad"] = user.Ad.ToString();
                Session["Soyad"] = user.Soyad.ToString();

                Session["LoggedUser"] = user;

                string id = Session["isId"].ToString();
                var ID = int.Parse(id);
                log.UserId = ID;
                log.Tarix = DateTime.Now;
                log.HereketId = user.Id;
                log.Description = user.Ad + " " + "Sistemə Daxiloldu";
                db.Logs.Add(log);
                db.SaveChanges();

                return RedirectToAction("Order", "KassaPos");
            }

            else
            {


                return RedirectToAction("Index", "Login");
            }

        }

        [LoggedUser]
        public ActionResult LoginOut(Log log)
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session["LoggedUser"] = null;

            string id = Session["isId"].ToString();
            var ID = int.Parse(id);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = ID;
            log.Description = "Sistemdən Ayrıldı";
            db.Logs.Add(log);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}