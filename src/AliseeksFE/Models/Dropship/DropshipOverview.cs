using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliseeksFE.Models.Dropship
{
    public class DropshipOverview
    {
        public DropshipAccount Account { get; set; }
        //public DropshipOrder[] Orders { get; set; } TODO: FUTURE
        public int ProductCount { get; set; }
        public int IntegrationCount { get; set; }
    }
}
