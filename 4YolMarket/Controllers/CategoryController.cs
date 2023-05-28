using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _4YolMarket.Filters;
using _4YolMarket.Models;
using Newtonsoft.Json;

namespace _4YolMarket.Controllers
{
    [LoggedUser]
    public class CategoryController : Controller
    {
        // GET: Category
        kassaEntities db = new kassaEntities();
        public ActionResult Index(int? page,int? SelectId)
        {
            if (page == null)
            {
                page = 1;
            }
            int skip = ((int)page - 1) * 10;
            List<Category> data = new List<Category>();
            if (SelectId==null)
            {
                data = db.Categories.Where(x => x.Status == true).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
            }
            else
            {
                data = db.Categories.Where(x => x.Status == true && x.Id==SelectId).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
            }
            ViewBag.SelectId = SelectId;
            int currentPage = page != null ? (int)page : 1;

            if (currentPage > 4)
            {
                ViewBag.startPage = currentPage - 4;
            }
            else
            {
                ViewBag.startPage = currentPage;
            }

            ViewBag.endPage = currentPage + 4;

            if (ViewBag.TotalPage <= ViewBag.endPage)
            {
                ViewBag.endPage = currentPage;
            }
            if (ViewBag.TotalPage == currentPage)
            {
                ViewBag.endPage = currentPage;
            }
            if (ViewBag.TotalPage == currentPage + 1)
            {
                ViewBag.endPage = currentPage + 1;
            }
            if (ViewBag.TotalPage == currentPage + 2)
            {
                ViewBag.endPage = currentPage + 2;
            }
            if (ViewBag.TotalPage == currentPage + 3)
            {
                ViewBag.endPage = currentPage + 3;
            }
            if (ViewBag.TotalPage == currentPage + 4)
            {
                ViewBag.endPage = currentPage + 4;
            }



            ViewBag.currentPage = currentPage;

            return View(data);
        }

        [HttpGet]
        public ActionResult CreateCategory()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(Category category,Log log)
        {
            category.Status = true;

            db.Categories.Add(category);
            db.SaveChanges();

            string Id = Session["isId"].ToString();
            var ID = int.Parse(Id);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = category.Id;
            log.Description = category.Ad+" "+ "Adlı Yeni Katiqorya Yaradıldı";
            db.Logs.Add(log);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delate(int Id,Log log)
        {
            Category category = db.Categories.FirstOrDefault(x => x.Id == Id);
            category.Status = false;

            db.SaveChanges();

            string id = Session["isId"].ToString();
            var ID = int.Parse(id);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = category.Id;
            log.Description = category.Ad+" "+ "Adlı Katiqorya Silindi";
            db.Logs.Add(log);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult UpdateCategory(int Id)
        {
            Category category = db.Categories.FirstOrDefault(x => x.Id == Id);

            return View(category);
        }

        public ActionResult Update(Category c,Log log)
        {
            Category category = db.Categories.FirstOrDefault(x => x.Id == c.Id);
            category.Ad = c.Ad;
            db.SaveChanges();
            string id = Session["isId"].ToString();
            var ID = int.Parse(id);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = category.Id;
            log.Description = category.Ad+" "+ "Adlı Katiqorya Yeniləndi";
            db.Logs.Add(log);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

       
    }
}