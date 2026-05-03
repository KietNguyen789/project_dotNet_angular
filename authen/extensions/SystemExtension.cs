using authen.common.BaseClass;
using authen.system.web.Controller;

namespace authen.extensions
{
    public static class SystemExtension
    {
        public static IServiceCollection addSystemServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers()
                .AddApplicationPart(typeof(sys_userController).Assembly)
                .AddNewtonsoftJson();
            return services;
        }
    }
}
