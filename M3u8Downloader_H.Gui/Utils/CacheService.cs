using M3u8Downloader_H.Abstractions.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace M3u8Downloader_H.Gui.Utils
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());

        public T? GetOrCreate<T>(object key, Func<ICacheEntry, T> factory)
            => memoryCache.GetOrCreate<T>(key, factory);    

        public Task<T?> GetOrCreateAsync<T>(object key, Func<ICacheEntry, Task<T>> factory)
            => memoryCache.GetOrCreateAsync<T>(key, factory);
    }
}
