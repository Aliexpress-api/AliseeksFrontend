using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using AliseeksFE.Models.Search;

namespace AliseeksFE.Models.Binders
{
   public class MultiselectModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return null;

            if (context.Metadata.ModelType == typeof(SearchCriteria))
            {
                return new MultiSelectBinder();
            }

            return null;
        }
    }

    public class MultiSelectBinder : IModelBinder
    {
        public MultiSelectBinder()
        {

        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            var valueAsString = valueProviderResult.FirstValue;

            //bindingContext.Result = ModelBindingResult.Success("test");

            return null;
        }
    }
}
