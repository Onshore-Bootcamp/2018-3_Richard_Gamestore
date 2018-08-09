using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gamestore_MVC.Mapping;
using Gamestore_MVC.Models;
using System.Configuration;
using Gamestore_DAL.Models;
using Gamestore_DAL.Mapping;
using Gamestore_DAL;
using System.Data.SqlClient;
using System.Reflection;

namespace Gamestore_MVC.Controllers

{
    //todo: add comments to program

    public class AccountController : Controller
    {
        private static Logger _Logger;


        UserMapper _Mapper = new UserMapper();

        private readonly UserDAO _UserDataAccess;
        public AccountController()
        {

            string log = ConfigurationManager.AppSettings["ErrorLogPath"];

            string connectionString = ConfigurationManager.ConnectionStrings["DataSource"].ConnectionString;
            _Logger = new Logger(log);
            _UserDataAccess = new UserDAO(connectionString, log);
        }

        // GET: Account

        [HttpGet]
        public ActionResult Register()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Register(User form)//a form gets data
        {
            ActionResult responce;
            try
            {

                if (ModelState.IsValid)
                {
                    UserDO dataObject = new UserDO();

                    dataObject.UserName = form.UserName;
                    dataObject.Password = form.Password;
                    dataObject.Email = form.Email;

                    dataObject.RoleId = 1;

                    _UserDataAccess.RegisterUser(dataObject);
                    TempData["UserName"] = dataObject.UserName;
                    responce = RedirectToAction("Login", "Account");
                    if (dataObject.Email.Contains('@'))
                    {
                        // continues 
                    }
                    else
                    {
                        responce = View(form);
                        TempData["Email Error Message"] = "Invalid Email @ is Requiered";

                    }
                }
                else
                {

                    responce = View(form);
                }

            }
            catch (SqlException sqlex)
            {

                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
                responce = RedirectToAction("Register", "Account");
            }
            return responce;
        }
        [HttpGet]
        public ActionResult Login()
        {
            Login login = new Login();
            if (TempData.ContainsKey("Username"))
            {
                login.UserName = (string)TempData["Username"];
            }
            return View(login);
        }



        [HttpPost]
        public ActionResult Login(Login form)
        {
            ActionResult responce;

            try
            {

                if (ModelState.IsValid)
                {
                    UserDO userDataObject = _UserDataAccess.ViewUserByUserName(form.UserName);
                    if (userDataObject.UserId != default(int))
                    {
                        if (userDataObject.Password.Equals(form.Password))
                        {
                            Session["UserName"] = userDataObject.UserName;
                            Session["UserId"] = userDataObject.UserId;
                            Session["RoleId"] = userDataObject.RoleId;
                            Session["RoleName"] = userDataObject.RoleName;

                            responce = RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            responce = View(form);
                        }
                    }
                    else
                    {
                        responce = View(form);
                    }
                }
                else
                {
                    responce = View(form);
                }

            }
            catch (SqlException sqlex)
            {
                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
                responce = RedirectToAction("Index", "Home");


            }
            catch (Exception ex)
            {
                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                responce = RedirectToAction("Index", "Home");

            }
            return responce;
        }

        [HttpGet]
        public ActionResult Delete(string username)
        {
            ActionResult responce;
            if ((long)Session["RoleId"] == 5)
            {


                try
                {

                    UserDO user = _UserDataAccess.ViewUserByUserName(username);
                    User deleteUser = _Mapper.MapDoToPo(user);
                    long userID = deleteUser.UserId;

                    if (userID > 0)
                    {
                        _UserDataAccess.DeleteUser(userID);
                        responce = RedirectToAction("AllUsers", "Account");
                    }
                    else
                    {
                        responce = RedirectToAction("AllUsers", "Account");
                    }

                }
                catch (SqlException sqlex)
                {

                    _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
                    responce = RedirectToAction("Index", "Home");

                }
                catch (Exception ex)
                {
                    _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                    responce = RedirectToAction("Index", "Home");

                }
            }
            else
            {
                responce = RedirectToAction("Login", "Account");
            }
            return responce;
        }
        public ActionResult AllUsers()
        {
           
            List<User> mappedUsers = new List<User>();
            ActionResult response;
            if (Session["RoleId"] != null && (long)Session["RoleId"] == 5)
            {
                try
                {
                    List<UserDO> allusers = _UserDataAccess.ViewUsers();


                    foreach (UserDO dataObjet in allusers)
                    {
                        mappedUsers.Add(_Mapper.MapDoToPo(dataObjet));
                    }



                }

                catch (SqlException sqlex)
                {

                    _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);

                }
                response = View(mappedUsers);

            }
            else
            {
                response = RedirectToAction("Login", "Account");
            }
            return response;
        }

