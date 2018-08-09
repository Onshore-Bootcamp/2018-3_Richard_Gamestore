using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamestore_DAL.Models;
using System.Data.SqlClient;


namespace Gamestore_DAL.Mapping
{
    public class OrderMapperDAL
    {
        public OrderDO MapReaderToSingle(SqlDataReader reader)
        {
            OrderDO result = new OrderDO();

            if (reader["OrderId"] != DBNull.Value)
            {
                result.OrderId= (long)reader["OrderId"];
            }

            if (reader["Email Address"] != DBNull.Value)
            {
                result.EmailAddress = (string)reader["Email Address"];
            }

            if (reader["Address"] != DBNull.Value)
            {
                result.Address = (string)reader["Address"];
            }
            if (reader["Phone"] != DBNull.Value)
            {
                result.Phone = (string)reader["Phone"];
            }
            if (reader["UserId"] != DBNull.Value)
            {
                result.UserId = (long)reader["UserId"];
            }
          
            return result;
        }
    }
}
