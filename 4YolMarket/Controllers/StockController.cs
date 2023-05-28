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
    public class StockController : Controller
    {
        // GET: Stock

        kassaEntities db = new kassaEntities();
        StoDto model = new StoDto();
        public ActionResult Index(string ad,int? page)
        {

            if (page == null)
            {
                page = 1;
            }
            int skip = ((int)page - 1) * 10;
            List<Stock> data = new List<Stock>();
            if (ad==null)
            {
                data = db.Stocks.Where(x=>x.Say_ceki_>0).OrderByDescending(x=>x.Id).ToList();
                ViewBag.product = db.Products.Where(x => x.Status == true).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                
            }
        
            if (ad != null)
            {
                data = db.Stocks.Where(x => x.Product.Ad == ad).ToList();
                ViewBag.product = db.Products.Where(x => x.Status == true).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                
            }
            ViewBag.seife = ad;
            model.Stocks = data;

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


            return View(model);
        }

        public ActionResult AddStock(Stock stock)
        {
            db.Stocks.Add(stock);
            db.SaveChanges();
            

            return RedirectToAction("Index");
        }

        public ActionResult Update(int Id,Stock s)
        {
            model.Stock = db.Stocks.FirstOrDefault(x => x.Id == Id);
            List<Stock> stock = db.Stocks.Where(x => x.ProductId == Id).ToList();
            foreach (var item in stock)
            {
                item.SalePrice = s.SalePrice;
                db.SaveChanges();
            }
            
            //return View(model);
            return RedirectToAction("Index");
        }


        //public JsonResult GetUserList()
        //{
        //    List<StockViewModel> userList = db.Stocks.Select(x => new StockViewModel
        //    {
                
        //        Id = x.Id,
        //        ProductId =x.ProductId,
        //        Say_ceki_ = x.Say_ceki_,
        //        SalePrice = x.SalePrice,
        //        Sekil=x.Product.Sekil

        //    }).ToList();



        //    return Json(userList, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult GetUserById(int Id)
        //{
        //    Stock model = db.Stocks.Where(x => x.Id == Id).SingleOrDefault();
        //    string value = string.Empty;
        //    value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
        //    {
        //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //    });

        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult SaveDateInDateBase(StockViewModel model, int? id)
        //{
        //    var result = false;

        //    try
        //    {
        //        if (model.Id > 0)
        //        {
        //            Stock sto = db.Stocks.SingleOrDefault(x => /*x.IsDelated == false && */x.Id == model.Id);
        //            sto.Say_ceki_ = model.Say_ceki_;
        //            //stu.DepartmenId = model.DepartmenId;

        //            db.SaveChanges();
        //            result = true;
        //        }
        //        //else
        //        //{
        //        //    User use = new User();
        //        //    use.Ad = model.Ad;
        //        //    //stu.DepartmenId = model.DepartmenId;
        //        //    db.Users.Add(use);
        //        //    db.SaveChanges();
        //        //    result = true;
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }


        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
    }
}