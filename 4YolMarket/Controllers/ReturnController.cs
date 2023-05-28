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
    public class ReturnController : Controller
    {
        // GET: Return

        kassaEntities db = new kassaEntities();
        ReturnDto model = new ReturnDto();
        public ActionResult Index(int? SelectId,int? page)
        {
            model.Stocks = db.Stocks.Where(x=>x.Say_ceki_>0).OrderByDescending(x=>x.Id).ToList();
            if (page == null)
            {
                page = 1;
            }
            int skip = ((int)page - 1) * 10;
            List<Stock> data = new List<Stock>();
            if (SelectId == null)
            {
                data = db.Stocks.ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Where(x=> x.Say_ceki_ > 0).Skip(skip).Take(10).ToList();
                model.Stocks = data;
            }
            if (SelectId!=null)
            {
                data = db.Stocks.Where(x=>x.ProductId==SelectId&& x.Say_ceki_ > 0).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                model.Stocks = data;
            }
            
            ViewBag.Mehsul = db.Products.Where(x => x.Status == true).ToList();
            ViewBag.SelectId = SelectId;
            ViewBag.Products = db.Products.Where(x => x.Status == true).ToList();
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

        public ActionResult Getir(int? StoId)
        {
            Stock stock = db.Stocks.FirstOrDefault(x => x.Id == StoId);
            int ProId = stock.ProductId;
            PurchaseItem purchaseItem = db.PurchaseItems.FirstOrDefault(x => x.ProductId == ProId);
            ViewBag.qiymet = stock.AlisMebleg;
            ViewBag.mehsul =stock.ProductId;
            //ViewBag.Firma = db.Firmas.ToList();
            return View(stock);
            
        }
        public ActionResult Qaytar(int? Id,Return ret,Log log,int StoId)
        {
            Stock stock = db.Stocks.FirstOrDefault(x => x.Id == StoId);
            int ProId = stock.ProductId;
            PurchaseItem purchaseitem = db.PurchaseItems.FirstOrDefault(x => x.ProductId == ProId);
            string id = Session["isId"].ToString();
            var ID = int.Parse(id);
            ret.UserId = ID;
            ret.Tarix = DateTime.Now;
            ret.StockId = StoId;
            ret.FirmaId = stock.Purchase.FirmaId;
            db.Returns.Add(ret);
            db.SaveChanges();
            
            PurchaseItem purchaseItem = db.PurchaseItems.FirstOrDefault(x => x.ProductId == ProId);
            decimal qiymet = purchaseItem.AlisMebleg;
            log.UserId = ID;
            log.HereketId = ID;
            log.Tarix = DateTime.Now;
            log.Description ="Alış Qiyməti"+" "+stock.AlisMebleg+"AZN deyerinde olan"+" "+ret.Say+" "+"ədəd/kg Məhsul"+" "+ ret.Description+" "+"Geri Qaytarlmalıdır";
            db.Logs.Add(log);
            db.SaveChanges();
            
           
            return RedirectToAction("List");
        }

        public ActionResult List(DateTime? Tim,DateTime? Tim2,int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            int skip = ((int)page - 1) * 10;
            List<Return> returns = db.Returns.Where(x => x.Status == true || x.Status == null).ToList();
            ViewBag.firma = returns.Select(x => x.FirmaId).ToList();
            List<Return> data = new List<Return>();
            if (Tim!=null && Tim2!=null)
            {
                data = db.Returns.Where(x => x.Status != false && x.Tarix>=Tim && x.Tarix<=Tim2).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                returns = data;
            }
            if (Tim != null && Tim2 == null)
            {
                data = db.Returns.Where(x => x.Status != false && x.Tarix >= Tim).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                returns = data;
            }
            if (Tim == null && Tim2 != null)
            {
                data = db.Returns.Where(x => x.Status != false &&  x.Tarix <= Tim2).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                returns = data;
            }
            if (Tim==null&& Tim2==null)
            {
                data= db.Returns.Where(x => x.Status == true || x.Status == null).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                returns = data;
            }
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
            ViewBag.Tim = Tim;
            ViewBag.Tim2 = Tim2;
            


            return View(returns);
        }
         public ActionResult Tesdiq(int? retId,Log log,Cash cash,KassaEmeliyat kassaEmeliyat)
        {
            Return ret = db.Returns.FirstOrDefault(x => x.Id == retId);
            ret.Status = true;
            db.SaveChanges();
            int Sto = ret.StockId;
            int ProId = ret.Stock.ProductId;
            Product product = db.Products.FirstOrDefault(x => x.Id == ProId);
            PurchaseItem purchaseItem = db.PurchaseItems.FirstOrDefault(x => x.ProductId == ProId);
            cash.Balance += purchaseItem.AlisMebleg*ret.Say;
            db.Cashs.Add(cash);
            Stock stock = db.Stocks.FirstOrDefault(x => x.Id == Sto);
            stock.Say_ceki_ -= ret.Say;
            db.SaveChanges();
            string Id = Session["isId"].ToString();
            var ID = int.Parse(Id);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = retId;
            log.Description = "Alış Qiyməti" + " " + purchaseItem.AlisMebleg + "AZN deyerinde olan" + " " + ret.Say + " " + "ədəd/kg Məhsul" + " " + ret.Description + " " + "Geri qaytarlıması admin tərəfindən təsdiqlənmişdir";
            db.Logs.Add(log);
            db.SaveChanges();

            kassaEmeliyat.Tarix = DateTime.Now;
            kassaEmeliyat.Gelir_Xerc_ = true;
            kassaEmeliyat.Mebleg = purchaseItem.AlisMebleg * ret.Say;
            db.KassaEmeliyats.Add(kassaEmeliyat);
            db.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult Legv(int? retId, Log log)
        {
            Return ret = db.Returns.FirstOrDefault(x => x.Id == retId);
            ret.Status = false;
            db.SaveChanges();
            //int ProId = ret.StockId;
            Stock stock = db.Stocks.FirstOrDefault(x => x.Id == ret.StockId);
            int ProId = stock.ProductId;
            PurchaseItem purchaseItem = db.PurchaseItems.FirstOrDefault(x => x.ProductId == ProId);
            string Id = Session["isId"].ToString();
            var ID = int.Parse(Id);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = retId;
            log.Description = "Alış Qiyməti" + " " + purchaseItem.AlisMebleg + "AZN deyerinde olan" + " " + ret.Say + " " + "ədəd/kg Məhsul" + " " + ret.Description + " " + " geri qaytarlıması ADMİN tərəfindən qəbuledilməmişdir";//Qeyd edilməmiş səbəbə görə
            db.Logs.Add(log);
            db.SaveChanges();
            
            return RedirectToAction("List");
        }

        public ActionResult CustomerReturns(int? SelectId,int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            int skip = ((int)page - 1) * 10;
            List<Stock> data = new List<Stock>();
            if (SelectId==null)
            {
                data= db.Stocks.Where(x=>x.Say_ceki_>0).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                model.Stocks = data;
            }
            if (SelectId != null)
            {
                data = db.Stocks.Where(x=>x.ProductId==SelectId).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                model.Stocks = data;
            }

            ViewBag.Product = db.Products.Where(x => x.Status == true).ToList();
            ViewBag.SelectId = SelectId;
            ViewBag.Products = db.Products.Where(x => x.Status == true).ToList();
            //model.Stocks2 = db.Stocks.Where(x => x.Product.Status == true && x.Say_ceki_ > 0).ToList();

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

        public ActionResult Filter(int StoId)
        {
            Stock stock = db.Stocks.FirstOrDefault(x => x.Id == StoId);
            int ProId = stock.ProductId;
            Product product = db.Products.FirstOrDefault(x => x.Id == ProId);
           
        
            ViewBag.qiymet = stock.SalePrice;

            return View(stock);
        }

        public ActionResult AddReturns(int? Id, Return ret, Log log, int StoId, Cash cash)
        {

            Stock stock = db.Stocks.FirstOrDefault(x => x.Id == StoId);
            stock.Say_ceki_ += ret.Say;
            string id = Session["isId"].ToString();
            var ID = int.Parse(id);
            ret.UserId = ID;
            ret.Tarix = DateTime.Now;
            ret.StockId = StoId;
            ret.Status = true;
            db.Returns.Add(ret);
            db.SaveChanges();
            int ProId = stock.ProductId;
            PurchaseItem purchaseItem = db.PurchaseItems.FirstOrDefault(x => x.ProductId == ProId);
            decimal qiymet = purchaseItem.AlisMebleg;
            log.UserId = ID;
            log.HereketId = ID;
            log.Tarix = DateTime.Now;
            log.Description = "Qiyməti" + " " + stock.SalePrice + "AZN deyerinde olan" + " " + ret.Say + " " + "ədəd/kg Məhsul" + " " + ret.Description + " " + "Müştəri tərəfindən geri qaytarildi";
            db.Logs.Add(log);
            db.SaveChanges();
            cash.Balance -= stock.SalePrice * ret.Say;
            db.Cashs.Add(cash);
            db.SaveChanges();
            
            return RedirectToAction("CustomerReturns");
        }

    }
}