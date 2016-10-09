using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseeksFE.Models.Search;

namespace AliseeksFE.Models.Dropship
{
    public class DropshipItemModel
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public SingleItemRequest Source { get; set; }
        public string ListingID { get; set; }
        public string Listing { get; set; }
        public DropshipListingRules Rules { get; set; }
        public string Image { get; set; }
    }
}
