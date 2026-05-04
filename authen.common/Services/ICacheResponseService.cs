namespace authen.common.Services
{
    public interface ICacheResponseService
    {
        Task setCacheResponseAsync(string cacheKey, object response, TimeSpan timeout);
        Task<string> getCacheReponseAsync(string cacheKey);
    }
}
