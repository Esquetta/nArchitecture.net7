using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Pipeline.Caching
{
    public class CacheRemovingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ICacheRemoverRequest
    {
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<CacheRemovingBehavior<TRequest, TResponse>> logger;

        public CacheRemovingBehavior(IDistributedCache distributedCache, ILogger<CacheRemovingBehavior<TRequest, TResponse>> logger)
        {
            this.distributedCache = distributedCache;
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response;
            if (request.BypassCache) return await next();

            async Task<TResponse> GetResponseAndRemoveCache()
            {
                response = await next();
                await distributedCache.RemoveAsync(request.CacheKey, cancellationToken);
                return response;

            }
            response = await GetResponseAndRemoveCache();
            logger.LogInformation($"Removed Cache -> {request.CacheKey}");

            return response;
        }
    }
}
