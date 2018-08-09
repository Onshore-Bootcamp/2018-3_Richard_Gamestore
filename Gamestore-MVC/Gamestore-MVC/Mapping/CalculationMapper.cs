using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gamestore_BLL.Models;
using Gamestore_MVC.Models;

namespace Gamestore_MVC.Mapping
{
    public class CalculationMapper
    {
        public Calculation MapBoToPo(CalculationBO from)
        {
            Calculation to = new Calculation();
            to.GameId = from.GameId;
            to.GameOrderId = from.GameOrderId;
            to.OrderId = from.OrderId;
            to.Price = from.Price;
            return to;
        }
        public CalculationBO MapPotoDo(Calculation from)
        {
            CalculationBO to= new CalculationBO();
            to.GameId = from.GameId;
            to.GameOrderId = from.GameOrderId;
            to.OrderId = from.OrderId;
            to.Price = from.Price;
            return to;

        }
    }
}