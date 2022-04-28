using System;
using System.Configuration;
using CacheManager.Core;
using SeadQueryCore;

namespace SeadQueryInfra
{
    public class NullCacheFactory
    {
        public ISeadQueryCache Create()
        {
            return new NullCache();
        }
    }
}
