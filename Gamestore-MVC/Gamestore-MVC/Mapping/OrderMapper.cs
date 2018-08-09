using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gamestore_DAL.Models;
using Gamestore_MVC.Models;

namespace Gamestore_MVC.Mapping
{
    public class OrderMapper
    {
        public Order MapDoToPo(OrderDO from)
        {
            Order to = new Order();
            to.OrderId = from.OrderId;
            to.EmailAddress = from.EmailAddress;
            to.Address = from.Address;
            to.Phone = from.Phone;
            to.UserId = from.UserId;
            return to;
        }
        public OrderDO MapPoToDo(Order from)
        {
            OrderDO to = new OrderDO();
            to.OrderId = from.OrderId;
            to.EmailAddress = from.EmailAddress;
            to.Address = from.Address;
            to.Phone = from.Phone;
            to.UserId = from.UserId;
            return to;
        }
    }
}