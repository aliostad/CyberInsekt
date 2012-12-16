using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CyberInsekt.Storage
{
    public class InMemoryUrlStore : UrlStoreBase
    {

        private ConcurrentDictionary<string, string> _dictionary = new ConcurrentDictionary<string, string>();

        protected override void Store(byte[] hash, string url)
        {
            var base64String = Convert.ToBase64String(hash);
            _dictionary.AddOrUpdate(base64String,
                                           (bb) => url,
                                           (bb, u) => u);
        }

        protected override bool Exists(byte[] hash, string url)
        {
            var base64String = Convert.ToBase64String(hash);
            return _dictionary.ContainsKey(base64String);
        }
    }
}
