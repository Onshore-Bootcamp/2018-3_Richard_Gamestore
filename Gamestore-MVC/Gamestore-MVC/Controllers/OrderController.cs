using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.Mvc;
using Gamestore_DAL;
using Gamestore_DAL.Models;
using Gamestore_MVC.Mapping;
using Gamestore_MVC.Models;

namespace Gamestore_MVC.Controllers
{
    public class OrderController : Controller
    {
        private static Logger _Logger;
        private string _conn;
        private static OrderMapper _Mapper = new OrderMapper();
        private static UserMapper _usermap = new UserMapper();
        private string _log;


        OrderMapper OrderMapper = new OrderMapper();

        private readonly OrderDAO _OrderDataAccess;
        private readonly GameOrderDAO _GameOrderDataAccess;
        private readonly UserDAO _UserDataAccess;
        public OrderController()
        {
            //todo: change Datasource name
            _log = ConfigurationManager.AppSettings["ErrorLogPath"];
            _conn = ConfigurationManager.ConnectionStrings["DataSource"].ConnectionString;

            _Logger = new Logger(_log);
            _OrderDataAccess = new OrderDAO(_conn, _log);
            _UserDataAccess = new UserDAO(_conn, _log);
            _GameOrderDataAccess = new GameOrderDAO(_conn, _log);
        }
        [HttpGet]
        public ActionResult CreateOrder()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CreateOrder(Order form)
        {
            //set response
            ActionResult response;
            try
            {
                //check
                if ((long)Session["RoleId"] > 0)
                {
                    if (ModelState.IsValid)
                    {
                        //makeing list of long game ids = to the sessiond cart
                        List<long> cart = (List<long>)Session["Cart"];
                        //check to see if cart is null or 0
                        if (cart == null)
                        {
                            return View();
                        }
                        else
                        {

                        }
                        //check to see if userid is null
                        if (Session["UserId"] == null)
                        {
                            response = RedirectToAction("Register", "Account");
                        }
                        else
                        {
                            //sets userid = to the session userid
                            long userId = (long)Session["UserId"];
                            //maping data
                            OrderDO dataObject = new OrderDO();
                            dataObject.OrderId = form.OrderId;
                            dataObject.EmailAddress = form.EmailAddress;
                            dataObject.Address = form.Address;
                            dataObject.Phone = form.Phone;
                            dataObject.UserId = userId;

                            //creating order and getting back the id
                            dataObject.OrderId = _OrderDataAccess.CreateOrder(dataObject);
                            
                            long orderId = dataObject.OrderId;
                            //make statement that every long gameid that is in the sessioned cart
                            foreach (long gameId in cart)
                            {
                                //instantiate gameorderdo based off of the game id and order id
                                GameOrderDO gameOrder = new GameOrderDO(gameId, orderId);
                                //call on the dao to create an gameorder
                                _GameOrderDataAccess.OrderCreate(gameOrder);
                            }

                            //setiing response
                            response = RedirectToAction("CreateOrder", "Order");
                        }
                    }
                    else
                    {
                        response = RedirectToAction("Login", "Account");
                    }
                }
                else
                {
                    response = RedirectToAction("Login", "Account");
                }
            }


            catch (SqlException sqlex)

            {
                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
                response = RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                response = RedirectToAction("Login", "Account");
            }

            return response;
        }

        [HttpGet]
        public ActionResult RemoveOrder(long OrderId)
        {
            ActionResult response = RedirectToAction("RemoveOrder", "Order");




            try
            {
                if (Session ["RoleId"]!= null && (long)Session["RoleId"] == 5)
                {
                    OrderDO order = _OrderDataAccess.ViewOrderById(OrderId);
                    Order RemoveOrder = _Mapper.MapDoToPo(order);
                    long orderId = RemoveOrder.OrderId;

                    if (OrderId > 0)
                    {
                        _OrderDataAccess.RemoveOrder(OrderId);
                        response = RedirectToAction("ViewOrder", "Order");
                    }
                    else
                    {
                        response = RedirectToAction("ViewOrder", "Order");
                    }

                }
                else
                {
                    response = RedirectToAction("ViewAllGames", "Game");
                }

            }
            catch (SqlException sqlex)
            {
                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
                response = RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                response = RedirectToAction("Login", "Account");

            }



            return response;
        }

        [HttpGet]
        public ActionResult ViewOrder()
        {
            //set response
            ActionResult response;
            //make new list of order pos 
            List<Order> mappedOrders = new List<Order>();
            //check
            if (Session["RoleId"] != null && (long)Session["RoleId"] == 5)
            {



                try
                {  
                    //list of order dos
                    List<OrderDO> AllOrders = _OrderDataAccess.ViewOrder();
                    //make use a new userdo
                    UserDO user = new UserDO();
                    //make order po new order
                    Order orderPO = new Order();
                    //statement
                    foreach (OrderDO dataObjet in AllOrders)
                    {
                        //pulling userId
                        user = _UserDataAccess.ViewUserById(dataObjet.UserId);
                        //mapping dataObject to po
                        orderPO = _Mapper.MapDoToPo(dataObjet);
                        //setting orderPo = to User 
                        orderPO.UserName = user.UserName;
                        //add the mapped orderPo 
                        mappedOrders.Add(orderPO);

                    }




                } 
                

                catch (SqlException sqlex)
                {

                    _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
                }
                catch (Exception ex)
                {
                    _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                }
                response = View(mappedOrders);
            }
            else
            {
                response = RedirectToAction("Login", "Account");
            }
           
            return response;
        }
        [HttpGet]
        public ActionResult UpdateOrder(long OrderId)
        {
            //set response
            ActionResult responce;
            try
            {
                //grabbing the order do order id
                OrderDO updateOrder = _OrderDataAccess.ViewOrderById(OrderId);
                //maping order do to orderpo
                Order Order = OrderMapper.MapDoToPo(updateOrder);
                //set orderid = to order.orderid
                OrderId = Order.OrderId;
                //check 
                if (OrderId > 0)
                {
                    responce = View(Order);
                }
                else
                {
                    responce = RedirectToAction("ViewAllGames", "Game");
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
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateOrder(Order form)
        {
            ActionResult respose;
            try
            {
                if (ModelState.IsValid)
                {
                    //maps order po to order do
                    OrderDO OrderDO = OrderMapper.MapPoToDo(form);
                    //da call for orderdo
                    _OrderDataAccess.UpdateOrder(OrderDO);

                    respose = View(form);
                }
                else
                {
                    respose = RedirectToAction("Login", "Account");
                }
            }
            catch (SqlException sqlex)
            {
                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
                respose = RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                respose = RedirectToAction("Index", "Home");
            }
            return respose;
        }
    }
}