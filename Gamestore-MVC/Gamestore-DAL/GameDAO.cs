using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamestore_DAL.Mapping;
using Gamestore_DAL.Models;
using System.Data;
using System.Reflection;

namespace Gamestore_DAL
{
    public class GameDAO
    {
        private static Logger _Logger;
        private static GameMapperDAL _Mapper = new GameMapperDAL();

        private readonly string _ConnectionString;
        public GameDAO(string connectionString, string log)
        {
            _Logger = new Logger (log);
            _ConnectionString = connectionString;
        }


        public GameDO ViewGameByGameId(long GameId)
        {
            GameDO game = new GameDO();

            try
            {
                //establishing a connection
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                    //setting command
                using (SqlCommand command = new SqlCommand("GAME_VIEW_BY_ID", connection))
                {
                    //setting the command type to stored procedure
                    command.CommandType = CommandType.StoredProcedure;
                    //setting the command timeout
                    command.CommandTimeout = 60;
                    //taking in the parameter gameid
                    command.Parameters.AddWithValue("@GameId", GameId);
                    //open the connection
                    connection.Open();
                    //setting the sql data reader
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        //stament that while the reader is reading the data we will instantiate these parameters
                        while (reader.Read())
                        {
                            game.GameId = reader["GameId"] != DBNull.Value ? (long)reader["GameId"] : 0;
                            game.Title = reader["Title"] != DBNull.Value ? (string)reader["Title"] : null;
                            game.Price = reader["Price"] != DBNull.Value ? (decimal)reader["Price"] : default(decimal);
                            game.Description = reader["Description"] != DBNull.Value ? (string)reader["Description"] : null;
                            game.DevelopingCompany = reader["Developing Company"] != DBNull.Value ? (string)reader["Developing Company"] : null;
                            game.GameCondition = reader["Condition"] != DBNull.Value ? (string)reader["Condition"] : null;
                        }
                    }


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
            return game;



        }

        public void AddGame(GameDO GameToCreate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                using (SqlCommand command = new SqlCommand("GAME_CREATE", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 60;
                    command.Parameters.AddWithValue("@Price", GameToCreate.Price);
                    command.Parameters.AddWithValue("@Title", GameToCreate.Title);
                    command.Parameters.AddWithValue("@Description", GameToCreate.Description);
                    command.Parameters.AddWithValue("@Developing_Company", GameToCreate.DevelopingCompany);
                    command.Parameters.AddWithValue("@Condition", GameToCreate.GameCondition);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

            }
            catch (SqlException sqlex)
            {
                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
            }
        }

        public void UpdateGame(GameDO gameUpdate)
        {
            try
            {
                
                using (SqlConnection connectionGamestore = new SqlConnection(_ConnectionString))
                using (SqlCommand updateGame = new SqlCommand("GAME_UPDATE", connectionGamestore))
                {
                    //sets commmand type
                    updateGame.CommandType = CommandType.StoredProcedure;
                    //sets timeout
                    updateGame.CommandTimeout = 60;
                    //takes in these parameters
                    updateGame.Parameters.AddWithValue("@GameId", gameUpdate.GameId);
                    updateGame.Parameters.AddWithValue("@Title", gameUpdate.Title);
                    updateGame.Parameters.AddWithValue("@Description", gameUpdate.Description);
                    updateGame.Parameters.AddWithValue("@Developing_Company", gameUpdate.DevelopingCompany);
                    updateGame.Parameters.AddWithValue("@Condition", gameUpdate.GameCondition);
                    updateGame.Parameters.AddWithValue("@Price", gameUpdate.Price);

                    //open the connection
                    connectionGamestore.Open();
                    updateGame.ExecuteNonQuery();

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
        }

        public void DeleteGame(long GameId)
        {
            try 
            {

                using (SqlConnection connectionGamestore = new SqlConnection(_ConnectionString))
                using (SqlCommand deleteGame = new SqlCommand("GAME_DELETE", connectionGamestore))
                {
                    //sets the command type 
                    deleteGame.CommandType = CommandType.StoredProcedure;
                    //sets timeout
                    deleteGame.CommandTimeout = 60;
                    //takes in parameter game id
                    deleteGame.Parameters.AddWithValue("@GameId", GameId);
                    //open the connection
                    connectionGamestore.Open();
                    deleteGame.ExecuteNonQuery();
                }

            }
            catch (SqlException sqlex)
            {
                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);

            }
        }

        public List<GameDO> ViewAllGames()
        {
            //makes a new list of gamedo
            List<GameDO> displayGame = new List<GameDO>();

            try
            {
                using (SqlConnection connectionGamestore = new SqlConnection(_ConnectionString))
                using (SqlCommand ViewAllGames = new SqlCommand("GAME_VIEW_ALL", connectionGamestore))
                {
                   //sets the command type
                    ViewAllGames.CommandType = CommandType.StoredProcedure;
                  //open connection
                    connectionGamestore.Open();

                    using (SqlDataReader sqlDataReader = ViewAllGames.ExecuteReader())
                    {

                        //read entries from the database
                        while (sqlDataReader.Read())
                        {
                            //mapping
                            GameDO game = _Mapper.MapReaderToSingle(sqlDataReader);
                            //add the game
                            displayGame.Add(game);
                        }
                    }
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
            return displayGame;
        }

        public void RemoveGame(long GameId)
        {
            try
            {

                using (SqlConnection connectionGamestore = new SqlConnection(_ConnectionString))
                using (SqlCommand RemoveGame = new SqlCommand("GAMEORDER_DELETE", connectionGamestore))
                {
                    //sets the command type 
                    RemoveGame.CommandType = CommandType.StoredProcedure;
                    //sets the timeout
                    RemoveGame.CommandTimeout = 60;
                    RemoveGame.Parameters.AddWithValue("@GameId", GameId);
                    //open the connection
                    connectionGamestore.Open();
                    //executes the sql statment
                    RemoveGame.ExecuteNonQuery();
                }

            }
            catch (SqlException sqlex)
            {
                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);

            }
        }
    }
}
