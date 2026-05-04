using authen.common.Services;
using authen.common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authen.common.Attributes
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;

        public CacheAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 1. Lấy dịch vụ CacheService từ DI Container
            // Get Service
            var cacheConfiguration = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();


            if (!cacheConfiguration.Enable)
            {
                await next();
                return;

            }
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheResponseService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cacheResponse = await cacheService.getCacheReponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                };
                context.Result = contentResult;
                return;

            }

            var excutedResult = await next();
            if (excutedResult.Result is OkObjectResult objectResult)
            {
                await cacheService.setCacheResponseAsync(cacheKey, objectResult.Value, TimeSpan.FromSeconds(this._timeToLiveSeconds));
            }

        }

        public static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            // Logic tạo key đơn giản: dùng đường dẫn URL
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(d => d.Key))
            {
                keyBuilder.Append($"|{key}-{value}");

            }
            return keyBuilder.ToString();
            //return $"{request.Path}";
        }
    }
}
