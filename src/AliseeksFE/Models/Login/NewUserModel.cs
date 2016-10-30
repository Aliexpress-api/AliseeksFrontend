using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AliseeksFE.Models.Login
{
    public class NewUserModel
    {
        [Required(ErrorMessage ="Please enter your username")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 50 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 50 characters")]
        [Compare("ConfirmPassword", ErrorMessage = "Passwords entered do not match")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 50 characters")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter your primary use")]
        public string PrimaryUse { get; set; }

        public string Referral { get; set; }
    }
}
