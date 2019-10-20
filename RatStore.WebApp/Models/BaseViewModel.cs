using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RatStore.Logic;

namespace RatStore.WebApp.Models
{
    public class BaseViewModel
    {
        public Customer CurrentCustomer { get; set; }
        public Location CurrentLocation { get; set; }
        public Cart Cart { get; set; } = new Cart();
        public bool LoggedIn { get; set; }
        public BaseViewModel()
        {
            LoggedIn = false;
        }
        public BaseViewModel(BaseViewModel other)
        {
            CurrentCustomer = other.CurrentCustomer;
            LoggedIn = other.LoggedIn;
        }
    }
}
