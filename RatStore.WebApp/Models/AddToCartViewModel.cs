using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using RatStore.Logic;

namespace RatStore.WebApp.Models
{
    public class AddToCartViewModel : BaseViewModel
    {
        public AddToCartViewModel(BaseViewModel _base) : base(_base)
        {

        }

        public Product Product { get; set; }

        [Range(0, 100)]
        [Required(ErrorMessage = "This field cannot be blank.")]
        public int? Quantity { get; set; }
    }
}
