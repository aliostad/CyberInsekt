using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace CyberInsekt.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public void TestMe()
        {
            throw new NotImplementedException();
            var crawler = new Crawler();
            crawler.Crawl("http://google.com");
            Thread.Sleep(3000);
        }
    }
}
