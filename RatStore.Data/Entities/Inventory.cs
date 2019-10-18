using System;
using System.Collections.Generic;

namespace RatStore.Data.Entities
{
    public partial class Inventory
    {
        public int LocationId { get; set; }
        public int ComponentId { get; set; }
        public int? Quantity { get; set; }

        public virtual Component Component { get; set; }
        public virtual Location Location { get; set; }
    }
}
