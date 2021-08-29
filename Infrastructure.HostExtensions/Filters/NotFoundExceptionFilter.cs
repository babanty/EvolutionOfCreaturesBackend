using Infrastructure.Tools.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;
using System.Net;

namespace Infrastructure.HostExtensions.Filters
{
    public class NotFoundExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<NotFoundExceptionFilter> _logger;

        public NotFoundExceptionFilter(ILogger<NotFoundExceptionFilter> logger)
        {
            _logger = logger;
        }


        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException)
            {
                var exception = context.Exception as NotFoundException;

                _logger.LogInformation(exception, "Response has a status-code of 404.");

                var objectResult = new ObjectResult(new FormatedExceptionMessages
                {
                    Messages = new string[] { exception.Message }.ToImmutableList()
                })
                {
                    StatusCode = (int)HttpStatusCode.NotFound
                };

                context.Result = objectResult;
                context.ExceptionHandled = true;
            }
        }
    }
}
