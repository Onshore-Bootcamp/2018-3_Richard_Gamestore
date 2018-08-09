using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamestore_DAL.Models
{
   public class GameOrderDO
    {
        public GameOrderDO(long GameId, long OrderId)
        {
            this.GameId = GameId;
            this.OrderId = OrderId;
            
        }
        public GameOrderDO()
        {

        }

        public long GameId { get; set; }
        public long OrderId { get; set; }
        public long GameOrderId { get; set; }
        public decimal Price { get; set; }

    }
}
