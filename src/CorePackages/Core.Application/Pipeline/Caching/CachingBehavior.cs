using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Core.Application.Pipeline.Caching
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ICachableRequest
    {
        private readonly IDistributedCache cache;
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> logger;

        private readonly CacheSettings cacheSettings;
        public CachingBehavior(IDistributedCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger, CacheSettings cacheSettings)
        {
            this.cache = cache;
            this.logger = logger;
            this.cacheSettings = cacheSettings;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response;
            if (request.BypassCache) return await next();

            async Task<TResponse> GetResponseAndAddToCache()
            {
                response = await next();
                TimeSpan? slidingExpiration = request.SlidingExpiration ?? TimeSpan.FromDays(cacheSettings.SlidingExpiration);
                DistributedCacheEntryOptions cacheOptions = new() { SlidingExpiration = slidingExpiration };
                byte[] seriliazeData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
                await cache.SetAsync(request.CacheKey, seriliazeData, cacheOptions,cancellationToken);
                return response;
            }

            byte[]? cachedResponse = await cache.GetAsync(request.CacheKey, cancellationToken);
            if (cachedResponse != null)
            {
                response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));
                logger.LogInformation($"Fetched from Cache -> {request.CacheKey}");
            }
            else
            {
                response = await GetResponseAndAddToCache();
                logger.LogInformation($"Added to Cache -> {request.CacheKey}");
            }
            return response;
        }
    }
}
