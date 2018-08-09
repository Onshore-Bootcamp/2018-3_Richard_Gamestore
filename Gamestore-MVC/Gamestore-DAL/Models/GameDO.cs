using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamestore_DAL.Models
{
    public class GameDO
    {
        public long GameId  { get; set; }
        public string Title { get; set; } 
        public string Description { get; set; }
        public string DevelopingCompany { get; set; }
        public string GameCondition { get; set; }
        public decimal Price { get; set; }



    }
}
