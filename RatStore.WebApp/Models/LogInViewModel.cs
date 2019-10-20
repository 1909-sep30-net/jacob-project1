using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RatStore.WebApp.Models
{
    public class LogInViewModel : BaseViewModel
    {
        public LogInViewModel(BaseViewModel _base) : base(_base)
        {

        }

        [Required(ErrorMessage = "This field is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Password { get; set; }
    }
}
