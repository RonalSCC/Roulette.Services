using Roulette.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Helpers.General.Response
{
    public class ResponseListBets: GeneralReturn
    {
        public List<EntityBet> ListBets { get; set; }
    }
}
