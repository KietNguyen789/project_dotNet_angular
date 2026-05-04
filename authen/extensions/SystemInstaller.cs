using authen.system.web.Controller;

namespace authen.extensions
{
    public class SystemInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers()
                .AddApplicationPart(typeof(sys_userController).Assembly)
                .AddNewtonsoftJson();
        }
    }
}
