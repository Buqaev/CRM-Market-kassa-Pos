using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _4YolMarket.Filters;
using _4YolMarket.Models;
namespace _4YolMarket.Controllers
{
    [LoggedUser]
    public class UserController : Controller
    {
        // GET: User
        kassaEntities db = new kassaEntities();
        public ActionResult Index()
        {
            List<User> users = db.Users.Where(x=>x.Status==true).ToList();
          
            
            return View(users);
        }

        [HttpGet]
        public ActionResult CreateUser()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(User user, HttpPostedFileBase Sekil,Log log)
        {
            user.Status = true;

            if (Sekil != null)
            {
                string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(Sekil.FileName));
                Sekil.SaveAs(path);
                user.Sekil = Path.GetFileName(Sekil.FileName);

            }
            if (user.Sekil==null)
            {
                user.Sekil = "Test";
            }
           
            db.Users.Add(user);
            db.SaveChanges();
            string Id = Session["isId"].ToString();
            var ID = int.Parse(Id);
            
            log.UserId = ID;
            log.HereketId = ID;
            log.Tarix = DateTime.Now;
            log.Description = "Yeni isdifadəçi yaratdı";
            db.Logs.Add(log);
            db.SaveChanges();
            return RedirectToAction("Index");

            
        }

        public ActionResult Delate(int Id)
        {
            User user = db.Users.FirstOrDefault(x => x.Id == Id);
            user.Status = false;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}