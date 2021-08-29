using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;
using System.Linq;

namespace Infrastructure.HostExtensions.Filters
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ValidationExceptionFilter> _logger;

        public ValidationExceptionFilter(ILogger<ValidationExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException)
            {
                var exception = context.Exception as ValidationException;

                _logger.LogInformation(exception, "Response has a status-code of 400.");

                context.Result = new BadRequestObjectResult(new FormatedExceptionMessages
                {
                    Messages = exception.Errors.Select(el => el.PropertyName + " : " + el.ErrorMessage).ToImmutableList()
                });
                context.ExceptionHandled = true;
            }
        }
    }
}
