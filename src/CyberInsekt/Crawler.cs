using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CyberInsekt.LinkExtraction;
using CyberInsekt.Storage;

namespace CyberInsekt
{
    public class Crawler
    {
        private CrawlerConfiguration _configuration;
        

        public Crawler()
        {
            LinkExtractor = new SimpleLinkExtractor().Extract;
            Requester = new HttpClient();
            Store = new InMemoryUrlStore();
            // TODO: read from config file

        }
        
        public Crawler(CrawlerConfiguration configuration) : this()
        {
            _configuration = configuration;
        }


        public Func<HttpResponseMessage, Task<IEnumerable<HttpRequestMessage>>>
            LinkExtractor { get; set; }

        public HttpClient Requester { get; set; }

        public IUrlStore Store { get; set; }
        
        public void Crawl()
        {
            // TODO:
            throw new NotImplementedException();
        }

        public void Crawl(string startUrl)
        {
            DownloadAndProcess(new HttpRequestMessage(HttpMethod.Get, startUrl));
        }

        public void Stop()
        {
            
        }

        private Task DownloadAndProcess(HttpRequestMessage request)
        {
            CrawlerRuntime.Current.TraceWriteLine(request.RequestUri.ToString(), TraceLevel.Info);
            HttpResponseMessage resp = null;
            Store.Store(request.RequestUri);
            return Requester.SendAsync(request)
                .Then((response) 
                 =>
                          {
                              if (response.Content == null)
                                  return TaskHelpers.Completed();

                              if (!response.IsSuccessStatusCode)
                                  TaskHelpers.Completed();
                              resp = response;

                              return response.Content.LoadIntoBufferAsync();

                          }
                
                ).Then( () =>
                    {
                        if(resp!=null)
                        {
                            LinkExtractor(resp).Then(
                                (links) =>
                                    {
                                        foreach (var link in links)
                                        {
                                            if (!Store.Exists(link.RequestUri))
                                                DownloadAndProcess(link);
                                        }
                                    }
                                );
                        }
                        
                    }
                )
                
                ;
        }
    }
}
