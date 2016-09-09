using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AliseeksFE.Models.Login
{
    public class ResetValidModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Token not valid")]
        public string Token { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 50 characters")]
        [Compare("ConfirmNewPassword", ErrorMessage = "Passwords entered do not match")]
        public string NewPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 50 characters")]
        public string ConfirmNewPassword { get; set; }
    }
}
