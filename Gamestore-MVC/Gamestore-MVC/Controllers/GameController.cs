using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gamestore_MVC.Mapping;
using Gamestore_MVC.Models;
using Gamestore_DAL.Models;
using Gamestore_DAL.Mapping;
using Gamestore_DAL;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using Gamestore_MVC;




namespace Gamestore_MVC.Controllers
{
    public class GameController : Controller
    {

        private static Logger _Logger;


        private GameMapper _Mapper = new GameMapper();

        private readonly GameDAO _GameDataAccess;
        public GameController()
        {
            //todo: change Datasource name
            string log = ConfigurationManager.AppSettings["ErrorLogPath"];
            string connectionString = ConfigurationManager.ConnectionStrings["DataSource"].ConnectionString;
            _Logger = new Logger(log);
            _GameDataAccess = new GameDAO(connectionString, log);
        }

        // GET: Game
        [HttpGet]
        public ActionResult AddGame()
        {
            ActionResult response;
            if ((long)Session["RoleId"] == 5)
            {
                response = View();
            }
            else
            {
                response = RedirectToAction("ViewAllGames", "Game");
            }
            return response;

        }


        /// <summary>
        /// post the game info that was created to the data base
        /// </summary>
      
        [HttpPost]
        public ActionResult AddGame(Game form)

        {
            ActionResult response;
            // checks role 
            if (Session["RoleId"] != null && (long)Session["RoleId"] == 5)
            {
                try
                {
                    //check modelstate
                    if (ModelState.IsValid)

                    {
                        //maps dataObject new game DO
                        GameDO dataObject = new GameDO();
                        dataObject.GameId = form.GameId;
                        dataObject.Title = form.Title;
                        dataObject.Description = form.Description;
                        dataObject.DevelopingCompany = form.DevelopingCompany;
                        dataObject.GameCondition = form.Condition;
                        dataObject.Price = form.Price;
                        //Adds a game based off the dao
                        _GameDataAccess.AddGame(dataObject);
                        //Temporarily holds onto the title
                        TempData["Title"] = dataObject.Title;
                        response = RedirectToAction("AddGame", "Game");

                    }
                    else
                    { // sets the response to View the form
                        response = View(form);
                    }
                }

                //catches and exception and logs it
                catch (SqlException sqlex)

                {
                    _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
                    response = RedirectToAction("AllUsers", "Account");
                }
                catch (Exception ex)
                {
                    _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                    response = RedirectToAction("AllUsers", "Account");
                }
            }
            else//if the check fails sets the response
            {
                response = RedirectToAction("Login", "Account");
            }
            

            return response;
        }

        [HttpGet]
        public ActionResult Delete(long GameId)


        {
            //set response
            ActionResult response;
            // do a check
            if (Session["RoleId"] != null && (long)Session["RoleId"] == 5)
            {
                try
                {

                        //pulls from database to push one game
                    GameDO game = _GameDataAccess.ViewGameByGameId(GameId);
                    //adding delete function
                    Game deletedGame = _Mapper.MapDoToPo(game);
                    //deleting game by id
                    GameId = deletedGame.GameId;
                    //check id CANT be default or less than 0
                    if (GameId > 0)
                    {
                        //delteing the game
                        _GameDataAccess.DeleteGame(GameId);
                        //once deleted rta to view all games
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
            return RedirectToAction("ViewAllGames", "Game");
        }

        public ActionResult ViewAllGames()
        {
            //setting response 
            ActionResult response;
            //making new list 
            List<Game> mappedGames = new List<Game>();
            try
            {
                //puls list from sql to push games
                List<GameDO> ViewAllGames = _GameDataAccess.ViewAllGames();



                //check
                foreach (GameDO dataObject in ViewAllGames)
                {
                    //adding the list of gamedos 
                    mappedGames.Add(_Mapper.MapDoToPo(dataObject));
                    dataObject.GameId = 1;
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
            //views the mapped games
            response = View(mappedGames);
            return response;
        }
        // game datails grabs the id of that game and shows the details
        public ActionResult GameDetails(long GameId)

        {
            ActionResult result;


            try
            {
                //check
                if (Session["RoleId"] != null && (long)Session["RoleId"] >= 3)
                {

                    //instantiate game by game id
                    GameDO game = _GameDataAccess.ViewGameByGameId(GameId);
                    //maps from do to po
                    Game detailsGame = _Mapper.MapDoToPo(game);
                   
                    //check
                    if (detailsGame.GameId > 0)
                    {
                        //returns the view
                        result = View(detailsGame);
                    }
                    else
                    {
                        //result = rta to view all games
                        result = RedirectToAction("ViewAllGames", "Game");
                    }

                }
                //if check fails
                else
                {
                    result = RedirectToAction("Login", ("Account"));
                }
            }
            catch (SqlException sqlex)
            {
                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
                result = RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                result = RedirectToAction("Index", "Home");

            }


            return result;

        }
        //udate game gets information to update that game
        [HttpGet]
        public ActionResult UpdateGame(long GameId)
        {
            //sets response 
            ActionResult responce;
            try
            {
                //instantiate updategame by game id
                GameDO updategame = _GameDataAccess.ViewGameByGameId(GameId);
                //maps from do to po
                Game gamedetails = _Mapper.MapDoToPo(updategame);
               
                //checks
                if (updategame.GameId > 0)
                {
                    responce = View(gamedetails);
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

        [HttpPost]
        public ActionResult UpdateGame(Game form)
        {
            //setting the response
            ActionResult response;
            //check
            if (ModelState.IsValid)
            {
                try
                {
                    //mapping gamedo to po
                    GameDO gameDO = _Mapper.MapPoToDo(form);

                    _GameDataAccess.UpdateGame(gameDO);

                    response = RedirectToAction("ViewAllGames", "Game");
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
                response = View(form);
            }

            return response;
        }



    }
}