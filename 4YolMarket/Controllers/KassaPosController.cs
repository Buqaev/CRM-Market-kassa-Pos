using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _4YolMarket.Filters;
using _4YolMarket.Models;
namespace _4YolMarket.Controllers
{
    [LoggedUser]
    public class KassaPosController : Controller
    {
        // GET: KassaPos
        kassaEntities db = new kassaEntities();
        public ActionResult Index(int OrdId,int? PrId,string axtar,int? CustId)
        {

            List<OrderItem> OrderItems = db.OrderItems.Where(x=>x.Order.Id==OrdId && x.Status == true).ToList();
            Order order = new Order();

            PosDto model = new PosDto();

            model.Order = db.Orders.FirstOrDefault(x => x.Id == OrdId);
            //model.Products = db.Products.Where(x => x.Status == true).ToList();
            model.Stocks = db.Stocks.Where(x => x.Product.Status == true && x.Say_ceki_>0).ToList();
            model.OrderItemss = db.OrderItems.Where(x => x.Order.Id == OrdId && x.Status==true).ToList();
            model.Customer = db.Customers.FirstOrDefault(x => x.Status == true);
            Session["PrId"] = PrId;
            if (OrderItems.Count() != 0)
            {

                OrderItem item = db.OrderItems.FirstOrDefault(x => x.OrderId == OrdId /*&& x.ProductId== PrId*/ && x.Status == true);
                //OrderItem item3 = db.OrderItems.FirstOrDefault(x => x.OrderId == OrdId && x.ProductId== PrId && x.Status==false);
               
                //if (item==null)
                //{
                //    OrderItem item2 = db.OrderItems.FirstOrDefault(x => x.OrderId == OrdId);
                //    //item2.MehsulMeblegi = item.Say * item.SatisMeblegi;
                //}
                //else
                //{
                //    //if (item.Status == true)

                //    //{
                //        item.MehsulMeblegi = item.Say * item.SatisMeblegi;
                        
                //        db.SaveChanges();
                //        decimal cem = (decimal)(from x in db.OrderItems.Where(x => x.OrderId == OrdId && x.Status == true) select x.MehsulMeblegi).Sum();
                //        ViewBag.ToplamMebleg = cem;
                //    //}
                //}
                decimal mebleg = (decimal)(from x in db.OrderItems.Where(x => x.Order.Id == OrdId && x.Status == true ) select x.MehsulMeblegi).Count();


                decimal say = db.OrderItems.Where(x => x.Order.Id == OrdId && x.Status == true).Count();
                //ViewBag.cesidSayi = (from x in db.ShoppingCarts.Where(x => x.UserId == ID && x.Status == true) select x.Say).Sum();
                if (mebleg>0)
                {

                    /*item.Order.ToplamMebleg */
                    decimal cem= (decimal)(from x in db.OrderItems.Where(x => x.Order.Id == OrdId &&x.Status==true) select x.MehsulMeblegi).Sum();
                    
                    //ViewBag.ToplamMebleg = cem;
                    Session["ToplamMebleg"] = cem;
                    model.OrderItem = item;
                    db.SaveChanges();
                }
            
            }

            List<OrderItem> data = db.OrderItems.Where(x => x.OrderId == OrdId  && x.Status == true).ToList();
            ViewBag.cesidSay = data.Select(x=>x.Say).Sum();
            List<OrderItem> data2 = db.OrderItems.Where(x => x.OrderId == OrdId && x.Status == true).ToList();
            ViewBag.mehsulSay = data2.Select(x => x.Id).Count();
            if (axtar==null)
            {
                ViewBag.musderiler = db.Customers.OrderByDescending(x=>x.Status==true).ToList();

            }
            else if (axtar != null)
            {
                ViewBag.musderiler = db.Customers.Where(x => x.Status == true&& x.Ad==axtar&&x.Soyad==axtar).ToList();
            }

            return View(model);
        }

        

