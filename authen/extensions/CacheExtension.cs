using authen.common.Services;
using authen.common.Helpers;
using StackExchange.Redis;

namespace authen.extensions
{
    public static class CacheExtension
    {
        public static IServiceCollection addCacheService(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConfig = configuration.GetSection("RedisConfiguration").Get<RedisConfiguration>()
                ?? new RedisConfiguration();
            services.AddSingleton(redisConfig);

            if (redisConfig.Enable)
            {
                services.AddSingleton<IConnectionMultiplexer>(
                    ConnectionMultiplexer.Connect(redisConfig.ConnectionString));

                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConfig.ConnectionString;
                    options.InstanceName = "MyCache";
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
                services.AddSingleton<IConnectionMultiplexer>(sp =>
                    ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false"));
            }

            services.AddScoped<ICacheResponseService, CacheResponseService>();
            return services;
        }
    }
}
