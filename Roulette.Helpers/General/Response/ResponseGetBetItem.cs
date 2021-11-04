using Roulette.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Helpers.General.Response
{
    public class ResponseGetBetItem : GeneralReturn
    {
        public EntityBet ItemBet { get; set; }
    }
}
