﻿using System;
using System.Collections.Generic;

namespace RatStore.Data.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Order = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int? PreferredStoreId { get; set; }

        public virtual Location PreferredStore { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
