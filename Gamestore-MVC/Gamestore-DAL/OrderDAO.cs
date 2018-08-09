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
    public class OrderDAO
    {
        private static Logger _Logger;
        private static OrderMapperDAL _Mapper = new OrderMapperDAL();


        private readonly string _ConnectionString;
        public OrderDAO(string connectionString, string log)
        {
            _Logger = new Logger(log);
            _ConnectionString = connectionString;
        }


        public List<OrderDO> ViewOrder()
        {
            List<OrderDO> order = new List<OrderDO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                using (SqlCommand command = new SqlCommand("ORDER_VIEW", connection))
                {


                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 60;
                
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderDO orders = _Mapper.MapReaderToSingle(reader);
                            order.Add(orders);

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
            return order;
        }


        public long CreateOrder(OrderDO OrderCreate)
        {
            long GameId = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                using (SqlCommand command = new SqlCommand("ORDER_CREATE", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 60;
                    command.Parameters.AddWithValue("@EmailAddress", OrderCreate.EmailAddress);
                    command.Parameters.AddWithValue("@Address", OrderCreate.Address);
                    command.Parameters.AddWithValue("@Phone", OrderCreate.Phone);
                    command.Parameters.AddWithValue("@UserId", OrderCreate.UserId);

                    connection.Open();
                    //pulls back the Id of the row you entered
                    object tempGameId = command.ExecuteScalar();
                    GameId = Convert.ToInt64(tempGameId);
                    connection.Close();
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

            return GameId;
        }


        public void UpdateOrder(OrderDO orderUpdate)
        {
            try
            {
                using (SqlConnection connectionGamestore = new SqlConnection(_ConnectionString))
                using (SqlCommand updateOrder = new SqlCommand("ORDER_UPDATE", connectionGamestore))
                {
                    updateOrder.CommandType = CommandType.StoredProcedure;

                    updateOrder.CommandTimeout = 60;
          
                    updateOrder.Parameters.AddWithValue("@OrderId", orderUpdate.OrderId);
                    updateOrder.Parameters.AddWithValue("@Email_Address", orderUpdate.EmailAddress);
                    updateOrder.Parameters.AddWithValue("@Address", orderUpdate.Address);
                    updateOrder.Parameters.AddWithValue("@Phone", orderUpdate.Phone);
                 


                    connectionGamestore.Open();
                    updateOrder.ExecuteNonQuery();

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


        public void RemoveOrder(long OrderId)
        {
            try
            {

                using (SqlConnection connectionGamestore = new SqlConnection(_ConnectionString))
                using (SqlCommand DeleteOrder = new SqlCommand("ORDER_DELETE", connectionGamestore))
                {
                    DeleteOrder.CommandType = CommandType.StoredProcedure;
                    DeleteOrder.CommandTimeout = 60;
                    DeleteOrder.Parameters.AddWithValue("@OrderId", OrderId);

                    connectionGamestore.Open();
                    DeleteOrder.ExecuteNonQuery();
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

        public OrderDO ViewOrderById(long orderId)
        {
            OrderDO order = new OrderDO();

            try
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionString))
                using (SqlCommand command = new SqlCommand("ORDER_VIEW_BY_ID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 60;

                    command.Parameters.AddWithValue("@OrderId", orderId);
           

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            order.OrderId = reader["OrderId"] != DBNull.Value ? (long)reader["OrderId"] : 0;
                            order.EmailAddress = reader["Email Address"] != DBNull.Value ? (string)reader["Email Address"] : null;
                            order.Address = reader["Address"] != DBNull.Value ? (string)reader["Address"] : null;
                            order.Phone = reader["Phone"] != DBNull.Value ? (string)reader["Phone"] : null;
                            order.UserId = reader["UserId"] != DBNull.Value ? (long)reader["UserId"] : 0;
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
            return order;

        }
    }
}



