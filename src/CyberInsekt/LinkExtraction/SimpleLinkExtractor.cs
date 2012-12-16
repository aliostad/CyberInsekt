using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HtmlAgilityPack;

namespace CyberInsekt.LinkExtraction
{
    public class SimpleLinkExtractor : ILinkExtractor
    {
        public IEnumerable<string> GetSupportedMediaTypes()
        {
            return new string[]{"text/html"};
        }

        public Task<IEnumerable<HttpRequestMessage>> Extract(HttpResponseMessage response)
         {
             var requests = new List<HttpRequestMessage>();

             // sanity check only _______________________________
             if (!response.IsSuccessStatusCode)
                 return TaskHelpers.FromResult( (IEnumerable<HttpRequestMessage>) requests);
            
            if(response.Content==null)
                return TaskHelpers.FromResult((IEnumerable<HttpRequestMessage>)requests);

            // it assumes it is already loaded into buffer
            return response.Content.ReadAsStringAsync()
                .Then(content =>
                          {
                              Extract(response.RequestMessage.RequestUri, content, requests);
                              return (IEnumerable<HttpRequestMessage>) requests;
                          }
                );


         }

        internal void Extract(Uri requestUri,
            string content, List<HttpRequestMessage> listTobeFilled)
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);
            document.DocumentNode.Descendants()
                .Where(n => n.Name.Equals("a", StringComparison.InvariantCultureIgnoreCase))
                .ToList()
                .ForEach(node =>
                             {
                                 var attribute = node.Attributes
                                     .FirstOrDefault(n => n.Name.Equals("href", 
                                         StringComparison.InvariantCultureIgnoreCase));

                                 if(attribute == null)
                                     return;

                                 try
                                 {
                                     var u = attribute.Value.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) ?
                                         new Uri(attribute.Value) :
                                         new Uri(attribute.Value, UriKind.Relative);
                                     if(!u.IsAbsoluteUri)
                                         u = new Uri(requestUri, attribute.Value);

                                     listTobeFilled.Add(new HttpRequestMessage(HttpMethod.Get, u));
                                 }
                                 catch (Exception exception)
                                 {
                                     
                                    CrawlerRuntime.Current.TraceWriteLine(
                                        exception.ToString(), TraceLevel.Error);
                                 }

                                 

                             }
                );
        }
    }
}
