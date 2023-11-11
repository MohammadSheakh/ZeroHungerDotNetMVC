using E_Commerce_Web_Application.DTOs.Seller;
using E_Commerce_Web_Application.Helper.Converter;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.DTOs.CollectRequstForm;
using ZeroHunger.DTOs.Employee;
using ZeroHunger.DTOs.FoodItem;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers.Employee
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EmployeeDashboard()
        {
            var db = new Entities();
            int empId = Convert.ToInt32(Session["userId"]);

            var dataFromDB = (from collectRequestForm in db.CollectRequstForms
                              where collectRequestForm.employeeId == empId
                              select collectRequestForm).ToList();

            // automapper use korte hobe .. 

            //var autoMapper = new AutoMapperConverter();
            //var convertedSeller = autoMapper.ConvertForList<EF.CollectRequstForm, CollectRequstFormDTO>(dataFromDB);
            ViewBag.AllCollectRequstForm = dataFromDB;

            //ViewBag.AllCollectRequstForm = convertedSeller.ToList();

            return View();
        }

        [HttpGet]
        public ActionResult ShowFoodItemDetailsForACollectRequestForm(int id)
        {
            var db = new Entities();
            int empId = Convert.ToInt32(Session["userId"]);

            // ekhane requestFromId ashbe id er moddhe .. er against e 
            /*
             requestItem table theke foodItemId ber korbo .. 

            foodItemId gula ekta list e rakhte hobe .. 

            shei list er moddhe loop chaliye 

            foodItem table er moddhe theke data ber kore niye ashbo 

             */



            //var dataFromDB = (from foodItem in db.FoodItems
            //                  where foodItem.foodItemId == id
            //                  select foodItem).ToList();

            var dataFromDB = (from requestItem in db.RequestItems
                             where requestItem.collectRequestFormId == id
                             join foodItem in db.FoodItems on requestItem.foodItemId equals foodItem.foodItemId
                             select foodItem).ToList();
                             //new
                             //{
                             //    requestItem.foodItemId,
                             //    //foodItem.foodtemId,
                             //    foodItem.name,
                             //    foodItem.quantity,
                             //    foodItem.quantityUnit,
                             //    foodItem.maxPreservationTime,
                             //    foodItem.sourceId,
                             //    // Include other fields from the FoodItem table as needed
                             //}.ToList();

            var autoMapper = new AutoMapperConverter();
            //var mapperClass = new YourMapperClass();
            var foodItems = autoMapper.ConvertForList<EF.FoodItem, FoodItemDTO>(dataFromDB);


            ViewBag.foodItems = foodItems;
            return View();
        }

        [HttpGet]
        public ActionResult EmployeeLogin()
        {
            return View();
        }


        [HttpPost]
        public ActionResult EmployeeLogin(EmployeeLoginDTO EmployeeLoginDto)
        {

            // login houar pore .. Session e User Name and User Type rakhte hobe

            // seller er email and password diye check korte hobe .. 
            // password .. hash kore database e rakhte hobe .. 

            var db = new Entities();
            if (ModelState.IsValid)
            {
                // sellerLoginDto.;
                //var data = (from product in db.Products
                //            where product.id == id
                //            select product).SingleOrDefault();

                // lets hash 

                var normalPassword = EmployeeLoginDto.password;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                
                var user = db.Employees.FirstOrDefault(u =>
                u.email == EmployeeLoginDto.email
                ); // string to byte

                // u.password == Encoding.UTF8.GetBytes(passwordHashFromUser)

                //var passwordInStringFormat = Encoding.UTF8.GetString(user.password);



                if (user != null)
                {
                    // ekhon password check korte hobe .. 
                    //bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(
                    //    normalPassword, Encoding.UTF8.GetString(user.password));

                    bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(
                        normalPassword, user.password);

                    if (isPasswordCorrect)
                    {
                        Session["userId"] = user.empId;
                        Session["userEmail"] = user.email;
                        Session["userName"] = user.name;
                        Session["userType"] = "Employee";
                        Session["userImage"] = user.image;
                        
                        // it means email and password is correct 
                        // return RedirectToAction("../Home/index");
                        return RedirectToAction("EmployeeDashboard");
                    }
                    else
                    {
                        // credential is wrong - password is wrong  
                        return RedirectToAction("EmployeeLogin");
                    }

                }
                else
                {
                    // credential is wrong - email is wrong .. 
                    return RedirectToAction("EmployeeLogin");
                }

            }

            return RedirectToAction("EmployeeLogin");
        }

    }
}