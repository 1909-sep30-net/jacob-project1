using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RatStore.Logic;

namespace RatStore.WebApp.Models
{
    public class OrderHistoryViewModel : BaseViewModel
    {
        public OrderHistoryViewModel(BaseViewModel _base) : base(_base)
        {

        }

        public List<Order> OrderHistory { get; set; }
    }
}
