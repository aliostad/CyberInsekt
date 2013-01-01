using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using CyberInsekt.SqlUrlStore;

namespace CyberInsekt.Harness
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(50, 50);
            var crawler = new Crawler();
            if(ConfigurationManager.ConnectionStrings["UrlStore"]!=null)
                crawler.Store = new UrlStore();
            crawler.Crawl("http://YAHOO.COM");
            Console.Read();
        }
    }
}
