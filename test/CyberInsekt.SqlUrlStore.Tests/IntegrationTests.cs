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
        public void StoreTest()
        {
            var urlStore = new UrlStore();
            var uri = new Uri("http://byterot.blogspot.com");
            urlStore.Store(uri);
            Assert.True(urlStore.Exists(uri));
        }

        [Fact]
        public void QueueTest()
        {
            var urlStore = new UrlStore();
            var uri = new Uri("http://byterot.blogspot.com");
            urlStore.Enqueue(uri);
            Uri uri2 = null;
            Assert.True(urlStore.TryDequeue(out uri2));
            Assert.NotNull(uri2);
            Assert.Equal(uri.ToString(), uri2.ToString());
        }

    }
}
