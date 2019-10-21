using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RatStore.Logic;

namespace RatStore.WebApp.Models
{
    public interface IBaseViewModel
    {
        public Customer CurrentCustomer
        {
            get;
            set;
        }

        public Location CurrentLocation
        {
            get;
            set;
        }
        public Cart Cart
        {
            get;
            set;
        }
        public bool LoggedIn
        {
            get;
            set;
        }
    }
}
