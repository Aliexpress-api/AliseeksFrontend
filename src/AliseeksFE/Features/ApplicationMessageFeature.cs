using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliseeksFE.Features
{
    public enum Category
    {
        Warning, Info, Danger, Success
    }

    public class ApplicationMessageFeature
    {
        public string Message { get; set; }
        public Category Level { get; set; }
    }
}
