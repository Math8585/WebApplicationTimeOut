using Microsoft.Extensions.Caching.Memory;

namespace WebApplication3.Services
{
    public class CacheService
    {
    
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void WriteResultToCache(string cacheKey, int result, int delay   )
        {
            _memoryCache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
        }

        public bool TryGetValue(string cacheKey, out int result)
        {
            return _memoryCache.TryGetValue(cacheKey, out result);
        }

    }
}
