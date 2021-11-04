using Roulette.Access;
using Roulette.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Domain
{
    public class DomainBet
    {
        public int RegisterBet(EntityBet entidad)
        {
            return AccessBet.Register_Bet(entidad);
        }

        public EntityBet GetBetByID(int? betID = null)
        {
            return AccessBet.Read_BetById(betID);
        }
    }
}
