using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliseeksFE.Utility.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class QueryStringIgnore : System.Attribute
    {
        public QueryStringIgnore()
        {

        }
    }
}
