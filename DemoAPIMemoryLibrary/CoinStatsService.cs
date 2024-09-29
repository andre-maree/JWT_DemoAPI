using Microsoft.Extensions.Caching.Memory;
using Models;
using System.Net.Http.Json;

namespace MemLib
{
    public class CoinStatsService : ICoinStatsService
    {
        private readonly IMemoryCache _memoryCache;
        public string cacheKey = "coincache";
        private readonly HttpClient _httpClient;

        public CoinStatsService(IMemoryCache memoryCache, HttpClient httpClient)
        {
            _memoryCache = memoryCache;
            _httpClient = httpClient;
        }

        public async Task<CoinsRoot?> GetCoinStats()
        {
            CoinsRoot? coinsRoot;

            if (!_memoryCache.TryGetValue(cacheKey, out coinsRoot))
            {
                coinsRoot = await _httpClient.GetFromJsonAsync<CoinsRoot?>("coins");

                _memoryCache.Set(cacheKey, coinsRoot,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1)));
            }

            return coinsRoot;
        }
    }
}
