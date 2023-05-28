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
    public class CheckoutOperationController : Controller
    {
        // GET: CheckoutOperation
        kassaEntities db = new kassaEntities();
        public ActionResult Index(int? page, int? SelsctId,DateTime?Tim1,DateTime?Tim2)
        {
            //List<KassaEmeliyat> kassaEmeliyats = db.KassaEmeliyats.ToList();


            if (page == null)
            {
                page = 1;
            }
            int skip = ((int)page - 1) * 10;
            List<KassaEmeliyat> data = new List<KassaEmeliyat>();
            if (Tim1==null && Tim2==null)
            {
                data = db.KassaEmeliyats.OrderByDescending(o => o.Id).ToList();

                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                
            }
            if (Tim1!=null&&Tim2!=null)
            {
                data = db.KassaEmeliyats.Where(x=>x.Tarix>=Tim1&&x.Tarix<=Tim2).OrderByDescending(o => o.Id).ToList();

                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
            }
            if (Tim1!=null && Tim2==null)
            {
                data = db.KassaEmeliyats.Where(x => x.Tarix >= Tim1).OrderByDescending(o => o.Id).ToList();

                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
            }
            if (Tim1 == null && Tim2 != null)
            {
                data = db.KassaEmeliyats.Where(x =>  x.Tarix <= Tim2).OrderByDescending(o => o.Id).ToList();

                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
            }
            ViewBag.Tim1 = Tim1;
            ViewBag.Tim2 = Tim2;
            ViewBag.SelsctId = SelsctId;
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
    }
}