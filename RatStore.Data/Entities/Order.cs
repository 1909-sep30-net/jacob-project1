using System;
using System.Collections.Generic;

namespace RatStore.Data.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? LocationId { get; set; }
        public int? CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
