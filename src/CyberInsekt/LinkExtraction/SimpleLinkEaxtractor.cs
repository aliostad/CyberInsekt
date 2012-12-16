using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HtmlAgilityPack;

namespace CyberInsekt.LinkExtraction
{
    public class SimpleLinkEaxtractor : ILinkExtractor
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
                              Extract(response, content, requests);
                              return (IEnumerable<HttpRequestMessage>) requests;
                          }
                );


         }

        internal void Extract(HttpResponseMessage response,
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
                                 try
                                 {

                                 }
                                 catch (Exception exception)
                                 {
                                     
                                     throw;
                                 }

                                 //response.RequestMessage.RequestUri

                             }
                );
        }
    }
}
