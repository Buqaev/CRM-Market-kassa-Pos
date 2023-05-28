using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _4YolMarket.Filters;
using _4YolMarket.Models;
using PagedList;
namespace _4YolMarket.Controllers
{
    [LoggedUser]
    public class LogsController : Controller
    {
        // GET: Logs
        kassaEntities db = new kassaEntities();
        public ActionResult Index(int? page)
        {
          
            if (page == null)
            {
                page = 1;
            }
            int skip = ((int)page - 1) * 10;
            List<Log> data = new List<Log>();
            List<Log> logs = db.Logs.OrderByDescending(x => x.Id).ToList();
            data = db.Logs.OrderByDescending(o => o.Id).ToList();
            ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
            ViewBag.Page = page;
            data = data.Skip(skip).Take(10).ToList();
            logs = data;
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
           
            if (ViewBag.TotalPage<= ViewBag.endPage)
            {
                ViewBag.endPage = currentPage;
            }
            if (ViewBag.TotalPage == currentPage)
            {
                ViewBag.endPage = currentPage;
            }
            if (ViewBag.TotalPage == currentPage+1)
            {
                ViewBag.endPage = currentPage+1;
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

            return View(logs);
        }
    }
}