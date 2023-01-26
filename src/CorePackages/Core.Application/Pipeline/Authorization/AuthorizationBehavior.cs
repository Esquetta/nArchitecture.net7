using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Pipeline.Authorization
{
    public class AuthorizationBehavior<TRequest,TResponse>:IPipelineBehavior<TRequest,TResponse>
    where TRequest: IRequest<TResponse>,ISecureRequest
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public AuthorizationBehavior(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            





            TResponse response = await next();
            return response;
        }
    }
}
