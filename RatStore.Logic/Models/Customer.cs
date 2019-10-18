using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Logic
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int PreferredStoreId { get; set; }
    }
}
