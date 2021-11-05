using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Entity
{
    public class EntityBet
    {
        public int BetID { get; set; }
        public int RouletteID { get; set; }
        public Guid UserID { get; set; }
        public decimal BetValue { get; set; }
        public int Number { get; set; }
        public bool Finished { get; set; }
        public bool Winner { get; set; }
    }
}
