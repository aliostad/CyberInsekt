using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CyberInsekt
{
    public class CrawlerRuntime
    {
        private static readonly CrawlerRuntime _current = new CrawlerRuntime();
        private CrawlerRuntime()
        {
            TraceWriteLine = (s, tl) => Trace.WriteLine(s);
        }


        public static CrawlerRuntime Current
        {
            get { return _current; }
        }

        public Action<string, TraceLevel> TraceWriteLine { get; set; }
    }
}
