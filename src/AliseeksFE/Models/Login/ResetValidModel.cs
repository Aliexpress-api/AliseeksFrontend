using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliseeksFE.Models.Login
{
    public class ResetValidModel
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
