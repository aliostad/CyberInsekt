using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CyberInsekt.SqlUrlStore.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public void Test()
        {
            var urlStore = new UrlStore();
            var uri = new Uri("http://byterot.blogspot.com");
            urlStore.Store(uri);
            Assert.True(urlStore.Exists(uri));
        }
    }
}
