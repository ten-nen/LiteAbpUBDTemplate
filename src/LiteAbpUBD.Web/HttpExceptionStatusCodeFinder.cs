using Microsoft.Extensions.Options;
using System.Net;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Authorization;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Validation;

namespace LiteAbpUBD.Web
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IHttpExceptionStatusCodeFinder))]
    public class HttpExceptionStatusCodeFinder : IHttpExceptionStatusCodeFinder, ITransientDependency
    {
        protected AbpExceptionHttpStatusCodeOptions Options { get; }

        public HttpExceptionStatusCodeFinder(
            IOptions<AbpExceptionHttpStatusCodeOptions> options)
        {
            Options = options.Value;
        }
        public HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception)
        {
            if (exception is IHasHttpStatusCode exceptionWithHttpStatusCode &&
             exceptionWithHttpStatusCode.HttpStatusCode > 0)
            {
                return (HttpStatusCode)exceptionWithHttpStatusCode.HttpStatusCode;
            }

            if (exception is IHasErrorCode exceptionWithErrorCode &&
                !exceptionWithErrorCode.Code.IsNullOrWhiteSpace())
            {
                if (Options.ErrorCodeToHttpStatusCodeMappings.TryGetValue(exceptionWithErrorCode.Code, out var status))
                {
                    return status;
                }
            }

            if (exception is AbpAuthorizationException)
            {
                return httpContext.User.Identity.IsAuthenticated
                    ? HttpStatusCode.Forbidden
                    : HttpStatusCode.Unauthorized;
            }

            if (exception is AbpValidationException || exception is EntityNotFoundException || exception is IBusinessException)
            {
                return HttpStatusCode.BadRequest;
            }

            if (exception is AbpDbConcurrencyException)
            {
                return HttpStatusCode.Conflict;
            }

            if (exception is NotImplementedException)
            {
                return HttpStatusCode.NotImplemented;
            }

            return HttpStatusCode.InternalServerError;
        }
    }
}
