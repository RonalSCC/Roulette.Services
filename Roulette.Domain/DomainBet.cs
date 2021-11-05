using Roulette.Access;
using Roulette.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Domain
{
    public class DomainBet
    {
        public string connectionString = string.Empty;
        /// <summary>
        /// Connection method to register a bet
        /// </summary>
        /// <param name="entidad">Object EntityBet to insert in BD</param>
        public int RegisterBet(EntityBet entidad)
        {
            AccessBet.connectionString = connectionString;
            return AccessBet.Register_Bet(entidad);
        }
        /// <summary>
        /// Connection method to get specific Bet
        /// </summary>
        /// <param name="betID">Bet ID to search</param>
        public EntityBet GetBetByID(int? betID = null)
        {
            AccessBet.connectionString = connectionString;
            return AccessBet.Read_BetById(betID);
        }        
        /// <summary>
        /// Connection method to get all Bets
        /// </summary>

        public List<EntityBet> ListBets()
        {
            AccessBet.connectionString = connectionString;
            return AccessBet.Read_Bets();
        }
    }
}
