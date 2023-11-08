using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = new Entities();
            //var CategoryBrandFromDB = db.CategoryBrands.ToList();


            //var CategoriesFromDB = db.Categories.ToList();
            //ViewBag.Categories = CategoriesFromDB;

            //var BrandsFromDB = db.Brands.ToList();
            //ViewBag.Brands = BrandsFromDB;

            //var ProductsFromDB = db.Products.ToList();
            //ViewBag.Products = ProductsFromDB;

            //return View(CategoryBrandFromDB); // DTO ? 🔰🔗
            return View();
        }

        public ActionResult logout()
        {
            
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}