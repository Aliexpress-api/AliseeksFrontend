using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliseeksFE.Utility.Extensions
{
    public static class DataTypeExtensions
    {
        public static string ToYesNot(this bool value)
        {
            return (value) ? "Yes" : "No";
        }
        
        public static string Truncate(this string value, int characters)
        {
            if (value.Length > characters)
                return value.Substring(0, characters) + "...";
            else
                return value;
        }

        public static bool IsEmptyOrNull(this string value)
        {
            return value == null || value == String.Empty;
        }
    }
}
