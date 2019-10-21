using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RatStore.Logic;

namespace RatStore.WebApp.Models
{
    public class SelectStoreViewModel : BaseViewModel
    {
        public SelectStoreViewModel(BaseViewModel _base) : base(_base)
        {
            
        }
        public List<Location> Locations { get; set; }
    }
}
