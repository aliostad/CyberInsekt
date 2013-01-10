using System;
using System.Diagnostics.Contracts;
using System.Text;
using System.Security.Cryptography;

namespace CyberInsekt.Storage
{
    public abstract class UrlStoreBase : IUrlStore
    {


        public void Store(Uri uri)
        {
            if (!uri.IsAbsoluteUri)
                throw new ArgumentException("uri must be absolute");
            Store(ComputeUriHash(uri), uri.ToString());
        }

        public bool Exists(Uri uri)
        {
            if (!uri.IsAbsoluteUri)
                throw new ArgumentException("uri must be absolute");
            return Exists(ComputeUriHash(uri), uri.ToString());
        }

        public void Enqueue(Uri uri)
        {
            if (!uri.IsAbsoluteUri)
                throw new ArgumentException("uri must be absolute");

            Enqueue(ComputeUriHash(uri), uri.ToString());
        }

        protected abstract void Store(byte[] hash, string url);
        protected abstract bool Exists(byte[] hash, string url);
        protected abstract void Enqueue(byte[] hash, string url);
        public abstract bool TryDequeue(out Uri uri);

        protected virtual byte[] ComputeUriHash(Uri uri)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                return sha1.ComputeHash(Encoding.UTF8.GetBytes(uri.ToString()));
            }
        }

        

      
    }
}
