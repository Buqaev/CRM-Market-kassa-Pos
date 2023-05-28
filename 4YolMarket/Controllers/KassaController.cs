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
    public class KassaController : Controller
    {
        // GET: Kassa
        kassaEntities db = new kassaEntities();
        KassaDto model = new KassaDto();

        public ActionResult Index()
        {
            List<Cash> cashes = db.Cashs.ToList();

            return View(cashes);
        }

        public ActionResult Expenses(int? Id)
        {
           
            model.Purchase = db.Purchases.FirstOrDefault(x => x.Id == Id);
            Purchase purchase = db.Purchases.FirstOrDefault(x => x.Id == Id);
            ViewBag.Borc = purchase.ToplamMebleg - purchase.OdenilenMebleg;
            ViewBag.CashBoxes = db.CashBoxs.Where(x => x.Status == true).ToList();


            db.SaveChanges();
            return View(model);
        }

        [HttpPost]
        public ActionResult Complete(decimal Mebleg, int Id,Cash cash,KassaEmeliyat emeliyat,int FirId,Log log,int? CashBoxId)
        {
            Purchase purchase = db.Purchases.FirstOrDefault(x => x.Id == Id);
            Firma firma = db.Firmas.FirstOrDefault(x => x.Id == FirId);
            emeliyat.Tarix = DateTime.Now;
            emeliyat.PurchaseId = Id;
            emeliyat.Gelir_Xerc_ = false;
            emeliyat.Mebleg = Mebleg;
            db.KassaEmeliyats.Add(emeliyat);
            db.SaveChanges();
            firma.Balance -= Mebleg;
            db.SaveChanges();
            CashBox cashBox = db.CashBoxs.FirstOrDefault(x => x.Id == CashBoxId);
            if (CashBoxId==null)
            {
                cash.Balance -= Mebleg;
                db.Cashs.Add(cash);
                db.SaveChanges();
            }

            if (CashBoxId!=null && cashBox.Balance>=Mebleg)
            {
                
                cashBox.Balance -= Mebleg;
                db.SaveChanges();
            }
            else if (CashBoxId != null && cashBox.Balance < Mebleg)
            {
                return RedirectToAction("Error");
            }
          


            string id = Session["isId"].ToString();
            var ID = int.Parse(id);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = Id;
            log.Description = "Qalan Borc" + " " +( purchase.ToplamMebleg-purchase.OdenilenMebleg) + " " + "AZN Dəyərində Olan Alışın Borcunun"+" "+Mebleg+" "+"AZN-ı Ödənildi";
            db.Logs.Add(log);
            db.SaveChanges();
            return RedirectToAction("Index", "Purchase",new { PurcId =Id, Mebleg= Mebleg });
        }

        public ActionResult Kassa()
        {
            ViewBag.ElaveXerc = db.CashBoxCategories.Where(x => x.Status == true).ToList();


            return View();
        }

        public ActionResult Pulcek(decimal? Mebleg,Log log)
        {
            
            Cash ch = new Cash();
            ch.Balance= ch.Balance -(decimal)Mebleg;
            db.Cashs.Add(ch);
            db.SaveChanges();

            //kassaEmeliyat.Gelir_Xerc_ = false;
            //kassaEmeliyat.Mebleg =(decimal) Mebleg;
            //kassaEmeliyat.Tarix = DateTime.Now;
            //db.KassaEmeliyats.Add(kassaEmeliyat);
            //db.SaveChanges();


            string id = Session["isId"].ToString();
            var ID = int.Parse(id);
            Admin admin = db.Admins.FirstOrDefault(x => x.Id == ID);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = ID;
            
            log.Description = "Kassadan Ümumi Məbləği" + " " + (Mebleg) + " " + "AZN Dəyərində Pul" + " " +admin.Ad + admin.Soyad  + " " + "tərəfindən götürüldü";
            db.Logs.Add(log);
            db.SaveChanges();
            


            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult Medaxil()
        {
            ViewBag.ElaveXerc = db.CashBoxCategories.Where(x => x.Status == true).ToList();
            ViewBag.Firma = db.Firmas.OrderByDescending(x => x.Id).ToList();
            return View();
        }
        public ActionResult Create(CashBoxLog cashBoxLog)
        {
            cashBoxLog.Tarix = DateTime.Now;
            db.CashBoxLogs.Add(cashBoxLog);
            db.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult List( DateTime? Tim, DateTime? Tim2, int? page)
        {
            CashBoxLogDto model = new CashBoxLogDto();


            if (page == null)
            {
                page = 1;
            }
            int skip = ((int)page - 1) * 10;
            List<CashBoxLog> data = new List<CashBoxLog>();


            data = db.CashBoxLogs.OrderByDescending(o => o.Id).ToList();


            ViewBag.CashBoxes = db.CashBoxs.Where(x => x.Status == true).ToList();

            //model.Firmas = db.Firmas.ToList();




            if ( Tim == null && Tim2 == null)
            {
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();


            }
            if (Tim != null && Tim2 == null)
            {

                data = db.CashBoxLogs.Where(x => x.Tarix >= Tim).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();


            }
            if (Tim == null && Tim2 != null)
            {
                data = db.CashBoxLogs.Where(x => x.Tarix <= Tim2).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();


            }
            if (Tim != null && Tim2 != null)
            {
                data = db.CashBoxLogs.Where(x => x.Tarix >= Tim && x.Tarix <= Tim2).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();


            }
            if (Tim != null && Tim2 != null)
            {
                data = db.CashBoxLogs.Where(x => x.Tarix >= Tim && x.Tarix <= Tim2).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();


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

           
            ViewBag.Tarix = Tim;
            ViewBag.Tarix2 = Tim2;


            
            //model.PurchaseItems = db.PurchaseItems.Where(x => x.PurchaseId == x.Purchase.Id).ToList();
            ViewBag.firma = db.Firmas.ToList();
            
            model.cashBoxLogs = data;
            return View(model);
        }

        public ActionResult Accept(int Id ,Log log)
        {
            CashBoxLog cashBoxLog = db.CashBoxLogs.FirstOrDefault(x => x.Id == Id);
            cashBoxLog.Status = true;
            db.SaveChanges();
            Cash ch = new Cash();
            if (cashBoxLog.CashBoxCategoryId != null&&cashBoxLog.FirmaId==null)
            {
                ch.Balance = ch.Balance - cashBoxLog.Mebleg;
            }
            if (cashBoxLog.CashBoxCategoryId == null&&cashBoxLog.FirmaId!=null)
            {
                ch.Balance = ch.Balance + cashBoxLog.Mebleg;
            }
            db.Cashs.Add(ch);
            db.SaveChanges();

            //kassaEmeliyat.Gelir_Xerc_ = false;
            //kassaEmeliyat.Mebleg =(decimal) Mebleg;
            //kassaEmeliyat.Tarix = DateTime.Now;
            //db.KassaEmeliyats.Add(kassaEmeliyat);
            //db.SaveChanges();


            string id = Session["isId"].ToString();
            var ID = int.Parse(id);
            Admin admin = db.Admins.FirstOrDefault(x => x.Id == ID);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = ID;
            if (cashBoxLog.CashBoxCategoryId!=null)
            {
                log.Description = "Kassadan Ümumi Məbləği" + " " + (cashBoxLog.Mebleg) + " " + "AZN Dəyərində Pul" + " " + cashBoxLog.CashBoxCategory.Name  + " " + "üçün götürüldü";

            }
            if (cashBoxLog.CashBoxCategoryId == null)
            {
                log.Description = "Kassaya Ümumi Məbləği" + " " + (cashBoxLog.Mebleg) + " " + "AZN Dəyərində Pul" + " " + cashBoxLog.Description + " " + "əlavə edildi";

            }
            db.Logs.Add(log);
            db.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult Delate(int Id)
        {
            CashBoxLog cashBoxLog = db.CashBoxLogs.FirstOrDefault(x => x.Id == Id);
            cashBoxLog.Status = false;
            db.SaveChanges();

            return RedirectToAction("List");
        }
        public ActionResult Error()
        {

            return View();
        }
    }
}