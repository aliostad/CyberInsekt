using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CyberInsekt.LinkExtraction
{
    public class RegexLinkExtractor : ILinkExtractor
    {
        public IEnumerable<string> GetSupportedMediaTypes()
        {
            return new string[] {"text/html"};
        }

        public Task<IEnumerable<Uri>> Extract(HttpResponseMessage response)
        {
            return response.Content.ReadAsStringAsync()
                .Then(s =>
                {
                    return AnchorRegex.FindHrefs(s)
                        .Distinct()
                        .Select(ss =>
                            {
                                Uri tempUri = null;
                                if (Uri.TryCreate(ss, UriKind.Absolute, out tempUri))
                                    return tempUri;
                                else
                                {
                                    return Uri.TryCreate(response.RequestMessage.RequestUri, ss,
                                        out tempUri) ? tempUri : null;
                                }
                            })
                            .Where(u => u != null);
                });
        }
    }
}
