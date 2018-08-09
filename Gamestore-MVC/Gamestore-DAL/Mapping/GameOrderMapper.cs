using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamestore_DAL.Models;

namespace Gamestore_DAL.Mapping
{
    class GameOrderMapper
    {
        public GameOrderDO MapReaderToSingle(SqlDataReader reader)
        {
            GameOrderDO result = new GameOrderDO();

            if (reader["GameId"] != DBNull.Value)
            {
                result.GameId = (long)reader["GameId"];
            }

         
            if (reader["Price"] != DBNull.Value)
            {
                result.Price = (decimal)reader["Price"];
            }
            return result;
        }
    }
}

