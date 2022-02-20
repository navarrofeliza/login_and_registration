using System;
using System.ComponentModel.DataAnnotations;

namespace LoginandReg.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email must be correct format")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email: ")]
        public string LoginEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be a minimum of 8 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password: ")]
        public string LoginPassword { get; set; }
    }
}