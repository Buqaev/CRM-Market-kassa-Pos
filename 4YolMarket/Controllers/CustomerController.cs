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
    public class CustomerController : Controller
    {
        // GET: Customer
        kassaEntities db = new kassaEntities();
        CustDto model = new CustDto();
        
        public ActionResult Index(int? page,int? SelectId)
        {
            

            if (page == null)
            {
                page = 1;
            }
            int skip = ((int)page - 1) * 10;
            List<Customer> data = new List<Customer>();
            
            if (SelectId==null)
            {
                data = db.Customers.Where(x => x.Status == true).ToList();

                
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                model.Customers = data;
            }
            else
            {
                data = db.Customers.Where(x => x.Status == true&& x.Id==SelectId).ToList();

               
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                model.Customers = data;
            }
            model.Customers2 = db.Customers.Where(x => x.Status == true).ToList();
            ViewBag.SelectId = SelectId;

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

        [HttpGet]
        public ActionResult MusderiGetir(int Id)
        {
            Customer customer = db.Customers.FirstOrDefault(x => x.Id == Id);

            return View(customer);
        }

        public ActionResult Sil(Customer c,decimal Mebleg,Cash cash,Log log,KassaEmeliyat emeliyat)
        {
            Customer customer = db.Customers.FirstOrDefault(x => x.Id == c.Id);
            Order order = db.Orders.FirstOrDefault(x => x.CustomerId == c.Id);
            customer.Balance -=Mebleg;
            
            db.SaveChanges();
            cash.Balance += Mebleg;
            db.Cashs.Add(cash);
            db.SaveChanges();
            if (Mebleg > 0)
            {
                emeliyat.Tarix = DateTime.Now;
                if (order?.Id!=null)
                {
                    emeliyat.OrderId = order.Id;
                }
                
                emeliyat.Gelir_Xerc_ = true;
                emeliyat.Mebleg = Mebleg;
                db.KassaEmeliyats.Add(emeliyat);
                db.SaveChanges();
            }

            string Id = Session["isId"].ToString();
            var ID = int.Parse(Id);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = c.Id;
            log.Description = customer.Ad + " "+"Borcunun"+" " +Mebleg +" "+ "AZN-ı Ödədi";
            db.Logs.Add(log);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        public ActionResult AddCustomer(Customer customer)
        {
            customer.Status = true;
            db.Customers.Add(customer);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}