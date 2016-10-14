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
        public static string SearchSingle(string link, string source) { return $"api/search/item?link={link}&source={source}"; }
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

        public const string DropshipAddProduct = "api/dropshipping/add";
        public const string DropshipUpdateProduct = "api/dropshipping/update";

        public static string DropshipGetProducts(int offset, int limit) { return $"api/dropshipping?offset={offset.ToString()}&limit={limit.ToString()}"; }
        public static string DropshipGetProduct(int id) { return $"api/dropshipping/{id}"; }
        public static string DropshipIntegrationGetProduct(string listing, string listingid) { return $"api/dropshipping/integrations/{listing}/{listingid}"; }

        public const string DropshipGetAccount = "api/dropshipping/account";
        public const string DropshipGetOrders = "api/dropshipping/account/orders";
        public const string DropshipCreateAccount = "api/dropshipping/account";
        public const string DropshipOverview = "api/dropshipping/overview";
        public const string DropshipOAuthShopify = "api/dropshipping/account/shopify/oauth";
        public const string DropshipIntegrations = "api/dropshipping/account/integrations";
    }
}
