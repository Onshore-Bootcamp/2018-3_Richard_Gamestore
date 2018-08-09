using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamestore_BLL.Models
{
    public class CalculationBO
    {
        public long GameId { get; set; }
        public long OrderId { get; set; }
        public long GameOrderId { get; set; }
        public decimal Price { get; set; }
    }
}
