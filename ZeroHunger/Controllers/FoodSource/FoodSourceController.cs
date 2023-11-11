using E_Commerce_Web_Application.DTOs;
using E_Commerce_Web_Application.DTOs.Seller;
using E_Commerce_Web_Application.Helper.Converter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using ZeroHunger.DTOs.Employee;
using ZeroHunger.DTOs.FoodItem;
using ZeroHunger.DTOs.FoodSource;
using ZeroHunger;

namespace ZeroHunger.Controllers.FoodSource
{
    public class FoodSourceController : Controller
    {
        // GET: FoodSource
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult FoodSourceDashboard()
        {
            var db = new EF.Entities();

            int sourceId = Convert.ToInt32(Session["userId"]);

            var dataFromDB = (from foodItem in db.FoodItems
                              where foodItem.sourceId == sourceId 
                              select foodItem).ToList();

            var autoMapper = new AutoMapperConverter();
            var convertedSeller = autoMapper.ConvertForList<EF.FoodItem, FoodItemDTO>(dataFromDB);

            // This is for Create New FoodItem 
            ViewBag.FoodItems = convertedSeller;
            

            // We need something that show all CollectRequestForm

            var dataFromDB2 = (from collectRequestForm in db.CollectRequstForms
                              where collectRequestForm.foodSourceId == sourceId
                              select collectRequestForm).ToList();

            ViewBag.AllCollectRequstForm = dataFromDB2.ToList();


            return View();
        }

        [HttpGet]
        public ActionResult CreateFoodSourceAccount()
        {
            // FoodSource er Login Page e back korte hobe 
            return View();
        }

        [HttpPost]
        public ActionResult CreateFoodSourceAccount(FoodSourceDTO ngoDto, HttpPostedFileBase image)
        {
            // after save those fields
            var db = new EF.Entities();
            if (ModelState.IsValid)
            {
                if (image != null && image.ContentLength > 0)
                {
                    // Handle the image upload, save it to a specific folder
                    var fileName = Path.GetFileName(image.FileName);

                    // lets manupulate file name for uniquness 
                    // fileName theke extension alada kore nite hobe ..
                    // date + name + extension ei format e save korte hobe 



                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

                    // Generate a unique file name by combining a GUID and the original file extension
                    var uniqueFileName = fileNameWithoutExtension + Guid.NewGuid().ToString() + Path.GetExtension(fileName);


                    var path = Path.Combine(Server.MapPath("~/Uploads"), uniqueFileName);

                    // Save the image to folder 
                    //var fullPath = Server.MapPath(path);
                    image.SaveAs(path);

                    // save file name / path to DB 
                    ngoDto.image = uniqueFileName;// 🔗🔰 only image name ki save kora lagbe naki only path ? 

                }




                // seller deowa password .. hash kore database e save korte hobe 

                var password = ngoDto.password; // byte to string
                var salt = BCrypt.Net.BCrypt.GenerateSalt();
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, salt);
                //sellerDto.password = Encoding.UTF8.GetBytes(passwordHash); // string to byte
                // sellerDto.password = Convert.FromBase64String(passwordHash);
                ngoDto.password = passwordHash;

                var autoMapper = new AutoMapperConverter();
                // autoMapper.ConvertForSingleInstance<SellerDTO, Seller>(sellerDto);

                db.FoodSources.Add(autoMapper.ConvertForSingleInstance<FoodSourceDTO, EF.FoodSource>(ngoDto));
                // as controller name and model name is similler we have to specify the namespace .. 

                ngoDto.address = "a";

                db.SaveChanges();
                //return RedirectToAction("NGODashboard");
                return RedirectToAction("FoodSourceLogin");

            }
            return View();
        }

        [HttpGet]
        public ActionResult FoodSourceLogin()
        {
            return View();
        }

        // 1. FoodSourceLogin
        [HttpPost]
        public ActionResult FoodSourceLogin(FoodSourceLoginDTO foodSourceLoginDto)
        {

            // login houar pore .. Session e User Name and User Type rakhte hobe

            // seller er email and password diye check korte hobe .. 
            // password .. hash kore database e rakhte hobe .. 

            var db = new EF.Entities();
            if (ModelState.IsValid)
            {
                // sellerLoginDto.;
                //var data = (from product in db.Products
                //            where product.id == id
                //            select product).SingleOrDefault();

                // lets hash 

                var normalPassword = foodSourceLoginDto.password;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                var user = db.FoodSources.FirstOrDefault(u =>
                u.email == foodSourceLoginDto.email
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
                        Session["userId"] = user.sourceId;
                        Session["userEmail"] = user.email;
                        Session["userName"] = user.name;
                        Session["userType"] = "FoodSource";
                        Session["userImage"] = user.image;

                        // it means email and password is correct 
                        return RedirectToAction("FoodSourceDashboard");
                    }
                    else
                    {
                        // credential is wrong - password is wrong  
                        return View(foodSourceLoginDto);
                    }

                }
                else
                {
                    // credential is wrong - email is wrong .. 
                    return View(foodSourceLoginDto);
                }

            }

