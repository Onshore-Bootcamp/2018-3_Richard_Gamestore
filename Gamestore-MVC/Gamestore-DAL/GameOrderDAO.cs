using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamestore_DAL.Mapping;
using Gamestore_DAL.Models;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;

namespace Gamestore_DAL
{
    public class GameOrderDAO
    {
        private static Logger _Logger;
        private static GameOrderMapper _Mapper = new GameOrderMapper();


        private readonly string _ConnectionString;
        public GameOrderDAO(string connectionString, string log)
        {
            _Logger = new Logger(log);
            _ConnectionString = connectionString;
        }

        public void OrderCreate(GameOrderDO OrderCreate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                using (SqlCommand command = new SqlCommand("GAMEORDER_CREATE", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 60;
                    command.Parameters.AddWithValue("@GameId", OrderCreate.GameId);
                    command.Parameters.AddWithValue("@OrderId", OrderCreate.OrderId);

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

        public List<GameOrderDO> ViewAllGameOrders()
        {
            List<GameOrderDO> viewAllGameOrders = new List<GameOrderDO>();

            try
            {
                using (SqlConnection connectionGamestore = new SqlConnection(_ConnectionString))
                using (SqlCommand ViewGameOrdersCommand = new SqlCommand("GAMEORDER_VIEW_ALL", connectionGamestore))
                {
                    ViewGameOrdersCommand.CommandType = CommandType.StoredProcedure;

                    connectionGamestore.Open();
                    using (SqlDataReader sqlDataReader = ViewGameOrdersCommand.ExecuteReader())
                    {

                        //read entries from the database
                        while (sqlDataReader.Read())
                        {
                            GameOrderDO order = _Mapper.MapReaderToSingle(sqlDataReader);
                            viewAllGameOrders.Add(order);
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
            return viewAllGameOrders;

        }
    }
}

