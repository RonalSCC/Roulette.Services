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
            return AccessRoulette.RegisterRoulette(entidad);
        }
    }
}
