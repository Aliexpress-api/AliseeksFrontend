using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using AliseeksFE.Models.Search;
using AliseeksFE.Utility.Extensions;

namespace AliseeksFE.Injectables.Account
{
    public class SearchValue : System.Attribute
    {
        public string DisplayName { get; set; }
        public object Default { get; set; }

        public SearchValue(object def, string displayname)
        {
            Default = def;
            DisplayName = displayname;
        }

        public string Convert(object value)
        {
            if (value.GetType() == typeof(bool))
                return ((bool)value).ToYesNot();

            return value.ToString();
        }
    }

    public class SavedSearchInject
    {
        public Dictionary<string, string> NonDefaultSearchValues(SearchCriteria model)
        {
            var values = new Dictionary<string, string>();
            var propertyInfo = model.GetType().GetProperties();

            foreach(var property in propertyInfo)
            {
                var attribute = property.GetCustomAttribute<SearchValue>();
                if(attribute != null && property.GetValue(model) != null && !object.Equals(property.GetValue(model),attribute.Default))
                {
                    values.Add(attribute.DisplayName, attribute.Convert(property.GetValue(model)));
                }
            }

            return values;
        }
    }
}
