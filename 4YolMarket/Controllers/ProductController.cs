using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _4YolMarket.Filters;
using _4YolMarket.Models;
namespace _4YolMarket.Controllers
{
    
    [LoggedUser]
    public class ProductController : Controller
    {
        // GET: Product

        kassaEntities db = new kassaEntities();
        public ActionResult Index(int?ProId,int? IsId, int? page,int? SelsctId)
        {
            ProductDto model = new ProductDto();
            model.products = db.Products.Where(x => x.Status == true).ToList();
            model.products2 = db.Products.Where(x => x.Status == true).ToList();

            // List<Product> products = db.Products.Where(x=>x.Status==true).ToList();

            if (page == null)
            {
                page = 1;
            }
            int skip = ((int)page - 1) * 10;
            List<Product> data = new List<Product>();

            ViewBag.Category = db.Categories.Where(x => x.Status == true).ToList();
            ViewBag.Brend = db.Brands.Where(x => x.Status == true).ToList();
            data = db.Products.Where(x => x.Status == true).OrderByDescending(o => o.Id).ToList();
            if (SelsctId!=null)
            {
                data = db.Products.Where(x => x.Status == true&&x.Id==SelsctId).OrderByDescending(o => o.Id).ToList();

                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
                model.products = data;
            }
            if (ProId != null)
            {
                model.product = db.Products.FirstOrDefault(x => x.Id == ProId);

                ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
                ViewBag.Page = page;
                data = data.Skip(skip).Take(10).ToList();
            }
          

            ViewBag.TotalPage = Math.Ceiling(data.Count / 10.00);
            ViewBag.Page = page;
            data = data.Skip(skip).Take(10).ToList();

           
            model.products = data;
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


            return View(model);
            
        }

        [HttpPost]
        public ActionResult CreateProduct(Product product, HttpPostedFileBase Sekil,Log log,string eded,string kg)
        {
            product.Status = true;
            if (eded!=null&&kg==null)
            {
                product.IsComtable = true;
            }
            else
            {
                product.IsComtable = false;
            }
            

            Random rnd = new Random();
            string[] deyerler = { "A", "B", "C", "D" };
            int d1, d2, d3;
            d1 = rnd.Next(0, 4);                                    //burada Random Kargo kodu yaratdim
            d2 = rnd.Next(0, 4);
            d3 = rnd.Next(0, 4);
            int s1, s2, s3;
            s1 = rnd.Next(100, 1000);
            s2 = rnd.Next(10, 99);
            s3 = rnd.Next(10, 99);

            string kod = s1.ToString() + deyerler[d1] + s2 + deyerler[d2] + s3 + deyerler[d3];
            product.ProductCode = kod;
            if (Sekil != null)
            {
                string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(Sekil.FileName));
                Sekil.SaveAs(path);
                product.Sekil = Path.GetFileName(Sekil.FileName);
            }
            db.Products.Add(product);
            db.SaveChanges();

            
            string Id = Session["isId"].ToString();
            var ID = int.Parse(Id);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = product.Id;
            log.Description = "Yeni Məhsul Əlavəolundu";
            db.Logs.Add(log);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult Delate(int Id,Log log)
        {
           Product products = db.Products.FirstOrDefault(x => x.Id == Id);
            products.Status = false;
            db.SaveChanges();

            string id = Session["isId"].ToString();
            var ID = int.Parse(id);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = products.Id;
            log.Description = "Məhsul Silindi";
            db.Logs.Add(log);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult UpdateProduct(int Id,Product p, HttpPostedFileBase Sekil,Log log)
        {
            
            Product product= db.Products.FirstOrDefault(x => x.Id == Id);
            product.Ad = p.Ad;
            product.Barcode = p.Barcode;
            product.Category = p.Category;
            product.Brand = p.Brand;
            product.IsComtable = p.IsComtable;
            if (Sekil != null)
            {
                string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(Sekil.FileName));
                Sekil.SaveAs(path);
                product.Sekil = Path.GetFileName(Sekil.FileName);
            }

            db.SaveChanges();
            string id = Session["isId"].ToString();
            var ID = int.Parse(id);
            log.UserId = ID;
            log.Tarix = DateTime.Now;
            log.HereketId = ID;
            log.Description =product.Ad+" "+"Adlı Məhsulun Adı"+p.Ad+" " + "Olaraq dəyişdirildi";
            db.Logs.Add(log);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}
