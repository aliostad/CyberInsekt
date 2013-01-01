using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CyberInsekt.LinkExtraction;
using CyberInsekt.Storage;

namespace CyberInsekt
{
    public class Crawler
    {
        private CrawlerConfiguration _configuration;
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);
        private bool _keepRunning = true;
        private int _requestsRunning = 0;
        private int _maxRequestsRunning = 50;

        public Crawler()
        {
            LinkExtractor = new SimpleLinkExtractor().Extract;
            Requester = new HttpClient();
            Store = new InMemoryUrlStore();
            // TODO: read from config file
            Start();
        }
        
        public Crawler(CrawlerConfiguration configuration) : this()
        {
            _configuration = configuration;
        }


        public Func<HttpResponseMessage, Task<IEnumerable<Uri>>>
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
            Store.Enqueue(new Uri(startUrl));
            //DownloadAndProcess();

        }

        private void Start()
        {
            Task.Factory.StartNew(() =>
                                      {
                                          Uri uri = null;
                                          while (_keepRunning)
                                          {
                                               _resetEvent.WaitOne(500);

                                               while (
                                                   _requestsRunning <= _maxRequestsRunning && 
                                                   Store.TryDequeue(out uri))
                                               {
                                                   DownloadAndProcess(new HttpRequestMessage(HttpMethod.Get, uri))
                                                       .ContinueWith( (t) =>
                                                                          {
                                                                              if(t.IsFaulted)
                                                                                   CrawlerRuntime.Current.TraceWriteLine(
                                                                                        t.Exception.ToString(), TraceLevel.Error);
                                                                          }
                                                       );
                                                   Interlocked.Increment(ref _requestsRunning);
                                               }

                                          }

                                          
                                      });
        }

        public void Stop()
        {
            
        }

        private void DownloadAndProcessSync(HttpRequestMessage request)
        {
            CrawlerRuntime.Current.TraceWriteLine(request.RequestUri.ToString(), TraceLevel.Info);
            Store.Store(request.RequestUri);
            var response = Requester.SendAsync(request).Result;
                            
            if (response.Content == null)
                return;

            if (!response.IsSuccessStatusCode)
                return;

             response.Content.LoadIntoBufferAsync().Wait();
            var messages = LinkExtractor(response).Result;
        }

        private Task DownloadAndProcess (HttpRequestMessage request)
        {
            CrawlerRuntime.Current.TraceWriteLine(request.RequestUri.ToString(), TraceLevel.Info);
            HttpResponseMessage resp = null;
            Store.Store(request.RequestUri);
            return Requester.SendAsync(request)
                .Then((response) 
                 =>
                          {
                              Interlocked.Decrement(ref _requestsRunning);
                              if (response.Content == null)
                                  return TaskHelpers.Completed();

                              if (!response.IsSuccessStatusCode)
                                  return TaskHelpers.Completed();
                              resp = response;

                              return response.Content.LoadIntoBufferAsync();

                          }
                
                )
                
                .Then( () =>
                    {
                        if(resp!=null)
                        {
                            LinkExtractor(resp).Then(
                                (links) =>
                                    {
                                        links.Where(x=> !Store.Exists(x))
                                            .ToList()
                                            .ForEach((link) =>
                                                         {
                                                             Store.Enqueue(link);
                                                         });
                                    }
                                );
                        }
                        
                    }
                );


        }
    }
}
