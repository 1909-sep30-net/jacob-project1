using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Data
{
    public class Order
    {
        public Order()
        {
            OrderDate = DateTime.Now;
        }
        public int LocationId { get; set; }
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        virtual public List<OrderDetails> OrderDetails { get; set; }

        /// <summary>
        /// Adds up the cost of the Products referenced in the OrderDetails.
        /// </summary>
        public decimal Cost 
        { 
            get
            {
                decimal sum = 0;
                foreach (OrderDetails orderDetails in OrderDetails)
                {
                    sum += orderDetails.Product.Cost * orderDetails.Quantity;
                }

                return sum;
            }
        }

        public int Count
        {
            get
            {
                int count = 0;
                foreach (OrderDetails orderDetails in OrderDetails)
                {
                    count += orderDetails.Quantity;
                }

                return count;
            }
        }
    }
}
