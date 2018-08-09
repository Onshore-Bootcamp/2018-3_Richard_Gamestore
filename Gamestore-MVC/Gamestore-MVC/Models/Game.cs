using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Gamestore_MVC.Models
{
    public class Game
    {
        public long GameId { get; set; }
        [Required]
        public decimal Price { get; set; }
       [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string DevelopingCompany { get; set; }
        [Required]
        public string Condition { get; set; }
    }
}