using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliseeksFE.Services
{
    public class ApiEndpoints
    {
        public const string Search = "api/search";
        public const string SearchCache = "api/search/cache";
        public const string Save = "api/search/save";
        public const string PriceHistory = "api/search/price";

        public const string Feedback = "api/feedback";

        public const string UserLogin = "api/user/login";
        public const string UserRegister = "api/user/register";
        public const string UserReset = "api/user/reset";
        public const string UserResetValid = "api/user/resetvalid";
        public static Func<string, string> UserAccount = (username) => { return $"api/user/{username}"; };

        public const string LoggingException = "api/logging/exception";
        public const string LoggingActivity = "api/logging/activity";
    }
}
