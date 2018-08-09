using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gamestore_MVC.Models;
using Gamestore_BLL.Models;
using Gamestore_BLL;
using Gamestore_DAL.Models;
using Gamestore_DAL;
using System.Configuration;
using Gamestore_MVC.Mapping;

namespace Gamestore_MVC.Controllers
{
    public class HomeController : Controller
    {
        private static Logger _Logger;
        private readonly GameOrderDAO _GameOrderDataAccess;
        private readonly CalculationBLL calc;
        private readonly GameDAO game;
        private readonly GameMapper mapper = new GameMapper();
        public HomeController()
        {


            string log = ConfigurationManager.AppSettings["ErrorLogPath"];

            string connectionString = ConfigurationManager.ConnectionStrings["DataSource"].ConnectionString;
            _Logger = new Logger(log);
            calc = new CalculationBLL();
            game = new GameDAO(connectionString, log);
            _GameOrderDataAccess = new GameOrderDAO(connectionString, log);
        }


        public ActionResult Index()
        {
            Game gamepo = new Game();
            try
            {
                List<CalculationBO> table = new List<CalculationBO>();
                List<GameOrderDO> gameorder = new List<GameOrderDO>();
                gameorder = _GameOrderDataAccess.ViewAllGameOrders();
                foreach (GameOrderDO view in gameorder)
                {
                    CalculationBO businesObject = new CalculationBO();
                    businesObject.GameId = view.GameId;
                    businesObject.GameOrderId = view.GameOrderId;
                    businesObject.Price = view.Price;
                    table.Add(businesObject);



                }
                long value = calc.Total(table);
                GameDO gamedo = new GameDO();

                gamedo = game.ViewGameByGameId(value);
                gamepo = mapper.MapDoToPo(gamedo);


            }
            catch
            {

            }

            return View(gamepo);
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}