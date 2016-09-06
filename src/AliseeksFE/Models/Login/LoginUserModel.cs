using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AliseeksFE.Models.Login
{
    public class LoginUserModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 50 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 50 characters")]
        public string Password { get; set; }
    }
}
