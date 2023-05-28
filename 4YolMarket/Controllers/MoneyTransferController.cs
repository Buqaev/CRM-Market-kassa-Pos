using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _4YolMarket.Models;
using _4YolMarket.Filters;
namespace _4YolMarket.Controllers
{
    [LoggedUser]
    public class MoneyTransferController : Controller
    {
        // GET: MoneyTransfer

        kassaEntities db = new kassaEntities();
        MonyTransferDto model = new MonyTransferDto();
        public ActionResult Index()
        {
            ViewBag.CashBox = db.CashBoxs.Where(x => x.Status == true).ToList();
            

            return View();
        }

        public ActionResult List(int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            int skip = ((int)page - 1) * 10;
            List<MoneyTransfer> moneyTransfers = db.MoneyTransfers.ToList();

            List<MoneyTransfer> data = new List<MoneyTransfer>();

            
                data = db.MoneyTransfers.OrderByDescending(x=>x.Id).ToList();


                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                model.moneyTransfers = data;
            

            //model.moneyTransfers = db.MoneyTransfers.OrderByDescending(x => x.Id).ToList();
            //ViewBag.SelectId = SelectId;

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

        public ActionResult Transfer(MoneyTransfer mt,int? CashBoxId101, int? CashBoxId102)
        {
            MoneyTransfer money = new MoneyTransfer();
            money.Mebleg = mt.Mebleg;
            money.FromCashBoxId = mt.FromCashBoxId;
            money.ToCashBoxId = mt.ToCashBoxId;
            db.MoneyTransfers.Add(money);
            db.SaveChanges();

            return RedirectToAction("List");
        }
        public ActionResult Accept(int Id, Log log)
        {
            MoneyTransfer mt = db.MoneyTransfers.FirstOrDefault(x => x.Id == Id);
            
            //CashBox tocashBox = db.CashBoxs.FirstOrDefault(x => x.Id == mt.ToCashBoxId);
            decimal negdbalance = db.Cashs.Select(x => x.Balance).Sum();

            if (mt.FromCashBoxId == null && mt.ToCashBoxId!=null)
            {
                CashBox cashBox = db.CashBoxs.FirstOrDefault(x => x.Id == mt.ToCashBoxId);

                if (negdbalance < mt.Mebleg)
                {

                    return RedirectToAction("Error");
                }
                //if (negdbalance >= mt.Mebleg)
                else
                {
                   
                    Cash cash = new Cash();
                    cash.Balance -= mt.Mebleg;
                    db.Cashs.Add(cash);
                    cashBox.Balance += mt.Mebleg;
                    mt.Status = true;
                    db.SaveChanges();
                    string id = Session["isId"].ToString();
                    var ID = int.Parse(id);
                    log.UserId = ID;
                    log.Tarix = DateTime.Now;
                    log.HereketId = Id;
                    log.Description =  mt.Mebleg + " " + "AZN NƏĞD HESABDAN KART HESABINA köçürüldü";
                    db.Logs.Add(log);
                    db.SaveChanges();



                    return RedirectToAction("List");
                }
            }
            if (mt.ToCashBoxId==null&& mt.FromCashBoxId != null)
            {
                CashBox cashBox = db.CashBoxs.FirstOrDefault(x => x.Id == mt.FromCashBoxId);

                if (cashBox.Balance < mt.Mebleg)
                {

                    return RedirectToAction("Error2");
                }
                //if (negdbalance >= mt.Mebleg)
                else
                {
                   
                  
                    cashBox.Balance -= mt.Mebleg;
                    mt.Status = true;
                    db.SaveChanges();
                    Cash cash = new Cash();
                    cash.Balance += mt.Mebleg;
                    db.Cashs.Add(cash);
                    db.SaveChanges();
                    string id = Session["isId"].ToString();
                    var ID = int.Parse(id);
                    log.UserId = ID;
                    log.Tarix = DateTime.Now;
                    log.HereketId = Id;
                    log.Description = mt.Mebleg + " " + "AZN KART HESABDAN NƏĞD HESABINA  köçürüldü";
                    db.Logs.Add(log);
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
            }

            return RedirectToAction("List");


        }
        public ActionResult Delate(int Id)
        {
            MoneyTransfer mt = db.MoneyTransfers.FirstOrDefault(x => x.Id == Id);
            mt.Status = false;
            db.SaveChanges();

            return RedirectToAction("List");
        }
        public ActionResult Error()
        {

            return View();
        }

        public ActionResult Error2()
        {

            return View();
        }

    }
}