using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace AliseeksFE.IntegrationTests.Utilities
{
    public static class QueryStringUtility
    {
        public static Dictionary<string, string> ToKeyValue(this object ob)
        {
            var obType = ob.GetType();
            var props = ob.GetType().GetProperties();
            var dictionary = new Dictionary<string, string>();

            foreach(var prop in props)
            {
                dictionary.Add(prop.Name, prop.GetValue(ob).ToString());
            }

            return dictionary;
        }
    }
}
