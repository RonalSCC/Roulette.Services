using Roulette.Access;
using Roulette.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Domain
{
    public class DomainRoulette
    {
        public int RegisterRoulette(EntityRoulette entidad)
        {
            return AccessRoulette.Register_Roulette(entidad);
        }

        public EntityRoulette UpdateRoulette(EntityRoulette entidad)
        {
            return AccessRoulette.Update_Roulette(entidad);
        }

        public List<EntityRoulette> ListRoulette()
        {
            return AccessRoulette.Read_Roulettes();
        }        
        
        public EntityRoulette GetRouletteByID(int? rouletteID = null)
        {
            return AccessRoulette.Read_RouletteById(rouletteID);
        }
    }
}