            return View(foodSourceLoginDto);
        }




        // 2. FoodSource can addFoodItem 

        [HttpPost]
        public ActionResult CreateNewFoodItem(FoodItemDTO foodItemDto)
        {
            if (Session["userId"] != null)
            {
                // after save those fields
                var db = new EF.Entities();
                if (ModelState.IsValid)
                {
                    foodItemDto.sourceId = Convert.ToInt32(Session["userId"]);
                    var autoMapper = new AutoMapperConverter();
                    // autoMapper.ConvertForSingleInstance<SellerDTO, Seller>(sellerDto);

                    db.FoodItems.Add(autoMapper.ConvertForSingleInstance<FoodItemDTO, EF.FoodItem>(foodItemDto));
                    // as controller name and model name is similler we have to specify the namespace .. 


                    db.SaveChanges();
                    return RedirectToAction("FoodSourceDashboard");
                }
            }
            
            //return View();
            return RedirectToAction("FoodSourceDashboard");
        }


        // 2.1 Show All FoodItem

        // SHow All FoodItem Of a FoodSource 

        [HttpGet]
        public ActionResult showAllFoodItemDetailsOfASource(int sourceId =1)
        {
            var db = new EF.Entities();
            //var data = db.Products.ToList();// List akare dey 

            var dataFromDB = (from foodItem in db.FoodItems
                              where foodItem.sourceId == sourceId
                              select foodItem).ToList();

            var autoMapper = new AutoMapperConverter();
            var convertedSeller = autoMapper.ConvertForList<EF.FoodItem, FoodItemDTO>(dataFromDB);

            // Remove All Items nam e ekta button thakbe .. Tokhon just FoodItems theke sourceId delete kore dibo - Done  


            // ekhan thekei Prottekta food er pashe Edit Button Thakbe .. 

            return View(convertedSeller);
        }


        // Ekdom last e CollectRequestForm er Button thakbe .. 
        // jeta te press korle ekta CollectRequestForm Create hobe .. 
        public ActionResult SubmitCollectRequestForm(/*CollectRequstForm collectRequestForm*/)
        {

            // 🔰🔗🔴 ekta check korte hobe .. sheta hocche .. 
            // kono foodItem na thakle CollectRequest form create hobe na .. 

            var db = new EF.Entities();

            var cart = Session["key"] as List<EF.FoodItem>;
            var sourceID = Convert.ToInt32(Session["userId"]);

            if (cart != null && cart.Any())
            {

                EF.CollectRequstForm collectRequestForm = new EF.CollectRequstForm
                {
                    // reqFormId <- auto generated after table creation
                    foodSourceId = sourceID,// From Session,
                    requestStatus = "Request Pending", //{ Request Pending, Request Accepted, Employee Assined, Food Colledted }
                    createdAt = DateTime.Now,
                    ngoId = 1 // this is Static value ;

                };

                
                foreach (var item in cart)
                {

                    EF.RequestItem requestItem = new EF.RequestItem
                    {
                        //id = auto generate hobe
                        foodItemId = item.foodItemId,
                        collectRequestFormId = collectRequestForm.reqFormId,
                    };

                    db.RequestItems.Add(requestItem);
                }

                db.CollectRequstForms.Add(collectRequestForm);
                db.SaveChanges();
            }
            else
            {
                EF.CollectRequstForm collectRequestForm = new EF.CollectRequstForm
                {
                    // reqFormId <- auto generated after table creation
                    foodSourceId = Convert.ToInt32(Session["userId"]),// From Session,
                    requestStatus = "Request Pending", //{ Request Pending, Request Accepted, Employee Assined, Food Colledted }
                    createdAt = DateTime.Now,
                    ngoId = 1 // this is Static value ;

                };


                int sourceId = Convert.ToInt32(Session["userId"]);
                
                var dataFromDB = (from foodItem in db.FoodItems
                                  where foodItem.sourceId == sourceId
                                  select foodItem).ToList();

                // then Prottek Food Item er jonno Request Item Table e Data entry hobe 

                foreach (var item in dataFromDB)
                {

                    EF.RequestItem requestItem = new EF.RequestItem
                    {
                        //id = auto generate hobe
                        foodItemId = item.foodItemId,
                        collectRequestFormId = collectRequestForm.reqFormId,
                    };
                    db.RequestItems.Add(requestItem);

                }
                db.CollectRequstForms.Add(collectRequestForm);
                db.SaveChanges();
            }

                
            return RedirectToAction("FoodSourceDashboard");// 🔰🔗⚫ sourceId pass kora lagbe 
        }

        // Remove FoodItems For FoodSource .. 
        // Remove All Items nam e ekta button thakbe .. Tokhon just FoodItems theke sourceId delete kore dibo  

        [HttpGet]
        public ActionResult RemoveSourceIdFromSingleFoodItems(int? id)
        {
            // 🔰🔗⚫ Remove korar shathe shathe Cart thekeo remove korte hobe !............. 
            // age dekhte hobe .. cart e ei item ta ase kina .. thakle cart thekeo remove korbo 
            var db = new EF.Entities();
            var foodItemId = id;
            //var dataFromDB = (from foodItem in db.FoodItems
            //                  where foodItem.sourceId == sourceId
            //                  select foodItem).ToList();
            var dataFromDB = (from foodItem in db.FoodItems
                              where foodItem.foodItemId == foodItemId
                              select foodItem).FirstOrDefault();

            //dataFromDB.sourceId = 0;
            dataFromDB.sourceId = null; // 🔗🔰⚫ Foreign Key te kokhono 0 assign kora jay na .. 
            // ⚫🔗🔰 null assign korte hoy 
            var autoMapper = new AutoMapperConverter();
            db.Entry(dataFromDB).CurrentValues.SetValues(autoMapper.ConvertForSingleInstance<EF.FoodItem,  FoodItemDTO > (dataFromDB)); // ⚫🔗🔰

            db.SaveChanges();
            // return RedirectToAction("RemoveSourceIdFromSingleFoodItemsPOST");

            return RedirectToAction("FoodSourceDashboard");
        }

        ////[HttpPost]
        ////public ActionResult RemoveSourceIdFromSingleFoodItemsPOST(FoodItemDTO foodItemDto)
        ////{
        ////    var db = new Entities();

        ////}


        [HttpGet]
        public ActionResult RemoveSourceIdFromSourcesFoodItems()
        {

            // sourceId <input type="hidden"> theke ashbe 
            // othoba source theke ashbe .. 

            int sourceId = Convert.ToInt32(Session["userId"]);
            var db = new EF.Entities();
            var dataFromDB = (from foodItem in db.FoodItems
                              where foodItem.sourceId == sourceId
                              select foodItem).ToList();

            foreach (var foodItem in dataFromDB)
            {
                foodItem.sourceId = null;
            }
            
            var autoMapper = new AutoMapperConverter();
            db.Entry(dataFromDB).CurrentValues.SetValues(autoMapper.ConvertForList<EF.FoodItem, FoodItemDTO>(dataFromDB)); // ⚫🔗🔰
            db.SaveChanges();
            return RedirectToAction("FoodSourceDashboard");
        }



        // 3. FoodSource can requst CollectRequestForm 
        // with FoodItem 
        // firstOfAll Amader ke cart e add korte hobe 
        // then shekhan theke database e save kora jete pare .. 
        // abar shob ekbar e database e save kora jete pare .. 




        // Requst Means Submit Data in CollectRequstForm 



        // 4. FoodItem gula je show korbo .. tar pashe ..
        // 5. add korar button thakbe .. shegula add kore 
        // tarpor sheta CollectRequst Form e send / save korte hobe .. 

        /////////////////////// Cart e add korte hobe .. 
        ///
        [HttpGet]
        public ActionResult addToCart(int? id)
        {
            var db = new EF.Entities();
            //First LINQ ..
            var data = (from foodItem in db.FoodItems
                        where foodItem.foodItemId == id
                        select foodItem).SingleOrDefault();
            if (data != null)
            {
                // Retrieve the existing cart from the session
                var cart = Session["key"] as List<EF.FoodItem>;


                if (cart == null)
                {
                    cart = new List<EF.FoodItem>();
                }


                cart.Add(data);
                int cartCount = Convert.ToInt32(Session["CartCount"]);
                Session["CartCount"] = cartCount + 1;

                Session["key"] = cart;
            }
            // return RedirectToAction("showAllProductsDetails");

            return RedirectToAction("FoodSourceDashboard");
        }


        // 6. ekta dashboard e Status dekhte parbe requested item gular 

        [HttpGet]
        public ActionResult StatusOfAllRequst(int foodSourceId) {
            // foodSourceId er value session theke ashbe .. 
            var db = new EF.Entities();
            int sourceId = Convert.ToInt32(Session["sourceId"]);
            var dataFromDB = (from collectRequestForm in db.CollectRequstForms
                              where collectRequestForm.foodSourceId == sourceId
                              select new
                              {
                                  collectRequestForm.reqFormId,
                                  collectRequestForm.ngoId,
                                  collectRequestForm.requestStatus,
                                  collectRequestForm.foodSourceId,
                                  //collectRequestForm.createdAt,
                              }).ToList();

            return View(dataFromDB);
        }


    }
}