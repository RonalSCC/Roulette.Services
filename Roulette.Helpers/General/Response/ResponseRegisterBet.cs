using Roulette.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Helpers.General
{
    public class ResponseRegisterBet : GeneralReturn
    {
        public EntityBet BetRegister { get; set; }
    }
}
