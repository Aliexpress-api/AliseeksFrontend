using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseeksFE.Models.Search;

namespace AliseeksFE.Models.Account
{
    public class UserOverview
    {
        public string Username { get; set; }
        public SavedSearch[] SavedSearches { get; set; }
    }
}
