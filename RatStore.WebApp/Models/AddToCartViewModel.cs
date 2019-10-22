using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using RatStore.Logic;

namespace RatStore.WebApp.Models
{
    public class AddToCartViewModel : BaseViewModel, IValidatableObject
    {
        public AddToCartViewModel(BaseViewModel _base) : base(_base)
        {

        }

        public Product Product { get; set; }

        [Range(0, 100)]
        [Required(ErrorMessage = "This field cannot be blank.")]
        public int? Quantity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<OrderDetails> runningCart = Cart.OrderDetails;
            runningCart.Add(new OrderDetails { Product = this.Product, Quantity = (int)this.Quantity });

            if (!CurrentLocation.CanFulfillOrder(runningCart))
                yield return new ValidationResult("Store cannot fulfill this quantity.");
        }
    }
}
