using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamestore_DAL.Models
{
    public class OrderDO
    {
        public long OrderId { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        
        public string Phone { get; set; }
        public long UserId { get; set; }
    }
}
