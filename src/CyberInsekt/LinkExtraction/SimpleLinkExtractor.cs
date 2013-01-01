using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using HtmlAgilityPack;

namespace CyberInsekt.LinkExtraction
{
    public class SimpleLinkExtractor : ILinkExtractor
    {
        public IEnumerable<string> GetSupportedMediaTypes()
        {
            return new string[]{"text/html"};
        }

        public Task<IEnumerable<Uri>> Extract(HttpResponseMessage response)
         {
             var requests = new List<Uri>();

         
            // sanity check only _______________________________
            if (!response.IsSuccessStatusCode)
                return TaskHelpers.FromResult((IEnumerable<Uri>)requests);

            if (response.Content == null)
                return TaskHelpers.FromResult((IEnumerable<Uri>)requests);

            // it assumes it is already loaded into buffer
            return response.Content.ReadAsStringAsync()
                .Then(content =>
                {
                    ExtractInternal(response.RequestMessage.RequestUri, content, requests);
                    return (IEnumerable<Uri>)requests;
                }
                );

         }

        internal void ExtractInternal(Uri requestUri,
            string content, List<Uri> listTobeFilled)
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
                                     string value = attribute.Value;
                                     value = HttpUtility.UrlDecode(value).Trim();

                                     if(value.ToLower().StartsWith("javascript") || value.ToLower().StartsWith("mailto"))
                                         return;

                                     var u = Regex.IsMatch(value, "http(s)?://") ?
                                         new Uri(value) :
                                         new Uri(value, UriKind.Relative);
                                     if(!u.IsAbsoluteUri)
                                         u = new Uri(requestUri, value);

                                     listTobeFilled.Add(u);
                                 }
                                 catch (Exception exception)
                                 {
                                     
                                    CrawlerRuntime.Current.TraceWriteLine(
                                        attribute.Value + "----" + exception.ToString(), TraceLevel.Error);
                                 }

                                 

                             }
                             
                );
        }
    }
}
