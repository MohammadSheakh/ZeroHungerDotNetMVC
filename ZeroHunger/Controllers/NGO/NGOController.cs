using E_Commerce_Web_Application.DTOs;
using E_Commerce_Web_Application.DTOs.Seller;
using E_Commerce_Web_Application.Helper.Converter;
using E_Commerce_Web_Application.Helper.CustomAttribute.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ZeroHunger.DTOs.CollectRequstForm;
using ZeroHunger.DTOs.Employee;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers.NGO
{
    public class NGOController : Controller
    {
        // GET: NGO
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult NGODashboard()
        {
            var db = new Entities();
            int ngoId = Convert.ToInt32(Session["ngoId"]);
            //var dataFromDB = (from collectRequestForm in db.CollectRequstForms
            //                  where collectRequestForm.ngoId == ngoId
            //                  select new
            //                  {
            //                      collectRequestForm.reqFormId,
            //                      collectRequestForm.requestStatus,
            //                      collectRequestForm.createdAt,

            //                      collectRequestForm.employeeId,

            //                      collectRequestForm.ngoId,
            //                      collectRequestForm.foodSourceId,

            //                  }).ToList();

            var dataFromDB = (from collectRequestForm in db.CollectRequstForms
                              where collectRequestForm.ngoId == ngoId
                              select collectRequestForm).ToList();

            ViewBag.AllCollectRequstForm = dataFromDB.ToList();
            // NGO er Dashboard e back korte hobe 
            return View();
        }

        [HttpGet]
        public ActionResult CreateNGOAccount()
        {
            // NGO er Dashboard e back korte hobe 
            return View();
        }

        //1/ Create NGO Account [Post] <-  Done 
        [HttpPost]
        public ActionResult CreateNGOAccount(NGODTO ngoDto, HttpPostedFileBase image)
        {
            // after save those fields
            var db = new Entities();
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

                db.NGOes.Add(autoMapper.ConvertForSingleInstance<NGODTO, EF.NGO>(ngoDto));
                // as controller name and model name is similler we have to specify the namespace .. 


                db.SaveChanges();
                //return RedirectToAction("NGODashboard");
                return RedirectToAction("NGOLogin");

            }
            return View();
        }


        [HttpGet]
        public ActionResult CreateNewEmployeeAccount()
        {
            // NGO er Dashboard e back korte hobe 
            return View();
        }


        //2/ Create Employee Account [Post] <-  Done 
        [HttpPost]
        public ActionResult CreateNewEmployeeAccount(EmployeeDTO employeeDto, HttpPostedFileBase image)
        {
            // after save those fields
            var db = new Entities();
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
                    employeeDto.image = uniqueFileName;// 🔗🔰 only image name ki save kora lagbe naki only path ? 

                }

                //if (shopLogo != null && shopLogo.ContentLength > 0)
                //{
                //    // Handle the image upload, save it to a specific folder
                //    var fileName = Path.GetFileName(shopLogo.FileName);
                //    var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                //    shopLogo.SaveAs(path);

                //    // save file path to DB 
                //    sellerDto.shopLogo = path;// 🔗🔰 only image name ki save kora lagbe naki only path ? 

                //}

                employeeDto.createdAt = DateTime.Now;
                // seller deowa password .. hash kore database e save korte hobe 

                var password = employeeDto.password; // byte to string
                var salt = BCrypt.Net.BCrypt.GenerateSalt();
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, salt);
                //sellerDto.password = Encoding.UTF8.GetBytes(passwordHash); // string to byte
                // sellerDto.password = Convert.FromBase64String(passwordHash);
                employeeDto.password = passwordHash;

                var autoMapper = new AutoMapperConverter();
                // autoMapper.ConvertForSingleInstance<SellerDTO, Seller>(sellerDto);

                db.Employees.Add(autoMapper.ConvertForSingleInstance<EmployeeDTO, EF.Employee>(employeeDto));
                // as controller name and model name is similler we have to specify the namespace .. 


                db.SaveChanges();
                return RedirectToAction("NGODashboard");
            }
            return View();
        }

        [NGOAuthGuard]
        [HttpGet] // View with DTO - status [working]
        public ActionResult showAllEmployeesDetails()
        {
            var db = new Entities();
            var dataFromDB = db.Employees.ToList(); // list of product 
            // DTO 
            var autoMapper = new AutoMapperConverter();
            //var mapperClass = new YourMapperClass();
            List<EmployeeDTO> sellerDTOList = autoMapper.ConvertForList<EF.Employee, EmployeeDTO>(dataFromDB);

            #region

            #endregion

            return View(sellerDTOList);
        }

        [HttpGet]
        public ActionResult NGOLogin()
        {
            return View();
        }


            [HttpPost]
        public ActionResult NGOLogin(NGOLoginDTO NGOLoginDto)
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

                var normalPassword = NGOLoginDto.password;
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

                var user = db.NGOes.FirstOrDefault(u =>
                u.email == NGOLoginDto.email
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
                        Session["userId"] = user.ngoId;
                        Session["userEmail"] = user.email;
                        Session["userName"] = user.name;
                        Session["userType"] = "NGO";
                        Session["userImage"] = user.image;
                        Session["ngoId"] = user.ngoId;

                        // it means email and password is correct 
                        // return RedirectToAction("../Home/index");
                        return RedirectToAction("NGODashboard");
                    }
                    else
                    {
                        // credential is wrong - password is wrong  
                        return View(NGOLoginDto);
                    }

                }
                else
                {
                    // credential is wrong - email is wrong .. 
                    return View(NGOLoginDto);
                }

            }

            return View(NGOLoginDto);
        }


        // 1. Create New Employee -> Partially DOne 
        // 2. Show All Employee -> Partially Done 
        // 3. NGO Login -> Partially Done 

        // 4. Kono CollectRequstForm ashse kina dekhte parbe .. 

        [HttpGet]
        public ActionResult ShowAllCollectRequstForm()
        {
            var db = new Entities();
            int ngoId = Convert.ToInt32(Session["ngoId"]);
            var dataFromDB = (from collectRequestForm in db.CollectRequstForms
                              where collectRequestForm.ngoId == ngoId
                              select new
                              {
                                  collectRequestForm.reqFormId,
                                  collectRequestForm.requestStatus,
                                  collectRequestForm.createdAt,

                                  collectRequestForm.employeeId,

                                  collectRequestForm.ngoId,
                                  collectRequestForm.foodSourceId,
                                  
                              }).ToList();

            return View(dataFromDB);
            // return View();
        }


        // 5. jodi kono ta ashe .. taile sheta te employee 
        // assign korte parbe .. 

        [HttpGet]
        public ActionResult AssignEmployeeToRequestForm(int? id)
        {

            var db = new Entities();
            //int ngoId = Convert.ToInt32(Session["ngoId"]);
            var dataFromDB = (from collectRequestForm in db.CollectRequstForms
                              where collectRequestForm.reqFormId == id
                              select collectRequestForm).FirstOrDefault();

            var autoMapper = new AutoMapperConverter();
            var convertedSeller = autoMapper.ConvertForSingleInstance<EF.CollectRequstForm, CollectRequstFormDTO >(dataFromDB);

            var dataFromEmployeeTable = (from emp in db.Employees
                                         select emp).ToList();

            ViewBag.employees = dataFromEmployeeTable;

            return View(convertedSeller);
            //return RedirectToAction("ShowAllCollectRequstForm");
        }

        [HttpPost]
        public ActionResult AssignEmployeeToRequestForm(CollectRequstFormDTO collectRequestFormDTO)
        {

            var db = new Entities();
            //int ngoId = Convert.ToInt32(Session["ngoId"]);

            var extractSeller = db.CollectRequstForms.Find(collectRequestFormDTO.reqFormId);
            extractSeller.employeeId = Convert.ToInt32(collectRequestFormDTO.employeeId);
            var autoMapper = new AutoMapperConverter();
            db.Entry(extractSeller).CurrentValues.SetValues(autoMapper.ConvertForSingleInstance<CollectRequstFormDTO, EF.CollectRequstForm>(collectRequestFormDTO));
            db.SaveChanges();

            return RedirectToAction("NGODashboard");
        }

        // 6. status update korte parbe .. 


    }
}