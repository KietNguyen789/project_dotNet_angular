using authen.common.data.Models;
using authen.Database.Mongodb.Collection;
using authen.common.Helpers;
namespace authen.extensions
{
    public static class addMongoDBExtension
    {
        public static IServiceCollection addMongoDBService(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

            services.AddSingleton<IMongoClientFactory, MongoClientFactory>();

            services.AddScoped<MongoDBContext>(s =>
            {
                var factory = s.GetRequiredService<IMongoClientFactory>();
                var httpContext = s.GetService<IHttpContextAccessor>()?.HttpContext;
                var host = httpContext?.Request.Host.Host;
                if (string.IsNullOrEmpty(host)) host = "localhost";

                var databaseName = host.Contains("localhost")
                    ? appSettings.mongodb_database
                    : appSettings.default_database;

                return factory.GetClientDatabase(databaseName);
            });

            return services;
        }
    }
}
