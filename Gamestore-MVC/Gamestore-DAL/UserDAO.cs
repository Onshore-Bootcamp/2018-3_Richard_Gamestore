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
    public class UserDAO
    {

        private static Logger _Logger;
        private static UserMapperDAL _Mapper = new UserMapperDAL();
        private readonly string _ConnectionString;
        public UserDAO(string connectionString, string log)
        { 
            _Logger = new Logger(log);
            _ConnectionString = connectionString;
        }
        public UserDO ViewUserByUserName(string username)
        {
            UserDO user = new UserDO();

            try
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                using (SqlCommand command = new SqlCommand("USER_VIEW_BY_USERNAME", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 60;

                    command.Parameters.AddWithValue("@UserName", username);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user.UserId = reader["UserId"] != DBNull.Value ? (long)reader["UserId"] : 0;
                            user.UserName = reader["Username"] != DBNull.Value ? (string)reader["Username"] : null;
                            user.Password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : null;
                            user.RoleId = reader["RoleId"] != DBNull.Value ? (long)reader["RoleId"] : 1;
                            user.RoleName = reader["RoleName"] != DBNull.Value ? (string)reader["RoleName"] : null;
                            user.Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : null;
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
            return user;
        }


        public void RegisterUser(UserDO userToRegister)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                using (SqlCommand command = new SqlCommand("USER_CREATE", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 60;

                    command.Parameters.AddWithValue("@UserName", userToRegister.UserName);
                    command.Parameters.AddWithValue("@Password", userToRegister.Password);
                    command.Parameters.AddWithValue("@RoleId", userToRegister.RoleId);
                    command.Parameters.AddWithValue("@Email", userToRegister.Email);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public void UpdateUser(UserDO user)
        {
            if (user.UserId == 0)
                throw new ArgumentException("UserId cannot be 0 when updating.");

            try
            {
                using (SqlConnection connectionGamestore= new SqlConnection(_ConnectionString))
                using (SqlCommand updateUser = new SqlCommand("USERS_UPDATE", connectionGamestore))
                {
                    updateUser.CommandType = CommandType.StoredProcedure;
                    updateUser.CommandTimeout = 60;

                    updateUser.Parameters.AddWithValue("@UserId", user.UserId);
                    updateUser.Parameters.AddWithValue("@Username", user.UserName);
                    updateUser.Parameters.AddWithValue("@Password", user.Password);
                    updateUser.Parameters.AddWithValue("@RoleId", user.RoleId);
                    updateUser.Parameters.AddWithValue("@Email", user.Email);


                    connectionGamestore.Open();
                    updateUser.ExecuteNonQuery();
                    connectionGamestore.Close();
                }
            }
            catch (Exception ex)
            {
                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
           
            }
        }
        public void DeleteUser(long userID)
        {
            try
            {
                using (SqlConnection connectionGamestore = new SqlConnection(_ConnectionString))
                using (SqlCommand deleteUser = new SqlCommand("USERS_DELETE", connectionGamestore))
                {
                    deleteUser.CommandType = CommandType.StoredProcedure;
                    deleteUser.CommandTimeout = 60;
                    deleteUser.Parameters.AddWithValue("@UserId", userID);

                    connectionGamestore.Open();
                    deleteUser.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                _Logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        public List<UserDO> ViewUsers()
        {
            List<UserDO> displayUsers = new List<UserDO>();

            try
            {
                using (SqlConnection connectionGameStore = new SqlConnection(_ConnectionString))
                using (SqlCommand viewUsers = new SqlCommand("USERS_VIEW_ALL", connectionGameStore))
                {
                    viewUsers.CommandType = CommandType.StoredProcedure;
                    connectionGameStore.Open();
                    using (SqlDataReader sqlDataReader = viewUsers.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            UserDO user = _Mapper.MapReaderToSingle(sqlDataReader);
                            displayUsers.Add(user);
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
            return displayUsers;
        }
        public UserDO ViewUserById(long userId)
        {
            UserDO user = new UserDO();

            try
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                using (SqlCommand command = new SqlCommand("USER_VIEW_BY_ID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 60;

                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user.UserId = reader["UserId"] != DBNull.Value ? (long)reader["UserId"] : 0;
                            user.UserName = reader["Username"] != DBNull.Value ? (string)reader["Username"] : null;
                            user.Password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : null;
                            user.RoleId = reader["RoleId"] != DBNull.Value ? (long)reader["RoleId"] : 1;
                            user.RoleName = reader["RoleName"] != DBNull.Value ? (string)reader["RoleName"] : null;
                            user.Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : null;
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
            return user;
        }
    }
}
