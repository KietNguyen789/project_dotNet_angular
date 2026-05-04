namespace authen.extensions
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration config);
    }
}
