using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RatStore.Logic;

namespace RatStore.WebApp.Models
{
    public class BaseViewModel : IBaseViewModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private IDataStore _dataStore;

        public BaseViewModel([FromServices] IHttpContextAccessor httpContextAccessor, IDataStore dataStore)
        {
            _httpContextAccessor = httpContextAccessor;
            _dataStore = dataStore;
        }

        public BaseViewModel(BaseViewModel other)
        {
            _httpContextAccessor = other._httpContextAccessor;
        }

        public Customer CurrentCustomer 
        { 
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<Customer>(_session.GetString("CurrentCustomer"));
                }
                catch (Exception e)
                {
                    Customer customer = new Customer();
                    CurrentCustomer = customer;
                    return customer;
                }
            }

            set
            {
                _session.SetString("CurrentCustomer", JsonConvert.SerializeObject(value));
            }
        }

        public Location CurrentLocation
        {
            get
            {
                try
                {
                    Location location = JsonConvert.DeserializeObject<Location>(_session.GetString("CurrentLocation"));
                    location.DataStore = _dataStore;
                    return location;
                }
                catch (Exception e)
                {
                    Location location = new Location(_dataStore);
                    CurrentLocation = location;
                    return location;
                }
            }

            set
            {
                _session.SetString("CurrentLocation", JsonConvert.SerializeObject(value));
            }
        }
        public Cart Cart
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<Cart>(_session.GetString("Cart"));
                }
                catch
                {
                    Cart _cart = new Cart();
                    Cart = _cart;
                    return _cart;
                }
            }

            set
            {
                _session.SetString("Cart", JsonConvert.SerializeObject(value));
            }
        }
        public bool LoggedIn
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<bool>(_session.GetString("LoggedIn"));
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            set
            {
                _session.SetString("LoggedIn", JsonConvert.SerializeObject(value));
            }
        }
    }
}
