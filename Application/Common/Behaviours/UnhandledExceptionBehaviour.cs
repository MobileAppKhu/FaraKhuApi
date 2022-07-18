using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Application.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
        
    private readonly IStringLocalizer<SharedResource> _localizer;

    public UnhandledExceptionBehaviour(IStringLocalizer<SharedResource> localizer)
    {
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
        catch (Exception)
        {
            Debugger.Break();
            throw new CustomException(new Error
            {
                ErrorType = ErrorType.Unexpected,
                Message = _localizer["Unexpected"]
            });
        }
    }
}