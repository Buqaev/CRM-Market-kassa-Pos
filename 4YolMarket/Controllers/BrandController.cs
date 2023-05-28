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
    public class BrandController : Controller
    {
        // GET: Brand
        kassaEntities db = new kassaEntities();
        public ActionResult Index(int? page,int? SelectId)
        {
            if (page == null)
            {
                page = 1;
            }
            int skip = ((int)page - 1) * 10;
            List<Brand> data = new List<Brand>();
            if (SelectId==null)
            {
                data = db.Brands.Where(x => x.Status == true).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
            }
            else
            {
                data = db.Brands.Where(x => x.Status == true&&x.Id==SelectId).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
            }
            
            ViewBag.product = db.Products.Where(x => x.Status == true).ToList();
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

        [HttpPost]
        public ActionResult UpdateBrand(Brand brand,Log log)
        {
            brand.Status = true;
            db.Brands.Add(brand);
            db.SaveChanges();

            string Id = Session["isId"].ToString();
            var ID = int.Parse(Id);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = brand.Id;
            log.Description = "Yeni Brend Yaradıldı";
            db.Logs.Add(log);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delate(int Id,Log log)
        {
            Brand brand = db.Brands.FirstOrDefault(x => x.Id == Id);
            brand.Status = false;
            db.SaveChanges();

            string id = Session["isId"].ToString();
            var ID = int.Parse(id);
            log.UserId = ID;
            log.HereketId = brand.Id;
            log.Tarix = DateTime.Now;
            log.Description = "Brend Silindi";
            db.Logs.Add(log);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetUserById(int UserId)
        {
            User model = db.Users.Where(x => x.Id == UserId).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(int Id,Brand br,Log log)
        {
            Brand brand = db.Brands.FirstOrDefault(x => x.Id == Id);
            brand.Ad = br.Ad;

            string id = Session["isId"].ToString();
            var ID = int.Parse(id);
            log.UserId = ID;
            log.HereketId = brand.Id;
            log.Tarix = DateTime.Now;
            log.Description = "Brendin məlumatları yeniləndi";
            db.Logs.Add(log);
            db.SaveChanges();

            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}