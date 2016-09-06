using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AliseeksFE.Models.Login
{
    public class ResetUserModel
    {
        [Required(ErrorMessage = "Please enter your email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
