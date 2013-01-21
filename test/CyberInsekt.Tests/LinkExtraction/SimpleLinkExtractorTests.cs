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

/*	    <a href="http://google.com"></a>
	    <a href="straight"></a>
	    <a href="/straight"></a>
	    <a href="../oneUp"></a>
	    <a href="../../twoUp"></a>

 */
 
            var list = new List<Uri>();
            var extractor = new HtmlAgilityLinkExtractor();
            extractor.ExtractInternal(new Uri("http://google.com/a/b/c/s"),
                              File.ReadAllText("Test.html"),
                              list);

            var urls = list.Select(x => x.ToString()).ToList();
            urls.ForEach(x=> Console.WriteLine(x));
            Assert.Contains("http://google.com/a/b/c/straight", urls);
            Assert.Contains("http://google.com/straight2", urls);
            Assert.Contains("http://google.com/a/b/oneUp", urls);
            Assert.Contains("http://google.com/a/twoUp", urls);
            Assert.Contains("http://google.com/", urls);

        }

    }
}
