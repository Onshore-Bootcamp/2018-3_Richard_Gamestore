using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamestore_DAL.Models
{
    public class UserDO
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        
        public long RoleId { get; set; }

        public string Email { get; set; }

        public string RoleName { get; set; }

    }
}
