using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using AliseeksFE.Utility.Attributes;
using System.Windows;

namespace AliseeksFE.Utility
{
    public class QueryStringEncoder
    {
        public string Encode(object ob)
        {
            var props = ob.GetType().GetProperties();
            var keyvalue = new List<string>();

            foreach(PropertyInfo prop in props)
            {
                var ignore = prop.GetCustomAttribute<QueryStringIgnore>();
                var val = prop.GetValue(ob);
                var name = prop.Name[0].ToString().ToLower() + prop.Name.Substring(1, prop.Name.Length - 1);

                if(val != null && ignore == null)
                {
                    keyvalue.Add($"{name}={val.ToString()}");
                }
            }

            string qs = string.Join("&", keyvalue);
            return Uri.EscapeUriString(qs);
        }
    }
}
