using System;
using System.Collections.Generic;

namespace RatStore.Data.Entities
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetails>();
            ProductComponent = new HashSet<ProductComponent>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public virtual ICollection<ProductComponent> ProductComponent { get; set; }
    }
}
