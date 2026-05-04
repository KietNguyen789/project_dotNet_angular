using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace authen.common.Services
{
    public class CacheResponseService : ICacheResponseService
    {

        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public CacheResponseService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task setCacheResponseAsync(string cacheKey, object response, TimeSpan timeout)
        {
            if (response == null)
                return;
            var serializeResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            await _distributedCache.SetStringAsync(cacheKey, serializeResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeout,
            });
        }
        public async Task<string> getCacheReponseAsync(string cacheKey)
        {
            var cacheResponse = await _distributedCache.GetStringAsync(cacheKey);
            return string.IsNullOrEmpty(cacheResponse) ? null : cacheResponse;
        }
    }
}
