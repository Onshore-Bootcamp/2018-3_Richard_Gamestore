using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamestore_DAL.Models;
using System.Data.SqlClient;


namespace Gamestore_DAL.Mapping
{
    public class GameMapperDAL
    {
        public GameDO MapReaderToSingle(SqlDataReader reader)
        {
           GameDO result = new GameDO();

            if (reader["GameId"] != DBNull.Value)
            {
                result.GameId = (long)reader["GameId"];
            }

            if (reader["Title"] != DBNull.Value)
            {
                result.Title = (string)reader["Title"];
            }

            if (reader["Description"] != DBNull.Value)
            {
                result.Description  = (string)reader["Description"];
            }
            if (reader["Developing Company"] != DBNull.Value)
            {
                result.DevelopingCompany = (string)reader["Developing Company"];
            }
            if (reader["Condition"] != DBNull.Value)
            {
                result.GameCondition = (string)reader["Condition"];
            }
            if (reader["Price"] != DBNull.Value)
            {
                result.Price = (decimal)reader["Price"];
            }
            return result;
        }
    }
}
