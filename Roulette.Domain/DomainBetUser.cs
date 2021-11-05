using Roulette.Access;
using Roulette.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Domain
{
    public class DomainBetUser
    {
        public string connectionString = string.Empty;
        /// <summary>
        /// Connection method to register a bet user
        /// </summary>
        /// <param name="entidad">Object EntityBetUser to insert in BD</param>
        public bool RegisterBetUser(EntityBetUser entidad)
        {
            AccessBetUser.connectionString = connectionString;
            return AccessBetUser.Register_BetUser(entidad);
        }
        /// <summary>
        /// Connection method to get specific Bet User
        /// </summary>
        /// <param name="betUserID">BetUser ID to search</param>
        public EntityBetUser GetBetUserByID(Guid? betUserID = null)
        {
            AccessBetUser.connectionString = connectionString;
            return AccessBetUser.Read_BetUserById(betUserID);
        }
    }
}
