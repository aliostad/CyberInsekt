using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace CyberInsekt
{
    public class Crawler
    {
        public Crawler()
        {
            
        }

        public Func<HttpResponseMessage, IEnumerable<HttpRequestMessage>>
            LinkExtractor { get; set; }



    }
}
