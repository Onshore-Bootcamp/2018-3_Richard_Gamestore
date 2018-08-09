using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Gamestore_MVC.Models
{
    public class Order
    {
        public long OrderId { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }

        
        public long UserId { get; set; }
        public string UserName { get; set; }

    }
}