using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Resources;
using Application.Utilities;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger, IStringLocalizer<SharedResource> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (CustomException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Debugger.Break();
                var requestName = typeof(TRequest).Name;
                _logger.LogError(ex, "Application Request: Unhandled Exception for Request {Name} {@Request}",
                    requestName, request);
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unexpected,
                    Message = _localizer["Unexpected"]
                });
            }
        }
    }
}