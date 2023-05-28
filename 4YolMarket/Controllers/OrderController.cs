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
    public class OrderController : Controller
    {
        // GET: Order

        kassaEntities db = new kassaEntities();
        
        public ActionResult Index()
        {
            OrderDto model = new OrderDto();

            model.Orders = db.Orders.Where(x => x.Status == true).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult NewOrder(Order order)
        {

            int say = db.Orders.Where(x => x.Status == true).Count();

            if (say == 0)
            {

                order.Tarix = DateTime.Now;

                //order.Status = true;
                db.Orders.Add(order);
                db.SaveChanges();
            }

            return View();
        }
    }
}