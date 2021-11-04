using Roulette.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Helpers.General
{
    public class ResponseGetRouletteItem : GeneralReturn
    {
        public EntityRoulette ItemRoulette { get; set; }
    }
}
