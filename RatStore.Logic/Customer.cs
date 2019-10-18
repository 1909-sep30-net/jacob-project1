using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Data
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int PreferredStoreId { get; set; }
    }
}
