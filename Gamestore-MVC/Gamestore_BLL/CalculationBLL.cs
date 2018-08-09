using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamestore_BLL.Models;



namespace Gamestore_BLL
{
    public class CalculationBLL
    {
        //take in list of bo
        public long Total(List<CalculationBO> calculation)
        {
          
            long value = (from Game in calculation
                          group Game by Game.GameId into GameGroup
                          orderby GameGroup.Sum(g => g.Price) descending
                          select GameGroup.Key).FirstOrDefault();
            return value;

        }
        



    }

}



