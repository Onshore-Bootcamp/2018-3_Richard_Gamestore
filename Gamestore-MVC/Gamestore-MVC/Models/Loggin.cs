﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Gamestore_MVC.Models
{
    public class Login
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }


    }
}