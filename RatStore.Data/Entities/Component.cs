using System;
using System.Collections.Generic;

namespace RatStore.Data.Entities
{
    public partial class Component
    {
        public Component()
        {
            Inventory = new HashSet<Inventory>();
            ProductComponent = new HashSet<ProductComponent>();
        }

        public int ComponentId { get; set; }
        public string Name { get; set; }
        public decimal? Cost { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<ProductComponent> ProductComponent { get; set; }
    }
}
