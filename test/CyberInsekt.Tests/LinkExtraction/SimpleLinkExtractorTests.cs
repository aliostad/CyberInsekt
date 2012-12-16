using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using CyberInsekt.LinkExtraction;
using Xunit;

namespace CyberInsekt.Tests.LinkExtraction
{
    public class SimpleLinkExtractorTests
    {
        [Fact]
        public void ExtractTest()
        {
            var list = new List<HttpRequestMessage>();
            var extractor = new SimpleLinkExtractor();
            extractor.Extract(new Uri("http://google.com/a/b/c/"),
                              File.ReadAllText("Test.html"),
                              list);

            var urls = list.Select(x => x.RequestUri.ToString()).ToList();
            Assert.Contains("http://google.com/a/b/c/straight", urls);
            Assert.Contains("http://google.com/a/b/oneUp", urls);
            Assert.Contains("http://google.com/a/twoUp", urls);
            Assert.Contains("http://google.com/", urls);

        }

    }
}
