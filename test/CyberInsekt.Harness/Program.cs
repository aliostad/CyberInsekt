using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CyberInsekt.Harness
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(50, 50);
            var crawler = new Crawler();
            crawler.Crawl("http://google.com");
            Console.Read();
        }
    }
}
