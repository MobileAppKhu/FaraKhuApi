using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Infrastructure.MiddleWare
{
    public class CustomExceptionMiddleWare
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,
            IStringLocalizer<SharedResource> localizer)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (CustomException e)
            {
                context.Response.StatusCode = (int) HttpStatusCode.NotAcceptable;
                context.Response.ContentType = "application/json; charset=utf-8";
                var newResponse = JsonConvert.SerializeObject(new
                {
                    Errors = e.Errors
                }, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                await context.Response.WriteAsync(newResponse);
            }
            catch (Exception)
            {
                Debugger.Break();
                // loggerService.Error(e);
                context.Response.StatusCode = (int) HttpStatusCode.NotAcceptable;
                context.Response.ContentType = "application/json; charset=utf-8";
                var newResponse = JsonConvert.SerializeObject(new Error
                    {
                        Message = localizer["Unexpected"],
                        ErrorType = ErrorType.Unexpected
                    },
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                await context.Response.WriteAsync(newResponse);
            }
        }
    }
}