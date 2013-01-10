using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace CyberInsekt.Storage
{
    public class InMemoryUrlStore : UrlStoreBase
    {

        private ConcurrentDictionary<string, string> _dictionaryForCrawled = new ConcurrentDictionary<string, string>();
        private ConcurrentDictionary<string, string> _dictionaryForQueue = new ConcurrentDictionary<string, string>();

        private ConcurrentQueue<Uri> _uriQueue = new ConcurrentQueue<Uri>(); 

        protected override void Store(byte[] hash, string url)
        {
            var base64String = Convert.ToBase64String(hash);
            _dictionaryForCrawled.AddOrUpdate(base64String,
                                           (bb) => url,
                                           (bb, u) => u);

        }

        protected override bool Exists(byte[] hash, string url)
        {
            var base64String = Convert.ToBase64String(hash);
            return _dictionaryForCrawled.ContainsKey(base64String) ||
                _dictionaryForQueue.ContainsKey(base64String);
        }

        protected override void Enqueue(byte[] hash, string url)
        {
            var base64String = Convert.ToBase64String(hash);

            if (!_dictionaryForCrawled.ContainsKey(base64String) &&
                !_dictionaryForQueue.ContainsKey(base64String)
                )
            {

                _dictionaryForCrawled.AddOrUpdate(base64String,
                                               (bb) => url,
                                               (bb, u) => u);

            }
        }

        public override bool TryDequeue(out Uri uri)
        {
            string value = null;
            var was = _uriQueue.TryDequeue(out uri);
            if (was)
                _dictionaryForQueue.TryRemove(Convert.ToBase64String(ComputeUriHash(uri)), out value);
            return was;
        }
    }
}
