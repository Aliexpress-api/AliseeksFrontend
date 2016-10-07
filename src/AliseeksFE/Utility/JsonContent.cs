using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace AliseeksFE.Utility
{
    public class JsonContent : StringContent
    {
        public JsonContent(string content)
            :base(content)
        {
            base.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        }
    }
}
