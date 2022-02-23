using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // this is for the [NotMapped] feature


namespace LoginandReg.Models
{
    public class User
    {
        // Don't forget the [Key]
        [Key]
        public int UserId { get; set; }
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2)]
        public string LastName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Anything below here will not be mapped into the database
        [NotMapped]
        [Compare("Password")] //capitalization does matter when comparing 
        [DataType(DataType.Password)]
        public string Confirm { get; set; }
    }

    // You can combine User classes into one Model, but make sure it's  OUTSIDE the original public class.
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
