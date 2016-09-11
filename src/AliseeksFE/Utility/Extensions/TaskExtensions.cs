using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliseeksFE.Utility.Extensions
{
    public static class AppTask
    {
        public static async void Forget(Action action)
        {
            await Task.Factory.StartNew(action);
        }
    }
}
