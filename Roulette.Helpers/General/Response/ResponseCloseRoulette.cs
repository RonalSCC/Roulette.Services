using Roulette.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Helpers.General.Response
{
    public class ResponseCloseRoulette: GeneralReturn
    {
        public List<EntityBet> ListBetsFinished { get; set; }
        public int WinningNumber { get; set; }
    }
}
