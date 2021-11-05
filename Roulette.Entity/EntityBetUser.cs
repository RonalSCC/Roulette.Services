using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette.Entity
{
    public class EntityBetUser
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public decimal Amount { get; set; }
        public bool Active { get; set; }
    }
}
