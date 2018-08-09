using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gamestore_DAL.Mapping;
using Gamestore_DAL.Models;
using Gamestore_MVC.Models;


namespace Gamestore_MVC.Mapping
{
    public class UserMapper
    {
        public User MapDoToPo(UserDO from)
        {
            User to = new User();
            to.UserId = from.UserId;
            to.UserName = from.UserName;
            to.Password = from.Password;
            to.RoleId = from.RoleId;
            to.Email = from.Email;
            to.RoleName = from.RoleName;

            return to;
        }

        public UserDO MapPoToDo(User from)
        {
            UserDO to = new UserDO();
            to.UserId = from.UserId;
            to.UserName = from.UserName;
            to.Password = from.Password;
            to.RoleId = from.RoleId;
            to.Email = from.Email;
            to.RoleName = from.RoleName;
            return to;

        }
    }
}