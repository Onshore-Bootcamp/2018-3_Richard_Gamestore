using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Gamestore_MVC.Models
{
    public class User
    {
        public long UserId { get; set; }

        [Required(ErrorMessage = "Please Enter Username")]
        [MaxLength(15, ErrorMessage = "Max length is 15")]
        [MinLength(4, ErrorMessage = "Minimum Length is 4")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Hey You Forgot Something!!!")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d).{4,15}$", ErrorMessage = "Password must be between 4 and 15 digits long and include at least one number.")]
        public string Password { get; set; }

        public long RoleId { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email Must Contain an @ and .com")]
        public string Email { get; set; }

        public string RoleName { get; set; }




    }
}