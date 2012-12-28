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
        private ConcurrentQueue<HttpRequestMessage> _requestQueue = new ConcurrentQueue<HttpRequestMessage>(); 
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);
        private bool _keepRunning = true;
        private int _requestsRunning = 0;
        private int _maxRequestsRunning = 1000;

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
            _requestQueue.Enqueue(new HttpRequestMessage(HttpMethod.Get, startUrl));
            //DownloadAndProcess();

        }

        private void Start()
        {
            Task.Factory.StartNew(() =>
                                      {
                                          HttpRequestMessage req = null;
                                          while (_keepRunning)
                                          {
                                               _resetEvent.WaitOne(500);

                                               while (!_requestQueue.IsEmpty && 
                                                   _requestsRunning <= _maxRequestsRunning && 
                                                   _requestQueue.TryDequeue(out req))
                                               {
                                                   DownloadAndProcess(req)
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
                                        Parallel.ForEach(links, (link) =>
                                                                    {
                                                var lnk = link;
                                                if (!Store.Exists(link.RequestUri))
                                                    _requestQueue.Enqueue(lnk);
                                            });
            }
                                );
                        }
                        
                    }
                );


        }
    }
}
