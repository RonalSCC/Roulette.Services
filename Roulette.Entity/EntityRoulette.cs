using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Entity
{
    public class EntityRoulette
    {
        public int RouletteID { get; set; }
        public string Name { get; set; }
        public bool Open { get; set; }
        public int? WinningNumber { get; set; }
        public DateTime OpenningDate { get; set; }
    }
}
