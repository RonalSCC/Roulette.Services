using Roulette.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Helpers.General
{
    public class ResponseListRoulettes : GeneralReturn
    {
        public List<EntityRoulette> ListRoulettes { get; set; }
    }
}
