using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using CacheCow.Client;
using CacheCow.Client.FileCacheStore;
using CyberInsekt.SqlUrlStore;

namespace CyberInsekt.Harness
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(200, 200);
            var crawler = new Crawler();
            var fileStore = new FileStore(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cache"));
            var cachingHandler = new CachingHandler(fileStore)
                {
                    InnerHandler = new HttpClientHandler()
                };
            
            crawler.Requester = new HttpClient(cachingHandler);
            if(ConfigurationManager.ConnectionStrings["UrlStore"]!=null)
                crawler.Store = new UrlStore();
            crawler.Crawl("http://YAHOO.COM");
            Console.Read();
        }
    }
}