        public ActionResult Order(DateTime? tarix,int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            int skip = ((int)page - 1) * 10;

            List<Order> data = new List<Order>();
            PosDto model = new PosDto();
            if (tarix == null)
            {
                data = db.Orders.Where(x=>x.Status==true).OrderByDescending(x=>x.Id).ToList();
                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
            }

           


            else if (tarix != null)
            {

                CultureInfo invariant = CultureInfo.InvariantCulture;
                string dateString = tarix.Value.ToString("yyyy/MM/dd", invariant);

                //DateTime date = DateTime.Parse(dateString);
                List< Order> order = db.Orders.Where(x => x.Status == true).ToList();
                string date= order.Select(x => x.Tarix).ToString();

                data = db.Orders.Where(x => x.Tarix.Year ==tarix.Value.Year && x.Tarix.Month==tarix.Value.Month&& x.Tarix.Day==tarix.Value.Day).ToList();

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

            model.Orders = data;
            ViewBag.tarix = tarix;
            
            return View(model);
        }

        public ActionResult OrderAdd()
        {
            

            Order order = new Order();
            order.Tarix = DateTime.Now;

                
                //order.Status = true;
                db.Orders.Add(order);
                db.SaveChanges();

           
            

            return RedirectToAction("Index", "KassaPos",new { OrdId=order.Id });

        }

        public ActionResult OrderDetay(int? Id,int? EmelyatId2)
        {
            OrdetayDto model = new OrdetayDto();

            List<OrderItem> orderItems = db.OrderItems.Where(x => x.Status == true && x.OrderId == Id).ToList();
            KassaEmeliyat kassaEmeliyat2 = db.KassaEmeliyats.FirstOrDefault(x => x.Id== EmelyatId2);
            model.orderItems = orderItems;
            model.order = db.Orders.FirstOrDefault(x => x.Id == Id);
            if (EmelyatId2 != null)
            {
                int? OrderId = kassaEmeliyat2.OrderId;
                model.orderItems = db.OrderItems.Where(x => x.OrderId == OrderId && x.Status == true).ToList();
                model.order = db.Orders.FirstOrDefault(x => x.Id == OrderId);
            }
            return View(model);
        }

        //[HttpPost]
        public ActionResult ProducAddCart(OrderItem orderItem,int? PrId, int OrdId,string Barcode)
        {
            Product BRCOD = db.Products.FirstOrDefault(x => x.Barcode == Barcode);
            if (PrId == null && BRCOD!=null)
            {
                PrId = BRCOD.Id;

            }

            int say = db.Orders.Where(x => x.Status == true).Count();
            int orderId = (from x in db.Orders.Where(x => x.Status == true) select (x.Id)).FirstOrDefault();
            OrderItem model = db.OrderItems.FirstOrDefault(x => x.ProductId == PrId && x.Order.Id == OrdId && x.Status==true);
            OrderItem model2 = db.OrderItems.FirstOrDefault(x => x.ProductId == PrId && x.Order.Id == OrdId && x.Status==true &&x.Product.Barcode==Barcode);


            //Barcode ye gore mehsul sayini bir bir artir.
            if (BRCOD!=null)
            {
                if (Barcode != null && BRCOD.Barcode != null)
                {


                    if (model2 != null)
                    {
                        if (model2.ProductId == PrId)
                        {
                            decimal Mehsay = (from z in db.Stocks.Where(x => x.ProductId == PrId) select z.Say_ceki_).Sum();

                            if (Mehsay > (decimal?)model2.Say)
                            {
                                model2.Say++;
                            model2.MehsulMeblegi = (decimal)model.Say * model.SatisMeblegi;

                            db.SaveChanges();
                            }
                        }
                    }
                }

            }

            if (BRCOD!=null)
            {
                if (Barcode == null && BRCOD.Barcode == null)
                {


                    if (model != null)
                    {


                        if (model.ProductId == PrId)
                        {
                            decimal Mehsay = (from z in db.Stocks.Where(x => x.ProductId == PrId) select z.Say_ceki_).First();
                            if (Mehsay > (decimal?)model.Say)
                            {
                                model.Say++;
                                model.MehsulMeblegi = (decimal)model.Say * model.SatisMeblegi;

                                db.SaveChanges();
                            }


                        }
                    }
                }

            }



            if (BRCOD!=null)
            {
                List<Product> products = db.Products.Where(x => x.Barcode == BRCOD.Barcode).ToList();
                int pId = products.Select(x => x.Id).First();
                List < Stock > stocks= db.Stocks.ToList();
                int Sto = stocks.Where(x => x.ProductId == pId).Count();
                
                if (Barcode != null && BRCOD.Barcode != null && Sto>0)
                {
                    decimal? Mehsay = (from z in db.Stocks.Where(x => x.ProductId == PrId) select z.Say_ceki_).Sum();
                    if (model2 == null && Mehsay > 0)
                    {



                        orderItem.Status = true;
                        orderItem.OrderId = OrdId;

                        orderItem.ProductId = (int)PrId;


                        orderItem.SatisMeblegi = db.Stocks.Where(x => x.ProductId == PrId).Select(x => x.SalePrice).FirstOrDefault();
                        orderItem.Say = (decimal)1;
                        orderItem.MehsulMeblegi = (decimal)orderItem.Say * orderItem.SatisMeblegi;

                        db.OrderItems.Add(orderItem);
                        db.SaveChanges();
                    }

                }

            }

            if (BRCOD!=null)
            {
                if (Barcode == null && BRCOD.Barcode == null)
                {

                    decimal Mehsay = (from z in db.Stocks.Where(x => x.ProductId == PrId) select z.Say_ceki_).First();
                    if (model == null && Mehsay > 0)
                    {



                        orderItem.Status = true;
                        orderItem.OrderId = OrdId;

                        orderItem.ProductId = (int)PrId;


                        orderItem.SatisMeblegi = db.Stocks.Where(x => x.ProductId == PrId).Select(x => x.SalePrice).FirstOrDefault();
                        orderItem.Say = (decimal)1;
                        orderItem.MehsulMeblegi = (decimal)orderItem.Say * orderItem.SatisMeblegi;

                        db.OrderItems.Add(orderItem);
                        db.SaveChanges();
                    }
                }

            }
            //if (Barcode==)
            //{

            //}

            ViewBag.qiymet = (decimal)orderItem.Say * orderItem.SatisMeblegi;

            
         

            return RedirectToAction("Index", "KassaPos", new { OrdId = OrdId, PrId = PrId });



        }

        public ActionResult Delete(int Id,int? OrdId)
        {
            //OrderItem orderItem = db.OrderItems.FirstOrDefault(x => x.Id == Id && x.OrderId==OrdId);
            //orderItem.Status = false;

            OrderItem model = db.OrderItems.FirstOrDefault(x => x.ProductId == Id && x.Order.Id == OrdId && x.Status == true);
            //model.Status =false ;

            db.OrderItems.Remove(model);
            db.SaveChanges();
            
            return RedirectToAction("Index", "KassaPos", new { OrdId = OrdId, PrId = Id });
        }

        public ActionResult Artir(int id,int OrdId,decimal? Say,decimal? ProductSay,decimal? ProductId)
        {
            //Product data = db.Products.FirstOrDefault(x => x.Id == id);

            OrderItem model = db.OrderItems.FirstOrDefault(x => x.ProductId == id&&x.Order.Id==OrdId && x.Status==true);
            decimal say = (from z in db.Stocks.Where(x => x.ProductId == id) select z.Say_ceki_).Sum();
           


                if (say > (decimal)model.Say)
                {

                    model.Say++;
                    decimal? a = model?.Say * model?.SatisMeblegi;
                    model.MehsulMeblegi = (decimal)a;
                    db.SaveChanges();
                }
            
           
         

            return RedirectToAction("Index", "KassaPos", new { OrdId = OrdId, PrId=id });
        }

        public ActionResult Azalt(int id, int OrdId)
        {
            OrderItem model = db.OrderItems.FirstOrDefault(x => x.ProductId == id && x.Order.Id == OrdId && x.Status == true);
            
            if (model.Say > 1)
            {
                model.Say--;
                decimal? a=  model?.Say * model?.SatisMeblegi;
                model.MehsulMeblegi = (decimal)a;
                db.SaveChanges();
            }



            return RedirectToAction("Index", "KassaPos", new { OrdId = OrdId, PrId = id });
        }

        public ActionResult SatisLegvi(int OrdId)
        {
            Order order = db.Orders.FirstOrDefault(x => x.Id == OrdId);

            order.Status = false;
            db.SaveChanges();

            return RedirectToAction("Order");
        }

        public ActionResult NegdSatisiSonlandir(int? OrdId,Order ord,KassaEmeliyat emeliyat,Cash cash,Log log)
        {
            decimal say = 0;
            Order order = db.Orders.FirstOrDefault(x => x.Id == OrdId);
            order.Status = true;
            order.OdenilenMebleg = ord.OdenilenMebleg;
            db.SaveChanges();
            List<OrderItem> data = db.OrderItems.Where(x => x.OrderId == OrdId && x.Status==true).ToList();

            foreach (var x in data)
            {

                //Stock stock = db.Stocks.SingleOrDefault(z => z.ProductId == x.ProductId);
                List<Stock> stock = db.Stocks.Where(z => z.ProductId == x.ProductId).ToList();
                decimal diffTotalCount =(decimal?) x.Say ?? 0;
                OrderItem orderItems =db.OrderItems.Where(j => j.OrderId == OrdId && j.Status == true && j.ProductId == x.ProductId).FirstOrDefault();
                decimal? qazanc = 0;
                foreach (var anbar in stock)
                {
                    
                    
                    decimal a = (from y in db.OrderItems.Where(j => j.OrderId == OrdId && j.Status == true && j.ProductId == x.ProductId) select y.Say).Sum();
                   
                    //stock.Say_ceki_ = stock.Say_ceki_ - a;

                    decimal wdiff = 0;
                   
                    if (diffTotalCount > anbar.Say_ceki_)
                    {
                        //Bu anbar kifayet qeder deyil
                        wdiff =(decimal?) anbar.Say_ceki_ ?? 0;
                        
                    }
                    else
                    {
                        //Bu anbar catacaq
                        wdiff = diffTotalCount;
                    }
                    //burdan cixilir
                    anbar.Say_ceki_ = (anbar.Say_ceki_ - wdiff);
                    decimal maya = (anbar.SalePrice - anbar.AlisMebleg) * wdiff;
                    qazanc = qazanc + maya;
                    db.SaveChanges();
                    diffTotalCount = (diffTotalCount - wdiff);
                    if (diffTotalCount == 0)
                    {
                        break;
                    }

                }

                orderItems.Qazanc = qazanc;

                db.SaveChanges();
                

            }
            List<decimal?> orderItem = (from x in db.OrderItems.Where(x => x.OrderId == OrdId) select x.MehsulMeblegi).ToList();
            order.ToplamMebleg = (decimal)orderItem.Sum();           
            db.SaveChanges();
           
            emeliyat.Tarix = DateTime.Now;
            emeliyat.OrderId = OrdId;
            emeliyat.Gelir_Xerc_ = true;
            emeliyat.Mebleg = ord.ToplamMebleg;
            db.KassaEmeliyats.Add(emeliyat);
            db.SaveChanges();
            cash.Balance = order.ToplamMebleg;
            db.Cashs.Add(cash);
            db.SaveChanges();

            string Id = Session["isId"].ToString();
            var ID = int.Parse(Id);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = order.Id;
            log.Description = "Toplam Məbləği" + " " + order.ToplamMebleg + " " + "AZN Dəyərində Nəğd Satışedildi";
            db.Logs.Add(log);
            db.SaveChanges();

            return RedirectToAction("Order");
        }

       public ActionResult SatisSonlandir(decimal OdenilenMebleg,decimal ToplamMebleg, int? OrdId, Customer customer,int? CustId,Order o,KassaEmeliyat emeliyat,decimal? meb,Log log,Cash cash)
        {
            Order order = db.Orders.FirstOrDefault(x => x.Id == OrdId);
            List<decimal?> orderItem = (from x in db.OrderItems.Where(x => x.OrderId == OrdId) select x.MehsulMeblegi).ToList();
            order.ToplamMebleg = (decimal)orderItem.Sum();
            
            db.SaveChanges();
            if (CustId == null)
            {
                decimal borc =  (from x in db.Orders.Where(x => x.Id == OrdId) select x.ToplamMebleg).Sum();

                customer.Status = true;
                customer.Balance = o.ToplamMebleg-o.OdenilenMebleg;
                order.OdenilenMebleg = o.OdenilenMebleg;
                db.Customers.Add(customer);
                db.SaveChanges();
                order.CustomerId = customer.Id;
            }
            else
            {
                
                order.CustomerId = CustId;
                db.SaveChanges();
                Order ord = db.Orders.FirstOrDefault(x => x.CustomerId == CustId);
                

                //db.SaveChanges();

                ord.Customer.Balance += ToplamMebleg - OdenilenMebleg;
                order.OdenilenMebleg = OdenilenMebleg;
                db.SaveChanges();


            }
            List<OrderItem> data = db.OrderItems.Where(x => x.OrderId == OrdId && x.Status==true).ToList();

            foreach (var x in data)
            {

                //Stock stock = db.Stocks.SingleOrDefault(z => z.ProductId == x.ProductId);
                List<Stock> stock = db.Stocks.Where(z => z.ProductId == x.ProductId).ToList();
                OrderItem orderItems = db.OrderItems.Where(j => j.OrderId == OrdId && j.Status == true && j.ProductId == x.ProductId).FirstOrDefault();

                decimal? qazanc = 0;
                decimal a = (from y in db.OrderItems.Where(j => j.OrderId == OrdId && j.Status == true && j.ProductId == x.ProductId) select y.Say).Sum();
                //stock.Say_ceki_ = stock.Say_ceki_ - a;
                decimal diffTotalCount = (decimal?)x.Say ?? 0;
                foreach (var anbar in stock)
                {


                    decimal ab = (from y in db.OrderItems.Where(j => j.OrderId == OrdId && j.Status == true && j.ProductId == x.ProductId) select y.Say).Sum();

                    //stock.Say_ceki_ = stock.Say_ceki_ - a;

                    decimal wdiff = 0;

                    if (diffTotalCount > anbar.Say_ceki_)
                    {
                        //Bu anbar kifayet qeder deyil
                        wdiff = (decimal?)anbar.Say_ceki_ ?? 0;
                    }
                    else
                    {
                        //Bu anbar catacaq
                        wdiff = diffTotalCount;
                    }
                    //burdan cixilir
                    anbar.Say_ceki_ = (anbar.Say_ceki_ - wdiff);
                    decimal maya = (anbar.SalePrice - anbar.AlisMebleg) * wdiff;
                    qazanc = qazanc + maya;
                    db.SaveChanges();
                    //anbar.Say_ceki_ = (anbar.Say_ceki_ - wdiff);

                    diffTotalCount = (diffTotalCount - wdiff);
                    if (diffTotalCount == 0)
                    {
                        break;
                    }

                }
                orderItems.Qazanc = qazanc;
                db.SaveChanges();


            }

            order.Status = true;

            cash.Balance = order.OdenilenMebleg;
            db.Cashs.Add(cash);
            db.SaveChanges();
            
            db.SaveChanges();
            if (order.OdenilenMebleg>0)
            {
                emeliyat.Tarix = DateTime.Now;
                emeliyat.OrderId = OrdId;
                emeliyat.Gelir_Xerc_ = true;
                emeliyat.Mebleg = order.OdenilenMebleg;
                db.KassaEmeliyats.Add(emeliyat);
                db.SaveChanges();
            }
        

            string Id = Session["isId"].ToString();
            var ID = int.Parse(Id);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = order.Id;
            log.Description = "Toplam Məbləği" + " " + order.ToplamMebleg + " " + "AZN Dəyərində Nisyə Satışedildi";
            db.Logs.Add(log);
            db.SaveChanges();

            return RedirectToAction("Order");
        }

    
       public JsonResult Test(decimal? say, decimal? ProId, decimal? OrdId)
        {
            OrderItem model = db.OrderItems.FirstOrDefault(x => x.ProductId == ProId && x.Order.Id == OrdId && x.Status == true);
            decimal Prosay = (from z in db.Stocks.Where(x => x.ProductId == ProId) select z.Say_ceki_).Sum();

            if (say < Prosay)
            {

                model.Say = (decimal)say;
                decimal? a = model?.Say * model?.SatisMeblegi;
                model.MehsulMeblegi = (decimal)a;
                db.SaveChanges();
                
               
                
                Order order = db.Orders.FirstOrDefault(x => x.Id == OrdId);

            }
            decimal? ToplamMebleg = db.OrderItems.Where(x => x.OrderId == OrdId && x.Status == true).Select(w => w.MehsulMeblegi).Sum();
            Session["ToplamMebleg"] = ToplamMebleg;
            db.SaveChanges();
            decimal MehsulSay = db.OrderItems.Where(x => x.Order.Id == OrdId && x.Status == true).Select(w => w.Say).Sum();
            return Json(new { OrdId = OrdId, PrId = ProId , Price = model.MehsulMeblegi, Pricesum = ToplamMebleg,ProductCount=Prosay, SebetSay= MehsulSay }, JsonRequestBehavior.AllowGet);
        }
      
    }
}