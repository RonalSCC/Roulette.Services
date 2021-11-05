using Roulette.Access;
using Roulette.Entity;
using Roulette.Helpers.General.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Domain
{
    public class DomainRoulette
    {
        public string connectionString = string.Empty;
        /// <summary>
        /// Connection method to register a Roulette
        /// </summary>
        /// <param name="entidad">Object EntityRoulette to insert in BD</param>
        public int RegisterRoulette(EntityRoulette entidad)
        {
            AccessRoulette.connectionString = connectionString;
            return AccessRoulette.Register_Roulette(entidad);
        }
        /// <summary>
        /// Connection method to update a Roulette
        /// </summary>
        /// <param name="entidad">Object EntityRoulette to update in BD</param>
        public EntityRoulette UpdateRoulette(EntityRoulette entidad)
        {
            AccessRoulette.connectionString = connectionString;
            return AccessRoulette.Update_Roulette(entidad);
        }
        /// <summary>
        /// Connection method to get all Bets
        /// </summary>
        public List<EntityRoulette> ListRoulette()
        {
            AccessRoulette.connectionString = connectionString;
            return AccessRoulette.Read_Roulettes();
        }
        /// <summary>
        /// Connection method to get specific Roulette
        /// </summary>
        /// <param name="rouletteID">Roulette ID to search</param>
        public EntityRoulette GetRouletteByID(int? rouletteID = null)
        {
            AccessRoulette.connectionString = connectionString;
            return AccessRoulette.Read_RouletteById(rouletteID);
        }
        /// <summary>
        /// Connection method to finish Roulette
        /// </summary>
        /// <param name="infoWinners">InputFinish_Roulette to winning bets</param>
        public bool Finish_Roulette(InputFinish_Roulette infoWinners)
        {
            AccessRoulette.connectionString = connectionString;
            return AccessRoulette.Finish_Roulette(infoWinners);
        }
    }
}
