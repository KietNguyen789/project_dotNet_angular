namespace authen.extensions
{
    public static class CacheExtension
    {
        public static IServiceCollection addCacheService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("MyRedisConStr");
                options.InstanceName = "MyCache";
            });
            return services;
        }
    }
}
