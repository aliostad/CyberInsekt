using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CyberInsekt.Storage;
using Xunit;

namespace CyberInsekt.Tests.Storage
{
    public class InMemoryUrlStoreTests
    {
        [Fact]
        public void DictionaryIsReferenceIndependent()
        {
            var store = new InMemoryUrlStore();
            var uri = new Uri("http://localhost/ali");
            var uri2 = new Uri("http://localhost/ali");

            store.Store(uri);
            Assert.True(store.Exists(uri2));
            store.Store(uri2);
            store.Store(uri);

        }

    }
}
