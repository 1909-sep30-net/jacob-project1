using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RatStore.WebApp.Models
{
    public class CreateCustomerViewModel : BaseViewModel
    {
        public CreateCustomerViewModel(BaseViewModel _base) : base(_base)
        {

        }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(24, MinimumLength = 6, ErrorMessage = "Username must be between 6 and 24 characters.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(24, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 24 characters.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string PhoneNumber { get; set; }
    }
}
