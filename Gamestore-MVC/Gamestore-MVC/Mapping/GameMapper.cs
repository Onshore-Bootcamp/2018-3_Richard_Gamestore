using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gamestore_DAL.Models;
using Gamestore_MVC.Models;
using System.Data.SqlClient;

namespace Gamestore_MVC.Mapping
{
    public class GameMapper
    {
        public Game MapDoToPo(GameDO from)
        {
            Game to = new Game();
            to.GameId = from. GameId;
            to.Title = from.Title;
            to. Description = from. Description;
            to. DevelopingCompany = from. DevelopingCompany;
            to. Condition = from. GameCondition;
            to.Price = from.Price;
            return to;
        }

        public GameDO MapPoToDo(Game from)
        {
            GameDO to = new GameDO();
            to.GameId = from.GameId;
            to.Title = from.Title;
            to.Description = from.Description;
            to.DevelopingCompany = from. DevelopingCompany;
            to.GameCondition= from.Condition;
            to.Price = from.Price;
            return to;
        }
    }
}
