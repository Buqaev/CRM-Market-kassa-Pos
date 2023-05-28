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
    public class FirmaController : Controller
    {
        // GET: Firma
        kassaEntities db = new kassaEntities();
        FirmaDto model = new FirmaDto();
        public ActionResult Index(int? Id,int? page,int? SelectId)
        {
            model.Firma = db.Firmas.SingleOrDefault(x => x.Id == Id);
            if (page == null)
            {
                page = 1;
            }
            int skip = ((int)page - 1) * 10;
            List<Firma> data = new List<Firma>();
            if (SelectId==null)
            {
                data = db.Firmas.ToList();

                
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                model.Firmas = data;
            }
            else
            {
                data = db.Firmas.Where(x=>x.Id==SelectId).ToList();


                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                model.Firmas = data;
            }


            //model.Firmas = db.Firmas.ToList();
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

            ViewBag.SelectId = SelectId;
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateFirma(Firma firma,Log log)
        {
           // Firma f = db.Firmas.FirstOrDefault(x => x.Id == firma.Id);

            if (firma.Balance ==null)
            {
                firma.Balance = 0;
            }

            db.Firmas.Add(firma);
            db.SaveChanges();


            string Id = Session["isId"].ToString();
            var ID = int.Parse(Id);
            log.UserId = ID;
            log.HereketId = firma.Id;
            log.Tarix = DateTime.Now;
            log.Description = firma.Ad + " " + "Adlı Yeni Firma Yaradıldı";
            db.Logs.Add(log);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult FirmaDetay(int Id)
        {
            model.Firma = db.Firmas.FirstOrDefault(x => x.Id == Id);

            return View(model);
        }

        public ActionResult Detay(int Id,decimal Mebleg,Cash cash)
        {
            Firma firma = db.Firmas.FirstOrDefault(x => x.Id == Id);
          
            firma.Balance -= Mebleg;
            db.SaveChanges();

            cash.Balance -= Mebleg;
            db.Cashs.Add(cash);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}