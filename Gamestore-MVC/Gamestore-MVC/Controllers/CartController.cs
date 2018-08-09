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
    public class CartController : Controller
    {
        private static Logger _Logger;


        GameMapper _Mapper = new GameMapper();

        private readonly GameDAO _GameDataAccess;
        public CartController()
        {
            string log = ConfigurationManager.AppSettings["ErrorLogPath"];
            string connectionString = ConfigurationManager.ConnectionStrings["DataSource"].ConnectionString;
            _Logger = new Logger(log);
            _GameDataAccess = new GameDAO(connectionString, log);
        }

        // GET: Cart
        public ActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public ActionResult AddToCart(long GameId)
        {
            ActionResult responce;

            List<long> cart = (List<long>)Session["Cart"];
            try
            {



                if (cart == null)
                {
                    cart = new List<long>();
                    cart.Add(GameId);
                }
                else
                {
                    cart.Add(GameId);
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

            Session["Cart"] = cart;
            responce = RedirectToAction("ViewAllGames", "Game");
            return responce;
        }
        [HttpGet]
        public ActionResult RemoveGame(long GameId)
        {
            ActionResult response;
            List<long> cart = (List<long>)Session["Cart"];
            try
            {

                if (GameId > 0)
                {
                    cart.Remove(GameId);
                    response = RedirectToAction("ViewAllGames", "Game");
                }
                else
                {
                    response = RedirectToAction("ViewAllGames", "Game");
                }


            }
            catch (SqlException sqlex)
            {
                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);

            }
            if (cart.Count > 0)
            {
                Session["Cart"] = cart;
            }
            else
            {
                Session["Cart"] = new List<long>();
            }
            response = RedirectToAction("ViewAllGames", "Game");
            return response;
        }

        [HttpGet]
        public ActionResult ViewCart()
        {
            List<long> cart = (List<long>)Session["Cart"];
            List<Game> mappedGames = new List<Game>();
            try
            {
                foreach (long gameId in cart)
                {
                    //call on game dao then call on method
                    GameDO game = _GameDataAccess.ViewGameByGameId(gameId);

                    mappedGames.Add(_Mapper.MapDoToPo(game));
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
            return View(mappedGames);
        }
    }
}