        public ActionResult UserDetails(string username)
        {
            ActionResult responce;
            if (Session["RoleId"]!= null &&(long)Session["RoleId"] == 5)
            {
                try
                {

                    UserDO user = _UserDataAccess.ViewUserByUserName(username);
                    User detailsUser = _Mapper.MapDoToPo(user);
                    long UserID = detailsUser.UserId;

                    if (UserID > 0)
                    {
                        responce = View(detailsUser);
                    }
                    else
                    {
                        responce = RedirectToAction("AllUsers", "Account");
                    }
                }
                catch (SqlException sqlex)
                {

                    _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
                    responce = RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                    responce = RedirectToAction("Index", "Home");

                }
            }
            else
            {
                responce = RedirectToAction("Login", "Account");
            }
            return responce;
        }
        [HttpGet]
        public ActionResult UpdateUser(string username)
        {
            ActionResult response;
            if (Session["RoleId"] != null && (long)Session["RoleId"] == 5)
            {
                try
                {

                    UserDO user = _UserDataAccess.ViewUserByUserName(username);
                    User updateUser = _Mapper.MapDoToPo(user);
                    long userID = updateUser.UserId;

                    //check id CANT be default or less than 0
                    if (userID > 0)
                    {
                        ViewBag.DropList = new List<SelectListItem>();
                        ViewBag.DropList.Add(new SelectListItem() { Text = "User", Value = 1.ToString() });
                        ViewBag.DropList.Add(new SelectListItem() { Text = "Moderator", Value = 3.ToString() });
                        ViewBag.DropList.Add(new SelectListItem() { Text = "Admin", Value = 5.ToString() });

                        response = View(updateUser);
                    }
                    else
                    {
                        response = RedirectToAction("AllUsers", "Account");
                    }

                }
                catch (SqlException sqlex)
                {

                    _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
                    response = RedirectToAction("Index", "Home");
                }
                catch (Exception ex)

                {
                    _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                    response = RedirectToAction("Index", "Home");
                }
            }
            else
            {
                response = RedirectToAction("Login", "Account");
            }
            return response;
        }

        [HttpPost]
        public ActionResult UpdateUser(User form)
        {
            ActionResult response;
            if (Session["RoleId"] != null && (long)Session["RoleId"] == 5)
            {
                try
                {


                    if (ModelState.IsValid)
                    {
                        //map form
                        UserDO userDO = _Mapper.MapPoToDo(form);
                        _UserDataAccess.UpdateUser(userDO);

                        response = RedirectToAction("AllUsers", "Account");
                    }
                    else
                    {
                        ViewBag.DropList = new List<SelectListItem>();
                        ViewBag.DropList.Add = (new SelectListItem() { Text = "User", Value = 1.ToString() });
                        ViewBag.DropList.Add = (new SelectListItem() { Text = "Moderator", Value = 3.ToString() });
                        ViewBag.DropList.Add = (new SelectListItem() { Text = "Admin", Value = 5.ToString() });
                        //refill your drop down list, or else.
                        //You have been warned!!!

                        response = View(form);
                    }
                }
                catch (SqlException sqlex)
                {

                    _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
                    response = RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                    response = RedirectToAction("Index", "Home");
                }
            }
            else
            {
                response = RedirectToAction("Login", "Account");
            }

            return response;

        }
        [HttpGet]
        public ActionResult LogOut()
        {
           
            
                Session.Abandon();
                return RedirectToAction("Index", "Home");
            
            
           

        }
    }
}