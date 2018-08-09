using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gamestore_MVC.Models
{
    public class Calculation
    {
        public long GameId { get; set; }
        public long OrderId { get; set; }
        public long GameOrderId { get; set; }
        public decimal Price { get; set; }
    }
}