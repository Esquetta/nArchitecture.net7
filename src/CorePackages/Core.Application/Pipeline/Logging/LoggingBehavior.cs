using CrossCuttingConcern.Logging;
using CrossCuttingConcern.Logging.SeriLog;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Core.Application.Pipeline.Logging
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ILoggableRequest
    {
        private readonly LoggerServiceBase loggerServiceBase;
        private readonly IHttpContextAccessor httpContextAccessor;
        public LoggingBehavior(LoggerServiceBase loggerServiceBase, IHttpContextAccessor httpContextAccessor)
        {
            this.loggerServiceBase = loggerServiceBase;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            List<LogParameter> logParameters = new();

            logParameters.Add(new LogParameter
            {
                Type = request.GetType().Name,
                Value = request
            });

            LogDetail logDetail = new()
            {
                MethodName = next.Method.Name,
                Parameters = logParameters,
                User = httpContextAccessor.HttpContext == null || httpContextAccessor.HttpContext.User.Identity.Name == null
              ? "?" : httpContextAccessor.HttpContext.User.Identity.Name
            };

            loggerServiceBase.Info(JsonConvert.SerializeObject(logDetail));

            return next();
        }
    }
}
