using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace AliseeksFE.Models.Dropship
{
    public enum AccountStatus
    {
        New, Existing
    }


    public class DropshipAccount
    {
        public int ID { get; set; }
        public string Username { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public AccountStatus Status { get; set; }
        public string Subscription { get; set; }
        public DropshipAccountRules Account { get; set; } = new DropshipAccountRules();
    }

    public class DropshipAccountRules
    {
        public DropshipListingRules Default { get; set; } = DropshipListingRules.Default;
    }

    public class DropshipAccountItem
    {
        public DropshipAccount Account { get; set; }
        public DropshipItemModel Item { get; set; }
    }
}
