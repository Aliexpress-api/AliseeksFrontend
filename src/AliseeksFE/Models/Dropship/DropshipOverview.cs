using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliseeksFE.Models.Dropship
{
    public class DropshipOverview
    {
        public DropshipAccount Account { get; set; }
        public DropshipOrder[] Orders { get; set; }
        public DropshipItemModel[] Items { get; set; }
    }
}
