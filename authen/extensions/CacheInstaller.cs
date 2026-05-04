
using authen.common.Helpers;
using authen.common.Services;
using StackExchange.Redis;


namespace authen.extensions
{
    public class CacheInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisConfiguration = configuration.GetSection("RedisConfiguration").Get<RedisConfiguration>();

            services.AddSingleton(redisConfiguration);

            if (!redisConfiguration.Enable)
                return;
            services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConfiguration.ConnectionString));
            services.AddStackExchangeRedisCache(option => option.Configuration = redisConfiguration.ConnectionString);
            services.AddSingleton<ICacheResponseService, CacheResponseService>();

        }
    }
}
