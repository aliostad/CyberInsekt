using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CyberInsekt.Harness
{
    class Program
    {
        static void Main(string[] args)
        {
            var crawler = new Crawler();
            crawler.Crawl("http://google.com");
            Console.Read();
        }
    }
}
