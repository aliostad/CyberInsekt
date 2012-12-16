using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CyberInsekt.Storage
{
    public interface IUrlStore
    {
        /// <summary>
        /// Stores Uri if does not exist.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>True if it did not exist. False if existed and did not have to add</returns>
        void Store(Uri uri);
        bool Exists(Uri uri);
    }
}
