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
    public class DashboardController : Controller
    {
        // GET: Dashboard
        kassaEntities db = new kassaEntities();
        DashboardDto model = new DashboardDto();
        public ActionResult Index(int? page)
        {
            int say = db.Products.Where(x => x.Status == true).Count();
            ViewBag.mehsay = say;

            DateTime bugun = DateTime.Today;
            int SatisSay = db.Orders.Count(x => x.Tarix.Year == bugun.Year && x.Tarix.Month == bugun.Month && x.Tarix.Day == bugun.Day && x.ToplamMebleg>0);
            ViewBag.SatisSay = SatisSay;

            decimal? SatisMebleg = db.Orders.Where(x => x.Tarix.Year == bugun.Year&&x.Tarix.Month==bugun.Month&&x.Tarix.Day==bugun.Day).Sum(x =>(decimal?) x.ToplamMebleg);
            ViewBag.SatisMebleg = SatisMebleg;

            decimal? kassa = db.Cashs.Sum(x =>(decimal?) x.Balance);
            ViewBag.Kassa = kassa;

            decimal? borc = db.Customers.Sum(x => (decimal?)x.Balance);
            ViewBag.MusderiBorcu = borc;

            decimal? aySatisi = db.Orders.Where(x => x.Tarix.Year == bugun.Year && x.Tarix.Month == bugun.Month).Sum(x => (decimal?)x.ToplamMebleg);
            ViewBag.AyinSatisi = aySatisi;

            DateTime bugun1 = DateTime.Today;
            decimal? Qazanc = db.OrderItems.Where(x => x.Order.Tarix.Year == bugun.Year && x.Order.Tarix.Month == bugun.Month && x.Order.Tarix.Day == bugun.Day && x.Order.ToplamMebleg > 0 && x.Order.CustomerId == null).Select(z=>z.Qazanc).Sum();
           
            ViewBag.Qazanc = Qazanc;

            decimal? AyQazanc = db.OrderItems.Where(x => x.Order.Tarix.Year == bugun.Year && x.Order.Tarix.Month == bugun.Month &&  x.Order.ToplamMebleg > 0 ).Select(z => z.Qazanc).Sum();
            ViewBag.Ayqazanc = AyQazanc;

            decimal? NisyeQazanc = db.OrderItems.Where(x => x.Order.Tarix.Year == bugun.Year && x.Order.Tarix.Month == bugun.Month && x.Order.Tarix.Day == bugun.Day && x.Order.ToplamMebleg > 0 && x.Order.CustomerId != null).Select(z => z.Qazanc).Sum();
            ViewBag.NisyeQazanc = NisyeQazanc;

            decimal? UmumiQazanc = db.OrderItems.Where(x => x.Order.Tarix.Year == bugun.Year && x.Order.Tarix.Month == bugun.Month && x.Order.Tarix.Day == bugun.Day && x.Order.ToplamMebleg > 0 ).Select(z => z.Qazanc).Sum();
            ViewBag.UmumiQazanc = UmumiQazanc;

            decimal? Pashabank = db.CashBoxs.Where(x => x.Status == true).Select(x => x.Balance).Sum();
            ViewBag.PashaBankBalance = Pashabank;

            if (page == null)
            {
                page = 1;
            }
            int skip = ((int)page - 1) * 7;
            List<Stock> data = new List<Stock>();
            data = db.Stocks.Where(x => x.Say_ceki_ <= (decimal?)2.5).ToList();

            ViewBag.TotalPage = Math.Ceiling(data.Count / 7.00);
            ViewBag.Page = page;
            data = data.Skip(skip).Take(7).ToList();
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
    }
}