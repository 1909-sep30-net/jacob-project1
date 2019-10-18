using System;
using System.Collections.Generic;

namespace RatStore.Data.Entities
{
    public partial class Location
    {
        public Location()
        {
            Inventory = new HashSet<Inventory>();
            Order = new HashSet<Order>();
        }

        public int LocationId { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
