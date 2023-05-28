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
    public class PurchaseController : Controller
    {
        // GET: Purchase

        kassaEntities db = new kassaEntities();
        AlisDto model = new AlisDto();

       
        public ActionResult Index(int?PurcId,decimal? Mebleg,int? FirmaId,DateTime? Tim,DateTime? Tim2, int? page)
        {

            if (page == null)
            {
                page = 1;
            }
            int skip = ((int)page - 1) * 10;
            List<Purchase> data = new List<Purchase>();

           
            data = db.Purchases.OrderByDescending(o => o.Id).ToList();


            ViewBag.CashBoxes = db.CashBoxs.Where(x => x.Status == true).ToList();

            model.Firmas = db.Firmas.ToList();

            

            
            if (FirmaId == null && Tim == null && Tim2 == null)
            {
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                

            }
            if (Tim!=null && Tim2==null)
            {
               
                data = db.Purchases.Where(x => x.Tarix >= Tim).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                

            }
            if (Tim==null && Tim2!=null)
            {
                data = db.Purchases.Where(x =>x.Tarix <= Tim2).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                

            }
            if (Tim!=null && Tim2!=null)
            {
                data = db.Purchases.Where(x => x.Tarix >= Tim && x.Tarix <= Tim2).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
               

            }
            if (Tim !=null && Tim2!=null && FirmaId!=null)
            {
                data = db.Purchases.Where(x => x.Tarix >= Tim && x.Tarix <= Tim2 && x.FirmaId==FirmaId).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                

            }

            if (PurcId != null)
            {
                Purchase purchase = db.Purchases.FirstOrDefault(x => x.Id == PurcId);
                purchase.OdenilenMebleg += (decimal)Mebleg;
                db.SaveChanges();
            }
           
            if (FirmaId != null)
            {
                data = db.Purchases.Where(x => x.Status == true&& x.FirmaId==FirmaId).ToList();
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

            ViewBag.FirmaId = FirmaId;
            ViewBag.Tarix = Tim;
            ViewBag.Tarix2 = Tim2;


            Session["Id"] = db.Products.Select(x => x.Id);
            model.PurchaseItems = db.PurchaseItems.Where(x => x.PurchaseId == x.Purchase.Id).ToList();
            ViewBag.firma = db.Firmas.ToList();
            ViewBag.product = db.Products.Where(x => x.Status == true).ToList();
            model.Purchases = data;
            return View(model);
        }

       

        public ActionResult BuyDelate(int Id,Log log)
        {
            Purchase purchase = db.Purchases.FirstOrDefault(x => x.Id == Id);
            var data = db.PurchaseItems.Where(x => x.PurchaseId == Id && x.Status == true).ToList();


            foreach (var x in data)
            {
       
                x.Status = false;
                db.SaveChanges();

            }

            purchase.Veziyyet = false;
            
            db.SaveChanges();

            string id = Session["isId"].ToString();
            var ID = int.Parse(id);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = Id;
            log.Description = "Toplam Məbləği" + " " + purchase.ToplamMebleg + " " + "AZN Dəyərində Məhsul Qəbul edilmədi (Təsdiqlənmədi)";
            db.Logs.Add(log);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

       

        [HttpGet]
        public ActionResult BuyList(int? Id,int? EmelyatId)
        {
            
            KassaEmeliyat kassaEmeliyat = db.KassaEmeliyats.FirstOrDefault(x => x.Id== EmelyatId);

            

            model.Purchase = db.Purchases.SingleOrDefault(x => x.Id == Id);
            model.PurchaseItems = db.PurchaseItems.Where(x => x.PurchaseId==Id && x.Status==true).ToList();
            if (EmelyatId!=null)
            {
                int? PurId = kassaEmeliyat.PurchaseId;
                model.PurchaseItems = db.PurchaseItems.Where(x => x.PurchaseId == PurId && x.Status == true).ToList();
            }
           
            ViewBag.Product = db.Products.Where(x => x.Status == true).ToList();
            return View(model);

            
        }

        [HttpPost]
        public ActionResult BuyItem(PurchaseItem purchaseItem,int? Id)
        {
          

            purchaseItem.Status = true;

            //ViewBag.firma = db.Firmas.ToList();
            //ViewBag.product = db.Products.Where(x=>x.Status==true).ToList();

            purchaseItem.Purchase = db.Purchases.SingleOrDefault(x=>x.Id==Id);
            db.PurchaseItems.Add(purchaseItem);
            db.SaveChanges();
            

            return RedirectToAction("Index");
        }

        public ActionResult Confirm(int Id,Log log,KassaEmeliyat emeliyat,Cash cash,int? firma,int? CashBoxId)
        {
            Purchase purchase = db.Purchases.FirstOrDefault(x => x.Id == Id);
            purchase.Veziyyet = true;
            db.SaveChanges();

            string id = Session["isId"].ToString();
            var ID = int.Parse(id);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = Id;
            log.Description = "Toplam Məbləği" + " " + purchase.ToplamMebleg + " " + "AZN Dəyərində Məhsul Anbara Qəbuledildi(Təsdiqləndi)";
            db.Logs.Add(log);
            db.SaveChanges();

            List<PurchaseItem> purchaseItems = db.PurchaseItems.Where(x => x.PurchaseId == Id).ToList();
            Purchase p = db.Purchases.FirstOrDefault(x => x.Id == Id);
            foreach (var item in purchaseItems)
            {
                //PurchaseItem purchaseItem = db.PurchaseItems.FirstOrDefault(x => x.PurchaseId == Id);

                Stock sto = new Stock();
                Stock stock = db.Stocks.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (stock!=null&&stock.AlisMebleg==item.AlisMebleg&&stock.Purchase.FirmaId==item.Purchase.FirmaId)
                {
                    
                    stock.Say_ceki_ += item.Say_Ceki_;
                    db.SaveChanges();
                }
                else /*if (stock==null)*/
                {
                    sto.ProductId = item.ProductId;
                    sto.AlisMebleg = item.AlisMebleg;
                    sto.PurchasesId = item.PurchaseId;
                    sto.Say_ceki_ = item.Say_Ceki_;
                    //decimal faiz= (item.AlisMebleg * 12) / 100;
                    sto.SalePrice =stock.SalePrice;
                    db.Stocks.Add(sto);
                    db.SaveChanges();
                }
                
                
                
            }
            if (p.OdenilenMebleg!=null&&p.OdenilenMebleg>0)
            {
                emeliyat.Tarix = DateTime.Now;
                emeliyat.PurchaseId = Id;
                emeliyat.Gelir_Xerc_ = false;
                emeliyat.Mebleg = p.OdenilenMebleg;
                db.KassaEmeliyats.Add(emeliyat);
                db.SaveChanges();

            }

            //Purchase purchase1 = new Purchase();
            Purchase purchase2 = db.Purchases.FirstOrDefault(x => x.Id == Id);
            cash.Balance -= purchase2.OdenilenMebleg;
            db.Cashs.Add(cash);
            db.SaveChanges();

            
            Firma data = db.Firmas.FirstOrDefault(x => x.Id == firma);
            data.Balance += purchase2.ToplamMebleg - purchase2.OdenilenMebleg;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ItemDelate(int Id)
        {
            PurchaseItem purchaseItem = db.PurchaseItems.FirstOrDefault(x => x.Id == Id);
            purchaseItem.Status = false;
            db.SaveChanges();

            return RedirectToAction("Index");
        }





        public ActionResult SaveOrder(int firma,string topmeb,decimal? odenmeb, PurchaseItem[] purchaseItems,int? id,KassaEmeliyat emeliyat,Cash cash,Log log)
        {
            string result = "Error! Order Is Not Complete!";
            Purchase model = new Purchase();
            if (odenmeb==null)
            {
                odenmeb = 0;
                
            }
            if (firma != null && topmeb!=null && odenmeb!=null && purchaseItems != null)
            {
                //var cutomerId = Guid.NewGuid();
                //Purchase model = new Purchase();
                //model.Id = cutomerId;
                model.Tarix = DateTime.Now;
                model.ToplamMebleg =decimal.Parse( topmeb);
                model.OdenilenMebleg =(decimal) odenmeb;
                model.FirmaId = firma;
                model.Status = true;
                
                db.Purchases.Add(model);
                db.SaveChanges();



                string Id = Session["isId"].ToString();
                var ID = int.Parse(Id);
                log.UserId = ID;
                log.Tarix = DateTime.Now;
                log.HereketId = model.Id;
                log.Description ="Toplam Məbləği"+" "+ model.ToplamMebleg + " " + "AZN Dəyərində Məhsul Təsdiqlənməsini Gözləyir";
                db.Logs.Add(log);
                db.SaveChanges();
                foreach (var item in purchaseItems)
                {
                    
                    PurchaseItem p = new PurchaseItem();
                    
                    p.PurchaseId = model.Id;
                    p.AlisMebleg = item.AlisMebleg;
                    p.IstehsalTarixi = item.IstehsalTarixi;
                    p.BitmeTarixi = item.BitmeTarixi;
                    p.ProductId = item.ProductId;
                    p.Say_Ceki_ = item.Say_Ceki_;
                    p.Status = true;
                    db.PurchaseItems.Add(p);
                    db.SaveChanges();
                    //Stock stock = db.Stocks.FirstOrDefault(x => x.ProductId == p.ProductId);
                    //stock.Say_ceki_ += item.Say_Ceki_;

                }
                db.SaveChanges();
                result = "Success! Order Is Complete!";
            }


            //db.KassaEmeliyats.Add(emeliyat);
            //db.SaveChanges();



            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //public JsonResult GetUserList()
        //{
        //    List<PurchaseViewModel> userList = db.PurchaseItems.Select(x => new PurchaseViewModel
        //    {
        //        Id = x.Id,
        //        AlisQiymeti = x.AlisMebleg,
        //        BitmeVaxti = x.BitmeTarixi,
        //        IsdehsalVaxti = x.IstehsalTarixi,
        //        ProductId=x.ProductId,
        //        Say=x.Say_Ceki_,

        //    }).ToList();



        //    return Json(userList, JsonRequestBehavior.AllowGet);
        //}
    }
}