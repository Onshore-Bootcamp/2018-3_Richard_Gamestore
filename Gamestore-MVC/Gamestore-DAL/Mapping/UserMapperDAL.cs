using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamestore_DAL.Models;
using System.Data.SqlClient;

namespace Gamestore_DAL.Mapping
{
    public class UserMapperDAL
    {

        public UserDO MapReaderToSingle(SqlDataReader reader)
        {
            UserDO result = new UserDO();

            if (reader["UserId"] != DBNull.Value)
            {
                result.UserId = (long)reader["UserId"];
            }
            if (reader["Username"] != DBNull.Value)
            {
                result.UserName = (string)reader["Username"];
            }
            if (reader["Password"] != DBNull.Value)
            {
                result.Password= (string)reader["Password"];
            }
            if (reader["RoleId"] != DBNull.Value)
            {
                result.RoleId = (long)reader["RoleId"];
            }
            if (reader["Email"] != DBNull.Value)
            {
                result.Email = (string)reader["Email"];
            }
            if (reader["RoleName"] != DBNull.Value)
            {
                result.RoleName = (string)reader["RoleName"];
            }
            return result;
        }
    }
}
